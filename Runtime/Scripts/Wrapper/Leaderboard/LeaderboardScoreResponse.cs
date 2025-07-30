#nullable enable
using System.Collections.Generic;
using UnityEngine.Scripting;
using System;

namespace TapTapMiniGame
{
    /// <summary>
    /// 排行榜分数响应数据
    /// </summary>
    [Preserve]
    public class LeaderboardScoreResponse
    {

        /// <summary>
        /// 排行榜信息
        /// </summary>
        [Preserve]
        public Leaderboard? leaderboard;

        /// <summary>
        /// 分数列表
        /// </summary>
        [Preserve]
        public List<Score>? scores;

        /// <summary>
        /// 一页标识
        /// </summary>
        [Preserve]
        public string? nextPage;
    }
}


