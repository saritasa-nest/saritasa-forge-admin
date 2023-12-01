using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Demo.Net7;
using Saritasa.NetForge.Demo.Net7.Models;

namespace Saritasa.NetForge.Tests.Utilities;

/// <summary>
/// Fixture of the test database.
/// </summary>
public class TestDatabaseFixture : IDisposable
{
    private const string ConnectionString = "Server=localhost;Database=NetForgeTest;Uid=postgres;Pwd=cfif5555;";

    private static readonly object initDatabaseLock = new();
    private static bool databaseInitialized;

    /// <summary>
    /// Constructor.
    /// </summary>
    public TestDatabaseFixture()
    {
        lock (initDatabaseLock)
        {
            if (!databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    context.AddRange(
                        new ContactInfo
                        {
                            Id = 1,
                            Email = "Test@test.test",
                            FullName = "Test Contact",
                            PhoneNumber = "12223334455"
                        },
                        new ContactInfo
                        {
                            Id = 2,
                            Email = "Test2@test.test",
                            FullName = "Test Contact2",
                            PhoneNumber = "22223334455"
                        });

                    context.SaveChanges();
                }

                databaseInitialized = true;
            }
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (databaseInitialized)
        {
            databaseInitialized = false;
        }
    }

    /// <summary>
    /// Creates test <see cref="ShopDbContext"/>.
    /// </summary>
    /// <returns><see cref="ShopDbContext"/> populated with test data.</returns>
    public ShopDbContext CreateContext()
    {
        return new ShopDbContext(
            new DbContextOptionsBuilder<ShopDbContext>()
                .UseNpgsql(ConnectionString)
                .Options);
    }
}
