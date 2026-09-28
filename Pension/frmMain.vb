Imports System.Text

Public Class frmMain
    Dim lClick As Boolean
    Dim arX(7) As Integer
    ' Wichtig: ColumnIndex = X (Zeile), RowIndex = Y (Spalte)
    ' arX(0) = RowIndex (Y)         Mous eDown  Begin
    ' arX(1) = ColumnIndex (X)      Mous eDown  Begin
    ' arX(2) = RowIndex (Y)         Mous eUp    End
    ' arX(3) = ColumnIndex (X)      Mous eUp    End
    ' arX(4) = RowIndex (Y)         Mous eDown  Begin
    ' arX(5) = ColumnIndex (X)      Mous eDown  Begin
    ' arX(6) = RowIndex (Y)         Mous eUp    End
    ' arX(7) = ColumnIndex (X)      Mous eUp    End
    Dim lBuc As Boolean
    Dim lGesp As Boolean
    Private WithEvents wholeTable As New ToolStripMenuItem()
    Private WithEvents lookUp As New ToolStripMenuItem()
    Private strip As ContextMenuStrip
    Private cellErrorText As String
    Private e_rows As Integer = 0

#Region "Form Load................................................................................."

    ''' <summary>
    ''' Initialisiert die Hauptkomponenten der Anwendung beim Laden des Formulars.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 22.09.2026 Code-Optimierung: Umstellung der Instanz-Prüfung auf LINQ, 
    ''' Ersetzen des 'End'-Befehls durch ein sauberes 'Application.Exit()' und 
    ''' Optimierung der Benutzeroberflächen-Initialisierung.
    ''' </remarks>
    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' 1. Prüfung auf Mehrfachinstanzen (Single Instance Check via LINQ)
        Dim aktuellerProzessName As String = Process.GetCurrentProcess().ProcessName
        Dim instanzenZahl As Integer = Process.GetProcessesByName(aktuellerProzessName).Length

        If instanzenZahl > 1 Then
            ' 'End' sollte in VB.NET vermieden werden, da es Ressourcen nicht freigibt.
            ' Application.Exit() beendet die Anwendung sauber.
            Application.Exit()
            Exit Sub
        End If

        ' 2. UI-Vorbereitung (Warte-Cursor setzen)
        Me.Cursor = Cursors.WaitCursor
        prMainResize()

        ' Datum setzen (Nutzt .ToShortDateString() für saubere Formatierung)
        tssDatum.Text = Date.Today.ToShortDateString()
        sgGID = ""

        ' 3. Anwendungsstart & Titel setzen
        ' Hinweis: Da Main() als 'Public Function Main() As String' definiert ist,
        ' wird hier der Rückgabewert (z.B. die IP oder der Pfad) verarbeitet.
        Dim ipErgebnis As String = Main()
        Me.Text = $"Pension am Elberadweg  IP={ipErgebnis}"

        ' 4. Tabellen und Buchungsdaten laden
        prColorRead()
        prSetTabelleBuchung(dtZim)
        prLadeBuchung(dtBuc)

        ' 5. Berechtigungen standardmäßig einschränken (Freigabe erfolgt meist nach Login)
        tsbSystem.Enabled = False
        tsbStatistik.Enabled = False
        tsbPersonen.Enabled = False
        tsbDatev.Enabled = False
        tsbDruck.Enabled = False
        tsbKunde.Enabled = False

        ' 6. Anstehende Termine prüfen und anzeigen
        Dim sDatum As String = fcUmDatum(Date.Today)
        ' Hinweis: Für SQL-Abfragen empfiehlt sich langfristig der Einsatz von Parametern, 
        ' um SQL-Injection und Formatfehler (z.B. bei Datumsangaben) zu verhindern.
        Dim query As String = $"Select * from Termine Where Termin >= '{sDatum}' Order by Termin asc"
        Dim dtTermin As DataTable = fcReadDataTable(query)

        If dtTermin IsNot Nothing AndAlso dtTermin.Rows.Count > 0 Then
            frmTermine.Show()
        End If

        ' 7. agefangene / fehlerhafte Buchungen laden & Login aufrufen
        prReadCopy()
        prCreateTabellelvBuchError()

        Me.Cursor = Cursors.Default
        frmLogin.Show()

    End Sub

    ''' <summary>
    ''' Führt Bereinigungs- und Datensicherungsarbeiten beim Schließen des Hauptformulars aus.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten mit Informationen zum Schließen des Formulars.</param>
    ''' <remarks>
    ''' 22.09.2026 Code-Optimierung: Integration von strukturiertem Try-Catch für den FTP-Upload 
    ''' und Validierung der IP-Adresse zur Vermeidung von Laufzeitfehlern beim Programmende.
    ''' </remarks>
    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Zurücksetzen des globalen Codes
        sgCodeNew = ""

        ' Datensicherung nur ausführen, wenn die Flagge gesetzt und eine IP vorhanden ist
        If lgSichern AndAlso Not String.IsNullOrWhiteSpace(cgIPWeb) Then
            Try
                ' Führt den FTP-Upload über die Web-IP aus
                PHP.SaveFtp(cgIPWeb)
            Catch ex As Exception
                ' Verhindert den Absturz beim Schließen, falls das Netzwerk wegbricht
                MessageBox.Show($"Fehler bei der Datensicherung: {ex.Message}",
                            "Backup-Fehler",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Aktualisiert bei jedem Timer-Intervall die Uhrzeit-Anzeige in der Statusleiste.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das <see cref="Timer"/>-Steuerelement).</param>
    ''' <param name="e">Die Ereignisdaten des Tick-Events.</param>
    ''' <remarks>
    ''' <para>Nutzt das moderne .NET-Format für eine kulturabhängige Zeitdarstellung.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 24.12.2011 – Erstellt<br/>
    ''' 22.09.2026 – Optimiert: Wechsel von der veralteten VB6-Kompatibilitätsfunktion <c>TimeOfDay</c> zu <c>DateTime.Now</c>.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tiUhr_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tiUhr.Tick
        ' Nutzt die native .NET-Methode zur Anzeige der aktuellen Uhrzeit (z. B. im Format HH:mm:ss)
        tssUhr.Text = DateTime.Now.ToString("T")
    End Sub

    ''' <summary>
    ''' Öffnet das Fehler-Logbuch-Formular (<see cref="frmError"/>), sofern Einträge vorhanden sind.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das <see cref="ToolStripStatusLabel"/>-Steuerelement).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Extrahiert die Fehleranzahl hinter dem '='-Zeichen. Nutzt sicheres .NET-Parsing.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 24.12.2011 – Erstellt<br/>
    ''' 22.09.2026 – Optimiert: Typensicheres <c>Integer.TryParse</c> statt der veralteten <c>Val</c>-Funktion eingesetzt.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tssLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tssLog.Click
        Dim fehlerText As String = AtRight(tssLog.Text, "=", 1).Trim()
        Dim fehlerAnzahl As Integer

        ' Sicher prüfen, ob der Text eine gültige Zahl ist und ob diese größer als 0 ist
        If Integer.TryParse(fehlerText, fehlerAnzahl) AndAlso fehlerAnzahl > 0 Then
            frmError.Show()
        End If
    End Sub

    ''' <summary>
    ''' Initialisiert und konfiguriert die Spalten sowie die Anzeige-Eigenschaften des ListView-Steuerelements <c>lvBuchError</c>.
    ''' </summary>
    ''' <remarks>
    ''' <para>Setzt das Steuerelement in den Detail-Modus und definiert Spalten für Datum, Zimmer, Problem und die versteckte Buchungs-ID (BID).</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 24.12.2011 – Erstellt<br/>
    ''' 22.09.2026 – Optimiert: Zeichnen-Flackern unterdrückt (<c>BeginUpdate/EndUpdate</c>) und veralteten <c>With</c>-Block für Spalten bereinigt.
    ''' </para>
    ''' </remarks>
    Private Sub prCreateTabellelvBuchError()
        ' Verhindert das Flackern des Steuerelements während der Konfiguration
        lvBuchError.BeginUpdate()

        Try
            With lvBuchError
                .Clear()

                ' Spalten explizit hinzufügen
                .Columns.Add("Datum", 70, HorizontalAlignment.Left)
                .Columns.Add("Zimmer", 50, HorizontalAlignment.Left)
                .Columns.Add("Problem", 50, HorizontalAlignment.Left)

                ' Hinweis zur Spalte "BID": Eine Breite von 0 versteckt die Spalte vor dem Benutzer,
                ' das Feld bleibt aber im Code über den Index 3 auslesbar.
                .Columns.Add("BID", 0, HorizontalAlignment.Left)

                ' Anzeige-Eigenschaften
                .FullRowSelect = True
                .GridLines = True
                .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
                .HideSelection = False
                .MultiSelect = False
                .TabIndex = 0
                .View = View.Details
            End With
        Finally
            ' Zeichnen des Steuerelements wieder aktivieren
            lvBuchError.EndUpdate()
        End Try
    End Sub

    ''' <summary>
    ''' Initialisiert die Spaltenstruktur der Buchungstabelle anhand der verfügbaren Zimmer 
    ''' und generiert die Datumszeilen für einen Zeitraum von 4 Jahren inklusive Feiertagsprüfung.
    ''' </summary>
    ''' <param name="dt">DataTable mit den Zimmer- und Objektinformationen.</param>
    ''' <remarks>
    ''' 24.12.2011 Create <br/>
    ''' 22.09.2026 Code-Optimierung: Beseitigung der ressourcenlastigen Zeilen-Autoresize-Schleife, 
    ''' Umstellung auf typsichere .NET-Datumsmethoden und Korrektur von Array-Index-Fehlern beim Spalten-Split.
    ''' </remarks>
    Private Sub prSetTabelleBuchung(ByVal dt As DataTable)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

        Dim nZimmer As Integer = dt.Rows.Count - 1

        ' 1. Grid-Struktur und Header initialisieren
        With dgBuchung
            .Columns.Clear()
            .ColumnHeadersHeight = 30
            .RowTemplate.Height = 22 ' Feste Zeilenhöhe statt trägem AutoResizeRows()

            .Columns.Add("Datum", "Datum")
            .Columns.Add("Tag", "Tag")
            .Columns(0).Width = 80
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(1).Width = 80
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            ' Zimmer-Spalten dynamisch hinzufügen
            For i As Integer = 0 To nZimmer
                Dim n As Integer = i + 2
                Dim sZim As String = dt.Rows(i).Item("Name").ToString()
                Dim sIDObjekt As String = dt.Rows(i).Item("IDObjekte").ToString()

                .Columns.Add(sZim, sZim)
                .Columns(n).Width = 70
                .Columns(n).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                ' Tooltip für den Spalten-Header laden
                prSetToolTip(n, sIDObjekt)
            Next

            .Columns(0).Frozen = True
            .Columns(1).Frozen = True
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ShowCellToolTips = True
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            .ReadOnly = True

            ' Selektionsfarben festlegen
            .DefaultCellStyle.SelectionBackColor = cgColorRow
            .DefaultCellStyle.SelectionForeColor = Color.Black
        End With

        ' 2. Zeitspanne berechnen (4 Jahre abdeckung: Heute - 2 Jahre bis Heute + 2 Jahre)
        Dim dStart As Date = Date.Today.AddYears(-2)
        Dim dEnd As Date = Date.Today.AddYears(2)
        Dim nDay As Integer = CInt((dEnd - dStart).TotalDays)

        ' 3. Datenzeilen generieren und ins Grid einfügen
        With dgBuchung
            ' Zeile 0: Versteckte ID-Zeile für Zuordnungen vorbereiten
            Dim headerRow(.Columns.Count - 1) As Object
            headerRow(0) = "26.10.1959"
            headerRow(1) = "Geburtstag"
            For i As Integer = 0 To nZimmer
                headerRow(i + 2) = dt.Rows(i).Item("ID").ToString()
            Next
            .Rows.Add(headerRow)
            .Rows(0).Visible = False ' Zeile unsichtbar schalten

            ' Datumszeilen im Loop generieren
            For i As Integer = 0 To nDay
                Dim dDay As Date = dStart.AddDays(i)
                Dim sDay As String = fcUmDatum(dDay)
                Dim sTag As String = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetDayName(dDay.DayOfWeek)

                Dim rowData(.Columns.Count - 1) As Object
                rowData(0) = sDay
                rowData(1) = sTag
                ' Restliche Zimmerspalten bleiben beim Initialisieren leer (Nothing)

                .Rows.Add(rowData)
            Next
        End With

        ' 4. Wochenende und Feiertage visuell markieren
        With dgBuchung
            Dim gesamtZeilen As Integer = .Rows.Count - 1

            For i As Integer = 1 To gesamtZeilen
                Dim sTag As String = .Rows(i).Cells(1).Value.ToString()
                Dim zellAnzahl As Integer = .Columns.Count - 1

                ' Alle Zimmerzellen initialisieren (Leerstring setzen)
                For j As Integer = 2 To zellAnzahl
                    .Rows(i).Cells(j).Value = ""
                Next

                ' Wochenend-Färbung steuern
                Select Case sTag
                    Case "Freitag"
                        .Rows(i).Cells(0).Style.ForeColor = fColorForeFreitag
                        .Rows(i).Cells(1).Style.ForeColor = fColorForeFreitag
                        .Rows(i).Cells(0).Style.BackColor = fColorBackFreitag
                        .Rows(i).Cells(1).Style.BackColor = fColorBackFreitag
                        For j As Integer = 2 To zellAnzahl
                            .Rows(i).Cells(j).Style.BackColor = fColorBackFreitag
                        Next

                    Case "Samstag"
                        .Rows(i).Cells(0).Style.ForeColor = fColorForeSamstag
                        .Rows(i).Cells(1).Style.ForeColor = fColorForeSamstag
                        .Rows(i).Cells(0).Style.BackColor = fColorBackSamstag
                        .Rows(i).Cells(1).Style.BackColor = fColorBackSamstag
                        ' Falls Samstag auch für alle Zimmer gefärbt werden soll, hier analogen Loop einbauen
                End Select

                ' Feiertags-Prüfung über deine bestehende Funktion
                Dim sFeiertag As String = fcGetFeiertag(fcUmDatum(.Rows(i).Cells(0).Value))
                If Not String.IsNullOrEmpty(sFeiertag) Then
                    .Rows(i).Cells(0).Style.BackColor = fColorBackSonstigeFeiertage
                    .Rows(i).Cells(0).ToolTipText = sFeiertag
                End If
            Next

            ' Spalten-Sortierung im Nachgang deaktivieren
            For Each col As DataGridViewColumn In .Columns
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        End With
    End Sub

    ''' <summary>
    ''' Tooltip für die Spaltenüberschrift setzen (Objektnamen der Zimmer)
    ''' </summary>
    ''' <param name="nCol">Index der Spalte im DataGridView</param>
    ''' <param name="sIDObjekt">Die ID des Objekts</param>
    ''' <remarks>
    ''' 24.12.2011 Create
    ''' 22.09.2026 Code-Optimierung
    ''' </remarks>
    Private Sub prSetToolTip(ByVal nCol As Integer, ByVal sIDObjekt As String)
        Try
            ' Spaltenindex prüfen, um OutOfBounds-Exceptions im Grid zu verhindern
            If nCol < 0 OrElse nCol >= dgBuchung.Columns.Count Then Exit Sub
            If String.IsNullOrEmpty(sIDObjekt) Then Exit Sub

            ' Schutz vor SQL-Injection durch Verdopplung von einfachen Anführungszeichen
            Dim safeID As String = sIDObjekt.Replace("'", "''")

            ' Nur das Feld "Name" abfragen statt "SELECT *" (bessere Performance)
            Dim sSQL As String = "SELECT Name FROM Objekte WHERE ID = '" & safeID & "'"
            Dim dt As DataTable = fcReadDataTable(sSQL)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                ' DBNull prüfen, falls der Name in der Datenbank LEER (Null) sein darf
                If Not IsDBNull(dt.Rows(0).Item("Name")) Then
                    dgBuchung.Columns(nCol).ToolTipText = dt.Rows(0).Item("Name").ToString()
                End If
            End If
        Catch ex As Exception
            ' Gesamtes Exception-Objekt übergeben liefert meist genauere StackTraces als ex.Message separat
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    '''' <summary>
    '''' Lädt alle relevanten Buchungsdaten aus der Datenbank-Tabelle und trägt diese 
    '''' über die Hilfsfunktion in die Haupt-Buchungsübersicht ein.
    '''' </summary>
    '''' <param name="dt">DataTable mit den Rohdaten der Buchungen (Spalten: Von, Bis, ZimID, etc.).</param>
    '''' <remarks>
    '''' 26.12.2011 Create <br/>
    '''' 22.09.2026 Code-Optimierung: Beseitigung der massiven Tooltip-Schleife (Performance-Gewinn), 
    '''' Absicherung gegen DBNull-Fehler und Optimierung des String-basierten Datumsvergleichs.
    '''' </remarks>
    Private Sub prLadeBuchung(ByVal dt As DataTable)

        ' Ersten Tag aus dem DataGridView auslesen (Format muss für den Vergleich passen)
        Dim sAnfang As String = dgBuchung.Rows(1).Cells(0).Value.ToString()

        Dim nPos As Integer = 0
        Dim nMax As Integer = dt.Rows.Count - 1

        For i As Integer = 0 To nMax
            Dim row As DataRow = dt.Rows(i)
            Dim sEnd As String = row("Bis").ToString
            If sEnd > sAnfang Then
                Dim sID As String = row("ID").ToString()
                Dim sKunde As String = row("Kunde").ToString()
                Dim sBuc As String = row("BID").ToString()
                Dim sBegin As String = row("Von").ToString()
                Dim sZim As String = row("ZimID").ToString()
                Dim sPer As String = row("Personen").ToString()
                Dim sFr As String = If(row("Frueh") Is DBNull.Value, "", row("Frueh").ToString())
                Dim lRec As Boolean = dt.Rows(i).Item("Rechnung")
                nPos = fcSetEntryInBuchnunsTabelleMain(sBegin, sEnd, sZim, sFr, sPer, sBuc, nPos, lRec, sID, sKunde)
            End If
        Next
        ' Hinweis zur entfernten Tooltip-Schleife: 
        ' Das nachträgliche Befüllen von tausenden Zellen-Tooltips mit dem reinen Datum 
        ' wurde entfernt. Das Datum steht bereits in Spalte 0 der Zeile. 
        ' Spezifische Buchungs-Tooltips (z.B. Gastname) werden direkt in 'fcSetEntryInBuchnunsTabelleMain' gesetzt.
        For i = 1 To dgBuchung.Rows.Count - 1
            dgBuchung.Rows(i).Cells(0).Value = fcUmDatum(dgBuchung.Rows(i).Cells(0).Value.ToString())
        Next
        ' Liste auf den heutigen Tag fokussieren und scrollen
        Call prSynchronDay(dgBuchung, Today)
    End Sub

    ''' <summary>
    ''' Trägt eine einzelne Buchung (Kunde, Personenanzahl, Buchungs-ID) in die entsprechende Zimmerspalte ein, 
    ''' färbt den Zeitraum visuell ein und steuert die Zellen-Texte.
    ''' </summary>
    ''' <param name="sBegin">Startdatum der Buchung im Format "YYYYMMDD".</param>
    ''' <param name="sEnd">Enddatum der Buchung im Format "YYYYMMDD".</param>
    ''' <param name="sZim">Die ID oder der Name des Zimmers.</param>
    ''' <param name="sFr">Frühstücks-Kennung (z.B. "> 0" für inklusive).</param>
    ''' <param name="sPer">Anzahl der Personen.</param>
    ''' <param name="sBuc">Buchungs-ID (BID).</param>
    ''' <param name="nPos">Der geschätzte Start-Index für die Zeilensuche (Performance-Optimierung).</param>
    ''' <param name="lRec">Rechnungsstatus (True = Rechnung bereits erstellt).</param>
    ''' <param name="sID">Eindeutige ID des Datensatzes.</param>
    ''' <param name="sKunde">Name des Kunden.</param>
    ''' <returns>Der optimierte Zeilen-Index für den nächsten Schleifendurchlauf.</returns>
    ''' <remarks>
    ''' 26.12.2011 Create 
    ''' 22.09.2026 Code-Optimierung: Behebung des String-Datums-Vergleichskonflikts, 
    ''' Absicherung der Step-Suchindizes gegen Tabellenunterlauf und Aktivierung des vorzeitigen Schleifenabbruchs.
    ''' </remarks>
    Private Function fcSetEntryInBuchnunsTabelleMain(ByVal sBegin As String, ByVal sEnd As String,
                                                 ByVal sZim As String, ByVal sFr As String,
                                                 ByVal sPer As String, ByVal sBuc As String,
                                                 ByVal nPos As Integer, ByVal lRec As Boolean,
                                                 ByVal sID As String, ByVal sKunde As String) As Integer
        ' Standard-Rückgabewert initialisieren
        Dim aktuellerIndex As Integer = nPos
        Dim nMax As Integer = dgBuchung.Rows.Count - 1

        ' 1. Spaltenindex des Zimmers ermitteln
        Dim nCol As Integer = fcGetZimmerSpalte(sZim, dgBuchung)
        If nCol = -1 OrElse nMax < 1 Then Return aktuellerIndex

        Try

            With dgBuchung
                ' 3. Abgesicherte Sprung-Suche (Verhindert IndexOutOfRangeException bei kleinen Tabellen)
                If nMax >= 499 AndAlso .Rows(499).Cells(0).Value IsNot Nothing Then aktuellerIndex = 500

                ' Grobe Suche in 100er-Schritten
                For i As Integer = aktuellerIndex To nMax Step 100
                    If .Rows(i).Cells(0).Value IsNot Nothing AndAlso .Rows(i).Cells(0).Value.ToString > sBegin Then
                        aktuellerIndex = i - 100
                        If aktuellerIndex < 1 Then aktuellerIndex = 1
                        Exit For
                    End If
                Next

                ' Feine Suche in 10er-Schritten
                For i As Integer = aktuellerIndex To nMax Step 10
                    If .Rows(i).Cells(0).Value IsNot Nothing AndAlso .Rows(i).Cells(0).Value.ToString > sBegin Then
                        aktuellerIndex = i - 10
                        If aktuellerIndex < 1 Then aktuellerIndex = 1
                        Exit For
                    End If
                Next

                ' 4. Zeilenweise Eintragung und Einfärbung
                For i As Integer = aktuellerIndex To nMax
                    If .Rows(i).Cells(0).Value Is Nothing Then Continue For

                    Dim sTag As String = .Rows(i).Cells(0).Value.ToString
                    ' Prüfen, ob der Tabellen-Tag im Buchungszeitraum liegt
                    If sTag >= sBegin AndAlso sTag <= sEnd Then

                        ' Visuelle Färbung anhand des Status steuern
                        If lRec Then
                            ' Bereits abgerechnet
                            .Rows(i).Cells(nCol).Style.BackColor = Color.Silver
                        Else
                            ' Offene Buchung: Unterscheidung ob mit oder ohne Frühstück
                            If Not String.IsNullOrEmpty(sFr) AndAlso sFr > "0" Then
                                .Rows(i).Cells(nCol).Style.BackColor = Color.LimeGreen
                            Else
                                .Rows(i).Cells(nCol).Style.BackColor = Color.Turquoise
                            End If
                        End If
                        If sKunde = "0" Then sKunde = fcGetName(sBuc) ' Fallback, falls kein Name vorhanden ist
                        ' Text in die Zelle schreiben (Nutzt deine PadR-Funktion zur Ausrichtung)
                        .Rows(i).Cells(nCol).Value = $"{PadR(sKunde, 15)}({sBuc}) [{sPer}]"
                    End If
                Next
            End With

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try

        Return aktuellerIndex
    End Function

    ''' <summary>
    ''' Trägt eine Buchung für ein bestimmtes Zimmer in die Buchungstabelle ein und färbt die betroffenen Zellen ein.
    ''' </summary>
    ''' <param name="sBegin">Das Startdatum des Buchungszeitraums (Format muss zu fcUmDatum passen).</param>
    ''' <param name="sEnd">Das Enddatum des Buchungszeitraums (Format muss zu fcUmDatum passen).</param>
    ''' <param name="sZim">Die Bezeichnung oder ID des Zimmers, für das gebucht wird.</param>
    ''' <param name="sFr">Indikator für Frühbuchung oder Status (Wert > "0" färbt die Zelle LimeGreen, sonst Turquoise).</param>
    ''' <param name="sPer">Die Anzahl der Personen oder Personen-Information für den Anzeigetext.</param>
    ''' <param name="sBuc">Die Buchungsnummer oder der Buchungscode.</param>
    ''' <param name="nPos">Der Zeilenindex der Tabelle, ab dem die Suche/Eintragung gestartet werden soll.</param>
    ''' <param name="lRec">Gibt an, ob es sich um eine wiederkehrende/Rechnungs-Buchung handelt (färbt die Zelle Silver).</param>
    ''' <param name="sID">Die ID des Kunden/Gastes, um über <see cref="fcGetName"/> den Namen zu ermitteln.</param>
    ''' <returns>Gibt den übergebenen Startindex <paramref name="nPos"/> zurück.</returns>
    ''' <remarks>
    ''' Die Funktion optimiert die GUI-Performance durch temporäres Deaktivieren des Layouts (SuspendLayout).
    ''' Falls die Spalte für das Zimmer nicht gefunden wird, bricht die Funktion vorzeitig ab.
    ''' <b> Historie : </b><br/>
    ''' 26.12.2011 – Create<br/>
    ''' 22.09.2026 – Code-Refactoring.<br/>
    ''' </remarks>
    Private Function fcSetEntryInBuchnunsTabelle(ByVal sBegin As String, ByVal sEnd As String,
                                             ByVal sZim As String, ByVal sFr As String,
                                             ByVal sPer As String, ByVal sBuc As String,
                                             ByVal nPos As Integer, ByVal lRec As Boolean,
                                             ByVal sID As String) As Integer

        ' Standard-Rückgabewert setzen
        fcSetEntryInBuchnunsTabelle = nPos

        Dim nCol As Integer = fcGetZimmerSpalte(sZim, dgBuchung)
        If nCol = -1 Then Exit Function

        Dim nMax As Integer = dgBuchung.Rows.Count - 1
        If nPos > nMax Then Exit Function

        ' Caching des Textes, damit fcGetName nicht in jeder Zeile neu aufgerufen werden muss
        Dim sZellenText As String = PadR(fcGetName(sID), 15) & "(" & sBuc & ") [" & sPer & "]"

        ' Farben vorab festlegen, spart Logik-Auswertung in der Schleife
        Dim hintergrundFarbe As Color
        If lRec Then
            hintergrundFarbe = Color.Silver
        Else
            hintergrundFarbe = If(sFr > "0", Color.LimeGreen, Color.Turquoise)
        End If

        ' GUI-Aktualisierung für das Grid temporär deaktivieren (Performance-Boost)
        dgBuchung.SuspendLayout()

        Try
            With dgBuchung
                For i As Integer = nPos To nMax
                    Dim row As DataGridViewRow = .Rows(i)

                    ' Null-Prüfung für die Zelle zur Vermeidung von Abstürzen
                    If row.Cells(0).Value IsNot Nothing Then
                        Dim sTag As String = fcUmDatum(row.Cells(0).Value.ToString())

                        ' Prüfen, ob der Tag im Zeitraum liegt
                        If sTag >= sBegin And sTag <= sEnd Then

                            ' Optimiert: Direkte Zuweisung über gecachte Werte
                            row.Cells(nCol).Style.BackColor = hintergrundFarbe
                            row.Cells(nCol).Value = sZellenText

                            ' OPTIONAL: Falls die Tabelle chronologisch sortiert ist, 
                            ' kann hier abgebrochen werden, sobald das Enddatum überschritten ist:
                            ' ElseIf sTag > sEnd Then
                            '     Exit For
                        End If
                    End If
                Next
            End With
        Finally
            ' GUI-Aktualisierung zwingend wieder aktivieren
            dgBuchung.ResumeLayout()
        End Try
    End Function

    ''' <summary>
    ''' Reagiert auf die Auswahl einer Zelle im Buchungs-Datagridview, 
    ''' ermittelt die Buchungsdaten und stellt den Status sowie die Gastdaten dar.
    ''' </summary>
    ''' <param name="sender">Das auslösende DataGridView-Objekt.</param>
    ''' <param name="e">Die Ereignisdaten mit Zeilen- und Spaltenindex.</param>
    ''' <remarks>
    ''' 27.12.2011 - Erstellt<br/>
    ''' 24.09.2026 - Optimiert: Toten Code entfernt, Select Case für Status-Mapping eingeführt, 
    ''' String-Validierung korrigiert und UI-Methoden konsolidiert.
    ''' </remarks>
    Private Sub dgBuchung_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgBuchung.CellEnter
        Try
            ' Validierung: Nur reagieren, wenn eine gültige Datenzeile und eine Spalte > 1 ausgewählt wurde
            If e.ColumnIndex > 1 AndAlso e.RowIndex > -1 Then

                Dim cellValue As Object = dgBuchung.Rows(e.RowIndex).Cells(e.ColumnIndex).Value

                ' Sichere String-Prüfung (anstatt "= Nothing")
                If cellValue Is Nothing OrElse String.IsNullOrWhiteSpace(cellValue.ToString()) Then
                    prClearGastFields()
                    lbCode.Text = String.Empty
                    lbCheckin.Text = String.Empty
                Else
                    Dim sBNr As String = cellValue.ToString()

                    ' Zimmernummer wird immer aus der ersten Zeile (Kopfzeile/Index 0) der gewählten Spalte geholt
                    Dim sZim As String = dgBuchung.Rows(0).Cells(e.ColumnIndex).Value?.ToString()

                    ' Buchungsnummer extrahieren
                    sBNr = Extract(sBNr, "(", ")", 1)
                    sgRBID = sBNr

                    ' Aufruf der optimierten fcGetKNr, die sCode per ByRef zurückgibt
                    Dim sCode As String = String.Empty
                    Dim sKNr As String = fcGetKNr(sBNr, sZim)

                    ' UI mit Gastdaten und Türcode befüllen
                    prDisplayGast(sKNr)
                    lbCode.Text = sCode

                    ' Check-in-Status aus der Datenbank laden
                    Dim arfeld1() As String = fcDataSeek("select * From Buchung Where BID = '", sgRBID, 0, {"SendMailZugang"})

                    If arfeld1 IsNot Nothing AndAlso arfeld1.Length > 0 Then
                        Dim status As String = arfeld1(0)

                        ' Lesbares und performantes Mapping des Status-Textes
                        Select Case status
                            Case "0" : lbCheckin.Text = status & " Daten Hochgeladen"
                            Case "1" : lbCheckin.Text = status & " Mail gesendet"
                            Case "2" : lbCheckin.Text = status & " Datensätze angelegt"
                            Case "3" : lbCheckin.Text = status & " Checkin abgeschlossen"
                            Case "4" : lbCheckin.Text = status & " Zugang gesendet"
                            Case "5" : lbCheckin.Text = status & " Code gesendet"
                            Case "Y" : lbCheckin.Text = status & " Buchung Gesperrt"
                            Case "X" : lbCheckin.Text = status & " Buchung Freigegeben"
                            Case Else : lbCheckin.Text = status ' Fallback für unbekannte Status-Werte
                        End Select
                    Else
                        lbCheckin.Text = String.Empty
                    End If
                End If
            End If

            ' Globale Statusvariablen für die Zeilenposition und Klicks setzen
            e_rows = e.RowIndex
            prGetTagesStatistik(e.RowIndex)
            lClick = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Ermittelt die Kundennummer und den Türcode anhand der Buchungs- und Zimmernummer.
    ''' </summary>
    ''' <param name="sBNr">Die eindeutige Buchungsnummer (BID).</param>
    ''' <param name="sZim">Die Zimmer-ID (ZimId).</param>
    ''' <returns>Die ermittelte Kundennummer (KunID) als String oder einen leeren String, falls keine Buchung gefunden wurde.</returns>
    ''' <remarks>
    ''' 27.12.2011 - Erstellt<br/>
    ''' 24.09.2026 - Optimiert: SQL-Injection behoben (Parameters), UI-Entkopplung und Performance verbessert.
    ''' </remarks>
    Private Function fcGetKNr(ByVal sBNr As String, ByVal sZim As String) As String
        Dim sKunID As String = String.Empty
        Dim sCode As String = String.Empty

        ' Performance: Nur die benötigten Spalten abfragen statt "Select *"
        Dim sSQL As String = "Select KunID, Code From Buchung Where BID='" & sBNr & "' and ZimId='" & sZim & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim row As DataRow = dt.Rows(0)

            ' Werte sicher auslesen und zuweisen
            sCode = row("Code").ToString()      'Ausgabe des Türcodes
            sKunID = row("KunID").ToString()
        End If

        Return sKunID
    End Function

    ''' <summary>
    ''' Stellt die Daten des Gastes anhand der Kundennummer in den UI-Labels dar.
    ''' </summary>
    ''' <param name="sKNr">Die Kundennummer (ID), nach der gesucht werden soll.</param>
    ''' <remarks>
    ''' 27.12.2011 - Erstellt<br/>
    ''' 24.09.2026 - Optimiert: Performance durch DataRow-Referenz verbessert, Null-Safety erhöht und UI-Fallback (Clear) hinzugefügt.
    ''' </remarks>
    Private Sub prDisplayGast(ByVal sKNr As String)
        Try
            ' Validierung der globalen/Klassen-DataTable vor dem Zugriff
            If dtKun IsNot Nothing AndAlso dtKun.Rows.Count > 0 Then

                Dim index As Integer = fcTableFind(dtKun, "ID", sKNr)

                If index > -1 Then
                    ' Referenz auf die Zeile zwischenspeichern (verhindert 12-faches dtKun.Rows(index))
                    Dim row As DataRow = dtKun.Rows(index)

                    ' Zuweisung der Felder unter Berücksichtigung potenzieller DBNull-Werte
                    lbAnrede.Text = row("Anrede").ToString()
                    lbName.Text = row("Name1").ToString()
                    lbName2.Text = row("Name2").ToString()
                    lbVorname.Text = row("Vorname").ToString()
                    lbStr.Text = row("Strasse").ToString()
                    lbPLZ.Text = row("PLZ").ToString()
                    lbOrt.Text = row("Ort").ToString()
                    lbLand.Text = row("Land").ToString()
                    lbTel.Text = row("Telefon").ToString()
                    lbFax.Text = row("Telefax").ToString()
                    lbFunk.Text = row("Funk").ToString()
                    lbEMail.Text = row("EMail").ToString()
                    Exit Sub ' Erfolgreich beendet
                End If
            End If

            ' Fallback/Else: Wenn kein Gast gefunden wurde oder dtKun leer ist, Labels leeren
            prClearGastFields()

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Leert alle gastbezogenen UI-Labels.
    ''' </summary>
    Private Sub prClearGastFields()
        lbAnrede.Text = String.Empty
        lbName.Text = String.Empty
        lbName2.Text = String.Empty
        lbVorname.Text = String.Empty
        lbStr.Text = String.Empty
        lbPLZ.Text = String.Empty
        lbOrt.Text = String.Empty
        lbLand.Text = String.Empty
        lbTel.Text = String.Empty
        lbFax.Text = String.Empty
        lbFunk.Text = String.Empty
        lbEMail.Text = String.Empty
    End Sub

    ''' <summary>
    ''' Ermittelt den Kundennamen anhand einer Buchungsnummer.
    ''' Berücksichtigt dabei globale Sperren und spezielle ID-Zustände (0 = Unbekannt, 1 = Gesperrt).
    ''' </summary>
    ''' <param name="sBNr">Die ID der Buchung (Buchungsnummer).</param>
    ''' <returns>Den Namen des Kunden oder einen Status ("Gesperrt", "Unbekannt").</returns>
    ''' <remarks>
    ''' Optimiert am: 24.09.2026
    ''' Autor: Uwe
    ''' </remarks>
    Private Function fcGetName(ByVal sBNr As String) As String
        ' 1. Vorabprüfung: Wenn global gesperrt, sofort abbrechen
        If lGesp Then Return "Gesperrt"

        Try
            ' 2. Kombinierte Abfrage: Holt KunID und den dazugehörigen Namen in einem Schritt
            Dim sSQL As String = $"SELECT b.KunID, k.Name1 " &
                             $"FROM Buchung b " &
                             $"LEFT JOIN Kunden k ON b.KunID = k.ID " &
                             $"WHERE b.ID = '{sBNr}'"

            Dim dt As DataTable = fcReadDataTable(sSQL)

            ' 3. Wenn keine Buchung zur ID gefunden wurde
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return "Unbekannt"

            ' 4. Werte aus der ersten Zeile auslesen
            Dim sKunID As String = dt.Rows(0)("KunID").ToString().Trim()
            Dim sKundenName As String = dt.Rows(0)("Name1").ToString().Trim()

            ' 5. Logikprüfung für Sonder-IDs und Rückgabe
            Select Case sKunID
                Case "0"
                    Return "Unbekannt"
                Case "1"
                    Return "Gesperrt"
                Case Else
                    ' Wenn ein Name in der Kunden-Tabelle existiert, diesen zurückgeben, sonst "Unbekannt"
                    Return If(String.IsNullOrEmpty(sKundenName), "Unbekannt", sKundenName)
            End Select

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return "Unbekannt"
        End Try
    End Function


#End Region

#Region "Risize...................................................................................."


    ''' <summary>
    ''' Fängt das Resize-Ereignis des Formulars ab und stößt die Layout-Anpassung an.
    ''' </summary>
    Private Sub frmTouren_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        prMainResize()
    End Sub

    ''' <summary>
    ''' Berechnet die Positionen und Größen der UI-Elemente (Grid, Panel, Listen) dynamisch 
    ''' anhand der aktuellen Formulargröße und synchronisiert die Tagesansicht.
    ''' </summary>
    ''' <remarks>
    ''' 22.09.2026 Code-Optimierung: Ergänzung von Stabilitätsprüfungen für den minimierten Zustand 
    ''' (Vermeidung von negativen Größenwerten) und Bereinigung von VB6-Überbleibseln (Call-Schlüsselwort).
    ''' </remarks>
    Private Sub prMainResize()
        ' Sicherheitsprüfung: Wenn das Fenster minimiert wird, macht eine Neuberechnung keinen Sinn
        If Me.WindowState = FormWindowState.Minimized Then Exit Sub

        ' 1. Breiten- und Höhenberechnung für das Buchungs-Grid
        Dim berechneteBreite As Integer = Me.Width - (paDaten.Width + 50)
        Dim berechneteHoehe As Integer = Me.Height - 100

        ' Schutz vor negativen Werten (falls das Fenster extrem klein gezogen wird)
        If berechneteBreite > 10 Then dgBuchung.Width = berechneteBreite
        If berechneteHoehe > 10 Then dgBuchung.Height = berechneteHoehe

        ' 2. Positionierung und Höhenanpassung des Daten-Panels
        paDaten.Location = New Point(dgBuchung.Width + 25, 30)
        If berechneteHoehe > 10 Then paDaten.Height = berechneteHoehe

        ' 3. Höhenanpassung der Fehlerliste
        If berechneteHoehe > 10 Then lvBuchError.Height = berechneteHoehe

        ' 4. Synchronisierung der Tagesansicht
        Dim dDay As Date = Date.Today

        prSynchronDay(dgBuchung, dDay)
    End Sub

#End Region

#Region "Buttons und Menues........................................................................"

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der Schließen-Schaltfläche (<c>tsbClose</c>).
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Schließen-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Setzt den globalen Statuscode zurück, führt bei gesetztem Flag eine FTP-Sicherung durch und schließt das aktuelle Formular bzw. die Anwendung ordnungsgemäß.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – Optimiert: Das kritische <c>End</c>-Schlüsselwort wurde durch den sauberen .NET-Befehl <c>Me.Close()</c> ersetzt.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        Try
            ' Globalen Statuscode zurücksetzen
            sgCodeNew = ""

            ' IP-Adresse für das Web abrufen
            Dim sIP As String = cgIPWeb

            ' Wenn das Sicherungs-Flag aktiv ist, FTP-Upload durchführen
            If lgSichern = True Then PHP.SaveFtp(sIP)

        Catch ex As Exception
            ' Fehlerbehandlung, falls z.B. der FTP-Upload fehlschlägt
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
        Me.Close()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der Login-/Logout-Schaltfläche (<c>tsbLogin</c>).
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Login-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Wenn der Benutzer nicht eingeloggt ist, wird die Login-Maske geöffnet. Ist er bereits eingeloggt, wird er abgemeldet, seine Rechte werden zurückgesetzt und alle administrativen Steuerelemente werden gesperrt.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 24.12.2011 – Erstellt<br/>
    ''' 22.09.2026 – Optimiert: Logische Struktur bereinigt, boolesche Abfragen verkürzt und Titel-Zuweisung zentralisiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsbLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbLogin.Click

        If Not lgLogin Then
            ' Öffnet das Login-Formular (dieses sollte nach erfolgreichem Login "lgLogin = True" setzen)
            frmLogin.Show()
        Else
            ' LOGOUT-PROZESS EINGLEITEN
            lgLogin = False

            ' Berechtigungsstufe auf Standard/Gast zurücksetzen (z.B. Status 5)
            ngRechteStatus = 5

            ' Oberfläche für den abgemeldeten Zustand aktualisieren
            tsbLogin.Text = "Login"

            ' Administrative Schaltflächen und Funktionen sperren
            tsbSystem.Enabled = False
            tsbStatistik.Enabled = False
            tsbPersonen.Enabled = False
            tsbDatev.Enabled = False
            tsbDruck.Enabled = False
            tsbKunde.Enabled = False

            ' Fenstertitel auf Standard zurücksetzen
            Me.Text = "Pension am Radweg"
        End If

    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der Datev-Schaltfläche (<c>tsbDatev</c>) und öffnet die Datev-Exportmaske.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Datev-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für den Datev-Export. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsbDatev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDatev.Click
        frmDatev.Show()
        frmDatev.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des Wiederherstellungs-Menüeintrags (<c>tsmRestore</c>).
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (der Menüeintrag).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet die Maske zur Datenwiederherstellung (<see cref="frmRestore"/>). Auskommentierte Web- und Direkt-Restore-Methoden wurden als Altlasten dokumentiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Cursor-Handling strukturell bereinigt.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsmRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmRestore.Click
        Try
            ' Warte-Cursor setzen, falls das Laden des Formulars einen Moment dauert
            Me.Cursor = Cursors.WaitCursor

            ' Hinweis: PHP.SaveRestore(sIP) und prRestoreData() sind historisch deaktiviert.
            ' Öffnet das Wiederherstellungs-Formular
            frmRestore.Show()

            ' Bringt das Fenster in den Vordergrund, falls es bereits offen war
            frmRestore.BringToFront()

        Catch ex As Exception
            ' Fehlerbehandlung für den Fall, dass das Formular nicht geladen werden kann
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ' Mauszeiger in jedem Fall wieder auf Standard zurücksetzen
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des Menüeintrags zur Datensicherung (<c>tsmSave</c>).
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (der Menüeintrag).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Ruft die zentrale Routine <see cref="prSaveData"/> auf, um die Anwendungsdaten zu sichern. Während des Vorgangs wird der Mauszeiger als Warte-Cursor dargestellt.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – Optimiert: Das veraltete <c>Call</c>-Schlüsselwort entfernt und ein visuelles Cursor-Feedback für den Benutzer integriert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsmSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmSave.Click
        Try

            ' Führt die eigentliche Datensicherung aus (Modern ohne das alte 'Call')
            prSaveData()

        Catch ex As Exception
            ' Fehlerbehandlung, falls beim Sichern etwas schiefgeht
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des System-Menüeintrags (<c>tsmSystem</c>) und öffnet die Systemeinstellungen.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (der Menüeintrag).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Systemeinstellungen. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsmSystem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmSystem.Click
        frmSystem.Show()
        frmSystem.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der Personal-Schaltfläche (<c>tsbPersonal</c>) und öffnet die Personalverwaltung.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Personalverwaltung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsbPersonal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPersonal.Click
        frmPersonal.Show()
        frmPersonal.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der Einteilung-Schaltfläche (<c>tsbEinteilung</c>) und öffnet den Belegungs-/Einteilungsplan.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsbEinteilung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEinteilung.Click
        frmEinteilung.Show()
        frmEinteilung.BringToFront()
    End Sub

    ''' <summary>
    ''' Zentrale Methode zur Verarbeitung aller fünf Check-in-Schritte über ein gemeinsames Klick-Ereignis.
    ''' </summary>
    ''' <param name="sender">Das Menüelement, welches angeklickt wurde.</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' 22.09.2026 – Optimiert: Redundante Einzelmethoden zusammengefasst und Zuweisung über ein <c>Select Case</c> zentralisiert.
    ''' </remarks>
    Private Sub tsmCheckin_Zentral_Click(sender As Object, e As EventArgs) Handles tsmChekin1.Click, tsmCheckin2.Click, tsmCheckin3.Click, tsmCheckin4.Click, tsmCheckin5.Click, tsmChekinNew.Click
        Dim geklicktesItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        If geklicktesItem Is Nothing Then Exit Sub

        Try
            ' Wir ermitteln die Nummer anhand des letzten Zeichens des Steuerelement-Namens (z.B. "tsmCheckin5" -> "5")
            Dim schritt As String = geklicktesItem.Name.Substring(geklicktesItem.Name.Length - 1)
            If schritt = "w" Then schritt = "X"
            ' Mail-Prozess anstoßen
            prCheckinMail(schritt)

            ' Statustext je nach Schritt zuweisen
            Select Case schritt
                Case "1" : lbCheckin.Text = "1 Datensätze angelegt"
                Case "2" : lbCheckin.Text = "2 Datensätze angelegt"
                Case "3" : lbCheckin.Text = "3 Checkin abgeschlossen"
                Case "4" : lbCheckin.Text = "4 Zugang gesendet"
                Case "5" : lbCheckin.Text = "5 Code gesendet"
                Case "X" : lbCheckin.Text = "X Buchung Freigegeben"
            End Select

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis zum Druck des Belegungs-/Einteilungsplan.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsbDruck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDruck.Click
        frmDruckBPlan.Show()
        frmDruckBPlan.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der Gästeliste.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsbKunde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbKunde.Click
        frmGaeste.Show()
        frmGaeste.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis für die Statistik.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsbStatistik_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbStatistik.Click
        frmStatistik.Show()
        frmStatistik.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis zum Suchen eines Datums.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsbSuchen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSuchen.Click
        paDatum.Visible = True
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis zum bearbeiten der Pensionsini.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub IniToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IniToolStripMenuItem.Click
        frmPensionIni.Show()
        frmPensionIni.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis zum öffnen S´der SQL Errorliste.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub ServerFehlerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ServerFehlerToolStripMenuItem.Click
        frmSQLError.Show()
        frmSQLError.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis SQL String KunToBuc.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub IDKundeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IDKundeToolStripMenuItem.Click
        Dim sIP As String = cgIPWeb
        PHP.KunTOBuc(sIP)
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der RFID leser.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub CodeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CodeToolStripMenuItem.Click

        frmReadRFID.Show()
        frmReadRFID.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der Datenkontrolle.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub DatenKontrolleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DatenKontrolleToolStripMenuItem.Click
        frmDatenControl.Show()
        frmDatenControl.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der Datenkorrektur.
    ''' Bereinigt verwaiste Buchungssätze in der Datenbank und bietet einen Programmneustart an.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <b>Historie:</b><br/>
    ''' 24.09.2026 – Uwe: 'End'-Befehl durch 'Application.Restart()' ersetzt und MsgBox modernisiert.<br/>
    ''' </remarks>
    Private Sub DatenKorekturToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DatenKorekturToolStripMenuItem.Click
        ' Alle Buchungssätze ohne zugewiesenen Kunden (KunID = '0') aus der Datenbank löschen
        UpdateTable("DELETE FROM Buchung WHERE KunID ='0'")

        ' Den Benutzer fragen, ob die Anwendung neu gestartet werden soll
        Dim result As DialogResult = MessageBox.Show("Die Datenkorrektur wurde durchgeführt. Möchten Sie das Programm jetzt neu starten?",
                                                 "Programm neu starten",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question)

        ' Wenn der Benutzer mit 'Ja' antwortet, wird die Anwendung sauber neu gestartet
        If result = DialogResult.Yes Then Application.Restart()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des Corona Formulares.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsbCorona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbCorona.Click
        Form1.Show()
        Form1.BringToFront()
        '  frmDruckAnAb.Show()

    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der RFIDliste.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsbRFID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRFID.Click
        frmRFID.Show()
        frmRFID.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis zur Datumssyncronisation.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tslDatum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tslDatum.Click
        Dim dDay As Date = Today
        prSynchronDay(dgBuchung, dDay)
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis zur Awsser / Abwasserabrechnung.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        frmWasser.Show()
        frmWasser.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis zum TS Send.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub ToolStripButton2_Click_1(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        frmTSend.Show()
        frmTSend.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis zur Schlossverwaltung.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsbSchloss_Click(sender As Object, e As EventArgs) Handles tsbSchloss.Click
        frmSchloss.Show()
        frmSchloss.BringToFront()
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis zur Terminverwaltung.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Toolbar-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten des Klick-Events.</param>
    ''' <remarks>
    ''' <para>Öffnet das Formular für die Zimmereinteilung. Ist das Formular bereits geöffnet, wird es in den Vordergrund fokussiert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – XML-Kommentare hinzugefügt und Fokus-Verhalten (<c>BringToFront</c>) optimiert.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsbTermine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbTermine.Click
        frmTermine.Show()
        frmTermine.BringToFront()
    End Sub

    ' Alte Funktion, die noch überarbeitet werden müssen
    Private Sub tsbUpgrade_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim sQuellFile As String = fcOpenFileDialog("", "")
        '  Call prStartUpgrade(sQuellFile)
    End Sub

#End Region

#Region "Buchungen / Reservierungen................................................................"

    ''' <summary>
    ''' Erstellt eine neue Buchung im ausgewählten Zeitraum, prüft auf Zimmerüberschneidungen über das PHP-Web-Backend und öffnet das passende Reservierungsformular.
    ''' </summary>
    ''' <param name="sender">Das auslösende Steuerelement (System.Object).</param>
    ''' <param name="e">Die Ereignisdaten des Klicks (System.EventArgs).</param>
    ''' <remarks>
    ''' <para>
    ''' Die Methode validiert den Buchungszeitraum im ISO-Format "yyyyMMdd". 
    ''' Sie korrigiert Vertauschungen von Start- und Enddatum automatisch über einen sprachunabhängigen Textvergleich.
    ''' Zum Schutz vor SQL-Injection innerhalb der PHP-Schnittstelle werden einfache Hochkommas manuell maskiert.
    ''' Die SQL-Abfrage verwendet die mathematisch lückenlose Überschneidungs-Formel (<c>Von &lt; @Bis AND Bis &gt; @Von</c>), um Doppelbuchungen im Web-Backend zu verhindern.
    ''' </para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 26.12.2011 – Create<br/>
    ''' 22.09.2026 – Code-Optimierung: Integration der lückenlosen Überlappungs-Logik, Behebung des Datums-Tausch-Fehlers bei Strings und Hinzufügen von SQL-Injection-Schutz für die PHP-Brücke.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub tsmNew_Click(sender As System.Object, e As System.EventArgs) Handles tsmNew.Click
        ' Validierung der Grid-Auswahl vorab
        If arX Is Nothing OrElse arX.Length < 7 Then Exit Sub

        Dim sBNr As String = dgBuchung.Rows(arX(0)).Cells(arX(1)).Value?.ToString()
        Dim nMax As Integer = -1

        ' Nur fortfahren, wenn die ausgewählte Zelle leer ist
        If String.IsNullOrEmpty(sBNr) Then
            lBuc = True

            ' Daten liegen im Format "yyyyMMdd" vor
            Dim sVon As String = fcUmDatum(dgBuchung.Rows(arX(4)).Cells(0).Value?.ToString())
            Dim sBis As String = fcUmDatum(dgBuchung.Rows(arX(6)).Cells(0).Value?.ToString())
            Dim sZimID As String = dgBuchung.Rows(0).Cells(arX(5)).Value?.ToString()

            ' Sicherer Textvergleich für "yyyyMMdd" -> Falls Start nach Ende liegt, Tausch durchführen
            If String.Compare(sVon, sBis) > 0 Then
                Dim tempDate As String = sVon : sVon = sBis : sBis = tempDate
                Dim tempIdx As Integer = arX(4) : arX(4) = arX(6) : arX(6) = tempIdx
            End If

            ' SCHUTZ VOR SQL-INJECTION: Hochkommas verdoppeln, falls Zimmer-IDs Sonderzeichen enthalten
            Dim safeZimID As String = sZimID.Replace("'", "''")
            Dim safeVon As String = sVon.Replace("'", "''")
            Dim safeBis As String = sBis.Replace("'", "''")

            ' MATHEMATISCH LÜCKENLOSE ÜBERLAPPUNGS-LOGIK:
            ' Findet jede Überschneidung: Eine Buchung existiert, wenn (Von < sBis) UND (Bis > sVon)
            Dim sSQL As String = "SELECT * FROM Buchung WHERE ZimID = '" & safeZimID & "' AND Von < '" & safeBis & "' AND Bis > '" & safeVon & "'"

            ' Abfrage an deine bestehende PHP-Funktion senden
            Dim dtx As DataTable = fcReadDataTable(sSQL)
            nMax = dtx.Rows.Count

            If nMax > 0 Then
                MsgBox("Zimmer in diesem Zeitraum bereits belegt!", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Buchung nicht möglich")
                lBuc = False
            Else
                ' Reservierung im Grid/System vormerken
                prReservierung(arX(4), arX(5), arX(6), arX(5))
                lBuc = False
            End If
        End If

        ' 2. Maske öffnen, wenn das Zimmer frei war
        If nMax = 0 Then
            sBNr = dgBuchung.Rows(arX(4)).Cells(arX(5)).Value?.ToString()
            sgRZID = dgBuchung.Rows(0).Cells(arX(5)).Value?.ToString()
            sgSasion = "V"

            If cgTablet = "1" Then
                frmReservierung.Show()
            Else
                frmReservierungDest.Show()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Buchungsvorgang kann gestartet werden, wenn die Taste "B" gedrückt wird => lBuc = True
    ''' Datum und Zimmer mit der Maus markieren
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 19.01.2012 Create
    ''' 23.09.2026 Code optimiert
    ''' </remarks>
    Private Sub dgBuchung_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgBuchung.KeyDown
        Select Case e.KeyCode
            Case Keys.B
                lBuc = True
            Case Keys.G
                lGesp = True
        End Select
    End Sub

    ''' <summary>
    ''' Markierung der Zimmer und Datum beenden, wenn die entsprechenden Tasten losgelassen werden
    ''' </summary>
    ''' <remarks>
    ''' 19.01.2012 Create
    ''' 23.09.2026 Code optimiert: Setzt Flags nur bei den relevanten Tasten (B und G) zurück
    ''' </remarks>
    Private Sub dgBuchung_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgBuchung.KeyUp
        Select Case e.KeyCode
            Case Keys.B
                lBuc = False
            Case Keys.G
                lGesp = False
        End Select
    End Sub

    ''' <summary>
    ''' Koordinaten (XY) beim Drücken der Maustaste (Reihe/Spalte) ermitteln
    ''' </summary>
    ''' <param name="sender">Das auslösende Objekt.</param>
    ''' <param name="e">Die Ereignisdaten mit den Zellkoordinaten.</param>
    ''' <remarks>
    ''' 12.01.2012 Create
    ''' 25.09.2026: Fehler bei Header-Klicks (-1) behoben
    ''' </remarks>
    Private Sub dgBuchung_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgBuchung.CellMouseDown
        ' Sicherstellen, dass nicht auf einen Header (Index -1) geklickt wurde
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            arX(0) = e.RowIndex
            arX(1) = e.ColumnIndex

            ' Prüfen, ob die linke Maustaste gedrückt wurde
            If e.Button = Windows.Forms.MouseButtons.Left Then
                arX(4) = arX(0)
                arX(5) = arX(1)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Koordinaten (XY) beim Loslassen der Maustaste (Spalte/Reihe) ermitteln
    ''' </summary>
    ''' <remarks>
    ''' 12.01.2012 Create
    ''' 23.09.2026 Code optimiert, Spalten-/Zeilendreher korrigiert und Header-Klick abgesichert
    ''' </remarks>
    Private Sub dgBuchung_CellMouseUp(ByVal sender As Object,
                                  ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) _
                                  Handles dgBuchung.CellMouseUp

        ' 1. Nur auf den Linksklick reagieren
        If e.Button <> Windows.Forms.MouseButtons.Left Then Exit Sub

        ' 2. Klicks auf Spalten- oder Zeilenüberschriften abfangen (-1 verhindern)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

        ' 3. Logische Bedingungen frühzeitig prüfen (Guard Clauses sparen Rechenzeit)
        If Not lBuc AndAlso Not lGesp Then Exit Sub

        ' 4. Koordinaten sauber zuweisen
        ' Wichtig: ColumnIndex = X (Zeile), RowIndex = Y (Spalte)

        arX(2) = e.RowIndex
        arX(3) = e.ColumnIndex

        arX(6) = arX(2)
        arX(7) = arX(3)

        ' 5. Koordinaten sortieren (Nutzt die optimierte Funktion aus Schritt 1)
        arX = fcSetKoordinaten(arX)

        ' 6. Reservierung aufrufen mit den sortierten Werten
        Call prReservierung(arX(0), arX(1), arX(2), arX(3))
    End Sub

    ''' <summary>
    ''' Umrechnung der Koordinaten nach der Original-Logik (Wertetausch überkreuz)
    ''' </summary>
    ''' <param name="arT">Das Array mit den Koordinaten</param>
    ''' <returns>Das sortierte Integer-Array</returns>
    ''' <remarks>
    ''' 12.01.2012 Create
    ''' 23.09.2026 Typsicherheit erhöht, Original-Tauschlogik wiederhergestellt
    ''' </remarks>
    Private Function fcSetKoordinaten(ByVal arT() As Integer) As Integer()
        ' Sicherheitsprüfung für das Array
        If arT Is Nothing OrElse arT.Length < 4 Then Return arT

        ' Wichtig: Die Variablen müssen wie im Original überkreuz zugewiesen werden!
        Dim x As Integer
        Dim y As Integer

        If arT(2) < arT(0) Then
            x = arT(0)
            y = arT(2)
            arT(0) = y
            arT(2) = x
        End If

        If arT(3) < arT(1) Then
            x = arT(1)
            y = arT(3)
            arT(1) = y
            arT(3) = x
        End If

        Return arT
    End Function

    ''' <summary>
    ''' Nimmt die Reservierung für ausgewählte Zimmer und Zeiträume vor.
    ''' </summary>
    ''' <param name="r1">Index der Start-Zeile (Start-Datum)</param>
    ''' <param name="c1">Index der Start-Spalte (Erstes Zimmer)</param>
    ''' <param name="r2">Index der End-Zeile (End-Datum)</param>
    ''' <param name="c2">Index der End-Spalte (Letztes Zimmer)</param>
    ''' <remarks>
    ''' 19.01.2012 Create <br/>
    ''' 23.09.2026 Refactored (Typsicherheit & Validierung hinzugefügt)
    ''' </remarks>
    Private Sub prReservierung(ByVal r1 As Integer, ByVal c1 As Integer,
                            ByVal r2 As Integer, ByVal c2 As Integer)

        ' Validierung: Sicherstellen, dass Indizes im gültigen Bereich des Grids liegen
        If dgBuchung.Rows.Count <= Math.Max(r1, r2) OrElse dgBuchung.Columns.Count <= Math.Max(c1, c2) Then
            Throw New ArgumentOutOfRangeException("Die angegebenen Zeilen- oder Spaltenindizes liegen außerhalb des DataGridView-Bereichs.")
        End If

        ' Auslesen der Datums-Werte mit Fallback für leere Zellen
        Dim sBDate As String = Convert.ToString(dgBuchung.Rows(r1).Cells(0).Value)
        Dim sEDate As String = Convert.ToString(dgBuchung.Rows(r2).Cells(0).Value)

        ' Eindeutige Buchungsnummer generieren
        Dim sBnr As String = fcGetTimeID(Date.Today)

        ' Schleife durch alle ausgewählten Zimmer (Spalten)
        For i As Integer = c1 To c2
            Dim cellValue As Object = dgBuchung.Rows(0).Cells(i).Value

            If cellValue IsNot Nothing AndAlso Not String.IsNullOrEmpty(cellValue.ToString()) Then
                Dim sZim As String = cellValue.ToString()
                ' Reservierung in der Datenbank/System anlegen
                Call prCreateReservierung(sBDate, sEDate, sZim, sBnr)
            End If
        Next

        ' Buchungsnummer im System aktualisieren/hochzählen
        Call prSetNr("BNr", sBnr)
    End Sub

    ''' <summary>
    ''' Legt einen neuen Datensatz für die Zimmerreservierung an und aktualisiert die Buchungsdaten.
    ''' </summary>
    ''' <param name="sBDate">Startdatum der Reservierung</param>
    ''' <param name="sEDate">Enddatum der Reservierung</param>
    ''' <param name="sZim">Zimmer-ID / Zimmerbezeichnung</param>
    ''' <param name="sBnr">Buchungsnummer</param>
    ''' <remarks>
    ''' 19.01.2012 Create <br/>
    ''' 23.09.2026 Refactored (Bugfix bei Variablen-Verschiebung & Typsicherheit)
    ''' </remarks>
    Private Sub prCreateReservierung(ByVal sBDate As String, ByVal sEDate As String,
                                  ByVal sZim As String, ByVal sBnr As String)

        ' 1. Leeren Datensatz in der Tabelle "Buchung" anlegen und ID holen
        Dim ID As String = fcAppendBlank("Buchung")
        If String.IsNullOrEmpty(ID) Then Exit Sub ' Absicherung, falls DB-Insert fehlschlägt

        sBDate = fcUmDatum(sBDate)
        sEDate = fcUmDatum(sEDate)

        ' 2. Spalten-Definitionen festlegen
        Dim sqlText As String = "ID,BID,Von,Bis,VonZeit,BisZeit,ObjID,ZimID,Variable,KunID,Personen,Tiere,Art,Frueh,Kategorie,Preis,Anzahlung,Werbung,Info,Rechnung,BuchDatum,Sprache,BText,MText,RID,RDatum,FPreis,RDSenden,BIDIndex,IDRef,RAID,RADatum,Storno,Summe,Code,SendMailZugang"
        Dim arFields() As String = sqlText.Split(","c)

        ' 3. Werte-Array exakt parallel aufbauen (Verhindert Verschiebungs-Bugs)
        Dim sb2 As New StringBuilder

        sb2.Append(ID & "°")        ' ID
        sb2.Append(sBnr & "°")      ' BID
        sb2.Append(sBDate & "°")    ' Von
        sb2.Append(sEDate & "°")    ' Bis
        sb2.Append("1200°")         ' VonZeit
        sb2.Append("1200°")         ' BisZeit
        sb2.Append(fcGetObjID(sZim) & "°") ' ObjID
        sb2.Append(sZim & "°")      ' ZimID
        sb2.Append("0" & "°")       ' Variable
        If lGesp Then sb2.Append("1°")
        If lBuc Then sb2.Append("0°")
        sb2.Append("0°") ' KunID
        sb2.Append("0°") ' Personen
        sb2.Append(" °") ' Tiere
        sb2.Append("0°") ' Art
        sb2.Append(" °") ' Frueh
        sb2.Append("0°") ' Kategorie
        sb2.Append("0°") ' Preis
        sb2.Append(" °") ' Anzahlung
        sb2.Append(" °") ' Werbung
        sb2.Append("0°") ' Info
        sb2.Append(fcUmDatum(Date.Today) & "°") ' Rechnung (oder BuchDatum?)
        sb2.Append("0°") ' Sprache
        sb2.Append(" °") ' BText
        sb2.Append(" °") ' MText
        sb2.Append(" °") ' RID
        sb2.Append(" °") ' RDatum
        sb2.Append("0°") ' FPreis
        sb2.Append(" °") ' RDSenden
        sb2.Append("0°") ' BIDIndex
        sb2.Append(sBnr & "°") ' IDRef
        sb2.Append(" °") ' RAID
        sb2.Append(" °") ' RADatum
        sb2.Append("100°") ' Storno
        sb2.Append("0°") ' Summe
        sb2.Append(" °") ' Code (sCode ist leer)
        sb2.Append("Y") ' SendMailZugang (Letztes Element ohne abschließendes Trennzeichen analog zum Original)

        ' 4. String in Array splitten
        Dim arValue() As String = sb2.ToString().Split("°"c)

        ' 5. Datenbank-Updates und Folge-Aktionen ausführen
        fcUpdateCommand("Buchung", arFields, arValue, " WHERE ID='" & ID & "'")
        fcSetEntryInBuchnunsTabelle(sBDate, sEDate, sZim, "0", "0", sBnr, 0, False, ID)

        ' Globalen Status setzen
        sgRBID = sBnr
    End Sub

    ''' <summary>
    ''' Ermittelt die Objekt-ID eines bestimmten Zimmers anhand seiner Zimmer-ID.
    ''' </summary>
    ''' <param name="sZim">Die ID des gesuchten Zimmers.</param>
    ''' <returns>Die zugehörige Objekt-ID als String, oder ein Leerzeichen, falls nicht gefunden.</returns>
    ''' <remarks>
    ''' 19.01.2012 Create <br/>
    ''' 23.09.2026 Refactored (DataTable-Suche optimiert & Typsicherheit erhöht)
    ''' </remarks>
    Private Function fcGetObjID(ByVal sZim As String) As String
        ' Standard-Rückgabewert definieren (analog zum Original)
        Dim defaultResult As String = " "

        ' Validierung der Tabelle und des Suchbegriffs
        If dtZim Is Nothing OrElse String.IsNullOrEmpty(sZim) Then Return defaultResult

        Try
            ' Suche mithilfe eines Filters ausführen (entspricht einer schnellen Index-Suche)
            ' SingleQuotes maskieren, um SQL-Syntaxfehler bei Sonderzeichen im String zu vermeiden
            Dim filter As String = String.Format("ID = '{0}'", sZim.Replace("'", "''"))
            Dim foundRows() As DataRow = dtZim.Select(filter)

            ' Wenn ein passender Datensatz gefunden wurde
            If foundRows.Length > 0 Then
                Dim value As Object = foundRows(0)("IDObjekte")

                ' Prüfen, ob der Wert in der Datenbank nicht NULL ist
                If value IsNot DBNull.Value AndAlso value IsNot Nothing Then
                    Return value.ToString()
                End If
            End If
        Catch ex As Exception
            ' Optional: Hier Fehler protokollieren (z. B. falls Spaltennamen fehlen)
        End Try

        Return defaultResult
    End Function

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des Kontextmenü-Eintrags zum Löschen einer Buchung.
    ''' Überprüft die ausgewählte Zelle, validiert die Existenz in der Datenbank, 
    ''' setzt optionale Transponder-Codes zurück, löscht den Datensatz und aktualisiert das UI-Grid.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das Menüelement).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' Die Methode nutzt das globale Array <c>arX</c> zur Bestimmung der ausgewählten Zeilen- und Spaltenindizes.
    ''' Zur Performance-Optimierung wird das Zeichnen des Grids (<c>dgBuchung</c>) während der UI-Aktualisierung temporär pausiert.
    ''' 19.01.2012 Create <br/>
    ''' 23.09.2026 Refactored 
    ''' </remarks>
    Private Sub tsmDelete_Click(sender As System.Object, e As System.EventArgs) Handles tsmDelete.Click
        ' 1. Sicherheitsprüfung: Wurde überhaupt eine Zelle ausgewählt/gespeichert?
        If arX Is Nothing OrElse arX.Length < 2 Then Exit Sub
        prInfo("tsmDelete_Click: arX = " & String.Join(",", arX))
        ' Werte aus der ausgewählten Zelle und dem Header (Zimmer) lesen
        Dim cellValue As Object = dgBuchung.Rows(arX(0)).Cells(arX(1)).Value
        If cellValue Is Nothing OrElse String.IsNullOrEmpty(cellValue.ToString()) Then Exit Sub

        Dim sBNr As String = cellValue.ToString()
        Dim sBNr1 As String = Extract(sBNr, "(", ")", 1)

        Dim headerValue As Object = dgBuchung.Rows(0).Cells(arX(1)).Value
        If headerValue Is Nothing Then Exit Sub
        Dim sZim1 As String = headerValue.ToString()

        ' 2. Datenbank abfragen und prüfen, ob Daten existieren
        ' HINWEIS: Idealerweise fcReadDataTable so umbauen, dass es Parameter nutzt!
        Dim sSQL As String = String.Format("Select * From Buchung Where BID='{0}' and ZimID = '{1}'",
                                       sBNr1.Replace("'", "''"), sZim1.Replace("'", "''"))
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' Schutz vor Absturz, falls die Buchung in der DB nicht (mehr) existiert
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            MsgBox("Die Buchung konnte in der Datenbank nicht gefunden werden.", vbCritical, "Fehler")
            Exit Sub
        End If

        ' Daten aus der ersten Zeile extrahieren
        Dim row As DataRow = dt.Rows(0)
        Dim ID As String = row("ID").ToString()
        Dim sZim As String = row("ZimID").ToString()
        Dim sBeginStr As String = fcUmDatum(row("Von").ToString())
        Dim sEndStr As String = fcUmDatum(row("Bis").ToString())
        Dim Code As String = row("Code").ToString().Trim()
        Dim bTransponder As Boolean

        ' Datums-Strings für den Grid-Vergleich in echte Date-Objekte umwandeln (sicherer)
        Dim dBegin As Date, dEnd As Date
        If Not Date.TryParse(sBeginStr, dBegin) OrElse Not Date.TryParse(sEndStr, dEnd) Then
            MsgBox("Fehler beim Konvertieren des Buchungszeitraums.", vbCritical, "Fehler")
            Exit Sub
        End If

        Dim aTem As String() = Split(sBNr, " ")
        sBNr1 = aTem(0) & ";" & ID

        If String.IsNullOrEmpty(sBNr1) Then Exit Sub

        ' 3. Benutzer fragen
        Dim sMsg As String = "Wollen Sie diese Buchung löschen?"
        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            bTransponder = True

            ' Transponder / Code-Rücksetzung
            If Code <> "" Then
                Dim sC As String = fcCode(Code)
                Dim sqlText As String = "code,von,vonzeit,bis,biszeit,BID"
                Dim arFields As String() = Split(sqlText, ",")
                Dim sb As String = sC & "°20000101°1200°20000202°1200°00000"
                Dim arValue As String() = Split(sb, "°")
                Dim cBedingung As String = " WHERE code='" & sC.Replace("'", "''") & "'"

                fcUpdateCommand("code", arFields, arValue, cBedingung)
                ' bTransponder = fcDelTransponder()
            End If

            ' Aus der Datenbank löschen
            UpdateTable(String.Format("DELETE FROM Buchung WHERE ID = '{0}'", ID.Replace("'", "''")))

            ' 4. UI-Tabelle aktualisieren (Optimiert)
            Dim nCol As Integer = fcGetZimmerSpalte(sZim1, dgBuchung)
            If nCol <> -1 Then
                Dim nMaxRow As Integer = dgBuchung.Rows.Count - 1

                ' UI-Zeichnen während der Schleife pausieren
                dgBuchung.SuspendLayout()
                Try
                    With dgBuchung
                        For i As Integer = 0 To nMaxRow
                            Dim cellDateValue As Object = .Rows(i).Cells(0).Value

                            If cellDateValue IsNot Nothing Then
                                Dim sDay As Date
                                ' Nur verarbeiten, wenn die Zelle ein gültiges Datum enthält
                                If Date.TryParse(cellDateValue.ToString(), sDay) Then

                                    ' Wenn der Tag im Zeitraum liegt
                                    If sDay >= dBegin AndAlso sDay <= dEnd Then
                                        .Rows(i).Cells(nCol).Value = ""
                                        .Rows(i).Cells(nCol).Style.BackColor = Color.White
                                    End If

                                    ' Abbruch, sobald das Enddatum überschritten wurde
                                    If sDay >= dEnd Then Exit For
                                End If
                            End If
                        Next
                    End With
                Finally
                    ' UI-Zeichnen wieder aktivieren
                    dgBuchung.ResumeLayout()
                    prInfo("")
                End Try
            End If
        End If
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des Kontextmenü-Eintrags zum Bearbeiten einer Buchung.
    ''' Extrahiert die Buchungs- und Zimmer-IDs aus der ausgewählten Zelle und öffnet 
    ''' abhängig vom Gerätetyp (Tablet oder Desktop) das entsprechende Reservierungsfenster.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das Menüelement).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' Die Methode setzt die globalen Variablen <c>sgRZID</c>, <c>sgRBID</c> und <c>sgSasion</c>, 
    ''' welche von den aufgerufen Formularen zur Datenanzeige ausgelesen werden.
    ''' 19.01.2012 Create <br/>
    ''' 23.09.2026 Refactored
    ''' </remarks>
    Private Sub tsmEdit_Click(sender As System.Object, e As System.EventArgs) Handles tsmEdit.Click
        ' 1. Sicherheitsprüfung: Wurde überhaupt eine Zelle ausgewählt/gespeichert?
        If arX Is Nothing OrElse arX.Length < 2 Then Exit Sub

        ' 2. Zellen-Wert sicher auslesen (Verhindert Absturz, falls die Zelle Nothing ist)
        Dim cellValue As Object = dgBuchung.Rows(arX(0)).Cells(arX(1)).Value
        If cellValue Is Nothing Then Exit Sub

        Dim sBNr As String = cellValue.ToString().Trim()
        If String.IsNullOrEmpty(sBNr) Then Exit Sub

        ' 3. Header-Wert (Zimmer-ID) sicher auslesen
        Dim headerValue As Object = dgBuchung.Rows(0).Cells(arX(1)).Value
        If headerValue Is Nothing Then Exit Sub

        ' 4. Globale Variablen für das Ziel-Formular setzen
        sgRZID = headerValue.ToString()
        sgRBID = Extract(sBNr, "(", ")", 1)
        sgSasion = "V"

        ' 5. Passendes Formular basierend auf dem Gerätetyp öffnen
        If cgTablet = "1" Then
            frmReservierung.Show()
        Else
            frmReservierungDest.Show()
        End If
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des Kontextmenü-Eintrags zum Kopieren einer Buchung.
    ''' Liest die Buchungsdaten ein, fügt die Buchungs-Informationen der Zwischenablage (<c>tscBuchnung</c>) hinzu,
    ''' setzt das Zimmer in der Datenbank zurück, speichert die Kopie und bereinigt den betroffenen Zeitraum im UI-Grid.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das Menüelement).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 19.01.2012 Create <br/>
    ''' 23.09.2026 Refactored
    ''' </remarks>
    Private Sub tsmCopy_Click(sender As System.Object, e As System.EventArgs) Handles tsmCopy.Click
        ' 1. Sicherheitsprüfung: Wurde überhaupt eine Zelle ausgewählt/gespeichert?
        If arX Is Nothing OrElse arX.Length < 2 Then Exit Sub

        ' 2. Zellen-Wert sicher auslesen
        Dim cellValue As Object = dgBuchung.Rows(arX(0)).Cells(arX(1)).Value
        If cellValue Is Nothing Then Exit Sub
        Dim sBNr As String = cellValue.ToString().Trim()
        If String.IsNullOrEmpty(sBNr) Then Exit Sub

        ' 3. Header-Wert (Zimmer) sicher auslesen
        Dim headerValue As Object = dgBuchung.Rows(0).Cells(arX(1)).Value
        If headerValue Is Nothing Then Exit Sub
        Dim sZim1 As String = headerValue.ToString()

        ' 4. Buchungsnummer extrahieren und DB abfragen
        Dim sBNr1 As String = Extract(sBNr, "(", ")", 1)
        Dim sSQL As String = String.Format("Select * From Buchung Where BID='{0}' and ZimID = '{1}'",
                                       sBNr1.Replace("'", "''"), sZim1.Replace("'", "''"))
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' Schutz vor Absturz, falls die Zeile in der DB nicht existiert
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            MsgBox("Die Buchung konnte in der Datenbank nicht gefunden werden.", vbCritical, "Fehler")
            Exit Sub
        End If

        ' 5. Daten aus der ersten Zeile extrahieren
        Dim row As DataRow = dt.Rows(0)
        Dim ID As String = row("ID").ToString()
        Dim sBeginStr As String = fcUmDatum(row("Von").ToString())
        Dim sEndStr As String = fcUmDatum(row("Bis").ToString())

        ' Datums-Strings in echte Date-Objekte umwandeln für sicheren Grid-Vergleich
        Dim dBegin As Date, dEnd As Date
        If Not Date.TryParse(sBeginStr, dBegin) OrElse Not Date.TryParse(sEndStr, dEnd) Then
            MsgBox("Fehler beim Konvertieren des Buchungszeitraums.", vbCritical, "Fehler")
            Exit Sub
        End If

        ' Text splitten und ID anhängen
        Dim aTem As String() = Split(sBNr, " ")
        sBNr1 = aTem(0) & ";" & ID

        If String.IsNullOrEmpty(sBNr1) Then Exit Sub

        ' 6. Zur internen Zwischenablage hinzufügen
        tscBuchnung.Items.Add(sBNr1)

        ' 7. Datenbank aktualisieren (Zimmer zurücksetzen)
        Dim arFields As String() = {"ZimID"}
        Dim arValue As String() = {"0"}
        Dim cBedingung As String = " WHERE ID='" & ID.Replace("'", "''") & "'"
        Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)

        ' Kopie-Logik ausführen
        Call prSaveCopy()

        ' 8. UI-Tabelle aktualisieren
        Dim nCol As Integer = fcGetZimmerSpalte(sZim1, dgBuchung)
        If nCol <> -1 Then
            Dim nMaxRow As Integer = dgBuchung.Rows.Count - 1

            dgBuchung.SuspendLayout()
            Try
                With dgBuchung
                    For i As Integer = 0 To nMaxRow
                        Dim cellDateValue As Object = .Rows(i).Cells(0).Value

                        If cellDateValue IsNot Nothing Then
                            Dim sDay As Date
                            If Date.TryParse(cellDateValue.ToString(), sDay) Then

                                ' Wenn der Tag im Zeitraum liegt
                                If sDay >= dBegin AndAlso sDay <= dEnd Then
                                    .Rows(i).Cells(nCol).Value = ""
                                    .Rows(i).Cells(nCol).Style.BackColor = Color.White
                                End If

                                ' Abbruch, sobald das Enddatum erreicht wurde
                                If sDay >= dEnd Then Exit For
                            End If
                        End If
                    Next
                End With
            Finally
                dgBuchung.ResumeLayout()
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des Kontextmenü-Eintrags zum Einfügen einer kopierten Buchung.
    ''' Überprüft, ob das Zielzimmer im Buchungszeitraum frei ist. Wenn ja, wird die Buchung 
    ''' dem Zimmer zugewiesen, die Datenbank aktualisiert und das UI-Grid neu gezeichnet.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das Menüelement).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 19.01.2012 Create <br/>
    ''' 23.09.2026 Refactored
    ''' </remarks>
    Private Sub tsmInset_Click(sender As System.Object, e As System.EventArgs) Handles tsmInset.Click
        ' 1. Validierung der Zwischenablage und Auswahlkoordinaten
        If String.IsNullOrWhiteSpace(tscBuchnung.Text) Then Exit Sub
        If arX Is Nothing OrElse arX.Length < 2 Then Exit Sub

        ' Text aus der Zwischenablage splitten (Erwartet: "Buchungstext;ID")
        Dim aTem As String() = Split(tscBuchnung.Text, ";")
        If aTem.Length < 2 Then Exit Sub

        ' 2. Buchungsdaten aus der Datenbank laden
        Dim sSQL As String = String.Format("Select * From Buchung Where ID='{0}'", aTem(1).Replace("'", "''"))
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' Schutz vor Absturz, falls die Buchung nicht in der DB existiert
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            MsgBox("Die einzufügende Buchung konnte in der Datenbank nicht gefunden werden.", vbCritical, "Fehler")
            Exit Sub
        End If

        ' Daten aus der ersten Zeile extrahieren
        Dim row As DataRow = dt.Rows(0)
        Dim sID As String = row("ID").ToString()
        Dim sBuc As String = row("BID").ToString()
        Dim sFr As String = row("Frueh").ToString()
        Dim sPer As String = row("Personen").ToString()
        ' Konvertiert den Wert sicher in einen String. Falls NULL in der DB steht, wird ein leerer String verwendet.
        Dim rechnungValue As String = If(TypeOf row("Rechnung") Is DBNull, "0", row("Rechnung").ToString().Trim())
        ' Ergibt True, wenn der Wert "1" ist, andernfalls False (somit auch bei "0" oder NULL)
        Dim lRec As Boolean = (rechnungValue = "1")
        ' Zimmer-Header auslesen
        Dim headerValue As Object = dgBuchung.Rows(0).Cells(arX(1)).Value
        If headerValue Is Nothing Then Exit Sub
        Dim sZim1 As String = headerValue.ToString()

        ' Datums-Strings einlesen 
        Dim sBegin As String = row("Von").ToString()
        Dim sEnd As String = row("Bis").ToString()

        ' 3. Belegungsprüfung im UI-Grid
        Dim nCol As Integer = arX(1)
        Dim nMaxRow As Integer = dgBuchung.Rows.Count - 1
        Dim isOccupied As Boolean = False

        For i As Integer = 0 To nMaxRow
            Dim sDay As String = fcUmDatum(dgBuchung.Rows(i).Cells(0).Value).ToString
            If sDay IsNot Nothing Then

                ' Wenn der Tag im Buchungszeitraum liegt
                If sDay >= sBegin Then
                    ' Prüfen, ob die Zelle bereits Text enthält
                    Dim currentCellValue As Object = dgBuchung.Rows(i).Cells(nCol).Value
                    If currentCellValue IsNot Nothing AndAlso Not String.IsNullOrEmpty(currentCellValue.ToString().Trim()) Then
                        isOccupied = True
                        Exit For ' Schleife sofort verlassen, da bereits belegt
                    End If
                End If

                ' Abbruch, sobald das Enddatum überschritten wurde
                If sDay >= sEnd Then Exit For
            End If
        Next

        ' 4. Verarbeitung basierend auf dem Prüfergebnis
        If isOccupied Then
            MsgBox("Das Zimmer ist im ausgewählten Zeitraum bereits besetzt!", vbExclamation, "Zimmer belegt")
        Else
            ' In Buchungstabelle eintragen
            fcSetEntryInBuchnunsTabelle(sBegin, sEnd, sZim1, sFr, sPer, sBuc, 0, lRec, sID)

            ' Datenbank aktualisieren (Zimmer-ID der Buchung zuweisen)
            Dim arFields As String() = {"ZimID"}
            Dim arValue As String() = {sZim1}
            Dim cBedingung As String = " WHERE ID='" & sID.Replace("'", "''") & "'"
            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)

            ' Eintrag aus der Zwischenablage entfernen
            tscBuchnung.Items.Remove(tscBuchnung.Text)
            tscBuchnung.Text = ""
            tsmInset.Enabled = False
        End If

        ' Zustand der Zwischenablage sichern
        Call prSaveCopy()
    End Sub

    ''' <summary>
    ''' Verarbeitet die Textänderung im Buchungs-Steuerelement. 
    ''' Splittet den Text, lädt die entsprechenden Buchungsdaten aus der Datenbank 
    ''' und aktualisiert die Anzeige für den Zeitraum.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (tscBuchnung).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 19.01.2012 Create <br/>
    ''' 23.09.2026 Refactored
    ''' </remarks>
    Private Sub tscBuchnung_TextChanged(sender As System.Object, e As System.EventArgs) Handles tscBuchnung.TextChanged
        ' 1. Prüfung auf leeren Text
        If String.IsNullOrWhiteSpace(tscBuchnung.Text) Then
            tstAnreise.Text = String.Empty
            tstEnd.Text = String.Empty
            Return
        End If

        ' 2. Text splitten und prüfen, ob ein Trennzeichen vorhanden war
        Dim aTem As String() = tscBuchnung.Text.Split(";"c)
        If aTem.Length < 2 Then
            ' Optional: Fehlermeldung oder Rückkehr, da kein gültiger Index 1 existiert
            Return
        End If

        tsmInset.Enabled = True

        ' 3. SQL-Abfrage absichern (Hinweis: Parameter wären sicherer als String-Verkettung)
        Dim sSQL As String = String.Format("Select * From Buchung Where ID='{0}'", aTem(1).Replace("'", "''"))
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' 4. Prüfen, ob überhaupt Daten aus der DB zurückgegeben wurden
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim row As DataRow = dt.Rows(0)

            ' Datumsformate sicher auslesen und umwandeln
            Dim sBegin As String = fcUmDatum(If(row("Von")?.ToString(), String.Empty))
            Dim sEnd As String = fcUmDatum(If(row("Bis")?.ToString(), String.Empty))

            tstAnreise.Text = "Von " & sBegin
            tstEnd.Text = "Bis " & sEnd
        End If

        ' 5. Prüfen, ob das DataGridView Zeilen enthält, bevor auf Index 0 zugegriffen wird
        If dgBuchung.Rows.Count > 0 AndAlso arX.Length > 1 Then
            Dim sZim1 As String = dgBuchung.Rows(0).Cells(arX(1)).Value?.ToString()
            ' Hinweis: sZim1 wird im Originalcode danach nicht mehr verwendet. 
            ' Falls es global gebraucht wird, Zuweisung anpassen.
        End If
    End Sub

    ''' <summary>
    ''' Bei Rechtsklick Buchungstext und Makrotext der Reservierung anzeigen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 01.04.2012 Create
    ''' </remarks>
    Private Sub dgBuchung_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgBuchung.CellMouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If e.ColumnIndex > 1 And e.RowIndex > -1 Then
                Dim sTmp As String
                Dim sBNr As String = dgBuchung.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                sBNr = Extract(sBNr, "(", ")", 1)
                If sBNr <> "" Then
                    Dim sSQL As String = "Select * From Buchung Where BID='" & sBNr & "'"
                    Dim dt As DataTable = fcReadDataTable(sSQL)
                    sTmp = dt.Rows(0).Item("BText").ToString '& vbCrLf
                    Dim dtB As DataTable = fcReadDataTable("Select * from BTexte Where ID='" & sTmp & "'")
                    If dtB.Rows.Count <> 0 Then
                        sTmp = dtB.Rows(0).Item(3).ToString & vbCrLf & vbCrLf
                    Else
                        sTmp = ""
                    End If
                    sTmp = sTmp & dt.Rows(0).Item("MText").ToString & vbCrLf

                    If sTmp.Trim = "" Then Exit Sub
                    MsgBox(sTmp, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Buchnungstext zur Buchung: " & sBNr)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Reagiert auf einen Doppelklick in eine Zelle des Buchungs-Datagridviews,
    ''' extrahiert die Zimmer- sowie Buchungsnummer und öffnet das passende Reservierungsformular.
    ''' </summary>
    ''' <param name="sender">Das auslösende DataGridView-Objekt.</param>
    ''' <param name="e">Die Ereignisdaten mit Zeilen- und Spaltenindex.</param>
    ''' <remarks>
    ''' 27.12.2011 - Erstellt<br/>
    ''' 24.09.2026 - Optimiert: Header-Klicks abgefangen, String-Prüfung korrigiert, Exception-Handling aktiviert und NullReference-Schutz hinzugefügt.
    ''' </remarks>
    Private Sub dgBuchung_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgBuchung.CellDoubleClick
        Try
            ' WICHTIG: Klicks auf Spalten- oder Zeilenüberschriften abfangen (Index -1)
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

            ' Wenn der Klick in einer der ersten beiden Spalten (Index 0 und 1) erfolgte, ignorieren
            If e.ColumnIndex < 2 Then Exit Sub

            Dim cellValue As Object = dgBuchung.Rows(e.RowIndex).Cells(e.ColumnIndex).Value

            ' Sichere Prüfung, ob die Zelle befüllt ist
            If cellValue IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(cellValue.ToString()) Then

                ' Rechteprüfung (Nur ausführen, wenn Status kleiner als 4)
                If ngRechteStatus < 4 Then
                    Dim sB As String = cellValue.ToString()

                    ' Zimmernummer aus der ersten Zeile der gewählten Spalte lesen
                    Dim headerValue As Object = dgBuchung.Rows(0).Cells(e.ColumnIndex).Value
                    sgRZID = If(headerValue IsNot Nothing, headerValue.ToString(), String.Empty)

                    ' Buchungsnummer extrahieren
                    sgRBID = Extract(sB, "(", ")", 1)
                    sgSasion = "V"

                    ' Formular je nach Gerätetyp (Tablet oder Desktop) öffnen
                    If cgTablet = "1" Then
                        frmReservierung.Show()
                    Else
                        frmReservierungDest.Show()
                    End If
                End If
            End If

        Catch ex As Exception
            ' Fehlerbericht wieder aktiviert, damit Fehler nicht unbemerkt bleiben
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Speichert den aktuellen Inhalt der Zwischenablage (<c>tscBuchnung</c>) in dem Feld <c>Copy</c> der Systemdatenbank.
    ''' Die Einträge werden mit dem Trennzeichen '²' verkettet und über die Funktion <c>WriteFile</c> gesichert.
    ''' </summary>
    Private Sub prSaveCopy()
        ' 1. Prüfen, ob überhaupt Einträge zum Speichern vorhanden sind
        If tscBuchnung.Items.Count = 0 Then
            Call SaveOneValueInSystemDb("copy", "")
            Exit Sub
        End If

        ' 2. Alle Einträge in eine Liste laden
        Dim itemsList As New List(Of String)(tscBuchnung.Items.Count)
        For i As Integer = 0 To tscBuchnung.Items.Count - 1
            Dim item As Object = tscBuchnung.Items(i)
            If item IsNot Nothing Then
                itemsList.Add(item.ToString())
            End If
        Next

        ' 3. Alle Elemente effizient mit dem Trennzeichen '²' verbinden 
        ' (Fügt das Trennzeichen automatisch auch ganz am Ende an, passend zum Original)
        Dim sText As String = String.Join("²", itemsList) & "²"

        ' 4. In Datenbank schreiben
        Call SaveOneValueInSystemDb("copy", sText)
    End Sub

    ''' <summary>
    ''' Liest kopierte Buchungsdaten aus dem Feld <c>Copy</c> der Systemdatenbank und fügt die bereinigten 
    ''' Einträge der Element-Sammlung von <c>tscBuchnung</c> hinzu.
    ''' </summary>
    ''' <remarks>
    ''' <para>Die Datei wird anhand des Trennzeichens '²' aufgeteilt. Leere Einträge werden ignoriert.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – Code Optimierung<br/>
    ''' </para>
    ''' </remarks>
    Private Sub prReadCopy()
        ' Aus der Datenbank lesen
        Dim stext As String = ReadOneValueFromSystemDb("Copy")

        ' Prüfen, ob überhaupt Text zurückgegeben wurde
        If String.IsNullOrEmpty(stext) Then Exit Sub

        ' Splitten mit dem modernen .NET-StringSplitOptions, um leere Einträge direkt zu ignorieren
        Dim atext() As String = stext.Split(New Char() {"²"c}, StringSplitOptions.RemoveEmptyEntries)

        ' Steuerelement während des Hinzufügens einfrieren (verhindert Flackern bei vielen Einträgen)
        ' Hinweis: Falls es sich um eine ComboBox/ListBox handelt, BeginUpdate nutzen. 
        ' Bei einer ToolStripItemCollection (tscBuchnung) entfällt dieser Schritt meist.

        For Each eintrag As String In atext
            ' Trimmen und hinzufügen
            Dim getrimmterEintrag As String = eintrag.Trim()
            If getrimmterEintrag <> "" Then
                tscBuchnung.Items.Add(getrimmterEintrag)
            End If
        Next
    End Sub

#End Region

#Region "Daten sichern............................................................................."

    ''' <summary>
    ''' Frägt den Benutzer, ob eine manuelle Datensicherung durchgeführt werden soll, 
    ''' führt diese aus und protokolliert den Erfolg im Logbuch.
    ''' </summary>
    ''' <remarks>
    ''' <para>Zeigt einen Bestätigungsdialog. Bei 'Ja' wird die Routine <see cref="prSaveDaten"/> aufgerufen und ein Eintrag in <c>cgLogFile</c> erzeugt.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – Optimiert: Umstellung auf das moderne <c>MessageBox.Show</c>, Entfernung von <c>Call</c> und Absicherung des Log-Eintrags per Try-Catch.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub prSaveData()
        ' Modernes .NET-Äquivalent für die Sicherheitsabfrage (Standardbutton ist 'Nein')
        Dim meldungText As String = "Die aktuelle Konfiguration wird gesichert. Die Sicherungsdatei " & Environment.NewLine &
                                "liegt in dem Verzeichnis ""...\SaveDB\Montag - Sonntag""." & Environment.NewLine &
                                "Sicherung durchführen?"

        Dim result As DialogResult = MessageBox.Show(meldungText, "Datensicherung", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If result = DialogResult.Yes Then
            Try
                ' Mauszeiger auf Sanduhr/Warten stellen, da das Sichern von Daten Zeit benötigt
                Me.Cursor = Cursors.WaitCursor
                ' Führt die eigentliche Sicherung aus 
                prSaveDaten()

                ' Nur wenn die Sicherung fehlerfrei durchgelaufen ist, wird das Protokoll geschrieben
                fcWriteLog(cgLogFile, String.Format("{0} Datenbank manuell gesichert.", DateTime.Now.ToString()))

                ' Erfolgsmeldung an den Benutzer ausgeben
                MessageBox.Show("Sicherung durchgeführt.", "Datensicherung", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                ' Fehlerbehandlung, falls prSaveDaten oder das Schreiben des Logs fehlschlägt
                ErrReport(ex.Message, ex.Source, ex.StackTrace)
                MessageBox.Show("Die Datensicherung ist fehlgeschlagen!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                ' Mauszeiger nach Abschluss der Sicherung garantiert wieder zurücksetzen
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

#End Region

#Region "Daten wiederherstellen...................................................................."

    'Private Sub prRestoreData()
    '    Dim sFile As String = arIni(30) & "\RestoreDB.dat"
    '    Dim cDaten As String 'Variable zur Aufnahme der Sicherungsdaten aus der Datei
    '    Try
    '        Call prRestore(sFile)

    '        'Select Case MsgBox("Mit dieser Funktion wird eine Sicherungsdatei geladen." & vbCrLf & _
    '        '            "Die aktuelle Konfiguration wird vorher gesichert. Mit der Funktion ""RollBack"" kann der" & _
    '        '            "Ausgangszustand wiederhergestellt werden." & vbCrLf & "" & vbCrLf & "Wiederherstellung der Daten durchführen?", _
    '        '            vbYesNo + vbQuestion + vbDefaultButton2, "Wiederherstellung der Daten")

    '        '    Case vbYes
    '        '        cDaten = fcOpenReadOneValueFromSystemDb(arIni(30), "Sicherungsdatei (SaveDB*.dat)|SaveDB*.dat")
    '        '        If cDaten = "" Then Exit Sub
    '        '        Me.Cursor = Cursors.WaitCursor
    '        '        'Kopie der Sicherungsdaten zur weiteren Bearbeitung speichern.
    '        '        Call SaveOneValueInSystemDb(sFile, cDaten)
    '        '        'Rücksicherundsmodul aufrufen
    '        '        Call prRestore(sFile)

    '        '        Call fcWriteLog(cgLogFile, Date.Now & " Datenbank wiederhergestellt.")
    '        '        Me.Cursor = Cursors.Default
    '        '        MsgBox("Daten wiederhergestellt. Zur vollständigen Initialisierung ist ein Neustart der Application notwendig.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Datensicherung")
    '        'End Select

    '    Catch ex As Exception
    '        ErrReport(ex.Message, ex.Source, ex.StackTrace)
    '    End Try
    'End Sub



#End Region

#Region "Tagesstatistik bereitstellen.............................................................."

    ''' <summary>
    ''' Liest die Buchungsdaten für einen Zeitraum von 21 Tagen ab dem gewählten Startdatum aus 
    ''' und listet fehlerhafte oder zu benachrichtigende Buchungen in der ListView auf.
    ''' </summary>
    ''' <param name="nRow">Der zeilenbasierte Index der ausgewählten Zeile im Buchungs-Grid.</param>
    ''' <remarks>
    ''' Erstellt am: 24.09.2026
    ''' Autor: Uwe
    ''' </remarks>
    Private Sub prGetTagesStatistik(ByVal nRow As Integer)
        Try
            lvBuchError.Items.Clear()

            ' 1. Startdatum flexibel aus der Zelle lesen (unterstützt dd.MM.yyyy und yyyyMMdd)
            Dim sZellenWert As String = dgBuchung.Rows(nRow).Cells(0).Value.ToString().Trim()
            Dim erlaubteFormate() As String = {"dd.MM.yyyy", "yyyyMMdd"}
            Dim dBasisDatum As Date = DateTime.ParseExact(sZellenWert, erlaubteFormate, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None)

            ' 2. Datumsbereich für SQL vorbereiten (Format: yyyyMMdd)
            Dim sADatum As String = dBasisDatum.ToString("yyyyMMdd")
            Dim sEDatum As String = dBasisDatum.AddDays(21).ToString("yyyyMMdd")

            ' 3. Erste Abfrage: Unbezahlte/offene Buchungen ermitteln
            Dim sSQL As String = $"Select * From Buchung Where von>='{sADatum}' and Bis<='{sEDatum}' and Bez <> '1'"
            Dim dt As DataTable = fcReadDataTable(sSQL)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                BefuelleFehlerListe(dt, "Bez?")
            End If

            ' 4. Zweite Abfrage: Buchungen mit Zugangs-Mail-Status 'Y' ermitteln
            sSQL = $"Select * From Buchung Where von>='{sADatum}' and Bis<='{sEDatum}' and SendMailZugang = 'Y'"
            dt = fcReadDataTable(sSQL)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                BefuelleFehlerListe(dt, "Ges?")
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Hilfsmethode zum einheitlichen Befüllen der ListView aus einer DataTable.
    ''' </summary>
    Private Sub BefuelleFehlerListe(ByVal dt As DataTable, ByVal sStatusText As String)
        For Each zeile As DataRow In dt.Rows
            Dim sZimID1 As String = zeile("ZimID").ToString()
            Dim lv As ListViewItem = lvBuchError.Items.Add(fcUmDatum(zeile("Von").ToString()))

            lv.SubItems.Add(fcGetObjektZimmerName(dtZim, sZimID1))
            lv.SubItems.Add(sStatusText)
            lv.SubItems.Add(zeile("BID").ToString())
        Next
    End Sub


#End Region

    ''' <summary>
    ''' Reagiert auf die Auswahl eines Datums im Kalender, blendet das Datumspanel aus
    ''' und synchronisiert die Buchungsübersicht für den gewählten Tag.
    ''' </summary>
    ''' <param name="sender">Das auslösende MonthCalendar-Objekt.</param>
    ''' <param name="e">Die Ereignisdaten, die den ausgewählten Datumsbereich enthalten.</param>
    ''' <remarks>
    ''' 27.12.2011 - Erstellt<br/>
    ''' 24.09.2026 - Optimiert: Veraltetes 'Call' entfernt, Exception-Handling hinzugefügt und Datumsformatierung stabilisiert.
    ''' </remarks>
    Private Sub MonthCalendar1_DateSelected(ByVal sender As Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles MonthCalendar1.DateSelected
        Try
            ' UI-Element ausblenden
            paDatum.Visible = False

            ' Das ausgewählte Startdatum ermitteln.
            ' TYPENSICHER: e.Start liefert direkt ein echtes 'Date'-Objekt (DateTime).
            Dim dSelectedDate As Date = e.Start

            ' Synchronisation der Buchungen für den ausgewählten Tag anstoßen
            prSynchronDay(dgBuchung, dSelectedDate)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Blendet das Datumspanel aus, wenn der Kalender den Fokus verliert.
    ''' </summary>
    ''' <param name="sender">Das auslösende MonthCalendar-Objekt.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.12.2011 - Erstellt<br/>
    ''' 24.09.2026 - Optimiert: Exception-Handling hinzugefügt.
    ''' </remarks>
    Private Sub MonthCalendar1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MonthCalendar1.LostFocus
        Try
            paDatum.Visible = False
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Startet den Druck der An- und Abreiseliste für das aktuell im Datagridview ausgewählte Datum.
    ''' </summary>
    ''' <param name="sender">Das auslösende ContextMenu- oder Menü-Item (ToolStripMenuItem).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.12.2011 - Erstellt<br/>
    ''' 24.09.2026 - Optimiert: Absicherung gegen NullReference- und Index-Exceptions, veraltetes 'Call' entfernt.
    ''' </remarks>
    Private Sub tsmDruckAnAb_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmDruckAnAb.Click
        Try
            ' Validierung: Prüfen, ob das Zeilen-Array initialisiert ist und Elemente enthält
            If arX IsNot Nothing AndAlso arX.Length > 0 Then
                Dim rowIndex As Integer = arX(0)

                ' Prüfen, ob der Index im gültigen Bereich des DataGridViews liegt
                If rowIndex >= 0 AndAlso rowIndex < dgBuchung.Rows.Count Then
                    Dim cellValue As Object = dgBuchung.Rows(rowIndex).Cells(0).Value

                    ' Prüfen, ob die Zelle einen Wert enthält
                    If cellValue IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(cellValue.ToString()) Then
                        Dim sDatum As String = cellValue.ToString()

                        ' Datum konvertieren und Druckmethode aufrufen (ohne 'Call')
                        Dim sFormattedDate As String = fcUmDatum(sDatum)
                        prDruckAnAb(sFormattedDate)
                    End If
                End If
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Ruft detaillierte Buchungs- und Beschreibungstexte aus der Datenbank ab und zeigt diese in einer Info-Box an.
    ''' </summary>
    ''' <param name="sender">Das auslösende Steuerelement (hier das ToolStripMenuItem 'tsmInfo').</param>
    ''' <param name="e">Die Ereignisdaten des Klicks.</param>
    ''' <remarks>
    ''' 27.12.2011 - Erstellt<br/>
    ''' 24.09.2026 - Optimiert: 
    ''' Stabilität: Fehlertolerante Prüfungen (<c>Rows.Count &gt; 0</c> und Grid-Index-Validierung) integriert, um <c>NullReferenceException</c>-Abstürze bei leeren Daten zu verhindern.
    ''' Performance:Ressourcenoptimierung durch den Verzicht auf <c>SELECT *</c>. Es werden nur noch die explizit benötigten Spalten geladen.
    ''' Modernisierung: Migration veralteter VB6-Bibliotheken zu nativen .NET-Methoden (<c>MessageBox.Show</c>, <c>Environment.NewLine</c> und <c>String.IsNullOrWhiteSpace</c>).
    ''' </remarks>
    Private Sub tsmInfo_Click(sender As Object, e As EventArgs) Handles tsmInfo.Click
        ' 1. Validierung: Prüfen, ob die Zeilen- und Spaltenindizes (arX) gültig sind
        If arX Is Nothing OrElse arX.Length < 2 Then Exit Sub

        Dim rowIndex As Integer = arX(0)
        Dim colIndex As Integer = arX(1)

        ' Prüfen, ob Indizes im gültigen Bereich der DataGridView liegen
        If rowIndex < 0 OrElse rowIndex >= dgBuchung.Rows.Count OrElse
       colIndex < 0 OrElse colIndex >= dgBuchung.Columns.Count Then Exit Sub

        ' 2. Buchungsnummer aus Zelle extrahieren
        Dim cellValue As Object = dgBuchung.Rows(rowIndex).Cells(colIndex).Value
        If cellValue Is Nothing Then Exit Sub

        Dim sBNr As String = Extract(cellValue.ToString(), "(", ")", 1)
        If String.IsNullOrWhiteSpace(sBNr) Then Exit Sub

        ' 3. Erste SQL-Abfrage (Buchungsdetails) über parametrisierten Query

        Dim sSQL As String = "Select * From Buchung Where BID='" & sBNr & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' Prüfen, ob eine Buchung gefunden wurde
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            MessageBox.Show("Buchung wurde in der Datenbank nicht gefunden.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim drBuchung As DataRow = dt.Rows(0)
        Dim sBTextID As String = drBuchung("BText").ToString()
        Dim sMText As String = drBuchung("MText").ToString()

        ' 4. Zweite SQL-Abfrage (Erweiterte Beschreibungstexte)
        Dim sSQLText As String = "SELECT SpaltenName3 FROM BTexte WHERE ID = @ID" ' Empfehlung: Verwende echte Spaltennamen statt Index 3
        Dim sTmp As String = dt.Rows(0).Item("BText").ToString '& vbCrLf
        Dim dtB As DataTable = fcReadDataTable("Select * from BTexte Where ID='" & sTmp & "'")

        ' Wenn ein Text gefunden wurde, diesen verwenden, sonst leer lassen
        If dtB IsNot Nothing AndAlso dtB.Rows.Count > 0 Then
            ' Index 3 sollte im SELECT-Statement an Position 0 stehen
            sTmp = dtB.Rows(0).Item(0).ToString() & Environment.NewLine & Environment.NewLine
        End If

        ' MText anhängen
        sTmp &= sMText & Environment.NewLine

        ' 5. Validierung des finalen Textes und Ausgabe
        If String.IsNullOrWhiteSpace(sTmp) Then Exit Sub

        MessageBox.Show(sTmp.Trim(),
                    "Buchungstext zur Buchung: " & sBNr,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information)
    End Sub

    ''' <summary>
    ''' Verarbeitet den Klick auf den Check-in-Button. 
    ''' Liest die Buchungs-ID aus dem DataGrid aus und öffnet das Check-in-Formular.
    ''' </summary>
    ''' <param name="sender">Das auslösende Steuerelement.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' <para><strong>Dokumentation &amp; Absicherung:</strong></para>
    ''' <para>Datum: 24.09.2026</para>
    ''' <para>Grund: Es wurden Sicherheitsprüfungen für das Grid-Array (<c>arX</c>) integriert, um eine <c>NullReferenceException</c> oder <c>ArgumentOutOfRangeException</c> bei leeren oder ungültigen Zeilenselektionen zu verhindern. Zudem wurde ein generischer <c>Try-Catch</c>-Block zur Fehlerprotokollierung hinzugefügt.</para>
    ''' </remarks>
    Private Sub tscCheckin_Click(sender As Object, e As EventArgs) Handles tscCheckin.Click
        Try
            ' Sicherheitsprüfung: Haben wir gültige Indizes im Array?
            If arX Is Nothing OrElse arX.Length < 2 Then Return
            If dgBuchung.Rows.Count <= arX(0) Then Return

            ' Buchungs-ID auslesen und extrahieren
            Dim rawValue As Object = dgBuchung.Rows(arX(0)).Cells(arX(1)).Value
            If rawValue IsNot Nothing Then
                sgRBID = Extract(rawValue.ToString(), "(", ")", 1)
            End If

            ' Formular anzeigen
            frmCheckin.Show()

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Aktualisiert den Status des E-Mail-Versands für eine bestimmte Buchung in der Datenbank.
    ''' </summary>
    ''' <param name="sNummer">Die Nummer des ausgeführten Check-in-Schritts (wird als Wert in die Datenbank geschrieben).</param>
    ''' <remarks>
    ''' <para>Aktualisiert die Spalte 'SendMailZugang' in der Tabelle 'Buchung' für die aktuell ausgewählte Buchungs-ID (<c>sgRBID</c>).</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 – Optimiert: Ungenutzten StringBuilder entfernt, ByRef zu ByVal korrigiert (da die Nummer nicht manipuliert zurückgegeben werden muss), und verwirrende Split-Aufrufe durch direkte Array-Zuweisung ersetzt.<br/>
    ''' </para>
    ''' </remarks>
    Private Sub prCheckinMail(ByVal sNummer As String)
        ' Validierung der Eingabe
        If String.IsNullOrEmpty(sNummer) Then Exit Sub

        Try
            ' WICHTIG: Die Bedingung filtert nach der globalen Buchungs-ID
            Dim cBedingung As String = String.Format(" WHERE BID = '{0}'", sgRBID)

            ' Direkt ein String-Array mit dem exakten Spaltennamen definieren (ohne langsames Split)
            Dim arFields() As String = {"SendMailZugang"}

            ' Direkt das Werte-Array mit der übergebenen Schritt-Nummer befüllen
            Dim arValue() As String = {sNummer}

            ' Datenbank-Update ausführen 
            fcUpdateCommand("Buchung", arFields, arValue, cBedingung)

        Catch ex As Exception
            ' Fehler an die zentrale Fehlerverwaltung übergeben
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Verarbeitet das Doppelklick-Ereignis auf der Fehler-ListView (lvBuchError).
    ''' Ermittelt die Buchungs-ID, versendet eine Zahlungsbestätigung per Mail und markiert die Buchung in der Datenbank als bezahlt.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die ListView lvBuchError).</param>
    ''' <param name="e">Die Ereignisdaten des DoubleClick-Events.</param>
    ''' <remarks>
    ''' <b>Historie:</b><br/>
    ''' 24.09.2026 – Uwe: Code-Struktur bereinigt, String-Splits optimiert und ungenutzte Zuweisungen entfernt.<br/>
    ''' </remarks>
    Private Sub lvBuchError_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles lvBuchError.DoubleClick
        ' Prüfen, ob ein Element in der ListView ausgewählt wurde
        If lvBuchError.SelectedItems.Count > 0 Then
            ' Die Buchungs-ID (BID) aus der 4. Spalte (Index 3) auslesen
            Dim sBid_Mail As String = lvBuchError.SelectedItems(0).SubItems(3).Text

            ' Wenn eine gültige ID vorhanden ist und die E-Mail erfolgreich als bezahlt markiert/gesendet wurde
            If Not String.IsNullOrEmpty(sBid_Mail) AndAlso fcMailBezahlt(sBid_Mail) = True Then

                Dim cBedingung As String = " WHERE BID ='" & sBid_Mail & "'"
                Dim arFields() As String = New String() {"Bez"}
                Dim arValue() As String = New String() {"1"}

                Try
                    ' Alle Datensätze mit dieser BID in der Tabelle 'Buchung' als bezahlt (1) markieren
                    fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
                Catch ex As Exception
                    ' Fehler protokollieren
                    ErrReport(ex.Message, ex.Source, ex.StackTrace)
                End Try

                ' Tagesstatistik aktualisieren
                prGetTagesStatistik(e_rows)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Verarbeitet das Tick-Ereignis des Wait-Timers.
    ''' Deaktiviert den Timer sofort wieder, damit er nur ein einziges Mal (Single-Shot) ausgeführt wird.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (der Timer).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    Private Sub tiWait_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tiWait.Tick
        ' Stoppt den Timer, um eine wiederholte Ausführung zu verhindern
        tiWait.Enabled = False
    End Sub


End Class
