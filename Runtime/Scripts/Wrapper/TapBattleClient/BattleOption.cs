using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 通用的Option
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class BattleOption
    {
        /// <summary>
        /// 证成功回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> success;

        /// <summary>
        /// 失败回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> fail;

        /// <summary>
        /// 完成回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> complete;
    }

    /// <summary>
    /// 连接多人对战服务选项 - 专用于Connect API
    /// 服务器返回格式: {"playerId":"xxx","errMsg":"connect:ok"}
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class BattleConnectOption
    {
        /// <summary>
        /// 连接成功回调 - 包含playerId
        /// </summary>
        [System.NonSerialized]
        public Action<BattleConnectResult> success;

        /// <summary>
        /// 连接失败回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> fail;

        /// <summary>
        /// 完成回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> complete;
    }

    /// <summary>
    /// 连接成功返回结果 - 包含playerId
    /// 对应服务器返回: {"playerId":"8b68677fcc7244be9df1afe51a3db34c","errMsg":"connect:ok"}
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class BattleConnectResult
    {
        /// <summary>
        /// 玩家ID - 服务器分配的全局唯一ID
        /// 这个ID在整个多人对战系统中唯一标识当前玩家
        /// </summary>
        public string playerId;

        /// <summary>
        /// 错误消息
        /// </summary>
        public string errMsg;
    }
    
    /// <summary>
    /// 开始对战选项 - 标准Option模式（双重回调机制）
    /// 对应Native: TapSdkOnlineBattleStartBattle(reqID) -> int返回值 + StartBattleCB异步回调
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class StartBattleOption
    {
        
        // === 立即回调（对应Native返回值）===
        
        /// <summary>
        /// 参数验证成功回调 - 对应Native返回0
        /// </summary>
        [System.NonSerialized]
        public Action<BattleValidationResponse> success;
        
        /// <summary>
        /// 参数验证失败回调 - 对应Native返回-1/-2
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> fail;
        
        /// <summary>
        /// 完成回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> complete;
        
        // 注意：实际开始结果通过ITapBattleEventHandler.OnStartBattleResult异步回调
    }

    /// <summary>
    /// 发送输入选项 - 标准Option模式（双重回调机制）
    /// 对应Native: TapSdkOnlineBattleSendInput(reqID, data, dataLen) -> int返回值 + SendInputCB异步回调
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class SendInputOption
    {
        /// <summary>
        /// 游戏操作数据（utf8字符串格式）
        /// </summary>
        public string data;
        
        // === 立即回调（对应Native返回值）===
        
        /// <summary>
        /// 参数验证成功回调 - 对应Native返回0
        /// </summary>
        [System.NonSerialized]
        public Action<BattleValidationResponse> success;
        
        /// <summary>
        /// 参数验证失败回调 - 对应Native返回-1/-2
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> fail;
        
        /// <summary>
        /// 完成回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> complete;
        
        // 注意：实际发送结果通过ITapBattleEventHandler.OnSendInputResult异步回调
    }

    /// <summary>
    /// 停止对战选项 - 标准Option模式（双重回调机制）
    /// 对应Native: TapSdkOnlineBattleStopBattle(reqID) -> int返回值 + StopBattleCB异步回调
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class StopBattleOption
    {
        
        // === 立即回调（对应Native返回值）===
        
        /// <summary>
        /// 参数验证成功回调 - 对应Native返回0
        /// </summary>
        [System.NonSerialized]
        public Action<BattleValidationResponse> success;
        
        /// <summary>
        /// 参数验证失败回调 - 对应Native返回-1/-2
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> fail;
        
        /// <summary>
        /// 完成回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> complete;
    }

    /// <summary>
    /// 踢玩家出房间选项 - 标准Option模式（双重回调机制）
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class KickRoomPlayerOption
    {
        /// <summary>
        /// player_id
        /// </summary>
        public string playerId;
        
        // === 立即回调（对应Native返回值）===
        
        /// <summary>
        /// 参数验证成功回调 - 对应Native返回0
        /// </summary>
        [System.NonSerialized]
        public Action<RoomValidationResponse> success;
        
        /// <summary>
        /// 参数验证失败回调 - 对应Native返回-1/-2
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> fail;
        
        /// <summary>
        /// 完成回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> complete;
        
        // 注意：实际踢人结果通过ITapBattleEventHandler.OnKickRoomPlayerResult异步回调
    }

    /// <summary>
    /// 发送自定义消息数据
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class SendCustomMessageData
    {
        /// <summary>
        /// 自定义消息内容（utf8字符串，最大2048字节）
        /// </summary>
        public string msg;
        
        /// <summary>
        /// 消息接收者类型：0-房间内所有玩家，1-队伍内所有玩家
        /// </summary>
        public int type;
    }

    /// <summary>
    /// 发送自定义消息选项 - 标准Option模式（双重回调机制）
    /// 对应Native: TapSdkOnlineBattleSendCustomMessage(msg, type) -> int返回值 + SendCustomMessageCB异步回调
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class SendCustomMessageOption
    {
        /// <summary>
        /// 消息数据，包含msg和type
        /// </summary>
        public SendCustomMessageData data;
        
        /// <summary>
        /// 自定义消息内容（utf8字符串，最大2048字节）
        /// 注意：这个字段已废弃，请使用data.msg
        /// </summary>
        [System.NonSerialized]
        public string msg;
        
        /// <summary>
        /// 消息接收者类型：0-房间内所有玩家，1-队伍内所有玩家
        /// 注意：这个字段已废弃，请使用data.type
        /// </summary>
        [System.NonSerialized]
        public int type;
        
        // === 立即回调（对应Native返回值）===
        
        /// <summary>
        /// 参数验证成功回调 - 对应Native返回0
        /// </summary>
        [System.NonSerialized]
        public Action<BattleValidationResponse> success;
        
        /// <summary>
        /// 参数验证失败回调 - 对应Native返回-1/-2
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> fail;
        
        /// <summary>
        /// 完成回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> complete;
        
        // 注意：实际发送结果通过ITapBattleEventHandler.OnSendCustomMessageResult异步回调
    }
}
