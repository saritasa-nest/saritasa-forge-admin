using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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
        IQueryable<object> query, string? searchString, Type entityType, ICollection<PropertyMetadata> properties)
    {
        if (string.IsNullOrEmpty(searchString) || !properties.Any(property => property.IsSearchable))
        {
            return query;
        }

        Expression? combinedILikeExpressions = null;

        // entity => entity
        var entityParam = Expression.Parameter(typeof(object), "entity");

        // entity => (entityType)entity
        var converted = Expression.Convert(entityParam, entityType);

        var searchWords = searchString.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        foreach (var searchWord in searchWords)
        {
            Expression? iLikeExpressions = null;
            var searchPattern = Expression.Constant($"%{searchWord}%");
            foreach (var property in properties)
            {
                if (property.IsSearchable)
                {
                    //query = query.Where(entity => EF.Functions.ILike(entity.Name, searchOptions.SearchString));

                    // --

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

                    // entity => ((entityType)entity).propertyName
                    var propertyExpression = Expression.Property(converted, property.Name);

                    var iLike = typeof(NpgsqlDbFunctionsExtensions).GetMethod(
                        nameof(NpgsqlDbFunctionsExtensions.ILike),
                        new[]
                        {
                        typeof(DbFunctions), typeof(string), typeof(string)
                        });

                    // entity => EF.Functions.ILike(((entityType)entity).propertyName, searchWord)
                    var iLikeCall = Expression.Call(
                        iLike!,
                        Expression.Constant(null, typeof(DbFunctions)),
                        propertyExpression,
                        searchPattern);

                    if (iLikeExpressions is null)
                    {
                        iLikeExpressions = iLikeCall;
                    }
                    else
                    {
                        // entity => EF.Functions.ILike(((entityType)entity).propertyName, searchWord) ||
                        //           EF.Functions.ILike(((entityType)entity).propertyName2, searchWord)
                        // Produces SQL:
                        // WHERE entity.propertyName ILIKE '%searchWord%' OR entity.propertyName2 ILIKE '%searchWord%'
                        iLikeExpressions = Expression.OrElse(iLikeExpressions, iLikeCall);
                    }
                }
            }

            if (combinedILikeExpressions is null)
            {
                combinedILikeExpressions = iLikeExpressions;
            }
            else
            {
                // Produces SQL:
                // WHERE (entity.propertyName ILIKE '%searchWord%' OR entity.propertyName2 ILIKE '%searchWord%')
                // AND   (entity.propertyName ILIKE '%searchWord2%' OR entity.propertyName2 ILIKE '%searchWord2%')
                combinedILikeExpressions = Expression.And(combinedILikeExpressions, iLikeExpressions!);
            }
        }

        var predicate = Expression.Lambda<Func<object, bool>>(combinedILikeExpressions!, entityParam);
        return query.Where(predicate);
    }
}
