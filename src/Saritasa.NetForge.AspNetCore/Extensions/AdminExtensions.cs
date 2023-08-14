using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Saritasa.NetForge.AspNetCore.Infrastructure;
using Saritasa.NetForge.AspNetCore.Infrastructure.Middlewares;
using Saritasa.NetForge.Domain.Entities;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

namespace Saritasa.NetForge.AspNetCore.Extensions;

/// <summary>
/// Provides extension methods to configure the admin panel.
/// </summary>
public static class AdminExtensions
{
    /// <summary>
    /// Inject the NetForge services.
    /// </summary>
    /// <param name="services">Collection of application services.</param>
    /// <param name="optionsBuilderAction">Action to configure the admin options using the options builder.</param>
    public static void AddNetForge(this IServiceCollection services,
        Action<IAdminOptionsBuilder>? optionsBuilderAction = null)
    {
        // Build the options.
        var adminOptionsBuilder = new AdminOptionsBuilder();
        optionsBuilderAction?.Invoke(adminOptionsBuilder);

        // Inject all required services.
        services.AddSingleton(adminOptionsBuilder.Options);
        adminOptionsBuilder.OrmOptionsBuilder?.ApplyServices(services);

        Infrastructure.DependencyInjection.AutoMapperModule.Register(services);
        Infrastructure.DependencyInjection.MediatRModule.Register(services);
    }

    /// <summary>
    /// Adds the admin panel UI.
    /// </summary>
    /// <param name="app">Web Application instance.</param>
    public static void UseNetForge(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }

        var optionsService = app.Services.GetRequiredService<AdminOptions>();
        var adminPanelEndpoint = optionsService.AdminPanelEndpoint;
        var apiEndpoint = optionsService.ApiEndpoint;

        var appRoutingSetup = new NetForgeRoutingSetup(app);
        appRoutingSetup.SetupRazorViewRoutes(adminPanelEndpoint);
        appRoutingSetup.SetupApiRoutes(apiEndpoint);

        // Make the application use blazor dependencies on a specific URL.
        app.UseWhen(context => context.Request.Path.StartsWithSegments(adminPanelEndpoint), applicationBuilder =>
        {
            applicationBuilder.UsePathBase(adminPanelEndpoint);
            applicationBuilder.UseStaticFiles("/static");
            applicationBuilder.UseBlazorFrameworkFiles();
            applicationBuilder.UseRouting();
        });

        app.UseWhen(context => context.Request.Path.StartsWithSegments(apiEndpoint), applicationBuilder =>
        {
            applicationBuilder.UseMiddleware<ApiExceptionMiddleware>();
        });
    }
}
