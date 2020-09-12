# AKStandard Item Description
## Rendering Mode
* Opaque - for drawing objects that are completely opaque.
* Cutout - For drawing objects that are clearly transparent or not transparent. Ideal for cutting out.
* Fade - for drawing transparent objects. Transparent - for drawing transparent objects, including reflections.
* Transparent - for drawing transparent objects. Reflection and other reflections are not transparent.
## Main Map
* Main - the appearance settings of the object's surface.
    * Albedo - the basic appearance of the object. Specify by textures and colors.
    * Normal Map - a pseudo representation of how the object's surface will be illuminated based on the texture. You can adjust the intensity with the slider.
    * Hight Map - Pseudo-texture-based shading of the object's surface with unevenness. The intensity can be adjusted with the slider.
    * Occlusion - The shadowing of the object's surface by light is based on the texture. The intensity can be adjusted with the slider.
* Emission - set the emission of the object's surface.
    * Color - color of the emission with texture or color. The Intensity of the color palette allows you to adjust the intensity.
    * Global Illumination - set the impact of the luminescence on the surroundings. This setting only has an effect on light baking.
        * Realtime - luminescence is used to calculate realtime peripheral lighting; it will allow you to light objects that are not set to Static.
        * Baked - the glow is baked into the surrounding Static objects during light baking. All other objects will only be illuminated by the LightProbe setting.
* Reflection - reflections and reflections of the object's surface.
    * Metallic - sets how metallic the object's surface will be in reflection. Textures and sliders allow you to set the intensity.
    * Smoothness - set how smooth the surface of the object is. Textures and sliders allow you to set the intensity. See also: * Source - This lets you set the intensity of the texture and slider.
    * Source - sets the source of the texture reference from which the smoothness is derived.
        * Albedo Alpha - Uses the Alpha value of the Albedo texture.
        * Metallic Alpha - uses the Alpha value of the Metallic texture.
        * Roughness Map - uses the texture specified for Smoothness.
* This is reflected in Albedo and Emission textures.
## Secondary Map
* Detail - the second setting of the object's appearance. It is mainly used to represent the detail of the character's skin.
    * Detail Mask - the area where the detail is displayed is controlled by the mask texture.
    * Detail Albedo - This is the texture that displays the detail.
    * Detail Normal - A normal map for the detail. The intensity is specified as a number instead of a slider.
    * Tilling Offset - This is where you can set up the repetitive display of the detail, etc.
    * The UV Set - the UV map that will determine where the detail will be displayed.
## Options
* Forward Rendering Options - setting the object's surface drawing options.
    * Specular Highlights - Whether or not to use specular reflection highlights.
    * Reflection - whether or not to use reflection reflections.
* Rendering Options - set the general drawing options.
    * Culling Mask - sj for the object's front and back display settings.
        * OFF - draws both the front and back sides of the object.
        * Front - draw only the back side.
        * Back - draw only the front side.
    * Render Queue - set up the order of drawing.
* Advanced Options
    * Enable GPU Instancing - ability to draw identical meshes together. [Details] (https://docs.unity3d.com/ja/2018.4/Manual/GPUInstancing.html)
    * Doubel Sided Global Illumination - uses both sides of an object when calculating ambient light.

Translated with www.DeepL.com/Translator (free version)