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

        var propertiesToDisplay = navigation
            .GetType()
            .GetProperties()
            .Select(property =>
            {
                var propertyAttribute = property.GetCustomAttribute<NetForgePropertyAttribute>();
                return (property, propertyAttribute);
            })
            .Where(property => property.propertyAttribute is not null && property.propertyAttribute.UseToDisplayNavigation);
        List<object> propertyValues = [];
        foreach (var property in propertiesToDisplay)
        {
            var value = property.property.GetValue(navigation);
            propertyValues.Add(value ?? property.propertyAttribute!.EmptyValueDisplay);
        }

        return string.Join("; ", propertyValues);
    }
}
