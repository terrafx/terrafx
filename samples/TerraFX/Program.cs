// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using TerraFX.ApplicationModel;
using TerraFX.Numerics;
using TerraFX.Samples.Graphics;
using TerraFX.Samples.UI;

namespace TerraFX.Samples;

public static unsafe class Program
{
    private static readonly Sample[] s_graphicsSamples = {
        new EnumerateGraphicsAdapters("Graphics.EnumerateGraphicsAdapters"),
        new HelloWindow("Graphics.HelloWindow"),
        new HelloTriangle("Graphics.HelloTriangle"),
        new HelloQuad("Graphics.HelloQuad"),
        new HelloTransform("Graphics.HelloTransform"),
        new HelloTexture("Graphics.HelloTexture"),
        new HelloTextureTransform("Graphics.HelloTextureTransform"),
        new HelloInstancing("Graphics.HelloInstancing"),
        new HelloTexture3D("Graphics.HelloTexture3D"),
        new HelloSmoke("Graphics.HelloSmoke", true),
        new HelloSierpinskiPyramid("Graphics.HelloSierpinskiPyramid", 5),
        new HelloSierpinskiQuad("Graphics.HelloSierpinskiQuad", 6),
    };

    private static readonly Sample[] s_uiSamples = {
        new EmptyWindow("UI.EmptyWindow"),
    };

    private static IEnumerable<Sample> AllSamples
    {
        get
        {
            var samples = Enumerable.Empty<Sample>();

            samples = samples.Concat(GraphicsSamples);
            samples = samples.Concat(UISamples);

            return samples;
        }
    }

    private static IEnumerable<Sample> GraphicsSamples => s_graphicsSamples;

    private static IEnumerable<Sample> UISamples => s_uiSamples;

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
        using var application = new Application();
        RunNoDispose(sample, timeout, windowLocation, windowSize, application);

        static void RunNoDispose(Sample sample, TimeSpan timeout, Vector2? windowLocation, Vector2? windowSize, Application application)
        {
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

        if (!ranAnySamples)
        {
            PrintHelp();
        }
    }

    private static void RunSample(Sample sample, TimeSpan timeout, Vector2? windowLocation, Vector2? windowSize)
    {
        Console.WriteLine($"Running: {sample.Name}");

        if (Debugger.IsAttached)
        {
            Run(sample, timeout, windowLocation, windowSize);
        }
        else
        {
            var thread = new Thread(() => Run(sample, timeout, windowLocation, windowSize));

            thread.Start();
            thread.Join();
        }
    }
}
