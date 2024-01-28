using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Helpers;
using Xunit;

namespace Saritasa.NetForge.Tests.EfCoreDataServiceTests;

/// <summary>
/// Tests for <see cref="IOrmDataService.UpdateAsync"/>.
/// </summary>
public class UpdateEntityTests : IDisposable
{
    private TestDbContext TestDbContext { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public UpdateEntityTests()
    {
        TestDbContext = EfCoreHelper.CreateTestDbContext();
        var contacts = Fakers.ContactInfoFaker.Generate(2);
        TestDbContext.ContactInfos.AddRange(contacts);
        TestDbContext.SaveChanges();
    }

    private bool disposedValue;

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Deletes the database after one test is complete,
    /// so it gives us the same state of the database for every test.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                TestDbContext.Dispose();
            }

            disposedValue = true;
        }
    }

    /// <summary>
    /// Create valid entity test.
    /// </summary>
    [Fact]
    public async Task UpdateEntity_ValidEntity_Success()
    {
        // Arrange
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(TestDbContext);

        var contactInfo = await TestDbContext.ContactInfos.FirstAsync();

        const string newEmail = "Test222@test.test";
        contactInfo.Email = newEmail;

        // Act
        await efCoreDataService.UpdateAsync(contactInfo, CancellationToken.None);

        // Assert
        Assert.Contains(TestDbContext.ContactInfos, contact => contact.Email.Equals(newEmail));
    }
}
