namespace Saritasa.NetForge.Controls.CustomFields.DateTimeFields;

/// <summary>
/// Represents DateOnly field.
/// </summary>
public partial class DateOnlyField : CustomField
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

        EntityTracker.SetPropertyValue(Property.Name, DateOnly.FromDateTime(value.Value));
    }
}
