import { convertDataToPointer } from '../utils';
let tjOnBLECharacteristicValueChangeCallback;
const OnBLECharacteristicValueChange = (res) => {
    const deviceIdPtr = convertDataToPointer(res.deviceId);
    const serviceIdPtr = convertDataToPointer(res.serviceId);
    const characteristicIdPtr = convertDataToPointer(res.characteristicId);
    const valuePtr = convertDataToPointer(res.value);
    GameGlobal.Module.dynCall_viiiii(tjOnBLECharacteristicValueChangeCallback, deviceIdPtr, serviceIdPtr, characteristicIdPtr, valuePtr, res.value.byteLength);
    GameGlobal.Module._free(deviceIdPtr);
    GameGlobal.Module._free(serviceIdPtr);
    GameGlobal.Module._free(characteristicIdPtr);
    GameGlobal.Module._free(valuePtr);
};
function TJ_OnBLECharacteristicValueChange() {
    tj.onBLECharacteristicValueChange(OnBLECharacteristicValueChange);
}
function TJ_OffBLECharacteristicValueChange() {
    tj.offBLECharacteristicValueChange();
}
function TJ_RegisterOnBLECharacteristicValueChangeCallback(callback) {
    tjOnBLECharacteristicValueChangeCallback = callback;
}
export default {
    TJ_OnBLECharacteristicValueChange,
    TJ_OffBLECharacteristicValueChange,
    TJ_RegisterOnBLECharacteristicValueChangeCallback,
};
