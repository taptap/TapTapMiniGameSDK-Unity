#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
using UnityEngine;
using System.Collections.Generic;
using System;

namespace TapTapMiniGame.Editor
{
    /// <summary>
    /// TapSDK服务器状态数据
    /// 用作Editor和Runtime之间的数据桥梁
    /// </summary>
    [CreateAssetMenu(fileName = "TapSDKServerStatusData", menuName = "TapSDK/Server Status Data")]
    public class TapSDKServerStatusData : ScriptableObject
    {
        [Header("Server Information")]
        public bool isRunning;
        public string serverAddress;
        public int serverPort;
        public int connectedClientsCount;
        public string serverStatus;
        
        [Header("Performance Info")]
        public float cpuUsage;
        public long memoryUsage;
        public float uptime;
        
        [Header("Network Statistics")]
        public long totalMessagesSent;
        public long totalMessagesReceived;
        public long totalBytesTransferred;
        
        [Header("Client Information")]
        public List<ClientConnectionInfo> connectedClients = new List<ClientConnectionInfo>();
        
        [Header("Recent Events")]
        public List<ServerEvent> recentEvents = new List<ServerEvent>();
        
        [System.Serializable]
        public class ClientConnectionInfo
        {
            public string clientId;
            public string clientIP;
            public string connectionTime;
            public long messagesSent;
            public long messagesReceived;
            public string lastActivity;
        }
        
        [System.Serializable]
        public class ServerEvent
        {
            public string timestamp;
            public string eventType;
            public string description;
            public string clientId;
        }
        
        /// <summary>
        /// 更新服务器状态
        /// </summary>
        /// <param name="serverModule">服务器模块实例</param>
        public void UpdateFromServerModule(TapServer.NetworkServerModule serverModule)
        {
            if (serverModule == null)
            {
                ResetToDefault();
                return;
            }
            
            isRunning = serverModule.IsRunning;
            serverAddress = serverModule.ServerAddress;
            connectedClientsCount = serverModule.ConnectedClientCount;
            serverStatus = isRunning ? "Running" : "Stopped";
            
            // 更新端口（通过反射获取私有字段）
            try
            {
                var portField = typeof(TapServer.NetworkServerModule).GetField("serverPort", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (portField != null)
                {
                    serverPort = (int)portField.GetValue(serverModule);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to get server port: {ex.Message}");
                serverPort = 8081; // 默认端口
            }
            
            // 更新客户端信息
            UpdateClientInfo(serverModule.GetConnectedClients());
            
            // 更新性能信息
            UpdatePerformanceInfo();
        }
        
        private void UpdateClientInfo(List<TapServer.NetworkServerModule.ClientInfo> clients)
        {
            connectedClients.Clear();
            
            if (clients != null)
            {
                foreach (var client in clients)
                {
                    connectedClients.Add(new ClientConnectionInfo
                    {
                        clientId = client.clientId,
                        clientIP = client.clientIP,
                        connectionTime = client.connectTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        lastActivity = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    });
                }
            }
        }
        
        private void UpdatePerformanceInfo()
        {
            // 获取基本性能信息
            cpuUsage = 0f; // 可以集成CPU监控
            memoryUsage = System.GC.GetTotalMemory(false);
            
            // 计算运行时间
            if (isRunning)
            {
                uptime += Time.deltaTime;
            }
            else
            {
                uptime = 0f;
            }
        }
        
        /// <summary>
        /// 添加服务器事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="description">事件描述</param>
        /// <param name="clientId">客户端ID</param>
        public void AddEvent(string eventType, string description, string clientId = null)
        {
            var serverEvent = new ServerEvent
            {
                timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                eventType = eventType,
                description = description,
                clientId = clientId
            };
            
            recentEvents.Add(serverEvent);
            
            // 保持最近100个事件
            if (recentEvents.Count > 100)
            {
                recentEvents.RemoveAt(0);
            }
        }
        
        /// <summary>
        /// 重置为默认状态
        /// </summary>
        public void ResetToDefault()
        {
            isRunning = false;
            serverAddress = null;
            serverPort = 8081;
            connectedClientsCount = 0;
            serverStatus = "Not Initialized";
            cpuUsage = 0f;
            memoryUsage = 0;
            uptime = 0f;
            totalMessagesSent = 0;
            totalMessagesReceived = 0;
            totalBytesTransferred = 0;
            connectedClients.Clear();
            recentEvents.Clear();
        }
        
        /// <summary>
        /// 获取格式化的运行时间
        /// </summary>
        /// <returns>格式化的运行时间字符串</returns>
        public string GetFormattedUptime()
        {
            if (uptime < 60)
            {
                return $"{uptime:F0}s";
            }
            else if (uptime < 3600)
            {
                return $"{uptime / 60:F0}m {uptime % 60:F0}s";
            }
            else
            {
                return $"{uptime / 3600:F0}h {(uptime % 3600) / 60:F0}m";
            }
        }
        
        /// <summary>
        /// 获取格式化的内存使用量
        /// </summary>
        /// <returns>格式化的内存使用量字符串</returns>
        public string GetFormattedMemoryUsage()
        {
            if (memoryUsage < 1024)
            {
                return $"{memoryUsage} B";
            }
            else if (memoryUsage < 1024 * 1024)
            {
                return $"{memoryUsage / 1024:F1} KB";
            }
            else
            {
                return $"{memoryUsage / (1024 * 1024):F1} MB";
            }
        }
    }
} 
#endif