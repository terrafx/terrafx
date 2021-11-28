// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Graphics;

/// <summary>Defines the settings used for a given memory allocator.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsMemoryAllocatorSettings
{
    /// <summary>The name of the data value that controls the default value of <see cref="IsExternallySynchronized" />.</summary>
    /// <remarks>
    ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
    ///     <para>Setting this data value has no affect on allocators that have already been created or which have an explicit value specified.</para>
    ///     <para>This data value is interpreted as a <c>bool</c>.</para>
    /// </remarks>
    public const string IsExternallySynchronizedDataName = "TerraFX.Graphics.GraphicsMemoryAllocatorSettings." + nameof(IsExternallySynchronized);

    /// <summary>The name of the data value that controls the default value of <see cref="MaximumHeapCountPerCollection" />.</summary>
    /// <remarks>
    ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
    ///     <para>Setting this data value has no affect on allocators that have already been created or which have an explicit value specified.</para>
    ///     <para>This data value is interpreted as an <c>int</c> where negative values are invalid.</para>
    /// </remarks>
    public const string MaximumHeapCountPerCollectionDataName = "TerraFX.Graphics.GraphicsMemoryAllocatorSettings." + nameof(MaximumHeapCountPerCollection);

    /// <summary>The name of the data value that controls the default value of <see cref="MaximumSharedHeapSize" />.</summary>
    /// <remarks>
    ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
    ///     <para>Setting this data value has no affect on allocators that have already been created or which have an explicit value specified.</para>
    ///     <para>This data value is interpreted as a <c>ulong</c>.</para>
    /// </remarks>
    public const string MaximumSharedHeapSizeDataName = "TerraFX.Graphics.GraphicsMemoryAllocatorSettings." + nameof(MaximumSharedHeapSize);

    /// <summary>The name of the data value that controls the default value of <see cref="MinimumHeapCountPerCollection" />.</summary>
    /// <remarks>
    ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
    ///     <para>Setting this data value has no affect on allocators that have already been created or which have an explicit value specified.</para>
    ///     <para>This data value is interpreted as an <c>int</c> where negative values are invalid.</para>
    /// </remarks>
    public const string MinimumHeapCountPerCollectionDataName = "TerraFX.Graphics.GraphicsMemoryAllocatorSettings." + nameof(MinimumHeapCountPerCollection);

    /// <summary>The name of the data value that controls the default value of <see cref="MinimumHeapSize" />.</summary>
    /// <remarks>
    ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
    ///     <para>Setting this data value has no affect on allocators that have already been created or which have an explicit value specified.</para>
    ///     <para>This data value is interpreted as a <c>ulong</c>.</para>
    /// </remarks>
    public const string MinimumHeapSizeDataName = "TerraFX.Graphics.GraphicsMemoryAllocatorSettings." + nameof(MinimumHeapSize);

    /// <summary>The name of the data value that controls the default value of <see cref="MinimumAllocatedRegionMarginSize" />.</summary>
    /// <remarks>
    ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
    ///     <para>Setting this data value has no affect on allocators that have already been created or which have an explicit value specified.</para>
    ///     <para>This data value is interpreted as a <c>ulong</c>.</para>
    /// </remarks>
    public const string MinimumAllocatedRegionMarginSizeDataName = "TerraFX.Graphics.GraphicsMemoryAllocatorSettings." + nameof(MinimumAllocatedRegionMarginSize);

    /// <summary>The name of the data value that controls the default value of <see cref="MinimumFreeRegionSizeToRegister" />.</summary>
    /// <remarks>
    ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
    ///     <para>Setting this data value has no affect on allocators that have already been created or which have an explicit value specified.</para>
    ///     <para>This data value is interpreted as a <c>ulong</c>.</para>
    /// </remarks>
    public const string MinimumFreeRegionSizeToRegisterDataName = "TerraFX.Graphics.GraphicsMemoryAllocatorSettings." + nameof(MinimumFreeRegionSizeToRegister);

    /// <summary>Gets a <c>true</c> if the memory allocator should be externally synchronized; otherwise, <c>false</c>.</summary>
    /// <remarks>This will default to checking the <see cref="IsExternallySynchronizedDataName"/> if <c>null</c>; otherwise, <c>false</c> if no AppContext data has been provided.</remarks>
    public bool? IsExternallySynchronized { get; init; }

    /// <summary>Gets the maximum number of <see cref="GraphicsMemoryHeap" /> allowed per <see cref="GraphicsMemoryHeapCollection" />.</summary>
    /// <remarks>This will default to checking <see cref="MaximumHeapCountPerCollectionDataName"/> if <c>negative</c> or <c>zero</c>; otherwise, <see cref="int.MaxValue" /> if no AppContext data has been provided.</remarks>
    public int MaximumHeapCountPerCollection { get; init; }

    /// <summary>Gets the maximum size of a shared <see cref="GraphicsMemoryHeap" />.</summary>
    /// <remarks>
    ///     <para>Allocation requests with sizes larger than this will be get a dedicated memory heap.</para>
    ///     <para>This will default to checking <see cref="MaximumSharedHeapSizeDataName"/> if <c>null</c>; otherwise, a <c>non-zero</c> system defined value if no AppContext data has been provided.</para>
    ///     <para>A value of zero is valid and will force every allocation to get a dedicated memory heap.</para>
    /// </remarks>
    public ulong? MaximumSharedHeapSize { get; init; }

    /// <summary>Gets the minimum number of <see cref="GraphicsMemoryHeap" /> allowed per <see cref="GraphicsMemoryHeapCollection" />.</summary>
    /// <remarks>This will default to checking <see cref="MaximumHeapCountPerCollectionDataName"/> if <c>negative</c>; otherwise, <c>zero</c> if no AppContext data has been provided.</remarks>
    public int MinimumHeapCountPerCollection { get; init; }

    /// <summary>Gets the minimum size of the free regions that should exist on either side of an allocated region.</summary>
    /// <remarks>This will default to checking <see cref="MinimumAllocatedRegionMarginSizeDataName"/> if <c>null</c>; otherwise, <c>zero</c> if no AppContext data has been provided.</remarks>
    public ulong? MinimumAllocatedRegionMarginSize { get; init; }

    /// <summary>Gets the minimum size of a <see cref="GraphicsMemoryHeap" />.</summary>
    /// <remarks>
    ///     <para>Allocation requests with sizes smaller than this will be increased to at least this size.</para>
    ///     <para>This will default to checking <see cref="MinimumHeapSizeDataName"/> if <c>zero</c>; otherwise, a <c>non-zero</c> system defined value if no AppContext data has been provided.</para>
    /// </remarks>
    public ulong MinimumHeapSize { get; init; }

    /// <summary>Gets the minimum size a free region should be for it to be placed in the collection of available free regions.</summary>
    /// <remarks>This will default to checking <see cref="MinimumFreeRegionSizeToRegisterDataName"/> if <c>zero</c>; otherwise, a <c>non-zero</c> system-defined value if no AppContext data has been provided.</remarks>
    public ulong MinimumFreeRegionSizeToRegister { get; init; }
}
