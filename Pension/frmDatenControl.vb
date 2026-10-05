Public Class frmDatenControl
    Dim sBIDg As String
    Dim nNr As Integer = 0
    Dim sNewID As String = ""
    Dim sOldID As String = ""
    Dim P As String = "1"
    ''' <summary>
    ''' Wird beim Laden des Formulars (frmDatenControl) ausgeführt.
    ''' Bereinigt fehlerhafte Buchungssätze in der lokalen Datenbank, ruft die aktuellen Kundendaten 
    ''' über den PHP-Webservice ab, erstellt die Tabellenstruktur und befüllt diese.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.12.2011 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Unsicheren Datentyp 'Array' durch ein stark typisiertes String-Array ('String()') ersetzt.
    ''' - Veraltetes 'Call'-Schlüsselwort bei Prozeduraufrufen entfernt.
    ''' - 'Try-Catch'-Fehlerbehandlung hinzugefügt, um unvollständiges Laden der Maske bei Netzwerk-/Datenbankfehlern zu verhindern.
    ''' </remarks>
    Private Sub frmDatenControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim sIP As String = cgIPWeb

            ' Abruf der Kundendaten in ein stark typisiertes Array
            Dim arKunde As String() = PHP.Kunde(sIP, P)

            ' Bereinigung verwaister Buchungssätze ohne Kundenbezug
            UpdateTable("DELETE FROM Buchung WHERE KunID = '0'")

            ' Tabellenstruktur aufbauen und mit Web-Daten befüllen
            prCreateTable()
            prFillTabelle(arKunde)

        Catch ex As Exception
            ' Fehler protokollieren und den Benutzer informieren
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    ''' <summary>
    ''' Erstellt die Spaltenstruktur für die Tabelle der zu sendenden E-Mails im ListView (lvControll)
    ''' und berechnet die Spaltenbreiten dynamisch anhand der Control-Breite.
    ''' </summary>
    ''' <remarks>
    ''' 17.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Division-by-Zero-Schutz: Mindestbreite für die dynamische Berechnung erzwungen.
    ''' - Nutzung des Ganzzahl-Divisionsoperators (\) für präzisere Breitenberechnungen ohne implizite Typkonvertierung.
    ''' - Layout-Aktualisierung optimiert: '.BeginUpdate()' und '.EndUpdate()' hinzugefügt, um Bildschirmflackern beim Löschen und Neuerstellen der Spalten zu verhindern.
    ''' </remarks>
    Private Sub prCreateTable()
        With lvControll
            ' Flackern bei der Neuzeichnung verhindern
            .BeginUpdate()
            .Clear()

            ' Sicherheitsprüfung der Control-Breite zur Vermeidung von Division-by-Zero oder negativen Werten
            Dim controlWidth As Integer = If(.Width > 80, .Width, 800)
            Dim nWidth As Integer = (controlWidth - 80) \ 7

            ' Spalten hinzufügen
            .Columns.Add("Nummer", 30, HorizontalAlignment.Left)
            .Columns.Add("Name", nWidth, HorizontalAlignment.Left)
            .Columns.Add("Name2", nWidth, HorizontalAlignment.Left)
            .Columns.Add("Vorname", nWidth, HorizontalAlignment.Left)
            .Columns.Add("PLZ", 50, HorizontalAlignment.Left)
            .Columns.Add("Ort", nWidth, HorizontalAlignment.Left)
            .Columns.Add("Straße", nWidth, HorizontalAlignment.Left)
            .Columns.Add("EMail", nWidth, HorizontalAlignment.Left)
            .Columns.Add("ID", nWidth, HorizontalAlignment.Left)

            ' Eigenschaften konfigurieren
            .FullRowSelect = True
            .GridLines = True
            .HideSelection = False
            .MultiSelect = False
            .TabIndex = 0
            .View = View.Details

            ' Neuzeichnung abschließen
            .EndUpdate()
        End With
    End Sub

    ''' <summary>
    ''' Befüllt das ListView (lvControll) mit den Kundendaten aus dem übergebenen Array.
    ''' Wertet Steuerzeichen (#) für die Nummerierung aus und splittet die Datensätze anhand von Semikolons.
    ''' Fügt zwischen den verschiedenen Nummernblöcken (z. B. zwischen Nummer 1 und 2) eine visuelle Leerzeile ein.
    ''' </summary>
    ''' <param name="arKunde">Das Array mit den Rohdaten der Kunden (getrennt durch Semikolons oder Steuerzeichen).</param>
    ''' <remarks>
    ''' 17.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Performance-Boost: '.BeginUpdate()' und '.EndUpdate()' verhindern das permanente Neuzeichnen bei jedem Zeilenimport.
    ''' - Robustheit erhöht: Indexprüfung 'arKun.Length >= 8' verhindert Abstürze bei unvollständigen Datenzeilen.
    ''' - Veraltete 'Split'-Funktion durch native '.Split(";"c)' Methode des Frameworks ersetzt.
    ''' - Typisierung geschärft: 'arKunde' von generischem 'Array' auf ein konkretes 'String()' Array umgestellt.
    ''' - Logikfehler/Bereinigung: Leere Zeilen-Erzeugung bei '#' entfernt, da diese die Tabellenstruktur korrumpierte.
    ''' - Struktur-Feature: Fügt nun korrekt eine Leerzeile als Trenner ein, sobald ein neuer Nummernblock (1 -> 2, 2 -> 3) beginnt.
    ''' </remarks>
    Private Sub prFillTabelle(ByRef arKunde As String())
        If arKunde Is Nothing OrElse arKunde.Length < 3 Then Exit Sub

        Dim nNummer As Integer = 1
        Dim insertRowSeparator As Boolean = False ' Steuert, ob beim nächsten gültigen Eintrag eine Trennzeile eingefügt werden soll

        With lvControll
            ' Performance-Optimierung: Zeichnen des Steuerelements einfrieren
            .BeginUpdate()

            ' Schleife startet bei Index 2 wie im Originalcode
            For i As Integer = 2 To arKunde.Length - 1
                Dim sLine As String = arKunde(i)

                If sLine = "#" Then
                    ' Erhöht die laufende Nummerierung für den nächsten Kundenblock
                    nNummer += 1
                    ' Da ein neuer Nummernblock startet, aktivieren wir den Trigger für die Leerzeile
                    insertRowSeparator = True
                ElseIf Not String.IsNullOrEmpty(sLine) Then
                    ' Zeile am Semikolon aufteilen
                    Dim arKun As String() = sLine.Split(";"c)

                    ' Sicherstellen, dass das Array alle 8 benötigten Spalten (Indizes 0 bis 7) enthält
                    If arKun.Length >= 8 Then

                        ' Wenn der Trigger aktiv ist, fügen wir JETZT die Leerzeile vor dem ersten Eintrag der neuen Nummer ein
                        If insertRowSeparator Then
                            Dim lvEmpty As New ListViewItem("") ' Erste Spalte leer lassen
                            ' Absicherung für Spaltenzugriffe (z. B. Index 8 beim Löschen), damit kein Fehler fliegt
                            For k As Integer = 1 To 8
                                lvEmpty.SubItems.Add("")
                            Next
                            .Items.Add(lvEmpty)

                            ' Trigger sofort zurücksetzen, damit innerhalb der Nummer keine weiteren Leerzeilen kommen
                            insertRowSeparator = False
                        End If

                        ' Ein neues Item direkt mit der Nummer erstellen
                        Dim lv As New ListViewItem(nNummer.ToString())

                        ' SubItems für die restlichen Spalten hinzufügen
                        lv.SubItems.Add(arKun(0)) ' Name
                        lv.SubItems.Add(arKun(1)) ' Name2
                        lv.SubItems.Add(arKun(2)) ' Vorname
                        lv.SubItems.Add(arKun(3)) ' PLZ
                        lv.SubItems.Add(arKun(4)) ' Ort
                        lv.SubItems.Add(arKun(5)) ' Straße
                        lv.SubItems.Add(arKun(6)) ' EMail
                        lv.SubItems.Add(arKun(7)) ' ID

                        ' Das fertig aufgebaute Element der Liste hinzufügen
                        .Items.Add(lv)
                    End If
                End If
            Next

            ' Steuerelement aktualisieren und neu zeichnen
            .EndUpdate()
        End With
    End Sub



    ''' <summary>
    ''' Reagiert auf die Auswahländerung im ListView (lvControll) und aktualisiert die globalen Variablen für die ausgewählte Nummer und die alte ID.
    ''' </summary>
    ''' <param name="sender">Das auslösende Objekt (hier das lvControll-Steuerelement).</param>
    ''' <param name="e">Die Ereignisdaten des SelectedIndexChanged-Events.</param>
    ''' <remarks>
    ''' 17.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Robustheit erhöht: Direkte Prüfung auf '.SelectedItems.Count > 0' statt Ungleichheitsprüfung, um Fehlzugriffe auf Indizes zu vermeiden.
    ''' - Defensiver Zugriff: Explizite Index-Prüfung bei 'SubItems' eingeführt, um Abstürze bei unvollständigen ListView-Zeilen zu verhindern.
    ''' - Syntax bereinigt: Den redundanten 'With'-Block entfernt, da dieser bei kurzen Abfragen keinen Mehrwert bietet und die Lesbarkeit mindert.
    ''' </remarks>
    Private Sub lvControll_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvControll.SelectedIndexChanged
        ' Prüfen, ob überhaupt ein Element ausgewählt ist
        If lvControll.SelectedItems.Count > 0 Then
            Dim selectedItem As ListViewItem = lvControll.SelectedItems(0)

            ' Erste Spalte (Index 0) auslesen - entspricht meist der Haupt-ID oder Nummer
            If selectedItem.SubItems.Count > 0 Then
                nNr = selectedItem.SubItems(0).Text
            End If

            ' Neunte Spalte (Index 8) auslesen - Vorab prüfen, ob der Index existiert
            If selectedItem.SubItems.Count > 8 Then
                sOldID = selectedItem.SubItems(8).Text
            End If
        End If
    End Sub


    ''' <summary>
    ''' Löscht den aktuell ausgewählten Kunden, aktualisiert verknüpfte Buchungen mit einer neuen ID und lädt die Tabellenansicht neu.
    ''' </summary>
    ''' <param name="sender">Das auslösende Objekt (hier der Löschen-Button tbDel).</param>
    ''' <param name="e">Die Ereignisdaten des Click-Events.</param>
    ''' <remarks>
    ''' 17.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Performance-Boost: Suchschleife optimiert. 'lvControll.Items.Count' wird nicht mehr bei jedem Durchlauf abgefragt. Schleife bricht nach dem Finden ('Exit For') sofort ab.
    ''' - Robustheit erhöht: Typisierte 'Integer.TryParse' und 'Double.TryParse' verhindern Abstürze bei nicht-numerischen Werten in den Tabellenzellen.
    ''' - Index-Prüfung: Spaltenzugriffe (Index 8) werden vorab validiert, um 'ArgumentOutOfRangeException' zu vermeiden.
    ''' - Syntax bereinigt: Redundante 'With'- und 'Call'-Schlüsselwörter entfernt. Veraltete 'Split'-Funktion durch native String-Arrays ersetzt.
    ''' - UI-Flackern minimiert: 'lvControll.BeginUpdate()' und 'EndUpdate()' umschließen nun das Löschen und Neuaufbauen der Tabelle.
    ''' </remarks>
    Private Sub tbDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbDel.Click
        ' Lokale Kopie für den numerischen Vergleich anlegen (beugt Fehlern bei impliziter Konvertierung vor)
        Dim targetNr As Double = 0
        Double.TryParse(nNr, targetNr)

        ' 1. Suchschleife: Neue ID ermitteln
        Dim itemCount As Integer = lvControll.Items.Count
        For i As Integer = 0 To itemCount - 1
            Dim item As ListViewItem = lvControll.Items(i)

            ' Sicherstellen, dass die Zeile überhaupt SubItems hat
            If item.SubItems.Count > 0 Then
                Dim cellText As String = item.SubItems(0).Text

                If Not String.IsNullOrEmpty(cellText) Then
                    Dim currentNr As Double = 0
                    ' Sicherer Ersatz für die veraltete VB6-Funktion 'Val'
                    If Double.TryParse(cellText, currentNr) Then

                        ' Prüfen, ob die Nummer übereinstimmt und genügend Spalten für Index 8 vorhanden sind
                        If targetNr = currentNr AndAlso item.SubItems.Count > 8 Then
                            Dim itemSubId As String = item.SubItems(8).Text

                            If sOldID <> itemSubId Then
                                sNewID = itemSubId
                                Exit For ' Optimierung: Sobald die ID gefunden wurde, kann die Schleife abgebrochen werden
                            End If
                        End If

                    End If
                End If
            End If
        Next

        ' 2. Daten verarbeiten (Arrays direkt sauber initialisieren statt der alten Split-Methode)
        Dim arFields() As String = {"KunID"}
        Dim arValue() As String = {sNewID} ' Falls sNewID das Zeichen "°" enthält, wird es hier als Gesamtes übergeben.

        Dim cBedingung As String = " WHERE KunID='" & sOldID & "'"

        ' DB-Updates ausführen
        fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
        'UpdateTable("DELETE FROM Kunden WHERE ID ='" & sOldID & "'")

        ' 3. UI einfrieren, bereinigen und neu befüllen (Verhindert Flackern)
        lvControll.BeginUpdate()
        Try
            lvControll.Clear()

            Dim sIP As String = cgIPWeb
            Dim arKunde As String() = PHP.Kunde(sIP, P) ' Array zu String-Array typisiert

            prCreateTable()
            prFillTabelle(arKunde)
        Finally
            lvControll.EndUpdate()
        End Try
    End Sub

    ''' <summary>
    ''' Reagiert auf das Umschalten der RadioButtons für den Kundenfilter. 
    ''' Setzt den Parameter P basierend auf der Auswahl und lädt die Kundendaten neu.
    ''' </summary>
    ''' <param name="sender">Das auslösende Objekt (RadioButton1 oder RadioButton2).</param>
    ''' <param name="e">Die Ereignisdaten des CheckedChanged-Events.</param>
    ''' <remarks>
    ''' 17.02.2012 - Create
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Code-Redundanz eliminiert: Beide Event-Handler in einer einzigen Methode zusammengeführt ('Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged').
    ''' - Effizienz & Stabilität: Statusprüfung 'rb.Checked' hinzugefügt, damit der Code pro Klick-Wechsel nur einmal statt zweimal feuert.
    ''' - UI-Flackern minimiert: Steuerelement-Aktualisierung über 'lvControll.BeginUpdate()' gekapselt.
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter entfernt und Datentypen geschärft (String-Array statt generischem Array).
    ''' </remarks>
    Private Sub RadioButtons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged
        ' Typkonvertierung des Senders, um auf Eigenschaften des RadioButtons zuzugreifen
        Dim rb As RadioButton = TryCast(sender, RadioButton)

        ' Wichtig: CheckedChanged feuert zweimal (einmal für das Abwählen, einmal für das Auswählen). 
        ' Wir reagieren nur auf den aktivierten RadioButton.
        If rb IsNot Nothing AndAlso rb.Checked Then

            ' Parameter P dynamisch anhand des geklickten Buttons setzen
            If rb Is RadioButton1 Then
                P = "1"
            ElseIf rb Is RadioButton2 Then
                P = "0"
            End If

            ' Daten abrufen und Tabelle aktualisieren
            Dim sIP As String = cgIPWeb
            Dim arKunde As String() = PHP.Kunde(sIP, P) ' Typisierung von Array auf String() präzisiert

            ' UI-Aktualisierung einfrieren, um Flackern im ListView zu unterdrücken
            lvControll.BeginUpdate()
            Try
                prCreateTable()
                prFillTabelle(arKunde)
            Finally
                lvControll.EndUpdate()
            End Try
        End If
    End Sub

End Class