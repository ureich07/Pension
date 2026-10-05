Imports System.Text

Public Class frmZusatz
    Dim lNew As Boolean
    Dim dtZ As DataTable
    Dim dtP As DataTable
    Dim sKNr As String
    Dim sPNr As String
    Dim sBNr As String
    Dim eRow As Integer
    Dim sZuID As String

#Region "Formular initialisieren..................................................................."

    Private Sub frmZusatz_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Cursor = Cursors.WaitCursor
        Me.BackColor = Color.LightYellow

        Dim sColor As Color = Color.LightYellow
        gbZusatz.BackColor = sColor
        ssMain.BackColor = sColor
        tsMain.BackColor = sColor
        paZusatz.BackColor = sColor

        lbZimNr.Text = frmReservierungDest.lbZimNr.Text
        lbName.Text = frmReservierungDest.tbName1.Text
        'dtZ = fcReadDataTable("Select * from Zusaetze Where BuchID='" & sgRBID & "' and ZimNr ='" & sgZNr & "'")
        dtZ = fcReadDataTable("Select * from Zusaetze Where BuchID='" & sgRBID & "' and ZimID ='" & sgRZID & "'")
        Call prSetTableZusatz()
        Call prFuelleTabelleZusatz(dtZ)
        Call prCreateTabelleLeistung()
        '   Call prFuelleTabelleLeistung(cgSystemPath & "\Zusatzkosten.ini")
        Me.Cursor = Cursors.Default
    End Sub

    ''' <summary>
    ''' Create Tabelle Zusatz
    ''' </summary>
    ''' <remarks>
    ''' 28.02.2012 Create 
    ''' </remarks>
    Private Sub prSetTableZusatz()
        With dgZusatz
            .Columns.Clear()
            .ColumnHeadersHeight = 25
            .Columns.Add("", "ID")
            .Columns.Add("", "Datum")
            .Columns.Add("", "Menge")
            .Columns.Add("", "Beschreibung")
            .Columns.Add("", "Preis")
            .Columns.Add("", "Steuer (%)")
            .Columns.Add("", "Gesamt")
            .Columns(0).Width = 50
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(0).Visible = False
            .Columns(1).Width = 80
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(2).Width = 50
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(3).Width = 400
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(4).Width = 80
            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(5).Width = 80
            .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(6).Width = 80
            .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False

            .AutoResizeRows()
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            ' Farben der selektierten Zeile
            With .DefaultCellStyle
                .SelectionBackColor = cgColorRow
                .SelectionForeColor = Color.Black
            End With
            .ReadOnly = True
        End With

    End Sub

    ''' <summary>
    ''' Tabelle "Leistung" erstellen
    ''' </summary>
    ''' <remarks>
    ''' 29.02.2012 Create
    ''' </remarks>
    Private Sub prCreateTabelleLeistung()
        With lvLeistung
            .Clear()
            .Columns.Add("Leistung", 150, HorizontalAlignment.Left)
            .Columns.Add("Preis", 50, HorizontalAlignment.Right)
            .Columns.Add("Steuer %", 60, HorizontalAlignment.Center)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            .Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub

    ''' <summary>
    ''' Tabelle Zusatz mit Daten füllen
    ''' </summary>
    ''' <remarks>
    ''' 29.02.2012 Create 
    ''' </remarks>
    Private Sub prFuelleTabelleZusatz(ByVal dt As DataTable)
        If dt.Rows.Count = 0 Then Exit Sub
        Dim nMax As Integer = dt.Rows.Count - 1
        Dim sb As New StringBuilder
        Dim arT() As String
        Dim nGesamt As Double = 0
        Dim nSumme As Double
        Dim nMenge As Integer
        Dim nSteuerSatz As Integer = 0
        Try
            With dgZusatz
                .Rows.Clear()
                For i As Integer = 0 To nMax
                    If dt.Rows(i).RowState <> DataRowState.Deleted Then
                        sb.Append(dt.Rows(i).Item("ID").ToString & ";")
                        sb.Append(fcUmDatum(dt.Rows(i).Item("Datum").ToString) & ";")
                        nSumme = dt.Rows(i).Item("Betrag") / 100
                        nMenge = dt.Rows(i).Item("Menge")
                        nSteuerSatz = dt.Rows(i).Item("Steuer")
                        sb.Append(nMenge.ToString & ";")
                        sb.Append(dt.Rows(i).Item("Bezeichnung").ToString & ";")
                        sb.Append(fcFormatDecimal(nSumme.ToString) & ";")
                        sb.Append(nSteuerSatz.ToString & ";")
                        nSumme = nSumme * nMenge
                        '   nSumme = nSumme + Math.Round((nSumme * 100) / (nSteuerSatz + 100), 2)
                        sb.Append((fcFormatDecimal(nSumme).ToString) & ";")
                        nGesamt = nGesamt + nSumme
                        lbZimNr.Text = dt.Rows(i).Item("ZimNr").ToString
                        lbName.Text = dt.Rows(i).Item("Name").ToString
                        arT = sb.ToString.Split(";")
                        .Rows.Add(arT)
                        sb.Remove(0, sb.Length)
                    End If
                Next
                lbSumme.Text = fcFormatDecimal(nGesamt.ToString)
            End With
            If dgZusatz.Rows.Count = 0 Then tsbDelete.Enabled = False
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)

        End Try
    End Sub

    ''' <summary>
    ''' Tabelle Leistungen mit Daten füllen
    ''' </summary>
    ''' <remarks>
    ''' 29.02.2012 Create 
    ''' </remarks>
    Private Sub prFuelleTabelleLeistung(ByVal sFile As String)
        Dim arTmp() As String = ReadOneValueFromSystemDb(sFile).Split(vbCrLf)
        Dim nMax As Integer = arTmp.Length - 1
        If nMax < 0 Then Exit Sub
        Dim arT() As String
        lvLeistung.Items.Clear()
        For i = 0 To nMax
            If arTmp(i).Trim <> "" Then
                arT = arTmp(i).Split(";")
                Dim lv As ListViewItem
                With lvLeistung
                    lv = .Items.Add(arT(0))
                    lv.SubItems.Add(fcChangeCentToEuroPunkt(arT(1)))
                    lv.SubItems.Add(arT(2))
                End With
            End If
        Next
    End Sub

#End Region

#Region "Buttons und Menü-Ereignisse..............................................................."

    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        Call prCloseForm()
    End Sub
    Private Sub prCloseForm()
        Me.Close()
        'frmMain.tsMain.Visible = True
        'frmMain.dgBuchung.Visible = True
        'frmMain.paDaten.Visible = True
        ''frmMain.dgBuchung.Select()
    End Sub

    Private Sub dgZusatz_CellDoubleClick(ByVal sender As Object, _
                ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
                Handles dgZusatz.CellDoubleClick
        paZusatz.Visible = True
        lNew = False
        dgZusatz.Enabled = False

        sZuID = dgZusatz.Rows(e.RowIndex).Cells(0).Value
        dtpDatum.Text = dgZusatz.Rows(e.RowIndex).Cells(1).Value
        tbMenge.Text = dgZusatz.Rows(e.RowIndex).Cells(2).Value
        tbLeistung.Text = dgZusatz.Rows(e.RowIndex).Cells(3).Value
        tbPreis.Text = dgZusatz.Rows(e.RowIndex).Cells(4).Value
        coSteuer.Text = dgZusatz.Rows(e.RowIndex).Cells(5).Value
        

    End Sub

    Private Sub tsbNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNew.Click
        lNew = True
        paZusatz.Visible = True
    End Sub

    Private Sub tsbDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelete.Click
        Dim sMsg As String = "Wollen Sie diese Leistung löschen?  "

        Dim cSql As String = "DELETE FROM Zusaetze WHERE ID = '" & sZuID & "'"
        If sZuID = "" Then Exit Sub
        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            'Datensatz per SQL aus der Tabelle löschen
            UpdateTable(cSql)
            'Änderung in DataTable "dtObj" speichern
            Call fcDeleteTableRow(dtZ, "ID = '" & sZuID & "'")
            dgZusatz.Select()
            Call prFuelleTabelleZusatz(dtZ)
        End If
    End Sub

    Private Sub dgZusatz_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgZusatz.CellEnter
        sZuID = dgZusatz.Rows(e.RowIndex).Cells(0).Value
    End Sub

#End Region

#Region "Risize...................................................................................."

    Private Sub frmKBuchen_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        'dgZusatz.Width = Me.Width - 40
        'dgZusatz.Height = Me.Height - 150

        'Dim nHMe As Integer = Me.Height / 2
        'Dim nWMe As Integer = Me.Width / 2
        'Dim nHpa As Integer = paZusatz.Height / 2
        'Dim nWpa As Integer = paZusatz.Width / 2
        'paZusatz.Location = New Point(nWMe - nWpa, nHMe - nHpa - 25)

    End Sub

#End Region

#Region "Zusätze bearbeiten........................................................................"

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Call prCloseZusatz()
    End Sub
    Private Sub prCloseZusatz()
        Call prLoockZusatz(False)
        paZusatz.Visible = False
        dgZusatz.Enabled = True
        dgZusatz.Select()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Call prSaveZusatz()
    End Sub

    ''' <summary>
    ''' Speicherung durchführen
    ''' </summary>
    ''' <remarks>
    ''' 18.12.2011 Create
    ''' </remarks>
    Private Sub prSaveZusatz()
        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String = ""
        Dim sID As String = sZuID
        If lNew Then sID = fcGetTimeID(Date.Today)
        Dim nSumme As Double = 0
        Try

            sqlText = "ID,BuchID,ZimID,Menge,Bezeichnung,Betrag,Steuer,Gesamt,Datum,ZimNr,Name"
            arFields = Split(sqlText, ",")
            sqlText = fcSaveZusatz(sID)
            arValue = Split(sqlText, "°")

            If lNew Then
                sID = fcAppendBlank("Zusaetze")
                ' Call fcInsertCommand("Zusaetze", arFields, arValue)
            End If 'Else
            cBedingung = " WHERE ID='" & sID & "'"
            Call fcUpdateCommand("Zusaetze", arFields, arValue, cBedingung)
            'End If
            'DataTable aktualisieren
            If lNew Then
                'Datensatz in DataTable "dtZ" speichern
                Call fcInsertTable(dtZ, arFields, arValue)
            Else
                'Datensatz in DataTable "dtZ" speichern
                cBedingung = "ID Like '" & sID & "'"
                Call fcUpdateTable(dtZ, arFields, arValue, cBedingung)
            End If

            Call prFuelleTabelleZusatz(dtZ)

            lNew = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            'Call prLoadObjInList(dtObj)
            Call prLoockZusatz(False)
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


        sb.Append(sID & "°")
        sb.Append(sgRBID & "°")
        sb.Append(sgRZID & "°")
        sb.Append(tbMenge.Text & "°")
        sb.Append(tbLeistung.Text & "°")
        sb.Append(Str(Val(tbPreis.Text) * 100) & "°")
        sb.Append(coSteuer.Text & "°")
        sb.Append(Str(Val(lbGesamt.Text) * 100) & "°")        ' sb.Append(lbSumme.Text & "°")
        sb.Append(fcUmDatum(dtpDatum.Value) & "°")
        sb.Append(lbZimNr.Text & "°")
        sb.Append(lbName.Text)
        fcSaveZusatz = sb.ToString
    End Function

    ''' <summary>
    ''' Steuerung der Button und Eingabefelder
    ''' </summary>
    ''' <param name="lStatus"></param>
    ''' <remarks>
    ''' 10.01.2012 Create
    ''' </remarks>
    Private Sub prLoockZusatz(ByVal lStatus As Boolean)
        tsbNew.Enabled = Not lStatus
        tsbDelete.Enabled = Not lStatus
    End Sub

#End Region

#Region "Hilfetexte in der Statusleiste ausgeben..................................................."

    'Private Sub tbName1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbName1.MouseMove
    '    tssInfo.Text = "Nachname / Firmenname"
    'End Sub

    'Private Sub tbVorname_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbVorname.MouseMove
    '    tssInfo.Text = "Vorname"
    'End Sub

    'Private Sub tbStrasse_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbStrasse.MouseMove
    '    tssInfo.Text = "Strasse"
    'End Sub

    'Private Sub tbPLZ_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbPLZ.MouseMove
    '    tssInfo.Text = "PLZ"
    'End Sub

    'Private Sub tbOrt_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbOrt.MouseMove
    '    tssInfo.Text = "Ort"
    'End Sub

    'Private Sub tbLand_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbLand.MouseMove
    '    tssInfo.Text = "Land (Kurz-Zeichen)"
    'End Sub

    'Private Sub tbPass_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbPass.MouseMove
    '    tssInfo.Text = "Ausweisnummer"
    'End Sub

    'Private Sub tbInfo_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbInfo.MouseMove
    '    tssInfo.Text = "Bemerkungen"
    'End Sub

    'Private Sub tbName2_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbName2.MouseMove
    '    tssInfo.Text = "Nachname wenn Firmenbezug"
    'End Sub

    'Private Sub dtpGeb_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dtpGeb.MouseMove
    '    tssInfo.Text = "Geburtsdatum"
    'End Sub

    'Private Sub tbTelefon_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbTelefon.MouseMove
    '    tssInfo.Text = "Telefon"
    'End Sub

    'Private Sub tbTelefax_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbTelefax.MouseMove
    '    tssInfo.Text = "Telefax"
    'End Sub

    'Private Sub tbHandy_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbHandy.MouseMove
    '    tssInfo.Text = "Mobiltelefon"
    'End Sub

    'Private Sub tbEMail_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbEMail.MouseMove
    '    tssInfo.Text = "E-Mail Adresse"
    'End Sub

    'Private Sub gbGaeste_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gbGaeste.MouseMove
    '    tssInfo.Text = ""
    'End Sub

    'Private Sub ToolStrip1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ToolStrip1.MouseMove
    '    tssInfo.Text = ""
    'End Sub

    'Private Sub dgZusatz_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgZusatz.MouseMove
    '    tssInfo.Text = ""
    'End Sub

    'Private Sub paZusatz_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles paZusatz.MouseMove
    '    tssInfo.Text = ""
    'End Sub

    'Private Sub cmdSave_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdSave.MouseMove
    '    tssInfo.Text = "Änderung speichern"
    'End Sub

    'Private Sub cmdClose_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdClose.MouseMove
    '    tssInfo.Text = "Bearbeitungsfenster schliessen"
    'End Sub

    'Private Sub lbClose_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbClose.MouseMove
    '    tssInfo.Text = "Bearbeitungsfenster schliessen"
    'End Sub

    'Private Sub coAnrede_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles coAnrede.MouseMove
    '    tssInfo.Text = "Anrede: Herr, Frau, Firma"
    'End Sub

    'Private Sub coPAnrede_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles coPAnrede.MouseMove
    '    tssInfo.Text = "Anrede: Herr, Frau"
    'End Sub

    'Private Sub tbPName_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbPName.MouseMove
    '    tssInfo.Text = "Nachname"
    'End Sub

    'Private Sub tbPVorname_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbPVorname.MouseMove
    '    tssInfo.Text = "Vorname"
    'End Sub

    'Private Sub tbPInfo_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbPInfo.MouseMove
    '    tssInfo.Text = "Bemerkung ...   Kind, Ehepartner..."
    'End Sub

    'Private Sub dtPPGeb_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dtPPGeb.MouseMove
    '    tssInfo.Text = "Geburtsdatum"
    'End Sub

    'Private Sub cmdNewP_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdNewP.MouseMove
    '    tssInfo.Text = "Partner hinzufügen"
    'End Sub

    'Private Sub cmdDelP_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdDelP.MouseMove
    '    tssInfo.Text = "Partner löschen"
    'End Sub

    'Private Sub cmdSaveP_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdSaveP.MouseMove
    '    tssInfo.Text = "Änderung speichern"
    'End Sub

    'Private Sub dgPartner_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgPartner.MouseMove
    '    tssInfo.Text = ""
    'End Sub

    'Private Sub GroupBox1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GroupBox1.MouseMove
    '    tssInfo.Text = ""
    'End Sub

#End Region

#Region "Listungsübersicht........................................................................."

    ''' <summary>
    ''' Leistungsliste zur Auswahl öffnen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 29.02.2012 Create
    ''' </remarks>
    Private Sub btZusatz_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btZusatz.Click
        paLeistung.Location = New Point(315, 175)
        paLeistung.Visible = Not paLeistung.Visible
        If paLeistung.Visible Then
            lvLeistung.Visible = True
        Else
            lvLeistung.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Mit Doppelklick eine Leistung auswählen und liste schliessen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 29.02.2012 Create
    ''' </remarks>
    Private Sub lvLeistung_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvLeistung.MouseDoubleClick

        With lvLeistung
            If .SelectedItems.Count <> 0 Then
                tbLeistung.Text = .SelectedItems(0).SubItems(0).Text
                tbPreis.Text = .SelectedItems(0).SubItems(1).Text
                coSteuer.Text = .SelectedItems(0).SubItems(2).Text
            End If
        End With
        paLeistung.Visible = False
        lvLeistung.Visible = False
    End Sub

    ''' <summary>
    ''' Leistungsliste schliessen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 29.02.2012 Create
    ''' </remarks>
    Private Sub btCloseLeistung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btCloseLeistung.Click
        paLeistung.Visible = False
        lvLeistung.Visible = False
    End Sub

    Private Sub tbMenge_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbMenge.TextChanged
        If tbMenge.Text.Trim = "" Then tbMenge.Text = "0"
        If IsNumeric(tbMenge.Text) = False Then tbMenge.Text = "0"
        Call prNewCalculateLeistung()
    End Sub

    Private Sub tbPreis_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbPreis.KeyPress
        Call prCheckNumericKey(e.KeyChar, tbPreis)
    End Sub

    Private Sub tbPreis_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbPreis.TextChanged
        'If tbPreis.Text.Trim = "" Then tbPreis.Text = "0"
        'If IsNumeric(tbPreis.Text) = False Then tbPreis.Text = "0,00"
        Call prNewCalculateLeistung()
    End Sub

    Private Sub coSteuer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coSteuer.SelectedIndexChanged
        If coSteuer.Text.Trim = "" Then coSteuer.Text = "0"
        If IsNumeric(coSteuer.Text) = False Then coSteuer.Text = "0"
        Call prNewCalculateLeistung()
    End Sub

    Private Sub prNewCalculateLeistung()

        Dim nMenge As Double = Val(tbMenge.Text)
        Dim nPreis As Double = Val(tbPreis.Text)
        Dim nSteuer As Double = Val(coSteuer.Text)
        Dim nNetto As Decimal = Math.Round((nPreis * 100) / (nSteuer + 100), 2) * nMenge
        ' Dim nMWst As Decimal = (Math.Round(nPreis * nSteuer) / 100) * nMenge
        Dim nMWst As Decimal = (nPreis * nMenge) - nNetto
        '  Dim nNetto As Double = (nPreis * nMenge) - nMWst
        Dim nSumme As Double = nNetto + nMWst
        lbNetto.Text = fcFormatDecimal(nNetto.ToString)
        lbSteuer.Text = fcFormatDecimal(nMWst.ToString)
        lbGesamt.Text = fcFormatDecimal(nSumme.ToString)

    End Sub

#End Region


End Class