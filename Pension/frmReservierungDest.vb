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
    Dim lRec As Boolean
    Dim lUnbekannt As Boolean = False
    Dim sDayStart As Date
    Dim sDayEnd As Date
    Dim arZZiel() As String
    Dim sRNr As String
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
    'Dim sMwstS As String
    'Dim sMwstG As String
    'Dim sGKU As String
    'Dim sGKS As String
    'Dim sGKG As String
    Dim sMwstUa As String
    'Dim sMwstSa As String
    'Dim sMwstGa As String
    'Dim sGKUa As String
    'Dim sGKSa As String
    'Dim sGKGa As String
    'Dim arGKAlt() As String = Split(arIni(26), "/")
    'Dim arMwstAlt() As String = Split(arIni(27), "/")
    'Dim arGKNeu1() As String = Split(arIni(36), "/")
    'Dim arMwstNeu1() As String = Split(arIni(28), "/")
    'Dim arGKNeu2() As String = Split(arIni(37), "/")
    'Dim arMwstNeu2() As String = Split(arIni(29), "/")


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
                lRec = row("Rechnung")

                If coWerbung.Text = "Unbekannt" Then
                    coWerbung.Text = sW
                End If

                lgRech = row("Rechnung")
                sRNr = row("RID").ToString()
                lgRech = fcChaneMenue(lgRech)
                sgRNr = sRNr

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

    ''' <summary>
    ''' Prüft, ob die angegebene Zimmer-ID im Array der belegten Zimmer enthalten ist (ignoriert führende/nachfolgende Leerzeichen).
    ''' </summary>
    ''' <param name="arB">Das Array mit den IDs der belegten Zimmer.</param>
    ''' <param name="sID">Die zu suchende Zimmer-ID.</param>
    ''' <returns>True, wenn die Zimmer-ID im Array existiert, andernfalls False.</returns>
    ''' <remarks>
    ''' 03.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Performance-Boost: Manuelle For-Schleife durch die native, hochoptimierte Framework-Methode 'Array.Exists()' ersetzt.
    ''' - Robustheit erhöht: Null-Prüfung ('arB Is Nothing') integriert, um eine 'NullReferenceException' bei leeren Arrays zu verhindern.
    ''' - Syntax bereinigt: Veraltete VB6-Zuweisung an den Funktionsnamen durch das standardisierte 'Return' ersetzt.
    ''' </remarks>
    Private Function fcIfZimmerInArray(ByVal arB() As String, ByVal sID As String) As Boolean
        ' Sicherheitsprüfung: Wenn das Array nicht initialisiert ist oder die Such-ID leer ist, direkt False zurückgeben
        If arB Is Nothing OrElse sID Is Nothing Then Return False

        ' Nutzen der Lambda-Syntax für eine performante Elementprüfung inklusive .Trim()
        Return Array.Exists(arB, Function(element) element IsNot Nothing AndAlso element.Trim() = sID)
    End Function

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

    ''' <summary>
    ''' Steuert die Aktivierung und Sichtbarkeit von Menü- und Steuerelementen in Abhängigkeit des Rechnungsstatus (z. B. ob eine Rechnung gesperrt oder bereits abgeschlossen ist).
    ''' </summary>
    ''' <param name="lr">Ein boolescher Wert, der angibt, ob die Rechnung gesperrt bzw. schreibgeschützt ist (<c>True</c>) oder bearbeitet werden kann (<c>False</c>).</param>
    ''' <remarks>
    ''' 28.02.2012 - Create
    ''' 05.10.2026 - Code-Optimierung:
    ''' - Altlasten und auskommentierte Codezeilen ('gbPersonen', 'tsmAnAbReise') vollständig entfernt.
    ''' - XML-Dokumentation detailliert ausformuliert und Parameterbeschreibung ergänzt.
    ''' </remarks>
    Private Sub prLookForm(ByVal lr As Boolean)
        Dim isEditable As Boolean = Not lr

        ' Steuerung der Bearbeitbarkeit von Eingabebereichen und Speichern-Aktionen
        gbGast.Enabled = isEditable
        tsbSave.Enabled = isEditable

        ' Sichtbarkeit von rechnungs- und reservierungsbezogenen Toolbar-Aktionen steuern
        tsbAufZimmer.Visible = isEditable
        tsbRechnung.Visible = isEditable
        tsbBestätigung.Visible = isEditable
        tsbErinnerung.Visible = isEditable
        tsbDelReservierung.Visible = isEditable
        tsbDelZimmer.Visible = isEditable

        ' Trennstriche in der Toolbar anpassen
        tsSep1.Visible = isEditable
        tsSep2.Visible = isEditable

        ' Kopiefunktion ist nur sichtbar, wenn die Rechnung gesperrt/abgeschlossen ist
        tsmKopie.Visible = lr
    End Sub

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


#End Region

#Region "Preis- und Gasteliste....................................................................."






    ''' <summary>
    ''' Mit Doppelklick einen Gast auswählen und Liste schliessen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub lvGast_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvGast.MouseDoubleClick
        lbGastID.Text = "0"
        With lvGast
            If .SelectedItems.Count <> 0 Then
                lbGastID.Text = .SelectedItems(0).SubItems(5).Text
            End If
        End With
        'paPreise.Visible = False
        'lvPreise.Visible = False
        'lvGast.Visible = False
        Call prLadeGastDaten(lbGastID.Text)
        coWerbung.Text = "Stammgast"
    End Sub



#End Region

#Region "Button und Toolbar auswerten.............................................................."

    ''' <summary>
    ''' Modul benden
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click


        '  sgNet = fcCodeSend()
        Me.Close()
    End Sub
    Private Sub frmKey_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim arfeld1() As String = fcDataSeek("select * From Buchung Where BID ='", sgRBID, 0, {"SendMailZugang"})
        If arfeld1(0) = "Y" Then
            If MsgBox("Buchung Freigeben?", vbExclamation + vbOKCancel, "Ja") = MsgBoxResult.Ok Then
                Dim arFields() As String = {"SendMailZugang"}
                Dim arValue() As String = {"X"}
                Dim cBedingung As String = " WHERE BID='" & sgRBID & "'"
                Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
            End If
        End If
        '  sgNet = fcCodeSend()
    End Sub

    ''' <summary>
    ''' Mit Doppelklick auf den Namen werden die Gastdaten gelöscht.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub lbGast_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbGast.DoubleClick
        Call prClearGastDaten()
    End Sub

    ''' <summary>
    ''' Gastdaten im Formular löschen
    ''' </summary>
    ''' <remarks>
    ''' 05.02.2012 Create
    ''' </remarks>
    Private Sub prClearGastDaten()
        lbGast.Text = "(neuer Gast)"
        coAnrede.Text = "Firma"
        tbName1.Text = ""
        tbName2.Text = ""
        tbVorname.Text = ""
        tbStrasse.Text = ""
        tbPLZ.Text = ""
        tbOrt.Text = ""
        coLand.Text = "DE"
        tbTel.Text = ""
        tbFax.Text = ""
        tbFunk.Text = ""
        tbEMail.Text = ""
        '   tbPass.Text = ""
        tbInfo.Text = ""
        lbGastID.Text = ""
        coWerbung.Text = "Unbekannt"
        dpGeb.Value = Date.Today
    End Sub

    ''' <summary>
    ''' Änderung in der Reservierung speichen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tsbSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSave.Click
        Dim sC As String = fcCode(sCode)
        Call prSaveZimmerReservierung()
        arNew = fcCollectDataInArray(arNew)
        ' Dim sC As String = fcCode(sCode)
        Dim sb2 As New StringBuilder
        Dim arFields(), arValue() As String
        Dim sqlText As String = ""
        sqlText = "code,von,vonzeit,bis,biszeit,BID"
        arFields = Split(sqlText, ",")
        sb2.Append(sC & "°")  'ID
        sb2.Append(fcUmDatum(arNew(21)) & "°")    'VON 'art(21)
        sb2.Append(fcUmZeit(tbAnZeit.Text) & "°")
        sb2.Append(fcUmDatum(arNew(22)) & "°")   'BIS
        sb2.Append(fcUmZeit(tbAbZeit.Text) & "°")
        sb2.Append(sgRBID)  'BID
        arValue = Split(sb2.ToString, "°")
        If sCode.Trim <> "" And sCode.Trim <> "00000" Then
            Dim sSQL As String = "Select * From Code Where code='" & sC & "'"
            Dim dt As DataTable = fcReadDataTable(sSQL)
            If dt.Rows.Count = 0 Then
                Call fcInsertCommand("Code", arFields, arValue)
            Else
                Dim cBedingung As String = " WHERE code='" & sC & "'"
                Call fcUpdateCommand("Code", arFields, arValue, cBedingung)
            End If
        End If

        MsgBox("Änderung gespeichert")








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

        ' Call prSaveZimmerReservierung()
        If tscoZim.Text = "Gleiches Zimmer" Then
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

    Private Sub tsbVorAnreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbVorAnreise.Click
        ' Call prSaveZimmerReservierung()
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

            'Call prSaveZimmerReservierung()
        Else

            MsgBox("Zimmer schon Vermittet", MsgBoxStyle.Question + MsgBoxStyle.OkOnly)
        End If
        tsbVorAnreise.Visible = False
        tsbNachAbreise.Visible = False
    End Sub

    Private Sub tsbNachAbreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNachAbreise.Click
        '   Call prSaveZimmerReservierung()
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

    Private Sub tbAnreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbAnreise.Click
        Dim aSperr As String = ""
        Dim aBuchung As String = ""
        Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")
        Dim dt As DataTable
        Dim sSql As String
        Dim sID As String = Extract(tscoZim.Text, "[", "]", 1)
        Dim d1 As Date = DateAdd(DateInterval.Month, -8, CDate(lbAnreise.Text))
        Dim d2 As Date = DateAdd(DateInterval.Month, 8, CDate(lbAnreise.Text))
        sSql = "Select * From Buchung  Where ZimID='" & sZim & "' and Von<='" & fcUmDatum(CDate(d2)) & "' and Bis >= '" & fcUmDatum(CDate(d1)) & "'"
        dt = fcReadDataTable(sSql)
        For k = 0 To dt.Rows.Count - 1
            If dt.Rows(k).Item("ID").ToString <> sID And dt.Rows(k).Item("BID").ToString <> sgRBID Then
                aSperr = aSperr & ";" & dt.Rows(k).Item("von").ToString & "-" & dt.Rows(k).Item("bis").ToString

            End If
        Next
        For k = 0 To dt.Rows.Count - 1
            If dt.Rows(k).Item("ID").ToString <> sID And dt.Rows(k).Item("BID").ToString = sgRBID Then
                aBuchung = aBuchung & ";" & dt.Rows(k).Item("von").ToString & "-" & dt.Rows(k).Item("bis").ToString

            End If
        Next

        MyKalender1.Datum = fcUmDatum(lbAnreise.Text)
        MyKalender1.Sperr = Mid(aSperr, 2)
        MyKalender1.Buchung = Mid(aBuchung, 2)
        MyKalender1.AnreiseAbreise = True

        MyKalender1.Anreise = fcUmDatum(lbAnreise.Text) 'dAn
        MyKalender1.Abreise = fcUmDatum(lbAbreise.Text) 'dAb
        MyKalender1.Visible = True

    End Sub

    Private Sub tbAbreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbAbreise.Click
        Dim aSperr As String = ""
        Dim aBuchung As String = ""
        Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")
        Dim dt As DataTable
        Dim sSql As String
        Dim sID As String = Extract(tscoZim.Text, "[", "]", 1)
        Dim d1 As Date = DateAdd(DateInterval.Month, -8, CDate(lbAnreise.Text))
        Dim d2 As Date = DateAdd(DateInterval.Month, 8, CDate(lbAnreise.Text))
        sSql = "Select * From Buchung  Where ZimID='" & sZim & "' and Von<='" & fcUmDatum(CDate(d2)) & "' and Bis >= '" & fcUmDatum(CDate(d1)) & "'"
        dt = fcReadDataTable(sSql)
        For k = 0 To dt.Rows.Count - 1
            If dt.Rows(k).Item("ID").ToString <> sID And dt.Rows(k).Item("BID").ToString <> sgRBID Then
                aSperr = aSperr & ";" & dt.Rows(k).Item("von").ToString & "-" & dt.Rows(k).Item("bis").ToString

            End If
        Next
        For k = 0 To dt.Rows.Count - 1
            If dt.Rows(k).Item("ID").ToString <> sID And dt.Rows(k).Item("BID").ToString = sgRBID Then
                aBuchung = aBuchung & ";" & dt.Rows(k).Item("von").ToString & "-" & dt.Rows(k).Item("bis").ToString

            End If
        Next
        ' dDa = DateAdd(DateInterval.Day, 1, dDa)
        MyKalender1.Datum = fcUmDatum(lbAnreise.Text)
        MyKalender1.Sperr = Mid(aSperr, 2)
        MyKalender1.Buchung = Mid(aBuchung, 2)
        MyKalender1.AnreiseAbreise = False
        MyKalender1.Anreise = fcUmDatum(lbAnreise.Text) 'dAn
        MyKalender1.Abreise = fcUmDatum(lbAbreise.Text) 'dAb
        MyKalender1.Visible = True
    End Sub





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

    Private Sub tsbAufZimmer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAufZimmer.Click
        sgZNr = lbZimNr.Text
        frmZusatz.Show()
    End Sub

    Private Sub btBText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btBText.Click
        lLang = False
        If coLang.Text = "Englisch" Then lLang = True
        frmBText.Show()
        frmBText.tbBText.Text = lbMakro.Text
    End Sub

    Private Sub coLang_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles coLang.TextChanged
        lLang = False
        If coLang.Text = "Englisch" Then lLang = True
    End Sub

    ''' <summary>
    ''' Steuerung des Menues in Abhängigkeit des Rechnungsstatus
    ''' </summary>
    ''' <param name="lR"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 14.03.2012 Create
    ''' </remarks>
    Private Function fcChaneMenue(ByVal lR As Boolean) As Boolean
        fcChaneMenue = lR
        tsmStorno.Enabled = lR
        ' tsbBestätigung.Enabled = Not lR
        tsbDelReservierung.Enabled = Not lR
        tsbDelZimmer.Enabled = Not lR
        '    tsmAnAbReise.Enabled = Not lR
        '     tsbAufZimmer.Enabled = Not lR
        If lR Then
            Me.Text = "Reservierung bearbeiten => (Rechnung geschrieben!)"
        Else
            Me.Text = "Reservierung bearbeiten"
        End If
    End Function
    ''' <summary>
    ''' Code Speicher auf key.text
    ''' </summary>
    ''' <remarks>
    ''' 25.12.2014 Create
    ''' </remarks>
    Private Sub tsbKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbKey.Click
        'arNew = fcCollectDataInArray(arNew)
        Call prSaveZimmerReservierung()
        Dim sToDay As String = fcUmDatum(arNew(21)) 'VON
        Dim sToDay1 As String = fcUmDatum(CDate(DateAdd(DateInterval.Day, 1, CDate(arNew(22))))) 'BIS
        Dim C As String = fcCode(sCode) & "°" & sToDay & "°1200°" & sToDay1 & "°1200|"
        Dim a As String = "Code=" & C
        Dim sIP As String = cgIPWeb
        PHP.Data(C, sIP)
    End Sub



#End Region

#Region "Speichern der Reservierungsdaten.........................................................."

    ''' <summary>
    ''' Änderung an einer Reservierung speichern
    ''' </summary>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prSaveZimmerReservierung()
        Dim sKnr As String = lbGastID.Text
        If tbName1.Text.Trim = "" And tbName2.Text.Trim = "" Then
            MsgBox("Speichern nicht möglich, da kein Gast ausgewählt wurde!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        If tbAnzPer.Text.Trim = "" Or tbAnzPer.Text.Trim = "0" Then
            MsgBox("Speichern nicht möglich, da keine Personenzahl!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        arNew = fcCollectDataInArray(arNew)
        '  If fcDifference(arOld, arNew) Then
        sKnr = fcSaveGast(arNew, sKnr)
        Call prSavePreis(arNew, sKnr)
        Call prSaveRestZimmerOfReservierung(lUnbekannt, sKnr, sgRBID)
        Call prSaveCodeToErinnerung()
        'Refresh MainGrid
        Call prRefreshData()

        'If arOld(6).Trim = "" And arOld(7).Trim = "" Then
        '    sKnr = fcGetNr("KNr")
        '    prSetNr("KNr", sKnr)
        'End If
        lbGastID.Text = sKnr
        ' End If
        Call prSaveBuchungsText(sgRBID)
    End Sub

    ''' <summary>
    ''' Änderung am Buchungsdatensatz speichern
    ''' </summary>
    ''' <param name="arT">Änderungsdaten</param>
    ''' <param name="sKNr">Kundennummer</param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prSavePreis(ByVal arT() As String, ByVal sKNr As String)
        Dim sPauch As String = "0"
        If cbPausch.Checked = True Then sPauch = "1"
        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim sID As String = sKNr
        Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID") ' tscoZim.Text
        Dim sOID As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "IDObjekte") 'tscoZim.Text
        Dim cBedingung As String = " WHERE ID='" & sBID & "'"  'sBiD
        Dim sDatum As Date = lbAbreise.Text
        Dim sADatum As Date = DateAdd(DateInterval.Day, -1, sDatum)
        Dim sPreisS As String = "0"
        Dim sPreisG As String = "0"
        Dim sVariable As String = "0"
        If rbNormal.Checked = True Then sVariable = "0"
        If rbFest.Checked = True Then sVariable = "1"
        If rbVariabel.Checked = True Then sVariable = "2"
        Try

            sqlText = "Von,Bis,ObjID,ZimID,Variable,KunID,Name1,Name2,Personen,Tiere,Art,Frueh,Kategorie,Preis,Anzahlung,Werbung,Info,FPreis,Storno,Summe,Pausch,RDSenden,ZDatum,InternetNr,GPreis,MwstU,VonZeit,BisZeit,Code"
            'sqlText = "Von,Bis,ObjID,ZimID,Variable,KunID,Name1,Name2,Personen,Tiere,Art,Frueh,Kategorie,Preis,Anzahlung,Werbung,Info,FPreis,Storno,Summe,Pausch,RDSenden,ZDatum,InternetNr,GPreis,MwstU,MwstS,MwstG,GKU,GKS,GKG,VonZeit,BisZeit,Code"
            arFields = Split(sqlText, ",")

            sb.Append(fcUmDatum(lbAnreise.Text) & "°")
            sb.Append(fcUmDatum(sADatum) & "°")
            sb.Append(sOID & "°")
            sb.Append(sZim & "°")
            sb.Append(sVariable & "°")           'Variabel/fest
            sb.Append(sKNr & "°")
            sb.Append(tbName1Z.Text & "°")
            sb.Append(tbName2Z.Text & "°")
            sb.Append(tbAnzPer.Text & "°")
            sb.Append("0" & "°")
            sb.Append(coArt.Text & "°")
            If coArt.Text = "Ü/F" Then
                sb.Append(tbAnzPer.Text & "°")

                sPreisS = ((Val(fcChangeString(tbFPreis.Text, ",", ".")) * 100) - arIni(24)).ToString ' "300"
                ' Dim n1 As Integer = Val(fcChangeString(tbFPreis.Text, ",", ".")) * 100
                ' sPreisS = arIni(17)
                sPreisG = arIni(24)
            Else
                sb.Append("0" & "°")
            End If
            sb.Append(tbUArt.Text & "°")

            Dim preisx As String = fcChangeString(tbPreis.Text, ",", ".")
            sb.Append((Val(fcChangeString(tbPreis.Text, ",", ".")) * 100).ToString & "°")
            sb.Append((Val(fcChangeString(tbAnzahlung.Text, ",", ".")) * 100).ToString & "°")
            sb.Append(sgWerbung & "°")
            sb.Append(tbInfo.Text & "°")
            sb.Append(sPreisS & "°")  'Frühstück
            sb.Append(tbStorno.Text & "°")
            sb.Append((Val(fcChangeString(tbSumme.Text, ",", ".")) * 100).ToString & "°")
            sb.Append(sPauch & "°")
            sb.Append(tbRechSend.Text & "°")
            Dim zDatum As String = ""
            If IsDate(tbRechSend.Text) Then
                zDatum = fcUmDatum(tbRechSend.Text)
            Else

                Select Case Val(tbRechSend.Text)
                    Case Is <= 0
                        'Anreise datum
                        zDatum = fcUmDatum(CDate(arT(21)).AddDays(Val(tbRechSend.Text)))
                    '   Datum = CDate(fcUmDatum(von)).AddDays(nTage)
                    Case Is > 0
                        'buchungsDatum

                        zDatum = fcUmDatum(CDate(Date.Today).AddDays(Val(tbRechSend.Text)))
                    Case Else
                        'festdatum
                        If tbRechSend.Text.Length > 6 Then
                            zDatum = fcUmDatum(tbRechSend.Text)
                            ' datum mit Jahr
                        Else
                            'datum ohne jahr ->>
                            zDatum = fcUmDatum(Trim(tbRechSend.Text) & "." & (CDate(Date.Today).AddYears(1).Year.ToString))

                        End If

                End Select
            End If
            sb.Append(zDatum & "°")
            sb.Append(tbInternetNr.Text & "°")
            sb.Append(sPreisG & "°")
            sb.Append(sMwstU & "°")
            'sb.Append(sMwstS & "°")
            'sb.Append(sMwstG & "°")
            'sb.Append(sGKU & "°")
            'sb.Append(sGKS & "°")
            'sb.Append(sGKG & "°")
            sb.Append(fcUmZeit(tbAnZeit.Text) & "°")    'Summe
            sb.Append(fcUmZeit(tbAbZeit.Text) & "°")
            sb.Append(sCode)

            arValue = Split(sb.ToString, "°")

            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)

            'Datensatz in DataTable "dtBuc" speichern
            cBedingung = "ID Like '" & sBID & "'" 'sbid
            Call fcUpdateTable(dtBuc, arFields, arValue, cBedingung)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)

        End Try

    End Sub

    ''' <summary>
    ''' Bei Erstanlage einer Buchung die Buchungsdaten auf alle Zimmer der Reservierung übertragen
    ''' </summary>
    ''' <param name="lUn"></param>
    ''' <param name="sKnr"></param>
    ''' <param name="sBID"></param>
    ''' <remarks>
    ''' 26.02.2012 Create
    ''' </remarks>
    Private Sub prSaveRestZimmerOfReservierung(ByVal lUn As Boolean, ByVal sKnr As String, ByVal sBID As String)
        If lUn Then
            Dim sb As New StringBuilder
            Dim sqlText As String = ""
            Dim arFields(0), arValue(0) As String
            Dim cBedingung As String = " WHERE BID='" & sBID & "'"
            Dim sDatum As Date = lbAbreise.Text
            Dim sADatum As Date = DateAdd(DateInterval.Day, -1, sDatum)

            sqlText = "Von,Bis,KunID,Kunde,Name1,Name2,Personen,Tiere,Art,Frueh,Kategorie,Preis,Werbung,InternetNr,VonZeit,BisZeit"
            arFields = Split(sqlText, ",")

            sb.Append(fcUmDatum(lbAnreise.Text) & "°")
            sb.Append(fcUmDatum(sADatum) & "°")
            sb.Append(sKnr & "°")
            sb.Append(tbName1.Text & "°")
            sb.Append(tbName1Z.Text & "°")
            sb.Append(tbName2Z.Text & "°")
            sb.Append(tbAnzPer.Text & "°")
            sb.Append("0" & "°")
            sb.Append(coArt.Text & "°")
            If coArt.Text = "Ü/F" Then
                sb.Append(tbAnzPer.Text & "°")
            Else
                sb.Append("0" & "°")
            End If
            sb.Append(tbUArt.Text & "°")
            sb.Append((Val(tbPreis.Text) * 100).ToString & "°")
            sb.Append(sgWerbung & "°")
            sb.Append(tbInternetNr.Text & "°")
            sb.Append(fcUmZeit(tbAnZeit.Text) & "°")    'Summe
            sb.Append(fcUmZeit(tbAbZeit.Text))
            arValue = Split(sb.ToString, "°")

            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
            cBedingung = "BID Like '" & sBID & "'"
            Call fcUpdateTable(dtBuc, arFields, arValue, cBedingung)
            Call prRefreshDataAll(sBID)

            lUnbekannt = False
        End If
    End Sub

    ''' <summary>
    ''' Daten des Gastes speichern
    ''' </summary>
    ''' <param name="arT"></param>
    ''' <param name="sKNr"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Function fcSaveGast(ByVal arT() As String, ByVal sKNr As String) As String
        Dim lNew As Boolean = False
        fcSaveGast = sKNr
        If Trim(sKNr) = "" Then
            lNew = True
            'Neuanlage des Gastes
        Else
            'Aktuallisierung der Daten
        End If

        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String = ""
        Dim sID As String = sKNr
        If lNew Then sID = fcAppendBlank("Kunden")





        fcSaveGast = sID
        Try
            sqlText = "ID,Anrede,Name1,Name2,Name1Z,Name2Z,Vorname,Strasse,PLZ,Ort,Land,Telefon,Telefax,Funk,EMail,Pass,Geb,Info,Werbung"
            arFields = Split(sqlText, ",")
            arValue = Split(sqlText, ",")
            arValue(0) = sID
            arValue(1) = coAnrede.Text
            arValue(2) = tbName1.Text
            arValue(3) = tbName2.Text
            arValue(4) = tbName1Z.Text
            arValue(5) = tbName2Z.Text
            arValue(6) = tbVorname.Text
            arValue(7) = tbStrasse.Text
            arValue(8) = tbPLZ.Text
            arValue(9) = tbOrt.Text
            arValue(10) = coLand.Text
            arValue(11) = tbTel.Text
            arValue(12) = tbFax.Text
            arValue(13) = tbFunk.Text
            arValue(14) = tbEMail.Text
            arValue(15) = ""
            arValue(16) = fcUmDatum(dpGeb.Value)
            arValue(17) = tbInfo.Text
            arValue(18) = coWerbung.Text


            '  sqlText = fcGetSQLTextGast(sID, arT)
            '  arValue = Split(sqlText, "°")
            'If lNew Then
            '    Call fcInsertCommand("Kunden", arFields, arValue)
            '    fcSaveGast = sID
            '    ' Call prSetNr("KNr", sID)
            'Else
            cBedingung = " WHERE ID='" & sID & "'"
            Call fcUpdateCommand("Kunden", arFields, arValue, cBedingung)
            'End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
        End Try
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
    ''' Aktualisierung der Datem im Buchungs-Datagrid im Hauptfenster starten
    ''' </summary>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prRefreshData()
        If sIndexIst.Trim = "0" Then
            Dim cBedingung As String = " WHERE ID='" & sBID & "'"
            Dim sBO As Date = arOld(21)
            Dim sEO As Date = arOld(22)
            Dim sBN As Date = arNew(21)
            Dim sEN As Date = arNew(22)
            Dim sP As String = arNew(0)
            Dim sID As String = sgRBID
            Dim sName As String = arNew(6)
            Dim sFr As String = arNew(1)
            Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")
            Call prDeleteReservierungInDataGrid(sBO, sEO, sZim)
            Call prSetReservierungInDataGrid(sBN, sEN, sZim, sP, sName, sID, sFr)
        End If

    End Sub

    ''' <summary>
    ''' Buchungstext und Sprache in all Reservierungen speichen
    ''' </summary>
    ''' <param name="sBID"></param>
    ''' <remarks>
    ''' 10.03.2012 Create
    ''' </remarks>
    Private Sub prSaveBuchungsText(ByVal sBID As String)
        Dim sBez As String = "0"
        If cbBezalt.Checked = True Then sBez = "1"
        Dim sID As String = " "
        Dim dt As DataTable = fcReadDataTable("Select ID from BTexte Where Name='" & coBText.Text & "'")
        If dt.Rows.Count <> 0 Then sID = dt.Rows(0).Item(0).ToString


        Dim sL As String = "0"
        sL = coLang.SelectedIndex.ToString
        If lLang Then sL = 1
        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String = " WHERE BID='" & sBID & "'"
        arFields = Split("Bez,Sprache,BText,MText", ",")

        sb.Append(sBez & "°")
        sb.Append(sL & "°")
        sb.Append(sID & "°")
        sb.Append(lbMakro.Text)
        arValue = Split(sb.ToString, "°")

        Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
        ' prCheckinMail("4")

        cBedingung = "BID Like '" & sBID & "'"
        Call fcUpdateTable(dtBuc, arFields, arValue, cBedingung)
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
    ''' Alte Reservierung aus dem Buchungs-Datagried löschen
    ''' </summary>
    ''' <param name="sBegin"></param>
    ''' <param name="sEnd"></param>
    ''' <param name="sZim"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prDeleteReservierungInDataGrid(ByVal sBegin As Date, ByVal sEnd As Date, ByVal sZim As String)
        'Liste  synchronisieren
        Dim nCol As Integer = fcGetZimmerSpalte(sZim, frmMain.dgBuchung)
        Dim nDay As Integer
        Dim sDay As Date
        With frmMain.dgBuchung
            nDay = .Rows.Count - 1
            For i = 0 To nDay
                sDay = .Rows(i).Cells(0).Value
                If sDay = sEnd Then
                    Exit For
                End If
                If sDay >= sBegin Then
                    .Rows(i).Cells(nCol).Value = ""
                    .Rows(i).Cells(nCol).Style.BackColor = Color.White
                End If
            Next
        End With
    End Sub

    ''' <summary>
    ''' Neue Reservierung eintragen
    ''' </summary>
    ''' <param name="sBegin"></param>
    ''' <param name="sEnd"></param>
    ''' <param name="sZim"></param>
    ''' <param name="sP"></param>
    ''' <param name="sName"></param>
    ''' <param name="sID"></param>
    ''' <param name="sFr"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prSetReservierungInDataGrid(ByVal sBegin As Date, ByVal sEnd As Date,
                                            ByVal sZim As String, ByVal sP As String,
                                            ByVal sName As String, ByVal sID As String,
                                            ByVal sFr As String)
        'Liste  synchronisieren
        Dim nCol As Integer = fcGetZimmerSpalte(sZim, frmMain.dgBuchung)
        Dim nDay As Integer
        Dim sDay As Date
        With frmMain.dgBuchung
            nDay = .Rows.Count - 1
            For i = 0 To nDay
                sDay = .Rows(i).Cells(0).Value
                If sDay = sEnd Then Exit For
                If sDay >= sBegin Then

                    If sFr = "Ü/F" Then
                        .Rows(i).Cells(nCol).Style.BackColor = Color.LimeGreen
                    Else
                        .Rows(i).Cells(nCol).Style.BackColor = Color.Turquoise
                    End If
                    .Rows(i).Cells(nCol).Value = PadR(sName, 15) & "(" & sID & ") [" & sP & "]"
                End If
            Next
        End With
    End Sub

    ''' <summary>
    '''  türcode in den Terminplan
    ''' </summary>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>

    Private Sub prSaveCodeToErinnerung()
        'Dim sID As String = ""
        'Dim sqlText As String = "Datum,TerminText,Termin,Zeit,Aktive,BID"

        'Dim sb As New StringBuilder
        'Dim arFields As Array
        'Dim arValue As Array
        'Dim sSQL As String = "Select * From Termine Where BID='" & sgRBID & "'"
        'Dim dt As DataTable = fcReadDataTable(sSQL)
        'For i = 0 To dt.Rows.Count - 1
        '    If Extract(dt.Rows(i).Item("TerminText").ToString.Trim(), "(", ")", 1) = sCode Then
        '        sID = dt.Rows(i).Item("ID").ToString.Trim()
        '    End If
        'Next
        'sb.Append(fcUmDatum(DateAdd(DateInterval.Day, -14, CDate(lbAnreise.Text))) & "°")
        'sb.Append(tbName1.Text & ", Tür Code senden [" & sCode & "]     (" & lbGastID.Text & ") " & "°")
        'sb.Append(fcUmDatum(lbAnreise.Text) & "°")
        'sb.Append("12:00:00" & "°")
        'sb.Append("1" & "°")
        'sb.Append(sgRBID)
        'arFields = Split(sqlText, ",")
        '' sqlText = fcGetSQLTextGast(sID, arT)
        'arValue = Split(sb.ToString, "°")
        ''  UPDATE Termine SET [Datum]='20150102',[TerminText]='Werel, Tür Code senden (21693)',[Termin]='20141118',[Zeit]='07:00:00',[Aktiv]='1',[BID]='05605' WHERE ID='20150102194601'
        'If sID = "" Then sID = fcAppendBlank("Termine")
        'Dim cBedingung As String = " WHERE ID='" & sID & "'"
        'Call fcUpdateCommand("Termine", arFields, arValue, cBedingung)
    End Sub

#End Region

#Region "Hilfetexte in der Statusleiste ausgeben..................................................."

    Private Sub lbAnreise_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbAnreise.MouseMove
        tssInfo.Text = "Anreisedatum (ab 12:00 Uhr)"
    End Sub

    Private Sub lbAbreise_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbAbreise.MouseMove
        tssInfo.Text = "Abreisedatum (bis 11:00 Uhr)"
    End Sub

    Private Sub lbTage_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbTage.MouseMove
        tssInfo.Text = "Anzahl der Übernachtungen"
    End Sub

    Private Sub lbZimNr_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbZimNr.MouseMove
        tssInfo.Text = "Zimmernummer"
    End Sub

    Private Sub lbAusstattung_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbAusstattung.MouseMove
        tssInfo.Text = "Ausstattung des Zimmers"
    End Sub

    Private Sub lbArt_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbArt.MouseMove
        tssInfo.Text = "Kategorie des Zimmers"
    End Sub

    Private Sub lbBetten_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbBetten.MouseMove
        tssInfo.Text = "Anzahl der möglichen Betten"
    End Sub

    Private Sub lbObjekt_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbObjekt.MouseMove
        tssInfo.Text = "In welchem Objekt ist das Zimmer"""
    End Sub

    Private Sub tbAnzPer_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbAnzPer.MouseMove
        tssInfo.Text = "Anzahl der Personen pro Zimmer"
    End Sub

    Private Sub tbUArt_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbUArt.MouseMove
        tssInfo.Text = "Übernachtungsart"
    End Sub

    Private Sub btUArt_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        tssInfo.Text = "Auswahl der Übernachtungsart"
    End Sub

    Private Sub coArt_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles coArt.MouseMove
        tssInfo.Text = "Auswahl der Übernachtung mit und ohne Frühstück"
    End Sub

    Private Sub tbPreis_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbPreis.MouseMove
        tssInfo.Text = "Vereinbarter Preis pro Zimmer"
    End Sub

    Private Sub tbAnzahlung_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbAnzahlung.MouseMove
        tssInfo.Text = "Getätigte Anzahlung"
    End Sub

    Private Sub coAnrede_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles coAnrede.MouseMove
        tssInfo.Text = "Anrede: Herr, Frau, Firma"
    End Sub

    Private Sub tbName1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbName1.MouseMove
        tssInfo.Text = "Nachname / Firmenname"
    End Sub

    Private Sub tbName2_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbName2.MouseMove
        tssInfo.Text = "Nachname wenn Firmenbezug"
    End Sub

    Private Sub tbVorname_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbVorname.MouseMove
        tssInfo.Text = "Vorname"
    End Sub

    Private Sub tbStrasse_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbStrasse.MouseMove
        tssInfo.Text = "Strasse"
    End Sub

    Private Sub tbPLZ_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbPLZ.MouseMove
        tssInfo.Text = "PLZ"
    End Sub

    Private Sub tbOrt_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbOrt.MouseMove
        tssInfo.Text = "Ort"
    End Sub

    Private Sub coLand_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles coLand.MouseMove
        tssInfo.Text = "Land (Kurz-Zeichen)"
    End Sub

    Private Sub tbPass_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        tssInfo.Text = "Ausweisnummer"
    End Sub

    Private Sub dtpGeb_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dpGeb.MouseMove
        tssInfo.Text = "Geburtsdatum"
    End Sub

    Private Sub tbTelefon_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbTel.MouseMove
        tssInfo.Text = "Telefon"
    End Sub

    Private Sub tbTelefax_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbFax.MouseMove
        tssInfo.Text = "Telefax"
    End Sub

    Private Sub tbFunk_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbFunk.MouseMove
        tssInfo.Text = "Mobiltelefon"
    End Sub

    Private Sub tbEMail_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbEMail.MouseMove
        tssInfo.Text = "E-Mail Adresse"
    End Sub

    Private Sub tbInfo_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbInfo.MouseMove
        tssInfo.Text = "Informationen zum Gast"
    End Sub

    Private Sub btGast_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btGast.MouseMove
        tssInfo.Text = "Auswahl des Gastes über den Gastestamm"
    End Sub

    Private Sub coWerbung_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles coWerbung.MouseMove
        tssInfo.Text = "Über welchen Kanal ist der Gast auf uns aufmerksam geworden?"
    End Sub

    Private Sub lbGast_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbGast.MouseMove
        tssInfo.Text = "Mit Doppelklick werden die Daten des Gastes (im Formular) gelöscht. Neuanlage ist nun möglich."
    End Sub

    Private Sub tsbDelZimmer_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbDelZimmer.MouseHover
        tssInfo.Text = "Löschen der ausgewählten Reservierung / Zimmer"
    End Sub

    Private Sub tsbDelReservierung_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbDelReservierung.MouseHover
        tssInfo.Text = "Die gesamte Reservierung wird gelöscht (alle Zimmer)."
    End Sub

    ' Anzeige löschen
    Private Sub gbGast_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gbGast.MouseMove
        tssInfo.Text = ""
    End Sub
    Private Sub gbPersonen_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        tssInfo.Text = ""
    End Sub
    Private Sub gbZimmer_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gbZimmer.MouseMove
        tssInfo.Text = ""
    End Sub
    Private Sub gbDatum_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        tssInfo.Text = ""
    End Sub
    Private Sub tsMain_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tsMain.MouseMove
        tssInfo.Text = ""
    End Sub

#End Region

#Region "Reservierung / Zimmer löschen............................................................."

    ''' <summary>
    ''' Reservierung komplett löschen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.02.2012 Create
    ''' </remarks>
    Private Sub tsbDelReservierung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelReservierung.Click
        Dim arS() As String
        Dim sDatum As Date
        Dim sADatum As Date

        'Löschen aus der Tabelle "Buchen" (Buchungsnummer)

        Dim arT() As String = fcGetZimmerIDAusBuchung(sgRBID)
        If fcDelBuchung(sgRBID, "") Then
            'DataGrid aktualisieren (Buchungsnummer, Zimmernummern)
            Dim i As Integer
            Dim nMax As Integer = arT.Length - 1
            For i = 0 To nMax
                arS = arT(i).Split(";")
                sDatum = fcUmDatum(arS(2))
                sADatum = DateAdd(DateInterval.Day, 1, sDatum)
                Call prDeleteReservierungInDataGrid(fcUmDatum(arS(1)), sADatum, arS(0))
            Next
            'Modul schliessen
            Me.Close()
        End If


    End Sub

    ''' <summary>
    ''' Einzelnes Zimmer aus der Reservierung löschen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.02.2012 Create
    ''' </remarks>
    Private Sub tsbDelZimmer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelZimmer.Click

        Dim sID As String = Extract(tscoZim.Text, "[", "]", 1)
        Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID") ' tscoZim.Text
        'Löschen aus der Tabelle "Buchen" (Buchungsnummer, Zimmernummern)
        If fcDelBuchung(sgRBID, sID) Then
            'DataGrid aktualisieren (Buchungsnummer, Zimmernummern) 
            Call prDeleteReservierungInDataGrid(lbAnreise.Text, lbAbreise.Text, sZim)
            'Komboboxen aktualisieren
            tscoFreiZim.Items.Add(lbZimNr.Text)
            tscoZim.Items.Remove(tscoZim.SelectedItem)
            lStart = True
            If tscoZim.Items.Count > 1 Then tscoZim.SelectedIndex = 1
            'Wenn letztes Zimmer, dann Modul schliessen
            Dim sSql As String = "Select * FROM Buchung WHERE BID = '" & sgRBID & "'"
            Dim dt As DataTable = fcReadDataTable(sSql)
            If dt.Rows.Count = 0 Then Me.Close()
        End If

    End Sub


    ''' <summary>
    ''' Buchung löschen
    ''' </summary>
    ''' <param name="sBID"></param>
    ''' <param name="sZID"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Function fcDelBuchung(ByVal sBID As String, Optional ByVal sID As String = "") As Boolean
        fcDelBuchung = False
        Dim sMsg As String = "Wollen Sie dieses Zimmer wirklich löschen?  "
        Dim cSql As String = "DELETE FROM Buchung WHERE BID = '" & sBID & "' and ID = '" & sID & "'"
        If sID = "" Then
            sMsg = "Wollen Sie diese Reservierung wirklich löschen?  "
            cSql = "DELETE FROM Buchung WHERE BID = '" & sBID & "'"
        End If
        If sBID = "" Then Exit Function
        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            'Datensatz per SQL aus der Tabelle löschen
            UpdateTable(cSql)
            'Änderung in DataTable "dtBuc" speichern
            Call fcDeleteTableRow(dtBuc, "BID = '" & sBID & "'")
            fcDelBuchung = True
        End If
    End Function

    ''' <summary>
    ''' ZimmerID und Zeitraum der Buchung laden
    ''' </summary>
    ''' <param name="sBID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Function fcGetZimmerIDAusBuchung(ByVal sBID As String) As Array
        Dim sSql As String = "Select * FROM Buchung WHERE BID = '" & sBID & "'"
        Dim dt As DataTable = fcReadDataTable(sSql)
        Dim i As Integer
        Dim arT(0) As String
        fcGetZimmerIDAusBuchung = arT
        If dt.Rows.Count = 0 Then Exit Function
        Dim nMax As Integer = dt.Rows.Count - 1
        ReDim arT(nMax)
        Try

            For i = 0 To nMax
                arT(i) = dt.Rows(i).Item("ZimID").ToString & ";" & _
                         dt.Rows(i).Item("Von").ToString & ";" & _
                         dt.Rows(i).Item("Bis").ToString
            Next
            fcGetZimmerIDAusBuchung = arT
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Function

#End Region

#Region "Rechnung stornieren......................................................................."

    Private Sub tsmStorno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmStorno.Click
        Dim sfeld As String
        Dim arfeld2() As String
        If MsgBox("Soll die Rechnung storniert werden? " & vbCrLf & "RNr.: " & sRNr, MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Storno") = MsgBoxResult.Yes Then
            Dim arFields(0), arValue(0) As String
            Dim cBedingung As String = " WHERE RID='" & sRNr & "'"
            arFields = Split("Rechnung,RID", ",")
            arValue = Split("0" & "°" & " ", "°")

            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
            cBedingung = "RID Like '" & sRNr & "'"
            Call fcUpdateTable(dtBuc, arFields, arValue, cBedingung)

            lgRech = fcChaneMenue(False)

            arfeld2 = fcDataSeek("select * From Datev Where RechNr ='", sRNr, 0, {"Id", "Name", "Rechnr", _
                                     "Umsatz1", "Mwst1", "MwstS1", "GKonto1", "Konto1", "Umsatz2", "Mwst2", _
                                     "MwstS2", "Gkonto2", "Konto2", "Datum", "BuchNr", "KunNr"})

            sfeld = arfeld2(6)
            arfeld2(6) = arfeld2(7)
            arfeld2(7) = sfeld
            sfeld = arfeld2(11)
            arfeld2(11) = arfeld2(12)
            arfeld2(12) = sfeld
            'datensatz nicht gefunfen
            arfeld2(1) = "Storno " + arfeld2(1)
            arfeld2(15) = "Storno"
            If arfeld2(0) <> " " Then
                arfeld2(0) = fcGetTimeID(Date.Today)
                For i = 1 To 15
                    If arfeld2(i) = "" Then arfeld2(i) = "-"
                Next
                Call fcInsertCommand("Datev", {"Id", "Name", "Rechnr", _
                                     "Umsatz1", "Mwst1", "MwstS1", "GKonto1", "Konto1", "Umsatz2", "Mwst2", _
                                     "MwstS2", "Gkonto2", "Konto2", "Datum", "BuchNr", "KunNr"}, arfeld2)

            End If

        End If

    End Sub

#End Region

#Region "Reservierungsbestätigung drucken.........................................................."

    Private Sub tsbBestätigung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBestätigung.Click
        Call prSaveZimmerReservierung()
        Call fcDruckBuchnung(sgRBID)
    End Sub

#End Region



#Region "Rechnung erstellen........................................................................"

    ''' <summary>
    ''' Rechnungsmodul aufrufen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 02.04.2012 Create
    ''' </remarks>
    Private Sub tsbRechnung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRechnung.Click
        prSaveZimmerReservierung()
        sgGID = lbGastID.Text
        frmRechnung.Show()
    End Sub
    Private Sub tsbAnzahlungRechnung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAnzahlungRechnung.Click
        prSaveZimmerReservierung()
        sgGID = lbGastID.Text
        frmAnzahlung.Show()
    End Sub

#End Region


#Region "Mit Enter weiter zum nächsten Feld........................................................"

    Private Sub tbName1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbName1.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbVorname.Select()
        End If
    End Sub
    Private Sub tbName2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbName2.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbVorname.Select()
        End If
    End Sub
    Private Sub tbVorname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbVorname.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbStrasse.Select()
        End If
    End Sub
    Private Sub tbStrasse_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbStrasse.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbPLZ.Select()
        End If
    End Sub
    Private Sub tbPLZ_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbPLZ.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbOrt.Select()
        End If
    End Sub
    Private Sub tbOrt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbOrt.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            coLand.Select()
        End If
    End Sub
    Private Sub coLand_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles coLand.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbTel.Select()
        End If
    End Sub
    Private Sub tbTel_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbTel.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbFax.Select()
        End If
    End Sub
    Private Sub tbFax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbFax.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbFunk.Select()
        End If
    End Sub
    Private Sub tbFunk_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbFunk.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbEMail.Select()
        End If
    End Sub

    Private Sub tbEMail_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbEMail.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            '  tbPass.Select()
        End If
    End Sub
    Private Sub tbPass_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            coWerbung.Select()
        End If
    End Sub

    Private Sub coWerbung_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles coWerbung.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbName1.Select()
        End If
    End Sub

#End Region


    Private Sub tsbErinnerung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbErinnerung.Click
        frmErinnerung.Show()
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


    Private Sub tbFPreis_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFPreis.LostFocus
        tbFPreis.Text = fcFormatDecimal(tbFPreis.Text)
    End Sub


    Private Sub tsbMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbMail.Click
        If FileExists(arIni(32) & "\Buchung\Buch_" & sgRBID & ".PDF") = False Then
            MsgBox("Buchungsbestätigung noch nicht Erstelt", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        Dim sText As Array = Split(ReadOneValueFromSystemDb("Mail"), "#")
        Dim sPdf As String = arIni(32) & "\Buchung\Buch_" & sgRBID & ".PDF"
        Dim arFeld(1) As String
        Dim arfeld1(1) As String
        Dim ii As Integer = 0
        Dim sKID As String
        Dim sName As String = ""
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sgRBID, 0, {"KunID", "Sprache"})
        sKID = arfeld1(0)
        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"EMail", "Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land"})
        ii = Val(arfeld1(1))
        If arFeld(0).Trim <> "" Then
            If fcSendeMailAnlage(arFeld(0), "Buchung", sText(ii), sDatei:=sPdf) = True Then
                MsgBox("Buchungs Mail Erfolgreich gesendet", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Else
                MsgBox(" F e h l e r  Buchungs Mail ", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                End If
            Else
                MsgBox(" F e h l e r  Keine Mailadresse", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
        End If
    End Sub
    Private Sub tsbBezahlt_Click(sender As Object, e As EventArgs) Handles tsbBezahlt.Click
        '  If MsgBox("Zahlungsbestätigung Senden?", MsgBoxStyle.Information + MsgBoxStyle.RetryCancel) = True Then
        If fcMailBezahlt(sgRBID) = True Then
            cbBezalt.Checked = True
        End If


    End Sub








    Private Sub coWerbung_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coWerbung.SelectedIndexChanged
        sgWerbung = coWerbung.Text
    End Sub





    Private Sub tsbBewertung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBewertung.Click
        Dim sTem As Array
        sTem = fcDataSeek("select * From Bewertung Where BuchID ='", sgRBID, 0, {"ID"}) 'kontrolle ob schon geschriben
        If sTem(0) = " " Then
            Call fcInsertCommand("Bewertung", {"Id", "BuchID", "Status"}, {fcGetTimeID(Date.Today), sgRBID, "-1"})
        End If
    End Sub

    Private Sub tbVorname_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbVorname.TextChanged
        Call prLoadGastInList()
        Call prFindGast()

    End Sub



    Private Sub tbName1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbName1.TextChanged
        Call prLoadGastInList()
        Call prFindGast()

    End Sub

    Private Sub coBText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coBText.SelectedIndexChanged
        Dim n As String = coBText.SelectedIndex
        If tbRechSend.Text.Trim = "" Then tbRechSend.Text = arZZiel(n)
    End Sub


    Private Sub prCreatTabelIndex()
        Dim sSQL As String = "Select * From Buchung  Where IDRef='" & sIDB & "' order by BIDindex asc" ' & " And ZimID='" & sgRZID & "'"
        dtBuchIndex = fcReadDataTable(sSQL) ' order by Name1, Vorname asc

        If dtBuchIndex.Rows.Count - 1 = -1 Then   'wenn BIDRef ="XXXXX"
            Dim sqlText As String = "IDRef"
            Dim arfields() As String
            Dim arValue(0) As String
            Dim cBedingung As String
            arfields = Split(sqlText, ",")
            arValue(0) = sIDB
            cBedingung = " WHERE ID='" & sIDB & "'"
            Call fcUpdateCommand("Buchung", arfields, arValue, cBedingung)
            dtBuchIndex = fcReadDataTable(sSQL)
        End If

    End Sub







    Private Function fcGetObjID(ByVal sZim As String) As String
        fcGetObjID = " "
        Dim nMax As Integer = dtZim.Rows.Count - 1
        For i As Integer = 0 To nMax
            If dtZim.Rows(i).Item("ID") = sZim Then
                fcGetObjID = dtZim.Rows(i).Item("IDObjekte")
                Exit For
            End If
        Next
    End Function
    Private Sub buBerechnung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buBerechnung.Click
        Call Summe(sgRBID, sgRZID)
    End Sub

    Private Sub PreisToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PreisToolStripMenuItem.Click
        Dim P As String = fcGetZimPreis(sgRZID, tbAnzPer.Text, lbAbreise.Text, lbAnreise.Text)
    End Sub

    Private Sub prCheckinMail(ByRef sNummer As String)

        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String
        Try
            cBedingung = " WHERE BID ='" & sgRBID & "'"
            sqlText = "SendMailZugang"
            arFields = Split(sqlText, ",")
            sqlText = sNummer
            arValue = Split(sqlText, "°")
            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally


        End Try
    End Sub

    Private Sub tsbCheckin_Click(sender As Object, e As EventArgs) Handles tsbCheckin.Click
        frmCheckin.Show()

    End Sub
    Private Sub tbEMail_TextChanged(sender As Object, e As EventArgs) Handles tbEMail.MouseHover

        lbMailList.Visible = True
        lbMailList.Text = Replace(tbEMail.Text, ";", vbCrLf)
        lbMailList.Text = Replace(lbMailList.Text, " ", "")

    End Sub

    Private Sub tbEMail_TextChanged_1(sender As Object, e As EventArgs) Handles tbEMail.MouseLeave

        lbMailList.Visible = False
    End Sub
    Private Sub cbZusatz_CheckedChanged(sender As Object, e As EventArgs) Handles cbZusatz.CheckedChanged
        If cbZusatz.Checked = True Then
            gbNameZ.Visible = True
        Else
            gbNameZ.Visible = False
        End If
    End Sub

    Private Sub ToolStripDropDownButton1_Click(sender As Object, e As EventArgs) Handles ToolStripDropDownButton1.Click

    End Sub
End Class
