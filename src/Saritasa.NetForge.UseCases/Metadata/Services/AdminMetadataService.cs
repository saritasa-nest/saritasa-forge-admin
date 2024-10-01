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
        var metadata = GetEntitiesMetadataFromCache();

        if (metadata != null)
        {
            return metadata;
        }

        // If not found in cache, fetch from ormMetadataService.
        metadata = ormMetadataService.GetMetadata().ToList();

        foreach (var entityMetadata in metadata)
        {
            entityMetadata.PluralName = PluralizationProvider.Pluralize(entityMetadata.DisplayName);
            entityMetadata.StringId = entityMetadata.PluralName;
            var entityOptions = adminOptions.EntityOptionsList
                .FirstOrDefault(options => options.EntityType == entityMetadata.ClrType);

            if (entityOptions != null)
            {
                entityMetadata.ApplyOptions(entityOptions, adminOptions);
            }

            entityMetadata.ApplyEntityAttributes(adminOptions);
            entityMetadata.Id = Guid.NewGuid();
        }

        CacheMetadata(metadata);
        return metadata;
    }

    /// <summary>
    /// Try to get the entities metadata from the cache.
    /// </summary>
    /// <returns>Collection of entity metadata.</returns>
    private ICollection<EntityMetadata>? GetEntitiesMetadataFromCache()
    {
        memoryCache.TryGetValue(MetadataCache, out ICollection<EntityMetadata>? metadata);
        return metadata;
    }

    /// <summary>
    /// Cache metadata for subsequent requests.
    /// </summary>
    /// <param name="metadata">Collection of entity metadata.</param>
    private void CacheMetadata(ICollection<EntityMetadata> metadata)
    {
        memoryCache.Set(MetadataCache, metadata);
    }
}
