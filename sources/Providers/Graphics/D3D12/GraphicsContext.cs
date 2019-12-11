// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Numerics;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D_FEATURE_LEVEL;
using static TerraFX.Interop.D3D12;
using static TerraFX.Interop.D3D12_COMMAND_LIST_TYPE;
using static TerraFX.Interop.D3D12_COMMAND_QUEUE_FLAGS;
using static TerraFX.Interop.D3D12_DESCRIPTOR_HEAP_FLAGS;
using static TerraFX.Interop.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.D3D12_FENCE_FLAGS;
using static TerraFX.Interop.D3D12_RESOURCE_BARRIER_TYPE;
using static TerraFX.Interop.D3D12_RESOURCE_BARRIER_FLAGS;
using static TerraFX.Interop.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.D3D12_RTV_DIMENSION;
using static TerraFX.Interop.DXGI;
using static TerraFX.Interop.DXGI_ALPHA_MODE;
using static TerraFX.Interop.DXGI_FORMAT;
using static TerraFX.Interop.DXGI_SCALING;
using static TerraFX.Interop.DXGI_SWAP_EFFECT;
using static TerraFX.Interop.Kernel32;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <summary>Represents a graphics context, which can be used for rendering images.</summary>
    public sealed unsafe class GraphicsContext : IDisposable, IGraphicsContext
    {
        private readonly GraphicsAdapter _graphicsAdapter;
        private readonly IGraphicsSurface _graphicsSurface;

        private ValueLazy<ID3D12CommandAllocator*[]> _commandAllocators;
        private ValueLazy<Pointer<ID3D12CommandQueue>> _commandQueue;
        private ValueLazy<Pointer<ID3D12Device>> _device;
        private ValueLazy<ID3D12Fence*[]> _fences;
        private ValueLazy<HANDLE[]> _fenceEvents;
        private ValueLazy<ulong[]> _fenceValues;
        private ValueLazy<ID3D12GraphicsCommandList*[]> _graphicsCommandLists;
        private ValueLazy<Pointer<ID3D12DescriptorHeap>> _renderTargetsHeap;
        private ValueLazy<ID3D12Resource*[]> _renderTargets;
        private ValueLazy<Pointer<IDXGISwapChain3>> _swapChain;

        private ulong _fenceValue;
        private uint _frameIndex;
        private State _state;

        internal GraphicsContext(GraphicsAdapter graphicsAdapter, IGraphicsSurface graphicsSurface)
        {
            _graphicsAdapter = graphicsAdapter;
            _graphicsSurface = graphicsSurface;

            _commandAllocators = new ValueLazy<ID3D12CommandAllocator*[]>(CreateCommandAllocators);
            _commandQueue = new ValueLazy<Pointer<ID3D12CommandQueue>>(CreateCommandQueue);
            _device = new ValueLazy<Pointer<ID3D12Device>>(CreateDevice);
            _fences = new ValueLazy<ID3D12Fence*[]>(CreateFences);
            _fenceEvents = new ValueLazy<HANDLE[]>(CreateFenceEvents);
            _fenceValues = new ValueLazy<ulong[]>(CreateFenceValues);
            _graphicsCommandLists = new ValueLazy<ID3D12GraphicsCommandList*[]>(CreateGraphicsCommandLists);
            _renderTargets = new ValueLazy<ID3D12Resource*[]>(CreateRenderTargets);
            _renderTargetsHeap = new ValueLazy<Pointer<ID3D12DescriptorHeap>>(CreateRenderTargetsHeap);
            _swapChain = new ValueLazy<Pointer<IDXGISwapChain3>>(CreateSwapChain);

            _ = _state.Transition(to: Initialized);

            // Do event hookups after we are in the initialized state, since an event could
            // technically fire while the constructor is still running.

            _graphicsSurface.SizeChanged += HandleGraphicsSurfaceSizeChanged;
        }

        /// <summary>Finalizes an instance of the <see cref="GraphicsContext" /> class.</summary>
        ~GraphicsContext()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets an array of <see cref="ID3D12CommandAllocator" /> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public ID3D12CommandAllocator*[] CommandAllocators
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _commandAllocators.Value;
            }
        }

        /// <summary>Gets the <see cref="ID3D12CommandQueue" /> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public ID3D12CommandQueue* CommandQueue
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _commandQueue.Value;
            }
        }

        /// <summary>Gets the <see cref="ID3D12Device" /> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public ID3D12Device* Device
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _device.Value;
            }
        }

        /// <summary>Gets an array of <see cref="ID3D12Fence" /> for protecting resources for any given <see cref="RenderTargets" />.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public ID3D12Fence*[] Fences
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _fences.Value;
            }
        }

        /// <summary>Gets an array of fence event handles for protecting resources for any given <see cref="RenderTargets" />.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public HANDLE[] FenceEvents
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _fenceEvents.Value;
            }
        }

        /// <summary>Gets an array of <see cref="ulong" /> for protecting resources for any given <see cref="RenderTargets" />.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public ulong[] FenceValues
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _fenceValues.Value;
            }
        }

        /// <inheritdoc />
        public IGraphicsAdapter GraphicsAdapter => _graphicsAdapter;

        /// <summary>Gets an array of <see cref="ID3D12GraphicsCommandList" /> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public ID3D12GraphicsCommandList*[] GraphicsCommandLists
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _graphicsCommandLists.Value;
            }
        }

        /// <inheritdoc />
        public IGraphicsSurface GraphicsSurface => _graphicsSurface;

        /// <summary>Gets an array of <see cref="ID3D12Resource" /> representing the render targets for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public ID3D12Resource*[] RenderTargets
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _renderTargets.Value;
            }
        }

        /// <summary>Gets the <see cref="ID3D12DescriptorHeap" /> for the <see cref="RenderTargets" />.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public ID3D12DescriptorHeap* RenderTargetsHeap
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _renderTargetsHeap.Value;
            }
        }

        /// <summary>Gets the <see cref="IDXGISwapChain3" /> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IDXGISwapChain3* SwapChain
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _swapChain.Value;
            }
        }

        /// <inheritdoc />
        public void BeginFrame(ColorRgba backgroundColor)
        {
            var frameIndex = SwapChain->GetCurrentBackBufferIndex();
            _frameIndex = frameIndex;
            WaitForFence(Fences[frameIndex], FenceEvents[frameIndex], FenceValues[frameIndex]);

            var commandAllocator = CommandAllocators[frameIndex];
            ThrowExternalExceptionIfFailed(nameof(ID3D12CommandAllocator.Reset), commandAllocator->Reset());

            var graphicsCommandList = GraphicsCommandLists[frameIndex];
            ThrowExternalExceptionIfFailed(nameof(ID3D12GraphicsCommandList.Reset), graphicsCommandList->Reset(commandAllocator, pInitialState: null));

            var renderTargetDescriptorSize = Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);
            var renderTargetHandle = RenderTargetsHeap->GetCPUDescriptorHandleForHeapStart();

            _ = renderTargetHandle.Offset((int)frameIndex, renderTargetDescriptorSize);
            graphicsCommandList->OMSetRenderTargets(1, &renderTargetHandle, RTsSingleHandleToDescriptorRange: TRUE, pDepthStencilDescriptor: null);

            var viewport = new D3D12_VIEWPORT {
                TopLeftX = 0.0f,
                TopLeftY = 0.0f,
                Width = _graphicsSurface.Width,
                Height = _graphicsSurface.Height,
                MinDepth = 0.0f,
                MaxDepth = 1.0f,
            };
            graphicsCommandList->RSSetViewports(1, &viewport);

            var scissorRect = new RECT {
                left = 0,
                top = 0,
                right = (int)_graphicsSurface.Width,
                bottom = (int)_graphicsSurface.Height,
            };
            graphicsCommandList->RSSetScissorRects(1, &scissorRect);

            var barrier = new D3D12_RESOURCE_BARRIER {
                Type = D3D12_RESOURCE_BARRIER_TYPE_TRANSITION,
                Flags = D3D12_RESOURCE_BARRIER_FLAG_NONE,
                Anonymous = new D3D12_RESOURCE_BARRIER._Anonymous_e__Union {
                    Transition = new D3D12_RESOURCE_TRANSITION_BARRIER {
                        pResource = RenderTargets[frameIndex],
                        Subresource = D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES,
                        StateBefore = D3D12_RESOURCE_STATE_PRESENT,
                        StateAfter = D3D12_RESOURCE_STATE_RENDER_TARGET,
                    },
                },
            };
            graphicsCommandList->ResourceBarrier(1, &barrier);

            graphicsCommandList->ClearRenderTargetView(renderTargetHandle, (float*)&backgroundColor, NumRects: 0, pRects: null);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public void EndFrame()
        {
            var frameIndex = _frameIndex;

            var barrier = new D3D12_RESOURCE_BARRIER {
                Type = D3D12_RESOURCE_BARRIER_TYPE_TRANSITION,
                Flags = D3D12_RESOURCE_BARRIER_FLAG_NONE,
                Anonymous = new D3D12_RESOURCE_BARRIER._Anonymous_e__Union {
                    Transition = new D3D12_RESOURCE_TRANSITION_BARRIER {
                        pResource = RenderTargets[frameIndex],
                        Subresource = D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES,
                        StateBefore = D3D12_RESOURCE_STATE_RENDER_TARGET,
                        StateAfter = D3D12_RESOURCE_STATE_PRESENT,
                    },
                },
            };

            var graphicsCommandList = GraphicsCommandLists[frameIndex];
            graphicsCommandList->ResourceBarrier(1, &barrier);

            ThrowExternalExceptionIfFailed(nameof(ID3D12GraphicsCommandList.Close), graphicsCommandList->Close());
            CommandQueue->ExecuteCommandLists(1, (ID3D12CommandList**)&graphicsCommandList);
        }

        /// <inheritdoc />
        public void PresentFrame()
        {
            var frameIndex = _frameIndex;
            ThrowExternalExceptionIfFailed(nameof(IDXGISwapChain.Present), SwapChain->Present(1, 0));

            var fenceValue = _fenceValue;

            var fence = Fences[frameIndex];
            ThrowExternalExceptionIfFailed(nameof(ID3D12CommandQueue.Signal), CommandQueue->Signal(fence, fenceValue));
            FenceValues[frameIndex] = fenceValue;

            _fenceValue++;
        }

        private ID3D12CommandAllocator*[] CreateCommandAllocators()
        {
            var commandAllocators = new ID3D12CommandAllocator*[_graphicsSurface.BufferCount];
            var iid = IID_ID3D12CommandAllocator;

            for (var i = 0; i < commandAllocators.Length; i++)
            {
                ID3D12CommandAllocator* commandAllocator;
                ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreateCommandAllocator), Device->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_DIRECT, &iid, (void**)&commandAllocator));
                commandAllocators[i] = commandAllocator;
            }

            return commandAllocators;
        }

        private Pointer<ID3D12CommandQueue> CreateCommandQueue()
        {
            ID3D12CommandQueue* commandQueue;

            var commandQueueDesc = new D3D12_COMMAND_QUEUE_DESC {
                Type = D3D12_COMMAND_LIST_TYPE_DIRECT,
                Priority = 0,
                Flags = D3D12_COMMAND_QUEUE_FLAG_NONE,
                NodeMask = 0,
            };

            var iid = IID_ID3D12CommandQueue;
            ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreateCommandQueue), Device->CreateCommandQueue(&commandQueueDesc, &iid, (void**)&commandQueue));

            return commandQueue;
        }

        private Pointer<ID3D12Device> CreateDevice()
        {
            ID3D12Device* device;

            var iid = IID_ID3D12Device;
            ThrowExternalExceptionIfFailed(nameof(D3D12CreateDevice), D3D12CreateDevice((IUnknown*)_graphicsAdapter.Adapter, D3D_FEATURE_LEVEL_11_0, &iid, (void**)&device));

            return device;
        }

        private ID3D12Fence*[] CreateFences()
        {
            var fences = new ID3D12Fence*[_graphicsSurface.BufferCount];
            var iid = IID_ID3D12Fence;

            for (var i = 0; i < fences.Length; i++)
            {
                ID3D12Fence* fence;
                ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreateFence), Device->CreateFence(InitialValue: 0, D3D12_FENCE_FLAG_NONE, &iid, (void**)&fence));
                fences[i] = fence;
            }
            
            return fences;
        }

        private HANDLE[] CreateFenceEvents()
        {
            var fenceEvents = new HANDLE[_graphicsSurface.BufferCount];

            for (var i = 0; i < fenceEvents.Length; i++)
            {
                HANDLE fenceEvent = CreateEventW(lpEventAttributes: null, bManualReset: FALSE, bInitialState: FALSE, lpName: null);

                if (fenceEvent == null)
                {
                    ThrowExternalExceptionForLastHRESULT(nameof(CreateEventW));
                }

                fenceEvents[i] = fenceEvent;
            }

            return fenceEvents;
        }

        private ulong[] CreateFenceValues()
        {
            var fenceValues = new ulong[_graphicsSurface.BufferCount];
            _fenceValue = 1;
            return fenceValues;
        }

        private ID3D12GraphicsCommandList*[] CreateGraphicsCommandLists()
        {
            var graphicsCommandLists = new ID3D12GraphicsCommandList*[_graphicsSurface.BufferCount];
            var iid = IID_ID3D12GraphicsCommandList;

            for (var i = 0; i < graphicsCommandLists.Length; i++)
            {
                ID3D12GraphicsCommandList* graphicsCommandList;
                ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreateCommandList), Device->CreateCommandList(nodeMask: 0, D3D12_COMMAND_LIST_TYPE_DIRECT, CommandAllocators[i], pInitialState: null, &iid, (void**)&graphicsCommandList));

                // Command lists are created in the recording state, but there is nothing
                // to record yet. The main loop expects it to be closed, so close it now.
                ThrowExternalExceptionIfFailed(nameof(ID3D12GraphicsCommandList.Close), graphicsCommandList->Close());

                graphicsCommandLists[i] = graphicsCommandList;
            }

            return graphicsCommandLists;
        }

        private ID3D12Resource*[] CreateRenderTargets()
        {
            var renderTargets = new ID3D12Resource*[_graphicsSurface.BufferCount];
            var iid = IID_ID3D12Resource;

            var renderTargetDescriptorSize = Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);
            var renderTargetHandle = RenderTargetsHeap->GetCPUDescriptorHandleForHeapStart();

            var renderTargetViewDesc = new D3D12_RENDER_TARGET_VIEW_DESC {
                Format = DXGI_FORMAT_R8G8B8A8_UNORM_SRGB,
                ViewDimension = D3D12_RTV_DIMENSION_TEXTURE2D,
                Anonymous = new D3D12_RENDER_TARGET_VIEW_DESC._Anonymous_e__Union {
                    Texture2D = new D3D12_TEX2D_RTV {
                        MipSlice = 0,
                        PlaneSlice = 0,
                    },
                },
            };

            for (var i = 0; i < renderTargets.Length; i++)
            {
                ID3D12Resource* renderTarget;
                ThrowExternalExceptionIfFailed(nameof(IDXGISwapChain.GetBuffer), SwapChain->GetBuffer((uint)i, &iid, (void**)&renderTarget));
                renderTargets[i] = renderTarget;

                Device->CreateRenderTargetView(renderTarget, &renderTargetViewDesc, renderTargetHandle);
                _ = renderTargetHandle.Offset((int)renderTargetDescriptorSize);
            }

            return renderTargets;
        }

        private Pointer<ID3D12DescriptorHeap> CreateRenderTargetsHeap()
        {
            ID3D12DescriptorHeap* renderTargetDescriptorHeap;

            var renderTargetDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV,
                NumDescriptors = (uint)_graphicsSurface.BufferCount,
                Flags = D3D12_DESCRIPTOR_HEAP_FLAG_NONE,
                NodeMask = 0,
            };

            var iid = IID_ID3D12DescriptorHeap;
            ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreateDescriptorHeap), Device->CreateDescriptorHeap(&renderTargetDescriptorHeapDesc, &iid, (void**)&renderTargetDescriptorHeap));

            return renderTargetDescriptorHeap;
        }

        private Pointer<IDXGISwapChain3> CreateSwapChain()
        {
            IDXGISwapChain3* swapChain;

            var swapChainDesc = new DXGI_SWAP_CHAIN_DESC1 {
                Width = (uint)_graphicsSurface.Width,
                Height = (uint)_graphicsSurface.Height,
                Format = DXGI_FORMAT_R8G8B8A8_UNORM,
                Stereo = FALSE,
                SampleDesc = new DXGI_SAMPLE_DESC {
                    Count = 1,
                    Quality = 0,
                },
                BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT,
                BufferCount = (uint)_graphicsSurface.BufferCount,
                Scaling = DXGI_SCALING_NONE,                
                SwapEffect = DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL,
                AlphaMode = DXGI_ALPHA_MODE_UNSPECIFIED,
                Flags = 0
            };

            var graphicsProvider = (GraphicsProvider)_graphicsAdapter.GraphicsProvider;
            var iid = IID_IDXGISwapChain3;

            switch (_graphicsSurface.Kind)
            {
                case GraphicsSurfaceKind.Win32:
                {
                    ThrowExternalExceptionIfFailed(nameof(IDXGIFactory2.CreateSwapChainForHwnd), graphicsProvider.Factory->CreateSwapChainForHwnd((IUnknown*)CommandQueue, _graphicsSurface.WindowHandle, &swapChainDesc, pFullscreenDesc: null, pRestrictToOutput: null, (IDXGISwapChain1**)&swapChain));
                    break;
                }

                default:
                {
                    ThrowInvalidOperationException(nameof(_graphicsSurface), _graphicsSurface);
                    swapChain = null;
                    break;
                }
            }

            // Fullscreen transitions are not currently supported
            ThrowExternalExceptionIfFailed(nameof(IDXGIFactory.MakeWindowAssociation), graphicsProvider.Factory->MakeWindowAssociation(_graphicsSurface.WindowHandle, DXGI_MWA_NO_ALT_ENTER));

            return swapChain;
        }

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                WaitForIdle();
                DisposeGraphicsCommandLists();
                DisposeCommandAllocators();
                DisposeRenderTargetsHeap();
                DisposeRenderTargets();
                DisposeFences();
                DisposeFenceEvents();
                DisposeSwapChain();
                DisposeCommandQueue();
                DisposeDevice();
            }

            _state.EndDispose();
        }

        private void DisposeCommandAllocators()
        {
            _state.AssertDisposing();

            if (_commandAllocators.IsCreated)
            {
                var commandAllocators = _commandAllocators.Value;

                foreach (var commandAllocator in commandAllocators)
                {
                    if (commandAllocator != null)
                    {
                        _ = commandAllocator->Release();
                    }
                }
            }
        }

        private void DisposeCommandQueue()
        {
            _state.AssertDisposing();

            if (_commandQueue.IsCreated)
            {
                var commandQueue = (ID3D12CommandQueue*)_commandQueue.Value;
                _ = commandQueue->Release();
            }
        }

        private void DisposeDevice()
        {
            _state.AssertDisposing();

            if (_device.IsCreated)
            {
                var device = (ID3D12Device*)_device.Value;
                _ = device->Release();
            }
        }

        private void DisposeFences()
        {
            _state.AssertDisposing();

            if (_fences.IsCreated)
            {
                var fences = _fences.Value;

                foreach (var fence in fences)
                {
                    if (fence != null)
                    {
                        _ = fence->Release();
                    }
                }
            }
        }

        private void DisposeFenceEvents()
        {
            _state.AssertDisposing();

            if (_fenceEvents.IsCreated)
            {
                var fenceEvents = _fenceEvents.Value;

                foreach (var fenceEvent in fenceEvents)
                {
                    if (fenceEvent != null)
                    {
                        _ = CloseHandle(fenceEvent);
                    }
                }
            }
        }

        private void DisposeGraphicsCommandLists()
        {
            _state.AssertDisposing();

            if (_graphicsCommandLists.IsCreated)
            {
                var graphicsCommandLists = _graphicsCommandLists.Value;

                foreach (var graphicsCommandList in graphicsCommandLists)
                {
                    if (graphicsCommandList != null)
                    {
                        _ = graphicsCommandList->Release();
                    }
                }
            }
        }

        private void DisposeRenderTargetsHeap()
        {
            _state.AssertDisposing();

            if (_renderTargetsHeap.IsCreated)
            {
                var renderTargetsHeap = (ID3D12DescriptorHeap*)_renderTargetsHeap.Value;
                _ = renderTargetsHeap->Release();
            }
        }

        private void DisposeRenderTargets()
        {
            _state.AssertDisposing();

            if (_renderTargets.IsCreated)
            {
                var renderTargets = _renderTargets.Value;

                foreach (var renderTarget in renderTargets)
                {
                    if (renderTarget != null)
                    {
                        _ = renderTarget->Release();
                    }
                }
            }
        }

        private void DisposeSwapChain()
        {
            _state.AssertDisposing();

            if (_swapChain.IsCreated)
            {
                var swapChain = (IDXGISwapChain3*)_swapChain.Value;
                _ = swapChain->Release();
            }
        }

        private void HandleGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> e)
        {
            WaitForIdle();

            if (_renderTargets.IsCreated)
            {
                var renderTargets = _renderTargets.Value;

                foreach (var renderTarget in renderTargets)
                {
                    if (renderTarget != null)
                    {
                        _ = renderTarget->Release();
                    }
                }

                _renderTargets.Reset(CreateRenderTargets);
            }

            if (_swapChain.IsCreated)
            {
                ThrowExternalExceptionIfFailed(nameof(IDXGISwapChain.ResizeBuffers), SwapChain->ResizeBuffers((uint)_graphicsSurface.BufferCount, (uint)_graphicsSurface.Width, (uint)_graphicsSurface.Height, DXGI_FORMAT_R8G8B8A8_UNORM, SwapChainFlags: 0));
            }
        }

        private void WaitForIdle()
        {
            if (_commandQueue.IsCreated)
            {
                var device = (ID3D12Device*)_device.Value;
                var commandQueue = (ID3D12CommandQueue*)_commandQueue.Value;

                ID3D12Fence* fence;

                var iid = IID_ID3D12Fence;
                ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreateFence), device->CreateFence(InitialValue: 0, D3D12_FENCE_FLAG_NONE, &iid, (void**)&fence));

                ThrowExternalExceptionIfFailed(nameof(ID3D12CommandQueue.Signal), commandQueue->Signal(fence, Value: 1));

                HANDLE fenceEvent = CreateEventW(lpEventAttributes: null, bManualReset: FALSE, bInitialState: FALSE, lpName: null);

                if (fenceEvent == null)
                {
                    ThrowExternalExceptionForLastHRESULT(nameof(CreateEventW));
                }

                WaitForFence(fence, fenceEvent, fenceValue: 1);

                _ = CloseHandle(fenceEvent);
                _ = fence->Release();
            }
        }

        private static void WaitForFence(ID3D12Fence* fence, HANDLE fenceEvent, ulong fenceValue)
        {
            if (fence->GetCompletedValue() < fenceValue)
            {
                _ = fence->SetEventOnCompletion(fenceValue, fenceEvent);
                _ = WaitForSingleObject(fenceEvent, INFINITE);
            }
        }
    }
}
