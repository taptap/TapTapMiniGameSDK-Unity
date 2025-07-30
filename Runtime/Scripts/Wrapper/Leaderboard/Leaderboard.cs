#nullable enable
using System.Collections.Generic;
using UnityEngine.Scripting;
using System;

namespace TapTapMiniGame
{


    /// <summary>
    /// 排行榜信息
    /// </summary>
    [Preserve]
    public class Leaderboard
    {
        /// <summary>
        /// 可用的时间周期
        /// </summary>
        [Preserve]
        public List<Period> availablePeriods = new List<Period>();

        /// <summary>
        /// 排行榜背景
        /// </summary>
        [Preserve]
        public Background? background;

        /// <summary>
        /// 排行榜ID
        /// </summary>
        [Preserve]
        public string id;

        /// <summary>
        /// 排行榜名称
        /// </summary>
        [Preserve]
        public string name;

        /// <summary>
        /// 当前周期
        /// </summary>
        [Preserve]
        public Period? period;

        /// <summary>
        /// 排行榜分数
        /// </summary>
        [Preserve]
        public Score? score;

        /// <summary>
        /// 时间周期
        /// </summary>
        [Preserve]
        public class Period
        {
            /// <summary>
            /// 展示文本
            /// </summary>
            [Preserve]
            public string? display;

            /// <summary>
            /// 周期标识
            /// </summary>
            [Preserve]
            public string? periodToken;
        }
        
        /// <summary>
        /// 排行榜背景信息
        /// </summary>
        [Preserve]
        public class Background
        {
            /// <summary>
            /// 背景颜色
            /// </summary>
            [Preserve]
            public string? color;

            /// <summary>
            /// 背景高度
            /// </summary>
            [Preserve]
            public int? height;

            /// <summary>
            /// 中等尺寸URL
            /// </summary>
            [Preserve]
            public string? mediumUrl;

            /// <summary>
            /// 原始格式
            /// </summary>
            [Preserve]
            public string? originalFormat;

            /// <summary>
            /// 原始大小
            /// </summary>
            [Preserve]
            public long? originalSize;

            /// <summary>
            /// 原始URL
            /// </summary>
            [Preserve]
            public string? originalUrl;

            /// <summary>
            /// 小尺寸URL
            /// </summary>
            [Preserve]
            public string? smallUrl;

            /// <summary>
            /// 背景URL
            /// </summary>
            [Preserve]
            public string? url;

            /// <summary>
            /// 背景宽度
            /// </summary>
            [Preserve]
            public int? width;
        }
    }
} 