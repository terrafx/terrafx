// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkDebugUtilsMessageSeverityFlagsEXT;
using static TerraFX.Interop.Vulkan.VkDebugUtilsMessageTypeFlagsEXT;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsService : GraphicsService
    {
        /// <summary>The default engine name used if <see cref="EngineNameDataName" /> was not set.</summary>
        public const string DefaultEngineName = "TerraFX";

        /// <summary>The name of a data value that controls the engine name for <see cref="VulkanInstance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on services thathave already been created.</para>
        ///     <para>This data value is interpreted as a string.</para>
        /// </remarks>
        public const string EngineNameDataName = "TerraFX.Graphics.VulkanGraphicsService.EngineName";

        /// <summary>The name of a data value that controls the optional extensions for <see cref="VulkanInstance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on services thathave already been created.</para>
        ///     <para>This data value is interpreted as a string of semicolon separated values.</para>
        /// </remarks>
        public const string OptionalExtensionNamesDataName = "TerraFX.Graphics.VulkanGraphicsService.OptionalExtensionNames";

        /// <summary>The name of a data value that controls the optional layers for <see cref="VulkanInstance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on services thathave already been created.</para>
        ///     <para>This data value is interpreted as a string of semicolon separated values.</para>
        /// </remarks>
        public const string OptionalLayerNamesDataName = "TerraFX.Graphics.VulkanGraphicsService.OptionalLayerNames";

        /// <summary>The name of a data value that controls the required extensions for <see cref="VulkanInstance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on services thathave already been created.</para>
        ///     <para>This data value is interpreted as a string of semicolon separated values.</para>
        /// </remarks>
        public const string RequiredExtensionNamesDataName = "TerraFX.Graphics.VulkanGraphicsService.RequiredExtensionNames";

        /// <summary>The name of a data value that controls the required layers for <see cref="VulkanInstance" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on services thathave already been created.</para>
        ///     <para>This data value is interpreted as a string of semicolon separated values.</para>
        /// </remarks>
        public const string RequiredLayerNamesDataName = "TerraFX.Graphics.VulkanGraphicsService.RequiredLayerNames";

        private readonly string _engineName;
        private readonly string[] _requiredExtensionNames;
        private readonly string[] _optionalExtensionNames;
        private readonly string[] _requiredLayerNames;
        private readonly string[] _optionalLayerNames;

        private ValueLazy<VkInstance> _vulkanInstance;
        private ValueLazy<ImmutableArray<VulkanGraphicsAdapter>> _adapters;

        private VkDebugUtilsMessengerEXT _vulkanDebugUtilsMessenger;

        private VolatileState _state;

        /// <summary>Initializes a new instance of the <see cref="VulkanGraphicsService" /> class.</summary>
        public VulkanGraphicsService()
        {
            if (DebugModeEnabled)
            {
                var optionalExtensionNamesData = AppContext.GetData(OptionalExtensionNamesDataName) as string;
                optionalExtensionNamesData += ";VK_EXT_debug_utils";
                AppDomain.CurrentDomain.SetData(OptionalExtensionNamesDataName, optionalExtensionNamesData);

                var optionalLayerNamesData = AppContext.GetData(OptionalLayerNamesDataName) as string;
                optionalLayerNamesData += ";VK_LAYER_KHRONOS_validation";
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

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsService" /> class.</summary>
        ~VulkanGraphicsService() => Dispose(isDisposing: false);

        // vkCreateDebugUtilsMessengerEXT
        private static ReadOnlySpan<sbyte> VKCREATEDEBUGUTILSMESSENGEREXT_FUNCTION_NAME => new sbyte[] { 0x76, 0x6B, 0x43, 0x72, 0x65, 0x61, 0x74, 0x65, 0x44, 0x65, 0x62, 0x75, 0x67, 0x55, 0x74, 0x69, 0x6C, 0x73, 0x4D, 0x65, 0x73, 0x73, 0x65, 0x6E, 0x67, 0x65, 0x72, 0x45, 0x58, 0x54, 0x00 };

        // vkDestroyDebugUtilsMessengerEXT
        private static ReadOnlySpan<sbyte> VKDESTROYDEBUGUTILSMESSENGEREXT_FUNCTION_NAME => new sbyte[] { 0x76, 0x6B, 0x44, 0x65, 0x73, 0x74, 0x72, 0x6F, 0x79, 0x44, 0x65, 0x62, 0x75, 0x67, 0x55, 0x74, 0x69, 0x6C, 0x73, 0x4D, 0x65, 0x73, 0x73, 0x65, 0x6E, 0x67, 0x65, 0x72, 0x45, 0x58, 0x54, 0x00 };

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkEnumeratePhysicalDevices(VkInstance, uint*, VkPhysicalDevice*)" /> failed.</exception>
        public override IEnumerable<VulkanGraphicsAdapter> Adapters => _adapters.Value;

        /// <summary>Gets the underlying <see cref="VkInstance" /> for the service.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateInstance(VkInstanceCreateInfo*, VkAllocationCallbacks*, VkInstance*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The service has been disposed.</exception>
        public VkInstance VulkanInstance => _vulkanInstance.Value;

        [UnmanagedCallersOnly]
        private static VkBool32 VulkanDebugUtilsMessengerCallback(VkDebugUtilsMessageSeverityFlagsEXT messageSeverity, VkDebugUtilsMessageTypeFlagsEXT messageTypes, VkDebugUtilsMessengerCallbackDataEXT* pCallbackData, void* pUserData)
        {
            var message = GetUtf8Span(pCallbackData->pMessage).GetString();
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
            ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsService));

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

                fixed (sbyte* engineName = _engineName.GetUtf8Span())
                {
                    var applicationInfo = new VkApplicationInfo {
                        sType = VK_STRUCTURE_TYPE_APPLICATION_INFO,
                        applicationVersion = 1,
                        pEngineName = engineName,
                        engineVersion = VK_MAKE_VERSION(0, 1, 0),
                        apiVersion = VK_API_VERSION_1_2,
                    };

                    var instanceCreateInfo = new VkInstanceCreateInfo {
                        sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
                        pApplicationInfo = &applicationInfo,
                        enabledLayerCount = enabledLayerCount,
                        ppEnabledLayerNames = enabledLayerNames,
                        enabledExtensionCount = enabledExtensionCount,
                        ppEnabledExtensionNames = enabledExtensionNames,
                    };

                    InitializeVulkanDebugUtilsMessengerCreateInfo(out var debugUtilsMessengerCreateInfo);

                    if (DebugModeEnabled)
                    {
                        instanceCreateInfo.pNext = &debugUtilsMessengerCreateInfo;
                    }

                    ThrowExternalExceptionIfNotSuccess(vkCreateInstance(&instanceCreateInfo, pAllocator: null, &vulkanInstance), nameof(vkCreateInstance));
                }

                if (DebugModeEnabled)
                {
                    _vulkanDebugUtilsMessenger = TryCreateVulkanDebugUtilsMessengerExt(vulkanInstance);
                }

                return vulkanInstance;
            }
            finally
            {
                Free(enabledLayerNames);
                Free(optionalLayerNamesBuffer);
                Free(requiredLayerNamesBuffer);
                Free(enabledExtensionNames);
                Free(optionalExtensionNamesBuffer);
                Free(requiredExtensionNamesBuffer);
            }

            static uint EnableProperties(sbyte* propertyNames, int propertyNamesCount, int propertySize, string[] requiredNames, string[] optionalNames, out sbyte* requiredNamesBuffer, out sbyte* optionalNamesBuffer, out sbyte** enabledNames)
            {
                var requiredNamesCount = MarshalNames(requiredNames, out requiredNamesBuffer);
                var optionalNamesCount = MarshalNames(optionalNames, out optionalNamesBuffer);

                enabledNames = (sbyte**)AllocateArray<nuint>((nuint)(requiredNamesCount + optionalNamesCount));

                var enabledPropertyCount = EnablePropertiesByName(propertyNames, propertyNamesCount, propertySize, requiredNamesBuffer, requiredNamesCount, enabledNames);

                if (enabledPropertyCount != requiredNamesCount)
                {
                    ThrowForMissingFeature();
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

            static void InitializeVulkanDebugUtilsMessengerCreateInfo(out VkDebugUtilsMessengerCreateInfoEXT vulkanDebugUtilsMessengerCreateInfo)
            {
                vulkanDebugUtilsMessengerCreateInfo = new VkDebugUtilsMessengerCreateInfoEXT {
                    sType = VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT,
                    messageSeverity = VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT,
                    messageType = VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT,
                    pfnUserCallback = &VulkanDebugUtilsMessengerCallback,
                };
            }

            static int MarshalNames(string[] names, out sbyte* namesBuffer)
            {
                nuint sizePerEntry = SizeOf<nuint>() + VK_MAX_EXTENSION_NAME_SIZE;
                namesBuffer = AllocateArray<sbyte>((nuint)names.Length * sizePerEntry);

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

            static VkDebugUtilsMessengerEXT TryCreateVulkanDebugUtilsMessengerExt(VkInstance instance)
            {
                VkDebugUtilsMessengerEXT vulkanDebugUtilsMessenger;

                InitializeVulkanDebugUtilsMessengerCreateInfo(out var debugUtilsMessengerCreateInfo);

                // We don't want to fail if creating the debug utils messenger failed
                var vkCreateDebugUtilsMessengerEXT = (delegate* unmanaged<IntPtr, VkDebugUtilsMessengerCreateInfoEXT*, VkAllocationCallbacks*, ulong*, VkResult>)vkGetInstanceProcAddr(instance, VKCREATEDEBUGUTILSMESSENGEREXT_FUNCTION_NAME.GetPointer());
                _ = vkCreateDebugUtilsMessengerEXT(instance, &debugUtilsMessengerCreateInfo, null, (ulong*)&vulkanDebugUtilsMessenger);

                return vulkanDebugUtilsMessenger;
            }
        }

        private void DisposeInstance(VkInstance vulkanInstance)
        {
            AssertDisposing(_state);

            if (_vulkanDebugUtilsMessenger != VkDebugUtilsMessengerEXT.NULL)
            {
                var vkDestroyDebugUtilsMessengerEXT = (delegate* unmanaged<IntPtr, VkDebugUtilsMessengerEXT, VkAllocationCallbacks*, void>)vkGetInstanceProcAddr(vulkanInstance, VKDESTROYDEBUGUTILSMESSENGEREXT_FUNCTION_NAME.GetPointer());
                vkDestroyDebugUtilsMessengerEXT(vulkanInstance, _vulkanDebugUtilsMessenger, null);
            }

            vkDestroyInstance(vulkanInstance, pAllocator: null);
        }

        private ImmutableArray<VulkanGraphicsAdapter> GetGraphicsAdapters()
        {
            ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsService));

            var instance = VulkanInstance;

            uint physicalDeviceCount;
            ThrowExternalExceptionIfNotSuccess(vkEnumeratePhysicalDevices(instance, &physicalDeviceCount, pPhysicalDevices: null), nameof(vkEnumeratePhysicalDevices));

            var physicalDevices = stackalloc VkPhysicalDevice[unchecked((int)physicalDeviceCount)];
            ThrowExternalExceptionIfNotSuccess(vkEnumeratePhysicalDevices(instance, &physicalDeviceCount, physicalDevices), nameof(vkEnumeratePhysicalDevices));

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
