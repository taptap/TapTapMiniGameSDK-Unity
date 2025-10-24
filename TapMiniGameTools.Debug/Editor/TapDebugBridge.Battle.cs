#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
using System;
using TapTapMiniGame;
using UnityEngine;
using LitJson;
using TapServer;

/// <summary>
/// TapTap多人对战调试桥接类
/// 用于Unity Editor环境下调试多人对战API
/// </summary>
public partial class TapDebugBridge
{
    #region 多人对战 - 生命周期管理

    /// <summary>
    /// 初始化多人对战SDK桥接
    /// </summary>
    public static void Battle_Initialize(ITapBattleEventHandler eventHandler)
    {
        // 1. 注册事件处理器到事件管理器
        TapBattleDebugEventManager.Instance.RegisterEventHandler(eventHandler);
        
        // 2. 发送Initialize消息到真机
        string messageData = JsonMapper.ToJson(new { type = "Battle_Initialize" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_Initialize回复: {response.ToJson()}");
            if (response.status == "success")
            {
                Debug.Log("[TapDebugBridge] Battle初始化成功，事件通知系统已激活");
            }
        });
    }

    /// <summary>
    /// 终止化多人对战SDK桥接
    /// </summary>
    public static void Battle_FinalizeSDK()
    {
        string messageData = JsonMapper.ToJson(new { type = "Battle_FinalizeSDK" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_FinalizeSDK回复: {response.ToJson()}");
        });
    }

    #endregion

    #region 多人对战 - 连接管理

    /// <summary>
    /// 连接多人对战服务桥接
    /// </summary>
    public static void Battle_Connect(BattleConnectOption option)
    {
        string messageData = JsonMapper.ToJson(new { type = "Battle_Connect" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_Connect回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BattleConnectResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 断开多人对战服务连接桥接
    /// </summary>
    public static void Battle_Disconnect(BattleOption option)
    {
        string messageData = JsonMapper.ToJson(new { type = "Battle_Disconnect" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_Disconnect回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    #endregion

    #region 多人对战 - 房间管理

    /// <summary>
    /// 创建房间桥接
    /// </summary>
    public static void Battle_CreateRoom(CreateRoomOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Battle_CreateRoom", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_CreateRoom回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<CreateRoomSuccessResponse>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 匹配房间桥接
    /// </summary>
    public static void Battle_MatchRoom(MatchRoomOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Battle_MatchRoom", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_MatchRoom回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<MatchRoomSuccessResponse>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 离开房间桥接
    /// </summary>
    public static void Battle_LeaveRoom(LeaveRoomOption option)
    {
        string messageData = JsonMapper.ToJson(new { type = "Battle_LeaveRoom" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_LeaveRoom回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<RoomValidationResponse>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取房间列表桥接
    /// </summary>
    public static void Battle_GetRoomList(GetRoomListOption option)
    {
        string messageData = JsonMapper.ToJson(new { type = "Battle_GetRoomList" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_GetRoomList回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetRoomListSuccessResponse>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 加入指定房间桥接
    /// </summary>
    public static void Battle_JoinRoom(JoinRoomOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Battle_JoinRoom", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_JoinRoom回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<JoinRoomSuccessResponse>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 踢玩家出房间桥接
    /// </summary>
    public static void Battle_KickRoomPlayer(KickRoomPlayerOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Battle_KickRoomPlayer", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_KickRoomPlayer回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<RoomValidationResponse>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    #endregion

    #region 多人对战 - 玩家属性更新

    /// <summary>
    /// 更新玩家自定义状态桥接
    /// </summary>
    public static void Battle_UpdatePlayerCustomStatus(UpdatePlayerCustomStatusOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Battle_UpdatePlayerCustomStatus", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_UpdatePlayerCustomStatus回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<UpdateValidationResponse>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 更新玩家自定义属性桥接
    /// </summary>
    public static void Battle_UpdatePlayerCustomProperties(UpdatePlayerCustomPropertiesOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Battle_UpdatePlayerCustomProperties", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_UpdatePlayerCustomProperties回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<UpdateValidationResponse>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 更新房间属性桥接
    /// </summary>
    public static void Battle_UpdateRoomProperties(UpdateRoomPropertiesOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Battle_UpdateRoomProperties", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_UpdateRoomProperties回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<UpdateValidationResponse>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    #endregion

    #region 多人对战 - 对战管理

    /// <summary>
    /// 开始对战桥接
    /// </summary>
    public static void Battle_StartBattle(StartBattleOption option)
    {
        string messageData = JsonMapper.ToJson(new { type = "Battle_StartBattle" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_StartBattle回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BattleValidationResponse>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 发送玩家输入桥接
    /// </summary>
    public static void Battle_SendInput(SendInputOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Battle_SendInput", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_SendInput回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BattleValidationResponse>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 停止对战桥接
    /// </summary>
    public static void Battle_StopBattle(StopBattleOption option)
    {
        string messageData = JsonMapper.ToJson(new { type = "Battle_StopBattle" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_StopBattle回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BattleValidationResponse>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    #endregion

    #region 多人对战 - 自定义消息

    /// <summary>
    /// 发送自定义消息桥接
    /// </summary>
    public static void Battle_SendCustomMessage(SendCustomMessageOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Battle_SendCustomMessage", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] 收到Battle_SendCustomMessage回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<BattleValidationResponse>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    #endregion

    #region 多人对战 - 随机数工具

    /// <summary>
    /// 创建随机数生成器桥接
    /// 注意: 这个API没有回调，需要特殊处理
    /// </summary>
    public static void Battle_NewRandomNumberGenerator(int seed)
    {
        string messageData = JsonMapper.ToJson(new {
            type = "Battle_NewRandomNumberGenerator",
            param = new { seed = seed }
        });

        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] Battle_NewRandomNumberGenerator执行完成");
        });
    }

    /// <summary>
    /// 生成随机整数桥接
    /// 注意: 这个API是同步返回值，需要特殊处理
    /// 当前实现为异步模式，返回0作为占位
    /// </summary>
    public static int Battle_RandomInt()
    {
        Debug.LogWarning("[TapDebugBridge] Battle_RandomInt在Editor环境下暂不支持同步返回，请在真机环境测试");
        // TODO: 考虑使用TapSyncCache机制支持同步返回
        return 0;
    }

    /// <summary>
    /// 释放随机数生成器桥接
    /// 注意: 这个API没有回调，需要特殊处理
    /// </summary>
    public static void Battle_FreeRandomNumberGenerator()
    {
        string messageData = JsonMapper.ToJson(new { type = "Battle_FreeRandomNumberGenerator" });

        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[TapDebugBridge] Battle_FreeRandomNumberGenerator执行完成");
        });
    }

    #endregion
}

#endif
