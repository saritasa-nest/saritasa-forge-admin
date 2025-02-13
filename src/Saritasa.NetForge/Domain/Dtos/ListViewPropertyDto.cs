using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Domain.Dtos;

/// <summary>
/// DTO for property that will be displayed on <c>List View</c> page.
/// </summary>
public record ListViewPropertyDto
{
    /// <summary>
    /// Property metadata.
    /// </summary>
    public PropertyMetadataDto Property { get; init; } = null!;

    /// <summary>
    /// Full path to property in case when the property is inside the navigation. For example: <c>Shop.Address.Street</c>.
    /// </summary>
    public string PropertyPath { get; init; } = null!;

    /// <summary>
    /// Navigation metadata when the property is inside the navigation.
    /// </summary>
    public NavigationMetadataDto? Navigation { get; init; }
}
