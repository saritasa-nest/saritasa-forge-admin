using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.EfCoreMetadataServiceTests.NavigationMetadata.Models;
using StronglyTypedIds;

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
    /// Gets or sets the database set for the tokens.
    /// </summary>
    public DbSet<Token> Tokens { get; private set; } = null!;

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

        modelBuilder.Entity<Token>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Shop>()
            .HasOne(s => s.Token)
            .WithOne()
            .HasForeignKey<Shop>(s => s.TokenId)
            .IsRequired(false);

        ConfigureStronglyTypedIds(modelBuilder);
    }

    private static readonly ConcurrentDictionary<Type, ValueConverter> StronglyTypedIdConverters = new();

    private static void ConfigureStronglyTypedIds(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                var actualPropertyType = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;
                var stronglyTypedIdAttribute = actualPropertyType.GetCustomAttribute<StronglyTypedIdAttribute>();
                if (stronglyTypedIdAttribute is null)
                {
                    continue;
                }

                var converter = StronglyTypedIdConverters.GetOrAdd(
                    property.ClrType,
                    _ => CreateStronglyTypedIdConverter(actualPropertyType));

                property.SetValueConverter(converter);

                // By default, strongly typed ids do not use value generation.
                // So we add generation for primary keys.
                // If you want value generation for other property, you will need to configure it separately.
                if (property.IsPrimaryKey())
                {
                    // We have a special case when primary key is a composite key,
                    // in this situation it would also be a foreign key
                    // In this case we should not be automatically generating a value for it.
                    //
                    // Also, sometimes we might have GUID PKs generated
                    // on client (via Guid.CreateVersion7() or even sent by Frontend).
                    // In that case, ValueGeneratedNever() must be set explicitly in EntityConfiguration.
                    // But this code will run after entity configurations, so we need to check
                    // that converter.ProviderClrType != typeof(Guid)
                    // to avoid reverting EntityConfiguration changes.
                    // String-backed IDs are also excluded: identity generation only works with signed integer columns.
                    if (property.ValueGenerated == ValueGenerated.Never &&
                        !property.IsForeignKey() &&
                        converter.ProviderClrType != typeof(Guid) &&
                        converter.ProviderClrType != typeof(string))
                    {
                        property.ValueGenerated = ValueGenerated.OnAdd;
                    }
                }
            }
        }
    }

    private static ValueConverter CreateStronglyTypedIdConverter(Type stronglyTypedIdType)
    {
        // id => id.Value
        var stronglyTypedIdParam = Expression.Parameter(stronglyTypedIdType, "id");
        var valueProperty = Expression.Property(stronglyTypedIdParam, "Value");
        var toProviderExpression = Expression.Lambda(valueProperty, stronglyTypedIdParam);

        var valuePropertyInfo = (PropertyInfo)valueProperty.Member;
        var valueType = valuePropertyInfo.PropertyType;

        // Example of expression:
        // value => new UserId(value)
        var valueParam = Expression.Parameter(valueType, "value");
        var ctor = stronglyTypedIdType.GetConstructor([valueType]);
        var fromProviderExpression = Expression.Lambda(Expression.New(ctor!, valueParam), valueParam);

        var converterType = typeof(ValueConverter<,>).MakeGenericType(stronglyTypedIdType, valueType);

        var converter = Activator.CreateInstance(converterType, toProviderExpression, fromProviderExpression, null);

        return (ValueConverter)converter!;
    }
}

