Public Class frmError

#Region "Form"

    ''' <summary>
    ''' Wird beim Laden des Fehlerlogbuch-Formulars (frmError) ausgeführt.
    ''' Prüft, ob eine Logdatei existiert, liest deren Inhalt seriell ein und stellt ihn in der TextBox dar.
    ''' Schlägt anschließend die Deaktivierung von Bedienelementen vor, falls keine Log-Einträge vorhanden sind.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 04.09.2008 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort beim Aufruf von 'prNoRecord' entfernt.
    ''' - Datei-Existenzprüfung und Einlesevorgang beibehalten (sofern 'ReadFileSeriell' intern sicher arbeitet).
    ''' </remarks>
    Private Sub frmError_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If FileExists(cgErrLogFile) Then
            tbErrtext.Text = ReadFileSeriell(cgErrLogFile)
        End If
        prNoRecord()
    End Sub

    ''' <summary>
    ''' Deaktiviert die Löschen-Schaltfläche (tsbDelete) in der Toolbar, wenn das Fehlerlogbuch leer ist,
    ''' um Fehlbedienungen durch den Benutzer zu vermeiden.
    ''' </summary>
    ''' <remarks>
    ''' 04.09.2008 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Ungenutzte lokale Variable 'lStatus' vollständig entfernt.
    ''' - Direktzuweisung der 'Enabled'-Eigenschaft optimiert (anstelle der 'If-Else'-Verzweigung).
    ''' </remarks>
    Private Sub prNoRecord()
        ' Direktzuweisung spart die If-Verzweigung: Wenn TextLength > 0, dann True, sonst False
        tsbDelete.Enabled = (tbErrtext.TextLength > 0)
    End Sub

#End Region

#Region "Menü / Toolbar"

    ''' <summary>
    ''' Schließt das aktuelle Formular und kehrt zum aufrufenden Fenster zurück.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 04.09.2008 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - XML-Dokumentation um die Parameter 'sender' und 'e' für Eventhandler ergänzt.
    ''' </remarks>
    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klicken auf die Löschen-Schaltfläche. Nach einer Sicherheitsabfrage wird
    ''' die Logdatei vom Datenträger gelöscht, der globale Fehlerzähler zurückgesetzt und die Hauptmaske aktualisiert.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 04.09.2008 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Veralteten VB6-Befehl 'Kill()' durch den nativen .NET-Befehl 'System.IO.File.Delete()' ersetzt.
    ''' - Altes 'Str$()'-Konstrukt (VB6) durch das moderne '.ToString()' ersetzt.
    ''' - Veraltetes 'Call'-Schlüsselwort bei 'Me.Close()' entfernt.
    ''' </remarks>
    Private Sub tsbDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelete.Click
        Try
            Dim result As DialogResult = MessageBox.Show("Soll das Fehlerlogbuch gelöscht werden?",
                                                         "Hinweis",
                                                         MessageBoxButtons.YesNo,
                                                         MessageBoxIcon.Information,
                                                         MessageBoxDefaultButton.Button2)

            If result = DialogResult.Yes Then
                ' .NET-Standard statt dem alten VB6-Kill()
                If System.IO.File.Exists(cgErrLogFile) Then
                    System.IO.File.Delete(cgErrLogFile)
                End If

                ngError = 0
                ' Modernes .ToString() statt Str$() verhindert ungewollte führende Leerzeichen
                frmMain.tssLog.Text = "Info = " & ngError.ToString()

                Me.Close()
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub


#End Region

End Class
