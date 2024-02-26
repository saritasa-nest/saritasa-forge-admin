namespace Saritasa.NetForge.Blazor.Infrastructure.Services;

/// <summary>
/// Service to make operations on files.
/// </summary>
public class FileService
{
    private readonly ILogger<FileService> logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    public FileService(ILogger<FileService> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Get file content as array of bytes.
    /// </summary>
    /// <param name="stream">Stream.</param>
    /// <returns>Content of the file.</returns>
    public async Task<byte[]> GetFileBytesAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Creates file.
    /// </summary>
    /// <param name="pathToFile">Path to file to create.</param>
    /// <param name="fileContent">Content to write to the new file.</param>
    public async Task CreateFileAsync(string pathToFile, byte[] fileContent)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(pathToFile)!);

        await using var fileStream = File.Create(pathToFile);
        await fileStream.WriteAsync(fileContent);

        logger.LogInformation("File with path {PathToFile} was created successfully.", pathToFile);
    }
}
