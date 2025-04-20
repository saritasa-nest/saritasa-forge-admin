using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Domain.UseCases.Interfaces;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;
using Saritasa.NetForge.Infrastructure.EfCore.Services;
using Saritasa.NetForge.Infrastructure.Helpers;

namespace Saritasa.NetForge.Controls.CustomFields;

/// <summary>
/// Field for a navigation collection.
/// </summary>
/// <typeparam name="T">Underlying type of the collection.</typeparam>
public partial class NavigationCollectionField<T> : CustomField
{
    [Inject]
    private IEntityService EntityService { get; set; } = null!;

    /// <summary>
    /// Navigation collection.
    /// </summary>
    public IEnumerable<T> PropertyValue
    {
        get => (IEnumerable<T>)EntityTracker.GetPropertyValue(Property.Name)!;
        set => EntityTracker.SetPropertyValue(Property.Name, value.ToList());
    }

    private IEnumerable<T> NavigationInstances { get; set; } = null!;

    private GetEntityDto EntityMetadata { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var entityType = Property.ClrType!.GetGenericArguments().First();

        NavigationInstances = Service
            .GetQuery(entityType)
            .Cast<T>()
            .OrderBy(instance => instance).ToList();

        EntityMetadata = await EntityService.GetEntityByTypeAsync(entityType, CancellationToken.None);
    }
}
