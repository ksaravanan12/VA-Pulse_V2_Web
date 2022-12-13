Imports Microsoft.VisualBasic
Imports System.Text
Imports System.Data
Imports System.IO
Imports System
Imports System.Web.HttpContext
Namespace GMSUI
    Public Class ADUserModel
        Private _UserName As String
        Private _DisplayName As String
        Private _SamAcountName As String
        Private _GroupName As String

        Public Property SamAcountName() As String
            Get
                Return _SamAcountName
            End Get
            Set(ByVal value As String)
                _SamAcountName = value
            End Set
        End Property

        Public Property DisplayName() As String
            Get
                Return _DisplayName
            End Get
            Set(ByVal value As String)
                _DisplayName = value
            End Set
        End Property
    
        Public Property Group() As String
            Get
                Return _GroupName
            End Get
            Set(ByVal value As String)
                _GroupName = value
            End Set
        End Property
    End Class
End Namespace