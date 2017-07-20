// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
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

        /// <summary>Gets the <see cref="IDispatcher" /> for the instance.</summary>
        IDispatcher Dispatcher { get; }

        /// <summary>Gets <see cref="FlowDirection" /> for the instance.</summary>
        FlowDirection FlowDirection { get; }

        /// <summary>Gets the handle for the instance.</summary>
        IntPtr Handle { get; }

        /// <summary>Gets a value that indicates whether the instance is the active window.</summary>
        bool IsActive { get; }

        /// <summary>Gets a value that indicates whether the instance is visible.</summary>
        bool IsVisible { get; }

        /// <summary>Gets the <see cref="IPropertySet" /> for the instance.</summary>
        IPropertySet Properties { get; }

        /// <summary>Gets the <see cref="ReadingDirection" /> for the instance.</summary>
        ReadingDirection ReadingDirection { get; }

        /// <summary>Gets the <see cref="IWindowManager" /> for the instance.</summary>
        IWindowManager WindowManager { get; }
        #endregion

        #region Methods
        /// <summary>Activates the instance.</summary>
        void Activate();

        /// <summary>Closes the instance.</summary>
        void Close();

        /// <summary>Hides the instance.</summary>
        void Hide();

        /// <summary>Shows the instance.</summary>
        void Show();
        #endregion
    }
}
