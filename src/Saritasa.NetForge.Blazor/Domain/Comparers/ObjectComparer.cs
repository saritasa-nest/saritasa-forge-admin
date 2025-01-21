using Saritasa.NetForge.Blazor.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.Blazor.Infrastructure.Helpers;

namespace Saritasa.NetForge.Blazor.Domain.Comparers;

/// <summary>
/// Comparer for objects. Uses their <see cref="object.ToString()"/> methods.
/// </summary>
/// <remarks>
/// Useful to compare objects when they have <see cref="object.ToString()"/> overridden.
/// </remarks>
public class ObjectComparer<T> : IEqualityComparer<T>
{
    private ICollection<PropertyMetadataDto> EntityProperties { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public ObjectComparer(ICollection<PropertyMetadataDto> entityProperties)
    {
        EntityProperties = entityProperties;
    }

    /// <inheritdoc />
    public bool Equals(T? x, T? y)
    {
        if (x is null && y is null)
        {
            return true;
        }

        if (x is null || y is null || x.GetType() != y.GetType())
        {
            return false;
        }

        return x.ConvertToString(EntityProperties) == y.ConvertToString(EntityProperties);
    }

    /// <inheritdoc />
    public int GetHashCode(T obj)
    {
        return obj.ConvertToString(EntityProperties).GetHashCode();
    }
}
