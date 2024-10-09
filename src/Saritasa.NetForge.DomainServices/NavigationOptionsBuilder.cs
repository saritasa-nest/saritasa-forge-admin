using System.Linq.Expressions;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.DomainServices.Extensions;

namespace Saritasa.NetForge.DomainServices;

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
    /// Sets whether display navigation entity details.
    /// </summary>
    public NavigationOptionsBuilder<TEntity> SetDisplayDetails(bool displayDetails)
    {
        options.DisplayDetails = displayDetails;
        return this;
    }

    /// <summary>
    /// Sets whether edit navigation entity details.
    /// </summary>
    public NavigationOptionsBuilder<TEntity> SetAsEditable()
    {
        options.EditDetails = true;
        return this;
    }
}
