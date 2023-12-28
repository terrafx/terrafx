// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.IO;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Interop.DirectX;
using TerraFX.Utilities;
using static TerraFX.Interop.DirectX.D3D;
using static TerraFX.Interop.DirectX.D3DCOMPILE;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using GC = System.GC;

namespace TerraFX.Samples;

public abstract unsafe class Sample : IDisposable
{
    private readonly string _assemblyPath;
    private readonly string _name;
    private TimeSpan _timeout;

    protected Sample(string name)
    {
        _assemblyPath = Path.GetDirectoryName(AppContext.BaseDirectory)!;
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
        ThrowIfNull(application);
        _timeout = timeout;
        application.Idle += OnIdle;
    }

    protected GraphicsShader CompileShader(GraphicsDevice graphicsDevice, GraphicsShaderKind kind, string shaderName, string entryPointName)
    {
        ThrowIfNull(graphicsDevice);

        GraphicsShader? graphicsShader = null;

        fixed (char* assetPath = GetAssetFullPath("Shaders", shaderName, $"{shaderName}{kind}.hlsl"))
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

            var result = D3DCompileFromFile(assetPath, pDefines: null, D3D_COMPILE_STANDARD_FILE_INCLUDE, entryPoint, GetD3D12CompileTarget(kind).GetPointerUnsafe(), compileFlags, Flags2: 0, &d3dShaderBlob, ppErrorMsgs: &d3dShaderErrorBlob);

            if (result.SUCCEEDED)
            {
                var bytecode = new UnmanagedArray<byte>(d3dShaderBlob->GetBufferSize());
                new UnmanagedReadOnlySpan<byte>((byte*)d3dShaderBlob->GetBufferPointer(), bytecode.Length).CopyTo(bytecode);

                switch (kind)
                {
                    case GraphicsShaderKind.Pixel:
                    {
                        graphicsShader = graphicsDevice.CreatePixelShader(bytecode, entryPointName);
                        break;
                    }

                    case GraphicsShaderKind.Vertex:
                    {
                        graphicsShader = graphicsDevice.CreateVertexShader(bytecode, entryPointName);
                        break;
                    }

                    default:
                    {
                        ThrowForInvalidKind(kind);
                        break;
                    }
                }
            }

#pragma warning disable CA1508 // Avoid dead conditional code
            if (d3dShaderBlob != null)
            {
                _ = d3dShaderBlob->Release();
            }

            if (d3dShaderErrorBlob != null)
            {
                _ = d3dShaderErrorBlob->Release();
            }
#pragma warning restore CA1508 // Avoid dead conditional code

            if (result.FAILED)
            {
                var errorMsg = GetUtf8Span((sbyte*)d3dShaderErrorBlob->GetBufferPointer(), (int)d3dShaderErrorBlob->GetBufferSize()).GetString();
                Console.WriteLine(errorMsg);
                ExceptionUtilities.ThrowExternalException(nameof(D3DCompileFromFile), result);
            }

            AssertNotNull(graphicsShader);
            return graphicsShader;
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
