using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.Controls.CustomFields;

/// <summary>
/// Represents text field.
/// </summary>
public partial class TextField : CustomField
{
    /// <summary>
    /// Property value.
    /// </summary>
    public string? PropertyValue
    {
        get => EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance)?.ToString();
        set
        {
            var convertedValue = Property.Converter is not null ? Property.Converter(value) : value;
            EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, convertedValue);
        }
    }

    /// <inheritdoc cref="PropertyMetadataBase.Lines"/>
    public int Lines => Property.IsMultiline ? Property.Lines : 1;

    /// <inheritdoc cref="PropertyMetadataBase.MaxLines"/>
    public int MaxLines => Property.IsMultiline ? Property.MaxLines : 1;

    /// <inheritdoc cref="PropertyMetadataBase.IsAutoGrow"/>
    public bool IsAutoGrow => Property.IsAutoGrow;
}
