// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Threading;

namespace TerraFX.UI;

/// <summary>An object which is created for a dispatcher.</summary>
public interface IUIDispatcherObject : IUIServiceObject
{
    /// <summary>Gets the dispatcher for which the object was created.</summary>
    UIDispatcher Dispatcher { get; }

    /// <summary>Gets the thread that was used to create <see cref="Dispatcher" />.</summary>
    Thread ParentThread { get; }
}
