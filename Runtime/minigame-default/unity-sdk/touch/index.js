import { formatTouchEvent, convertOnTouchStartListenerResultToPointer } from '../utils';
let tjOnTouchCancelCallback;
let tjOnTouchEndCallback;
let tjOnTouchMoveCallback;
let tjOnTouchStartCallback;
function handleTouchEvent(res, callback) {
    const dataPtr = convertOnTouchStartListenerResultToPointer({
        touches: res.touches.map(v => formatTouchEvent(v, res.type)),
        changedTouches: res.changedTouches.map(v => formatTouchEvent(v, res.type, 1)),
        timeStamp: parseInt(res.timeStamp.toString(), 10),
    });
    GameGlobal.Module.dynCall_viii(callback, dataPtr, res.touches.length, res.changedTouches.length);
    GameGlobal.Module._free(dataPtr);
}
const OnTouchCancel = (res) => {
    handleTouchEvent(res, tjOnTouchCancelCallback);
};
const OnTouchEnd = (res) => {
    handleTouchEvent(res, tjOnTouchEndCallback);
};
const OnTouchMove = (res) => {
    handleTouchEvent(res, tjOnTouchMoveCallback);
};
const OnTouchStart = (res) => {
    handleTouchEvent(res, tjOnTouchStartCallback);
};
function TJ_OnTouchCancel() {
    tj.onTouchCancel(OnTouchCancel);
}
function TJ_OffTouchCancel() {
    tj.offTouchCancel(OnTouchCancel);
}
function TJ_OnTouchEnd() {
    tj.onTouchEnd(OnTouchEnd);
}
function TJ_OffTouchEnd() {
    tj.offTouchEnd(OnTouchEnd);
}
function TJ_OnTouchMove() {
    tj.onTouchMove(OnTouchMove);
}
function TJ_OffTouchMove() {
    tj.offTouchMove(OnTouchMove);
}
function TJ_OnTouchStart() {
    tj.onTouchStart(OnTouchStart);
}
function TJ_OffTouchStart() {
    tj.offTouchStart(OnTouchStart);
}
function TJ_RegisterOnTouchCancelCallback(callback) {
    tjOnTouchCancelCallback = callback;
}
function TJ_RegisterOnTouchEndCallback(callback) {
    tjOnTouchEndCallback = callback;
}
function TJ_RegisterOnTouchMoveCallback(callback) {
    tjOnTouchMoveCallback = callback;
}
function TJ_RegisterOnTouchStartCallback(callback) {
    tjOnTouchStartCallback = callback;
}
export default {
    TJ_OnTouchCancel,
    TJ_OffTouchCancel,
    TJ_OnTouchEnd,
    TJ_OffTouchEnd,
    TJ_OnTouchMove,
    TJ_OffTouchMove,
    TJ_OnTouchStart,
    TJ_OffTouchStart,
    TJ_RegisterOnTouchCancelCallback,
    TJ_RegisterOnTouchEndCallback,
    TJ_RegisterOnTouchMoveCallback,
    TJ_RegisterOnTouchStartCallback,
};
