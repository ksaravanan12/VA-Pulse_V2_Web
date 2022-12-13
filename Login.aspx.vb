Imports System
Imports System.Data
Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Imports System.DirectoryServices

Imports System.DirectoryServices.AccountManagement
Imports System.Text.RegularExpressions

Imports System.Management.Automation
Imports System.Management.Automation.Host
Imports System.Management.Automation.Provider
Imports System.Management.Automation.Runspaces
Imports System.Collections.ObjectModel

Namespace GMSUI
    Partial Class Login
       
        Inherits System.Web.UI.Page
        
        Dim attemptcnt As Integer = 0
        Dim configattemptcnt As Integer = 0

        Dim API As New GMSAPI_New.GMSAPI_New
        Dim sStrRXml As XmlNode

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Session.Clear()

            txtPassword.Attributes.Add("type", "password")

            Try

                'Check AD User
                ChkADGroupUser("")

            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" ERROR Login Page Load : " & ex.Message)
            End Try

        End Sub

        Private Function isKeyExists(ByVal nKey) As Boolean

            Dim result As Boolean = False
            Dim i As Integer

            Try

                For i = 0 To Request.Headers.AllKeys.Length - 1
                    If Request.Headers.GetKey(i).ToUpper = nKey.ToString.ToUpper Then
                        result = True
                        Exit For
                    End If
                Next

            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" ERROR isKeyExists : " & ex.Message)
            End Try

            Return result
        End Function

        Public Shared Function GetADUserByPowerShell(ByVal serverip As String, ByVal findname As String, ByRef bResult As Boolean) As List(Of ADUserModel)

            Dim lstADUsers As New List(Of ADUserModel)()
            Dim dstname As String = ""
            Dim GroupName As String = ""
            Dim samaccountname As String = ""

            Try
                General.WriteADLog("domain : " & serverip & ", username: " & findname)

                ' create Powershell runspace
                Dim runspace As Runspace = RunspaceFactory.CreateRunspace()

                ' open it
                runspace.Open()

                ' create a pipeline and feed it the script text
                Dim pipeline As Pipeline = runspace.CreatePipeline()

                pipeline.Commands.AddScript("import-module ActiveDirectory")

                'Old - ADSAMACCOUNTNAME use
                'Dim adgetscript As String = "Get-ADUser -server " & serverip & " -filter {samaccountname -eq '" & findname & "'} -Properties MemberOf | Select-Object SamAccountName, @{Label = 'MemberOf'; Expression = {($_.MemberOf | ForEach-Object {([regex]""CN=(.*?),"").match($_).Groups[1].Value}) -join "",""}} | Format-List"

                'New - employeenumber use
                Dim adgetscript As String = "Get-ADUser -server " & serverip & " -filter {employeenumber -eq '" & findname & "'} -Properties MemberOf | Select-Object SamAccountName, @{Label = 'MemberOf'; Expression = {($_.MemberOf | ForEach-Object {([regex]""CN=(.*?),"").match($_).Groups[1].Value}) -join "",""}} | Format-List"

                pipeline.Commands.AddScript(adgetscript)

                General.WriteADLog(Convert.ToString("GET Script:") & adgetscript)
                ' add an extra command to transform the script output objects into nicely formatted strings
                ' remove this line to get the actual objects that the script returns. For example, the script
                ' "Get-Process" returns a collection of System.Diagnostics.Process instances.
                pipeline.Commands.Add("Out-String")

                ' execute the script
                Dim results As Collection(Of PSObject) = pipeline.Invoke()

                ' close the runspace
                runspace.Close()

                ' convert the script result into a single string
                For Each result As PSObject In results
                    General.WriteADLog("result" + result.ToString())

                    lstADUsers = GetGroupName(findname, result.ToString())
                Next

            Catch ex As Exception
                bResult = False
                General.WriteADLog("error: " + ex.Message)
            End Try

            General.WriteADLog("Total Count: " + lstADUsers.Count.ToString())

            '            Dim sitm As String = "SamAccountName : vhaisahatanm" & _
            '"MemberOf       : VHAMTZ RTLS Staff,VHAFRE RTLS Equipment Mgr,VHAMTZ RTLS Equipme" & _
            '"                 nt Mgr,VHAMAC RTLS Equipment Mgr,VHAREN RTLS Equipment Mgr,VHAS" & _
            '"                 FC RTLS Equipment Mgr,VHAPAL RTLS Equipment Mgr,VHAMPD RTLS Equ" & _
            '"                 ipment Mgr,VHAOKL_RTLS Equipment Mgr,VHAOKL RTLS Staff,VHAOKL R" & _
            '"                 TLS Equipment Mgr,VHAALX RTLS Equipment Mgr,VHAALX RTLS Staff,V" & _
            '"                 HACMP RTLS Equipment Mgr,VHACMD RTLS Equipment Mgr,VHACMB RTLS " & _
            '"                 Equipment Mgr,VHACMC RTLS Equipment Mgr,VHACMM RTLS Equipment M" & _
            '"                 gr,VHALIV RTLS Equipment Mgr,VHAOKL RTLS CL Staff,VHAJAC RTLS C" & _
            '"                 L Staff,VHABOS RTLS Staff,VHABHS RTLS Staff,VHAWHC RTLS Equipme" & _
            '"                 nt Mgr,VHAWHC RTLS Staff,VHANCT RTLS Equipment Mgr,VHANCT RTLS " & _
            '"                 Staff,VHATOG RTLS Equipment Mgr,VHATOG RTLS Staff,VHANHM RTLS E" & _
            '"                 quipment Mgr,VHANHM RTLS Staff,VHAWRJ RTLS Equipment Mgr,VHAWRJ" & _
            '"                  RTLS Staff,VHABED RTLS Equipment Mgr,VHABED RTLS Staff,VHAMAN" & _
            '"                 RTLS Equipment Mgr,VHAMAN RTLS Staff,VHAPRO RTLS Equipment Mgr," & _
            '"                 VHAPRO RTLS Staff,VHAPRO RTLS SPW Staff,VHABOS RTLS Equipment M" & _
            '"                gr,VHABHS RTLS Equipment Mgr,VHABRK RTLS Equipment Mgr,VHABRK R" & _
            '"                 TLS Staff,VHABRK RTLS SPW Inventory Support,VHAHOU RTLS CL Staf" & _
            '"                 f,VHACMM RTLS Staff,VHAFAR RTLS Staff,VHACMB RTLS Staff,RTLS_ND" & _
            '"                 RDWtest,VHACMD RTLS Staff,VHACMC RTLS Staff,VAISAAllUsers,VHACM" & _
            '"                 P RTLS Staff,VA OIT Field Office Citrix Access Gateway User,VHA" & _
            '"                 ISH_Maximo_Dev_Users,VA IT ICS Albany Contractors,VHAMASTERDoma" & _
            '"                 inUser01,VHACitrixCAGAccess01,VHAISP Jazz_Users,VHAISP RM_Users" & _
            '"                 ,VHAISP CCM_Users,VHAISP RQM_Users,VHAVINCI_Users,CDW_Full,VHAI" & _
            '"                 SP Rational Web Clients,VA OIT PD PPO TSPR Users,VHAFRE RTLS St" & _
            '"                 aff,VHALIV RTLS Staff,VINCI_Outage"

            '            lstADUsers = GetGroupName(findname, sitm.ToString())

            Return lstADUsers

        End Function

        Public Shared Function GetGroupName(ByVal searrchname As String, Optional ByVal resulstr As String = "") As List(Of ADUserModel)

            Dim lstADUsers As New List(Of ADUserModel)()
            Dim separator As String = ""

            Try

                If Len(resulstr) > 0 Then

                    separator = "MemberOf"
                    Dim membersplitlst As String() = resulstr.Split(New String() {separator}, StringSplitOptions.None)

                    If membersplitlst.Length > 1 Then

                        Dim memberofcontent_temp As String = membersplitlst(1).Replace("       : ", "")
                        Dim memberofcontent As String = memberofcontent_temp.Replace("                 ", "")

                        separator = ","
                        Dim cnsplitlst As String() = memberofcontent.Split(New String() {separator}, StringSplitOptions.None)

                        For gidx As Integer = 0 To cnsplitlst.Length - 1
                            Dim sitm As String = cnsplitlst(gidx)

                            Dim admdl As New ADUserModel()
                            admdl.SamAcountName = searrchname

                            sitm = sitm.Replace(vbCrLf, "")

                            admdl.Group = sitm
                            lstADUsers.Add(admdl)

                            General.WriteADLog("searrchname " & searrchname & ", Group Name " & sitm)
                        Next

                    End If
                End If
            Catch ex As Exception
                General.WriteADLog("GetGroupName: " + ex.Message)
            End Try

            Return lstADUsers
        End Function

        Public Function AuthenticateActiveDirectoryUser(ByVal username As String, ByVal password As String, ByVal ADServerIp As String) As Boolean

            Dim loginsuccess As Boolean = False

            Try

                Using pc As New System.DirectoryServices.AccountManagement.PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain, ADServerIp)
                    loginsuccess = pc.ValidateCredentials(username, password)
                End Using

            Catch ex As Exception
                WriteLog(" AuthenticateActiveDirectoryUser ERROR : " & ex.Message)
            End Try

            Return loginsuccess

        End Function

        '*************************************************************************************************************************************************************************'
        ' Function Name : Login                                                                                                                                                   '
        ' Input         : Username and Paswword                                                                                                                                   '
        ' Output        : XML Data                                                                                                                                                '
        ' Description   : (API -UserAuthentication) Used to login. It will return XML data with                                                                                   '
        '                 (Response and API Access Key and Associated Sites and User role and User Type)                                                                          '
        '*************************************************************************************************************************************************************************'
        Protected Sub btnLogin_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.ServerClick

            Dim sUsername, sPwd As String
            Dim sStrXml As XmlNode

            Dim smUser As String = ""
            Dim coll As New NameValueCollection
            Dim lstADUsers As New List(Of ADUserModel)

            sUsername = txtUsername.Value
            sPwd = txtPassword.Value

            Try

                sStrXml = API.UserAuthentication(sUsername, sPwd)

                Dim responsefromServer As String = sStrXml.Item("Response").InnerText

                If responsefromServer.ToLower() = "true" Then

                    Dim ApiKey As String = sStrXml.Item("AuthenticationKey").InnerText
                    Dim username As String = sStrXml.Item("Username").InnerText
                    Dim usertype As String = sStrXml.Item("usertype").InnerText
                    Dim UserID As String = sStrXml.Item("UserID").InnerText
                    Dim UserRole As String = sStrXml.Item("UserRoleId").InnerText
                    Dim userviews As String = sStrXml.Item("UserViews").InnerText
                    Dim UserCompanyId As String = sStrXml.Item("UserCompanyId").InnerText
                    Dim email As String = sStrXml.Item("Email").InnerText

                    g_IsADUser = 0

                    WriteLog(" UserName : " & username & " UserType : " & usertype & " UserRole : " & UserRole & "")

                    CreateAuthToken() 'Session Fixation Prevention

                    g_UserRole = UserRole
                    g_UserName = UserName
                    g_TypeId = UserType
                    g_UserAPI = ApiKey
                    g_UserId = UserID
                    g_UserViews = UserViews
                    g_UserCompanyId = UserCompanyId
                    g_UserEmail = email
                    g_IsTempMonitoring = sStrXml.Item("IsTempMonitoring").InnerText
                    g_AllowAccessForStar = sStrXml.Item("AllowAccessForStar").InnerText
                    g_AllowAccessForKPI = sStrXml.Item("IsAllowAccessForKPI").InnerText
                    g_IsPulseReport = sStrXml.Item("IsPulseReport").InnerText

                    Response.Redirect("Home.aspx")

                Else

                    Dim sADServerIP As String = ""
                    Dim sADUserName As String = ""
                    Dim sADPassword As String = ""

                    'Get AD Directory Info
                    GetADServer(sADServerIP, sADUserName, sADPassword)

                    Dim isLoginSuccess As Boolean = AuthenticateActiveDirectoryUser(sUsername, sPwd, sADServerIP)

                    If isLoginSuccess = True Then

                        ChkADGroupUser(sUsername)
                        CreateAuthToken() 'Session Fixation Prevention

                    Else

                        tdAlert.InnerHtml = responsefromServer
                        tdAlert.Style("display") = ""
                        txtPassword.Attributes.Add("type", "password")
                        tdAlert.Attributes.Add("class", "login-alertBox")
                        txtUsername.Value = ""
                        txtPassword.Value = ""

                    End If

                End If

            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" Login : " & ex.Message.ToString)
            End Try

        End Sub

        Protected Sub btnForgotPassword_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnForgotPassword.ServerClick

            Dim sUsername, sPwd, Response As String

            Dim sXmlNode As XmlNode

            sUsername = txtUsername.Value
            sPwd = txtPassword.Value

            Try
	    
                sXmlNode = GetForgotPassword(sUsername)

                Response = sXmlNode.Item("Response").InnerText

                tdAlert.InnerHtml = Response
                tdAlert.Style("display") = ""

                If Response.ToString.Contains("sent to your email") Then
                    tdAlert.Attributes.Add("class", "login-alertBoxSuccess")
                Else
                    tdAlert.Attributes.Add("class", "login-alertBox")
                End If

                txtPassword.Attributes.Add("type", "password")
                txtUsername.Value = ""
                txtPassword.Value = ""

            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" Login : " & ex.Message.ToString)
            End Try

        End Sub

        Public Sub GetADServer(ByRef sADServerIP As String, ByRef sADUserName As String, ByRef sADPassword As String)

            Dim dtADServer As New DataTable
            Dim dsAD As New DataSet

            Try

                'Get Active Directory Info
                sStrRXml = API.GetADDirectory()
                dsAD.ReadXml(New XmlNodeReader(sStrRXml))

                If Not dsAD Is Nothing Then
                    If dsAD.Tables.Count <> 0 Then

                        dtADServer = dsAD.Tables(0)

                        If Not dtADServer Is Nothing Then

                            If dtADServer.Rows.Count > 0 Then
                                sADServerIP = CheckIsDBNull(dtADServer.Rows(0).Item("ServerIp"), , "")
                                sADUserName = CheckIsDBNull(dtADServer.Rows(0).Item("UserName"), , "")
                                sADPassword = CheckIsDBNull(dtADServer.Rows(0).Item("Pwd"), , "")
                            Else
                                WriteADLog("AD Server information not configure in GMS.")
                            End If

                        End If

                    End If
                End If

            Catch ex As Exception
                WriteLog(" GetADServer ERROR : " & ex.Message.ToString)
            End Try

        End Sub

        Public Sub ChkADGroupUser(ByVal UserName As String)

            Dim dt As New DataTable
            Dim dtRole As New DataTable
            Dim ds As New DataSet

            Dim EnableActiveDirectoryRoles As Boolean = False
            Dim SSOIAttribute As String = ""
            Dim HeaderValue As String = ""
            Dim arrRoles() As String
            Dim RoleId As Integer = 0
            Dim bIsNoRoleMap As Boolean = False
            Dim sADServerIP As String = ""
            Dim sADUserName As String = ""
            Dim sADPassword As String = ""
            Dim GroupName As String = ""
            Dim strGroupName As String = ""
            Dim arrGroup() As String
            Dim ADUser As String = ""
            Dim UserRoleName As String = ""
            Dim bResult As Boolean = False
            Dim bADResult As Boolean = True

            Dim lstADUsers As New List(Of ADUserModel)
            Dim dtADServer As New DataTable

            Dim VACompanys As String = ""
            Dim VASites As String = ""

            Dim sVACompany() As String
            Dim sVASite() As String

            Dim strCompanys As String = ""
            Dim strSites As String = ""

            Try

                WriteADLog("-------------------------------------------------------------------------------------------------------------------------------------------")
                WriteADLog("                                            SSOI Header Section START                                                                      ")
                WriteADLog("-------------------------------------------------------------------------------------------------------------------------------------------")

                For i = 0 To Request.Headers.AllKeys.Length - 1
                    WriteADLog(" Index  : " & i & " Key : " & Request.Headers.GetKey(i).ToUpper & " Value : " & Request.Headers(Request.Headers.GetKey(i).ToString).ToString)
                Next

                WriteADLog("-------------------------------------------------------------------------------------------------------------------------------------------")
                WriteADLog("                                            SSOI Header Section END                                                                        ")
                WriteADLog("-------------------------------------------------------------------------------------------------------------------------------------------")

                'Get AD Directory Server
                GetADServer(sADServerIP, sADUserName, sADPassword)

                'Get Manage Role Info
                sStrRXml = API.GetMangeRolesInfo()
                ds.ReadXml(New XmlNodeReader(sStrRXml))

                If Not ds Is Nothing Then
                    If ds.Tables.Count <> 0 Then

                        'Get SSOIAttribute
                        dt = ds.Tables(0)

                        'Get Roles
                        dtRole = ds.Tables(1)

                    End If
                End If

                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then

                        'Enable Active Rols
                        EnableActiveDirectoryRoles = dt.Rows(0).Item("EnableActiveDirectoryRoles")

                        If EnableActiveDirectoryRoles = True Then

                            ''SSOIAttribute
                            SSOIAttribute = dt.Rows(0).Item("SSOIAttribute")

                            'Get Header
                            HeaderValue = Request.Headers(SSOIAttribute)

                            If UserName = "" Then
                                UserName = HeaderValue
                            End If

                            'Find Header Value  
                            If isKeyExists(SSOIAttribute) Then
                                bResult = True
                            End If

                            If bResult = True Then
                                WriteADLog("Is User Key Found  : PASS")
                            Else
                                WriteADLog("Is User Key Found  : FAILED")
                            End If

                            WriteADLog("User Key : " & SSOIAttribute)
                            WriteADLog("User Name : " & UserName)

                            If bResult = False Then
                                WriteADLog("User Key not valid, You can check valid user key (refer to SSOI header section) and update the key value in GMS - Manage Role Mapping - SSOI Attribute for AD Role")
                                Response.Redirect("ErrorPage.aspx?ErrMsg=SSOI User key defined not valid.")
                            End If

                        End If

                    End If

                End If

                WriteADLog("Connect AD database")
                WriteADLog("AD Server : " & sADServerIP)
                WriteADLog("AD User   : " & sADUserName)
                WriteADLog("AD Password : XXXXX")

                'Get AD Group List
                lstADUsers = GetADUserByPowerShell(sADServerIP, UserName, bADResult)

                If bADResult = False Then
                    WriteADLog("AD DB connection failed, Please check user credential")
                    Response.Redirect("ErrorPage.aspx?ErrMsg=RTLS AD Server not connected.")
                Else
                    WriteADLog("AD DB connected sucessfully")
                End If

                If Not lstADUsers Is Nothing Then
                    If lstADUsers.Count > 0 Then

                        WriteADLog("Get AD GroupName By UserName :")

                        For nIdx As Integer = 0 To lstADUsers.Count - 1

                            GroupName = lstADUsers.Item(nIdx).Group
                            ADUser = lstADUsers.Item(nIdx).SamAcountName

                            If strGroupName.Trim <> "" Then
                                strGroupName = strGroupName & "," & GroupName
                            Else
                                strGroupName = GroupName
                            End If

                            WriteADLog(" Group Name :" & GroupName)

                        Next

                        If strGroupName.Trim <> "" Then
                            arrGroup = strGroupName.Split(",")

                            '--------------------------------------------------------------------------------AD Group Get Server List---------------------------------------------------------------------------------------'
                            If arrGroup.Length > 0 Then

                                For nCIdx As Integer = 0 To arrGroup.Length - 1
                                    If arrGroup(nCIdx).ToUpper.Contains("RTLS ADMIN") = True Then

                                        sVACompany = Replace(arrGroup(nCIdx), "_", " ").Split(" ")

                                        If sVACompany.Length > 0 Then
                                            VACompanys = sVACompany(0)
                                        End If

                                        If strCompanys = "" Then
                                            strCompanys = VACompanys
                                        Else
                                            strCompanys = strCompanys & "," & VACompanys
                                        End If

                                    End If
                                Next

                                ''VA Companys
                                If Len(strCompanys) > 0 Then
                                    WriteADLog("VA RTLS Admin Server List : " & strCompanys)
                                End If

                                '-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------'

                                '--------------------------------------------------------------------------------AD Group Get Site List-----------------------------------------------------------------------------------------'

                                For nSIdx As Integer = 0 To arrGroup.Length - 1
                                    If arrGroup(nSIdx).ToUpper.Contains("RTLS EQUIPMENT") = True Then

                                        sVASite = Replace(arrGroup(nSIdx), "_", " ").Split(" ")

                                        If sVASite.Length > 0 Then
                                            VASites = sVASite(0)
                                        End If

                                        If strSites = "" Then
                                            strSites = VASites
                                        Else
                                            strSites = strSites & "," & VASites
                                        End If

                                    End If
                                Next

                                'VA Sites
                                If Len(strSites) > 0 Then
                                    WriteADLog("VA RTLS Equipment Manager Site List : " & strSites)
                                End If
                                '----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------'

                            End If
                        End If
                    Else

                        WriteADLog("User not found in AD Group Or User Key not valid, You can check valid user key (refer to SSOI header section) and update the key value in GMS - Manage Role Mapping - SSOI Attribute for AD Role")

                        Response.Redirect("ErrorPage.aspx?ErrMsg=RTLS AD Group not available for the user.")
                    End If
                Else
                    Response.Redirect("ErrorPage.aspx?ErrMsg=RTLS AD Group not available for the user.")
                End If

                Session("VACompanys") = ""
                Session("VASites") = ""

                Dim sGroupName As String = ""
                Dim dRow() As DataRow

                ' GMS Config AD Role Groups
                If Not dtRole Is Nothing Then
                    If dtRole.Rows.Count > 0 Then

                        '-------------------------------------------------------------------Technical Admin-----------------------------------------------------------------------------------------'

                        ' Check GMS Config Technical Admin AD Groups
                        dRow = dtRole.Select("GMSRoleId=" & enumUserRole.TechnicalAdmin)

                        If dRow.Length > 0 Then

                            arrRoles = dRow(0).Item("RoleName").Split(",")

                            If Not arrGroup Is Nothing Then
                                If arrGroup.Length > 0 Then

                                    For nGIdx As Integer = 0 To arrGroup.Length - 1
                                        If Array.IndexOf(arrRoles, arrGroup(nGIdx)) > -1 Then ' AD Group Name Exists in GMS AD Groups

                                            RoleId = CheckIsDBNull(dRow(0).Item("GMSRoleId"), , 0)
                                            UserRoleName = "Technical Admin"
                                            sGroupName = arrGroup(nGIdx)

                                            '' Assign VISN Companys for session
                                            Session("VACompanys") = strCompanys

                                            Exit For
                                        End If
                                    Next

                                End If
                            End If

                        End If

                        '-------------------------------------------------------------------Maintenance OR Maintenance/Prism-----------------------------------------------------------------------------------------------'

                        If RoleId = 0 Then

                            ' Check GMS Config Maintenence OR Maintenance/Prism AD Groups

                            dRow = dtRole.Select("GMSRoleId='" & enumUserRole.MaintenancePrism & "'")

                            If dRow.Length = 0 Then
                                dRow = dtRole.Select("GMSRoleId='" & enumUserRole.Maintenance & "'")
                            End If
                           
                            If dRow.Length > 0 Then

                                arrRoles = dRow(0).Item("RoleName").Split(",")

                                If Not arrGroup Is Nothing Then
                                    If arrGroup.Length > 0 Then

                                        For nGIdx As Integer = 0 To arrGroup.Length - 1
                                            If Array.IndexOf(arrRoles, arrGroup(nGIdx)) > -1 Then ' AD Group Name Exists in GMS AD Groups

                                                sGroupName = arrGroup(nGIdx)
                                                RoleId = CheckIsDBNull(dRow(0).Item("GMSRoleId"), , 0)

                                                If RoleId = enumUserRole.MaintenancePrism Then
                                                    UserRoleName = "Maintenance/Prism"
                                                Else
                                                    UserRoleName = "Maintenance"
                                                End If

                                                '' Assign VISN Sites for session
                                                Session("VASites") = strSites

                                                Exit For
                                            End If
                                        Next

                                    End If
                                End If

                            End If

                        End If

                        If RoleId = 0 Then
                            If isKeyExists(SSOIAttribute) Then
                                WriteADLog("AD Group not found in GMS.")
                                Response.Redirect("ErrorPage.aspx?ErrMsg=User is not associated CenTrak AD groups. Please contact the administrator.")
                            Else
                                WriteADLog("AD Group not found in GMS.")
                                Response.Redirect("ErrorPage.aspx?ErrMsg=User role can’t be determined.")
                            End If
                        Else

                            WriteADLog("AD Group matching with GMS Role")
                            WriteADLog("AD Group Name: " & sGroupName)

                            Dim UName As String = Request.Headers("firstName") & " " & Request.Headers("lastName")

                            g_UserRole = RoleId

                            If Trim(UName) = "" Then
                                g_UserName = UserName
                            Else
                                g_UserName = UName
                            End If

                            g_TypeId = ""
                            g_UserAPI = VA_AuthenticationKey & RoleId
                            g_UserId = 0
                            g_IsADUser = 1

                            Try
                                g_UserEmail = Request.Headers("ADEMAIL")

                                WriteADLog("AD Email: " & g_UserEmail)
                            Catch ex As Exception
                                WriteLog(" ADEMAIL ERROR : " & ex.Message.ToString)
                            End Try

                            WriteADLog("GMS Role Name: " & UserRoleName)

                            WriteADLog("User login successfully")

                            'User Role Exists
                            Response.Redirect("Home.aspx")
                        End If

                    Else
                        Response.Redirect("ErrorPage.aspx?ErrMsg=User role can’t be determined.")
                    End If
                Else
                    Response.Redirect("ErrorPage.aspx?ErrMsg=User role can’t be determined.")
                End If

            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" ChkADGroupUser ERROR : " & ex.Message.ToString)
            End Try

        End Sub

    End Class

End Namespace
