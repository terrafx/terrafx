// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;
using TerraFX.Graphics;

namespace TerraFX.UnitTests.Graphics;

/// <summary>Provides a set of tests covering headless device-layer creation on a software (WARP) adapter.</summary>
/// <remarks>
///     These tests do not require a physical GPU. They select the software adapter (WARP on Windows) via
///     <see cref="GraphicsAdapter.IsSoftware" /> so they can run on GPU-less CI runners. If no software adapter
///     is available, the test is ignored rather than failed.
/// </remarks>
internal static class GraphicsDeviceTests
{
    private static GraphicsService CreateService()
    {
        if (!OperatingSystem.IsWindows())
        {
            Assert.Ignore("Graphics device tests require Windows.");
        }
        return GraphicsService.Create();
    }

    private static GraphicsAdapter GetSoftwareAdapter(GraphicsService service)
    {
        foreach (var adapter in service.Adapters)
        {
            if (adapter.IsSoftware)
            {
                return adapter;
            }
        }

        Assert.Ignore("No software (WARP) adapter is available on this machine.");
        throw new InvalidOperationException("unreachable");
    }

    /// <summary>Provides validation that a software adapter is enumerated and correctly reports <see cref="GraphicsAdapter.IsSoftware" />.</summary>
    [Test]
    public static void AdaptersIncludeSoftwareAdapterTest()
    {
        using var service = CreateService();

        var adapterCount = 0;
        var softwareAdapterCount = 0;

        foreach (var adapter in service.Adapters)
        {
            adapterCount++;

            if (adapter.IsSoftware)
            {
                softwareAdapterCount++;
                Assert.That(adapter.Description, Is.Not.Null.And.Not.Empty);
                Assert.That(adapter.Service, Is.SameAs(service));
            }
        }

        Assert.That(adapterCount, Is.GreaterThan(0));

        if (softwareAdapterCount == 0)
        {
            Assert.Ignore("No software (WARP) adapter is available on this machine.");
        }
    }

    /// <summary>Provides validation that <see cref="GraphicsAdapter.CreateDevice()" /> succeeds headlessly on a software adapter.</summary>
    [Test]
    public static void CreateDeviceOnSoftwareAdapterTest()
    {
        using var service = CreateService();
        var adapter = GetSoftwareAdapter(service);

        using var device = adapter.CreateDevice();

        Assert.That(device, Is.Not.Null);
        Assert.That(device.Adapter, Is.SameAs(adapter));
        Assert.That(device.Service, Is.SameAs(service));
        Assert.That(device.RenderCommandQueue, Is.Not.Null);
        Assert.That(device.ComputeCommandQueue, Is.Not.Null);
        Assert.That(device.CopyCommandQueue, Is.Not.Null);
    }

    /// <summary>Provides validation that a buffer can be allocated through the device, exercising the graphics memory manager end-to-end on a software adapter.</summary>
    [Test]
    public static void CreateBufferOnSoftwareDeviceTest()
    {
        using var service = CreateService();
        var adapter = GetSoftwareAdapter(service);

        using var device = adapter.CreateDevice();

        var byteLength = (nuint)(64 * 1024);
        using var buffer = device.CreateUploadBuffer(byteLength);

        Assert.That(buffer, Is.Not.Null);
        Assert.That(buffer.Device, Is.SameAs(device));
        Assert.That(buffer.ByteLength, Is.GreaterThanOrEqualTo(byteLength));
    }
}
