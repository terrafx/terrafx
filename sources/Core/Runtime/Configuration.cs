// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using static TerraFX.Utilities.AppContextUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Runtime;

/// <summary>Provides various configuration switches and values for TerraFX.</summary>
public static class Configuration
{
    /// <summary><c>true</c> if TerraFX should break on a failed assert; otherwise, <c>false</c>.</summary>
    /// <remarks>
    ///     <para>This defaults to <c>true</c> in debug builds of TerraFX; otherwise, it defaults to <c>false</c>.</para>
    ///     <para>Users can enable this via an <see cref="AppContext" /> switch to get additional validation in their own assemblies.</para>
    /// </remarks>
    public static nuint DefaultAlignment { get; } = GetAppContextData(
        $"{typeof(Configuration).FullName}.{nameof(DefaultAlignment)}",
        defaultValue: (nuint)16
    );

    /// <summary><c>true</c> if TerraFX should use <see cref="CultureInfo.InvariantCulture" /> when resolving resources; otherwise, <c>false</c>.</summary>
    /// <remarks>
    ///     <para>This defaults to <c>false</c>.</para>
    ///     <para>Users can enable this via an <see cref="AppContext" /> switch to force invariant strings during resource lookup.</para>
    /// </remarks>
    public static bool InvariantResourceLookup { get; } = GetAppContextData(
        $"{typeof(Configuration).FullName}.{nameof(InvariantResourceLookup)}",
        defaultValue: false
    );

    /// <summary><c>true</c> if TerraFX is current running in a 32-bit process; otherwise, <c>false</c>.</summary>
    public static bool Is32BitProcess { get; } = SizeOf<nuint>() == 4;

    /// <summary><c>true</c> if TerraFX is current running in a 64-bit process; otherwise, <c>false</c>.</summary>
    public static bool Is64BitProcess { get; } = SizeOf<nuint>() == 8;

    /// <summary><c>true</c> if TerraFX was built with the <c>Debug</c> configuration; otherwise, <c>false</c>.</summary>
    /// <remarks>This value is not configurable via an <see cref="AppContext" /> switch.</remarks>
#if DEBUG
    public static bool IsDebug { get; } = true;
#else
    public static bool IsDebug { get; } = false;
#endif

    /// <summary><c>true</c> if TerraFX was built with the <c>Release</c> configuration; otherwise, <c>false</c>.</summary>
    /// <remarks>This value is not configurable via an <see cref="AppContext" /> switch.</remarks>
    public static bool IsRelease { get; } = !IsDebug;

    internal static nuint MaxArrayLength => (uint)Array.MaxLength;
}
