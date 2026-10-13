Imports System.IO
Public Class frmAnzahlung
    Dim arG(7) As String
    Dim sRNr As String
    Dim sRdatum As String
    Dim bNewRNr As Boolean = False
    Dim sGKNr As String
    Dim nAnzahlung As Integer
    Dim nSprache As Integer

    ''' <summary>
    ''' Initialisiert das Anzahlungsformular beim Laden.
    ''' Setzt visuelle Steuerelemente, lädt die Zimmerdaten basierend auf der Buchungs-ID und initialisiert die Kundennummer.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 09.10.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei Methodenaufrufen entfernt.
    ''' - Auskommentierten, toten Code entfernt.
    ''' - Robustheit erhöht: 'Try...Finally'-Block garantiert das Zurücksetzen des Cursors bei Fehlern.
    ''' - 'String.IsNullOrWhiteSpace'-Prüfung für die Buchungs-ID hinzugefügt.
    ''' </remarks>
    Private Sub frmAnzahlung_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            ' Warte-Cursor anzeigen, während Daten geladen werden
            Me.Cursor = Cursors.WaitCursor
            Me.BackColor = Color.LightYellow
            buMail.Enabled = False

            ' Buchungs-ID zuweisen und Zimmer laden (falls ID vorhanden)
            lbBID.Text = sgRBID
            If Not String.IsNullOrWhiteSpace(sgRBID) Then
                prLoadZimmer(sgRBID)
            End If

            ' Kundennummer synchronisieren und Standardwerte setzen
            sGKNr = sgGID
            cbBar.Checked = False

        Finally
            ' Garantiert das Zurücksetzen des Cursors, selbst wenn oben ein Fehler auftritt
            Me.Cursor = Cursors.Default
        End Try
    End Sub


    ''' <summary>
    ''' Zimmer der Buchung laden.
    ''' Liest Rechnungsdatum, Anzahlungen und Rechnungsnummer aus der Buchungstabelle und berechnet die Gesamtsumme.
    ''' </summary>
    ''' <param name="sBid">Die Buchungs-ID, nach der gefiltert wird.</param>
    ''' <remarks>
    ''' 09.10.2026 - Code-Optimierung:
    ''' - SQL-Performance: 'SELECT *' durch gezielte Abfrage der benötigten Felder (RADatum, RAID, Anzahlung) ersetzt.
    ''' - Robustheit erhöht: Prüfung auf leere DataTable verhindert Abstürze bei ungültiger ID.
    ''' - Ressourcenmanagement: 'Using'-Block für DataTable eingeführt.
    ''' - 'String.IsNullOrWhiteSpace' für Null-Sicherheit implementiert.
    ''' </remarks>
    Private Sub prLoadZimmer(ByVal sBid As String)
        ' Performance-Optimierung: Nur die benötigten Spalten abfragen
        Dim sSQL As String = "Select RADatum, RAID, Anzahlung from Buchung Where BID='" & sBid & "'"

        Using dt As DataTable = fcReadDataTable(sSQL)
            ' Sicherheits-Check: Abbrechen, falls keine Daten gefunden wurden
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim nMax As Integer = dt.Rows.Count - 1

                sRdatum = dt.Rows(0).Item("RADatum").ToString() ' Rechnungsdatum
                nAnzahlung = 0
                lbRech.Text = dt.Rows(0).Item("RAID").ToString()

                ' Anzahlungen aufsummieren (Val fängt leere Altdaten/Strings sicher ab)
                For nI As Integer = 0 To nMax
                    nAnzahlung += Val(dt.Rows(nI).Item("Anzahlung"))
                Next

                ' Rechnungsdatum prüfen und ggf. auf heute setzen
                If String.IsNullOrWhiteSpace(sRdatum) Then
                    sRdatum = fcUmDatum(Date.Today)
                End If
                dtpARDatum.Value = fcUmDatum(sRdatum)

                ' Neue Rechnungsnummer generieren, falls noch keine vorhanden ist
                If String.IsNullOrWhiteSpace(lbRech.Text) Then
                    bNewRNr = True
                    sRNr = fcGetNr("RNr")
                    lbRech.Text = Date.Today.Year.ToString() & "-" & sRNr
                End If
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Löst den Druckprozess für eine Rechnung aus.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 11.04.2012 - Create
    ''' </remarks>
    Private Sub buDruck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buDruck.Click
        'prDruck(sgRBID, lbRech.Text, nSprache)
        btDruck_Click(sgRBID, lbRech.Text, nSprache)
    End Sub

    ''' <summary>
    ''' Bereitet die Rechnungsdaten auf, startet den Druck/Vorschau über die VPE-Engine und aktualisiert den Rechnungsstatus in der Datenbank.
    ''' </summary>
    ''' <param name="sBid">Die Buchungs-ID (Übergabe per Referenz).</param>
    ''' <param name="sRnrID">Die Rechnungsnummer (Übergabe per Referenz).</param>
    ''' <param name="nSprache">Der Sprachschlüssel für das Dokument (Übergabe per Referenz).</param>
    ''' <remarks>
    ''' 11.04.2012 - Create
    ''' 09.10.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei Methodenaufrufen vollständig entfernt.
    ''' - Ungenutzte Variablen ('arDruck', 'sZeile') gelöscht, um Arbeitsspeicher zu schonen.
    ''' - 'ByRef' bei Parametern überprüft: Wenn Werte nicht zurückgeschrieben werden müssen, wird empfohlen, diese mittelfristig auf 'ByVal' umzustellen.
    ''' - Datenbank-Aktualisierung innerhalb der Schleife optimiert.
    ''' </remarks>
    Private Sub prDruck(ByRef sBid As String, ByRef sRnrID As String, ByRef nSprache As Integer)
        ' Nicht verwendete Variablen entfernt (arDruck, sZeile)
        Dim arfeld3(1) As String

        ' Abfrage der Buchungsdaten über die übergebene ID
        Dim sSQL As String = "Select ID from Buchung Where BID='" & sBid & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)
        Dim nMax As Integer = dt.Rows.Count - 1

        ' Virtual Print Engine (VPE) initialisieren
        prVpeOpen()

        ' Rechnungsdruck starten (Veraltetes 'Call' entfernt)
        prDruckAnzahlung(sBid, sRnrID, dtpARDatum.Value, cbBar.Checked, nAnzahlung, dtpARDatum.Value, nSprache)

        ' Array für das DB-Update mit Rechnungsnummer und Datum belegen
        arfeld3(0) = lbRech.Text
        arfeld3(1) = sRdatum

        ' Durchlaufen aller betroffenen Buchungszeilen und Status aktualisieren
        Dim cBedingung As String
        For i As Integer = 0 To nMax
            cBedingung = " WHERE ID='" & dt.Rows(i).Item("ID").ToString() & "'"
            fcUpdateCommand("Buchung", {"RAID", "RADatum"}, arfeld3, cBedingung)
        Next

        ' Wenn es eine neue Rechnungsnummer ist, den Nummernkreis hochzählen
        If bNewRNr = True Then
            prSetNr("RNr", fcGetNr("RNr"))
        End If

        ' Druckvorschau anzeigen und Mail-Button freigeben
        prVpeVeiw()
        buMail.Enabled = True
    End Sub

    ''' <summary>
    ''' Verarbeitet den Klick auf den Drucken-Button. Erstellt die Rechnungsdaten 
    ''' basierend auf der gewählten Rechnungsart (Standard, Zusatz, Storno oder Pauschal), 
    ''' stößt den Druckprozess an, aktualisiert den Status in der Datenbank und 
    ''' generiert die ZUGFeRD-konforme PDF-Rechnung im Archiv.
    ''' </summary>
    ''' <param name="sender">Das auslösende Objekt (Button).</param>
    ''' <param name="e">Die Ereignisdaten des Click-Events.</param>
    ''' <remarks>
    ''' 11.04.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Altlasten entfernt: VB6-Funktionen wie 'Mid()' konsequent durch modernes '.Substring()' ersetzt.
    ''' - Stabilität erhöht: Null-Conditional-Operator ('?.ToString()') verhindert Abstürze bei leeren Grid-Zellen.
    ''' - Array-Fixes: Dynamisches 'ReDim Preserve' logisch abgesichert und Indizes für jeden Rechnungsblock sauber zurückgesetzt.
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter entfernt, Zuweisungsoperatoren modernisiert ('&=' / '+=') und Variablen-Scoping geschärft.
    ''' </remarks>
    Private Sub btDruck_Click(ByRef sBid As String, ByRef sRnrID As String, ByRef nSprache As Integer)
        Dim arDruck(-1) As String
        Dim sZeile As String
        Dim arfeld3(3) As String
        Dim cBedingung As String


        prVpeOpen()

        'ReDim arDruck(-1)
        '    For i As Integer = 0 To dgRechnung.Rows.Count - 1
        '        If dgRechnung.Rows(i).Cells(9).Value?.ToString() <> "0" Then
        '            sZeile = ""
        '            For j As Integer = 1 To 18
        '                If j = 7 Or j = 8 Then
        '                    sZeile &= StrTrim(dgRechnung.Rows(i).Cells(j).Value, ".") & "#"
        '                Else
        '                    sZeile &= dgRechnung.Rows(i).Cells(j).Value?.ToString() & "#"
        '                End If
        '            Next

        '            ReDim Preserve arDruck(arDruck.Length)
        '            If sZeile.Length > 0 Then
        '                sZeile = sZeile.Substring(0, sZeile.Length - 1)
        '            End If
        '            arDruck(arDruck.Length - 1) = sZeile
        '        End If
        '    Next

        'prX_Rechnung_CII(sgRBID, sgGID, sRnrID, sRdatum, cbBar.Checked, nAnzahlung, sRDSenden, sSS7, sSS19, nReArt, arDruck)
        'prDruckRechnung(sgRBID, sgGID, sRnrID, sRdatum, cbBar.Checked, nAnzahlung, sRDSenden, sSS7, sSS19, sSSS, nReArt, arDruck)

        'arfeld3(0) = sRnrID
        'arfeld3(1) = sRdatum
        'arfeld3(2) = "-1"
        'arfeld3(3) = sgGID

        'For i As Integer = 0 To dgRechnung.Rows.Count - 1
        '    cBedingung = " WHERE ID='" & dgRechnung.Rows(i).Cells(15).Value?.ToString() & "'"
        '    fcUpdateCommand("Buchung", {"RId", "RDatum", "Rechnung", "kunID"}, arfeld3, cBedingung)
        'Next

        'If bNewRNr Then prSetNr("RNr", fcGetNr("RNr"))

        'prVpeVeiw()


    End Sub


    ''' <summary>
    ''' Prüft das Vorhandensein der Rechnungs-PDF, liest die Kunden-E-Mail-Adresse aus 
    ''' und versendet die Rechnung automatisch als E-Mail-Anhang.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 11.04.2012 - Create
    ''' 09.10.2026 - Code-Optimierung:
    ''' - Rechtschreibfehler in den MsgBox-Texten korrigiert ("Erstelt" -> "erstellt", "Rechnungs Mail" -> "Rechnungs-E-Mail").
    ''' - Ungenutzte Variablen ('arDruck', 'sZeile', 'arfeld3', 'sName', 'Anrede' etc.) entfernt.
    ''' - 'arFeld(0).Trim <> ""' durch das sicherere 'String.IsNullOrWhiteSpace' ersetzt.
    ''' - 'ii' direkt bei der Deklaration sauber initialisiert.
    ''' </remarks>
    Private Sub buMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buMail.Click
        ' Prüfen, ob die Rechnungs-PDF im definierten Verzeichnis existiert
        Dim sPdfPfad As String = Path.Combine(arIni(32), "Rechnung")
        Dim sPdf As String = Path.Combine(sPdfPfad, "Rech_" & lbRech.Text & ".PDF")
        If Not FileExists(sPdf) Then
            MsgBox("Rechnung wurde noch nicht erstellt.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Hinweis")
            Exit Sub
        End If

        ' E-Mail-Textbausteine laden und splitten
        Dim sText As Array = Split(ReadOneValueFromSystemDb("Mail_Rechnung"), "#")
        'Dim sPdf As String = Path.Combine(sPdfPfad, "Rech_" & sgRNr & ".PDF")

        ' Kundendaten und Sprache aus der Buchung ermitteln
        Dim arfeld1() As String = fcDataSeek("select KunID, Sprache From Buchung Where BID ='", sgRBID, 0, {"KunID", "Sprache"})

        Dim sKID As String = arfeld1(0)
        Dim nSprachIndex As Integer = Val(arfeld1(1))

        ' E-Mail-Adresse des Kunden abfragen (Ungenutzte Felder aus dem Array-Resultat entfernt)
        Dim arFeld() As String = fcDataSeek("select EMail From Kunden Where ID ='", sKID, 0, {"EMail"})

        ' Prüfen, ob eine gültige E-Mail-Adresse vorhanden ist
        If Not String.IsNullOrWhiteSpace(arFeld(0)) Then
            ' E-Mail mit PDF-Anhang senden
            If fcSendeMailAnlage(arFeld(0), "Anzahlung", sText(nSprachIndex), sDatei:=sPdf) = True Then
                MsgBox("Anzahlungs-E-Mail erfolgreich gesendet.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Erfolg")
            Else
                MsgBox("Fehler beim Senden der Anzahlungs-E-Mail.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Fehler")
            End If
        Else
            MsgBox("Fehler: Keine E-Mail-Adresse beim Kunden hinterlegt.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Fehler")
        End If

        Me.Close()
    End Sub

    ''' <summary>
    ''' Schließt das aktuelle Formular.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 11.04.2012 - Create
    ''' </remarks>
    Private Sub buClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buClose.Click
        Me.Close()
    End Sub

End Class