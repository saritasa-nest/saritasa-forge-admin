namespace Saritasa.NetForge.DomainServices.Extensions;

/// <summary>
/// <see cref="string"/> extensions.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Truncates <paramref name="value"/>.
    /// </summary>
    /// <param name="value">String to truncate.</param>
    /// <param name="maxChars">Maximum characters.</param>
    /// <returns>Truncated string.</returns>
    public static string Truncate(this string value, int maxChars)
    {
        return value.Length <= maxChars ? value : $"{value[..maxChars]}...";
    }
}
