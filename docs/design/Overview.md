# Overview

TerraFX aims, first and foremost, to be composable and lightweight. That is, it
aims to provide a lot of possible functionality, but users should not be forced
to pick up more than necessary.

Most of this composability is built around the concept of services and consumers.
TerraFX will be defining the general contract for a given component and that will
itself not provide any functionality outside some of the "primitive" concepts. We
will then expose a set of default services that users can pick from that provide
different implementations for a given component. These could be implemented by an
existing framework, in managed code, or some new framework of the services choosing.

The contract defined by TerraFX aims to be general-purpose where possible and to
not dive down into framework specific functionality. Where something is commonly
provided but not necessarily general-purpose, a decision needs to be made on whether
that should be required by the core contract or if it should be considered a valid
extension point that can light up via feature detection.

## Core Concepts

TerraFX aims to allow the application and service to decide how things function
where possible. This also applies for concepts like memory management and I/O.

As such, components should be taking a `System.Buffers.MemoryManager<T>` and
working with `Memory<T>` and `Span<T>` where possible. They should also largely
operate using the `System.IO.Pipeline` functionality whenever dealing with I/O.

Components should be multi-thread aware and use `async` where possible. The application
model will be taking a `System.Threading.Tasks.TaskScheduler` in order to allow
applications to provide a customized environment where that is important.

Interop should be explicitly blittable to avoid runtime overhead and any implicit
costs associated with marshalling. Interop should also be isolated to interop
specific projects and should not live in the services themselves. This allows
better reuse and for the interop functionality to be changed without updating
the service.

## General Structure

TerraFX is laid out such that each component lives in its own library. The current
goals involve providing the following components for v1.0:
* Audio
* Graphics
* Input
* User Interface

There will also be the following projects which help bring everything together.

### Core

The core library is meant to contain the shared types that all components depend
on and should ideally be the only dependency for any given component library.
This should help keep a clean layering and reduce dependencies between the components.

### Transformations Pipeline

The transformations pipeline is meant to provide a mechanism through which components
can take their data and transform it as needed. In the graphics component, this
might provide simple transformations like rotation or scaling, and it might also
provide more complex transforms like shaders. In the audio component, this might
provide functionality like transcoding, mixing, or reverberation.

### Scene Graph

The scene graph is meant to provide a way to provide component data for the
application. It should allow a user to provide additional metadata on any
given node, to provide transformations that should occur, and to allow basic
event driven actions to occur. For example, one might want to provide
positional metadata to a node that contains sound metadata or one might want to
indicate that a mesh should be rotated 37 degrees before being rendered. One
might also want to indicate that when a mesh reaches a certain position, that a
sound should being playing.

### Application Model

The application model is meant to be the driver for the entire system. It should
contain the logic for bringing the components online and then making things
happen while the it is running. It is responsible for bringing in the data from
the scene graph, executing the transformations, and driving the events for the
system as needed.
