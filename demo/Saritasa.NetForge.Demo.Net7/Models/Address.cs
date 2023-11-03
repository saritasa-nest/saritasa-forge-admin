using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Domain.Attributes;

namespace Saritasa.NetForge.Demo.Net7.Models;

/// <summary>
/// Represents an address entity.
/// </summary>
[Comment("Represents the address of the shop.")]
[NetForgeEntity(GroupName = "Test Name")]
public class Address
{
    /// <summary>
    /// The unique identifier for the address.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The street name and number.
    /// </summary>
    [NetForgeProperty(DisplayName = "Street name", Description = "Street name without street number.", Order = 3)]
    public required string Street { get; set; }

    /// <summary>
    /// The city where the address is located.
    /// </summary>
    [NetForgeProperty(Order = 4)]
    public required string City { get; set; }

    /// <summary>
    /// The postal code of the address.
    /// </summary>
    public required string PostalCode { get; set; }

    /// <summary>
    /// The name of the country.
    /// </summary>
    [NetForgeProperty(Description = "Country name.", Order = 5)]
    public required string Country { get; set; }

    /// <summary>
    /// The latitude coordinate of the address location.
    /// </summary>
    [NetForgeProperty(Order = 0)]
    public double Latitude { get; set; }

    /// <summary>
    /// The longitude coordinate of the address location.
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// The contact phone number associated with the address.
    /// </summary>
    public required string ContactPhone { get; set; }
}
