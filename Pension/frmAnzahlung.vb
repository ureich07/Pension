Public Class frmAnzahlung
    Dim arG(7) As String
    Dim sRNr As String
    Dim sRdatum As String
    Dim bNewRNr As Boolean = False
    Dim sGKNr As String
    Dim nAnzahlung As Integer
    Dim nSprache As Integer
    Private Sub frmAnzahlung_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

    ''' <summary>
    ''' Druck Rechnung
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 11.04.2012 Create
    ''' </remarks>
    Private Sub buDruck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buDruck.Click
        prDruck(sgRBID, lbRech.Text, nSprache)
    End Sub
    Private Sub prDruck(ByRef sBid As String, ByRef sRnrID As String, ByRef nSprache As Integer)
        Dim arDruck(1) As String
        Dim sZeile As String = ""
        Dim arfeld3(1) As String
        Dim sSQL As String = "Select * from Buchung Where BID='" & sBid & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)
        Dim nMax As Integer = dt.Rows.Count - 1
        prVpeOpen()

        Call prDruckAnzahlung(sBid, sRnrID, dtpARDatum.Value, cbBar.Checked, nAnzahlung, dtpARDatum.Value, nSprache)
        'Call prDruckRechnung(sgRBID, sgGID, tbRNr.Text, sRdatum, cbBar.Checked, nAnzahlung, sRDSenden, arDruck)
        Dim cBedingung As String
        arfeld3(0) = lbRech.Text 'Rechnungsmummer
        arfeld3(1) = sRdatum  'Rechnungsdatum
        'arfeld3(2) = "-1"  'Rechnung geschrieben
        'arfeld3(3) = sgGID 'kunden ID

        For i = 0 To nMax
            cBedingung = " WHERE ID='" & dt.Rows(i).Item("ID").ToString & "'" 'feld 14 sind die Id für die Buchungen
            Call fcUpdateCommand("Buchung", {"RAID", "RADatum"}, arfeld3, cBedingung)
        Next
        If bNewRNr = True Then prSetNr("RNr", fcGetNr("RNr")) 'neue rechnungsnummer
        prVpeVeiw()
        buMail.Enabled = True

    End Sub


    Private Sub buMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buMail.Click
        Dim arDruck(1) As String
        Dim sZeile As String = ""
        Dim arfeld3(3) As String
      
        Dim ii As Integer = 0
        
        If FileExists(arIni(32) & "\Rech_" & lbRech.Text & ".PDF") = False Then
            MsgBox("Rechnung noch nicht Erstelt", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        Dim sText As Array = Split(ReadOneValueFromSystemDb("Mail_Rechnung"), "#")
        Dim sPdf As String = arIni(32) & "\Rech_" & sgRNr & ".PDF"
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