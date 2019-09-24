// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.VkResult;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Vulkan.Graphics
{
    /// <summary>Provides access to a Vulkan based graphics subsystem.</summary>
    [Export(typeof(IGraphicsProvider))]
    [Export(typeof(GraphicsProvider))]
    [Shared]
    public sealed unsafe class GraphicsProvider : IDisposable, IGraphicsProvider
    {
        private ResettableLazy<ImmutableArray<GraphicsAdapter>> _adapters;
        private ResettableLazy<IntPtr> _instance;

        private State _state;

        /// <summary>Initializes a new instance of the <see cref="GraphicsProvider" /> class.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateInstance(VkInstanceCreateInfo*, VkAllocationCallbacks*, IntPtr*)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="vkEnumeratePhysicalDevices(IntPtr, uint*, IntPtr*)" /> failed.</exception>
        [ImportingConstructor]
        public GraphicsProvider()
        {
            _adapters = new ResettableLazy<ImmutableArray<GraphicsAdapter>>(GetGraphicsAdapters);
            _instance = new ResettableLazy<IntPtr>(CreateInstance);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="GraphicsProvider" /> class.</summary>
        ~GraphicsProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="IGraphicsAdapter" /> instances currently available.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IEnumerable<IGraphicsAdapter> GraphicsAdapters
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _adapters.Value;
            }
        }

        /// <summary>Gets the <c>vkInstance</c> for the current instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr Instance
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _instance.Value;
            }
        }

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        private static IntPtr CreateInstance()
        {
            var enabledExtensionCount = 2u;

            var enabledExtensionNames = stackalloc sbyte*[(int)enabledExtensionCount];
            enabledExtensionNames[0] = (sbyte*)Unsafe.AsPointer(ref Unsafe.AsRef(in VK_KHR_SURFACE_EXTENSION_NAME[0]));

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                enabledExtensionNames[1] = (sbyte*)Unsafe.AsPointer(ref Unsafe.AsRef(in VK_KHR_WIN32_SURFACE_EXTENSION_NAME[0]));
            }
            else
            {
                enabledExtensionNames[1] = (sbyte*)Unsafe.AsPointer(ref Unsafe.AsRef(in VK_KHR_XLIB_SURFACE_EXTENSION_NAME[0]));
            }

            var createInfo = new VkInstanceCreateInfo {
                sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
                pNext = null,
                flags = 0,
                pApplicationInfo = null,
                enabledLayerCount = 0,
                ppEnabledLayerNames = null,
                enabledExtensionCount = enabledExtensionCount,
                ppEnabledExtensionNames = enabledExtensionNames,
            };

            IntPtr instance;
            var result = vkCreateInstance(&createInfo, null, &instance);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkCreateInstance), (int)result);
            }

            return instance;
        }

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeInstance();
            }

            _state.EndDispose();
        }

        private void DisposeInstance()
        {
            _state.AssertDisposing();

            if (_instance.IsCreated)
            {
                vkDestroyInstance(_instance.Value, null);
            }
        }

        private ImmutableArray<GraphicsAdapter> GetGraphicsAdapters()
        {
            var instance = _instance.Value;

            var physicalDeviceCount = 0u;
            var result = vkEnumeratePhysicalDevices(instance, &physicalDeviceCount, null);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkEnumeratePhysicalDevices), (int)result);
            }

            var physicalDevices = stackalloc IntPtr[unchecked((int)physicalDeviceCount)];
            result = vkEnumeratePhysicalDevices(instance, &physicalDeviceCount, physicalDevices);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkEnumeratePhysicalDevices), (int)result);
            }

            var adapters = ImmutableArray.CreateBuilder<GraphicsAdapter>(unchecked((int)physicalDeviceCount));

            for (var index = 0u; index < physicalDeviceCount; index++)
            {
                var adapter = new GraphicsAdapter(this, physicalDevices[index]);
                adapters.Add(adapter);
            }

            return adapters.ToImmutable();
        }
    }
}
