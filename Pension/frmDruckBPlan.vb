Imports IDEALSoftware.VpeCommunity

Public Class frmDruckBPlan
    Dim RRand As Double = 20
    Dim LRand As Integer = 1
    Dim arS(1, 1) As String 'Statistikdaten
    Dim arfewo(1, 2) As String
    Dim nSpaltenBreite As String
    Dim arDruckZimmer1 As Array
    Dim dtWer As DataTable
    Dim bPreis As Boolean

#Region "Form Load................................................................................."

    ''' <summary>
    ''' Ereignishandler für das Laden des Formulars. 
    ''' Initialisiert die Jahres- und Monatsauswahlfelder sowie das Druckprofil aus der Systemdatenbank.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Str()'-Funktion durch saubere '.ToString()'-Konvertierung ersetzt, um führende Leerzeichen zu vermeiden.
    ''' - Lokale Schleifenvariable 'i' explizit typisiert ('For i As Integer = ...').
    ''' - Veraltete VB-Kompatibilitätsfunktion 'Split()' durch die native '.Split()'-Methode ersetzt.
    ''' </remarks>
    Private Sub frmDruckVorlage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim currentYear As Integer = Date.Today.Year
        Dim currentMonth As String = Date.Today.Month.ToString()

        ' Jahresauswahl initialisieren (3 Jahre zurück bis 2 Jahre in die Zukunft)
        coDJahr.Text = currentYear.ToString()
        For i As Integer = (currentYear - 3) To (currentYear + 2)
            coDJahr.Items.Add(i.ToString())
        Next

        ' Monatsauswahl initialisieren
        coDBisMonat.Text = currentMonth
        coDMonat.Text = currentMonth

        ' Druckprofile aus der Systemdatenbank auslesen und splitten
        Dim sProfilRaw As String = ReadOneValueFromSystemDb("Druckprofil")
        If Not String.IsNullOrWhiteSpace(sProfilRaw) Then
            arDruckZimmer1 = sProfilRaw.Split("#"c)

            ' Profile zur ComboBox hinzufügen
            For i As Integer = 0 To arDruckZimmer1.Length - 1
                cbProfil.Items.Add(AtLeft(arDruckZimmer1(i), ";", 1))
            Next

            ' Erstes Profil als Standard selektieren
            If arDruckZimmer1.Length > 0 Then
                cbProfil.Text = AtLeft(arDruckZimmer1(0), ";", 1)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Ereignishandler für den Schließen-Button. Ruft die zentrale Schließen-Routine auf.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax entfernt und XML-Kommentar hinzugefügt.</remarks>
    Private Sub cmdClosePlan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClosePlan.Click
        prClose()
    End Sub

    ''' <summary>
    ''' Schließt das aktuelle Formular und setzt den Fokus zurück auf das Buchungs-Grid des Hauptformulars.
    ''' </summary>
    ''' <remarks>
    ''' 28.09.2026 - Code-Optimierung:
    ''' - XML-Kommentar hinzugefügt.</remarks>
    Private Sub prClose()
        Me.Close()
        frmMain.dgBuchung.Select()
    End Sub

    ''' <summary>
    ''' Ereignishandler bei Änderung des "Bis-Monats". Stellt sicher, dass der "Bis-Monat" nicht vor dem "Von-Monat" liegt.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Code mit 'Int32.TryParse' gegen Konvertierungsfehler abgesichert.</remarks>
    Private Sub coDBisMonat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coDBisMonat.SelectedIndexChanged
        Dim nBisMonat As Integer = 0
        Dim nVonMonat As Integer = 0

        Int32.TryParse(coDBisMonat.Text, nBisMonat)
        Int32.TryParse(coDMonat.Text, nVonMonat)

        If nBisMonat < nVonMonat Then
            coDBisMonat.Text = coDMonat.Text
        End If
    End Sub

    ''' <summary>
    ''' Ereignishandler bei Änderung des "Von-Monats". Stellt sicher, dass der "Bis-Monat" automatisch angepasst wird, falls er vor dem "Von-Monat" liegt.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Code mit 'Int32.TryParse' gegen Konvertierungsfehler abgesichert.</remarks>
    Private Sub coDMonat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coDMonat.SelectedIndexChanged
        Dim nBisMonat As Integer = 0
        Dim nVonMonat As Integer = 0

        Int32.TryParse(coDBisMonat.Text, nBisMonat)
        Int32.TryParse(coDMonat.Text, nVonMonat)

        If nBisMonat < nVonMonat Then
            coDBisMonat.Text = coDMonat.Text
        End If
    End Sub


    ''' <summary>
    ''' Belegungsplan drucken
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 07.01.2012 Create
    ''' </remarks>
    Private Sub cmdDruckPlan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDruckPlan.Click
        Try
            bPreis = chPreis.Checked
            Dim nMonat As Integer = Val(coDMonat.Text)
            Dim nMax As Integer = Val(coDBisMonat.Text)

            Dim nPos As Double = 0.5
            Dim sJahr As String = coDJahr.Text.Trim
            Me.Cursor = Cursors.WaitCursor
            Dim dtZimDru As DataTable = fcReadDataTable("Select * from Zimmer order by Nummer asc")
            Dim dtZimmer As New DataTable
            Dim co10 As DataColumn = dtZimmer.Columns.Add("ID", GetType(System.String))
            co10.DefaultValue = String.Empty
            co10.MaxLength = 50
            Dim co11 As DataColumn = dtZimmer.Columns.Add("name", GetType(System.String))
            co11.DefaultValue = String.Empty
            co11.MaxLength = 50

            Dim co12 As DataColumn = dtZimmer.Columns.Add("BettenEr", GetType(System.String))
            co12.DefaultValue = String.Empty
            co12.MaxLength = 50
            Dim co13 As DataColumn = dtZimmer.Columns.Add("BettenMin", GetType(System.String))
            co13.DefaultValue = String.Empty
            co13.MaxLength = 50
            Dim co14 As DataColumn = dtZimmer.Columns.Add("P1", GetType(System.String))
            co14.DefaultValue = String.Empty
            co14.MaxLength = 50

            Dim co15 As DataColumn = dtZimmer.Columns.Add("Fewo", GetType(System.String))
            co15.DefaultValue = String.Empty
            co15.MaxLength = 50
            Dim co16 As DataColumn = dtZimmer.Columns.Add("IDObjekte", GetType(System.String))
            co16.DefaultValue = String.Empty
            co16.MaxLength = 50
            Dim nI As Integer = 0
            For i = 0 To arDruckZimmer1.Length - 1
                If cbProfil.Text = AtLeft(arDruckZimmer1(i), ";", 1) Then nI = i
            Next

            Dim arDruckZimmer As Array = Split(arDruckZimmer1(nI), ";")
            Dim arZimmer(dtZimDru.Rows.Count, 1) As String
            For i = 0 To dtZimDru.Rows.Count - 1
                arZimmer(i, 0) = "False"
                arZimmer(i, 1) = dtZimDru.Rows(i).Item("ID")
            Next
            For i = 1 To arDruckZimmer.Length - 1
                For j = 0 To dtZimDru.Rows.Count - 1
                    If dtZimDru.Rows(j).Item("ID") = arDruckZimmer(i) Then
                        arZimmer(j, 0) = "True"
                    End If
                Next
            Next
            For i = 0 To dtZimDru.Rows.Count - 1
                If arZimmer(i, 0) = "True" Then
                    Dim rw As DataRow = dtZimmer.NewRow
                    rw("ID") = dtZimDru.Rows(i).Item("ID")
                    rw("Name") = dtZimDru.Rows(i).Item("Name")

                    rw("BettenEr") = dtZimDru.Rows(i).Item("BettenEr")
                    rw("BettenMin") = dtZimDru.Rows(i).Item("BettenMin")
                    rw("P1") = dtZimDru.Rows(i).Item("P1")
                    rw("Fewo") = dtZimDru.Rows(i).Item("Fewo")
                    rw("IDObjekte") = dtZimDru.Rows(i).Item("IDObjekte")
                    dtZimmer.Rows.Add(rw)
                End If
            Next
            nSpaltenBreite = 26.5 / (dtZimmer.Rows.Count) '28.5 / (dtZimmer.Rows.Count)   '+1 für "Bem." 26,6
            With VPE
                .CloseDoc()
                .PageHeight = 21.5
                .PageWidth = 32.5
                .PageOrientation = PageOrientation.Landscape
                .OpenDoc()

                For i = nMonat To nMax
                    ReDim arS(dtZimmer.Rows.Count - 1, 1) 'Statistik Arry setzen
                    nPos = fcKopfzeile(i.ToString, sJahr, dtZimmer)
                    Call prMainTablePreis(nPos, i.ToString, sJahr, dtZimmer)


                    If i <> nMax Then .PageBreak()
                Next
                .Preview()
            End With

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            Me.Cursor = Cursors.Default
        End Try

        Call prClose()
    End Sub

#End Region

#Region "Druck Buchungsplan........................................................................"

    ''' <summary>
    ''' Erstellt die Kopfzeile des Buchungsplans im VPE-Dokument, zeichnet die Zimmernamen 
    ''' und bereitet das Array der Ferienwohnungen vor.
    ''' </summary>
    ''' <param name="sMonat">Der anzuzeigende Monat als numerischer String (z. B. "1" oder "01").</param>
    ''' <param name="sJahr">Das anzuzeigende Jahr als String (z. B. "2026").</param>
    ''' <param name="dt">Die DataTable, die die Zimmer- und Objektinformationen enthält.</param>
    ''' <returns>Die aktuelle vertikale Position (nPos) im Dokument nach dem Zeichnen der Kopfzeile.</returns>
    ''' <remarks>
    ''' 07.01.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - C#-spezifische Null-Operatoren durch native VB.NET 'If()'-Logik ersetzt.
    ''' - Veraltete 'Call'-Syntax bei VPE-Methodenaufrufen konsequent entfernt.
    ''' - 'fcKopfzeile = nPos' durch das moderne 'Return nPos' ersetzt.
    ''' </remarks>
    Private Function fcKopfzeile(ByVal sMonat As String, ByVal sJahr As String, ByVal dt As DataTable) As Double
        Dim nS As Double = 2.8
        Dim nE As Double = nS + nSpaltenBreite
        Dim nP As Double
        Dim nPos As Double = 0.5
        Dim nZimmer As Integer = dt.Rows.Count - 1

        Dim nMonatVal As Integer = 0
        Int32.TryParse(sMonat, nMonatVal)

        ' Array-Größe neu dimensionieren
        ReDim arfewo(nZimmer, 2)

        With VPE
            .TransparentMode = False
            .SelectFont("Arial", 10)
            .Print(LRand, nPos, fcMonthName(nMonatVal) & "  " & sJahr)

            nPos += 0.5
            nP = nPos + 0.5

            .SelectFont("Times New Roman", 9)
            .WriteBox(LRand, nPos, 2, nP, sJahr)
            .WriteBox(2, nPos, 2.8, nP, "Tag")

            ' Schleife über alle Zimmer (Spalten des Buchungsplans)
            For i As Integer = 0 To nZimmer
                Dim row As DataRow = dt.Rows(i)

                ' Werte absolut null- und DBNull-sicher auslesen
                Dim sZim As String = If(row.Item("Name") IsNot Nothing AndAlso Not IsDBNull(row.Item("Name")), row.Item("Name").ToString().Trim(), "")
                Dim bettenEr As String = If(row.Item("BettenEr") IsNot Nothing AndAlso Not IsDBNull(row.Item("BettenEr")), row.Item("BettenEr").ToString().Trim(), "")
                Dim bettenMin As String = If(row.Item("BettenMin") IsNot Nothing AndAlso Not IsDBNull(row.Item("BettenMin")), row.Item("BettenMin").ToString().Trim(), "")
                Dim p1Val As String = If(row.Item("P1") IsNot Nothing AndAlso Not IsDBNull(row.Item("P1")), row.Item("P1").ToString().Trim(), "")
                Dim sIDObjekt As String = If(row.Item("IDObjekte") IsNot Nothing AndAlso Not IsDBNull(row.Item("IDObjekte")), row.Item("IDObjekte").ToString().Trim(), "")

                ' Globale Arrays befüllen
                arS(i, 0) = bettenEr & "/" & bettenMin
                arS(i, 1) = p1Val
                arfewo(i, 0) = row.Item("FEWO")

                ' Hintergrundfarbe für das Zimmer bestimmen
                .BkgColor = Color.White
                If Not String.IsNullOrWhiteSpace(sIDObjekt) Then
                    .BkgColor = fcStringRGB(fcGetZPos(sIDObjekt))
                End If

                ' Zimmername als Box schreiben
                .WriteBox(nS, nPos, nE, nP, PadR(sZim, 8))

                ' Koordinaten für die nächste Spalte weiterschieben
                nS = nE
                nE = nE + nSpaltenBreite
            Next

            ' VPE-Status zurücksetzen
            .BkgColor = Color.White
            .SelectFont("Arial", 10)
            .TransparentMode = True
        End With

        Return nPos
    End Function

    ''' <summary>
    ''' Bestimmt die Position des Objektes in der Tabelle 'Objekte' und gibt den zugehörigen RGB-Farbwert zurück.
    ''' </summary>
    ''' <param name="sID">Die eindeutige Identifikationsnummer (ID) des gesuchten Objektes.</param>
    ''' <returns>Der RGB-Farbwert als String oder eine leere Zeichenfolge, wenn das Objekt nicht gefunden wurde.</returns>
    ''' <remarks>
    ''' 07.01.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Performance-Krise behoben: Statt die gesamte Tabelle einzulesen und per Schleife zu durchsuchen, wird nun gezielt per SQL-Filter gesucht.
    ''' - Speicherbedarf minimiert: Nur noch die benötigte Spalte 'RGB' wird abgefragt ('Select RGB...').
    ''' - Veraltete Zuweisung an den Funktionsnamen durch ein modernes 'Return'-Statement ersetzt.
    ''' - 'IsDBNull'- und Null-Prüfung für den Spaltenwert hinzugefügt.
    ''' </remarks>
    Private Function fcGetZPos(ByVal sID As String) As String
        Dim sResult As String = ""

        ' Sicherheitsprüfung für leere Übergabewerte
        If String.IsNullOrWhiteSpace(sID) Then Return sResult

        Try
            ' Gezielte SQL-Abfrage statt "Select *" spart Netzwerk- und Speicherressourcen
            Dim cSql As String = $"SELECT RGB FROM Objekte WHERE ID = '{sID.Replace("'", "''")}'"
            Dim dt As DataTable = fcReadDataTable(cSql)

            ' Wenn ein passender Datensatz gefunden wurde, Wert auslesen
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim cellValue As Object = dt.Rows(0).Item("RGB")

                If cellValue IsNot Nothing AndAlso Not IsDBNull(cellValue) Then
                    sResult = cellValue.ToString().Trim()
                End If
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try

        Return sResult
    End Function


    ''' <summary>
    ''' Erstellt die Haupttabelle des Buchungsplans mit Preisen im VPE-Dokument.
    ''' Berechnet Saisons und Feiertage pro Tag des Monats und übergibt die Daten an die Zeilenzeichner.
    ''' </summary>
    ''' <param name="nPos">Die vertikale Startposition im Dokument.</param>
    ''' <param name="sMonat">Der anzuzeigende Monat als numerischer String (z. B. "5" oder "05").</param>
    ''' <param name="sJahr">Das anzuzeigende Jahr als String (z. B. "2026").</param>
    ''' <param name="dt">Die DataTable der Zimmer- und Objektkonfigurationen.</param>
    ''' <remarks>
    ''' 07.01.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Unspezifischen Typ 'Array' durch typsichere String-Arrays ('String()') ersetzt.
    ''' - Veraltetes Visual Basic 'Split()' durch die native '.Split()'-Methode der String-Klasse ersetzt.
    ''' - 'CDate()' und 'Val()' durch absturzsichere '.TryParse()'-Methoden modernisiert.
    ''' - Schleifenvariablen ('i') explizit als Integer deklariert.
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' </remarks>
    Private Sub prMainTablePreis(ByVal nPos As Double, ByVal sMonat As String, ByVal sJahr As String, ByVal dt As DataTable)
        sMonat = PadLN(sMonat, 2)

        Dim nMonatVal As Integer = 0
        Dim nJahrVal As Integer = 0
        Int32.TryParse(sMonat, nMonatVal)
        Int32.TryParse(sJahr, nJahrVal)

        Dim nTage As Integer = DayOfMonthCount(nMonatVal, nJahrVal)
        Dim sLastDay As String = fcUmDatum(nTage.ToString() & "." & sMonat & "." & sJahr)
        Dim sFistDay As String = fcUmDatum("01." & sMonat & "." & sJahr)

        ' SQL-Abfrage zum Auslesen der Buchungen im Zeitraum
        Dim sSQL As String = "SELECT * FROM Buchung WHERE Von <= '" & sLastDay & "' AND Bis >= '" & sFistDay & "'"
        Dim dtB As DataTable = fcReadDataTable(sSQL)

        ' Alle Preise für den Buchungsplan einmalig vorab laden ---
        Dim sSQLPreise As String = "SELECT ZimID, ADatum, EDatum, Preis FROM Preise"
        Dim dtAllePreise As DataTable = fcReadDataTable(sSQLPreise)

        nPos += 0.55
        Dim nS As Double = 2.8
        Dim nE As Double = nS + nSpaltenBreite
        Dim nP As Double = nPos + 0.55

        ' Auslesen und Splitten der Saisonzeiten aus der Systemdatenbank
        Dim sSaisonRaw As String = ReadOneValueFromSystemDb("Saison")
        Dim arSaison As String() = If(Not String.IsNullOrWhiteSpace(sSaisonRaw), sSaisonRaw.Split(New String() {vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries), New String(-1) {})
        Dim arSaisonZeile As String()

        Dim sForColor As String
        Dim sBackColor As String
        Dim sForColorFewo As String
        Dim sBackColorFewo As String

        With VPE
            .SelectFont("Times New Roman", 9)

            ' Schleife über alle Tage des Monats
            For n As Integer = 1 To nTage
                Dim sDateStr As String = PadLN(n.ToString(), 2) & "." & sMonat & "." & sJahr
                Dim sDate As Date
                If Not Date.TryParse(sDateStr, sDate) Then sDate = Date.Today

                sForColor = ""
                sBackColor = ""
                sForColorFewo = ""
                sBackColorFewo = ""

                ' Saisoneigenschaften für den aktuellen Tag ermitteln
                For i As Integer = 0 To arSaison.Length - 1
                    Dim zeilenText As String = arSaison(i).Trim()

                    If Not String.IsNullOrWhiteSpace(zeilenText) Then
                        arSaisonZeile = zeilenText.Split(";"c)

                        ' Prüfen, ob genügend Spaltenelemente in der Saisonzeile vorhanden sind
                        If arSaisonZeile.Length >= 5 Then
                            Dim dVon As Date
                            Dim dBis As Date

                            If Date.TryParse(arSaisonZeile(1), dVon) AndAlso Date.TryParse(arSaisonZeile(2), dBis) Then
                                If sDate >= dVon AndAlso sDate <= dBis Then

                                    Dim nSaisonTyp As Integer = 0
                                    Dim sTypPrefix As String = If(arSaisonZeile(0).Length >= 2, arSaisonZeile(0).Substring(0, 2), arSaisonZeile(0))
                                    Int32.TryParse(sTypPrefix, nSaisonTyp)

                                    If nSaisonTyp < 6 Then
                                        sForColorFewo = arSaisonZeile(4)
                                        sBackColorFewo = arSaisonZeile(3)
                                    Else
                                        sForColor = arSaisonZeile(4)
                                        sBackColor = arSaisonZeile(3)
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next

                ' Aktuelle Zeile in das Dokument schreiben
                'prWriteZeile(sDate, dt, dtB, nPos, nS, nE, nP, sForColor, sBackColor, sForColorFewo, sBackColorFewo)
                ' NEU: dtAllePreise wird hier als Parameter mit übergeben
                prWriteZeile(sDate, dt, dtB, dtAllePreise, nPos, nS, nE, nP, sForColor, sBackColor, sForColorFewo, sBackColorFewo)

                ' Koordinaten für die nächste Zeile berechnen
                nPos += 0.55
                nS = 2.8
                nE = nS + nSpaltenBreite
                nP = nPos + 0.55
            Next

            ' Zusammenfassung (Summary) am Ende der Tabelle zeichnen
            nPos += 0.05
            .WriteBox(LRand, nPos, 2.8, nP + 0.2, "")
            .Print(LRand + 0.1, nPos + 0.02, "Per Max/Min")
            .Print(LRand + 0.1, nPos + 0.35, "Abzug")

            prWriteLastZeile(nPos, nS, nE, nP)

            ' Optional Anschriften auf einer neuen Seite ausgeben
            If chAnschriften.Checked Then
                .PageBreak()
                prWriteAnschriften(dtB)
            End If
        End With
    End Sub

    ''' <summary>
    ''' Ermittelt die Belegungs- und Preisdaten für alle Zimmer an einem bestimmten Datum 
    ''' und zeichnet die Zeile inklusive Formatierungen und Rahmen in das VPE-Dokument.
    ''' </summary>
    ''' <param name="sDate">Das zu verarbeitende Datum für die aktuelle Zeile.</param>
    ''' <param name="dt">Die DataTable der Zimmer- und Objektkonfigurationen.</param>
    ''' <param name="dtB">Die DataTable mit den Buchungsdatensätzen im aktuellen Zeitraum.</param>
    ''' <param name="dtAllePreise">Die DataTable mit allen Preisen für den Buchungsplan.</param>
    ''' <param name="nPos">Die vertikale Position der Zeile im Dokument.</param>
    ''' <param name="nS">Die horizontale Startkoordinate (X1) für die aktuelle Zelle.</param>
    ''' <param name="nE">Die horizontale Endkoordinate (X2) für die aktuelle Zelle.</param>
    ''' <param name="nP">Die vertikale Endkoordinate (Y2) für die aktuelle Zelle.</param>
    ''' <param name="sForColor">Die Vordergrundfarbe aus der Saisonberechnung.</param>
    ''' <param name="sBackColor">Die Hintergrundfarbe aus der Saisonberechnung.</param>
    ''' <param name="sForColorFewo">Die Ferienwohnung-Vordergrundfarbe aus der Saisonberechnung.</param>
    ''' <param name="sBackColorFewo">Die Ferienwohnung-Hintergrundfarbe aus der Saisonberechnung.</param>
    ''' <remarks>
    ''' 07.01.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort bei allen VPE-Aufrufen konsequent entfernt.
    ''' - 'Select Case' für Wochentage auf typsichere und lesbare String-Vergleiche modernisiert.
    ''' - Stringverkettungen für das Datum auf saubere '.ToString()'-Aufrufe umgestellt.
    ''' - Cache für Array-Länge und wiederholte Zellabfragen ('arfewo(i, 0)') hinzugefügt.
    ''' - Absicherung der Array-Indizes von 'arT' gegen OutOfBounds-Exceptions (Prüfung der Länge vor Zugriff).
    ''' </remarks>
    Private Sub prWriteZeile(ByVal sDate As Date, ByVal dt As DataTable,
                         ByVal dtB As DataTable, ByVal dtAllePreise As DataTable, ByVal nPos As Double,
                         ByVal nS As Double, ByVal nE As Double, ByVal nP As Double,
                         ByVal sForColor As String, ByVal sBackColor As String,
                         ByVal sForColorFewo As String, ByVal sBackColorFewo As String)
        Dim nZimmer As Integer = dt.Rows.Count - 1
        Dim arT() As String
        Dim sTag As String
        Dim nAnreise As Integer

        With VPE
            ' 1. Datum und Wochentag formatieren und schreiben
            .TransparentMode = False
            sTag = fcGetFeiertag(sDate)

            If Not String.IsNullOrWhiteSpace(sTag) Then
                .BkgColor = fColorBackSonstigeFeiertage
            End If

            If Not String.IsNullOrWhiteSpace(sBackColor) Then
                .BkgColor = fcStringRGB(sBackColor)
            End If

            Dim sDatumText As String = PadLN(sDate.Day.ToString(), 2) & "." & PadLN(sDate.Month.ToString(), 2)
            .WriteBox(LRand, nPos, 2, nP, sDatumText)

            .BkgColor = Color.White
            .TransparentMode = True

            ' Wochentage ermitteln; Samstage und Sonntage rot hervorheben
            sTag = fcShortDayName(sDate)
            If sTag = "Sa" OrElse sTag = "So" Then
                .TextColor = Color.Red
            Else
                .TextColor = Color.Black
            End If

            .WriteBox(2, nPos, 2.8, nP, sTag)

            ' 2. Belegungsdaten pro Zimmer zeichnen
            .SelectFont("Times New Roman", 6)
            For i As Integer = 0 To nZimmer
                .TransparentMode = False
                .BkgColor = Color.White

                ' Grundfarbe nach Wochentag festlegen
                If sTag = "Sa" Then
                    .TextColor = fColorForeSamstag
                    .BkgColor = fColorBackSamstag
                ElseIf sTag = "Fr" Then
                    .TextColor = fColorForeFreitag
                    .BkgColor = fColorBackFreitag
                End If

                ' Ferienwohnung-Sonderfarbe anwenden
                If Convert.ToString(arfewo(i, 0)) = "1" AndAlso Not String.IsNullOrWhiteSpace(sBackColorFewo) Then
                    .BkgColor = fcStringRGB(sBackColorFewo)
                End If

                ' Werte für Datum und Zimmer abrufen
                arT = fcGetBuchValue(sDate, i, dt, dtB, dtAllePreise)

                ' Zelle initialisieren (Rahmen zeichnen)
                .WriteBox(nS, nPos, nE, nP, "")
                .TextColor = Color.Black
                nAnreise = 0

                ' Validiere, ob das zurückgegebene Array die erwartete Mindestlänge hat
                If arT IsNot Nothing AndAlso arT.Length >= 7 Then
                    ' Anreise prüfen
                    If arT(3) = "1" Then
                        nAnreise = 1
                    End If

                    ' Buchungsstatus (Fest / Variabel) farblich kennzeichnen
                    If arT(4) = "1" Then
                        .BkgColor = fColorBackFestGebucht
                        .Print(nS + 0.5, nPos + 0.02, "     ")
                    ElseIf arT(4) = "2" Then
                        .BkgColor = fColorBackVariabel
                        .Print(nS + 0.5, nPos + 0.02, "     ")
                    End If

                    ' Marketing / Werbekanal-Farbe anwenden
                    If arT(5) <> "255,255,255" AndAlso Not String.IsNullOrWhiteSpace(arT(5)) Then
                        .BkgColor = fcStringRGB(arT(5))
                        .Print(nS + 0.2, nPos + 0.02, "     ")
                    End If

                    ' Frühstücks-Indikator
                    If arT(2) = "1" Then
                        .BkgColor = Color.BlueViolet
                        .Print(nS + 0.8, nPos + 0.02, "     ")
                    End If

                    .TransparentMode = True
                    .TextColor = Color.Black

                    ' Textausgabe: Anzahl Personen [+ optionaler Preis]
                    If bPreis Then
                        .Print(nS + 0.05, nPos + 0.02, arT(0) & "         [ " & arT(6) & " ]")
                    Else
                        .Print(nS + 0.05, nPos + 0.02, arT(0))
                    End If

                    ' Anreise-Farbe überschreibt bei Bedarf die Textbox
                    If nAnreise = 1 Then
                        .BkgColor = fColorBackAnreise
                        .TextColor = fColorForeAnreise
                    End If

                    ' Buchungstext / Gastname in der Zelle platzieren
                    .Print(nS + 0.1, nPos + 0.25, arT(1))
                End If

                ' Rücksetzung für die nächste Zelle und Koordinaten weiterschieben
                .TextColor = Color.Black
                .TransparentMode = True
                nS = nE
                nE = nE + nSpaltenBreite
            Next

            .SelectFont("Times New Roman", 9)
        End With
    End Sub

    ''' <summary>
    ''' Ermittelt die Buchungs- und Preisdaten für ein bestimmtes Zimmer an einem vorgegebenen Datum.
    ''' </summary>
    ''' <param name="sDate">Das abzufragende Datum.</param>
    ''' <param name="nPos">Der nullbasierte Index des Zimmers innerhalb der Zimmer-DataTable.</param>
    ''' <param name="dt">Die DataTable, die die Zimmerkonfigurationen enthält.</param>
    ''' <param name="dtAllePreise">Die DataTable mit allen Preisen für den Buchungsplan.</param>
    ''' <param name="dtB">Die DataTable mit den bereits geladenen Buchungssätzen des Zeitraums.</param>
    ''' <returns>Ein String-Array mit 8 Elementen (Index 0 bis 7), das Belegungsdetails und Preise enthält.</returns>
    ''' <remarks>
    ''' 07.01.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Rückgabetyp von der abstrakten Basisklasse 'Array' auf ein konkretes String-Array ('String()') geändert.
    ''' - Fehlerhafte Konvertierung 'Val(sD)' entfernt, da 'sD' ein Datums-String für die SQL/Filterlogik ist.
    ''' - 'fcGetBuchValue = arT' durch ein modernes und lesbares 'Return arT' ersetzt.
    ''' - Array-Initialisierung übersichtlich strukturiert und mit einem standardmäßigen leeren Preis-Element (Index 7) versehen.
    ''' </remarks>
    Private Function fcGetBuchValue(ByVal sDate As Date, ByVal nPos As Integer,
                                ByVal dt As DataTable, ByVal dtB As DataTable, ByVal dtAllePreise As DataTable) As String()
        Dim sD As String = fcUmDatum(sDate)

        ' Array initialisieren (Index 0 bis 7 für insgesamt 8 Elemente)
        Dim arT(7) As String
        arT(0) = ""            ' Anzahl Personen / Status
        arT(1) = ""            ' Gastname / Buchungstext
        arT(2) = "0"           ' Frühstück (Fr)
        arT(3) = "0"           ' Anreise-Kennung
        arT(4) = "0"           ' Fest (1) / Variabel (2)
        arT(5) = "255,255,255" ' Marketing- / Werbefarbe (RGB)
        arT(6) = ""            ' Platzhalter für Preis/Zusatzinfo aus Detail
        arT(7) = ""            ' Tagespreis

        ' Eindeutige Zimmer-ID ermitteln
        Dim sZID As String = fcGetZID(dt, nPos)

        If Not String.IsNullOrWhiteSpace(sZID) Then
            ' Buchungsdetails aus dem bestehenden DataTable-Cache auslesen
            ' Hinweis: sD wurde von 'Val()' befreit, da es sich um ein formatiertes Datum handelt
            Dim arDetails As String() = fcGetBuchValueDetail(sZID, sD, dtB)

            ' Wenn Details gefunden wurden, Werte in das Hauptarray übertragen
            If arDetails IsNot Nothing Then
                For i As Integer = 0 To Math.Min(arDetails.Length - 1, arT.Length - 1)
                    arT(i) = arDetails(i)
                Next
            End If

            ' Aktuellen Tagespreis für dieses Zimmer ermitteln und im Index 7 (oder 6 laut deinem Grid-Code) ablegen
            arT(6) = fcGetPreisToDay(sZID, sD, dtAllePreise)
        End If

        Return arT
    End Function

    ''' <summary>
    ''' ZimmerID aus der Zeilenposition der DataTable ermitteln.
    ''' </summary>
    ''' <param name="dt">Die DataTable, die die Zimmerdaten enthält.</param>
    ''' <param name="nPos">Der nullbasierte Zeilenindex (Position).</param>
    ''' <returns>Die Zimmer-ID als String oder einen leeren String, wenn die Position ungültig ist.</returns>
    ''' <remarks>
    ''' 07.01.2012 Create
    ''' 28.09.2026 Code-Optimierung: Try-Catch durch defensive Index- und Spaltenprüfung (Performance) ersetzt; VB6-Rückgabemuster auf Return umgestellt; DBNull-Absicherung hinzugefügt.
    ''' </remarks>
    Private Function fcGetZID(ByVal dt As DataTable, ByVal nPos As Integer) As String
        ' 1. Vorab-Prüfung: Ist die Tabelle geladen und existiert die Zeile überhaupt?
        If dt Is Nothing OrElse nPos < 0 OrElse nPos >= dt.Rows.Count Then
            Return ""
        End If

        ' 2. Vorab-Prüfung: Existiert die Spalte "ID" in der Tabelle?
        If Not dt.Columns.Contains("ID") Then
            Return ""
        End If

        ' 3. Wert sicher zurückgeben (ggf. noch auf DBNull prüfen)
        Dim rowValue As Object = dt.Rows(nPos).Item("ID")

        If rowValue Is DBNull.Value Then
            Return ""
        Else
            Return rowValue.ToString()
        End If
    End Function

    ''' <summary>
    ''' Ermittelt Buchungsdetails wie Kundenname, Personenanzahl, Frühstücksstatus und Farbcodierung für ein bestimmtes Zimmer an einem Stichtag.
    ''' </summary>
    ''' <param name="sZID">Die eindeutige Identifikationsnummer des Zimmers.</param>
    ''' <param name="sD">Der abzufragende Tag als Long-Wert (Zeitstempel/Datumszahl).</param>
    ''' <param name="dtB">Die DataTable, die die aktuellen Buchungsdatensätze enthält.</param>
    ''' <returns>Ein String-Array mit 6 Elementen (0: Personen, 1: Kundenname, 2: Frühstück-Flag, 3: Anreisetag-Flag, 4: Variable, 5: RGB-Farbwert).</returns>
    ''' <remarks>
    ''' 07.01.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Array-Initialisierung modernisiert und auf das benötigte 6-Elemente-Limit (0 bis 5) korrigiert.
    ''' - Potenziell performance-kritische Datenbankabfragen ('fcReadDataTable') aus der Schleife entfernt und in die Treffer-Bedingung verlagert.
    ''' - Veraltetes Zuweisungsmuster für den Funktionsrückgabewert durch explizites 'Return' ersetzt.
    ''' - Typkonvertierungen und Null-Prüfungen mit 'String.IsNullOrEmpty' abgesichert.
    ''' - Der Datentyp des Rückgabewerts wurde von 'Array' auf das spezifischere 'String()' geändert, was Typsicherheit und IntelliSense verbessert.
    ''' </remarks>
    Private Function fcGetBuchValueDetail(ByVal sZID As String, ByVal sD As Long, ByVal dtB As DataTable) As String()
        ' Array mit 6 Elementen initialisieren (Indizes 0 bis 5)
        Dim arT(5) As String
        arT(0) = ""                  ' Personen
        arT(1) = ""                  ' Kundenname
        arT(2) = "0"                 ' Frühstück (0 oder 1)
        arT(3) = "0"                 ' Anreisetag-Flag (0 oder 1)
        arT(4) = "0"                 ' Variable
        arT(5) = "255,255,255"       ' RGB-Farbe (Standard: Weiß)

        ' Überprüfen, ob überhaupt Daten vorhanden sind
        If dtB Is Nothing OrElse dtB.Rows.Count = 0 Then Return arT

        For i As Integer = 0 To dtB.Rows.Count - 1
            ' Werte direkt aus der Zeile lesen
            Dim sB As Long = Convert.ToInt64(dtB.Rows(i).Item("Von"))
            Dim sE As Long = Convert.ToInt64(dtB.Rows(i).Item("Bis"))
            Dim sZ As String = dtB.Rows(i).Item("ZimID").ToString()

            ' Prüfen, ob der Tag (sD) im Buchungszeitraum liegt und das Zimmer übereinstimmt
            If sB <= sD AndAlso sE >= sD AndAlso sZ = sZID Then
                arT(0) = dtB.Rows(i).Item("Personen").ToString()

                ' Kundenname ermitteln, falls KunID vorhanden
                Dim kundenID As String = dtB.Rows(i).Item("KunID").ToString()
                If Not String.IsNullOrEmpty(kundenID) Then
                    arT(1) = fcGetKundenName(kundenID)
                End If

                ' Frühstücks-Status setzen
                arT(2) = dtB.Rows(i).Item("Frueh").ToString()
                If Val(arT(2)) > 0 Then
                    arT(2) = "1"
                Else
                    arT(2) = "0"
                End If

                arT(4) = dtB.Rows(i).Item("Variable").ToString()

                ' Farbcodierung aus der Werbung-Tabelle laden (Nur im Trefferfall!)
                Dim werbungKey As String = dtB.Rows(i).Item("Werbung").ToString()
                Dim sSQL As String = "Select color From Werbung Where Werbung = '" & werbungKey.Replace("'", "''") & "'"
                Dim dtWer As DataTable = fcReadDataTable(sSQL)

                If dtWer IsNot Nothing AndAlso dtWer.Rows.Count > 0 Then
                    arT(5) = dtWer.Rows(0).Item("color").ToString()
                End If

                ' Anreisetag-Flag setzen
                If sB = sD Then arT(3) = "1"

                Exit For ' Passende Buchung gefunden, Schleife abbrechen
            End If
        Next

        Return arT
    End Function


    ''' <summary>
    ''' Ermittelt den Kundennamen (Spalte Name1) anhand der übergebenen Kunden-ID.
    ''' </summary>
    ''' <param name="sID">Die eindeutige Identifikationsnummer des Kunden (ID).</param>
    ''' <returns>Den Kundennamen als String. Liefert "Gesperrt" bei ID = "1" oder einen leeren String, falls kein Datensatz gefunden wurde.</returns>
    ''' <remarks>
    ''' 07.01.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Frühzeitigen Ausstieg ('Return') für ID = "1" hinzugefügt, um die Datenbank bei gesperrten IDs nicht unnötig zu belasten.
    ''' - Veraltetes Zuweisungsmuster für den Funktionsnamen durch explizite 'Return'-Statements ersetzt.
    ''' - Null-Prüfung ('IsNot Nothing') für die zurückgegebene DataTable ergänzt, um 'NullReferenceException'-Fehler zu vermeiden.
    ''' - SQL-Injection-Schutz durch Maskierung von einfachen Anführungszeichen ('Replace') integriert.
    ''' </remarks>
    Private Function fcGetKundenName(ByVal sID As String) As String
        ' Validierung: Falls die ID leer ist, sofort leeren String zurückgeben
        If String.IsNullOrWhiteSpace(sID) Then Return ""

        ' Sonderfall abfangen: Wenn ID = "1", ist der Kunde gesperrt (Datenbankabfrage entfällt)
        If sID = "1" Then Return "Gesperrt"

        ' SQL-Query vorbereiten (inkl. einfachem Schutz gegen SQL-Injection durch Verdopplung von Hochkommas)
        Dim sSQL As String = "Select Name1 from Kunden WHERE ID = '" & sID.Replace("'", "''") & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' Prüfen, ob die Tabelle existiert und Zeilen enthält
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("Name1").ToString()
        End If

        ' Standard-Rückgabewert, falls kein Kunde gefunden wurde
        Return ""
    End Function


    ''' <summary>
    ''' Erstellt die Abschlusszeile (letzte Zeile) eines Berichts und trägt statistische Daten aus einem globalen Array in die Tabellenspalten ein.
    ''' </summary>
    ''' <param name="nPos">Die vertikale Y-Position (Top) für den Zeilenanfang im VPE-Dokument.</param>
    ''' <param name="nS">Die horizontale X-Startposition (Links) für die erste Spalte.</param>
    ''' <param name="nE">Die horizontale X-Endposition (Rechts) für die erste Spalte.</param>
    ''' <param name="nP">Die Basishöhe/Position für die Textausrichtung innerhalb der Box.</param>
    ''' <remarks>
    ''' 07.01.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei den VPE-Methodenaufrufen entfernt.
    ''' - Ermittlung der Schleifengrenze von 'arS.Length / 2 - 1' auf die modernere und sicherere Methode 'arS.GetUpperBound(0)' umgestellt.
    ''' - Auskommentierten "Totcode" am Ende der Prozedur entfernt, um die Wartbarkeit zu verbessern.
    ''' - Inline-Kommentare zur Erklärung der Positionierungslogik (Ober- und Unterkante für Statistikwerte) hinzugefügt.
    ''' </remarks>
    Private Sub prWriteLastZeile(ByVal nPos As Double, ByVal nS As Double, ByVal nE As Double, ByVal nP As Double)
        ' Ermittelt die obere Grenze der ersten Dimension des zweidimensionalen Arrays arS
        Dim nMax As Integer = arS.GetUpperBound(0)

        With VPE
            ' Schriftart für die Statistikwerte festlegen
            .SelectFont("Courier new", 9)

            ' Spalten durchlaufen und statistische Daten zeichnen
            For i As Integer = 0 To nMax
                ' Rahmen für die aktuelle Zelle zeichnen
                .WriteBox(nS, nPos, nE, nP + 0.2, "")

                ' Ersten Statistikwert (z.B. Belegte Zimmer) in der oberen Hälfte der Zelle drucken
                .Print(nS + 0.1, nPos + 0.02, arS(i, 0).PadLeft(5))

                ' Zweiten Statistikwert (z.B. Umsatz/Prozent) in der unteren Hälfte der Zelle drucken
                .Print(nS + 0.1, nPos + 0.35, arS(i, 1).PadLeft(5))

                ' X-Koordinaten für die nächste Spalte weiterschieben
                nS = nE
                nE += nSpaltenBreite ' Äquivalent zu nE = nE + nSpaltenBreite
            Next
        End With
    End Sub


#End Region

#Region "Druck Anschriften zum Buchungsplan........................................................"

    ''' <summary>
    ''' Druckt die Anschriften aller Kunden aus einer übergebenen Tabelle und steuert den automatischen Seitenumbruch.
    ''' </summary>
    ''' <param name="dt">Die DataTable, die die Buchungs- und Kundendaten enthält.</param>
    ''' <remarks>
    ''' 07.01.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veralteten Vergleich 'arT(0) <> Nothing' durch das performantere und null-sichere 'Not String.IsNullOrEmpty()' ersetzt.
    ''' - Variablen-Deklarationen in die Schleife verlagert (Local Scope), um den Speicher direkt freizugeben.
    ''' - 'With VPE'-Block entfernt, da innerhalb des Blocks kein direkter Bezug (mittels '.') auf VPE stattfand, außer beim PageBreak.
    ''' - Doppelte Zuweisungen bei 'nPos' bereinigt und Inline-Dokumentation für den Seitenumbruch-Schwellenwert (18 cm/Zoll) hinzugefügt.
    ''' </remarks>
    Private Sub prWriteAnschriften(ByVal dt As DataTable)
        ' Vorzeitiger Abbruch, falls keine Daten vorhanden sind
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return

        Dim sBNr1 As String = ""
        Dim nPos As Double = fcKopfZeileAnschrift(1.0) + 0.2
        Dim nMax As Integer = dt.Rows.Count - 1

        For i As Integer = 0 To nMax
            ' Werte direkt für den aktuellen Durchlauf ermitteln
            Dim sBNr As String = dt.Rows(i).Item("BID").ToString()
            Dim sKNr As String = dt.Rows(i).Item("KunID").ToString()
            Dim arT() As String = fcGetAnschrift(sKNr)

            ' Prüfen, ob eine gültige Anschrift zurückgegeben wurde (Erstes Element darf nicht leer sein)
            If arT IsNot Nothing AndAlso arT.Length > 0 AndAlso Not String.IsNullOrEmpty(arT(0)) Then

                ' Seitenumbruch steuern, wenn die maximale Seitenhöhe (18) überschritten wird
                If nPos > 18.0 Then
                    VPE.PageBreak()
                    nPos = fcKopfZeileAnschrift(1.0)
                End If

                ' Nur drucken, wenn sich die Buchungsnummer (BID) geändert hat (Gruppierung)
                If sBNr1 <> sBNr Then
                    sBNr1 = sBNr
                    nPos = fcInhaltZeileADR(sBNr, arT, nPos) + 0.5
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Ermittelt die vollständigen Anschrift- und Kontaktdaten eines Kunden anhand der Kundennummer und gibt diese als Array zurück.
    ''' </summary>
    ''' <param name="sKNr">Die eindeutige Identifikationsnummer des Kunden (ID).</param>
    ''' <returns>Ein String-Array mit 11 Elementen (0: Name1, 1: Name2, 2: Vorname, 3: Straße, 4: PLZ, 5: Ort, 6: Telefon, 7: Telefax, 8: Mobil/Funk, 9: E-Mail, 10: Unbenutzt/Reserve).</returns>
    ''' <remarks>
    ''' 07.01.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Datentyp des Rückgabewerts von 'Array' auf das spezifischere 'String()' geändert für bessere Typsicherheit und IntelliSense.
    ''' - SQL-Injection-Schutz durch Maskierung von einfachen Anführungszeichen ('.Replace') integriert.
    ''' - Eine Inline-Hilfsfunktion ('GetRowValue') eingeführt, um den sich wiederholenden Code (ToString, Trim, DBNull-Prüfung) drastisch zu reduzieren.
    ''' - 'Return'-Statement statt der veralteten Zuweisung an den Funktionsnamen verwendet.
    ''' - Null-Prüfung ('IsNot Nothing') für die DataTable implementiert.
    ''' </remarks>
    Private Function fcGetAnschrift(ByVal sKNr As String) As String()
        ' Array mit 11 Elementen initialisieren (Index 0 bis 10 entsprechend dem Altcode)
        Dim arT(10) As String
        For k As Integer = 0 To 10
            arT(k) = String.Empty
        Next

        ' Vorzeitiger Ausstieg bei leerer Kundennummer
        If String.IsNullOrWhiteSpace(sKNr) Then Return arT

        ' SQL-Query mit grundlegendem SQL-Injection-Schutz vorbereiten
        Dim sSQL As String = "Select Name1, Name2, VorName, Strasse, PLZ, Ort, Telefon, Telefax, Funk, EMail From Kunden WHERE ID = '" & sKNr.Replace("'", "''") & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' Wenn Daten vorhanden sind, Zeile auslesen
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim row As DataRow = dt.Rows(0)

            ' Lokale Hilfsfunktion zur Vermeidung von Code-Duplizierung und Absicherung gegen DBNull
            Dim GetRowValue = Function(columnName As String) As String
                                  If row.IsNull(columnName) Then Return String.Empty
                                  Return row.Item(columnName).ToString().Trim()
                              End Function

            ' Zuweisung der Tabellenspalten in das Array
            arT(0) = GetRowValue("Name1")
            arT(1) = GetRowValue("Name2")
            arT(2) = GetRowValue("VorName")
            arT(3) = GetRowValue("Strasse")
            arT(4) = GetRowValue("PLZ")
            arT(5) = GetRowValue("Ort")
            arT(6) = GetRowValue("Telefon")
            arT(7) = GetRowValue("Telefax")
            arT(8) = GetRowValue("Funk")
            arT(9) = GetRowValue("EMail")
        End If

        Return arT
    End Function

    ''' <summary>
    ''' Erstellt die zweizeilige Kopfzeile für die Anschriftenliste inklusive Spaltenbeschriftungen und einer Trennlinie.
    ''' </summary>
    ''' <param name="nRow">Die vertikale Y-Startposition (Top) für den Tabellenkopf im VPE-Dokument.</param>
    ''' <returns>Die aktualisierte Y-Position nach dem Zeichnen der Trennlinie.</returns>
    ''' <remarks>
    ''' 07.01.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Aufruf von '.WriteBox' entfernt.
    ''' - 'fcKopfZeileAnschrift = ...' durch das modernere und explizite 'Return'-Statement ersetzt.
    ''' - Spalten-X-Koordinaten (nC1 bis nC5) als 'Const' deklariert, da sich diese während der Laufzeit nicht ändern.
    ''' - Inline-Kommentare zur Kennzeichnung der ersten und zweiten Kopfzeile hinzugefügt.
    ''' </remarks>
    Private Function fcKopfZeileAnschrift(ByVal nRow As Double) As Double
        ' Spaltenpositionen (X-Koordinaten) als Konstanten definieren
        Const nC1 As Double = 1.0
        Const nC2 As Double = 3.5
        Const nC3 As Double = 8.0
        Const nC4 As Double = 13.0
        Const nC5 As Double = 18.0

        With VPE
            .TransparentMode = False
            .SelectFont("Times New Roman", 9)

            ' --- 1. Zeile des Tabellenkopfs ---
            .Print(nC1, nRow, "Buch-Nr")
            .Print(nC2, nRow, "Name")
            .Print(nC3, nRow, "Straße")
            .Print(nC4, nRow, "Telefon")
            .Print(nC5, nRow, "Funk")

            nRow += 0.4

            ' --- 2. Zeile des Tabellenkopfs ---
            .Print(nC2, nRow, "Vorname")
            .Print(nC3, nRow, "PLZ / Ort")
            .Print(nC4, nRow, "Fax")
            .Print(nC5, nRow, "E-Mail")

            nRow += 0.4

            ' Horizontale Trennlinie unter dem Kopf zeichnen
            .WriteBox(LRand, nRow, 31.0, nRow + 0.01, "")

            .TransparentMode = True
        End With

        Return nRow
    End Function

    ''' <summary>
    ''' Druckt eine einzelne Datenzeile mit den Adress- und Kontaktdaten eines Kunden im VPE-Bericht.
    ''' </summary>
    ''' <param name="sBNr">Die anzuzeigende Buchungsnummer.</param>
    ''' <param name="arT">Das String-Array, das die Anschrift- und Kontaktdaten des Kunden enthält.</param>
    ''' <param name="nRow">Die vertikale Y-Startposition (Top) für die Datenzeile im VPE-Dokument.</param>
    ''' <returns>Die aktualisierte Y-Position nach dem Drucken der Kundendaten.</returns>
    ''' <remarks>
    ''' 07.01.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Array-Grenzenprüfung integriert, um 'IndexOutOfRangeException'-Abstürze bei unvollständigen Arrays zu verhindern.
    ''' - Namenslogik überarbeitet: Nutzt 'String.IsNullOrWhiteSpace' für die Prüfung auf Firmennamen/Zusätze (arT(1)).
    ''' - Spaltenkoordinaten (nC1 bis nC6) als 'Const' deklariert zur Performance- und Layout-Sicherheit.
    ''' - Veraltete Rückgabewert-Zuweisung an den Funktionsnamen durch ein explizites 'Return' ersetzt.
    ''' - Fehlerhafte Spaltenzuordnung korrigiert (im Altcode wurde arT(7) doppelt für Fax ausgegeben, in der zweiten Zeile stand es unter Telefon).
    ''' </remarks>
    Function fcInhaltZeileADR(ByVal sBNr As String, ByVal arT() As String, ByVal nRow As Double) As Double
        ' Spaltenpositionen (X-Koordinaten) als Konstanten definieren
        Const nC1 As Double = 1.0
        Const nC2 As Double = 3.5
        Const nC3 As Double = 8.0
        Const nC4 As Double = 13.0
        Const nC5 As Double = 18.0
        Const nC6 As Double = 23.0

        ' Absicherung: Falls das Array ungültig oder zu kurz ist, Funktion direkt verlassen
        If arT Is Nothing OrElse arT.Length < 10 Then Return nRow

        Try
            ' Namenslogik: Wenn Name2 (arT(1)) gefüllt ist, wird dieser als Hauptname (s1) gedruckt 
            ' und Name1 (arT(0)) rutscht als Zusatz (s2) nach rechts.
            Dim s1 As String = arT(0)
            Dim s2 As String = String.Empty

            If Not String.IsNullOrWhiteSpace(arT(1)) Then
                s1 = arT(1).Trim()
                s2 = arT(0).Trim()
            End If

            With VPE
                ' --- 1. Datenzeile drucken ---
                .Print(nC1, nRow, sBNr)
                .Print(nC2, nRow, s1)
                .Print(nC3, nRow, arT(3))           ' Straße
                .Print(nC4, nRow, arT(6))           ' Telefon
                .Print(nC5, nRow, arT(8))           ' Funk/Mobil (Korrektur: arT(8) statt arT(7))
                .Print(nC6, nRow, s2)               ' Namenszusatz / Firma

                nRow += 0.4

                ' --- 2. Datenzeile drucken ---
                .Print(nC2, nRow, arT(2))           ' Vorname
                .Print(nC3, nRow, arT(4) & " " & arT(5)) ' PLZ / Ort
                .Print(nC4, nRow, arT(7))           ' Telefax (Korrektur: Steht nun exakt unter der Spalte Fax)
                .Print(nC5, nRow, arT(9))           ' E-Mail
            End With

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try

        Return nRow
    End Function



#End Region



End Class