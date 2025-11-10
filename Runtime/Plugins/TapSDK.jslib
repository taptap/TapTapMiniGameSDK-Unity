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
    Tap_OnShareMessage: function(callbackId){
        tap.onShareMessage(_Tap_ContactCommonCallback(null, _TJPointer_stringify_adaptor(callbackId)));
    },
    Tap_OffShareMessage:function(){
        tap.offShareMessage();
    },
    Tap_openFriendList: function(){
        tap.openFriendList();
    }
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

const TapLeaderboardLibrary = {
    Tap_ContactLeaderboardCallback: function (args, callbackId) {
        return Object.assign(args || {}, {
            callback: {
                onSuccess: function(res) {
                    _Tap_JSCallback(callbackId, "success", res);
                },
                onFailure: function(code, message) {
                    _Tap_JSCallback(callbackId, "fail", {
                        code: code,
                        message: message
                    });
                }
            }
        })
    },
    Tap_CreateLeaderBoardManager:function() {
        window._tapLeaderBoardManager = tap.getLeaderboardManager();
    },
    Tap_LeaderBoardManager_OpenLeaderboard:function(str, callbackId){
        if (!window._tapLeaderBoardManager) {
            window._tapLeaderBoardManager = tap.getLeaderboardManager();
        }
        const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(str));
        window._tapLeaderBoardManager.openLeaderboard(_Tap_ContactLeaderboardCallback(args, _TJPointer_stringify_adaptor(callbackId)))
    },
    Tap_LeaderBoardManager_SubmitScores:function(str, callbackId){
        if (!window._tapLeaderBoardManager) {
            window._tapLeaderBoardManager = tap.getLeaderboardManager();
        }
        const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(str));
        window._tapLeaderBoardManager.submitScores(_Tap_ContactLeaderboardCallback(args, _TJPointer_stringify_adaptor(callbackId)))
    },
    Tap_LeaderBoardManager_LoadLeaderboardScores:function(str, callbackId){
        if (!window._tapLeaderBoardManager) {
            window._tapLeaderBoardManager = tap.getLeaderboardManager();
        }
        const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(str));
        window._tapLeaderBoardManager.loadLeaderboardScores(_Tap_ContactLeaderboardCallback(args, _TJPointer_stringify_adaptor(callbackId)))
    },
    Tap_LeaderBoardManager_LoadCurrentPlayerLeaderboardScore:function(str, callbackId){
        if (!window._tapLeaderBoardManager) {
            window._tapLeaderBoardManager = tap.getLeaderboardManager();
        }
        const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(str));
        window._tapLeaderBoardManager.loadCurrentPlayerLeaderboardScore(_Tap_ContactLeaderboardCallback(args, _TJPointer_stringify_adaptor(callbackId)))
    },
    Tap_LeaderBoardManager_LoadPlayerCenteredScores:function(str, callbackId){
        if (!window._tapLeaderBoardManager) {
            window._tapLeaderBoardManager = tap.getLeaderboardManager();
        }
        const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(str));
        window._tapLeaderBoardManager.loadPlayerCenteredScores(_Tap_ContactLeaderboardCallback(args, _TJPointer_stringify_adaptor(callbackId)))
    }
};
mergeInto(LibraryManager.library, TapLeaderboardLibrary);
const TapCloudSaveLibrary = {
    Tap_CreateCloudSaveManager:function() {
        window._tapCloudSaveManager = tap.getCloudSaveManager();
    },
    Tap_CloudSaveManager_CreateArchive:function(str, callbackId){
        try {
            if (!window._tapCloudSaveManager) {
                window._tapCloudSaveManager = tap.getCloudSaveManager();
            }
            const csharpArgs = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(str));
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId); // Convert callbackId at the beginning of function

            
            // Convert C# parameter format to JS API format
            const jsArgs = {
                archiveMetaData: {
                    name: csharpArgs.name,
                    summary: csharpArgs.description || csharpArgs.name,
                    extra: csharpArgs.data || "",
                    playtime: csharpArgs.playtime || 0
                },
                archiveFilePath: csharpArgs.archiveFilePath,  // Now this is a required parameter
                success: function(res) {

                    // Convert return format: API returns {uuid, fileId}, C# expects {archiveId, fileId}
                    const convertedRes = {
                        archiveId: res.uuid || "", // uuid -> archiveId
                        fileId: res.fileId || ""   // Keep fileId
                    };

                    _Tap_JSCallback(callbackIdStr, "success", convertedRes);
                },
                fail: function(errMsg, errno) {

                    _Tap_JSCallback(callbackIdStr, "fail", { // Use string directly
                        code: errno || -1,
                        message: errMsg || "CreateArchive failed"
                    });
                },
                complete: function(errMsg, errno) {

                }
            };
            
            // Add cover path if available
            if (csharpArgs.archiveCoverPath) {
                jsArgs.archiveCoverPath = csharpArgs.archiveCoverPath;
            }
            

            window._tapCloudSaveManager.createArchive(jsArgs);
        } catch (error) {

            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "Unknown error"
            });
        }
    },
    Tap_CloudSaveManager_UpdateArchive:function(str, callbackId){
        try {
            if (!window._tapCloudSaveManager) {
                window._tapCloudSaveManager = tap.getCloudSaveManager();
            }
            const csharpArgs = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(str));
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);

            
            // Convert C# parameter format to JS API format
            const jsArgs = {
                archiveUUID: csharpArgs.archiveId, // C# uses archiveId, JS API needs archiveUUID
                archiveMetaData: {
                    name: csharpArgs.name,
                    summary: csharpArgs.description || csharpArgs.name,
                    extra: csharpArgs.data || "",
                    playtime: csharpArgs.playtime || 0
                },
                archiveFilePath: csharpArgs.archiveFilePath,
                success: function(res) {

                    _Tap_JSCallback(callbackIdStr, "success", res);
                },
                fail: function(errMsg, errno) {

                    _Tap_JSCallback(callbackIdStr, "fail", {
                        code: errno || -1,
                        message: errMsg || "UpdateArchive failed"
                    });
                },
                complete: function(errMsg, errno) {

                }
            };
            
            // Add cover path if available
            if (csharpArgs.archiveCoverPath) {
                jsArgs.archiveCoverPath = csharpArgs.archiveCoverPath;
            }
            

            window._tapCloudSaveManager.updateArchive(jsArgs);
        } catch (error) {

            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "Unknown error"
            });
        }
    },
    Tap_CloudSaveManager_GetArchiveList:function(str, callbackId){
        try {
            if (!window._tapCloudSaveManager) {
                window._tapCloudSaveManager = tap.getCloudSaveManager();
            }
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);

            
            // According to docs, getArchiveList needs no parameters, only callbacks
            const jsArgs = {
                success: function(res) {

                    // Convert return data format: saves -> archives, ArchiveDetailData format conversion
                    const convertedRes = {
                        total: res.saves ? res.saves.length : 0,
                        archives: res.saves ? res.saves.map(save => ({
                            archiveId: save.uuid,        // uuid -> archiveId
                            fileId: save.fileId,         // Keep fileId for future use
                            name: save.name,
                            description: save.summary,   // summary -> description
                            size: save.saveSize,         // saveSize -> size
                            createTime: save.createdTime * 1000,  // Convert seconds to milliseconds
                            modifyTime: save.modifiedTime * 1000,  // Convert seconds to milliseconds
                            data: save.extra || ""       // Extra data
                        })) : []
                    };
                    _Tap_JSCallback(callbackIdStr, "success", convertedRes);
                },
                fail: function(errMsg, errno) {

                    _Tap_JSCallback(callbackIdStr, "fail", {
                        code: errno || -1,
                        message: errMsg || "GetArchiveList failed"
                    });
                },
                complete: function(errMsg, errno) {

                }
            };
            
            window._tapCloudSaveManager.getArchiveList(jsArgs);
        } catch (error) {

            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "Unknown error"
            });
        }
    },
    Tap_CloudSaveManager_GetArchiveData:function(str, callbackId){
        try {
            if (!window._tapCloudSaveManager) {
                window._tapCloudSaveManager = tap.getCloudSaveManager();
            }
            const csharpArgs = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(str));
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);

            
            // According to docs, getArchiveData needs archiveUUID and archiveFileId
            const jsArgs = {
                archiveUUID: csharpArgs.archiveId,  // Archive UUID
                archiveFileId: csharpArgs.fileId || csharpArgs.archiveId, // Use fileId, fallback to archiveId if not available
                targetFilePath: csharpArgs.targetFilePath || "", // Optional parameter
                success: function(res) {

                    // According to docs, res.filePath contains downloaded file path, need to read file content
                    try {
                        // Read file content
                        const fileContent = tap.getFileSystemManager().readFileSync(res.filePath, 'utf8');

                        
                        // Convert return data format
                        const convertedRes = {
                            data: fileContent || "", // File content
                            archive: {
                                name: "Archive", // Simplified handling, should get from archive list in practice
                                archiveId: csharpArgs.archiveId,
                                size: fileContent ? fileContent.length : 0
                            }
                        };
                        _Tap_JSCallback(callbackIdStr, "success", convertedRes);
                    } catch (readError) {

                        // If file read fails, return file path as data
                        const convertedRes = {
                            data: res.filePath || "",
                            archive: {
                                name: "Archive",
                                archiveId: csharpArgs.archiveId,
                                size: 0
                            }
                        };
                        _Tap_JSCallback(callbackIdStr, "success", convertedRes);
                    }
                },
                fail: function(errMsg, errno) {

                    _Tap_JSCallback(callbackIdStr, "fail", {
                        code: errno || -1,
                        message: errMsg || "GetArchiveData failed"
                    });
                },
                complete: function(errMsg, errno) {

                }
            };
            

            window._tapCloudSaveManager.getArchiveData(jsArgs);
        } catch (error) {

            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "Unknown error"
            });
        }
    },
    Tap_CloudSaveManager_GetArchiveCover:function(str, callbackId){
        try {
            if (!window._tapCloudSaveManager) {
                window._tapCloudSaveManager = tap.getCloudSaveManager();
            }
            const csharpArgs = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(str));
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);

            
            // According to docs, getArchiveCover needs archiveUUID and archiveFileId
            const jsArgs = {
                archiveUUID: csharpArgs.archiveId,
                archiveFileId: csharpArgs.archiveId, // Simplified handling, use same value
                targetFilePath: csharpArgs.targetFilePath || "", // Optional parameter
                success: function(res) {

                    // Convert return data format
                    const convertedRes = {
                        filePath: res.filePath || ""
                    };
                    _Tap_JSCallback(callbackIdStr, "success", convertedRes);
                },
                fail: function(errMsg, errno) {

                    _Tap_JSCallback(callbackIdStr, "fail", {
                        code: errno || -1,
                        message: errMsg || "GetArchiveCover failed"
                    });
                },
                complete: function(errMsg, errno) {

                }
            };
            

            window._tapCloudSaveManager.getArchiveCover(jsArgs);
        } catch (error) {

            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "Unknown error"
            });
        }
    },
    Tap_CloudSaveManager_DeleteArchive:function(str, callbackId){
        try {
            if (!window._tapCloudSaveManager) {
                window._tapCloudSaveManager = tap.getCloudSaveManager();
            }
            const csharpArgs = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(str));
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);

            
            // According to docs, deleteArchive needs archiveUUID
            const jsArgs = {
                archiveUUID: csharpArgs.archiveId, // C# uses archiveId, JS API needs archiveUUID
                success: function(res) {

                    // Convert return data format: API returns {uuid}, C# expects {archiveId}
                    const convertedRes = {
                        archiveId: res.uuid || csharpArgs.archiveId
                    };
                    _Tap_JSCallback(callbackIdStr, "success", convertedRes);
                },
                fail: function(errMsg, errno) {

                    _Tap_JSCallback(callbackIdStr, "fail", {
                        code: errno || -1,
                        message: errMsg || "DeleteArchive failed"
                    });
                },
                complete: function(errMsg, errno) {

                }
            };
            

            window._tapCloudSaveManager.deleteArchive(jsArgs);
        } catch (error) {

            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "Unknown error"
            });
        }
    }
};
mergeInto(LibraryManager.library, TapCloudSaveLibrary);

var TapBattleLibrary = { 
    // === OnlineBattleManager Creation and Management ===
    
    Tap_CreateOnlineBattleManager: function () {
        try {
            window._tapOnlineBattleManager = tap.getOnlineBattleManager();
        } catch (error) {
            console.error("[TapBattle] Error in Tap_CreateOnlineBattleManager:", error.message || "Unknown error");
        }
    },
    
    // === SDK Management Interface ===
    
    Tap_Battle_Initialize: function (callbackId) {
        try {
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.initialize(_Tap_ContactCommonCallback({}, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_Initialize:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId)               ;
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "Initialize failed"
            });
        }
    },
    
    Tap_Battle_Connect: function (callbackId) {
        try {
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.connect(_Tap_ContactCommonCallback({}, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_Connect:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "Connect failed"
            });
        }
    },
    
    Tap_Battle_Disconnect: function (callbackId) {
        try {
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.disconnect(_Tap_ContactCommonCallback({}, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_Disconnect:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "Disconnect failed"
            });
        }
    },
    
    Tap_Battle_Finalize: function () {
        try {
            window._tapOnlineBattleManager.finalize();
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_Finalize:", error.message || "Unknown error");
        }
    },
    
    Tap_Battle_DestroyManager: function () {
        try {
            if (window._tapOnlineBattleManager && window._tapOnlineBattleManager.destroy) {
                window._tapOnlineBattleManager.destroy();
            }
            window._tapOnlineBattleManager = null;
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_DestroyManager:", error.message || "Unknown error");
        }
    },
    
    // === Room Management Interface ===
    
    Tap_Battle_CreateRoom: function (jsonData, callbackId) {
        try {
            const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(jsonData));
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.createRoom(_Tap_ContactCommonCallback(args, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_CreateRoom:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "CreateRoom failed"
            });
        }
    },
    
    Tap_Battle_MatchRoom: function (jsonData, callbackId) {
        try {
            const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(jsonData));
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.matchRoom(_Tap_ContactCommonCallback(args, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_MatchRoom:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "MatchRoom failed"
            });
        }
    },
    
    Tap_Battle_LeaveRoom: function (callbackId) {
        try {
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.leaveRoom(_Tap_ContactCommonCallback({}, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_LeaveRoom:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "LeaveRoom failed"
            });
        }
    },
    
    // === Player Properties Interface ===
    
    Tap_Battle_UpdatePlayerCustomStatus: function (status, callbackId) {
        try {
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.updatePlayerCustomStatus(_Tap_ContactCommonCallback({
                status: status
            }, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_UpdatePlayerCustomStatus:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "UpdatePlayerCustomStatus failed"
            });
        }
    },
    
    Tap_Battle_UpdatePlayerCustomProperties: function (properties, callbackId) {
        try {
            const propertiesStr = _TJPointer_stringify_adaptor(properties);
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.updatePlayerCustomProperties(_Tap_ContactCommonCallback({
                properties: propertiesStr
            }, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_UpdatePlayerCustomProperties:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "UpdatePlayerCustomProperties failed"
            });
        }
    },
    
    Tap_Battle_UpdateRoomProperties: function (jsonData, callbackId) {
        try {
            const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(jsonData));
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.updateRoomProperties(_Tap_ContactCommonCallback(args, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_UpdateRoomProperties:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "UpdateRoomProperties failed"
            });
        }
    },
    
    // === Battle Management Interface ===
    
    Tap_Battle_StartBattle: function (callbackId) {
        try {
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.startBattle(_Tap_ContactCommonCallback({}, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_StartBattle:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "StartBattle failed"
            });
        }
    },
    
    Tap_Battle_SendInput: function (jsonData, callbackId) {
        try {
            const jsonDataStr = _TJPointer_stringify_adaptor(jsonData);
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.sendInput(_Tap_ContactCommonCallback({
                data: jsonDataStr
            }, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_SendInput:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "SendInput failed"
            });
        }
    },
    
    Tap_Battle_StopBattle: function (callbackId) {
        try {
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.stopBattle(_Tap_ContactCommonCallback({}, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_StopBattle:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "StopBattle failed"
            });
        }
    },

    // === New Battle Management APIs ===

    Tap_Battle_KickRoomPlayer: function (str, callbackId) {
        try {
            const playerId = _TJPointer_stringify_adaptor(str);
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.kickRoomPlayer(_Tap_ContactCommonCallback({
                playerId: playerId
            }, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_KickRoomPlayer:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "KickRoomPlayer failed"
            });
        }
    },

    Tap_Battle_SendCustomMessage: function (str, callbackId) {
        try {
            const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(str));
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            const finalData = _Tap_ContactCommonCallback(args, callbackIdStr);
            window._tapOnlineBattleManager.sendCustomMessage(finalData);
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_SendCustomMessage:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "SendCustomMessage failed"
            });
        }
    },
    
    // === Random Number Generator Interface ===
    
    Tap_Battle_NewRandomNumberGenerator: function (seed) {
        try {
            window._randomGenerator = window._tapOnlineBattleManager.newRandomNumberGenerator(seed);
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_NewRandomNumberGenerator:", error.message || "Unknown error");
            return null;
        }
    },
    
    Tap_Battle_RandomInt: function () {
        try {
            if (window._randomGenerator && window._randomGenerator) {
                return window._randomGenerator.randomInt();
            } else {
                console.error("[TapBattle] No current RandomNumberGenerator instance available");
                return 0;
            }
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_RandomInt:", error.message || "Unknown error");
            return 0;
        }
    },
    
    Tap_Battle_FreeRandomNumberGenerator: function () {
        try {
            window._randomGenerator.free();
            window._randomGenerator = null;
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_FreeRandomNumberGenerator:", error.message || "Unknown error");
        }
    },

    // === RegisterListener ===
    Tap_Battle_RegisterListener: function (callbackIdsJson) {
        try {
            const callbackIdsStr = _TJPointer_stringify_adaptor(callbackIdsJson);
            const callbackIds = JSON.parse(callbackIdsStr);
            
            console.log("[TapBattle] Tap_Battle_RegisterListener callbackIds:", callbackIdsStr);
            console.log(callbackIdsStr)
            
            if (window._tapOnlineBattleManager && window._tapOnlineBattleManager.registerListener) {
                const listenerConfig = {};
                
                // Configure callback functions for each event type
                if (callbackIds.onDisconnected) {
                    listenerConfig.onDisconnected = (errorInfo) => {
                        console.log("[TapBattle] onDisconnected:", JSON.stringify(errorInfo, null, 2));
                        _Tap_JSCallback(callbackIds.onDisconnected, "success", errorInfo);
                    };
                }
                
                if (callbackIds.onBattleServiceError) {
                    listenerConfig.onBattleServiceError = (errorInfo) => {
                        console.log("[TapBattle] onBattleServiceError:", JSON.stringify(errorInfo, null, 2));
                        _Tap_JSCallback(callbackIds.onBattleServiceError, "success", errorInfo);
                    };
                }
                
                if (callbackIds.onRoomPropertiesChange) {
                    listenerConfig.onRoomPropertiesChange = (info) => {
                        console.log("[TapBattle] onRoomPropertiesChange:", JSON.stringify(info, null, 2));
                        _Tap_JSCallback(callbackIds.onRoomPropertiesChange, "success", info);
                    };
                }
                
                if (callbackIds.onPlayerCustomPropertiesChange) {
                    listenerConfig.onPlayerCustomPropertiesChange = (info) => {
                        console.log("[TapBattle] onPlayerCustomPropertiesChange:", JSON.stringify(info, null, 2));
                        _Tap_JSCallback(callbackIds.onPlayerCustomPropertiesChange, "success", info);
                    };
                }
                
                if (callbackIds.onPlayerCustomStatusChange) {
                    listenerConfig.onPlayerCustomStatusChange = (info) => {
                        console.log("[TapBattle] onPlayerCustomStatusChange:", JSON.stringify(info, null, 2));
                        _Tap_JSCallback(callbackIds.onPlayerCustomStatusChange, "success", info);
                    };
                }
                
                if (callbackIds.onBattleStop) {
                    listenerConfig.onBattleStop = (info) => {
                        console.log("[TapBattle] onBattleStop:", JSON.stringify(info, null, 2));
                        _Tap_JSCallback(callbackIds.onBattleStop, "success", info);
                    };
                }
                
                if (callbackIds.onBattleFrame) {
                    listenerConfig.onBattleFrame = (info) => {
                        console.log("[TapBattle] onBattleFrame:", JSON.stringify(info, null, 2));
                        _Tap_JSCallback(callbackIds.onBattleFrame, "success", info);
                    };
                }
                
                if (callbackIds.onBattleStart) {
                    listenerConfig.onBattleStart = (info) => {
                        console.log("[TapBattle] onBattleStart:", JSON.stringify(info, null, 2));
                        _Tap_JSCallback(callbackIds.onBattleStart, "success", info);
                    };
                }
                
                if (callbackIds.playerOffline) {
                    listenerConfig.playerOffline = (info) => {
                        console.log("[TapBattle] playerOffline:", JSON.stringify(info, null, 2));
                        _Tap_JSCallback(callbackIds.playerOffline, "success", info);
                    };
                }
                
                if (callbackIds.playerLeaveRoom) {
                    listenerConfig.playerLeaveRoom = (info) => {
                        console.log("[TapBattle] playerLeaveRoom:", JSON.stringify(info, null, 2));
                        _Tap_JSCallback(callbackIds.playerLeaveRoom, "success", info);
                    };
                }
                
                if (callbackIds.playerEnterRoom) {
                    listenerConfig.playerEnterRoom = (info) => {
                        console.log("[TapBattle] playerEnterRoom:", JSON.stringify(info, null, 2));
                        _Tap_JSCallback(callbackIds.playerEnterRoom, "success", info);
                    };
                }
                
                if (callbackIds.onCustomMessage) {
                    listenerConfig.onCustomMessage = (info) => {
                        console.log("[TapBattle] onCustomMessage:", JSON.stringify(info, null, 2));
                        _Tap_JSCallback(callbackIds.onCustomMessage, "success", info);
                    };
                }
                
                if (callbackIds.onPlayerKicked) {
                    listenerConfig.onPlayerKicked = (info) => {
                        console.log("[TapBattle] onPlayerKicked:", JSON.stringify(info, null, 2));
                        _Tap_JSCallback(callbackIds.onPlayerKicked, "success", info);
                    };
                }
                
                // Call window._tapOnlineBattleManager.registerListener
                window._tapOnlineBattleManager.registerListener(listenerConfig);
                console.log("[TapBattle] Tap_Battle_RegisterListener registration completed, " + Object.keys(callbackIds).length + " event callbacks registered");
            } else {
                console.error("[TapBattle] window._tapOnlineBattleManager.registerListener method not available");
            }
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_RegisterListener:", error.message || "Unknown error");
            console.error(error.message)
        }
    },

    Tap_Battle_GetRoomList: function (jsonData, callbackId) {
        try {
            const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(jsonData));
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.getRoomList(_Tap_ContactCommonCallback(args, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_GetRoomList:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "GetRoomList failed"
            });
        }
    },

    Tap_Battle_JoinRoom: function (jsonData, callbackId) {
        try {
            const args = _Tap_formatJsonStr(_TJPointer_stringify_adaptor(jsonData));
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            window._tapOnlineBattleManager.joinRoom(_Tap_ContactCommonCallback(args, callbackIdStr));
        } catch (error) {
            console.error("[TapBattle] Error in Tap_Battle_JoinRoom:", error.message || "Unknown error");
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "JoinRoom failed"
            });
        }
    },

};

mergeInto(LibraryManager.library, TapBattleLibrary);