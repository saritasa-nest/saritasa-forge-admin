using Microsoft.AspNetCore.Components.Forms;
using Saritasa.NetForge.Domain;
using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.Demo.Infrastructure.UploadFiles;

/// <summary>
/// Strategy to upload file to file system.
/// </summary>
public class UploadFileToFileSystemStrategy : IUploadFileStrategy
{
    private readonly AdminOptions adminOptions;

    /// <summary>
    /// Constructor.
    /// </summary>
    public UploadFileToFileSystemStrategy(AdminOptions adminOptions)
    {
        this.adminOptions = adminOptions;
    }

    /// <summary>
    /// Uploads file to file system.
    /// </summary>
    /// <param name="file">File to upload.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Relative path to uploaded file.</returns>
    public async Task<string> UploadFileAsync(IBrowserFile file, CancellationToken cancellationToken)
    {
        var path = Path.Combine(adminOptions.MediaFolder, file.Name);
        var pathToCreate = Path.Combine(adminOptions.StaticFilesFolder, path);
        Directory.CreateDirectory(Path.GetDirectoryName(pathToCreate)!);

        await using var fileStream = File.Create(pathToCreate);
        var maxImageSize = 1024 * 1024 * adminOptions.MaxImageSizeInMb;
        await file.OpenReadStream(maxImageSize, cancellationToken).CopyToAsync(fileStream, cancellationToken);

        return path;
    }
}
