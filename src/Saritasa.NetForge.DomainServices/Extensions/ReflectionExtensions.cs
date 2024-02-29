namespace Saritasa.NetForge.DomainServices.Extensions;

/// <summary>
/// Contains methods that work with reflection.
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    /// Gets value of <paramref name="obj"/> property with given name.
    /// </summary>
    public static object? GetPropertyValue(this object? obj, string propertyName)
    {
        return obj?.GetType().GetProperty(propertyName)?.GetValue(obj);
    }
}
