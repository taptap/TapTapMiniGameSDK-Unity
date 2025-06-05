import moduleHelper from './module-helper';
import { launchEventType } from '../plugin-config';
import { setArrayBuffer, uid } from './utils';
import '../events';
export default {
    TJReportGameStart() {
        GameGlobal.manager.reportCustomLaunchInfo();
    },
    TJReportGameSceneError(sceneId, errorType, errStr, extInfo) {
        if (GameGlobal.manager && GameGlobal.manager.reportGameSceneError) {
            GameGlobal.manager.reportGameSceneError(sceneId, errorType, errStr, extInfo);
        }
    },
    TJWriteLog(str) {
        if (GameGlobal.manager && GameGlobal.manager.writeLog) {
            GameGlobal.manager.writeLog(str);
        }
    },
    TJWriteWarn(str) {
        if (GameGlobal.manager && GameGlobal.manager.writeWarn) {
            GameGlobal.manager.writeWarn(str);
        }
    },
    TJHideLoadingPage() {
        if (GameGlobal.manager && GameGlobal.manager.hideLoadingPage) {
            GameGlobal.manager.hideLoadingPage();
        }
    },
    TJReportUserBehaviorBranchAnalytics(branchId, branchDim, eventType) {
        tj.reportUserBehaviorBranchAnalytics({ branchId, branchDim, eventType });
    },
    TJPreloadConcurrent(count) {
        if (GameGlobal.manager && GameGlobal.manager.setConcurrent) {
            GameGlobal.manager.setConcurrent(count);
        }
    },
    TJIsCloudTest() {
        if (typeof GameGlobal.isTest !== 'undefined' && GameGlobal.isTest) {
            return true;
        }
        return false;
    },
    TJUncaughtException(needAbort) {
        function currentStackTrace() {
            const err = new Error('TJUncaughtException');
            return err;
        }
        const err = currentStackTrace();
        let fullTrace = err.stack?.toString();
        if (fullTrace) {
            const posOfThisFunc = fullTrace.indexOf('TJUncaughtException');
            if (posOfThisFunc !== -1) {
                fullTrace = fullTrace.substr(posOfThisFunc);
            }
            const posOfRaf = fullTrace.lastIndexOf('browserIterationFunc');
            if (posOfRaf !== -1) {
                fullTrace = fullTrace.substr(0, posOfRaf);
            }
        }
        const realTimelog = tj.getRealtimeLogManager();
        realTimelog.error(fullTrace);
        const logmanager = tj.getLogManager({ level: 0 });
        logmanager.warn(fullTrace);
        if (needAbort === true) {
            GameGlobal.onCrash(err);
            throw err;
        }
        else {
            setTimeout(() => {
                throw err;
            }, 0);
        }
    },
    TJCleanAllFileCache() {
        if (GameGlobal.manager && GameGlobal.manager.cleanCache) {
            const key = uid();
            GameGlobal.manager.cleanAllCache().then((res) => {
                moduleHelper.send('CleanAllFileCacheCallback', JSON.stringify({
                    callbackId: key,
                    result: res,
                }));
            });
            return key;
        }
        return '';
    },
    TJCleanFileCache(fileSize) {
        if (GameGlobal.manager && GameGlobal.manager.cleanCache) {
            const key = uid();
            GameGlobal.manager.cleanCache(fileSize).then((res) => {
                moduleHelper.send('CleanFileCacheCallback', JSON.stringify({
                    callbackId: key,
                    result: res,
                }));
            });
            return key;
        }
        return '';
    },
    TJRemoveFile(path) {
        if (GameGlobal.manager && GameGlobal.manager.removeFile && path) {
            const key = uid();
            GameGlobal.manager.removeFile(path).then((res) => {
                moduleHelper.send('RemoveFileCallback', JSON.stringify({
                    callbackId: key,
                    result: res,
                }));
            });
            return key;
        }
        return '';
    },
    TJGetCachePath(url) {
        if (GameGlobal.manager && GameGlobal.manager.getCachePath) {
            return GameGlobal.manager.getCachePath(url);
        }
    },
    TJGetPluginCachePath() {
        if (GameGlobal.manager && GameGlobal.manager.PLUGIN_CACHE_PATH) {
            return GameGlobal.manager.PLUGIN_CACHE_PATH;
        }
    },
    TJOnLaunchProgress() {
        if (GameGlobal.manager && GameGlobal.manager.onLaunchProgress) {
            const key = uid();
            // 异步执行，保证C#已经记录这个回调ID
            setTimeout(() => {
                GameGlobal.manager.onLaunchProgress((e) => {
                    moduleHelper.send('OnLaunchProgressCallback', JSON.stringify({
                        callbackId: key,
                        res: JSON.stringify(Object.assign({}, e.data, {
                            type: e.type,
                        })),
                    }));
                    
                    if (e.type === launchEventType.prepareGame) {
                        moduleHelper.send('RemoveLaunchProgressCallback', JSON.stringify({
                            callbackId: key,
                        }));
                    }
                });
            }, 0);
            return key;
        }
        return '';
    },
    TJSetDataCDN(path) {
        if (GameGlobal.manager && GameGlobal.manager.setDataCDN) {
            GameGlobal.manager.setDataCDN(path);
        }
    },
    TJSetPreloadList(paths) {
        if (GameGlobal.manager && GameGlobal.manager.setPreloadList) {
            const list = (paths || '').split(',').filter(str => !!str && !!str.trim());
            GameGlobal.manager.setPreloadList(list);
        }
    },
    TJSetArrayBuffer(buffer, offset, callbackId) {
        setArrayBuffer(buffer, offset, callbackId);
    },
    TJLaunchOperaBridge(args) {
        const res = GameGlobal.events.emit('launchOperaMsgBridgeFromWasm', args);
        if (Array.isArray(res) && res.length > 0) {
            return res[0];
        }
        return null;
    },
    TJLaunchOperaBridgeToC(callback, args) {
        moduleHelper.send('LaunchOperaBridgeToC', JSON.stringify({
            callback,
            args,
        }));
    },
};
