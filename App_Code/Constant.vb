Imports Microsoft.VisualBasic
Imports System.IO
Namespace GMSUI
    Public Module Constant
        Public Const LOG_FILEPATH As String = "." 'Local and HH
        Public Const LOG_FILENAME As String = "Log.txt"

        Public Const LOGIN_SESSION_ERROR As String = "101"
        Public Const APIURL = "http://www.viewtrax.com/GMSAPINEW/GMSAPI_New.asmx"
        Public Enum ENUMLOG
            UISendTCPCommands = 1
            UIDatabaseWrite = 2
            WEBSERVICE_GETTags = 3
            WEBSERVICE_GETDevices = 4
            WEBSERVICE_GETSETTINGS = 5
            WEBSERVICE_GETTransports = 6
            WEBSERVICE_GETransfers = 7
            WINSERVICE_SendTCPCommands = 8
            WINSERVICE_SPCALLS = 9
            WINSERVICE_LOGS = 10
        End Enum
        Public Enum WPSLOGTYPE
            CONTROLLER = 1
            ALERT = 2
            TRANSPORT = 3
            UISETTINGS = 4
            SYSTEMSTATUS = 5
            UILOG = 6
        End Enum
    End Module
End Namespace

