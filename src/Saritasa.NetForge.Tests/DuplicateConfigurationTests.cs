using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Infrastructure.Admin;
using Xunit;

namespace Saritasa.NetForge.Tests;

/// <summary>
/// Duplicate entity options configuration tests.
/// </summary>
public class DuplicateEntityOptionsConfigurationTests
{
    /// <summary>
    /// Verify that configure entity with both fluent API and configuration class only create 1 instance of the entity options.
    /// </summary>
    [Fact]
    public void ConfigureEntity_WithFluentApiAndConfiguration_ShouldNotCreateDuplicateOptions()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();
        const string mockDescription = "This is a description";

        // Act
        adminOptionsBuilder.ConfigureEntity<User>(builder =>
        {
            builder.SetDescription(mockDescription);
        }).ConfigureEntity(new UserAdminConfiguration());

        // Assert
        var adminOptions = adminOptionsBuilder.Create();
        var userDescription = adminOptions.EntityOptionsList.FirstOrDefault(x => x.EntityType == typeof(User))
            ?.Description;
        Assert.Equal(mockDescription, userDescription);

        // Ensure that only one instance of entity options is created
        var entityOptionsCount = adminOptions.EntityOptionsList.Count;
        Assert.Equal(1, entityOptionsCount);
    }
}
