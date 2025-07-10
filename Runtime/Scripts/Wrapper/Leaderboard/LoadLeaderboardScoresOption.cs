#nullable enable
using System;
using System.Collections.Generic;

namespace TapTapMiniGame
{
    public class LoadLeaderboardScoresOption
    {
        /// <summary>
        /// 排行榜ID
        /// </summary>
        public string leaderboardId;

        /// <summary>
        /// 首次请求传null
        /// </summary>
        public string? nextPage;

        /// <summary>
        /// 限制
        /// </summary>
        public string collection;

        /// <summary>
        /// 周期标识（可为空）  
        /// </summary>
        public string? periodToken;

        public LeaderboardCallback<LeaderboardScoreResponse> callback;
    }
    
}