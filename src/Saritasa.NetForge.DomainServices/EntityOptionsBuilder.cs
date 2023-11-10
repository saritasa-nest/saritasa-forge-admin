using System.Linq.Expressions;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.DomainServices.Extensions;

namespace Saritasa.NetForge.DomainServices;

/// <summary>
/// Builder class for configuring entity options within the admin panel.
/// </summary>
/// <typeparam name="TEntity">The type of entity for which options are being configured.</typeparam>
public class EntityOptionsBuilder<TEntity> where TEntity : class
{
    private readonly EntityOptions options = new(typeof(TEntity));

    /// <summary>
    /// Sets the description for the entity being configured.
    /// </summary>
    /// <param name="description">The description to set for the entity.</param>
    public EntityOptionsBuilder<TEntity> SetDescription(string description)
    {
        options.Description = description;
        return this;
    }

    /// <summary>
    /// Sets the name for the entity being configured.
    /// </summary>
    /// <param name="name">The name to set for the entity.</param>
    public EntityOptionsBuilder<TEntity> SetDisplayName(string name)
    {
        options.DisplayName = name;
        return this;
    }

    /// <summary>
    /// Sets the plural name for the entity being configured.
    /// </summary>
    /// <param name="pluralName">The plural name to set for the entity.</param>
    public EntityOptionsBuilder<TEntity> SetPluralName(string pluralName)
    {
        options.PluralName = pluralName;
        return this;
    }

    /// <summary>
    /// Sets whether the entity should be hidden from the view.
    /// </summary>
    public EntityOptionsBuilder<TEntity> SetIsHidden(bool isHidden)
    {
        options.IsHidden = isHidden;
        return this;
    }

    /// <summary>
    /// Sets the group for the entity being configured.
    /// </summary>
    public EntityOptionsBuilder<TEntity> SetGroup(string groupName)
    {
        options.GroupName = groupName;
    }

    /// <summary>
    /// Configure custom search.
    /// </summary>
    /// <param name="searchFunction">Custom search function.</param>
    public EntityOptionsBuilder<TEntity> ConfigureSearch(
        Func<IServiceProvider?, IQueryable<TEntity>, string, IQueryable<TEntity>> searchFunction)
    {
        // We need to override provided Func to perform successful cast from IQueryable<TEntity> to IQueryable<object>
        options.SearchFunction = (serviceProvider, query, searchTerm) =>
                searchFunction.Invoke(serviceProvider, query.Cast<TEntity>(), searchTerm);
        return this;
    }

    /// <summary>
    /// Creates and returns the configured entity options.
    /// </summary>
    public EntityOptions Create()
    {
        return options;
    }

    /// <summary>
    /// Configures options for specific entity's property.
    /// </summary>
    /// <param name="propertyExpression">
    /// Expression that represents property. For example: <c>entity => entity.Name</c>.
    /// </param>
    /// <param name="propertyOptionsBuilderAction">An action that builds property options.</param>
    public EntityOptionsBuilder<TEntity> ConfigureProperty(
        Expression<Func<TEntity, object?>> propertyExpression,
        Action<PropertyOptionsBuilder> propertyOptionsBuilderAction)
    {
        var propertyOptionsBuilder = new PropertyOptionsBuilder();
        propertyOptionsBuilderAction.Invoke(propertyOptionsBuilder);

        var propertyName = propertyExpression.GetMemberName();
        var propertyOptions = propertyOptionsBuilder.Create(propertyName);

        options.PropertyOptions.Add(propertyOptions);
        return this;
    }

    /// <summary>
    /// Adds calculated properties for the specified entity type.
    /// </summary>
    /// <param name="propertyExpressions">An array of lambda expressions representing calculated properties.</param>
    public EntityOptionsBuilder<TEntity> AddCalculatedProperties(
        params Expression<Func<TEntity, object?>>[] propertyExpressions)
    {
        var propertyNames = propertyExpressions.Select(expression => expression.GetMemberName());
        options.CalculatedPropertyNames.AddRange(propertyNames);
        return this;
    }
}
