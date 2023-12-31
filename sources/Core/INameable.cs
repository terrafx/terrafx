// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TerraFX;

/// <summary>Defines an object which can have a name.</summary>
public interface INameable
{
    /// <summary>Gets or sets the name of the object.</summary>
    /// <remarks>An input of <c>null</c> is treated as <see cref="MemberInfo.Name" /></remarks>
    [AllowNull]
    string Name { get; set; }
}
