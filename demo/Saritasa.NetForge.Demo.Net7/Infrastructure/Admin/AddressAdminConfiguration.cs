using Microsoft.Extensions.Options;
using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.DomainServices.Interfaces;

namespace Saritasa.NetForge.Demo.Net7.Infrastructure.Admin;

/// <summary>
/// <see cref="Address"/> admin panel configuration.
/// </summary>
public class AddressAdminConfiguration : IEntityAdminConfiguration<Address>
{
    /// <inheritdoc />
    public void Configure(EntityOptionsBuilder<Address> entityOptionsBuilder)
    {
        entityOptionsBuilder.ConfigureProperty(address => address.Id, propertyBuilder =>
        {
            propertyBuilder
                .SetDescription("Item identifier.")
                .SetOrder(2);
        }).ConfigureProperty(address => address.ContactPhone, propertyBuilder =>
        {
            propertyBuilder
                .SetDisplayName("Phone")
                .SetDescription("Address contact phone.")
                .SetOrder(1)
                .SetSearchType(SearchType.CaseInsensitiveContains);
        }).ConfigureProperty(address => address.PostalCode, propertyBuilder =>
        {
            propertyBuilder.SetIsHidden(true);
        }).ConfigureProperty(address => address.City, propertyBuilder =>
        {
            propertyBuilder.SetDisplayName("Town");
        }).ConfigureProperty(address => address.Longitude, propertyBuilder =>
        {
            propertyBuilder
                .SetOrder(6)
                .SetSearchType(SearchType.CaseSensitiveStartsWith);
        }).ConfigureProperty(address => address.Country, propertyBuilder =>
        {
            propertyBuilder.SetSearchType(SearchType.CaseInsensitiveContains);
        });
    }
}