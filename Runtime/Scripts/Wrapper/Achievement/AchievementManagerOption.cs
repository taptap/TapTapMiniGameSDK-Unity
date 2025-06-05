using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    [Preserve]
    public class AchievementManagerOption
    {
        /// <summary>
        /// 成就达成时 SDK 是否需要展示一个气泡弹窗提示
        /// </summary>
        public bool enableToast;
    }
}