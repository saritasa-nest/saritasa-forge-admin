using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.DTOs;

namespace Saritasa.NetForge.UseCases.Metadata.SearchEntities;

/// <summary>
/// Handler for <see cref="SearchEntitiesQuery"/>.
/// </summary>
internal class SearchEntitiesQueryHandler : IRequestHandler<SearchEntitiesQuery, IEnumerable<EntityMetadataDto>>
{
    private readonly IOrmMetadataService ormMetadataService;
    private readonly IMapper mapper;
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Constructor.
    /// </summary>
    public SearchEntitiesQueryHandler(IOrmMetadataService ormMetadataService, IMapper mapper,
        IServiceProvider serviceProvider)
    {
        this.ormMetadataService = ormMetadataService;
        this.mapper = mapper;
        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public Task<IEnumerable<EntityMetadataDto>> Handle(SearchEntitiesQuery request,
        CancellationToken cancellationToken)
    {
        var entityMetadatas = ormMetadataService.GetEntities();
        var adminOptions = (AdminOptions)serviceProvider.GetRequiredService(typeof(AdminOptions));

        foreach (var entityMetadata in entityMetadatas)
        {
            ApplyEntityOptions(entityMetadata, adminOptions);
        }

        return Task.FromResult(mapper.Map<IEnumerable<EntityMetadataDto>>(entityMetadatas));
    }

    /// <summary>
    /// Applies entity-specific options to the given <see cref="EntityMetadata"/> using the provided options.>.
    /// </summary>
    /// <param name="entityMetadata">The metadata of the entity to which options are applied.</param>
    /// <param name="adminOptions">The admin options containing entity-specific settings.</param>
    private static void ApplyEntityOptions(EntityMetadata entityMetadata, AdminOptions adminOptions)
    {
        var entityOptions =
            adminOptions.EntityOptionsList.FirstOrDefault(options => options.EntityType == entityMetadata.ClrType);

        if (entityOptions == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(entityOptions.Description))
        {
            entityMetadata.Description = entityOptions.Description;
        }
    }
}
