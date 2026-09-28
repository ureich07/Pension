Imports System.Text
Public Class frmRechnung
    Dim sRNr As String
    Dim arZ(20, 2) As String
    Dim sZID As String
    Dim sGKNr As String
    Dim nGNetto As Double
    Dim nGBrutto As Double
    Dim nGSt7 As Double
    Dim nGSt19 As Double
    Dim nGStS As Double
    Dim sSS7 As String
    Dim sSS19 As String
    Dim sSSS As String
    Dim sGKU As String
    Dim sGKS As String
    Dim sGKG As String
    Dim nAnzahlung As Double
    Dim sRdatum As String
    Dim lNew As Boolean
    Dim sRNr1 As String = ""
    Dim bNewRNr As Boolean = False
    Dim nReArt As Integer = 4  '0 zimmer, 1 Extras und zimmer, 2 Extras, 3 Storno, 4 Pauschal
    Dim sRDSenden As String
    Dim sPausch As String = "0"

#Region "Load Form und Funktionen zur Darstellung des Moduls......................................."

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Cursor = Cursors.WaitCursor
        Me.BackColor = Color.LightYellow
        Dim sColor As Color = Color.LightYellow
        gbOption.BackColor = sColor
        gbGast.BackColor = sColor
        gbRechnung.BackColor = sColor
        'gbZimmer.BackColor = sColor
        'ssMain.BackColor = sColor
        'tsMain.BackColor = sColor
        coSteuer.Text = "0"
        buMail.Enabled = False
        ' Call Main()
        Call prSetTabelleRechnung(dtZim)
        Call prLoadZimmer(sgRBID, sgRNr)
        Call prLadeGastDaten(sgGID)
        lbBID.Text = sgRBID
        If sgRNr.Trim = "" Then
            sRNr = fcGetNr("RNr")
            tbRNr.Text = Date.Today.Year & "-" & sRNr
        Else
            tbRNr.Text = sgRNr
        End If
        Me.Cursor = Cursors.Default
        Call prRefreshRechnungsPosition()
        sGKNr = sgGID
    End Sub

    ''' <summary>
    ''' Tabelle Rechnungen erstellen
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks>
    ''' 19.3.2012 Create
    ''' </remarks>
    Private Sub prSetTabelleRechnung(ByVal dt As DataTable)

        With dgRechnung
            Dim nB As Integer = 50
            .Columns.Clear()
            .ColumnHeadersHeight = 30
            .Columns.Add("POS", "POS")
            .Columns.Add("Zimmer", "Zimmer")
            .Columns.Add("Menge", "Menge")
            .Columns.Add("Text", "Text")
            .Columns.Add("Betrag", "Betrag")
            .Columns.Add("ST7", "St Üb")
            .Columns.Add("ST19", "St. Sp")
            .Columns.Add("STS", "St. Ge")
            .Columns.Add("Netto", "Netto")
            .Columns.Add("Brutto", "Brutto")
            .Columns.Add("Zusatz", "Zusatz")
            .Columns.Add("Zeit", "Zeit")
            .Columns.Add("Art", "Art")
            .Columns.Add("Ausstattung", "Ausstattung")
            .Columns.Add("FPreis", "FPreis")
            .Columns.Add("sID", "sID")
            .Columns.Add("Storno", "Storno")
            .Columns.Add("Summe", "Summe")
            .Columns.Add("GPreis", "GPreis")
            .Columns(0).Width = 50
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(1).Width = 80
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(2).Width = 50
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(3).Width = 150
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            .Columns(4).Width = 50
            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(5).Width = 50
            .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns(6).Width = 50
            .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(7).Width = 50
            .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(8).Width = 80
            .Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(9).Width = 80
            .Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns(10).Width = nB
            .Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(11).Width = nB
            .Columns(11).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns(12).Width = nB
            .Columns(12).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(13).Width = nB
            .Columns(13).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(14).Width = nB
            .Columns(14).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(15).Width = nB
            .Columns(15).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(16).Width = nB
            .Columns(16).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(17).Width = nB
            .Columns(17).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(18).Width = nB
            .Columns(18).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False

            .AutoResizeRows()
            'Sortierung der Spalten verhindern
            Dim DGVCol As DataGridViewColumn
            For Each DGVCol In .Columns
                DGVCol.SortMode = DataGridViewColumnSortMode.Automatic
            Next
            'Tooltips für Feiertage aktivieren
            .ShowCellToolTips = True
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            ' Farben der selektierten Zeile
            With .DefaultCellStyle
                .SelectionBackColor = cgColorRow 'Color.GreenYellow
                .SelectionForeColor = Color.Black
            End With
            .ReadOnly = True
        End With


    End Sub

    ''' <summary>
    ''' Gastdaten laden und anzeigen
    ''' </summary>
    ''' <param name="sID"></param>
    ''' <remarks>
    ''' 04.02.2012 Create
    ''' </remarks>
    Private Sub prLadeGastDaten(ByVal sID As String)
        Dim sSQL As String = "Select * From Kunden Where ID='" & sID & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                lbAnrede.Text = .Item("Anrede").ToString
                lbName1.Text = .Item("Name1").ToString
                lbName2.Text = .Item("Name2").ToString
                lbVorname.Text = .Item("Vorname").ToString
                lbStrasse.Text = .Item("Strasse").ToString

                lbPLZ.Text = .Item("PLZ").ToString
                lbOrt.Text = .Item("Ort").ToString
                lbLand.Text = .Item("Land").ToString
            End With
        End If
    End Sub

    ''' <summary>
    ''' Zimmer der Buchung laden
    ''' </summary>
    ''' <param name="sBid"></param>
    ''' <remarks></remarks>
    Private Sub prLoadZimmer(ByVal sBid As String, ByVal sRNrID As String)
        Dim sSQL As String = "Select * from Buchung Where BID='" & sBid & "' and RID='" & sRNrID & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)
        Dim nMax As Integer = dt.Rows.Count - 1
        sRdatum = dt.Rows(0).Item("RDatum") 'rechnungsdatum
        sRDSenden = dt.Rows(0).Item("RDSenden").ToString.Trim
        sPausch = dt.Rows(0).Item("Pausch").ToString.Trim
        If sPausch = "1" Then
            rbPausch.Checked = True
            nReArt = 4
        End If

        If sRDSenden.Trim = "" Then

            cbBar.Checked = True
        Else
            cbBar.Checked = False
        End If



        If sRdatum.Trim = "" Then sRdatum = fcUmDatum(Date.Today)
        dtpRDatum.Value = fcUmDatum(sRdatum)
        tbRNr.Text = sRNrID                                         'rechnungsnummer 
        If tbRNr.Text.Trim = "" Then
            bNewRNr = True
            sRNr = fcGetNr("RNr")
            tbRNr.Text = Date.Today.Year & "-" & sRNr
        End If

        arZ(0, 0) = "99"
        arZ(0, 1) = "Weiteres Zimmer"
        Dim n As Integer = 0
        Dim i As Integer
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                arZ(i + 1, 0) = dt.Rows(i).Item("ZimID").ToString
                arZ(i + 1, 2) = dt.Rows(i).Item("ID").ToString
                arZ(i + 1, 1) = " "
            Next
        End If

        For i = 1 To 20
            If arZ(i, 0) <> Nothing Then
                arZ(i, 1) = fcGetObjektZimmerName(dtZim, arZ(i, 0)).Trim
                If arZ(i, 1) <> "" Then
                    chliZimmer.Items.Add(arZ(i, 1) & "[" & arZ(i, 2) & "]")
                    chliZimmer.SetItemChecked(n, True)
                    n += 1
                End If
            End If
        Next
        prRefreshRechnungsPosition()
    End Sub

    ''' <summary>
    ''' Mit Loslassen der Maustaste Rechnungspositionen aktualisieren
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 27.03.2012 Create
    ''' </remarks>
    Private Sub chliZimmer_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles chliZimmer.MouseUp
        Call prRefreshRechnungsPosition()
    End Sub

    ''' <summary>
    ''' Für jeden ausgewählten Eintrag die Rechnungsdaten laden
    ''' </summary>
    ''' <remarks>
    ''' 27.03.2012 Create
    ''' </remarks>
    Private Sub prRefreshRechnungsPosition()
        dgRechnung.Rows.Clear()
        coZimmer.Items.Clear()
        nGNetto = 0
        nGBrutto = 0
        nGSt7 = 0
        nGSt19 = 0
        nAnzahlung = 0
        lbAnzahlung.Text = nAnzahlung
        If chliZimmer.CheckedItems.Count = 0 Then Exit Sub

        For Each item As Object In chliZimmer.CheckedItems
            Call prAddRechnungsPosition(item.ToString())
            coZimmer.Items.Add(item.ToString())
        Next
        Call prCalculateSumme()
    End Sub

    ''' <summary>
    ''' Rechnungsposition zum GridView hinzufügen
    ''' </summary>
    ''' <param name="sZim"></param>
    ''' <remarks>
    ''' 27.03.2012 Create
    ''' </remarks>
    Private Sub prAddRechnungsPosition(ByVal sZim As String)
        Dim arT() As String
        Dim arL() As String
        Dim arF() As String
        Dim nTage As Integer = 0
        Dim nRB As Decimal = 0
        Dim nRBF As Decimal = 0
        Dim nRBG As Decimal = 0
        Dim nFr As Integer          'Anzahl Frühstück
        Dim sID As String = Extract(sZim, "[", "]", 1)
        sZim = AtLeft(sZim, "[", 1)
        sZID = fcGetObjektZimmerID(dtZim, sZim, "ID")
        Dim sSQL As String = "Select * from Buchung Where BID='" & sgRBID & "' and ID='" & sID & "'"

        Dim dt As DataTable = fcReadDataTable(sSQL)
        If dt.Rows.Count = 0 Then Exit Sub

        arT = fcGetData(sSQL, 0, {"Von", "Bis", "ZimID", "Personen", "Art", "Frueh", "Preis", "Anzahlung", "FPreis", "ID", "Storno", "Summe", "MwstU", "MwstS", "MwstG", "Gpreis", "GKU", "GKS", "GKG"})
        arF = fcGetData("select * From Zimmer Where ID ='" & sZID & "'", 0, {"Name", "Art", "Ausstattung"})
        arT(2) = arF(1)
        If Trim(arT(15)) = "" Then arT(15) = "0"
        'Anzahl Tage
        nTage = fcGetAnzahlTage(arT(0), arT(1))
        nFr = Val(arT(5))
        'Zimmer
        nRB = Val(arT(6))
        'Frühstück
        If arT(4) = "Ü/F" Then
            nRBF = Val(arT(8) * nFr) 'arIni(17)speise Frühstück
            nRBG = Val(arT(15) * nFr) 'getränke
            nRB = nRB - nRBF - nRBG
        End If
        sSS7 = arT(12)
        sSS19 = arT(13)
        sSSS = arT(14)
        sGKU = arT(16)
        sGKS = arT(17)
        sGKG = arT(18)
        arL = fcGetDataForRechnung(dgRechnung.Rows.Count, nTage, sZim, arT(2), nRB, nRBF, sSS7, sSS19, "0", arT(0), arT(1), arT(3), arT(4), arF(2), arT(8), arT(9), arT(10), arT(11), sSSS, arT(15))
        dgRechnung.Rows.Add(arL)

        'Anzahlung
        nAnzahlung += Val(arT(7)) / 100

        'Extras
        Call prGetExtrasZimmer(sgRBID, sZID)

    End Sub

    ''' <summary>
    ''' Anzahl der Tage ermitteln
    ''' </summary>
    ''' <param name="sVon"></param>
    ''' <param name="sBis"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fcGetAnzahlTage(ByVal sVon As String, ByVal sBis As String) As String
        fcGetAnzahlTage = "0"
        Dim dVon As Date = fcUmDatum(sVon)
        Dim dBis As Date = fcUmDatum(sBis)
        dBis = DateAdd(DateInterval.Day, 1, dBis)
        fcGetAnzahlTage = DateDiff(DateInterval.Day, dVon, dBis)
    End Function

    ''' <summary>
    ''' Datensatz für Rechnungsposition zusammenstellen
    ''' </summary>
    ''' <param name="nPos"></param>
    ''' <param name="nTage"></param>
    ''' <param name="sZim"></param>
    ''' <param name="sText"></param>
    ''' <param name="nBrutto7"></param>
    ''' <param name="nBrutto19"></param>
    ''' <param name="nSteuer7"></param>
    ''' <param name="nSteuer19"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 02.04.2012 Add Zusatz und Datum (von-bis)
    ''' </remarks>
    Private Function fcGetDataForRechnung(ByVal nPos As Integer, ByVal nTage As Integer, _
                                          ByVal sZim As String, ByVal sText As String, _
                                          ByVal nBrutto7 As Decimal, ByVal nBrutto19 As Decimal, _
                                          ByVal nSteuer7 As Integer, ByVal nSteuer19 As Integer, _
                                          ByVal sZusatz As String, ByVal sVon As String, _
                                          ByVal sBis As String, ByVal sPersonen As String, _
                                          ByVal sArt As String, ByVal sAusstattung As String, _
                                          ByVal fPreis As String, ByVal sID As String, _
                                          ByVal sStorno As String, ByVal sSumme As String, _
                                          ByVal nSteuerS As Integer, ByVal nBruttoS As Decimal) As Array

        Dim arL(18) As String
        Dim nRB As Decimal = 0       'rb= Brutto
        Dim nRBF As Decimal = 0
        Dim nRBS As Decimal = 0
        Dim nRN As Decimal = 0        'rn =netto
        Dim nRNF As Decimal = 0
        Dim nRNS As Decimal = 0
        Dim nST As Decimal = 0         'Steuer
        Dim nSTF As Decimal = 0
        Dim nSTS As Decimal = 0
        Dim nBruttoS1 As Decimal = nBruttoS * Val(sPersonen)  'Frühstück ?
        If nBrutto7 > 0 Then
            'nRN = Math.Round((nBrutto7 / 100) / (1 + nSteuer7 / 100), 2)
            'nST = ((nBrutto7 / 100) - nRN) * nTage
            'nRN = nRN * nTage
            'nRB = (nBrutto7 * nTage) / 100
            'nRN = nRN
            nRB = (nBrutto7 * nTage) / 100
            nRN = Math.Round((nRB) / (1 + nSteuer7 / 100), 2)
            nST = nRB - nRN
            'nRN = nRN
            '            nRN = Math.Round(nRB / (1 + nSteuer7 / 100), 2)
            '           nST = nRB - nRN
        End If
        If nBrutto19 > 0 Then
            nRBF = (nBrutto19 * nTage) / 100
            nRNF = Math.Round(nRBF / (1 + nSteuer19 / 100), 2)
            nSTF = nRBF - nRNF
        End If
        If nBruttoS > 0 Then
            nRBS = (nBruttoS1 * nTage) / 100
            nRNS = Math.Round(nRBS / (1 + nSteuerS / 100), 2)
            nSTS = nRBS - nRNS
        End If
        If nReArt = 4 Then
            If nBruttoS > 0 Then   'Getränke
                nRBS = (nBruttoS1 * nTage) / 100
                nRNS = Math.Round(nRBS / (1 + nSteuerS / 100), 2)
                nSTS = nRBS - nRNS
            End If
            If nBrutto19 > 0 Then   'Früschstück
                nRBF = (nBrutto19 * nTage) / 100
                nRNF = Math.Round(nRBF / (1 + nSteuer19 / 100), 2)
                nSTF = nRBF - nRNF
            End If
            nRB = (sSumme / 100) - nRBS - nRBF
            nRN = Math.Round(nRB / (1 + nSteuer7 / 100), 2)
            nST = nRB - nRN
        End If

        arL(0) = nPos.ToString
        arL(1) = sZim
        arL(2) = nTage.ToString
        arL(3) = sText
        arL(4) = fcFormatDecimal((nBrutto7 + nBrutto19 + nBruttoS1) / 100, 2)
        arL(5) = fcFormatDecimal((nST).ToString, 2) '"Steuer"
        arL(6) = fcFormatDecimal((nSTF).ToString, 2) '"Steuer" 
        arL(7) = fcFormatDecimal((nSTS).ToString, 2) '"Steuer" 
        arL(8) = fcFormatDecimal((nRN + nRNF + nRNS).ToString, 2) '"Netto" 
        arL(9) = fcFormatDecimal((nRB + nRBF + nRBS).ToString, 2) '"Brutto"
        arL(10) = sZusatz
        arL(12) = sArt
        arL(13) = sAusstattung
        arL(14) = fPreis
        arL(15) = sID
        arL(16) = sStorno
        arL(17) = sSumme
        arL(18) = nBruttoS
        If sZusatz = "0" Then

            arL(11) = fcUmDatum(sVon) & " - " & CDate(fcUmDatum(sBis)).AddDays(1) & "#" & sPersonen
        Else
            arL(11) = "-#-"
        End If

        fcGetDataForRechnung = arL
    End Function




    ''' <summary>
    ''' Extras zum Zimmer laden
    ''' </summary>
    ''' <param name="sBID"></param>
    ''' <param name="sZID"></param>
    ''' <remarks>
    ''' 20.03.2012 Create
    ''' 02.04.2012 Add Zusatz und Datum
    ''' </remarks>
    Private Sub prGetExtrasZimmer(ByVal sBID As String, ByVal sZID As String)
        Dim arL() As String
        Dim nZusatz As Double = 0
        Dim dt As DataTable = fcReadDataTable("Select * from Zusaetze Where BuchID='" & sBID & "' and ZimID ='" & sZID & "'")
        If dt.Rows.Count = 0 Then Exit Sub
        Dim nMax As Integer = dt.Rows.Count - 1
        Dim nSumme As Double
        Dim nMenge As Integer
        Dim nSteuerSatz As Integer = 0
        Dim sText As String
        Dim sZim As String
        Dim sST As String = "0"
        Dim sSTF As String = "0"
        Dim sSTS As String = "0"
        Dim nRB As Decimal = 0
        Dim nRBF As Decimal = 0
        Dim nRBS As Decimal = 0
        Dim sZuID As String
        Try
            For i As Integer = 0 To nMax
                sST = "0"
                sSTF = "0"
                sSTS = "0"
                nRB = 0
                nRBF = 0
                nRBS = 0
                If dt.Rows(i).RowState <> DataRowState.Deleted Then
                    nSumme = dt.Rows(i).Item("Betrag") '/ 100
                    nMenge = dt.Rows(i).Item("Menge")
                    nSteuerSatz = dt.Rows(i).Item("Steuer")
                    sText = dt.Rows(i).Item("Bezeichnung")
                    sZim = dt.Rows(i).Item("ZimNr")
                    sZuID = dt.Rows(i).Item("ID")
                    Select Case nSteuerSatz
                        Case arIni(11)
                            sST = arIni(11)
                            nRB = nSumme
                        Case arIni(10)
                            sSTF = arIni(10)
                            nRBF = nSumme
                        Case arIni(23)
                            sSTS = arIni(23)
                            nRBS = nSumme
                    End Select
                    'If nSteuerSatz = arIni(11) Then
                    '    sST = arIni(11)
                    '    nRB = nSumme
                    'ElseIf nSteuerSatz = arIni(10) Then
                    '    sSTF = arIni(10)
                    '    nRBF = nSumme
                    'End If


                    arL = fcGetDataForRechnung(dgRechnung.Rows.Count, nMenge, sZim, sText, nRB, nRBF, sST, sSTF, "1", "", "", "", "", "", nSteuerSatz, sZuID, "", "", nRBS, sSTS)

                    dgRechnung.Rows.Add(arL)
                    'arL = fcGetDataForRechnung(dgRechnung.Rows.Count, nMenge, sZim, sText, nSumme * 100, nSteuerSatz, "1", "", "")
                    'dgRechnung.Rows.Add(arL)

                End If
            Next
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    Private Sub rbExtraORe_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbExtraORe.CheckedChanged

        nReArt = 0
        Call prRefreshRechnungsPosition()
    End Sub

    Private Sub rbExtraMRe_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbExtraMRe.CheckedChanged

        nReArt = 1
         Call prRefreshRechnungsPosition()
    End Sub

    Private Sub rbExtraGRe_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbExtraGRe.CheckedChanged

        nReArt = 2
        Call prRefreshRechnungsPosition()
    End Sub
    Private Sub rbStorno_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbStorno.CheckedChanged

        nReArt = 3
        Call prRefreshRechnungsPosition()
    End Sub

    Private Sub rbPausch_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbPausch.CheckedChanged
        nReArt = 4
        Call prRefreshRechnungsPosition()
    End Sub

    Private Sub btClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btClose.Click
        Me.Close()
    End Sub

#End Region

#Region "Einzelne Rechnungsposition bearbeiten oder hinzufügen....................................."

    ''' <summary>
    ''' Mit Doppelclick Daten zum Bearbeiten auswählen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 27.03.2012 Create
    ''' </remarks>
    Private Sub dgRechnung_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgRechnung.CellDoubleClick
        Dim nSumme As Double = 0
        Try
            With dgRechnung.Rows(e.RowIndex)
                'tbPos.Text = .Cells(0).Value
                If .Cells(0).Value <> "" And .Cells(9).Value = "1" Then
                    tbPos.Text = .Cells(0).Value
                    tbPos.Enabled = False
                    tbText.Enabled = False
                    coZimmer.Enabled = False
                    coZimmer.Text = .Cells(1).Value
                    tbMenge.Text = .Cells(2).Value
                    tbText.Text = .Cells(3).Value
                    tbBetrag.Text = .Cells(4).Value
                    nSumme = .Cells(5).Value
                    If nSumme > 0 Then
                        lbSteuer.Text = .Cells(5).Value
                        coSteuer.Text = arIni(11)
                    Else
                        nSumme = .Cells(6).Value
                        If nSumme > 0 Then
                            lbSteuer.Text = .Cells(6).Value
                            coSteuer.Text = arIni(10)
                        Else
                            lbSteuer.Text = "0,00"
                            coSteuer.Text = "0"
                        End If
                    End If
                    lbNetto.Text = .Cells(7).Value
                    lbBrutto.Text = .Cells(8).Value
                    lbZusatz.Text = .Cells(9).Value
                    lbVon.Text = AtLeft(.Cells(10).Value, "-", 1)
                    lbBis.Text = AtRight(.Cells(10).Value, "-", 1)

                End If
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tbMenge_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbMenge.TextChanged
        Call prNewCalculatePosition()
    End Sub

    Private Sub tbBetrag_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBetrag.TextChanged
        Call prNewCalculatePosition()
    End Sub

    Private Sub coSteuer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coSteuer.SelectedIndexChanged
        Call prNewCalculatePosition()
    End Sub

    ''' <summary>
    ''' Rechnungsposition neu kalkulieren
    ''' </summary>
    ''' <remarks>
    ''' 27.03.2012 Create
    ''' </remarks>
    Private Sub prNewCalculatePosition()
        Dim nRB As Decimal = 0
        Dim nRBF As Decimal = 0
        Dim nRBS As Decimal = 0
        Dim nST As Decimal = 0
        Dim nSTF As Decimal = 0
        Dim nSTS As Decimal = 0
        Dim arL() As String
        Select Case coSteuer.Text
            Case arIni(11)
                nRB = Val(tbBetrag.Text) * 100
                nST = Val(coSteuer.Text)
            Case arIni(10)
                nRBF = Val(tbBetrag.Text) * 100
                nSTF = Val(coSteuer.Text)
            Case arIni(23)
                nRBS = Val(tbBetrag.Text) * 100
                nSTS = Val(coSteuer.Text)
        End Select
        'If coSteuer.Text = "7" Then
        '    nRB = Val(tbBetrag.Text) * 100
        '    nST = Val(coSteuer.Text)
        'Else
        '    nRBF = Val(tbBetrag.Text) * 100
        '    nSTF = Val(coSteuer.Text)
        'End If

        arL = fcGetDataForRechnung(Val(tbPos.Text), Val(tbMenge.Text), coZimmer.Text, tbText.Text, nRB, nRBF, nST, nSTF, "1", lbVon.Text, lbBis.Text, "", "", "", "", "", "", "", nSTS, nRBS)

        If coSteuer.Text = "7" Then
            lbSteuer.Text = arL(5)
        Else
            lbSteuer.Text = arL(6)
        End If
        lbNetto.Text = arL(7)
        lbBrutto.Text = arL(8)
    End Sub

    ''' <summary>
    ''' Rechnungsposition in GridView einfügen / überschreiben
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 27.03.2012 Create
    ''' </remarks>
    Private Sub btPosSpeichern_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btPosSpeichern.Click
        Dim nPos As Integer = Val(tbPos.Text)
        If tbPos.Text = "" Then    'neu
            lNew = True

            Dim nRB As Decimal = 0
            Dim nRBF As Decimal = 0
            Dim nRBS As Decimal = 0
            Dim nST As Decimal = 0
            Dim nSTF As Decimal = 0
            Dim nSTS As Decimal = 0
            Dim arL() As String
            Select Case coSteuer.Text
                Case arIni(11)
                    nRB = Val(tbBetrag.Text) * 100
                    nST = Val(coSteuer.Text)
                Case arIni(10)
                    nRBF = Val(tbBetrag.Text) * 100
                    nSTF = Val(coSteuer.Text)
                Case arIni(23)
                    nRBS = Val(tbBetrag.Text) * 100
                    nSTS = Val(coSteuer.Text)
            End Select
           
            dgRechnung.Rows(nPos).Cells(13).Value = coSteuer.Text
            tbPos.Text = dgRechnung.Rows.Count
            arL = fcGetDataForRechnung(Val(tbPos.Text), Val(tbMenge.Text), coZimmer.Text, tbText.Text, nRB, nRBF, nST, nSTF, "1", lbVon.Text, lbBis.Text, "", "", "", "", "", "", "", nSTS, nRBS)


          
            dgRechnung.Rows.Add(arL)

        Else
            lNew = False
            With dgRechnung
                .Rows(nPos).Cells(0).Value = tbPos.Text
                .Rows(nPos).Cells(1).Value = coZimmer.Text
                .Rows(nPos).Cells(2).Value = tbMenge.Text
                .Rows(nPos).Cells(3).Value = tbText.Text
                .Rows(nPos).Cells(4).Value = tbBetrag.Text
                .Rows(nPos).Cells(13).Value = coSteuer.Text
                If coSteuer.Text = arIni(11) Then
                    .Rows(nPos).Cells(5).Value = lbSteuer.Text
                    .Rows(nPos).Cells(6).Value = "0,00"

                ElseIf coSteuer.Text = arIni(10) Then
                    .Rows(nPos).Cells(5).Value = "0,00"
                    .Rows(nPos).Cells(6).Value = lbSteuer.Text

                Else
                    .Rows(nPos).Cells(5).Value = "0,00"
                    .Rows(nPos).Cells(6).Value = "0,00"

                End If
                .Rows(nPos).Cells(7).Value = lbNetto.Text
                .Rows(nPos).Cells(8).Value = lbBrutto.Text
            End With
        End If
        prSaveZusatz(nPos)
        Call prCalculateSumme()
        Call prClearEingabe()
    End Sub
    ''' <summary>
    ''' Speicherung durchführen
    ''' </summary>
    ''' <remarks>
    ''' 18.12.2011 Create
    ''' </remarks>
    Private Sub prSaveZusatz(ByRef nPos As Integer)
        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String = ""
        Dim sID As String = ""
        sID = dgRechnung.Rows(nPos).Cells(14).Value
        If lNew Then
            sID = fcGetTimeID(Date.Today)
            dgRechnung.Rows(nPos).Cells(14).Value = sID
        End If

        Dim nSumme As Double = 0
        Try

            sqlText = "ID,BuchID,ZimID,Menge,Bezeichnung,Betrag,Steuer,Gesamt,Datum,ZimNr,Name"
            arFields = Split(sqlText, ",")
            sqlText = fcSaveZusatz(sID)
            arValue = Split(sqlText, "°")

            If lNew Then
                Call fcInsertCommand("Zusaetze", arFields, arValue)
            Else
                cBedingung = " WHERE ID='" & sID & "'"
                Call fcUpdateCommand("Zusaetze", arFields, arValue, cBedingung)
            End If
            ''DataTable aktualisieren
            'If lNew Then
            '    'Datensatz in DataTable "dtZ" speichern
            '    Call fcInsertTable(dtZ, arFields, arValue)
            'Else
            '    'Datensatz in DataTable "dtZ" speichern
            '    cBedingung = "ID Like '" & sID & "'"
            '    Call fcUpdateTable(dtZ, arFields, arValue, cBedingung)
            'End If

            'Call prFuelleTabelleZusatz(dtZ)

            lNew = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ''Call prLoadObjInList(dtObj)
            'Call prLoockZusatz(False)
            lNew = False

        End Try
    End Sub

    ''' <summary>
    ''' Zu speichernde Daten aufbereiten
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 10.01..2012 Create
    ''' </remarks>
    Private Function fcSaveZusatz(ByVal sID As String) As String
        Dim sb As New StringBuilder
        Dim sGeb As String = " "
        'ID,BuchID,Pos,Menge,Bezeichnung,Betrag,Steuer,Gesamt,Datum,ZimNr,Name


        sb.Append(sID & "°")   'ID
        sb.Append(sgRBID & "°") 'buchID
        sb.Append(sgRZID & "°")  'ZimmerID
        sb.Append(tbMenge.Text & "°")   'menge
        sb.Append(tbText.Text & "°")     'leistung
        sb.Append(Str(Val(tbBetrag.Text) * 100) & "°")   'betrag
        sb.Append(coSteuer.Text & "°")
        sb.Append(Str(Val(lbBrutto.Text) * 100) & "°") '      ' sb.Append(lbSumme.Text & "°")
        sb.Append(fcUmDatum(Date.Today.ToString) & "°")
        sb.Append(coZimmer.Text & "°")
        sb.Append(lbName1.Text)
        fcSaveZusatz = sb.ToString
    End Function
    Private Sub prCalculateSumme()
        nGNetto = 0
        nGBrutto = 0
        nGSt7 = 0
        nGSt19 = 0
        nGStS = 0
        Dim nGSt As Decimal = 0
        If nReArt = 4 Then
            With dgRechnung
                For i As Integer = 0 To .Rows.Count - 1
                    nGNetto += .Rows(i).Cells(8).Value
                    nGBrutto += .Rows(i).Cells(9).Value  'Pauschalsumme (9)
                    nGSt7 += .Rows(i).Cells(5).Value
                    nGSt19 += .Rows(i).Cells(6).Value
                    nGStS += .Rows(i).Cells(7).Value
                Next
            End With
            nGSt = nGSt19 + nGSt7 + nGStS
            lbGNetto.Text = fcFormatDecimal(nGNetto.ToString, 2)
            lbGSt7.Text = fcFormatDecimal(nGSt.ToString, 2)
            '    lbGSt19.Text = fcFormatDecimal(nGSt19.ToString, 2)
            lbAnzahlung.Text = fcFormatDecimal(nAnzahlung.ToString, 2)
            lbGesamt.Text = fcFormatDecimal(nGBrutto - Val(lbAnzahlung.Text), 2)
            ' lbAnzahlung.Text = fcFormatDecimal(nAnzahlung.ToString, 2)
        Else
            With dgRechnung
                For i As Integer = 0 To .Rows.Count - 1
                    nGNetto += .Rows(i).Cells(8).Value
                    nGBrutto += .Rows(i).Cells(9).Value
                    nGSt7 += .Rows(i).Cells(5).Value
                    nGSt19 += .Rows(i).Cells(6).Value
                    nGStS += .Rows(i).Cells(7).Value
                Next
            End With
            nGSt = nGSt19 + nGSt7 + nGStS
            lbGNetto.Text = fcFormatDecimal(nGNetto.ToString, 2)
            lbGSt7.Text = fcFormatDecimal(nGSt.ToString, 2)
            '    lbGSt19.Text = fcFormatDecimal(nGSt19.ToString, 2)
            lbAnzahlung.Text = fcFormatDecimal(nAnzahlung.ToString, 2)
            lbGesamt.Text = fcFormatDecimal(nGBrutto - Val(lbAnzahlung.Text), 2)
        ' lbAnzahlung.Text = fcFormatDecimal(nAnzahlung.ToString, 2)
        End If
    End Sub

    ''' <summary>
    ''' Bearbeitung abbrechen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 27.03.2012 Create
    ''' </remarks>
    Private Sub btAbbruch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btAbbruch.Click
        Call prClearEingabe()
    End Sub

    ''' <summary>
    ''' Eingaben zurücksetzen
    ''' </summary>
    ''' <remarks>
    ''' 27.03.2012 Create
    ''' </remarks>
    Private Sub prClearEingabe()
        'tbPos.Enabled = True
        tbText.Enabled = True
        coZimmer.Enabled = True
        tbPos.Text = ""
        tbMenge.Text = "0"
        coZimmer.Text = ""
        tbText.Text = ""
        tbBetrag.Text = "0.00"
        coSteuer.Text = "0"
        lbSteuer.Text = "0.00"
        lbNetto.Text = "0.00"
        lbBrutto.Text = "0.00"
    End Sub

#End Region

    Private Sub btGastDaten_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btGastDaten.Click
        tiGast.Enabled = True
        sGKNr = sgGID
        frmGaeste.Show()
    End Sub

    Private Sub tiGast_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tiGast.Tick
        Application.DoEvents()
        If sGKNr <> sgGID Then
            tiGast.Enabled = False
            If MsgBox("Sollen die Gastdaten überschrieben werden?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Gastauswahl") = MsgBoxResult.Yes Then
                Call prLadeGastDaten(sgGID)
                sGKNr = sgGID
            End If

        End If
    End Sub

    ''' <summary>
    ''' Druck Rechnung
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 11.04.2012 Create
    ''' </remarks>
    Private Sub btDruck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btDruck.Click
        Dim arDruck(1) As String
        Dim sZeile As String = ""
        Dim arfeld3(3) As String
        sRNr1 = tbRNr.Text
        prVpeOpen()
        nReArt = 4
        If nReArt = 0 Or nReArt = 1 Then
            For i = 0 To dgRechnung.Rows.Count - 1
                If dgRechnung.Rows(i).Cells(10).Value = "0" Then
                    sZeile = ""
                    For j = 1 To 18
                        If j = 8 Or j = 9 Then
                            sZeile = sZeile + StrTrim(dgRechnung.Rows(i).Cells(j).Value, ".") + "#"
                        Else
                            sZeile = sZeile + dgRechnung.Rows(i).Cells(j).Value + "#"
                        End If

                    Next
                    ReDim Preserve arDruck(i)
                    sZeile = Mid(sZeile, 1, sZeile.Length - 1)
                    arDruck(i) = sZeile

                End If
            Next
            ' biD GastID,RechnungsID,Pararry,Bar
            sgRNr = tbRNr.Text
            'prX_Rechnung(ByRef sBID As String, ByVal sKID As String, ByRef nRNr As String, ByVal sdatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByRef sRZiel As String, ByVal S1 As String, ByRef s2 As String, ByVal nReArt As Integer, ByVal ParamArray arDruck1() As String)

            'Call prX_Rechnung(sgRBID, sgGID, tbRNr.Text, sRdatum, cbBar.Checked, nAnzahlung, sRDSenden, sSS7, sSS19, nReArt, arDruck)
            Call prX_Rechnung_CII(sgRBID, sgGID, tbRNr.Text, sRdatum, cbBar.Checked, nAnzahlung, sRDSenden, sSS7, sSS19, nReArt, arDruck)

            Call prDruckRechnung(sgRBID, sgGID, tbRNr.Text, sRdatum, cbBar.Checked, nAnzahlung, sRDSenden, sSS7, sSS19, sSSS, sGKU, sGKG, sGKS, nReArt, arDruck)
            Dim cBedingung As String
            arfeld3(0) = tbRNr.Text 'Rechnungsmummer
            arfeld3(1) = sRdatum  'Rechnungsdatum
            arfeld3(2) = "-1"  'Rechnung geschrieben
            arfeld3(3) = sgGID 'kunden ID
            For i = 0 To dgRechnung.Rows.Count - 1
                cBedingung = " WHERE ID='" & dgRechnung.Rows(i).Cells(15).Value & "'" 'feld 15 sind die Id für die Buchungen
                Call fcUpdateCommand("Buchung", {"RId", "RDatum", "Rechnung", "kunID"}, arfeld3, cBedingung)
            Next
            If bNewRNr = True Then prSetNr("RNr", fcGetNr("RNr")) 'neue rechnungsnummer

            arfeld3 = fcDataSeek("select * From Bewertung Where BuchID ='", sgRBID, 0, {"ID"}) 'kontrolle ob schon geschriben
            If arfeld3(0) = " " Then
                If CheckBox1.Checked = True Then
                    ' If MsgBox(" Bewertungs Mail schreiben ?", vbExclamation + vbOKCancel, "Schreiben") = MsgBoxResult.Ok Then
                    Call fcInsertCommand("Bewertung", {"Id", "BuchID", "Status"}, {fcGetTimeID(Date.Today), sgRBID, "-1"})
                End If
            End If
            If nReArt = 0 Then prVpeVeiw()
        End If
        Dim ii As Integer = 0
        If nReArt = 2 Or nReArt = 1 Then
            If nReArt = 1 Then prVpeNewPage()
            For i = 0 To dgRechnung.Rows.Count - 1
                If dgRechnung.Rows(i).Cells(9).Value = "1" Then
                    sZeile = ""
                    For j = 1 To 13
                        sZeile = sZeile + dgRechnung.Rows(i).Cells(j).Value + "#"
                    Next
                    ReDim Preserve arDruck(ii)
                    sZeile = Mid(sZeile, 1, sZeile.Length - 1)
                    arDruck(ii) = sZeile
                    ii = ii + 1
                End If
            Next
            Call prDruckRechnungZusatz(sgRBID, sgGID, tbRNr.Text & "-1", sRdatum, True, 0, arDruck)
            '   Dim dt As DataTable = fcReadDataTable("Select * from Zusaetze Where BuchID='" & sBID & "' and ZimID ='" & sZID & "'")
            Dim cBedingung As String
            ReDim arfeld3(1)
            arfeld3(0) = tbRNr.Text & "-1" 'Rechnungsmummer
            arfeld3(1) = sRdatum  'Rechnungsdatum

            For i = 0 To dgRechnung.Rows.Count - 1
                cBedingung = " WHERE ID='" & dgRechnung.Rows(i).Cells(15).Value & "'" 'feld 15 sind die Id für die Buchungen
                Call fcUpdateCommand("Zusaetze", {"RID", "RDatum"}, arfeld3, cBedingung)
            Next
            ReDim arfeld3(3)
            arfeld3(0) = tbRNr.Text 'Rechnungsmummer
            arfeld3(1) = sRdatum  'Rechnungsdatum
            arfeld3(2) = "-1"  'Rechnung geschrieben
            arfeld3(3) = sgGID 'kunden ID
            For i = 0 To dgRechnung.Rows.Count - 1
                cBedingung = " WHERE ID='" & dgRechnung.Rows(i).Cells(15).Value & "'" 'feld 15 sind die Id für die Buchungen
                Call fcUpdateCommand("Buchung", {"RId", "RDatum", "Rechnung", "kunID"}, arfeld3, cBedingung)
            Next
            If bNewRNr = True Then prSetNr("RNr", fcGetNr("RNr")) 'neue rechnungsnummer
            prVpeVeiw()

        End If
        If nReArt = 3 Then    'Storno
            For i = 0 To dgRechnung.Rows.Count - 1
                If dgRechnung.Rows(i).Cells(9).Value = "0" Then
                    sZeile = ""
                    For j = 1 To 16
                        If j = 7 Or j = 8 Then
                            sZeile = sZeile + StrTrim(dgRechnung.Rows(i).Cells(j).Value, ".") + "#"
                        Else
                            sZeile = sZeile + dgRechnung.Rows(i).Cells(j).Value + "#"
                        End If

                    Next
                    ReDim Preserve arDruck(i)
                    sZeile = Mid(sZeile, 1, sZeile.Length - 1)
                    arDruck(i) = sZeile

                End If
            Next
            ' biD GastID,RechnungsID,Pararry,Bar
            Call prDruckRechnungStorno(sgRBID, sgGID, tbRNr.Text, sRdatum, cbBar.Checked, nAnzahlung, sRDSenden, arDruck)
            Dim cBedingung As String
            arfeld3(0) = tbRNr.Text 'Rechnungsmummer
            arfeld3(1) = sRdatum  'Rechnungsdatum
            arfeld3(2) = "-1"  'Rechnung geschrieben
            arfeld3(3) = sgGID 'kunden ID
            For i = 0 To dgRechnung.Rows.Count - 1
                cBedingung = " WHERE ID='" & dgRechnung.Rows(i).Cells(15).Value & "'" 'feld 15 sind die Id für die Buchungen
                Call fcUpdateCommand("Buchung", {"RId", "RDatum", "Rechnung", "kunID"}, arfeld3, cBedingung)
            Next
            If bNewRNr = True Then prSetNr("RNr", fcGetNr("RNr")) 'neue rechnungsnummer

            arfeld3 = fcDataSeek("select * From Bewertung Where BuchID ='", sgRBID, 0, {"ID"}) 'kontrolle ob schon geschriben
            If arfeld3(0) = " " Then
                If CheckBox1.Checked = True Then
                    ' If MsgBox(" Bewertungs Mail schreiben ?", vbExclamation + vbOKCancel, "Schreiben") = MsgBoxResult.Ok Then
                    Call fcInsertCommand("Bewertung", {"Id", "BuchID", "Status"}, {fcGetTimeID(Date.Today), sgRBID, "-1"})
                End If
            End If
            prVpeVeiw()
        End If
        If nReArt = 4 Then      'Pauschal
            For i = 0 To dgRechnung.Rows.Count - 1
                If dgRechnung.Rows(i).Cells(9).Value <> "0" Then
                    sZeile = ""
                    For j = 1 To 18 '13
                        If j = 7 Or j = 8 Then
                            sZeile = sZeile + StrTrim(dgRechnung.Rows(i).Cells(j).Value, ".") + "#"
                        Else
                            sZeile = sZeile + dgRechnung.Rows(i).Cells(j).Value + "#"
                        End If
                    Next
                    ReDim Preserve arDruck(i)
                    sZeile = Mid(sZeile, 1, sZeile.Length - 1)
                    arDruck(i) = sZeile
                End If
            Next
            ' biD GastID,RechnungsID,Pararry,Bar
            ' Call prX_Rechnung(sgRBID, sgGID, tbRNr.Text, sRdatum, cbBar.Checked, nAnzahlung, sRDSenden, sSS7, sSS19, nReArt, arDruck)
            prX_Rechnung_CII(sgRBID, sgGID, tbRNr.Text, sRdatum, cbBar.Checked, nAnzahlung, sRDSenden, sSS7, sSS19, nReArt, arDruck)
            Call prDruckRechnung(sgRBID, sgGID, tbRNr.Text, sRdatum, cbBar.Checked, nAnzahlung, sRDSenden, sSS7, sSS19, sSSS, sGKU, sGKS, sGKG, nReArt, arDruck)
            Dim cBedingung As String
            arfeld3(0) = tbRNr.Text 'Rechnungsmummer
            arfeld3(1) = sRdatum  'Rechnungsdatum
            arfeld3(2) = "-1"  'Rechnung geschrieben
            arfeld3(3) = sgGID 'kunden ID
            For i = 0 To dgRechnung.Rows.Count - 1
                cBedingung = " WHERE ID='" & dgRechnung.Rows(i).Cells(15).Value & "'" 'feld 15 sind die Id für die Buchungen
                Call fcUpdateCommand("Buchung", {"RId", "RDatum", "Rechnung", "kunID"}, arfeld3, cBedingung)
            Next
            If bNewRNr = True Then prSetNr("RNr", fcGetNr("RNr")) 'neue rechnungsnummer

            arfeld3 = fcDataSeek("select * From Bewertung Where BuchID ='", sgRBID, 0, {"ID"}) 'kontrolle ob schon geschriben
            If arfeld3(0) = " " Then
                If CheckBox1.Checked = True Then
                    ' If MsgBox(" Bewertungs Mail schreiben ?", vbExclamation + vbOKCancel, "Schreiben") = MsgBoxResult.Ok Then
                    Call fcInsertCommand("Bewertung", {"Id", "BuchID", "Status"}, {fcGetTimeID(Date.Today), sgRBID, "-1"})
                End If
            End If
            prVpeVeiw()
        End If
        Dim sJahr As String = Mid(tbRNr.Text, 1, 4)
        If DirExists(arIni(31) & "\" & sJahr) = False Then CreateDir(arIni(31) & "\" & sJahr)


        Dim sDateiPDF As String = cgPfad & "\Ablage\Rechnung.PDF"
        Dim sDateiCII As String = cgPfad & "\Ablage\Rechnung.xml"

        Dim sDateiZug As String = arIni(31) & "\" & sJahr & "\Rechnung_" & tbRNr.Text & ".pdf"
        EmbedXmlInPdf(sDateiPDF, sDateiCII, sDateiZug)
        buMail.Enabled = True
        '  Me.Close()
    End Sub
    Private Sub buMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buMail.Click
        Dim arDruck(1) As String
        Dim sZeile As String = ""
        Dim arfeld3(3) As String
        Dim ii As Integer = 0
        Dim sJahr As String = Mid(sRNr1, 1, 4)
        If FileExists(arIni(31) & "\" & sJahr & "\Rechnung_" & sRNr1 & ".PDF") = False Then
            MsgBox("Rechnung noch nicht Erstelt", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        Dim sText As Array = Split(ReadOneValueFromSystemDb("Mail_Rechnung"), "#")
        Dim sPdf As String = arIni(31) & "\" & sJahr & "\Rechnung_" & sRNr1 & ".PDF"
        ' Dim sDatei1 As String = arIni(32) & "\Rechnung\Rech_" & sRNr1 & ".XML"
        Dim arFeld(1) As String
        Dim arfeld1(1) As String
        ii = 0
        Dim sKID As String
        Dim sName As String = ""
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sgRBID, 0, {"KunID", "Sprache"})
        sKID = arfeld1(0)
        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"EMail", "Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land"})
        ii = Val(arfeld1(1))
        If arFeld(0).Trim <> "" Then
            If fcSendeMailAnlage(arFeld(0), "Rechnung", sText(ii), sDatei:=sPdf) = True Then
                MsgBox("Rechnungs Mail Erfolgreich gesendet", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                Dim cBedingung As String
                arfeld3(0) = tbRNr.Text 'Rechnungsmummer
                arfeld3(1) = sRdatum  'Rechnungsdatum
                arfeld3(2) = "-2"  'Rechnung geschrieben und versendet
                arfeld3(3) = sgGID 'kunden ID
                For i = 0 To dgRechnung.Rows.Count - 1
                    cBedingung = " WHERE ID='" & dgRechnung.Rows(i).Cells(15).Value & "'" 'feld 15 sind die Id für die Buchungen
                    Call fcUpdateCommand("Buchung", {"RId", "RDatum", "Rechnung", "kunID"}, arfeld3, cBedingung)
                Next
            Else
                MsgBox(" F e h l e r  Rechnungs Mail ", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            End If
        Else
            MsgBox(" F e h l e r  Keine Mailadresse", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
        End If
        Me.Close()
    End Sub


    Private Sub dtpRDatum_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpRDatum.ValueChanged
        sRdatum = fcUmDatum(dtpRDatum.Value)
    End Sub
End Class