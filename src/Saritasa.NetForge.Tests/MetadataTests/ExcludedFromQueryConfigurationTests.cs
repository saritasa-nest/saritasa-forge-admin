using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.Tests.Domain.Models;
using Xunit;

namespace Saritasa.NetForge.Tests.MetadataTests;

/// <summary>
/// Exclude property from entity when query configuration tests.
/// </summary>
public class ExcludedFromQueryConfigurationTests
{
    /// <summary>
    /// Verify that specific property should be excluded from query when configured.
    /// </summary>
    [Fact]
    public void AdminOptions_ConfigureEntityPropertyToBeExcluded_ShouldBeExcluded()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();
        var adminOptions = adminOptionsBuilder.Create();

        // Act
        adminOptionsBuilder.ConfigureEntity<User>(entityOptionsBuilder =>
        {
            entityOptionsBuilder.ConfigureProperty(user => user.DateOfBirth,
                propertyBuilder => propertyBuilder.SetIsExcludedFromQuery(true));
        });

        // Assert
        Assert.True(adminOptions.EntityOptionsList.FirstOrDefault(x => x.EntityType == typeof(User))!.PropertyOptions
            .FirstOrDefault(x => x.PropertyName == nameof(User.DateOfBirth))!.IsExcludedFromQuery);
    }

    /// <summary>
    /// Verify that specific property should not be excluded from query when configured.
    /// </summary>
    [Fact]
    public void AdminOptions_ConfigureEntityPropertyToBeNotExcluded_ShouldNotBeExcluded()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();
        var adminOptions = adminOptionsBuilder.Create();

        // Act
        adminOptionsBuilder.ConfigureEntity<User>(entityOptionsBuilder =>
        {
            entityOptionsBuilder.ConfigureProperty(user => user.DateOfBirth,
                propertyBuilder => propertyBuilder.SetIsExcludedFromQuery(false));
        });

        // Assert
        Assert.False(adminOptions.EntityOptionsList.FirstOrDefault(x => x.EntityType == typeof(User))!.PropertyOptions
            .FirstOrDefault(x => x.PropertyName == nameof(User.DateOfBirth))!.IsExcludedFromQuery);
    }
}
