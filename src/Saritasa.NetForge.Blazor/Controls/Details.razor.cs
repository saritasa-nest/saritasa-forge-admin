using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Blazor.Pages;
using Saritasa.NetForge.Mvvm.ViewModels.Details;

namespace Saritasa.NetForge.Blazor.Controls;

/// <summary>
/// Entity details.
/// </summary>
[Route("/details/{id:guid}")]
public partial class Details : MvvmComponentBase<DetailsViewModel>
{
    /// <summary>
    /// Entity id.
    /// </summary>
    [Parameter]
    public Guid Id { get; set; }

    /// <inheritdoc/>
    protected override DetailsViewModel CreateViewModel()
    {
        return ViewModelFactory.Create<DetailsViewModel>(Id);
    }
}
