using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.DomainServices;

/// <summary>
/// Builder class for configuring entity's property options.
/// </summary>
public class EntityPropertyOptionsBuilder
{
    private readonly EntityPropertyOptions options = new();

    /// <summary>
    /// Creates property options.
    /// </summary>
    /// <param name="propertyName">Property name to create options for.</param>
    /// <returns>Property options.</returns>
    public EntityPropertyOptions Create(string propertyName)
    {
        options.PropertyName = propertyName;

        return options;
    }

    /// <summary>
    /// Sets whether the property should be hidden from the view.
    /// </summary>
    public EntityPropertyOptionsBuilder SetIsHidden(bool isHidden)
    {
        options.IsHidden = isHidden;
        return this;
    }

    /// <summary>
    /// Sets new display name to property.
    /// </summary>
    /// <param name="displayName">Name to display.</param>
    public EntityPropertyOptionsBuilder SetDisplayName(string displayName)
    {
        options.DisplayName = displayName;
        return this;
    }

    /// <summary>
    /// Sets description to property. Displayed as tooltip when user hovering corresponding property.
    /// </summary>
    /// <param name="description">Description.</param>
    public EntityPropertyOptionsBuilder SetDescription(string description)
    {
        options.Description = description;
        return this;
    }
}
