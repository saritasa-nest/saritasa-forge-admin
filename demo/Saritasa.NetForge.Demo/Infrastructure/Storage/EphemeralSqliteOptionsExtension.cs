using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Saritasa.NetForge.Demo.Infrastructure.DependencyInjection;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

public class EphemeralSqliteOptionsExtension : IDbContextOptionsExtension
{
    private class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        public ExtensionInfo(IDbContextOptionsExtension extension) : base(extension) { }

        public override int GetServiceProviderHashCode() => 0;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => true;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) { }

        public override bool IsDatabaseProvider => true;

        public override string LogFragment => string.Empty;
    }
    
    public void ApplyServices(IServiceCollection services)
    {
        services.AddEphemeralSqlite();
        services.AddScoped<IRelationalConnection, EphemeralSqliteRelationalConnection>();
    }

    public void Validate(IDbContextOptions options)
    {
    }

    public DbContextOptionsExtensionInfo Info => new ExtensionInfo(this);
}