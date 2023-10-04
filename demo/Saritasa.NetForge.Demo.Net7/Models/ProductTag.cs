﻿namespace Saritasa.NetForge.Demo.Net7.Models;

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
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of the tag.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// List of products associated with this tag.
    /// </summary>
    public List<Product> Products { get; set; } = new();
}

