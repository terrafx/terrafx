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
        private static readonly Assembly s_d3d12Provider = Assembly.LoadFrom("TerraFX.Provider.D3D12.dll");
        private static readonly Assembly s_vulkanProvider = Assembly.LoadFrom("TerraFX.Provider.Vulkan.dll");

        private static readonly Assembly s_pulseAudioProvider = Assembly.LoadFrom("TerraFX.Provider.PulseAudio.dll");

        private static readonly Sample[] s_samples = {
            new EnumerateGraphicsAdapters("D3D12.EnumerateGraphicsAdapters", s_d3d12Provider),
            new EnumerateGraphicsAdapters("Vulkan.EnumerateGraphicsAdapters", s_vulkanProvider),

            new HelloWindow("D3D12.HelloWindow", s_d3d12Provider),
            new HelloWindow("Vulkan.HelloWindow", s_vulkanProvider),

            new EnumerateAudioAdapters("PulseAudio.EnumerateAudioAdapters.Sync", false, s_pulseAudioProvider),
            new EnumerateAudioAdapters("PulseAudio.EnumerateAudioAdapters.Async", true, s_pulseAudioProvider),

            new PlaySampleAudio("PulseAudio.PlaySampleAudio", s_pulseAudioProvider),
            new RecordSampleAudio("PulseAudio.RecordSampleAudio", s_pulseAudioProvider)
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
                isSupported = true;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                isSupported = !sample.CompositionAssemblies.Contains(s_d3d12Provider);
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
            {
                sample.Initialize(application);
            }
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
