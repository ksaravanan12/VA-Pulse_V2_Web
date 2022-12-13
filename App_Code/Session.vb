Imports Microsoft.VisualBasic
Imports System.Web.HttpContext
Imports System.data

Namespace GMSUI

    Public Module Session
    
        Private Const STR_SESSION_USERID As String = "UserId"
        Private Const STR_SESSION_USERNAME As String = "UserName"
        Private Const STR_SESSION_USERType As String = "Typeid"
        Private Const STR_SESSION_USERAPI As String = "UserAPI"
        Private Const STR_SESSION_SITELIST As String = "SITELIST"
        Private Const STR_SESSION_ALERTLIST As String = "ALERTLIST"
        Private Const STR_SESSION_GMSLASTUPDATE As String = "Gmslastupdate"
        Private Const STR_SESSION_USERROLE As String = "UserRole"       
        Private Const STR_SESSION_USERVIEWS As String = "UserViews"
        Private Const STR_SESSION_ISTEMPMONITORING As String = "TempMonitoring"
        Private Const STR_SESSION_COMPANYID As String = "CompanyId"
        Private Const STR_SESSION_ALLOWACCESSFORSTAR As String = "AccessStar"
        Private Const STR_SESSION_ALLOWACCESSFORKPI As String = "IsAllowAccessForKPI"
        Private Const STR_SESSION_USEREMAIL As String = ""
        Private Const STR_SESSION_ISADUSER As String = "0"
	Private Const STR_SESSION_PULSEREPORT As String = "IsPulseReport"
	
        '**********************************************************************************************************
        ' g_userID Property
        ' =================
        ' Description :  To Set and Get the Current UserId from the g_UserID Property
        '***********************************************************************************************************
        Public Property g_UserId() As Integer
	
            Get
                Return Current.Session(STR_SESSION_USERID)
            End Get
            Set(ByVal Value As Integer)
                Current.Session(STR_SESSION_USERID) = Value
            End Set
	    
        End Property
	
        '**********************************************************************************************************
        ' g_UserName Property
        ' =================
        ' Description :  To Set and Get the Current UserName from the g_UserName Property
        '***********************************************************************************************************
        Public Property g_UserName() As String
	
            Get
                Return Current.Session(STR_SESSION_USERNAME)
            End Get
            Set(ByVal Value As String)
                Current.Session(STR_SESSION_USERNAME) = Value
            End Set
	    
        End Property
	
        '**********************************************************************************************************
        ' g_TypeId Property
        ' =================
        ' Description :  To Set and Get the Current UserType from the g_TypeId Property
        '***********************************************************************************************************
        Public Property g_TypeId() As String
	
            Get
                Return Current.Session(STR_SESSION_USERType)
            End Get
            Set(ByVal Value As String)
                Current.Session(STR_SESSION_USERType) = Value
            End Set
	    
        End Property
	
        '**********************************************************************************************************
        ' g_UserAPI Property
        ' =================
        ' Description :  To Set and Get the Current User API from the g_UserAPI Property
        '***********************************************************************************************************
        Public Property g_UserAPI() As String
	
            Get
                Return Current.Session(STR_SESSION_USERAPI)
            End Get
            Set(ByVal Value As String)
                Current.Session(STR_SESSION_USERAPI) = Value
            End Set
	    
        End Property
	
        '**********************************************************************************************************
        ' g_sitelist Property
        ' =================
        ' Description :  To Set and Get the Current Site Summary from the g_sitelist Property
        '***********************************************************************************************************
        Public Property g_sitelist() As Object
	
            Get
                Return Current.Session(STR_SESSION_SITELIST)
            End Get
            Set(ByVal Value As Object)
                Current.Session(STR_SESSION_SITELIST) = Value
            End Set
	    
        End Property
	
        '**********************************************************************************************************
        ' g_Gmslastupdate Property
        ' =================
        ' Description :  To Set and Get the Current Site Last update from the g_Gmslastupdate Property
        '**********************************************************************************************************
        Public Property g_Gmslastupdate() As Object
	
            Get
                Return Current.Session(STR_SESSION_GMSLASTUPDATE)
            End Get
            Set(ByVal Value As Object)
                Current.Session(STR_SESSION_GMSLASTUPDATE) = Value
            End Set
	    
        End Property
	
        '**********************************************************************************************************
        ' g_alertlist Property
        ' =================
        ' Description :  To Set and Get the Current Site Summary from the g_alertlist Property
        '***********************************************************************************************************
        Public Property g_alertlist() As Object
	
            Get
                Return Current.Session(STR_SESSION_ALERTLIST)
            End Get
            Set(ByVal Value As Object)
                Current.Session(STR_SESSION_ALERTLIST) = Value
            End Set
	    
        End Property
	
        '**********************************************************************************************************
        ' g_UserRole Property
        ' =================
        ' Description :  To Set and Get the Current Site User Role from the g_UserRole Property
        '***********************************************************************************************************
        Public Property g_UserRole() As Integer
	
            Get
                Return Current.Session(STR_SESSION_USERROLE)
            End Get
            Set(ByVal Value As Integer)
                Current.Session(STR_SESSION_USERROLE) = Value
            End Set
	    
        End Property

        '**********************************************************************************************************
        ' g_UserViews Property
        ' =================
        ' Description :  To Set and Get the Current UserViews from the g_UserViews Property
        '***********************************************************************************************************
        Public Property g_UserViews() As String
	
            Get
                Return Current.Session(STR_SESSION_USERVIEWS)
            End Get
            Set(ByVal Value As String)
                Current.Session(STR_SESSION_USERVIEWS) = Value
            End Set
	    
        End Property

        '**********************************************************************************************************
        ' g_UserEmail Property
        ' =================
        ' Description :  To Set and Get the Current UserViews from the g_UserEmail Property
        '***********************************************************************************************************
        Public Property g_UserEmail() As String
	
            Get
                Return Current.Session(STR_SESSION_USEREMAIL)
            End Get
            Set(ByVal Value As String)
                Current.Session(STR_SESSION_USEREMAIL) = Value
            End Set
	    
        End Property

        '**********************************************************************************************************
        ' g_UserEmail Property
        ' =================
        ' Description :  To Set and Get the Current UserViews from the g_UserEmail Property
        '***********************************************************************************************************
        Public Property g_IsADUser() As String
	
            Get
                Return Current.Session(STR_SESSION_ISADUSER)
            End Get
            Set(ByVal Value As String)
                Current.Session(STR_SESSION_ISADUSER) = Value
            End Set
	    
        End Property
	
	'**********************************************************************************************************
        ' g_UserViews Property
        ' =================
        ' Description :  To Set and Get the Current UserViews from the g_UserViews Property
        '***********************************************************************************************************
        Public Property g_IsTempMonitoring() As String
	
            Get
                Return Current.Session(STR_SESSION_ISTEMPMONITORING)
            End Get
            Set(ByVal Value As String)
                Current.Session(STR_SESSION_ISTEMPMONITORING) = Value
            End Set
	    
        End Property
	
        '**********************************************************************************************************
        ' g_AllowAccessForStar Property
        ' =================
        ' Description :  To Set and Get the Current UserViews from the g_UserViews Property
        '***********************************************************************************************************
        Public Property g_AllowAccessForStar() As String
	
            Get
                Return Current.Session(STR_SESSION_ALLOWACCESSFORSTAR)
            End Get
            Set(ByVal Value As String)
                Current.Session(STR_SESSION_ALLOWACCESSFORSTAR) = Value
            End Set
	    
        End Property
	
	'**********************************************************************************************************
        ' g_UserCompanyId Property
        ' =================
        ' Description :  To Set and Get the Current User CompanyId from the g_UserCompanyId Property
        '***********************************************************************************************************
        Public Property g_UserCompanyId() As String
	
            Get
                Return Current.Session(STR_SESSION_COMPANYID)
            End Get
            Set(ByVal Value As String)
                Current.Session(STR_SESSION_COMPANYID) = Value
            End Set
	    
        End Property
	
	'**********************************************************************************************************
        ' g_AllowAccessForKPI Property
        ' =================
        ' Description :  To Set and Get the Current User Allow Access For KPI from the g_AllowAccessForKPI Property
        '***********************************************************************************************************
        Public Property g_AllowAccessForKPI() As String
	
            Get
                Return Current.Session(STR_SESSION_ALLOWACCESSFORKPI)
            End Get
            Set(ByVal Value As String)
                Current.Session(STR_SESSION_ALLOWACCESSFORKPI) = Value
            End Set
	    
        End Property

	'**********************************************************************************************************
        ' g_IsPulseReport Property
        ' =================
        ' Description :  To Set and Get the Current User Is Pulse Report from the g_IsPulseReport Property
        '***********************************************************************************************************
        Public Property g_IsPulseReport() As String
	
            Get
                Return Current.Session(STR_SESSION_PULSEREPORT)
            End Get
            Set(ByVal Value As String)
                Current.Session(STR_SESSION_PULSEREPORT) = Value
            End Set

        End Property
	
    End Module
    
End Namespace

