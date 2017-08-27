// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from DXSample.h in https://github.com/Microsoft/DirectX-Graphics-Samples
// Original source is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.IO;
using TerraFX.Interop;
using static TerraFX.Interop.D3D_FEATURE_LEVEL;
using static TerraFX.Interop.D3D12;
using static TerraFX.Interop.DXGI_ADAPTER_FLAG;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Windows;
using static TerraFX.Samples.DirectX.D3D12.DXSampleHelper;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples.DirectX.D3D12
{
    public abstract unsafe class DXSample : IDisposable
    {
        #region Fields
        // Viewport dimensions
        protected uint _width;

        protected uint _height;

        protected float _aspectRatio;

        // Adapter info
        protected bool _useWarpDevice;

        // Root assets path
        private string _assetsPath;

        // Window title
        private string _title;
        #endregion

        #region Constructors
        protected DXSample(uint width, uint height, string name)
        {
            _width = width;
            _height = height;
            _title = name;
            _useWarpDevice = false;
            _assetsPath = GetAssetsPath();
            _aspectRatio = ((float)(width)) / ((float)(height));
        }
        #endregion

        #region Destructors
        ~DXSample()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region Properties
        public uint Width
        {
            get
            {
                return _width;
            }
        }

        public uint Height
        {
            get
            {
                return _height;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
        }
        #endregion

        #region Methods
        public abstract void OnInit();

        public abstract void OnUpdate();

        public abstract void OnRender();

        public abstract void OnDestroy();

        // Samples override the event handlers to handle specific messages
        public virtual void OnKeyDown(byte key) { }

        public virtual void OnKeyUp(byte key) { }

        public void ParseCommandLineArgs(string[] args)
        {
            foreach (var arg in args)
            {
                if (string.IsNullOrEmpty(arg) || ((arg[0] != '-') && (arg[0] != '/')))
                {
                    continue;
                }

                if ((arg.Length == 5) && (string.Compare(arg, 1, " (WARP)", 2, 4, StringComparison.OrdinalIgnoreCase) == 0))
                {
                    _useWarpDevice = true;
                    _title += " (WARP)";
                }
            }
        }

        protected virtual void Dispose(bool isDisposing)
        {

        }

        // Helper function for resolving the full path of assets
        protected string GetAssetFullPath(string assetName)
        {
            return Path.Combine(_assetsPath, assetName);
        }

        // Helper function for acquiring the first available hardware adapter that supports Direct3D 12.
        // If no such adapter can be found, returns null.
        protected IDXGIAdapter1* GetHardwareAdapter(IDXGIFactory2* pFactory)
        {
            IDXGIAdapter1* adapter;
            var EnumAdapters1 = MarshalFunction<IDXGIFactory1.EnumAdapters1>(pFactory->lpVtbl->BaseVtbl.EnumAdapters1);

            for (var adapterIndex = 0u; DXGI_ERROR_NOT_FOUND != EnumAdapters1((IDXGIFactory1*)(pFactory), adapterIndex, &adapter); ++adapterIndex)
            {
                var GetDesc1 = MarshalFunction<IDXGIAdapter1.GetDesc1>(adapter->lpVtbl->GetDesc1);

                DXGI_ADAPTER_DESC1 desc;
                GetDesc1(adapter, &desc);

                if ((desc.Flags & (uint)(DXGI_ADAPTER_FLAG_SOFTWARE)) != 0)
                {
                    // Don't select the Basic Render Driver adapter.
                    // If you want a software adapter, pass in "/warp" on the command line.
                    continue;
                }

                // Check to see if the adapter supports Direct3D 12, but don't create the
                // actual device yet.
                var iid = IID_ID3D12Device;
                if (SUCCEEDED(D3D12CreateDevice((IUnknown*)(adapter), D3D_FEATURE_LEVEL_11_0, &iid, null)))
                {
                    break;
                }
            }

            return adapter;
        }

        protected void SetCustomWindowText(string text)
        {
            fixed (char* windowText = $"{_title}: {text}")
            {
                SetWindowText(Win32Application.Hwnd, windowText);
            }
        }
        #endregion

        #region System.IDisposable Methods
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
