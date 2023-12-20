using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Saritasa.NetForge.Blazor.Infrastructure.DependencyInjection;

namespace Saritasa.NetForge.Tests.Helpers;

/// <summary>
/// Automapper helper.
/// </summary>
internal static class AutomapperHelper
{
    /// <summary>
    /// Creates automapper.
    /// </summary>
    internal static IMapper CreateAutomapper()
    {
        var serviceCollection = new ServiceCollection();
        AutoMapperModule.Register(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IMapper>();
    }
}
