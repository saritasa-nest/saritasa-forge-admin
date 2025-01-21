# Migration Guides by Version

## 0.5.0-alpha

Pay attention that namespaces of methods used in `AddNetForge` method were changed. 
So you need to update such namespaces.

### Before

```csharp
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;
```

### After

```csharp
using Saritasa.NetForge.Blazor.Infrastructure.EfCore.Extensions;
```