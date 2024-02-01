using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Demo.Constants;
using Saritasa.NetForge.Domain.Attributes;

namespace Saritasa.NetForge.Demo.Models;

/// <summary>
/// Count of products in the shop.
/// </summary>
[Keyless]
[NetForgeEntity(GroupName = GroupConstants.Shops)]
public class ShopProductsCount
{
    /// <summary>
    /// Shop.
    /// </summary>
    [NetForgeNavigation(IsIncluded = true, Order = 1, DisplayName = "Shop Id")]
    public required Shop Shop { get; init; }

    /// <summary>
    /// Products count.
    /// </summary>
    [NetForgeProperty(Order = 2)]
    public int ProductsCount { get; init; }
}
