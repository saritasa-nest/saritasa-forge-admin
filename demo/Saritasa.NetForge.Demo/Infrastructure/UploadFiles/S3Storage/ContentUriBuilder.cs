using MimeTypes;

namespace Saritasa.NetForge.Demo.Infrastructure.UploadFiles.S3Storage;

/// <summary>
/// Contains methods for creating content URI for files to upload them.
/// </summary>
public static class ContentUriBuilder
{
    private const string DefaultFileExtension = ".bin";

    /// <summary>
    /// Create a content URI for a file to upload it.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="contentType">Content type.</param>
    /// <returns>Content URI.</returns>
    public static string Build(string fileName, string contentType)
    {
        var fileExtension = MimeTypeMap.GetExtension(contentType, false);
        if (string.IsNullOrEmpty(fileExtension))
        {
            fileExtension = DefaultFileExtension;
        }

        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName).ToLower();
        return $"{fileNameWithoutExtension}_{Guid.NewGuid():N}{fileExtension}";
    }
}
