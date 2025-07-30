using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    [Preserve]
    public class ShareMessageParam
    {
        /// <summary>
        /// 分享标题
        /// </summary>
        public string title;

        /// <summary>
        /// 分享文案
        /// </summary>
        public string desc;

        /// <summary>
        /// 透传字段，可在唤起时通过 onShow 函数获取
        /// </summary>
        public string query;

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