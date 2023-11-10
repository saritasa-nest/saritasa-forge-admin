using Bogus;
using Saritasa.NetForge.Demo.Net7.Models;

namespace Saritasa.NetForge.Demo.Net7.Infrastructure.Seeders;

public class SuppliersSeeder
{
    private readonly ShopDbContext shopDbContext;
    private readonly ILogger<SuppliersSeeder> logger;

    private readonly Faker faker = new();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="shopDbContext">Database context.</param>
    /// <param name="logger">Logger.</param>

    public SuppliersSeeder(ShopDbContext shopDbContext, ILogger<SuppliersSeeder> logger)
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
            foreach (var chunkRange in chunk)
            {
                var supplier = GenerateSupplier();
                shopDbContext.Suppliers.Add(supplier);
            }
            count += await shopDbContext.SaveChangesAsync(cancellationToken);
        }

        logger.LogInformation("Created {count} suppliers.", count);
        return count;
    }

    private Supplier GenerateSupplier() => new()
    {
        Name = faker.Company.CompanyName()
    };
}
