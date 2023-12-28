// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the List<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="UnmanagedValueList{T}" /> struct.</summary>
public static class UnmanagedValueList
{
    /// <summary>Gets an empty list.</summary>
    public static UnmanagedValueList<T> Empty<T>()
        where T : unmanaged => new UnmanagedValueList<T>();
}
