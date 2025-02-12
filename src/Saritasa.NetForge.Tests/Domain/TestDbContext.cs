using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.EfCoreMetadataServiceTests.NavigationMetadata.Entities;

namespace Saritasa.NetForge.Tests.Domain;

/// <summary>
/// Database context for tests.
/// </summary>
internal class TestDbContext : IdentityDbContext<User>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the database set for the shops.
    /// </summary>
    public DbSet<Shop> Shops { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the database set for the addresses.
    /// </summary>
    public DbSet<Address> Addresses { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the database set for the products.
    /// </summary>
    public DbSet<Product> Products { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the database set for the product tags.
    /// </summary>
    public DbSet<ProductTag> ProductTags { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the database set for the contact information.
    /// </summary>
    public DbSet<ContactInfo> ContactInfos { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the database set for the suppliers.
    /// </summary>
    public DbSet<Supplier> Suppliers { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the database set for the employees.
    /// </summary>
    public DbSet<Employee> Employees { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the database set for the departments.
    /// </summary>
    public DbSet<Department> Departments { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the database set for the projects.
    /// </summary>
    public DbSet<Project> Projects { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the database set for the clients.
    /// </summary>
    public DbSet<Client> Clients { get; private set; } = null!;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Supplier>()
            .HasKey(c => new { c.Name, c.City });
    }
}
