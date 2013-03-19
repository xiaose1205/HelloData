define(function (require, exports, module) {
    var json = require('json');

    exports.setSession = function (key, cachedata) {
        if (typeof (Storage) !== "undefined") {
            sessionStorage.setItem(key, json.stringify(cachedata));
        }
    };

    exports.getSession = function (key) {
        if (typeof (Storage) !== "undefined") {
            var cachedata = sessionStorage.getItem(key);
            if (cachedata == null) {
                return null;
            }
            return json.parse(cachedata);
        } else {
            return null;
        }
    };

    exports.removeSession = function (key) {
        if (typeof (Storage) !== "undefined") {
            sessionStorage.removeItem(key);
        }
    };

    // <param name="key">关键词</param>
    // <param name="objectxml">缓存对象</param>
    // <param name="cacheTime">缓存时间，以分为单位</param>
    exports.setCache = function (key, objectxml, cacheTime) {
        if (typeof (Storage) !== "undefined") {
            var nowTime = Date.parse(new Date());
            var expiredTime = nowTime + cacheTime * 60 * 1000;
            if (cacheTime < 0) {
                return;
            }
            var cachedata = {};
            cachedata.expiredTime = cacheTime == 0 ? 0 : expiredTime;
            cachedata.value = objectxml;
            localStorage.setItem(key, json.stringify(cachedata));
        }
    };

    exports.getCache = function (key) {
        if (typeof (Storage) !== "undefined") {
            var nowTime = Date.parse(new Date());

            var cachedata = localStorage.getItem(key);
            if (cachedata == null) {
                return null;
            }
            cachedata = json.parse(cachedata);
            if (cachedata.expiredTime > nowTime || cachedata.expiredTime == 0) {
                return cachedata.value;
            } else {
                localStorage.removeItem(key);
                return null;
            }
        } else {
            return null;
        }
    };

    exports.removeCache = function (key) {
        if (typeof (Storage) !== "undefined") {
            localStorage.removeItem(key);
        }
    };

});