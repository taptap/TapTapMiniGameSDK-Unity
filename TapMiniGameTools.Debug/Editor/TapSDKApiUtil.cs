using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System.Linq;

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
            "complete"
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
    }

    public class TsUtil : TapSDKApiUtil
    {
        
    }
        
}