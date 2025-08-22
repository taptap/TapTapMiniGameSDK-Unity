import moduleHelper from './module-helper';
import { getListObject, uid, formatJsonStr } from './utils';
const userInfoButtonList = {};
const getObject = getListObject(userInfoButtonList, 'userInfoButton');
export default {
    TJCreateUserInfoButton(option) {
        const config = formatJsonStr(option);
        const id = uid();
        const button = tj.createUserInfoButton({
            type: config.type,
            text: config.text,
            withCredentials: config.withCredentials || true,
            lang: config.lang || 'en',
            style: {
                left: config.style.left / window.devicePixelRatio,
                top: config.style.top / window.devicePixelRatio,
                width: config.style.width / window.devicePixelRatio,
                height: config.style.height / window.devicePixelRatio,
                backgroundColor: config.style.backgroundColor,
                color: config.style.color,
                textAlign: config.style.textAlign,
                fontSize: config.style.fontSize,
                borderRadius: config.style.borderRadius,
                borderColor: config.style.borderColor,
                borderWidth: config.style.borderWidth,
                lineHeight: config.style.lineHeight / window.devicePixelRatio,
            },
        });
        userInfoButtonList[id] = button;
        return id;
    },
    TJUserInfoButtonShow(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.show();
    },
    TJUserInfoButtonDestroy(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.destroy();
        if (userInfoButtonList) {
            delete userInfoButtonList[id];
        }
    },
    TJUserInfoButtonHide(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.hide();
    },
    TJUserInfoButtonOffTap(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        obj.offTap();
    },
    TJUserInfoButtonOnTap(id) {
        const obj = getObject(id);
        if (!obj) {
            return;
        }
        
        obj.onTap((res) => {
            res.userInfo = res.userInfo || {};
            moduleHelper.send('UserInfoButtonOnTapCallback', JSON.stringify({
                callbackId: id,
                errCode: res.err_code || (res.errMsg.indexOf('getUserInfo:fail') === 0 ? 1 : 0),
                errMsg: res.errMsg || '',
                signature: res.signature || '',
                encryptedData: res.encryptedData || '',
                iv: res.iv || '',
                cloudID: res.cloudID || '',
                userInfoRaw: JSON.stringify({
                    nickName: res.userInfo.nickName || '',
                    avatarUrl: res.userInfo.avatarUrl || '',
                    country: res.userInfo.country || '',
                    province: res.userInfo.province || '',
                    city: res.userInfo.city || '',
                    language: res.userInfo.language || '',
                    gender: res.userInfo.gender || 0,
                }),
            }));
        });
    },
};
