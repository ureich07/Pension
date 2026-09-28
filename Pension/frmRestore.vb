Imports MySql
Imports MySql.Data
Imports MySql.Data.MySqlClient



Imports System.Text

Public Class frmRestore

    Private Sub frmRestore_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Cursor = Cursors.WaitCursor
        Dim sIP As String = cgIPWeb
        Dim arDir As Array = PHP.SaveRestore(sIP)
        Dim sDatum As String = ""
        Dim sZeit As String = ""
        Me.Cursor = Cursors.Default
        If arDir.Length <> 0 Then
            For i = 0 To arDir.Length - 1
                sZeit = Mid(arDir(i), 25, 2) & ":" & Mid(arDir(i), 27, 2) & ":" & Mid(arDir(i), 29, 2)
                sDatum = fcUmDatum(Mid(arDir(i), 17, 8))
                coDatei.Items.Add(sDatum & " " & sZeit)
            Next

            coDatei.Text = coDatei.Items(0)
        Else
            Dim sMsg As String = "FTP Verbindung gescheitert "
            MsgBox(sMsg, vbOKCancel)
            Me.Close()
        End If


    End Sub
    Private Sub buWiederherstellen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buWiederherstellen.Click
        Dim sDatum As String = fcUmDatum(Mid(coDatei.Text, 1, 10))
        Dim sZeit As String = Mid(coDatei.Text, 12, 2) & Mid(coDatei.Text, 15, 2) & Mid(coDatei.Text, 18, 2)
        Dim sDatei As String = "./DBSicherung/DBSi" & sDatum & sZeit & ".dat"
        Dim sIP As String = cgIPWeb
        Dim sErr As Boolean = PHP.Restore(sIP, sDatei)
        If sErr = True Then
            'Ausgabe das rechner neu gestartet werden mus
            'keine datensicherung durchführen
            lgSichern = False
        Else
            'datenwiederherstellung gescheitert
        End If
        '    Dim sMsg As String = "Programm neu Starten"
        '    If MsgBox(sMsg, vbOKCancel, "OK") = MsgBoxResult.Ok Then End
        Me.Close()
    End Sub
    Private Sub buAbbrechen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buAbbrechen.Click
        Me.Close()
    End Sub


   
End Class