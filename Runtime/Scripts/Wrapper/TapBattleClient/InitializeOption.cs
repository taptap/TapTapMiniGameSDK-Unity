using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 初始化选项 - 封装eventHandler和标准回调
    /// 对应Native: TapSdkOnlineBattleInitialize(callbacks) -> int返回值
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class InitializeOption
    {
        /// <summary>
        /// 事件处理器 - 对应Native callbacks参数
        /// </summary>
        [System.NonSerialized]
        public ITapBattleEventHandler eventHandler;
        
        // === 标准回调格式 ===
        
        /// <summary>
        /// 初始化成功回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> success;
        
        /// <summary>
        /// 初始化失败回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> fail;
        
        /// <summary>
        /// 初始化完成回调（无论成功失败都会调用）
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> complete;
    }
}
