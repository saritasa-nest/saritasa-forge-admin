namespace Saritasa.NetForge.Demo.Infrastructure.UploadFiles.S3Storage;

/// <summary>
/// Settings for S3 service.
/// </summary>
public record S3Settings
{
    /// <summary>
    /// Region name.
    /// </summary>
    public required string RegionName { get; init; }

    /// <summary>
    /// Bucket name.
    /// </summary>
    public required string BucketName { get; init; }

    /// <summary>
    /// Access key. Required when using Minio.
    /// </summary>
    public string AccessKey { get; init; } = string.Empty;

    /// <summary>
    /// Secret key. Required when using Minio.
    /// </summary>
    public string SecretKey { get; init; } = string.Empty;

    /// <summary>
    /// Specifies the endpoint to access with the client.
    /// Required when using Minio.
    /// </summary>
    public string ServiceUrl { get; init; } = string.Empty;

    /// <summary>
    /// Specifies that requests will always use path style addressing.
    /// Required true when using Minio.
    /// </summary>
    public bool ForcePathStyle { get; init; }
}

