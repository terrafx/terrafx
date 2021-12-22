// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.UI;
using TerraFX.Utilities;

namespace TerraFX.Samples.UI;

public sealed class EmptyWindow : Sample
{
    private Window _window = null!;
    private TimeSpan _elapsedTime;

    public EmptyWindow(string name, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
    }

    public Window Window => _window;

    public override void Cleanup()
    {
        _window?.Dispose();
        base.Cleanup();
    }

    public override void Initialize(Application application, TimeSpan timeout)
    {
        ExceptionUtilities.ThrowIfNull(application);

        var uiService = application.ServiceProvider.UIService;

        _window = uiService.DispatcherForCurrentThread.CreateWindow();
        _window.SetTitle(Name);

        _window.Show();

        base.Initialize(application, timeout);
    }

    protected override void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs)
    {
        ExceptionUtilities.ThrowIfNull(sender);

        _elapsedTime += eventArgs.Delta;

        if (_elapsedTime >= Timeout)
        {
            var application = (Application)sender;
            application.RequestExit();
        }
    }
}
