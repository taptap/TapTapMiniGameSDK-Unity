#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE

using System;

namespace TapTapMiniGame
{
    public partial class Tap
    {
        #region 分享
        public static void SetShareboardHidden(SetShareboardHiddenOption option)
        {
            TapDebugBridge.SetShareboardHidden(option);
        }
        
        public static void ShowShareboard(ShowShareboardOption option)
        {
            TapDebugBridge.ShowShareboard(option);
        }
        
        public static void OnShareMessage(OnShareOption option)
        {
            TapDebugBridge.OnShareMessage(option);
        }

        public static void OffShareMessage()
        {
            TapDebugBridge.OffShareMessage();
        }

        public static void OpenFriendList()
        {
            TapDebugBridge.OpenFriendList();
        }
        #endregion
        
        public static void CreateHomeScreenWidget(CreateHomeScreenWidgetOption option)
        {
            TapDebugBridge.CreateHomeScreenWidget(option);
        }
        
        public static void HasHomeScreenWidgetAndPinned(HasHomeScreenWidgetOption option)
        {
            TapDebugBridge.HasHomeScreenWidgetAndPinned(option);
        }

        public static void GetLeaderBoardManager()
        {
            TapDebugBridge.Tap_GetLeaderBoardManager();
        }

        public static void OpenLeaderboard(OpenLeaderboardOption option)
        {
            TapDebugBridge.Tap_OpenLeaderboard(option);
        }

        public static void SubmitScore(SubmitScoreOption option)
        {
            TapDebugBridge.Tap_SubmitScore(option);
        }

        public static void LoadLeaderboardScores(LoadLeaderboardScoresOption option)
        {
            TapDebugBridge.Tap_LoadLeaderboardScores(option);
        }

        public static void LoadCurrentPlayerLeaderboardScore(LoadCurrentPlayerLeaderboardScoreOption option)
        {
            TapDebugBridge.Tap_LoadCurrentPlayerLeaderboardScore(option);
        }

        public static void LoadPlayerCenteredScores(LoadPlayerCenteredScoresOption option)
        {
            TapDebugBridge.Tap_LoadPlayerCenteredScores(option);
        }
    }
    #region 成就
    public class TapAchievement {
        public static void Setup(AchievementManagerOption option)
        {
            TapDebugBridge.TapAchievement_Setup(option);
        }
        public static void RegisterCallBack(AchievementListener listener)
        {
            TapDebugBridge.TapAchievement_RegisterCallBack(listener);
        }
        public static void UnRegisterCallBack(AchievementListener listener)
        {
            TapDebugBridge.TapAchievement_UnRegisterCallBack(listener);
        }
        public static void Unlock(string achievementId)
        {
            TapDebugBridge.TapAchievement_Unlock(achievementId);
        }
        public static void Increment(string achievementId, int steps)
        {
            TapDebugBridge.TapAchievement_Increment(achievementId, steps);
        }
        public static void ShowAchievements()
        {
            TapDebugBridge.TapAchievement_ShowAchievements();
        }
        public static void SetToastEnable(bool enabled)
        {
            TapDebugBridge.TapAchievement_SetToastEnable(enabled);
        }
    }
    #endregion
    
    #region 云存档
#if TAP_CLOUDSAVE_ENABLE
    public class TapCloudSave
    {
        /// <summary>
        /// 初始化云存档管理器
        /// </summary>
        public static void Setup()
        {
            TapDebugBridge.TapCloudSave_Setup();
        }

        /// <summary>
        /// 创建存档
        /// </summary>
        /// <param name="option">创建存档选项</param>
        public static void CreateArchive(CreateArchiveOption option)
        {
            TapDebugBridge.TapCloudSave_CreateArchive(option);
        }

        /// <summary>
        /// 更新存档
        /// </summary>
        /// <param name="option">更新存档选项</param>
        public static void UpdateArchive(UpdateArchiveOption option)
        {
            TapDebugBridge.TapCloudSave_UpdateArchive(option);
        }

        /// <summary>
        /// 获取存档列表
        /// </summary>
        /// <param name="option">获取存档列表选项</param>
        public static void GetArchiveList(GetArchiveListOption option)
        {
            TapDebugBridge.TapCloudSave_GetArchiveList(option);
        }

        /// <summary>
        /// 获取存档数据
        /// </summary>
        /// <param name="option">获取存档数据选项</param>
        public static void GetArchiveData(GetArchiveDataOption option)
        {
            TapDebugBridge.TapCloudSave_GetArchiveData(option);
        }

        /// <summary>
        /// 获取存档封面
        /// </summary>
        /// <param name="option">获取存档封面选项</param>
        public static void GetArchiveCover(GetArchiveCoverOption option)
        {
            TapDebugBridge.TapCloudSave_GetArchiveCover(option);
        }

        /// <summary>
        /// 删除存档
        /// </summary>
        /// <param name="option">删除存档选项</param>
        public static void DeleteArchive(DeleteArchiveOption option)
        {
            TapDebugBridge.TapCloudSave_DeleteArchive(option);
        }
    }
#endif // TAP_CLOUDSAVE_ENABLE
    #endregion
}

#endif