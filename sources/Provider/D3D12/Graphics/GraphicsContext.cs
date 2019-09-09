// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;
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

namespace TerraFX.Provider.D3D12.Graphics
{
    /// <summary>Represents a graphics context, which can be used for rendering images.</summary>
    public sealed unsafe class GraphicsContext : IDisposable, IGraphicsContext
    {
        private readonly GraphicsAdapter _graphicsAdapter;
        private readonly IGraphicsSurface _graphicsSurface;

        private readonly Lazy<ID3D12CommandAllocator*[]> _commandAllocators;
        private readonly Lazy<IntPtr> _commandQueue;
        private readonly Lazy<IntPtr> _device;
        private readonly Lazy<ID3D12Fence*[]> _fences;
        private readonly Lazy<IntPtr[]> _fenceEvents;
        private readonly Lazy<ulong[]> _fenceValues;
        private readonly Lazy<ID3D12GraphicsCommandList*[]> _graphicsCommandLists;
        private readonly Lazy<IntPtr> _renderTargetsHeap;
        private readonly Lazy<ID3D12Resource*[]> _renderTargets;
        private readonly Lazy<IntPtr> _swapChain;

        private ulong _fenceValue;
        private uint _frameIndex;
        private State _state;

        internal GraphicsContext(GraphicsAdapter graphicsAdapter, IGraphicsSurface graphicsSurface)
        {
            _graphicsAdapter = graphicsAdapter;
            _graphicsSurface = graphicsSurface;

            _commandAllocators = new Lazy<ID3D12CommandAllocator*[]>(CreateCommandAllocators, isThreadSafe: true);
            _commandQueue = new Lazy<IntPtr>(CreateCommandQueue, isThreadSafe: true);
            _device = new Lazy<IntPtr>(CreateDevice, isThreadSafe: true);
            _fences = new Lazy<ID3D12Fence*[]>(CreateFences, isThreadSafe: true);
            _fenceEvents = new Lazy<IntPtr[]>(CreateFenceEvents, isThreadSafe: true);
            _fenceValues = new Lazy<ulong[]>(CreateFenceValues, isThreadSafe: true);
            _graphicsCommandLists = new Lazy<ID3D12GraphicsCommandList*[]>(CreateGraphicsCommandLists, isThreadSafe: true);
            _renderTargets = new Lazy<ID3D12Resource*[]>(CreateRenderTargets, isThreadSafe: true);
            _renderTargetsHeap = new Lazy<IntPtr>(CreateRenderTargetsHeap, isThreadSafe: true);
            _swapChain = new Lazy<IntPtr>(CreateSwapChain, isThreadSafe: true);

            _ = _state.Transition(to: Initialized);
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
                return (ID3D12CommandQueue*)_commandQueue.Value;
            }
        }

        /// <summary>Gets the <see cref="ID3D12Device" /> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public ID3D12Device* Device
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return (ID3D12Device*)_device.Value;
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
        public IntPtr[] FenceEvents
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

        /// <summary>Gets the <see cref="IGraphicsAdapter" /> for the instance.</summary>
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

        /// <summary>Gets the <see cref="IGraphicsSurface" /> for the instance.</summary>
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
                return (ID3D12DescriptorHeap*)_renderTargetsHeap.Value;
            }
        }

        /// <summary>Gets the <see cref="IDXGISwapChain3" /> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IDXGISwapChain3* SwapChain
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return (IDXGISwapChain3*)_swapChain.Value;
            }
        }

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Begins a new frame for rendering.</summary>
        /// <param name="backgroundColor">A color to which the background should be cleared.</param>
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

        /// <summary>Ends the frame currently be rendered.</summary>
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

        /// <summary>Presents the last frame rendered.</summary>
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

        private IntPtr CreateCommandQueue()
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

            return (IntPtr)commandQueue;
        }

        private IntPtr CreateDevice()
        {
            ID3D12Device* device;

            var iid = IID_ID3D12Device;
            ThrowExternalExceptionIfFailed(nameof(D3D12CreateDevice), D3D12CreateDevice((IUnknown*)_graphicsAdapter.Adapter, D3D_FEATURE_LEVEL_11_0, &iid, (void**)&device));

            return (IntPtr)device;
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

        private IntPtr[] CreateFenceEvents()
        {
            var fenceEvents = new IntPtr[_graphicsSurface.BufferCount];

            for (var i = 0; i < fenceEvents.Length; i++)
            {
                var fenceEvent = CreateEvent(lpEventAttributes: null, bManualReset: FALSE, bInitialState: FALSE, lpName: null);

                if (fenceEvent == IntPtr.Zero)
                {
                    ThrowExternalExceptionForLastHRESULT(nameof(CreateEvent));
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
                ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreateCommandList), Device->CreateCommandList(NodeMask: 0, D3D12_COMMAND_LIST_TYPE_DIRECT, CommandAllocators[i], pInitialState: null, &iid, (void**)&graphicsCommandList));

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

        private IntPtr CreateRenderTargetsHeap()
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

            return (IntPtr)renderTargetDescriptorHeap;
        }

        private IntPtr CreateSwapChain()
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
                SwapEffect = DXGI_SWAP_EFFECT_FLIP_DISCARD,
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

            return (IntPtr)swapChain;
        }

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
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
            if (_commandAllocators.IsValueCreated)
            {
                foreach (var commandAllocator in _commandAllocators.Value)
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
            if (_commandQueue.IsValueCreated)
            {
                var commandQueue = (ID3D12CommandQueue*)_commandQueue.Value;
                _ = commandQueue->Release();
            }
        }

        private void DisposeDevice()
        {
            if (_device.IsValueCreated)
            {
                var device = (ID3D12Device*)_device.Value;
                _ = device->Release();
            }
        }

        private void DisposeFences()
        {
            if (_fences.IsValueCreated)
            {
                foreach (var fence in _fences.Value)
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
            if (_fenceEvents.IsValueCreated)
            {
                foreach (var fenceEvent in _fenceEvents.Value)
                {
                    if (fenceEvent != IntPtr.Zero)
                    {
                        _ = CloseHandle(fenceEvent);
                    }
                }
            }
        }

        private void DisposeGraphicsCommandLists()
        {
            if (_graphicsCommandLists.IsValueCreated)
            {
                foreach (var graphicsCommandList in _graphicsCommandLists.Value)
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
            if (_renderTargetsHeap.IsValueCreated)
            {
                var renderTargetsHeap = (ID3D12DescriptorHeap*)_renderTargetsHeap.Value;
                _ = renderTargetsHeap->Release();
            }
        }

        private void DisposeRenderTargets()
        {
            if (_renderTargets.IsValueCreated)
            {
                foreach (var renderTarget in _renderTargets.Value)
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
            if (_swapChain.IsValueCreated)
            {
                var swapChain = (IDXGISwapChain3*)_swapChain.Value;
                _ = swapChain->Release();
            }
        }

        private static void WaitForFence(ID3D12Fence* fence, IntPtr fenceEvent, ulong fenceValue)
        {
            if (fence->GetCompletedValue() < fenceValue)
            {
                fence->SetEventOnCompletion(fenceValue, fenceEvent);
                WaitForSingleObject(fenceEvent, INFINITE);
            }
        }
    }
}
