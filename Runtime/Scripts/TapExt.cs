#if (UNITY_WEBGL || UNITY_MINIGAME || WEIXINMINIGAME) && !(UNITY_EDITOR && TAP_DEBUG_ENABLE)
using System;

namespace TapTapMiniGame
{
    public partial class Tap
    {
        public static void SetShareboardHidden(SetShareboardHiddenOption option)
        {
            TapExtManagerHandler.Instance.SetShareboardHidden(option);
        }
        
        public static void ShowShareboard(ShowShareboardOption option)
        {
            TapExtManagerHandler.Instance.ShowShareboard(option);
        }
        
        public static void OnShareMessage(OnShareOption option)
        {
            TapExtManagerHandler.Instance.OnShareMessage(option);
        }

        public static void OffShareMessage()
        {
            TapExtManagerHandler.Instance.OffShareMessage();
        }

        public static void OpenFriendList()
        {
            TapExtManagerHandler.Instance.OpenFriendList();
        }
        
        public static void CreateHomeScreenWidget(CreateHomeScreenWidgetOption option)
        {
            TapExtManagerHandler.Instance.CreateHomeScreenWidget(option);
        }
        
        public static void HasHomeScreenWidgetAndPinned(HasHomeScreenWidgetOption option)
        {
            TapExtManagerHandler.Instance.HasHomeScreenWidgetAndPinned(option);
        }

        public static void GetLeaderBoardManager()
        {
            TapExtManagerHandler.Instance.GetLeaderBoardManager();
        }

        public static void OpenLeaderboard(OpenLeaderboardOption option)
        {
            TapExtManagerHandler.Instance.OpenLeaderboard(option);
        }

        public static void SubmitScore(SubmitScoreOption option)
        {
            TapExtManagerHandler.Instance.SubmitScores(option);
        }

        public static void LoadLeaderboardScores(LoadLeaderboardScoresOption option)
        {
            TapExtManagerHandler.Instance.LoadLeaderboardScores(option);
        }

        public static void LoadCurrentPlayerLeaderboardScore(LoadCurrentPlayerLeaderboardScoreOption option)
        {
            TapExtManagerHandler.Instance.LoadCurrentPlayerLeaderboardScore(option);
        }

        public static void LoadPlayerCenteredScores(LoadPlayerCenteredScoresOption option)
        {
            TapExtManagerHandler.Instance.LoadPlayerCenteredScores(option);
        }
    }

    public class TapAchievement {
        public static void Setup(AchievementManagerOption option)
        {
            TapExtManagerHandler.Instance.SetupAchievementManager(option);
        }
        public static void RegisterCallBack(AchievementListener listener)
        {
            TapExtManagerHandler.Instance.AchievementManager_RegisterListener(listener);
        }
        public static void UnRegisterCallBack(AchievementListener listener)
        {
            TapExtManagerHandler.Instance.AchievementManager_UnregisterListener(listener);
        }
        public static void Unlock(string achievementId)
        {
            TapExtManagerHandler.Instance.AchievementManager_Unlock(achievementId);
        }
        public static void Increment(string achievementId, int steps)
        {
            TapExtManagerHandler.Instance.AchievementManager_IncrementSteps(achievementId, steps);
        }
        public static void ShowAchievements()
        {
            TapExtManagerHandler.Instance.AchievementManager_ShowAllAchievements();
        }
        public static void SetToastEnable(bool enabled)
        {
            TapExtManagerHandler.Instance.AchievementManager_SetToastEnabled(enabled);
        }
    }

    /// <summary>
    /// 云存档管理类
    /// </summary>
    public class TapCloudSave
    {
        /// <summary>
        /// 初始化云存档管理器
        /// </summary>
        public static void Setup()
        {
            TapExtManagerHandler.Instance.CreateCloudSaveManager();
        }

        /// <summary>
        /// 创建存档
        /// </summary>
        /// <param name="option">创建存档选项</param>
        public static void CreateArchive(CreateArchiveOption option)
        {
            TapExtManagerHandler.Instance.CloudSaveManager_CreateArchive(option);
        }

        /// <summary>
        /// 更新存档
        /// </summary>
        /// <param name="option">更新存档选项</param>
        public static void UpdateArchive(UpdateArchiveOption option)
        {
            TapExtManagerHandler.Instance.CloudSaveManager_UpdateArchive(option);
        }

        /// <summary>
        /// 获取存档列表
        /// </summary>
        /// <param name="option">获取存档列表选项</param>
        public static void GetArchiveList(GetArchiveListOption option)
        {
            TapExtManagerHandler.Instance.CloudSaveManager_GetArchiveList(option);
        }

        /// <summary>
        /// 获取存档数据
        /// </summary>
        /// <param name="option">获取存档数据选项</param>
        public static void GetArchiveData(GetArchiveDataOption option)
        {
            TapExtManagerHandler.Instance.CloudSaveManager_GetArchiveData(option);
        }

        /// <summary>
        /// 获取存档封面
        /// </summary>
        /// <param name="option">获取存档封面选项</param>
        public static void GetArchiveCover(GetArchiveCoverOption option)
        {
            TapExtManagerHandler.Instance.CloudSaveManager_GetArchiveCover(option);
        }

        /// <summary>
        /// 删除存档
        /// </summary>
        /// <param name="option">删除存档选项</param>
        public static void DeleteArchive(DeleteArchiveOption option)
        {
            TapExtManagerHandler.Instance.CloudSaveManager_DeleteArchive(option);
        }
    }

    /// <summary>
    /// Tap多人对战客户端 - OnlineBattleManager架构
    /// </summary>
    public class TapBattleClient
    {
        /// <summary>
        /// 初始化多人对战SDK - OnlineBattleManager架构
        /// 自动创建Manager实例
        /// </summary>
        /// <param name="eventHandler">事件处理器，需实现ITapBattleEventHandler接口</param>
        public static void Initialize(ITapBattleEventHandler eventHandler)
        {
            TapExtManagerHandler.Instance.TapBattle_Initialize(eventHandler);
        }

        /// <summary>
        /// 终止化多人对战SDK
        /// 自动销毁Manager实例
        /// </summary>
        public static void FinalizeSDK()
        {
            TapExtManagerHandler.Instance.TapBattle_Finalize();
        }

        // === 房间管理API ===

        /// <summary>
        /// 创建房间 - OnlineBattleManager架构
        /// </summary>
        /// <param name="option">创建房间选项</param>
        public static void CreateRoom(CreateRoomOption option)
        {
            TapExtManagerHandler.Instance.TapBattle_CreateRoom(option);
        }

        /// <summary>
        /// 匹配房间 - OnlineBattleManager架构
        /// </summary>
        /// <param name="option">匹配房间选项</param>
        public static void MatchRoom(MatchRoomOption option)
        {
            TapExtManagerHandler.Instance.TapBattle_MatchRoom(option);
        }

        /// <summary>
        /// 离开房间 - OnlineBattleManager架构
        /// </summary>
        /// <param name="option">离开房间选项</param>
        public static void LeaveRoom(LeaveRoomOption option)
        {
            TapExtManagerHandler.Instance.TapBattle_LeaveRoom(option);
        }

        // === 玩家属性更新API ===

        /// <summary>
        /// 更新玩家自定义状态 - OnlineBattleManager架构
        /// </summary>
        /// <param name="option">更新选项</param>
        public static void UpdatePlayerCustomStatus(UpdatePlayerCustomStatusOption option)
        {
            TapExtManagerHandler.Instance.TapBattle_UpdatePlayerCustomStatus(option);
        }

        /// <summary>
        /// 更新玩家自定义属性 - OnlineBattleManager架构
        /// </summary>
        /// <param name="option">更新选项</param>
        public static void UpdatePlayerCustomProperties(UpdatePlayerCustomPropertiesOption option)
        {
            TapExtManagerHandler.Instance.TapBattle_UpdatePlayerCustomProperties(option);
        }

        /// <summary>
        /// 更新房间属性 - OnlineBattleManager架构
        /// </summary>
        /// <param name="option">更新选项</param>
        public static void UpdateRoomProperties(UpdateRoomPropertiesOption option)
        {
            TapExtManagerHandler.Instance.TapBattle_UpdateRoomProperties(option);
        }

        // === 对战管理API ===

        /// <summary>
        /// 开始对战 - OnlineBattleManager架构
        /// </summary>
        /// <param name="option">开始对战选项</param>
        public static void StartBattle(StartBattleOption option)
        {
            TapExtManagerHandler.Instance.TapBattle_StartBattle(option);
        }

        /// <summary>
        /// 发送玩家输入 - OnlineBattleManager架构
        /// </summary>
        /// <param name="option">发送输入选项</param>
        public static void SendInput(SendInputOption option)
        {
            TapExtManagerHandler.Instance.TapBattle_SendInput(option);
        }

        /// <summary>
        /// 停止对战 - OnlineBattleManager架构
        /// </summary>
        /// <param name="option">停止对战选项</param>
        public static void StopBattle(StopBattleOption option)
        {
            TapExtManagerHandler.Instance.TapBattle_StopBattle(option);
        }

        // === 随机数工具API ===

        /// <summary>
        /// 创建随机数生成器
        /// 创建后会成为当前活跃的随机数生成器实例，native层会自动管理实例
        /// </summary>
        /// <param name="seed">随机数种子</param>
        public static void NewRandomNumberGenerator(int seed)
        {
            TapExtManagerHandler.Instance.TapBattle_NewRandomNumberGenerator(seed);
        }

        /// <summary>
        /// 生成随机整数
        /// 使用当前活跃的随机数生成器实例生成随机数
        /// </summary>
        /// <returns>随机整数</returns>
        public static int RandomInt()
        {
            return TapExtManagerHandler.Instance.TapBattle_RandomInt();
        }

        /// <summary>
        /// 释放当前的随机数生成器
        /// 释放当前活跃的随机数生成器实例
        /// </summary>
        public static void FreeRandomNumberGenerator()
        {
            TapExtManagerHandler.Instance.TapBattle_FreeRandomNumberGenerator();
        }

        // === 连接管理API ===

        /// <summary>
        /// 连接多人对战服务 - OnlineBattleManager架构
        /// </summary>
        /// <param name="option">连接选项</param>
        public static void Connect(BattleConnectOption option)
        {
            TapExtManagerHandler.Instance.TapBattle_Connect(option);
        }

        /// <summary>
        /// 断开多人对战服务连接 - OnlineBattleManager架构
        /// </summary>
        /// <param name="option">断开连接选项</param>
        public static void Disconnect(BattleOption option)
        {
            TapExtManagerHandler.Instance.TapBattle_Disconnect(option);
        }

        // === 新增API ===

        /// <summary>
        /// 踢玩家出房间 - OnlineBattleManager架构
        /// 仅限房主调用，且房间未开战时才能使用
        /// </summary>
        /// <param name="option">踢玩家选项</param>
        public static void KickRoomPlayer(KickRoomPlayerOption option)
        {
            TapExtManagerHandler.Instance.TapBattle_KickRoomPlayer(option);
        }

        /// <summary>
        /// 发送自定义消息 - OnlineBattleManager架构
        /// 每秒允许调用3次，无需等待请求完成的回调
        /// </summary>
        /// <param name="option">发送自定义消息选项</param>
        public static void SendCustomMessage(SendCustomMessageOption option)
        {
            TapExtManagerHandler.Instance.TapBattle_SendCustomMessage(option);
        }
    }
}

#endif