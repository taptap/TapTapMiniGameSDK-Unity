import response from './response';
import moduleHelper from './module-helper';
import { cacheArrayBuffer, formatJsonStr, formatResponse } from './utils';
import { fileInfoHandler, fileInfoType, responseWrapper } from './file-info';
function runMethod(method, option, callbackId, isString = false) {
    try {
        const fs = tj.getFileSystemManager();
        let config;
        if (typeof option === 'string') {
            config = formatJsonStr(option);
        }
        else {
            config = option;
        }
        if (method === 'readZipEntry' && !config.encoding) {
            config.encoding = 'utf-8';
            console.error('fs.readZipEntry不支持读取ArrayBuffer，已改为utf-8');
        }
        
        fs[method]({
            ...config,
            success(res) {
                let returnRes = '';
                if (method === 'read') {
                    cacheArrayBuffer(callbackId, res.arrayBuffer);
                    returnRes = JSON.stringify({
                        bytesRead: res.bytesRead,
                        arrayBufferLength: res.arrayBuffer?.byteLength ?? 0,
                    });
                }
                else if (method === 'readCompressedFile') {
                    cacheArrayBuffer(callbackId, res.data);
                    returnRes = JSON.stringify({
                        arrayBufferLength: res.data?.byteLength ?? 0,
                    });
                }
                else if (method === 'readFile') {
                    if (config.encoding) {
                        returnRes = JSON.stringify({
                            stringData: res.data || '',
                        });
                    }
                    else {
                        cacheArrayBuffer(callbackId, res.data);
                        returnRes = JSON.stringify({
                            arrayBufferLength: res.data?.byteLength ?? 0,
                        });
                    }
                }
                else {
                    returnRes = JSON.stringify(res);
                }
                // console.log(`fs.${method} success:`, res);
                moduleHelper.send('FileSystemManagerCallback', JSON.stringify({
                    callbackId, type: 'success', res: returnRes, method: isString ? `${method}_string` : method,
                }));
            },
            fail(res) {
                
                moduleHelper.send('FileSystemManagerCallback', JSON.stringify({
                    callbackId, type: 'fail', res: JSON.stringify(res), method: isString ? `${method}_string` : method,
                }));
            },
            complete(res) {
                moduleHelper.send('FileSystemManagerCallback', JSON.stringify({
                    callbackId, type: 'complete', res: JSON.stringify(res), method: isString ? `${method}_string` : method,
                }));
            },
        });
    }
    catch (e) {
        moduleHelper.send('FileSystemManagerCallback', JSON.stringify({
            callbackId, type: 'complete', res: 'fail', method: isString ? `${method}_string` : method,
        }));
    }
}
export default {
        TJGetUserDataPath() {
        return tj.env.USER_DATA_PATH;
    },
    TJWriteFileSync(filePath, data, encoding) {
        try {
            const fs = tj.getFileSystemManager();
            
            fs.writeFileSync(filePath, data, encoding);
            fileInfoHandler.addFileInfo(filePath, data);
        }
        catch (e) {
            console.error(e);
            if (e.message) {
                return e.message;
            }
            return 'fail';
        }
        return 'ok';
    },
    TJAccessFileSync(filePath) {
        try {
            const fs = tj.getFileSystemManager();
            fs.accessSync(filePath);
            return 'access:ok';
        }
        catch (e) {
            
            if (e.message) {
                return e.message;
            }
            return 'fail';
        }
    },
    TJAccessFile(path, s, f, c) {
        const fs = tj.getFileSystemManager();
        fs.access({
            path,
            ...response.handleText(s, f, c),
        });
    },
    TJCopyFileSync(src, dst) {
        try {
            const fs = tj.getFileSystemManager();
            fs.copyFileSync(src, dst);
            return 'copyFile:ok';
        }
        catch (e) {
            console.error(e);
            if (e.message) {
                return e.message;
            }
            return 'fail';
        }
    },
    TJCopyFile(srcPath, destPath, s, f, c) {
        const fs = tj.getFileSystemManager();
        fs.copyFile({
            srcPath,
            destPath,
            ...response.handleText(s, f, c),
        });
    },
    TJUnlinkSync(filePath) {
        try {
            const fs = tj.getFileSystemManager();
            fs.unlinkSync(filePath);
            fileInfoHandler.removeFileInfo(filePath);
            return 'unlink:ok';
        }
        catch (e) {
            console.error(e);
            if (e.message) {
                return e.message;
            }
            return 'fail';
        }
    },
    TJUnlink(filePath, s, f, c) {
        const fs = tj.getFileSystemManager();
        fs.unlink({
            filePath,
            ...responseWrapper(response.handleText(s, f, c), { filePath, type: fileInfoType.remove }),
        });
    },
    TJWriteFile(filePath, data, encoding, s, f, c) {
        const fs = tj.getFileSystemManager();
        fs.writeFile({
            filePath,
            data: data.buffer,
            encoding,
            ...responseWrapper(response.handleTextLongBack(s, f, c), { filePath, data: data.buffer, type: fileInfoType.add }),
        });
    },
    TJWriteStringFile(filePath, data, encoding, s, f, c) {
        const fs = tj.getFileSystemManager();
        fs.writeFile({
            filePath,
            data,
            encoding,
            ...responseWrapper(response.handleTextLongBack(s, f, c), { filePath, data, type: fileInfoType.add }),
        });
    },
    TJAppendFile(filePath, data, encoding, s, f, c) {
        const fs = tj.getFileSystemManager();
        fs.appendFile({
            filePath,
            data: data.buffer,
            encoding,
            ...response.handleTextLongBack(s, f, c),
        });
    },
    TJAppendStringFile(filePath, data, encoding, s, f, c) {
        const fs = tj.getFileSystemManager();
        fs.appendFile({
            filePath,
            data,
            encoding,
            ...response.handleTextLongBack(s, f, c),
        });
    },
    TJWriteBinFileSync(filePath, data, encoding) {
        const fs = tj.getFileSystemManager();
        try {
            fs.writeFileSync(filePath, data.buffer, encoding);
            fileInfoHandler.addFileInfo(filePath, data.buffer);
        }
        catch (e) {
            console.error(e);
            if (e.message) {
                return e.message;
            }
            return 'fail';
        }
        return 'ok';
    },
    TJReadFile(option, callbackId) {
        runMethod('readFile', option, callbackId);
    },
    TJReadFileSync(option) {
        const fs = tj.getFileSystemManager();
        const config = formatJsonStr(option);
        try {
            const { filePath } = config;
            const res = fs.readFileSync(config.filePath, config.encoding, config.position, config.length);
            if (!config.encoding && typeof res !== 'string') {
                cacheArrayBuffer(filePath, res);
                return `${res.byteLength}`;
            }
            return res;
        }
        catch (e) {
            console.error(e);
            if (e.message) {
                return e.message;
            }
            return 'fail';
        }
    },
    TJMkdir(dirPath, recursive, s, f, c) {
        const fs = tj.getFileSystemManager();
        fs.mkdir({
            dirPath,
            recursive: Boolean(recursive),
            ...response.handleText(s, f, c),
        });
    },
    TJMkdirSync(dirPath, recursive) {
        try {
            const fs = tj.getFileSystemManager();
            fs.mkdirSync(dirPath, Boolean(recursive));
            return 'mkdir:ok';
        }
        catch (e) {
            console.error(e);
            if (e.message) {
                return e.message;
            }
            return 'fail';
        }
    },
    TJRmdir(dirPath, recursive, s, f, c) {
        const fs = tj.getFileSystemManager();
        fs.rmdir({
            dirPath,
            recursive: Boolean(recursive),
            ...response.handleText(s, f, c),
        });
    },
    TJRmdirSync(dirPath, recursive) {
        try {
            const fs = tj.getFileSystemManager();
            fs.rmdirSync(dirPath, Boolean(recursive));
            return 'rmdirSync:ok';
        }
        catch (e) {
            console.error(e);
            if (e.message) {
                return e.message;
            }
            return 'fail';
        }
    },
    TJStat(conf, callbackId) {
        const config = formatJsonStr(conf);
        tj.getFileSystemManager().stat({
            ...config,
            success(res) {
                if (!Array.isArray(res.stats)) {
                    
                    res.one_stat = res.stats;
                    
                    res.stats = null;
                }
                moduleHelper.send('StatCallback', JSON.stringify({
                    callbackId,
                    type: 'success',
                    res: JSON.stringify(res),
                }));
            },
            fail(res) {
                moduleHelper.send('StatCallback', JSON.stringify({
                    callbackId,
                    type: 'fail',
                    res: JSON.stringify(res),
                }));
            },
            complete(res) {
                
                if (!Array.isArray(res.stats)) {
                    
                    res.one_stat = res.stats;
                    
                    res.stats = null;
                }
                moduleHelper.send('StatCallback', JSON.stringify({
                    callbackId,
                    type: 'complete',
                    res: JSON.stringify(res),
                }));
            },
        });
    },
    TJ_FileSystemManagerClose(option, callbackId) {
        runMethod('close', option, callbackId);
    },
    TJ_FileSystemManagerFstat(option, callbackId) {
        runMethod('fstat', option, callbackId);
    },
    TJ_FileSystemManagerFtruncate(option, callbackId) {
        runMethod('ftruncate', option, callbackId);
    },
    TJ_FileSystemManagerGetFileInfo(option, callbackId) {
        runMethod('getFileInfo', option, callbackId);
    },
    TJ_FileSystemManagerGetSavedFileList(option, callbackId) {
        runMethod('getSavedFileList', option, callbackId);
    },
    TJ_FileSystemManagerOpen(option, callbackId) {
        runMethod('open', option, callbackId);
    },
    TJ_FileSystemManagerRead(option, data, callbackId) {
        const config = formatJsonStr(option);
        config.arrayBuffer = data.buffer;
        runMethod('read', config, callbackId);
    },
    TJ_FileSystemManagerReadCompressedFile(option, callbackId) {
        runMethod('readCompressedFile', option, callbackId);
    },
    TJ_FileSystemManagerReadZipEntry(option, callbackId) {
        runMethod('readZipEntry', option, callbackId);
    },
    TJ_FileSystemManagerReadZipEntryString(option, callbackId) {
        runMethod('readZipEntry', option, callbackId, true);
    },
    TJ_FileSystemManagerReaddir(option, callbackId) {
        runMethod('readdir', option, callbackId);
    },
    TJ_FileSystemManagerRemoveSavedFile(option, callbackId) {
        runMethod('removeSavedFile', option, callbackId);
    },
    TJ_FileSystemManagerRename(option, callbackId) {
        runMethod('rename', option, callbackId);
    },
    TJ_FileSystemManagerSaveFile(option, callbackId) {
        runMethod('saveFile', option, callbackId);
    },
    TJ_FileSystemManagerTruncate(option, callbackId) {
        runMethod('truncate', option, callbackId);
    },
    TJ_FileSystemManagerUnzip(option, callbackId) {
        runMethod('unzip', option, callbackId);
    },
    TJ_FileSystemManagerWrite(option, data, callbackId) {
        const config = formatJsonStr(option);
        config.data = data.buffer;
        runMethod('write', config, callbackId);
    },
    TJ_FileSystemManagerWriteString(option, callbackId) {
        runMethod('write', option, callbackId, true);
    },
    TJ_FileSystemManagerReaddirSync(dirPath) {
        const fs = tj.getFileSystemManager();
        try {
            
            return JSON.stringify(fs.readdirSync(dirPath) || []);
        }
        catch (e) {
            console.error(e);
            return '[]';
        }
    },
    TJ_FileSystemManagerReadCompressedFileSync(option, callbackId) {
        const fs = tj.getFileSystemManager();
        const res = fs.readCompressedFileSync(formatJsonStr(option));
        cacheArrayBuffer(callbackId, res);
        return res.byteLength;
    },
    TJ_FileSystemManagerAppendFileStringSync(filePath, data, encoding) {
        const fs = tj.getFileSystemManager();
        fs.appendFileSync(filePath, data, encoding);
    },
    TJ_FileSystemManagerAppendFileSync(filePath, data, encoding) {
        const fs = tj.getFileSystemManager();
        fs.appendFileSync(filePath, data.buffer, encoding);
    },
    TJ_FileSystemManagerRenameSync(oldPath, newPath) {
        const fs = tj.getFileSystemManager();
        fs.renameSync(oldPath, newPath);
        return 'ok';
    },
    TJ_FileSystemManagerReadSync(option, callbackId) {
        const fs = tj.getFileSystemManager();
        const res = fs.readSync(formatJsonStr(option));
        cacheArrayBuffer(callbackId, res.arrayBuffer);
        return JSON.stringify({
            bytesRead: res.bytesRead,
            arrayBufferLength: res.arrayBuffer?.byteLength ?? 0,
        });
    },
    TJ_FileSystemManagerFstatSync(option) {
        const fs = tj.getFileSystemManager();
        const res = fs.fstatSync(formatJsonStr(option));
        formatResponse('Stats', res);
        return JSON.stringify(res);
    },
    TJ_FileSystemManagerStatSync(path, recursive) {
        const fs = tj.getFileSystemManager();
        const res = fs.statSync(path, recursive);
        let resArray;
        if (Array.isArray(res)) {
            resArray = res;
        }
        else {
            resArray = [res];
        }
        return JSON.stringify(resArray);
    },
    TJ_FileSystemManagerWriteSync(option, data) {
        const fs = tj.getFileSystemManager();
        const optionConfig = formatJsonStr(option);
        optionConfig.data = data.buffer;
        const res = fs.writeSync(optionConfig);
        return JSON.stringify({
            mode: res.bytesWritten,
        });
    },
    TJ_FileSystemManagerWriteStringSync(option) {
        const fs = tj.getFileSystemManager();
        const res = fs.writeSync(formatJsonStr(option));
        return JSON.stringify({
            mode: res.bytesWritten,
        });
    },
    TJ_FileSystemManagerOpenSync(option) {
        const fs = tj.getFileSystemManager();
        return fs.openSync(formatJsonStr(option));
    },
    TJ_FileSystemManagerSaveFileSync(tempFilePath, filePath) {
        const fs = tj.getFileSystemManager();
        return fs.saveFileSync(tempFilePath, filePath);
    },
    TJ_FileSystemManagerCloseSync(option) {
        const fs = tj.getFileSystemManager();
        fs.closeSync(formatJsonStr(option));
        return 'ok';
    },
    TJ_FileSystemManagerFtruncateSync(option) {
        const fs = tj.getFileSystemManager();
        fs.ftruncateSync(formatJsonStr(option));
        return 'ok';
    },
    TJ_FileSystemManagerTruncateSync(option) {
        const fs = tj.getFileSystemManager();
        fs.truncateSync(formatJsonStr(option));
        return 'ok';
    },
};
