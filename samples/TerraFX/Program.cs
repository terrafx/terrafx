// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Linq;
using System.IO;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Samples.Graphics;

namespace TerraFX.Samples
{
    public static unsafe class Program
    {
        private static readonly Assembly D3D12Provider = Assembly.LoadFrom("TerraFX.Provider.D3D12.dll");
        private static readonly Assembly VulkanProvider = Assembly.LoadFrom("TerraFX.Provider.Vulkan.dll");

        private static readonly Sample[] Samples = {
            new EnumerateGraphicsAdapters("D3D12.EnumerateGraphicsAdapter", D3D12Provider),
            new EnumerateGraphicsAdapters("Vulkan.EnumerateGraphicsAdapter", VulkanProvider),
        };

        public static void Main(string[] args)
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            if ((args.Length == 0) || args.Any((arg) => Matches(arg, "?", "h", "help")))
            {
                PrintHelp(args);
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

        private static void PrintHelp(string[] args)
        {
            Console.WriteLine("General Options");
            Console.WriteLine("    ALL:     Indicates that all samples should be run.");
            Console.WriteLine();

            Console.WriteLine("Available Samples - Can specify multiple");

            foreach (var sample in Samples)
            {
                Console.WriteLine($"    {sample.Name}");
            }
        }

        private static void Run(Sample sample)
        {
            var application = new Application(sample.CompositionAssemblies);
            {
                application.Idle += sample.OnIdle;
            }
            application.Run();
        }

        private static void RunSamples(string[] args)
        {
            var ranAnySamples = false;

            if (args.Any((arg) => Matches(arg, "all")))
            {
                foreach (var sample in Samples)
                {
                    RunSample(sample);
                    ranAnySamples = true;
                }
            }

            foreach (var arg in args)
            {
                foreach (var sample in Samples.Where((sample) => arg.Equals(sample.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    RunSample(sample);
                    ranAnySamples = true;
                }
            }

            if (ranAnySamples == false)
            {
                PrintHelp(args);
            }
        }

        private static void RunSample(Sample sample)
        {
            Console.WriteLine($"Running: {sample.Name}");
            Run(sample);
        }
    }
}
