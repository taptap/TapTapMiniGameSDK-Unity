
import moduleHelper from './module-helper';
import { uid, formatResponse, formatJsonStr, onEventCallback, offEventCallback, getListObject, stringifyRes } from './utils';
let OnAccelerometerChangeList;
let OnAudioInterruptionBeginList;
let OnAudioInterruptionEndList;
let OnBLEConnectionStateChangeList;
let OnBLEMTUChangeList;
let OnBLEPeripheralConnectionStateChangedList;
let OnBeaconServiceChangeList;
let OnBeaconUpdateList;
let OnBluetoothAdapterStateChangeList;
let OnBluetoothDeviceFoundList;
let OnCompassChangeList;
let OnDeviceMotionChangeList;
let OnDeviceOrientationChangeList;
let OnErrorList;
let OnHideList;
let OnInteractiveStorageModifiedList;
let OnKeyDownList;
let OnKeyUpList;
let OnKeyboardCompleteList;
let OnKeyboardConfirmList;
let OnKeyboardHeightChangeList;
let OnKeyboardInputList;
let OnMemoryWarningList;
let OnMouseDownList;
let OnMouseMoveList;
let OnMouseUpList;
let OnNetworkStatusChangeList;
let OnNetworkWeakChangeList;
let OnScreenRecordingStateChangedList;
let OnShowList;
let OnUnhandledRejectionList;
let OnUserCaptureScreenList;
let OnVoIPChatInterruptedList;
let OnVoIPChatMembersChangedList;
let OnVoIPChatSpeakersChangedList;
let OnVoIPChatStateChangedList;
let OnWheelList;
let OnWindowResizeList;
let tjOnAddToFavoritesResolveConf;
let tjOnCopyUrlResolveConf;
let tjOnHandoffResolveConf;
let tjOnShareTimelineResolveConf;
let tjOnGameLiveStateChangeResolveConf;
const DownloadTaskList = {};
const FeedbackButtonList = {};
const LogManagerList = {};
const RealtimeLogManagerList = {};
const UpdateManagerList = {};
const VideoDecoderList = {};
const tjDownloadTaskHeadersReceivedList = {};
const tjDownloadTaskProgressUpdateList = {};
const tjFeedbackButtonTapList = {};
const tjVideoDecoderList = {};
const getDownloadTaskObject = getListObject(DownloadTaskList, 'DownloadTask');
const getFeedbackButtonObject = getListObject(FeedbackButtonList, 'FeedbackButton');
const getLogManagerObject = getListObject(LogManagerList, 'LogManager');
const getRealtimeLogManagerObject = getListObject(RealtimeLogManagerList, 'RealtimeLogManager');
const getUpdateManagerObject = getListObject(UpdateManagerList, 'UpdateManager');
const getVideoDecoderObject = getListObject(VideoDecoderList, 'VideoDecoder');
export default {
    TJ_AddCard(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.addCard({
            ...config,
            success(res) {
                formatResponse('AddCardSuccessCallbackResult', res);
                moduleHelper.send('AddCardCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('AddCardCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('AddCardCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_AuthPrivateMessage(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.authPrivateMessage({
            ...config,
            success(res) {
                formatResponse('AuthPrivateMessageSuccessCallbackResult', res);
                moduleHelper.send('AuthPrivateMessageCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('AuthPrivateMessageCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('AuthPrivateMessageCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_Authorize(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.authorize({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('AuthorizeCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('AuthorizeCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('AuthorizeCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_CheckIsAddedToMyMiniProgram(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.checkIsAddedToMyMiniProgram({
            ...config,
            success(res) {
                formatResponse('CheckIsAddedToMyMiniProgramSuccessCallbackResult', res);
                moduleHelper.send('CheckIsAddedToMyMiniProgramCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('CheckIsAddedToMyMiniProgramCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('CheckIsAddedToMyMiniProgramCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_CheckSession(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.checkSession({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('CheckSessionCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('CheckSessionCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('CheckSessionCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ChooseImage(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.chooseImage({
            ...config,
            success(res) {
                formatResponse('ChooseImageSuccessCallbackResult', res);
                moduleHelper.send('ChooseImageCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ChooseImageCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ChooseImageCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ChooseMedia(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.chooseMedia({
            ...config,
            success(res) {
                formatResponse('ChooseMediaSuccessCallbackResult', res);
                moduleHelper.send('ChooseMediaCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ChooseMediaCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ChooseMediaCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ChooseMessageFile(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.chooseMessageFile({
            ...config,
            success(res) {
                formatResponse('ChooseMessageFileSuccessCallbackResult', res);
                moduleHelper.send('ChooseMessageFileCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ChooseMessageFileCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ChooseMessageFileCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_CloseBLEConnection(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.closeBLEConnection({
            ...config,
            success(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('CloseBLEConnectionCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('CloseBLEConnectionCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('CloseBLEConnectionCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_CloseBluetoothAdapter(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.closeBluetoothAdapter({
            ...config,
            success(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('CloseBluetoothAdapterCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('CloseBluetoothAdapterCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('CloseBluetoothAdapterCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_CompressImage(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.compressImage({
            ...config,
            success(res) {
                formatResponse('CompressImageSuccessCallbackResult', res);
                moduleHelper.send('CompressImageCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('CompressImageCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('CompressImageCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_CreateBLEConnection(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.createBLEConnection({
            ...config,
            success(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('CreateBLEConnectionCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('CreateBLEConnectionCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('CreateBLEConnectionCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_CreateBLEPeripheralServer(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.createBLEPeripheralServer({
            ...config,
            success(res) {
                formatResponse('CreateBLEPeripheralServerSuccessCallbackResult', res);
                moduleHelper.send('CreateBLEPeripheralServerCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('CreateBLEPeripheralServerCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('CreateBLEPeripheralServerCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ExitMiniProgram(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.exitMiniProgram({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ExitMiniProgramCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ExitMiniProgramCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ExitMiniProgramCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ExitVoIPChat(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.exitVoIPChat({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ExitVoIPChatCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ExitVoIPChatCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ExitVoIPChatCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_FaceDetect(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.faceDetect({
            ...config,
            success(res) {
                formatResponse('FaceDetectSuccessCallbackResult', res);
                moduleHelper.send('FaceDetectCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('FaceDetectCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('FaceDetectCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetAvailableAudioSources(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getAvailableAudioSources({
            ...config,
            success(res) {
                formatResponse('GetAvailableAudioSourcesSuccessCallbackResult', res);
                moduleHelper.send('GetAvailableAudioSourcesCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetAvailableAudioSourcesCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetAvailableAudioSourcesCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetBLEDeviceCharacteristics(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getBLEDeviceCharacteristics({
            ...config,
            success(res) {
                formatResponse('GetBLEDeviceCharacteristicsSuccessCallbackResult', res);
                moduleHelper.send('GetBLEDeviceCharacteristicsCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('GetBLEDeviceCharacteristicsCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('GetBLEDeviceCharacteristicsCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetBLEDeviceRSSI(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getBLEDeviceRSSI({
            ...config,
            success(res) {
                formatResponse('GetBLEDeviceRSSISuccessCallbackResult', res);
                moduleHelper.send('GetBLEDeviceRSSICallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetBLEDeviceRSSICallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetBLEDeviceRSSICallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetBLEDeviceServices(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getBLEDeviceServices({
            ...config,
            success(res) {
                formatResponse('GetBLEDeviceServicesSuccessCallbackResult', res);
                moduleHelper.send('GetBLEDeviceServicesCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('GetBLEDeviceServicesCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('GetBLEDeviceServicesCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetBLEMTU(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getBLEMTU({
            ...config,
            success(res) {
                formatResponse('GetBLEMTUSuccessCallbackResult', res);
                moduleHelper.send('GetBLEMTUCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('GetBLEMTUCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('GetBLEMTUCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetBackgroundFetchData(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getBackgroundFetchData({
            ...config,
            success(res) {
                formatResponse('GetBackgroundFetchDataSuccessCallbackResult', res);
                moduleHelper.send('GetBackgroundFetchDataCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetBackgroundFetchDataCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetBackgroundFetchDataCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetBackgroundFetchToken(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getBackgroundFetchToken({
            ...config,
            success(res) {
                formatResponse('GetBackgroundFetchTokenSuccessCallbackResult', res);
                moduleHelper.send('GetBackgroundFetchTokenCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetBackgroundFetchTokenCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetBackgroundFetchTokenCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetBatteryInfo(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getBatteryInfo({
            ...config,
            success(res) {
                formatResponse('GetBatteryInfoSuccessCallbackResult', res);
                moduleHelper.send('GetBatteryInfoCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetBatteryInfoCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetBatteryInfoCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetBeacons(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getBeacons({
            ...config,
            success(res) {
                formatResponse('GetBeaconsSuccessCallbackResult', res);
                moduleHelper.send('GetBeaconsCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BeaconError', res);
                moduleHelper.send('GetBeaconsCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BeaconError', res);
                moduleHelper.send('GetBeaconsCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetBluetoothAdapterState(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getBluetoothAdapterState({
            ...config,
            success(res) {
                formatResponse('GetBluetoothAdapterStateSuccessCallbackResult', res);
                moduleHelper.send('GetBluetoothAdapterStateCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('GetBluetoothAdapterStateCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('GetBluetoothAdapterStateCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetBluetoothDevices(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getBluetoothDevices({
            ...config,
            success(res) {
                formatResponse('GetBluetoothDevicesSuccessCallbackResult', res);
                moduleHelper.send('GetBluetoothDevicesCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('GetBluetoothDevicesCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('GetBluetoothDevicesCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetChannelsLiveInfo(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getChannelsLiveInfo({
            ...config,
            success(res) {
                formatResponse('GetChannelsLiveInfoSuccessCallbackResult', res);
                moduleHelper.send('GetChannelsLiveInfoCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetChannelsLiveInfoCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetChannelsLiveInfoCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetChannelsLiveNoticeInfo(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getChannelsLiveNoticeInfo({
            ...config,
            success(res) {
                formatResponse('GetChannelsLiveNoticeInfoSuccessCallbackResult', res);
                moduleHelper.send('GetChannelsLiveNoticeInfoCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetChannelsLiveNoticeInfoCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetChannelsLiveNoticeInfoCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetClipboardData(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getClipboardData({
            ...config,
            success(res) {
                formatResponse('GetClipboardDataSuccessCallbackOption', res);
                moduleHelper.send('GetClipboardDataCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetClipboardDataCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetClipboardDataCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetConnectedBluetoothDevices(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getConnectedBluetoothDevices({
            ...config,
            success(res) {
                formatResponse('GetConnectedBluetoothDevicesSuccessCallbackResult', res);
                moduleHelper.send('GetConnectedBluetoothDevicesCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('GetConnectedBluetoothDevicesCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('GetConnectedBluetoothDevicesCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetExtConfig(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getExtConfig({
            ...config,
            success(res) {
                formatResponse('GetExtConfigSuccessCallbackResult', res);
                moduleHelper.send('GetExtConfigCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetExtConfigCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetExtConfigCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetFuzzyLocation(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getFuzzyLocation({
            ...config,
            success(res) {
                formatResponse('GetFuzzyLocationSuccessCallbackResult', res);
                moduleHelper.send('GetFuzzyLocationCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetFuzzyLocationCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetFuzzyLocationCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetGameClubData(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getGameClubData({
            ...config,
            success(res) {
                formatResponse('GetGameClubDataSuccessCallbackResult', res);
                moduleHelper.send('GetGameClubDataCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetGameClubDataCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetGameClubDataCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetGroupEnterInfo(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getGroupEnterInfo({
            ...config,
            success(res) {
                formatResponse('GetGroupEnterInfoSuccessCallbackResult', res);
                moduleHelper.send('GetGroupEnterInfoCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetGroupEnterInfoCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetGroupEnterInfoCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetInferenceEnvInfo(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getInferenceEnvInfo({
            ...config,
            success(res) {
                formatResponse('GetInferenceEnvInfoSuccessCallbackResult', res);
                moduleHelper.send('GetInferenceEnvInfoCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetInferenceEnvInfoCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetInferenceEnvInfoCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetLocalIPAddress(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getLocalIPAddress({
            ...config,
            success(res) {
                formatResponse('GetLocalIPAddressSuccessCallbackResult', res);
                moduleHelper.send('GetLocalIPAddressCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetLocalIPAddressCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetLocalIPAddressCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetNetworkType(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getNetworkType({
            ...config,
            success(res) {
                formatResponse('GetNetworkTypeSuccessCallbackResult', res);
                moduleHelper.send('GetNetworkTypeCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetNetworkTypeCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetNetworkTypeCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetPrivacySetting(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getPrivacySetting({
            ...config,
            success(res) {
                formatResponse('GetPrivacySettingSuccessCallbackResult', res);
                moduleHelper.send('GetPrivacySettingCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetPrivacySettingCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetPrivacySettingCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetScreenBrightness(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getScreenBrightness({
            ...config,
            success(res) {
                formatResponse('GetScreenBrightnessSuccessCallbackOption', res);
                moduleHelper.send('GetScreenBrightnessCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetScreenBrightnessCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetScreenBrightnessCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetScreenRecordingState(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getScreenRecordingState({
            ...config,
            success(res) {
                formatResponse('GetScreenRecordingStateSuccessCallbackResult', res);
                moduleHelper.send('GetScreenRecordingStateCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetScreenRecordingStateCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetScreenRecordingStateCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetSetting(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getSetting({
            ...config,
            success(res) {
                formatResponse('GetSettingSuccessCallbackResult', res);
                moduleHelper.send('GetSettingCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetSettingCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetSettingCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetShareInfo(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getShareInfo({
            ...config,
            success(res) {
                formatResponse('GetGroupEnterInfoSuccessCallbackResult', res);
                moduleHelper.send('GetShareInfoCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetShareInfoCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetShareInfoCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetStorageInfo(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getStorageInfo({
            ...config,
            success(res) {
                formatResponse('GetStorageInfoSuccessCallbackOption', res);
                moduleHelper.send('GetStorageInfoCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetStorageInfoCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetStorageInfoCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetSystemInfo(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getSystemInfo({
            ...config,
            success(res) {
                formatResponse('SystemInfo', res);
                moduleHelper.send('GetSystemInfoCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetSystemInfoCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetSystemInfoCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetSystemInfoAsync(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getSystemInfoAsync({
            ...config,
            success(res) {
                formatResponse('SystemInfo', res);
                moduleHelper.send('GetSystemInfoAsyncCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetSystemInfoAsyncCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetSystemInfoAsyncCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetUserInfo(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getUserInfo({
            ...config,
            success(res) {
                formatResponse('GetUserInfoSuccessCallbackResult', res);
                moduleHelper.send('GetUserInfoCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetUserInfoCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetUserInfoCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetUserInteractiveStorage(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getUserInteractiveStorage({
            ...config,
            success(res) {
                formatResponse('GetUserInteractiveStorageSuccessCallbackResult', res);
                moduleHelper.send('GetUserInteractiveStorageCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GetUserInteractiveStorageFailCallbackResult', res);
                moduleHelper.send('GetUserInteractiveStorageCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetUserInteractiveStorageCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetWeRunData(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getWeRunData({
            ...config,
            success(res) {
                formatResponse('GetWeRunDataSuccessCallbackResult', res);
                moduleHelper.send('GetWeRunDataCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetWeRunDataCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetWeRunDataCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_HideKeyboard(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.hideKeyboard({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('HideKeyboardCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('HideKeyboardCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('HideKeyboardCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_HideLoading(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.hideLoading({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('HideLoadingCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('HideLoadingCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('HideLoadingCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_HideShareMenu(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.hideShareMenu({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('HideShareMenuCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('HideShareMenuCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('HideShareMenuCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_HideToast(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.hideToast({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('HideToastCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('HideToastCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('HideToastCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_InitFaceDetect(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.initFaceDetect({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('InitFaceDetectCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('InitFaceDetectCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('InitFaceDetectCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_IsBluetoothDevicePaired(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.isBluetoothDevicePaired({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('IsBluetoothDevicePairedCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('IsBluetoothDevicePairedCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('IsBluetoothDevicePairedCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_JoinVoIPChat(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.joinVoIPChat({
            ...config,
            success(res) {
                formatResponse('JoinVoIPChatSuccessCallbackResult', res);
                moduleHelper.send('JoinVoIPChatCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('JoinVoIPChatError', res);
                moduleHelper.send('JoinVoIPChatCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('JoinVoIPChatError', res);
                moduleHelper.send('JoinVoIPChatCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_Login(conf, callbackId) {
        const config = formatJsonStr(conf);
        if (!config.timeout) {
            delete config.timeout;
        }
        tj.login({
            ...config,
            success(res) {
                formatResponse('LoginSuccessCallbackResult', res);
                moduleHelper.send('LoginCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('RequestFailCallbackErr', res);
                moduleHelper.send('LoginCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('LoginCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_MakeBluetoothPair(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.makeBluetoothPair({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('MakeBluetoothPairCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('MakeBluetoothPairCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('MakeBluetoothPairCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_NavigateToMiniProgram(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.navigateToMiniProgram({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('NavigateToMiniProgramCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('NavigateToMiniProgramCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('NavigateToMiniProgramCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_NotifyBLECharacteristicValueChange(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.notifyBLECharacteristicValueChange({
            ...config,
            success(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('NotifyBLECharacteristicValueChangeCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('NotifyBLECharacteristicValueChangeCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('NotifyBLECharacteristicValueChangeCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenAppAuthorizeSetting(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openAppAuthorizeSetting({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenAppAuthorizeSettingCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenAppAuthorizeSettingCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenAppAuthorizeSettingCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenBluetoothAdapter(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openBluetoothAdapter({
            ...config,
            success(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('OpenBluetoothAdapterCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('OpenBluetoothAdapterCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('OpenBluetoothAdapterCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenCard(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openCard({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenCardCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenCardCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenCardCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenChannelsActivity(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openChannelsActivity({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsActivityCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsActivityCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsActivityCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenChannelsEvent(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openChannelsEvent({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsEventCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsEventCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsEventCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenChannelsLive(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openChannelsLive({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsLiveCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsLiveCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsLiveCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenChannelsUserProfile(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openChannelsUserProfile({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsUserProfileCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsUserProfileCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsUserProfileCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenCustomerServiceChat(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openCustomerServiceChat({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenCustomerServiceChatCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenCustomerServiceChatCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenCustomerServiceChatCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenCustomerServiceConversation(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openCustomerServiceConversation({
            ...config,
            success(res) {
                formatResponse('OpenCustomerServiceConversationSuccessCallbackResult', res);
                moduleHelper.send('OpenCustomerServiceConversationCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenCustomerServiceConversationCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenCustomerServiceConversationCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenPrivacyContract(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openPrivacyContract({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenPrivacyContractCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenPrivacyContractCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenPrivacyContractCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenSetting(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openSetting({
            ...config,
            success(res) {
                formatResponse('OpenSettingSuccessCallbackResult', res);
                moduleHelper.send('OpenSettingCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenSettingCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenSettingCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenSystemBluetoothSetting(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openSystemBluetoothSetting({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenSystemBluetoothSettingCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenSystemBluetoothSettingCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenSystemBluetoothSettingCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_PreviewImage(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.previewImage({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('PreviewImageCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('PreviewImageCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('PreviewImageCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_PreviewMedia(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.previewMedia({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('PreviewMediaCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('PreviewMediaCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('PreviewMediaCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ReadBLECharacteristicValue(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.readBLECharacteristicValue({
            ...config,
            success(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('ReadBLECharacteristicValueCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('ReadBLECharacteristicValueCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('ReadBLECharacteristicValueCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_RemoveStorage(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.removeStorage({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RemoveStorageCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RemoveStorageCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RemoveStorageCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_RemoveUserCloudStorage(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.removeUserCloudStorage({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RemoveUserCloudStorageCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RemoveUserCloudStorageCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RemoveUserCloudStorageCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ReportScene(conf, callbackId) {
        const config = formatJsonStr(conf);
        if (GameGlobal.manager && GameGlobal.manager.setGameStage) {
            GameGlobal.manager.setGameStage(config.sceneId);
        }
        tj.reportScene({
            ...config,
            success(res) {
                formatResponse('ReportSceneSuccessCallbackResult', res);
                moduleHelper.send('ReportSceneCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('ReportSceneFailCallbackErr', res);
                moduleHelper.send('ReportSceneCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('ReportSceneError', res);
                moduleHelper.send('ReportSceneCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_RequestMidasFriendPayment(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.requestMidasFriendPayment({
            ...config,
            success(res) {
                formatResponse('RequestMidasFriendPaymentSuccessCallbackResult', res);
                moduleHelper.send('RequestMidasFriendPaymentCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('MidasFriendPaymentError', res);
                moduleHelper.send('RequestMidasFriendPaymentCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('MidasFriendPaymentError', res);
                moduleHelper.send('RequestMidasFriendPaymentCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_RequestMidasPayment(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.requestMidasPayment({
            ...config,
            success(res) {
                formatResponse('RequestMidasPaymentSuccessCallbackResult', res);
                moduleHelper.send('RequestMidasPaymentCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('RequestMidasPaymentFailCallbackErr', res);
                moduleHelper.send('RequestMidasPaymentCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('MidasPaymentError', res);
                moduleHelper.send('RequestMidasPaymentCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_RequestSubscribeMessage(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.requestSubscribeMessage({
            ...config,
            success(res) {
                formatResponse('RequestSubscribeMessageSuccessCallbackResult', res);
                moduleHelper.send('RequestSubscribeMessageCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('RequestSubscribeMessageFailCallbackResult', res);
                moduleHelper.send('RequestSubscribeMessageCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RequestSubscribeMessageCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_RequestSubscribeSystemMessage(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.requestSubscribeSystemMessage({
            ...config,
            success(res) {
                formatResponse('RequestSubscribeSystemMessageSuccessCallbackResult', res);
                moduleHelper.send('RequestSubscribeSystemMessageCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('RequestSubscribeMessageFailCallbackResult', res);
                moduleHelper.send('RequestSubscribeSystemMessageCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RequestSubscribeSystemMessageCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_RequirePrivacyAuthorize(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.requirePrivacyAuthorize({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RequirePrivacyAuthorizeCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RequirePrivacyAuthorizeCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RequirePrivacyAuthorizeCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_RestartMiniProgram(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.restartMiniProgram({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RestartMiniProgramCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RestartMiniProgramCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RestartMiniProgramCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SaveFileToDisk(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.saveFileToDisk({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SaveFileToDiskCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SaveFileToDiskCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SaveFileToDiskCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SaveImageToPhotosAlbum(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.saveImageToPhotosAlbum({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SaveImageToPhotosAlbumCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SaveImageToPhotosAlbumCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SaveImageToPhotosAlbumCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ScanCode(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.scanCode({
            ...config,
            success(res) {
                formatResponse('ScanCodeSuccessCallbackResult', res);
                moduleHelper.send('ScanCodeCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ScanCodeCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ScanCodeCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SetBLEMTU(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.setBLEMTU({
            ...config,
            success(res) {
                formatResponse('SetBLEMTUSuccessCallbackResult', res);
                moduleHelper.send('SetBLEMTUCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('SetBLEMTUFailCallbackResult', res);
                moduleHelper.send('SetBLEMTUCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetBLEMTUCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SetBackgroundFetchToken(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.setBackgroundFetchToken({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetBackgroundFetchTokenCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetBackgroundFetchTokenCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetBackgroundFetchTokenCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SetClipboardData(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.setClipboardData({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetClipboardDataCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetClipboardDataCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetClipboardDataCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SetDeviceOrientation(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.setDeviceOrientation({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetDeviceOrientationCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetDeviceOrientationCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetDeviceOrientationCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SetEnableDebug(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.setEnableDebug({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetEnableDebugCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetEnableDebugCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetEnableDebugCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SetInnerAudioOption(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.setInnerAudioOption({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetInnerAudioOptionCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetInnerAudioOptionCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetInnerAudioOptionCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SetKeepScreenOn(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.setKeepScreenOn({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetKeepScreenOnCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetKeepScreenOnCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetKeepScreenOnCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SetMenuStyle(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.setMenuStyle({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetMenuStyleCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetMenuStyleCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetMenuStyleCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SetScreenBrightness(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.setScreenBrightness({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetScreenBrightnessCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetScreenBrightnessCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetScreenBrightnessCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SetStatusBarStyle(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.setStatusBarStyle({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetStatusBarStyleCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetStatusBarStyleCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetStatusBarStyleCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SetUserCloudStorage(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.setUserCloudStorage({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetUserCloudStorageCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetUserCloudStorageCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetUserCloudStorageCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_SetVisualEffectOnCapture(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.setVisualEffectOnCapture({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetVisualEffectOnCaptureCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetVisualEffectOnCaptureCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('SetVisualEffectOnCaptureCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ShowActionSheet(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.showActionSheet({
            ...config,
            success(res) {
                formatResponse('ShowActionSheetSuccessCallbackResult', res);
                moduleHelper.send('ShowActionSheetCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowActionSheetCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowActionSheetCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ShowKeyboard(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.showKeyboard({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowKeyboardCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowKeyboardCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowKeyboardCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ShowLoading(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.showLoading({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowLoadingCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowLoadingCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowLoadingCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ShowModal(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.showModal({
            ...config,
            success(res) {
                formatResponse('ShowModalSuccessCallbackResult', res);
                moduleHelper.send('ShowModalCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowModalCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowModalCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ShowShareImageMenu(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.showShareImageMenu({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowShareImageMenuCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowShareImageMenuCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowShareImageMenuCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ShowShareMenu(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.showShareMenu({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowShareMenuCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowShareMenuCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowShareMenuCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ShowToast(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.showToast({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowToastCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowToastCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('ShowToastCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_StartAccelerometer(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.startAccelerometer({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StartAccelerometerCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StartAccelerometerCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StartAccelerometerCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_StartBeaconDiscovery(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.startBeaconDiscovery({
            ...config,
            success(res) {
                formatResponse('BeaconError', res);
                moduleHelper.send('StartBeaconDiscoveryCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BeaconError', res);
                moduleHelper.send('StartBeaconDiscoveryCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BeaconError', res);
                moduleHelper.send('StartBeaconDiscoveryCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_StartBluetoothDevicesDiscovery(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.startBluetoothDevicesDiscovery({
            ...config,
            success(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('StartBluetoothDevicesDiscoveryCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('StartBluetoothDevicesDiscoveryCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('StartBluetoothDevicesDiscoveryCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_StartCompass(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.startCompass({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StartCompassCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StartCompassCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StartCompassCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_StartDeviceMotionListening(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.startDeviceMotionListening({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StartDeviceMotionListeningCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StartDeviceMotionListeningCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StartDeviceMotionListeningCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_StopAccelerometer(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.stopAccelerometer({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StopAccelerometerCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StopAccelerometerCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StopAccelerometerCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_StopBeaconDiscovery(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.stopBeaconDiscovery({
            ...config,
            success(res) {
                formatResponse('BeaconError', res);
                moduleHelper.send('StopBeaconDiscoveryCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BeaconError', res);
                moduleHelper.send('StopBeaconDiscoveryCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BeaconError', res);
                moduleHelper.send('StopBeaconDiscoveryCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_StopBluetoothDevicesDiscovery(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.stopBluetoothDevicesDiscovery({
            ...config,
            success(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('StopBluetoothDevicesDiscoveryCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('StopBluetoothDevicesDiscoveryCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('StopBluetoothDevicesDiscoveryCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_StopCompass(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.stopCompass({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StopCompassCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StopCompassCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StopCompassCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_StopDeviceMotionListening(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.stopDeviceMotionListening({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StopDeviceMotionListeningCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StopDeviceMotionListeningCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StopDeviceMotionListeningCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_StopFaceDetect(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.stopFaceDetect({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StopFaceDetectCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StopFaceDetectCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StopFaceDetectCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_UpdateKeyboard(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.updateKeyboard({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('UpdateKeyboardCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('UpdateKeyboardCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('UpdateKeyboardCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_UpdateShareMenu(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.updateShareMenu({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('UpdateShareMenuCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('UpdateShareMenuCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('UpdateShareMenuCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_UpdateVoIPChatMuteConfig(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.updateVoIPChatMuteConfig({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('UpdateVoIPChatMuteConfigCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('UpdateVoIPChatMuteConfigCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('UpdateVoIPChatMuteConfigCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_UpdateWeChatApp(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.updateWeChatApp({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('UpdateWeChatAppCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('UpdateWeChatAppCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('UpdateWeChatAppCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_VibrateLong(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.vibrateLong({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('VibrateLongCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('VibrateLongCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('VibrateLongCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_VibrateShort(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.vibrateShort({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('VibrateShortCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('VibrateShortFailCallbackResult', res);
                moduleHelper.send('VibrateShortCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('VibrateShortCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_WriteBLECharacteristicValue(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.writeBLECharacteristicValue({
            ...config,
            success(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('WriteBLECharacteristicValueCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('WriteBLECharacteristicValueCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('BluetoothError', res);
                moduleHelper.send('WriteBLECharacteristicValueCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_StartGameLive(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.startGameLive({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StartGameLiveCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StartGameLiveCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('StartGameLiveCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_CheckGameLiveEnabled(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.checkGameLiveEnabled({
            ...config,
            success(res) {
                formatResponse('CheckGameLiveEnabledSuccessCallbackOption', res);
                moduleHelper.send('CheckGameLiveEnabledCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('CheckGameLiveEnabledCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('CheckGameLiveEnabledCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetUserCurrentGameliveInfo(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getUserCurrentGameliveInfo({
            ...config,
            success(res) {
                formatResponse('GetUserCurrentGameliveInfoSuccessCallbackOption', res);
                moduleHelper.send('GetUserCurrentGameliveInfoCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetUserCurrentGameliveInfoCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetUserCurrentGameliveInfoCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetUserRecentGameLiveInfo(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getUserRecentGameLiveInfo({
            ...config,
            success(res) {
                formatResponse('GetUserGameLiveDetailsSuccessCallbackOption', res);
                moduleHelper.send('GetUserRecentGameLiveInfoCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetUserRecentGameLiveInfoCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetUserRecentGameLiveInfoCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_GetUserGameLiveDetails(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getUserGameLiveDetails({
            ...config,
            success(res) {
                formatResponse('GetUserGameLiveDetailsSuccessCallbackOption', res);
                moduleHelper.send('GetUserGameLiveDetailsCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetUserGameLiveDetailsCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('GetUserGameLiveDetailsCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenChannelsLiveCollection(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openChannelsLiveCollection({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsLiveCollectionCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsLiveCollectionCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenChannelsLiveCollectionCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenPage(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openPage({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenPageCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenPageCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenPageCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_RequestMidasPaymentGameItem(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.requestMidasPaymentGameItem({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RequestMidasPaymentGameItemCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('MidasPaymentGameItemError', res);
                moduleHelper.send('RequestMidasPaymentGameItemCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('MidasPaymentGameItemError', res);
                moduleHelper.send('RequestMidasPaymentGameItemCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_RequestSubscribeLiveActivity(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.requestSubscribeLiveActivity({
            ...config,
            success(res) {
                formatResponse('RequestSubscribeLiveActivitySuccessCallbackResult', res);
                moduleHelper.send('RequestSubscribeLiveActivityCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RequestSubscribeLiveActivityCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('RequestSubscribeLiveActivityCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_OpenBusinessView(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.openBusinessView({
            ...config,
            success(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenBusinessViewCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenBusinessViewCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('OpenBusinessViewCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_ExitPointerLock() {
        tj.exitPointerLock();
    },
    TJ_OperateGameRecorderVideo(option) {
        tj.operateGameRecorderVideo(formatJsonStr(option));
    },
    TJ_RemoveStorageSync(key) {
        tj.removeStorageSync(key);
    },
    TJ_ReportEvent(eventId, data) {
        tj.reportEvent(eventId, formatJsonStr(data));
    },
    TJ_ReportMonitor(name, value) {
        tj.reportMonitor(name, value);
    },
    TJ_ReportPerformance(id, value, dimensions) {
        tj.reportPerformance(id, value, dimensions);
    },
    TJ_ReportUserBehaviorBranchAnalytics(option) {
        tj.reportUserBehaviorBranchAnalytics(formatJsonStr(option));
    },
    TJ_RequestPointerLock() {
        tj.requestPointerLock();
    },
    TJ_ReserveChannelsLive(option) {
        tj.reserveChannelsLive(formatJsonStr(option));
    },
    TJ_RevokeBufferURL(url) {
        tj.revokeBufferURL(url);
    },
    TJ_SetPreferredFramesPerSecond(fps) {
        tj.setPreferredFramesPerSecond(fps);
    },
    TJ_SetStorageSync(key, data) {
        tj.setStorageSync(key, formatJsonStr(data));
    },
    TJ_ShareAppMessage(option) {
        tj.shareAppMessage(formatJsonStr(option));
    },
    TJ_TriggerGC() {
        tj.triggerGC();
    },
    TJ_OnAccelerometerChange() {
        if (!OnAccelerometerChangeList) {
            OnAccelerometerChangeList = [];
        }
        const callback = (res) => {
            formatResponse('OnAccelerometerChangeListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnAccelerometerChangeCallback', resStr);
        };
        OnAccelerometerChangeList.push(callback);
        tj.onAccelerometerChange(callback);
    },
    TJ_OffAccelerometerChange() {
        (OnAccelerometerChangeList || []).forEach((v) => {
            tj.offAccelerometerChange(v);
        });
    },
    TJ_OnAudioInterruptionBegin() {
        if (!OnAudioInterruptionBeginList) {
            OnAudioInterruptionBeginList = [];
        }
        const callback = (res) => {
            formatResponse('GeneralCallbackResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnAudioInterruptionBeginCallback', resStr);
        };
        OnAudioInterruptionBeginList.push(callback);
        tj.onAudioInterruptionBegin(callback);
    },
    TJ_OffAudioInterruptionBegin() {
        (OnAudioInterruptionBeginList || []).forEach((v) => {
            tj.offAudioInterruptionBegin(v);
        });
    },
    TJ_OnAudioInterruptionEnd() {
        if (!OnAudioInterruptionEndList) {
            OnAudioInterruptionEndList = [];
        }
        const callback = (res) => {
            formatResponse('GeneralCallbackResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnAudioInterruptionEndCallback', resStr);
        };
        OnAudioInterruptionEndList.push(callback);
        tj.onAudioInterruptionEnd(callback);
    },
    TJ_OffAudioInterruptionEnd() {
        (OnAudioInterruptionEndList || []).forEach((v) => {
            tj.offAudioInterruptionEnd(v);
        });
    },
    TJ_OnBLEConnectionStateChange() {
        if (!OnBLEConnectionStateChangeList) {
            OnBLEConnectionStateChangeList = [];
        }
        const callback = (res) => {
            formatResponse('OnBLEConnectionStateChangeListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnBLEConnectionStateChangeCallback', resStr);
        };
        OnBLEConnectionStateChangeList.push(callback);
        tj.onBLEConnectionStateChange(callback);
    },
    TJ_OffBLEConnectionStateChange() {
        (OnBLEConnectionStateChangeList || []).forEach((v) => {
            tj.offBLEConnectionStateChange(v);
        });
    },
    TJ_OnBLEMTUChange() {
        if (!OnBLEMTUChangeList) {
            OnBLEMTUChangeList = [];
        }
        const callback = (res) => {
            formatResponse('OnBLEMTUChangeListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnBLEMTUChangeCallback', resStr);
        };
        OnBLEMTUChangeList.push(callback);
        tj.onBLEMTUChange(callback);
    },
    TJ_OffBLEMTUChange() {
        (OnBLEMTUChangeList || []).forEach((v) => {
            tj.offBLEMTUChange(v);
        });
    },
    TJ_OnBLEPeripheralConnectionStateChanged() {
        if (!OnBLEPeripheralConnectionStateChangedList) {
            OnBLEPeripheralConnectionStateChangedList = [];
        }
        const callback = (res) => {
            formatResponse('OnBLEPeripheralConnectionStateChangedListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnBLEPeripheralConnectionStateChangedCallback', resStr);
        };
        OnBLEPeripheralConnectionStateChangedList.push(callback);
        tj.onBLEPeripheralConnectionStateChanged(callback);
    },
    TJ_OffBLEPeripheralConnectionStateChanged() {
        (OnBLEPeripheralConnectionStateChangedList || []).forEach((v) => {
            tj.offBLEPeripheralConnectionStateChanged(v);
        });
    },
    TJ_OnBackgroundFetchData() {
        const callback = (res) => {
            formatResponse('OnBackgroundFetchDataListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnBackgroundFetchDataCallback', resStr);
        };
        tj.onBackgroundFetchData(callback);
    },
    TJ_OnBeaconServiceChange() {
        if (!OnBeaconServiceChangeList) {
            OnBeaconServiceChangeList = [];
        }
        const callback = (res) => {
            formatResponse('OnBeaconServiceChangeListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnBeaconServiceChangeCallback', resStr);
        };
        OnBeaconServiceChangeList.push(callback);
        tj.onBeaconServiceChange(callback);
    },
    TJ_OffBeaconServiceChange() {
        (OnBeaconServiceChangeList || []).forEach((v) => {
            tj.offBeaconServiceChange(v);
        });
    },
    TJ_OnBeaconUpdate() {
        if (!OnBeaconUpdateList) {
            OnBeaconUpdateList = [];
        }
        const callback = (res) => {
            formatResponse('OnBeaconUpdateListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnBeaconUpdateCallback', resStr);
        };
        OnBeaconUpdateList.push(callback);
        tj.onBeaconUpdate(callback);
    },
    TJ_OffBeaconUpdate() {
        (OnBeaconUpdateList || []).forEach((v) => {
            tj.offBeaconUpdate(v);
        });
    },
    TJ_OnBluetoothAdapterStateChange() {
        if (!OnBluetoothAdapterStateChangeList) {
            OnBluetoothAdapterStateChangeList = [];
        }
        const callback = (res) => {
            formatResponse('OnBluetoothAdapterStateChangeListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnBluetoothAdapterStateChangeCallback', resStr);
        };
        OnBluetoothAdapterStateChangeList.push(callback);
        tj.onBluetoothAdapterStateChange(callback);
    },
    TJ_OffBluetoothAdapterStateChange() {
        (OnBluetoothAdapterStateChangeList || []).forEach((v) => {
            tj.offBluetoothAdapterStateChange(v);
        });
    },
    TJ_OnBluetoothDeviceFound() {
        if (!OnBluetoothDeviceFoundList) {
            OnBluetoothDeviceFoundList = [];
        }
        const callback = (res) => {
            formatResponse('OnBluetoothDeviceFoundListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnBluetoothDeviceFoundCallback', resStr);
        };
        OnBluetoothDeviceFoundList.push(callback);
        tj.onBluetoothDeviceFound(callback);
    },
    TJ_OffBluetoothDeviceFound() {
        (OnBluetoothDeviceFoundList || []).forEach((v) => {
            tj.offBluetoothDeviceFound(v);
        });
    },
    TJ_OnCompassChange() {
        if (!OnCompassChangeList) {
            OnCompassChangeList = [];
        }
        const callback = (res) => {
            formatResponse('OnCompassChangeListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnCompassChangeCallback', resStr);
        };
        OnCompassChangeList.push(callback);
        tj.onCompassChange(callback);
    },
    TJ_OffCompassChange() {
        (OnCompassChangeList || []).forEach((v) => {
            tj.offCompassChange(v);
        });
    },
    TJ_OnDeviceMotionChange() {
        if (!OnDeviceMotionChangeList) {
            OnDeviceMotionChangeList = [];
        }
        const callback = (res) => {
            formatResponse('OnDeviceMotionChangeListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnDeviceMotionChangeCallback', resStr);
        };
        OnDeviceMotionChangeList.push(callback);
        tj.onDeviceMotionChange(callback);
    },
    TJ_OffDeviceMotionChange() {
        (OnDeviceMotionChangeList || []).forEach((v) => {
            tj.offDeviceMotionChange(v);
        });
    },
    TJ_OnDeviceOrientationChange() {
        if (!OnDeviceOrientationChangeList) {
            OnDeviceOrientationChangeList = [];
        }
        const callback = (res) => {
            formatResponse('OnDeviceOrientationChangeListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnDeviceOrientationChangeCallback', resStr);
        };
        OnDeviceOrientationChangeList.push(callback);
        tj.onDeviceOrientationChange(callback);
    },
    TJ_OffDeviceOrientationChange() {
        (OnDeviceOrientationChangeList || []).forEach((v) => {
            tj.offDeviceOrientationChange(v);
        });
    },
    TJ_OnError() {
        if (!OnErrorList) {
            OnErrorList = [];
        }
        const callback = (res) => {
            formatResponse('Error', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnErrorCallback', resStr);
        };
        OnErrorList.push(callback);
        tj.onError(callback);
    },
    TJ_OffError() {
        (OnErrorList || []).forEach((v) => {
            tj.offError(v);
        });
    },
    TJ_OnHide() {
        if (!OnHideList) {
            OnHideList = [];
        }
        const callback = (res) => {
            formatResponse('GeneralCallbackResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnHideCallback', resStr);
        };
        OnHideList.push(callback);
        tj.onHide(callback);
    },
    TJ_OffHide() {
        (OnHideList || []).forEach((v) => {
            tj.offHide(v);
        });
    },
    TJ_OnInteractiveStorageModified() {
        if (!OnInteractiveStorageModifiedList) {
            OnInteractiveStorageModifiedList = [];
        }
        const callback = (res) => {
            const resStr = res;
            moduleHelper.send('_OnInteractiveStorageModifiedCallback', resStr);
        };
        OnInteractiveStorageModifiedList.push(callback);
        tj.onInteractiveStorageModified(callback);
    },
    TJ_OffInteractiveStorageModified() {
        (OnInteractiveStorageModifiedList || []).forEach((v) => {
            tj.offInteractiveStorageModified(v);
        });
    },
    TJ_OnKeyDown() {
        if (!OnKeyDownList) {
            OnKeyDownList = [];
        }
        const callback = (res) => {
            formatResponse('OnKeyDownListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnKeyDownCallback', resStr);
        };
        OnKeyDownList.push(callback);
        tj.onKeyDown(callback);
    },
    TJ_OffKeyDown() {
        (OnKeyDownList || []).forEach((v) => {
            tj.offKeyDown(v);
        });
    },
    TJ_OnKeyUp() {
        if (!OnKeyUpList) {
            OnKeyUpList = [];
        }
        const callback = (res) => {
            formatResponse('OnKeyDownListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnKeyUpCallback', resStr);
        };
        OnKeyUpList.push(callback);
        tj.onKeyUp(callback);
    },
    TJ_OffKeyUp() {
        (OnKeyUpList || []).forEach((v) => {
            tj.offKeyUp(v);
        });
    },
    TJ_OnKeyboardComplete() {
        if (!OnKeyboardCompleteList) {
            OnKeyboardCompleteList = [];
        }
        const callback = (res) => {
            formatResponse('OnKeyboardInputListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnKeyboardCompleteCallback', resStr);
        };
        OnKeyboardCompleteList.push(callback);
        tj.onKeyboardComplete(callback);
    },
    TJ_OffKeyboardComplete() {
        (OnKeyboardCompleteList || []).forEach((v) => {
            tj.offKeyboardComplete(v);
        });
    },
    TJ_OnKeyboardConfirm() {
        if (!OnKeyboardConfirmList) {
            OnKeyboardConfirmList = [];
        }
        const callback = (res) => {
            formatResponse('OnKeyboardInputListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnKeyboardConfirmCallback', resStr);
        };
        OnKeyboardConfirmList.push(callback);
        tj.onKeyboardConfirm(callback);
    },
    TJ_OffKeyboardConfirm() {
        (OnKeyboardConfirmList || []).forEach((v) => {
            tj.offKeyboardConfirm(v);
        });
    },
    TJ_OnKeyboardHeightChange() {
        if (!OnKeyboardHeightChangeList) {
            OnKeyboardHeightChangeList = [];
        }
        const callback = (res) => {
            formatResponse('OnKeyboardHeightChangeListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnKeyboardHeightChangeCallback', resStr);
        };
        OnKeyboardHeightChangeList.push(callback);
        tj.onKeyboardHeightChange(callback);
    },
    TJ_OffKeyboardHeightChange() {
        (OnKeyboardHeightChangeList || []).forEach((v) => {
            tj.offKeyboardHeightChange(v);
        });
    },
    TJ_OnKeyboardInput() {
        if (!OnKeyboardInputList) {
            OnKeyboardInputList = [];
        }
        const callback = (res) => {
            formatResponse('OnKeyboardInputListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnKeyboardInputCallback', resStr);
        };
        OnKeyboardInputList.push(callback);
        tj.onKeyboardInput(callback);
    },
    TJ_OffKeyboardInput() {
        (OnKeyboardInputList || []).forEach((v) => {
            tj.offKeyboardInput(v);
        });
    },
    TJ_OnMemoryWarning() {
        if (!OnMemoryWarningList) {
            OnMemoryWarningList = [];
        }
        const callback = (res) => {
            formatResponse('OnMemoryWarningListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnMemoryWarningCallback', resStr);
        };
        OnMemoryWarningList.push(callback);
        tj.onMemoryWarning(callback);
    },
    TJ_OffMemoryWarning() {
        (OnMemoryWarningList || []).forEach((v) => {
            tj.offMemoryWarning(v);
        });
    },
    TJ_OnMessage() {
        const callback = (res) => {
            const resStr = res;
            moduleHelper.send('_OnMessageCallback', resStr);
        };
        tj.onMessage(callback);
    },
    TJ_OnMouseDown() {
        if (!OnMouseDownList) {
            OnMouseDownList = [];
        }
        const callback = (res) => {
            formatResponse('OnMouseDownListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnMouseDownCallback', resStr);
        };
        OnMouseDownList.push(callback);
        tj.onMouseDown(callback);
    },
    TJ_OffMouseDown() {
        (OnMouseDownList || []).forEach((v) => {
            tj.offMouseDown(v);
        });
    },
    TJ_OnMouseMove() {
        if (!OnMouseMoveList) {
            OnMouseMoveList = [];
        }
        const callback = (res) => {
            formatResponse('OnMouseMoveListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnMouseMoveCallback', resStr);
        };
        OnMouseMoveList.push(callback);
        tj.onMouseMove(callback);
    },
    TJ_OffMouseMove() {
        (OnMouseMoveList || []).forEach((v) => {
            tj.offMouseMove(v);
        });
    },
    TJ_OnMouseUp() {
        if (!OnMouseUpList) {
            OnMouseUpList = [];
        }
        const callback = (res) => {
            formatResponse('OnMouseDownListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnMouseUpCallback', resStr);
        };
        OnMouseUpList.push(callback);
        tj.onMouseUp(callback);
    },
    TJ_OffMouseUp() {
        (OnMouseUpList || []).forEach((v) => {
            tj.offMouseUp(v);
        });
    },
    TJ_OnNetworkStatusChange() {
        if (!OnNetworkStatusChangeList) {
            OnNetworkStatusChangeList = [];
        }
        const callback = (res) => {
            formatResponse('OnNetworkStatusChangeListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnNetworkStatusChangeCallback', resStr);
        };
        OnNetworkStatusChangeList.push(callback);
        tj.onNetworkStatusChange(callback);
    },
    TJ_OffNetworkStatusChange() {
        (OnNetworkStatusChangeList || []).forEach((v) => {
            tj.offNetworkStatusChange(v);
        });
    },
    TJ_OnNetworkWeakChange() {
        if (!OnNetworkWeakChangeList) {
            OnNetworkWeakChangeList = [];
        }
        const callback = (res) => {
            formatResponse('OnNetworkWeakChangeListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnNetworkWeakChangeCallback', resStr);
        };
        OnNetworkWeakChangeList.push(callback);
        tj.onNetworkWeakChange(callback);
    },
    TJ_OffNetworkWeakChange() {
        (OnNetworkWeakChangeList || []).forEach((v) => {
            tj.offNetworkWeakChange(v);
        });
    },
    TJ_OnScreenRecordingStateChanged() {
        if (!OnScreenRecordingStateChangedList) {
            OnScreenRecordingStateChangedList = [];
        }
        const callback = (res) => {
            formatResponse('OnScreenRecordingStateChangedListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnScreenRecordingStateChangedCallback', resStr);
        };
        OnScreenRecordingStateChangedList.push(callback);
        tj.onScreenRecordingStateChanged(callback);
    },
    TJ_OffScreenRecordingStateChanged() {
        (OnScreenRecordingStateChangedList || []).forEach((v) => {
            tj.offScreenRecordingStateChanged(v);
        });
    },
    TJ_OnShareMessageToFriend() {
        const callback = (res) => {
            formatResponse('OnShareMessageToFriendListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnShareMessageToFriendCallback', resStr);
        };
        tj.onShareMessageToFriend(callback);
    },
    TJ_OnShow() {
        if (!OnShowList) {
            OnShowList = [];
        }
        const callback = (res) => {
            formatResponse('OnShowListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnShowCallback', resStr);
        };
        OnShowList.push(callback);
        tj.onShow(callback);
    },
    TJ_OffShow() {
        (OnShowList || []).forEach((v) => {
            tj.offShow(v);
        });
    },
    TJ_OnUnhandledRejection() {
        if (!OnUnhandledRejectionList) {
            OnUnhandledRejectionList = [];
        }
        const callback = (res) => {
            formatResponse('OnUnhandledRejectionListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnUnhandledRejectionCallback', resStr);
        };
        OnUnhandledRejectionList.push(callback);
        tj.onUnhandledRejection(callback);
    },
    TJ_OffUnhandledRejection() {
        (OnUnhandledRejectionList || []).forEach((v) => {
            tj.offUnhandledRejection(v);
        });
    },
    TJ_OnUserCaptureScreen() {
        if (!OnUserCaptureScreenList) {
            OnUserCaptureScreenList = [];
        }
        const callback = (res) => {
            formatResponse('GeneralCallbackResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnUserCaptureScreenCallback', resStr);
        };
        OnUserCaptureScreenList.push(callback);
        tj.onUserCaptureScreen(callback);
    },
    TJ_OffUserCaptureScreen() {
        (OnUserCaptureScreenList || []).forEach((v) => {
            tj.offUserCaptureScreen(v);
        });
    },
    TJ_OnVoIPChatInterrupted() {
        if (!OnVoIPChatInterruptedList) {
            OnVoIPChatInterruptedList = [];
        }
        const callback = (res) => {
            formatResponse('OnVoIPChatInterruptedListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnVoIPChatInterruptedCallback', resStr);
        };
        OnVoIPChatInterruptedList.push(callback);
        tj.onVoIPChatInterrupted(callback);
    },
    TJ_OffVoIPChatInterrupted() {
        (OnVoIPChatInterruptedList || []).forEach((v) => {
            tj.offVoIPChatInterrupted(v);
        });
    },
    TJ_OnVoIPChatMembersChanged() {
        if (!OnVoIPChatMembersChangedList) {
            OnVoIPChatMembersChangedList = [];
        }
        const callback = (res) => {
            formatResponse('OnVoIPChatMembersChangedListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnVoIPChatMembersChangedCallback', resStr);
        };
        OnVoIPChatMembersChangedList.push(callback);
        tj.onVoIPChatMembersChanged(callback);
    },
    TJ_OffVoIPChatMembersChanged() {
        (OnVoIPChatMembersChangedList || []).forEach((v) => {
            tj.offVoIPChatMembersChanged(v);
        });
    },
    TJ_OnVoIPChatSpeakersChanged() {
        if (!OnVoIPChatSpeakersChangedList) {
            OnVoIPChatSpeakersChangedList = [];
        }
        const callback = (res) => {
            formatResponse('OnVoIPChatSpeakersChangedListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnVoIPChatSpeakersChangedCallback', resStr);
        };
        OnVoIPChatSpeakersChangedList.push(callback);
        tj.onVoIPChatSpeakersChanged(callback);
    },
    TJ_OffVoIPChatSpeakersChanged() {
        (OnVoIPChatSpeakersChangedList || []).forEach((v) => {
            tj.offVoIPChatSpeakersChanged(v);
        });
    },
    TJ_OnVoIPChatStateChanged() {
        if (!OnVoIPChatStateChangedList) {
            OnVoIPChatStateChangedList = [];
        }
        const callback = (res) => {
            formatResponse('OnVoIPChatStateChangedListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnVoIPChatStateChangedCallback', resStr);
        };
        OnVoIPChatStateChangedList.push(callback);
        tj.onVoIPChatStateChanged(callback);
    },
    TJ_OffVoIPChatStateChanged() {
        (OnVoIPChatStateChangedList || []).forEach((v) => {
            tj.offVoIPChatStateChanged(v);
        });
    },
    TJ_OnWheel() {
        if (!OnWheelList) {
            OnWheelList = [];
        }
        const callback = (res) => {
            formatResponse('OnWheelListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnWheelCallback', resStr);
        };
        OnWheelList.push(callback);
        tj.onWheel(callback);
    },
    TJ_OffWheel() {
        (OnWheelList || []).forEach((v) => {
            tj.offWheel(v);
        });
    },
    TJ_OnWindowResize() {
        if (!OnWindowResizeList) {
            OnWindowResizeList = [];
        }
        const callback = (res) => {
            formatResponse('OnWindowResizeListenerResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnWindowResizeCallback', resStr);
        };
        OnWindowResizeList.push(callback);
        tj.onWindowResize(callback);
    },
    TJ_OffWindowResize() {
        (OnWindowResizeList || []).forEach((v) => {
            tj.offWindowResize(v);
        });
    },
    TJ_OnAddToFavorites() {
        const callback = (res) => {
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnAddToFavoritesCallback', resStr);
            return tjOnAddToFavoritesResolveConf;
        };
        tj.onAddToFavorites(callback);
    },
    TJ_OnAddToFavorites_Resolve(conf) {
        try {
            tjOnAddToFavoritesResolveConf = formatJsonStr(conf);
            return;
        }
        catch (e) {
        }
        tjOnAddToFavoritesResolveConf = {};
    },
    TJ_OffAddToFavorites() {
        tj.offAddToFavorites();
    },
    TJ_OnCopyUrl() {
        const callback = (res) => {
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnCopyUrlCallback', resStr);
            return tjOnCopyUrlResolveConf;
        };
        tj.onCopyUrl(callback);
    },
    TJ_OnCopyUrl_Resolve(conf) {
        try {
            tjOnCopyUrlResolveConf = formatJsonStr(conf);
            return;
        }
        catch (e) {
        }
        tjOnCopyUrlResolveConf = {};
    },
    TJ_OffCopyUrl() {
        tj.offCopyUrl();
    },
    TJ_OnHandoff() {
        const callback = (res) => {
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnHandoffCallback', resStr);
            return tjOnHandoffResolveConf;
        };
        tj.onHandoff(callback);
    },
    TJ_OnHandoff_Resolve(conf) {
        try {
            tjOnHandoffResolveConf = formatJsonStr(conf);
            return;
        }
        catch (e) {
        }
        tjOnHandoffResolveConf = {};
    },
    TJ_OffHandoff() {
        tj.offHandoff();
    },
    TJ_OnShareTimeline() {
        const callback = (res) => {
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnShareTimelineCallback', resStr);
            return tjOnShareTimelineResolveConf;
        };
        tj.onShareTimeline(callback);
    },
    TJ_OnShareTimeline_Resolve(conf) {
        try {
            tjOnShareTimelineResolveConf = formatJsonStr(conf);
            return;
        }
        catch (e) {
        }
        tjOnShareTimelineResolveConf = {};
    },
    TJ_OffShareTimeline() {
        tj.offShareTimeline();
    },
    TJ_OnGameLiveStateChange() {
        const callback = (res) => {
            formatResponse('OnGameLiveStateChangeCallbackResult', res);
            const resStr = stringifyRes(res);
            moduleHelper.send('_OnGameLiveStateChangeCallback', resStr);
            return tjOnGameLiveStateChangeResolveConf;
        };
        tj.onGameLiveStateChange(callback);
    },
    TJ_OnGameLiveStateChange_Resolve(conf) {
        try {
            tjOnGameLiveStateChangeResolveConf = formatJsonStr(conf);
            return;
        }
        catch (e) {
        }
        tjOnGameLiveStateChangeResolveConf = {};
    },
    TJ_OffGameLiveStateChange() {
        tj.offGameLiveStateChange();
    },
    TJ_SetHandoffQuery(query) {
        const res = tj.setHandoffQuery(formatJsonStr(query));
        return res;
    },
    TJ_GetAccountInfoSync() {
        const res = tj.getAccountInfoSync();
        formatResponse('AccountInfo', res);
        return JSON.stringify(res);
    },
    TJ_GetAppAuthorizeSetting() {
        const res = tj.getAppAuthorizeSetting();
        formatResponse('AppAuthorizeSetting', JSON.parse(JSON.stringify(res)));
        return JSON.stringify(res);
    },
    TJ_GetAppBaseInfo() {
        const res = tj.getAppBaseInfo();
        formatResponse('AppBaseInfo', res);
        return JSON.stringify(res);
    },
    TJ_GetBatteryInfoSync() {
        const res = tj.getBatteryInfoSync();
        formatResponse('GetBatteryInfoSyncResult', res);
        return JSON.stringify(res);
    },
    TJ_GetDeviceInfo() {
        const res = tj.getDeviceInfo();
        formatResponse('DeviceInfo', res);
        return JSON.stringify(res);
    },
    TJ_GetEnterOptionsSync() {
        const res = tj.getEnterOptionsSync();
        formatResponse('EnterOptionsGame', res);
        return JSON.stringify(res);
    },
    TJ_GetExptInfoSync(keys) {
        const res = tj.getExptInfoSync(formatJsonStr(keys));
        formatResponse('IAnyObject', res);
        return JSON.stringify(res);
    },
    TJ_GetExtConfigSync() {
        const res = tj.getExtConfigSync();
        formatResponse('IAnyObject', res);
        return JSON.stringify(res);
    },
    TJ_GetLaunchOptionsSync() {
        const res = tj.getLaunchOptionsSync();
        formatResponse('LaunchOptionsGame', res);
        return JSON.stringify(res);
    },
    TJ_GetMenuButtonBoundingClientRect() {
        const res = tj.getMenuButtonBoundingClientRect();
        formatResponse('ClientRect', res);
        return JSON.stringify(res);
    },
    TJ_GetStorageInfoSync() {
        const res = tj.getStorageInfoSync();
        formatResponse('GetStorageInfoSyncOption', res);
        return JSON.stringify(res);
    },
    TJ_GetSystemInfoSync() {
        const res = tj.getSystemInfoSync();
        formatResponse('SystemInfo', res);
        return JSON.stringify(res);
    },
    TJ_GetSystemSetting() {
        const res = tj.getSystemSetting();
        formatResponse('SystemSetting', JSON.parse(JSON.stringify(res)));
        return JSON.stringify(res);
    },
    TJ_GetWindowInfo() {
        const res = tj.getWindowInfo();
        formatResponse('WindowInfo', res);
        return JSON.stringify(res);
    },
    TJ_CreateImageData() {
        const res = tj.createImageData();
        formatResponse('ImageData', res);
        return JSON.stringify(res);
    },
    TJ_CreatePath2D() {
        const res = tj.createPath2D();
        formatResponse('Path2D', res);
        return JSON.stringify(res);
    },
    TJ_IsPointerLocked() {
        const res = tj.isPointerLocked();
        return res;
    },
    TJ_IsVKSupport(version) {
        const res = tj.isVKSupport(formatJsonStr(version));
        return res;
    },
    TJ_SetCursor(path, x, y) {
        const res = tj.setCursor(formatJsonStr(path), x, y);
        return res;
    },
    TJ_SetMessageToFriendQuery(option) {
        const res = tj.setMessageToFriendQuery(formatJsonStr(option));
        return res;
    },
    TJ_GetTextLineHeight(option) {
        const res = tj.getTextLineHeight(formatJsonStr(option));
        return res;
    },
    TJ_LoadFont(path) {
        const res = tj.loadFont(formatJsonStr(path));
        return res;
    },
    TJ_GetGameLiveState() {
        const res = tj.getGameLiveState();
        formatResponse('GameLiveState', res);
        return JSON.stringify(res);
    },
    TJ_DownloadFile(conf) {
        const config = formatJsonStr(conf);
        const callbackId = uid();
        const obj = tj.downloadFile({
            ...config,
            success(res) {
                formatResponse('DownloadFileSuccessCallbackResult', res);
                moduleHelper.send('DownloadFileCallback', JSON.stringify({
                    callbackId, type: 'success', res: JSON.stringify(res),
                }));
            },
            fail(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('DownloadFileCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res),
                }));
            },
            complete(res) {
                formatResponse('GeneralCallbackResult', res);
                moduleHelper.send('DownloadFileCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res),
                }));
            },
        });
        DownloadTaskList[callbackId] = obj;
        return callbackId;
    },
    TJ_CreateFeedbackButton(option) {
        const obj = tj.createFeedbackButton(formatJsonStr(option));
        const key = uid();
        FeedbackButtonList[key] = obj;
        return key;
    },
    TJ_GetLogManager(option) {
        const obj = tj.getLogManager(formatJsonStr(option));
        const key = uid();
        LogManagerList[key] = obj;
        return key;
    },
    TJ_GetRealtimeLogManager() {
        const obj = tj.getRealtimeLogManager();
        const key = uid();
        RealtimeLogManagerList[key] = obj;
        return key;
    },
    TJ_GetUpdateManager() {
        const obj = tj.getUpdateManager();
        const key = uid();
        UpdateManagerList[key] = obj;
        return key;
    },
    TJ_CreateVideoDecoder() {
        const obj = tj.createVideoDecoder();
        const key = uid();
        VideoDecoderList[key] = obj;
        return key;
    },
    TJ_DownloadTaskAbort(id) {
        const obj = getDownloadTaskObject(id);
        if (!obj) {
            return;
        }
        obj.abort();
    },
    TJ_DownloadTaskOffHeadersReceived(id) {
        const obj = getDownloadTaskObject(id);
        if (!obj) {
            return;
        }
        offEventCallback(tjDownloadTaskHeadersReceivedList, (v) => {
            obj.offHeadersReceived(v);
        }, id);
    },
    TJ_DownloadTaskOffProgressUpdate(id) {
        const obj = getDownloadTaskObject(id);
        if (!obj) {
            return;
        }
        offEventCallback(tjDownloadTaskProgressUpdateList, (v) => {
            obj.offProgressUpdate(v);
        }, id);
    },
    TJ_DownloadTaskOnHeadersReceived(id) {
        const obj = getDownloadTaskObject(id);
        if (!obj) {
            return;
        }
        const callback = onEventCallback(tjDownloadTaskHeadersReceivedList, '_DownloadTaskOnHeadersReceivedCallback', id, id);
        obj.onHeadersReceived(callback);
    },
    TJ_DownloadTaskOnProgressUpdate(id) {
        const obj = getDownloadTaskObject(id);
        if (!obj) {
            return;
        }
        const callback = onEventCallback(tjDownloadTaskProgressUpdateList, '_DownloadTaskOnProgressUpdateCallback', id, id);
        obj.onProgressUpdate(callback);
    },
    TJFeedbackButtonSetProperty(id, key, value) {
        const obj = getFeedbackButtonObject(id);
        if (!obj) {
            return;
        }
        if (/^\s*(\{.*\}|\[.*\])\s*$/.test(value)) {
            try {
                const jsonValue = JSON.parse(value);
                Object.assign(obj[key], jsonValue);
            }
            catch (e) {
                obj[key] = value;
            }
        }
        else {
            obj[key] = value;
        }
    },
    TJ_FeedbackButtonDestroy(id) {
        const obj = getFeedbackButtonObject(id);
        if (!obj) {
            return;
        }
        obj.destroy();
    },
    TJ_FeedbackButtonHide(id) {
        const obj = getFeedbackButtonObject(id);
        if (!obj) {
            return;
        }
        obj.hide();
    },
    TJ_FeedbackButtonOffTap(id) {
        const obj = getFeedbackButtonObject(id);
        if (!obj) {
            return;
        }
        offEventCallback(tjFeedbackButtonTapList, (v) => {
            obj.offTap(v);
        }, id);
    },
    TJ_FeedbackButtonOnTap(id) {
        const obj = getFeedbackButtonObject(id);
        if (!obj) {
            return;
        }
        const callback = onEventCallback(tjFeedbackButtonTapList, '_FeedbackButtonOnTapCallback', id, id);
        obj.onTap(callback);
    },
    TJ_FeedbackButtonShow(id) {
        const obj = getFeedbackButtonObject(id);
        if (!obj) {
            return;
        }
        obj.show();
    },
    TJ_LogManagerDebug(id, args) {
        const obj = getLogManagerObject(id);
        if (!obj) {
            return;
        }
        obj.debug(args);
    },
    TJ_LogManagerInfo(id, args) {
        const obj = getLogManagerObject(id);
        if (!obj) {
            return;
        }
        obj.info(args);
    },
    TJ_LogManagerLog(id, args) {
        const obj = getLogManagerObject(id);
        if (!obj) {
            return;
        }
        obj.log(args);
    },
    TJ_LogManagerWarn(id, args) {
        const obj = getLogManagerObject(id);
        if (!obj) {
            return;
        }
        obj.warn(args);
    },
    TJ_RealtimeLogManagerAddFilterMsg(id, msg) {
        const obj = getRealtimeLogManagerObject(id);
        if (!obj) {
            return;
        }
        obj.addFilterMsg(msg);
    },
    TJ_RealtimeLogManagerError(id, args) {
        const obj = getRealtimeLogManagerObject(id);
        if (!obj) {
            return;
        }
        obj.error(args);
    },
    TJ_RealtimeLogManagerInfo(id, args) {
        const obj = getRealtimeLogManagerObject(id);
        if (!obj) {
            return;
        }
        obj.info(args);
    },
    TJ_RealtimeLogManagerSetFilterMsg(id, msg) {
        const obj = getRealtimeLogManagerObject(id);
        if (!obj) {
            return;
        }
        obj.setFilterMsg(msg);
    },
    TJ_RealtimeLogManagerWarn(id, args) {
        const obj = getRealtimeLogManagerObject(id);
        if (!obj) {
            return;
        }
        obj.warn(args);
    },
    TJ_UpdateManagerApplyUpdate(id) {
        const obj = getUpdateManagerObject(id);
        if (!obj) {
            return;
        }
        obj.applyUpdate();
    },
    TJ_UpdateManagerOnCheckForUpdate(id) {
        const obj = getUpdateManagerObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            formatResponse('OnCheckForUpdateListenerResult', res);
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify(res),
            });
            moduleHelper.send('_UpdateManagerOnCheckForUpdateCallback', resStr);
        };
        obj.onCheckForUpdate(callback);
    },
    TJ_UpdateManagerOnUpdateFailed(id) {
        const obj = getUpdateManagerObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            formatResponse('GeneralCallbackResult', res);
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify(res),
            });
            moduleHelper.send('_UpdateManagerOnUpdateFailedCallback', resStr);
        };
        obj.onUpdateFailed(callback);
    },
    TJ_UpdateManagerOnUpdateReady(id) {
        const obj = getUpdateManagerObject(id);
        if (!obj) {
            return;
        }
        const callback = (res) => {
            formatResponse('GeneralCallbackResult', res);
            const resStr = JSON.stringify({
                callbackId: id,
                res: JSON.stringify(res),
            });
            moduleHelper.send('_UpdateManagerOnUpdateReadyCallback', resStr);
        };
        obj.onUpdateReady(callback);
    },
    TJ_VideoDecoderGetFrameData(id) {
        const obj = getVideoDecoderObject(id);
        if (!obj) {
            return JSON.stringify(formatResponse('FrameDataOptions'));
        }
        return JSON.stringify(formatResponse('FrameDataOptions', obj.getFrameData(), id));
    },
    TJ_VideoDecoderRemove(id) {
        const obj = getVideoDecoderObject(id);
        if (!obj) {
            return;
        }
        obj.remove();
    },
    TJ_VideoDecoderSeek(id, position) {
        const obj = getVideoDecoderObject(id);
        if (!obj) {
            return;
        }
        obj.seek(position);
    },
    TJ_VideoDecoderStart(id, option) {
        const obj = getVideoDecoderObject(id);
        if (!obj) {
            return;
        }
        obj.start(formatJsonStr(option));
    },
    TJ_VideoDecoderStop(id) {
        const obj = getVideoDecoderObject(id);
        if (!obj) {
            return;
        }
        obj.stop();
    },
    TJ_VideoDecoderOff(id, eventName) {
        const obj = getVideoDecoderObject(id);
        if (!obj) {
            return;
        }
        offEventCallback(tjVideoDecoderList, (v) => {
            obj.off(eventName, v);
        }, id);
    },
    TJ_VideoDecoderOn(id, eventName) {
        const obj = getVideoDecoderObject(id);
        if (!obj) {
            return;
        }
        const callback = onEventCallback(tjVideoDecoderList, '_VideoDecoderOnCallback', id, id + eventName);
        obj.on(eventName, callback);
    },
};
