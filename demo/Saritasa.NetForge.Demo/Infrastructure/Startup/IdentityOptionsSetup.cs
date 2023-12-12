using Microsoft.AspNetCore.Identity;

namespace Saritasa.NetForge.Demo.Infrastructure.Startup;

/// <summary>
/// Identity options setup.
/// </summary>
public class IdentityOptionsSetup
{
    /// <summary>
    /// Setup identity.
    /// </summary>
    /// <param name="options">The options.</param>
    public void Setup(IdentityOptions options)
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireNonAlphanumeric = false;
    }
}