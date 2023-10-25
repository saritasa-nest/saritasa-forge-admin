using System.Linq.Expressions;
using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.DomainServices.Extensions;

/// <summary>
/// Extensions to <see cref="IQueryable"/>.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Select only those properties from entity that exists in <paramref name="properties"/>.
    /// </summary>
    /// <param name="query">
    /// Query that contain data for some entity. For example, all data of <c>Address</c> entity.
    /// </param>
    /// <param name="entityType">Entity type.</param>
    /// <param name="properties">Entity properties to select.</param>
    /// <returns>Query with selected data.</returns>
    /// <remarks>
    /// Reflection can't be translated to SQL, so we have to build expression dynamically.
    /// </remarks>
    public static IQueryable<object> SelectProperties(
        this IQueryable<object> query, Type entityType, IEnumerable<PropertyMetadata> properties)
    {
        // entity => entity
        var entity = Expression.Parameter(typeof(object), "entity");

        // entity => (entityType)entity
        var convertedEntity = Expression.Convert(entity, entityType);

        var bindings = properties
            .Select(property => Expression.Property(convertedEntity, property.Name))
            .Select(member => Expression.Bind(member.Member, member));

        var ctor = entityType.GetConstructors()[0];

        // entity => new entityType
        // { PropertyName1 = ((entityType)entity).PropertyName1, PropertyName2 = ((entityType)entity).PropertyName2 ...  }
        var memberInit = Expression.MemberInit(Expression.New(ctor), bindings);

        var selectLambda = Expression.Lambda<Func<object, object>>(memberInit, entity);

        return query.Select(selectLambda);
    }
}
