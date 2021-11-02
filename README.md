# CosmosFramework

CosmosFramework是一款轻量级的Unity开发框架。模块完善，拥有丰富的Unity方法扩展以及工具链。async/await语法支持，多网络通道支持。框架已做插件化，开发时放入Packages即可。

## Master、V0.1分支暂停维护，稳定版请使用V1.0，最新内容请切换到V1.1!

## 环境

- Unity版本：2017及以上； .NET API版本：4.x。

## 模块简介

- **Audio**： 游戏音效模块。通过注册音效信息，播放时传入音效名即可自动加载音效资源播放。音效支持分组，且可整组播放。

- **Config**： 游戏常用配置模块。用户可在游戏初始化时读取配置文件，并缓存于配置模块。运行时在其他所需位置读取对应配置的数据。

- **Event**： 事件中心模块。使用标准事件模型，提供了监听、移除、派发等常用事件功能。提供事件观测方法，可实时检测事件状态。

- **FSM**： 有限状态机模块。完全抽象的有限状态机，可针对不同类型的拥有者做状态机实现。

- **ObjectsPool**：对象池模块。提供常用的对象生成回收等功能。底层使用数据结构Pool进行实现。

- **Resource**：资源加载模块。框架内部提供了Resources与AB两种加载模式，通过切换加载模式可变更当前默认的加载方式。资源加载模块亦提供了自定义加载通道。
实现自定义加载通道时，须实现继承并实现IResourceLoadHelper接口，并将helper对象传入ResourceLoadChannel中。通过注册ResourceLoadChannel来使用自定义加载方案。
使用自定义加载通道时，第一个参数需要传入通道名(channelName)，剩余参数则与内置加载API相同。

- **Scene**：场景加载模块。提供常用的异步、同步加载嵌入的场景功能。支持自定义实现加载方式。

- **Data**：数据缓存模块。提供树状结构的数据缓存中心。

- **Entity**：游戏实体模块。管理游戏运行时的实体对象。

- **Input**：输入适配模块。用于适配不同平台的输入方式。

- **Hotfix**：热更新模块。此模块适用于基于C#的热更方案。

- **Network**：网络模块。提供了多种高速可靠的UDP协议，如RUDP、SUDP、KCP等，默认使用KCP协议。网络以通道(Channel)形式区分各个连接，支持多种网络类型同时连接。可同时实现作为客户端(Client)以及服务器(Server)。支持async/await语法；

- **UI**：UI模块。基于UGUI实现。提供UI常用功能，如优先级、现实隐藏、获取以及组别设置等。

- **Main**：模块中心。自定义模块与扩展模块都存于此。自定义模块按照内置模块相同格式写入后，可享有完全同等与内置模块的生命周期与权限。几乎与内置模块无异。此主模块的内置轮询池：FixedRefreshHandler、LateRefreshHandler、RefreshHandler、ElapseRefreshHandler可对需要统一进行轮询管理的对象进行统一轮询，减少由于过多的Update等mono回调导致的新能损耗。

- **Controller**：控制器模块。提供常用需要轮询(Update)对象的统一管理。

- **WebRequest**：UnityWebRequest模块，可用于加载持久化资源、网络资源下载等需求。支持获取AssetBundle、AudioClip、Texture2D、string。当资源获取到后，用户可通过WebRequestCallback对资源进行操作；

- **Download**：下载模块。支持localhost本地文件下载与http文件下载。文件下载时以byte流异步增量写入本地。下载中支持动态添加、移除下载任务；

## 内置数据、工具

- **Utility**：提供了反射、算法、断言、转换、Debug富文本、IO、加密、Json、MessagePack、Time、Text、Unity协程、Unity组件等常用工具函数。

- **Singleton**：单例基类。提供了线程安全、非线程安全、MONO单例基类。

- **DataStructure**：常用数据结构。链表、双向链表、二叉树、四叉树、LRU、线程锁等数据结构。

- **Behaviour**：内置生命周期函数，此生命周期可参考Unity的MONO生命周期。需要注意，此内置生命周期适用于原生模块与自定义模块，相对于Unity生命周期是独立的。生命周期优先级依次为：
    - OnInitialization
    - OnActive
    - OnPreparatory
    - OnFixRefresh
    - OnRefresh
    - OnLateRefresh
    - OnDeactive
    - OnTermination
    
- **Extensions**：静态扩展工具。提供unity的扩展以及.NETCore对unity.NET的扩展。

- **Awaitable** ：此工具提供了async/await语法在unity环境中的支持。可以像写c#原生异步一样,在Unity中写异步。支持Task异步，Task执行完成后会回到主线程，使用时按照正常格式写即可；
代码参考：

```CSharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cosmos;
public class AwaitableTest : MonoBehaviour
{
    private async void Start()
    {
        Debug.Log("AwaitableTest >>> Before Coroutine Start");
        await StartCoroutine(EnumRun());
        await new WaitForSeconds(3);
        await EnumRun();
        Debug.LogError("AwaitableTest >>> After Coroutine Start");
        await EnumWait();
    }
    IEnumerator EnumRun()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("AwaitableTest >>> After IEnumerator EnumRun");
    }
    IEnumerator EnumWait()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("AwaitableTest >>> After IEnumerator EnumWait");
    }
}
```

- **EventCore** ：轻量级事件模块，可自定义监听的数据类型；

- **ReferencePool** ：全局引用池模块；

- **Editor** ：Editor中提供了在Hierarchy常用检索对象、组件的方法，EditorConfig提供了代码生成是自动创建代码标头的功能；

- **QuarkAsset** ：QuarkAsset是快速的资源管理方案。动态加载时资源无需放入Resources、StreamingAssets或打包成AB包进行加载，在Assets目录下的任意位置都可以被加载到。加载时可通过文件名+后缀进行完全限定，也可以通过指定路径加载。

- **FutureTask**：异步任务检测，支持多线程与协程异步进度检测。检测函数需要传入Func<bool>格式的函数，当条件返回值为true时，异步检测结束；注意：FutureTask本身并不是协程，不能代替协程执行异步任务；
    
## 内置架构 PureMVC

- 基于原始PureMVC改进的更适于理解的架构。
    框架提供了基于特性更加简洁的注册方式：
    - 1、MVCCommandAttribute，对应Command，即C层；
    - 2、MVCMediatorAttribute，对应Mediator，即V层；
    - 3、MVCProxyAttribute，对应Proxy，即M层；
    
- MVC自动注册只需在入口调用MVC.RegisterAttributedMVC()方法即可。

- 派生的代理类需要覆写构造函数，并传入NAME参数。

- 需要注意，MVC.RegisterAttributedMVC()方法需要传入对应的程序集。支持多程序集反射。

## 注意事项

- 自定义模块实现：
    
```csharp
    using Cosmos;
    public interface IMyManager : IModuleManager
    {
        //自定义一个接口，使自定义的接口继承自IModuleManager
    }
```
    
```csharp
    using Cosmos;
    [Module]
    internal class MyManager :Module, IMyManager
    {
        //创建接口对应的类，继承自Module与IMyManager，并标记上[Module]特性
        //完成以上步骤后，MyManager作为一个模块就被自动生成了。
        //以此种方法定义的模块，被生成后等同于原生模块，享有完全相同的生命周期。
    
        [TickRefresh]
        void TickRefresh()
        {
            //被标记上[TickRefresh]的方法将在Update中执行；
        }
        [LateRefresh]
        void LateRefresh()
        {
            //被标记上[LateRefresh]的方法将在LateUpdate中执行；
        }
        [FixedRefresh]
        void FixedRefresh()
        {
            //被标记上[FixedRefresh]的方法将在FixedUpdate中执行；
        }
    
        //一个模块中只允许拥有一个#Refresh类函数。
    }
```
 - 自定义模块入口实现：
```csharp
    using Cosmos;
    public class MyEntry:Cosmos.CosmosEntry
    {
        //自定义实现一个类作为项目的模块入口，并继承自CosmosEntry。
        //将自定义实现的模块按照以下格式写成静态属性，则整个游戏项目均可通过 MyEntry获取自定义以及原生的所有模块。
        public static IMyManager MyManager { get { return GetModule<IMyManager>(); } }
    }
```
- 项目启动：
    将CosmosConfig挂载于合适的GameObject上，运行Unity。若CosmosConfig上的PrintModulePreparatory处于true状态，则控制台会显示初始化信息。  自此，项目启动完成。
    
- 部分带有Helper的模块可由使用者进行自定义实现，也可使用提供的Default对象；

- 框架提供第三方适配，如Utility.Json，用户可自定义任意JSON方案。框架建议使用的高速传输协议为MessagePack，包含适配方案。
MessagePack 链接地址：https://github.com/neuecc/MessagePack-CSharp

- 最新请使用 V1.1 版本，稳定版请使用V1.0。V0.1 已经停止维护。Master暂停维护。

- 内置案例地址：Assets\Examples\ 。

- 数据结构中，提供了池的的底层对象“Pool”，框架中的对象池与引用池皆为“Pool”作为底层实现；

## 其他

- V1.1_UPM可以通过UnityPackageManager进行加载。加载时请填入：https://github.com/DonnYep/CosmosFramework.git#V1.1_UPM

- 手动加载V1.1_UPM方式：
   - 方式1：选择V1.1（默认分支）,选择Assets/CosmosFramework文件夹，拷贝到工程的Packages目录下。
   - 方式2：选择V1.1_UPM分支，将里面的内容拷贝到工程的Packages目录下。

- CosmosEngine服务器：https://github.com/DonnYep/CosmosEngine

- KCP地址：https://github.com/skywind3000/kcp

- PureMVC地址：https://github.com/DonnYep/PureMVC

- 服务器版本的KCP与客户端版本的KCP皆为参考自Mirror。Mirror地址:https://github.com/vis2k/Mirror

- 部分模块演示请观看视频：https://www.bilibili.com/video/BV1x741157eR
                        https://www.bilibili.com/video/BV17u411Z7Ni/


