import moduleHelper from './module-helper';
import { formatJsonStr, getListObject, uid } from './utils';
const videoList = {};
const getObject = getListObject(videoList, 'video');
export default {
    TJCreateVideo(conf) {
        const id = uid();
        const params = formatJsonStr(conf);
        
        if (params.underGameView) {
            GameGlobal.enableTransparentCanvas = true;
        }
        videoList[id] = tj.createVideo(params);
        return id;
    },
    TJVideoSetProperty(id, key, value) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        if (key === 'x' || key === 'y' || key === 'width' || key === 'height') {
            obj[key] = +value;
        }
        else if (key === 'src' || key === 'poster') {
            obj[key] = value;
        }
    },
    TJVideoPlay(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.play();
    },
    TJVideoAddListener(id, key) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj[key]((e) => {
            moduleHelper.send('OnVideoCallback', JSON.stringify({
                callbackId: id,
                errMsg: key,
                position: e && e.position,
                buffered: e && e.buffered,
                duration: e && e.duration,
            }));
            if (key === 'onError') {
                GameGlobal.enableTransparentCanvas = false;
                console.error(e);
            }
        });
    },
    TJVideoDestroy(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.destroy();
        GameGlobal.enableTransparentCanvas = false;
    },
    TJVideoExitFullScreen(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.exitFullScreen();
    },
    TJVideoPause(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.pause();
    },
    TJVideoRequestFullScreen(id, direction) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.requestFullScreen(direction);
    },
    TJVideoSeek(id, time) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.seek(time);
    },
    TJVideoStop(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.stop();
    },
    TJVideoRemoveListener(id, key) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj[key]();
    },
};
