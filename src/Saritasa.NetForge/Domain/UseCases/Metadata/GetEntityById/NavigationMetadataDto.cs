using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;

/// <summary>
/// DTO for <see cref="NavigationMetadata"/>.
/// </summary>
public record NavigationMetadataDto : PropertyMetadataDto
{
    /// <inheritdoc cref="NavigationMetadata.IsCollection"/>
    public bool IsCollection { get; set; }

    /// <inheritdoc cref="NavigationMetadata.TargetEntityProperties"/>
    public List<PropertyMetadataDto> TargetEntityProperties { get; set; } = [];

    /// <summary>
    /// Target navigation entity's navigations.
    /// </summary>
    public List<NavigationMetadataDto> TargetEntityNavigations { get; set; } = [];
}
