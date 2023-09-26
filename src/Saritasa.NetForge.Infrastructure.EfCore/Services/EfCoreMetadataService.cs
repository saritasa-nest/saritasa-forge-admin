using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            var entityTypes = model.GetEntityTypes();
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
}
