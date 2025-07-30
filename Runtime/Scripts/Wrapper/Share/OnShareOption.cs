using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    public class OnShareOption
    { 

        /// <summary>
        /// 分享成功的回调函数
        /// </summary>
        public Action<OnShareMessageListenerResult> success;

        /// <summary>
        /// 分享失败的回调函数
        /// </summary>
        public Action<OnShareMessageListenerResult> fail;

    }
}