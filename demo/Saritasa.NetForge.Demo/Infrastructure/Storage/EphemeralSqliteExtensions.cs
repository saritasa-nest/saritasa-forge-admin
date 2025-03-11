using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

public static class EphemeralSqliteExtensions
{
    public static DbContextOptionsBuilder UseEphemeralSqlite(this DbContextOptionsBuilder optionsBuilder)
    {
        var extension = new EphemeralSqliteOptionsExtension();
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
        return optionsBuilder.UseSqlite(string.Empty);
    }
}