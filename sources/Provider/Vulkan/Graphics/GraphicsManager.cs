// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;
using static System.Threading.Interlocked;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Interop.VkResult;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Vulkan.Graphics
{
    /// <summary>Provides a means of managing the graphics subsystem.</summary>
    [Export(typeof(IGraphicsManager))]
    [Export(typeof(GraphicsManager))]
    [Shared]
    public sealed unsafe class GraphicsManager : IDisposable, IGraphicsManager
    {
        #region Fields
        /// <summary>The Vulkan instance.</summary>
        internal readonly Lazy<IntPtr> _instance;

        /// <summary>The <see cref="GraphicsAdapter" /> instances available in the system.</summary>
        internal readonly Lazy<ImmutableArray<GraphicsAdapter>> _adapters;

        /// <summary>The <see cref="State" /> of the instance.</summary>
        internal readonly State _state;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="GraphicsManager" /> class.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateInstance(VkInstanceCreateInfo*, VkAllocationCallbacks*, IntPtr*)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="vkEnumeratePhysicalDevices(IntPtr, uint*, IntPtr*)" /> failed.</exception>
        [ImportingConstructor]
        public GraphicsManager()
        {
            _instance = new Lazy<IntPtr>(CreateInstance, isThreadSafe: true);
            _adapters = new Lazy<ImmutableArray<GraphicsAdapter>>(GetGraphicsAdapters, isThreadSafe: true);
            _state.Transition(to: Initialized);
        }
        #endregion

        #region Destructors
        /// <summary>Finalizes an instance of the <see cref="GraphicsManager" /> class.</summary>
        ~GraphicsManager()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region Static Methods
        /// <summary>Creates a Vulkan instance.</summary>
        /// <returns>A Vulkan instance.</returns>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateInstance(VkInstanceCreateInfo*, VkAllocationCallbacks*, IntPtr*)" /> failed.</exception>
        internal static IntPtr CreateInstance()
        {
            var createInfo = new VkInstanceCreateInfo() {
                sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
                pNext = null,
                flags = 0,
                pApplicationInfo = null,
                enabledLayerCount = 0,
                ppEnabledLayerNames = null,
                enabledExtensionCount = 0,
                ppEnabledExtensionNames = null
            };

            IntPtr instance;
            var result = vkCreateInstance(&createInfo, null, &instance);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkCreateInstance), (int)(result));
            }

            return instance;
        }
        #endregion

        #region Methods
        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        /// <exception cref="ExternalException">The call to <see cref="vkDestroyInstance(IntPtr, VkAllocationCallbacks*)" /> failed.</exception>
        internal void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeInstance();
            }

            _state.EndDispose();
        }

        /// <summary>Disposes of the Vulkan instance that was created.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkDestroyInstance(IntPtr, VkAllocationCallbacks*)" /> failed.</exception>
        internal void DisposeInstance()
        {
            if (_instance.IsValueCreated)
            {
                vkDestroyInstance(_instance.Value, null);
            }
        }

        /// <summary>Gets the <see cref="GraphicsAdapter" /> instances available in the system.</summary>
        /// <returns>The <see cref="GraphicsAdapter" /> instances available in the system.</returns>
        /// <exception cref="ExternalException">The call to <see cref="vkEnumeratePhysicalDevices(IntPtr, uint*, IntPtr*)" /> failed.</exception>
        internal ImmutableArray<GraphicsAdapter> GetGraphicsAdapters()
        {
            var instance = _instance.Value;

            var physicalDeviceCount = 0u;
            var result = vkEnumeratePhysicalDevices(instance, &physicalDeviceCount, null);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkEnumeratePhysicalDevices), (int)(result));
            }

            var physicalDevices = stackalloc IntPtr[unchecked((int)(physicalDeviceCount))];
            result = vkEnumeratePhysicalDevices(instance, &physicalDeviceCount, physicalDevices);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkEnumeratePhysicalDevices), (int)(result));
            }

            var adapters = ImmutableArray.CreateBuilder<GraphicsAdapter>(unchecked((int)(physicalDeviceCount)));

            for (var index = 0u; index < physicalDeviceCount; index++)
            {
                var adapter = new GraphicsAdapter(this, physicalDevices[index]);
                adapters.Add(adapter);
            }

            return adapters.ToImmutable();
        }
        #endregion

        #region TerraFX.Graphics.IGraphicsManager Properties
        /// <summary>Gets the <see cref="IGraphicsAdapter" /> instances currently available.</summary>
        public IEnumerable<IGraphicsAdapter> GraphicsAdapters
        {
            get
            {
                _state.ThrowIfDisposed();
                return _adapters.Value;
            }
        }
        #endregion

        #region System.IDisposable Methods
        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
