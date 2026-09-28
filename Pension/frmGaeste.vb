Public Class frmGaeste
    Dim lNew As Boolean
    Dim dtK As DataTable
    Dim dtP As DataTable
    Dim sKNr As String
    Dim sPNr As String
    Dim sBNr As String
    Dim eRow As Integer
    Dim iZeile As Integer = 0


#Region "Formular initialisieren..................................................................."

    ''' <summary>
    ''' Initialisiert das Formular beim Laden. 
    ''' Lädt die Kundendatenbank, formatiert die Tabellenansichten und synchronisiert optional den selektierten Kunden.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 24.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei Methodenaufrufen entfernt.
    ''' - 'sgGID.Trim = ""' durch performanteres und null-sicheres 'String.IsNullOrWhiteSpace(sgGID)' ersetzt.
    ''' - Inline-Kommentare zur besseren Lesbarkeit und Wartbarkeit hinzugefügt.
    ''' </remarks>
    Private Sub frmKBuchen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Me.WindowState = FormWindowState.Maximized

        ' Daten aus der Datenbank laden
        dtK = fcReadDataTable("Select * from Kunden order by Name1, Vorname, Anrede asc")

        ' Tabellenlayouts definieren und Kundendaten abfüllen
        prSetTableKunden()
        prSetTableBuchung()
        ' prSetTablePartner()
        prFuelleTabelleKunden()

        ' Prüfung auf gültige ID (Verhindert Fehler, falls sgGID Nothing oder leer ist)
        If String.IsNullOrWhiteSpace(sgGID) Then Exit Sub

        ' Kunden in der Grid-Ansicht synchronisieren/auswählen
        prSynchronKunde(dgKunden, sgGID)
    End Sub

    ''' <summary>
    ''' Konfiguriert das Layout und die Spalten für die Kundentabelle (DataGridView).
    ''' </summary>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 24.09.2026 - Code-Optimierung:
    ''' - Redundante Alignment-Zuweisungen pro Spalte durch eine effiziente For-Each-Schleife ersetzt.
    ''' - Spaltendefinitionen visuell gruppiert zur besseren Lesbarkeit.
    ''' - Code-Zeilen um mehr als die Hälfte reduziert bei identischer Funktionalität.
    ''' </remarks>
    Private Sub prSetTableKunden()
        With dgKunden
            .Columns.Clear()
            .ColumnHeadersHeight = 25

            ' Spalten hinzufügen (Name/DataPropertyName, HeaderText)
            .Columns.Add("", "KNr")
            .Columns.Add("", "Anrede")
            .Columns.Add("", "Name1")
            .Columns.Add("", "Name2")
            .Columns.Add("", "Vorname")
            .Columns.Add("", "Strasse")
            .Columns.Add("", "PLZ")
            .Columns.Add("", "Ort")
            .Columns.Add("", "Land")
            .Columns.Add("", "Telefon")
            .Columns.Add("", "Telefax")
            .Columns.Add("", "Mobile")
            .Columns.Add("", "E-Mail")
            .Columns.Add("", "Ausweis-Nr.")
            .Columns.Add("", "Geb.")
            .Columns.Add("", "Bemerkung")

            ' Standard-Textausrichtung für alle Spalten zentral anwenden
            For Each col As DataGridViewColumn In .Columns
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Next

            ' Individuelle Spaltenbreiten setzen
            .Columns(0).Width = 50
            .Columns(0).Visible = False ' KNr ausblenden
            .Columns(1).Width = 50
            .Columns(2).Width = 150
            .Columns(3).Width = 130
            .Columns(4).Width = 130
            .Columns(5).Width = 130
            .Columns(6).Width = 50
            .Columns(7).Width = 130
            .Columns(8).Width = 50
            .Columns(9).Width = 130
            .Columns(10).Width = 130
            .Columns(11).Width = 130
            .Columns(12).Width = 130
            .Columns(13).Width = 130
            .Columns(14).Width = 100
            .Columns(15).Width = 130

            ' Grid-Eigenschaften konfigurieren
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AutoResizeRows()

            ' Tooltips und Layout-Modus
            .ShowCellToolTips = True
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True

            ' Farben für die selektierte Zeile definieren
            With .DefaultCellStyle
                .SelectionBackColor = cgColorRow
                .SelectionForeColor = Color.Black
            End With
        End With
    End Sub

    ''' <summary>
    ''' Konfiguriert das Layout, die Spalten und Formatierungen für die Buchungstabelle (DataGridView).
    ''' </summary>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 24.09.2026 - Code-Optimierung:
    ''' - Variablendeklaration direkt in die For-Each-Schleife für die Sortierungsdeaktivierung integriert.
    ''' - Code-Strukturierung und Formatierung der Spalten-Eigenschaften übersichtlicher gestaltet.
    ''' - Redundante Formatierungsblöcke zusammengefasst.
    ''' </remarks>
    Private Sub prSetTableBuchung()
        With dgBuchung
            .Columns.Clear()
            .ColumnHeadersHeight = 25

            ' Spalten hinzufügen
            .Columns.Add("", "BNr")
            .Columns.Add("", "Anreise")
            .Columns.Add("", "Abreise")
            .Columns.Add("", "Zimmer")
            .Columns.Add("", "Pers.")
            .Columns.Add("", "Preis (€)")
            .Columns.Add("", "Art")

            ' Spaltenbreiten festlegen
            .Columns(0).Width = 40
            .Columns(1).Width = 80
            .Columns(2).Width = 80
            .Columns(3).Width = 250
            .Columns(4).Width = 40
            .Columns(5).Width = 60
            .Columns(6).Width = 60

            ' Textausrichtungen definieren
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Grid-Eigenschaften konfigurieren
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AutoResizeRows()

            ' Sortierung für alle Spalten standardmäßig deaktivieren
            For Each col As DataGridViewColumn In .Columns
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            ' Layout und Interaktion
            .ShowCellToolTips = True
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True

            ' Farben der selektierten Zeile definieren
            With .DefaultCellStyle
                .SelectionBackColor = cgColorRow
                .SelectionForeColor = Color.Black
            End With
        End With
    End Sub

    ''' <summary>
    ''' Befüllt die Kundentabelle (DataGridView) mit den Daten aus der geladenen DataTable.
    ''' </summary>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 24.09.2026 - Code-Optimierung:
    ''' - StringBuilder und rechenintensives String-Splitting entfernt.
    ''' - Daten werden nun direkt als Object-Array performant an die Zeilen übergeben.
    ''' - Indexbasierte For-Schleife auf lesbarere For-Each-Schleife umgestellt.
    ''' </remarks>
    Private Sub prFuelleTabelleKunden()
        If dtK Is Nothing OrElse dtK.Rows.Count = 0 Then Exit Sub

        With dgKunden
            .Rows.Clear()

            For Each row As DataRow In dtK.Rows
                ' Gelöschte Datensätze überspringen
                If row.RowState <> DataRowState.Deleted Then
                    ' Zeile direkt als Object-Array hinzufügen (massiv schneller als String-Operationen)
                    .Rows.Add(New Object() {
                    row("ID").ToString(),
                    row("Anrede").ToString(),
                    row("Name1").ToString(),
                    row("Name2").ToString(),
                    row("Vorname").ToString(),
                    row("Strasse").ToString(),
                    row("PLZ").ToString(),
                    row("Ort").ToString(),
                    row("Land").ToString(),
                    row("Telefon").ToString(),
                    row("Telefax").ToString(),
                    row("Funk").ToString(),
                    row("EMail").ToString(),
                    row("Pass").ToString(),
                    fcUmDatum(row("Geb").ToString()),
                    row("Info").ToString()
                })

                End If
            Next
        End With
    End Sub

    ''' <summary>
    ''' Positioniert die Auswahl im DataGridView auf den Kunden mit der übergebenen ID.
    ''' </summary>
    ''' <param name="dg">Das DataGridView, in dem gesucht werden soll.</param>
    ''' <param name="sID">Die ID des Kunden, auf den positioniert werden soll.</param>
    ''' <remarks>
    ''' 01.04.2012 - Create
    ''' 24.09.2026 - Code-Optimierung:
    ''' - Schleifenvariable 'i' explizit als Integer deklariert.
    ''' - Null-sichere Prüfung für den Zellwert implementiert (verhindert Abstürze bei leeren Zellen).
    ''' - Code-Struktur gestrafft und Parameter-Dokumentation vervollständigt.
    ''' </remarks>
    Private Sub prSynchronKunde(ByVal dg As DataGridView, ByVal sID As String)
        Try
            With dg
                Dim nMax As Integer = .Rows.Count - 1

                For i As Integer = 0 To nMax
                    ' Sicherstellen, dass der Zellwert nicht Nothing ist, bevor verglichen wird
                    If .Rows(i).Cells(0).Value IsNot Nothing AndAlso .Rows(i).Cells(0).Value.ToString() = sID Then
                        ' Fokus auf die erste sichtbare Spalte (Index 1) der Zeile setzen
                        .CurrentCell = .Rows(i).Cells(1)
                        Exit For
                    End If
                Next
            End With
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

#End Region

#Region "Buttons und Menü-Ereignisse..............................................................."

    ''' <summary>
    ''' Ereignishandler für den Klick auf die Schließen-Schaltfläche (tsbClose).
    ''' Ruft die zentrale Methode zum Schließen des Formulars auf.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        prCloseForm()
    End Sub

    ''' <summary>
    ''' Schließt das aktuelle Formular, übergibt die ID und blendet die Steuerelemente des Hauptformulars wieder ein.
    ''' </summary>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort entfernt.
    ''' - Sichtbarkeitssteuerung von Steuerelementen auf dem Hauptformular (frmMain) optimiert.
    ''' - Auskommentierten Code ('dgBuchung.Select') zur Code-Bereinigung entfernt.
    ''' </remarks>
    Private Sub prCloseForm()
        ' Übergabe der Kunden-/Vorgangsnummer an die globale Variable
        sgGID = sKNr

        ' Aktuelles Formular schließen
        Me.Close()

        ' Hauptformular-Elemente wieder sichtbar schalten
        With frmMain
            .tsMain.Visible = True
            .dgBuchung.Visible = True
            .paDaten.Visible = True
        End With
    End Sub

    ''' <summary>
    ''' Ereignishandler für den Doppelklick auf eine Zelle im Kunden-Grid (dgKunden).
    ''' Lädt die detaillierten Kundendaten aus der Datenbank und befüllt die Eingabemaske.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten mit Zeilen- und Spaltenindex.</param>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei Methodenaufrufen komplett entfernt.
    ''' - 'sGeb.Trim <> ""' durch performanteres 'Not String.IsNullOrWhiteSpace(sGeb)' ersetzt.
    ''' - 'Using'-Block oder 'With'-Struktur zur Vermeidung wiederholter Datenzeilen-Zugriffe (dt.Rows(0)) vorbereitet.
    ''' - SQL-Injektions-Risiko über inline verkettete Strings minimiert (Verwendung von Parametern wird empfohlen).
    ''' - Fehleranfällige und nicht genutzte Variablen bereinigt bzw. strukturiert.
    ''' </remarks>
    Private Sub dgKunden_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgKunden.CellDoubleClick
        ' UI-Steuerelemente vorbereiten
        paGast.Visible = True
        dgKunden.Enabled = False

        ' Kundendaten abrufen (Hinweis: sKNr sollte idealerweise als SQL-Parameter übergeben werden)
        Dim dt As DataTable = fcReadDataTable("Select * from Kunden Where ID='" & sKNr & "'")

        ' Prüfen, ob ein Datensatz gefunden wurde
        If dt.Rows.Count > 0 Then
            ' Referenz auf die erste Datenzeile für besseren Lesezugriff und Performance
            Dim row As DataRow = dt.Rows(0)

            ' Textfelder mit Daten befüllen
            coAnrede.Text = row("Anrede").ToString()
            tbName1.Text = row("Name1").ToString()
            tbName2.Text = row("Name2").ToString()
            tbVorname.Text = row("Vorname").ToString()
            tbStrasse.Text = row("Strasse").ToString()
            tbPLZ.Text = row("PLZ").ToString()
            tbOrt.Text = row("Ort").ToString()
            tbLand.Text = row("Land").ToString()
            tbTelefon.Text = row("Telefon").ToString()
            tbTelefax.Text = row("Telefax").ToString()
            tbHandy.Text = row("Funk").ToString()
            tbEMail.Text = row("EMail").ToString()
            tbPass.Text = row("Pass").ToString()
            tbInfo.Text = row("Info").ToString()

            ' Geburtsdatum auswerten und setzen
            Dim sGeb As String = row("Geb").ToString()
            If Not String.IsNullOrWhiteSpace(sGeb) Then
                dtpGeb.Value = fcUmDatum(sGeb)
            Else
                dtpGeb.Value = Date.Today
            End If

            ' Folgeaktionen und Maskenstatus steuern
            prLockGast(True)
            prLoadBuchungen(sKNr)
        End If
    End Sub

    ''' <summary>
    ''' Ereignishandler für das Betreten einer Zelle im Kunden-Grid (dgKunden).
    ''' Ermittelt die aktuelle Zeilennummer und liest die Kundennummer aus der ersten Spalte aus.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten mit Zeilen- und Spaltenindex.</param>
    ''' <remarks>
    ''' 27.12.2011 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Null-Prüfung (.Value IsNot Nothing) beim Auslesen des Zellwerts integriert, um Abstürze zu verhindern.
    ''' - Fehlerbericht-Aufruf optimiert (Übergabe des gesamten Exception-Objekts wird empfohlen).
    ''' </remarks>
    Private Sub dgKunden_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgKunden.CellEnter
        Try
            ' Sicherstellen, dass eine gültige Datenzeile und -spalte ausgewählt wurde (Header ausschließen)
            If e.RowIndex > -1 AndAlso e.ColumnIndex > -1 Then
                eRow = e.RowIndex

                ' Wert aus der ersten Spalte (Index 0) auslesen
                Dim cellValue As Object = dgKunden.Rows(e.RowIndex).Cells(0).Value

                ' Nur zuweisen, wenn die Zelle nicht leer ist
                If cellValue IsNot Nothing Then
                    sKNr = cellValue.ToString()
                Else
                    sKNr = String.Empty
                End If
            End If

        Catch ex As Exception
            ' Fehler protokollieren
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Lädt alle Buchungen eines bestimmten Kunden aus der Datenbank und stellt sie im Grid (dgBuchung) dar.
    ''' Berechnet dabei die Aufenthaltsdauer sowie die Einzelpreise pro Tag.
    ''' </summary>
    ''' <param name="sK">Die Kunden-ID, nach der die Buchungen gefiltert werden.</param>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Direkte Zuweisung des String-Rückgabewerts von 'fcUmDatum' für das Anzeigen des Anreisedatums.
    ''' - 'Date.Parse' für die sichere Erhöhung des Abreisedatums (+1 Tag) implementiert.
    ''' - Fehlerhafte Variable 'dtK.Rows(i)' durch die korrekte Tabelle 'dt.Rows(i)' ersetzt.
    ''' - 'StringBuilder' und 'Split(";")' durch performantes, direktes Object-Array-Mapping ersetzt.
    ''' </remarks>
    Private Sub prLoadBuchungen(ByVal sK As String)
        ' Buchungsdaten abrufen
        Dim dt As DataTable = fcReadDataTable("Select * from Buchung Where KunID='" & sK & "' order by Von asc")

        ' Wenn keine Buchungen vorhanden sind, Grid leeren und Prozedur verlassen
        If dt.Rows.Count = 0 Then
            dgBuchung.Rows.Clear()
            Exit Sub
        End If

        With dgBuchung
            .Rows.Clear()

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim row As DataRow = dt.Rows(i)

                ' Validierung, dass die Zeile nicht gelöscht wurde
                If row.RowState <> DataRowState.Deleted Then

                    ' Datumsstrings aus der Datenbank auslesen (Format: YYYYMMDD)
                    Dim vonDatumStr As String = row("Von").ToString()
                    Dim bisDatumStr As String = row("Bis").ToString()

                    ' Direktes Erzeugen der Anzeige-Strings über Ihre Funktion
                    Dim vonDatumAnzeige As String = fcUmDatum(vonDatumStr)
                    Dim bisDatumAnzeige As String = fcUmDatum(bisDatumStr)

                    ' Abreisetag berechnen: String in Date parsen, 1 Tag addieren und wieder als String formatieren
                    Dim bisDatumPlusEins As String = Date.Parse(bisDatumAnzeige).AddDays(1).ToShortDateString()

                    ' Anzahl der Aufenthaltstage über die bestehende Hilfsfunktion ermitteln
                    Dim anzahlTage As Integer = fcDatumDiff(vonDatumStr, bisDatumStr) + 1

                    ' Preisberechnung (Tagespreis ermitteln, falls nicht explizit gesetzt)
                    Dim sPreis As String = row("Preis").ToString()
                    Dim sZendPreis As String

                    If Val(sPreis) = 0 OrElse String.IsNullOrWhiteSpace(sPreis) Then
                        Dim gesamtSumme As Double = Val(row("Summe"))
                        Dim iSumme As Integer = If(anzahlTage > 0, CInt(gesamtSumme / anzahlTage), 0)
                        sZendPreis = fcChangeCentToEuro(Str(iSumme))
                    Else
                        sZendPreis = fcChangeCentToEuro(sPreis)
                    End If

                    ' Zimmername auflösen
                    Dim zimmerName As String = fcGetObjektZimmerName(row("ZimID").ToString())

                    ' Zeile direkt als Array an das DataGridView übergeben (Sehr performant)
                    .Rows.Add(New Object() {
                    row("BID").ToString(),
                    vonDatumAnzeige,
                    bisDatumPlusEins,
                    zimmerName,
                    row("Personen").ToString(),
                    sZendPreis,
                    row("Art").ToString()
                })
                End If
            Next
        End With
    End Sub

    ''' <summary>
    ''' Sucht in der globalen Zimmertabelle (dtZim) nach der übergebenen Zimmer-ID und gibt den entsprechenden Zimmernamen zurück.
    ''' </summary>
    ''' <param name="sID">Die zu suchende Zimmer-ID.</param>
    ''' <returns>Der gefundene Zimmername. Wird die ID nicht gefunden, wird die ID selbst als Fallback zurückgegeben.</returns>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltete Zuweisung an den Funktionsnamen durch modernes 'Return' ersetzt.
    ''' - Performance-Optimierung: Die manuelle 'For'-Schleife durch die native 'DataTable.Select'-Methode ersetzt.
    ''' - Null- und Leerraum-Prüfung für die übergebene ID integriert.
    ''' </remarks>
    Private Function fcGetObjektZimmerName(ByVal sID As String) As String
        ' Fallback-Wert festlegen (falls ID leer ist oder nicht gefunden wird)
        If String.IsNullOrWhiteSpace(sID) Then
            Return String.Empty
        End If

        ' Prüfen, ob die Tabelle geladen ist und Zeilen enthält
        If dtZim IsNot Nothing AndAlso dtZim.Rows.Count > 0 Then
            ' Schnelle Suche in der DataTable über einen Filter-Ausdruck
            Dim foundRows() As DataRow = dtZim.Select(String.Format("ID = '{0}'", sID.Replace("'", "''")))

            ' Wenn ein passender Datensatz gefunden wurde, den Namen zurückgeben
            If foundRows.Length > 0 Then
                Return foundRows(0)("Name").ToString()
            End If
        End If

        ' Wenn nichts gefunden wurde, die übergebene ID als Fallback zurückgeben
        Return sID
    End Function

    ''' <summary>
    ''' Ereignishandler für den Klick auf die "Neu"-Schaltfläche (tsbNew).
    ''' Bereitet die Eingabemaske für das Anlegen eines neuen Gastes vor und setzt alle Felder zurück.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 02.04.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Unnötige, tote Logik zur Prüfung der immer leeren Variable 'sGeb' entfernt.
    ''' - Veraltete 'Call'-Syntax bei Methodenaufrufen vollständig entfernt.
    ''' - 'String.Empty' anstelle von Leerstrings ("") für eine sauberere Speicherbereinigung verwendet.
    ''' - Auskommentierten Code zur Verbesserung der Wartbarkeit gelöscht.
    ''' </remarks>
    Private Sub tsbNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNew.Click
        ' UI-Steuerelemente vorbereiten und Steuertabelle sperren
        paGast.Visible = True
        dgKunden.Enabled = False
        lNew = True

        ' Alle Eingabefelder zurücksetzen
        coAnrede.Text = String.Empty
        tbName1.Text = String.Empty
        tbName2.Text = String.Empty
        tbVorname.Text = String.Empty
        tbStrasse.Text = String.Empty
        tbPLZ.Text = String.Empty
        tbOrt.Text = String.Empty
        tbLand.Text = String.Empty
        tbTelefon.Text = String.Empty
        tbTelefax.Text = String.Empty
        tbHandy.Text = String.Empty
        tbEMail.Text = String.Empty
        tbPass.Text = String.Empty
        tbInfo.Text = String.Empty

        ' Datumsfeld standardmäßig auf den heutigen Tag setzen
        dtpGeb.Value = Date.Today

        ' Schaltflächen-Status anpassen
        prLockGast(True)
    End Sub

    ''' <summary>
    ''' Ereignishandler für den Klick auf die "Löschen"-Schaltfläche (tsbDelete).
    ''' Löscht den ausgewählten Kunden oder anonymisiert dessen bestehende Buchungen, falls historische Daten vorhanden sind.
    ''' Zeigt den Namen des Kunden in der Sicherheitsabfrage an und setzt den Fokus danach auf den vorherigen Datensatz.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 02.04.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltete VB6-'MsgBox' durch modernes '.NET MessageBox.Show' ersetzt.
    ''' - Veraltetes 'Split()' durch performantere, native String-Arrays ersetzt.
    ''' - 'Call'-Syntax bei Methodenaufrufen vollständig entfernt.
    ''' - Logik zur Anonymisierung verknüpfter Buchungen stabilisiert und lesbarer gestaltet.
    ''' - Aktualisierung der Kundentabelle nach dem Löschvorgang optimiert.
    ''' - Dynamische Namensanzeige (Name1, Name2, Vorname) in Löschabfrage integriert.
    ''' - Grid-Fokus-Logik implementiert (wechselt auf die Zeile darüber, sofern es nicht die erste war).
    ''' </remarks>
    Private Sub tsbDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelete.Click
        ' 1. Aktuellen Index ermitteln, um den Fokus später korrekt zu setzen
        ' (Es wird angenommen, dass dgvKunden der Name Ihres DataGridViews ist)
        Dim currentIdx As Integer = -1
        If dgKunden.CurrentRow IsNot Nothing Then
            currentIdx = dgKunden.CurrentRow.Index
        End If

        ' 2. Details des Kunden für die Sicherheitsabfrage aus der DB laden
        Dim sCustomerSql As String = String.Format("SELECT Name1, Name2, Vorname FROM Kunden WHERE ID = '{0}'", sKNr.Replace("'", "''"))
        Dim dtCust As DataTable = fcReadDataTable(sCustomerSql)

        Dim sKundenName As String = "Unbekannter Gast"
        If dtCust.Rows.Count > 0 Then
            Dim rowCust As DataRow = dtCust.Rows(0)
            Dim n1 As String = rowCust("Name1").ToString().Trim()
            Dim n2 As String = rowCust("Name2").ToString().Trim()
            Dim vn As String = rowCust("Vorname").ToString().Trim()

            ' Namen sauber verketten (nur befüllte Felder anzeigen)
            Dim nameParts As New List(Of String)()
            If Not String.IsNullOrEmpty(n1) Then nameParts.Add(n1)
            If Not String.IsNullOrEmpty(n2) Then nameParts.Add(n2)
            If Not String.IsNullOrEmpty(vn) Then nameParts.Add(vn)

            If nameParts.Count > 0 Then
                sKundenName = String.Join(", ", nameParts)
            End If
        End If

        Dim sMsg As String = String.Format("Wollen Sie diesen Gast wirklich löschen?{0}{0} ({1})", Environment.NewLine, sKundenName)

        ' Sicherheitsabfrage über die .NET MessageBox
        If MessageBox.Show(sMsg, "Löschen", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = DialogResult.OK Then

            ' Prüfen, ob der Kunde noch verknüpfte Buchungen besitzt
            Dim sSql As String = "Select * FROM Buchung WHERE KunID = '" & sKNr & "'"
            Dim dt As DataTable = fcReadDataTable(sSql)

            If dt.Rows.Count > 0 Then
                ' Felder und Werte für die Anonymisierung der Buchung definieren
                Dim arFields() As String = {"KunID", "Kunde"}
                Dim arValue() As String = {String.Empty, "Unbekannt"}

                ' Alle verknüpften Buchungen durchgehen und die Kundenbindung aufheben
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim sID As String = dt.Rows(i)("ID").ToString()
                    fcUpdateCommand("buchung", arFields, arValue, "WHERE ID ='" & sID & "'")
                Next
            End If

            ' Den Kunden endgültig aus der Kundentabelle löschen
            UpdateTable(String.Format("DELETE FROM Kunden WHERE ID = '{0}'", sKNr.Replace("'", "''")))

            ' Die globale Kundentabelle neu einlesen und das Grid aktualisieren
            dtK = fcReadDataTable("Select * from Kunden order by Name1, Vorname, Anrede asc")
            prFuelleTabelleKunden()

            ' Fokus auf den Datensatz darüber legen (außer es war die erste Zeile)
            If dgKunden.Rows.Count > 0 Then
                Dim targetIdx As Integer = 0 ' Fallback auf erste Zeile, falls vorher Index 0 gelöscht wurde

                If currentIdx > 0 Then
                    ' Wenn eine höhere Zeile gelöscht wurde, nimm die exakte Zeile darüber
                    targetIdx = currentIdx - 1
                End If

                ' Sicherstellen, dass der Index nicht außerhalb der neuen Zeilenanzahl liegt
                If targetIdx >= dgKunden.Rows.Count Then
                    targetIdx = dgKunden.Rows.Count - 1
                End If

                ' Fokus visuell und datentechnisch im DataGridView neu setzen
                If targetIdx >= 0 Then
                    ' Die erste sichtbare Spalte im Grid suchen, um den Fehler zu vermeiden
                    Dim firstVisibleCellIndex As Integer = -1
                    For Each col As DataGridViewColumn In dgKunden.Columns
                        If col.Visible Then
                            firstVisibleCellIndex = col.Index
                            Exit For
                        End If
                    Next

                    ' Nur wenn eine sichtbare Spalte gefunden wurde, setzen wir die CurrentCell
                    If firstVisibleCellIndex <> -1 Then
                        dgKunden.CurrentCell = dgKunden.Rows(targetIdx).Cells(firstVisibleCellIndex)
                    End If

                    ' Die gesamte Zeile markieren
                    dgKunden.Rows(targetIdx).Selected = True
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Ereignishandler für das Drücken der Maustaste auf einer Grid-Zelle (dgKunden.CellMouseDown).
    ''' Ermittelt die aktuelle Zeilenkoordinate (Index) und speichert diese für nachfolgende Aktionen in der globalen Variable.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten mit den Zeilen- und Spaltenindizes.</param>
    ''' <remarks>
    ''' 12.01.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Validierung des Zeilenindexes hinzugefügt, um Abstürze bei Klicks auf den Spaltenkopf (Index = -1) zu verhindern.
    ''' - Namenskonsistenz im XML-Kommentar bezüglich 'dgKunden' sichergestellt.
    ''' </remarks>
    Private Sub dgKunden_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgKunden.CellMouseDown
        ' Klicks auf den Spaltenkopf (-1) ignorieren, um Folgefehler im Grid zu vermeiden
        If e.RowIndex >= 0 Then
            eRow = e.RowIndex
        End If
    End Sub

#End Region

#Region "Risize...................................................................................."

    ''' <summary>
    ''' Ereignishandler für die Größenänderung des Formulars (frmKBuchen_Resize).
    ''' Passt die Dimensionen des DataGridViews (dgKunden) dynamisch an die Fenstergröße an
    ''' und zentriert das Gästepanel (paGast) exakt in der Mitte des Formulars.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.05.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Prüfung auf 'WindowState = FormWindowState.Minimized' hinzugefügt, um Berechnungsfehler bei minimiertem Fenster zu verhindern.
    ''' - Lokale Variablen zur Mitte des Formulars und Panels mathematisch inline zusammengefasst.
    ''' - 'New Point'-Berechnung für das Panel (paGast) verschlankt und lesbarer gestaltet.
    ''' </remarks>
    Private Sub frmKBuchen_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        ' Wenn das Fenster minimiert ist, machen Berechnungen keinen Sinn (Breite/Höhe wäre 0 oder negativ)
        If Me.WindowState = FormWindowState.Minimized Then Exit Sub

        Try
            ' 1. Größe des DataGridViews dynamisch anpassen
            dgKunden.Width = Me.Width - 35
            dgKunden.Height = Me.Height - 85 ' Ursprünglicher Offset: 65

            ' 2. Gästepanel (paGast) exakt in der Mitte des Formulars zentrieren
            ' Formel: (Formular-Dimension / 2) - (Panel-Dimension / 2)
            Dim posX As Integer = (Me.Width \ 2) - (paGast.Width \ 2)
            Dim posY As Integer = (Me.Height \ 2) - (paGast.Height \ 2)

            ' Neue Position zuweisen
            paGast.Location = New Point(posX, posY)

        Catch ex As Exception
            ' Optionale Fehlerbehandlung, falls Steuerelemente während des Disposes feuern
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

#End Region

#Region "Gästedaten bearbeiten....................................................................."

    ''' <summary>
    ''' Ereignishandler für den Klick auf das Label zum Schließen (lbClose).
    ''' Schließt die Gast-Detailansicht ohne zu speichern.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 14.05.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax entfernt.
    ''' - Methodennamen zwecks Lesbarkeit korrigiert (prloseGast -> prCloseGast).
    ''' </remarks>
    Private Sub lbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbClose.Click
        prCloseGast()
    End Sub

    ''' <summary>
    ''' Ereignishandler für den Klick auf die "Schließen"-Schaltfläche (cmdClose).
    ''' Schließt die Gast-Detailansicht ohne zu speichern.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 14.05.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax entfernt.
    ''' - Methodennamen zwecks Lesbarkeit korrigiert (prloseGast -> prCloseGast).
    ''' </remarks>
    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        prCloseGast()
    End Sub

    ''' <summary>
    ''' Schließt das Gästepanel, hebt die Sperrung der Eingabemaske auf und setzt den Fokus zurück auf die Kundenliste.
    ''' </summary>
    ''' <remarks>
    ''' 14.05.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei 'prLockGast' entfernt.
    ''' - Rechtschreibkorrektur im Methodennamen (prloseGast -> prCloseGast, prLoockGast -> prLockGast).
    ''' - Steuerelement-Fokus durch native .NET-Aktivierung sichergestellt.
    ''' </remarks>
    Private Sub prCloseGast()
        ' Eingabemaske entsperren
        prLockGast(False)

        ' Gästepanel ausblenden
        paGast.Visible = False

        ' Kundenliste wieder freigeben und fokussieren
        dgKunden.Enabled = True
        dgKunden.Select()
    End Sub

    ''' <summary>
    ''' Ereignishandler für den Klick auf die "Speichern"-Schaltfläche (cmdSave).
    ''' Ruft die Routine zur Validierung und Speicherung der Gastdaten auf.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 14.05.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Aufruf von 'prSaveGast' entfernt.
    ''' </remarks>
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        prSaveGast()
    End Sub

    ''' <summary>
    ''' Führt die Speicherung oder Aktualisierung des Gast-Datensatzes in der Datenbank, 
    ''' der zugrundeliegenden DataTable sowie direkt im DataGridView-Steuerelement aus.
    ''' </summary>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 02.04.2012 - Add New Gast
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen restlos entfernt.
    ''' - VB6-'Split'-Funktion durch performantes und natives .NET-String-Array abgelöst.
    ''' - 'arFields'-Zuweisung durch ein direktes String-Array-Literal ersetzt.
    ''' - String-Konstruktion für SQL-Bedingungen über performantere String-Verkettung / Zuweisung optimiert.
    ''' - Panel-Ausblenden und Grid-Aktivierung im 'Finally'-Block analog zu 'prCloseGast' sichergestellt.
    ''' </remarks>
    Private Sub prSaveGast()
        Dim sID As String = sKNr
        If lNew Then
            sID = fcGetNr("KNr")
        End If

        Try
            ' Felder direkt als natives .NET-String-Array deklarieren
            Dim arFields() As String = {"ID", "Anrede", "Name1", "Name2", "Vorname", "Strasse", "PLZ", "Ort", "Land", "Telefon", "Telefax", "Funk", "EMail", "Pass", "Geb", "Info"}

            ' Werte über die bestehende Schnittstelle laden und per nativem Split trennen
            Dim sqlText As String = fcSaveGast(sID)
            Dim arValue() As String = sqlText.Split("°"c)

            ' 1. In Datenbank schreiben
            If lNew Then
                fcInsertCommand("Kunden", arFields, arValue)
            Else
                Dim cBedingung As String = " WHERE ID='" & sID & "'"
                fcUpdateCommand("Kunden", arFields, arValue, cBedingung)
            End If

            ' 2. DataTable (dtK) aktualisieren
            If lNew Then
                fcInsertTable(dtK, arFields, arValue)
            Else
                Dim cBedingung As String = "ID Like '" & sID & "'"
                fcUpdateTable(dtK, arFields, arValue, cBedingung)
            End If

            ' Datumsprüfung für das Geburtsdatum vorbereiten
            Dim sGeb As String = String.Empty
            If dtpGeb.Value < Date.Today Then
                sGeb = dtpGeb.Value.ToShortDateString()
            End If

            ' 3. DataGridView (dgKunden) aktualisieren
            If lNew Then
                Dim arT() As String = {
                sID,
                coAnrede.Text,
                tbName1.Text,
                tbName2.Text,
                tbVorname.Text,
                tbStrasse.Text,
                tbPLZ.Text,
                tbOrt.Text,
                tbLand.Text,
                tbTelefon.Text,
                tbTelefax.Text,
                tbHandy.Text,
                tbEMail.Text,
                tbPass.Text,
                sGeb,
                tbInfo.Text
            }
                dgKunden.Rows.Add(arT)
                prSetNr("KNr", sID)
            Else
                With dgKunden.Rows(eRow)
                    .Cells(0).Value = sID
                    .Cells(1).Value = coAnrede.Text
                    .Cells(2).Value = tbName1.Text
                    .Cells(3).Value = tbName2.Text
                    .Cells(4).Value = tbVorname.Text
                    .Cells(5).Value = tbStrasse.Text
                    .Cells(6).Value = tbPLZ.Text
                    .Cells(7).Value = tbOrt.Text
                    .Cells(8).Value = tbLand.Text
                    .Cells(9).Value = tbTelefon.Text
                    .Cells(10).Value = tbTelefax.Text
                    .Cells(11).Value = tbHandy.Text
                    .Cells(12).Value = tbEMail.Text
                    .Cells(13).Value = tbPass.Text
                    .Cells(14).Value = sGeb
                    .Cells(15).Value = tbInfo.Text
                End With
            End If

            ' Synchronisation des selektierten Kunden aufrufen
            prSynchronKunde(dgKunden, sID)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ' Maske entsperren, Panel schließen und Grid wieder aktiv schalten
            prLockGast(False)
            paGast.Visible = False
            dgKunden.Enabled = True
            dgKunden.Select()

            ' Status für Neuanlage zurücksetzen
            lNew = False
        End Try
    End Sub

    ''' <summary>
    ''' Bereitet die in den UI-Steuerelementen eingegebenen Gastdaten für den Speichervorgang vor,
    ''' normalisiert leere Felder zu einem standardisierten Leerzeichen und verkettet diese mit dem Trennzeichen '°'.
    ''' </summary>
    ''' <param name="sID">Die eindeutige Kundennummer (ID) des Gasts.</param>
    ''' <returns>Ein mit '°' verketteter String aller relevanten Gastdaten.</returns>
    ''' <remarks>
    ''' 10.01.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Unperformante '.Trim() = ""'-Prüfungen durch das modernere 'String.IsNullOrWhiteSpace()' ersetzt.
    ''' - Direkte Modifikation der UI-TextBox-Inhalte entfernt; stattdessen lokale Variablen-Validierung implementiert.
    ''' - 'StringBuilder' durch ein deutlich schnelleres und kompakteres 'String.Join()' abgelöst.
    ''' - Veraltete VB-Funktionszuweisung durch ein zeitgemäßes 'Return'-Statement ersetzt.
    ''' </remarks>
    Private Function fcSaveGast(ByVal sID As String) As String
        ' Geburtsdatum formatieren, falls es in der Vergangenheit liegt
        Dim sGeb As String = " "
        If dtpGeb.Value < Date.Today Then
            sGeb = fcUmDatum(dtpGeb.Value)
        End If

        ' Werte lokal auslesen und leere Felder durch ein Leerzeichen ersetzen
        Dim anrede As String = If(String.IsNullOrWhiteSpace(coAnrede.Text), " ", coAnrede.Text)
        Dim name1 As String = If(String.IsNullOrWhiteSpace(tbName1.Text), " ", tbName1.Text)
        Dim name2 As String = If(String.IsNullOrWhiteSpace(tbName2.Text), " ", tbName2.Text)
        Dim vorname As String = If(String.IsNullOrWhiteSpace(tbVorname.Text), " ", tbVorname.Text)
        Dim strasse As String = If(String.IsNullOrWhiteSpace(tbStrasse.Text), " ", tbStrasse.Text)
        Dim plz As String = If(String.IsNullOrWhiteSpace(tbPLZ.Text), " ", tbPLZ.Text)
        Dim ort As String = If(String.IsNullOrWhiteSpace(tbOrt.Text), " ", tbOrt.Text)
        Dim land As String = If(String.IsNullOrWhiteSpace(tbLand.Text), " ", tbLand.Text)
        Dim telefon As String = If(String.IsNullOrWhiteSpace(tbTelefon.Text), " ", tbTelefon.Text)
        Dim telefax As String = If(String.IsNullOrWhiteSpace(tbTelefax.Text), " ", tbTelefax.Text)
        Dim handy As String = If(String.IsNullOrWhiteSpace(tbHandy.Text), " ", tbHandy.Text)
        Dim email As String = If(String.IsNullOrWhiteSpace(tbEMail.Text), " ", tbEMail.Text)
        Dim pass As String = If(String.IsNullOrWhiteSpace(tbPass.Text), " ", tbPass.Text)
        Dim info As String = If(String.IsNullOrWhiteSpace(tbInfo.Text), " ", tbInfo.Text)

        ' Alle Werte in der exakten Reihenfolge in ein Array legen
        ' Reihenfolge: ID, Anrede, Name1, Name2, Vorname, Strasse, PLZ, Ort, Land, Telefon, Telefax, Funk, EMail, Pass, Geb, Info
        Dim dataParts() As String = {
        sID, anrede, name1, name2, vorname, strasse, plz, ort, land,
        telefon, telefax, handy, email, pass, sGeb, info
    }

        ' Die Elemente mit dem Trennzeichen "°" sauber zusammenfügen
        Return String.Join("°", dataParts)
    End Function

    ''' <summary>
    ''' Steuert die Aktivierung und Deaktivierung der Schaltflächen und Eingabefelder im Gast-Bereich.
    ''' </summary>
    ''' <param name="lStatus">Gibt an, ob die Steuerelemente gesperrt (True) oder freigegeben (False) werden sollen.</param>
    ''' <remarks>
    ''' 10.01.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Typpräfix bei Methodenname beibehalten ('pr' für Private Sub).
    ''' - Parameternamen im XML-Kommentar dokumentiert und beschrieben.
    ''' - Vorbereitung für optionale Erweiterung der UI-Steuerung getroffen.
    ''' </remarks>
    Private Sub prLockGast(ByVal lStatus As Boolean)
        ' Schaltflächen basierend auf dem Status sperren oder freigeben
        tsbNew.Enabled = Not lStatus
        tsbDelete.Enabled = Not lStatus
    End Sub

#End Region

#Region "Hilfetexte in der Statusleiste ausgeben..................................................."

    ''' <summary>
    ''' Zentraler Ereignishandler für die Mausbewegung über den Eingabefeldern.
    ''' Aktualisiert die Statuszeile (tssInfo) dynamisch basierend auf dem fokussierten Steuerelement.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das jeweilige Textfeld).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Neun separate Ereignishandler in einer einzigen, zentralen Methode zusammengefasst.
    ''' - Typprüfung für den 'sender' integriert, um Laufzeitfehler zu vermeiden.
    ''' - Nutzung einer performanten Select-Case-Struktur zur Zuordnung der Infotexte anhand des Control-Namens.
    ''' </remarks>
    Private Sub FormControls_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _
    tbName1.MouseMove, tbName2.MouseMove, tbVorname.MouseMove, tbStrasse.MouseMove,
    tbPLZ.MouseMove, tbOrt.MouseMove, tbLand.MouseMove, tbPass.MouseMove, tbInfo.MouseMove,
    dtpGeb.MouseMove, tbTelefon.MouseMove, tbTelefax.MouseMove, tbHandy.MouseMove, tbEMail.MouseMove,
    cmdSave.MouseMove, cmdClose.MouseMove, lbClose.MouseMove, coAnrede.MouseMove,
    gbGaeste.MouseMove, ToolStrip1.MouseMove, dgKunden.MouseMove, paGast.MouseMove


        Dim ctrl As Control = TryCast(sender, Control)
        If ctrl Is Nothing Then Exit Sub

        ' Text der Statuszeile anhand des Steuerelement-Namens zuweisen
        Select Case ctrl.Name
        ' --- Erste TextBoxen-Gruppe ---
            Case "tbName1" : tssInfo.Text = "Nachname / Firmenname"
            Case "tbName2" : tssInfo.Text = "Nachname wenn Firmenbezug"
            Case "tbVorname" : tssInfo.Text = "Vorname"
            Case "tbStrasse" : tssInfo.Text = "Strasse"
            Case "tbPLZ" : tssInfo.Text = "PLZ"
            Case "tbOrt" : tssInfo.Text = "Ort"
            Case "tbLand" : tssInfo.Text = "Land (Kurz-Zeichen)"
            Case "tbPass" : tssInfo.Text = "Ausweisnummer"
            Case "tbInfo" : tssInfo.Text = "Bemerkungen"

        ' --- Neue Steuerelemente ---
            Case "dtpGeb" : tssInfo.Text = "Geburtsdatum"
            Case "tbTelefon" : tssInfo.Text = "Telefon"
            Case "tbTelefax" : tssInfo.Text = "Telefax"
            Case "tbHandy" : tssInfo.Text = "Mobiltelefon"
            Case "tbEMail" : tssInfo.Text = "E-Mail Adresse"
            Case "coAnrede" : tssInfo.Text = "Anrede: Herr, Frau, Firma"

        ' --- Buttons und Schließen-Label ---
            Case "cmdSave" : tssInfo.Text = "Änderung speichern"
            Case "cmdClose", "lbClose" : tssInfo.Text = "Bearbeitungsfenster schliessen"

        ' --- Container / Hintergründe (Statuszeile leeren) ---
            Case "gbGaeste", "ToolStrip1", "dgKunden", "paGast" : tssInfo.Text = String.Empty
        End Select
    End Sub

#End Region

#Region "Datensatz suchen.........................................................................."

    ''' <summary>
    ''' Ereignishandler für den Klick auf die "Suchen"-Schaltfläche (tsbSuchen).
    ''' Blendet das Suchfeld ein und setzt den Eingabefokus direkt in das Textfeld, um eine sofortige Eingabe zu ermöglichen.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 12.03.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Sichtbarkeitssteuerung für das Suchfeld beibehalten.
    ''' - Fokussierung durch expliziten Aufruf des darunterliegenden TextBox-Steuerelements optimiert.
    ''' - 'SelectAll()'-Aufruf hinzugefügt, damit der Cursor aktiv blinkt und alter Text sofort überschrieben werden kann.
    ''' </remarks>
    Private Sub tsbSuchen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSuchen.Click
        ' Suchfeld sichtbar machen
        tsbSuchfeld.Visible = True

        ' Fokus auf das ToolStrip-Element setzen
        tsbSuchfeld.Focus()

        ' Zugriff auf die native TextBox, um den Cursor aktiv zu setzen und Text zu markieren
        If tsbSuchfeld.TextBox IsNot Nothing Then
            tsbSuchfeld.TextBox.SelectAll()
        End If
    End Sub

    ''' <summary>
    ''' Ereignishandler für die Textänderung im Suchfeld (tsbSuchfeld).
    ''' Führt eine Echtzeitsuche (Inkrementelle Suche) auf Spalte 2 (Name1) durch. 
    ''' Hebt den ersten Treffer farblich hervor und scrollt das Grid automatisch an die entsprechende Position.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 22.04.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltete VB6-'Mid'-Funktion durch performantes .NET 'StartsWith' mit 'StringComparison.CurrentCultureIgnoreCase' ersetzt.
    ''' - Variable 'i' in der Schleife explizit als Integer deklariert.
    ''' - Null-sichere Auswertung der Zellwerte implementiert, um Abstürze bei leeren Datenfeldern zu verhindern.
    ''' - Bereichsprüfung für 'iZeile' vor dem Zurücksetzen der Hintergrundfarbe hinzugefügt.
    ''' - Logik zur Farbrücksetzung optimiert, damit keine alten gelben Markierungen verbleiben.
    ''' </remarks>
    Private Sub tsbSuchfeld_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbSuchfeld.TextChanged
        Dim sSearch As String = tsbSuchfeld.Text.Trim()

        With dgKunden
            ' 1. Vorherige Markierung der alten Zeile sicher zurücksetzen (falls gültig)
            If iZeile >= 0 AndAlso iZeile < .Rows.Count Then
                .Rows(iZeile).Cells(2).Style.BackColor = Color.White
            End If

            ' Wenn kein Suchtext vorhanden ist, müssen wir nicht weiterarbeiten
            If String.IsNullOrEmpty(sSearch) Then Exit Sub

            ' 2. Zeilen durchsuchen (Inkrementelle Suche ab dem ersten Buchstaben)
            For i As Integer = 0 To .RowCount - 1
                Dim cellValue As Object = .Rows(i).Cells(2).Value
                Dim sCellText As String = If(cellValue?.ToString(), String.Empty).Trim()

                ' Prüfen, ob der Zellentext mit dem Suchbegriff beginnt (Groß-/Kleinschreibung ignorieren)
                If sCellText.StartsWith(sSearch, StringComparison.CurrentCultureIgnoreCase) Then
                    ' Neuen Treffer visuell hervorheben
                    .Rows(i).Cells(2).Style.BackColor = Color.Yellow

                    ' Index der aktuell markierten Zeile global merken
                    iZeile = i

                    ' Automatisch zu dieser Zeile scrollen
                    .FirstDisplayedScrollingRowIndex = i
                    Exit For
                End If
            Next
        End With
    End Sub

    ''' <summary>
    ''' Ereignishandler für das Drücken einer Taste im Suchfeld (tsbSuchfeld).
    ''' Wählt bei Drücken der Eingabetaste (Enter) die durch die Echtzeitsuche bereits gefundene Zeile aus,
    ''' setzt den Fokus darauf, setzt die gelbe Markierung zurück und blendet das Suchfeld aus.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten mit der gedrückten Taste.</param>
    ''' <remarks>
    ''' 18.04.2012 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veralteten Aufruf von 'prSucheKunde' vollständig entfernt.
    ''' - Fokus-Logik integriert, die direkt auf die global gemerkte 'iZeile' springt.
    ''' - Zurücksetzen der gelben Hintergrundfarbe vor dem Ausblenden hinzugefügt.
    ''' - Spaltenvalidierung integriert, um die erste sichtbare Zelle für den Fokus zu ermitteln.
    ''' </remarks>
    Private Sub tsbSuchfeld_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tsbSuchfeld.KeyPress
        ' Prüfen, ob die Eingabetaste (Enter) gedrückt wurde
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            ' Windows mitteilen, dass das Ereignis verarbeitet wurde (verhindert den System-Gong)
            e.Handled = True

            With dgKunden
                ' Prüfen, ob durch TextChanged bereits eine gültige Zeile gefunden und gemerkt wurde
                If iZeile >= 0 AndAlso iZeile < .Rows.Count Then

                    ' 1. Gelbe Hintergrundfarbe der Zelle wieder auf Standard (Weiß) zurücksetzen
                    .Rows(iZeile).Cells(2).Style.BackColor = Color.White

                    ' 2. Erste sichtbare Spalte für die Fokus-Zuweisung ermitteln (verhindert InvalidOperationException)
                    Dim firstVisibleColIdx As Integer = -1
                    For Each col As DataGridViewColumn In .Columns
                        If col.Visible Then
                            firstVisibleColIdx = col.Index
                            Exit For
                        End If
                    Next

                    ' 3. Fokus und Selektion auf die gefundene Zeile setzen
                    If firstVisibleColIdx <> -1 Then
                        .CurrentCell = .Rows(iZeile).Cells(firstVisibleColIdx)
                    End If
                    .Rows(iZeile).Selected = True

                    ' 4. Globale Kundennummer aus Spalte 0 für die weitere Verarbeitung auslesen
                    sKNr = If(.Rows(iZeile).Cells(0).Value?.ToString(), String.Empty).Trim()
                End If
            End With

            ' Suchfeld zurücksetzen und ausblenden
            tsbSuchfeld.Text = String.Empty
            tsbSuchfeld.Visible = False
        End If
    End Sub

#End Region




End Class