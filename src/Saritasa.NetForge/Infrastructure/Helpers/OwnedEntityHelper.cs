using System.Linq.Expressions;
using Saritasa.NetForge.Domain.Extensions;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Infrastructure.Helpers;

/// <summary>
/// Helps work with <see href="https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities">Owned entities</see>.
/// </summary>
public static class OwnedEntityHelper
{
    /// <summary>
    /// Ensures that <paramref name="parentInstance"/> has initialized navigation instance.
    /// If it is not initialized this method will do initialization with default values for every property.
    /// </summary>
    /// <remarks>
    /// It uses expressions to put initialized value directly to <paramref name="parentInstance"/> reference.
    /// We need it because owned entity has properties that should be treated as normal properties of the entity.
    /// But when owned navigation is null, we will not be able to change property values.
    /// </remarks>
    public static void EnsureNavigationInstance(object parentInstance, NavigationMetadataDto ownedNavigation)
    {
        var navigationValue = parentInstance.GetPropertyValue(ownedNavigation.Name);
        if (navigationValue is not null)
        {
            return;
        }

        var entityType = parentInstance.GetType();
        var entityParameter = Expression.Parameter(entityType, $"{entityType.Name}Parameter");
        var navigationExpression = Expression.Property(entityParameter, ownedNavigation.Name);

        var constructorInfo = ownedNavigation.ClrType!.GetConstructors()[0];
        var newExpression = Expression.New(constructorInfo);
        var newNavigationAssign = Expression.Assign(navigationExpression, newExpression);
        var lambda = Expression.Lambda(newNavigationAssign, entityParameter);
        lambda.Compile().DynamicInvoke(parentInstance);
    }
}
