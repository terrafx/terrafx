using System;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace TerraFX.Audio
{
    public interface IAudioDevice : IDisposable
    {
        PipeWriter Writer { get; }

        Task RunAsync();
    }
}
