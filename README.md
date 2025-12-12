# Cloudflare.LeakedCredentials

[![CI](https://github.com/brandonhenricks/cloudflare-leakedcredentials/actions/workflows/ci.yml/badge.svg)](https://github.com/brandonhenricks/cloudflare-leakedcredentials/actions/workflows/ci.yml)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Cloudflare.LeakedCredentials.svg)](https://www.nuget.org/packages/Cloudflare.LeakedCredentials)

A minimal [netstandard2.0](https://learn.microsoft.com/dotnet/standard/net-standard) library that parses Cloudflare's `Exposed-Credential-Check` response header so origin servers can detect credential exposure without pulling in framework dependencies.

## Purpose
1. Keep framework dependencies out of the security signal by exposing only a single, functional entry point (`ExposedCredentialCheck.Parse`).
2. Require callers to provide a `Func<string, string?>` header accessor so the library works with ASP.NET, HttpClient, middleware pipelines, or any custom HTTP stack.

## Contents
- [Purpose](#purpose)
- [Getting started](#getting-started)
- [Usage](#usage)
- [API contract](#api-contract)
- [Best practices](#best-practices)
- [Testing & validation](#testing--validation)
- [NuGet package](#nuget-package)
- [Contributing](#contributing)
- [License](#license)

## Getting started
1. Consume the published NuGet package using `dotnet add package Cloudflare.LeakedCredentials` or build from source with `dotnet build Cloudflare.LeakedCredentials.sln` (netstandard2.0 only).
2. Provide a header getter delegate when calling `Parse` so you retain control of how headers are fetched.

```csharp
var result = ExposedCredentialCheck.Parse(context.Request.Headers.Get);
if (result == ExposedCredentialCheckResult.Exposed)
{
		// trigger additional logging, alerting, or rejection logic
}
```

## API contract
- `ExposedCredentialCheck.Parse(Func<string, string?> getHeader)`
	- Emits `ExposedCredentialCheckResult.None` for missing/null/empty headers.
	- Uses `Enum.IsDefined()` plus a positive value check so only known exposure states are surfaced.
	- Throws `ArgumentNullException` if the delegate is null to keep contracts strict.

## Best practices
- Keep header access centralized inside middleware, filters, or helpers before passing a delegate into this library; it simplifies testing and throttling.
- Treat every result that is not `None` as actionable—`PotentiallyExposed` still warrants monitoring or rate limiting.
- Wrap log and metric instrumentation around the exposure result so it can feed dashboards without leaking secrets.
- Preserve netstandard2.0 compatibility: avoid framework-specific APIs in wrappers so this package stays portable.
- Respect null-safety guarantees by never calling `Parse` with a null delegate and by validating header values before acting on them.

## Testing & validation
- Keep builds fast with `dotnet build Cloudflare.LeakedCredentials.sln`.
- The tests live under `tests/Cloudflare.LeakedCredentials.Tests/`; add coverage-sensitive scenarios there and expose internals via `InternalsVisibleTo` when needed.
- Use `dotnet test` in CI to exercise the `ExposedCredentialCheck` parsing logic across all states.

## NuGet package
- The repository already publishes the `Cloudflare.LeakedCredentials` NuGet package targeting netstandard2.0.
- Consume it with `dotnet add package Cloudflare.LeakedCredentials` and keep your package reference updated from nuget.org.
- Package publishing follows the standard `dotnet pack` / `dotnet nuget push` flow and should only be run by authorized maintainers.
## Resiliency notes
- The library keeps the enum validation strict (using `Enum.IsDefined` and positive value checks) so unknown headers fall back to `None` rather than failing the request.
- Header parsing is intentionally synchronous and stateless—provide thread-safe delegates when sampling headers from shared contexts.

## Next steps
- Run `dotnet test Cloudflare.LeakedCredentials.sln` locally and in CI to validate the parsing coverage you just added.
- When bumping the package, update `Directory.Build.props` metadata and align `Directory.Packages.props` versions with any new dependencies.
- Draft release notes that explain which Cloudflare response states are surfaced so downstream teams can react appropriately.

## Contributing
- Open issues for bug reports or feature ideas.
- Keep future API changes backward-compatible with `ExposedCredentialCheck.Parse`.

## License
See the [LICENSE](LICENSE) file for details.
