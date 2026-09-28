Public Class frmSQLError
    Dim P As String = "0"
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call prSQLText()

    End Sub

    Private Sub rbSQLError_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSQLError.CheckedChanged
        P = "0"
       Call prSQLText()
    End Sub

    Private Sub rbWebStoerung_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbWebStoerung.CheckedChanged
        P = "1"
        Call prSQLText()
    End Sub

    Private Sub buDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buDelete.Click
        Dim sIP As String = cgIPWeb
        Dim a As Boolean = PHP.SQLErrorDel(sIP, P)
        Call prSQLText()
    End Sub
    Private Sub prSQLText()
        Dim sIP As String = cgIPWeb
        Dim artext() As String = Split(PHP.SQLError(sIP, P), vbCrLf)
        tbSQL.Text = ""
        For i = 0 To artext.Length - 1
            tbSQL.Text = tbSQL.Text & artext(i) & vbCrLf
        Next
    End Sub
End Class