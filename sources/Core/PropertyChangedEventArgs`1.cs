// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX;

/// <summary>Provides data for a property changed event.</summary>
/// <typeparam name="T">The type of the property that was changed.</typeparam>
/// <remarks>Initializes a new instance of the <see cref="PropertyChangedEventArgs{T}" /> class.</remarks>
/// <param name="previousValue">The previous value of the property that was changed.</param>
/// <param name="currentValue">The current value of the property that was changed.</param>
public sealed class PropertyChangedEventArgs<T>(T previousValue, T currentValue) : EventArgs
{
    private readonly T _previousValue = previousValue;
    private readonly T _currentValue = currentValue;

    /// <summary>Gets the current value of the property that was changed.</summary>
    public T CurrentValue => _currentValue;

    /// <summary>Gets the previous value of the property that was changed.</summary>
    public T PreviousValue => _previousValue;
}
