using Saritasa.NetForge.Domain.Extensions;

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
            if (Property.IsNullable)
            {
                EntityInstance.SetNestedPropertyValue(Property.PropertyPath, null);
            }
            else
            {
                EntityInstance.SetNestedPropertyValue(Property.PropertyPath, default(DateOnly));
            }
            return;
        }

        EntityInstance.SetNestedPropertyValue(Property.PropertyPath, DateOnly.FromDateTime(value.Value));
    }
}
