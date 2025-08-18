#nullable enable
using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 删除存档选项
    /// </summary>
    [Preserve]
    public class DeleteArchiveOption
    {
        /// <summary>
        /// 存档ID
        /// </summary>
        public string archiveId;
        
        /// <summary>
        /// 成功回调函数
        /// </summary>
        public Action<CloudArchiveOperationResponse>? success;
        
        /// <summary>
        /// 失败回调函数
        /// </summary>
        public Action<int, string>? fail;
    }
}