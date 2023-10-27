namespace Saritasa.NetForge.Domain.Enums;

/// <summary>
/// Search type.
/// </summary>
public enum SearchType
{
    /// <summary>
    /// Case insensitive partial search.
    /// </summary>
    CaseInsensitiveContains,

    /// <summary>
    /// Case sensitive starts with search.
    /// </summary>
    CaseSensitiveStartsWith,

    /// <summary>
    /// Search type not specified.
    /// </summary>
    None
}
