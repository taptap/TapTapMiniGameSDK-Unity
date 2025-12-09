#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using LitJson;
using System.Collections;

namespace TapServer
{
    /// <summary>
    /// Unity WebSocket服务器实现
    /// 纯WebSocket功能，无UI界面，通过代码配置
    /// </summary>
    public class UnityWebSocketServer : MonoBehaviour
    {
        // 内部配置 - 通过代码设置
        internal int port = 8081;
        internal bool autoStart = false;  // 默认不自动启动
        internal int maxConnections = 10;
        internal float heartbeatTimeout = 60f;
        internal bool showDebugInfo = false;  // 默认关闭调试信息
        internal bool logMessages = false;   // 默认关闭消息日志

        // 事件定义
        public event Action<string, string> OnClientConnected;    // clientId, clientIP
        public event Action<string> OnClientDisconnected;         // clientId
        public event Action<string, string> OnMessageReceived;    // clientId, message
        public event Action<string> OnServerStarted;              // serverAddress
        public event Action OnServerStopped;

        private TcpListener server;
        private Thread serverThread;
        private bool isRunning;
        private string serverIP;
        private readonly object clientsLock = new object();
        private Dictionary<string, WebSocketClient> clients = new Dictionary<string, WebSocketClient>();
        private Queue<LogEntry> logQueue = new Queue<LogEntry>();

        // WebSocket协议相关
        private const string WebSocketMagicString = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

        private struct LogEntry
        {
            public string message;
            public LogType type;
        }

        // 公共属性
        public bool IsRunning => isRunning;
        public int ConnectedClientCount
        {
            get
            {
                lock (clientsLock)
                {
                    return clients.Count;
                }
            }
        }
        public string ServerAddress => isRunning ? $"ws://{serverIP}:{port}" : null;

        void Start()
        {
            if (autoStart)
            {
                StartServer();
            }
        }

        void Update()
        {
            // 处理日志队列（在主线程中）
            while (logQueue.Count > 0)
            {
                LogEntry log = logQueue.Dequeue();
                switch (log.type)
                {
                    case LogType.Log:
                        Debug.Log(log.message);
                        break;
                    case LogType.Warning:
                        Debug.LogWarning(log.message);
                        break;
                    case LogType.Error:
                        Debug.LogError(log.message);
                        break;
                }
            }

            // 清理超时的客户端
            CleanupTimeoutClients();
        }

        public void StartServer()
        {
            if (isRunning) return;

            try
            {
                // 使用 TapSDKApiUtil 的优化IP获取方法
                string ipString = TapTapMiniGame.TapSDKApiUtil.GetLocalIPAddress();
                IPAddress localAddress = IPAddress.Parse(ipString);
                
                LogMessage($"Using IP from TapSDKApiUtil: {ipString}");

                serverIP = localAddress.ToString();
                server = new TcpListener(localAddress, port);
                server.Start();
                isRunning = true;

                LogMessage("WebSocket Server Started Successfully!");
                LogMessage($"Access Address: ws://{serverIP}:{port}");

                // 触发服务器启动事件
                MainThreadDispatcher.Enqueue(() => {
                    OnServerStarted?.Invoke(ServerAddress);
                });

                // 在新线程中监听客户端连接
                serverThread = new Thread(ListenForClients);
                serverThread.IsBackground = true;
                serverThread.Start();
            }
            catch (Exception e)
            {
                LogError($"Error starting server: {e.Message}");
            }
        }

        public void StopServer()
        {
            isRunning = false;
            
            lock (clientsLock)
            {
                foreach (var client in clients.Values)
                {
                    client.Close();
                }
                clients.Clear();
            }

            server?.Stop();
            serverThread?.Join(1000);
            LogMessage("WebSocket Server stopped.");

            // 触发服务器停止事件
            MainThreadDispatcher.Enqueue(() => {
                OnServerStopped?.Invoke();
            });
        }

        private void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    TcpClient tcpClient = server.AcceptTcpClient();
                    
                    lock (clientsLock)
                    {
                        if (clients.Count >= maxConnections)
                        {
                            LogWarning("Max connections reached, rejecting new client");
                            tcpClient.Close();
                            continue;
                        }
                    }

                    string clientId = Guid.NewGuid().ToString();
                    WebSocketClient wsClient = new WebSocketClient(tcpClient, clientId);
                    
                    Thread clientThread = new Thread(() => HandleClientCommunication(wsClient));
                    clientThread.IsBackground = true;
                    clientThread.Start();

                    string clientIP = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
                    LogMessage($"New client connected from: {clientIP} (ID: {clientId})");
                }
                catch (Exception e)
                {
                    if (isRunning)
                    {
                        LogError($"Error accepting client: {e.Message}");
                    }
                }
            }
        }

        private void HandleClientCommunication(WebSocketClient client)
        {
            string clientIP = null;
            try
            {
                // 执行WebSocket握手
                if (!PerformWebSocketHandshake(client))
                {
                    LogWarning($"WebSocket handshake failed for client {client.Id}");
                    client.Close();
                    return;
                }

                lock (clientsLock)
                {
                    clients[client.Id] = client;
                }

                // 获取客户端IP
                clientIP = ((IPEndPoint)client.TcpClient.Client.RemoteEndPoint).Address.ToString();

                LogMessage($"WebSocket handshake successful for client {client.Id}");

                // 触发客户端连接事件
                MainThreadDispatcher.Enqueue(() => {
                    OnClientConnected?.Invoke(client.Id, clientIP);
                });

                // 处理WebSocket消息
                while (isRunning && client.IsConnected)
                {
                    try
                    {
                        string message = client.ReceiveMessage();
                        if (message != null)
                        {
                            client.UpdateLastActivity();
                            ProcessMessage(client, message);
                            
                            // 触发消息接收事件
                            MainThreadDispatcher.Enqueue(() => {
                                OnMessageReceived?.Invoke(client.Id, message);
                            });
                        }
                        else
                        {
                            // 消息为null时，检查连接状态
                            if (!client.IsConnected)
                            {
                                LogMessage($"Client {client.Id} connection lost");
                                break;
                            }
                            
                            // 短暂休眠避免CPU占用过高
                            Thread.Sleep(10);
                        }
                    }
                    catch (Exception e)
                    {
                        LogError($"Error handling client {client.Id}: {e.Message}");
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                LogError($"Error in client communication: {e.Message}");
            }
            finally
            {
                lock (clientsLock)
                {
                    if (clients.ContainsKey(client.Id))
                    {
                        clients.Remove(client.Id);
                    }
                }
                client.Close();
                LogMessage($"Client {client.Id} disconnected");

                // 触发客户端断开事件
                MainThreadDispatcher.Enqueue(() => {
                    OnClientDisconnected?.Invoke(client.Id);
                });
            }
        }

        private bool PerformWebSocketHandshake(WebSocketClient client)
        {
            try
            {
                // 读取HTTP请求
                string request = client.ReadHttpRequest();
                if (string.IsNullOrEmpty(request))
                {
                    return false;
                }

                // 解析WebSocket密钥
                string webSocketKey = ExtractWebSocketKey(request);
                if (string.IsNullOrEmpty(webSocketKey))
                {
                    return false;
                }

                // 生成响应密钥
                string responseKey = GenerateWebSocketAcceptKey(webSocketKey);

                // 发送握手响应
                string response = 
                    "HTTP/1.1 101 Switching Protocols\r\n" +
                    "Upgrade: websocket\r\n" +
                    "Connection: Upgrade\r\n" +
                    $"Sec-WebSocket-Accept: {responseKey}\r\n" +
                    "\r\n";

                client.SendRaw(Encoding.UTF8.GetBytes(response));
                return true;
            }
            catch (Exception e)
            {
                LogError($"WebSocket handshake error: {e.Message}");
                return false;
            }
        }

        private string ExtractWebSocketKey(string request)
        {
            string pattern = @"Sec-WebSocket-Key:\s*(.+)";
            Match match = Regex.Match(request, pattern, RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value.Trim() : null;
        }

        private string GenerateWebSocketAcceptKey(string webSocketKey)
        {
            string combined = webSocketKey + WebSocketMagicString;
            byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(combined));
            return Convert.ToBase64String(hash);
        }

        private void ProcessMessage(WebSocketClient client, string message)
        {
            if (logMessages)
            {
                LogMessage($"Received from {client.Id}: {message}");
            }

            try
            {
                // 尝试解析JSON消息
                if (message.StartsWith("{") && message.EndsWith("}"))
                {
                    JsonData jsonData = JsonMapper.ToObject(message);
                    if (jsonData.Keys.Contains("type"))
                    {
                        string messageType = jsonData["type"].ToString();
                        ProcessJsonMessage(client, messageType, jsonData);
                        return;
                    }
                }

                // 处理普通文本消息
                ProcessTextMessage(client, message);
            }
            catch (Exception e)
            {
                LogError($"Error processing message: {e.Message}");
            }
        }

        protected virtual void ProcessJsonMessage(WebSocketClient client, string type, JsonData data)
        {
            switch (type)
            {
                case "ping":
                    // 响应心跳
                    var pongResponse = new { type = "pong", timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() };
                    client.SendMessage(JsonMapper.ToJson(pongResponse));
                    break;

                default:
                    // 回显JSON消息 - 将JsonData转换为字符串避免序列化问题
                    // var response = new { 
                    //     type = "echo", 
                    //     originalMessage = JsonMapper.ToJson(data), // 转换为字符串
                    //     timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    //     serverId = "Unity-WebSocket-Server"
                    // };
                    // client.SendMessage(JsonMapper.ToJson(response));
                    break;
            }
        }

        protected virtual void ProcessTextMessage(WebSocketClient client, string message)
        {
            // 简单回显
            // client.SendMessage($"Echo: {message}");
        }

        private void CleanupTimeoutClients()
        {
            if (Time.time % 5f < Time.deltaTime) // 每5秒检查一次
            {
                lock (clientsLock)
                {
                    var timeoutClients = new List<string>();
                    var disconnectedClients = new List<string>();
                    
                    foreach (var kvp in clients)
                    {
                        // 检查超时
                        if (kvp.Value.IsTimeout(heartbeatTimeout))
                        {
                            timeoutClients.Add(kvp.Key);
                        }
                        // 检查连接状态
                        else if (!kvp.Value.IsConnected)
                        {
                            disconnectedClients.Add(kvp.Key);
                        }
                    }

                    // 清理超时客户端
                    foreach (string clientId in timeoutClients)
                    {
                        LogMessage($"Client {clientId} timed out");
                        clients[clientId].Close();
                        clients.Remove(clientId);
                        
                        // 触发断开事件
                        MainThreadDispatcher.Enqueue(() => {
                            OnClientDisconnected?.Invoke(clientId);
                        });
                    }
                    
                    // 清理已断开的客户端
                    foreach (string clientId in disconnectedClients)
                    {
                        LogMessage($"Cleaning up disconnected client {clientId}");
                        clients[clientId].Close();
                        clients.Remove(clientId);
                        
                        // 触发断开事件
                        MainThreadDispatcher.Enqueue(() => {
                            OnClientDisconnected?.Invoke(clientId);
                        });
                    }
                }
            }
        }

        public void BroadcastMessage(string message)
        {
            lock (clientsLock)
            {
                var disconnectedClients = new List<string>();
                
                foreach (var kvp in clients)
                {
                    var client = kvp.Value;
                    try
                    {
                        // 检查连接状态，如果已断开就跳过并标记清理
                        if (!client.IsConnected)
                        {
                            disconnectedClients.Add(kvp.Key);
                            continue;
                        }
                        
                        client.SendMessage(message);
                    }
                    catch (Exception e)
                    {
                        LogError($"Error broadcasting to client {client.Id}: {e.Message}");
                        disconnectedClients.Add(kvp.Key);
                    }
                }
                
                // 清理已断开的连接
                foreach (string clientId in disconnectedClients)
                {
                    if (clients.ContainsKey(clientId))
                    {
                        LogMessage($"Cleaning up disconnected client {clientId} during broadcast");
                        clients[clientId].Close();
                        clients.Remove(clientId);
                        
                        // 触发断开事件
                        MainThreadDispatcher.Enqueue(() => {
                            OnClientDisconnected?.Invoke(clientId);
                        });
                    }
                }
            }
        }

        private void LogMessage(string message)
        {
            if (showDebugInfo)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    logQueue.Enqueue(new LogEntry { message = $"[WebSocket Server] {message}", type = LogType.Log });
                });
            }
        }

        private void LogWarning(string message)
        {
            if (showDebugInfo)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    logQueue.Enqueue(new LogEntry { message = $"[WebSocket Server] {message}", type = LogType.Warning });
                });
            }
        }

        private void LogError(string message)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                logQueue.Enqueue(new LogEntry { message = $"[WebSocket Server] {message}", type = LogType.Error });
            });
        }

        void OnDestroy()
        {
            StopServer();
        }
    }

    /// <summary>
    /// WebSocket客户端连接类
    /// </summary>
    public class WebSocketClient
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        private string id;
        private DateTime lastActivity;
        private bool isConnected;
        
        // 半包缓存：用于处理 TCP 流中的不完整 WebSocket 帧
        private byte[] partialFrameBuffer = null;
        private int partialFrameLength = 0;

        public string Id => id;
        public bool IsConnected => CheckConnectionStatus();
        public TcpClient TcpClient => tcpClient;

        private bool CheckConnectionStatus()
        {
            if (!isConnected) return false;
            
            try
            {
                // 多重检查连接状态
                if (tcpClient == null || !tcpClient.Connected) 
                {
                    isConnected = false;
                    return false;
                }
                
                // 检查Socket连接状态
                if (tcpClient.Client == null || !tcpClient.Client.Connected)
                {
                    isConnected = false;
                    return false;
                }
                
                // 使用Poll检查连接状态（更准确）
                if (tcpClient.Client.Poll(0, SelectMode.SelectRead) && tcpClient.Client.Available == 0)
                {
                    isConnected = false;
                    return false;
                }
                
                return true;
            }
            catch (Exception)
            {
                isConnected = false;
                return false;
            }
        }

        public WebSocketClient(TcpClient tcpClient, string id)
        {
            this.tcpClient = tcpClient;
            this.id = id;
            this.stream = tcpClient.GetStream();
            this.lastActivity = DateTime.Now;
            this.isConnected = true;
        }

        public void UpdateLastActivity()
        {
            lastActivity = DateTime.Now;
        }

        public bool IsTimeout(float timeoutSeconds)
        {
            return (DateTime.Now - lastActivity).TotalSeconds > timeoutSeconds;
        }

        public string ReadHttpRequest()
        {
            try
            {
                byte[] buffer = new byte[1024];
                StringBuilder request = new StringBuilder();
                
                while (stream.DataAvailable || request.Length == 0)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        request.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                        if (request.ToString().Contains("\r\n\r\n"))
                        {
                            break;
                        }
                    }
                }

                return request.ToString();
            }
            catch
            {
                return null;
            }
        }

        public void SendRaw(byte[] data)
        {
            if (IsConnected)
            {
                stream.Write(data, 0, data.Length);
            }
        }

        public void SendMessage(string message)
        {
            if (!IsConnected) return;

            try
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                byte[] frame = CreateWebSocketFrame(messageBytes);
                stream.Write(frame, 0, frame.Length);
            }
            catch (Exception e)
            {
                MainThreadDispatcher.SafeLogError($"Error sending message: {e.Message}");
                isConnected = false;
            }
        }

        public string ReceiveMessage()
        {
            if (!IsConnected) return null;

            try
            {
                byte[] workingBuffer;
                int workingLength;

                // 如果有半包缓存，使用缓存数据
                if (partialFrameBuffer != null && partialFrameLength > 0)
                {
                    // 尝试读取更多数据来完成半包
                    if (stream.DataAvailable)
                    {
                        byte[] tempBuffer = new byte[8192];
                        int newBytesRead = stream.Read(tempBuffer, 0, tempBuffer.Length);
                        
                        if (newBytesRead > 0)
                        {
                            // 合并缓存数据和新数据
                            byte[] combinedBuffer = new byte[partialFrameLength + newBytesRead];
                            Array.Copy(partialFrameBuffer, 0, combinedBuffer, 0, partialFrameLength);
                            Array.Copy(tempBuffer, 0, combinedBuffer, partialFrameLength, newBytesRead);
                            
                            workingBuffer = combinedBuffer;
                            workingLength = partialFrameLength + newBytesRead;
                            
                            // 清空缓存（稍后如果还是半包会重新缓存）
                            partialFrameBuffer = null;
                            partialFrameLength = 0;
                        }
                        else
                        {
                            return null; // 没有新数据
                        }
                    }
                    else
                    {
                        return null; // 等待更多数据
                    }
                }
                else
                {
                    // 检查是否有新数据可读
                    if (!stream.DataAvailable)
                    {
                        if (!tcpClient.Connected || !tcpClient.Client.Connected)
                        {
                            isConnected = false;
                            return null;
                        }
                        return null;
                    }

                    // 读取新数据
                    byte[] buffer = new byte[8192];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    
                    if (bytesRead == 0)
                    {
                        isConnected = false;
                        return null;
                    }
                    
                    workingBuffer = buffer;
                    workingLength = bytesRead;
                }

                // 解析 WebSocket 帧
                if (workingLength < 2)
                {
                    // 数据不足，缓存起来
                    CachePartialFrame(workingBuffer, workingLength);
                    return null;
                }

                bool fin = (workingBuffer[0] & 0x80) != 0;
                int opcode = workingBuffer[0] & 0x0F;
                bool masked = (workingBuffer[1] & 0x80) != 0;
                int payloadLength = workingBuffer[1] & 0x7F;
                
                // 处理关闭帧
                if (opcode == 8)
                {
                    isConnected = false;
                    return null;
                }
                
                int offset = 2;
                
                // 处理扩展长度
                if (payloadLength == 126)
                {
                    if (workingLength < offset + 2)
                    {
                        CachePartialFrame(workingBuffer, workingLength);
                        return null;
                    }
                    payloadLength = (workingBuffer[2] << 8) | workingBuffer[3];
                    offset += 2;
                }
                else if (payloadLength == 127)
                {
                    if (workingLength < offset + 8)
                    {
                        CachePartialFrame(workingBuffer, workingLength);
                        return null;
                    }
                    // 只使用低32位
                    payloadLength = (workingBuffer[6] << 24) | (workingBuffer[7] << 16) | (workingBuffer[8] << 8) | workingBuffer[9];
                    offset += 8;
                    
                    // 限制最大消息大小
                    if (payloadLength > 1024 * 1024)
                    {
                        MainThreadDispatcher.SafeLogError($"Message too large: {payloadLength} bytes");
                        isConnected = false;
                        return null;
                    }
                }

                // 处理掩码
                byte[] maskingKey = new byte[4];
                if (masked)
                {
                    if (workingLength < offset + 4)
                    {
                        CachePartialFrame(workingBuffer, workingLength);
                        return null;
                    }
                    Array.Copy(workingBuffer, offset, maskingKey, 0, 4);
                    offset += 4;
                }

                // 检查是否有完整的负载数据
                int totalFrameSize = offset + payloadLength;
                if (workingLength < totalFrameSize)
                {
                    // 半包：缓存当前数据，等待下次读取
                    CachePartialFrame(workingBuffer, workingLength);
                    return null;
                }

                // 提取负载数据
                byte[] payload = new byte[payloadLength];
                Array.Copy(workingBuffer, offset, payload, 0, payloadLength);

                // 解除掩码
                if (masked)
                {
                    for (int i = 0; i < payloadLength; i++)
                    {
                        payload[i] = (byte)(payload[i] ^ maskingKey[i % 4]);
                    }
                }

                // 如果缓冲区中还有剩余数据（粘包），缓存起来供下次使用
                int remainingBytes = workingLength - totalFrameSize;
                if (remainingBytes > 0)
                {
                    byte[] remainingBuffer = new byte[remainingBytes];
                    Array.Copy(workingBuffer, totalFrameSize, remainingBuffer, 0, remainingBytes);
                    CachePartialFrame(remainingBuffer, remainingBytes);
                }

                return Encoding.UTF8.GetString(payload);
            }
            catch (Exception e)
            {
                MainThreadDispatcher.SafeLogError($"WebSocket error: {e.Message}");
                isConnected = false;
                partialFrameBuffer = null;
                partialFrameLength = 0;
                return null;
            }
        }

        /// <summary>
        /// 缓存不完整的 WebSocket 帧数据
        /// </summary>
        private void CachePartialFrame(byte[] data, int length)
        {
            if (length > 0)
            {
                partialFrameBuffer = new byte[length];
                Array.Copy(data, 0, partialFrameBuffer, 0, length);
                partialFrameLength = length;
            }
        }

        private byte[] CreateWebSocketFrame(byte[] payload)
        {
            int frameLength = 2 + payload.Length;
            if (payload.Length > 125)
            {
                frameLength += 2;
            }

            byte[] frame = new byte[frameLength];
            frame[0] = 0x81; // FIN + Text frame
            
            if (payload.Length <= 125)
            {
                frame[1] = (byte)payload.Length;
                Array.Copy(payload, 0, frame, 2, payload.Length);
            }
            else
            {
                frame[1] = 126;
                frame[2] = (byte)(payload.Length >> 8);
                frame[3] = (byte)(payload.Length & 0xFF);
                Array.Copy(payload, 0, frame, 4, payload.Length);
            }

            return frame;
        }

        public void Close()
        {
            isConnected = false;
            stream?.Close();
            tcpClient?.Close();
        }
    }
} 
#endif 