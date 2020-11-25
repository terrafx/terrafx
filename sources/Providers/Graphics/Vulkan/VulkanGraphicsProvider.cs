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
    /// <inheritdoc />
    [Export(typeof(GraphicsProvider))]
    [Shared]
    public sealed unsafe class VulkanGraphicsProvider : GraphicsProvider
    {
        /// <summary>The default engine name used if <see cref="EngineNameDataName" /> was not set.</summary>
        public const string DefaultEngineName = "TerraFX";

        /// <summary>The name of a data value that controls the engine name for <see cref="VulkanInstance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on providers that have already been created.</para>
        ///     <para>This data value is interpreted as a string.</para>
        /// </remarks>
        public const string EngineNameDataName = "TerraFX.Graphics.Providers.Vulkan.GraphicsProvider.EngineName";

        /// <summary>The name of a data value that controls the optional extensions for <see cref="VulkanInstance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on providers that have already been created.</para>
        ///     <para>This data value is interpreted as a string of semicolon separated values.</para>
        /// </remarks>
        public const string OptionalExtensionNamesDataName = "TerraFX.Graphics.Providers.Vulkan.GraphicsProvider.OptionalExtensionNames";

        /// <summary>The name of a data value that controls the optional layers for <see cref="VulkanInstance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on providers that have already been created.</para>
        ///     <para>This data value is interpreted as a string of semicolon separated values.</para>
        /// </remarks>
        public const string OptionalLayerNamesDataName = "TerraFX.Graphics.Providers.Vulkan.GraphicsProvider.OptionalLayerNames";

        /// <summary>The name of a data value that controls the required extensions for <see cref="VulkanInstance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on providers that have already been created.</para>
        ///     <para>This data value is interpreted as a string of semicolon separated values.</para>
        /// </remarks>
        public const string RequiredExtensionNamesDataName = "TerraFX.Graphics.Providers.Vulkan.GraphicsProvider.RequiredExtensionNames";

        /// <summary>The name of a data value that controls the required layers for <see cref="VulkanInstance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on providers that have already been created.</para>
        ///     <para>This data value is interpreted as a string of semicolon separated values.</para>
        /// </remarks>
        public const string RequiredLayerNamesDataName = "TerraFX.Graphics.Providers.Vulkan.GraphicsProvider.RequiredLayerNames";

        private static readonly delegate* unmanaged<uint, VkDebugReportObjectTypeEXT, ulong, nuint, int, sbyte*, sbyte*, void*, uint> s_vulkanDebugReportCallback = (delegate* unmanaged<uint, VkDebugReportObjectTypeEXT, ulong, nuint, int, sbyte*, sbyte*, void*, uint>)(delegate*<uint, VkDebugReportObjectTypeEXT, ulong, nuint, int, sbyte*, sbyte*, void*, uint>)&VulkanDebugReportCallback;

        private readonly string _engineName;
        private readonly string[] _requiredExtensionNames;
        private readonly string[] _optionalExtensionNames;
        private readonly string[] _requiredLayerNames;
        private readonly string[] _optionalLayerNames;

        private ValueLazy<VkInstance> _vulkanInstance;
        private ValueLazy<ImmutableArray<VulkanGraphicsAdapter>> _adapters;

        private VkDebugReportCallbackEXT _vulkanDebugReportCallbackExt;

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

            _vulkanInstance = new ValueLazy<VkInstance>(CreateVulkanInstance);
            _adapters = new ValueLazy<ImmutableArray<VulkanGraphicsAdapter>>(GetGraphicsAdapters);

            _ = _state.Transition(to: Initialized);

            static string GetEngineName()
            {
                var engineNameData = AppContext.GetData(EngineNameDataName) as string;

                if (string.IsNullOrWhiteSpace(engineNameData))
                {
                    engineNameData = DefaultEngineName;
                    AppDomain.CurrentDomain.SetData(EngineNameDataName, engineNameData);
                }

                return engineNameData;
            }

            static string[] GetNames(string dataName)
            {
                var namesData = AppContext.GetData(dataName) as string ?? string.Empty;
                var names = namesData.Split(';', StringSplitOptions.RemoveEmptyEntries);
                return names.Distinct().ToArray();
            }
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsProvider" /> class.</summary>
        ~VulkanGraphicsProvider() => Dispose(isDisposing: false);

        // vkCreateDebugReportCallbackEXT
        private static ReadOnlySpan<sbyte> VKCREATEDEBUGREPORTCALLBACKEXT_FUNCTION_NAME => new sbyte[] { 0x76, 0x6B, 0x43, 0x72, 0x65, 0x61, 0x74, 0x65, 0x44, 0x65, 0x62, 0x75, 0x67, 0x52, 0x65, 0x70, 0x6F, 0x72, 0x74, 0x43, 0x61, 0x6C, 0x6C, 0x62, 0x61, 0x63, 0x6B, 0x45, 0x58, 0x54, 0x00 };

        // vkDestroyDebugReportCallbackEXT
        private static ReadOnlySpan<sbyte> VKDESTROYDEBUGREPORTCALLBACKEXT_FUNCTION_NAME => new sbyte[] { 0x76, 0x6B, 0x44, 0x65, 0x73, 0x74, 0x72, 0x6F, 0x79, 0x44, 0x65, 0x62, 0x75, 0x67, 0x52, 0x65, 0x70, 0x6F, 0x72, 0x74, 0x43, 0x61, 0x6C, 0x6C, 0x62, 0x61, 0x63, 0x6B, 0x45, 0x58, 0x54, 0x00 };

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkEnumeratePhysicalDevices(IntPtr, uint*, IntPtr*)" /> failed.</exception>
        public override IEnumerable<VulkanGraphicsAdapter> Adapters => _adapters.Value;

        /// <summary>Gets the underlying <see cref="VkInstance" /> for the provider.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateInstance(VkInstanceCreateInfo*, VkAllocationCallbacks*, IntPtr*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The provider has been disposed.</exception>
        public VkInstance VulkanInstance => _vulkanInstance.Value;

        private static uint VulkanDebugReportCallback(uint flags, VkDebugReportObjectTypeEXT objectType, ulong @object, nuint location, int messageCode, sbyte* pLayerPrefix, sbyte* pMessage, void* pUserData)
        {
            var message = MarshalUtf8ToReadOnlySpan(pMessage).AsString();
            Debug.WriteLine(message);
            return VK_FALSE;
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _vulkanInstance.Dispose(DisposeInstance);
            }

            _state.EndDispose();
        }

        private VkInstance CreateVulkanInstance()
        {
            _state.ThrowIfDisposedOrDisposing();

            sbyte* requiredExtensionNamesBuffer = null;
            sbyte* optionalExtensionNamesBuffer = null;
            sbyte** enabledExtensionNames = null;

            sbyte* requiredLayerNamesBuffer = null;
            sbyte* optionalLayerNamesBuffer = null;
            sbyte** enabledLayerNames = null;

            try
            {
                VkInstance vulkanInstance;

                uint enabledExtensionCount;
                var extensionProperties = GetExtensionProperties();

                fixed (VkExtensionProperties* pExtensionProperties = extensionProperties)
                {
                    enabledExtensionCount = EnableProperties((sbyte*)pExtensionProperties, extensionProperties.Length, sizeof(VkExtensionProperties), _requiredExtensionNames, _optionalExtensionNames, out requiredExtensionNamesBuffer, out optionalExtensionNamesBuffer, out enabledExtensionNames);
                }

                uint enabledLayerCount;
                var layerProperties = GetLayerProperties();

                fixed (VkLayerProperties* pLayerProperties = layerProperties)
                {
                    enabledLayerCount = EnableProperties((sbyte*)pLayerProperties, layerProperties.Length, sizeof(VkLayerProperties), _requiredLayerNames, _optionalLayerNames, out requiredLayerNamesBuffer, out optionalLayerNamesBuffer, out enabledLayerNames);
                }

                fixed (sbyte* engineName = MarshalStringToUtf8(_engineName))
                {
                    var applicationInfo = new VkApplicationInfo {
                        sType = VK_STRUCTURE_TYPE_APPLICATION_INFO,
                        applicationVersion = 1,
                        pEngineName = engineName,
                        engineVersion = VK_MAKE_VERSION(0, 1, 0),
                        apiVersion = VK_API_VERSION_1_1,
                    };

                    var instanceCreateInfo = new VkInstanceCreateInfo {
                        sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
                        pApplicationInfo = &applicationInfo,
                        enabledLayerCount = enabledLayerCount,
                        ppEnabledLayerNames = enabledLayerNames,
                        enabledExtensionCount = enabledExtensionCount,
                        ppEnabledExtensionNames = enabledExtensionNames,
                    };

                    ThrowExternalExceptionIfNotSuccess(vkCreateInstance(&instanceCreateInfo, pAllocator: null, (IntPtr*)&vulkanInstance), nameof(vkCreateInstance));
                }

                if (DebugModeEnabled)
                {
                    _vulkanDebugReportCallbackExt = TryCreateVulkanDebugReportCallbackExt(vulkanInstance);
                }

                return vulkanInstance;
            }
            finally
            {
                if (enabledLayerNames != null)
                {
                    Free(enabledLayerNames);
                }

                if (optionalLayerNamesBuffer != null)
                {
                    Free(optionalLayerNamesBuffer);
                }

                if (requiredLayerNamesBuffer != null)
                {
                    Free(requiredLayerNamesBuffer);
                }

                if (enabledExtensionNames != null)
                {
                    Free(enabledExtensionNames);
                }

                if (optionalExtensionNamesBuffer != null)
                {
                    Free(optionalExtensionNamesBuffer);
                }

                if (requiredExtensionNamesBuffer != null)
                {
                    Free(requiredExtensionNamesBuffer);
                }
            }

            static uint EnableProperties(sbyte* propertyNames, int propertyNamesCount, int propertySize, string[] requiredNames, string[] optionalNames, out sbyte* requiredNamesBuffer, out sbyte* optionalNamesBuffer, out sbyte** enabledNames)
            {
                var requiredNamesCount = MarshalNames(requiredNames, out requiredNamesBuffer);
                var optionalNamesCount = MarshalNames(optionalNames, out optionalNamesBuffer);

                enabledNames = (sbyte**)Allocate((nuint)((requiredNamesCount + optionalNamesCount) * sizeof(sbyte*)));

                var enabledPropertyCount = EnablePropertiesByName(propertyNames, propertyNamesCount, propertySize, requiredNamesBuffer, requiredNamesCount, enabledNames);

                if (enabledPropertyCount != requiredNamesCount)
                {
                    ThrowNotSupportedExceptionForMissingFeature(nameof(VkExtensionProperties));
                }
                enabledPropertyCount += EnablePropertiesByName(propertyNames, propertyNamesCount, propertySize, optionalNamesBuffer, optionalNamesCount, enabledNames + enabledPropertyCount);

                return enabledPropertyCount;
            }

            static uint EnablePropertiesByName(sbyte* propertyNames, int propertyNamesCount, int propertySize, sbyte* targetNames, int targetNamesCount, sbyte** enabledNames)
            {
                uint enabledPropertyCount = 0;

                var pPropertyName = propertyNames;
                var pTargetName = targetNames;

                for (var i = 0; i < targetNamesCount; i++)
                {
                    var targetNameLength = *(int*)pTargetName;
                    pTargetName += sizeof(nuint);
                    var targetName = new ReadOnlySpan<sbyte>(pTargetName, targetNameLength + 1);

                    for (var n = 0; n < propertyNamesCount; n++)
                    {
                        var propertyName = new ReadOnlySpan<sbyte>(pPropertyName, targetNameLength + 1);

                        if (propertyName.SequenceEqual(targetName))
                        {
                            enabledNames[enabledPropertyCount] = pTargetName;
                            enabledPropertyCount += 1;
                            break;
                        }

                        pPropertyName += propertySize;
                    }
                    pTargetName += VK_MAX_EXTENSION_NAME_SIZE;
                }

                return enabledPropertyCount;
            }

            static VkExtensionProperties[] GetExtensionProperties()
            {
                uint extensionPropertiesCount = 0;
                ThrowExternalExceptionIfNotSuccess(vkEnumerateInstanceExtensionProperties(pLayerName: null, &extensionPropertiesCount, pProperties: null), nameof(vkEnumerateInstanceExtensionProperties));

                var extensionProperties = new VkExtensionProperties[extensionPropertiesCount];

                fixed (VkExtensionProperties* pExtensionProperties = extensionProperties)
                {
                    ThrowExternalExceptionIfNotSuccess(vkEnumerateInstanceExtensionProperties(pLayerName: null, &extensionPropertiesCount, pExtensionProperties), nameof(vkEnumerateInstanceExtensionProperties));
                }

                return extensionProperties;
            }

            static VkLayerProperties[] GetLayerProperties()
            {
                uint layerPropertiesCount = 0;
                ThrowExternalExceptionIfNotSuccess(vkEnumerateInstanceLayerProperties(&layerPropertiesCount, pProperties: null), nameof(vkEnumerateInstanceLayerProperties));

                var layerProperties = new VkLayerProperties[layerPropertiesCount];

                fixed (VkLayerProperties* pLayerProperties = layerProperties)
                {
                    ThrowExternalExceptionIfNotSuccess(vkEnumerateInstanceLayerProperties(&layerPropertiesCount, pLayerProperties), nameof(vkEnumerateInstanceLayerProperties));
                }

                return layerProperties;
            }

            static int MarshalNames(string[] names, out sbyte* namesBuffer)
            {
                nuint sizePerEntry = SizeOf<nuint>() + VK_MAX_EXTENSION_NAME_SIZE;
                namesBuffer = (sbyte*)Allocate((nuint)names.Length * sizePerEntry);

                var pCurrent = namesBuffer;

                for (var i = 0; i < names.Length; i++)
                {
                    var destination = new Span<byte>(pCurrent + sizeof(nint), (int)VK_MAX_EXTENSION_NAME_SIZE);
                    var length = Encoding.UTF8.GetBytes(names[i], destination);

                    pCurrent[sizeof(nuint) + length] = (sbyte)'\0';

                    *(int*)pCurrent = length;
                    pCurrent += sizePerEntry;
                }

                return names.Length;
            }

            static VkDebugReportCallbackEXT TryCreateVulkanDebugReportCallbackExt(VkInstance instance)
            {
                VkDebugReportCallbackEXT vulkanDebugReportCallbackExt;

                var debugReportCallbackCreateInfo = new VkDebugReportCallbackCreateInfoEXT {
                    sType = VK_STRUCTURE_TYPE_DEBUG_REPORT_CALLBACK_CREATE_INFO_EXT,
                    flags = (uint)(VK_DEBUG_REPORT_ERROR_BIT_EXT | VK_DEBUG_REPORT_WARNING_BIT_EXT | VK_DEBUG_REPORT_PERFORMANCE_WARNING_BIT_EXT),
                    pfnCallback = s_vulkanDebugReportCallback,
                };

                // We don't want to fail if creating the debug report callback failed
                var vkCreateDebugReportCallbackEXT = (delegate* unmanaged<IntPtr, VkDebugReportCallbackCreateInfoEXT*, VkAllocationCallbacks*, ulong*, VkResult>)vkGetInstanceProcAddr(instance, VKCREATEDEBUGREPORTCALLBACKEXT_FUNCTION_NAME.AsPointer());
                _ = vkCreateDebugReportCallbackEXT(instance, &debugReportCallbackCreateInfo, null, (ulong*)&vulkanDebugReportCallbackExt);

                return vulkanDebugReportCallbackExt;
            }
        }

        private void DisposeInstance(VkInstance vulkanInstance)
        {
            _state.AssertDisposing();

            if (_vulkanDebugReportCallbackExt != VK_NULL_HANDLE)
            {
                var vkDestroyDebugReportCallbackEXT = (delegate* unmanaged<IntPtr, VkDebugReportCallbackEXT, VkAllocationCallbacks*, void>)vkGetInstanceProcAddr(vulkanInstance, VKDESTROYDEBUGREPORTCALLBACKEXT_FUNCTION_NAME.AsPointer());
                vkDestroyDebugReportCallbackEXT(vulkanInstance, _vulkanDebugReportCallbackExt, null);
            }

            vkDestroyInstance(vulkanInstance, pAllocator: null);
        }

        private ImmutableArray<VulkanGraphicsAdapter> GetGraphicsAdapters()
        {
            _state.ThrowIfDisposedOrDisposing();

            var instance = VulkanInstance;

            uint physicalDeviceCount;
            ThrowExternalExceptionIfNotSuccess(vkEnumeratePhysicalDevices(instance, &physicalDeviceCount, pPhysicalDevices: null), nameof(vkEnumeratePhysicalDevices));

            var physicalDevices = stackalloc VkPhysicalDevice[unchecked((int)physicalDeviceCount)];
            ThrowExternalExceptionIfNotSuccess(vkEnumeratePhysicalDevices(instance, &physicalDeviceCount, (IntPtr*)physicalDevices), nameof(vkEnumeratePhysicalDevices));

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
