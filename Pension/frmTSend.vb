Imports System.Text

Public Class frmTSend
    ''' <summary>
    ''' Formular-Lade-Ereignis: Initialisiert die ListView, parst ausstehende RFID-Transponder-Daten,
    ''' überträgt diese sequenziell an die PHP-Schnittstelle und speichert nicht-gesendete Datensätze für einen späteren Versuch.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Fehlerhafte, doppelte 'fcUmDatum'-Konvertierung korrigiert.
    ''' - Veraltetes VB-spezifisches 'Split()' durch typsicheres '.Split()' ersetzt.
    ''' - Variable 'bFehler' semantisch korrigiert (hieß vorher True, obwohl sie Erfolg trackte).
    ''' - Konstante 'System.Windows.Forms.ColumnHeaderStyle.Nonclickable' auf Kurzschreibweise reduziert.
    ''' - Redundante UI-Refreshes entfernt, um Performance zu steigern.
    ''' </remarks>
    Private Sub frmTSend_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        buEnde.Enabled = False

        ' 1. ListView konfigurieren
        With lvTrans
            .Clear()
            .Columns.Add("Code", 150, HorizontalAlignment.Left)
            .Columns.Add("Von", 100, HorizontalAlignment.Left)
            .Columns.Add("Zeit", 60, HorizontalAlignment.Left)
            .Columns.Add("Bis", 100, HorizontalAlignment.Left)
            .Columns.Add("Zeit", 60, HorizontalAlignment.Left)
            .Columns.Add("Gesendet", 100, HorizontalAlignment.Left)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            .TabIndex = 0
            .View = View.Details
        End With

        ' 2. RFID-Rohdaten aus der System-DB einlesen und aufteilen
        Dim rawDbValue As String = ReadOneValueFromSystemDb("RFIDtem")
        Dim sRFID1 As String = rawDbValue & sgCodeNew
        Dim aRFID As String() = sRFID1.Split(New Char() {"|"c}, StringSplitOptions.RemoveEmptyEntries)

        ' 3. Daten in die ListView eintragen
        For i As Integer = 0 To aRFID.Length - 1
            Dim aCode As String() = aRFID(i).Split("#"c)

            ' Sicherstellen, dass das Array mindestens 5 Elemente (Index 0 bis 4) enthält
            If aCode.Length > 4 Then
                ' Das erste Element (Index 0) als Haupteintrag hinzufügen
                Dim lvItem As ListViewItem = lvTrans.Items.Add(aCode(0))     ' Code

                ' Die restlichen Spalten als SubItems hinzufügen
                lvItem.SubItems.Add(fcUmDatum(aCode(1)))                    ' Von (Datum)
                lvItem.SubItems.Add(aCode(2))                               ' Zeit Von
                lvItem.SubItems.Add(fcUmDatum(aCode(3)))                    ' Bis (Datum)
                lvItem.SubItems.Add(aCode(4))                               ' Zeit Bis
                lvItem.SubItems.Add("")                                     ' Sendestatus (initial leer)
            End If
        Next

        Me.Refresh()

        ' 4. Daten verarbeiten und per PHP übertragen
        Dim allTransmissionsSuccessful As Boolean = True

        For i As Integer = 0 To lvTrans.Items.Count - 1
            Dim currentItem As ListViewItem = lvTrans.Items(i)

            ' Datensegmente für Konvertierung vorbereiten (WICHTIG: Das Datum liegt in der ListView bereits konvertiert vor)
            Dim itemCode As String = currentItem.SubItems(0).Text
            Dim dateFrom As String = currentItem.SubItems(1).Text
            Dim timeFrom As String = currentItem.SubItems(2).Text
            Dim dateTo As String = currentItem.SubItems(3).Text
            Dim timeTo As String = currentItem.SubItems(4).Text

            ' Verschlüsselten/Formatierten String für PHP aufbauen
            Dim sCode As String = $"{itemCode};{fcRFIDDatum(dateFrom)};{fcRFIDZeit(timeFrom)};{fcRFIDDatum(dateTo)};{fcRFIDZeit(timeTo)}"
            sCode = fcUmCode(sCode) & "|"

            ' Datenübertragung via PHP-Klasse prüfen
            If PHP.Data(sCode, arIni(34)) = False Then
                currentItem.SubItems(5).Text = "False"
                allTransmissionsSuccessful = False
            Else
                currentItem.SubItems(5).Text = "True"
            End If

            currentItem.EnsureVisible() ' Automatisch zum aktuellen Element scrollen
            Me.Refresh()

            ' Kürzere, kontrollierte Wartezeit (1 Sekunde laut Altcode)
            fcWait(1)
        Next

        ' 5. Fehlerhafte/Nicht gesendete Einträge sammeln und zurück in die DB schreiben
        Dim sbFailedCodes As New StringBuilder()

        For i As Integer = 0 To lvTrans.Items.Count - 1
            Dim currentItem As ListViewItem = lvTrans.Items(i)

            If currentItem.SubItems(5).Text = "False" Then
                sbFailedCodes.Append(currentItem.SubItems(0).Text).Append("#")
                sbFailedCodes.Append(currentItem.SubItems(1).Text).Append("#")
                sbFailedCodes.Append(currentItem.SubItems(2).Text).Append("#")
                sbFailedCodes.Append(currentItem.SubItems(3).Text).Append("#")
                sbFailedCodes.Append(currentItem.SubItems(4).Text).Append("|")
            End If
        Next

        SaveOneValueInSystemDb("RFIDtem", sbFailedCodes.ToString())

        ' 6. Abschlussprüfung: Wenn alles fehlerfrei war, schließt sich das Fenster automatisch
        If allTransmissionsSuccessful Then
            Me.Close()
        End If

        buEnde.Enabled = True
    End Sub

    ''' <summary>
    ''' Schließt das Sende-Formular manuell.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.02.2012 - Create
    ''' </remarks>
    Private Sub buEnde_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buEnde.Click
        Me.Close()
    End Sub

End Class