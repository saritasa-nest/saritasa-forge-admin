using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Field for a navigation collection.
/// </summary>
/// <typeparam name="T">Underlying type of the collection.</typeparam>
public partial class NavigationCollectionField<T> : CustomField
{
    [Inject]
    private IDialogService DialogService { get; set; } = null!;

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

    private readonly DialogOptions navigationDetailsDialogOptions = new()
    {
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Large,
        FullWidth = true,
        NoHeader = true
    };

    private Task OpenDialogAsync(DialogOptions options, DialogParameters parameters)
    {
        return DialogService.ShowAsync<NavigationEntityEditDialog>("Edit", parameters, options);
    }
}
