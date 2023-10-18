using Microsoft.AspNetCore.Components;
using MudBlazor;
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

    private static object? GetPropertyValue(object source, string propertyName)
    {
        return source.GetType().GetProperty(propertyName)!.GetValue(source);
    }

    //private async Task<GridData<object>> LoadGridData(GridState<object> state)
    //{
    //    _requestDto.Page = state.Page;
    //    _requestDto.PageSize = state.PageSize;

    //    var apiResponse = await GetAnimalList(_requestDto);
    //    var data = new GridState<object>
    //    {
    //        Items = apiResponse.Items,
    //        TotalItems = apiResponse.ItemTotalCount
    //    };

    //    return data;
    //}
}
