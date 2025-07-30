import { formatJsonStr, uid, onEventCallback, offEventCallback, getListObject, convertDataToPointer, convertInfoToPointer, formatResponse } from '../utils';
const UDPSocketList = {};
const tjUDPSocketCloseList = {};
const tjUDPSocketErrorList = {};
const tjUDPSocketListeningList = {};
const tjUDPSocketMessageList = {};
const getUDPSocketObject = getListObject(UDPSocketList, 'UDPSocket');
let tjUDPSocketOnMessageCallback;
function TJ_CreateUDPSocket() {
    const obj = tj.createUDPSocket();
    const key = uid();
    UDPSocketList[key] = obj;
    return key;
}
function TJ_UDPSocketClose(id) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    obj.close();
    delete UDPSocketList[id];
}
function TJ_UDPSocketConnect(id, option) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    obj.connect(formatJsonStr(option));
}
function TJ_UDPSocketOffClose(id) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    offEventCallback(tjUDPSocketCloseList, (v) => {
        obj.offClose(v);
    }, id);
}
function TJ_UDPSocketOffError(id) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    offEventCallback(tjUDPSocketErrorList, (v) => {
        obj.offError(v);
    }, id);
}
function TJ_UDPSocketOffListening(id) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    offEventCallback(tjUDPSocketListeningList, (v) => {
        obj.offListening(v);
    }, id);
}
function TJ_UDPSocketOffMessage(id) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    offEventCallback(tjUDPSocketMessageList, (v) => {
        obj.offMessage(v);
    }, id);
}
function TJ_UDPSocketOnClose(id) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    const callback = onEventCallback(tjUDPSocketCloseList, '_UDPSocketOnCloseCallback', id, id);
    obj.onClose(callback);
}
function TJ_UDPSocketOnError(id) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    const callback = onEventCallback(tjUDPSocketErrorList, '_UDPSocketOnErrorCallback', id, id);
    obj.onError(callback);
}
function TJ_UDPSocketOnListening(id) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    const callback = onEventCallback(tjUDPSocketListeningList, '_UDPSocketOnListeningCallback', id, id);
    obj.onListening(callback);
}
function TJ_UDPSocketOnMessage(id, needInfo) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    if (!tjUDPSocketMessageList[id]) {
        tjUDPSocketMessageList[id] = [];
    }
    const callback = (res) => {
        formatResponse('UDPSocketOnMessageListenerResult', res);
        const idPtr = convertDataToPointer(id);
        const messagePtr = convertDataToPointer(res.message);
        if (needInfo) {
            const localInfoPtr = convertInfoToPointer(res.localInfo);
            const remoteInfoPtr = convertInfoToPointer(res.remoteInfo);
            GameGlobal.Module.dynCall_viiiii(tjUDPSocketOnMessageCallback, idPtr, messagePtr, res.message.length || res.message.byteLength, localInfoPtr, remoteInfoPtr);
            GameGlobal.Module._free(localInfoPtr);
            GameGlobal.Module._free(remoteInfoPtr);
        }
        else {
            GameGlobal.Module.dynCall_viiiii(tjUDPSocketOnMessageCallback, idPtr, messagePtr, res.message.length || res.message.byteLength, 0, 0);
        }
        GameGlobal.Module._free(idPtr);
        GameGlobal.Module._free(messagePtr);
    };
    tjUDPSocketMessageList[id].push(callback);
    obj.onMessage(callback);
}
function TJ_UDPSocketSendString(id, data, param) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    const config = formatJsonStr(param);
    obj.send({
        address: config.address,
        message: data,
        port: config.port,
        setBroadcast: config.setBroadcast,
    });
}
function TJ_UDPSocketSendBuffer(id, dataPtr, dataLength, param) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    const config = formatJsonStr(param);
    obj.send({
        address: config.address,
        message: GameGlobal.Module.HEAPU8.buffer.slice(dataPtr, dataPtr + dataLength),
        port: config.port,
        length: config.length,
        offset: config.offset,
        setBroadcast: config.setBroadcast,
    });
}
function TJ_UDPSocketSetTTL(id, ttl) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    obj.setTTL(ttl);
}
function TJ_UDPSocketWriteString(id, data, param) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    const config = formatJsonStr(param);
    obj.write({
        address: config.address,
        message: data,
        port: config.port,
        setBroadcast: config.setBroadcast,
    });
}
function TJ_UDPSocketWriteBuffer(id, dataPtr, dataLength, param) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return;
    }
    const config = formatJsonStr(param);
    obj.write({
        address: config.address,
        message: GameGlobal.Module.HEAPU8.buffer.slice(dataPtr, dataPtr + dataLength),
        port: config.port,
        length: config.length,
        offset: config.offset,
        setBroadcast: config.setBroadcast,
    });
}
function TJ_UDPSocketBind(id, param) {
    const obj = getUDPSocketObject(id);
    if (!obj) {
        return 0;
    }
    const config = formatJsonStr(param);
    return obj.bind(config.port);
}
function TJ_RegisterUDPSocketOnMessageCallback(callback) {
    tjUDPSocketOnMessageCallback = callback;
}
export default {
    TJ_CreateUDPSocket,
    TJ_UDPSocketBind,
    TJ_UDPSocketClose,
    TJ_UDPSocketConnect,
    TJ_UDPSocketOffClose,
    TJ_UDPSocketOffError,
    TJ_UDPSocketOffListening,
    TJ_UDPSocketOffMessage,
    TJ_UDPSocketOnClose,
    TJ_UDPSocketOnError,
    TJ_UDPSocketOnListening,
    TJ_UDPSocketOnMessage,
    TJ_UDPSocketSendString,
    TJ_UDPSocketSendBuffer,
    TJ_UDPSocketSetTTL,
    TJ_UDPSocketWriteString,
    TJ_UDPSocketWriteBuffer,
    TJ_RegisterUDPSocketOnMessageCallback,
};
