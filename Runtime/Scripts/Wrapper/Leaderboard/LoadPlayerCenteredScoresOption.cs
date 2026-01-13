#nullable enable
using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    [Preserve]
    public class LoadPlayerCenteredScoresOption
    {
        [Preserve]
        public string leaderboardId;

        [Preserve]
        public string collection;

        [Preserve]
        public string? periodToken = null;

        [Preserve]
        public int? maxCount = null;

        [Preserve]
        public LeaderboardCallback<LeaderboardScoreResponse> callback;
    }
}