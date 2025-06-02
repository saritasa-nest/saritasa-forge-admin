using Saritasa.NetForge.Domain.Extensions;

namespace Saritasa.NetForge.Controls.CustomFields.DateTimeFields;

/// <summary>
/// Represents TimeOnly field.
/// </summary>
public partial class TimeOnlyField : CustomField
{
    /// <summary>
    /// Property time value.
    /// </summary>
    public TimeSpan? PropertyValue
    {
        get
        {
            var propertyValue = EntityInstance.GetNestedPropertyValue(Property.PropertyPath)?.ToString();

            var isTimeParsed = DateTime.TryParse(propertyValue, out var dateTime);

            if (isTimeParsed)
            {
                return dateTime.TimeOfDay;
            }

            return null;
        }

        set => SetPropertyValue(value);
    }

    /// <summary>
    /// Handles input changes.
    /// </summary>
    /// <param name="value">Input value.</param>
    private void SetPropertyValue(TimeSpan? value)
    {
        if (!value.HasValue)
        {
            if (Property.IsNullable)
            {
                EntityInstance.SetNestedPropertyValue(Property.PropertyPath, null);
            }
            else
            {
                EntityInstance.SetNestedPropertyValue(Property.PropertyPath, default(TimeOnly));
            }
            return;
        }

        if (Property.ClrType == typeof(TimeOnly) || Property.ClrType == typeof(TimeOnly?))
        {
            EntityInstance.SetNestedPropertyValue(Property.PropertyPath, TimeOnly.FromTimeSpan(value.Value));
        }
        else
        {
            EntityInstance.SetNestedPropertyValue(Property.PropertyPath, value);
        }
    }
}
