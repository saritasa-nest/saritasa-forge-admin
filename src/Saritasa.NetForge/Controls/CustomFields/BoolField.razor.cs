using Saritasa.NetForge.Domain.Extensions;

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
        get => (bool?)EntityInstance.GetNestedPropertyValue(Property.PropertyPath);
        set => EntityInstance.SetNestedPropertyValue(Property.PropertyPath, value);
    }
}
