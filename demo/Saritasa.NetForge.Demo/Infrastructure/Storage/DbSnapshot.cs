namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

/// <summary>
/// Stores the database snapshot's location.
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
            logger.LogInformation("Snapshot location set: {Location}", value);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (string.IsNullOrEmpty(snapShotLocation))
        {
            return;
        }

        File.Delete(snapShotLocation);
        logger.LogInformation("Deleted database snapshot: {Location}", snapShotLocation);
    }
}