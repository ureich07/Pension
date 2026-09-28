
Partial Public Class myKalender
    Inherits System.Windows.Forms.UserControl ' mit und ohne, bringt nichts

    Dim d As String '= '"20150613" 'monath der in der mitte angezeigt werden soll
    Dim dAn As String = "20150605" 'anreise datum
    Dim dAb As String = "20150615" 'abreise Datum
    Dim aBuchung As String = "20150525-20150530"
    Dim aSperr As String = "20150625-20150630"
    Dim bAB As Boolean '= False ' true -> anreisedatum ändern, false -> Abreisedatum
   
    Property Datum() As String
        Get
            Return d
        End Get
        Set(ByVal value As String)
            d = value
            tbD.Text = d
        End Set
    End Property
    Property Anreise() As String
        Get
            Return dAn
        End Get
        Set(ByVal value As String)
            dAn = value
            tbAnreise.Text = dAn
        End Set
    End Property
    Property Abreise() As String
        Get
            Return dAb
        End Get
        Set(ByVal value As String)
            dAb = value
            If dAb = " " Then dAb = "20200101"
            Dim d2 As Date = DateAdd(DateInterval.Day, -1, CDate(fcUmDatum(dAb)))
            dAb = fcUmDatum(CDate(d2))
            tbAbreise.Text = dAb
        End Set
    End Property
    Property AnreiseAbreise() As Boolean
        Get
            Return bAB
        End Get
        Set(ByVal value As Boolean)
            bAB = value
            tbAnAb.Text = bAB
            lbAnAb.Text = "Abreise"
            If bAB = True Then
                lbAnAb.Text = "Anreise"

            End If
        End Set
    End Property
    Property Buchung() As String
        Get
            Return aBuchung
        End Get
        Set(ByVal value As String)
            aBuchung = value
            tbBuchung.Text = aBuchung
        End Set
    End Property
    Property Sperr() As String
        Get
            Return aSperr
        End Get
        Set(ByVal value As String)
            aSperr = value
            tbSperr.Text = aSperr
        End Set
    End Property

    Private Sub UserControl1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' CheckBox1.Checked = False
        Call prKalender(d, dAn, dAb, aBuchung, aSperr, bAB)
    End Sub

    Private Sub prKalender(ByRef sdatum As String, ByRef Anreise As String, ByRef Abreise As String, ByVal Buchung As String, ByRef Sperr As String, ByRef bAnAb As Boolean)
        Dim tag As Array = {"MO", "DI", "MI", "DO", "FR", "SO", "SO", " ", "MO", "DI", "MI", "DO", "FR", "SO", "SO", " ", "MO", "DI", "MI", "DO", "FR", "SO", "SO"}
        Dim sMonat As Array = {"", "Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember"}
        pKalender.BackColor = Color.Khaki
        tbAnreise.Text = Anreise
        tbAbreise.Text = Abreise
        pKalender.Width = 680
        pKalender.Height = 280
        With dgvKalender
            .Width = 650
            .Height = 200
            .RowCount = 6

            .ColumnCount = 23
            .ColumnHeadersVisible = True
            .RowHeadersVisible = False
            .MultiSelect = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False
            .ClearSelection()
            .DefaultCellStyle.SelectionBackColor = Color.Transparent
            .Columns(1).Name = "XX"

            Dim w As Integer = .Width / 23
            Dim h As Integer = (.Height / 7)
            .ColumnHeadersHeight = h
            For i = 0 To 22
                .Columns(i).Width = w
                .Columns(i).Name = tag(i)
                For j = 0 To 5
                    .Rows(j).Height = h
                    .Rows(j).Cells(i).Value = ""
                    .Rows(j).Cells(i).Style.BackColor = Color.LightGray
                Next
            Next

            Dim datum As Date = fcDatum(sdatum)
            Dim d1 As Date = DateAdd(DateInterval.Month, -1, datum)
            Call prCreateCalender(d1, 0, Anreise, Abreise, Buchung, Sperr)
            lMonat1.Text = sMonat(d1.Month) & " " & d1.Year
            Dim d2 As Date = datum
            Call prCreateCalender(d2, 8, Anreise, Abreise, Buchung, Sperr)
            lMonat2.Text = sMonat(d2.Month) & " " & d2.Year
            Dim d3 As Date = DateAdd(DateInterval.Month, 1, datum)
            Call prCreateCalender(d3, 16, Anreise, Abreise, Buchung, Sperr)
            lMonat3.Text = sMonat(d3.Month) & " " & d3.Year
        End With
    End Sub

    Private Sub prCreateCalender(ByRef ddatum As Date, ByRef nfeld As Integer, ByRef sVon As String, ByRef sBis As String, ByVal Buchung As String, Sperr As String)
        Dim dVon As Date = fcDatum(sVon)
        Dim dBis As Date = fcDatum(sBis)
        Dim d11 As Date = CDate("01." + Str(ddatum.Month) + Str(ddatum.Year))
        Dim nRows As Integer = 0
        Dim nCol As Integer = d11.DayOfWeek - 1 + nfeld
        Dim d2 As Date = DateAdd(DateInterval.Month, 1, ddatum)
        Dim VonBis1 As Array
        Dim VonBis As Array
        If nCol = -1 Then nCol = 6 + nfeld
        If d11.DayOfWeek = DayOfWeek.Monday Then
            nRows = 1
        End If
        With dgvKalender
            Do While True
                If d11.Month = d2.Month Then
                    Exit Do
                End If
                .Rows(nRows).Cells(nCol).Value = d11.Day
                .Rows(nRows).Cells(nCol).Style.BackColor = Color.White
                If d11 >= dVon And d11 <= dBis Then .Rows(nRows).Cells(nCol).Style.BackColor = Color.Green
                'Buchungen der gleiche BID
                If aBuchung <> "" Then
                    VonBis1 = Split(Buchung, ";")

                    For i = 0 To VonBis1.Length - 1
                        VonBis = Split(VonBis1(i), "-")
                        If d11 >= fcDatum(VonBis(0)) And d11 <= fcDatum(VonBis(1)) Then
                            .Rows(nRows).Cells(nCol).Style.BackColor = Color.GreenYellow
                        End If
                    Next
                End If
                'buchungen der fremden BID
                If aSperr <> "" Then
                    VonBis1 = Split(Sperr, ";")
                    For i = 0 To VonBis1.Length - 1
                        VonBis = Split(VonBis1(i), "-")
                        If d11 >= fcDatum(VonBis(0)) And d11 <= fcDatum(VonBis(1)) Then
                            .Rows(nRows).Cells(nCol).Style.BackColor = Color.Red
                        End If
                    Next
                End If
                nCol = nCol + 1
                If nCol = 7 + nfeld Then
                    nCol = 0 + nfeld
                    nRows = nRows + 1
                End If
                d11 = DateAdd(DateInterval.Day, 1, d11)
            Loop
        End With

    End Sub
    ''' <summary>
    ''' Datumsumwandlung, (20080101 > 01.09.2008)
    ''' </summary>
    ''' <param name="sDatum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function fcDatum(ByVal sDatum As String) As Date
        If Trim$(sDatum) = "" Then
            fcDatum = Date.Today
            Exit Function
        End If
        '(20080101 > 01.09.2008)
        fcDatum = CDate(Mid(sDatum, 7, 2) & "." & Mid(sDatum, 5, 2) & "." & Mid(sDatum, 1, 4))

    End Function

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles zurück.Click
        Dim dD As Date = DateAdd(DateInterval.Month, 1, fcDatum(d))
        Dim sdatum As String = CDate(dD)
        d = Mid(sdatum, 7, 4) & Mid(sdatum, 4, 2) & Mid(sdatum, 1, 2)
        Call prKalender(d, dAn, dAb, aBuchung, aSperr, bAB)
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles vor.Click
        Dim dD As Date = DateAdd(DateInterval.Month, -1, fcDatum(d))
        Dim sdatum As String = CDate(dD)
        d = Mid(sdatum, 7, 4) & Mid(sdatum, 4, 2) & Mid(sdatum, 1, 2)
        Call prKalender(d, dAn, dAb, aBuchung, aSperr, bAB)
    End Sub

    Private Sub dgvKalender_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvKalender.CellContentClick
        With dgvKalender
            If .Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Green Or .Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.White Then
                Dim tag As String = .Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                If tag.Length = 1 Then tag = "0" & tag
                Dim dx As String = ""
                If e.ColumnIndex >= 0 And e.ColumnIndex <= 6 Then
                    Dim d1 As Date = DateAdd(DateInterval.Month, -1, fcDatum(d))
                    Dim sdatum As String = CDate(d1)
                    dx = Mid(sdatum, 7, 4) & Mid(sdatum, 4, 2) & tag
                End If
                If e.ColumnIndex >= 8 And e.ColumnIndex <= 14 Then
                    Dim d1 As Date = fcDatum(d)
                    Dim sdatum As String = CDate(d1)
                    dx = Mid(sdatum, 7, 4) & Mid(sdatum, 4, 2) & tag
                End If
                If e.ColumnIndex >= 16 And e.ColumnIndex <= 22 Then
                    Dim d1 As Date = DateAdd(DateInterval.Month, 1, fcDatum(d))
                    Dim sdatum As String = CDate(d1)
                    dx = Mid(sdatum, 7, 4) & Mid(sdatum, 4, 2) & tag
                End If
                If dx <> "" Then
                    Dim nAnreise As Integer = Val(tbAnreise.Text)
                    Dim nAbreise As Integer = Val(tbAbreise.Text)
                    Dim nDx As Integer = Val(dx)
                    If tbAnAb.Text = "True" Then
                        'anreise datum ändern
                        If nAbreise >= nDx Then
                            'ok
                            If control(nDx, nAbreise, aBuchung & ";" & aSperr) = True Then
                                If cgTablet = 0 Then
                                    frmReservierungDest.lbAnreise.Text = fcUmDatum(dx)
                                Else
                                    frmReservierung.lbAnreise.Text = fcUmDatum(dx)
                                End If
                            '  MyClass.Visible = False
                        End If
                    End If

                Else
                    'abreise datum ändern
                    If nAnreise <= nDx Then
                        'ok
                            If control(nAnreise, nDx, aBuchung & ";" & aSperr) = True Then

                                Dim d2 As Date = DateAdd(DateInterval.Day, 1, CDate(fcUmDatum(dx)))
                                If cgTablet = 0 Then
                                    frmReservierungDest.lbAbreise.Text = CDate(d2)
                                Else
                                    frmReservierung.lbAbreise.Text = CDate(d2)
                                End If
                            '    MyClass.Visible = False
                        End If
                    End If
                End If
            End If
            End If
        End With
    End Sub

    Private Function control(ByRef Anreise As Integer, ByRef Abreise As Integer, Sperr As String) As Boolean
        control = True
        Dim VonBis1 As Array = Split(Sperr, ";")
        Dim VonBis As Array
        Dim nAn As Integer = 0
        Dim nAb As Integer = 0
        For i = 0 To VonBis1.Length - 1
            If VonBis1(i) <> "" Then
                VonBis = Split(VonBis1(i), "-")
                nAn = Val(VonBis(0))
                nAb = Val(VonBis(1))
                If nAn <= Abreise And nAn >= Anreise Then control = False
                If nAn <= Anreise And nAb >= Abreise Then control = False
                If nAb >= Anreise And nAb <= Abreise Then control = False
                If nAn >= Anreise And nAb <= Abreise Then control = False
            End If
        Next
    End Function

    Private Sub tbD_TextChanged(sender As System.Object, e As System.EventArgs) Handles tbD.TextChanged
        Call prKalender(d, dAn, dAb, aBuchung, aSperr, bAB)
    End Sub

    Private Sub tbAbreise_TextChanged(sender As System.Object, e As System.EventArgs) Handles tbAbreise.TextChanged
        Call prKalender(d, dAn, dAb, aBuchung, aSperr, bAB)
    End Sub

    Private Sub tbAnreise_TextChanged(sender As System.Object, e As System.EventArgs) Handles tbAnreise.TextChanged
        Call prKalender(d, dAn, dAb, aBuchung, aSperr, bAB)
    End Sub

    Private Sub tbAnAb_TextChanged(sender As System.Object, e As System.EventArgs) Handles tbAnAb.TextChanged
        Call prKalender(d, dAn, dAb, aBuchung, aSperr, bAB)
    End Sub

    Private Sub tbBuchung_TextChanged(sender As System.Object, e As System.EventArgs) Handles tbBuchung.TextChanged
        Call prKalender(d, dAn, dAb, aBuchung, aSperr, bAB)
    End Sub

    Private Sub tbSperr_TextChanged(sender As System.Object, e As System.EventArgs) Handles tbSperr.TextChanged
        Call prKalender(d, dAn, dAb, aBuchung, aSperr, bAB)
    End Sub

    Private Sub buExet_Click(sender As System.Object, e As System.EventArgs) Handles buExet.Click
        '   MyClass.Visible = False
    End Sub

    Private Sub pKalender_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pKalender.Paint

    End Sub
End Class
