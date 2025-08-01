#if (UNITY_WEBGL || WEIXINMINIGAME || UNITY_EDITOR) && !TAP_DEBUG_ENABLE
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

using TapTapMiniGame;
using Touch = UnityEngine.Touch;

internal class TouchData
{
    public Touch touch;
    public long timeStamp;
}

/**
 * 由于Unity WebGL发布的多点触控存在问题, 导致在小游戏宿主中多点触控存在粘连的情况
 * 所以需要使用WX的触控接口重新覆盖Unity的BaseInput关于触控方面的接口
 * 通过设置StandaloneInputModule.inputOverride的方式来实现
*/
[RequireComponent(typeof(StandaloneInputModule))]
public class TapTouchInputOverride : BaseInput
{
    private bool _isInitWechatSDK;
    private readonly List<TouchData> _touches = new List<TouchData>();
    private StandaloneInputModule _standaloneInputModule = null;

    protected override void Awake()
    {
        base.Awake();
        _standaloneInputModule = GetComponent<StandaloneInputModule>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (string.IsNullOrEmpty(Tap.GetSystemInfoSync().platform)) return;
        InitWechatTouchEvents();
        if (_standaloneInputModule)
        {
            _standaloneInputModule.inputOverride = this;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UnregisterWechatTouchEvents();
        if (_standaloneInputModule)
        {
            _standaloneInputModule.inputOverride = null;
        }
    }

    private void InitWechatTouchEvents()
    {
        if (!_isInitWechatSDK)
        {
            Tap.InitSDK((code) =>
            {
                _isInitWechatSDK = true;
                RegisterWechatTouchEvents();
            });
        }
        else
        {
            RegisterWechatTouchEvents();
        }
    }

    private void RegisterWechatTouchEvents()
    {
        Tap.OnTouchStart(OnTjTouchStart);
        Tap.OnTouchMove(OnTjTouchMove);
        Tap.OnTouchEnd(OnTjTouchEnd);
        Tap.OnTouchCancel(OnTjTouchCancel);
    }

    private void UnregisterWechatTouchEvents()
    {
        Tap.OffTouchStart(OnTjTouchStart);
        Tap.OffTouchMove(OnTjTouchMove);
        Tap.OffTouchEnd(OnTjTouchEnd);
        Tap.OffTouchCancel(OnTjTouchCancel);
    }

    private void OnTjTouchStart(OnTouchStartListenerResult touchEvent)
    {
        foreach (var tjTouch in touchEvent.changedTouches)
        {
            var data = FindOrCreateTouchData(tjTouch.identifier);
            data.touch.phase = TouchPhase.Began;
            data.touch.position = new Vector2(tjTouch.clientX, tjTouch.clientY);
            data.touch.rawPosition = data.touch.position;
            data.timeStamp = touchEvent.timeStamp;

            // Debug.Log($"OnTjTouchStart:{tjTouch.identifier}, {data.touch.phase}");
        }
    }

    private void OnTjTouchMove(OnTouchStartListenerResult touchEvent)
    {
        foreach (var tjTouch in touchEvent.changedTouches)
        {
            var data = FindOrCreateTouchData(tjTouch.identifier);
            UpdateTouchData(data, new Vector2(tjTouch.clientX, tjTouch.clientY), touchEvent.timeStamp, TouchPhase.Moved);
        }
    }

    private void OnTjTouchEnd(OnTouchStartListenerResult touchEvent)
    {
        foreach (var tjTouch in touchEvent.changedTouches)
        {
            TouchData data = FindTouchData(tjTouch.identifier);
            if (data == null)
            {
                Debug.LogError($"OnTjTouchEnd, error identifier:{tjTouch.identifier}");
                return;
            }

            if (data.touch.phase == TouchPhase.Canceled || data.touch.phase == TouchPhase.Ended)
            {
                Debug.LogWarning($"OnTjTouchEnd, error phase:{tjTouch.identifier}, phase:{data.touch.phase}");
            }

            // Debug.Log($"OnTjTouchEnd:{tjTouch.identifier}");
            UpdateTouchData(data, new Vector2(tjTouch.clientX, tjTouch.clientY), touchEvent.timeStamp, TouchPhase.Ended);
        }

        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        if (selectedObject != null)
        {
            Button button = selectedObject.GetComponent<Button>();
            if (button != null)
            {
                int clickListenerCount = button.onClick.GetPersistentEventCount();
                if (clickListenerCount > 0) {
                    button.onClick.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
                    button.onClick.Invoke();
                    button.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
                }
            }
        }
    }

    private void OnTjTouchCancel(OnTouchStartListenerResult touchEvent)
    {
        foreach (var tjTouch in touchEvent.changedTouches)
        {
            TouchData data = FindTouchData(tjTouch.identifier);
            if (data == null)
            {
                Debug.LogError($"OnTjTouchCancel, error identifier:{tjTouch.identifier}");
                return;
            }

            if (data.touch.phase == TouchPhase.Canceled || data.touch.phase == TouchPhase.Ended)
            {
                Debug.LogWarning($"OnTjTouchCancel, error phase:{tjTouch.identifier}, phase:{data.touch.phase}");
            }

            // Debug.Log($"OnTjTouchCancel:{tjTouch.identifier}");
            UpdateTouchData(data, new Vector2(tjTouch.clientX, tjTouch.clientY), touchEvent.timeStamp, TouchPhase.Canceled);
        }
    }

    private void LateUpdate()
    {
        foreach (var t in _touches)
        {
            if (t.touch.phase == TouchPhase.Began)
            {
                t.touch.phase = TouchPhase.Stationary;
            }
        }

        RemoveEndedTouches();
    }

    private void RemoveEndedTouches()
    {
        if (_touches.Count > 0)
        {
            _touches.RemoveAll(touchData =>
            {
                var touch = touchData.touch;
                return touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
            });
        }
    }

    private TouchData FindTouchData(int identifier)
    {
        foreach (var touchData in _touches)
        {
            var touch = touchData.touch;
            if (touch.fingerId == identifier)
            {
                return touchData;
            }
        }

        return null;
    }

    private TouchData FindOrCreateTouchData(int identifier)
    {
        var touchData = FindTouchData(identifier);
        if (touchData != null)
        {
            return touchData;
        }

        var data = new TouchData();
        data.touch.pressure = 1.0f;
        data.touch.maximumPossiblePressure = 1.0f;
        data.touch.type = TouchType.Direct;
        data.touch.tapCount = 1;
        data.touch.fingerId = identifier;
        data.touch.radius = 0;
        data.touch.radiusVariance = 0;
        data.touch.altitudeAngle = 0;
        data.touch.azimuthAngle = 0;
        data.touch.deltaTime = 0;
        _touches.Add(data);
        return data;
    }

    private static void UpdateTouchData(TouchData data, Vector2 pos, long timeStamp, TouchPhase phase)
    {
        data.touch.phase = phase;
        data.touch.deltaPosition = pos - data.touch.position;
        data.touch.position = pos;
        data.touch.deltaTime = (timeStamp - data.timeStamp) / 1000000.0f;
    }

#if !UNITY_EDITOR
    public override bool touchSupported
    {
        get
        {
            return true;
        }
    }
    public override bool mousePresent
    {
        get
        {
            return false;
        }
    }
    public override int touchCount
    {
        get { return _touches.Count; }
    }

    public override Touch GetTouch(int index)
    {
        // Debug.LogError($"GetTouch touchCount:{touchCount}, index:{index}, touch:{_touches[index].touch.fingerId}, {_touches[index].touch.phase}");
        return _touches[index].touch;
    }

#endif
}
#endif
