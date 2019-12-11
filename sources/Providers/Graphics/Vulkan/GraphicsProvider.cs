// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkResult;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.VkDebugReportFlagBitsEXT;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <summary>Provides access to a Vulkan based graphics subsystem.</summary>
    [Export(typeof(IGraphicsProvider))]
    [Shared]
    public sealed unsafe class GraphicsProvider : IDisposable, IGraphicsProvider
    {
        // TerraFX
        private static ReadOnlySpan<sbyte> s_engineName => new sbyte[] { 0x54, 0x65, 0x72, 0x72, 0x61, 0x46, 0x58, 0x00 };

        // VK_LAYER_LUNARG_standard_validation
        private static ReadOnlySpan<sbyte> VK_LAYER_LUNARG_STANDARD_VALIDATION_LAYER_NAME => new sbyte[] { 0x56, 0x4B, 0x5F, 0x4C, 0x41, 0x59, 0x45, 0x52, 0x5F, 0x4C, 0x55, 0x4E, 0x41, 0x52, 0x47, 0x5F, 0x73, 0x74, 0x61, 0x6E, 0x64, 0x61, 0x72, 0x64, 0x5F, 0x76, 0x61, 0x6C, 0x69, 0x64, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x00 };

        // vkCreateDebugReportCallbackEXT
        private static ReadOnlySpan<sbyte> VKCREATEDEBUGREPORTCALLBACKEXT_FUNCTION_NAME => new sbyte[] { 0x76, 0x6B, 0x43, 0x72, 0x65, 0x61, 0x74, 0x65, 0x44, 0x65, 0x62, 0x75, 0x67, 0x52, 0x65, 0x70, 0x6F, 0x72, 0x74, 0x43, 0x61, 0x6C, 0x6C, 0x62, 0x61, 0x63, 0x6B, 0x45, 0x58, 0x54, 0x00 };

        // vkDestroyDebugReportCallbackEXT
        private static ReadOnlySpan<sbyte> VKDESTROYDEBUGREPORTCALLBACKEXT_FUNCTION_NAME => new sbyte[] { 0x76, 0x6B, 0x44, 0x65, 0x73, 0x74, 0x72, 0x6F, 0x79, 0x44, 0x65, 0x62, 0x75, 0x67, 0x52, 0x65, 0x70, 0x6F, 0x72, 0x74, 0x43, 0x61, 0x6C, 0x6C, 0x62, 0x61, 0x63, 0x6B, 0x45, 0x58, 0x54, 0x00 };

#if DEBUG
        private NativeDelegate<PFN_vkDebugReportCallbackEXT> _debugReportCallback;
        private ulong _debugReportCallbackExt;
#endif

        private ValueLazy<ImmutableArray<GraphicsAdapter>> _adapters;
        private ValueLazy<IntPtr> _instance;

        private State _state;

        /// <summary>Initializes a new instance of the <see cref="GraphicsProvider" /> class.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateInstance(VkInstanceCreateInfo*, VkAllocationCallbacks*, IntPtr*)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="vkEnumeratePhysicalDevices(IntPtr, uint*, IntPtr*)" /> failed.</exception>
        [ImportingConstructor]
        public GraphicsProvider()
        {
            _adapters = new ValueLazy<ImmutableArray<GraphicsAdapter>>(GetGraphicsAdapters);
            _instance = new ValueLazy<IntPtr>(CreateInstance);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="GraphicsProvider" /> class.</summary>
        ~GraphicsProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        private IntPtr CreateInstance()
        {
            var enabledExtensionCount = 0u;
            var isSurfaceExtensionSupported = false;
            var isWin32SurfaceExtensionSupported = false;
            var isXlibSurfaceExtensionSupported = false;
            var isDebugReportExtensionSupported = false;

            var extensionPropertyCount = 0u;
            ThrowExternalExceptionIfFailed(nameof(vkEnumerateInstanceExtensionProperties), vkEnumerateInstanceExtensionProperties(pLayerName: null, &extensionPropertyCount, pProperties: null));

            var extensionProperties = new VkExtensionProperties[extensionPropertyCount];

            fixed (VkExtensionProperties* pExtensionProperties = extensionProperties)
            {
                ThrowExternalExceptionIfFailed(nameof(vkEnumerateInstanceExtensionProperties), vkEnumerateInstanceExtensionProperties(pLayerName: null, &extensionPropertyCount, pExtensionProperties));
            }

            for (var i = 0; i < extensionProperties.Length; i++)
            {
                var extensionName = new ReadOnlySpan<sbyte>(Unsafe.AsPointer(ref extensionProperties[i].extensionName[0]), int.MaxValue);
                extensionName = extensionName.Slice(0, extensionName.IndexOf((sbyte)'\0') + 1);

                if (!isSurfaceExtensionSupported && VK_KHR_SURFACE_EXTENSION_NAME.SequenceEqual(extensionName))
                {
                    isSurfaceExtensionSupported = true;
                    enabledExtensionCount++;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (!isWin32SurfaceExtensionSupported && VK_KHR_WIN32_SURFACE_EXTENSION_NAME.SequenceEqual(extensionName))
                    {
                        isWin32SurfaceExtensionSupported = true;
                        enabledExtensionCount++;
                    }
                }
                else
                {
                    if (!isXlibSurfaceExtensionSupported && VK_KHR_XLIB_SURFACE_EXTENSION_NAME.SequenceEqual(extensionName))
                    {
                        isXlibSurfaceExtensionSupported = true;
                        enabledExtensionCount++;
                    }
                }

#if DEBUG
                if (!isDebugReportExtensionSupported && VK_EXT_DEBUG_REPORT_EXTENSION_NAME.SequenceEqual(extensionName))
                {
                    isDebugReportExtensionSupported = true;
                    enabledExtensionCount++;
                }
#endif
            }

            var enabledExtensionNames = stackalloc sbyte*[(int)enabledExtensionCount];

            if (!isSurfaceExtensionSupported)
            {
                ThrowInvalidOperationException(nameof(isSurfaceExtensionSupported), isSurfaceExtensionSupported);
            }

            enabledExtensionNames[0] = (sbyte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(VK_KHR_SURFACE_EXTENSION_NAME));

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (!isWin32SurfaceExtensionSupported)
                {
                    ThrowInvalidOperationException(nameof(isWin32SurfaceExtensionSupported), isWin32SurfaceExtensionSupported);
                }

                enabledExtensionNames[1] = (sbyte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(VK_KHR_WIN32_SURFACE_EXTENSION_NAME));
            }
            else
            {
                if (!isXlibSurfaceExtensionSupported)
                {
                    ThrowInvalidOperationException(nameof(isXlibSurfaceExtensionSupported), isXlibSurfaceExtensionSupported);
                }

                enabledExtensionNames[1] = (sbyte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(VK_KHR_XLIB_SURFACE_EXTENSION_NAME));
            }

            if (isDebugReportExtensionSupported)
            {
                // We don't want to throw if the debug extension isn't available
                enabledExtensionNames[2] = (sbyte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(VK_EXT_DEBUG_REPORT_EXTENSION_NAME));
            }

            var enabledLayerCount = 0u;
            var isLunarGStandardValidationLayerSupported = false;

#if DEBUG
            var layerPropertyCount = 0u;
            ThrowExternalExceptionIfFailed(nameof(vkEnumerateInstanceLayerProperties), vkEnumerateInstanceLayerProperties(&layerPropertyCount, pProperties: null));

            var layerProperties = new VkLayerProperties[layerPropertyCount];

            fixed (VkLayerProperties* pLayerProperties = layerProperties)
            {
                ThrowExternalExceptionIfFailed(nameof(vkEnumerateInstanceLayerProperties), vkEnumerateInstanceLayerProperties(&layerPropertyCount, pLayerProperties));
            }

            for (var i = 0; i < layerProperties.Length; i++)
            {
                var layerName = new ReadOnlySpan<sbyte>(Unsafe.AsPointer(ref layerProperties[i].layerName[0]), int.MaxValue);
                layerName = layerName.Slice(0, layerName.IndexOf((sbyte)'\0') + 1);

                if (!isLunarGStandardValidationLayerSupported && VK_LAYER_LUNARG_STANDARD_VALIDATION_LAYER_NAME.SequenceEqual(layerName))
                {
                    isLunarGStandardValidationLayerSupported = true;
                    enabledLayerCount++;
                }
            }
#endif

            var enabledLayerNames = stackalloc sbyte*[(int)enabledLayerCount];

            if (isLunarGStandardValidationLayerSupported)
            {
                enabledLayerNames[0] = (sbyte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(VK_LAYER_LUNARG_STANDARD_VALIDATION_LAYER_NAME));
            }

            var applicationInfo = new VkApplicationInfo {
                sType = VK_STRUCTURE_TYPE_APPLICATION_INFO,
                pNext = null,
                pApplicationName = null,
                applicationVersion = 1,
                pEngineName = (sbyte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(s_engineName)),
                engineVersion = VK_MAKE_VERSION(0, 1, 0),
                apiVersion = VK_API_VERSION_1_0,
            };

            var createInfo = new VkInstanceCreateInfo {
                sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
                pNext = null,
                flags = 0,
                pApplicationInfo = &applicationInfo,
                enabledLayerCount = enabledLayerCount,
                ppEnabledLayerNames = enabledLayerNames,
                enabledExtensionCount = enabledExtensionCount,
                ppEnabledExtensionNames = enabledExtensionNames,
            };

            IntPtr instance;
            var result = vkCreateInstance(&createInfo, null, &instance);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkCreateInstance), (int)result);
            }

            if (isDebugReportExtensionSupported && isLunarGStandardValidationLayerSupported)
            {
#if DEBUG
                ulong debugReportCallbackExt;

                _debugReportCallback = new NativeDelegate<PFN_vkDebugReportCallbackEXT>(DebugReportCallback);

                var debugReportCallbackCreateInfo = new VkDebugReportCallbackCreateInfoEXT {
                    sType = VK_STRUCTURE_TYPE_DEBUG_REPORT_CALLBACK_CREATE_INFO_EXT,
                    pNext = null,
                    flags = (uint)(VK_DEBUG_REPORT_ERROR_BIT_EXT | VK_DEBUG_REPORT_WARNING_BIT_EXT | VK_DEBUG_REPORT_PERFORMANCE_WARNING_BIT_EXT),
                    pfnCallback = _debugReportCallback,
                    pUserData = null,
                };

                // We don't want to fail if creating the debug report callback failed
                var vkCreateDebugReportCallbackEXT = vkGetInstanceProcAddr(instance, (sbyte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(VKCREATEDEBUGREPORTCALLBACKEXT_FUNCTION_NAME)));
                _ = Marshal.GetDelegateForFunctionPointer<PFN_vkCreateDebugReportCallbackEXT>(vkCreateDebugReportCallbackEXT)(instance, &debugReportCallbackCreateInfo, pAllocator: null, &debugReportCallbackExt);
                _debugReportCallbackExt = debugReportCallbackExt;
#endif
            }

            return instance;
        }

#if DEBUG
        private static uint DebugReportCallback(uint flags, VkDebugReportObjectTypeEXT objectType, ulong @object, UIntPtr location, int messageCode, sbyte* pLayerPrefix, sbyte* pMessage, void* pUserData)
        {
            var messageLength = new ReadOnlySpan<sbyte>(pMessage, int.MaxValue).IndexOf((sbyte)'\0') + 1;
            var message = new string(pMessage, 0, messageLength);

            Debug.WriteLine(message);
            return VK_FALSE;
        }
#endif

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
#if DEBUG
                if (_debugReportCallbackExt != 0)
                {
                    var vkDestroyDebugReportCallbackEXT = vkGetInstanceProcAddr(_instance.Value, (sbyte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(VKDESTROYDEBUGREPORTCALLBACKEXT_FUNCTION_NAME)));
                    Marshal.GetDelegateForFunctionPointer<PFN_vkDestroyDebugReportCallbackEXT>(vkDestroyDebugReportCallbackEXT)(_instance.Value, _debugReportCallbackExt, pAllocator: null);
                }
#endif

                vkDestroyInstance(_instance.Value, pAllocator: null);
            }
        }

        private ImmutableArray<GraphicsAdapter> GetGraphicsAdapters()
        {
            var instance = _instance.Value;

            var physicalDeviceCount = 0u;
            var result = vkEnumeratePhysicalDevices(instance, &physicalDeviceCount, pPhysicalDevices: null);

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
