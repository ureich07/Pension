
Partial Public Class myKalender
    Inherits System.Windows.Forms.UserControl ' mit und ohne, bringt nichts

    ' Inherits System.Windows.Forms.UserControl ' Aktivieren, falls es sich um die Code-Behind-Datei eines UserControls handelt

    Private d As String = ""           ' Monat, der in der Mitte angezeigt werden soll
    Private dAn As String = "20150605" ' Anreisedatum
    Private dAb As String = "20150615" ' Abreisedatum
    Private aBuchung As String = "20150525-20150530"
    Private aSperr As String = "20150625-20150630"
    Private bAB As Boolean = False     ' True -> Anreisedatum ändern, False -> Abreisedatum ändern

    ''' <summary>
    ''' Holt oder setzt das anzuzeigende Fokus-Datum (z. B. der Monat, der in der Mitte des Kalenders steht).
    ''' </summary>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: XML-Kommentare hinzugefügt und Sichtbarkeit der Backing-Variable auf Private geändert.
    ''' </remarks>
    Public Property Datum() As String
        Get
            Return d
        End Get
        Set(ByVal value As String)
            d = value
            tbD.Text = d
        End Set
    End Property

    ''' <summary>
    ''' Holt oder setzt das ausgewählte Anreisedatum.
    ''' </summary>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: XML-Kommentare hinzugefügt.
    ''' </remarks>
    Public Property Anreise() As String
        Get
            Return dAn
        End Get
        Set(ByVal value As String)
            dAn = value
            tbAnreise.Text = dAn
        End Set
    End Property

    ''' <summary>
    ''' Holt oder setzt das ausgewählte Abreisedatum. 
    ''' Subtrahiert bei der Zuweisung automatisch einen Tag für die interne Kalenderdarstellung.
    ''' </summary>
    ''' <remarks>
    ''' 12.10.2026 - Fehlerbehebung &amp; Optimierung: Konvertierungsfehler bei CDate behoben, DateAdd durch .AddDays(-1) ersetzt.
    ''' </remarks>
    Public Property Abreise() As String
        Get
            Return dAb
        End Get
        Set(ByVal value As String)
            dAb = value
            If String.IsNullOrWhiteSpace(dAb) Then dAb = "20200101"

            ' Sicherstellen, dass die Datumskonvertierung nicht crasht
            ' Falls dAb im Format "20150615" ankommt, muss es korrekt in ein Date geparst werden
            Dim temporaeresDatum As Date

            ' Versucht das Datum im Format YYYYMMDD oder als Standard-Datum zu parsen
            If Date.TryParseExact(dAb, "yyyyMMDD", Nothing, System.Globalization.DateTimeStyles.None, temporaeresDatum) OrElse Date.TryParse(dAb, temporaeresDatum) Then
                ' 1 Tag abziehen (.NET Standard statt VB6 DateAdd)
                Dim d2 As Date = temporaeresDatum.AddDays(-1)
                dAb = fcUmDatum(d2)
            End If

            tbAbreise.Text = dAb
        End Set
    End Property

    ''' <summary>
    ''' Holt oder setzt den aktuellen Modus des Kalenders.
    ''' True = Anreise-Modus, False = Abreise-Modus.
    ''' </summary>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Bedingte Zuweisung vereinfacht und XML-Kommentare hinzugefügt.
    ''' </remarks>
    Public Property AnreiseAbreise() As Boolean
        Get
            Return bAB
        End Get
        Set(ByVal value As Boolean)
            bAB = value
            tbAnAb.Text = bAB.ToString()

            ' Label-Text basierend auf dem Boolean setzen
            If bAB Then
                lbAnAb.Text = "Anreise"
            Else
                lbAnAb.Text = "Abreise (Abreisetag - 1 Tag  => wenn Abreisetag 21. ist, dann als Abreisetag 20. auswählen!) "
            End If
        End Set
    End Property

    ''' <summary>
    ''' Holt oder setzt die Liste der bestehenden Buchungszeiträume (Format: "von-bis;von-bis").
    ''' </summary>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: XML-Kommentare hinzugefügt.
    ''' </remarks>
    Public Property Buchung() As String
        Get
            Return aBuchung
        End Get
        Set(ByVal value As String)
            aBuchung = value
            tbBuchung.Text = aBuchung
        End Set
    End Property

    ''' <summary>
    ''' Holt oder setzt die Liste der gesperrten Zeiträume (Format: "von-bis;von-bis").
    ''' </summary>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: XML-Kommentare hinzugefügt.
    ''' </remarks>
    Public Property Sperr() As String
        Get
            Return aSperr
        End Get
        Set(ByVal value As String)
            aSperr = value
            tbSperr.Text = aSperr
        End Set
    End Property

    ''' <summary>
    ''' Verarbeitet das Load-Ereignis des UserControls.
    ''' Initialisiert den Kalender beim Laden des Steuerelements mit den aktuellen Variablenwerten.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Veraltetes 'Call'-Schlüsselwort entfernt, ungenutzten auskommentierten Code bereinigt und XML-Kommentare hinzugefügt.
    ''' </remarks>
    Private Sub UserControl1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lbAnAb.AutoSize = False
        lbAnAb.Dock = DockStyle.Top
        ' Optionale Verschönerung: Text zentrieren und Höhe festlegen
        lbAnAb.TextAlign = ContentAlignment.MiddleCenter
        lbAnAb.Height = 30

        ' Kalender mit den initialen Werten aufbauen
        prKalender(d, dAn, dAb, aBuchung, aSperr, bAB)
    End Sub

    ''' <summary>
    ''' Zeichnet und initialisiert das DataGridView-Kalendersteuerelement (dgvKalender).
    ''' Berechnet die Spaltenbreiten, setzt die Wochentage und generiert die Ansicht für drei Monate 
    ''' (Vormonat, aktueller Monat, Folgemonat).
    ''' </summary>
    ''' <param name="sdatum">Das Fokus-Datum als String.</param>
    ''' <param name="Anreise">Das aktuelle Anreisedatum.</param>
    ''' <param name="Abreise">Das aktuelle Abreisedatum.</param>
    ''' <param name="Buchung">Die Liste der bestehenden Buchungszeiträume.</param>
    ''' <param name="Sperr">Die Liste der gesperrten Zeiträume.</param>
    ''' <param name="bAnAb">Der aktuelle Auswahlmodus (Anreise/Abreise).</param>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Parameter auf ByVal korrigiert, stark typisierte String-Arrays eingeführt, DateAdd und Call eliminiert.
    ''' </remarks>
    Private Sub prKalender(ByVal sdatum As String, ByVal Anreise As String, ByVal Abreise As String, ByVal Buchung As String, ByVal Sperr As String, ByVal bAnAb As Boolean)
        ' Stark typisierte Arrays statt des generischen Typs 'Array' verwenden
        Dim tag() As String = {"MO", "DI", "MI", "DO", "FR", "SA", "SO", " ", "MO", "DI", "MI", "DO", "FR", "SA", "SO", " ", "MO", "DI", "MI", "DO", "FR", "SA", "SO"}
        Dim sMonat() As String = {"", "Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember"}

        ' Panel-Dimensionen und Textfelder setzen
        pKalender.BackColor = Color.Khaki
        tbAnreise.Text = Anreise
        tbAbreise.Text = Abreise
        pKalender.Width = 680
        pKalender.Height = 280

        With dgvKalender
            .Width = 650
            .Height = 200
            .RowCount = 6
            .ColumnCount = 23
            .ColumnHeadersVisible = True
            .RowHeadersVisible = False
            .MultiSelect = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False
            .ClearSelection()
            .DefaultCellStyle.SelectionBackColor = Color.Transparent

            ' Spaltenbreiten und -höhen berechnen
            Dim w As Integer = CInt(.Width / 23)
            Dim h As Integer = CInt(.Height / 7)
            .ColumnHeadersHeight = h

            ' Grid initialisieren und Standard-Hintergrund setzen
            For i As Integer = 0 To 22
                .Columns(i).Width = w
                .Columns(i).Name = tag(i)
                For j As Integer = 0 To 5
                    .Rows(j).Height = h
                    .Rows(j).Cells(i).Value = ""
                    .Rows(j).Cells(i).Style.BackColor = Color.LightGray
                Next
            Next

            ' Fokus-Datum parsen
            Dim datum As Date = fcDatum(sdatum)

            ' 1. Vormonat generieren (Spalte 0)
            Dim d1 As Date = datum.AddMonths(-1)
            prCreateCalender(d1, 0, Anreise, Abreise, Buchung, Sperr)
            lMonat1.Text = sMonat(d1.Month) & " " & d1.Year

            ' 2. Aktueller Monat generieren (Spalte 8)
            Dim d2 As Date = datum
            prCreateCalender(d2, 8, Anreise, Abreise, Buchung, Sperr)
            lMonat2.Text = sMonat(d2.Month) & " " & d2.Year

            ' 3. Folgemonat generieren (Spalte 16)
            Dim d3 As Date = datum.AddMonths(1)
            prCreateCalender(d3, 16, Anreise, Abreise, Buchung, Sperr)
            lMonat3.Text = sMonat(d3.Month) & " " & d3.Year
        End With
    End Sub


    ''' <summary>
    ''' Zeichnet die Tage eines bestimmten Monats in einen festgelegten Spaltenbereich des DataGridViews (dgvKalender).
    ''' Färbt die Tage basierend auf dem aktuellen Buchungszeitraum (Grün), bestehenden Buchungen (GrünGelb) oder Sperrzeiten (Rot) ein.
    ''' </summary>
    ''' <param name="ddatum">Das Datum des anzuzeigenden Monats.</param>
    ''' <param name="nfeld">Der Startindex der Spalte im Grid (0 für Vormonat, 8 für aktuellen Monat, 16 für Folgemonat).</param>
    ''' <param name="sVon">Das aktuelle Anreisedatum als String.</param>
    ''' <param name="sBis">Das aktuelle Abreisedatum als String.</param>
    ''' <param name="Buchung">Die Liste der eigenen Buchungszeiträume (mit ";" und "-" getrennt).</param>
    ''' <param name="Sperr">Die Liste der gesperrten/fremden Zeiträume (mit ";" und "-" getrennt).</param>
    ''' <remarks>
    ''' 12.10.2026 - Performance- &amp; Bugfix-Optimierung: Parameter auf ByVal gesetzt, String-Splitting aus der Schleife ausgelagert, fehlerhaften Str()-Befehl durch New Date() ersetzt und VB6-Funktionen entfernt.
    ''' </remarks>
    Private Sub prCreateCalender(ByVal ddatum As Date, ByVal nfeld As Integer, ByVal sVon As String, ByVal sBis As String, ByVal Buchung As String, ByVal Sperr As String)
        Dim dVon As Date = fcDatum(sVon)
        Dim dBis As Date = fcDatum(sBis)

        ' Sicherer Neuaufbau des ersten Zustandstages des Monats ohne den fehleranfälligen Str()-Befehl
        Dim d11 As Date = New Date(ddatum.Year, ddatum.Month, 1)

        ' Bestimmung des korrekten Zeilen- und Spaltenstarts basierend auf dem Wochentag
        ' DayOfWeek.Monday ist 1. In .NET liefert DayOfWeek für Sonntag eine 0.
        Dim dayOfWeekOffset As Integer = If(d11.DayOfWeek = DayOfWeek.Sunday, 6, CInt(d11.DayOfWeek) - 1)
        Dim nCol As Integer = dayOfWeekOffset + nfeld
        Dim nRows As Integer = 0

        ' Zielmonat für die Abbruchbedingung festhalten
        Dim zielMonat As Integer = d11.Month

        ' PERFORMANCE-OPTIMIERUNG: Strings vorab parsen statt in jedem Schleifendurchlauf für jeden Tag neu!
        Dim buchungIntervalle As New List(Of Tuple(Of Date, Date))()
        If Not String.IsNullOrWhiteSpace(Buchung) Then
            For Each b In Buchung.Split(";"c)
                Dim parts = b.Split("-"c)
                If parts.Length = 2 Then
                    buchungIntervalle.Add(Tuple.Create(fcDatum(parts(0)), fcDatum(parts(1))))
                End If
            Next
        End If

        Dim sperrIntervalle As New List(Of Tuple(Of Date, Date))()
        If Not String.IsNullOrWhiteSpace(Sperr) Then
            For Each s In Sperr.Split(";"c)
                Dim parts = s.Split("-"c)
                If parts.Length = 2 Then
                    sperrIntervalle.Add(Tuple.Create(fcDatum(parts(0)), fcDatum(parts(1))))
                End If
            Next
        End If

        With dgvKalender
            ' Alle Tage des aktuellen Monats durchlaufen
            Do While d11.Month = zielMonat
                .Rows(nRows).Cells(nCol).Value = d11.Day

                ' Standardhintergrund (Weiß) für aktive Tage
                Dim zellenFarbe As Color = Color.White

                ' 1. Prüfung: Aktuelle Auswahl (Anreise bis Abreise)
                If d11 >= dVon AndAlso d11 <= dBis Then
                    zellenFarbe = Color.Green
                End If

                ' 2. Prüfung: Bestehende eigene Buchungen (GrünGelb)
                For Each intervall In buchungIntervalle
                    If d11 >= intervall.Item1 AndAlso d11 <= intervall.Item2 Then
                        zellenFarbe = Color.GreenYellow
                        Exit For
                    End If
                Next

                ' 3. Prüfung: Fremde Sperrzeiten (Rot überlagert eigene Buchungen)
                For Each intervall In sperrIntervalle
                    If d11 >= intervall.Item1 AndAlso d11 <= intervall.Item2 Then
                        zellenFarbe = Color.Red
                        Exit For
                    End If
                Next

                ' Farbe der Zelle zuweisen
                .Rows(nRows).Cells(nCol).Style.BackColor = zellenFarbe

                ' Spalten- und Zeilenumbruch steuern (7 Tage pro Block)
                nCol += 1
                If nCol = 7 + nfeld Then
                    nCol = 0 + nfeld
                    nRows += 1
                End If

                ' Nächsten Tag aufrufen (.NET Standard statt VB6 DateAdd)
                d11 = d11.AddDays(1)
            Loop
        End With
    End Sub


    ''' <summary>
    ''' Konvertiert einen kompakten Datums-String im SQL-Format (yyyyMMdd) in ein echtes .NET-Date-Objekt (z. B. 20080101 -> 01.01.2008).
    ''' </summary>
    ''' <param name="sDatum">Das zu konvertierende Datum im Format 'yyyyMMdd'.</param>
    ''' <returns>Das konvertierte Date-Objekt. Bei leeren oder ungültigen Eingaben wird das aktuelle Tagesdatum (Date.Today) zurückgegeben.</returns>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Umstellung von VB6-Stringfunktionen (Mid, Trim$) auf typsicheres DateTime.TryParseExact; Rückgabe über Return-Schlüsselwort modernisiert.
    ''' </remarks>
    Public Shared Function fcDatum(ByVal sDatum As String) As Date
        ' Prüfung auf leere Eingabe
        If String.IsNullOrWhiteSpace(sDatum) Then
            Return Date.Today
        End If

        Dim getrimmtesDatum As String = sDatum.Trim()
        Dim Formate() As String = {"dd.MM.yyyy", "d.M.yyyy", "dd.MM.yyyy HH:mm:ss", "d.M.yyyy H:mm:ss", "yyyyMMdd"}
        Dim dResult As Date

        ' Versucht das exakte Format yyyyMMdd zeitzonenunabhängig zu parsen
        If Date.TryParseExact(getrimmtesDatum, Formate, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, dResult) Then
            Return dResult
        Else
            ' Rückfallwert, falls das Format nicht exakt 8-stellig numerisch war (z. B. bei Beschädigung der Daten)
            Return Date.Today
        End If
    End Function

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der Zurück-Schaltfläche.
    ''' Erhöht das Fokus-Datum um einen Monat und aktualisiert die Kalenderansicht.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Gefährliche String-Zerschneidung (Mid) durch natives .ToString("yyyyMMdd") ersetzt, DateAdd und Call eliminiert.
    ''' </remarks>
    Private Sub btZur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles zurück.Click
        ' Fokus-Datum parsen und 1 Monat hinzufügen (.NET Standard statt VB6 DateAdd)
        Dim dD As Date = fcDatum(d).AddMonths(1)

        ' Direkt im korrekten Zielformat (yyyyMMdd) ohne Ländereinstellungsprobleme wegschreiben
        d = dD.ToString("yyyyMMdd")

        ' Kalender neu zeichnen
        prKalender(d, dAn, dAb, aBuchung, aSperr, bAB)
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der Vor-Schaltfläche.
    ''' Verringert das Fokus-Datum um einen Monat und aktualisiert die Kalenderansicht.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Gefährliche String-Zerschneidung (Mid) durch natives .ToString("yyyyMMdd") ersetzt, DateAdd und Call eliminiert.
    ''' </remarks>
    Private Sub tbVor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles vor.Click
        ' Fokus-Datum parsen und 1 Monat abziehen
        Dim dD As Date = fcDatum(d).AddMonths(-1)

        ' Direkt im korrekten Zielformat (yyyyMMdd) wegschreiben
        d = dD.ToString("yyyyMMdd")

        ' Kalender neu zeichnen
        prKalender(d, dAn, dAb, aBuchung, aSperr, bAB)
    End Sub

    ''' <summary>
    ''' Verarbeitet den Klick auf ein Datum im Kalender-Grid. 
    ''' Berechnet das ausgewählte Datum abhängig von der Spaltenregion (Vormonat, aktueller Monat, Folgemonat)
    ''' und aktualisiert das Anreise- oder Abreisedatum im Hauptformular nach erfolgreicher Überschneidungsprüfung.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten des Zellenklicks.</param>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Absicherung gegen Header-Klicks (-1) hinzugefügt, länderspezifische CDate/Mid-Kombination durch stabiles .ToString() ersetzt, Logikfehler bei Leerzellen verhindert.
    ''' </remarks>
    Private Sub dgvKalender_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvKalender.CellContentClick
        ' Ignorieren, wenn auf Header-Zeilen oder -Spalten geklickt wurde (-1)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

        With dgvKalender
            Dim zelle As DataGridViewCell = .Rows(e.RowIndex).Cells(e.ColumnIndex)

            ' Nur reagieren, wenn die Zelle weiß (verfügbar) oder grün (bereits ausgewählt) ist
            If zelle.Style.BackColor = Color.Green OrElse zelle.Style.BackColor = Color.White Then

                ' Prüfen, ob die Zelle überhaupt einen Wert enthält (Verhindert Fehler bei Leerzellen)
                If zelle.Value Is Nothing OrElse String.IsNullOrWhiteSpace(zelle.Value.ToString()) Then Exit Sub

                ' Tag zweistellig formatieren (aus "5" wird "05")
                Dim tag As String = zelle.Value.ToString().Trim()
                If tag.Length = 1 Then tag = "0" & tag

                Dim dx As String = ""
                Dim basisDatum As Date = fcDatum(d)

                ' Region 1: Vormonat (Spalten 0 bis 6)
                If e.ColumnIndex >= 0 AndAlso e.ColumnIndex <= 6 Then
                    Dim d1 As Date = basisDatum.AddMonths(-1)
                    dx = d1.ToString("yyyyMM") & tag
                End If

                ' Region 2: Aktueller Monat (Spalten 8 to 14)
                If e.ColumnIndex >= 8 AndAlso e.ColumnIndex <= 14 Then
                    dx = basisDatum.ToString("yyyyMM") & tag
                End If

                ' Region 3: Folgemonat (Spalten 16 to 22)
                If e.ColumnIndex >= 16 AndAlso e.ColumnIndex <= 22 Then
                    Dim d1 As Date = basisDatum.AddMonths(1)
                    dx = d1.ToString("yyyyMM") & tag
                End If

                ' Wenn ein gültiges Datum generiert werden konnte
                If Not String.IsNullOrEmpty(dx) Then
                    Dim nAnreise As Integer = Val(tbAnreise.Text)
                    Dim nAbreise As Integer = Val(tbAbreise.Text)
                    Dim nDx As Integer = Val(dx)

                    ' Prüfen, welcher Modus aktiv ist (Anreise ändern oder Abreise ändern)
                    If tbAnAb.Text = "True" Then
                        ' ANREISE DATUM ÄNDERN
                        If nAbreise >= nDx Then
                            ' Prüfung auf Überschneidungen mit bestehenden Buchungen/Sperren
                            If control(nDx, nAbreise, aBuchung & ";" & aSperr) = True Then
                                ' Ziel-Formular basierend auf Gerätetyp (Tablet oder Desktop) aktualisieren
                                If cgTablet = 0 Then
                                    frmReservierungDest.lbAnreise.Text = fcUmDatum(dx)
                                Else
                                    frmReservierung.lbAnreise.Text = fcUmDatum(dx)
                                End If
                            End If
                        End If
                    Else
                        ' ABREISE DATUM ÄNDERN
                        If nAnreise <= nDx Then
                            ' Prüfung auf Überschneidungen
                            If control(nAnreise, nDx, aBuchung & ";" & aSperr) = True Then
                                ' Bei Abreise einen Tag aufschlagen (.NET Standard statt VB6 DateAdd)
                                Dim d2 As Date = fcDatum(dx).AddDays(1)

                                If cgTablet = 0 Then
                                    frmReservierungDest.lbAbreise.Text = d2.ToString("dd.MM.yyyy")
                                Else
                                    frmReservierung.lbAbreise.Text = d2.ToString("dd.MM.yyyy")
                                End If
                            End If
                        End If
                    End If
                    If cgTablet = 0 Then
                        ' Aufruf für frmReservierungDest
                        BerechneUndSetzeTage(frmReservierungDest.lbAnreise, frmReservierungDest.lbAbreise, frmReservierungDest.lbTage)
                    Else
                        ' Aufruf für frmReservierung
                        BerechneUndSetzeTage(frmReservierung.lbAnreise, frmReservierung.lbAbreise, frmReservierung.lbTage)
                    End If


                End If

                End If
        End With
    End Sub

    ''' <summary>
    ''' Berechnet die Differenz in Tagen zwischen zwei Datumstexten (inklusive des Abreisetages) 
    ''' und schreibt das Ergebnis in das angegebene Ziel-Label.
    ''' </summary>
    ''' <param name="lblAnreise">Das Label, das das Anreisedatum enthält.</param>
    ''' <param name="lblAbreise">Das Label, das das Abreisedatum enthält.</param>
    ''' <param name="lblTage">Das Ziel-Label, in das die berechneten Tage eingetragen werden.</param>
    ''' <remarks>
    ''' 12.10.2026 - Code-Optimierung:
    ''' - Zusammenfassung von dupliziertem Code in eine zentrale, wiederverwendbare Methode.
    ''' - TimeSpan-Berechnung vereinfacht durch direkte Nutzung der '.Days'-Eigenschaft.
    ''' </remarks>
    Private Sub BerechneUndSetzeTage(ByVal lblAnreise As Label, ByVal lblAbreise As Label, ByVal lblTage As Label)
        ' Datumswerte einlesen und konvertieren
        Dim dVon As Date = fcDatum(lblAnreise.Text)
        Dim dBis As Date = fcDatum(lblAbreise.Text)
        ' Abreisedatum um einen Tag erhöhen (.NET-Alternative zu DateAdd)
        dBis = dBis.AddDays(1)
        ' Differenz berechnen und direkt als Text zuweisen (.Days liefert direkt den Integer-Wert)
        lblTage.Text = (dBis - dVon).Days.ToString()
    End Sub


    ''' <summary>
    ''' Prüft, ob ein geplanter Buchungszeitraum (Anreise bis Abreise) mit einer Liste von gesperrten oder bereits belegten Zeiträumen kollidiert.
    ''' </summary>
    ''' <param name="Anreise">Das geplante Anreisedatum als Ganzzahl (Format: yyyyMMdd).</param>
    ''' <param name="Abreise">Das geplante Abreisedatum als Ganzzahl (Format: yyyyMMdd).</param>
    ''' <param name="Sperr">Eine mit ";" und "-" formatierte Kette von gesperrten Zeiträumen.</param>
    ''' <returns>True, wenn der Zeitraum frei ist; False, wenn eine Überschneidung vorliegt.</returns>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Parameter auf ByVal gesetzt, vier verschachtelte If-Prüfungen auf eine mathematisch exakte Standardformel komprimiert, Return-Zuweisung modernisiert.
    ''' </remarks>
    Private Function control(ByVal Anreise As Integer, ByVal Abreise As Integer, ByVal Sperr As String) As Boolean
        ' Wenn keine Sperrzeiten übergeben wurden, ist der Zeitraum gültig
        If String.IsNullOrWhiteSpace(Sperr) Then Return True

        ' String am Semikolon aufteilen (Nutzt stark typisiertes String-Array)
        Dim vonBis1() As String = Sperr.Split(";"c)

        For i As Integer = 0 To vonBis1.Length - 1
            If vonBis1(i).Trim() <> "" Then
                Dim vonBis() As String = vonBis1(i).Split("-"c)

                ' Sicherstellen, dass das Intervall aus genau zwei Teilen (Von und Bis) besteht
                If vonBis.Length = 2 Then
                    Dim nAn As Integer = Val(vonBis(0))
                    Dim nAb As Integer = Val(vonBis(1))

                    ' Standardformel für Zeitüberschneidungen:
                    ' Ein Zeitraum überschneidet sich, wenn die neue Anreise VOR dem bestehenden Ende liegt
                    ' UND die neue Abreise NACH dem bestehenden Anfang liegt.
                    If Anreise < nAb AndAlso Abreise > nAn Then
                        Return False ' Kollision gefunden!
                    End If
                End If
            End If
        Next

        Return True ' Keine Kollisionen gefunden
    End Function

    ''' <summary>
    ''' Zentraler Ereignishandler, der bei Textänderungen der Steuer-Textboxen den Kalender automatisch neu zeichnet.
    ''' </summary>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: Veraltetes 'Call'-Schlüsselwort entfernt und XML-Kommentare hinzugefügt.
    ''' </remarks>
    Private Sub KalenderSteuerung_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        tbD.TextChanged, tbAbreise.TextChanged, tbAnreise.TextChanged, tbAnAb.TextChanged, tbBuchung.TextChanged, tbSperr.TextChanged

        ' Ruft die Kalender-Zeichenlogik mit den aktuellen Werten auf
        prKalender(d, dAn, dAb, aBuchung, aSperr, bAB)
    End Sub

    ''' <summary>
    ''' Verarbeitet das Klick-Ereignis der Schließen-Schaltfläche (buExet) und blendet das Steuerelement aus.
    ''' </summary>
    ''' <remarks>
    ''' 12.10.2026 - Optimiert: XML-Kommentare hinzugefügt.
    ''' </remarks>
    Private Sub buExet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buExet.Click
        Me.Visible = False ' "Me" ist im Kontext eines UserControls oder Formulars sauberer als "MyClass"
    End Sub


End Class
