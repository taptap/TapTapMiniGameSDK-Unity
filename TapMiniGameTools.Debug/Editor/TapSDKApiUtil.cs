using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace TapTapMiniGame
{
    public class TapSDKApiUtil
    {
        /// <summary>
        /// 延迟执行Action，默认等待1帧
        /// </summary>
        /// <param name="action">要执行的Action</param>
        /// <param name="delay">延迟时间（秒），默认等待1帧</param>
        public static void DelayCall(Action action, float delay = -1)
        {
            TapSDKManagerHandler.Instance.StartCoroutine(DelayCoroutine(action, delay));
        }

        /// <summary>
        /// 已知的回调函数字段名称列表
        /// </summary>
        private static readonly HashSet<string> CallbackFieldNames = new HashSet<string>
        {
            "success",
            "fail", 
            "complete",
            "callback"
        };

        /// <summary>
        /// 添加新的回调字段名称（如果将来需要扩展）
        /// </summary>
        /// <param name="fieldName">要添加的字段名称</param>
        public static void AddCallbackFieldName(string fieldName)
        {
            if (!string.IsNullOrEmpty(fieldName))
            {
                CallbackFieldNames.Add(fieldName);
                Debug.Log($"[TapSDKApiUtil] 添加回调字段名称: {fieldName}");
            }
        }

        /// <summary>
        /// 获取当前所有的回调字段名称
        /// </summary>
        /// <returns>回调字段名称列表</returns>
        public static string[] GetCallbackFieldNames()
        {
            return CallbackFieldNames.ToArray();
        }

        /// <summary>
        /// 创建一个不包含函数字段的对象副本，用于JSON序列化
        /// </summary>
        /// <param name="source">源对象</param>
        /// <returns>排除函数字段后的对象</returns>
        public static object CreateSerializableObject(object source)
        {
            if (source == null) return null;

            var sourceType = source.GetType();
            var result = new Dictionary<string, object>();

            try
            {
                // 获取所有公共字段
                var fields = sourceType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    // 跳过已知的回调函数字段
                    if (CallbackFieldNames.Contains(field.Name))
                    {
                        continue;
                    }
                    
                    var value = field.GetValue(source);
                    result[field.Name] = value;
                }

                // 获取所有公共属性
                var properties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    // 跳过已知的回调函数字段和不可读属性
                    if (CallbackFieldNames.Contains(property.Name) || !property.CanRead)
                    {
                        continue;
                    }
                    
                    try
                    {
                        var value = property.GetValue(source);
                        result[property.Name] = value;
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"[TapSDKApiUtil] 获取属性 {property.Name} 失败: {e.Message}");
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"[TapSDKApiUtil] 创建可序列化对象失败: {e.Message}");
                return new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// 将对象序列化为JSON字符串，自动排除函数字段
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>JSON字符串</returns>
        public static string ToJsonExcludingCallbacks(object obj)
        {
            if (obj == null) return "null";

            try
            {
                var serializableObj = CreateSerializableObject(obj);
                return JsonMapper.ToJson(serializableObj);
            }
            catch (Exception e)
            {
                Debug.LogError($"[TapSDKApiUtil] JSON序列化失败: {e.Message}");
                return "{}";
            }
        }

        /// <summary>
        /// 安全的JSON序列化方法，自动排除函数字段 (简化别名)
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>JSON字符串</returns>
        public static string SafeToJson(object obj)
        {
            return ToJsonExcludingCallbacks(obj);
        }
        
        /// <summary>
        /// 通用的成功回调调用方法，自动创建空的成功结果对象
        /// </summary>
        /// <param name="successCallback">success回调</param>
        /// <param name="message">成功消息</param>
        public static void InvokeSuccessCallback(object successCallback, string message = "Editor Success")
        {
            if (successCallback == null) return;

            try
            {
                // 获取Action的泛型参数类型
                var callbackType = successCallback.GetType();
                if (callbackType.IsGenericType && callbackType.GetGenericTypeDefinition() == typeof(Action<>))
                {
                    var parameterType = callbackType.GetGenericArguments()[0];
                    
                    // 创建参数类型的实例
                    var resultInstance = Activator.CreateInstance(parameterType);
                    
                    // 尝试设置errMsg属性
                    var errMsgProperty = parameterType.GetProperty("errMsg");
                    if (errMsgProperty != null && errMsgProperty.CanWrite)
                    {
                        errMsgProperty.SetValue(resultInstance, message);
                    }
                    
                    // 调用回调
                    callbackType.GetMethod("Invoke").Invoke(successCallback, new[] { resultInstance });
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[TapSDKApiUtil] 调用success回调失败: {e.Message}");
            }
        }

        /// <summary>
        /// 通用的完成回调调用方法，自动创建空的完成结果对象
        /// </summary>
        /// <param name="completeCallback">complete回调</param>
        /// <param name="message">完成消息</param>
        public static void InvokeCompleteCallback(object completeCallback, string message = "Editor Complete")
        {
            if (completeCallback == null) return;

            try
            {
                // 获取Action的泛型参数类型
                var callbackType = completeCallback.GetType();
                if (callbackType.IsGenericType && callbackType.GetGenericTypeDefinition() == typeof(Action<>))
                {
                    var parameterType = callbackType.GetGenericArguments()[0];
                    
                    // 创建参数类型的实例
                    var resultInstance = Activator.CreateInstance(parameterType);
                    
                    // 尝试设置errMsg属性
                    var errMsgProperty = parameterType.GetProperty("errMsg");
                    if (errMsgProperty != null && errMsgProperty.CanWrite)
                    {
                        errMsgProperty.SetValue(resultInstance, message);
                    }
                    
                    // 调用回调
                    callbackType.GetMethod("Invoke").Invoke(completeCallback, new[] { resultInstance });
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[TapSDKApiUtil] 调用complete回调失败: {e.Message}");
            }
        }

        private static IEnumerator DelayCoroutine(Action action, float delay)
        {
            if (delay <= 0)
                yield return null; // 等待1帧
            else
                yield return new WaitForSeconds(delay); // 等待指定时间

            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError($"[TapSDKApiUtil] Action执行异常: {e.Message}");
            }
        }

        /// <summary>
        /// 获取本地IP地址的通用静态方法
        /// 优先级：NetworkInterface方法 > DNS方法 > 127.0.0.1
        /// </summary>
        /// <returns>检测到的本地IP地址</returns>
        public static string GetLocalIPAddress()
        {
            // 优先级1: 尝试NetworkInterface方法
            string networkInterfaceIP = TryGetIPFromNetworkInterface();
            if (!string.IsNullOrEmpty(networkInterfaceIP))
            {
                return networkInterfaceIP;
            }

            // 优先级2: 尝试DNS方法
            string dnsIP = TryGetIPFromDNS();
            if (!string.IsNullOrEmpty(dnsIP))
            {
                return dnsIP;
            }

            // 最后fallback
            Debug.LogError("[TapSDKApiUtil] All IP detection methods failed, using localhost");
            return "127.0.0.1";
        }

        /// <summary>
        /// 通过NetworkInterface获取本地IP地址
        /// </summary>
        /// <returns>找到的IP地址，失败时返回null</returns>
        private static string TryGetIPFromNetworkInterface()
        {
            try
            {
                var localIPs = new List<IPAddress>();

                // 遍历所有活动的网络接口
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    // 只考虑运行中且非环回的接口
                    if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        // 获取接口的IP地址信息
                        foreach (UnicastIPAddressInformation ipInfo in ni.GetIPProperties().UnicastAddresses)
                        {
                            // 筛选IPv4地址
                            if (ipInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                // 排除回环地址
                                if (!IPAddress.IsLoopback(ipInfo.Address))
                                {
                                    localIPs.Add(ipInfo.Address);
                                }
                            }
                        }
                    }
                }

                if (localIPs.Count > 0)
                {
                    // 按优先级排序选择最佳IP
                    var selectedIP = localIPs
                        .OrderByDescending(ip =>
                        {
                            var bytes = ip.GetAddressBytes();
                            // 优先级：192.168.x.x > 10.x.x.x > 172.16-31.x.x > 其他
                            if (bytes[0] == 192 && bytes[1] == 168) return 3;
                            if (bytes[0] == 10) return 2;
                            if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) return 1;
                            return 0;
                        })
                        .First();

                    string ipAddress = selectedIP.ToString();
                    Debug.Log($"[TapSDKApiUtil] NetworkInterface method found IP: {ipAddress}");

                    // 如果找到多个IP，记录所有找到的IP
                    if (localIPs.Count > 1)
                    {
                        string allIPs = string.Join(", ", localIPs.Select(ip => ip.ToString()));
                        Debug.Log($"[TapSDKApiUtil] Available IPs: {allIPs}, selected: {ipAddress}");
                    }

                    return ipAddress;
                }

                Debug.LogWarning("[TapSDKApiUtil] NetworkInterface method: No valid IP addresses found");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[TapSDKApiUtil] NetworkInterface method failed: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 通过DNS获取本地IP地址
        /// </summary>
        /// <returns>找到的IP地址，失败时返回null</returns>
        private static string TryGetIPFromDNS()
        {
            try
            {
                // 优化的DNS方法，增加更好的错误处理
                string hostname = Environment.MachineName;
                if (string.IsNullOrEmpty(hostname))
                {
                    hostname = Dns.GetHostName();
                }

                Debug.Log($"[TapSDKApiUtil] DNS method trying to resolve hostname: {hostname}");

                var hostEntry = Dns.GetHostEntry(hostname);
                var dnsIPs = hostEntry.AddressList
                    .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                    .Where(ip => !IPAddress.IsLoopback(ip))
                    .OrderByDescending(ip =>
                    {
                        var bytes = ip.GetAddressBytes();
                        if (bytes[0] == 192 && bytes[1] == 168) return 3;
                        if (bytes[0] == 10) return 2;
                        if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) return 1;
                        return 0;
                    })
                    .ToList();

                if (dnsIPs.Any())
                {
                    string ipAddress = dnsIPs.First().ToString();
                    Debug.Log($"[TapSDKApiUtil] DNS method found IP: {ipAddress}");
                    return ipAddress;
                }

                Debug.LogWarning("[TapSDKApiUtil] DNS method: No valid IP addresses found in DNS resolution");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[TapSDKApiUtil] DNS method failed: {e.Message}");
                return null;
            }
        }
    }

    public class TsUtil : TapSDKApiUtil
    {
        
    }
        
}