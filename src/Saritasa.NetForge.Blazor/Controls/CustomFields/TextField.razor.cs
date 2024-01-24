using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Represents text field.
/// </summary>
public partial class TextField
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
    /// Property value.
    /// </summary>
    public string? PropertyValue
    {
        get => EntityModel.GetType().GetProperty(Property.Name)?.GetValue(EntityModel)?.ToString();
        set => EntityModel.GetType().GetProperty(Property.Name)?.SetValue(EntityModel, value);
    }
}
