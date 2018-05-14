// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.ApplicationModel
{
    /// <summary>Represents the event args that occur during an <see cref="Application.Idle" /> event.</summary>
    /// <remarks>This is a struct, rather than derived from <see cref="EventArgs" />, to prevent unecessary heap allocations.</remarks>
    public readonly struct ApplicationIdleEventArgs
    {
        #region Fields
        private readonly TimeSpan _delta;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="ApplicationIdleEventArgs" /> class.</summary>
        /// <param name="delta">The delta between the current and previous <see cref="Application.Idle" /> events.</param>
        public ApplicationIdleEventArgs(TimeSpan delta)
        {
            _delta = delta;
        }
        #endregion

        #region Properties
        /// <summary>Gets the delta between the current and previous <see cref="Application.Idle" /> events.</summary>
        public TimeSpan Delta
        {
            get
            {
                return _delta;
            }
        }
        #endregion
    }
}
