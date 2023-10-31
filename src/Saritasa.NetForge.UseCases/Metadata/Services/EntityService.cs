using AutoMapper;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.DomainServices.Extensions;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.DTOs;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;
using Saritasa.Tools.Common.Pagination;
using Saritasa.Tools.Domain.Exceptions;

namespace Saritasa.NetForge.UseCases.Metadata.Services;

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

        var pagedList = PagedListFactory.FromSource(query, searchOptions.Page, searchOptions.PageSize);

        return Task.FromResult(pagedList.ToMetadataObject());
    }
}
