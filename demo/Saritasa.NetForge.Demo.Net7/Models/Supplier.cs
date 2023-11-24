using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Demo.Net7.Constants;
using Saritasa.NetForge.Domain.Attributes;

namespace Saritasa.NetForge.Demo.Net7.Models;

/// <summary>
/// Represents shop supplier.
/// </summary>
[PrimaryKey(nameof(Name), nameof(City))]
[NetForgeEntity(GroupName = GroupConstants.Shops)]
public class Supplier
{
    /// <summary>
    /// Supplier's name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// City where supplier works.
    /// </summary>
    public required string City { get; set; }

    /// <summary>
    /// Whether a supplier still works.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The list of shops which this supplier works with.
    /// </summary>
    public List<Shop> Shops { get; set; } = new();
}
