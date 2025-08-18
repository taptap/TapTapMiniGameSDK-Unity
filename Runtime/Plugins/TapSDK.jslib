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
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId); // 在函数开始时转换callbackId
            console.log("[TapCloudSave] C# args:", csharpArgs);
            
            // 转换C#参数格式为JS API格式
            const jsArgs = {
                archiveMetaData: {
                    name: csharpArgs.name,
                    summary: csharpArgs.description || csharpArgs.name,
                    extra: csharpArgs.data || "",
                    playtime: csharpArgs.playtime || 0
                },
                archiveFilePath: csharpArgs.archiveFilePath,  // 现在这是必需参数
                success: function(res) {
                    console.log("[TapCloudSave] CreateArchive success:", res);
                    // 转换返回格式：文档返回{uuid, fileId}，C#期望{archiveId, fileId}
                    const convertedRes = {
                        archiveId: res.uuid || "", // uuid -> archiveId
                        fileId: res.fileId || ""   // 保留fileId
                    };
                    console.log("[TapCloudSave] CreateArchive converted response:", convertedRes);
                    _Tap_JSCallback(callbackIdStr, "success", convertedRes);
                },
                fail: function(errMsg, errno) {
                    console.log("[TapCloudSave] CreateArchive fail:", errMsg, errno);
                    _Tap_JSCallback(callbackIdStr, "fail", { // 直接使用字符串
                        code: errno || -1,
                        message: errMsg || "CreateArchive failed"
                    });
                },
                complete: function(errMsg, errno) {
                    console.log("[TapCloudSave] CreateArchive complete:", errMsg, errno);
                }
            };
            
            // 如果有封面路径，添加封面路径
            if (csharpArgs.archiveCoverPath) {
                jsArgs.archiveCoverPath = csharpArgs.archiveCoverPath;
            }
            
            console.log("[TapCloudSave] JS args:", jsArgs);
            window._tapCloudSaveManager.createArchive(jsArgs);
        } catch (error) {
            console.error("[TapCloudSave] CreateArchive error:", error);
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
            console.log("[TapCloudSave] UpdateArchive C# args:", csharpArgs);
            
            // 转换C#参数格式为JS API格式
            const jsArgs = {
                archiveUUID: csharpArgs.archiveId, // C#使用archiveId，JS API需要archiveUUID
                archiveMetaData: {
                    name: csharpArgs.name,
                    summary: csharpArgs.description || csharpArgs.name,
                    extra: csharpArgs.data || "",
                    playtime: csharpArgs.playtime || 0
                },
                archiveFilePath: csharpArgs.archiveFilePath,
                success: function(res) {
                    console.log("[TapCloudSave] UpdateArchive success:", res);
                    _Tap_JSCallback(callbackIdStr, "success", res);
                },
                fail: function(errMsg, errno) {
                    console.log("[TapCloudSave] UpdateArchive fail:", errMsg, errno);
                    _Tap_JSCallback(callbackIdStr, "fail", {
                        code: errno || -1,
                        message: errMsg || "UpdateArchive failed"
                    });
                },
                complete: function(errMsg, errno) {
                    console.log("[TapCloudSave] UpdateArchive complete:", errMsg, errno);
                }
            };
            
            // 如果有封面路径，添加封面路径
            if (csharpArgs.archiveCoverPath) {
                jsArgs.archiveCoverPath = csharpArgs.archiveCoverPath;
            }
            
            console.log("[TapCloudSave] UpdateArchive JS args:", jsArgs);
            window._tapCloudSaveManager.updateArchive(jsArgs);
        } catch (error) {
            console.error("[TapCloudSave] UpdateArchive error:", error);
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
            console.log("[TapCloudSave] GetArchiveList - 获取存档列表");
            
            // 根据文档，getArchiveList不需要参数，只需要回调
            const jsArgs = {
                success: function(res) {
                    console.log("[TapCloudSave] GetArchiveList success:", res);
                    // 转换返回数据格式：saves -> archives, ArchiveDetailData格式转换
                    const convertedRes = {
                        total: res.saves ? res.saves.length : 0,
                        archives: res.saves ? res.saves.map(save => ({
                            archiveId: save.uuid,        // uuid -> archiveId
                            fileId: save.fileId,         // 保留fileId供后续使用
                            name: save.name,
                            description: save.summary,   // summary -> description
                            size: save.saveSize,         // saveSize -> size
                            createTime: save.createdTime * 1000,  // 秒转毫秒
                            modifyTime: save.modifiedTime * 1000,  // 秒转毫秒
                            data: save.extra || ""       // 额外数据
                        })) : []
                    };
                    _Tap_JSCallback(callbackIdStr, "success", convertedRes);
                },
                fail: function(errMsg, errno) {
                    console.log("[TapCloudSave] GetArchiveList fail:", errMsg, errno);
                    _Tap_JSCallback(callbackIdStr, "fail", {
                        code: errno || -1,
                        message: errMsg || "GetArchiveList failed"
                    });
                },
                complete: function(errMsg, errno) {
                    console.log("[TapCloudSave] GetArchiveList complete:", errMsg, errno);
                }
            };
            
            window._tapCloudSaveManager.getArchiveList(jsArgs);
        } catch (error) {
            console.error("[TapCloudSave] GetArchiveList error:", error);
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
            console.log("[TapCloudSave] GetArchiveData C# args:", csharpArgs);
            
            // 根据文档，getArchiveData需要archiveUUID和archiveFileId
            const jsArgs = {
                archiveUUID: csharpArgs.archiveId,  // 存档UUID
                archiveFileId: csharpArgs.fileId || csharpArgs.archiveId, // 使用fileId，如果没有则使用archiveId作为备用
                targetFilePath: csharpArgs.targetFilePath || "", // 可选参数
                success: function(res) {
                    console.log("[TapCloudSave] GetArchiveData success:", res);
                    // 根据文档，res.filePath包含下载的文件路径，需要读取文件内容
                    try {
                        // 读取文件内容
                        const fileContent = tap.getFileSystemManager().readFileSync(res.filePath, 'utf8');
                        console.log("[TapCloudSave] GetArchiveData file content:", fileContent);
                        
                        // 转换返回数据格式
                        const convertedRes = {
                            data: fileContent || "", // 文件内容
                            archive: {
                                name: "Archive", // 简化处理，实际应该从存档列表获取
                                archiveId: csharpArgs.archiveId,
                                size: fileContent ? fileContent.length : 0
                            }
                        };
                        _Tap_JSCallback(callbackIdStr, "success", convertedRes);
                    } catch (readError) {
                        console.error("[TapCloudSave] GetArchiveData read file error:", readError);
                        // 如果文件读取失败，返回文件路径作为数据
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
                    console.log("[TapCloudSave] GetArchiveData fail:", errMsg, errno);
                    _Tap_JSCallback(callbackIdStr, "fail", {
                        code: errno || -1,
                        message: errMsg || "GetArchiveData failed"
                    });
                },
                complete: function(errMsg, errno) {
                    console.log("[TapCloudSave] GetArchiveData complete:", errMsg, errno);
                }
            };
            
            console.log("[TapCloudSave] GetArchiveData JS args:", jsArgs);
            window._tapCloudSaveManager.getArchiveData(jsArgs);
        } catch (error) {
            console.error("[TapCloudSave] GetArchiveData error:", error);
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
            console.log("[TapCloudSave] GetArchiveCover C# args:", csharpArgs);
            
            // 根据文档，getArchiveCover需要archiveUUID和archiveFileId
            const jsArgs = {
                archiveUUID: csharpArgs.archiveId,
                archiveFileId: csharpArgs.archiveId, // 简化处理，使用相同值
                targetFilePath: csharpArgs.targetFilePath || "", // 可选参数
                success: function(res) {
                    console.log("[TapCloudSave] GetArchiveCover success:", res);
                    // 转换返回数据格式
                    const convertedRes = {
                        filePath: res.filePath || ""
                    };
                    _Tap_JSCallback(callbackIdStr, "success", convertedRes);
                },
                fail: function(errMsg, errno) {
                    console.log("[TapCloudSave] GetArchiveCover fail:", errMsg, errno);
                    _Tap_JSCallback(callbackIdStr, "fail", {
                        code: errno || -1,
                        message: errMsg || "GetArchiveCover failed"
                    });
                },
                complete: function(errMsg, errno) {
                    console.log("[TapCloudSave] GetArchiveCover complete:", errMsg, errno);
                }
            };
            
            console.log("[TapCloudSave] GetArchiveCover JS args:", jsArgs);
            window._tapCloudSaveManager.getArchiveCover(jsArgs);
        } catch (error) {
            console.error("[TapCloudSave] GetArchiveCover error:", error);
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
            console.log("[TapCloudSave] DeleteArchive C# args:", csharpArgs);
            
            // 根据文档，deleteArchive需要archiveUUID
            const jsArgs = {
                archiveUUID: csharpArgs.archiveId, // C#使用archiveId，JS API需要archiveUUID
                success: function(res) {
                    console.log("[TapCloudSave] DeleteArchive success:", res);
                    // 转换返回数据格式：文档返回{uuid}，C#期望{archiveId}
                    const convertedRes = {
                        archiveId: res.uuid || csharpArgs.archiveId
                    };
                    _Tap_JSCallback(callbackIdStr, "success", convertedRes);
                },
                fail: function(errMsg, errno) {
                    console.log("[TapCloudSave] DeleteArchive fail:", errMsg, errno);
                    _Tap_JSCallback(callbackIdStr, "fail", {
                        code: errno || -1,
                        message: errMsg || "DeleteArchive failed"
                    });
                },
                complete: function(errMsg, errno) {
                    console.log("[TapCloudSave] DeleteArchive complete:", errMsg, errno);
                }
            };
            
            console.log("[TapCloudSave] DeleteArchive JS args:", jsArgs);
            window._tapCloudSaveManager.deleteArchive(jsArgs);
        } catch (error) {
            console.error("[TapCloudSave] DeleteArchive error:", error);
            const callbackIdStr = _TJPointer_stringify_adaptor(callbackId);
            _Tap_JSCallback(callbackIdStr, "fail", {
                code: -1,
                message: error.message || "Unknown error"
            });
        }
    }
};
mergeInto(LibraryManager.library, TapCloudSaveLibrary);

