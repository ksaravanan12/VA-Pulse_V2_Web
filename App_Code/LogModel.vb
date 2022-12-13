Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Text
Imports System.Collections.Generic

Public Class LogModel
    Public Property Updatedon() As DateTime
        Get
            Return m_Updatedon
        End Get
        Set(value As DateTime)
            m_Updatedon = Value
        End Set
    End Property
    Private m_Updatedon As DateTime
    Public Property TypeId() As String
        Get
            Return m_TypeId
        End Get
        Set(value As String)
            m_TypeId = value
        End Set
    End Property
    Private m_TypeId As String
    Public Property Category() As String
        Get
            Return m_Category
        End Get
        Set(value As String)
            m_Category = value
        End Set
    End Property
    Private m_Category As String
    Public Property Log() As String
        Get
            Return m_Log
        End Get
        Set(value As String)
            m_Log = Value
        End Set
    End Property
    Private m_Log As String
End Class
Public Class ControllerLogModel
    Public Property SNO() As String
        Get
            Return m_SNO
        End Get
        Set(value As String)
            m_SNO = value
        End Set
    End Property
    Private m_SNO As String

    Public Property Controller() As String
        Get
            Return m_Controller
        End Get
        Set(value As String)
            m_Controller = value
        End Set
    End Property
    Private m_Controller As String

    Public Property DateTime() As DateTime
        Get
            Return m_DateTime
        End Get
        Set(value As DateTime)
            m_DateTime = value
        End Set
    End Property
    Private m_DateTime As String

    Public Property ControllerState() As String
        Get
            Return m_ControllerState
        End Get
        Set(value As String)
            m_ControllerState = value
        End Set
    End Property
    Private m_ControllerState As String

    Public Property TagInFields() As String
        Get
            Return m_TagInFields
        End Get
        Set(value As String)
            m_TagInFields = value
        End Set
    End Property
    Private m_TagInFields As String

    Public Property MagLock() As String
        Get
            Return m_MagLock
        End Get
        Set(value As String)
            m_MagLock = value
        End Set
    End Property
    Private m_MagLock As String

    Public Property DoorStatus() As String
        Get
            Return m_DoorStatus
        End Get
        Set(value As String)
            m_DoorStatus = value
        End Set
    End Property
    Private m_DoorStatus As String

    Public Property Buzzer() As String
        Get
            Return m_Buzzer
        End Get
        Set(value As String)
            m_Buzzer = value
        End Set
    End Property
    Private m_Buzzer As String

    Public Property Relay1() As String
        Get
            Return m_Relay1
        End Get
        Set(value As String)
            m_Relay1 = value
        End Set
    End Property
    Private m_Relay1 As String

    Public Property Relay2() As String
        Get
            Return m_Relay2
        End Get
        Set(value As String)
            m_Relay2 = value
        End Set
    End Property
    Private m_Relay2 As String

    Public Property Relay3() As String
        Get
            Return m_Relay3
        End Get
        Set(value As String)
            m_Relay3 = value
        End Set
    End Property
    Private m_Relay3 As String

    Public Property UIControllerState() As String
        Get
            Return m_UIControllerState
        End Get
        Set(value As String)
            m_UIControllerState = value
        End Set
    End Property
    Private m_UIControllerState As String

    Public Property UIAlert() As String
        Get
            Return m_UIAlert
        End Get
        Set(value As String)
            m_UIAlert = value
        End Set
    End Property
    Private m_UIAlert As String

    Public Property DoorAjar() As String
        Get
            Return m_DoorAjar
        End Get
        Set(value As String)
            m_DoorAjar = value
        End Set
    End Property
    Private m_DoorAjar As String

    Public Property FireAlarmIO() As String
        Get
            Return m_FireAlarmIO
        End Get
        Set(value As String)
            m_FireAlarmIO = value
        End Set
    End Property
    Private m_FireAlarmIO As String

    Public Property EmergencyDoorIO() As String
        Get
            Return m_EmergencyDoorIO
        End Get
        Set(value As String)
            m_EmergencyDoorIO = value
        End Set
    End Property
    Private m_EmergencyDoorIO As String

    Public Property GeneralInput1IO() As String
        Get
            Return m_GeneralInput1IO
        End Get
        Set(value As String)
            m_GeneralInput1IO = value
        End Set
    End Property
    Private m_GeneralInput1IO As String

    Public Property GeneralInput2IO() As String
        Get
            Return m_GeneralInput2IO
        End Get
        Set(value As String)
            m_GeneralInput2IO = value
        End Set
    End Property
    Private m_GeneralInput2IO As String

    Public Property KeyPad1State() As String
        Get
            Return m_KeyPad1State
        End Get
        Set(value As String)
            m_KeyPad1State = value
        End Set
    End Property
    Private m_KeyPad1State As String

    Public Property KeyPad2State() As String
        Get
            Return m_KeyPad2State
        End Get
        Set(value As String)
            m_KeyPad2State = value
        End Set
    End Property
    Private m_KeyPad2State As String

    Public Property KeyCode() As String
        Get
            Return m_KeyCode
        End Get
        Set(value As String)
            m_KeyCode = value
        End Set
    End Property
    Private m_KeyCode As String

    Public Property Validation() As String
        Get
            Return m_Validation
        End Get
        Set(value As String)
            m_Validation = value
        End Set
    End Property
    Private m_Validation As String

    Public Property Message() As String
        Get
            Return m_Message
        End Get
        Set(value As String)
            m_Message = value
        End Set
    End Property
    Private m_Message As String

End Class
Public Class TATSModel
    Public Property IDX() As String
        Get
            Return m_IDX
        End Get
        Set(value As String)
            m_IDX = value
        End Set
    End Property
    Private m_IDX As String

    Public Property Tag() As Integer
        Get
            Return m_Tag
        End Get
        Set(value As Integer)
            m_Tag = value
        End Set
    End Property
    Private m_Tag As Integer

    Public Property Type() As String
        Get
            Return m_Type
        End Get
        Set(value As String)
            m_Type = value
        End Set
    End Property
    Private m_Type As String

    Public Property PathControllers() As String
        Get
            Return m_PathControllers
        End Get
        Set(value As String)
            m_PathControllers = value
        End Set
    End Property
    Private m_PathControllers As String

    Public Property DestinationControllers() As String
        Get
            Return m_DestinationControllers
        End Get
        Set(value As String)
            m_DestinationControllers = value
        End Set
    End Property
    Private m_DestinationControllers As String

    Public Property CompletionType() As String
        Get
            Return m_CompletionType
        End Get
        Set(value As String)
            m_CompletionType = value
        End Set
    End Property
    Private m_CompletionType As String

    Public Property Duration() As String
        Get
            Return m_Duration
        End Get
        Set(value As String)
            m_Duration = value
        End Set
    End Property
    Private m_Duration As String

    Public Property CreatedTime() As DateTime
        Get
            Return m_CreatedTime
        End Get
        Set(value As DateTime)
            m_CreatedTime = value
        End Set
    End Property
    Private m_CreatedTime As DateTime

    Public Property EndTime() As DateTime
        Get
            Return m_EndTime
        End Get
        Set(value As DateTime)
            m_EndTime = value
        End Set
    End Property
    Private m_EndTime As DateTime

    Public Property InitiatedTime() As String
        Get
            Return m_InitiatedTime
        End Get
        Set(value As String)
            m_InitiatedTime = value
        End Set
    End Property
    Private m_InitiatedTime As String

    Public Property Status() As String
        Get
            Return m_Status
        End Get
        Set(value As String)
            m_Status = value
        End Set
    End Property
    Private m_Status As String

End Class
Public Class AlertModel
    Public Property LOGID() As String
        Get
            Return m_LOGID
        End Get
        Set(value As String)
            m_LOGID = value
        End Set
    End Property
    Private m_LOGID As Integer

    Public Property LogType() As String
        Get
            Return m_LogType
        End Get
        Set(value As String)
            m_LogType = value
        End Set
    End Property
    Private m_LogType As String

    Public Property DateTime() As DateTime

        Get
            Return m_DateTime
        End Get
        Set(value As DateTime)
            m_DateTime = value
        End Set
    End Property
    Private m_DateTime As DateTime

    Public Property Events() As String

        Get
            Return m_Events
        End Get
        Set(value As String)
            m_Events = value
        End Set
    End Property
    Private m_Events As String

    Public Property AlertType() As String

        Get
            Return m_AlertType
        End Get
        Set(value As String)
            m_AlertType = value
        End Set
    End Property
    Private m_AlertType As String

    Public Property Tag() As Integer

        Get
            Return m_Tag
        End Get
        Set(value As Integer)
            m_Tag = value
        End Set
    End Property
    Private m_Tag As Integer

    Public Property Device() As String

        Get
            Return m_Device
        End Get
        Set(value As String)
            m_Device = value
        End Set
    End Property
    Private m_Device As String

    Public Property Annotation() As String

        Get
            Return m_Annotation
        End Get
        Set(value As String)
            m_Annotation = value
        End Set
    End Property
    Private m_Annotation As String

    Public Property ActionBy() As String

        Get
            Return m_ActionBy
        End Get
        Set(value As String)
            m_ActionBy = value
        End Set
    End Property
    Private m_ActionBy As String

End Class
Public Class SystemStatusModel
    Public Property Type() As String
        Get
            Return m_Type
        End Get
        Set(value As String)
            m_Type = value
        End Set
    End Property
    Private m_Type As String

    Public Property SubType() As String
        Get
            Return m_SubType
        End Get
        Set(value As String)
            m_SubType = value
        End Set
    End Property
    Private m_SubType As String

    Public Property Status() As String

        Get
            Return m_Status
        End Get
        Set(value As String)
            m_Status = value
        End Set
    End Property
    Private m_Status As String

    Public Property Description() As String

        Get
            Return m_Description
        End Get
        Set(value As String)
            m_Description = value
        End Set
    End Property
    Private m_Description As String

    Public Property UIVersion() As String

        Get
            Return m_UIVersion
        End Get
        Set(value As String)
            m_UIVersion = value
        End Set
    End Property
    Private m_UIVersion As String

    

End Class
Public Class AccessSettingModel

    Public Property accessid As Integer

    Public Property accessname As String

    Public Property patientlst As List(Of AccessLevelInfo)

    Public Property stafflist As List(Of AccessLevelInfo)

    Public Property undefinedlst As List(Of AccessLevelInfo)

    Public Property iolst As AccessLevelInfo

    Public Property escortlst As List(Of EscortInfo)

    Public Property asssetinf As AssetInfo

    Public Property secinf As SecurityInfo
End Class
Public Class ControllerMdl
    Public Property controller As String
End Class
Public Class AccessLevelInfo
    Public Property AccessId As Integer
    Public Property TypeId As Integer
    Public Property RiskClearanceId As Integer
    Public Property DoorLock As Integer
    Public Property SystemTimeout As Integer
    Public Property Buzzer As Integer
    Public Property Egress As Integer
    Public Property ISOOneFireOnOff As Integer
    Public Property ISOOneDoorAjarOnOff As Integer
    Public Property ISOOneTgOnOff As Integer
    Public Property ISOOneTgNoBzOnOff As Integer
    Public Property ISOOneUnauthOnOff As Integer
    Public Property ISOOneEmergencyOnOff As Integer
    Public Property ISOOneLoiterOnOff As Integer
    Public Property ISOOneIdleEgressOnOff As Integer
    Public Property ISOOneTamperOnOff As Integer
    Public Property ISOOneLockCmdOnOff As Integer
    Public Property ISOOneUnLockCmdOnOff As Integer
    Public Property ISOTwoFireOnOff As Integer
    Public Property ISOTwoDoorAjarOnOff As Integer
    Public Property ISOTwoTgOnOff As Integer
    Public Property ISOTwoTgNoBzOnOff As Integer
    Public Property ISOTwoUnauthOnOff As Integer
    Public Property ISOTwoEmergencyOnOff As Integer
    Public Property ISOTwoLoiterOnOff As Integer
    Public Property ISOTwoIdleEgressOnOff As Integer
    Public Property ISOTwoTamperOnOff As Integer
    Public Property ISOTwoLockCmdOnOff As Integer
    Public Property ISOTwoUnLockCmdOnOff As Integer
    Public Property ISOThreeFireOnOff As Integer
    Public Property ISOThreeDoorAjarOnOff As Integer
    Public Property ISOThreeTgOnOff As Integer
    Public Property ISOThreeTgNoBzOnOff As Integer
    Public Property ISOThreeUnauthOnOff As Integer
    Public Property ISOThreeEmergencyOnOff As Integer
    Public Property ISOThreeLoiterOnOff As Integer
    Public Property ISOThreeIdleEgressOnOff As Integer
    Public Property ISOThreeTamperOnOff As Integer
    Public Property ISOThreeLockCmdOnOff As Integer
    Public Property ISOThreeUnLockCmdOnOff As Integer
    Public Property GeneralIO1 As Integer
    Public Property IO1_EgressConditional As Integer
    Public Property IO1_Conditional As Integer
    Public Property GeneralIO2 As Integer
    Public Property IO2_EgressConditional As Integer
    Public Property IO2_Conditional As Integer
    Public Property ISOOneText As String
    Public Property ISOTwoText As String
    Public Property ISOThreeText As String
    Public Property EgressBtnCmb As Integer
End Class
Public Class EscortInfo
    Public Property Id As Integer
    Public Property AccessId As Integer
    Public Property EscortTypeId As Integer
    Public Property OptionId As Integer
    Public Property CustomPatientLevel As Integer
    Public Property CustomStaffLevel As Integer
    Public Property DoorLock As Integer
    Public Property SystemTimeout As Integer
    Public Property Buzzer As Integer
    Public Property Egress As Integer
End Class
Public Class AssetInfo
    Public Property Id As Integer
    Public Property AccessId As Integer
    Public Property DoorLock As Integer
    Public Property SystemTimeout As Integer
    Public Property Buzzer As Integer
    Public Property Egress As Integer
    Public Property StaffEscort As Boolean
    Public Property PatientEscort As Boolean
End Class
Public Class SecurityInfo
    Public Property Id As Integer
    Public Property GlobalKeyCode As String
    Public Property LDoorLock As Integer
    Public Property LSystemTimeout As Integer
    Public Property LBuzzer As Integer
    Public Property LEgress As Integer
    Public Property ULDoorLock As Integer
    Public Property ULSystemTimeout As Integer
    Public Property ULBuzzer As Integer
    Public Property ULEgress As Integer
    Public Property TDoorLock As Integer
    Public Property TSystemTimeout As Integer
    Public Property TBuzzer As Integer
    Public Property TEgress As Integer
    Public Property FDoorLock As Integer
    Public Property FSystemTimeout As Integer
    Public Property FBuzzer As Integer
    Public Property FEgress As Integer
    Public Property LoiterTimeout As Integer
    Public Property DoorAjarTimeout As Integer
    Public Property KeyCodeButtonPressTimeout As Integer
    Public Property EmergencyWaitTime As Integer
    Public Property UnauthorizedEgressAlertBuzzer As Integer
    Public Property UnauthorizedEgressAlertType As Integer
    Public Property EmergencyType As Integer
    Public Property TagRelinquishedTimer As Integer
    Public Property IsButtonPress As Integer
    Public Property DoorAjarBuzzer As Integer
    Public Property UnResponsiveTamperTag As Integer
    Public Property SensorTamperTag As Integer
    Public Property IsEnableIpCamera As Integer
    Public Property UnresponsiveWaitTime As Integer
    Public Property AlertTimeout As Integer
    Public Property LoiterBlinkAndChrip As Integer
    Public Property AccessId As Integer
    Public Property KeycodeStaffRequire As Integer
    Public Property TransportWrngTime As Integer
    Public Property TransferWrngTime As Integer
    Public Property TamperWrngTime As Integer
    Public Property UnresponsiveHeartBeatTime As Integer
    Public Property EnableUnresponsiveDeviceAlert As Integer
    Public Property UnresponsiveDeviceTimeMin As Integer
    Public Property MaxAttempt As Integer
    Public Property StaffDuressBtns As Integer
    Public Property UnresponsiveStarInMin As Integer
    Public Property UnresponsiveControllerInMin As Integer
    Public Property TagLooseUnitType As Integer
    Public Property StaffDuressType As Integer
    Public Property StaffTagRelinquishedTimer As Integer
    Public Property LoiterAlarmBlinkAndChrip As Integer
End Class
