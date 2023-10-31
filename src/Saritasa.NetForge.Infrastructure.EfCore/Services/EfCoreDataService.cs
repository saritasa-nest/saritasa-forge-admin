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
    public IQueryable<object> Search(
        IQueryable<object> query, string? searchString, Type entityType, IEnumerable<PropertyMetadata> properties)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            return query;
        }

        Expression? combinedSearchExpressions = null;

        // entity => entity
        var entity = Expression.Parameter(typeof(object), Entity);

        // entity => (entityType)entity
        var convertedEntity = Expression.Convert(entity, entityType);

        const string splitSearchStringRegex = """(?<=["])[^"]*(?="\s|"$)|(?<=['])[^']*(?='\s|'$)|[^\s"']+""";
        var matches = Regex.Matches(searchString, splitSearchStringRegex);

        var searchEntries = matches.Select(match => match.Value);
        foreach (var searchEntry in searchEntries)
        {
            var searchConstant = Expression.Constant(searchEntry);

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

                var searchMethodCall = searchType switch
                {
                    SearchType.ContainsCaseInsensitive
                        => GetContainsCaseInsensitiveMethodCall(propertyExpression, searchConstant),

                    SearchType.StartsWithCaseSensitive
                        => GetStartsWithCaseSensitiveMethodCall(propertyExpression, searchConstant),

                    SearchType.ExactMatchCaseInsensitive
                        => GetExactMatchCaseInsensitiveMethodCall(propertyExpression, searchConstant),

                    _ => throw new InvalidOperationException("Incorrect search type was used.")
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

    private static MethodCallExpression GetContainsCaseInsensitiveMethodCall(
        MemberExpression propertyExpression, ConstantExpression searchConstant)
    {
        // entity => Regex.IsMatch(((entityType)entity).propertyName, searchWord, RegexOptions.IgnoreCase)
        return Expression.Call(
            isMatch, propertyExpression, searchConstant, Expression.Constant(RegexOptions.IgnoreCase));
    }

    private static readonly MethodInfo isMatch =
        typeof(Regex).GetMethod(nameof(Regex.IsMatch), new[] { typeof(string), typeof(string), typeof(RegexOptions) })!;

    private static MethodCallExpression GetStartsWithCaseSensitiveMethodCall(
        MemberExpression propertyExpression, ConstantExpression searchConstant)
    {
        var property = GetConvertedExpressionWhenPropertyIsNotString(propertyExpression);

        // entity => ((entityType)entity).propertyName.StartsWith(searchConstant)
        return Expression.Call(property, startsWith, searchConstant);
    }

    private static readonly MethodInfo startsWith =
        typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) })!;

    private static MethodCallExpression GetExactMatchCaseInsensitiveMethodCall(
        MemberExpression propertyExpression, ConstantExpression searchConstant)
    {
        var property = GetConvertedExpressionWhenPropertyIsNotString(propertyExpression);

        // entity => ((entityType)entity).propertyName.Equal(searchConstant)
        return Expression.Call(property, equal, searchConstant);
    }

    private static readonly MethodInfo equal =
        typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string) })!;

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
}
