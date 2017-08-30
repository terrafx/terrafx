// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TerraFX.Graphics;
using TerraFX.Interop;
using static System.Threading.Interlocked;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Interop.VkResult;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.Vulkan.Graphics
{
    /// <summary>Provides a means of managing the graphics subsystem.</summary>
    [Export(typeof(IGraphicsManager))]
    [Export(typeof(GraphicsManager))]
    [Shared]
    public sealed unsafe class GraphicsManager : IDisposable, IGraphicsManager
    {
        #region State Constants
        /// <summary>Indicates the graphics manager is not disposing or disposed.</summary>
        internal const int NotDisposingOrDisposed = 0;

        /// <summary>Indicates the graphics manager is being disposed.</summary>
        internal const int Disposing = 1;

        /// <summary>Indicates the graphics manager has been disposed.</summary>
        internal const int Disposed = 2;
        #endregion

        #region Fields
        /// <summary>The Vulkan instance.</summary>
        internal IntPtr _instance;

        /// <summary>The <see cref="GraphicsAdapter" /> instances available in the system.</summary>
        internal ImmutableArray<GraphicsAdapter> _adapters;

        /// <summary>The state for the instance.</summary>
        /// <remarks>
        ///     <para>This field is <c>volatile</c> to ensure state changes update all threads simultaneously.</para>
        ///     <para><c>volatile</c> does add a read/write barrier at every access, but the state transitions are believed to be infrequent enough for this to not be a problem.</para>
        /// </remarks>
        internal volatile int _state;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="GraphicsManager" /> class.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateInstance(VkInstanceCreateInfo*, VkAllocationCallbacks*, IntPtr*)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="vkEnumeratePhysicalDevices(IntPtr, uint*, IntPtr*)" /> failed.</exception>
        [ImportingConstructor]
        internal GraphicsManager()
        {
            _instance = CreateVulkanInstance();
            _adapters = GetGraphicsAdapters(this, _instance);
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
        internal static IntPtr CreateVulkanInstance()
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

        /// <summary>Gets the <see cref="GraphicsAdapter" /> instances available in the system.</summary>
        /// <param name="graphicsManager">The <see cref="GraphicsManager" /> the adapters belong to.</param>
        /// <param name="instance">The Vulkan instance used to enumerate the available adapters.</param>
        /// <returns>The <see cref="GraphicsAdapter" /> instances available in the system.</returns>
        /// <exception cref="ExternalException">The call to <see cref="vkEnumeratePhysicalDevices(IntPtr, uint*, IntPtr*)" /> failed.</exception>
        internal static ImmutableArray<GraphicsAdapter> GetGraphicsAdapters(GraphicsManager graphicsManager, IntPtr instance)
        {
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
                var adapter = new GraphicsAdapter(graphicsManager, physicalDevices[index]);
                adapters.Add(adapter);
            }

            return adapters.ToImmutable();
        }

        /// <summary>Throws a <see cref="ObjectDisposedException" /> if the instance has already been disposed.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        internal static void ThrowIfDisposed(int state)
        {
            if (state >= Disposing) // (_state == Disposing) || (_state == Disposed)
            {
                ThrowObjectDisposedException(nameof(GraphicsManager));
            }
        }
        #endregion

        #region Methods
        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        /// <exception cref="ExternalException">The call to <see cref="vkDestroyInstance(IntPtr, VkAllocationCallbacks*)" /> failed.</exception>
        internal void Dispose(bool isDisposing)
        {
            var previousState = Exchange(ref _state, Disposing);

            if (previousState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                DisposeVulkanInstance();
            }

            Debug.Assert(_instance == IntPtr.Zero);
            _state = Disposed;
        }

        /// <summary>Disposes of the Vulkan instance that was created.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkDestroyInstance(IntPtr, VkAllocationCallbacks*)" /> failed.</exception>
        internal void DisposeVulkanInstance()
        {
            Debug.Assert(_state == Disposing);

            if (_instance != IntPtr.Zero)
            {
                vkDestroyInstance(_instance, null);
                _instance = IntPtr.Zero;
            }
        }
        #endregion

        #region TerraFX.Graphics.IGraphicsManager Properties
        /// <summary>Gets the <see cref="IGraphicsAdapter" /> instances currently available.</summary>
        public IEnumerable<IGraphicsAdapter> GraphicsAdapters
        {
            get
            {
                ThrowIfDisposed(_state);
                return _adapters;
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
