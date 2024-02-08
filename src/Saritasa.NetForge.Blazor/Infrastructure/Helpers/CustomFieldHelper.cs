using Saritasa.NetForge.Blazor.Controls.CustomFields;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Infrastructure.Helpers;

/// <summary>
/// Helps to get custom field for property type.
/// </summary>
public static class CustomFieldHelper
{
    private static IReadOnlyDictionary<List<Type>, Type> TypeMappingDictionary { get; }
        = new Dictionary<List<Type>, Type>
        {
            {
                [typeof(string)], typeof(TextField)
            },
            {
                [
                    typeof(short), typeof(short?),
                    typeof(ushort), typeof(ushort?),
                    typeof(int), typeof(int?),
                    typeof(uint), typeof(uint?),
                    typeof(long), typeof(long?),
                    typeof(ulong), typeof(ulong?),
                    typeof(float), typeof(float?),
                    typeof(double), typeof(double?),
                    typeof(decimal), typeof(decimal?)
                ], typeof(NumberField<>)
            },
            {
                [
                    typeof(DateTime), typeof(DateTime?),
                    typeof(DateTimeOffset), typeof(DateTimeOffset?),
                    typeof(DateOnly), typeof(DateOnly?)
                ], typeof(DateField)
            },
            {
                [typeof(bool), typeof(bool?)], typeof(BoolField)
            }
        };

    /// <summary>
    /// Gets custom field <see cref="Type"/> depending on <paramref name="property"/>.
    /// </summary>
    public static Type GetComponentType(PropertyMetadataDto property)
    {
        if (property is NavigationMetadataDto navigation)
        {
            return navigation.IsCollection
                ? typeof(NavigationCollectionField<>).MakeGenericType(property.ClrType!.GetGenericArguments().First())
                : typeof(NavigationField);
        }

        foreach (var (types, inputType) in TypeMappingDictionary)
        {
            if (types.Contains(property.ClrType!))
            {
                return inputType.IsGenericType ? inputType.MakeGenericType(property.ClrType!) : inputType;
            }
        }

        return property.ClrType!.IsEnum ? typeof(EnumField) : typeof(TextField);
    }
}
