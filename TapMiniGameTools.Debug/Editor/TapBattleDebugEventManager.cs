#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE

using UnityEngine;
using TapTapMiniGame;
using LitJson;
using System;

namespace TapServer
{
    /// <summary>
    /// 多人对战事件管理器 - Unity Editor端
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
                case "OnPlayerEnterRoom":
                    var enterInfo = JsonMapper.ToObject<PlayerEnterRoomInfo>(eventDataJson);
                    registeredEventHandler.OnPlayerEnterRoom(enterInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 📥 OnPlayerEnterRoom");
                    break;

                case "OnPlayerLeaveRoom":
                    var leaveInfo = JsonMapper.ToObject<PlayerLeaveRoomInfo>(eventDataJson);
                    registeredEventHandler.OnPlayerLeaveRoom(leaveInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 📤 OnPlayerLeaveRoom");
                    break;

                case "OnPlayerKicked":
                    var kickedInfo = JsonMapper.ToObject<PlayerKickedInfo>(eventDataJson);
                    registeredEventHandler.OnPlayerKicked(kickedInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 👢 OnPlayerKicked");
                    break;

                // 玩家事件
                case "OnPlayerOffline":
                    var offlineInfo = JsonMapper.ToObject<PlayerOfflineInfo>(eventDataJson);
                    registeredEventHandler.OnPlayerOffline(offlineInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 📴 OnPlayerOffline");
                    break;

                case "OnPlayerCustomStatusChange":
                    var statusInfo = JsonMapper.ToObject<PlayerCustomStatusChangeInfo>(eventDataJson);
                    registeredEventHandler.OnPlayerCustomStatusChange(statusInfo);
                    Debug.Log($"[TapBattleDebugEventManager] ⚡ OnPlayerCustomStatusChange");
                    break;

                case "OnPlayerCustomPropertiesChange":
                    var propInfo = JsonMapper.ToObject<PlayerCustomPropertiesChangeInfo>(eventDataJson);
                    registeredEventHandler.OnPlayerCustomPropertiesChange(propInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 🔧 OnPlayerCustomPropertiesChange");
                    break;

                // 房间属性事件
                case "OnRoomPropertiesChange":
                    var roomInfo = JsonMapper.ToObject<RoomPropertiesChangeInfo>(eventDataJson);
                    registeredEventHandler.OnRoomPropertiesChange(roomInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 🏠 OnRoomPropertiesChange");
                    break;

                // 对战事件
                case "OnBattleStart":
                    var battleStartInfo = JsonMapper.ToObject<BattleStartInfo>(eventDataJson);
                    registeredEventHandler.OnBattleStart(battleStartInfo);
                    Debug.Log($"[TapBattleDebugEventManager] ▶️ OnBattleStart: seed={battleStartInfo.seed}");
                    break;

                case "OnBattleFrame":
                    // frameData是字符串类型，直接提取
                    string frameData = eventData.ToString();
                    registeredEventHandler.OnBattleFrame(frameData);
                    // 帧数据频繁，不输出日志
                    break;

                case "OnBattleStop":
                        var battleStopInfo = JsonMapper.ToObject<BattleStopInfo>(eventDataJson);
                        registeredEventHandler.OnBattleStop(battleStopInfo);
                    Debug.Log($"[TapBattleDebugEventManager] ⏹️ OnBattleStop");
                    break;

                // 消息事件
                case "OnCustomMessage":
                    var customMsgInfo = JsonMapper.ToObject<CustomMessageInfo>(eventDataJson);
                    registeredEventHandler.OnCustomMessage(customMsgInfo);
                    Debug.Log($"[TapBattleDebugEventManager] 💬 OnCustomMessage");
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


