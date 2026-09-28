Public Class Form1
    Dim dtPreis As DataTable
    Dim dtZimmer As DataTable
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim nMax As Integer = dtZim.Rows.Count - 1
        With cbZimmer
            .Items.Clear()
            For i = 0 To nMax
                .Items.Add(dtZim.Rows(i).Item("Name").ToString())
            Next
            .Text = dtZim.Rows(0).Item("Name").ToString()
        End With
        lbZimID.Text = fcGetObjektZimmerID(dtZim, cbZimmer.Text, "ID")
        Dim Sql As String = "SELECT * from Preise Where ZimID='" & lbZimID.Text & "'"
        dtPreis = fcReadDataTable(Sql)

        Sql = "SELECT * from Preise Where ID='" & lbZimID.Text & "'"
        dtZimmer = fcReadDataTable(Sql)

        With lvPreise
            .Clear()
            .Columns.Add("Datum", 70, HorizontalAlignment.Left)
            .Columns.Add("Tag", 70, HorizontalAlignment.Left)
            .Columns.Add("Preis", 100, HorizontalAlignment.Left)
            .Columns.Add("Dauer", 50, HorizontalAlignment.Left)
            .Columns.Add("Abschlag", 100, HorizontalAlignment.Left)
            .Columns.Add("Summe", 100, HorizontalAlignment.Left)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            ' .Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub

    Private Sub cbZimmer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbZimmer.SelectedIndexChanged
        lbZimID.Text = fcGetObjektZimmerID(dtZim, cbZimmer.Text, "ID")
        'Dim Sql As String = "SELECT * from Preise Where ZimID='" & lbZimID.Text & "'"
        'dtPre = fcReadDataTable(Sql)
        'Sql = "SELECT * from Zimmer Where ID='" & lbZimID.Text & "'"
        'dtZimmer = fcReadDataTable(Sql)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        lbRest.Text = ""
        Dim Sql As String = "SELECT * from Preise Where ZimID='" & lbZimID.Text & "'"
        dtPreis = fcReadDataTable(Sql)
        Sql = "SELECT * from Zimmer Where ID='" & lbZimID.Text & "'"
        dtZimmer = fcReadDataTable(Sql)
        Dim sVon As String = fcUmDatum(dtpVon.Value.ToString)
        Dim sBis As String = fcUmDatum(dtpBis.Value.ToString)
        Dim sVon1 As String = fcUmDatum(CDate(DateAdd("d", -1, CDate(fcUmDatum(sVon.Trim)))))
        Dim sBis1 As String = sBis.Trim
        Dim dTag As Date
        Dim MaxEr As Integer = Val(dtZimmer.Rows(0).Item("BettenEr").ToString)
        Dim MaxKi As Integer = Val(dtZimmer.Rows(0).Item("BettenKi").ToString)
        Dim Min As Integer = Val(dtZimmer.Rows(0).Item("BettenMin").ToString)
        Dim Pers As Integer = 0
        If MaxEr < Val(tbPersonen.Text) Then
            lbRest.Text = "zuviele Erwachsende"
            Exit Sub
        End If
        Pers = Val(tbPersonen.Text) + Val(tbKinder.Text)
        If MaxEr + MaxKi < Pers Then
            lbRest.Text = "zuviele kinder"
            Exit Sub
        End If
        If Pers <= Min Then Pers = Min
        Pers = Pers - 1
        If Pers < 0 Then
            lbRest.Text = "???"
            Exit Sub
        End If
        Dim aAb(10) As Integer
        aAb(0) = Val(dtZimmer.Rows(0).Item("P1").ToString)
        aAb(1) = Val(dtZimmer.Rows(0).Item("P2").ToString)
        aAb(2) = Val(dtZimmer.Rows(0).Item("P3").ToString)
        aAb(3) = Val(dtZimmer.Rows(0).Item("P4").ToString)
        aAb(4) = Val(dtZimmer.Rows(0).Item("P5").ToString)
        aAb(5) = Val(dtZimmer.Rows(0).Item("P6").ToString)
        aAb(6) = Val(dtZimmer.Rows(0).Item("P7").ToString)
        aAb(7) = Val(dtZimmer.Rows(0).Item("P8").ToString)
        aAb(8) = Val(dtZimmer.Rows(0).Item("P9").ToString)
        aAb(9) = Val(dtZimmer.Rows(0).Item("P10").ToString)
        Dim iAbschlag As Integer = aAb(Pers) * 100

        lbABschlag.Text = fcDecStr(iAbschlag / 100,,,)
        '  dtPreis errechnen
        Dim dVon As Date = CDate(fcUmDatum(sVon))
        Dim dBis As Date = CDate(fcUmDatum(sBis))

        Dim iTage As Integer = DateDiff("d", dVon, dBis)
        dBis = DateAdd("d", -1, dBis)
        Dim iDay As Integer
        Dim sDatum As String
        Dim aPreisx() As String
        Dim aDauer() As String
        Dim aPreis(iTage, 3) As Integer

        lbTag.Text = iTage
        For i = 0 To dtPreis.Rows.Count - 1
            For j = 0 To iTage - 1
                dTag = DateAdd("d", j, dVon)
                iDay = Weekday(dTag)

                iDay = iDay - 2
                If iDay = -1 Then iDay = 6
                sDatum = CDate(dTag)

                sDatum = fcUmDatum(sDatum)
                sVon = dtPreis.Rows(i).Item("ADatum").ToString
                sBis = dtPreis.Rows(i).Item("EDatum").ToString
                If sDatum >= sVon And sDatum <= sBis Then
                    aPreisx = Split(dtPreis.Rows(i).Item("Preis").ToString, "|")
                    aDauer = Split(dtPreis.Rows(i).Item("Dauer").ToString, "|")
                    If aPreis(j, 0) < aPreisx(iDay) Then aPreis(j, 0) = aPreisx(iDay)
                    If aPreis(j, 1) < aDauer(iDay) Then aPreis(j, 1) = aDauer(iDay)
                    aPreis(j, 2) = Val(sDatum)
                    aPreis(j, 3) = iDay
                End If
            Next
        Next
        Dim minDauer As Integer = 0
        Dim iSumme As Integer = 0
        For i = 0 To iTage - 1
            iSumme = iSumme + aPreis(i, 0) + iAbschlag
            If minDauer < aPreis(i, 1) Then minDauer = aPreis(i, 1)

        Next
        lbSumme.Text = fcDecStr(iSumme / 100,,,)
        lbMin.Text = minDauer
        lvPreise.Items.Clear()
        For i = 0 To iTage - 1

            Dim lv As ListViewItem

            With lvPreise
                lv = .Items.Add(fcUmDatum(Str(aPreis(i, 2)).Trim))
                lv.SubItems.Add(fcTagName(aPreis(i, 3)))
                lv.SubItems.Add(fcDecStr(aPreis(i, 0) / 100,,,))
                lv.SubItems.Add(aPreis(i, 1))
                lv.SubItems.Add(fcDecStr(iAbschlag / 100,,,))
                lv.SubItems.Add(fcDecStr((aPreis(i, 0) + iAbschlag) / 100,,,))
            End With
        Next
        'kontrolle ob frei
        Dim dtB As DataTable
        Sql = "SELECT * from buchung Where ZimID='" & lbZimID.Text & "' and Bis > '" & sVon1 & "' and Bis < '" & sBis1 & "'" ' abreise in der mitte
        dtB = fcReadDataTable(Sql)
        If dtB.Rows.Count <> 0 Then
            lbRest.Text = "Anreise zu früh"
            Exit Sub
        End If
        Sql = "SELECT * from buchung Where ZimID='" & lbZimID.Text & "' and Von>'" & sVon1 & "' and Von<'" & sBis1 & "'" ' abreise in der mitte
        dtB = fcReadDataTable(Sql)
        If dtB.Rows.Count <> 0 Then
            lbRest.Text = "Abreise zu Spät"
            Exit Sub
        End If
        Sql = "SELECT * from buchung Where ZimID='" & lbZimID.Text & "' and Von<'" & sVon1 & "' and Bis>'" & sBis1 & "'" ' abreise in der mitte
        dtB = fcReadDataTable(Sql)
        If dtB.Rows.Count <> 0 Then
            lbRest.Text = "Besetzt"
            Exit Sub
        End If
        lbRest.Text = "Frei"
        If lbTag.Text < lbMin.Text Then lbRest.Text = lbRest.Text & " mindestdauer"
    End Sub
    Private Function fcTagName(ByRef nr As Integer) As String
        Dim at() As String = {"Mo", "Di", "Mi", "Do", "Fr", "Sa", "So"}
        fcTagName = at(nr)
    End Function
End Class