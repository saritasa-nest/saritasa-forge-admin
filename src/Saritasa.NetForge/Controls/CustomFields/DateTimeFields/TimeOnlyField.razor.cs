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
            var propertyValue = EntityTracker.GetPropertyValue(Property.Name)?.ToString();

            var isTimeParsed = DateTime.TryParse(propertyValue, out var dateTime);

            if (isTimeParsed)
            {
                return dateTime.TimeOfDay;
            }

            return null;
        }

        set
        {
            SetPropertyValue(value);
        }
    }

    /// <summary>
    /// Handles input changes.
    /// </summary>
    /// <param name="value">Input value.</param>
    private void SetPropertyValue(TimeSpan? value)
    {
        if (value == null)
        {
            if (Property.IsNullable)
            {
                EntityTracker.SetPropertyValue(Property.Name, null);
            }
            return;
        }

        if (Property.ClrType == typeof(TimeOnly) || Property.ClrType == typeof(TimeOnly?))
        {
            EntityTracker.SetPropertyValue(Property.Name, TimeOnly.FromTimeSpan(value.Value));
        }
        else
        {
            EntityTracker.SetPropertyValue(Property.Name, value);
        }
    }
}
