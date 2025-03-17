namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

/// <summary>
/// Stores the database snapshot's location.
/// Automatically delete it after the application is closed.
/// </summary>
internal class DbSnapshot : IDisposable
{
    /// <summary>
    /// Location of the database snapshot.
    /// </summary>
    public string? SnapshotLocation { get; set; }

    /// <inheritdoc />
    public void Dispose()
    {
        if (!string.IsNullOrEmpty(SnapshotLocation))
        {
            File.Delete(SnapshotLocation);
        }
    }
}