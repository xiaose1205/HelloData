define(function (require, exports, module) {
    var $ = require('lib/jquery.js');
    require("lib/messenger.js");

    $._messengerDefaults = {
        extraClasses: 'messenger-fixed messenger-theme-air messenger-on-top'
    };

    /*弹出层*/
    exports.alertMsg = function (message, result, func) {
        $.globalMessenger().post({
            message: message,
            type: result == 1 ? 'success' : 'error',
            showCloseButton: true
        });

    };
    /*多个一起判断*/
    exports.confirmMsg = function (message, okvalue, canelvalue, funcok, funccanel) {
        $.globalMessenger().post({
            message: message,
            actions: {
                retry: {
                    label: okvalue,
                    phrase: 'Retrying TIME',
                    auto: true,
                    delay: 10,
                    action: function () {
                        if (typeof (funcok) != "undefined")
                            funcok();
                    }
                },
                cancel: {
                    label: canelvalue,
                    action: function () {
                        if (typeof (funccanel) != "undefined")
                            funccanel();
                        return msg.cancel();
                    }
                }
            }
        });
    };

});