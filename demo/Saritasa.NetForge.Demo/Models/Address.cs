using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Domain.Attributes;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.Demo.Constants;

namespace Saritasa.NetForge.Demo.Models;

/// <summary>
/// Represents an address entity.
/// </summary>
[Comment("Represents the address of the shop.")]
[NetForgeEntity(GroupName = GroupConstants.Shops)]
public class Address
{
    /// <summary>
    /// The unique identifier for the address.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The street name and number.
    /// </summary>
    [NetForgeProperty(DisplayName = "Street name", Description = "Street name without street number.",
        Order = 3, FormOrder = 1, SearchType = SearchType.StartsWithCaseSensitive)]
    [Required]
    public required string Street { get; set; }

    /// <summary>
    /// The city where the address is located.
    /// </summary>
    [NetForgeProperty(Order = 4, FormOrder = 2, SearchType = SearchType.StartsWithCaseSensitive)]
    [Required]
    public required string City { get; set; }

    /// <summary>
    /// Display name of the address.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// The postal code of the address.
    /// </summary>
    [NetForgeProperty(Order = 6)]
    [Required]
    public required string PostalCode { get; set; }

    /// <summary>
    /// The name of the country.
    /// </summary>
    [NetForgeProperty(Description = "Country name.", Order = 5, FormOrder = 3)]
    [Required]
    public required string Country { get; set; }

    /// <summary>
    /// Full address.
    /// </summary>
    public string FullAddress => $"{Country}, {Street}, {City}";

    /// <summary>
    /// The latitude coordinate of the address location.
    /// </summary>
    [NetForgeProperty(Order = 0)]
    public double Latitude { get; set; }

    /// <summary>
    /// The longitude coordinate of the address location.
    /// </summary>
    [NetForgeProperty(IsHiddenFromDetails = true)]
    public double Longitude { get; set; }

    /// <summary>
    /// The contact phone number associated with the address.
    /// </summary>
    [Required]
    public required string ContactPhone { get; set; }

    /// <summary>
    /// Id of user who created the address.
    /// </summary>
    public required int CreatedByUserId { get; set; }

    /// <summary>
    /// Id of last user who updated the address. If null, then the address was not changed after creation.
    /// </summary>
    public int? UpdatedByUserId { get; set; }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not Address address)
        {
            return false;
        }

        return address.Street == Street && address.City == City;
    }

    /// <inheritdoc />
    public override int GetHashCode() => Street.GetHashCode() + City.GetHashCode();
}
