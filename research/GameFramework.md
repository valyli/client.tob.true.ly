[Simeple Introduce](https://www.233tw.com/unity/49968)  
[Official](https://gameframework.cn/)  
[FAQ](https://gameframework.cn/faq/)  
[API Reference](https://gameframework.cn/api/index.html)  
[video tutorial](https://www.bilibili.com/video/BV1sE411C7cu?spm_id_from=333.337.search-card.all.click)

# Github
[GameFramework](https://github.com/EllanJiang/GameFramework)  
[UnityGameFramework](https://github.com/EllanJiang/UnityGameFramework.git)  
[StarForce(Demo Project)](https://github.com/EllanJiang/StarForce)  


# Goal
User Guide
Code anlysis(Debug)
Inspector useage
Code standard

# Code standard
* Member's name start with **m_**
* Static memeber's name start with **s_**
* To keep an empty directory on Git, put a file in it for occupation. This file should be named **.gitkeep**.
* Use string.ToLowerInvariant()
* Never remove / delete **.meta** files in project manually.
* Should set texture export format when importing. Set by hand or xxxImporter.
* Use TryGetValue() on Dictionary to test a key, and get a value.

# Compile
## Build GameFramework.dll
> 1. [Download .NET Framework 3.5 SP1](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks?cid=msbuild-developerpacks)
> 2. Install *dotnetfx35.exe*, Maybe should use VPN.
> 3. Build dll, and copy .dll and .xml to \StarForce\Assets\GameFramework\Libraries\
* Do not commit those dll to override existed in git.
* Publish project package must use Release version of dll.

# Common
## FsmState<T>
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |GF                              |
|Hierarchy                    ||

|Funtions                     |                                 |
|:----------------------------|:---------------------------------|
|ChangeState()                |Invoke OnLeave() and OnEnter() internally. |
* Could improve: Add changing conditions to check validity of changing rule.

## CommonFileSystemStream
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |GF                              |
|Hierarchy                    |FileSystemStream, IDisposable|

|Funtions                     |                                 |
|:----------------------------|:---------------------------------|
|Awake()                      |**[step 1]** First entry of GameFramework. Initialize Utilites, helpers and so on.  
Read / Write file by *System.IO*
  ```csharp
  class DefaultFileSystemHelper
  {
    public override FileSystemStream CreateFileSystemStream(string fullPath, FileSystemAccess access, bool createNew)
    {
        if (fullPath.StartsWith(AndroidFileSystemPrefixString, StringComparison.Ordinal))
        {
            return new AndroidFileSystemStream(fullPath, access, createNew);
        }
        else
        {
            return new CommonFileSystemStream(fullPath, access, createNew);
        }
    }
  }
  ```

## FileSystem
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |GF                              |
|Hierarchy                    |IFileSystem|

* It combine some files in one physical file. It descript the organization of those files storage structure.
  ```mermaid
  classDiagram

  class FileInfo {
      <<struct>>
      string m_Name   # File name
      long m_Offset
      int m_Length
  }

  class FileSystem {
      <<class>>
      Dictionary<string, int> m_FileDatas
      List<BlockData> m_BlockDatas
      SortedDictionary<int, StringData> m_StringDatas

      FileInfo  GetFileInfo(string name)
  }

  FileSystem--*FileInfo
  ```
  ```csharp
  StringData stringData = fileSystem.ReadStringData(blockData.StringIndex);
  fileSystem.m_StringDatas.Add(blockData.StringIndex, stringData);
  fileSystem.m_FileDatas.Add(stringData.GetString(fileSystem.m_HeaderData.GetEncryptBytes()), i);
  ```
* Allocates memory from the unmanaged memory of the process.
  ```csharp
  System.Runtime.InteropServices.Marshal
  ```
* *FileSystem* could combine fragments by *TryCombineFreeBlocks()* then delete or write a file.

## AndroidFileSystemStream

## Variable
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |GF                              |
|Hierarchy                    ||

| Funtions   |                                           |
| :--------- | :---------------------------------------- |
| GetValue() | Return the current value.                 |
| SetValue() | set value with any of the scalar types.   |
| Clear()    | Clear the current.                        |
| ToString() | Returns a formatted string of this color. |

| Properties |                              |
| :--------- | :--------------------------- |
| Type       | The type of the scalar data. |
| Value      | The value of the scalar data |

* An Variable object can hold any of the scalar types such as int, float, and char, as well as      pointers, structures, and object id references.
* Use this class to work with such data types in collections (such as List and Dictionary), Key-value coding, and other calls that require IReference.
* Variable objects are always immutable.
```csharp
public sealed class VarInt32 : Variable<int>{...}
procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
```
Files:
```
\StarForce\Assets\GameFramework\Scripts\Runtime\Variable\VarInt32.cs
```

## ReferencePool
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |GF                              |
|Hierarchy                    ||

| Funtions  |                                |
| :-------- | :----------------------------- |
| Acquire() | Get an instance from the pool. |
* ReferencePool is a way to optimize game and lower the burden that is placed on the CPU when having to rapidly create and destroy new objects.

```csharp
LoadConfigFailureEventArgs loadConfigFailureEventArgs = ReferencePool.Acquire<LoadConfigFailureEventArgs>();
```

## ObjectPoolManager
| Funtions                      |                                                               |
| :---------------------------- | :------------------------------------------------------------ |
| CreateSingleSpawnObjectPool() | Get an objectPool that objects can be spawned only one time.  |
| CreateMultiSpawnObjectPool()  | Get an objectPool that objects can be spawned multiple times. |

## ObjectPool
| Funtions   |                                                   |
| :--------- | :------------------------------------------------ |
| Spawn()    | Get an object from the ObjectPool.                |
| Unspawn()  | Release an object.                                |
| CanSpawn() | Check whether the objectPool can spawn an object. |
| Register() | add a object to the the objectPool.               |

* ObjectPool is similar with ReferencePool, the differences is ObjectPool often used to manage unity object.
* Whether an object can be spawned in multiple times depended on the "AllowMultiSpawn" propertie

```csharp
m_HPBarItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<HPBarItemObject>("HPBarItem", 16);
m_HPBarItemObjectPool.Register(HPBarItemObject.Create(hpBarItem), true);
HPBarItemObject hpBarItemObject = m_HPBarItemObjectPool.Spawn();
```

# Game Entry Flow
## BaseComponent
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |UGF                              |
|Hierarchy                    |GameFrameworkComponent : MonoBehaviour|

|Funtions                     |                                 |
|:----------------------------|:---------------------------------|
|Awake()                      |**[step 1]** First entry of GameFramework. Initialize Utilites, helpers and so on.  And RegisterComponent() |

Attached to **Builtin** node.
Component name set with :
```csharp
[AddComponentMenu("Game Framework/Base")]
```

## GameEntry (UGF)
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |Game                              |
|Hierarchy                    |MonoBehaviour|

Use GameEntry as a static class to simplify invoking and reduce cost of looping in GetComponent(). Example:
```csharp
(ProcedureBase)GameEntry.Procedure.CurrentProcedure
```
Files:
```
\StarForce\Assets\GameFramework\Scripts\Runtime\Base\GameEntry.cs
\StarForce\Assets\GameMain\Scripts\Base\GameEntry.cs
\StarForce\Assets\GameMain\Scripts\Base\GameEntry.Builtin.cs
\StarForce\Assets\GameMain\Scripts\Base\GameEntry.Custom.cs
```

## Flow chart
```mermaid
graph TB
    1[BaseComponent.Awake] --> 1.1

    subgraph Initialize all components extend GameFrameworkComponent in Random order
    1.1[ProcedureComponent.Awake] --> 1.2[DebuggerComponent.Awake] --> 1.3[...]
    end

    subgraph Framework Update
    1.3 --> 2.1[GameEntry.Start<br/>GameEntry.InitBuiltinComponents<br/>bundle GameFrameworkComponents to GameEntry static members.]

     --> 3
    3[BaseComponent.Update] --> 4[GameFrameworkEntry.Update in GF] --> 5[Loop all GameFrameworkModule]
    end

    subgraph Resource Flow
    1.3-->6
    6[ProcedureComponent.Start] --delay one frame--> 6.1[m_ProcedureManager.StartProcedure m_EntranceProcedure]
    -->7[ProcedureLaunch]--OnUpdate-->8[ProcedureSplash]
    8--EditorResourceMode-->18[ProcedurePreload]
    8--Package-->10[ProcedureInitResources]
    8--Updatable-->11[ProcedureCheckVersion]-->12{m_NeedUpdateVersion}
    12--true-->13[ProcedureUpdateVersion]-->14
    12--false-->14[ProcedureVerifyResources]
    14-->15[ProcedureCheckResources]-->16{m_NeedUpdateResources = <br/>updateCount > 0}
    16--true-->17[ProcedureUpdateResources]--m_UpdateResourcesComplete=true-->18
    16--false-->18[ProcedurePreload]-->19[ProcedureChangeScene]
    end
```
GameFramework can not promise order of:
> GameFrameworkComponent in GameEntry.s_GameFrameworkComponents
GameFrameworkModule in GameFrameworkEntry.s_GameFrameworkModules

Because of :
> * All **GameFrameworkComponent** will register self in **Awake** with GameEntry.RegisterComponent(this)
> * All **GameFrameworkModule** will be created by GameFrameworkEntry.GetModule() at **first invoked**.
>   ```csharp
>   // Rule of module name.
>   string moduleName = Utility.Text.Format("{0}.{1}", interfaceType.Namespace, interfaceType.Name.Substring(1));
>   Type moduleType = Type.GetType(moduleName);
>   ```
> * The order that Unity calls each GameObject's Awake is not deterministic. [ref](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html)

Attention:
>* Not recommend to set Awake order. (Edit>Project Settings>Script Execution Order)
>* Modify order of **GameFrameworkModule** by **Priority** in it.
![](assets/GameFramework-50d64811.png)

# ...Helper
## DefaultTextHelper (UGF)
Use StringBuilder & cache it to reduce memory allocations.

# Agent & TaskPool
* Which modules use agent now:
![](assets/GameFramework-cc6145c6.png)
* Workflow example in paragraph **WebRequestComponent**
* All *Agent* working **asynchronously**, but **all in one thread now**.
  - If want to use multi-thread, should start thread in *Agent*.
* Task priority is smaller running firstly.

# GameFrameworkComponent (UGF)
class GameFrameworkComponent : MonoBehaviour

## ConfigComponent (UGF)
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |UnityGameFramework.Runtime        |
|Hierarchy                    |GameFrameworkComponent|

Use ConfigComponent as a Component to simplify reading config information. Example:
```csharp
ConfigComponent Config = GameEntry.GetComponent<ConfigComponent>();
Config.ReadData(configAssetName, this);

ConfigComponent Config = GameEntry.GetComponent<ConfigComponent>();
int id = Config.GetInt("Scene.Menu");
```
any function that delivers its result asynchronously, make sure your file have loaded Successfully. Example:
```csharp
GameEntry.GetComponent<EventComponent>().Subscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
GameEntry.GetComponent<EventComponent>().Subscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
private void OnLoadConfigSuccess(object sender, GameEventArgs e){
    //TODO
}
private void OnLoadConfigFailure(object sender, GameEventArgs e){
    //TODO
}
```
the file can be text or byte stream
* How to return line in XML string:
  ```
  &#x000A;
  ```
  ```xml
  <String Key="Menu.StartButton" Value="开始&#x000A; 9" />
  ```

## DataTableComponent (UGF)
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |UnityGameFramework.Runtime        |
|Hierarchy                    |GameFrameworkComponent|

Use DataTableComponent as a Component to reading Excel or other files. Example:
```csharp
DataTableComponent DataTable = GameEntry.GetComponent<DataTableComponent>();
DataTable.LoadDataTable(dataTableName, dataTableAssetName, this);

DataTableComponent DataTable = GameEntry.GetComponent<DataTableComponent>();
IDataTable<DRAircraft> dtAircraft = GameEntry.DataTable.GetDataTable<DRAircraft>();
DRAircraft drAircraft = dtAircraft.GetDataRow(TypeId);
int ThrusterId = drAircraft.ThrusterId;
```
any function that delivers its result asynchronously, make sure your file have loaded Successfully. Example:
```csharp
GameEntry.GetComponent<EventComponent>().Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
GameEntry.GetComponent<EventComponent>().Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
private void OnLoadDataTableSuccess(object sender, GameEventArgs e){
    //TODO
}
private void OnLoadDataTableFailure(object sender, GameEventArgs e){
    //TODO
}
```
the file can be text or byte stream.

### Generate data table
* Menu
```csharp
namespace StarForce.Editor.DataTableTools
{
    public sealed class DataTableGeneratorMenu
    {
        [MenuItem("Star Force/Generate DataTables")]
        private static void GenerateDataTables()
        {
            foreach (string dataTableName in ProcedurePreload.DataTableNames)
            {
              DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
              DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);

            }
```
* Table names
```csharp
namespace StarForce
{
    public class ProcedurePreload : ProcedureBase
    {
        public static readonly string[] DataTableNames = new string[]
        {
            "Aircraft",
            "Armor",
            "Asteroid",
            "Entity",
            "Music",
            "Scene",
            "Sound",
            "Thruster",
            "UIForm",
            "UISound",
            "Weapon",
        };
```
* Path
```csharp
namespace StarForce.Editor.DataTableTools
{
    public sealed class DataTableGenerator
    {
        private const string DataTablePath = "Assets/GameMain/DataTables";
        private const string CSharpCodePath = "Assets/GameMain/Scripts/DataTable";
        private const string CSharpCodeTemplateFileName = "Assets/GameMain/Configs/DataTableCodeTemplate.txt";
```
### Data table
* Path
```shell
d:\git\StarForce\Assets\GameMain\DataTables\
```
* Numerical value not support NULL. It will cause Exception in *ParseDataRow*

### DataTable Processer
#### DataProcessor (For one data type) (How to instantiate)
```shell
Assets/GameMain/Scripts/Editor/DataTableGenerator/DataTableProcessor.DataProcessorUtility.cs
```
```csharp
        private static class DataProcessorUtility
        {
            private static readonly IDictionary<string, DataProcessor> s_DataProcessors = new SortedDictionary<string, DataProcessor>(StringComparer.Ordinal);

            static DataProcessorUtility()
            {
                System.Type dataProcessorBaseType = typeof(DataProcessor);                << DataProcessor
                Assembly assembly = Assembly.GetExecutingAssembly();
                System.Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    if (!types[i].IsClass || types[i].IsAbstract)
                    {
                        continue;
                    }

                    if (dataProcessorBaseType.IsAssignableFrom(types[i]))
                    {
                        DataProcessor dataProcessor = (DataProcessor)Activator.CreateInstance(types[i]);
                        foreach (string typeString in dataProcessor.GetTypeStrings())
                        {
                            s_DataProcessors.Add(typeString.ToLowerInvariant(), dataProcessor);                << Auto create instance by lowercase name
                        }
                    }
                }
            }
```

#### DataTableProcessor (For one table)

#### DataTableGenerator
```csharp
                if (dataTableProcessor.IsSystem(i))
                {
                    string languageKeyword = dataTableProcessor.GetLanguageKeyword(i);        <<< C# language type
                    if (languageKeyword == "string")
                    {
                        stringBuilder.AppendFormat("            {0} = columnStrings[index++];", dataTableProcessor.GetName(i)).AppendLine();
                    }
                    else
                    {
                        stringBuilder.AppendFormat("            {0} = {1}.Parse(columnStrings[index++]);", dataTableProcessor.GetName(i), languageKeyword).AppendLine();
                    }
                }
```

#### Other infos
```shell
DataTableProcessor.IdProcessor.cs    // It is called by DataProcessorUtility.GetDataProcessor("id")
```

## LocalizationComponent (UGF)
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |UnityGameFramework.Runtime        |
|Hierarchy                    |GameFrameworkComponent|

Use LocalizationComponent as a Component to simplify reading localization information. Example:
```csharp
LocalizationComponent Localization = GameEntry.GetComponent<LocalizationComponent>();
Localization.ReadData(dictionaryAssetName, this);

LocalizationComponent Localization = GameEntry.GetComponent<LocalizationComponent>();
string Title = GameEntry.Localization.GetString("AskQuitGame.Title");
```
any function that delivers its result asynchronously, make sure your file have loaded Successfully. Example:
```csharp
GameEntry.GetComponent<EventComponent>().Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
GameEntry.GetComponent<EventComponent>().Subscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);
private void OnLoadDictionarySuccess(object sender, GameEventArgs e){
    //TODO
}
private void OnLoadDictionaryFailure(object sender, GameEventArgs e){
    //TODO
}
```

## SettingComponent (UGF)
| Attributes |                            |
| :--------- | :------------------------- |
| Namespace  | UnityGameFramework.Runtime |
| Hierarchy  | GameFrameworkComponent     |

| Funtions        |                                                                             |
| :-------------- | :-------------------------------------------------------------------------- |
| Save()          | Writes all modified preferences to disk.                                    |
| HasSetting()    | Returns true if the given key exists, otherwise returns false.              |
| RemoveSetting() | Removes the given key.                                                      |
| SetInt()        | Sets a single integer value for the preference identified by the given key. |
| GetInt()        | Returns the value corresponding to key in the preference file if it exists. |
* Stores player preferences between game sessions. It can store string, float and integer values into the user’s platform registry just like PlayerPrefs.
```csharp
GameEntry.Setting.SetString(Constant.Setting.Language, m_SelectedLanguage.ToString());
GameEntry.Setting.Save();
```

## EventComponent (UGF)
| Attributes |                            |
| :--------- | :------------------------- |
| Namespace  | UnityGameFramework.Runtime |
| Hierarchy  | GameFrameworkComponent     |

| Funtions      |                         |
| :------------ | :---------------------- |
| Subscribe()   | add callback to event.  |
| Unsubscribe() | cancel the callback.    |
| Fire()        | send event.             |
| FireNow()     | send event immediately. |

Files:
```
\StarForce\Assets\GameFramework\Scripts\Runtime\Event\EventComponent.cs
```
* Fire() send event at the second frame ,handle in the main thread.
* FireNow() send event immediately, it is not a thread-safe way.

## EntityComponent (UGF)
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |UnityGameFramework.Runtime        |
|Hierarchy                    |GameFrameworkComponent|

| Funtions                |                                     |
| :---------------------- | :---------------------------------- |
| ShowEntity()            | add Entity to manager and Scene.    |
| HideEntity()            | hide Entity from manager and Scene. |
| HideAllLoadedEntities() | hide all loaded Entitis.            |
| GetEntityGroup()        | find EntityGroup from mnager.       |
| GetEntity()             | find Entiy from manager.            |

Files:
```
\StarForce\Assets\GameFramework\Scripts\Runtime\Entity\EntityComponent.cs
```

* Hide entity means putting a entity into the recycle queue.
* Entitis in the recycle queue will be recycled by entityGroup's objectPool  on Update() in EntityManager and entitiyInfos will be released.

ShowEntity function that delivers its result asynchronously, make sure entity have loaded Successfully. Example:
```csharp
GameEntry.GetComponent<EventComponent>().Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
GameEntry.GetComponent<EventComponent>().Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
{
    ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
    if (ne.EntityLogicType == typeof(MyAircraft))
        {
            m_MyAircraft = (MyAircraft)ne.Entity.Logic;
        }
    }

protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
{
    ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
    Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
}
```

### EntityLogic

Use EntityLogic to add entity game logic.

| Attributes |                            |
| :--------- | :------------------------- |
| Namespace  | UnityGameFramework.Runtime |
| Hierarchy  | MonoBehaviour              |

| Funtions       |                                              |
| :------------- | :------------------------------------------- |
| OnInit()       | be called when it is being loaded.           |
| OnRecycle()    | be called when it is in Recycle Queue.       |
| OnShow()       | be called when it is added to Scene.         |
| OnHide()       | be called when it is removed from Scene.     |
| OnAttached()   | be called when a entity is attached to it.   |
| OnDetached()   | be called when a entity is detached from it. |
| OnAttachTo()   | be called when it is attached to a entity.   |
| OnDetachFrom() | be called when it is detached from a entity. |
| OnUpdate()     | be called every frame, if it is enabled.     |

* Use Visible to Activate or deactivate the entity.
* Use CachedTransform to modify Position, rotation and scale of an entity.
* Change transform in onUpdate() may interrupt CachedTransform initialize code in onShow(), delay execution by one frame will be fine.

Files:
```
\StarForce\Assets\GameFramework\Scripts\Runtime\Entity\EntityLogic.cs
```

### Entity asset load flow
```csharp
class ResourceManager.ResourceLoader.LoadResourceAgent
{
  private void OnLoadResourceAgentHelperLoadComplete(object sender, LoadResourceAgentHelperLoadCompleteEventArgs e)
  {
    assetObject = AssetObject.Create(m_Task.AssetName, e.Asset, dependencyAssets, m_Task.ResourceObject.Target, m_ResourceHelper, m_ResourceLoader);
    m_ResourceLoader.m_AssetPool.Register(assetObject, true);
    m_ResourceLoader.m_AssetToResourceMap.Add(e.Asset, m_Task.ResourceObject.Target);
    OnAssetObjectReady(assetObject)
    {
      class EntityManager
      {
        private void LoadAssetSuccessCallback(string entityAssetName, object entityAsset, float duration, object userData)
        {
          EntityInstanceObject entityInstanceObject = EntityInstanceObject.Create(entityAssetName, entityAsset, m_EntityHelper.InstantiateEntity(entityAsset), m_EntityHelper)
          {
            class DefaultEntityHelper
            {
              public override object InstantiateEntity(object entityAsset)
              {
                  return Instantiate((Object)entityAsset);
              }
            }
          }
          showEntityInfo.EntityGroup.RegisterEntityInstanceObject(entityInstanceObject, true);

          InternalShowEntity(showEntityInfo.EntityId, entityAssetName, showEntityInfo.EntityGroup, entityInstanceObject.Target, true, duration, showEntityInfo.UserData)
          {
            class DefaultEntityHelper
            {
              public override IEntity CreateEntity(object entityInstance, IEntityGroup entityGroup, object userData)
              {
                Transform transform = gameObject.transform;
                transform.SetParent(((MonoBehaviour)entityGroup.Helper).transform);
                return gameObject.GetOrAddComponent<Entity>();
              }
            }
          }
        }
      }
    }
  }
}

```


## UIComponent (UGF)
| Attributes |                            |
| :--------- | :------------------------- |
| Namespace  | UnityGameFramework.Runtime |
| Hierarchy  | GameFrameworkComponent     |

| Funtions                |                                         |
| :---------------------- | :-------------------------------------- |
| OpenUIForm()            | add form to manager and Scene.          |
| CloseUIForm()           | close form from manager and Scene.      |
| CloseAllLoadedUIForms() | hide all panels from manager and Scene. |
| GetUIGroup()            | find uiGroup from manager.              |
| GetUIForm()             | find form from manager.                 |

Files:
```
\StarForce\Assets\GameFramework\Scripts\Runtime\UI\UIComponent.cs
```

* cllose form means putting a panel into the recycle queue.
* panels in the recycle queue will be recycled by objectPool  on Update() in UIManager.

OpenUIForm function that delivers its result asynchronously, make sure entity have loaded Successfully. Example:
```csharp
GameEntry.GetComponent<EventComponent>().Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
GameEntry.GetComponent<EventComponent>().Subscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
protected virtual void OnOpenUIFormSuccess(object sender, GameEventArgs e)
{
    OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
    if (ne.UserData != this)
    {
        return;
    }
    m_MenuForm = (MenuForm)ne.UIForm.Logic;
}

protected virtual void OnOpenUIFormFailure(object sender, GameEventArgs e)
{
    OpenUIFormFailureEventArgs ne = (OpenUIFormFailureEventArgs)e;
    Log.Warning("open uiform failure with error message '{0}'.", ne.ErrorMessage);
}
```

### UIFormLogic

Use UIFormLogic to add ui logic.

| Attributes |                            |
| :--------- | :------------------------- |
| Namespace  | UnityGameFramework.Runtime |
| Hierarchy  | MonoBehaviour              |

| Funtions         |                                                       |
| :--------------- | :---------------------------------------------------- |
| OnInit()         | be called when panel is being loaded.                 |
| OnRecycle()      | be called when panel is in Recycle Queue.             |
| OnOpen()         | be called when panel is added to Scene.               |
| OnClose()        | be called when panel is removed from Scene.           |
| OnPause()        | be called when interaction with the panel is paused.  |
| OnResume()       | be called when interaction with the panel is resumed. |
| OnCover()        | be called when panel is covered by other panel.       |
| OnReveal()       | be called when panel is Revealed.                     |
| OnUpdate()       | be called every frame, if it is enabled.              |
| OnDepthChanged() | be called when the depth of panel is Changed.         |

* Use Visible to Activate or deactivate the panel.
* Use CachedTransform to modify Position, rotation and scale of an panel.
* Production of UI panel presets and additions to scripts which need to be inherited from UIFormLogic to achieve the corresponding functionality in scripts

Files:
```
\StarForce\Assets\GameFramework\Scripts\Runtime\UI\UIFormLogic.cs
```

ShowEntity function that delivers its result asynchronously, make sure entity have loaded Successfully. Example:
```csharp
GameEntry.GetComponent<EventComponent>().Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
GameEntry.GetComponent<EventComponent>().Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
{
    ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
    if (ne.EntityLogicType == typeof(MyAircraft))
        {
            m_MyAircraft = (MyAircraft)ne.Entity.Logic;
        }
    }

protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
{
    ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
    Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
}
```

## SoundComponent (UGF)
| Attributes |                            |
| :--------- | :------------------------- |
| Namespace  | UnityGameFramework.Runtime |
| Hierarchy  | GameFrameworkComponent     |

| Funtions      |                                                  |
| :------------ | :----------------------------------------------- |
| PlaySound()   | Play the a clip.                                 |
| StopSound()   | Stop playing the clip.                           |
| PauseSound()  | Pause playing the clip.                          |
| ResumeSound() | Unpause the paused playback of this AudioSource. |

Files:
```
\StarForce\Assets\GameFramework\Scripts\Runtime\Sound\SoundComponent.cs
```

### SoundGroup

| Attributes |                            |
| :--------- | :------------------------- |
| Namespace  | UnityGameFramework.Runtime |
| Hierarchy  |                            |

| Properties       |                                                          |
| :--------------- | :------------------------------------------------------- |
| Mute             | mute Un- / Mutes the audioSource in the group.           |
| Volume           | The volume of the audioSource in the group (0.0 to 1.0). |
| AgentHelperCount | The count of the agent in then group.                    |
* AgentHelperCount means there are up to the count of sounds will play at the same time.
* Agent can set a priority, The Agent with lowest priority will reset first.
* Agent can play sound at a given worldPosition in world space.
* Agent can update position by binding a entity loaded in scene and will reset when the binding entity has released.

Files:
```
\StarForce\Assets\GameFramework\Scripts\Runtime\UI\UIFormLogic.cs
```

## ProcedureComponent
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |UGF                              |
|Hierarchy                    |GameFrameworkComponent|

|Funtions                     |                                 |
|:----------------------------|:---------------------------------|
|Start()                |Start procedure delay one frame. |

> **ProcedureComponentInspector: GameFrameworkInspector**
> |Funtions                     |                                 |
> |:----------------------------|:---------------------------------|
> |RefreshTypeNames()          |Auto refresh procedures type after C# code compiled. |

Files:
```
\StarForce\Assets\GameFramework\Scripts\Runtime\Procedure\ProcedureComponent.cs
\StarForce\Assets\GameFramework\Scripts\Editor\Inspector\ProcedureComponentInspector.cs
```

* Config procedures by name:
![](assets/GameFramework-c040bc19.png)

* Just allow one ProcedureManager. (Because class ProcedureManager : GameFrameworkModule)
* Start entrance procedure at the second frame to promise all Awake() & Start() are executed.
```csharp
m_ProcedureManager.Initialize(GameFrameworkEntry.GetModule<IFsmManager>(), procedures);

yield return new WaitForEndOfFrame();

m_ProcedureManager.StartProcedure(m_EntranceProcedure.GetType());
```

## WebRequestComponent
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |UGF                              |
|Hierarchy                    |GameFrameworkComponent|

|Funtions                     |                                 |
|:----------------------------|:---------------------------------|
|Awake()                |m_WebRequestManager = GameFrameworkEntry.GetModule<IWebRequestManager>() |
|Start()                |AddWebRequestAgentHelper() |

**Use this component work flow to show how does TaskPool running with agent and task.**
### Init Step
1. Call *WebRequestComponent.AddWebRequestAgentHelper()* (UGF)
2. Create several *UnityWebRequestAgentHelper* (UGF)
  - *Helper.CreateHelper* create by type name.
    ```csharp
    WebRequestAgentHelperBase webRequestAgentHelper = Helper.CreateHelper(m_WebRequestAgentHelperTypeName, m_CustomWebRequestAgentHelper, index);
    ```
  - *UnityWebRequestAgentHelper* base on *IWebRequestAgentHelper* (GF)
3. Add *UnityWebRequestAgentHelper* into *WebRequestManager* module:
  - new *WebRequestAgent* (GF) to include instance of *IWebRequestAgentHelper*.
  - Register *WebRequestManager* callback funtions to agent event handler.
  - Add *WebRequestAgent* to *m_TaskPool.m_FreeAgents* (*Stack<ITaskAgent<T>>*) by *AddAgent()*

### Invoke Step
1. Use case:
    ```csharp
    GameEntry.WebRequest.AddWebRequest(Utility.Text.Format(GameEntry.BuiltinData.BuildInfo.CheckVersionUrl, GetPlatformPath()), this);
    ```
2. Call *WebRequestComponent.AddWebRequest()* (UGF) until call *WebRequestManager.AddWebRequest()* (GF)
  - Create *WebRequestTask* base on *TaskBase*
  - Add *WebRequestTask* to *m_TaskPool.m_WaitingTasks* (*GameFrameworkLinkedList<T>*) by *AddTask()*
3. Wait to *WebRequestManager.Update()* is called.
  - Put a waiting task into running task if has free agent. (*TaskPool.ProcessWaitingTasks()*)
  - Call this agent to start task
    ```csharp
    StartTaskStatus status = agent.Start(task);
    ```
  - Call *UnityWebRequestAgentHelper.Request()*
    ```csharp
    m_Helper.Request(m_Task.WebRequestUri, m_Task.UserData);
    ```
  - Invoke Untiy3D function and wait return.
    ```csharp
    m_UnityWebRequest = UnityWebRequest.Get(webRequestUri);
    ```
  - Check return in *UnityWebRequestAgentHelper.Update()*
    - If *m_UnityWebRequest.isDone* is true, will fire an event finally. StackTrace:
      ![](assets/GameFramework-5282f445.png)

## DownloadComponent
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |UGF                              |
|Hierarchy                    |GameFrameworkComponent|

* Pair with *DownloadManager* (GF)
* About FlushSize and Timeout
  ```csharp
  private void OnDownloadAgentHelperUpdateBytes(object sender, DownloadAgentHelperUpdateBytesEventArgs e)
  {
      m_WaitTime = 0f;
      try
      {
          m_FileStream.Write(e.GetBytes(), e.Offset, e.Length);
          m_WaitFlushSize += e.Length;
          m_SavedLength += e.Length;

          if (m_WaitFlushSize >= m_Task.FlushSize)
          {
              m_FileStream.Flush();
              m_WaitFlushSize = 0;
          }
      }
  }
  ```
* Executed API
  UnityWebRequestDownloadAgentHelper
  ```csharp
  m_UnityWebRequest = new UnityWebRequest(downloadUri);
  m_UnityWebRequest.SetRequestHeader("Range", Utility.Text.Format("bytes={0}-{1}", fromPosition, toPosition));
  m_UnityWebRequest.downloadHandler = new DownloadHandler(this);
  #if UNITY_2017_2_OR_NEWER
  m_UnityWebRequest.SendWebRequest();
  ```
  *UnityWebRequestDownloadAgentHelper.DownloadHandler* inherits from *UnityEngine.Networking.DownloadHandlerScript*

## SceneComponent
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |UGF                              |
|Hierarchy                    |GameFrameworkComponent|

|Funtions                     |                                 |
|:----------------------------|:---------------------------------|
|Awake()                |m_GameFrameworkScene = SceneManager.GetSceneAt(GameEntry.GameFrameworkSceneId); |
|SetActiveScene()                |SceneManager.SetActiveScene(activeScene);</br>RefreshMainCamera() |
|RefreshSceneOrder()        | Get max scene, and SetActiveScene(scene);|
|SetSceneOrder()        | Default order is zero</br> always active max order one</br> |


## ResourceComponent
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |UGF                              |
|Hierarchy                    |GameFrameworkComponent|

* Pair with *ResourceManager* (GF)
### Features
* LoadAsset
  ```csharp
  class ResourceManager.ResourceLoader
  {
    public void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData)
    {
      LoadAssetTask mainTask = LoadAssetTask.Create(assetName, assetType, priority, resourceInfo, dependencyAssetNames, loadAssetCallbacks, userData);
      m_TaskPool.AddTask(mainTask);
      if (!resourceInfo.Ready)
      {
          m_ResourceManager.UpdateResource(resourceInfo.ResourceName);
      }

    }
  }
  ```
  TaskPool.Update() to call Agent.Start() at first.
  ```csharp
  class ResourceManager.ResourceLoader.LoadResourceAgent
  {
    public StartTaskStatus Start(LoadResourceTaskBase task)
    {
      m_Helper.ReadFile(fileSystem, resourceInfo.ResourceName.FullName);
    }
  }
  ```
  Call agent helper in UGF
  ```csharp
  class DefaultLoadResourceAgentHelper
  {
    public override void ReadFile(IFileSystem fileSystem, string name)
    {
      m_FileAssetBundleCreateRequest = AssetBundle.LoadFromFileAsync(fileSystem.FullPath, 0u, (ulong)fileInfo.Offset);
    }
  }
  ```

* Update / Download
  ```csharp
  class GameFramework.Resource.ResourceUpdater
  {
    private bool DownloadResource(UpdateInfo updateInfo)
    {
      m_DownloadManager.AddDownload(updateInfo.ResourcePath, Utility.Path.GetRemotePath(Path.Combine(m_ResourceManager.m_UpdatePrefixUri, resourceFullNameWithCrc32)), updateInfo)
      {
        DownloadTask downloadTask = DownloadTask.Create(downloadPath, downloadUri, tag, priority, m_FlushSize, m_Timeout, userData);
        m_TaskPool.AddTask(downloadTask);
      }
    }
  }
  ```
  It has serveral entries in UGF or GF:
  ```csharp
  GameEntry.Resource.UpdateResources();
  m_ResourceManager.UpdateResources(updateResourcesCompleteCallback);
  ```

* Verify
  ```csharp
  class GameFramework.Resource.ResourceVerifier
  {
    private bool VerifyResource(VerifyInfo verifyInfo)
    {
      fileSystem.ReadFile(fileName, m_ResourceManager.m_CachedStream);
      // Convert to CRC32 and compare immediately.
    }
  }
  ```
  Entry points:
  ```csharp
  m_ResourceManager.VerifyResources(verifyResourceLengthPerFrame, verifyResourcesCompleteCallback);
  GameFramework.Resource.ResourceVerifier.Update()
  {
    if (m_FailureFlag)
    {
        // Will fix version list file
        GenerateReadWriteVersionList();
    }
  }
  ```


# Resource Tools
[vedio](https://www.bilibili.com/video/BV1sE411C7cu?p=4&vd_source=e68deb734011863d4a3f9f42402d920c)
[ref 1](https://blog.csdn.net/qq_26999509/article/details/102758769)  (Some name has changed in this article, such as AssetBundleEditor.xml、AssetBundleCollection.xml、AssetBundleBuilder.xml, change from AssetBundleXXX to ResourcXXX)
[ref 2](https://gameframework.cn/uncategorized/%e4%bd%bf%e7%94%a8-assetbundle-%e6%9e%84%e5%bb%ba%e5%b7%a5%e5%85%b7/)
* AssetDatabase is verison 2 that use LMDB.

## Mode of Package  
1. Package
Build out all data files to: *Output Package Path*.
Copy *Output Package Path* to *StreamingAssets*.
This mode generate full files for Standalone Game.
2. Full Path
Build out all data and version list files to: *Output Full Path*.
Upload *Output Full Path* to http server.
This mode generate full files for Updatable Game.
3. Packed
Build out partial data to *Output Packed Path* which was set *Packed* flag in *Resource Editor*.
Copy *Output Packed Path* to *StreamingAssets*.
This mode generate files which should be published with Updatable Game.
* We always choose mode *Full Path* and *Packed* together for our game need resource update feature.


## Config Files
Could modify config file path at here.
```
\StarForce\Assets\GameMain\Scripts\Editor\GameFrameworkConfigs.cs
```
Default config path is :
```
/Assets/GameFramework/Configs/
```
## ResourceEditor
```csharp
class ResourceEditorController
class ResourceEditor : EditorWindow
```

### Step
1. Config mapping in Editor: Game Framework->Resource Tools->Resource Editor.
2. Set asset filter. ***ScanSourceAssets()*** will execute this filer.
  a. Based on guids of AssetDatabase(U3D).
  b. Skip folder(empty).
3. Organize relationship with Resource and Assets in editor.
4. Click Save button, save files:
  a. ResourceEditor.xml
  b. ResourceCollection.xml

### ResourceCollection.xml
```xml
<?xml version="1.0" encoding="UTF-8"?>
<UnityGameFramework>
  <ResourceCollection>
    <Resources>
      <Resource Name="Configs" FileSystem="GameData" LoadType="0" Packed="True" ResourceGroups="Base" />
    </Resources>
    <Assets>
      <Asset Guid="44c8db52241385c45bbb14a1718f17bf" ResourceName="Configs" />
      <Asset Guid="7fd11dc5d29076d469d414dec2818f11" ResourceName="Configs" />
      </Assets>
    </ResourceCollection>
  </UnityGameFramework>
```
```csharp
class ResourceCollection
  private readonly SortedDictionary<string, Resource> m_Resources;
  private readonly SortedDictionary<string, Asset> m_Assets;
```
* Resources: Descript relation of *Resource Name* and *FileSystem*. Those elements will be modified manually.
* Assets: Descript relation of U3D assets and *Resource Name*. Those elements were generated by *ResourceEditor* automatically.

## Resource Builder
```csharp
class ResourceBuilder
class ResourceBuilderController
```
### Step
1. Set output path. Must set it outside the U3D project.
2. Click build button. Wait processing.
3. Check output files:
  a. Build report:
  ![](assets/GameFramework-cf4601ab.png)
  b. Output assetbudles of Untiy3D. Those file will be wrote in FileSystem(.dat).
    ![](assets/GameFramework-4ddfe89e.png)
  c. Output packages:
  ![](assets/GameFramework-0b5f80a0.png)
  d. Output & Resource Editor relation:
  ![](assets/GameFramework-79619e8c.png)
## Core Code of Resource Builder
```csharp
class ResourceBuilderController:
  public bool BuildResources()
```
### Step
1. Remove files in StreamingAssets. Will keep **.gitkeep**.
2. Remove empty directories at all.
3. Analyze asset dependency.
  a. Ignore dependency of Scene(.untiy).
  b. Ignore all Scripts.
  c. Ignore analyzied one.
4. Generate *m_DependencyResources* and *m_DependencyAssets*.
5. PrepareBuildData()
  - a. Get Resource and Assets data from: *m_Resources & m_Assets*.
  - b. Loop each asset:
    - 1. Read asset file to bytes.
    - 2. Calculate bytes CRC32.
    - 3. Get dependency data from *m_ResourceAnalyzerController*
    - 4. Add asset guid, len, hash code(CRC32), dependency asset names to *m_ResourceDatas*
  - c. Loop with *m_ResourceDatas*:
    - 1. Not allow Resource data without any asset.
    - 2. If IsLoadFromBinary, add to *binaryResourceDatas*.
    - 3. else generate AssetBundleBuild(U3D), add to *assetBundleBuildDatas* and *assetBundleResourceDatas*.
6. BuildResources() on target platforms.
  - a. Create and Clean output directories.
  - b. Clean files not relate with *assetBundleResourceDatas*. (Keep all .manifest).
  - c. Delete all .manifest without pair of data files.
  - d. At here will keep all data and manifest files at lastest building.
  - e. Call *BuildPipeline.BuildAssetBundles()*, build data and manifest to working path.
    > ```csharp
    > AssetBundleManifest assetBundleManifest = BuildPipeline.BuildAssetBundles(workingPath, assetBundleBuildDatas, buildAssetBundleOptions, GetBuildTarget(platform));
    > ```
  - f. Create *FileSystem*(GF) for saving data in this file structure in furture, and gourp by *m_ResourceDatas*.
    - i. For *m_OutputPackageFileSystems*
    - ii. For *m_OutputPackedFileSystems*  
  - g. Call *ProcessAssetBundle()* for each *ResourceData*:
    - 1. Read bytes from assetbundle file built with U3D.
    - 2. Encrypt bytes by XOR.
      > There has a question:
      > *Utility.Encryption.GetQuickXorBytes()* and *Utility.Encryption.GetXorBytes()* are not differency. Why?
    - 3. Call *ProcessOutput()* to write bytes in to *m_OutputPackageFileSystems*.( -> .dat)
      - i. For *OutputPackageSelected*
      - ii. For *OutputPackedSelected && resourceData.Packed*
      - iii. For *OutputFullSelected*. ( -> {CRC32 x8}.dat)
        > Just this mode support compress.
        > ? Should find which kind of compress.
    - 4. Generate and Add *ResourceCode* to *ResourceData* in *m_ResourceDatas*.
  - h. Call *ProcessBinary()* :
    > Like *ProcessAssetBundle()*.
    > Differency is read bytes from asset file at first step.
  - i. Call *ProcessPackageVersionList()*:
    - 1. Generate *PackageVersionList.Asset* with asset name and dependency asset index.
    - 2. Generate *PackageVersionList.Resource* from *m_ResourceDatas*.
    - 3. Generate *PackageVersionList.FileSystem* from *GetFileSystemNames(resourceDatas)*.
    - 4. Generate *PackageVersionList.ResourceGroup* from *GetResourceGroupNames(resourceDatas)*.
    - 5. Build version with callback: V0~2.
    - 6. Write to *Output Package Path* / *GameFrameworkVersion.dat*
  - j. Call *ProcessUpdatableVersionList()*:
    > Like *ProcessPackageVersionList()*.
    > Write to *Output Full Path* / *GameFrameworkVersion. {CRC32 x8}.dat*
  - k. Call *ProcessReadOnlyVersionList()*:
    - 1. Get all packaged Resource Data.
    - 2. Generate *LocalVersionList.Resource* and *LocalVersionList.FileSystem*.
    - 3. Write to *Output Packed Path* / *GameFrameworkList.dat*
  - l. Call *OnPostprocessPlatform()*. If platform is *Windows* and *outputPackageSelected* is true, copy data files to *StreamingAssets* automatically.
  - m. Call *StarForceBuildEventHandler.OnPostprocessAllPlatforms()*. We could add some operation at here.

### Principle Files  
#### XML  
* ResourceEditor.xml  
*Resource Editor* settings. Include filter of assets.
* ResourceBuilder.xml
*Resource Builder* settings.
* ResourceCollection.xml
Output of *Resource Editor* operation.

#### Output of Builder
* GameFrameworkVersion.dat / GameFrameworkVersion. {CRC32 x8}.dat
Information of resource at all. Include: Asset, Resource, FileSystem, ResourceGroup.
* GameFrameworkList.dat
Partial information of resource. Just Include: Resource, FileSystem.



### Tips  
* What is assetBundleResourceData.Variant?
It is like sub name of Resource to distinguish resource.
![](assets/GameFramework-3001ef8c.png)
```csharp
validNames.Add(GetResourceFullName(assetBundleResourceData.Name, assetBundleResourceData.Variant).ToLowerInvariant());
```

* Just only allow one Asset in one Resource.
```csharp
ResourceCollection.SetResourceLoadType()
if ((loadType == LoadType.LoadFromBinary || loadType == LoadType.LoadFromBinaryAndQuickDecrypt || loadType == LoadType.LoadFromBinaryAndDecrypt) && resource.GetAssets().Length > 1)
{
    return false;
}
```

### To do
* What's the effect of m_ScatteredAssets?
I find author wrote it is not finished yet. It is designed for testing.


## Resource Flow (ProcedureCheckVersion->ProcedureVerifyResources->ProcedureCheckResources->ProcedurePreload)
### Open This Mode
1. Disable Resource Mode
![](vx_images/421341010239275.png)
2. Change Resource Mode to Updatable
![](vx_images/232320810220849.png)

### Step
1. Http Get version file from: https://starforce.gameframework.cn/Resources/{Platform}Version.txt
Example: https://starforce.gameframework.cn/Resources/WindowsVersion.txt  
Return a json file:
```json
{
  "ForceUpdateGame": false,
  "LatestGameVersion": "0.1.0",
  "InternalGameVersion": 1,
  "InternalResourceVersion": 1,
  "UpdatePrefixUri": "https://starforce.gameframework.cn/Resources/0_1_0_1/Windows",
  "VersionListLength": 7158,
  "VersionListHashCode": 1985842577,
  "VersionListCompressedLength": 2643,
  "VersionListCompressedHashCode": 288676703,
  "END_OF_JSON": ""
}
```
Set *m_CheckVersionComplete* = true

2. Compare version in *VersionListProcessor.CheckVersionList()*.
Local version file path: It is read/write path.
Example on windows10:
```shell
C:/Users/Administrator/AppData/LocalLow/Game Framework/Star Force/GameFrameworkVersion.dat
# If you want to test Update processing again and again, just remove those files in this directory [Persistence Path]
```
Load local *internalResourceVersion* from *GameFrameworkVersion.dat*.
Compare *internalResourceVersion*(local) with *latestInternalResourceVersion*(remote)
Set *m_NeedUpdateVersion*

3. Judge m_NeedUpdateVersion in Update().
If *m_NeedUpdateVersion* is false, Jump to 4??.
If *m_NeedUpdateVersion* is false, Jump to 4.

4. Change state to *ProcedureVerifyResources*.
5. *ResourceManager.ResourceVerifier.VerifyResources()*:
  - a. Call *m_ResourceManager.m_ResourceHelper.LoadBytes()* asynchronously.
    ```
    C:/Users/Administrator/AppData/LocalLow/Game Framework/Star Force/GameFrameworkList.dat
    ```
  - b. Callback *OnLoadReadWriteVersionListSuccess()*
6. *OnLoadReadWriteVersionListSuccess()*
  - a. Deserialize *GameFrameworkList.dat* to *LocalVersionList*
  - b. Analyze *LocalVersionList.Resource[]* and *LocalVersionList.FileSystem[]*
  - c. Combine *m_VerifyInfos*
  - d. *m_LoadReadWriteVersionListComplete* = true
7. Wait entry *ResourceManager.ResourceVerifier.Update()* in GameFramework.
  - a. Loop *m_VerifyInfos*, check each by *VerifyResource()*
    - Read from *FileSystem*
    - Compare length between file and *verifyInfo.Length*
    - Compare hashcode between file and verifyInfo.HashCode. Read from stream and step size is 4K bytes.
  - b. One loop just verify one *VersionInfo* only. And wait next Update() to check the next one.
  - c. After each loop, will refresh *EventManager* to trigger such as *ProcedureVerifyResources.OnResourceVerifySuccess()* refreshing UI.
  - d. If loop all finished, then:
    - d.1 If verify failed. Will call *GenerateReadWriteVersionList()* to fix 'GameFrameworkList.dat'
    - d.2 else verify successed. Set *ProcedureVerifyResources.m_VerifyResourcesComplete* = true
  - e. Wait Update() change state to *ProcedureCheckResources*
8. *ResourceManager.CheckResources()*
  - a. Load files asynchronously:

    | Directory                   | FileName          | LoadBytesCallbacks                      |
    |:----------------------------|:------------------|:---------------|
    |Persistence Path            |GameFrameworkVersion.dat  | OnLoadUpdatableVersionListSuccess |
    |StreamingAssets            |GameFrameworkList.dat| OnLoadReadOnlyVersionListSuccess or OnLoadReadOnlyVersionListFailure |
    |Persistence Path            |GameFrameworkList.dat| OnLoadReadWriteVersionListSuccess |
    Above functions executed in random order.
  - b. *OnLoadUpdatableVersionListSuccess()*
      - b.1. Deserialize to *UpdatableVersionList*
      - b.2. Combine *m_CheckInfos*, *m_ResourceGroups*, *m_ResourceManager.m_AssetInfos*  with *UpdatableVersionList* data.
  - c. *OnLoadReadWriteVersionListSuccess()*
  - d. *OnLoadReadOnlyVersionListFailure()*
  - e. execute *RefreshCheckInfoStatus()*.  Loop for *m_CheckInfos*
      - e.1. *RefreshStatus()*, Check resource has exist in file system in persistence path or StreamingAssets, to decide how to deal with it (Diverge to *CheckStatus* [Disuse, StorageInReadOnly, StorageInReadWrite, Update, Unavailable]).
       - e.2. Tidy (move or remove) file in *FileSystem* with results generated above.
       - e.3. Add result to *m_ResourceManager.m_ResourceInfos* and *m_ResourceManager.m_ReadWriteResourceInfos*
   - f. Remove empty FileSystem and Directory.
   - g. Set *m_CheckResourcesComplete* = true, set *m_NeedUpdateResources*
   - h. Wait Update() change state to:
        - h.1 If m_NeedUpdateResources is true, *ProcedureCheckResources*
        - h.2 else, *ProcedurePreload*
9. In *ProcedurePreload*:
  - a. LoadConfig, LoadDataTable, LoadDictionary, LoadFont and so on.
  Example:
    ```csharp
    string configAssetName = AssetUtility.GetConfigAsset(configName, false);
    m_LoadedFlag.Add(configAssetName, false);
    GameEntry.Config.ReadData(configAssetName, this);
    ```
    ```csharp
    string configAssetName = AssetUtility.GetConfigAsset(configName, false);
    m_LoadedFlag.Add(configAssetName, false);
    GameEntry.Config.ReadData(configAssetName, this);
    ```
  - b. add to *m_LoadedFlag*, and set *false*.
  - c. Check all is *true* in *m_LoadedFlag* in *Update()*
  - d. Change state to *ProcedureChangeScene*. That's all !

## Resource Update Flow
### Step
1. GameEntry.Resource.CheckVersionList(m_VersionInfo.InternalResourceVersion)
  - If versionListFile not exist, *CheckVersionListResult.NeedUpdate*
    ```shell
    C:/Users/Administrator/AppData/LocalLow/Game Framework/Star Force/GameFrameworkVersion.dat
    ```
  - ...other way
2. set *m_NeedUpdateVersion* = true
3. *OnUpdate()*
    ```csharp
    ChangeState<ProcedureUpdateVersion>(procedureOwner);
    ```
4. *GameEntry.Resource.UpdateVersionList()* -> *ResourceManager.VersionListProcessor.UpdateVersionList()*
  Download remote version list file to local file.
    ```csharp
    string latestVersionListFullNameWithCrc32 = Utility.Text.Format("{0}.{2:x8}.{1}", RemoteVersionListFileName.Substring(0, dotPosition), RemoteVersionListFileName.Substring(dotPosition + 1), m_VersionListHashCode);
    m_DownloadManager.AddDownload(localVersionListFilePath, Utility.Path.GetRemotePath(Path.Combine(m_ResourceManager.m_UpdatePrefixUri, latestVersionListFullNameWithCrc32)), this);
    ```
    ```shell
    C:/Users/Administrator/AppData/LocalLow/Game Framework/Star Force/GameFrameworkVersion.dat
    https://starforce.gameframework.cn/Resources/0_1_0_1/Windows/GameFrameworkVersion.765d8d91.dat
    ```
5. Add download task
    ```csharp
    DownloadTask downloadTask = DownloadTask.Create(downloadPath, downloadUri, tag, priority, m_FlushSize, m_Timeout, userData);
    m_TaskPool.AddTask(downloadTask);
    ```
6. *DownloadManager.m_TaskPool.Update()*
  *UnityWebRequestDownloadAgentHelper.Download()*
    ```csharp
    public override void Download(string downloadUri, object userData)
    {
        m_UnityWebRequest = new UnityWebRequest(downloadUri);
        m_UnityWebRequest.downloadHandler = new DownloadHandler(this);
        m_UnityWebRequest.SendWebRequest();
    }
    ```
    *UnityWebRequestDownloadAgentHelper.Update()*
    If *m_UnityWebRequest.isDone*, go *DownloadManager.DownloadAgent.OnDownloadAgentHelperComplete()*
      - 6.1 Remove old file.
      - 6.2 Move new download file ({0}.download) to replace old file.
7. *ResourceManager.VersionListProcessor.OnDownloadSuccess()*

8. *ResourceManager.ResourceChecker.OnLoadUpdatableVersionListSuccess()*
    ```csharp
    ResourceManager.ResourceChecker.RefreshCheckInfoStatus()
    {
      if (ci.Status == CheckInfo.CheckStatus.Update)
      {
        ResourceNeedUpdate(ci.ResourceName, ci.FileSystemName, ci.LoadType, ci.Length, ci.HashCode, ci.CompressedLength, ci.CompressedHashCode)
        {
          ResourceManager.ResourceUpdater.AddResourceUpdate()
          {
            m_UpdateCandidateInfo.Add()
          }
        }
      }
    }
    ```
9. *ResourceManager.ResourceUpdater.OnDownloadSuccess()* for each.
  Remove old file.
  Remove from *m_UpdateCandidateInfo*
  m_ResourceManager.m_ResourceInfos[updateInfo.ResourceName].MarkReady();
10. Finished all of one Resource, *GenerateReadWriteVersionList()*
11. Finished all
    ```csharp
    if (m_UpdateCandidateInfo.Count <= 0 && ResourceUpdateAllComplete != null)
    {
        ResourceUpdateAllComplete();
    }
    ```
    ```csharp
    ProcedureUpdateResources.OnUpdateResourcesComplete()
    {
      m_UpdateResourcesComplete = true
      {
        ChangeState<ProcedurePreload>(procedureOwner);
      }
    }
    ```


## Load Resource
* All resources (asset and binary) informations are managed by *ResourceManager*.
```csharp
internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
{
  private Dictionary<string, AssetInfo> m_AssetInfos;
  private Dictionary<ResourceName, ResourceInfo> m_ResourceInfos;
}
{
  private ResourceInfo GetResourceInfo(string assetName)
  {
      if (string.IsNullOrEmpty(assetName))
      {
          return null;
      }

      AssetInfo assetInfo = m_ResourceManager.GetAssetInfo(assetName);
      if (assetInfo == null)
      {
          return null;
      }

      return m_ResourceManager.GetResourceInfo(assetInfo.ResourceName);
  }
}
```

* All assets and resources pool are managed by *ResourceManager.ResourceLoader*.
```csharp
internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
{
  private sealed partial class ResourceLoader
  {
    private IObjectPool<AssetObject> m_AssetPool;
    private IObjectPool<ResourceObject> m_ResourcePool;
  }
}
```

### Step  
1. *ResourceManager.LoadAsset()*
```csharp
m_ResourceManager.LoadAsset(dataAssetName, priority, m_LoadAssetCallbacks, userData);
```
2. *ResourceLoader.LoadAsset()*
```csharp
m_ResourceLoader.LoadAsset(assetName, null, priority, loadAssetCallbacks, userData);
```
3. *ResourceLoader.CheckAsset()*
4. Create *LoadAssetTask* and search / create *LoadDependencyAssetTask*, add to *m_TaskPool*
```csharp
LoadAssetTask mainTask = LoadAssetTask.Create(assetName, assetType, priority, resourceInfo, dependencyAssetNames, loadAssetCallbacks, userData);
m_TaskPool.AddTask(mainTask);
```
5. If not *resourceInfo.Ready*, call m_ResourceManager.UpdateResource(). Control by *ResourceUpdater.m_UpdateCandidateInfo*
```csharp
if (!resourceInfo.Ready)
{
    m_ResourceManager.UpdateResource(resourceInfo.ResourceName);
}
```
6. Wait execute *TaskPool.Update()*:
  - 6.1 *ProcessWaitingTasks()*
    Get one free *LoadResourceAgent*
    Add to *m_WorkingAgents*
    Call *LoadResourceAgent.Start()*
    If this asset has exist in *m_ResourceLoader.m_AssetPool*, call *7.4 LoadResourceAgent.OnAssetObjectReady()*
    Call *m_Helper.ReadFile()*, load **AssetBundle** from:
    ```csharp
    m_FileAssetBundleCreateRequest = AssetBundle.LoadFromFileAsync(fullPath);
    ```
  - 6.2 *ProcessRunningTasks()*, no more important for this processing.
7. *DefaultLoadResourceAgentHelper.Update()*
```csharp
private void Update()
{
#if UNITY_5_4_OR_NEWER
    UpdateUnityWebRequest();
#else
    UpdateWWW();
#endif
    UpdateFileAssetBundleCreateRequest();
    UpdateBytesAssetBundleCreateRequest();
    UpdateAssetBundleRequest();
    UpdateAsyncOperation();
}
```
  - 7.1 *UpdateFileAssetBundleCreateRequest()*
      (*AssetBundleCreateRequest*) m_FileAssetBundleCreateRequest.**isDone**
      ```csharp
      private void OnLoadResourceAgentHelperReadFileComplete(object sender, LoadResourceAgentHelperReadFileCompleteEventArgs e)
      {
          ResourceObject resourceObject = ResourceObject.Create(m_Task.ResourceInfo.ResourceName.Name, e.Resource, m_ResourceHelper, m_ResourceLoader);
          m_ResourceLoader.m_ResourcePool.Register(resourceObject, true);
          s_LoadingResourceNames.Remove(m_Task.ResourceInfo.ResourceName.Name);
          OnResourceObjectReady(resourceObject);
      }
      ```
  - 7.2 *LoadResourceAgent.OnResourceObjectReady()*
      ```csharp
      private void OnResourceObjectReady(ResourceObject resourceObject)
      {
          m_Task.LoadMain(this, resourceObject);
      }
      public void LoadMain(LoadResourceAgent agent, ResourceObject resourceObject)
      {
          m_ResourceObject = resourceObject;
          agent.Helper.LoadAsset(resourceObject.Target, AssetName, AssetType, IsScene);
      }

      class DefaultLoadResourceAgentHelper
      {
        public override void LoadAsset(object resource, string assetName, Type assetType, bool isScene)
        {
          AssetBundle assetBundle = resource as AssetBundle;
          m_AssetBundleRequest = assetBundle.LoadAssetAsync(assetName);
        }
      }
      ```
  - 7.3 *UpdateAssetBundleRequest()*
    (*AssetBundleCreateRequest*) m_AssetBundleRequest.**isDone**
    ```csharp
    private void OnLoadResourceAgentHelperLoadComplete(object sender, LoadResourceAgentHelperLoadCompleteEventArgs e)
    {
      OnAssetObjectReady()
    }
    ```
  - 7.4 *OnAssetObjectReady()*
    Callback which set at **Step 1**
    ```csharp
    public override void OnLoadAssetSuccess(LoadResourceAgent agent, object asset, float duration)
    {
        base.OnLoadAssetSuccess(agent, asset, duration);
        if (m_LoadAssetCallbacks.LoadAssetSuccessCallback != null)
        {
            m_LoadAssetCallbacks.LoadAssetSuccessCallback(AssetName, asset, duration, UserData);
        }
    }
    ```
8. Load Scene Step
  It like paragraph 7. But judged if scene, will use special API for it in Unity3D.
  ```csharp
  m_AsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
  ```
  ```csharp
  UpdateAsyncOperation()

  if (m_Task.IsScene)
  {
    m_ResourceLoader.m_SceneToAssetMap.Add(m_Task.AssetName, asset);
  }

  class ResourceManager.ResourceLoader
  {
    private void OnLoadResourceAgentHelperLoadComplete(object sender, LoadResourceAgentHelperLoadCompleteEventArgs e)
    {
      if (m_Task.IsScene)
      {
          assetObject = m_ResourceLoader.m_AssetPool.Spawn(m_Task.AssetName);
      }
      OnAssetObjectReady(assetObject);
    }
  }

  private void OnAssetObjectReady(AssetObject assetObject)
  {
      m_Helper.Reset();

      object asset = assetObject.Target;
      if (m_Task.IsScene)
      {
          m_ResourceLoader.m_SceneToAssetMap.Add(m_Task.AssetName, asset);
      }

      m_Task.OnLoadAssetSuccess(this, asset, (float)(DateTime.UtcNow - m_Task.StartTime).TotalSeconds);
      m_Task.Done = true;
  }
  ```

## Load Scene
### Step
```csharp
GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), Constant.AssetPriority.SceneAsset, this)
{
  m_SceneManager.LoadScene(sceneAssetName, priority, userData)
  {
    m_ResourceManager.LoadScene(sceneAssetName, priority, m_LoadSceneCallbacks, userData);
    {
      m_ResourceLoader.LoadScene(sceneAssetName, priority, loadSceneCallbacks, userData);
      {
        LoadSceneTask mainTask = LoadSceneTask.Create(sceneAssetName, priority, resourceInfo, dependencyAssetNames, loadSceneCallbacks, userData);
        m_TaskPool.AddTask(mainTask);
        # Change to paragraph 6.
      }
    }
  }
}
```
```csharp
class DefaultLoadResourceAgentHelper
{
  public override void LoadAsset(object resource, string assetName, Type assetType, bool isScene)
  {
    m_AsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
  }
}
```

## Unload Scene  
### Step
```csharp
GameEntry.Scene.UnloadScene()
{
  SceneManager.UnloadScene()
  {
    ResourceManager.UnloadScene()
    {
      ResourceLoader.UnloadScene()
      {
        m_SceneToAssetMap.Remove(sceneAssetName);
        m_AssetPool.Unspawn(asset);
        m_AssetPool.ReleaseObject(asset);
        ResourceManager.m_ResourceHelper.UnloadScene(sceneAssetName, unloadSceneCallbacks, userData)
        {
          AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(SceneComponent.GetSceneName(sceneAssetName));
        }
      }
    }
  }
}
```

## Unload Resource
### Step
1. Unspawn from ObjectPool
```csharp
ResourceComponent.UnloadAsset()
{
  ResourceManager.UnloadAsset()
  {
    ResourceLoader.UnloadAsset()
    {
      m_AssetPool.Unspawn(asset)
      {
        internalObject.Unspawn();
        if (Count > m_Capacity && internalObject.SpawnCount <= 0)
        {
            Release();
        }
      }
    }
  }
}
```
2. Release from ObjectPool in Update()
```csharp
ObjectPoolManager.Update()
{
  ObjectPoolManager.ObjectPool.Release()
  {
    # Find each could released
    ReleaseObject()
    {
      internalObject.Release(false)
      {
        m_EntityHelper.ReleaseEntity(m_EntityAsset, Target)
        {
          m_ResourceComponent.UnloadAsset(entityAsset);
          Destroy((Object)entityInstance);
        }
      }
      ReferencePool.Release(internalObject);
    }
  }
}
```

## Resource Update Operation Flow
#### Install Http Server
1. Java
[jdk-1.8](http://10.60.80.2/data/d/soft/java/)

2. Tomcat
[download](http://10.60.80.2/data/d/soft/apache-tomcat-7.0.72.exe)
    - Modify config:
      - Add below line before *</Host>* in *conf/server.xml*
        ```xml
        <Context path="" docBase="D:/output/" debug="0" reloadable="true" crossContext="true" />
        ```
      - Open directory listing in *conf/web.xml*
        ```XML
        <param-name>listings</param-name>
        <param-value>true</param-value>
        ```
    - Restart Tomcat service.

### Prepare Environment for Project  
1. Set *Output Directory* path to *D:/output/StarForceAssetBundle*:
![](assets/GameFramework-92a0480c.png)

2. Build project and check results.
![](assets/GameFramework-3ea8de7b.png)

3. Build *WindowsVersion.txt* file in:
    ```shell
    D:\output\StarForceAssetBundle\Full\WindowsVersion.txt
    ```
    Content is:
    ```json
    {
      "ForceUpdateGame": false,
      "LatestGameVersion": "0.1.0",
      "InternalGameVersion": 1,
      "InternalResourceVersion": 1,
      "UpdatePrefixUri": "http://127.0.0.1:8080/StarForceAssetBundle/Full/0_1_0_1/Windows",
      "VersionListLength": 7161,
      "VersionListHashCode": -851609584,
      "VersionListCompressedLength": 2671,
      "VersionListCompressedHashCode": 1901795202,
      "END_OF_JSON": ""
    }
    ```
    - **Attention**: Address url of *UpdatePrefixUri* is used for updatable files(.dat)
    - Setting value is copied from *BuildLog.txt* manually.
      ![](assets/GameFramework-35e23efb.png)
      ![](assets/GameFramework-d33b1074.png)

4. Edit *\StarForce\Assets\GameMain\Configs\BuildInfo.txt*
  - Change url to your local host.
  - Change *https* to *http* for local test.
    ```json
    {
      "GameVersion": "0.1.0",
      "InternalGameVersion": 0,
      "CheckVersionUrl": "http://127.0.0.1:8080/StarForceAssetBundle/Full/{0}Version.txt",
      "WindowsAppUrl": "http://127.0.0.1:8080",
      "MacOSAppUrl": "http://127.0.0.1:8080",
      "IOSAppUrl": "http://127.0.0.1:8080",
      "AndroidAppUrl": "http://127.0.0.1:8080",
      "END_OF_JSON": ""
    }
    ```

5. Check and Clear files(.dat) in *persistentPath*
    ```
    C:\Users\Administrator\AppData\LocalLow\Game Framework\Star Force
    ```

6. Run project in *updatable* Resource Mode. If could see below picture, you win.
![](assets/GameFramework-5e685b00.png)

7. Double check in Tomcat log file:
![](assets/GameFramework-a0869894.png)
![](assets/GameFramework-2428db8f.png)

### Update Resource Version
1. Build & deploy resource files(.dat) into http server content directory.
2. Modify *WindowsVersion.txt*:
    ```
    "InternalResourceVersion": 4,
    "UpdatePrefixUri": "http://127.0.0.1:8080/StarForceAssetBundle/Full/0_1_0_4/Windows",
    ```
3. Run project & check result in Game View & http logs.


## EditorResourceComponent (UGF)
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|Namespace                    |UnityGameFramework.Runtime        |
|Hierarchy                    | MonoBehaviour, IResourceManager |
Use for *Editor Resource Mode*.
It implement all of methods of *ResourceComponent* to simulate working flow of package mode.


# Tutorial
## Add Asset to Resource

## Config local http server for Resource update

## Add new Entity

## Add new UGuiForm

# Build New Project
1. Create new project in Untiy3D Hub
2. Copy *GameFramework* to new project.
BuildSetting add Scenes

3. Set Layer "Targetable Object" (SetLayerRecursively)
```
m_Scenes: []

```
```
m_Scenes:
- enabled: 1
  path: Assets/StarForce Launcher.unity
- enabled: 1
  path: Assets/GameMain/Scenes/Main.unity
- enabled: 1
  path: Assets/GameMain/Scenes/Menu.unity

```

# Q&A
## About UGUI
[Unity3D Doc in 2018.3](https://docs.unity3d.com/2018.3/Documentation/ScriptReference/EventSystems.IPointerDownHandler.html)
This document can not find in 2021.3. Instead of it, as a [plug-in](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/index.html)
