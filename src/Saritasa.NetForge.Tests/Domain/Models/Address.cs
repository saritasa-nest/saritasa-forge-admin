using Saritasa.NetForge.Domain.Attributes;

namespace Saritasa.NetForge.Tests.Domain.Models;

/// <summary>
/// Represents an address entity.
/// </summary>
public class Address
{
    /// <summary>
    /// The unique identifier for the address.
    /// </summary>
    [NetForgeProperty(DisplayAsHtml = true)]
    public int Id { get; set; }

    /// <summary>
    /// The street name and number.
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// The city where the address is located.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// The postal code of the address.
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// The name of the country.
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// The latitude coordinate of the address location.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// The longitude coordinate of the address location.
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// The contact phone number associated with the address.
    /// </summary>
    public string ContactPhone { get; set; } = string.Empty;
}
