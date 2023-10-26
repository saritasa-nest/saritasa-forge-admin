using Saritasa.NetForge.Domain.Entities.Metadata;

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

    IQueryable<object> CaseInsensitiveSearch(
        IQueryable<object> query, string searchString, Type? entityType, ICollection<PropertyMetadata> properties);
}
