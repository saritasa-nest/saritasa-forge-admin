using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Mvvm.ViewModels.CreateEntity;

namespace Saritasa.NetForge.Blazor.Pages;

/// <summary>
/// Create entity page.
/// </summary>
[Route("/entities/{stringId}/create")]
public partial class CreateEntity : MvvmComponentBase<CreateEntityViewModel>
{
    /// <summary>
    /// Entity id.
    /// </summary>
    [Parameter]
    public string StringId { get; set; } = null!;

    /// <inheritdoc/>
    protected override CreateEntityViewModel CreateViewModel()
    {
        return ViewModelFactory.Create<CreateEntityViewModel>(StringId);
    }
}
