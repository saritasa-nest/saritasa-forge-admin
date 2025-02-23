﻿using Saritasa.NetForge.Domain;
using Saritasa.NetForge.Domain.Interfaces;
using Saritasa.NetForge.Demo.Models;

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

        entityOptionsBuilder.ConfigureSearch((_, query, searchTerm) =>
        {
            return query.Where(e => e.Email.Contains(searchTerm) && e.FullName.Contains(searchTerm));
        });
    }
}