namespace Saritasa.NetForge.DomainServices.Comparers;

/// <summary>
/// Comparer for select control.
/// </summary>
public class ObjectComparer<T> : IEqualityComparer<T>
{
    /// <inheritdoc />
    public bool Equals(T? x, T? y)
    {
        return x?.ToString() == y?.ToString();
    }

    /// <inheritdoc />
    public int GetHashCode(T? obj)
    {
        return obj is null
            ? 0
            : obj.ToString()!.GetHashCode();
    }
}
