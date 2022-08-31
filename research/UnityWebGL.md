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
* Result:
    This solution is OK.
    1. IndexedDB snapshot:
        ![](vx_images/279801617220872.png)


# Other Problems:
1. Should disable ***Strip Engine Code*** in Untiy setting.
    ```
    This could be caused by a class being stripped from the build even though it is needed. Try disabling 'Strip Engine Code' in Player Settings.
    ```
2. ExecutionEngineException: Attempting to call method 'Test::OnMessage<Test+AnyEnum>' for which no ahead of time (AOT) code was generated.  Consider increasing the --generic-virtual-method-iterations=1 argument
[feature-preview-il2cpp-full-generic-sharing-in-unity-20221-beta](https://blog.unity.com/technology/feature-preview-il2cpp-full-generic-sharing-in-unity-20221-beta)
