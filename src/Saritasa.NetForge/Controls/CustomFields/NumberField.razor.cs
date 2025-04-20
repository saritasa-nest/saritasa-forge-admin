using System.Reflection;

namespace Saritasa.NetForge.Controls.CustomFields;

file static class NumberFieldHelpers
{
    public static object? ParseValueType<T>(string? input, IFormatProvider? formatProvider) where T : struct, ISpanParsable<T>
    {
        return T.TryParse(input, formatProvider, out var parseResult) ? parseResult : null;
    }

    public static object? ParseClass<T>(string? input, IFormatProvider? formatProvider) where T : class, ISpanParsable<T>
    {
        return T.TryParse(input, formatProvider, out var parseResult) ? parseResult : null;
    }

    private static bool ImplementsSelfGenericINumber(Type type)
    {
        Type interfaceType;
        try
        {
            interfaceType = typeof(ISpanParsable<>).MakeGenericType(type);
        }
        catch (ArgumentException)
        {
            return false;
        }

        return interfaceType.IsAssignableFrom(type);
    }

    public static Func<string?, IFormatProvider?, object?> ExtractParser(Type type)
    {
        if (!ImplementsSelfGenericINumber(type))
        {
            throw new InvalidOperationException($"'{type}' does not implement ISpanParsable<T>");
        }

        var methodName = type.IsValueType ? nameof(ParseValueType) : nameof(ParseClass);
        var methodInfo = typeof(NumberFieldHelpers)
            .GetMethod(methodName, BindingFlags.Public | BindingFlags.Static)!
            .MakeGenericMethod(type);

        return (input, formatProvider) => methodInfo.Invoke(null, [input, formatProvider]);
    }
}

/// <summary>
/// Represents number field.
/// </summary>
public partial class NumberField<T> : CustomField
{
    private static readonly Func<string?, IFormatProvider?, object?> Parser;

    private static bool IsValueTypeNullable =>
        typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>);

    static NumberField()
    {
        var type = typeof(T);
        if (IsValueTypeNullable)
        {
            type = type.GetGenericArguments()[0];
        }

        Parser = NumberFieldHelpers.ExtractParser(type);
    }

    /// <summary>
    /// Property value.
    /// </summary>
    public string? PropertyValue
    {
        get => EntityTracker.GetPropertyValue(Property.Name)?.ToString();
        set
        {
            var parsedValue = Parser(value, Property.FormatProvider);
            if (parsedValue != null)
            {
                EntityTracker.SetPropertyValue(Property.Name, parsedValue);
                EntityTracker.PropertyErrors.Remove(Property.Name);
                return;
            }

            FieldErrorModel = new()
            {
                ErrorMessage = "Not a valid number"
            };

            EntityTracker.PropertyErrors[Property.Name] = FieldErrorModel.ErrorMessage;
        }
    }
}
