Public Class myInit
    Public Shared Function ReadEntry(ByRef Inhalt As String, ByRef var As String) As String
        Dim aInhalt As Array = Split(Inhalt, vbCrLf)
        Dim aZeile As Array
        ReadEntry = ""
        For i = 0 To aInhalt.Length - 1
            If Mid(aInhalt(i), 1, 1) <> "[" And aInhalt(i) <> "" Then

                aZeile = Split(aInhalt(i), "=")
                If var.Trim = Trim(aZeile(0)) Then
                    ReadEntry = Trim(aZeile(1))
                End If

            End If

        Next

    End Function
    Public Shared Function WriteEntry(ByRef Inhalt As String, ByRef var As String, ByRef wert As String) As String
        Dim aInhalt As Array = Split(Inhalt, vbCrLf)
        Dim aZeile As Array
        Inhalt = ""
        For i = 0 To aInhalt.Length - 1
            If Mid(aInhalt(i), 1, 1) <> "[" And aInhalt(i) <> "" Then

                aZeile = Split(aInhalt(i), "=")
                If var.Trim = Trim(aZeile(0)) Then
                    aZeile(1) = Trim(wert)
                    aInhalt(i) = aZeile(0) & "=" & aZeile(1)
                End If
            End If
            If aInhalt(i) <> "" Then
                Inhalt = Inhalt & aInhalt(i) & vbCrLf
            End If
        Next
        WriteEntry = Inhalt

    End Function
    Public Shared Function ReadIni() As String

        ReadIni = ReadOneValueFromSystemDb("Pension")
       
    End Function

    Public Shared Sub WriteIni(ByRef Inhalt As String)
        'Dim b As String = ""
        'Dim a() As String = Split(Inhalt, vbCrLf)
        'For i = 0 To 7
        '    b = b & a(i) & vbCrLf
        'Next
        'b = b & "RFIDPort=COM3" & vbCrLf
        'b = b & "IPSchloss=192.168.178.68" & vbCrLf
        'For i = 8 To a.Length - 2
        '    b = b & a(i) & vbCrLf
        'Next
        'b = b & a(a.Length - 1)

        SaveOneValueInSystemDb("Pension", Inhalt)

    End Sub
End Class
