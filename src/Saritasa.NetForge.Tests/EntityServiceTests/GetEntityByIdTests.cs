using System.ComponentModel;
using Moq;
using Saritasa.NetForge.Domain;
using Saritasa.NetForge.Domain.Attributes;
using Saritasa.NetForge.Domain.Exceptions;
using Saritasa.NetForge.Domain.UseCases.Interfaces;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.Domain.UseCases.Metadata.Services;
using Saritasa.NetForge.Domain.UseCases.Services;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Constants;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Helpers;
using Xunit;

namespace Saritasa.NetForge.Tests.EntityServiceTests;

/// <summary>
/// Tests for <see cref="EntityService.GetEntityByIdAsync"/>.
/// </summary>
public class GetEntityByIdTests : IDisposable
{
    private const string AttributeTestEntityId = "Addresses";
    private const string FluentApiTestEntityId = "Shops";

    private readonly TestDbContext testDbContext;
    private readonly IEntityService entityService;
    private readonly AdminOptionsBuilder adminOptionsBuilder;

    /// <summary>
    /// Constructor.
    /// </summary>
    public GetEntityByIdTests()
    {
        testDbContext = EfCoreHelper.CreateTestDbContext();
        adminOptionsBuilder = new AdminOptionsBuilder();
        var adminMetadataService = new AdminMetadataService(
            EfCoreHelper.CreateEfCoreMetadataService(testDbContext),
            adminOptionsBuilder.Create(),
            MemoryCacheHelper.CreateMemoryCache());

        entityService = new EntityService(adminMetadataService, new Mock<IServiceProvider>().Object);
    }

    private bool disposedValue;

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Deletes the database after one test is complete,
    /// so it gives us the same state of the database for every test.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                testDbContext.Dispose();
            }

            disposedValue = true;
        }
    }

    /// <summary>
    /// Test for case when string id is valid.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_ValidStringId_ShouldBeNotNull()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(AttributeTestEntityId, CancellationToken.None);

        // Assert
        Assert.NotNull(entity);
    }

    /// <summary>
    /// Test for case when string id is invalid.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_InvalidStringId_ShouldThrowNotFoundException()
    {
        // Arrange
        const string invalidStringId = "Addresses2";

        // Act
        var getEntityByIdCall = () => entityService.GetEntityByIdAsync(invalidStringId, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(getEntityByIdCall);
    }

    /// <summary>
    /// Test for case when property excluded from query via Fluent API.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithExcludedFromQueryPropertyViaFluentApi_PropertyShouldBeExcluded()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder
                .ConfigureProperty(shop => shop.IsOpen, optionsBuilder => optionsBuilder.SetIsExcludedFromQuery(true));
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.IsExcludedFromQuery);
    }

    /// <summary>
    /// Test for case when property excluded from query via <see cref="NetForgePropertyAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithExcludedFromQueryPropertyViaAttribute_PropertyShouldBeExcluded()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(AttributeTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.IsExcludedFromQuery);
    }

    /// <summary>
    /// Test for case when property is hidden via Fluent API.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithHiddenPropertyViaFluentApi_PropertyShouldBeHidden()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.ConfigureProperty(shop => shop.IsOpen, optionsBuilder => optionsBuilder.SetIsHidden(true));
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.IsHidden);
    }

    /// <summary>
    /// Test for case when property is hidden via <see cref="NetForgePropertyAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithHiddenPropertyViaAttribute_PropertyShouldBeHidden()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(AttributeTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.IsHidden);
    }

    /// <summary>
    /// Test for case when property is hidden from list view via Fluent API.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithHiddenFromListViewPropertyViaFluentApi_PropertyShouldBeHidden()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.ConfigureProperty(
                shop => shop.IsOpen, optionsBuilder => optionsBuilder.SetIsHiddenFromListView(true));
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.IsHiddenFromListView);
    }

    /// <summary>
    /// Test for case when property is hidden from list view via <see cref="NetForgePropertyAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithHiddenFromListViewPropertyViaAttribute_PropertyShouldBeHidden()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(AttributeTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.IsHiddenFromListView);
    }

    /// <summary>
    /// Test for case when property is hidden from create entity page via Fluent API.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithHiddenFromCreatePropertyViaFluentApi_PropertyShouldBeHidden()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.ConfigureProperty(
                shop => shop.IsOpen, optionsBuilder => optionsBuilder.SetIsHiddenFromCreate(true));
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.IsHiddenFromCreate);
    }

    /// <summary>
    /// Test for case when property is hidden from list view via <see cref="NetForgePropertyAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithHiddenFromCreatePropertyViaAttribute_PropertyShouldBeHidden()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(AttributeTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.IsHiddenFromCreate);
    }

    /// <summary>
    /// Test for case when property is hidden from details via Fluent API.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithHiddenFromDetailsPropertyViaFluentApi_PropertyShouldBeHidden()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.ConfigureProperty(
                shop => shop.IsOpen, optionsBuilder => optionsBuilder.SetIsHiddenFromDetails(true));
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.IsHiddenFromDetails);
    }

    /// <summary>
    /// Test for case when property is hidden from details via <see cref="NetForgePropertyAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithHiddenFromDetailsPropertyViaAttribute_PropertyShouldBeHidden()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(AttributeTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.IsHiddenFromDetails);
    }

    /// <summary>
    /// Test for case when properties don't have ordering.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithoutOrdering_PrimaryKeyShouldBeFirst()
    {
        // Arrange
        const string expectedPropertyName = nameof(Shop.Id);

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.Equal(expectedPropertyName, entity.Properties.First().Name);
    }

    /// <summary>
    /// Test for case when properties have ordering via Fluent API.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithOrderingViaFluentApi_OrderShouldBeSet()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder
                .ConfigureProperty(shop => shop.TotalSales, optionsBuilder => optionsBuilder.SetOrder(0))
                .ConfigureProperty(shop => shop.Id, optionsBuilder => optionsBuilder.SetOrder(1));
        });
        const string expectedPropertyName = nameof(Shop.TotalSales);
        const int expectedOrder = 0;

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);
        var actualOrder = entity.Properties.First(property => property.Name == expectedPropertyName).Order;

        // Assert
        Assert.Equal(expectedOrder, actualOrder);
    }

    /// <summary>
    /// Test for case when properties have ordering via <see cref="NetForgePropertyAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithOrderingViaAttribute_OrderShouldBeSet()
    {
        // Arrange
        const string expectedPropertyName = nameof(Address.Latitude);
        const int expectedOrder = 0;

        // Act
        var entity = await entityService.GetEntityByIdAsync(AttributeTestEntityId, CancellationToken.None);
        var actualOrder = entity.Properties.First(property => property.Name == expectedPropertyName).Order;

        // Assert
        Assert.Equal(expectedOrder, actualOrder);
    }

    /// <summary>
    /// Test for case when property have configured form order via FluentAPI.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_FluentAPI_FormOrder_ShouldBeConfigured()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.ConfigureProperty(shop => shop.TotalSales, optionsBuilder => optionsBuilder.SetFormOrder(1));
        });
        const string expectedPropertyName = nameof(Shop.TotalSales);
        const int expectedOrder = 1;

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);
        var actualOrder = entity.Properties.First(property => property.Name == expectedPropertyName).FormOrder;

        // Assert
        Assert.Equal(expectedOrder, actualOrder);
    }

    /// <summary>
    /// Test for case when property have configured form order via FluentAPI.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_FluentAPI_NavigationFormOrder_ShouldBeConfigured()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.IncludeNavigation<Address>(shop => shop.Address, optionsBuilder => optionsBuilder.SetFormOrder(1));
        });
        const string expectedPropertyName = nameof(Shop.Address);
        const int expectedOrder = 1;

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);
        var actualOrder = entity.Properties.First(property => property.Name == expectedPropertyName).FormOrder;

        // Assert
        Assert.Equal(expectedOrder, actualOrder);
    }

    /// <summary>
    /// Test for case when property have <see cref="NetForgePropertyAttribute.FormOrder"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_Attribute_FormOrder_ShouldBeConfigured()
    {
        // Arrange
        const string expectedPropertyName = nameof(Address.PostalCode);
        const int expectedOrder = 1;

        // Act
        var entity = await entityService.GetEntityByIdAsync(AttributeTestEntityId, CancellationToken.None);
        var actualOrder = entity.Properties.First(property => property.Name == expectedPropertyName).FormOrder;

        // Assert
        Assert.Equal(expectedOrder, actualOrder);
    }

    /// <summary>
    /// Test for case when property has set display name via Fluent API.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithPropertyDisplayNameViaFluentApi_DisplayNameShouldChange()
    {
        // Arrange
        const string displayName = "Sales";
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.ConfigureProperty(shop => shop.TotalSales,
                    optionsBuilder => optionsBuilder.SetDisplayName(displayName));
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(
            entity.Properties, property => property.DisplayName.Equals(displayName));
    }

    /// <summary>
    /// Test for case when property has set display name via <see cref="NetForgePropertyAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithPropertyDisplayNameViaAttribute_DisplayNameShouldChange()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(AttributeTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(
            entity.Properties, property => property.DisplayName.Equals(AddressConstants.LatitudeDisplayName));
    }

    /// <summary>
    /// Test for case when property has set display name via <see cref="DisplayNameAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithPropertyDisplayNameViaBuiltInAttribute_DisplayNameShouldChange()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(AttributeTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(
            entity.Properties, property => property.DisplayName.Equals(AddressConstants.StreetDisplayName));
    }

    /// <summary>
    /// Test for case when property has set description via Fluent API.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithPropertyDescriptionViaFluentApi_DescriptionShouldChange()
    {
        // Arrange
        const string description = "Information about total sales that was made by shop.";
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.ConfigureProperty(
                shop => shop.TotalSales,
                optionsBuilder => optionsBuilder.SetDescription(description));
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(
            entity.Properties, property => property.Description.Equals(description));
    }

    /// <summary>
    /// Test for case when property has set description via <see cref="NetForgePropertyAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithPropertyDescriptionViaAttribute_DescriptionShouldChange()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(AttributeTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(
            entity.Properties, property => property.Description.Equals(AddressConstants.LatitudeDescription));
    }

    /// <summary>
    /// Test for case when property has set description via <see cref="DescriptionAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithPropertyDescriptionViaBuiltInAttribute_DescriptionShouldChange()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(AttributeTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(
            entity.Properties, property => property.Description.Equals(AddressConstants.StreetDescription));
    }

    /// <summary>
    /// Test for case when navigation included to entity.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithNavigation_ShouldBeNotNull()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.IncludeNavigation<Address>(entity => entity.Address, navigationOptionsBuilder =>
            {
                navigationOptionsBuilder.IncludeProperty(navigation => navigation.Id);
            });
        });

        const string navigationPropertyName = nameof(Shop.Address);

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.Name.Equals(navigationPropertyName));
    }

    /// <summary>
    /// Test for case when property inside navigation customized.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_CustomizedPropertyInsideNavigation_ShouldBeCustomized()
    {
        // Arrange
        const string idDisplayName = "Address Id";
        const string streetDescription = "Address street name.";

        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.IncludeNavigation<Address>(entity => entity.Address, navigationOptionsBuilder =>
            {
                navigationOptionsBuilder.
                    IncludeProperty(address => address.Id, propertyOptionsBuilder =>
                    {
                        propertyOptionsBuilder.SetDisplayName(idDisplayName);
                    })
                    .IncludeProperty(address => address.Street, propertyOptionsBuilder =>
                    {
                        propertyOptionsBuilder.SetDescription(streetDescription);
                    });
            });
        });

        const string navigationPropertyName = nameof(Shop.Address);

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        var navigation = entity.Properties
            .FirstOrDefault(property => property.Name.Equals(navigationPropertyName)) as NavigationMetadataDto;

        Assert.NotNull(navigation);
        Assert
            .Contains(navigation.TargetEntityProperties, property => property.DisplayName.Equals(idDisplayName));
        Assert
            .Contains(navigation.TargetEntityProperties, property => property.Description.Equals(streetDescription));
    }

    /// <summary>
    /// Test for case when entity operations are not configured.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_EntityOperationsAreNotConfigured_TheyShouldBeTrue()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.True(entity.CanAdd);
        Assert.True(entity.CanEdit);
        Assert.True(entity.CanDelete);
    }

    /// <summary>
    /// Test for case when add possibility of an entity is disabled.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_AddOperationDisabled_CanAddShouldBeFalse()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.SetCanAdd(false);
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.False(entity.CanAdd);
    }

    /// <summary>
    /// Test for case when edit possibility of an entity is disabled.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_EditOperationDisabled_CanEditShouldBeFalse()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.SetCanEdit(false);
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.False(entity.CanEdit);
    }

    /// <summary>
    /// Test for case when delete possibility of an entity is disabled.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_DeleteOperationDisabled_CanDeleteShouldBeFalse()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.SetCanDelete(false);
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.False(entity.CanDelete);
    }

    /// <summary>
    /// Test to check that custom per-model create message is configured correctly.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_EntityCreateMessage_ShouldBeConfigured()
    {
        // Arrange
        const string entityCreateMessage = "Entity create test message.";
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.SetEntityCreateMessage(entityCreateMessage);
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.Equal(entityCreateMessage, entity.MessageOptions.EntityCreateMessage);
    }

    /// <summary>
    /// Test to check that custom per-model save message is configured correctly.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_EntitySaveMessage_ShouldBeConfigured()
    {
        // Arrange
        const string entitySaveMessage = "Entity save test message.";
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.SetEntitySaveMessage(entitySaveMessage);
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.Equal(entitySaveMessage, entity.MessageOptions.EntitySaveMessage);
    }

    /// <summary>
    /// Test to check that custom per-model delete message is configured correctly.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_EntityDeleteMessage_ShouldBeConfigured()
    {
        // Arrange
        const string entityDeleteMessage = "Entity delete test message.";
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.SetEntityDeleteMessage(entityDeleteMessage);
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.Equal(entityDeleteMessage, entity.MessageOptions.EntityDeleteMessage);
    }

    /// <summary>
    /// Test to check that custom per-model bulk delete message is configured correctly.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_EntityBulkDeleteMessage_ShouldBeConfigured()
    {
        // Arrange
        const string entityBulkDeleteMessage = "Entity bulk delete test message.";
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.SetEntityBulkDeleteMessage(entityBulkDeleteMessage);
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.Equal(entityBulkDeleteMessage, entity.MessageOptions.EntityBulkDeleteMessage);
    }

    /// <summary>
    /// Test to check that custom create entity action is configured correctly.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_CreateAction_ShouldBeConfigured()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.SetCreateAction((_, entity) =>
            {
                entity.IsOpen = true;
            });
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.NotNull(entity.CreateAction);
    }

    /// <summary>
    /// Test to check that custom update entity action is configured correctly.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_UpdateAction_ShouldBeConfigured()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.SetUpdateAction((_, entity) =>
            {
                entity.IsOpen = true;
            });
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        // Assert
        Assert.NotNull(entity.UpdateAction);
    }

    /// <summary>
    /// Verifies default sort configured correctly.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_SetDefaultSort_ShouldBeConfigured()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.ConfigureProperty(shop => shop.Name, propertyBuilder =>
            {
                propertyBuilder.SetDefaultSort(1, isAscending: true);
            });
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);
        var name = entity.Properties.First(property => property.PropertyPath == nameof(Shop.Name));

        // Assert
        Assert.Contains(
            entity.DefaultOrderings, ordering => ordering is { PropertyPath: nameof(Shop.Name), IsAscending: true });
        Assert.Single(entity.DefaultOrderings);

        Assert.True(name.IsSortable);
    }

    /// <summary>
    /// Verifies default sort configured correctly using navigation's property.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_SetDefaultSort_InNavigation_ShouldBeConfigured()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Product>(builder =>
        {
            builder.IncludeNavigation<Shop>(product => product.Shop, shopBuilder =>
            {
                shopBuilder.IncludeNavigation<Address>(shop => shop.Address, addressBuilder =>
                {
                    addressBuilder.IncludeProperty(address => address.Street, streetBuilder =>
                    {
                        streetBuilder.SetDefaultSort(1, isAscending: true);
                    });
                });
            });
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync("Products", CancellationToken.None);

        var shop = (NavigationMetadataDto)entity.Properties.First(property => property.Name == nameof(Product.Shop));
        var address = shop.TargetEntityNavigations.First(property => property.Name == nameof(Shop.Address));
        var sortableProperty = address.TargetEntityProperties.First(property => property.Name == nameof(Address.Street));

        // Assert
        Assert.Contains(
            entity.DefaultOrderings, ordering => ordering is { PropertyPath: "Shop.Address.Street", IsAscending: true });
        Assert.Single(entity.DefaultOrderings);

        Assert.True(sortableProperty.IsSortable);
    }

    /// <summary>
    /// Verifies default sort on multiple properties configured correctly.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_SetDefaultSort_MultipleSort_ShouldBeConfigured()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.ConfigureProperty(shop => shop.Name, propertyBuilder =>
            {
                propertyBuilder.SetDefaultSort(1, isAscending: true);
            });

            builder.ConfigureProperty(shop => shop.IsOpen, propertyBuilder =>
            {
                propertyBuilder.SetDefaultSort(2, isAscending: false);
            });
        });
        const int orderingsCount = 2;

        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);

        var name = entity.Properties.First(property => property.PropertyPath == nameof(Shop.Name));
        var isOpen = entity.Properties.First(property => property.PropertyPath == nameof(Shop.IsOpen));

        // Assert
        Assert.Contains(
            entity.DefaultOrderings, ordering => ordering is { PropertyPath: nameof(Shop.Name), IsAscending: true });
        Assert.Contains(
            entity.DefaultOrderings, ordering => ordering is { PropertyPath: nameof(Shop.IsOpen), IsAscending: false });
        Assert.Equal(orderingsCount, entity.DefaultOrderings.Count);

        Assert.True(name.IsSortable);
        Assert.True(isOpen.IsSortable);
    }

    /// <summary>
    /// Verifies default sort uses primary key when not configured.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithoutDefaultSortConfiguration_ShouldUsePrimaryKey()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(FluentApiTestEntityId, CancellationToken.None);
        var id = entity.Properties.First(property => property.PropertyPath == nameof(Shop.Id));

        // Assert
        var primaryKey = entity.Properties.First(property => property.IsPrimaryKey);
        Assert.Contains(entity.DefaultOrderings, ordering => ordering.PropertyPath == primaryKey.Name);
        Assert.Single(entity.DefaultOrderings);

        Assert.True(id.IsSortable);
    }

    /// <summary>
    /// Verifies default sort on entity with composite primary key uses all primary keys when not configured.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithoutDefaultSortConfiguration_CompositePrimaryKey_ShouldUseAllPrimaryKeys()
    {
        // Arrange
        const int primaryKeysCount = 2;

        // Act
        var entity = await entityService.GetEntityByIdAsync("Suppliers", CancellationToken.None);
        var primaryKeyNames = entity.Properties
            .Where(property => property.IsPrimaryKey)
            .Select(property => property.Name);

        var name = entity.Properties.First(property => property.PropertyPath == nameof(Supplier.Name));
        var city = entity.Properties.First(property => property.PropertyPath == nameof(Supplier.City));

        // Assert
        Assert.Contains(entity.DefaultOrderings, ordering => primaryKeyNames.Contains(ordering.PropertyPath));
        Assert.Equal(primaryKeysCount, entity.DefaultOrderings.Count);

        Assert.True(name.IsSortable);
        Assert.True(city.IsSortable);
    }
}
