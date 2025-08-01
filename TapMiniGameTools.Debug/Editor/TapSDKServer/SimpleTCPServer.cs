#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
using UnityEngine;

namespace TapServer
{
    public class SimpleTCPServer : MonoBehaviour
    {
        [Header("Server Settings")]
        public int port = 8080;
        
        [Header("WebSocket Server Reference")]
        public UnityWebSocketServer webSocketServer;

        void Start()
        {
            StartWebSocketServer();
        }

        private void StartWebSocketServer()
        {
            if (webSocketServer == null)
            {
                // 如果没有手动分配WebSocket服务器，尝试在同一个GameObject上查找
                webSocketServer = GetComponent<UnityWebSocketServer>();
                if (webSocketServer == null)
                {
                    // 如果没有找到，动态添加一个
                    webSocketServer = gameObject.AddComponent<UnityWebSocketServer>();
                }
            }

            // 设置端口
            webSocketServer.port = port;
            
            // 启动WebSocket服务器
            webSocketServer.StartServer();
            
            Debug.Log("========================================");
            Debug.Log("WebSocket Server Started!");
            Debug.Log("Using UnityWebSocketServer for client connections");
            Debug.Log("========================================");
        }

        void OnDestroy()
        {
            if (webSocketServer != null)
            {
                webSocketServer.StopServer();
            }
        }

        void OnGUI()
        {
            // WebSocket服务器由UnityWebSocketServer处理GUI显示
            GUILayout.BeginArea(new Rect(10, 170, 300, 50));
            GUILayout.Label($"Server Mode: WebSocket");
            GUILayout.Label($"Managed by: UnityWebSocketServer");
            GUILayout.EndArea();
        }
    }
}
#endif