using System.Linq.Expressions;
using Saritasa.NetForge.Domain.Extensions;
using Saritasa.NetForge.Tests.Domain.Models;
using Xunit;

namespace Saritasa.NetForge.Tests.Expressions;

/// <summary>
/// Tests for <see cref="ExpressionExtensions"/>.
/// </summary>
public class ExpressionExtensionsTests
{
    /// <summary>
    /// Verifies that expression that returned by <see cref="ExpressionExtensions.GetPropertyExpression"/>
    /// can be used for getting property's value.
    /// </summary>
    [Fact]
    public void GetPropertyExpression_RegularProperty_ShouldGetShopName()
    {
        // Arrange
        var shopParameter = Expression.Parameter(typeof(Shop));
        var shop = new Shop { Name = "Test Shop" };

        // Act
        var propertyExpression = ExpressionExtensions.GetPropertyExpression(shopParameter, "Name");
        var lambda = Expression.Lambda(propertyExpression, shopParameter);
        var shopName = lambda.Compile().DynamicInvoke(shop);

        // Assert
        Assert.Equal("Test Shop", shopName);
    }

    /// <summary>
    /// Verifies that expression that returned by <see cref="ExpressionExtensions.GetPropertyExpression"/>
    /// can be used for getting nested property's value.
    /// </summary>
    [Fact]
    public void GetPropertyExpression_NestedProperty_ShouldGetShopAddressStreet()
    {
        // Arrange
        var productParameter = Expression.Parameter(typeof(Product));
        var shop = new Product
        {
            Shop = new Shop
            {
                Address = new Address
                {
                    Street = "Test Street"
                }
            }
        };

        // Act
        var propertyExpression = ExpressionExtensions.GetPropertyExpression(productParameter, "Shop.Address.Street");
        var lambda = Expression.Lambda(propertyExpression, productParameter);
        var street = lambda.Compile().DynamicInvoke(shop);

        // Assert
        Assert.Equal("Test Street", street);
    }

    /// <summary>
    /// Verifies that expression that returned by <see cref="ExpressionExtensions.GetPropertyExpressionWithNullCheck"/>
    /// can be used for getting property's value.
    /// </summary>
    [Fact]
    public void GetPropertyExpressionWithNullCheck_RegularProperty_ShouldGetShopName()
    {
        // Arrange
        var shopParameter = Expression.Parameter(typeof(Shop));
        var shop = new Shop { Name = "Test Shop" };

        // Act
        var propertyExpression = ExpressionExtensions.GetPropertyExpressionWithNullCheck(shopParameter, "Name");
        var lambda = Expression.Lambda(propertyExpression, shopParameter);
        var shopName = lambda.Compile().DynamicInvoke(shop);

        // Assert
        Assert.Equal("Test Shop", shopName);
    }

    /// <summary>
    /// Verifies that expression that returned by <see cref="ExpressionExtensions.GetPropertyExpressionWithNullCheck"/>
    /// can be used for getting nested property's value.
    /// </summary>
    [Fact]
    public void GetPropertyExpressionWithNullCheck_NestedProperty_ShouldGetShopAddressStreet()
    {
        // Arrange
        var productParameter = Expression.Parameter(typeof(Product));
        var shop = new Product
        {
            Shop = new Shop
            {
                Address = new Address
                {
                    Street = "Test Street"
                }
            }
        };

        // Act
        var propertyExpression = ExpressionExtensions.GetPropertyExpressionWithNullCheck(productParameter, "Shop.Address.Street");
        var lambda = Expression.Lambda(propertyExpression, productParameter);
        var street = lambda.Compile().DynamicInvoke(shop);

        // Assert
        Assert.Equal("Test Street", street);
    }
}
