// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Interop;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples
{
    public abstract class Sample
    {
        internal static readonly Assembly s_uiProviderWin32 = Assembly.LoadFrom("TerraFX.UI.Providers.Win32.dll");
        internal static readonly Assembly s_uiProviderXlib = Assembly.LoadFrom("TerraFX.UI.Providers.Xlib.dll");

        private readonly string _assemblyPath;
        private readonly string _name;
        private readonly Assembly[] _compositionAssemblies;

        protected Sample(string name, Assembly[] compositionAssemblies)
        {
            var entryAssembly = Assembly.GetEntryAssembly()!;
            _assemblyPath = Path.GetDirectoryName(entryAssembly.Location)!;

            _name = name;

            _compositionAssemblies = new Assembly[compositionAssemblies.Length + 1];
            _compositionAssemblies[0] = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? s_uiProviderWin32 : s_uiProviderXlib;

            Array.Copy(compositionAssemblies, 0, _compositionAssemblies, 1, compositionAssemblies.Length);
        }

        // ps_5_0
        private static ReadOnlySpan<sbyte> D3D12CompileTarget_ps_5_0 => new sbyte[] { 0x70, 0x73, 0x5F, 0x35, 0x5F, 0x30, 0x00 };

        // vs_5_0
        private static ReadOnlySpan<sbyte> D3D12CompileTarget_vs_5_0 => new sbyte[] { 0x76, 0x73, 0x5F, 0x35, 0x5F, 0x30, 0x00 };

        public Assembly[] CompositionAssemblies => _compositionAssemblies;

        public string Name => _name;

        public virtual void Cleanup()
        {
        }

        public virtual void Initialize(Application application) => application.Idle += OnIdle;

        protected abstract void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs);

        protected unsafe GraphicsShader CompileShader(GraphicsDevice graphicsDevice, GraphicsShaderKind kind, string shaderName, string entryPointName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _compositionAssemblies.Contains(Program.s_graphicsProviderD3D12))
            {
                var assetName = $"{shaderName}{kind}.hlsl";

                fixed (char* assetPath = GetAssetFullPath("Shaders", assetName))
                fixed (sbyte* entryPoint = MarshalStringToUtf8(entryPointName))
                {
                    var compileFlags = 0u;

#if DEBUG
                    // Enable better shader debugging with the graphics debugging tools.
                    compileFlags |= D3DCOMPILE_DEBUG | D3DCOMPILE_SKIP_OPTIMIZATION;
#endif
                    ID3DBlob* d3dShaderBlob = null;

                    try
                    {
                        var result = D3DCompileFromFile((ushort*)assetPath, pDefines: null, (ID3DInclude*)D3D_COMPILE_STANDARD_FILE_INCLUDE, entryPoint, GetD3D12CompileTarget(kind).AsPointer(), compileFlags, Flags2: 0, &d3dShaderBlob, ppErrorMsgs: null);

                        if (FAILED(result))
                        {
                            ThrowExternalException(nameof(D3DCompileFromFile), result);
                        }

                        var shaderBytecode = new ReadOnlySpan<byte>(d3dShaderBlob->GetBufferPointer(), (int)d3dShaderBlob->GetBufferSize());
                        return graphicsDevice.CreateShader(kind, shaderBytecode, entryPointName);
                    }
                    finally
                    {
                        if (d3dShaderBlob != null)
                        {
                            d3dShaderBlob->Release();
                        }
                    }
                }
            }
            else
            {
                var assetName = $"{shaderName}{kind}.glsl";
                var assetPath = GetAssetFullPath("Shaders", assetName);
                var assetOutput = Path.ChangeExtension(assetPath, "spirv");

                var additionalArgs = string.Empty;

#if DEBUG
                // Enable better shader debugging with the graphics debugging tools.
                additionalArgs += $" -g -O0";
#endif

                var glslcProcessStartInfo = new ProcessStartInfo {
                    Arguments = $"-fshader-stage={GetVulkanShaderStage(kind)} -o \"{assetOutput}\" -std=450core --target-env=vulkan1.0 --target-spv=spv1.0 -x glsl{additionalArgs} {assetPath}",
                    FileName = "glslc.exe",
                    WorkingDirectory = Path.GetDirectoryName(assetPath)!,
                };
                Process.Start(glslcProcessStartInfo)!.WaitForExit();

                using var fileReader = File.OpenRead(assetOutput);

                var bytecode = new byte[fileReader.Length];
                _ = fileReader.Read(bytecode);

                return graphicsDevice.CreateShader(kind, bytecode, entryPointName);
            }

            static ReadOnlySpan<sbyte> GetD3D12CompileTarget(GraphicsShaderKind graphicsShaderKind)
            {
                ReadOnlySpan<sbyte> d3d12CompileTarget;

                switch (graphicsShaderKind)
                {
                    case GraphicsShaderKind.Vertex:
                    {
                        d3d12CompileTarget = D3D12CompileTarget_vs_5_0;
                        break;
                    }

                    case GraphicsShaderKind.Pixel:
                    {
                        d3d12CompileTarget = D3D12CompileTarget_ps_5_0;
                        break;
                    }

                    default:
                    {
                        ThrowArgumentOutOfRangeException(nameof(graphicsShaderKind), graphicsShaderKind);
                        d3d12CompileTarget = default;
                        break;
                    }
                }

                return d3d12CompileTarget;
            }

            static string GetVulkanShaderStage(GraphicsShaderKind graphicsShaderKind)
            {
                string vulkanShaderStage;

                switch (graphicsShaderKind)
                {
                    case GraphicsShaderKind.Vertex:
                    {
                        vulkanShaderStage = "vertex";
                        break;
                    }

                    case GraphicsShaderKind.Pixel:
                    {
                        vulkanShaderStage = "fragment";
                        break;
                    }

                    default:
                    {
                        ThrowArgumentOutOfRangeException(nameof(graphicsShaderKind), graphicsShaderKind);
                        vulkanShaderStage = string.Empty;
                        break;
                    }
                }

                return vulkanShaderStage;
            }
        }

        private string GetAssetFullPath(string assetCategory, string assetName) => Path.Combine(_assemblyPath, "Assets", assetCategory, assetName);
    }
}
