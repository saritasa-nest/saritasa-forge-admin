using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls;

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
    /// Property value.
    /// </summary>
    public string? PropertyValue { get; set; } = null!;

    /// <summary>
    /// Property date value.
    /// </summary>
    public DateTime? PropertyDateValue { get; set; } = null!;

    private bool? propertyBooleanValue;

    /// <summary>
    /// Property value when <see cref="PropertyType"/> is <see cref="bool"/>.
    /// </summary>
    public bool? PropertyBooleanValue
    {
        get => propertyBooleanValue;
        set
        {
            propertyBooleanValue = value;
            HandleInputChange(propertyBooleanValue);
        }
    }

    /// <summary>
    /// Sets <see cref="PropertyType"/> after all parameters set.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        PropertyType = EntityModel.GetType().GetProperty(Property.Name)!.PropertyType;
        PropertyValue = EntityModel.GetType().GetProperty(Property.Name)?.GetValue(EntityModel)?.ToString();

        var actualPropertyType = Nullable.GetUnderlyingType(PropertyType) ?? PropertyType;
        if (actualPropertyType == typeof(DateTime)
            || actualPropertyType == typeof(DateTimeOffset)
            || actualPropertyType == typeof(DateOnly))
        {
            var isDateParsed = DateTime.TryParse(PropertyValue, out var parsedDate);

            if (isDateParsed)
            {
                PropertyDateValue = parsedDate;
            }
        }
        else if (actualPropertyType == typeof(bool))
        {
            var isBooleanParsed = bool.TryParse(PropertyValue, out var parsedBoolean);

            if (isBooleanParsed)
            {
                PropertyBooleanValue = parsedBoolean;
            }
        }
    }

    private IReadOnlyDictionary<List<Type>, InputType> TypeMappingDictionary { get; init; }
        = new Dictionary<List<Type>, InputType>
    {
        {
            new List<Type> { typeof(string) }, InputType.Text
        },
        {
            new List<Type>
            {
                typeof(short), typeof(short?),
                typeof(ushort), typeof(ushort?),
                typeof(int), typeof(int?),
                typeof(uint), typeof(uint?),
                typeof(long), typeof(long?),
                typeof(ulong), typeof(ulong?),
                typeof(float), typeof(float?),
                typeof(double), typeof(double?),
                typeof(decimal), typeof(decimal?)
            }, InputType.Number
        },
        {
            new List<Type>
            {
                typeof(DateTime), typeof(DateTime?),
                typeof(DateTimeOffset), typeof(DateTimeOffset?)
            }, InputType.DateTimeLocal
        },
        {
            new List<Type>
            {
                typeof(DateOnly), typeof(DateOnly?),
            }, InputType.Date
        }
    };

    /// <summary>
    /// Gets <see cref="InputType"/> depending on <paramref name="propertyType"/>.
    /// </summary>
    private InputType GetInputType(Type propertyType)
    {
        foreach (var (types, inputType) in TypeMappingDictionary)
        {
            if (types.Contains(propertyType))
            {
                return inputType;
            }
        }

        return InputType.Text;
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
        object convertedValue;
        if (actualPropertyType == typeof(DateTimeOffset))
        {
            convertedValue = DateTimeOffset.Parse(stringValue);
        }
        else if (actualPropertyType == typeof(DateOnly))
        {
            convertedValue = DateOnly.Parse(stringValue);
        }
        else if (actualPropertyType.IsEnum)
        {
            convertedValue = Enum.Parse(actualPropertyType, stringValue);
        }
        else
        {
            convertedValue = Convert.ChangeType(value, actualPropertyType);
        }

        property.SetValue(EntityModel, convertedValue);
    }
}
