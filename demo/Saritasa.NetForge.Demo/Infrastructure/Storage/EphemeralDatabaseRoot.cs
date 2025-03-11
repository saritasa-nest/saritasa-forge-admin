using System.Collections.Concurrent;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

internal class EphemeralDatabaseRoot
{
    public static ConcurrentDictionary<HttpContext, EphemeralStorage> Stores { get; } = new();
}