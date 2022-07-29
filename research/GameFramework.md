[Simeple Introduce](https://www.233tw.com/unity/49968)  
[Official](https://gameframework.cn/)  
[FAQ](https://gameframework.cn/faq/)  
[API Reference](https://gameframework.cn/api/index.html)  

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
* Member name start with **m_**

# Common


# Game Entry Flow
## BaseComponent
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|static class                 |False                              |
|Namespace                    |UGF                              |
|Hierarchy                    |GameFrameworkComponent : MonoBehaviour|

|Funtions                     |                                 |
|:----------------------------|:---------------------------------|
|Awake()                      |First entry of GameFramework   |

Attached to **Builtin** node.
Component name is :
```csharp
[AddComponentMenu("Game Framework/Base")]
```

## GameEntry (UGF)
|Attributes                   |                                 |
|:----------------------------|:---------------------------------|
|static class                 |True                              |
|Namespace                    |UGF                              |
|Hierarchy                    |GameFrameworkComponent : MonoBehaviour|



# DefaultTextHelper (UGF)
Use StringBuilder & cache it to reduce memory allocations.
