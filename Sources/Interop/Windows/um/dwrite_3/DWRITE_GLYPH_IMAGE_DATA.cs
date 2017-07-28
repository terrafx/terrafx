// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    /// <summary>Data for a single glyph from GetGlyphImageData.</summary>
    unsafe public /* blittable */ struct DWRITE_GLYPH_IMAGE_DATA
    {
        #region Fields
        /// <summary>Pointer to the glyph data, be it SVG, PNG, JPEG, TIFF.</summary>
        public /* readonly */ void* imageData;

        /// <summary>Size of glyph data in bytes.</summary>
        public UINT32 imageDataSize;

        /// <summary>Unique identifier for the glyph data. Clients may use this to cache a parsed/decompressed version and tell whether a repeated call to the same font returns the same data.</summary>
        public UINT32 uniqueDataId;

        /// <summary>Pixels per em of the returned data. For non-scalable raster data (PNG/TIFF/JPG), this can be larger or smaller than requested from GetGlyphImageData when there isn't an exact match. For scaling intermediate sizes, use: desired pixels per em * font em size / actual pixels per em.</summary>
        public UINT32 pixelsPerEm;

        /// <summary>Size of image when the format is pixel data.</summary>
        public D2D1_SIZE_U pixelSize;

        /// <summary>Left origin along the horizontal Roman baseline.</summary>
        public D2D1_POINT_2L horizontalLeftOrigin;

        /// <summary>Right origin along the horizontal Roman baseline.</summary>
        public D2D1_POINT_2L horizontalRightOrigin;

        /// <summary>Top origin along the vertical central baseline.</summary>
        public D2D1_POINT_2L verticalTopOrigin;

        /// <summary>Bottom origin along vertical central baseline.</summary>
        public D2D1_POINT_2L verticalBottomOrigin;
        #endregion
    }
}
