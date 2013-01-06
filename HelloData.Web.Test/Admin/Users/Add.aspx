<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="HelloData.Web.Test.Admin.Users.Add" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Style/menu.css" rel="stylesheet" type="text/css" />
    <link href="../Style/common.css" rel="stylesheet" type="text/css" />
    <link href="../Style/theme/hello-cerulean.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.form.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../../Scripts/artDialog.js?skin=idialog" type="text/javascript"></script>
    <script src="../../Scripts/plugins/iframeTools.source.js" type="text/javascript"></script>
    <script src="../js/jquery.common.js" type="text/javascript"></script>
    <script src="../js/jquery.actions.js" type="text/javascript"></script>
</head>
<body id="child_form">
    <form id="form1" runat="server">
    <div class="z-legend">
        <strong>用户基本信息</strong>
    </div>
    <table cellspacing="0" cellpadding="2" width="100%" align="center" class="formTable">
        <tbody>
            <tr>
                <td valign="middle" align="center">
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tbody>
                            <tr>
                                <td height="30" style="width: 160px;" align="right">
                                    用户昵称：
                                </td>
                                <td align="left">
                                    <input id="name" value="" name="name" size="36" type="text" data-null="false" data-msg="请输入用户名称" /><span
                                        class="notnull">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td height="30" align="right">
                                    用户密码：
                                </td>
                                <td align="left">
                                    <input type="password" name="password" id="password" maxlength="50" data-null="false"
                                        data-minlength="6" data-msg="请输入用户密码" /><span class="notnull">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td height="30" align="right">
                                    确认密码：
                                </td>
                                <td align="left">
                                    <input type="password" name="repassword" id="password1" maxlength="50" /><span class="notnull">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td height="30" align="right">
                                    联系电话：
                                </td>
                                <td align="left">
                                    <input type="text" name="phone" id="phone" />
                                </td>
                            </tr>
                            <tr>
                                <td height="30" align="right">
                                    所属经理：
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="managelists" runat="server" Width="120px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td height="30" align="right">
                                    是否管理员：
                                </td>
                                <td align="left">
                                    <input type="radio" name="isadmin" id="isadmin" value="1" checked="checked" />是
                                    <input type="radio" name="isadmin" id="isadmin1" value="0" />否
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
   <%-- <div class="autool_buttons">
        <button class="btn  btn-primary" type="button">
            <i class="icon-white icon-ok"></i>确认
        </button>
        <button class="btn  btn-primary" type="button">
            <i class="icon-white icon-repeat"></i>重置
        </button>
    </div>--%>
    </form>
</body>
</html>
<script type="text/javascript">
    var api = art.dialog.open.api;
    api.button(
		{
		    name: '登录',
		    callback: function () {
		        if (check(username) && check(password)) form.submit();
		        return false;
		    },
		    focus: true
		},
		{
		    name: '取消'
		}
	);
    $(document).ready(function () {
        art.dialog.tips("dasdsad");
        $(".btn").click(function () {
            if ($("#password").val() != $("#password1").val()) {
                Dialog.alert("两次密码输入不相同");
                return false;
            }
            $.childAction(parentDialog.cancelButton.onclick);
        });
    });

</script>
