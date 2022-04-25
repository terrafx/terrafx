# Samples

TerraFX provides multiple samples; both to showcase the current features and help provides examples of how to utilize the framework.

Each example will give a brief explanation, share the that can be used to run the sample, and provide a screenshot showing an example of the output.

## [Graphics/EnumerateGraphicsAdapters.cs](../../samples/TerraFX/Graphics/EnumerateGraphicsAdapters.cs)

This shows how a user can get a list of the graphics adapters (both physical and virtual) supported by the current system.

![Enumerate Graphics Adapters Sample Image](https://blob.terrafx.dev/images/samples/EnumerateGraphicsAdapters.png)

## [Graphics/HelloWindow.cs](../../samples/TerraFX/Graphics/HelloWindow.cs)

This shows how a user can trivially create a blank window and then "clear" it to the specified color. In this case, [Cornflower Blue](https://en.wikipedia.org/wiki/Cornflower_blue).

![Hello Window Sample Image](https://blob.terrafx.dev/images/samples/HelloWindow.png)

## [Graphics/HelloTriangle.cs](../../samples/TerraFX/Graphics/HelloTriangle.cs)

This expands on HelloWindow and renders a single triangle using vertice coloring.

![Hello Triangle Sample Image](https://blob.terrafx.dev/images/samples/HelloTriangle.png)

## [Graphics/HelloQuad.cs](../../samples/TerraFX/Graphics/HelloQuad.cs)

This expands on HelloTriangle to render a square using a second triangle. It adds in the concept of index buffers so 4 vertices, rather than 6 can be specified.

![Hello Quad Sample Image](https://blob.terrafx.dev/images/samples/HelloQuad.png)

## [Graphics/HelloTransform.cs](../../samples/TerraFX/Graphics/HelloTransform.cs)

This expands on HelloTriangle to move the triangle horizontally across the screen using a constant buffer to supply the transformation.

## [Graphics/HelloTexture.cs](../../samples/TerraFX/Graphics/HelloTexture.cs)

This expands on HelloTriangle by using a texture to color the triangle. It appears as a black and white checkerboard.

![Hello Texture Sample Image](https://blob.terrafx.dev/images/samples/HelloTexture.png)

## [Graphics/HelloTextureTransform.cs](../../samples/TerraFX/Graphics/HelloTextureTransform.cs)

This effectively combines HelloTexture and HelloTransform showing how both constant buffers and textures can be supplied to the shader. The triangle moves in a circle rather than horizontally.

## [Graphics/HelloTexture3D.cs](../../samples/TerraFX/Graphics/HelloTexture3D.cs)

This expands on HelloQuad by using a texture to color the square. It uses a 3D rather than 2D texture which allows it to appear animated.

![Hello Texture3D Sample Image](https://blob.terrafx.dev/images/samples/HelloTexture3D.png)

## [Audio/EnumerateAudioAdapters.cs](../../samples/TerraFX/Audio/EnumerateAudioAdapters.cs)

This shows how a user can get a list of the audio adapters (both physical and virtual) supported by the current system.

## [Audio/PlaySampleAudio.cs](../../samples/TerraFX/Audio/PlaySampleAudio.cs)

This shows how a user can easily play a sine wave with a specific frequency, even if the underlying device operates at a different frequency.
