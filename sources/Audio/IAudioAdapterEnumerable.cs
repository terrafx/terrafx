// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;

namespace TerraFX.Audio
{
    /// <summary>Represents an enumerable collection of audio adapters.</summary>
    public interface IAudioAdapterEnumerable
        : IEnumerable<IAudioAdapter>, IAsyncEnumerable<IAudioAdapter>
    { }
}
