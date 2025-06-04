namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

/// <summary>
/// Defines an ephemeral database session context.
/// </summary>
/// <param name="SessionId">Session ID.</param>
/// <param name="CancellationToken">Manual cancellation token.</param>
public record SessionContext(Guid SessionId, CancellationToken CancellationToken)
{
    /// <summary>
    /// HTTP context key.
    /// </summary>
    public const string HttpContextKey = nameof(SessionContext);

    /// <summary>
    /// Constructor.
    /// </summary>
    public SessionContext(CancellationToken cancellationToken) : this(Guid.NewGuid(), cancellationToken)
    {
    }
}
