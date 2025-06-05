import { formatJsonStr, formatResponse, convertDataToPointer } from '../utils';
let tjStartGyroscopeCallback;
let tjStopGyroscopeCallback;
let tjOnGyroscopeChangeCallback;
const OnGyroscopeChange = (res) => {
    formatResponse('OnGyroscopeChangeListenerResult', res);
    const xPtr = convertDataToPointer(res.x);
    const yPtr = convertDataToPointer(res.y);
    const zPtr = convertDataToPointer(res.z);
    GameGlobal.Module.dynCall_viii(tjOnGyroscopeChangeCallback, xPtr, yPtr, zPtr);
    GameGlobal.Module._free(xPtr);
    GameGlobal.Module._free(yPtr);
    GameGlobal.Module._free(zPtr);
};
function handleCallback(callback, id, callbackType, res) {
    formatResponse('GeneralCallbackResult', res);
    const idPtr = convertDataToPointer(id);
    const msgPtr = convertDataToPointer(res.errMsg);
    GameGlobal.Module.dynCall_viii(callback, idPtr, callbackType, msgPtr);
    GameGlobal.Module._free(idPtr);
    GameGlobal.Module._free(msgPtr);
}
function TJ_StartGyroscope(id, conf) {
    const config = formatJsonStr(conf);
    tj.startGyroscope({
        ...config,
        success(res) {
            handleCallback(tjStartGyroscopeCallback, id, 2, res);
        },
        fail(res) {
            handleCallback(tjStartGyroscopeCallback, id, 1, res);
        },
        complete(res) {
            handleCallback(tjStartGyroscopeCallback, id, 0, res);
        },
    });
}
function TJ_StopGyroscope(id, conf) {
    const config = formatJsonStr(conf);
    tj.stopGyroscope({
        ...config,
        success(res) {
            handleCallback(tjStopGyroscopeCallback, id, 2, res);
        },
        fail(res) {
            handleCallback(tjStopGyroscopeCallback, id, 1, res);
        },
        complete(res) {
            handleCallback(tjStopGyroscopeCallback, id, 0, res);
        },
    });
}
function TJ_OnGyroscopeChange() {
    tj.onGyroscopeChange(OnGyroscopeChange);
}
function TJ_OffGyroscopeChange() {
    tj.offGyroscopeChange();
}
function TJ_RegisterStartGyroscopeCallback(callback) {
    tjStartGyroscopeCallback = callback;
}
function TJ_RegisterStopGyroscopeCallback(callback) {
    tjStopGyroscopeCallback = callback;
}
function TJ_RegisterOnGyroscopeChangeCallback(callback) {
    tjOnGyroscopeChangeCallback = callback;
}
export default {
    TJ_StartGyroscope,
    TJ_StopGyroscope,
    TJ_OnGyroscopeChange,
    TJ_OffGyroscopeChange,
    TJ_RegisterStartGyroscopeCallback,
    TJ_RegisterStopGyroscopeCallback,
    TJ_RegisterOnGyroscopeChangeCallback,
};
