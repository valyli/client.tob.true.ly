# Reference
## WebGL Browser Compatibility
* [Unity manual](https://docs.unity3d.com/2019.1/Documentation/Manual/webgl-browsercompatibility.html)
    * iOS Safari not to support WebGL2.0
* [webreport.com](https://webglreport.com/?v=1)
    * Check browser's compatibility with WebGL versions.
    * Check compatibility results:
    
        | OS      | Browser | WebGL 1.0 | WebGL 2.0 |
        | :------ | :------ | :-------: | :-------: |
        | Windows | Chrome  |     Y     |     Y     |
        | Android | Chrome  |     Y     |     Y     |
        | iOS     | Chrome  |     Y     |     Y     |
        | iOS     | Safari  |     Y     |     Y     |
    * Those result above conflict with Unity document.

# Prepare Environment
## Unity
* Version: 2021.3.8f1c1 LTS
* Create URP project
    ![](vx_images/374363510221498.png)
* Disable compression
    ![](vx_images/172782410225108.png)

## Browser Version
* Windows 10
    * Chrome, Version 107.0.5304.88 (Official Build) (64-bit)
* Android 
    * Chrome, Beta 109.0.5414.23
* iOS
    * Chrome, 108.0.5359.52
    * Safari, (iOS 15.6.1)

## Screen Size:
* PC: 960*600
* Android: 2283x672
* iOS: 2424x1074
* SRP fallback in WebGL?
    ![](vx_images/419892710221359.png)
* And incur too much Drawcall(batches)
    ![](vx_images/423621711233588.png)

## Device
* Major:
    * iOS: XR
    * Android: Vivo x70pro
* Minor:
    * iOS: 6 plus
    * Android: Xiaomi 11

## Graphics Device
[About graphicsShaderLevel](https://docs.unity3d.com/2019.1/Documentation/ScriptReference/SystemInfo-graphicsShaderLevel.html)

| System  | graphicsDeviceType |               graphicsDeviceVersion               | graphicsShaderLevel | 
| :------ | :----------------- | :-----------------------------------------------: | :-----------------: |
| Windows | OpenGLES3          | OpenGL ES 3.0 (WebGL 2.0(OpenGL ES 3.0 Chromium)) |         35          |
| Android | OpenGLES3          | OpenGL ES 3.0 (WebGL 2.0(OpenGL ES 3.0 Chromium)) |         35          |
| iOS     | OpenGLES3          |             OpenGL ES 3.0 (WebGL 2.0)             |         35          |


# Test Record 
| No. | Base On |                                   Changing                                   | iOS FPS | Android FPS | PC FPS |                         Comment                         |
| :-- | :------ | :--------------------------------------------------------------------------: | :-----: | :---------: | :----: | ------------------------------------------------------- |
| 1   | 1       |                               Default Setting                                |    5    |     59      |   59   | SRP not available                                       |
| 2   | 1       |                                 DXT -> ASTC                                  |    5    |     59      |   59   |                                                         |
| 3   | 1       |                         Color Space: Linear -> Gamma                         |    5    |     59      |   59   |                                                         |
| 4   | 3       |                          Auto Graphics API: Enable                           |    5    |     59      |   59   | Has rebuild shaders                                     |
| 5   | 4       |                      Lightmap encoding: High -> Normal                       |    5    |     59      |   59   | Wrong color on all platform                             |
| 6   | 3       |                          Graphics API: Add WebGL 1                           |    5    |     59      |   59   |                                                         |
| 7   | 6       |                         Graphics API: Remove WebGL 2                         |   31    |     59      |   59   |                                                         |
| 8   | 7       |        Turn off Example Assets. And Application.targetFrameRate = 60;        |  31/59  |     59      |   59   | Turn off and on at iOS, increase FPS 30->38             |
| 9   | 8       |                      Application.targetFrameRate = 100;                      |   30    |     59      |   59   | Top limited FPS on mobile is 60                         |
| 10  | 9       |        Post-process Volume: Increase parameters to enhance visibility        |   30    |     59      |   59   | Sure: Post-process Volume not available                 |
| 11  | 9       |                   Vsync Count: Every V Blank -> Don't Sync                   |   30    |     59      |   59   |                                                         |
| 12  | 9       |       Color Space: Gamma -> Linear & Graphics API: Remove WebGL 1 -> 2       |    5    |     59      |   59   | OK: Post-process Volume available. But iOS only 5 FPS   |
| 13  | 12      |                         Turn off half batch in scene                         |    7    |     59      |   59   |                                                         |
| 14  | 13      | Scene from Three.js, Close Fog & Post-process, Linear -> Gamma, WebGL 2 -> 1 |   59    |     59      |   59   | Use same resources to compare with three.js (WebGL 1.0) |
| 15  | 14      |                        Gamma -> Linear, WebGL 1 -> 2                         |   59    |     59      |   59   | Use same resources to compare with three.js (WebGL 2.0) |

# Comparison
## [forest](https://test.looc.io/forest/index.html)
* Mac (lijia):
    * **[BAD]** Safari (15.6): 2~4 fps
    * Chrome: 14~18 fps
* PC (Chrome): 
    * lijia: 60 fps 
    * wanglei: 35 fps 
* **[BAD]** iOS (liuwei, Safari 14): 40~50 fps. But other said it is bad on Ventura 13.1.1 and Safari 16.1.1 5. [ref1](https://developer.apple.com/forums/thread/696821)  [ref2](https://developer.apple.com/forums/thread/672478)
* Android: Unknown (Can not open)
* Build with Unity.
## [smashkarts.io](https://smashkarts.io/)
* Mac:
    * Chrome: 60 fps
    * Safari: **Crash**, 25~30 fps
* iOS (xr):
    * Chrome: 30~40 fps
    * Safari: **Crash**, 27~29 fps
## Our Demo (version 11 & 12)
* Mac (lijia):
    * Chrome: 
        * WebGL1.0: 59 fps
        * WebGL 2.0: 59 fps
    * Safari: 
        * WebGL1.0: 59 fps
        * **[BAD]** WebGL2.0: 8 fps
* iOS (xr):
    * Chrome / Safari: 
        * WebGL1.0: 30 fps
        * **[BAD]** WebGL 2.0: 5 fps
* iOS (8 plus)
    * Safari (14):
        * WebGL1.0: 60fps
        * WebGL2.0: Not to support.

## Our Demo (version 13, WebGL2.0): Compare Variant Browsers
Turn off *EnableExampleAssets*. Remove overload to reveal deversities among each browsers.
![](vx_images/229673818239674.png)

* Safari:  fps.
* Chrome:  fps.
* FireFox:  fps.
* Edge:  fps.
* Opera:  fps.

## Build Demo In Different Unity Version
* 2021.3.8f1c1 LTS & 2023.1.0a21 ALPHA got same results.

## Three.js
[demo1](https://threejs.org/examples/#webgl_animation_keyframes)
> Important steps:
> * Deploy Three.js examples on local PC, and change WebGL versions in testing.
> * Must attend to avoid cached files in Safari. Open new tab for each testing.
```javascript
const renderer = new THREE.WebGL1Renderer( { antialias: true } );    // WebGL 1.0
//const renderer = new THREE.WebGLRenderer( { antialias: true } );   // WebGL 2.0 (default)
console.info("renderer.isWebGL1Renderer = " + renderer.isWebGL1Renderer );
```
* Three.js

| WebGL | Platform | Safari | Chrome | 
| :---- | :------: | :----: | :----: |
| 1.0   |   iOS    |   60   |   60   |
|       | Android  |   NA   |  HIGH  |
| 2.0   |   iOS    |   30   |   60   |
|       | Android  |   NA   |  LOW   |
> Note: 
>    * Meshes are black on Vivo x70pro in WebGL 1.0. And slow in WebGL 2.0
>    * Test on Xiaomi 11 again. And HIGH >= LOW + 40%, all above 100 FPS.
>    * Sometimes, it could hit 60 fps on iOS in WebGL2.0 in Safari. And not stable. Sometimes just 15 fps.

* Unity

| WebGL   | Platform | Safari | Chrome |
| :------ | :------: | :----: | :----: |
| 1.0(14) |   iOS    |   60   |   60   |
|         | Android  |   NA   |   60   |
| 2.0(15) |   iOS    |   20   |   14   |
|         | Android  |   NA   |   60   |


* WebGL 2.0
* iOS:
    * Safari: 30 fps
    * Chrome: 60 fps
* The bug same as Unity.
    * PC: 60 fps
    * iOS: 30 fps
    * Android: 30~60 fps, and un-stable.


# About Safari / iOS
## Reference
[SAFARI 15.2 WEBGL performance disaster](https://developer.apple.com/forums/thread/696821)
> * Risk: Safari update may cause new problems. Such as: should change WebGL 2.0 ->1.0.

[Frame-rate drops at game beginning on iOS safari?](https://forum.unity.com/threads/frame-rate-drops-at-game-beginning-on-ios-safari.1064447/)
```csharp
PlayerSettings.WebGL.emscriptenArgs = "-s msimd128";
BuildOptions option = BuildOptions.None;          
BuildPipeline.BuildPlayer(GetScenePaths(), "webgl", BuildTarget.WebGL, option);
```
* This code is failed on 2021.3.8f1c1 LTS & 2023.1.0a21 ALPHA:
    ![](vx_images/33654314239923.png)
* Try another arguments failed 
    
[Removing support for GLES2 and WebGL1 in 2023.1a](https://forum.unity.com/threads/removing-support-for-gles2-and-webgl1-in-2023-1a.1360090/)
> * Roadmap: Unity will focus on GLES3 and WebGL 2 graphics APIs.
>     They removed the choice of *Graphics APIs*  in *PlayerSetting* on 2023 version.
>    ![](vx_images/557484212238628.png)

[Issue Track: WEBGL BAD PERFORMANCE WHEN PLAYING IN SAFARI](https://issuetracker.unity3d.com/issues/webgl-bad-performance-when-playing-in-safari)
> * This is an issue with the Safari browser and its performance for WebGL being converted to Metal. We've reported the issue to Apple, and have alerted the other interested WebGL parties external to Unity, but from our side, there's nothing we can do.

[Search issue on Unity](https://unity3d.com/search?refinement=issues&gq=fps%20ios)

## Inference
* *Metal* of *Apple* in some new versions of iOS system have bugs on WebGL supporting. Bugs:
    * Low FPS for WebGL 2.0. Less than 5 FPS.
    * Low FPS for WebGL 1.0. Halve FPS.
    * Low FPS in 20~60 seconds at the beginning. Lost 20~30% performance.
* iOS support WebGL 1.0 better if version less than or equal 14.
* iOS has those bugs if version more than 14.
* Bugs are not relevant to browsers used. Such as *Safari*, *Edge*, *FireFox* or *Chrome*. They show those bugs at all.

# Conclusion
* iOS Metal has bug on WebGL 2.0 in some versions. The FPS is too low. 
* So we should use 
2 factions: Metal, ??
Which version update to Metal driver?
android try to use urp
add vconsole in test
check batch on webgl or render debug
control webgl in js or c# code
must combine avatar meshes

resolution:
webgl 1.0
self browser ?


? force to webgl 1.0 in unity by javascript. because 1.0 is twice of 2.0 in three.js on ios.
