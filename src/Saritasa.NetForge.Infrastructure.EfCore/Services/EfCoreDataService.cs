using System.Linq.Expressions;
using System.Text.RegularExpressions;
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

        Expression? combinedIsMatchExpressions = null;

        // entity => entity
        var entity = Expression.Parameter(typeof(object), "entity");

        // entity => (entityType)entity
        var converted = Expression.Convert(entity, entityType);

        var searchWords = searchString.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        foreach (var searchWord in searchWords)
        {
            Expression? isMatchExpressions = null;
            var searchPattern = Expression.Constant(searchWord);
            foreach (var property in properties)
            {
                if (property.IsSearchable)
                {
                    // entity => ((entityType)entity).propertyName
                    var propertyExpression = Expression.Property(converted, property.Name);

                    var isMatch = typeof(Regex).GetMethod(
                        nameof(Regex.IsMatch),
                        new[]
                        {
                            typeof(string), typeof(string), typeof(RegexOptions)
                        });

                    // entity => Regex.IsMatch(((entityType)entity).propertyName, searchWord, RegexOptions.IgnoreCase)
                    var isMatchCall = Expression.Call(
                        isMatch!, propertyExpression, searchPattern, Expression.Constant(RegexOptions.IgnoreCase));

                    if (isMatchExpressions is null)
                    {
                        isMatchExpressions = isMatchCall;
                    }
                    else
                    {
                        // entity => Regex.IsMatch(((entityType)entity).propertyName, searchWord, RegexOptions.IgnoreCase) ||
                        //           Regex.IsMatch(((entityType)entity).propertyName2, searchWord, RegexOptions.IgnoreCase)
                        // Produces SQL:
                        // WHERE entity.propertyName ~ ('(?ip)' || searchWord) OR entity.propertyName2 ~ ('(?ip)' || searchWord)
                        isMatchExpressions = Expression.OrElse(isMatchExpressions, isMatchCall);
                    }
                }
            }

            if (combinedIsMatchExpressions is null)
            {
                combinedIsMatchExpressions = isMatchExpressions;
            }
            else
            {
                // Produces SQL:
                // WHERE (entity.propertyName ~ ('(?ip)' || searchWord) OR entity.propertyName2 ~ ('(?ip)' || searchWord))
                // AND   (entity.propertyName ~ ('(?ip)' || searchWord2) OR entity.propertyName2 ~ ('(?ip)' || searchWord2))
                combinedIsMatchExpressions = Expression.And(combinedIsMatchExpressions, isMatchExpressions!);
            }
        }

        var predicate = Expression.Lambda<Func<object, bool>>(combinedIsMatchExpressions!, entity);
        return query.Where(predicate);
    }
}
