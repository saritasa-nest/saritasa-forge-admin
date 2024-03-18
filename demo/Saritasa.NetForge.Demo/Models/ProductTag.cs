namespace Saritasa.NetForge.Demo.Models;

/// <summary>
/// Represents a tag associated with a product in the shop.
/// </summary>
public class ProductTag
{
    /// <summary>
    /// The unique identifier for the tag.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the tag.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The description of the tag.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// List of products associated with this tag.
    /// </summary>
    public List<Product> Products { get; set; } = new();

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Id} - {Name}";
    }
}

