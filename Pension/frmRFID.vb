Public Class frmRFID
    Dim bNew As Boolean
    ' Dim arTmp() As String
    ' Dim sTmp As String
    Dim sVon As String
    Dim sBis As String
    Dim nSelect As Integer = 2

    ''' <summary>
    ''' Wird beim Laden des RFID-Formulars ausgelöst. Initialisiert die Steuerelemente, 
    ''' baut die Tabellenstrukturen auf und befüllt die Zeit-Auswahlboxen mit formatierten Werten.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 02.10.2026 - Code-Optimierung:
    ''' - Manuelle String-Erweiterung ("0" & si) durch native .NET-Formatierung ("D2") ersetzt.
    ''' - Fehleranfällige 'Mid'-Funktion bei der Datumsextraktion durch sicheres '.ToShortDateString()' ersetzt.
    ''' - Schleifenvariablen direkt in den For-Schleifen deklariert und typisiert.
    ''' - Veraltete 'ByVal'-Syntax aus Methodensignatur entfernt.
    ''' </remarks>
    Private Sub frmRFID_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Formular- und Grid-Abmessungen festlegen
        Me.Width = 1400
        dgZimmerChip.Width = 695

        ' Standard-Schaltflächenstatus setzen
        prSetMaskenStatus(False)
        prSetButton(True)
        prSetButton1(True)

        ' Tabellen initialisieren und RFID-Daten laden
        prSetTabelleZimCh(dtZim)
        prCreateTabelleRFID()
        prLoadRFIDInList()

        ' 1. Stunden-Auswahlboxen befüllen (00 bis 23)
        cbBisZeit.Items.Clear()
        cbVonZeit.Items.Clear()
        For i As Integer = 0 To 23
            ' "D2" erzwingt zwei Stellen mit führender Null
            Dim formatierteStunde As String = i.ToString("D2")
            cbBisZeit.Items.Add(formatierteStunde)
            cbVonZeit.Items.Add(formatierteStunde)
        Next
        cbVonZeit.Text = "12"
        cbBisZeit.Text = "12"

        ' 2. Minuten-Auswahlboxen in 5-Minuten-Schritten befüllen (00 bis 55)
        cbBisZeitM.Items.Clear()
        cbVonZeitM.Items.Clear()
        For i As Integer = 0 To 55 Step 5
            Dim formatierteMinute As String = i.ToString("D2")
            cbBisZeitM.Items.Add(formatierteMinute)
            cbVonZeitM.Items.Add(formatierteMinute)
        Next
        cbVonZeitM.Text = "00"
        cbBisZeitM.Text = "00"

        ' 3. Aktuell ausgewähltes Datum aus den Kalendern sicher auslesen und konvertieren
        sVon = fcUmDatum(mcVon.SelectionStart.ToShortDateString())
        sBis = fcUmDatum(mcBis.SelectionStart.ToShortDateString())
    End Sub

    ''' <summary>
    ''' Erstellt die Spaltenstruktur für die RFID-Code-Tabelle (ListView)
    ''' und definiert das visuelle Verhalten der Liste.
    ''' </summary>
    ''' <remarks>
    ''' 02.10.2026 - Code-Optimierung:
    ''' - Doppelte Spaltenbezeichnung "Zeit" in "Von Zeit" und "Bis Zeit" zur besseren Übersicht geändert.
    ''' - Namespaces gekürzt (ColumnHeaderStyle.Nonclickable).
    ''' - Auskommentierten 'Totcode' (.Sorting) entfernt.
    ''' </remarks>
    Private Sub prCreateTabelleRFID()
        With lvRFID
            .Clear()

            ' Spaltenkonfiguration definieren
            .Columns.Add("Code", 140, HorizontalAlignment.Left)
            .Columns.Add("Von", 110, HorizontalAlignment.Left)
            .Columns.Add("Von Zeit", 60, HorizontalAlignment.Left)
            .Columns.Add("Bis", 110, HorizontalAlignment.Left)
            .Columns.Add("Bis Zeit", 60, HorizontalAlignment.Left)
            .Columns.Add("Bemerkung", 100, HorizontalAlignment.Left)

            ' Verhalten der ListView festlegen
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub


    ''' <summary>
    ''' Bereinigt veraltete RFID-Code-Datensätze in der Datenbank und befüllt 
    ''' die ListView (lvRFID) mit den aktuell gültigen RFID-Zugangsdaten.
    ''' </summary>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 02.10.2026 - Code-Optimierung:
    ''' - Manuelle Stunden-Formatierung durch native .NET-Formatzeichenfolge ("HH") ersetzt.
    ''' - Auf performantere 'For Each'-Schleife für Datenzeilen umgestellt.
    ''' - XML-Dokumentation inhaltlich korrigiert (RFID statt Zimmer).
    ''' - Schleifenvariablen lokalisiert und typisiert.
    ''' </remarks>
    Private Sub prLoadRFIDInList()
        ' Aktuelles Datum ermitteln und die Stunde immer zweistellig formatieren (z. B. "09" oder "14")
        Dim sZeit As String = TimeOfDay.ToString("HH")
        Dim dDay As Date = Today
        Dim sTag As String = fcUmDatum(dDay.ToString())

        ' Veraltete Datensätze direkt in der Datenbank löschen
        UpdateTable($"DELETE FROM Code WHERE bis < '{sTag}'")

        lvRFID.Items.Clear()

        ' Aktuelle RFID-Codes abrufen
        Dim sSQL As String = "SELECT code, von, vonzeit, bis, biszeit, BID FROM Code"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            For Each row As DataRow In dt.Rows
                ' Neues ListViewItem mit der Hauptspalte "code" erstellen
                Dim lv As ListViewItem = lvRFID.Items.Add(row("code").ToString())

                ' SubItems sicher hinzufügen und Datumsangaben konvertieren
                With lv.SubItems
                    .Add(fcUmDatum(row("von").ToString()))
                    .Add(row("vonzeit").ToString())
                    .Add(fcUmDatum(row("bis").ToString()))
                    .Add(row("biszeit").ToString())
                    .Add(row("BID").ToString())
                End With
            Next
        End If
    End Sub

    ''' <summary>
    ''' Reagiert auf die Änderung der Auswahl in der RFID-ListView und 
    ''' überträgt die Detailinformationen des ausgewählten Codes in die Eingabefelder und Kalender.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 02.10.2026 - Code-Optimierung:
    ''' - Veraltete 'ByVal'-Modifizierer aus der Signatur entfernt.
    ''' - Lokale Variable für das ausgewählte ListViewItem eingeführt.
    ''' - 'Date.TryParse' für eine absturzsichere Zuweisung an die Kalender-Steuerelemente integriert.
    ''' - Veraltete 'Mid'-Funktion durch sichere '.Substring'-Operationen ersetzt.
    ''' </remarks>
    Private Sub lvRFID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvRFID.SelectedIndexChanged
        With lvRFID
            If .SelectedItems.Count > 0 Then
                Dim selectedRow As ListViewItem = .SelectedItems(0)

                ' RFID-Code und Bemerkung direkt zuweisen
                tbRFID.Text = selectedRow.SubItems(0).Text
                tbBemerkung.Text = selectedRow.SubItems(5).Text

                ' Von-Datum sicher parsen und dem Kalender übergeben
                Dim dVon As Date
                If Date.TryParse(selectedRow.SubItems(1).Text, dVon) Then
                    mcVon.SelectionStart = dVon
                    mcVon.SelectionEnd = dVon
                End If

                ' Von-Zeit extrahieren (Stundenanteil, die ersten 2 Zeichen)
                Dim vonZeitRaw As String = selectedRow.SubItems(2).Text
                If vonZeitRaw.Length >= 2 Then
                    cbVonZeit.Text = vonZeitRaw.Substring(0, 2)
                End If

                ' Bis-Datum sicher parsen und dem Kalender übergeben
                Dim dBis As Date
                If Date.TryParse(selectedRow.SubItems(3).Text, dBis) Then
                    mcBis.SelectionStart = dBis
                    mcBis.SelectionEnd = dBis
                End If

                ' Bis-Zeit extrahieren (Stundenanteil, die ersten 2 Zeichen)
                Dim bisZeitRaw As String = selectedRow.SubItems(4).Text
                If bisZeitRaw.Length >= 2 Then
                    cbBisZeit.Text = bisZeitRaw.Substring(0, 2)
                End If
            End If
        End With
    End Sub

    ''' <summary>
    ''' Bereitet die Eingabemaske für die Erfassung eines neuen RFID-Codes vor.
    ''' Setzt die Textfelder zurück, aktiviert die Steuerelemente und initialisiert die Kalenderwerte.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 02.10.2026 - Code-Optimierung:
    ''' - Fehleranfällige implizite String-zu-Datum-Konvertierung (.ToString) bei den Kalendern entfernt.
    ''' - Direkte und typsichere Übergabe des DateTime-Objekts (Date.Today) implementiert.
    ''' - Veraltete 'ByVal'-Modifizierer entfernt.
    ''' </remarks>
    Private Sub tsbNew_Click(sender As Object, e As EventArgs) Handles tsbNew.Click
        bNew = True
        prSetMaskenStatus(True)
        prSetButton(False)
        ' Eingabefelder zurücksetzen
        tbRFID.Text = ""
        tbBemerkung.Text = ""
        tbRFID.Focus()

        ' Kalenderwerte typsicher auf heute (Start) und morgen (Ende) setzen
        Dim heute As Date = Date.Today
        mcVon.SelectionStart = heute
        mcVon.SelectionEnd = heute

        Dim morgen As Date = Date.Today.AddDays(1)
        mcBis.SelectionStart = morgen
        mcBis.SelectionEnd = morgen
    End Sub

    ''' <summary>
    ''' Schließt das RFID-Verwaltungsformular.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    Private Sub tsbClose_Click(sender As Object, e As EventArgs) Handles tsbClose.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Wird aufgerufen, während das Formular geschlossen wird. Bereinigt ungenutzten Totcode.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die FormClosing-Ereignisdaten.</param>
    Private Sub frmRFID_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ' Totcode entfernt, um die Methode sauber zu halten
    End Sub

    ''' <summary>
    ''' Schaltet die Maske in den Bearbeitungsmodus für einen bestehenden RFID-Code um.
    ''' Aktiviert die Steuerelemente, sperrt jedoch das RFID-Schlüsselfeld gegen Manipulation.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    Private Sub tsbEdit_Click(sender As Object, e As EventArgs) Handles tsbEdit.Click
        bNew = False
        prSetMaskenStatus(True)
        prSetButton(False)
        tbRFID.Enabled = False
    End Sub

    ''' <summary>
    ''' Bricht den aktuellen Bearbeitungs- oder Erfassungsmodus ab und sperrt die Eingabefelder wieder.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    Private Sub tsbReturn_Click(sender As Object, e As EventArgs) Handles tsbReturn.Click
        prSetMaskenStatus(False)
        prSetButton(True)
    End Sub

    ''' <summary>
    ''' Verarbeitet den Klick auf die Löschen-Schaltfläche.
    ''' Fordert eine Bestätigung an, sendet einen Löschbefehl (mit historischem Datum) an das RFID-Terminal
    ''' und entfernt den Datensatz anschließend aus der Datenbank.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 02.10.2026 - Code-Optimierung:
    ''' - Sicherheitsabfrage (MessageBox) vor dem Löschen integriert.
    ''' - Logikfehler behoben, bei dem die Bereinigungsfunktion fcWeg34 sofort überschrieben wurde.
    ''' - Ungenutzte Variablen entfernt und ByVal bereinigt.
    ''' </remarks>
    Private Sub tsbDel_Click(sender As Object, e As EventArgs) Handles tsbDel.Click
        Dim rawRFID As String = tbRFID.Text.Trim()

        If String.IsNullOrEmpty(rawRFID) Then
            MessageBox.Show("Bitte wählen Sie zuerst einen RFID-Code aus.", "Keine Auswahl", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim sMsg As String = $"Möchten Sie den RFID-Chip '{rawRFID}' wirklich löschen?"
        If MessageBox.Show(sMsg, "RFID-Code löschen", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = DialogResult.OK Then
            ' RFID-Code bereinigen (Falls fcWeg34 zwingend benötigt wird, hier nutzen. Sonst Text verwenden)
            Dim sRFID As String = If(rawRFID.Length = 19, rawRFID, fcWeg34(rawRFID))

            ' Löschen durch Senden eines abgelaufenen Datums an das Terminal
            prSendCode(sRFID, "20000101", "1200", "20000202", "1200")

            ' Aus Datenbank entfernen (Status "0" signalisiert Löschen/Inaktiv)
            prSaveSatz($"{sRFID}°20000101°1200°20000202°1200°0", False)

            prLoadRFIDInList()
        End If
    End Sub

    ''' <summary>
    ''' Verarbeitet den Klick auf die Speichern-Schaltfläche.
    ''' Validiert die RFID-Eingabe, prüft bei Neuanlagen auf Duplikate, sichert den Datensatz 
    ''' in der Datenbank und überträgt die Berechtigung an das RFID-Terminal.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 02.10.2026 - Code-Optimierung:
    ''' - Kritischen Fehler behoben: Methode bricht nun bei bereits vergebenem Chip korrekt ab (Exit Sub).
    ''' - 'MsgBox' durch modernes 'MessageBox.Show' ersetzt.
    ''' - Stringkonstruktion durch lesbare String-Interpolation ($) modernisiert.
    ''' - Doppelte Codeaufrufe strukturell bereinigt.
    ''' </remarks>
    Private Sub tsbSave_Click(sender As Object, e As EventArgs) Handles tsbSave.Click
        Dim rawText As String = tbRFID.Text.Trim()
        Dim sRFID As String = ""

        ' 1. RFID-Code basierend auf der Länge formatieren/bereinigen
        If rawText.Length <> 5 Then
            sRFID = If(rawText.Length = 19, rawText, fcWeg34(rawText))
        Else
            sRFID = fcCode(rawText)
        End If

        ' Validierung: Leere Eingabe verhindern
        If String.IsNullOrEmpty(sRFID) Then
            MessageBox.Show("Ungültiger RFID-Code.", "Eingabefehler", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Zeit-Strings zusammensetzen
        Dim vonZeitGesamt As String = $"{cbVonZeit.Text}{cbVonZeitM.Text}"
        Dim bisZeitGesamt As String = $"{cbBisZeit.Text}{cbBisZeitM.Text}"

        ' 2. Bei Neuanlagen prüfen, ob der RFID-Chip bereits existiert
        If bNew Then
            Dim sSQL As String = $"Select code FROM Code WHERE code = '{sRFID}'"
            Dim dt As DataTable = fcReadDataTable(sSQL)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                MessageBox.Show("Dieser RFID-Chip ist bereits vergeben!", "Doppelter Eintrag", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                tbRFID.Focus()
                Exit Sub ' Vorgang abbrechen, um kein Duplikat/Fehlüberschreibung zu erzeugen
            End If

            ' Datensatz vorbereiten und in DB speichern
            Dim dataString As String = $"{sRFID}°{sVon}°{vonZeitGesamt}°{sBis}°{bisZeitGesamt}°{tbBemerkung.Text}"
            prSaveSatz(dataString, bNew)
        End If

        ' 3. Daten an das RFID-Terminal übertragen
        prSendCode(sRFID, sVon, vonZeitGesamt, sBis, bisZeitGesamt)

        ' 4. Ansicht aktualisieren und Maske sperren
        prLoadRFIDInList()
        prSetMaskenStatus(False)
        prSetButton(False)
    End Sub

    ''' <summary>
    ''' Speichert oder aktualisiert einen Datensatz in der Datenbank anhand einer getrennten Zeichenkette.
    ''' Teilt die übergebenen Werte auf und führt je nach Status eine Insert- oder Update-Operation aus.
    ''' </summary>
    ''' <param name="Var">Die per Trennzeichen (°) strukturierten Datenwerte des Datensatzes.</param>
    ''' <param name="bNew">Bestimmt, ob ein neuer Datensatz eingefügt (True) oder ein bestehender aktualisiert wird (False).</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort entfernt.
    ''' - Array-Initialisierung und Split-Funktion auf native .NET-Methoden modernisiert.
    ''' - String-Konstruktion für die WHERE-Bedingung via String-Interpolation ($) vereinfacht.
    ''' - Expliziten Boolean-Vergleich (bNew = True) auf direkten Ausdruck verkürzt.
    ''' </remarks>
    Private Sub prSaveSatz(ByRef Var As String, ByRef bNew As Boolean)
        Const FeldStruktur As String = "code°von°vonzeit°bis°biszeit°BID"

        Dim arFields As String() = FeldStruktur.Split("°"c)
        Dim arValue As String() = Var.Split("°"c)

        If bNew Then
            fcInsertCommand("Code", arFields, arValue)
        Else
            Dim cBedingung As String = $" WHERE code='{arValue(0)}'"
            fcUpdateCommand("Code", arFields, arValue, cBedingung)
        End If
    End Sub

    ''' <summary>
    ''' Verarbeitet die Änderung des Startdatums im Kalender (mcVon).
    ''' Formatiert das ausgewählte Startdatum über die Hilfsfunktion fcUmDatum.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten mit dem gewählten Datumsbereich.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Fehleranfällige 'Mid'- und 'ToString'-Konvertierung durch native '.SelectionStart.ToShortDateString()' ersetzt.
    ''' </remarks>
    Private Sub mcVon_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles mcVon.DateChanged
        sVon = fcUmDatum(mcVon.SelectionStart.ToShortDateString())
    End Sub

    ''' <summary>
    ''' Verarbeitet die Änderung des Enddatums im Kalender (mcBis).
    ''' Formatiert das ausgewählte Enddatum über die Hilfsfunktion fcUmDatum.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten mit dem gewählten Datumsbereich.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - 'Mid' durch die saubere .NET-Methode 'ToShortDateString()' ersetzt.
    ''' </remarks>
    Private Sub mcBis_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles mcBis.DateChanged
        sBis = fcUmDatum(mcBis.SelectionStart.ToShortDateString())
    End Sub

    ''' <summary>
    ''' Steuert die Aktivierung (Enabled) der Eingabemaske für die RFID- und Datumseinstellungen.
    ''' </summary>
    ''' <param name="bAktiv">Übergibt True, um die Bearbeitung zu erlauben, oder False, um die Maske zu sperren.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create (als prEnabled und prDisEnabled)
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Redundante Methoden 'prEnabled' und 'prDisEnabled' in eine zentrale Steuerungsfunktion zusammengeführt.
    ''' - Erhöht die Wartbarkeit und verhindert doppelten Code.
    ''' </remarks>
    Private Sub prSetMaskenStatus(ByVal bAktiv As Boolean)
        tbRFID.Enabled = bAktiv
        lvRFID.Enabled = Not bAktiv
        mcVon.Enabled = bAktiv
        mcBis.Enabled = bAktiv
        cbBisZeit.Enabled = bAktiv
        cbVonZeit.Enabled = bAktiv
        cbBisZeitM.Enabled = bAktiv
        cbVonZeitM.Enabled = bAktiv
        tbBemerkung.Enabled = bAktiv

        ' Sonderfall aus prDisEnabled: Wird nur deaktiviert, wenn bAktiv True ist (bzw. umgekehrt)
        dgZimmerChip.Enabled = Not bAktiv
    End Sub

    ''' <summary>
    ''' Steuert die Aktivierung (Enabled) der Buttons
    ''' </summary>
    ''' <param name="bAktiv">Übergibt True, um die Bearbeitung zu erlauben, oder False, um die Button zu sperren.</param>
    ''' <remarks>
    ''' 03.10.2026 - Create
    ''' </remarks>
    Private Sub prSetButton(ByVal bAktiv As Boolean)
        tsbNew.Enabled = bAktiv
        tsbEdit.Enabled = bAktiv
        tsbSave.Enabled = Not bAktiv
        tsbReturn.Enabled = Not bAktiv
        tsbDel.Enabled = bAktiv
    End Sub

    ''' <summary>
    ''' Steuert die Aktivierung (Enabled) der Buttons1
    ''' </summary>
    ''' <param name="bAktiv">Übergibt True, um die Bearbeitung zu erlauben, oder False, um die Button1 zu sperren.</param>
    ''' <remarks>
    ''' 03.10.2026 - Create
    ''' </remarks>
    Private Sub prSetButton1(ByVal bAktiv As Boolean)
        tsbSave1.Enabled = Not bAktiv
        tsbReturn1.Enabled = Not bAktiv
    End Sub

    ''' <summary>
    ''' Verarbeitet den Klick auf die Schaltfläche zum Einlesen der RFID-Karten.
    ''' Öffnet das entsprechende Dialogfenster (frmReadRFID) zur Erfassung der Chip-Daten.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Instanzprüfung hinzugefügt: Verhindert das doppelte Öffnen des Formulars.
    ''' - 'Activate' integriert, um ein bereits geöffnetes Fenster in den Vordergrund zu holen.
    ''' </remarks>
    Private Sub tsbReadRFID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbReadRFID.Click
        If frmReadRFID.Visible Then
            frmReadRFID.Activate()
        Else
            frmReadRFID.Show()
        End If
    End Sub

    ''' <summary>
    ''' Erstellt die Tabellenstruktur im DataGridView (dgZimmerChip) und befüllt es
    ''' mit den Zimmer- und Transponderdaten aus der übergebenen DataTable.
    ''' </summary>
    ''' <param name="dt">Die DataTable, welche die Quellbepflanzung der Zimmer und Chips enthält.</param>
    ''' <remarks>
    ''' 24.12.2011 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Performance-Optimierung: Befüllung der Zeilen via 'AddRange' beschleunigt, statt jede Zeile einzeln visuell hinzuzufügen.
    ''' - Explizite Variablendeklaration (i As Integer) in den For-Schleifen erzwungen.
    ''' - Speicherzugriff optimiert: Direkte Instanziierung von 'DataGridViewRow'-Objekten für den Massenimport.
    ''' </remarks>
    Private Sub prSetTabelleZimCh(ByVal dt As DataTable)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

        Dim nMax As Integer = dt.Rows.Count - 1

        With dgZimmerChip
            .MultiSelect = False
            .Columns.Clear()
            .ColumnHeadersHeight = 30

            ' Spalten definieren
            .Columns.Add("Zimmer", "Zimmer")
            .Columns.Add("Chip1", "Chip1")
            .Columns.Add("Chip2", "Chip2")
            .Columns.Add("Chip3", "Chip3")
            .Columns.Add("Chip4", "Chip4")
            .Columns.Add("Chip5", "Chip5")

            ' Spalten-Layout einrichten
            .Columns(0).Width = 70
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            For i As Integer = 1 To 5
                .Columns(i).Width = 120
                .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Next

            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            .ReadOnly = True

            ' Farben der selektierten Zeile definieren
            With .DefaultCellStyle
                .SelectionBackColor = cgColorRow
                .SelectionForeColor = Color.Black
            End With

            ' Performance-Boost: Zeilen-Array vorbereiten und gesammelt übergeben
            Dim gridRows(nMax) As DataGridViewRow

            For i As Integer = 0 To nMax
                Dim row As New DataGridViewRow()
                row.CreateCells(dgZimmerChip)

                row.Cells(0).Value = dt.Rows(i).Item("Name").ToString()
                row.Cells(1).Value = dt.Rows(i).Item("Trans1").ToString()
                row.Cells(2).Value = dt.Rows(i).Item("Trans2").ToString()
                row.Cells(3).Value = dt.Rows(i).Item("Trans3").ToString()
                row.Cells(4).Value = dt.Rows(i).Item("Trans4").ToString()
                row.Cells(5).Value = dt.Rows(i).Item("Trans5").ToString()

                gridRows(i) = row
            Next

            ' Alle Zeilen auf einmal hinzufügen
            .Rows.AddRange(gridRows)

            .AutoResizeRows()
            .MultiSelect = True
        End With
    End Sub

    ''' <summary>
    ''' Reagiert auf das Betreten einer Zelle im DataGridView (dgZimmerChip).
    ''' Schaltet bei Klick auf Spalte 0 alle Transponder der Zeile um, ansonsten gezielt den angewählten Transponder.
    ''' Markierte Transponder erhalten eine farbliche Kennzeichnung und ein führendes "x".
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten mit Zeilen- und Spaltenindex.</param>
    ''' <remarks>
    ''' 27.12.2011 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Fehleranfällige Schleifen-Abfrage korrigiert (zuvor wurde 5-mal starr Zelle 1 statt Zelle 'i' geprüft).
    ''' - Redundante UI-Logik in die Hilfsprozedur 'prToggleChipZelle' ausgelagert (DRY-Prinzip).
    ''' - VB6-Legacy-Funktion 'Mid' durch moderne .NET-Methoden '.StartsWith()' und '.Substring()' ersetzt.
    ''' - Explizite Typisierung der Schleifen-Laufvariablen ergänzt.
    ''' </remarks>
    Private Sub dgZimmerChip_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgZimmerChip.CellEnter
        ' Zeilen- und Spaltenindex auf Gültigkeit prüfen (Header-Klicks ignorieren)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

        If nSelect <> 2 Then
            Try
                Dim currentRow As DataGridViewRow = dgZimmerChip.Rows(e.RowIndex)

                If e.ColumnIndex = 0 Then
                    ' Prüfen, ob mindestens eine der Chip-Zellen (Spalte 1 bis 5) bereits markiert ist
                    Dim bBereitsMarkiert As Boolean = False
                    For i As Integer = 1 To 5
                        If currentRow.Cells(i).Style.BackColor = Color.YellowGreen Then
                            bBereitsMarkiert = True
                            Exit For
                        End If
                    Next

                    ' Zustand für alle 5 Chip-Spalten umschalten
                    For i As Integer = 1 To 5
                        ' Wenn bereits markiert -> alle abwählen (False), sonst alle anwählen (True)
                        prToggleChipZelle(currentRow.Cells(i), Not bBereitsMarkiert)
                    Next
                Else
                    ' Zustand für die einzeln ausgewählte Chip-Zelle invertieren
                    Dim bIstMarkiert As Boolean = (currentRow.Cells(e.ColumnIndex).Style.BackColor = Color.YellowGreen)
                    prToggleChipZelle(currentRow.Cells(e.ColumnIndex), Not bIstMarkiert)
                End If

            Catch ex As Exception
                ErrReport(ex.Message, ex.Source, ex.StackTrace)
            End Try
        End If
        nSelect = 0
    End Sub

    ''' <summary>
    ''' Schaltet den Status einer einzelnen DataGridView-Zelle (Farbe und "x"-Präfix) um.
    ''' </summary>
    ''' <param name="cell">Die zu manipulierende DataGridViewCell.</param>
    ''' <param name="bMarkieren">True, um die Zelle auszuwählen (YellowGreen / "x"), False für den Standardzustand (Weiß / ohne "x").</param>
    Private Sub prToggleChipZelle(ByVal cell As DataGridViewCell, ByVal bMarkieren As Boolean)
        Dim sValue As String = If(cell.Value IsNot Nothing, cell.Value.ToString(), "")

        If bMarkieren Then
            cell.Style.BackColor = Color.YellowGreen
            If Not sValue.StartsWith("x") Then
                cell.Value = "x" & sValue
            End If
        Else
            cell.Style.BackColor = Color.White
            If sValue.StartsWith("x") Then
                cell.Value = sValue.Substring(1)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Schaltet die Benutzeroberfläche in den Modus für die Sammel-Chip-Bearbeitung um oder verlässt diesen wieder.
    ''' Aktiviert bzw. deaktiviert die entsprechenden Menüleisten-Schaltflächen (ToolStrips) und Eingabefelder.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.12.2011 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Redundante If-Else-Struktur vollständig aufgelöst.
    ''' - UI-Statussteuerung über logische Invertierung (Not) zusammengefasst.
    ''' - Erhöht die Übersichtlichkeit und Wartbarkeit massiv (Code-Umfang halbiert).
    ''' </remarks>
    Private Sub tsbSammelChip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSammelChip.Click
        ' Den aktuellen Zustand sichern (wird als Basis für das Umschalten genutzt)
        Dim bSammelModusAktiv As Boolean = tsbNew.Enabled

        ' Standard-Aktionen deaktivieren, wenn der Sammelmodus betreten wird (und umgekehrt)
        Dim bStandardAktiv As Boolean = Not bSammelModusAktiv

        tsbNew.Enabled = bStandardAktiv
        tsbEdit.Enabled = bStandardAktiv
        tsbDel.Enabled = bStandardAktiv
        tsbReturn.Enabled = bStandardAktiv
        tsbSave.Enabled = bStandardAktiv

        ' Sammel-Schaltflächen und Eingabemaske spiegelverkehrt schalten
        tsbReturn1.Enabled = bSammelModusAktiv
        tsbSave1.Enabled = bSammelModusAktiv

        dgZimmerChip.Enabled = bSammelModusAktiv
        mcVon.Enabled = bSammelModusAktiv
        mcBis.Enabled = bSammelModusAktiv
        cbBisZeit.Enabled = bSammelModusAktiv
        cbBisZeitM.Enabled = bSammelModusAktiv
        cbVonZeit.Enabled = bSammelModusAktiv
        cbVonZeitM.Enabled = bSammelModusAktiv
    End Sub

    ''' <summary>
    ''' Setzt alle markierten Transponder im DataGridView (dgZimmerChip) zurück.
    ''' Entfernt bei allen Zellen das führende "x", welches zur Auswahlmarkierung genutzt wurde.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.12.2011 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Fehleranfällige VB6-Legacy-Funktion 'Mid' durch moderne .NET-Methoden '.StartsWith()' und '.Substring()' ersetzt.
    ''' - Null-Checks integriert: Verhindert Abstürze bei leeren Zellen oder nicht vorhandenen Zeilen.
    ''' - Explizite Variablendeklaration (i, j As Integer) für die Schleifen erzwungen.
    ''' - Unnötige Abhängigkeit von 'dtZim.Rows.Count' gelöst; die Schleife läuft nun direkt und sicher über die Zeilen des Grids.
    ''' </remarks>
    Private Sub tsbReturn1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbReturn1.Click
        ' Prüfen, ob überhaupt Zeilen im DataGridView vorhanden sind
        If dgZimmerChip.Rows.Count = 0 Then Exit Sub

        Dim nMax As Integer = dgZimmerChip.Rows.Count - 1

        For i As Integer = 0 To nMax
            ' Sicherstellen, dass es sich um eine reguläre Datenzeile handelt
            If dgZimmerChip.Rows(i).IsNewRow Then Continue For

            For j As Integer = 1 To 5
                Dim cell As DataGridViewCell = dgZimmerChip.Rows(i).Cells(j)

                ' Null-Check für den Zellwert
                If cell.Value IsNot Nothing AndAlso Not IsDBNull(cell.Value) Then
                    Dim sTrans As String = cell.Value.ToString()

                    ' Prüfen und "x" entfernen
                    If sTrans.StartsWith("x") Then
                        cell.Value = sTrans.Substring(1)
                    End If
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' Verarbeitet den Klick auf die Sammel-Speichern-Schaltfläche (tsbSave1).
    ''' Identifiziert alle mit "x" markierten Transponder, prüft in der Datenbank auf Existenz (Insert/Update)
    ''' und speichert die geänderten Sätze. Schließt den Vorgang mit der Terminal-Übertragung ab.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.12.2011 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - String-Konstruktion vollständig auf lesbare String-Interpolation ($) modernisiert.
    ''' - 'Mid' und 'Trim' durch native .NET-Methoden '.StartsWith()', '.Substring()' und '.Trim()' ersetzt.
    ''' - Redundante UI-Steuerung am Ende durch Wiederverwendung der Methode 'tsbSammelChip_Click' (oder prSetMaskenStatus) ersetzt.
    ''' - Robustheit erhöht durch Null-Checks bei den Zellwerten des DataGridViews.
    ''' </remarks>
    Private Sub tsbSave1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSave1.Click
        If dgZimmerChip.Rows.Count = 0 Then Exit Sub

        Dim nMax As Integer = dgZimmerChip.Rows.Count - 1

        ' Zusammensetzen des Code-Anhangs mittels String-Interpolation
        Dim sCodeAnhang As String = $"°{sVon}°{cbVonZeit.Text}{cbVonZeitM.Text}°{sBis}°{cbBisZeit.Text}{cbBisZeitM.Text}°"
        sgCodeNew = ""

        For i As Integer = 0 To nMax
            If dgZimmerChip.Rows(i).IsNewRow Then Continue For

            ' Zimmername aus Spalte 0 auslesen
            Dim cellZimmer As DataGridViewCell = dgZimmerChip.Rows(i).Cells(0)
            Dim sZimmer As String = If(cellZimmer.Value IsNot Nothing, cellZimmer.Value.ToString(), "")

            For j As Integer = 1 To 5
                Dim cellChip As DataGridViewCell = dgZimmerChip.Rows(i).Cells(j)

                If cellChip.Value IsNot Nothing AndAlso Not IsDBNull(cellChip.Value) Then
                    Dim sTrans As String = cellChip.Value.ToString()

                    ' Nur markierte Einträge verarbeiten
                    If sTrans.StartsWith("x") Then
                        ' Das "x" entfernen und den eigentlichen Code isolieren
                        Dim sCleanCode As String = sTrans.Substring(1).Trim()

                        If sCleanCode <> "" Then
                            ' Vollständigen Datensatz-String bauen (code°von°vonzeit°bis°biszeit°BID)
                            Dim sCodeNew As String = $"{sCleanCode}{sCodeAnhang}{sZimmer}"

                            ' Prüfen, ob der Code bereits in der DB existiert
                            Dim sSQL As String = $"Select * From Code WHERE code = '{sCleanCode}'"
                            Dim dt As DataTable = fcReadDataTable(sSQL)

                            Dim bNewRecord As Boolean = (dt.Rows.Count = 0)

                            ' Datensatz speichern (Nutzt die optimierte prSaveSatz)
                            prSaveSatz(sCodeNew, bNewRecord)
                        End If
                    End If
                End If
            Next
        Next

        ' Daten an Terminals übertragen und Listen aktualisieren
        prSendCodeAll()
        prLoadRFIDInList()

        ' UI-Zustand zurücksetzen: Simuliert das erneute Klicken auf tsbSammelChip,
        ' um den Sammelmodus sauber zu beenden und die Maske wieder zu sperren.
        tsbSammelChip_Click(sender, e)
    End Sub

    ''' <summary>
    ''' Überträgt die Berechtigungsdaten eines einzelnen Transponders an das RFID-Terminal (PHP-Webservice).
    ''' </summary>
    ''' <param name="sc1">Der einzulesende Chip- oder Transpondercode.</param>
    ''' <param name="von">Das Startdatum der Gültigkeit.</param>
    ''' <param name="vonzeit">Die Startuhrzeit der Gültigkeit.</param>
    ''' <param name="bis">Das Enddatum der Gültigkeit.</param>
    ''' <param name="biszeit">Die Enduhrzeit der Gültigkeit.</param>
    ''' <remarks>
    ''' 27.12.2011 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - String-Konstruktion auf moderne String-Interpolation ($) umgestellt.
    ''' - 'Try-Finally'-Block hinzugefügt: Garantiert das Zurücksetzen des Cursors bei Verbindungsfehlern.
    ''' </remarks>
    Private Sub prSendCode(ByRef sc1 As String, ByVal von As String, ByRef vonzeit As String, ByRef bis As String, ByRef biszeit As String)
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim C As String = $"{sc1}°{von}°{vonzeit}°{bis}°{biszeit}|"
            Dim sIP As String = cgIPWeb

            PHP.Data(C, sIP)
        Finally
            ' Wird auch im Fehlerfall (z. B. Netzwerk-Timeout) zwingend ausgeführt
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Signalisiert dem RFID-Terminal (PHP-Webservice), alle anstehenden Datensätze gesammelt zu verarbeiten.
    ''' </summary>
    ''' <remarks>
    ''' 27.12.2011 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Absicherung der Benutzeroberfläche über 'Try-Finally' integriert, um ein Einfrieren des Cursors zu verhindern.
    ''' </remarks>
    Private Sub prSendCodeAll()
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim sIP As String = cgIPWeb
            PHP.DataAll(sIP)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub


End Class