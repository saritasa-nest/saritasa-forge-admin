using AutoMapper;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.Blazor.Pages;
using Saritasa.NetForge.DomainServices.Extensions;
using Saritasa.NetForge.Mvvm.Navigation;
using Saritasa.NetForge.Mvvm.ViewModels.EditEntity;
using Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls;

public partial class EntityPropertiesDataGrid : ComponentBase
{
    [Inject]
    private IEntityService EntityService { get; set; }

    [Inject]
    private IMapper Mapper { get; set; }

    [Inject]
    private INavigationService NavigationService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ILogger<EntityDetails> Logger { get; set; } = default!;

    /// <summary>
    /// Entity id.
    /// </summary>
    [Parameter]
    public string StringId { get; set; } = null!;

    public EntityDetailsModel Model { get; set; }

    /// <summary>
    /// Search string.
    /// </summary>
    public string? SearchString { get; set; }

    /// <summary>
    /// Data grid reference.
    /// </summary>
    public MudDataGrid<object>? DataGrid { get; set; }

    /// <summary>
    /// Whether instance of the entity can be added.
    /// </summary>
    public bool CanAdd { get; set; }

    /// <summary>
    /// Whether instance of the entity can be edited.
    /// </summary>
    public bool CanEdit { get; set; }

    private Dictionary<string, object>? NonKeylessEntityDataGridAttributes { get; set; }

    /// <inheritdoc />S
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        await LoadAsync(CancellationToken.None);

        if (CanEdit)
        {
            NonKeylessEntityDataGridAttributes = new Dictionary<string, object>
            {
                { "Hover", true },
                { "RowClass", "cursor-pointer" },
                { "RowClick", EventCallback.Factory.Create<DataGridRowClickEventArgs<object>>(this, NavigateToEditing) }
            };
        }
    }

    public async Task LoadAsync(CancellationToken cancellationToken)
    {
        var entity = await EntityService.GetEntityByIdAsync(StringId, cancellationToken);
        Model = Mapper.Map<EntityDetailsModel>(entity);

        Model = Model with
        {
            Properties = Model.Properties
                .Where(property =>
                {
                    if (property is NavigationMetadataDto navigation)
                    {
                        navigation.TargetEntityProperties = navigation.TargetEntityProperties
                            .Where(targetProperty
                                => targetProperty is { IsHidden: false, IsHiddenFromListView: false })
                            .ToList();
                    }

                    return property is { IsHidden: false, IsHiddenFromListView: false };
                })
                .ToList()
        };

        CanAdd = !Model.IsKeyless;
        CanEdit = !Model.IsKeyless;
    }

    private void NavigateToEditing(DataGridRowClickEventArgs<object> row)
    {
        var primaryKeyValues = Model.Properties
            .Where(property => property.IsPrimaryKey)
            .Select(primaryKey => row.Item.GetPropertyValue(primaryKey.Name)!.ToString()!);

        NavigationService.NavigateTo<EditEntityViewModel>(
            parameters: new[] { StringId, string.Join("--", primaryKeyValues) });
    }

    /// <summary>
    /// Loads entity's data to data grid.
    /// </summary>
    /// <param name="gridState">Information about grid state. For example, current page, page size.</param>
    /// <returns>Grid data collection populated with entity's data.</returns>
    public async Task<GridData<object>> LoadEntityGridDataAsync(GridState<object> gridState)
    {
        var orderBy = gridState.SortDefinitions
            .Select(sort =>
            {
                var column = DataGrid!.RenderedColumns.First(column => column.PropertyName.Equals(sort.SortBy));
                var navigationName = column.UserAttributes["NavigationName"]?.ToString();

                return new OrderByDto
                {
                    FieldName = column.Title,
                    IsDescending = sort.Descending,
                    NavigationName = navigationName
                };
            })
            .ToList();

        if (!orderBy.Any())
        {
            var primaryKeyName = Model.Properties.FirstOrDefault(property => property.IsPrimaryKey)?.Name;

            if (primaryKeyName is not null)
            {
                orderBy.Add(new OrderByDto
                {
                    FieldName = primaryKeyName
                });
            }
        }

        var searchOptions = new SearchOptions
        {
            Page = gridState.Page + 1,
            PageSize = gridState.PageSize,
            SearchString = SearchString,
            OrderBy = orderBy
        };

        var entityData = await EntityService
            .SearchDataForEntityAsync(Model.ClrType, Model.Properties, searchOptions, Model.SearchFunction, Model.CustomQueryFunction);

        var data = new GridData<object>
        {
            Items = entityData.Items,
            TotalItems = entityData.Metadata.TotalCount
        };

        return data;
    }

    /// <summary>
    /// Searches data by search string.
    /// </summary>
    public void Search()
    {
        DataGrid?.ReloadServerData();
    }

    /// <summary>
    /// Delete entity.
    /// </summary>
    public async Task DeleteEntityAsync(object entity, CancellationToken cancellationToken)
    {
        await EntityService.DeleteEntityAsync(entity, entity.GetType(), cancellationToken);
        DataGrid?.ReloadServerData();
    }

    private async void ShowDeleteEntityConfirmationAsync(object source)
    {
        var parameters = new DialogParameters();
        parameters.Add(nameof(ConfirmationDialog.ContentText), "Are you sure you want to delete this record?");
        parameters.Add(nameof(ConfirmationDialog.ButtonText), "Yes");
        parameters.Add(nameof(ConfirmationDialog.Color), Color.Error);

        var result = await (await DialogService.ShowAsync<ConfirmationDialog>("Delete", parameters)).Result;
        if (!result.Canceled)
        {
            try
            {
                await DeleteEntityAsync(source, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to delete record due to error: {ex.Message}", Severity.Error);
                Logger.LogError("Failed to delete record due to error: {ex.Message}", ex.Message);
            }
        }
    }
}
