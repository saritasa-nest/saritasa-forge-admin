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
    /// Entity instance that contains property value for this field.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public object EntityInstance { get; init; } = null!;

    /// <summary>
    /// Is field with read only access.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool IsReadOnly { get; init; }

    /// <summary>
    /// Property value.
    /// </summary>
    public string? PropertyValue
    {
        get => EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance)?.ToString();
        set
        {
            var propertyType = Property.ClrType!;
            var actualPropertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            var enumValue = Enum.Parse(actualPropertyType, value!);
            EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, enumValue);
        }
    }
}
