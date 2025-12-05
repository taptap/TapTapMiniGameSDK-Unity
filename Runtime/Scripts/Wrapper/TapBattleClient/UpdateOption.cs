using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 更新玩家自定义状态选项 - 标准Option模式（双重回调机制）
    /// 对应Native: TapSdkOnlineBattleUpdatePlayerCustomStatus() -> int返回值 + UpdatePlayerCustomStatusCB异步回调
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class UpdatePlayerCustomStatusOption
    {
        /// <summary>
        /// 玩家自定义状态（整型数值）
        /// </summary>
        public int status;
        
        // === 立即回调（对应Native返回值）===
        
        /// <summary>
        /// 参数验证成功回调 - 对应Native返回0
        /// </summary>
        [System.NonSerialized]
        public Action<UpdateValidationResponse> success;
        
        /// <summary>
        /// 参数验证失败回调 - 对应Native返回-1/-2
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> fail;
        
        /// <summary>
        /// 完成回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> complete;
        
    }

    /// <summary>
    /// 更新玩家自定义属性选项 - 标准Option模式（双重回调机制）
    /// 对应Native: TapSdkOnlineBattleUpdatePlayerCustomProperties() -> int返回值 + UpdatePlayerCustomPropertiesCB异步回调
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class UpdatePlayerCustomPropertiesOption
    {
        /// <summary>
        /// 玩家自定义属性（utf8字符串，最大2048字节）
        /// </summary>
        public string properties;
        
        // === 立即回调（对应Native返回值）===
        
        /// <summary>
        /// 参数验证成功回调 - 对应Native返回0
        /// </summary>
        [System.NonSerialized]
        public Action<UpdateValidationResponse> success;
        
        /// <summary>
        /// 参数验证失败回调 - 对应Native返回-1/-2
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> fail;
        
        /// <summary>
        /// 完成回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> complete;
        
        // 注意：实际更新结果通过ITapBattleEventHandler.OnUpdatePlayerCustomPropertiesResult异步回调
    }

    /// <summary>
    /// 更新房间自定义属性选项 - 标准Option模式（双重回调机制）
    /// 对应Native: TapSdkOnlineBattleUpdateRoomCustomProperties() -> int返回值 + UpdateRoomCustomPropertiesCB异步回调
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class UpdateRoomCustomPropertiesOption
    {
        /// <summary>
        /// 房间自定义属性（utf8字符串，最大2048字节）
        /// </summary>
        public string properties;
        
        // === 立即回调（对应Native返回值）===
        
        /// <summary>
        /// 参数验证成功回调 - 对应Native返回0
        /// </summary>
        [System.NonSerialized]
        public Action<UpdateValidationResponse> success;
        
        /// <summary>
        /// 参数验证失败回调 - 对应Native返回-1/-2
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> fail;
        
        /// <summary>
        /// 完成回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> complete;
        
        // 注意：实际更新结果通过ITapBattleEventHandler.OnUpdateRoomCustomPropertiesResult异步回调
    }

    /// <summary>
    /// 更新房间属性数据
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class UpdateRoomPropertiesData
    {
        /// <summary>
        /// 房间名称
        /// </summary>
        public string name;
        
        /// <summary>
        /// 房间自定义属性（utf8字符串，最大2048字节）
        /// </summary>
        public string customProperties;
        
        [Preserve]
        public UpdateRoomPropertiesData() { }
    }

    /// <summary>
    /// 更新房间属性选项 - 标准Option模式（双重回调机制）
    /// 对应Native: TapSdkOnlineBattleUpdateRoomProperties() -> int返回值 + UpdateRoomPropertiesCB异步回调
    /// </summary>
    [Preserve]
    [System.Serializable]
    public class UpdateRoomPropertiesOption
    {
        /// <summary>
        /// 更新房间属性数据，包含name和customProperties
        /// </summary>
        public UpdateRoomPropertiesData data;
        
        /// <summary>
        /// 房间名称
        /// 注意：这个字段已废弃，请使用data.name
        /// </summary>
        [System.NonSerialized]
        public string name;
        
        /// <summary>
        /// 参数验证成功回调 - 对应Native返回0
        /// </summary>
        [System.NonSerialized]
        public Action<UpdateValidationResponse> success;
        
        /// <summary>
        /// 参数验证失败回调 - 对应Native返回-1/-2
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> fail;
        
        /// <summary>
        /// 完成回调
        /// </summary>
        [System.NonSerialized]
        public Action<TapCallbackResult> complete;
        
        // 注意：实际更新结果通过ITapBattleEventHandler.OnUpdateRoomPropertiesResult异步回调
    }
}
