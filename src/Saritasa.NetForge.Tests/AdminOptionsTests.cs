using Saritasa.NetForge.Blazor.Domain;
using Saritasa.NetForge.Blazor.Domain.Entities.Options;
using Xunit;

namespace Saritasa.NetForge.Tests;

/// <summary>
/// <see cref="AdminOptions"/> tests.
/// </summary>
public class AdminOptionsTests
{
    /// <summary>
    /// Verify that entity group headers are expanded when configured.
    /// </summary>
    [Fact]
    public void AdminOptions_ConfigureGroupHeadersToBeExpanded_ShouldBeExpanded()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();
        var adminOptions = adminOptionsBuilder.Create();

        // Act
        adminOptionsBuilder.SetGroupHeadersExpanded(true);

        // Assert
        Assert.True(adminOptions.GroupHeadersExpanded);
    }

    /// <summary>
    /// Verify that entity group headers are not expanded when configured.
    /// </summary>
    [Fact]
    public void AdminOptions_ConfigureGroupHeadersToBeNotExpanded_ShouldNotBeExpanded()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();
        var adminOptions = adminOptionsBuilder.Create();

        // Act
        adminOptionsBuilder.SetGroupHeadersExpanded(false);

        // Assert
        Assert.False(adminOptions.GroupHeadersExpanded);
    }

    /// <summary>
    /// Verify that entity group headers are expanded by default.
    /// </summary>
    [Fact]
    public void AdminOptions_GroupHeadersAreNotConfigured_ShouldBeExpanded()
    {
        // Arrange & Act
        var adminOptionsBuilder = new AdminOptionsBuilder();
        var adminOptions = adminOptionsBuilder.Create();

        // Assert
        Assert.True(adminOptions.GroupHeadersExpanded);
    }

    /// <summary>
    /// Test to check that custom global create message is configured correctly.
    /// </summary>
    [Fact]
    public void AdminOptions_EntityCreateMessage_ShouldBeConfigured()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();
        var adminOptions = adminOptionsBuilder.Create();

        const string entityCreateMessage = "Test global entity create message.";

        // Act
        adminOptionsBuilder.SetEntityCreateMessage(entityCreateMessage);

        // Assert
        Assert.Equal(entityCreateMessage, adminOptions.MessageOptions.EntityCreateMessage);
    }

    /// <summary>
    /// Test to check that custom global save message is configured correctly.
    /// </summary>
    [Fact]
    public void AdminOptions_EntitySaveMessage_ShouldBeConfigured()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();
        var adminOptions = adminOptionsBuilder.Create();

        const string entitySaveMessage = "Test global entity save message.";

        // Act
        adminOptionsBuilder.SetEntitySaveMessage(entitySaveMessage);

        // Assert
        Assert.Equal(entitySaveMessage, adminOptions.MessageOptions.EntitySaveMessage);
    }

    /// <summary>
    /// Test to check that custom global delete message is configured correctly.
    /// </summary>
    [Fact]
    public void AdminOptions_EntityDeleteMessage_ShouldBeConfigured()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();
        var adminOptions = adminOptionsBuilder.Create();

        const string entityDeleteMessage = "Test global entity delete message.";

        // Act
        adminOptionsBuilder.SetEntityDeleteMessage(entityDeleteMessage);

        // Assert
        Assert.Equal(entityDeleteMessage, adminOptions.MessageOptions.EntityDeleteMessage);
    }

    /// <summary>
    /// Test to check that custom global bulk delete message is configured correctly.
    /// </summary>
    [Fact]
    public void AdminOptions_EntityBulkDeleteMessage_ShouldBeConfigured()
    {
        // Arrange
        var adminOptionsBuilder = new AdminOptionsBuilder();
        var adminOptions = adminOptionsBuilder.Create();

        const string entityBulkDeleteMessage = "Test global entity bulk delete message.";

        // Act
        adminOptionsBuilder.SetEntityBulkDeleteMessage(entityBulkDeleteMessage);

        // Assert
        Assert.Equal(entityBulkDeleteMessage, adminOptions.MessageOptions.EntityBulkDeleteMessage);
    }
}
