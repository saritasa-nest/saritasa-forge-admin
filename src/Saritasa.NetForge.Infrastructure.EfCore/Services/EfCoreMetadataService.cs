using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

namespace Saritasa.NetForge.Infrastructure.EfCore.Services;

/// <inheritdoc/>
internal class EfCoreMetadataService : IOrmMetadataService
{
    private readonly EfCoreOptions efCoreOptions;
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Constructor.
    /// </summary>
    public EfCoreMetadataService(EfCoreOptions efCoreOptions, IServiceProvider serviceProvider)
    {
        this.efCoreOptions = efCoreOptions;
        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public IEnumerable<EntityMetadata> GetMetadata()
    {
        var dbContextTypes = efCoreOptions.DbContexts;
        var metadata = new List<EntityMetadata>();

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
            var entitiesMetadata = entityTypes.Select(GetEntityMetadata);
            metadata.AddRange(entitiesMetadata);
        }

        return metadata;
    }

    private EntityMetadata GetEntityMetadata(IReadOnlyEntityType entityType)
    {
        var propertiesMetadata = entityType.GetProperties().Select(GetPropertyMetadata);
        var entityMetadata = new EntityMetadata
        {
            Name = entityType.ShortName(),
            PluralName = $"{entityType.ShortName()}s",
            ClrType = entityType.ClrType,
            Description = entityType.GetComment() ?? string.Empty,
            IsHidden = entityType.IsPropertyBag, // TODO: check whether we need to bypass this
            Properties = propertiesMetadata.ToList()
        };

        return entityMetadata;
    }

    private PropertyMetadata GetPropertyMetadata(IReadOnlyProperty property)
    {
        var propertyMetadata = new PropertyMetadata
        {
            Name = property.Name,
            Description = property.GetComment() ?? string.Empty,
            ClrType = property.ClrType,
            PropertyInformation = property.PropertyInfo,
            IsForeignKey = property.IsForeignKey(),
            IsPrimaryKey = property.IsPrimaryKey(),
            IsNullable = property.IsNullable,
            Order = property.GetColumnOrder() ?? default,
            PrincipalEntityType = property.FindFirstPrincipal()?.ClrType
        };

        return propertyMetadata;
    }
}
