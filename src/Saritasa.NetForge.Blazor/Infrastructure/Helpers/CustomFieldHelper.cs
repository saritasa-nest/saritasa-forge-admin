using Saritasa.NetForge.Blazor.Controls.CustomFields;

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
            },
            {
                [typeof(byte[])], typeof(UploadFile)
            }
        };

    /// <summary>
    /// Gets custom field <see cref="Type"/> depending on <paramref name="propertyType"/>.
    /// </summary>
    public static Type GetComponentType(Type propertyType)
    {
        foreach (var (types, inputType) in TypeMappingDictionary)
        {
            if (types.Contains(propertyType))
            {
                return inputType.IsGenericType ? inputType.MakeGenericType(propertyType) : inputType;
            }
        }

        return propertyType.IsEnum ? typeof(EnumField) : typeof(TextField);
    }
}
