using Microsoft.Extensions.Caching.Memory;
using PluralizeService.Core;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.DomainServices.Extensions;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

namespace Saritasa.NetForge.UseCases.Metadata.Services;

/// <summary>
/// Provides methods for retrieving entities metadata.
/// </summary>
public class AdminMetadataService
{
    private const string MetadataCache = "MetadataCache";
    private readonly IOrmMetadataService ormMetadataService;
    private readonly AdminOptions adminOptions;
    private readonly IMemoryCache memoryCache;

    /// <summary>
    /// Constructor.
    /// </summary>>
    public AdminMetadataService(IOrmMetadataService ormMetadataService, AdminOptions adminOptions,
        IMemoryCache memoryCache)
    {
        this.ormMetadataService = ormMetadataService;
        this.adminOptions = adminOptions;
        this.memoryCache = memoryCache;
    }

    /// <summary>
    /// Get the entities metadata.
    /// </summary>
    public IEnumerable<EntityMetadata> GetMetadata()
    {
        // Try to get the models metadata from the cache.
        memoryCache.TryGetValue(MetadataCache, out ICollection<EntityMetadata>? metadata);

        if (metadata != null)
        {
            return metadata;
        }

        // If not found in cache, fetch from ormMetadataService.
        metadata = ormMetadataService.GetMetadata().ToList();

        foreach (var entityMetadata in metadata)
        {
            entityMetadata.PluralName = PluralizationProvider.Pluralize(entityMetadata.Name);
            entityMetadata.ApplyOptions(adminOptions);
            entityMetadata.ApplyEntityAttributes();
            entityMetadata.Id = Guid.NewGuid();
        }

        // Store in cache for subsequent requests.
        memoryCache.Set(MetadataCache, metadata);
        return metadata;
    }

    /// <summary>
    /// Find metadata for the certain entity based on its unique identifier.
    /// </summary>
    /// <param name="entityId">The unique identifier of the entity.</param>
    /// <returns>An instance of <see cref="EntityMetadata"/> if the metadata is found; otherwise, null.</returns>
    public EntityMetadata? FindEntityMetadata(Guid entityId)
    {
        var metadata = GetMetadata();
        var entityMetadata = metadata.FirstOrDefault(metadataItem => metadataItem.Id == entityId);
        return entityMetadata;
    }
}
