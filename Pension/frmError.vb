Public Class frmError

#Region "Form"
    ''' <summary>
    ''' FormularErrorlogbuch wird geladen und initalisiert
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    '''04.09.2008 Create
    ''' </remarks>
    Private Sub frmError_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If FileExists(cgErrLogFile) Then tbErrtext.Text = ReadFileSeriell(cgErrLogFile)
        Call prNoRecord()
    End Sub


    ''' <summary>
    ''' Wenn keine Datensätze vorhanden sind, dann Tasten sperren um Fehlbedienungen zu vermeiden.
    ''' </summary>
    ''' <remarks>
    ''' 04.09.2008 Create
    ''' </remarks>
    Private Sub prNoRecord()
        Dim lStatus As Boolean = True
        If tbErrtext.TextLength = 0 Then
            tsbDelete.Enabled = False
        Else
            tsbDelete.Enabled = True
        End If
    End Sub


#End Region

#Region "Menü / Toolbar"

    ''' <summary>
    ''' Formular wird geschlossen
    ''' </summary>
    ''' <remarks>
    ''' 04.09.2008 Create 
    ''' </remarks>
    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        Me.Close()
    End Sub


    Private Sub tsbDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelete.Click
        Try

            Select Case MsgBox("Soll das Fehlerlogbuch gelöscht werden?", _
                        vbYesNo + vbInformation + vbDefaultButton2, "Hinweis")

                Case vbYes
                    Kill(cgErrLogFile)
                    ngError = 0
                    frmMain.tssLog.Text = "Info = " & Str$(ngError)
                    Call Me.Close()
            End Select

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    Private Sub tsbDruck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
#End Region

End Class