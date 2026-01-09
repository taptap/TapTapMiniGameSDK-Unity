using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;
using LitJson;

namespace TapTapMiniGame
{
    /// <summary>
    /// Tap多人联机事件处理器接口
    /// 用于统一处理多人联机的所有事件回调
    /// 与TapOnlineBattleListenerOption保持一致的数据类型
    /// </summary>
    [Preserve]
    public interface ITapBattleEventHandler
    {
        /// <summary>
        /// 连接断开通知
        /// </summary>
        public void OnDisconnected(DisconnectedInfo info);

        /// <summary>
        /// 对战服务错误通知
        /// </summary>
        public void OnBattleServiceError(BattleServiceErrorInfo info);

        /// <summary>
        /// 房间属性变更通知
        /// </summary>
        public void OnRoomPropertiesChanged(RoomPropertiesNotification info);

        /// <summary>
        /// 玩家自定义属性变更通知
        /// </summary>
        public void OnPlayerCustomPropertiesChanged(PlayerCustomPropertiesNotification info);

        /// <summary>
        /// 玩家自定义状态变更通知
        /// </summary>
        public void OnPlayerCustomStatusChanged(PlayerCustomStatusNotification info);

        /// <summary>
        /// 帧同步停止通知
        /// </summary>
        public void OnFrameSyncStopped(FrameSyncStopInfo info);

        /// <summary>
        /// 帧同步数据通知
        /// </summary>
        public void OnFrameReceived(FrameData frameData);

        /// <summary>
        /// 帧同步开始通知
        /// </summary>
        public void OnFrameSyncStarted(FrameSyncStartInfo info);

        /// <summary>
        /// 玩家离线通知
        /// </summary>
        public void OnPlayerOffline(PlayerOfflineNotification info);

        /// <summary>
        /// 玩家离开房间通知
        /// </summary>
        public void OnPlayerLeft(LeaveRoomNotification info);

        /// <summary>
        /// 玩家进入房间通知
        /// </summary>
        public void OnPlayerEntered(EnterRoomNotification info);

        /// <summary>
        /// 自定义消息通知
        /// </summary>
        public void OnCustomMessageReceived(CustomMessageNotification info);

        /// <summary>
        /// 玩家被踢出房间通知
        /// </summary>
        public void OnPlayerKicked(PlayerKickedInfo info);
    }
}
