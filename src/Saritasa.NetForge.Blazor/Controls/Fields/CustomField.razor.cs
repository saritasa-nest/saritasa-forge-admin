using Microsoft.AspNetCore.Components;

namespace Saritasa.NetForge.Blazor.Controls.Fields;

/// <summary>
/// Represents C# type mapped to HTML field.
/// </summary>
public partial class CustomField
{
    /// <summary>
    /// Property name.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string PropertyName { get; set; } = null!;

    /// <summary>
    /// Property type.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Type PropertyType { get; set; } = null!;

    /// <summary>
    /// Entity model that contains property value for this field.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public object EntityModel { get; set; } = null!;

    /// <summary>
    /// Handles input changes.
    /// </summary>
    /// <param name="value">Input value.</param>
    /// <param name="propertyName">Name of property that related to the input.</param>
    public void HandleInputChange(object value, string propertyName)
    {
        var property = EntityModel.GetType().GetProperty(propertyName)!;

        if (string.IsNullOrEmpty(value.ToString()))
        {
            property.SetValue(EntityModel, null);
            return;
        }

        var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        var convertedValue = Convert.ChangeType(value, propertyType);

        property.SetValue(EntityModel, convertedValue);
    }
}
