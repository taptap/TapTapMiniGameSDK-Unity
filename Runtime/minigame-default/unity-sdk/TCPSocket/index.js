import { formatJsonStr, uid, onEventCallback, offEventCallback, getListObject, convertInfoToPointer, formatResponse, convertDataToPointer } from '../utils';
const TCPSocketList = {};
const tjTCPSocketBindWifiList = {};
const tjTCPSocketCloseList = {};
const tjTCPSocketConnectList = {};
const tjTCPSocketErrorList = {};
const tjTCPSocketMessageList = {};
const getTCPSocketObject = getListObject(TCPSocketList, 'TCPSocket');
let tjTCPSocketOnMessageCallback;
function TJ_CreateTCPSocket() {
    const obj = tj.createTCPSocket();
    const key = uid();
    TCPSocketList[key] = obj;
    return key;
}
function TJ_TCPSocketBindWifi(id, option) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    obj.bindWifi(formatJsonStr(option));
}
function TJ_TCPSocketClose(id) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    obj.close();
    delete TCPSocketList[id];
}
function TJ_TCPSocketConnect(id, option) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    obj.connect(formatJsonStr(option));
}
function TJ_TCPSocketWriteString(id, data) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    obj.write(data);
}
function TJ_TCPSocketWriteBuffer(id, dataPtr, dataLength) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    obj.write(GameGlobal.Module.HEAPU8.buffer.slice(dataPtr, dataPtr + dataLength));
}
function TJ_TCPSocketOffBindWifi(id) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    offEventCallback(tjTCPSocketBindWifiList, (v) => {
        obj.offBindWifi(v);
    }, id);
}
function TJ_TCPSocketOffClose(id) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    offEventCallback(tjTCPSocketCloseList, (v) => {
        obj.offClose(v);
    }, id);
}
function TJ_TCPSocketOffConnect(id) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    offEventCallback(tjTCPSocketConnectList, (v) => {
        obj.offConnect(v);
    }, id);
}
function TJ_TCPSocketOffError(id) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    offEventCallback(tjTCPSocketErrorList, (v) => {
        obj.offError(v);
    }, id);
}
function TJ_TCPSocketOffMessage(id) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    offEventCallback(tjTCPSocketMessageList, (v) => {
        obj.offMessage(v);
    }, id);
}
function TJ_TCPSocketOnBindWifi(id) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    const callback = onEventCallback(tjTCPSocketBindWifiList, '_TCPSocketOnBindWifiCallback', id, id);
    obj.onBindWifi(callback);
}
function TJ_TCPSocketOnClose(id) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    const callback = onEventCallback(tjTCPSocketCloseList, '_TCPSocketOnCloseCallback', id, id);
    obj.onClose(callback);
}
function TJ_TCPSocketOnConnect(id) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    const callback = onEventCallback(tjTCPSocketConnectList, '_TCPSocketOnConnectCallback', id, id);
    obj.onConnect(callback);
}
function TJ_TCPSocketOnError(id) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    const callback = onEventCallback(tjTCPSocketErrorList, '_TCPSocketOnErrorCallback', id, id);
    obj.onError(callback);
}
function TJ_TCPSocketOnMessage(id, needInfo) {
    const obj = getTCPSocketObject(id);
    if (!obj) {
        return;
    }
    if (!tjTCPSocketMessageList[id]) {
        tjTCPSocketMessageList[id] = [];
    }
    const callback = (res) => {
        formatResponse('TCPSocketOnMessageListenerResult', res);
        const idPtr = convertDataToPointer(id);
        const messagePtr = convertDataToPointer(res.message);
        if (needInfo) {
            const localInfoPtr = convertInfoToPointer(res.localInfo);
            const remoteInfoPtr = convertInfoToPointer(res.remoteInfo);
            GameGlobal.Module.dynCall_viiiii(tjTCPSocketOnMessageCallback, idPtr, messagePtr, res.message.length || res.message.byteLength, localInfoPtr, remoteInfoPtr);
            GameGlobal.Module._free(localInfoPtr);
            GameGlobal.Module._free(remoteInfoPtr);
        }
        else {
            GameGlobal.Module.dynCall_viiiii(tjTCPSocketOnMessageCallback, idPtr, messagePtr, res.message.length || res.message.byteLength, 0, 0);
        }
        GameGlobal.Module._free(idPtr);
        GameGlobal.Module._free(messagePtr);
    };
    tjTCPSocketMessageList[id].push(callback);
    obj.onMessage(callback);
}
function TJ_RegisterTCPSocketOnMessageCallback(callback) {
    tjTCPSocketOnMessageCallback = callback;
}
export default {
    TJ_CreateTCPSocket,
    TJ_TCPSocketBindWifi,
    TJ_TCPSocketClose,
    TJ_TCPSocketConnect,
    TJ_TCPSocketWriteString,
    TJ_TCPSocketWriteBuffer,
    TJ_TCPSocketOffBindWifi,
    TJ_TCPSocketOffClose,
    TJ_TCPSocketOffConnect,
    TJ_TCPSocketOffError,
    TJ_TCPSocketOffMessage,
    TJ_TCPSocketOnBindWifi,
    TJ_TCPSocketOnClose,
    TJ_TCPSocketOnConnect,
    TJ_TCPSocketOnError,
    TJ_TCPSocketOnMessage,
    TJ_RegisterTCPSocketOnMessageCallback,
};
