using Microsoft.AspNetCore.Authorization;
using Saritasa.NetForge.Blazor.Infrastructure.Authentication;
using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.Blazor.Infrastructure.DependencyInjection.Startup;

/// <summary>
/// Authorization options setup for admin panel.
/// </summary>
public class AuthorizationOptionsSetup
{
    private readonly AdminOptions adminOptions;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="adminOptions">Admin options.</param>
    public AuthorizationOptionsSetup(AdminOptions adminOptions)
    {
        this.adminOptions = adminOptions;
    }

    /// <summary>
    /// Setup authorization.
    /// </summary>
    /// <param name="options">Authorization options.</param>
    public void Setup(AuthorizationOptions options)
    {
        if (adminOptions.AdminPanelAccessRoles.Any() || adminOptions.CustomAuthFunction is not null)
        {
            options.AddPolicy("AdminPanelAccess", policy =>
            {
                if (adminOptions.AdminPanelAccessRoles.Any())
                {
                    policy.RequireRole(adminOptions.AdminPanelAccessRoles);
                }

                if (adminOptions.CustomAuthFunction != null)
                {
                    policy.AddRequirements(new CustomAuthFunctionRequirement(adminOptions.CustomAuthFunction));
                }
            });
        }
    }
}
