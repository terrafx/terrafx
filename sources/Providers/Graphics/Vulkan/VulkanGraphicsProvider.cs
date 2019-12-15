// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.VkDebugReportFlagBitsEXT;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.InteropUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc cref="GraphicsProvider" />
    [Export(typeof(GraphicsProvider))]
    [Shared]
    public sealed unsafe class VulkanGraphicsProvider : GraphicsProvider
    {
        /// <summary>The default engine name used if <see cref="EngineNameDataName" /> was not set.</summary>
        public const string DefaultEngineName = "TerraFX";

        /// <summary>The name of a data value that controls the engine name for <see cref="Instance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value after a graphics provider instance has been created has no affect.</para>
        ///     <para>This data value is interpreted as a string.</para>
        /// </remarks>
        public const string EngineNameDataName = "TerraFX.Graphics.Providers.Vulkan.GraphicsProvider.EngineName";

        /// <summary>The name of a data value that controls the optional extensions for <see cref="Instance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value after a graphics provider instance has been created has no affect.</para>
        ///     <para>This data value is interpreted as a list of semicolon separated values.</para>
        /// </remarks>
        public const string OptionalExtensionNamesDataName = "TerraFX.Graphics.Providers.Vulkan.GraphicsProvider.OptionalExtensionNames";

        /// <summary>The name of a data value that controls the optional layers for <see cref="Instance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value after a graphics provider instance has been created has no affect.</para>
        ///     <para>This data value is interpreted as a list of semicolon separated values.</para>
        /// </remarks>
        public const string OptionalLayerNamesDataName = "TerraFX.Graphics.Providers.Vulkan.GraphicsProvider.OptionalLayerNames";

        /// <summary>The name of a data value that controls the required extensions for <see cref="Instance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value after a graphics provider instance has been created has no affect.</para>
        ///     <para>This data value is interpreted as a list of semicolon separated values.</para>
        /// </remarks>
        public const string RequiredExtensionNamesDataName = "TerraFX.Graphics.Providers.Vulkan.GraphicsProvider.RequiredExtensionNames";

        /// <summary>The name of a data value that controls the required layers for <see cref="Instance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value after a graphics provider instance has been created has no affect.</para>
        ///     <para>This data value is interpreted as a list of semicolon separated values.</para>
        /// </remarks>
        public const string RequiredLayerNamesDataName = "TerraFX.Graphics.Providers.Vulkan.GraphicsProvider.RequiredLayerNames";

        private static readonly NativeDelegate<PFN_vkDebugReportCallbackEXT> s_debugReportCallback = new NativeDelegate<PFN_vkDebugReportCallbackEXT>(DebugReportCallback);

        private readonly string _engineName;
        private readonly string[] _requiredExtensionNames;
        private readonly string[] _optionalExtensionNames;
        private readonly string[] _requiredLayerNames;
        private readonly string[] _optionalLayerNames;

        private ValueLazy<ImmutableArray<VulkanGraphicsAdapter>> _graphicsAdapters;
        private ValueLazy<VkInstance> _instance;

        private VkDebugReportCallbackEXT _debugReportCallbackExt;

        private State _state;

        /// <summary>Initializes a new instance of the <see cref="VulkanGraphicsProvider" /> class.</summary>
        [ImportingConstructor]
        public VulkanGraphicsProvider()
        {
            if (DebugModeEnabled)
            {
                var optionalExtensionNamesData = AppContext.GetData(OptionalExtensionNamesDataName) as string;
                optionalExtensionNamesData += ";VK_EXT_debug_report";
                AppDomain.CurrentDomain.SetData(OptionalExtensionNamesDataName, optionalExtensionNamesData);

                var optionalLayerNamesData = AppContext.GetData(OptionalLayerNamesDataName) as string;
                optionalLayerNamesData += ";VK_LAYER_LUNARG_standard_validation";
                AppDomain.CurrentDomain.SetData(OptionalLayerNamesDataName, optionalLayerNamesData);
            }

            _engineName = GetEngineName();

            _requiredExtensionNames = GetNames(RequiredExtensionNamesDataName);
            _optionalExtensionNames = GetNames(OptionalExtensionNamesDataName);

            _requiredLayerNames = GetNames(RequiredLayerNamesDataName);
            _optionalLayerNames = GetNames(OptionalLayerNamesDataName);

            _graphicsAdapters = new ValueLazy<ImmutableArray<VulkanGraphicsAdapter>>(GetGraphicsAdapters);
            _instance = new ValueLazy<VkInstance>(CreateInstance);

            _ = _state.Transition(to: Initialized);

            static string GetEngineName()
            {
                var engineName = AppContext.GetData(EngineNameDataName) as string;

                if (string.IsNullOrWhiteSpace(engineName))
                {
                    engineName = DefaultEngineName;
                    AppDomain.CurrentDomain.SetData(EngineNameDataName, engineName);
                }

                return engineName;
            }

            static string[] GetNames(string dataName)
            {
                var extensionPropertyNamesDataValue = AppContext.GetData(dataName) as string ?? string.Empty;
                var extensionPropertyNames = extensionPropertyNamesDataValue.Split(';', StringSplitOptions.RemoveEmptyEntries);
                return extensionPropertyNames.Distinct().ToArray();
            }
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsProvider" /> class.</summary>
        ~VulkanGraphicsProvider()
        {
            Dispose(isDisposing: false);
        }

        // vkCreateDebugReportCallbackEXT
        private static ReadOnlySpan<sbyte> VKCREATEDEBUGREPORTCALLBACKEXT_FUNCTION_NAME => new sbyte[] { 0x76, 0x6B, 0x43, 0x72, 0x65, 0x61, 0x74, 0x65, 0x44, 0x65, 0x62, 0x75, 0x67, 0x52, 0x65, 0x70, 0x6F, 0x72, 0x74, 0x43, 0x61, 0x6C, 0x6C, 0x62, 0x61, 0x63, 0x6B, 0x45, 0x58, 0x54, 0x00 };

        // vkDestroyDebugReportCallbackEXT
        private static ReadOnlySpan<sbyte> VKDESTROYDEBUGREPORTCALLBACKEXT_FUNCTION_NAME => new sbyte[] { 0x76, 0x6B, 0x44, 0x65, 0x73, 0x74, 0x72, 0x6F, 0x79, 0x44, 0x65, 0x62, 0x75, 0x67, 0x52, 0x65, 0x70, 0x6F, 0x72, 0x74, 0x43, 0x61, 0x6C, 0x6C, 0x62, 0x61, 0x63, 0x6B, 0x45, 0x58, 0x54, 0x00 };

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkEnumeratePhysicalDevices(IntPtr, uint*, IntPtr*)" /> failed.</exception>
        public override IEnumerable<GraphicsAdapter> GraphicsAdapters
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _graphicsAdapters.Value;
            }
        }

        /// <summary>Gets the underlying <see cref="VkInstance" />.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateInstance(VkInstanceCreateInfo*, VkAllocationCallbacks*, IntPtr*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The instance has been disposed.</exception>
        public VkInstance Instance
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _instance.Value;
            }
        }

        private static uint DebugReportCallback(uint flags, VkDebugReportObjectTypeEXT objectType, ulong @object, UIntPtr location, int messageCode, sbyte* pLayerPrefix, sbyte* pMessage, void* pUserData)
        {
            var message = MarshalUtf8ToReadOnlySpan(pMessage).AsString();
            Debug.WriteLine(message);
            return VK_FALSE;
        }

        private VkInstance CreateInstance()
        {
            _state.AssertNotDisposedOrDisposing();

            sbyte* requiredExtensionNamesBuffer = null;
            sbyte* optionalExtensionNamesBuffer = null;
            sbyte** enabledExtensionNames = null;

            sbyte* requiredLayerNamesBuffer = null;
            sbyte* optionalLayerNamesBuffer = null;
            sbyte** enabledLayerNames = null;

            try
            {
                VkInstance instance;

                var enabledExtensionCount = EnableExtensionProperties(_requiredExtensionNames, _optionalExtensionNames, out requiredExtensionNamesBuffer, out optionalExtensionNamesBuffer, out enabledExtensionNames);
                var enabledLayerCount = EnableLayerProperties(_requiredLayerNames, _optionalLayerNames, out requiredLayerNamesBuffer, out optionalLayerNamesBuffer, out enabledLayerNames);

                fixed (sbyte* engineName = MarshalStringToUtf8(_engineName))
                {
                    var applicationInfo = new VkApplicationInfo {
                        sType = VK_STRUCTURE_TYPE_APPLICATION_INFO,
                        applicationVersion = 1,
                        pEngineName = engineName,
                        engineVersion = VK_MAKE_VERSION(0, 1, 0),
                        apiVersion = VK_API_VERSION_1_0,
                    };

                    var createInfo = new VkInstanceCreateInfo {
                        sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
                        pApplicationInfo = &applicationInfo,
                        enabledLayerCount = enabledLayerCount,
                        ppEnabledLayerNames = enabledLayerNames,
                        enabledExtensionCount = enabledExtensionCount,
                        ppEnabledExtensionNames = enabledExtensionNames,
                    };

                    ThrowExternalExceptionIfNotSuccess(nameof(vkCreateInstance), vkCreateInstance(&createInfo, pAllocator: null, (IntPtr*)&instance));
                }

                if (DebugModeEnabled)
                {
                    _debugReportCallbackExt = TryCreateDebugReportCallbackExt(instance);
                }

                return instance;
            }
            finally
            {
                if (enabledLayerNames != null)
                {
                    Marshal.FreeHGlobal((IntPtr)enabledLayerNames);
                }

                if (optionalLayerNamesBuffer != null)
                {
                    Marshal.FreeHGlobal((IntPtr)optionalLayerNamesBuffer);
                }

                if (requiredLayerNamesBuffer != null)
                {
                    Marshal.FreeHGlobal((IntPtr)requiredLayerNamesBuffer);
                }

                if (enabledExtensionNames != null)
                {
                    Marshal.FreeHGlobal((IntPtr)enabledExtensionNames);
                }

                if (optionalExtensionNamesBuffer != null)
                {
                    Marshal.FreeHGlobal((IntPtr)optionalExtensionNamesBuffer);
                }

                if (requiredExtensionNamesBuffer != null)
                {
                    Marshal.FreeHGlobal((IntPtr)requiredExtensionNamesBuffer);
                }
            }

            static uint EnableExtensionProperties(string[] requiredExtensionNames, string[] optionalExtensionNames, out sbyte* requiredExtensionNamesBuffer, out sbyte* optionalExtensionNamesBuffer, out sbyte** enabledExtensionNames)
            {
                var requiredExtensionNamesCount = MarshalNames(requiredExtensionNames, out requiredExtensionNamesBuffer);
                var optionalExtensionNamesCount = MarshalNames(optionalExtensionNames, out optionalExtensionNamesBuffer);

                enabledExtensionNames = (sbyte**)Marshal.AllocHGlobal((requiredExtensionNamesCount + optionalExtensionNamesCount) * sizeof(sbyte*));
                var extensionProperties = GetExtensionProperties();

                var enabledExtensionCount = EnableExtensionNames(extensionProperties, requiredExtensionNamesBuffer, requiredExtensionNamesCount, enabledExtensionNames);

                if (enabledExtensionCount != requiredExtensionNamesCount)
                {
                    ThrowNotSupportedExceptionForMissingFeature(nameof(VkExtensionProperties));
                }
                enabledExtensionCount += EnableExtensionNames(extensionProperties, optionalExtensionNamesBuffer, optionalExtensionNamesCount, enabledExtensionNames + enabledExtensionCount);

                return enabledExtensionCount;

                static uint EnableExtensionNames(VkExtensionProperties[] extensionProperties, sbyte* extensionNames, int extensionNamesCount, sbyte** enabledExtensionNames)
                {
                    uint enabledExtensionCount = 0;
                    var pCurrent = extensionNames;

                    for (var i = 0; i < extensionNamesCount; i++)
                    {
                        var extensionNameLength = *(int*)pCurrent;
                        pCurrent += IntPtr.Size;
                        var extensionName = new ReadOnlySpan<sbyte>(pCurrent, extensionNameLength + 1);

                        for (var n = 0; n < extensionProperties.Length; n++)
                        {
                            var extensionPropertyName = MemoryMarshal.CreateReadOnlySpan(ref extensionProperties[n].extensionName[0], extensionNameLength + 1);

                            if (extensionPropertyName.SequenceEqual(extensionName))
                            {
                                enabledExtensionNames[enabledExtensionCount] = pCurrent;
                                enabledExtensionCount += 1;
                                break;
                            }
                        }
                        pCurrent += VK_MAX_EXTENSION_NAME_SIZE;
                    }

                    return enabledExtensionCount;
                }

                static VkExtensionProperties[] GetExtensionProperties()
                {
                    uint propertyCount = 0;
                    ThrowExternalExceptionIfNotSuccess(nameof(vkEnumerateInstanceExtensionProperties), vkEnumerateInstanceExtensionProperties(pLayerName: null, &propertyCount, pProperties: null));

                    var extensionProperties = new VkExtensionProperties[propertyCount];

                    fixed (VkExtensionProperties* pExtensionProperties = extensionProperties)
                    {
                        ThrowExternalExceptionIfNotSuccess(nameof(vkEnumerateInstanceExtensionProperties), vkEnumerateInstanceExtensionProperties(pLayerName: null, &propertyCount, pExtensionProperties));
                    }

                    return extensionProperties;
                }
            }

            static uint EnableLayerProperties(string[] requiredLayerNames, string[] optionalLayerNames, out sbyte* requiredLayerNamesBuffer, out sbyte* optionalLayerNamesBuffer, out sbyte** enabledLayerNames)
            {
                var requiredLayerNamesCount = MarshalNames(requiredLayerNames, out requiredLayerNamesBuffer);
                var optionalLayerNamesCount = MarshalNames(optionalLayerNames, out optionalLayerNamesBuffer);

                enabledLayerNames = (sbyte**)Marshal.AllocHGlobal((requiredLayerNamesCount + optionalLayerNamesCount) * sizeof(sbyte*));
                var layerProperties = GetLayerProperties();

                var enabledLayerCount = EnableLayerNames(layerProperties, requiredLayerNamesBuffer, requiredLayerNamesCount, enabledLayerNames);

                if (enabledLayerCount != requiredLayerNamesCount)
                {
                    ThrowNotSupportedExceptionForMissingFeature(nameof(VkLayerProperties));
                }
                enabledLayerCount += EnableLayerNames(layerProperties, optionalLayerNamesBuffer, optionalLayerNamesCount, enabledLayerNames + enabledLayerCount);

                return enabledLayerCount;

                static uint EnableLayerNames(VkLayerProperties[] layerProperties, sbyte* layerNames, int layerNamesCount, sbyte** enabledLayerNames)
                {
                    uint enabledLayerCount = 0;
                    var pCurrent = layerNames;

                    for (var i = 0; i < layerNamesCount; i++)
                    {
                        var layerNameLength = *(int*)pCurrent;
                        pCurrent += IntPtr.Size;
                        var layerName = new ReadOnlySpan<sbyte>(pCurrent, layerNameLength + 1);

                        for (var n = 0; n < layerProperties.Length; n++)
                        {
                            var layerPropertyName = MemoryMarshal.CreateReadOnlySpan(ref layerProperties[n].layerName[0], layerNameLength + 1);

                            if (layerPropertyName.SequenceEqual(layerName))
                            {
                                enabledLayerNames[enabledLayerCount] = pCurrent;
                                enabledLayerCount += 1;
                                break;
                            }
                        }
                        pCurrent += VK_MAX_EXTENSION_NAME_SIZE;
                    }

                    return enabledLayerCount;
                }

                static VkLayerProperties[] GetLayerProperties()
                {
                    uint propertyCount = 0;
                    ThrowExternalExceptionIfNotSuccess(nameof(vkEnumerateInstanceLayerProperties), vkEnumerateInstanceLayerProperties(&propertyCount, pProperties: null));

                    var layerProperties = new VkLayerProperties[propertyCount];

                    fixed (VkLayerProperties* pLayerProperties = layerProperties)
                    {
                        ThrowExternalExceptionIfNotSuccess(nameof(vkEnumerateInstanceLayerProperties), vkEnumerateInstanceLayerProperties(&propertyCount, pLayerProperties));
                    }

                    return layerProperties;
                }
            }

            static int MarshalNames(string[] names, out sbyte* marshaledNames)
            {
                var sizePerEntry = IntPtr.Size + VK_MAX_EXTENSION_NAME_SIZE;
                marshaledNames = (sbyte*)Marshal.AllocHGlobal(names.Length * sizePerEntry);

                var pCurrent = marshaledNames;

                for (var i = 0; i < names.Length; i++)
                {
                    var destination = new Span<byte>(pCurrent + IntPtr.Size, VK_MAX_EXTENSION_NAME_SIZE);
                    var length = Encoding.UTF8.GetBytes(names[i], destination);

                    pCurrent[IntPtr.Size + length] = (sbyte)'\0';

                    *(int*)pCurrent = length;
                    pCurrent += sizePerEntry;
                }

                return names.Length;
            }

            static VkDebugReportCallbackEXT TryCreateDebugReportCallbackExt(VkInstance instance)
            {
                VkDebugReportCallbackEXT debugReportCallbackExt;

                var debugReportCallbackCreateInfo = new VkDebugReportCallbackCreateInfoEXT {
                    sType = VK_STRUCTURE_TYPE_DEBUG_REPORT_CALLBACK_CREATE_INFO_EXT,
                    flags = (uint)(VK_DEBUG_REPORT_ERROR_BIT_EXT | VK_DEBUG_REPORT_WARNING_BIT_EXT | VK_DEBUG_REPORT_PERFORMANCE_WARNING_BIT_EXT),
                    pfnCallback = s_debugReportCallback,
                };

                // We don't want to fail if creating the debug report callback failed
                var vkCreateDebugReportCallbackEXT = vkGetInstanceProcAddr(instance, VKCREATEDEBUGREPORTCALLBACKEXT_FUNCTION_NAME.AsPointer());
                _ = MarshalFunctionPointer<PFN_vkCreateDebugReportCallbackEXT>(vkCreateDebugReportCallbackEXT)(instance, &debugReportCallbackCreateInfo, pAllocator: null, (ulong*)&debugReportCallbackExt);

                return debugReportCallbackExt;
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
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
                var instance = _instance.Value;

                if (_debugReportCallbackExt != VK_NULL_HANDLE)
                {
                    var vkDestroyDebugReportCallbackEXT = vkGetInstanceProcAddr(instance, VKDESTROYDEBUGREPORTCALLBACKEXT_FUNCTION_NAME.AsPointer());
                    MarshalFunctionPointer<PFN_vkDestroyDebugReportCallbackEXT>(vkDestroyDebugReportCallbackEXT)(instance, _debugReportCallbackExt, pAllocator: null);
                }

                vkDestroyInstance(instance, pAllocator: null);
            }
        }

        private ImmutableArray<VulkanGraphicsAdapter> GetGraphicsAdapters()
        {
            _state.AssertNotDisposedOrDisposing();

            var instance = _instance.Value;

            uint physicalDeviceCount;
            ThrowExternalExceptionIfNotSuccess(nameof(vkEnumeratePhysicalDevices), vkEnumeratePhysicalDevices(instance, &physicalDeviceCount, pPhysicalDevices: null));

            var physicalDevices = stackalloc IntPtr[unchecked((int)physicalDeviceCount)];
            ThrowExternalExceptionIfNotSuccess(nameof(vkEnumeratePhysicalDevices), vkEnumeratePhysicalDevices(instance, &physicalDeviceCount, physicalDevices));

            var adapters = ImmutableArray.CreateBuilder<VulkanGraphicsAdapter>(unchecked((int)physicalDeviceCount));

            for (uint index = 0; index < physicalDeviceCount; index++)
            {
                var adapter = new VulkanGraphicsAdapter(this, physicalDevices[index]);
                adapters.Add(adapter);
            }

            return adapters.ToImmutable();
        }
    }
}
