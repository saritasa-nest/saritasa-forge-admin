using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Represents number field.
/// </summary>
public partial class NumberField<T>
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
    public T? PropertyValue
    {
        get => (T?)EntityModel.GetType().GetProperty(Property.Name)?.GetValue(EntityModel);
        set => EntityModel.GetType().GetProperty(Property.Name)?.SetValue(EntityModel, value);
    }
}
