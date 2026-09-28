Imports System.Text

Public Class frmTermine

    Dim dtE As DataTable
    Dim sTID As String
    Dim lNew As Boolean
    Dim sZim As String
    Dim sAktiv As String
    Dim sBID As String
    Dim bFilter As Boolean = True
    Dim sDatum As String = fcUmDatum(Date.Today)
    Dim sTerminText As String
#Region "Formular initialisieren..................................................................."

    Private Sub frmTermine_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sIDDatum As String = fcUmDatum(DateAdd(DateInterval.Day, -400, Date.Today)) & "000000"
        Dim cSql As String = "DELETE FROM TERMINE WHERE ID < '" & sIDDatum & "'"
        Dim sColor As Color = Color.LightYellow
        tsMain.BackColor = sColor
        Me.BackColor = sColor
        gbZusatz.BackColor = sColor
        paEingabe.BackColor = sColor
        'lbDatum.Text = Date.Today
        'sZim = frmReservierung.lbZimNr.Text

        dtE = fcReadDataTable("Select * from Termine Where Termin>='" & sDatum & "' Order by Termin asc") ' Where BID='" & sgRBID & "'")
        Call prSetTable()
        Call prFuelleTabelle(dtE)
        Me.Cursor = Cursors.Default
    End Sub

    ''' <summary>
    ''' Create Tabelle 
    ''' </summary>
    ''' <remarks>
    ''' 04.04.2012 Create 
    ''' </remarks>
    Private Sub prSetTable()
        With dgTermin
            .Columns.Clear()
            .ColumnHeadersHeight = 25
            .Columns.Add("", "ID")
            .Columns.Add("", "Datum")
            .Columns.Add("", "Eintrag")
            .Columns.Add("", "Termin")
            .Columns.Add("", "Zeit")
            .Columns.Add("", "Aktive")
            .Columns.Add("", "BID")
            .Columns.Add("", "Zimmer")
            .Columns(0).Visible = False
            .Columns(1).Width = 80
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(2).Width = 300
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(3).Width = 80
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(4).Width = 60
            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(5).Width = 50
            .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(6).Width = 50
            .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            '.Columns(6).Visible = False
            .Columns(7).Width = 50
            .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            '.Columns(7).Visible = False

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
    ''' Tabelle mit Daten füllen
    ''' </summary>
    ''' <remarks>
    ''' 04.04.2012 Create 
    ''' </remarks>
    Private Sub prFuelleTabelle(ByVal dt As DataTable)
        If dt.Rows.Count = 0 Then Exit Sub
        Dim nMax As Integer = dt.Rows.Count - 1
        Dim sb As New StringBuilder
        Dim arT() As String
        Dim sA As String

        Try
            With dgTermin
                .Rows.Clear()
                For i As Integer = 0 To nMax
                    If dt.Rows(i).RowState <> DataRowState.Deleted Then
                        If bFilter = False Then
                            sb.Append(dt.Rows(i).Item("ID").ToString & ";")
                            sb.Append(fcUmDatum(dt.Rows(i).Item("Datum").ToString) & ";")
                            sb.Append(dt.Rows(i).Item("TerminText").ToString & ";")
                            sb.Append(fcUmDatum(dt.Rows(i).Item("Termin").ToString) & ";")
                            sb.Append(dt.Rows(i).Item("Zeit").ToString & ";")
                            sA = dt.Rows(i).Item("Aktive").ToString
                            If sA = "0" Then
                                sb.Append("Nein" & ";")
                            Else
                                sb.Append("Ja" & ";")
                            End If
                            sb.Append(dt.Rows(i).Item("BID").ToString & ";")
                            sb.Append(dt.Rows(i).Item("Zimmer").ToString & ";")

                            arT = sb.ToString.Split(";")
                            .Rows.Add(arT)
                            sb.Remove(0, sb.Length)
                        Else
                            If dt.Rows(i).Item("Datum").ToString <= sDatum Then
                                sb.Append(dt.Rows(i).Item("ID").ToString & ";")
                                sb.Append(fcUmDatum(dt.Rows(i).Item("Datum").ToString) & ";")
                                sb.Append(dt.Rows(i).Item("TerminText").ToString & ";")
                                sb.Append(fcUmDatum(dt.Rows(i).Item("Termin").ToString) & ";")
                                sb.Append(dt.Rows(i).Item("Zeit").ToString & ";")
                                sA = dt.Rows(i).Item("Aktive").ToString
                                If sA = "0" Then
                                    sb.Append("Nein" & ";")
                                Else
                                    sb.Append("Ja" & ";")
                                End If
                                sb.Append(dt.Rows(i).Item("BID").ToString & ";")
                                sb.Append(dt.Rows(i).Item("Zimmer").ToString & ";")

                                arT = sb.ToString.Split(";")
                                .Rows.Add(arT)
                                sb.Remove(0, sb.Length)
                            End If
                        End If
                    End If
                Next
            End With
            If dgTermin.Rows.Count = 0 Then tsbDelete.Enabled = False
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

#End Region

#Region "Buttons und Menü-Ereignisse..............................................................."

    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        Call prCloseForm()
    End Sub
    Private Sub prCloseForm()
        Me.Close()
    End Sub

    Private Sub dgTermin_CellDoubleClick(ByVal sender As Object, _
                ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
                Handles dgTermin.CellDoubleClick
        Dim sA As String
        paEingabe.Visible = True
        lNew = False
        dgTermin.Enabled = False
        With dgTermin.Rows(e.RowIndex)

            sTID = .Cells(0).Value
            lbDatum.Text = .Cells(1).Value
            tbEintrag.Text = .Cells(2).Value
            dtpDatum.Text = .Cells(3).Value
            dtpTime.Text = .Cells(4).Value
            sA = .Cells(5).Value
            chAktive.CheckState = CheckState.Unchecked
            If sA = "Ja" Then
                chAktive.CheckState = CheckState.Checked
            End If
            sBID = .Cells(6).Value
            sZim = .Cells(7).Value
        End With
    End Sub

    Private Sub tsbNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNew.Click
        lNew = True
        sBID = " "
        sZim = " "
        lbDatum.Text = Date.Today
        tbEintrag.Text = " "
        dtpDatum.Value = Date.Today
        dtpTime.Value = Date.Today

        chAktive.CheckState = CheckState.Unchecked
        paEingabe.Visible = True
    End Sub

    Private Sub tsbDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelete.Click
        Dim sMsg As String = "Wollen Sie diese Erinnerung löschen?  "

        Dim cSql As String = "DELETE FROM Termine WHERE ID = '" & sTID & "'"
        If sTID = "" Then Exit Sub
        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            'Datensatz per SQL aus der Tabelle löschen
            UpdateTable(cSql)
            'Änderung in DataTable "dtE" speichern
            Call fcDeleteTableRow(dtE, "ID = '" & sTID & "'")
            dgTermin.Select()
            Call prFuelleTabelle(dtE)
        End If
    End Sub
    Private Sub tsbFilter_Click(sender As System.Object, e As System.EventArgs) Handles tsbFilter.Click
        If bFilter = False Then
            bFilter = True
        Else
            bFilter = False
        End If
         prFuelleTabelle(dtE)
    End Sub

    Private Sub tsbMail_Click(sender As System.Object, e As System.EventArgs) Handles tsbMail.Click
        'If lbEMail.Text <> "" And Extract(sTerminText, "[", "]", 1) <> "" Then
        '    Dim sInhalt As Array = Split(ReadFileUniCode(cgSystemPath & "\Mail_Code.ini"), "#")
        '    sInhalt(0) = exchange(sInhalt(0), "{", "}", Extract(sTerminText, "[", "]", 1), 1)
        '    If fcSendeMailAnlage(lbEMail.Text, "Tür - Schlüssel", sInhalt(0)) = True Then
        '        Dim arFields(1) As String
        '        Dim arValue(1) As String

        '        arFields(0) = "Aktive"
        '        arFields(1) = "TerminText"

        '        arValue(0) = "0"
        '        arValue(1) = sTerminText
        '        arValue(1) = exchange(arValue(1), "(", ")", "(Gesendet" & Date.Today & ")", 1)
        '        Call fcUpdateCommand("Termine", arFields, arValue, " WHERE ID='" & sTID & "'")
        '        dtE = fcReadDataTable("Select * from Termine Where Termin>='" & sDatum & "' Order by Termin asc") ' Where BID='" & sgRBID & "'")
        '        prFuelleTabelle(dtE)
        '        MsgBox("Buchungs Mail Erfolgreich gesendet", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
        '    Else
        '        MsgBox(" F e h l e r  Buchungs Mail ", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
        '        '  dgTermin.Rows(e.RowIndex).Cells(5).Value = "Nein"
        '    End If





        'End If
    End Sub

    Private Sub dgTermin_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgTermin.CellEnter
        sTID = dgTermin.Rows(e.RowIndex).Cells(0).Value
        sAktiv = dgTermin.Rows(e.RowIndex).Cells(5).Value
        sBID = dgTermin.Rows(e.RowIndex).Cells(6).Value
        sTerminText = dgTermin.Rows(e.RowIndex).Cells(2).Value
        Dim sKnr As String = fcGetKNr(sBID)
        Call prDisplayGast(sKnr)
    End Sub

    Private Function fcGetKNr(ByVal sBNr As String) As String
        fcGetKNr = ""
        lbVon.Text = ""
        lbBis.Text = ""
        liZimmer.Items.Clear()
        Dim sSQL As String = "Select * From Buchung Where BID='" & sBNr & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)
        If dt.Rows.Count > 0 Then
            lbVon.Text = fcUmDatum(dt.Rows(0).Item("Von").ToString)
            lbBis.Text = fcUmDatum(dt.Rows(0).Item("Bis").ToString)

            fcGetKNr = dt.Rows(0).Item("KunID").ToString
            For i As Integer = 0 To dt.Rows.Count - 1
                liZimmer.Items.Add(fcGetObjektZimmerName(dtZim, dt.Rows(i).Item("ZimID").ToString))
            Next
        End If



    End Function

    Private Sub prDisplayGast(ByVal sKNr As String)
        Try
            lbName.Text = ""
            lbName2.Text = ""
            lbVorname.Text = ""
            lbTel.Text = ""
            lbEMail.Text = ""

            Dim sSQL As String = "Select * From Kunden Where ID='" & sKNr & "'"
            Dim dt As DataTable = fcReadDataTable(sSQL)
            If dt.Rows.Count > 0 Then
                lbName.Text = dt.Rows(0).Item("Name1").ToString
                lbName2.Text = dt.Rows(0).Item("Name2").ToString
                lbVorname.Text = dt.Rows(0).Item("Vorname").ToString
                lbTel.Text = dt.Rows(0).Item("Telefon").ToString & " / " & dt.Rows(0).Item("Funk").ToString
                lbEMail.Text = dt.Rows(0).Item("EMail").ToString
            End If
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

#End Region



#Region "Termine bearbeiten........................................................................"
    'sSt = sSt & "<Termine>" & vbCrLf
    '  sSt = sSt & "ID, dbText,16" & vbCrLf
    '  sSt = sSt & "Datum, dbText,8" & vbCrLf
    '  sSt = sSt & "TerminText, dbText,255" & vbCrLf
    '  sSt = sSt & "Termin, dbText,8" & vbCrLf
    '  sSt = sSt & "Zeit, dbText,5" & vbCrLf
    '  sSt = sSt & "Aktive, dbText,1" & vbCrLf
    '  sSt = sSt & "BID, dbText,16" & vbCrLf
    '  sSt = sSt & "Zimmer, dbText,16" & vbCrLf
    '  sSt = sSt & "<End>" & vbCrLf

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Call prClose()
    End Sub
    Private Sub prClose()
        Call prLoockZusatz(False)
        paEingabe.Visible = False
        dgTermin.Enabled = True
        dgTermin.Select()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Call prSaveTermin()
        Call prClose()
    End Sub

    ''' <summary>
    ''' Speicherung durchführen
    ''' </summary>
    ''' <remarks>
    ''' 04.04.2012 Create
    ''' </remarks>
    Private Sub prSaveTermin()
        If tbEintrag.Text.Trim = "" Then
            MsgBox("Kein Termintext eingegeben!.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Speichern")
            Exit Sub
        End If
        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String = ""
        Dim sID As String = sTID
        If lNew Then sID = fcGetTimeID(Date.Today)
        ' Dim nSumme As Double = 0
        Try

            sqlText = "ID,Datum,TerminText,Termin,Zeit,Aktive,BID,Zimmer"
            arFields = Split(sqlText, ",")
            sqlText = fcSaveTermin(sID)
            arValue = Split(sqlText, "°")

            If lNew Then
                Call fcInsertCommand("Termine", arFields, arValue)
            Else
                cBedingung = " WHERE ID='" & sID & "'"
                Call fcUpdateCommand("Termine", arFields, arValue, cBedingung)
            End If
            'DataTable aktualisieren
            If lNew Then
                'Datensatz in DataTable "dtE" speichern
                Call fcInsertTable(dtE, arFields, arValue)
            Else
                'Datensatz in DataTable "dtZ" speichern
                cBedingung = "ID Like '" & sID & "'"
                Call fcUpdateTable(dtE, arFields, arValue, cBedingung)
            End If

            Call prFuelleTabelle(dtE)

            lNew = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            Call prLoockZusatz(False)
            lNew = False

        End Try
    End Sub

    ''' <summary>
    ''' Zu speichernde Daten aufbereiten
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 04.04.2012 Create
    ''' </remarks>
    Private Function fcSaveTermin(ByVal sID As String) As String
        Dim sb As New StringBuilder
        Dim sGeb As String = " "
        ' "ID,Datum,TerminText,Termin,Zeit,Aktive,BID,Zimmer"


        sb.Append(sID & "°")
        sb.Append(fcUmDatum(lbDatum.Text) & "°")
        sb.Append(tbEintrag.Text & "°")
        sb.Append(fcUmDatum(dtpDatum.Value) & "°")
        sb.Append(dtpTime.Text & "°")
        sb.Append(chAktive.CheckState & "°")
        sb.Append(sBID & "°")
        sb.Append(sZim)
        fcSaveTermin = sb.ToString
    End Function

    ''' <summary>
    ''' Steuerung der Button und Eingabefelder
    ''' </summary>
    ''' <param name="lStatus"></param>
    ''' <remarks>
    ''' 04.04.2012 Create
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



    
   
  
End Class