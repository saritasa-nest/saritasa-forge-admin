using Saritasa.NetForge.Blazor.Domain.UseCases.Common;
using Saritasa.NetForge.Blazor.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.Tools.Common.Pagination;

namespace Saritasa.NetForge.Blazor.Infrastructure.Abstractions.Interfaces;

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
    /// Get instance by primary key value.
    /// </summary>
    /// <param name="primaryKey">
    /// Primary key values.
    /// In case of composite primary key, they have to be separated by "--".
    /// </param>
    /// <param name="entityType">Type to get instance of.</param>
    /// <param name="includedNavigationNames">Included navigation names.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Instance.</returns>
    Task<object> GetInstanceAsync(
        string primaryKey,
        Type entityType,
        IEnumerable<string> includedNavigationNames,
        CancellationToken cancellationToken);

    /// <summary>
    /// Adds entity to the database.
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    /// <param name="entityType">Entity type.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task AddAsync(object entity, Type entityType, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes entity in the database.
    /// </summary>
    /// <param name="entity">Entity to delete.</param>
    /// <param name="entityType">Entity type.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task DeleteAsync(object entity, Type entityType, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes all provided entities in the database.
    /// </summary>
    /// <param name="entities">Entities to delete.</param>
    /// <param name="entityType">Entity type.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task BulkDeleteAsync(IEnumerable<object> entities, Type entityType, CancellationToken cancellationToken);

    /// <summary>
    /// Updates entity in the database.
    /// </summary>
    /// <param name="entity">Entity to update.</param>
    /// <param name="originalEntity">Entity that contains initial unchanged values.</param>
    /// <param name="afterUpdateAction">Action that will be called after update.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task<object> UpdateAsync(
        object entity,
        object originalEntity,
        Action<IServiceProvider?, object, object>? afterUpdateAction,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get the data for the specific entity type.
    /// </summary>
    /// <param name="entityType">Entity type to search data. For example, search all data for entity with type <c>Address</c>.</param>
    /// <param name="properties">Entity properties metadata to be included in returned data.</param>
    /// <param name="searchOptions">Search options.</param>
    /// <param name="searchFunction">Custom search function.</param>
    /// <param name="customQueryFunction">Custom query function.</param>
    /// <returns>Entity instances with pages metadata.</returns>
    Task<PagedListMetadataDto<object>> SearchDataForEntityAsync(
        Type? entityType,
        ICollection<PropertyMetadataDto> properties,
        SearchOptions searchOptions,
        Func<IServiceProvider?, IQueryable<object>, string, IQueryable<object>>? searchFunction = null,
        Func<IServiceProvider?, IQueryable<object>, IQueryable<object>>? customQueryFunction = null);
}
