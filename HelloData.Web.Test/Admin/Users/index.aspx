<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Maste/MainPage.Master" AutoEventWireup="true"
    CodeBehind="index.aspx.cs" Inherits="HelloData.Web.Test.Admin.Users.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


        $(document).ready(function () {

            $("#grid").flexigrid({
                url: '/AjaxHandler.ashx?handler=user&type=list',
                dataType: 'json',
                colModel: [
                    { display: 'id', name: 'id', width: 100, align: 'center', hide: true },
                    { display: '用户名称', name: 'username', width: 250, align: 'center' },
                    { display: '手机号码', name: 'phone', width: 150, align: 'center' },

                    { display: '所属经理', name: 'mangername', width: 150, align: 'center' }
                    ],
                minColToggle: 1,
                onrowclick: false,
                sortname: "id",
                sortorder: "asc",
                usepager: true,
                useRp: true,
                rp: 15,
                resizable: false,
                width: 'auto',
                height: 'auto',
                autoload: true,
                singleSelect: true,
                specify: true,
                striped: true,
                showcheckbox: true,
                showToggleBtn: true

            });
            doQuery();
            $("#add").click(function () {
                $.AddAction(600, 280, '添加用户', "add.aspx", doQuery); ;
            });
            $("#edit").click(function () {
                $.EditAction(600, 280, '添加用户', "edit.aspx", doQuery); ;
            });
            $("#delete").click(function () {
                $.DeleteAction("user", doQuery); ;
            });
        });
        function doQuery() {
            var contactQuery = {
                "name": $("#name").val()
            };
            var params = {
                extParam: contactQuery
            };
            $('#grid')[0].p.newp = 1;
            $('#grid').flexOptions(params).flexReload();
        }
        function repwd() {
            var checkedRows = $("#grid").getCheckedRows();
            if (!checkedRows || checkedRows.length <= 0) {
                Dialog.alert("请选择需要重置密码的用户！");
                return false;
            }
            var ids = "";
            for (var i = 0; i < checkedRows.length; i++) {
                ids += checkedRows[i][0] + ",";
            }
            Dialog.confirm("确认要重置密码选中数据吗?", function () {
                $.post("/AjaxHandler.ashx", { handler: 'user', type: 'repwd', ids: ids }, function (data) {
                    if (data.Result == 1) {
                        doQuery();
                    } else {
                        Dialog.alert(data.Message);
                    }
                }, "json");
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="btn-toolbar">
        <div class="btn-group">
            <a class="btn " id="add"><i class=" icon-plus-sign"></i>新增 </a><a class="btn " href="#"
                id="edit"><i class=" icon-edit"></i>修改 </a><a class="btn " href="#" id="delete"><i
                    class=" icon-remove"></i>删除 </a>
        </div>
        <div class="btn-group">
            <a class="btn " href="#"><i class=" icon-upload"></i>导入 </a><a class="btn " href="#">
                <i class=" icon-download"></i>导出 </a>
        </div>
        <div class="btn-group">
            <a class="btn " href="#"><i class=" icon-user"></i>修改密码 </a>
        </div>
    </div>
    <div class="cont_tools">
        <div class="input-prepend">
            <i class="icon-eye-open"></i><span>用户名称:</span><input id="prependedInput" size="16"
                type="text" />
            <button class="btn  btn-primary" type="submit" onclick="doQuery()">
                <i class="icon-white icon-search"></i>查询
            </button>
        </div>
    </div>
    <table id="grid">
    </table>
</asp:Content>
