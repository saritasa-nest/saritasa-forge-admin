using Saritasa.NetForge.UseCases.Common;

namespace Saritasa.NetForge.Mvvm.ViewModels;

/// <summary>
/// Message about uploading image.
/// </summary>
public record UploadImageMessage
{
    /// <summary>
    /// Contains information about changed images.
    /// </summary>
    public ICollection<ImageDto> ChangedFiles { get; set; } = new List<ImageDto>();
}
