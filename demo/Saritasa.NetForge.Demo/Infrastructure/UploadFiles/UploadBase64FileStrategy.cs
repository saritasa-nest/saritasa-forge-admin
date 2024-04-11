using Microsoft.AspNetCore.Components.Forms;
using Saritasa.NetForge.Domain;

namespace Saritasa.NetForge.Demo.Infrastructure.UploadFiles;

/// <summary>
/// Strategy when uploaded file converts to base 64 string.
/// </summary>
public class UploadBase64FileStrategy : IUploadFileStrategy
{
    /// <summary>
    /// Converts given file to base 64 string.
    /// </summary>
    /// <param name="file">File to upload.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>File as base 64 string.</returns>
    public async Task<object> UploadFileAsync(IBrowserFile file, CancellationToken cancellationToken)
    {
        const int maxImageSizeInMb = 10;

        var maxImageSize = 1024 * 1024 * maxImageSizeInMb;
        var stream = file.OpenReadStream(maxImageSize, cancellationToken);
        var selectedFileBytes = await GetFileBytesAsync(stream, cancellationToken);

        return $"data:{file.ContentType};base64,{Convert.ToBase64String(selectedFileBytes)}";
    }

    private static async Task<byte[]> GetFileBytesAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }
}