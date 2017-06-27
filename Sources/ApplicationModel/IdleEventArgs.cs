// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.ApplicationModel
{
    /// <summary>Represents the event args that occur during an <see cref="Application.Idle" /> event.</summary>
    public sealed class IdleEventArgs : EventArgs
    {
        #region Fields
        private readonly TimeSpan _delta;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="IdleEventArgs" /> class.</summary>
        /// <param name="delta">The delta between the current and previous <see cref="Application.Idle" /> events.</param>
        public IdleEventArgs(TimeSpan delta)
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
