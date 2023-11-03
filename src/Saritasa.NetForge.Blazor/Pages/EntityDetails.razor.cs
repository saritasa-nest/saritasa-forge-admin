using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;

namespace Saritasa.NetForge.Blazor.Pages;

/// <summary>
/// Entity details.
/// </summary>
[Route("/entities/{stringId}")]
public partial class EntityDetails : MvvmComponentBase<EntityDetailsViewModel>
{
    /// <summary>
    /// Entity id.
    /// </summary>
    [Parameter]
    public string StringId { get; set; } = null!;

    /// <inheritdoc/>
    protected override EntityDetailsViewModel CreateViewModel()
    {
        return ViewModelFactory.Create<EntityDetailsViewModel>(StringId);
    }
}
