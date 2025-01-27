namespace Saritasa.NetForge.Domain.Entities.Options;

/// <summary>
/// Contains custom messages for operations.
/// </summary>
public class MessageOptions
{
    /// <summary>
    /// Message indicating that entity was created successfully.
    /// </summary>
    public string? EntityCreateMessage { get; set; }

    /// <summary>
    /// Message indicating that entity was saved successfully.
    /// </summary>
    public string? EntitySaveMessage { get; set; }

    /// <summary>
    /// Message indicating that entity was deleted successfully.
    /// </summary>
    public string? EntityDeleteMessage { get; set; }

    /// <summary>
    /// Message indicating that multiple entities were deleted successfully.
    /// </summary>
    public string? EntityBulkDeleteMessage { get; set; }
}
