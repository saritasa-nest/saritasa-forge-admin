namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

/// <summary>
/// Accessor for manual cancellation token.
/// </summary>
public interface IManualCancellationTokenAccessor
{
    /// <summary>
    /// Gets the current perform context.
    /// </summary>
    CancellationToken? CancellationToken { get; set; }
}
