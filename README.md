# MergeCutePet
本科毕设。这里是客户端采用基于消息机制的unity框架（泰课上可以找到），unity版本是2019.4.40f1c1。基本玩法复刻的是《口袋精灵2》（一个已经倒闭的网页游戏）



启动过程：

主要需要部署的是服务器端，需要运行在photonserver里。

（1）下载 [Photon Engine SDKs | Photon Engine](https://www.photonengine.com/zh-CN/sdks#server-sdkserverserver) ，注意这里下v4.0版本的SDK。下载后，没记错解压到自己指定目录下就行。

（2）配置

[服务器端程序]: https://github.com/hsc19980906/MergePetServer

中的MergePetServer的生成路径，该路径位于刚刚解压PhotonServer路径的“..\deploy\MergePetServer\bin\”。

（3）配置PhotonServer路径的“..\deploy\bin_Win64\”下的PhotonServer.config文件。

```xml
	<!-- Instance settings -->
	<MergePetServer
		MaxMessageSize="512000"
		MaxQueuedDataPerPeer="512000"
		PerPeerMaxReliableDataInTransit="51200"
		PerPeerTransmitRateLimitKBSec="256"
		PerPeerTransmitRatePeriodMilliseconds="200"
		MinimumTimeout="5000"
		MaximumTimeout="30000"
		DisplayName="MergePetServer"
		>

		<!-- 0.0.0.0 opens listeners on all available IPs. Machines with multiple IPs should define the correct one here. -->
		<!-- Port 5055 is Photon's default for UDP connections. -->
		<UDPListeners>
			<UDPListener
				IPAddress="0.0.0.0"
				Port="5055"
				OverrideApplication="MergePetServer">
			</UDPListener>
		</UDPListeners>

		<!-- 0.0.0.0 opens listeners on all available IPs. Machines with multiple IPs should define the correct one here. -->
		<!-- Port 4530 is Photon's default for TCP connecttions. -->
		<!-- A Policy application is defined in case that policy requests are sent to this listener (known bug of some some flash clients) -->
		<TCPListeners>
			<TCPListener
				IPAddress="0.0.0.0"
				Port="4530"
				PolicyFile="Policy\assets\socket-policy.xml"
				InactivityTimeout="10000"
				OverrideApplication="MergePetServer"
				>
			</TCPListener>
		</TCPListeners>

		<!-- Policy request listener for Unity and Flash (port 843) and Silverlight (port 943)  -->
		<PolicyFileListeners>
			<!-- multiple Listeners allowed for different ports -->
			<PolicyFileListener
			  IPAddress="0.0.0.0"
			  Port="843"
			  PolicyFile="Policy\assets\socket-policy.xml"
			  InactivityTimeout="10000">
			</PolicyFileListener>
			<PolicyFileListener
			  IPAddress="0.0.0.0"
			  Port="943"
			  PolicyFile="Policy\assets\socket-policy-silverlight.xml"
			  InactivityTimeout="10000">
			</PolicyFileListener>
		</PolicyFileListeners>

		<!-- WebSocket (and Flash-Fallback) compatible listener -->
		<WebSocketListeners>
			<WebSocketListener
				IPAddress="0.0.0.0"
				Port="9090"
				DisableNagle="true"
				InactivityTimeout="10000"
				OverrideApplication="MergePetServer">
			</WebSocketListener>
		</WebSocketListeners>

		<!-- Defines the Photon Runtime Assembly to use. -->
		<Runtime
			Assembly="PhotonHostRuntime, Culture=neutral"
			Type="PhotonHostRuntime.PhotonDomainManager"
			UnhandledExceptionPolicy="Ignore">
		</Runtime>


		<!-- Defines which applications are loaded on start and which of them is used by default. Make sure the default application is defined. -->
		<!-- Application-folders must be located in the same folder as the bin_win32 folders. The BaseDirectory must include a "bin" folder. -->
		<Applications Default="MergePetServer">

			<!-- MergePetServer Application -->
			<Application
				Name="MergePetServer"
				BaseDirectory="MergePetServer"
				Assembly="MergePetServer"
				Type="MergePetServer.MergePetApplication"
				ForceAutoRestart="true"
				WatchFiles="dll;config"
				ExcludeFiles="log4net.config">
			</Application>

			<!-- CounterPublisher Application -->
			<Application
				Name="CounterPublisher"
				BaseDirectory="CounterPublisher"
				Assembly="CounterPublisher"
				Type="Photon.CounterPublisher.Application"
				ForceAutoRestart="true"
				WatchFiles="dll;config"
				ExcludeFiles="log4net.config">
			</Application>

		</Applications>
	</MergePetServer>
```

如果自己想对这个文件做一些其他配置，可以查查官网资料或是参考一些博客。

（4）启动PhotonServer路径的“..\deploy\bin_Win64\”下PhotonControl.exe，左下角会出现它的小图标，右击该图标，选择MergePetServer->Start as application就可以开启客户端，开始游戏了。

注意：客户端里目前配置的服务器端地址是127.0.0.1也就是本机地址（在NetManager脚本中可以修改它），如果希望远程联机，又不想买云服务器的话，可以试试内网穿透。