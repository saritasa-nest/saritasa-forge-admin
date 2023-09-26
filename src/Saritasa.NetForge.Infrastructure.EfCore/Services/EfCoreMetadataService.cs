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
    public IEnumerable<EntityMetadata> GetEntities()
    {
        var dbContextTypes = efCoreOptions.DbContexts;
        var entitiesMetadata = new List<EntityMetadata>();

        foreach (var dbContextType in dbContextTypes)
        {
            var dbContextService = serviceProvider.GetService(dbContextType);

            if (dbContextService == null)
            {
                continue;
            }

            var dbContext = (DbContext)dbContextService;
            var model = dbContext.GetService<IDesignTimeModel>().Model;

            // Get entities and exclude those that are not represented as DbSet properties in the DbContext.
            var dbContextEntityTypes = GetDbContextEntities(dbContext);
            var entityTypes = model.GetEntityTypes()
                .Where(entityType => dbContextEntityTypes.Contains(entityType.ClrType));

            var dbContextEntitiesMetadata = entityTypes.Select(entityType => new EntityMetadata
            {
                Name = entityType.ShortName(),
                PluralName = $"{entityType.ShortName()}s",
                ClrType = entityType.ClrType,
                Description = GetEntityDescription(entityType),
            });
            entitiesMetadata.AddRange(dbContextEntitiesMetadata);
        }

        return entitiesMetadata;
    }

    /// <summary>
    /// Gets the entity description from the EF Core entity comment.
    /// </summary>
    /// <param name="entityType">Type of the entity to get the description from.</param>
    private static string GetEntityDescription(IReadOnlyEntityType entityType)
    {
        return entityType.GetComment() ?? string.Empty;
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
