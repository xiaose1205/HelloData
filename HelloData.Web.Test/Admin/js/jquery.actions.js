$().ready(function () {

});

/*操作添加的页面*/
$.AddAction = function (width, height, name, url, funCallback) {
    art.dialog.open(url,
    { title: name, width: width, height: height, close: function () {
        var bValue = art.dialog.data('bValue'); // 读取B页面的数据
        if (bValue !== undefined) funCallback();
    },
    lock: true,
        resize: false,  
    });
};
/*操作修改的页面*/
$.EditAction = function (width, height, name, url, funCallback) {
    var checkedRows = $("#grid").getCheckedRows();
    if (!checkedRows || checkedRows.length <= 0) {
        art.dialog.alert("请选择需要编辑的数据！");
        return false;
    }
    if (checkedRows.length > 1) {
        art.dialog.alert("每次只能编辑一个数据！");
        return false;
    }
    var id = checkedRows[0][0];
    art.dialog.open(url,
    { title: name, width: width, height: height, close: function () {
        var bValue = art.dialog.data('bValue'); // 读取B页面的数据
        if (bValue !== undefined) funCallback();
    },
    lock: true, 
    resize: false
    });

};
/*操作删除的页面*/
$.DeleteAction = function (handler, funCallback) {
    var checkedRows = $("#grid").getCheckedRows();
    if (!checkedRows || checkedRows.length <= 0) {
        art.dialog.alert("请选择需要删除的数据！");
        return false;
    }
    var ids = "";
    for (var i = 0; i < checkedRows.length; i++) {
        ids += checkedRows[i][0] + ",";
    }
    art.dialog.confirm("确认要删除选中数据吗?", function () {
        $.post("/AjaxHandler.ashx", { handler: handler, type: 'delete', ids: ids }, function (data) {
            if (data.Result == 1) {
                funCallback();
            } else {
                art.dialog.alert(data.Message);
            }
        }, "json");
    }
    , function () {
    });
};

$.childAction = function (fun) {
    var isunvailty = false ;
    $("#form1").find("input[type][name]").each(function () {

        if (($(this).attr("data-null") != "" && $(this).attr("data-null") == "false") && $.trim($(this).val()) == "") {
            art.dialog.alert($(this).attr("data-msg"));
            isunvailty = true;
            $(this).focus();
            return false;
        }
        if ($(this).attr("data-minlength") != undefined && $(this).attr("data-minlength") != "" && $.trim($(this).val()).length < $(this).attr("data-minlength")) {
            art.dialog.alert("输入字符长度请大于" + $(this).attr("data-minlength") + "");
            $(this).focus(); isunvailty = true; return false;
        }
        if (($(this).attr("data-maxlength") != undefined
            && $(this).attr("data-maxlength") != "")
            && $.trim($(this).val()).length > $(this).attr("data-maxlength")) {
            art.dialog.alert("输入字符长度请小于" + $(this).attr("data-maxlength") + "");
            $(this).focus(); isunvailty = true; return false;
        }
        if ($(this).attr("data-reg") != undefined && $(this).attr("data-reg") != "") {
            var reg = $(this).attr("data-reg");
            if (!reg.test($(this).val())) {
                art.dialog.alert($(this).attr("data-msg"));
                $(this).focus(); isunvailty = true; return false;
            }
        }
    });
    if (isunvailty == false) {
        $("#form1").xwAjaxPost(fun);
    }
};