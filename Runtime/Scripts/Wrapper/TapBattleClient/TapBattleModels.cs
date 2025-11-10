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
        /// 玩家ID
        /// </summary>
        public string playerId;
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
        public int resultCode;
        
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
        public int resultCode;

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
        public string gitCommit;
        
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
        public int resultCode;

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
        public int resultCode;

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
        public int resultCode;

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
        public int resultCode;

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
        /// 玩家ID
        /// </summary>
        public string id;

        /// <summary>
        /// 玩家状态: 0=离线, 1=在线
        /// </summary>
        public int status;

        /// <summary>
        /// 自定义玩家状态(可选)
        /// </summary>
        public int customStatus;

        /// <summary>
        /// 自定义玩家属性(可选,JSON字符串,最大2048字节)
        /// </summary>
        public string customProperties;
    }

    /// <summary>
    /// 创建房间响应 - 对齐Native CreateRoomResponse (保留原有结构用于兼容)
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class CreateRoomResponse
    {
        public int errorCode;
        public string message;
        public string roomId;
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
        public int errorCode;
        public string message;
        public string roomId;
        // 简化版，一期不包含teams详细信息
    }

    /// <summary>
    /// 获取房间列表请求 - 对齐JS层数据结构
    /// JS层格式: { roomType: string, offset: number, limit: number }
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class GetRoomListRequest
    {
        /// <summary>
        /// 房间类型（可选，不填则拉取全部类型的房间）
        /// </summary>
        public string roomType;

        /// <summary>
        /// 偏移量（可选，默认0，第一次请求时为0）
        /// </summary>
        public int offset;

        /// <summary>
        /// 请求获取的房间数量（可选，默认20，最大100）
        /// </summary>
        public int limit;
    }

    /// <summary>
    /// 获取房间列表成功响应
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class GetRoomListSuccessResponse
    {
        /// <summary>
        /// 房间列表
        /// </summary>
        public RoomInfo[] rooms;

        /// <summary>
        /// 错误消息 (成功时为 "getRoomList:ok")
        /// </summary>
        public string errMsg;
    }

    /// <summary>
    /// 加入房间请求 - 对齐JS层数据结构
    /// JS层格式: { data: { roomId: string, playerCfg: {...} } }
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class JoinRoomRequest
    {
        /// <summary>
        /// 房间ID
        /// </summary>
        public string roomId;

        /// <summary>
        /// 玩家配置（可选）
        /// </summary>
        public PlayerConfig playerCfg;
    }

    /// <summary>
    /// 加入房间成功响应
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class JoinRoomSuccessResponse
    {
        /// <summary>
        /// 房间信息
        /// </summary>
        public RoomInfo roomInfo;

        /// <summary>
        /// 错误消息 (成功时为 "joinRoom:ok")
        /// </summary>
        public string errMsg;
    }

    // === 事件通知相关结构 ===

    /// <summary>
    /// 玩家进入房间通�� - 对齐Native EnterRoomNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class EnterRoomNotification
    {
        public string roomId;          // 房间ID
        public PlayerInfo playerInfo;  // 进入房间的玩家完整信息
    }

    /// <summary>
    /// 玩家离开房间通知 - 对齐Native LeaveRoomNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class LeaveRoomNotification
    {
        public string roomId;       // 房间ID
        public string roomOwnerId;  // 房主ID (如果离开的是房主,则为新房主ID)
        public string playerId;     // 离开房间的玩家ID
    }

    /// <summary>
    /// 玩家离线通知 - 对齐Native PlayerOfflineNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class PlayerOfflineNotification
    {
        public string roomId;       // 房间ID
        public string roomOwnerId;  // 房主ID (如果离线的是房主,则为新房主ID)
        public string playerId;     // 离线玩家ID
    }

    /// <summary>
    /// 对战开始通知 - 对齐Native BattleStartNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class BattleStartNotification
    {
        public RoomInfo roomInfo;   // 完整的房间信息(包含players等)
        public int battleId;        // 对战ID,房间内唯一
        public int seed;            // 随机数种子,用于NewRandomNumberGenerator
        public string serverTms;    // 对战开始服务器时间(毫秒时间戳字符串)
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
        public string playerId;    // 玩家ID
        public string data;        // 玩家操作数据(UTF-8字符串)
        public string serverTms;   // 服务器收到操作的时间(毫秒时间戳字符串)
    }

    /// <summary>
    /// 对战停止通知 - 对齐Native BattleStopNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class BattleStopNotification
    {
        public string roomId;   // 房间ID
        public int battleId;    // 对战ID
        public int reason;      // 结束原因: 0=房主主动结束, 1=超时结束(30分钟)
    }

    /// <summary>
    /// 玩家自定义状态变更通知
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class PlayerCustomStatusNotification
    {
        public string playerId;  // 更新了自定义状态的玩家ID
        public int status;       // 新的自定义状态值
    }

    /// <summary>
    /// 玩家自定义属性变更通知
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class PlayerCustomPropertiesNotification
    {
        public string playerId;     // 更新了自定义属性的玩家ID
        public string properties;   // 新的自定义属性值
    }

    /// <summary>
    /// 房间属性变更通知
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class RoomPropertiesNotification
    {
        public string id;                 // 房间ID
        public string name;               // 房间名称
        public string customProperties;   // 房间自定义属性
    }

    /// <summary>
    /// 对战服务错误通知
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class BattleServiceErrorNotification
    {
        public string errorMessage;
    }

    /// <summary>
    /// 玩家被踢出房间通知 - 对齐Native RoomPlayerKickedNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class RoomPlayerKickedNotification
    {
        public string roomId;    // 被踢玩家所属房间ID
        public string playerId;  // 被踢玩家ID
    }

    /// <summary>
    /// 自定义消息通知 - 对齐Native CustomMessageNotification
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class CustomMessageNotification
    {
        public string playerId;  // 消息发送者玩家ID
        public string msg;       // 自定义消息内容(UTF-8字符串,最大2048字节)
    }
}
