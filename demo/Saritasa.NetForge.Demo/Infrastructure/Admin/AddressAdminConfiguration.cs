using Saritasa.NetForge.Domain;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.Domain.Interfaces;
using Saritasa.NetForge.Demo.Models;

namespace Saritasa.NetForge.Demo.Infrastructure.Admin;

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
                .SetSearchType(SearchType.ContainsCaseInsensitive);
        }).ConfigureProperty(address => address.PostalCode, propertyBuilder =>
        {
            propertyBuilder.SetIsHidden(true);
        }).ConfigureProperty(address => address.City, propertyBuilder =>
        {
            propertyBuilder.SetDisplayName("Town");
        }).ConfigureProperty(address => address.Street, propertyBuilder =>
        {
            propertyBuilder.SetIsMultiline(autoGrow: true, maxLines: 10);
        }).ConfigureCalculatedProperty(address => address.FullAddress, propertyBuilder =>
        {
            propertyBuilder
                .SetDescription("Contains street, city and country.")
                .SetOrder(5);
        }).ConfigureProperty(address => address.Longitude, propertyBuilder =>
        {
            propertyBuilder
                .SetOrder(6)
                .SetSearchType(SearchType.StartsWithCaseSensitive)
                .SetIsHiddenFromListView(true);
        }).ConfigureProperty(address => address.Country, propertyBuilder =>
        {
            propertyBuilder.SetSearchType(SearchType.ContainsCaseInsensitive);
        }).ConfigureProperty(address => address.CreatedByUserId, propertyBuilder =>
        {
            propertyBuilder
                .SetIsHiddenFromCreate(true)
                .SetIsReadOnly(true);
        });

        entityOptionsBuilder.SetAfterUpdateAction((serviceProvider, originalEntity, modifiedEntity) =>
        {
            var dbContext = serviceProvider!.GetRequiredService<ShopDbContext>();

            const string country = "Germany";
            if (originalEntity.Country == country)
            {
                return;
            }

            if (modifiedEntity.Country == country)
            {
                modifiedEntity.PostalCode = "99998";
            }

            dbContext.SaveChanges();
        });

        entityOptionsBuilder
            .SetEntityCreateMessage("Address was created.")
            .SetEntitySaveMessage("Address was saved.")
            .SetEntityDeleteMessage("Address was deleted.")
            .SetEntityBulkDeleteMessage("Selected addresses were deleted.");
    }
}