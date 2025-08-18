#nullable enable
using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 获取存档数据选项
    /// </summary>
    [Preserve]
    public class GetArchiveDataOption
    {
        /// <summary>
        /// 存档ID (UUID)
        /// </summary>
        public string archiveId;
        
        /// <summary>
        /// 文件ID
        /// </summary>
        public string? fileId;
        
        /// <summary>
        /// 目标文件路径（可选）
        /// </summary>
        public string? targetFilePath;
        
        /// <summary>
        /// 成功回调函数
        /// </summary>
        public Action<CloudArchiveDataResponse>? success;
        
        /// <summary>
        /// 失败回调函数
        /// </summary>
        public Action<int, string>? fail;
    }
}