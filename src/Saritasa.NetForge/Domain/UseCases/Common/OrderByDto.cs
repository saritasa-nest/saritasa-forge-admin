namespace Saritasa.NetForge.Domain.UseCases.Common;

/// <summary>
/// DTO for ordering.
/// </summary>
public record OrderByDto
{
    /// <summary>
    /// Property path including navigations. For example: <c>Shop.Address.Street</c>.
    /// </summary>
    public string PropertyPath { get; init; } = string.Empty;

    /// <summary>
    /// Direction of order.
    /// </summary>
    public bool IsDescending { get; init; }
}
