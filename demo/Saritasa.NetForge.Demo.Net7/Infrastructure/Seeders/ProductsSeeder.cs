﻿using Bogus;
using Saritasa.NetForge.Demo.Net7.Models;

namespace Saritasa.NetForge.Demo.Net7.Infrastructure.Seeders;

/// <summary>
/// Products seeder.
/// </summary>
internal class ProductsSeeder
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
                var product = GenerateProduct();
                shopDbContext.Products.Add(product);
            }
            
            count += await shopDbContext.SaveChangesAsync(cancellationToken);
        }
        
        logger.LogInformation("Created {count} products.", count);
        return count;
    }

    private Product GenerateProduct() => new()
    {
        Name = faker.Commerce.ProductName(),
        Description = faker.Commerce.ProductDescription(),
        Price = decimal.Parse(faker.Commerce.Price()),
        StockQuantity = faker.Random.Int(min: 0, max: 100),
        WidthInCentimeters = GetRandomFloat(1000),
        HeightInCentimeters = GetRandomFloat(1000),
        WeightInGrams = GetRandomFloat(1000),
        LengthInCentimeters = GetRandomFloat(1000),
        CreatedDate = DateTime.UtcNow,
        UpdatedDate = DateTime.UtcNow,
    };

    private float GetRandomFloat(float max = 0)
    {
        return (float)Math.Round(faker.Random.Float(max: max), 2);
    }
}