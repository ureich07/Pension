Option Strict Off
Option Explicit On
Public Class IniAccess
    Private Declare Function GetPrivateProfileString Lib "kernel32" _
            Alias "GetPrivateProfileStringA" ( _
            ByVal lpAppName As String, _
            ByVal lpKeyName As String, _
            ByVal lpDefault As String, _
            ByVal lpReturnedString As String, _
            ByVal nSize As Integer, _
            ByVal lpFileName As String) As Integer
    Private Declare Function WritePrivateProfileString Lib "kernel32" _
            Alias "WritePrivateProfileStringA" ( _
            ByVal lpAppName As String, _
            ByVal lpKeyName As String, _
            ByVal lpString As String, _
            ByVal lpFileName As String) As Integer
    Private Const MAXSTRINGLEN As Short = 260
    Private MyIniName As String
    Private MyString As String

    '==== Dateiname ====================
    Public WriteOnly Property IniFile() As String
        Set(ByVal Value As String)
            MyIniName = Value
        End Set
    End Property

    Public Function ReadEntry(ByVal Section As String, ByVal Key As String, ByVal Default_Renamed As String) As String
        Dim l As Integer
        Dim sKey As String
        sKey = Key & Chr(0)
        l = GetPrivateProfileString(Section & Chr(0), sKey, Default_Renamed & Chr(0), MyString, MAXSTRINGLEN, MyIniName & Chr(0))
        ReadEntry = Trim(Left(MyString, InStr(1, MyString, Chr(0), CompareMethod.Binary) - 1))
    End Function

    '==== Schreiben ====================
    Public Sub WriteEntry(ByVal Section As String, ByVal Key As String, ByVal Value As String)
        Dim sKey As String

        sKey = Key & Chr(0)
        MyString = LSet(Left(Value, MAXSTRINGLEN - 1) & Chr(0), Len(MyString))
        Call WritePrivateProfileString(Section & Chr(0), sKey, MyString, MyIniName & Chr(0))
    End Sub

    Private Sub Class_Initialize_Renamed()
        MyString = Space(MAXSTRINGLEN)
    End Sub

    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub
End Class