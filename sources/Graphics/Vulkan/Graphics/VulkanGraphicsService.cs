// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using TerraFX.Collections;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkDebugUtilsMessageSeverityFlagsEXT;
using static TerraFX.Interop.Vulkan.VkDebugUtilsMessageTypeFlagsEXT;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.VkValidationFeatureEnableEXT;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.AppContextUtilities;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsService : GraphicsService
{
    /// <summary>The engine name for the service.</summary>
    /// <remarks>This defaults to <c>TerraFX</c>.</remarks>
    public static readonly string EngineName = GetAppContextData(
        $"{typeof(VulkanGraphicsService).FullName}.{nameof(EngineName)}",
        defaultValue: "TerraFX"
    );

    /// <summary>The names for extensions which should be optionally enabled for the service..</summary>
    /// <remarks>
    ///     <para>This defaults to <see cref="Array.Empty{T}" />.</para>
    ///     <para>When <see cref="GraphicsService.EnableDebugMode" /> is <c>true</c> this will implicilty include <c>VK_EXT_debug_utils</c> and <c>VK_EXT_validation_features</c>.</para>
    /// </remarks>
    public static readonly string[] OptionalExtensionNames = GetAppContextData(
        $"{typeof(VulkanGraphicsService).FullName}.{nameof(OptionalExtensionNames)}",
        defaultValue: Array.Empty<string>()
    );

    /// <summary>The names for layer which should be optionally enabled for the service..</summary>
    /// <remarks>
    ///     <para>This defaults to <see cref="Array.Empty{T}" />.</para>
    ///     <para>When <see cref="GraphicsService.EnableDebugMode" /> is <c>true</c> this will implicilty include <c>VK_LAYER_KHRONOS_validation</c>.</para>
    /// </remarks>
    public static readonly string[] OptionalLayerNames = GetAppContextData(
        $"{typeof(VulkanGraphicsService).FullName}.{nameof(OptionalLayerNames)}",
        defaultValue: Array.Empty<string>()
    );

    /// <summary>The names for extensions which should be required to succesfully create the service..</summary>
    /// <remarks>This defaults to <see cref="Array.Empty{T}" />.</remarks>
    public static readonly string[] RequiredExtensionNames = GetAppContextData(
        $"{typeof(VulkanGraphicsService).FullName}.{nameof(RequiredExtensionNames)}",
        defaultValue: Array.Empty<string>()
    );

    /// <summary>The names for layers which should be required to succesfully create the service..</summary>
    /// <remarks>This defaults to <see cref="Array.Empty{T}" />.</remarks>
    public static readonly string[] RequiredLayerNames = GetAppContextData(
        $"{typeof(VulkanGraphicsService).FullName}.{nameof(RequiredLayerNames)}",
        defaultValue: Array.Empty<string>()
    );

    private VkInstance _vkInstance;
    private VkDebugUtilsMessengerEXT _vkDebugUtilsMessenger;

    private readonly VkInstanceManualImports _vkInstanceManualImports;

    private ValueList<VulkanGraphicsAdapter> _adapters;

    /// <summary>Initializes a new instance of the <see cref="VulkanGraphicsService" /> class.</summary>
    public VulkanGraphicsService()
    {
        _vkInstance = CreateVkInstance(ref _vkInstanceManualImports);
        _vkDebugUtilsMessenger = CreateVkDebugUtilsMessenger();
        _adapters = GetAdapters();

        static uint EnableVkProperties(sbyte* propertyNames, nuint propertyNamesCount, uint propertySize, string[] requiredNames, string[] optionalNames, out sbyte* requiredNamesBuffer, out sbyte* optionalNamesBuffer, out sbyte** enabledNames)
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

        static uint EnableVkPropertiesByName(sbyte* propertyNames, nuint propertyNamesCount, uint propertySize, sbyte* targetNames, int targetNamesCount, sbyte** enabledNames)
        {
            uint enabledPropertyCount = 0;

            var pPropertyName = propertyNames;
            var pTargetName = targetNames;

            for (var targetNamesIndex = 0; targetNamesIndex < targetNamesCount; targetNamesIndex++)
            {
                var targetNameLength = *(int*)pTargetName;
                pTargetName += sizeof(nuint);
                var targetName = new ReadOnlySpan<sbyte>(pTargetName, targetNameLength + 1);

                for (nuint propertyNamesIndex = 0; propertyNamesIndex < propertyNamesCount; propertyNamesIndex++)
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

        static UnmanagedArray<VkExtensionProperties> GetVkExtensionProperties()
        {
            uint vkExtensionPropertiesCount = 0;
            ThrowExternalExceptionIfNotSuccess(vkEnumerateInstanceExtensionProperties(pLayerName: null, &vkExtensionPropertiesCount, pProperties: null));

            var vkExtensionProperties = new UnmanagedArray<VkExtensionProperties>(vkExtensionPropertiesCount);
            ThrowExternalExceptionIfNotSuccess(vkEnumerateInstanceExtensionProperties(pLayerName: null, &vkExtensionPropertiesCount, vkExtensionProperties.GetPointerUnsafe(0)));
            return vkExtensionProperties;
        }

        static UnmanagedArray<VkLayerProperties> GetVkLayerProperties()
        {
            uint vkLayerPropertiesCount = 0;
            ThrowExternalExceptionIfNotSuccess(vkEnumerateInstanceLayerProperties(&vkLayerPropertiesCount, pProperties: null));

            var vkLayerProperties = new UnmanagedArray<VkLayerProperties>(vkLayerPropertiesCount);
            ThrowExternalExceptionIfNotSuccess(vkEnumerateInstanceLayerProperties(&vkLayerPropertiesCount, vkLayerProperties.GetPointerUnsafe(0)));
            return vkLayerProperties;
        }

        static void InitializeVkDebugUtilsMessengerCreateInfo(out VkDebugUtilsMessengerCreateInfoEXT vkDebugUtilsMessengerCreateInfo)
        {
            vkDebugUtilsMessengerCreateInfo = new VkDebugUtilsMessengerCreateInfoEXT {
                sType = VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT,
                pNext = null,
                flags = 0,
                messageSeverity = VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT,
                messageType = VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT,
                pfnUserCallback = &VulkanDebugUtilsMessengerCallback,
                pUserData = null,
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

        VkDebugUtilsMessengerEXT CreateVkDebugUtilsMessenger()
        {
            var vkDebugUtilsMessenger = VkDebugUtilsMessengerEXT.NULL;
            var vkCreateDebugUtilsMessengerExt = _vkInstanceManualImports.vkCreateDebugUtilsMessengerEXT;

            if (EnableDebugMode && (vkCreateDebugUtilsMessengerExt is not null))
            {
                InitializeVkDebugUtilsMessengerCreateInfo(out var vkDebugUtilsMessengerCreateInfo);
                _ = vkCreateDebugUtilsMessengerExt(_vkInstance, &vkDebugUtilsMessengerCreateInfo, null, &vkDebugUtilsMessenger);
            }

            return vkDebugUtilsMessenger;
        }

        VkInstance CreateVkInstance(ref VkInstanceManualImports vkInstanceManualImports)
        {
            VkInstance vkInstance;

            fixed (sbyte* pEngineName = EngineName.GetUtf8Span())
            {
                var requiredExtensionNames = RequiredExtensionNames;
                var optionalExtensionNames = OptionalExtensionNames;

                if (EnableDebugMode)
                {
                    optionalExtensionNames = optionalExtensionNames.Append("VK_EXT_debug_utils")
                                                                   .Append("VK_EXT_validation_features")
                                                                   .Distinct().ToArray();
                }

                var vkExtensionProperties = GetVkExtensionProperties();
                var enabledVkExtensionPropertiesCount = EnableVkProperties((sbyte*)vkExtensionProperties.GetPointerUnsafe(0), vkExtensionProperties.Length, SizeOf<VkExtensionProperties>(), requiredExtensionNames, optionalExtensionNames, out var requiredExtensionNamesBuffer, out var optionalExtensionNamesBuffer, out var enabledExtensionNames);

                var requiredLayerNames = RequiredLayerNames;
                var optionalLayerNames = OptionalLayerNames;

                if (EnableDebugMode)
                {
                    optionalLayerNames = optionalLayerNames.Append("VK_LAYER_KHRONOS_validation")
                                                           .Distinct().ToArray();
                }

                var vkLayerProperties = GetVkLayerProperties();
                var enabledVkLayerPropertiesCount = EnableVkProperties((sbyte*)vkLayerProperties.GetPointerUnsafe(0), vkLayerProperties.Length, SizeOf<VkLayerProperties>(), requiredLayerNames, optionalLayerNames, out var requiredLayerNamesBuffer, out var optionalLayerNamesBuffer, out var enabledLayerNames);

                var vkApplicationInfo = new VkApplicationInfo {
                    sType = VK_STRUCTURE_TYPE_APPLICATION_INFO,
                    pNext = null,
                    pApplicationName = null,
                    applicationVersion = 1,
                    pEngineName = pEngineName,
                    engineVersion = VK_MAKE_VERSION(0, 1, 0),
                    apiVersion = VK_API_VERSION_1_2,
                };

                var vkInstanceCreateInfo = new VkInstanceCreateInfo {
                    sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
                    pNext = null,
                    flags = 0,
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

                if (EnableDebugMode)
                {
                    InitializeVkDebugUtilsMessengerCreateInfo(out vkDebugUtilsMessengerCreateInfo);
                    vkInstanceCreateInfo.pNext = &vkDebugUtilsMessengerCreateInfo;

                    vkValidationFeaturesEXT = new VkValidationFeaturesEXT {
                        sType = VK_STRUCTURE_TYPE_VALIDATION_FEATURES_EXT,
                        pNext = null,
                        enabledValidationFeatureCount = EnableGpuValidation ? 3u : 0u,
                        pEnabledValidationFeatures = vkValidationFeatureEnables,
                        disabledValidationFeatureCount = 0,
                        pDisabledValidationFeatures = null,

                    };
                    vkDebugUtilsMessengerCreateInfo.pNext = &vkValidationFeaturesEXT;
                }

                var result = vkCreateInstance(&vkInstanceCreateInfo, pAllocator: null, &vkInstance);

                vkExtensionProperties.Dispose();
                vkLayerProperties.Dispose();

                Free(requiredExtensionNamesBuffer);
                Free(optionalExtensionNamesBuffer);
                Free(enabledExtensionNames);

                Free(requiredLayerNamesBuffer);
                Free(optionalLayerNamesBuffer);
                Free(enabledLayerNames);

                ThrowExternalExceptionIfNotSuccess(result, nameof(vkCreateInstance));
            }

            // vkCreateDebugUtilsMessengerEXT
            ReadOnlySpan<sbyte> vkCreateDebugUtilsMessengerEXT = new sbyte[] { 0x76, 0x6B, 0x43, 0x72, 0x65, 0x61, 0x74, 0x65, 0x44, 0x65, 0x62, 0x75, 0x67, 0x55, 0x74, 0x69, 0x6C, 0x73, 0x4D, 0x65, 0x73, 0x73, 0x65, 0x6E, 0x67, 0x65, 0x72, 0x45, 0x58, 0x54, 0x00 };
            vkInstanceManualImports.vkCreateDebugUtilsMessengerEXT = (delegate* unmanaged<VkInstance, VkDebugUtilsMessengerCreateInfoEXT*, VkAllocationCallbacks*, VkDebugUtilsMessengerEXT*, VkResult>)vkGetInstanceProcAddr(vkInstance, vkCreateDebugUtilsMessengerEXT.GetPointer());

            // vkDestroyDebugUtilsMessengerEXT
            ReadOnlySpan<sbyte> vkDestroyDebugUtilsMessengerEXT = new sbyte[] { 0x76, 0x6B, 0x44, 0x65, 0x73, 0x74, 0x72, 0x6F, 0x79, 0x44, 0x65, 0x62, 0x75, 0x67, 0x55, 0x74, 0x69, 0x6C, 0x73, 0x4D, 0x65, 0x73, 0x73, 0x65, 0x6E, 0x67, 0x65, 0x72, 0x45, 0x58, 0x54, 0x00 };
            vkInstanceManualImports.vkDestroyDebugUtilsMessengerEXT = (delegate* unmanaged<VkInstance, VkDebugUtilsMessengerEXT, VkAllocationCallbacks*, void>)vkGetInstanceProcAddr(vkInstance, vkDestroyDebugUtilsMessengerEXT.GetPointer());

            // vkCreateWin32SurfaceKHR
            ReadOnlySpan<sbyte> vkCreateWin32SurfaceKHR = new sbyte[] { 0x76, 0x6B, 0x43, 0x72, 0x65, 0x61, 0x74, 0x65, 0x57, 0x69, 0x6E, 0x33, 0x32, 0x53, 0x75, 0x72, 0x66, 0x61, 0x63, 0x65, 0x4B, 0x48, 0x52, 0x00 };
            vkInstanceManualImports.vkCreateWin32SurfaceKHR = (delegate* unmanaged<VkInstance, VkWin32SurfaceCreateInfoKHR*, VkAllocationCallbacks*, VkSurfaceKHR*, VkResult>)vkGetInstanceProcAddr(vkInstance, vkCreateWin32SurfaceKHR.GetPointer());

            // vkCreateXlibSurfaceKHR
            ReadOnlySpan<sbyte> vkCreateXlibSurfaceKHR = new sbyte[] { 0x76, 0x6B, 0x43, 0x72, 0x65, 0x61, 0x74, 0x65, 0x58, 0x6C, 0x69, 0x62, 0x53, 0x75, 0x72, 0x66, 0x61, 0x63, 0x65, 0x4B, 0x48, 0x52, 0x00 };
            vkInstanceManualImports.vkCreateXlibSurfaceKHR = (delegate* unmanaged<VkInstance, VkXlibSurfaceCreateInfoKHR*, VkAllocationCallbacks*, VkSurfaceKHR*, VkResult>)vkGetInstanceProcAddr(vkInstance, vkCreateXlibSurfaceKHR.GetPointer());

            return vkInstance;
        }

        ValueList<VulkanGraphicsAdapter> GetAdapters()
        {
            var adapters = new ValueList<VulkanGraphicsAdapter>();

            var vkInstance = _vkInstance;

            uint vkPhysicalDeviceCount;
            ThrowExternalExceptionIfNotSuccess(vkEnumeratePhysicalDevices(vkInstance, &vkPhysicalDeviceCount, pPhysicalDevices: null));

            var vkPhysicalDevices = stackalloc VkPhysicalDevice[unchecked((int)vkPhysicalDeviceCount)];
            ThrowExternalExceptionIfNotSuccess(vkEnumeratePhysicalDevices(vkInstance, &vkPhysicalDeviceCount, vkPhysicalDevices));

            for (var index = 0; index < vkPhysicalDeviceCount; index++)
            {
                var adapter = new VulkanGraphicsAdapter(this, vkPhysicalDevices[index]);
                adapters.Add(adapter);
            }

            return adapters;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsService" /> class.</summary>
    ~VulkanGraphicsService() => Dispose(isDisposing: false);

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkEnumeratePhysicalDevices(VkInstance, uint*, VkPhysicalDevice*)" /> failed.</exception>
    public override IEnumerable<VulkanGraphicsAdapter> Adapters => _adapters;

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkInstance" /> for the service.</summary>
    public VkInstance VkInstance => _vkInstance;

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
        if (isDisposing)
        {
            _adapters.Dispose();
        }

        var vkInstance = _vkInstance;

        DisposeVkDebugUtilsMessenger(vkInstance, _vkDebugUtilsMessenger, in _vkInstanceManualImports);
        _vkDebugUtilsMessenger = VkDebugUtilsMessengerEXT.NULL;

        DisposeVkInstance(vkInstance);
        _vkInstance = VkInstance.NULL;

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
    protected override void SetNameUnsafe(string value)
    {
    }
}
