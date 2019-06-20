namespace TerraFX.Audio
{
    /// <summary>Options for a <see cref="AudioDecoder"/>.</summary>
    public class AudioDecoderOptions
    {
        /// <summary>The sample rate of input data.</summary>
        public int SampleRate { get; set; }

        /// <summary>The number of input channels.</summary>
        public int Channels { get; set; }
    }
}
