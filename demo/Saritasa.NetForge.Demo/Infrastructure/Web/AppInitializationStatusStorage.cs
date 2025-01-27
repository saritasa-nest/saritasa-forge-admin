namespace Saritasa.NetForge.Demo.Infrastructure.Web;

/// <summary>
/// Contains an info about current status of initialization process.
/// </summary>
public class AppInitializationStatusStorage
{
    /// <summary>
    /// Whether application has already initialized or not.
    /// </summary>
    public bool HasAppInitialized { get; set; } = false;
}
