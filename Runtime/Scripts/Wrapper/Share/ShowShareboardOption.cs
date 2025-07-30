using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    [Preserve]
    public class ShowShareboardOption
    {
        /// <summary>
        /// 分享模板id，必传
        /// </summary>
        public string templateId;
        
        /// <summary>
        /// 分享场景参数，用户打开分享卡片会将这个参数透传到小游戏
        /// </summary>
        public string sceneParam;

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