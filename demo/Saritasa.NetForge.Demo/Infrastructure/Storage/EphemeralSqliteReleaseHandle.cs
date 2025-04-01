using System.Runtime.InteropServices;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

/// <summary>
/// <see cref="CriticalHandle"/> that automatically remove the database file upon garbage collection.
/// </summary>
internal class EphemeralSqliteReleaseHandle : CriticalHandle
{
    private string? sqliteDbPath;

    /// <summary>
    /// Constructor.
    /// </summary>
    public EphemeralSqliteReleaseHandle(string sqliteDbPath) : base(-1)
    {
        this.sqliteDbPath = sqliteDbPath;
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method would be called when all <see cref="EphemeralSqliteConnectionFactory"/>
    /// and <see cref="EphemeralSqliteConnectionFactory.EphemeralSqliteConnection"/> are garbage collected,
    /// which ensure that nothing can access the database.
    /// </remarks>
    protected override bool ReleaseHandle()
    {
        var path = Interlocked.Exchange(ref sqliteDbPath, null);
        if (path == null)
        {
            // Would only return false for catastrophic failure.
            return true;
        }
        
        File.Delete(path);
        return true;
    }

    /// <inheritdoc />
    /// <remarks>
    /// Reference reads are atomic.
    /// </remarks>
    public override bool IsInvalid => sqliteDbPath == null;
}
