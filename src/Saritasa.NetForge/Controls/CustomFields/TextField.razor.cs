using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Extensions;

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
        get => EntityInstance.GetNestedPropertyValue(Property.PropertyPath)?.ToString();
        set => EntityInstance.SetNestedPropertyValue(Property.PropertyPath, value);
    }

    /// <inheritdoc cref="PropertyMetadataBase.Lines"/>
    public int Lines => Property.IsMultiline ? Property.Lines : 1;

    /// <inheritdoc cref="PropertyMetadataBase.MaxLines"/>
    public int MaxLines => Property.IsMultiline ? Property.MaxLines : 1;

    /// <inheritdoc cref="PropertyMetadataBase.IsAutoGrow"/>
    public bool IsAutoGrow => Property.IsAutoGrow;
}
