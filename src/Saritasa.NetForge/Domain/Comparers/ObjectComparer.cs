using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;
using Saritasa.NetForge.Infrastructure.Helpers;

namespace Saritasa.NetForge.Domain.Comparers;

/// <summary>
/// Comparer for objects.
/// Uses <see cref="EntityInstanceExtensions.ConvertToString(object?, GetEntityDto)"/> to compare.
/// </summary>
public class ObjectComparer<T> : IEqualityComparer<T>
{
    private GetEntityDto EntityMetadata { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public ObjectComparer(GetEntityDto entityMetadata)
    {
        EntityMetadata = entityMetadata;
    }

    /// <inheritdoc />
    public bool Equals(T? x, T? y)
    {
        if (x is null && y is null)
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        var xType = x.GetType();
        var yType = y.GetType();

        if (xType.IsLazyLoadingProxy() || yType.IsLazyLoadingProxy())
        {
            return xType.GetPocoType() == yType.GetPocoType();
        }

        if (xType != yType)
        {
            return false;
        }

        return x.ConvertToString(EntityMetadata) == y.ConvertToString(EntityMetadata);
    }

    /// <inheritdoc />
    public int GetHashCode(T obj)
    {
        return obj.ConvertToString(EntityMetadata).GetHashCode();
    }
}
