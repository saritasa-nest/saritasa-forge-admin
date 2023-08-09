using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Demo.NetCore7.Models;

namespace Saritasa.NetForge.Demo.NetCore7.DbContexts;

/// <summary>
/// Represents the database context for the shop.
/// </summary>
public class ShopDbContext : DbContext
{
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
    public DbSet<Shop> Shops { get; set; }

    /// <summary>
    /// Gets or sets the database set for the addresses.
    /// </summary>
    public DbSet<Address> Addresses { get; set; }

    /// <summary>
    /// Gets or sets the database set for the products.
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Gets or sets the database set for the product tags.
    /// </summary>
    public DbSet<ProductTag> ProductTags { get; set; }

    /// <summary>
    /// Gets or sets the database set for the contact information.
    /// </summary>
    public DbSet<ContactInfo> ContactInfos { get; set; }
}

