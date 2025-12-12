# Copilot Instructions for cloudflare-leakedcredentials

## Project Overview
A minimal C# library that parses Cloudflare's `Exposed-Credential-Check` HTTP response header to detect credential exposure in data breaches. Part of Cloudflare's bot management and security features.

## Architecture

**Core Components:**
- `ExposedCredentialCheck.cs` - Static parser class with single entry point `Parse(Func<string, string?> getHeader)`
- `ExposedCredentialCheckResult.cs` - Enum representing credential check states (None, NoExposure, Exposed, PotentiallyExposed, Error)

**Design Pattern:**
- **Functional header injection**: The `Parse()` method accepts a header retrieval function rather than direct header objects. This decouples from any specific HTTP framework (ASP.NET, HttpClient, custom middleware, etc.)
- Example: `ExposedCredentialCheck.Parse(context.Request.Headers.Get)` or custom implementations

**Key Integration Point:**
The library is framework-agnostic by design—consumers provide a `Func<string, string?>` that retrieves headers from their HTTP context, making it compatible with any C# HTTP library or middleware.

## Build & Development Workflow

**Build:**
```
dotnet build Cloudflare.LeakedCredentials.sln
```

**Configuration:**
- **Target Framework:** `netstandard2.0` (compatible with .NET Framework 4.6.1+ and all .NET Core versions)
- **Language Version:** C# 8.0
- **Nullable:** Enabled (strict null safety)

**Important:** The project currently targets netstandard2.0, making it maximally portable. Maintain this when adding features.

## Code Patterns & Conventions

1. **Null-safety:**
   - Guard clauses on public API parameters (see `ArgumentNullException` in Parse method)
   - Use nullable reference types (`Nullable: enable`) throughout

2. **Header Parsing Logic:**
   - Returns enum default (`None`) for missing/null/empty headers
   - Validates parsed integer against enum via `Enum.IsDefined()` with value > 0
   - Only defined enum values > 0 are accepted to prevent accepting 0 (None) as explicit result

3. **XML Documentation:**
   - All public members must have `<summary>`, `<param>`, `<returns>`, and `<exception>` tags
   - Comprehensive for API clarity (consumers are likely middleware implementers)

4. **Namespace:**
   - Root namespace: `Cloudflare.LeakedCredentials.Origin`
   - "Origin" indicates this represents Cloudflare origin-server context

## Testing Notes
Currently no test project in solution structure. If adding tests, create `src/Cloudflare.LeakedCredentials.Tests/` following the folder convention and use `InternalsVisibleTo` in the main project (pattern already exists in .csproj).

## When Making Changes
- Preserve netstandard2.0 compatibility—avoid newer TFMs unless expanding scope
- Keep the functional injection pattern for `Parse()` (don't refactor to require specific HTTP libraries)
- Maintain comprehensive XML docs for all public members
- Validate enum parsing logic when adding new result states

## External Dependencies
None (no NuGet packages)—intentionally minimal for maximum compatibility and zero friction on adoption.
