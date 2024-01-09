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
    /// Property type.
    /// </summary>
    public Type PropertyType { get; set; } = null!;

    /// <summary>
    /// Property date value.
    /// </summary>
    public DateTime? PropertyValue { get; set; }

    /// <summary>
    /// Sets <see cref="PropertyType"/> after all parameters set.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        PropertyType = EntityModel.GetType().GetProperty(Property.Name)!.PropertyType;
        var propertyValue = EntityModel.GetType().GetProperty(Property.Name)?.GetValue(EntityModel)?.ToString();

        var isDateParsed = DateTime.TryParse(propertyValue, out var parsedDate);

        if (isDateParsed)
        {
            PropertyValue = parsedDate;
        }
    }

    /// <summary>
    /// Handles input changes.
    /// </summary>
    /// <param name="value">Input value.</param>
    private void HandleInputChange(object? value)
    {
        var property = EntityModel.GetType().GetProperty(Property.Name)!;

        var stringValue = value?.ToString();
        if (string.IsNullOrEmpty(stringValue))
        {
            property.SetValue(EntityModel, null);
            return;
        }

        var actualPropertyType = Nullable.GetUnderlyingType(PropertyType) ?? PropertyType;
        if (actualPropertyType == typeof(DateTimeOffset))
        {
            property.SetValue(EntityModel, DateTimeOffset.Parse(stringValue));
        }
        else if (actualPropertyType == typeof(DateOnly))
        {
            property.SetValue(EntityModel, DateOnly.Parse(stringValue));
        }
        else
        {
            property.SetValue(EntityModel, DateTime.Parse(stringValue));
        }
    }
}
