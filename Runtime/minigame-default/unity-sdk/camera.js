import moduleHelper from './module-helper';
import { formatJsonStr, cacheArrayBuffer, getListObject } from './utils';
const cameraList = {};
const getObject = getListObject(cameraList, 'camera');
export default {
    TJCameraCreateCamera(conf, callbackId) {
        const obj = tj.createCamera({
            ...formatJsonStr(conf),
            success(res) {
                moduleHelper.send('CameraCreateCallback', JSON.stringify({
                    callbackId,
                    type: 'success',
                    res: JSON.stringify(res),
                }));
            },
            fail(res) {
                moduleHelper.send('CameraCreateCallback', JSON.stringify({
                    callbackId,
                    type: 'fail',
                    res: JSON.stringify(res),
                }));
            },
            complete(res) {
                moduleHelper.send('CameraCreateCallback', JSON.stringify({
                    callbackId,
                    type: 'complete',
                    res: JSON.stringify(res),
                }));
            },
        });
        cameraList[callbackId] = obj;
    },
    TJCameraCloseFrameChange(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.closeFrameChange();
    },
    TJCameraDestroy(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.destroy();
    },
    TJCameraListenFrameChange(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.listenFrameChange();
    },
    TJCameraOnAuthCancel(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify(res),
            });
            moduleHelper.send('CameraOnAuthCancelCallback', resStr);
        };
        obj.onAuthCancel(callback);
    },
    TJCameraOnCameraFrame(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            cacheArrayBuffer(id, res.data);
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify({
                    width: res.width,
                    height: res.height,
                }),
            });
            moduleHelper.send('CameraOnCameraFrameCallback', resStr);
        };
        obj.onCameraFrame(callback);
    },
    TJCameraOnStop(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify(res),
            });
            moduleHelper.send('CameraOnStopCallback', resStr);
        };
        obj.onStop(callback);
    },
};
