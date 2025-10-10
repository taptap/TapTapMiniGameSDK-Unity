using System.Collections.Generic;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 多人对战错误码常量
    /// </summary>
    [Preserve]
    public static class TapBattleErrorCodes
    {
        public const int SUCCESS = 0;              // 成功
        public const int NETWORK_ERROR = 6;        // 网络错误
        public const int NOT_IN_ROOM = 20;         // 未在房间中
        public const int NOT_ROOM_OWNER = 22;      // 不是房主
        public const int BATTLE_NOT_STARTED = 25;  // 对战未开始
        public const int PLAYER_NOT_FOUND = 30;    // 玩家不存在
        
        /// <summary>
        /// 多人对战错误码字典 - 基于Native API文档
        /// </summary>
        private static readonly Dictionary<int, string> ErrorDescriptions = new Dictionary<int, string>
        {
            { SUCCESS, "操作成功" },
            { NETWORK_ERROR, "网络连接错误，请检查网络状态" },
            { NOT_IN_ROOM, "用户未在房间中，请先加入房间" },
            { NOT_ROOM_OWNER, "当前用户不是房主，无权执行此操作" },
            { BATTLE_NOT_STARTED, "对战尚未开始，请等待对战开始" },
            { PLAYER_NOT_FOUND, "指定的玩家不存在" },
            { -1, "未知错误或系统异常" }
        };
        
        /// <summary>
        /// 根据错误码获取友好的错误描述
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns>错误描述</returns>
        public static string GetErrorDescription(int errorCode)
        {
            if (ErrorDescriptions.TryGetValue(errorCode, out string description))
            {
                return description;
            }
            return $"未知错误码: {errorCode}";
        }
    }
}
