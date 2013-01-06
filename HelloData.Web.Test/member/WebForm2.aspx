<%@ Page Title="" Language="C#" MasterPageFile="~/tempte/Site1.Master" AutoEventWireup="true"
    CodeBehind="WebForm2.aspx.cs" Inherits="HelloData.Web.Test.member.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <span runat="server" id="spanid"></span>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <a href="/en-us/default.aspx">english</a> <a href="/zh-cn/default.aspx">中文</a> <a
            href="../WebForm1.aspx">测试页面跳转</a>
    </div>
</asp:Content>
