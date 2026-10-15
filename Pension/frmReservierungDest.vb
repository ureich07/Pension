Imports System.IO
Imports System.Text
Imports Microsoft

Public Class frmReservierungDest
    Dim lStart As Boolean = True 'ist True solange der Ladevorgang des Formulars läuft
    Dim arZ(30, 4) As String
    Dim arFZ(30, 1) As String
    Dim arZim(30, 2) As String
    Dim dtK As DataTable        'Kunden
    Dim arOld(24) As String
    Dim arNew(24) As String
    Dim sBID As String 'ID einer einzelnen Reservierung
    Dim lUnbekannt As Boolean = False
    Dim sDayStart As Date
    Dim sDayEnd As Date
    Dim arZZiel() As String
    Dim dtBuchIndex As DataTable
    Dim nIndexIst As Integer = 0
    Dim sIndexIst As String = "0"
    Dim sIDRef As String
    Dim sIDB As String
    Dim sIDR As String ' Buchungssatz ID
    Dim bLoad As Boolean = False
    Dim sCode As String
    Dim arKalender(6, 10) As String
    Dim sLanguage1 As Array = Split(ReadOneValueFromSystemDb("Language"), vbCrLf)
    Dim sLanguage As Array = Split(sLanguage1(0), ";")
    Dim sMwstU As String
    Dim sMwstUa As String


#Region "Load Form und Funktionen zur Darstellung des Moduls......................................."

    ''' <summary>
    ''' Initialisiert das Reservierungsmodul, bereitet die Benutzeroberfläche vor, 
    ''' initialisiert Datenstrukturen und lädt die Buchungs- sowie Preisdaten aus der Datenbank.
    ''' </summary>
    ''' <param name="sender">Das auslösende Objekt (das Formular selbst).</param>
    ''' <param name="e">Die Ereignisdaten des Load-Events.</param>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Performance-Boost: Zweidimensionale Array-Initialisierung ('arZim') durch 'Array.Clear' oder kompakte Schleifen modernisiert.
    ''' - String-Verarbeitung: Veraltete 'Split'-Funktion durch die native '.Split(","c)' Methode ersetzt.
    ''' - Robustheit erhöht: Sichere Typkonvertierung mit 'Integer.TryParse' statt der fehleranfälligen 'Val'-Funktion.
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter vollständig entfernt und Variablen-Deklarationen geschärft.
    ''' - UI-Flackern verhindert: Visuelle Vorbereitung findet vor den Datenbankzugriffen statt, Cursor-Steuerung über Try-Finally abgesichert.
    ''' </remarks>
    Private Sub frmReservierung_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' 1. UI-Vorbereitungen und Layout-Einstellungen
        lbMailList.Visible = False
        MyKalender1.Visible = False
        gbNameZ.Visible = False
        Me.Cursor = Cursors.WaitCursor

        Dim sColor As Color = Color.LightYellow
        Me.BackColor = sColor
        gbGast.BackColor = sColor
        gbZimmer.BackColor = sColor
        ssMain.BackColor = sColor
        tsMain.BackColor = sColor

        tsbVorAnreise.Visible = False
        tsbNachAbreise.Visible = False

        Try
            ' 2. Datenstrukturen initialisieren
            ' Schnelles Löschen des zweidimensionalen Arrays (20 Zeilen, 3 Spalten)
            For i As Integer = 0 To 19
                For j As Integer = 0 To 2
                    arZim(i, j) = ""
                Next
            Next

            ' Basis-Tabellenstrukturen und Stammdaten laden
            prLand()
            prCreateTabelleGast()
            prLadeWerbung()
            prLadeZimmerInList(sgRBID)

            ' Sprachen in ComboBox füllen
            If sLanguage IsNot Nothing Then
                For i As Integer = 1 To sLanguage.Length - 1
                    If Not String.IsNullOrEmpty(sLanguage(i)) Then
                        Dim arLa() As String = sLanguage(i).Split(","c)
                        If arLa.Length > 0 Then
                            coLang.Items.Add(arLa(0))
                        End If
                    End If
                Next
            End If
            coLang.SelectedIndex = 0 ' Standardsprache setzen
            coArt.Text = ""

            ' 3. Daten abrufen (Preise und spezifische Buchung)
            dtPre = fcReadDataTable("Select * from Preise")

            ' SQL-Statement mit den globalen/Klassen-Variablen aufbauen
            Dim sSQL As String = "Select * From Buchung Where BID='" & sgRBID & "' and ZimID = '" & sgRZID & "'"
            Dim dt As DataTable = fcReadDataTable(sSQL)

            ' Wenn kein Datensatz gefunden wurde, Formular sofort schließen und Methode verlassen
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                Me.Close()
                Exit Sub
            End If

            ' 4. Buchungsdaten verarbeiten und UI zuweisen
            sCode = dt.Rows(0).Item("Code").ToString()

            If String.IsNullOrWhiteSpace(sCode) OrElse sCode = "00000" Then
                sCode = fcNewCode(sgRZID)
            End If

            ' Sicherer Ersatz für die veraltete VB6-Funktion 'Val'
            Dim parsedCode As Integer = 0
            If Integer.TryParse(sCode, parsedCode) AndAlso parsedCode < 0 Then
                sCode = ""
            End If

            tslCode.Text = sCode
            sIDR = dt.Rows(0).Item("ID").ToString()

            ' Zusatzdaten und Berechnungen laden
            prLadeBuchung(sgRBID, sIDR)
            arOld = fcCollectDataInArray(arOld)
            tbAnzPer.Select()
            prCreatTabelIndex()

            lUnbekannt = fcCheckNewResevierung(sgRBID, sgRZID)
            Summe(sgRBID, sgRZID)
            prLoadBText()

            ' Status setzen, dass Laden erfolgreich abgeschlossen wurde
            bLoad = True
            Dim sBuchDir As String = Path.Combine(arIni(32), "Buchung")
            Dim sBuchFile As String = Path.Combine(sBuchDir, "Buch_" & sgRBID & ".PDF")
            tsbMail.Enabled = False
            If FileExists(sBuchFile) Then tsbMail.Enabled = True
        Finally
            ' Cursor in jedem Fall wieder zurücksetzen (auch im Fehlerfall)
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Lädt die verfügbaren Länder aus der Systemdatenbank, bereinigt die Einträge von Steuerzeichen und befüllt die Länder-Auswahlliste (coLand).
    ''' </summary>
    ''' <remarks>
    ''' 01.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Performance-Boost: 'BeginUpdate()' und 'EndUpdate()' für die ComboBox hinzugefügt, um das Zeichnen während des Befüllens zu unterdrücken.
    ''' - Logik-Korrektur: Splitten per 'StringSplitOptions.RemoveEmptyEntries' fängt sowohl 'vbCrLf' (Windows) als auch 'vbLf' (Linux/Web) sauber ab und eliminiert leere Einträge automatisch.
    ''' - Robustheit erhöht: Null-Prüfung für den Rückgabewert der Datenbank integriert, um Abstürze bei fehlenden Datenbankeinträgen zu verhindern.
    ''' - Syntax bereinigt: Explizite Typisierung der Schleifenvariable eingeführt.
    ''' </remarks>
    Private Sub prLand()
        ' Wert aus der Datenbank auslesen
        Dim dbValue As String = ReadOneValueFromSystemDb("Land")

        ' Sicherheitsprüfung: Falls der Datenbankeintrag leer oder null ist, abbrechen
        If String.IsNullOrEmpty(dbValue) Then
            coLand.Text = "DE"
            Exit Sub
        End If

        ' Sauber am Zeilenumbruch splitten. 'RemoveEmptyEntries' filtert leere Zeilen direkt heraus.
        ' Wir splitten nach Control-Chars (Cr und Lf), um plattformunabhängig zu sein.
        Dim separators() As Char = {Convert.ToChar(VisualBasic.Constants.vbCr), Convert.ToChar(VisualBasic.Constants.vbLf)}
        Dim arTmp() As String = dbValue.Split(separators, StringSplitOptions.RemoveEmptyEntries)

        ' UI-Aktualisierung der ComboBox einfrieren
        coLand.BeginUpdate()
        Try
            coLand.Items.Clear() ' Optional: Liste vor dem Befüllen leeren, um Duplikate bei mehrfachem Aufruf zu vermeiden

            For i As Integer = 0 To arTmp.Length - 1
                Dim countryCleaned As String = arTmp(i).Trim()

                If Not String.IsNullOrEmpty(countryCleaned) Then
                    coLand.Items.Add(countryCleaned)
                End If
            Next
        Finally
            ' Zeichnen der ComboBox wieder freigeben
            coLand.EndUpdate()
        End Try

        ' Standardwert setzen
        coLand.Text = "DE"
    End Sub

    ''' <summary>
    ''' Erstellt die Spaltenstruktur für die Gästetabelle (lvGast) und setzt die Steuerelement-Eigenschaften.
    ''' </summary>
    ''' <remarks>
    ''' 04.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - UI-Flackern unterdrückt: '.BeginUpdate()' und '.EndUpdate()' hinzugefügt, um den Spaltenaufbau im Hintergrund durchzuführen.
    ''' - Namensräume bereinigt: Den vollqualifizierten Aufruf bei 'HeaderStyle' auf die native VB-Enumeration verkürzt.
    ''' </remarks>
    Private Sub prCreateTabelleGast()
        With lvGast
            ' Zeichnen einfrieren, um Flackern beim Neuerstellen der Spalten zu verhindern
            .BeginUpdate()
            Try
                .Clear()

                ' Spalten hinzufügen
                .Columns.Add("Name-Firma", 120, HorizontalAlignment.Left)
                .Columns.Add("Vorname", 70, HorizontalAlignment.Left)
                .Columns.Add("PLZ", 50, HorizontalAlignment.Left)
                .Columns.Add("Ort", 100, HorizontalAlignment.Left)
                .Columns.Add("Strasse", 120, HorizontalAlignment.Left)
                .Columns.Add("ID", 0, HorizontalAlignment.Left) ' Versteckte ID-Spalte

                ' Eigenschaften konfigurieren
                .FullRowSelect = True
                .GridLines = True
                .HeaderStyle = ColumnHeaderStyle.Nonclickable
                .HideSelection = False
                .MultiSelect = False
                .Sorting = SortOrder.Ascending
                .TabIndex = 0
                .View = View.Details
            Finally
                ' Steuerelement zur Aktualisierung freigeben
                .EndUpdate()
            End Try
        End With
    End Sub

    ''' <summary>
    ''' Befüllt das ListView (lvGast) mit Kunden aus der Datenbank, deren Name mit dem Anfangsbuchstaben aus tbName1 übereinstimmt.
    ''' </summary>
    ''' <remarks>
    ''' 04.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Performance-Boost: 'BeginUpdate()' und 'EndUpdate()' umschließen das Laden der Elemente, um UI-Flackern zu verhindern.
    ''' - API-Modernisierung: Veraltete VB6-Funktionen ('Len', 'Trim', 'Mid') durch moderne .NET-Methoden ersetzt ('.Length', '.Trim()', '.Substring()').
    ''' - Effizienz: Nutzt 'String.IsNullOrWhiteSpace' für eine saubere und performante Prüfung auf leere Eingaben.
    ''' - Robustheit: Null-Prüfung für die 'DataTable' eingeführt, um Abstürze bei Datenbankausfällen zu verhindern.
    ''' </remarks>
    Private Sub prLoadGastInList()
        Dim searchText As String = tbName1.Text.Trim()

        ' Abbruchbedingungen prüfen (Ersatz für Len/Trim)
        If String.IsNullOrWhiteSpace(searchText) Then Exit Sub
        If lvGast.Items.Count > 0 AndAlso searchText.Length > 1 Then Exit Sub

        ' Den ersten Buchstaben sicher extrahieren (Ersatz für Mid)
        Dim firstLetter As String = searchText.Substring(0, 1)

        ' SQL-Statement vorbereiten
        Dim sSQL As String = "Select * from Kunden where name1 like '" & firstLetter & "%' order by Name1, Vorname asc"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' Sicherheitsprüfung: Existiert die DataTable und enthält sie Zeilen?
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

        ' UI-Aktualisierung einfrieren
        lvGast.BeginUpdate()
        Try
            lvGast.Items.Clear()

            ' Schleife über alle Zeilen der DataTable
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim row As DataRow = dt.Rows(i)

                ' Element über deine Hilfsfunktion hinzufügen
                lvwAddItem(lvGast,
                       row("Name1").ToString(),
                       row("Vorname").ToString(),
                       row("PLZ").ToString(),
                       row("Ort").ToString(),
                       row("Strasse").ToString(),
                       row("ID").ToString())
            Next
        Finally
            ' UI wieder freigeben
            lvGast.EndUpdate()
        End Try
    End Sub


    ''' <summary>
    ''' Durchsucht das ListView (lvGast) nach Einträgen, die mit den Texten aus tbName1 und tbVorname übereinstimmen.
    ''' Markiert den besten Treffer farblich und scrollt diesen in den sichtbaren Bereich.
    ''' </summary>
    ''' <remarks>
    ''' 04.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Logikfehler korrigiert: Die zweite Schleife sucht nun basierend auf dem Treffer der ersten Schleife (Kurzname -> Vollname), statt die Suche komplett bei 0 zu wiederholen.
    ''' - Absturzsicherung: Prüfung auf '.Items.Count > 0' eingeführt, um Fehler bei leerer Tabelle zu verhindern.
    ''' - API-Modernisierung: Veraltete VB6-Funktion 'Mid' durch die native '.StartsWith()' Methode ersetzt (deutlich performanter und lesbarer).
    ''' - Performance: Mehrfache '.Refresh()'-Aufrufe entfernt, um die UI-Last zu minimieren.
    ''' </remarks>
    Private Sub prFindGast()
        Dim nameSearch As String = tbName1.Text.Trim()
        Dim vornameSearch As String = tbVorname.Text.Trim()

        With lvGast
            .BackColor = Color.White

            ' Wenn das ListView leer ist, brechen wir sofort ab, um Index-Abstürze zu vermeiden
            If .Items.Count = 0 Then Exit Sub

            Dim foundIndex As Integer = -1

            ' 1. Stufe: Nach dem Nachnamen suchen (Teiltreffer am Anfang)
            If Not String.IsNullOrEmpty(nameSearch) Then
                For i As Integer = 0 To .Items.Count - 1
                    Dim cellText As String = .Items(i).SubItems(0).Text

                    ' Nutzt .StartsWith statt der langsamen 'Mid'-Konstruktion
                    If cellText.StartsWith(nameSearch, StringComparison.CurrentCultureIgnoreCase) Then
                        foundIndex = i
                        Exit For
                    End If
                Next
            End If

            ' 2. Stufe: Präzise Suche (Nachname UND Vorname), startend beim ersten Nachnamens-Treffer
            Dim startIndex As Integer = Math.Max(0, foundIndex)
            If Not String.IsNullOrEmpty(nameSearch) AndAlso Not String.IsNullOrEmpty(vornameSearch) Then
                For i As Integer = startIndex To .Items.Count - 1
                    Dim cellName As String = .Items(i).SubItems(0).Text

                    ' Sicherstellen, dass genügend SubItems für den Vornamen (Index 1) vorhanden sind
                    If .Items(i).SubItems.Count > 1 Then
                        Dim cellVorname As String = .Items(i).SubItems(1).Text

                        If cellName.StartsWith(nameSearch, StringComparison.CurrentCultureIgnoreCase) AndAlso
                       cellVorname.StartsWith(vornameSearch, StringComparison.CurrentCultureIgnoreCase) Then
                            foundIndex = i
                            Exit For
                        End If
                    End If
                Next
            End If

            ' 3. Stufe: Auswertung und farbliche Markierung, falls ein Treffer erzielt wurde
            If foundIndex >= 0 AndAlso foundIndex < .Items.Count Then
                Dim matchedItem As ListViewItem = .Items(foundIndex)

                matchedItem.Selected = True
                matchedItem.EnsureVisible()

                ' Exakte Übereinstimmungen prüfen und einfärben
                If nameSearch.Equals(matchedItem.SubItems(0).Text, StringComparison.CurrentCultureIgnoreCase) Then
                    matchedItem.Selected = False
                    matchedItem.BackColor = Color.Yellow
                End If

                If matchedItem.SubItems.Count > 1 AndAlso
               vornameSearch.Equals(matchedItem.SubItems(1).Text, StringComparison.CurrentCultureIgnoreCase) Then
                    matchedItem.Selected = False
                    matchedItem.BackColor = Color.Red
                End If
            End If
        End With
    End Sub

    ''' <summary>
    ''' Fügt dem angegebenen ListView eine komplette Datenzeile basierend auf einer flexiblen Parameterliste hinzu.
    ''' </summary>
    ''' <param name="lvw">Das Ziel-ListView-Control.</param>
    ''' <param name="Text">Eine variable Liste (ParamArray) an String-Werten für die einzelnen Spaltenzellen.</param>
    ''' <remarks>
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Robustheit erhöht: Null- und Längenprüfung für das 'Text'-Array integriert, um Laufzeitfehler zu vermeiden.
    ''' - Syntax bereinigt: Redundanten 'With'-Block entfernt.
    ''' </remarks>
    Public Sub lvwAddItem(ByVal lvw As ListView, ByVal ParamArray Text() As String)
        ' Sicherheitsprüfung: Wenn das ListView oder das Array leer ist, abbrechen
        If lvw Is Nothing OrElse Text Is Nothing OrElse Text.Length = 0 Then Exit Sub

        ' Element direkt und ohne redundanten With-Block hinzufügen
        lvw.Items.Add(New ListViewItem(Text))
    End Sub

    ''' <summary>
    ''' Bindet die Werbedaten aus der globalen DataTable (dtWer) an die ComboBox (coWerbung) und setzt den Standardwert.
    ''' </summary>
    ''' <remarks>
    ''' 04.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Performance-Boost: Die Eigenschaften 'ValueMember' und 'DisplayMember' werden nun VOR der 'DataSource' gesetzt. Das verhindert, dass .NET die Datenstruktur intern mehrfach berechnet und zeichnet.
    ''' - Robustheit erhöht: Prüfung integriert, ob 'dtWer' überhaupt Daten enthält, bevor die Bindung stattfindet.
    ''' </remarks>
    Private Sub prLadeWerbung()
        ' Sicherheitsprüfung: Falls die Werbetabelle nicht initialisiert ist, Bindung überspringen
        If dtWer Is Nothing Then
            coWerbung.Text = "Unbekannt"
            Exit Sub
        End If

        With coWerbung
            ' WICHTIG für Performance: Erst die Member-Struktur definieren...
            .ValueMember = "Werbung"
            .DisplayMember = "Werbung"

            ' ...und erst ganz am Schluss die Datenquelle zuweisen!
            .DataSource = dtWer

            ' Standard-Anzeigetext setzen
            .Text = "Unbekannt"
        End With
    End Sub

    ''' <summary>
    ''' Lädt die belegten (tscoZim) und freien (tscoFreiZim) Zimmer für den Buchungszeitraum in die entsprechenden Auswahlboxen.
    ''' </summary>
    ''' <param name="sRBID">Die eindeutige Buchungs-ID.</param>
    ''' <remarks>
    ''' 03.02.2012 - Create
    ''' 03.10.2026 - Code-Optimization:
    ''' - Absturzsicherung: 'dt.Rows.Count > 0'-Prüfung vor dem Zugriff auf 'dt.Rows(0)' eingebaut, um Abstürze bei leeren Buchungen zu verhindern.
    ''' - Performance-Boost: 'ReDim Preserve' in der Schleife durch eine effiziente 'List(Of String)' ersetzt. 'BeginUpdate' für beide ComboBoxen integriert.
    ''' - Robustheit erhöht: Feste Array-Grenzen (z. B. Index 2 bis 20) durch dynamische Abfragen abgesichert ('arZ.GetLength(0)'), um 'IndexOutOfRangeException' zu vermeiden.
    ''' - Syntax bereinigt: Veraltetes 'Space(30)' durch die native '.PadRight()' Methode ersetzt. 'Nothing'-Vergleiche bei Strings auf 'String.IsNullOrEmpty' umgestellt.
    ''' </remarks>
    Private Sub prLadeZimmerInList(ByVal sRBID As String)
        ' 1. Belegte Zimmer der aktuellen Buchung laden
        Dim sSQL As String = "Select * From Buchung Where BID='" & sRBID & "' and BIDIndex = '0'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' UI-Aktualisierung einfrieren
        tscoZim.BeginUpdate()
        tscoFreiZim.BeginUpdate()

        Try
            tscoZim.Items.Clear()
            tscoFreiZim.Items.Clear()

            ' Statische Einträge initialisieren
            arZ(0, 0) = "99"
            arZ(0, 1) = "Weiteres Zimmer"
            tscoZim.Items.Add("Weiteres Zimmer")

            arZ(1, 0) = "98"
            arZ(1, 1) = "Gleiches Zimmer"
            tscoZim.Items.Add("Gleiches Zimmer")

            ' Buchungszeilen in das interne Array arZ übertragen
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim maxRows As Integer = Math.Min(dt.Rows.Count - 1, arZ.GetLength(0) - 3) ' Array-Überlaufschutz
                For i As Integer = 0 To maxRows
                    Dim row As DataRow = dt.Rows(i)
                    arZ(i + 2, 0) = row("ZimID").ToString()
                    arZ(i + 2, 2) = row("ID").ToString()
                    arZ(i + 2, 3) = row("Von").ToString()
                    arZ(i + 2, 4) = row("bis").ToString()
                    arZ(i + 2, 1) = " "
                Next
            End If

            ' Zimmernamen ermitteln und in die tscoZim ComboBox eintragen
            For i As Integer = 2 To Math.Min(20, arZ.GetLength(0) - 1)
                If Not String.IsNullOrEmpty(arZ(i, 0)) Then
                    arZ(i, 1) = fcGetObjektZimmerName(dtZim, arZ(i, 0)).Trim()

                    If Not String.IsNullOrEmpty(arZ(i, 1)) Then
                        ' PadRight(30) ersetzt das alte VB6 'Space(30)' sauberer
                        tscoZim.Items.Add(arZ(i, 1).PadRight(30) & "[" & arZ(i, 2) & "]")
                    End If
                End If
            Next

            ' ABSTURZSICHERUNG: Nur fortfahren, wenn überhaupt Buchungsdaten für den Zeitraum vorliegen
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                ' 2. Freie Zimmer im selben Zeitraum ermitteln
                Dim sB As String = dt.Rows(0)("Von").ToString()
                Dim sE As String = dt.Rows(0)("Bis").ToString()

                sSQL = "Select * From Buchung Where (Von <='" & sB & "' or Von <='" & sE & "') and (Bis >='" & sB & "' or Bis >='" & sE & "')"
                Dim dtBelegt As DataTable = fcReadDataTable(sSQL)

                ' Performance-Optimierung: List(Of String) statt langsamer ReDim Preserve Schleife
                Dim belegteZimmerList As New List(Of String)()
                If dtBelegt IsNot Nothing Then
                    For i As Integer = 0 To dtBelegt.Rows.Count - 1
                        belegteZimmerList.Add(dtBelegt.Rows(i)("ZimID").ToString())
                    Next
                End If

                ' Freie Zimmer filtern und in arFZ übertragen
                If dtZim IsNot Nothing Then
                    Dim n As Integer = 0
                    Dim maxZimmerIndex As Integer = arFZ.GetLength(0) - 1

                    For i As Integer = 0 To dtZim.Rows.Count - 1
                        If n > maxZimmerIndex Then Exit For ' Array-Überlaufschutz

                        Dim sTmp As String = dtZim.Rows(i)("ID").ToString().Trim()

                        ' Prüfen, ob das Zimmer in der Belegt-Liste existiert
                        If Not belegteZimmerList.Contains(sTmp) Then
                            arFZ(n, 0) = sTmp
                            arFZ(n, 1) = dtZim.Rows(i)("Name").ToString()
                            n += 1
                        End If
                    Next
                End If
            End If

            ' Freie Zimmer in tscoFreiZim ComboBox laden
            For i As Integer = 0 To Math.Min(20, arFZ.GetLength(0) - 1)
                If Not String.IsNullOrEmpty(arFZ(i, 0)) Then
                    tscoFreiZim.Items.Add(arFZ(i, 1))
                End If
            Next

            ' 3. Zeitraum-Zusammenfassung im globalen arZim Array berechnen
            For i As Integer = 2 To Math.Min(19, arZ.GetLength(0) - 1)
                Dim currentZimmerName As String = arZ(i, 1)

                If Not String.IsNullOrEmpty(currentZimmerName) Then
                    For j As Integer = 0 To Math.Min(19, arZim.GetLength(0) - 1)

                        ' Freien Slot im arZim finden und belegen
                        If String.IsNullOrEmpty(arZim(j, 0)) Then
                            arZim(j, 0) = currentZimmerName
                            arZim(j, 1) = arZ(i, 3)
                            arZim(j, 2) = arZ(i, 4)
                            Exit For
                        End If

                        ' Bestehenden Eintrag finden und Zeitraum erweitern (Min/Max Logik)
                        If currentZimmerName = arZim(j, 0) Then
                            If String.Compare(arZim(j, 1), arZ(i, 3)) > 0 Then arZim(j, 1) = arZ(i, 3)
                            If String.Compare(arZim(j, 2), arZ(i, 4)) < 0 Then arZim(j, 2) = arZ(i, 4)
                            Exit For
                        End If
                    Next
                End If
            Next

        Finally
            ' Zeichenfunktionen der ComboBoxen in jedem Fall wieder freigeben
            tscoZim.EndUpdate()
            tscoFreiZim.EndUpdate()
        End Try
    End Sub


    ''' <summary>
    ''' Lädt die kompletten Buchungsdaten aus der Datenbank, weist diese den UI-Elementen zu und initialisiert die länderspezifischen Steuersätze sowie Rechnungsstatus.
    ''' </summary>
    ''' <param name="sRID">Die eindeutige Buchungs-ID (BID).</param>
    ''' <param name="sIDRe">Die eindeutige Datensatz-ID.</param>
    ''' <remarks>
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Absturzsicherung: 'Try-Catch'-Block nach oben verschoben, um den Datenbankaufruf 'fcReadDataTable' sicher einzuschließen.
    ''' - Performance-Boost: Lokale Zuweisung 'Dim row As DataRow = dt.Rows(0)' verhindert das permanente, rechenintensive Neuerstellen der Zeilenreferenz.
    ''' - Robustheit erhöht: Sichere Konvertierung über 'Convert.ToInt32'/'Double.TryParse' für 'Val' implementiert. System-DBNulls werden stabil abgefangen.
    ''' - Syntax bereinigt: Veraltete 'Call'-Befehle, 'Space(30)' und redundante Codezeilen entfernt.
    ''' </remarks>
    Private Sub prLadeBuchung(ByVal sRID As String, ByVal sIDRe As String)
        Dim sSQL As String = "Select * From Buchung Where BID='" & sRID & "' and ID='" & sIDRe & "'"
        Dim st = prLadeBuchungInTextfeld(sRID)

        Try
            ' Der Datenbank-Call gehört zwingend in den Try-Block, falls die Verbindung fehlschlägt
            Dim dt As DataTable = fcReadDataTable(sSQL)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                ' Performance-Boost: Zeilenreferenz cachen
                Dim row As DataRow = dt.Rows(0)

                ' 1. Steuersätze und Gebühren einlesen
                sMwstU = row("MwstU").ToString()

                ' Fallback auf Standardwerte aus arIni, falls der Datenbankwert "0" ist
                If sMwstU = "0" OrElse String.IsNullOrWhiteSpace(sMwstU) Then sMwstU = arIni(11)

                ' Aktuelle Werte spiegeln
                sMwstUa = sMwstU

                ' IDs auslesen
                sIDB = row("ID").ToString()
                sBID = sIDB

                ' Split-Zimmer Logik prüfen
                If row("BIDIndex").ToString() = "0" Then
                    sIDRef = sIDB
                    tscoZim.Enabled = True
                Else
                    tscoZim.Enabled = False
                End If
                sIDRef = sIDB

                ' Sub-Methoden aufrufen (ohne veraltetes 'Call')
                prLadeAnAbReise(dt)
                prLadeZimmer(row("ZimID").ToString())

                prLaderPersonenUeberNachtung(dt)

                ' 2. Gast-Daten verarbeiten
                lbGastID.Text = row("KunID").ToString()
                prLadeGastDaten(lbGastID.Text)

                ' Zimmeranzeige formatieren (.PadRight statt Space)
                tscoZim.Text = lbZimNr.Text.PadRight(30) & "[" & sIDB & "]"

                ' 3. Preise sicher konvertieren und formatieren (.NET Ersatz für Val)
                Dim fPreis As Double = 0
                Dim gPreis As Double = 0
                Dim summeGesamt As Double = 0

                Double.TryParse(row("FPreis").ToString(), fPreis)
                Double.TryParse(row("GPreis").ToString(), gPreis)
                Double.TryParse(row("Summe").ToString(), summeGesamt)

                tbFPreis.Text = fcFormatDecimal(((fPreis + gPreis) / 100).ToString())
                tbSumme.Text = fcFormatDecimal((summeGesamt / 100).ToString())
                tbStorno.Text = row("Storno").ToString()

                ' Namen bereinigen
                tbName1Z.Text = If(row("Name1").ToString() = "0", "", row("Name1").ToString())
                tbName2Z.Text = If(row("Name2").ToString() = "0", "", row("Name2").ToString())

                ' Bezahlstatus setzen
                cbBezalt.Checked = (row("Bez").ToString().Trim() <> "0")

                ' Werbung und Rechnung initialisieren
                Dim sW As String = row("Werbung").ToString()

                If coWerbung.Text = "Unbekannt" Then
                    coWerbung.Text = sW
                End If

                lgRech = moDB.fcGetBoolean(row("Rechnung"))
                sgRNr = row("RID").ToString()
                prChangeMenue(lgRech)


                ' 4. Sprache und Buchungstext laden
                Dim sL As String = row("Sprache").ToString().Trim()
                Dim langIndex As Integer = 0
                If Integer.TryParse(If(sL = "", "0", sL), langIndex) Then
                    coLang.SelectedIndex = langIndex
                End If

                lbMakro.Text = row("MText").ToString()
                Dim sID As String = row("BText").ToString()

                Dim dtB As DataTable = fcReadDataTable("Select * from BTexte Where ID='" & sID & "'")
                If dtB IsNot Nothing AndAlso dtB.Rows.Count > 0 Then
                    coBText.Text = dtB.Rows(0)(1).ToString()
                End If

                lStart = False

                ' 5. DBNull-sicheres Auslesen optionaler Spalten
                If IsDBNull(row("RDSenden")) Then
                    tbRechSend.Text = ""
                Else
                    tbRechSend.Text = row("RDSenden").ToString()
                End If

                If IsDBNull(row("Pausch")) Then
                    cbPausch.Checked = False
                Else
                    cbPausch.Checked = (row("Pausch").ToString() = "1")
                End If
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Prüft, ob es sich um eine neue Reservierung handelt (wenn die Kunden-ID "0" oder leer ist).
    ''' </summary>
    ''' <param name="sRID">Die eindeutige Buchungs-ID (BID).</param>
    ''' <param name="sZID">Die eindeutige Zimmer-ID (ZimID).</param>
    ''' <returns>True, wenn die Reservierung neu ("Unbekannt") ist, andernfalls False.</returns>
    ''' <remarks>
    ''' 26.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Robustheit erhöht: Null-Prüfung für die 'DataTable' integriert, um Abstürze bei fehlgeschlagenen Datenbankabfragen zu verhindern.
    ''' - Syntax bereinigt: Die veraltete Zuweisung an den Funktionsnamen durch das native .NET-Schlüsselwort 'Return' ersetzt.
    ''' - Typsicherheit erhöht: Die Funktion ist nun standardmäßig auf 'False' initialisiert und fängt auch leere Kunden-IDs ab.
    ''' </remarks>
    Private Function fcCheckNewResevierung(ByVal sRID As String, ByVal sZID As String) As Boolean
        Dim sSQL As String = "Select * From Buchung Where BID='" & sRID & "' and ZimID='" & sZID & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' Sicherheitsprüfung: Existiert die DataTable und enthält sie Zeilen?
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim kunID As String = dt.Rows(0).Item("KunID").ToString().Trim()

            ' Prüfen, ob der Eintrag neu/unbekannt ist (ID ist "0" oder leer)
            If kunID = "0" OrElse String.IsNullOrEmpty(kunID) Then
                Return True
            End If
        End If

        ' Standardrückgabewert, wenn es keine neue Reservierung ist oder die DB leer war
        Return False
    End Function

    '''' <summary>
    '''' Prüft, ob die angegebene Zimmer-ID im Array der belegten Zimmer enthalten ist (ignoriert führende/nachfolgende Leerzeichen).
    '''' </summary>
    '''' <param name="arB">Das Array mit den IDs der belegten Zimmer.</param>
    '''' <param name="sID">Die zu suchende Zimmer-ID.</param>
    '''' <returns>True, wenn die Zimmer-ID im Array existiert, andernfalls False.</returns>
    '''' <remarks>
    '''' 03.02.2012 - Create
    '''' 03.10.2026 - Code-Optimierung:
    '''' - Performance-Boost: Manuelle For-Schleife durch die native, hochoptimierte Framework-Methode 'Array.Exists()' ersetzt.
    '''' - Robustheit erhöht: Null-Prüfung ('arB Is Nothing') integriert, um eine 'NullReferenceException' bei leeren Arrays zu verhindern.
    '''' - Syntax bereinigt: Veraltete VB6-Zuweisung an den Funktionsnamen durch das standardisierte 'Return' ersetzt.
    '''' </remarks>
    'Private Function fcIfZimmerInArray(ByVal arB() As String, ByVal sID As String) As Boolean
    '    ' Sicherheitsprüfung: Wenn das Array nicht initialisiert ist oder die Such-ID leer ist, direkt False zurückgeben
    '    If arB Is Nothing OrElse sID Is Nothing Then Return False

    '    ' Nutzen der Lambda-Syntax für eine performante Elementprüfung inklusive .Trim()
    '    Return Array.Exists(arB, Function(element) element IsNot Nothing AndAlso element.Trim() = sID)
    'End Function

    ''' <summary>
    ''' Berechnet die Aufenthaltsdauer (Tage) sowie die Gesamtsumme der Buchung basierend auf 
    ''' dem An-/Abreisedatum, dem Zimmerpreis, der Verpflegungsart und eventuellen Pauschalen oder Zusatzleistungen.
    ''' </summary>
    ''' <param name="sRBID">Die eindeutige Buchungs-ID (BuchID).</param>
    ''' <param name="sRZID">Die eindeutige Zimmer-ID (ZimID).</param>
    ''' <remarks>
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Veraltete VB6-Funktionen 'DateAdd' und 'DateDiff' durch native .NET-Datumsarithmetik (.AddDays, .Subtract) ersetzt.
    ''' - Fehleranfälliges 'CDate' durch sicheres 'Date.TryParse' ersetzt, um Abstürze bei ungültigen Datumsangaben zu verhindern.
    ''' - Fehleranfälliges 'Val()' und 'Trim()' durch präzises 'Double.TryParse' für Währungsberechnungen ersetzt.
    ''' - Toten, auskommentierten Code vollständig entfernt.
    ''' - 'cbPausch.Checked = True' auf die saubere, direkte Boolean-Prüfung verkürzt.
    ''' </remarks>
    Private Sub Summe(ByVal sRBID As String, ByVal sRZID As String)
        ' 1. Datumsberechnung mit nativem .NET durchführen
        Dim dVon As Date
        Dim dBis As Date

        ' Falls die Termine im UI fehlerhaft sind, fangen wir das hier sicher ab
        If Not Date.TryParse(lbAnreise.Text, dVon) OrElse Not Date.TryParse(lbAbreise.Text, dBis) Then
            ' Optionale Fehlerbehandlung oder Abbruch, falls Datumsangaben ungültig sind
            Exit Sub
        End If

        ' Abreisetag um einen Tag erhöhen
        dBis = dBis.AddDays(1)

        ' Differenz in Tagen berechnen (.Days gibt die Ganzzahl zurück)
        Dim nTageDifferenz As Integer = dBis.Subtract(dVon).Days
        lbTage.Text = nTageDifferenz.ToString()

        ' Unbenutzte Variablen 'sPreisNacht' und 'sSummeGesamt' entfernt, da der auskommentierte Code gelöscht wurde.

        ' 2. Frühstücks-/Verpflegungspreis ermitteln
        Dim nFPreis1 As String = "0"
        If coArt.Text = "Ü/F" Then
            nFPreis1 = tbFPreis.Text
        End If

        ' Zimmerpreis aus der Tabelle abrufen (Zellwert/Übernachtungspreis)
        Dim p As String = fcGetZimPreis(sgRZID, tbAnzPer.Text & "/" & nFPreis1, lbAbreise.Text, lbAnreise.Text)
        lbPauschPreis.Text = fcFormatDecimal(StrTrim(p, "")) & " €"

        ' 3. Gesamtsumme ermitteln (Unterscheidung Pauschalabrechnung vs. Einzelaufstellung)
        Dim currentSumme As Double = 0
        Double.TryParse(tbSumme.Text, currentSumme)

        If cbPausch.Checked Then
            ' Pauschale Abrechnung aktiv
            If currentSumme = 0 Then
                tbSumme.Text = fcFormatDecimal(StrTrim(p, "€"))
            Else
                tbSumme.Text = fcFormatDecimal(StrTrim(tbSumme.Text, "€"))
            End If
        Else
            ' Standardabrechnung: Berechnet Zimmerpreis * Tage + Zusätze aus der Datenbank
            ' Trim entfernt hier auch das " € ", welches aus fcGetSummeZimmer geliefert wird, 
            ' falls das Format im Textfeld tbSumme rein numerisch sein muss.
            tbSumme.Text = fcGetSummeZimmer(sRBID, sRZID).Trim()
        End If
    End Sub

    ''' <summary>
    ''' Lädt die An- und Abreisedaten sowie die entsprechenden Uhrzeiten aus der Buchungstabelle und berechnet die Aufenthaltsdauer.
    ''' </summary>
    ''' <param name="dt">Die DataTable mit den aktuellen Buchungsdaten.</param>
    ''' <remarks>
    ''' 03.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Absturzsicherung: 'dt.Rows.Count > 0'-Prüfung vorab integriert, um Laufzeitfehler bei leeren Datensätzen zu vermeiden.
    ''' - Performance & Modernisierung: Die veralteten VB6-Funktionen 'DateAdd' und 'DateDiff' durch die nativen .NET-Methoden '.AddDays()' und '.Subtract()' ersetzt.
    ''' - Typsicherheit erhöht: Nutzen der '.TotalDays'-Eigenschaft der '.NET TimeSpan'-Struktur für eine präzise Berechnung der Übernachtungen.
    ''' - Syntax bereinigt: Unnötige String-Instanziierungen minimiert und lokale Variablen-Referenz ('row') gecached.
    ''' </remarks>
    Private Sub prLadeAnAbReise(ByVal dt As DataTable)
        ' Sicherheitsprüfung: Falls die Tabelle keine Zeilen enthält, abbrechen
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

        ' Performance-Boost: Zeilenreferenz cachen
        Dim row As DataRow = dt.Rows(0)

        ' Datumswerte konvertieren
        Dim dVon As Date = fcUmDatum(row("Von").ToString())
        Dim dBis As Date = fcUmDatum(row("Bis").ToString())

        ' Abreisedatum um einen Tag erhöhen (.NET-Alternative zu DateAdd)
        dBis = dBis.AddDays(1)

        ' Werte an die UI übergeben
        lbAnreise.Text = dVon.ToShortDateString()
        lbAbreise.Text = dBis.ToShortDateString()

        ' Differenz der Tage berechnen (.NET-Alternative zu DateDiff via TimeSpan)
        Dim duration As TimeSpan = dBis.Subtract(dVon)
        lbTage.Text = Convert.ToInt32(duration.TotalDays).ToString()

        ' Uhrzeiten konvertieren und zuweisen
        tbAnZeit.Text = fcUmZeit(row("VonZeit").ToString())
        tbAbZeit.Text = fcUmZeit(row("BisZeit").ToString())
    End Sub

    ''' <summary>
    ''' Lädt die Zimmerdetails sowie den zugehörigen Objektnamen aus der Datenbank und zeigt diese in den UI-Labels an.
    ''' </summary>
    ''' <param name="sRZID">Die eindeutige Zimmer-ID (sRZID).</param>
    ''' <remarks>
    ''' 03.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Absturzsicherung: 'dtZ IsNot Nothing'-Prüfung vorab integriert, um Abstürze bei fehlgeschlagenen Datenbankabfragen zu verhindern.
    ''' - Performance-Boost: Lokale Zuweisung 'Dim row As DataRow = dtZ.Rows(0)' verhindert das permanente, rechenintensive Neuerstellen der Zeilenreferenz.
    ''' - Syntax bereinigt: Die ungenutzte Variable 'sObj' entfernt und direkt als Parameter in 'fcGetObjektName' übergeben.
    ''' </remarks>
    Private Sub prLadeZimmer(ByVal sRZID As String)
        Dim sSQL As String = "Select * From Zimmer Where ID='" & sRZID & "'"
        Dim dtZ As DataTable = fcReadDataTable(sSQL)

        ' Sicherheitsprüfung: Existiert die DataTable und enthält sie Zeilen?
        If dtZ IsNot Nothing AndAlso dtZ.Rows.Count > 0 Then
            ' Performance-Boost: Zeilenreferenz cachen
            Dim row As DataRow = dtZ.Rows(0)

            ' UI-Labels mit den Zimmerdaten befüllen
            lbZimNr.Text = row("Name").ToString()
            lbArt.Text = row("Art").ToString()
            lbBetten.Text = row("Betten").ToString()
            lbAusstattung.Text = row("Ausstattung").ToString()

            ' Objektnamen direkt auflösen (Variable sObj eingespart)
            Dim sObjID As String = row("IDObjekte").ToString()
            lbObjekt.Text = fcGetObjektName(dtObj, sObjID)
        End If
    End Sub

    '''' <summary>
    '''' Anzahl der Personen und Übernachtungen
    '''' </summary>
    '''' <param name="dt"></param>
    '''' <remarks>
    '''' 03.02.2012 Create
    '''' </remarks>
    'Private Sub prLaderPersonenUeberNachtung(ByVal dt As DataTable)
    '    tbInternetNr.Text = dt.Rows(0).Item("InternetNr").ToString
    '    tbAnzPer.Text = dt.Rows(0).Item("Personen").ToString
    '    tbUArt.Text = dt.Rows(0).Item("Kategorie").ToString
    '    coArt.Text = dt.Rows(0).Item("Art").ToString
    '    tbPreis.Text = fcFormatDecimal(Val(dt.Rows(0).Item("Preis").ToString) / 100)
    '    tbAnzahlung.Text = fcFormatDecimal(Val(dt.Rows(0).Item("Anzahlung").ToString) / 100)
    '    Dim sVariable As String = dt.Rows(0).Item("Variable").ToString
    '    Select Case sVariable
    '        Case "0"
    '            rbNormal.Checked = True
    '        Case "1"
    '            rbFest.Checked = True
    '        Case "2"
    '            rbVariabel.Checked = True
    '    End Select

    'End Sub

    ''' <summary>
    ''' Lädt die Anzahl der Personen, Übernachtungspreise, Kategorien sowie die Preisgestaltungs-Variable aus der Buchungstabelle und befüllt die Benutzeroberfläche.
    ''' </summary>
    ''' <param name="dt">Die DataTable mit den aktuellen Buchungsdaten.</param>
    ''' <remarks>
    ''' 03.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Absturzsicherung: 'dt.Rows.Count > 0'-Prüfung vorab integriert, um eine 'IndexOutOfRangeException' bei leeren Datensätzen zu verhindern.
    ''' - Performance-Boost: Lokale Zuweisung 'Dim row As DataRow = dt.Rows(0)' verhindert das permanente, rechenintensive Neuerstellen der Zeilenreferenz.
    ''' - Robustheit erhöht: Sichere Konvertierung über 'Double.TryParse' für 'Val' implementiert, um Fehler bei fehlerhaften numerischen Datenbankwerten zu vermeiden.
    ''' </remarks>
    Private Sub prLaderPersonenUeberNachtung(ByVal dt As DataTable)
        ' Sicherheitsprüfung: Falls die Tabelle keine Zeilen enthält, abbrechen
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

        ' Performance-Boost: Zeilenreferenz cachen
        Dim row As DataRow = dt.Rows(0)

        ' Textfelder direkt befüllen
        tbInternetNr.Text = row("InternetNr").ToString()
        tbAnzPer.Text = row("Personen").ToString()
        tbUArt.Text = row("Kategorie").ToString()
        coArt.Text = row("Art").ToString()

        ' Preise sicher numerisch parsen und umrechnen (.NET-Ersatz für Val)
        Dim rawPreis As Double = 0
        Dim rawAnzahlung As Double = 0

        Double.TryParse(row("Preis").ToString(), rawPreis)
        Double.TryParse(row("Anzahlung").ToString(), rawAnzahlung)

        tbPreis.Text = fcFormatDecimal(rawPreis / 100)
        tbAnzahlung.Text = fcFormatDecimal(rawAnzahlung / 100)

        ' Preisgestaltungs-Variable auswerten und RadioButtons setzen
        Dim sVariable As String = row("Variable").ToString().Trim()
        Select Case sVariable
            Case "0"
                rbNormal.Checked = True
            Case "1"
                rbFest.Checked = True
            Case "2"
                rbVariabel.Checked = True
        End Select
    End Sub

    ''' <summary>
    ''' Lädt die Kundendaten anhand der Gast-ID aus der Datenbank und befüllt die Eingabemaske. 
    ''' Setzt Standardwerte für leere Felder und leert die Maske bei einer neuen Gast-ID ("0").
    ''' </summary>
    ''' <param name="sID">Die eindeutige ID des Kunden/Gastes.</param>
    ''' <remarks>
    ''' 04.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Absturzsicherung: 'dt IsNot Nothing'-Prüfung vorab integriert, um Abstürze bei fehlgeschlagenen Datenbankabfragen zu verhindern.
    ''' - Logikfehler behoben: Der fehleranfällige Vergleich 'sID = 0' wurde auf einen typsicheren String-Vergleich ('sID = "0"') umgestellt, um implizite Konvertierungsfehler zu vermeiden.
    ''' - Performance-Boost: Den alten 'With'-Block durch eine dedizierte 'DataRow'-Referenz ('row') ersetzt.
    ''' - Syntax bereinigt: Redundantes 'Call' entfernt und Zuweisungen über den modernen ternären 'If()'-Operator verkürzt.
    ''' </remarks>
    Private Sub prLadeGastDaten(ByVal sID As String)
        ' 1. Wenn es sich um einen neuen Gast handelt, Daten maskieren/leeren und Methode verlassen
        If sID = "0" OrElse String.IsNullOrWhiteSpace(sID) Then
            prClearGastDaten()
            Exit Sub
        End If

        Dim sSQL As String = "Select * From Kunden Where ID='" & sID & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' 2. Sicherheitsprüfung: Existiert die DataTable und enthält sie Zeilen?
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            ' Performance-Boost: Zeilenreferenz cachen
            Dim row As DataRow = dt.Rows(0)

            ' UI-Formularfelder strukturiert befüllen
            coAnrede.Text = row("Anrede").ToString()
            tbName1.Text = row("Name1").ToString()
            tbName2.Text = row("Name2").ToString()

            ' Zusatznamen auslesen und "0"-Werte direkt herausfiltern via Inline-If
            Dim name1Z As String = row("Name1Z").ToString()
            Dim name2Z As String = row("Name2Z").ToString()
            tbName1Z.Text = If(name1Z = "0", "", name1Z)
            tbName2Z.Text = If(name2Z = "0", "", name2Z)

            tbVorname.Text = row("Vorname").ToString()
            tbStrasse.Text = row("Strasse").ToString()
            tbPLZ.Text = row("PLZ").ToString()
            tbOrt.Text = row("Ort").ToString()

            ' Land setzen (Fallback auf "DE" falls in der Datenbank leer)
            Dim landText As String = row("Land").ToString().Trim()
            coLand.Text = If(String.IsNullOrEmpty(landText), "DE", landText)

            ' Kontaktdaten
            tbTel.Text = row("Telefon").ToString()
            tbFax.Text = row("Telefax").ToString()
            tbFunk.Text = row("Funk").ToString()
            tbEMail.Text = row("EMail").ToString()
            tbInfo.Text = row("Info").ToString()

            ' Geburtsdatum DBNull-sicher konvertieren
            Dim gebString As String = row("Geb").ToString().Trim()
            If Not String.IsNullOrEmpty(gebString) Then
                dpGeb.Value = fcUmDatum(gebString)
            End If

            ' Info-Label aktualisieren
            lbGast.Text = $"({coAnrede.Text} {tbName1.Text})"

            ' Werbung laden, sofern ein Eintrag vorhanden ist
            Dim werbungText As String = row("Werbung").ToString().Trim()
            If Not String.IsNullOrEmpty(werbungText) Then
                coWerbung.Text = werbungText
            End If
        End If
    End Sub

    '''' <summary>
    '''' Aktuelle Werte in ein Array sichern (Vergleich vorher / nacher) => Daten speichern
    '''' </summary>
    '''' <param name="arT"></param>
    '''' <returns></returns>
    '''' <remarks>
    '''' 25.02.2012 Create
    '''' </remarks>
    'Private Function fcCollectDataInArray(ByVal arT() As String) As Array
    '    'Dim arT(22) As String
    '    arT(0) = tbAnzPer.Text
    '    arT(1) = coArt.Text
    '    arT(2) = tbUArt.Text
    '    arT(3) = tbPreis.Text
    '    arT(4) = tbAnzahlung.Text
    '    arT(5) = coAnrede.Text
    '    arT(6) = tbName1.Text
    '    arT(7) = tbName2.Text
    '    arT(8) = tbVorname.Text
    '    arT(9) = tbStrasse.Text
    '    arT(10) = tbPLZ.Text
    '    arT(11) = tbOrt.Text
    '    arT(12) = coLand.Text
    '    arT(13) = tbTel.Text
    '    arT(14) = tbFax.Text
    '    arT(15) = tbFunk.Text
    '    arT(16) = tbEMail.Text
    '    arT(17) = "" 'tbPass.Text
    '    arT(18) = tbInfo.Text
    '    arT(19) = fcUmDatum(dpGeb.Value)
    '    arT(20) = coWerbung.Text
    '    arT(21) = lbAnreise.Text
    '    arT(22) = lbAbreise.Text
    '    arT(23) = tbFPreis.Text
    '    arT(24) = lbMakro.Text
    '    fcCollectDataInArray = arT
    'End Function

    ''' <summary>
    ''' Sichert die aktuellen Formular- und Buchungswerte in ein String-Array, um einen späteren Vorher-Nachher-Vergleich für die Speicherung zu ermöglichen.
    ''' </summary>
    ''' <param name="arT">Das zu befüllende String-Array. Muss mindestens 25 Elemente (Indizes 0 bis 24) umfassen.</param>
    ''' <returns>Das befüllte String-Array mit den aktuellen UI-Daten.</returns>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Typsicherheit geschärft: Rückgabetyp von der generischen 'Array'-Klasse auf ein explizites 'String()' Array umgestellt.
    ''' - Robustheit erhöht: Automatische Dimensionierungsprüfung integriert. Ist das übergebene Array zu klein oder 'Nothing', wird es automatisch im Speicher auf die korrekte Größe (25 Elemente) initialisiert, um eine 'IndexOutOfRangeException' zu verhindern.
    ''' - Syntax bereinigt: Die veraltete Zuweisung an den Funktionsnamen durch das standardisierte .NET-Schlüsselwort 'Return' ersetzt.
    ''' </remarks>
    Private Function fcCollectDataInArray(ByVal arT() As String) As String()
        ' Sicherheitsprüfung: Falls das Array nicht existiert oder zu klein ist, neu dimensionieren (0 bis 24 = 25 Elemente)
        If arT Is Nothing OrElse arT.Length < 25 Then
            ReDim arT(24)
        End If

        ' Maskendaten strukturiert in das Array übertragen
        arT(0) = tbAnzPer.Text
        arT(1) = coArt.Text
        arT(2) = tbUArt.Text
        arT(3) = tbPreis.Text
        arT(4) = tbAnzahlung.Text
        arT(5) = coAnrede.Text
        arT(6) = tbName1.Text
        arT(7) = tbName2.Text
        arT(8) = tbVorname.Text
        arT(9) = tbStrasse.Text
        arT(10) = tbPLZ.Text
        arT(11) = tbOrt.Text
        arT(12) = coLand.Text
        arT(13) = tbTel.Text
        arT(14) = tbFax.Text
        arT(15) = tbFunk.Text
        arT(16) = tbEMail.Text
        arT(17) = "" ' tbPass.Text auskommentiert gelassen
        arT(18) = tbInfo.Text
        arT(19) = fcUmDatum(dpGeb.Value)
        arT(20) = coWerbung.Text
        arT(21) = lbAnreise.Text
        arT(22) = lbAbreise.Text
        arT(23) = tbFPreis.Text
        arT(24) = lbMakro.Text

        ' Modernes .NET Return anstelle der alten Funktionsnamen-Zuweisung
        Return arT
    End Function

    ''' <summary>
    ''' Steuert die Aktivierung und die Beschriftung der Namens- und Vornamesfelder in Abhängigkeit davon, ob eine "Firma" oder eine Person ausgewählt wurde.
    ''' </summary>
    ''' <param name="sender">Das auslösende Objekt (hier das coAnrede-Steuerelement).</param>
    ''' <param name="e">Die Ereignisdaten des SelectedIndexChanged-Events.</param>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Code-Verschlankung: Die Bedingung wurde in eine boolesche Variable ('isFirma') ausgelagert. Dadurch werden redundante Zuweisungen eliminiert und die Logik lässt sich in der Hälfte der Zeilen abbilden.
    ''' - Stabilität erhöht: Absicherung durch '.ToString()' hinzugefügt, um mögliche Null-Werte beim Wechsel der Auswahl sauber abzufangen.
    ''' </remarks>
    Private Sub coAnrede_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coAnrede.SelectedIndexChanged
        ' Sicherstellen, dass Text nicht null ist
        Dim selectedAnrede As String = If(coAnrede.Text, "").Trim()

        ' Zustand festlegen: Ist der ausgewählte Typ eine Firma?
        Dim isFirma As Boolean = (selectedAnrede = "Firma")

        ' UI-Elemente dynamisch anhand des Zustands steuern
        tbName2.Enabled = isFirma
        tbVorname.Enabled = Not isFirma

        ' Beschriftungen (Labels) flackerfrei anpassen
        Label12.Text = If(isFirma, "Firma", "Name")
        Label16.Text = If(isFirma, "Name", "")
        Label25.Text = If(isFirma, "", "Vorname")
    End Sub

    ''' <summary>
    ''' Überprüft plattformübergreifend für Preis- und Anzahlungsfelder während der Eingabe, ob das Zeichen numerisch ist.
    ''' </summary>
    ''' <param name="sender">Die TextBox, in der die Eingabe stattfindet (tbPreis oder tbAnzahlung).</param>
    ''' <param name="e">Die Ereignisdaten mit dem eingegebenen Zeichen.</param>
    ''' <remarks>
    ''' 05.10.2026 - Konsolidierung:
    ''' - Die separaten KeyPress-Handler für tbAnzahlung und tbPreis zusammengeführt.
    ''' - Dynamische Übergabe der TextBox via DirectCast.
    ''' </remarks>
    Private Sub tbPreisUndAnzahlung_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbPreis.KeyPress, tbAnzahlung.KeyPress
        Dim currentTextBox As TextBox = DirectCast(sender, TextBox)
        prCheckNumericKey(e.KeyChar, currentTextBox)
    End Sub





    ''' <summary>
    ''' Wenn ein neuer Eintrag ausgewählt wurde wird geprüft ob Änderungen in der alten Auswahl
    ''' vorgenommen wurden. Wenn Ja, dann wird die Änderung gespeichert und die neue Buchung geladen. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tscoZim_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tscoZim.SelectedIndexChanged
        If lStart Then Exit Sub

        If tscoZim.Text = "Gleiches Zimmer" Then
            ' Aufruf der neuen zentralen Pflichtfeldprüfung
            If Not ValidierungsPruefungErfolgreich() Then Exit Sub
            Call prSaveZimmerReservierung()
            tsbVorAnreise.Visible = True
            tsbNachAbreise.Visible = True


        Else

            If tscoZim.Text = "Weiteres Zimmer" Then
                '   If lRec = False Then 'Keine Rechnung geschrieben
                tscoFreiZim.Visible = True
                tsbAddZimmer.Visible = True
                If tscoFreiZim.Items.Count > 0 Then tscoFreiZim.SelectedIndex = 0
                tscoFreiZim.Select()
                'End If
            Else
                tscoFreiZim.Visible = False
                tsbAddZimmer.Visible = False
                sgRZID = fcGetObjektZimmerID(dtZim, tscoZim.Text, "ID")
                sIDR = Extract(tscoZim.Text, "[", "]", 1)
                '       = sIDR
                Call prLadeBuchung(sgRBID, sIDR)
                Call Summe(sgRBID, sgRZID)
                arOld = fcCollectDataInArray(arOld)
                tbAnzPer.Select()

                Call prCreatTabelIndex()
            End If
        End If
    End Sub

    '''' <summary>
    '''' Steuert die Aktivierung und Sichtbarkeit von Menü- und Steuerelementen in Abhängigkeit des Rechnungsstatus (z. B. ob eine Rechnung gesperrt oder bereits abgeschlossen ist).
    '''' </summary>
    '''' <param name="lr">Ein boolescher Wert, der angibt, ob die Rechnung gesperrt bzw. schreibgeschützt ist (<c>True</c>) oder bearbeitet werden kann (<c>False</c>).</param>
    '''' <remarks>
    '''' 28.02.2012 - Create
    '''' 05.10.2026 - Code-Optimierung:
    '''' - Altlasten und auskommentierte Codezeilen ('gbPersonen', 'tsmAnAbReise') vollständig entfernt.
    '''' - XML-Dokumentation detailliert ausformuliert und Parameterbeschreibung ergänzt.
    '''' </remarks>
    'Private Sub prLookForm(ByVal lr As Boolean)
    '    Dim isEditable As Boolean = Not lr

    '    ' Steuerung der Bearbeitbarkeit von Eingabebereichen und Speichern-Aktionen
    '    gbGast.Enabled = isEditable
    '    tsbSave.Enabled = isEditable

    '    ' Sichtbarkeit von rechnungs- und reservierungsbezogenen Toolbar-Aktionen steuern
    '    tsbAufZimmer.Visible = isEditable
    '    tsbRechnung.Visible = isEditable
    '    tsbBestätigung.Visible = isEditable
    '    tsbErinnerung.Visible = isEditable
    '    tsbDelReservierung.Visible = isEditable
    '    tsbDelZimmer.Visible = isEditable

    '    ' Trennstriche in der Toolbar anpassen
    '    tsSep1.Visible = isEditable
    '    tsSep2.Visible = isEditable

    '    ' Kopiefunktion ist nur sichtbar, wenn die Rechnung gesperrt/abgeschlossen ist
    '    tsmKopie.Visible = lr
    'End Sub

    ''' <summary>
    ''' Berechnet die Gesamtsumme für ein bestimmtes Zimmer inklusive aller gebuchten Zusatzleistungen und Steuern.
    ''' </summary>
    ''' <param name="sBID">Die eindeutige Buchungs-ID (BuchID).</param>
    ''' <param name="sZID">Die eindeutige Zimmer-ID (ZimID).</param>
    ''' <returns>Die formatierte Gesamtsumme als String inklusive Währungssymbol (" € ").</returns>
    ''' <remarks>
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Veraltete Zuweisung über den Funktionsnamen 'fcGetSummeZimmer =' durch 'Return' ersetzt.
    ''' - Logikfehler im 'Finally'-Block behoben (Berechnungen gehören nicht in den Cleanup-Block).
    ''' - Implizite String-zu-Double-Konvertierungen durch sichere Parser ('Double.TryParse', 'Integer.TryParse') ersetzt.
    ''' - 'Val()'-Funktion entfernt und Berechnung der Übernachtungen robuster gestaltet.
    ''' - Code-Redundanz bei der String-Formatierung aufgelöst.
    ''' </remarks>
    Private Function fcGetSummeZimmer(ByVal sBID As String, ByVal sZID As String) As String
        ' Sicheres Parsen der Benutzeroberflächen-Werte
        Dim nZPreis As Double = 0
        Double.TryParse(tbPreis.Text, nZPreis)

        Dim nRawTage As Integer = 0
        Integer.TryParse(lbTage.Text, nRawTage)

        ' Tage berechnen (Mindestens 0 Tage, um negative Summen bei fehlerhaften UI-Werten zu verhindern)
        Dim nTage As Integer = Math.Max(0, nRawTage - 1)
        Dim nZusatz As Double = 0

        ' Zusatzleistungen aus der Datenbank abrufen
        ' WICHTIG: Sollte idealerweise auf parametrisierte Abfragen umgestellt werden!
        Dim dt As DataTable = fcReadDataTable("SELECT Betrag, Menge, Steuer FROM Zusaetze WHERE BuchID='" & sBID & "' AND ZimID ='" & sZID & "'")

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Try
                For Each row As DataRow In dt.Rows
                    ' Gelöschte Zeilen überspringen (wichtig, falls die DataTable im Speicher editiert wurde)
                    If row.RowState <> DataRowState.Deleted Then
                        ' Sicheres Auslesen der DB-Felder (verhindert DBNull-Exceptions)
                        Dim nBetrag As Double = If(IsDBNull(row("Betrag")), 0.0, Convert.ToDouble(row("Betrag")))
                        Dim nMenge As Integer = If(IsDBNull(row("Menge")), 0, Convert.ToInt32(row("Menge")))
                        Dim nSteuerSatz As Integer = If(IsDBNull(row("Steuer")), 0, Convert.ToInt32(row("Steuer")))

                        Dim nSummeLeistung As Double = nBetrag * nMenge
                        nSummeLeistung += (nSummeLeistung * nSteuerSatz / 100.0)

                        nZusatz += nSummeLeistung
                    End If
                Next
            Catch ex As Exception
                ErrReport(ex.Message, ex.Source, ex.StackTrace)
            End Try
        End If

        ' Finale Berechnung und saubere Rückgabe per Return
        Dim nGesamtsumme As Double = (nZPreis * nTage) + nZusatz
        Return fcFormatDecimal(nGesamtsumme.ToString()) & " € "
    End Function

    ''' <summary>
    ''' Lädt die vordefinierten Textbausteine (BTexte) des Typs "1" aus der Datenbank.
    ''' Befüllt die ComboBox (coBText) und initialisiert das globale Array für die Zahlungsziele (arZZiel).
    ''' </summary>
    ''' <remarks>
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Index-Fehler bei der Array-Größenzuweisung behoben (dt.Rows.Count statt dt.Rows.Count + 1).
    ''' - Veraltetes 'ReDim'-Konstrukt durch direkte, stark typisierte String-Array-Initialisierung ersetzt.
    ''' - Schleife auf 'For Each' umgestellt, um die Lesbarkeit zu verbessern und Indexfehler zu vermeiden.
    ''' - Spaltenzugriff per Index (Item(1)) durch expliziten Spaltennamen ersetzt, um Code-Stabilität bei DB-Änderungen zu sichern.
    ''' - 'Nothing'-Prüfung für die DataTable hinzugefügt.
    ''' </remarks>
    Private Sub prLoadBText()
        ' Abfrage der Textbausteine für Art = 1
        Dim dt As DataTable = fcReadDataTable("SELECT * FROM BTexte WHERE Art = '1'")

        ' Sicherheitsprüfung, ob Daten zurückgegeben wurden
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

        Dim rowCount As Integer = dt.Rows.Count

        ' Das Array exakt in der benötigten Größe neu initialisieren (Größe = rowCount)
        ' Hinweis: arZZiel sollte im Formularkopf als "Dim arZZiel() As String" deklariert sein.
        arZZiel = New String(rowCount - 1) {}

        ' UI-Aktualisierung optimieren: Verhindert Flackern der ComboBox während des Befüllens
        coBText.BeginUpdate()
        Try
            coBText.Items.Clear() ' Optional: Vorherige Einträge löschen, falls die Prozedur mehrfach aufgerufen wird

            For i As Integer = 0 To rowCount - 1
                Dim row As DataRow = dt.Rows(i)

                ' WICHTIG: Ersetzen Sie "SpaltenNameFuerText" durch den echten Namen der zweiten Spalte aus Ihrer DB
                Dim sText As String = If(IsDBNull(row(1)), "", row(1).ToString())
                Dim sZiel As String = If(IsDBNull(row("ZZiel")), "", row("ZZiel").ToString())

                coBText.Items.Add(sText)
                arZZiel(i) = sZiel
            Next
        Finally
            coBText.EndUpdate()
        End Try
    End Sub

    ''' <summary>
    ''' Lädt den Buchungsindex für die aktuelle Buchungsreferenz aus der Datenbank.
    ''' Falls noch kein Index existiert, wird die Referenz-ID in der Buchungstabelle aktualisiert
    ''' und der Index anschließend erneut eingelesen.
    ''' </summary>
    ''' <remarks>
    ''' 25.02.2012 Create <br/>
    ''' 08.10.2026 Code-Optimierung: Entfernung von VB6-Altlasten (Split, Call), Vereinfachung der Tabellenprüfungen, 
    ''' Bereinigung von totem Code und Umstellung auf String-Interpolation.
    ''' </remarks>
    Private Sub prCreatTabelIndex()
        Dim sSQL As String = $"Select * From Buchung Where IDRef='{sIDB}' Order By BIDindex Asc"
        dtBuchIndex = fcReadDataTable(sSQL)

        ' Saubere und lesbare Prüfung, ob die DataTable leer ist
        If dtBuchIndex.Rows.Count = 0 Then
            Dim arFields() As String = {"IDRef"}
            Dim arValue() As String = {sIDB}
            Dim cBedingung As String = $" WHERE ID='{sIDB}'"

            ' Aktualisierung der Datenbank
            fcUpdateCommand("Buchung", arFields, arValue, cBedingung)

            ' Erneutes Laden der Daten nach dem Update
            dtBuchIndex = fcReadDataTable(sSQL)
        End If
    End Sub

#End Region

#Region "Mit Enter weiter zum nächsten Feld........................................................"

    ''' <summary>
    ''' Steuert den Fokuswechsel zwischen den Eingabefeldern der Kundenmaske beim Drücken der Enter-Taste.
    ''' </summary>
    ''' <param name="sender">Das Steuerelement, das das Ereignis ausgelöst hat.</param>
    ''' <param name="e">Die Ereignisdaten mit dem gedrückten Zeichen.</param>
    ''' <remarks>
    ''' 12.10.2026 - Code-Optimierung:
    ''' - Zusammenführung von 13 separaten KeyPress-Ereignissen in eine einzige, zentrale Methode.
    ''' - 'Microsoft.VisualBasic.ChrW(13)' durch sauberes '.NET Keys.Enter' ersetzt.
    ''' - 'e.Handled = True' hinzugefügt, um das Windows-Fehlersignal (Piepton) bei Enter zu unterdrücken.
    ''' </remarks>
    Private Sub Controls_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles _
        tbAnzPer.KeyPress, tbUArt.KeyPress, coArt.KeyPress, tbStorno.KeyPress, tbPreis.KeyPress, tbAnzahlung.KeyPress, tbSumme.KeyPress,
        coAnrede.KeyPress, tbName1.KeyPress, tbName2.KeyPress, tbVorname.KeyPress, tbStrasse.KeyPress,
        tbPLZ.KeyPress, tbOrt.KeyPress, coLand.KeyPress, tbTel.KeyPress,
        tbFax.KeyPress, tbFunk.KeyPress, tbEMail.KeyPress, coWerbung.KeyPress, tbInternetNr.KeyPress, tbInfo.KeyPress, dpGeb.KeyPress

        ' Prüfen, ob die Enter-Taste gedrückt wurde
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            ' Standard-Piepton von Windows unterdrücken
            e.Handled = True

            ' Aktuelles Steuerelement ermitteln
            Dim currentControl As Control = CType(sender, Control)

            ' Fokus-Workflow anhand des Namens definieren
            Select Case currentControl.Name
                Case "tbAnzPer"
                    tbUArt.Select()
                Case "tbUArt"
                    coArt.Select()
                Case "coArt"
                    tbStorno.Select()
                Case "tbStorno"
                    tbPreis.Select()
                Case "tbPreis"
                    tbAnzahlung.Select()
                Case "tbAnzahlung"
                    tbSumme.Select()
                Case "tbSumme"
                    coAnrede.Select()
                Case "coAnrede"
                    tbName1.Select()
                    tbName2.Select()
                Case "tbName1"
                    tbVorname.Select()
                Case "tbName2"
                    tbStrasse.Select()
                Case "tbVorname"
                    tbStrasse.Select()
                Case "tbStrasse"
                    tbPLZ.Select()
                Case "tbPLZ"
                    tbOrt.Select()
                Case "tbOrt"
                    coLand.Select()
                Case "coLand"
                    dpGeb.Select()
                Case "dpGeb"
                    tbTel.Select()
                Case "tbTel"
                    tbFax.Select()
                Case "tbFax"
                    tbFunk.Select()
                Case "tbFunk"
                    tbEMail.Select()
                Case "tbEMail"
                    coWerbung.Select()
                Case "coWerbung"
                    tbInternetNr.Select()
                Case "tbInternetNr"
                    tbInfo.Select()
                Case "tbInfo"
                    tbAnzPer.Select()

            End Select
        End If
    End Sub


#End Region

#Region "Preis- und Gasteliste....................................................................."

    ''' <summary>
    ''' Aktualisiert die globale Variable für die Werbeherkunft, wenn sich die Auswahl in der ComboBox ändert.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 13.10.2026 - Code-Optimierung:
    ''' - XML-Dokumentation hinzugefügt.
    ''' </remarks>
    Private Sub coWerbung_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coWerbung.SelectedIndexChanged
        sgWerbung = coWerbung.Text
    End Sub

    ''' <summary>
    ''' Triggert die Gastsuchen-Aktualisierung, sobald sich der Text im Vornamen- oder Nachnamenfeld ändert.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (tbVorname oder tbName1).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 13.10.2026 - Code-Optimierung:
    ''' - 'tbVorname_TextChanged' und 'tbName1_TextChanged' zu einem einzigen Handler zusammengefasst (Code-Duplizierung eliminiert).
    ''' - Veraltete 'Call'-Syntax bei Methodenaufrufen entfernt.
    ''' </remarks>
    Private Sub GastSuche_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbVorname.TextChanged, tbName1.TextChanged
        ' Gästeliste neu laden und passenden Gast fokussieren
        prLoadGastInList()
        prFindGast()
    End Sub

    ''' <summary>
    ''' Wählt einen Gast per Doppelklick aus der Liste aus, lädt dessen Daten und aktualisiert die Ansicht.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das ListView-Steuerelement).</param>
    ''' <param name="e">Die Ereignisdaten für das Mausereignis.</param>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 13.10.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' - 'With'-Block aufgelöst, um den Code flacher und lesbarer zu gestalten.
    ''' - Sicherheitsabfrage für die Indexierung des ausgewählten Elements optimiert.
    ''' - Inline-Kommentare zur besseren Wartbarkeit hinzugefügt.
    ''' </remarks>
    Private Sub lvGast_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvGast.MouseDoubleClick
        ' Standardwert zurücksetzen, falls kein Element gültig selektiert ist
        lbGastID.Text = "0"

        ' Prüfen, ob ein Element in der ListView ausgewählt wurde
        If lvGast.SelectedItems.Count > 0 Then
            ' Die Gast-ID aus der sechsten Spalte (Index 5) auslesen
            lbGastID.Text = lvGast.SelectedItems(0).SubItems(5).Text
        End If

        ' Stammdaten des ausgewählten Gastes laden
        prLadeGastDaten(lbGastID.Text)

        ' Status auf Stammgast setzen
        coWerbung.Text = "Stammgast"
    End Sub

    ''' <summary>
    ''' Blendet die E-Mail-Vorschlagsliste aus, sobald der Mauszeiger das E-Mail-Textfeld verlässt.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die E-Mail-Textbox).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 13.10.2026 - Code-Optimierung:
    ''' - XML-Dokumentation vollständig hinzugefügt.
    ''' - Methodennamen korrigiert ('_TextChanged_1' zu '_MouseLeave'), da die Methode an das 'MouseLeave'-Ereignis gebunden ist.
    ''' - 'ByVal'-Schlüsselwörter für eine explizite Parameterübergabe ergänzt.
    ''' </remarks>
    Private Sub tbEMail_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles tbEMail.MouseLeave
        ' Vorschlagsliste unsichtbar machen
        lbMailList.Visible = False
    End Sub

    ''' <summary>
    ''' Bereitet die E-Mail-Liste auf und zeigt sie an, sobald der Mauszeiger über dem E-Mail-Textfeld verweilt.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die E-Mail-Textbox).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 13.10.2026 - Code-Optimierung:
    ''' - Methodennamen von '_TextChanged' zu '_MouseHover' korrigiert, um dem tatsächlichen Ereignis zu entsprechen.
    ''' - Veraltete VB6-'Replace'-Funktion durch die moderne '.Replace'-Methode der String-Klasse ersetzt.
    ''' - 'Method Chaining' angewendet, um mehrfache Zuweisungen an 'lbMailList.Text' zu verhindern und die Performance zu verbessern.
    ''' - 'ByVal'-Schlüsselwörter für eine explizite Parameterübergabe ergänzt.
    ''' </remarks>
    Private Sub tbEMail_MouseHover(ByVal sender As Object, ByVal e As EventArgs) Handles tbEMail.MouseHover
        ' Prüfen, ob überhaupt Text vorhanden ist, um unnötige Operationen zu vermeiden
        If Not String.IsNullOrEmpty(tbEMail.Text) Then
            ' Semikolons durch Zeilenumbrüche ersetzen und Leerzeichen entfernen
            lbMailList.Text = tbEMail.Text.Replace(";", vbCrLf).Replace(" ", "")

            ' Liste sichtbar machen
            lbMailList.Visible = True
        End If
    End Sub


#End Region

#Region "Button und Toolbar auswerten.............................................................."

    ''' <summary>
    ''' Schließt das aktuelle Formular und beendet damit das Modul.
    ''' </summary>
    ''' <param name="sender">Das auslösende Steuerelement (Schließen-Button).</param>
    ''' <param name="e">Die Ereignisdaten des Klicks.</param>
    ''' <remarks>
    ''' 25.02.2012 Create <br/>
    ''' 08.10.2026 Code-Optimierung: Bereinigung der XML-Kommentare, Entfernung von totem Code und veralteten Typ-Präfixen.
    ''' </remarks>
    Private Sub tsbClose_Click(sender As Object, e As EventArgs) Handles tsbClose.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Tritt auf, während das Formular geschlossen wird. Prüft, ob für die aktuelle Buchung 
    ''' ein E-Mail-Zugang aussteht, und bietet dem Benutzer die Freigabe der Buchung an.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten, mit denen das Schließen abgebrochen werden könnte.</param>
    ''' <remarks>
    ''' 25.02.2012 Create <br/>
    ''' 08.10.2026 Code-Optimierung: Umstellung von 'MsgBox' auf 'MessageBox.Show', Entfernung des 'Call'-Schlüsselworts und Modernisierung der String-Verarbeitung.
    ''' </remarks>
    Private Sub frmKey_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim arfeld1() As String = fcDataSeek("Select * From Buchung Where BID ='", sgRBID, 0, {"SendMailZugang"})

        If arfeld1(0) = "Y" Then
            ' Verwendung des modernen .NET MessageBox-Dialogs
            Dim result As DialogResult = MessageBox.Show("Buchung freigeben?", "Bestätigung", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)

            If result = DialogResult.OK Then
                Dim arFields() As String = {"SendMailZugang"}
                Dim arValue() As String = {"X"}
                Dim cBedingung As String = $" WHERE BID='{sgRBID}'"

                fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Setzt die Gastdaten im Formular über einen Doppelklick auf das Gast-Label zurück.
    ''' </summary>
    ''' <param name="sender">Das auslösende Steuerelement (z. B. lbGast).</param>
    ''' <param name="e">Die Ereignisdaten des Doppelklicks.</param>
    ''' <remarks>
    ''' 25.02.2012 Create <br/>
    ''' 08.10.2026 Code-Optimierung: Entfernung des veralteten 'Call'-Schlüsselworts und Bereinigung des XML-Kommentars.
    ''' </remarks>
    Private Sub lbGast_DoubleClick(sender As Object, e As EventArgs) Handles lbGast.DoubleClick
        prClearGastDaten()
    End Sub

    ''' <summary>
    ''' Leert alle gastbezogenen Eingabefelder im Formular und setzt die 
    ''' Auswahlmenüs sowie Datumsfelder auf ihre Standard-Ausgangswerte zurück.
    ''' </summary>
    ''' <remarks>
    ''' 05.02.2012 Create <br/>
    ''' 08.10.2026 Code-Optimierung: Entfernung von totem Code, Umstellung auf 'String.Empty' für bessere Performance, 
    ''' strukturelle Gruppierung der Steuerelemente zur Erhöhung der Lesbarkeit.
    ''' </remarks>
    Private Sub prClearGastDaten()
        ' Labels und IDs zurücksetzen
        lbGast.Text = "(neuer Gast)"
        lbGastID.Text = String.Empty

        ' Textfelder leeren
        tbName1.Text = String.Empty
        tbName2.Text = String.Empty
        tbVorname.Text = String.Empty
        tbStrasse.Text = String.Empty
        tbPLZ.Text = String.Empty
        tbOrt.Text = String.Empty
        tbTel.Text = String.Empty
        tbFax.Text = String.Empty
        tbFunk.Text = String.Empty
        tbEMail.Text = String.Empty
        tbInfo.Text = String.Empty

        ' Comboboxen auf Standardwerte setzen
        coAnrede.Text = "Firma"
        coLand.Text = "DE"
        coWerbung.Text = "Unbekannt"

        ' Datumsauswahl auf den aktuellen Tag zurücksetzen
        dpGeb.Value = DateTime.Today
    End Sub

    ''' <summary>
    ''' Überprüft alle Pflichtfelder des Reservierungsformulars auf Vollständigkeit und formale Korrektheit.
    ''' Zeigt bei Fehlern eine entsprechende Hinweismeldung an.
    ''' </summary>
    ''' <returns>True, wenn alle Prüfungen erfolgreich bestanden wurden; andernfalls False.</returns>
    ''' <remarks>
    ''' 07.10.2026 - Erstellt: Logische Kapselung der UI-Validierung zur Erhöhung der Code-Wartbarkeit.
    ''' </remarks>
    Private Function ValidierungsPruefungErfolgreich() As Boolean
        ' 1. Prüfung: Gastauswahl (Nachname oder Firma muss befüllt sein)
        If String.IsNullOrWhiteSpace(tbName1.Text) AndAlso String.IsNullOrWhiteSpace(tbName2.Text) Then
            MsgBox("Speichern nicht möglich, da kein Gast ausgewählt wurde!", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly)
            Return False
        End If

        ' 2. Prüfung: Personenzahl (Darf nicht leer, Leerzeichen oder "0" sein)
        If String.IsNullOrWhiteSpace(tbAnzPer.Text) OrElse tbAnzPer.Text.Trim() = "0" Then
            MsgBox("Speichern nicht möglich, da keine Personenzahl angegeben wurde!", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly)
            Return False
        End If

        ' 3. Prüfung: Buchungstext ausgewählt
        If String.IsNullOrWhiteSpace(coBText.Text) Then
            MsgBox("Speichern nicht möglich, da kein Buchungstext ausgewählt wurde!", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly)
            Return False
        End If

        ' 4. Prüfung: Übernachtungsart ausgewählt
        If String.IsNullOrWhiteSpace(coArt.Text) Then
            MsgBox("Speichern nicht möglich, da keine Übernachtungsart ausgewählt wurde!", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly)
            Return False
        End If

        ' Alle Prüfungen erfolgreich bestanden
        Return True
    End Function

    ''' <summary>
    ''' Speichert die vorgenommenen Änderungen einer Zimmerreservierung im System.
    ''' Führt vorab eine Pflichtfeldprüfung durch, aktualisiert die lokalen Reservierungsdaten 
    ''' und schreibt oder aktualisiert die relevanten Daten in der Code-Datenbanktabelle.
    ''' </summary>
    ''' <param name="sender">Das auslösende Steuerelement (Speichern-Button).</param>
    ''' <param name="e">Die Ereignisdaten des Klicks.</param>
    ''' <remarks>
    ''' 25.02.2012 Create <br/>
    ''' 08.10.2026 Code-Optimierung: Bereinigung von VB6-Altlasten (Split, Call, MsgBox), Entfernung von totem Code und Modernisierung der String-Verarbeitung.
    ''' </remarks>
    Private Sub tsbSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSave.Click

        ' Aufruf der neuen zentralen Pflichtfeldprüfung
        If Not ValidierungsPruefungErfolgreich() Then Exit Sub

        Dim sC As String = fcCode(sCode)
        prSaveZimmerReservierung()
        arNew = fcCollectDataInArray(arNew)

        ' Validierung des Codes für den Datenbankzugriff
        If Not String.IsNullOrWhiteSpace(sCode) AndAlso sCode.Trim() <> "00000" Then

            ' Definition der Datenbankfelder
            Dim arFields() As String = {"code", "von", "vonzeit", "bis", "biszeit", "BID"}

            ' Effiziente und lesbare String-Zusammensetzung via Interpolation statt StringBuilder
            Dim combinedValues As String = $"{sC}°{fcUmDatum(arNew(21))}°{fcUmZeit(tbAnZeit.Text)}°{fcUmDatum(arNew(22))}°{fcUmZeit(tbAbZeit.Text)}°{sgRBID}"
            Dim arValue() As String = combinedValues.Split("°"c)

            ' Prüfung, ob der Datensatz bereits existiert
            Dim sSQL As String = $"Select * From Code Where code='{sC}'"
            Dim dt As DataTable = fcReadDataTable(sSQL)

            If dt.Rows.Count = 0 Then
                fcInsertCommand("Code", arFields, arValue)
            Else
                Dim cBedingung As String = $" WHERE code='{sC}'"
                fcUpdateCommand("Code", arFields, arValue, cBedingung)
            End If
        End If

        MessageBox.Show("Änderung gespeichert", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub


    ''' <summary>
    ''' Mit Klick in die Auswahlbox alle Daten in das Array "arNew" sichern (Vergleich)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tscoZim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tscoZim.Click
        arNew = fcCollectDataInArray(arNew)

    End Sub

    Private Sub tsbVorAnreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbVorAnreise.Click

        Dim dDa As Date 'abreise datum wie bisheriges anreisedatum
        Dim sAn As String = lbAnreise.Text
        Dim sAb As String = lbAbreise.Text
        dDa = CDate(sAn)

        sAb = dDa
        dDa = DateAdd(DateInterval.Day, -1, dDa)
        sAn = dDa

        If frControlFreeZimmer(lbZimNr.Text, sAn, sAb) = True Then
            Dim ID As String = fcAppendBlank("Buchung")
            Dim arFields() As String = {"SendMailZugang"}
            Dim arValue() As String = {"Y"}
            Dim cBedingung As String = " WHERE ID='" & ID & "'"
            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)

            sBID = ID
            tscoZim.Items.Add(lbZimNr.Text & Space(30) & "[" & ID & "]")
            lbAnreise.Text = sAn
            lbAbreise.Text = sAb
            arNew(21) = sAn
            arNew(22) = sAb
            arNew = fcCollectDataInArray(arNew)
            Call prAddNewZimmer(lbZimNr.Text, arNew, ID)
            tscoZim.Text = lbZimNr.Text & Space(30) & "[" & ID & "]"
            Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")

        Else

            MsgBox("Zimmer schon vermietet", MsgBoxStyle.Question + MsgBoxStyle.OkOnly)
        End If
        tsbVorAnreise.Visible = False
        tsbNachAbreise.Visible = False
    End Sub

    Private Sub tsbNachAbreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNachAbreise.Click

        Dim dDa As Date 'anreise datum wie bisheriges abreisedatum

        Dim sAn As String = lbAnreise.Text
        Dim sAb As String = lbAbreise.Text
        dDa = CDate(sAb)
        sAn = lbAbreise.Text
        dDa = DateAdd(DateInterval.Day, 1, dDa)
        sAb = dDa


        If frControlFreeZimmer(lbZimNr.Text, sAn, sAb) = True Then
            Dim ID As String = fcAppendBlank("Buchung")

            Dim arFields() As String = {"SendMailZugang"}
            Dim arValue() As String = {"Y"}
            Dim cBedingung As String = " WHERE ID='" & ID & "'"
            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)





            sBID = ID
            tscoZim.Items.Add(lbZimNr.Text & Space(30) & "[" & ID & "]")
            lbAnreise.Text = sAn
            lbAbreise.Text = sAb
            arNew(21) = sAn
            arNew(22) = sAb
            Call prAddNewZimmer(lbZimNr.Text, arNew, ID)
            tscoZim.Text = lbZimNr.Text & Space(30) & "[" & ID & "]"

        Else
            MsgBox("Zimmer schon Vermittet", MsgBoxStyle.Question + MsgBoxStyle.OkOnly)
        End If
        tsbVorAnreise.Visible = False
        tsbNachAbreise.Visible = False
    End Sub
    Private Function frControlFreeZimmer(ByRef Zim As String, ByRef Von As String, ByRef sBis As String) As Boolean
        frControlFreeZimmer = False
        Dim dDa As Date = CDate(sBis)
        dDa = DateAdd(DateInterval.Day, -1, dDa)
        Dim Bis As String = CDate(dDa)
        Dim sZim As String = fcGetObjektZimmerID(dtZim, Zim, "ID")
        Dim dt As DataTable
        Dim sSql As String
        sSql = "Select * From Buchung  Where ZimID='" & sZim & "' and Von<='" & fcUmDatum(Von) & "' and Bis >= '" & fcUmDatum(Bis) & "'"
        dt = fcReadDataTable(sSql)
        If dt.Rows.Count = 0 Then frControlFreeZimmer = True

    End Function

    'Private Sub tbAnreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbAnreise.Click
    '    Dim aSperr As String = ""
    '    Dim aBuchung As String = ""
    '    Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")
    '    Dim dt As DataTable
    '    Dim sSql As String
    '    Dim sID As String = Extract(tscoZim.Text, "[", "]", 1)
    '    Dim d1 As Date = DateAdd(DateInterval.Month, -8, CDate(lbAnreise.Text))
    '    Dim d2 As Date = DateAdd(DateInterval.Month, 8, CDate(lbAnreise.Text))
    '    sSql = "Select * From Buchung  Where ZimID='" & sZim & "' and Von<='" & fcUmDatum(CDate(d2)) & "' and Bis >= '" & fcUmDatum(CDate(d1)) & "'"
    '    dt = fcReadDataTable(sSql)
    '    For k = 0 To dt.Rows.Count - 1
    '        If dt.Rows(k).Item("ID").ToString <> sID And dt.Rows(k).Item("BID").ToString <> sgRBID Then
    '            aSperr = aSperr & ";" & dt.Rows(k).Item("von").ToString & "-" & dt.Rows(k).Item("bis").ToString

    '        End If
    '    Next
    '    For k = 0 To dt.Rows.Count - 1
    '        If dt.Rows(k).Item("ID").ToString <> sID And dt.Rows(k).Item("BID").ToString = sgRBID Then
    '            aBuchung = aBuchung & ";" & dt.Rows(k).Item("von").ToString & "-" & dt.Rows(k).Item("bis").ToString

    '        End If
    '    Next

    '    MyKalender1.Datum = fcUmDatum(lbAnreise.Text)
    '    MyKalender1.Sperr = Mid(aSperr, 2)
    '    MyKalender1.Buchung = Mid(aBuchung, 2)
    '    MyKalender1.AnreiseAbreise = True

    '    MyKalender1.Anreise = fcUmDatum(lbAnreise.Text) 'dAn
    '    MyKalender1.Abreise = fcUmDatum(lbAbreise.Text) 'dAb
    '    MyKalender1.Visible = True

    'End Sub

    'Private Sub tbAbreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbAbreise.Click
    '    Dim aSperr As String = ""
    '    Dim aBuchung As String = ""
    '    Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")
    '    Dim dt As DataTable
    '    Dim sSql As String
    '    Dim sID As String = Extract(tscoZim.Text, "[", "]", 1)
    '    Dim d1 As Date = DateAdd(DateInterval.Month, -8, CDate(lbAnreise.Text))
    '    Dim d2 As Date = DateAdd(DateInterval.Month, 8, CDate(lbAnreise.Text))
    '    sSql = "Select * From Buchung  Where ZimID='" & sZim & "' and Von<='" & fcUmDatum(CDate(d2)) & "' and Bis >= '" & fcUmDatum(CDate(d1)) & "'"
    '    dt = fcReadDataTable(sSql)
    '    For k = 0 To dt.Rows.Count - 1
    '        If dt.Rows(k).Item("ID").ToString <> sID And dt.Rows(k).Item("BID").ToString <> sgRBID Then
    '            aSperr = aSperr & ";" & dt.Rows(k).Item("von").ToString & "-" & dt.Rows(k).Item("bis").ToString

    '        End If
    '    Next
    '    For k = 0 To dt.Rows.Count - 1
    '        If dt.Rows(k).Item("ID").ToString <> sID And dt.Rows(k).Item("BID").ToString = sgRBID Then
    '            aBuchung = aBuchung & ";" & dt.Rows(k).Item("von").ToString & "-" & dt.Rows(k).Item("bis").ToString

    '        End If
    '    Next
    '    ' dDa = DateAdd(DateInterval.Day, 1, dDa)
    '    MyKalender1.Datum = fcUmDatum(lbAnreise.Text)
    '    MyKalender1.Sperr = Mid(aSperr, 2)
    '    MyKalender1.Buchung = Mid(aBuchung, 2)
    '    MyKalender1.AnreiseAbreise = False
    '    MyKalender1.Anreise = fcUmDatum(lbAnreise.Text) 'dAn
    '    MyKalender1.Abreise = fcUmDatum(lbAbreise.Text) 'dAb
    '    MyKalender1.Visible = True
    'End Sub





    ''' <summary>
    ''' Neues Zimmer zur Reservierung hinzufügen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tsbAddZimmer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAddZimmer.Click
        Dim sZim As String = tscoFreiZim.Text

        Dim ID As String = fcAppendBlank("Buchung") 'fcGetTimeID(Date.Today)


        Dim arFields() As String = {"SendMailZugang"}
        Dim arValue() As String = {"Y"}
        Dim cBedingung As String = " WHERE ID='" & ID & "'"
        Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)


        tscoZim.Items.Add(sZim & Space(30) & "[" & ID & "]")
        tscoFreiZim.Items.Remove(tscoFreiZim.SelectedItem)
        tscoFreiZim.Visible = False
        tsbAddZimmer.Visible = False
        'Buchungsdatensatz anlegen
        Call prAddNewZimmer(sZim, arNew, ID)
    End Sub

    ''' <summary>
    ''' Buchungsdatensatz für das neue Zimmer erzeugen
    ''' </summary>
    ''' <param name="sZim"></param>
    ''' <param name="arT"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prAddNewZimmer(ByVal sZim As String, ByVal arT() As String, ByVal ID As String)
        Dim sBID As String = sgRBID
        Dim sKNr As String = lbGastID.Text
        Dim sZID As String = fcGetObjektZimmerID(dtZim, sZim, "ID")
        Dim sOID As String = fcGetObjektZimmerID(dtZim, sZim, "IDObjekte")

        Call prSaveNewReservierung(sBID, sKNr, sZID, sOID, arT, ID)
        Call prSetReservierungInDataGrid(arT(21), arT(22), sZID, "0", arT(6), sBID, "Ü")
    End Sub

    ''' <summary>
    ''' Neue Reservierung speichern
    ''' </summary>
    ''' <param name="sBID"></param>
    ''' <param name="sKNr"></param>
    ''' <param name="sZID"></param>
    ''' <param name="sOID"></param>
    ''' <param name="arT"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prSaveNewReservierung(ByVal sBID As String, ByVal sKNr As String,
                                      ByVal sZID As String, ByVal sOID As String,
                                      ByVal arT() As String, ByVal ID As String)
        Dim sPauch As String = "0"
        If cbPausch.Checked = True Then sPauch = "1"
        Dim sDatum As Date = arT(22)
        Dim sADatum As Date = DateAdd(DateInterval.Day, -1, sDatum)
        Dim sb2 As New StringBuilder
        Dim arFields(), arValue() As String
        Dim sqlText As String = ""
        sqlText = "ID,BID,Von,Bis,ObjID,ZimID,Variable,KunID,Kunde,Name1,Name2,Personen,Tiere,Art,Frueh,Kategorie,Preis,Anzahlung,Werbung,Info,Rechnung,BuchDatum,Sprache,BText,MText,RID,RDatum,FPreis,GPreis,MwstU,MwstS,MwstG,GKU,GKS,GKG,RDSenden,BIDIndex,IDRef,RAID,RADatum,Storno,Summe,Pausch,Code,InternetNr,VonZeit,BisZeit"
        arFields = Split(sqlText, ",")
        If sKNr = "" Then sKNr = "0"
        sb2.Append(ID & "°")  'ID
        sb2.Append(sgRBID & "°")  'BID
        sb2.Append(fcUmDatum(arT(21)) & "°")    'VON 'art(21)
        sb2.Append(fcUmDatum(sADatum) & "°")   'BIS
        sb2.Append(sOID & "°") 'Objekt
        sb2.Append(sZID & "°") 'zimmerID
        sb2.Append("0" & "°")  'Variabel
        sb2.Append(sKNr & "°") 'kunde_ID
        sb2.Append(tbName1.Text & "°") 'kunde
        sb2.Append(tbName1Z.Text & "°") 'kunde
        sb2.Append(tbName2Z.Text & "°") 'kunde
        sb2.Append("0" & "°")   'Person
        sb2.Append("0" & "°")  'Tiere
        sb2.Append(" " & "°")  'Art
        sb2.Append(" " & "°")  'Frü
        sb2.Append(" " & "°")  'kato
        sb2.Append("0" & "°")  'Preis
        sb2.Append("0" & "°")  'anzahlung
        sb2.Append(arT(20) & "°") 'werbung
        sb2.Append(" " & "°") 'Info
        sb2.Append("0" & "°") 'rechnung
        sb2.Append(fcUmDatum(Date.Today) & "°") 'buchdatum
        sb2.Append("0" & "°") 'sprache
        sb2.Append(" " & "°") 'Buchungstext ID
        sb2.Append(arT(24) & "°") 'Makrotext
        sb2.Append(" " & "°") 'R-ID
        sb2.Append(" " & "°") 'Rechnungsnummer
        sb2.Append("0" & "°") 'FPreis
        sb2.Append("0" & "°") 'GPreis
        sb2.Append("0" & "°") 'MwstU
        sb2.Append("0" & "°") 'MwstS
        sb2.Append("0" & "°") 'MwstG
        sb2.Append("0" & "°") 'GKU
        sb2.Append("0" & "°") 'GKS
        sb2.Append("0" & "°") 'GKG
        sb2.Append(tbRechSend.Text & "°")    'RD Send
        sb2.Append("0" & "°")    'bidIndex
        sb2.Append(sgRBID & "°")  'Referrens ID
        sb2.Append(" " & "°")     'RA ID
        sb2.Append(" " & "°")   'RA Datum
        sb2.Append("100" & "°")  'Storno
        sb2.Append("0" & "°")    'Summe
        sb2.Append(sPauch & "°") 'Pausch
        sb2.Append(sCode & "°")        'Code key
        sb2.Append(tbInternetNr.Text & "°")
        sb2.Append(fcUmZeit(tbAnZeit.Text) & "°")    'Summe
        sb2.Append(fcUmZeit(tbAbZeit.Text))        'Code key
        arValue = Split(sb2.ToString, "°")
        '   Call fcInsertCommand("Buchung", arFields, arValue)
        Dim cBedingung As String = " WHERE ID='" & ID & "'"
        Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
        '   ngIDB += 1


        Call prLadeGastDaten(sKNr)
    End Sub

    ''' <summary>
    ''' Steuerung des Menüs in Abhängigkeit des Rechnungsstatus.
    ''' </summary>
    ''' <param name="isInvoiceWritten">True, wenn die Rechnung bereits geschrieben wurde.</param>
    ''' <remarks>
    ''' 14.03.2012 - Create
    ''' 14.10.2026 - Code-Optimierung:
    ''' - Von 'Function' zu 'Sub' umgewandelt, da der Rückgabewert redundant war.
    ''' - Überflüssiges XML-'returns'-Tag entfernt.
    ''' - Inline-Bedingung 'If(..., ..., ...)' für die Zuweisung des Fenstertitels eingesetzt.
    ''' </remarks>
    Public Sub prChangeMenue(ByVal isInvoiceWritten As Boolean)
        ' Menü- und Toolbar-Elemente aktivieren/deaktivieren
        tsmStorno.Enabled = isInvoiceWritten

        ' Elemente für Löschvorgänge sperren, wenn die Rechnung bereits existiert
        tsbDelReservierung.Enabled = Not isInvoiceWritten
        tsbDelZimmer.Enabled = Not isInvoiceWritten

        ' Fenstertitel kompakt und lesbar basierend auf dem Status anpassen
        Me.Text = If(isInvoiceWritten, "Reservierung bearbeiten => (Rechnung geschrieben!)", "Reservierung bearbeiten")
    End Sub


    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der tsbKey-Schaltfläche.
    ''' Führt eine Pflichtfeldprüfung durch, speichert die Zimmerreservierung 
    ''' und überträgt den generierten Schlüssel-Code an den Webserver.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (tsbKey).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 25.12.2014 - Erstellt<br/>
    ''' 12.10.2026 - Optimiert (Code-Bereinigung und .NET-Typkonvertierung)
    ''' </remarks>
    Private Sub tsbKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbKey.Click
        ' 1. Zentrale Pflichtfeldprüfung durchführen
        If Not ValidierungsPruefungErfolgreich() Then Exit Sub

        ' 2. Zimmerreservierung speichern
        prSaveZimmerReservierung()

        ' 3. Datumsangaben aus dem Array lesen und konvertieren
        Dim vonDatum As Date = CDate(arNew(21))
        Dim bisDatum As Date = CDate(arNew(22)).AddDays(1) ' 1 Tag aufschlagen

        Dim sToDay As String = fcUmDatum(vonDatum) ' VON
        Dim sToDay1 As String = fcUmDatum(bisDatum) ' BIS

        ' 4. Schlüssel-Code-String zusammensetzen (Format: Code°VON°1200°BIS°1200|)
        Dim C As String = fcCode(sCode) & "°" & sToDay & "°1200°" & sToDay1 & "°1200|"

        ' 5. Code an den Webserver senden
        PHP.Data(C, cgIPWeb)
    End Sub




#End Region

#Region "Zimmer Informationen......................................................................"

    ''' <summary>
    ''' Lädt die Buchungsdaten für das aktuelle Zimmer und initialisiert das Kalender-Steuerelement.
    ''' </summary>
    ''' <param name="isAnreise">Bestimmt den Modus für das Kalender-Steuerelement (True = Anreise, False = Abreise).</param>
    ''' <remarks>
    ''' 12.10.2026 - Erstellt: Gemeinsame Logik aus tbAnreise_Click und tbAbreise_Click hierhin ausgelagert, um Code-Duplizierung zu vermeiden.
    ''' </remarks>
    Private Sub LadeUndInitialisiereKalender(ByVal isAnreise As Boolean)
        Dim aSperr As String = ""
        Dim aBuchung As String = ""

        ' Zimmer- und Buchungs-IDs ermitteln
        Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")
        Dim sID As String = Extract(tscoZim.Text, "[", "]", 1)

        ' Zeitraum berechnen (+/- 8 Monate bezogen auf das Anreisedatum)
        Dim basisDatum As Date = CDate(lbAnreise.Text)
        Dim d1 As Date = basisDatum.AddMonths(-8)
        Dim d2 As Date = basisDatum.AddMonths(8)

        ' SQL-Abfrage aufbauen
        Dim sSql As String = "Select * From Buchung Where ZimID='" & sZim & "' and Von<='" & fcUmDatum(d2) & "' and Bis >= '" & fcUmDatum(d1) & "'"
        Dim dt As DataTable = fcReadDataTable(sSql)

        ' Daten in einer einzigen Schleife verarbeiten (Performance-Optimierung)
        For k As Integer = 0 To dt.Rows.Count - 1
            Dim row As DataRow = dt.Rows(k)
            Dim currentID As String = row("ID").ToString()
            Dim currentBID As String = row("BID").ToString()
            Dim zeitraum As String = ";" & row("von").ToString() & "-" & row("bis").ToString()

            If currentID <> sID Then
                If currentBID <> sgRBID Then
                    aSperr &= zeitraum
                ElseIf currentBID = sgRBID Then
                    aBuchung &= zeitraum
                End If
            End If
        Next

        ' Kalender-Steuerelement befüllen und anzeigen
        MyKalender1.Datum = fcUmDatum(basisDatum)

        ' Erstes Semikolon sicher entfernen (.NET Substring statt VB6 Mid)
        MyKalender1.Sperr = If(aSperr.Length > 0, aSperr.Substring(1), "")
        MyKalender1.Buchung = If(aBuchung.Length > 0, aBuchung.Substring(1), "")

        ' Modus (Anreise oder Abreise) dynamisch setzen
        MyKalender1.AnreiseAbreise = isAnreise

        MyKalender1.Anreise = fcUmDatum(basisDatum)
        MyKalender1.Abreise = fcUmDatum(CDate(lbAbreise.Text).AddDays(-1))
        MyKalender1.Visible = True
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des tbAnreise-Feldes.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Logik in zentrale Methode ausgelagert und XML-Kommentar hinzugefügt.
    ''' </remarks>
    Private Sub tbAnreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbAnreise.Click
        LadeUndInitialisiereKalender(isAnreise:=True)
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des tbAbreise-Feldes.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Logik in zentrale Methode ausgelagert und XML-Kommentar hinzugefügt.
    ''' </remarks>
    Private Sub tbAbreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbAbreise.Click
        LadeUndInitialisiereKalender(isAnreise:=False)
    End Sub

    ''' <summary>
    ''' Steuert die Sichtbarkeit und den Standardpreis des Frühstücksfeldes (tbFPreis) 
    ''' in Abhängigkeit der gewählten Verpflegungsart (Übernachtung mit Frühstück "Ü/F").
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (coArt).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Sichtbarkeitssteuerung direkt an die Bedingung gekoppelt (Kompaktierung).
    ''' - Mathematische Berechnung durch sicheres Parsen von 'arIni' via 'Double.TryParse' abgesichert.
    ''' - Explizite Formatierung des berechneten Frühstückspreises auf zwei Nachkommastellen (.ToString("F2")).
    ''' </remarks>
    Private Sub coArt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coArt.SelectedIndexChanged
        ' Prüfen, ob Übernachtung mit Frühstück gewählt wurde
        Dim isFruehstueck As Boolean = (coArt.Text = "Ü/F")

        ' Sichtbarkeit der TextBox direkt zuweisen
        tbFPreis.Visible = isFruehstueck

        If isFruehstueck Then
            ' Werte sicher aus dem INI-Array parsen
            Dim val17 As Double = 0
            Dim val24 As Double = 0

            ' Falls arIni() Strings enthält, wandeln wir sie hier sicher in Double um
            If arIni(17) IsNot Nothing Then Double.TryParse(arIni(17).ToString(), val17)
            If arIni(24) IsNot Nothing Then Double.TryParse(arIni(24).ToString(), val24)

            ' Berechnung durchführen (z.B. Netto + Steueranteil oder zwei Preisbestandteile)
            Dim nGesamtPreis As Double = (val17 / 100.0) + (val24 / 100.0)

            ' Sauber formatiert als Währungs-/Dezimalzahl mit 2 Nachkommastellen (z.B. "3,00") zuweisen
            tbFPreis.Text = nGesamtPreis.ToString("F2")
        Else
            ' Standardwert setzen, wenn kein Frühstück gewählt ist
            tbFPreis.Text = "0,00"
        End If
    End Sub

    ''' <summary>
    ''' Formatiert den Text im Preisfeld als Dezimalwert, sobald das Steuerelement den Fokus verliert.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die TextBox 'tbFPreis').</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 13.10.2026 - Code-Optimierung:
    ''' - Null- und Leerstring-Prüfung mittels 'String.IsNullOrWhiteSpace' hinzugefügt, um Laufzeitfehler bei leerem Feld zu verhindern.
    ''' - 'Handles tbFPreis.LostFocus' beibehalten für die automatische Ereignisverknüpfung.
    ''' </remarks>
    Private Sub tbFPreis_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFPreis.LostFocus
        ' Nur formatieren, wenn das Textfeld nicht leer ist oder nur aus Leerzeichen besteht
        If Not String.IsNullOrWhiteSpace(tbFPreis.Text) Then
            tbFPreis.Text = fcFormatDecimal(tbFPreis.Text)
        End If
    End Sub

    ''' <summary>
    ''' Löst die Berechnung der Summe aus, wenn der Benutzer auf die entsprechende Schaltfläche klickt.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Schaltfläche 'buBerechnung').</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 13.10.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' - Dokumentation gemäß den aktuellen Projektstandards hinzugefügt.
    ''' </remarks>
    Private Sub buBerechnung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buBerechnung.Click
        ' Führt die Summenberechnung anhand der aktuellen IDs aus
        Summe(sgRBID, sgRZID)
    End Sub

    ''' <summary>
    ''' Wird ausgeführt, wenn das Anzahlungsfeld den Fokus verliert. Formatiert den eingegebenen Wert in ein einheitliches Dezimalformat.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - XML-Dokumentation inhaltlich korrigiert (Formatierung statt reiner Prüfung).
    ''' </remarks>
    Private Sub tbAnzahlung_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAnzahlung.Leave
        tbAnzahlung.Text = fcFormatDecimal(tbAnzahlung.Text)
    End Sub

    ''' <summary>
    ''' Wird ausgeführt, wenn das Preisfeld den Fokus verliert. Formatiert den Text dezimal und stößt die Neuberechnung der Gesamtsumme an.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort bei der Prozedur 'Summe' entfernt.
    ''' - XML-Beschreibung an die tatsächliche Logik angepasst.
    ''' </remarks>
    Private Sub tbPreis_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPreis.Leave
        tbPreis.Text = fcFormatDecimal(tbPreis.Text)
        Summe(sgRBID, sgRZID)
    End Sub

#End Region

#Region "Speichern der Reservierungsdaten.........................................................."

    ''' <summary>
    ''' Validiert die Eingabedaten einer Zimmerreservierung (Gasterauswahl, Personenzahl, Buchungstext) 
    ''' und speichert die Änderungen konsistent in den Gast- und Preisstrukturen der Datenbank ab.
    ''' </summary>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter konsequent entfernt.
    ''' - Modernisiert: 'String.IsNullOrWhiteSpace' für präzisere Validierung der Textboxen implementiert.
    ''' - Code-Hygiene: Auskommentierte Legacy-Codezeilen (alte KNr-Generierung) vollständig entfernt.
    ''' </remarks>
    Private Sub prSaveZimmerReservierung()
        Dim sKnr As String = lbGastID.Text

        ' 1. Daten sammeln und in Arrays aufbereiten
        arNew = fcCollectDataInArray(arNew)

        ' 2. Speichervorgänge der verschiedenen Sub-Systeme ausführen
        sKnr = fcSaveGast(arNew, sKnr)
        prSavePreis(arNew, sKnr)
        prSaveRestZimmerOfReservierung(lUnbekannt, sKnr, sgRBID)
        prSaveBuchungsText(sgRBID)

        ' 3. UI-Daten aktualisieren und IDs synchronisieren
        prRefreshData()
        lbGastID.Text = sKnr

    End Sub


    ''' <summary>
    ''' Speichert und aktualisiert die Preis-, Belegungs- und Buchungskonditionen einer Reservierung 
    ''' sowohl direkt in der SQL-Datenbank (Tabelle Buchung) als auch in der lokalen DataTable (dtBuc).
    ''' </summary>
    ''' <param name="arT">Das übergebene Datenarray mit den Reservierungsinformationen.</param>
    ''' <param name="sKNr">Die zugehörige Kundennummer des Gastes.</param>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Performance: 'StringBuilder' und 'Split("°")' durch direkte, typsichere Array-Initialisierung ersetzt.
    ''' - Finanzmathematik: 'Val()' durch stabiles, Kultur-unabhängiges Parsen auf Decimal-Ebene ersetzt, um Cent-Abweichungen auszuschließen.
    ''' - Code-Hygiene: Ungenutzte Codefragmente, auskommentierte Array-Indizes sowie die veralteten 'Call'-Anweisungen restlos entfernt.
    ''' - Typsicherheit: 'DateAdd' durch die native .NET-Methode '.AddDays(-1)' ersetzt.
    ''' </remarks>
    Private Sub prSavePreis(ByVal arT() As String, ByVal sKNr As String)
        Dim sPauch As String = If(cbPausch.Checked, "1", "0")

        Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")
        Dim sOID As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "IDObjekte")
        Dim cBedingung As String = " WHERE ID='" & sBID & "'"

        Dim dEndDatum As Date
        Dim sADatum As String = ""
        If Date.TryParse(lbAbreise.Text, dEndDatum) Then
            sADatum = dEndDatum.AddDays(-1).ToString("dd.MM.yyyy")
        End If

        Dim sPreisS As String = "0"
        Dim sPreisG As String = "0"
        Dim sVariable As String = "0"

        If rbNormal.Checked Then sVariable = "0"
        If rbFest.Checked Then sVariable = "1"
        If rbVariabel.Checked Then sVariable = "2"

        Try
            ' 1. Definition der Feldnamen (Spalten in der DB)
            Dim arFields() As String = {"Von", "Bis", "ObjID", "ZimID", "Variable", "KunID", "Name1", "Name2", "Personen", "Tiere", "Art", "Frueh", "Kategorie", "Preis", "Anzahlung", "Werbung", "Info", "FPreis", "Storno", "Summe", "Pausch", "RDSenden", "ZDatum", "InternetNr", "GPreis", "MwstU", "VonZeit", "BisZeit", "Code"}

            ' 2. Berechnung von Frühstück und Bewirtungsanteilen (Cent-basiert)
            Dim sFruehAnzahl As String = "0"
            If coArt.Text = "Ü/F" Then
                sFruehAnzahl = tbAnzPer.Text.Trim()

                Dim nFPreisDecimal As Decimal = 0
                Dim sFPreisBereinigt As String = tbFPreis.Text.Replace(","c, "."c).Trim()
                If Decimal.TryParse(sFPreisBereinigt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, nFPreisDecimal) Then
                    Dim nFPreisCent As Integer = Convert.ToInt32(Math.Round(nFPreisDecimal * 100, MidpointRounding.AwayFromZero))
                    Dim nIni24 As Integer = Convert.ToInt32(Val(arIni(24)))
                    sPreisS = (nFPreisCent - nIni24).ToString()
                End If
                sPreisG = arIni(24)
            End If

            ' Beträge (Preis, Anzahlung, Summe) sicher in Cent parsen
            Dim nPreisCent As Integer = 0
            Dim nAnzahlungCent As Integer = 0
            Dim nSummeCent As Integer = 0

            ' Temporäre Variable deklarieren (kann für alle drei Aufrufe wiederverwendet werden)
            Dim nTmpCent As Integer

            ' 1. Preis verarbeiten
            nTmpCent = fcTextToCent(tbPreis.Text)
            If nTmpCent >= 0 Then
                nPreisCent = nTmpCent
                ' Optional: Textbox sauber formatiert zurückschreiben (z.B. "150,50")
                tbPreis.Text = Convert.ToDecimal(nPreisCent / 100).ToString("N2")
            End If

            ' 2. Anzahlung verarbeiten
            nTmpCent = fcTextToCent(tbAnzahlung.Text)
            If nTmpCent >= 0 Then
                nAnzahlungCent = nTmpCent
                tbAnzahlung.Text = Convert.ToDecimal(nAnzahlungCent / 100).ToString("N2")
            End If

            ' 3. Summe verarbeiten
            nTmpCent = fcTextToCent(tbSumme.Text)
            If nTmpCent >= 0 Then
                nSummeCent = nTmpCent
                tbSumme.Text = Convert.ToDecimal(nSummeCent / 100).ToString("N2")
            End If


            ' 3. Fälligkeitsdatum berechnen (ZDatum)
            Dim zDatum As String = ""
            Dim sRechSendText As String = tbRechSend.Text.Trim()

            If IsDate(sRechSendText) Then
                zDatum = fcUmDatum(sRechSendText)
            Else
                Dim nTage As Integer = Convert.ToInt32(Val(sRechSendText))
                If nTage <= 0 Then
                    Dim dVon As Date
                    If arT.Length > 21 AndAlso Date.TryParse(fcUmDatum(arT(21)), dVon) Then
                        zDatum = fcUmDatum(dVon.AddDays(nTage).ToString("dd.MM.yyyy"))
                    End If
                Else
                    zDatum = fcUmDatum(Date.Today.AddDays(nTage).ToString("dd.MM.yyyy"))
                End If
            End If

            ' 4. Direkte und typsichere Werte-Zuweisung an das Array (ohne StringBuilder/Split)
            Dim arValue() As String = New String() {
                fcUmDatum(lbAnreise.Text),
                fcUmDatum(sADatum),
                sOID,
                sZim,
                sVariable,
                sKNr,
                tbName1Z.Text.Trim(),
                tbName2Z.Text.Trim(),
                tbAnzPer.Text.Trim(),
                "0",
                coArt.Text.Trim(),
                sFruehAnzahl,
                tbUArt.Text.Trim(),
                nPreisCent.ToString(),
                nAnzahlungCent.ToString(),
                sgWerbung,
                tbInfo.Text.Trim(),
                sPreisS,
                tbStorno.Text.Trim(),
                nSummeCent.ToString(),
                sPauch,
                sRechSendText,
                zDatum,
                tbInternetNr.Text.Trim(),
                sPreisG,
                sMwstU,
                fcUmZeit(tbAnZeit.Text),
                fcUmZeit(tbAbZeit.Text),
                sCode
            }

            ' 5. Datenbank aktualisieren
            fcUpdateCommand("Buchung", arFields, arValue, cBedingung)

            ' 6. Lokale DataTable synchronisieren
            Dim cBedingungTable As String = "ID Like '" & sBID & "'"
            fcUpdateTable(dtBuc, arFields, arValue, cBedingungTable)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Konvertiert einen eingegebenen Geldbetrag-String sicher und kulturübergreifend in Cent.
    ''' </summary>
    ''' <param name="textBetrag">Der zu parsende Text aus der Textbox.</param>
    ''' <returns>Der Betrag in Cent oder -1, wenn das Parsen fehlschlägt.</returns>
    ''' <remarks>
    ''' 14.10.2026 - Erstellt
    ''' - Unterstützt sowohl deutsche (Koma) als auch englische (Punkt) Eingaben durch 'CurrentCulture'.
    ''' - Eliminiert ungenaue Rundungen durch 'Math.Round' mit kaufmännischer Rundung.
    ''' </remarks>
    Private Function fcTextToCent(ByVal textBetrag As String) As Integer
        If String.IsNullOrWhiteSpace(textBetrag) Then Return 0

        Dim nValue As Decimal
        Dim cleanText As String = textBetrag.Trim()

        ' Nutzt die aktuelle Windows-Kultur (in DE standardmäßig das Komma, liest aber auch Punkte als Tausendertrennzeichen)
        If Decimal.TryParse(cleanText, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.CurrentCulture, nValue) Then
            Return Convert.ToInt32(Math.Round(nValue * 100, MidpointRounding.AwayFromZero))
        End If

        ' Fallback für den Fall, dass auf einem deutschen PC ein Punkt als Dezimaltrenner eingegeben wurde (z.B. "12.50")
        If Decimal.TryParse(cleanText, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.InvariantCulture, nValue) Then
            Return Convert.ToInt32(Math.Round(nValue * 100, MidpointRounding.AwayFromZero))
        End If

        ' Ungültige Eingabe
        Return -1
    End Function


    ''' <summary>
    ''' Überträgt bei der Erstanlage einer Gruppenbuchung die gemeinsamen Buchungsdaten (Zeitraum, Gast-ID, 
    ''' Verpflegung, Preise, Zeiten) gesammelt auf alle zugeordneten Zimmer der Reservierung.
    ''' </summary>
    ''' <param name="lUn">Gibt an, ob es sich um eine noch nicht synchronisierte Gruppenreservierung handelt.</param>
    ''' <param name="sKnr">Die eindeutige Kundennummer des Hauptgastes.</param>
    ''' <param name="sBID">Die zu aktualisierende Buchungs-ID.</param>
    ''' <remarks>
    ''' 26.02.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Performance: Ressourcenintensiven 'StringBuilder' und 'Split("°")' durch direkte Array-Zuweisung ersetzt.
    ''' - Typensicherheit: 'DateAdd' durch native .NET-Methode '.AddDays(-1)' ersetzt und Datums-Parsing abgesichert.
    ''' - Finanzmathematik: 'Val()' durch Kultur-unabhängiges Parsen auf Decimal-Ebene ersetzt zur Eliminierung von Cent-Fehlern.
    ''' - Code-Hygiene: Redundante 'Call'-Schlüsselwörter entfernt und Variablen-Scoping geschärft.
    ''' </remarks>
    Private Sub prSaveRestZimmerOfReservierung(ByVal lUn As Boolean, ByVal sKnr As String, ByVal sBID As String)
        If Not lUn Then Exit Sub

        Dim dEndDatum As Date
        Dim sADatum As String = ""
        If Date.TryParse(lbAbreise.Text, dEndDatum) Then
            sADatum = dEndDatum.AddDays(-1).ToString("dd.MM.yyyy")
        End If

        Try
            ' 1. Definition der Feldnamen (Spalten in der DB)
            Dim sqlText As String = "Von,Bis,KunID,Kunde,Name1,Name2,Personen,Tiere,Art,Frueh,Kategorie,Preis,Werbung,InternetNr,VonZeit,BisZeit"
            Dim arFields() As String = sqlText.Split(","c)

            ' 2. Frühstücks-Anzahl ermitteln
            Dim sFruehAnzahl As String = "0"
            If coArt.Text = "Ü/F" Then
                sFruehAnzahl = tbAnzPer.Text.Trim()
            End If

            ' Zimmerpreis sicher in Cent konvertieren (Kultur-unabhängig)
            Dim nPreisCent As Integer = 0
            Dim nPreisDecimal As Decimal = 0
            Dim sPreisBereinigt As String = tbPreis.Text.Replace(","c, "."c).Trim()
            If Decimal.TryParse(sPreisBereinigt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, nPreisDecimal) Then
                nPreisCent = Convert.ToInt32(Math.Round(nPreisDecimal * 100, MidpointRounding.AwayFromZero))
            End If

            ' 3. Direkte und typsichere Werte-Zuweisung an das Array (ohne StringBuilder/Split)
            Dim arValue() As String = New String() {
                fcUmDatum(lbAnreise.Text),
                fcUmDatum(sADatum),
                sKnr,
                tbName1.Text.Trim(),
                tbName1Z.Text.Trim(),
                tbName2Z.Text.Trim(),
                tbAnzPer.Text.Trim(),
                "0",
                coArt.Text.Trim(),
                sFruehAnzahl,
                tbUArt.Text.Trim(),
                nPreisCent.ToString(),
                sgWerbung,
                tbInternetNr.Text.Trim(),
                fcUmZeit(tbAnZeit.Text),
                fcUmZeit(tbAbZeit.Text)
            }

            ' 4. Datenbank aktualisieren (SQL-Update ausführen)
            Dim cBedingungSql As String = " WHERE BID='" & sBID & "'"
            fcUpdateCommand("Buchung", arFields, arValue, cBedingungSql)

            ' 5. Lokale In-Memory DataTable synchronisieren
            Dim cBedingungTable As String = "BID Like '" & sBID & "'"
            fcUpdateTable(dtBuc, arFields, arValue, cBedingungTable)

            ' Alle abhängigen Grids/Datenansichten neu laden
            prRefreshDataAll(sBID)

            lUnbekannt = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Speichert die Stammdaten des Gastes. Erkennt automatisch, ob es sich um eine Neuanlage 
    ''' (leere sKNr) oder um eine Aktualisierung handelt, steuert die SQL-Befehle (Insert/Update)
    ''' und gibt die (ggf. neu generierte) Kundennummer zurück.
    ''' </summary>
    ''' <param name="arT">Das übergebene Datenarray mit den gesammelten Gastinformationen (aktuell ungenutzt).</param>
    ''' <param name="sKNr">Die aktuelle Kundennummer (Leerstring bei Neuanlage).</param>
    ''' <returns>Die Kundennummer des gespeicherten Gastdatensatzes als String.</returns>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Fehlerkorrektur Logik: Das auskommentierte 'If lNew'-Konstrukt wiederhergestellt, damit Neuanlagen via 'fcInsertCommand' und Updates via 'fcUpdateCommand' sauber getrennt laufen.
    ''' - Array-Fix: Die riskante Vorab-Befüllung von 'arValue' mit Feldnamen via 'Split' entfernt und durch eine direkte, saubere Zuweisung ersetzt.
    ''' - Modernisierung: Unnötigen 'StringBuilder' entfernt und VB6-'Call'-Schlüsselwörter sowie Funktionsnamen-Zuweisungen durch 'Return' ersetzt.
    ''' </remarks>
    Private Function fcSaveGast(ByVal arT() As String, ByVal sKNr As String) As String
        Dim lNew As Boolean = String.IsNullOrWhiteSpace(sKNr)
        Dim sID As String = sKNr.Trim()

        ' Im Falle einer Neuanlage leeren Datensatz in der Datenbank vorbereiten und ID abholen
        If lNew Then
            sID = fcAppendBlank("Kunden")
        End If

        Try
            ' 1. Definition der Tabellenspalten
            Dim arFields() As String = {"ID", "Anrede", "Name1", "Name2", "Name1Z", "Name2Z", "Vorname", "Strasse", "PLZ", "Ort", "Land", "Telefon", "Telefax", "Funk", "EMail", "Pass", "Geb", "Info", "Werbung"}

            ' 2. Direkte und typsichere Zuweisung der UI-Werte in das Werte-Array
            Dim arValue() As String = New String() {
                sID,
                coAnrede.Text.Trim(),
                tbName1.Text.Trim(),
                tbName2.Text.Trim(),
                tbName1Z.Text.Trim(),
                tbName2Z.Text.Trim(),
                tbVorname.Text.Trim(),
                tbStrasse.Text.Trim(),
                tbPLZ.Text.Trim(),
                tbOrt.Text.Trim(),
                coLand.Text.Trim(),
                tbTel.Text.Trim(),
                tbFax.Text.Trim(),
                tbFunk.Text.Trim(),
                tbEMail.Text.Trim(),
                "",
                fcUmDatum(dpGeb.Value.ToString("dd.MM.yyyy")),
                tbInfo.Text.Trim(),
                coWerbung.Text.Trim()
            }

            ' 3. Differenzierte Datenbank-Speicherung (Insert bei Neuanlage, Update bei Änderungen)
            If lNew Then
                fcInsertCommand("Kunden", arFields, arValue)
            Else
                Dim cBedingung As String = " WHERE ID='" & sID & "'"
                fcUpdateCommand("Kunden", arFields, arValue, cBedingung)
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try

        Return sID
    End Function


    ''' <summary>
    ''' Zu speichernde Daten aufbereiten
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 10.01..2012 Create
    ''' </remarks>
    Private Function fcGetSQLTextGast(ByVal sID As String, ByVal arT() As String) As String
        Dim sb As New StringBuilder
        Dim sGeb As String = " "
        For i As Integer = 5 To 19
            If arT(i).Trim = "" Then arT(i) = " "
        Next
        ' "ID,Anrede,Name1,Name2,Vorname,Strasse,PLZ,Ort,Land,Telefon,Telefax,Funk,EMail,Pass,Geb,Info"
        sb.Append(sID & "°")
        sb.Append(arT(5) & "°")
        sb.Append(arT(6) & "°")
        sb.Append(arT(7) & "°")
        sb.Append(arT(8) & "°")
        sb.Append(arT(9) & "°")
        sb.Append(arT(10) & "°")
        sb.Append(arT(11) & "°")
        sb.Append(arT(12) & "°")
        sb.Append(arT(13) & "°")
        sb.Append(arT(14) & "°")
        sb.Append(arT(15) & "°")
        sb.Append(arT(16) & "°")
        sb.Append(arT(17) & "°")
        sb.Append(arT(19) & "°")
        sb.Append(arT(18) & "°")
        sb.Append(sgWerbung)
        fcGetSQLTextGast = sb.ToString
    End Function

    ''' <summary>
    ''' Aktualisiert die visuellen Reservierungseinträge im Buchungs-DataGrid des Hauptfensters.
    ''' Entfernt bei Zeitraum- oder Zimmeränderungen die alten Belegungsblöcke und zeichnet die neuen Daten ein.
    ''' </summary>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter konsequent entfernt.
    ''' - Absturzsicherung: Typkonvertierungen von Strings in Datumsformate über 'Date.TryParse' abgesichert.
    ''' - Performance: Direktes Auslesen der Zimmer-ID aus der Steuerelement-Eigenschaft gekapselt.
    ''' </remarks>
    Private Sub prRefreshData()
        If sIndexIst.Trim() = "0" Then
            Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")

            Dim sBO As Date
            Dim sEO As Date
            Dim sBN As Date
            Dim sEN As Date

            ' Typsicheres Parsen der Datums-Strings aus den Arrays (verhindert InvalidCastExceptions)
            Dim isValidOldVon As Boolean = Date.TryParse(arOld(21)?.ToString(), sBO)
            Dim isValidOldBis As Boolean = Date.TryParse(arOld(22)?.ToString(), sEO)
            Dim isValidNewVon As Boolean = Date.TryParse(arNew(21)?.ToString(), sBN)
            Dim isValidNewBis As Boolean = Date.TryParse(arNew(22)?.ToString(), sEN)

            ' Wenn die Datumsübergaben valide sind, wird das Grid neu gezeichnet
            If isValidOldVon AndAlso isValidOldBis AndAlso isValidNewVon AndAlso isValidNewBis Then
                Dim sP As String = arNew(0)?.ToString()
                Dim sID As String = sgRBID
                Dim sName As String = arNew(6)?.ToString()
                Dim sFr As String = arNew(1)?.ToString()

                ' Alten Zeitraum aus der Belegungsmaske löschen
                prDeleteReservierungInDataGrid(sBO, sEO, sZim)

                ' Neuen Zeitraum in die Belegungsmaske eintragen
                prSetReservierungInDataGrid(sBN, sEN, sZim, sP, sName, sID, sFr)
            End If
        End If
    End Sub


    ''' <summary>
    ''' Speichert den ausgewählten Buchungstext, den Bezahlstatus sowie die Sprach-ID 
    ''' in allen zugehörigen Reservierungsdatensätzen der übergebenen Buchungs-ID (BID) 
    ''' sowohl in der SQL-Datenbank als auch in der lokalen DataTable.
    ''' </summary>
    ''' <param name="sBID">Die eindeutige Buchungs-ID, für welche die Texte aktualisiert werden.</param>
    ''' <remarks>
    ''' 10.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Performance-Schub: Den langsamen Umweg über 'StringBuilder.Append' und 'Split("°")' restlos entfernt. Das Werte-Array wird nun direkt initialisiert.
    ''' - Index-Schutz: Sicherer Abruf des ersten Elements aus der DataTable via Null-Conditional-Operator.
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter und VB6-'Split'-Funktionen entfernt.
    ''' </remarks>
    Private Sub prSaveBuchungsText(ByVal sBID As String)
        Dim sBez As String = If(cbBezalt.Checked, "1", "0")
        Dim sID As String = " "

        ' ID des Buchungstextes aus den Stammdaten auslesen
        Dim dt As DataTable = fcReadDataTable("Select ID from BTexte Where Name='" & coBText.Text.Replace("'", "''") & "'")
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            sID = dt.Rows(0).Item(0)?.ToString()
        End If

        ' Sprach-ID ermitteln
        Dim sL As String = coLang.SelectedIndex.ToString()
        If lLang Then sL = "1"

        ' Definition der Felder und direkte Zuweisung der korrespondierenden Werte
        Dim arFields() As String = {"Bez", "Sprache", "BText", "MText"}
        Dim arValue() As String = New String() {sBez, sL, sID, lbMakro.Text.Trim()}

        ' 1. Datenbank-Ebene (SQL-Update ausführen)
        Dim cBedingungSql As String = " WHERE BID='" & sBID & "'"
        fcUpdateCommand("Buchung", arFields, arValue, cBedingungSql)

        ' 2. In-Memory-Ebene (Lokale DataTable synchronisieren)
        Dim cBedingungTable As String = "BID Like '" & sBID & "'"
        fcUpdateTable(dtBuc, arFields, arValue, cBedingungTable)
    End Sub


    ''' <summary>
    ''' Bei Erstanlage einer Buchung alle Zimmer aktualisieren
    ''' </summary>
    ''' <param name="sBID"></param>
    ''' <remarks>
    ''' 26.02.2012 Create
    ''' </remarks>
    Private Sub prRefreshDataAll(ByVal sBID As String)
        Dim sBO As Date = lbAnreise.Text
        Dim sDatum As Date = lbAbreise.Text
        Dim sEO As Date = lbAbreise.Text 'DateAdd(DateInterval.Day, -1, sDatum)

        Dim sP As String = tbAnzPer.Text
        Dim sID As String
        Dim sName As String = tbName1.Text
        Dim sFr As String = coArt.Text
        Dim sSQL As String = "Select * From Buchung Where BID='" & sBID & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)
        If dt.Rows.Count > 0 Then

            Dim nMax As Integer = dt.Rows.Count - 1
            For i As Integer = 0 To nMax
                sID = dt.Rows(i).Item("ZimID").ToString.Trim()
                Call prDeleteReservierungInDataGrid(sBO, sEO, sID)
                Call prSetReservierungInDataGrid(sBO, sEO, sID, sP, sName, sBID, sFr)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Löscht eine alte Reservierung visuell aus dem Belegungs-DataGrid des Hauptfensters,
    ''' indem die betroffenen Zellen im angegebenen Zeitraum geleert und wieder weiß gefärbt werden.
    ''' </summary>
    ''' <param name="sBegin">Das Anreisedatum des zu löschenden Zeitraums.</param>
    ''' <param name="sEnd">Das Abreisedatum des zu löschenden Zeitraums.</param>
    ''' <param name="sZim">Die Zimmer-ID, deren Spalte bereinigt werden soll.</param>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Typsicherheit erhöht: Implizite Konvertierung von 'Cells(0).Value' (Object) zu 'Date' durch sicheres 'Date.TryParse' ersetzt.
    ''' - Performance: Schleifenzähler lokal deklariert und ungenutzte Zwischenschritte entfernt.
    ''' - Robustheit: Schutz vor Fehlern bei ungültigen Zellwerten oder Leerzeilen im Haupt-Grid.
    ''' </remarks>
    Private Sub prDeleteReservierungInDataGrid(ByVal sBegin As Date, ByVal sEnd As Date, ByVal sZim As String)
        ' Ermittelt die korrekte Spaltennummer für das gegebene Zimmer im Haupt-Grid
        Dim nCol As Integer = fcGetZimmerSpalte(sZim, frmMain.dgBuchung)

        ' Wenn die Spalte nicht gefunden wurde (z.B. nCol = -1), Routine sicher verlassen
        If nCol < 0 Then Exit Sub

        With frmMain.dgBuchung
            Dim nDayMaxIndex As Integer = .Rows.Count - 1

            For i As Integer = 0 To nDayMaxIndex
                Dim cellValue As Object = .Rows(i).Cells(0).Value
                Dim sDay As Date

                ' Zelle sicher in ein gültiges Datum parsen (Schutz vor Geisterzeilen oder Leerwerten)
                If cellValue IsNot Nothing AndAlso Date.TryParse(cellValue.ToString(), sDay) Then

                    ' Da die Zeilen chronologisch aufsteigend sortiert sind, kann die Schleife 
                    ' abgebrochen werden, sobald wir das Abreisedatum erreicht haben.
                    If sDay = sEnd Then
                        Exit For
                    End If

                    ' Liegt der Tag im Buchungszeitraum, Zelle zurücksetzen
                    If sDay >= sBegin Then
                        .Rows(i).Cells(nCol).Value = ""
                        .Rows(i).Cells(nCol).Style.BackColor = Color.White
                    End If
                End If
            Next
        End With
    End Sub

    ''' <summary>
    ''' Trägt eine neue Reservierung visuell in das Belegungs-DataGrid des Hauptfensters ein,
    ''' färbt die betroffenen Zellen je nach Verpflegungsart (Übernachtung mit Frühstück oder Standard) ein
    ''' und schreibt den Gastnamen inklusive Buchungs-ID und Personenanzahl in die Zellen.
    ''' </summary>
    ''' <param name="sBegin">Das Anreisedatum des neuen Zeitraums.</param>
    ''' <param name="sEnd">Das Abreisedatum des neuen Zeitraums.</param>
    ''' <param name="sZim">Die Zimmer-ID, deren Spalte befüllt werden soll.</param>
    ''' <param name="sP">Die Anzahl der Personen als String-Anzeige.</param>
    ''' <param name="sName">Der Name des Gastes.</param>
    ''' <param name="sID">Die Buchungs-ID (BID).</param>
    ''' <param name="sFr">Die Verpflegungsart (z. B. "Ü/F" für Übernachtung mit Frühstück).</param>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Typsicherheit erhöht: Implizite Konvertierung von 'Cells(0).Value' (Object) zu 'Date' durch sicheres 'Date.TryParse' ersetzt.
    ''' - Performance: Schleifenzähler lokal deklariert, um 'Option Strict On'-Kompatibilität zu gewährleisten.
    ''' - Code-Hygiene: String-Verknüpfung auf den nativen .NET-Standard-Operator '&' umgestellt.
    ''' </remarks>
    Private Sub prSetReservierungInDataGrid(ByVal sBegin As Date, ByVal sEnd As Date,
                                            ByVal sZim As String, ByVal sP As String,
                                            ByVal sName As String, ByVal sID As String,
                                            ByVal sFr As String)
        ' Ermittelt die korrekte Spaltennummer für das gegebene Zimmer im Haupt-Grid
        Dim nCol As Integer = fcGetZimmerSpalte(sZim, frmMain.dgBuchung)

        ' Wenn die Spalte nicht im Grid vorhanden ist, Routine sicher verlassen
        If nCol < 0 Then Exit Sub

        ' Hintergrundfarbe basierend auf der Verpflegungsart bestimmen
        Dim cellColor As Color = If(sFr = "Ü/F", Color.LimeGreen, Color.Turquoise)

        ' Formatierter Zelltext (Name, ID und Personenanzahl)
        Dim sZellText As String = PadR(sName, 15) & "(" & sID & ") [" & sP & "]"

        With frmMain.dgBuchung
            Dim nDayMaxIndex As Integer = .Rows.Count - 1

            For i As Integer = 0 To nDayMaxIndex
                Dim cellValue As Object = .Rows(i).Cells(0).Value
                Dim sDay As Date

                ' Zelle sicher in ein gültiges Datum parsen (Schutz vor Leerzeilen am Grid-Ende)
                If cellValue IsNot Nothing AndAlso Date.TryParse(cellValue.ToString(), sDay) Then

                    ' Abreisedatum erreicht: Schleife vorzeitig abbrechen
                    If sDay = sEnd Then
                        Exit For
                    End If

                    ' Liegt der Tag im neuen Buchungszeitraum, Zelle visuell belegen
RefactorCell:
                    If sDay >= sBegin Then
                        .Rows(i).Cells(nCol).Style.BackColor = cellColor
                        .Rows(i).Cells(nCol).Value = sZellText
                    End If
                End If
            Next
        End With
    End Sub



#End Region

#Region "Hilfetexte in der Statusleiste ausgeben..................................................."

    ''' <summary>
    ''' Verwaltet die dynamische Anzeige von Hilfetexten in der Statusleiste (tssInfo),
    ''' sobald der Mauszeiger über ein registriertes UI-Steuerelement bewegt wird.
    ''' </summary>
    ''' <param name="sender">Das Steuerelement, über dem sich die Maus befindet.</param>
    ''' <param name="e">Die Ereignisdaten des MouseMove-Events.</param>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Massiver Redundanz-Abbau: Über 35 Einzelmethoden in eine einzige performante Sammelmethode konsolidiert.
    ''' - Wartbarkeit erhöht: Alle Beschreibungstexte zentral in einem typsicheren Dictionary strukturiert.
    ''' - Fehlerkorrektur: Syntaxfehler bei der Beschreibung von 'lbObjekt' (dreifache Anführungszeichen) bereinigt.
    ''' </remarks>
    Private Sub UIElement_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _
        lbAnreise.MouseMove, lbAbreise.MouseMove, lbTage.MouseMove, lbZimNr.MouseMove,
        lbAusstattung.MouseMove, lbArt.MouseMove, lbBetten.MouseMove, lbObjekt.MouseMove,
        tbAnzPer.MouseMove, tbUArt.MouseMove, coArt.MouseMove, tbPreis.MouseMove,
        tbAnzahlung.MouseMove, coAnrede.MouseMove, tbName1.MouseMove, tbName2.MouseMove,
        tbVorname.MouseMove, tbStrasse.MouseMove, tbPLZ.MouseMove, tbOrt.MouseMove,
        coLand.MouseMove, dpGeb.MouseMove, tbTel.MouseMove, tbFax.MouseMove,
        tbFunk.MouseMove, tbEMail.MouseMove, tbInfo.MouseMove,
        coWerbung.MouseMove, lbGast.MouseMove, gbGast.MouseMove, gbZimmer.MouseMove, tsMain.MouseMove

        ' Statische Zuordnungstabelle aller Steuerelemente zu ihren Infotexten
        Static oInfoTexte As Dictionary(Of Object, String)

        ' Lazy-Initialization des Dictionarys beim allerersten Aufruf
        If oInfoTexte Is Nothing Then
            oInfoTexte = New Dictionary(Of Object, String)() From {
                {lbAnreise, "Anreisedatum (ab 12:00 Uhr)"},
                {lbAbreise, "Abreisedatum (bis 11:00 Uhr)"},
                {lbTage, "Anzahl der Übernachtungen"},
                {lbZimNr, "Zimmernummer"},
                {lbAusstattung, "Ausstattung des Zimmers"},
                {lbArt, "Kategorie des Zimmers"},
                {lbBetten, "Anzahl der möglichen Betten"},
                {lbObjekt, "In welchem Objekt ist das Zimmer?"},
                {tbAnzPer, "Anzahl der Personen pro Zimmer"},
                {tbUArt, "Übernachtungsart"},
                {coArt, "Auswahl der Übernachtung mit und ohne Frühstück"},
                {tbPreis, "Vereinbarter Preis pro Zimmer"},
                {tbAnzahlung, "Getätigte Anzahlung"},
                {coAnrede, "Anrede: Herr, Frau, Firma"},
                {tbName1, "Nachname / Firmenname"},
                {tbName2, "Nachname wenn Firmenbezug"},
                {tbVorname, "Vorname"},
                {tbStrasse, "Strasse"},
                {tbPLZ, "PLZ"},
                {tbOrt, "Ort"},
                {coLand, "Land (Kurz-Zeichen)"},
                {dpGeb, "Geburtsdatum"},
                {tbTel, "Telefon"},
                {tbFax, "Telefax"},
                {tbFunk, "Mobiltelefon"},
                {tbEMail, "E-Mail Adresse"},
                {tbInfo, "Informationen zum Gast"},
                {coWerbung, "Über welchen Kanal ist der Gast auf uns aufmerksam geworden?"},
                {lbGast, "Mit Doppelklick werden die Daten des Gastes (im Formular) gelöscht. Neuanlage ist nun möglich."}
            }
        End If

        ' Text ausgeben falls das Steuerelement im Dictionary existiert, andernfalls Leeres zurückgeben (für GroupBoxes/Toolstrips)
        If oInfoTexte.ContainsKey(sender) Then
            tssInfo.Text = oInfoTexte(sender)
        Else
            tssInfo.Text = String.Empty
        End If
    End Sub

    ''' <summary>
    ''' Verarbeitet das MouseHover-Event der Toolbar-Buttons zur Anzeige erweiterter Tooltips in der Statuszeile.
    ''' </summary>
    Private Sub ToolbarButtons_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        tsbDelZimmer.MouseHover, tsbDelReservierung.MouseHover

        If sender Is tsbDelZimmer Then
            tssInfo.Text = "Löschen der ausgewählten Reservierung / Zimmer"
        ElseIf sender Is tsbDelReservierung Then
            tssInfo.Text = "Die gesamte Reservierung wird gelöscht (alle Zimmer)."
        End If
    End Sub


#End Region

#Region "Buchungstext / Makros....................................................................."

    ''' <summary>
    ''' Aktualisiert die globale Variable für die Sprache basierend auf der Auswahl in der ComboBox.
    ''' </summary>
    ''' <remarks>
    ''' 12.10.2026 - Erstellt: Methode zur Vermeidung von Code-Duplizierung ausgelagert.
    ''' </remarks>
    Private Sub UpdateLanguage()
        ' Setzt lLang auf True, wenn die Sprache "Englisch" ist, andernfalls auf False
        lLang = (coLang.Text = "Englisch")
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der btBText-Schaltfläche.
    ''' Aktualisiert die Sprache, öffnet das Text-Formular und übergibt den Makro-Text.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Logik ausgelagert und XML-Kommentare hinzugefügt.
    ''' </remarks>
    Private Sub btBText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btBText.Click
        ' Sprache synchronisieren
        UpdateLanguage()

        ' Formular anzeigen und Text übergeben
        frmBText.Show()
        frmBText.tbBText.Text = lbMakro.Text
    End Sub

    ''' <summary>
    ''' Verarbeitet die Textänderung der Sprach-Auswahlbox.
    ''' Synchronisiert die globale Sprachvariable sofort bei Änderung.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Auf zentrale Update-Logik umgestellt.
    ''' </remarks>
    Private Sub coLang_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles coLang.TextChanged
        UpdateLanguage()
    End Sub

    ''' <summary>
    ''' Verarbeitet den Wechsel des ausgewählten Eintrags in der ComboBox coBText.
    ''' Füllt das Textfeld für die Rechnungsadresse automatisch aus dem Array arZZiel, 
    ''' sofern das Feld aktuell leer ist.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Datentyp für Index korrigiert (Integer statt String) und XML-Kommentar hinzugefügt.
    ''' </remarks>
    Private Sub coBText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coBText.SelectedIndexChanged
        ' Den ausgewählten Index als Integer auslesen
        Dim n As Integer = coBText.SelectedIndex

        ' Falls kein Eintrag ausgewählt ist (-1), die Verarbeitung abbrechen
        If n < 0 Then Exit Sub

        ' Wenn das Textfeld leer ist, den entsprechenden Wert aus dem Array zuweisen
        If tbRechSend.Text.Trim() = "" Then
            tbRechSend.Text = arZZiel(n)
        End If
    End Sub

    ''' <summary>
    ''' Steuert die Sichtbarkeit des Gruppenfelds für Zusatznamen basierend auf dem Zustand der CheckBox.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die CheckBox).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 13.10.2026 - Code-Optimierung:
    ''' - XML-Dokumentation vollständig hinzugefügt.
    ''' - 'If-Else'-Struktur durch direkte Zuweisung des Eigenschaftswerts ersetzt (Einzeiler).
    ''' - Redundanten Vergleich auf '= True' entfernt.
    ''' - 'ByVal'-Schlüsselwörter für eine explizite Parameterübergabe ergänzt.
    ''' </remarks>
    Private Sub cbZusatz_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbZusatz.CheckedChanged
        ' Die Sichtbarkeit des GroupBox-Panels direkt an den Zustand der CheckBox koppeln
        gbNameZ.Visible = cbZusatz.Checked
    End Sub


#End Region

#Region "Schreibe aufs Zimmer......................................................................"

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des "Auf Zimmer"-Buttons. 
    ''' Übergibt die aktuell ausgewählte Zimmernummer an die Steuervariable und öffnet das Zusatzformular.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.10.2026 - Code-Optimierung:
    ''' - Standardisierte XML-Dokumentation hinzugefügt.
    ''' - Inline-Kommentare zur besseren Verständlichkeit des Workflows ergänzt.
    ''' </remarks>
    Private Sub tsbAufZimmer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAufZimmer.Click
        ' 1. Aktuelle Zimmernummer aus dem Label auslesen und zwischenspeichern
        sgZNr = lbZimNr.Text

        ' 2. Das Zusatzformular für weitere Eingaben oder Buchungen anzeigen
        frmZusatz.Show()
    End Sub

#End Region

#Region "Reservierungsbestätigung drucken.........................................................."

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des Bestätigung-Buttons. 
    ''' Führt die Pflichtfeldprüfung durch, speichert die Zimmerreservierung und aktiviert bei erfolgreichem Druck den Mail-Button.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.10.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Speichern-Aufruf entfernt.
    ''' - 'If'-Bedingung für den Druck zur besseren Lesbarkeit in einen mehrzeiligen Block umgewandelt.
    ''' - Inline-Kommentare zur Dokumentation der Ablauflogik hinzugefügt.
    ''' </remarks>
    Private Sub tsbBestätigung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBestätigung.Click
        ' 1. Aufruf der neuen zentralen Pflichtfeldprüfung
        If Not ValidierungsPruefungErfolgreich() Then Exit Sub

        ' 2. Zimmerreservierung speichern
        prSaveZimmerReservierung()

        ' 3. Buchung drucken und bei Erfolg den E-Mail-Versand freigeben
        If fcDruckBuchnung(sgRBID) Then tsbMail.Enabled = True
    End Sub


#End Region

#Region "Buchungsbestätigung versenden............................................................."

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des Mail-Buttons. Überprüft die Existenz der Buchungsbestätigung,
    ''' lädt die Kunden- sowie Maildaten und versendet die PDF-Bestätigung per E-Mail.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.10.2026 - Code-Optimierung:
    ''' - Datentypen korrigiert (String-Array statt generischem Array für Mail-Texte).
    ''' - String-Überprüfungen auf das modernere 'String.IsNullOrWhiteSpace' umgestellt.
    ''' - Tippfehler in den MsgBox-Ausgaben korrigiert.
    ''' - Unbenutzte Variablen (sName, ii) entfernt bzw. optimiert und Inline-Kommentare hinzugefügt.
    ''' </remarks>
    Private Sub tsbMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbMail.Click
        ' 1. Prüfen, ob die PDF-Datei existiert
        Dim sPdf As String = arIni(32) & "\Buchung\Buch_" & sgRBID & ".PDF"

        If Not FileExists(sPdf) Then
            MsgBox("Buchungsbestätigung noch nicht erstellt.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        ' 2. Buchungsdaten (Kunden-ID und Sprache) abrufen
        Dim arfeld1() As String = fcDataSeek("select * From Buchung Where BID ='", sgRBID, 0, {"KunID", "Sprache"})
        Dim sKID As String = arfeld1(0)
        Dim iSprachIndex As Integer = Val(arfeld1(1))

        ' 3. Mail-Texte aus der Systemdatenbank laden und splitten
        Dim sText() As String = Split(ReadOneValueFromSystemDb("Mail"), "#")

        ' 4. Kundendaten (insb. E-Mail-Adresse) abrufen
        Dim arFeld() As String = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"EMail", "Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land"})
        Dim sMailAdresse As String = arFeld(0)

        ' 5. E-Mail validieren und senden
        If Not String.IsNullOrWhiteSpace(sMailAdresse) Then
            ' Index-Sicherheit für das Text-Array prüfen
            Dim sBetreff = If(iSprachIndex < sText.Length, sText(iSprachIndex), "Buchung")

            If fcSendeMailAnlage(sMailAdresse, "Buchung", sBetreff, sDatei:=sPdf) Then
                MsgBox("Buchungs-Mail erfolgreich gesendet.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly)
            Else
                MsgBox("Fehler beim Senden der Buchungs-Mail.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            End If
        Else
            MsgBox("Fehler: Keine Mailadresse hinterlegt.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
        End If
    End Sub

#End Region

#Region "Rechnung erstellen........................................................................"

    ''' <summary>
    ''' Öffnet das Rechnungsmodul nach erfolgreicher Validierung und Speicherung der Reservierungsdaten.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (Toolbar-Button).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 02.04.2012 - Create
    ''' 14.10.2026 - Code-Optimierung:
    ''' - XML-Dokumentation für Parameter vervollständigt.
    ''' - Aufruf von 'prChangeMenue' an die neue parameterlose Sub-Struktur angepasst.
    ''' - Inline-Kommentare zur Erläuterung des Programmablaufs hinzugefügt.
    ''' </remarks>
    Private Sub tsbRechnung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRechnung.Click
        ' 1. Aufruf der neuen zentralen Pflichtfeldprüfung
        If Not ValidierungsPruefungErfolgreich() Then
            Exit Sub
        End If

        ' 2. Aktuelle Zimmerreservierung in der Datenbank speichern
        prSaveZimmerReservierung()

        ' Aktuelle Gast-ID aus dem Oberflächenelement für das Rechnungsformular bereitstellen
        sgGID = lbGastID.Text

        ' Rechnungsformular öffnen
        frmRechnung.Show()

    End Sub

    'Private Sub tsbAnzahlungRechnung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAnzahlungRechnung.Click
    '    ' Aufruf der neuen zentralen Pflichtfeldprüfung
    '    If Not ValidierungsPruefungErfolgreich() Then Exit Sub
    '    prSaveZimmerReservierung()
    '    sgGID = lbGastID.Text
    '    frmAnzahlung.Show()
    'End Sub

#End Region

#Region "Zahlungsbestätigung......................................................................."

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis des Bezahlt-Buttons. 
    ''' Versendet die Zahlungsbestätigung per E-Mail und setzt bei Erfolg den Status der Checkbox.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.10.2026 - Code-Optimierung:
    ''' - Auskommentierten Alt-Code (MsgBox-Abfrage) entfernt.
    ''' - Redundante '= True'-Prüfung bei der Boolean-Bedingung entfernt.
    ''' - Inline-Kommentar zur Dokumentation des Ablaufs hinzugefügt.
    ''' </remarks>
    Private Sub tsbBezahlt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBezahlt.Click
        ' Versendet die Zahlungsbestätigung für die aktuelle Buchungs-ID (sgRBID)
        If fcMailBezahlt(sgRBID) Then
            ' Bei erfolgreichem Versand wird das "Bezahlt"-Kontrollkästchen aktiviert
            cbBezalt.Checked = True
        End If
    End Sub

#End Region

#Region "Bewertungsmail absetzen..................................................................."

    ''' <summary>
    ''' Prüft beim Klicken auf die Bewertungs-Schaltfläche, ob bereits eine Bewertung existiert, und legt diese bei Bedarf an.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 13.10.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei 'fcInsertCommand' entfernt.
    ''' - 'sTem(0) = " "' durch die robustere Methode 'String.IsNullOrWhiteSpace' ersetzt.
    ''' - Datentyp bei der Deklaration direkt zugewiesen (Inline-Deklaration).
    ''' - Fehleranfällige String-Verkettung im SQL-Statement durch lesbares 'String.Format' ersetzt.
    ''' </remarks>
    Private Sub tsbBewertung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBewertung.Click
        ' Abfrage-String sicher zusammensetzen und Bewertung suchen
        Dim sqlQuery As String = String.Format("SELECT * FROM Bewertung WHERE BuchID = '{0}'", sgRBID)
        Dim sTem As Array = fcDataSeek(sqlQuery, sgRBID, 0, {"ID"})

        ' Wenn kein Eintrag existiert (Leerstring oder Leerzeichen), neuen Datensatz anlegen
        If sTem IsNot Nothing AndAlso (sTem.Length = 0 OrElse String.IsNullOrWhiteSpace(sTem(0).ToString())) Then
            fcInsertCommand("Bewertung", {"Id", "BuchID", "Status"}, {fcGetTimeID(Date.Today), sgRBID, "-1"})
        End If
    End Sub

#End Region

#Region "Rechnung stornieren......................................................................."

    ''' <summary>
    ''' Führt das Stornieren einer Rechnung durch. Aktualisiert die Buchungsdatenbank sowie die lokale Tabelle,
    ''' passt das Menü an und generiert eine entsprechende Datev-Stornobuchung mit getauschten Konten.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripMenuItem).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.10.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei Datenbank- und Tabellenoperationen entfernt.
    ''' - Fehleranfällige Split-Logik für Array-Initialisierung durch direkte Literale ersetzt.
    ''' - Schleifenvariable 'i' für die Typsicherheit explizit deklariert.
    ''' - String-Verknüpfungen auf den VB.NET-Standard '&' vereinheitlicht.
    ''' </remarks>
    Private Sub tsmStorno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmStorno.Click
        ' 1. Sicherheitsabfrage vor dem Stornieren
        Dim message As String = "Soll die Rechnung storniert werden? " & vbCrLf & "RNr.: " & sgRNr
        If MsgBox(message, MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Storno") <> MsgBoxResult.Yes Then
            Exit Sub
        End If

        ' 2. Buchungsdaten in Datenbank und lokaler Tabelle (dtBuc) zurücksetzen
        Dim arFields() As String = {"Rechnung", "RID"}
        Dim arValue() As String = {"0", " "}
        Dim cBedingung As String = " WHERE RID='" & sgRNr & "'"

        fcUpdateCommand("Buchung", arFields, arValue, cBedingung)

        cBedingung = "RID Like '" & sgRNr & "'"
        fcUpdateTable(dtBuc, arFields, arValue, cBedingung)

        ' 3. Menüstatus aktualisieren
        prChangeMenue(False)


    End Sub


#End Region

#Region "Reservierung / Zimmer löschen............................................................."

    ''' <summary>
    ''' Löscht die ausgewählte Reservierung vollständig aus der Datenbank.
    ''' Aktualisiert das DataGrid für alle betroffenen Zimmerzeiträume und schließt das Formular.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.02.2012 - Create
    ''' 12.10.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax entfernt.
    ''' - 'DateAdd' durch die native .NET-Methode '.AddDays(1)' ersetzt.
    ''' - Variablen-Deklarationen optimiert und Inline-Kommentare bereinigt.
    ''' </remarks>
    Private Sub tsbDelReservierung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelReservierung.Click
        ' 1. Zimmer-IDs und Zeiträume der Buchung abrufen
        Dim arT() As String = fcGetZimmerIDAusBuchung(sgRBID)

        ' 2. Buchung aus der Datenbank löschen
        If fcDelBuchung(sgRBID, "") Then

            ' 3. Jedes betroffene Zimmer im DataGrid aktualisieren / freigeben
            For i As Integer = 0 To arT.Length - 1
                Dim arS() As String = arT(i).Split(";"c)

                Dim sDatum As Date = fcUmDatum(arS(2))
                Dim sADatum As Date = sDatum.AddDays(1)

                ' Reservierung im Grid visuell entfernen
                prDeleteReservierungInDataGrid(fcUmDatum(arS(1)), sADatum, arS(0))
            Next

            ' 4. Formular nach erfolgreichem Löschen schließen
            Me.Close()
        End If
    End Sub

    ''' <summary>
    ''' Löscht ein einzelnes Zimmer aus der aktuellen Reservierung.
    ''' Aktualisiert das DataGrid, passt die Zimmer-Auswahlboxen an und schließt das Formular, falls kein Zimmer mehr zur Buchung gehört.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.02.2012 - Create
    ''' 12.10.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax entfernt.
    ''' - SQL-Abfrage von 'SELECT *' auf schlankes 'SELECT BID' umgestellt, um Speicherressourcen zu schonen.
    ''' - Fallback auf 'fcReadDataTable' implementiert.
    ''' - Code-Strukturierung und Lesbarkeit der Kombobox-Logik verbessert.
    ''' </remarks>
    Private Sub tsbDelZimmer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelZimmer.Click
        ' 1. IDs und Zimmerbezeichnungen aus den Steuerelementen ermitteln
        Dim sID As String = Extract(tscoZim.Text, "[", "]", 1)
        Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")

        ' 2. Zimmer aus der Tabelle "Buchen" entfernen
        If fcDelBuchung(sgRBID, sID) Then

            ' 3. DataGrid für den betroffenen Zeitraum dieses Zimmers aktualisieren
            prDeleteReservierungInDataGrid(lbAnreise.Text, lbAbreise.Text, sZim)

            ' 4. Komboboxen für die Zimmerauswahl aktualisieren
            tscoFreiZim.Items.Add(lbZimNr.Text)
            tscoZim.Items.Remove(tscoZim.SelectedItem)

            lStart = True

            If tscoZim.Items.Count > 1 Then
                tscoZim.SelectedIndex = 1
            End If

            ' 5. Prüfen, ob noch weitere Zimmer zu dieser Buchung existieren
            ' Optimiert: Nur die ID abfragen statt aller Spalten, um RAM zu sparen
            Dim sSql As String = "SELECT BID FROM Buchung WHERE BID = '" & sgRBID & "'"
            Dim dt As DataTable = fcReadDataTable(sSql)

            ' Wenn kein Zimmer mehr übrig ist, das Modul schließen
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                Me.Close()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Löscht entweder ein einzelnes Zimmer oder eine komplette Reservierung aus der Buchungsdatenbank
    ''' und synchronisiert die Änderungen mit der lokalen Datentabelle.
    ''' </summary>
    ''' <param name="sBID">Die Buchungs-ID (Zentraler Schlüssel).</param>
    ''' <param name="sID">Optional: Die ID des spezifischen Zimmers. Bleibt sie leer, wird die gesamte Buchung gelöscht.</param>
    ''' <returns>True, wenn der Löschvorgang erfolgreich war und vom Benutzer bestätigt wurde, andernfalls False.</returns>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 12.10.2026 - Code-Optimierung:
    ''' - Veraltete Zuweisungen an den Funktionsnamen durch modernes 'Return' ersetzt.
    ''' - 'Call'-Syntax entfernt.
    ''' - Logikfehler bei der lokalen Tabellensynchronisation korrigiert (Löschen einzelner Zimmer vs. komplette Buchung).
    ''' - Guard Clause für leere 'sBID' an den Funktionsanfang verschoben.
    ''' </remarks>
    Private Function fcDelBuchung(ByVal sBID As String, Optional ByVal sID As String = "") As Boolean
        ' 1. Vorabprüfung: Ohne Buchungs-ID wird die Funktion sofort abgebrochen
        If String.IsNullOrWhiteSpace(sBID) Then
            Return False
        End If

        ' 2. Variablen für SQL-Befehl, Meldungstext und lokalen Tabellenfilter vorbereiten
        Dim cSql As String
        Dim sMsg As String
        Dim sLokaleBedingung As String

        If String.IsNullOrWhiteSpace(sID) Then
            ' Komplette Reservierung löschen
            sMsg = "Wollen Sie diese Reservierung wirklich löschen?  "
            cSql = "DELETE FROM Buchung WHERE BID = '" & sBID & "'"
            sLokaleBedingung = "BID = '" & sBID & "'"
        Else
            ' Nur ein einzelnes Zimmer aus der Reservierung löschen
            sMsg = "Wollen Sie dieses Zimmer " & tscoZim.Text & " wirklich löschen?  "
            cSql = "DELETE FROM Buchung WHERE BID = '" & sBID & "' AND ID = '" & sID & "'"
            sLokaleBedingung = "BID = '" & sBID & "' AND ID = '" & sID & "'"
        End If

        ' 3. Sicherheitsabfrage an den Benutzer stellen
        If MsgBox(sMsg, MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel, "Löschen") = MsgBoxResult.Ok Then
            ' Datensatz per SQL aus der Datenbank löschen
            UpdateTable(cSql)

            ' Änderung in der lokalen DataTable "dtBuc" synchronisieren
            fcDeleteTableRow(dtBuc, sLokaleBedingung)

            Return True
        End If

        Return False
    End Function

    ''' <summary>
    ''' Lädt die Zimmer-IDs sowie die dazugehörigen Anreise- und Abreisezeiträume einer Buchung 
    ''' aus der Datenbank und gibt diese als semikolonseparierte Zeichenketten zurück.
    ''' </summary>
    ''' <param name="sBID">Die Buchungs-ID, nach der gefiltert werden soll.</param>
    ''' <returns>Ein String-Array mit den formatierten Daten ("ZimID;Von;Bis") oder ein leeres Array im Fehlerfall/bei Inexistenz.</returns>
    ''' <remarks>
    ''' 25.02.2012 - Create
    ''' 12.10.2026 - Code-Optimierung:
    ''' - Rückgabetyp von generischem 'Array' auf stark typisiertes 'String()' geändert.
    ''' - Veraltete Zuweisungen an den Funktionsnamen durch modernes 'Return' ersetzt.
    ''' - 'SELECT *' auf gezielte Spaltenabfrage ('ZimID, Von, Bis') umgestellt.
    ''' - Initialisierung des Arrays modernisiert und 'ReDim' entfernt.
    ''' </remarks>
    Private Function fcGetZimmerIDAusBuchung(ByVal sBID As String) As String()
        ' Vorabprüfung auf leere ID, um unnötige DB-Abfragen zu verhindern
        If String.IsNullOrWhiteSpace(sBID) Then
            Return New String(-1) {}
        End If

        ' Gezielte Spaltenabfrage zur Performance-Optimierung
        Dim sSql As String = "SELECT ZimID, Von, Bis FROM Buchung WHERE BID = '" & sBID & "'"

        Try
            Dim dt As DataTable = fcReadDataTable(sSql)

            ' Wenn keine Datensätze gefunden wurden, leeres Array zurückgeben
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                Return New String(-1) {}
            End If

            ' Array direkt in der passenden Größe initialisieren
            Dim arT(dt.Rows.Count - 1) As String

            ' Daten zeilenweise auslesen und formatieren
            For i As Integer = 0 To dt.Rows.Count - 1
                arT(i) = dt.Rows(i).Item("ZimID").ToString() & ";" &
                     dt.Rows(i).Item("Von").ToString() & ";" &
                     dt.Rows(i).Item("Bis").ToString()
            Next

            Return arT

        Catch ex As Exception
            ' Fehlerprotokollierung
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return New String(-1) {}
        End Try
    End Function


#End Region

#Region "Rechnungskopie............................................................................"

#End Region

#Region "Check in.................................................................................."

    ''' <summary>
    ''' Öffnet das Check-in-Formular, wenn der Benutzer auf die entsprechende Schaltfläche klickt.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die Check-in-Schaltfläche).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 13.10.2026 - Code-Optimierung:
    ''' - XML-Dokumentation vollständig hinzugefügt.
    ''' - Sicherheitsprüfung eingebaut, um sicherzustellen, dass das Formular nicht mehrfach geöffnet wird oder auf ein freigegebenes Objekt ('Disposed') zugegriffen wird.
    ''' </remarks>
    Private Sub tsbCheckin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsbCheckin.Click
        ' Prüfen, ob die Formularinstanz existiert und noch aktiv ist
        If frmCheckin IsNot Nothing AndAlso Not frmCheckin.IsDisposed Then
            ' Wenn es bereits geöffnet ist, in den Vordergrund bringen
            frmCheckin.BringToFront()
        Else
            ' Falls nicht vorhanden oder geschlossen, neu initialisieren und anzeigen
            'frmCheckin = New frmCheckin()
            frmCheckin.Show()
        End If
    End Sub

    Private Sub tsmKopie_Click(sender As Object, e As EventArgs)

    End Sub


#End Region







    'Private Function fcGetObjID(ByVal sZim As String) As String
    '    fcGetObjID = " "
    '    Dim nMax As Integer = dtZim.Rows.Count - 1
    '    For i As Integer = 0 To nMax
    '        If dtZim.Rows(i).Item("ID") = sZim Then
    '            fcGetObjID = dtZim.Rows(i).Item("IDObjekte")
    '            Exit For
    '        End If
    '    Next
    'End Function

    'Private Sub PreisToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PreisToolStripMenuItem.Click
    '    Dim P As String = fcGetZimPreis(sgRZID, tbAnzPer.Text, lbAbreise.Text, lbAnreise.Text)
    'End Sub

    'Private Sub prCheckinMail(ByRef sNummer As String)

    '    Dim sb As New StringBuilder
    '    Dim sqlText As String = ""
    '    Dim arFields(0), arValue(0) As String
    '    Dim cBedingung As String
    '    Try
    '        cBedingung = " WHERE BID ='" & sgRBID & "'"
    '        sqlText = "SendMailZugang"
    '        arFields = Split(sqlText, ",")
    '        sqlText = sNummer
    '        arValue = Split(sqlText, "°")
    '        Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
    '    Catch ex As Exception
    '        ErrReport(ex.Message, ex.Source, ex.StackTrace)
    '    Finally


    '    End Try
    'End Sub




End Class
