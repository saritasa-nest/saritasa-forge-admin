using Bogus;
using Saritasa.NetForge.Demo.Models;

namespace Saritasa.NetForge.Demo.Infrastructure.Seeders;

/// <summary>
/// Seed the address data.
/// </summary>
internal class AddressesSeeder
{
    private readonly ShopDbContext shopDbContext;
    private readonly ILogger<AddressesSeeder> logger;

    private readonly Faker faker = new();
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="shopDbContext">Database context.</param>
    /// <param name="logger">Logger.</param>
    public AddressesSeeder(ShopDbContext shopDbContext, ILogger<AddressesSeeder> logger)
    {
        this.shopDbContext = shopDbContext;
        this.logger = logger;
    }
    
    /// <summary>
    /// Seed.
    /// </summary>
    /// <param name="numberOfItems">Total items to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Number of created items.</returns>
    public async Task<int> Seed(int numberOfItems, CancellationToken cancellationToken = default)
    {
        var count = 0;
        
        foreach (var chunk in Tools.Common.Utils.CollectionUtils
                     .ChunkSelectRange(Enumerable.Range(0, numberOfItems), chunkSize: 50))
        {
            foreach (var _ in chunk)
            {
                shopDbContext.Addresses.Add(GenerateAddress());
            }
            count += await shopDbContext.SaveChangesAsync(cancellationToken);
        }
        
        logger.LogInformation("Created {Count} addresses.", count);
        return count;
    }

    private Address GenerateAddress() => new()
    {
        Street = faker.Address.StreetName(),
        City = faker.Address.City(),
        PostalCode = faker.Address.ZipCode(),
        Country = faker.Address.Country(),
        Latitude = faker.Address.Latitude(),
        Longitude = faker.Address.Longitude(),
        ContactPhone = faker.Phone.PhoneNumber()
    };
}