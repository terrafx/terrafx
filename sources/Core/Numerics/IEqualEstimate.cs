// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using TerraFX.Numerics;
using TerraFX.Utilities;

namespace TerraFX.Numerics
{

    /// <summary>An interface for the IsSimilarTo() method.</summary>
    /// <typeparam name="T">The type for whick IsSimilarTo() needs to be implemented.</typeparam>
    public interface IEqualEstimate<T>
    {
        /// <summary>Returns true if the two instances (this and right) are within numerical epsilon distance from each other.</summary>
        /// <param name="right">The right instance to compare.</param>
        /// <param name="epsilon">The threshold below which they are sufficiently similar.</param>
        /// <returns>True if similar, false otherwise.</returns>
        public bool EqualEstimate(T right, T epsilon);
    }
}
