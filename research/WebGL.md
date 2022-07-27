# Outlook
https://zhuanlan.zhihu.com/p/162878354?tdsourcetag=s_pcqq_aiomsg

# WebAssembly
[Conception](https://blog.csdn.net/xuanhun521/article/details/111466058)
[On Egert](https://zhuanlan.zhihu.com/p/30513129)
[Performance Test](https://github.com/liuxinyumocn/WX3DPhysicsTest)

# Three.js
## 简介
Three.js 是一款 webGL 框架，由于其易用性被广泛应用。Three.js 在 WebGL 的 API 接口基础上，又进行的一层封装。

## 优势
1.包体小，加载速度更快。（核心包体积为600k，全部可选组件总体积6M左右。）
2.开发语言为javascript，语言普及率较高，拥有丰富的资料可供参考与使用。可与主流Web前端框架搭配使用。
3.内置了常用的材质与光照效果，方便快速搭建各种场景。

## 缺点
> * Three.js不是游戏引擎，只能提供渲染相关方法，需要配合其他Web前端技术才能构建完整的应用程序，开发人员需要掌握jquery或vue，react等前端框架构建ui，掌握Ammo构建物理效果等。
> * javascript在执行复杂数据计算和大文件解析时效率低下。对于物理引擎，寻路，json解析等任务会出现明显掉帧。(改善方法为使用多语言交叉编译，将复杂计算通过效率更高的语言编译后供javascript调用。)
> * 对于复杂材质和光效需要使用OpenGL Shader Language自定义开发，由于直接操作WebGL，代码编写较繁琐，Three.js没有提供太多的技术支持。

## 相关工具链
> * Three.js editor  http://threejs.org/editor/
> * ThreeNodes https://idflood.github.io/ThreeNodes.js/

## 应用场景
物联网3D可视化，商品在线展示，数据可视化，微信小程序，家装室内设计相关。

## 学习方向
> * 研究在vue中集成Three.js的相关方法和开发流程。
> * 研究主流Web前端技术，包括UI，网络，数据驱动等模块。
> * 研究WebAssembly，Jsbridge等技术，改善程序运行效率。

# Unity3D
Advantage
> * Good on project organization.
> * Abundant tool-chains.

Disadvantage
> * Size of release. If publish to WebGL.

## Embed to H5(html page)
Must be exported WebGL
### Achievable
> * Generate native js script.
> * U3D framework (include GameFramework) size is 8MB.

## To do:
> * Success, but has problem of load remote resource data by GF.
>   Maybe should implement a new resource management to disperse resources for H5.
> * Efficiency on mobile.
> * WebGL project structure will be different with app modes on several aspect such as resource package, hotfix and networking.
>> [GameFramework FAQ](http://gameframework.cn/faq/)
>> WebGL 平台没有经过测试，需要自行尝试并做出一定地修改，交流群里有接入 WebGL 成功的同学。

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
