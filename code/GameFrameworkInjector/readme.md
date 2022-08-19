1. Execute injector
```shell
injector.bat <Target Project Path>
```
2. Set ENABLE_LOG in PlayerSetting:
![](vx_images/241901017220858.png)
3. Add Scenes:
![](vx_images/545483117247317.png)
![](vx_images/452851117239284.png)
4. Set Version in '0.1.0'
![](vx_images/130162017227151.png)
Resolve Error (Optional)
![](vx_images/384303309239987.png)
Add Layer *Targetable Object*
![](vx_images/549563409236542.png)

# Build in Command Line 
Cmd :
```shell
"C:\Program Files\Unity\Hub\Editor\2021.3.7f1c1\Editor\Unity.exe" -quit -batchmode -logFile mybuild.log -projectPath d:\git\client.tob.true.ly\code\GameFrameworkInjector\Cap1\ -executeMethod UnityGameFramework.Editor.ResourceTools.ResourceBuilder.CmdBuild
```
Output:
```shell
file:///D:/output/StarForceAssetBundle/Full/0_1_0_22/AndroidVersion.txt
```