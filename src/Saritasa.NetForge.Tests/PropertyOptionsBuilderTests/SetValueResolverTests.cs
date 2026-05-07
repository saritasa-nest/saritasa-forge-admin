using Saritasa.NetForge.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Xunit;

namespace Saritasa.NetForge.Tests.PropertyOptionsBuilderTests;

/// <summary>
/// <see cref="PropertyOptionsBuilder.SetValueResolver"/> tests.
/// </summary>
public class SetValueResolverTests
{
    /// <summary>
    /// Verify that the converter works with a custom strongly-typed ID.
    /// </summary>
    [Fact]
    public void SetValueResolver_CustomStronglyTypedId_ShouldConvert()
    {
        // Arrange
        var builder = new PropertyOptionsBuilder();
        var options = builder.Create("TestProperty");

        builder.SetValueResolver(value => int.TryParse(value, out var id) ? new TokenId(id) : default);

        // Act
        var resolvedValue = options.ValueResolver?.Invoke("99");

        // Assert
        Assert.IsType<TokenId>(resolvedValue);
        Assert.Equal(new TokenId(99), resolvedValue);
    }
}
