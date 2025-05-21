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
    private const string InvalidExpressionMessage = "Invalid expression.";

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

            _ => throw new ArgumentException(InvalidExpressionMessage, nameof(expression))
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

    /// <summary>
    /// Gets full member name of the property expression.
    /// </summary>
    /// <param name="propertyExpression">Property expression.</param>
    /// <returns>
    /// Example: when expression is <c>product => product.Shop.Address.Street</c>,
    /// then this method will return <c>Shop.Address.Street</c>.</returns>
    /// <exception cref="ArgumentException">Thrown when invalid expression is used.</exception>
    public static string GetFullMemberName(this Expression propertyExpression)
    {
        try
        {
            if (propertyExpression is LambdaExpression lambda)
            {
                if (lambda.Body is MemberExpression memberExpression)
                {
                    return memberExpression.GetFullPropertyPath();
                }

                // It is used for cases when MemberExpression is converted to some type
                if (lambda.Body is UnaryExpression unaryExpression)
                {
                    return unaryExpression.Operand.GetFullPropertyPath();
                }
            }
        }
        catch
        {
            throw new ArgumentException(InvalidExpressionMessage, nameof(propertyExpression));
        }

        throw new ArgumentException(InvalidExpressionMessage, nameof(propertyExpression));
    }

    private static string GetFullPropertyPath(this Expression expression)
    {
        var stringExpression = expression.ToString();
        var propertyPathStartIndex = stringExpression.IndexOf('.') + 1;
        return propertyPathStartIndex > 0 ? stringExpression[propertyPathStartIndex..] : stringExpression;
    }
}
