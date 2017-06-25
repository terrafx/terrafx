// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Collections;
using TerraFX.Threading;

namespace TerraFX.UI
{
    /// <summary>Defines a window.</summary>
    public interface IWindow
    {
        #region Properties
        /// <summary>Gets a <see cref="Rectangle" /> that represents the bounds of the instance.</summary>
        Rectangle Bounds { get; }

        /// <summary>Gets the dispatcher for the instance.</summary>
        IDispatcher Dispatcher { get; }

        /// <summary>Gets a value that indicates whether the flow-direction of the instance is left-to-right.</summary>
        bool IsLeftToRight { get; }

        /// <summary>Gets a value that indicates whether the instance is visible.</summary>
        bool IsVisible { get; }

        /// <summary>Gets the set of properties associated with the instance.</summary>
        IPropertySet Properties { get; }
        #endregion

        #region Methods
        /// <summary>Activates the instance.</summary>
        void Activate();

        /// <summary>Closes the instance.</summary>
        void Close();
        #endregion
    }
}
