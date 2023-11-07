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
    public Task<PagedListMetadataDto<object>> SearchDataForEntityAsync(
        Type? entityType,
        ICollection<PropertyMetadata> properties,
        SearchOptions searchOptions,
        Func<IServiceProvider?, IQueryable<object>, string, IQueryable<object>>? searchFunction)
    {
        if (entityType is null)
        {
            throw new NotFoundException("Entity with given type was not found.");
        }

        var query = dataService.GetQuery(entityType);

        query = query.SelectProperties(entityType, properties);

        query = Search(query, searchOptions.SearchString, entityType, properties, searchFunction);

        var pagedList = PagedListFactory.FromSource(query, searchOptions.Page, searchOptions.PageSize);

        return Task.FromResult(pagedList.ToMetadataObject());
    }

    private IQueryable<object> Search(
        IQueryable<object> query,
        string? searchString,
        Type entityType,
        ICollection<PropertyMetadata> properties,
        Func<IServiceProvider?, IQueryable<object>, string, IQueryable<object>>? searchFunction)
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            if (properties.Any(property => property.SearchType != SearchType.None))
            {
                query = dataService.Search(query, searchString, entityType, properties);
            }

            if (searchFunction is not null)
            {
                query = searchFunction(serviceProvider, query, searchString);
            }
        }

        return query;
    }
}
