using System.Linq.Expressions;
using System.Reflection;

namespace Saritasa.NetForge.Domain.Extensions;

/// <summary>
/// Contains methods that work with reflection.
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    /// Gets value of <paramref name="obj"/> property with given name.
    /// </summary>
    public static object? GetPropertyValue(this object? obj, string propertyName)
    {
        return obj?.GetType().GetProperty(propertyName)?.GetValue(obj);
    }

    /// <summary>
    /// Gets value of <paramref name="obj"/> property with given name.
    /// </summary>
    /// <param name="obj">Object instance to get property value from.</param>
    /// <param name="propertyPath">
    /// Property path excluding root entity. For example: Entity - <c>Shop</c>, Property Path - <c>OwnerContact.Email</c>.
    /// </param>
    /// <returns>Property value.</returns>
    public static object? GetNestedPropertyValue(this object? obj, string propertyPath)
    {
        if (obj is null)
        {
            return null;
        }

        var entityType = obj.GetType();

        // entity
        var entity = Expression.Parameter(typeof(object), "entity");

        // (entityType)entity
        var convertedEntity = Expression.Convert(entity, entityType);

        // ((entityType)entity).propertyName
        var propertyExpression = ExpressionExtensions.GetPropertyExpression(convertedEntity, propertyPath);

        // entity => ((entityType)entity).propertyName
        var lambda = Expression.Lambda(propertyExpression, entity);
        var compiledLambda = lambda.Compile();
        try
        {
            return compiledLambda.DynamicInvoke(obj);
        }
        catch (Exception exception) when (exception.InnerException is NullReferenceException)
        {
            // If any navigation in propertyPath is null or just property has null value
            // then NullReferenceException will be thrown.
            // So we handle to have behavior similar to conditional access
            // For example: Product.Shop?.OwnerContact?.Email
            return null;
        }
    }

    /// <summary>
    /// Sets value of <paramref name="obj"/> property with given name.
    /// </summary>
    public static void SetPropertyValue(this object? obj, string propertyName, object value)
    {
        obj?.GetType().GetProperty(propertyName)?.SetValue(obj, value);
    }

    /// <summary>
    /// Sets value of <paramref name="obj"/> property with given name.
    /// </summary>
    /// <param name="obj">Property of this instance will be changed.</param>
    /// <param name="propertyPath">
    /// Property path excluding root entity. For example: Root Entity - <c>Shop</c>, Property Path - <c>Address.Street</c>.
    /// <paramref name="obj"/> contains instance of root entity.
    /// </param>
    /// <param name="value">Value to set.</param>
    public static void SetNestedPropertyValue(this object? obj, string propertyPath, object? value)
    {
        if (obj is null)
        {
            return;
        }

        var entityType = obj.GetType();

        // entity
        var entity = Expression.Parameter(typeof(object), "entity");

        // (entityType)entity
        var convertedEntity = Expression.Convert(entity, entityType);

        // ((entityType)entity).propertyName
        var propertyExpression = ExpressionExtensions.GetPropertyExpression(convertedEntity, propertyPath);

        // ((entityType)entity).propertyName = value
        var assignExpression = Expression.Assign(propertyExpression, Expression.Constant(value));

        // entity => ((entityType)entity).propertyName = value
        var lambda = Expression.Lambda(assignExpression, entity);
        lambda.Compile().DynamicInvoke(obj);
    }
}
