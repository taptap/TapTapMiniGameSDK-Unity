#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    public class SubmitScoreOption
    {
        public List<ScoreItem> scores;
        
        public LeaderboardCallback<SubmitScoresResponse> callback;
        
    }
    
    public class ScoreItem
    {

        /// <summary>
        /// 排行榜ID
        /// </summary>
        public string leaderboardId;

        /// <summary>
        /// 分数
        /// </summary>
        public long score;
    }

    /// <summary>
    /// 提交分数响应类
    /// </summary>
    [Preserve]
    public class SubmitScoresResponse
    {
        /// <summary>
        /// 提交分数结果列表
        /// </summary>
        [Preserve]
        public List<Item> results = new List<Item>();

        /// <summary>
        /// 提交分数结果类
        /// </summary>
        [Preserve]
        public class Item
        {
            /// <summary>
            /// 排行榜 ID
            /// </summary>
            [Preserve]
            public string leaderboardId = "";

            /// <summary>
            /// 用户 ID
            /// </summary>
            [Preserve]
            public string openid = "";

            /// <summary>
            /// 周期 Token
            /// </summary>
            [Preserve]
            public string periodToken = "";

            /// <summary>
            /// 分数结果
            /// </summary>
            [Preserve]
            public Score? scoreResult;

            [Preserve]
            public string unionid = "";

            /// <summary>
            /// 分数结果类
            /// </summary>
            [Preserve]
            public class Score
            {
                /// <summary>
                /// 是否为新最佳分数
                /// </summary>
                [Preserve]
                public bool newBest = false;

                /// <summary>
                /// 原始分数
                /// </summary>
                [Preserve]
                public long rawScore = 0;

                /// <summary>
                /// 显示分数
                /// </summary>
                [Preserve]
                public string scoreDisplay = "";
            }
        }
    }
}