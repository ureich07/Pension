Public Class frmPensionIni

    ''' <summary>
    ''' Wird beim Laden des Pensions-Einstellungsformulars ausgelöst. 
    ''' Lädt den hinterlegten Konfigurationswert für die Pension aus der Systemdatenbank.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das Formular).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 02.10.2026 - Code-Optimierung:
    ''' - Veraltete 'ByVal'-Modifizierer aus der Signatur entfernt.
    ''' - Direkte Zuweisung des ausgelesenen Datenbankwertes an das Textfeld implementiert.
    ''' </remarks>
    Private Sub frmPensionsIni_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tbFeld.Text = ReadOneValueFromSystemDb("Pension")
        ' Hebt die Standardmarkierung auf und setzt den Cursor ans Ende des Textes
        tbFeld.SelectionStart = tbFeld.Text.Length
        tbFeld.SelectionLength = 0
    End Sub

    ''' <summary>
    ''' Verarbeitet den Klick auf die Speichern-Schaltfläche. 
    ''' Sichert den aktuellen Inhalt des Textfeldes dauerhaft in der Systemdatenbank.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 02.10.2026 - Code-Optimierung:
    ''' - Veraltete 'ByVal'-Modifizierer aus der Signatur entfernt.
    ''' - Übergabe des bereinigten Textes (mittels Trim) zur Vermeidung ungewollter Leerzeichen.
    ''' </remarks>
    Private Sub buSave_Click(sender As Object, e As EventArgs) Handles buSave.Click
        SaveOneValueInSystemDb("Pension", tbFeld.Text.Trim())
    End Sub

End Class