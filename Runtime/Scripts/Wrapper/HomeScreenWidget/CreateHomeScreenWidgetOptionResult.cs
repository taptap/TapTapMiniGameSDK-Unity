using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    [Preserve]
    public class CreateHomeScreenWidgetOptionResult
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public string errNo;

        /// <summary>
        /// 调用结果
        /// </summary>
        public string errMsg;
    }

    public enum createHomeScreenWidgetErrorCode
    {
        AppIdInvalid = 1518001, // 1518001: appId 无效
        GameAlreadyAdded = 1518002, // 1518002: 重复添加
        UserPermissionDenied = 1518003, // 1518003: 用户未授权
        GetRemoteServiceError = 1518004, // 1518004: 获取主进程服务失败
        UnknownError = 1518005 // 1518005: 未知错误
    }
}