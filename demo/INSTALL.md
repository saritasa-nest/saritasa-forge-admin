## Required software

- Visual Studio 2026 (https://www.visualstudio.com/downloads/download-visual-studio-vs.aspx) or JetBrains Rider
- .NET SDK 10 (https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- git

## Simple run

It is recommended to launch the `Saritasa.NetForge.Demo.AppHost` project which automatically sets everything up for you including minio, sqlite database, and demo project.

## Project initialization

1. Copy `demo\Saritasa.NetForge.Demo\appsettings.json.template` file to `demo\Saritasa.NetForge.Demo\appsettings.Development.json`

2. Update the `ConnectionStrings:AppDatabase` setting in that file to target your local development server/database

3. Update the `S3Settings`. For local development use `docker compose` with `src\demo\docker-compose.yaml` file to run S3 service container. For example, you can do it inside [WSL](https://dev.solita.fi/2021/12/21/docker-on-wsl2-without-docker-desktop.html). `AccessKey` and `SecretKey` you can find in `Minio` at the [address](http://127.0.0.1:9001/). Login and password you can find in `src\demo\docker-compose.yaml`. `ServiceUrl` is http://127.0.0.1:9000 and `ForcePathStyle` is `true`.