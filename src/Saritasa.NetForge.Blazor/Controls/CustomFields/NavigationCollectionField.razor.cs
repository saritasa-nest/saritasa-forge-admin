using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

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
    public IEnumerable<T> NavigationCollection
    {
        get => (IEnumerable<T>)EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance)!;
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value.ToList());
    }

    private List<T> NavigationInstances { get; set; } = new();

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var propertyType = Property.ClrType!.GetGenericArguments().First();

        NavigationInstances = Service.GetQuery(propertyType).Cast<T>().ToList();
    }
}
