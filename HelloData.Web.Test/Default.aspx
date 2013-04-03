<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HelloData.Web.Test._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/lib/jquery-1.8.0.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <span runat="server" id="spanid"></span>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <a href="/en-us/default.aspx">english</a> <a href="/zh-cn/default.aspx">中文</a>
        <a href="WebForm1.aspx">测试页面跳转</a>
    </div>
    </form>
</body>
</html>
