using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

namespace Saritasa.NetForge.Infrastructure.EfCore.Services;

/// <summary>
/// Data service for EF core.
/// </summary>
public class EfCoreDataService : IOrmDataService
{
    private const string Entity = "entity";

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
    public IQueryable<object> Search(IQueryable<object> query,
        string? searchString,
        Type entityType,
        ICollection<PropertyMetadata> properties,
        Func<object?, IQueryable<object>, string, IQueryable<object>>? customSearch)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            return query;
        }

        if (customSearch is not null)
        {
            query = CustomSearch(query, searchString, customSearch);

            return query;
        }

        Expression? combinedSearchExpressions = null;

        // entity => entity
        var entity = Expression.Parameter(typeof(object), Entity);

        // entity => (entityType)entity
        var convertedEntity = Expression.Convert(entity, entityType);

        var searchWords = searchString.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        foreach (var searchWord in searchWords)
        {
            var searchFor = searchWord;

            var isExactMatch = false;
            const char quote = '"';
            if (searchWord.StartsWith(quote) && searchWord.EndsWith(quote))
            {
                const int startIndex = 1;
                var endIndex = searchFor.LastIndexOf(quote);

                searchFor = searchFor[startIndex..endIndex];

                isExactMatch = true;
            }

            var searchConstant = Expression.Constant(searchFor);

            Expression? searchExpressions = null;
            foreach (var property in properties)
            {
                var searchType = property.SearchType;
                if (!searchType.HasValue)
                {
                    continue;
                }

                // entity => ((entityType)entity).propertyName
                var propertyExpression = Expression.Property(convertedEntity, property.Name);

                if (isExactMatch)
                {
                    searchType = SearchType.ExactMatch;
                }

                var searchMethodCall = searchType switch
                {
                    SearchType.CaseInsensitiveContains
                        => GetCaseInsensitiveContainsMethodCall(propertyExpression, searchConstant),

                    SearchType.CaseSensitiveStartsWith
                        => GetCaseSensitiveStartsWithCall(propertyExpression, searchConstant),

                    SearchType.ExactMatch
                        => GetExactMatchCall(propertyExpression, searchConstant),

                    _ => throw new NotImplementedException("Unsupported search type was used.")
                };

                if (searchExpressions is null)
                {
                    searchExpressions = searchMethodCall;
                }
                else
                {
                    // Example:
                    // entity => Regex.IsMatch(((entityType)entity).propertyName, searchWord, RegexOptions.IgnoreCase) ||
                    //           ((entityType)entity).propertyName2.StartsWith(searchConstant)
                    searchExpressions = Expression.OrElse(searchExpressions, searchMethodCall);
                }
            }

            if (combinedSearchExpressions is null)
            {
                combinedSearchExpressions = searchExpressions;
            }
            else
            {
                combinedSearchExpressions = Expression.And(combinedSearchExpressions, searchExpressions!);
            }
        }

        var predicate = Expression.Lambda<Func<object, bool>>(combinedSearchExpressions!, entity);



        return query.Where(predicate);
    }

    private MethodCallExpression GetCaseInsensitiveContainsMethodCall(
        MemberExpression propertyExpression, ConstantExpression searchConstant)
    {
        var isMatch = typeof(Regex).GetMethod(
            nameof(Regex.IsMatch),
            new[]
            {
                typeof(string), typeof(string), typeof(RegexOptions)
            });

        // entity => Regex.IsMatch(((entityType)entity).propertyName, searchWord, RegexOptions.IgnoreCase)
        return Expression.Call(
            isMatch!, propertyExpression, searchConstant, Expression.Constant(RegexOptions.IgnoreCase));
    }

    private MethodCallExpression GetCaseSensitiveStartsWithCall(
        MemberExpression propertyExpression, ConstantExpression searchConstant)
    {
        var startsWith = typeof(string).GetMethod(
            nameof(string.StartsWith),
            new[]
            {
                typeof(string)
            });

        var property = GetConvertedExpressionWhenPropertyIsNotString(propertyExpression);

        // entity => ((entityType)entity).propertyName.StartsWith(searchConstant)
        return Expression.Call(property, startsWith!, searchConstant);
    }

    private MethodCallExpression GetExactMatchCall(
        MemberExpression propertyExpression, ConstantExpression searchConstant)
    {
        var equal = typeof(string).GetMethod(
            nameof(string.Equals),
            new[]
            {
                typeof(string)
            });

        var property = GetConvertedExpressionWhenPropertyIsNotString(propertyExpression);

        // entity => ((entityType)entity).propertyName.Equal(searchConstant)
        return Expression.Call(property, equal!, searchConstant);
    }

    private static Expression GetConvertedExpressionWhenPropertyIsNotString(MemberExpression propertyExpression)
    {
        var propertyType = ((PropertyInfo)propertyExpression.Member).PropertyType;

        if (propertyType != typeof(string))
        {
            // When passed property expression does not represent string we call ToString()
            return Expression.Call(propertyExpression, typeof(object).GetMethod(nameof(ToString))!);
        }

        return propertyExpression;
    }

    private static IQueryable<object> CustomSearch(
        IQueryable<object> query,
        string? searchString,
        Func<object?, IQueryable<object>, string, IQueryable<object>> customSearch)
    {
        return customSearch.Invoke(null, query, searchString!);
    }
}
