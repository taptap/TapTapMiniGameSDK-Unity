import moduleHelper from './module-helper';
import { formatJsonStr, getListObject, uid } from './utils';
const gameRecorderList = {};
let tjGameRecorderList;
const getObject = getListObject(gameRecorderList, 'gameRecorder');
export default {
    TJ_GetGameRecorder() {
        const id = uid();
        gameRecorderList[id] = tj.getGameRecorder();
        return id;
    },
    TJ_GameRecorderOff(id, eventType) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        if (!obj || typeof tjGameRecorderList === 'undefined' || typeof tjGameRecorderList[eventType] === 'undefined') {
            return;
        }
        
        for (const key in Object.keys(tjGameRecorderList[eventType])) {
            const callback = tjGameRecorderList[eventType][key];
            if (callback) {
                obj.off(eventType, callback);
            }
        }
        tjGameRecorderList[eventType] = {};
    },
    TJ_GameRecorderOn(id, eventType) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        if (!tjGameRecorderList) {
            tjGameRecorderList = {
                start: {},
                stop: {},
                pause: {},
                resume: {},
                abort: {},
                timeUpdate: {},
                error: {},
            };
        }
        const callbackId = uid();
        const callback = (res) => {
            let result = '';
            if (res) {
                result = JSON.stringify(res);
            }
            const resStr = JSON.stringify({
                id,
                res: JSON.stringify({
                    eventType,
                    result,
                }),
            });
            moduleHelper.send('_OnGameRecorderCallback', resStr);
        };
        if (tjGameRecorderList[eventType]) {
            tjGameRecorderList[eventType][callbackId] = callback;
            obj.on(eventType, callback);
            return callbackId;
        }
        return '';
    },
    TJ_GameRecorderStart(id, option) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        const data = formatJsonStr(option);
        obj.start(data);
    },
    TJ_GameRecorderAbort(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.abort();
    },
    TJ_GameRecorderPause(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.pause();
    },
    TJ_GameRecorderResume(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.resume();
    },
    TJ_GameRecorderStop(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.stop();
    },
    TJ_OperateGameRecorderVideo(option) {
        if (typeof tj.operateGameRecorderVideo !== 'undefined') {
            const data = formatJsonStr(option);
            data.fail = (res) => {
                console.error(res);
            };
            tj.operateGameRecorderVideo(data);
        }
    },
};
