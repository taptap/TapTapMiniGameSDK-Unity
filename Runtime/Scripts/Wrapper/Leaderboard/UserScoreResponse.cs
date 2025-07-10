#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using System;

namespace TapTapMiniGame
{
    /// <summary>
    /// 用户分数响应数据
    /// </summary>
    [Preserve]
    [Serializable]
    public class UserScoreResponse
    {
        /// <summary>
        /// 当前用户的分数
        /// </summary>
        [Preserve]
        public Score? currentUserScore;

        /// <summary>
        /// 排行榜信息
        /// </summary>
        [Preserve]
        public Leaderboard? leaderboard;
    }
}

