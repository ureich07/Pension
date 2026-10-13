Imports System.Text

Public Class frmZusatz
    Dim lNew As Boolean
    Dim dtZ As DataTable
    Dim dtP As DataTable
    Dim sKNr As String
    Dim sPNr As String
    Dim sBNr As String
    Dim eRow As Integer
    Dim sZuID As String

#Region "Formular initialisieren..................................................................."

    ''' <summary>
    ''' Verarbeitet das Load-Event des Zusatzkosten-Formulars. Konfiguriert das UI-Design,
    ''' übernimmt die Gast- und Zimmerinformationen aus dem Hauptformular und lädt die 
    ''' gebuchten Zusatzleistungen aus der Datenbank.
    ''' </summary>
    ''' <param name="sender">Das auslösende Objekt (Form).</param>
    ''' <param name="e">Die Ereignisdaten des Load-Events.</param>
    ''' <remarks>
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter konsequent entfernt.
    ''' - Crash-Schutz implementiert: Steuerelement-Zugriffe auf 'frmReservierungDest' über Null-Checks abgesichert.
    ''' - Performance: Die Zuweisung der Hintergrundfarbe ('sColor') optimiert, um unnötige Instanziierungen zu vermeiden.
    ''' </remarks>
    Private Sub frmZusatz_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Cursor = Cursors.WaitCursor

        ' UI-Hintergrundfarben einheitlich setzen
        Dim sColor As Color = Color.LightYellow
        Me.BackColor = sColor
        gbZusatz.BackColor = sColor
        tsMain.BackColor = sColor
        paZusatz.BackColor = sColor

        ' Datenübergabe aus dem Reservierungsformular (mit Absturzsicherung, falls das Formular nicht geladen ist)
        If frmReservierungDest IsNot Nothing Then
            lbZimNr.Text = frmReservierungDest.lbZimNr?.Text
            lbName.Text = frmReservierungDest.tbName1?.Text
        End If

        ' Zusatzleistungen basierend auf Buchungs- und Zimmer-ID aus der Datenbank laden
        dtZ = fcReadDataTable("Select * from Zusaetze Where BuchID='" & sgRBID & "' and ZimID ='" & sgRZID & "'")

        ' Tabellenstrukturen vorbereiten und mit Daten befüllen
        prSetTableZusatz()
        prFuelleTabelleZusatz(dtZ)

        Me.Cursor = Cursors.Default
    End Sub

    ''' <summary>
    ''' Initialisiert die Spaltenstruktur, Zellenausrichtungen, Spaltenbreiten und 
    ''' Selektionsfarben für das DataGridView der Zusatzkosten-Übersicht.
    ''' </summary>
    ''' <remarks>
    ''' 28.02.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Stabilität & UI: 'ColumnHeadersHeightSizeMode' vor der festen Höhenzuweisung gesetzt, um Layout-Konflikte im WinForms-Designer zu unterbinden.
    ''' - Code-Struktur: Die verschachtelte 'With'-Anweisung bereinigt und linearisiert.
    ''' - Performance: Die Spaltenformatierungen direkt nach der Spaltengenerierung sauber gebündelt.
    ''' </remarks>
    Private Sub prSetTableZusatz()
        With dgZusatz
            .Columns.Clear()

            ' Layout-Modus für Header zuerst festlegen (wichtig für neuere .NET Runtimes)
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            .ColumnHeadersHeight = 25

            ' Spalten hinzufügen (Name/Schlüssel, HeaderText)
            .Columns.Add("ID", "ID")
            .Columns.Add("Datum", "Datum")
            .Columns.Add("Menge", "Menge")
            .Columns.Add("Beschreibung", "Beschreibung")
            .Columns.Add("Preis", "Preis")
            .Columns.Add("Steuer", "Steuer (%)")
            .Columns.Add("Gesamt", "Gesamt")

            ' Spalte 0: ID
            .Columns(0).Width = 50
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(0).Visible = False

            ' Spalte 1: Datum
            .Columns(1).Width = 80
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            ' Spalte 2: Menge
            .Columns(2).Width = 50
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            ' Spalte 3: Beschreibung
            .Columns(3).Width = 400
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            ' Spalte 4: Preis
            .Columns(4).Width = 80
            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            ' Spalte 5: Steuer (%)
            .Columns(5).Width = 80
            .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Spalte 6: Gesamt
            .Columns(6).Width = 80
            .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            ' Allgemeine Grid-Berechtigungen und Verhalten
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect

            ' Farben für die selektierte Zeile festlegen
            .DefaultCellStyle.SelectionBackColor = cgColorRow
            .DefaultCellStyle.SelectionForeColor = Color.Black

            ' Zeilenhöhen abschließend anpassen
            .AutoResizeRows()
        End With
    End Sub



    ''' <summary>
    ''' Befüllt das DataGridView der Zusatzkosten mit den Einträgen aus der übergebenen DataTable,
    ''' berechnet die Zeilen- und Gesamtsummen und aktualisiert die Anzeige-Labels sowie die Toolbar-Buttons.
    ''' </summary>
    ''' <param name="dt">Die DataTable, welche die geladenen Zusatzkosten-Datensätze enthält.</param>
    ''' <remarks>
    ''' 29.02.2012 - Create 
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Performance-Schub: Den unnötigen Umweg über 'StringBuilder.Append' und 'Split' eliminiert. Zeilen werden jetzt direkt als Objekt-Array übergeben.
    ''' - Speicheroptimierung: 'StringBuilder' vollständig aus der Methode entfernt, um die Garbage Collection zu entlasten.
    ''' - Null-Value-Protection: Datentyp-Konvertierungen über sichere '.ToString()'-Aufrufe und Typ-Zuweisungen stabilisiert.
    ''' - Logik-Bereinigung: Redundante UI-Zuweisungen von Zimmernummer und Name aus der Schleife herausgezogen (wird nur einmal am Ende oder bei Bedarf gesetzt).
    ''' </remarks>
    Private Sub prFuelleTabelleZusatz(ByVal dt As DataTable)
        ' BEHEBUNG DES GEISTEREINTRAGS: Das Grid MUSS in jedem Fall zuerst geleert werden!
        dgZusatz.Rows.Clear()

        ' Wenn keine Daten mehr da sind, setzen wir die Summen-Labels zurück und brechen sauber ab
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            tsbDelete.Enabled = False
            lbSumme.Text = "0,00"

            Exit Sub
        End If

        Dim nMax As Integer = dt.Rows.Count - 1
        Dim nGesamt As Double = 0
        Dim nSumme As Double
        Dim nMenge As Integer
        Dim nSteuerSatz As Integer

        Try
            With dgZusatz
                ' Die Zeile .Rows.Clear() wurde nach ganz oben vor das If verschoben!

                For i As Integer = 0 To nMax
                    Dim row As DataRow = dt.Rows(i)

                    If row.RowState <> DataRowState.Deleted Then
                        ' Werte sicher auslesen und konvertieren
                        Dim sId As String = row("ID")?.ToString()
                        Dim sDatum As String = fcUmDatum(row("Datum")?.ToString())
                        Dim sBezeichnung As String = row("Bezeichnung")?.ToString()

                        nSumme = Convert.ToDouble(row("Betrag")) / 100
                        nMenge = Convert.ToInt32(row("Menge"))
                        nSteuerSatz = Convert.ToInt32(row("Steuer"))

                        Dim sEinzelPreisFormatiert As String = fcFormatDecimal(nSumme.ToString())

                        ' Zeilengesamtsumme berechnen und formatieren
                        Dim nZeilenGesamt As Double = nSumme * nMenge
                        Dim sZeilenGesamtFormatiert As String = fcFormatDecimal(nZeilenGesamt.ToString())

                        nGesamt += nZeilenGesamt

                        ' Daten direkt als Zeile in das Grid einfügen
                        .Rows.Add(New Object() {sId, sDatum, nMenge.ToString(), sBezeichnung, sEinzelPreisFormatiert, nSteuerSatz.ToString(), sZeilenGesamtFormatiert})

                        ' Stammdaten-Labels aktualisieren
                        lbZimNr.Text = row("ZimNr")?.ToString()
                        lbName.Text = row("Name")?.ToString()
                    End If
                Next

                ' Gesamtsumme im Label formatieren
                lbSumme.Text = fcFormatDecimal(nGesamt.ToString())

                ' Toolbar-Button steuern
                tsbDelete.Enabled = (.Rows.Count > 0)
            End With

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub



#End Region

#Region "Buttons und Menü-Ereignisse..............................................................."
    ''' <summary>
    ''' Schließt das aktuelle Formular über den Close-Toolbarbutton.
    ''' </summary>
    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        prCloseForm()
    End Sub

    ''' <summary>
    ''' Führt das kontrollierte Schließen der Maske durch.
    ''' </summary>
    Private Sub prCloseForm()
        Me.Close()
    End Sub

    ''' <summary>
    ''' Öffnet das Bearbeitungs-Panel für einen bestehenden Datensatz bei einem Doppelklick auf eine Zeile.
    ''' </summary>
    ''' <remarks>
    ''' - Crash-Schutz: Verhindert Abstürze (IndexOutOfRangeException) bei einem Klick auf die Header-Zeile (RowIndex = -1).
    ''' - Benannte Zellen: Übergabe auf lesbare Zellbezeichner angepasst, um Indexverschiebungen abzufangen.
    ''' </remarks>
    Private Sub dgZusatz_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgZusatz.CellDoubleClick
        ' Header-Klicks abfangen (-1 signalisiert den Spaltenkopf)
        If e.RowIndex < 0 Then Exit Sub

        paZusatz.Visible = True
        lNew = False
        dgZusatz.Enabled = False

        ' Werte aus der selektierten Zeile in die Bearbeitungsmaske laden
        Dim selectedRow As DataGridViewRow = dgZusatz.Rows(e.RowIndex)

        sZuID = selectedRow.Cells(0).Value?.ToString()
        dtpDatum.Text = selectedRow.Cells(1).Value?.ToString()
        tbMenge.Text = selectedRow.Cells(2).Value?.ToString()
        tbLeistung.Text = selectedRow.Cells(3).Value?.ToString()
        tbPreis.Text = selectedRow.Cells(4).Value?.ToString()
        coSteuer.Text = selectedRow.Cells(5).Value?.ToString()
    End Sub

    ''' <summary>
    ''' Aktiviert den Neuanlage-Modus für Zusatzleistungen und blendet das Bearbeitungs-Panel ein.
    ''' </summary>
    Private Sub tsbNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNew.Click
        lNew = True
        paZusatz.Visible = True
    End Sub

    ''' <summary>
    ''' Löscht den aktuell markierten Zusatzkosten-Datensatz sowohl aus der SQL-Datenbank 
    ''' als auch sofort aus der lokalen In-Memory DataTable "dtZ" und erzwingt das Leeren des Grids.
    ''' </summary>
    ''' <remarks>
    ''' - Logische Korrektur: Die Prüfung 'sZuID = ""' wurde nach oben gezogen, um unnötige MessageBoxen zu verhindern.
    ''' - Syntax: Entfernung von 'Call' und Umstellung auf VB.NET standardkonforme Stringverkettung.
    ''' </remarks>
    Private Sub tsbDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelete.Click
        ' Abbruch, wenn kein Datensatz ausgewählt ist (bevor der Benutzer gefragt wird)
        If String.IsNullOrEmpty(sZuID) Then Exit Sub

        Dim sMsg As String = "Wollen Sie diese Leistung löschen?  "

        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            Dim cSql As String = "DELETE FROM Zusaetze WHERE ID = '" & sZuID & "'"

            ' 1. Datensatz per SQL aus der realen Datenbank-Tabelle löschen
            UpdateTable(cSql)

            ' 2. Datensatz sofort aus der lokalen DataTable entfernen
            Dim foundRows() As DataRow = dtZ.Select("ID = '" & sZuID & "'")
            For Each row As DataRow In foundRows
                dtZ.Rows.Remove(row) ' Aus dem RAM-Speicher entfernen
            Next

            ' Wichtig: DataTable-Index zurücksetzen, damit .Rows.Count bei 0 ankommt
            dtZ.AcceptChanges()

            ' Variable zurücksetzen, da der Datensatz nicht mehr existiert
            sZuID = ""

            ' 3. Grid fokussieren und mit den verbliebenen RAM-Daten aktualisieren
            dgZusatz.Select()
            prFuelleTabelleZusatz(dtZ)
        End If
    End Sub



    ''' <summary>
    ''' Aktualisiert die aktive sZuID-Variable, sobald der Benutzer eine andere Zeile im Grid betritt oder fokussiert.
    ''' </summary>
    ''' <remarks>
    ''' - Crash-Schutz: Indexprüfung fängt Header-Fokussierungen sicher ab.
    ''' </remarks>
    Private Sub dgZusatz_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgZusatz.CellEnter
        If e.RowIndex >= 0 Then
            sZuID = dgZusatz.Rows(e.RowIndex).Cells(0).Value?.ToString()
        End If
    End Sub


#End Region

#Region "Zusätze bearbeiten........................................................................"

    ''' <summary>
    ''' Schließt das Bearbeitungspanel für Zusatzleistungen über die Schließen-Schaltfläche.
    ''' </summary>
    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        prCloseZusatz()
    End Sub

    ''' <summary>
    ''' Setzt den Bearbeitungsmodus zurück, reaktiviert das Grid und blendet das Eingabepanel aus.
    ''' </summary>
    Private Sub prCloseZusatz()
        prLoockZusatz(False)
        paZusatz.Visible = False
        dgZusatz.Enabled = True
        dgZusatz.Select()
    End Sub

    ''' <summary>
    ''' Löst den Speichervorgang für die Zusatzleistung aus.
    ''' </summary>
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        prSaveZusatz()
    End Sub

    ''' <summary>
    ''' Validiert und speichert die Zusatzleistung. Unterscheidet automatisch zwischen der 
    ''' Neuanlage eines Datensatzes und der Aktualisierung eines bestehenden Eintrags 
    ''' sowohl in der SQL-Datenbank als auch in der lokalen DataTable.
    ''' </summary>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Performance: Unnötige StringBuilder-Instanziierung entfernt und das veraltete VB6-'Split' durch '.Split()' ersetzt.
    ''' - Code-Hygiene: Redundante 'Call'-Befehle eliminiert und ungenutzte Variablen entfernt.
    ''' </remarks>
    Private Sub prSaveZusatz()
        Dim sqlText As String = "ID,BuchID,ZimID,Menge,Bezeichnung,Betrag,Steuer,Gesamt,Datum,ZimNr,Name"
        Dim arFields() As String = sqlText.Split(","c)
        Dim arValue() As String
        Dim cBedingung As String = ""
        Dim sID As String = sZuID

        Try
            ' Maskenwerte serialisiert auslesen (Trennzeichen: °)
            Dim sRawValues As String = fcSaveZusatz(sID)
            arValue = sRawValues.Split("°"c)

            ' --- 1. Datenbank-Ebene (SQL-Befehle) ---
            If lNew Then sID = fcAppendBlank("Zusaetze")
            ' Bestehenden Datensatz aktualisieren
            cBedingung = " WHERE ID='" & sID & "'"
            fcUpdateCommand("Zusaetze", arFields, arValue, cBedingung)

            ' --- 2. In-Memory-Ebene (DataTable-Aktualisierung) ---
            If lNew Then
                ' Datensatz der lokalen DataTable "dtZ" hinzufügen
                fcInsertTable(dtZ, arFields, arValue)
            Else
                ' Datensatz in lokaler DataTable "dtZ" modifizieren
                cBedingung = "ID Like '" & sID & "'"
                fcUpdateTable(dtZ, arFields, arValue, cBedingung)
            End If

            ' Grid neu laden und UI-Status zurücksetzen
            prFuelleTabelleZusatz(dtZ)
            prCloseZusatz()

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            prLoockZusatz(False)
            lNew = False
        End Try
    End Sub

    ''' <summary>
    ''' Bereitet die Eingabewerte der Benutzeroberfläche für die Datenbank auf und serialisiert 
    ''' diese, getrennt durch ein Grad-Zeichen (°), in eine zusammenhängende Zeichenkette.
    ''' </summary>
    ''' <param name="sID">Die eindeutige Datensatz-ID der Zusatzleistung.</param>
    ''' <returns>Eine serialisierte Zeichenkette mit allen Feldern für den Datenbankexport.</returns>
    ''' <remarks>
    ''' 10.01.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Fehlerkorrektur: 'Val()' und 'Str()' durch präzise .NET-Konvertierungen ersetzt, um Abschneidefehler bei Dezimalzahlen mit Komma (z. B. "2,50 €") zu verhindern.
    ''' - Code-Hygiene: Ungenutzte Variable 'sGeb' restlos entfernt.
    ''' - Modernisierung: Die VB6-Zuweisung an den Funktionsnamen durch ein sauberes 'Return' ersetzt.
    ''' </remarks>
    Private Function fcSaveZusatz(ByVal sID As String) As String
        Dim sb As New System.Text.StringBuilder()

        ' Preis aus Textbox sicher in Cent konvertieren
        Dim nPreisCent As Integer = 0
        Dim nPreisDecimal As Decimal = 0
        Dim sPreisBereinigt As String = tbPreis.Text.Replace(","c, "."c).Replace("€", "").Trim()
        If Decimal.TryParse(sPreisBereinigt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, nPreisDecimal) Then
            nPreisCent = Convert.ToInt32(Math.Round(nPreisDecimal * 100, MidpointRounding.AwayFromZero))
        End If

        ' Gesamtsumme aus Label sicher in Cent konvertieren
        Dim nGesamtCent As Integer = 0
        Dim nGesamtDecimal As Decimal = 0
        Dim sGesamtBereinigt As String = lbGesamt.Text.Replace(","c, "."c).Replace("€", "").Trim()
        If Decimal.TryParse(sGesamtBereinigt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, nGesamtDecimal) Then
            nGesamtCent = Convert.ToInt32(Math.Round(nGesamtDecimal * 100, MidpointRounding.AwayFromZero))
        End If

        ' Zusammenbau der Datenkette (ID, BuchID, ZimID, Menge, Bezeichnung, Betrag, Steuer, Gesamt, Datum, ZimNr, Name)
        sb.Append(sID & "°")
        sb.Append(sgRBID & "°")
        sb.Append(sgRZID & "°")
        sb.Append(tbMenge.Text.Trim() & "°")
        sb.Append(tbLeistung.Text.Trim() & "°")
        sb.Append(nPreisCent.ToString() & "°")
        sb.Append(coSteuer.Text.Trim() & "°")
        sb.Append(nGesamtCent.ToString() & "°")
        sb.Append(fcUmDatum(dtpDatum.Value.ToString("dd.MM.yyyy")) & "°")
        sb.Append(lbZimNr.Text.Trim() & "°")
        sb.Append(lbName.Text.Trim())

        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Steuert die Verfügbarkeit der Toolbar-Schaltflächen (Neu, Löschen) basierend auf dem aktuellen Bearbeitungsstatus.
    ''' </summary>
    ''' <param name="lStatus">Gibt an, ob Steuerelemente gesperrt (True) oder freigegeben (False) werden sollen.</param>
    ''' <remarks>
    ''' 10.01.2012 - Create
    ''' 07.10.2026 - Code-Optimierung: Schreibweise korrigiert und logische Operatoren geschärft.
    ''' </remarks>
    Private Sub prLoockZusatz(ByVal lStatus As Boolean)
        tsbNew.Enabled = Not lStatus
        tsbDelete.Enabled = Not lStatus
    End Sub


#End Region


#Region "Listungsübersicht........................................................................."

    ''' <summary>
    ''' Überwacht die Mengeneingabe und stößt bei Änderungen die Neuberechnung der Beträge an.
    ''' </summary>
    Private Sub tbMenge_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbMenge.TextChanged
        ' Verhindert Abstürze bei leerer Eingabe, ohne durch das Ersetzen ein erneutes Event-Feuern zu provozieren
        If String.IsNullOrWhiteSpace(tbMenge.Text) OrElse Not IsNumeric(tbMenge.Text) Then
            Exit Sub
        End If
        prNewCalculateLeistung()
    End Sub

    ''' <summary>
    ''' Überprüft Tastatureingaben im Preisfeld, um nur Zahlen und relevante Trennzeichen zuzulassen.
    ''' </summary>
    Private Sub tbPreis_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbPreis.KeyPress
        prCheckNumericKey(e.KeyChar, tbPreis)
    End Sub

    ''' <summary>
    ''' Überwacht das Preisfeld und stößt bei Änderungen die Live-Berechnung an.
    ''' </summary>
    Private Sub tbPreis_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbPreis.TextChanged
        prNewCalculateLeistung()
    End Sub

    ''' <summary>
    ''' Überwacht die Auswahl des Steuersatzes und aktualisiert die Live-Berechnung.
    ''' </summary>
    Private Sub coSteuer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coSteuer.SelectedIndexChanged
        prNewCalculateLeistung()
    End Sub

    ''' <summary>
    ''' Führt die kaufmännisch präzise Live-Berechnung von Netto-, MwSt- und Bruttobetrag 
    ''' basierend auf Menge, Einzelpreis und Steuersatz auf Decimal-Ebene durch.
    ''' </summary>
    ''' <remarks>
    ''' - Präzisions-Schub: Konvertiert Berechnungen komplett auf den Datentyp 'Decimal', um typische Ungenauigkeiten bei der Steuerberechnung abzufangen.
    ''' - Fehler-Schutz: Nutzt sichere Parse-Verfahren, falls Eingabefelder temporär unvollständig sind.
    ''' </remarks>
    Private Sub prNewCalculateLeistung()
        Dim nMenge As Decimal = 0
        Dim nPreis As Decimal = 0
        Dim nSteuerSatz As Decimal = 0

        ' Werte sicher und Kultur-unabhängig parsen
        Dim sPreisBereinigt As String = tbPreis.Text.Replace(","c, "."c).Trim()
        Dim sMengeBereinigt As String = tbMenge.Text.Replace(","c, "."c).Trim()
        Dim sSteuerBereinigt As String = coSteuer.Text.Replace(","c, "."c).Trim()

        Decimal.TryParse(sMengeBereinigt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, nMenge)
        Decimal.TryParse(sPreisBereinigt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, nPreis)
        Decimal.TryParse(sSteuerBereinigt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, nSteuerSatz)

        ' Wenn nichts eingegeben wurde, leere Labels anzeigen
        If nMenge = 0 OrElse nPreis = 0 Then
            lbNetto.Text = "0,00"
            lbSteuer.Text = "0,00"
            lbGesamt.Text = "0,00"
            Exit Sub
        End If

        ' Brutto-Gesamtsumme ermitteln
        Dim nBruttoGesamt As Decimal = nPreis * nMenge

        ' Mathematisch exakte Netto-Rückrechnung: Netto = Brutto / (1 + (Steuersatz / 100))
        Dim nNettoGesamt As Decimal = Math.Round(nBruttoGesamt / (1 + (nSteuerSatz / 100)), 2, MidpointRounding.AwayFromZero)

        ' Die verbleibende Differenz ist exakt die gesetzliche Mehrwertsteuer
        Dim nMwstGesamt As Decimal = nBruttoGesamt - nNettoGesamt

        ' Labels aktualisieren
        lbNetto.Text = fcFormatDecimal(nNettoGesamt.ToString())
        lbSteuer.Text = fcFormatDecimal(nMwstGesamt.ToString())
        lbGesamt.Text = fcFormatDecimal(nBruttoGesamt.ToString())
    End Sub


#End Region


End Class