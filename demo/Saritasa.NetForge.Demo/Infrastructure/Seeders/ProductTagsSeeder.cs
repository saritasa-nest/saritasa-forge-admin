using Bogus;
using Saritasa.NetForge.Demo.Models;

namespace Saritasa.NetForge.Demo.Infrastructure.Seeders;

/// <summary>
///  Product Tags seeder.
/// </summary>
internal class ProductTagsSeeder
{
    private readonly ShopDbContext shopDbContext;
    private readonly ILogger<ProductTagsSeeder> logger;

    private readonly Faker faker = new();
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="shopDbContext">Database context.</param>
    /// <param name="logger">Logger.</param>
    public ProductTagsSeeder(ShopDbContext shopDbContext, ILogger<ProductTagsSeeder> logger)
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
                shopDbContext.ProductTags.Add(GenerateProductTag());
            }
            count += await shopDbContext.SaveChangesAsync(cancellationToken);
        }
        
        logger.LogInformation("Created {Count} product tags.", count);
        return count;
    }

    private ProductTag GenerateProductTag() => new()
    {
        Name = faker.Random.Word(),
        Description = faker.Lorem.Sentence(),
    };
}