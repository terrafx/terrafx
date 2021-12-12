// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Collections;
using TerraFX.Threading;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Graphics;

public partial class GraphicsDevice
{
    /// <summary>Defines a small pool for managing created graphics contexts.</summary>
    /// <typeparam name="TGraphicsDevice">The type of graphics device which owns the pool.</typeparam>
    /// <typeparam name="TGraphicsContext">The type of graphics context managed by the pool.</typeparam>
    protected unsafe struct ContextPool<TGraphicsDevice, TGraphicsContext> : IDisposable
        where TGraphicsDevice : GraphicsDevice
        where TGraphicsContext : GraphicsContext
    {
        private readonly ValueMutex _mutex;

        private ValueList<TGraphicsContext> _createdContexts;
        private ValueQueue<TGraphicsContext> _availableContexts;

        /// <summary>Initializes a new instance of the <see cref="ContextPool{TGraphicsDevice, TGraphicsContext}" /> struct.</summary>
        public ContextPool()
        {
            _mutex = new ValueMutex();

            _createdContexts = new ValueList<TGraphicsContext>();
            _availableContexts = new ValueQueue<TGraphicsContext>();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            for (var i = 0; i < _createdContexts.Count; i++)
            {
                _createdContexts[i].Dispose();
            }

            _createdContexts.Clear();
            _availableContexts.Clear();
        }

        /// <summary>Rents a graphics context from the pool, creating a new context if none are available.</summary>
        /// <param name="device">The device which owns the graphics context.</param>
        /// <param name="create">The callback which will create a new context if none are available.</param>
        /// <returns>A graphics context that is owned by <paramref name="device" />.</returns>
        public TGraphicsContext Rent(TGraphicsDevice device, delegate*<TGraphicsDevice, TGraphicsContext> create)
        {
            using var mutex = new DisposableMutex(_mutex, isExternallySynchronized: false);
            return RentInternal(device, create);
        }

        /// <summary>Returns a graphics context to the pool.</summary>
        /// <param name="context">The graphics context that should be returned to the pool.</param>
        public void Return(TGraphicsContext context)
        {
            using var mutex = new DisposableMutex(_mutex, isExternallySynchronized: false);
            ReturnInternal(context);
        }

        private TGraphicsContext RentInternal(TGraphicsDevice device, delegate*<TGraphicsDevice, TGraphicsContext> create)
        {
            // This method should only be called under a mutex
            AssertNotNull(device);

            if (!_availableContexts.TryDequeue(out var context))
            {
                context = create(device);
                _createdContexts.Add(context);
            }

            AssertNotNull(context);
            return context;
        }

        private void ReturnInternal(TGraphicsContext context)
        {
            AssertNotNull(context);
            Assert(AssertionsEnabled && _createdContexts.Contains(context));
            _availableContexts.Enqueue(context);
        }
    }
}
