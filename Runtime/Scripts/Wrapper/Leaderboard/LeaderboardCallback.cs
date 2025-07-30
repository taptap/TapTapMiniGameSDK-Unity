using System;

namespace TapTapMiniGame
{

    public class LeaderboardFailureResponse
    {
        public int code;

        public string message;
    }

    public class LeaderboardCallback<T>
    {
        public Action<T> onSuccess;

        public Action<int, string> onFailure;
    }
    

}