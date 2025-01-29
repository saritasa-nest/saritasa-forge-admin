using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Demo.Models;

namespace Saritasa.NetForge.Demo;

/// <summary>
/// Represents the database context for the shop.
/// </summary>
public class ShopDbContext : IdentityDbContext<User>
{
    static ShopDbContext()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShopDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the database set for the shops.
    /// </summary>
    public DbSet<Shop> Shops { get; private set; }

    /// <summary>
    /// Gets or sets the database set for the addresses.
    /// </summary>
    public DbSet<Address> Addresses { get; private set; }

    /// <summary>
    /// Gets or sets the database set for the products.
    /// </summary>
    public DbSet<Product> Products { get; private set; }

    /// <summary>
    /// Gets or sets the database set for the product tags.
    /// </summary>
    public DbSet<ProductTag> ProductTags { get; private set; }

    /// <summary>
    /// Gets or sets the database set for the contact information.
    /// </summary>
    public DbSet<ContactInfo> ContactInfos { get; private set; }

    /// <summary>
    /// Gets or sets the database set for the suppliers.
    /// </summary>
    public DbSet<Supplier> Suppliers { get; private set; }

    /// <summary>
    /// Gets or sets the database set for the counts of shop products.
    /// </summary>
    public DbSet<ShopProductsCount> ShopProductsCounts { get; private set; }
    
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ForceHavingAllStringsAsVarchars(modelBuilder);
        
        modelBuilder.Entity<Product>()
            .ToTable(options => options.HasComment("Represents single product in the Shop."));

        modelBuilder.Entity<Address>()
            .Property(address => address.DisplayName)
            .HasComputedColumnSql("city || ', ' || street", stored: true);
    }

    private static void ForceHavingAllStringsAsVarchars(ModelBuilder modelBuilder)
    {
        var stringColumns = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.ClrType == typeof(string));
        
        foreach (var mutableProperty in stringColumns)
        {
            mutableProperty.SetIsUnicode(false);
        }
    }
}
