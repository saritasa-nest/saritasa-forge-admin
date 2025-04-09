using MudBlazor;

namespace Saritasa.NetForge.Domain.Dtos;

/// <summary>
/// Represents default sort that applied when no other sort is applied.
/// </summary>
public class DefaultSortDto
{
    /// <summary>
    /// Path to property including navigations.
    /// </summary>
    /// <remarks>
    /// For example full path to property: <c>Product.Shop.Address.Street</c>,
    /// then this should contain <c>Shop.Address.Street</c>.
    /// </remarks>
    public string PropertyPath { get; init; } = string.Empty;

    /// <summary>
    /// Sort direction.
    /// </summary>
    public SortDirection SortDirection { get; init; }

    /// <summary>
    /// Represents order that used when multiple sort is configured.
    /// </summary>
    /// <remarks>
    /// Example with Address: City has <c>Order = 1</c>, Street has <c>Order = 2</c>,
    /// so sort will be applied by city first, then by street.
    /// </remarks>
    public int Order { get; init; }
}
