#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    [Preserve]
    public class LoadLeaderboardScoresOption
    {
        /// <summary>
        /// 排行榜ID
        /// </summary>
        [Preserve]
        public string leaderboardId;

        /// <summary>
        /// 首次请求传null
        /// </summary>
        [Preserve]
        public string? nextPage;

        /// <summary>
        /// 限制
        /// </summary>
        [Preserve]
        public string collection;

        /// <summary>
        /// 周期标识（可为空）
        /// </summary>
        [Preserve]
        public string? periodToken;

        [Preserve]
        public LeaderboardCallback<LeaderboardScoreResponse> callback;
    }

}