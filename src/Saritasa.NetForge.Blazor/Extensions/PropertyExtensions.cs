using System.Text.RegularExpressions;

namespace Saritasa.NetForge.Blazor.Extensions;

/// <summary>
/// Extensions for entity properties.
/// </summary>
public static class PropertyExtensions
{
    /// <summary>
    /// Convert CamelCase word into words with space between.
    /// </summary>
    /// <param name="value">The CamelCase word need to be convert.</param>
    public static string ToMeaningfulName(this string value)
    {
        // Using regex to add a space at the beginning of every single uppercase letter except the beginning of the word.
        return Regex.Replace(value, "([A-Z])([A-Z])([a-z])|([a-z])([A-Z])", "$1$4 $2$3$5");
    }
}
