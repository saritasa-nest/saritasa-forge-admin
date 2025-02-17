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