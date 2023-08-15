using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Demo.NetCore7.Models;

namespace Saritasa.NetForge.Demo.NetCore7.DbContexts;

/// <summary>
/// Represents the database context for identity.
/// </summary>
public class AppIdentityDbContext : IdentityDbContext<User>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppIdentityDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
    {
    }
}
