using System.ComponentModel;
using System.Linq.Expressions;
using AutoMapper;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.DomainServices.Extensions;
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

    /// <summary>
    /// Constructor for <see cref="EntityService"/>.
    /// </summary>
    public EntityService(IMapper mapper, AdminMetadataService adminMetadataService, IOrmDataService dataService)
    {
        this.mapper = mapper;
        this.adminMetadataService = adminMetadataService;
        this.dataService = dataService;
    }

    /// <inheritdoc />
    public Task<IEnumerable<EntityMetadataDto>> SearchEntitiesAsync(CancellationToken cancellationToken)
    {
        var metadata = adminMetadataService.GetMetadata()
            .Where(entityMetadata => !entityMetadata.IsHidden);
        return Task.FromResult(mapper.Map<IEnumerable<EntityMetadataDto>>(metadata));
    }

    /// <inheritdoc />
    public Task<GetEntityByIdDto> GetEntityByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var metadata = adminMetadataService
            .GetMetadata()
            .First(entityMetadata => entityMetadata.Id == id);

        var metadataDto = mapper.Map<GetEntityByIdDto>(metadata);

        var displayableProperties = metadataDto.Properties
            .Where(property => property is { IsForeignKey: false, IsHidden: false });

        var orderedProperties = displayableProperties
            .OrderByDescending(property => property is { Name: "Id", Order: null })
            .ThenByDescending(property => property.Order.HasValue)
            .ThenBy(property => property.Order)
            .ToList();

        metadataDto = metadataDto with { Properties = orderedProperties };

        return Task.FromResult(metadataDto);
    }

    /// <inheritdoc />
    public Task<PagedListMetadataDto<object>> SearchDataForEntityAsync(Type? entityType,
        ICollection<PropertyMetadata> properties, SearchOptions searchOptions)
    {
        if (entityType is null)
        {
            throw new NotFoundException("Entity with given type was not found.");
        }

        var query = dataService.GetQuery(entityType);

        query = query.SelectProperties(entityType, properties);

        query = dataService.Search(query, searchOptions.SearchString, entityType, properties);

        if (!string.IsNullOrEmpty(searchOptions.OrderBy))
        {
            query = Order(query, searchOptions.OrderBy, entityType);
        }

        var pagedList = PagedListFactory.FromSource(query, searchOptions.Page, searchOptions.PageSize);

        return Task.FromResult(pagedList.ToMetadataObject());
    }

    private static IOrderedQueryable<object> Order(IQueryable<object> query, string orderBy, Type entityType)
    {
        var separatedOrderBy = OrderParsingDelegates.ParseSeparated(orderBy);
        var keySelectors = GetKeySelectors(separatedOrderBy, entityType);

        return CollectionUtils.OrderMultiple(query, separatedOrderBy, keySelectors);
    }

    private static (string FieldName, Expression<Func<object, object>> Selector)[] GetKeySelectors(
        (string FieldName, ListSortDirection Order)[] orderBy, Type entityType)
    {
        // entity
        var entity = Expression.Parameter(typeof(object), "entity");

        // (entityType)entity
        var convertedEntity = Expression.Convert(entity, entityType);

        // Collection of sorted properties. For example:
        // ((entityType)entity).Name
        // ((entityType)entity).Description
        // ((entityType)entity).Count
        // ...
        // Note that there are converting property to object.
        // We need it to sort types that are not string. For example, numbers.
        var propertyExpressions = orderBy
            .Select(order => Expression.Convert(Expression.Property(convertedEntity, order.FieldName), typeof(object)));

        // Make lambdas with properties. For example:
        // entity => ((entityType)entity).Name
        // entity => ((entityType)entity).Description
        // entity => ((entityType)entity).Count
        // ...
        var lambdas = propertyExpressions
            .Select(property => Expression.Lambda<Func<object, object>>(property, entity))
            .ToList();

        // Make tuple with selected property names and property lambdas. For example:
        // ("name", entity => ((entityType)entity).Name)
        // ("description", entity => ((entityType)entity).Description)
        // ("count", entity => ((entityType)entity).Count)
        // ...
        var keySelectors = new (string, Expression<Func<object, object>>)[orderBy.Length];
        for (var i = 0; i < orderBy.Length; i++)
        {
            keySelectors[i] = (orderBy[i].FieldName, lambdas[i]);
        }

        return keySelectors;
    }
}
