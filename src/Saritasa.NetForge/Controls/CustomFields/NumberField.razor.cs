using Saritasa.NetForge.Domain.Extensions;

namespace Saritasa.NetForge.Controls.CustomFields;

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
        get => (T?)EntityInstance.GetNestedPropertyValue(Property.PropertyPath);
        set => EntityInstance.SetNestedPropertyValue(Property.PropertyPath, value);
    }
}
