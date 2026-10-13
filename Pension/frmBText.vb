Public Class frmBText

    Dim nPos As Integer = 3
    Dim nZeichen As Integer


#Region "Formular initialisieren..................................................................."

    ''' <summary>
    ''' Initialisiert das Formular beim Laden. 
    ''' Formatiert die Steuerelemente, setzt die Zeichenbegrenzung und lädt vordefinierte Textbausteine in die ComboBox.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 09.10.2026 - Code-Optimierung:
    ''' - SQL-Abfrage optimiert (unnötige String-Verkettung entfernt).
    ''' - Ressourcenmanagement durch 'Using'-Block für die DataTable implementiert.
    ''' - Performance-Optimierung: Schleife durch direkte DataSource-Bindung der ComboBox ersetzt.
    ''' </remarks>
    Private Sub frmBText_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hintergrundfarbe einheitlich festlegen
        Dim sColor As Color = Color.LightYellow
        Me.BackColor = sColor
        tsMain.BackColor = sColor

        ' Standardwert für Zeichenanzeige setzen
        tslZeichen.Text = "255"

        ' Position basierend auf Sprachauswahl anpassen
        If lLang Then nPos = 4

        ' Daten aus der Datenbank lesen und ComboBox befüllen
        Using dt As DataTable = fcReadDataTable("Select * from BTexte Where Art='0'")
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                ' Effizientes Befüllen der ComboBox via DataBinding statt For-Loop
                tscoBText.ComboBox.ValueMember = dt.Columns(0).ColumnName
                tscoBText.ComboBox.DisplayMember = dt.Columns(1).ColumnName
                tscoBText.ComboBox.DataSource = dt
            End If
        End Using

        ' Verbleibende Zeichen berechnen und anzeigen
        nZeichen = tbBText.Text.Length
        tslZeichen.Text = (255 - nZeichen).ToString()
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
    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Aktiviert die Hinzufügen-Schaltfläche, wenn sich der Text in der ComboBox ändert.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 09.10.2026 - Code-Optimierung:
    ''' - Event-Handler für Textänderung optimiert.
    ''' </remarks>
    Private Sub tscoBText_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tscoBText.TextChanged
        tsbAdd.Enabled = True
    End Sub

    ''' <summary>
    ''' Aktualisiert die verbleibende Zeichenanzahl und aktiviert die Speichern-Schaltfläche bei Textänderung.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 09.10.2026 - Code-Optimierung:
    ''' - Berechnung der verbleibenden Zeichen vereinheitlicht.
    ''' </remarks>
    Private Sub tbBText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBText.TextChanged
        tsbSpeichern.Enabled = True
        nZeichen = tbBText.Text.Length
        tslZeichen.Text = (255 - nZeichen).ToString()
    End Sub

    ''' <summary>
    ''' Fügt das ausgewählte Makro an den bestehenden Text an und deaktiviert die Hinzufügen-Schaltfläche.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 09.10.2026 - Code-Optimierung:
    ''' - String-Verkettung auf den modernen '+=' Operator umgestellt.
    ''' </remarks>
    Private Sub tsbAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAdd.Click
        tsbAdd.Enabled = False
        tbBText.Text += fcAddMakro(tscoBText.Text, nPos)
    End Sub

    ''' <summary>
    ''' Speichert den Text im entsprechenden Reservierungsformular, abhängig davon, ob ein Tablet genutzt wird.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 09.10.2026 - Code-Optimierung:
    ''' - Code-Formatierung bereinigt und überflüssige Leerzeilen entfernt.
    ''' </remarks>
    Private Sub tsbSpeichern_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSpeichern.Click
        tsbSpeichern.Enabled = False

        If cgTablet = 0 Then
            frmReservierungDest.lbMakro.Text = tbBText.Text
        Else
            frmReservierung.lbMakro.Text = tbBText.Text
        End If
    End Sub

    ''' <summary>
    ''' Liest ein Makro basierend auf dem Namen aus der Datenbank und gibt den Inhalt der definierten Spaltenposition zurück.
    ''' </summary>
    ''' <param name="sName">Der Name des zu suchenden Textbausteins.</param>
    ''' <param name="nPos">Der Spaltenindex des gewünschten Textinhaltes.</param>
    ''' <returns>Der gefundene Makrotext gefolgt von einem Zeilenumbruch, oder ein leerer String.</returns>
    ''' <remarks>
    ''' 09.10.2026 - Code-Optimierung:
    ''' - Ressourcenmanagement: 'Using'-Block für DataTable eingeführt.
    ''' - Stabilitäts-Upgrade: Null- und Indexprüfungen vor dem Datenzugriff implementiert.
    ''' - Veraltete implizite Funktionsrückgabe durch explizites 'Return' ersetzt.
    ''' </remarks>
    Private Function fcAddMakro(ByVal sName As String, ByVal nPos As Integer) As String
        Using dt As DataTable = fcReadDataTable("Select * from BTexte Where Name='" & sName & "'")
            ' Prüfen, ob Daten vorhanden sind und der Spaltenindex existiert
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso dt.Columns.Count > nPos Then
                Return dt.Rows(0).Item(nPos).ToString() & vbCrLf
            End If
        End Using

        Return String.Empty
    End Function

#End Region
End Class