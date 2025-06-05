using Bogus;
using Saritasa.NetForge.Demo.Models;

namespace Saritasa.NetForge.Demo.Infrastructure.Seeders;

/// <summary>
/// Helps to create fake domain model instances.
/// </summary>
internal static class FakerExtensions
{
    /// <summary>
    /// Generates <see cref="Address"/>.
    /// </summary>
    internal static Address GenerateAddress(this Faker faker) => new()
    {
        Street = faker.Address.StreetName(),
        City = faker.Address.City(),
        PostalCode = faker.Address.ZipCode(),
        Country = faker.Address.Country(),
        Latitude = faker.Address.Latitude(),
        Longitude = faker.Address.Longitude(),
        ContactPhone = faker.Phone.PhoneNumber(),
        CreatedByUserId = faker.IndexGlobal
    };
}