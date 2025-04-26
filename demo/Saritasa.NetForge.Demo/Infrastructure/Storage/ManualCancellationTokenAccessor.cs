namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

/// <inheritdoc />
/// <remarks>
/// This is a mirror from <see cref="Microsoft.AspNetCore.Http.HttpContextAccessor"/>.
/// <br/>
/// <br/>
/// Simply storing Ephemeral Sqlite path in service scope is not reliable,
/// since an HTTP context can have multiple scopes.
/// <br/>
/// This class is used by <see cref="EphemeralSqliteConnectionFactory"/>
/// to associate a database path to a session.
/// </remarks>
internal class ManualCancellationTokenAccessor : IManualCancellationTokenAccessor
{
    private class TokenHolder
    {
        public CancellationToken? Token { get; set; }
    }

    private static readonly AsyncLocal<TokenHolder> CurrentToken = new();

    /// <inheritdoc />
    public CancellationToken? CancellationToken
    {
        get => CurrentToken.Value?.Token;
        set
        {
            var holder = CurrentToken.Value;
            if (holder != null)
            {
                holder.Token = null;
            }

            if (value != null)
            {
                CurrentToken.Value = new TokenHolder { Token = value };
            }
        }
    }
}