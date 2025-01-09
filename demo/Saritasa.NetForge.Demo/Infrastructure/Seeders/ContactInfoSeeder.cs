using Bogus;
using Saritasa.NetForge.Demo.Models;

namespace Saritasa.NetForge.Demo.Infrastructure.Seeders;

/// <summary>
/// Contact information seeder.
/// </summary>
internal class ContactInfoSeeder
{
    private readonly ShopDbContext shopDbContext;
    private readonly ILogger<ContactInfoSeeder> logger;

    private readonly Faker faker = new();
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="shopDbContext">Database context.</param>
    /// <param name="logger">Logger.</param>
    public ContactInfoSeeder(ShopDbContext shopDbContext, ILogger<ContactInfoSeeder> logger)
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
                shopDbContext.ContactInfos.Add(GenerateContactInfo());
            }
            count += await shopDbContext.SaveChangesAsync(cancellationToken);
        }
        
        logger.LogInformation("Created {Count} contacts.", count);
        return count;
    }

    private ContactInfo GenerateContactInfo() => new()
    {
        PhoneNumber = faker.Phone.PhoneNumber(),
        FullName = faker.Name.FullName(),
        Email = faker.Internet.Email(),
    };
}