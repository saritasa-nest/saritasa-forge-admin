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

        // entity => entity
        var entity = Expression.Parameter(typeof(object), Entity);

        // entity => (entityType)entity
        var convertedEntity = Expression.Convert(entity, entityType);

        Expression? combinedSearchExpressions = null;

        var searchEntries = GetSearchEntries(searchString);
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

    /// <summary>
    /// Retrieves search entries from <paramref name="searchString"/> using regular expression. Handles single and double quotes.
    /// </summary>
    /// <param name="searchString">Search string.</param>
    /// <returns>Collection of search entries.</returns>
    /// <remarks>
    /// For example if search string is: <c>"William William" 'Test Test' "Single" 'double' also empty</c>,
    /// then result will have 6 entries:
    /// <list type="bullet">
    ///     <item>William William</item>
    ///     <item>Test Test</item>
    ///     <item>Single</item>
    ///     <item>double</item>
    ///     <item>also</item>
    ///     <item>empty</item>
    /// </list>
    /// </remarks>
    private static IEnumerable<string> GetSearchEntries(string searchString)
    {
        const string splitSearchStringRegex = """(?<=["])[^"]*(?="\s|"$)|(?<=['])[^']*(?='\s|'$)|[^\s"']+""";
        var matches = Regex.Matches(searchString, splitSearchStringRegex);

        return matches.Select(match => match.Value);
    }

    private static readonly MethodInfo isMatch =
        typeof(Regex).GetMethod(nameof(Regex.IsMatch), new[] { typeof(string), typeof(string), typeof(RegexOptions) })!;

    private static readonly MethodInfo startsWith =
        typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) })!;

    /// <summary>
    /// Gets call of method similar to <see cref="string.Contains(string)"/> but case insensitive.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="Regex.IsMatch(string, string, RegexOptions)"/> with <see cref="RegexOptions.IgnoreCase"/>.
    /// </remarks>
    private static MethodCallExpression GetContainsCaseInsensitiveMethodCall(
        MemberExpression propertyExpression, ConstantExpression searchConstant)
    {
        // entity => Regex.IsMatch(((entityType)entity).propertyName, searchWord, RegexOptions.IgnoreCase)
        return Expression.Call(
            isMatch, propertyExpression, searchConstant, Expression.Constant(RegexOptions.IgnoreCase));
    }

    /// <summary>
    /// Gets call of <see cref="string.StartsWith(string)"/>.
    /// </summary>
    /// <remarks>
    /// If given search entry is not string, <c>ToString</c> will be called.
    /// </remarks>
    private static MethodCallExpression GetStartsWithCaseSensitiveMethodCall(
        MemberExpression propertyExpression, ConstantExpression searchConstant)
    {
        var property = GetConvertedExpressionWhenPropertyIsNotString(propertyExpression);

        // entity => ((entityType)entity).propertyName.StartsWith(searchConstant)
        return Expression.Call(property, startsWith, searchConstant);
    }

    /// <summary>
    /// Gets call of method similar to <see cref="string.Equals(string)"/> but case insensitive.
    /// </summary>
    /// <remarks>
    /// Adds <c>^</c> at the start and <c>$</c> at the end of search entry to make exact match.
    /// Uses <see cref="Regex.IsMatch(string, string, RegexOptions)"/> with <see cref="RegexOptions.IgnoreCase"/>.
    /// </remarks>
    private static MethodCallExpression GetExactMatchCaseInsensitiveMethodCall(
        MemberExpression propertyExpression, ConstantExpression searchConstant)
    {
        var property = GetConvertedExpressionWhenPropertyIsNotString(propertyExpression);

        var exactMatchSearchConstant = Expression.Constant($"^{searchConstant.Value}$");

        // entity => Regex.IsMatch(((entityType)entity).propertyName, ^searchWord$, RegexOptions.IgnoreCase)
        return Expression.Call(
            isMatch, property, exactMatchSearchConstant, Expression.Constant(RegexOptions.IgnoreCase));
    }

    /// <summary>
    /// When <paramref name="propertyExpression"/> does not represent <see langword="string"/>
    /// then <c>ToString</c> will be called to underlying property.
    /// </summary>
    private static Expression GetConvertedExpressionWhenPropertyIsNotString(MemberExpression propertyExpression)
    {
        var propertyType = ((PropertyInfo)propertyExpression.Member).PropertyType;

        if (propertyType != typeof(string))
        {
            return Expression.Call(propertyExpression, typeof(object).GetMethod(nameof(ToString))!);
        }

        return propertyExpression;
    }
}
