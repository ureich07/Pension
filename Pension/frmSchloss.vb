Public Class frmSchloss
    Private Sub frmSchloss_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        prLoad()
        Enabel(False)

    End Sub

    Private Sub tsbEdit_Click(sender As Object, e As EventArgs) Handles tsbEdit.Click
        Enabel(True)
    End Sub

    Private Sub tsbSave_Click(sender As Object, e As EventArgs) Handles tsbSave.Click
        Enabel(False)
        Dim sTmp As String = "Speicher_Mail_Set|"
        sTmp = sTmp & tbOpenZeit.Text & "|"
        sTmp = sTmp & tbCloseZeit.Text & "|"
        sTmp = sTmp & tbDoorZeit.Text & "|"
        sTmp = sTmp & tbWarteZeit.Text & "|"
        If cbAlarm.Checked = False Then
            sTmp = sTmp & "0|"
        Else
            sTmp = sTmp & "1|"
        End If
        sTmp = sTmp & tbIPMail.Text & "|"
        sTmp = sTmp & tbSMail.Text & ";"
        sTmp = sTmp & tbSKey.Text & ";"
        sTmp = sTmp & tbSMTP.Text & ";"
        sTmp = sTmp & tbPort.Text & ";"
        sTmp = sTmp & tbMail1.Text & ";"
        sTmp = sTmp & tbMail2.Text & ";"
        sTmp = sTmp & tbBetr.Text & ";|"
        If False = PHP.Data(sTmp, arIni(34)) Then

        End If
    End Sub

    Private Sub tsbRet_Click(sender As Object, e As EventArgs) Handles tsbRet.Click
        Enabel(False)
        prLoad()
    End Sub
    Private Sub Enabel(ByRef x As Boolean)
        tbOpenZeit.Enabled = x
        tbCloseZeit.Enabled = x
        tbDoorZeit.Enabled = x
        tbWarteZeit.Enabled = x
        tbSMail.Enabled = x
        tbSKey.Enabled = x
        tbSMTP.Enabled = x
        tbPort.Enabled = x
        tbMail1.Enabled = x
        tbMail2.Enabled = x
        tbIPMail.Enabled = x
        tbBetr.Enabled = x
        cbAlarm.Enabled = x
        Dim sColor As Color = Color.Black
        If x = True Then
            sColor = Color.Blue
        End If
        tbOpenZeit.ForeColor = sColor
        tbCloseZeit.ForeColor = sColor
        tbDoorZeit.ForeColor = sColor
        tbWarteZeit.ForeColor = sColor
        tbSMail.ForeColor = sColor
        tbSKey.ForeColor = sColor
        tbSMTP.ForeColor = sColor
        tbPort.ForeColor = sColor
        tbMail1.ForeColor = sColor
        tbMail2.ForeColor = sColor
        tbIPMail.ForeColor = sColor
        tbBetr.ForeColor = sColor
        cbAlarm.ForeColor = sColor
    End Sub
    Private Sub prLoad()
        Dim sTmp As String = PHP.PHPnoF("http://" & arIni(34), "POST", "Daten=Speicher_Mail_Get")
        Dim aTmp() As String = Split(sTmp, "|")
        If aTmp.Length >= 2 Then

            '  If aTmp(0) <> "ERROR" Then
            tbOpenZeit.Text = aTmp(0)
            tbCloseZeit.Text = aTmp(1)
            tbDoorZeit.Text = aTmp(2)
            tbWarteZeit.Text = aTmp(3)
            If aTmp(4) = "1" Then
                cbAlarm.Checked = True
            Else
                cbAlarm.Checked = False
            End If
            tbIPMail.Text = aTmp(5)
            Dim aTmp1() As String = Split(aTmp(6), ";")
            tbSMail.Text = aTmp1(0)
            tbSKey.Text = aTmp1(1)
            tbSMTP.Text = aTmp1(2)
            tbPort.Text = aTmp1(3)
            tbMail1.Text = aTmp1(4)
            tbMail2.Text = aTmp1(5)
            tbBetr.Text = aTmp1(6)


        End If

    End Sub

    Private Sub buDel_Click(sender As Object, e As EventArgs) Handles buDel.Click
        Dim sMsg As String = "Wollen Sie alle Chips wirklich löschen?  "
        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            Dim sTmp As String = PHP.PHPnoF("http://" & arIni(34), "POST", "Daten=Speicher_Del")
        End If

    End Sub

    Private Sub ToolStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles ToolStrip1.ItemClicked

    End Sub
End Class