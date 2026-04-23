# Migration Guides by Version

This file contains the way to handle breaking changes and how to migrate to the new version.

## 1.0.0

- Pay attention that namespaces of methods `AddNetForge` and `UseNetForge` were changed. 
So you need to update usings.

### Before

```csharp
using Saritasa.NetForge.Blazor.Extensions;
```

### After

```csharp
using Saritasa.NetForge.Extensions;
```

- Use `IncludeAllEntities` instead of `ExcludeAllEntities`.

## 1.1.0

If a project that uses NetForge does not have any .razor file, then you need to add `<RequiresAspNetWebAssets>true</RequiresAspNetWebAssets>` to your .csproj file.
See details [here](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-10.0?view=aspnetcore-9.0#blazor-script-as-static-web-asset).

```csharp
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
  <RequiresAspNetWebAssets>true</RequiresAspNetWebAssets>
</PropertyGroup>
```