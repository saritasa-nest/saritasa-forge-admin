namespace Saritasa.NetForge.Tests.Domain.Models;

/// <summary>
/// Represents a shop entity.
/// </summary>
internal class Shop
{
    /// <summary>
    /// Unique identifier for the shop.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the shop.
    /// </summary>
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
    public decimal TotalSales { get; set; }

    /// <summary>
    /// The shop is currently open for business.
    /// </summary>
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
