using System.Reflection;
using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Blazor.Infrastructure.Helpers;
using Saritasa.NetForge.Blazor.Pages;
using Saritasa.NetForge.Mvvm.ViewModels;

namespace Saritasa.NetForge.Blazor.Infrastructure.Navigation;

/// <summary>
/// Navigation service implementing page navigation.
/// </summary>
public class NavigationService : INavigationService
{
    private readonly NavigationManager navigationManager;
    private readonly IDictionary<Type, Type> viewModelsPages;

    /// <summary>
    /// Constructor.
    /// </summary>
    public NavigationService(NavigationManager navigationManager)
    {
        this.navigationManager = navigationManager;
        viewModelsPages = GetPages();
    }

    /// <inheritdoc />
    public void NavigateTo<TViewModel>(bool forceLoad = false, bool replace = false, params object[] parameters)
        where TViewModel : BaseViewModel
    {
        var viewModelType = typeof(TViewModel);
        NavigateTo(viewModelType, forceLoad, replace, parameters);
    }

    private static Dictionary<Type, Type> GetPages()
    {
        var assembly = Assembly.GetAssembly(typeof(App))!;
        var openGenericType = typeof(MvvmComponentBase<>);

        var viewModelPairs = from type in assembly.GetTypes()
                             where !type.IsAbstract
                                && type.GetCustomAttributes<RouteAttribute>().Any()
                                && type.BaseType is not null
                                && type.BaseType.IsGenericType
                                && type.BaseType.GetGenericTypeDefinition() == openGenericType
                                && type.BaseType.GenericTypeArguments.Length == 1
                             let pageType = type.BaseType
                             let viewModelType = pageType.GenericTypeArguments.Single()
                             select KeyValuePair.Create(viewModelType, pageType);

        return new Dictionary<Type, Type>(viewModelPairs);
    }

    private void NavigateTo(Type viewModelType, bool forceLoad = false, bool replace = false,
        params object[] parameters)
    {
        if (!viewModelsPages.TryGetValue(viewModelType, out var pageType))
        {
            throw new InvalidOperationException("There are no registered pages for view model.");
        }

        var routePath = RouteHelper.GetRoute(pageType, parameters);
        navigationManager.NavigateTo(routePath, forceLoad, replace);
    }
}
