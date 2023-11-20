using Microsoft.AspNetCore.Components;
using MudBlazor;
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

    private EntityDetailsViewModel entityDetailsViewModel = null!;

    private readonly List<BreadcrumbItem> items = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("Entities", href: "/admin"),
    };

    /// <inheritdoc/>
    protected override EntityDetailsViewModel CreateViewModel()
    {
        entityDetailsViewModel = ViewModelFactory.Create<EntityDetailsViewModel>(StringId);
        return entityDetailsViewModel;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        // Add BreadcrumbItem with the new href value because can not get StringId directly.
        items.Add(new BreadcrumbItem(entityDetailsViewModel.Model.PluralName, href: $"/admin/entities/{StringId}"));
    }
}
