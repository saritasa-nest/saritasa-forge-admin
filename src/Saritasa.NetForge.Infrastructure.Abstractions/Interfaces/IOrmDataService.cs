namespace Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Service for retrieving data from ORM.
/// </summary>
public interface IOrmDataService
{
    /// <summary>
    /// Get all data of specified type.
    /// </summary>
    /// <param name="clrType">CLR type.</param>
    /// <returns>Data.</returns>
    IQueryable GetData(Type clrType);
}
