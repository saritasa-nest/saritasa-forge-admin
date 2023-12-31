﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.Mvvm.Navigation;
using Saritasa.NetForge.Mvvm.ViewModels.CreateEntity;
using Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;

namespace Saritasa.NetForge.Blazor.Pages;

/// <summary>
/// Entity details.
/// </summary>
[Route("/entities/{stringId}")]
public partial class EntityDetails : MvvmComponentBase<EntityDetailsViewModel>
{
    [Inject]
    private INavigationService NavigationService { get; set; } = null!;

    /// <summary>
    /// Entity id.
    /// </summary>
    [Parameter]
    public string StringId { get; set; } = null!;

    private readonly List<BreadcrumbItem> breadcrumbItems = new();

    /// <inheritdoc/>
    protected override EntityDetailsViewModel CreateViewModel()
    {
        return ViewModelFactory.Create<EntityDetailsViewModel>(StringId);
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        var adminPanelEndpoint = AdminOptions.AdminPanelEndpoint;

        breadcrumbItems.Add(new BreadcrumbItem("Entities", href: adminPanelEndpoint));
        // Add BreadcrumbItem with the new href value because can not get StringId directly.
        breadcrumbItems.Add(new BreadcrumbItem(ViewModel.Model.PluralName, href: $"{adminPanelEndpoint}/entities/{StringId}"));
    }

    private void NavigateToCreation()
    {
        NavigationService.NavigateTo<CreateEntityViewModel>(parameters: StringId);
    }
}
