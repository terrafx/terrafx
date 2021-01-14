// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods to supplement <see cref="AppContext" />.</summary>
    public static unsafe class AppContextUtilities
    {
        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static bool GetAppContextData(string dataName, bool defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is bool value)
            {
                return value;
            }
            else if ((data is string stringValue) && bool.TryParse(stringValue, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static byte GetAppContextData(string dataName, byte defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is byte value)
            {
                return value;
            }
            else if ((data is string stringValue) && byte.TryParse(stringValue, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static double GetAppContextData(string dataName, double defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is double value)
            {
                return value;
            }
            else if ((data is string stringValue) && double.TryParse(stringValue, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static short GetAppContextData(string dataName, short defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is short value)
            {
                return value;
            }
            else if ((data is string stringValue) && short.TryParse(stringValue, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static int GetAppContextData(string dataName, int defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is int value)
            {
                return value;
            }
            else if ((data is string stringValue) && int.TryParse(stringValue, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static long GetAppContextData(string dataName, long defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is long value)
            {
                return value;
            }
            else if ((data is string stringValue) && long.TryParse(stringValue, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static nint GetAppContextData(string dataName, nint defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is nint value)
            {
                return value;
            }
            else if ((data is string stringValue) && nint.TryParse(stringValue, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static sbyte GetAppContextData(string dataName, sbyte defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is sbyte value)
            {
                return value;
            }
            else if ((data is string stringValue) && sbyte.TryParse(stringValue, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static float GetAppContextData(string dataName, float defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is float value)
            {
                return value;
            }
            else if ((data is string stringValue) && float.TryParse(stringValue, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static string GetAppContextData(string dataName, string defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is string value)
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static ushort GetAppContextData(string dataName, ushort defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is ushort value)
            {
                return value;
            }
            else if ((data is string stringValue) && ushort.TryParse(stringValue, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static uint GetAppContextData(string dataName, uint defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is uint value)
            {
                return value;
            }
            else if ((data is string stringValue) && uint.TryParse(stringValue, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static ulong GetAppContextData(string dataName, ulong defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is ulong value)
            {
                return value;
            }
            else if ((data is string stringValue) && ulong.TryParse(stringValue, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
        /// <param name="dataName">The name of the app context data to get.</param>
        /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="dataName" />.</param>
        /// <returns>The value of the app context data associated with <paramref name="dataName"/> or <paramref name="defaultValue" /> if none exists.</returns>
        public static nuint GetAppContextData(string dataName, nuint defaultValue)
        {
            var data = AppContext.GetData(dataName);

            if (data is nuint value)
            {
                return value;
            }
            else if ((data is string stringValue) && nuint.TryParse(stringValue, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Sets the value of the app context data associated with a given name.</summary>
        /// <param name="dataName">The name of the app context data to set.</param>
        /// <param name="value">The new value to assign to the app context data associated with <paramref name="dataName" />.</param>
        public static void SetAppContextData<T>(string dataName, T value) => AppDomain.CurrentDomain.SetData(dataName, value);
    }
}
