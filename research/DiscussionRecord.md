# Talk with ToC [Client]
|Date                   |  2022/7/29                       |
|:----------------------------|:---------------------------------|
|Attendees                    |张迪,李佳,邢冲,王磊                              |
|Topic                    |Possibility of cooperation between ToC and ToB on client development|

> ## Meeting Summary
> 1. 建议不要使用 ToC 的前端框架，重新搭建 ToB 的框架，避免历史问题影响。
> 2. ToC 的制作标准比较特殊，ToB 未必能够采用。（见附录）
> 3. 热更方案都准备选用 huatuo （Hybird CLR）。存在政策风险，如发生，退化方案：先取消热更新。后可再部分改用ILRuntime。
> 4. 推荐可以参考ET，前端结构比较简单。而 GameFramework 比较复杂，需要能力较强的人来处理框架问题。
> 5. 推荐了一个候选人(Fun plus)

> ## Appendix
> * ToC 制作规格：
>>  Mesh surface > 10K
>>  Skeleton > 100  
>>  Lights = 4 Point + 1 Directory  
>>  Texture(Avator) = 3
>>  Viewing angle 45°

> ## To do & Result:
> 1. 向HR跟进候选人信息。  
>    结果：反馈有一些不合适的因素，无法入职。
> 2. ET 调研。  
>    结果：
>    a. ET的侧重点在前后端共用 C# 开发，且更侧重后端结构
>    b. ET对前端支持的功能比GameFramework少
>    c. ET自带的代码热更方案是ILRuntime，相对性能还是弱一些。也可以考虑改用 Hybird CLR
>    d. 另外发现，在应用 ECS 时，如果使用 Hybird CLR 等热更技术应用后，相应部分的代码将失去 ECS 作用。


# Talk with ToC [Server]
|Date                   |  2022/7/29                       |
|:----------------------------|:---------------------------------|
|Attendees                    |张迪,李佳,邢冲                           |
|Topic                    |How to design a scalable server cluster|

> ## Meeting Summary
> 1.
