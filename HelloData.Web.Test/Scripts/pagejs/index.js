define(function (require, exports, module) {
    var $ = require('lib/jquery.js');
     
    $("#home_banner").scroll({
        width: 480,
        imgCls: ".banner_list",
        poiCls: ".ban_ic",
        prevBtn: ".banner_btn_l",
        nextBtn: ".banner_btn_r",
        poiNorCls: "ban_ash",
        poiCurCls: "ban_acti"
    });
    function b(e) {
        var d = $(this);
        var c = d.find(".mark a");
        var f = c.length;
        var g = d.find(".touchblock");
        if (e < 1) {
            e = f;
            g.removeClass("ani").css("left", (f + 1) * -100 + "%");
        } else {
            if (e > f) {
                e = 1;
                g.removeClass("ani").css("left", "0px");
            }
        }
        setTimeout(function () {
            g.addClass("ani").css("left", -100 * e + "%");
            d.find(".mark_line").css("left", 61 * (e - 1) + "px");
            c.removeClass("current");
            $(c[e - 1]).addClass("current");
        },
        0);
    }
    $(".blockContainer").each(function () {
        var c = $(this);
        var d = c.find(".touchblock section");
        var f = d.length;
        c.find(".mark a").each(function (g, h) {
            $(h).bind("click",
            function () {
                b.call(c, g + 1);
            });
        });
        if (f <= 1) {
            return;
        }
        _first = d.get(0);
        _last = d.get(d.length - 1);
        $(_last).clone().insertBefore($(_first));
        $(_first).clone().insertAfter($(_last));
        var e = 100 / (f + 2) + "%";
        c.find(".touchblock").css("width", (f + 2) * 100 + "%").css("left", "-100%");
        c.find(".touchblock section").css({
            "width": e
        });
    });
    function a() {
        var e = {},
        c = $(this).find(".mark a");
        e.len = c.length;
        for (var d = 0; d < e.len; d++) {
            if (/current/.test(c[d].className)) {
                e.cur = d + 1;
            }
        }
        return e;
    }
    $(".blockContainer").swipeLeft(function () {
        var c = a.call($(this));
        c.cur++;
        b.call($(this), c.cur);
    }).swipeRight(function () {
        var c = a.call($(this));
        c.cur--;
        b.call($(this), c.cur);
    });
    $.ajax({
        url: "/t/continue/",
        cache: false,
        data: "",
        dataType: "json",
        success: function (c) {
            if (c.status == 0) {
                $("#continueA").attr("href", c.continueUrl);
                $("#continueA").html(c.title);
                $("#continue").removeClass("hidden");
            }
        }
    });
 
});