using System.Linq.Expressions;

namespace Saritasa.NetForge.Domain.Extensions;

/// <summary>
/// Extensions to expressions.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Separator between properties. For example: <c>Shop.Address.Street</c>.
    /// </summary>
    public const char PropertySeparator = '.';

    /// <summary>
    /// Gets <paramref name="expression"/> member name.
    /// </summary>
    /// <param name="expression">Expression.</param>
    /// <remarks>
    /// When passed <paramref name="expression"/> like <c>entity => entity.Name</c>, then this method will return "Name".
    /// </remarks>
    /// <returns>Member name.</returns>
    public static string GetMemberName<T>(this Expression<Func<T, object?>> expression)
    {
        return GetMemberName(expression.Body);
    }

    /// <summary>
    /// Gets expression member name.
    /// </summary>
    /// <param name="expression">Expression.</param>
    /// <returns>Member name.</returns>
    public static string GetMemberName(this Expression expression)
    {
        return expression switch
        {
            MemberExpression memberExpression => memberExpression.Member.Name,

            MethodCallExpression methodCallExpression => methodCallExpression.Method.Name,

            UnaryExpression unaryExpression => GetMemberName(unaryExpression),

            null => throw new ArgumentNullException(nameof(expression), "Expression can't be null"),

            _ => throw new ArgumentException("Invalid expression.", nameof(expression))
        };
    }

    private static string GetMemberName(UnaryExpression unaryExpression)
    {
        if (unaryExpression.Operand is MethodCallExpression methodExpression)
        {
            return methodExpression.Method.Name;
        }

        return ((MemberExpression)unaryExpression.Operand).Member.Name;
    }

    /// <summary>
    /// Gets property expression that contains in <paramref name="entity"/>.
    /// To get property, just pass its name in <paramref name="propertyName"/>. For example: <c>Description</c>.
    /// This method supports getting nested properties.
    /// For example: You can pass <paramref name="propertyName"/> as <c>Address.Id</c>.
    /// </summary>
    /// <param name="entity">
    /// Entity <see cref="ParameterExpression"/>. Can have different <see cref="Expression"/> representation.
    /// For example:
    /// When you use <see cref="Expression.Convert(Expression, Type)"/> to <see cref="ParameterExpression"/>.
    /// </param>
    /// <param name="propertyName">Property name.</param>
    /// <returns>Property <see cref="MemberExpression"/>.</returns>
    public static MemberExpression GetPropertyExpression(Expression entity, string propertyName)
    {
        var body = entity;
        foreach (var member in propertyName.Split(PropertySeparator))
        {
            body = Expression.Property(body, member);
        }

        return (MemberExpression)body;
    }

    /// <summary>
    /// Gets property expression and check that navigations are not null in case of nested property.
    /// </summary>
    /// <param name="entity">
    /// Entity <see cref="ParameterExpression"/>. Can have different <see cref="Expression"/> representation.
    /// For example:
    /// When you use <see cref="Expression.Convert(Expression, Type)"/> to <see cref="ParameterExpression"/>.
    /// </param>
    /// <param name="propertyPath">Property path.</param>
    /// <returns>
    /// Property expression with null checks on navigations.
    /// For example: if <paramref name="propertyPath"/> is <c>Shop.Address.Street</c>,
    /// then this method will return expression like <c>Shop.Address?.Street</c>.
    /// </returns>
    /// <remarks>
    /// Use case where we need null check in property expression:
    /// When the property expression will be used in <see cref="IEnumerable{T}"/>.
    /// In case of <see cref="IQueryable"/> use <see cref="GetPropertyExpression"/>.
    /// </remarks>
    public static Expression GetPropertyExpressionWithNullCheck(Expression entity, string propertyPath)
    {
        var body = entity;
        var splitPropertyPath = propertyPath.Split(PropertySeparator);
        for (var propertyIndex = 0; propertyIndex < splitPropertyPath.Length; propertyIndex++)
        {
            var propertyName = splitPropertyPath[propertyIndex];
            Expression property = Expression.Property(body, propertyName);

            var isRootProperty = propertyIndex == 0;
            var isLastProperty = splitPropertyPath.Length == propertyIndex + 1;
            var isSingleProperty = isRootProperty && isLastProperty;
            if (isSingleProperty)
            {
                // Note that there are converting property to object.
                // We need it to sort types that are not string. For example, numbers.
                body = Expression.Convert(property, typeof(object));
            }
            else if (isRootProperty)
            {
                // Root property can't be null, so we don't perform the null check.
                body = property;
            }
            else if (isLastProperty)
            {
                // We use GetPropertyExpression to remove null checks from "body" expression.
                // Otherwise, expression will fail when used in some cases, for example in a collection Select method.
                var propertyExpression = GetPropertyExpression(entity, propertyPath);
                body = Expression.Condition(
                    Expression.Equal(body, Expression.Constant(null, body.Type)),
                    Expression.Constant(null, typeof(object)),
                    Expression.Convert(propertyExpression, typeof(object)));
            }
            else
            {
                // In case of nested property we check that navigation that contains property is not null.
                // For example: Shop has Name property, we check that Shop is not null.
                // (Shop == null) ? null : Shop.Name
                body = Expression.Condition(
                    Expression.Equal(body, Expression.Constant(null, body.Type)),
                    Expression.Constant(null, property.Type),
                    property);
            }
        }

        return body;
    }

    /// <summary>
    /// Combines all expressions to one with <see langword="AND"/> operator between.
    /// </summary>
    /// <returns>
    /// If <paramref name="expressionToCombineWith"/> is <c>null</c>,
    /// then <paramref name="expression"/> will be returned unchanged.
    /// </returns>
    public static Expression AddAndBetween(this Expression? expressionToCombineWith, Expression expression)
    {
        if (expressionToCombineWith is not null)
        {
            // Example:
            // entity => ((entityType)entity).propertyName.Equals(searchEntry) ||
            //           ((entityType)entity).propertyName2.StartsWith(searchEntry) ||
            //           ...) &&
            //           ((entityType)entity).propertyName.Equals(searchEntry2) ||
            //           ((entityType)entity).propertyName2.StartsWith(searchEntry2) ||
            //           ...) && ...
            return Expression.And(expressionToCombineWith, expression);
        }

        return expression;
    }

    /// <summary>
    /// Combines expressions to one with <see langword="OR"/> operator between.
    /// </summary>
    /// <returns>
    /// If <paramref name="expressionToCombineWith"/> is <c>null</c>,
    /// then <paramref name="expression"/> will be returned unchanged.
    /// </returns>
    public static Expression AddOrBetween(this Expression? expressionToCombineWith, Expression expression)
    {
        if (expressionToCombineWith is not null)
        {
            // Add OR operator between every searchable property using search entry
            // Example:
            // entity => ((entityType)entity).propertyName.Equals(searchEntry) ||
            //           ((entityType)entity).propertyName2.StartsWith(searchEntry) ||
            //           ...
            return Expression.OrElse(expressionToCombineWith, expression);
        }

        return expression;
    }
}
