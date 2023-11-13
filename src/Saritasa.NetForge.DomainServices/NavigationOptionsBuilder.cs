using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.DomainServices;

/// <summary>
/// Builder class for configuring entity's navigation options.
/// </summary>
public class NavigationOptionsBuilder
{
    private readonly NavigationOptions options = new();

    /// <summary>
    /// Creates navigation options.
    /// </summary>
    /// <param name="navigationName">Navigation name to create options for.</param>
    /// <returns>Navigation options.</returns>
    public NavigationOptions Create(string navigationName)
    {
        options.NavigationName = navigationName;

        return options;
    }

    /// <summary>
    /// Sets whether the navigation should be hidden from the view.
    /// </summary>
    public NavigationOptionsBuilder SetIsHidden(bool isHidden)
    {
        options.IsHidden = isHidden;
        return this;
    }

    /// <summary>
    /// Sets new display name to navigation.
    /// </summary>
    /// <param name="displayName">Name to display.</param>
    public NavigationOptionsBuilder SetDisplayName(string displayName)
    {
        options.DisplayName = displayName;
        return this;
    }

    /// <summary>
    /// Sets description to navigation.
    /// </summary>
    /// <param name="description">Description.</param>
    public NavigationOptionsBuilder SetDescription(string description)
    {
        options.Description = description;
        return this;
    }

    /// <summary>
    /// Sets order to navigation.
    /// </summary>
    /// <param name="order">Order number.</param>
    public NavigationOptionsBuilder SetOrder(int order)
    {
        options.Order = order;
        return this;
    }

    /// <summary>
    /// Set display format to navigation value.
    /// </summary>
    /// <param name="displayFormat">Display format.</param>
    public NavigationOptionsBuilder SetDisplayFormat(string displayFormat)
    {
        options.DisplayFormat = displayFormat;
        return this;
    }

    /// <summary>
    /// Set format provider to navigation value.
    /// </summary>
    /// <param name="formatProvider">Format provider instance.</param>
    public NavigationOptionsBuilder SetFormatProvider(IFormatProvider formatProvider)
    {
        options.FormatProvider = formatProvider;
        return this;
    }
}
