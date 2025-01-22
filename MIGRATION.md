# Migration Guides by Version

## 0.5.0-alpha

Pay attention that namespaces of methods `AddNetForge` and `UseNetForge` were changed. 
So you need to update usings.

### Before

```csharp
using Saritasa.NetForge.Blazor.Extensions;
```

### After

```csharp
using Saritasa.NetForge.Extensions;
```