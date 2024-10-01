using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields.DateTimeFields;

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
            var propertyValue = EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance)?.ToString();

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

    [Inject]
    private AdminOptions AdminOptions { get; init; } = null!;

    /// <summary>
    /// Handles input changes.
    /// </summary>
    /// <param name="value">Input value.</param>
    private void SetPropertyValue(TimeSpan? value)
    {
        var property = EntityInstance.GetType().GetProperty(Property.Name)!;

        if (!value.HasValue)
        {
            property.SetValue(EntityInstance, null);
            return;
        }

        if (Property.ClrType == typeof(TimeOnly) || Property.ClrType == typeof(TimeOnly?))
        {
            property.SetValue(EntityInstance, TimeOnly.FromTimeSpan(value.Value));
        }
        else
        {
            property.SetValue(EntityInstance, value);
        }
    }
}
