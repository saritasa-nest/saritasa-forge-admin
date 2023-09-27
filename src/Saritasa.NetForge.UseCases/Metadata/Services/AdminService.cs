using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

namespace Saritasa.NetForge.UseCases.Metadata.Services;

/// <summary>
/// Provides methods for managing and retrieving entities data.
/// </summary>
public class AdminService
{
    private readonly IOrmMetadataService ormMetadataService;
    private readonly AdminOptions adminOptions;

    /// <summary>
    /// Constructor.
    /// </summary>>
    public AdminService(IOrmMetadataService ormMetadataService, AdminOptions adminOptions)
    {
        this.ormMetadataService = ormMetadataService;
        this.adminOptions = adminOptions;
    }

    /// <summary>
    /// Get the models metadata.
    /// </summary>
    public IEnumerable<ModelMetadata> GetMetadata()
    {
        var modelsMetadata = ormMetadataService.GetMetadata().ToList();

        foreach (var entityMetadata in modelsMetadata.SelectMany(metadataItem => metadataItem.Entities))
        {
            ApplyEntityOptions(entityMetadata, adminOptions);
        }

        return modelsMetadata;
    }

    /// <summary>
    /// Applies entity-specific options to the given <see cref="EntityMetadata"/> using the provided options.>.
    /// </summary>
    /// <param name="entityMetadata">The metadata of the entity to which options are applied.</param>
    /// <param name="adminOptions">The admin options containing entity-specific settings.</param>
    private static void ApplyEntityOptions(EntityMetadata entityMetadata, AdminOptions adminOptions)
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
    }
}
