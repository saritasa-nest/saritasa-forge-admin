using Saritasa.NetForge.Blazor.Controls.CustomFields;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Infrastructure.Helpers;

/// <summary>
/// Helps to get custom field for property type.
/// </summary>
public static class CustomFieldHelper
{
    private static readonly IEnumerable<Type> NumberFieldTypes = new List<Type>
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
    };

    private static readonly IEnumerable<Type> DateFieldTypes = new List<Type>
    {
        typeof(DateTime), typeof(DateTime?),
        typeof(DateTimeOffset), typeof(DateTimeOffset?),
        typeof(DateOnly), typeof(DateOnly?)
    };

    private static readonly IEnumerable<Type> BooleanFieldTypes = new List<Type>
    {
        typeof(bool), typeof(bool?)
    };

    private static readonly IEnumerable<Type> TextFieldTypes = new List<Type>
    {
        typeof(string)
    };

    private static IReadOnlyDictionary<IEnumerable<Type>, Type> TypeMappingDictionary { get; } =
        new Dictionary<IEnumerable<Type>, Type>
        {
            { NumberFieldTypes, typeof(NumberField<>) },
            { DateFieldTypes, typeof(DateField) },
            { BooleanFieldTypes, typeof(BoolField) },
            { TextFieldTypes, typeof(TextField) }
        };

    /// <summary>
    /// Gets custom field <see cref="Type"/> depending on <paramref name="property"/>.
    /// </summary>
    public static Type GetComponentType(PropertyMetadataDto property)
    {
        if (property.IsPathToImage || property.IsBase64Image)
        {
            return typeof(UploadImage);
        }

        foreach (var (types, inputType) in TypeMappingDictionary)
        {
            if (types.Contains(property.ClrType!))
            {
                return inputType.IsGenericType ? inputType.MakeGenericType(property.ClrType!) : inputType;
            }
        }

        // Text field is a default one.
        return property.ClrType!.IsEnum ? typeof(EnumField) : typeof(TextField);
    }
}
