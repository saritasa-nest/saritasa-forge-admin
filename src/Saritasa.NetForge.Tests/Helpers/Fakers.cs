using Bogus;
using Saritasa.NetForge.Tests.Domain.Models;

namespace Saritasa.NetForge.Tests.Helpers;

/// <summary>
/// Contains faker instances.
/// </summary>
internal static class Fakers
{
    /// <summary>
    /// Address faker.
    /// </summary>
    public static readonly Faker<Address> AddressFaker = new Faker<Address>()
        .RuleFor(a => a.Street, f => f.Address.StreetAddress())
        .RuleFor(a => a.City, f => f.Address.City())
        .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
        .RuleFor(a => a.Country, f => f.Address.Country())
        .RuleFor(a => a.Latitude, f => f.Address.Latitude())
        .RuleFor(a => a.Longitude, f => f.Address.Longitude())
        .RuleFor(a => a.ContactPhone, f => f.Phone.PhoneNumber());

    /// <summary>
    /// Contact Information faker.
    /// </summary>
    public static readonly Faker<ContactInfo> ContactInfoFaker = new Faker<ContactInfo>()
        .RuleFor(ci => ci.FullName, f => f.Name.FullName())
        .RuleFor(ci => ci.Email, (f, ci) => f.Internet.Email(ci.FullName))
        .RuleFor(ci => ci.PhoneNumber, f => f.Phone.PhoneNumber());

    /// <summary>
    /// Product faker.
    /// </summary>
    public static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
        .RuleFor(p => p.Price, f => f.Finance.Amount(10))
        .RuleFor(p => p.MaxPrice, (f, p) => f.Finance.Amount(p.Price + 10, 2000))
        .RuleFor(p => p.StockQuantity, f => f.Random.Int(0, 100))
        .RuleFor(p => p.AveragePurchaseCount, f => f.Random.Int(0, 100))
        .RuleFor(p => p.WeightInGrams, f => f.Random.Float(100, 2000))
        .RuleFor(p => p.LengthInCentimeters, f => f.Random.Float(10, 100))
        .RuleFor(p => p.WidthInCentimeters, f => f.Random.Float(10, 100))
        .RuleFor(p => p.HeightInCentimeters, f => f.Random.Float(10, 100))
        .RuleFor(p => p.Volume, f => f.Random.Double(1, 100))
        .RuleFor(p => p.Barcode, f => f.Random.Long(1000000000000, 99999999999999))
        .RuleFor(p => p.IsAvailable, f => f.Random.Bool())
        .RuleFor(p => p.IsSalesEnded, f => f.Random.Bool())
        .RuleFor(p => p.CreatedDate, f => f.Date.Recent(10))
        .RuleFor(p => p.UpdatedDate, (f, p) => f.Date.Future(refDate: p.CreatedDate))
        .RuleFor(p => p.RemovedAt, (f, p) => f.Random.Bool() ? f.Date.Future(refDate: p.UpdatedDate) : null)
        .RuleFor(p => p.EndOfSalesDate, (_, p) => p.IsSalesEnded ?? false ? p.UpdatedDate : null)
        .RuleFor(p => p.PreviousSupplyDate, f => f.Date.PastDateOnly())
        .RuleFor(p => p.NextSupplyDate, f => f.Date.FutureDateOnly())
        .RuleFor(p => p.Category, f => f.PickRandom(Enum.GetValues(typeof(Category)).Cast<Category>()))
        .RuleFor(p => p.Supplier, _ => SupplierFaker!.Generate())
        .RuleFor(p => p.Tags, f => f.Make(3, () => ProductTagFaker!.Generate()));

    /// <summary>
    /// Product Tag faker.
    /// </summary>
    public static readonly Faker<ProductTag> ProductTagFaker = new Faker<ProductTag>()
        .RuleFor(pt => pt.Name, f => f.Random.Word())
        .RuleFor(pt => pt.Description, f => f.Lorem.Paragraph());

    /// <summary>
    /// Shop faker.
    /// </summary>
    public static readonly Faker<Shop> ShopFaker = new Faker<Shop>()
        .RuleFor(s => s.Name, f => f.Company.CompanyName())
        .RuleFor(s => s.Address, _ => AddressFaker.Generate())
        .RuleFor(s => s.OpenedDate, f => f.Date.Past(5))
        .RuleFor(s => s.TotalSales, f => f.Finance.Amount(1000, 1000000))
        .RuleFor(s => s.IsOpen, f => f.Random.Bool())
        .RuleFor(s => s.Products, (f, _) => f.Make(3, () => ProductFaker.Generate()))
        .RuleFor(s => s.OwnerContact, _ => ContactInfoFaker.Generate())
        .RuleFor(s => s.Suppliers, f => f.Make(3, () => SupplierFaker!.Generate()));

    /// <summary>
    /// Supplier faker.
    /// </summary>
    public static readonly Faker<Supplier> SupplierFaker = new Faker<Supplier>()
        .RuleFor(s => s.Name, f => f.Company.CompanyName())
        .RuleFor(s => s.City, f => f.Address.City())
        .RuleFor(s => s.IsActive, f => f.Random.Bool());

    /// <summary>
    /// User faker.
    /// </summary>
    public static readonly Faker<User> UserFaker = new Faker<User>()
        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
        .RuleFor(u => u.LastName, f => f.Name.LastName())
        .RuleFor(u => u.UserName, f => f.Internet.UserName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.DateOfBirth, f => f.Person.DateOfBirth.Date.ToUniversalTime())
        .RuleFor(u => u.EmailConfirmed, f => f.Random.Bool())
        .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("###-###-#####"))
        .RuleFor(u => u.PhoneNumberConfirmed, f => f.Random.Bool())
        .RuleFor(u => u.PasswordHash, f => f.Internet.Password())
        .RuleFor(u => u.SecurityStamp, f => f.Internet.Password());
}
