namespace Saritasa.NetForge.Mvvm.ViewModels;

/// <summary>
/// Base view model.
/// </summary>
public abstract class BaseViewModel
{
    /// <summary>
    /// Load view model.
    /// </summary>
    public virtual Task LoadAsync()
        => Task.CompletedTask;
}
