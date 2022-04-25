// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Threading;

namespace TerraFX.UI.Advanced;

/// <summary>An object which is created for a dispatcher.</summary>
public abstract class UIDispatcherObject : UIServiceObject
{
    private readonly UIDispatcher _dispatcher;
    private readonly Thread _parentThread;

    private protected UIDispatcherObject(UIDispatcher dispatcher, string? name = null) : base(dispatcher.Service, name)
    {
        _dispatcher = dispatcher;
        _parentThread = dispatcher.ParentThread;
    }

    /// <summary>Gets the dispatcher for which the object was created.</summary>
    public UIDispatcher Dispatcher => _dispatcher;

    /// <summary>Gets the thread that was used to create <see cref="Dispatcher" />.</summary>
    public Thread ParentThread => _parentThread;
}
