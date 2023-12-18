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

        // Act
        adminOptionsBuilder.ConfigureEntity(new UserAdminConfiguration())
        .ConfigureEntity<User>(builder =>
        {
            builder.SetIsHidden(true);
        });

        // Assert
        var adminOptions = adminOptionsBuilder.Create();
        var entityOptionsCount = adminOptions.EntityOptionsList.Count;
        Assert.Equal(1, entityOptionsCount);
    }

    /// <summary>
    /// Verify that configure entity with both fluent API and configuration class will take the second configuration values.
    /// </summary>
    [Fact]
    public void ConfigureEntity_WithFluentApiAndConfiguration_ShouldCreateOverrideValue()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();
        const string mockDescription = "This is an override description";

        // Act
        adminOptionsBuilder.ConfigureEntity(new UserAdminConfiguration())
        .ConfigureEntity<User>(builder =>
        {
            builder.SetDescription(mockDescription);
        });

        // Assert
        var adminOptions = adminOptionsBuilder.Create();
        var userDescription = adminOptions.EntityOptionsList.FirstOrDefault(x => x.EntityType == typeof(User))
            ?.Description;
        Assert.Equal(mockDescription, userDescription);
    }
}
