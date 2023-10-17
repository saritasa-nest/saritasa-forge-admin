using System.Linq.Expressions;
using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.DomainServices;

/// <summary>
/// Builder class for configuring entity options within the admin panel.
/// </summary>
/// <typeparam name="TEntity">The type of entity for which options are being configured.</typeparam>
public class EntityOptionsBuilder<TEntity> where TEntity : class
{
    private readonly EntityOptions options = new(typeof(TEntity));

    /// <summary>
    /// Instance of <typeparamref name="TEntity"/>. Used to configure properties of an <typeparamref name="TEntity"/>.
    /// </summary>
    public TEntity? Entity { get; set; }

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
    public EntityOptionsBuilder<TEntity> SetName(string name)
    {
        options.Name = name;
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
    /// Creates and returns the configured entity options.
    /// </summary>
    public EntityOptions Create()
    {
        return options;
    }

    /// <summary>
    /// Sets property as hidden. Given property will not be displayed in admin panel.
    /// </summary>
    /// <param name="propertyExpression">
    /// Expression that represents property to hide. For example: <c>entity => entity.Name</c>.
    /// </param>
    public EntityOptionsBuilder<TEntity> HasHidden(Expression<Func<TEntity, object>> propertyExpression)
    {
        var propertyOptions = GetPropertyOptions(propertyExpression);

        propertyOptions.IsHidden = true;

        return this;
    }

    private static string GetPropertyName(Expression expression)
    {
        if (expression is UnaryExpression unaryExpression)
        {
            return GetMemberName(unaryExpression);
        }

        var memberExpression = (MemberExpression)expression;
        return memberExpression.Member.Name;
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
    /// Sets new display name to property.
    /// </summary>
    /// <param name="propertyExpression">
    /// Expression that represents property to change display name. For example: <c>entity => entity.Name</c>.
    /// </param>
    /// <param name="displayName">Name to display.</param>
    public EntityOptionsBuilder<TEntity> HasDisplayName(
        Expression<Func<TEntity, object>> propertyExpression, string displayName)
    {
        var propertyOptions = GetPropertyOptions(propertyExpression);

        propertyOptions.DisplayName = displayName;

        return this;
    }

    /// <summary>
    /// Sets description to property. Displayed as tooltip when user hovering corresponding property.
    /// </summary>
    /// <param name="propertyExpression">
    /// Expression that represents property to set description. For example: <c>entity => entity.Name</c>.
    /// </param>
    /// <param name="description">Description.</param>
    public EntityOptionsBuilder<TEntity> HasDescription(
        Expression<Func<TEntity, object>> propertyExpression, string description)
    {
        var propertyOptions = GetPropertyOptions(propertyExpression);

        propertyOptions.Description = description;

        return this;
    }

    private EntityPropertyOptions GetPropertyOptions(Expression<Func<TEntity, object>> propertyExpression)
    {
        var propertyName = GetPropertyName(propertyExpression.Body);

        var propertyOptions = options.PropertyOptions
            .FirstOrDefault(propertyOptions => propertyOptions.PropertyName == propertyName);

        if (propertyOptions is null)
        {
            propertyOptions = new EntityPropertyOptions { PropertyName = propertyName };

            options.PropertyOptions.Add(propertyOptions);
        }

        return propertyOptions;
    }
}
