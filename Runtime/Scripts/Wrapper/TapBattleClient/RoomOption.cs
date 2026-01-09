using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 创建房间选项 - 标准Option模式（双重回调机制）
    /// 对应Native: TapSdkOnlineBattleCreateRoom(reqID, clientID, data, dataLen) -> int返回值 + CreateRoomCB异步回调
    /// </summary>
    [Preserve]
    public class CreateRoomOption
    {
        /// <summary>
        /// 创建房间请求数据
        /// </summary>
        [Preserve]
        public CreateRoomConfig data;
        
        // === 立即回调（对应Native返回值）===
        
        /// <summary>
        /// 创建房间成功回调 - 包含完整房间信息
        /// </summary>
        [Preserve]
        public Action<CreateRoomSuccessResponse> success;
        
        /// <summary>
        /// 参数验证失败回调 - 对应Native返回-1/-2
        /// </summary>
        [Preserve]
        public Action<TapCallbackResult> fail;
        
        /// <summary>
        /// 完成回调
        /// </summary>
        [Preserve]
        public Action<TapCallbackResult> complete;
        
        // 注意：实际创建结果通过ITapBattleEventHandler.OnCreateRoomResult异步回调
    }

    /// <summary>
    /// 匹配房间选项 - 标准Option模式（双重回调机制）
    /// 对应Native: TapSdkOnlineBattleMatchRoom(reqID, clientID, data, dataLen) -> int返回值 + MatchRoomCB异步回调
    /// </summary>
    [Preserve]
    public class MatchRoomOption
    {
        /// <summary>
        /// 匹配房间请求数据
        /// </summary>
        [Preserve]
        public MatchRoomConfig data;
        
        // === 立即回调（对应Native返回值）===
        
        /// <summary>
        /// 匹配房间成功回调 - 包含完整房间信息
        /// </summary>
        [Preserve]
        public Action<MatchRoomSuccessResponse> success;
        
        /// <summary>
        /// 参数验证失败回调 - 对应Native返回-1/-2
        /// </summary>
        [Preserve]
        public Action<TapCallbackResult> fail;
        
        /// <summary>
        /// 完成回调
        /// </summary>
        [Preserve]
        public Action<TapCallbackResult> complete;
        
        // 注意：实际匹配结果通过ITapBattleEventHandler.OnMatchRoomResult异步回调
    }

    /// <summary>
    /// 离开房间选项 - 标准Option模式（双重回调机制）
    /// 对应Native: TapSdkOnlineBattleLeaveRoom() -> int返回值 + LeaveRoomCB异步回调
    /// </summary>
    [Preserve]
    public class LeaveRoomOption
    {
        // === 立即回调（对应Native返回值）===

        /// <summary>
        /// 参数验证成功回调 - 对应Native返回0
        /// </summary>
        [Preserve]
        public Action<RoomValidationResponse> success;

        /// <summary>
        /// 参数验证失败回调 - 对应Native返回-1/-2
        /// </summary>
        [Preserve]
        public Action<TapCallbackResult> fail;

        /// <summary>
        /// 完成回调
        /// </summary>
        [Preserve]
        public Action<TapCallbackResult> complete;

        // 注意：实际离开结果通过ITapBattleEventHandler.OnLeaveRoomResult异步回调
    }

    /// <summary>
    /// 获取房间列表选项
    /// </summary>
    [Preserve]
    public class GetRoomListOption
    {
        /// <summary>
        /// 获取房间列表请求数据（可选）
        /// </summary>
        [Preserve]
        public GetRoomListRequest data;

        /// <summary>
        /// 获取房间列表成功回调 - 包含房间列表
        /// </summary>
        [Preserve]
        public Action<GetRoomListSuccessResponse> success;

        /// <summary>
        /// 获取失败回调
        /// </summary>
        [Preserve]
        public Action<TapCallbackResult> fail;

        /// <summary>
        /// 完成回调
        /// </summary>
        [Preserve]
        public Action<TapCallbackResult> complete;
    }

    /// <summary>
    /// 加入房间选项
    /// </summary>
    [Preserve]
    public class JoinRoomOption
    {
        /// <summary>
        /// 加入房间请求数据
        /// </summary>
        [Preserve]
        public JoinRoomRequest data;

        /// <summary>
        /// 加入房间成功回调 - 包含完整房间信息
        /// </summary>
        [Preserve]
        public Action<JoinRoomSuccessResponse> success;

        /// <summary>
        /// 加入失败回调
        /// </summary>
        [Preserve]
        public Action<TapCallbackResult> fail;

        /// <summary>
        /// 完成回调
        /// </summary>
        [Preserve]
        public Action<TapCallbackResult> complete;
    }
}
