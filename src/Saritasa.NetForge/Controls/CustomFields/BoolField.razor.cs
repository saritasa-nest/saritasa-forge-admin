namespace Saritasa.NetForge.Controls.CustomFields;

/// <summary>
/// Represents bool field.
/// </summary>
public partial class BoolField : CustomField
{
    /// <summary>
    /// Property value.
    /// </summary>
    public bool? PropertyValue
    {
        get => (bool?)EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance);
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value);
    }
}
