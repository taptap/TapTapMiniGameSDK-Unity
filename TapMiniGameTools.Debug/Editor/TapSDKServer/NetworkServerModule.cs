#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace TapServer
{
    /// <summary>
    /// TapSDK 开发网络服务器 - 一站式解决方案
    /// 自动初始化、自动启动，提供简化的消息收发API
    /// 仅在Unity Editor环境下工作
    /// </summary>
    public class NetworkServerModule : MonoBehaviour
    {
        // 单例实例
        private static NetworkServerModule _instance;
        public static NetworkServerModule Instance
        {
            get
            {
                if (_instance == null)
                {
                    CreateInstance();
                }
                return _instance;
            }
        }

        [Header("开发服务器 (自动配置)")]
        [SerializeField] private bool enableDebugLog = true;
        [SerializeField] private bool showGUI = false;
        [SerializeField] private bool autoStartOnPlay = true;

        // 内部组件
        private UnityWebSocketServer webSocketServer;
        private int serverPort = 8081;
        
        // 客户端管理
        private Dictionary<string, ClientInfo> connectedClients = new Dictionary<string, ClientInfo>();
        private List<string> clientIds = new List<string>();
        
        // 客户端连接状态跟踪（用于等待协程）
        private bool hasClientConnected = false;
        
        // 消息回调系统（旧版，保留用于回滚）
        // private Dictionary<string, Action<string, ResponseData>> messageCallbacks = new Dictionary<string, Action<string, ResponseData>>();
        
        
        // 消息类型回调系统（基于 requestId 的精确匹配）
        private Dictionary<string, CallbackInfo> requestCallbacks = new Dictionary<string, CallbackInfo>();
        private readonly object _callbackLock = new object();  // 线程锁，保护requestCallbacks
        
        // 消息队列系统
        private Queue<QueuedRequest> sendQueue = new Queue<QueuedRequest>();
        private bool isProcessingSendQueue = false;  // 队列处理状态标志
        private int maxConcurrentRequests = 10;      // 最大并发数
        private int activeRequestCount = 0;          // 当前活跃请求数
        private float requestTimeout = 30f;          // 请求超时时间（秒）
        
        // 统计信息（用于调试）
        private int totalQueuedRequests = 0;         // 累计入队请求数
        private int totalProcessedRequests = 0;      // 累计处理请求数

        // 事件回调（保留给高级用户）
        public event Action<string> OnServerStarted;
        public event Action OnServerStopped;
        public event Action<string, string> OnClientConnected;  // clientId, clientIP
        public event Action<string> OnClientDisconnected;       // clientId
        public event Action<string, ResponseData> OnMessageReceived; // clientId, jsonData
        public event Action<string, string> OnTextMessageReceived; // clientId, textMessage
        public event Action<string, string> OnError;            // clientId, error

        // 服务器状态
        public bool IsRunning => webSocketServer != null && webSocketServer.IsRunning;
        public int ConnectedClientCount => clientIds.Count;
        public string ServerAddress => webSocketServer?.ServerAddress;

        // 客户端信息
        [System.Serializable]
        public class ClientInfo
        {
            public string clientId;
            public string clientIP;
            public DateTime connectTime;
        }

        /// <summary>
        /// 回调信息 - 存储单个请求的回调及元数据
        /// </summary>
        private class CallbackInfo
        {
            public Action<string, ResponseData> Callback;    // 回调函数
            public DateTime SendTime;                        // 发送时间（用于计算响应时间）
            public string MessageType;                       // 消息类型（用于日志和匹配）
            public string RequestId;                         // 请求ID（唯一标识）
        }

        /// <summary>
        /// 队列请求 - 存储待发送的消息
        /// </summary>
        private class QueuedRequest
        {
            public string MessageData;                       // 消息内容
            public Action<string, ResponseData> Callback;    // 回调函数
            public DateTime QueueTime;                       // 入队时间（用于监控）
        }

        /// <summary>
        /// 并发测试会话 - 追踪测试状态
        /// </summary>
        private class ConcurrentTestSession
        {
            public string TestId;
            public int ExpectedCount;
            public HashSet<int> ReceivedIndices = new HashSet<int>();
            public DateTime StartTime;
            public DateTime LastReceiveTime;
            public int MessageSize;
        }

        // 测试系统状态
        private ConcurrentTestSession currentTestSession = null;

        #region 单例模式

        /// <summary>
        /// 自动创建单例实例
        /// </summary>
        private static void CreateInstance()
        {
            // 确保只在Unity Editor中运行
            if (!Application.isEditor)
            {
                return;
            }

            // 查找是否已存在实例
            _instance = FindObjectOfType<NetworkServerModule>();
            
            if (_instance == null)
            {
                // 创建新的GameObject和组件
                GameObject serverObj = new GameObject("TapSDK_NetworkServer");
                _instance = serverObj.AddComponent<NetworkServerModule>();
                
                // 标记为DontDestroyOnLoad（可选）
                DontDestroyOnLoad(serverObj);
                
                Debug.Log("[TapSDK开发服务器] 自动创建单例实例");
            }
        }

        /// <summary>
        /// 静态初始化方法 - 在编辑器Play模式开始时自动调用
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInitialize()
        {
            // 确保只在Unity Editor中运行
            if (!Application.isEditor)
            {
                return;
            }

            // 自动创建并初始化实例
            var instance = Instance; // 这会触发CreateInstance()
            Debug.Log("[TapSDK开发服务器] 自动初始化完成");
        }

        #endregion

        #region Unity生命周期

        private void Awake()
        {
            // 确保只在Unity Editor中运行
            if (!Application.isEditor)
            {
                DestroyImmediate(gameObject);
                return;
            }

            // 单例检查
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeDevServer();
            }
            else if (_instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }
        }

        private void Start()
        {
            // 自动启动服务器
            if (autoStartOnPlay)
            {
                StartServer();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            // Editor Stop时自动关闭服务器
            if (pauseStatus && IsRunning)
            {
                // StopServer();
            }
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                CleanupCallbacks();
                StopServer();
                UnsubscribeFromServerEvents();
                _instance = null;
            }
        }

        #endregion

        #region 自动初始化

        private void InitializeDevServer()
        {
            // 自动创建和配置WebSocket服务器
            webSocketServer = GetComponent<UnityWebSocketServer>();
            if (webSocketServer == null)
            {
                webSocketServer = gameObject.AddComponent<UnityWebSocketServer>();
            }

            // 自动寻找可用端口
            serverPort = FindAvailablePort(8081);

            // 默认配置 - 开发环境优化
            webSocketServer.port = serverPort;
            webSocketServer.maxConnections = 10;
            webSocketServer.autoStart = false; // 我们手动控制
            webSocketServer.heartbeatTimeout = 60f;
            webSocketServer.showDebugInfo = enableDebugLog;
            webSocketServer.logMessages = enableDebugLog;

            // 绑定事件
            SubscribeToServerEvents();

            // 重置同步缓存
            TapTapMiniGame.TapSyncCache.ResetCache();
            
            // 始终显示端口信息，方便多Unity实例调试
            Debug.Log($"[TapSDK开发服务器] ✅ 初始化完成 - 使用端口: {serverPort}，TapEnv数据缓存已重置");
        }

        private int FindAvailablePort(int startPort = 8081)
        {
            const int maxPort = 8200; // 限制端口范围，避免无限循环
            
            for (int port = startPort; port <= maxPort; port++)
            {
                if (IsPortAvailable(port))
                {
                    // 始终显示找到的端口，方便多Unity实例调试
                    if (port != startPort)
                    {
                        Debug.Log($"[TapSDK开发服务器] ⚠️ 默认端口 {startPort} 被占用，使用端口: {port}");
                    }
                    return port;
                }
            }
            
            // 如果所有端口都被占用，返回默认端口（会在启动时报错）
            Debug.LogWarning($"[TapSDK开发服务器] ❌ 端口范围 {startPort}-{maxPort} 全部被占用，使用默认端口 {startPort} (可能会失败)");
            return startPort;
        }

        /// <summary>
        /// 检查指定端口是否可用
        /// 使用IPGlobalProperties来检测，不实际占用端口，避免端口释放延迟问题
        /// </summary>
        /// <param name="port">要检查的端口号</param>
        /// <returns>true表示端口可用，false表示被占用</returns>
        private bool IsPortAvailable(int port)
        {
            try
            {
                // 方法1: 使用IPGlobalProperties检测端口（推荐，不会实际占用端口）
                var ipGlobalProperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();
                
                // 检查TCP监听端口
                var tcpListeners = ipGlobalProperties.GetActiveTcpListeners();
                foreach (var endpoint in tcpListeners)
                {
                    if (endpoint.Port == port)
                    {
                        if (enableDebugLog)
                        {
                            Debug.Log($"[TapSDK开发服务器] 端口 {port} 已被TCP监听占用");
                        }
                        return false;
                    }
                }
                
                // 检查TCP连接端口
                var tcpConnections = ipGlobalProperties.GetActiveTcpConnections();
                foreach (var connection in tcpConnections)
                {
                    if (connection.LocalEndPoint.Port == port)
                    {
                        if (enableDebugLog)
                        {
                            Debug.Log($"[TapSDK开发服务器] 端口 {port} 已被TCP连接占用");
                        }
                        return false;
                    }
                }
                
                return true;
            }
            catch (System.Exception e)
            {
                // 如果IPGlobalProperties方法失败，使用备用方法
                if (enableDebugLog)
                {
                    Debug.LogWarning($"[TapSDK开发服务器] IPGlobalProperties检查失败: {e.Message}，使用备用检测方法");
                }
                
                // 方法2: 备用方案 - 尝试绑定端口（快速检测）
                System.Net.Sockets.Socket socket = null;
                try
                {
                    socket = new System.Net.Sockets.Socket(
                        System.Net.Sockets.AddressFamily.InterNetwork,
                        System.Net.Sockets.SocketType.Stream,
                        System.Net.Sockets.ProtocolType.Tcp
                    );
                    
                    socket.SetSocketOption(
                        System.Net.Sockets.SocketOptionLevel.Socket,
                        System.Net.Sockets.SocketOptionName.ReuseAddress,
                        false
                    );
                    
                    socket.Bind(new System.Net.IPEndPoint(System.Net.IPAddress.Any, port));
                    return true;
                }
                catch (System.Net.Sockets.SocketException)
                {
                    return false;
                }
                finally
                {
                    try
                    {
                        socket?.Close();
                        socket?.Dispose();
                    }
                    catch
                    {
                        // 忽略释放异常
                    }
                }
            }
        }

        private void SubscribeToServerEvents()
        {
            if (webSocketServer != null)
            {
                webSocketServer.OnServerStarted += HandleServerStarted;
                webSocketServer.OnServerStopped += HandleServerStopped;
                webSocketServer.OnClientConnected += HandleClientConnected;
                webSocketServer.OnClientDisconnected += HandleClientDisconnected;
                webSocketServer.OnMessageReceived += HandleMessageReceived;
            }
        }

        private void UnsubscribeFromServerEvents()
        {
            if (webSocketServer != null)
            {
                webSocketServer.OnServerStarted -= HandleServerStarted;
                webSocketServer.OnServerStopped -= HandleServerStopped;
                webSocketServer.OnClientConnected -= HandleClientConnected;
                webSocketServer.OnClientDisconnected -= HandleClientDisconnected;
                webSocketServer.OnMessageReceived -= HandleMessageReceived;
            }
        }
        

        #endregion

        #region 简化的公共API

        /// <summary>
        /// 生成唯一的请求ID
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <returns>唯一的请求ID</returns>
        private string GenerateRequestId(string messageType)
        {
            return $"{messageType}_{Guid.NewGuid():N}_{DateTime.Now.Ticks}";
        }

        /// <summary>
        /// 发送消息并设置回调 - 主要API
        /// </summary>
        /// <param name="messageData">要发送的JSON字符串数据（必须包含type字段）</param>
        /// <param name="callback">收到回复时的回调函数</param>
        public void SendMessage(string messageData, Action<string, ResponseData> callback = null)
        {
            if (!IsRunning)
            {
                LogWarning("服务器未运行，无法发送消息");
                // 立即调用回调返回错误
                callback?.Invoke("", new ResponseData 
                { 
                    status = "error", 
                    resultJson = "{\"errMsg\":\"服务器未运行\"}" 
                });
                return;
            }

            // 创建队列请求
            var queuedRequest = new QueuedRequest
            {
                MessageData = messageData,
                Callback = callback,
                QueueTime = DateTime.Now
            };

            // 加入队列（线程安全）
            lock (sendQueue)
            {
                sendQueue.Enqueue(queuedRequest);
                totalQueuedRequests++;
            }

            if (enableDebugLog)
            {
                Debug.Log($"[NetworkServerModule] 📥 消息入队 (队列长度: {sendQueue.Count})");
            }

            // 启动队列处理协程（如果尚未启动）
            if (!isProcessingSendQueue)
            {
                StartCoroutine(ProcessSendQueueCoroutine());
            }
        }

        /// <summary>
        /// 队列处理协程 - 控制并发发送
        /// </summary>
        private System.Collections.IEnumerator ProcessSendQueueCoroutine()
        {
            isProcessingSendQueue = true;
            
            if (enableDebugLog)
            {
                Debug.Log($"[NetworkServerModule] 📤 队列处理协程启动");
            }

            while (true)
            {
                QueuedRequest request = null;

                // 检查队列（线程安全）
                lock (sendQueue)
                {
                    // 调试工具：移除并发限制，只要队列非空就处理
                    if (sendQueue.Count > 0)
                    {
                        request = sendQueue.Dequeue();
                        
                        // 检查请求是否超时（在队列中等待过久）
                        var waitTime = (DateTime.Now - request.QueueTime).TotalSeconds;
                        if (waitTime > requestTimeout)
                        {
                            Debug.LogWarning($"[NetworkServerModule] ⏱️ 请求在队列中超时 (等待{waitTime:F2}秒)");
                            request.Callback?.Invoke("", new ResponseData 
                            { 
                                status = "queue_timeout", 
                                resultJson = $"{{\"errMsg\":\"队列超时({waitTime:F2}秒)\"}}" 
                            });
                            request = null; // 跳过此请求
                        }
                    }
                }

                // 处理请求
                if (request != null)
                {
                    SendMessageInternal(request.MessageData, request.Callback);
                    totalProcessedRequests++;
                }

                // 检查退出条件：调试工具模式下，只检查队列是否为空
                bool shouldExit = false;
                lock (sendQueue)
                {
                    shouldExit = (sendQueue.Count == 0);
                }

                if (shouldExit)
                {
                    isProcessingSendQueue = false;
                    if (enableDebugLog)
                    {
                        Debug.Log($"[NetworkServerModule] ✅ 队列处理完成 (总计: 入队{totalQueuedRequests}, 处理{totalProcessedRequests})");
                    }
                    yield break;
                }

                // 短暂等待，避免CPU占用过高
                yield return new WaitForSeconds(0.01f);
            }
        }

        /// <summary>
        /// 实际发送消息（内部方法）
        /// </summary>
        /// <param name="messageData">消息数据</param>
        /// <param name="callback">回调函数</param>
        private void SendMessageInternal(string messageData, Action<string, ResponseData> callback)
        {
            string messageType = "unknown";
            string requestId = null;

            try
            {
                // 1. 解析消息JSON
                JsonData jsonData = JsonMapper.ToObject(messageData);
                
                if (jsonData.ContainsKey("type"))
                {
                    messageType = jsonData["type"].ToString();
                }

                // 2. 生成并注入 requestId（如果有回调）
                if (callback != null)
                {
                    requestId = GenerateRequestId(messageType);
                    jsonData["requestId"] = requestId;
                    messageData = JsonMapper.ToJson(jsonData);
                }

                if (enableDebugLog)
                {
                    Debug.Log($"[NetworkServerModule] 📤 发送: {messageType}, RequestId: {requestId ?? "无"}");
                }
            }
            catch (Exception e)
            {
                LogError($"消息预处理失败: {e.Message}");
                callback?.Invoke("", new ResponseData 
                { 
                    status = "error", 
                    resultJson = $"{{\"errMsg\":\"预处理失败: {e.Message}\"}}" 
                });
                return;
            }

            // 3. 注册回调（使用 requestId 作为 key）
            if (callback != null && !string.IsNullOrEmpty(requestId))
            {
                lock (_callbackLock)
                {
                    requestCallbacks[requestId] = new CallbackInfo
                    {
                        Callback = callback,
                        SendTime = DateTime.Now,
                        MessageType = messageType,
                        RequestId = requestId
                    };
                    activeRequestCount++;
                    
                    if (enableDebugLog)
                    {
                        Debug.Log($"[NetworkServerModule] 📝 注册回调: RequestId={requestId}, Type={messageType}, 活跃数={activeRequestCount}");
                    }
                }
            }

            // 4. 构造并发送消息
            try
            {
                var message = new
                {
                    data = messageData,
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };

                BroadcastToAll(message);
            }
            catch (Exception e)
            {
                LogError($"发送消息失败: {e.Message}");
                
                // 失败时清理回调
                if (callback != null && !string.IsNullOrEmpty(requestId))
                {
                    lock (_callbackLock)
                    {
                        if (requestCallbacks.ContainsKey(requestId))
                        {
                            requestCallbacks.Remove(requestId);
                            activeRequestCount--;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置消息类型的回调处理
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="callback">回调函数</param>
        public void SetMessageCallback(string messageType, Action<string, ResponseData> callback)
        {
            // 注意：此方法已废弃，保留用于兼容性
            // messageCallbacks[messageType] = callback;
            LogWarning("SetMessageCallback已废弃，现在使用RequestId机制");
        }

        /// <summary>
        /// 等待客户端连接的协程
        /// 用于在游戏初始化流程中等待调试客户端（手机）连接
        /// </summary>
        /// <param name="timeout">超时时间（秒），默认30秒</param>
        /// <returns>协程迭代器</returns>
        public System.Collections.IEnumerator WaitForClientConnected(float timeout = 30f)
        {
            if (enableDebugLog)
            {
                Debug.Log($"[TapSDK开发服务器] 等待客户端连接... (超时: {timeout}秒)");
            }
            
            float elapsedTime = 0f;
            
            // 等待直到有客户端连接或超时
            while (!hasClientConnected && elapsedTime < timeout)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            if (hasClientConnected)
            {
                if (enableDebugLog)
                {
                    Debug.Log($"[TapSDK开发服务器] ✅ 客户端已连接，继续执行 (等待时间: {elapsedTime:F2}秒)");
                }
            }
            else
            {
                Debug.LogWarning($"[TapSDK开发服务器] ⏱️ 等待客户端连接超时 ({timeout}秒)，继续执行");
            }
        }

        /// <summary>
        /// 手动启动服务器
        /// </summary>
        public void StartServer()
        {
            if (webSocketServer == null)
            {
                LogError("WebSocket服务器未初始化");
                return;
            }

            if (IsRunning)
            {
                LogWarning("服务器已经在运行中");
                return;
            }

            try
            {
                // 重置客户端连接状态
                hasClientConnected = false;
                
                webSocketServer.StartServer();
            }
            catch (Exception e)
            {
                LogError($"启动服务器失败: {e.Message}");
                OnError?.Invoke("server", e.Message);
            }
        }

        /// <summary>
        /// 清理所有回调和队列
        /// </summary>
        private void CleanupCallbacks()
        {
            int cleanedCallbacks = 0;
            int cleanedQueue = 0;

            lock (_callbackLock)
            {
                // 清理所有 requestCallbacks
                cleanedCallbacks = requestCallbacks.Count;
                
                if (cleanedCallbacks > 0)
                {
                    Debug.LogWarning($"[NetworkServerModule] 🗑️ 清理 {cleanedCallbacks} 个未完成回调");
                }
                
                requestCallbacks.Clear();
                activeRequestCount = 0;
            }

            lock (sendQueue)
            {
                cleanedQueue = sendQueue.Count;
                
                if (cleanedQueue > 0)
                {
                    Debug.LogWarning($"[NetworkServerModule] 🗑️ 清理 {cleanedQueue} 个队列消息");
                }
                
                sendQueue.Clear();
            }

            isProcessingSendQueue = false;
            
            if (cleanedCallbacks > 0 || cleanedQueue > 0)
            {
                Debug.Log($"[NetworkServerModule] ✅ 清理完成");
            }
        }

        /// <summary>
        /// 手动停止服务器
        /// </summary>
        public void StopServer()
        {
            if (webSocketServer == null || !IsRunning) return;

            // 清理所有回调和队列
            CleanupCallbacks();

            try
            {
                webSocketServer.StopServer();
            }
            catch (Exception e)
            {
                LogError($"停止服务器出错: {e.Message}");
                OnError?.Invoke("server", e.Message);
            }
        }

        /// <summary>
        /// 广播消息到所有客户端
        /// </summary>
        /// <param name="data">要广播的数据</param>
        public void BroadcastToAll(object data)
        {
            if (!IsRunning)
            {
                LogWarning("服务器未运行，无法广播消息");
                return;
            }

            try
            {
                string jsonMessage = JsonMapper.ToJson(data);
                webSocketServer.BroadcastMessage(jsonMessage);
                
                if (enableDebugLog)
                {
                    Debug.Log($"[TapSDK开发服务器] 广播消息到 {clientIds.Count} 个客户端");
                }
            }
            catch (Exception e)
            {
                LogError($"广播消息失败: {e.Message}");
                OnError?.Invoke("broadcast", e.Message);
            }
        }

        /// <summary>
        /// 获取已连接的客户端列表
        /// </summary>
        public List<ClientInfo> GetConnectedClients()
        {
            return new List<ClientInfo>(connectedClients.Values);
        }

        #endregion

        #region 内部事件处理

        private void HandleServerStarted(string serverAddress)
        {
            // 始终显示服务器启动信息，方便多Unity实例调试
            Debug.Log($"[TapSDK开发服务器] ✅ 服务器启动成功 - 地址: {serverAddress} (端口: {serverPort})");
            
            OnServerStarted?.Invoke(serverAddress);
        }

        private void HandleServerStopped()
        {
            connectedClients.Clear();
            clientIds.Clear();
            
            if (enableDebugLog)
            {
                Debug.Log("[TapSDK开发服务器] ⛔ 服务器停止");
            }
            OnServerStopped?.Invoke();
        }

        private void HandleClientConnected(string clientId, string clientIP)
        {
            var clientInfo = new ClientInfo
            {
                clientId = clientId,
                clientIP = clientIP,
                connectTime = DateTime.Now
            };
            
            connectedClients[clientId] = clientInfo;
            clientIds.Add(clientId);
            
            // 标记已有客户端连接（用于等待协程）
            hasClientConnected = true;
            
            if (enableDebugLog)
            {
                Debug.Log($"[TapSDK开发服务器] 🔗 客户端连接: {clientIP} (总连接数: {clientIds.Count})");
            }
            
            // 延迟启动客户端数据同步流程
            StartCoroutine(InitializeClientDataSync(clientId));
            
            OnClientConnected?.Invoke(clientId, clientIP);
        }
        
        /// <summary>
        /// 初始化客户端数据同步流程 - 统一管理所有同步API
        /// </summary>
        private System.Collections.IEnumerator InitializeClientDataSync(string clientId)
        {
            // 等待连接稳定
            yield return new WaitForSeconds(1.0f);
            
            if (enableDebugLog)
            {
                Debug.Log($"[TapSDK开发服务器] 🔄 开始客户端数据同步流程 {clientId}");
            }
            
            // // 同步TapEnv数据
            // yield return StartCoroutine(RequestTapEnvData(clientId));
            
            // // 同步SystemInfo数据
            // yield return StartCoroutine(RequestSystemInfoData(clientId));
            
            // // 同步SystemSetting数据
            // yield return StartCoroutine(RequestSystemSettingData(clientId));
            
            // // 同步WindowInfo数据
            // yield return StartCoroutine(RequestWindowInfoData(clientId));
            
            // // 同步DeviceInfo数据
            // yield return StartCoroutine(RequestDeviceInfoData(clientId));
            
            // // 同步AppBaseInfo数据
            // yield return StartCoroutine(RequestAppBaseInfoData(clientId));
            
            // // 同步AppAuthorizeSetting数据
            // yield return StartCoroutine(RequestAppAuthorizeSettingData(clientId));
            
            // // 同步BatteryInfo数据
            // yield return StartCoroutine(RequestBatteryInfoData(clientId));
            
            if (enableDebugLog)
            {
                Debug.Log($"[TapSDK开发服务器] ✅ 客户端数据同步流程完成 {clientId}");
            }
        }
        
        /// <summary>
        /// 向客户端请求TapEnv数据同步
        /// </summary>
        private System.Collections.IEnumerator RequestTapEnvData(string clientId)
        {
            try
            {
                var requestMessage = new
                {
                    type = "SyncTapEnv",
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
                
                string messageData = JsonMapper.ToJson(requestMessage);
                
                // 发送请求（同步API，不等待complete）
                SendMessage(messageData, (responseClientId, response) =>
                {
                    // 直接在这里处理TapEnv数据更新
                    HandleTapEnvDataUpdate(responseClientId, response);
                });
                
                if (enableDebugLog)
                {
                    Debug.Log($"[TapSDK开发服务器] 📤 Requested TapEnv data from new client {clientId}");
                }
            }
            catch (Exception e)
            {
                LogError($"Failed to request TapEnv data from client {clientId}: {e.Message}");
            }
            
            yield return null;
        }
        
        /// <summary>
        /// 向客户端请求SystemInfo数据同步
        /// </summary>
        private System.Collections.IEnumerator RequestSystemInfoData(string clientId)
        {
            try
            {
                var requestMessage = new
                {
                    type = "SyncSystemInfo",
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
                
                string messageData = JsonMapper.ToJson(requestMessage);
                
                // 发送请求（同步API，不等待complete）
                SendMessage(messageData, (responseClientId, response) =>
                {
                    // 直接在这里处理SystemInfo数据更新
                    HandleSystemInfoDataUpdate(responseClientId, response);
                });
                
                if (enableDebugLog)
                {
                    Debug.Log($"[TapSDK开发服务器] 📤 Requested SystemInfo data from new client {clientId}");
                }
            }
            catch (Exception e)
            {
                LogError($"Failed to request SystemInfo data from client {clientId}: {e.Message}");
            }
            
            yield return null;
        }

        /// <summary>
        /// 向客户端请求SystemSetting数据同步
        /// </summary>
        private System.Collections.IEnumerator RequestSystemSettingData(string clientId)
        {
            try
            {
                var requestMessage = new
                {
                    type = "SyncSystemSetting",
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
                
                string messageData = JsonMapper.ToJson(requestMessage);
                
                // 发送请求（同步API，不等待complete）
                SendMessage(messageData, (responseClientId, response) =>
                {
                    HandleSystemSettingDataUpdate(responseClientId, response);
                });
                
                if (enableDebugLog)
                {
                    Debug.Log($"[TapSDK开发服务器] 📤 Requested SystemSetting data from new client {clientId}");
                }
            }
            catch (Exception e)
            {
                LogError($"Failed to request SystemSetting data from client {clientId}: {e.Message}");
            }
            
            yield return null;
        }

        /// <summary>
        /// 向客户端请求WindowInfo数据同步
        /// </summary>
        private System.Collections.IEnumerator RequestWindowInfoData(string clientId)
        {
            try
            {
                var requestMessage = new
                {
                    type = "SyncWindowInfo",
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
                
                string messageData = JsonMapper.ToJson(requestMessage);
                
                // 发送请求（同步API，不等待complete）
                SendMessage(messageData, (responseClientId, response) =>
                {
                    HandleWindowInfoDataUpdate(responseClientId, response);
                });
                
                if (enableDebugLog)
                {
                    Debug.Log($"[TapSDK开发服务器] 📤 Requested WindowInfo data from new client {clientId}");
                }
            }
            catch (Exception e)
            {
                LogError($"Failed to request WindowInfo data from client {clientId}: {e.Message}");
            }
            
            yield return null;
        }

        /// <summary>
        /// 向客户端请求DeviceInfo数据同步
        /// </summary>
        private System.Collections.IEnumerator RequestDeviceInfoData(string clientId)
        {
            try
            {
                var requestMessage = new
                {
                    type = "SyncDeviceInfo",
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
                
                string messageData = JsonMapper.ToJson(requestMessage);
                
                // 发送请求（同步API，不等待complete）
                SendMessage(messageData, (responseClientId, response) =>
                {
                    HandleDeviceInfoDataUpdate(responseClientId, response);
                });
                
                if (enableDebugLog)
                {
                    Debug.Log($"[TapSDK开发服务器] 📤 Requested DeviceInfo data from new client {clientId}");
                }
            }
            catch (Exception e)
            {
                LogError($"Failed to request DeviceInfo data from client {clientId}: {e.Message}");
            }
            
            yield return null;
        }

        /// <summary>
        /// 向客户端请求AppBaseInfo数据同步
        /// </summary>
        private System.Collections.IEnumerator RequestAppBaseInfoData(string clientId)
        {
            try
            {
                var requestMessage = new
                {
                    type = "SyncAppBaseInfo",
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
                
                string messageData = JsonMapper.ToJson(requestMessage);
                
                // 发送请求（同步API，不等待complete）
                SendMessage(messageData, (responseClientId, response) =>
                {
                    HandleAppBaseInfoDataUpdate(responseClientId, response);
                });
                
                if (enableDebugLog)
                {
                    Debug.Log($"[TapSDK开发服务器] 📤 Requested AppBaseInfo data from new client {clientId}");
                }
            }
            catch (Exception e)
            {
                LogError($"Failed to request AppBaseInfo data from client {clientId}: {e.Message}");
            }
            
            yield return null;
        }

        /// <summary>
        /// 向客户端请求AppAuthorizeSetting数据同步
        /// </summary>
        private System.Collections.IEnumerator RequestAppAuthorizeSettingData(string clientId)
        {
            try
            {
                var requestMessage = new
                {
                    type = "SyncAppAuthorizeSetting",
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
                
                string messageData = JsonMapper.ToJson(requestMessage);
                
                // 发送请求（同步API，不等待complete）
                SendMessage(messageData, (responseClientId, response) =>
                {
                    HandleAppAuthorizeSettingDataUpdate(responseClientId, response);
                });
                
                if (enableDebugLog)
                {
                    Debug.Log($"[TapSDK开发服务器] 📤 Requested AppAuthorizeSetting data from new client {clientId}");
                }
            }
            catch (Exception e)
            {
                LogError($"Failed to request AppAuthorizeSetting data from client {clientId}: {e.Message}");
            }
            
            yield return null;
        }

        /// <summary>
        /// 向客户端请求BatteryInfo数据同步
        /// </summary>
        private System.Collections.IEnumerator RequestBatteryInfoData(string clientId)
        {
            try
            {
                var requestMessage = new
                {
                    type = "SyncBatteryInfo",
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
                
                string messageData = JsonMapper.ToJson(requestMessage);
                
                // 发送请求（同步API，不等待complete）
                SendMessage(messageData, (responseClientId, response) =>
                {
                    HandleBatteryInfoDataUpdate(responseClientId, response);
                });
                
                if (enableDebugLog)
                {
                    Debug.Log($"[TapSDK开发服务器] 📤 Requested BatteryInfo data from new client {clientId}");
                }
            }
            catch (Exception e)
            {
                LogError($"Failed to request BatteryInfo data from client {clientId}: {e.Message}");
            }
            
            yield return null;
        }

        private void HandleClientDisconnected(string clientId)
        {
            if (connectedClients.ContainsKey(clientId))
            {
                connectedClients.Remove(clientId);
            }
            clientIds.Remove(clientId);
            
            // 如果所有客户端都断开，清理所有回调
            if (clientIds.Count == 0)
            {
                hasClientConnected = false;
                
                // 清理所有 requestCallbacks
                lock (_callbackLock)
                {
                    int clearedCount = requestCallbacks.Count;
                    requestCallbacks.Clear();
                    activeRequestCount = 0;
                    
                    if (clearedCount > 0 && enableDebugLog)
                    {
                        Debug.Log($"[NetworkServerModule] 🗑️ 客户端断开，清理了 {clearedCount} 个回调");
                    }
                }
            }
            
            if (enableDebugLog)
            {
                Debug.Log($"[TapSDK开发服务器] ❌ 客户端断开 (剩余连接数: {clientIds.Count})");
            }
            OnClientDisconnected?.Invoke(clientId);
        }

        private void HandleMessageReceived(string clientId, string message)
        {
            // 检查消息是否为JSON格式
            if (message.Trim().StartsWith("{") && message.Trim().EndsWith("}"))
            {
                try
                {
                    // 解析为通用JSON对象
                    JsonData jsonData = JsonMapper.ToObject(message);
                    
                    // 检查是否包含type字段
                    if (jsonData.ContainsKey("type"))
                    {
                        string messageType = jsonData["type"].ToString();
                        
                        // 特殊处理：BattleEvent事件推送
                        if (messageType == "BattleEvent")
                        {
                            HandleBattleEventMessage(clientId, jsonData);
                            return;
                        }
                        
                        // 特殊处理：Debug_TestMessage测试消息
                        if (messageType == "Debug_TestMessage")
                        {
                            HandleDebugTestMessage(clientId, jsonData);
                            return;
                        }

                        if (enableDebugLog)
                        {
                            Debug.Log($"[NetworkServerModule] 📩 收到响应-1: {messageType}");
                        }

                        // 创建ResponseData对象
                        ResponseData responseData = new ResponseData();
                        responseData.type = messageType;
                        
                        // 提取状态和结果数据
                        if (jsonData.ContainsKey("status"))
                        {
                            responseData.status = jsonData["status"].ToString();
                        }
                        
                        // 安全地提取结果数据
                        try
                        {
                            if (jsonData.ContainsKey("result"))
                            {
                                responseData.resultJson = JsonMapper.ToJson(jsonData["result"]);
                            }
                            else if (jsonData.ContainsKey("data"))
                            {
                                responseData.resultJson = JsonMapper.ToJson(jsonData["data"]);
                            }
                            else if (jsonData.ContainsKey("resultData"))
                            {
                                responseData.resultJson = JsonMapper.ToJson(jsonData["resultData"]);
                            }
                            else
                            {
                                // 如果没有result或data字段，使用整个消息作为结果
                                responseData.resultJson = message;
                            }
                        }
                        catch (Exception serializeEx)
                        {
                            // 如果序列化失败，直接使用原始消息
                            LogWarning($"JSON字段序列化失败: {serializeEx.Message}，使用原始消息");
                            responseData.resultJson = message;
                        }

                        // 提取 requestId
                        string requestId = null;
                        if (jsonData.ContainsKey("requestId"))
                        {
                            requestId = jsonData["requestId"].ToString();
                        }
                        
                        responseData.requestId = requestId;

                        // 触发通用事件
                        OnMessageReceived?.Invoke(clientId, responseData);

                        // 基于 requestId 精确匹配回调
                        if (!string.IsNullOrEmpty(requestId))
                        {
                            lock (_callbackLock)
                            {
                                if (requestCallbacks.ContainsKey(requestId))
                                {
                                    CallbackInfo callbackInfo = requestCallbacks[requestId];
                                    var responseTime = (DateTime.Now - callbackInfo.SendTime).TotalMilliseconds;

                                    if (enableDebugLog)
                                    {
                                        Debug.Log($"[NetworkServerModule] 📥 收到响应 - RequestId:{requestId}, Type:{callbackInfo.MessageType}, Status:{responseData.status}, ResponseTime:{responseTime:F2}ms");
                                    }

                                    try
                                    {
                                        callbackInfo.Callback?.Invoke(clientId, responseData);
                                        
                                        if (enableDebugLog)
                                        {
                                            Debug.Log($"[NetworkServerModule] ✅ 回调执行成功 - RequestId:{requestId}, Type:{callbackInfo.MessageType}, Status:{responseData.status}");
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        LogError($"回调执行出错: {e.Message}");
                                        SendErrorResponse(clientId, messageType, $"回调执行失败: {e.Message}");
                                    }

                                    // 不删除回调，永久保留
                                }
                                else
                                {
                                    // 未找到 requestId 对应的回调
                                    // 过滤 ping/pong 和 BattleEvent
                                    if (messageType != "ping" && messageType != "pong" && messageType != "BattleEvent")
                                    {
                                        if (enableDebugLog)
                                        {
                                            Debug.LogWarning($"[NetworkServerModule] ⚠️ 未找到 requestId 对应的回调: {requestId}, Type:{messageType}, Status:{responseData.status}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string logMessage = message.Length > 200 ? message.Substring(0, 200) + "..." : message;
                        LogWarning($"收到JSON消息但缺少type字段: {logMessage}");
                        SendErrorResponse(clientId, "unknown", "消息格式错误：缺少type字段");
                    }
                }
                catch (Exception e)
                {
                    string logMessage = message.Length > 200 ? message.Substring(0, 200) + "..." : message;
                    LogWarning($"JSON解析失败: {e.Message}，消息: {logMessage}");
                    SendErrorResponse(clientId, "parse_error", $"JSON解析失败: {e.Message}");
                    
                    // 同时触发文本消息处理
                    OnTextMessageReceived?.Invoke(clientId, message);
                }
            }
            else
            {
                // 处理普通文本消息
                if (enableDebugLog)
                {
                    string logMessage = message.Length > 200 ? message.Substring(0, 200) + "..." : message;
                    Debug.Log($"[TapSDK开发服务器] 收到文本: {logMessage}");
                }
                OnTextMessageReceived?.Invoke(clientId, message);
            }
        }

        /// <summary>
        /// 处理多人联机事件消息
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="message">事件消息JSON数据</param>
        private void HandleBattleEventMessage(string clientId, JsonData message)
        {
            try
            {
                if (!message.ContainsKey("eventType") || !message.ContainsKey("eventData"))
                {
                    LogError($"[NetworkServerModule] BattleEvent缺少字段: {message.ToJson()}");
                    return;
                }

                string eventType = message["eventType"].ToString();
                JsonData eventData = message["eventData"];

                // 转发到事件管理器
                TapBattleDebugEventManager.Instance.OnBattleEventReceived(eventType, eventData);

                if (enableDebugLog)
                {
                    Debug.Log($"[NetworkServerModule] 📥 {eventType}");
                }
            }
            catch (Exception e)
            {
                LogError($"[NetworkServerModule] BattleEvent处理失败 ({message.ToJson()}): {e.Message}");
            }
        }

        /// <summary>
        /// 向客户端发送错误响应
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="messageType">消息类型</param>
        /// <param name="errorMessage">错误消息</param>
        private void SendErrorResponse(string clientId, string messageType, string errorMessage)
        {
            try
            {
                var errorResponse = new
                {
                    type = messageType,
                    status = "error",
                    data = new { 
                        error = errorMessage, 
                        timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() 
                    }
                };
                
                string jsonResponse = JsonMapper.ToJson(errorResponse);
                webSocketServer?.BroadcastMessage(jsonResponse); // 暂时广播，实际应该只发给指定客户端
                
                if (enableDebugLog)
                {
                    Debug.Log($"[TapSDK开发服务器] 📤 发送错误响应: {errorMessage}");
                }
            }
            catch (Exception e)
            {
                LogError($"发送错误响应失败: {e.Message}");
            }
        }

        #endregion

        #region 日志辅助

        private void LogMessage(string message)
        {
            if (enableDebugLog)
            {
                Debug.Log($"[TapSDK开发服务器] {message}");
            }
        }

        private void LogWarning(string message)
        {
            if (enableDebugLog)
            {
                Debug.LogWarning($"[TapSDK开发服务器] {message}");
            }
        }

        private void LogError(string message)
        {
            Debug.LogError($"[TapSDK开发服务器] {message}");
        }

        #endregion

        #region 开发者GUI (可选)

        private void OnGUI()
        {
            if (!showGUI) return;

            // 简洁的开发者面板
            GUILayout.BeginArea(new Rect(10, 400, 300, 240));
            
            GUILayout.BeginVertical("box");
            GUILayout.Label("🛠️ TapSDK 开发服务器", new GUIStyle(GUI.skin.label) 
                { fontSize = 14, fontStyle = FontStyle.Bold });
            
            // 状态显示
            string status = IsRunning ? "✅ 运行中" : "❌ 已停止";
            GUILayout.Label($"状态: {status}");
            
            if (IsRunning)
            {
                GUILayout.Label($"地址: {ServerAddress}");
                GUILayout.Label($"连接数: {clientIds.Count}");
            }

            // 控制按钮
            GUILayout.BeginHorizontal();
            if (!IsRunning && GUILayout.Button("启动"))
            {
                StartServer();
            }
            if (IsRunning && GUILayout.Button("停止"))
            {
                StopServer();
            }
            GUILayout.EndHorizontal();

            // 测试按钮
            if (IsRunning && GUILayout.Button("📤 发送测试消息"))
            {
                string testMessage = JsonMapper.ToJson(new { type = "test", message = "服务器测试消息" });
                SendMessage(testMessage, (clientId, response) =>
                {
                    Debug.Log($"收到客户端回复: {response.ToJson()}");
                });
            }
            
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        /// <summary>
        /// 处理TapEnv数据更新消息
        /// </summary>
        private void HandleTapEnvDataUpdate(string clientId, ResponseData responseData)
        {
            try
            {
                if (responseData.status == "success")
                {
                    if (!string.IsNullOrEmpty(responseData.resultJson))
                    {
                        // 直接获取USER_DATA_PATH字符串，去除JSON序列化的引号
                        string userDataPath = responseData.resultJson.Trim('"');
                        
                        // 更新缓存的env数据
                        TapTapMiniGame.TapSyncCache.UpdateCache(userDataPath);
                        
                        if (enableDebugLog)
                        {
                            Debug.Log($"[TapSDK开发服务器] 📥 Updated TapEnv data from client {clientId}");
                            Debug.Log($"[TapSDK开发服务器] USER_DATA_PATH: {userDataPath}");
                        }
                    }
                    else
                    {
                        LogError($"TapEnv data update message missing resultData field from client {clientId}");
                    }
                }
                else
                {
                    LogError($"Client {clientId} failed to provide TapEnv data: {responseData.status}");
                }
            }
            catch (Exception e)
            {
                LogError($"Error processing TapEnv data update from client {clientId}: {e.Message}");
                LogError($"Response data: {responseData.ToJson()}");
            }
        }
        
        /// <summary>
        /// 处理SystemInfo数据更新消息
        /// </summary>
        private void HandleSystemInfoDataUpdate(string clientId, ResponseData responseData)
        {
            try
            {
                if (responseData.status == "success")
                {
                    if (!string.IsNullOrEmpty(responseData.resultJson))
                    {
                        // 解析SystemInfo JSON数据
                        var systemInfo = responseData.GetResult<TapTapMiniGame.SystemInfo>();
                        if (systemInfo != null)
                        {
                            // 更新缓存的SystemInfo数据
                            TapTapMiniGame.TapSyncCache.UpdateSystemInfoCache(systemInfo);
                            
                            if (enableDebugLog)
                            {
                                Debug.Log($"[TapSDK开发服务器] 📥 Updated SystemInfo data from client {clientId}");
                                Debug.Log($"[TapSDK开发服务器] Platform: {systemInfo.platform}, Brand: {systemInfo.brand}");
                            }
                        }
                        else
                        {
                            LogError($"Failed to parse SystemInfo data from client {clientId}");
                        }
                    }
                    else
                    {
                        LogError($"SystemInfo data update message missing resultData field from client {clientId}");
                    }
                }
                else
                {
                    LogError($"Client {clientId} failed to provide SystemInfo data: {responseData.status}");
                }
            }
            catch (Exception e)
            {
                LogError($"Error processing SystemInfo data update from client {clientId}: {e.Message}");
                LogError($"Response data: {responseData.ToJson()}");
            }
        }

        /// <summary>
        /// 处理SystemSetting数据更新
        /// </summary>
        private void HandleSystemSettingDataUpdate(string clientId, ResponseData responseData)
        {
            try
            {
                if (responseData.status == "success")
                {
                    if (!string.IsNullOrEmpty(responseData.resultJson))
                    {
                        var systemSetting = responseData.GetResult<TapTapMiniGame.SystemSetting>();
                        if (systemSetting != null)
                        {
                            TapTapMiniGame.TapSyncCache.UpdateSystemSettingCache(systemSetting);
                            
                            if (enableDebugLog)
                            {
                                Debug.Log($"[TapSDK开发服务器] 📥 Updated SystemSetting data from client {clientId}");
                            }
                        }
                        else
                        {
                            LogError($"Failed to parse SystemSetting data from client {clientId}");
                        }
                    }
                    else
                    {
                        LogError($"SystemSetting data update message missing resultData field from client {clientId}");
                    }
                }
                else
                {
                    LogError($"Client {clientId} failed to provide SystemSetting data: {responseData.status}");
                }
            }
            catch (Exception e)
            {
                LogError($"Error processing SystemSetting data update from client {clientId}: {e.Message}");
            }
        }

        /// <summary>
        /// 处理WindowInfo数据更新
        /// </summary>
        private void HandleWindowInfoDataUpdate(string clientId, ResponseData responseData)
        {
            try
            {
                if (responseData.status == "success")
                {
                    if (!string.IsNullOrEmpty(responseData.resultJson))
                    {
                        var windowInfo = responseData.GetResult<TapTapMiniGame.WindowInfo>();
                        if (windowInfo != null)
                        {
                            TapTapMiniGame.TapSyncCache.UpdateWindowInfoCache(windowInfo);
                            
                            if (enableDebugLog)
                            {
                                Debug.Log($"[TapSDK开发服务器] 📥 Updated WindowInfo data from client {clientId}");
                            }
                        }
                        else
                        {
                            LogError($"Failed to parse WindowInfo data from client {clientId}");
                        }
                    }
                    else
                    {
                        LogError($"WindowInfo data update message missing resultData field from client {clientId}");
                    }
                }
                else
                {
                    LogError($"Client {clientId} failed to provide WindowInfo data: {responseData.status}");
                }
            }
            catch (Exception e)
            {
                LogError($"Error processing WindowInfo data update from client {clientId}: {e.Message}");
            }
        }

        /// <summary>
        /// 处理DeviceInfo数据更新
        /// </summary>
        private void HandleDeviceInfoDataUpdate(string clientId, ResponseData responseData)
        {
            try
            {
                if (responseData.status == "success")
                {
                    if (!string.IsNullOrEmpty(responseData.resultJson))
                    {
                        var deviceInfo = responseData.GetResult<TapTapMiniGame.DeviceInfo>();
                        if (deviceInfo != null)
                        {
                            TapTapMiniGame.TapSyncCache.UpdateDeviceInfoCache(deviceInfo);
                            
                            if (enableDebugLog)
                            {
                                Debug.Log($"[TapSDK开发服务器] 📥 Updated DeviceInfo data from client {clientId}");
                            }
                        }
                        else
                        {
                            LogError($"Failed to parse DeviceInfo data from client {clientId}");
                        }
                    }
                    else
                    {
                        LogError($"DeviceInfo data update message missing resultData field from client {clientId}");
                    }
                }
                else
                {
                    LogError($"Client {clientId} failed to provide DeviceInfo data: {responseData.status}");
                }
            }
            catch (Exception e)
            {
                LogError($"Error processing DeviceInfo data update from client {clientId}: {e.Message}");
            }
        }

        /// <summary>
        /// 处理AppBaseInfo数据更新
        /// </summary>
        private void HandleAppBaseInfoDataUpdate(string clientId, ResponseData responseData)
        {
            try
            {
                if (responseData.status == "success")
                {
                    if (!string.IsNullOrEmpty(responseData.resultJson))
                    {
                        var appBaseInfo = responseData.GetResult<TapTapMiniGame.AppBaseInfo>();
                        if (appBaseInfo != null)
                        {
                            TapTapMiniGame.TapSyncCache.UpdateAppBaseInfoCache(appBaseInfo);
                            
                            if (enableDebugLog)
                            {
                                Debug.Log($"[TapSDK开发服务器] 📥 Updated AppBaseInfo data from client {clientId}");
                            }
                        }
                        else
                        {
                            LogError($"Failed to parse AppBaseInfo data from client {clientId}");
                        }
                    }
                    else
                    {
                        LogError($"AppBaseInfo data update message missing resultData field from client {clientId}");
                    }
                }
                else
                {
                    LogError($"Client {clientId} failed to provide AppBaseInfo data: {responseData.status}");
                }
            }
            catch (Exception e)
            {
                LogError($"Error processing AppBaseInfo data update from client {clientId}: {e.Message}");
            }
        }

        /// <summary>
        /// 处理AppAuthorizeSetting数据更新
        /// </summary>
        private void HandleAppAuthorizeSettingDataUpdate(string clientId, ResponseData responseData)
        {
            try
            {
                if (responseData.status == "success")
                {
                    if (!string.IsNullOrEmpty(responseData.resultJson))
                    {
                        var appAuthorizeSetting = responseData.GetResult<TapTapMiniGame.AppAuthorizeSetting>();
                        if (appAuthorizeSetting != null)
                        {
                            TapTapMiniGame.TapSyncCache.UpdateAppAuthorizeSettingCache(appAuthorizeSetting);
                            
                            if (enableDebugLog)
                            {
                                Debug.Log($"[TapSDK开发服务器] 📥 Updated AppAuthorizeSetting data from client {clientId}");
                            }
                        }
                        else
                        {
                            LogError($"Failed to parse AppAuthorizeSetting data from client {clientId}");
                        }
                    }
                    else
                    {
                        LogError($"AppAuthorizeSetting data update message missing resultData field from client {clientId}");
                    }
                }
                else
                {
                    LogError($"Client {clientId} failed to provide AppAuthorizeSetting data: {responseData.status}");
                }
            }
            catch (Exception e)
            {
                LogError($"Error processing AppAuthorizeSetting data update from client {clientId}: {e.Message}");
            }
        }

        /// <summary>
        /// 处理BatteryInfo数据更新
        /// </summary>
        private void HandleBatteryInfoDataUpdate(string clientId, ResponseData responseData)
        {
            try
            {
                if (responseData.status == "success")
                {
                    if (!string.IsNullOrEmpty(responseData.resultJson))
                    {
                        var batteryInfo = responseData.GetResult<TapTapMiniGame.GetBatteryInfoSyncResult>();
                        if (batteryInfo != null)
                        {
                            TapTapMiniGame.TapSyncCache.UpdateBatteryInfoCache(batteryInfo);
                            
                            if (enableDebugLog)
                            {
                                Debug.Log($"[TapSDK开发服务器] 📥 Updated BatteryInfo data from client {clientId}");
                            }
                        }
                        else
                        {
                            LogError($"Failed to parse BatteryInfo data from client {clientId}");
                        }
                    }
                    else
                    {
                        LogError($"BatteryInfo data update message missing resultData field from client {clientId}");
                    }
                }
                else
                {
                    LogError($"Client {clientId} failed to provide BatteryInfo data: {responseData.status}");
                }
            }
            catch (Exception e)
            {
                LogError($"Error processing BatteryInfo data update from client {clientId}: {e.Message}");
            }
        }
        
        #endregion
        
        #region 并发测试系统
        
        /// <summary>
        /// 处理调试测试消息
        /// </summary>
        private void HandleDebugTestMessage(string clientId, JsonData message)
        {
            if (currentTestSession == null)
            {
                Debug.LogError("[Debug Test] 收到测试消息，但没有活跃的测试会话");
                return;
            }
            
            try
            {
                string testId = message["testId"].ToString();
                int messageIndex = int.Parse(message["messageIndex"].ToString());
                
                if (testId != currentTestSession.TestId)
                {
                    Debug.LogWarning($"[Debug Test] 测试ID不匹配: 期望{currentTestSession.TestId}, 实际{testId}");
                    return;
                }
                
                currentTestSession.ReceivedIndices.Add(messageIndex);
                currentTestSession.LastReceiveTime = DateTime.Now;
                
                if (enableDebugLog)
                {
                    Debug.Log($"[Debug Test] 收到消息 [{messageIndex}/{currentTestSession.ExpectedCount}], 累计收到: {currentTestSession.ReceivedIndices.Count}");
                }
                
                // 检查是否所有消息都已收到
                if (currentTestSession.ReceivedIndices.Count == currentTestSession.ExpectedCount)
                {
                    FinalizeConcurrentTest(true);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Debug Test] 处理测试消息失败: {e.Message}\n{e.StackTrace}");
            }
        }
        
        /// <summary>
        /// 完成测试并输出结果
        /// </summary>
        private void FinalizeConcurrentTest(bool completedNormally)
        {
            if (currentTestSession == null) return;
            
            var elapsed = (currentTestSession.LastReceiveTime - currentTestSession.StartTime).TotalMilliseconds;
            int receivedCount = currentTestSession.ReceivedIndices.Count;
            int expectedCount = currentTestSession.ExpectedCount;
            
            Debug.Log($"========== 并发测试结果 ==========");
            Debug.Log($"测试ID: {currentTestSession.TestId}");
            Debug.Log($"消息大小: {currentTestSession.MessageSize} 字符");
            Debug.Log($"期望收到: {expectedCount} 条");
            Debug.Log($"实际收到: {receivedCount} 条");
            Debug.Log($"总耗时: {elapsed:F0} ms");
            
            if (receivedCount == expectedCount)
            {
                Debug.Log($"✅ 测试通过！所有消息都已收到");
            }
            else
            {
                Debug.LogError($"❌ 测试失败！丢失 {expectedCount - receivedCount} 条消息");
                
                // 找出丢失的消息编号
                List<int> missing = new List<int>();
                for (int i = 1; i <= expectedCount; i++)
                {
                    if (!currentTestSession.ReceivedIndices.Contains(i))
                    {
                        missing.Add(i);
                    }
                }
                
                if (missing.Count <= 20)
                {
                    Debug.LogError($"丢失的消息编号: {string.Join(", ", missing)}");
                }
                else
                {
                    Debug.LogError($"丢失的消息编号（前20个）: {string.Join(", ", missing.Take(20))}...");
                }
            }
            
            Debug.Log($"====================================");
            
            currentTestSession = null;
        }
        
        /// <summary>
        /// 启动并发消息测试
        /// </summary>
        public void StartConcurrentMessageTest(int messageCount, int messageSize = 200)
        {
            if (currentTestSession != null)
            {
                Debug.LogWarning("[Debug Test] 已有测试正在进行，请等待完成");
                return;
            }
            
            string testId = $"test_{DateTime.Now:yyyyMMdd_HHmmss}_{UnityEngine.Random.Range(1000, 9999)}";
            
            currentTestSession = new ConcurrentTestSession
            {
                TestId = testId,
                ExpectedCount = messageCount,
                MessageSize = messageSize,
                StartTime = DateTime.Now,
                LastReceiveTime = DateTime.Now
            };
            
            Debug.Log($"[Debug Test] 启动并发测试: testId={testId}, count={messageCount}, size={messageSize}");
            
            // 向Client发送测试指令（需要封装为带 data 字段的格式）
            var innerCommand = new
            {
                type = "Debug_StartConcurrentTest",
                testId = testId,
                messageCount = messageCount,
                messageSize = messageSize
            };
            
            var testCommand = new
            {
                data = JsonMapper.ToJson(innerCommand),
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            
            BroadcastToAll(testCommand);
            
            // 启动超时检测协程（30秒）
            StartCoroutine(CheckTestTimeout(testId, 30.0f));
        }
        
        /// <summary>
        /// 检查测试超时
        /// </summary>
        private System.Collections.IEnumerator CheckTestTimeout(string testId, float timeoutSeconds)
        {
            yield return new WaitForSeconds(timeoutSeconds);
            
            if (currentTestSession != null && currentTestSession.TestId == testId)
            {
                Debug.LogWarning($"[Debug Test] 测试超时（{timeoutSeconds}秒），强制结束");
                FinalizeConcurrentTest(false);
            }
        }
        
        #endregion
    }
    
    public class ResponseData
    {
        public string type = "";
        public string status = "";
        public string resultJson = "";
        public string requestId = "";

        public string ToJson()
        {
            return $"[{type}] {status}, {resultJson}";
        }

        /// <summary>
        /// 将 resultJson 转换为指定类型的对象
        /// </summary>
        /// <typeparam name="T">目标类型，如 TapCallbackResult、LoginSuccessCallbackResult 等</typeparam>
        /// <returns>转换后的对象，转换失败时返回 default(T)</returns>
        public T GetResult<T>()
        {
            if (string.IsNullOrEmpty(resultJson))
            {
                return default(T);
            }

            try
            {
                // 检查 resultJson 是否已经是 JSON 字符串
                if (resultJson.Trim().StartsWith("{") && resultJson.Trim().EndsWith("}"))
                {
                    return JsonMapper.ToObject<T>(resultJson);
                }
                else
                {
                    // 如果不是 JSON 格式，尝试解析为简单对象
                    Debug.LogWarning($"[TapSDK开发服务器] resultJson 不是标准 JSON 格式: {resultJson}");
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[TapSDK开发服务器] JSON 转换失败: {ex.Message}");
                Debug.LogError($"[TapSDK开发服务器] 目标类型: {typeof(T).Name}");
                Debug.LogError($"[TapSDK开发服务器] JSON 内容: {resultJson}");
                return default(T);
            }
        }

        /// <summary>
        /// 尝试将 resultJson 转换为指定类型的对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="result">转换结果</param>
        /// <returns>转换是否成功</returns>
        public bool TryGetResult<T>(out T result)
        {
            result = default(T);
            
            if (string.IsNullOrEmpty(resultJson))
            {
                return false;
            }

            try
            {
                if (resultJson.Trim().StartsWith("{") && resultJson.Trim().EndsWith("}"))
                {
                    result = JsonMapper.ToObject<T>(resultJson);
                    return true;
                }
                else
                {
                    Debug.LogWarning($"[TapSDK开发服务器] resultJson 不是标准 JSON 格式: {resultJson}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[TapSDK开发服务器] JSON 转换失败: {ex.Message}");
                Debug.LogError($"[TapSDK开发服务器] 目标类型: {typeof(T).Name}");
                Debug.LogError($"[TapSDK开发服务器] JSON 内容: {resultJson}");
                return false;
            }
        }
    }
}
#endif 