// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnitLite;
using System.Reflection;

namespace TerraFX.UnitTests
{
    /// <summary>Provides a set of methods for executing a program.</summary>
    public static class Program
    {
        /// <summary>The entry point of the program.</summary>
        /// <param name="args">The arguments that should be used when running the program.</param>
        /// <returns>The exit code of the program.</returns>
        public static int Main(string[] args)
        {
            var autoRun = new AutoRun(Assembly.GetEntryAssembly());
            return autoRun.Execute(args);
        }
    }
}
