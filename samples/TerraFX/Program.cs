// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading;
using TerraFX.ApplicationModel;
using TerraFX.Numerics;
using TerraFX.Samples.Audio;
using TerraFX.Samples.Graphics;
using TerraFX.Samples.ServiceProviders;

namespace TerraFX.Samples;

public static unsafe class Program
{
    internal static readonly ApplicationServiceProvider s_pulseAudioServiceProvider = new PulseAudioServiceProvider();

    internal static readonly ApplicationServiceProvider s_uiServiceProvider = OperatingSystem.IsWindows() ? new Win32WindowServiceProvider() : new XlibWindowServiceProvider();

    [SupportedOSPlatform("windows10.0")]
    internal static readonly ApplicationServiceProvider s_d3d12GraphicsServiceProvider = new D3D12GraphicsServiceProvider();

    internal static readonly ApplicationServiceProvider s_vulkanGraphicsServiceProvider = new VulkanGraphicsServiceProvider(s_uiServiceProvider);

    [SupportedOSPlatform("windows10.0")]
    private static readonly Sample[] s_d3d12Samples = {
        new EnumerateGraphicsAdapters("D3D12.EnumerateGraphicsAdapters", s_d3d12GraphicsServiceProvider),
        new HelloWindow("D3D12.HelloWindow", s_d3d12GraphicsServiceProvider),
        new HelloTriangle("D3D12.HelloTriangle", s_d3d12GraphicsServiceProvider),
        new HelloQuad("D3D12.HelloQuad", s_d3d12GraphicsServiceProvider),
        new HelloTransform("D3D12.HelloTransform", s_d3d12GraphicsServiceProvider),
        new HelloTexture("D3D12.HelloTexture", s_d3d12GraphicsServiceProvider),
        new HelloTextureTransform("D3D12.HelloTextureTransform", s_d3d12GraphicsServiceProvider),
        new HelloInstancing("D3D12.HelloInstancing", s_d3d12GraphicsServiceProvider),
        new HelloTexture3D("D3D12.HelloTexture3D", s_d3d12GraphicsServiceProvider),
        new HelloSmoke("D3D12.HelloSmoke", true, s_d3d12GraphicsServiceProvider),
        new HelloSierpinskiPyramid("D3D12.HelloSierpinskiPyramid", 5, s_d3d12GraphicsServiceProvider),
        new HelloSierpinskiQuad("D3D12.HelloSierpinskiQuad", 6, s_d3d12GraphicsServiceProvider),
    };

    private static readonly Sample[] s_vulkanSamples = {
        new EnumerateGraphicsAdapters("Vulkan.EnumerateGraphicsAdapters", s_vulkanGraphicsServiceProvider),
        new HelloWindow("Vulkan.HelloWindow", s_vulkanGraphicsServiceProvider),
        new HelloTriangle("Vulkan.HelloTriangle", s_vulkanGraphicsServiceProvider),
        new HelloQuad("Vulkan.HelloQuad", s_vulkanGraphicsServiceProvider),
        new HelloTransform("Vulkan.HelloTransform", s_vulkanGraphicsServiceProvider),
        new HelloTexture("Vulkan.HelloTexture", s_vulkanGraphicsServiceProvider),
        new HelloTextureTransform("Vulkan.HelloTextureTransform", s_vulkanGraphicsServiceProvider),
        new HelloInstancing("Vulkan.HelloInstancing", s_vulkanGraphicsServiceProvider),
        new HelloTexture3D("Vulkan.HelloTexture3D", s_vulkanGraphicsServiceProvider),
        new HelloSmoke("Vulkan.HelloSmoke", true, s_vulkanGraphicsServiceProvider),
        new HelloSierpinskiPyramid("Vulkan.HelloSierpinskiPyramid", 5, s_vulkanGraphicsServiceProvider),
        new HelloSierpinskiQuad("Vulkan.HelloSierpinskiQuad", 6, s_vulkanGraphicsServiceProvider),
    };

    private static readonly Sample[] s_pulseAudioSamples = {
        new EnumerateAudioAdapters("PulseAudio.EnumerateAudioAdapters.Sync", false, s_pulseAudioServiceProvider),
        new EnumerateAudioAdapters("PulseAudio.EnumerateAudioAdapters.Async", true, s_pulseAudioServiceProvider),

        new PlaySampleAudio("PulseAudio.PlaySampleAudio", s_pulseAudioServiceProvider),
    };

    private static IEnumerable<Sample> AllSamples
    {
        get
        {
            var samples = Enumerable.Empty<Sample>();

            samples = samples.Concat(AudioSamples);
            samples = samples.Concat(GraphicsSamples);

            return samples;
        }
    }

    private static IEnumerable<Sample> AudioSamples
    {
        get
        {
            var samples = Enumerable.Empty<Sample>();

            if (OperatingSystem.IsLinux())
            {
                samples = samples.Concat(s_pulseAudioSamples);
            }

            return samples;
        }
    }

    private static IEnumerable<Sample> GraphicsSamples
    {
        get
        {
            var samples = Enumerable.Empty<Sample>();

            if (OperatingSystem.IsWindowsVersionAtLeast(10))
            {
                samples = samples.Concat(s_d3d12Samples);
            }

            if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
            {
                samples = samples.Concat(s_vulkanSamples);
            }

            return samples;
        }
    }

    public static void Main(string[] args)
    {
        Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;

        if ((args.Length == 0) || args.Any((arg) => Matches(arg, "?", "h", "help")))
        {
            PrintHelp();
        }
        else
        {
            RunSamples(args);
        }
    }

    private static bool Matches(string arg, params string[] keywords)
    {
        return keywords.Any((keyword) => ((arg.Length == keyword.Length) && arg.Equals(keyword, StringComparison.OrdinalIgnoreCase))
                                      || (((arg.Length - 1) == keyword.Length) && ((arg[0] == '-') || (arg[0] == '/')) && (string.Compare(arg, 1, keyword, 0, keyword.Length, StringComparison.OrdinalIgnoreCase) == 0)));
    }

    private static void PrintHelp()
    {
        Console.WriteLine("General Options");
        Console.WriteLine("    ALL:     Indicates that all samples should be run.");
        Console.WriteLine();

        Console.WriteLine("Available Samples - Can specify multiple");

        foreach (var sample in AllSamples)
        {
            Console.WriteLine($"    {sample.Name}");
        }
    }

    private static void Run(Sample sample, TimeSpan timeout, Vector2? windowLocation, Vector2? windowSize)
    {
        var application = new Application(sample.ServiceProvider);

        if (sample is HelloWindow window)
        {
            window.Initialize(application, timeout, windowLocation, windowSize);
        }
        else
        {
            sample.Initialize(application, timeout);
        }

        application.Run();
        sample.Cleanup();
    }

    private static void RunSamples(string[] args)
    {
        var ranAnySamples = false;

        Vector2? windowLocation = null;
        if (args.Any((arg) => Matches(arg, "-windowLocation")))
        {
            var argsList = args.ToList();
            var index = argsList.IndexOf("-windowLocation");
            (float x, float y) = (-1, -1);
            (var xOk, var yOk) = (false, false);
            if (index != -1 && argsList.Count >= index + 2)
            {
                xOk = float.TryParse(argsList[++index], out x);
                yOk = float.TryParse(argsList[++index], out y);
            }
            if (xOk && yOk)
            {
                windowLocation = Vector2.Create(x, y);
            }
        }

        Vector2? windowSize = null;
        if (args.Any((arg) => Matches(arg, "-windowSize")))
        {
            var argsList = args.ToList();
            var index = argsList.IndexOf("-windowSize");
            (float w, float h) = (-1, -1);
            (var wOk, var hOk) = (false, false);
            if (index != -1 && argsList.Count >= index + 2)
            {
                wOk = float.TryParse(argsList[++index], out w);
                hOk = float.TryParse(argsList[++index], out h);
            }
            if (wOk && hOk)
            {
                windowSize = Vector2.Create(w, h);
            }
        }

        if (args.Any((arg) => Matches(arg, "all")))
        {
            foreach (var sample in AllSamples)
            {
                RunSample(sample, TimeSpan.FromSeconds(2.5), windowLocation, windowSize);
                ranAnySamples = true;
            }
        }
        else
        {
            foreach (var arg in args)
            {
                foreach (var sample in AllSamples.Where((sample) => arg.Equals(sample.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    RunSample(sample, TimeSpan.MaxValue, windowLocation, windowSize);
                    ranAnySamples = true;
                }
            }
        }

        if (ranAnySamples)
        {
            if (OperatingSystem.IsWindowsVersionAtLeast(10))
            {
                s_d3d12GraphicsServiceProvider.Dispose();
            }

            if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
            {
                s_vulkanGraphicsServiceProvider.Dispose();
            }

            if (OperatingSystem.IsLinux())
            {
                s_pulseAudioServiceProvider.Dispose();
            }
        }
        else
        {
            PrintHelp();
        }
    }

    private static void RunSample(Sample sample, TimeSpan timeout, Vector2? windowLocation, Vector2? windowSize)
    {
        Console.WriteLine($"Running: {sample.Name}");
        var thread = new Thread(() => Run(sample, timeout, windowLocation, windowSize));

        thread.Start();
        thread.Join();
    }
}
