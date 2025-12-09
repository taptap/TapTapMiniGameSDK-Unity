#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
using System;
using System.Collections;
using System.Collections.Generic;
using TapTapMiniGame;
using UnityEngine;
using LitJson;
using TapServer;
using UnityEditorInternal;

/// <summary>
/// TapTap调试桥接类
/// 用于server和client之间的通讯
/// </summary>
public partial class TapDebugBridge
{
    #region 系统
    /// <summary>
    /// 打开系统蓝牙设置桥接
    /// </summary>
    public static void OpenSystemBluetoothSetting(OpenSystemBluetoothSettingOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenSystemBluetoothSetting", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenSystemBluetoothSetting回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 打开应用授权设置桥接
    /// </summary>
    public static void OpenAppAuthorizeSetting(OpenAppAuthorizeSettingOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenAppAuthorizeSetting", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenAppAuthorizeSetting回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取窗口信息桥接
    /// </summary>
    public static WindowInfo GetWindowInfo()
    {
#if UNITY_EDITOR && TAP_DEBUG_ENABLE
        // 在Editor环境下使用TapSyncCache缓存的真机数据
        return TapTapMiniGame.TapSyncCache.GetWindowInfo();
#else
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
#endif
    }

    /// <summary>
    /// 获取系统设置桥接
    /// </summary>
    public static SystemSetting GetSystemSetting()
    {
#if UNITY_EDITOR && TAP_DEBUG_ENABLE
        // 在Editor环境下使用TapSyncCache缓存的真机数据
        return TapTapMiniGame.TapSyncCache.GetSystemSetting();
#else
        return new SystemSetting
        {
            bluetoothEnabled = true,
            deviceOrientation = "portrait",
            locationEnabled = true,
            wifiEnabled = true
        };
#endif
    }
    
    /// <summary>
    /// [Object getStorageInfoSync()
    /// getStorageInfo 的同步版本
    /// </summary>
    /// <returns></returns>
    public static GetStorageInfoSyncOption GetStorageInfoSync()
    {
        NotSupported("GetStorageInfoSync");
        return new GetStorageInfoSyncOption();
    }
    
    public static void GetStorageInfo(GetStorageInfoOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetStorageInfoOption", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetStorageInfoOption回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetStorageInfoSuccessCallbackOption>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取系统信息同步桥接
    /// 在Editor环境下返回真机缓存的数据，在真机环境下调用原生方法
    /// </summary>
    public static TapTapMiniGame.SystemInfo GetSystemInfoSync()
    {
#if UNITY_EDITOR && TAP_DEBUG_ENABLE
        // 在Editor环境下使用TapSyncCache缓存的真机数据
        return TapTapMiniGame.TapSyncCache.GetSystemInfo();
#else
        // 在真机环境下使用原始的模拟数据实现
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
#endif
    }

    /// <summary>
    /// 获取系统信息异步桥接
    /// </summary>
    public static void GetSystemInfoAsync(GetSystemInfoAsyncOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetSystemInfoAsync", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetSystemInfoAsync回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<TapTapMiniGame.SystemInfo>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取系统信息桥接
    /// </summary>
    public static void GetSystemInfo(GetSystemInfoOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetSystemInfo", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetSystemInfo回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<TapTapMiniGame.SystemInfo>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取设备信息桥接
    /// </summary>
    public static DeviceInfo GetDeviceInfo()
    {
#if UNITY_EDITOR && TAP_DEBUG_ENABLE
        // 在Editor环境下使用TapSyncCache缓存的真机数据
        return TapTapMiniGame.TapSyncCache.GetDeviceInfo();
#else
        return new DeviceInfo
        {
            brand = "Editor",
            model = "Unity Editor",
            platform = "windows",
            system = "Windows"
        };
#endif
    }

    /// <summary>
    /// 获取应用基础信息桥接
    /// </summary>
    public static AppBaseInfo GetAppBaseInfo()
    {
#if UNITY_EDITOR && TAP_DEBUG_ENABLE
        // 在Editor环境下使用TapSyncCache缓存的真机数据
        return TapTapMiniGame.TapSyncCache.GetAppBaseInfo();
#else
        return new AppBaseInfo
        {
            SDKVersion = "1.0.0",
            enableDebug = true,
            language = "zh_CN",
            version = "1.0.0"
        };
#endif
    }

    /// <summary>
    /// 获取应用授权设置桥接
    /// </summary>
    public static AppAuthorizeSetting GetAppAuthorizeSetting()
    {
#if UNITY_EDITOR && TAP_DEBUG_ENABLE
        // 在Editor环境下使用TapSyncCache缓存的真机数据
        return TapTapMiniGame.TapSyncCache.GetAppAuthorizeSetting();
#else
        return new AppAuthorizeSetting
        {
            albumAuthorized = "authorized",
            cameraAuthorized = "authorized",
            locationAuthorized = "authorized",
            microphoneAuthorized = "authorized"
        };
#endif
    }

    /// <summary>
    /// 获取更新管理器桥接
    /// </summary>
    public static TapUpdateManager GetUpdateManager()
    {
        TapDebugBridge.NotSupported("TapUpdateManager");
        return default(TapUpdateManager);
    }
    #endregion

    #region 生命周期
    /// <summary>
    /// 监听小游戏显示事件桥接
    /// </summary>
    public static void OnShow(Action<OnShowListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnShow" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnShow回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var showResult = response.GetResult<OnShowListenerResult>();
                result?.Invoke(showResult);
            }
        });
    }

    /// <summary>
    /// 监听小游戏隐藏事件桥接
    /// </summary>
    public static void OnHide(Action<GeneralCallbackResult> res)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnHide" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnHide回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var hideResult = response.GetResult<GeneralCallbackResult>();
                res?.Invoke(hideResult);
            }
        });
    }

    /// <summary>
    /// 取消监听小游戏显示事件桥接
    /// </summary>
    public static void OffShow(Action<OnShowListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffShow" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffShow回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 取消监听小游戏隐藏事件桥接
    /// </summary>
    public static void OffHide(Action<GeneralCallbackResult> res)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffHide" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffHide回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 获取启动选项同步桥接
    /// </summary>
    public static LaunchOptionsGame GetLaunchOptionsSync()
    {
        return new LaunchOptionsGame
        {
            scene = 1001,
            query = new Dictionary<string, string>(),
            shareTicket = ""
        };
    }

    /// <summary>
    /// 获取进入选项同步桥接
    /// </summary>
    public static EnterOptionsGame GetEnterOptionsSync()
    {
        return new EnterOptionsGame
        {
            scene = 1001,
            query = new Dictionary<string, string>(),
            shareTicket = ""
        };
    }
    #endregion

    #region 应用级事件
    /// <summary>
    /// 监听未处理的Promise拒绝事件桥接
    /// </summary>
    public static void OnUnhandledRejection(Action<OnUnhandledRejectionListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnUnhandledRejection" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnUnhandledRejection回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var rejectionResult = response.GetResult<OnUnhandledRejectionListenerResult>();
                result?.Invoke(rejectionResult);
            }
        });
    }

    /// <summary>
    /// 监听全局错误事件桥接
    /// </summary>
    public static void OnError(Action<Error> error)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnError" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnError回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var errorResult = response.GetResult<Error>();
                error?.Invoke(errorResult);
            }
        });
    }

    /// <summary>
    /// 监听音频中断结束事件桥接
    /// </summary>
    public static void OnAudioInterruptionEnd(Action<GeneralCallbackResult> res)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnAudioInterruptionEnd" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnAudioInterruptionEnd回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var endResult = response.GetResult<GeneralCallbackResult>();
                res?.Invoke(endResult);
            }
        });
    }

    /// <summary>
    /// 监听音频中断开始事件桥接
    /// </summary>
    public static void OnAudioInterruptionBegin(Action<GeneralCallbackResult> res)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnAudioInterruptionBegin" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnAudioInterruptionBegin回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var beginResult = response.GetResult<GeneralCallbackResult>();
                res?.Invoke(beginResult);
            }
        });
    }

    /// <summary>
    /// 取消监听未处理的Promise拒绝事件桥接
    /// </summary>
    public static void OffUnhandledRejection(Action<OnUnhandledRejectionListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffUnhandledRejection" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffUnhandledRejection回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 取消监听全局错误事件桥接
    /// </summary>
    public static void OffError(Action<Error> error)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffError" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffError回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 取消监听音频中断结束事件桥接
    /// </summary>
    public static void OffAudioInterruptionEnd(Action<GeneralCallbackResult> res)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffAudioInterruptionEnd" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffAudioInterruptionEnd回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 取消监听音频中断开始事件桥接
    /// </summary>
    public static void OffAudioInterruptionBegin(Action<GeneralCallbackResult> res)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffAudioInterruptionBegin" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffAudioInterruptionBegin回复: {response.ToJson()}");
        });
    }
    #endregion

    #region 性能
    /// <summary>
    /// 触发GC桥接
    /// </summary>
    public static void TriggerGC()
    {
        string messageData = JsonMapper.ToJson(new { type = "TriggerGC" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TriggerGC回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 报告性能数据桥接
    /// </summary>
    public static void ReportPerformance(double id, double value, string dimensions)
    {
        var param = new { id = id, value = value, dimensions = dimensions };
        string messageData = JsonMapper.ToJson(new { type = "ReportPerformance", param = param });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ReportPerformance回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 设置启用调试桥接
    /// </summary>
    public static void SetEnableDebug(SetEnableDebugOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetEnableDebug", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetEnableDebug回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取实时日志管理器桥接
    /// </summary>
    public static TapRealtimeLogManager GetRealtimeLogManager()
    {
        return default(TapRealtimeLogManager);
    }

    /// <summary>
    /// 获取日志管理器桥接
    /// </summary>
    public static TapLogManager GetLogManager(GetLogManagerOption option)
    {
        return default(TapLogManager);
    }
    #endregion

    #region 重启
    /// <summary>
    /// 重启小程序桥接
    /// </summary>
    public static void RestartMiniProgram(RestartMiniProgramOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "RestartMiniProgram", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到RestartMiniProgram回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }
    #endregion

    #region 文件
    /// <summary>
    /// 下载文件桥接
    /// </summary>
    public static TapDownloadTask DownloadFile(DownloadFileOption option)
    {
        NotSupported("TapDownloadTask");
        // var downloadTask = new TapDownloadTask();
        // var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        // string messageData = JsonMapper.ToJson(new { type = "DownloadFile", param = serializableParam });
        // NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        // {
        //     Debug.Log($"[测试] 收到DownloadFile回复: {response.ToJson()}");
        //     switch (response.status)
        //     {
        //         case "success":
        //             option.success?.Invoke(response.GetResult<DownloadFileSuccessCallbackResult>());
        //             break;
        //         case "fail":
        //             option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
        //             break;
        //         case "complete":
        //             option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
        //             break;
        //     }
        // });
        return default(TapDownloadTask);
    }
    #endregion

    #region 支付
    /// <summary>
    /// 请求Midas游戏道具支付桥接
    /// </summary>
    public static void RequestMidasPaymentGameItem(RequestMidasPaymentGameItemOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "RequestMidasPaymentGameItem", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到RequestMidasPaymentGameItem回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<MidasPaymentGameItemError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<MidasPaymentGameItemError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 请求Midas好友支付桥接
    /// </summary>
    public static void RequestMidasFriendPayment(RequestMidasFriendPaymentOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "RequestMidasFriendPayment", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到RequestMidasFriendPayment回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<RequestMidasFriendPaymentSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<MidasFriendPaymentError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<MidasFriendPaymentError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 请求Midas支付桥接
    /// </summary>
    public static void RequestMidasPayment(RequestMidasPaymentOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "RequestMidasPayment", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到RequestMidasPayment回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<RequestMidasPaymentSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<RequestMidasPaymentFailCallbackErr>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<MidasPaymentError>());
                    break;
            }
        });
    }
    #endregion

    #region 数据缓存
    /// <summary>
    /// 设置存储同步桥接
    /// </summary>
    public static void SetStorageSync<T>(string key, T data)
    {
        var param = new { key = key, data = data };
        string messageData = JsonMapper.ToJson(new { type = "SetStorageSync", param = param });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetStorageSync回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 设置用户云存储桥接
    /// </summary>
    public static void SetUserCloudStorage(SetUserCloudStorageOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetUserCloudStorage", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetUserCloudStorage回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }
    
    /// <summary>
    /// 从本地缓存中移除指定 key。
    /// </summary>
    public static void RemoveStorage(RemoveStorageOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "RemoveStorage", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] RemoveStorage: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 移除存储同步桥接
    /// </summary>
    public static void RemoveStorageSync(string key)
    {
        var param = new { key = key };
        string messageData = JsonMapper.ToJson(new { type = "RemoveStorageSync", param = param });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到RemoveStorageSync回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 移除用户云存储桥接
    /// </summary>
    public static void RemoveUserCloudStorage(RemoveUserCloudStorageOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "RemoveUserCloudStorage", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到RemoveUserCloudStorage回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 撤销Buffer URL桥接
    /// </summary>
    public static void RevokeBufferURL(string url)
    {
        var param = new { url = url };
        string messageData = JsonMapper.ToJson(new { type = "RevokeBufferURL", param = param });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到RevokeBufferURL回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 获取用户交互存储桥接
    /// </summary>
    public static void GetUserInteractiveStorage(GetUserInteractiveStorageOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetUserInteractiveStorage", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetUserInteractiveStorage回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetUserInteractiveStorageSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GetUserInteractiveStorageFailCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听交互存储修改桥接
    /// </summary>
    public static void OnInteractiveStorageModified(Action<string> res)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnInteractiveStorageModified" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnInteractiveStorageModified回复: {response.ToJson()}");
            if (response.status == "success")
            {
                res?.Invoke(response.resultJson);
            }
        });
    }

    /// <summary>
    /// 取消监听交互存储修改桥接
    /// </summary>
    public static void OffInteractiveStorageModified(Action<string> res)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffInteractiveStorageModified" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffInteractiveStorageModified回复: {response.ToJson()}");
        });
    }
    #endregion

    #region 转发分享
    /// <summary>
    /// 获取分享信息桥接
    /// </summary>
    public static void GetShareInfo(GetShareInfoOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetShareInfo", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetShareInfo回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetGroupEnterInfoSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 分享应用消息桥接
    /// </summary>
    public static void ShareAppMessage(ShareAppMessageOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ShareAppMessage", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ShareAppMessage回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 显示分享图片菜单桥接
    /// </summary>
    public static void ShowShareImageMenu(ShowShareImageMenuOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ShowShareImageMenu", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ShowShareImageMenu回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 更新分享菜单桥接
    /// </summary>
    public static void UpdateShareMenu(UpdateShareMenuOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "UpdateShareMenu", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到UpdateShareMenu回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听分享消息给朋友桥接
    /// </summary>
    public static void OnShareMessageToFriend(Action<OnShareMessageToFriendListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnShareMessageToFriend" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnShareMessageToFriend回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var shareResult = response.GetResult<OnShareMessageToFriendListenerResult>();
                result?.Invoke(shareResult);
            }
        });
    }

    /// <summary>
    /// 监听添加到收藏桥接
    /// </summary>
    public static void OnAddToFavorites(Action<Action<OnAddToFavoritesListenerResult>> callback)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnAddToFavorites" });
        NotSupported("OnAddToFavorites");
        // NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        // {
        //     Debug.Log($"[测试] 收到OnAddToFavorites回复: {response.ToJson()}");
        //     if (response.status == "success")
        //     {
        //         var favoriteResult = response.GetResult<OnAddToFavoritesListenerResult>();
        //         callback?.Invoke(favoriteResult);
        //     }
        // });
    }

    /// <summary>
    /// 取消监听添加到收藏桥接
    /// </summary>
    public static void OffAddToFavorites(Action<Action<OnAddToFavoritesListenerResult>> callback = null)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffAddToFavorites" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffAddToFavorites回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 监听复制URL桥接
    /// </summary>
    public static void OnCopyUrl(Action<Action<OnCopyUrlListenerResult>> callback)
    {
        NotSupported("OnCopyUrl");
        // string messageData = JsonMapper.ToJson(new { type = "OnCopyUrl" });
        // NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        // {
        //     Debug.Log($"[测试] 收到OnCopyUrl回复: {response.ToJson()}");
        //     if (response.status == "success")
        //     {
        //         var copyResult = response.GetResult<OnCopyUrlListenerResult>();
        //         callback?.Invoke((result) => result?.Invoke(copyResult));
        //     }
        // });
    }

    /// <summary>
    /// 取消监听复制URL桥接
    /// </summary>
    public static void OffCopyUrl(Action<Action<OnCopyUrlListenerResult>> callback = null)
    {
        NotSupported("OffCopyUrl");
        // string messageData = JsonMapper.ToJson(new { type = "OffCopyUrl" });
        // NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        // {
        //     Debug.Log($"[测试] 收到OffCopyUrl回复: {response.ToJson()}");
        // });
    }

    /// <summary>
    /// 监听转接桥接
    /// </summary>
    public static void OnHandoff(Action<Action<OnHandoffListenerResult>> callback)
    {
        NotSupported("OnHandoff");
        // string messageData = JsonMapper.ToJson(new { type = "OnHandoff" });
        // NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        // {
        //     Debug.Log($"[测试] 收到OnHandoff回复: {response.ToJson()}");
        //     if (response.status == "success")
        //     {
        //         var handoffResult = response.GetResult<OnHandoffListenerResult>();
        //         callback?.Invoke((result) => result?.Invoke(handoffResult));
        //     }
        // });
    }

    /// <summary>
    /// 取消监听转接桥接
    /// </summary>
    public static void OffHandoff(Action<Action<OnHandoffListenerResult>> callback = null)
    {
        NotSupported("OffHandoff");
        // string messageData = JsonMapper.ToJson(new { type = "OffHandoff" });
        // NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        // {
        //     Debug.Log($"[测试] 收到OffHandoff回复: {response.ToJson()}");
        // });
    }

    /// <summary>
    /// 监听分享到朋友圈桥接
    /// </summary>
    public static void OnShareTimeline(Action<Action<OnShareTimelineListenerResult>> callback)
    {
        NotSupported("OnShareTimeline");
        // string messageData = JsonMapper.ToJson(new { type = "OnShareTimeline" });
        // NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        // {
        //     Debug.Log($"[测试] 收到OnShareTimeline回复: {response.ToJson()}");
        //     if (response.status == "success")
        //     {
        //         var timelineResult = response.GetResult<OnShareTimelineListenerResult>();
        //         callback?.Invoke((result) => result?.Invoke(timelineResult));
        //     }
        // });
    }

    /// <summary>
    /// 取消监听分享到朋友圈桥接
    /// </summary>
    public static void OffShareTimeline(Action<Action<OnShareTimelineListenerResult>> callback = null)
    {
        NotSupported("OffShareTimeline");
        // string messageData = JsonMapper.ToJson(new { type = "OffShareTimeline" });
        // NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        // {
        //     Debug.Log($"[测试] 收到OffShareTimeline回复: {response.ToJson()}");
        // });
    }

    /// <summary>
    /// 设置转接查询桥接
    /// </summary>
    public static bool SetHandoffQuery(string query)
    {
        var param = new { query = query };
        string messageData = JsonMapper.ToJson(new { type = "SetHandoffQuery", param = param });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetHandoffQuery回复: {response.ToJson()}");
        });
        return true;
    }

    /// <summary>
    /// 设置消息给朋友查询桥接
    /// </summary>
    public static bool SetMessageToFriendQuery(SetMessageToFriendQueryOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetMessageToFriendQuery", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetMessageToFriendQuery回复: {response.ToJson()}");
        });
        return true;
    }
    #endregion

    #region 数据分析
    /// <summary>
    /// 报告场景桥接
    /// </summary>
    public static void ReportScene(ReportSceneOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ReportScene", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ReportScene回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<ReportSceneSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<ReportSceneFailCallbackErr>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<ReportSceneError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 报告事件桥接
    /// </summary>
    public static void ReportEvent<T>(string eventId, T data)
    {
        var param = new { eventId = eventId, data = data };
        string messageData = JsonMapper.ToJson(new { type = "ReportEvent", param = param });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ReportEvent回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 报告监控桥接
    /// </summary>
    public static void ReportMonitor(string name, double value)
    {
        var param = new { name = name, value = value };
        string messageData = JsonMapper.ToJson(new { type = "ReportMonitor", param = param });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ReportMonitor回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 报告用户行为分支分析桥接
    /// </summary>
    public static void ReportUserBehaviorBranchAnalytics(ReportUserBehaviorBranchAnalyticsOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ReportUserBehaviorBranchAnalytics", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ReportUserBehaviorBranchAnalytics回复: {response.ToJson()}");
        });
    }
    #endregion

    #region 位置
    /// <summary>
    /// 获取模糊位置桥接
    /// </summary>
    public static void GetFuzzyLocation(GetFuzzyLocationOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetFuzzyLocation", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetFuzzyLocation回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetFuzzyLocationSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }
    #endregion

    #region 输入法
    /// <summary>
    /// 更新键盘桥接
    /// </summary>
    public static void UpdateKeyboard(UpdateKeyboardOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "UpdateKeyboard", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到UpdateKeyboard回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 显示键盘桥接
    /// </summary>
    public static void ShowKeyboard(ShowKeyboardOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ShowKeyboard", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ShowKeyboard回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听键盘输入桥接
    /// </summary>
    public static void OnKeyboardInput(Action<OnKeyboardInputListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnKeyboardInput" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnKeyboardInput回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var inputResult = response.GetResult<OnKeyboardInputListenerResult>();
                result?.Invoke(inputResult);
            }
        });
    }

    /// <summary>
    /// 监听键盘高度变化桥接
    /// </summary>
    public static void OnKeyboardHeightChange(Action<OnKeyboardHeightChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnKeyboardHeightChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnKeyboardHeightChange回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var heightResult = response.GetResult<OnKeyboardHeightChangeListenerResult>();
                result?.Invoke(heightResult);
            }
        });
    }

    /// <summary>
    /// 监听键盘确认桥接
    /// </summary>
    public static void OnKeyboardConfirm(Action<OnKeyboardInputListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnKeyboardConfirm" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnKeyboardConfirm回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var confirmResult = response.GetResult<OnKeyboardInputListenerResult>();
                result?.Invoke(confirmResult);
            }
        });
    }

    /// <summary>
    /// 监听键盘完成桥接
    /// </summary>
    public static void OnKeyboardComplete(Action<OnKeyboardInputListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnKeyboardComplete" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnKeyboardComplete回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var completeResult = response.GetResult<OnKeyboardInputListenerResult>();
                result?.Invoke(completeResult);
            }
        });
    }

    /// <summary>
    /// 取消监听键盘输入桥接
    /// </summary>
    public static void OffKeyboardInput(Action<OnKeyboardInputListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffKeyboardInput" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffKeyboardInput回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 取消监听键盘高度变化桥接
    /// </summary>
    public static void OffKeyboardHeightChange(Action<OnKeyboardHeightChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffKeyboardHeightChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffKeyboardHeightChange回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 取消监听键盘确认桥接
    /// </summary>
    public static void OffKeyboardConfirm(Action<OnKeyboardInputListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffKeyboardConfirm" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffKeyboardConfirm回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 取消监听键盘完成桥接
    /// </summary>
    public static void OffKeyboardComplete(Action<OnKeyboardInputListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffKeyboardComplete" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffKeyboardComplete回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 隐藏键盘桥接
    /// </summary>
    public static void HideKeyboard(HideKeyboardOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "HideKeyboard", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到HideKeyboard回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }
    #endregion

    #region 蓝牙-信标(Beacon)
    /// <summary>
    /// 停止信标发现桥接
    /// </summary>
    public static void StopBeaconDiscovery(StopBeaconDiscoveryOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "StopBeaconDiscovery", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到StopBeaconDiscovery回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BeaconError>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BeaconError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BeaconError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 开始信标发现桥接
    /// </summary>
    public static void StartBeaconDiscovery(StartBeaconDiscoveryOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "StartBeaconDiscovery", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到StartBeaconDiscovery回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BeaconError>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BeaconError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BeaconError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听信标更新桥接
    /// </summary>
    public static void OnBeaconUpdate(Action<OnBeaconUpdateListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnBeaconUpdate" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnBeaconUpdate回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var beaconResult = response.GetResult<OnBeaconUpdateListenerResult>();
                result?.Invoke(beaconResult);
            }
        });
    }

    /// <summary>
    /// 监听信标服务变化桥接
    /// </summary>
    public static void OnBeaconServiceChange(Action<OnBeaconServiceChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnBeaconServiceChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnBeaconServiceChange回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var serviceResult = response.GetResult<OnBeaconServiceChangeListenerResult>();
                result?.Invoke(serviceResult);
            }
        });
    }

    /// <summary>
    /// 取消监听信标更新桥接
    /// </summary>
    public static void OffBeaconUpdate(Action<OnBeaconUpdateListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffBeaconUpdate" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffBeaconUpdate回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 取消监听信标服务变化桥接
    /// </summary>
    public static void OffBeaconServiceChange(Action<OnBeaconServiceChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffBeaconServiceChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffBeaconServiceChange回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 获取信标桥接
    /// </summary>
    public static void GetBeacons(GetBeaconsOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetBeacons", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetBeacons回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetBeaconsSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BeaconError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BeaconError>());
                    break;
            }
        });
    }
    #endregion

    #region 蓝牙-低功耗外围设备
    /// <summary>
    /// 监听低功耗外围设备连接状态变化桥接
    /// </summary>
    public static void OnBLEPeripheralConnectionStateChanged(Action<OnBLEPeripheralConnectionStateChangedListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnBLEPeripheralConnectionStateChanged" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnBLEPeripheralConnectionStateChanged回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var connectionResult = response.GetResult<OnBLEPeripheralConnectionStateChangedListenerResult>();
                result?.Invoke(connectionResult);
            }
        });
    }

    /// <summary>
    /// 取消监听低功耗外围设备连接状态变化桥接
    /// </summary>
    public static void OffBLEPeripheralConnectionStateChanged(Action<OnBLEPeripheralConnectionStateChangedListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffBLEPeripheralConnectionStateChanged" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffBLEPeripheralConnectionStateChanged回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 创建低功耗外围设备服务器桥接
    /// </summary>
    public static void CreateBLEPeripheralServer(CreateBLEPeripheralServerOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "CreateBLEPeripheralServer", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到CreateBLEPeripheralServer回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<CreateBLEPeripheralServerSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 写入低功耗特征值桥接
    /// </summary>
    public static void WriteBLECharacteristicValue(WriteBLECharacteristicValueOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "WriteBLECharacteristicValue", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到WriteBLECharacteristicValue回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 设置低功耗MTU桥接
    /// </summary>
    public static void SetBLEMTU(SetBLEMTUOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetBLEMTU", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetBLEMTU回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<SetBLEMTUSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<SetBLEMTUFailCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 读取低功耗特征值桥接
    /// </summary>
    public static void ReadBLECharacteristicValue(ReadBLECharacteristicValueOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ReadBLECharacteristicValue", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ReadBLECharacteristicValue回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听低功耗MTU变化桥接
    /// </summary>
    public static void OnBLEMTUChange(Action<OnBLEMTUChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnBLEMTUChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnBLEMTUChange回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var mtuResult = response.GetResult<OnBLEMTUChangeListenerResult>();
                result?.Invoke(mtuResult);
            }
        });
    }

    /// <summary>
    /// 监听低功耗连接状态变化桥接
    /// </summary>
    public static void OnBLEConnectionStateChange(Action<OnBLEConnectionStateChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnBLEConnectionStateChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnBLEConnectionStateChange回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var connectionResult = response.GetResult<OnBLEConnectionStateChangeListenerResult>();
                result?.Invoke(connectionResult);
            }
        });
    }

    /// <summary>
    /// 取消监听低功耗MTU变化桥接
    /// </summary>
    public static void OffBLEMTUChange(Action<OnBLEMTUChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffBLEMTUChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffBLEMTUChange回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 取消监听低功耗连接状态变化桥接
    /// </summary>
    public static void OffBLEConnectionStateChange(Action<OnBLEConnectionStateChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffBLEConnectionStateChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffBLEConnectionStateChange回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 通知低功耗特征值变化桥接
    /// </summary>
    public static void NotifyBLECharacteristicValueChange(NotifyBLECharacteristicValueChangeOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "NotifyBLECharacteristicValueChange", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到NotifyBLECharacteristicValueChange回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取低功耗MTU桥接
    /// </summary>
    public static void GetBLEMTU(GetBLEMTUOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetBLEMTU", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetBLEMTU回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetBLEMTUSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取低功耗设备服务桥接
    /// </summary>
    public static void GetBLEDeviceServices(GetBLEDeviceServicesOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetBLEDeviceServices", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetBLEDeviceServices回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetBLEDeviceServicesSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取低功耗设备RSSI桥接
    /// </summary>
    public static void GetBLEDeviceRSSI(GetBLEDeviceRSSIOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetBLEDeviceRSSI", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetBLEDeviceRSSI回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetBLEDeviceRSSISuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取低功耗设备特征桥接
    /// </summary>
    public static void GetBLEDeviceCharacteristics(GetBLEDeviceCharacteristicsOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetBLEDeviceCharacteristics", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetBLEDeviceCharacteristics回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetBLEDeviceCharacteristicsSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 创建低功耗连接桥接
    /// </summary>
    public static void CreateBLEConnection(CreateBLEConnectionOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "CreateBLEConnection", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到CreateBLEConnection回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 关闭低功耗连接桥接
    /// </summary>
    public static void CloseBLEConnection(CloseBLEConnectionOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "CloseBLEConnection", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到CloseBLEConnection回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }
    #endregion

    #region 蓝牙-通用
    /// <summary>
    /// 停止蓝牙设备发现桥接
    /// </summary>
    public static void StopBluetoothDevicesDiscovery(StopBluetoothDevicesDiscoveryOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "StopBluetoothDevicesDiscovery", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到StopBluetoothDevicesDiscovery回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 开始蓝牙设备发现桥接
    /// </summary>
    public static void StartBluetoothDevicesDiscovery(StartBluetoothDevicesDiscoveryOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "StartBluetoothDevicesDiscovery", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到StartBluetoothDevicesDiscovery回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 打开蓝牙适配器桥接
    /// </summary>
    public static void OpenBluetoothAdapter(OpenBluetoothAdapterOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenBluetoothAdapter", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenBluetoothAdapter回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听蓝牙设备发现桥接
    /// </summary>
    public static void OnBluetoothDeviceFound(Action<OnBluetoothDeviceFoundListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnBluetoothDeviceFound" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnBluetoothDeviceFound回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var deviceResult = response.GetResult<OnBluetoothDeviceFoundListenerResult>();
                result?.Invoke(deviceResult);
            }
        });
    }

    /// <summary>
    /// 监听蓝牙适配器状态变化桥接
    /// </summary>
    public static void OnBluetoothAdapterStateChange(Action<OnBluetoothAdapterStateChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnBluetoothAdapterStateChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnBluetoothAdapterStateChange回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var stateResult = response.GetResult<OnBluetoothAdapterStateChangeListenerResult>();
                result?.Invoke(stateResult);
            }
        });
    }

    /// <summary>
    /// 取消监听蓝牙设备发现桥接
    /// </summary>
    public static void OffBluetoothDeviceFound(Action<OnBluetoothDeviceFoundListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffBluetoothDeviceFound" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffBluetoothDeviceFound回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 取消监听蓝牙适配器状态变化桥接
    /// </summary>
    public static void OffBluetoothAdapterStateChange(Action<OnBluetoothAdapterStateChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffBluetoothAdapterStateChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffBluetoothAdapterStateChange回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 配对蓝牙设备桥接
    /// </summary>
    public static void MakeBluetoothPair(MakeBluetoothPairOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "MakeBluetoothPair", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到MakeBluetoothPair回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 检查蓝牙设备是否已配对桥接
    /// </summary>
    public static void IsBluetoothDevicePaired(IsBluetoothDevicePairedOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "IsBluetoothDevicePaired", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到IsBluetoothDevicePaired回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取已连接的蓝牙设备桥接
    /// </summary>
    public static void GetConnectedBluetoothDevices(GetConnectedBluetoothDevicesOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetConnectedBluetoothDevices", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetConnectedBluetoothDevices回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetConnectedBluetoothDevicesSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取蓝牙设备桥接
    /// </summary>
    public static void GetBluetoothDevices(GetBluetoothDevicesOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetBluetoothDevices", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetBluetoothDevices回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetBluetoothDevicesSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取蓝牙适配器状态桥接
    /// </summary>
    public static void GetBluetoothAdapterState(GetBluetoothAdapterStateOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetBluetoothAdapterState", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetBluetoothAdapterState回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetBluetoothAdapterStateSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 关闭蓝牙适配器桥接
    /// </summary>
    public static void CloseBluetoothAdapter(CloseBluetoothAdapterOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "CloseBluetoothAdapter", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到CloseBluetoothAdapter回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<BluetoothError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<BluetoothError>());
                    break;
            }
        });
    }
    #endregion

    #region 设备
    /// <summary>
    /// 获取电池信息同步桥接
    /// </summary>
    public static GetBatteryInfoSyncResult GetBatteryInfoSync()
    {
#if UNITY_EDITOR && TAP_DEBUG_ENABLE
        // 在Editor环境下使用TapSyncCache缓存的真机数据
        return TapTapMiniGame.TapSyncCache.GetBatteryInfo();
#else
        return new GetBatteryInfoSyncResult
        {
            isCharging = false,
            level = 85
        };
#endif
    }

    /// <summary>
    /// 获取电池信息桥接
    /// </summary>
    public static void GetBatteryInfo(GetBatteryInfoOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetBatteryInfo", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetBatteryInfo回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetBatteryInfoSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 设置剪贴板数据桥接
    /// </summary>
    public static void SetClipboardData(SetClipboardDataOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetClipboardData", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetClipboardData回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取剪贴板数据桥接
    /// </summary>
    public static void GetClipboardData(GetClipboardDataOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetClipboardData", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetClipboardData回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetClipboardDataSuccessCallbackOption>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听网络弱连接变化桥接
    /// </summary>
    public static void OnNetworkWeakChange(Action<OnNetworkWeakChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnNetworkWeakChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnNetworkWeakChange回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var networkResult = response.GetResult<OnNetworkWeakChangeListenerResult>();
                result?.Invoke(networkResult);
            }
        });
    }

    /// <summary>
    /// 监听网络状态变化桥接
    /// </summary>
    public static void OnNetworkStatusChange(Action<OnNetworkStatusChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnNetworkStatusChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnNetworkStatusChange回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var statusResult = response.GetResult<OnNetworkStatusChangeListenerResult>();
                result?.Invoke(statusResult);
            }
        });
    }

    /// <summary>
    /// 取消监听网络弱连接变化桥接
    /// </summary>
    public static void OffNetworkWeakChange(Action<OnNetworkWeakChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffNetworkWeakChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffNetworkWeakChange回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 取消监听网络状态变化桥接
    /// </summary>
    public static void OffNetworkStatusChange(Action<OnNetworkStatusChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffNetworkStatusChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffNetworkStatusChange回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 获取网络类型桥接
    /// </summary>
    public static void GetNetworkType(GetNetworkTypeOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetNetworkType", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetNetworkType回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetNetworkTypeSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取本地IP地址桥接
    /// </summary>
    public static void GetLocalIPAddress(GetLocalIPAddressOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetLocalIPAddress", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetLocalIPAddress回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetLocalIPAddressSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 设置录屏时的视觉效果桥接
    /// </summary>
    public static void SetVisualEffectOnCapture(SetVisualEffectOnCaptureOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetVisualEffectOnCapture", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetVisualEffectOnCapture回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 设置屏幕亮度桥接
    /// </summary>
    public static void SetScreenBrightness(SetScreenBrightnessOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetScreenBrightness", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetScreenBrightness回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 设置保持屏幕常亮桥接
    /// </summary>
    public static void SetKeepScreenOn(SetKeepScreenOnOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetKeepScreenOn", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetKeepScreenOn回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听用户截屏桥接
    /// </summary>
    public static void OnUserCaptureScreen(Action<GeneralCallbackResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnUserCaptureScreen" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnUserCaptureScreen回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var captureResult = response.GetResult<GeneralCallbackResult>();
                result?.Invoke(captureResult);
            }
        });
    }

    /// <summary>
    /// 监听录屏状态变化桥接
    /// </summary>
    public static void OnScreenRecordingStateChanged(Action<OnScreenRecordingStateChangedListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnScreenRecordingStateChanged" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnScreenRecordingStateChanged回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var recordingResult = response.GetResult<OnScreenRecordingStateChangedListenerResult>();
                result?.Invoke(recordingResult);
            }
        });
    }

    /// <summary>
    /// 取消监听用户截屏桥接
    /// </summary>
    public static void OffUserCaptureScreen(Action<GeneralCallbackResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffUserCaptureScreen" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffUserCaptureScreen回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 取消监听录屏状态变化桥接
    /// </summary>
    public static void OffScreenRecordingStateChanged(Action<OnScreenRecordingStateChangedListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffScreenRecordingStateChanged" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffScreenRecordingStateChanged回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 获取录屏状态桥接
    /// </summary>
    public static void GetScreenRecordingState(GetScreenRecordingStateOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetScreenRecordingState", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetScreenRecordingState回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetScreenRecordingStateSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取屏幕亮度桥接
    /// </summary>
    public static void GetScreenBrightness(GetScreenBrightnessOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetScreenBrightness", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetScreenBrightness回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetScreenBrightnessSuccessCallbackOption>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 开始加速度计监听桥接
    /// </summary>
    public static void StartAccelerometer(StartAccelerometerOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "StartAccelerometer", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到StartAccelerometer回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 停止加速度计监听桥接
    /// </summary>
    public static void StopAccelerometer(StopAccelerometerOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "StopAccelerometer", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到StopAccelerometer回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听加速度计数据变化桥接
    /// </summary>
    public static void OnAccelerometerChange(Action<OnAccelerometerChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnAccelerometerChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnAccelerometerChange回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var accelResult = response.GetResult<OnAccelerometerChangeListenerResult>();
                result?.Invoke(accelResult);
            }
        });
    }

    /// <summary>
    /// 取消监听加速度计数据变化桥接
    /// </summary>
    public static void OffAccelerometerChange(Action<OnAccelerometerChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffAccelerometerChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffAccelerometerChange回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 开始罗盘监听桥接
    /// </summary>
    public static void StartCompass(StartCompassOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "StartCompass", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到StartCompass回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 停止罗盘监听桥接
    /// </summary>
    public static void StopCompass(StopCompassOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "StopCompass", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到StopCompass回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听罗盘数据变化桥接
    /// </summary>
    public static void OnCompassChange(Action<OnCompassChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnCompassChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnCompassChange回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var compassResult = response.GetResult<OnCompassChangeListenerResult>();
                result?.Invoke(compassResult);
            }
        });
    }

    /// <summary>
    /// 取消监听罗盘数据变化桥接
    /// </summary>
    public static void OffCompassChange(Action<OnCompassChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffCompassChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffCompassChange回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 开始设备运动监听桥接
    /// </summary>
    public static void StartDeviceMotionListening(StartDeviceMotionListeningOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "StartDeviceMotionListening", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到StartDeviceMotionListening回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 停止设备运动监听桥接
    /// </summary>
    public static void StopDeviceMotionListening(StopDeviceMotionListeningOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "StopDeviceMotionListening", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到StopDeviceMotionListening回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听设备运动数据变化桥接
    /// </summary>
    public static void OnDeviceMotionChange(Action<OnDeviceMotionChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnDeviceMotionChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnDeviceMotionChange回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var motionResult = response.GetResult<OnDeviceMotionChangeListenerResult>();
                result?.Invoke(motionResult);
            }
        });
    }

    /// <summary>
    /// 取消监听设备运动数据变化桥接
    /// </summary>
    public static void OffDeviceMotionChange(Action<OnDeviceMotionChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffDeviceMotionChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffDeviceMotionChange回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 设置设备方向桥接
    /// </summary>
    public static void SetDeviceOrientation(SetDeviceOrientationOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetDeviceOrientation", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetDeviceOrientation回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听设备方向变化桥接
    /// </summary>
    public static void OnDeviceOrientationChange(Action<OnDeviceOrientationChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnDeviceOrientationChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnDeviceOrientationChange回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var orientationResult = response.GetResult<OnDeviceOrientationChangeListenerResult>();
                result?.Invoke(orientationResult);
            }
        });
    }

    /// <summary>
    /// 取消监听设备方向变化桥接
    /// </summary>
    public static void OffDeviceOrientationChange(Action<OnDeviceOrientationChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffDeviceOrientationChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffDeviceOrientationChange回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 开始陀螺仪监听桥接
    /// </summary>
    public static void StartGyroscope(StartGyroscopeOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "StartGyroscope", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到StartGyroscope回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听陀螺仪数据变化桥接
    /// </summary>
    public static void OnGyroscopeChange(Action<OnGyroscopeChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnGyroscopeChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnGyroscopeChange回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var gyroResult = response.GetResult<OnGyroscopeChangeListenerResult>();
                result?.Invoke(gyroResult);
            }
        });
    }

    /// <summary>
    /// 取消监听陀螺仪数据变化桥接
    /// </summary>
    public static void OffGyroscopeChange(Action<OnGyroscopeChangeListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffGyroscopeChange" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffGyroscopeChange回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 监听内存警告桥接
    /// </summary>
    public static void OnMemoryWarning(Action<OnMemoryWarningListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnMemoryWarning" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnMemoryWarning回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var memoryResult = response.GetResult<OnMemoryWarningListenerResult>();
                result?.Invoke(memoryResult);
            }
        });
    }

    /// <summary>
    /// 取消监听内存警告桥接
    /// </summary>
    public static void OffMemoryWarning(Action<OnMemoryWarningListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffMemoryWarning" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffMemoryWarning回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 扫描二维码桥接
    /// </summary>
    public static void ScanCode(ScanCodeOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ScanCode", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ScanCode回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<ScanCodeSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 短震动桥接
    /// </summary>
    public static void VibrateShort(VibrateShortOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "VibrateShort", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到VibrateShort回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<VibrateShortFailCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 长震动桥接
    /// </summary>
    public static void VibrateLong(VibrateLongOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "VibrateLong", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到VibrateLong回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }
    #endregion

    #region 功能性API
    /// <summary>
    /// 添加卡片桥接
    /// </summary>
    public static void AddCard(AddCardOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "AddCard", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到AddCard回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<AddCardSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 私有消息授权桥接
    /// </summary>
    public static void AuthPrivateMessage(AuthPrivateMessageOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "AuthPrivateMessage", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到AuthPrivateMessage回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<AuthPrivateMessageSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 检查是否已添加到我的小程序桥接
    /// </summary>
    public static void CheckIsAddedToMyMiniProgram(CheckIsAddedToMyMiniProgramOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "CheckIsAddedToMyMiniProgram", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到CheckIsAddedToMyMiniProgram回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<CheckIsAddedToMyMiniProgramSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 选择图片桥接
    /// </summary>
    public static void ChooseImage(ChooseImageOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ChooseImage", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ChooseImage回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<ChooseImageSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 选择媒体桥接
    /// </summary>
    public static void ChooseMedia(ChooseMediaOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ChooseMedia", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ChooseMedia回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<ChooseMediaSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 选择消息文件桥接
    /// </summary>
    public static void ChooseMessageFile(ChooseMessageFileOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ChooseMessageFile", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ChooseMessageFile回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<ChooseMessageFileSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 压缩图片桥接
    /// </summary>
    public static void CompressImage(CompressImageOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "CompressImage", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到CompressImage回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<CompressImageSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 退出小程序桥接
    /// </summary>
    public static void ExitMiniProgram(ExitMiniProgramOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ExitMiniProgram", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ExitMiniProgram回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 退出语音聊天桥接
    /// </summary>
    public static void ExitVoIPChat(ExitVoIPChatOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ExitVoIPChat", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ExitVoIPChat回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 人脸检测桥接
    /// </summary>
    public static void FaceDetect(FaceDetectOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "FaceDetect", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到FaceDetect回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<FaceDetectSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取可用音频源桥接
    /// </summary>
    public static void GetAvailableAudioSources(GetAvailableAudioSourcesOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetAvailableAudioSources", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetAvailableAudioSources回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetAvailableAudioSourcesSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取后台获取数据桥接
    /// </summary>
    public static void GetBackgroundFetchData(GetBackgroundFetchDataOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetBackgroundFetchData", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetBackgroundFetchData回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetBackgroundFetchDataSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取后台获取令牌桥接
    /// </summary>
    public static void GetBackgroundFetchToken(GetBackgroundFetchTokenOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetBackgroundFetchToken", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetBackgroundFetchToken回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetBackgroundFetchTokenSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取频道直播信息桥接
    /// </summary>
    public static void GetChannelsLiveInfo(GetChannelsLiveInfoOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetChannelsLiveInfo", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetChannelsLiveInfo回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetChannelsLiveInfoSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取频道直播通知信息桥接
    /// </summary>
    public static void GetChannelsLiveNoticeInfo(GetChannelsLiveNoticeInfoOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetChannelsLiveNoticeInfo", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetChannelsLiveNoticeInfo回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetChannelsLiveNoticeInfoSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取扩展配置桥接
    /// </summary>
    public static void GetExtConfig(GetExtConfigOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetExtConfig", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetExtConfig回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetExtConfigSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取游戏俱乐部数据桥接
    /// </summary>
    public static void GetGameClubData(GetGameClubDataOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetGameClubData", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetGameClubData回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetGameClubDataSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取群进入信息桥接
    /// </summary>
    public static void GetGroupEnterInfo(GetGroupEnterInfoOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetGroupEnterInfo", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetGroupEnterInfo回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetGroupEnterInfoSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取推理环境信息桥接
    /// </summary>
    public static void GetInferenceEnvInfo(GetInferenceEnvInfoOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetInferenceEnvInfo", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetInferenceEnvInfo回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetInferenceEnvInfoSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取微信运动数据桥接
    /// </summary>
    public static void GetWeRunData(GetWeRunDataOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetWeRunData", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetWeRunData回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetWeRunDataSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 初始化人脸检测桥接
    /// </summary>
    public static void InitFaceDetect(InitFaceDetectOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "InitFaceDetect", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到InitFaceDetect回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 加入语音聊天桥接
    /// </summary>
    public static void JoinVoIPChat(JoinVoIPChatOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "JoinVoIPChat", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到JoinVoIPChat回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<JoinVoIPChatSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<JoinVoIPChatError>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<JoinVoIPChatError>());
                    break;
            }
        });
    }

    /// <summary>
    /// 导航到小程序桥接
    /// </summary>
    public static void NavigateToMiniProgram(NavigateToMiniProgramOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "NavigateToMiniProgram", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到NavigateToMiniProgram回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 打开卡片桥接
    /// </summary>
    public static void OpenCard(OpenCardOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenCard", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenCard回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 打开频道活动桥接
    /// </summary>
    public static void OpenChannelsActivity(OpenChannelsActivityOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenChannelsActivity", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenChannelsActivity回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 打开频道事件桥接
    /// </summary>
    public static void OpenChannelsEvent(OpenChannelsEventOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenChannelsEvent", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenChannelsEvent回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 打开频道直播桥接
    /// </summary>
    public static void OpenChannelsLive(OpenChannelsLiveOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenChannelsLive", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenChannelsLive回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 打开频道用户资料桥接
    /// </summary>
    public static void OpenChannelsUserProfile(OpenChannelsUserProfileOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenChannelsUserProfile", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenChannelsUserProfile回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 打开客服聊天桥接
    /// </summary>
    public static void OpenCustomerServiceChat(OpenCustomerServiceChatOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenCustomerServiceChat", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenCustomerServiceChat回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 打开客服对话桥接
    /// </summary>
    public static void OpenCustomerServiceConversation(OpenCustomerServiceConversationOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenCustomerServiceConversation", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenCustomerServiceConversation回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<OpenCustomerServiceConversationSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 预览图片桥接
    /// </summary>
    public static void PreviewImage(PreviewImageOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "PreviewImage", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到PreviewImage回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 预览媒体桥接
    /// </summary>
    public static void PreviewMedia(PreviewMediaOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "PreviewMedia", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到PreviewMedia回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 请求订阅消息桥接
    /// </summary>
    public static void RequestSubscribeMessage(RequestSubscribeMessageOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "RequestSubscribeMessage", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到RequestSubscribeMessage回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<RequestSubscribeMessageSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<RequestSubscribeMessageFailCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 请求订阅系统消息桥接
    /// </summary>
    public static void RequestSubscribeSystemMessage(RequestSubscribeSystemMessageOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "RequestSubscribeSystemMessage", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到RequestSubscribeSystemMessage回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<RequestSubscribeSystemMessageSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<RequestSubscribeMessageFailCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 保存文件到磁盘桥接
    /// </summary>
    public static void SaveFileToDisk(SaveFileToDiskOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SaveFileToDisk", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SaveFileToDisk回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 保存图片到相册桥接
    /// </summary>
    public static void SaveImageToPhotosAlbum(SaveImageToPhotosAlbumOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SaveImageToPhotosAlbum", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SaveImageToPhotosAlbum回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 设置后台获取令牌桥接
    /// </summary>
    public static void SetBackgroundFetchToken(SetBackgroundFetchTokenOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetBackgroundFetchToken", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetBackgroundFetchToken回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 设置内部音频选项桥接
    /// </summary>
    public static void SetInnerAudioOption(SetInnerAudioOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetInnerAudioOption", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetInnerAudioOption回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 设置首选帧率桥接
    /// </summary>
    public static void SetPreferredFramesPerSecond(double fps)
    {
        var param = new { fps = fps };
        string messageData = JsonMapper.ToJson(new { type = "SetPreferredFramesPerSecond", param = param });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetPreferredFramesPerSecond回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 停止人脸检测桥接
    /// </summary>
    public static void StopFaceDetect(StopFaceDetectOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "StopFaceDetect", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到StopFaceDetect回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 更新语音聊天静音配置桥接
    /// </summary>
    public static void UpdateVoIPChatMuteConfig(UpdateVoIPChatMuteConfigOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "UpdateVoIPChatMuteConfig", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到UpdateVoIPChatMuteConfig回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 更新TapTap应用桥接
    /// </summary>
    public static void UpdateTaptapApp(UpdateTaptapAppOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "UpdateTaptapApp", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到UpdateTaptapApp回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 开始游戏直播桥接
    /// </summary>
    public static void StartGameLive(StartGameLiveOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "StartGameLive", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到StartGameLive回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 检查游戏直播是否开启桥接
    /// </summary>
    public static void CheckGameLiveEnabled(CheckGameLiveEnabledOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "CheckGameLiveEnabled", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到CheckGameLiveEnabled回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<CheckGameLiveEnabledSuccessCallbackOption>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取用户当前游戏直播信息桥接
    /// </summary>
    public static void GetUserCurrentGameliveInfo(GetUserCurrentGameliveInfoOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetUserCurrentGameliveInfo", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetUserCurrentGameliveInfo回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetUserCurrentGameliveInfoSuccessCallbackOption>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取用户最近游戏直播信息桥接
    /// </summary>
    public static void GetUserRecentGameLiveInfo(GetUserRecentGameLiveInfoOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetUserRecentGameLiveInfo", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetUserRecentGameLiveInfo回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetUserGameLiveDetailsSuccessCallbackOption>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取用户游戏直播详情桥接
    /// </summary>
    public static void GetUserGameLiveDetails(GetUserGameLiveDetailsOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetUserGameLiveDetails", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetUserGameLiveDetails回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetUserGameLiveDetailsSuccessCallbackOption>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 打开频道直播收藏桥接
    /// </summary>
    public static void OpenChannelsLiveCollection(OpenChannelsLiveCollectionOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenChannelsLiveCollection", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenChannelsLiveCollection回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取游戏直播状态桥接
    /// </summary>
    public static GameLiveState GetGameLiveState()
    {
        return null;
    }

    /// <summary>
    /// 监听游戏直播状态变化桥接
    /// </summary>
    public static void OnGameLiveStateChange(Action<OnGameLiveStateChangeCallbackResult, Action<OnGameLiveStateChangeCallbackResponse>> callback)
    {
        
    }

    /// <summary>
    /// 取消监听游戏直播状态变化桥接
    /// </summary>
    public static void OffGameLiveStateChange(Action<OnGameLiveStateChangeCallbackResult, Action<OnGameLiveStateChangeCallbackResponse>> callback = null)
    {
        
    }

    /// <summary>
    /// 打开页面桥接
    /// </summary>
    public static void OpenPage(OpenPageOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenPage", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenPage回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 打开业务视图桥接
    /// </summary>
    public static void OpenBusinessView(OpenBusinessViewOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenBusinessView", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenBusinessView回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 请求订阅直播活动桥接
    /// </summary>
    public static void RequestSubscribeLiveActivity(RequestSubscribeLiveActivityOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "RequestSubscribeLiveActivity", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到RequestSubscribeLiveActivity回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<RequestSubscribeLiveActivitySuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 监听后台获取数据桥接
    /// </summary>
    public static void OnBackgroundFetchData(Action<OnBackgroundFetchDataListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnBackgroundFetchData" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnBackgroundFetchData回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var backgroundFetchResult = response.GetResult<OnBackgroundFetchDataListenerResult>();
                result?.Invoke(backgroundFetchResult);
            }
        });
    }

    /// <summary>
    /// 监听消息桥接
    /// </summary>
    public static void OnMessage(Action<string> res)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnMessage" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnMessage回复: {response.ToJson()}");
            if (response.status == "success")
            {
                res?.Invoke(response.resultJson);
            }
        });
    }

    /// <summary>
    /// 退出指针锁定桥接
    /// </summary>
    public static void ExitPointerLock()
    {
        string messageData = JsonMapper.ToJson(new { type = "ExitPointerLock" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ExitPointerLock回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 操作游戏录制视频桥接
    /// </summary>
    public static void OperateGameRecorderVideo(OperateGameRecorderVideoOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OperateGameRecorderVideo", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OperateGameRecorderVideo回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 请求指针锁定桥接
    /// </summary>
    public static void RequestPointerLock()
    {
        string messageData = JsonMapper.ToJson(new { type = "RequestPointerLock" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到RequestPointerLock回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 保留频道直播桥接
    /// </summary>
    public static void ReserveChannelsLive(ReserveChannelsLiveOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ReserveChannelsLive", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ReserveChannelsLive回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 监听按键桥接
    /// </summary>
    public static void OnKeyDown(Action<OnKeyDownListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnKeyDown" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnKeyDown回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var keyDownResult = response.GetResult<OnKeyDownListenerResult>();
                result?.Invoke(keyDownResult);
            }
        });
    }

    /// <summary>
    /// 取消监听按键桥接
    /// </summary>
    public static void OffKeyDown(Action<OnKeyDownListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffKeyDown" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffKeyDown回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 监听按键桥接
    /// </summary>
    public static void OnKeyUp(Action<OnKeyDownListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnKeyUp" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnKeyUp回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var keyUpResult = response.GetResult<OnKeyDownListenerResult>();
                result?.Invoke(keyUpResult);
            }
        });
    }

    /// <summary>
    /// 取消监听按键桥接
    /// </summary>
    public static void OffKeyUp(Action<OnKeyDownListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffKeyUp" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffKeyUp回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 监听鼠标按下桥接
    /// </summary>
    public static void OnMouseDown(Action<OnMouseDownListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnMouseDown" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnMouseDown回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var mouseDownResult = response.GetResult<OnMouseDownListenerResult>();
                result?.Invoke(mouseDownResult);
            }
        });
    }

    /// <summary>
    /// 取消监听鼠标按下桥接
    /// </summary>
    public static void OffMouseDown(Action<OnMouseDownListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffMouseDown" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffMouseDown回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 监听鼠标移动桥接
    /// </summary>
    public static void OnMouseMove(Action<OnMouseMoveListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnMouseMove" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnMouseMove回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var mouseMoveResult = response.GetResult<OnMouseMoveListenerResult>();
                result?.Invoke(mouseMoveResult);
            }
        });
    }

    /// <summary>
    /// 取消监听鼠标移动桥接
    /// </summary>
    public static void OffMouseMove(Action<OnMouseMoveListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffMouseMove" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffMouseMove回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 监听鼠标按下桥接
    /// </summary>
    public static void OnMouseUp(Action<OnMouseDownListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnMouseUp" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnMouseUp回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var mouseUpResult = response.GetResult<OnMouseDownListenerResult>();
                result?.Invoke(mouseUpResult);
            }
        });
    }

    /// <summary>
    /// 取消监听鼠标按下桥接
    /// </summary>
    public static void OffMouseUp(Action<OnMouseDownListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffMouseUp" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffMouseUp回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 监听语音聊天中断桥接
    /// </summary>
    public static void OnVoIPChatInterrupted(Action<OnVoIPChatInterruptedListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnVoIPChatInterrupted" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnVoIPChatInterrupted回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var voipChatInterruptedResult = response.GetResult<OnVoIPChatInterruptedListenerResult>();
                result?.Invoke(voipChatInterruptedResult);
            }
        });
    }

    /// <summary>
    /// 取消监听语音聊天中断桥接
    /// </summary>
    public static void OffVoIPChatInterrupted(Action<OnVoIPChatInterruptedListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffVoIPChatInterrupted" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffVoIPChatInterrupted回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 监听语音聊天成员变化桥接
    /// </summary>
    public static void OnVoIPChatMembersChanged(Action<OnVoIPChatMembersChangedListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnVoIPChatMembersChanged" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnVoIPChatMembersChanged回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var voipChatMembersChangedResult = response.GetResult<OnVoIPChatMembersChangedListenerResult>();
                result?.Invoke(voipChatMembersChangedResult);
            }
        });
    }

    /// <summary>
    /// 取消监听语音聊天成员变化桥接
    /// </summary>
    public static void OffVoIPChatMembersChanged(Action<OnVoIPChatMembersChangedListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffVoIPChatMembersChanged" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffVoIPChatMembersChanged回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 监听语音聊天发言人变化桥接
    /// </summary>
    public static void OnVoIPChatSpeakersChanged(Action<OnVoIPChatSpeakersChangedListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnVoIPChatSpeakersChanged" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnVoIPChatSpeakersChanged回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var voipChatSpeakersChangedResult = response.GetResult<OnVoIPChatSpeakersChangedListenerResult>();
                result?.Invoke(voipChatSpeakersChangedResult);
            }
        });
    }

    /// <summary>
    /// 取消监听语音聊天发言人变化桥接
    /// </summary>
    public static void OffVoIPChatSpeakersChanged(Action<OnVoIPChatSpeakersChangedListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffVoIPChatSpeakersChanged" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffVoIPChatSpeakersChanged回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 监听语音聊天状态变化桥接
    /// </summary>
    public static void OnVoIPChatStateChanged(Action<OnVoIPChatStateChangedListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnVoIPChatStateChanged" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnVoIPChatStateChanged回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var voipChatStateChangedResult = response.GetResult<OnVoIPChatStateChangedListenerResult>();
                result?.Invoke(voipChatStateChangedResult);
            }
        });
    }

    /// <summary>
    /// 取消监听语音聊天状态变化桥接
    /// </summary>
    public static void OffVoIPChatStateChanged(Action<OnVoIPChatStateChangedListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffVoIPChatStateChanged" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffVoIPChatStateChanged回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 监听滚轮桥接
    /// </summary>
    public static void OnWheel(Action<OnWheelListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OnWheel" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnWheel回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var wheelResult = response.GetResult<OnWheelListenerResult>();
                result?.Invoke(wheelResult);
            }
        });
    }

    /// <summary>
    /// 取消监听滚轮桥接
    /// </summary>
    public static void OffWheel(Action<OnWheelListenerResult> result)
    {
        string messageData = JsonMapper.ToJson(new { type = "OffWheel" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffWheel回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 获取实验信息桥接
    /// </summary>
    public static T GetExptInfoSync<T>(string[] keys)
    {
        NotSupported("GetExptInfoSync");
        // var serializableParam = TapSDKApiUtil.CreateSerializableObject(new { keys = keys });
        // string messageData = JsonMapper.ToJson(new { type = "GetExptInfoSync", param = serializableParam });
        // NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        // {
        //     Debug.Log($"[测试] 收到GetExptInfoSync回复: {response.ToJson()}");
        //     if (response.status == "success")
        //     {
        //         var exptInfoResult = response.GetResult<T>();
        //         return exptInfoResult;
        //     }
        // });
        return default(T);
    }

    /// <summary>
    /// 获取扩展配置桥接
    /// </summary>
    public static T GetExtConfigSync<T>()
    {
        NotSupported("GetExtConfigSync");
        // var serializableParam = TapSDKApiUtil.CreateSerializableObject(new { keys = new string[0] });
        // string messageData = JsonMapper.ToJson(new { type = "GetExtConfigSync", param = serializableParam });
        // NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        // {
        //     Debug.Log($"[测试] 收到GetExtConfigSync回复: {response.ToJson()}");
        //     if (response.status == "success")
        //     {
        //         var extConfigResult = response.GetResult<T>();
        //         return extConfigResult;
        //     }
        // });
        return default(T);
    }

    /// <summary>
    /// 创建图像数据桥接
    /// </summary>
    public static ImageData CreateImageData()
    {
        return null;
    }

    /// <summary>
    /// 创建路径数据桥接
    /// </summary>
    public static Path2D CreatePath2D()
    {
        return null;
    }

    /// <summary>
    /// 检查是否锁定指针桥接
    /// </summary>
    public static bool IsPointerLocked()
    {
        return false;
    }

    /// <summary>
    /// 检查是否支持VK桥接
    /// </summary>
    public static bool IsVKSupport(string version)
    {
        return false;
    }

    /// <summary>
    /// 设置光标桥接
    /// </summary>
    public static bool SetCursor(string path, double x, double y)
    {
        return false;
    }

    /// <summary>
    /// 获取文本行高度桥接
    /// </summary>
    public static double GetTextLineHeight(GetTextLineHeightOption option)
    {
        return 0;
    }

    /// <summary>
    /// 加载字体桥接
    /// </summary>
    public static string LoadFont(string path)
    {
        return null;
    }

    /// <summary>
    /// 创建反馈按钮桥接
    /// </summary>
    public static TapFeedbackButton CreateFeedbackButton(CreateOpenSettingButtonOption option)
    {
        return null;
    }

    /// <summary>
    /// 创建视频解码器桥接
    /// </summary>
    public static TapVideoDecoder CreateVideoDecoder()
    {
        return null;
    }


    #endregion
} 
#endif 