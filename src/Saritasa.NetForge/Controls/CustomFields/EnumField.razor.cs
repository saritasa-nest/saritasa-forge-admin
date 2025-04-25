using Saritasa.NetForge.Domain.Extensions;

namespace Saritasa.NetForge.Controls.CustomFields;

/// <summary>
/// Represents enum field.
/// </summary>
public partial class EnumField : CustomField
{
    /// <summary>
    /// Property value.
    /// </summary>
    public string? PropertyValue
    {
        get => EntityInstance.GetNestedPropertyValue(Property.PropertyPath)?.ToString();
        set
        {
            var propertyType = Property.ClrType!;
            var actualPropertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            var enumValue = Enum.Parse(actualPropertyType, value!);
            EntityInstance.SetNestedPropertyValue(Property.PropertyPath, enumValue);
        }
    }
}
