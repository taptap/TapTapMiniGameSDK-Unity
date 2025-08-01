# 服务器端网络模块API接口文档

## 概述

这是Unity WebSocket服务器端网络模块的完整API文档。该模块提供了封装好的WebSocket服务器功能，专门用于Unity运行时作为游戏服务器。

## 适用场景

- **运行环境**: Unity运行时（Unity Editor 或 构建的服务器版本）
- **主要功能**: 作为游戏服务器，接受客户端连接
- **通信格式**: 统一使用JSON数据格式
- **支持平台**: Unity Editor、PC服务器构建版本

---

## NetworkServerModule

### 基本信息

**命名空间**: `TapServer`  
**继承**: `MonoBehaviour`  
**主要功能**: 提供WebSocket服务器功能，支持多客户端连接

### 安装和设置

#### 1. 导入必要文件

确保您的项目包含以下文件：
- `TapServer/Scripts/NetworkServerModule.cs`
- `TapServer/Scripts/UnityWebSocketServer.cs` 
- `TapServer/Scripts/MainThreadDispatcher.cs`
- 相关的依赖组件

#### 2. 依赖项

- **LitJson**: 用于JSON序列化/反序列化
- **UnityWebSocket**: WebSocket底层实现

### 初始化

```csharp
// 方式1: 代码初始化
NetworkServerModule server = gameObject.AddComponent<NetworkServerModule>();
server.Initialize(8081, 10);

// 方式2: Inspector配置 + 代码初始化
public NetworkServerModule networkServer; // 在Inspector中拖拽赋值
networkServer.Initialize(serverPort, maxConnections);
```

### 核心API

#### 服务器控制

```csharp
/// <summary>
/// 初始化服务器
/// </summary>
/// <param name="serverPort">服务器端口 (默认8081)</param>
/// <param name="maxClientConnections">最大客户端连接数 (默认10)</param>
public void Initialize(int serverPort = 8081, int maxClientConnections = 10)

/// <summary>
/// 启动服务器
/// </summary>
public void StartServer()

/// <summary>
/// 停止服务器
/// </summary>
public void StopServer()
```

#### 消息发送

```csharp
/// <summary>
/// 向指定客户端发送JSON数据
/// </summary>
/// <param name="clientId">客户端ID</param>
/// <param name="data">要发送的数据对象</param>
public void SendToClient(string clientId, object data)

/// <summary>
/// 向所有客户端发送JSON数据（重载版本）
/// </summary>
/// <param name="data">要发送的数据对象</param>
public void SendToClient(object data)

/// <summary>
/// 广播JSON数据到所有客户端
/// </summary>
/// <param name="data">要广播的数据对象</param>
public void BroadcastToAll(object data)
```

#### 状态查询

```csharp
/// <summary>
/// 服务器是否正在运行
/// </summary>
public bool IsRunning { get; }

/// <summary>
/// 已连接的客户端数量
/// </summary>
public int ConnectedClientCount { get; }

/// <summary>
/// 服务器地址
/// </summary>
public string ServerAddress { get; }

/// <summary>
/// 获取已连接的客户端列表
/// </summary>
/// <returns>客户端信息列表</returns>
public List<ClientInfo> GetConnectedClients()

/// <summary>
/// 获取客户端数量
/// </summary>
/// <returns>客户端数量</returns>
public int GetClientCount()
```

### 事件回调

```csharp
/// <summary>
/// 服务器启动事件
/// </summary>
public event Action<string> OnServerStarted; // serverAddress

/// <summary>
/// 服务器停止事件
/// </summary>
public event Action OnServerStopped;

/// <summary>
/// 客户端连接事件
/// </summary>
public event Action<string, string> OnClientConnected; // clientId, clientIP

/// <summary>
/// 客户端断开事件
/// </summary>
public event Action<string> OnClientDisconnected; // clientId

/// <summary>
/// 消息接收事件
/// </summary>
public event Action<string, JsonData> OnMessageReceived; // clientId, jsonData

/// <summary>
/// 错误事件
/// </summary>
public event Action<string, string> OnError; // clientId, error
```

### 客户端信息类

```csharp
[System.Serializable]
public class ClientInfo
{
    public string clientId;
    public string clientIP;
    public DateTime connectTime;
}
```

## 完整使用示例

### 基础服务器实现

```csharp
using UnityEngine;
using TapServer;
using LitJson;

public class GameServer : MonoBehaviour
{
    [Header("Server Settings")]
    public int serverPort = 8081;
    public int maxConnections = 10;
    
    private NetworkServerModule networkServer;
    
    void Start()
    {
        // 初始化服务器
        networkServer = gameObject.AddComponent<NetworkServerModule>();
        networkServer.Initialize(serverPort, maxConnections);
        
        // 绑定事件
        networkServer.OnServerStarted += OnServerStarted;
        networkServer.OnServerStopped += OnServerStopped;
        networkServer.OnClientConnected += OnClientConnected;
        networkServer.OnClientDisconnected += OnClientDisconnected;
        networkServer.OnMessageReceived += OnMessageReceived;
        networkServer.OnError += OnServerError;
        
        // 启动服务器
        networkServer.StartServer();
    }
    
    private void OnServerStarted(string serverAddress)
    {
        Debug.Log($"服务器启动成功: {serverAddress}");
    }
    
    private void OnServerStopped()
    {
        Debug.Log("服务器已停止");
    }
    
    private void OnClientConnected(string clientId, string clientIP)
    {
        Debug.Log($"客户端连接: {clientId} from {clientIP}");
        
        // 发送欢迎消息给指定客户端
        var welcomeData = new 
        { 
            type = "welcome", 
            data = new 
            { 
                message = "欢迎加入游戏!", 
                serverTime = System.DateTime.Now.ToString(),
                onlineCount = networkServer.ConnectedClientCount
            }
        };
        networkServer.SendToClient(clientId, welcomeData);
        
        // 通知所有客户端有新玩家加入
        var broadcastData = new 
        { 
            type = "player_joined", 
            data = new { message = $"玩家 {clientId} 加入了游戏" }
        };
        networkServer.SendToClient(broadcastData); // 使用重载方法发送给所有客户端
    }
    
    private void OnClientDisconnected(string clientId)
    {
        Debug.Log($"客户端断开: {clientId}");
        
        // 通知其他客户端有玩家离开
        var leaveData = new 
        { 
            type = "player_left", 
            data = new { message = $"玩家 {clientId} 离开了游戏" }
        };
        networkServer.SendToClient(leaveData);
    }
    
    private void OnMessageReceived(string clientId, JsonData jsonData)
    {
        try
        {
            if (jsonData.ContainsKey("type"))
            {
                string messageType = jsonData["type"].ToString();
                
                switch (messageType)
                {
                    case "login":
                        HandleLoginMessage(clientId, jsonData);
                        break;
                    case "game_action":
                        HandleGameAction(clientId, jsonData);
                        break;
                    case "chat":
                        HandleChatMessage(clientId, jsonData);
                        break;
                    case "ping":
                        HandlePingMessage(clientId, jsonData);
                        break;
                    default:
                        Debug.Log($"收到未知类型消息从 {clientId}: {messageType}");
                        break;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"处理客户端 {clientId} 消息时出错: {e.Message}");
            SendErrorMessage(clientId, "消息格式错误");
        }
    }
    
    private void OnServerError(string clientId, string error)
    {
        Debug.LogError($"服务器错误 (客户端: {clientId}): {error}");
    }
    
    // 消息处理方法
    private void HandleLoginMessage(string clientId, JsonData data)
    {
        // 处理登录逻辑
        Debug.Log($"处理登录消息从客户端: {clientId}");
    }
    
    private void HandleGameAction(string clientId, JsonData data)
    {
        // 处理游戏动作
        Debug.Log($"处理游戏动作从客户端: {clientId}");
    }
    
    private void HandleChatMessage(string clientId, JsonData data)
    {
        // 处理聊天消息
        if (data.ContainsKey("data") && data["data"].ContainsKey("content"))
        {
            string content = data["data"]["content"].ToString();
            
            // 广播聊天消息给所有客户端
            var chatBroadcast = new 
            { 
                type = "chat", 
                data = new 
                { 
                    sender = clientId, 
                    content = content, 
                    timestamp = System.DateTime.Now.ToString("HH:mm:ss")
                }
            };
            networkServer.SendToClient(chatBroadcast);
        }
    }
    
    private void HandlePingMessage(string clientId, JsonData data)
    {
        // 响应心跳
        var pongData = new 
        { 
            type = "pong", 
            timestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };
        networkServer.SendToClient(clientId, pongData);
    }
    
    private void SendErrorMessage(string clientId, string errorMessage)
    {
        var errorData = new 
        { 
            type = "error", 
            data = new 
            { 
                message = errorMessage, 
                timestamp = System.DateTime.Now.ToString()
            }
        };
        networkServer.SendToClient(clientId, errorData);
    }
    
    private void OnDestroy()
    {
        // 清理资源
        if (networkServer != null && networkServer.IsRunning)
        {
            networkServer.StopServer();
        }
    }
}
```

## 服务器端消息格式

### 接收的消息格式（来自客户端）

#### 1. 登录消息
```json
{
  "type": "login",
  "data": {
    "playerId": "unique_player_id",
    "playerName": "Player Name",
    "version": "1.0.0"
  },
  "timestamp": 1234567890
}
```

#### 2. 游戏动作
```json
{
  "type": "game_action",
  "data": {
    "action": "move",
    "position": { "x": 10.5, "y": 20.3 },
    "speed": 3.5
  },
  "timestamp": 1234567890
}
```

#### 3. 聊天消息
```json
{
  "type": "chat",
  "data": {
    "content": "Hello everyone!",
    "channel": "global"
  },
  "timestamp": 1234567890
}
```

#### 4. 心跳消息
```json
{
  "type": "ping",
  "timestamp": 1234567890
}
```

### 发送的消息格式（发送给客户端）

#### 1. 欢迎消息
```json
{
  "type": "welcome",
  "data": {
    "message": "欢迎加入游戏!",
    "serverTime": "2023-12-01 10:30:00",
    "onlineCount": 5
  }
}
```

#### 2. 游戏更新
```json
{
  "type": "game_update",
  "data": {
    "players": [
      {
        "playerId": "player1",
        "playerName": "Player1",
        "position": { "x": 10.5, "y": 20.3 }
      }
    ],
    "timestamp": 1234567890
  }
}
```

#### 3. 聊天广播
```json
{
  "type": "chat",
  "data": {
    "sender": "Player Name",
    "content": "Hello everyone!",
    "timestamp": "10:30:15"
  }
}
```

#### 4. 心跳响应
```json
{
  "type": "pong",
  "timestamp": 1234567890
}
```

#### 5. 错误消息
```json
{
  "type": "error",
  "data": {
    "message": "错误描述",
    "code": "ERROR_CODE",
    "timestamp": "2023-12-01 10:30:00"
  }
}
```

## 配置参数

### Inspector配置

```csharp
[Header("Server Settings")]
[SerializeField] private int port = 8081;                  // 服务器端口
[SerializeField] private int maxConnections = 10;          // 最大连接数
[SerializeField] private bool autoStart = true;            // 自动启动
[SerializeField] private bool enableDebugLog = true;       // 调试日志
```

### 代码配置

```csharp
// 在Start()方法中配置
networkServer.Initialize(
    serverPort: 8081,           // 服务器端口
    maxClientConnections: 20    // 最大连接数
);
```

## 错误处理

### 常见错误类型

1. **端口被占用**: 其他应用程序正在使用指定端口
2. **网络权限**: 防火墙阻止连接
3. **客户端格式错误**: 接收到无效的JSON数据
4. **连接超时**: 客户端连接超时或意外断开
5. **内存不足**: 服务器资源不足

### 错误处理最佳实践

```csharp
// 绑定错误事件
networkServer.OnError += (clientId, error) =>
{
    Debug.LogError($"服务器错误 (客户端: {clientId}): {error}");
    
    // 发送错误消息给客户端
    var errorMsg = new 
    { 
        type = "error", 
        data = new { message = error } 
    };
    
    if (!string.IsNullOrEmpty(clientId))
    {
        networkServer.SendToClient(clientId, errorMsg);
    }
};

// 消息处理中的错误处理
private void OnMessageReceived(string clientId, JsonData jsonData)
{
    try
    {
        // 处理消息...
    }
    catch (System.Exception e)
    {
        Debug.LogError($"处理消息时出错: {e.Message}");
        SendErrorMessage(clientId, "消息处理失败");
    }
}
```

## 性能优化

### 连接管理优化

```csharp
// 定期清理无效连接
private void Update()
{
    if (Time.time % 30f < Time.deltaTime) // 每30秒检查一次
    {
        CleanupInactiveConnections();
    }
}

private void CleanupInactiveConnections()
{
    var clients = networkServer.GetConnectedClients();
    foreach (var client in clients)
    {
        // 检查客户端是否长时间无响应
        if ((DateTime.Now - client.connectTime).TotalMinutes > 10)
        {
            Debug.Log($"清理无响应客户端: {client.clientId}");
            // 可以添加断开逻辑
        }
    }
}
```

### 消息批处理

```csharp
private List<object> messageQueue = new List<object>();
private float lastBatchTime;

public void QueueMessage(object message)
{
    messageQueue.Add(message);
}

private void Update()
{
    // 每100ms批量发送消息
    if (Time.time - lastBatchTime >= 0.1f && messageQueue.Count > 0)
    {
        var batchMessage = new 
        { 
            type = "batch", 
            messages = messageQueue.ToArray() 
        };
        
        networkServer.SendToClient(batchMessage);
        messageQueue.Clear();
        lastBatchTime = Time.time;
    }
}
```

## 部署指南

### 开发环境测试

1. 在Unity Editor中创建一个测试场景
2. 添加包含NetworkServerModule的GameObject
3. 配置服务器端口和最大连接数
4. 运行场景启动服务器

### 生产环境部署

#### 1. 构建设置
- 构建平台：PC, Mac & Linux Standalone
- Target Platform：根据服务器系统选择
- Architecture：x86_64

#### 2. 网络配置
```bash
# 开放服务器端口（以Ubuntu为例）
sudo ufw allow 8081

# 检查端口是否开放
sudo netstat -tulpn | grep :8081
```

#### 3. 服务器启动脚本
```bash
#!/bin/bash
# server_start.sh
cd /path/to/your/server
./YourServerName.x86_64 -batchmode -nographics
```

### Docker部署

```dockerfile
# Dockerfile
FROM ubuntu:20.04

# 安装依赖
RUN apt-get update && apt-get install -y \
    libx11-6 \
    libxrandr2 \
    libxinerama1 \
    libxcursor1

# 复制服务器文件
COPY ./Server /opt/game-server
WORKDIR /opt/game-server

# 设置执行权限
RUN chmod +x ./GameServer.x86_64

# 暴露端口
EXPOSE 8081

# 启动命令
CMD ["./GameServer.x86_64", "-batchmode", "-nographics"]
```

## 监控和日志

### 基础监控

```csharp
public class ServerMonitor : MonoBehaviour
{
    private NetworkServerModule server;
    private float lastLogTime;
    
    void Start()
    {
        server = GetComponent<NetworkServerModule>();
    }
    
    void Update()
    {
        // 每分钟记录服务器状态
        if (Time.time - lastLogTime >= 60f)
        {
            LogServerStatus();
            lastLogTime = Time.time;
        }
    }
    
    private void LogServerStatus()
    {
        Debug.Log($"[ServerMonitor] 在线客户端: {server.ConnectedClientCount}");
        Debug.Log($"[ServerMonitor] 服务器状态: {(server.IsRunning ? "运行中" : "已停止")}");
        Debug.Log($"[ServerMonitor] 内存使用: {GC.GetTotalMemory(false) / 1024 / 1024} MB");
    }
}
```

## 故障排除

### 常见问题解决

| 问题 | 可能原因 | 解决方案 |
|------|----------|----------|
| 服务器启动失败 | 端口被占用 | 更换端口或关闭占用端口的程序 |
| 客户端无法连接 | 防火墙阻止 | 配置防火墙允许对应端口 |
| 消息丢失 | 网络不稳定 | 实现消息确认机制 |
| 内存泄漏 | 未清理断开的连接 | 定期清理无效连接 |
| 性能下降 | 消息处理过频繁 | 实现消息批处理 |

### 调试工具

```csharp
// 在Inspector中显示调试信息
[System.Serializable]
public class ServerDebugInfo
{
    public bool isRunning;
    public int connectedClients;
    public string serverAddress;
    public float uptime;
}

public ServerDebugInfo debugInfo; // 在Inspector中可见

void Update()
{
    // 更新调试信息
    debugInfo.isRunning = networkServer.IsRunning;
    debugInfo.connectedClients = networkServer.ConnectedClientCount;
    debugInfo.serverAddress = networkServer.ServerAddress;
    debugInfo.uptime = Time.time;
}
```

## 总结

服务器端网络模块提供了：

✅ **简单易用**: 几行代码即可启动WebSocket服务器  
✅ **功能完整**: 支持多客户端连接管理和消息处理  
✅ **线程安全**: 内置MainThreadDispatcher解决Unity线程问题  
✅ **高性能**: 支持消息批处理和连接优化  
✅ **易于调试**: 内置日志和监控功能  
✅ **生产就绪**: 支持Docker部署和错误处理  

通过这个模块，您可以快速搭建稳定的Unity游戏服务器。 