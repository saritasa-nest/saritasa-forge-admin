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
    /// Gets custom field <see cref="Type"/> depending on <see cref="PropertyMetadataDto.ClrType"/>.
    /// </summary>
    public static Type GetComponentType(PropertyMetadataDto propertyMetadata)
    {
        if (propertyMetadata.IsRichTextField)
        {
            return typeof(CkEditorField);
        }

        var propertyType = propertyMetadata.ClrType!;

        foreach (var (types, inputType) in TypeMappingDictionary)
        {
            if (types.Contains(propertyType))
            {
                return inputType.IsGenericType ? inputType.MakeGenericType(propertyType) : inputType;
            }
        }

        // Text field is a default one.
        return propertyType.IsEnum ? typeof(EnumField) : typeof(TextField);
    }
}
