using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Domain.UseCases.Interfaces;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.Infrastructure.Helpers;

namespace Saritasa.NetForge.Controls.CustomFields;

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
        get => EntityTracker.GetPropertyValue(Property.Name);
        set => EntityTracker.SetPropertyValue(Property.Name, value);
    }

    private IEnumerable<object> NavigationInstances { get; set; } = null!;

    private GetEntityDto EntityMetadata { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        NavigationInstances = Service
            .GetQuery(Property.ClrType!)
            .OrderBy(instance => instance);

        EntityMetadata = await EntityService.GetEntityByTypeAsync(Property.ClrType!, CancellationToken.None);
    }
}
