# AGENTS.md

## Repository overview
`Saritasa.NetForge` is a reusable ASP.NET Core Blazor admin-panel library (not a standalone app).  
Main integration points for consumers are `AddNetForge(...)` and `UseNetForge(...)`.

## Project layout (high-value paths)
- `src/Saritasa.NetForge/` - main library code.
- `src/Saritasa.NetForge.Tests/` - xUnit tests for library behavior.
- `src/Saritasa.NetForge.slnx` - primary solution used by CI.
- `demo/Saritasa.NetForge.Demo/` - demo host app (net10.0) used for manual validation.
- `docs/` - focused behavior docs (`SEARCH.md`, `NAVIGATIONS.md`).
- `.github/workflows/` - CI source of truth for validation and packaging commands.
- `tmp/` - transient publish/build output; do not treat as source.

## Prerequisites
- .NET SDKs 8, 9, and 10 installed (`INSTALL.md`).

## Verified build/test/package commands (from repo root)
```powershell
dotnet restore src\Saritasa.NetForge.slnx
dotnet build src\Saritasa.NetForge.slnx -nologo
dotnet test src -nologo
```

## Completion checks
Before considering work complete:
1. `dotnet build src\Saritasa.NetForge.slnx -nologo` succeeds.
2. `dotnet test src -nologo` passes for all target frameworks.

## Workflow and safety constraints
- Keep changes scoped to source/docs; ignore `bin/`, `obj/`, and `tmp/` outputs.
- Do not commit environment-specific config (for example `appsettings.Development.json`).
- Respect multi-targeting (`net8.0;net9.0;net10.0`) in library and tests.
- Analyzer baseline is defined in `src/Directory.Build.props` (StyleCop + Saritasa analyzers).
- For behavior-level changes, update/add tests under `src/Saritasa.NetForge.Tests/`.

## Nested AGENTS.md
No nested `AGENTS.md` files exist currently.  
If demo-specific workflows diverge further, add `demo/AGENTS.md` for local-only demo guidance.
