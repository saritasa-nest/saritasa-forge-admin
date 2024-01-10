using System.ComponentModel;
using Saritasa.NetForge.Domain.Attributes;
using Saritasa.NetForge.Tests.Utilities;

namespace Saritasa.NetForge.Tests.Domain.Models;

/// <summary>
/// Represents a shop entity.
/// </summary>
public class Shop
{
    /// <summary>
    /// Unique identifier for the shop.
    /// </summary>
    [NetForgeProperty(Order = 1)]
    public int Id { get; set; }

    /// <summary>
    /// The name of the shop.
    /// </summary>
    [DisplayName(ShopConstants.NameDisplayName)]
    [Description(ShopConstants.NameDescription)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The address of the shop.
    /// </summary>
    public Address? Address { get; set; }

    /// <summary>
    /// The date when the shop was opened.
    /// </summary>
    public DateTime OpenedDate { get; set; }

    /// <summary>
    /// The total sales amount for the shop.
    /// </summary>
    [NetForgeProperty(
        Order = 0,
        DisplayName = ShopConstants.TotalSalesDisplayName,
        Description = ShopConstants.TotalSalesDescription)]
    public decimal TotalSales { get; set; }

    /// <summary>
    /// The shop is currently open for business.
    /// </summary>
    [NetForgeProperty(IsExcludedFromQuery = true, IsHidden = true)]
    public bool IsOpen { get; set; }

    /// <summary>
    /// The list of products available in the shop.
    /// </summary>
    public List<Product> Products { get; set; } = new();

    /// <summary>
    /// The shop owner's contact information.
    /// </summary>
    public ContactInfo? OwnerContact { get; set; }

    /// <summary>
    /// The list of suppliers.
    /// </summary>
    public List<Supplier> Suppliers { get; set; } = new();
}
