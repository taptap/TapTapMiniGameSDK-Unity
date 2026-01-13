#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE

using UnityEngine;
using TapTapMiniGame;
using LitJson;
using System;

namespace TapServer
{
    /// <summary>
    /// 多人联机事件管理器 - Unity Editor端
    /// 接收来自真机的事件推送，转发到游戏代码注册的事件处理器
    /// </summary>
    public class TapBattleDebugEventManager
    {
        #region 单例模式

        private static TapBattleDebugEventManager _instance;
        public static TapBattleDebugEventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TapBattleDebugEventManager();
                }
                return _instance;
            }
        }

        private TapBattleDebugEventManager() { }

        #endregion

        #region 事件处理器管理

        private ITapBattleEventHandler registeredEventHandler = null;

        /// <summary>
        /// 注册事件处理器 (由TapDebugBridge.Battle_Initialize调用)
        /// </summary>
        public void RegisterEventHandler(ITapBattleEventHandler eventHandler)
        {
            registeredEventHandler = eventHandler;
            Debug.Log($"[TapBattleDebugEventManager] 事件处理器已注册: {eventHandler != null}");
        }

        /// <summary>
        /// 清除事件处理器
        /// </summary>
        public void UnregisterEventHandler()
        {
            registeredEventHandler = null;
            Debug.Log("[TapBattleDebugEventManager] 事件处理器已清除");
        }

        #endregion

        #region 事件接收与分发

        /// <summary>
        /// 处理来自真机的事件推送
        /// </summary>
        public void OnBattleEventReceived(string eventType, JsonData eventData)
        {
            if (registeredEventHandler == null)
            {
                Debug.LogWarning($"[TapBattleDebugEventManager] 收到事件 {eventType} 但未注册处理器");
                return;
            }

            try
            {
                DispatchEvent(eventType, eventData);
            }
            catch (Exception e)
            {
                Debug.LogError($"[TapBattleDebugEventManager] 分发事件 {eventType} 失败: {e.Message}\n{e.StackTrace}");
            }
        }

        /// <summary>
        /// 根据事件类型分发到对应的处理器方法
        /// </summary>
        private void DispatchEvent(string eventType, JsonData eventData)
        {
            string eventDataJson = eventData.ToJson();

            switch (eventType)
            {
                // 房间事件
                case "OnPlayerEntered":
                    var enterInfo = JsonMapper.ToObject<EnterRoomNotification>(eventDataJson);
                    registeredEventHandler.OnPlayerEntered(enterInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 📥 OnPlayerEntered");
                    break;

                case "OnPlayerLeft":
                    var leaveInfo = JsonMapper.ToObject<LeaveRoomNotification>(eventDataJson);
                    registeredEventHandler.OnPlayerLeft(leaveInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 📤 OnPlayerLeft");
                    break;

                case "OnPlayerKicked":
                    var kickedInfo = JsonMapper.ToObject<PlayerKickedInfo>(eventDataJson);
                    registeredEventHandler.OnPlayerKicked(kickedInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 👢 OnPlayerKicked");
                    break;

                // 玩家事件
                case "OnPlayerOffline":
                    var offlineInfo = JsonMapper.ToObject<PlayerOfflineNotification>(eventDataJson);
                    registeredEventHandler.OnPlayerOffline(offlineInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 📴 OnPlayerOffline");
                    break;

                case "OnPlayerCustomStatusChanged":
                    var statusInfo = JsonMapper.ToObject<PlayerCustomStatusNotification>(eventDataJson);
                    registeredEventHandler.OnPlayerCustomStatusChanged(statusInfo);
                    Debug.Log($"[TapBattleDebugEventManager] ⚡ OnPlayerCustomStatusChanged");
                    break;

                case "OnPlayerCustomPropertiesChanged":
                    // 直接从JsonData提取字段，避免LitJson的双重JSON解析bug
                    var propInfo = new PlayerCustomPropertiesNotification
                    {
                        playerId = eventData["playerId"]?.ToString(),
                        properties = eventData["properties"]?.ToString()  // 保持JSON字符串格式
                    };
                    registeredEventHandler.OnPlayerCustomPropertiesChanged(propInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 🔧 OnPlayerCustomPropertiesChanged");
                    break;

                // 房间属性事件
                case "OnRoomPropertiesChanged":
                    // 直接从JsonData提取字段，避免LitJson的双重JSON解析bug
                    var roomInfo = new RoomPropertiesNotification
                    {
                        id = eventData["id"]?.ToString(),
                        name = eventData["name"]?.ToString(),
                        customProperties = eventData["customProperties"]?.ToString()  // 保持JSON字符串格式
                    };
                    registeredEventHandler.OnRoomPropertiesChanged(roomInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 🏠 OnRoomPropertiesChanged");
                    break;

                // 帧同步事件
                case "OnFrameSyncStarted":
                    var battleStartInfo = JsonMapper.ToObject<FrameSyncStartInfo>(eventDataJson);
                    registeredEventHandler.OnFrameSyncStarted(battleStartInfo);
                    Debug.Log($"[TapBattleDebugEventManager] ▶️ OnFrameSyncStarted: seed={battleStartInfo.seed}");
                    break;

                case "OnFrameReceived":
                    // 解析帧数据
                    var frameData = JsonMapper.ToObject<FrameData>(eventDataJson);
                    registeredEventHandler.OnFrameReceived(frameData);
                    // 帧数据频繁，不输出日志
                    break;

                case "OnFrameSyncStopped":
                        var battleStopInfo = JsonMapper.ToObject<FrameSyncStopInfo>(eventDataJson);
                        registeredEventHandler.OnFrameSyncStopped(battleStopInfo);
                    Debug.Log($"[TapBattleDebugEventManager] ⏹️ OnFrameSyncStopped");
                    break;

                // 消息事件
                case "OnCustomMessageReceived":
                    var customMsgInfo = JsonMapper.ToObject<CustomMessageNotification>(eventDataJson);
                    registeredEventHandler.OnCustomMessageReceived(customMsgInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 💬 OnCustomMessageReceived");
                    break;

                // 错误事件
                case "OnBattleServiceError":
                    var errorInfo = JsonMapper.ToObject<BattleServiceErrorInfo>(eventDataJson);
                    registeredEventHandler.OnBattleServiceError(errorInfo);
                    Debug.LogError($"[TapBattleDebugEventManager] ❌ OnBattleServiceError: {errorInfo.errorMessage}");
                    break;

                case "OnDisconnected":
                    var disconnectInfo = JsonMapper.ToObject<DisconnectedInfo>(eventDataJson);
                    registeredEventHandler.OnDisconnected(disconnectInfo);
                    Debug.LogWarning($"[TapBattleDebugEventManager] 🔌 OnDisconnected: {disconnectInfo.reason}");
                    break;

                default:
                    Debug.LogWarning($"[TapBattleDebugEventManager] ⚠️ 未知事件类型: {eventType}");
                    break;
            }
        }

        #endregion
    }
}

#endif


