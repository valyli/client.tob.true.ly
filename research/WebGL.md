# Outlook
https://zhuanlan.zhihu.com/p/162878354?tdsourcetag=s_pcqq_aiomsg

# Three.js

# Unity3D
Advantage
> * Good on project organization.
> * Abundant tool-chains.

Disadvantage
> * Size of release. If publish to WebGL.

## Embed to H5
Must be exported WebGL
### Achievable
> * Generate native js script.
> * U3D framework (include GameFramework) size is 8MB.

## To do:
> * Success, but has problem of load remote resource data by GF.
> * Efficiency on mobile.

## Embed to Android App
https://blog.csdn.net/qq_41708811/article/details/122297182
### Achievable
> * Embed in app like an Activity.
> * Extract U3D parts from release Android package and inject to target Android package.
> * Could Communicate both sides.

### To do
> * Re-sign published andorid packages.

## Embed to iOS App  
https://blog.csdn.net/weixin_40583225/article/details/123754802
### Achievable
> * Embed in app like a sub view.
> * Build in IL2CPP.
> * Could communicate both sides via invoke function.
### To do
> * Learn & test iOS project in **swift** or **object-c**.


# Tools
## Allow CORS
Should be allow CORS for testing WebGL project if it contain remote resource loading.
### Chrome extension: Allow CORS: Access-Control-Allow-Origin
https://chrome.google.com/webstore/detail/allow-cors-access-control/lhobafahddgcelffkeicbaginigeejlf?hl=en
Reference: http://hellopp.cn/blogs/blog/614fd313bea5f308747b098d

### Script setting in Unity3D (Not validated)
http://t.zoukankan.com/bossing-p-10943045.html

# Others
How to invoke native function in Unity3D? (Android, iOS, js)
...
