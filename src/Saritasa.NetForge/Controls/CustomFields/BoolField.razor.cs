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
        get => EntityTracker.GetPropertyValue(Property.Name) as bool?;
        set => EntityTracker.SetPropertyValue(Property.Name, value);
    }
}
