// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace System.Runtime.CompilerServices
{
    /// <summary>Defines an attribute which specifies the attached parameter should be considered not null when the method returns.</summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class EnsuresNotNullAttribute : Attribute
    {
    }
}
