using System.Collections;
using AutoMapper;
using MudBlazor;
using Saritasa.NetForge.Mvvm.Utils;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;
using Saritasa.Tools.Domain.Exceptions;

namespace Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;

/// <summary>
/// ViewModel representing details of an entity.
/// </summary>
public class EntityDetailsViewModel : BaseViewModel
{
    private const string DefaultEmptyValueDisplay = "-";

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

    /// <summary>
    /// Whether the entity exists.
    /// </summary>
    public bool IsEntityExists { get; private set; } = true;

    /// <inheritdoc/>
    public override async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            var entity = await entityService.GetEntityByIdAsync(Model.StringId, cancellationToken);
            Model = mapper.Map<EntityDetailsModel>(entity);
        }
        catch (NotFoundException)
        {
            IsEntityExists = false;
        }
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

        var entityData = await entityService
            .SearchDataForEntityAsync(Model.ClrType, Model.Properties, searchOptions, Model.SearchFunction, Model.CustomQueryFunction);

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
    public object GetPropertyValue(object? source, PropertyMetadataDto property)
    {
        var propertyInfo = source?.GetType().GetProperty(property.Name);
        var value = propertyInfo?.GetValue(source);

        if (value is null || value.ToString() == string.Empty)
        {
            return !string.IsNullOrEmpty(property.EmptyValueDisplay)
                ? property.EmptyValueDisplay
                : DefaultEmptyValueDisplay;
        }

        if (property is NavigationMetadataDto navigation)
        {
            value = GetNavigationValue(value, navigation);
        }

        value = FormatValue(value, property.Name);

        return value;
    }

    private static object GetNavigationValue(object navigation, NavigationMetadataDto navigationMetadata)
    {
        var primaryKeys = navigationMetadata.TargetEntityProperties
            .Where(targetProperty => targetProperty.IsPrimaryKey)
            .ToList();

        if (!primaryKeys.Any())
        {
            return navigation;
        }

        if (!navigationMetadata.IsCollection)
        {
            if (primaryKeys.Count == 1)
            {
                return navigation.GetType().GetProperty(primaryKeys[0].Name)!.GetValue(navigation)!;
            }

            return JoinPrimaryKeys(primaryKeys, navigation);
        }

        var primaryKeyValues = new List<object?>();

        foreach (var item in (navigation as IEnumerable)!)
        {
            if (primaryKeys.Count == 1)
            {
                primaryKeyValues.Add(item.GetType().GetProperty(primaryKeys[0].Name)!.GetValue(item));
            }
            else
            {
                primaryKeyValues.Add($"{{ {JoinPrimaryKeys(primaryKeys, item)} }}");
            }
        }

        return $"[ {string.Join(", ", primaryKeyValues)} ]";
    }

    private static string JoinPrimaryKeys(IEnumerable<PropertyMetadataDto> primaryKeys, object navigation)
    {
        var primaryKeyValues = primaryKeys
            .Select(primaryKey => primaryKey.Name)
            .Select(primaryKeyName => navigation.GetType().GetProperty(primaryKeyName)!.GetValue(navigation));

        return string.Join("; ", primaryKeyValues);
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
