using System.ComponentModel.DataAnnotations;

namespace Saritasa.NetForge.Tests.Domain.Models;

/// <summary>
/// Represents a tag associated with a product in the shop.
/// </summary>
internal class ProductTag
{
    /// <summary>
    /// The unique identifier for the tag.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the tag.
    /// </summary>
    [MinLength(1)]
    [MaxLength(10)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of the tag.
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;

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
