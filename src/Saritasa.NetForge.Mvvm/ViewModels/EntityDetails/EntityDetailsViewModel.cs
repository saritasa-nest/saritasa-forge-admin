using System.Text;
using AutoMapper;
using MudBlazor;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Mvvm.Utils;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Interfaces;

namespace Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;

/// <summary>
/// ViewModel representing details of an entity.
/// </summary>
public class EntityDetailsViewModel : BaseViewModel
{
    /// <summary>
    /// Entity details model.
    /// </summary>
    public EntityDetailsModel Model { get; private set; }

    private readonly IEntityService entityService;
    private readonly IMapper mapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    public EntityDetailsViewModel(string stringId, IMapper mapper, IEntityService entityService)
    {
        Model = new EntityDetailsModel { StringId = stringId };

        this.mapper = mapper;
        this.entityService = entityService;
    }

    /// <summary>
    /// Search string.
    /// </summary>
    public string? SearchString { get; set; }

    /// <summary>
    /// Data grid reference.
    /// </summary>
    public MudDataGrid<object>? DataGrid { get; set; }

    /// <inheritdoc/>
    public override async Task LoadAsync(CancellationToken cancellationToken)
    {
        var entity = await entityService.GetEntityByIdAsync(Model.StringId, cancellationToken);

        Model = mapper.Map<EntityDetailsModel>(entity);
    }

    /// <summary>
    /// Loads entity's data to data grid.
    /// </summary>
    /// <param name="gridState">Information about grid state. For example, current page, page size.</param>
    /// <returns>Grid data collection populated with entity's data.</returns>
    public async Task<GridData<object>> LoadEntityGridDataAsync(GridState<object> gridState)
    {
        var sortDefinitions = gridState.SortDefinitions
            .Select(sort => new
            {
                DataGrid!.RenderedColumns.First(column => column.PropertyName.Equals(sort.SortBy)).Title,
                sort.Descending,
                sort.Index
            })
            .ToList();

        var orderBy = new StringBuilder();
        foreach (var sort in sortDefinitions)
        {
            orderBy.Append($"{sort.Title}:{(sort.Descending ? "desc" : "asc")}");

            if (sortDefinitions.IndexOf(sort) + 1 != sortDefinitions.Count)
            {
                orderBy.Append(',');
            }
        }

        var searchOptions = new SearchOptions
        {
            Page = gridState.Page + 1,
            PageSize = gridState.PageSize,
            SearchString = SearchString,
            OrderBy = orderBy.ToString()
        };

        var entityData = await entityService
            .SearchDataForEntityAsync(Model.ClrType, Model.Properties, searchOptions, Model.SearchFunction);

        var data = new GridData<object>
        {
            Items = entityData.Items,
            TotalItems = entityData.Metadata.TotalCount
        };

        return data;
    }

    /// <summary>
    /// Gets property's display name.
    /// </summary>
    /// <param name="property">Property.</param>
    /// <returns>Display name.</returns>
    public string GetPropertyDisplayName(PropertyMetadata property)
    {
        return !string.IsNullOrEmpty(property.DisplayName)
            ? property.DisplayName
            : property.Name;
    }

    /// <summary>
    /// Gets property value via <c>Reflection</c>.
    /// </summary>
    /// <param name="source">Source object.</param>
    /// <param name="propertyName">Property name.</param>
    /// <returns>Property value.</returns>
    public object? GetPropertyValue(object source, string propertyName)
    {
        var propertyInfo = source.GetType().GetProperty(propertyName);
        var value = propertyInfo?.GetValue(source);

        if (value != null)
        {
            value = FormatValue(value, propertyName);
        }

        return value;
    }

    private string FormatValue(object value, string propertyName)
    {
        var propertyMetadata = Model.Properties.FirstOrDefault(property => property.Name == propertyName);
        return DataFormatUtils.GetFormattedValue(value, propertyMetadata?.DisplayFormat,
            propertyMetadata?.FormatProvider);
    }

    /// <summary>
    /// Searches data by search string.
    /// </summary>
    public void Search()
    {
        DataGrid?.ReloadServerData();
    }
}
