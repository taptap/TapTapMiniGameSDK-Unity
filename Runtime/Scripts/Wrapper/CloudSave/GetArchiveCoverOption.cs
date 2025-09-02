#nullable enable
using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 获取存档封面选项
    /// </summary>
    [Preserve]
    public class GetArchiveCoverOption
    {
        /// <summary>
        /// 存档ID
        /// </summary>
        public string archiveId;
        
        /// <summary>
        /// 成功回调函数
        /// </summary>
        public Action<CloudArchiveCoverResponse>? success;
        
        /// <summary>
        /// 失败回调函数
        /// </summary>
        public Action<int, string>? fail;
    }
}