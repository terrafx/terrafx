// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Globalization;
using System.Resources;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Runtime;

/// <summary>Provides a set of methods for looking up localized resources.</summary>
public static partial class Resources
{
    /// <summary>Gets the <see cref="ResourceManager" /> instance that is used to lookup the localized resources.</summary>
    public static ResourceManager ResourceManager { get; } = new ResourceManager(typeof(Resources));

    private static CultureInfo? s_culture;

    /// <summary>Gets or sets the <see cref="CultureInfo" /> used during resource lookup.</summary>
    /// <remarks>
    ///     <para>When this property has a value of <c>null</c>, <see cref="CultureInfo.CurrentUICulture" /> is used instead.</para>
    ///     <para>When <see cref="InvariantResourceLookup" /> is <c>true</c>; this property will always return <see cref="CultureInfo.InvariantCulture" /> and attempting to set it may assert.</para>
    /// </remarks>
    public static CultureInfo? Culture
    {
        get
        {
            return InvariantResourceLookup ? CultureInfo.InvariantCulture : s_culture;
        }

        set
        {
            Assert(!InvariantResourceLookup);
            s_culture = value;
        }
    }

    /// <summary>Gets the resource string associated with a given name.</summary>
    /// <param name="name">The name of the resource to get.</param>
    /// <returns>The resource string associated with <paramref name="name" /> or <see cref="string.Empty" /> if none exists.</returns>
    public static string GetString(string name)
        => ResourceManager.GetString(name, Culture) ?? string.Empty;
}
