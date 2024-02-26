using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

namespace Saritasa.NetForge.Blazor.Infrastructure.Services;

/// <inheritdoc />
public class FileService : IFileService
{
    private readonly ILogger<FileService> logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    public FileService(ILogger<FileService> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task<byte[]> GetFileBytesAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    /// <inheritdoc />
    public async Task CreateFileAsync(string pathToFile, byte[] fileContent)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(pathToFile)!);

        await using var fileStream = File.Create(pathToFile);
        await fileStream.WriteAsync(fileContent);

        logger.LogInformation("File with path {PathToFile} was created successfully.", pathToFile);
    }
}
