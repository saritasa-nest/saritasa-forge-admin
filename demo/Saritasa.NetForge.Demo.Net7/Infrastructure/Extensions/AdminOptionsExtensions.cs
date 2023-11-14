using Microsoft.AspNetCore.Identity;
using Saritasa.NetForge.Demo.Net7.Infrastructure.Admin;
using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.DomainServices;

namespace Saritasa.NetForge.Demo.Net7.Infrastructure.Extensions;

/// <summary>
/// Extension class for <see cref="AdminOptionsBuilder"/>.
/// </summary>
public static class AdminOptionsExtensions
{
    /// <summary>
    /// Add entities into identity group.
    /// </summary>
    /// <param name="optionsBuilder">Admin options builder.</param>
    public static void AddIdentityGroup(this AdminOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureEntity<IdentityRole>(entityOptionsBuilder =>
        {
            entityOptionsBuilder.SetGroup("Identity");
        }).ConfigureEntity<User>(entityOptionsBuilder =>
        {
            entityOptionsBuilder.SetGroup("Identity");
        }).ConfigureEntity<IdentityRoleClaim<string>>(entityOptionsBuilder =>
        {
            entityOptionsBuilder.SetGroup("Identity");
        }).ConfigureEntity<IdentityUserClaim<string>>(entityOptionsBuilder =>
        {
            entityOptionsBuilder.SetGroup("Identity");
        }).ConfigureEntity<IdentityUserLogin<string>>(entityOptionsBuilder =>
        {
            entityOptionsBuilder.SetGroup("Identity");
        }).ConfigureEntity<IdentityUserRole<string>>(entityOptionsBuilder =>
        {
            entityOptionsBuilder.SetGroup("Identity");
        }).ConfigureEntity<IdentityUserToken<string>>(entityOptionsBuilder =>
        {
            entityOptionsBuilder.SetGroup("Identity");
        }).ConfigureEntity(new UserAdminConfiguration());
    }
}