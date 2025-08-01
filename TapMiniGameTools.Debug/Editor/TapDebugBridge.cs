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

    #region 基础API桥接方法

    /// <summary>
    /// 初始化SDK桥接
    /// </summary>
    public static void Tap_InitSDK(Action<int> callback)
    {
        string messageData = JsonMapper.ToJson(new { type = "InitSDK" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到InitSDK回复: {response.ToJson()}");
            if (response.status == "success")
            {
                int code = response.GetResult<int>();
                callback?.Invoke(code);
            }
        });
    }

    public static void NotSupported(string functionName = "")
    {
        Debug.Log($"Not Supported in Editor {functionName}");
    }
    #endregion

    #region 私有辅助方法

    /// <summary>
    /// 发送消息到客户端
    /// </summary>
    /// <param name="method">方法名</param>
    /// <param name="data">数据</param>
    private static void SendMessageToClient(string method, object data)
    {
        // TODO: 实现向客户端发送消息的逻辑
        Debug.Log($"SendMessageToClient: {method} - {data}");
    }

    /// <summary>
    /// 处理来自服务器的消息
    /// </summary>
    /// <param name="message">消息内容</param>
    private static void HandleServerMessage(string message)
    {
        // TODO: 实现处理服务器消息的逻辑
        Debug.Log($"HandleServerMessage: {message}");
    }

    #endregion
    
    /// <summary>
    /// 通用桥接方法：
    /// - TResult: 回调里成功/失败/完成 都会用到的 Result 类型
    /// - TOption: 传入的 Option 类型（ShowLoadingOption、HideLoadingOption、ShowModalOption…）
    /// </summary>
    private static void CallApi<TResult, TOption>(
        string apiType,
        TOption option,
        Action<TResult> onSuccess,
        Action<TResult> onFail,
        Action<TResult> onComplete
    ) where TOption : class
    {
        // 序列化参数
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = apiType, param = serializableParam });
    
        // 发送
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            // 根据返回的 status 反射出 TResult
            Debug.Log($"[测试] 收到回复: {response.ToJson()}");
            TResult result = response.GetResult<TResult>();
            switch (response.status)
            {
                case "success":
                    onSuccess?.Invoke(result);
                    break;
                case "fail":
                    onFail?.Invoke(result);
                    break;
                case "complete":
                    onComplete?.Invoke(result);
                    break;
            }
        });
    }

} 
#endif 