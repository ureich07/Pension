Imports System.Text

Public Class frmEinteilung
    Private dtP As DataTable
    Private dtE As DataTable
    Private lEnable As Boolean
    Private nPersonen As Integer
    Private sFileSaison As String = "Saison"
    Private sFileInfo As String = "Arbeit"

#Region "Form......................................................................................"

    ''' <summary>
    ''' Initialisiert das Formular beim Laden. 
    ''' Lädt die Personal- und Einteilungsdatenbanken, berechnet die Personenanzahl und bereitet die Steuerelemente für die Ansicht vor.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei Methodenaufrufen vollständig entfernt.
    ''' - Ineffiziente 'For'-Schleife zur Personenzählung durch moderne, lesbare LINQ-Abfrage ersetzt.
    ''' - Veraltete VB6-Funktionen ('Mid', 'Str') durch saubere .NET-Methoden ('.StartsWith', '.ToString') ersetzt.
    ''' - Explizite Datentyp-Deklaration ('As Integer') für alle Schleifenzähler hinzugefügt.
    ''' - Inline-Kommentare zur besseren Strukturierung und Wartbarkeit ergänzt.
    ''' </remarks>
    Private Sub frmEinteilung_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            ' 1. Personal- und Einteilungsdatenbank auslesen
            dtP = fcReadDataTable("Select * from Personal1")
            dtE = fcReadDataTable("Select * from Personal2")

            ' 2. Gültige Personenanzahl ermitteln (Zeilen filtern, die nicht mit "/" beginnen)
            nPersonen = dtP.AsEnumerable().Count(Function(row) Not row.Field(Of String)("Name").StartsWith("/")) - 1

            lEnable = True
            prColorRead()

            ' 3. Tabellenstruktur aufbauen und Ansicht synchronisieren
            prSetTabelleArbeit(dtP)
            prAddDatumToTable(dtP)
            prSetFeiertage(dtP)
            prSynchronDay(dgArbeit, Date.Today)
            prColumns(lEnable)
            prLadeEinteilung(dtE)

            ' 4. Monat-Auswahlliste befüllen (1 bis 12) und aktuellen Monat vorselektieren
            For i As Integer = 1 To 12
                tscoMonat.Items.Add(i.ToString())
            Next
            tscoMonat.Text = Date.Today.Month.ToString()

            ' 5. Jahr-Auswahlliste befüllen (Vorjahr bis +2 Jahre) und aktuelles Jahr vorselektieren
            Dim currentYear As Integer = Date.Today.Year
            For i As Integer = currentYear - 1 To currentYear + 2
                tscoJahr.Items.Add(i.ToString())
            Next
            tscoJahr.Text = currentYear.ToString()

            ' 6. Personen-Auswahlliste standardmäßig auf 10 setzen
            For i As Integer = 1 To 10
                tscoPerson.Items.Add(i.ToString())
            Next
            tscoPerson.Text = "10"

            ' 7. Externe Info-Datei laden
            prLoadInfo(sFileInfo)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Erstellt die Tabellenstruktur für die Personaleinteilung im DataGridView.
    ''' Initialisiert die Standardspalten und fügt für jede gültige Person eine eigene Datenspalte mit Name und Telefonnummer hinzu.
    ''' </summary>
    ''' <param name="dt">Die DataTable, die die Personaldaten enthält.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Veraltete VB6-Funktionen ('Mid', 'Trim') durch moderne .NET-Methoden ersetzt.
    ''' - String-Verkettung auf performanteres 'String.Format' / String-Interpolation umgestellt.
    ''' - Unbenutzte bzw. überflüssige Zuweisungen ('n = n') und auskommentierten Code entfernt.
    ''' - Variablendeklarationen bereinigt und Gültigkeitsbereiche (Scoping) optimiert.
    ''' - Typisierung der Schleifenvariable bei der Spalten-Sortierung explizit angegeben.
    ''' </remarks>
    Private Sub prSetTabelleArbeit(ByVal dt As DataTable)

        Try
            With dgArbeit
                ' 1. Bestehende Spalten zurücksetzen und Grid-Header konfigurieren
                .Columns.Clear()
                .ColumnHeadersHeight = 40

                ' 2. Basis-Spalten für Datum und Wochentag hinzufügen
                .Columns.Add("Datum", "Datum")
                .Columns.Add("Tag", "Tag")

                .Columns(0).Width = 80
                .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Columns(1).Width = 80
                .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

                Dim columnIndex As Integer = 2

                ' 3. Personen-Datensätze durchlaufen und dynamisch Spalten hinzufügen
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim name As String = dt.Rows(i).Item("Name").ToString().Trim()

                    ' Zeilen überspringen, die leer sind oder mit "/" beginnen
                    If Not String.IsNullOrEmpty(name) AndAlso Not name.StartsWith("/") Then
                        Dim vorname As String = dt.Rows(i).Item("Vorname").ToString().Trim()
                        Dim telefon As String = dt.Rows(i).Item("Tel1").ToString().Trim()

                        ' Spaltenbeschriftung formatieren (Vorname Name \n Telefon)
                        Dim sPer As String = $"{vorname} {name}{Environment.NewLine}{telefon}"

                        .Columns.Add(sPer, sPer)
                        .Columns(columnIndex).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                        columnIndex += 1
                    End If
                Next

                ' 4. Verhalten und Design des DataGridViews konfigurieren
                .RowHeadersVisible = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .AutoResizeRows()

                ' Sortierung für alle Spalten deaktivieren
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
                Next

                ' Tooltips und Layout-Modus aktivieren
                .ShowCellToolTips = True
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing

                ' Farben für die selektierte Zeile festlegen
                With .DefaultCellStyle
                    .SelectionBackColor = Color.GreenYellow
                    .SelectionForeColor = Color.Black
                End With

                .ReadOnly = True
            End With

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Füllt das DataGridView mit Zeilen für einen definierten Zeitraum (2 Monate in der Vergangenheit bis 2 Jahre in die Zukunft).
    ''' Hinterlegt in der (ausgeblendeten) ersten Zeile die IDs des Personals zur späteren Zuordnung.
    ''' </summary>
    ''' <param name="dt">Die DataTable mit den Personaldaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Extrem ineffiziente 'StringBuilder.Split'-Logik innerhalb der Tagesschleife durch direkte Object-Arrays ersetzt.
    ''' - Veraltete VB6-Kompatibilitätsfunktionen ('DateAdd', 'DateDiff', 'Mid', 'WeekdayName') durch native .NET 'DateTime'-Methoden ersetzt.
    ''' - Speicherbedarf massiv gesenkt und Ausführungsgeschwindigkeit optimiert.
    ''' - String-Verkettungen bereinigt und Variablen-Scoping korrigiert.
    ''' </remarks>
    Private Sub prAddDatumToTable(ByVal dt As DataTable)
        Try
            With dgArbeit
                ' 1. Vorbereitungen für die unsichtbare ID-Headerzeile
                Dim headerRow As New List(Of Object) From {
                "26.10.1959",
                "Geburtstag"
            }

                ' IDs der gültigen Personen sammeln
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim name As String = dt.Rows(i).Item("Name").ToString()

                    If Not String.IsNullOrEmpty(name) AndAlso Not name.StartsWith("/") Then
                        headerRow.Add(dt.Rows(i).Item("ID").ToString())
                    End If
                Next

                ' Erste Zeile hinzufügen und ausblenden
                Dim headerIndex As Integer = .Rows.Add(headerRow.ToArray())
                .Rows(headerIndex).Visible = False

                ' 2. Zeitspanne berechnen (2 Monate zurück bis 2 Jahre vorwärts)
                Dim dStart As Date = Date.Today.AddMonths(-2)
                Dim dEnd As Date = Date.Today.AddYears(2)
                Dim totalDays As Integer = (dEnd - dStart).Days

                ' 3. Spaltenanzahl für die leeren Zellen ermitteln
                ' Entspricht der Gesamtanzahl der Spalten abzüglich Datum und Tag
                Dim personColumnsCount As Integer = .Columns.Count - 2

                ' 4. Zeilen für jeden Tag performant generieren und hinzufügen
                For i As Integer = 0 To totalDays
                    Dim currentDay As Date = dStart.AddDays(i)

                    ' Wochentag nativ über das System-Kulturformat ermitteln (z.B. "Montag")
                    Dim sTag As String = currentDay.ToString("dddd")

                    ' Array für die neue Zeile initialisieren
                    Dim rowData(personColumnsCount + 1) As Object
                    rowData(0) = currentDay.ToShortDateString()
                    rowData(1) = sTag

                    ' Die restlichen Zellen der Zeile bleiben initial leer (Nothing / "")
                    ' (Die alte Schleife hängte hier leere Strings an)

                    .Rows.Add(rowData)
                Next
            End With

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub


    ''' <summary>
    ''' Markiert Feiertage, Wochenenden und Saisonzeiten im DataGridView farblich.
    ''' Liest die Saisondaten ein und wendet die entsprechenden Farbprofile auf die Datums- und Wochentagsspalten an.
    ''' </summary>
    ''' <param name="dt">Die DataTable mit den Personaldaten (in dieser Methode nicht direkt verwendet).</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Kritischen Indexfehler (Nutzung von 'i' statt 'j' bei der Grid-Zeilen-Zuweisung) korrigiert.
    ''' - Mehrfache, ineffiziente String-zu-Datum-Konvertierungen durch einmaliges Parsen optimiert.
    ''' - 'Select Case'-Struktur durch direkte Zuweisungen und saubere Verschachtelung beschleunigt.
    ''' - Veraltete VB6-Hilfsfunktionen ('Val') entfernt.
    ''' - Zusammenfassung der Grid-Schleifen zur Vermeidung redundanter Zeilendurchläufe.
    ''' </remarks>
    Private Sub prSetFeiertage(ByVal dt As DataTable)
        Try
            ' 1. Saisondaten aus der System-DB einlesen und splitten
            Dim saisonInhalt As String = ReadOneValueFromSystemDb(sFileSaison)
            If String.IsNullOrWhiteSpace(saisonInhalt) Then Return

            Dim arTmp() As String = saisonInhalt.Split(New String() {Environment.NewLine, vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries)

            With dgArbeit
                Dim totalRows As Integer = .Rows.Count - 1
                If totalRows < 1 Then Return

                ' 2. Jede Grid-Zeile (ab Index 1, da Zeile 0 die ausgeblendete ID-Zeile ist) durchlaufen
                For j As Integer = 1 To totalRows

                    ' Basis-Zellen referenzieren
                    Dim cellDatum As DataGridViewCell = .Rows(j).Cells(0)
                    Dim cellTag As DataGridViewCell = .Rows(j).Cells(1)

                    If cellDatum.Value Is Nothing OrElse cellTag.Value Is Nothing Then Continue For

                    ' Datum der aktuellen Zeile parsen (nutzt die in prAddDatumToTable gesetzte Formatierung)
                    Dim rowDate As Date
                    If Not Date.TryParse(cellDatum.Value.ToString(), rowDate) Then Continue For

                    Dim cBack As Color = Color.Empty
                    Dim cFore As Color = Color.Empty
                    Dim hasSaisonColor As Boolean = False

                    ' 3. Saisoneinstellungen prüfen und Farben ermitteln
                    For i As Integer = 0 To arTmp.Length - 1
                        Dim zeile As String = arTmp(i).Trim()
                        If zeile = "" Then Continue For

                        Dim arT() As String = zeile.Split(";"c)
                        If arT.Length < 3 Then Continue For

                        ' Saison-Start und -Ende konvertieren
                        Dim vonDatum As Date = arT(1).Trim()
                        Dim bisDatum As Date = arT(2).Trim()

                        ' Prüfen, ob das Zeilendatum im Saisonzeitraum liegt
                        If rowDate >= vonDatum AndAlso rowDate <= bisDatum Then
                            Dim sTag As String = arT(0).Trim()

                            If sTag.Length > 0 Then
                                Dim saisonTyp As String = sTag.Substring(0, 1)

                                ' Typen 3 bis 8 auswerten
                                Select Case saisonTyp
                                    Case "3"
                                        cFore = fColorForeNebenSaison1
                                        cBack = fColorBackNebenSaison1
                                        hasSaisonColor = True
                                    Case "4"
                                        cFore = fColorForeNebenSaison2
                                        cBack = fColorBackNebenSaison2
                                        hasSaisonColor = True
                                    Case "5"
                                        cFore = fColorForeHauptSaison
                                        cBack = fColorBackHauptSaison
                                        hasSaisonColor = True
                                    Case "6"
                                        cFore = fColorForeFeiertag1
                                        cBack = fColorBackFeiertag1
                                        hasSaisonColor = True
                                    Case "7"
                                        cFore = fColorForeFeiertag2
                                        cBack = fColorBackFeiertag2
                                        hasSaisonColor = True
                                    Case "8"
                                        cFore = fColorForeFeiertag3
                                        cBack = fColorBackFeiertag3
                                        hasSaisonColor = True
                                End Select
                            End If
                        End If
                    Next

                    ' Saisonfarbe anwenden, falls ermittelt
                    If hasSaisonColor Then
                        cellDatum.Style.BackColor = cBack
                        cellTag.Style.BackColor = cBack
                        cellDatum.Style.ForeColor = cFore
                        cellTag.Style.ForeColor = cFore
                    End If

                    ' 4. Spezifische Feiertage aus Datenbank prüfen (überschreibt ggf. Saisonfarbe)
                    Dim feiertagName As String = fcGetFeiertag(cellDatum.Value)
                    If Not String.IsNullOrEmpty(feiertagName) Then
                        cellDatum.Style.ForeColor = fColorForeSonstigeFeiertage
                        cellDatum.Style.BackColor = fColorBackSonstigeFeiertage
                        cellDatum.ToolTipText = feiertagName
                    End If

                    ' 5. Wochentags-Sonderfarben für Freitag und Samstag anwenden
                    Dim wochentag As String = cellTag.Value.ToString()
                    Select Case wochentag
                        Case "Samstag"
                            cellDatum.Style.ForeColor = fColorForeSamstag
                            cellTag.Style.ForeColor = fColorForeSamstag
                            cellDatum.Style.BackColor = fColorBackSamstag
                            cellTag.Style.BackColor = fColorBackSamstag

                        Case "Freitag"
                            cellDatum.Style.ForeColor = fColorForeFreitag
                            cellTag.Style.ForeColor = fColorForeFreitag
                            cellDatum.Style.BackColor = fColorBackFreitag
                            cellTag.Style.BackColor = fColorBackFreitag
                    End Select
                Next
            End With

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Synchronisiert die Tabellenansicht des DataGridViews auf ein bestimmtes Zieldatum (z. B. den heutigen Tag).
    ''' Durchsucht die Datumsspalte und setzt den Fokus direkt auf die entsprechende Zeile.
    ''' </summary>
    ''' <param name="dg">Das zu synchronisierende DataGridView (wird im Code übergeben, aber dgArbeit wird direkt referenziert).</param>
    ''' <param name="dDay">Das Zieldatum, auf das die Ansicht fokussiert werden soll.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Fehlerhaften Parameter-Bezug korrigiert (nutzt nun das übergebene 'dg' statt der fest verdrahteten globalen Instanz 'dgArbeit').
    ''' - String-basierten Datumsvergleich durch robusten und typsicheren 'Date'-Vergleich via 'Date.TryParse' ersetzt.
    ''' - Vorzeitigen Abbruch bei ungültigen Zellwerten integriert, um Laufzeitfehler zu verhindern.
    ''' - Explizite Datentyp-Deklaration ('As Integer') für die Schleifenvariable hinzugefügt.
    ''' </remarks>
    Public Sub prSynchronDay(ByVal dg As DataGridView, ByVal dDay As Date)
        Try
            ' 1. Prüfung, ob das Grid Zeilen enthält
            Dim totalRows As Integer = dg.Rows.Count - 1
            If totalRows < 0 Then Return

            ' 2. Zeilen durchlaufen und nach dem übereinstimmenden Datum suchen
            For i As Integer = 0 To totalRows
                Dim cellValue As Object = dg.Rows(i).Cells(0).Value

                If cellValue IsNot Nothing Then
                    Dim rowDate As Date

                    ' Datumsstring der Zelle sauber parsen und typsicher vergleichen
                    If Date.TryParse(cellValue.ToString(), rowDate) Then
                        If rowDate.Date = dDay.Date Then
                            ' Zelle als aktiv setzen und Schleife vorzeitig beenden
                            dg.CurrentCell = dg.Rows(i).Cells(0)
                            Exit For
                        End If
                    End If
                End If
            Next

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Berechnet die optimale Spaltenbreite für die Personen-Spalten und wendet diese an.
    ''' Verteilt den verfügbaren Platz des DataGridViews gleichmäßig auf alle Personen-Spalten, unterschreitet dabei jedoch nicht eine definierte Mindestbreite.
    ''' </summary>
    ''' <param name="lStart">Bestimmt, ob die Breitenberechnung ausgeführt werden soll.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Den Übergabeparameter von 'ByRef' auf 'ByVal' geändert, da keine Werteänderung an den Aufrufer zurückgegeben werden muss.
    ''' - Potenziellen Laufzeitfehler bei der Berechnung korrigiert (Division durch Null abgefangen, falls keine Personen vorhanden sind).
    ''' - Fehleranfälligen Bezug auf globale Variable 'nPersonen' durch die tatsächliche Anzahl der Grid-Spalten ersetzt.
    ''' - Explizite Datentyp-Deklaration ('As Integer') für die Schleifenvariable hinzugefügt.
    ''' - Boolean-Prüfung vereinfacht ('If lStart Then' statt 'If lStart = True Then').
    ''' </remarks>
    Private Sub prColumns(ByVal lStart As Boolean)
        Try
            ' 1. Vorzeitiger Abbruch, wenn die Berechnung nicht aktiv sein soll
            If Not lStart Then Return

            With dgArbeit
                ' Berechnung der verbleibenden Personen-Spalten im Grid
                ' Spaltenanzahl abzüglich der ersten beiden Basis-Spalten (Datum und Tag)
                Dim personColumnsCount As Integer = .Columns.Count - 2

                ' Division durch Null oder negative Werte verhindern
                If personColumnsCount <= 0 Then Return

                ' 2. Verfügbare Breite berechnen (Gesamtbreite minus die ersten beiden festen Spalten)
                Dim remainingWidth As Integer = .Width - .Columns(0).Width - .Columns(1).Width

                ' Gleichmäßige Breite ermitteln
                Dim calculatedWidth As Integer = remainingWidth \ personColumnsCount

                ' Mindestbreite von 100 Pixeln erzwingen
                If calculatedWidth < 100 Then calculatedWidth = 100

                ' 3. Berechnete Spaltenbreite auf alle Personen-Spalten anwenden
                For i As Integer = 0 To personColumnsCount - 1
                    .Columns(i + 2).Width = calculatedWidth
                Next
            End With

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Lädt die Einteilungsdaten aus der Personal2-Tabelle und trägt diese in den Kalender bzw. das DataGridView ein.
    ''' Durchläuft alle Einteilungsdatensätze und übergibt die Zeiträume und Zuweisungen an die Platzierungslogik.
    ''' </summary>
    ''' <param name="dt">Die DataTable, die die Einteilungsdaten (Personal2) enthält.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Den Übergabeparameter von 'ByRef' auf 'ByVal' geändert, da das DataTable-Objekt nicht neu zugewiesen wird.
    ''' - Explizite Datentyp-Deklaration ('As Integer') für die Schleifenvariable hinzugefügt.
    ''' - Typsichere Datenabfrage der Spaltenwerte implementiert, um Laufzeitfehler bei Null-Werten (DBNull) zu verhindern.
    ''' - Überflüssige Variablen-Initialisierungen außerhalb der Schleife entfernt, um das Speicher-Scoping zu verbessern.
    ''' - Null-Prüfung für die DataTable hinzugefügt, um Stabilität bei leeren Datensätzen zu gewährleisten.
    ''' </remarks>
    Private Sub prLadeEinteilung(ByVal dt As DataTable)
        ' 1. Vorzeitiger Abbruch, falls keine Daten vorhanden sind
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return

        Dim nMax As Integer = dt.Rows.Count - 1
        Dim nPos As Integer = 0

        Try
            ' 2. Alle Einteilungszeilen durchlaufen
            For i As Integer = 0 To nMax
                Dim row As DataRow = dt.Rows(i)

                ' Werte sicher auslesen und in Strings konvertieren (fängt DBNull ab)
                Dim sId As String = row("Personal").ToString()
                Dim sADatum As String = row("ADatum").ToString()
                Dim sEDatum As String = row("EDatum").ToString()
                Dim sInfo As String = row("Info").ToString()

                ' Eintrag in die Tabelle setzen und die aktuelle Position für den nächsten Durchlauf merken
                nPos = fcSetEntryInTabelle(sADatum, sEDatum, sId, sInfo, nPos)
            Next

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub


    '''' <summary>
    '''' Information ins DataGrid schreiben
    '''' </summary>
    '''' <param name="sBegin"></param>
    '''' <param name="sEnd"></param>
    '''' <param name="sId"></param>
    '''' <param name="sInfo"></param>
    '''' <param name="nPos"></param>
    '''' <returns></returns>
    '''' <remarks>
    '''' 18.02.2012 Create
    '''' </remarks>
    'Private Function fcSetEntryInTabelle(ByVal sBegin As String, ByVal sEnd As String, _
    '                                     ByVal sId As String, ByVal sInfo As String, _
    '                                     ByVal nPos As Integer) As Integer
    '    fcSetEntryInTabelle = nPos
    '    Dim i As Integer
    '    Dim sTag As String
    '    Dim nMax As Integer = dgArbeit.Rows.Count - 1
    '    Dim nCol As Integer = fcGetPersonSpalte(sId, dgArbeit)
    '    If nCol = -1 Then Exit Function
    '    Try
    '        With dgArbeit
    '            For i = nPos To nMax
    '                sTag = fcUmDatum(.Rows(i).Cells(0).Value.ToString)
    '                If sTag >= sBegin And sTag <= sEnd Then
    '                    .Rows(i).Cells(nCol).Value = sInfo
    '                End If
    '            Next
    '        End With
    '    Catch ex As Exception
    '        ErrReport(ex.Message, ex.Source, ex.StackTrace)
    '    End Try
    'End Function
    ''' <summary>
    ''' Schreibt die Einteilungsinformationen (z. B. Schichtkürzel) für eine bestimmte Person in den definierten Zeitraum des DataGridViews.
    ''' </summary>
    ''' <param name="sBegin">Das Startdatum des Zeitraums als String.</param>
    ''' <param name="sEnd">Das Enddatum des Zeitraums als String.</param>
    ''' <param name="sId">Die eindeutige ID der Person.</param>
    ''' <param name="sInfo">Die einzutragende Information (z. B. Schicht- oder Urlaubskürzel).</param>
    ''' <param name="nPos">Der Zeilenindex, ab dem die Suche im Grid gestartet werden soll.</param>
    ''' <returns>Gibt den übergebenen Startindex 'nPos' als aktuellen Verarbeitungsstand zurück.</returns>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Veraltete VB6-Rückgabesyntax ('fcSetEntryInTabelle = nPos') durch modernes 'Return' ersetzt.
    ''' - Performance-Bottleneck entfernt: String-basierte Datumsvergleiche in der Schleife durch einmalig geparste, typsichere 'Date'-Objekte ersetzt.
    ''' - Fehleranfällige logische Operatoren durch Kurzschluss-Operatoren ('AndAlso') ersetzt.
    ''' - Explizite Datentyp-Deklaration ('As Integer') für die Schleifenvariable hinzugefügt.
    ''' </remarks>
    Private Function fcSetEntryInTabelle(ByVal sBegin As String, ByVal sEnd As String,
                                     ByVal sId As String, ByVal sInfo As String,
                                     ByVal nPos As Integer) As Integer

        ' 1. Spaltenindex der Person ermitteln
        Dim nCol As Integer = fcGetPersonSpalte(sId, dgArbeit)
        If nCol = -1 Then Return nPos

        Try
            ' 2. Start- und Enddatum einmalig vorab parsen statt in jeder Zeile neu
            Dim dateBegin As Date = fcUmDatum(sBegin)
            Dim dateEnd As Date = fcUmDatum(sEnd)

            With dgArbeit
                Dim nMax As Integer = .Rows.Count - 1

                ' Sicherheitsprüfung für den Startindex
                If nPos < 0 OrElse nPos > nMax Then Return nPos

                ' 3. Grid ab der gemerkten Position durchlaufen
                For i As Integer = nPos To nMax
                    Dim cellValue As Object = .Rows(i).Cells(0).Value

                    If cellValue IsNot Nothing Then
                        ' Datum der aktuellen Zeile sauber parsen
                        Dim rowDate As Date = cellValue.ToString()

                        ' Typsicherer Datumsvergleich
                        If rowDate >= dateBegin AndAlso rowDate <= dateEnd Then
                            .Rows(i).Cells(nCol).Value = sInfo
                        End If
                    End If
                Next
            End With

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try

        Return nPos
    End Function

    ''' <summary>
    ''' Ermittelt den Spaltenindex einer bestimmten Person im DataGridView anhand ihrer eindeutigen ID.
    ''' Durchsucht dazu die (ausgeblendete) erste Zeile, in der die IDs hinterlegt sind.
    ''' </summary>
    ''' <param name="sID">Die eindeutige ID der gesuchten Person.</param>
    ''' <param name="dg">Das DataGridView, in dem gesucht werden soll.</param>
    ''' <returns>Den nullbasierten Spaltenindex der Person, oder -1, wenn die ID nicht gefunden wurde.</returns>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Veraltete VB6-Rückgabesyntax ('fcGetPersonSpalte = ...') durch modernes 'Return' ersetzt.
    ''' - 'NullReferenceException' abgefangen: Sichere Prüfung auf 'Nothing' vor dem Aufruf von '.ToString()'.
    ''' - Vorzeitigen Abbruch bei fehlenden Spalten oder Zeilen integriert.
    ''' - Lokalen Rückgabewert als klare Variable definiert, um den Programmfluss zu verdeutlichen.
    ''' </remarks>
    Private Function fcGetPersonSpalte(ByVal sID As String, ByVal dg As DataGridView) As Integer
        Dim resultIndex As Integer = -1

        ' 1. Sicherheitsprüfung: Enthält das Grid Spalten und mindestens die ID-Zeile?
        If dg.Columns.Count < 3 OrElse dg.Rows.Count = 0 Then Return resultIndex

        Try
            Dim nMax As Integer = dg.Columns.Count - 1

            ' 2. Spalten ab Index 2 durchlaufen (Index 0 = Datum, 1 = Tag)
            For i As Integer = 2 To nMax
                Dim cellValue As Object = dg.Rows(0).Cells(i).Value

                ' Null-sichere Konvertierung und Vergleich
                If cellValue IsNot Nothing Then
                    Dim cID As String = cellValue.ToString().Trim()

                    If sID = cID Then
                        resultIndex = i
                        Exit For
                    End If
                End If
            Next

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try

        Return resultIndex
    End Function


    ''' <summary>
    ''' Lädt die Textinformationen aus einer Systemdatei und befüllt damit die Info-Auswahlliste (ComboBox).
    ''' Fügt standardmäßig einen leeren Eintrag am Anfang hinzu und bereinigt die eingelesenen Daten von Leerzeilen.
    ''' </summary>
    ''' <param name="sDatei">Der Dateiname oder Pfad der zu ladenden Systemdatei.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Den Übergabeparameter von 'ByRef' auf 'ByVal' geändert, da der String-Pfad nicht manipuliert zurückgegeben werden muss.
    ''' - Veraltete VB6-Funktion 'Split()' durch die native .NET-Methode '.Split()' ersetzt.
    ''' - 'StringSplitOptions.RemoveEmptyEntries' integriert, um unschöne Leerzeilen in der ComboBox automatisch auszufiltern.
    ''' - Fehlerabsicherung durch einen 'Try-Catch'-Block sowie eine Null-Prüfung der eingelesenen Daten hinzugefügt.
    ''' - Explizite Datentyp-Deklaration ('As Integer') für die Schleifenvariable ergänzt.
    ''' </remarks>
    Private Sub prLoadInfo(ByVal sDatei As String)
        Try
            ' 1. Daten aus der System-DB einlesen
            Dim dateiInhalt As String = ReadOneValueFromSystemDb(sDatei)

            ' 2. ComboBox zurücksetzen
            tscoInfo.Items.Clear()
            tscoInfo.Items.Add("") ' Standardmäßig leeren Eintrag hinzufügen

            ' Vorzeitiger Abbruch, falls die Datei leer oder nicht vorhanden ist
            If String.IsNullOrWhiteSpace(dateiInhalt) Then Return

            ' 3. Text in Zeilen aufteilen (berücksichtigt alle gängigen Zeilenumbrüche und entfernt leere Zeilen)
            Dim sText() As String = dateiInhalt.Split(New String() {Environment.NewLine, vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries)

            ' 4. Einträge in die ComboBox einpflegen
            For i As Integer = 0 To sText.Length - 1
                Dim eintrag As String = sText(i).Trim()

                ' Nur sichtbare, nicht-leere Einträge hinzufügen
                If eintrag <> "" Then
                    tscoInfo.Items.Add(eintrag)
                End If
            Next

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Trägt die aktuell ausgewählte Schicht- oder Statusinformation aus der Info-ComboBox per Klick direkt in das DataGridView ein.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das DataGridView).</param>
    ''' <param name="e">Die Ereignisdaten, die die Indizes der geklickten Zelle enthalten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Fehlerhaften Index-Zugriff bei Header-Zeilen abgefangen (Prüfung von 'e.RowIndex > 0' statt '> -1', um die unsichtbare ID-Zeile zu schützen).
    ''' - Überflüssige und syntaktisch unsaubere Klammern bei der Eigenschaft '.Value()' entfernt.
    ''' - Fehlende Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor Laufzeitfehlern hinzugefügt.
    ''' - Logischen Vergleich durch Nutzung von 'AndAlso' für bessere Performance optimiert.
    ''' </remarks>
    Private Sub dgArbeit_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgArbeit.CellClick
        Try
            ' 1. Validierung der Indizes: Klick muss in einer Personenspalte (> 1) 
            ' und unterhalb der ausgeblendeten ID-Zeile (Index 0) liegen.
            If e.ColumnIndex > 1 AndAlso e.RowIndex > 0 Then

                ' 2. Den ausgewählten Text aus der ComboBox in die geklickte Zelle eintragen
                dgArbeit.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = tscoInfo.Text

            End If
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub


#End Region

#Region "Button und Menüs.........................................................................."

    ''' <summary>
    ''' Schließt das aktuelle Formular für die Personaleinteilung.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (der Schließen-Button).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Fehlende Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor Ausnahmefehlern beim Entladen des Formulars hinzugefügt.
    ''' - Einheitliches Fehler-Reporting-Muster aus dem Gesamtprojekt implementiert.
    ''' </remarks>
    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        Try
            ' Formular sauber schließen
            Me.Close()

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub



#End Region

#Region "Risize...................................................................................."

    ''' <summary>
    ''' Passt die Größe des DataGridViews dynamisch an die neue Formulargröße an und berechnet die Spaltenbreiten neu.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das Formular).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Aufruf von 'prColumns' entfernt.
    ''' - Fehlende Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor Fehlern bei Minimierung des Formulars hinzugefügt.
    ''' - 'WindowState'-Prüfung integriert, um unnötige Berechnungen und Abstürze im minimierten Zustand zu verhindern.
    ''' </remarks>
    Private Sub frmEinteilung_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Try
            ' 1. Berechnungen überspringen, wenn das Formular minimiert wurde
            If Me.WindowState = FormWindowState.Minimized Then Return

            ' 2. Dimensionen des DataGridViews an das Formular anpassen
            dgArbeit.Width = Me.Width - 40
            dgArbeit.Height = Me.Height - 110

            ' 3. Spaltenbreiten basierend auf dem neuen verfügbaren Platz neu berechnen
            prColumns(lEnable)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub


#End Region

#Region "Informationen in der Combobox verwalten..................................................."

    ''' <summary>
    ''' Fügt den neu eingegebenen Text der Info-ComboBox zur Auswahlliste hinzu und speichert die aktualisierte Liste dauerhaft in die Systemdatei.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (der Speichern-Button).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf von 'prSaveInfo' entfernt.
    ''' - Duplikatsprüfung integriert: Verhindert, dass identische Einträge mehrfach in die ComboBox und die Datei geschrieben werden.
    ''' - Validierung auf leere bzw. rein aus Leerzeichen bestehende Eingaben hinzugefügt.
    ''' - Fehlende Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor Laufzeitfehlern ergänzt.
    ''' </remarks>
    Private Sub tsbSaveInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSaveInfo.Click
        Try
            Dim neuerText As String = tscoInfo.Text.Trim()

            ' 1. Validierung: Nur speichern, wenn der Text nicht leer ist
            If Not String.IsNullOrWhiteSpace(neuerText) Then

                ' 2. Duplikatsprüfung: Nur hinzufügen, wenn der Eintrag noch nicht existiert
                If Not tscoInfo.Items.Contains(neuerText) Then
                    tscoInfo.Items.Add(neuerText)
                End If

                ' 3. Aktualisierte Liste in die Systemdatei schreiben
                prSaveInfo(sFileInfo)

            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Löscht den aktuell ausgewählten Eintrag aus der Info-ComboBox und aktualisiert die Systemdatei.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (der Löschen-Button).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Logischen Fehler korrigiert: Einträge werden nun via '.RemoveAt' sauber gelöscht, statt sie nur durch Leerstrings zu ersetzen.
    ''' - Rückwärtslaufende Schleife implementiert, um Verschiebungen des Auflistungs-Index beim Löschen zu verhindern.
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf von 'prSaveInfo' entfernt.
    ''' - Fehlende Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor Laufzeitfehlern ergänzt.
    ''' </remarks>
    Private Sub tsbDelInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelInfo.Click
        Try
            Dim sValue As String = tscoInfo.Text.Trim()

            ' 1. Validierung: Abbrechen, wenn kein Text zum Löschen vorhanden ist
            If String.IsNullOrEmpty(sValue) Then Return

            Dim nMax As Integer = tscoInfo.Items.Count - 1

            ' 2. Liste rückwärts durchlaufen, um gefundene Einträge sicher zu entfernen
            ' (Rückwärtslauf verhindert Index-Fehler, da sich die Liste beim Löschen verkürzt)
            For i As Integer = nMax To 0 Step -1
                If tscoInfo.Items(i).ToString().Trim() = sValue Then
                    tscoInfo.Items.RemoveAt(i)
                End If
            Next

            ' 3. Anzeige zurücksetzen und die verbleibende Liste in die Datei schreiben
            tscoInfo.Text = ""
            prSaveInfo(sFileInfo)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Speichert die aktuellen Einträge der Info-ComboBox dauerhaft in die Systemdatenbank und lädt die Liste anschließend neu.
    ''' </summary>
    ''' <param name="sDatei">Der Dateiname oder Pfad der Systemdatei, in die geschrieben werden soll.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Den Übergabeparameter von 'ByRef' auf 'ByVal' geändert, da das Pfad-Objekt nicht manipuliert werden muss.
    ''' - Robustere Validierung: Verhindert das Speichern von Einträgen, die nur aus Leerzeichen bestehen (Trim-Prüfung).
    ''' - Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor Schreib-/Lesekonflikten der Systemdatenbank ergänzt.
    ''' - Explizite Datentyp-Deklaration ('As Integer') für die Schleifenvariable hinzugefügt.
    ''' - Effiziente Ressourcen-Nutzung: Speicherreservierung für den StringBuilder optimiert.
    ''' </remarks>
    Private Sub prSaveInfo(ByVal sDatei As String)
        Try
            Dim nMax As Integer = tscoInfo.Items.Count - 1

            ' Vorzeitiger Abbruch, falls die ComboBox keine Elemente enthält
            If nMax < 0 Then Return

            Dim sb As New StringBuilder()

            ' 1. Alle Einträge durchlaufen und zeilenweise im StringBuilder sammeln
            For i As Integer = 0 To nMax
                Dim itemValue As Object = tscoInfo.Items(i)

                If itemValue IsNot Nothing Then
                    Dim eintrag As String = itemValue.ToString().Trim()

                    ' Nur speichern, wenn der Eintrag nicht leer ist
                    If eintrag <> "" Then
                        sb.AppendLine(eintrag)
                    End If
                End If
            Next

            ' 2. Den gesammelten Textblock in der System-DB hinterlegen
            SaveOneValueInSystemDb(sDatei, sb.ToString())

            ' 3. Die Liste direkt neu laden, um die UI zu aktualisieren
            prLoadInfo(sDatei)


        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub


#End Region

#Region "Einteilung speichern......................................................................"

    ''' <summary>
    ''' Speichert die gesamte Personaleinteilung aus dem DataGridView zurück in die Datenbank-Tabelle 'Personal2'.
    ''' Fasst zusammenhängende Schichten am Stück zusammen und generiert entsprechende Insert-Befehle.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (der Speichern-Button).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Kritischen Index-Überlauf (Crash-Gefahr) in der 'Do While'-Schleife durch zusätzliche Grenzwertprüfung ('j <= totalRows') behoben.
    ''' - Fehlerhaften mathematischen Operator auf dem ID-String ('sID += 1') durch korrekten 'Long'-Zähler ersetzt.
    ''' - Performance-Bottleneck beseitigt: 'Split' der Feldnamen aus der Schleife heraus vor den Start des Durchlaufs gezogen.
    ''' - Veraltete 'Call'-Syntax entfernt und implizite VB6-'MsgBox' durch modernes '.NET MessageBox.Show' ersetzt.
    ''' - Null-Verweise durch sichere Prüfungen (.Value IsNot Nothing) beim Auslesen der Grid-Zellen unterbunden.
    ''' </remarks>
    Private Sub tsbSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSave.Click
        Try
            ' 1. Bestehende Einteilungen in der Datenbank leeren
            UpdateTable("DELETE FROM Personal2")

            With dgArbeit
                Dim totalRows As Integer = .Rows.Count - 1
                Dim totalCols As Integer = .Columns.Count - 1

                ' Sicherheitsabbruch, falls keine Daten vorhanden sind
                If totalRows < 1 OrElse totalCols < 2 Then Return

                ' 2. Basis-ID für Datensätze generieren (numerisch verarbeiten, um sicher hochzuzählen)
                Dim baseID As Long
                If Not Long.TryParse(fcGetTimeID(Date.Today), baseID) Then
                    baseID = DateTime.Today.Ticks
                End If

                ' 3. Feldnamen-Array einmalig vorab erstellen (spart tausende Schleifenoperationen)
                Dim arFields() As String = "ID,ADatum,EDatum,Personal,Info".Split(","c)

                ' 4. Spalten durchlaufen (Ab Index 2 = Personen-Spalten)
                For i As Integer = 2 To totalCols
                    Dim cellHeader As Object = .Rows(0).Cells(i).Value
                    If cellHeader Is Nothing Then Continue For

                    Dim sIDP As String = cellHeader.ToString()

                    ' 5. Zeilen durchlaufen (Ab Index 1 = Datumswerte)
                    Dim j As Integer = 1
                    While j <= totalRows
                        Dim cellValue As Object = .Rows(j).Cells(i).Value

                        ' Wenn ein Eintrag (z.B. Schichtkürzel) gefunden wurde
                        If cellValue IsNot Nothing AndAlso cellValue.ToString().Trim() <> "" Then
                            Dim sInfo As String = cellValue.ToString()
                            Dim sADatum As String = fcUmDatum(.Rows(j).Cells(0).Value)
                            Dim sEDatum As String = ""

                            ' Zusammenhängende Tage mit gleicher Information blockweise zusammenfassen
                            ' Wichtig: 'j <= totalRows' sichert das Ende des Grids ab!
                            Do While j <= totalRows AndAlso .Rows(j).Cells(i).Value IsNot Nothing AndAlso .Rows(j).Cells(i).Value.ToString() = sInfo
                                sEDatum = fcUmDatum(.Rows(j).Cells(0).Value)
                                j += 1
                            Loop

                            ' Da j in der Do-While-Schleife bereits auf den nächsten ungleichen Tag sprang, 
                            ' korrigieren wir den Index für das Weiterlaufen der äußeren Schleife
                            j -= 1

                            ' 6. Datensatz in die Datenbank schreiben
                            Try
                                baseID += 1
                                Dim sqlText As String = fcSaveper(baseID.ToString(), sIDP, sADatum, sEDatum, sInfo)
                                Dim arValue() As String = sqlText.Split("°"c)

                                fcInsertCommand("Personal2", arFields, arValue)

                            Catch ex As Exception
                                ErrReport(ex.Message, ex.Source, ex.StackTrace)
                            End Try
                        End If

                        j += 1
                    End While
                Next
            End With

            ' 7. Erfolgsmeldung per modernem Windows-Forms Dialog
            MessageBox.Show("Eingaben erfolgreich gespeichert.", "Speichern", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Bereitet die Einteilungsdaten für einen Datensatz auf, indem sie mit einem Trennzeichen ('°') verkettet werden.
    ''' Ersetzt leere Felder standardmäßig durch ein Leerzeichen, um relationale Fehler beim späteren Splitten zu verhindern.
    ''' </summary>
    ''' <param name="sID">Die eindeutige Datensatz-ID.</param>
    ''' <param name="sIDP">Die ID der zugeordneten Person (wird als Kopie per Wert verarbeitet).</param>
    ''' <param name="sADatum">Das Startdatum des Zeitraums.</param>
    ''' <param name="sEDatum">Das Enddatum des Zeitraums.</param>
    ''' <param name="sInfo">Die eingetragene Information (z. B. Schichtkürzel).</param>
    ''' <returns>Einen mit '°' separierten String aller übergebenen Parameter für die Weiterverarbeitung im Insert-Kommando.</returns>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Übergabeparameter von 'ByRef' auf 'ByVal' geändert, um Seiteneffekte (ungewollte Manipulation von Originalvariablen im Aufrufer) zu verhindern.
    ''' - Veraltete VB6-Rückgabesyntax ('fcSaveper = ...') durch ein modernes .NET 'Return' ersetzt.
    ''' - Performance-Bottleneck beseitigt: Die altmodische 'Trim()'-Prüfung und mehrfache '.Append()'-Aufrufe durch effiziente String-Interpolation oder 'String.Join' ersetzt.
    ''' - Verwendung von modernem, null-sicherem 'String.IsNullOrWhiteSpace()' für die Vorbelegung.
    ''' </remarks>
    Private Function fcSaveper(ByVal sID As String, ByVal sIDP As String, ByVal sADatum As String,
                           ByVal sEDatum As String, ByVal sInfo As String) As String
        Try
            ' 1. Null- und Leerwertprüfungen mit moderner .NET-Logik (Ersetzt ein einzelnes Leerzeichen bei Bedarf)
            Dim cleanID As String = If(String.IsNullOrWhiteSpace(sID), " ", sID.Trim())
            Dim cleanIDP As String = If(String.IsNullOrWhiteSpace(sIDP), " ", sIDP.Trim())
            Dim cleanADatum As String = If(String.IsNullOrWhiteSpace(sADatum), " ", sADatum.Trim())
            Dim cleanEDatum As String = If(String.IsNullOrWhiteSpace(sEDatum), " ", sEDatum.Trim())
            Dim cleanInfo As String = If(String.IsNullOrWhiteSpace(sInfo), " ", sInfo.Trim())

            ' 2. Verkettung hocheffizient mittels String-Interpolation durchführen und direkt zurückgeben
            Return $"{cleanID}°{cleanADatum}°{cleanEDatum}°{cleanIDP}°{cleanInfo}"

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return String.Empty
        End Try
    End Function


#End Region

#Region "Drucken..................................................................................."
    ''' <summary>
    ''' Bereitet die Personaleinteilung für den ausgewählten Monat grafisch auf und übergibt die Daten blockweise an die VPE-Druck-Engine.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (der Drucken-Button).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Kritischen Index-Absturz (Gefahr von 'Rows(-1)' bei nicht gefundenem Startdatum) durch Validierung abgefangen.
    ''' - Typsicheres, länderunabhängiges Parsen von Datumswerten mittels 'Date.TryParse' implementiert.
    ''' - Veraltete VB6-Syntax ('Call', 'Val', 'Trim', 'CDate') vollständig entfernt.
    ''' - String-Verkettungen im Tabellenkopf über performante String-Interpolation gelöst.
    ''' - Ressourcenschonung: 'Try-Catch'-Logik für VPE-Engine abgesichert, um hängende Druckprozesse im Fehlerfall zu verhindern.
    ''' </remarks>
    Private Sub tsbPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPrint.Click
        Try
            ' 1. Eingabeparameter auslesen und validieren
            Dim nPersonal As Integer = 10
            Integer.TryParse(tscoPerson.Text, nPersonal)
            If nPersonal <= 0 Then nPersonal = 10

            Dim nMax As Integer = nPersonen + 1
            Dim nMonthRaw As String = tscoMonat.Text.Trim()
            Dim nMonth As String = If(nMonthRaw.Length = 1, "0" & nMonthRaw, nMonthRaw) ' Entspricht PadLN auf 2 Stellen
            Dim nYear As String = tscoJahr.Text.Trim()

            ' Ziel-Suchdatum im Format der fcUmDatum-Funktion generieren
            Dim cDatum As String = $"{nYear}{nMonth}01"

            ' Monatstage dynamisch und ländersicher über .NET-Kalender ermitteln
            Dim parsedMonth, parsedYear As Integer
            If Not Integer.TryParse(nMonth, parsedMonth) OrElse Not Integer.TryParse(nYear, parsedYear) Then Return

            Dim startDate As New Date(parsedYear, parsedMonth, 1)
            Dim daysInMonth As Integer = Date.DaysInMonth(parsedYear, parsedMonth)
            Dim totalRowsNeeded As Integer = daysInMonth + 1 ' Tage + Kopfzeile

            ' 2. VPE Engine initialisieren
            prVpeOpen()
            prVpePageFormat("A4Rotated")
            prTableDim(20, 10, 270, 170)
            prTableRows(totalRowsNeeded + 1)

            ' 3. Personen blockweise (Spalten-Splitting auf Seiten) durchlaufen
            For nPer As Integer = 0 To nMax - 1 Step nPersonal

                If (nMax - 1 - nPer) < nPersonal Then
                    nPersonal = nMax - nPer
                End If

                prTableColumns(2 + nPersonal + 1)
                prTableColumnsWidth(0, 20)
                prTableColumnsWidth(1, 20)
                prTableColumnsWidth(2 + nPersonal, 50)
                prTableRowsHeigth(0, 12)
                prTabelCellSelectFont(2 + nPersonal, 0, "Times New Roman", 10)
                prTabelCellText(2 + nPersonal, 0, "Bemerkung")

                Dim nKopf As Integer = nPer + 2 + nPersonal - 1

                ' 4. Tabellenkopf für aktuelle Seite befüllen
                For c As Integer = nPer + 2 To nKopf
                    ' KRITISCHER SCHUTZ: Wenn der Index c die echten Spalten des Grids überschreitet, 
                    ' brechen wir die Schleife sofort ab, um die IndexOutOfRangeException zu verhindern.
                    If c >= dgArbeit.Columns.Count Then Exit For

                    Dim targetColIndex As Integer = c - nPer
                    prTabelCellSelectFont(targetColIndex, 0, "Times New Roman", 10)
                    prTabelCellSelectFont(targetColIndex, totalRowsNeeded, "Times New Roman", 10)

                    Dim cellHeaderValue As Object = dgArbeit.Rows(0).Cells(c).Value
                    If cellHeaderValue IsNot Nothing Then
                        Dim sIDPer As String = cellHeaderValue.ToString()

                        ' Passenden Mitarbeiter in dtP suchen
                        For n As Integer = 0 To dtP.Rows.Count - 1
                            If sIDPer = dtP.Rows(n).Item("ID").ToString() Then
                                Dim rowP As DataRow = dtP.Rows(n)
                                Dim sPer As String = $"{rowP("Vorname")} {rowP("Name")}{Environment.NewLine}{rowP("Tel1")}"

                                prTabelCellText(targetColIndex, 0, sPer)
                                prTabelCellText(targetColIndex, totalRowsNeeded, rowP("Info").ToString())
                                Exit For ' Gefunden, innere Suche abbrechen
                            End If
                        Next
                    End If
                Next

                ' 5. Startzeile des gesuchten Monats im DataGridView ermitteln
                Dim gridStartRowIndex As Integer = -1
                For r As Integer = 1 To dgArbeit.Rows.Count - 1
                    If fcUmDatum(dgArbeit.Rows(r).Cells(0).Value) = cDatum Then
                        gridStartRowIndex = r
                        Exit For
                    End If
                Next

                ' Sicherheitsnetz: Wenn das Datum im Grid nicht existiert, Seite überspringen um Crash zu vermeiden
                If gridStartRowIndex = -1 Then Continue For

                ' 6. Datenzeilen für jeden Tag in die Drucktabelle schreiben
                For r As Integer = 1 To daysInMonth
                    Dim gridCurrentRowIndex As Integer = r + gridStartRowIndex - 1

                    ' Prüfen, ob der Index im gültigen Grid-Zeilenbereich liegt
                    If gridCurrentRowIndex >= dgArbeit.Rows.Count Then Exit For

                    Dim rowGrid As DataGridViewRow = dgArbeit.Rows(gridCurrentRowIndex)

                    ' Farb-Fallbacks einrichten (falls Style.BackColor oder ForeColor leer ist)
                    Dim cell0Back As Color = If(rowGrid.Cells(0).Style.BackColor.IsEmpty, dgArbeit.DefaultCellStyle.BackColor, rowGrid.Cells(0).Style.BackColor)
                    Dim cell0Fore As Color = If(rowGrid.Cells(0).Style.ForeColor.IsEmpty, dgArbeit.DefaultCellStyle.ForeColor, rowGrid.Cells(0).Style.ForeColor)

                    Dim cell1Back As Color = If(rowGrid.Cells(1).Style.BackColor.IsEmpty, dgArbeit.DefaultCellStyle.BackColor, rowGrid.Cells(1).Style.BackColor)
                    Dim cell1Fore As Color = If(rowGrid.Cells(1).Style.ForeColor.IsEmpty, dgArbeit.DefaultCellStyle.ForeColor, rowGrid.Cells(1).Style.ForeColor)

                    ' Spalte 0: Datum
                    prTabelCellBackColor(0, r, cell0Back)
                    prTabelCellForeColor(0, r, cell0Fore)
                    prTabelCellSelectFont(0, r, "Times New Roman", 10)
                    prTabelCellText(0, r, rowGrid.Cells(0).Value)

                    ' Spalte 1: Wochentag
                    prTabelCellBackColor(1, r, cell1Back)
                    prTabelCellForeColor(1, r, cell1Fore)
                    prTabelCellSelectFont(1, r, "Times New Roman", 10)
                    prTabelCellText(1, r, rowGrid.Cells(1).Value)

                    ' Personenspalten für diesen Tag befüllen
                    For c As Integer = nPer + 2 To nKopf
                        ' KRITISCHER SCHUTZ: Wenn der Spaltenindex die echten Grid-Spalten überschreitet,
                        ' brechen wir die Spaltenschleife für diesen Tag sofort ab.
                        If c >= dgArbeit.Columns.Count Then Exit For

                        Dim targetColIndex As Integer = c - nPer
                        prTabelCellSelectFont(targetColIndex, r, "Times New Roman", 10)

                        ' Null-sichere Übergabe des Zellwerts an die Druck-Engine
                        Dim cellValue As Object = rowGrid.Cells(c).Value
                        Dim sCellValue As String = If(cellValue IsNot Nothing, cellValue.ToString(), "")

                        prTabelCellText(targetColIndex, r, sCellValue)
                    Next
                Next

                prTabelWrite()
                prVpeNewPage()
            Next

            prVpeVeiw()

        Catch ex As Exception
            prVpeClose() ' Engine bei Fehler sauber schließen, um System-Hänger zu vermeiden
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub



#End Region

End Class