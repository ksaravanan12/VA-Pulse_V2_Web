<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GMSAdminlogin.aspx.vb" Inherits="GMSUI.GMSAdminlogin"  Title="GMS - Admin Login"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>GMS-Login</title>
    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <link rel="shortcut icon" href="centrakicon.ico" type="image/x-icon" />
    <script src="Javascript/jquery.js" type="text/javascript"></script>
    <script language="javascript">
      
        $(document).ready(function()
        {
            $("#txtUsername").focus(function(srcc)
            {
                if ($(this).val() == $(this)[0].title)
                {
                    $(this).removeClass("defaultTextActive");
                    $(this).val("");
                }
            });
            
            $("#txtUsername").blur(function()
            {
                if ($(this).val() == "")
                {
                    $(this).addClass("defaultTextActive");
                    $(this).val($(this)[0].title);
                }
            });
            $("#txtPassword").focus(function(srcc)
            {
                if ($(this).val() == $(this)[0].title)
                {
                    
                    $(this).removeClass("defaultTextActive");
                    $(this).val("");
                    $(this)[0].type="password";
                }
            });
            
            $("#txtPassword").blur(function()
            {
                if ($(this).val() == "")
                {
                    $(this).addClass("defaultTextActive");
                    $(this).val($(this)[0].title);
                   // $(this)[0].type="text";
                }
            });
            $("#txtUsername").blur(); 
            $("#txtPassword").blur(); 
                   
        });
        function Validate()
        {
            var UserName =  document.getElementById('txtUsername').value;
            
            if( UserName == "" || UserName == "User Name")
            {
                alert("Enter the Username");
                return false;
            }
            return true;
            
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="tblMain" cellpadding="0" cellspacing="0" border="0" style="width: 100%;
                height: 732px; vertical-align: middle;" align="center">
                <tr>
                    <td align="center" valign="middle">
                        <table cellpadding="5" cellspacing="0" border="0">
                            <tr>
                                <td class="login-logo">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="10" cellspacing="0" border="0" style="width: 100%;">
                                        <tr>
                                            <td>
                                                <input id="txtUsername" class="defaultText" title="User Name" type="text" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input id="txtPassword" class="defaultText" title="Password" type="text" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="10" cellspacing="0" border="0" style="width: 100%;">
                                        <tr>
                                            <td align="center">
                                                <input id="btnLogin" type="submit" runat="server" class="login-btn" value="Login" />
                                            </td>                                  
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td id="tdAlert" runat="server" class="login-alertBox" style="display: none;">
                                    Incorrect User Name or<br />
                                    Password</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
