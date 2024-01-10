using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Represents bool field.
/// </summary>
public partial class BoolField
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
    /// Property value.
    /// </summary>
    public bool? PropertyValue
    {
        get => (bool?)EntityModel.GetType().GetProperty(Property.Name)?.GetValue(EntityModel);
        set => EntityModel.GetType().GetProperty(Property.Name)?.SetValue(EntityModel, value);
    }
}
