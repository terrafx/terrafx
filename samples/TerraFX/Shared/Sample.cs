// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.IO;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Interop.DirectX;
using static TerraFX.Interop.DirectX.D3D;
using static TerraFX.Interop.DirectX.D3DCOMPILE;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using GC = System.GC;

namespace TerraFX.Samples;

public abstract class Sample : IDisposable
{
    private readonly string _assemblyPath;
    private readonly string _name;
    private TimeSpan _timeout;

    protected Sample(string name)
    {
        var entryAssembly = Assembly.GetEntryAssembly()!;
        _assemblyPath = Path.GetDirectoryName(entryAssembly.Location)!;

        _name = name;
    }

    ~Sample() => Dispose(isDisposing: false);

    // ps_5_0
    private static ReadOnlySpan<sbyte> D3D12CompileTarget_ps_5_0 => new sbyte[] { 0x70, 0x73, 0x5F, 0x35, 0x5F, 0x30, 0x00 };

    // vs_5_0
    private static ReadOnlySpan<sbyte> D3D12CompileTarget_vs_5_0 => new sbyte[] { 0x76, 0x73, 0x5F, 0x35, 0x5F, 0x30, 0x00 };

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
        var assetName = $"{shaderName}{kind}.hlsl";

        fixed (char* assetPath = GetAssetFullPath("Shaders", shaderName, assetName))
        fixed (sbyte* entryPoint = entryPointName.GetUtf8Span())
        {
            var compileFlags = 0u;

            if (GraphicsService.EnableDebugMode)
            {
                // Enable better shader debugging with the graphics debugging tools.
                compileFlags |= D3DCOMPILE_DEBUG | D3DCOMPILE_SKIP_OPTIMIZATION;
            }
            else
            {
                compileFlags |= D3DCOMPILE_OPTIMIZATION_LEVEL3;
            }

            ID3DBlob* d3dShaderBlob = null;
            ID3DBlob* d3dShaderErrorBlob = null;

            try
            {
                var result = D3DCompileFromFile((ushort*)assetPath, pDefines: null, D3D_COMPILE_STANDARD_FILE_INCLUDE, entryPoint, GetD3D12CompileTarget(kind).GetPointer(), compileFlags, Flags2: 0, &d3dShaderBlob, ppErrorMsgs: &d3dShaderErrorBlob);

                if (FAILED(result))
                {
                    // todo: var span = TerraFX.Utilities.InteropUtilities.MarshalUtf8ToReadOnlySpan((sbyte*)pError->GetBufferPointer(), (int)pError->GetBufferSize());
                    var errorMsg = System.Text.Encoding.UTF8.GetString((byte*)d3dShaderErrorBlob->GetBufferPointer(), (int)d3dShaderErrorBlob->GetBufferSize());
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
                    _ = d3dShaderBlob->Release();
                }

                if (d3dShaderErrorBlob != null)
                {
                    _ = d3dShaderErrorBlob->Release();
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
    }

    protected virtual void Dispose(bool isDisposing) => Cleanup();

    protected abstract void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs);

    private string GetAssetFullPath(string assetCategory, string assetFolder, string assetName) => Path.Combine(_assemblyPath, "Assets", assetCategory, assetFolder, assetName);
}
