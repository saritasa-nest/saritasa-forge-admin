using Extensions.Hosting.AsyncInitialization;

namespace Saritasa.NetForge.Demo.Infrastructure.Startup;

/// <summary>
/// GC Trigger that guarantee database handles are periodically released.
/// </summary>
internal sealed class GCTrigger : IAsyncInitializer
{
    private const int AwaitTimeMinutes = 15;
    
    private readonly IHostApplicationLifetime hostApplicationLifetime;

    /// <summary>
    /// Constructor.
    /// </summary>
    public GCTrigger(IHostApplicationLifetime hostApplicationLifetime)
    {
        this.hostApplicationLifetime = hostApplicationLifetime;
    }

    /// <inheritdoc />
    public Task InitializeAsync(CancellationToken ct)
    {
        _ = Task.Run(async () =>
        {
            var waitSpan = TimeSpan.FromMinutes(AwaitTimeMinutes);
            var cancellationToken = hostApplicationLifetime.ApplicationStopping;
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(waitSpan, cancellationToken);
                    GC.Collect(2, GCCollectionMode.Default, false);
                }
            }
            catch (OperationCanceledException)
            {
            }
        });

        return Task.CompletedTask;
    }
}