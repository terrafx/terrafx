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

internal abstract unsafe class Sample(string name) : IDisposable
{
    private readonly string _assemblyPath = Path.GetDirectoryName(AppContext.BaseDirectory)!;
    private readonly string _name = name;
    private TimeSpan _timeout;

    ~Sample() => Dispose(isDisposing: false);

    private static ReadOnlySpan<byte> D3D12CompileTarget_ps_5_0 => "ps_5_0"u8;

    private static ReadOnlySpan<byte> D3D12CompileTarget_vs_5_0 => "vs_5_0"u8;

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

            var result = D3DCompileFromFile(assetPath, pDefines: null, D3D_COMPILE_STANDARD_FILE_INCLUDE, entryPoint, (sbyte*)GetD3D12CompileTarget(kind).GetPointerUnsafe(), compileFlags, Flags2: 0, &d3dShaderBlob, ppErrorMsgs: &d3dShaderErrorBlob);

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
                    case GraphicsShaderKind.Unknown:
                    {
                        ThrowForInvalidKind(kind);
                        break;
                    }
                }
            }

            if (d3dShaderBlob != null)
            {
                _ = d3dShaderBlob->Release();
            }

            if (d3dShaderErrorBlob != null)
            {
                _ = d3dShaderErrorBlob->Release();
            }

            if (result.FAILED)
            {
                var errorMsg = GetUtf8Span((sbyte*)d3dShaderErrorBlob->GetBufferPointer(), (int)d3dShaderErrorBlob->GetBufferSize()).GetString();
                Console.WriteLine(errorMsg);
                ExceptionUtilities.ThrowExternalException(nameof(D3DCompileFromFile), result);
            }

            AssertNotNull(graphicsShader);
            return graphicsShader;
        }

        static ReadOnlySpan<byte> GetD3D12CompileTarget(GraphicsShaderKind graphicsShaderKind)
        {
            ReadOnlySpan<byte> d3d12CompileTarget;

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
                case GraphicsShaderKind.Unknown:
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
