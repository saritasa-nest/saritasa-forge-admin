using System.Text.RegularExpressions;

namespace Saritasa.NetForge.Infrastructure.EfCore.Services;

/// <summary>
/// Extensions for entity properties.
/// </summary>
public static class PropertyExtensions
{
    /// <summary>
    /// Convert camel case word into words with space between.
    /// </summary>
    /// <param name="value">The camelcase word need to be convert.</param>
    public static string ToMeaningfulName(this string value)
    {
        // Using regex to add a space at the beginning of every single uppercase letter except the beginning of the word.
        return Regex.Replace(value, "(?!^)([A-Z])", " $1");
    }
}
