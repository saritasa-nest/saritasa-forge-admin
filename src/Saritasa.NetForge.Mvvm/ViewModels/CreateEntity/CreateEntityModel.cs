using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.Mvvm.ViewModels.CreateEntity;

/// <summary>
/// Model for create entity page.
/// </summary>
public class CreateEntityModel
{
    /// <inheritdoc cref="EntityMetadata.Id"/>
    public string StringId { get; set; } = string.Empty;
}
