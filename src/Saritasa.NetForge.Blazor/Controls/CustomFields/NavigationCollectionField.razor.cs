namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Field for a navigation collection.
/// </summary>
/// <typeparam name="T">Underlying type of the collection.</typeparam>
public partial class NavigationCollectionField<T> : CustomField
{
    /// <summary>
    /// Navigation collection.
    /// </summary>
    public IEnumerable<T> PropertyValue
    {
        get => (IEnumerable<T>)EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance)!;
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value.ToList());
    }

    private IEnumerable<T> NavigationInstances { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var propertyType = Property.ClrType!.GetGenericArguments().First();

        NavigationInstances = Service
            .GetQuery(propertyType)
            .Cast<T>()
            .OrderBy(instance => instance);
    }
}

/// <summary>
/// Comparer for select control.
/// </summary>
public class SelectComparer<T> : IEqualityComparer<T>
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
