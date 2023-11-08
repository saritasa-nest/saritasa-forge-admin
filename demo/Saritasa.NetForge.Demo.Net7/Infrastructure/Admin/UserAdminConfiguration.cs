using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.DomainServices.Interfaces;

namespace Saritasa.NetForge.Demo.Net7.Infrastructure.Admin;

/// <summary>
/// <see cref="User"/> admin panel configuration.
/// </summary>
public class UserAdminConfiguration : IEntityAdminConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityOptionsBuilder<User> entityOptionsBuilder)
    {
        entityOptionsBuilder.AddCalculatedProperties(user => user.FullName, user => user.Age)
            .ConfigureProperty(user => user.FullName, builder => builder.SetOrder(1));
    }
}