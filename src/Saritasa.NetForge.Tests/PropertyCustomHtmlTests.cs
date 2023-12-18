using Saritasa.NetForge.Domain.Attributes;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.Tests.Domain.Models;
using Xunit;

namespace Saritasa.NetForge.Tests;

/// <summary>
/// Custom HTML for entity properties tests.
/// </summary>
public class PropertyCustomHtmlTests
{
    /// <summary>
    /// Configure entity property through fluent API test.
    /// </summary>
    [Fact]
    public void ConfigureProperty_WithCustomHtml_ThroughFluentApi_Success()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();

        // Act
        adminOptionsBuilder.ConfigureEntity<User>(builder =>
        {
            builder.ConfigureProperty(user => user.FirstName, propBuilder => propBuilder.SetDisplayAsHtml(true));
        });

        // Assert
        var adminOptions = adminOptionsBuilder.Create();
        var userEntity = adminOptions.EntityOptionsList.FirstOrDefault(x => x.EntityType == typeof(User));
        var userFirstNameAllowedHtml = userEntity?.PropertyOptions.FirstOrDefault(x => x.PropertyName == "FirstName")?.DisplayAsHtml;

        Assert.True(userFirstNameAllowedHtml);
    }

    /// <summary>
    /// Configure entity property through attribute configuration test.
    /// </summary>
    [Fact]
    public void ConfigureProperty_WithCustomHtml_ThroughAttribute_Success()
    {
        // Arrange
        var propertyInfo = typeof(Address).GetProperty(nameof(Address.Id));
        var attribute = propertyInfo?.GetCustomAttributes(typeof(NetForgePropertyAttribute), false)
            .SingleOrDefault() as NetForgePropertyAttribute;

        // Assert
        Assert.NotNull(attribute);
        Assert.True(attribute.DisplayAsHtml);
    }
}
