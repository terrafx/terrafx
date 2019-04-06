// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.


namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for manipulating floating-point values.</summary>
    public static class FloatUtilities
    {
        #region Static Methods
        /// <summary>Clamps a <see cref="float" /> value to be between a minimum and maximum value.</summary>
        /// <param name="value">The value to restrict.</param>
        /// <param name="min">The minimum value (inclusive).</param>
        /// <param name="max">The maximum value (inclusive).</param>
        /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
        public static float Clamp(float value, float min, float max)
        {
            // The compare order here is important.
            // It ensures we match HLSL behavior for the scenario where min is larger than max.

            var result = value;

            result = (result > max) ? max : result;
            result = (result < min) ? min : result;

            return result;
        }
        #endregion
    }
}
