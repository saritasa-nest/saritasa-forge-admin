﻿using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using PluralizeService.Core;
using Saritasa.NetForge.Blazor.Domain.Attributes;
using Saritasa.NetForge.Blazor.Domain.Entities.Metadata;
using Saritasa.NetForge.Blazor.Domain.Entities.Options;
using Saritasa.NetForge.Blazor.Domain.Extensions;
using Saritasa.NetForge.Blazor.Infrastructure.Abstractions.Interfaces;

namespace Saritasa.NetForge.Blazor.Domain.UseCases.Metadata.Services;

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
        var cachedMetadata = GetEntitiesMetadataFromCache();

        if (cachedMetadata != null)
        {
            return cachedMetadata;
        }

        // If not found in cache, fetch from ormMetadataService.
        var metadata = ormMetadataService.GetMetadata().ToList();
        ExcludeEntities(metadata, adminOptions);

        foreach (var entityMetadata in metadata)
        {
            entityMetadata.PluralName = PluralizationProvider.Pluralize(entityMetadata.DisplayName);
            entityMetadata.StringId = entityMetadata.PluralName;
            var entityOptions = adminOptions.EntityOptionsList
                .FirstOrDefault(options => options.EntityType == entityMetadata.ClrType);

            if (entityOptions != null)
            {
                var calculatedProperties = GetCalculatedPropertiesMetadata(entityOptions);
                entityMetadata.Properties.AddRange(calculatedProperties);
                entityMetadata.ApplyOptions(entityOptions, adminOptions);

                if (entityOptions.ExcludeAllProperties)
                {
                    // Exclude properties if it was specified in the entity options.
                    ExcludeProperties(entityMetadata, entityOptions);
                }
            }

            entityMetadata.ApplyEntityAttributes(adminOptions);
            entityMetadata.Id = Guid.NewGuid();
        }

        CacheMetadata(metadata);
        return metadata;
    }

    private static void ExcludeEntities(List<EntityMetadata> entityMetadatas, AdminOptions adminOptions)
    {
        // Exclude entities if it was specified in the admin options.
        if (adminOptions.IncludeAllEntities)
        {
            return;
        }

        var initialCount = entityMetadatas.Count;

        // Include only entities specified in the IncludedEntities list or those with NetForgeEntityAttribute.
        entityMetadatas.RemoveAll(e =>
            e.ClrType != null &&
            !adminOptions.IncludedEntities.Contains(e.ClrType) &&
            e.ClrType.GetCustomAttribute<NetForgeEntityAttribute>() == null);

        // If no entities in the IncludedEntities list or with NetForgeEntityAttribute, clear all entities.
        if (entityMetadatas.Count == initialCount)
        {
            entityMetadatas.Clear();
        }
    }

    private static void ExcludeProperties(EntityMetadata entityMetadata, EntityOptions entityOptions)
    {
        if (!entityOptions.ExcludeAllProperties)
        {
            return;
        }

        ExcludePropertyItems(entityMetadata.Properties, entityOptions.IncludedProperties);
        ExcludePropertyItems(entityMetadata.Navigations, entityOptions.IncludedProperties);
    }

    private static void ExcludePropertyItems<T>(List<T> items, List<string> includedProperties) where T :
        PropertyMetadataBase
    {
        var initialCount = items.Count;

        // Include only properties specified in the IncludedProperties list or those with NetForgePropertyAttribute.
        items.RemoveAll(p => p.ClrType != null
                             && !includedProperties.Contains(p.Name)
                             && p.PropertyInformation?.GetCustomAttribute<NetForgePropertyAttribute>() == null);

        if (items.Count == initialCount)
        {
            items.Clear();
        }
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
