using System.ComponentModel;
using Saritasa.NetForge.Demo.Constants;
using Saritasa.NetForge.Domain.Attributes;

namespace Saritasa.NetForge.Demo.Models;

/// <summary>
/// Represents a shop entity.
/// </summary>
[NetForgeEntity(GroupName = GroupConstants.Shops)]
public class Shop
{
    /// <summary>
    /// Unique identifier for the shop.
    /// </summary>
    [Description("Shop identifier.")]
    [DisplayName("Identifier")]
    public int Id { get; set; }

    /// <summary>
    /// The name of the shop.
    /// </summary>
    [Description("Shop name.")]
    [NetForgeProperty(IsSortable = true)]
    public required string Name { get; set; }

    /// <summary>
    /// The address of the shop.
    /// </summary>
    [NetForgeNavigation(DisplayName = "AddressId", Description = "Address identifier.", Order = 1)]
    public Address? Address { get; set; }

    /// <summary>
    /// The date when the shop was opened.
    /// </summary>
    public DateTime OpenedDate { get; set; }

    /// <summary>
    /// The total sales amount for the shop.
    /// </summary>
    [NetForgeProperty(DisplayName = "Sales", Description = "Total sales.", IsSortable = true)]
    public decimal TotalSales { get; set; }

    /// <summary>
    /// The shop is currently open for business.
    /// </summary>
    public bool IsOpen { get; set; }

    /// <summary>
    /// The list of products available in the shop.
    /// </summary>
    [DisplayName("ShopProducts")]
    public List<Product> Products { get; set; } = new();

    /// <summary>
    /// The shop owner's contact information.
    /// </summary>
    public ContactInfo? OwnerContact { get; set; }

    /// <summary>
    /// The list of suppliers.
    /// </summary>
    public List<Supplier> Suppliers { get; set; } = new();

    /// <summary>
    /// Path to image file.
    /// </summary>
    [NetForgeProperty(
        IsPathToImage = true,
        ImageFolder = "Shop images",
        Order = 3)]
    public string? PathToImage { get; set; }

    /// <summary>
    /// Bytes that represents image.
    /// </summary>
    [NetForgeProperty(
        IsBase64Image = true,
        Order = 4, 
        Description = "Represents base 64 image: data:image/{MIME};base64,{bytes of image}.")]
    public string? Base64Image { get; set; }
}

