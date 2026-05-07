using Saritasa.NetForge.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Xunit;

namespace Saritasa.NetForge.Tests.PropertyOptionsBuilderTests;

/// <summary>
/// <see cref="PropertyOptionsBuilder.SetConverter"/> tests.
/// </summary>
public class SetConverterTests
{
    /// <summary>
    /// Verify that the converter works with a custom strongly-typed ID.
    /// </summary>
    [Fact]
    public void SetConverter_CustomStronglyTypedId_ShouldConvert()
    {
        // Arrange
        var builder = new PropertyOptionsBuilder();
        var options = builder.Create("TestProperty");

        builder.SetConverter(value => int.TryParse(value, out var id) ? new TokenId(id) : default);

        // Act
        var converted = options.Converter?.Invoke("99");

        // Assert
        Assert.IsType<TokenId>(converted);
        Assert.Equal(new TokenId(99), converted);
    }
}

