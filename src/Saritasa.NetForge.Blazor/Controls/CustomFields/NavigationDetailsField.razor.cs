using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Field for a navigation entity details.
/// </summary>
public partial class NavigationDetailsField : CustomField
{
    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Property value.
    /// </summary>
    public object? PropertyValue
    {
        get => EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance);
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value);
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
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
