import response from './response';
import { formatJsonStr } from './utils';
const CloudIDObject = {};
function fixTJCallFunctionData(data) {
    
    for (const key in data) {
        if (typeof data[key] === 'object') {
            fixTJCallFunctionData(data[key]);
        }
        else if (typeof data[key] === 'string' && CloudIDObject[data[key]]) {
            data[key] = CloudIDObject[data[key]];
        }
    }
}
export default {
    TJCallFunctionInit(conf) {
        const config = formatJsonStr(conf);
        tj.cloud.init(config);
    },
    TJCallFunction(name, data, conf, s, f, c) {
        const d = JSON.parse(data);
        fixTJCallFunctionData(d);
        tj.cloud.callFunction({
            name,
            data: d,
            config: conf === '' ? null : JSON.parse(conf),
            ...response.handlecloudCallFunction(s, f, c),
        });
    },
    TJCloudID(cloudId) {
        
        const res = tj.cloud.CloudID(cloudId);
        const r = JSON.stringify(res);
        CloudIDObject[r] = res;
        return r;
    },
};
