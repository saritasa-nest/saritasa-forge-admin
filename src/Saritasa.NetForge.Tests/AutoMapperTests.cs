using AutoMapper;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Saritasa.NetForge.Blazor.Infrastructure.DependencyInjection;

namespace Saritasa.NetForge.Tests;

/// <summary>
/// Test for AutoMapper configuration.
/// </summary>
public class AutoMapperTests
{
    /// <summary>
    /// Verify that automapper configuration is valid.
    /// </summary>
    [Fact]
    public void AutoMapper_Configuration_Valid()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        AutoMapperModule.Register(serviceCollection);

        // Act
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mapper = serviceProvider.GetRequiredService<IMapper>();

        // Assert
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
