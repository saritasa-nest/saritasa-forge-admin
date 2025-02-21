using System.Linq.Expressions;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Domain.Extensions;

namespace Saritasa.NetForge.Domain;

/// <summary>
/// Helps with navigation options.
/// </summary>
public static class NavigationOptionsHelper
{
    /// <summary>
    /// Creates navigation options.
    /// </summary>
    public static NavigationOptions CreateNavigationOptions<TEntity, TNavigation>(
        Expression<Func<TEntity, object?>> navigationExpression,
        Action<NavigationOptionsBuilder<TNavigation>> navigationOptionsBuilderAction)
    {
        var navigationOptionsBuilder = new NavigationOptionsBuilder<TNavigation>();
        navigationOptionsBuilderAction.Invoke(navigationOptionsBuilder);

        var includedPropertyName = navigationExpression.GetMemberName();
        return navigationOptionsBuilder.Create(includedPropertyName);
    }
}
