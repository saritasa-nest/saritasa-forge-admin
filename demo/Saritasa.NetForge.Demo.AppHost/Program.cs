using Saritasa.NetForge.Demo.Infrastructure.UploadFiles.S3Storage;

var builder = DistributedApplication.CreateBuilder(args);

const int minioPort = 9000;
var storageSettings = new S3Settings()
{
    RegionName = "eu-central-1",
    BucketName = "saritasa-netforge-demo",
    AccessKey = "minioadmin",
    SecretKey = "minioadmin",
    ServiceUrl = $"http://localhost:{minioPort.ToString()}",
    ForcePathStyle = true
};

var minio = builder.AddContainer("Minio", "minio/minio")
    .WithVolume("minio-volume", target: "/data")
    .WithArgs("server", "/data", "--console-address", ":9001")
    .WithEndpoint(port: 9001, targetPort: 9001, name: "web", scheme: "http")
    .WithEndpoint(port: minioPort, targetPort: minioPort, name: "api", scheme: "http")
    .WithLifetime(ContainerLifetime.Persistent);

var mainWebsite = ConfigureWebProject(
    builder.AddProject<Projects.Saritasa_NetForge_Demo>(name: "netforge-demo-web", launchProfileName: "Run Web"));

builder.Build().Run();

IResourceBuilder<ProjectResource> ConfigureWebProject(IResourceBuilder<ProjectResource> builder)
{
    return builder
        .WithEnvironment("S3Settings__RegionName", storageSettings.RegionName)
        .WithEnvironment("S3Settings__BucketName", storageSettings.BucketName)
        .WithEnvironment("S3Settings__AccessKey", storageSettings.AccessKey)
        .WithEnvironment("S3Settings__SecretKey", storageSettings.SecretKey)
        .WithEnvironment("S3Settings__ServiceUrl", storageSettings.ServiceUrl)
        .WithEnvironment("S3Settings__ForcePathStyle", storageSettings.ForcePathStyle.ToString())
        .WaitFor(minio);
}
