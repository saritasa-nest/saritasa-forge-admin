using System.ComponentModel;
using System.Linq.Expressions;
using AutoMapper;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.DTOs;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.UseCases.Metadata.Services;
using Saritasa.Tools.Common.Pagination;
using Saritasa.Tools.Common.Utils;
using Saritasa.Tools.Domain.Exceptions;

namespace Saritasa.NetForge.UseCases.Services;

/// <inheritdoc />
public class EntityService : IEntityService
{
    private readonly IMapper mapper;
    private readonly AdminMetadataService adminMetadataService;
    private readonly IOrmDataService dataService;
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Constructor for <see cref="EntityService"/>.
    /// </summary>
    public EntityService(
        IMapper mapper,
        AdminMetadataService adminMetadataService,
        IOrmDataService dataService,
        IServiceProvider serviceProvider)
    {
        this.mapper = mapper;
        this.adminMetadataService = adminMetadataService;
        this.dataService = dataService;
        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public Task<IEnumerable<EntityMetadataDto>> SearchEntitiesAsync(CancellationToken cancellationToken)
    {
        var metadata = adminMetadataService.GetMetadata()
            .Where(entityMetadata => !entityMetadata.IsHidden);
        return Task.FromResult(mapper.Map<IEnumerable<EntityMetadataDto>>(metadata));
    }

    /// <inheritdoc />
    public Task<GetEntityByIdDto> GetEntityByIdAsync(string stringId, CancellationToken cancellationToken)
    {
        var metadata = adminMetadataService
            .GetMetadata()
            .FirstOrDefault(entityMetadata => entityMetadata.StringId.Equals(stringId, StringComparison.OrdinalIgnoreCase));

        if (metadata is null)
        {
            throw new NotFoundException("Metadata for entity was not found.");
        }

        var displayableProperties = metadata.Properties
            .Where(property => property is { IsForeignKey: false, IsExcludedFromQuery: false });

        var propertyDtos = mapper
            .Map<IEnumerable<PropertyMetadata>, IEnumerable<PropertyMetadataDto>>(displayableProperties);

        var displayableNavigations = metadata.Navigations
            .Where(navigation => navigation is { IsExcludedFromQuery: false, IsIncluded: true });

        var navigationDtos = mapper
            .Map<IEnumerable<NavigationMetadata>, IEnumerable<NavigationMetadataDto>>(displayableNavigations);

        propertyDtos = propertyDtos.Union(navigationDtos);

        var orderedProperties = propertyDtos
            .OrderByDescending(property => property is { IsPrimaryKey: true, Order: null })
            .ThenByDescending(property => property.Order.HasValue)
            .ThenBy(property => property.Order)
            .ToList();

        var metadataDto = mapper.Map<GetEntityByIdDto>(metadata) with { Properties = orderedProperties };

        return Task.FromResult(metadataDto);
    }

    /// <inheritdoc />
    public Task<PagedListMetadataDto<object>> SearchDataForEntityAsync(
        Type? entityType,
        ICollection<PropertyMetadataDto> properties,
        SearchOptions searchOptions,
        Func<IServiceProvider?, IQueryable<object>, string, IQueryable<object>>? searchFunction,
        Func<IServiceProvider?, IQueryable<object>, IQueryable<object>>? customQueryFunction)
    {
        if (entityType is null)
        {
            throw new NotFoundException("Entity with given type was not found.");
        }

        var query = dataService.GetQuery(entityType);

        query = SelectProperties(query, entityType, properties.Where(property => !property.IsCalculatedProperty));

        query = ApplyCustomQuery(query, customQueryFunction);

        query = Search(query, searchOptions.SearchString, entityType, properties, searchFunction);

        if (searchOptions.OrderBy is not null)
        {
            query = Order(query, searchOptions.OrderBy.ToList(), entityType);
        }

        var pagedList = PagedListFactory.FromSource(query, searchOptions.Page, searchOptions.PageSize);

        return Task.FromResult(pagedList.ToMetadataObject());
    }

    /// <summary>
    /// Select only those properties from entity that exists in <paramref name="properties"/>.
    /// </summary>
    /// <param name="query">
    /// Query that contain data for some entity. For example, all data of <c>Address</c> entity.
    /// </param>
    /// <param name="entityType">Entity type.</param>
    /// <param name="properties">Entity properties to select.</param>
    /// <returns>Query with selected data.</returns>
    /// <remarks>
    /// Reflection can't be translated to SQL, so we have to build expression dynamically.
    /// </remarks>
    private static IQueryable<object> SelectProperties(
        IQueryable<object> query, Type entityType, IEnumerable<PropertyMetadataDto> properties)
    {
        // entity => entity
        var entity = Expression.Parameter(typeof(object), "entity");

        // entity => (entityType)entity
        var convertedEntity = Expression.Convert(entity, entityType);

        var bindings = properties
            .Select(property => Expression.Property(convertedEntity, property.Name))
            .Select(member => Expression.Bind(member.Member, member));

        var ctor = entityType.GetConstructors()[0];

        // entity => new entityType
        // { PropertyName1 = ((entityType)entity).PropertyName1, PropertyName2 = ((entityType)entity).PropertyName2 ...  }
        var memberInit = Expression.MemberInit(Expression.New(ctor), bindings);

        var selectLambda = Expression.Lambda<Func<object, object>>(memberInit, entity);

        return query.Select(selectLambda);
    }

    private IQueryable<object> Search(
        IQueryable<object> query,
        string? searchString,
        Type entityType,
        ICollection<PropertyMetadataDto> properties,
        Func<IServiceProvider?, IQueryable<object>, string, IQueryable<object>>? searchFunction)
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            foreach (var property in properties)
            {
                if (property is NavigationMetadataDto navigation
                    && navigation.TargetEntityProperties.Any(targetProperty => targetProperty.SearchType != SearchType.None))
                {
                    var propertyNamesWithSearchType = navigation.TargetEntityProperties
                        .Select(targetProperty => (targetProperty.Name, targetProperty.SearchType));

                    query = dataService
                        .Search(query, searchString, entityType, propertyNamesWithSearchType, navigation.Name);
                }
            }

            if (properties.Any(property => property.SearchType != SearchType.None))
            {
                var propertyNamesWithSearchType = properties.Select(property => (property.Name, property.SearchType));

                query = dataService.Search(query, searchString, entityType, propertyNamesWithSearchType);
            }

            if (searchFunction is not null)
            {
                query = searchFunction(serviceProvider, query, searchString);
            }
        }

        return query;
    }

    private static IOrderedQueryable<object> Order(
        IQueryable<object> query, IList<OrderByDto> orderBy, Type entityType)
    {
        var orderByTuples = orderBy
            .Select(order =>
                (order.FieldName, order.IsDescending ? ListSortDirection.Descending : ListSortDirection.Ascending))
            .ToArray();

        var keySelectors = GetKeySelectors(orderBy, entityType);

        return CollectionUtils.OrderMultiple(query, orderByTuples, keySelectors);
    }

    private static (string FieldName, Expression<Func<object, object>> Selector)[] GetKeySelectors(
        IList<OrderByDto> orderByFields, Type entityType)
    {
        // entity
        var entity = Expression.Parameter(typeof(object), "entity");

        // (entityType)entity
        var convertedEntity = Expression.Convert(entity, entityType);

        // Collection of sorted properties. For example:
        // ((entityType)entity).Name
        // ((entityType)entity).Description
        // ((entityType)entity).Count
        // ((entityType)entity).Address.Street
        // ...
        // Note that there are converting property to object.
        // We need it to sort types that are not string. For example, numbers.
        var propertyExpressions = orderByFields
            .Select(field =>
            {
                Expression propertyExpression;
                if (field.NavigationName is not null)
                {
                    var navigationExpression = Expression.Property(convertedEntity, field.NavigationName);

                    // ((entityType)entity).NavigationName.FieldName
                    propertyExpression = Expression.Property(navigationExpression, field.FieldName);
                }
                else
                {
                    // ((entityType)entity).FieldName
                    propertyExpression = Expression.Property(convertedEntity, field.FieldName);
                }

                return Expression.Convert(propertyExpression, typeof(object));
            });

        // Make lambdas with properties. For example:
        // entity => ((entityType)entity).Name
        // entity => ((entityType)entity).Description
        // entity => ((entityType)entity).Count
        // entity => ((entityType)entity).Address.Street
        // ...
        var lambdas = propertyExpressions
            .Select(property => Expression.Lambda<Func<object, object>>(property, entity))
            .ToList();

        // Make tuple with selected property names and property lambdas. For example:
        // ("name", entity => ((entityType)entity).Name)
        // ("description", entity => ((entityType)entity).Description)
        // ("count", entity => ((entityType)entity).Count)
        // ("street", entity => ((entityType)entity).Address.Street)
        // ...
        var keySelectors = new (string, Expression<Func<object, object>>)[orderByFields.Count];
        for (var i = 0; i < orderByFields.Count; i++)
        {
            keySelectors[i] = (orderByFields[i].FieldName, lambdas[i]);
        }

        return keySelectors;
    }

    private IQueryable<object> ApplyCustomQuery(
        IQueryable<object> query,
        Func<IServiceProvider?, IQueryable<object>, IQueryable<object>>? customQueryFunction)
    {
        if (customQueryFunction is not null)
        {
            query = customQueryFunction(serviceProvider, query);
        }

        return query;
    }

    /// <inheritdoc />
    public async Task CreateEntityAsync(object entity, Type entityType, CancellationToken cancellationToken)
    {
        await dataService.AddAsync(entity, entityType, cancellationToken);
    }
}
