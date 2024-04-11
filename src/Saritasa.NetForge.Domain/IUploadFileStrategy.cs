using Microsoft.AspNetCore.Components.Forms;

namespace Saritasa.NetForge.Domain;

/// <summary>
/// Upload file strategy.
/// </summary>
public interface IUploadFileStrategy
{
    /// <summary>
    /// Uploads file.
    /// </summary>
    /// <param name="file">File to upload.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>File related object.</returns>
    Task<object> UploadFileAsync(IBrowserFile file, CancellationToken cancellationToken);
}
