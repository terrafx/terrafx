// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.ApplicationModel;
using TerraFX.Samples.Audio;
using TerraFX.Samples.Graphics;

namespace TerraFX.Samples
{
    public static unsafe class Program
    {
        internal static readonly Assembly s_audioProviderPulseAudio = Assembly.LoadFrom("TerraFX.Audio.Providers.PulseAudio.dll");

        internal static readonly Assembly s_graphicsProviderD3D12 = Assembly.LoadFrom("TerraFX.Graphics.Providers.D3D12.dll");
        internal static readonly Assembly s_graphicsProviderVulkan = Assembly.LoadFrom("TerraFX.Graphics.Providers.Vulkan.dll");

        private static readonly Sample[] s_samples = {
            new EnumerateGraphicsAdapters("D3D12.EnumerateGraphicsAdapters", s_graphicsProviderD3D12),
            new EnumerateGraphicsAdapters("Vulkan.EnumerateGraphicsAdapters", s_graphicsProviderVulkan),

            new HelloWindow("D3D12.HelloWindow", s_graphicsProviderD3D12),
            new HelloWindow("Vulkan.HelloWindow", s_graphicsProviderVulkan),

            new HelloTriangle("D3D12.HelloTriangle", s_graphicsProviderD3D12),
            new HelloTriangle("Vulkan.HelloTriangle", s_graphicsProviderVulkan),

            new HelloQuad("D3D12.HelloQuad", s_graphicsProviderD3D12),
            new HelloQuad("Vulkan.HelloQuad", s_graphicsProviderVulkan),

            new HelloTransform("D3D12.HelloTransform", s_graphicsProviderD3D12),
            new HelloTransform("Vulkan.HelloTransform", s_graphicsProviderVulkan),

            new HelloTexture("D3D12.HelloTexture", s_graphicsProviderD3D12),
            new HelloTexture("Vulkan.HelloTexture", s_graphicsProviderVulkan),

            new HelloTextureTransform("D3D12.HelloTextureTransform", s_graphicsProviderD3D12),
            new HelloTextureTransform("Vulkan.HelloTextureTransform", s_graphicsProviderVulkan),

            new HelloTexture3D("D3D12.HelloTexture3D", s_graphicsProviderD3D12),
            new HelloTexture3D("Vulkan.HelloTexture3D", s_graphicsProviderVulkan),

            new HelloSmoke("D3D12.HelloSmoke", true, s_graphicsProviderD3D12),
            new HelloSmoke("Vulkan.HelloSmoke", true, s_graphicsProviderVulkan),

            new HelloSierpinskiPyramid("D3D12.HelloSierpinskiPyramid", 5, s_graphicsProviderD3D12),
            new HelloSierpinskiPyramid("Vulkan.HelloSierpinskiPyramid", 5, s_graphicsProviderVulkan),

            new HelloSierpinskiQuad("D3D12.HelloSierpinskiQuad", 6, s_graphicsProviderD3D12),
            new HelloSierpinskiQuad("Vulkan.HelloSierpinskiQuad", 6, s_graphicsProviderVulkan),

            new EnumerateAudioAdapters("PulseAudio.EnumerateAudioAdapters.Sync", false, s_audioProviderPulseAudio),
            new EnumerateAudioAdapters("PulseAudio.EnumerateAudioAdapters.Async", true, s_audioProviderPulseAudio),

            new PlaySampleAudio("PulseAudio.PlaySampleAudio", s_audioProviderPulseAudio),
        };

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

        private static bool IsSupported(Sample sample)
        {
            bool isSupported;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                isSupported = !sample.CompositionAssemblies.Contains(s_audioProviderPulseAudio);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                isSupported = !sample.CompositionAssemblies.Contains(s_graphicsProviderD3D12);
            }
            else
            {
                isSupported = false;
            }

            return isSupported;
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

            foreach (var sample in s_samples)
            {
                if (IsSupported(sample))
                {
                    Console.WriteLine($"    {sample.Name}");
                }
            }
        }

        private static void Run(Sample sample)
        {
            using var application = new Application(sample.CompositionAssemblies);
            sample.Initialize(application);

            application.Run();
            sample.Cleanup();
        }

        private static void RunSamples(string[] args)
        {
            var ranAnySamples = false;

            if (args.Any((arg) => Matches(arg, "all")))
            {
                foreach (var sample in s_samples)
                {
                    if (IsSupported(sample))
                    {
                        RunSample(sample);
                        ranAnySamples = true;
                    }
                }
            }

            foreach (var arg in args)
            {
                foreach (var sample in s_samples.Where((sample) => arg.Equals(sample.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    if (IsSupported(sample))
                    {
                        RunSample(sample);
                        ranAnySamples = true;
                    }
                }
            }

            if (ranAnySamples == false)
            {
                PrintHelp();
            }
        }

        private static void RunSample(Sample sample)
        {
            Console.WriteLine($"Running: {sample.Name}");
            var thread = new Thread(() => Run(sample));
            
            thread.Start();
            thread.Join();
        }
    }
}
