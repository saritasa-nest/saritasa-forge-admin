using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Saritasa.NetForge.Domain.Attributes;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Field for navigation property.
/// </summary>
public partial class NavigationField : CustomField
{
    /// <summary>
    /// Property value.
    /// </summary>
    public object? PropertyValue
    {
        get => EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance);
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value);
    }

    private IEnumerable<object> NavigationInstances { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        NavigationInstances = Service
            .GetQuery(Property.ClrType!)
            .OrderBy(instance => instance);
    }

    private static string ConvertNavigationToString(object? navigation)
    {
        if (navigation is null)
        {
            return string.Empty;
        }

        var instanceType = navigation.GetType();
        var attributes = instanceType.GetCustomAttributes(false);
        var attributes2 = instanceType.GetCustomAttributesData();
        var attributes3 = instanceType.GetCustomAttribute(typeof(NetForgeEntityAttribute));
        var propertiesToDisplay = instanceType
            .GetProperties()
            .Where(property => property.GetCustomAttribute(typeof(DisplayAttribute)) is not null);
        List<object> propertyValues = [];
        foreach (var property in propertiesToDisplay)
        {
            var value = property.GetValue(navigation);
            propertyValues.Add(value);
        }

        var navigationInstanceToDisplay = string.Join(';', propertyValues);
        return navigationInstanceToDisplay;
    }
}
