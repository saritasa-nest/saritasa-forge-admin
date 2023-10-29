using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.UseCases.Metadata.SearchDataForEntity;

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

    private readonly IMediator mediator;
    private readonly IMapper mapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    public EntityDetailsViewModel(Guid id, IMediator mediator, IMapper mapper)
    {
        Model = new EntityDetailsModel { Id = id };

        this.mediator = mediator;
        this.mapper = mapper;
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
        var entity = await mediator.Send(new GetEntityByIdQuery(Model.Id), cancellationToken);

        Model = mapper.Map<EntityDetailsModel>(entity);
    }

    /// <summary>
    /// Loads entity's data to data grid.
    /// </summary>
    /// <param name="gridState">Information about grid state. For example, current page, page size.</param>
    /// <returns>Grid data collection populated with entity's data.</returns>
    public async Task<GridData<object>> LoadEntityGridDataAsync(GridState<object> gridState)
    {
        var searchOptions = new SearchOptions
        {
            Page = gridState.Page + 1,
            PageSize = gridState.PageSize,
            SearchString = SearchString
        };

        var entityData = await mediator.Send(new SearchDataForEntityQuery(Model.ClrType, Model.Properties, searchOptions));

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
        return source.GetType().GetProperty(propertyName)?.GetValue(source);
    }

    /// <summary>
    /// Searches data by search string.
    /// </summary>
    public void Search()
    {
        DataGrid?.ReloadServerData();
    }
}
