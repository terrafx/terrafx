// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Interop.DirectX;
using static TerraFX.Interop.DirectX.D3D;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using GC = System.GC;

#if DEBUG
using static TerraFX.Interop.DirectX.D3DCOMPILE;
#endif

namespace TerraFX.Samples;

public abstract class Sample : IDisposable
{
    private readonly string _assemblyPath;
    private readonly string _name;
    private readonly ApplicationServiceProvider _serviceProvider;
    private TimeSpan _timeout;

    protected Sample(string name, ApplicationServiceProvider serviceProvider)
    {
        var entryAssembly = Assembly.GetEntryAssembly()!;
        _assemblyPath = Path.GetDirectoryName(entryAssembly.Location)!;

        _name = name;
        _serviceProvider = serviceProvider;
    }

    ~Sample() => Dispose(isDisposing: false);

    // ps_5_0
    private static ReadOnlySpan<sbyte> D3D12CompileTarget_ps_5_0 => new sbyte[] { 0x70, 0x73, 0x5F, 0x35, 0x5F, 0x30, 0x00 };

    // vs_5_0
    private static ReadOnlySpan<sbyte> D3D12CompileTarget_vs_5_0 => new sbyte[] { 0x76, 0x73, 0x5F, 0x35, 0x5F, 0x30, 0x00 };

    public ApplicationServiceProvider ServiceProvider => _serviceProvider;

    public string Name => _name;

    public TimeSpan Timeout => _timeout;

    public virtual void Cleanup() { }

    public void Dispose()
    {
        Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    public virtual void Initialize(Application application, TimeSpan timeout)
    {
        _timeout = timeout;
        application.Idle += OnIdle;
    }

    protected unsafe GraphicsShader CompileShader(GraphicsDevice graphicsDevice, GraphicsShaderKind kind, string shaderName, string entryPointName)
    {
        if (OperatingSystem.IsWindowsVersionAtLeast(10) && (_serviceProvider == Program.s_d3d12GraphicsServiceProvider))
        {
            var assetName = $"{shaderName}{kind}.hlsl";

            fixed (char* assetPath = GetAssetFullPath("Shaders", shaderName, assetName))
            fixed (sbyte* entryPoint = entryPointName.GetUtf8Span())
            {
                var compileFlags = 0u;

#if DEBUG
                // Enable better shader debugging with the graphics debugging tools.
                compileFlags |= D3DCOMPILE_DEBUG | D3DCOMPILE_SKIP_OPTIMIZATION;
#endif
                ID3DBlob* d3dShaderBlob = null;
                ID3DBlob* pError = null;

                try
                {
                    var result = D3DCompileFromFile((ushort*)assetPath, pDefines: null, D3D_COMPILE_STANDARD_FILE_INCLUDE, entryPoint, GetD3D12CompileTarget(kind).GetPointer(), compileFlags, Flags2: 0, &d3dShaderBlob, ppErrorMsgs: &pError);

                    if (FAILED(result))
                    {
                        // todo: var span = TerraFX.Utilities.InteropUtilities.MarshalUtf8ToReadOnlySpan((sbyte*)pError->GetBufferPointer(), (int)pError->GetBufferSize());
                        var errorMsg = System.Text.Encoding.UTF8.GetString((byte*)pError->GetBufferPointer(), (int)pError->GetBufferSize());
                        Console.WriteLine(errorMsg);
                        ThrowExternalException(nameof(D3DCompileFromFile), result);
                    }

                    var bytecode = new UnmanagedArray<byte>(d3dShaderBlob->GetBufferSize());
                    new UnmanagedReadOnlySpan<byte>((byte*)d3dShaderBlob->GetBufferPointer(), bytecode.Length).CopyTo(bytecode);

                    switch (kind)
                    {
                        case GraphicsShaderKind.Pixel:
                        {
                            return graphicsDevice.CreatePixelShader(bytecode, entryPointName);
                        }

                        case GraphicsShaderKind.Vertex:
                        {
                            return graphicsDevice.CreateVertexShader(bytecode, entryPointName);
                        }

                        default:
                        {
                            ThrowForInvalidKind(kind);
                            return null!;
                        }
                    }
                }
                finally
                {
                    if (d3dShaderBlob != null)
                    {
                        d3dShaderBlob->Release();
                    }
                    if (pError != null)
                    {
                        pError->Release();
                    }
                }
            }
        }
        else
        {
            var assetName = $"{shaderName}{kind}.glsl";
            var assetPath = GetAssetFullPath("Shaders", shaderName, assetName);
            var assetOutput = Path.ChangeExtension(assetPath, "spirv");

            var additionalArgs = string.Empty;

#if DEBUG
                // Enable better shader debugging with the graphics debugging tools.
                additionalArgs += $" -g -O0";
#endif

            var glslcProcessStartInfo = new ProcessStartInfo {
                Arguments = $"-fshader-stage={GetVulkanShaderStage(kind)} -o \"{assetOutput}\" -std=450core --target-env=vulkan1.0 --target-spv=spv1.0 -x glsl{additionalArgs} {assetPath}",
                FileName = "glslc",
                WorkingDirectory = Path.GetDirectoryName(assetPath)!,
            };
            Process.Start(glslcProcessStartInfo)!.WaitForExit();

            using var fileReader = File.OpenRead(assetOutput);

            var bytecode = new UnmanagedArray<byte>((nuint)fileReader.Length);
            _ = fileReader.Read(bytecode.AsSpan());

            switch (kind)
            {
                case GraphicsShaderKind.Pixel:
                {
                    return graphicsDevice.CreatePixelShader(bytecode, entryPointName);
                }

                case GraphicsShaderKind.Vertex:
                {
                    return graphicsDevice.CreateVertexShader(bytecode, entryPointName);
                }

                default:
                {
                    ThrowForInvalidKind(kind);
                    return null!;
                }
            }
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
                    ThrowNotImplementedException();
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
                    ThrowNotImplementedException();
                    vulkanShaderStage = string.Empty;
                    break;
                }
            }

            return vulkanShaderStage;
        }
    }

    protected virtual void Dispose(bool isDisposing) => Cleanup();

    protected abstract void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs);

    private string GetAssetFullPath(string assetCategory, string assetFolder, string assetName) => Path.Combine(_assemblyPath, "Assets", assetCategory, assetFolder, assetName);
}
