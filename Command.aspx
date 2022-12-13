<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Command.aspx.vb" Inherits="Command" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Command Dialog</title>

    <script type="text/javascript" language="javascript" src="Javascript/jquery-1.10.2.js"></script>
    
    <script type="text/javascript">
        this.onload = function () 
        {
           HideTagCtrl();
           HideMonitorCtrl();
        }
        
        function TagValidate()
        {
           var TagId = $('#txtCommandTagId').val();
           
            if(TagId == "")
            {
                alert("Id should not be empty !!!");
                $('#txtTagId').focus();
                return false;
            }
            
            if(TagId == 0)
            {
                alert("Id is not a valid !!!");
                $('#txtTagId').focus();
                return false;
            }
            return true;
        }
        
        function fnAddTagCommand()
        {
            var sTagId = $('#txtCommandTagId').val();
            var sTagValue = $('#txtTagValue').val();
            var sTagCommand = $('#ddlTagCommand').val();
            
            var TagCommandObj = {};

            TagCommandObj["TagId"] = sTagId;
            TagCommandObj["Value"] = sTagValue;
            TagCommandObj["Command"] = sTagCommand;

            var tagCommandjson = JSON.stringify(TagCommandObj);

            //var DbConnectorPath = "AjaxConnector.aspx?cmd=PushDataToRedis&sid=" + parseInt(getParameterByName("siteid")) + "&jsonpushdata=" + tagCommandjson + "&deviceid=" + sTagId + "&devicetype=1";
            var DbConnectorPath = "AjaxConnector.aspx?cmd=PushDataToRedis&sid=1&jsonpushdata=" + tagCommandjson + "&deviceid=" + sTagId + "&devicetype=1&ChannelName='chlCommand'";
        }
       
        function HideTagCtrl()
        {
            document.getElementById("txtTagValue").style.display='none';
            document.getElementById("ddlTagLEDON").style.display='none';
            document.getElementById("ddlTagSETFREQUENCY").style.display='none';
            document.getElementById("ddlTagSETLFREGISTERCONFIG").style.display='none';
            document.getElementById("ddlTagTURNONVIBRATOR").style.display='none';
            document.getElementById("ddlTagTURNONBUZZER").style.display='none';
        }
        
        function HideMonitorCtrl()
        {
            document.getElementById("txtMonitorValue").style.display='none';
            document.getElementById("ddlMonitorSETFREQUENCY").style.display='none';
        }
        
        //Tag Command Change
        function showTagCommandValue(ctrl) 
        {
            HideTagCtrl();
               
            if(ctrl.value == "0x2" )
            {
                document.getElementById("ddlTagLEDON").style.display='inline';
            }
            else if(ctrl.value == "0x4" )
            {
                document.getElementById("ddlTagSETFREQUENCY").style.display='inline';
            }
            else if(ctrl.value == "0xA" )
            {
                document.getElementById("ddlTagSETLFREGISTERCONFIG").style.display='inline';
            }
            else if(ctrl.value == "0x11" )
            {
                document.getElementById("ddlTagTURNONVIBRATOR").style.display='inline';
            }
             else if(ctrl.value == "0x1" )
            {
                document.getElementById("ddlTagTURNONBUZZER").style.display='inline';
            }
        } 
        
        function SetTagCommandValue(ctrl)
        {
            $('#txtTagValue').val(ctrl.value);
        }
        
        function SetMonitorCommandValue(ctrl)
        {
            $('#txtMonitorValue').val(ctrl.value);
        }
        
        //Monitor Command Change
        function showMonitorCommandValue(ctrl) 
        {
            HideMonitorCtrl();
               
            if(ctrl.value == "0x3" )
            {
                document.getElementById("ddlMonitorSETFREQUENCY").style.display='inline';
            }
        } 
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="tblCommand" runat="server">
            <table cellpadding="3" cellspacing="3">
                <tr>
                    <td colspan="3">
                        Send Command To Tag</td>
                </tr>
                <tr>
                    <td>
                        Tag Id:</td>
                    <td>
                        <input type="text" id="txtCommandTagId" style="width: 220px;"/></td>
                    <td>
                        <input type="button" id="btnTagAddCommand" value="Add Command" class="clsButton"
                            onclick="if(! TagValidate()) return false; fnAddTagCommand()" /></td>
                </tr>
                <tr>
                    <td>
                        Command:</td>
                    <td colspan="2">
                        <select id="ddlTagCommand" style="width: 370px;" onchange="showTagCommandValue(this)">
                            <option value="0x1">RESET</option>
                            <option value="0x2">LED ON</option>
                            <option value="0x3">LED OFF</option>
                            <option value="0x4">SET FREQUENCY</option>
                            <option value="0x5">GET PROFILE</option>
                            <option value="0x6">GET VERSION</option>
                            <option value="0x7">GET BATTERY STATUS</option>
                            <option value="0x14">GET TEMPERATURE</option>
                            <option value="0xA">SET LF REGISTER CONFIG</option>
                            <option value="0xB">GET LF REGISTER CONFIG</option>
                            <option value="0xC">SET FACTORY SLEEP</option>
                            <option value="0x11">TURN ON VIBRATOR</option>
                            <option value="0x12">TURN OFF VIBRATOR</option>
                            <option value="0x1">TURN ON BUZZER</option>
                            <option value="0x12">TURN OFF BUZZER</option>
                            <option value="0x1A">ACTIVATE HIGH ALERT</option>
                            <option value="0x1B">ACTIVATE LOW ALERT</option>
                            <option value="0x19">CLEAR ALERT</option>
                            <option value="0x15">CLEAR OFFLINE TEMPERATURE</option>
                            <option value="0x16">SET CALIBRATION PARAMS</option>
                            <option value="0x17">GET CALIBRATION PARAMS</option>
                            <option value="0x18">ENTER CALIBRATION MODE</option>
                            <option value="0x20">GET SUMMARY INFO</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        Value:</td>
                    <td colspan="2">
                        <input type="text" id="txtTagValue" style="width: 220px;" />
                        <select id="ddlTagLEDON" style="width: 370px;" onchange="SetTagCommandValue(this)">
                            <option value="0x1" selected="selected">2ms On</option>
                            <option value="0x2">4ms On</option>
                            <option value="0x3">8ms OnF</option>
                            <option value="0x4">16ms On</option>
                            <option value="0x5">32ms On</option>
                            <option value="0x6">64ms On</option>
                            <option value="0x7">128ms On</option>
                        </select>
                        <select id="ddlTagSETFREQUENCY" style="width: 370px;" onchange="SetTagCommandValue(this)">
                            <option value="0x1" selected="selected">1 - 904 and 914 MHz (NA)</option>
                            <option value="0x2">2 - 905 and 915 MHz (NA)</option>
                            <option value="0x3">3 - 906 and 916 MHz (NA)</option>
                            <option value="0x4">4 - 907 and 917 MHz (NA)</option>
                            <option value="0x5">5 - 908 and 918 MHz (NA)</option>
                            <option value="0x6">6 - 909 and 919 MHz (NA)</option>
                            <option value="0x7">7 - 912 and 917 MHz (Spectralink)</option>
                            <option value="0x8">8 - 915 and 918 MHz (Spectralink)</option>
                            <option value="0x9">9 - 868.9 and 869.9 MHz (EU)</option>
                            <option value="0x10">10 - 868.4 and 869.9 MHz (EU)</option>
                            <option value="0x11">11 - 922 and 926 MHz (AU)</option>
                            <option value="0x12">12 - 865.3 and 866.3 MHz (IN)</option>
                            <option value="0x13">13 - 865.7 and 866.7 MHz (IN)</option>
                        </select>
                        <select id="ddlTagSETLFREGISTERCONFIG" style="width: 370px;" onchange="SetTagCommandValue(this)">
                            <option value="0x1" selected="selected">High</option>
                            <option value="0x2">Medium High</option>
                            <option value="0x3">Medium</option>
                            <option value="0x4">Low</option>
                            <option value="0x5">None</option>
                        </select>
                        <select id="ddlTagTURNONVIBRATOR" style="width: 370px;" onchange="SetTagCommandValue(this)">
                            <option value="0x1" selected="selected">OnTime 500ms; OffTime 500ms</option>
                            <option value="0x2">OnTime 2secs; OffTime 1sec</option>
                            <option value="0x3">OnTime 1sec; OffTime 1sec</option>
                        </select>
                        <select id="ddlTagTURNONBUZZER" style="width: 370px;" onchange="SetTagCommandValue(this)">
                            <option value="0x1" selected="selected">OnTime 1sec; OffTime 1sec</option>
                            <option value="0x2">OnTime 500ms; OffTime 500ms</option>
                            <option value="0x3">OnTime 2secs; OffTime 1sec</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        Send Command To Monitor</td>
                </tr>
                <tr>
                    <td>
                        Monitor Id:</td>
                    <td>
                        <input type="text" id="txtMonitorId" style="width: 220px;" /></td>
                    <td>
                        <input type="button" id="btnMonitorAddCommand" value="Add Command" class="clsButton"
                            onclick="if(! Validate()) return false;" /></td>
                </tr>
                <tr>
                    <td>
                        Command:</td>
                    <td colspan="2">
                        <select id="ddlMonitorCommand" style="width: 370px;"  onchange="showMonitorCommandValue(this)">
                            <option value="0x1">RESET</option>
                            <option value="0x3">SET FREQUENCY</option>
                            <option value="0x5">GET PROFILE</option>
                            <option value="0x6">GET VERSION</option>
                            <option value="0x7">GET BATTERY STATUS</option>
                            <option value="0x10">EGRESS BUZZER ON</option>
                            <option value="0x11">EGRESS BUZZER OFF</option>
                            <option value="0x12">CLEAR SUMMARY INFO</option>
                            <option value="0x13">GET SUMMARY INFO</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        Value:</td>
                    <td colspan="2">
                        <input type="text" id="txtMonitorValue" style="width: 220px;" />
                        <select id="ddlMonitorSETFREQUENCY" style="width: 370px;" onchange="SetMonitorCommandValue(this)">
                            <option value="0x1" selected="selected">1 - 904 and 914 MHz (NA)</option>
                            <option value="0x2">2 - 905 and 915 MHz (NA)</option>
                            <option value="0x3">3 - 906 and 916 MHz (NA)</option>
                            <option value="0x4">4 - 907 and 917 MHz (NA)</option>
                            <option value="0x5">5 - 908 and 918 MHz (NA)</option>
                            <option value="0x6">6 - 909 and 919 MHz (NA)</option>
                            <option value="0x7">7 - 912 and 917 MHz (Spectralink)</option>
                            <option value="0x8">8 - 915 and 918 MHz (Spectralink)</option>
                            <option value="0x9">9 - 868.9 and 869.9 MHz (EU)</option>
                            <option value="0x10">10 - 868.4 and 869.9 MHz (EU)</option>
                            <option value="0x11">11 - 922 and 926 MHz (AU)</option>
                            <option value="0x12">12 - 865.3 and 866.3 MHz (IN)</option>
                            <option value="0x13">13 - 865.7 and 866.7 MHz (IN)</option>
                        </select>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
