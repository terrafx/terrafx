// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.	

using System.Runtime.CompilerServices;
using TerraFX.Graphics;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Utilities;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_LIST_TYPE;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Interop.DirectX.DXGI_FORMAT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Utilities;

internal static unsafe partial class D3D12Utilities
{
    private static readonly DXGI_FORMAT[] s_dxgiFormatMap = new DXGI_FORMAT[(int)GraphicsFormat.COUNT__] {
        DXGI_FORMAT_UNKNOWN,                    // Unknown

        DXGI_FORMAT_R32G32B32A32_FLOAT,         // R32G32B32A32_SFLOAT
        DXGI_FORMAT_R32G32B32A32_UINT,          // R32G32B32A32_UINT
        DXGI_FORMAT_R32G32B32A32_SINT,          // R32G32B32A32_SINT

        DXGI_FORMAT_R32G32B32_FLOAT,            // R32G32B32_SFLOAT
        DXGI_FORMAT_R32G32B32_UINT,             // R32G32B32_UINT
        DXGI_FORMAT_R32G32B32_SINT,             // R32G32B32_SINT

        DXGI_FORMAT_R16G16B16A16_FLOAT,         // R16G16B16A16_SFLOAT
        DXGI_FORMAT_R16G16B16A16_UNORM,         // R16G16B16A16_UNORM
        DXGI_FORMAT_R16G16B16A16_UINT,          // R16G16B16A16_UINT 
        DXGI_FORMAT_R16G16B16A16_SNORM,         // R16G16B16A16_SNORM
        DXGI_FORMAT_R16G16B16A16_SINT,          // R16G16B16A16_SINT

        DXGI_FORMAT_R32G32_FLOAT,               // R32G32_SFLOAT
        DXGI_FORMAT_R32G32_UINT,                // R32G32_UINT
        DXGI_FORMAT_R32G32_SINT,                // R32G32_SINT

        DXGI_FORMAT_D32_FLOAT_S8X24_UINT,       // D32_SFLOAT_S8X24_UINT

        DXGI_FORMAT_R10G10B10A2_UNORM,          // R10G10B10A2_UNORM
        DXGI_FORMAT_R10G10B10A2_UINT,           // R10G10B10A2_UINT

        DXGI_FORMAT_R11G11B10_FLOAT,            // R11G11B10_UFLOAT

        DXGI_FORMAT_R8G8B8A8_UNORM,             // R8G8B8A8_UNORM
        DXGI_FORMAT_R8G8B8A8_UNORM_SRGB,        // R8G8B8A8_SRGB
        DXGI_FORMAT_R8G8B8A8_UINT,              // R8G8B8A8_UINT
        DXGI_FORMAT_R8G8B8A8_SNORM,             // R8G8B8A8_SNORM
        DXGI_FORMAT_R8G8B8A8_SINT,              // R8G8B8A8_SINT

        DXGI_FORMAT_R16G16_FLOAT,               // R16G16_SFLOAT
        DXGI_FORMAT_R16G16_UNORM,               // R16G16_UNORM
        DXGI_FORMAT_R16G16_UINT,                // R16G16_UINT
        DXGI_FORMAT_R16G16_SNORM,               // R16G16_SNORM
        DXGI_FORMAT_R16G16_SINT,                // R16G16_SINT

        DXGI_FORMAT_D32_FLOAT,                  // D32_SFLOAT

        DXGI_FORMAT_R32_FLOAT,                  // R32_SFLOAT
        DXGI_FORMAT_R32_UINT,                   // R32_UINT
        DXGI_FORMAT_R32_SINT,                   // R32_SINT

        DXGI_FORMAT_D24_UNORM_S8_UINT,          // D24_UNORM_S8_UINT

        DXGI_FORMAT_R8G8_UNORM,                 // R8G8_UNORM
        DXGI_FORMAT_R8G8_UINT,                  // R8G8_UINT
        DXGI_FORMAT_R8G8_SNORM,                 // R8G8_SNORM
        DXGI_FORMAT_R8G8_SINT,                  // R8G8_SINT

        DXGI_FORMAT_D16_UNORM,                  // D16_UNORM

        DXGI_FORMAT_R16_FLOAT,                  // R16_SFLOAT
        DXGI_FORMAT_R16_UNORM,                  // R16_UNORM
        DXGI_FORMAT_R16_UINT,                   // R16_UINT 
        DXGI_FORMAT_R16_SNORM,                  // R16_SNORM
        DXGI_FORMAT_R16_SINT,                   // R16_SINT

        DXGI_FORMAT_R8_UNORM,                   // R8_UNORM
        DXGI_FORMAT_R8_UINT,                    // R8_UINT
        DXGI_FORMAT_R8_SNORM,                   // R8_SNORM
        DXGI_FORMAT_R8_SINT,                    // R8_SINT

        DXGI_FORMAT_R9G9B9E5_SHAREDEXP,         // R9G9B9E5_UFLOAT

        DXGI_FORMAT_R8G8_B8G8_UNORM,            // R8G8B8G8_UNORM
        DXGI_FORMAT_G8R8_G8B8_UNORM,            // G8R8G8B8_UNORM

        DXGI_FORMAT_BC1_UNORM,                  // BC1_UNORM
        DXGI_FORMAT_BC1_UNORM_SRGB,             // BC1_UNORM_SRGB

        DXGI_FORMAT_BC2_UNORM,                  // BC2_UNORM
        DXGI_FORMAT_BC2_UNORM_SRGB,             // BC2_UNORM_SRGB

        DXGI_FORMAT_BC3_UNORM,                  // BC3_UNORM
        DXGI_FORMAT_BC3_UNORM_SRGB,             // BC3_UNORM_SRGB

        DXGI_FORMAT_BC4_UNORM,                  // BC4_UNORM
        DXGI_FORMAT_BC4_SNORM,                  // BC4_SNORM

        DXGI_FORMAT_BC5_UNORM,                  // BC5_UNORM
        DXGI_FORMAT_BC5_SNORM,                  // BC5_SNORM

        DXGI_FORMAT_B5G6R5_UNORM,               // B5G6R5_UNORM

        DXGI_FORMAT_B5G5R5A1_UNORM,             // B5G5R5A1_UNORM

        DXGI_FORMAT_B8G8R8A8_UNORM,             // B8G8R8A8_UNORM
        DXGI_FORMAT_B8G8R8A8_UNORM_SRGB,        // B8G8R8A8_SRGB

        DXGI_FORMAT_BC6H_UF16,                  // BC6H_UFLOAT
        DXGI_FORMAT_BC6H_SF16,                  // BC6H_SFLOAT

        DXGI_FORMAT_BC7_UNORM,                  // BC7_UNORM
        DXGI_FORMAT_BC7_UNORM_SRGB,             // BC7_SRGB

        DXGI_FORMAT_NV12,                       // NV12
        DXGI_FORMAT_YUY2,                       // YUY2

        DXGI_FORMAT_B4G4R4A4_UNORM,             // B4G4R4A4_UNORM
    };

    private static readonly D3D12_COMMAND_LIST_TYPE[] s_d3d12CommandListTypeMap = new D3D12_COMMAND_LIST_TYPE[(int)GraphicsContextKind.COUNT__] {
        (D3D12_COMMAND_LIST_TYPE)(-1),      // Unknown

        D3D12_COMMAND_LIST_TYPE_DIRECT,     // Render
        D3D12_COMMAND_LIST_TYPE_COPY,       // Copy
        D3D12_COMMAND_LIST_TYPE_COMPUTE,    // Compute
    };      

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DXGI_FORMAT AsDxgiFormat(this GraphicsFormat format) => s_dxgiFormatMap[(uint)format];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static D3D12_COMMAND_LIST_TYPE AsD3D12CommandListType(this GraphicsContextKind kind) => s_d3d12CommandListTypeMap[(uint)kind];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CloseIfNotNull(HANDLE handle)
    {
        if (handle != HANDLE.NULL)
        {
            _ = CloseHandle(handle);
        }
    }

    public static ID3D12CommandAllocator* GetLatestD3D12CommandAllocator(ID3D12CommandAllocator* d3d12CommandAllocator, out uint d3d12CommandAllocatorVersion)
    {
        ID3D12CommandAllocator* result;

        d3d12CommandAllocatorVersion = 0;
        result = d3d12CommandAllocator;

        return result;
    }

    public static ID3D12CommandQueue* GetLatestD3D12CommandQueue(ID3D12CommandQueue* d3d12CommandQueue, out uint d3d12CommandQueueVersion)
    {
        ID3D12CommandQueue* result;

        d3d12CommandQueueVersion = 0;
        result = d3d12CommandQueue;

        return result;
    }

    public static ID3D12DescriptorHeap* GetLatestD3D12DescriptorHeap(ID3D12DescriptorHeap* d3d12DescriptorHeap, out uint d3d12DescriptorHeapVersion)
    {
        ID3D12DescriptorHeap* result;

        d3d12DescriptorHeapVersion = 0;
        result = d3d12DescriptorHeap;

        return result;
    }

    public static ID3D12Device* GetLatestD3D12Device(ID3D12Device* d3d12Device, out uint d3d12DeviceVersion)
    {
        ID3D12Device* result;

        if (d3d12Device->QueryInterface(__uuidof<ID3D12Device9>(), (void**)&result).SUCCEEDED)
        {
            d3d12DeviceVersion = 9;
            _ = d3d12Device->Release();
        }
        else if (d3d12Device->QueryInterface(__uuidof<ID3D12Device8>(), (void**)&result).SUCCEEDED)
        {
            d3d12DeviceVersion = 8;
            _ = d3d12Device->Release();
        }
        else if (d3d12Device->QueryInterface(__uuidof<ID3D12Device7>(), (void**)&result).SUCCEEDED)
        {
            d3d12DeviceVersion = 7;
            _ = d3d12Device->Release();
        }
        else if (d3d12Device->QueryInterface(__uuidof<ID3D12Device6>(), (void**)&result).SUCCEEDED)
        {
            d3d12DeviceVersion = 6;
            _ = d3d12Device->Release();
        }
        else if (d3d12Device->QueryInterface(__uuidof<ID3D12Device5>(), (void**)&result).SUCCEEDED)
        {
            d3d12DeviceVersion = 5;
            _ = d3d12Device->Release();
        }
        else if (d3d12Device->QueryInterface(__uuidof<ID3D12Device4>(), (void**)&result).SUCCEEDED)
        {
            d3d12DeviceVersion = 4;
            _ = d3d12Device->Release();
        }
        else if (d3d12Device->QueryInterface(__uuidof<ID3D12Device3>(), (void**)&result).SUCCEEDED)
        {
            d3d12DeviceVersion = 3;
            _ = d3d12Device->Release();
        }
        else if (d3d12Device->QueryInterface(__uuidof<ID3D12Device2>(), (void**)&result).SUCCEEDED)
        {
            d3d12DeviceVersion = 2;
            _ = d3d12Device->Release();
        }
        else if (d3d12Device->QueryInterface(__uuidof<ID3D12Device1>(), (void**)&result).SUCCEEDED)
        {
            d3d12DeviceVersion = 1;
            _ = d3d12Device->Release();
        }
        else
        {
            d3d12DeviceVersion = 0;
            result = d3d12Device;
        }

        return result;
    }

    public static ID3D12Fence* GetLatestD3D12Fence(ID3D12Fence* d3d12Fence, out uint d3d12FenceVersion)
    {
        ID3D12Fence* result;

        if (d3d12Fence->QueryInterface(__uuidof<ID3D12Fence1>(), (void**)&result).SUCCEEDED)
        {
            d3d12FenceVersion = 1;
            _ = d3d12Fence->Release();
        }
        else
        {
            d3d12FenceVersion = 0;
            result = d3d12Fence;
        }

        return result;
    }

    public static ID3D12GraphicsCommandList* GetLatestD3D12GraphicsCommandList(ID3D12GraphicsCommandList* d3d12GraphicsCommandList, out uint d3d12GraphicsCommandListVersion)
    {
        ID3D12GraphicsCommandList* result;

        if (d3d12GraphicsCommandList->QueryInterface(__uuidof<ID3D12GraphicsCommandList6>(), (void**)&result).SUCCEEDED)
        {
            d3d12GraphicsCommandListVersion = 6;
            _ = d3d12GraphicsCommandList->Release();
        }
        else if (d3d12GraphicsCommandList->QueryInterface(__uuidof<ID3D12GraphicsCommandList5>(), (void**)&result).SUCCEEDED)
        {
            d3d12GraphicsCommandListVersion = 5;
            _ = d3d12GraphicsCommandList->Release();
        }
        else if (d3d12GraphicsCommandList->QueryInterface(__uuidof<ID3D12GraphicsCommandList4>(), (void**)&result).SUCCEEDED)
        {
            d3d12GraphicsCommandListVersion = 4;
            _ = d3d12GraphicsCommandList->Release();
        }
        else if (d3d12GraphicsCommandList->QueryInterface(__uuidof<ID3D12GraphicsCommandList3>(), (void**)&result).SUCCEEDED)
        {
            d3d12GraphicsCommandListVersion = 3;
            _ = d3d12GraphicsCommandList->Release();
        }
        else if (d3d12GraphicsCommandList->QueryInterface(__uuidof<ID3D12GraphicsCommandList2>(), (void**)&result).SUCCEEDED)
        {
            d3d12GraphicsCommandListVersion = 2;
            _ = d3d12GraphicsCommandList->Release();
        }
        else if (d3d12GraphicsCommandList->QueryInterface(__uuidof<ID3D12GraphicsCommandList1>(), (void**)&result).SUCCEEDED)
        {
            d3d12GraphicsCommandListVersion = 1;
            _ = d3d12GraphicsCommandList->Release();
        }
        else
        {
            d3d12GraphicsCommandListVersion = 0;
            result = d3d12GraphicsCommandList;
        }

        return result;
    }

    public static ID3D12Heap* GetLatestD3D12Heap(ID3D12Heap* d3d12Heap, out uint d3d12HeapVersion)
    {
        ID3D12Heap* result;

        if (d3d12Heap->QueryInterface(__uuidof<ID3D12Heap1>(), (void**)&result).SUCCEEDED)
        {
            d3d12HeapVersion = 1;
            _ = d3d12Heap->Release();
        }
        else
        {
            d3d12HeapVersion = 0;
            result = d3d12Heap;
        }

        return result;
    }

    public static ID3D12PipelineState* GetLatestD3D12PipelineState(ID3D12PipelineState* d3d12PipelineState, out uint d3d12PipelineStateVersion)
    {
        ID3D12PipelineState* result;

        d3d12PipelineStateVersion = 0;
        result = d3d12PipelineState;

        return result;
    }

    public static ID3D12Resource* GetLatestD3D12Resource(ID3D12Resource* d3d12Resource, out uint d3d12ResourceVersion)
    {
        ID3D12Resource* result;

        if (d3d12Resource->QueryInterface(__uuidof<ID3D12Resource2>(), (void**)&result).SUCCEEDED)
        {
            d3d12ResourceVersion = 2;
            _ = d3d12Resource->Release();
        }
        else if (d3d12Resource->QueryInterface(__uuidof<ID3D12Resource1>(), (void**)&result).SUCCEEDED)
        {
            d3d12ResourceVersion = 1;
            _ = d3d12Resource->Release();
        }
        else
        {
            d3d12ResourceVersion = 0;
            result = d3d12Resource;
        }

        return result;
    }

    public static ID3D12RootSignature* GetLatestD3D12RootSignature(ID3D12RootSignature* d3d12RootSignature, out uint d3d12RootSignatureVersion)
    {
        ID3D12RootSignature* result;

        d3d12RootSignatureVersion = 0;
        result = d3d12RootSignature;

        return result;
    }

    public static IDXGIAdapter1* GetLatestDxgiAdapter(IDXGIAdapter1* dxgiAdapter, out uint dxgiAdapterVersion)
    {
        IDXGIAdapter1* result;

        if (dxgiAdapter->QueryInterface(__uuidof<IDXGIAdapter4>(), (void**)&result).SUCCEEDED)
        {
            dxgiAdapterVersion = 4;
            _ = dxgiAdapter->Release();
        }
        else if (dxgiAdapter->QueryInterface(__uuidof<IDXGIAdapter3>(), (void**)&result).SUCCEEDED)
        {
            dxgiAdapterVersion = 3;
            _ = dxgiAdapter->Release();
        }
        else if (dxgiAdapter->QueryInterface(__uuidof<IDXGIAdapter2>(), (void**)&result).SUCCEEDED)
        {
            dxgiAdapterVersion = 2;
            _ = dxgiAdapter->Release();
        }
        else
        {
            dxgiAdapterVersion = 1;
            result = dxgiAdapter;
        }

        return result;
    }

    public static IDXGIFactory3* GetLatestDxgiFactory(IDXGIFactory3* dxgiFactory, out uint dxgiFactoryVersion)
    {
        IDXGIFactory3* result;

        if (dxgiFactory->QueryInterface(__uuidof<IDXGIFactory7>(), (void**)&result).SUCCEEDED)
        {
            dxgiFactoryVersion = 7;
            _ = dxgiFactory->Release();
        }
        else if (dxgiFactory->QueryInterface(__uuidof<IDXGIFactory6>(), (void**)&result).SUCCEEDED)
        {
            dxgiFactoryVersion = 6;
            _ = dxgiFactory->Release();
        }
        else if (dxgiFactory->QueryInterface(__uuidof<IDXGIFactory5>(), (void**)&result).SUCCEEDED)
        {
            dxgiFactoryVersion = 5;
            _ = dxgiFactory->Release();
        }
        else if (dxgiFactory->QueryInterface(__uuidof<IDXGIFactory4>(), (void**)&result).SUCCEEDED)
        {
            dxgiFactoryVersion = 4;
            _ = dxgiFactory->Release();
        }
        else
        {
            dxgiFactoryVersion = 3;
            result = dxgiFactory;
        }

        return result;
    }

    public static IDXGISwapChain1* GetLatestDxgiSwapchain(IDXGISwapChain1* dxgiSwapchain, out uint dxgiSwapchainVersion)
    {
        IDXGISwapChain1* result;

        if (dxgiSwapchain->QueryInterface(__uuidof<IDXGISwapChain4>(), (void**)&result).SUCCEEDED)
        {
            dxgiSwapchainVersion = 4;
            _ = dxgiSwapchain->Release();
        }
        else if (dxgiSwapchain->QueryInterface(__uuidof<IDXGISwapChain3>(), (void**)&result).SUCCEEDED)
        {
            dxgiSwapchainVersion = 3;
            _ = dxgiSwapchain->Release();
        }
        else if (dxgiSwapchain->QueryInterface(__uuidof<IDXGISwapChain2>(), (void**)&result).SUCCEEDED)
        {
            dxgiSwapchainVersion = 2;
            _ = dxgiSwapchain->Release();
        }
        else
        {
            dxgiSwapchainVersion = 1;
            result = dxgiSwapchain;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReleaseIfNotNull<TUnknown>(TUnknown* unknown)
        where TUnknown : unmanaged, IUnknown.Interface
    {
        if (unknown is not null)
        {
            _ = unknown->Release();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowExternalExceptionIfFailed(HRESULT value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value.FAILED)
        {
            AssertNotNull(valueExpression);
            ExceptionUtilities.ThrowExternalException(valueExpression, value);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetD3D12Name<TD3D12Object>(ref this TD3D12Object self, string name, [CallerArgumentExpression("self")] string component = "")
        where TD3D12Object : unmanaged, ID3D12Object.Interface
    {
        if (GraphicsService.EnableDebugMode)
        {
            var componentName = $"{name}: {component}";

            fixed (char* pName = componentName)
            {
                _ = self.SetName((ushort*)pName);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetDxgiName<TDXGIObject>(ref this TDXGIObject self, string name, [CallerArgumentExpression("self")] string component = "")
        where TDXGIObject : unmanaged, IDXGIObject.Interface
    {
        if (GraphicsService.EnableDebugMode)
        {
            var componentName = $"{name}: {component}";

            fixed (char* pName = componentName)
            {
                _ = self.SetPrivateData(AsReadonlyPointer(in WKPDID_D3DDebugObjectName), (uint)componentName.Length, (ushort*)pName);
            }
        }
    }
}
