using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    [Preserve]
    public class LeaderboardFailureResponse
    {
        [Preserve]
        public int code;

        [Preserve]
        public string message;
    }

    [Preserve]
    public class LeaderboardCallback<T>
    {
        [Preserve]
        public Action<T> onSuccess;

        [Preserve]
        public Action<int, string> onFailure;
    }


}