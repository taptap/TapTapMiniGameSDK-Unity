import moduleHelper from './module-helper';
import { formatJsonStr, cacheArrayBuffer, getListObject, uid } from './utils';
const recorderManagerList = {};
const getObject = getListObject(recorderManagerList, 'video');
export default {
    TJ_GetRecorderManager() {
        const id = uid();
        recorderManagerList[id] = tj.getRecorderManager();
        return id;
    },
    TJ_OnRecorderError(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify(res),
            });
            moduleHelper.send('_OnRecorderErrorCallback', resStr);
        };
        obj.onError(callback);
    },
    TJ_OnRecorderFrameRecorded(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            cacheArrayBuffer(id, res.frameBuffer);
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify({
                    frameBufferLength: res.frameBuffer.byteLength,
                    isLastFrame: res.isLastFrame,
                }),
            });
            moduleHelper.send('_OnRecorderFrameRecordedCallback', resStr);
        };
        obj.onFrameRecorded(callback);
    },
    TJ_OnRecorderInterruptionBegin(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify(res),
            });
            moduleHelper.send('_OnRecorderInterruptionBeginCallback', resStr);
        };
        obj.onInterruptionBegin(callback);
    },
    TJ_OnRecorderInterruptionEnd(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify(res),
            });
            moduleHelper.send('_OnRecorderInterruptionEndCallback', resStr);
        };
        obj.onInterruptionEnd(callback);
    },
    TJ_OnRecorderPause(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify(res),
            });
            moduleHelper.send('_OnRecorderPauseCallback', resStr);
        };
        obj.onPause(callback);
    },
    TJ_OnRecorderResume(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify(res),
            });
            moduleHelper.send('_OnRecorderResumeCallback', resStr);
        };
        obj.onResume(callback);
    },
    TJ_OnRecorderStart(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify(res),
            });
            moduleHelper.send('_OnRecorderStartCallback', resStr);
        };
        obj.onStart(callback);
    },
    TJ_OnRecorderStop(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify(res),
            });
            moduleHelper.send('_OnRecorderStopCallback', resStr);
        };
        obj.onStop(callback);
    },
    TJ_RecorderPause(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.pause();
    },
    TJ_RecorderResume(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.resume();
    },
    TJ_RecorderStart(id, option) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const conf = formatJsonStr(option);
        obj.start(conf);
    },
    TJ_RecorderStop(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.stop();
    },
};
