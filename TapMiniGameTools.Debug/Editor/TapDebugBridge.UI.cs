#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
using System;
using System.Collections;
using System.Collections.Generic;
using TapTapMiniGame;
using UnityEngine;
using LitJson;
using TapServer;

/// <summary>
/// TapTap调试桥接类
/// 用于server和client之间的通讯
/// </summary>
public partial class TapDebugBridge
{
    #region 界面
    /// <summary>
    /// 显示Toast提示桥接
    /// </summary>
    public static void ShowToast(ShowToastOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ShowToast", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ShowToast回复: {response.ToJson()}");
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
    /// 显示Loading提示桥接
    /// </summary>
    public static void ShowLoading(ShowLoadingOption option)
    {
        // var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        // string messageData = JsonMapper.ToJson(new { type = "ShowLoading", param = serializableParam });
        // NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        // {
        //     Debug.Log($"[测试] 收到ShowLoading回复: {response.ToJson()}");
        //     switch (response.status)
        //     {
        //         case "success":
        //             option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
        //             break;
        //         case "fail":
        //             option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
        //             break;
        //         case "complete":
        //             option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
        //             break;
        //     }
        // });
        CallApi<GeneralCallbackResult, ShowLoadingOption>(
            apiType: "ShowLoading",
            option: option,
            onSuccess: option.success,
            onFail:    option.fail,
            onComplete:option.complete
        );
    }

    /// <summary>
    /// 隐藏Loading提示桥接
    /// </summary>
    public static void HideLoading(HideLoadingOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "HideLoading", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到HideLoading回复: {response.ToJson()}");
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
    /// 显示模态对话框桥接
    /// </summary>
    public static void ShowModal(ShowModalOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ShowModal", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ShowModal回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<ShowModalSuccessCallbackResult>());
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
    /// 显示操作菜单桥接
    /// </summary>
    public static void ShowActionSheet(ShowActionSheetOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ShowActionSheet", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ShowActionSheet回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<ShowActionSheetSuccessCallbackResult>());
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
    /// 隐藏消息提示框桥接
    /// </summary>
    public static void HideToast(HideToastOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "HideToast", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到HideToast回复: {response.ToJson()}");
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
    /// 隐藏分享菜单桥接
    /// </summary>
    public static void HideShareMenu(HideShareMenuOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "HideShareMenu", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到HideShareMenu回复: {response.ToJson()}");
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
    /// 显示分享菜单桥接
    /// </summary>
    public static void ShowShareMenu(ShowShareMenuOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ShowShareMenu", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到ShowShareMenu回复: {response.ToJson()}");
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
    /// 设置菜单样式桥接
    /// </summary>
    public static void SetMenuStyle(SetMenuStyleOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetMenuStyle", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetMenuStyle回复: {response.ToJson()}");
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
    /// 获取菜单按钮边界客户端矩形桥接
    /// </summary>
    public static ClientRect GetMenuButtonBoundingClientRect()
    {
        // 同步方法，返回模拟的客户端矩形数据
        return new ClientRect
        {
            bottom = 50,
            height = 32,
            left = 281,
            right = 367,
            top = 18,
            width = 86
        };
    }

    /// <summary>
    /// 设置状态栏样式桥接
    /// </summary>
    public static void SetStatusBarStyle(SetStatusBarStyleOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetStatusBarStyle", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetStatusBarStyle回复: {response.ToJson()}");
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
    /// 监听窗口尺寸变化事件桥接
    /// </summary>
    public static void OnWindowResize(Action<OnWindowResizeListenerResult> result)
    {
        // 模拟窗口尺寸变化监听
        string messageData = JsonMapper.ToJson(new { type = "OnWindowResize" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnWindowResize回复: {response.ToJson()}");
            if (response.status == "success")
            {
                var windowResult = response.GetResult<OnWindowResizeListenerResult>();
                result?.Invoke(windowResult);
            }
        });
    }

    /// <summary>
    /// 取消监听窗口尺寸变化事件桥接
    /// </summary>
    public static void OffWindowResize(Action<OnWindowResizeListenerResult> result)
    {
        // 模拟取消窗口尺寸变化监听
        string messageData = JsonMapper.ToJson(new { type = "OffWindowResize" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffWindowResize回复: {response.ToJson()}");
            // 取消监听通常不需要回调处理
        });
    }
    #endregion
} 
#endif 