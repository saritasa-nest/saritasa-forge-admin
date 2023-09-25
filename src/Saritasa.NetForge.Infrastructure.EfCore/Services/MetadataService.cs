using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Saritasa.NetForge.Domain.Entities;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

namespace Saritasa.NetForge.Infrastructure.EfCore.Services;

/// <inheritdoc/>
internal class MetadataService : IMetadataService
{
    private readonly EfCoreOptions efCoreOptions;
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Constructor.
    /// </summary>
    public MetadataService(EfCoreOptions efCoreOptions, IServiceProvider serviceProvider)
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

    private string GetEntityDescription(IReadOnlyEntityType entityType)
    {
        var comment = entityType.GetComment();
        return comment;
    }
}
