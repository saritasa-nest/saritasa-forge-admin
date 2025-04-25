using Saritasa.NetForge.Domain.Extensions;

namespace Saritasa.NetForge.Controls.CustomFields.DateTimeFields;

/// <summary>
/// Represents DateTime field.
/// </summary>
public partial class DateTimeField : CustomField
{
    /// <summary>
    /// Property date value.
    /// </summary>
    public DateTime? PropertyValue
    {
        get
        {
            var propertyValue = EntityInstance.GetNestedPropertyValue(Property.PropertyPath)?.ToString();

            var isDateParsed = DateTime.TryParse(propertyValue, out var parsedDate);

            if (isDateParsed)
            {
                return parsedDate;
            }

            return null;
        }

        set => SetPropertyValue(value);
    }

    /// <summary>
    /// Handles input changes.
    /// </summary>
    /// <param name="value">Input value.</param>
    private void SetPropertyValue(DateTime? value)
    {
        if (!value.HasValue)
        {
            EntityInstance.SetNestedPropertyValue(Property.PropertyPath, null);
            return;
        }

        var actualPropertyType = Nullable.GetUnderlyingType(Property.ClrType!) ?? Property.ClrType;
        if (actualPropertyType == typeof(DateTimeOffset))
        {
            EntityInstance.SetNestedPropertyValue(Property.PropertyPath, new DateTimeOffset(value.Value));
        }
        else
        {
            EntityInstance.SetNestedPropertyValue(Property.PropertyPath, value);
        }
    }
}
