using Microsoft.EntityFrameworkCore;

namespace Saritasa.NetForge.Infrastructure.EfCore.Extensions;

/// <summary>
/// Database context extensions.
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Get <see cref="DbSet{TEntity}"/> of given <paramref name="entityType"/>.
    /// </summary>
    /// <param name="context">Database context.</param>
    /// <param name="entityType">Entity type.</param>
    /// <returns><see cref="IQueryable"/> that populated with entities of <paramref name="entityType"/>.</returns>
    /// <remarks>
    /// Due to absence of non generic version of <see cref="DbContext.Set{TEntity}()"/> we created this method.
    /// </remarks>
    public static IQueryable Set(this DbContext context, Type entityType)
    {
        var setMethod = typeof(DbContext)
            .GetMethods()
            .Single(p => p.Name == nameof(DbContext.Set) && p.ContainsGenericParameters && !p.GetParameters().Any());

        setMethod = setMethod.MakeGenericMethod(entityType);

        return (setMethod.Invoke(context, null) as IQueryable)!;
    }
}
