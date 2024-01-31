using Microsoft.EntityFrameworkCore;

namespace Saritasa.NetForge.Demo.Models;

/// <summary>
/// Count of products in the shop.
/// </summary>
[Keyless]
public class ShopProductsCount
{
    /// <summary>
    /// Shop.
    /// </summary>
    public required Shop Shop { get; init; }

    /// <summary>
    /// Products count.
    /// </summary>
    public int ProductsCount { get; init; }
}
