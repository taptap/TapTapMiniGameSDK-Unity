let logger;
export default {
    TJLogManagerDebug(str) {
        if (!logger) {
            logger = tj.getLogManager({ level: 0 });
        }
        logger.debug(str);
    },
    TJLogManagerInfo(str) {
        if (!logger) {
            logger = tj.getLogManager({ level: 0 });
        }
        logger.info(str);
    },
    TJLogManagerLog(str) {
        if (!logger) {
            logger = tj.getLogManager({ level: 0 });
        }
        logger.log(str);
    },
    TJLogManagerWarn(str) {
        if (!logger) {
            logger = tj.getLogManager({ level: 0 });
        }
        logger.warn(str);
    },
};
