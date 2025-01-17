using Saritasa.NetForge.Blazor.Infrastructure.Helpers;

namespace Saritasa.NetForge.DomainServices.Comparers;

/// <summary>
/// Comparer for objects. Uses their <see cref="object.ToString()"/> methods.
/// </summary>
/// <remarks>
/// Useful to compare objects when they have <see cref="object.ToString()"/> overridden.
/// </remarks>
public class ObjectComparer<T> : IEqualityComparer<T>
{
    /// <inheritdoc />
    public bool Equals(T? x, T? y)
    {
        return x.ConvertToString() == y.ConvertToString();
    }

    /// <inheritdoc />
    public int GetHashCode(T obj)
    {
        return obj.ConvertToString().GetHashCode();
    }
}
