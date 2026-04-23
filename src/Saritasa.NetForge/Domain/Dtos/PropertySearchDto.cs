using Saritasa.NetForge.Domain.Enums;

namespace Saritasa.NetForge.Domain.Dtos;

/// <summary>
/// Property search DTO.
/// </summary>
public record PropertySearchDto
{
    /// <summary>
    /// Property path including navigations. For example: <c>Shop.Address.Street</c>.
    /// </summary>
    public string PropertyPath { get; init; } = string.Empty;

    /// <summary>
    /// Search type.
    /// </summary>
    public SearchType SearchType { get; init; }
}
