define(function (require, exports, module) {
    var $ = require('lib/jquery.js');
    var resources = require('resources');
    var routes = require('./routes');
    var querystring = require('lib/querystring');

    var form = require("./common.actions");
    $("#showmsg").click(function () { 

        form.alertMsg("用户名密码错误",0);
    }); 
    var route, search, qs;
    exports.run = function () {
        /**异步加载所需要的模板和样式文件,配置在resources.js中**/
        $.each(resources, function (key, val) {
            require.async(resources[key], function () {

                if (typeof (Storage) !== "undefined") {

                    sessionStorage.setItem(key, val);
                }
            });
        });
        /**传递querystring参数过去**/
        search = window.location.search;
        if (typeof search !== 'undefined' && search != '') {
            search = search.substring(1);
            qs = querystring.parse(search);
        } else {
            qs = {};
        }

        /**判断当前页面是属于什么目录，按照routes配置，执行相应js**/
        if (typeof window.location.pathname !== 'undefined' && window.location.pathname != null && window.location.pathname != '') {
            var route = window.location.pathname.toLocaleLowerCase();
            if (typeof routes[route] !== 'undefined') {
                require.async(routes[route], function (app) {
                    if (typeof app !== 'undefined' && typeof app.init !== 'undefined') {
                        app.init(qs);
                    }
                });
            } else {
                // default redirect to index page
                require.async(routes["/index.html"], function (app) {
                    if (typeof app.init !== 'undefined') {
                        app.init(qs);
                    }
                });
            }
        }

    };
    var scrollcheck = function () {
        $(document).ready(function () {
            var w = $(window).width() - 20;
            if (w > 620) {
                w = 620;
            }
            $(".article img").each(function (i) {
                var image = new Image();
                image.src = $(this).attr("src");
                image.onerror = function () {
                    $(".article img:eq(" + i + ")").attr("src", "/theme/images/noimg2.jpg");
                };

                if ($(".article img:eq(" + i + ")").width() > w) {
                    var h = $(".article img:eq(" + i + ")").height() * w / $(".article img:eq(" + i + ")").width()
                    $(".article img:eq(" + i + ")").width(w);
                    $(".article img:eq(" + i + ")").height(h);
                }
            });

            $('#top').click(function () {
                $("html, body").animate({ scrollTop: 0 }, -10);
                return false;
            });
        });
    };

    $(document).bind('scroll', function () {
        scrollcheck();
        if ($(window).scrollTop() <= $(window).height()) {
            $("#top").stop(true, false).animate({ bottom: "-200px" }, 500);
            $("#top").hide();
        } else {
            $("#top").stop(true, false).animate({ bottom: "15px" }, 500);
            $("#top").show();
        }
    });
});