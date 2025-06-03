using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Extensions;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using ExpressionExtensions = Saritasa.NetForge.Domain.Extensions.ExpressionExtensions;

namespace Saritasa.NetForge.Infrastructure.EfCore.Services;

/// <inheritdoc/>
public class EfCoreMetadataService : IOrmMetadataService
{
    private readonly EfCoreOptions efCoreOptions;
    private readonly IServiceProvider serviceProvider;
    private readonly AdminOptions adminOptions;

    /// <summary>
    /// Constructor.
    /// </summary>
    public EfCoreMetadataService(
        EfCoreOptions efCoreOptions, IServiceProvider serviceProvider, AdminOptions adminOptions)
    {
        this.efCoreOptions = efCoreOptions;
        this.serviceProvider = serviceProvider;
        this.adminOptions = adminOptions;
    }

    /// <inheritdoc/>
    public IEnumerable<EntityMetadata> GetMetadata()
    {
        var dbContextTypes = efCoreOptions.DbContexts;
        var metadata = new List<EntityMetadata>();

        // Iterate over all DbContexts, collect entities and their properties.
        foreach (var dbContextType in dbContextTypes)
        {
            var dbContextService = serviceProvider.GetService(dbContextType);

            if (dbContextService == null)
            {
                continue;
            }

            var dbContext = (DbContext)dbContextService;

            var model = dbContext.GetService<IDesignTimeModel>().Model;
            var entityTypes = model.GetEntityTypes().ToList();
            metadata.AddRange(entityTypes.Select(GetEntityMetadata));
        }

        return metadata;
    }

    private byte? currentEntityMaxNavigationDepth;

    /// <summary>
    /// Retrieve metadata for certain entity type.
    /// </summary>
    /// <param name="entityType">The EF Core entity type to retrieve metadata for.</param>
    /// <returns>An <see cref="EntityMetadata"/> object containing metadata information for the entity type.</returns>
    private EntityMetadata GetEntityMetadata(IReadOnlyEntityType entityType)
    {
        var entityOptions = adminOptions.EntityOptionsList
            .FirstOrDefault(entityOptions => entityOptions.EntityType == entityType.ClrType);
        currentEntityMaxNavigationDepth = entityOptions?.MaxNavigationDepth;
        return new EntityMetadata
        {
            DisplayName = entityType.ShortName(),
            ClrType = entityType.ClrType,
            Description = entityType.GetComment() ?? string.Empty,
            IsHidden = entityType.IsPropertyBag,
            Properties = GetPropertiesMetadata(entityType),
            Navigations = GetNavigationsMetadata(entityType),
            IsKeyless = entityType.FindPrimaryKey() is null,
            IsOwned = entityType.IsOwned()
        };
    }

    private List<NavigationMetadata> GetNavigationsMetadata(
        IReadOnlyEntityType entityType,
        int depth = 1,
        StringBuilder? propertyPath = null,
        NavigationMetadata? navigationMetadata = null)
    {
        // GetNavigations retrieves all navigations except many-to-many navigations.
        // GetSkipNavigations retrieves many-to-many navigations
        var efNavigations = entityType
            .GetNavigations()
            .Concat<IReadOnlyNavigationBase>(entityType.GetSkipNavigations());

        List<NavigationMetadata> navigations = [];
        foreach (var efNavigation in efNavigations)
        {
            var navigation = GetNavigationMetadata(efNavigation, depth, propertyPath, navigationMetadata);

            if (navigation is null)
            {
                continue;
            }

            navigations.Add(navigation);
        }

        return navigations;
    }

    /// <summary>
    /// Retrieve metadata for a navigation of an entity type.
    /// </summary>
    /// <param name="navigation">The EF Core navigation to retrieve metadata for.</param>
    /// <param name="depth">
    /// Represents current depth. If greater than max depth, then the navigation will not be visited.
    /// </param>
    /// <param name="propertyPath">Contains full path to the property.</param>
    /// <param name="parentNavigationMetadata">Parent navigation. It means one navigation is child of another.</param>
    /// <returns>A <see cref="PropertyMetadata"/> object containing metadata information for the navigation.</returns>
    private NavigationMetadata? GetNavigationMetadata(
        IReadOnlyNavigationBase navigation,
        int depth,
        StringBuilder? propertyPath,
        NavigationMetadata? parentNavigationMetadata)
    {
        var maxNavigationDepth = currentEntityMaxNavigationDepth ?? adminOptions.MaxNavigationDepth;
        if (depth > maxNavigationDepth)
        {
            return null;
        }

        depth++;

        var isNullable = false;
        if (navigation is IReadOnlySkipNavigation { ForeignKey: not null } skipNavigation)
        {
            isNullable = !(skipNavigation.ForeignKey.IsRequired || skipNavigation.ForeignKey.IsRequiredDependent);
        }
        else if (navigation is IReadOnlyNavigation readOnlyNavigation)
        {
            isNullable = !readOnlyNavigation.ForeignKey.IsRequiredDependent;
        }

        propertyPath = AddPropertyNameToPath(propertyPath, navigation.Name);
        var actualPropertyPath = propertyPath.ToString();
        var navigationMetadata = new NavigationMetadata
        {
            Name = navigation.Name,
            DisplayName = GetDisplayName(parentNavigationMetadata, actualPropertyPath),
            PropertyPath = actualPropertyPath,
            IsCollection = navigation.IsCollection,
            PropertyInformation = navigation.PropertyInfo,
            ClrType = navigation.ClrType,
            IsNullable = isNullable,
            NavigationMetadata = parentNavigationMetadata
        };

        if (navigation is INavigation efNavigation)
        {
            navigationMetadata.IsOwnership = efNavigation.ForeignKey.IsOwnership;
        }

        navigationMetadata.TargetEntityProperties
            = GetPropertiesMetadata(navigation.TargetEntityType, propertyPath, navigationMetadata);
        navigationMetadata.TargetEntityNavigations
            = GetNavigationsMetadata(navigation.TargetEntityType, depth, propertyPath, navigationMetadata);

        RemovePropertyNameFromPath(propertyPath, navigation.Name);
        return navigationMetadata;
    }

    private static List<PropertyMetadata> GetPropertiesMetadata(
        IReadOnlyEntityType entityType,
        StringBuilder? propertyPath = null,
        NavigationMetadata? navigationMetadata = null)
    {
        var propertiesMetadata = entityType
            .GetProperties()
            .Select(property => GetPropertyMetadata(property, propertyPath, navigationMetadata))
            .Where(property => !property.IsShadow); // We can't access shadow properties data, so it should be excluded.

        var reflectionProperties = entityType.ClrType
            .GetProperties()
            .Where(property => !propertiesMetadata.Any(metadata => metadata.Name == property.Name));

        var calculatedProperties = reflectionProperties
            .Where(property => property is { CanRead: true, CanWrite: false })
            .Select(propertyInfo => GetCalculatedPropertyMetadata(propertyInfo, propertyPath));
        return propertiesMetadata.Union(calculatedProperties).ToList();
    }

    /// <summary>
    /// Retrieve metadata for a property of an entity type.
    /// </summary>
    /// <param name="property">The EF Core property to retrieve metadata for.</param>
    /// <param name="propertyPath">Contains full path to the property.</param>
    /// <param name="navigationMetadata">The property belongs to this navigation.</param>
    /// <returns>A <see cref="PropertyMetadata"/> object containing metadata information for the property.</returns>
    private static PropertyMetadata GetPropertyMetadata(
        IReadOnlyProperty property, StringBuilder? propertyPath, NavigationMetadata? navigationMetadata)
    {
        var propertyName = property.Name;
        propertyPath = AddPropertyNameToPath(propertyPath, propertyName);
        var actualPropertyPath = propertyPath.ToString();
        var propertyMetadata = new PropertyMetadata
        {
            Name = propertyName,
            PropertyPath = actualPropertyPath,
            Description = property.GetComment() ?? string.Empty,
            ClrType = property.ClrType,
            DisplayName = GetDisplayName(navigationMetadata, actualPropertyPath),
            PropertyInformation = property.PropertyInfo,
            IsForeignKey = property.IsForeignKey(),
            IsPrimaryKey = property.IsPrimaryKey(),
            IsNullable = property.IsNullable,
            Order = property.GetColumnOrder(),
            IsShadow = property.IsShadowProperty(),
            IsValueGeneratedOnAdd = property.ValueGenerated.HasFlag(ValueGenerated.OnAdd),
            IsValueGeneratedOnUpdate = property.ValueGenerated.HasFlag(ValueGenerated.OnUpdate),
            IsReadOnly = property.PropertyInfo?.SetMethod is null || property.PropertyInfo.SetMethod.IsPrivate,
            NavigationMetadata = navigationMetadata
        };

        RemovePropertyNameFromPath(propertyPath, propertyName);
        return propertyMetadata;
    }

    private static PropertyMetadata GetCalculatedPropertyMetadata(PropertyInfo propertyInfo, StringBuilder? propertyPath)
    {
        var propertyName = propertyInfo.Name;
        propertyPath = AddPropertyNameToPath(propertyPath, propertyName);
        var propertyMetadata = new PropertyMetadata
        {
            Name = propertyName,
            PropertyPath = propertyPath.ToString(),
            ClrType = propertyInfo.PropertyType,
            IsEditable = false,
            PropertyInformation = propertyInfo,
            IsCalculatedProperty = true
        };

        RemovePropertyNameFromPath(propertyPath, propertyName);
        return propertyMetadata;
    }

    private static StringBuilder AddPropertyNameToPath(StringBuilder? propertyPath, string propertyName)
    {
        return propertyPath is null
            ? new StringBuilder(propertyName)
            : propertyPath.Append($"{ExpressionExtensions.PropertySeparator}{propertyName}");
    }

    private static void RemovePropertyNameFromPath(StringBuilder propertyPath, string propertyName)
    {
        propertyPath.Replace($"{ExpressionExtensions.PropertySeparator}{propertyName}", string.Empty);
    }

    /// <summary>
    /// Makes display name of owned navigation's property to contain navigation name.
    /// For example: Owned navigation is Director, property is Age, then the display name will be: Director Age.
    /// </summary>
    /// <param name="navigationMetadata">Parent navigation.</param>
    /// <param name="propertyPath">This path will be used as display name with dots replaced with spaces.</param>
    private static string GetDisplayName(NavigationMetadata? navigationMetadata, string propertyPath)
    {
        return navigationMetadata is not null && navigationMetadata.IsOwnership
            ? propertyPath.Replace(ExpressionExtensions.PropertySeparator, ' ').ToMeaningfulName()
            : string.Empty;
    }
}
