using Microsoft.Extensions.Caching.Memory;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

namespace Saritasa.NetForge.UseCases.Metadata.Services;

/// <summary>
/// Provides methods for managing and retrieving entities data.
/// </summary>
public class AdminService
{
    private const string MetadataCache = "MetadataCache";
    private readonly IOrmMetadataService ormMetadataService;
    private readonly AdminOptions adminOptions;
    private readonly IMemoryCache memoryCache;

    /// <summary>
    /// Constructor.
    /// </summary>>
    public AdminService(IOrmMetadataService ormMetadataService, AdminOptions adminOptions, IMemoryCache memoryCache)
    {
        this.ormMetadataService = ormMetadataService;
        this.adminOptions = adminOptions;
        this.memoryCache = memoryCache;
    }

    /// <summary>
    /// Get the metadata.
    /// </summary>
    public IEnumerable<EntityMetadata> GetMetadata()
    {
        // Try to get the models metadata from the cache.
        if (memoryCache.TryGetValue(MetadataCache, out IList<EntityMetadata>? metadata))
        {
            return metadata;
        }

        // If not found in cache, fetch from ormMetadataService.
        metadata = ormMetadataService.GetMetadata().ToList();

        foreach (var entityMetadata in metadata)
        {
            ApplyEntityOptions(entityMetadata);
            entityMetadata.Id = Guid.NewGuid();
        }

        // Store in cache for subsequent requests.
        memoryCache.Set(MetadataCache, metadata, TimeSpan.FromMinutes(30));

        return metadata;
    }

    /// <summary>
    /// Applies entity-specific options to the given <see cref="EntityMetadata"/> using the provided options.>.
    /// </summary>
    /// <param name="entityMetadata">The metadata of the entity to which options are applied.</param>
    private void ApplyEntityOptions(EntityMetadata entityMetadata)
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

        if (!string.IsNullOrEmpty(entityOptions.Name))
        {
            entityMetadata.Name = entityOptions.Name;
        }

        if (!string.IsNullOrEmpty(entityOptions.PluralName))
        {
            entityMetadata.PluralName = entityOptions.PluralName;
        }

        if (entityOptions.IsHidden)
        {
            entityMetadata.IsHidden = entityOptions.IsHidden;
        }
    }
}
