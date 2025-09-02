#nullable enable
using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 创建存档选项
    /// </summary>
    [Preserve]
    public class CreateArchiveOption
    {
        /// <summary>
        /// 存档名称（必需）
        /// </summary>
        public string name;
        
        /// <summary>
        /// 存档描述
        /// </summary>
        public string? description;
        
        /// <summary>
        /// 存档数据（将存储在ArchiveMetaData.extra中）
        /// </summary>
        public string? data;
        
        /// <summary>
        /// 存档文件路径（必需）- 存档文件的路径，单个存档文件大小不超过10MB
        /// </summary>
        public string archiveFilePath;
        
        /// <summary>
        /// 存档封面文件路径（可选）- 封面大小不超过512KB
        /// </summary>
        public string? archiveCoverPath;
        
        /// <summary>
        /// 游戏时间（秒数，可选）
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