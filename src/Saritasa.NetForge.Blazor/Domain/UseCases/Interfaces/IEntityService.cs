using System.ComponentModel.DataAnnotations;
using Saritasa.NetForge.Blazor.Domain.Entities.Metadata;
using Saritasa.NetForge.Blazor.Domain.UseCases.Metadata.DTOs;
using Saritasa.NetForge.Blazor.Domain.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Domain.UseCases.Interfaces;

/// <summary>
/// Handle operations related to entities.
/// </summary>
public interface IEntityService
{
    /// <summary>
    /// Search for entities metadata.
    /// </summary>
    /// <param name="cancellationToken">Token for cancelling async operation.</param>
    Task<IEnumerable<EntityMetadataDto>> SearchEntitiesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Get <see cref="EntityMetadata"/> by <see cref="EntityMetadata.Id"/>.
    /// </summary>
    /// <param name="stringId">Entity string identifier.</param>
    /// <param name="cancellationToken">Token for cancelling async operation.</param>
    Task<GetEntityDto> GetEntityByIdAsync(string stringId, CancellationToken cancellationToken);

    /// <summary>
    /// Get <see cref="EntityMetadata"/> by <see cref="EntityMetadata.ClrType"/>.
    /// </summary>
    /// <param name="entityType">Entity type.</param>
    /// <param name="cancellationToken">Token for cancelling async operation.</param>
    /// <returns>Entity DTO.</returns>
    Task<GetEntityDto> GetEntityByTypeAsync(Type entityType, CancellationToken cancellationToken);

    /// <summary>
    /// Validates the specified entity instance and populates the provided list of validation errors.
    /// </summary>
    /// <param name="instance">The object instance to be validated.</param>
    /// <param name="properties">The metadata of properties for entity.</param>
    /// <param name="errors">A reference to a list of <see cref="ValidationResult"/> where validation errors will be stored.</param>
    /// <returns>Returns <c>true</c> if the entity is valid; otherwise, returns <c>false</c>.</returns>
    bool ValidateEntity(object instance, ICollection<PropertyMetadataDto> properties, ref List<ValidationResult> errors);
}
