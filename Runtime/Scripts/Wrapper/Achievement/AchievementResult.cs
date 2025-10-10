using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    [Preserve]
    public class AchievementResult
    {
        /// <summary>
        /// 成就ID
        /// </summary>
        public string achievementId;

        /// <summary>
        /// 成就名称
        /// </summary>
        public string achievementName;

        /// <summary>
        /// 成就类型，分为普通成就 `AchievementType.NORMAL` 、白金成就 `AchievementType.PLATINUM`
        /// </summary>
        public string achievementType;

        /// <summary>
        /// 当前成就进度，如果不是分步式成就该值为 0
        /// </summary>
        public int currentSteps;
    }

    public enum AchievementType
    {
        /// <summary>
        /// 普通成就
        /// </summary>
        NORMAL,

        /// <summary>
        /// 分步式成就
        /// </summary>
        PLATINUM
    }
}