// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Utilities;

/// <summary>Provides a set of methods to supplement <see cref="AppContext" />.</summary>
public static unsafe class AppContextUtilities
{
    /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static bool GetAppContextData(string name, bool defaultValue)
    {
        var data = AppContext.GetData(name);

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
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static byte GetAppContextData(string name, byte defaultValue)
    {
        var data = AppContext.GetData(name);

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
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static double GetAppContextData(string name, double defaultValue)
    {
        var data = AppContext.GetData(name);

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
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static short GetAppContextData(string name, short defaultValue)
    {
        var data = AppContext.GetData(name);

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
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static int GetAppContextData(string name, int defaultValue)
    {
        var data = AppContext.GetData(name);

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
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static long GetAppContextData(string name, long defaultValue)
    {
        var data = AppContext.GetData(name);

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
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static nint GetAppContextData(string name, nint defaultValue)
    {
        var data = AppContext.GetData(name);

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
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static sbyte GetAppContextData(string name, sbyte defaultValue)
    {
        var data = AppContext.GetData(name);

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
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static float GetAppContextData(string name, float defaultValue)
    {
        var data = AppContext.GetData(name);

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
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static string GetAppContextData(string name, string defaultValue)
    {
        var data = AppContext.GetData(name);
        return data is string value ? value : defaultValue;
    }

    /// <summary>Gets the value of the app context data associated with a given name or a default value if none exists.</summary>
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static ushort GetAppContextData(string name, ushort defaultValue)
    {
        var data = AppContext.GetData(name);

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
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static uint GetAppContextData(string name, uint defaultValue)
    {
        var data = AppContext.GetData(name);

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
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static ulong GetAppContextData(string name, ulong defaultValue)
    {
        var data = AppContext.GetData(name);

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
    /// <param name="name">The name of the app context data to get.</param>
    /// <param name="defaultValue">The default value returned if no app context data is associated with <paramref name="name" />.</param>
    /// <returns>The value of the app context data associated with <paramref name="name"/> or <paramref name="defaultValue" /> if none exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static nuint GetAppContextData(string name, nuint defaultValue)
    {
        var data = AppContext.GetData(name);

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
    /// <param name="name">The name of the app context data to set.</param>
    /// <param name="value">The new value to assign to the app context data associated with <paramref name="name" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    public static void SetAppContextData<T>(string name, T? value) => AppDomain.CurrentDomain.SetData(name, value);
}
