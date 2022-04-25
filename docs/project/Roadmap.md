# Roadmap

The roadmap attempts to list the current goals, important dates, and other core information about the project.

TerraFX is still in the early stages of development. For v1.0 to ship, the current minimum goals are to provide Audio, Graphics, Input, and Windowing layers on both systems. Additional components that are desired include a scene graph, transformations pipeline, and basic physics support.

There was previous considerations for supporting Linux and/or MacOS as well, but due to limited time and contributors this was scoped back to Windows only.

## Current Status

The current status for each "component" is listed below. More details about the overal design can be found in the [design](../design) folder.

### General
Each component provides access to a different multimedia "subsystem". These components are brought together in the `Application` which exposes the various components and provides access to the "main loop".

### Audio
The audio subsystem has not started development but will likely use [XAudio2](https://docs.microsoft.com/en-us/windows/win32/xaudio2/xaudio2-introduction) or the Windows Audio Session API ([WASAPI](https://docs.microsoft.com/en-us/windows/win32/coreaudio/wasapi)).

### Graphics
The graphics subsystem is being actively developed using [Direct3D 12](https://docs.microsoft.com/en-us/windows/win32/direct3d12/what-is-directx-12-) as the primary backend. Currently users can render a clear screen of any specified color. Adding support for rendering a set of primitives and meshes are next on the list.

### Input
The input subsystem has not started development but will provide support for both Keyboard and Mouse input at a minimum. This will likely utilize the underlying window messaging model for retrieving the input events.

### Windowing
A minimal windowing subsystem already exists and utilizes the [Win32 "WinMsg"](https://docs.microsoft.com/en-us/windows/win32/api/_winmsg/) APIs. Currently this allows users to create barebones windows, perform basic operations such as resizing or moving, and manage their lifetimes. This subsystem includes an abstraction over the "message pump".
