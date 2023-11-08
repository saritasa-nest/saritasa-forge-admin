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
            var entityOptions =
                adminOptions.EntityOptionsList.FirstOrDefault(options => options.EntityType == entityMetadata.ClrType);

            if (entityOptions != null)
            {
                var calculatedProperties = GetCalculatedPropertiesMetadata(entityOptions);
                entityMetadata.Properties.AddRange(calculatedProperties);
                entityMetadata.ApplyOptions(entityOptions);
            }

            entityMetadata.ApplyEntityAttributes();
            entityMetadata.Id = Guid.NewGuid();
        }

        CacheMetadata(metadata);
        return metadata;
    }

    /// <summary>
    /// Retrieves metadata for calculated properties defined in the entity options.
    /// </summary>
    /// <param name="entityOptions">The entity options that specify the calculated properties.</param>
    /// <returns>An enumerable collection of calculated property metadata.</returns>
    private static IEnumerable<PropertyMetadata> GetCalculatedPropertiesMetadata(EntityOptions entityOptions)
    {
        var propertiesMetadata = new List<PropertyMetadata>();

        foreach (var propertyName in entityOptions.CalculatedPropertyNames)
        {
            var propertyInformation = entityOptions.EntityType.GetProperty(propertyName);

            if (propertyInformation == null)
            {
                continue;
            }

            var propertyMetadata = new PropertyMetadata
            {
                Name = propertyName,
                IsEditable = false,
                PropertyInformation = propertyInformation,
                IsCalculatedProperty = true
            };

            propertiesMetadata.Add(propertyMetadata);
        }

        return propertiesMetadata;
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
