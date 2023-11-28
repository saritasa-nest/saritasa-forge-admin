using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Mvvm.ViewModels.CreateEntity;

/// <summary>
/// Model for create entity page.
/// </summary>
public class CreateEntityModel
{
    /// <inheritdoc cref="EntityMetadata.Id"/>
    public string StringId { get; set; } = string.Empty;

    /// <inheritdoc cref="EntityMetadata.DisplayName"/>
    public string DisplayName { get; set; } = string.Empty;

    /// <inheritdoc cref="EntityMetadata.ClrType"/>
    public Type? ClrType { get; set; }

    /// <inheritdoc cref="EntityMetadata.Properties"/>
    public ICollection<PropertyMetadataDto> Properties { get; set; } = new List<PropertyMetadataDto>();
}
