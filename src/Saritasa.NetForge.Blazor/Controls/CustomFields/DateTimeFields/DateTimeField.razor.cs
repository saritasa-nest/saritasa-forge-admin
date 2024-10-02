namespace Saritasa.NetForge.Blazor.Controls.CustomFields.DateTimeFields;

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

        var actualPropertyType = Nullable.GetUnderlyingType(Property.ClrType!) ?? Property.ClrType;
        if (actualPropertyType == typeof(DateTimeOffset))
        {
            property.SetValue(EntityInstance, new DateTimeOffset(value.Value));
        }
        else
        {
            property.SetValue(EntityInstance, value);
        }
    }
}

