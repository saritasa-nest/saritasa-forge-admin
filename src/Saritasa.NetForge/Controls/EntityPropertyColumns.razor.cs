using System.Collections;
using System.Text;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.Domain.Dtos;
using Saritasa.NetForge.Infrastructure.Helpers;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Domain.UseCases.Interfaces;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.MVVM.Navigation;
using Saritasa.NetForge.MVVM.Utils;
using Saritasa.NetForge.MVVM.ViewModels.EditEntity;

namespace Saritasa.NetForge.Controls;

/// <summary>
/// Columns for entity properties.
/// </summary>
public partial class EntityPropertyColumns : ComponentBase
{
    [Inject]
    private AdminOptions AdminOptions { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ILogger<EntityPropertyColumns> Logger { get; set; } = default!;

    [Inject]
    private IEntityService EntityService { get; set; } = null!;

    [Inject]
    private INavigationService NavigationService { get; set; } = null!;

    [Inject]
    private IOrmDataService DataService { get; set; } = null!;

    /// <summary>
    /// Properties of the entity.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IEnumerable<PropertyMetadataDto> Properties { get; set; } = null!;

    /// <summary>
    /// Data grid that contains these columns. Used for reloading data grid data.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public MudDataGrid<object>? DataGrid { get; set; }

    /// <summary>
    /// Whether the entity is navigation.
    /// </summary>
    /// <remarks>
    /// For example, <c>Shop</c> has <c>Products</c>. And <c>Products</c> have <c>Supplier</c>.
    /// If the entity is <c>Shop</c> then this method will contain <see langword="false"/>.
    /// Otherwise, <see langword="true"/>.
    /// </remarks>
    [Parameter]
    public bool IsNavigationEntity { get; set; }

    /// <summary>
    /// Whether instance of the entity can be deleted.
    /// </summary>
    [Parameter]
    public bool CanDelete { get; set; }

    /// <inheritdoc cref="MessageOptions.EntityDeleteMessage"/>
    [Parameter]
    public string? EntityDeleteMessage { get; set; }

    /// <summary>
    /// Gets property's display name.
    /// </summary>
    /// <param name="property">Property.</param>
    /// <returns>Display name.</returns>
    private string GetPropertyDisplayName(PropertyMetadataDto property)
    {
        return !string.IsNullOrEmpty(property.DisplayName)
            ? property.DisplayName
            : property.Name;
    }

    private static object GetNavigationValue(object navigationInstance, NavigationMetadataDto navigationMetadata)
    {
        var primaryKeys = navigationMetadata.TargetEntityProperties
            .Where(targetProperty => targetProperty.IsPrimaryKey)
            .ToList();

        if (!primaryKeys.Any())
        {
            return navigationInstance;
        }

        if (!navigationMetadata.IsCollection)
        {
            if (primaryKeys.Count == 1)
            {
                return navigationInstance.GetType().GetProperty(primaryKeys[0].Name)!.GetValue(navigationInstance)!;
            }

            return JoinPrimaryKeys(primaryKeys, navigationInstance);
        }

        var primaryKeyValues = new List<object?>();

        foreach (var item in (navigationInstance as IEnumerable)!)
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

    private string FormatValue(object value, PropertyMetadataDto propertyMetadata)
    {
        return DataFormatUtils
            .GetFormattedValue(value, propertyMetadata?.DisplayFormat, propertyMetadata?.FormatProvider);
    }

    private async Task OpenDialogAsync(object navigationInstance, NavigationMetadataDto navigationMetadata)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            FullWidth = true,
            MaxWidth = MaxWidth.ExtraLarge
        };

        var parameters = new DialogParameters
        {
            { nameof(navigationInstance), navigationInstance },
            { nameof(navigationMetadata), navigationMetadata }
        };
        await DialogService.ShowAsync<NavigationDialog>(title: string.Empty, parameters, options);
    }

    /// <summary>
    /// Edit navigation entity by its identifier.
    /// </summary>
    private async Task NavigateToEditing(object navigationInstance, NavigationMetadataDto navigationMetadata)
    {
        var entityMetadata = await EntityService
            .GetEntityByTypeAsync(navigationMetadata.ClrType!, CancellationToken.None);

        var primaryKeyValues = navigationInstance.GetPrimaryKeyValues(navigationMetadata.TargetEntityProperties);
        NavigationService.NavigateTo<EditEntityViewModel>(parameters: [entityMetadata.StringId, primaryKeyValues]);
    }

    private async Task ShowDeleteEntityConfirmationAsync(object source)
    {
        var parameters = new DialogParameters
        {
            { nameof(ConfirmationDialog.ContentText), "Are you sure you want to delete this record?" },
            { nameof(ConfirmationDialog.ButtonText), "Yes" },
            { nameof(ConfirmationDialog.Color), Color.Error }
        };

        var result = await (await DialogService.ShowAsync<ConfirmationDialog>("Delete", parameters)).Result;
        if (!result.Canceled)
        {
            try
            {
                await DataService.DeleteAsync(source, source.GetType(), CancellationToken.None);
                DataGrid?.ReloadServerData();
                ShowEntityDeleteMessage();
            }
            catch (Exception ex)
            {
                var message = ex.InnerException is not null ? ex.InnerException.Message : ex.Message;
                Snackbar.Add($"Failed to delete record due to error: {message}", Severity.Error);
                Logger.LogError(ex, "Failed to delete record due to error: {ErrorMessage}", message);
            }
        }
    }

    private void ShowEntityDeleteMessage()
    {
        string entityDeleteMessage;
        if (!string.IsNullOrEmpty(EntityDeleteMessage))
        {
            entityDeleteMessage = EntityDeleteMessage;
        }
        else if (!string.IsNullOrEmpty(AdminOptions.MessageOptions.EntityDeleteMessage))
        {
            entityDeleteMessage = AdminOptions.MessageOptions.EntityDeleteMessage;
        }
        else
        {
            entityDeleteMessage = "Entity was deleted successfully.";
        }

        Snackbar.Add(entityDeleteMessage, Severity.Success);
    }

    private IEnumerable<ListViewPropertyDto> GetListViewProperties()
    {
        List<ListViewPropertyDto> listViewProperties = [];

        var includedProperties = Properties
            .Where(property => property is { IsHidden: false, IsHiddenFromListView: false });
        foreach (var property in includedProperties)
        {
            if (property is NavigationMetadataDto navigationMetadata)
            {
                var propertyPath = new StringBuilder(navigationMetadata.Name);

                HandleNavigation(navigationMetadata);

                void HandleNavigation(NavigationMetadataDto navigation)
                {
                    if (navigation.IsCollection)
                    {
                        var listViewProperty = new ListViewPropertyDto
                        {
                            Property = navigation,
                            PropertyPath = propertyPath.ToString(),
                            Navigation = navigation
                        };

                        listViewProperties.Add(listViewProperty);
                        return;
                    }

                    var targetProperties = navigation.TargetEntityProperties
                        .Where(targetProperty => targetProperty is { IsHidden: false, IsHiddenFromListView: false });
                    foreach (var targetProperty in targetProperties)
                    {
                        var listViewProperty = new ListViewPropertyDto
                        {
                            Property = targetProperty,
                            PropertyPath = $"{propertyPath}.{targetProperty.Name}",
                            Navigation = navigation
                        };

                        listViewProperties.Add(listViewProperty);
                    }

                    var targetNavigations = navigation.TargetEntityNavigations
                        .Where(targetNavigation => targetNavigation is { IsHidden: false, IsHiddenFromListView: false });
                    foreach (var targetNavigation in targetNavigations)
                    {
                        var navigationString = $".{targetNavigation.Name}";
                        propertyPath = propertyPath.Append(navigationString);
                        HandleNavigation(targetNavigation);
                        propertyPath = propertyPath.Replace(navigationString, string.Empty);
                    }
                }
            }
            else
            {
                var listViewProperty = new ListViewPropertyDto
                {
                    Property = property,
                    PropertyPath = property.Name
                };

                listViewProperties.Add(listViewProperty);
            }
        }

        // Display principal entity primary key at the start of columns if the order is not set.
        return listViewProperties
            .OrderByDescending(property => property is { Property: { IsPrimaryKey: true, Order: null } }
                                           && !property.PropertyPath.Contains('.')) // Means property is not part of a navigation
            .ThenByDescending(property => property.Property.Order.HasValue)
            .ThenBy(property => property.Property.Order);
    }

    /// <summary>
    /// Removes the last property part from the given path.
    /// </summary>
    /// <param name="propertyPath">Path to access the property. For example: <c>Shop.Address.Street</c>.</param>
    /// <returns>Path without the property part. For example: <c>Shop.Address.Street</c> -> <c>Shop.Address</c>.</returns>
    private static string RemoveLastPropertyFromPath(string propertyPath)
    {
        if (string.IsNullOrEmpty(propertyPath))
        {
            return propertyPath;
        }

        var lastPropertySeparator = propertyPath.LastIndexOf('.');
        return lastPropertySeparator >= 0 ? propertyPath[..lastPropertySeparator] : propertyPath;
    }
}
