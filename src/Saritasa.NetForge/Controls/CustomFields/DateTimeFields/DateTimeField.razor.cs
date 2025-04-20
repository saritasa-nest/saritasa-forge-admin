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
            var propertyValue = EntityTracker.GetPropertyValue(Property.Name)?.ToString();

            var isDateParsed = DateTime.TryParse(propertyValue, out var parsedDate);

            if (isDateParsed)
            {
                return parsedDate;
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
    private void SetPropertyValue(DateTime? value)
    {
        if (value == null)
        {
            if (Property.IsNullable)
            {
                EntityTracker.SetPropertyValue(Property.Name, null);
            }
            return;
        }

        var actualPropertyType = Nullable.GetUnderlyingType(Property.ClrType!) ?? Property.ClrType;
        if (actualPropertyType == typeof(DateTimeOffset))
        {
            EntityTracker.SetPropertyValue(Property.Name, new DateTimeOffset(value.Value));
        }
        else
        {
            EntityTracker.SetPropertyValue(Property.Name, value);
        }
    }
}
