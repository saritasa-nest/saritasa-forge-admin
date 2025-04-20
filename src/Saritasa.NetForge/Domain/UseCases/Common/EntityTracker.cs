namespace Saritasa.NetForge.Domain.UseCases.Common;

/// <summary>
/// Entity tracker.
/// </summary>
public class EntityTracker
{
    /// <summary>
    /// Tracking object.
    /// </summary>
    public object TrackingObject { get; }

    /// <summary>
    /// Property errors related to <see cref="TrackingObject"/>.
    /// </summary>
    public IDictionary<string, string> PropertyErrors { get; } = new Dictionary<string, string>();

    /// <summary>
    /// Constructor.
    /// </summary>
    public EntityTracker(object trackingObject)
    {
        TrackingObject = trackingObject;
    }

    /// <inheritdoc cref="Saritasa.NetForge.Domain.Extensions.ReflectionExtensions.GetPropertyValue"/>
    public object? GetPropertyValue(string propertyName)
    {
        return TrackingObject.GetType().GetProperty(propertyName)?.GetValue(TrackingObject);
    }

    /// <inheritdoc cref="Saritasa.NetForge.Domain.Extensions.ReflectionExtensions.SetPropertyValue"/>
    public void SetPropertyValue(string propertyName, object? value)
    {
        var propertyInfo = TrackingObject.GetType().GetProperty(propertyName);
        ArgumentNullException.ThrowIfNull(propertyInfo);
        propertyInfo.SetValue(TrackingObject, value);
    }
}
