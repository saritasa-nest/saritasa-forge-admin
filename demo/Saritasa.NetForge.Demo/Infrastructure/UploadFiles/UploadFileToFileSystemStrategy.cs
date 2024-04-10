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
        var stream = file.OpenReadStream(maxImageSize, cancellationToken);
        var selectedFileBytes = ReadFully(stream, stream.Length);

        await fileStream.WriteAsync(selectedFileBytes, cancellationToken);
        return path;
    }

    private static byte[] GetFileBytes(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Reads data from a stream until the end is reached. The
    /// data is returned as a byte array. An IOException is
    /// thrown if any of the underlying IO calls fail.
    /// </summary>
    /// <param name="stream">The stream to read data from</param>
    /// <param name="initialLength">The initial buffer length</param>
    public static byte[] ReadFully (Stream stream, long initialLength)
    {
        // If we've been passed an unhelpful initial length, just
        // use 32K.
        if (initialLength < 1)
        {
            initialLength = 32768;
        }
    
        byte[] buffer = new byte[initialLength];
        int read=0;
    
        int chunk;
        while ( (chunk = stream.Read(buffer, read, buffer.Length-read)) > 0)
        {
            read += chunk;
        
            // If we've reached the end of our buffer, check to see if there's
            // any more information
            if (read == buffer.Length)
            {
                int nextByte = stream.ReadByte();
            
                // End of stream? If so, we're done
                if (nextByte==-1)
                {
                    return buffer;
                }
            
                // Nope. Resize the buffer, put in the byte we've just
                // read, and continue
                byte[] newBuffer = new byte[buffer.Length*2];
                Array.Copy(buffer, newBuffer, buffer.Length);
                newBuffer[read]=(byte)nextByte;
                buffer = newBuffer;
                read++;
            }
        }
        // Buffer is now too big. Shrink it.
        byte[] ret = new byte[read];
        Array.Copy(buffer, ret, read);
        return ret;
    }
}
