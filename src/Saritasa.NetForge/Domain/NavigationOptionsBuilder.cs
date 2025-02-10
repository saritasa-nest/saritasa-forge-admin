using System.Linq.Expressions;
using Saritasa.NetForge.Domain.Extensions;
using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.Domain;

/// <summary>
/// Builder class for configuring entity's navigation options.
/// </summary>
public class NavigationOptionsBuilder<TEntity>
{
    private readonly NavigationOptions options = new();

    /// <summary>
    /// Creates property options.
    /// </summary>
    /// <param name="propertyName">Property name to create options for.</param>
    /// <returns>Property options.</returns>
    public NavigationOptions Create(string propertyName)
    {
        options.PropertyName = propertyName;

        return options;
    }

    /// <summary>
    /// Includes navigation's property.
    /// </summary>
    /// <param name="propertyExpression">Lambda expression representing property to include.</param>
    /// <param name="propertyOptionsBuilderAction">An action that builds property options.</param>
    public NavigationOptionsBuilder<TEntity> IncludeProperty(
        Expression<Func<TEntity, object?>> propertyExpression,
        Action<PropertyOptionsBuilder>? propertyOptionsBuilderAction = null)
    {
        var propertyOptionsBuilder = new PropertyOptionsBuilder();
        propertyOptionsBuilderAction?.Invoke(propertyOptionsBuilder);

        var propertyName = propertyExpression.GetMemberName();
        var propertyOptions = propertyOptionsBuilder.Create(propertyName);

        options.PropertyOptions.Add(propertyOptions);
        return this;
    }

    /// <summary>
    /// Includes navigation's calculated property.
    /// </summary>
    /// <param name="calculatedPropertyExpression">Lambda expression representing calculated property to include.</param>
    /// <param name="calculatedPropertyOptionsBuilderAction">An action that builds calculated property options.</param>
    public NavigationOptionsBuilder<TEntity> IncludeCalculatedProperty(
        Expression<Func<TEntity, object?>> calculatedPropertyExpression,
        Action<CalculatedPropertyOptionsBuilder>? calculatedPropertyOptionsBuilderAction = null)
    {
        var calculatedPropertyOptionsBuilder = new CalculatedPropertyOptionsBuilder();
        calculatedPropertyOptionsBuilderAction?.Invoke(calculatedPropertyOptionsBuilder);

        var calculatedPropertyName = calculatedPropertyExpression.GetMemberName();
        var calculatedPropertyOptions = calculatedPropertyOptionsBuilder.Create(calculatedPropertyName);

        options.CalculatedPropertyOptions.Add(calculatedPropertyOptions);
        return this;
    }

    /// <summary>
    /// Adds navigation property to the entity.
    /// </summary>
    /// <param name="navigationExpression">Lambda expression representing navigation to include.</param>
    /// <param name="navigationOptionsBuilderAction">An action that builds navigation options.</param>
    public NavigationOptionsBuilder<TEntity> IncludeNavigation<TNavigation>(
        Expression<Func<TEntity, object?>> navigationExpression,
        Action<NavigationOptionsBuilder<TNavigation>> navigationOptionsBuilderAction)
    {
        var navigationOptions = NavigationOptionsHelper.CreateNavigationOptions(navigationExpression, navigationOptionsBuilderAction);
        options.NavigationsOptions.Add(navigationOptions);
        return this;
    }
}
