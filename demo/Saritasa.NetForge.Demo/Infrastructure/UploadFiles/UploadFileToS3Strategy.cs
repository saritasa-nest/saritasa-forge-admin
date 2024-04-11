using Microsoft.AspNetCore.Components.Forms;
using Saritasa.NetForge.Domain;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

namespace Saritasa.NetForge.Demo.Infrastructure.UploadFiles;

/// <summary>
/// Strategy to upload file to S3.
/// </summary>
public class UploadFileToS3Strategy : IUploadFileStrategy
{
    private readonly IBlobStorageService blobStorageService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public UploadFileToS3Strategy(IBlobStorageService blobStorageService)
    {
        this.blobStorageService = blobStorageService;
    }

    /// <summary>
    /// Uploads file to S3.
    /// </summary>
    /// <param name="file">File to upload.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>File URI.</returns>
    public async Task<object> UploadFileAsync(IBrowserFile file, CancellationToken cancellationToken)
    {
        var fileUri = ContentUriBuilder.Build(file.Name, file.ContentType);
        var fileContent = file.OpenReadStream(cancellationToken: cancellationToken);
        await blobStorageService.UploadAsync(fileUri, fileContent, cancellationToken);

        return fileUri;
    }
}
