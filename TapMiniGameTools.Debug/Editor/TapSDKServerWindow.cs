#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
using TapServer;
#endif

namespace TapTapMiniGame.Editor
{
    /// <summary>
    /// TapSDK服务器状态监控窗口
    /// 显示服务器信息：IP、端口、运行状态、客户端连接数量等
    /// </summary>
    public partial class TapSDKServerWindow : EditorWindow
    {
        #region 常量
        public const string TAP_DEBUG_DEFINE = "TAP_DEBUG_ENABLE";
        #endregion

        #region 窗口管理
        [MenuItem("TapTap小游戏/Tools/调试工具")]
        public static void ShowWindow()
        {
            var window = GetWindow<TapSDKServerWindow>("TapMiniGame调试工具");
            window.minSize = new Vector2(400, 300);
            window.Show();
        }
        #endregion

        #region 私有变量
        private bool autoRefresh = true;
        private float refreshInterval = 1.0f;
        private double lastRefreshTime;
        
        private Vector2 scrollPosition;
        private GUIStyle headerStyle;
        private GUIStyle statusStyle;
        private GUIStyle clientItemStyle;
        private GUIStyle warningStyle;
        private bool showClientDetails = true;
        private bool autoGenerateQRCode = true; // 自动生成二维码开关
        
        // 调试开关状态
        private bool isDebugEnabled;
        private bool panelDebugEnabled = false;
        
        // 缓存的服务器信息
        private ServerInfo cachedServerInfo;
        private bool previousServerRunning = false; // 用于检测服务器状态变化
#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
        private List<NetworkServerModule.ClientInfo> cachedClients;
#endif
        
        // 二维码相关
        private Texture2D qrCodeTexture;
        private bool showQRCode = false;
        #endregion

        #region 服务器信息结构
        private struct ServerInfo
        {
            public bool isRunning;
            public string serverAddress;
            public int port;
            public int clientCount;
            public string status;
        }
        #endregion

        #region Unity生命周期
        private void OnEnable()
        {
            EditorApplication.update += OnEditorUpdate;
            CheckDebugEnabled();
            
            // 重置服务器状态追踪
            previousServerRunning = false;
            
            if (isDebugEnabled)
            {
                RefreshServerInfo();
            }
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
            // 清理二维码纹理
            if (qrCodeTexture != null)
            {
                DestroyImmediate(qrCodeTexture);
            }
        }

        private void OnEditorUpdate()
        {
            // 定期检查调试模式状态变化（独立于当前状态）
            if (autoRefresh && EditorApplication.timeSinceStartup - lastRefreshTime > refreshInterval)
            {
                bool previousDebugState = isDebugEnabled;
                CheckDebugEnabled(); // 重新检查调试状态
                
                // 如果调试状态发生变化，强制刷新界面
                if (previousDebugState != isDebugEnabled)
                {
                    Repaint();
                    lastRefreshTime = EditorApplication.timeSinceStartup;
                    return; // 状态改变后立即刷新，避免重复处理
                }
                
                // 只有在调试模式开启且自动刷新开启时才刷新服务器信息
                if (isDebugEnabled && autoRefresh)
                {
                    RefreshServerInfo();
                    Repaint();
                }
                
                lastRefreshTime = EditorApplication.timeSinceStartup;
            }
        }
        #endregion

        #region GUI绘制
        private void OnGUI()
        {
            // 确保最小窗口尺寸
            if (position.width < 600 || position.height < 400)
            {
                minSize = new Vector2(600, 400);
            }

            InitializeStyles();
            
            EditorGUILayout.Space(10);
            
            // 标题
            GUILayout.Label("TapMiniGame 调试工具", headerStyle);
            
            EditorGUILayout.Space(10);
            
            // 调试开关控制
            DrawDebugToggle();
            
            EditorGUILayout.Space(10);
            
            // 根据调试开关状态显示不同内容
            if (isDebugEnabled)
            {
                // 控制面板
                if (panelDebugEnabled)
                    DrawControlPanel();
                
                EditorGUILayout.Space(10);
                
                // 服务器状态
                DrawServerStatus();
                
                EditorGUILayout.Space(10);

                // 本地调试工具区域
                DrawLocalDebugTools();
                
                EditorGUILayout.Space(10);
                
                // 客户端列表
                DrawClientList();
                
                EditorGUILayout.Space(10);
                
                // 操作按钮
                DrawActionButtons();
            }
            else
            {
                // 显示调试开关未开启的提示
                DrawDebugDisabledMessage();
            }
        }

        private void InitializeStyles()
        {
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 16,
                    alignment = TextAnchor.MiddleCenter
                };
            }
            
            if (statusStyle == null)
            {
                statusStyle = new GUIStyle(EditorStyles.label)
                {
                    fontSize = 12,
                    normal = { textColor = Color.gray }
                };
            }
            
            if (clientItemStyle == null)
            {
                clientItemStyle = new GUIStyle(EditorStyles.helpBox)
                {
                    padding = new RectOffset(10, 10, 5, 5),
                    margin = new RectOffset(0, 0, 2, 2)
                };
            }
            
            if (warningStyle == null)
            {
                warningStyle = new GUIStyle(EditorStyles.label)
                {
                    fontSize = 14,
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = Color.yellow }
                };
            }
        }

        private void DrawDebugToggle()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Debug Control", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            
            bool newDebugEnabled = EditorGUILayout.Toggle("Enable TapSDK Debug", isDebugEnabled);
            
            if (newDebugEnabled != isDebugEnabled)
            {
                SetDebugEnabled(newDebugEnabled);
            }
            
            // 显示当前状态
            string statusText = isDebugEnabled ? "ENABLED" : "DISABLED";
            GUI.color = isDebugEnabled ? Color.green : Color.red;
            GUILayout.Label($"[{statusText}]", statusStyle);
            GUI.color = Color.white;
            
            EditorGUILayout.EndHorizontal();
            
            // 显示提示信息
            EditorGUILayout.HelpBox(
                "This toggle controls the 'TAP_DEBUG_ENABLE' scripting define symbol. " +
                "When enabled, the TapSDK debug server will be activated and the monitor will show server information.",
                MessageType.Info
            );
            
            EditorGUILayout.EndVertical();
        }

        private void DrawDebugDisabledMessage()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.Space(20);
            
            GUILayout.Label("调试开关未开启", warningStyle);
            
            EditorGUILayout.Space(10);
            
            EditorGUILayout.HelpBox(
                "请在上方勾选 'Enable TapSDK Debug' 开关来启用调试功能。\n\n" +
                "启用后将会：\n" +
                "• 添加 TAP_DEBUG_ENABLE 宏定义\n" +
                "• 激活 TapSDK 调试服务器\n" +
                "• 显示服务器状态和客户端连接信息\n"+
                "• 构建小游戏包体时需要关闭调试功能",
                MessageType.Warning
            );
            
            EditorGUILayout.Space(200);
            
            // // 构建调试客户端按钮
            // EditorGUILayout.BeginHorizontal();
            // GUILayout.FlexibleSpace();
            //
            // GUI.color = Color.cyan;
            //
            // if (GUILayout.Button("构建Tap小游戏调试客户端", GUILayout.Height(35), GUILayout.Width(200)))
            // {
            //     EditorApplication.delayCall += () => TapMiniGameDebugClientBuilder.BuildDebugClient();
            // }
            // GUI.color = Color.white;
            //
            // GUILayout.FlexibleSpace();
            // EditorGUILayout.EndHorizontal();
            //
            // EditorGUILayout.Space(5);
            //
            // EditorGUILayout.HelpBox(
            //     "此功能会构建独立的调试客户端，用于测试TapSDK功能。\n" +
            //     "构建过程不会影响您当前的项目构建配置。",
            //     MessageType.Info
            // );
            //
            // EditorGUILayout.Space(20);
            
            EditorGUILayout.EndVertical();
        }

        private void DrawControlPanel()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Control Panel", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            autoRefresh = EditorGUILayout.Toggle("Auto Refresh", autoRefresh);
            GUI.enabled = autoRefresh;
            refreshInterval = EditorGUILayout.Slider("Interval (s)", refreshInterval, 0.1f, 5.0f);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh Now", GUILayout.Width(100)))
            {
                RefreshServerInfo();
            }
            showClientDetails = EditorGUILayout.Toggle("Show Client Details", showClientDetails);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            autoGenerateQRCode = EditorGUILayout.Toggle("Auto Generate QR Code", autoGenerateQRCode);
            EditorGUILayout.HelpBox("When enabled, QR code will be automatically generated when server starts.", MessageType.Info);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
        }

        private void DrawServerStatus()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Server Status", EditorStyles.boldLabel);
            
            if (cachedServerInfo.isRunning)
            {
                // 服务器运行中
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Status:", GUILayout.Width(80));
                GUI.color = Color.green;
                GUILayout.Label("● RUNNING", statusStyle);
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Address:", GUILayout.Width(80));
                EditorGUILayout.SelectableLabel(cachedServerInfo.serverAddress ?? "N/A", statusStyle);
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Port:", GUILayout.Width(80));
                GUILayout.Label(cachedServerInfo.port.ToString(), statusStyle);
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Clients:", GUILayout.Width(80));
                GUILayout.Label(cachedServerInfo.clientCount.ToString(), statusStyle);
                EditorGUILayout.EndHorizontal();
                
                // 显示二维码状态
                if (showQRCode)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("QR Code:", GUILayout.Width(80));
                    GUI.color = Color.green;
                    GUILayout.Label("● Generated", statusStyle);
                    GUI.color = Color.white;
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                // 服务器未运行
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Status:", GUILayout.Width(80));
                GUI.color = Color.red;
                GUILayout.Label("● STOPPED", statusStyle);
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
                
                GUILayout.Label("Server is not running", statusStyle);
            }
            
            EditorGUILayout.EndVertical();
        }

        private void DrawClientList()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Connected Clients", EditorStyles.boldLabel);
#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
            if (cachedClients != null)
            {
                GUILayout.Label($"({cachedClients.Count})", statusStyle);
            }
#else
            GUILayout.Label("(0)", statusStyle);
#endif
            EditorGUILayout.EndHorizontal();
            
#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
            if (cachedClients != null && cachedClients.Count > 0)
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.MaxHeight(150));
                
                foreach (var client in cachedClients)
                {
                    DrawClientItem(client);
                }
                
                EditorGUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("No clients connected", statusStyle);
            }
#else
            GUILayout.Label("No clients connected", statusStyle);
#endif
            
            EditorGUILayout.EndVertical();
        }

#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
        private void DrawClientItem(NetworkServerModule.ClientInfo client)
        {
            EditorGUILayout.BeginVertical(clientItemStyle);
            
            EditorGUILayout.BeginHorizontal();
            GUI.color = Color.cyan;
            GUILayout.Label("●", GUILayout.Width(20));
            GUI.color = Color.white;
            GUILayout.Label($"Client: {client.clientId}", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            
            if (showClientDetails)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("IP:", GUILayout.Width(60));
                GUILayout.Label(client.clientIP ?? "N/A", statusStyle);
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Connected:", GUILayout.Width(60));
                var duration = System.DateTime.Now - client.connectTime;
                GUILayout.Label($"{duration.TotalSeconds:F0}s ago", statusStyle);
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndVertical();
        }
#endif

        private void DrawActionButtons()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Actions", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();

            // 根据服务器状态显示不同按钮
            if (cachedServerInfo.isRunning)
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Stop Server", GUILayout.Height(30)))
                {
                    StopServer();
                }
                GUI.color = Color.white;
            }
            else if (Application.isPlaying)
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Start Server", GUILayout.Height(30)))
                {
#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
                    var serverModule = NetworkServerModule.Instance;
                    if (serverModule != null)
                    {
                        serverModule.StartServer();
                        RefreshServerInfo();
                    }
#endif
                }
                GUI.color = Color.white;
            }
            else
            {
                GUI.enabled = false;
                GUILayout.Button("Server Stopped (Enter Play Mode)", GUILayout.Height(30));
                GUI.enabled = true;
            }
            
            if (GUILayout.Button("Copy Address", GUILayout.Height(30)))
            {
                CopyServerAddress();
            }
            
            if (GUILayout.Button("Generate QR Code", GUILayout.Height(30)))
            {
                GenerateQRCode();
            }
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void DrawLocalDebugTools()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("本地调试工具", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            
            // 下载状态和进度
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("下载状态", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(downloadStatus);
            if (isDownloading)
            {
                EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(false, 20), 
                    downloadProgress, $"{(downloadProgress * 100):F1}%");
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);

            // 服务器状态
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("本地小游戏静态服务器：", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"状态: {(isLocalServerRunning ? "运行中" : "已停止")}");
            if (isLocalServerRunning)
            {
                EditorGUILayout.LabelField($"地址: {localServerUrl}");
                EditorGUILayout.LabelField($"请求数: {localRequestCount}");
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);

            // 操作按钮
            if (!isDownloading && !isLocalServerRunning)
            {
                if (Application.isPlaying)
                {
                    if (GUILayout.Button("开始下载并启动服务器"))
                    {
                        _ = StartDownload();
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("请先进入Play模式以启动服务器", MessageType.Info);
                }
            }

            // 二维码显示
            if (showLocalQRCode && localQRCodeTexture != null)
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("调试二维码", EditorStyles.boldLabel);
                
                // 居中显示二维码
                float size = 200;
                float x = (EditorGUIUtility.currentViewWidth - size) * 0.5f;
                EditorGUI.DrawPreviewTexture(new Rect(x, EditorGUILayout.GetControlRect().y, size, size), 
                    localQRCodeTexture);
                
                GUILayout.Space(size + 10);

                // URL显示和复制
                if (panelDebugEnabled)
                {
                    EditorGUILayout.SelectableLabel(localDebugUrl, EditorStyles.textField, 
                        GUILayout.Height(40));
                
                    if (GUILayout.Button("复制URL"))
                    {
                        EditorGUIUtility.systemCopyBuffer = localDebugUrl;
                    }
                }
                EditorGUILayout.EndVertical();
            }
            
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region 调试开关控制
        private void CheckDebugEnabled()
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            isDebugEnabled = defines.Contains(TAP_DEBUG_DEFINE);
        }

        private void SetDebugEnabled(bool enabled)
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            
            List<string> definesList = defines.Split(';').ToList();
            
            if (enabled)
            {
                if (!definesList.Contains(TAP_DEBUG_DEFINE))
                {
                    definesList.Add(TAP_DEBUG_DEFINE);
                }
            }
            else
            {
                definesList.Remove(TAP_DEBUG_DEFINE);
            }
            
            // 移除空字符串
            definesList.RemoveAll(string.IsNullOrEmpty);
            
            string newDefines = string.Join(";", definesList);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, newDefines);
            
            isDebugEnabled = enabled;
            
            Debug.Log($"TapSDK Debug {(enabled ? "Enabled" : "Disabled")}. Scripting defines updated: {newDefines}");
            
            // 如果禁用调试，清空缓存的服务器信息
            if (!enabled)
            {
                cachedServerInfo = new ServerInfo
                {
                    isRunning = false,
                    serverAddress = null,
                    port = 0,
                    clientCount = 0,
                    status = "Debug Disabled"
                };
                
                // 重置之前的服务器状态
                previousServerRunning = false;
                
                // 隐藏二维码
                showQRCode = false;
                if (qrCodeTexture != null)
                {
                    DestroyImmediate(qrCodeTexture);
                    qrCodeTexture = null;
                }
                
#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
                cachedClients = new List<NetworkServerModule.ClientInfo>();
#endif
            }
        }
        #endregion

        #region 数据刷新
        private void RefreshServerInfo()
        {
            if (!isDebugEnabled)
                return;
            
            if(!Application.isPlaying) return;
                
            lastRefreshTime = EditorApplication.timeSinceStartup;
            
#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
            // 获取服务器实例
            var serverModule = NetworkServerModule.Instance;
            
            if (serverModule != null)
            {
                // 更新服务器信息
                cachedServerInfo = new ServerInfo
                {
                    isRunning = serverModule.IsRunning,
                    serverAddress = serverModule.ServerAddress,
                    port = GetServerPort(serverModule),
                    clientCount = serverModule.ConnectedClientCount,
                    status = serverModule.IsRunning ? "Running" : "Stopped"
                };
                
                // 检测服务器状态变化 - 如果服务器从停止变为运行，自动生成二维码
                if (cachedServerInfo.isRunning && !previousServerRunning && autoGenerateQRCode)
                {
                    AutoGenerateQRCode();
                }
                
                // 更新之前的服务器状态
                previousServerRunning = cachedServerInfo.isRunning;
                
                // 更新客户端列表
                cachedClients = serverModule.GetConnectedClients();
            }
            else
            {
                // 服务器未初始化
                cachedServerInfo = new ServerInfo
                {
                    isRunning = false,
                    serverAddress = null,
                    port = 0,
                    clientCount = 0,
                    status = "Not Initialized"
                };
                
                // 重置之前的服务器状态
                previousServerRunning = false;
                
                cachedClients = new List<NetworkServerModule.ClientInfo>();
            }
#else
            // 调试未启用时的默认状态
            cachedServerInfo = new ServerInfo
            {
                isRunning = false,
                serverAddress = null,
                port = 0,
                clientCount = 0,
                status = "Debug Disabled"
            };
            
            // 重置之前的服务器状态
            previousServerRunning = false;
#endif
        }

#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
        private int GetServerPort(NetworkServerModule serverModule)
        {
            // 通过反射获取私有字段serverPort
            var field = typeof(NetworkServerModule).GetField("serverPort", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (field != null)
            {
                return (int)field.GetValue(serverModule);
            }
            
            return 8081; // 默认端口
        }
#endif
        #endregion

        #region 操作方法
        private void StopServer()
        {
#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
            var serverModule = NetworkServerModule.Instance;
            if (serverModule != null)
            {
                serverModule.StopServer();
                RefreshServerInfo();
            }
#endif
        }

        private void CopyServerAddress()
        {
            if (!string.IsNullOrEmpty(cachedServerInfo.serverAddress))
            {
                EditorGUIUtility.systemCopyBuffer = cachedServerInfo.serverAddress;
                Debug.Log($"Server address copied to clipboard: {cachedServerInfo.serverAddress}");
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Server address is not available", "OK");
            }
        }

        private void GenerateQRCode()
        {
            if (!string.IsNullOrEmpty(cachedServerInfo.serverAddress))
            {
                try
                {
                    // 清理之前的纹理
                    if (qrCodeTexture != null)
                    {
                        DestroyImmediate(qrCodeTexture);
                    }
                    
                    // 生成新的二维码
                    qrCodeTexture = ZXingQRCodeGenerator.GenerateQRCode(cachedServerInfo.serverAddress, 200, 200);
                    showQRCode = true;
                    
                    Debug.Log($"QR Code generated for server address: {cachedServerInfo.serverAddress}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to generate QR code: {ex.Message}");
                    EditorUtility.DisplayDialog("Error", $"Failed to generate QR code: {ex.Message}", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Server address is not available", "OK");
            }
        }

        private void AutoGenerateQRCode()
        {
            if (!string.IsNullOrEmpty(cachedServerInfo.serverAddress))
            {
                try
                {
                    // 清理之前的纹理
                    if (qrCodeTexture != null)
                    {
                        DestroyImmediate(qrCodeTexture);
                    }
                    
                    // 生成新的二维码
                    qrCodeTexture = ZXingQRCodeGenerator.GenerateQRCode(cachedServerInfo.serverAddress, 200, 200);
                    showQRCode = true;
                    
                    Debug.Log($"[Auto] QR Code automatically generated for server address: {cachedServerInfo.serverAddress}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[Auto] Failed to generate QR code: {ex.Message}");
                    // 自动生成失败时不显示弹窗，只记录错误日志
                }
            }
            else
            {
                Debug.LogWarning("[Auto] Cannot generate QR code: Server address is not available");
            }
        }
        
        private void SaveQRCode()
        {
            if (qrCodeTexture != null)
            {
                try
                {
                    string path = EditorUtility.SaveFilePanel("Save QR Code", "", "TapSDK_QRCode", "png");
                    if (!string.IsNullOrEmpty(path))
                    {
                        var bytes = qrCodeTexture.EncodeToPNG();
                        System.IO.File.WriteAllBytes(path, bytes);
                        Debug.Log($"QR Code saved to: {path}");
                        EditorUtility.DisplayDialog("Success", $"QR Code saved to:\n{path}", "OK");
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to save QR code: {ex.Message}");
                    EditorUtility.DisplayDialog("Error", $"Failed to save QR code: {ex.Message}", "OK");
                }
            }
        }
        #endregion
    }
}
#endif