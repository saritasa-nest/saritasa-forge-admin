using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Saritasa.NetForge.Tests.Utilities.Helpers;

/// <summary>
/// <see cref="IMemoryCache"/> helper.
/// </summary>
internal static class MemoryCacheHelper
{
    /// <summary>
    /// Creates <see cref="IMemoryCache"/>.
    /// </summary>
    /// <returns></returns>
    internal static IMemoryCache CreateMemoryCache()
    {
        var memoryCache = new Mock<IMemoryCache>();

        // Used in the underlying code of the memory cache implementation
        var cacheEntry = Mock.Of<ICacheEntry>();

        memoryCache
            .Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry);

        return memoryCache.Object;
    }
}
