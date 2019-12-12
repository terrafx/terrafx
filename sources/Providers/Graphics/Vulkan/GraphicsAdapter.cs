// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.InteropUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc cref="IGraphicsAdapter" />
    public sealed unsafe class GraphicsAdapter : IGraphicsAdapter
    {
        private readonly GraphicsProvider _graphicsProvider;
        private readonly VkPhysicalDevice _physicalDevice;

        private ValueLazy<VkPhysicalDeviceProperties> _physicalDeviceProperties;
        private ValueLazy<string> _deviceName;

        private State _state;

        internal GraphicsAdapter(GraphicsProvider graphicsProvider, VkPhysicalDevice physicalDevice)
        {
            _graphicsProvider = graphicsProvider;
            _physicalDevice = physicalDevice;

            _physicalDeviceProperties = new ValueLazy<VkPhysicalDeviceProperties>(GetPhysicalDeviceProperties);
            _deviceName = new ValueLazy<string>(GetDeviceName);

            _ = _state.Transition(to: Initialized);
        }

        /// <inheritdoc />
        public uint DeviceId => PhysicalDeviceProperties.deviceID;

        /// <inheritdoc />
        public string DeviceName => _deviceName.Value;

        /// <inheritdoc />
        public IGraphicsProvider GraphicsProvider => _graphicsProvider;

        /// <summary>Gets the underlying <see cref="VkPhysicalDevice" />.</summary>
        /// <exception cref="ObjectDisposedException">The instance has been disposed.</exception>
        public VkPhysicalDevice PhysicalDevice
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _physicalDevice;
            }
        }

        /// <summary>Gets the <see cref="VkPhysicalDeviceProperties" /> for <see cref="PhysicalDevice" />.</summary>
        /// <exception cref="ObjectDisposedException">The instance has been disposed and the value was not otherwise cached.</exception>
        public ref readonly VkPhysicalDeviceProperties PhysicalDeviceProperties => ref _physicalDeviceProperties.RefValue;

        /// <inheritdoc />
        public uint VendorId => PhysicalDeviceProperties.vendorID;

        /// <inheritdoc />
        public IGraphicsContext CreateGraphicsContext(IGraphicsSurface graphicsSurface)
        {
            _state.ThrowIfDisposedOrDisposing();
            ThrowIfNull(graphicsSurface, nameof(graphicsSurface));
            return new GraphicsContext(this, graphicsSurface);
        }

        /// <inheritdoc />
        /// <remarks>While there are no unmanaged resources to cleanup, we still want to mark the instance as disposed if the <see cref="GraphicsProvider" /> was disposed or if the adapter no longer exists.</remarks>
        public void Dispose()
        {
            _ = _state.BeginDispose();
            _state.EndDispose();
        }

        private VkPhysicalDeviceProperties GetPhysicalDeviceProperties()
        {
            _state.ThrowIfDisposedOrDisposing();

            VkPhysicalDeviceProperties physicalDeviceProperties;
            vkGetPhysicalDeviceProperties(PhysicalDevice, &physicalDeviceProperties);
            return physicalDeviceProperties;
        }

        private string GetDeviceName() => MarshalUtf8ToReadOnlySpan(in PhysicalDeviceProperties.deviceName[0], 256).AsString() ?? string.Empty;
    }
}
