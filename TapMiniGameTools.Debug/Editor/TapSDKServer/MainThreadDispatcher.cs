#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace TapServer
{
    /// <summary>
    /// 主线程调度器
    /// 用于将子线程中的任务调度到主线程执行
    /// </summary>
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static MainThreadDispatcher _instance;
        private static readonly object _lock = new object();
        
        private readonly ConcurrentQueue<Action> _actionQueue = new ConcurrentQueue<Action>();
        private readonly List<Action> _currentActions = new List<Action>();

        public static MainThreadDispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            GameObject go = new GameObject("MainThreadDispatcher");
                            _instance = go.AddComponent<MainThreadDispatcher>();
                            DontDestroyOnLoad(go);
                        }
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            // 执行队列中的所有任务
            _currentActions.Clear();
            
            while (_actionQueue.TryDequeue(out Action action))
            {
                _currentActions.Add(action);
            }

            foreach (var action in _currentActions)
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"MainThreadDispatcher execution error: {e.Message}");
                }
            }
        }

        /// <summary>
        /// 将任务排队到主线程执行
        /// </summary>
        /// <param name="action">要执行的任务</param>
        public static void Enqueue(Action action)
        {
            if (action == null) return;
            
            Instance._actionQueue.Enqueue(action);
        }

        /// <summary>
        /// 在主线程执行任务（如果已经在主线程则直接执行）
        /// </summary>
        /// <param name="action">要执行的任务</param>
        public static void Execute(Action action)
        {
            if (action == null) return;

            if (IsMainThread())
            {
                action.Invoke();
            }
            else
            {
                Enqueue(action);
            }
        }

        /// <summary>
        /// 检查当前是否在主线程
        /// </summary>
        /// <returns>如果在主线程返回true</returns>
        public static bool IsMainThread()
        {
            return System.Threading.Thread.CurrentThread.ManagedThreadId == 1;
        }

        /// <summary>
        /// 安全地记录日志（可从任何线程调用）
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="logType">日志类型</param>
        public static void SafeLog(string message, LogType logType = LogType.Log)
        {
            Execute(() =>
            {
                switch (logType)
                {
                    case LogType.Log:
                        Debug.Log(message);
                        break;
                    case LogType.Warning:
                        Debug.LogWarning(message);
                        break;
                    case LogType.Error:
                        Debug.LogError(message);
                        break;
                }
            });
        }

        /// <summary>
        /// 安全地记录普通日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public static void SafeLogMessage(string message)
        {
            SafeLog(message, LogType.Log);
        }

        /// <summary>
        /// 安全地记录警告日志
        /// </summary>
        /// <param name="message">警告消息</param>
        public static void SafeLogWarning(string message)
        {
            SafeLog(message, LogType.Warning);
        }

        /// <summary>
        /// 安全地记录错误日志
        /// </summary>
        /// <param name="message">错误消息</param>
        public static void SafeLogError(string message)
        {
            SafeLog(message, LogType.Error);
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
} 
#endif 