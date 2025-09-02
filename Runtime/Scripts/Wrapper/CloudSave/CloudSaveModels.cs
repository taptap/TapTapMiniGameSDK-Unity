using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 云存档数据
    /// </summary>
    [Preserve]
    public class CloudArchive
    {
        
        /// <summary>
        /// 存档ID (UUID)
        /// </summary>
        public string archiveId;
        
        /// <summary>
        /// 文件ID
        /// </summary>
        public string fileId;
        
        /// <summary>
        /// 存档名称
        /// </summary>
        public string name;
        
        /// <summary>
        /// 存档描述
        /// </summary>
        public string description;
        
        /// <summary>
        /// 存档数据
        /// </summary>
        public string data;
        
        /// <summary>
        /// 存档封面图片数据（base64编码）
        /// </summary>
        public string cover;
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public long createTime;
        
        /// <summary>
        /// 修改时间
        /// </summary>
        public long modifyTime;
        
        /// <summary>
        /// 数据大小（字节）
        /// </summary>
        public int size;
    }

    /// <summary>
    /// 云存档列表响应
    /// </summary>
    [Preserve]
    public class CloudArchiveListResponse
    {
        
        /// <summary>
        /// 存档列表
        /// </summary>
        public List<CloudArchive> archives;
        
        /// <summary>
        /// 总数量
        /// </summary>
        public int total;
    }

    /// <summary>
    /// 存档数据响应
    /// </summary>
    [Preserve]
    public class CloudArchiveDataResponse
    {
        
        /// <summary>
        /// 存档数据
        /// </summary>
        public string data;
        
        /// <summary>
        /// 存档信息
        /// </summary>
        public CloudArchive archive;
    }

    /// <summary>
    /// 存档封面响应
    /// </summary>
    [Preserve]
    public class CloudArchiveCoverResponse
    {
        
        /// <summary>
        /// 封面图片数据（base64编码）
        /// </summary>
        public string cover;
        
        /// <summary>
        /// 存档ID
        /// </summary>
        public string archiveId;
    }

    /// <summary>
    /// 创建/更新/删除存档响应
    /// </summary>
    [Preserve]
    public class CloudArchiveOperationResponse
    {
        
        /// <summary>
        /// 存档ID (UUID)
        /// </summary>
        public string archiveId;
        
        /// <summary>
        /// 文件ID（删除操作时为null）
        /// </summary>
        public string fileId;
    }
}