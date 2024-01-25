namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Represents text field.
/// </summary>
public partial class TextField : CustomField
{
    /// <summary>
    /// Property value.
    /// </summary>
    public string? PropertyValue
    {
        get => EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance)?.ToString();
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value);
    }
}
