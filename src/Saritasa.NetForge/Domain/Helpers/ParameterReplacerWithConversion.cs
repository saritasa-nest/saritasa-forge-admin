using System.Linq.Expressions;

namespace Saritasa.NetForge.Domain.Helpers;

/// <summary>
/// Replaces parameter expression with the provided one. Also converts it to target type.
/// </summary>
public class ParameterReplacerWithConversion : ExpressionVisitor
{
    private readonly ParameterExpression newParameter;
    private readonly Type targetType;

    /// <summary>
    /// Constructor.
    /// </summary>
    public ParameterReplacerWithConversion(ParameterExpression newParameter, Type targetType)
    {
        this.newParameter = newParameter;
        this.targetType = targetType;
    }

    /// <inheritdoc />
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return Expression.Convert(newParameter, targetType);
    }
}
