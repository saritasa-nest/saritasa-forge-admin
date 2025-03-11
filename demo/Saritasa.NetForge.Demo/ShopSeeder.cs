using Saritasa.NetForge.Demo.Models;

namespace Saritasa.NetForge.Demo;

public static class ShopSeeder
{
    public static void SeedData(this ShopDbContext context)
    {
        var supplier = new Supplier()
        {
            Name = "MySupplier",
            City = "My City, MC"
        };
        context.Suppliers.Add(supplier);
        var product = new Product
        {
            Name = "My product",
            Description = "My awesome description",
            Supplier = supplier,
            Price = 10,
            EndOfSalesDate = DateTimeOffset.UtcNow
        };

        context.Products.Add(product);
        context.SaveChanges();
    }
}