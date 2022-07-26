# Reference
[How to use IndexedDB](https://hacks.mozilla.org/2012/02/storing-images-and-files-in-indexeddb/)
[IndexedDB API](https://developer.mozilla.org/en-US/docs/Web/API/IndexedDB_API/Using_IndexedDB)
[W3C](https://www.w3.org/TR/IndexedDB/)
[Want to load from db in js synchronize??](javascript promise)
[Interact between U3D and JS](https://docs.unity3d.com/Manual/webgl-interactingwithbrowserscripting.html)
[Interact between U3D and JS 2](https://docs.unity3d.com/2022.2/Documentation/Manual/webgl-interactingwithbrowserscripting.html)
[WebGL memory & Asset Data in U3D](https://docs.unity3d.com/2020.1/Documentation/Manual/webgl-memory.html)
[Customize the WebGL Cache behavior](https://docs.unity3d.com/2022.2/Documentation/Manual/webgl-caching.html)
[Deploy template of Http Server](https://docs.unity3d.com/2022.2/Documentation/Manual/webgl-server-configuration-code-samples.html)
[Unity-StripCode](https://www.cnblogs.com/littleperilla/p/15997214.html)

## Fully shared generic
[What is?](https://blog.unity.com/technology/feature-preview-il2cpp-full-generic-sharing-in-unity-20221-beta)
```
Wouldn’t it be great to have just one, fully shared generic implementation for any List<T>? Well, check out the IL2CPP Code Generation option “Faster (smaller) builds” in Player Settings. 
```

## WebAssembly
[About WebAssembly](https://www.zhihu.com/question/304577684)
[WebAssembly vs Javascript](https://zhuanlan.zhihu.com/p/57001874)
[Official website](https://webassembly.org/)
> * Not support multi-threads in WebAssembly
> * Compatibility
![](vx_images/213625713247401.png)

## Unity WebGL Templates 
```
C:\Program Files\Unity\Hub\Editor\2021.3.8f1c1\Editor\Data\PlaybackEngines\WebGLSupport\BuildTools\WebGLTemplates
```
# Resolution Test
# Use browser file-cache
* Step:
    1. Browser will cache files after requesting.
    2. Will get file from local cache will request same file again, 
    3. We implement this feature by *UnityWebRequest* in Unity.
* Result:
    1. *UnityWebRequest* could load file from cache by URL.
    2. *UnityWebRequest* just only support asynchronizing mode.
* Problem:
    1. Because of difference operations between asynchronizing of *UnityWebRequest* and synchronizing of *FileSystem* in *GameFramework*.
    2. Because of coroutine of Unity and C# must return **IEnumerator** until to root of *MonoBehavior* class to keep current logic holding with **yield return StartCoroutine()**
    3. So we must re-structure*GameFramework* *FileSystem*  if want to implement this feature. But workload is too high.


# Use browser IndexedDB
* Step:
    1. *FileStream* of C# could visit data in IndexedDB after compiled to java script. 
    2. Modify *DefaultResourceHelper* class loading function on Web GL. Use *FileStream* in asynchronizing mode instead of *UnityWebRequest* in synchronizing mode. But also use *callback* to simulate synchronizing.
    3. Set *Resource Mode* of GameFramework on *Updatable*.
    ~~4. Copy resource packages into HTTP server.~~
        ```
        D:\output\Cap1AssetBundle\Full\0_1_0_2\WebGL
        ```
     5. Settings:
     ![](vx_images/245481917227165.png)
     ![](vx_images/384141817239298.png)
    ![](vx_images/590835214220942.png)
* Result:
    This solution is OK.
    1. IndexedDB snapshot:
        ![](vx_images/279801617220872.png)

* GameFramework fork:
https://github.com/valyli/GameFramework

* Next to do:
    0. HybridCLR does not to support WebGL now. [ref](https://focus-creative-games.github.io/hybridclr/supported_platform/#ios)
    1. How to upgrade client resource?
    2. How to upgrade client code? (or use HybridCLR useGlobalIl2cpp=true)
    3. How to resolve strip code problem?
    4. Enhance download speed.
    5. Open gzip for resources. [ref](https://zhuanlan.zhihu.com/p/475307249)
    6. Check is there have IndexedDB flush problem?[ref](https://gamedev.stackexchange.com/questions/184369/file-saved-to-indexeddb-lost-unless-we-change-scenes)
    7. Catch url parameters.[ref](https://blog.csdn.net/xunideshijie/article/details/123795652)
    8.  Interact between Unity and Java script. [ref](https://docs.unity3d.com/Manual/webgl-interactingwithbrowserscripting.html) [ref2](https://www.cnblogs.com/littleperilla/p/15640464.html)
    9. Review on official troubleshooting. [ref](https://docs.unity3d.com/2019.2/Documentation/Manual/webgl-debugging.html)
    10. How to debug webgl via unity tools on pc and mobile? [Not support debug in browser. Can use Profiler to analyze performance]
    11. Support https protocal.
    12. Verify webgl on iOS. [OK, on iOS 15.6]
    13. Resolve striping of code. [OK, by Fully shared generic]
    14. Should we catch exception at outside of WebAssembly and How?
    15. Resovle warning: 
        ```
        warning: 2 FS.syncfs operations in flight at once, probably just doing extra work
        ```
    16. Rendering Limitation on WebGL. [ref](https://www.cnblogs.com/littleperilla/p/15673963.html)
    17. Remove white loading bar. [ref](https://www.cnblogs.com/littleperilla/p/15673963.html)
    18. Unity not support MicroPhone on WebGL. [github](https://github.com/tgraupmann/UnityWebGLMicrophone)
    19. Exception of AOT:
        ```
        d273a9ed-24f7-4e27-88b5-c1e4e13a7d73:3 ExecutionEngineException: Attempting to call method 'UnityGameFramework.Runtime.DefaultTextHelper::Format<System.String, System.String, System.String, System.String, System.String, System.String, System.Single, System.String, System.Exception>' for which no ahead of time (AOT) code was generated.  Consider increasing the --generic-virtual-method-iterations=1 argument

        ```

# Http Server Config
1. Install Tomcat [url](http://10.60.80.2:8099/ftp/tools/webserver/apache-tomcat-9.0.65.exe)
2. Install special JVM for Tomcat [url](http://10.60.80.2:8099/ftp/tools/webserver/jdk-18_windows-x64_bin.exe)
3. Configure JVM path for Tomcat
![](vx_images/339513019245585.png)
4. Copy jar files to Tomcat lib path
    [download jar file](http://10.60.80.2:8099/ftp/tools/webserver/TomcatFilter.jar)
    To path:
    ```shell
    C:\Program Files\Apache Software Foundation\Tomcat 9.0\lib
    ```
5. Configure web.xml in Tomcat conf:
    ```xml
            <param-name>listings</param-name>
            <param-value>true</param-value>
    ```
    
    ```xml
    <welcome-file-list>
        <welcome-file>index.html</welcome-file>
        <welcome-file>index.htm</welcome-file>
        <welcome-file>index.jsp</welcome-file>
    </welcome-file-list>


    <filter>
        <filter-name>httpResponseHeaderFilter</filter-name>
        <filter-class>com.vking.power.web.filter.HttpResponseHeaderFilter</filter-class>
    </filter>

	<filter-mapping>
        <filter-name>httpResponseHeaderFilter</filter-name>
        <url-pattern>*.wasm</url-pattern>
    </filter-mapping>
    ```
6. Configure server.xml in Tomcat conf:
    ```xml
    	<Context path="" docBase="D:/ftp/build_output/" debug="0" reloadable="true" crossContext="true" />
    ```
7. Start Tomcat server.


# Limitation & Peculiarity
[ref1](https://docs.unity3d.com/2021.3/Documentation/Manual/webgl-assetbundles.html)
1. WebGL does not support threading. 
2. Because 1, Unity WebGL builds need to decompress AssetBundle data on the main thread when the download is done, blocking the main thread. 
3. Because 2, AssetBundles are compressed using LZ4 instead.
4. If you need smaller compression sizes than LZ4 delivers, you can configure your web server to use gzip or Brotli compression (on top of LZ4 compression) on your AssetBundles
    
# Optimization
## 1. Reduce code size
> Preparing before testing:
>* To avoid code stripping impact building reuslt, disable *Strip Engine Code* before testing.
>* *GameFramework.dll* must be release version.
> Basic output size:
>     ![](vx_images/582421609227457.png)

    [ref](https://docs.unity3d.com/2021.3/Documentation/Manual/webgl-building.html)

1. Select *Faster(smaller) builds* option. *[code size reduced]* *[impact runtime speed]*
    ![](vx_images/329000509221164.png)
    ![](vx_images/142240609239590.png)

2. Remove *Package* and modify some code use this package. *[code size not to change]*
    ![](vx_images/250395409247623.png)
    ![](vx_images/444755409240292.png)
3. *Code Optimization* select *Size*. *[code size reduced]* *[impact runtime speed]*
    ![](vx_images/112302010236847.png)
    ![](vx_images/336853210232601.png)
4. *Compression Format* select *Gzip*. *[code size reduced]*  
    ![](vx_images/215781611250481.png)
    ![](vx_images/411701611248085.png)
    ![](vx_images/574171711245587.png)
5. *Strip Engine Code* and not to set *link.xml*. Just save 1MB.
    ![](vx_images/268850411236851.png)
    ![](vx_images/288960311240296.png)
# Other Problems:
1. Should disable ***Strip Engine Code*** in Untiy setting.
    ```
    This could be caused by a class being stripped from the build even though it is needed. Try disabling 'Strip Engine Code' in Player Settings.
    ```
2. ExecutionEngineException: Attempting to call method 'Test::OnMessage<Test+AnyEnum>' for which no ahead of time (AOT) code was generated.  Consider increasing the --generic-virtual-method-iterations=1 argument
[feature-preview-il2cpp-full-generic-sharing-in-unity-20221-beta](https://blog.unity.com/technology/feature-preview-il2cpp-full-generic-sharing-in-unity-20221-beta)

3. Sometimes maybe can not save persistent files such as SelfAvatar.json.
    ![](vx_images/249052612226828.png)

4. Log.Info() cause a fault.
    The code is:
    ![](vx_images/318534710233067.png)
    Will cause the fault as below:
    ![](vx_images/229433210237952.png)
    OR
    ![](vx_images/415824510221616.png)
    But if enable *Development Build* and *Full With Stacktrace* to debug this bug, this fault will be disappeared. Everything is OK.
    ![](vx_images/368713410231086.png)
    ![](vx_images/341504610224120.png)
    So I do not not the reason of it now.
    * Now, I know this fault reason. Forgot to clear cache files in Browser. 
        How to clear it in this [doc](WebGLUpdatable.md)