using Bogus;
using Saritasa.NetForge.Demo.Models;

namespace Saritasa.NetForge.Demo.Infrastructure.Seeders;

/// <summary>
/// Products seeder.
/// </summary>
internal class ProductsSeeder : ISeeder
{
    private readonly ShopDbContext shopDbContext;
    private readonly ILogger<ProductsSeeder> logger;

    private readonly Faker faker = new();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="shopDbContext">Database context.</param>
    /// <param name="logger">Logger.</param>
    public ProductsSeeder(ShopDbContext shopDbContext, ILogger<ProductsSeeder> logger)
    {
        this.shopDbContext = shopDbContext;
        this.logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<int> Seed(int numberOfItems, CancellationToken cancellationToken = default)
    {
        var count = 0;
        
        foreach (var chunk in Tools.Common.Utils.CollectionUtils
                     .ChunkSelectRange(Enumerable.Range(0, numberOfItems), chunkSize: 50))
        {
            foreach (var _ in chunk)
            {
                var product = GenerateProduct();
                shopDbContext.Products.Add(product);
            }
            
            count += await shopDbContext.SaveChangesAsync(cancellationToken);
        }
        
        logger.LogInformation("Created {Count} products.", count);
        return count;
    }

    private Product GenerateProduct() => new()
    {
        Name = faker.Commerce.ProductName(),
        Description = faker.Commerce.ProductDescription(),
        Price = decimal.Parse(faker.Commerce.Price()),
        MaxPrice = decimal.Parse(faker.Commerce.Price()),
        StockQuantity = faker.Random.Int(min: 0, max: 100),
        AveragePurchaseCount = faker.Random.Int(min: 0, max: 100),
        WidthInCentimeters = GetRandomFloat(1000),
        HeightInCentimeters = GetRandomFloat(1000),
        WeightInGrams = GetRandomFloat(1000),
        LengthInCentimeters = GetRandomFloat(1000),
        Volume = Math.Round(faker.Random.Double(max: 10), 2),
        Barcode = faker.Random.Long(min: 0),
        IsAvailable = faker.Random.Bool(),
        IsSalesEnded = faker.Random.Bool().OrNull(faker),
        CreatedDate = DateTime.UtcNow,
        UpdatedDate = DateTime.UtcNow,
        RemovedAt = faker.Date.Past().OrNull(faker),
        EndOfSalesDate = faker.Date.PastOffset().OrNull(faker),
        PreviousSupplyDate = faker.Date.PastDateOnly(),
        NextSupplyDate = faker.Date.FutureDateOnly().OrNull(faker),
        Category = faker.Random.Enum<Category>(),
        Supplier = new Supplier
        {
            Name = faker.Company.CompanyName(),
            City = faker.Address.City(),
            IsActive = faker.Random.Bool(),
            Director = new Director
            {
                Name = faker.Name.FullName(),
                Age = faker.Random.Int(min: 0, max: 100),
                IsActive = faker.Random.Bool(),
                Department = faker.Random.Enum<Department>(),
                Birthday = faker.Date.PastDateOnly(),
                DirectorSince = faker.Date.Past(),
                LastWorkDay = faker.Date.PastOffset().OrNull(faker),
                StartWorkTime = faker.Date.BetweenTimeOnly(new TimeOnly(7, 0), new TimeOnly(22, 0)),
                Address = faker.GenerateAddress().OrNull(faker),
                Company = new Company
                {
                    Name = faker.Company.CompanyName(),
                    EmployeeCount = faker.Random.Int(min: 1, max: 100000),
                }
            }
        }
    };

    private float GetRandomFloat(float max = 0)
    {
        return (float)Math.Round(faker.Random.Float(max: max), 2);
    }
}