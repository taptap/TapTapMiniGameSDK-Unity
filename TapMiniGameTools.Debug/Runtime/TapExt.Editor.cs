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
            TapDebugBridge.GetLeaderBoardManager();
        }

        public static void OpenLeaderboard(OpenLeaderboardOption option)
        {
            TapDebugBridge.OpenLeaderboard(option);
        }

        public static void SubmitScore(SubmitScoreOption option)
        {
            TapDebugBridge.SubmitScores(option);
        }

        public static void LoadLeaderboardScores(LoadLeaderboardScoresOption option)
        {
            TapDebugBridge.LoadLeaderboardScores(option);
        }

        public static void LoadCurrentPlayerLeaderboardScore(LoadCurrentPlayerLeaderboardScoreOption option)
        {
            TapDebugBridge.LoadCurrentPlayerLeaderboardScore(option);
        }

        public static void LoadPlayerCenteredScores(LoadPlayerCenteredScoresOption option)
        {
            TapDebugBridge.LoadPlayerCenteredScores(option);
        }
    }
    #region 成就
    public class TapAchievement {
        public static void Setup(AchievementManagerOption option)
        {
            TapDebugBridge.SetupAchievementManager(option);
        }
        public static void RegisterCallBack(AchievementListener listener)
        {
            TapDebugBridge.AchievementManager_RegisterListener(listener);
        }
        public static void UnRegisterCallBack(AchievementListener listener)
        {
            TapDebugBridge.AchievementManager_UnregisterListener(listener);
        }
        public static void Unlock(string achievementId)
        {
            TapDebugBridge.AchievementManager_Unlock(achievementId);
        }
        public static void Increment(string achievementId, int steps)
        {
            TapDebugBridge.AchievementManager_IncrementSteps(achievementId, steps);
        }
        public static void ShowAchievements()
        {
            TapDebugBridge.AchievementManager_ShowAllAchievements();
        }
        public static void SetToastEnable(bool enabled)
        {
            TapDebugBridge.AchievementManager_SetToastEnabled(enabled);
        }
    }
    #endregion
}

#endif