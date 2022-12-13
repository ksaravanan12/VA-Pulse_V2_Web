<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    Title="Site List" Inherits="GMSUI.SiteList" CodeFile="SiteList.aspx.vb" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=3"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_SetupSite.js?d=150"></script>
    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <link rel="stylesheet" type="text/css" href="scripts/demo_table_jui.css" />
    <link rel="stylesheet" type="text/css" href="scripts/jquery-ui-1.7.2.custom.css" />
    <script type="text/javascript" src="scripts/Pagingjquery.dataTables.min.js"></script>
    <script type="text/javascript" src="scripts/jquery-1.7.2.js"></script>
    <script type="text/javascript" src="Javascript/jquery-ui.min.js"></script>
    <script type="text/javascript" language="javascript" src="scripts/PagingDatatablesLoadGraph.js"></script>
    <script language="javascript" type="text/javascript">

        var g_isUserVerified = false;

        function EditValidate() {

            var strpwd = document.getElementById("<%=txtpwd.ClientID%>");
            var conpwd = document.getElementById("<%=hid_pwd.ClientID%>");

            if (Trim(strpwd.value) == "") {
                alert("Password should not be empty!");
                strpwd.value = "";
                strpwd.focus();
                return false;
            }
            else if (Trim(strpwd.value) != Trim(conpwd.value)) {
                alert("Incorrect Password!");
                strpwd.value = "";
                strpwd.focus();
                return false;
            }

            return true;
        }

        function Validate() {

            var strCompany = document.getElementById("<%=selCompany.ClientID%>");
            var strSiteName = document.getElementById("<%=txtSiteName.ClientID%>");
            var strSiteFolderName = document.getElementById("<%=txtSiteFolder.ClientID%>");
            var strpwd = document.getElementById("<%=txtpwd.ClientID%>");
            var conpwd = document.getElementById("<%=hid_pwd.ClientID%>");

            if (Trim(strCompany.value) == 0) {
                alert("Select company name!");
                strCompany.value = 0;
                strCompany.focus();
                return false;
            }
            else if (Trim(strSiteName.value) == "") {
                alert("Site name should not be empty!");
                strSiteName.value = "";
                strSiteName.focus();
                return false;
            }
            else if (Trim(strSiteFolderName.value) == "") {
                alert("Site Folder name should not be empty!");
                strSiteFolderName.value = "";
                strSiteFolderName.focus();
                return false;
            }
            else if (Trim(strpwd.value) == "") {
                alert("Password should not be empty!");
                strpwd.value = "";
                strpwd.focus();
                return false;
            }
            else if (Trim(strpwd.value) != Trim(conpwd.value)) {
                alert("Incorrect Password!");
                strpwd.value = "";
                strpwd.focus();
                return false;
            }

            return true;
        }

        //function to trim the preceedind and trailing spaces in that given string
        function Trim(str) {
            while (str.substring(0, 1) == ' ') {
                str = str.substring(1, str.length);
            }
            while (str.substring(str.length - 1, str.length) == ' ') {
                str = str.substring(0, str.length - 1);
            }
            return str;
        }	

    </script>
    <script language="javascript" type="text/javascript">

        var g_UserRole = "";
        var g_UserId = "";
        var curSiteid = "";

        var isButtonClicked = 0;
        var isChanged = 0;

        this.onload = function () {

            document.getElementById('tdLeftMenu').style.display = "none";
            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;

            LoadSiteData(0);
        }

        function Redirect(sid) {

            location.href = "Setting.aspx";
        }

        function ShowAddSite() {

            clearCtrlData();
            document.getElementById("<%=ddServerName.ClientID%>").disabled = false;

            document.getElementById("<%=ddServerName.ClientID%>").style.display = '';
            document.getElementById("<%=ddEditServerName.ClientID%>").style.display = 'none';

            document.getElementById("<%=btnSave.ClientID%>").style.display = '';
            document.getElementById("<%=btnUpdate.ClientID%>").style.display = 'none';
            document.getElementById("<%=trIsLinked.ClientID%>").style.display = 'none';
            document.getElementById("<%=trIsGroup.ClientID%>").style.display = 'none';
            document.getElementById("<%=trStatus.ClientID%>").style.display = 'none';

            $('#tdAddSite').show('slide', { direction: 'left' }, 500);
        }

        function HideAddSite() {

            clearCtrlData();
            $('#tdAddSite').hide('slide', { direction: 'left' }, 500);
        }

        function LoadSiteData(isExport) {

            document.getElementById("divLoading").style.display = "";

            Load_Setup_Site(isExport);
        }

        function CallSiteSetup(IsAdd) {

            var masterid, companyid, sitename, sitefolder, fileformat, status, isgroup, isgroupmember, serverip, authuserid, authpassword,
                locationcode, qbn, IsPrismView, TimeZone, IsUnDeleteTags, IsUnDeleteMonitors, IsDefinedTagsinCore, IsDefinedInfrastructureinCore;

            masterid = document.getElementById("<%=drpMaster.ClientID%>").value;
            companyid = document.getElementById("<%=selCompany.ClientID%>").value;
            sitename = document.getElementById("<%=txtSiteName.ClientID%>").value;
            sitefolder = document.getElementById("<%=txtSiteFolder.ClientID%>").value;
            fileformat = document.getElementById("<%=drpFileFormat.ClientID%>").value;
            status = document.getElementById("<%=selStatus.ClientID%>").value;
            isgroup = document.getElementById("<%=drpgroup.ClientID%>").value;
            isgroupmember = document.getElementById("<%=chkType.ClientID%>").checked;
            serverip = document.getElementById("<%=ddServerName.ClientID%>").value;
            authuserid = document.getElementById("<%=hid_userid.ClientID%>").value;
            authpassword = document.getElementById("<%=txtpwd.ClientID%>").value;
            locationcode = document.getElementById("<%=txtLocationCode.ClientID%>").value;
            qbn = document.getElementById("<%=txtQBN.ClientID%>").value;
            IsPrismView = document.getElementById("<%=chkIsPrismView.ClientID%>").checked;
            IsUnDeleteTags = document.getElementById("<%=chkIsUnDeleteTags.ClientID%>").checked;
            IsUnDeleteMonitors = document.getElementById("<%=chkIsUnDeleteMonitors.ClientID%>").checked;
            TimeZone = document.getElementById("selTimeZone").value;
            IsDefinedTagsinCore = document.getElementById("<%=chkIsDefinedTagsinCore.ClientID%>").checked;
            IsDefinedInfrastructureinCore = document.getElementById("<%=chkIsDefinedInfrastructureinCore.ClientID%>").checked;

            if (IsAdd == "1") {

                if (!Validate()) return false;

                try {
                    PageVisitDetails(g_UserId, "Site List", enumPageAction.Add, "New Site Added [" + sitename + "]");
                }
                catch (e) {

                }
            }
            else {

                if (!EditValidate()) return false;

                SiteDataId = curSiteid;

                try {
                    PageVisitDetails(g_UserId, "Site List", enumPageAction.Edit, "Edit site profile [" + sitename + "]");
                }
                catch (e) {

                }
            }

            SiteDataId = document.getElementById("<%=hdnSiteDataId.ClientID%>").value;

            if (SiteDataId == "")
                IsAdd = "1";
            else
                IsAdd = "2";

            AddSiteSetup(masterid, SiteDataId, IsAdd, companyid, sitename, sitefolder, fileformat, ConvertBooltoInt(status), isgroup, isgroupmember,
                        serverip, authpassword, locationcode, qbn, ConvertBooltoInt(IsPrismView), TimeZone, ConvertBooltoInt(IsUnDeleteTags), ConvertBooltoInt(IsUnDeleteMonitors),
                        ConvertBooltoInt(IsDefinedTagsinCore), ConvertBooltoInt(IsDefinedInfrastructureinCore));
        }

        function ConvertBooltoInt(boolval) {

            var intval = 0;

            if (boolval)
                intval = 1;

            return intval;
        }

        function doAfterAddedSite() {

            window.location = "SiteList.aspx";
        }

        function clearCtrlData() {

            document.getElementById("<%=hdnSiteDataId.ClientID%>").value = "";
            document.getElementById("<%=drpMaster.ClientID%>").selectedIndex = 0;
            document.getElementById("<%=selCompany.ClientID%>").selectedIndex = 0;
            document.getElementById("<%=txtSiteName.ClientID%>").value = "";
            document.getElementById("<%=txtSiteFolder.ClientID%>").value = "";
            document.getElementById("<%=drpFileFormat.ClientID%>").selectedIndex = 2;
            document.getElementById("<%=selStatus.ClientID%>").selectedIndex = 0;
            document.getElementById("<%=drpgroup.ClientID%>").selectedIndex = 0;
            document.getElementById("<%=chkType.ClientID%>").checked = false;
            document.getElementById("<%=ddServerName.ClientID%>").selectedIndex = 0;
            document.getElementById("<%=txtpwd.ClientID%>").value = "";
            document.getElementById("<%=txtLocationCode.ClientID%>").value = "";
            document.getElementById("<%=txtQBN.ClientID%>").value = "";
            document.getElementById("<%=chkIsPrismView.ClientID%>").checked = false;
            
            document.getElementById("<%=chkIsUnDeleteTags.ClientID%>").checked = false;
            document.getElementById("<%=chkIsUnDeleteMonitors.ClientID%>").checked = false;
            document.getElementById("selTimeZone").value = "Central Standard Time";

            document.getElementById("<%=chkIsDefinedTagsinCore.ClientID%>").checked = false;
            document.getElementById("<%=chkIsDefinedInfrastructureinCore.ClientID%>").checked = false;
        }

        function showTip3(text, lf, tp) {
            var elementRef = document.getElementById('tooltip3');
            elementRef.style.position = 'absolute';
            elementRef.innerHTML = text;
            elementRef.style.display = '';
            tempX = lf;
            tempY = tp;
            $("#tooltip3").css({ left: tempX, top: tempY })
        }

        function hideTip3() {
            var elementRef = document.getElementById('tooltip3');
            elementRef.style.display = 'none';
        }

        function deleteConfiguration(siteId) {
            if (confirm("Are you sure do you want to delete this site?") == true) {
                var AuthUserId = document.getElementById("<%=hid_userid.ClientID%>").value;
                DeleteConfigurationSetting(siteId, AuthUserId);
            }
        }

        function showSiteConfigurationForEdit(SiteDataId, SiteName, Status, ServerIP, SiteFolder, LocationCode,
                                              FileFormat, IsGroup, IsGroupMember, QBN, CompanyId, IsPrismView, TimeZone,
                                              IsUnDeleteTags, IsUnDeleteMonitors, DefinedTagsinCore, DefinedInfrastructureinCore) {

            document.getElementById("tdError").innerHTML = "";
            document.getElementById("<%=ddServerName.ClientID%>").style.display = 'none';
            document.getElementById("<%=ddEditServerName.ClientID%>").style.display = '';

            clearCtrlData();
            curSiteid = SiteDataId;

            document.getElementById("<%=ddEditServerName.ClientID%>").disabled = true;
            document.getElementById("<%=chkType.ClientID%>").disabled = true;

            document.getElementById("<%=btnSave.ClientID%>").style.display = 'none';
            document.getElementById("<%=btnUpdate.ClientID%>").style.display = '';
            document.getElementById("<%=trIsLinked.ClientID%>").style.display = '';
            document.getElementById("<%=trIsGroup.ClientID%>").style.display = '';
            document.getElementById("<%=trStatus.ClientID%>").style.display = '';

            $('#tdAddSite').show('slide', { direction: 'left' }, 500);

            var element_CompanyId = document.getElementById("<%=selCompany.ClientID%>");
            element_CompanyId.value = CompanyId;

            if (IsGroupMember == "True")
                document.getElementById("<%=chkType.ClientID%>").checked = true;
            else
                document.getElementById("<%=chkType.ClientID%>").checked = false;

            var element_Status = document.getElementById("<%=selStatus.ClientID%>");

            if (Status == "Active")
                document.getElementById("<%=selStatus.ClientID%>").selectedIndex = 0;
            else
                document.getElementById("<%=selStatus.ClientID%>").selectedIndex = 1;

            var element_IsGroup = document.getElementById("<%=drpgroup.ClientID%>");

            if (IsGroup == "True")
                element_IsGroup.value = 1;
            else
                element_IsGroup.value = 0;

            if (IsPrismView == "True")
                document.getElementById("<%=chkIsPrismView.ClientID%>").checked = true;
            else
                document.getElementById("<%=chkIsPrismView.ClientID%>").checked = false;

            if (IsUnDeleteTags == "True")
                document.getElementById("<%=chkIsUnDeleteTags.ClientID%>").checked = true;
            else
                document.getElementById("<%=chkIsUnDeleteTags.ClientID%>").checked = false;

            if (IsUnDeleteMonitors == "True")
                document.getElementById("<%=chkIsUnDeleteMonitors.ClientID%>").checked = true;
            else
                document.getElementById("<%=chkIsUnDeleteMonitors.ClientID%>").checked = false;

            if (DefinedTagsinCore == "True")
                document.getElementById("<%=chkIsDefinedTagsinCore.ClientID%>").checked = true;
            else
                document.getElementById("<%=chkIsDefinedTagsinCore.ClientID%>").checked = false;

            if (DefinedInfrastructureinCore == "True")
                document.getElementById("<%=chkIsDefinedInfrastructureinCore.ClientID%>").checked = true;
            else
                document.getElementById("<%=chkIsDefinedInfrastructureinCore.ClientID%>").checked = false;

            document.getElementById("<%=txtSiteName.ClientID%>").value = decodeURIComponent(SiteName);
            document.getElementById("<%=txtSiteFolder.ClientID%>").value = decodeURIComponent(SiteFolder);

            var element_CompanyId = document.getElementById("<%=selCompany.ClientID%>");
            element_CompanyId.value = CompanyId;

            var element_FileFormat = document.getElementById("<%=drpFileFormat.ClientID%>");
            element_FileFormat.value = FileFormat;

            var element_ServerIP = document.getElementById("<%=ddEditServerName.ClientID%>");
            element_ServerIP.value = ServerIP;

            document.getElementById("<%=txtLocationCode.ClientID%>").value = decodeURIComponent(LocationCode);
            document.getElementById("<%=txtQBN.ClientID%>").value = decodeURIComponent(QBN);
            document.getElementById("selTimeZone").value = decodeURIComponent(TimeZone);

            document.getElementById("ctl00_ContentPlaceHolder1_hdnSiteDataId").value = SiteDataId;
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        }        
        
    </script>
    <div id="tooltip3" class="tool3" style="display: none;">
    </div>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="height: 20px;">
        </tr>
        <tr>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="3">
                            <table width="100%">
                                <tr>
                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow' onclick='Redirect();'>
                                        <a href="AdminSettings.aspx">
                                            <img src='images/Left-Arrow.png' title='Settings' border="0" /></a>
                                    </td>
                                    <td style='width: 15px;' valign="top">
                                    </td>
                                    <td align='left' valign="top" style="width: 581px;">
                                        <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                            <tr>
                                                <td align='left' class='SHeader1'>
                                                    Settings
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='left' class='subHeader_black'>
                                                    Manage Sites
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" style="width: 750px;">
                                        <input id="btnAdd" runat="server" class="clsButton" onclick="ShowAddSite();" type="button"
                                            value="Add" />
                                    </td>
                                    <td align="right">
                                        <input id="btnExport" runat="server" class="clsButton" onclick="LoadSiteData(1);"
                                            type="button" value="Export" />
                                    </td>
                                   
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="height: 5px;">
                        <td class="bordertop" valign="top" colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="tdAddSite" class="cssTable" style="display: none;">
                    <table cellpadding="3" cellspacing="5" border="0">
                        <tr>
                            <td class="clsLALabel" style="width: 120px;" align="left">
                                Company Name :
                            </td>
                            <td align="left">
                                <asp:dropdownlist class="cssDropDown" id="selCompany" name="selCompany" runat="server"
                                    width="203px">
                                </asp:dropdownlist>
                            </td>
                        </tr>
                        <tr id="trIsLinked" style="display: none" runat="server">
                            <td class="clsLALabel" style="width: 120px;" align="left">
                                Is Linked :
                            </td>
                            <td align="left">
                                <asp:checkbox id="chkType" runat="server" />
                            </td>
                        </tr>
                        <tr id="trMaster" style="display: none" runat="server">
                            <td class="clsLALabel" style="width: 120px;" align="left">
                                Master :
                            </td>
                            <td align="left">
                                <asp:dropdownlist class="cssDropDown" id="drpMaster" runat="server" width="203px">
                                </asp:dropdownlist>
                            </td>
                        </tr>
                        <tr id="trIsGroup" style="display: none" runat="server">
                            <td class="clsLALabel" style="width: 120px;" align="left">
                                Is Group :
                            </td>
                            <td align="left">
                                <asp:dropdownlist class="cssDropDown" id="drpgroup" runat="server">
                                    <asp:listitem text="Yes" value="1" />
                                    <asp:listitem text="No" value="0" />
                                </asp:dropdownlist>
                            </td>
                        </tr>
                        <tr>
                            <td class="clsLALabel" style="width: 120px;" align="left">
                                Site Name :
                            </td>
                            <td align="left">
                                <input class="cssTextbox" type="text" id="txtSiteName" name="txtSiteName" runat="server"
                                    style="width: 500px;" />&nbsp;<span class="clsErrorTxt">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="clsLALabel" style="width: 120px;" align="left">
                                Time Zone:
                            </td>
                            <td align="left">
                                <select id="selTimeZone" name="selTimeZone" style="width: 300px;" class="cssDropDown">
                                    <option value="Morocco Standard Time">(GMT) Casablanca</option>
                                    <option value="GMT Standard Time">(GMT) Greenwich Mean Time : Dublin, Edinburgh, Lisbon,
                                        London</option>
                                    <option value="Greenwich Standard Time">(GMT) Monrovia, Reykjavik</option>
                                    <option value="W. Europe Standard Time">(GMT+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm,
                                        Vienna</option>
                                    <option value="Central Europe Standard Time">(GMT+01:00) Belgrade, Bratislava, Budapest,
                                        Ljubljana, Prague</option>
                                    <option value="Romance Standard Time">(GMT+01:00) Brussels, Copenhagen, Madrid, Paris</option>
                                    <option value="Central European Standard Time">(GMT+01:00) Sarajevo, Skopje, Warsaw,
                                        Zagreb</option>
                                    <option value="W. Central Africa Standard Time">(GMT+01:00) West Central Africa</option>
                                    <option value="Jordan Standard Time">(GMT+02:00) Amman</option>
                                    <option value="GTB Standard Time">(GMT+02:00) Athens, Bucharest, Istanbul</option>
                                    <option value="Middle East Standard Time">(GMT+02:00) Beirut</option>
                                    <option value="Egypt Standard Time">(GMT+02:00) Cairo</option>
                                    <option value="South Africa Standard Time">(GMT+02:00) Harare, Pretoria</option>
                                    <option value="FLE Standard Time">(GMT+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn,
                                        Vilnius</option>
                                    <option value="Israel Standard Time">(GMT+02:00) Jerusalem</option>
                                    <option value="E. Europe Standard Time">(GMT+02:00) Minsk</option>
                                    <option value="Namibia Standard Time">(GMT+02:00) Windhoek</option>
                                    <option value="Arabic Standard Time">(GMT+03:00) Baghdad</option>
                                    <option value="Arab Standard Time">(GMT+03:00) Kuwait, Riyadh</option>
                                    <option value="Russian Standard Time">(GMT+03:00) Moscow, St. Petersburg, Volgograd</option>
                                    <option value="E. Africa Standard Time">(GMT+03:00) Nairobi</option>
                                    <option value="Georgian Standard Time">(GMT+03:00) Tbilisi</option>
                                    <option value="Iran Standard Time">(GMT+03:30) Tehran</option>
                                    <option value="Arabian Standard Time">(GMT+04:00) Abu Dhabi, Muscat</option>
                                    <option value="Azerbaijan Standard Time">(GMT+04:00) Baku</option>
                                    <option value="Mauritius Standard Time">(GMT+04:00) Port Louis</option>
                                    <option value="Caucasus Standard Time">(GMT+04:00) Yerevan</option>
                                    <option value="Afghanistan Standard Time">(GMT+04:30) Kabul</option>
                                    <option value="Ekaterinburg Standard Time">(GMT+05:00) Ekaterinburg</option>
                                    <option value="Pakistan Standard Time">(GMT+05:00) Islamabad, Karachi</option>
                                    <option value="West Asia Standard Time">(GMT+05:00) Tashkent</option>
                                    <option value="India Standard Time">(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi</option>
                                    <option value="Sri Lanka Standard Time">(GMT+05:30) Sri Jayawardenepura</option>
                                    <option value="Nepal Standard Time">(GMT+05:45) Kathmandu</option>
                                    <option value="N. Central Asia Standard Time">(GMT+06:00) Almaty, Novosibirsk</option>
                                    <option value="Central Asia Standard Time">(GMT+06:00) Astana, Dhaka</option>
                                    <option value="Myanmar Standard Time">(GMT+06:30) Yangon (Rangoon)</option>
                                    <option value="SE Asia Standard Time">(GMT+07:00) Bangkok, Hanoi, Jakarta</option>
                                    <option value="North Asia Standard Time">(GMT+07:00) Krasnoyarsk</option>
                                    <option value="China Standard Time">(GMT+08:00) Beijing, Chongqing, Hong Kong, Urumqi</option>
                                    <option value="North Asia East Standard Time">(GMT+08:00) Irkutsk, Ulaan Bataar</option>
                                    <option value="Singapore Standard Time">(GMT+08:00) Kuala Lumpur, Singapore</option>
                                    <option value="W. Australia Standard Time">(GMT+08:00) Perth</option>
                                    <option value="Taipei Standard Time">(GMT+08:00) Taipei</option>
                                    <option value="Tokyo Standard Time">(GMT+09:00) Osaka, Sapporo, Tokyo</option>
                                    <option value="Korea Standard Time">(GMT+09:00) Seoul</option>
                                    <option value="Yakutsk Standard Time">(GMT+09:00) Yakutsk</option>
                                    <option value="Cen. Australia Standard Time">(GMT+09:30) Adelaide</option>
                                    <option value="AUS Central Standard Time">(GMT+09:30) Darwin</option>
                                    <option value="E. Australia Standard Time">(GMT+10:00) Brisbane</option>
                                    <option value="AUS Eastern Standard Time">(GMT+10:00) Canberra, Melbourne, Sydney</option>
                                    <option value="West Pacific Standard Time">(GMT+10:00) Guam, Port Moresby</option>
                                    <option value="Tasmania Standard Time">(GMT+10:00) Hobart</option>
                                    <option value="Vladivostok Standard Time">(GMT+10:00) Vladivostok</option>
                                    <option value="Central Pacific Standard Time">(GMT+11:00) Magadan, Solomon Is., New
                                        Caledonia</option>
                                    <option value="New Zealand Standard Time">(GMT+12:00) Auckland, Wellington</option>
                                    <option value="Fiji Standard Time">(GMT+12:00) Fiji, Kamchatka, Marshall Is.</option>
                                    <option value="Tonga Standard Time">(GMT+13:00) Nuku'alofa</option>
                                    <option value="Azores Standard Time">(GMT-01:00) Azores</option>
                                    <option value="Cape Verde Standard Time">(GMT-01:00) Cape Verde Is.</option>
                                    <option value="Mid-Atlantic Standard Time">(GMT-02:00) Mid-Atlantic</option>
                                    <option value="E. South America Standard Time">(GMT-03:00) Brasilia</option>
                                    <option value="Argentina Standard Time">(GMT-03:00) Buenos Aires</option>
                                    <option value="SA Eastern Standard Time">(GMT-03:00) Georgetown</option>
                                    <option value="Greenland Standard Time">(GMT-03:00) Greenland</option>
                                    <option value="Montevideo Standard Time">(GMT-03:00) Montevideo</option>
                                    <option value="Newfoundland Standard Time">(GMT-03:30) Newfoundland</option>
                                    <option value="Atlantic Standard Time">(GMT-04:00) Atlantic Time (Canada)</option>
                                    <option value="SA Western Standard Time">(GMT-04:00) La Paz</option>
                                    <option value="Central Brazilian Standard Time">(GMT-04:00) Manaus</option>
                                    <option value="Pacific SA Standard Time">(GMT-04:00) Santiago</option>
                                    <option value="Venezuela Standard Time">(GMT-04:30) Caracas</option>
                                    <option value="SA Pacific Standard Time">(GMT-05:00) Bogota, Lima, Quito, Rio Branco</option>
                                    <option value="Eastern Standard Time">(GMT-05:00) Eastern Time (US & Canada)</option>
                                    <option value="US Eastern Standard Time">(GMT-05:00) Indiana (East)</option>
                                    <option value="Central America Standard Time">(GMT-06:00) Central America</option>
                                    <option value="Central Standard Time" selected="selected">(GMT-06:00) Central Time (US
                                        & Canada)</option>
                                    <option value="Central Standard Time (Mexico)">(GMT-06:00) Guadalajara, Mexico City,
                                        Monterrey</option>
                                    <option value="Canada Central Standard Time">(GMT-06:00) Saskatchewan</option>
                                    <option value="US Mountain Standard Time">(GMT-07:00) Arizona</option>
                                    <option value="Mountain Standard Time (Mexico)">(GMT-07:00) Chihuahua, La Paz, Mazatlan</option>
                                    <option value="Mountain Standard Time">(GMT-07:00) Mountain Time (US & Canada)</option>
                                    <option value="Pacific Standard Time">(GMT-08:00) Pacific Time (US & Canada)</option>
                                    <option value="Pacific Standard Time (Mexico)">(GMT-08:00) Tijuana, Baja California</option>
                                    <option value="Alaskan Standard Time">(GMT-09:00) Alaska</option>
                                    <option value="Hawaiian Standard Time">(GMT-10:00) Hawaii</option>
                                    <option value="Samoa Standard Time">(GMT-11:00) Midway Island, Samoa</option>
                                    <option value="Dateline Standard Time">(GMT-12:00) International Date Line West</option>
                                </select>
                                &nbsp;<span class="clsErrorTxt">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="clsLALabel" style="width: 120px;" align="left">
                                Site Folder Name :
                            </td>
                            <td align="left">
                                <input class="cssTextbox" type="text" id="txtSiteFolder" name="txtSiteFolder" runat="server"
                                    style="width: 200px;" />&nbsp;<span class="clsErrorTxt">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="clsLALabel" style="width: 120px;" align="left">
                                Site Type :
                            </td>
                            <td align="left">
                                <asp:dropdownlist class="cssDropDown" id="drpFileFormat" runat="server">
                                    <asp:listitem selected="True">2X</asp:listitem>
                                    <asp:listitem>3X</asp:listitem>
                                </asp:dropdownlist>
                            </td>
                        </tr>
                        <tr id="trStatus" style="display: none" runat="server">
                            <td class="clsLALabel" style="width: 120px;" align="left">
                                Status :
                            </td>
                            <td align="left">
                                <select id="selStatus" class="cssDropDown" name="selStatus" runat="server">
                                    <option value="1" selected="selected">Active</option>
                                    <option value="0">Inactive</option>
                                </select>
                            </td>
                        </tr>
                        <tr id="trservername">
                            <td class="clsLALabel" style="width: 120px;" align="left">
                                Server Name :
                            </td>
                            <td align="left">
                                <asp:dropdownlist class="cssDropDown" id="ddServerName" runat="server" width="360px">
                                </asp:dropdownlist>
                                <asp:dropdownlist class="cssDropDown" id="ddEditServerName" runat="server" width="360px">
                                </asp:dropdownlist>
                                &nbsp;<span class="clsErrorTxt">*</span>
                            </td>
                        </tr>
                                                    <tr>
                                                        <td class="clsLALabel" style="width: 120px;" align="left">
                                                            Location Code :
                                                        </td>
                                                        <td align="left">
                                                            <input class="cssTextbox" type="text" style="width: 200px;" id="txtLocationCode"
                                                                name="txtLocationCode" runat="server" />&nbsp
                                                        </td>
                                                    </tr>
                        <tr id="trQBN">
                            <td class="clsLALabel" style="width: 120px;" align="left">
                                QBN :
                            </td>
                            <td align="left">
                                <input class="cssTextbox" type="text" id="txtQBN" name="txtQBN" runat="server" style="width: 200px;" />&nbsp
                            </td>
                        </tr>
                        <tr>
                            <td class="clsLALabel" style="width: 120px;" align="left">
                                Password :
                            </td>
                            <td align="left">
                                <input class="cssTextbox" type="password" id="txtpwd" name="txtpwd" runat="server"
                                    style="width: 200px;" />&nbsp;<span class="clsLALabel">(For User Verification)</span>&nbsp;<span
                                        class="clsErrorTxt">*</span>
                            </td>
                        </tr>
                        <tr id="trIsPrismView">
                            <td class="clsLALabel" style="width: 120px;" align="left">
                                Is Prism View:
                            </td>
                            <td align="left">
                                <asp:checkbox id="chkIsPrismView" runat="server" />
                            </td>
                        </tr>
                        <tr id="trPermissionhdr">
                            <td class="report_text" colspan="2">
                                Special Permissions
                            </td>
                        </tr>
                        <tr id="trSplPermissions">
                            <td style="width: 300px;" class="clsLALabel" colspan="2">
                                <table cellpadding="3" cellspacing="5" border="0">
                                    <tr>
                                        <td>
                                            Do not allow automatic recreation of tags:
                                        </td>
                                        <td>
                                            <asp:checkbox id="chkIsUnDeleteTags" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Do not allow automatic recreation of monitors:
                                        </td>
                                        <td>
                                            <asp:checkbox id="chkIsUnDeleteMonitors" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Only create and automatically recreate tags if tag is defined in Core:
                                        </td>
                                        <td>
                                            <asp:checkbox id="chkIsDefinedTagsinCore" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Only create and automatically recreate monitors if monitor is defined in Core:
                                        </td>
                                        <td>
                                            <asp:checkbox id="chkIsDefinedInfrastructureinCore" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                            <td align="left" style="height: 24px">
                                <input type="button" value="Save" style="width: 100px; height: 30px;" class="clsExportExcel"
                                    name="btnSave" id="btnSave" runat="server" onclick="CallSiteSetup(1);" />
                                <input type="button" value="Update" style="width: 110px; height: 30px;" class="clsExportExcel"
                                    name="btnUpdate" id="btnUpdate" runat="server" onclick="CallSiteSetup(2)" />
                                <input type="button" id="btnCancel" value="Cancel" style="width: 110px; height: 30px;"
                                    class="clsExportExcel" onclick="HideAddSite();" />
                                <input type="hidden" name="hdnSiteDataId" id="hdnSiteDataId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left" style="padding-left: 5px;">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4" id="tdError" class="clsMapErrorTxt">
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr style="height: 15px;">
        </tr>
        <tr>
            <td valign="top">
                <table id="tblSiteListInfo" cellspacing="1" cellpadding="1" style="width: 100%; padding-top: 10px;"
                    class="display">
                </table>
            </td>
        </tr>
    </table>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divUpdate">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divLoading">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
    <input type="hidden" id="hid_userrole" runat="server" />
    <input type="hidden" id="hid_pwd" runat="server" />
    <input type="hidden" id="hid_userid" runat="server" />
</asp:content>
