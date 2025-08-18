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
}

#endif