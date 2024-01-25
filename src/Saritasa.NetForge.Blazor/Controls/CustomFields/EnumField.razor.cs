namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

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
        get => EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance)?.ToString();
        set
        {
            var propertyType = Property.ClrType!;
            var actualPropertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            var enumValue = Enum.Parse(actualPropertyType, value!);
            EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, enumValue);
        }
    }
}
