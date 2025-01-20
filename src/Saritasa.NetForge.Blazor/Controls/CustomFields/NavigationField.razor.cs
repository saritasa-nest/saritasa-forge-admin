using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Field for navigation property.
/// </summary>
public partial class NavigationField : CustomField
{
    [Inject]
    private IEntityService EntityService { get; set; } = null!;

    /// <summary>
    /// Property value.
    /// </summary>
    public object? PropertyValue
    {
        get => EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance);
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value);
    }

    private IEnumerable<object> NavigationInstances { get; set; } = null!;

    private ICollection<PropertyMetadataDto> EntityProperties { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        NavigationInstances = Service
            .GetQuery(Property.ClrType!)
            .OrderBy(instance => instance);

        var entity = await EntityService.GetEntityByTypeAsync(Property.ClrType!, CancellationToken.None);
        EntityProperties = entity.Properties;
    }
}
