// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from D3D12HelloWindow.h in https://github.com/Microsoft/DirectX-Graphics-Samples
// Original source is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using TerraFX.Interop;
using static TerraFX.Interop.D3D_FEATURE_LEVEL;
using static TerraFX.Interop.D3D12;
using static TerraFX.Interop.D3D12_COMMAND_QUEUE_FLAGS;
using static TerraFX.Interop.D3D12_COMMAND_LIST_TYPE;
using static TerraFX.Interop.D3D12_DESCRIPTOR_HEAP_FLAGS;
using static TerraFX.Interop.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.D3D12_FENCE_FLAGS;
using static TerraFX.Interop.D3D12_RESOURCE_BARRIER_FLAGS;
using static TerraFX.Interop.D3D12_RESOURCE_BARRIER_TYPE;
using static TerraFX.Interop.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.DXGI;
using static TerraFX.Interop.DXGI_FORMAT;
using static TerraFX.Interop.DXGI_SWAP_EFFECT;
using static TerraFX.Interop.Kernel32;
using static TerraFX.Interop.Windows;
using static TerraFX.Samples.DirectX.D3D12.DXSampleHelper;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples.DirectX.D3D12
{
    public unsafe class HelloWindow : DXSample
    {
        #region Constants
        private const uint FrameCount = 2;
        #endregion

        #region Fields
        // Pipeline objects
        private IDXGISwapChain3* _swapChain;
        private ID3D12Device* _device;
        private _renderTargets_e__FixedBuffer _renderTargets;
        private ID3D12CommandAllocator* _commandAllocator;
        private ID3D12CommandQueue* _commandQueue;
        private ID3D12DescriptorHeap* _rtvHeap;
        private ID3D12PipelineState* _pipelineState;
        private ID3D12GraphicsCommandList* _commandList;
        private uint _rtvDescriptorSize;

        // Synchronization objects.
        private uint _frameIndex;
        private IntPtr _fenceEvent;
        private ID3D12Fence* _fence;
        private ulong _fenceValue;
        #endregion

        #region Constructors
        public HelloWindow(uint width, uint height, string name)
            : base(width, height, name)
        {
            _frameIndex = 0;
            _rtvDescriptorSize = 0;
            _pipelineState = null;
        }
        #endregion

        #region Methods
        public override void OnInit()
        {
            LoadPipeline();
            LoadAssets();
        }

        // Update frame-based values.
        public override void OnUpdate()
        {
        }

        // Render the scene.
        public override void OnRender()
        {
            // Record all the commands we need to render the scene into the command list.
            PopulateCommandList();

            // Execute the command list.
            var ppCommandLists = stackalloc ID3D12CommandList*[1];
            ppCommandLists[0] = (ID3D12CommandList*)(_commandList);

            _commandQueue->ExecuteCommandLists(1, ppCommandLists);

            // Present the frame.
            ThrowIfFailed(nameof(IDXGISwapChain3.Present), _swapChain->Present(1, 0));

            WaitForPreviousFrame();
        }

        public override void OnDestroy()
        {
            // Ensure that the GPU is no longer referencing resources that are about to be
            // cleaned up by the destructor.
            WaitForPreviousFrame();

            CloseHandle(_fenceEvent);
        }

        // Load the rendering pipeline dependencies.
        private void LoadPipeline()
        {
            Guid iid;
            ID3D12Debug* debugController;
            IDXGIFactory4* factory;
            IDXGIAdapter* adapter;
            ID3D12Device* device;
            IDXGISwapChain1* swapChain;

            var dxgiFactoryFlags = 0u;

            try
            {
#if DEBUG
                // Enable the debug layer (requires the Graphics Tools "optional feature").
                // NOTE: Enabling the debug layer after device creation will invalidate the active device.
                iid = IID_ID3D12Debug;
                if (SUCCEEDED(D3D12GetDebugInterface(&iid, (void**)(&debugController))))
                {
                    debugController->EnableDebugLayer();

                    // Enable additional debug layers.
                    dxgiFactoryFlags |= DXGI_CREATE_FACTORY_DEBUG;
                }
#endif

                iid = IID_IDXGIFactory4;
                ThrowIfFailed(nameof(CreateDXGIFactory2), CreateDXGIFactory2(dxgiFactoryFlags, &iid, (void**)(&factory)));

                if (_useWarpDevice)
                {
                    iid = IID_IDXGIAdapter;
                    ThrowIfFailed(nameof(IDXGIFactory4.EnumWarpAdapter), factory->EnumWarpAdapter(&iid, (void**)(&adapter)));
                }
                else
                {
                    adapter = GetHardwareAdapter(factory);
                }

                iid = IID_ID3D12Device;
                ThrowIfFailed(nameof(D3D12CreateDevice), D3D12CreateDevice((IUnknown*)(adapter), D3D_FEATURE_LEVEL_11_0, &iid, (void**)(&device)));
                _device = device;

                // Describe and create the command queue.
                var queueDesc = new D3D12_COMMAND_QUEUE_DESC {
                    Flags = D3D12_COMMAND_QUEUE_FLAG_NONE,
                    Type = D3D12_COMMAND_LIST_TYPE_DIRECT
                };

                iid = IID_ID3D12CommandQueue;
                ID3D12CommandQueue* commandQueue;

                ThrowIfFailed(nameof(ID3D12Device.CreateCommandQueue), _device->CreateCommandQueue(&queueDesc, &iid, (void**)(&commandQueue)));
                _commandQueue = commandQueue;

                // Describe and create the swap chain.
                var swapChainDesc = new DXGI_SWAP_CHAIN_DESC1 {
                    BufferCount = FrameCount,
                    Width = _width,
                    Height = _height,
                    Format = DXGI_FORMAT_R8G8B8A8_UNORM,
                    BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT,
                    SwapEffect = DXGI_SWAP_EFFECT_FLIP_DISCARD
                };
                swapChainDesc.SampleDesc.Count = 1;

                ThrowIfFailed(nameof(IDXGIFactory4.CreateSwapChainForHwnd), factory->CreateSwapChainForHwnd(
                    (IUnknown*)(_commandQueue),         // Swap chain needs the queue so that it can force a flush on it.
                    Win32Application.Hwnd,
                    &swapChainDesc,
                    null,
                    null,
                    &swapChain
                ));

                // This sample does not support fullscreen transitions.
                ThrowIfFailed(nameof(IDXGIFactory4.MakeWindowAssociation), factory->MakeWindowAssociation(Win32Application.Hwnd, DXGI_MWA_NO_ALT_ENTER));

                iid = IID_IDXGISwapChain3;
                IDXGISwapChain3* pvObject;

                ThrowIfFailed(nameof(IDXGISwapChain1.QueryInterface), swapChain->QueryInterface(&iid, (void**)(&pvObject)));
                _swapChain = pvObject;

                _frameIndex = _swapChain->GetCurrentBackBufferIndex();

                // Create descriptor heaps.
                {
                    // Describe and create a render target view (RTV) descriptor heap.
                    var rtvHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                        NumDescriptors = FrameCount,
                        Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV,
                        Flags = D3D12_DESCRIPTOR_HEAP_FLAG_NONE
                    };

                    iid = IID_ID3D12DescriptorHeap;
                    ID3D12DescriptorHeap* pvHeap;

                    ThrowIfFailed(nameof(ID3D12Device.CreateDescriptorHeap), _device->CreateDescriptorHeap(&rtvHeapDesc, &iid, (void**)(&pvHeap)));
                    _rtvHeap = pvHeap;
                    _rtvDescriptorSize = _device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);
                }

                // Create frame resources.
                {
                    D3D12_CPU_DESCRIPTOR_HANDLE rtvHandle;
                    _rtvHeap->GetCPUDescriptorHandleForHeapStart(&rtvHandle);

                    iid = IID_ID3D12Resource;

                    // Create a RTV for each frame.
                    for (var n = 0u; n < FrameCount; n++)
                    {
                        ID3D12Resource* pSurface;
                        ThrowIfFailed(nameof(IDXGISwapChain3.GetBuffer), _swapChain->GetBuffer(n, &iid, (void**)(&pSurface)));
                        _renderTargets[unchecked((int)(n))] = pSurface;

                        _device->CreateRenderTargetView(_renderTargets[unchecked((int)(n))], null, rtvHandle);
                        rtvHandle.ptr += _rtvDescriptorSize;
                    }
                }

                iid = IID_ID3D12CommandAllocator;
                ID3D12CommandAllocator* pCommandAllocator;

                ThrowIfFailed(nameof(ID3D12Device.CreateRenderTargetView), _device->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_DIRECT, &iid, (void**)(&pCommandAllocator)));
                _commandAllocator = pCommandAllocator;
            }
            finally
            {

            }
        }

        // Load the sample assets.
        private void LoadAssets()
        {
            // Create the command list.
            var iid = IID_ID3D12GraphicsCommandList;
            ID3D12GraphicsCommandList* pCommandList;

            ThrowIfFailed(nameof(ID3D12Device.CreateCommandList), _device->CreateCommandList(0, D3D12_COMMAND_LIST_TYPE_DIRECT, _commandAllocator, null, &iid, (void**)(&pCommandList)));
            _commandList = pCommandList;

            // Command lists are created in the recording state, but there is nothing
            // to record yet. The main loop expects it to be closed, so close it now.
            ThrowIfFailed(nameof(ID3D12GraphicsCommandList.Close), _commandList->Close());

            // Create synchronization objects.
            {
                iid = IID_ID3D12Fence;
                ID3D12Fence* pFence;

                ThrowIfFailed(nameof(ID3D12Device.CreateFence), _device->CreateFence(0, D3D12_FENCE_FLAG_NONE, &iid, (void**)(&pFence)));
                _fence = pFence;

                _fenceValue = 1;

                // Create an event handle to use for frame synchronization.
                _fenceEvent = CreateEvent(null, FALSE, FALSE, null);
                if (_fenceEvent == IntPtr.Zero)
                {
                    ThrowExternalExceptionForLastHRESULT(nameof(CreateEvent));
                }
            }
        }

        private void PopulateCommandList()
        {
            // Command list allocators can only be reset when the associated 
            // command lists have finished execution on the GPU; apps should use 
            // fences to determine GPU execution progress.
            ThrowIfFailed(nameof(ID3D12CommandAllocator.Reset), _commandAllocator->Reset());

            // However, when ExecuteCommandList() is called on a particular command 
            // list, that command list can then be reset at any time and must be before 
            // re-recording.
            ThrowIfFailed(nameof(ID3D12GraphicsCommandList.Reset), _commandList->Reset(_commandAllocator, _pipelineState));

            // Indicate that the back buffer will be used as a render target.
            var barrier = new D3D12_RESOURCE_BARRIER {
                Type = D3D12_RESOURCE_BARRIER_TYPE_TRANSITION,
                Flags = D3D12_RESOURCE_BARRIER_FLAG_NONE
            };

            barrier.Transition.pResource = _renderTargets[unchecked((int)(_frameIndex))];
            barrier.Transition.StateBefore = D3D12_RESOURCE_STATE_PRESENT;
            barrier.Transition.StateAfter = D3D12_RESOURCE_STATE_RENDER_TARGET;
            barrier.Transition.Subresource = D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES;

            _commandList->ResourceBarrier(1, &barrier);

            D3D12_CPU_DESCRIPTOR_HANDLE rtvHandle;
            _rtvHeap->GetCPUDescriptorHandleForHeapStart(&rtvHandle);
            rtvHandle.ptr += (_frameIndex * _rtvDescriptorSize);

            // Record commands.
            var clearColor = stackalloc float[4];

            clearColor[0] = 0.0f;
            clearColor[1] = 0.2f;
            clearColor[2] = 0.4f;
            clearColor[3] = 1.0f;

            _commandList->ClearRenderTargetView(rtvHandle, clearColor, 0, null);

            // Indicate that the back buffer will now be used to present.

            barrier.Transition.pResource = _renderTargets[unchecked((int)(_frameIndex))];
            barrier.Transition.StateBefore = D3D12_RESOURCE_STATE_RENDER_TARGET;
            barrier.Transition.StateAfter = D3D12_RESOURCE_STATE_PRESENT;

            _commandList->ResourceBarrier(1, &barrier);

            ThrowIfFailed(nameof(ID3D12GraphicsCommandList.Close), _commandList->Close());
        }

        private void WaitForPreviousFrame()
        {
            // WAITING FOR THE FRAME TO COMPLETE BEFORE CONTINUING IS NOT BEST PRACTICE.
            // This is code implemented as such for simplicity. The D3D12HelloFrameBuffering
            // sample illustrates how to use fences for efficient resource usage and to
            // maximize GPU utilization.

            // Signal and increment the fence value.
            var fence = _fenceValue;
            ThrowIfFailed(nameof(ID3D12CommandQueue.Signal), _commandQueue->Signal(_fence, fence));

            _fenceValue++;

            // Wait until the previous frame is finished.
            if (_fence->GetCompletedValue() < fence)
            {
                ThrowIfFailed(nameof(ID3D12Fence.SetEventOnCompletion), _fence->SetEventOnCompletion(fence, _fenceEvent));
                WaitForSingleObject(_fenceEvent, INFINITE);
            }

            _frameIndex = _swapChain->GetCurrentBackBufferIndex();
        }
        #endregion

        #region Structs
        public /* blittable */ unsafe struct _renderTargets_e__FixedBuffer
        {
            #region Fields
            public ID3D12Resource* e0;

            public ID3D12Resource* e1;
            #endregion

            #region Properties
            public ID3D12Resource* this[int index]
            {
                get
                {
                    fixed (ID3D12Resource** e = &e0)
                    {
                        return e[index];
                    }
                }

                set
                {
                    fixed (ID3D12Resource** e = &e0)
                    {
                        e[index] = value;
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
