using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Mvvm.ViewModels;

namespace Saritasa.NetForge.Blazor.Pages;

/// <summary>
/// Component base on view model.
/// </summary>
/// <typeparam name="TViewModel">View model type.</typeparam>
public class MvvmComponentBase<TViewModel> : ComponentBase
    where TViewModel : BaseViewModel
{
    /// <summary>
    /// View model factory.
    /// </summary>
    [Inject]
    protected ViewModelFactory ViewModelFactory { get; private set; }

    /// <summary>
    /// View model.
    /// </summary>
    public TViewModel ViewModel { get; private set; }

    /// <inheritdoc />
    protected sealed override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        ViewModel = CreateViewModel();

        await OnInitializedComponentAsync();

        await OnViewModelPreLoadingAsync();

        await ViewModel.LoadAsync();
    }

    /// <summary>
    /// Handler for component async initialization.
    /// </summary>
    protected virtual Task OnInitializedComponentAsync()
        => Task.CompletedTask;

    /// <summary>
    /// Handler for view model pre loading.
    /// </summary>
    protected virtual Task OnViewModelPreLoadingAsync()
        => Task.CompletedTask;

    /// <summary>
    /// Create view model for current component.
    /// </summary>
    /// <returns>Created view model.</returns>
    protected virtual TViewModel CreateViewModel()
        => ViewModelFactory.Create<TViewModel>();
}
