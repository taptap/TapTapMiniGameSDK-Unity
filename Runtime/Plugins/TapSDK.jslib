mergeInto(LibraryManager.library, {
    Tap_formatJsonStr: function (str) {
        const args = JSON.parse(str) || {};
        Object.keys(args).forEach((v) => {
            if (args[v] === null) {
                delete args[v];
            }
        });
        return args;
    },
    Tap_JSCallback: function (callbackId, type, res) {
        SendMessage("TapExtManagerHandler", "HandleJSCallback", JSON.stringify({
            callbackId: callbackId,
            type: type,
            res: JSON.stringify(res)
        }));
    },
    Tap_ContactCommonCallback: function (args, callbackId) {
        return Object.assign(args || {}, {
            success: function (res) { _Tap_JSCallback(callbackId, "success", res); },
            fail: function (res) { _Tap_JSCallback(callbackId, "fail", res); },
            complete: function (res) { _Tap_JSCallback(callbackId, "complete", res); }
        });
    }
});

var TapShareLibrary = {
    Tap_SetShareboardHidden: function (str, callbackId) {
        const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(str));
        tap.setShareboardHidden(_Tap_ContactCommonCallback(args, _TJPointer_stringify_adaptor(callbackId)));
    },
    Tap_ShowShareboard: function (str, callbackId) {
        const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(str));
        tap.showShareboard(_Tap_ContactCommonCallback(args, _TJPointer_stringify_adaptor(callbackId)));
    },
    Tap_OnShareMessage: function(){
        const listener = function(resolve, channel){
            window._tapOnShareMessageResolve = resolve;
            SendMessage("TapExtManagerHandler", "OnShareMessageCallback", channel);
        };
        tap.onShareMessage(listener);
    },
    Tap_OnShareMessageResolve: function(conf, callbackId){
        try {
            const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(conf));
            const merged = _Tap_ContactCommonCallback(args, _TJPointer_stringify_adaptor(callbackId));

            if(window._tapOnShareMessageResolve){
                window._tapOnShareMessageResolve(merged);
                window._tapOnShareMessageResolve = null;
            }
        } catch(e) {
             if(window._tapOnShareMessageResolve){
                window._tapOnShareMessageResolve({});
                window._tapOnShareMessageResolve = null;
             }
        }
    },
    Tap_OffShareMessage:function(){
        tap.offShareMessage();
    },
};
mergeInto(LibraryManager.library, TapShareLibrary);

var TapAchievementLibrary = {
    Tap_CreateAchievementManager:function(option) {
        const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(option));
        window._tapAchievementManager = tap.createAchievementManager({ toastEnable: args.enableToast });
    },
    Tap_AchievementManager_SetToastEnabled:function(enabled){
        if (!window._tapAchievementManager) {
            window._tapAchievementManager = tap.createAchievementManager();
        }
        window._tapAchievementManager.toastEnable = enabled;
    },
    Tap_AchievementManager_RegisterListener:function(callbackId){
        if (!window._tapAchievementManager) {
            window._tapAchievementManager = tap.createAchievementManager();
        }
        var csharpId = _TJPointer_stringify_adaptor(callbackId);
        window._tapAchievementManager.registerListener({
            onAchievementSuccess: function (code, result) {
                _Tap_JSCallback(csharpId, "success", {
                    code: code,
                    result: result
                });
            },
            onAchievementFailure: function (id, code, msg) {
                _Tap_JSCallback(csharpId, "fail", {
                    id: id,
                    code: code,
                    msg: msg
                });
            }
        });
    },
    Tap_AchievementManager_UnregisterListener:function(callbackId){
        if (!window._tapAchievementManager) {
            window._tapAchievementManager = tap.createAchievementManager();
        }
        window._tapAchievementManager.unregisterListener();
        _Tap_JSCallback(_TJPointer_stringify_adaptor(callbackId), "remove", null);
    },
    Tap_AchievementManager_Unlock:function(id){
        if (!window._tapAchievementManager) {
            window._tapAchievementManager = tap.createAchievementManager();
        }
        window._tapAchievementManager.unlockAchievement({ achievementId: _TJPointer_stringify_adaptor(id) });
    },
    Tap_AchievementManager_IncrementSteps:function(id, steps){
        if (!window._tapAchievementManager) {
            window._tapAchievementManager = tap.createAchievementManager();
        }
        window._tapAchievementManager.incrementAchievement({
            achievementId: _TJPointer_stringify_adaptor(id),
            steps: steps
        });
    },
    Tap_AchievementManager_ShowAllAchievements:function(){
        if (!window._tapAchievementManager) {
            window._tapAchievementManager = tap.createAchievementManager();
        }
        window._tapAchievementManager.showAchievements();
    },
};

mergeInto(LibraryManager.library, TapAchievementLibrary);

var TapHomeScreenWidgetLibrary = {
    Tap_CreateHomeScreenWidget: function (callbackId) {
        tap.createHomeScreenWidget(_Tap_ContactCommonCallback({}, _TJPointer_stringify_adaptor(callbackId)));
    },
    Tap_HasHomeScreenWidgetAndPinned: function (callbackId) {
        tap.hasHomeScreenWidgetAndPinned(_Tap_ContactCommonCallback({}, _TJPointer_stringify_adaptor(callbackId)));
    },
};
mergeInto(LibraryManager.library, TapHomeScreenWidgetLibrary);