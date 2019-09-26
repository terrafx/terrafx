# Versioning

TerraFX follows [Semantic Versioning 2.0.0](https://semver.org/spec/v2.0.0.html).

This means that a given version number is `MAJOR.MINOR.PATCH` where:
 * `MAJOR` is incremented when incompatible API changes are made
 * `MINOR` is incremented when you add functionality in a backwards compatible manner
 * `PATCH` is incremented when a backwards compatible bug fix is made

Preview and nightly builds may contain a `-SUFFIX` following the `PATCH`. `SUFFIX` denotes the development stage for a given package and is currently either `alpha`, `beta`, or `rc`.

Nightly builds may also contain a `.BUILD` following the `-SUFFIX`. `BUILD` denotes the date the build was produced and the build number of the day. The date is UTC based and follows the form `yyyymmdd` to ensure it is monotonically increasing.

## Current Version

The repository has not currently made its first release and is versioned as `0.1.0-alpha`. As such the API is not considered stable and is still in the initial development stages.
