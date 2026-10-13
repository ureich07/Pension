Public Class frmPauschal
    Dim arG(7) As String
    Dim sRNr As String
    Dim sRdatum As String
    Dim bNewRNr As Boolean = False
    Dim sGKNr As String
    Dim nAnzahlung As Integer
    Private Sub frmPauschal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Cursor = Cursors.WaitCursor
        Me.BackColor = Color.LightYellow
        buMail.Enabled = False
        '  Call Main()
        Call prLoadZimmer(sgRBID)
        lbBID.Text = sgRBID
        Me.Cursor = Cursors.Default
        sGKNr = sgGID
        cbBar.Checked = False
    End Sub
    ''' <summary>
    ''' Zimmer der Buchung laden
    ''' </summary>
    ''' <param name="sBid"></param>
    ''' <remarks></remarks>
    Private Sub prLoadZimmer(ByVal sBid As String)
        Dim sSQL As String = "Select * from Buchung Where BID='" & sBid & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)
        Dim nMax As Integer = dt.Rows.Count - 1
        sRdatum = dt.Rows(0).Item("RADatum").ToString  'rechnungsdatum
        nAnzahlung = 0
        lbRech.Text = dt.Rows(0).Item("RAID").ToString
        For nI = 0 To nMax
            nAnzahlung = nAnzahlung + Val(dt.Rows(nI).Item("Anzahlung").ToString)
        Next
        If sRdatum.Trim = "" Then sRdatum = fcUmDatum(Date.Today)
        dtpARDatum.Value = fcUmDatum(sRdatum)
        If lbRech.Text.Trim = "" Then
            bNewRNr = True
            sRNr = fcGetNr("RNr")
            lbRech.Text = Date.Today.Year & "-" & sRNr
        End If
    End Sub
 
  
    ' ''' <summary>
    ' ''' Druck Rechnung
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks>
    ' ''' 11.04.2012 Create
    ' ''' </remarks>
    'Private Sub buDruck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buDruck.Click
    '    Dim arDruck(1) As String
    '    Dim sZeile As String = ""
    '    Dim arfeld3(3) As String
    '    prVpeOpen()

    '    For i = 0 To dgRechnung.Rows.Count - 1
    '        If dgRechnung.Rows(i).Cells(9).Value = "0" Then
    '            sZeile = ""
    '            For j = 1 To 13
    '                sZeile = sZeile + dgRechnung.Rows(i).Cells(j).Value + "#"
    '            Next
    '            ReDim Preserve arDruck(i)
    '            sZeile = Mid(sZeile, 1, sZeile.Length - 1)
    '            arDruck(i) = sZeile
    '        End If
    '    Next
    '    ' biD GastID,RechnungsID,Pararry,Bar
    '    Call prDruckRechnung(sgRBID, sgGID, tbRNr.Text, sRdatum, cbBar.Checked, nAnzahlung, sRDSenden, arDruck)
    '    Dim cBedingung As String
    '    arfeld3(0) = tbRNr.Text 'Rechnungsmummer
    '    arfeld3(1) = sRdatum  'Rechnungsdatum
    '    arfeld3(2) = "-1"  'Rechnung geschrieben
    '    arfeld3(3) = sgGID 'kunden ID
    '    For i = 0 To dgRechnung.Rows.Count - 1
    '        cBedingung = " WHERE ID='" & dgRechnung.Rows(i).Cells(14).Value & "'" 'feld 14 sind die Id für die Buchungen
    '        Call fcUpdateCommand("Buchung", {"RId", "RDatum", "Rechnung", "kunID"}, arfeld3, cBedingung)
    '    Next
    '    If bNewRNr = True Then prSetNr("RNr", fcGetNr("RNr")) 'neue rechnungsnummer

    '    arfeld3 = fcDataSeek("select * From Bewertung Where BuchID ='", sgRBID, 0, {"ID"}) 'kontrolle ob schon geschriben
    '    If arfeld3(0) = " " Then
    '        '  If CheckBox1.Checked = True Then
    '        ' If MsgBox(" Bewertungs Mail schreiben ?", vbExclamation + vbOKCancel, "Schreiben") = MsgBoxResult.Ok Then
    '        ' Call fcInsertCommand("Bewertung", {"Id", "BuchID", "Status"}, {fcGetTimeID(Date.Today), sgRBID, "-1"})
    '        ' End If
    '    End If
    '    prVpeVeiw()

    '    Dim ii As Integer = 0

    '    buMail.Enabled = True
    'End Sub

    Private Sub buMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buMail.Click
        Dim arDruck(1) As String
        Dim sZeile As String = ""
        Dim arfeld3(3) As String

        Dim ii As Integer = 0

        If FileExists(arIni(32) & "\Rech_" & lbRech.Text & ".PDF") = False Then
            MsgBox("Rechnung noch nicht Erstelt", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        Dim sText As Array = Split(ReadFile(cgSystemPath & "\Mail_Rechnung.ini"), "#")
        Dim sDatei As String = arIni(32) & "\Rech_" & sgRNr & ".PDF"
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
            If fcSendeMailAnlage(arFeld(0), "Rechnung", sText(ii), , , sDatei) = True Then
                MsgBox("Rechnungs Mail Erfolgreich gesendet", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Else
                MsgBox(" F e h l e r  Rechnungs Mail ", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            End If
        Else
            MsgBox(" F e h l e r  Keine Mailadresse", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
        End If
        Me.Close()
    End Sub


    Private Sub buClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buClose.Click
        Me.Close()
    End Sub


End Class