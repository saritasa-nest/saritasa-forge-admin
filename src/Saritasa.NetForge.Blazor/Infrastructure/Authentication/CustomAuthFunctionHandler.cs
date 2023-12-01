using Microsoft.AspNetCore.Authorization;

namespace Saritasa.NetForge.Blazor.Infrastructure.Authentication;

/// <summary>
/// Authorization handler for a custom authentication function.
/// </summary>
public class CustomAuthFunctionHandler : AuthorizationHandler<CustomAuthFunctionRequirement>
{
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public CustomAuthFunctionHandler(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        CustomAuthFunctionRequirement requirement)
    {
        if (await requirement.CustomAuthFunction.Invoke(serviceProvider))
        {
            context.Succeed(requirement);
        }
    }
}
