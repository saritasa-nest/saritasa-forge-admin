using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Saritasa.NetForge.Demo.Infrastructure.Extensions;

/// <summary>
/// Extensions class for <see cref="T:Microsoft.EntityFrameworkCore.Infrastructure.IInfrastructure`1"/>.
/// </summary>
internal static class InfrastructureExtensions
{
    /// <summary>
    /// Get instance from an <see cref="T:Microsoft.EntityFrameworkCore.Infrastructure.IInfrastructure`1"/>.
    /// </summary>
    /// <param name="infrastructure">Infrastructure.</param>
    /// <typeparam name="T">Instance type.</typeparam>
    /// <returns>Instance.</returns>
    /// <remarks>
    /// <see cref="Microsoft.EntityFrameworkCore.DbContext"/> uses a service provider different from the one our current scope uses.
    /// This method would help extracting such service provider.
    /// </remarks>
    public static T GetInstance<T>(this IInfrastructure<T> infrastructure) => infrastructure.Instance;
}