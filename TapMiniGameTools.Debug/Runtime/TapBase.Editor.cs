#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE

using System;
using UnityEngine;

namespace TapTapMiniGame
{
    /// <summary>
    /// 小游戏宿主SDK对外暴露的API
    /// </summary>
    public partial class Tap: TapBase
    {
        /// <summary>
        /// 重写基类的 env 属性，提供 Editor 环境下的实现
        /// 使用TapEnvCache提供真机环境的TapEnv数据
        /// </summary>
        public new static TapEnvDebug env
        {
            get
            {
                // 在 Editor 环境下返回增强的调试环境，支持真机数据缓存
                return new TapEnvDebug();
            }
        }

        /// <summary>
        /// 重写基类的 InitSDK 方法，提供 Editor 环境下的实现
        /// </summary>
        /// <param name="callback">初始化完成后的回调</param>
        public new static void InitSDK(System.Action<int> callback)
        {
            // Editor 环境下的初始化逻辑
            Debug.Log("TapBase.Editor: InitSDK called in Editor environment");
            
            // 模拟初始化成功
            try
            {
                callback?.Invoke(0);
                Debug.Log("TapBase.Editor: InitSDK completed successfully");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"TapBase.Editor: InitSDK failed with error: {ex.Message}");
                
                // 调用失败回调，传入错误状态码
                callback?.Invoke(-1);
            }
        }
    }
}
#endif