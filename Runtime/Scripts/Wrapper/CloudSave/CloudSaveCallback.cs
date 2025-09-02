using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 云存档失败响应的消息结构
    /// </summary>
    [Preserve]
    public class CloudSaveErrorMessage
    {
        public CloudSaveErrorMessage() { }
        
        [Preserve]
        public string errMsg;
        [Preserve]
        public int errno;
    }

    /// <summary>
    /// 云存档失败响应
    /// </summary>
    [Preserve]
    public class CloudSaveFailureResponse
    {
        public CloudSaveFailureResponse() { }
        
        [Preserve]
        public int code;
        [Preserve]
        public CloudSaveErrorMessage message;
    }

    /// <summary>
    /// 云存档通用回调
    /// </summary>
    [Preserve]
    public class CloudSaveCallback<T>
    {
        [Preserve]
        public Action<T> onSuccess;
        [Preserve]
        public Action<int, string> onFailure;
    }
}