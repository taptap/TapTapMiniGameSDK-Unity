import moduleHelper from './module-helper';
import { formatJsonStr } from './utils';
let shareResolve;
export default {
    TJShareAppMessage(conf) {
        tj.shareAppMessage({
            ...formatJsonStr(conf),
        });
    },
    TJOnShareAppMessage(conf, isPromise) {
        tj.onShareAppMessage(() => ({
            ...formatJsonStr(conf),
            promise: isPromise
                ? new Promise((resolve) => {
                    shareResolve = resolve;
                    moduleHelper.send('OnShareAppMessageCallback');
                })
                : null,
        }));
    },
    TJOnShareAppMessageResolve(conf) {
        if (shareResolve) {
            shareResolve(formatJsonStr(conf));
        }
    },
};
tj.showShareMenu({
    menus: ['shareAppMessage', 'shareTimeline'],
});
