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
        var actualPropertyType = Nullable.GetUnderlyingType(Property.ClrType!) ?? Property.ClrType;
        var isOffset = actualPropertyType == typeof(DateTimeOffset);
        if (!value.HasValue)
        {
            if (Property.IsNullable)
            {
                EntityInstance.SetNestedPropertyValue(Property.PropertyPath, null);
            }
            else
            {
                EntityInstance
                    .SetNestedPropertyValue(Property.PropertyPath, isOffset ? default(DateTimeOffset) : default(DateTime));
            }
            return;
        }

        if (isOffset)
        {
            EntityInstance.SetNestedPropertyValue(Property.PropertyPath, new DateTimeOffset(value.Value));
        }
        else
        {
            EntityInstance.SetNestedPropertyValue(Property.PropertyPath, value);
        }
    }
}
