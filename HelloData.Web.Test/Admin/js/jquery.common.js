
(function ($) {

    /*
    * xwAjaxPost form plugin
    * @requires jQuery v1.1 or later
    * post提交时统一处理整合代码
    *pre-need:jquery.form.js && jquery.blockUI.js
    * Revision: wangjun
    * Version: 1.0.0
    */

    /*
    *  buton的类型不要设置为submit。
    *  demo: function updaterole() {
    *    $("#form1").xwAjaxPost(function () {
    * 回调的ajax处理成功后的方法  parent.callback_refresh_diagclose();
    *    });
    *}
    */
    $.fn.xwAjaxPost = function (options) {

        if (typeof options == 'function')
            options = { EndPost: options };

        var defaults = {

        }
        var options = $.extend(defaults, options);
        this.each(function () {
            var suboptions = {
                dataType: 'json',
                beforeSubmit: postBefore,
                success: postSuccess,
                type:"post"
            };
            $(this).ajaxForm(suboptions);
            $(this).submit();
            return this;
        });
        /*结果为json格式*/
        function postSuccess(responseData, statusText) {
            var result = responseData.Result;
            var message = responseData.Message;
            if (result == undefined) {
                result = 1;
                message = "处理成功！";
            }
            if (typeof options.EndPost == 'function') {
                if ($("body").find("#isok").length == 0) {
                    $("body").append("   <input type='hidden' id='isok' />");
                }
                $("#isok").val("isok");
            }
            $.unblockUI();
            if ($("body").find("#loadingpanel222").length != 0) {
                $("#loadingpanel222").remove();
            }
            if (result == 1) {
                Dialog.alert(message, options.EndPost);
            }
            else {
                art.dialog({
                    lock: true,
                    fixed: true,
                    content: "message"
                }); 
            }
        }

        function postBefore() {
            if ($("body").find("#loading").length != 0) {
                $.blockUI({ message: $('#loading') });
            }
            else {
                $("body").append(" <div id='loadingpanel222'> <img alt='loading' id='loading' src='../Images/loading.gif')' style='display:none' /></div>");
                $.blockUI({ message: $('#loading') });
            }
        }
        function postError() {
            $.unblockUI();
            art.dialog({
                lock: true,
                fixed: true,
                content: "处理失败"
            });
        }

    };
    /*
    * disable form plugin
    * @requires jQuery v1.1 or later
    *disable  设置ture,false
     
    * Revision: wangwei
    * Version: 1.0.0
    */
    /*
    *demo: $(".input_style_E").disable(false);
    *       $(".input_style_E").disable(true);
    */
    $.fn.disable = function (options) {
        if (options == null || options == false) {
            this.each(function () {
                return $(this).removeAttr("disabled");
            });
        }
        else {
            this.each(function () {
                return $(this).attr("disabled", "disabled");
            });
        }
    };
    /*
    * xwAjaxPost form plugin
    * @requires jQuery v1.1 or later
    * post提交时统一处理整合代码
    *pre-need:jquery.form.js && jquery.blockUI.js
    * Revision: wangjun
    * Version: 1.0.0
    */

    /*
    *  buton的类型不要设置为submit。
    *  demo: function updaterole() {
    *    $("#form1").xwAjaxPost(function () {
    * 回调的ajax处理成功后的方法  parent.callback_refresh_diagclose();
    *    });
    *}
    */
    $.fn.xwAjaxPostpre = function (options) {

        if (typeof options == 'function')
            options = { EndPost: options };

        var defaults = {

        }
        var options = $.extend(defaults, options);
        this.each(function () {
            var suboptions = {
                dataType: 'json',
                beforeSubmit: postBefore,
                success: postSuccess
            };
            $(this).ajaxForm(suboptions);
            $(this).submit();
            return this;
        });
        /*结果为json格式*/
        function postSuccess(responseData, statusText) {
            var result = responseData.Result;
            var message = responseData.Message;
            if (result == undefined) {
                result = 1;
                message = "处理成功！";
            }

            $.unblockUI();
            if ($("body").find("#loadingpanel222").length != 0) {
                $("#loadingpanel222").remove();
            }
            if (result == 1) {
                alert(message);
               options.EndPost(); ;
            }
            else {
                alert(message);
            }
        }

        function postBefore() {
            if ($("body").find("#loading").length != 0) {
                $.blockUI({ message: $('#loading') });
            }
            else {
                $("body").append(" <div id='loadingpanel222'> <img alt='loading' id='loading' src='admin/Images/loading.gif')' style='display:none' /></div>");
                $.blockUI({ message: $('#loading') });
            }
        }
        function postError() {
            $.unblockUI();
            alert("");
        }

    };
})(jQuery);

 