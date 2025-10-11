using UnityEngine.Scripting;

namespace TapTapMiniGame
{

    /// <summary>
    /// 登录响应 - Result类型
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class SignInResponse
    {
        /// <summary>
        /// 错误码 (0=成功)
        /// </summary>
        public int error_code;
        
        /// <summary>
        /// 消息
        /// </summary>
        public string message;
    }

    /// <summary>
    /// 初始化响应 - 对应Native TapSdkOnlineBattleInitialize返回值
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class InitializeResponse
    {
        /// <summary>
        /// 初始化结果码 (0=成功, -1=失败)
        /// </summary>
        public int result_code;
        
        /// <summary>
        /// 消息
        /// </summary>
        public string message;
        
        /// <summary>
        /// SDK版本信息
        /// </summary>
        public string version;
    }

    /// <summary>
    /// 登录响应
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class SignInValidationResponse
    {
        /// <summary>
        /// 结果码
        /// 0 = 成功
        /// -1 = 失败
        /// -2 = 未初始化
        /// </summary>
        public int result_code;

        /// <summary>
        /// 消息
        /// </summary>
        public string message;
    }

    /// <summary>
    /// 版本信息响应 - 对应Native TapSdkOnlineBattleVersion()同步返回
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class VersionResponse
    {
        /// <summary>
        /// SDK版本号（如：1.2.5）
        /// </summary>
        public string version;
        
        /// <summary>
        /// Git提交版本（可选）
        /// </summary>
        public string git_commit;
        
        /// <summary>
        /// 消息
        /// </summary>
        public string message;
    }

    /// <summary>
    /// 登出响应
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class SignOutValidationResponse
    {
        /// <summary>
        /// 结果码 (0=成功, -1=失败, -2=未初始化)
        /// </summary>
        public int result_code;

        /// <summary>
        /// 消息
        /// </summary>
        public string message;
    }

    /// <summary>
    /// 房间操作响应
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class RoomValidationResponse
    {
        /// <summary>
        /// 结果码 (0=成功, -1=失败, -2=未初始化)
        /// </summary>
        public int result_code;

        /// <summary>
        /// 消息
        /// </summary>
        public string message;
    }

    /// <summary>
    /// 更新操作响应
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class UpdateValidationResponse
    {
        /// <summary>
        /// 结果码 (0=成功, -1=失败, -2=未初始化)
        /// </summary>
        public int result_code;

        /// <summary>
        /// 消息
        /// </summary>
        public string message;
    }

    /// <summary>
    /// 对战操作响应
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class BattleValidationResponse
    {
        /// <summary>
        /// 结果码 (0=成功, -1=失败, -2=未初始化)
        /// </summary>
        public int result_code;

        /// <summary>
        /// 消息
        /// </summary>
        public string message;
    }

    // === 房间管理相关结构 ===

    /// <summary>
    /// 创建房间请求 - 对齐JS层数据结构
    /// JS层格式: { roomCfg: {...}, playerCfg: {...} }
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class CreateRoomRequest
    {
        /// <summary>
        /// 房间配置
        /// JS层字段名: roomCfg
        /// </summary>
        public RoomConfig roomCfg;
        
        /// <summary>
        /// 玩家配置（可选）
        /// JS层字段名: playerCfg
        /// </summary>
        public PlayerConfig playerCfg;
    }

    /// <summary>
    /// 匹配房间请求 - 对齐JS层数据结构
    /// JS层格式: { roomCfg: {...}, playerCfg: {...} }
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class MatchRoomRequest
    {
        /// <summary>
        /// 房间配置
        /// </summary>
        public RoomConfig roomCfg;
        
        /// <summary>
        /// 玩家配置（可选）
        /// </summary>
        public PlayerConfig playerCfg;
    }


    /// <summary>
    /// 房间配置
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class RoomConfig
    {
        /// <summary>
        /// 房间最大人数
        /// </summary>
        public int maxPlayerCount;
        
        /// <summary>
        /// 房间类型
        /// </summary>
        public string type;
        
        /// <summary>
        /// 房间名称
        /// </summary>
        public string name;
        
        /// <summary>
        /// 自定义房间属性（可选，最大2048字节）
        /// </summary>
        public string customProperties;
        
        /// <summary>
        /// 匹配参数（从PlayerConfig移过来）
        /// </summary>
        public MatchParams matchParams;
    }

    /// <summary>
    /// 玩家配置
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class PlayerConfig
    {
        /// <summary>
        /// 自定义玩家状态（可选）
        /// </summary>
        public int customStatus;
        
        /// <summary>
        /// 自定义玩家属性（可选，最大2048字节）
        /// </summary>
        public string customProperties;
        
        // matchParams 已移动到 RoomConfig 中
    }

    /// <summary>
    /// 匹配参数
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class MatchParams
    {
        /// <summary>
        /// 玩家等级（改为string类型）
        /// </summary>
        public string level;
        
        /// <summary>
        /// 玩家积分（改为string类型）
        /// </summary>
        public string score;
    }

    /// <summary>
    /// 创建房间成功响应 - 包含完整房间信息
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class CreateRoomSuccessResponse
    {
        /// <summary>
        /// 房间信息
        /// </summary>
        public RoomInfo roomInfo;
        
        /// <summary>
        /// 错误消息 (成功时为 "createRoom:ok")
        /// </summary>
        public string errMsg;
    }
    
    /// <summary>
    /// 房间信息
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class RoomInfo
    {
        /// <summary>
        /// 创建时间 (时间戳)
        /// </summary>
        public string createTime;
        
        /// <summary>
        /// 自定义房间属性 (JSON字符串)
        /// </summary>
        public string customProperties;
        
        /// <summary>
        /// 房间ID
        /// </summary>
        public string id;
        
        /// <summary>
        /// 房间最大人数
        /// </summary>
        public int maxPlayerCount;
        
        /// <summary>
        /// 房间名称
        /// </summary>
        public string name;
        
        /// <summary>
        /// 房主ID
        /// </summary>
        public string ownerId;
        
        /// <summary>
        /// 房间内玩家列表
        /// </summary>
        public PlayerInfo[] players;
        
        /// <summary>
        /// 房间类型
        /// </summary>
        public string type;
    }
    
    /// <summary>
    /// 玩家信息
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class PlayerInfo
    {
        /// <summary>
        /// 自定义玩家属性 (JSON字符串)
        /// </summary>
        public string customProperties;
        
        /// <summary>
        /// 玩家ID
        /// </summary>
        public string id;
        
        /// <summary>
        /// 玩家状态
        /// </summary>
        public int status;
    }

    /// <summary>
    /// 创建房间响应 - 对齐Native CreateRoomResponse (保留原有结构用于兼容)
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class CreateRoomResponse
    {
        public int error_code;
        public string message;
        public string room_id;
    }

    /// <summary>
    /// 匹配房间成功响应 - 包含完整房间信息
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class MatchRoomSuccessResponse
    {
        /// <summary>
        /// 房间信息
        /// </summary>
        public RoomInfo roomInfo;
        
        /// <summary>
        /// 错误消息 (成功时为 "matchRoom:ok")
        /// </summary>
        public string errMsg;
    }

    /// <summary>
    /// 匹配房间响应 - 对齐Native MatchRoomResponse (保留原有结构用于兼容)
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class MatchRoomResponse
    {
        public int error_code;
        public string message;
        public string room_id;
        // 简化版，一期不包含teams详细信息
    }

    // === 事件通知相关结构 ===

    /// <summary>
    /// 玩家进入房间通知 - 对齐Native EnterRoomNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class EnterRoomNotification
    {
        public string room_id;
        public string player_id;
    }

    /// <summary>
    /// 玩家离开房间通知 - 对齐Native LeaveRoomNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class LeaveRoomNotification
    {
        public string room_id;
        public string player_id;
        public string room_owner_id;
    }

    /// <summary>
    /// 玩家离线通知 - 对齐Native PlayerOfflineNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class PlayerOfflineNotification
    {
        public string room_id;
        public string player_id;
        public string room_owner_id;
    }

    /// <summary>
    /// 对战开始通知 - 对齐Native BattleStartNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class BattleStartNotification
    {
        public string room_id;
        public int seed;
        public long server_tms;
    }

    /// <summary>
    /// 帧同步数据 - 对齐Native BattleFrameSynchronization
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class BattleFrameSynchronization
    {
        public int id;
        public PlayerInput[] inputs;
    }

    /// <summary>
    /// 玩家输入数据 - 对齐Native格式
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class PlayerInput
    {
        public string player_id;
        public string data;
        public string server_tms;
    }

    /// <summary>
    /// 对战停止通知 - 对齐Native BattleStopNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class BattleStopNotification
    {
        public string room_id;
    }

    /// <summary>
    /// 玩家自定义状态变更通知
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class PlayerCustomStatusNotification
    {
        public string room_id;
        public string player_id;
        public int custom_status;
    }

    /// <summary>
    /// 玩家自定义属性变更通知
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class PlayerCustomPropertiesNotification
    {
        public string room_id;
        public string player_id;
        public string custom_properties;
    }

    /// <summary>
    /// 房间自定义属性变更通知
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class RoomCustomPropertiesNotification
    {
        public string room_id;
        public string custom_properties;
    }

    /// <summary>
    /// 对战服务错误通知
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class BattleServiceErrorNotification
    {
        public string error_message;
    }

    /// <summary>
    /// 玩家被踢出房间通知 - 对齐Native RoomPlayerKickedNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class RoomPlayerKickedNotification
    {
        public string room_id;
        public string team_id;
        public string player_id;
    }

    /// <summary>
    /// 自定义消息通知 - 对齐Native CustomMessageNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class CustomMessageNotification
    {
        public string player_id;
        public string msg;
    }
}
