#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System;
using System.Collections;
using System.Threading;
using System.Net.Sockets;
using System.Linq;
using UnityEngine.Networking;

namespace TapTapMiniGame.Editor
{
    public partial class TapSDKServerWindow : EditorWindow
    {
        #region 常量定义
        /// <summary>
        /// 小游戏包的CDN下载地址
        /// </summary>
        private const string DOWNLOAD_URL = "https://app-res.tapimg.com/file/20ab3af9b7cf636fb1a1b5235a8b4a53.zip";

        /// <summary>
        /// 本地存储的hash值的EditorPrefs键名
        /// </summary>
        private const string LOCAL_HASH_KEY = "TapSDK_LocalServer_Hash";

        /// <summary>
        /// 本地调试文件存储目录
        /// </summary>
        private const string DEBUG_DIR = "Library/TapMiniGameDebug";

        /// <summary>
        /// 服务器优先尝试的端口列表
        /// 使用动态端口范围（49152-65535）避免与系统端口和注册端口冲突
        /// </summary>
        private static readonly int[] PREFERRED_PORTS = { 
            50080, 50081, 50082, 50083, 50084,  // 50080-50084
            51080, 51081, 51082, 51083, 51084,  // 51080-51084
            52080, 52081, 52082, 52083, 52084   // 52080-52084
        };

        /// <summary>
        /// 默认端口号
        /// </summary>
        private const int DEFAULT_PORT = 50080;
        #endregion

        #region 私有变量
        /// <summary>
        /// 是否正在下载中
        /// </summary>
        private bool isDownloading = false;

        /// <summary>
        /// 下载进度（0-1）
        /// </summary>
        private float downloadProgress = 0f;

        /// <summary>
        /// 下载状态描述文本
        /// </summary>
        private string downloadStatus = "";

        /// <summary>
        /// 当前下载请求对象
        /// </summary>
        private UnityWebRequest currentDownload;

        /// <summary>
        /// 本地服务器是否正在运行
        /// </summary>
        private bool isLocalServerRunning = false;

        /// <summary>
        /// 本地服务器使用的端口号
        /// </summary>
        private int localServerPort = DEFAULT_PORT;

        /// <summary>
        /// 本地服务器的完整URL地址
        /// </summary>
        private string localServerUrl = "";

        /// <summary>
        /// 本地服务器的取消令牌源，用于优雅关闭服务器
        /// </summary>
        private CancellationTokenSource localServerCancellation;

        /// <summary>
        /// TCP监听器实例，处理HTTP请求
        /// </summary>
        private TcpListener localTcpListener;

        /// <summary>
        /// 服务器处理的请求总数
        /// </summary>
        private int localRequestCount = 0;

        /// <summary>
        /// 调试URL，用于生成二维码
        /// </summary>
        private string localDebugUrl = "";

        /// <summary>
        /// 二维码纹理对象
        /// </summary>
        private Texture2D localQRCodeTexture;

        /// <summary>
        /// 是否显示二维码
        /// </summary>
        private bool showLocalQRCode = false;

        /// <summary>
        /// 本地IP地址
        /// </summary>
        private string localIP;

        /// <summary>
        /// 游戏配置信息
        /// </summary>
        private GameInfo gameInfo;

        /// <summary>
        /// 静态服务器根目录的完整路径
        /// </summary>
        private string serverRootPath;

        /// <summary>
        /// 默认首页
        /// </summary>
        private const string DEFAULT_PAGE = "index.html";
        
        /// <summary>
        /// 用户构建配置列表
        /// </summary>
        private List<BuildProfileInfo> userBuildProfiles = new List<BuildProfileInfo>();
        
        /// <summary>
        /// 当前选中的构建配置索引
        /// </summary>
        private int selectedProfileIndex = 0;
        
        /// <summary>
        /// 当前选中的构建输出路径
        /// </summary>
        private string currentUserDstPath = "";
        #endregion

        #region 初始化
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                // 只有当窗口已经打开时才调用相关方法，避免自动创建窗口
                if (HasOpenInstances<TapSDKServerWindow>())
                {
                    var window = GetWindow<TapSDKServerWindow>();
                    window.OnLocalServerEnable();
                }
            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                // 只有当窗口已经打开时才调用相关方法，避免自动创建窗口
                if (HasOpenInstances<TapSDKServerWindow>())
                {
                    var window = GetWindow<TapSDKServerWindow>();
                    window.OnLocalServerDisable();
                }
            }
        }

        private void OnLocalServerEnable()
        {
            // 在主线程中初始化根目录路径
            serverRootPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", DEBUG_DIR));
            Debug.Log($"Server root path: {serverRootPath}");
            
            GetLocalIPAddress();
            LoadUserBuildProfiles();
            CheckLocalFiles();
        }

        private void OnLocalServerDisable()
        {
            StopLocalServer();
            if (currentDownload != null)
            {
                currentDownload.Abort();
                currentDownload.Dispose();
                currentDownload = null;
            }
        }

        private void GetLocalIPAddress()
        {
            // 使用TapSDKApiUtil中的优化IP获取方法
            localIP = TapSDKApiUtil.GetLocalIPAddress();
        }
        
        /// <summary>
        /// 加载用户的构建配置
        /// </summary>
        private void LoadUserBuildProfiles()
        {
            try
            {
                // 使用共享工具类加载所有 TapTap 构建配置
                userBuildProfiles = TapMiniGameConfigHelper.LoadAllBuildProfiles();
                
                if (userBuildProfiles.Count > 0)
                {
                    // 默认选择第一个配置
                    selectedProfileIndex = 0;
                    currentUserDstPath = userBuildProfiles[0].dstPath;
                    LoadUserGameJson();
                }
                else
                {
                    Debug.LogError("No user build profiles found. Please build your game first!");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading user build profiles: {e.Message}");
            }
        }
        
        /// <summary>
        /// 从用户的构建输出加载 game.json
        /// </summary>
        private void LoadUserGameJson()
        {
            if (string.IsNullOrEmpty(currentUserDstPath))
            {
                Debug.LogError("Current DST path is empty. Build configuration is invalid!");
                return;
            }
            
            try
            {
                // 使用共享工具类从 DST 路径加载游戏信息
                gameInfo = TapMiniGameConfigHelper.LoadGameInfoFromDst(currentUserDstPath);
                
                if (gameInfo != null && !string.IsNullOrEmpty(gameInfo.appId))
                {
                    string profileName = userBuildProfiles.Count > 0 && selectedProfileIndex < userBuildProfiles.Count ? 
                        userBuildProfiles[selectedProfileIndex].profileName : "Unknown";
                    Debug.Log($"User game info loaded from [{profileName}]: appId={gameInfo.appId}, productName={gameInfo.productName}");
                }
                else
                {
                    Debug.LogError($"Failed to load game info from: {currentUserDstPath}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading user game info: {e.Message}");
            }
        }
        
        /// <summary>
        /// 当用户切换构建配置时调用
        /// </summary>
        private void OnUserProfileSelectionChanged(int newIndex)
        {
            if (newIndex >= 0 && newIndex < userBuildProfiles.Count && newIndex != selectedProfileIndex)
            {
                selectedProfileIndex = newIndex;
                currentUserDstPath = userBuildProfiles[selectedProfileIndex].dstPath;
                
                // 重新加载游戏信息
                LoadUserGameJson();
                
                // 如果二维码已生成，需要重新生成
                if (showLocalQRCode)
                {
                    GenerateLocalQRCode();
                }
            }
        }

        private void LoadGameJson()
        {
            try
            {
                // 从minigame目录读取game.json（示例游戏，已废弃）
                string localPath = Path.Combine(Application.dataPath, "..", DEBUG_DIR);
                string gameJsonPath = Path.Combine(localPath, "minigame", "game.json");
                
                Debug.Log($"Loading game.json from: {gameJsonPath}");
                
                if (File.Exists(gameJsonPath))
                {
                    string jsonContent = File.ReadAllText(gameJsonPath);
                    gameInfo = JsonUtility.FromJson<GameInfo>(jsonContent);
                    Debug.Log($"Game info loaded: {gameInfo.appId}");
                }
                else
                {
                    Debug.Log($"game.json not found at: {gameJsonPath}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading game info: {e.Message}");
            }
        }
        #endregion

        #region 本地文件管理
        private void CheckLocalFiles()
        {
            // 检查本地目录是否存在
            string localPath = Path.Combine(Application.dataPath, "..", DEBUG_DIR);
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }

            // 检查minigame/game.json是否存在
            string gameJsonPath = Path.Combine(localPath, "minigame", "game.json");
            bool hasLocalFiles = File.Exists(gameJsonPath);

            // 获取当前URL中的hash
            string currentHash = ExtractHashFromUrl(DOWNLOAD_URL);
            string savedHash = EditorPrefs.GetString(LOCAL_HASH_KEY, "");

            if (!hasLocalFiles || currentHash != savedHash)
            {
                // 需要下载
                downloadStatus = hasLocalFiles ? "需要更新" : "需要下载";
            }
            else
            {
                downloadStatus = "本地文件已是最新";
                // 如果示例游戏已存在且用户有有效的 gameInfo，启动服务器
                if (gameInfo != null && !string.IsNullOrEmpty(gameInfo.appId))
                {
                    Debug.Log($"示例游戏已存在，使用用户 appId: {gameInfo.appId} 启动服务器");
                    StartLocalServer();
                }
                else
                {
                    Debug.LogError("无法启动服务器：未找到有效的 gameInfo。请先构建你的游戏或刷新配置。");
                }
            }
        }

        private string ExtractHashFromUrl(string url)
        {
            // 从URL中提取hash值
            int lastSlash = url.LastIndexOf('/');
            if (lastSlash >= 0)
            {
                string fileName = url.Substring(lastSlash + 1);
                int dotIndex = fileName.LastIndexOf('.');
                if (dotIndex >= 0)
                {
                    return fileName.Substring(0, dotIndex);
                }
            }
            return "";
        }
        #endregion

        #region 下载管理
        private async Task StartDownload()
        {
            if (isDownloading) return;

            try
            {
                isDownloading = true;
                downloadProgress = 0f;
                downloadStatus = "开始下载...";

                // 创建下载请求
                currentDownload = UnityWebRequest.Get(DOWNLOAD_URL);
                var operation = currentDownload.SendWebRequest();

                // 监控下载进度
                while (!operation.isDone)
                {
                    downloadProgress = operation.progress;
                    downloadStatus = $"下载中... {(downloadProgress * 100):F1}%";
                    await Task.Delay(100);
                }

                if (currentDownload.result == UnityWebRequest.Result.Success)
                {
                    // 确保目录存在
                    string localPath = Path.Combine(Application.dataPath, "..", DEBUG_DIR);
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                        Debug.Log($"Created directory: {localPath}");
                    }

                    // 下载成功，保存文件
                    string zipPath = Path.Combine(localPath, "game.zip");
                    Debug.Log($"Saving file to: {zipPath}");
                    File.WriteAllBytes(zipPath, currentDownload.downloadHandler.data);

                    // 解压文件到minigame子目录
                    string minigamePath = Path.Combine(localPath, "minigame");
                    await UnzipFile(zipPath, minigamePath);

                    // 更新hash记录
                    string currentHash = ExtractHashFromUrl(DOWNLOAD_URL);
                    EditorPrefs.SetString(LOCAL_HASH_KEY, currentHash);

                    downloadStatus = "下载完成";

                    // 下载完成后启动本地服务器
                    // 注意：使用用户的 gameInfo.appId（已从 LoadUserBuildProfiles 加载）
                    // 但 package URL 指向本地下载的示例游戏包
                    if (gameInfo != null && !string.IsNullOrEmpty(gameInfo.appId))
                    {
                        Debug.Log($"示例游戏下载完成，使用用户 appId: {gameInfo.appId} 生成调试链接");
                        StartLocalServer();
                    }
                    else
                    {
                        Debug.LogError("无法生成调试链接：未找到有效的 gameInfo。请先构建你的游戏或刷新配置。");
                    }
                }
                else
                {
                    downloadStatus = $"下载失败: {currentDownload.error}";
                    Debug.LogError($"Download failed: {currentDownload.error}");
                }
            }
            catch (Exception e)
            {
                downloadStatus = $"下载出错: {e.Message}";
                Debug.LogError($"Download error: {e}");
            }
            finally
            {
                isDownloading = false;
                if (currentDownload != null)
                {
                    currentDownload.Dispose();
                    currentDownload = null;
                }
            }
        }

        private async Task UnzipFile(string zipPath, string targetPath)
        {
            await Task.Run(() =>
            {
                try
                {
                    Debug.Log($"Unzipping {zipPath} to {targetPath}");
                    
                    // 如果目标目录存在，先删除
                    if (Directory.Exists(targetPath))
                    {
                        Directory.Delete(targetPath, true);
                        Debug.Log($"Deleted existing directory: {targetPath}");
                    }
                    
                    // 创建目标目录
                    Directory.CreateDirectory(targetPath);
                    
                    // 解压文件
                    System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, targetPath);
                    
                    Debug.Log("Unzip completed successfully");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Unzip error: {e}");
                    throw;
                }
            });
        }
        #endregion

        #region 服务器管理
        private void StartLocalServer()
        {
            if (isLocalServerRunning) return;

            try
            {
                // 查找可用端口
                localServerPort = FindAvailableLocalPort();
                localServerUrl = $"http://{localIP}:{localServerPort}/";

                // 启动TCP监听器
                localTcpListener = new TcpListener(IPAddress.Any, localServerPort);
                localTcpListener.Start();

                isLocalServerRunning = true;
                localServerCancellation = new CancellationTokenSource();

                // 在后台线程中监听请求
                _ = Task.Run(() => ListenForLocalRequests(localServerCancellation.Token));

                // 生成二维码
                WaitForServerAndGenerateQRCode();


                Debug.Log($"Local server started on {localIP}:{localServerPort}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to start server: {e.Message}");
                StopLocalServer();
            }
        }

        private void StopLocalServer()
        {
            if (!isLocalServerRunning) return;

            try
            {
                localServerCancellation?.Cancel();
                localTcpListener?.Stop();
                localTcpListener = null;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error stopping server: {e.Message}");
            }
            finally
            {
                isLocalServerRunning = false;
                showLocalQRCode = false;
            }
        }

        private int FindAvailableLocalPort()
        {
            foreach (int port in PREFERRED_PORTS)
            {
                try
                {
                    var testListener = new TcpListener(IPAddress.Any, port);
                    testListener.Start();
                    testListener.Stop();
                    return port;
                }
                catch
                {
                    continue;
                }
            }
            throw new Exception("No available ports found");
        }

        private async Task ListenForLocalRequests(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && localTcpListener != null)
            {
                try
                {
                    var client = await localTcpListener.AcceptTcpClientAsync();
                    _ = HandleLocalRequest(client);
                }
                catch (Exception) when (cancellationToken.IsCancellationRequested)
                {
                    // 正常取消，不需要处理
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error accepting connection: {e.Message}");
                }
            }
        }

        #region 服务器请求处理
        private async Task HandleLocalRequest(TcpClient client)
        {
            try
            {
                using (client)
                {
                    // 设置超时
                    client.ReceiveTimeout = 5000;
                    client.SendTimeout = 30000;

                    using (var stream = client.GetStream())
                    {
                        // 读取请求
                        byte[] buffer = new byte[4096];
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        Debug.Log($"Full request:\n{request}");

                        // 解析请求行
                        string[] lines = request.Split(new[] { "\r\n" }, StringSplitOptions.None);
                        string[] requestParts = lines[0].Split(' ');
                        if (requestParts.Length < 2) return;

                        string method = requestParts[0];
                        string path = requestParts[1];

                        Debug.Log($"Method: {method}, Path: {path}");

                        // 处理请求
                        if (method == "OPTIONS")
                        {
                            await ServeOptions(stream);
                            return;
                        }

                        // 检查请求路径，支持 /game.zip 和 /download/game.zip
                        bool isGameZipRequest = path == "/game.zip" || path == "/download/game.zip";
                        
                        if (method == "GET" && isGameZipRequest)
                        {
                            await ServeGameZip(stream);
                        }
                        else
                        {
                            await ServeNotFound(stream, path);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error handling request: {e.Message}");
            }
        }

        private async Task ServeGameZip(NetworkStream stream)
        {
            string zipPath = Path.Combine(serverRootPath, "game.zip");
            Debug.Log($"Attempting to serve: {zipPath}");

            if (File.Exists(zipPath))
            {
                try
                {
                    byte[] content = File.ReadAllBytes(zipPath);
                    
                    // 构建HTTP响应
                    StringBuilder responseBuilder = new StringBuilder();
                    responseBuilder.AppendLine("HTTP/1.1 200 OK");
                    responseBuilder.AppendLine("Content-Type: application/zip");
                    responseBuilder.AppendLine($"Content-Length: {content.Length}");
                    responseBuilder.AppendLine("Accept-Ranges: bytes");
                    responseBuilder.AppendLine("Cache-Control: no-cache");
                    
                    // 添加CORS跨域支持
                    responseBuilder.AppendLine("Access-Control-Allow-Origin: *");
                    responseBuilder.AppendLine("Access-Control-Allow-Methods: GET, POST, OPTIONS");
                    responseBuilder.AppendLine("Access-Control-Allow-Headers: *");
                    responseBuilder.AppendLine("Access-Control-Max-Age: 86400");
                    
                    responseBuilder.AppendLine("Connection: close");
                    responseBuilder.AppendLine(); // 空行分隔头和体

                    byte[] headerBytes = Encoding.UTF8.GetBytes(responseBuilder.ToString());
                    
                    // 发送响应头
                    await stream.WriteAsync(headerBytes, 0, headerBytes.Length);
                    await stream.FlushAsync();
                    
                    // 发送文件内容
                    await stream.WriteAsync(content, 0, content.Length);
                    await stream.FlushAsync();
                    
                    localRequestCount++;
                    Debug.Log($"Successfully served game.zip ({content.Length} bytes)");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error serving game.zip: {e.Message}");
                    await ServeNotFound(stream, "game.zip");
                }
            }
            else
            {
                Debug.LogError($"game.zip not found at: {zipPath}");
                await ServeNotFound(stream, "game.zip");
            }
        }

        private async Task ServeNotFound(NetworkStream stream, string path)
        {
            string errorMessage = $"File not found: {path}";
            byte[] errorBytes = Encoding.UTF8.GetBytes(errorMessage);
            
            StringBuilder responseBuilder = new StringBuilder();
            responseBuilder.AppendLine("HTTP/1.1 404 Not Found");
            responseBuilder.AppendLine("Content-Type: text/plain; charset=utf-8");
            responseBuilder.AppendLine($"Content-Length: {errorBytes.Length}");
            responseBuilder.AppendLine("Access-Control-Allow-Origin: *");
            responseBuilder.AppendLine("Access-Control-Allow-Methods: GET, POST, OPTIONS");
            responseBuilder.AppendLine("Access-Control-Allow-Headers: *");
            responseBuilder.AppendLine("Connection: close");
            responseBuilder.AppendLine();
            responseBuilder.Append(errorMessage);

            byte[] responseBytes = Encoding.UTF8.GetBytes(responseBuilder.ToString());
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
            await stream.FlushAsync();
            
            Debug.Log($"404 Not Found: {path}");
        }

        private async Task ServeOptions(NetworkStream stream)
        {
            StringBuilder responseBuilder = new StringBuilder();
            responseBuilder.AppendLine("HTTP/1.1 200 OK");
            responseBuilder.AppendLine("Content-Length: 0");
            
            // CORS预检响应头
            responseBuilder.AppendLine("Access-Control-Allow-Origin: *");
            responseBuilder.AppendLine("Access-Control-Allow-Methods: GET, POST, OPTIONS, HEAD");
            responseBuilder.AppendLine("Access-Control-Allow-Headers: *");
            responseBuilder.AppendLine("Access-Control-Max-Age: 86400");
            responseBuilder.AppendLine("Access-Control-Allow-Credentials: false");
            
            responseBuilder.AppendLine("Connection: close");
            responseBuilder.AppendLine(); // 空行结束头部
            
            byte[] headerBytes = Encoding.UTF8.GetBytes(responseBuilder.ToString());
            await stream.WriteAsync(headerBytes, 0, headerBytes.Length);
            await stream.FlushAsync();
            
            Debug.Log("✓ Served OPTIONS preflight response");
        }
#endregion
        #endregion

        #region 二维码生成
        
        private async void WaitForServerAndGenerateQRCode()
        {
            // 0.1秒轮询检查cachedServerInfo.isRunning=true时才执行GenerateLocalQRCode
            while (!cachedServerInfo.isRunning)
            {
                await Task.Delay(100); // 等待0.1秒
            }
            GenerateLocalQRCode();
        }
        
        private void GenerateLocalQRCode()
        {
            if (string.IsNullOrEmpty(localServerUrl)) return;
            if (gameInfo == null || string.IsNullOrEmpty(gameInfo.appId))
            {
                Debug.LogError("Cannot generate QR code: missing app ID");
                return;
            }

            try
            {
                // 构建调试URL
                var options = new Dictionary<string, string>
                {
                    // {"app_id", gameInfo.appId},
                    {"clean_profile", "true"}
                    // {"referer", "qrcode"},
                    // {"test_type", "offline"},
                    // {"mute_start", "false"}
                };

                // 添加scene_param参数 - 使用本地服务器地址
                if (!string.IsNullOrEmpty(cachedServerInfo.serverAddress))
                {
                    // 使用本地服务器的地址作为 scene_param
                    options.Add("scene_param", cachedServerInfo.serverAddress);
                    options.Add("dev_params", cachedServerInfo.serverAddress);
                    Debug.Log($"Added scene_param: {cachedServerInfo.serverAddress}");
                }
                else
                {
                    Debug.Log("Local server URL is empty, skipping scene_param");
                }

                localDebugUrl = GenerateDebugUrl(
                    gameInfo.appId,
                    $"{localServerUrl}game.zip",
                    gameInfo.productName,
                    options
                );

                // 生成二维码
                localQRCodeTexture = ZXingQRCodeGenerator.GenerateQRCode(localDebugUrl, 256, 256);
                showLocalQRCode = true;

                Debug.Log($"Generated QR Code URL: {localDebugUrl}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to generate QR code: {e.Message}");
            }
        }

        private string GenerateDebugUrl(string miniGameId, string packageUrl, string gameName, Dictionary<string, string> options = null)
        {
            if (string.IsNullOrEmpty(miniGameId))
            {
                throw new ArgumentException("miniGameId is required");
            }

            var urlBuilder = new StringBuilder("taptap://taptap.com/miniapp?");
            
            // 必须参数
            urlBuilder.Append($"id={miniGameId}");
            
            // 添加type和package（调试模式必须）
            urlBuilder.Append("&type=debug");
            urlBuilder.Append($"&package={UnityWebRequest.EscapeURL(packageUrl)}");
            
            // 游戏名称（可选）
            // if (!string.IsNullOrEmpty(gameName))
            // {
            //     urlBuilder.Append($"&game_name={UnityWebRequest.EscapeURL(gameName)}");
            // }
            
            // 添加其他可选参数
            if (options != null)
            {
                foreach (var option in options)
                {
                    if (!string.IsNullOrEmpty(option.Value))
                    {
                        urlBuilder.Append($"&{option.Key}={UnityWebRequest.EscapeURL(option.Value)}");
                    }
                }
            }
            
            return urlBuilder.ToString();
        }
        #endregion

        #region GUI绘制
        private void DrawLocalServerGUI()
        {
            EditorGUILayout.Space(10);
            
            // 用户构建配置选择
            if (userBuildProfiles != null && userBuildProfiles.Count > 0)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("用户游戏构建配置", EditorStyles.boldLabel);
                
                // 确保索引有效性
                if (selectedProfileIndex < 0 || selectedProfileIndex >= userBuildProfiles.Count)
                {
                    selectedProfileIndex = 0;
                }
                
                // 创建配置文件名称数组
                string[] profileNames = userBuildProfiles.Select(p => $"{p.profileName} ({p.hostName})").ToArray();
                
                // 下拉框选择配置
                EditorGUI.BeginChangeCheck();
                int newSelectedIndex = EditorGUILayout.Popup("当前配置:", selectedProfileIndex, profileNames);
                if (EditorGUI.EndChangeCheck() && newSelectedIndex != selectedProfileIndex)
                {
                    OnUserProfileSelectionChanged(newSelectedIndex);
                }
                
                // 显示当前配置详情
                if (selectedProfileIndex >= 0 && selectedProfileIndex < userBuildProfiles.Count)
                {
                    var currentProfile = userBuildProfiles[selectedProfileIndex];
                    EditorGUILayout.LabelField("构建路径:", currentProfile.dstPath, EditorStyles.wordWrappedLabel);
                }
                
                // 显示游戏信息
                if (gameInfo != null && !string.IsNullOrEmpty(gameInfo.appId))
                {
                    EditorGUILayout.LabelField("App ID:", gameInfo.appId);
                    EditorGUILayout.LabelField("游戏名称:", gameInfo.productName ?? "N/A");
                }
                else
                {
                    EditorGUILayout.HelpBox("未找到游戏信息，请先构建游戏", MessageType.Warning);
                }
                
                // 刷新按钮
                if (GUILayout.Button("刷新配置"))
                {
                    LoadUserBuildProfiles();
                }
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(10);
            }
            else
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.HelpBox("未找到用户构建配置，请先构建你的 TapTap 小游戏", MessageType.Warning);
                if (GUILayout.Button("刷新配置"))
                {
                    LoadUserBuildProfiles();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(10);
            }

            // 下载状态和进度（示例游戏，可选）
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("示例游戏下载", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(downloadStatus);
            if (isDownloading)
            {
                EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(false, 20), 
                    downloadProgress, $"{(downloadProgress * 100):F1}%");
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10);

            // 服务器状态
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("服务器状态", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"状态: {(isLocalServerRunning ? "运行中" : "已停止")}");
            if (isLocalServerRunning)
            {
                EditorGUILayout.LabelField($"地址: {localServerUrl}");
                EditorGUILayout.LabelField($"请求数: {localRequestCount}");
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10);

            // 操作按钮
            if (!isDownloading && !isLocalServerRunning)
            {
                if (GUILayout.Button("开始下载并启动服务器"))
                {
                    _ = StartDownload();
                }
            }
            else if (isLocalServerRunning)
            {
                if (GUILayout.Button("停止服务器"))
                {
                    StopLocalServer();
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
                EditorGUILayout.SelectableLabel(localDebugUrl, EditorStyles.textField, 
                    GUILayout.Height(40));
                
                if (GUILayout.Button("复制URL"))
                {
                    EditorGUIUtility.systemCopyBuffer = localDebugUrl;
                }
                
                EditorGUILayout.EndVertical();
            }
        }
        #endregion
    }
}
#endif 