define(function (require, exports, module) {

    require("/css/book.css");
    var $ = require("../lib/jquery");
    $(".navBtn").click(function () {
      
        if ($(".navGroup").css("display") == "none")
            $(".navGroup").show("slow");
        else {
            $(".navGroup").hide("slow");

        }
    });

});