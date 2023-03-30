// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace System.Diagnostics;

internal sealed class UnreachableException : Exception
{
    public UnreachableException(string? message) : base(message)
    {
    }
}
