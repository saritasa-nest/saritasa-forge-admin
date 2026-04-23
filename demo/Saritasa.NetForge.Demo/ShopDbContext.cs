using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Saritasa.NetForge.Demo.Models;
using StronglyTypedIds;

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

    /// <summary>
    /// Gets or sets the database set for the tokens.
    /// </summary>
    public DbSet<Token> Tokens { get; private set; }
    
    /// <inheritdoc />
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
        // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
        // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
        // use the DateTimeOffsetToBinaryConverter
        // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
        // This only supports millisecond precision, but should be sufficient for most use cases.
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            configurationBuilder
                .Properties<DateTimeOffset>()
                .HaveConversion<DateTimeOffsetToBinaryConverter>();
            configurationBuilder
                .Properties<DateTimeOffset?>()
                .HaveConversion<DateTimeOffsetToBinaryConverter>();
        }
    }

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

        modelBuilder.Entity<Token>().HasKey(token => token.Id);

        ConfigureStronglyTypedIds(modelBuilder);
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
                        // property.ValueGenerated = ValueGenerated.OnAdd;
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
