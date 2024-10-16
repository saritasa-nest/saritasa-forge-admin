using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls;

/// <summary>
/// Navigation entity details component.
/// </summary>
public partial class NavigationEntityDetails : ComponentBase
{
    [Inject]
    private IEntityService EntityService { get; set; } = null!;

    /// <summary>
    /// Navigation metadata.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public NavigationMetadataDto NavigationMetadata { get; set; } = null!;

    /// <summary>
    /// Properties of the entity.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IEnumerable<PropertyMetadataDto> Properties { get; set; } = null!;

    /// <summary>
    /// Entity instance.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public object? EntityInstance { get; set; }

    //private async IEnumerable<PropertyMetadataDto> GetProperties()
    //{
    //    var entityMetadata = await EntityService.GetEntityByTypeAsync(Property.ClrType!, CancellationToken.None);
    //    //entityMetadata.PluralName
    //}

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        var entityMetadata = await EntityService.GetEntityByTypeAsync(NavigationMetadata.ClrType!, CancellationToken.None);
        Properties = entityMetadata.Properties;
    }
}
