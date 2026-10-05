Imports System.Text
''' <summary>
''' Formular zur Anzeige und Verwaltung von SQL-Fehlermeldungen und Web-Störungsprotokollen.
''' </summary>
Public Class frmSQLError
    ' Statusvariable: 0 = SQL-Fehler, 1 = Web-Störung
    Dim mStatusType As Integer = 0

    ''' <summary>
    ''' Wird beim Laden des Fehlerformulars ausgelöst. Initialisiert die Textanzeige.
    ''' </summary>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        prSQLText()
    End Sub

    ''' <summary>
    ''' Wechselt die Anzeige auf SQL-Fehlerprotokolle.
    ''' </summary>
    Private Sub rbSQLError_CheckedChanged(sender As Object, e As EventArgs) Handles rbSQLError.CheckedChanged
        If rbSQLError.Checked Then
            mStatusType = 0
            prSQLText()
        End If
    End Sub

    ''' <summary>
    ''' Wechselt die Anzeige auf Web-Störungsprotokolle.
    ''' </summary>
    Private Sub rbWebStoerung_CheckedChanged(sender As Object, e As EventArgs) Handles rbWebStoerung.CheckedChanged
        If rbWebStoerung.Checked Then
            mStatusType = 1
            prSQLText()
        End If
    End Sub

    ''' <summary>
    ''' Löscht das ausgewählte Protokoll über eine PHP-Webanforderung und aktualisiert die Anzeige.
    ''' </summary>
    Private Sub buDelete_Click(sender As Object, e As EventArgs) Handles buDelete.Click
        Dim sIP As String = cgIPWeb
        ' Hinweis: 'a' wird deklariert, um den Rückgabewert der Funktion sauber aufzunehmen
        Dim success As Boolean = PHP.SQLErrorDel(sIP, mStatusType.ToString())
        prSQLText()
    End Sub

    ''' <summary>
    ''' Ruft die Protokolldaten remote ab und befüllt das Textfeld performant über einen StringBuilder.
    ''' </summary>
    Private Sub prSQLText()
        Dim sIP As String = cgIPWeb
        Dim rawText As String = PHP.SQLError(sIP, mStatusType.ToString())

        ' Robustes Splitten nach Zeilenumbrüchen
        Dim arText() As String = rawText.Split(New String() {vbCrLf, vbLf}, StringSplitOptions.None)

        Dim sb As New StringBuilder()
        For i As Integer = 0 To arText.Length - 1
            sb.Append(arText(i)).Append(vbCrLf)
        Next

        tbSQL.Text = sb.ToString()
    End Sub
End Class
