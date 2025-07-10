#nullable enable
using System;

namespace TapTapMiniGame
{
    public class LoadPlayerCenteredScoresOption
    {
        public string leaderboardId;

        public string collection;

        public string? periodToken = null;

        public int? maxCount = null;

        public LeaderboardCallback<LeaderboardScoreResponse> callback;
    }
}