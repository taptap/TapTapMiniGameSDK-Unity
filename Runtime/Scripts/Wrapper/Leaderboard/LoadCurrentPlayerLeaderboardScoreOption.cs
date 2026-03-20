#nullable enable
using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    [Preserve]
    public class LoadCurrentPlayerLeaderboardScoreOption
    {
        [Preserve]
        public string leaderboardId;

        [Preserve]
        public string collection;

        [Preserve]
        public string? periodToken;

        [Preserve]
        public LeaderboardCallback<UserScoreResponse> callback;
    }
}
