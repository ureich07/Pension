Public Class frmWasser
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub prBerechnung()
        Dim nProz16 As Double = 0
        Dim nProz5 As Double = 0
        Dim nBV As Double = 0
        Dim nBE As Double = 0
        Dim nRest As Double = 0
        Dim nGesammt As Double = 0
        Dim nRBV As Double = 0
        Dim nRBE As Double = 0
        nRest = Val(Replace(tbRest.Text, ",", "."))
        nBV = Val(Replace(tbBetragVoll.Text, ",", "."))
        nBE = Val(Replace(tbBetragErmaessigt.Text, ",", "."))
        nGesammt = nBE + nBV
        lbGesammt.Text = nGesammt
        nProz16 = fcRound(nBV * 100 / nGesammt, 2)
        nProz5 = fcRound(nBE * 100 / nGesammt, 2)
        lbPro16.Text = nProz16
        lbPro5.Text = nProz5
        nRBV = fcRound(nRest * nProz16 / 100, 2)
        nRBE = fcRound(nRest * nProz5 / 100, 2)
        lbBetragMwstVoll.Text = nRBV
        lbBetragMwstErmae.Text = nRBE
        lbSumme.Text = nRBE + nRBV
    End Sub



    Private Sub tbRest_TextChanged(sender As Object, e As EventArgs) Handles tbRest.TextChanged
        prBerechnung()
    End Sub

    Private Sub tbBetragVoll_TextChanged(sender As Object, e As EventArgs) Handles tbBetragVoll.TextChanged
        prBerechnung()
    End Sub

    Private Sub tbBetragErmaessigt_TextChanged(sender As Object, e As EventArgs) Handles tbBetragErmaessigt.TextChanged
        prBerechnung()
    End Sub
End Class