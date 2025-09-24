#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE

using System;
using UnityEngine;
using System.IO;

namespace TapTapMiniGame
{
    /// <summary>
    /// TapSDK同步数据缓存管理器
    /// 用于在Editor环境下缓存来自真机的各种API数据
    /// </summary>
    public static class TapSyncCache 
    {
        // TapEnv相关缓存
        private static string _cachedUserDataPath = null;
        
        // SystemInfo相关缓存
        private static TapTapMiniGame.SystemInfo _cachedSystemInfo = null;
        
        // SystemSetting相关缓存
        private static SystemSetting _cachedSystemSetting = null;
        
        // WindowInfo相关缓存
        private static WindowInfo _cachedWindowInfo = null;
        
        // DeviceInfo相关缓存
        private static DeviceInfo _cachedDeviceInfo = null;
        
        // AppBaseInfo相关缓存
        private static AppBaseInfo _cachedAppBaseInfo = null;
        
        // AppAuthorizeSetting相关缓存
        private static AppAuthorizeSetting _cachedAppAuthorizeSetting = null;
        
        // BatteryInfo相关缓存
        private static GetBatteryInfoSyncResult _cachedBatteryInfo = null;
        
        /// <summary>
        /// 默认用户数据路径（兜底使用）
        /// </summary>
        private static readonly string DEFAULT_USER_DATA_PATH = 
            Path.Combine(Application.persistentDataPath, "TapMiniGameDebug");
        
        #region TapEnv相关方法
        
        /// <summary>
        /// 获取用户数据路径
        /// 优先返回真机缓存的路径，失败时返回默认路径
        /// </summary>
        public static string GetUserDataPath() 
        {
            if (string.IsNullOrEmpty(_cachedUserDataPath))
            {
                if (enableDebugLog)
                    Debug.LogWarning("[TapSyncCache] USER_DATA_PATH not ready, using default path");
                return DEFAULT_USER_DATA_PATH;
            }
            
            return _cachedUserDataPath;
        }
        
        /// <summary>
        /// 更新TapEnv缓存数据
        /// </summary>
        public static void UpdateCache(string userDataPath) 
        {
            if (string.IsNullOrEmpty(userDataPath))
            {
                Debug.LogWarning("[TapSyncCache] Attempted to update with null or empty userDataPath");
                return;
            }
            
            _cachedUserDataPath = userDataPath;
            
            if (enableDebugLog)
                Debug.Log($"[TapSyncCache] TapEnv数据已更新: USER_DATA_PATH = {_cachedUserDataPath}");
        }
        
        /// <summary>
        /// 更新SystemInfo缓存数据
        /// </summary>
        public static void UpdateSystemInfoCache(TapTapMiniGame.SystemInfo systemInfo)
        {
            if (systemInfo == null)
            {
                Debug.LogWarning("[TapSyncCache] Attempted to update with null systemInfo");
                return;
            }
            
            _cachedSystemInfo = systemInfo;
            
            if (enableDebugLog)
                Debug.Log($"[TapSyncCache] SystemInfo数据已更新: platform = {systemInfo.platform}, brand = {systemInfo.brand}");
        }
        
        #endregion
        
        #region SystemInfo相关方法
        
        /// <summary>
        /// 获取系统信息
        /// 优先返回真机缓存的信息，失败时返回默认信息
        /// </summary>
        public static TapTapMiniGame.SystemInfo GetSystemInfo() 
        {
            if (_cachedSystemInfo == null)
            {
                if (enableDebugLog)
                    Debug.LogWarning("[TapSyncCache] SystemInfo not ready, using default SystemInfo");
                
                // 返回默认的SystemInfo
                return new TapTapMiniGame.SystemInfo
                {
                    SDKVersion = "1.0.0",
                    brand = "Editor",
                    model = "Unity Editor",
                    pixelRatio = 1.0f,
                    screenHeight = 1080,
                    screenWidth = 1920,
                    windowHeight = 1080,
                    windowWidth = 1920,
                    statusBarHeight = 24,
                    system = "Windows",
                    platform = "windows",
                    version = "1.0.0"
                };
            }
            
            return _cachedSystemInfo;
        }
        
        #endregion
        
        #region SystemSetting相关方法
        
        /// <summary>
        /// 获取系统设置
        /// 优先返回真机缓存的信息，失败时返回默认信息
        /// </summary>
        public static SystemSetting GetSystemSetting() 
        {
            if (_cachedSystemSetting == null)
            {
                if (enableDebugLog)
                    Debug.LogWarning("[TapSyncCache] SystemSetting not ready, using default SystemSetting");
                
                // 返回默认的SystemSetting
                return new SystemSetting
                {
                    bluetoothEnabled = true,
                    deviceOrientation = "portrait",
                    locationEnabled = true,
                    wifiEnabled = true
                };
            }
            
            return _cachedSystemSetting;
        }
        
        /// <summary>
        /// 更新SystemSetting缓存数据
        /// </summary>
        public static void UpdateSystemSettingCache(SystemSetting systemSetting)
        {
            if (systemSetting == null)
            {
                Debug.LogWarning("[TapSyncCache] Attempted to update with null systemSetting");
                return;
            }
            
            _cachedSystemSetting = systemSetting;
            
            if (enableDebugLog)
                Debug.Log($"[TapSyncCache] SystemSetting数据已更新");
        }
        
        #endregion
        
        #region WindowInfo相关方法
        
        /// <summary>
        /// 获取窗口信息
        /// 优先返回真机缓存的信息，失败时返回默认信息
        /// </summary>
        public static WindowInfo GetWindowInfo() 
        {
            if (_cachedWindowInfo == null)
            {
                if (enableDebugLog)
                    Debug.LogWarning("[TapSyncCache] WindowInfo not ready, using default WindowInfo");
                
                // 返回默认的WindowInfo
                return new WindowInfo
                {
                    pixelRatio = 1.0f,
                    screenHeight = 1080,
                    screenWidth = 1920,
                    windowHeight = 1080,
                    windowWidth = 1920,
                    statusBarHeight = 24,
                    screenTop = 0,
                    safeArea = new SafeArea
                    {
                        bottom = 1080,
                        height = 1080,
                        left = 0,
                        right = 1920,
                        top = 0,
                        width = 1920
                    }
                };
            }
            
            return _cachedWindowInfo;
        }
        
        /// <summary>
        /// 更新WindowInfo缓存数据
        /// </summary>
        public static void UpdateWindowInfoCache(WindowInfo windowInfo)
        {
            if (windowInfo == null)
            {
                Debug.LogWarning("[TapSyncCache] Attempted to update with null windowInfo");
                return;
            }
            
            _cachedWindowInfo = windowInfo;
            
            if (enableDebugLog)
                Debug.Log($"[TapSyncCache] WindowInfo数据已更新");
        }
        
        #endregion
        
        #region DeviceInfo相关方法
        
        /// <summary>
        /// 获取设备信息
        /// 优先返回真机缓存的信息，失败时返回默认信息
        /// </summary>
        public static DeviceInfo GetDeviceInfo() 
        {
            if (_cachedDeviceInfo == null)
            {
                if (enableDebugLog)
                    Debug.LogWarning("[TapSyncCache] DeviceInfo not ready, using default DeviceInfo");
                
                // 返回默认的DeviceInfo
                return new DeviceInfo
                {
                    brand = "Editor",
                    model = "Unity Editor", 
                    platform = "windows",
                    system = "Windows"
                };
            }
            
            return _cachedDeviceInfo;
        }
        
        /// <summary>
        /// 更新DeviceInfo缓存数据
        /// </summary>
        public static void UpdateDeviceInfoCache(DeviceInfo deviceInfo)
        {
            if (deviceInfo == null)
            {
                Debug.LogWarning("[TapSyncCache] Attempted to update with null deviceInfo");
                return;
            }
            
            _cachedDeviceInfo = deviceInfo;
            
            if (enableDebugLog)
                Debug.Log($"[TapSyncCache] DeviceInfo数据已更新");
        }
        
        #endregion
        
        #region AppBaseInfo相关方法
        
        /// <summary>
        /// 获取应用基础信息
        /// 优先返回真机缓存的信息，失败时返回默认信息
        /// </summary>
        public static AppBaseInfo GetAppBaseInfo() 
        {
            if (_cachedAppBaseInfo == null)
            {
                if (enableDebugLog)
                    Debug.LogWarning("[TapSyncCache] AppBaseInfo not ready, using default AppBaseInfo");
                
                // 返回默认的AppBaseInfo
                return new AppBaseInfo
                {
                    SDKVersion = "1.0.0",
                    enableDebug = true,
                    language = "zh_CN",
                    version = "1.0.0"
                };
            }
            
            return _cachedAppBaseInfo;
        }
        
        /// <summary>
        /// 更新AppBaseInfo缓存数据
        /// </summary>
        public static void UpdateAppBaseInfoCache(AppBaseInfo appBaseInfo)
        {
            if (appBaseInfo == null)
            {
                Debug.LogWarning("[TapSyncCache] Attempted to update with null appBaseInfo");
                return;
            }
            
            _cachedAppBaseInfo = appBaseInfo;
            
            if (enableDebugLog)
                Debug.Log($"[TapSyncCache] AppBaseInfo数据已更新");
        }
        
        #endregion
        
        #region AppAuthorizeSetting相关方法
        
        /// <summary>
        /// 获取应用授权设置
        /// 优先返回真机缓存的信息，失败时返回默认信息
        /// </summary>
        public static AppAuthorizeSetting GetAppAuthorizeSetting() 
        {
            if (_cachedAppAuthorizeSetting == null)
            {
                if (enableDebugLog)
                    Debug.LogWarning("[TapSyncCache] AppAuthorizeSetting not ready, using default AppAuthorizeSetting");
                
                // 返回默认的AppAuthorizeSetting
                return new AppAuthorizeSetting
                {
                    albumAuthorized = "authorized",
                    cameraAuthorized = "authorized", 
                    locationAuthorized = "authorized",
                    microphoneAuthorized = "authorized"
                };
            }
            
            return _cachedAppAuthorizeSetting;
        }
        
        /// <summary>
        /// 更新AppAuthorizeSetting缓存数据
        /// </summary>
        public static void UpdateAppAuthorizeSettingCache(AppAuthorizeSetting appAuthorizeSetting)
        {
            if (appAuthorizeSetting == null)
            {
                Debug.LogWarning("[TapSyncCache] Attempted to update with null appAuthorizeSetting");
                return;
            }
            
            _cachedAppAuthorizeSetting = appAuthorizeSetting;
            
            if (enableDebugLog)
                Debug.Log($"[TapSyncCache] AppAuthorizeSetting数据已更新");
        }
        
        #endregion
        
        #region BatteryInfo相关方法
        
        /// <summary>
        /// 获取电池信息
        /// 优先返回真机缓存的信息，失败时返回默认信息
        /// </summary>
        public static GetBatteryInfoSyncResult GetBatteryInfo() 
        {
            if (_cachedBatteryInfo == null)
            {
                if (enableDebugLog)
                    Debug.LogWarning("[TapSyncCache] BatteryInfo not ready, using default BatteryInfo");
                
                // 返回默认的BatteryInfo
                return new GetBatteryInfoSyncResult
                {
                    isCharging = false,
                    level = 85
                };
            }
            
            return _cachedBatteryInfo;
        }
        
        /// <summary>
        /// 更新BatteryInfo缓存数据
        /// </summary>
        public static void UpdateBatteryInfoCache(GetBatteryInfoSyncResult batteryInfo)
        {
            if (batteryInfo == null)
            {
                Debug.LogWarning("[TapSyncCache] Attempted to update with null batteryInfo");
                return;
            }
            
            _cachedBatteryInfo = batteryInfo;
            
            if (enableDebugLog)
                Debug.Log($"[TapSyncCache] BatteryInfo数据已更新");
        }
        
        #endregion
        
        #region 通用缓存管理
        
        /// <summary>
        /// 重置所有缓存
        /// </summary>
        public static void ResetCache() 
        {
            _cachedUserDataPath = null;
            _cachedSystemInfo = null;
            _cachedSystemSetting = null;
            _cachedWindowInfo = null;
            _cachedDeviceInfo = null;
            _cachedAppBaseInfo = null;
            _cachedAppAuthorizeSetting = null;
            _cachedBatteryInfo = null;
            
            if (enableDebugLog)
                Debug.Log("[TapSyncCache] 所有缓存已重置");
        }
        
        #endregion
        
        /// <summary>
        /// 调试日志开关
        /// </summary>
        private static bool enableDebugLog => true;
    }
    
    /// <summary>
    /// 增强的TapEnv实现，支持真机数据缓存
    /// </summary>
    public class TapEnvDebug 
    {
        /// <summary>
        /// 用户数据路径
        /// 在Editor环境下返回真机缓存的路径，在真机环境下调用原生方法
        /// </summary>
        public string USER_DATA_PATH 
        {
            get
            {
#if UNITY_EDITOR && TAP_DEBUG_ENABLE
                return TapSyncCache.GetUserDataPath();
#else
                return GetUserDataPath();
#endif
            }
        }
        
        // 保留原有的DllImport方法（真机环境使用）
        [System.Runtime.InteropServices.DllImport("__Internal", EntryPoint = "TJGetUserDataPath")]
        private static extern string GetUserDataPath();
    }
}

#endif