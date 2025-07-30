import moduleHelper from './module-helper';
import { formatJsonStr, getListObject, offEventCallback, onEventCallback } from './utils';
const uploadTaskList = {};
const tjUpdateTaskOnProgressList = {};
const tjUpdateTaskOnHeadersList = {};
const getObject = getListObject(uploadTaskList, 'uploadTask');
export default {
    TJ_UploadFile(option, callbackId) {
        const conf = formatJsonStr(option);
        const obj = tj.uploadFile({
            ...conf,
            success: (res) => {
                moduleHelper.send('UploadFileCallback', JSON.stringify({
                    callbackId,
                    type: 'success',
                    res: JSON.stringify(res),
                }));
            },
            fail: (res) => {
                moduleHelper.send('UploadFileCallback', JSON.stringify({
                    callbackId,
                    type: 'fail',
                    res: JSON.stringify(res),
                }));
            },
            complete: (res) => {
                moduleHelper.send('UploadFileCallback', JSON.stringify({
                    callbackId,
                    type: 'complete',
                    res: JSON.stringify(res),
                }));
                setTimeout(() => {
                    if (uploadTaskList) {
                        delete uploadTaskList[callbackId];
                    }
                }, 0);
            },
        });
        uploadTaskList[callbackId] = obj;
    },
    TJUploadTaskAbort(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.abort();
    },
    TJUploadTaskOffHeadersReceived(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        offEventCallback(tjUpdateTaskOnHeadersList, (v) => {
            obj.offHeadersReceived(v);
        }, id);
    },
    TJUploadTaskOffProgressUpdate(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        offEventCallback(tjUpdateTaskOnProgressList, (v) => {
            obj.offProgressUpdate(v);
        }, id);
    },
    TJUploadTaskOnHeadersReceived(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const callback = onEventCallback(tjUpdateTaskOnHeadersList, '_OnHeadersReceivedCallback', id);
        obj.onHeadersReceived(callback);
    },
    TJUploadTaskOnProgressUpdate(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const callback = onEventCallback(tjUpdateTaskOnProgressList, '_OnProgressUpdateCallback', id);
        obj.onProgressUpdate(callback);
    },
};
