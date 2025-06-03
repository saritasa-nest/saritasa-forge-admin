using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.Extensions;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.MVVM.ViewModels;

namespace Saritasa.NetForge.Controls.CustomFields;

/// <summary>
/// Base class for custom fields.
/// </summary>
/// <remarks>
/// We use <see cref="OwningComponentBase{IOrmDataService}"/> here
/// because we want to provide isolated version of <see cref="IOrmDataService"/> to the component.
/// Use case: We want to load navigation data to some field. But entity has more than one navigation,
/// without <see cref="OwningComponentBase{IOrmDataService}"/> loading data from one database context
/// will be executed at the same time and will cause exception.
/// Inheriting this class provides to every component different <see cref="IOrmDataService"/>
/// with different database contexts.
/// </remarks>
public abstract class CustomField : OwningComponentBase<IOrmDataService>
{
    /// <summary>
    /// Property name.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public PropertyMetadataDto Property { get; init; } = null!;

    /// <summary>
    /// Entity instance that contains property value for this field.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public object EntityInstance { get; init; } = null!;

    /// <summary>
    /// Is field with read only access.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool IsReadOnly { get; init; }

    /// <summary>
    /// Error model to display the field error.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public FieldErrorModel FieldErrorModel { get; set; } = null!;

    /// <summary>
    /// Gets <see cref="PropertyMetadata.DisplayName"/> from <see cref="Property"/>.
    /// If property does not have display name then <see cref="PropertyMetadata.Name"/> will be used.
    /// </summary>
    protected string GetDisplayName()
    {
        return Property.DisplayName != string.Empty ? Property.DisplayName : Property.Name.ToMeaningfulName();
    }
}
