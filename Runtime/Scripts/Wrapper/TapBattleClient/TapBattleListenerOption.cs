using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
// === TapOnlineBattle事件监听器数据结构 ===

/// <summary>
/// TapOnlineBattle事件监听器选项类
/// 用于一次性传入多个事件回调函数，每个回调对应一个独立的callbackId
/// </summary>
[Preserve]
public class TapOnlineBattleListenerOption
{
    public Action<DisconnectedInfo> onDisconnected;
    public Action<BattleServiceErrorInfo> onBattleServiceError;
    public Action<RoomPropertiesNotification> onRoomPropertiesChange;
    public Action<PlayerCustomPropertiesNotification> onPlayerCustomPropertiesChange;
    public Action<PlayerCustomStatusNotification> onPlayerCustomStatusChange;
    public Action<FrameSyncStopInfo> onBattleStop;
    public Action<string> onBattleFrame;
    public Action<FrameSyncStartInfo> onBattleStart;
    public Action<PlayerOfflineNotification> playerOffline;
    public Action<LeaveRoomNotification> playerLeaveRoom;
    public Action<EnterRoomNotification> playerEnterRoom;
    public Action<CustomMessageNotification> onCustomMessage;
    public Action<PlayerKickedInfo> onPlayerKicked;
}

/// <summary>
/// 连接断开信息
/// </summary>
[Preserve]
public class DisconnectedInfo
{
    public string reason;
    public int code;
    
    [Preserve]
    public DisconnectedInfo() { }
}

/// <summary>
/// 对战服务错误信息 - 对齐Native BattleServiceErrorNotification (补充errorCode)
/// </summary>
[Preserve]
public class BattleServiceErrorInfo
{
    public string errorMessage;
    public int errorCode;  // 错误码
    
    [Preserve]
    public BattleServiceErrorInfo() { }
}

/// <summary>
/// 帧同步停止信息 - 对齐Native BattleStopNotification
/// </summary>
[Preserve]
public class FrameSyncStopInfo
{
    public string roomId;   // 房间ID
    public int battleId;    // 对战ID
    public int reason;      // 结束原因: 0=房主主动结束, 1=超时结束(30分钟)

    [Preserve]
    public FrameSyncStopInfo() { }
}


/// <summary>
/// 帧同步信息
/// </summary>
[Preserve]
public class BattleFrameInfo<T>
{
    /// <summary>
    /// 帧ID
    /// </summary>
    public int id;
    
    /// <summary>
    /// 玩家输入数据列表
    /// </summary>
    public List<T> inputs;
    
    [Preserve]
    public BattleFrameInfo() { }
}

/// <summary>
/// 帧同步开始信息 - 对齐Native BattleStartNotification
/// </summary>
[Preserve]
public class FrameSyncStartInfo
{
    public RoomInfo roomInfo;   // 完整的房间信息(包含players等)
    public int battleId;        // 对战ID,房间内唯一
    public int seed;            // 随机数种子,用于NewRandomNumberGenerator
    public string serverTms;    // 对战开始服务器时间(毫秒时间戳字符串)

    [Preserve]
    public FrameSyncStartInfo() { }
}

/// <summary>
/// 玩家被踢信息 - 对齐Native RoomPlayerKickedNotification
/// </summary>
[Preserve]
public class PlayerKickedInfo
{
    public string roomId;    // 被踢玩家所属房间ID
    public string playerId;  // 被踢玩家ID
    
    [Preserve]
    public PlayerKickedInfo() { }
}
}