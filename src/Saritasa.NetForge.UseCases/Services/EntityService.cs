using System.Linq.Expressions;
using AutoMapper;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.DomainServices.Extensions;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.DTOs;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.UseCases.Metadata.Services;
using Saritasa.Tools.Common.Pagination;
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
            .First(entityMetadata => entityMetadata.StringId.Equals(stringId));

        var metadataDto = mapper.Map<GetEntityByIdDto>(metadata);

        var displayableProperties = metadata.Properties
            .Where(property => property is { IsForeignKey: false, IsHidden: false });

        var displayablePropertyDtos = mapper
            .Map<IEnumerable<PropertyMetadata>, IEnumerable<PropertyMetadataDto>>(displayableProperties);

        var displayableNavigations = mapper
            .Map<IEnumerable<NavigationMetadata>, IEnumerable<PropertyMetadataDto>>(metadata.Navigations);

        var allProperties = displayablePropertyDtos.Union(displayableNavigations);

        var orderedProperties = allProperties
            .OrderByDescending(property => property is { Name: "Id", Order: null })
            .ThenByDescending(property => property.Order.HasValue)
            .ThenBy(property => property.Order)
            .ToList();

        metadataDto = metadataDto with { Properties = orderedProperties };

        return Task.FromResult(metadataDto);
    }

    /// <inheritdoc />
    public Task<PagedListMetadataDto<object>> SearchDataForEntityAsync(
        Type? entityType,
        ICollection<PropertyMetadataDto> properties,
        SearchOptions searchOptions,
        Func<IServiceProvider?, IQueryable<object>, string, IQueryable<object>>? searchFunction)
    {
        if (entityType is null)
        {
            throw new NotFoundException("Entity with given type was not found.");
        }

        var query = dataService.GetQuery(entityType);

        query = SelectProperties(query, entityType, properties.Where(property => !property.IsCalculatedProperty));

        query = Search(query, searchOptions.SearchString, entityType, properties, searchFunction);

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
}
