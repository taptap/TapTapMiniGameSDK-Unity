#nullable enable
using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 当前用户的分数信息
    /// </summary>
    [Preserve]
    public class Score
    {
        /// <summary>
        /// 用户排名
        /// </summary>
        [Preserve]
        public long? rank;

        /// <summary>
        /// 排名展示文本
        /// </summary>
        [Preserve]
        public string? rankDisplay;

        /// <summary>
        /// 用户分数
        /// </summary>
        [Preserve]
        public long? score;

        /// <summary>
        /// 分数展示文本
        /// </summary>
        [Preserve]
        public string? scoreDisplay;

        /// <summary>
        /// 分数提交时间
        /// </summary>
        [Preserve]
        public long? scoreSubmittedTime;

        /// <summary>
        /// 用户信息
        /// </summary>
        [Preserve]
        public User? user;

        /// <summary>
        /// 用户信息
        /// </summary>
        [Preserve]
        public class User
        {
            /// <summary>
            /// 用户头像信息
            /// </summary>
            [Preserve]
            public Avatar? avatar;

            /// <summary>
            /// 用户名称
            /// </summary>
            [Preserve]
            public string? name;

            /// <summary>
            /// 开放平台ID
            /// </summary>
            [Preserve]
            public string? openid;

            /// <summary>
            /// 统一ID
            /// </summary>
            [Preserve]
            public string? unionid;
            
            /// <summary>
            /// 用户头像信息
            /// </summary>
            [Preserve]
            public class Avatar
            {
                /// <summary>
                /// 头像颜色
                /// </summary>
                [Preserve]
                public string? color;

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
                /// 头像URL
                /// </summary>
                [Preserve]
                public string? url;
            }
        }
    }
}
