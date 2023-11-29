using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Enums;

namespace Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Service for retrieving data from ORM.
/// </summary>
public interface IOrmDataService
{
    /// <summary>
    /// Get query of entity with specified type.
    /// </summary>
    /// <param name="clrType">CLR type.</param>
    /// <returns>Entity data.</returns>
    IQueryable<object> GetQuery(Type clrType);

    /// <summary>
    /// Performs search.
    /// </summary>
    /// <param name="query">Query to search.</param>
    /// <param name="searchString">Search string.</param>
    /// <param name="entityType">Entity type.</param>
    /// <param name="properties">Properties.</param>
    /// <returns>Query with searched data.</returns>
    IQueryable<object> Search(
        IQueryable<object> query,
        string searchString,
        Type entityType,
        IEnumerable<(string Name, SearchType SearchType)> properties);
}
