#nullable enable
using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 更新存档选项
    /// </summary>
    [Preserve]
    public class UpdateArchiveOption
    {
        /// <summary>
        /// 存档ID
        /// </summary>
        public string archiveId;
        
        /// <summary>
        /// 存档名称
        /// </summary>
        public string? name;
        
        /// <summary>
        /// 存档描述
        /// </summary>
        public string? description;
        
        /// <summary>
        /// 存档数据
        /// </summary>
        public string? data;
        
        /// <summary>
        /// 存档封面图片数据（base64编码）
        /// </summary>
        public string? cover;
        
        /// <summary>
        /// 存档文件路径
        /// </summary>
        public string? archiveFilePath;
        
        /// <summary>
        /// 存档封面文件路径
        /// </summary>
        public string? archiveCoverPath;
        
        /// <summary>
        /// 游戏时间（秒）
        /// </summary>
        public int? playtime;
        
        /// <summary>
        /// 成功回调函数
        /// </summary>
        public Action<CloudArchiveOperationResponse>? success;
        
        /// <summary>
        /// 失败回调函数
        /// </summary>
        public Action<int, string>? fail;
        
        /// <summary>
        /// 完成回调函数（无论成功或失败都会调用）
        /// </summary>
        public Action<TapCallbackResult>? complete;
    }
}