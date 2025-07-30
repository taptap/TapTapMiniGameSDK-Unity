using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    [Preserve]
    public class OnShareMessageListenerResult
    {
        /// <summary>
        /// 分享渠道
        /// </summary>
        public string channel;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errMsg;
    }
}