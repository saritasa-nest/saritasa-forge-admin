namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

/// <summary>
/// Stores the migrated and seeded database snapshot's location.
/// Automatically delete it after the application is closed.
/// </summary>
internal class DbSnapshot : IDisposable
{
    private readonly ILogger<DbSnapshot> logger;
    private string? snapShotLocation;

    /// <summary>
    /// Constructor.
    /// </summary>
    public DbSnapshot(ILogger<DbSnapshot> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Location of the database snapshot.
    /// </summary>
    public string? SnapshotLocation
    {
        get => snapShotLocation;
        set
        {
            snapShotLocation = value;
            logger.LogDebug("Snapshot location set: {Location}", value);
        }
    }

    /// <inheritdoc cref="Dispose()" />
    /// <param name="disposing">Whether this method is called from <see cref="Dispose()"/> or not.</param>
    protected virtual void Dispose(bool disposing)
    {
        logger.LogDebug("Cleaning unreleased handles.");
        GC.Collect(2);
        
        var snapshotLocationLocal = Interlocked.Exchange(ref snapShotLocation, null);
        if (string.IsNullOrEmpty(snapshotLocationLocal))
        {
            return;
        }

        File.Delete(snapshotLocationLocal);
        logger.LogDebug("Deleted database snapshot: {Location}", snapshotLocationLocal);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    ~DbSnapshot()
    {
        Dispose(false);
    }
}