Public Class frmCheckin


    ''' <summary>
    ''' Initialisiert das Check-in-Formular, baut die Spalten der ListView dynamisch auf 
    ''' und befüllt diese mit den ausgelesenen Buchungsdaten.
    ''' </summary>
    ''' <param name="sender">Das auslösende Steuerelement.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' <para><strong>Optimierung &amp; Absicherung:</strong></para>
    ''' <para>Datum: 24.09.2026</para>
    ''' <para>Grund: Es wurden Sicherheitsprüfungen für die Array-Längen (<c>aText</c> und <c>aName</c>) integriert, um Abstürze (<c>IndexOutOfRangeException</c>) bei leeren Datenbank-Rückgaben zu verhindern. Zudem wurde die Performance durch <c>BeginUpdate</c> und <c>EndUpdate</c> bei der ListView-Befüllung massiv verbessert und ein Fehler-Logging (<c>Try-Catch</c>) hinzugefügt.</para>
    ''' </remarks>
    Private Sub frmCheckin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' 1. Daten abrufen
            Dim sText As String = fcTabelView("Checkin", "BID='" & sgRBID & "'", "ZID;Name;Vorname;Straße;PLZ;Ort;GDatum", "Zimmer;Name;Vorname;Straße;PLZ;Ort;E-Mail")

            ' Validierung: Wenn keine Daten zurückkommen oder der String leer ist, abbrechen
            If String.IsNullOrWhiteSpace(sText) Then Return

            Dim aText() As String = sText.Split(New String() {vbCrLf}, StringSplitOptions.None)
            If aText.Length < 2 Then Return ' Es müssen mindestens Längen- und Namenszeile existieren

            Dim aLength() As String = aText(0).Split("|"c)
            Dim aName() As String = aText(1).Split("|"c)

            ' 2. ListView-Spalten vorbereiten
            With lvTabel
                .Clear()
                .View = View.Details
                .FullRowSelect = True
                .GridLines = True
                .HeaderStyle = ColumnHeaderStyle.Nonclickable
                .HideSelection = False
                .MultiSelect = False
                .TabIndex = 0

                ' Spalten dynamisch hinzufügen
                For i As Integer = 0 To aName.Length - 1
                    Dim iBreite As Integer = 100 ' Standardbreite, falls Konvertierung fehlschlägt
                    If i < aLength.Length Then
                        iBreite = Val(aLength(i)) * 9
                    End If
                    .Columns.Add(aName(i), iBreite, HorizontalAlignment.Left)
                Next
            End With

            ' 3. ListView-Zeilen effizient befüllen
            If aText.Length > 2 Then
                lvTabel.BeginUpdate() ' Verhindert Flackern und erhöht die Performance enorm

                For i As Integer = 2 To aText.Length - 1
                    If String.IsNullOrWhiteSpace(aText(i)) Then Continue For

                    Dim aRowData() As String = aText(i).Split("|"c)
                    If aRowData.Length = 0 Then Continue For

                    ' Zimmernamen ermitteln
                    Dim sZim As String = fcGetObjektZimmerName(dtZim, aRowData(0))
                    If String.IsNullOrWhiteSpace(sZim) Then sZim = aRowData(0)

                    ' Haupteintrag hinzufügen
                    Dim lvItem As ListViewItem = lvTabel.Items.Add(sZim)

                    ' Untereinträge (SubItems) hinzufügen
                    For j As Integer = 1 To aRowData.Length - 1
                        lvItem.SubItems.Add(aRowData(j))
                    Next
                Next

                lvTabel.EndUpdate() ' Zeichnen der ListView wieder aktivieren
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Schließt das aktuelle Formular.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 09.10.2026 - Code-Optimierung:
    ''' - Standard-Event-Handler dokumentiert.
    ''' </remarks>
    Private Sub buClose_Click(sender As Object, e As EventArgs) Handles buClose.Click
        Me.Close()
    End Sub
End Class