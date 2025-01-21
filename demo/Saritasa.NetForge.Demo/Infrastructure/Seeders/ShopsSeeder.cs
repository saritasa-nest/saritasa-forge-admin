using Bogus;
using Saritasa.NetForge.Demo.Models;

namespace Saritasa.NetForge.Demo.Infrastructure.Seeders;

/// <summary>
/// Shops seeder.
/// </summary>
internal class ShopsSeeder
{
    private readonly ShopDbContext shopDbContext;
    private readonly ILogger<ShopsSeeder> logger;

    private readonly Faker faker = new();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="shopDbContext">Database context.</param>
    /// <param name="logger">Logger.</param>

    public ShopsSeeder(ShopDbContext shopDbContext, ILogger<ShopsSeeder> logger)
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
                var shop = GenerateShop();
                shopDbContext.Shops.Add(shop);
            }
            count += await shopDbContext.SaveChangesAsync(cancellationToken);
        }
        
        logger.LogInformation("Created {Count} shops.", count);
        return count;
    }

    private Shop GenerateShop() => new()
    {
        Name = faker.Company.CompanyName(),
        OpenedDate = faker.Date.Past(yearsToGoBack: 1).ToUniversalTime(),
        TotalSales = decimal.Parse(faker.Commerce.Price(max: 100000)),
        IsOpen = faker.Random.Bool(),
    };
}