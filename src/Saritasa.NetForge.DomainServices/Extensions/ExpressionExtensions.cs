using System.Linq.Expressions;

namespace Saritasa.NetForge.DomainServices.Extensions;

/// <summary>
/// Extensions to expressions.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Gets <paramref name="expression"/> member name.
    /// </summary>
    /// <param name="expression">Expression.</param>
    /// <remarks>
    /// When passed <paramref name="expression"/> like <c>entity => entity.Name</c>, then this method will return "Name".
    /// </remarks>
    /// <returns>Member name.</returns>
    public static string GetMemberName<T>(this Expression<Func<T, object>> expression)
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
}
