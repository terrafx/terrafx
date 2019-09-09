// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Collections;
using TerraFX.Graphics;
using TerraFX.Graphics.Geometry2D;

namespace TerraFX.UI
{
    /// <summary>Defines a window.</summary>
    public interface IWindow
    {
        /// <summary>Gets a <see cref="Rectangle" /> that represents the bounds of the instance.</summary>
        Rectangle Bounds { get; }

        /// <summary>Gets <see cref="FlowDirection" /> for the instance.</summary>
        FlowDirection FlowDirection { get; }

        /// <summary>Gets the handle for the instance.</summary>
        IntPtr Handle { get; }

        /// <summary>Gets a value that indicates whether the instance is the active window.</summary>
        bool IsActive { get; }

        /// <summary>Gets a value that indicates whether the instance is enabled.</summary>
        bool IsEnabled { get; }

        /// <summary>Gets a value that indicates whether the instance is visible.</summary>
        bool IsVisible { get; }

        /// <summary>Gets the <see cref="Thread" /> that was used to create the instance.</summary>
        Thread ParentThread { get; }

        /// <summary>Gets the <see cref="IPropertySet" /> for the instance.</summary>
        IPropertySet Properties { get; }

        /// <summary>Gets the <see cref="ReadingDirection" /> for the instance.</summary>
        ReadingDirection ReadingDirection { get; }

        /// <summary>Gets the title for the instance.</summary>
        string Title { get; }

        /// <summary>Gets the <see cref="IWindowProvider" /> for the instance.</summary>
        IWindowProvider WindowProvider { get; }

        /// <summary>Gets the <see cref="WindowState" /> for the instance.</summary>
        WindowState WindowState { get; }

        /// <summary>Activates the instance.</summary>
        void Activate();

        /// <summary>Closes the instance.</summary>
        void Close();

        /// <summary>Creates a new <see cref="IGraphicsSurface" /> for the instance.</summary>
        /// <param name="bufferCount">The number of buffers created for the instance.</param>
        /// <returns>A new <see cref="IGraphicsSurface" /> for the instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferCount" /> is less than or equal to zero.</exception>
        IGraphicsSurface CreateGraphicsSurface(int bufferCount);

        /// <summary>Disables the instance.</summary>
        void Disable();

        /// <summary>Enables the instance.</summary>
        void Enable();

        /// <summary>Hides the instance.</summary>
        void Hide();

        /// <summary>Maximizes the instance.</summary>
        void Maximize();

        /// <summary>Minimizes the instance.</summary>
        void Minimize();

        /// <summary>Restores the instance.</summary>
        void Restore();

        /// <summary>Shows the instance.</summary>
        void Show();

        /// <summary>Tries to activate the instance.</summary>
        /// <returns><c>true</c> if the instance was succesfully activated; otherwise, <c>false</c>.</returns>
        bool TryActivate();
    }
}
