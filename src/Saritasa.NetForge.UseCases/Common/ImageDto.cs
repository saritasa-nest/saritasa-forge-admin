namespace Saritasa.NetForge.UseCases.Common;

/// <summary>
/// DTO for image.
/// </summary>
public record ImageDto
{
    /// <summary>
    /// Name of the image property.
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// Path to file.
    /// </summary>
    public string? PathToFile { get; set; }

    /// <summary>
    /// Bytes that represent file content.
    /// </summary>
    public byte[]? FileContent { get; set; }
}
