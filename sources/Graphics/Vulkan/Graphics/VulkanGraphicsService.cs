// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkDebugUtilsMessageSeverityFlagsEXT;
using static TerraFX.Interop.Vulkan.VkDebugUtilsMessageTypeFlagsEXT;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.VkValidationFeatureEnableEXT;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsService : GraphicsService
{
    /// <summary>The default engine name used if <see cref="EngineNameDataName" /> was not set.</summary>
    public const string DefaultEngineName = "TerraFX";

    /// <summary>The name of a data value that controls the engine name for <see cref="VkInstance" />.</summary>
    /// <remarks>
    ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
    ///     <para>Setting this data value has no affect on services thathave already been created.</para>
    ///     <para>This data value is interpreted as a string.</para>
    /// </remarks>
    public const string EngineNameDataName = "TerraFX.Graphics.VulkanGraphicsService.EngineName";

    /// <summary>The name of a data value that controls the optional extensions for <see cref="VkInstance" />.</summary>
    /// <remarks>
    ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
    ///     <para>Setting this data value has no affect on services thathave already been created.</para>
    ///     <para>This data value is interpreted as a string of semicolon separated values.</para>
    /// </remarks>
    public const string OptionalExtensionNamesDataName = "TerraFX.Graphics.VulkanGraphicsService.OptionalExtensionNames";

    /// <summary>The name of a data value that controls the optional layers for <see cref="VkInstance" />.</summary>
    /// <remarks>
    ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
    ///     <para>Setting this data value has no affect on services thathave already been created.</para>
    ///     <para>This data value is interpreted as a string of semicolon separated values.</para>
    /// </remarks>
    public const string OptionalLayerNamesDataName = "TerraFX.Graphics.VulkanGraphicsService.OptionalLayerNames";

    /// <summary>The name of a data value that controls the required extensions for <see cref="VkInstance" />.</summary>
    /// <remarks>
    ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
    ///     <para>Setting this data value has no affect on services thathave already been created.</para>
    ///     <para>This data value is interpreted as a string of semicolon separated values.</para>
    /// </remarks>
    public const string RequiredExtensionNamesDataName = "TerraFX.Graphics.VulkanGraphicsService.RequiredExtensionNames";

    /// <summary>The name of a data value that controls the required layers for <see cref="VkInstance" />.</summary>
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
    private readonly VkInstanceManualImports _vkInstanceManualImports;

    private readonly ImmutableArray<VulkanGraphicsAdapter> _adapters;
    private readonly VkInstance _vkInstance;
    private readonly VkDebugUtilsMessengerEXT _vkDebugUtilsMessenger;

    /// <summary>Initializes a new instance of the <see cref="VulkanGraphicsService" /> class.</summary>
    public VulkanGraphicsService()
    {
        if (EnableDebugMode)
        {
            var optionalExtensionNamesData = AppContext.GetData(OptionalExtensionNamesDataName) as string;
            optionalExtensionNamesData += ";VK_EXT_debug_utils;VK_EXT_validation_features";
            AppDomain.CurrentDomain.SetData(OptionalExtensionNamesDataName, optionalExtensionNamesData);

            var optionalLayerNamesData = AppContext.GetData(OptionalLayerNamesDataName) as string;
            optionalLayerNamesData += ";VK_LAYER_KHRONOS_validation";
            AppDomain.CurrentDomain.SetData(OptionalLayerNamesDataName, optionalLayerNamesData);
        }

        var engineName = GetEngineName();
        _engineName = engineName;

        var requiredExtensionNames = GetNames(RequiredExtensionNamesDataName);
        _requiredExtensionNames = requiredExtensionNames;

        var optionalExtensionNames = GetNames(OptionalExtensionNamesDataName);
        _optionalExtensionNames = optionalExtensionNames;

        var requiredLayerNames = GetNames(RequiredLayerNamesDataName);
        _requiredLayerNames = requiredLayerNames;

        var optionalLayerNames = GetNames(OptionalLayerNamesDataName);
        _optionalLayerNames = optionalLayerNames;

        var vkInstance = CreateVkInstance(engineName, requiredExtensionNames, optionalExtensionNames, requiredLayerNames, optionalLayerNames, EnableDebugMode);
        _vkInstance = vkInstance;

        InitializeVkInstanceManualImports(vkInstance, ref _vkInstanceManualImports);

        _vkDebugUtilsMessenger = CreateVkDebugUtilsMessenger(vkInstance, in _vkInstanceManualImports);

        _adapters = GetGraphicsAdapters(this, vkInstance);

        static VkDebugUtilsMessengerEXT CreateVkDebugUtilsMessenger(VkInstance vkInstance, in VkInstanceManualImports vkInstanceManualImports)
        {
            var vkDebugUtilsMessenger = VkDebugUtilsMessengerEXT.NULL;

            if (EnableDebugMode && (vkInstanceManualImports.vkCreateDebugUtilsMessengerEXT is not null))
            {
                InitializeVkDebugUtilsMessengerCreateInfo(out var vkDebugUtilsMessengerCreateInfo);
                _ = vkInstanceManualImports.vkCreateDebugUtilsMessengerEXT(vkInstance, &vkDebugUtilsMessengerCreateInfo, null, &vkDebugUtilsMessenger);
            }

            return vkDebugUtilsMessenger;
        }

        static VkInstance CreateVkInstance(string engineName, string[] requiredExtensionNames, string[] optionalExtensionNames, string[] requiredLayerNames, string[] optionalLayerNames, bool enableDebugMode)
        {
            sbyte* requiredExtensionNamesBuffer = null;
            sbyte* optionalExtensionNamesBuffer = null;
            sbyte** enabledExtensionNames = null;

            sbyte* requiredLayerNamesBuffer = null;
            sbyte* optionalLayerNamesBuffer = null;
            sbyte** enabledLayerNames = null;

            try
            {
                VkInstance vkInstance;

                uint enabledVkExtensionPropertiesCount;
                var vkExtensionProperties = GetVkExtensionProperties();

                fixed (VkExtensionProperties* pVkExtensionProperties = vkExtensionProperties)
                {
                    enabledVkExtensionPropertiesCount = EnableVkProperties((sbyte*)pVkExtensionProperties, vkExtensionProperties.Length, SizeOf<VkExtensionProperties>(), requiredExtensionNames, optionalExtensionNames, out requiredExtensionNamesBuffer, out optionalExtensionNamesBuffer, out enabledExtensionNames);
                }

                uint enabledVkLayerPropertiesCount;
                var vkLayerProperties = GetVkLayerProperties();

                fixed (VkLayerProperties* pVkLayerProperties = vkLayerProperties)
                {
                    enabledVkLayerPropertiesCount = EnableVkProperties((sbyte*)pVkLayerProperties, vkLayerProperties.Length, SizeOf<VkLayerProperties>(), requiredLayerNames, optionalLayerNames, out requiredLayerNamesBuffer, out optionalLayerNamesBuffer, out enabledLayerNames);
                }

                fixed (sbyte* pEngineName = engineName.GetUtf8Span())
                {
                    var vkApplicationInfo = new VkApplicationInfo {
                        sType = VK_STRUCTURE_TYPE_APPLICATION_INFO,
                        applicationVersion = 1,
                        pEngineName = pEngineName,
                        engineVersion = VK_MAKE_VERSION(0, 1, 0),
                        apiVersion = VK_API_VERSION_1_2,
                    };

                    var vkInstanceCreateInfo = new VkInstanceCreateInfo {
                        sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
                        pApplicationInfo = &vkApplicationInfo,
                        enabledLayerCount = enabledVkLayerPropertiesCount,
                        ppEnabledLayerNames = enabledLayerNames,
                        enabledExtensionCount = enabledVkExtensionPropertiesCount,
                        ppEnabledExtensionNames = enabledExtensionNames,
                    };

                    Unsafe.SkipInit<VkDebugUtilsMessengerCreateInfoEXT>(out var vkDebugUtilsMessengerCreateInfo);
                    Unsafe.SkipInit<VkValidationFeaturesEXT>(out var vkValidationFeaturesEXT);

                    var vkValidationFeatureEnables = stackalloc VkValidationFeatureEnableEXT[3] {
                        VK_VALIDATION_FEATURE_ENABLE_GPU_ASSISTED_EXT,
                        VK_VALIDATION_FEATURE_ENABLE_BEST_PRACTICES_EXT,
                        VK_VALIDATION_FEATURE_ENABLE_SYNCHRONIZATION_VALIDATION_EXT
                    };

                    if (enableDebugMode)
                    {
                        InitializeVkDebugUtilsMessengerCreateInfo(out vkDebugUtilsMessengerCreateInfo);
                        vkInstanceCreateInfo.pNext = &vkDebugUtilsMessengerCreateInfo;

                        vkValidationFeaturesEXT = new VkValidationFeaturesEXT {
                            sType = VK_STRUCTURE_TYPE_VALIDATION_FEATURES_EXT,
                            enabledValidationFeatureCount = EnableGpuValidation ? 3u : 0u,
                            pEnabledValidationFeatures = vkValidationFeatureEnables,

                        };
                        vkDebugUtilsMessengerCreateInfo.pNext = &vkValidationFeaturesEXT;
                    }

                    ThrowExternalExceptionIfNotSuccess(vkCreateInstance(&vkInstanceCreateInfo, pAllocator: null, &vkInstance));
                }

                return vkInstance;
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
        }

        static uint EnableVkProperties(sbyte* propertyNames, int propertyNamesCount, uint propertySize, string[] requiredNames, string[] optionalNames, out sbyte* requiredNamesBuffer, out sbyte* optionalNamesBuffer, out sbyte** enabledNames)
        {
            var requiredNamesCount = MarshalNames(requiredNames, out requiredNamesBuffer);
            var optionalNamesCount = MarshalNames(optionalNames, out optionalNamesBuffer);

            enabledNames = (sbyte**)AllocateArray<nuint>((nuint)(requiredNamesCount + optionalNamesCount));

            var enabledPropertyCount = EnableVkPropertiesByName(propertyNames, propertyNamesCount, propertySize, requiredNamesBuffer, requiredNamesCount, enabledNames);

            if (enabledPropertyCount != requiredNamesCount)
            {
                ThrowForMissingFeature();
            }
            enabledPropertyCount += EnableVkPropertiesByName(propertyNames, propertyNamesCount, propertySize, optionalNamesBuffer, optionalNamesCount, enabledNames + enabledPropertyCount);

            return enabledPropertyCount;
        }

        static uint EnableVkPropertiesByName(sbyte* propertyNames, int propertyNamesCount, uint propertySize, sbyte* targetNames, int targetNamesCount, sbyte** enabledNames)
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

        static ImmutableArray<VulkanGraphicsAdapter> GetGraphicsAdapters(VulkanGraphicsService service, VkInstance vkInstance)
        {
            uint vkPhysicalDeviceCount;
            ThrowExternalExceptionIfNotSuccess(vkEnumeratePhysicalDevices(vkInstance, &vkPhysicalDeviceCount, pPhysicalDevices: null));

            var vkPhysicalDevices = stackalloc VkPhysicalDevice[unchecked((int)vkPhysicalDeviceCount)];
            ThrowExternalExceptionIfNotSuccess(vkEnumeratePhysicalDevices(vkInstance, &vkPhysicalDeviceCount, vkPhysicalDevices));

            var adapters = ImmutableArray.CreateBuilder<VulkanGraphicsAdapter>(unchecked((int)vkPhysicalDeviceCount));

            for (uint index = 0; index < vkPhysicalDeviceCount; index++)
            {
                var adapter = new VulkanGraphicsAdapter(service, vkPhysicalDevices[index]);
                adapters.Add(adapter);
            }

            return adapters.ToImmutable();
        }

        static VkExtensionProperties[] GetVkExtensionProperties()
        {
            uint vkExtensionPropertiesCount = 0;
            ThrowExternalExceptionIfNotSuccess(vkEnumerateInstanceExtensionProperties(pLayerName: null, &vkExtensionPropertiesCount, pProperties: null));

            var vkExtensionProperties = new VkExtensionProperties[vkExtensionPropertiesCount];

            fixed (VkExtensionProperties* pExtensionProperties = vkExtensionProperties)
            {
                ThrowExternalExceptionIfNotSuccess(vkEnumerateInstanceExtensionProperties(pLayerName: null, &vkExtensionPropertiesCount, pExtensionProperties));
            }

            return vkExtensionProperties;
        }

        static VkLayerProperties[] GetVkLayerProperties()
        {
            uint vkLayerPropertiesCount = 0;
            ThrowExternalExceptionIfNotSuccess(vkEnumerateInstanceLayerProperties(&vkLayerPropertiesCount, pProperties: null));

            var vkLayerProperties = new VkLayerProperties[vkLayerPropertiesCount];

            fixed (VkLayerProperties* pLayerProperties = vkLayerProperties)
            {
                ThrowExternalExceptionIfNotSuccess(vkEnumerateInstanceLayerProperties(&vkLayerPropertiesCount, pLayerProperties));
            }

            return vkLayerProperties;
        }

        static string[] GetNames(string dataName)
        {
            var namesData = AppContext.GetData(dataName) as string ?? string.Empty;
            var names = namesData.Split(';', StringSplitOptions.RemoveEmptyEntries);
            return names.Distinct().ToArray();
        }

        static void InitializeVkDebugUtilsMessengerCreateInfo(out VkDebugUtilsMessengerCreateInfoEXT vkDebugUtilsMessengerCreateInfo)
        {
            vkDebugUtilsMessengerCreateInfo = new VkDebugUtilsMessengerCreateInfoEXT {
                sType = VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT,
                messageSeverity = VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT,
                messageType = VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT,
                pfnUserCallback = &VulkanDebugUtilsMessengerCallback,
            };
        }

        static void InitializeVkInstanceManualImports(VkInstance vkInstance, ref VkInstanceManualImports vkInstanceManualImports)
        {
            ReadOnlySpan<sbyte> vkCreateDebugUtilsMessengerEXT = new sbyte[] { 0x76, 0x6B, 0x43, 0x72, 0x65, 0x61, 0x74, 0x65, 0x44, 0x65, 0x62, 0x75, 0x67, 0x55, 0x74, 0x69, 0x6C, 0x73, 0x4D, 0x65, 0x73, 0x73, 0x65, 0x6E, 0x67, 0x65, 0x72, 0x45, 0x58, 0x54, 0x00 };
            vkInstanceManualImports.vkCreateDebugUtilsMessengerEXT = (delegate* unmanaged<VkInstance, VkDebugUtilsMessengerCreateInfoEXT*, VkAllocationCallbacks*, VkDebugUtilsMessengerEXT*, VkResult>)vkGetInstanceProcAddr(vkInstance, vkCreateDebugUtilsMessengerEXT.GetPointer());

            ReadOnlySpan<sbyte> vkDestroyDebugUtilsMessengerEXT = new sbyte[] { 0x76, 0x6B, 0x44, 0x65, 0x73, 0x74, 0x72, 0x6F, 0x79, 0x44, 0x65, 0x62, 0x75, 0x67, 0x55, 0x74, 0x69, 0x6C, 0x73, 0x4D, 0x65, 0x73, 0x73, 0x65, 0x6E, 0x67, 0x65, 0x72, 0x45, 0x58, 0x54, 0x00 };
            vkInstanceManualImports.vkDestroyDebugUtilsMessengerEXT = (delegate* unmanaged<VkInstance, VkDebugUtilsMessengerEXT, VkAllocationCallbacks*, void>)vkGetInstanceProcAddr(vkInstance, vkDestroyDebugUtilsMessengerEXT.GetPointer());

            ReadOnlySpan<sbyte> vkCreateWin32SurfaceKHR = new sbyte[] { 0x76, 0x6B, 0x43, 0x72, 0x65, 0x61, 0x74, 0x65, 0x57, 0x69, 0x6E, 0x33, 0x32, 0x53, 0x75, 0x72, 0x66, 0x61, 0x63, 0x65, 0x4B, 0x48, 0x52, 0x00 };
            vkInstanceManualImports.vkCreateWin32SurfaceKHR = (delegate* unmanaged<VkInstance, VkWin32SurfaceCreateInfoKHR*, VkAllocationCallbacks*, VkSurfaceKHR*, VkResult>)vkGetInstanceProcAddr(vkInstance, vkCreateWin32SurfaceKHR.GetPointer());

            ReadOnlySpan<sbyte> vkCreateXlibSurfaceKHR = new sbyte[] { 0x76, 0x6B, 0x43, 0x72, 0x65, 0x61, 0x74, 0x65, 0x58, 0x6C, 0x69, 0x62, 0x53, 0x75, 0x72, 0x66, 0x61, 0x63, 0x65, 0x4B, 0x48, 0x52, 0x00 };
            vkInstanceManualImports.vkCreateXlibSurfaceKHR = (delegate* unmanaged<VkInstance, VkXlibSurfaceCreateInfoKHR*, VkAllocationCallbacks*, VkSurfaceKHR*, VkResult>)vkGetInstanceProcAddr(vkInstance, vkCreateXlibSurfaceKHR.GetPointer());
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
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsService" /> class.</summary>
    ~VulkanGraphicsService() => Dispose(isDisposing: false);

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkEnumeratePhysicalDevices(VkInstance, uint*, VkPhysicalDevice*)" /> failed.</exception>
    public override IEnumerable<VulkanGraphicsAdapter> Adapters => _adapters;

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkInstance" /> for the service.</summary>
    public VkInstance VkInstance
    {
        get
        {
            AssertNotDisposed();
            return _vkInstance;
        }
    }

    /// <summary>Gets the methods that must be manually imported for <see cref="VkInstance" />.</summary>
    public ref readonly VkInstanceManualImports VkInstanceManualImports => ref _vkInstanceManualImports;

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
        var vkInstance = _vkInstance;

        DisposeVkDebugUtilsMessenger(vkInstance, _vkDebugUtilsMessenger, in _vkInstanceManualImports);
        DisposeVkInstance(vkInstance);

        static void DisposeVkDebugUtilsMessenger(VkInstance vkInstance, VkDebugUtilsMessengerEXT vkDebugUtilsMessenger, in VkInstanceManualImports vkInstanceManualImports)
        {
            if (vkDebugUtilsMessenger != VkDebugUtilsMessengerEXT.NULL)
            {
                vkInstanceManualImports.vkDestroyDebugUtilsMessengerEXT(vkInstance, vkDebugUtilsMessenger, null);
            }
        }

        static void DisposeVkInstance(VkInstance vkInstance)
        {
            if (vkInstance != VkInstance.NULL)
            {
                vkDestroyInstance(vkInstance, pAllocator: null);
            }
        }
    }

    /// <inheritdoc />
    protected override void SetNameInternal(string value)
    {
    }
}
