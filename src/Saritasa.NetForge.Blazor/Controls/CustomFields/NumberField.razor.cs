namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Represents number field.
/// </summary>
public partial class NumberField<T> : CustomField
{
    /// <summary>
    /// Property value.
    /// </summary>
    public T? PropertyValue
    {
        get => (T?)EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance);
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value);
    }
}
