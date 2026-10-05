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

    ''' <summary>
    ''' Wird beim Laden des Hauptformulars (frmMain) ausgeführt.
    ''' Initialisiert das Masken-Layout, lädt die Zimmer- und Gastdaten zur aktuellen Buchung, 
    ''' generiert bei Bedarf eine neue Rechnungsnummer und aktualisiert die Rechnungspositionen.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 05.10.2026 - Code-Optimization:
    ''' - 'Try-Catch-Finally'-Struktur hinzugefügt, um unvollständiges Laden bei DB-Fehlern zu verhindern.
    ''' - Garantiertes Zurücksetzen des Cursors im 'Finally'-Block (wichtig bei vorzeitigem Abbruch).
    ''' - Veraltetes 'Call'-Schlüsselwort bei allen Prozeduraufrufen entfernt.
    ''' - Auskommentierte Code-Leichen ('gbZimmer', 'ssMain', 'tsMain', 'Call Main()') entfernt.
    ''' - String-Verkettung für die Rechnungsnummer modernisiert.
    ''' </remarks>
    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Warte-Cursor anzeigen, da Daten aus der DB geladen werden
        Me.Cursor = Cursors.WaitCursor

        Try
            ' 1. Visuelle Initialisierung
            Dim sColor As Color = Color.LightYellow
            Me.BackColor = sColor
            gbOption.BackColor = sColor
            gbGast.BackColor = sColor
            gbRechnung.BackColor = sColor

            ' Standardwerte und Element-Zustände setzen
            coSteuer.Text = "0"
            buMail.Enabled = False

            ' 2. Daten laden (Reihenfolge beibehalten)
            prSetTabelleRechnung(dtZim)
            prLoadZimmer(sgRBID, sgRNr)
            prLadeGastDaten(sgGID)

            ' Buchungs-ID im UI anzeigen
            lbBID.Text = sgRBID

            ' 3. Rechnungsnummer verarbeiten
            If String.IsNullOrWhiteSpace(sgRNr) Then
                sRNr = fcGetNr("RNr")
                ' Modernes String-Interpolation ($) statt Verkettung mit &
                tbRNr.Text = $"{Date.Today.Year}-{sRNr}"
            Else
                tbRNr.Text = sgRNr
            End If

            ' Rechnungsaufstellung aktualisieren
            prRefreshRechnungsPosition()

            ' Gast-ID für Kontierung zuweisen
            sGKNr = sgGID

        Catch ex As Exception
            ' Fehler reporten, damit die Anwendung bei DB-Fehlern nicht unkontrolliert abstürzt
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ' WICHTIG: Der Cursor MUSS im Finally zurückgesetzt werden, 
            ' damit der Nutzer bei einem Fehler nicht auf einem dauerhaften WaitCursor hängen bleibt.
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Initialisiert die Tabellenstruktur des Rechnungs-GridViews (dgRechnung).
    ''' Erstellt alle benötigten Spalten, definiert Spaltenbreiten, Ausrichtungen,
    ''' Farb- und Selektionsstile und unterbindet die automatische Spaltensortierung.
    ''' </summary>
    ''' <param name="dt">Die DataTable mit den Zimmer-Stammdaten (wird in dieser Prozedur aktuell nicht aktiv verwendet).</param>
    ''' <remarks>
    ''' 19.03.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Logikfehler behoben: Spaltensortierung wird nun wie im Kommentar gefordert via 'NotSortable' unterbunden.
    ''' - Redundante und unübersichtliche Spalten-Zuweisungen durch strukturierte Arrays und Schleifen stark verkürzt.
    ''' - Ungenutzte lokale Variable 'DGVCol' in die Schleife integriert.
    ''' - Performance beim Neuaufbau der Tabellenstruktur optimiert.
    ''' </remarks>
    Private Sub prSetTabelleRechnung(ByVal dt As DataTable)
        With dgRechnung
            .Columns.Clear()
            .ColumnHeadersHeight = 30
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .ShowCellToolTips = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing

            ' Farben der selektierten Zeile definieren
            .DefaultCellStyle.SelectionBackColor = cgColorRow
            .DefaultCellStyle.SelectionForeColor = Color.Black

            ' 1. Spalten hinzufügen (Name/Key und Header-Text)
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

            ' 2. Individuelle Breiten und Ausrichtungen für die ersten Spalten setzen
            Dim columnSettings As New Dictionary(Of Integer, (Width As Integer, Alignment As DataGridViewContentAlignment)) From {
                {0, (50, DataGridViewContentAlignment.MiddleCenter)},  ' POS
                {1, (80, DataGridViewContentAlignment.MiddleLeft)},    ' Zimmer
                {2, (50, DataGridViewContentAlignment.MiddleCenter)},  ' Menge
                {3, (150, DataGridViewContentAlignment.MiddleLeft)},   ' Text
                {4, (50, DataGridViewContentAlignment.MiddleRight)},   ' Betrag
                {5, (50, DataGridViewContentAlignment.MiddleRight)},   ' ST7
                {6, (50, DataGridViewContentAlignment.MiddleRight)},   ' ST19
                {7, (50, DataGridViewContentAlignment.MiddleRight)},   ' STS
                {8, (80, DataGridViewContentAlignment.MiddleRight)},   ' Netto
                {9, (80, DataGridViewContentAlignment.MiddleRight)}    ' Brutto
            }

            For Each setting In columnSettings
                .Columns(setting.Key).Width = setting.Value.Width
                .Columns(setting.Key).DefaultCellStyle.Alignment = setting.Value.Alignment
            Next

            ' 3. Alle restlichen Spalten ab Index 10 erhalten eine Standardbreite von 50 und Rechtsbündigkeit
            Dim nB As Integer = 50
            For i As Integer = 10 To .Columns.Count - 1
                .Columns(i).Width = nB
                .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Next

            ' 4. Eigenschaften für alle Spalten anwenden (Sortierung ausschalten)
            For Each col As DataGridViewColumn In .Columns
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            .AutoResizeRows()
        End With
    End Sub

    ''' <summary>
    ''' Lädt die Gast- bzw. Kundendaten anhand der übergebenen ID aus der Datenbank 
    ''' und stellt diese in den entsprechenden Oberflächenelementen (Labels) dar.
    ''' </summary>
    ''' <param name="sID">Die eindeutige Kunden-ID (ID).</param>
    ''' <remarks>
    ''' 04.02.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Sicherheitsprüfung für 'dt IsNot Nothing' hinzugefügt, um NullReferenceExceptions bei Verbindungsabbrüchen zu verhindern.
    ''' - String-Konvertierung von '.ToString()' auf die robustere Variante 'Convert.ToString()' umgestellt, um DBNull-Fehler abzufangen.
    ''' - XML-Dokumentation für Parameter vervollständigt.
    ''' </remarks>
    Private Sub prLadeGastDaten(ByVal sID As String)
        Dim sSQL As String = "SELECT Anrede, Name1, Name2, Vorname, Strasse, PLZ, Ort, Land FROM Kunden WHERE ID='" & sID & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' Sicherheitsprüfung: Hat die Datenbank überhaupt eine gültige Tabelle zurückgegeben und enthält diese Zeilen?
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            With dt.Rows(0)
                ' Convert.ToString() fängt im Gegensatz zu .ToString() ein potenzielles DBNull sicher ab,
                ' ohne dass die Anwendung abstürzt, und gibt stattdessen einen leeren String ("") zurück.
                lbAnrede.Text = Convert.ToString(.Item("Anrede"))
                lbName1.Text = Convert.ToString(.Item("Name1"))
                lbName2.Text = Convert.ToString(.Item("Name2"))
                lbVorname.Text = Convert.ToString(.Item("Vorname"))
                lbStrasse.Text = Convert.ToString(.Item("Strasse"))
                lbPLZ.Text = Convert.ToString(.Item("PLZ"))
                lbOrt.Text = Convert.ToString(.Item("Ort"))
                lbLand.Text = Convert.ToString(.Item("Land"))
            End With
        End If
    End Sub

    ''' <summary>
    ''' Lädt alle gebuchten Zimmer zu einer bestimmten Buchung und Rechnungsnummer aus der Datenbank.
    ''' Initialisiert Rechnungsdaten (Datum, Zahlungsart), befüllt die Zimmerliste (chliZimmer) und hakt die aktiven Zimmer an.
    ''' </summary>
    ''' <param name="sBid">Die eindeutige Buchungs-ID (BID).</param>
    ''' <param name="sRNrID">Die bestehende Rechnungsnummer (RID).</param>
    ''' <remarks>
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Kritischen Absturz-Bug behoben: 'Rows.Count'-Prüfung an den Anfang verschoben, um Index-Fehler bei leeren Abfragen zu verhindern.
    ''' - 'Nothing'- und 'DBNull'-Prüfungen für 'dt' und Datenbankfelder hinzugefügt.
    ''' - Redundanten/doppelten Aufruf von 'prRefreshRechnungsPosition' entfernt (wird bereits im Load-Event direkt danach gefeuert).
    ''' - String-Verarbeitung modernisiert (String-Interpolation statt '&' Verknüpfung).
    ''' - Schleifenvariablen lokal typisiert und ungenutztes 'nMax' entfernt.
    ''' </remarks>
    Private Sub prLoadZimmer(ByVal sBid As String, ByVal sRNrID As String)
        Dim sSQL As String = "SELECT RDatum, RDSenden, Pausch, ZimID, ID FROM Buchung WHERE BID='" & sBid & "' AND RID='" & sRNrID & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' CRITICAL BUGFIX: Sofort abbrechen, wenn keine Daten gefunden wurden (verhindert Absturz bei dt.Rows(0))
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

        Dim firstRow As DataRow = dt.Rows(0)

        ' 1. Stammdaten der Rechnung auslesen
        sRdatum = Convert.ToString(firstRow("RDatum")).Trim()
        sRDSenden = Convert.ToString(firstRow("RDSenden")).Trim()
        sPausch = Convert.ToString(firstRow("Pausch")).Trim()

        ' Pauschalierungsstatus prüfen
        If sPausch = "1" Then
            rbPausch.Checked = True
            nReArt = 4
        End If

        ' Zahlungsart (Bar) vorbelegen, falls Sendedatum leer ist
        cbBar.Checked = String.IsNullOrWhiteSpace(sRDSenden)

        ' Rechnungsdatum validieren und dem DateTimePicker zuweisen
        If String.IsNullOrWhiteSpace(sRdatum) Then
            sRdatum = fcUmDatum(Date.Today)
        End If
        dtpRDatum.Value = fcUmDatum(sRdatum)

        ' Rechnungsnummer setzen oder neu generieren
        tbRNr.Text = sRNrID.Trim()
        If String.IsNullOrWhiteSpace(tbRNr.Text) Then
            bNewRNr = True
            sRNr = fcGetNr("RNr")
            tbRNr.Text = $"{Date.Today.Year}-{sRNr}"
        End If

        ' 2. Zimmer-Array initialisieren (arZ)
        arZ(0, 0) = "99"
        arZ(0, 1) = "Weiteres Zimmer"

        ' Gefundene Zimmer in das Array übertragen
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim row As DataRow = dt.Rows(i)
            arZ(i + 1, 0) = Convert.ToString(row("ZimID"))
            arZ(i + 1, 2) = Convert.ToString(row("ID"))
            arZ(i + 1, 1) = " "
        Next

        ' 3. CheckedListBox (chliZimmer) befüllen
        chliZimmer.BeginUpdate()
        Try
            chliZimmer.Items.Clear() ' Vorherige Einträge löschen, um Dopplungen zu vermeiden
            Dim n As Integer = 0

            ' Array auswerten (Begrenzt auf max. 20 Einträge laut Ihrer Struktur)
            For i As Integer = 1 To 20
                If arZ(i, 0) IsNot Nothing Then
                    arZ(i, 1) = fcGetObjektZimmerName(dtZim, arZ(i, 0)).Trim()

                    If Not String.IsNullOrWhiteSpace(arZ(i, 1)) Then
                        ' Eintrag hinzufügen (Format: Zimmername [ID])
                        chliZimmer.Items.Add($"{arZ(i, 1)}[{arZ(i, 2)}]")
                        chliZimmer.SetItemChecked(n, True)
                        n += 1
                    End If
                End If
            Next
        Finally
            chliZimmer.EndUpdate()
        End Try

        ' HINWEIS: Der Aufruf von prRefreshRechnungsPosition() wurde hier entfernt, 
        ' da er im 'frmMain_Load' unmittelbar nach dieser Methode ohnehin aufgerufen wird.
    End Sub

    ''' <summary>
    ''' Löst die Aktualisierung der Rechnungspositionen aus, sobald der Benutzer die Maustaste 
    ''' über der Zimmer-Auswahlliste (chliZimmer) loslässt.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (chliZimmer).</param>
    ''' <param name="e">Die Ereignisdaten mit den Mauskoordinaten.</param>
    ''' <remarks>
    ''' 27.03.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort entfernt.
    ''' - XML-Dokumentation für Parameter vervollständigt.
    ''' </remarks>
    Private Sub chliZimmer_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles chliZimmer.MouseUp
        prRefreshRechnungsPosition()
    End Sub

    ''' <summary>
    ''' Setzt die bestehenden Rechnungsdaten und Summen zurück und lädt für jedes in der Liste 
    ''' aktivierte Zimmer die aktuellen Rechnungspositionen und Steuersätze neu.
    ''' </summary>
    ''' <remarks>
    ''' 27.03.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort bei 'prAddRechnungsPosition' und 'prCalculateSumme' entfernt.
    ''' - UI-Aktualisierung der ComboBox (coZimmer) und des Grids beschleunigt (BeginUpdate/EndUpdate), um Flackern zu minimieren.
    ''' - Zuweisung des Initialwerts für 'lbAnzahlung' kaufmännisch sauber formatiert.
    ''' </remarks>
    Private Sub prRefreshRechnungsPosition()
        ' Vorherige Zeilen und Steuerelemente leeren
        dgRechnung.Rows.Clear()

        ' UI-Zeichnen für die ComboBox temporär einfrieren (Performance-Schutz)
        coZimmer.BeginUpdate()
        Try
            coZimmer.Items.Clear()

            ' Globale Summen- und Anzahlungsspeicher zurücksetzen
            nGNetto = 0
            nGBrutto = 0
            nGSt7 = 0
            nGSt19 = 0
            nAnzahlung = 0

            ' Anzahlungs-Label zurücksetzen (Formatiert als Standard-Dezimalzahl)
            lbAnzahlung.Text = nAnzahlung.ToString("F2")

            ' Wenn kein Zimmer angehakt ist, brechen wir nach dem Leeren ab
            If chliZimmer.CheckedItems.Count = 0 Then Exit Sub

            ' Alle ausgewählten Zimmer durchlaufen und deren Positionen laden
            For Each item As Object In chliZimmer.CheckedItems
                If item IsNot Nothing Then
                    Dim sItemText As String = item.ToString()
                    prAddRechnungsPosition(sItemText)
                    coZimmer.Items.Add(sItemText)
                End If
            Next
        Finally
            ' UI-Zeichnen wieder freigeben
            coZimmer.EndUpdate()
        End Try

        ' Gesamtsumme der aktiven Positionen neu kalkulieren
        prCalculateSumme()
    End Sub

    ''' <summary>
    ''' Bereitet eine Buchung sowie deren Zimmer- und Frühstücksinformationen auf und fügt diese als neue Position dem Rechnungs-GridView (dgRechnung) hinzu.
    ''' </summary>
    ''' <param name="sZim">Der rohe Zimmer-String, der die Buchungs-ID in eckigen Klammern enthält (z. B. "Zimmer 101 [1234]").</param>
    ''' <remarks>
    ''' 27.03.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Doppelten Datenbankaufruf (fcReadDataTable + fcGetData) entfernt; Daten werden direkt aus der ersten Abfrage verwendet.
    ''' - Veraltete 'Val()'-Funktion durch typsicheres 'Decimal.TryParse' bzw. 'Integer.TryParse' ersetzt.
    ''' - Typkonvertierungen für Währungs- und Steuerberechnungen mathematisch präzisiert.
    ''' - Unnötiges 'Call'-Schlüsselwort bei 'prGetExtrasZimmer' entfernt.
    ''' - String-Bereinigung und Fehlerabsicherung bei potenziellen Leerwerten optimiert.
    ''' </remarks>
    Private Sub prAddRechnungsPosition(ByVal sZim As String)
        Dim arT() As String
        Dim arL() As String
        Dim arF() As String
        Dim nTage As Integer = 0
        Dim nRB As Decimal = 0
        Dim nRBF As Decimal = 0
        Dim nRBG As Decimal = 0
        Dim nFr As Integer = 0

        ' IDs aus dem übergebenen Zimmer-String extrahieren
        Dim sID As String = Extract(sZim, "[", "]", 1)
        sZim = AtLeft(sZim, "[", 1)
        sZID = fcGetObjektZimmerID(dtZim, sZim, "ID")

        Dim sSQL As String = "SELECT * FROM Buchung WHERE BID='" & sgRBID & "' AND ID='" & sID & "'"

        ' OPTIMIERUNG: Wir holen die Felddaten direkt. Wenn das Array leer ist, gab es keine Zeilen.
        arT = fcGetData(sSQL, 0, {"Von", "Bis", "ZimID", "Personen", "Art", "Frueh", "Preis", "Anzahlung", "FPreis", "ID", "Storno", "Summe", "MwstU", "MwstS", "MwstG", "Gpreis", "GKU", "GKS", "GKG"})
        If arT Is Nothing OrElse arT.Length = 0 Then Exit Sub

        ' Zimmer-Stammdaten abrufen
        arF = fcGetData("SELECT * FROM Zimmer WHERE ID ='" & sZID & "'", 0, {"Name", "Art", "Ausstattung"})

        ' Zimmerart/Kategorie in das Buchungs-Array übertragen
        arT(2) = arF(1)

        ' Absicherung für den Getränkepreis (Gpreis), falls dieser leer ist
        If String.IsNullOrWhiteSpace(arT(15)) Then arT(15) = "0"

        ' Anzahl der Aufenthaltstage ermitteln
        nTage = fcGetAnzahlTage(arT(0), arT(1))

        ' Sicheres Parsen der numerischen Werte aus den DB-Strings (Ersetzt das unpräzise Val())
        Integer.TryParse(arT(5), nFr)
        Decimal.TryParse(arT(6), nRB)

        ' Frühstücks- und Getränkeberechnung bei Ü/F
        If arT(4) = "Ü/F" Then
            Dim nFPreis As Decimal = 0
            Dim nGPreis As Decimal = 0

            Decimal.TryParse(arT(8), nFPreis)
            Decimal.TryParse(arT(15), nGPreis)

            nRBF = nFPreis * nFr ' Speisenanteil Frühstück
            nRBG = nGPreis * nFr ' Getränkeanteil Frühstück

            ' Reiner Zimmerpreis abzüglich der darin enthaltenen Frühstücksanteile
            nRB = nRB - nRBF - nRBG
        End If

        ' Globale Variablen für Steuersätze und Konten befüllen
        sSS7 = arT(12)
        sSS19 = arT(13)
        sSSS = arT(14)
        sGKU = arT(16)
        sGKS = arT(17)
        sGKG = arT(18)



        ' VORBEREITUNG FÜR DEN FUNKTIONSAUFRUF:
        ' Sicheres Parsen der Steuersätze, um leere Strings/Leerzeichen abzufangen
        Dim nSteuer7Wert As Integer = 0
        Dim nSteuer19Wert As Integer = 0
        Dim nSteuerSWert As Integer = 0

        ' Zuweisung & Parsen für MwstU (Steuer 7%)
        sSS7 = arT(12)
        Integer.TryParse(sSS7, nSteuer7Wert)

        ' Zuweisung & Parsen für MwstS (Steuer 19%)
        sSS19 = arT(13)
        Integer.TryParse(sSS19, nSteuer19Wert)

        ' Zuweisung & Parsen für MwstG (Sonstige Steuer)
        sSSS = arT(14)
        Integer.TryParse(sSSS, nSteuerSWert)

        ' Konten-Strings bleiben unberührt
        sGKU = arT(16)
        sGKS = arT(17)
        sGKG = arT(18)

        ' AUFRUF DER FUNKTION MIT DEN REINEN ZAHLEN-VARIABLEN:
        arL = fcGetDataForRechnung(dgRechnung.Rows.Count, nTage, sZim, arT(2), nRB, nRBF,
                                   nSteuer7Wert, nSteuer19Wert, "0", arT(0), arT(1),
                                   arT(3), arT(4), arF(2), arT(8), arT(9), arT(10),
                                   arT(11), nSteuerSWert, arT(15))
        dgRechnung.Rows.Add(arL)

        ' Anzahlung kaufmännisch korrekt aufsummieren
        Dim nAktuelleAnzahlung As Decimal = 0
        Decimal.TryParse(arT(7), nAktuelleAnzahlung)
        nAnzahlung += (nAktuelleAnzahlung / 100D)

        ' Zusatzleistungen/Extras des Zimmers einlesen
        prGetExtrasZimmer(sgRBID, sZID)
    End Sub

    ''' <summary>
    ''' Berechnet die Anzahl der Aufenthaltstage zwischen einem Anreise- und Abreisedatum.
    ''' Erhöht das Abreisedatum intern um einen Tag und ermittelt die mathematische Differenz.
    ''' </summary>
    ''' <param name="sVon">Das Anreisedatum als Zeichenfolge (String).</param>
    ''' <param name="sBis">Das Abreisedatum als Zeichenfolge (String).</param>
    ''' <returns>Die Anzahl der Tage als formatierte Zeichenfolge (String). Bei Fehlern wird "0" zurückgegeben.</returns>
    ''' <remarks>
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Veraltete VB6-Befehle 'DateAdd' und 'DateDiff' durch native .NET-Arithmetik (.AddDays und .Subtract) ersetzt.
    ''' - Rückgabe über den Funktionsnamen ('fcGetAnzahlTage =') durch das moderne 'Return'-Schlüsselwort abgelöst.
    ''' - 'Date.TryParse' integriert, um Abstürze bei fehlerhaften Datumsformaten im System abzufangen.
    ''' </remarks>
    Public Function fcGetAnzahlTage(ByVal sVon As String, ByVal sBis As String) As String
        Dim dVon As Date
        Dim dBis As Date

        ' Sicheres Parsen der Datums-Strings unter Einbeziehung Ihrer Konvertierungsfunktion fcUmDatum.
        ' Falls das Format ungültig ist, wird sicher "0" zurückgegeben.
        If Not Date.TryParse(fcUmDatum(sVon).ToString(), dVon) OrElse
           Not Date.TryParse(fcUmDatum(sBis).ToString(), dBis) Then
            Return "0"
        End If

        ' Abreisedatum kaufmännisch um einen Tag erhöhen
        dBis = dBis.AddDays(1)

        ' Differenz in Tagen über die TimeSpan berechnen (.Days gibt die Ganzzahl zurück)
        Dim nTage As Integer = dBis.Subtract(dVon).Days

        ' Rückgabe als String
        Return nTage.ToString()
    End Function

    ''' <summary>
    ''' Berechnet die Netto-, Steuer- und Bruttobeträge für die verschiedenen Steuersätze einer Rechnungsposition 
    ''' und stellt diese zusammen mit den Stammdaten als String-Array für das GridView bereit.
    ''' </summary>
    ''' <param name="nPos">Die fortlaufende Positionsnummer der Zeile.</param>
    ''' <param name="nTage">Anzahl der Aufenthaltstage.</param>
    ''' <param name="sZim">Bezeichnung des Zimmers.</param>
    ''' <param name="sText">Kategorie-/Zimmertext.</param>
    ''' <param name="nBrutto7">Rohbetrag für den ermäßigten Steuersatz (z. B. Übernachtung).</param>
    ''' <param name="nBrutto19">Rohbetrag für den vollen Steuersatz (z. B. Speisen/Frühstück).</param>
    ''' <param name="nSteuer7">Der Prozentsatz der ermäßigten Steuer (z. B. 7).</param>
    ''' <param name="nSteuer19">Der Prozentsatz der vollen Steuer (z. B. 19).</param>
    ''' <param name="sZusatz">Kennzeichen für Zusatzleistungen ("0" für Standardübernachtung).</param>
    ''' <param name="sVon">Anreisedatum.</param>
    ''' <param name="sBis">Abreisedatum.</param>
    ''' <param name="sPersonen">Anzahl der Personen als Zeichenfolge.</param>
    ''' <param name="sArt">Verpflegungsart (z. B. Ü/F).</param>
    ''' <param name="sAusstattung">Ausstattungsmerkmale des Zimmers.</param>
    ''' <param name="fPreis">Frühstücks-Basispreis.</param>
    ''' <param name="sID">Eindeutige Buchungszeilen-ID.</param>
    ''' <param name="sStorno">Stornierungsstatus oder Kennzeichen.</param>
    ''' <param name="sSumme">Vorgegebene Pauschalsumme bei Pauschalabrechnung.</param>
    ''' <param name="nSteuerS">Sondersteuersatz (z. B. für Getränkeanteile).</param>
    ''' <param name="nBruttoS">Rohbetrag des Sondersteuersatzes.</param>
    ''' <returns>Ein stark typisiertes String-Array mit allen berechneten und formatierten Werten für die Tabellenzeile.</returns>
    ''' <remarks>
    ''' 02.04.2012 - Add Zusatz und Datum (von-bis)
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Rückgabetyp von 'Array' auf das typsichere 'String()' umgestellt.
    ''' - Veraltete Zuweisung über Funktionsnamen durch 'Return' ersetzt.
    ''' - Auskommentierte Berechnungsaltlasten im Rechenkern entfernt.
    ''' - Fehleranfälliges 'Val()' durch 'Integer.TryParse' und 'Decimal.TryParse' ersetzt.
    ''' - Datums-Arithmetik für das Abrechnungsdatum durch 'Date.TryParse' abgesichert.
    ''' </remarks>
    Private Function fcGetDataForRechnung(ByVal nPos As Integer, ByVal nTage As Integer,
                                          ByVal sZim As String, ByVal sText As String,
                                          ByVal nBrutto7 As Decimal, ByVal nBrutto19 As Decimal,
                                          ByVal nSteuer7 As Integer, ByVal nSteuer19 As Integer,
                                          ByVal sZusatz As String, ByVal sVon As String,
                                          ByVal sBis As String, ByVal sPersonen As String,
                                          ByVal sArt As String, ByVal sAusstattung As String,
                                          ByVal fPreis As String, ByVal sID As String,
                                          ByVal sStorno As String, ByVal sSumme As String,
                                          ByVal nSteuerS As Integer, ByVal nBruttoS As Decimal) As String()

        Dim arL(18) As String
        Dim nRB As Decimal = 0       ' Brutto ermäßigt (7%)
        Dim nRBF As Decimal = 0      ' Brutto voll (19%)
        Dim nRBS As Decimal = 0      ' Brutto Sonder
        Dim nRN As Decimal = 0       ' Netto ermäßigt
        Dim nRNF As Decimal = 0      ' Netto voll
        Dim nRNS As Decimal = 0      ' Netto Sonder
        Dim nST As Decimal = 0       ' Steuer ermäßigt
        Dim nSTF As Decimal = 0      ' Steuer voll
        Dim nSTS As Decimal = 0      ' Steuer Sonder

        ' Personenanzahl sicher ermitteln
        Dim nPersonenCount As Integer = 0
        Integer.TryParse(sPersonen, nPersonenCount)

        Dim nBruttoS1 As Decimal = nBruttoS * nPersonenCount

        ' 1. Standard-Berechnungen nach Steuersätzen getrennt
        If nBrutto7 > 0 Then
            nRB = (nBrutto7 * nTage) / 100D
            nRN = Math.Round(nRB / (1D + (nSteuer7 / 100D)), 2, MidpointRounding.AwayFromZero)
            nST = nRB - nRN
        End If

        If nBrutto19 > 0 Then
            nRBF = (nBrutto19 * nTage) / 100D
            nRNF = Math.Round(nRBF / (1D + (nSteuer19 / 100D)), 2, MidpointRounding.AwayFromZero)
            nSTF = nRBF - nRNF
        End If

        If nBruttoS > 0 Then
            nRBS = (nBruttoS1 * nTage) / 100D
            nRNS = Math.Round(nRBS / (1D + (nSteuerS / 100D)), 2, MidpointRounding.AwayFromZero)
            nSTS = nRBS - nRNS
        End If

        ' 2. Sonderlogik für Pauschalabrechnungen (nReArt = 4) überschreibt obige Werte kaskadierend
        If nReArt = 4 Then
            If nBruttoS > 0 Then
                nRBS = (nBruttoS1 * nTage) / 100D
                nRNS = Math.Round(nRBS / (1D + (nSteuerS / 100D)), 2, MidpointRounding.AwayFromZero)
                nSTS = nRBS - nRNS
            End If

            If nBrutto19 > 0 Then
                nRBF = (nBrutto19 * nTage) / 100D
                nRNF = Math.Round(nRBF / (1D + (nSteuer19 / 100D)), 2, MidpointRounding.AwayFromZero)
                nSTF = nRBF - nRNF
            End If

            Dim nÜbergabeSumme As Decimal = 0
            Decimal.TryParse(sSumme, nÜbergabeSumme)
            If nÜbergabeSumme > 0 Then
                ' Der Übernachtungsanteil ergibt sich aus der Gesamtsumme abzüglich Verpflegung und Getränke
                nRB = (nÜbergabeSumme / 100D) - nRBS - nRBF
                nRN = Math.Round(nRB / (1D + (nSteuer7 / 100D)), 2, MidpointRounding.AwayFromZero)
                nST = nRB - nRN
            End If
        End If

        ' 3. Array-Befüllung für die Tabellenzeile
        arL(0) = nPos.ToString()
        arL(1) = sZim
        arL(2) = nTage.ToString()
        arL(3) = sText
        arL(4) = fcFormatDecimal(((nBrutto7 + nBrutto19 + nBruttoS1) / 100D).ToString(), 2)
        arL(5) = fcFormatDecimal(nST.ToString(), 2)  ' lbSteuer     ermäßigt
        arL(6) = fcFormatDecimal(nSTF.ToString(), 2) ' lbSteuer     voll
        arL(7) = fcFormatDecimal(nSTS.ToString(), 2) ' lbSteuer     Sonder
        arL(8) = fcFormatDecimal((nRN + nRNF + nRNS).ToString(), 2) 'lbNetto
        arL(9) = fcFormatDecimal((nRB + nRBF + nRBS).ToString(), 2) 'lbBrutto
        arL(10) = sZusatz
        arL(12) = sArt
        arL(13) = sAusstattung
        arL(14) = fPreis
        arL(15) = sID
        arL(16) = sStorno
        arL(17) = sSumme
        arL(18) = nBruttoS.ToString()

        ' Datumsbereich für den Ausdruck aufbereiten
        If sZusatz = "0" Then
            Dim dBisDatum As Date
            If Date.TryParse(fcUmDatum(sBis).ToString(), dBisDatum) Then
                ' Native Addition des Abreisetzubaus statt CDate-Cast
                arL(11) = $"{fcUmDatum(sVon)} - {dBisDatum.AddDays(1).ToShortDateString()}#{sPersonen}"
            Else
                arL(11) = $"{fcUmDatum(sVon)} - {fcUmDatum(sBis)}#{sPersonen}"
            End If
        Else
            arL(11) = "-#-"
        End If

        Return arL
    End Function


    ''' <summary>
    ''' Lädt alle gebuchten Zusatzleistungen (Extras) zu einem bestimmten Zimmer aus der Datenbank,
    ''' schlüsselt sie nach den hinterlegten Steuersätzen auf und fügt sie dem Rechnungs-GridView hinzu.
    ''' </summary>
    ''' <param name="sBID">Die eindeutige Buchungs-ID (BuchID).</param>
    ''' <param name="sZID">Die eindeutige Zimmer-ID (ZimID).</param>
    ''' <remarks>
    ''' 20.03.2012 - Create
    ''' 02.04.2012 - Add Zusatz und Datum
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Datentypen für Geldbeträge konsistent auf 'Decimal' umgestellt (Verhindert Rundungsfehler).
    ''' - 'For Each'-Schleife implementiert und auskommentierte Code-Leichen vollständig entfernt.
    ''' - 'DBNull'-Prüfungen für alle Datenbankfelder ergänzt, um Laufzeitabstürze zu vermeiden.
    ''' - Steuersatz-Übergabe an 'fcGetDataForRechnung' an die erwarteten Integer-Typen angepasst.
    ''' </remarks>
    Private Sub prGetExtrasZimmer(ByVal sBID As String, ByVal sZID As String)
        Dim arL() As String
        Dim dt As DataTable = fcReadDataTable("SELECT Betrag, Menge, Steuer, Bezeichnung, ZimNr, ID FROM Zusaetze WHERE BuchID='" & sBID & "' AND ZimID ='" & sZID & "'")

        ' Sicherheitsprüfung, ob Zusatzleistungen vorhanden sind
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

        Try
            For Each row As DataRow In dt.Rows
                ' Gelöschte Datenzeilen im Speicher überspringen
                If row.RowState <> DataRowState.Deleted Then

                    ' Werte absturzsicher aus der DataRow auslesen
                    Dim nSumme As Decimal = If(IsDBNull(row("Betrag")), 0D, Convert.ToDecimal(row("Betrag")))
                    Dim nMenge As Integer = If(IsDBNull(row("Menge")), 0, Convert.ToInt32(row("Menge")))
                    Dim sText As String = If(IsDBNull(row("Bezeichnung")), "", row("Bezeichnung").ToString())
                    Dim sZim As String = If(IsDBNull(row("ZimNr")), "", row("ZimNr").ToString())
                    Dim sZuID As String = If(IsDBNull(row("ID")), "", row("ID").ToString())

                    ' Roh-Steuersatz aus der DB lesen (wird als numerischer Wert oder String erwartet)
                    Dim nSteuerSatzRaw As Integer = 0
                    If Not IsDBNull(row("Steuer")) Then Integer.TryParse(row("Steuer").ToString(), nSteuerSatzRaw)

                    ' Variablen für die steuerliche Aufteilung initialisieren
                    Dim nSteuer7 As Integer = 0
                    Dim nSteuer19 As Integer = 0
                    Dim nSteuerS As Integer = 0

                    Dim nRB As Decimal = 0   ' Netto/Brutto-Anteil 7%
                    Dim nRBF As Decimal = 0  ' Netto/Brutto-Anteil 19%
                    Dim nRBS As Decimal = 0  ' Netto/Brutto-Anteil Sonder

                    ' Hilfsvariablen für den INI-Vergleich parsen
                    Dim ini11 As Integer = 0
                    Dim ini10 As Integer = 0
                    Dim ini23 As Integer = 0

                    If arIni(11) IsNot Nothing Then Integer.TryParse(arIni(11).ToString(), ini11)
                    If arIni(10) IsNot Nothing Then Integer.TryParse(arIni(10).ToString(), ini10)
                    If arIni(23) IsNot Threading.Thread.CurrentThread.CurrentCulture.NumberFormat Then Integer.TryParse(arIni(23).ToString(), ini23)

                    ' Aufteilung anhand des Steuersatzes vornehmen
                    Select Case nSteuerSatzRaw
                        Case ini11
                            nSteuer7 = ini11
                            nRB = nSumme
                        Case ini10
                            nSteuer19 = ini10
                            nRBF = nSumme
                        Case ini23
                            nSteuerS = ini23
                            nRBS = nSumme
                        Case Else
                            ' Fallback, falls der Steuersatz nicht in der INI definiert ist
                            nSteuer7 = nSteuerSatzRaw
                            nRB = nSumme
                    End Select

                    ' Datensatz über die kaufmännische Funktion aufbereiten
                    ' sZusatz wird als "1" übergeben, Datumsfelder bleiben für Extras leer ("")
                    arL = fcGetDataForRechnung(dgRechnung.Rows.Count, nMenge, sZim, sText, nRB, nRBF,
                                               nSteuer7, nSteuer19, "1", "", "", "", "", "",
                                               nSteuerSatzRaw.ToString(), sZuID, "", "", nSteuerS, nRBS)

                    ' Zeile dem Grid hinzufügen
                    dgRechnung.Rows.Add(arL)
                End If
            Next
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Zentraler Eventhandler für alle Rechnungsart-Optionen. 
    ''' Ermittelt die gewählte Rechnungsart dynamisch aus der Tag-Eigenschaft des RadioButtons.
    ''' </summary>
    Private Sub RechnungsArt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles rbExtraORe.CheckedChanged, rbExtraMRe.CheckedChanged, rbExtraGRe.CheckedChanged, rbStorno.CheckedChanged, rbPausch.CheckedChanged

        Dim rb As RadioButton = DirectCast(sender, RadioButton)

        ' Nur reagieren, wenn der RadioButton aktiv gesetzt wurde und ein gültiger Tag vorhanden ist
        If rb.Checked AndAlso rb.Tag IsNot Nothing Then
            Dim nGewaehlteArt As Integer = 0
            If Integer.TryParse(rb.Tag.ToString(), nGewaehlteArt) Then
                nReArt = nGewaehlteArt
                prRefreshRechnungsPosition()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Schließt das aktuelle Formular.
    ''' </summary>
    Private Sub btClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btClose.Click
        Me.Close()
    End Sub

#End Region

#Region "Einzelne Rechnungsposition bearbeiten oder hinzufügen....................................."

    ''' <summary>
    ''' Ermöglicht das Auswählen einer Rechnungsposition per Doppelklick, um deren Daten 
    ''' in die Bearbeitungsmaske (Eingabefelder und Steuerlabels) zu übernehmen.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (dgRechnung).</param>
    ''' <param name="e">Die Ereignisdaten mit den Zeilen- und Spaltenindizes.</param>
    ''' <remarks>
    ''' 27.03.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Kritischen Index-Fehler bei Klick auf Spaltenköpfe (e.RowIndex = -1) abgefangen.
    ''' - Absturzsichere String-Wandlung via 'Convert.ToString()' implementiert (fängt DBNull/Nothing ab).
    ''' - Logik zur Steuersatz-Ermittlung (Spalte 5 vs 6) strukturiert und lesbarer gestaltet.
    ''' - Fehlerhafter leerer Catch-Block entfernt bzw. für die Fehleranalyse vorbereitet.
    ''' </remarks>
    Private Sub dgRechnung_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgRechnung.CellDoubleClick
        ' CRITICAL BUGFIX: Klicks auf den Spaltenkopf (-1) oder außerhalb abfangen
        If e.RowIndex < 0 Then Exit Sub

        Try
            Dim row As DataGridViewRow = dgRechnung.Rows(e.RowIndex)

            ' Zellwerte sicher in Strings konvertieren, um Fehler durch DBNull zu vermeiden
            Dim sPos As String = Convert.ToString(row.Cells(0).Value).Trim()
            Dim sZusatz As String = Convert.ToString(row.Cells(10).Value).Trim() ' ACHTUNG: Index 9 im alten Code entsprach der 10. Spalte (.Cells(10)) laut prSetTabelleRechnung

            ' Prüfung: Position darf nicht leer sein und es muss sich um eine Zusatzleistung handeln
            If Not String.IsNullOrWhiteSpace(sPos) AndAlso sZusatz = "1" Then

                ' 1. Eingabefelder sperren und Text zuweisen
                tbPos.Text = sPos
                tbPos.Enabled = False
                tbText.Enabled = False
                coZimmer.Enabled = False

                coZimmer.Text = Convert.ToString(row.Cells(1).Value)
                tbMenge.Text = Convert.ToString(row.Cells(2).Value)
                tbText.Text = Convert.ToString(row.Cells(3).Value)
                tbBetrag.Text = Convert.ToString(row.Cells(4).Value)

                ' 2. Steuersatz ermitteln (Auswertung der berechneten Steuerfelder)
                Dim nSteuer7 As Double = 0
                Dim nSteuer19 As Double = 0

                ' Werte parsen, um mathematisch sauber auf "> 0" prüfen zu können
                Double.TryParse(Convert.ToString(row.Cells(5).Value), nSteuer7)
                Double.TryParse(Convert.ToString(row.Cells(6).Value), nSteuer19)

                If nSteuer7 > 0 Then
                    lbSteuer.Text = nSteuer7.ToString("F2")
                    coSteuer.Text = Convert.ToString(arIni(11))
                ElseIf nSteuer19 > 0 Then
                    lbSteuer.Text = nSteuer19.ToString("F2")
                    coSteuer.Text = Convert.ToString(arIni(10))
                Else
                    lbSteuer.Text = "0,00"
                    coSteuer.Text = "0"
                End If

                ' 3. Restliche kaufmännische Werte und Datumsbereiche zuweisen
                lbNetto.Text = Convert.ToString(row.Cells(7).Value)
                lbBrutto.Text = Convert.ToString(row.Cells(8).Value)
                lbZusatz.Text = sZusatz

                ' Datumsangaben extrahieren (Spalte 11 laut prSetTabelleRechnung, Index 11)
                Dim sZeitraum As String = Convert.ToString(row.Cells(11).Value)
                lbVon.Text = AtLeft(sZeitraum, "-", 1).Trim()
                lbBis.Text = AtRight(sZeitraum, "-", 1).Trim()

            End If

        Catch ex As Exception
            ' Fehler im Logbuch vermerken, statt ihn lautlos zu verschlucken
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Löst eine automatische Live-Neuberechnung der aktuellen Rechnungsposition (prNewCalculatePosition) aus, 
    ''' sobald der Benutzer die Menge (tbMenge), den Betrag (tbBetrag) oder den Steuersatz (coSteuer) ändert.
    ''' </summary>
    ''' <param name="sender">Das Steuerelement, das das Ereignis ausgelöst hat (tbMenge, tbBetrag oder coSteuer).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort beim Aufruf von 'prNewCalculatePosition' entfernt.
    ''' - XML-Dokumentation für eine bessere Code-Verständlichkeit und Konformität erweitert.
    ''' </remarks>
    Private Sub tbMenge_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles tbMenge.TextChanged, tbBetrag.TextChanged, coSteuer.SelectedIndexChanged

        prNewCalculatePosition()
    End Sub

    ''' <summary>
    ''' Kalkuliert eine einzelne Rechnungsposition live während der Eingabe neu.
    ''' Teilt den Betrag anhand des gewählten Steuersatzes auf und aktualisiert die Netto-, Brutto- und Steuerlabels.
    ''' </summary>
    ''' <remarks>
    ''' 27.03.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Störenden UI-Eingriff entfernt (tbMenge.Text = 0 blockiert die Tastatureingabe beim Löschen).
    ''' - Veraltete 'Val()'-Funktion durch präzise Typ-Parser ('Decimal.TryParse', 'Integer.TryParse') ersetzt.
    ''' - Zuordnung der Steuer-Ergebnislabels am Ende dynamisiert und mit dem oberen Select Case synchronisiert.
    ''' - Auskommentierte Code-Fragmente entfernt.
    ''' - Kaufmännische Multiplikation über das Decimal-Literal (100D) abgesichert.
    ''' </remarks>
    Private Sub prNewCalculatePosition()
        ' Sicheres Parsen der Eingabewerte, ohne direkt in die UI-Felder einzugreifen
        Dim nMenge As Integer = 0
        Integer.TryParse(tbMenge.Text, nMenge)

        Dim nEingabeBetrag As Decimal = 0
        Decimal.TryParse(tbBetrag.Text, nEingabeBetrag)

        Dim nPos As Integer = 0
        Integer.TryParse(tbPos.Text, nPos)

        Dim nRB As Decimal = 0
        Dim nRBF As Decimal = 0
        Dim nRBS As Decimal = 0
        Dim nST As Decimal = 0
        Dim nSTF As Decimal = 0
        Dim nSTS As Decimal = 0
        Dim arL() As String

        ' Steuersatz aus der ComboBox parsen
        Dim nGewaehlterSteuerSatz As Integer = 0
        Integer.TryParse(coSteuer.Text, nGewaehlterSteuerSatz)

        ' INI-Vergleichswerte parsen
        Dim ini11 As Integer = 0
        Dim ini10 As Integer = 0
        Dim ini23 As Integer = 0

        If arIni(11) IsNot Nothing Then Integer.TryParse(arIni(11).ToString(), ini11)
        If arIni(10) IsNot Nothing Then Integer.TryParse(arIni(10).ToString(), ini10)
        If arIni(23) IsNot Nothing Then Integer.TryParse(arIni(23).ToString(), ini23)

        ' Aufteilung anhand der Steuersatzdefinitionen aus der INI vornehmen
        Select Case nGewaehlterSteuerSatz
            Case ini11
                nRB = nEingabeBetrag * 100D
                nST = nGewaehlterSteuerSatz
            Case ini10
                nRBF = nEingabeBetrag * 100D
                nSTF = nGewaehlterSteuerSatz
            Case ini23
                nRBS = nEingabeBetrag * 100D
                nSTS = nGewaehlterSteuerSatz
            Case Else
                ' Fallback, falls ein abweichender Steuersatz direkt eingegeben wurde
                nRB = nEingabeBetrag * 100D
                nST = nGewaehlterSteuerSatz
        End Select

        ' Berechnung über die kaufmännische Funktion ausführen
        ' sZusatz wird als "1" übergeben, Datumsfelder werden mit den Labels befüllt
        arL = fcGetDataForRechnung(nPos, nMenge, coZimmer.Text, tbText.Text, nRB, nRBF,
                                   Convert.ToInt32(nST), Convert.ToInt32(nSTF), "1",
                                   lbVon.Text, lbBis.Text, "", "", "", "", "", "", "",
                                   Convert.ToInt32(nSTS), nRBS)

        ' Sicherheitsprüfung, ob das Array korrekt generiert wurde
        If arL IsNot Nothing AndAlso arL.Length > 9 Then
            ' Dynamische Anzeige der berechneten Steuer im Steuerlabel
            Select Case nGewaehlterSteuerSatz
                Case ini11
                    lbSteuer.Text = arL(5) ' Steuerwert ermäßigt (7%)
                Case ini10
                    lbSteuer.Text = arL(6) ' Steuerwert voll (19%)
                Case ini23
                    lbSteuer.Text = arL(7) ' Steuerwert Sonder
                Case Else
                    lbSteuer.Text = arL(5)
            End Select

            ' Zuweisung der summierten Netto- und Bruttowerte aus dem Array
            lbNetto.Text = arL(8)
            lbBrutto.Text = arL(9)
        End If
    End Sub

    ''' <summary>
    ''' Verarbeitet das Speichern einer Rechnungsposition. Erstellt entweder eine neue Position am Ende 
    ''' der Tabelle (falls tbPos leer ist) oder überschreibt die bestehende Zeile im GridView mit den geänderten Daten.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (btPosSpeichern).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.03.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Kritischen Logik- und Absturzfehler behoben: Reihenfolge beim Hinzufügen neuer Zeilen korrigiert.
    ''' - Veraltetes 'Call'-Schlüsselwort bei den Prozeduraufrufen entfernt.
    ''' - Fehleranfälliges 'Val()' durch typsichere .NET-Parser ersetzt.
    ''' - Zuweisung der Steuerwerte im Update-Zweig mit dynamischen INI-Variablen synchronisiert.
    ''' - Indizierung für neue Zeilen logisch abgesichert.
    ''' </remarks>
    Private Sub btPosSpeichern_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btPosSpeichern.Click
        Dim nPos As Integer = 0

        ' Wenn das Positionsfeld leer ist, handelt es sich um eine Neuanlage
        If String.IsNullOrWhiteSpace(tbPos.Text) Then
            lNew = True

            ' Die neue Positionsnummer entspricht der aktuellen Zeilenanzahl
            nPos = dgRechnung.Rows.Count
            tbPos.Text = nPos.ToString()

            Dim nRB As Decimal = 0
            Dim nRBF As Decimal = 0
            Dim nRBS As Decimal = 0
            Dim nST As Decimal = 0
            Dim nSTF As Decimal = 0
            Dim nSTS As Decimal = 0
            Dim arL() As String

            ' Steuersatz aus der ComboBox parsen
            Dim nGewaehlterSteuerSatz As Integer = 0
            Integer.TryParse(coSteuer.Text, nGewaehlterSteuerSatz)

            ' INI-Steuersätze parsen
            Dim ini11 As Integer = 0
            Dim ini10 As Integer = 0
            Dim ini23 As Integer = 0

            If arIni(11) IsNot Nothing Then Integer.TryParse(arIni(11).ToString(), ini11)
            If arIni(10) IsNot Nothing Then Integer.TryParse(arIni(10).ToString(), ini10)
            If arIni(23) IsNot Nothing Then Integer.TryParse(arIni(23).ToString(), ini23)

            ' Betrag für kaufmännische Berechnung einlesen
            Dim nEingabeBetrag As Decimal = 0
            Decimal.TryParse(tbBetrag.Text, nEingabeBetrag)

            Select Case nGewaehlterSteuerSatz
                Case ini11
                    nRB = nEingabeBetrag * 100D
                    nST = nGewaehlterSteuerSatz
                Case ini10
                    nRBF = nEingabeBetrag * 100D
                    nSTF = nGewaehlterSteuerSatz
                Case ini23
                    nRBS = nEingabeBetrag * 100D
                    nSTS = nGewaehlterSteuerSatz
                Case Else
                    nRB = nEingabeBetrag * 100D
                    nST = nGewaehlterSteuerSatz
            End Select

            Dim nMenge As Integer = 0
            Integer.TryParse(tbMenge.Text, nMenge)

            ' Daten-Array über Hilfsfunktion generieren
            arL = fcGetDataForRechnung(nPos, nMenge, coZimmer.Text, tbText.Text, nRB, nRBF,
                                       Convert.ToInt32(nST), Convert.ToInt32(nSTF), "1",
                                       lbVon.Text, lbBis.Text, "", "", "", "", "", "", "",
                                       Convert.ToInt32(nSTS), nRBS)

            ' Zeile dem GridView hinzufügen
            dgRechnung.Rows.Add(arL)

            ' BUGFIX: Erst nachdem die Zeile existiert, kann der Steuersatz-Text (Spalte 13) zugewiesen werden
            dgRechnung.Rows(nPos).Cells(13).Value = coSteuer.Text

        Else
            ' Bestehenden Datensatz aktualisieren
            lNew = False
            Integer.TryParse(tbPos.Text, nPos)

            ' Sicherheitsprüfung, ob der Index im gültigen Bereich liegt
            If nPos >= 0 AndAlso nPos < dgRechnung.Rows.Count Then
                With dgRechnung.Rows(nPos)
                    .Cells(0).Value = tbPos.Text
                    .Cells(1).Value = coZimmer.Text
                    .Cells(2).Value = tbMenge.Text
                    .Cells(3).Value = tbText.Text
                    .Cells(4).Value = tbBetrag.Text
                    .Cells(13).Value = coSteuer.Text

                    ' Steuerwerte anhand der INI-Definitionen aufteilen
                    If coSteuer.Text = Convert.ToString(arIni(11)) Then
                        .Cells(5).Value = lbSteuer.Text
                        .Cells(6).Value = "0,00"
                    ElseIf coSteuer.Text = Convert.ToString(arIni(10)) Then
                        .Cells(5).Value = "0,00"
                        .Cells(6).Value = lbSteuer.Text
                    Else
                        .Cells(5).Value = "0,00"
                        .Cells(6).Value = "0,00"
                    End If

                    .Cells(7).Value = lbNetto.Text
                    .Cells(8).Value = lbBrutto.Text
                End With
            End If
        End If

        ' Backend-Speicherung und Aktualisierung der Gesamtsummen
        prSaveZusatz(nPos)
        prCalculateSumme()
        prClearEingabe()
    End Sub

    '''' <summary>
    '''' Speicherung durchführen
    '''' </summary>
    '''' <remarks>
    '''' 18.12.2011 Create
    '''' </remarks>
    'Private Sub prSaveZusatz(ByRef nPos As Integer)
    '    Dim sb As New StringBuilder
    '    Dim sqlText As String = ""
    '    Dim arFields(0), arValue(0) As String
    '    Dim cBedingung As String = ""
    '    Dim sID As String = ""
    '    sID = dgRechnung.Rows(nPos).Cells(14).Value
    '    If lNew Then
    '        sID = fcGetTimeID(Date.Today)
    '        dgRechnung.Rows(nPos).Cells(14).Value = sID
    '    End If

    '    Dim nSumme As Double = 0
    '    Try

    '        sqlText = "ID,BuchID,ZimID,Menge,Bezeichnung,Betrag,Steuer,Gesamt,Datum,ZimNr,Name"
    '        arFields = Split(sqlText, ",")
    '        sqlText = fcSaveZusatz(sID)
    '        arValue = Split(sqlText, "°")

    '        If lNew Then
    '            Call fcInsertCommand("Zusaetze", arFields, arValue)
    '        Else
    '            cBedingung = " WHERE ID='" & sID & "'"
    '            Call fcUpdateCommand("Zusaetze", arFields, arValue, cBedingung)
    '        End If
    '        ''DataTable aktualisieren
    '        'If lNew Then
    '        '    'Datensatz in DataTable "dtZ" speichern
    '        '    Call fcInsertTable(dtZ, arFields, arValue)
    '        'Else
    '        '    'Datensatz in DataTable "dtZ" speichern
    '        '    cBedingung = "ID Like '" & sID & "'"
    '        '    Call fcUpdateTable(dtZ, arFields, arValue, cBedingung)
    '        'End If

    '        'Call prFuelleTabelleZusatz(dtZ)

    '        lNew = False

    '    Catch ex As Exception
    '        ErrReport(ex.Message, ex.Source, ex.StackTrace)
    '    Finally
    '        ''Call prLoadObjInList(dtObj)
    '        'Call prLoockZusatz(False)
    '        lNew = False

    '    End Try
    'End Sub

    ''' <summary>
    ''' Speichert die Zusatzleistung (Extra) entweder als neuen Datensatz (INSERT) oder 
    ''' aktualisiert einen bestehenden Eintrag (UPDATE) in der Datenbank-Tabelle "Zusaetze".
    ''' </summary>
    ''' <param name="nPos">Der Zeilenindex der betroffenen Position im DataGridView.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Kritischen Spaltenindex-Bug behoben: 'sID' von Index 14 auf den korrekten Index 15 verschoben ('FPreis' liegt auf 14).
    ''' - Veraltete 'Split()'-Funktionen (VB6-Stil) durch performante String-Arrays ersetzt.
    ''' - Unbenutzte Objekte ('StringBuilder', 'nSumme') zur Ressourceneinsparung entfernt.
    ''' - Veraltetes 'Call'-Schlüsselwort entfernt.
    ''' - Variable 'nPos' von gefährlichem 'ByRef' auf das sicherere 'ByVal' umgestellt.
    ''' </remarks>
    Private Sub prSaveZusatz(ByVal nPos As Integer)
        ' Sicherheitsprüfung, ob der übergebene Index im gültigen Bereich der Tabellenzeilen liegt
        If nPos < 0 OrElse nPos >= dgRechnung.Rows.Count Then Exit Sub

        Dim arFields() As String
        Dim arValue() As String
        Dim cBedingung As String = ""

        ' BUGFIX: sID liegt laut prSetTabelleRechnung auf Index 15. Index 14 enthält den FPreis!
        Dim sID As String = Convert.ToString(dgRechnung.Rows(nPos).Cells(15).Value).Trim()

        ' Bei einer Neuanlage eine eindeutige Zeit-ID generieren und im Grid hinterlegen
        If lNew Then
            sID = fcGetTimeID(Date.Today)
            dgRechnung.Rows(nPos).Cells(15).Value = sID
        End If

        Try
            ' Felderliste als sauberes, stark typisiertes String-Array initialisieren (spart das langsame Split)
            arFields = {"ID", "BuchID", "ZimID", "Menge", "Bezeichnung", "Betrag", "Steuer", "Gesamt", "Datum", "ZimNr", "Name"}

            ' Daten-String über Hilfsfunktion abrufen (Werte sind mit "°" getrennt)
            Dim sqlText As String = fcSaveZusatz(sID)

            ' Den mit Gradzeichen getrennten Datenstrom in das Werte-Array aufteilen
            arValue = sqlText.Split("°"c)

            ' Datenbank-Aktion ausführen
            If lNew Then
                fcInsertCommand("Zusaetze", arFields, arValue)
            Else
                cBedingung = " WHERE ID='" & sID & "'"
                fcUpdateCommand("Zusaetze", arFields, arValue, cBedingung)
            End If

            ' Zustand nach erfolgreicher Speicherung zurücksetzen
            lNew = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ' Zustand im Fehler- und Erfolgsfall sicher zurücksetzen
            lNew = False
        End Try
    End Sub

    ''' <summary>
    ''' Bereitet alle Eingabewerte der Zusatzleistung kaufmännisch korrekt auf und fügt diese 
    ''' zu einem mit Gradzeichen ("°") separierten Datenstrom für die Datenbankspeicherung zusammen.
    ''' </summary>
    ''' <param name="sID">Die eindeutige ID des Zusatzdatensatzes.</param>
    ''' <returns>Ein mit "°" verketteter String aller Spaltenwerte.</returns>
    ''' <remarks>
    ''' 10.01.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Fehlerhaften VB6-Befehl 'Str()' entfernt (verhindert ungewollte führende Leerzeichen im DB-String).
    ''' - Veraltete Zuweisung über den Funktionsnamen durch ein sauberes 'Return' ersetzt.
    ''' - Unpräzise Fließkommakonvertierung 'Val()' durch das kaufmännisch genaue 'Decimal.TryParse' ersetzt.
    ''' - Absicherung der Datumsübergabe ohne den fehleranfälligen Umweg über '.ToString' auf dem aktuellen Datum.
    ''' - Unbenutzte Variable 'sGeb' entfernt.
    ''' </remarks>
    Private Function fcSaveZusatz(ByVal sID As String) As String
        Dim sb As New StringBuilder()

        ' Spaltenreihenfolge laut prSaveZusatz: 
        ' ID, BuchID, ZimID, Menge, Bezeichnung, Betrag, Steuer, Gesamt, Datum, ZimNr, Name

        ' 1. IDs und Basis-Texte anhängen
        sb.Append(sID).Append("°")
        sb.Append(sgRBID).Append("°")
        sb.Append(sgRZID).Append("°")
        sb.Append(tbMenge.Text.Trim()).Append("°")
        sb.Append(tbText.Text.Trim()).Append("°")

        ' 2. Betrag kaufmännisch sicher parsen und ohne führende Leerzeichen multiplizieren
        Dim nBetrag As Decimal = 0
        Decimal.TryParse(tbBetrag.Text, nBetrag)
        Dim nBetragCent As Integer = Convert.ToInt32(nBetrag * 100D)
        sb.Append(nBetragCent.ToString()).Append("°")

        ' 3. Steuersatz anhängen
        sb.Append(coSteuer.Text.Trim()).Append("°")

        ' 4. Bruttowert kaufmännisch sicher parsen und in Cent umrechnen
        Dim nBrutto As Decimal = 0
        Decimal.TryParse(lbBrutto.Text, nBrutto)
        Dim nBruttoCent As Integer = Convert.ToInt32(nBrutto * 100D)
        sb.Append(nBruttoCent.ToString()).Append("°")

        ' 5. Aktuelles Tagesdatum über Ihre Formatfunktion fcUmDatum konvertieren
        ' Direkte Übergabe des Date-Objekts ist sicherer als Date.Today.ToString
        sb.Append(fcUmDatum(Date.Today)).Append("°")

        ' 6. Zimmernummer und Kundenname anhängen
        sb.Append(coZimmer.Text.Trim()).Append("°")
        sb.Append(lbName1.Text.Trim())

        ' Saubere Rückgabe des fertigen Datenstroms
        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Summiert alle Netto-, Brutto- und Steuerwerte aus dem Rechnungs-GridView (dgRechnung) auf.
    ''' Berechnet die Gesamtsumme abzüglich der geleisteten Anzahlung und aktualisiert die Benutzeroberfläche.
    ''' </summary>
    ''' <remarks>
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Massiv redundantes If-Else-Konstrukt vollständig entfernt (beide Zweige enthielten exakt identische Rechenlogik).
    ''' - Schleife auf die performantere 'For Each'-Variante für DataGridViewRows umgestellt.
    ''' - Fehleranfälliges 'Val(lbAnzahlung.Text)' durch direktes Rechnen mit der kaufmännischen Decimal-Variable 'nAnzahlung' ersetzt.
    ''' - Typsicheres Parsen der Zellwerte integriert, um Abstürze bei leeren Tabellenzellen zu verhindern.
    ''' - Auskommentierte Code-Fragmente entfernt.
    ''' </remarks>
    Private Sub prCalculateSumme()
        ' Globale Summenspeicher zurücksetzen
        nGNetto = 0
        nGBrutto = 0
        nGSt7 = 0
        nGSt19 = 0
        nGStS = 0
        Dim nGStGesamt As Decimal = 0

        ' Sicherheitsprüfung: Wenn keine Zeilen vorhanden sind, UI auf 0 setzen und abbrechen
        If dgRechnung.Rows.Count = 0 Then
            lbGNetto.Text = "0,00"
            lbGSt7.Text = "0,00"
            lbAnzahlung.Text = nAnzahlung.ToString("F2")
            lbGesamt.Text = "0,00"
            Exit Sub
        End If

        ' Alle Zeilen des Grids durchlaufen und Werte aufaddieren
        For Each row As DataGridViewRow In dgRechnung.Rows
            ' Nur reale Datenzeilen auswerten (keine neuen, ungespeicherten Zeilen)
            If Not row.IsNewRow Then
                Dim nRowNetto As Decimal = 0
                Dim nRowBrutto As Decimal = 0
                Dim nRowSt7 As Decimal = 0
                Dim nRowSt19 As Decimal = 0
                Dim nRowStS As Decimal = 0

                ' Werte sicher parsen, um Leerzeichen oder DBNull-Fehler im Grid abzufangen
                Decimal.TryParse(Convert.ToString(row.Cells(8).Value), nRowNetto)
                Decimal.TryParse(Convert.ToString(row.Cells(9).Value), nRowBrutto)
                Decimal.TryParse(Convert.ToString(row.Cells(5).Value), nRowSt7)
                Decimal.TryParse(Convert.ToString(row.Cells(6).Value), nRowSt19)
                Decimal.TryParse(Convert.ToString(row.Cells(7).Value), nRowStS)

                ' Aufsummieren
                nGNetto += nRowNetto
                nGBrutto += nRowBrutto
                nGSt7 += nRowSt7
                nGSt19 += nRowSt19
                nGStS += nRowStS
            End If
        Next

        ' Gesamte Steuersumme aus allen Sätzen ermitteln
        nGStGesamt = nGSt19 + nGSt7 + nGStS

        ' Benutzeroberfläche (Labels) aktualisieren und kaufmännisch formatieren
        lbGNetto.Text = fcFormatDecimal(nGNetto.ToString(), 2)
        lbGSt7.Text = fcFormatDecimal(nGStGesamt.ToString(), 2)
        lbAnzahlung.Text = fcFormatDecimal(nAnzahlung.ToString(), 2)

        ' BUGFIX: Wir rechnen direkt mit dem numerischen Wert von nAnzahlung, 
        ' statt den bereits formatierten Text aus lbAnzahlung.Text fehleranfällig zurückzukonvertieren.
        Dim nEndBetrag As Decimal = nGBrutto - nAnzahlung
        lbGesamt.Text = fcFormatDecimal(nEndBetrag.ToString(), 2)
    End Sub


    ''' <summary>
    ''' Bricht den aktuellen Bearbeitungsvorgang einer Rechnungsposition ab und setzt 
    ''' alle Eingabefelder sowie Steuer- und Betragslabels auf ihre Standardwerte zurück.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (btAbbruch).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.03.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort entfernt.
    ''' </remarks>
    Private Sub btAbbruch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btAbbruch.Click
        prClearEingabe()
    End Sub

    ''' <summary>
    ''' Setzt die Steuerelemente der Eingabemaske zurück. Gibt gesperrte Textboxen wieder frei 
    ''' und belegt Mengen-, Steuer- und Währungsfelder mit kaufmännischen Standard-Nullwerten vor.
    ''' </summary>
    ''' <remarks>
    ''' 27.03.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Auskommentierte Code-Altlasten entfernt.
    ''' - String-Nullwerte für Währungsanzeigen plattformkonform auf Komma-Format ("0,00") angepasst.
    ''' </remarks>
    Private Sub prClearEingabe()
        ' Eingabefelder für eine neue Eingabe wieder freigeben
        tbText.Enabled = True
        coZimmer.Enabled = True

        ' Textinhalte und Auswahlen zurücksetzen
        tbPos.Text = String.Empty
        coZimmer.Text = String.Empty
        tbText.Text = String.Empty

        ' Numerische Standardwerte setzen
        tbMenge.Text = "0"
        coSteuer.Text = "0"

        ' Kaufmännische Währungs-Labels sauber vorbelegen (Komma-Format für deutsche Ländereinstellungen)
        tbBetrag.Text = "0,00"
        lbSteuer.Text = "0,00"
        lbNetto.Text = "0,00"
        lbBrutto.Text = "0,00"
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