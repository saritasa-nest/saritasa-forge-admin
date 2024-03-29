﻿namespace Saritasa.NetForge.Tests.Domain.Models;

/// <summary>
/// Represents shop supplier.
/// </summary>
internal class Supplier
{
    /// <summary>
    /// Supplier's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// City where supplier works.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Whether a supplier still works.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The list of shops which this supplier works with.
    /// </summary>
    public List<Shop> Shops { get; set; } = new();

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Name}; {City}";
    }
}
