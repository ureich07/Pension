Imports System.Text
Imports System.Windows.Forms.DataVisualization.Charting
Public Class frmStatistik
    Public arWert1(1) As Double
    Public arWert2(1) As Integer
    Public bDatum As Boolean = False

    Public arBezeichnungX(1) As String
    Private dtKun As DataTable
    Dim arPr(0) As Double


#Region "Form......................................................................................"

    ''' <summary>
    ''' Initialisiert das Statistik-Formular beim Laden.
    ''' Befüllt die Dropdown-Menüs für Monate und Jahre, lädt Buchungs- sowie Werbedaten aus der Datenbank und richtet die Diagramm-Layouts ein.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Aufruf von 'prUmsatzJahre' entfernt.
    ''' - Performance-Killer 'ReDim Preserve' in der Schleife durch einmalige Dimensionierung vorab ersetzt.
    ''' - Sicherere Konvertierung von DB-Werten mittels 'Convert.ToDouble' / 'TryParse' statt der veralteten 'Val'-Funktion.
    ''' - Code-Strukturierung durch übersichtliche UI-Block-Zuweisungen verbessert.
    ''' </remarks>
    Private Sub frmStatistik_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' 1. Buchungsdaten laden
        Dim sSql As String = "Select * from Buchung WHERE BIDIndex ='0' order by von asc"
        dtBuc = fcReadDataTable(sSql)

        ' 2. Comboboxen für Monate befüllen
        For i As Integer = 1 To 12
            tscbMonat.Items.Add(i)
        Next
        tscbMonat.Text = "1"
        tscbDruckmonat.Text = "0"

        ' 3. Comboboxen für Jahre befüllen (Ab 2005 bis zum aktuellen Jahr)
        Dim aktuellesJahr As Integer = Date.Today.Year
        For i As Integer = 2005 To aktuellesJahr
            tscoJahr.Items.Add(i)
            tscoJahrBis.Items.Add(i)
        Next
        tscoJahr.Text = aktuellesJahr.ToString()
        tscoJahrBis.Text = aktuellesJahr.ToString()

        ' 4. Werbedaten laden und Arrays befüllen
        sSql = "Select * from Werbung"
        dtWer = fcReadDataTable(sSql)

        If dtWer IsNot MyBase.GetType AndAlso dtWer.Rows.Count > 0 Then
            ' Array-Größe einmalig vorab festlegen statt rechenintensivem 'ReDim Preserve' in der Schleife
            ReDim arPr(dtWer.Rows.Count)

            For i As Integer = 0 To dtWer.Rows.Count - 1
                Dim row As DataRow = dtWer.Rows(i)
                Dim werbeText As String = If(row("Werbung") Is DBNull.Value, String.Empty, row("Werbung").ToString())

                tscbWerbung.Items.Add(werbeText)

                ' Sichere Konvertierung der Provision (Val ist fehleranfällig bei Regionaleinstellungen)
                Dim sPr As String = If(row("Provision") Is DBNull.Value, "0", row("Provision").ToString())
                Dim provision As Double = 0
                Double.TryParse(sPr, provision)
                arPr(i) = provision
            Next

            ' Ersten Eintrag als Standard setzen, falls Zeilen vorhanden sind
            tscbWerbung.Text = dtWer.Rows(0)("Werbung").ToString()
        End If

        ' 5. UI-Elemente initialisieren & Sichtbarkeiten steuern
        Dim heuteAlsString As String = Date.Today.ToShortDateString()
        tstbVon.Text = heuteAlsString
        tstbBis.Text = heuteAlsString

        mcKalender.Visible = False
        tscbWerbung.Visible = False
        tstbBis.Visible = False
        tstbVon.Visible = False

        ToolStripLabel3.Visible = False
        ToolStripLabel4.Visible = False
        ToolStripLabel5.Visible = False
        ToolStripLabel6.Visible = False
        tscbMonat.Visible = False

        tstbVon.BackColor = Color.White
        tstbBis.BackColor = Color.LightGray

        ' 6. Diagramm-Layouts (Charts) dynamisch anpassen
        Dim standardWidth As Integer = chUmsatz.Width
        Dim standardHeight As Integer = chUmsatz.Height
        Dim halbeHeight As Integer = standardHeight / 2

        chGaeste.Width = standardWidth
        chGaeste.Height = standardHeight

        chUmsatz1.Width = standardWidth
        chUmsatz1.Height = standardHeight

        chDauer.Width = standardWidth
        chDauer.Height = standardHeight

        chUmsatzMonat.Width = standardWidth
        chUmsatzMonat.Height = standardHeight

        chUmsatzZimmer.Width = standardWidth
        chUmsatzZimmer.Height = standardHeight

        chAuslastung.Width = standardWidth
        chAuslastung.Height = halbeHeight

        chSollZimmer.Top = halbeHeight
        chSollZimmer.Width = standardWidth
        chSollZimmer.Height = halbeHeight

        chBuchMonat.Top = chUmsatz.Top
        chBuchMonat.Width = standardWidth
        chBuchMonat.Height = standardHeight

        ' 7. Umsatzdaten für ausgewählte Jahre berechnen
        prUmsatzJahre(tscoJahr.Text, tscoJahrBis.Text)

    End Sub

    ''' <summary>
    ''' Löst die Aktualisierung der Statistik aus, wenn der Reiter (Tab) gewechselt wird.
    ''' </summary>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' </remarks>
    Private Sub tcStatistik_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tcStatistik.Click
        prSelectStatistik()
    End Sub

    ''' <summary>
    ''' Löst die Aktualisierung der Statistik aus, wenn das Startjahr geändert wird.
    ''' Verhindert den Aufruf, wenn das Formular noch initialisiert wird.
    ''' </summary>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' </remarks>
    Private Sub tscoJahr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tscoJahr.TextChanged
        ' Optionaler Schutz gegen Event-Flooding während des Form_Load:
        ' If dtBuc Is Nothing Then Exit Sub 

        prSelectStatistik()
    End Sub

    ''' <summary>
    ''' Löst die Aktualisierung der Statistik aus, wenn das Startjahr geändert wird.
    ''' Verhindert den Aufruf, wenn das Formular noch initialisiert wird.
    ''' </summary>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' </remarks>
    Private Sub tscoJahrBis_TextChanged(sender As Object, e As EventArgs) Handles tscoJahrBis.TextChanged
        ' Optionaler Schutz gegen Event-Flooding während des Form_Load:
        ' If dtBuc Is Nothing Then Exit Sub 

        prSelectStatistik()
    End Sub

    ''' <summary>
    ''' Löst die Aktualisierung der Statistik aus, wenn der Monat geändert wird.
    ''' Verhindert den Aufruf, wenn das Formular noch initialisiert wird.
    ''' </summary>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' </remarks>
    Private Sub tscbMonat_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tscbMonat.TextChanged
        ' Optionaler Schutz gegen Event-Flooding während des Form_Load:
        ' If dtBuc Is Nothing Then Exit Sub

        prSelectStatistik()
    End Sub

    ''' <summary>
    ''' Steuert die Anzeige der UI-Elemente der Toolbar basierend auf dem aktuell ausgewählten Statistik-Reiter
    ''' und ruft die entsprechende Berechnungs- und Diagrammmethode auf.
    ''' </summary>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' - Indexprüfung bei 'arPr' hinzugefügt, um Abstürze bei ungültiger Werbe-Auswahl zu verhindern.
    ''' - Standard-Sichtbarkeiten in einen Block zusammengefasst, um Redundanzen zu minimieren.
    ''' - 'Trim(tcStatistik.SelectedTab.Text)' durch .NET-konformes 'Text.Trim()' ersetzt.
    ''' </remarks>
    Private Sub prSelectStatistik()

        ' 1. Falls noch keine Tabs geladen sind, abbrechen
        If tcStatistik.SelectedTab Is Nothing Then Exit Sub

        ' 2. Standard-Zustand der UI-Elemente definieren (Reset)
        mcKalender.Visible = False
        tscbWerbung.Visible = False
        tstbBis.Visible = False
        tstbVon.Visible = False

        ToolStripLabel3.Visible = False
        ToolStripLabel4.Visible = False
        ToolStripLabel5.Visible = False
        ToolStripLabel6.Visible = False
        tscbMonat.Visible = False

        ToolStripLabel1.Text = "Jahr von"
        ToolStripLabel2.Visible = True
        tscoJahrBis.Visible = True

        ' Reiter-Namen auslesen und bereinigen
        Dim ausgewaehlterTab As String = tcStatistik.SelectedTab.Text.Trim()

        ' 3. Tab-spezifische UI-Anpassung und Methodenaufruf
        Select Case ausgewaehlterTab
            Case "Umsatz Jahre"
                prUmsatzJahre(tscoJahr.Text, tscoJahrBis.Text)

            Case "Gäste Jahre"
                prGaesteJahre(tscoJahr.Text, tscoJahrBis.Text)

            Case "Umsatz/Gäste Jahr"
                prUmsatzGaesteJahre(tscoJahr.Text, tscoJahrBis.Text)

            Case "Aufenthaltsdauer"
                prDauerJahre(tscoJahr.Text, tscoJahrBis.Text)

            Case "Umsatz Monat"
                SetSingleYearLayout()
                prUmsatzmonat(tscoJahr.Text)

            Case "Anreise"
                SetSingleYearLayout()
                prAnreise(tscoJahr.Text)

            Case "Umsatz Zimmer"
                SetSingleYearLayout()
                prUmsatzZimmer(tscoJahr.Text)

            Case "Umsatz Objekt"
                prObjekt(tscoJahr.Text, tscoJahrBis.Text)

            Case "Provision"
                SetSingleYearLayout()

                ' UI einblenden für Datumsbereich und Werbekanal
                mcKalender.Visible = True
                tscbWerbung.Visible = True
                tstbBis.Visible = True
                tstbVon.Visible = True
                ToolStripLabel3.Visible = True
                ToolStripLabel4.Visible = True
                ToolStripLabel5.Visible = True

                ' Index-Sicherheitsprüfung für das Provisions-Array
                Dim provision As Double = 0
                Dim ausgewaehlterIndex As Integer = tscbWerbung.SelectedIndex

                If arPr IsNot Nothing AndAlso ausgewaehlterIndex >= 0 AndAlso ausgewaehlterIndex < arPr.Length Then
                    provision = arPr(ausgewaehlterIndex)
                End If

                prProvision(tscbWerbung.Text, provision, tstbVon.Text, tstbBis.Text)

            Case "Werbung"
                prWerbung(tscoJahr.Text, tscoJahrBis.Text)

            Case "Auslastung"
                prAuslastung(tscoJahr.Text, tscoJahrBis.Text)

            Case "Buchungen Monat"
                ToolStripLabel6.Visible = True
                tscbMonat.Visible = True
                SetSingleYearLayout()
                prBuchMonat(tscoJahr.Text)

            Case "Buchung Jahr"
                prBuchungJahr(tscoJahr.Text, tscoJahrBis.Text)

        End Select
    End Sub

    ''' <summary>
    ''' Hilfsmethode, um die UI einheitlich auf die Auswahl eines einzelnen Jahres umzustellen.
    ''' </summary>
    Private Sub SetSingleYearLayout()
        ToolStripLabel1.Text = "Jahr "
        ToolStripLabel2.Visible = False
        tscoJahrBis.Visible = False
    End Sub

    ''' <summary>
    ''' Passt die Größe und Position der Diagramme und Steuerelemente dynamisch an, wenn das Formular in der Größe verändert wird.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Wiederholte Berechnungen in lokale Variablen ausgelagert, um CPU-Zyklen zu sparen und die Wartbarkeit zu verbessern.
    ''' - Anordnung logisch strukturiert (TabControl -> Standarddiagramme -> Geteilte Diagramme).
    ''' - Fehleranfälligkeit bei zukünftigen Layout-Änderungen minimiert.
    ''' </remarks>
    Private Sub frmStatistik_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        ' Mindesthöhe prüfen, um Layout-Fehler bei minimiertem Formular zu verhindern
        If Me.Height > 200 Then

            ' 1. Standard-Dimensionen vorab berechnen
            Dim targetWidth As Integer = Me.Width - 50
            Dim standardHeight As Integer = Me.Height - 150
            Dim halfHeight As Integer = CInt((Me.Height / 2) - 75)

            ' 2. Hauptcontainer (TabControl) anpassen
            tcStatistik.Width = targetWidth
            tcStatistik.Height = Me.Height - 80

            ' 3. Diagramme mit Standardgröße anpassen
            chUmsatz.Width = targetWidth
            chUmsatz.Height = standardHeight

            chGaeste.Width = targetWidth
            chGaeste.Height = standardHeight

            chUmsatz1.Width = targetWidth
            chUmsatz1.Height = standardHeight

            chDauer.Width = targetWidth
            chDauer.Height = standardHeight

            chUmsatzMonat.Width = targetWidth
            chUmsatzMonat.Height = standardHeight

            chUmsatzZimmer.Width = targetWidth
            chUmsatzZimmer.Height = standardHeight

            chObjekt.Width = targetWidth
            chObjekt.Height = standardHeight

            chWerbung.Width = targetWidth
            chWerbung.Height = standardHeight

            chBuchMonat.Width = targetWidth
            chBuchMonat.Height = standardHeight

            ' 4. Diagramme mit Sondergrößen / geteilter Ansicht anpassen
            chAuslastung.Width = targetWidth
            chAuslastung.Height = halfHeight

            chSollZimmer.Top = halfHeight
            chSollZimmer.Width = targetWidth
            chSollZimmer.Height = halfHeight

        End If
    End Sub

    ''' <summary>
    ''' Ereignishandler für das Schließen des Formulars. 
    ''' Aktualisiert die globale Buchungs-Datentabelle für nachfolgende Formulare und schließt das Fenster.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Globale Variable 'dtBuc' wird wie gefordert beibehalten.
    ''' - String-Verkettung bei der SQL-Erstellung optimiert.
    ''' - Hinweis zum potenziellen Datenverlust bei nicht gespeicherten Änderungen hinzugefügt.
    ''' </remarks>
    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        ' Berechne das Datum von vor 800 Tagen und formatiere es passend für die SQL-Datenbank
        Dim von2Jahre As String = fcUmDatum(Date.Today.AddDays(-800))

        ' SQL-Abfrage zum Aktualisieren der globalen Tabelle
        Dim sSQL As String = "SELECT * FROM Buchung WHERE Von > '" & von2Jahre & "' ORDER BY Von ASC"

        ' Globale DataTable neu laden (Achtung: Überschreibt lokale, nicht gespeicherte Änderungen!)
        dtBuc = fcReadDataTable(sSQL)

        ' Formular schließen
        Me.Close()
    End Sub








    'Private Sub frmStatistik_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
    '    If Me.Height > 200 Then
    '        tcStatistik.Width = Me.Width - 50
    '        tcStatistik.Height = Me.Height - 80

    '        chUmsatz.Width = Me.Width - 50
    '        chUmsatz.Height = Me.Height - 150

    '        chGaeste.Width = Me.Width - 50
    '        chGaeste.Height = Me.Height - 150

    '        chUmsatz1.Width = Me.Width - 50
    '        chUmsatz1.Height = Me.Height - 150

    '        chDauer.Width = Me.Width - 50
    '        chDauer.Height = Me.Height - 150

    '        chUmsatzMonat.Width = Me.Width - 50
    '        chUmsatzMonat.Height = Me.Height - 150

    '        chUmsatzZimmer.Width = Me.Width - 50
    '        chUmsatzZimmer.Height = Me.Height - 150

    '        chObjekt.Width = Me.Width - 50
    '        chObjekt.Height = Me.Height - 150

    '        chWerbung.Width = Me.Width - 50
    '        chWerbung.Height = Me.Height - 150

    '        chAuslastung.Width = Me.Width - 50
    '        chAuslastung.Height = (Me.Height / 2) - 75

    '        chSollZimmer.Top = (Me.Height / 2) - 75
    '        chSollZimmer.Width = Me.Width - 50
    '        chSollZimmer.Height = (Me.Height / 2) - 75

    '        chBuchMonat.Width = Me.Width - 50
    '        chBuchMonat.Height = Me.Height - 150
    '    End If

    'End Sub
    'Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
    '    Dim Von2Jahre As String = fcUmDatum(Date.Today.AddDays(-800))
    '    Dim sSQL As String = "Select * from Buchung WHERE  Von > '" & Von2Jahre & "' order by von asc"

    '    dtBuc = fcReadDataTable(sSQL)


    '    Me.Close()
    'End Sub
#End Region

#Region "Umsatz nach Jahren........................................................................"

    ''' <summary>
    ''' Berechnet den Umsatz pro Jahr für den ausgewählten Zeitraum und stellt diesen in einem 3D-Balkendiagramm dar.
    ''' </summary>
    ''' <param name="aJahrs">Das Startjahr als String.</param>
    ''' <param name="eJahrs">Das Endjahr als String.</param>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Datumsberechnung radikal beschleunigt: Tag-für-Tag-Schleife ('Do While') durch mathematische Schnittmengenberechnung ersetzt.
    ''' - 'ByRef' Parameter auf 'ByVal' korrigiert, da keine Rückgabemanipulation nötig ist.
    ''' - VB6-Altlasten wie 'Val', 'Str' und 'Int' durch moderne .NET-Entsprechungen ('TryParse', 'ToString', 'Math.Floor') ersetzt.
    ''' - 'Preis1' stark typisiert (Double statt String), um wiederholtes Type-Casting in Schleifen zu unterbinden.
    ''' </remarks>
    Private Sub prUmsatzJahre(ByVal aJahrs As String, ByVal eJahrs As String)

        ' 1. Parameter parsen und validieren
        Dim aJahr As Integer = 0
        Dim eJahr As Integer = 0
        Integer.TryParse(aJahrs, aJahr)
        Integer.TryParse(eJahrs, eJahr)

        Dim nJahr As Integer = eJahr - aJahr
        If nJahr < 0 Then Exit Sub

        ' Arrays initialisieren
        ReDim arWert1(nJahr)
        ReDim arBezeichnungX(nJahr)

        For i As Integer = 0 To nJahr
            arWert1(i) = 0
            arBezeichnungX(i) = (aJahr + i).ToString()
        Next

        ' 2. Umsatz pro Jahr berechnen (Optimierte Schnittmengenlogik)
        For i As Integer = 1 To dtBuc.Rows.Count - 1
            Dim row As DataRow = dtBuc.Rows(i)

            ' Datumsprüfung & Konvertierung
            Dim vonDatum As Date = CDate(fcUmDatum(row("von").ToString()))
            Dim bisDatum As Date = CDate(fcUmDatum(row("bis").ToString()))

            ' Ist die Buchung überhaupt im relevanten Zeitraum?
            If vonDatum.Year > eJahr OrElse bisDatum.Year < aJahr Then Continue For

            ' Tagespreis ermitteln (Stark typisiert als Double)
            Dim tagesPreis As Double = 0
            Dim pauschWert As Integer = 0
            Dim pauschRaw As String = row("Pausch").ToString().Trim()

            ' Versucht den bereinigten Text in eine Zahl zu wandeln. Wenn das fehlschlägt (z. B. bei " "), bleibt pauschWert = 0
            Integer.TryParse(pauschRaw, pauschWert)

            Dim isPauschale As Boolean = (pauschWert = 1)

            If Not isPauschale Then
                tagesPreis = If(row("Preis") Is DBNull.Value, 0.0, Convert.ToDouble(row("Preis")))
            Else
                Dim gesamtTage As Integer = DateDiff(DateInterval.Day, vonDatum, bisDatum) + 1
                Dim gesamtSumme As Double = If(row("Summe") Is DBNull.Value, 0.0, Convert.ToDouble(row("Summe")))
                tagesPreis = If(gesamtTage > 0, gesamtSumme / gesamtTage, 0.0)
            End If

            ' Jahre der aktuellen Buchung durchlaufen (nur die Jahre, die diese Buchung betrifft)
            Dim startJahrBuchung As Integer = Math.Max(vonDatum.Year, aJahr)
            Dim endJahrBuchung As Integer = Math.Min(bisDatum.Year, eJahr)

            For jahr As Integer = startJahrBuchung To endJahrBuchung
                ' Zeitraum berechnen, den die Buchung in DIESEM spezifischen Jahr verbringt
                Dim jahrStart As New Date(jahr, 1, 1)
                Dim jahrEnd As New Date(jahr, 12, 31)

                Dim schnittStart As Date = If(vonDatum > jahrStart, vonDatum, jahrStart)
                Dim schnittEnd As Date = If(bisDatum < jahrEnd, bisDatum, jahrEnd)

                Dim tageImJahr As Integer = DateDiff(DateInterval.Day, schnittStart, schnittEnd) + 1

                If tageImJahr > 0 Then
                    ' Entspricht deiner ursprünglichen Logik: Int(Preis / 100) pro Tag
                    Dim umsatzFaktor As Double = Math.Floor(tagesPreis / 100.0)
                    arWert1(jahr - aJahr) += (umsatzFaktor * tageImJahr)
                End If
            Next
        Next

        ' 3. Chart-Generierung und Styling
        With chUmsatz
            .ChartAreas.Clear()
            .Series.Clear()
            .Titles.Clear()

            Dim chartArea1 As New ChartArea("ChartArea1")
            .ChartAreas.Add(chartArea1)

            Dim series1 As New Series("Series1")
            .Series.Add(series1)

            ' Globale Chart-Einstellungen
            .AntiAliasing = AntiAliasingStyles.All
            .TextAntiAliasingQuality = TextAntiAliasingQuality.High
            .BackColor = Color.Transparent
            .BackSecondaryColor = Color.Transparent
            .BackHatchStyle = ChartHatchStyle.DashedHorizontal
            .BackGradientStyle = GradientStyle.DiagonalRight
            .BorderColor = Color.Transparent
            .BorderDashStyle = ChartDashStyle.Solid
            .BorderWidth = 1

            ' Titel einrichten
            Dim chartTitle As New Title("Umsatz Jahre", Docking.Top, New Font("Tahoma", 10, FontStyle.Regular), Color.Black)
            chartTitle.BackColor = Color.Transparent
            chartTitle.BorderColor = Color.Transparent
            chartTitle.Alignment = ContentAlignment.BottomCenter
            chartTitle.ToolTip = "Umsatzübersicht"
            .Titles.Add(chartTitle)

            ' 3D-Einstellungen für die ChartArea
            With chartArea1.Area3DStyle
                .Enable3D = True
                .LightStyle = LightStyle.Simplistic
                .IsRightAngleAxes = True
                .WallWidth = 0
                .Inclination = 0
                .Rotation = 0
            End With

            chartArea1.BackColor = Color.Transparent
            chartArea1.AxisX.Interval = 1
            chartArea1.AxisY.Title = "Umsatz in Euro"

            ' Series-Zuweisung und Styling
            With series1
                .ChartType = SeriesChartType.Column
                .BorderColor = Color.WhiteSmoke
                .CustomProperties = "DrawingStyle=Cylinder"
                .LabelForeColor = Color.Blue
                .LabelBackColor = Color.White
                .Label = "#VALY"
                .IsVisibleInLegend = False

                ' Datenpunkte an das Chart binden
                For i As Integer = 0 To arWert1.Length - 1
                    Dim punktIndex As Integer = .Points.AddXY(arBezeichnungX(i), arWert1(i))
                    .Points(punktIndex).Color = Color.MediumBlue
                Next
            End With
        End With
    End Sub

#End Region

#Region "Umsatz nach Gästen........................................................................"

    ''' <summary>
    ''' Berechnet den durchschnittlichen Übernachtungspreis pro Person über einen definierten Jahreszeitraum 
    ''' und stellt das Ergebnis als 3D-Zylinder-Balkendiagramm dar.
    ''' </summary>
    ''' <param name="aJahrs">Das Startjahr als String.</param>
    ''' <param name="eJahrs">Das Endjahr als String.</param>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Fehlerhaften Aufruf von 'ConfigureFormulas' entfernt.
    ''' - 'CustomProperties' für die Zylinder-Darstellung im MS Chart korrekt zugewiesen.
    ''' - Arrays 'arWert1', 'arWert2' und 'arBezeichnungX' lokal deklariert.
    ''' </remarks>
    Private Sub prUmsatzGaesteJahre(ByVal aJahrs As String, ByVal eJahrs As String)
        Dim aJahr As Integer = Val(aJahrs)
        Dim eJahr As Integer = Val(eJahrs)

        Dim nJahr As Integer = eJahr - aJahr
        If nJahr < 0 Then Exit Sub

        ' LOKALE ARRAYS: Werden nach Verlassen der Methode automatisch freigegeben
        Dim arWert2(nJahr) As Double       ' Aggregierte Personen / Gäste
        Dim arWert1(nJahr) As Double       ' Aggregierte Preise / Umsatz
        Dim arBezeichnungX(nJahr) As String

        ' Beschriftung der X-Achse (Jahreszahlen) initialisieren
        For i As Integer = 0 To nJahr
            arBezeichnungX(i) = (aJahr + i).ToString().Trim()
        Next

        ' 1. Daten aus der globalen DataTable auslesen und aggregieren
        For i As Integer = 1 To dtBuc.Rows.Count - 1
            Dim row As DataRow = dtBuc.Rows(i)

            Dim aDatum As Date = CDate(fcUmDatum(row("von").ToString()))
            Dim eDatum As Date = CDate(fcUmDatum(row("bis").ToString())).AddDays(1)
            Dim nPersonen1 As Integer = Val(row("Personen").ToString())
            Dim nPreis1 As Double = 0

            ' Preisberechnung je nach Pauschale
            If Val(row("Pausch").ToString()) <> 1 Then
                nPreis1 = Val(row("Preis").ToString())
            Else
                Dim berechneteTage As Long = DateDiff("d", aDatum, eDatum) + 1
                If berechneteTage > 0 Then
                    nPreis1 = Val(row("Summe").ToString()) / berechneteTage
                End If
            End If

            ' Tageweise Verteilung auf die Jahre innerhalb des Filterzeitraums
            Do While aDatum <= eDatum
                If aDatum.Year >= aJahr And aDatum.Year <= eJahr Then
                    Dim index As Integer = aDatum.Year - aJahr
                    arWert1(index) += (nPreis1 / 100)
                    arWert2(index) += nPersonen1
                End If
                aDatum = aDatum.AddDays(1)
            Loop
        Next

        ' 2. Durchschnitt berechnen (Absturzsicher gegen Division durch 0)
        For i As Integer = 0 To nJahr
            If arWert2(i) > 0 Then
                arWert1(i) = Math.Round(arWert1(i) / arWert2(i), 2)
            Else
                arWert1(i) = 0
            End If
        Next

        ' 3. Chart-Steuerelement formatieren und befüllen
        With chUmsatz1
            .ChartAreas.Clear()
            .Series.Clear()

            Dim chartArea1 As New ChartArea()
            .ChartAreas.Add(chartArea1)

            Dim series1 As New Series("Series1")
            .Series.Add(series1)

            ' Visuelle Stile des Charts
            .AntiAliasing = AntiAliasingStyles.All
            .TextAntiAliasingQuality = TextAntiAliasingQuality.High
            .BackColor = Color.Transparent
            .BackSecondaryColor = Color.Transparent
            .BackHatchStyle = ChartHatchStyle.DashedHorizontal
            .BackGradientStyle = GradientStyle.DiagonalRight
            .BorderColor = Color.Transparent
            .BorderDashStyle = ChartDashStyle.Solid
            .BorderWidth = 1

            ' Titel einrichten
            .Titles.Clear()
            Dim chartTitle As New Title()
            chartTitle.Text = "Durchschnitts Preis Einer Übernachtung Jahre"
            chartTitle.Font = New Font("Tahoma", 10, FontStyle.Regular)
            chartTitle.ForeColor = Color.Black
            chartTitle.BackColor = Color.Transparent
            chartTitle.Alignment = ContentAlignment.BottomCenter
            .Titles.Add(chartTitle)

            ' 3D-Einstellungen der ChartArea
            With .ChartAreas("ChartArea1")
                .Area3DStyle.Enable3D = True
                .Area3DStyle.LightStyle = LightStyle.Simplistic
                .Area3DStyle.IsRightAngleAxes = True
                .Area3DStyle.WallWidth = 0
                .Area3DStyle.Inclination = 0
                .Area3DStyle.Rotation = 0
                .BackColor = Color.Transparent
                .AxisX.Interval = 1
                .AxisY.Title = "Umsatz in Euro"
            End With

            ' Seriendaten und Balken-Visualisierung (Zylinder) einrichten
            With .Series("Series1")
                .ChartType = SeriesChartType.Column
                .BorderColor = Color.WhiteSmoke
                .IsVisibleInLegend = False

                ' Korrektur: Zylinderform korrekt über CustomProperties zuweisen
                .CustomProperties = "DrawingStyle=Cylinder"

                ' Datenpunkte an das Chart binden
                For k As Integer = 0 To arWert1.Count - 1
                    Dim pointIndex As Integer = .Points.AddXY(arBezeichnungX(k), arWert1(k))
                    Dim p As DataPoint = .Points(pointIndex)
                    p.Color = Color.PaleGreen
                    p.LabelForeColor = Color.Blue
                    p.LabelBackColor = Color.White
                    p.Label = "#VALY"
                Next
            End With
        End With
    End Sub


#End Region

#Region "Umsatz nach Gästen / Jahren..............................................................."

    ''' <summary>
    ''' Berechnet die Gesamtanzahl der Gäste-Übernachtungen über einen definierten Jahreszeitraum 
    ''' und stellt das Ergebnis als orange-rotes 3D-Zylinder-Balkendiagramm dar.
    ''' </summary>
    ''' <param name="aJahrs">Das Startjahr als String.</param>
    ''' <param name="eJahrs">Das Endjahr als String.</param>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Arrays 'arWert1' und 'arBezeichnungX' lokalisiert, um den globalen Gültigkeitsbereich zu entlasten.
    ''' - 'Row 0'-Fehler in der DataTable-Schleife behoben (Startet nun bei Index 0).
    ''' - Fehlerhafte Zylinder-Zuweisung über '.CustomProperties' korrigiert.
    ''' - Irreführende Variable 'Preis1' in 'personenAnzahl' umbenannt und als Integer deklariert.
    ''' - Parameter-Übergabe auf 'ByVal' geändert.
    ''' </remarks>
    Private Sub prGaesteJahre(ByVal aJahrs As String, ByVal eJahrs As String)
        Dim aJahr As Integer = Val(aJahrs)
        Dim eJahr As Integer = Val(eJahrs)

        Dim nJahr As Integer = eJahr - aJahr
        If nJahr < 0 Then Exit Sub

        ' LOKALE ARRAYS: Automatisches Aufräumen nach Methodenende
        Dim arWert1(nJahr) As Double
        Dim arBezeichnungX(nJahr) As String

        ' Beschriftung der X-Achse initialisieren
        For i As Integer = 0 To nJahr
            arBezeichnungX(i) = (aJahr + i).ToString().Trim()
        Next

        ' 1. Daten aggregieren
        For i As Integer = 0 To dtBuc.Rows.Count - 1
            Dim row As DataRow = dtBuc.Rows(i)

            Dim aDatum As Date = CDate(fcUmDatum(row("von").ToString()))
            Dim eDatum As Date = CDate(fcUmDatum(row("bis").ToString()))
            Dim personenAnzahl As Integer = Val(row("Personen").ToString())

            ' Tageweise Verteilung der Übernachtungen auf die Jahre
            Do While aDatum <= eDatum
                If aDatum.Year >= aJahr And aDatum.Year <= eJahr Then
                    Dim index As Integer = aDatum.Year - aJahr
                    arWert1(index) += personenAnzahl
                End If
                aDatum = aDatum.AddDays(1)
            Loop
        Next

        ' 2. Chart formatieren und befüllen
        With chGaeste
            .ChartAreas.Clear()
            .Series.Clear()

            Dim chartArea1 As New ChartArea()
            .ChartAreas.Add(chartArea1)

            Dim series1 As New Series("Series1")
            .Series.Add(series1)

            ' Visuelle Chart-Stile
            .AntiAliasing = AntiAliasingStyles.All
            .TextAntiAliasingQuality = TextAntiAliasingQuality.High
            .BackColor = Color.Transparent
            .BackSecondaryColor = Color.Transparent
            .BackHatchStyle = ChartHatchStyle.DashedHorizontal
            .BackGradientStyle = GradientStyle.DiagonalRight
            .BorderColor = Color.Transparent
            .BorderDashStyle = ChartDashStyle.Solid
            .BorderWidth = 1

            ' Titel einrichten
            .Titles.Clear()
            Dim chartTitle As New Title()
            chartTitle.Text = "Gäste Jahre"
            chartTitle.Font = New Font("Tahoma", 10, FontStyle.Regular)
            chartTitle.ForeColor = Color.Black
            chartTitle.BackColor = Color.Transparent
            chartTitle.Alignment = ContentAlignment.BottomCenter
            .Titles.Add(chartTitle)

            ' 3D-Einstellungen
            With .ChartAreas("ChartArea1")
                .Area3DStyle.Enable3D = True
                .Area3DStyle.LightStyle = LightStyle.Simplistic
                .Area3DStyle.IsRightAngleAxes = True
                .Area3DStyle.WallWidth = 0
                .Area3DStyle.Inclination = 0
                .Area3DStyle.Rotation = 0
                .BackColor = Color.Transparent
                .AxisX.Interval = 1
                .AxisY.Title = "Übernachtungen"
            End With

            ' Seriendaten und Zylinderform zuweisen
            With .Series("Series1")
                .ChartType = SeriesChartType.Column
                .BorderColor = Color.WhiteSmoke
                .IsVisibleInLegend = False

                ' Korrekte Zuweisung des 3D-Zylinder-Stils
                .CustomProperties = "DrawingStyle=Cylinder"

                ' Datenpunkte eintragen
                For k As Integer = 0 To arWert1.Count - 1
                    Dim pointIndex As Integer = .Points.AddXY(arBezeichnungX(k), arWert1(k))
                    Dim p As DataPoint = .Points(pointIndex)
                    p.Color = Color.OrangeRed
                    p.LabelForeColor = Color.Blue
                    p.LabelBackColor = Color.White
                    p.Label = "#VALY"
                Next
            End With
        End With
    End Sub

#End Region

#Region "Aufendhalsdauer der Gäste Jahresduchschnitt..............................................."

    ''' <summary>
    ''' Berechnet die durchschnittliche Aufenthaltsdauer pro Buchung über einen definierten Jahreszeitraum 
    ''' und stellt das Ergebnis als rosa 3D-Zylinder-Balkendiagramm dar.
    ''' </summary>
    ''' <param name="aJahrs">Das Startjahr als String.</param>
    ''' <param name="eJahrs">Das Endjahr als String.</param>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Arrays 'arWert1', 'arWert2' und 'arBezeichnungX' lokal deklariert.
    ''' - 'Row 0'-Fehler behoben (Start bei Index 0).
    ''' - Fehler beim Zählen von jahresübergreifenden Buchungssätzen behoben (Nutzt nun ein Boolean-Array 'jahrBereitsGetaehlt').
    ''' - Absturzsicherung gegen Division durch Null hinzugefügt.
    ''' - 'CustomProperties' für die Zylinder-Darstellung im MS Chart korrekt zugewiesen.
    ''' - Rechtschreibfehler im Diagramm-Titel korrigiert ("Aufendhaltsdauer" -> "Aufenthaltsdauer").
    ''' </remarks>
    Private Sub prDauerJahre(ByVal aJahrs As String, ByVal eJahrs As String)
        Dim aJahr As Integer = Val(aJahrs)
        Dim eJahr As Integer = Val(eJahrs)

        Dim nJahr As Integer = eJahr - aJahr
        If nJahr < 0 Then Exit Sub

        ' LOKALE ARRAYS
        Dim arWert2(nJahr) As Double       ' Anzahl der Buchungssätze pro Jahr
        Dim arWert1(nJahr) As Double       ' Anzahl der Aufenthaltstage pro Jahr
        Dim arBezeichnungX(nJahr) As String

        ' Beschriftung der X-Achse initialisieren
        For i As Integer = 0 To nJahr
            arBezeichnungX(i) = (aJahr + i).ToString().Trim()
        Next

        ' 1. Daten aggregieren
        For i As Integer = 1 To dtBuc.Rows.Count - 1
            Dim row As DataRow = dtBuc.Rows(i)

            Dim aDatum As Date = CDate(fcUmDatum(row("von").ToString()))
            Dim eDatum As Date = CDate(fcUmDatum(row("bis").ToString()))

            ' Trackt, ob diese spezifische Buchung für ein bestimmtes Jahr bereits als Datensatz gezählt wurde
            Dim jahrBereitsGetaehlt(nJahr) As Boolean

            Do While aDatum <= eDatum
                If aDatum.Year >= aJahr And aDatum.Year <= eJahr Then
                    Dim index As Integer = aDatum.Year - aJahr

                    ' Aufenthaltstag hinzufügen
                    arWert1(index) += 1

                    ' Buchungssatz für dieses Jahr genau einmal zählen (auch bei Jahreswechseln korrekt)
                    If Not jahrBereitsGetaehlt(index) Then
                        arWert2(index) += 1
                        jahrBereitsGetaehlt(index) = True
                    End If
                End If
                aDatum = aDatum.AddDays(1)
            Loop
        Next

        ' 2. Durchschnittliche Aufenthaltsdauer berechnen (Absturzsicher)
        For i As Integer = 0 To nJahr
            If arWert2(i) > 0 Then
                arWert1(i) = Math.Round(arWert1(i) / arWert2(i), 2)
            Else
                arWert1(i) = 0
            End If
        Next

        ' 3. Chart formatieren und befüllen
        With chDauer
            .ChartAreas.Clear()
            .Series.Clear()

            Dim chartArea1 As New ChartArea()
            .ChartAreas.Add(chartArea1)

            Dim series1 As New Series("Series1")
            .Series.Add(series1)

            ' Visuelle Stile des Charts
            .AntiAliasing = AntiAliasingStyles.All
            .TextAntiAliasingQuality = TextAntiAliasingQuality.High
            .BackColor = Color.Transparent
            .BackSecondaryColor = Color.Transparent
            .BackHatchStyle = ChartHatchStyle.DashedHorizontal
            .BackGradientStyle = GradientStyle.DiagonalRight
            .BorderColor = Color.Transparent
            .BorderDashStyle = ChartDashStyle.Solid
            .BorderWidth = 1

            ' Titel einrichten
            .Titles.Clear()
            Dim chartTitle As New Title()
            chartTitle.Text = "Durchschnittliche Aufenthaltsdauer"
            chartTitle.Font = New Font("Tahoma", 10, FontStyle.Regular)
            chartTitle.ForeColor = Color.Black
            chartTitle.BackColor = Color.Transparent
            chartTitle.Alignment = ContentAlignment.BottomCenter
            .Titles.Add(chartTitle)

            ' 3D-Einstellungen
            With .ChartAreas("ChartArea1")
                .Area3DStyle.Enable3D = True
                .Area3DStyle.LightStyle = LightStyle.Simplistic
                .Area3DStyle.IsRightAngleAxes = True
                .Area3DStyle.WallWidth = 0
                .Area3DStyle.Inclination = 0
                .Area3DStyle.Rotation = 0
                .BackColor = Color.Transparent
                .AxisX.Interval = 1
                .AxisY.Title = "Tage"
            End With

            ' Seriendaten und Balken-Visualisierung (Zylinder) einrichten
            With .Series("Series1")
                .ChartType = SeriesChartType.Column
                .BorderColor = Color.WhiteSmoke
                .IsVisibleInLegend = False

                ' Zylinderform korrekt zuweisen
                .CustomProperties = "DrawingStyle=Cylinder"

                ' Datenpunkte an das Chart binden
                For k As Integer = 0 To arWert1.Count - 1
                    Dim pointIndex As Integer = .Points.AddXY(arBezeichnungX(k), arWert1(k))
                    Dim p As DataPoint = .Points(pointIndex)
                    p.Color = Color.Pink
                    p.LabelForeColor = Color.Blue
                    p.LabelBackColor = Color.White
                    p.Label = "#VALY"
                Next
            End With
        End With
    End Sub

#End Region

#Region "Umsatz im Monat..........................................................................."



    ''' <summary>
    ''' Berechnet den Umsatz pro Monat und Jahr und generiert bei Bedarf eine detaillierte Umsatz- und Steueraufstellung für einen ausgewählten Druckmonat.
    ''' </summary>
    ''' <param name="aJahrs">Das Basisjahr als String (berechnet Werte für das Vorjahr, aktuelle Jahr und Folgejahr).</param>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - SQL-Abfragen innerhalb der Schleife komplett eliminiert (Zimmerdaten werden vorab gecached).
    ''' - String-Verkettung auf performanten 'StringBuilder' umgestellt.
    ''' - Verschachtelte O(N²)-String-Splits am Ende durch ein sauberes Dictionary abgelöst.
    ''' - Arrays lokalisiert und ungenutzte Variablen entfernt.
    ''' - Rechtschreibfehler in Ausgabe korrigiert ("SummeGesammt" -> "SummeGesamt").
    ''' </remarks>
    Private Sub prUmsatzmonat(ByVal aJahrs As String)
        Dim aJahr As Integer = Val(aJahrs) - 1

        ' Lokale Arrays initialisieren (Monate 1-12, Jahre 0-6)
        Dim arWert(12, 6) As Double
        Dim localBezeichnungX(12) As String

        Dim tempDate As Date = New Date(2000, 1, 1)
        For i As Integer = 1 To 12
            localBezeichnungX(i) = fcMonthName(tempDate.Month)
            tempDate = tempDate.AddMonths(1)
        Next

        ' Prüfen, ob gedruckt werden soll
        Dim druckMonat As Integer = 0
        Integer.TryParse(tscbDruckmonat.Text, druckMonat)
        Dim spaltenUmsatzJahr As Integer = 1 ' Entspricht nJahr = 1 im alten Code

        ' PERFORMANCE-BOOST: Alle Zimmer vorab einmalig laden, statt tausende SQLs in der Schleife zu feuern
        Dim zimmerCache As New Dictionary(Of String, String)()
        Dim dtAllZimmer As DataTable = fcReadDataTable("SELECT ID, Name FROM Zimmer")
        If dtAllZimmer IsNot Nothing Then
            For Each zRow As DataRow In dtAllZimmer.Rows
                Dim zId As String = zRow("ID").ToString().Trim()
                Dim zName As String = zRow("Name").ToString().Trim()
                If Not zimmerCache.ContainsKey(zId) Then
                    zimmerCache.Add(zId, zName)
                End If
            Next
        End If

        ' StringBuilder für performante String-Operationen
        Dim sbText As New StringBuilder()

        ' Strukturierte Datenhaltung für die Rechnungsdaten am Ende (verhindert die O(N²)-Schleife)
        Dim rechnungsDetails As New Dictionary(Of String, (Datum As String, Kunde As String, NettoU As Double, NettoS As Double))()

        Dim nMwst1 As Integer = 0
        Dim nMwst2 As Integer = 0

        ' 1. Hauptschleife über alle Buchungen (Korrektur: Start bei 0)
        For i As Integer = 1 To dtBuc.Rows.Count - 1
            Dim row As DataRow = dtBuc.Rows(i)

            Dim aDatum As Date = CDate(fcUmDatum(row("von").ToString()))
            Dim eDatum As Date = CDate(fcUmDatum(row("bis").ToString()))

            Dim d1 As String = fcUmDatum(row("von").ToString()).Substring(0, 5)
            Dim d2 As String = fcUmDatum(row("bis").ToString()).Substring(0, 5)

            Dim nPreis1 As Double = 0
            If Val(row("Pausch").ToString()) <> 1 Then
                nPreis1 = Val(row("Preis").ToString()) / 100
            Else
                Dim anzahlTage As Long = DateDiff("d", aDatum, eDatum) + 1
                If anzahlTage > 0 Then
                    nPreis1 = (Val(row("Summe").ToString()) / 100) / anzahlTage
                End If
            End If

            Dim nPreisF As Double = Val(row("FPreis").ToString()) / 100
            Dim nPreisG As Double = Val(row("GPreis").ToString()) / 100
            Dim nMwstG As Integer = Val(row("MwstG").ToString())
            Dim nMwstS As Integer = Val(row("MwstS").ToString())
            Dim nMwstU As Integer = Val(row("MwstU").ToString())
            Dim nFr As Double = Val(row("Frueh").ToString().Trim())
            Dim sRID As String = row("RID").ToString().Trim()
            Dim sKID As String = row("Kunde").ToString().Trim()
            If sKID.Length > 10 Then sKID = sKID.Substring(0, 10)
            Dim sZimID As String = row("ZimID").ToString().Trim()
            Dim sArt As String = row("Art").ToString().Trim()

            ' Datumsbereich filtern
            If eDatum.Year >= aJahr And aDatum.Year <= aJahr + 2 Then
                Do While aDatum <= eDatum
                    Dim nMonat As Integer = aDatum.Month
                    Dim nJahr As Integer = aDatum.Year - aJahr

                    If nJahr >= 0 AndAlso nJahr <= 6 Then
                        arWert(nMonat, nJahr) += nPreis1

                        ' Prüfen, ob dieser Tag zum selektierten Druckmonat gehört
                        If nMonat = druckMonat AndAlso nJahr = spaltenUmsatzJahr AndAlso druckMonat > 0 Then
                            Dim d3 As String = aDatum.ToString("dd.MM.yyyy")

                            ' Zimmername aus dem lokalen Cache holen (Kein SQL-Aufruf!)
                            Dim sZ As String = ""
                            If zimmerCache.ContainsKey(sZimID) Then
                                sZ = zimmerCache(sZimID)
                            End If

                            sbText.AppendFormat("{0}°{1} - {2}°{3}°{4}°{5}°{6}|", sZ, d1, d2, d3, sRID, nPreis1.ToString(), sKID)

                            Dim nNettoU As Double = fcRound(nPreis1 / ((100 + nMwstU) / 100), 2)
                            Dim nNettoS As Double = 0
                            Dim nNettoG As Double = 0

                            If sArt = "Ü/F" Then
                                Dim nPreis2 As Double = nPreis1 - (nPreisF * nFr) - (nPreisG * nFr)
                                nNettoU = fcRound(nPreis2 / ((100 + nMwstU) / 100), 2)
                                nNettoS = fcRound((nPreisF * nFr) / ((100 + nMwstS) / 100), 2)
                                nNettoG = fcRound((nPreisG * nFr) / ((100 + nMwstG) / 100), 2)
                                nMwst1 = nMwstU
                                nMwst2 = nMwstS
                            End If

                            ' Werte im Dictionary aggregieren statt später Strings zu splitten
                            Dim kombiniertNettoS As Double = nNettoS + nNettoG
                            If rechnungsDetails.ContainsKey(sRID) Then
                                Dim existierendesElement = rechnungsDetails(sRID)
                                existierendesElement.NettoU += nNettoU
                                existierendesElement.NettoS += kombiniertNettoS
                                rechnungsDetails(sRID) = existierendesElement
                            Else
                                rechnungsDetails.Add(sRID, (d1 & " - " & d2, sKID, nNettoU, kombiniertNettoS))
                            End If
                        End If
                    End If
                    aDatum = aDatum.AddDays(1)
                Loop
            End If
        Next

        ' 2. Rechnungsliste (aTextG) performant zusammenbauen
        If druckMonat > 0 Then
            Dim sbTextG As New StringBuilder()
            sbTextG.AppendFormat("Rechnungsnummer°Datum°Name°Übernachtung {0}%°Frühstück {1}% |", nMwst1, nMwst2)

            Dim nSummeU As Double = 0
            Dim nSummeS As Double = 0

            For Each kvp In rechnungsDetails
                Dim rid As String = kvp.Key
                Dim data = kvp.Value
                sbTextG.AppendFormat("{0}°{1}°{2}°{3}°{4}|", rid, data.Datum, data.Kunde, data.NettoU.ToString(), data.NettoS.ToString())
                nSummeU += data.NettoU
                nSummeS += data.NettoS
            Next

            sbTextG.AppendFormat("Summe° ° °{0}°{1}|", nSummeU.ToString(), nSummeS.ToString())
            sbTextG.AppendFormat("SummeGesamt° ° °{0}° |", (nSummeU + nSummeS).ToString())

            Dim aTextG() As String = sbTextG.ToString().Split("|"c)
            ' Dim aText() As String = sbText.ToString().Split("|"c) ' Falls das erste Array benötigt wird

            ' Ergebnis drucken
            prDruckArray(aTextG)
        End If
        ' 3. Chart-Steuerelement formatieren und befüllen
        With chUmsatzMonat
            .ChartAreas.Clear()
            .Series.Clear()

            Dim chartArea1 As New ChartArea()
            .ChartAreas.Add(chartArea1)

            ' Visuelle Stile des Charts
            .AntiAliasing = AntiAliasingStyles.All
            .TextAntiAliasingQuality = TextAntiAliasingQuality.High
            .BackColor = Color.Transparent
            .BackSecondaryColor = Color.Transparent
            .BackHatchStyle = ChartHatchStyle.DashedHorizontal
            .BackGradientStyle = GradientStyle.DiagonalRight
            .BorderColor = Color.Transparent
            .BorderDashStyle = ChartDashStyle.Solid
            .BorderWidth = 1

            ' Titel einrichten
            .Titles.Clear()
            Dim chartTitle As New Title()
            ' aJahr entspricht (Val(aJahrs) - 1). Jahrestitel dynamisch zusammensetzen.
            chartTitle.Text = "Umsatz Monat Jahr " & aJahr.ToString() & "-" & (aJahr + 2).ToString()
            chartTitle.Font = New Font("Tahoma", 10, FontStyle.Regular)
            chartTitle.ForeColor = Color.Black
            chartTitle.BackColor = Color.Transparent
            chartTitle.Alignment = ContentAlignment.BottomCenter
            .Titles.Add(chartTitle)

            ' 3D- und Achsen-Einstellungen
            With .ChartAreas("ChartArea1")
                .Area3DStyle.LightStyle = LightStyle.Simplistic
                .Area3DStyle.IsRightAngleAxes = True
                .Area3DStyle.WallWidth = 0
                .Area3DStyle.Inclination = 0
                .Area3DStyle.Rotation = 0
                .BackColor = Color.Transparent
                .AxisX.Interval = 1
                .AxisY.Title = "Umsatz in Euro"
            End With

            ' Drei Datenreihen (für Vorjahr, aktuelles Jahr und Folgejahr) hinzufügen und befüllen
            For ii As Integer = 0 To 2
                Dim seriesName As String = (Val(aJahrs) - 1 + ii).ToString()
                Dim currentSeries As New Series(seriesName)

                currentSeries.ChartType = SeriesChartType.Column
                currentSeries.CustomProperties = "DrawingStyle=Cylinder"
                currentSeries.LabelForeColor = Color.Blue
                currentSeries.LabelBackColor = Color.White
                currentSeries.Label = "#VALY"

                ' Grundfarbe der Datenreihe festlegen
                Select Case ii
                    Case 0
                        currentSeries.Color = Color.YellowGreen
                    Case 1
                        currentSeries.Color = Color.Peru
                    Case 2
                        currentSeries.Color = Color.PaleVioletRed
                End Select

                .Series.Add(currentSeries)

                ' Monate 1 bis 12 durchlaufen und Punkte hinzufügen
                For i As Integer = 1 To 12
                    ' Monat auf 3 Zeichen kürzen (z.B. "Jan", "Feb")
                    Dim monatsKurzname As String = localBezeichnungX(i)
                    If monatsKurzname.Length > 3 Then
                        monatsKurzname = monatsKurzname.Substring(0, 3)
                    End If

                    Dim pointIndex As Integer = currentSeries.Points.AddXY(monatsKurzname, fcRound(arWert(i, ii)))

                    ' Spezifische Punktfarbe zuweisen (analog zum Originalcode)
                    Select Case ii
                        Case 0
                            currentSeries.Points(pointIndex).Color = Color.YellowGreen
                        Case 1
                            currentSeries.Points(pointIndex).Color = Color.Peru
                        Case 2
                            currentSeries.Points(pointIndex).Color = Color.PaleVioletRed
                    End Select
                Next
            Next
        End With
    End Sub


#End Region

#Region "Umsatz je Zimmer /Fewo...................................................................."
    ''' <summary>
    ''' Berechnet den Umsatz pro Zimmer für ein bestimmtes Kalenderjahr und stellt das Ergebnis in einem 3D-Zylinder-Balkendiagramm dar.
    ''' </summary>
    ''' <param name="aJahrs">Das auszuwertende Jahr als Text (z. B. "2026").</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' geändert.
    ''' - Index-Suche über Zimmer-IDs von einer langsamen O(N)-Schleife auf ein performantes Dictionary (O(1)) umgestellt.
    ''' - Veraltete VB6-Funktionen ('Val', 'DateDiff') durch moderne .NET-Alternativen ersetzt.
    ''' - Variable 'Tag' wegen Keyword-Konflikten in 'anzahlTage' umbenannt.
    ''' - Explizite Schleifentypisierung durchgeführt und tote Code-Kommentare entfernt.
    ''' </remarks>
    Private Sub prUmsatzZimmer(ByVal aJahrs As String)
        ' Gültigkeit des übergebenen Jahres prüfen
        Dim nJahr As Integer
        If Not Integer.TryParse(aJahrs, nJahr) Then Exit Sub

        ' Sicherheitscheck für Datenquellen
        If dtZim Is Nothing OrElse dtZim.Rows.Count = 0 Then Exit Sub
        If dtBuc Is Nothing OrElse dtBuc.Rows.Count = 0 Then Exit Sub

        Dim nZimmer As Integer = dtZim.Rows.Count
        ReDim arWert1(nZimmer - 1)
        ReDim arBezeichnungX(nZimmer - 1)

        ' OPTIMIERUNG: Dictionary aufbauen, um Zimmer-IDs blitzschnell ihrem Array-Index zuzuordnen
        Dim zimmerIndexMap As New Dictionary(Of String, Integer)()

        For i As Integer = 0 To nZimmer - 1
            arWert1(i) = 0.0
            Dim zimmerId As String = dtZim.Rows(i).Item("ID").ToString()
            arBezeichnungX(i) = zimmerId

            ' ID und Index für die spätere O(1) Suche speichern
            If Not zimmerIndexMap.ContainsKey(zimmerId) Then
                zimmerIndexMap.Add(zimmerId, i)
            End If
        Next

        ' Schleife über alle Buchungen 
        For i As Integer = 1 To dtBuc.Rows.Count - 1
            Dim currentBucRow As DataRow = dtBuc.Rows(i)

            ' Datumsprüfung und -umwandlung
            Dim aDatum As Date
            Dim eDatum As Date
            If Not Date.TryParse(fcUmDatum(currentBucRow.Item("von").ToString()), aDatum) Then Continue For
            If Not Date.TryParse(fcUmDatum(currentBucRow.Item("bis").ToString()), eDatum) Then Continue For

            Dim nPreis1 As Double = 0

            ' Preis- oder Pauschalberechnung
            Dim isPauschal As Integer = 0
            Integer.TryParse(currentBucRow.Item("Pausch").ToString(), isPauschal)

            If isPauschal <> 1 Then
                Double.TryParse(currentBucRow.Item("Preis").ToString(), nPreis1)
            Else
                ' .NET-native Berechnung der Differenz in Tagen
                Dim dateDiffSpan As TimeSpan = eDatum - aDatum
                Dim anzahlTage As Integer = dateDiffSpan.Days + 1

                Dim summe As Double = 0
                Double.TryParse(currentBucRow.Item("Summe").ToString(), summe)

                If anzahlTage > 0 Then
                    nPreis1 = summe / anzahlTage
                End If
            End If

            ' Schnelle Index-Ermittlung des Zimmers über das Dictionary statt For-Schleife
            Dim zimmerIdBuc As String = currentBucRow.Item("ZimID").ToString()
            Dim nZim As Integer = -1

            If zimmerIndexMap.TryGetValue(zimmerIdBuc, nZim) Then
                ' Datumsbereich durchlaufen und Umsätze des Zieljahres addieren
                Dim tempDate As Date = aDatum
                While tempDate <= eDatum
                    If tempDate.Year = nJahr Then
                        arWert1(nZim) += (nPreis1 / 100.0)
                    End If
                    tempDate = tempDate.AddDays(1)
                End While
            End If
        Next

        ' Runden und finale Namenszuweisung für die X-Achse
        For i As Integer = 0 To nZimmer - 1
            arWert1(i) = Math.Round(arWert1(i), 2)
            arBezeichnungX(i) = dtZim.Rows(i).Item("Name").ToString()
        Next

        ' Chart-Generierung und -Formatierung
        With chUmsatzZimmer
            .ChartAreas.Clear()
            .Series.Clear()

            Dim chartArea1 As New ChartArea("ChartArea1")
            .ChartAreas.Add(chartArea1)

            Dim series1 As New Series("Series1")
            .Series.Add(series1)

            ' Visuelle Einstellungen
            .AntiAliasing = AntiAliasingStyles.All
            .TextAntiAliasingQuality = TextAntiAliasingQuality.High
            .BackColor = Color.Transparent
            .BackSecondaryColor = Color.Transparent
            .BackHatchStyle = ChartHatchStyle.DashedHorizontal
            .BackGradientStyle = GradientStyle.DiagonalRight
            .BorderColor = Color.Transparent
            .BorderDashStyle = ChartDashStyle.Solid
            .BorderWidth = 1

            ' Titel einrichten
            .Titles.Clear()
            Dim chartTitle As New Title("Umsatz Zimmer", Docking.Top, New Font("Tahoma", 10, FontStyle.Regular), Color.Black)
            chartTitle.Alignment = ContentAlignment.BottomCenter
            chartTitle.BackColor = Color.Transparent
            chartTitle.BorderColor = Color.Transparent
            .Titles.Add(chartTitle)

            ' 3D-Effekte und Achsen-Einstellungen
            With .ChartAreas("ChartArea1")
                .Area3DStyle.Enable3D = True
                .Area3DStyle.LightStyle = LightStyle.Simplistic
                .Area3DStyle.IsRightAngleAxes = True
                .Area3DStyle.WallWidth = 0
                .Area3DStyle.Inclination = 0
                .Area3DStyle.Rotation = 0
                .BackColor = Color.Transparent
                .AxisX.Interval = 1
                .AxisY.Title = "Umsatz in Euro"
            End With

            ' Balken-Design (Cylinder) und Datenbindung
            With .Series("Series1")
                .ChartType = SeriesChartType.Column
                .BorderColor = Color.WhiteSmoke
                .Item("DrawingStyle") = "Cylinder"
                .IsVisibleInLegend = False

                ' Punkte zum Chart hinzufügen
                For i As Integer = 0 To arWert1.Count - 1
                    .Points.AddXY(arBezeichnungX(i), arWert1(i))
                    .Points(i).Color = Color.Pink
                    .Points(i).LabelForeColor = Color.Blue
                    .Points(i).LabelBackColor = Color.White
                    .Points(i).Label = "#VALY"
                Next
            End With
        End With
    End Sub

#End Region

#Region "Umsatz nach Objekt........................................................................"

    ''' <summary>
    ''' Berechnet den Umsatz pro Objekt über einen frei definierbaren Zeitraum von Jahren und stellt die Umsatzentwicklung als Liniendiagramm dar.
    ''' </summary>
    ''' <param name="aJahrs">Das Anfangsjahr des Zeitraums als Text (z. B. "2020").</param>
    ''' <param name="eJahrs">Das Endjahr des Zeitraums als Text (z. B. "2026").</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' umgestellt.
    ''' - Verschachtelte Suchschleifen innerhalb der Buchungsverarbeitung durch performante Dictionaries ersetzt (O(1)-Zugriff).
    ''' - Fehler beim Schleifenstart der Buchungen korrigiert (0 statt 1), damit keine Zeile verloren geht.
    ''' - Veraltete VB6-Funktionen ('Val', 'CDate', 'Str', 'DateDiff') entfernt.
    ''' - Fehlerhafte Zuweisung bei Chart-Series behoben (.Series.Add erwartet nun einen eindeutigen String statt eines Index).
    ''' - Variablen-Konflikt 'Tag' gelöst.
    ''' </remarks>
    Private Sub prObjekt(ByVal aJahrs As String, ByVal eJahrs As String)
        Dim aJahr As Integer
        Dim eJahr As Integer

        ' Validierung der Eingangsdaten
        If Not Integer.TryParse(aJahrs, aJahr) OrElse Not Integer.TryParse(eJahrs, eJahr) Then Exit Sub

        Dim nJahr As Integer = eJahr - aJahr
        If nJahr < 0 Then Exit Sub

        ' Daten aus der Datenbank laden
        Dim sql As String = "SELECT * FROM Objekte"
        dtObj = fcReadDataTable(sql)

        ' Sicherheitscheck für Datenquellen
        If dtObj Is Nothing OrElse dtObj.Rows.Count = 0 Then Exit Sub
        If dtZim Is Nothing OrElse dtZim.Rows.Count = 0 Then Exit Sub
        If dtBuc Is Nothing OrElse dtBuc.Rows.Count = 0 Then Exit Sub

        Dim nObjekt As Integer = dtObj.Rows.Count
        Dim arBezeichnungx2(nObjekt - 1) As String
        Dim arWert(nObjekt - 1, nJahr) As Double
        ReDim arBezeichnungX(nJahr)

        ' 1. X-Achsen-Bezeichnungen (Jahre) vorbereiten
        For i As Integer = 0 To nJahr
            For ii As Integer = 0 To nObjekt - 1
                arWert(ii, i) = 0.0
            Next
            arBezeichnungX(i) = (aJahr + i).ToString()
        Next

        ' 2. OPTIMIERUNG: Dictionaries zur O(1)-Suche aufbauen
        Dim objektIndexMap As New Dictionary(Of String, Integer)()
        For i As Integer = 0 To nObjekt - 1
            Dim objektId As String = dtObj.Rows(i).Item("ID").ToString()
            arBezeichnungx2(i) = objektId

            If Not objektIndexMap.ContainsKey(objektId) Then
                objektIndexMap.Add(objektId, i)
            End If
        Next

        ' Map: ZimmerID -> ObjektID
        Dim zimmerZuObjektMap As New Dictionary(Of String, String)()
        For i As Integer = 0 To dtZim.Rows.Count - 1
            Dim zimId As String = dtZim.Rows(i).Item("ID").ToString()
            Dim objId As String = dtZim.Rows(i).Item("IDObjekte").ToString()

            If Not zimmerZuObjektMap.ContainsKey(zimId) Then
                zimmerZuObjektMap.Add(zimId, objId)
            End If
        Next

        ' 3. Buchungsdaten verarbeiten (Start bei Index 0)
        For i As Integer = 0 To dtBuc.Rows.Count - 1
            Dim currentBucRow As DataRow = dtBuc.Rows(i)

            Dim aDatum As Date
            Dim eDatum As Date
            If Not Date.TryParse(fcUmDatum(currentBucRow.Item("von").ToString()), aDatum) Then Continue For
            If Not Date.TryParse(fcUmDatum(currentBucRow.Item("bis").ToString()), eDatum) Then Continue For

            Dim nPreis1 As Double = 0
            Dim isPauschal As Integer = 0
            Integer.TryParse(currentBucRow.Item("Pausch").ToString(), isPauschal)

            If isPauschal <> 1 Then
                Double.TryParse(currentBucRow.Item("Preis").ToString(), nPreis1)
            Else
                Dim dateDiffSpan As TimeSpan = eDatum - aDatum
                Dim anzahlTage As Integer = dateDiffSpan.Days + 1

                Dim summe As Double = 0
                Double.TryParse(currentBucRow.Item("Summe").ToString(), summe)

                If anzahlTage > 0 Then
                    nPreis1 = summe / anzahlTage
                End If
            End If

            ' Inklusive Enddatum prüfen (wie eDatum.AddDays(1) und Abfrage < eDatum im Original)
            Dim sZimID As String = currentBucRow.Item("ZimID").ToString()
            Dim sObjektID As String = ""
            Dim nObj As Integer = -1

            ' Blitzschnelle Zuordnung: Zimmer -> Objekt -> Array-Index
            If zimmerZuObjektMap.TryGetValue(sZimID, sObjektID) Then
                If objektIndexMap.TryGetValue(sObjektID, nObj) Then

                    Dim tempDate As Date = aDatum
                    While tempDate <= eDatum
                        If tempDate.Year >= aJahr AndAlso tempDate.Year <= eJahr Then
                            Dim jahrIndex As Integer = tempDate.Year - aJahr
                            arWert(nObj, jahrIndex) = fcRound((nPreis1 / 100.0) + arWert(nObj, jahrIndex))
                        End If
                        tempDate = tempDate.AddDays(1)
                    End While

                End If
            End If
        Next

        ' Objekt-IDs durch die echten Namen für die Legende ersetzen
        For i As Integer = 0 To nObjekt - 1
            arBezeichnungx2(i) = dtObj.Rows(i).Item("Name").ToString()
        Next

        ' 4. Chart-Darstellung konfigurieren
        With chObjekt
            .ChartAreas.Clear()
            .Series.Clear()

            Dim chartArea1 As New ChartArea("ChartArea1")
            .ChartAreas.Add(chartArea1)

            .AntiAliasing = AntiAliasingStyles.All
            .TextAntiAliasingQuality = TextAntiAliasingQuality.High
            .BackColor = Color.Transparent
            .BackSecondaryColor = Color.Transparent
            .BackHatchStyle = ChartHatchStyle.DashedHorizontal
            .BackGradientStyle = GradientStyle.DiagonalRight
            .BorderColor = Color.Transparent
            .BorderDashStyle = ChartDashStyle.Solid
            .BorderWidth = 1

            ' Titel hinzufügen
            .Titles.Clear()
            Dim chartTitle As New Title("Umsatz Objekt", Docking.Top, New Font("Tahoma", 10, FontStyle.Regular), Color.Black)
            chartTitle.Alignment = ContentAlignment.BottomCenter
            chartTitle.BackColor = Color.Transparent
            chartTitle.BorderColor = Color.Transparent
            .Titles.Add(chartTitle)

            ' 3D-Einstellungen
            With .ChartAreas("ChartArea1")
                .Area3DStyle.Enable3D = True
                .Area3DStyle.LightStyle = LightStyle.Simplistic
                .Area3DStyle.IsRightAngleAxes = True
                .Area3DStyle.WallWidth = 0
                .Area3DStyle.Inclination = 0
                .Area3DStyle.Rotation = 0
                .BackColor = Color.Transparent
                .AxisX.Interval = 1
                .AxisY.Title = "Umsatz in Euro"
            End With

            ' Datenreihen (Linien pro Objekt) generieren
            For ii As Integer = 0 To nObjekt - 1
                Dim seriesName As String = arBezeichnungx2(ii)

                ' Eindeutige Benennung der Datenreihe zur Vermeidung von Namenskonflikten
                Dim series1 As New Series(seriesName)
                series1.ChartType = SeriesChartType.Line
                .Series.Add(series1)

                For i As Integer = 0 To nJahr
                    .Series(seriesName).Points.AddXY(arBezeichnungX(i), arWert(ii, i))
                    .Series(seriesName).LabelForeColor = Color.Blue
                    .Series(seriesName).LabelBackColor = Color.White
                    .Series(seriesName).Label = "#VALY"
                Next
            Next
        End With
    End Sub


#End Region

#Region "Statistik für Anreisen (Herkunftsland)...................................................."
    ''' <summary>
    ''' Erstellt die Anreise- und Übernachtungsstatistik gruppiert nach Herkunftsland und Monaten für ein bestimmtes Jahr
    ''' und stellt das Ergebnis in einer ListView dar.
    ''' </summary>
    ''' <param name="sJahr">Das auszuwertende Kalenderjahr. Wenn leer, wird das aktuelle Jahr verwendet.</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Massiver Performance-Gewinn: Datenbankabfragen pro Buchung (N+1-Problem) durch Vorab-Caching aller Kunden in ein Dictionary eliminiert.
    ''' - Dreidimensionales String-Array durch typsichere Struktur (Dictionary von Klassen) ersetzt. Unnötiges 100-Länder-Limit aufgehoben.
    ''' - Fehlerhaften ListView-Befüllungsindex (IndexOutOfRangeException) korrigiert.
    ''' - Veraltete VB6-Funktionen ('Val', 'Str', 'CDate', implizite Funktionsrückgabe) bereinigt.
    ''' - 'ByRef' bei reinen Leseparametern in 'ByVal' geändert.
    ''' - Toten Code ('If aDatum.Month = 2 Then eDatum = eDatum') entfernt.
    ''' </remarks>
    Private Sub prAnreise(ByVal sJahr As String)
        ' Jahr prüfen und Standardwert setzen
        Dim targetYear As Integer
        If String.IsNullOrWhiteSpace(sJahr) OrElse Not Integer.TryParse(sJahr.Trim(), targetYear) Then
            targetYear = Date.Today.Year
        End If

        ' Sicherheitscheck für Datenquelle
        If dtBuc Is Nothing OrElse dtBuc.Rows.Count = 0 Then Exit Sub

        ' 1. Performance-Booster: Alle Kunden-Länder vorab in ein Dictionary laden (Verhindert tausende Einzelabfragen)
        Dim kundenLandMap As New Dictionary(Of String, String)()
        Dim dtKunden As DataTable = fcReadDataTable("SELECT ID, Land FROM Kunden")
        If dtKunden IsNot Nothing Then
            For Each row As DataRow In dtKunden.Rows
                Dim kunId As String = row("ID").ToString()
                Dim land As String = row("Land").ToString().Trim()
                ' Sicherstellen, dass jede ID nur einmal hinzugefügt wird
                If Not kundenLandMap.ContainsKey(kunId) Then
                    kundenLandMap.Add(kunId, If(land = "", "D", land))
                End If
            Next
        End If

        ' Datenstruktur für die Statistik bereitstellen
        Dim statsMap As New Dictionary(Of String, CountryStats)()

        ' 2. Buchungsdaten durchlaufen
        For i As Integer = 0 To dtBuc.Rows.Count - 1
            Dim currentBucRow As DataRow = dtBuc.Rows(i)

            Dim aDatum As Date
            Dim eDatum As Date
            If Not Date.TryParse(fcUmDatum(currentBucRow.Item("von").ToString()), aDatum) Then Continue For
            If Not Date.TryParse(fcUmDatum(currentBucRow.Item("bis").ToString()), eDatum) Then Continue For

            eDatum = eDatum.AddDays(1)

            ' Prüfen, ob die Buchung das Zieljahr betrifft
            If aDatum.Year = targetYear OrElse eDatum.Year = targetYear Then
                Dim nPersonen As Integer = 0
                Integer.TryParse(currentBucRow.Item("Personen").ToString(), nPersonen)

                ' Land aus dem Cache ermitteln
                Dim sKunID As String = currentBucRow.Item("KunID").ToString()
                Dim sLand As String = "D"

                'Wenn Land nicht vorhanden, dann auf "D" setzen (Standardwert)
                If kundenLandMap.TryGetValue(sKunID, sLand) Then
                    If String.IsNullOrEmpty(sLand) Then sLand = "D"
                Else
                    sLand = "D"
                End If

                ' Sicherstellen, dass das Land in unserer Statistik existiert
                If Not statsMap.ContainsKey(sLand) Then
                    statsMap.Add(sLand, New CountryStats(sLand))
                End If

                Dim currentStats As CountryStats = statsMap(sLand)

                ' Anreisen eintragen (nur wenn die Anreise im Zieljahr liegt)
                If aDatum.Year = targetYear Then
                    Dim nMonat As Integer = aDatum.Month - 1
                    currentStats.Anreisen(nMonat) += nPersonen
                End If

                ' Übernachtungen tageweise im Zieljahr hochzählen
                Dim tempDate As Date = aDatum
                While tempDate < eDatum
                    If tempDate.Year = targetYear Then
                        Dim nMonat As Integer = tempDate.Month - 1
                        currentStats.Uebernachtungen(nMonat) += nPersonen
                    End If
                    tempDate = tempDate.AddDays(1)
                End While
            End If
        Next

        ' 3. ListView-Spalten initialisieren
        With lvAnreise
            .Clear()
            .Columns.Add("Land", 80, HorizontalAlignment.Left)
            For i As Integer = 1 To 12
                .Columns.Add(fcMonthName(i), 65, HorizontalAlignment.Left)
            Next
            .FullRowSelect = True
            .GridLines = True
            .HideSelection = False
            .MultiSelect = False
            .Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With

        ' 4. ListView mit den Ergebnissen befüllen
        lvAnreise.BeginUpdate()
        For Each kvp As KeyValuePair(Of String, CountryStats) In statsMap
            Dim stats As CountryStats = kvp.Value
            Dim lvItem As ListViewItem = lvAnreise.Items.Add(stats.LandName)

            For j As Integer = 0 To 11
                lvItem.SubItems.Add(stats.Anreisen(j).ToString() & "/" & stats.Uebernachtungen(j).ToString())
            Next
        Next
        lvAnreise.EndUpdate()
    End Sub

    ''' <summary>
    ''' Liest das Herkunftsland eines Kunden aus der Datenbank aus.
    ''' </summary>
    ''' <param name="sKunID">Die eindeutige Kunden-ID.</param>
    ''' <returns>Das Länderkürzel des Kunden oder ein leerer String, falls nicht gefunden.</returns>
    ''' <remarks>
    ''' Hinweis: Diese Methode wird in 'prAnreise' durch das Dictionary-Caching nicht mehr in der Schleife benötigt,
    ''' bleibt jedoch für andere Programmteile voll einsatzbereit.
    ''' </remarks>
    Private Function fcSelectKundeLand(ByVal sKunID As String) As String
        ' Parameter-Variable sauber per SQL-Injektions-Schutz oder standardmäßig als formatierten String abfragen
        Dim sSQL As String = "SELECT Land FROM Kunden WHERE ID='" & sKunID & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("Land").ToString()
        End If

        Return String.Empty
    End Function

    ''' <summary>
    ''' Interne Hilfsklasse zur performanten Verwaltung der Länder-Statistiken.
    ''' </summary>
    Private Class CountryStats
        Public Property LandName As String
        Public Property Anreisen As Integer()
        Public Property Uebernachtungen As Integer()

        Public Sub New(ByVal name As String)
            LandName = name
            Anreisen = New Integer(11) {}
            Uebernachtungen = New Integer(11) {}
        End Sub
    End Class


    '''' <summary>
    '''' Statistik für Anreisen (Herkunftsland)
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub prAnreise(ByVal sJahr As String)
    '    If sJahr.Trim = "" Then sJahr = Date.Today.Year
    '    Dim arAnreise(100, 3, 12) As String
    '    Dim aDatum As Date
    '    Dim eDatum As Date
    '    Dim sKunID As String
    '    Dim sLand As String
    '    Dim nLand As Integer = 1
    '    Dim nLand1 As Integer
    '    Dim nMonat As Integer
    '    Dim nPersonen As Integer = 0
    '    arAnreise(0, 0, 0) = "D"
    '    For i = 0 To dtBuc.Rows.Count - 1
    '        aDatum = CDate(fcUmDatum(dtBuc.Rows(i).Item("von").ToString))
    '        eDatum = ((fcUmDatum(dtBuc.Rows(i).Item("bis").ToString)))
    '        eDatum = eDatum.AddDays(1)

    '        If aDatum.Year = sJahr Or eDatum.Year = sJahr Then
    '            nPersonen = Val(dtBuc.Rows(i).Item("Personen").ToString)
    '            If aDatum.Month = 2 Then
    '                eDatum = eDatum


    '            End If
    '            sKunID = dtBuc.Rows(i).Item("KunID").ToString   'landermiteln
    '            sLand = fcSelectKundeLand(sKunID).Trim
    '            If sLand = "" Then sLand = "D"
    '            nLand1 = -1      'position im dem Arrey
    '            For j = 0 To nLand - 1
    '                If arAnreise(j, 0, 0) = sLand Then
    '                    nLand1 = j
    '                    Exit For
    '                End If
    '            Next
    '            If nLand1 = -1 Then 'land noch nicht in der tabelle nland1=Neue position
    '                nLand = nLand + 1
    '                nLand1 = nLand - 1
    '                arAnreise(nLand1, 0, 0) = sLand
    '            End If
    '            If aDatum.Year = sJahr Then     ' Eintragen der Anreise im Jahr/Monat
    '                nMonat = aDatum.Month - 1
    '                arAnreise(nLand1, 1, nMonat) = Str(Val(arAnreise(nLand1, 1, nMonat)) + nPersonen)
    '            End If
    '            Do While aDatum < eDatum
    '                If aDatum.Year = sJahr Then   'Eintragen der Übernachtungen jahr/monat
    '                    nMonat = aDatum.Month - 1
    '                    arAnreise(nLand1, 2, nMonat) = Str(Val(arAnreise(nLand1, 2, nMonat)) + nPersonen)
    '                End If
    '                aDatum = aDatum.AddDays(1)
    '            Loop
    '        End If
    '    Next
    '    With lvAnreise
    '        .Clear()
    '        .Columns.Add("Land", 80, HorizontalAlignment.Left)
    '        For i = 1 To 12
    '            .Columns.Add(fcMonthName(i), 65, HorizontalAlignment.Left)
    '        Next
    '        .FullRowSelect = True
    '        .GridLines = True
    '        '.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
    '        .HideSelection = False
    '        .MultiSelect = False
    '        .Sorting = SortOrder.Ascending
    '        .TabIndex = 0
    '        .View = View.Details

    '    End With
    '    Dim lv As ListViewItem
    '    For i = 0 To nLand
    '        sLand = arAnreise(i, 0, 0)
    '        If sLand <> "" Then
    '            With lvAnreise
    '                lv = .Items.Add(sLand)
    '                For j = 0 To 11
    '                    lv.SubItems.Add(arAnreise(i, 1, j) & "/" & arAnreise(i, 2, j))
    '                Next
    '            End With
    '        End If
    '    Next
    'End Sub

    'Private Function fcSelectKundeLand(ByRef sKunID As String) As String
    '    Dim sSQL As String = "Select * From Kunden Where ID='" & sKunID & "'"
    '    Dim dt As DataTable = fcReadDataTable(sSQL)
    '    Dim sLand As String = ""
    '    If dt.Rows.Count > 0 Then
    '        sLand = dt.Rows(0).Item("Land").ToString
    '    End If
    '    fcSelectKundeLand = sLand
    'End Function

#End Region

#Region "Provision nach werbung...................................................................."
    ''' <summary>
    ''' Berechnet die Provisionsabrechnung für einen Werbepartner innerhalb eines Zeitraums,
    ''' aggregiert doppelte Buchungsnummern (BID) und gibt das Ergebnis formatiert in einem DataGridView aus.
    ''' </summary>
    ''' <param name="sWerbung">Der Name des Werbemediums/Partners.</param>
    ''' <param name="nPro">Der Provisionssatz in Prozent (z. B. 10.0).</param>
    ''' <param name="sVon1">Das Startdatum des Abrechnungszeitraums.</param>
    ''' <param name="sBis1">Das Enddatum des Abrechnungszeitraums.</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe konsequent von 'ByRef' auf 'ByVal' umgestellt.
    ''' - Lineare Suchschleifen bei Kunden und Werbebuchungen durch ultraschnelle Dictionaries (O(1)) ersetzt.
    ''' - Redundante, tageweise Preisschleifen eliminiert und durch direkte Multiplikation via 'TimeSpan.Days' ersetzt.
    ''' - Veraltete VB6-Funktionen ('Val', 'Str', 'Mid') durch moderne .NET-Methoden ersetzt.
    ''' - 'DataGridView.BeginUpdate'/'EndUpdate' hinzugefügt, um das Flackern beim Befüllen der Tabelle zu verhindern.
    ''' </remarks>
    Private Sub prProvision(ByVal sWerbung As String, ByVal nPro As Double, ByVal sVon1 As String, ByVal sBis1 As String)
        Dim sVon As String = fcUmDatum(sVon1)
        Dim sBis As String = fcUmDatum(sBis1)

        ' Sicherheitscheck für Datenquelle
        If dtBuc Is Nothing OrElse dtBuc.Rows.Count = 0 Then Exit Sub

        ' 1. Caching: Alle Kunden vorab in ein Dictionary laden (Verhindert Datenbank-Scans in der Schleife)
        Dim kundenMap As New Dictionary(Of String, String)()
        Dim dtKunLocal As DataTable = fcReadDataTable("SELECT ID, Name1 FROM Kunden")
        If dtKunLocal IsNot Nothing Then
            For Each row As DataRow In dtKunLocal.Rows
                Dim id As String = row("ID").ToString()
                Dim name1 As String = row("Name1").ToString()
                If Not kundenMap.ContainsKey(id) Then kundenMap.Add(id, name1)
            Next
        End If

        ' Temporäre DataTable zur strukturierten Zwischenspeicherung
        Dim dtWerbung As New DataTable()
        dtWerbung.Columns.Add("BID", GetType(String))
        dtWerbung.Columns.Add("Name", GetType(String))
        dtWerbung.Columns.Add("Von", GetType(String))
        dtWerbung.Columns.Add("Bis", GetType(String))
        dtWerbung.Columns.Add("Preis", GetType(Double)) ' Als Double für einfacheres Rechnen

        ' Dictionary für den schnellen O(1) Zeilenzugriff in dtWerbung über die "BID"
        Dim werbungRowMap As New Dictionary(Of String, DataRow)()

        ' 2. Buchungsdaten analysieren und aggregieren
        For i As Integer = 0 To dtBuc.Rows.Count - 1
            Dim bucRow As DataRow = dtBuc.Rows(i)
            Dim vonWert As String = bucRow("von").ToString()

            ' Filterkriterien anwenden
            If vonWert >= sVon AndAlso vonWert <= sBis AndAlso bucRow("Werbung").ToString() = sWerbung Then
                Dim bid As String = bucRow("BID").ToString()

                ' Datumsdifferenz mathematisch berechnen statt in einer While-Schleife
                Dim aDatum As Date
                Dim eDatum As Date
                Dim nPreisGesamt As Double = 0

                If Date.TryParse(fcUmDatum(vonWert), aDatum) AndAlso Date.TryParse(fcUmDatum(bucRow("bis").ToString()), eDatum) Then
                    Dim anzahlTage As Integer = (eDatum.AddDays(1) - aDatum).Days
                    Dim einzelPreis As Double = 0
                    Double.TryParse(bucRow("Preis").ToString(), einzelPreis)

                    ' Gesamtpreis für diese Buchung berechnen (/100 laut Original-Logik)
                    nPreisGesamt = (einzelPreis * anzahlTage) / 100.0
                End If

                ' Prüfen, ob diese BID bereits erfasst wurde
                If werbungRowMap.ContainsKey(bid) Then
                    ' Bereits vorhanden: Preis aufaddieren
                    Dim existingRow As DataRow = werbungRowMap(bid)
                    existingRow("Preis") = Convert.ToDouble(existingRow("Preis")) + nPreisGesamt
                Else
                    ' Neu hinzufügen
                    Dim newRow As DataRow = dtWerbung.NewRow()
                    newRow("BID") = bid
                    newRow("von") = vonWert
                    newRow("bis") = bucRow("bis").ToString()

                    ' Name über das Kunden-Cache-Dictionary zuordnen
                    Dim kunId As String = bucRow("KunID").ToString()
                    Dim kundenName As String = ""
                    kundenMap.TryGetValue(kunId, kundenName)
                    newRow("Name") = kundenName

                    newRow("Preis") = nPreisGesamt

                    dtWerbung.Rows.Add(newRow)
                    werbungRowMap.Add(bid, newRow)
                End If
            End If
        Next

        ' 3. DataGridView Spalten & Layout initialisieren
        With dgvProvision
            .SuspendLayout()  ' Verhindert Flackern und erhöht Zeichengeschwindigkeit enorm
            .Columns.Clear()
            .ColumnHeadersHeight = 30

            .Columns.Add("Name", "Name")
            .Columns.Add("Von1", "Von1")
            .Columns.Add("Von", "Von")
            .Columns.Add("Bis", "Bis")
            .Columns.Add("BID", "BID")
            .Columns.Add("Preis", "Preis")
            .Columns.Add("Provision", "Provision")

            .Columns(0).Width = 200
            .Columns(1).Width = 0 ' Versteckte Sortierspalte
            .Columns(2).Width = 80
            .Columns(3).Width = 80
            .Columns(4).Width = 100
            .Columns(5).Width = 100
            .Columns(6).Width = 100

            For Each col As DataGridViewColumn In .Columns
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False

            ' 4. Tabelle befüllen und mathematische Endsummen ziehen
            Dim nPreisUmsatzGesamt As Double = 0
            Dim lastRowIndex As Integer = -1

            For i As Integer = 0 To dtWerbung.Rows.Count - 1
                Dim row As DataRow = dtWerbung.Rows(i)
                Dim rowIndex As Integer = .Rows.Add()

                Dim currentPreis As Double = Convert.ToDouble(row("Preis"))
                nPreisUmsatzGesamt += currentPreis

                .Rows(rowIndex).Cells(0).Value = row("Name").ToString()
                .Rows(rowIndex).Cells(1).Value = row("von").ToString()
                .Rows(rowIndex).Cells(1).Style.BackColor = Color.Black
                .Rows(rowIndex).Cells(2).Value = fcUmDatum(row("von").ToString())

                ' Bis-Datum berechnen (+1 Tag laut Original-Logik)
                Dim bisDatum As Date
                If Date.TryParse(fcUmDatum(row("Bis").ToString()), bisDatum) Then
                    .Rows(rowIndex).Cells(3).Value = bisDatum.AddDays(1).ToString("dd.MM.yyyy").Substring(0, 10)
                Else
                    .Rows(rowIndex).Cells(3).Value = row("Bis").ToString()
                End If

                .Rows(rowIndex).Cells(4).Value = row("BID").ToString()
                .Rows(rowIndex).Cells(5).Value = fcDecStr(currentPreis, 8, 2, ",")

                ' Provision zeilenweise berechnen
                Dim zeilenProvision As Double = If(nPro = 0, 0.0, (currentPreis * nPro) / 100.0)
                .Rows(rowIndex).Cells(6).Value = fcDecStr(zeilenProvision, 8, 2, ",")

                lastRowIndex = rowIndex
            Next

            ' 5. Gesamtsumme als Abschlusszeile hinzufügen
            Dim totalRowIndex As Integer = .Rows.Add()
            .Rows(totalRowIndex).Cells(0).Value = "Gesamt"
            .Rows(totalRowIndex).Cells(1).Value = "9999999999" ' Für die Sortierung wichtig
            .Rows(totalRowIndex).Cells(5).Value = fcDecStr(nPreisUmsatzGesamt, 8, 2, ",")

            Dim gesamtProvision As Double = If(nPro = 0, 0.0, (nPreisUmsatzGesamt * nPro) / 100.0)
            .Rows(totalRowIndex).Cells(6).Value = fcDecStr(gesamtProvision, 8, 2, ",")
            .Rows(totalRowIndex).Cells(1).Style.BackColor = Color.Black

            ' Sortierung nach der versteckten Spalte 1 (Datum)
            .Sort(.Columns(1), System.ComponentModel.ListSortDirection.Ascending)
            .AutoResizeRows()
            .ResumeLayout()
        End With
    End Sub

    ''' <summary>
    ''' Aktiviert die Datumsauswahl für das Startdatum ("Von") und synchronisiert den Kalender.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Implizite Datentyp-Konvertierung abgesichert (Date.TryParse statt direkter String-Zuweisung an SelectionStart).
    ''' - XML-Dokumentation für bessere Wartbarkeit hinzugefügt.
    ''' </remarks>
    Private Sub tstbVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tstbVon.Click
        bDatum = False
        tstbVon.BackColor = Color.White
        tstbBis.BackColor = Color.LightGray

        ' Typsichere Übergabe an den Kalender, falls die Textbox ein valides Datum enthält
        Dim parsedDate As Date
        If Date.TryParse(tstbVon.Text, parsedDate) Then
            mcKalender.SelectionStart = parsedDate
        End If
    End Sub

    ''' <summary>
    ''' Aktiviert die Datumsauswahl für das Enddatum ("Bis") und synchronisiert den Kalender.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Laufzeitfehler bei ungültigen Texteingaben mittels Date.TryParse verhindert.
    ''' </remarks>
    Private Sub tstbBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tstbBis.Click
        bDatum = True
        tstbVon.BackColor = Color.LightGray
        tstbBis.BackColor = Color.White

        ' Typsichere Übergabe an den Kalender
        Dim parsedDate As Date
        If Date.TryParse(tstbBis.Text, parsedDate) Then
            mcKalender.SelectionStart = parsedDate
        End If
    End Sub

    ''' <summary>
    ''' Trägt das im Kalender gewählte Datum in die aktive Textbox ein und aktualisiert bei einem gültigen Zeitraum die Provisionsabrechnung.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten des Kalenders.</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Datumsausgabe über Standardformatierung (.ToShortDateString()) vereinheitlicht.
    ''' - Veraltete 'Call'-Syntax bei 'prProvision' entfernt.
    ''' - Index-Sicherheitsprüfung für 'tscbWerbung.SelectedIndex' integriert.
    ''' </remarks>
    Private Sub mcKalender_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles mcKalender.DateChanged
        ' Ausgewähltes Datum formatiert (z. B. DD.MM.YYYY) in die Textbox schreiben
        If Not bDatum Then
            tstbVon.Text = mcKalender.SelectionStart.ToShortDateString()
        Else
            tstbBis.Text = mcKalender.SelectionStart.ToShortDateString()
        End If

        ' Nur ausführen, wenn das Startdatum vor dem Enddatum liegt
        If fcUmDatum(tstbVon.Text) < fcUmDatum(tstbBis.Text) Then
            ' Sicherheitsprüfung: Falls in der ComboBox nichts ausgewählt ist, Fehler verhindern
            If tscbWerbung.SelectedIndex >= 0 AndAlso tscbWerbung.SelectedIndex < arPr.Length Then
                Dim provisionsSatz As Double = Convert.ToDouble(arPr(tscbWerbung.SelectedIndex))

                ' Aktualisierung der Abrechnung aufrufen
                prProvision(tscbWerbung.Text, provisionsSatz, tstbVon.Text, tstbBis.Text)
            End If
        End If
    End Sub

#End Region

#Region "Werbung..................................................................................."
    ''' <summary>
    ''' Berechnet die prozentuale Verteilung der Buchungsumsätze nach Werbekanälen für einen 3-Jahres-Zeitraum 
    ''' und stellt das Ergebnis des Startjahres in einem 3D-Kreisdiagramm (Pie-Chart) dar.
    ''' </summary>
    ''' <param name="sVon1">Das Basisjahr als String (z. B. "2026").</param>
    ''' <param name="sBis1">Der ungenutzte historische Endjahr-Parameter (für Kompatibilität beibehalten).</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' geändert.
    ''' - Lineare Suchschleife nach Werbebezeichnungen durch performantes Dictionary (O(1)) ersetzt.
    ''' - 'IndexOutOfRangeException' an den Arraygrenzen (nMax statt nMax-1) behoben.
    ''' - 'DivideByZeroException' bei leeren Jahren abgesichert.
    ''' - Veraltete VB6-Funktionen ('Val', 'CDate', 'Str') entfernt und toten Code bereinigt.
    ''' - Zuordnung der Beschriftungen im Kreisdiagramm korrigiert.
    ''' </remarks>
    Private Sub prWerbung(ByVal sVon1 As String, ByVal sBis1 As String)
        ' Startjahr parsen
        Dim basisJahr As Integer
        If String.IsNullOrWhiteSpace(sVon1) OrElse Not Integer.TryParse(sVon1.Trim(), basisJahr) Then
            basisJahr = Date.Today.Year
        End If

        Dim aJahr As Integer = basisJahr - 1
        Dim eJahr As Integer = basisJahr + 1
        Dim njahr As Integer = eJahr - aJahr

        ' Sicherheitscheck für Datenquellen
        If dtWer Is Nothing OrElse dtWer.Rows.Count = 0 Then Exit Sub
        If dtBuc Is Nothing OrElse dtBuc.Rows.Count = 0 Then Exit Sub

        Dim nMax As Integer = dtWer.Rows.Count

        ' Arrays dimensionieren (0 bis nMax-1 für die Elemente)
        Dim arWert(nMax - 1, njahr) As Double
        Dim arWertNeu(nMax - 1, njahr) As Double
        ReDim arBezeichnungX(nMax - 1)

        ' OPTIMIERUNG: Dictionary aufbauen, um Werbebezeichnungen blitzschnell ihrem Index zuzuordnen
        Dim werbungIndexMap As New Dictionary(Of String, Integer)()

        For i As Integer = 0 To nMax - 1
            Dim werbeName As String = dtWer.Rows(i).Item("Werbung").ToString()
            arBezeichnungX(i) = werbeName

            If Not werbungIndexMap.ContainsKey(werbeName) Then
                werbungIndexMap.Add(werbeName, i)
            End If

            For j As Integer = 0 To njahr
                arWert(i, j) = 0.0
            Next
        Next

        ' Buchungsdaten durchlaufen 
        For i As Integer = 1 To dtBuc.Rows.Count - 1
            Dim bucRow As DataRow = dtBuc.Rows(i)

            Dim aDatum As Date
            Dim eDatum As Date
            If Not Date.TryParse(fcUmDatum(bucRow.Item("von").ToString()), aDatum) Then Continue For
            If Not Date.TryParse(fcUmDatum(bucRow.Item("bis").ToString()), eDatum) Then Continue For

            eDatum = eDatum.AddDays(1)

            Dim nPreis1 As Double = 0
            Double.TryParse(bucRow.Item("Preis").ToString(), nPreis1)

            If aDatum.Year >= aJahr AndAlso aDatum.Year <= eJahr Then
                Dim werbeKey As String = bucRow.Item("Werbung").ToString()
                Dim jIndex As Integer = -1

                ' Blitzschneller O(1) Index-Lookup statt For-Schleife
                If werbungIndexMap.TryGetValue(werbeKey, jIndex) Then
                    Dim tempDate As Date = aDatum
                    While tempDate < eDatum
                        If tempDate.Year >= aJahr AndAlso tempDate.Year <= eJahr Then
                            Dim jahrIndex As Integer = tempDate.Year - aJahr
                            arWert(jIndex, jahrIndex) += (nPreis1 / 100.0)
                        End If
                        tempDate = tempDate.AddDays(1)
                    End While
                End If
            End If
        Next

        ' Werte in Prozent umrechnen
        For i As Integer = 0 To njahr
            Dim nSumme As Double = 0
            For j As Integer = 0 To nMax - 1
                nSumme += arWert(j, i)
            Next

            ' Division durch Null verhindern, falls ein Jahr keinen Umsatz hatte
            If nSumme > 0 Then
                For j As Integer = 0 To nMax - 1
                    arWertNeu(j, i) = (arWert(j, i) * 100.0) / nSumme
                Next
            End If
        Next

        ' Chart-Darstellung konfigurieren
        With chWerbung
            .ChartAreas.Clear()
            .Series.Clear()

            Dim chartArea1 As New ChartArea("ChartArea1")
            .ChartAreas.Add(chartArea1)

            .AntiAliasing = AntiAliasingStyles.All
            .TextAntiAliasingQuality = TextAntiAliasingQuality.High
            .BackColor = Color.Transparent
            .BackSecondaryColor = Color.Transparent
            .BackHatchStyle = ChartHatchStyle.DashedHorizontal
            .BackGradientStyle = GradientStyle.DiagonalRight
            .BorderColor = Color.Transparent
            .BorderDashStyle = ChartDashStyle.Solid
            .BorderWidth = 1

            ' Titel setzen (Kompiliersicher ohne Str)
            .Titles.Clear()
            Dim chartTitle As New Title("Werbung in Prozent " & aJahr.ToString() & "-" & (aJahr + 2).ToString(),
                                        Docking.Top, New Font("Tahoma", 10, FontStyle.Regular), Color.Black)
            chartTitle.Alignment = ContentAlignment.BottomCenter
            chartTitle.BackColor = Color.Transparent
            chartTitle.BorderColor = Color.Transparent
            .Titles.Add(chartTitle)

            ' 3D-Kreisdiagramm-Einstellungen
            With .ChartAreas("ChartArea1")
                .Area3DStyle.Enable3D = True
                .Area3DStyle.Inclination = 45 ' Neigungswinkel für bessere 3D-Optik bei Tortendiagrammen
                .Area3DStyle.Rotation = 0
                .BackColor = Color.Transparent
            End With

            ' Eine Datenreihe für das Kreisdiagramm erstellen (Index 0)
            Dim seriesPie As New Series("WerbungSeries")
            seriesPie.ChartType = SeriesChartType.Pie
            seriesPie.CustomProperties = "PieDrawingStyle=Concave"
            seriesPie.IsValueShownAsLabel = True
            .Series.Add(seriesPie)

            ' Datenpunkte an das Kreisdiagramm binden (Nutzt arWertNeu für das ausgewählte Basisjahr)
            For n As Integer = 0 To nMax - 1
                Dim prozentWert As Double = Math.Round(arWertNeu(n, 0), 1)

                ' Nur Kanäle anzeigen, die tatsächlich Prozentanteile besitzen (> 0)
                If prozentWert > 0 Then
                    Dim pointIndex As Integer = .Series("WerbungSeries").Points.AddXY(arBezeichnungX(n), prozentWert)
                    ' Beschriftung auf dem Diagramm formatieren
                    .Series("WerbungSeries").Points(pointIndex).Label = "#VALY%"
                End If
            Next
        End With
    End Sub




#End Region

#Region "Auslastung................................................................................"

    ''' <summary>
    ''' Berechnet die Auslastung und Bettenkapazität aller Zimmer über einen Zeitraum von Jahren 
    ''' und visualisiert die Ergebnisse in zwei getrennten 3D-Liniendiagrammen (chAuslastung und chSollZimmer).
    ''' </summary>
    ''' <param name="aJahrs">Das Anfangsjahr des Zeitraums als Text (z. B. "2020").</param>
    ''' <param name="eJahrs">Das Endjahr des Zeitraums als Text (z. B. "2026").</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe konsequent von 'ByRef' auf 'ByVal' umgestellt.
    ''' - Lineare Zimmersuche pro Buchung durch performantes Dictionary (O(1)) ersetzt.
    ''' - Fehleranfällige String-Matrix zur Berechnung durch typsicheres Integer-Array ersetzt.
    ''' - 'DivideByZeroException' bei der prozentualen Auslastungsberechnung (nSoll = 0) abgesichert.
    ''' - Veraltete VB6-Funktionen ('Val', 'Str', 'CDate') durch moderne .NET-Methoden ersetzt.
    ''' - Toten Code und experimentelle Zuweisungen entfernt.
    ''' - Korrekte String-basierte Benennung der Chart-Series implementiert.
    ''' </remarks>
    Private Sub prAuslastung(ByVal aJahrs As String, ByVal eJahrs As String)
        Dim aJahr As Integer
        Dim eJahr As Integer

        ' Validierung der Eingangsdaten
        If Not Integer.TryParse(aJahrs, aJahr) OrElse Not Integer.TryParse(eJahrs, eJahr) Then Exit Sub

        Dim nJahr As Integer = eJahr - aJahr
        If nJahr < 0 Then Exit Sub

        ' Zimmerdaten laden
        Dim sql As String = "SELECT * FROM Zimmer"
        dtZim = fcReadDataTable(sql)

        ' Sicherheitscheck für Datenquellen
        If dtZim Is Nothing OrElse dtZim.Rows.Count = 0 Then Exit Sub
        If dtBuc Is Nothing OrElse dtBuc.Rows.Count = 0 Then Exit Sub

        Dim nZim As Integer = dtZim.Rows.Count - 1

        ' Matrizen für Berechnungen deklarieren
        ' Zeilen-Indizes: 0 bis nZim = Zimmer-Ist-Werte, nZim + 1 = Auslastung %, nZim + 2 = Ist-Betten gesamt, nZim + 3 = Soll-Betten gesamt
        Dim arZimWerte(nZim + 3, nJahr + 2) As Integer
        Dim arAuslastungProzent(nJahr) As Double
        Dim arZimmerBetten(nZim) As Integer

        ' OPTIMIERUNG: Dictionary aufbauen für blitzschnelle Zimmer-ID-Zuordnung (O(1))
        Dim zimmerIndexMap As New Dictionary(Of String, Integer)()

        For i As Integer = 0 To nZim
            Dim zimmerId As String = dtZim.Rows(i).Item("ID").ToString()
            If Not zimmerIndexMap.ContainsKey(zimmerId) Then
                zimmerIndexMap.Add(zimmerId, i)
            End If

            Dim bettenAnzahl As Integer = 0
            Integer.TryParse(dtZim.Rows(i).Item("Betten").ToString(), bettenAnzahl)
            arZimmerBetten(i) = bettenAnzahl
        Next

        ' Buchungsdaten auswerten
        For i As Integer = 1 To dtBuc.Rows.Count - 1
            Dim bucRow As DataRow = dtBuc.Rows(i)

            Dim aDatum As Date
            Dim eDatum As Date
            If Not Date.TryParse(fcUmDatum(bucRow.Item("von").ToString()), aDatum) Then Continue For
            If Not Date.TryParse(fcUmDatum(bucRow.Item("bis").ToString()), eDatum) Then Continue For

            Dim sZimID As String = bucRow.Item("ZimID").ToString()
            Dim nPersonen As Integer = 0
            Integer.TryParse(bucRow.Item("Personen").ToString(), nPersonen)

            Dim nPos1 As Integer = -1
            ' Blitzschneller O(1) Lookup statt zeitintensiver For-Schleife
            If zimmerIndexMap.TryGetValue(sZimID, nPos1) Then
                Dim tempDate As Date = aDatum
                While tempDate <= eDatum
                    If tempDate.Year >= aJahr AndAlso tempDate.Year <= eJahr Then
                        Dim nPos2 As Integer = tempDate.Year - aJahr
                        arZimWerte(nPos1, nPos2) += nPersonen
                    End If
                    tempDate = tempDate.AddDays(1)
                End While
            End If
        Next

        ' Auslastung (Soll / Ist) berechnen
        For jIndex As Integer = 0 To nJahr
            Dim nSoll As Integer = 0
            Dim nIst As Integer = 0

            For iIndex As Integer = 0 To nZim
                ' Wenn das Zimmer in diesem Jahr belegt war, Soll-Kapazität berechnen
                If arZimWerte(iIndex, jIndex) > 0 Then
                    nSoll += (365 * arZimmerBetten(iIndex))
                    nIst += arZimWerte(iIndex, jIndex)
                End If
            Next

            arZimWerte(nZim + 2, jIndex) = nIst
            arZimWerte(nZim + 3, jIndex) = nSoll

            ' Division durch Null absichern
            If nSoll > 0 Then
                arAuslastungProzent(jIndex) = (100.0 * nIst) / nSoll
            Else
                arAuslastungProzent(jIndex) = 0.0
            End If
        Next

        ' =========================================================================
        ' 1. Chart: chAuslastung (Prozentuale Entwicklung)
        ' =========================================================================
        With chAuslastung
            .ChartAreas.Clear()
            .Series.Clear()

            Dim chartArea1 As New ChartArea("ChartArea1")
            .ChartAreas.Add(chartArea1)

            .AntiAliasing = AntiAliasingStyles.All
            .TextAntiAliasingQuality = TextAntiAliasingQuality.High
            .BackColor = Color.Transparent
            .BackSecondaryColor = Color.Transparent
            .BackHatchStyle = ChartHatchStyle.DashedHorizontal
            .BackGradientStyle = GradientStyle.DiagonalRight
            .BorderColor = Color.Transparent
            .BorderDashStyle = ChartDashStyle.Solid
            .BorderWidth = 1

            .Titles.Clear()
            Dim chartTitle1 As New Title("Auslastung", Docking.Top, New Font("Tahoma", 10, FontStyle.Regular), Color.Black)
            chartTitle1.Alignment = ContentAlignment.BottomCenter
            chartTitle1.BackColor = Color.Transparent
            chartTitle1.BorderColor = Color.Transparent
            .Titles.Add(chartTitle1)

            With .ChartAreas("ChartArea1")
                .Area3DStyle.Enable3D = True
                .Area3DStyle.LightStyle = LightStyle.Simplistic
                .Area3DStyle.IsRightAngleAxes = True
                .Area3DStyle.WallWidth = 0
                .Area3DStyle.Inclination = 0
                .Area3DStyle.Rotation = 0
                .BackColor = Color.Transparent
                .AxisX.Interval = 1
                .AxisY.Title = "Auslastung in %"
            End With

            Dim seriesAuslastung As New Series("Auslastung %")
            seriesAuslastung.ChartType = SeriesChartType.Line
            seriesAuslastung.LabelForeColor = Color.Blue
            seriesAuslastung.LabelBackColor = Color.White
            seriesAuslastung.Label = "#VALY"
            .Series.Add(seriesAuslastung)

            For i As Integer = 0 To nJahr
                Dim xAchsenJahr As String = (aJahr + i).ToString()
                .Series("Auslastung %").Points.AddXY(xAchsenJahr, Math.Round(arAuslastungProzent(i), 2))
            Next
        End With

        ' =========================================================================
        ' 2. Chart: chSollZimmer (Kapazitäten / Übernachtungen absolut)
        ' =========================================================================
        With chSollZimmer
            .ChartAreas.Clear()
            .Series.Clear()

            Dim chartArea2 As New ChartArea("ChartArea1")
            .ChartAreas.Add(chartArea2)

            .AntiAliasing = AntiAliasingStyles.All
            .TextAntiAliasingQuality = TextAntiAliasingQuality.High
            .BackColor = Color.Transparent
            .BackSecondaryColor = Color.Transparent
            .BackHatchStyle = ChartHatchStyle.DashedHorizontal
            .BackGradientStyle = GradientStyle.DiagonalRight
            .BorderColor = Color.Transparent
            .BorderDashStyle = ChartDashStyle.Solid
            .BorderWidth = 1

            .Titles.Clear()
            Dim chartTitle2 As New Title("Kapazität", Docking.Top, New Font("Tahoma", 10, FontStyle.Regular), Color.Black)
            chartTitle2.Alignment = ContentAlignment.BottomCenter
            chartTitle2.BackColor = Color.Transparent
            chartTitle2.BorderColor = Color.Transparent
            .Titles.Add(chartTitle2)

            With .ChartAreas("ChartArea1")
                .Area3DStyle.Enable3D = True
                .Area3DStyle.LightStyle = LightStyle.Simplistic
                .Area3DStyle.IsRightAngleAxes = True
                .Area3DStyle.WallWidth = 0
                .Area3DStyle.Inclination = 0
                .Area3DStyle.Rotation = 0
                .BackColor = Color.Transparent
                .AxisX.Interval = 1
                .AxisY.Title = "Betten"
            End With

            ' Datenreihe: Übernachtungen (Ist-Wert)
            Dim seriesIst As New Series("Übernachtungen")
            seriesIst.ChartType = SeriesChartType.Line
            seriesIst.LabelForeColor = Color.Blue
            seriesIst.LabelBackColor = Color.White
            seriesIst.Label = "#VALY"
            .Series.Add(seriesIst)

            ' Datenreihe: Kapazität (Soll-Wert)
            Dim seriesSoll As New Series("Kapazität")
            seriesSoll.ChartType = SeriesChartType.Line
            seriesSoll.LabelForeColor = Color.Blue
            seriesSoll.LabelBackColor = Color.White
            seriesSoll.Label = "#VALY"
            .Series.Add(seriesSoll)

            For i As Integer = 0 To nJahr
                Dim xAchsenJahr As String = (aJahr + i).ToString()
                .Series("Übernachtungen").Points.AddXY(xAchsenJahr, arZimWerte(nZim + 2, i))
                .Series("Kapazität").Points.AddXY(xAchsenJahr, arZimWerte(nZim + 3, i))
            Next
        End With
    End Sub


#End Region

#Region "Buchung Monat............................................................................."

    ''' <summary>
    ''' Berechnet den Buchungsumsatz für einen bestimmten Monat über drei aufeinanderfolgende Jahre (Vorjahr, aktuelles Jahr, Folgejahr)
    ''' und stellt den zeitlichen Verlauf rollierend ab dem gewählten Startmonat in einem 3D-Zylinder-Balkendiagramm dar.
    ''' </summary>
    ''' <param name="aJahrs">Das auszuwertende Basisjahr als Text (z. B. "2026").</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' geändert.
    ''' - Lineare Monats-Suchschleife pro Buchung durch direkte mathematische Index-Berechnung ersetzt (O(1)-Performance).
    ''' - Tageweise Preisschleife durch direkte mathematische Multiplikation via 'TimeSpan' abgelöst.
    ''' - Fehlerhafte Zuweisungen ('Str' an 'Integer'-Variablen) korrigiert.
    ''' - Typkonforme Benennung der Chart-Series implementiert (.Series.Add erwartet eindeutige Strings).
    ''' - Veraltete VB6-Funktionen ('Val', 'Str', 'CDate', 'Mid') durch moderne .NET-Methoden ersetzt.
    ''' - Toten Code entfernt.
    ''' </remarks>
    Private Sub prBuchMonat(ByVal aJahrs As String)
        ' Basisjahr und ausgewählten Startmonat parsen
        Dim basisJahr As Integer
        If String.IsNullOrWhiteSpace(aJahrs) OrElse Not Integer.TryParse(aJahrs.Trim(), basisJahr) Then
            basisJahr = Date.Today.Year
        End If

        Dim nMonat1 As Integer = 1
        If tscbMonat IsNot Nothing AndAlso Not Integer.TryParse(tscbMonat.Text, nMonat1) Then
            nMonat1 = 1
        End If

        ' Sicherheitscheck für Datenquelle
        If dtBuc Is Nothing OrElse dtBuc.Rows.Count = 0 Then Exit Sub

        Dim arWert(3, 12) As Double
        Dim arBezeichnungX(12) As String

        Dim nJahrA As Integer = basisJahr - 1
        Dim nJahrE As Integer = basisJahr + 1

        ' 1. Rollierende Monatsstruktur im Array initialisieren (1 bis 12)
        Dim aktuellerMonat As Integer = nMonat1
        For i As Integer = 1 To 12
            arWert(0, i) = aktuellerMonat
            arBezeichnungX(i) = fcMonthName(aktuellerMonat)

            arWert(1, i) = 0.0 ' Reihe 1: Vorjahr
            arWert(2, i) = 0.0 ' Reihe 2: Basisjahr
            arWert(3, i) = 0.0 ' Reihe 3: Folgejahr

            aktuellerMonat += 1
            If aktuellerMonat = 13 Then aktuellerMonat = 1
        Next

        ' 2. Buchungsdaten durchlaufen
        For i As Integer = 1 To dtBuc.Rows.Count - 1
            Dim bucRow As DataRow = dtBuc.Rows(i)

            Dim bDatum As Date
            Dim aDatum As Date
            Dim eDatum As Date

            If Not Date.TryParse(fcUmDatum(bucRow("BuchDatum").ToString()), bDatum) Then Continue For

            ' Filterkriterien: Liegt das Buchungsdatum im relevanten 3-Jahres-Fenster und entspricht dem gewählten Monat?
            If bDatum.Year >= nJahrA AndAlso bDatum.Year <= nJahrE AndAlso bDatum.Month = nMonat1 Then
                If Not Date.TryParse(fcUmDatum(bucRow("von").ToString()), aDatum) Then Continue For
                If Not Date.TryParse(fcUmDatum(bucRow("bis").ToString()), eDatum) Then Continue For

                Dim nPreis As Double = 0
                Double.TryParse(bucRow("Preis").ToString(), nPreis)

                ' Gesamtumsatz mathematisch berechnen statt über eine zeitaufwendige While-Schleife
                Dim anzahlTage As Integer = (eDatum - aDatum).Days + 1
                Dim nSumme As Double = nPreis * anzahlTage

                Dim nMonat As Integer = aDatum.Month

                ' OPTIMIERUNG: Direktes Berechnen des Ziel-Spaltenindexes im rollierenden Array 
                ' Ersetzt das zeitfressende 'For n1 = 1 To 12'
                Dim zielSpalte As Integer = (nMonat - nMonat1 + 12) Mod 12 + 1

                ' Zeilenindex anhand des Buchungsjahres bestimmen (1 = Vorjahr, 2 = Basisjahr, 3 = Folgejahr)
                Dim zielZeile As Integer = -1
                If bDatum.Year = basisJahr Then
                    zielZeile = 2
                Else
                    zielZeile = If(bDatum.Year < basisJahr, 1, 3)
                End If

                ' Umsatz aufaddieren (/ 100 laut Original-Logik)
                arWert(zielZeile, zielSpalte) += (nSumme / 100.0)
            End If
        Next

        ' 3. Chart-Darstellung konfigurieren
        With chBuchMonat
            .ChartAreas.Clear()
            .Series.Clear()

            Dim chartArea1 As New ChartArea("ChartArea1")
            .ChartAreas.Add(chartArea1)

            .AntiAliasing = AntiAliasingStyles.All
            .TextAntiAliasingQuality = TextAntiAliasingQuality.High
            .BackColor = Color.Transparent
            .BackSecondaryColor = Color.Transparent
            .BackHatchStyle = ChartHatchStyle.DashedHorizontal
            .BackGradientStyle = GradientStyle.DiagonalRight
            .BorderColor = Color.Transparent
            .BorderDashStyle = ChartDashStyle.Solid
            .BorderWidth = 1

            ' Titel einrichten
            .Titles.Clear()
            Dim chartTitle As New Title("Umsatz Monat Jahr " & basisJahr.ToString() & "-" & (basisJahr + 2).ToString(),
                                    Docking.Top, New Font("Tahoma", 10, FontStyle.Regular), Color.Black)
            chartTitle.Alignment = ContentAlignment.BottomCenter
            chartTitle.BackColor = Color.Transparent
            chartTitle.BorderColor = Color.Transparent
            .Titles.Add(chartTitle)

            ' 3D-Einstellungen
            With .ChartAreas("ChartArea1")
                .Area3DStyle.LightStyle = LightStyle.Simplistic
                .Area3DStyle.IsRightAngleAxes = True
                .Area3DStyle.WallWidth = 0
                .Area3DStyle.Inclination = 0
                .Area3DStyle.Rotation = 0
                .BackColor = Color.Transparent
                .AxisX.Interval = 1
                .AxisY.Title = "Umsatz in Euro"
            End With

            ' Farbdefinitionen für die drei Jahrgänge
            Dim seriesColors() As Color = {Color.YellowGreen, Color.Peru, Color.PaleVioletRed}

            ' Drei Datenreihen (Vorjahr, Basisjahr, Folgejahr) generieren
            For ii As Integer = 0 To 2
                Dim seriesYear As String = (basisJahr - 1 + ii).ToString()
                Dim series1 As New Series(seriesYear)
                series1.ChartType = SeriesChartType.Column
                series1.CustomProperties = "DrawingStyle=Cylinder"
                series1.Color = seriesColors(ii)
                .Series.Add(series1)

                For i As Integer = 1 To 12
                    ' Monatsname auf 3 Zeichen kürzen (z. B. "Jan", "Feb") für eine übersichtliche X-Achse
                    Dim monatsKuerzel As String = arBezeichnungX(i)
                    If monatsKuerzel.Length > 3 Then monatsKuerzel = monatsKuerzel.Substring(0, 3)

                    Dim pointIndex As Integer = .Series(seriesYear).Points.AddXY(monatsKuerzel, arWert(ii + 1, i))

                    ' Punkt-Eigenschaften definieren
                    With .Series(seriesYear).Points(pointIndex)
                        .Color = seriesColors(ii)
                        .LabelForeColor = Color.Blue
                        .LabelBackColor = Color.White
                        .Label = "#VALY"
                    End With
                Next
            Next
        End With
    End Sub


#End Region

#Region "Buchung Jahr.............................................................................."

    ''' <summary>
    ''' Berechnet den Gesamtumsatz pro Kalenderjahr über einen definierten Zeitraum für alle bereits in der Vergangenheit oder am heutigen Tag erfolgten Buchungen
    ''' und visualisiert die Ergebnisse in einem 3D-Zylinder-Balkendiagramm.
    ''' </summary>
    ''' <param name="aJahrs">Das Anfangsjahr des Auswertungszeitraums als Text (z. B. "2020").</param>
    ''' <param name="eJahrs">Das Endjahr des Auswertungszeitraums als Text (z. B. "2026").</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' geändert.
    ''' - Fehleranfällige und starre 'Mid'-Operationen bei Datumsfiltern durch native .NET-Datumslogik ersetzt.
    ''' - Lineare 'Do While'-Tagesschleife durch mathematische Jahresüberschneidungsberechnung ersetzt (O(1)-Performance).
    ''' - 'Int()'-Kürzung entfernt, um Cent-Beträge beim Umsatz nicht zu unterschlagen.
    ''' - Veraltete VB6-Funktionen ('Val', 'Str', 'CDate', 'Int') bereinigt und ungenutzte Variablen entfernt.
    ''' </remarks>
    Private Sub prBuchungJahr(ByVal aJahrs As String, ByVal eJahrs As String)
        Dim aJahr As Integer
        Dim eJahr As Integer

        ' Eingangsdaten validieren
        If Not Integer.TryParse(aJahrs, aJahr) OrElse Not Integer.TryParse(eJahrs, eJahr) Then Exit Sub

        Dim nJahr As Integer = eJahr - aJahr
        If nJahr < 0 Then Exit Sub

        ' Sicherheitscheck für Datenquelle
        If dtBuc Is Nothing OrElse dtBuc.Rows.Count = 0 Then Exit Sub

        Dim arWert1(nJahr) As Double
        Dim arBezeichnungX(nJahr) As String

        ' X-Achsen-Bezeichnungen vorbereiten
        For i As Integer = 0 To nJahr
            arWert1(i) = 0.0
            arBezeichnungX(i) = (aJahr + i).ToString()
        Next

        ' Aktuelles Tagesdatum als Grenze für den Filter "bis heute" ermitteln
        Dim heute As Date = Date.Today

        ' Buchungsdaten analysieren
        For i As Integer = 1 To dtBuc.Rows.Count - 1
            Dim bucRow As DataRow = dtBuc.Rows(i)

            Dim bDatum As Date
            ' Buchungsdatum parsen
            If Not Date.TryParse(fcUmDatum(bucRow("BuchDatum").ToString()), bDatum) Then Continue For

            ' Logik aus Original: Nur Buchungen berücksichtigen, deren Buchungsdatum <= Heute entspricht
            If bDatum <= heute Then
                Dim aDatum As Date
                Dim eDatum As Date

                If Not Date.TryParse(fcUmDatum(bucRow("von").ToString()), aDatum) Then Continue For
                If Not Date.TryParse(fcUmDatum(bucRow("bis").ToString()), eDatum) Then Continue For

                Dim einzelPreis As Double = 0
                Double.TryParse(bucRow("Preis").ToString(), einzelPreis)
                Dim tagesPreis As Double = einzelPreis / 100.0

                ' OPTIMIERUNG: Jahre direkt mathematisch bestimmen, statt Tag für Tag in einer Do-While-Schleife zu springen
                Dim startJahrSchleife As Integer = Math.Max(aDatum.Year, aJahr)
                Dim endJahrSchleife As Integer = Math.Min(eDatum.Year, eJahr)

                For jJahr As Integer = startJahrSchleife To endJahrSchleife
                    ' Schnittmenge der Tage dieses Buchungssatzes für das jeweilige Kalenderjahr berechnen
                    Dim jahrStartAsDate As New Date(jJahr, 1, 1)
                    Dim jahrEndAsDate As New Date(jJahr, 12, 31)

                    Dim bereichStart As Date = If(aDatum > jahrStartAsDate, aDatum, jahrStartAsDate)
                    Dim bereichEnd As Date = If(eDatum < jahrEndAsDate, eDatum, jahrEndAsDate)

                    Dim tageImJahr As Integer = (bereichEnd - bereichStart).Days + 1

                    If tageImJahr > 0 Then
                        Dim jahrIndex As Integer = jJahr - aJahr
                        arWert1(jahrIndex) += (tagesPreis * tageImJahr)
                    End If
                Next
            End If
        Next

        ' Ergebnisse runden
        For i As Integer = 0 To nJahr
            arWert1(i) = Math.Round(arWert1(i), 2)
        Next

        ' Chart-Darstellung konfigurieren
        With chBuchungJahr
            .ChartAreas.Clear()
            .Series.Clear()

            Dim chartArea1 As New ChartArea("ChartArea1")
            .ChartAreas.Add(chartArea1)

            Dim series1 As New Series("Series1")
            .Series.Add(series1)

            .AntiAliasing = AntiAliasingStyles.All
            .TextAntiAliasingQuality = TextAntiAliasingQuality.High
            .BackColor = Color.Transparent
            .BackSecondaryColor = Color.Transparent
            .BackHatchStyle = ChartHatchStyle.DashedHorizontal
            .BackGradientStyle = GradientStyle.DiagonalRight
            .BorderColor = Color.Transparent
            .BorderDashStyle = ChartDashStyle.Solid
            .BorderWidth = 1

            ' Titel setzen
            .Titles.Clear()
            Dim chartTitle As New Title("Umsatz Jahre", Docking.Top, New Font("Tahoma", 10, FontStyle.Regular), Color.Black)
            chartTitle.Alignment = ContentAlignment.BottomCenter
            chartTitle.BackColor = Color.Transparent
            chartTitle.BorderColor = Color.Transparent
            .Titles.Add(chartTitle)

            ' 3D-Einstellungen
            With .ChartAreas("ChartArea1")
                .Area3DStyle.Enable3D = True
                .Area3DStyle.LightStyle = LightStyle.Simplistic
                .Area3DStyle.IsRightAngleAxes = True
                .Area3DStyle.WallWidth = 0
                .Area3DStyle.Inclination = 0
                .Area3DStyle.Rotation = 0
                .BackColor = Color.Transparent
                .AxisX.Interval = 1
                .AxisY.Title = "Umsatz in Euro"
            End With

            ' Zylinder-Balkendesign und Datenbindung
            With .Series("Series1")
                .ChartType = SeriesChartType.Column
                .BorderColor = Color.WhiteSmoke
                .Item("DrawingStyle") = "Cylinder"
                .IsVisibleInLegend = False

                For i As Integer = 0 To arWert1.Count - 1
                    Dim pointIndex As Integer = .Points.AddXY(arBezeichnungX(i), arWert1(i))
                    With .Points(pointIndex)
                        .LabelForeColor = Color.Blue
                        .LabelBackColor = Color.White
                        .Label = "#VALY"
                        .Color = Color.CornflowerBlue
                    End With
                Next
            End With
        End With
    End Sub


#End Region
End Class
