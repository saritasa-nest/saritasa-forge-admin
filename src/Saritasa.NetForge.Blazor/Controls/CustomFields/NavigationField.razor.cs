using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Field for navigation property.
/// </summary>
public partial class NavigationField : CustomField
{
    [Inject]
    private IOrmDataService DataService { get; init; } = null!;

    /// <summary>
    /// Property value.
    /// </summary>
    public object? PropertyValue
    {
        get => EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance);
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value);
    }

    private List<object> NavigationInstances { get; set; } = new();

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        NavigationInstances = DataService.GetQuery(Property.ClrType!).ToList();
    }
}
