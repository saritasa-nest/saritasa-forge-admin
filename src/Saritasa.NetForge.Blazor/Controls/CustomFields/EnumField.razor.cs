using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Represents enum field.
/// </summary>
public partial class EnumField
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

    [Parameter]
    [EditorRequired]
    /// <summary>
    /// Is field with read only access.
    /// </summary>
    public bool IsReadOnly { get; init; }

    /// <summary>
    /// Property value.
    /// </summary>
    public string? PropertyValue
    {
        get => EntityModel.GetType().GetProperty(Property.Name)?.GetValue(EntityModel)?.ToString();
        set
        {
            var propertyType = Property.ClrType!;
            var actualPropertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            var enumValue = Enum.Parse(actualPropertyType, value!);
            EntityModel.GetType().GetProperty(Property.Name)?.SetValue(EntityModel, enumValue);
        }
    }
}
