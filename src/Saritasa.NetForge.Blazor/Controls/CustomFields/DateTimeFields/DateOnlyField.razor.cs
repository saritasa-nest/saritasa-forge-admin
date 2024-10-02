namespace Saritasa.NetForge.Blazor.Controls.CustomFields.DateTimeFields;

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
            var propertyValue = EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance)?.ToString();

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
        var property = EntityInstance.GetType().GetProperty(Property.Name)!;

        if (!value.HasValue)
        {
            property.SetValue(EntityInstance, null);
            return;
        }

        property.SetValue(EntityInstance, DateOnly.FromDateTime(value.Value));
    }
}
