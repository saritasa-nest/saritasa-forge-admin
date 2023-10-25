using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Blazor.Pages;
using Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;

namespace Saritasa.NetForge.Blazor.Controls;

/// <summary>
/// Entity details.
/// </summary>
[Route("/details/{id:guid}")]
public partial class EntityDetails : MvvmComponentBase<EntityDetailsViewModel>
{
    /// <summary>
    /// Entity id.
    /// </summary>
    [Parameter]
    public Guid Id { get; set; }

    /// <inheritdoc/>
    protected override EntityDetailsViewModel CreateViewModel()
    {
        return ViewModelFactory.Create<EntityDetailsViewModel>(Id);
    }
}
