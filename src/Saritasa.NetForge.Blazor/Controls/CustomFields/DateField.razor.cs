using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Represents date field.
/// </summary>
public partial class DateField
{
    /// <summary>
    /// Property name.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public PropertyMetadataDto Property { get; init; } = null!;

    /// <summary>
    /// Entity model that contains property value for this field.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public object EntityModel { get; init; } = null!;

    /// <summary>
    /// Is field with read only access.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool IsReadOnly { get; init; }

    /// <summary>
    /// Property date value.
    /// </summary>
    public DateTime? PropertyValue
    {
        get
        {
            var propertyValue = EntityModel.GetType().GetProperty(Property.Name)?.GetValue(EntityModel)?.ToString();

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
        var property = EntityModel.GetType().GetProperty(Property.Name)!;

        if (!value.HasValue)
        {
            property.SetValue(EntityModel, null);
            return;
        }

        var actualPropertyType = Nullable.GetUnderlyingType(Property.ClrType!) ?? Property.ClrType;
        if (actualPropertyType == typeof(DateTimeOffset))
        {
            property.SetValue(EntityModel, new DateTimeOffset(value.Value));
        }
        else if (actualPropertyType == typeof(DateOnly))
        {
            property.SetValue(EntityModel, DateOnly.FromDateTime(value.Value));
        }
        else
        {
            property.SetValue(EntityModel, value);
        }
    }
}
