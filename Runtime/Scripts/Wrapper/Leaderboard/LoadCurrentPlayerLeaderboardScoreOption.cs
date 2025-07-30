#nullable enable
using System;

namespace TapTapMiniGame
{
    public class LoadCurrentPlayerLeaderboardScoreOption
    {
        public string leaderboardId;

        public string collection;

        public string? periodToken;

        public LeaderboardCallback<UserScoreResponse> callback;
    }
}
