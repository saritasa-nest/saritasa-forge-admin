using Microsoft.AspNetCore.Components;
using MudBlazor;

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
    public string PropertyName { get; init; } = null!;

    /// <summary>
    /// Property type.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Type PropertyType { get; init; } = null!;

    /// <summary>
    /// Entity model that contains property value for this field.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public object EntityModel { get; init; } = null!;

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
    /// <param name="propertyName">Name of property that related to the input.</param>
    public void HandleInputChange(object value, string propertyName)
    {
        var property = EntityModel.GetType().GetProperty(propertyName)!;

        var stringValue = value.ToString();
        if (string.IsNullOrEmpty(stringValue))
        {
            property.SetValue(EntityModel, null);
            return;
        }

        var propertyType = Nullable.GetUnderlyingType(PropertyType) ?? PropertyType;
        object convertedValue;
        if (propertyType == typeof(DateTimeOffset))
        {
            convertedValue = DateTimeOffset.Parse(stringValue);
        }
        else if (propertyType == typeof(DateOnly))
        {
            convertedValue = DateOnly.Parse(stringValue);
        }
        else if (propertyType.IsEnum)
        {
            convertedValue = Enum.Parse(propertyType, stringValue);
        }
        else
        {
            convertedValue = Convert.ChangeType(value, propertyType);
        }

        property.SetValue(EntityModel, convertedValue);
    }
}
