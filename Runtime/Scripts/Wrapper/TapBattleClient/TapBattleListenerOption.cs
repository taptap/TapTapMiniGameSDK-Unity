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
    public Action<RoomPropertiesChangeInfo> onRoomPropertiesChange;
    public Action<PlayerCustomPropertiesChangeInfo> onPlayerCustomPropertiesChange;
    public Action<PlayerCustomStatusChangeInfo> onPlayerCustomStatusChange;
    public Action<BattleStopInfo> onBattleStop;
    public Action<string> onBattleFrame;
    public Action<BattleStartInfo> onBattleStart;
    public Action<PlayerOfflineInfo> playerOffline;
    public Action<PlayerLeaveRoomInfo> playerLeaveRoom;
    public Action<PlayerEnterRoomInfo> playerEnterRoom;
    public Action<CustomMessageInfo> onCustomMessage;
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
/// 对战服务错误信息
/// </summary>
[Preserve]
public class BattleServiceErrorInfo
{
    public string errorMessage;
    public int errorCode;
    
    [Preserve]
    public BattleServiceErrorInfo() { }
}

/// <summary>
/// 房间属性变化信息
/// </summary>
[Preserve]
public class RoomPropertiesChangeInfo
{
    public string id;                    // 房间ID
    public string name;                  // 房间名称
    public Dictionary<string, object> customProperties;  // 自定义属性

    [Preserve]
    public RoomPropertiesChangeInfo() { }
}

/// <summary>
/// 玩家自定义属性变化信息
/// </summary>
[Preserve]
public class PlayerCustomPropertiesChangeInfo
{
    public string playerId;
    public Dictionary<string, object> properties;
    
    [Preserve]
    public PlayerCustomPropertiesChangeInfo() { }
}

/// <summary>
/// 玩家自定义状态变化信息
/// </summary>
[Preserve]
public class PlayerCustomStatusChangeInfo
{
    public string playerId;
    public int status;
    
    [Preserve]
    public PlayerCustomStatusChangeInfo() { }
}

/// <summary>
/// 对战停止信息
/// </summary>
[Preserve]
public class BattleStopInfo
{
    public string roomId;     // 房间ID
    public string battleId;   // 对战ID（字符串类型，与服务器返回一致）
    public int reason;        // 结束原因: 0=房主主动结束, 1=超时结束(30分钟)

    [Preserve]
    public BattleStopInfo() { }
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
/// 对战开始信息
/// </summary>
[Preserve]
public class BattleStartInfo
{
    public string roomId;
    public string battleId;  // 对战ID，房间内唯一（字符串类型，与服务器返回一致）
    public int startTime;
    public int seed;  // 随机数种子，用于NewRandomNumberGenerator

    [Preserve]
    public BattleStartInfo() { }
}

/// <summary>
/// 玩家离线信息
/// </summary>
[Preserve]
public class PlayerOfflineInfo
{
    public string playerId;
    public string playerName;
    
    [Preserve]
    public PlayerOfflineInfo() { }
}

/// <summary>
/// 玩家离开房间信息
/// </summary>
[Preserve]
public class PlayerLeaveRoomInfo
{
    public string playerId;
    public string playerName;
    public string roomId;
    
    [Preserve]
    public PlayerLeaveRoomInfo() { }
}

/// <summary>
/// 玩家进入房间信息
/// </summary>
[Preserve]
public class PlayerEnterRoomInfo
{
    public string roomId;
    public PlayerInfo playerInfo;
    
    [Preserve]
    public PlayerEnterRoomInfo() { }
}

/// <summary>
/// 自定义消息信息
/// </summary>
[Preserve]
public class CustomMessageInfo
{
    public string playerId;    // 消息发送者玩家ID
    public object message;     // 消息内容
    public int type;           // 消息类型

    [Preserve]
    public CustomMessageInfo() { }
}

/// <summary>
/// 玩家被踢信息
/// </summary>
[Preserve]
public class PlayerKickedInfo
{
    public string playerId;
    public string reason;
    
    [Preserve]
    public PlayerKickedInfo() { }
}
}