using Microsoft.AspNetCore.Authorization;

namespace Saritasa.NetForge.Blazor.Infrastructure.Authentication;

/// <summary>
/// Authorization requirement for a custom authentication function.
/// </summary>
public class CustomAuthFunctionRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="customAuthFunction">Custom authentication function for admin panel access.</param>
    public CustomAuthFunctionRequirement(Func<IServiceProvider, Task<bool>> customAuthFunction)
    {
        CustomAuthFunction = customAuthFunction;
    }

    /// <summary>
    /// Custom authentication function for admin panel access.
    /// </summary>
    public Func<IServiceProvider, Task<bool>> CustomAuthFunction { get; }
}
