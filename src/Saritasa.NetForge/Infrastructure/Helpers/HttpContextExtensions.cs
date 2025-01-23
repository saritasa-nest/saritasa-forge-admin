using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Saritasa.NetForge.Infrastructure.Helpers;

/// <summary>
/// Provides extension methods for the HttpContext class.
/// </summary>
public static class HttpContextExtensions
{
    private static readonly ActionDescriptor EmptyActionDescriptor = new();

    /// <summary>
    /// Writes the result to the response.
    /// </summary>
    /// <param name="context">Http context instance.</param>
    /// <param name="result">Action result instance.</param>
    public static Task WriteResultAsync<TResult>(this HttpContext context, TResult result)
        where TResult : IActionResult
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var executor = context.RequestServices.GetService<IActionResultExecutor<TResult>>();

        if (executor == null)
        {
            throw new InvalidOperationException($"No result executor for '{typeof(TResult).FullName}' has been registered.");
        }

        var routeData = context.GetRouteData();
        var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);
        return executor.ExecuteAsync(actionContext, result);
    }
}
