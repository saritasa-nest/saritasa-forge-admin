using Saritasa.NetForge.Demo.Models;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.DomainServices.Interfaces;

namespace Saritasa.NetForge.Demo.Infrastructure.Admin;

/// <summary>
/// <see cref="ContactInfo"/> admin panel configuration.
/// </summary>
public class ContactInfoAdminConfiguration  : IEntityAdminConfiguration<ContactInfo>
{
    /// <inheritdoc />
    public void Configure(EntityOptionsBuilder<ContactInfo> entityOptionsBuilder)
    {
        entityOptionsBuilder.SetCanAdd(false);
        entityOptionsBuilder.SetCanEdit(false);
        entityOptionsBuilder.SetCanDelete(false);
    }
}