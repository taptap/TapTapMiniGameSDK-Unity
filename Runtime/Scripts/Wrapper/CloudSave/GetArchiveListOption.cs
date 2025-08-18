#nullable enable
using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 获取存档列表选项
    /// </summary>
    [Preserve]
    public class GetArchiveListOption
    {
        /// <summary>
        /// 分页大小，默认10
        /// </summary>
        public int? pageSize;
        
        /// <summary>
        /// 页码，从0开始，默认0
        /// </summary>
        public int? pageIndex;
        
        /// <summary>
        /// 排序方式 ("createTime" | "modifyTime")，默认按修改时间排序
        /// </summary>
        public string? sortBy;
        
        /// <summary>
        /// 排序顺序 ("asc" | "desc")，默认降序
        /// </summary>
        public string? sortOrder;
        
        /// <summary>
        /// 成功回调函数
        /// </summary>
        public Action<CloudArchiveListResponse>? success;
        
        /// <summary>
        /// 失败回调函数
        /// </summary>
        public Action<int, string>? fail;
    }
}