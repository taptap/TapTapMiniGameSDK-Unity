using System.Collections.Generic;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 多人联机错误码常量
    /// </summary>
    [Preserve]
    public static class TapBattleErrorCodes
    {
        // 成功
        public const int SUCCESS = 0;                                   // 成功

        // 通用错误码
        public const int ERROR_SYSTEM_ERROR = 1;                        // 系统错误
        public const int ERROR_SDK_ERROR = 2;                           // SDK错误，可能内存分配失败、http对象创建失败等
        public const int ERROR_REQUEST_RATE_LIMIT_EXCEEDED = 3;        // 请求频率超限
        public const int ERROR_MALICIOUS_USER = 4;                      // 网关认定为恶意用户，拒绝请求或关闭连接。建议不要重连
        public const int ERROR_TOO_MANY_CONNECTIONS = 5;                // 因同时连接数过多而被踢下线。建议不要重连，避免互踢，导致重连死循环
        public const int ERROR_NETWORK_ERROR = 6;                       // 网络错误，可能是连接超时、断开等

        // 请求错误码
        public const int ERROR_INVALID_REQUEST = 11;                    // 请求不合法
        public const int ERROR_INVALID_AUTHORIZATION = 12;              // 认证信息不合法
        public const int ERROR_UNAUTHORIZED = 13;                       // 尚未完成登录认证
        public const int ERROR_ALREADY_CONNECTED = 14;                  // 已经登录，不能重复登录
        public const int ERROR_PREVIOUS_REQUEST_IN_PROGRESS = 15;       // 上一个请求未完成，不接受新的请求
        public const int ERROR_UNIMPLEMENTED = 16;                      // 请求了后端服务未实现的功能
        public const int ERROR_FORBIDDEN = 17;                          // 用户没有对当前动作的权限，引导重新身份验证并不能提供任何帮助，而且这个请求也不应该被重复提交

        // 房间相关错误码
        public const int ERROR_ROOM_TEMPLATE_NOT_FOUND = 18;            // 房间模板不存在
        public const int ERROR_ROOM_COUNT_LIMIT_EXCEEDED = 19;          // 房间总数量超过限制
        public const int ERROR_NOT_IN_ROOM = 20;                        // 尚未加入房间
        public const int ERROR_ALREADY_IN_ROOM = 21;                    // 已经在房间中，不能重复加入
        public const int ERROR_NOT_ROOM_OWNER = 22;                     // 不是房主，不能执行此操作
        public const int ERROR_ROOM_FULL = 23;                          // 房间已满，不能加入
        public const int ERROR_ROOM_NOT_EXIST = 24;                     // 房间不存在

        // 对战相关错误码
        public const int ERROR_FRAME_SYNC_NOT_STARTED = 25;             // 对战未开始，不能执行此操作
        public const int ERROR_FRAME_SYNC_ALREADY_STARTED = 26;         // 对战已开始，不能执行此操作
        public const int ERROR_FRAME_INPUT_SIZE_LIMIT_EXCEEDED = 28;    // 对战帧数据大小超过限制
        public const int ERROR_FRAME_INPUT_COUNT_LIMIT_EXCEEDED = 29;   // 每帧可接受的输入数量超过限制

        // 玩家相关错误码
        public const int ERROR_PLAYER_NOT_FOUND = 30;                   // 玩家不存在
        
        /// <summary>
        /// 多人联机错误码字典 - 基于Native API文档
        /// </summary>
        private static readonly Dictionary<int, string> ErrorDescriptions = new Dictionary<int, string>
        {
            // 成功
            { SUCCESS, "操作成功" },

            // 通用错误
            { ERROR_SYSTEM_ERROR, "系统错误，请稍后重试" },
            { ERROR_SDK_ERROR, "SDK内部错误，可能是内存分配失败或HTTP对象创建失败" },
            { ERROR_REQUEST_RATE_LIMIT_EXCEEDED, "请求频率超限，请降低请求频率" },
            { ERROR_MALICIOUS_USER, "网关认定为恶意用户，请求被拒绝，建议不要重连" },
            { ERROR_TOO_MANY_CONNECTIONS, "同时连接数过多被踢下线，建议不要重连，避免互踢导致重连死循环" },
            { ERROR_NETWORK_ERROR, "网络连接错误，可能是连接超时或断开，请检查网络状态" },

            // 请求错误
            { ERROR_INVALID_REQUEST, "请求不合法，请检查请求参数" },
            { ERROR_INVALID_AUTHORIZATION, "认证信息不合法，请重新登录" },
            { ERROR_UNAUTHORIZED, "尚未完成登录认证，请先登录" },
            { ERROR_ALREADY_CONNECTED, "已经登录，不能重复登录" },
            { ERROR_PREVIOUS_REQUEST_IN_PROGRESS, "上一个请求未完成，请等待当前请求完成" },
            { ERROR_UNIMPLEMENTED, "请求的功能后端服务尚未实现" },
            { ERROR_FORBIDDEN, "用户没有权限执行此操作，引导重新身份验证并不能提供任何帮助，而且这个请求也不应该被重复提交" },

            // 房间相关错误
            { ERROR_ROOM_TEMPLATE_NOT_FOUND, "房间模板不存在，请检查房间模板配置" },
            { ERROR_ROOM_COUNT_LIMIT_EXCEEDED, "房间总数量超过限制，请稍后再试" },
            { ERROR_NOT_IN_ROOM, "用户未在房间中，请先加入房间" },
            { ERROR_ALREADY_IN_ROOM, "已经在房间中，不能重复加入" },
            { ERROR_NOT_ROOM_OWNER, "当前用户不是房主，无权执行此操作" },
            { ERROR_ROOM_FULL, "房间已满，无法加入" },
            { ERROR_ROOM_NOT_EXIST, "房间不存在，请检查房间ID" },

            // 对战相关错误
            { ERROR_FRAME_SYNC_NOT_STARTED, "对战尚未开始，请等待对战开始" },
            { ERROR_FRAME_SYNC_ALREADY_STARTED, "对战已开始，无法执行此操作" },
            { ERROR_FRAME_INPUT_SIZE_LIMIT_EXCEEDED, "对战帧数据大小超过限制，请减小数据量" },
            { ERROR_FRAME_INPUT_COUNT_LIMIT_EXCEEDED, "每帧可接受的输入数量超过限制，请减少输入数量" },

            // 玩家相关错误
            { ERROR_PLAYER_NOT_FOUND, "指定的玩家不存在" }
        };
        
        /// <summary>
        /// 根据错误码获取友好的错误描述
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns>错误描述</returns>
        public static string GetErrorDescription(int errorCode)
        {
            if (ErrorDescriptions.TryGetValue(errorCode, out string description))
            {
                return description;
            }
            return $"未知错误码: {errorCode}";
        }
    }
}
