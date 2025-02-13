using Saritasa.NetForge.Domain;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.EfCoreMetadataServiceTests.NavigationMetadata.Entities;
using Saritasa.NetForge.Tests.Helpers;
using Xunit;

namespace Saritasa.NetForge.Tests.EfCoreMetadataServiceTests.NavigationMetadata;

/// <summary>
/// Tests for global and entity levels navigation depth.
/// </summary>
public class NavigationDepthTests
{
    private readonly TestDbContext testDbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    public NavigationDepthTests()
    {
        testDbContext = EfCoreHelper.CreateTestDbContext();
    }

    /// <summary>
    /// Validates that default navigation depth value (2) affects getting navigation metadata.
    /// </summary>
    [Fact]
    public void GetMetadata_DefaultNavigationDepthIs2_ShouldGetProjectNavigationWithoutNestedNavigations()
    {
        // Arrange
        var efCoreMetadataService = EfCoreHelper.CreateEfCoreMetadataService(testDbContext);

        // Act
        var metadata = efCoreMetadataService.GetMetadata();
        var employeeMetadata = metadata.First(entityMetadata => entityMetadata.ClrType!.Name == nameof(Employee));
        var departmentMetadata = employeeMetadata.Navigations.First(navigation => navigation.Name == nameof(Department));
        var projectMetadata = departmentMetadata.TargetEntityNavigations
            .FirstOrDefault(navigation => navigation.Name == nameof(Department.Projects));

        // Assert
        Assert.NotNull(projectMetadata);
        Assert.Empty(projectMetadata.TargetEntityNavigations);
    }

    /// <summary>
    /// Validates that global navigation depth affects getting navigation metadata.
    /// </summary>
    [Fact]
    public void GetMetadata_GlobalNavigationDepthIs1_ShouldGetDepartmentNavigationWithoutNestedNavigations()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();
        adminOptionsBuilder.SetMaxNavigationDepth(1);
        var adminOptions = adminOptionsBuilder.Create();
        var efCoreMetadataService = EfCoreHelper.CreateEfCoreMetadataService(testDbContext, adminOptions);

        // Act
        var metadata = efCoreMetadataService.GetMetadata();
        var employeeMetadata = metadata.First(entityMetadata => entityMetadata.ClrType!.Name == nameof(Employee));
        var departmentMetadata = employeeMetadata.Navigations.First(navigation => navigation.Name == nameof(Department));

        // Assert
        Assert.NotNull(departmentMetadata);
        Assert.Empty(departmentMetadata.TargetEntityNavigations);
    }

    /// <summary>
    /// Validates that entity level navigation depth affects getting navigation metadata.
    /// </summary>
    [Fact]
    public void GetMetadata_EntityNavigationDepthIs1_ShouldGetDepartmentNavigationWithoutNestedNavigations()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();
        adminOptionsBuilder.ConfigureEntity<Employee>(builder =>
        {
            builder.SetMaxNavigationDepth(1);
        });
        var adminOptions = adminOptionsBuilder.Create();
        var efCoreMetadataService = EfCoreHelper.CreateEfCoreMetadataService(testDbContext, adminOptions);

        // Act
        var metadata = efCoreMetadataService.GetMetadata();
        var employeeMetadata = metadata.First(entityMetadata => entityMetadata.ClrType!.Name == nameof(Employee));
        var departmentMetadata = employeeMetadata.Navigations.First(navigation => navigation.Name == nameof(Department));

        // Assert
        Assert.NotNull(departmentMetadata);
        Assert.Empty(departmentMetadata.TargetEntityNavigations);
    }

    /// <summary>
    /// Validates that entity level navigation depth has precedence over global navigation depth.
    /// </summary>
    [Fact]
    public void GetMetadata_EntityNavigationDepth_ShouldTakePrecedenceOverGlobalNavigationDepth()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();
        adminOptionsBuilder.SetMaxNavigationDepth(1);
        adminOptionsBuilder.ConfigureEntity<Employee>(builder =>
        {
            builder.SetMaxNavigationDepth(0);
        });
        var adminOptions = adminOptionsBuilder.Create();
        var efCoreMetadataService = EfCoreHelper.CreateEfCoreMetadataService(testDbContext, adminOptions);

        // Act
        var metadata = efCoreMetadataService.GetMetadata();
        var employeeMetadata = metadata.First(entityMetadata => entityMetadata.ClrType!.Name == nameof(Employee));

        // Assert
        Assert.Empty(employeeMetadata.Navigations);
    }
}
