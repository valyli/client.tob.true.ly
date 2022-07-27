[Simple Introduce](https://www.jianshu.com/p/53161ce351e7)

[What is it](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html)
It is better convenient on code organizing than traditional DLLs.

> While there is nothing technically invalid about cyclical references between classes in the same assembly, cyclical references between classes in different assemblies are not allowed. If you encounter a cyclical reference error, you must refactor your code to remove the cyclical reference or to put the mutually referencing classes in the same assembly.

> Conditionally including an assembly

> Defining symbols based on Unity and project package versions

> Getting assembly information in build scripts


[Special folders and script compilation order](https://docs.unity3d.com/Manual/ScriptCompileOrderFolders.html)

[Assembly Definition properties](https://docs.unity3d.com/Manual/class-AssemblyDefinitionImporter.html#general)
> The default namespace for scripts
 in this assembly definition. If you use either Rider or Visual Studio as your code editor, they automatically add this namespace to any new scripts you create in this assembly definition.
