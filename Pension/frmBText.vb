Public Class frmBText

    Dim nPos As Integer = 3
    Dim nZeichen As Integer


#Region "Formular initialisieren..................................................................."

    Private Sub frmBText_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sColor As Color = Color.LightYellow
        Me.BackColor = sColor
        tsMain.BackColor = sColor
        tslZeichen.Text = "255"
        If lLang Then nPos = 4
        Dim dt As DataTable = fcReadDataTable("Select * from BTexte Where Art='" & "0" & "'")
        If dt.Rows.Count = 0 Then Exit Sub
        For i As Integer = 0 To dt.Rows.Count - 1
            tscoBText.Items.Add(dt.Rows(i).Item(1))
        Next
        nZeichen = tbBText.Text.Length
        tslZeichen.Text = (255 - nZeichen).ToString
    End Sub

#End Region



    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        Me.Close()
    End Sub

    Private Sub tscoBText_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tscoBText.TextChanged
        tsbAdd.Enabled = True
    End Sub

    Private Sub tbBText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBText.TextChanged
        tsbSpeichern.Enabled = True
        nZeichen = tbBText.Text.Length
        tslZeichen.Text = (255 - nZeichen).ToString
    End Sub

    Private Sub tsbAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAdd.Click
        tsbAdd.Enabled = False

        tbBText.Text = tbBText.Text & fcAddMakro(tscoBText.Text, npos)
    End Sub

    Private Sub tsbSpeichern_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSpeichern.Click
        tsbSpeichern.Enabled = False
        If cgTablet = 0 Then
            frmReservierungDest.lbMakro.Text = tbBText.Text
        Else


            frmReservierung.lbMakro.Text = tbBText.Text
        End If

    End Sub

    Private Function fcAddMakro(ByVal sName As String, ByVal nPos As Integer) As String
        fcAddMakro = ""
        Dim dt As DataTable = fcReadDataTable("Select * from BTexte Where Name='" & sName & "'")
        If dt.Rows.Count = 0 Then Exit Function
        fcAddMakro = dt.Rows(0).Item(nPos).ToString & vbCrLf
    End Function
End Class