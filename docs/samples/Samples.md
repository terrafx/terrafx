# Samples

TerraFX provides multiple samples; both to showcase the current features and help provides examples of how to utilize the framework.

Each example will give a brief explanation, share the that can be used to run the sample, and provide a screenshot showing an example of the output.

## Availability

Most samples are cross-platform and provide support for executing on any support platform/architecture. Some samples, however, are only available on certain systems. For example, most Direct3D 12 samples are only available on Windows. There are, however, Vulkan variants that can be run on Linux.

## [Graphics/EnumerateGraphicsAdapters.cs](../../samples/TerraFX/Graphics/EnumerateGraphicsAdapters.cs)

This shows how a user can get a list of the graphics adapters (both physical and virtual) supported by the current system.

### D3D12.EnumerateGraphicsAdapters - Windows

![D3D12.EnumerateGraphicsAdapters - Windows](D3D12.EnumerateGraphicsAdapters%20-%20Windows.png "D3D12.EnumerateGraphicsAdapters - Windows")

### Vulkan.EnumerateGraphicsAdapters - Linux

![Vulkan.EnumerateGraphicsAdapters - Linux](Vulkan.EnumerateGraphicsAdapters%20-%20Linux.png "Vulkan.EnumerateGraphicsAdapters - Linux")

### Vulkan.EnumerateGraphicsAdapters - Windows

![Vulkan.EnumerateGraphicsAdapters - Windows](Vulkan.EnumerateGraphicsAdapters%20-%20Windows.png "Vulkan.EnumerateGraphicsAdapters - Windows")

## [Graphics/HelloWindow.cs](../../samples/TerraFX/Graphics/HelloWindow.cs)

This shows how a user can trivially create a blank window and then "clear" it to the specified color. In this case, [Cornflower Blue](https://en.wikipedia.org/wiki/Cornflower_blue).

### D3D12.HelloWindow - Windows

![D3D12.HelloWindow - Windows](D3D12.HelloWindow%20-%20Windows.png "D3D12.EnumerateGraphicsAdapter - Windows")

### Vulkan.HelloWindow - Linux

![Vulkan.HelloWindow - Linux](Vulkan.HelloWindow%20-%20Linux.png "Vulkan.EnumerateGraphicsAdapter - Linux")

### Vulkan.HelloWindow - Windows

![Vulkan.HelloWindow - Windows](Vulkan.HelloWindow%20-%20Windows.png "Vulkan.EnumerateGraphicsAdapter - Windows")

## [Audio/EnumerateAudioAdapters.cs](../../samples/TerraFX/Audio/EnumerateAudioAdapters.cs)

This shows how a user can get a list of the audio adapters (both physical and virtual) supported by the current system.

### PulseAudio.EnumerateAudioAdapters.Async and PulseAudio.EnumerateAudioAdapters.Sync - Linux

Both PulseAudio.EnumerateAudioAdapters.Async and PulseAudio.EnumerateAudioAdapters.Sync operate the same way, and produce the same output.

![PulseAudio.EnumerateAudioAdapters - Linux](PulseAudio.EnumerateAudioAdapters%20-%20Linux.png)

## [Audio/PlaySampleAudio.cs](../../samples/TerraFX/Audio/PlaySampleAudio.cs)

This shows how a user can easily play a sine wave with a specific frequency, even if the underlying device operates at a different frequency.

### PulseAudio.PlaySampleAudio - Linux

![PulseAudio.PlaySampleAudio - Linux](PulseAudio.PlaySampleAudio%20-%20Linux.png)

[Audio File](PulseAudio.PlaySampleAudio%20-%20Linux.wav)
