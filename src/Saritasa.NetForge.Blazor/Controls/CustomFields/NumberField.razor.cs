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

    private T? propertyValue;

    /// <summary>
    /// Property date value.
    /// </summary>
    public T? PropertyValue
    {
        get => propertyValue;
        set
        {
            propertyValue = value;
            EntityModel.GetType().GetProperty(Property.Name)?.SetValue(EntityModel, value);
        }
    }

    /// <summary>
    /// Sets <see cref="PropertyType"/> after all parameters set.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        propertyValue = (T?)EntityModel.GetType().GetProperty(Property.Name)?.GetValue(EntityModel);
    }
}
