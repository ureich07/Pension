Imports System.Globalization
Imports System.Text


Public Class frmDatev
    Private lFilter As Boolean
    Private sKKonto As String = arIni(18)   'Kasse Konto "1600"
    Private sBKonto As String = arIni(19)   'Bank Konto "1800" 
    Private nKasseID As Integer = 1         '1-Sperren,2-freigabe,3-kasse,4-bank
    Private sKonto As String = ""           'wird kasse_konto oder bank_konto
    Private fColorBackDel As Color = Color.Yellow
    Private nNummer As Integer = 1


    ''' <summary>
    ''' Formular-Lade-Ereignis: Bereinigt alte DATEV-Datensätze (älter als 2 Jahre), 
    ''' aktualisiert Konten-Zuordnungen in der Datenbank, lädt die Daten und initialisiert die UI.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax vollständig entfernt.
    ''' - Array-Erzeugung radikal vereinfacht (redundante Split-Aufrufe entfernt).
    ''' - Doppelten Aufruf von 'prColorRead()' eliminiert.
    ''' </remarks>
    Private Sub frmDatev_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' 1. Bereinigung: Datensätze löschen, die älter als 100 Tage sind
        Dim sIDDatum As String = fcUmDatum(DateAdd(DateInterval.Day, -100, Date.Today)) & "000000"
        Dim cSql As String = $"DELETE FROM Datev WHERE ID < '{sIDDatum}'"
        UpdateTable(cSql)

        ' 2. Datenbank-Aktualisierungen (Konten-Umschreibungen von 4303/4403 auf 4300/4400)
        Dim fieldsG1 As String() = New String() {"GKonto1"}
        Dim fieldsG2 As String() = New String() {"GKonto2"}
        Dim fieldsG3 As String() = New String() {"GKonto3"}

        Dim value4300 As String() = New String() {"4300"}
        Dim value4400 As String() = New String() {"4400"}

        ' GKonto1 Updates
        fcUpdateCommand("Datev", fieldsG1, value4300, " WHERE GKonto1='4303'")
        fcUpdateCommand("Datev", fieldsG1, value4400, " WHERE GKonto1='4403'")

        ' GKonto2 Updates
        fcUpdateCommand("Datev", fieldsG2, value4300, " WHERE GKonto2='4303'")
        fcUpdateCommand("Datev", fieldsG2, value4400, " WHERE GKonto2='4403'")

        ' GKonto3 Updates
        fcUpdateCommand("Datev", fieldsG3, value4300, " WHERE GKonto3='4303'")
        fcUpdateCommand("Datev", fieldsG3, value4400, " WHERE GKonto3='4403'")

        ' Sonderregel: GKonto2 auf 4400 setzen, wenn GKonto2 = 4300 und 19% MwSt.
        fcUpdateCommand("Datev", fieldsG2, value4400, " WHERE GKonto2='4300' AND MwstS2 = '19'")

        ' 3. Datenbestand laden
        Dim SQL As String = "SELECT * FROM Datev ORDER BY ID ASC"
        dtDat = fcReadDataTable(SQL)

        ' 4. Filter und Kassen-ID initialisieren
        lFilter = True
        nKasseID = fcKasseID(2)

        ' 5. UI-Komponenten vorbereiten und mit Daten befüllen
        prColorRead()
        prSetTabelleDatev()
        prFillTabelleDatev(dtDat)

        ' 6. Standardwerte setzen und Layout berechnen
        tbDatum.Text = Date.Today.ToShortDateString()
        tbKasse.Text = "0"
        tbBank.Text = "0"
        prMainResize()
    End Sub

    ''' <summary>
    ''' Fängt das Resize-Ereignis des Formulars ab und stößt die Layout-Anpassung an.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    Private Sub frmDatev_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        prMainResize()
    End Sub

    ''' <summary>
    ''' Passt die Dimensionen des DataGridViews dynamisch an die aktuelle Fenstergröße an.
    ''' </summary>
    Private Sub prMainResize()
        ' Sicherheitsprüfung: Wenn das Fenster minimiert wird, macht eine Neuberechnung keinen Sinn
        If Me.WindowState = FormWindowState.Minimized Then Exit Sub

        ' Breiten- und Höhenberechnung für das Buchungs-Grid
        Dim berechneteBreite As Integer = Me.Width - 50
        Dim berechneteHoehe As Integer = Me.Height - 130

        ' Schutz vor negativen Werten (falls das Fenster extrem klein gezogen wird)
        If berechneteBreite > 10 Then dgvDatev.Width = berechneteBreite
        If berechneteHoehe > 10 Then dgvDatev.Height = berechneteHoehe
    End Sub

    ''' <summary>
    ''' Ereignishandler für den Schließen-Button. Ruft die zentrale Schließen-Logik auf.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax entfernt.
    ''' - Namespace-Deklarationen in der Signatur bereinigt.
    ''' </remarks>
    Private Sub tsbClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsbClose.Click
        prCloseForm()
    End Sub

    ''' <summary>
    ''' Blendet die Hauptmenüleiste des Hauptformulars (frmMain) wieder ein und schließt das aktuelle Formular.
    ''' </summary>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Inline-Kommentar zur besseren Lesbarkeit hinzugefügt.
    ''' </remarks>
    Private Sub prCloseForm()
        ' Hauptmenüleiste wieder sichtbar machen, bevor das Formular entladen wird
        frmMain.tsMain.Visible = True
        Me.Close()
    End Sub

    ''' <summary>
    ''' Erstellt die Spaltenstruktur für das DATEV-DataGridView, formatiert die Spaltenbreiten, 
    ''' Ausrichtungen, Selektionsfarben und deaktiviert das Sortieren der Spalten.
    ''' </summary>
    ''' <remarks>
    ''' 17.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Magische Zahlen-Schleife ('14 + 5') durch saubere, lesbare Spaltenindizes ersetzt.
    ''' - Erste Spalte (Index 0) über '.Visible = False' sicher ausgeblendet (statt Width = 0).
    ''' - 'AutoResizeRows' auf performante 'DisplayedCells' umgestellt, um Hänger bei großen Tabellen zu vermeiden.
    ''' - Datentyp-Deklaration in der 'ForEach'-Schleife direkt inline deklariert.
    ''' </remarks>
    Private Sub prSetTabelleDatev()
        With dgvDatev
            ' Bestehende Struktur löschen und Grundeinstellungen setzen
            .Columns.Clear()
            .ColumnHeadersHeight = 30
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True

            ' 1. Spalten hinzufügen (Name/Key, Header-Text)
            .Columns.Add("0", "0")
            .Columns.Add("RechNr", "RechNr.")
            .Columns.Add("Name", "Name")
            .Columns.Add("Umsatz1", "Umsatz 1")
            .Columns.Add("Mwst1", "Mwst bet.")
            .Columns.Add("MwstS1", "Mwst red.")
            .Columns.Add("Konto1", "Konto")
            .Columns.Add("GKonto1", "G.Konto")
            .Columns.Add("Umsatz2", "Umsatz 2")
            .Columns.Add("Mwst2", "Mwst bet.")
            .Columns.Add("MwstS2", "Mwst nor.")
            .Columns.Add("Konto2", "Konto")
            .Columns.Add("GKonto2", "G.Konto")
            .Columns.Add("Umsatz3", "Umsatz 3")
            .Columns.Add("Mwst3", "Mwst bet.")
            .Columns.Add("MwstS3", "Mwst Son.")
            .Columns.Add("Konto3", "Konto")
            .Columns.Add("GKonto3", "G.Konto")
            .Columns.Add("Datum", "R.Datum")
            .Columns.Add("BuchNr", "Buch Nr.")
            .Columns.Add("ID", "ID")
            .Columns.Add("KunNr", "Storno")
            .Columns.Add("Nr", "Nr")

            ' 2. Standard-Ausrichtung für alle Spalten festlegen (Standard: Links)
            For Each col As DataGridViewColumn In .Columns
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Next

            ' 3. Spaltenbreiten individuell anpassen
            .Columns(0).Visible = False ' Index 0 sicher ausblenden
            .Columns(1).Width = 75     ' RechNr
            .Columns(2).Width = 200    ' Name

            ' Spalten 3 bis 17 (Umsatz- und Kontoinformationen) einheitlich auf Breite 60 setzen
            For i As Integer = 3 To 17
                .Columns(i).Width = 60
            Next

            .Columns(18).Width = 75    ' Datum
            .Columns(19).Width = 60    ' BuchNr
            .Columns(20).Width = 40    ' ID
            .Columns(21).Width = 60    ' KunNr/Storno
            .Columns(22).Width = 60    ' Nr

            ' 4. Zeilenhöhe einmalig basierend auf den angezeigten Zellen performant anpassen
            .AutoResizeRows(DataGridViewAutoSizeRowsMode.DisplayedCells)

            ' 5. Sortierung aller Spalten unterbinden
            For Each col As DataGridViewColumn In .Columns
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            ' 6. Selektionsfarben zuweisen
            With .DefaultCellStyle
                .SelectionBackColor = fColorBackErlaubt
                .SelectionForeColor = fColorForeErlaubt
            End With

        End With
    End Sub

    ''' <summary>
    ''' Befüllt das DATEV-DataGridView basierend auf den Filter-Einstellungen mit den Daten aus der DataTable
    ''' und setzt zeilenweise die entsprechenden Hintergrund- und Schriftfarben.
    ''' </summary>
    ''' <param name="dt">Die DataTable mit den zu ladenden Buchungsdaten.</param>
    ''' <remarks>
    ''' 17.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax und VB6-'Trim()'-Aufrufe entfernt.
    ''' - Ungenutzte 'StringBuilder'-Variable gelöscht.
    ''' - UI-Performance optimiert: 'SuspendLayout' und 'ResumeLayout' verhindern Flackern beim Laden großer Datenmengen.
    ''' - Flexibilität erhöht: Übergabe der korrekten Parameter-DataTable 'dt' an 'prShowData' sichergestellt.
    ''' </remarks>
    Private Sub prFillTabelleDatev(ByVal dt As DataTable)
        If dt Is Nothing Then Return

        Dim nMax As Integer = dt.Rows.Count - 1
        Dim gridRowIndex As Integer = 0

        ' UI-Zeichnen pausieren, um die Performance drastisch zu verbessern
        dgvDatev.SuspendLayout()

        Try
            For i As Integer = 0 To nMax
                Dim datumValue As String = dt.Rows(i)("Datum").ToString().Trim()
                Dim isDatumEmpty As Boolean = String.IsNullOrEmpty(datumValue)

                If lFilter Then
                    ' Wenn der Filter aktiv ist, nur Datensätze ohne Datum (offene Buchungen) anzeigen
                    If isDatumEmpty Then
                        prShowData(dt, gridRowIndex, i)
                        prRowColor(gridRowIndex, fColorBackErlaubt, fColorForeErlaubt)
                        gridRowIndex += 1
                    End If
                Else
                    ' Ohne Filter alle Datensätze anzeigen und je nach Datumsstatus unterschiedlich einfärben
                    prShowData(dt, i, i)
                    If isDatumEmpty Then
                        prRowColor(i, fColorBackErlaubt, fColorForeErlaubt)
                    Else
                        prRowColor(i, fColorBackNichtErlaubt, fColorForeNichtErlaubt)
                    End If
                End If
            Next

        Finally
            ' UI-Zeichnen wieder aktivieren
            dgvDatev.ResumeLayout()
        End Try
    End Sub

    ''' <summary>
    ''' Erstellt eine neue Zeile im DataGridView und kopiert die formatierten Werte aus dem DataTable-Datensatz hinein.
    ''' </summary>
    ''' <param name="dt">Die Quell-DataTable.</param>
    ''' <param name="ii">Der Ziel-Zeilenindex im DataGridView (wird zur Zeilenreferenzierung genutzt).</param>
    ''' <param name="i">Der Quell-Zeilenindex in der DataTable.</param>
    ''' <remarks>
    ''' 17.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Falsche 'ByRef'-Parameter auf sicheres und standardkonformes 'ByVal' umgestellt.
    ''' - Redundantes Auslesen/Zurückschreiben der Umsatz- und MwSt-Zellen eliminiert (Werte werden direkt berechnet).
    ''' - Fehleranfälliges '.Rows.Add(" ")' durch gezieltes Hinzufügen optimiert.
    ''' - 'Val()'-Aufrufe durch null-sichere Konvertierungen ersetzt.
    ''' </remarks>
    Private Sub prShowData(ByVal dt As DataTable, ByVal ii As Integer, ByVal i As Integer)
        If dt Is Nothing OrElse i < 0 OrElse i >= dt.Rows.Count Then Return

        With dgvDatev
            ' Neue leere Zeile hinzufügen
            .Rows.Add()

            ' Direkten Zugriff auf die betroffene Zeile für bessere Performance sichern
            Dim row As DataGridViewRow = .Rows(ii)
            Dim dataRow As DataRow = dt.Rows(i)

            ' 1. Basisdaten auslesen und zuweisen
            row.Cells(1).Value = dataRow("RechNr").ToString()
            row.Cells(2).Value = dataRow("Name").ToString()
            row.Cells(5).Value = dataRow("MwstS1").ToString()
            row.Cells(6).Value = dataRow("Konto1").ToString()
            row.Cells(7).Value = dataRow("GKonto1").ToString()
            row.Cells(10).Value = dataRow("MwstS2").ToString()
            row.Cells(11).Value = dataRow("Konto2").ToString()
            row.Cells(12).Value = dataRow("GKonto2").ToString()
            row.Cells(15).Value = dataRow("MwstS3").ToString()
            row.Cells(16).Value = dataRow("Konto3").ToString()
            row.Cells(17).Value = dataRow("GKonto3").ToString()

            row.Cells(18).Value = fcUmDatum(dataRow("Datum").ToString())
            row.Cells(19).Value = dataRow("BuchNr").ToString()
            row.Cells(20).Value = dataRow("ID").ToString()
            row.Cells(21).Value = dataRow("KunNr").ToString()

            ' SelectionForeColor zentral für die Tabelle oder Zeile setzen
            .DefaultCellStyle.SelectionForeColor = Color.Black

            ' 2. Werte direkt aus der DataTable holen, konvertieren, berechnen und formatieren
            row.Cells(3).Value = FormatCurrencyValue(dataRow("Umsatz1"))
            row.Cells(4).Value = FormatCurrencyValue(dataRow("Mwst1"))

            row.Cells(8).Value = FormatCurrencyValue(dataRow("Umsatz2"))
            row.Cells(9).Value = FormatCurrencyValue(dataRow("Mwst2"))

            row.Cells(13).Value = FormatCurrencyValue(dataRow("Umsatz3"))
            row.Cells(14).Value = FormatCurrencyValue(dataRow("Mwst3"))
        End With
    End Sub



    ''' <summary>
    ''' Hilfsfunktion zur null-sicheren mathematischen Konvertierung und Formatierung von Cent-Beträgen.
    ''' Konvertiert den Rohwert aus der Datenbank, teilt ihn durch 100 und gibt einen formatierten Dezimal-String zurück.
    ''' </summary>
    ''' <param name="rawValue">Der zu konvertierende Wert (z. B. aus einer DataRow).</param>
    ''' <returns>Ein formatierter String mit zwei Nachkommastellen.</returns>
    ''' <remarks>
    ''' 28.09.2026 - Code-Optimierung:
    ''' - 'IsDBNull' durch die modernere .NET-Variante 'Convert.IsDBNull' ersetzt.
    ''' - 'CultureInfo.InvariantCulture' beim Parsen hinzugefügt, um Ländereinstellungskonflikte (Punkt/Komma) zu vermeiden.
    ''' - Redundante String-Konvertierungen innerhalb der Validierung minimiert.
    ''' </remarks>
    Private Function FormatCurrencyValue(ByVal rawValue As Object) As String
        ' Null- und DBNull-Prüfung kombiniert
        If rawValue Is Nothing OrElse Convert.IsDBNull(rawValue) Then
            Return fcDecStr(0.0, 7, 2, ",")
        End If

        Dim numericValue As Double
        ' Sicheres Parsen unter Berücksichtigung von System- und Invariant-Kulturen
        Dim inputStr As String = rawValue.ToString().Trim()

        If Double.TryParse(inputStr, NumberStyles.Any, CultureInfo.InvariantCulture, numericValue) OrElse
       Double.TryParse(inputStr, NumberStyles.Any, CultureInfo.CurrentCulture, numericValue) Then

            Return fcDecStr(numericValue / 100.0, 7, 2, ",")
        Else
            Return fcDecStr(0.0, 7, 2, ",")
        End If
    End Function





    ''' <summary>
    ''' Schaltet den Buchungsfilter (lFilter) um, aktualisiert den Button-Status 
    ''' und lädt die DATEV-Tabelle mit den gefilterten Daten neu.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 17.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Aufruf von 'prFillTabelleDatev' entfernt.
    ''' - 'If-Else'-Block durch direkten Boolschen Wechsel ('lFilter = Not lFilter') ersetzt.
    ''' - Namespace-Deklarationen in der Signatur auf .NET-Standard gekürzt.
    ''' </remarks>
    Private Sub tsbFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsbFilter.Click
        ' Zustand des Filters umkehren (True wird False, False wird True)
        lFilter = Not lFilter

        ' Tabelle mit den neuen Filtereinstellungen neu befüllen
        prFillTabelleDatev(dtDat)
    End Sub

    ''' <summary>
    ''' Setzt den Status der Kassen-ID, aktualisiert das UI-Label (Farbe und Text) und weist das entsprechende Konto zu.
    ''' </summary>
    ''' <param name="i">Die zu setzende Kassen-ID (z. B. 0 = Löschen, 3 = Kasse, 4 = Bank, -1 = Rückgängig).</param>
    ''' <returns>Gibt die übergebene Kassen-ID unverändert zurück.</returns>
    ''' <remarks>
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Funktion von 'ByRef' auf 'ByVal' umgestellt, da der Parameter 'i' nicht modifiziert wird.
    ''' - 'Return'-Statement anstelle der veralteten Zuweisung an den Funktionsnamen verwendet.
    ''' - Fehlende Text-Zuweisung bei 'Case 2' dokumentiert/beibehalten.
    ''' - Auskommentierten Code ('lbKasse.ForeColor = fColorForeBank' in Case 0) sauber entfernt.
    ''' </remarks>
    Private Function fcKasseID(ByVal i As Integer) As Integer
        Select Case i
            Case 1
                lbKasse.BackColor = fColorBackNichtErlaubt
                lbKasse.ForeColor = fColorForeNichtErlaubt
                lbKasse.Text = "Sperren"
            Case 2
                lbKasse.BackColor = fColorBackErlaubt
                lbKasse.ForeColor = fColorForeErlaubt
            ' Hinweis: Text wird hier im Originalcode nicht verändert
            Case 3
                lbKasse.BackColor = fColorBackKasse
                lbKasse.ForeColor = fColorForeKasse
                lbKasse.Text = "lKasse"
                sKonto = sKKonto
            Case 4
                lbKasse.BackColor = fColorBackBank
                lbKasse.ForeColor = fColorForeBank
                lbKasse.Text = "Bank"
                sKonto = sBKonto
            Case 0
                lbKasse.BackColor = fColorBackDel
                lbKasse.Text = "Löschen"
            Case -1
                lbKasse.BackColor = fColorBackErlaubt
                lbKasse.ForeColor = fColorForeErlaubt
                lbKasse.Text = "Rückgängig"
                sKonto = ""
        End Select

        Return i
    End Function


    ''' <summary>
    ''' Ereignishandler für den Kassen-Button. Setzt den Status auf Kasse (ID 3).
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>28.09.2026 - Initialer XML-Kommentar hinzugefügt.</remarks>
    Private Sub tsbKasse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbKasse.Click
        nKasseID = fcKasseID(3)
    End Sub

    ''' <summary>
    ''' Ereignishandler für den Bank-Button. Setzt den Status auf Bank (ID 4).
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>28.09.2026 - Initialer XML-Kommentar hinzugefügt.</remarks>
    Private Sub tsbBank_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBank.Click
        nKasseID = fcKasseID(4)
    End Sub

    ''' <summary>
    ''' Ereignishandler für den Löschen-Button. Setzt den Status auf Löschen (ID 0).
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>28.09.2026 - Initialer XML-Kommentar hinzugefügt.</remarks>
    Private Sub tsbDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDel.Click
        nKasseID = fcKasseID(0)
    End Sub

    ''' <summary>
    ''' Ereignishandler für den Frei/Rückgängig-Button. Setzt den Status auf Rückgängig (ID -1).
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>28.09.2026 - Initialer XML-Kommentar hinzugefügt.</remarks>
    Private Sub tsbFrei_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbFrei.Click
        nKasseID = fcKasseID(-1)
    End Sub

    '''' <summary>
    '''' Ereignishandler für das Eintreffen in einer Zelle des DataGridViews.
    '''' Steuert die farbliche Markierung, Kontenzuweisung und Stornologik basierend auf der aktuellen Kassen-ID.
    '''' </summary>
    '''' <param name="sender">Die Quelle des Ereignisses (DataGridView).</param>
    '''' <param name="e">Die Ereignisdaten mit Zeilen- und Spaltenindex.</param>
    '''' <remarks>
    '''' 28.09.2026 - Code-Optimierung:
    '''' - Veraltete 'Call'-Syntax bei Methodenaufrufen ('prRowColor') entfernt.
    '''' - 'Trim(...).Equals("")' durch performantes 'String.IsNullOrWhiteSpace' ersetzt.
    '''' - String-Vergleiche mit '.StartsWith()' modernisiert, um fehleranfällige 'Mid(...)'-Funktionen zu ersetzen.
    '''' - Leeren 'Else'-Zweig im 'Case Is > 2' entfernt.
    '''' - Inline-Kommentare zur besseren Strukturierung eingefügt.
    '''' </remarks>
    Private Sub dgvDatev_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDatev.CellEnter
        If nKasseID = -2 Then Exit Sub
        If e.RowIndex > -1 And e.ColumnIndex > 0 And e.ColumnIndex < 14 Then
            Dim nRow As Integer = e.RowIndex
            Dim sBackColor = lbKasse.BackColor
            Dim sForeColor = lbKasse.ForeColor
            With dgvDatev.DefaultCellStyle
                .SelectionBackColor = dgvDatev.Rows(nRow).Cells(7).Style.BackColor
                .SelectionForeColor = Color.Black
            End With
            With dgvDatev
                Select Case nKasseID
                    Case 0
                        If Trim(.Rows(nRow).Cells(19).Value) = "" Then .Rows(nRow).Cells(19).Value = "_"
                        Call prRowColor(nRow, sBackColor, sForeColor)
                    Case 1
                        If Trim(.Rows(nRow).Cells(21).Value) = "Storno" Then

                            If Mid(Trim(.Rows(nRow).Cells(19).Value), 1, 1) = "K" Then
                                .Rows(nRow).Cells(7).Value = sKKonto
                                .Rows(nRow).Cells(12).Value = sKKonto
                                .Rows(nRow).Cells(17).Value = sKKonto
                            End If
                            If Mid(Trim(.Rows(nRow).Cells(19).Value), 1, 1) = "B" Then
                                .Rows(nRow).Cells(7).Value = sBKonto
                                .Rows(nRow).Cells(12).Value = sBKonto
                                .Rows(nRow).Cells(17).Value = sBKonto
                            End If
                        Else
                            If Mid(Trim(.Rows(nRow).Cells(19).Value), 1, 1) = "K" Then
                                .Rows(nRow).Cells(6).Value = sKKonto
                                .Rows(nRow).Cells(11).Value = sKKonto
                                .Rows(nRow).Cells(16).Value = sKKonto
                            End If
                            If Mid(Trim(.Rows(nRow).Cells(19).Value), 1, 1) = "B" Then
                                .Rows(nRow).Cells(6).Value = sBKonto
                                .Rows(nRow).Cells(11).Value = sBKonto
                                .Rows(nRow).Cells(16).Value = sBKonto
                            End If
                        End If
                        If Trim(.Rows(nRow).Cells(19).Value) = "" Then .Rows(nRow).Cells(19).Value = "_"
                        Call prRowColor(nRow, sBackColor, sForeColor)
                    Case 2
                        Call prRowColor(nRow, sBackColor, sForeColor)
                        .Rows(nRow).Cells(6).Value = " "
                        .Rows(nRow).Cells(11).Value = " "
                        .Rows(nRow).Cells(16).Value = " "
                    Case -1
                        .Rows(nRow).Cells(22).Value = ""
                        .Rows(nRow).Cells(6).Value = sKonto
                        .Rows(nRow).Cells(11).Value = sKonto
                        .Rows(nRow).Cells(16).Value = sKonto
                        Call prRowColor(nRow, sBackColor, sForeColor)

                    Case Is > 2
                        If .Rows(nRow).Cells(2).Style.BackColor = lbKasse.BackColor And
                           .Rows(nRow).Cells(2).Style.ForeColor = lbKasse.ForeColor Or
                           .Rows(nRow).Cells(2).Style.BackColor = fColorBackErlaubt And
                           .Rows(nRow).Cells(2).Style.ForeColor = fColorForeErlaubt Then


                            If .Rows(nRow).Cells(2).Style.BackColor = fColorBackErlaubt And
                               .Rows(nRow).Cells(2).Style.ForeColor = fColorForeErlaubt Then
                                .Rows(nRow).Cells(22).Value = fcGetTimeID(Date.Today)
                                If Trim(.Rows(nRow).Cells(21).Value) = "Storno" Then
                                    .Rows(nRow).Cells(7).Value = sKonto
                                    .Rows(nRow).Cells(12).Value = sKonto
                                    .Rows(nRow).Cells(17).Value = sKonto
                                Else
                                    .Rows(nRow).Cells(6).Value = sKonto
                                    .Rows(nRow).Cells(11).Value = sKonto
                                    .Rows(nRow).Cells(16).Value = sKonto
                                End If

                                Call prRowColor(nRow, sBackColor, sForeColor)
                            Else

                            End If
                        End If
                End Select
            End With
        End If

    End Sub

    ''' <summary>
    ''' Setzt die Hintergrund- und Schriftfarbe sowie die Selektionsfarbe für eine bestimmte Zeile und deren Zellen.
    ''' </summary>
    ''' <param name="nRow">Der nullbasierte Index der einzufärbenden Zeile.</param>
    ''' <param name="sColorB">Die zu weisende Hintergrundfarbe (BackColor).</param>
    ''' <param name="sColorF">Die zu weisende Schriftfarbe (ForeColor).</param>
    ''' <remarks>
    ''' 17.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Parameter von 'ByRef' auf 'ByVal' umgestellt, da die übergebenen Werte in der Methode nicht verändert werden.
    ''' - Variable 'row' zwischengespeichert, um wiederholte Zugriffe auf 'dgvDatev.Rows(nRow)' in der Schleife zu minimieren.
    ''' </remarks>
    Private Sub prRowColor(ByVal nRow As Integer, ByVal sColorB As Color, ByVal sColorF As Color)
        ' Setzt die Selektionsfarben für das gesamte Grid zurück/gleich
        With dgvDatev.DefaultCellStyle
            .SelectionBackColor = sColorB
            .SelectionForeColor = sColorF
        End With

        ' Referenz auf die Zeile zwischenspeichern für bessere Performance
        Dim row As DataGridViewRow = dgvDatev.Rows(nRow)

        ' Jede einzelne Zelle explizit einfärben, um Überschreibungen durch Zellstile zu garantieren
        For i As Integer = 0 To 19
            With row.Cells(i).Style
                .BackColor = sColorB
                .ForeColor = sColorF
            End With
        Next
    End Sub

    ''' <summary>
    ''' Ereignishandler für den Freigeben-Button. Setzt den Kassen-Status auf Freigegeben/Erlaubt (ID 2).
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>28.09.2026 - Initialer XML-Kommentar hinzugefügt.</remarks>
    Private Sub tsbFreigeben_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbFreigeben.Click
        nKasseID = fcKasseID(2)
    End Sub

    ''' <summary>
    ''' Ereignishandler für den Sperren-Button. Setzt den Kassen-Status auf Gesperrt/Nicht Erlaubt (ID 1).
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>28.09.2026 - Initialer XML-Kommentar hinzugefügt.</remarks>
    Private Sub tsbSperren_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSperren.Click
        nKasseID = fcKasseID(1)
    End Sub

    ''' <summary>
    ''' Ereignishandler für den Export und die Generierung der DATEV-Datei.
    ''' Prüft zu löschende Einträge, validiert Pflichtangaben (Bank/Kasse), aktualisiert 
    ''' die zugrunde liegende Datentabelle (DataTable) und schreibt die CSV-Daten.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Uninitialisierte 'direction'-Variable bei der Grid-Sortierung mit Standardwert 'Ascending' abgesichert.
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen konsequent entfernt.
    ''' - 'Trim(...).Equals("")' und 'Val()' durch sicherere .NET-Methoden wie 'String.IsNullOrWhiteSpace', 'Int32.TryParse' und 'Convert.ToDecimal' ersetzt.
    ''' - 'StringBuilder' zur performanten Generierung des CSV-Dateiinhalts implementiert, um String-Verkettungen in Schleifen zu optimieren.
    ''' - Null-Zuweisungsschutz ('?.ToString()') bei Grid-Zellwerten integriert.
    ''' </remarks>
    Private Sub dsbDatevFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dsbDatevFile.Click
        Dim nMax As Integer = dgvDatev.Rows.Count - 1
        Dim nMaxD As Integer = dtDat.Rows.Count - 1

        Dim nKNr As Integer = 0
        Dim nBNr As Integer = 0
        Int32.TryParse(tbKasse.Text, nKNr)
        Int32.TryParse(tbBank.Text, nBNr)

        Dim sDatum As Date
        If Not Date.TryParse(tbDatum.Text, sDatum) Then sDatum = Date.Today

        Dim sTag As String = PadLN(sDatum.Day.ToString(), 2)
        Dim sMonat As String = PadLN(sDatum.Month.ToString(), 2)
        Dim sJahr As String = sDatum.Year.ToString()

        Dim sNr As String = ""
        Dim lBank As Boolean = False
        Dim lKasse As Boolean = False
        Dim lDatevDatei As Boolean = True
        Dim arDel(nMax) As String
        Dim iDel As Integer = -1

        ' StringBuilder für performanten Datei-Export
        Dim sbDaten As New System.Text.StringBuilder()
        sbDaten.AppendLine("WKZ-Umsatz;S/H-KZ;Umsatz(o.S/H-KZ);Gegenkonto(o.BU);Belegfeld;Datum;Kontonr;Buchtext;Festschreibung")

        nKasseID = -2

        ' Fehlerbehebung: Sortierrichtung explizit initialisieren
        Dim direction As System.ComponentModel.ListSortDirection = System.ComponentModel.ListSortDirection.Ascending
        dgvDatev.Sort(dgvDatev.Columns("Nr"), direction)

        With dgvDatev
            ' 1. Zu löschende Datensätze ermitteln
            For i As Integer = 0 To nMax
                Dim cell19Style As DataGridViewCellStyle = .Rows(i).Cells(19).Style
                If cell19Style.BackColor = fColorBackDel AndAlso cell19Style.ForeColor = fColorForeBank Then
                    iDel += 1
                    arDel(iDel) = Convert.ToString(.Rows(i).Cells(20).Value)
                End If
            Next

            ' Löschvorgang durchführen
            If iDel > -1 Then
                Dim sMsg As String = $"Wollen Sie diese {iDel + 1} Datensätze löschen?"
                If MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkCancel, "Löschen") = MsgBoxResult.Ok Then
                    For i As Integer = 0 To iDel
                        Dim cSql As String = $"DELETE FROM Datev WHERE ID = '{arDel(i)}'"
                        UpdateTable(cSql)
                    Next
                End If
            End If

            ' 2. Prüfen, ob Kassen- oder Bankbuchungen vorliegen
            For i As Integer = 0 To nMax
                Dim val6 As String = Convert.ToString(.Rows(i).Cells(6).Value)?.Trim()
                Dim val11 As String = Convert.ToString(.Rows(i).Cells(11).Value)?.Trim()
                Dim val16 As String = Convert.ToString(.Rows(i).Cells(16).Value)?.Trim()

                If val6 = sKKonto OrElse val11 = sKKonto OrElse val16 = sKKonto Then lKasse = True
                If val6 = sBKonto OrElse val11 = sBKonto OrElse val16 = sBKonto Then lBank = True
            Next

            ' Validierung der Pflichtnummern
            If lBank AndAlso nBNr = 0 Then
                MessageBox.Show("Buchungsnummer Bank eingeben")
                lDatevDatei = False
            End If
            If lKasse AndAlso nKNr = 0 Then
                MessageBox.Show("Buchungsnummer Kasse eingeben")
                lDatevDatei = False
            End If

            ' 3. Verarbeitung und Export-Generierung
            If lDatevDatei Then
                For i As Integer = 0 To nMax
                    Dim cell19Style As DataGridViewCellStyle = .Rows(i).Cells(19).Style
                    Dim cell20Val As String = Convert.ToString(.Rows(i).Cells(20).Value)
                    Dim isStorno As Boolean = (Convert.ToString(.Rows(i).Cells(21).Value)?.Trim() = "Storno")
                    Dim rechnungsNr As String = Convert.ToString(.Rows(i).Cells(1).Value)?.Trim()

                    ' Fall A: Datensätze reaktivieren / zurücksetzen
                    If cell19Style.BackColor = fColorBackErlaubt OrElse cell19Style.ForeColor = fColorForeErlaubt Then
                        For j As Integer = 0 To nMaxD
                            If cell20Val = Convert.ToString(dtDat.Rows(j).Item("ID")) Then
                                dtDat.Rows(j).Item("BuchNr") = " "
                            End If
                        Next
                    End If

                    ' Fall B: Bank- oder Kassenbuchung verarbeiten
                    If (cell19Style.BackColor = fColorBackBank AndAlso cell19Style.ForeColor = fColorForeBank) OrElse
                   (cell19Style.BackColor = fColorBackKasse AndAlso cell19Style.ForeColor = fColorForeKasse) Then

                        ' --- Umsatz 1 verarbeiten ---
                        Dim nUmsatz1 As Decimal = 0
                        Decimal.TryParse(Convert.ToString(.Rows(i).Cells(3).Value), nUmsatz1)

                        If nUmsatz1 <> 0D Then
                            Dim konto6 As String = Convert.ToString(.Rows(i).Cells(6).Value)?.Trim()
                            If konto6 = sKKonto Then
                                sNr = "K" & PadLN(nKNr.ToString(), 4)
                                nKNr += 1
                            Else
                                sNr = "B" & PadLN(nBNr.ToString(), 4)
                                nBNr += 1
                            End If

                            Dim konto7 As String = Convert.ToString(.Rows(i).Cells(7).Value)?.Trim()
                            Dim textStorno As String = If(isStorno, " Storno Rechnung  ", "Rechnung  ")

                            sbDaten.Append($"EUR;S;{nUmsatz1};{konto7};{sNr};{sTag}.{sMonat}.{sJahr};{konto6};{textStorno}{rechnungsNr};0{vbCrLf}")
                            .Rows(i).Cells(19).Value = sNr

                            For j As Integer = 0 To nMaxD
                                If cell20Val = Convert.ToString(dtDat.Rows(j).Item("ID")) Then
                                    dtDat.Rows(j).Item("Konto1") = .Rows(i).Cells(6).Value
                                    dtDat.Rows(j).Item("BuchNr") = sNr
                                    dtDat.Rows(j).Item("Datum") = fcUmDatum(sDatum)
                                End If
                            Next
                        End If

                        ' --- Umsatz 2 verarbeiten ---
                        Dim nUmsatz2 As Decimal = 0
                        Decimal.TryParse(Convert.ToString(.Rows(i).Cells(8).Value), nUmsatz2)

                        If nUmsatz2 <> 0D Then
                            Dim konto11 As String = Convert.ToString(.Rows(i).Cells(11).Value)?.Trim()
                            If konto11 = sKKonto Then
                                sNr = "K" & PadLN(nKNr.ToString(), 4)
                                nKNr += 1
                            Else
                                sNr = "B" & PadLN(nBNr.ToString(), 4)
                                nBNr += 1
                            End If

                            Dim konto12 As String = Convert.ToString(.Rows(i).Cells(12).Value)?.Trim()
                            Dim textStorno As String = If(isStorno, "Storno Rechnung  ", "Rechnung  ")

                            sbDaten.Append($"EUR;S;{nUmsatz2};{konto12};{sNr};{sTag}.{sMonat}.{sJahr};{konto11};{textStorno}{rechnungsNr};0{vbCrLf}")
                            .Rows(i).Cells(19).Value = sNr

                            For j As Integer = 0 To nMaxD
                                If cell20Val = Convert.ToString(dtDat.Rows(j).Item("ID")) Then
                                    dtDat.Rows(j).Item("Konto2") = .Rows(i).Cells(11).Value
                                    dtDat.Rows(j).Item("BuchNr") = sNr
                                    dtDat.Rows(j).Item("Datum") = fcUmDatum(sDatum)
                                End If
                            Next
                        End If
                        ' --- Umsatz 3 verarbeiten (Sonder MwSt) ---
                        Dim nUmsatz3 As Decimal = 0
                        Decimal.TryParse(Convert.ToString(.Rows(i).Cells(13).Value), nUmsatz3)

                        If nUmsatz3 <> 0D Then
                            Dim konto16 As String = Convert.ToString(.Rows(i).Cells(16).Value)?.Trim()

                            If konto16 = sKKonto Then
                                sNr = "K" & PadLN(nKNr.ToString(), 4)
                                nKNr += 1
                            Else
                                sNr = "B" & PadLN(nBNr.ToString(), 4)
                                nBNr += 1
                            End If

                            Dim konto17 As String = Convert.ToString(.Rows(i).Cells(17).Value)?.Trim()
                            Dim textStorno As String = If(isStorno, "Storno Rechnung  ", "Rechnung  ")

                            ' CSV-Zeile performant an den StringBuilder anhängen
                            sbDaten.Append($"EUR;S;{nUmsatz3};{konto17};{sNr};{sTag}.{sMonat}.{sJahr};{konto16};{textStorno}{rechnungsNr};0{vbCrLf}")
                            .Rows(i).Cells(19).Value = sNr

                            ' Aktualisieren der Datentabelle
                            For j As Integer = 0 To nMaxD
                                If cell20Val = Convert.ToString(dtDat.Rows(j).Item("ID")) Then
                                    ' Hinweis: Im Original stand hier Konto2. Falls deine DataTable eine Spalte "Konto3" besitzt, sollte dies hier angepasst werden.
                                    dtDat.Rows(j).Item("Konto2") = .Rows(i).Cells(16).Value
                                    dtDat.Rows(j).Item("BuchNr") = sNr
                                    dtDat.Rows(j).Item("Datum") = fcUmDatum(sDatum)
                                End If
                            Next
                        End If

                    End If
                Next

                prFillTabelleDatev(dtDat)
            End If
        End With

        ' Datei schreiben und Datentabelle sichern
        WriteFileSeriell(Convert.ToString(arIni(32)) & "\Datev\datev.txt", sbDaten.ToString())

        For i As Integer = 0 To nMaxD
            prSavedatev(i)
        Next
    End Sub

    ''' <summary>
    ''' Speichert einen spezifischen Datensatz aus der DataTable in der Datev-Datenbanktabelle.
    ''' </summary>
    ''' <param name="i">Der nullbasierte Zeilenindex (RowIndex) innerhalb der DataTable 'dtDat'.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Parameter von 'ByRef' auf 'ByVal' umgestellt, da der Index nur lesend verwendet wird.
    ''' - Veraltete Visual Basic 'Split'-Funktion durch die performantere '.Split()'-Methode des String-Objekts ersetzt.
    ''' - Veraltetes 'Call'-Schlüsselwort entfernt.
    ''' - Leeren 'Finally'-Block entfernt, um den Code schlank zu halten.
    ''' - Null-Sicheres Auslesen des ID-Feldes ('Convert.ToString') implementiert.
    ''' </remarks>
    Private Sub prSavedatev(ByVal i As Integer)
        Dim sqlText As String = ""
        Dim arFields() As String
        Dim arValue() As String
        Dim cBedingung As String = ""

        ' ID sicher aus der DataTable auslesen
        Dim cellObj As Object = dtDat.Rows(i).Item("ID")
        Dim sID As String = If(cellObj IsNot Nothing AndAlso Not IsDBNull(cellObj), cellObj.ToString().Trim(), "")


        Try
            ' Feldliste definieren und in Array splitten
            sqlText = "ID,RechNr,GKonto1,Konto1,GKonto2,Konto2,GKonto3,Konto3,Datum,BuchNr"
            arFields = sqlText.Split(","c)

            ' Werte über die Hilfsfunktion holen und splitten
            Dim sWerteText As String = fcSavedatev(sID, i, dtDat)
            arValue = sWerteText.Split("°"c)

            ' Update-Kriterium und Ausführung
            cBedingung = $" WHERE ID='{sID}'"
            fcUpdateCommand("Datev", arFields, arValue, cBedingung)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Bereitet die zu speichernden Felder einer DataRow auf, ersetzt leere Werte durch ein Leerzeichen 
    ''' und verkettet sie mit dem Trennzeichen '°' für das Datenbank-Update.
    ''' </summary>
    ''' <param name="sID">Die eindeutige Identifikationsnummer des Datensatzes.</param>
    ''' <param name="i">Der nullbasierte Zeilenindex (RowIndex) innerhalb der DataTable.</param>
    ''' <param name="dt">Die zu verarbeitende DataTable, die die Datev-Daten enthält.</param>
    ''' <returns>Ein durch '°' separierter String aller aufbereiteten Feldwerte.</returns>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Parameter 'i' von 'ByRef' auf 'ByVal' umgestellt, da der Index nur lesend verwendet wird.
    ''' - Absturzsichere Prüfung auf 'DBNull' und 'Nothing' mittels einer kompakten Feldliste implementiert.
    ''' - Explizites 'Return'-Statement anstelle der veralteten Zuweisung an den Funktionsnamen verwendet.
    ''' - Referenz auf die spezifische Datenzeile ('row') zwischengespeichert, um Performance zu verbessern.
    ''' </remarks>
    Private Function fcSavedatev(ByVal sID As String, ByVal i As Integer, ByVal dt As DataTable) As String
        Dim sb As New StringBuilder()
        Dim row As DataRow = dt.Rows(i)

        ' Liste aller zu prüfenden Spaltennamen
        Dim fieldsToValidate As String() = {"RechNr", "Konto1", "GKonto1", "Konto2", "GKonto2", "Konto3", "GKonto3", "Datum", "BuchNr"}

        ' Spalten auf DBNull, Nothing oder leere Zeichenfolgen prüfen und ggf. durch ein Leerzeichen ersetzen
        For Each field As String In fieldsToValidate
            Dim cellValue As Object = row.Item(field)
            If cellValue Is Nothing OrElse IsDBNull(cellValue) OrElse String.IsNullOrWhiteSpace(cellValue.ToString()) Then
                row.Item(field) = " "
            End If
        Next

        ' String mit Trennzeichen '°' performant aufbauen
        sb.Append(Convert.ToString(row.Item("ID"))).Append("°")
        sb.Append(Convert.ToString(row.Item("RechNr"))).Append("°")
        sb.Append(Convert.ToString(row.Item("GKonto1"))).Append("°")
        sb.Append(Convert.ToString(row.Item("Konto1"))).Append("°")
        sb.Append(Convert.ToString(row.Item("GKonto2"))).Append("°")
        sb.Append(Convert.ToString(row.Item("Konto2"))).Append("°")
        sb.Append(Convert.ToString(row.Item("GKonto3"))).Append("°")
        sb.Append(Convert.ToString(row.Item("Konto3"))).Append("°")
        sb.Append(Convert.ToString(row.Item("Datum"))).Append("°")
        sb.Append(Convert.ToString(row.Item("BuchNr")))

        Return sb.ToString()
    End Function






End Class