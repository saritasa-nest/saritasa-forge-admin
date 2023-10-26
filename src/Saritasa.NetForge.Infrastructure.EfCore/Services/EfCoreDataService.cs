using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

namespace Saritasa.NetForge.Infrastructure.EfCore.Services;

/// <summary>
/// Data service for EF core.
/// </summary>
public class EfCoreDataService : IOrmDataService
{
    private readonly EfCoreOptions efCoreOptions;
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Constructor.
    /// </summary>
    public EfCoreDataService(EfCoreOptions efCoreOptions, IServiceProvider serviceProvider)
    {
        this.efCoreOptions = efCoreOptions;
        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public IQueryable<object> GetQuery(Type clrType)
    {
        foreach (var dbContextType in efCoreOptions.DbContexts)
        {
            var dbContextService = serviceProvider.GetService(dbContextType);

            if (dbContextService == null)
            {
                continue;
            }

            var dbContext = (DbContext)dbContextService;
            return dbContext.Set(clrType).OfType<object>();
        }

        throw new ArgumentException("Database entity with given type was not found", nameof(clrType));
    }

    /// <inheritdoc />
    public IQueryable<object> CaseInsensitiveSearch(
        IQueryable<object> query, string searchString, Type? entityType, ICollection<PropertyMetadata> properties)
    {
        var searchStrings = searchString.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        Expression? finalExpression = null;
        var entityParam = Expression.Parameter(typeof(object), "entity");
        var searchConstant = Expression.Constant($"%{searchString}%");
        var converted = Expression.Convert(entityParam, entityType);

        foreach (var property in properties)
        {
            if (property.IsSearchable)
            {
                //query = query.Where(entity => EF.Functions.ILike(entity.Name, searchOptions.SearchString));

                // ---

                var propertyExpression = Expression.Property(converted, property.Name);

                //var isMatchMethod = typeof(System.Text.RegularExpressions.Regex).GetMethod("IsMatch",
                //    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                //    null,
                //    new[]
                //    {
                //            typeof(string),
                //            typeof(string),
                //            typeof(System.Text.RegularExpressions.RegexOptions)
                //    },
                //    null
                //);

                //var isMatchCall = Expression.Call(isMatchMethod, propertyExpression, searchConstant,
                //    Expression.Constant(System.Text.RegularExpressions.RegexOptions.IgnoreCase));

                // ---

                var iLike = typeof(NpgsqlDbFunctionsExtensions).GetMethod("ILike",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new[] {
                        typeof(DbFunctions),
                        typeof(string),
                        typeof(string)
                    },
                    null
                );

                var iLikeCall = Expression.Call(
                    iLike,
                    Expression.Constant(null, typeof(DbFunctions)),
                    propertyExpression,
                    searchConstant);

                if (finalExpression is null)
                {
                    finalExpression = iLikeCall;
                }
                else
                {
                    finalExpression = Expression.OrElse(finalExpression, iLikeCall);
                }
            }
        }


        Expression? finalExpression123 = null;

        foreach (var searchString123 in searchStrings)
        {
            var expression123 = TestCaseInsensitive(searchString123, entityType, properties);

            if (finalExpression123 is null)
            {
                finalExpression123 = expression123;
            }
            else
            {
                finalExpression123 = Expression.And(finalExpression123, expression123);
            }
        }

        var finalPredicate = Expression.Lambda<Func<object, bool>>(finalExpression123!, entityParam);
        return query.Where(finalPredicate);
    }

    private Expression TestCaseInsensitive(string searchString, Type? entityType, ICollection<PropertyMetadata> properties)
    {
        Expression? finalExpression = null;
        var entityParam = Expression.Parameter(typeof(object), "entity");
        var searchConstant = Expression.Constant($"%{searchString}%");
        var converted = Expression.Convert(entityParam, entityType);

        foreach (var property in properties)
        {
            if (property.IsSearchable)
            {
                //query = query.Where(entity => EF.Functions.ILike(entity.Name, searchOptions.SearchString));

                // ---

                var propertyExpression = Expression.Property(converted, property.Name);

                //var isMatchMethod = typeof(System.Text.RegularExpressions.Regex).GetMethod("IsMatch",
                //    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                //    null,
                //    new[]
                //    {
                //            typeof(string),
                //            typeof(string),
                //            typeof(System.Text.RegularExpressions.RegexOptions)
                //    },
                //    null
                //);

                //var isMatchCall = Expression.Call(isMatchMethod, propertyExpression, searchConstant,
                //    Expression.Constant(System.Text.RegularExpressions.RegexOptions.IgnoreCase));

                // ---

                var iLike = typeof(NpgsqlDbFunctionsExtensions).GetMethod("ILike",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new[] {
                        typeof(DbFunctions),
                        typeof(string),
                        typeof(string)
                    },
                    null
                );

                var iLikeCall = Expression.Call(
                    iLike,
                    Expression.Constant(null, typeof(DbFunctions)),
                    propertyExpression,
                    searchConstant);

                if (finalExpression is null)
                {
                    finalExpression = iLikeCall;
                }
                else
                {
                    finalExpression = Expression.OrElse(finalExpression, iLikeCall);
                }
            }
        }

        return finalExpression;
    }
}
