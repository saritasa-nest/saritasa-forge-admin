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
        get => EntityTracker.GetPropertyValue(Property.Name)?.ToString();
        set
        {
            var propertyType = Property.ClrType!;
            var actualPropertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            var enumValue = Enum.Parse(actualPropertyType, value!);
            EntityTracker.SetPropertyValue(Property.Name, enumValue);
        }
    }
}
