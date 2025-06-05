using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
namespace TapTapMiniGame
{
    public class HasHomeScreenWidgetOption
    {
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
        public Action<HasHomeScreenWidgetOptionResult> success;
    }
}