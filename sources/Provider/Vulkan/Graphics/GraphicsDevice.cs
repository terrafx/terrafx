// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Utilities;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Vulkan.Graphics
{
    /// <summary>Represents a graphics device.</summary>
    public sealed unsafe class GraphicsDevice : IDisposable, IGraphicsDevice
    {
        private readonly GraphicsAdapter _graphicsAdapter;
        private readonly IntPtr _device;
        private readonly IntPtr _queue;

        private State _state;

        internal GraphicsDevice(GraphicsAdapter graphicsAdapter, IntPtr device, IntPtr queue)
        {
            _graphicsAdapter = graphicsAdapter;
            _device = device;
            _queue = queue;
        }

        /// <summary>Finalizes an instance of the <see cref="GraphicsDevice" /> class.</summary>
        ~GraphicsDevice()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="IGraphicsAdapter" /> for the instance.</summary>
        public IGraphicsAdapter GraphicsAdapter => _graphicsAdapter;

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle => _device;

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

            if (_device != IntPtr.Zero)
            {
                vkDestroyDevice(_device, pAllocator: null);
            }
        }
    }
}
