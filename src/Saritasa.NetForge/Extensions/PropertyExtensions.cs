using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Extensions;

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

    /// <summary>
    /// Gets <see cref="PropertyMetadata.DisplayName"/>.
    /// If property does not have display name then <see cref="PropertyMetadata.Name"/> will be used.
    /// </summary>
    public static string GetDisplayName(this PropertyMetadataDto property)
    {
        return property.DisplayName != string.Empty ? property.DisplayName : property.Name.ToMeaningfulName();
    }
}
