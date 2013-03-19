//configure seajs
seajs.config({
    alias: {

        'jquery': 'lib/jquery.js',
        'bootstrap': 'lib/bootstrap'
    },
    preload: ['jquery', 'bootstrap']
    


});
define(function (require, exports, module) {

    require('./init').run();

});
