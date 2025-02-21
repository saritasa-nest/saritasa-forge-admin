using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.EfCoreMetadataServiceTests.NavigationMetadata.Models;
using Saritasa.NetForge.Tests.Helpers;
using Xunit;

namespace Saritasa.NetForge.Tests.EfCoreMetadataServiceTests.NavigationMetadata;

/// <summary>
/// Tests related to navigation metadata.
/// </summary>
public class NavigationMetadataTests : IDisposable
{
    private readonly TestDbContext testDbContext;
    private readonly IOrmMetadataService ormMetadataService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public NavigationMetadataTests()
    {
        testDbContext = EfCoreHelper.CreateTestDbContext();
        ormMetadataService = EfCoreHelper.CreateEfCoreMetadataService(testDbContext);
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
        if (disposedValue)
        {
            return;
        }

        if (disposing)
        {
            testDbContext.Dispose();
        }

        disposedValue = true;
    }

    /// <summary>
    /// Validates that regular navigation metadata is getting successfully.
    /// </summary>
    [Fact]
    public void GetMetadata_RegularNavigation_ShouldGetNavigationMetadata()
    {
        // Act
        var metadata = ormMetadataService.GetMetadata();
        var employeeMetadata = metadata.First(entityMetadata => entityMetadata.ClrType!.Name == nameof(Employee));

        // Assert
        Assert.Contains(employeeMetadata.Navigations, navigation => navigation.Name == nameof(Employee.Department));
    }

    /// <summary>
    /// Validates that nested navigation metadata is getting successfully.
    /// </summary>
    [Fact]
    public void GetMetadata_NestedNavigation_ShouldGetNavigationMetadata()
    {
        // Act
        var metadata = ormMetadataService.GetMetadata();
        var employeeMetadata = metadata.First(entityMetadata => entityMetadata.ClrType!.Name == nameof(Employee));
        var departmentMetadata = employeeMetadata.Navigations.First(navigation => navigation.Name == nameof(Department));

        // Assert
        Assert.Contains(departmentMetadata.TargetEntityNavigations, navigation => navigation.Name == nameof(Department.Projects));
    }

    /// <summary>
    /// Validates that self-reference navigation metadata is getting successfully.
    /// </summary>
    [Fact]
    public void GetMetadata_SelfReferenceNavigation_ShouldGetNavigationMetadata()
    {
        // Act
        var metadata = ormMetadataService.GetMetadata();
        var employeeMetadata = metadata.First(entityMetadata => entityMetadata.ClrType!.Name == nameof(Employee));

        // Assert
        Assert.Contains(employeeMetadata.Navigations, navigation => navigation.Name == nameof(Employee.Supervisor));
        Assert.Contains(employeeMetadata.Navigations, navigation => navigation.Name == nameof(Employee.Supervised));
    }

    /// <summary>
    /// Validates that navigation metadata of root entity is getting successfully when it is used as navigation.
    /// </summary>
    [Fact]
    public void GetMetadata_RootEntityAsNavigation_ShouldGetNavigationMetadata()
    {
        // Act
        var metadata = ormMetadataService.GetMetadata();
        var employeeMetadata = metadata.First(entityMetadata => entityMetadata.ClrType!.Name == nameof(Employee));
        var departmentMetadata = employeeMetadata.Navigations.First(navigation => navigation.Name == nameof(Department));

        // Assert
        Assert.Contains(departmentMetadata.TargetEntityNavigations, navigation => navigation.Name == nameof(Department.Employees));
    }

    /// <summary>
    /// Validates that navigation collection can have another navigation.
    /// </summary>
    [Fact]
    public void GetMetadata_NavigationCollection_ShouldHaveAnotherNavigation()
    {
        // Act
        var metadata = ormMetadataService.GetMetadata();
        var departmentMetadata = metadata.First(entityMetadata => entityMetadata.ClrType!.Name == nameof(Department));
        var projectMetadata = departmentMetadata.Navigations
            .First(navigation => navigation.Name == nameof(Department.Projects));

        // Assert
        Assert.Contains(projectMetadata.TargetEntityNavigations, navigation => navigation.Name == nameof(Project.Client));
    }

    /// <summary>
    /// Validates that the same entity type can be used multiple times.
    /// </summary>
    [Fact]
    public void GetMetadata_DifferentNavigationsWithTheSameType_ShouldGetNavigationMetadata()
    {
        // Arrange
        const string navigationName = nameof(Employee.Department);

        // Act
        var metadata = ormMetadataService.GetMetadata();
        var employeeMetadata = metadata.First(entityMetadata => entityMetadata.ClrType!.Name == nameof(Employee));
        var supervisorMetadata = employeeMetadata.Navigations.First(navigation => navigation.Name == nameof(Employee.Supervisor));

        // Assert
        Assert.Contains(employeeMetadata.Navigations, navigation => navigation.Name == navigationName);
        Assert.Contains(supervisorMetadata.TargetEntityNavigations, navigation => navigation.Name == navigationName);
    }
}
