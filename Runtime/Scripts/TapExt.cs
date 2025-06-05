#if UNITY_WEBGL || WEIXINMINIGAME || UNITY_EDITOR
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
        
        public static void OnShareMessage(Action<Action<ShareMessageParam>, string> action)
        {
            TapExtManagerHandler.Instance.OnShareMessage(action);
        }

        public static void OffShareMessage()
        {
            TapExtManagerHandler.Instance.OffShareMessage();
        }
        
        public static void CreateHomeScreenWidget(CreateHomeScreenWidgetOption option)
        {
            TapExtManagerHandler.Instance.CreateHomeScreenWidget(option);
        }
        
        public static void HasHomeScreenWidgetAndPinned(HasHomeScreenWidgetOption option)
        {
            TapExtManagerHandler.Instance.HasHomeScreenWidgetAndPinned(option);
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
}

#endif