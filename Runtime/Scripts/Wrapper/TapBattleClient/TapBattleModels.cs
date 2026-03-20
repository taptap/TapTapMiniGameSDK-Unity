using UnityEngine.Scripting;
using System.Collections.Generic;

namespace TapTapMiniGame
{

    /// <summary>
    /// 登录响应 - Result类型
    /// </summary>
    [Preserve]
    public class SignInResponse
    {
        /// <summary>
        /// 玩家ID
        /// </summary>
        [Preserve]
        public string playerId;
    }

    /// <summary>
    /// 初始化响应 - 对应Native TapSdkOnlineBattleInitialize返回值
    /// </summary>
    [Preserve]
    public class InitializeResponse
    {
        /// <summary>
        /// 初始化结果码 (0=成功, -1=失败)
        /// </summary>
        [Preserve]
        public int resultCode;
        
        /// <summary>
        /// 消息
        /// </summary>
        [Preserve]
        public string message;
        
        /// <summary>
        /// SDK版本信息
        /// </summary>
        [Preserve]
        public string version;
    }

    /// <summary>
    /// 登录响应
    /// </summary>
    [Preserve]
    public class SignInValidationResponse
    {
        /// <summary>
        /// 结果码
        /// 0 = 成功
        /// -1 = 失败
        /// -2 = 未初始化
        /// </summary>
        [Preserve]
        public int resultCode;

        /// <summary>
        /// 消息
        /// </summary>
        [Preserve]
        public string message;
    }

    /// <summary>
    /// 版本信息响应 - 对应Native TapSdkOnlineBattleVersion()同步返回
    /// </summary>
    [Preserve]
    public class VersionResponse
    {
        /// <summary>
        /// SDK版本号（如：1.2.5）
        /// </summary>
        [Preserve]
        public string version;
        
        /// <summary>
        /// Git提交版本（可选）
        /// </summary>
        [Preserve]
        public string gitCommit;
        
        /// <summary>
        /// 消息
        /// </summary>
        [Preserve]
        public string message;
    }

    /// <summary>
    /// 登出响应
    /// </summary>
    [Preserve]
    public class SignOutValidationResponse
    {
        /// <summary>
        /// 结果码 (0=成功, -1=失败, -2=未初始化)
        /// </summary>
        [Preserve]
        public int resultCode;

        /// <summary>
        /// 消息
        /// </summary>
        [Preserve]
        public string message;
    }

    /// <summary>
    /// 房间操作响应
    /// </summary>
    [Preserve]
    public class RoomValidationResponse
    {
        /// <summary>
        /// 结果码 (0=成功, -1=失败, -2=未初始化)
        /// </summary>
        [Preserve]
        public int resultCode;

        /// <summary>
        /// 消息
        /// </summary>
        [Preserve]
        public string message;
    }

    /// <summary>
    /// 更新操作响应
    /// </summary>
    [Preserve]
    public class UpdateValidationResponse
    {
        /// <summary>
        /// 结果码 (0=成功, -1=失败, -2=未初始化)
        /// </summary>
        [Preserve]
        public int resultCode;

        /// <summary>
        /// 消息
        /// </summary>
        [Preserve]
        public string message;
    }

    /// <summary>
    /// 对战操作响应
    /// </summary>
    [Preserve]
    public class BattleValidationResponse
    {
        /// <summary>
        /// 结果码 (0=成功, -1=失败, -2=未初始化)
        /// </summary>
        [Preserve]
        public int resultCode;

        /// <summary>
        /// 消息
        /// </summary>
        [Preserve]
        public string message;
    }

    // === 房间管理相关结构 ===

    /// <summary>
    /// 创建房间配置 - 对齐JS层数据结构
    /// JS层格式: { roomCfg: {...}, playerCfg: {...} }
    /// </summary>
    [Preserve]
    public class CreateRoomConfig
    {
        /// <summary>
        /// 房间配置
        /// JS层字段名: roomCfg
        /// </summary>
        [Preserve]
        public RoomConfig roomCfg;
        
        /// <summary>
        /// 玩家配置（可选）
        /// JS层字段名: playerCfg
        /// </summary>
        [Preserve]
        public PlayerConfig playerCfg;
    }

    /// <summary>
    /// 匹配房间配置 - 对齐JS层数据结构
    /// JS层格式: { roomCfg: {...}, playerCfg: {...} }
    /// </summary>
    [Preserve]
    public class MatchRoomConfig
    {
        /// <summary>
        /// 房间配置
        /// </summary>
        [Preserve]
        public RoomConfig roomCfg;
        
        /// <summary>
        /// 玩家配置（可选）
        /// </summary>
        [Preserve]
        public PlayerConfig playerCfg;
    }


    /// <summary>
    /// 房间配置
    /// </summary>
    [Preserve]
    public class RoomConfig
    {
        /// <summary>
        /// 房间最大人数
        /// </summary>
        [Preserve]
        public int maxPlayerCount;
        
        /// <summary>
        /// 房间类型
        /// </summary>
        [Preserve]
        public string type;
        
        /// <summary>
        /// 房间名称
        /// </summary>
        [Preserve]
        public string name;
        
        /// <summary>
        /// 自定义房间属性（可选，最大2048字节）
        /// </summary>
        [Preserve]
        public string customProperties;
        
        /// <summary>
        /// 匹配参数（Key-Value键值对）
        /// </summary>
        public Dictionary<string, string> matchParams;
    }

    /// <summary>
    /// 玩家配置
    /// </summary>
    [Preserve]
    public class PlayerConfig
    {
        /// <summary>
        /// 自定义玩家状态（可选）
        /// </summary>
        [Preserve]
        public int customStatus;
        
        /// <summary>
        /// 自定义玩家属性（可选，最大2048字节）
        /// </summary>
        [Preserve]
        public string customProperties;
    }


    /// <summary>
    /// 创建房间成功响应 - 包含完整房间信息
    /// </summary>
    [Preserve]
    public class CreateRoomSuccessResponse
    {
        /// <summary>
        /// 房间信息
        /// </summary>
        [Preserve]
        public RoomInfo roomInfo;
        
        /// <summary>
        /// 错误消息 (成功时为 "createRoom:ok")
        /// </summary>
        [Preserve]
        public string errMsg;
    }
    
    /// <summary>
    /// 房间基础信息 - 用于房间列表等仅包含基本属性的场景
    /// </summary>
    [Preserve]
    public class RoomBaseInfo
    {
        /// <summary>
        /// 创建时间 (时间戳)
        /// </summary>
        [Preserve]
        public string createTime;
        
        /// <summary>
        /// 自定义房间属性 (JSON字符串)
        /// </summary>
        [Preserve]
        public string customProperties;
        
        /// <summary>
        /// 房间ID
        /// </summary>
        [Preserve]
        public string id;
        
        /// <summary>
        /// 房间最大人数
        /// </summary>
        [Preserve]
        public int maxPlayerCount;
        
        /// <summary>
        /// 房间名称
        /// </summary>
        [Preserve]
        public string name;
    }
    
    /// <summary>
    /// 房间完整信息 - 继承基础信息并添加详细属性
    /// 用于创建房间、加入房间等需要完整信息的场景
    /// </summary>
    [Preserve]
    public class RoomInfo : RoomBaseInfo
    {
        /// <summary>
        /// 房主ID
        /// </summary>
        [Preserve]
        public string ownerId;
        
        /// <summary>
        /// 房间内玩家列表
        /// </summary>
        [Preserve]
        public PlayerInfo[] players;
        
        /// <summary>
        /// 房间类型
        /// </summary>
        [Preserve]
        public string type;
    }
    
    /// <summary>
    /// 房间列表信息 - 继承基础信息并添加当前人数
    /// 用于获取房间列表API返回，包含实时人数但不包含详细玩家列表
    /// </summary>
    [Preserve]
    public class RoomListInfo : RoomBaseInfo
    {
        /// <summary>
        /// 房间当前人数
        /// </summary>
        [Preserve]
        public int playerCount;
    }
    
    /// <summary>
    /// 玩家信息
    /// </summary>
    [Preserve]
    public class PlayerInfo
    {
        /// <summary>
        /// 玩家ID
        /// </summary>
        [Preserve]
        public string id;

        /// <summary>
        /// 玩家状态: 0=离线, 1=在线
        /// </summary>
        [Preserve]
        public int status;

        /// <summary>
        /// 自定义玩家状态(可选)
        /// </summary>
        [Preserve]
        public int customStatus;

        /// <summary>
        /// 自定义玩家属性(可选,JSON字符串,最大2048字节)
        /// </summary>
        [Preserve]
        public string customProperties;
    }

    /// <summary>
    /// 匹配房间成功响应 - 包含完整房间信息
    /// </summary>
    [Preserve]
    public class MatchRoomSuccessResponse
    {
        /// <summary>
        /// 房间信息
        /// </summary>
        [Preserve]
        public RoomInfo roomInfo;
        
        /// <summary>
        /// 错误消息 (成功时为 "matchRoom:ok")
        /// </summary>
        [Preserve]
        public string errMsg;
    }

    /// <summary>
    /// 获取房间列表请求 - 对齐JS层数据结构
    /// JS层格式: { roomType: string, offset: number, limit: number }
    /// </summary>
    [Preserve]
    public class GetRoomListRequest
    {
        /// <summary>
        /// 房间类型（可选，不填则拉取全部类型的房间）
        /// </summary>
        [Preserve]
        public string roomType;

        /// <summary>
        /// 偏移量（可选，默认0，第一次请求时为0）
        /// </summary>
        [Preserve]
        public int offset;

        /// <summary>
        /// 请求获取的房间数量（可选，默认20，最大100）
        /// </summary>
        [Preserve]
        public int limit;
    }

    /// <summary>
    /// 获取房间列表成功响应
    /// </summary>
    [Preserve]
    public class GetRoomListSuccessResponse
    {
        /// <summary>
        /// 房间列表（包含基础信息和当前人数，不含ownerId、players、type）
        /// </summary>
        [Preserve]
        public RoomListInfo[] rooms;

        /// <summary>
        /// 错误消息 (成功时为 "getRoomList:ok")
        /// </summary>
        [Preserve]
        public string errMsg;
    }

    /// <summary>
    /// 加入房间请求 - 对齐JS层数据结构
    /// JS层格式: { data: { roomId: string, playerCfg: {...} } }
    /// </summary>
    [Preserve]
    public class JoinRoomRequest
    {
        /// <summary>
        /// 房间ID
        /// </summary>
        [Preserve]
        public string roomId;

        /// <summary>
        /// 玩家配置（可选）
        /// </summary>
        [Preserve]
        public PlayerConfig playerCfg;
    }

    /// <summary>
    /// 加入房间成功响应
    /// </summary>
    [Preserve]
    public class JoinRoomSuccessResponse
    {
        /// <summary>
        /// 房间信息
        /// </summary>
        [Preserve]
        public RoomInfo roomInfo;

        /// <summary>
        /// 错误消息 (成功时为 "joinRoom:ok")
        /// </summary>
        [Preserve]
        public string errMsg;
    }

    // === 事件通知相关结构 ===

    /// <summary>
    /// 玩家进入房间通�� - 对齐Native EnterRoomNotification
    /// </summary>
    [Preserve]
    public class EnterRoomNotification
    {
        [Preserve]
        public string roomId;          // 房间ID
        [Preserve]
        public PlayerInfo playerInfo;  // 进入房间的玩家完整信息
    }

    /// <summary>
    /// 玩家离开房间通知 - 对齐Native LeaveRoomNotification
    /// </summary>
    [Preserve]
    public class LeaveRoomNotification
    {
        [Preserve]
        public string roomId;       // 房间ID
        [Preserve]
        public string roomOwnerId;  // 房主ID (如果离开的是房主,则为新房主ID)
        [Preserve]
        public string playerId;     // 离开房间的玩家ID
    }

    /// <summary>
    /// 玩家离线通知 - 对齐Native PlayerOfflineNotification
    /// </summary>
    [Preserve]
    public class PlayerOfflineNotification
    {
        [Preserve]
        public string roomId;       // 房间ID
        [Preserve]
        public string roomOwnerId;  // 房主ID (如果离线的是房主,则为新房主ID)
        [Preserve]
        public string playerId;     // 离线玩家ID
    }
    

    /// <summary>
    /// 玩家自定义状态变更通知
    /// </summary>
    [Preserve]
    public class PlayerCustomStatusNotification
    {
        [Preserve]
        public string playerId;  // 更新了自定义状态的玩家ID
        [Preserve]
        public int status;       // 新的自定义状态值
    }

    /// <summary>
    /// 玩家自定义属性变更通知
    /// </summary>
    [Preserve]
    public class PlayerCustomPropertiesNotification
    {
        [Preserve]
        public string playerId;     // 更新了自定义属性的玩家ID
        [Preserve]
        public string properties;   // 新的自定义属性值
    }

    /// <summary>
    /// 房间属性变更通知
    /// </summary>
    [Preserve]
    public class RoomPropertiesNotification
    {
        [Preserve]
        public string id;                 // 房间ID
        [Preserve]
        public string name;               // 房间名称
        [Preserve]
        public string customProperties;   // 房间自定义属性
    }

    /// <summary>
    /// 自定义消息通知 - 对齐Native CustomMessageNotification
    /// </summary>
    [Preserve]
    public class CustomMessageNotification
    {
        [Preserve]
        public string playerId;  // 消息发送者玩家ID
        [Preserve]
        public string msg;       // 自定义消息内容(UTF-8字符串,最大2048字节)
    }
}
