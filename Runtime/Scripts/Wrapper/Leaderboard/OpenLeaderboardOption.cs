#nullable enable
using System;

namespace TapTapMiniGame
{
    public class OpenLeaderboardOption
    {
        /// <summary>
        /// 排行榜ID
        /// </summary>
        public string leaderboardId;

        /// <summary>
        /// 榜单类型，比如"public"是总榜, "friends"是好友榜
        /// </summary>
        public string collection;
        
        
        public OpenLeaderboardCallback? callback;
    }
    
    public class OpenLeaderboardCallback
    {
        public Action<string?> onSuccess;

        public Action<int, string> onFailure;
    }
}