using System.Collections;
using AutoMapper;
using MudBlazor;
using Saritasa.NetForge.Mvvm.Utils;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

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
        var orderBy = gridState.SortDefinitions
            .Select(sort => new OrderByDto
            {
                FieldName =
                    DataGrid!.RenderedColumns.First(column => column.PropertyName.Equals(sort.SortBy)).Title,
                IsDescending = sort.Descending
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
    public string GetPropertyDisplayName(PropertyMetadataDto property)
    {
        return !string.IsNullOrEmpty(property.DisplayName)
            ? property.DisplayName
            : property.Name;
    }

    /// <summary>
    /// Gets property value via <c>Reflection</c>.
    /// </summary>
    /// <param name="source">Source object.</param>
    /// <param name="property">Property metadata.</param>
    /// <returns>Property value.</returns>
    public object? GetPropertyValue(object source, PropertyMetadataDto property)
    {
        var propertyInfo = source.GetType().GetProperty(property.Name);
        var value = propertyInfo?.GetValue(source);

        if (value is null)
        {
            return value;
        }

        if (property.IsNavigation)
        {
            var primaryKey = property.TargetEntityProperties
                .FirstOrDefault(targetProperty => targetProperty.IsPrimaryKey);

            if (primaryKey is not null)
            {
                if (!property.IsNavigationCollection)
                {
                    value = value.GetType().GetProperty(primaryKey.Name)!.GetValue(value);
                }
                else
                {
                    var primaryKeys = new List<object?>();

                    foreach (var item in (value as IEnumerable)!)
                    {
                        primaryKeys.Add(item.GetType().GetProperty(primaryKey.Name)!.GetValue(item));
                    }

                    value = $"[{string.Join(", ", primaryKeys)}]";
                }
            }
        }

        value = FormatValue(value!, property.Name);

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
