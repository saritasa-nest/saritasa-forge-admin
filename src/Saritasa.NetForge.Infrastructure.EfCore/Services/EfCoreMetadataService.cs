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
    public IEnumerable<ModelMetadata> GetMetadata()
    {
        var dbContextTypes = efCoreOptions.DbContexts;
        var metadata = new List<ModelMetadata>();

        foreach (var dbContextType in dbContextTypes)
        {
            var dbContextService = serviceProvider.GetService(dbContextType);

            if (dbContextService == null)
            {
                continue;
            }

            metadata.Add(GetDbContextMetadata((DbContext)dbContextService));
        }

        return metadata;
    }

    private ModelMetadata GetDbContextMetadata(DbContext dbContext)
    {
        var model = dbContext.GetService<IDesignTimeModel>().Model;
        var entityTypes = model.GetEntityTypes().ToList();
        var entitiesMetadata = entityTypes.Select(GetEntityMetadata);
        return new ModelMetadata
        {
            Entities = entitiesMetadata,
            Name = nameof(dbContext),
            ClrType = dbContext.GetType()
        };
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
            Properties = propertiesMetadata
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
            Order = property.GetColumnOrder() ?? default
        };

        return propertyMetadata;
    }

    /// <summary>
    /// Retrieves a collection of entity types associated with the provided <see cref="DbContext"/>.
    /// </summary>
    /// <param name="dbContext">The DbContext instance for which to retrieve entity types.</param>
    private static IEnumerable<Type> GetDbContextEntities(DbContext dbContext)
    {
        var dbSets = dbContext.GetType().GetProperties()
            .Where(property => property.PropertyType.IsGenericType &&
                               typeof(DbSet<>).IsAssignableFrom(property.PropertyType.GetGenericTypeDefinition()));
        return dbSets.Select(dbSet => dbSet.PropertyType.GetGenericArguments()[0]);
    }
}
