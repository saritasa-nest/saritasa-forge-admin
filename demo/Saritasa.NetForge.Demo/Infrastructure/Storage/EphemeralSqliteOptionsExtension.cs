using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Saritasa.NetForge.Demo.Infrastructure.DependencyInjection;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

/// <summary>
/// Ephemeral SQLite options extension.
/// </summary>
internal class EphemeralSqliteOptionsExtension : IDbContextOptionsExtension
{
    /// <inheritdoc />
    private class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        /// <inheritdoc />
        public ExtensionInfo(IDbContextOptionsExtension extension) : base(extension) { }

        /// <inheritdoc />
        public override int GetServiceProviderHashCode() => 0;

        /// <inheritdoc />
        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => true;

        /// <inheritdoc />
        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) { }

        /// <inheritdoc />
        public override bool IsDatabaseProvider => true;

        /// <inheritdoc />
        public override string LogFragment => string.Empty;
    }
    
    /// <inheritdoc />
    public void ApplyServices(IServiceCollection services)
    {
        services.AddEphemeralSqlite();
        services.AddScoped<IRelationalConnection, EphemeralSqliteRelationalConnection>();
    }

    /// <inheritdoc />
    public void Validate(IDbContextOptions options)
    {
    }

    /// <inheritdoc />
    public DbContextOptionsExtensionInfo Info => new ExtensionInfo(this);
}