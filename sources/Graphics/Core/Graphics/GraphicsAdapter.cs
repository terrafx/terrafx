// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics adapter which can be used for computational or graphical operations.</summary>
    public abstract class GraphicsAdapter : IDisposable
    {
        private readonly GraphicsProvider _provider;

        /// <summary>Initializes a new instance of the <see cref="GraphicsAdapter" /> class.</summary>
        /// <param name="provider">The provider which enumerated the adapter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider" /> is <c>null</c>.</exception>
        protected GraphicsAdapter(GraphicsProvider provider)
        {
            ThrowIfNull(provider, nameof(provider));
            _provider = provider;
        }

        /// <summary>Gets the PCI Device ID (DID) for the adapter.</summary>
        /// <exception cref="ObjectDisposedException">The adapter has been disposed and the value was not otherwise cached.</exception>
        public abstract uint DeviceId { get; }

        /// <summary>Gets the name of the adapter.</summary>
        /// <exception cref="ObjectDisposedException">The adapter has been disposed and the value was not otherwise cached.</exception>
        public abstract string Name { get; }

        /// <summary>Gets the provider which enumerated the adapter.</summary>
        public GraphicsProvider Provider => _provider;

        /// <summary>Gets the PCI Vendor ID (VID) for the adapter.</summary>
        /// <exception cref="ObjectDisposedException">The adapter has been disposed and the value was not otherwise cached.</exception>
        public abstract uint VendorId { get; }

        /// <summary>Creates a new graphics device which utilizes the adapter to render to a surface.</summary>
        /// <param name="surface">The surface to which the context can render.</param>
        /// <param name="contextCount">The number of contexts the device should maintain.</param>
        /// <returns>A new graphics device which utilizes the the adapter to render to <paramref name="surface" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="surface" /> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">The adapter has been disposed.</exception>
        public abstract GraphicsDevice CreateDevice(IGraphicsSurface surface, int contextCount);

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
