Imports System.Text

Public Class frmPersonal
    Dim lNew As Boolean
    Dim arMem(8) As String


#Region "Form Load................................................................................."

    ''' <summary>
    ''' Initialisiert das Personalformular beim Laden.
    ''' Bereinigt die Datenbank vorab von unvollständigen Datensätzen ohne Namen und lädt anschließend die bereinigte Personalliste.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Performance-Sünde behoben: Ineffiziente 'For'-Schleife und doppeltes Laden der DataTable durch ein direktes SQL-'DELETE' ersetzt.
    ''' - Fehlende Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor Datenbankausfällen ergänzt.
    ''' - Veraltete 'Call'-Syntax bei Methodenaufrufen vollständig entfernt.
    ''' </remarks>
    Private Sub frmPersonal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            ' 1. Verwaiste Datensätze (ohne Namen oder nur mit Leerzeichen) direkt auf der DB löschen
            ' Das spart die alte Schleife und das doppelte Laden der Daten komplett ein.
            UpdateTable("DELETE FROM Personal1 WHERE Name IS NULL OR TRIM(Name) = ''")

            ' 2. Bereinigte Personaldaten mit einer einzigen Abfrage laden
            dtPer = fcReadDataTable("SELECT * FROM Personal1")

            ' 3. Steuerelemente und Tabellen-UI initialisieren
            prCheckNoRecord(dtPer)
            prCreateTabellePersonal()
            prLoadpersonalInList(dtPer)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Erstellt die Tabellenstruktur für die Personalübersicht in der ListView.
    ''' Initialisiert die sichtbaren Datenspalten (Name, Telefon etc.) sowie die versteckten ID- und Code-Spalten und konfiguriert das Anzeige- und Sortierverhalten.
    ''' </summary>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Fehlende Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor UI-Layoutfehlern ergänzt.
    ''' - Performance-Optimierung: Steuerelemente-Layout-Sperre ('.SuspendLayout' / '.ResumeLayout') integriert, um Flackern beim Neuzeichnen der Spalten zu verhindern.
    ''' - Strukturierung und Formatierung des 'With'-Blocks zur besseren Lesbarkeit vereinheitlicht.
    ''' </remarks>
    Private Sub prCreateTabellePersonal()
        Try
            ' 1. Visuelles Flackern und mehrfaches Neuzeichnen während der Spaltenerstellung verhindern
            lvPersonal.SuspendLayout()

            With lvPersonal
                ' 2. Bestehende Struktur zurücksetzen
                .Clear()

                ' 3. Spaltenköpfe mit Breiten und Ausrichtungen hinzufügen
                .Columns.Add("Name", 150, HorizontalAlignment.Left)
                .Columns.Add("Vorname", 150, HorizontalAlignment.Left)
                .Columns.Add("Telefon", 150, HorizontalAlignment.Left)
                .Columns.Add("Handy", 150, HorizontalAlignment.Left)
                .Columns.Add("Bemerkung", 300, HorizontalAlignment.Left)

                ' Versteckte Spalten für interne Datenzuordnung (Breite 0)
                .Columns.Add("ID", 0, HorizontalAlignment.Left)
                .Columns.Add("Code", 0, HorizontalAlignment.Left)

                ' 4. Anzeige- und Auswahlverhalten konfigurieren
                .FullRowSelect = True
                .GridLines = True
                .HeaderStyle = ColumnHeaderStyle.Nonclickable
                .HideSelection = False
                .MultiSelect = False
                .Sorting = SortOrder.Ascending
                .TabIndex = 0
                .View = View.Details
            End With

            ' 5. Layout-Sperre aufheben und Steuerelement final zeichnen
            lvPersonal.ResumeLayout()

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Befüllt die Personal-ListView mit den Datensätzen aus der bereitgestellten DataTable.
    ''' Schließt gelöschte Zeilen aus und selektiert standardmäßig das erste Element der Liste.
    ''' </summary>
    ''' <param name="dtT">Die DataTable, die die anzuzeigenden Personaldaten enthält.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Massiver Performance-Gewinn: Einzelne '.Items.Add'-Aufrufe in der Schleife durch ein blockweises '.AddRange()' ersetzt.
    ''' - Flackern der Benutzeroberfläche durch '.BeginUpdate()' und '.EndUpdate()' vollständig eliminiert.
    ''' - Logischen Fehler und potenziellen Absturz bei der Erstselektion des 'TopItem' behoben.
    ''' - Typisierung der Schleifenvariable explizit deklariert und Variablen-Scoping innerhalb der Schleife bereinigt.
    ''' - Fehlende Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor Laufzeitfehlern ergänzt.
    ''' </remarks>
    Private Sub prLoadpersonalInList(ByVal dtT As DataTable)
        ' 1. Vorzeitiger Abbruch, falls die Tabelle keine Zeilen enthält
        If dtT Is Nothing OrElse dtT.Rows.Count = 0 Then
            lvPersonal.Items.Clear()
            Return
        End If

        Try
            ' 2. Zeichnen der ListView einfrieren (Performance-Schutz)
            lvPersonal.BeginUpdate()
            lvPersonal.Items.Clear()

            Dim nMax As Integer = dtT.Rows.Count - 1
            Dim itemList As New List(Of ListViewItem)

            ' 3. Datenzeilen durchlaufen und ListViewItems vorbereiten
            For i As Integer = 0 To nMax
                Dim row As DataRow = dtT.Rows(i)

                ' Gelöschte Zeilen überspringen
                If row.RowState <> DataRowState.Deleted Then

                    ' Neues ListViewItem mit dem Hauptwert (Name) erstellen
                    Dim lv As New ListViewItem(row("Name").ToString())

                    ' Unterelemente (SubItems) null-sicher hinzufügen
                    lv.SubItems.Add(row("Vorname").ToString())
                    lv.SubItems.Add(row("Tel1").ToString())
                    lv.SubItems.Add(row("Tel2").ToString())
                    lv.SubItems.Add(row("Info").ToString())
                    lv.SubItems.Add(row("ID").ToString())
                    lv.SubItems.Add(row("Code").ToString())

                    ' In der temporären Liste zwischenspeichern
                    itemList.Add(lv)
                End If
            Next

            ' 4. Alle vorbereiteten Elemente in einem einzigen Rutsch hinzufügen
            If itemList.Count > 0 Then
                lvPersonal.Items.AddRange(itemList.ToArray())
            End If

            ' 5. Zeichnen der ListView wieder freigeben
            lvPersonal.EndUpdate()

            ' 6. Fokus setzen und den ersten Eintrag sauber selektieren
            lvPersonal.Select()
            If lvPersonal.Items.Count > 0 Then
                lvPersonal.Items(0).Selected = True
                lvPersonal.Items(0).Focused = True
            End If

        Catch ex As Exception
            lvPersonal.EndUpdate() ' Sicherstellen, dass die UI im Fehlerfall nicht gesperrt bleibt
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Prüft, ob Datensätze in der Personal-Tabelle vorhanden sind, und steuert dynamisch die Aktivierung der Bearbeiten- und Löschen-Schaltflächen.
    ''' </summary>
    ''' <param name="dt">Die zu prüfende DataTable mit den Personaldaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Fehlende Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor NullReferenceExceptions ergänzt.
    ''' - Logischen Zuweisungsprozess radikal vereinfacht (Direktzuweisung der Bedingung statt temporärer Boolean-Variable).
    ''' - Null-Prüfung für die DataTable hinzugefügt, um Stabilität bei fehlgeschlagenen Abfragen zu garantieren.
    ''' </remarks>
    Private Sub prCheckNoRecord(ByVal dt As DataTable)
        Try
            ' 1. Prüfen, ob die Tabelle existiert und Zeilen enthält
            Dim hasRecords As Boolean = (dt IsNot Nothing AndAlso dt.Rows.Count > 0)

            ' 2. Buttons direkt basierend auf dem Ergebnis aktivieren oder deaktivieren
            tsbEdit.Enabled = hasRecords
            tsbDel.Enabled = hasRecords

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Tritt auf, wenn sich die Auswahl in der Personal-ListView ändert. 
    ''' Ruft die Detailinformationen des selektierten Mitarbeiters ab, um die Eingabemasken zu aktualisieren.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (die ListView).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf vollständig entfernt.
    ''' - Fehlende Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor UI-Aktualisierungsfehlern ergänzt.
    ''' - Index-Absicherung integriert: Der Aufruf wird übersprungen, wenn die Auswahl temporär leer ist (z. B. während des Listen-Resets).
    ''' </remarks>
    Private Sub lvPersonal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvPersonal.SelectedIndexChanged
        Try
            ' Vorzeitiger Abbruch, wenn kein Element ausgewählt ist 
            ' (verhindert Fehler beim Leeren oder Neuaufbau der ListView)
            If lvPersonal.SelectedItems.Count = 0 Then Return

            ' Detailinformationen des ausgewählten Mitarbeiters laden
            prGetInfolvPersonal()

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Überträgt die Detailinformationen des aktuell in der ListView ausgewählten Mitarbeiters in die entsprechenden Eingabe- und Textfelder.
    ''' Lädt zusätzlich die Anschrift basierend auf der Mitarbeiter-ID und setzt die Wegstrecken-Eingabe zurück.
    ''' </summary>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf von 'prGetAnschrift' entfernt.
    ''' - Performance-Optimierung: Mehrfache, redundante Zugriffe auf das 'SelectedItems(0)'-Objekt durch Deklaration einer lokalen Zwischenvariable ersetzt.
    ''' - Fehlende Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor IndexOutOfBound-Ausnahmen bei den SubItems ergänzt.
    ''' - Logische Prüfung verfeinert ('If .Count > 0' statt 'undgleich 0') für saubereren Programmfluss.
    ''' </remarks>
    Private Sub prGetInfolvPersonal()
        Try
            ' 1. Vorzeitiger Abbruch, falls kein Element selektiert ist
            If lvPersonal.SelectedItems.Count = 0 Then Return

            ' 2. Das ausgewählte Element einmalig in einer lokalen Variable zwischenspeichern
            ' Das spart dem System bei jedem Feldzugriff das erneute Suchen in der Auflistung.
            Dim selectedUser As ListViewItem = lvPersonal.SelectedItems(0)

            ' 3. Text- und Label-Felder sicher mit den Daten aus den SubItems befüllen
            tbName.Text = selectedUser.SubItems(0).Text
            tbVorname.Text = selectedUser.SubItems(1).Text
            tbTel1.Text = selectedUser.SubItems(2).Text
            tbTel2.Text = selectedUser.SubItems(3).Text
            tbInfo.Text = selectedUser.SubItems(4).Text
            lbID.Text = selectedUser.SubItems(5).Text
            lbChip.Text = selectedUser.SubItems(6).Text
            buTransDel.Enabled = True
            If String.IsNullOrWhiteSpace(lbChip.Text) Then buTransDel.Enabled = False
            ' 4. Zugehörige Anschrift über das System laden
            prGetAnschrift(lbID.Text)

            ' 5. Eingabefeld für Wegstrecke zurücksetzen und aktivieren
            tbWeg34.Text = String.Empty
            tbWeg34.Enabled = True

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Lädt die Anschriftdaten (Straße, PLZ, Ort) eines Mitarbeiters basierend auf seiner ID aus der Datenbank und befüllt die entsprechenden Textfelder.
    ''' </summary>
    ''' <param name="sID">Die eindeutige ID des gesuchten Mitarbeiters.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Fehlende Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor Datenbankausfällen ergänzt.
    ''' - 'Exit Sub'-Struktur durch eine saubere, moderne 'If-Return'-Bedingung ersetzt.
    ''' - NullReferenceExceptions abgefangen: Typsichere Auswertung der Datenbankspalten (.ToString() fängt DBNull ab).
    ''' - Verwendung von 'String.Empty' anstelle von leeren Anführungszeichen zur Speicheroptimierung.
    ''' </remarks>
    Private Sub prGetAnschrift(ByVal sID As String)
        ' 1. Eingabefelder vorsorglich zurücksetzen
        tbStrasse.Text = String.Empty
        tbPLZ.Text = String.Empty
        tbOrt.Text = String.Empty

        ' Sicherheitsprüfung: Falls keine ID übergeben wurde, sofort abbrechen
        If String.IsNullOrWhiteSpace(sID) Then Return

        Try
            ' 2. Anschriftdaten gezielt über die ID abfragen
            Dim dt As DataTable = fcReadDataTable($"SELECT Strasse, PLZ, Ort FROM Personal1 WHERE ID = '{sID.Replace("'", "''")}'")

            ' Vorzeitiger Abbruch, falls kein Datensatz gefunden wurde
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return

            Dim row As DataRow = dt.Rows(0)

            ' 3. Felder befüllen, sofern der Datensatz nicht als gelöscht markiert ist
            If row.RowState <> DataRowState.Deleted Then
                tbStrasse.Text = row("Strasse").ToString().Trim()
                tbPLZ.Text = row("PLZ").ToString().Trim()
                tbOrt.Text = row("Ort").ToString().Trim()
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Ermittelt den Objektnamen aus einer bereitgestellten DataTable anhand der eindeutigen Objekt-ID.
    ''' </summary>
    ''' <param name="dtT">Die DataTable, die die Objektdaten (z. B. ID und Name) enthält.</param>
    ''' <param name="sObj">Die eindeutige ID des gesuchten Objekts.</param>
    ''' <returns>Den Namen des Objekts, oder ein Leerzeichen (" "), wenn die ID nicht gefunden wurde.</returns>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Veraltete VB6-Rückgabesyntax ('fcGetObjektName = ...') durch modernes, typsicheres 'Return' ersetzt.
    ''' - Performance-Bottleneck entfernt: Schleife und mehrfache String-Konvertierungen durch effiziente DataTable.Select-Filterung ersetzt.
    ''' - NullReferenceExceptions abgefangen: Sichere Prüfung der DataTable auf Gültigkeit ('dtT IsNot Nothing').
    ''' - Explizites Abschneiden von Leerzeichen ('Trim') bei den Vergleichswerten vereinheitlicht.
    ''' </remarks>
    Public Function fcGetObjektName(ByVal dtT As DataTable, ByVal sObj As String) As String
        ' Standard-Rückgabewert (ein Leerzeichen, wie im Original-Code definiert)
        Dim resultName As String = " "

        ' 1. Sicherheitsprüfung: Ist die Tabelle gültig und enthält Zeilen?
        If dtT Is Nothing OrElse dtT.Rows.Count = 0 OrElse String.IsNullOrWhiteSpace(sObj) Then
            Return resultName
        End If

        Try
            Dim searchId As String = sObj.Trim()

            ' 2. Den passenden Datensatz direkt über die integrierte Select-Filterung der DataTable suchen
            ' Das ersetzt die langsame For-Schleife und ignoriert gelöschte Zeilen automatisch.
            Dim foundRows() As DataRow = dtT.Select($"ID = '{searchId.Replace("'", "''")}'")

            ' 3. Wenn ein Treffer erzielt wurde und die Zeile nicht gelöscht ist, Namen auslesen
            If foundRows.Length > 0 Then
                Dim row As DataRow = foundRows(0)

                If row.RowState <> DataRowState.Deleted Then
                    Dim nameValue As Object = row("Name")

                    If nameValue IsNot Nothing AndAlso nameValue IsNot DBNull.Value Then
                        resultName = nameValue.ToString()
                    End If
                End If
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try

        Return resultName
    End Function



#End Region

#Region "Buttons und Menü-Ereignisse..............................................................."


    ''' <summary>
    ''' Schließt das aktuelle Personalformular.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        Try
            Me.Close()
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Bereitet die Eingabemaske für das Anlegen eines neuen Personal-Datensatzes vor.
    ''' Sichert die aktuellen Felddaten im Zwischenspeicher und setzt die Textfelder zurück.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    Private Sub tsbNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNew.Click
        Try
            lNew = True

            ' 1. Aktuelle Werte im Speicher-Array sichern
            prBackupMaskeToMemory()

            ' 2. Eingabemaske vollständig leeren
            prClearMaske()

            ' 3. Steuerelemente freigeben und Fokus setzen
            prLoock(True)
            tbName.Select()

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Schaltet die Eingabemaske in den Bearbeitungsmodus und sichert die bestehenden Daten für einen eventuellen Abbruch.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    Private Sub tsbEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEdit.Click
        Try
            prLoock(True)

            ' Aktuelle Werte im Speicher-Array sichern
            prBackupMaskeToMemory()

            tbName.Select()

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Speichert die eingegebenen Personalinformationen in der Datenbank und sperrt anschließend die Eingabemaske wieder.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    Private Sub tsbSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSave.Click
        Try
            prSavepersonal()
            prLoock(False)

            tbWeg34.Text = String.Empty
            tbWeg34.Enabled = True

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Bricht den aktuellen Bearbeitungs- oder Neuanlagevorgang ab und stellt die zuvor gesicherten Daten wieder her.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    Private Sub tsbBreak_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBreak.Click
        Try
            prLoock(False)

            ' Werte aus dem Speicher-Array wieder in die Maske zurückschreiben
            lbID.Text = arMem(0)
            tbName.Text = arMem(1)
            tbVorname.Text = arMem(2)
            tbStrasse.Text = arMem(3)
            tbPLZ.Text = arMem(4)
            tbOrt.Text = arMem(5)
            tbTel1.Text = arMem(6)
            tbTel2.Text = arMem(7)
            tbInfo.Text = arMem(8)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Löscht den aktuell ausgewählten Personal-Datensatz permanent aus der Datenbank nach einer Sicherheitsabfrage.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen vollständig entfernt.
    ''' - Implizite VB6-'MsgBox' durch modernes, typsicheres '.NET MessageBox.Show' mit Icons ersetzt.
    ''' - 'Exit Sub'-Abbrüche durch saubere 'Return'-Strukturen ersetzt.
    ''' - SQL-Injection-Schutz durch Maskierung des ID-Strings ('Replace') nachgerüstet.
    ''' - Absturzgefahr bei der Listen-Erstselektion nach dem Löschen behoben (Prüfung auf 'Items(0)' statt 'TopItem').
    ''' - Fehlende Fehlerabsicherung durch einen globalen 'Try-Catch'-Block ergänzt.
    ''' </remarks>
    Private Sub tsbDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDel.Click
        Dim sID As String = lbID.Text.Trim()

        ' Sicherheitsprüfung: Wenn keine ID vorhanden ist, Aktion abbrechen
        If String.IsNullOrEmpty(sID) Then Return

        ' Transponder-Schutzprüfung
        If Not String.IsNullOrEmpty(lbChip.Text.Trim()) Then
            MessageBox.Show("Bitte löschen Sie zuerst den zugeordneten Transponder!", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim sMsg As String = "Wollen Sie diese Person wirklich löschen?"
            Dim cSql As String = $"DELETE FROM Personal1 WHERE ID = '{sID.Replace("'", "''")}'"

            ' Sicherheitsabfrage via modernem Windows-Forms Dialog
            If MessageBox.Show(sMsg, "Person löschen", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = DialogResult.OK Then

                ' 1. Datensatz per SQL aus der Tabelle löschen
                UpdateTable(cSql)

                ' 3. Eingabemaske zurücksetzen
                prClearMaske()

                ' 4. Datenbestand neu laden und Benutzeroberfläche aktualisieren
                dtPer = fcReadDataTable("SELECT * FROM Personal1")
                prLoadpersonalInList(dtPer)

                ' 5. UI-Fokus und Auswahl auf das erste Element setzen
                lvPersonal.Select()
                If lvPersonal.Items.Count > 0 Then
                    lvPersonal.Items(0).Selected = True
                    lvPersonal.Items(0).Focused = True
                End If

                ' 6. Button-Aktivierungen prüfen
                prCheckNoRecord(dtPer)
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Sichert den aktuellen Inhalt der Maskenfelder im globalen Speicher-Array 'arMem', 
    ''' um die Daten bei einem späteren Abbruch wiederherstellen zu können.
    ''' </summary>
    ''' <remarks>
    ''' 27.09.2026 - Create (Ausgelagert aus tsbNew / tsbEdit)
    ''' </remarks>
    Private Sub prBackupMaskeToMemory()
        Try
            arMem(0) = lbID.Text
            arMem(1) = tbName.Text
            arMem(2) = tbVorname.Text
            arMem(3) = tbStrasse.Text
            arMem(4) = tbPLZ.Text
            arMem(5) = tbOrt.Text
            arMem(6) = tbTel1.Text
            arMem(7) = tbTel2.Text
            arMem(8) = tbInfo.Text
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub


    ''' <summary>
    ''' Leert alle Eingabefelder und Labels der Personal-Maske und setzt diese in den Standardzustand zurück.
    ''' </summary>
    ''' <remarks>
    ''' 27.09.2026 - Create (Ausgelagert aus tsbNew / tsbDel)
    ''' </remarks>
    Private Sub prClearMaske()
        Try
            tbName.Text = String.Empty
            tbVorname.Text = String.Empty
            tbStrasse.Text = String.Empty
            tbPLZ.Text = String.Empty
            tbOrt.Text = String.Empty
            tbTel1.Text = String.Empty
            tbTel2.Text = String.Empty
            tbInfo.Text = String.Empty
            tbWeg34.Text = String.Empty
            lbChip.Text = String.Empty
            lbID.Text = " "
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Steuert den Aktivierungszustand (Sperren/Freigeben) der Schaltflächen und Eingabefelder im Personalformular.
    ''' </summary>
    ''' <param name="lStatus">True, wenn die Maske für die Bearbeitung freigegeben werden soll; False, wenn sie gesperrt werden soll.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 27.09.2026 - Code-Optimierung:
    ''' - Fehlende Fehlerabsicherung durch einen 'Try-Catch'-Block zum Schutz vor UI-Zugriffsfehlern ergänzt.
    ''' - 'prCheckNoRecord'-Aufruf intelligent reaktiviert: Stellt sicher, dass die Edit/Del-Buttons nach dem Sperren nur aktiv sind, wenn Daten existieren.
    ''' - Anordnung der Zuweisungen zur besseren Übersicht und Wartbarkeit logisch in Blöcke (Buttons, Eingabefelder, Sonder-Elemente) unterteilt.
    ''' </remarks>
    Private Sub prLoock(ByVal lStatus As Boolean)
        Try
            Dim lInverse As Boolean = Not lStatus

            ' 1. Steuerung der Standard-Schaltflächen und der Hauptliste
            tsbNew.Enabled = lInverse
            tsbBreak.Enabled = lStatus
            tsbSave.Enabled = lStatus
            lvPersonal.Enabled = lInverse

            ' 2. Steuerung der Text-Eingabefelder (Aktiv bei Bearbeitung)
            tbName.Enabled = lStatus
            tbVorname.Enabled = lStatus
            tbStrasse.Enabled = lStatus
            tbPLZ.Enabled = lStatus
            tbOrt.Enabled = lStatus
            tbTel1.Enabled = lStatus
            tbTel2.Enabled = lStatus
            tbInfo.Enabled = lStatus
            tbWeg34.Enabled = lStatus

            ' 3. Steuerung von Sonder-Schaltflächen (Aktiv im Lese-Modus)
            buSend.Enabled = lInverse
            buTransDel.Enabled = lInverse

            ' 4. Dynamische Absicherung für Edit- und Löschen-Buttons
            If lStatus Then
                ' Während der Bearbeitung sind Edit und Löschen generell gesperrt
                tsbEdit.Enabled = False
                tsbDel.Enabled = False
            Else
                ' Nach dem Speichern/Abbrechen entscheidet der Datenbestand, ob die Buttons aktiv sein dürfen
                prCheckNoRecord(dtPer)
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub


#End Region

#Region "Speichern................................................................................."

    ''' <summary>
    ''' Führt das Speichern oder Aktualisieren eines Personal-Datensatzes in der Datenbank und der lokalen DataTable aus.
    ''' </summary>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei Methodenaufrufen entfernt.
    ''' - Ungenutzte Variablen ('sb') entfernt.
    ''' - String-Verkettungen bereinigt und Übersichtlichkeit der DataTable-Aktualisierung erhöht.
    ''' - Robuste Typisierung bei Array-Initialisierungen sichergestellt.
    ''' </remarks>
    Private Sub prSavepersonal()
        Dim sID As String = lbID.Text

        ' Falls es sich um einen neuen Datensatz handelt, eine neue Zeit-ID generieren
        If lNew Then
            sID = fcGetTimeID(Date.Today)
        End If

        Dim fieldsText As String = "ID,Name,Vorname,Strasse,PLZ,Ort,Tel1,Tel2,Info,Code"
        Dim arFields As String() = fieldsText.Split(","c)

        ' Werte aus der UI/Hilfsfunktion laden und splitten
        Dim valuesText As String = fcSavePersonal(sID)
        Dim arValue As String() = valuesText.Split("°"c)

        Try
            ' 1. Datenbank-Operation (Insert oder Update)
            If lNew Then
                fcInsertCommand("Personal1", arFields, arValue)
            Else
                Dim dbCondition As String = " WHERE ID='" & sID & "'"
                fcUpdateCommand("Personal1", arFields, arValue, dbCondition)
            End If

            ' 2. Lokale DataTable synchronisieren
            If lNew Then
                fcInsertTable(dtPer, arFields, arValue)
            Else
                Dim tableCondition As String = "ID Like '" & sID & "'"
                fcUpdateTable(dtPer, arFields, arValue, tableCondition)
            End If

            lNew = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ' Ansicht aktualisieren und neu laden
            dtPer = fcReadDataTable("SELECT * FROM Personal1")
            prLoadpersonalInList(dtPer)
            prCheckNoRecord(dtPer)
            prLoock(False)

            ' Gespeicherten Eintrag in der Liste selektieren (sofern Werte vorhanden sind)
            If arValue.Length > 0 Then
                prSelectEntry(lvPersonal, arValue(0))
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Sucht einen Eintrag anhand der ID in der ListView und markiert diesen als selektiert.
    ''' </summary>
    ''' <param name="lv">Die ListView, in der gesucht werden soll.</param>
    ''' <param name="sEntry">Die zu suchende ID (erster Spaltenwert).</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax entfernt.
    ''' - Null- und Typprüfungen für sicheren Zugriff auf SubItems implementiert.
    ''' </remarks>
    Private Sub prSelectEntry(ByVal lv As ListView, ByVal sEntry As String)
        If lv Is Nothing OrElse String.IsNullOrEmpty(sEntry) Then Exit Sub

        For Each item As ListViewItem In lv.Items
            If item.SubItems.Count > 0 AndAlso item.SubItems(0).Text = sEntry Then
                lv.Select()
                item.Selected = True
                item.EnsureVisible() ' Stellt sicher, dass das Element im sichtbaren Bereich ist
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' Bereitet die zu speichernden Personal-Daten aus den UI-Elementen auf und verkettet sie mit einem Trennzeichen.
    ''' Leere Felder werden automatisch mit einem Leerzeichen gefüllt.
    ''' </summary>
    ''' <param name="sID">Die eindeutige ID des Personal-Datensatzes.</param>
    ''' <returns>Ein durch ein Grad-Zeichen (°) getrennter String aller Feldwerte.</returns>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - '.Trim = ""' durch performanteres und null-sicheres 'String.IsNullOrWhiteSpace()' ersetzt.
    ''' - Veraltete Zuweisung an den Funktionsnamen durch ein explizites 'Return' ersetzt.
    ''' - Übersichtliche Formatierung der String-Verkettung implementiert.
    ''' </remarks>
    Private Function fcSavePersonal(ByVal sID As String) As String
        ' Validierung der Textfelder: Wenn leer oder nur Leerzeichen, wird ein einzelnes Leerzeichen gesetzt
        If String.IsNullOrWhiteSpace(tbName.Text) Then tbName.Text = " "
        If String.IsNullOrWhiteSpace(tbVorname.Text) Then tbVorname.Text = " "
        If String.IsNullOrWhiteSpace(tbStrasse.Text) Then tbStrasse.Text = " "
        If String.IsNullOrWhiteSpace(tbPLZ.Text) Then tbPLZ.Text = " "
        If String.IsNullOrWhiteSpace(tbOrt.Text) Then tbOrt.Text = " "
        If String.IsNullOrWhiteSpace(tbTel1.Text) Then tbTel1.Text = " "
        If String.IsNullOrWhiteSpace(tbTel2.Text) Then tbTel2.Text = " "
        If String.IsNullOrWhiteSpace(tbInfo.Text) Then tbInfo.Text = " "
        If String.IsNullOrWhiteSpace(lbChip.Text) Then lbChip.Text = " "

        ' Effizientes Zusammenbauen des Daten-Strings
        Dim sb As New StringBuilder
        sb.Append(sID).Append("°")
        sb.Append(tbName.Text).Append("°")
        sb.Append(tbVorname.Text).Append("°")
        sb.Append(tbStrasse.Text).Append("°")
        sb.Append(tbPLZ.Text).Append("°")
        sb.Append(tbOrt.Text).Append("°")
        sb.Append(tbTel1.Text).Append("°")
        sb.Append(tbTel2.Text).Append("°")
        sb.Append(tbInfo.Text).Append("°")
        sb.Append(lbChip.Text)

        Return sb.ToString()
    End Function


#End Region

#Region "Mit Enter weiter zum nächsten Feld........................................................"

    ''' <summary>
    ''' Steuert den Fokuswechsel zum nächsten Steuerelement beim Drücken der Eingabetaste (Enter).
    ''' Unterdrückt zudem den Standard-Systempiepton.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das aktuelle Textfeld).</param>
    ''' <param name="e">Die Ereignisdaten mit dem gedrückten Zeichen.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Redundante Einzelmethoden in eine zentrale, dynamische Ereignismethode zusammengeführt.
    ''' - Veraltete 'Microsoft.VisualBasic.ChrW(13)'-Syntax durch die native .NET-Konstante 'Convert.ToChar(Keys.Enter)' ersetzt.
    ''' - 'e.Handled = True' hinzugefügt, um das störende Windows-Piepsen beim Enter-Druck zu verhindern.
    ''' </remarks>
    Private Sub PersonalFields_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles _
    tbName.KeyPress, tbVorname.KeyPress, tbStrasse.KeyPress, tbPLZ.KeyPress,
    tbOrt.KeyPress, tbTel1.KeyPress, tbTel2.KeyPress, tbInfo.KeyPress

        ' Prüfen, ob die Eingabetaste (Enter) gedrückt wurde
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            ' Standard-Verhalten (z. B. Piepton) unterdrücken
            e.Handled = True

            ' Aktuelles Textfeld ermitteln
            Dim currentTextBox As TextBox = TryCast(sender, TextBox)
            If currentTextBox IsNot Nothing Then

                ' Fokus-Reihenfolge dynamisch steuern
                Select Case currentTextBox.Name
                    Case "tbName" : tbVorname.Select()
                    Case "tbVorname" : tbStrasse.Select()
                    Case "tbStrasse" : tbPLZ.Select()
                    Case "tbPLZ" : tbOrt.Select()
                    Case "tbOrt" : tbTel1.Select()
                    Case "tbTel1" : tbTel2.Select()
                    Case "tbTel2" : tbInfo.Select()
                    Case "tbInfo" : tbName.Select()
                End Select

            End If
        End If
    End Sub


#End Region

    ''' <summary>
    ''' Bereitet die Transponder-Daten vor, sendet diese an den Webserver (PHP) und speichert das Personal ab.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Aufruf von 'prSavepersonal' entfernt.
    ''' - Toten, auskommentierten Code gelöscht, um die Wartbarkeit zu verbessern.
    ''' - String-Verkettung modernisiert und '.Trim' direkt mit Null-Prüfung kombiniert.
    ''' </remarks>
    Private Sub buSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buSend.Click
        Me.Cursor = Cursors.WaitCursor
        ' Basis-Codevorlage definieren
        Dim codeTemplate As String = "20000101°1200°20990101°1200|"

        ' Chip-Text auslesen, Leerzeichen entfernen und mit Template verketten
        Dim chipText As String = If(lbChip.Text IsNot Nothing, lbChip.Text.Trim(), "")
        sgCodeNew = chipText & "°" & codeTemplate

        ' Ziel-IP-Adresse aus den globalen Einstellungen laden
        Dim sIP As String = cgIPWeb

        ' Daten an die PHP-Schnittstelle übertragen
        PHP.Data(sgCodeNew, sIP)

        ' Personal-Datensatz lokal und in der Datenbank speichern
        prSavepersonal()
        Me.Cursor = Cursors.Default
    End Sub

    ''' <summary>
    ''' Reagiert auf die Änderung des Textes im Eingabefeld. Sobald die erwartete Länge von 10 Zeichen 
    ''' erreicht ist, wird das Feld gesperrt, der Code konvertiert, der Speicher-Button fokussiert 
    ''' und das Löschen des Transponders deaktiviert.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - System-Namespaces in der Signatur gekürzt.
    ''' - Null-Prüfung ('IsNot Nothing') für den Textwert hinzugefügt, um potenzielle Laufzeitfehler zu verhindern.
    ''' - Inline-Kommentare zur besseren Nachvollziehbarkeit der Geschäftslogik ergänzt.
    ''' </remarks>
    Private Sub tbWeg34_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbWeg34.TextChanged
        ' Sicherheitsprüfung: Sicherstellen, dass das Steuerelement und der Text existieren
        If tbWeg34.Text IsNot Nothing AndAlso tbWeg34.Text.Length = 10 Then
            ' Weitere Eingaben blockieren
            tbWeg34.Enabled = False

            ' Transponder-Code über Hilfsfunktion umrechnen und im Label anzeigen
            lbChip.Text = fcWeg34(tbWeg34.Text)

            ' Fokus direkt auf den Speichern-Button setzen
            tsbSave.Select()

            ' Löschen-Schaltfläche während des Prozesses deaktivieren
            buTransDel.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Löscht den aktuellen Transponder-Code (Chip-ID), zeigt das Sende-Formular an 
    ''' und aktualisiert die Personal-Datenbank.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Aufruf von 'prSavepersonal' entfernt.
    ''' - Großen Block auskommentierten "Leichen-Codes" entfernt.
    ''' - 'System.Object' und 'System.EventArgs' auf die kürzeren .NET-Typen 'Object' und 'EventArgs' reduziert.
    ''' - Null-sicheres Auslesen des Steuerelements (.Trim) sichergestellt.
    ''' </remarks>
    Private Sub buTransDel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buTransDel.Click
        Me.Cursor = Cursors.WaitCursor
        ' Basis-Codevorlage für das Löschen/Deaktivieren definieren
        Dim deleteTemplate As String = "20000101#1200#20000102#1200|"

        ' Chip-Text auslesen, Leerzeichen entfernen und mit Template verketten
        Dim chipText As String = If(lbChip.Text IsNot Nothing, lbChip.Text.Trim(), "")
        sgCodeNew = chipText & "#" & deleteTemplate

        ' Transponder-Anzeige in der UI leeren
        lbChip.Text = ""

        ' Personal-Datensatz lokal und in der Datenbank aktualisieren
        prSavepersonal()
        Me.Cursor = Cursors.Default
    End Sub


End Class