using Saritasa.NetForge.Domain.Attributes;

namespace Saritasa.NetForge.Demo.Net7.Models;

/// <summary>
/// Represents a product entity.
/// </summary>
[NetForgeEntity(Description = "The product in the shop.")]
public class Product
{
    /// <summary>
    /// The unique identifier for the product.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the product.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The description of the product.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// The price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The quantity of the product in stock.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// The weight of the product in grams.
    /// </summary>
    public double WeightInGrams { get; set; }

    /// <summary>
    /// The length of the product in centimeters.
    /// </summary>
    public double LengthInCentimeters { get; set; }

    /// <summary>
    /// The width of the product in centimeters.
    /// </summary>
    public double WidthInCentimeters { get; set; }

    /// <summary>
    /// The height of the product in centimeters.
    /// </summary>
    public double HeightInCentimeters { get; set; }

    /// <summary>
    /// The date and time when the product was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// The date and time when the product was last updated.
    /// </summary>
    public DateTime UpdatedDate { get; set; }

    /// <summary>
    /// The list of tags associated with the product.
    /// </summary>
    public List<ProductTag> Tags { get; set; } = new();
}

