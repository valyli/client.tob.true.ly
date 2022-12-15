# Purpose
To research SRP working on different platform.

# Environment
Same as in *UnityWebGLRender.md*
Project: *UrpRenderTest*

# SRP Batcher (Is it enable?)

## Android
* Enabled on default setting.
![](vx_images/121195616227548.png)

| Graphics API | SRP Batcher |
| :----------: | :---------: |
|  OpenGL ES3  |      Y      |
|  OpenGL ES2  |      N      |
|    Vulkan    |      Y      |

* OpenGL ES2 (Android).
    ![](vx_images/368854417236938.png)
    
## iOS
**Wait to test?**

## WebGL

| WebGL | Platform | Safari | Chrome | 
| :---- | :------: | :----: | :----: |
| 1.0   |   iOS    |   N    |   N    |
|       | Android  |   NA   |   N    |
|       |    PC    |   NA   |   N    |
| 2.0   |   iOS    |   Y    |   Y    |
|       | Android  |   NA   |   Y    |
|       |    PC    |   NA   |   Y    |

* But FPS is very slow in WebGL 2.0 in iOS.

![](vx_images/24295416239681.png)

# SRP Fallback
* SRP working
    ![](vx_images/534895916247714.png)
* SRP not work
    ![](vx_images/133660017240383.png)
    * Lost Post-processing.
    * PBR is keeping.

# Conclusion 
* SRP Batcher can not work in WebGL 1.0.
* SRP will be fallback in WebGL 1.0.
* SRP fallback will lost too many details in rendering result.



