// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.D3D12.Graphics
{
    /// <summary>Represents a graphics device.</summary>
    public sealed unsafe class GraphicsDevice : IDisposable, IGraphicsDevice
    {
        private readonly GraphicsAdapter _graphicsAdapter;
        private readonly ID3D12Device* _device;
        private readonly ID3D12CommandQueue* _commandQueue;

        private State _state;

        internal GraphicsDevice(GraphicsAdapter graphicsAdapter, ID3D12Device* device, ID3D12CommandQueue* commandQueue)
        {
            _graphicsAdapter = graphicsAdapter;
            _device = device;
            _commandQueue = commandQueue;
        }

        /// <summary>Finalizes an instance of the <see cref="GraphicsDevice" /> class.</summary>
        ~GraphicsDevice()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="IGraphicsAdapter" /> for the instance.</summary>
        public IGraphicsAdapter GraphicsAdapter => _graphicsAdapter;

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle => (IntPtr)_device;

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeDevice();
            }

            _state.EndDispose();
        }

        private void DisposeDevice()
        {
            _state.AssertDisposing();

            if (_commandQueue != null)
            {
                _ = _commandQueue->Release();
            }

            if (_device != null)
            {
                _ = _device->Release();
            }
        }
    }
}
