#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
using UnityEngine;
using System;
using System.Collections.Generic;
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
        
        // 消息回调系统
        private Dictionary<string, Action<string, ResponseData>> messageCallbacks = new Dictionary<string, Action<string, ResponseData>>();

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
            
            if (enableDebugLog)
            {
                Debug.Log($"[TapSDK开发服务器] 初始化完成，端口: {serverPort}，TapEnv数据缓存已重置");
            }
        }

        private int FindAvailablePort(int startPort = 8081)
        {
            const int maxPort = 8200; // 限制端口范围，避免无限循环
            
            for (int port = startPort; port <= maxPort; port++)
            {
                if (IsPortAvailable(port))
                {
                    if (enableDebugLog)
                    {
                        Debug.Log($"[TapSDK开发服务器] 找到可用端口: {port}");
                    }
                    return port;
                }
                else if (enableDebugLog)
                {
                    Debug.Log($"[TapSDK开发服务器] 端口 {port} 已被占用，尝试下一个");
                }
            }
            
            // 如果所有端口都被占用，返回默认端口（会在启动时报错）
            Debug.LogWarning($"[TapSDK开发服务器] 端口范围 {startPort}-{maxPort} 全部被占用，使用默认端口 {startPort}");
            return startPort;
        }

        /// <summary>
        /// 检查指定端口是否可用
        /// </summary>
        /// <param name="port">要检查的端口号</param>
        /// <returns>true表示端口可用，false表示被占用</returns>
        private bool IsPortAvailable(int port)
        {
            System.Net.Sockets.TcpListener listener = null;
            try
            {
                // 使用TcpListener测试端口
                listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Any, port);
                listener.Start();
                return true;
            }
            catch (System.Net.Sockets.SocketException)
            {
                // 端口被占用或其他网络错误
                return false;
            }
            catch (System.Exception e)
            {
                // 其他异常，假设端口不可用
                if (enableDebugLog)
                {
                    Debug.LogWarning($"[TapSDK开发服务器] 检查端口 {port} 时出现异常: {e.Message}");
                }
                return false;
            }
            finally
            {
                // 确保在所有情况下都释放监听器
                try
                {
                    listener?.Stop();
                }
                catch
                {
                    // 忽略Stop时的异常
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
        /// 发送消息并设置回调 - 主要API
        /// </summary>
        /// <param name="messageData">要发送的JSON字符串数据（必须包含type字段）</param>
        /// <param name="callback">收到回复时的回调函数</param>
        public void SendMessage(string messageData, Action<string, ResponseData> callback = null)
        {
            if (!IsRunning)
            {
                LogWarning("服务器未运行，无法发送消息");
                return;
            }

            // 尝试从messageData中提取type信息
            string messageType = null;
            try
            {
                // 直接解析JSON字符串
                JsonData jsonData = JsonMapper.ToObject(messageData);
                if (jsonData.ContainsKey("type"))
                {
                    messageType = jsonData["type"].ToString();
                }
            }
            catch (Exception e)
            {
                LogWarning($"无法从messageData中提取type信息: {e.Message}");
            }

            // 注册回调
            if (callback != null && !string.IsNullOrEmpty(messageType))
            {
                messageCallbacks[messageType] = callback;
            }

            // 构造消息
            var message = new
            {
                data = messageData,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            // 发送给所有客户端
            BroadcastToAll(message);

            if (enableDebugLog)
            {
                Debug.Log($"[TapSDK开发服务器] 发送消息: {messageType ?? "unknown"}");
            }
        }

        /// <summary>
        /// 设置消息类型的回调处理
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="callback">回调函数</param>
        public void SetMessageCallback(string messageType, Action<string, ResponseData> callback)
        {
            messageCallbacks[messageType] = callback;
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
                webSocketServer.StartServer();
            }
            catch (Exception e)
            {
                LogError($"启动服务器失败: {e.Message}");
                OnError?.Invoke("server", e.Message);
            }
        }

        /// <summary>
        /// 手动停止服务器
        /// </summary>
        public void StopServer()
        {
            if (webSocketServer == null || !IsRunning) return;

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
            if (enableDebugLog)
            {
                Debug.Log($"[TapSDK开发服务器] ✅ 服务器启动: {serverAddress}");
            }
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
            
            // 同步TapEnv数据
            yield return StartCoroutine(RequestTapEnvData(clientId));
            
            // 同步SystemInfo数据
            yield return StartCoroutine(RequestSystemInfoData(clientId));
            
            // 同步SystemSetting数据
            yield return StartCoroutine(RequestSystemSettingData(clientId));
            
            // 同步WindowInfo数据
            yield return StartCoroutine(RequestWindowInfoData(clientId));
            
            // 同步DeviceInfo数据
            yield return StartCoroutine(RequestDeviceInfoData(clientId));
            
            // 同步AppBaseInfo数据
            yield return StartCoroutine(RequestAppBaseInfoData(clientId));
            
            // 同步AppAuthorizeSetting数据
            yield return StartCoroutine(RequestAppAuthorizeSettingData(clientId));
            
            // 同步BatteryInfo数据
            yield return StartCoroutine(RequestBatteryInfoData(clientId));
            
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
                
                // 发送请求
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
                
                // 发送请求
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
                
                // 发送请求
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
                        
                        if (enableDebugLog)
                        {
                            Debug.Log($"[TapSDK开发服务器] 收到消息类型: {messageType}");
                        }
                        

                        // 创建ResponseData对象用于回调
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

                        // 触发通用事件
                        OnMessageReceived?.Invoke(clientId, responseData);

                        // 查找并执行回调
                        if (messageCallbacks.ContainsKey(messageType))
                        {
                            try
                            {
                                messageCallbacks[messageType]?.Invoke(clientId, responseData);
                            }
                            catch (Exception e)
                            {
                                LogError($"执行消息回调出错 ({messageType}): {e.Message}");
                                SendErrorResponse(clientId, messageType, $"回调执行失败: {e.Message}");
                            }
                        }
                        else if (enableDebugLog)
                        {
                            Debug.Log($"[TapSDK开发服务器] 未找到消息类型 '{messageType}' 的回调处理");
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
                
                // // 简单回应文本消息
                // var response = new
                // {
                //     type = "text_echo",
                //     status = "success",
                //     data = new { originalMessage = message, serverTime = DateTime.Now.ToString() }
                // };
                // BroadcastToAll(response);
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
    }
    
    public class ResponseData
    {
        public string type = "";
        public string status = "";
        public string resultJson = "";

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