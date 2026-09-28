Imports System.Text
Public Class frmPensionIni

    Private Sub buLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buLoad.Click
        tbFeld.Text = ReadOneValueFromSystemDb("Pension")
    End Sub

    Private Sub buSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buSave.Click
        SaveOneValueInSystemDb("Pension", tbFeld.Text)
    End Sub

    Private Sub buMwstAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buMwstAll.Click
        'Dim sb As New StringBuilder
        'Dim sqlText As String = ""
        'Dim arFields(0), arValue(0) As String
        'Try
        '    sqlText = "MwstU,MwstS,MwstG,GKU,GKS,GKG"
        '    arFields = Split(sqlText, ",")
        '    sqlText = arIni(11) & "°" & arIni(23) & "°" & arIni(10) & "°" & arIni(20) & "°" & arIni(25) & "°" & arIni(21)
        '    arValue = Split(sqlText, "°")
        '    Call fcUpdateCommand("Buchung", arFields, arValue, "")
        'Catch ex As Exception
        '    ErrReport(ex.Message, ex.Source, ex.StackTrace)
        ' Finally


        ' End Try
        Dim sRID As String
        Dim cBedingung As String = ""
        Dim sSQL As String
        Dim dtB As DataTable
        Dim sqlText As String = ""
        Dim sGPreis As String = ""
        Dim sFPreis As String = ""
        Dim sID As String
        Dim arFields(0), arValue(0) As String
        sqlText = "MwstU,MwstS,MwstG,GKU,GKS,GKG,GPreis,FPreis"
        arFields = Split(sqlText, ",")
        sSQL = "Select * from Buchung WHERE  Von > '20200701' order by von asc"
        ' sSQL = "Select * from Buchung WHERE BIDIndex ='0' order by von asc"
        dtB = fcReadDataTable(sSQL)
        Dim svon As String
        For i = 0 To dtB.Rows.Count - 1
            sRID = Trim(dtB.Rows(i).Item("RID").ToString)
            If sRID = "" Or sRID = "0" Then
                sID = dtB.Rows(i).Item("ID").ToString
                cBedingung = " WHERE ID ='" & sID & "'"
                svon = fcUmDatum(dtB.Rows(i).Item("Von").ToString)
                sGPreis = Trim(dtB.Rows(i).Item("GPreis").ToString)
                sFPreis = Trim(dtB.Rows(i).Item("FPreis").ToString)
                If Trim(dtB.Rows(i).Item("Art").ToString) = "Ü/F" Then
                    If sFPreis <> "" And sFPreis <> "0" Then
                        If sGPreis = "" Or sGPreis = "0" Then
                            sGPreis = "100"
                            sFPreis = Str(Val(sFPreis) - 100)
                        End If
                    End If
                End If

                sqlText = arIni(11) & "°" & arIni(23) & "°" & arIni(10) & "°" & arIni(20) & "°" & arIni(25) & "°" & arIni(21) & "°" & sGPreis & "°" & sFPreis
                arValue = Split(sqlText, "°")
                Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
                tbFeld.Text = i
            End If
           


        Next



    End Sub
End Class