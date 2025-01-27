using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Domain.UseCases.Interfaces;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;

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
        get => (IEnumerable<T>)EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance)!;
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value.ToList());
    }

    private IEnumerable<T> NavigationInstances { get; set; } = null!;

    private ICollection<PropertyMetadataDto> EntityProperties { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var entityType = Property.ClrType!.GetGenericArguments().First();

        NavigationInstances = Service
            .GetQuery(entityType)
            .Cast<T>()
            .OrderBy(instance => instance);

        var entity = await EntityService.GetEntityByTypeAsync(entityType, CancellationToken.None);
        EntityProperties = entity.Properties;
    }
}
