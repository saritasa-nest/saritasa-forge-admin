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
    /// Exact match search.
    /// </summary>
    ExactMatch,

    /// <summary>
    /// Search type is not specified.
    /// </summary>
    None
}
