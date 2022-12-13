<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="siteoverview.aspx.vb" Inherits="GMSUI.siteoverview" title="Site over view" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script>
     function Redirect(sid){
            location.href="Home.aspx"
        }
</script>
<table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 85%;" id="siteOverview" runat="server">
                    
                </table>
            </td>
        </tr>
        <tr style=" height:20px;"></tr>
        <tr>
        <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 85%;" id="siteotherDevice" runat="server">
                        
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

