using System.Collections;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.DomainServices.Extensions;
using Saritasa.NetForge.Mvvm.Utils;
using Saritasa.NetForge.UseCases.Constants;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls;

/// <summary>
/// Columns for entity properties.
/// </summary>
public partial class EntityPropertyColumns : ComponentBase
{
    [Inject]
    private AdminOptions AdminOptions { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Properties of the entity.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IEnumerable<PropertyMetadataDto> Properties { get; set; } = null!;

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

    /// <summary>
    /// Gets property value via <c>Reflection</c>.
    /// </summary>
    /// <param name="source">Source object.</param>
    /// <param name="property">Property metadata.</param>
    /// <returns>Property value.</returns>
    private object GetPropertyValue(object? source, PropertyMetadataDto property)
    {
        var propertyInfo = source?.GetType().GetProperty(property.Name);
        var value = propertyInfo?.GetValue(source);

        if (value is null || value.ToString() == string.Empty)
        {
            return !string.IsNullOrEmpty(property.EmptyValueDisplay)
                ? property.EmptyValueDisplay
                : DefaultValueConstants.DefaultEmptyPropertyValueDisplay;
        }

        if (property is NavigationMetadataDto navigation)
        {
            value = GetNavigationValue(value, navigation);
        }

        value = FormatValue(value, property.Name);

        if (property.ClrType == typeof(string))
        {
            var stringValue = value.ToString();

            var maxCharacters = property.TruncationMaxCharacters ?? AdminOptions.TruncationMaxCharacters;

            if (maxCharacters != default)
            {
                value = stringValue!.Truncate(maxCharacters);
            }
        }

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
        var propertyMetadata = Properties.FirstOrDefault(property => property.Name == propertyName);
        return DataFormatUtils.GetFormattedValue(value, propertyMetadata?.DisplayFormat,
            propertyMetadata?.FormatProvider);
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
}
