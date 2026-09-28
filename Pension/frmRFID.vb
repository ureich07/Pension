Public Class frmRFID
    Dim bNew As Boolean
    ' Dim arTmp() As String
    ' Dim sTmp As String
    Dim sVon As String
    Dim sBis As String
    Dim nSelect As Integer = 2

    Private Sub frmRFID_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        prDisEnebled()
        Me.Width = 1400
        dgZimmerChip.Width = 695
        tsbReturn1.Enabled = False
        tsbSave1.Enabled = False
        prSetTabelleZimCh(dtZim)
        prCreateTabelleRFID()
        prLoadRFIDInList()
        Dim si As String
        For i = 0 To 23
            si = i.ToString
            If si.Length = 1 Then si = "0" & si

            cbBisZeit.Items.Add(si)
            cbVonZeit.Items.Add(si)
        Next

        cbVonZeit.Text = "12"
        cbBisZeit.Text = "12"
        For i = 0 To 55 Step 5
            si = i.ToString
            If si.Length = 1 Then si = "0" & si
            cbBisZeitM.Items.Add(si)
            cbVonZeitM.Items.Add(si)
        Next
        cbVonZeitM.Text = "00"
        cbBisZeitM.Text = "00"
        sVon = fcUmDatum(Mid(mcVon.SelectionStart.ToString, 1, 10))
        sBis = fcUmDatum(Mid(mcBis.SelectionStart.ToString, 1, 10))
    End Sub
    Private Sub prCreateTabelleRFID()
        With lvRFID
            .Clear()
            .Columns.Add("Code", 140, HorizontalAlignment.Left)
            .Columns.Add("Von", 110, HorizontalAlignment.Left)
            .Columns.Add("Zeit", 50, HorizontalAlignment.Left)
            .Columns.Add("Bis", 110, HorizontalAlignment.Left)
            .Columns.Add("Zeit", 50, HorizontalAlignment.Left)
            .Columns.Add("Bemerkung", 100, HorizontalAlignment.Left)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            '.Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub

    ''' <summary>
    ''' Tabelle Zimmer mit daten aus der DataTabel "Zimmer" füllen
    ''' </summary>

    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Sub prLoadRFIDInList()

        Dim i As Integer


        Dim sZeit As String = TimeOfDay.Hour.ToString
        If sZeit.Length = 1 Then sZeit = "0" & sZeit
        Dim dDay As Date = Today
        Dim sTag As String = fcUmDatum(dDay.ToString) ' & sZeit & "00"
        UpdateTable("DELETE FROM Code WHERE bis < '" & sTag & "'") 'löschen alter datensätze
        lvRFID.Items.Clear()
        Dim sSQL As String = "Select * From Code "
        Dim dt As DataTable = fcReadDataTable(sSQL)

        If dt.Rows.Count <> 0 Then
            For i = 0 To dt.Rows.Count - 1
                Dim lv As ListViewItem
                With lvRFID


                    lv = .Items.Add(dt.Rows(i).Item("code"))
                    lv.SubItems.Add(fcUmDatum(dt.Rows(i).Item("von")))
                    lv.SubItems.Add(dt.Rows(i).Item("vonzeit"))
                    lv.SubItems.Add(fcUmDatum(dt.Rows(i).Item("bis")))
                    lv.SubItems.Add(dt.Rows(i).Item("biszeit"))
                    lv.SubItems.Add(dt.Rows(i).Item("BID"))
                End With
            Next
        End If


    End Sub

    Private Sub lvRFID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvRFID.SelectedIndexChanged

        With lvRFID
            If .SelectedItems.Count <> 0 Then
                tbRFID.Text = .SelectedItems(0).SubItems(0).Text
                mcVon.SelectionStart = .SelectedItems(0).SubItems(1).Text
                mcVon.SelectionEnd = .SelectedItems(0).SubItems(1).Text
                cbVonZeit.Text = Mid(.SelectedItems(0).SubItems(2).Text, 1, 2)
                mcBis.SelectionStart = .SelectedItems(0).SubItems(3).Text
                mcBis.SelectionEnd = .SelectedItems(0).SubItems(3).Text
                cbBisZeit.Text = Mid(.SelectedItems(0).SubItems(4).Text, 1, 2)
                tbBemerkung.Text = .SelectedItems(0).SubItems(5).Text
            End If
        End With
    End Sub

    Private Sub tsbNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNew.Click
        bNew = True
        prEnebled()
        tbRFID.Text = ""
        tbBemerkung.Text = ""
        tbRFID.Focus()
        mcVon.SelectionStart = Date.Today.ToString
        mcVon.SelectionEnd = Date.Today.ToString
        mcBis.SelectionStart = Date.Today.AddDays(+1).ToString
        mcBis.SelectionEnd = Date.Today.AddDays(+1).ToString
    End Sub

    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        ' WriteFile("RFID", sTmp)
        'prRFIDSend(sTmp)

        Me.Close()
    End Sub
    Private Sub frmRFID_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        '  SaveOneValueInSystemDb("RFID", sTmp)
        'prRFIDSend(sTmp)
    End Sub
    Private Sub tsbEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEdit.Click
        bNew = False
        prEnebled()
        tbRFID.Enabled = False
    End Sub

    Private Sub tsbReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbReturn.Click
        prDisEnebled()
    End Sub

    Private Sub tsbDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDel.Click




        Dim i As Integer


        Dim sRFID As String = fcWeg34(tbRFID.Text)
        sRFID = tbRFID.Text
        prSendCode(sRFID, "20000101", "1200", "20000202", "1200")
        prSaveSatz(sRFID & "°20000101°1200°20000202°1200°0", False)
        prLoadRFIDInList()
    End Sub

    Private Sub tsbSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSave.Click
        Dim i As Integer
        Dim sRFID As String
        If tbRFID.Text.Length <> 5 Then
            If tbRFID.Text.Length <> 19 Then
                sRFID = fcWeg34(tbRFID.Text)
            Else
                sRFID = tbRFID.Text
            End If
        Else
                sRFID = fcCode(tbRFID.Text.Trim)
        End If
        If bNew = True Then
            Dim sSQL As String = "Select * From Code WHERE code = '" & sRFID & "'"
            Dim dt As DataTable = fcReadDataTable(sSQL)
            If dt.Rows.Count <> 0 Then
                MsgBox("Chip schon vergeben")
                bNew = False
            End If

            Dim Var As String = sRFID & "°" & sVon & "°" & cbVonZeit.Text & cbVonZeitM.Text & "°" & sBis & "°" & cbBisZeit.Text & cbBisZeitM.Text & "°" & tbBemerkung.Text
            prSaveSatz(Var, bNew)
            ' prSendCode(sRFID, sVon, cbVonZeit.Text & cbVonZeitM.Text, sBis, cbBisZeit.Text & cbBisZeitM.Text)

        End If
        prSendCode(sRFID, sVon, cbVonZeit.Text & cbVonZeitM.Text, sBis, cbBisZeit.Text & cbBisZeitM.Text)
        prLoadRFIDInList()
        prDisEnebled()
    End Sub
    Private Sub prSaveSatz(ByRef Var As String, ByRef bNew As Boolean)
        Dim sFeld As String = "code°von°vonzeit°bis°biszeit°BID"
        Dim arFields() As String = Split(sFeld, "°")
        Dim arValue() As String = Split(Var, "°")
        Dim cBedingung As String = ""
        If bNew = True Then
            Call fcInsertCommand("Code", arFields, arValue)
        Else
            cBedingung = " WHERE code='" & arValue(0) & "'"
            Call fcUpdateCommand("Code", arFields, arValue, cBedingung)
        End If
    End Sub

    Private Sub mcVon_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles mcVon.DateChanged
        sVon = fcUmDatum(Mid(mcVon.SelectionStart.ToString, 1, 10))
    End Sub

    Private Sub mcBis_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles mcBis.DateChanged
        sBis = fcUmDatum(Mid(mcBis.SelectionStart.ToString, 1, 10))
    End Sub
    Private Sub prEnebled()
        tbRFID.Enabled = True
        lvRFID.Enabled = False
        mcVon.Enabled = True
        mcBis.Enabled = True
        cbBisZeit.Enabled = True
        cbVonZeit.Enabled = True
        cbBisZeitM.Enabled = True
        cbVonZeitM.Enabled = True
        tbBemerkung.Enabled = True
    End Sub
    Private Sub prDisEnebled()
        tbRFID.Enabled = False
        dgZimmerChip.Enabled = False
        lvRFID.Enabled = True
        mcVon.Enabled = False
        mcBis.Enabled = False
        cbBisZeit.Enabled = False
        cbVonZeit.Enabled = False
        cbBisZeitM.Enabled = False
        cbVonZeitM.Enabled = False
        tbBemerkung.Enabled = False
    End Sub


    Private Sub tsbReadRFID_Click(sender As Object, e As EventArgs) Handles tsbReadRFID.Click
        frmReadRFID.Show()

    End Sub

    Private Sub tsbSchlossSet_Click(sender As Object, e As EventArgs) Handles tsbSchlossSet.Click
        frmSchloss.Show()
    End Sub






    ''' <summary>
    ''' Tabelle ZimmerChip erstellen
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks>
    ''' 24.12.2011 Create
    ''' </remarks>
    Private Sub prSetTabelleZimCh(ByVal dt As DataTable)
        Dim nMax As Integer = dt.Rows.Count - 1
        With dgZimmerChip
            .MultiSelect = False
            .Columns.Clear()
            .ColumnHeadersHeight = 30
            .Columns.Add("Zimmer", "Zimmer")
            .Columns.Add("Chip1", "Chip1")
            .Columns.Add("Chip2", "Chip2")
            .Columns.Add("Chip3", "Chip3")
            .Columns.Add("Chip4", "Chip4")
            .Columns.Add("Chip5", "Chip5")
            .Columns(0).Width = 70
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            For i = 1 To 5
                .Columns(i).Width = 120
                .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Next
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AutoResizeRows()
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            '.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            ' Farben der selektierten Zeile
            With .DefaultCellStyle
                .SelectionBackColor = cgColorRow 'Color.GreenYellow
                .SelectionForeColor = Color.Black
            End With
            .ReadOnly = True
            For i = 0 To nMax
                .Rows.Add("")
                .Rows(i).Cells(0).Value = dt.Rows(i).Item("Name").ToString
                .Rows(i).Cells(1).Value = dt.Rows(i).Item("Trans1").ToString
                .Rows(i).Cells(2).Value = dt.Rows(i).Item("Trans2").ToString
                .Rows(i).Cells(3).Value = dt.Rows(i).Item("Trans3").ToString
                .Rows(i).Cells(4).Value = dt.Rows(i).Item("Trans4").ToString
                .Rows(i).Cells(5).Value = dt.Rows(i).Item("Trans5").ToString
            Next
            .MultiSelect = True
        End With
    End Sub
    ''' <summary>
    ''' Buchungseintrag im Datagridview auswählen und Buchungsnummer ermitteln
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 27.12.2011 Create
    ''' </remarks>
    Private Sub dgZimmerChip_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgZimmerChip.CellEnter
        If nSelect <> 2 Then
            Dim sTrans As String = ""
            Try
                If e.ColumnIndex = 0 Then
                    For i = 1 To 5
                        If dgZimmerChip.Rows(e.RowIndex).Cells(1).Style.BackColor = Color.YellowGreen Then
                            nSelect = 1
                        End If
                    Next
                    If nSelect = 0 Then
                        For i = 1 To 5
                            dgZimmerChip.Rows(e.RowIndex).Cells(i).Style.BackColor = Color.YellowGreen
                            sTrans = dgZimmerChip.Rows(e.RowIndex).Cells(i).Value
                            dgZimmerChip.Rows(e.RowIndex).Cells(i).Value = "x" & sTrans
                        Next
                    Else
                        For i = 1 To 5
                            dgZimmerChip.Rows(e.RowIndex).Cells(i).Style.BackColor = Color.White
                            sTrans = dgZimmerChip.Rows(e.RowIndex).Cells(i).Value
                            If Mid(sTrans, 1, 1) = "x" Then
                                dgZimmerChip.Rows(e.RowIndex).Cells(i).Value = Mid(sTrans, 2)
                            End If
                        Next
                    End If
                Else
                    If dgZimmerChip.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.YellowGreen Then
                        dgZimmerChip.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.White
                        sTrans = dgZimmerChip.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                        If Mid(sTrans, 1, 1) = "x" Then
                            dgZimmerChip.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Mid(sTrans, 2)
                        End If
                    Else
                        dgZimmerChip.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.YellowGreen
                        sTrans = dgZimmerChip.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                        dgZimmerChip.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = "x" & sTrans
                    End If
                End If
            Catch ex As Exception
                ErrReport(ex.Message, ex.Source, ex.StackTrace)
            End Try
        End If
        nSelect = 0
    End Sub

    Private Sub tsbSammelChip_Click(sender As Object, e As EventArgs) Handles tsbSammelChip.Click
        If tsbNew.Enabled = True Then
            tsbNew.Enabled = False
            tsbEdit.Enabled = False
            tsbDel.Enabled = False
            tsbReturn.Enabled = False
            tsbSave.Enabled = False

            tsbReturn1.Enabled = True
            tsbSave1.Enabled = True

            dgZimmerChip.Enabled = True
            mcVon.Enabled = True
            mcBis.Enabled = True
            cbBisZeit.Enabled = True
            cbBisZeitM.Enabled = True
            cbVonZeit.Enabled = True
            cbVonZeitM.Enabled = True

        Else
            tsbNew.Enabled = True
            tsbEdit.Enabled = True
            tsbDel.Enabled = True
            tsbReturn.Enabled = True
            tsbSave.Enabled = True

            tsbReturn1.Enabled = False
            tsbSave1.Enabled = False

            dgZimmerChip.Enabled = False
            mcVon.Enabled = False
            mcBis.Enabled = False
            cbBisZeit.Enabled = False
            cbBisZeitM.Enabled = False
            cbVonZeit.Enabled = False
            cbVonZeitM.Enabled = False
        End If
    End Sub

    Private Sub tsbReturn1_Click(sender As Object, e As EventArgs) Handles tsbReturn1.Click
        Dim nMax As Integer = dtZim.Rows.Count - 1
        Dim sTrans As String
        For i = 0 To nMax
            For j = 1 To 5
                sTrans = dgZimmerChip.Rows(i).Cells(j).Value
                If Mid(sTrans, 1, 1) = "x" Then
                    dgZimmerChip.Rows(i).Cells(j).Value = Mid(sTrans, 2)
                End If
            Next

        Next
    End Sub

    Private Sub tsbSave1_Click(sender As Object, e As EventArgs) Handles tsbSave1.Click
        Dim nMax As Integer = dtZim.Rows.Count - 1
        Dim sTrans As String
        Dim sCodeNew1 As String = "°" & sVon & "°" & cbVonZeit.Text & cbVonZeitM.Text & "°" & sBis & "°" & cbBisZeit.Text & cbBisZeitM.Text & "°"
        Dim sCodeNew As String
        sgCodeNew = ""

        For i = 0 To nMax
            For j = 1 To 5
                sTrans = dgZimmerChip.Rows(i).Cells(j).Value
                If Mid(sTrans, 1, 1) = "x" Then
                    If Trim(Mid(sTrans, 2)) <> "" Then
                        sTrans = Mid(sTrans, 2).Trim
                        sCodeNew = sTrans & sCodeNew1 & dgZimmerChip.Rows(i).Cells(0).Value
                        bNew = True
                        Dim sSQL As String = "Select * From Code WHERE code = '" & sTrans & "'"
                        Dim dt As DataTable = fcReadDataTable(sSQL)
                        If dt.Rows.Count <> 0 Then
                            bNew = False
                        End If
                        prSaveSatz(sCodeNew, bNew)
                    End If
                End If
            Next

        Next
        prSendCodeAll()
        prLoadRFIDInList()
        tsbNew.Enabled = True
        tsbEdit.Enabled = True
        tsbDel.Enabled = True
        tsbReturn.Enabled = True
        tsbSave.Enabled = True
        tsbReturn1.Enabled = False
        tsbSave1.Enabled = False

        dgZimmerChip.Enabled = False
        mcVon.Enabled = False
        mcBis.Enabled = False
        cbBisZeit.Enabled = False
        cbBisZeitM.Enabled = False
        cbVonZeit.Enabled = False
        cbVonZeitM.Enabled = False
    End Sub

    Private Sub prSendCode(ByRef sc1 As String, ByVal von As String, ByRef vonzeit As String, ByRef bis As String, ByRef biszeit As String)
        Me.Cursor = Cursors.WaitCursor
        Dim C As String = sc1 & "°" & von & "°" & vonzeit & "°" & bis & "°" & biszeit & "|"
        Dim sIP As String = cgIPWeb
        PHP.Data(C, sIP)
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub prSendCodeAll()
        Me.Cursor = Cursors.WaitCursor
        Dim sIP As String = cgIPWeb
        PHP.DataAll(sIP)
        Me.Cursor = Cursors.Default
    End Sub

    'Private Sub prRFIDSend(ByRef sT As String)
    '    Me.Cursor = Cursors.WaitCursor
    '    Dim save As String = sT
    '    arTmp = Split(sT, vbCrLf) 'vbCrLf
    '    Dim i As Integer
    '    Dim nMax As Integer = arTmp.Length - 1
    '    Dim sT1 As String
    '    If nMax <> 0 Then
    '        Dim arZeile() As String
    '        sT = ""
    '        For i = 0 To nMax - 1
    '            arZeile = Split(arTmp(i), "#")
    '            sT1 = ""
    '            sT1 = fcRFIDDatum(arZeile(1)) & ";"
    '            sT1 = sT1 & fcRFIDZeit(arZeile(2)) & ";"
    '            sT1 = sT1 & fcRFIDDatum(arZeile(3)) & ";"
    '            sT1 = sT1 & fcRFIDZeit(arZeile(4))
    '            sT1 = fcUmCode(sT1) & "|" 'vbCrLf
    '            sT = sT & arZeile(0).Trim & ";" & sT1
    '        Next
    '    End If
    '    If False = PHP.Data(sT, arIni(34)) Then
    '        MsgBox("Daten nicht Gesendet")
    '    End If
    '    sT = save
    '    Me.Cursor = Cursors.Arrow
    'End Sub
End Class