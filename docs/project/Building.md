# Building the repository

The repository aims to be simple to configure, build, and produce binaries. To achieve this it pulls in all dependencies as NuGet packages and provides a set of build scripts to assist in performing common tasks such as restoring dependencies, building, testing, and produce NuGet packages.

These scripts are:
 * [restore.cmd](../../restore.cmd) and [restore.sh](../../restore.sh)
 * [build.cmd](../../build.cmd) and [build.sh](../../build.sh)
 * [test.cmd](../../test.cmd) and [test.sh](../../test.sh)
 * [pack.cmd](../../pack.cmd) and [pack.sh](../../pack.sh)

Each script only performs the designated build step and expects that the .NET SDK is already on the box. This means that after a clean enlistment, you must run both the `restore` and `build` scripts (in that order).

In addition there are [scripts/cibuild.cmd](../../scripts/cibuild.cmd) and [scripts/cibuild.sh](../../scripts/cibuild.sh) scripts which allow you to reproduce everything the CI environment does. This includes automatically acquiring a known good .NET SDK and running each step in the correct sequence (first restore, then build, then test, then pack).

## Requirements

.NET 5.0 SDK: https://dotnet.microsoft.com/download/dotnet/5.0

## Available Arguments

All scripts forward to a central build script: [scripts/build.ps1](../../scripts/build.ps1) or [scripts/build.sh](../../scripts/build.sh) specifying a single argument matching the name of the script and additionally forwards any user specified arguments given.

This means that any given script can run any of the total steps required. For example, `build.cmd` by default just specifies the `-build` command.  But if you specified `build.cmd -restore`, it would also run the `restore` command. The scripts are aware of command ordering requirements and will run them in the correct order (e.g. `restore` will always run before `build`).

The arguments take a single dash (`-`) as the prefix.

The available arguments can be seen via the `help` command, but are additionally listed below for reference:
```
Common settings:
  -configuration <value>  Build configuration (Debug, Release)
  -verbosity <value>      Msbuild verbosity (q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic])
  -help                   Print help and exit

Actions:
  -restore                Restore dependencies
  -build                  Build solution
  -test                   Run all tests in the solution
  -pack                   Package build artifacts

Advanced settings:
  -solution <value>       Path to solution to build
  -ci                     Set when running on CI server
  -architecture <value>   Test Architecture (<auto>, amd64, x64, x86, arm64, arm)

Command line arguments not listed above are passed through to MSBuild.
The above arguments can be shortened as much as to be unambiguous (e.g. -co for configuration, -t for test, etc.).
```

## Utilizing `dotnet`

Given that the above build scripts all utilize `dotnet` behind the scenes, you can also utilize the same for working with the repository.

That is, all of the existing well known commands "just work". This includes, but is not limited to:
 * `dotnet restore`
 * `dotnet build`
 * `dotnet test`
 * `dotnet pack`
