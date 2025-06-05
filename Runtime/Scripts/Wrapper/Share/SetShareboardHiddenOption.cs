using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    [Preserve]
    public class SetShareboardHiddenOption
    {
        /// <summary>
        /// 是否隐藏分享面板
        /// </summary>
        public bool hidden;

        /// <summary>
        /// 接口调用结束的回调函数（调用成功、失败都会执行）
        /// </summary>
        public Action<TapCallbackResult> complete;

        /// <summary>
        /// 接口调用失败的回调函数
        /// </summary>
        public Action<TapCallbackResult> fail;

        /// <summary>
        /// 接口调用成功的回调函数
        /// </summary>
        public Action<TapCallbackResult> success;
    }
}