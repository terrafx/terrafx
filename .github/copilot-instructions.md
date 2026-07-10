# Copilot instructions for TerraFX

TerraFX is a framework for developing multimedia-based applications. This file
gives an agent the context needed to build, test, and modify the repo correctly
on the first try. Keep it accurate — if a build/layout detail here becomes wrong,
fix it as part of your change.

## Layout

- `sources/` — shipping libraries (`Core` → `TerraFX.dll`, plus `Graphics`, `UI`,
  `ApplicationModel`, `WinForms`). `Core` is the bulk of the code (numerics,
  collections, unmanaged spans/arrays, threading, utilities).
- `tests/Core/` — NUnit tests (`TerraFX.UnitTests`). Only `Collections`,
  `Numerics`, `Runtime`, and `Utilities` are currently covered.
- `samples/` — runnable samples (`Exe`, not packable).
- `docs/` — design/project docs. `scripts/` — the real build logic (`build.ps1`,
  `cibuild.cmd`). Root `*.cmd`/`*.sh` are thin wrappers.
- Build config lives in `Directory.Build.props`/`.targets` (root + `sources/`),
  `Directory.Packages.props` (central package versions, CPM), and `.editorconfig`
  (516 lines, the source of truth for style/naming).

## Building

- SDK: driven by `global.json` (`10.0.100`, `allowPrerelease`,
  `rollForward: latestFeature`). Target framework is `net10.0`
  (`net10.0-windows` for `WinForms`).
- This is a **Windows-only** codebase (`SupportedOSPlatform` =
  `windows10.0.17763.0`, hard dependency on `TerraFX.Interop.Windows`).
- Each step script does exactly one step and they order themselves:
  - `restore.cmd`, `build.cmd`, `test.cmd`, `pack.cmd`.
  - `build.cmd` **already implies `-build`** — do NOT pass `-build` again, it
    errors with "parameter 'build' is specified more than once". To restore then
    build, run `build.cmd -restore`. Pass `-configuration Release|Debug`.
  - `scripts/cibuild.cmd` reproduces CI (acquires SDK, then restore→build→test→pack).
- Plain `dotnet restore|build|test|pack` on `TerraFX.slnx` also works.

## Non-negotiable build settings (they make sloppy changes fail the build)

- `TreatWarningsAsErrors=true`, `AnalysisLevel=latest-all`,
  `EnforceCodeStyleInBuild=true`, `Nullable=enable`, `Features=strict`,
  `GenerateDocumentationFile=true`. A missing XML doc, a nullable violation, or
  an analyzer/style diagnostic (`CAxxxx`/`IDExxxx`) is a hard error, not a warning.
- `IsAotCompatible=true` and libraries are `IsTrimmable=true` with the trim
  analyzer on — keep new library code AOT/trim-safe (no unbounded reflection).
- `AllowUnsafeBlocks=true`; `unsafe`/pointers are normal here. Debug builds set
  `CheckForOverflowUnderflow=true`, so mark intentional wrapping with `unchecked`.

## Conventions (enforced at `error` severity via `.editorconfig`)

- Every `.cs` file starts with the standard copyright header (auto-inserted;
  don't hand-write a different one).
- Naming: `_camelCase` instance fields, `s_camelCase` static fields, `PascalCase`
  public/protected/const, `camelCase` locals/params, `I`-prefixed interfaces,
  `T`-prefixed type parameters, `Async` suffix on async methods. Use `nameof`.
- One public type per file. Generic types put arity in the filename with a
  backtick (`UnmanagedArray`1.cs`); partials split by concern into sibling files
  (`.DebugView.cs`, `.Enumerator.cs`, `.Metadata.cs`).
- Public members need `///` XML docs (build fails otherwise). Match the doc phrasing
  of neighboring members; **watch for copy-paste doc/param mistakes** — several
  already exist in the tree.
- Reuse the shared helpers instead of re-rolling logic: `ExceptionUtilities`
  (`Throw*`, `ThrowIf*`, `ThrowFor*`), `AssertionUtilities` (`Assert*`),
  `VectorUtilities`/`UnsafeUtilities` for SIMD/pointer work.
- Numerics types are `readonly struct`s wrapping `Vector128<float>` and interop
  with `System.Numerics` via `Unsafe.BitCast`/`As<,>` (see `Numerics/Quaternion.cs`).
  Keep SIMD math written inline; don't hoist shared sub-expressions.
- Equality: `!=` operators delegate to `!(left == right)`. Structs implement the full
  `IEquatable`/`IComparable`/`IFormattable`/`ISpanFormattable`/`IUtf8SpanFormattable`
  set where relevant; UTF-8 formatting overloads name their span `utf8Destination`.

## Tests

- NUnit 4 via `Microsoft.NET.Test.Sdk` + `NUnit3TestAdapter`. Run with
  `test.cmd -configuration Debug|Release` (or `dotnet test`). Results (`trx`) land in
  `artifacts/tst/`.
- **Run both `Debug` and `Release` locally, and run `Debug` first.** CI runs both,
  and `Debug` intentionally catches more: it enables `CheckForOverflowUnderflow=true`
  (so intentional integer wrapping must be marked `unchecked` or it throws) and the
  `[Conditional("DEBUG")]` asserts (`AssertionUtilities.Assert*`), both of which are
  no-ops under `Release`. A suite that is green in `Release` can still surface real
  bugs in `Debug`, so don't consider a change validated until `Debug` is green too.
- When you fix a bug in `Core`, add/extend a regression test under the matching
  `tests/Core/<Area>/` folder following the existing `*Tests.cs` patterns.

## Gotchas

- `AnalysisLevel` is `latest-all`, not `preview-all` — keep it that way so
  preview-only analyzers don't turn into build-breaking errors under
  warnings-as-errors.
- XML docs are copy-pasted between similar members — double-check `<summary>`/
  `<param>` text actually describes the member you're editing.
- XML docs are copy-pasted between similar members — double-check `<summary>`/
  `<param>` text actually describes the member you're editing.
