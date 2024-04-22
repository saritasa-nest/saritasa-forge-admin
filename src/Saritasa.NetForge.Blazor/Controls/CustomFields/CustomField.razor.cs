using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Mvvm.ViewModels;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

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
    /// Error model to show error for component.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public ComponentErrorModel ComponentErrorModel { get; set; } = null!;
}
