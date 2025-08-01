# TapSDK小游戏API调试功能

## 概述

本目录包含TapSDK小游戏API调试功能的核心实现，为开发者提供在Unity编辑器中调试小游戏API的完整解决方案。通过WebSocket服务器与小游戏客户端建立连接，实现API调用的实时调试和测试。

## 目录结构

```
Mock/
├── TapDebugBridge.cs          # 主要的API调试桥接类
├── TapDebugBridge.UI.cs       # UI相关API的桥接实现
├── TapDebugBridge.OpenApi.cs  # 开放接口API的桥接实现
├── TapDebugBridge.Other.cs    # 其他功能API的桥接实现
├── TapSDKApiUtil.cs          # API工具类，处理序列化和回调
├── Tap.Editor.cs             # 编辑器模式下的API实现
├── TapSDKServer/             # 服务器核心模块
│   ├── NetworkServerModule.cs    # 网络服务器管理
│   ├── UnityWebSocketServer.cs   # WebSocket服务器实现
│   └── MainThreadDispatcher.cs   # 主线程调度器
└── ApiTree.json              # API结构定义文件
```

## 工作原理

### 1. 架构设计

```
┌─────────────────┐    WebSocket     ┌──────────────────┐
│   小游戏客户端    │ ◄──────────────► │ Unity编辑器服务器  │
│                 │                 │                  │
│ • API调用请求    │                 │ • TapDebugBridge  │
│ • 接收响应数据    │                 │ • NetworkServer   │
│ • 显示调试信息    │                 │ • API模拟实现     │
└─────────────────┘                 └──────────────────┘
```

### 2. 消息流程

1. **客户端发起API调用**
   - 小游戏客户端调用TapSDK API
   - 将API调用封装为JSON消息
   - 通过WebSocket发送到Unity服务器

2. **服务器处理请求**
   - `NetworkServerModule`接收WebSocket消息
   - 解析消息获取API类型和参数
   - 调用对应的`TapDebugBridge`桥接方法

3. **API桥接执行**
   - `TapDebugBridge`根据API类型路由到具体实现
   - 使用`TapSDKApiUtil`处理参数序列化
   - 执行模拟的API逻辑或返回测试数据

4. **响应返回**
   - 将处理结果封装为响应消息
   - 通过WebSocket返回给客户端
   - 客户端接收并处理响应数据

## 核心组件

### NetworkServerModule
- **功能**：WebSocket服务器的核心管理类
- **特点**：
  - 单例模式，自动初始化
  - 支持多客户端连接
  - 提供消息收发API
  - 实时连接状态监控

### TapDebugBridge
- **功能**：API调试桥接的主要实现
- **分类**：
  - `TapDebugBridge.cs` - 基础桥接框架
  - `TapDebugBridge.UI.cs` - UI相关API
  - `TapDebugBridge.OpenApi.cs` - 开放接口API
  - `TapDebugBridge.Other.cs` - 其他功能API

### TapSDKApiUtil
- **功能**：API调用的工具类
- **特点**：
  - 自动排除回调函数字段进行序列化
  - 提供通用的回调调用方法
  - 支持延迟执行和异步处理

## 支持的API类别

### 基础系统API
- `GetSystemInfo` - 获取系统信息
- `GetDeviceInfo` - 获取设备信息
- `GetWindowInfo` - 获取窗口信息
- `OnShow`/`OnHide` - 应用生命周期监听

### UI界面API
- `ShowToast`/`HideToast` - Toast消息提示
- `ShowLoading`/`HideLoading` - 加载提示
- `ShowModal` - 模态对话框
- `ShowActionSheet` - 操作菜单
- `ShowShareMenu`/`HideShareMenu` - 分享菜单

### 用户相关API
- `GetUserInfo` - 获取用户信息
- `Login` - 用户登录
- `CheckSession` - 检查会话状态
- `Authorize` - 授权管理

### 功能性API
- `ShowShareboard` - 分享面板
- `OnShareMessage` - 分享消息监听
- `CreateHomeScreenWidget` - 桌面小组件
- `AchievementManager` - 成就系统
- `Leaderboard` - 排行榜系统

## 消息格式

### 请求消息
```json
{
  "type": "API名称",
  "param": {
    "参数名": "参数值",
    "success": "回调函数",
    "fail": "回调函数",
    "complete": "回调函数"
  }
}
```

### 响应消息
```json
{
  "status": "success|fail|complete",
  "data": {
    "errMsg": "错误信息",
    "结果字段": "返回值"
  }
}
```

## 使用方法

### 1. 环境要求
- Unity 2019.4或更高版本
- 定义编译符号：`TAP_DEBUG_ENABLE`
- 平台设置：WebGL或MiniGame

### 2. 启动调试服务器
```csharp
// 服务器会在进入Play模式时自动启动
// 默认监听端口：8081
// 可通过编辑器菜单查看：TapTap > SDK Server Monitor
```

### 3. 客户端连接
```javascript
// 客户端通过WebSocket连接到Unity服务器
// 连接地址：ws://[Unity编辑器IP]:8081
```

### 4. API调用示例
```javascript
// 客户端调用示例
const message = {
  type: "ShowToast",
  param: {
    title: "测试消息",
    icon: "success"
  }
};
webSocket.send(JSON.stringify(message));
```

## 扩展开发

### 添加新的API支持
1. 在对应的`TapDebugBridge.*.cs`文件中添加桥接方法
2. 使用`TapSDKApiUtil.CreateSerializableObject`处理参数
3. 通过`NetworkServerModule.Instance.SendMessage`发送响应

### 自定义回调处理
```csharp
// 使用通用桥接方法
CallApi<ResultType, OptionType>(
    apiType: "CustomAPI",
    option: option,
    onSuccess: option.success,
    onFail: option.fail,
    onComplete: option.complete
);
```

## 调试技巧

### 1. 日志输出
- 所有API调用都会在Unity控制台输出详细日志
- 日志格式：`[测试] 收到{API名称}回复: {响应内容}`

### 2. 服务器监控
- 使用`TapSDK Server Monitor`窗口查看：
  - 服务器运行状态
  - 客户端连接列表
  - 消息收发统计

### 3. 常见问题排查
- **连接失败**：检查网络和防火墙设置
- **API无响应**：确认编译符号是否正确
- **参数错误**：检查JSON格式和参数类型

## 技术特点

- **类型安全**：使用强类型的C#实现，避免运行时错误
- **自动序列化**：智能排除回调函数，支持复杂对象序列化
- **模块化设计**：按功能分类，便于维护和扩展
- **实时调试**：即时响应，支持断点调试和日志跟踪
- **多客户端支持**：可同时连接多个客户端进行测试

---

本调试功能为TapSDK小游戏开发提供了强大的调试支持，大大提高了开发效率和调试体验。 