using System.ComponentModel.DataAnnotations;
using Extender;
using Moq;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Helpers;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.Services;
using Saritasa.NetForge.UseCases.Services;
using Xunit;

namespace Saritasa.NetForge.Tests.EntityServiceTests;

/// <summary>
/// Tests for <see cref="EntityService.ValidateEntity"/>.
/// </summary>
public class ValidateEntityTests
{
    private readonly IEntityService entityService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public ValidateEntityTests()
    {
        var testDbContext = EfCoreHelper.CreateTestDbContext();
        var adminOptionsBuilder = new AdminOptionsBuilder();
        var adminMetadataService = new AdminMetadataService(
            EfCoreHelper.CreateEfCoreMetadataService(testDbContext),
            adminOptionsBuilder.Create(),
            MemoryCacheHelper.CreateMemoryCache());

        entityService = new EntityService(adminMetadataService,
            EfCoreHelper.CreateEfCoreDataService(testDbContext),
            new Mock<IServiceProvider>().Object);
    }

    /// <summary>
    /// Test for case when single validation attribute on single property is valid.
    /// </summary>
    [Fact]
    public void ValidateEntity_SinglePropertySingleValidate_ShouldNotHaveError()
    {
        // Prepare
        const string propertyName = "Street";

        var typeExtender = new TypeExtender("Saritasa.NetForge.Tests.Domain.Models.Dummy");
        typeExtender.AddProperty(propertyName, typeof(string), new List<Tuple<Type, object[]>>()
        {
            new(typeof(RequiredAttribute), null!)
        });
        var instanceType = typeExtender.FetchType();

        var instance = Activator.CreateInstance(instanceType);
        instance!.GetType().GetProperty(propertyName)?.SetValue(instance, "value");

        var errors = new List<ValidationResult>();

        // Act
        var result = entityService.ValidateEntity(instance, ref errors);

        // Assert
        Assert.True(result);
        Assert.Empty(errors);
    }

    /// <summary>
    /// Test for case when single validation attribute on single property is not valid.
    /// </summary>
    [Fact]
    public void ValidateEntity_SinglePropertySingleValidate_ShouldHaveError()
    {
        // Prepare
        const string propertyName = "Street";

        var typeExtender = new TypeExtender("Saritasa.NetForge.Tests.Domain.Models.Dummy");
        typeExtender.AddProperty(propertyName, typeof(string), new List<Tuple<Type, object[]>>()
        {
            new(typeof(RequiredAttribute), null!)
        });
        var instanceType = typeExtender.FetchType();

        var instance = Activator.CreateInstance(instanceType);

        var errors = new List<ValidationResult>();

        // Act
        var result = entityService.ValidateEntity(instance!, ref errors);

        // Assert
        Assert.False(result);
        Assert.Single(errors);

        var requireError = errors[0];
        Assert.Contains(propertyName, requireError.MemberNames);
        Assert.All(new List<string> { propertyName, "required" }, item => Assert.Contains(item, requireError.ErrorMessage));
    }

    /// <summary>
    /// Test for case when multiple validation attribute on single property is valid.
    /// </summary>
    [Fact]
    public void ValidateEntity_SinglePropertyMultipleValidate_ShouldNotHaveError()
    {
        // Prepare
        const string propertyName = "Street";

        var typeExtender = new TypeExtender("Saritasa.NetForge.Tests.Domain.Models.Dummy");
        typeExtender.AddProperty(propertyName, typeof(string), new List<Tuple<Type, object[]>>()
        {
            new(typeof(MinLengthAttribute), [1]),
            new(typeof(MaxLengthAttribute), [10]),
        });
        var instanceType = typeExtender.FetchType();

        var instance = Activator.CreateInstance(instanceType);
        instance!.GetType().GetProperty(propertyName)?.SetValue(instance, "value");

        var errors = new List<ValidationResult>();

        // Act
        var result = entityService.ValidateEntity(instance, ref errors);

        // Assert
        Assert.True(result);
        Assert.Empty(errors);
    }

    /// <summary>
    /// Test for case when multiple validation attribute on single property is not valid.
    /// </summary>
    [Fact]
    public void ValidateEntity_SinglePropertyMultipleValidate_ShouldHaveError()
    {
        // Prepare
        const string propertyName = "Street";
        const int minLengthValue = 10;
        const int maxLengthValue = 1;

        var typeExtender = new TypeExtender("Saritasa.NetForge.Tests.Domain.Models.Dummy");
        typeExtender.AddProperty(propertyName, typeof(string), new List<Tuple<Type, object[]>>()
        {
            new(typeof(MinLengthAttribute), [minLengthValue]),
            new(typeof(MaxLengthAttribute), [maxLengthValue]),
        });
        var instanceType = typeExtender.FetchType();

        var instance = Activator.CreateInstance(instanceType);
        instance!.GetType().GetProperty(propertyName)?.SetValue(instance, "value");

        var errors = new List<ValidationResult>();

        // Act
        var result = entityService.ValidateEntity(instance, ref errors);

        // Assert
        Assert.False(result);
        Assert.Equal(2, errors.Count);

        var minLengthError = errors[0];
        Assert.Contains(propertyName, minLengthError.MemberNames);
        Assert.All(new List<string> { propertyName, "minimum length", minLengthValue.ToString() }, item => Assert.Contains(item, minLengthError.ErrorMessage));

        var maxLengthError = errors[1];
        Assert.Contains(propertyName, maxLengthError.MemberNames);
        Assert.All(new List<string> { propertyName, "maximum length", maxLengthValue.ToString() }, item => Assert.Contains(item, maxLengthError.ErrorMessage));
    }

    /// <summary>
    /// Test for case when multiple validation attribute on multiple property is valid.
    /// </summary>
    [Fact]
    public void ValidateEntity_MultiplePropertyMultipleValidate_ShouldNotHaveError()
    {
        // Prepare
        const string firstPropertyName = "Street";
        const string secondPropertyName = "Latitude";

        var typeExtender = new TypeExtender("Saritasa.NetForge.Tests.Domain.Models.Dummy");
        typeExtender.AddProperty(firstPropertyName, typeof(string), new List<Tuple<Type, object[]>>()
        {
            new(typeof(MinLengthAttribute), [1]),
            new(typeof(MaxLengthAttribute), [10]),
        });
        typeExtender.AddProperty(secondPropertyName, typeof(int), new List<Tuple<Type, object[]>>()
        {
            new(typeof(RequiredAttribute), null!),
            new(typeof(RangeAttribute), [1, 10]),
        });
        var instanceType = typeExtender.FetchType();

        var instance = Activator.CreateInstance(instanceType);
        instance!.GetType().GetProperty(firstPropertyName)?.SetValue(instance, "value");
        instance.GetType().GetProperty(secondPropertyName)?.SetValue(instance, 5);

        var errors = new List<ValidationResult>();

        // Act
        var result = entityService.ValidateEntity(instance, ref errors);

        // Assert
        Assert.True(result);
        Assert.Empty(errors);
    }

    /// <summary>
    /// Test for case when multiple validation attribute on multiple property is not valid.
    /// </summary>
    [Fact]
    public void ValidateEntity_MultiplePropertyMultipleValidate_ShouldHaveError()
    {
        // Prepare
        const string firstPropertyName = "Street";
        const int firstPropertyMinLengthValue = 10;
        const int firstPropertyMaxLengthValue = 1;

        const string secondPropertyName = "Latitude";
        const int secondPropertyMinLengthValue = 1;
        const int secondPropertyMaxLengthValue = 10;

        var typeExtender = new TypeExtender("Saritasa.NetForge.Tests.Domain.Models.Dummy");
        typeExtender.AddProperty(firstPropertyName, typeof(string), new List<Tuple<Type, object[]>>()
        {
            new(typeof(MinLengthAttribute), [firstPropertyMinLengthValue]),
            new(typeof(MaxLengthAttribute), [firstPropertyMaxLengthValue]),
        });
        typeExtender.AddProperty(secondPropertyName, typeof(int), new List<Tuple<Type, object[]>>()
        {
            new(typeof(RangeAttribute), [secondPropertyMinLengthValue, secondPropertyMaxLengthValue]),
        });
        var instanceType = typeExtender.FetchType();

        var instance = Activator.CreateInstance(instanceType);
        instance!.GetType().GetProperty(firstPropertyName)?.SetValue(instance, "value");

        var errors = new List<ValidationResult>();

        // Act
        var result = entityService.ValidateEntity(instance, ref errors);

        // Assert
        Assert.False(result);
        Assert.Equal(3, errors.Count);

        var minLengthError = errors[0];
        Assert.Contains(firstPropertyName, minLengthError.MemberNames);
        Assert.All(new List<string> { firstPropertyName, "minimum length", firstPropertyMinLengthValue.ToString() }, item => Assert.Contains(item, minLengthError.ErrorMessage));

        var maxLengthError = errors[1];
        Assert.Contains(firstPropertyName, maxLengthError.MemberNames);
        Assert.All(new List<string> { firstPropertyName, "maximum length", firstPropertyMaxLengthValue.ToString() }, item => Assert.Contains(item, maxLengthError.ErrorMessage));

        var rangeError = errors[2];
        Assert.Contains(secondPropertyName, rangeError.MemberNames);
        Assert.All(new List<string> { secondPropertyName, "between", secondPropertyMinLengthValue.ToString(), secondPropertyMaxLengthValue.ToString() }, item => Assert.Contains(item, rangeError.ErrorMessage));
    }

    /// <summary>
    /// Test for case when custom validation attribute is valid.
    /// </summary>
    [Fact]
    public void ValidateEntity_CustomValidationAttribute_ShouldNotHaveError()
    {
        // Prepare
        var instance = new Dummy
        {
            Phone = "0123-456-789"
        };

        var errors = new List<ValidationResult>();

        // Act
        var result = entityService.ValidateEntity(instance, ref errors);

        // Assert
        Assert.True(result);
        Assert.Empty(errors);
    }

    /// <summary>
    /// Test for case when custom validation attribute is not valid.
    /// </summary>
    [Fact]
    public void ValidateEntity_CustomValidationAttribute_ShouldHaveError()
    {
        var property = "Phone";

        // Prepare
        var instance = new Dummy
        {
            Phone = "0123456789"
        };

        var errors = new List<ValidationResult>();

        // Act
        var result = entityService.ValidateEntity(instance, ref errors);

        // Assert
        Assert.False(result);
        Assert.Single(errors);

        var phoneMaskError = errors[0];
        Assert.Contains("Phone", phoneMaskError.MemberNames);
        Assert.All(new List<string> { property, "does not match", }, item => Assert.Contains(item, phoneMaskError.ErrorMessage));
    }
}
