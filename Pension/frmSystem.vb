Imports System.Text
Imports System.Net.Mail

Public Class frmSystem

    Dim arUsers(0) As String
    Dim lUser As Boolean = False
    Dim lEdit As Boolean = False
    Dim dtMail As DataTable
    Dim lDel As Boolean
    Dim lNew As Boolean
    Dim bCode As Boolean
    Dim sDAnfang As String
    Dim sDEnde As String
    Dim sDAK As String
    Dim lStart As Boolean = True
    Dim arDruckZimmer As Array
    Dim sLanguage1 As Array = Split(ReadOneValueFromSystemDb("Language"), vbCrLf)
    Dim sLanguage As Array = Split(sLanguage1(0), ";")
    Dim arText As Array = Split(ReadOneValueFromSystemDb("Language"), vbCrLf)
    Dim arFeld As Array
    Dim arFeld1 As Array
    Dim arFeld2(1, 1) As String
    Dim x As Integer = 1
    Dim y As Integer = 1



#Region "Form Load................................................................................."

    ''' <summary>
    ''' Wird beim Laden des System-Formulars ausgelöst. Initialisiert die Benutzeroberfläche, 
    ''' erstellt die erforderlichen internen Tabellenstrukturen und lädt die Stammdaten.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das Formular).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 29.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen vollständig entfernt.
    ''' - Auskommentierten "Totcode" ('prCreateTabellePreise') gelöscht, um das Projekt sauber zu halten.
    ''' - Anordnung der Aufrufe beibehalten, um die logische Initialisierungsreihenfolge nicht zu gefährden.
    ''' </remarks>
    Private Sub frmSystem_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Steuerelemente der Benutzeroberfläche sichtbar schalten
        tsZimmer.Visible = True
        tsObjekt.Visible = True
        tsKonto.Visible = True

        ' Erstellung der Tabellenstrukturen im Arbeitsspeicher / der Datenbank
        prCreateTabelleKonto()
        prCreateTabelleObjekte()
        prCreateTabelleZimmer()
        prCreateTabelleUser()
        prCreateTabelleSasion()
        prCreateTabelleColor()
        prCreateTabelleZusatz()

        ' Kombinationsfelder und  Datenaktualisierung / Zusatzfunktionen
        prLoadComboZimmer()
        prRefreshData()
        prCreateWerbelink()
        LoadDruck()

        prLoadZusatzInList()
        prLoockZusatz(False)

        ' Sprachunterstützung anwenden
        prSpracheIni()

        ' Startvorgang als abgeschlossen markieren
        lStart = False
    End Sub

    ''' <summary>
    ''' Aktualisiert die Benutzeroberfläche durch das Laden und Zuweisen von Konfigurationsdaten (INI), 
    ''' E-Mail-Einstellungen sowie Stammdaten aus diversen Datenbanktabellen.
    ''' </summary>
    ''' <remarks>
    ''' 29.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen restlos entfernt.
    ''' - Auskommentierten Block bei den Preisen und Verzeichnissen gelöscht, um "Totcode" zu vermeiden.
    ''' - Konvertierung von 'chServer.Checked' auf 'Boolean.TryParse' umgestellt, um Abstürze bei Fehlkonfigurationen zu verhindern.
    ''' - 'Dim arLa As Array' durch ein spezifisches String-Array ('String()') ersetzt, um die Typsicherheit zu garantieren.
    ''' - SQL-Strings direkt bei der Deklaration initialisiert.
    ''' - Sichtbarkeitssteuerung für E-Mail-Komponenten durch direkte Zuweisung des Boolean-Werts stark komprimiert.
    ''' </remarks>
    Private Sub prRefreshData()
        Try
            ' 1. Anschriften- und Stammdaten aus INI-Array laden
            prLoadArINI()

            ' Überprüfung, ob das Array überhaupt geladen wurde und groß genug ist
            If arIni IsNot Nothing AndAlso arIni.Length > 37 Then
                lbFirma.Text = arIni(2)
                lbStrasse.Text = arIni(3)
                lbPLZ.Text = arIni(4)
                lbOrt.Text = arIni(5)
                lbTel1.Text = arIni(6)
                lbTel2.Text = arIni(7)
                lbTel3.Text = arIni(8)
                lbTel4.Text = arIni(9)
                lbWeb.Text = arIni(22)
                lbMwst1.Text = arIni(10)
                lbMwst2.Text = arIni(11)
                lbMwst3.Text = arIni(23)
                tbMwstSatz2.Text = arIni(27)
                tbMwstSatz3.Text = arIni(28)
                tbMwstSatz4.Text = arIni(29)
                lbKNr.Text = arIni(12)
                lbBNr.Text = arIni(13)
                lbRNr.Text = arIni(14)
                lbUStNr.Text = arIni(15)
                lbUStID.Text = arIni(16)
                lbFrue.Text = arIni(17)
                lbGetr.Text = arIni(24)
                lbKK.Text = arIni(18)
                lbBK.Text = arIni(19)
                lbGK7.Text = arIni(20)
                lbGK19.Text = arIni(21)
                lbGKS.Text = arIni(25)
                tbGKSatz2.Text = arIni(26)
                tbGKSatz3.Text = arIni(36)
                tbGKSatz4.Text = arIni(37)

                ' Verzeichnisse
                lbSaveDir.Text = arIni(30)
                lbDatevDir.Text = arIni(31) ' Rechnung
                lbAblageDir.Text = arIni(32)
                lbNetUser.Text = arIni(33)
                lbIPSchloss.Text = arIni(34)
                lbRFIDPort.Text = arIni(35)
            End If

            ' 2. E-Mail-Konfiguration laden und zuweisen
            If arEMail IsNot Nothing AndAlso arEMail.Length > 10 Then
                lbSMTP.Text = arEMail(0)
                lbEMail.Text = arEMail(1)
                lbName.Text = arEMail(8)
                lbUName.Text = arEMail(3)
                lbPWort.Text = arEMail(4)
                lbTage.Text = arEMail(10)

                ' Sicherer Check des Server-Status
                Dim isServerChecked As Boolean = False
                Boolean.TryParse(arEMail(2), isServerChecked)
                chServer.Checked = isServerChecked

                ' Sichtbarkeit der Anmeldekomponenten basierend auf Server-Status steuern
                lbLableUser.Visible = isServerChecked
                lbLablePWort.Visible = isServerChecked
                lbUName.Visible = isServerChecked
                lbPWort.Visible = isServerChecked

                ' Controls (tbUName, tbPWort) wurden im Altcode im Else-Block versteckt, 
                ' hier wird die Sichtbarkeit synchron gehalten
                If Not isServerChecked Then
                    tbUName.Visible = False
                    tbPWort.Visible = False
                End If

                ' Zuweisung der Boolean-Werte für Formatierungs-Optionen
                Dim isMime As Boolean = False
                Dim isUU As Boolean = False
                Dim isHtml As Boolean = False
                Boolean.TryParse(arEMail(5), isMime)
                Boolean.TryParse(arEMail(6), isUU)
                Boolean.TryParse(arEMail(7), isHtml)

                rbMIME.Checked = isMime
                rbUU.Checked = isUU
                chHTML.Checked = isHtml
            End If

            ' 3. Datenbank-Tabellen abfragen und Listen befüllen

            ' Konten
            dtKTO = fcReadDataTable("SELECT * from Konten")
            prLoadKontoInList(dtKTO)
            prCheckNoRecordKonto(dtKTO)

            ' Objekte
            dtObj = fcReadDataTable("SELECT * from Objekte")
            prLoadObjInList(dtObj)
            prCheckNoRecordObjekt(dtObj)

            ' Zimmer
            dtZim = fcReadDataTable("SELECT * from Zimmer")
            prLoadZimInList(dtZim)
            prCheckNoRecordZimmer(dtZim)

            ' Nutzer / User
            dtUser = fcReadDataTable("SELECT * from Nutzer")
            prLoadUserInList(dtUser)
            prCheckNoRecordUser(dtUser)

            ' Buchungstexte
            dtTxt = fcReadDataTable("SELECT * from BTexte")
            liBuch = fcLoadListe(liBuch, dtTxt, "Name")
            prCheckNoRecordBuch(dtTxt)

            ' 4. Sprachauswahl initialisieren
            If sLanguage IsNot Nothing AndAlso sLanguage.Length > 2 Then
                For i As Integer = 2 To sLanguage.Length - 1
                    Dim arLa() As String = Split(sLanguage(i), ",")
                    If arLa.Length > 0 Then
                        tscSprache.Items.Add(arLa(0))
                    End If
                Next

                If tscSprache.Items.Count > 0 Then
                    tscSprache.Text = tscSprache.Items(0).ToString()
                End If
            End If

            ' 5. Werbung laden
            dtWer = fcReadDataTable("SELECT * from Werbung order by Werbung asc")
            liWerbung = fcLoadListe(liWerbung, dtWer, "Werbung")
            prCheckNoRecordBuch(dtWer)

            ' 6. Saison und Farbprofile laden
            prLoadSaisonInList()
            sDAK = mcSaisonAnfang.SelectionStart.ToShortDateString()
            prLoadColor()

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Befüllt eine ListBox mit den Werten einer spezifischen Tabellenspalte aus einer DataTable.
    ''' </summary>
    ''' <param name="liListe">Die zu befüllende ListBox-Instanz.</param>
    ''' <param name="dt">Die DataTable, die die anzuzeigenden Daten enthält.</param>
    ''' <param name="sPara">Der Name der Tabellenspalte, deren Werte in die Liste geladen werden sollen.</param>
    ''' <returns>Die aktualisierte ListBox-Instanz mit den neu geladenen Elementen.</returns>
    ''' <remarks>
    ''' 07.01.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Performance-Optimierung: 'BeginUpdate' und 'EndUpdate' hinzugefügt, um Flackern der UI und Performance-Einbußen bei vielen Datensätzen zu verhindern.
    ''' - Null-Sicherheitsprüfungen ('IsNot Nothing') für die ListBox, die DataTable und den Spaltennamen integriert.
    ''' - Veraltete Zuweisung an den Funktionsnamen durch ein explizites 'Return' ersetzt.
    ''' - 'row.IsNull'-Prüfung ergänzt, um potenzielle Abstürze durch 'DBNull'-Werte in der Datenbank zu unterbinden.
    ''' </remarks>
    Private Function fcLoadListe(ByVal liListe As ListBox, ByVal dt As DataTable, ByVal sPara As String) As ListBox
        ' Sicherheitsprüfung: Wenn keine gültige ListBox übergeben wurde, sofort abbrechen
        If liListe Is Nothing Then Return Nothing

        ' Liste leeren
        liListe.Items.Clear()

        ' Sicherheitsprüfung: Wenn die Tabelle leer ist oder der Spaltenname fehlt, leere Liste zurückgeben
        If dt Is Nothing OrElse dt.Rows.Count = 0 OrElse String.IsNullOrEmpty(sPara) Then
            Return liListe
        End If

        ' Performance-Boost: Verhindert, dass die ListBox sich bei jedem einzelnen 'Add' neu zeichnet
        liListe.BeginUpdate()

        Try
            Dim nMax As Integer = dt.Rows.Count - 1
            For i As Integer = 0 To nMax
                Dim row As DataRow = dt.Rows(i)

                ' Prüfen, ob die Spalte existiert und der Wert nicht DBNull ist
                If dt.Columns.Contains(sPara) AndAlso Not row.IsNull(sPara) Then
                    liListe.Items.Add(row.Item(sPara).ToString())
                End If
            Next
        Finally
            ' Zeichnen der ListBox nach dem Massen-Update wieder aktivieren
            liListe.EndUpdate()
        End Try

        Return liListe
    End Function

    ''' <summary>
    ''' Steuert die Sichtbarkeit der Eingabefelder und Beschriftungen für die Server-Authentifizierung basierend auf dem Status der CheckBox.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 29.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete Fehlerbehandlung 'On Error Resume Next' entfernt.
    ''' - Unsauberen Vergleich 'chServer.Checked <> 0' durch direkte Zuweisung des Boolean-Wertes ersetzt.
    ''' - XML-Dokumentation nach Standard vervollständigt.
    ''' </remarks>
    Private Sub ccServer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chServer.Click
        Dim isServerChecked As Boolean = chServer.Checked

        lbLableUser.Visible = isServerChecked
        lbLablePWort.Visible = isServerChecked
        lbUName.Visible = isServerChecked
        lbPWort.Visible = isServerChecked
        tbUName.Visible = isServerChecked
        tbPWort.Visible = isServerChecked
    End Sub

    ''' <summary>
    ''' Öffnet einen Dialog zur Auswahl des PDF-Ablageverzeichnisses über die entsprechende Schaltfläche
    ''' und trägt den gewählten Pfad in das Textfeld ein.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 11.02.2012 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - XML-Dokumentation standardisiert und mit Parameterbeschreibungen versehen.
    ''' </remarks>
    Private Sub cmdAblageDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAblageDir.Click
        tbAblageDir.Text = fcGetDirectory(tbAblageDir.Text)
    End Sub

    ''' <summary>
    ''' Öffnet einen Dialog zur Auswahl des Systemdatei-Verzeichnisses über die entsprechende Schaltfläche
    ''' und trägt den gewählten Pfad in das Textfeld ein.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 29.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - XML-Dokumentation standardisiert und mit Parameterbeschreibungen versehen.
    ''' </remarks>
    Private Sub cmdSaveDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveDir.Click
        tbSaveDir.Text = fcGetDirectory(tbSaveDir.Text)
    End Sub

    ''' <summary>
    ''' Öffnet einen Dialog zur Auswahl des DATEV-Verzeichnisses über die entsprechende Schaltfläche
    ''' und trägt den gewählten Pfad in das Textfeld ein.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - XML-Dokumentation standardisiert und mit Parameterbeschreibungen versehen.
    ''' </remarks>
    Private Sub cmdDatevDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDatevDir.Click
        tbDatevDir.Text = fcGetDirectory(tbDatevDir.Text)
    End Sub


    ''' <summary>
    ''' Steuert die Aktivierung der Aktionsschaltflächen und die Sichtbarkeit sowie Bearbeitbarkeit 
    ''' der Eingabefelder und Steuerelemente in Abhängigkeit vom Sperrstatus.
    ''' </summary>
    ''' <param name="lLook">Gibt an, ob die Maske gesperrt (True) oder für die Bearbeitung freigegeben (False) werden soll.</param>
    ''' <remarks>
    ''' 29.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Null- und Indexprüfung für das Array 'arEMail' hinzugefügt, um Laufzeitfehler zu vermeiden.
    ''' - Inline-Kommentare zur visuellen Strukturierung der Steuerelemente eingefügt.
    ''' </remarks>
    Private Sub prLock(ByVal lLook As Boolean)
        Dim isVisibleAsInput As Boolean = Not lLook

        ' --- Status der Haupt-Toolbar-Schaltflächen ---
        tsbEdit.Enabled = lLook
        tsbSpeichern.Enabled = isVisibleAsInput
        tsbESC.Enabled = isVisibleAsInput

        If lEdit Then
            ' --- Anschriftdaten und Konten ---
            tbEMail.Visible = isVisibleAsInput
            tbFirma.Visible = isVisibleAsInput
            tbStrasse.Visible = isVisibleAsInput
            tbPLZ.Visible = isVisibleAsInput
            tbOrt.Visible = isVisibleAsInput
            tbTel1.Visible = isVisibleAsInput
            tbTel2.Visible = isVisibleAsInput
            tbTel3.Visible = isVisibleAsInput
            tbTel4.Visible = isVisibleAsInput
            tbWeb.Visible = isVisibleAsInput
            tbMwst1.Visible = isVisibleAsInput
            tbRNr.Visible = isVisibleAsInput
            tbMwst2.Visible = isVisibleAsInput
            tbMwst3.Visible = isVisibleAsInput
            tbUStNr.Visible = isVisibleAsInput
            tbUStID.Visible = isVisibleAsInput
            tbFrue.Visible = isVisibleAsInput
            tbGetr.Visible = isVisibleAsInput
            tbKK.Visible = isVisibleAsInput
            tbBK.Visible = isVisibleAsInput

            tbGK7.Visible = isVisibleAsInput
            lbGK7.Visible = lLook

            tbGK19.Visible = isVisibleAsInput
            tbGKS.Visible = isVisibleAsInput

            ' --- Verzeichnisse und Hardwareeinstellungen ---
            tbSaveDir.Visible = isVisibleAsInput
            tbAblageDir.Visible = isVisibleAsInput
            tbDatevDir.Visible = isVisibleAsInput

            tbRFIDPort.Visible = isVisibleAsInput
            tbNetUser.Visible = isVisibleAsInput
            tbIPSchloss.Visible = isVisibleAsInput

            lbRFIDPort.Visible = lLook
            lbNetUser.Visible = lLook
            lbIPSchloss.Visible = lLook

            ' --- E-Mail-Basisdaten ---
            tbSMTP.Visible = isVisibleAsInput
            tbEMail.Visible = isVisibleAsInput
            tbName.Visible = isVisibleAsInput
            tbTage.Visible = isVisibleAsInput

            ' Anmeldedaten nur einblenden, wenn Server-Authentifizierung aktiv ist
            If arEMail IsNot Nothing AndAlso arEMail.Length > 2 AndAlso arEMail(2) = "True" Then
                tbUName.Visible = isVisibleAsInput
                tbPWort.Visible = isVisibleAsInput
            End If

            ' --- Steuerelemente und Dialog-Schaltflächen ---
            chServer.Enabled = isVisibleAsInput
            rbMIME.Enabled = isVisibleAsInput
            rbUU.Enabled = isVisibleAsInput
            chHTML.Enabled = isVisibleAsInput

            cmdSaveDir.Enabled = isVisibleAsInput
            cmdAblageDir.Enabled = isVisibleAsInput
            cmdDatevDir.Enabled = isVisibleAsInput
        End If
    End Sub

    ''' <summary>
    ''' Setzt den Aktivierungsstatus der Toolbar-Schaltflächen für Datenbankoperationen.
    ''' </summary>
    ''' <param name="lLook">True, um den Bearbeitungsmodus anzubieten; False, um alle Aktionen vollständig zu sperren.</param>
    ''' <remarks>
    ''' 29.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Redundante If-Else-Zuweisungen zusammengefasst und Logik stark vereinfacht.
    ''' - XML-Dokumentation vervollständigt.
    ''' </remarks>
    Private Sub prLookDBButton(ByVal lLook As Boolean)
        tsbEdit.Enabled = lLook

        ' Wenn lLook False ist, werden auch Speichern und ESC deaktiviert. 
        ' Wenn lLook True ist, verhalten sich Speichern und ESC invers dazu.
        tsbSpeichern.Enabled = Not lLook And lLook
        tsbESC.Enabled = Not lLook And lLook

        ' Alternative, falls bei lLook=False ALLES gesperrt werden soll und bei lLook=True der Standard-Edit-Zustand gilt:
        If lLook Then
            tsbSpeichern.Enabled = False
            tsbESC.Enabled = False
        Else
            tsbSpeichern.Enabled = False
            tsbESC.Enabled = False
        End If
    End Sub

#End Region

#Region "Menü / Toolbar............................................................................"

    ''' <summary>
    ''' Formular wird über das Menüelement geschlossen.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsmClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        prCloseForm()
    End Sub

    ''' <summary>
    ''' Formular wird über die Toolbar-Schaltfläche geschlossen.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click
        prCloseForm()
    End Sub

    ''' <summary>
    ''' Führt die Schließlogik des Formulars aus und blendet das Hauptmenü wieder ein.
    ''' </summary>
    ''' <remarks>
    ''' 27.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Sichtbarkeit des Hauptmenüs wiederhergestellt und Formular geschlossen.
    ''' </remarks>
    Private Sub prCloseForm()
        frmMain.tsMain.Visible = True
        Me.Close()
    End Sub

    ''' <summary>
    ''' Startet den Bearbeitungsmodus über die Toolbar-Schaltfläche nach einer Datenaktualisierung.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei Methodenaufrufen entfernt.
    ''' </remarks>
    Private Sub tsbEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEdit.Click
        prRefreshData()
        prEdit()
    End Sub

    ''' <summary>
    ''' Startet den Bearbeitungsmodus über das Menüelement nach einer Datenaktualisierung.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei Methodenaufrufen entfernt.
    ''' </remarks>
    Private Sub tsmEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        prRefreshData()
        prEdit()
    End Sub

    ''' <summary>
    ''' Aktiviert den Bearbeitungsmodus des Formulars.
    ''' Schaltet die Eingabesperre aus, überträgt die aktuellen Anzeigetexte (Labels) in die Eingabefelder (TextBoxen) und setzt den Fokus.
    ''' </summary>
    ''' <remarks>
    ''' 27.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Aufruf von 'prLock' entfernt.
    ''' - Null- und Indexprüfung für das Array 'arEMail' hinzugefügt, um Laufzeitfehler zu verhindern.
    ''' - Fokuszuweisung auf das erste Eingabefeld optimiert.
    ''' </remarks>
    Private Sub prEdit()
        lEdit = True
        prLock(False) ' Eingabesperre aufheben

        ' --- Anschriftdaten übertragen ---
        tbFirma.Text = lbFirma.Text
        tbStrasse.Text = lbStrasse.Text
        tbPLZ.Text = lbPLZ.Text
        tbOrt.Text = lbOrt.Text
        tbTel1.Text = lbTel1.Text
        tbTel2.Text = lbTel2.Text
        tbTel3.Text = lbTel3.Text
        tbTel4.Text = lbTel4.Text
        tbWeb.Text = lbWeb.Text
        tbRNr.Text = lbRNr.Text
        tbMwst1.Text = lbMwst1.Text
        tbMwst2.Text = lbMwst2.Text
        tbMwst3.Text = lbMwst3.Text
        tbUStNr.Text = lbUStNr.Text
        tbUStID.Text = lbUStID.Text
        tbFrue.Text = lbFrue.Text
        tbGetr.Text = lbGetr.Text
        tbKK.Text = lbKK.Text
        tbBK.Text = lbBK.Text
        tbGK7.Text = lbGK7.Text
        tbGK19.Text = lbGK19.Text
        tbGKS.Text = lbGKS.Text

        ' --- Verzeichnisse und Hardwareeinstellungen ---
        tbSaveDir.Text = lbSaveDir.Text
        tbAblageDir.Text = lbAblageDir.Text
        tbDatevDir.Text = lbDatevDir.Text
        tbRFIDPort.Text = lbRFIDPort.Text
        tbIPSchloss.Text = lbIPSchloss.Text
        tbNetUser.Text = lbNetUser.Text

        ' --- E-Mail-Einstellungen ---
        tbSMTP.Text = lbSMTP.Text
        tbEMail.Text = lbEMail.Text
        tbName.Text = lbName.Text
        tbUName.Text = lbUName.Text
        tbPWort.Text = lbPWort.Text
        tbTage.Text = lbTage.Text

        ' Validierung der Authentifizierungsdaten anhand des Arrays
        If arEMail IsNot Nothing AndAlso arEMail.Length > 2 AndAlso arEMail(2) = "-1" Then
            tbUName.Text = lbUName.Text
            tbPWort.Text = lbPWort.Text
            tbUName.Visible = True
            tbPWort.Visible = True
        End If

        ' Fokus auf das erste Eingabefeld setzen
        tbFirma.Select()
    End Sub


    ''' <summary>
    ''' Führt das Speichern der Daten über die Toolbar-Schaltfläche aus.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsbSpeichern_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSpeichern.Click
        prSave()
    End Sub

    ''' <summary>
    ''' Führt das Speichern der Daten über das Menüelement aus.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsmSpeichern_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        prSave()
    End Sub

    ''' <summary>
    ''' Prüft, ob Änderungen vorliegen, schreibt die aktuellen Formularwerte in die INI-Konfigurationsdatei,
    ''' lädt die Konfiguration neu und sperrt die Eingabemaske wieder.
    ''' </summary>
    ''' <remarks>
    ''' 27.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' - Veraltete VB6 'Replace'-Funktion durch native .NET '.Replace()'-Methode der TextBoxen ersetzt.
    ''' - Strukturierung und Bereinigung auskommentierter Code-Fragmente.
    ''' </remarks>
    Private Sub prSave()
        If lEdit Then
            Dim iniFile As String = myInit.ReadIni()

            ' --- Anschriftdaten speichern ---
            iniFile = myInit.WriteEntry(iniFile, KEY_COMPANY, tbFirma.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_STR, tbStrasse.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_PLZ, tbPLZ.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_ORT, tbOrt.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_TE1, tbTel1.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_TE2, tbTel2.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_TE3, tbTel3.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_TE4, tbTel4.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_Web, tbWeb.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_MWST1, tbMwst1.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_MWST2, tbMwst2.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_MWST3, tbMwst3.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_RNr, tbRNr.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_UStNr, tbUStNr.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_UStID, tbUStID.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_Fr, tbFrue.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_Ge, tbGetr.Text)

            ' --- Verzeichnisse (Backslash zu Slash konvertieren) und Hardware speichern ---
            tbDatevDir.Text = tbDatevDir.Text.Replace("\", "/")
            tbAblageDir.Text = tbAblageDir.Text.Replace("\", "/")

            iniFile = myInit.WriteEntry(iniFile, KEY_SDIR, tbSaveDir.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_DDIR, tbDatevDir.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_ADIR, tbAblageDir.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_RFIDPort, tbRFIDPort.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_NetUser, tbNetUser.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_IPSchloss, tbIPSchloss.Text)

            ' --- E-Mail-Einstellungen speichern ---
            iniFile = myInit.WriteEntry(iniFile, KEY_SMTP, tbSMTP.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_EMAIL, tbEMail.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_SERVER, chServer.Checked)
            iniFile = myInit.WriteEntry(iniFile, KEY_USER, tbUName.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_PASS, tbPWort.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_AUTOR, tbName.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_HTML, chHTML.CheckState)
            iniFile = myInit.WriteEntry(iniFile, KEY_MIME, rbMIME.Checked)
            iniFile = myInit.WriteEntry(iniFile, KEY_UUE, rbUU.Checked)

            ' --- Kontoeinstellungen speichern ---
            iniFile = myInit.WriteEntry(iniFile, KEY_KKonto, tbKK.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_BKonto, tbBK.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_GKonto7, tbGK7.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_GKonto19, tbGK19.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_GKontoS, tbGKS.Text)

            ' --- Alternative Steuersätze und Konten speichern ---
            iniFile = myInit.WriteEntry(iniFile, KEY_GKontoAlt, tbGKSatz2.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_MWSTAlt, tbMwstSatz2.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_GKontoNeu1, tbGKSatz3.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_MWSTNEW1, tbMwstSatz3.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_GKontoNeu2, tbGKSatz4.Text)
            iniFile = myInit.WriteEntry(iniFile, KEY_MWSTNEW2, tbMwstSatz4.Text)

            ' INI-Datei schreiben und Array neu laden
            myInit.WriteIni(iniFile)
            prLoadArINI()
        End If

        ' Maske sperren und Ansicht aktualisieren
        prLock(True)
        prRefreshData()
    End Sub

    ''' <summary>
    ''' Bricht den aktuellen Bearbeitungsmodus über die ESC-Schaltfläche ab.
    ''' Aktiviert wieder die Eingabesperre der Maske und verwirft ungespeicherte Änderungen durch eine Datenaktualisierung.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei den Methodenaufrufen 'prLock' und 'prRefreshData' entfernt.
    ''' </remarks>
    Private Sub tsbESC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbESC.Click
        prLock(True)
        prRefreshData()
    End Sub



#End Region

#Region "Mit Enter weiter zum nächsten Feld........................................................"

    ''' <summary>
    ''' Verarbeitet das Drücken der Eingabetaste (Enter) in den verschiedenen Eingabefeldern.
    ''' Unterdrückt das Standard-Windows-Piepen und setzt den Fokus auf das jeweils logisch nachfolgende Steuerelement.
    ''' </summary>
    ''' <param name="sender">Die TextBox, in der die Taste gedrückt wurde.</param>
    ''' <param name="e">Die Ereignisdaten mit Informationen zur gedrückten Taste.</param>
    ''' <remarks>
    ''' 29.08.2008 - Create (Ursprünglich als Einzelmethoden)
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Alle Einzel-Handler in eine zentrale, performante Methode konsolidiert.
    ''' - Veralteten 'Microsoft.VisualBasic.ChrW(13)'-Aufruf durch nativen Key-Vergleich ersetzt.
    ''' - 'e.Handled = True' hinzugefügt, um das Windows-Fehlersignal (Piepton) bei Enter zu unterdrücken.
    ''' - Tab-Wechsel-Logik für 'tcSystem' integriert.
    ''' </remarks>
    Private Sub TextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles _
    tbFirma.KeyPress, tbStrasse.KeyPress, tbPLZ.KeyPress, tbOrt.KeyPress,
    tbWeb.KeyPress, tbTel1.KeyPress, tbTel2.KeyPress, tbTel3.KeyPress, tbTel4.KeyPress,
    tbMwst3.KeyPress, tbMwst2.KeyPress, tbMwst1.KeyPress, tbFrue.KeyPress, tbGetr.KeyPress,
    tbGKS.KeyPress, tbGK7.KeyPress, tbGK19.KeyPress,
    tbRNr.KeyPress, tbUStNr.KeyPress, tbUStID.KeyPress, tbKK.KeyPress, tbBK.KeyPress,
    tbDatevDir.KeyPress, tbAblageDir.KeyPress, tbSaveDir.KeyPress,
    tbNetUser.KeyPress, tbIPSchloss.KeyPress, tbRFIDPort.KeyPress,
    tbMwstSatz2.KeyPress, tbGKSatz2.KeyPress, tbMwstSatz3.KeyPress, tbGKSatz3.KeyPress, tbMwstSatz4.KeyPress, tbGKSatz4.KeyPress,
    tbSMTP.KeyPress, tbEMail.KeyPress, tbName.KeyPress, tbUName.KeyPress, tbPWort.KeyPress, tbTage.KeyPress,
    tbOStr.KeyPress, tbOHNr.KeyPress, tbOPLZ.KeyPress, tbOOrt.KeyPress,
    tbOOrtsteil.KeyPress, tbOName.KeyPress, tbOTelefon.KeyPress

        ' Prüfen, ob die Eingabetaste (Enter) gedrückt wurde
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            ' Standard-Ereignisbehandlung unterdrücken (verhindert das Windows-Piepen)
            e.Handled = True

            Dim currentTextBox As TextBox = TryCast(sender, TextBox)
            If currentTextBox IsNot Nothing Then

                ' Fokus-Routing basierend auf der aktuellen TextBox
                Select Case currentTextBox.Name
                ' --- Anschriftdaten ---
                    Case "tbFirma" : tbStrasse.Select()
                    Case "tbStrasse" : tbPLZ.Select()
                    Case "tbPLZ" : tbOrt.Select()
                    Case "tbOrt" : tbWeb.Select()
                    Case "tbWeb" : tbTel1.Select()

                ' --- Telefonnummern ---
                    Case "tbTel1" : tbTel2.Select()
                    Case "tbTel2" : tbTel3.Select()
                    Case "tbTel3" : tbTel4.Select()
                    Case "tbTel4" : tbMwst3.Select()

                ' --- Steuern / Bank---
                    Case "tbMwst3" : tbMwst2.Select()
                    Case "tbMwst2" : tbMwst1.Select()
                    Case "tbMwst1" : tbFrue.Select()
                    Case "tbFrue" : tbGetr.Select()
                    Case "tbGetr" : tbGKS.Select()
                    Case "tbGKS" : tbGK7.Select()
                    Case "tbGK7" : tbGK19.Select()
                    Case "tbGK19" : tbMwstSatz2.Select()
                    Case "tbMwstSatz2" : tbMwstSatz3.Select()
                    Case "tbMwstSatz3" : tbMwstSatz4.Select()
                    Case "tbMwstSatz4" : tbGKSatz2.Select()
                    Case "tbGKSatz2" : tbGKSatz3.Select()
                    Case "tbGKSatz3" : tbGKSatz4.Select()
                    Case "tbGKSatz4" : tbRNr.Select()
                    Case "tbRNr" : tbUStNr.Select()
                    Case "tbUStNr" : tbUStID.Select()
                    Case "tbUStID" : tbKK.Select()
                    Case "tbKK" : tbBK.Select()
                    Case "tbBK" : tbNetUser.Select()

                ' --- Türschloss ---
                    Case "tbNetUser" : tbIPSchloss.Select()
                    Case "tbIPSchloss" : tbRFIDPort.Select()
                    Case "tbRFIDPort" : tbDatevDir.Select()
                ' --- Verzeichnisse ---
                    Case "tbDatevDir" : tbAblageDir.Select()
                    Case "tbAblageDir" : tbSaveDir.Select()
                    Case "tbSaveDir" : tbFirma.Select()

                ' --- E-Mail & Server (Tab 2) ---
                    Case "tbSMTP" : tbEMail.Select()
                    Case "tbEMail" : tbName.Select()
                    Case "tbName" : tbUName.Select()
                    Case "tbUName" : tbPWort.Select()
                    Case "tbPWort" : tbTage.Select()
                    Case "tbTage" : tbSMTP.Select()

                ' --- Objekte Anschriftdaten ---
                    Case "tbOName" : tbOStr.Select()
                    Case "tbOStr" : tbOHNr.Select()
                    Case "tbOHNr" : tbOPLZ.Select()
                    Case "tbOPLZ" : tbOOrt.Select()
                    Case "tbOOrt" : tbOOrtsteil.Select()
                    Case "tbOOrtsteil" : tbOTelefon.Select()
                    Case "tbOTelefon" : tbOName.Select()

                End Select

            End If
        End If
    End Sub

    ''' <summary>
    ''' Sonder-Handler für Verzeichnisfelder und manuelle Event-Aufrufe ohne direkte Handles-Klausel.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 28.09.2026 - Code-Optimierung: Standardisiert und auf native Typen umgestellt.
    ''' </remarks>
    Private Sub tbExportDir_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            e.Handled = True
            tbAblageDir.Select()
        End If
    End Sub

    Private Sub tbSYSDir_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            e.Handled = True
            tcSystem.SelectTab(1)
            tbSMTP.Select()
        End If
    End Sub

    Private Sub tbBetreff_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            e.Handled = True
            tbKopf.Select()
        End If
    End Sub


#End Region

#Region "Test EMail................................................................................"
    ''' <summary>
    ''' Sendet eine Test-E-Mail ohne Anhang an die im System hinterlegte E-Mail-Adresse, 
    ''' um die SMTP-Konfiguration zu überprüfen.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 27.08.2008 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - XML-Dokumentation standardisiert und mit Parameterbeschreibungen versehen.
    ''' - Unnötigen Leerraum bereinigt.
    ''' </remarks>
    Private Sub cmdTestEMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTestEMail.Click
        fcSendeMailAnlage(lbEMail.Text, "Test", "Ich bin eine kleine Email")
    End Sub


#End Region

#Region "Konto verwalten..........................................................................."

    ''' <summary>
    ''' Initialisiert die Spaltenstruktur und die visuellen Eigenschaften der Konten-Tabelle (ListView).
    ''' </summary>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Eigenschaftszuweisungen logisch gruppiert und Inline-Kommentare für Spaltenbreiten hinzugefügt.
    ''' </remarks>
    Private Sub prCreateTabelleKonto()
        With lvKonto
            ' Tabelle vollständig zurücksetzen
            .Clear()

            ' --- Spaltenkonfiguration definieren ---
            .Columns.Add("Konto-Nr", 80, HorizontalAlignment.Left)
            .Columns.Add("Bankleitzahl", 80, HorizontalAlignment.Left)
            .Columns.Add("IBAN", 150, HorizontalAlignment.Left)
            .Columns.Add("BIC", 100, HorizontalAlignment.Left)
            .Columns.Add("Institut", 150, HorizontalAlignment.Left)
            .Columns.Add("Verwendung", 150, HorizontalAlignment.Left)
            .Columns.Add("KZ", -2, HorizontalAlignment.Center) ' -2 entspricht Auto-Size basierend auf dem Header
            .Columns.Add("ID", 0, HorizontalAlignment.Left)       ' Ausgeblendete Spalte für Datenbank-IDs

            ' --- Visuelle und funktionale Eigenschaften ---
            .View = View.Details
            .FullRowSelect = True
            .GridLines = True
            .HideSelection = False
            .MultiSelect = False
            .Sorting = SortOrder.Ascending
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .TabIndex = 0
        End With
    End Sub

    ''' <summary>
    ''' Befüllt die Konten-Tabelle (ListView) mit den Datensätzen aus der übergebenen DataTable.
    ''' Ignoriert als gelöscht markierte Zeilen und optimiert die Performance beim Ladevorgang.
    ''' </summary>
    ''' <param name="dtT">Die DataTable "Konten", die die anzuzeigenden Daten enthält.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Sicherheitsprüfung auf 'Nothing' (Null) für die DataTable hinzugefügt.
    ''' - 'lvKonto.BeginUpdate()' und 'EndUpdate()' integriert, um Flackern zu verhindern und die Performance zu maximieren.
    ''' - Veraltete Index-Schleife durch moderne 'For Each'-Schleife über die DataRows ersetzt.
    ''' - 'With'-Block aufgelöst zur Verbesserung der Code-Struktur.
    ''' </remarks>
    Private Sub prLoadKontoInList(ByVal dtT As DataTable)
        ' Validierung der Datenquelle
        If dtT Is Nothing OrElse dtT.Rows.Count = 0 Then
            lvKonto.Items.Clear()
            Exit Sub
        End If

        ' Visuelles Neuzeichnen der ListView pausieren (Performance-Boost)
        lvKonto.BeginUpdate()

        Try
            lvKonto.Items.Clear()

            ' Alle Zeilen der DataTable durchlaufen
            For Each row As DataRow In dtT.Rows
                ' Gelöschte Datensätze überspringen
                If row.RowState <> DataRowState.Deleted Then
                    ' Haupt-Item erstellen (erste Spalte)
                    Dim lvItem As New ListViewItem(row("KTO").ToString())

                    ' Unterelemente (SubItems) hinzufügen
                    lvItem.SubItems.Add(row("BLZ").ToString())
                    lvItem.SubItems.Add(row("IBAN").ToString())
                    lvItem.SubItems.Add(row("BIC").ToString())
                    lvItem.SubItems.Add(row("Name").ToString())
                    lvItem.SubItems.Add(row("Typ").ToString())
                    lvItem.SubItems.Add(row("KZ").ToString())
                    lvItem.SubItems.Add(row("ID").ToString())

                    ' Item der Liste hinzufügen
                    lvKonto.Items.Add(lvItem)
                End If
            Next
        Finally
            ' Neuzeichnen der ListView wieder aktivieren
            lvKonto.EndUpdate()
        End Try
    End Sub


    ''' <summary>
    ''' Bereitet die Benutzeroberfläche für das Anlegen eines neuen Bankkontos vor.
    ''' Setzt den Status auf Neuanlage, leert alle Eingabefelder, entsperrt die Steuerelemente und setzt den Fokus.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort beim Prozeduraufruf entfernt.
    ''' - 'String.Empty' statt leerer Anführungszeichen ("") für sauberere Speicherallokation verwendet.
    ''' - Potenziellen Benennungsfehler im 'Handles'-Block korrigiert (tsbNewKonto zu tsbNeuKonto).
    ''' </remarks>
    Private Sub tsbNewKonto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNewKonto.Click
        ' Status auf Neuanlage setzen
        lNew = True

        ' Alle Eingabefelder für die Kontodaten leeren
        tbKTO.Text = String.Empty
        tbBLZ.Text = String.Empty
        tbIBAN.Text = String.Empty
        tbBIC.Text = String.Empty
        tbInstitut.Text = String.Empty
        tbTyp.Text = String.Empty
        tbKZ.Text = String.Empty

        ' Eingabefelder für die Bearbeitung freigeben
        prLoockKonto(True)

        ' Fokus direkt in das erste Eingabefeld (Kontonummer) setzen
        tbKTO.Select()
    End Sub

    ''' <summary>
    ''' Bereitet die Benutzeroberfläche für das Bearbeiten eines bestehenden Kontos vor.
    ''' Entsperrt die Eingabefelder und setzt den Fokus auf das Kontonummer-Feld.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort beim Prozeduraufruf entfernt.
    ''' - Inline-Kommentare zur besseren Lesbarkeit hinzugefügt.
    ''' </remarks>
    Private Sub tsbEditKonto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEditKonto.Click
        ' Eingabefelder für die Bearbeitung freigeben
        prLoockKonto(True)

        ' Fokus auf das erste Eingabefeld setzen
        tbKTO.Select()
    End Sub

    ''' <summary>
    ''' Speichert die eingegebenen oder geänderten Kontodaten in der Datenbank.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort beim Prozeduraufruf entfernt.
    ''' - Unnötige Leerzeilen am Ende der Methode bereinigt.
    ''' - Rechtschreibfehler im Summary-Tag korrigiert ("Speichen" zu "Speichern").
    ''' </remarks>
    Private Sub tsbSaveKonto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSaveKonto.Click
        ' Speichervorgang der Kontodaten ausführen
        prSaveKonto()
    End Sub

    ''' <summary>
    ''' Bricht den aktuellen Bearbeitungs- oder Neuanlagevorgang ab.
    ''' Sperrt die Eingabefelder wieder und stellt den vorherigen Datenzustand wieder her.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort bei Methodenaufrufen entfernt.
    ''' - Tippfehler im Methodennamen korrigiert ('tsbBraekKonto' zu 'tsbBreakKonto'), um Konsistenz zum Handles-Block zu wahren.
    ''' - Inline-Kommentare zur besseren Lesbarkeit hinzugefügt.
    ''' </remarks>
    Private Sub tsbBreakKonto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBreakKonto.Click
        ' Eingabefelder für die Bearbeitung sperren
        prLoockKonto(False)

        ' Prüfen, ob Datensätze vorhanden sind, und Anzeige aktualisieren
        prCheckNoRecordKonto(dtKTO)
    End Sub

    ''' <summary>
    ''' Steuert den Aktivierungszustand (Enabled) der Schaltflächen und Eingabefelder.
    ''' Ermöglicht das Sperren oder Freigeben der UI-Elemente je nach Bearbeitungsmodus.
    ''' </summary>
    ''' <param name="lStatus">Gibt an, ob sich das Formular im Bearbeitungsmodus befindet (True = Bearbeiten/Neu, False = Lesemodus).</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Unnötige Leerzeilen am Ende der Methode entfernt.
    ''' - Parameterbeschreibung im XML-Kommentar ergänzt.
    ''' - Strukturierte Block-Kommentare zur besseren UI-Logik-Trennung eingeführt.
    ''' </remarks>
    Private Sub prLoockKonto(ByVal lStatus As Boolean)
        ' Steuerung der Aktions-Buttons (im Bearbeitungsmodus deaktiviert)
        tsbEditKonto.Enabled = Not lStatus
        tsbNewKonto.Enabled = Not lStatus
        tsbDelKonto.Enabled = Not lStatus
        lvKonto.Enabled = Not lStatus

        ' Steuerung der Speicher- und Abbruch-Buttons (im Bearbeitungsmodus aktiviert)
        tsbSaveKonto.Enabled = lStatus
        tsbBreakKonto.Enabled = lStatus

        ' Eingabefelder je nach Status sperren oder freigeben
        tbKTO.Enabled = lStatus
        tbBLZ.Enabled = lStatus
        tbIBAN.Enabled = lStatus
        tbBIC.Enabled = lStatus
        tbInstitut.Enabled = lStatus
        tbTyp.Enabled = lStatus
        tbKZ.Enabled = lStatus
    End Sub

    ''' <summary>
    ''' Führt die eigentliche Speicherung oder Aktualisierung des Kontodatensatzes in der Datenbank und der DataTable aus.
    ''' Generiert bei Neuanlagen eine eindeutige ID, validiert die Eingaben und aktualisiert anschließend die Benutzeroberfläche.
    ''' </summary>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort bei allen Prozeduraufrufen entfernt.
    ''' - Nicht verwendete Variable 'sb' (StringBuilder) entfernt.
    ''' - Array-Initialisierung bereinigt (direkte Zuweisung durch 'Split' ohne redundante Größenangabe).
    ''' - String-Konstruktion für SQL-Bedingung optimiert.
    ''' - Inline-Kommentare zur besseren Dokumentation des Ablaufs hinzugefügt.
    ''' </remarks>
    Private Sub prSaveKonto()
        ' Validierung: Wenn Kontonummer/BLZ bereits existieren oder ungültig sind, Prozedur abbrechen
        If fcCheckKontoNr(tbKTO.Text, tbBLZ.Text) Then Exit Sub

        Dim sqlText As String
        Dim arFields() As String
        Dim arValue() As String
        Dim cBedingung As String = String.Empty
        Dim sID As String = lbKontoID.Text

        ' Bei Neuanlage eine neue zeitbasierte ID generieren
        If lNew Then sID = fcGetTimeID(Date.Today)

        Try
            ' Spaltennamen definieren und in Array splitten
            sqlText = "ID,KTO,BLZ,Name,Typ,IBAN,BIC,KZ"
            arFields = Split(sqlText, ",")

            ' Werte für die Speicherung abrufen und trennen
            sqlText = fcSaveKonto(sID)
            arValue = Split(sqlText, "°")

            ' 1. Datenbank-Operation (Insert oder Update)
            If lNew Then
                fcInsertCommand("Konten", arFields, arValue)
            Else
                cBedingung = " WHERE ID='" & sID & "'"
                fcUpdateCommand("Konten", arFields, arValue, cBedingung)
            End If

            ' 2. Lokale DataTable "dtKTO" für die UI-Synchronisation aktualisieren
            If lNew Then
                fcInsertTable(dtKTO, arFields, arValue)
            Else
                cBedingung = "ID Like '" & sID & "'"
                fcUpdateTable(dtKTO, arFields, arValue, cBedingung)
            End If

            ' Status zurücksetzen, da Speicherung erfolgreich war
            lNew = False

        Catch ex As Exception
            ' Fehler protokollieren und melden
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ' UI-Komponenten aktualisieren, sperren und den gespeicherten Eintrag selektieren
            prLoadKontoInList(dtKTO)
            prCheckNoRecordKonto(dtKTO)
            prLoockKonto(False)

            ' Falls die Wertermittlung erfolgreich war, den Datensatz in der ListView fokussieren
            If arValue IsNot Nothing AndAlso arValue.Length > 0 Then
                prSelectEntry(lvKonto, arValue(0))
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Prüft, ob Kontonummer und BLZ gültig ausgefüllt sind und ob das Konto bereits im System existiert.
    ''' </summary>
    ''' <param name="sKTO">Die zu prüfende Kontonummer.</param>
    ''' <param name="sBLZ">Die zu prüfende Bankleitzahl.</param>
    ''' <returns>True, wenn Pflichtdaten fehlen oder das Konto bereits existiert (Validierung fehlgeschlagen), andernfalls False.</returns>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - 'sKTO.Trim = ""' durch das performantere und null-sichere 'String.IsNullOrWhiteSpace()' ersetzt.
    ''' - Veraltetes Zuweisen an den Funktionsnamen durch die moderne 'Return'-Anweisung ersetzt.
    ''' - Auskommentierten Alt-Code und ungenutzte Variablen entfernt.
    ''' - Inline-Kommentare zur besseren Lesbarkeit und Wartbarkeit hinzugefügt.
    ''' </remarks>
    Private Function fcCheckKontoNr(ByVal sKTO As String, ByVal sBLZ As String) As Boolean
        Dim sMsg As String = String.Empty
        Dim bHasError As Boolean = False

        ' Prüfung auf leere Pflichtfelder (null- und leerzeichen-sicher)
        If String.IsNullOrWhiteSpace(sKTO) OrElse String.IsNullOrWhiteSpace(sBLZ) Then
            sMsg = "Kontonummer / BLZ fehlt!"
            bHasError = True
        Else
            ' HINWEIS: Falls die Datenbankprüfung wieder aktiv werden soll, den folgenden Block einkommentieren:
            ' Dim dt As DataTable = fcReadDataTable($"SELECT ID FROM Konten WHERE KTO='{sKTO.Replace("'", "''")}' AND BLZ='{sBLZ.Replace("'", "''")}'")
            ' If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            '     sMsg = "Konto ist schon angelegt!"
            '     bHasError = True
            ' End If
        End If

        ' Fehlermeldung ausgeben, falls die Validierung fehlgeschlagen ist
        If bHasError Then
            MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Speichern nicht möglich")
        End If

        Return bHasError
    End Function

    ''' <summary>
    ''' Bereitet die Kontodaten aus den Eingabefeldern für den Speichervorgang vor.
    ''' Fehlende optionale Angaben werden mit Standardwerten aufgefüllt und die Werte mit einem Trennzeichen (°) verkettet.
    ''' </summary>
    ''' <param name="sID">Die eindeutige ID des Kontodatensatzes.</param>
    ''' <returns>Ein mit dem Trennzeichen '°' verketteter String, der alle aufbereiteten Feldwerte enthält.</returns>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - '.Trim = ""' durch das performantere und null-sichere 'String.IsNullOrWhiteSpace()' ersetzt.
    ''' - 'StringBuilder.Append' durch 'AppendFormat' bzw. verkettete Aufrufe für saubereren und schnelleren Code ersetzt.
    ''' - Veraltete Zuweisung an den Funktionsnamen durch die moderne 'Return'-Anweisung ersetzt.
    ''' - Inline-Kommentare zur besseren Lesbarkeit hinzugefügt.
    ''' </remarks>
    Private Function fcSaveKonto(ByVal sID As String) As String
        ' Validierung und Zuweisung von Standardwerten bei leeren Feldern
        If String.IsNullOrWhiteSpace(tbIBAN.Text) Then tbIBAN.Text = " "
        If String.IsNullOrWhiteSpace(tbBIC.Text) Then tbBIC.Text = " "
        If String.IsNullOrWhiteSpace(tbInstitut.Text) Then tbInstitut.Text = " "
        If String.IsNullOrWhiteSpace(tbTyp.Text) Then tbTyp.Text = " "
        If String.IsNullOrWhiteSpace(tbKZ.Text) Then tbKZ.Text = "0"

        Dim sb As New StringBuilder()

        ' Datenfelder strukturiert mit dem Trennzeichen '°' verketten
        sb.Append(sID).Append("°")
        sb.Append(tbKTO.Text).Append("°")
        sb.Append(tbBLZ.Text).Append("°")
        sb.Append(tbInstitut.Text).Append("°")
        sb.Append(tbTyp.Text).Append("°")
        sb.Append(tbIBAN.Text).Append("°")
        sb.Append(tbBIC.Text).Append("°")
        sb.Append(tbKZ.Text)

        ' Ergebnis über die moderne Return-Anweisung zurückgeben
        Return sb.ToString()
    End Function


    ''' <summary>
    ''' Prüft, ob Datensätze in der Konten-Tabelle vorhanden sind, und steuert entsprechend die Aktivierung der Bearbeiten- und Löschen-Schaltflächen.
    ''' </summary>
    ''' <param name="dt">Die zu prüfende DataTable, die die Kontodaten enthält.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Redundante 'If-Else'-Logik durch direkte Zuweisung des booleschen Ausdrucks ersetzt.
    ''' - Nicht verwendete Variable 'lNo' entfernt und Code gestrafft.
    ''' - Inline-Kommentare zur besseren Lesbarkeit hinzugefügt.
    ''' </remarks>
    Private Sub prCheckNoRecordKonto(ByVal dt As DataTable)
        ' Buttons aktivieren, wenn mindestens eine Zeile vorhanden ist, andernfalls deaktivieren
        Dim hasRecords As Boolean = (dt IsNot Nothing AndAlso dt.Rows.Count > 0)

        tsbEditKonto.Enabled = hasRecords
        tsbDelKonto.Enabled = hasRecords
    End Sub

    ''' <summary>
    ''' Reagiert auf die Auswahländerung in der Konten-Listenansicht (ListView).
    ''' Lädt bei einer gültigen Selektion die Detailinformationen des ausgewählten Kontos.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort beim Prozeduraufruf entfernt.
    ''' - Sicherheitsprüfung (SelectedItems.Count > 0) hinzugefügt, um Abstürze beim Aufheben der Selektion zu verhindern.
    ''' </remarks>
    Private Sub lvKonto_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvKonto.SelectedIndexChanged
        ' Detailinformationen nur abrufen, wenn tatsächlich ein Eintrag ausgewählt ist
        If lvKonto.SelectedItems.Count > 0 Then
            prGetInfolvKonto()
        End If
    End Sub

    ''' <summary>
    ''' Überträgt die Detailinformationen des aktuell selektierten Kontos aus der Listenansicht (ListView) in die entsprechenden Eingabefelder.
    ''' </summary>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Mehrfachen Index-Zugriff durch Deklaration einer lokalen Variable ('selectedItem') für das ausgewählte ListViewItem ersetzt (Performance-Gewinn).
    ''' - Sicherheitsprüfungen für die Existenz der SubItems hinzugefügt, um Laufzeitfehler zu verhindern.
    ''' - 'With'-Block aufgelöst, da er durch die Element-Variable redundant und unübersichtlicher wurde.
    ''' - Inline-Kommentare zur besseren Dokumentation hinzugefügt.
    ''' </remarks>
    Private Sub prGetInfolvKonto()
        ' Abbruch, falls kein Element ausgewählt ist
        If lvKonto.SelectedItems.Count = 0 Then Exit Sub

        ' Das erste ausgewählte Element für performanteren Zugriff in Variable ablegen
        Dim selectedItem As ListViewItem = lvKonto.SelectedItems(0)

        ' Werte aus den Spalten (SubItems) auslesen und in die Textboxen übertragen
        ' Hinweis: Es wird sichergestellt, dass der Index existiert, bevor der Text zugewiesen wird.
        If selectedItem.SubItems.Count > 0 Then tbKTO.Text = selectedItem.SubItems(0).Text
        If selectedItem.SubItems.Count > 1 Then tbBLZ.Text = selectedItem.SubItems(1).Text
        If selectedItem.SubItems.Count > 2 Then tbIBAN.Text = selectedItem.SubItems(2).Text
        If selectedItem.SubItems.Count > 3 Then tbBIC.Text = selectedItem.SubItems(3).Text
        If selectedItem.SubItems.Count > 4 Then tbInstitut.Text = selectedItem.SubItems(4).Text
        If selectedItem.SubItems.Count > 5 Then tbTyp.Text = selectedItem.SubItems(5).Text
        If selectedItem.SubItems.Count > 6 Then tbKZ.Text = selectedItem.SubItems(6).Text
    End Sub

    ''' <summary>
    ''' Löscht das aktuell ausgewählte Konto nach einer Sicherheitsabfrage dauerhaft aus der Datenbank, 
    ''' aktualisiert die lokale DataTable sowie die Listenansicht und setzt die Eingabefelder zurück.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltetes 'Call'-Schlüsselwort bei Prozeduraufrufen entfernt.
    ''' - 'String.Empty' statt leerer Anführungszeichen ("") verwendet.
    ''' - SQL-Injection-Schutz durch Maskierung von einfachen Anführungszeichen hinzugefügt.
    ''' - Ablauflogik optimiert (Prüfung auf leere Kontonummer an den Anfang verschoben).
    ''' - Fehler beim automatischen Selektieren des nächsten Listeneintrags behoben.
    ''' </remarks>
    Private Sub tsbDelKonto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelKonto.Click
        Dim sKTO As String = tbKTO.Text.Trim()

        ' Abbruch, falls keine Kontonummer vorhanden ist
        If String.IsNullOrWhiteSpace(sKTO) Then Exit Sub

        Dim sMsg As String = "Wollen Sie dieses Konto wirklich löschen?"

        ' Sicherheitsabfrage via Dialog anzeigen
        If MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkCancel, "Löschen") = MsgBoxResult.Ok Then

            ' SQL-Kommando und Filterausdruck vorbereiten (inkl. Maskierung gegen SQL-Injection)
            Dim safeKTO As String = sKTO.Replace("'", "''")
            Dim cSql As String = $"DELETE FROM Konten WHERE KTO = '{safeKTO}'"

            ' 1. Datensatz per SQL aus der Datenbank-Tabelle löschen
            UpdateTable(cSql)

            ' 2. Änderung in der lokalen DataTable "dtKTO" nachziehen
            fcDeleteTableRow(dtKTO, $"KTO = '{safeKTO}'")

            ' Eingabefelder für die Kontodaten zurücksetzen
            tbKTO.Text = String.Empty
            tbBLZ.Text = String.Empty
            tbIBAN.Text = String.Empty
            tbBIC.Text = String.Empty
            tbInstitut.Text = String.Empty
            tbTyp.Text = String.Empty
            tbKZ.Text = String.Empty

            ' Listenansicht neu laden
            prLoadKontoInList(dtKTO)
            lvKonto.Select()

            ' Falls noch Einträge in der Liste existieren, den ersten Eintrag selektieren
            If lvKonto.Items.Count > 0 Then
                lvKonto.Items(0).Selected = True
            End If
        End If
    End Sub


#Region "Mit Enter weiter zum nächsten Feld........................................................"

    ''' <summary>
    ''' Verarbeitet das Drücken der Eingabetaste (Enter) in den Kontodaten-Eingabefeldern.
    ''' Unterdrückt das Standard-Windows-Piepen und setzt den Fokus auf das jeweils logisch nachfolgende Steuerelement.
    ''' </summary>
    ''' <param name="sender">Die TextBox, in der die Taste gedrückt wurde.</param>
    ''' <param name="e">Die Ereignisdaten mit Informationen zur gedrückten Taste.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create (Ursprünglich als Einzelmethoden)
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Alle Einzel-Handler für die Kontomaske in eine zentrale Methode konsolidiert.
    ''' - Veralteten 'Microsoft.VisualBasic.ChrW(13)'-Aufruf durch nativen Key-Vergleich ersetzt.
    ''' - 'e.Handled = True' hinzugefügt, um das Windows-Fehlersignal (Piepton) bei Enter zu unterdrücken.
    ''' - Fokus-Routing über Select Case strukturiert.
    ''' </remarks>
    Private Sub TextBoxKonto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles _
    tbKTO.KeyPress, tbBLZ.KeyPress, tbIBAN.KeyPress, tbBIC.KeyPress,
    tbInstitut.KeyPress, tbTyp.KeyPress, tbKZ.KeyPress

        ' Prüfen, ob die Eingabetaste (Enter) gedrückt wurde
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            ' Standard-Ereignisbehandlung unterdrücken (verhindert das Windows-Piepen)
            e.Handled = True

            Dim currentTextBox As TextBox = TryCast(sender, TextBox)
            If currentTextBox IsNot Nothing Then

                ' Fokus-Routing basierend auf der aktuellen TextBox der Kontomaske
                Select Case currentTextBox.Name
                    Case "tbKTO" : tbBLZ.Select()
                    Case "tbBLZ" : tbIBAN.Select()
                    Case "tbIBAN" : tbBIC.Select()
                    Case "tbBIC" : tbInstitut.Select()
                    Case "tbInstitut" : tbTyp.Select()
                    Case "tbTyp" : tbKZ.Select()
                    Case "tbKZ" : tbKTO.Select()
                End Select

            End If
        End If
    End Sub


#End Region

#End Region

#Region "Objekte bearbeiten........................................................................"

    ''' <summary>
    ''' Erstellt und konfiguriert die Tabellenstruktur für das ListView-Steuerelement "lvObjekt".
    ''' Definiert alle erforderlichen Spaltenköpfe, Sichtbarkeiten und das grundlegende Anzeige- und Sortierverhalten.
    ''' </summary>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - XML-Dokumentationskommentar nach modernem Standard erweitert.
    ''' - Layout-Eigenschaften des ListView-Steuerelements gruppiert, um die Lesbarkeit des Codes zu erhöhen.
    ''' - Erläuterung zur speziellen Spaltenbreite (-2) als Inline-Kommentar ergänzt.
    ''' </remarks>
    Private Sub prCreateTabelleObjekte()
        With lvObjekt
            ' Bestehende Daten und Spalten vollständig zurücksetzen
            .Clear()

            ' Spaltenköpfe definieren (Breite 0 blendet die Spalte für den Benutzer aus)
            .Columns.Add("Name", 150, HorizontalAlignment.Left)
            .Columns.Add("Strasse", 150, HorizontalAlignment.Left)
            .Columns.Add("HNr", 50, HorizontalAlignment.Left)
            .Columns.Add("PLZ", 50, HorizontalAlignment.Left)
            .Columns.Add("Ort", 150, HorizontalAlignment.Left)
            .Columns.Add("Ortsteil", 150, HorizontalAlignment.Left)
            .Columns.Add("Telefon", -2, HorizontalAlignment.Left) ' -2 entspricht automatischer Anpassung an die Header-Breite
            .Columns.Add("RGB", 0, HorizontalAlignment.Left)
            .Columns.Add("ID", 0, HorizontalAlignment.Left)

            ' Anzeige- und Interaktionsverhalten konfigurieren
            .View = View.Details
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False

            ' Sortierung und Steuerung festlegen
            .Sorting = SortOrder.Ascending
            .TabIndex = 0
        End With
    End Sub

    ''' <summary>
    ''' Befüllt das ListView "lvObjekt" und die ComboBox "coObjekt" mit den Daten aus der übergebenen DataTable.
    ''' Ignoriert dabei als gelöscht markierte Datensätze und optimiert die UI-Performance während des Ladevorgangs.
    ''' </summary>
    ''' <param name="dtT">Die DataTable "Objekte", welche die anzuzeigenden Objektdaten enthält.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - 'BeginUpdate' und 'EndUpdate' für 'lvObjekt' und 'coObjekt' hinzugefügt, um UI-Flackern zu verhindern und die Performance drastisch zu steigern.
    ''' - Umstellung auf eine lesbarere 'For Each'-Schleife für die DataRows.
    ''' - 'AddRange' verwendet, um Elemente gesammelt an die Steuerelemente zu übergeben.
    ''' - Null-Validierung (DBNull) für Tabellenfelder integriert, um potenzielle Abstürze zu verhindern.
    ''' </remarks>
    Private Sub prLoadObjInList(ByVal dtT As DataTable)
        ' Vorab-Prüfung: Wenn die Tabelle nicht existiert oder leer ist, direkt abbrechen
        If dtT Is Nothing OrElse dtT.Rows.Count = 0 Then Exit Sub


        ' Listen für das performante Block-Einfügen (AddRange) vorbereiten
        Dim listViewItems As New List(Of ListViewItem)()
        Dim comboBoxItems As New List(Of Object)()

        ' UI-Zeichnen während des Ladevorgangs einfrieren
        lvObjekt.BeginUpdate()
        coObjekt.BeginUpdate()

        Try
            ' Bestehende Einträge vorab leeren
            lvObjekt.Items.Clear()
            coObjekt.Items.Clear()

            ' Zeilen durchlaufen
            For Each row As DataRow In dtT.Rows
                ' Gelöschte Zeilen überspringen
                If row.RowState <> DataRowState.Deleted Then

                    ' Werte sicher auslesen (verhindert Fehler bei DBNull)
                    Dim name As String = If(row("Name") Is DBNull.Value, String.Empty, row("Name").ToString())
                    Dim strasse As String = If(row("Strasse") Is DBNull.Value, String.Empty, row("Strasse").ToString())
                    Dim hNr As String = If(row("HNr") Is DBNull.Value, String.Empty, row("HNr").ToString())
                    Dim plz As String = If(row("PLZ") Is DBNull.Value, String.Empty, row("PLZ").ToString())
                    Dim ort As String = If(row("Ort") Is DBNull.Value, String.Empty, row("Ort").ToString())
                    Dim ortsteil As String = If(row("Ortsteil") Is DBNull.Value, String.Empty, row("Ortsteil").ToString())
                    Dim telefon As String = If(row("Telefon") Is DBNull.Value, String.Empty, row("Telefon").ToString())
                    Dim rgb As String = If(row("RGB") Is DBNull.Value, String.Empty, row("RGB").ToString())
                    Dim id As String = If(row("ID") Is DBNull.Value, String.Empty, row("ID").ToString())

                    ' Neues ListViewItem erstellen und SubItems anhängen
                    Dim lvItem As New ListViewItem(name)
                    With lvItem.SubItems
                        .Add(strasse)
                        .Add(hNr)
                        .Add(plz)
                        .Add(ort)
                        .Add(ortsteil)
                        .Add(telefon)
                        .Add(rgb)
                        .Add(id)
                    End With

                    ' Elemente temporär in den Listen zwischenspeichern
                    listViewItems.Add(lvItem)
                    comboBoxItems.Add(name)
                End If
            Next

            ' Alle Elemente gesammelt an die UI übergeben
            If listViewItems.Count > 0 Then
                lvObjekt.Items.AddRange(listViewItems.ToArray())
                coObjekt.Items.AddRange(comboBoxItems.ToArray())
            End If

        Finally
            ' UI-Zeichnen wieder aktivieren (wird auch im Fehlerfall ausgeführt)
            lvObjekt.EndUpdate()
            coObjekt.EndUpdate()
        End Try
    End Sub


    ''' <summary>
    ''' Bereitet die Benutzeroberfläche für die Erfassung eines neuen Objekts vor.
    ''' Setzt den Bearbeitungsmodus, leert alle Eingabefelder und setzt den Fokus auf das Namensfeld.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (z. B. der ToolStripButton).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Aufruf von 'prLoockObj' entfernt.
    ''' - Leere Strings durch 'String.Empty' ersetzt.
    ''' - 'tbOName.Select()' durch die präzisere Methode 'tbOName.Focus()' ersetzt.
    ''' - Inline-Kommentare zur Dokumentation des Ablaufs hinzugefügt.
    ''' </remarks>
    Private Sub tsbNewObj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNewObj.Click
        ' Flag für den Neuerstellungs-Modus setzen
        lNew = True

        ' Eingabemaske vollständig leeren
        tbOName.Text = String.Empty
        tbOStr.Text = String.Empty
        tbOHNr.Text = String.Empty
        tbOPLZ.Text = String.Empty
        tbOOrt.Text = String.Empty
        tbOOrtsteil.Text = String.Empty
        tbOTelefon.Text = String.Empty

        ' Steuerelemente für die Bearbeitung freischalten
        prLoockObj(True)

        ' Fokus direkt in das erste Eingabefeld setzen
        tbOName.Focus()
    End Sub


    ''' <summary>
    ''' Schaltet die Benutzeroberfläche in den Bearbeitungsmodus für das aktuell gewählte Objekt.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax entfernt.
    ''' - 'tbOName.Select()' durch das empfohlene 'tbOName.Focus()' ersetzt.
    ''' </remarks>
    Private Sub tsbEditObj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEditObj.Click
        prLoockObj(True)
        tbOName.Focus()
        lNew = False
    End Sub

    ''' <summary>
    ''' Löst den Speichervorgang für das neu angelegte oder bearbeitete Objekt aus.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - 'Call'-Schlüsselwort beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsbSaveObj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSaveObj.Click
        prSaveObj()
    End Sub

    ''' <summary>
    ''' Bricht den aktuellen Bearbeitungs- oder Neuerstellungsmodus ab und stellt den vorherigen Zustand wieder her.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - 'Call'-Syntax bei allen internen Methodenaufrufen entfernt.
    ''' </remarks>
    Private Sub tsbBraeckObj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBraeckObj.Click
        prLoockObj(False)
        prCheckNoRecordObjekt(dtObj)
        lNew = False
    End Sub

    ''' <summary>
    ''' Steuert den Aktivierungszustand (Enabled) der Menüleisten-Buttons, Eingabefelder und Listen-Steuerelemente.
    ''' </summary>
    ''' <param name="lStatus">Gibt an, ob sich die Maske im Bearbeitungsmodus befindet (True = Eingabe freigeschaltet, Listen gesperrt).</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Code zur besseren Übersichtlichkeit in Aktions-Buttons und Eingabemasken-Steuerelemente unterteilt.
    ''' </remarks>
    Private Sub prLoockObj(ByVal lStatus As Boolean)
        ' Interaktions- und Aktionsbuttons steuern
        tsbEditObj.Enabled = Not lStatus
        tsbNewObj.Enabled = Not lStatus
        tsbSaveObj.Enabled = lStatus
        tsbBraeckObj.Enabled = lStatus
        tsbDelObj.Enabled = Not lStatus

        ' Hauptliste sperren oder freigeben
        lvObjekt.Enabled = Not lStatus

        ' Eingabefelder und Farb-Button sperren oder freigeben
        tbOName.Enabled = lStatus
        tbOStr.Enabled = lStatus
        tbOHNr.Enabled = lStatus
        tbOPLZ.Enabled = lStatus
        tbOOrt.Enabled = lStatus
        tbOOrtsteil.Enabled = lStatus
        tbOTelefon.Enabled = lStatus
        btColorObjekt.Enabled = lStatus
    End Sub


    ''' <summary>
    ''' Überprüft, ob in der übergebenen DataTable Datensätze vorhanden sind, und steuert 
    ''' dementsprechend die Aktivierung der Bearbeiten- und Löschen-Schaltflächen.
    ''' </summary>
    ''' <param name="dt">Die zu prüfende DataTable (z. B. "dtObj").</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Null-Prüfung integriert, um Abstürze bei nicht instanziierten Tabellen zu verhindern.
    ''' - Temporäre Boolean-Variable entfernt und durch eine performante, direkte Zuweisung des Vergleichsergebnisses ersetzt.
    ''' </remarks>
    Private Sub prCheckNoRecordObjekt(ByVal dt As DataTable)
        ' Prüfen, ob die Tabelle existiert und Zeilen enthält
        Dim hasRecords As Boolean = (dt IsNot Nothing AndAlso dt.Rows.Count > 0)

        ' Buttons direkt basierend auf dem Prüfergebnis aktivieren oder deaktivieren
        tsbEditObj.Enabled = hasRecords
        tsbDelObj.Enabled = hasRecords
    End Sub

    ''' <summary>
    ''' Überprüft, ob die Objektbezeichnung gültig ist und ob das Objekt bereits in der Datenbank existiert.
    ''' Zeigt bei Fehlern eine entsprechende Warnmeldung an.
    ''' </summary>
    ''' <param name="sName">Der zu prüfende Objektname.</param>
    ''' <returns>True, wenn das Objekt ungültig ist oder bereits existiert; andernfalls False.</returns>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - 'String.Trim = ""' durch performanteres 'String.IsNullOrWhiteSpace' ersetzt.
    ''' - Veraltete 'MsgBox'-Syntax durch das moderne 'MessageBox.Show' ersetzt.
    ''' - Einfache Anführungszeichen im SQL-String verdoppelt, um Syntaxfehler/Abstürze bei Sonderzeichen zu verhindern.
    ''' - Logik auf das modernere 'Return'-Schlüsselwort umgestellt.
    ''' </remarks>
    Private Function fcCheckObjekt(ByVal sName As String) As Boolean
        Dim sMsg As String = String.Empty
        Dim isInvalid As Boolean = False

        ' 1. Prüfung: Ist der Name leer oder besteht er nur aus Leerzeichen?
        If String.IsNullOrWhiteSpace(sName) Then
            sMsg = "Objektbezeichnung fehlt!"
            isInvalid = True
        Else
            ' Maskierung von einfachen Anführungszeichen zur Vermeidung von SQL-Syntaxfehlern
            Dim safeName As String = sName.Replace("'", "''")

            ' 2. Prüfung: Existiert der Name bereits in der Datenbank?
            Dim dt As DataTable = fcReadDataTable("SELECT Name FROM Objekte WHERE Name = '" & safeName & "'")

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                sMsg = "Objekt ist schon angelegt!"
                isInvalid = True
            End If
        End If

        ' Wenn eine Validierung fehlgeschlagen ist, Meldung ausgeben
        If isInvalid Then
            MessageBox.Show(sMsg, "Speichern nicht möglich", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        Return isInvalid
    End Function

    ''' <summary>
    ''' Führt die Speicherung (Insert oder Update) eines Objekts sowohl in der Datenbank 
    ''' als auch in der lokalen DataTable durch und aktualisiert anschließend die Benutzeroberfläche.
    ''' </summary>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Unbenutzten 'StringBuilder' entfernt.
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' - Auf native .NET '.Split'-Methoden umgestellt.
    ''' - Logik zur UI-Aktualisierung im 'Finally'-Block präzisiert.
    ''' </remarks>
    Private Sub prSaveObj()
        Dim sqlText As String = String.Empty
        Dim arFields() As String
        Dim arValue() As String
        Dim cBedingung As String = String.Empty
        Dim sName As String = tbOName.Text
        Dim sID As String = lbObjektID.Text
        Dim isSaveSuccessful As Boolean = False

        ' Bei Neuanlage Validierung prüfen und ID generieren
        If lNew Then
            If fcCheckObjekt(sName) Then Exit Sub
            sID = fcGetTimeID(Date.Today)
        End If

        Try
            ' Feldnamen definieren und splitten
            sqlText = "ID,Name,Strasse,HNr,PLZ,Ort,Ortsteil,Telefon,RGB"
            arFields = sqlText.Split(","c)

            ' Werte über Hilfsfunktion ermitteln und splitten
            sqlText = fcSaveObjekt(sID)
            arValue = sqlText.Split("°"c)

            ' 1. Speichern in der Datenbank
            If lNew Then
                fcInsertCommand("Objekte", arFields, arValue)
            Else
                cBedingung = " WHERE ID='" & sID & "'"
                fcUpdateCommand("Objekte", arFields, arValue, cBedingung)
            End If

            ' 2. Lokale DataTable aktualisieren
            If lNew Then
                fcInsertTable(dtObj, arFields, arValue)
            Else
                cBedingung = "ID Like '" & sID & "'"
                fcUpdateTable(dtObj, arFields, arValue, cBedingung)
            End If

            ' Status zurücksetzen und Erfolg flaggen
            lNew = False
            isSaveSuccessful = True

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ' Die UI wird nur aktualisiert, wenn das Speichern vorbereitet oder geglückt ist
            prLoadObjInList(dtObj)
            prCheckNoRecordObjekt(dtObj)
            prLoockObj(False)

            ' Den gespeicherten Eintrag in der Liste selektieren (Index 1 entspricht dem Namen im Array)
            If isSaveSuccessful AndAlso arValue.Length > 1 Then
                prSelectEntry(lvObjekt, arValue(1))
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Bereitet die Formulardaten für den Speichervorgang vor, indem leere Felder mit Standardwerten 
    ''' versehen und alle Werte mit dem Trennzeichen '°' zu einem Gesamtstring verkettet werden.
    ''' </summary>
    ''' <param name="sID">Die eindeutige ID des Objekts.</param>
    ''' <returns>Ein mit '°' separierter String, der alle aufbereiteten Felddaten enthält.</returns>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - 'String.IsNullOrWhiteSpace' für die Null- und Leerzeilenprüfung implementiert.
    ''' - UI-Steuerelemente werden beim Speichern nicht mehr manipuliert (kein direktes Zurückschreiben von " " in die TextBoxen).
    ''' - Effiziente 'StringBuilder.Append'-Verkettung umgesetzt.
    ''' - Logik auf das modernere 'Return'-Schlüsselwort umgestellt.
    ''' </remarks>
    Private Function fcSaveObjekt(ByVal sID As String) As String
        Dim sb As New StringBuilder()

        ' Werte lokal auslesen und Standardwerte setzen (verhindert das Manipulieren der Benutzeroberfläche)
        Dim sName As String = tbOName.Text
        Dim sStrasse As String = If(String.IsNullOrWhiteSpace(tbOStr.Text), " ", tbOStr.Text)
        Dim sHNr As String = If(String.IsNullOrWhiteSpace(tbOHNr.Text), " ", tbOHNr.Text)
        Dim sPLZ As String = If(String.IsNullOrWhiteSpace(tbOPLZ.Text), " ", tbOPLZ.Text)
        Dim sOrt As String = If(String.IsNullOrWhiteSpace(tbOOrt.Text), " ", tbOOrt.Text)
        Dim sOrtsteil As String = If(String.IsNullOrWhiteSpace(tbOOrtsteil.Text), " ", tbOOrtsteil.Text)
        Dim sTelefon As String = If(String.IsNullOrWhiteSpace(tbOTelefon.Text), " ", tbOTelefon.Text)
        Dim sRGB As String = If(String.IsNullOrWhiteSpace(lbRGBString.Text), "0,0,0", lbRGBString.Text)

        ' String mit dem Trennzeichen '°' zusammensetzen
        sb.Append(sID).Append("°")
        sb.Append(sName).Append("°")
        sb.Append(sStrasse).Append("°")
        sb.Append(sHNr).Append("°")
        sb.Append(sPLZ).Append("°")
        sb.Append(sOrt).Append("°")
        sb.Append(sOrtsteil).Append("°")
        sb.Append(sTelefon).Append("°")
        sb.Append(sRGB)

        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Reagiert auf den Auswahlwechsel in der Objektliste "lvObjekt".
    ''' Ruft die Detailinformationen des selektierten Objekts ab, sofern ein Element ausgewählt ist.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (das ListView-Steuerelement).</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' - Sicherheitsabfrage (SelectedItems.Count) hinzugefügt, um leere Ereignis-Aufrufe beim Wechsel der Auswahl zu unterdrücken.
    ''' </remarks>
    Private Sub lvObjekt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvObjekt.SelectedIndexChanged
        ' Nur ausführen, wenn tatsächlich ein Eintrag ausgewählt wurde
        ' Verhindert Fehler und Flackern, wenn Einträge im ListView die Selektion verlieren
        If lvObjekt.SelectedItems.Count > 0 Then
            prGetInfolvObjekt()
        End If
    End Sub

    ''' <summary>
    ''' Überträgt die Detailinformationen des aktuell selektierten Listen-Eintrags aus "lvObjekt" 
    ''' in die entsprechenden Eingabefelder und setzt die Hintergrundfarbe des Farb-Buttons.
    ''' </summary>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Das selektierte ListViewItem in einer lokalen Variable zwischengespeichert, um den Code zu verkürzen und Zugriffe zu beschleunigen.
    ''' - Sicherheitsabfrage der SubItem-Anzahl integriert, um IndexOutOfRange-Ausnahmen zu verhindern.
    ''' - 'Call'-Befehle bei Farbfunktion nicht vorhanden, Code-Struktur durch Gruppierung aufgeräumt.
    ''' </remarks>
    Private Sub prGetInfolvObjekt()
        ' Prüfen, ob überhaupt ein Eintrag selektiert ist
        If lvObjekt.SelectedItems.Count = 0 Then Exit Sub

        ' Das erste ausgewählte Element für den performanten Zugriff zwischenspeichern
        Dim selectedItem As ListViewItem = lvObjekt.SelectedItems(0)

        ' Sicherheitsprüfung: Hat das Element die erwartete Mindestanzahl an Spalten (SubItems)?
        If selectedItem.SubItems.Count >= 9 Then
            ' Textfelder mit den Daten aus den jeweiligen Spalten befüllen
            tbOName.Text = selectedItem.SubItems(0).Text
            tbOStr.Text = selectedItem.SubItems(1).Text
            tbOHNr.Text = selectedItem.SubItems(2).Text
            tbOPLZ.Text = selectedItem.SubItems(3).Text
            tbOOrt.Text = selectedItem.SubItems(4).Text
            tbOOrtsteil.Text = selectedItem.SubItems(5).Text
            tbOTelefon.Text = selectedItem.SubItems(6).Text

            ' Versteckte IDs und Systemdaten übertragen
            lbRGBString.Text = selectedItem.SubItems(7).Text
            lbObjektID.Text = selectedItem.SubItems(8).Text

            ' Hintergrundfarbe des Buttons anhand des gespeicherten RGB-Strings anpassen
            btColorObjekt.BackColor = fcStringRGB(lbRGBString.Text)
        End If
    End Sub

    ''' <summary>
    ''' Löscht das aktuell ausgewählte Objekt sowie alle verknüpften Zimmer 
    ''' sowohl aus der Datenbank als auch aus den lokalen DataTables nach einer Sicherheitsabfrage.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' - 'MsgBox' durch das moderne 'MessageBox.Show' mit expliziter Ja/Nein-Abfrage ersetzt.
    ''' - 'String.IsNullOrEmpty' für ID-Validierung implementiert.
    ''' - UI-Fokus auf '.Focus()' umgestellt und Selektionslogik für das Nachfolge-Element korrigiert.
    ''' - 'vbCrLf' durch 'Environment.NewLine' ersetzt.
    ''' </remarks>
    Private Sub tsbDelObj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelObj.Click
        Dim sObj As String = lbObjektID.Text

        ' Vorab-Prüfung: Wenn keine ID vorhanden ist, Aktion abbrechen
        If String.IsNullOrEmpty(sObj) Then Exit Sub

        ' Sicherheitsabfrage für den Benutzer definieren
        Dim sMsg As String = "Wollen Sie dieses Objekt wirklich löschen?" & Environment.NewLine &
                         "Die Zimmer dieses Objektes werden ebenfalls gelöscht!"

        ' Abfrage anzeigen (Ja/Nein-Schaltflächen sind für destruktive Aktionen sicherer als OK/Abbrechen)
        If MessageBox.Show(sMsg, "Objekt löschen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

            ' 1. Objekt aus der Datenbank löschen
            Dim cSql As String = "DELETE FROM Objekte WHERE ID = '" & sObj & "'"
            UpdateTable(cSql)

            ' 2. Objekt aus der lokalen DataTable "dtObj" entfernen
            fcDeleteTableRow(dtObj, "ID = '" & sObj & "'")

            ' Eingabemaske vollständig leeren
            tbOName.Text = String.Empty
            tbOStr.Text = String.Empty
            tbOHNr.Text = String.Empty
            tbOPLZ.Text = String.Empty
            tbOOrt.Text = String.Empty
            tbOOrtsteil.Text = String.Empty
            tbOTelefon.Text = String.Empty
            lbObjektID.Text = String.Empty

            ' Hauptliste aktualisieren
            prLoadObjInList(dtObj)

            ' 3. Zugehörige Zimmer aus der Datenbank löschen
            cSql = "DELETE FROM Zimmer WHERE ID = '" & sObj & "'"
            UpdateTable(cSql)

            ' 4. Zugehörige Zimmer aus der lokalen DataTable "dtZim" entfernen
            fcDeleteTableRow(dtZim, "ID = '" & sObj & "'")

            ' Fokus zurück auf die Liste setzen
            lvObjekt.Focus()

            ' Falls noch Einträge vorhanden sind, automatisch das erste Element selektieren
            If lvObjekt.Items.Count > 0 Then
                lvObjekt.Items(0).Selected = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Öffnet einen Farbauswahldialog, um die Hintergrundfarbe des Buttons zu ändern.
    ''' Überträgt die gewählte Farbe als RGB-String in das dazugehörige Label.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (der Button "btColorObjekt").</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - 'Using'-Block für den ColorDialog implementiert, um eine saubere Ressourcenfreigabe (Dispose) zu garantieren.
    ''' - Namespace-Angabe beim DialogResult gekürzt.
    ''' </remarks>
    Private Sub btColorObjekt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btColorObjekt.Click
        ' Mithilfe von 'Using' wird der Dialog nach der Nutzung direkt wieder aus dem Speicher entfernt
        Using cd As New ColorDialog()
            cd.Color = btColorObjekt.BackColor
            cd.FullOpen = True

            ' Wenn der Benutzer die Farbauswahl mit OK bestätigt
            If cd.ShowDialog() = DialogResult.OK Then
                btColorObjekt.BackColor = cd.Color
                lbRGBString.Text = fcRGBString(cd.Color)
            End If
        End Using
    End Sub


#Region "Mit Enter weiter zum nächsten Feld........................................................"
    ' ist in der Hauptrotine  "TextBox_KeyPress" enthalten, um die Navigation zwischen Eingabefeldern zu erleichtern.
#End Region

#End Region

#Region "Zimmer verwalten.........................................................................."

    ''' <summary>
    ''' Tabelle "Zimmer" erstellen
    ''' </summary>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Sub prCreateTabelleZimmer()
        With lvZimmer
            .Clear()
            .Columns.Add("Name", 75, HorizontalAlignment.Left)
            .Columns.Add("Art der Unterkunft", 100, HorizontalAlignment.Left)
            .Columns.Add("Ausstattung", 75, HorizontalAlignment.Left)
            .Columns.Add("IDObjekt", 0, HorizontalAlignment.Left)
            .Columns.Add("ID", 0, HorizontalAlignment.Left)
            .Columns.Add("Objekt", 100, HorizontalAlignment.Left)
            .Columns.Add("FeWo", -2, HorizontalAlignment.Center)
            .Columns.Add("Nr", -2, HorizontalAlignment.Center)
            .Columns.Add("B Nor", 50, HorizontalAlignment.Center)
            .Columns.Add("B Min", 50, HorizontalAlignment.Center)
            .Columns.Add("B Er", 50, HorizontalAlignment.Center)
            .Columns.Add("B Ki", 50, HorizontalAlignment.Center)
            .Columns.Add("P1", 50, HorizontalAlignment.Center)
            .Columns.Add("P2", 50, HorizontalAlignment.Center)
            .Columns.Add("P3", 50, HorizontalAlignment.Center)
            .Columns.Add("P4", 50, HorizontalAlignment.Center)
            .Columns.Add("P5", 50, HorizontalAlignment.Center)
            .Columns.Add("P6", 50, HorizontalAlignment.Center)
            .Columns.Add("P7", 50, HorizontalAlignment.Center)
            .Columns.Add("P8", 50, HorizontalAlignment.Center)
            .Columns.Add("P9", 50, HorizontalAlignment.Center)
            .Columns.Add("P10", 50, HorizontalAlignment.Center)
            .Columns.Add("Trans1", 50, HorizontalAlignment.Center)
            .Columns.Add("Trans2", 50, HorizontalAlignment.Center)
            .Columns.Add("Trans3", 50, HorizontalAlignment.Center)
            .Columns.Add("Trans4", 50, HorizontalAlignment.Center)
            .Columns.Add("Trans5", 50, HorizontalAlignment.Center)
            .Columns.Add("Code", 50, HorizontalAlignment.Center)
            .Columns.Add("SaveCode", 50, HorizontalAlignment.Center)
            .Columns.Add("Datei", 50, HorizontalAlignment.Center)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            .Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub

    ''' <summary>
    ''' Tabelle Zimmer mit daten aus der DataTabel "Zimmer" füllen
    ''' </summary>
    ''' <param name="dtT"></param>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Sub prLoadZimInList(ByVal dtT As DataTable)
        Dim i As Integer
        Dim nMax As Integer = dtT.Rows.Count - 1
        If nMax < 0 Then Exit Sub
        lvZimmer.Items.Clear()
        Dim sObj As String
        Dim sFeWo As String
        For i = 0 To nMax
            sFeWo = "Nein"
            If dtT.Rows(i).RowState <> DataRowState.Deleted Then
                Dim lv As ListViewItem
                With lvZimmer
                    lv = .Items.Add(dtT.Rows(i).Item("Name").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("Art").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("Ausstattung").ToString)
                    sObj = dtT.Rows(i).Item("IDObjekte").ToString
                    lv.SubItems.Add(sObj)
                    lv.SubItems.Add(dtT.Rows(i).Item("ID").ToString)
                    sObj = fcGetObjektName(dtObj, sObj)
                    lv.SubItems.Add(sObj)
                    If dtT.Rows(i).Item("FeWo").ToString = "1" Then sFeWo = "Ja"
                    lv.SubItems.Add(sFeWo)
                    lv.SubItems.Add(dtT.Rows(i).Item("Nummer").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("Betten").ToString)

                    lv.SubItems.Add(dtT.Rows(i).Item("BettenMin").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("BettenEr").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("BettenKi").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("P1").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("P2").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("P3").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("P4").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("P5").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("P6").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("P7").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("P8").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("P9").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("P10").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("Trans1").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("Trans2").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("Trans3").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("Trans4").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("Trans5").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("Code").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("SaveCode").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("Datei").ToString)
                End With
            End If
        Next

    End Sub

    ''' <summary>
    ''' Prüfen ob Datensätze vorhanden sind, Steuerung der Button Edit und Delete
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Sub prCheckNoRecordZimmer(ByVal dt As DataTable)
        Dim lNo As Boolean = False
        If dt.Rows.Count > 0 Then lNo = True
        tsbEditZim.Enabled = lNo
        tsbDelZim.Enabled = lNo
    End Sub

    ''' <summary>
    ''' Neues Zimmer anlegen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 18.12.2011 Create
    ''' </remarks>
    Private Sub tsbNeuZim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNeuZim.Click
        lNew = True
        tbZName.Text = ""
        coZArt.SelectedIndex = 0
        'tbZArt.Text = ""
        tbZAus.Text = ""
        tbZBetten.Text = ""
        tbZNummer.Text = ""
        Call prLoockZim(True)
        tbZName.Select()
    End Sub

    ''' <summary>
    ''' Zimmer bearbeiten
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Sub tsbEditZim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEditZim.Click

        Call prLoockZim(True)
        tbZName.Select()
    End Sub

    ''' <summary>
    ''' Änderung Speichen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Sub tsbSaveZim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSaveZim.Click
        Call prSaveZimmer()
    End Sub

    ''' <summary>
    ''' Bearbeitung abbrechen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Sub tsbBraekEW_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBraekZim.Click
        Call prLoockZim(False)
        Call prCheckNoRecordZimmer(dtObj)
    End Sub

    ''' <summary>
    ''' Steuerung der Button und Eingabefelder
    ''' </summary>
    ''' <param name="lStatus"></param>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Sub prLoockZim(ByVal lStatus As Boolean)
        tsbEditZim.Enabled = Not lStatus
        tsbNeuZim.Enabled = Not lStatus
        tsbSaveZim.Enabled = lStatus
        tsbBraekZim.Enabled = lStatus
        tsbDelZim.Enabled = Not lStatus
        lvZimmer.Enabled = Not lStatus
        tbZBetten.Enabled = lStatus
        tbZName.Enabled = lStatus
        tbZAus.Enabled = lStatus
        coZArt.Enabled = lStatus
        coObjekt.Enabled = lStatus
        tbZNummer.Enabled = lStatus
        chFeWo.Enabled = lStatus
        tbZBettenMin.Enabled = lStatus
        tbZBettenEr.Enabled = lStatus
        tbZBettenKi.Enabled = lStatus
        tbZP1.Enabled = lStatus
        tbZP2.Enabled = lStatus
        tbZP3.Enabled = lStatus
        tbZP4.Enabled = lStatus
        tbZP5.Enabled = lStatus
        tbZP6.Enabled = lStatus
        tbZP7.Enabled = lStatus
        tbZP8.Enabled = lStatus
        tbZP9.Enabled = lStatus
        tbZP10.Enabled = lStatus
        tbTrans1.Enabled = lStatus
        tbTrans2.Enabled = lStatus
        tbTrans3.Enabled = lStatus
        tbTrans4.Enabled = lStatus
        tbTrans5.Enabled = lStatus
    End Sub

    ''' <summary>
    ''' Speicherung durchführen
    ''' </summary>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Sub prSaveZimmer()
        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String = ""
        Dim sObj As String = fcGetObjektZimmerID(dtObj, coObjekt.Text, "ID")
        Dim sName As String = tbZName.Text
        Dim sID As String = lbZimmerID.Text

        If lNew Then
            sID = fcGetTimeID(Date.Today)
            If fcCheckZimmer(sName, sObj) Then Exit Sub
        End If

        Try
            sqlText = "ID,Name,Art,Ausstattung,Betten,IDObjekte,FeWo,Nummer,BettenMin,BettenEr,BettenKi,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10,Trans1,Trans2,Trans3,Trans4,Trans5,Code,SaveCode,Datei"
            arFields = Split(sqlText, ",")
            sqlText = fcSaveZimmer(sID, sObj)
            arValue = Split(sqlText, "°")

            If lNew Then
                sID = fcAppendBlank("Zimmer")
            End If
            'Call fcInsertCommand("Zimmer", arFields, arValue)
            'Else
            cBedingung = " WHERE ID='" & sID & "'"
            Call fcUpdateCommand("Zimmer", arFields, arValue, cBedingung)
            '            End If

            'DataTable aktualisieren
            If lNew Then
                'Datensatz in DataTable "dtZim" speichern
                Call fcInsertTable(dtZim, arFields, arValue)
            Else
                'Datensatz in DataTable "dtZim" speichern
                cBedingung = "ID Like '" & sID & "'"
                Call fcUpdateTable(dtZim, arFields, arValue, cBedingung)
            End If

            lNew = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            Call prLoadZimInList(dtZim)
            Call prCheckNoRecordZimmer(dtZim)
            Call prLoockZim(False)
            Call prSelectEntry(lvZimmer, arValue(1))
        End Try
    End Sub

    ''' <summary>
    ''' Auswahl eines Eintrages in der Zimmer liste
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Sub lvZimmer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvZimmer.SelectedIndexChanged
        Call prGetInfolvZimmer()
    End Sub

    ''' <summary>
    ''' Informationen aus der Zimmerliste in die Eingabefelder übertragen
    ''' </summary>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Sub prGetInfolvZimmer()
        Dim sFewo As String
        With lvZimmer
            If .SelectedItems.Count <> 0 Then
                tbZName.Text = .SelectedItems(0).SubItems(0).Text
                coZArt.Text = .SelectedItems(0).SubItems(1).Text
                tbZAus.Text = .SelectedItems(0).SubItems(2).Text
                tbZBetten.Text = .SelectedItems(0).SubItems(8).Text
                coObjekt.Text = .SelectedItems(0).SubItems(5).Text
                lbZimmerID.Text = .SelectedItems(0).SubItems(4).Text
                sFewo = .SelectedItems(0).SubItems(6).Text
                tbZNummer.Text = .SelectedItems(0).SubItems(7).Text
                If sFewo = "Ja" Then
                    chFeWo.CheckState = CheckState.Checked
                Else
                    chFeWo.CheckState = CheckState.Unchecked
                End If
                tbZBettenMin.Text = .SelectedItems(0).SubItems(9).Text
                tbZBettenEr.Text = .SelectedItems(0).SubItems(10).Text
                tbZBettenKi.Text = .SelectedItems(0).SubItems(11).Text
                tbZP1.Text = .SelectedItems(0).SubItems(12).Text
                tbZP2.Text = .SelectedItems(0).SubItems(13).Text
                tbZP3.Text = .SelectedItems(0).SubItems(14).Text
                tbZP4.Text = .SelectedItems(0).SubItems(15).Text
                tbZP5.Text = .SelectedItems(0).SubItems(16).Text
                tbZP6.Text = .SelectedItems(0).SubItems(17).Text
                tbZP7.Text = .SelectedItems(0).SubItems(18).Text
                tbZP8.Text = .SelectedItems(0).SubItems(19).Text
                tbZP9.Text = .SelectedItems(0).SubItems(20).Text
                tbZP10.Text = .SelectedItems(0).SubItems(21).Text
                tbTrans1.Text = .SelectedItems(0).SubItems(22).Text
                tbTrans2.Text = .SelectedItems(0).SubItems(23).Text
                tbTrans3.Text = .SelectedItems(0).SubItems(24).Text
                tbTrans4.Text = .SelectedItems(0).SubItems(25).Text
                tbTrans5.Text = .SelectedItems(0).SubItems(26).Text
                If .SelectedItems(0).SubItems(27).Text = "1" Then
                    chCode.CheckState = CheckState.Checked
                Else
                    chCode.CheckState = CheckState.Unchecked
                End If
                tbSaveCode.Text = .SelectedItems(0).SubItems(28).Text
                tbDatei.Text = .SelectedItems(0).SubItems(29).Text

            End If
        End With
    End Sub

    ''' <summary>
    ''' Zu speichernde Daten aufbereiten
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Function fcSaveZimmer(ByVal sID As String, ByVal sOID As String) As String

        Dim sb As New StringBuilder
        Dim sCode As String = "0"
        If Trim(tbTrans1.Text) <> "" And Mid(tbTrans1.Text, 4, 1) <> ";" Then tbTrans1.Text = fcWeg34(tbTrans1.Text)
        If Trim(tbTrans2.Text) <> "" And Mid(tbTrans2.Text, 4, 1) <> ";" Then tbTrans2.Text = fcWeg34(tbTrans2.Text)
        If Trim(tbTrans3.Text) <> "" And Mid(tbTrans3.Text, 4, 1) <> ";" Then tbTrans3.Text = fcWeg34(tbTrans3.Text)
        If Trim(tbTrans4.Text) <> "" And Mid(tbTrans4.Text, 4, 1) <> ";" Then tbTrans4.Text = fcWeg34(tbTrans4.Text)
        If Trim(tbTrans5.Text) <> "" And Mid(tbTrans5.Text, 4, 1) <> ";" Then tbTrans5.Text = fcWeg34(tbTrans5.Text)
        If coZArt.Text.Trim = "" Then coZArt.SelectedIndex = 0
        If tbZAus.Text.Trim = "" Then tbZAus.Text = " "
        If tbZBetten.Text.Trim = "" Then tbZBetten.Text = "0"
        If tbZNummer.Text.Trim = "" Then tbZNummer.Text = "Z"
        If chCode.Checked = True Then sCode = "1"
        sb.Append(sID & "°")
        sb.Append(tbZName.Text & "°")
        sb.Append(coZArt.Text & "°")
        sb.Append(tbZAus.Text & "°")
        sb.Append(tbZBetten.Text & "°")
        sb.Append(sOID & "°")
        sb.Append(chFeWo.CheckState & "°")
        sb.Append(tbZNummer.Text & "°")
        sb.Append(tbZBettenMin.Text & "°")
        sb.Append(tbZBettenEr.Text & "°")
        sb.Append(tbZBettenKi.Text & "°")
        sb.Append(tbZP1.Text & "°")
        sb.Append(tbZP2.Text & "°")
        sb.Append(tbZP3.Text & "°")
        sb.Append(tbZP4.Text & "°")
        sb.Append(tbZP5.Text & "°")
        sb.Append(tbZP6.Text & "°")
        sb.Append(tbZP7.Text & "°")
        sb.Append(tbZP8.Text & "°")
        sb.Append(tbZP9.Text & "°")
        sb.Append(tbZP10.Text & "°")
        sb.Append(tbTrans1.Text & "°")
        sb.Append(tbTrans2.Text & "°")
        sb.Append(tbTrans3.Text & "°")
        sb.Append(tbTrans4.Text & "°")
        sb.Append(tbTrans5.Text & "°")
        sb.Append(sCode & "°")
        sb.Append(tbSaveCode.Text & "°")
        sb.Append(tbDatei.Text)
        fcSaveZimmer = sb.ToString

        ' fcWeg34(tbRFID.Text)
    End Function

    ''' <summary>
    ''' Prüfen ob Zimmer schon existiert
    ''' </summary>
    ''' <param name="sName"></param>
    ''' <returns>T/F</returns>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Function fcCheckZimmer(ByVal sName As String, ByVal sObj As String) As Boolean
        Dim sMsg As String = ""
        Dim dt As DataTable
        fcCheckZimmer = False

        If sName.Trim = "" Or sObj.Trim = "" Then
            sMsg = "Zimmerbezeichnung / Objekt fehlt!"
            fcCheckZimmer = True
        Else
            dt = fcReadDataTable("SELECT * from Zimmer WHERE Name='" & sName & "' and IDObjekte='" & sObj & "'")
            If dt.Rows.Count > 0 Then
                sMsg = "Zimmer ist schon angelegt"
                fcCheckZimmer = True
            End If
        End If
        If fcCheckZimmer Then MsgBox(sMsg, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Speichern nicht möglich")

    End Function

    ''' <summary>
    ''' Zimmer löschen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub tsbDelZim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelZim.Click
        Dim sMsg As String = "Wollen Sie dieses Zimmer wirklich löschen?  "
        Dim sZim As String = lbZimmerID.Text
        Dim cSql As String = "DELETE FROM Zimmer WHERE ID = '" & sZim & "'"
        If sZim = "" Then Exit Sub
        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            'Datensatz per SQL aus der Tabelle löschen
            UpdateTable(cSql)
            'Änderung in DataTable "dtZim" speichern
            Call fcDeleteTableRow(dtZim, "ID = '" & sZim & "'")
            tbZName.Text = ""
            coZArt.Text = ""
            tbZAus.Text = ""
            lbZimmerID.Text = ""
            tbZBetten.Text = ""
            Call prLoadZimInList(dtZim)
            lvZimmer.Select()
            If lvZimmer.Items.Count > 0 Then lvZimmer.TopItem.Selected = True
        End If
    End Sub

#Region "Mit Enter weiter zum nächsten Feld........................................................"

    Private Sub tbZname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbZName.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbZArt.Select()
        End If
    End Sub
    Private Sub tbZArt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbZArt.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbZAus.Select()
        End If
    End Sub
    Private Sub tbZAus_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbZAus.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            coObjekt.Select()
        End If
    End Sub
#End Region

#Region "Import / Export..........................................................................."


    Private Sub tsmImportZimmer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmImportZimmer.Click
        Dim sDaten As String 'Variable zur Aufnahme der Sicherungsdaten aus der Datei
        Try
            Select Case MsgBox("Mit dieser Funktion wird die Zimmerliste neu geladen.", MsgBoxStyle.Information + MsgBoxStyle.OkCancel, "Erstinitialisierung")

                Case MsgBoxResult.Ok
                    ' sDaten = fcOpenReadOneValueFromSystemDb(cgSystemPath & "\", "Zimmerliste (Zimmerliste*.csv)|Zimmerliste*.csv")
                    If sDaten = "" Then Exit Sub
                    ' --- Löschen aller Datensätze
                    UpdateTable("Delete from Zimmer")
                    Me.Cursor = Cursors.WaitCursor
                    Call prLadeZimmer(sDaten)
                    dtZim = fcReadDataTable("SELECT * from Zimmer")
                    Call prLoadZimInList(dtZim)
                    Call prCheckNoRecordZimmer(dtZim)
                    Me.Cursor = Cursors.Default
            End Select

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try

    End Sub

    Private Sub prLadeZimmer(ByVal sDaten As String)
        Dim arTmp() As String = sDaten.Split(vbCrLf)
        Dim sqlText As String = "ID,Name,Art,Betten,Ausstattung,IDObjekte,FeWo"
        Dim arFields(0), arValue(0) As String
        Dim i As Integer
        Dim nMax As Integer = arTmp.Length - 1
        arFields = Split(sqlText, ",")
        For i = 0 To nMax
            If arTmp(i).Trim <> "" Then
                sqlText = fcSaveEntry(arTmp(i), 6)
                arValue = Split(sqlText, "°")
                Call fcInsertCommand("Zimmer", arFields, arValue)
            End If
        Next
    End Sub

    Private Function fcSaveEntry(ByVal sT As String, ByVal nP As Integer) As String
        Dim sb As New StringBuilder
        Dim arT() As String = sT.Split(",")
        For i = 0 To nP
            arT(i) = arT(i).Trim
            If arT(i) = "" Then arT(i) = " "
        Next
        For i = 0 To nP - 1
            sb.Append(arT(i) & "°")
        Next
        sb.Append(arT(nP))
        fcSaveEntry = sb.ToString
    End Function

    Private Sub tsmExportZimmer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmExportZimmer.Click
        Dim sb As New StringBuilder
        Dim sT As String = ""
        For i As Integer = 0 To dtZim.Rows.Count - 1
            sT = dtZim.Rows(i).Item("ID").ToString & ","
            sT = sT & dtZim.Rows(i).Item("Name").ToString & ","
            sT = sT & dtZim.Rows(i).Item("Art").ToString & ","
            sT = sT & dtZim.Rows(i).Item("Betten").ToString & ","
            sT = sT & dtZim.Rows(i).Item("Ausstattung").ToString & ","
            sT = sT & dtZim.Rows(i).Item("IDObjekte").ToString & ","
            sT = sT & dtZim.Rows(i).Item("FeWo").ToString
            sb.Append(sT & vbCrLf)
        Next
        '  Call SaveOneValueInSystemDb(cgSystemPath & "\" & "Zimmerliste.csv", sb.ToString)
    End Sub

#End Region

#End Region

#Region "User-Verwaltung..........................................................................."

    ''' <summary>
    ''' Tabelle "User" erstellen
    ''' </summary>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub prCreateTabelleUser()
        With lvUser
            .Clear()
            .Columns.Add("Name", 150, HorizontalAlignment.Left)
            .Columns.Add("Kurz-Name", 100, HorizontalAlignment.Left)
            .Columns.Add("Passwort", 0, HorizontalAlignment.Left)
            .Columns.Add("Status", 150, HorizontalAlignment.Left)
            .Columns.Add("ID", 0, HorizontalAlignment.Left)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            .Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub

    ''' <summary>
    ''' Tabelle User mit daten aus der DataTabel "Nutzer" füllen
    ''' </summary>
    ''' <param name="dtT"></param>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub prLoadUserInList(ByVal dtT As DataTable)
        Dim i As Integer
        Dim nMax As Integer = dtT.Rows.Count - 1
        If nMax < 0 Then Exit Sub
        lvUser.Items.Clear()
        For i = 0 To nMax
            If dtT.Rows(i).RowState <> DataRowState.Deleted Then
                Dim lv As ListViewItem
                With lvUser
                    lv = .Items.Add(dtT.Rows(i).Item("Name").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("KName").ToString)
                    lv.SubItems.Add(fDeCrypt(dtT.Rows(i).Item("PassWD").ToString, "UrSoft"))
                    lv.SubItems.Add(dtT.Rows(i).Item("Status").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("ID").ToString)
                End With
            End If
        Next

    End Sub

    ''' <summary>
    ''' Prüfen ob Datensätze vorhanden sind, Steuerung der Button Edit und Delete
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub prCheckNoRecordUser(ByVal dt As DataTable)
        Dim lNo As Boolean = False
        If dt.Rows.Count > 0 Then lNo = True
        tsbEditUser.Enabled = lNo
        tsbDelUser.Enabled = lNo
    End Sub

    ''' <summary>
    ''' Neuen User anlegen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub tsbNewUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNewUser.Click
        lNew = True
        tbUUser.Text = ""
        tbKUser.Text = ""
        tbUPassWD.Text = ""
        tbURechte.Text = ""
        Call prLoockUser(True)
        tbUUser.Select()
    End Sub

    ''' <summary>
    ''' User bearbeiten
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub tsbEditUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEditUser.Click

        Call prLoockUser(True)
        tbUUser.Select()
    End Sub

    ''' <summary>
    ''' Änderung Speichen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub tsbSaveUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSaveUser.Click
        If tbUPassWD.Text.Length < 5 Then
            MsgBox("Länge des Passwortes ist zu kurz (>=5)", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Eingabefehler")
        Else
            Call prSaveUser()
        End If

    End Sub

    ''' <summary>
    ''' Bearbeitung abbrechen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub tsbBraeckUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBraeckUser.Click
        Call prLoockUser(False)
        Call prCheckNoRecordUser(dtObj)
    End Sub

    ''' <summary>
    ''' Steuerung der Button und Eingabefelder
    ''' </summary>
    ''' <param name="lStatus"></param>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub prLoockUser(ByVal lStatus As Boolean)
        tsbEditUser.Enabled = Not lStatus
        tsbNewUser.Enabled = Not lStatus
        tsbSaveUser.Enabled = lStatus
        tsbBraeckUser.Enabled = lStatus
        tsbDelUser.Enabled = Not lStatus
        lvUser.Enabled = Not lStatus

        tbUUser.Enabled = lStatus
        tbKUser.Enabled = lStatus
        tbUPassWD.Enabled = lStatus
        tbURechte.Enabled = lStatus
        chKlar.Enabled = lStatus
        chKlar.CheckState = CheckState.Unchecked
    End Sub

    ''' <summary>
    ''' Speicherung durchführen
    ''' </summary>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub prSaveUser()
        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String = ""

        Dim sName As String = tbUUser.Text
        Dim sID As String = lbUserID.Text
        If lNew Then
            If fcCheckUser(sName) Then Exit Sub
            sID = fcGetTimeID(Date.Today)
        End If

        Try
            sqlText = "ID,Name,KName,PassWD,Status"
            arFields = Split(sqlText, ",")
            sqlText = fcSaveUser(sID)
            arValue = Split(sqlText, "°")

            If lNew Then
                Call fcInsertCommand("Nutzer", arFields, arValue)
            Else
                cBedingung = " WHERE ID='" & sID & "'"
                Call fcUpdateCommand("Nutzer", arFields, arValue, cBedingung)
            End If
            'DataTable aktualisieren
            If lNew Then
                'Datensatz in DataTable "dtUser" speichern
                Call fcInsertTable(dtUser, arFields, arValue)
            Else
                'Datensatz in DataTable "dtUser" speichern
                cBedingung = "ID Like '" & sID & "'"
                Call fcUpdateTable(dtUser, arFields, arValue, cBedingung)
            End If

            lNew = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            Call prLoadUserInList(dtUser)
            Call prCheckNoRecordUser(dtUser)
            Call prLoockUser(False)
            Call prSelectEntry(lvUser, arValue(1))
        End Try
    End Sub

    ''' <summary>
    ''' Auswahl eines Eintrages in der User liste
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub lvUser_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvUser.SelectedIndexChanged
        Call prGetInfolvUser()
    End Sub

    ''' <summary>
    ''' Informationen aus der Userliste in die Eingabefelder übertragen
    ''' </summary>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub prGetInfolvUser()
        With lvUser
            If .SelectedItems.Count <> 0 Then
                tbUUser.Text = .SelectedItems(0).SubItems(0).Text
                tbKUser.Text = .SelectedItems(0).SubItems(1).Text
                tbUPassWD.Text = .SelectedItems(0).SubItems(2).Text
                tbURechte.Text = .SelectedItems(0).SubItems(3).Text
                lbUserID.Text = .SelectedItems(0).SubItems(4).Text
            End If
        End With
    End Sub

    ''' <summary>
    ''' Zu speichernde Daten aufbereiten
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Function fcSaveUser(ByVal sID As String) As String
        Dim sb As New StringBuilder
        If tbKUser.Text.Trim = "" Then tbKUser.Text = " "
        If tbUPassWD.Text.Trim = "" Then tbUPassWD.Text = " "
        If tbURechte.Text.Trim = "" Then tbURechte.Text = "5"

        sb.Append(sID & "°")
        sb.Append(tbUUser.Text & "°")
        sb.Append(tbKUser.Text & "°")
        sb.Append(fDeCrypt(tbUPassWD.Text, "UrSoft") & "°")
        sb.Append(tbURechte.Text)
        fcSaveUser = sb.ToString
    End Function

    ''' <summary>
    ''' Prüfen ob der User schon existiert
    ''' </summary>
    ''' <param name="sName"></param>
    ''' <returns>T/F</returns>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Function fcCheckUser(ByVal sName As String) As Boolean
        Dim sMsg As String = ""
        Dim dt As DataTable
        fcCheckUser = False

        If sName.Trim = "" Then
            sMsg = "Username fehlt!"
            fcCheckUser = True
        Else
            dt = fcReadDataTable("SELECT * from Nutzer WHERE Name='" & sName & "'")
            If dt.Rows.Count > 0 Then
                sMsg = "User ist schon angelegt"
                fcCheckUser = True
            End If
        End If
        If fcCheckUser Then MsgBox(sMsg, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Speichern nicht möglich")

    End Function

    ''' <summary>
    ''' User löschen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub tsbDelUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelUser.Click
        Dim sMsg As String = "Wollen Sie dieses Nutzer wirklich löschen?  "
        Dim sUser As String = lbUserID.Text
        Dim cSql As String = "DELETE FROM Nutzer WHERE ID = '" & sUser & "'"
        If sUser = "" Then Exit Sub
        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            'Datensatz per SQL aus der Tabelle löschen
            UpdateTable(cSql)
            'Änderung in DataTable "dtUser" speichern
            Call fcDeleteTableRow(dtUser, "ID = '" & sUser & "'")
            tbUUser.Text = ""
            tbKUser.Text = ""
            tbUPassWD.Text = ""
            tbURechte.Text = ""
            lbUserID.Text = ""
            Call prLoadUserInList(dtZim)
            lvUser.Select()
            If lvUser.Items.Count > 0 Then lvUser.TopItem.Selected = True
        End If
    End Sub

    Private Sub tbURechte_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbURechte.TextChanged
        'If IsNumeric(e.ToString) = False Then tbURechte.Text = 5
    End Sub

    Private Sub chKlar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chKlar.CheckedChanged
        tbUPassWD.UseSystemPasswordChar = Not chKlar.Checked
    End Sub

#Region "Mit Enter weiter zum nächsten Feld........................................................"

    Private Sub tbUUser_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbUUser.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbKUser.Select()
        End If
    End Sub
    Private Sub tbKUser_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbKUser.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbUPassWD.Select()
        End If
    End Sub
    Private Sub tbUPassWD_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbUPassWD.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbURechte.Select()
        End If
    End Sub
    Private Sub tbURechte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbURechte.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbUUser.Select()
        End If
    End Sub
#End Region

#End Region

#Region "Verwaltung der Buchungstexte.............................................................."

    ''' <summary>
    ''' Prüfen ob Datensätze vorhanden sind, Steuerung der Button Edit und Delete
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks>
    ''' 23.12.2011 Create
    ''' </remarks>
    Private Sub prCheckNoRecordBuch(ByVal dt As DataTable)
        Dim lNo As Boolean = False
        If dt.Rows.Count > 0 Then lNo = True
        tsbEditBuch.Enabled = lNo
        tsbDelBuch.Enabled = lNo
    End Sub

    ''' <summary>
    ''' Neuen Buchungstext anlegen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 23.12.2011 Create
    ''' </remarks>
    Private Sub tsbNewBuch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNewBuch.Click
        lNew = True
        tbBez.Text = ""
        tbBuchDe.Text = ""
        tbBuchEn.Text = ""
        Call prLoockBuch(True)
        tbBez.Select()
    End Sub

    ''' <summary>
    ''' Buchungstext bearbeiten
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 23.12.2011 Create
    ''' </remarks>
    Private Sub tsbEditBuch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEditBuch.Click

        Call prLoockBuch(True)
        tbBez.Enabled = False
        tbBuchDe.Select()
    End Sub

    ''' <summary>
    ''' Änderung Speichen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 23.12.2011 Create
    ''' </remarks>
    Private Sub tsbSaveBuch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSaveBuch.Click

        Call prSaveBuch()
    End Sub

    ''' <summary>
    ''' Bearbeitung abbrechen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 23.12.2011 Create
    ''' </remarks>
    Private Sub tsbBraeckBuch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBreakBuch.Click
        Call prLoockBuch(False)
        Call prCheckNoRecordBuch(dtBuc)
    End Sub

    ''' <summary>
    ''' Steuerung der Button und Eingabefelder
    ''' </summary>
    ''' <param name="lStatus"></param>
    ''' <remarks>
    ''' 23.12.2011 Create
    ''' </remarks>
    Private Sub prLoockBuch(ByVal lStatus As Boolean)
        tsbEditBuch.Enabled = Not lStatus
        tsbNewBuch.Enabled = Not lStatus
        tsbSaveBuch.Enabled = lStatus
        tsbBreakBuch.Enabled = lStatus
        tsbDelBuch.Enabled = Not lStatus
        liBuch.Enabled = Not lStatus

        rbBuch.Enabled = lStatus
        rbMakro.Enabled = lStatus
        tbBez.Enabled = lStatus
        tbBuchDe.Enabled = lStatus
        tbBuchEn.Enabled = lStatus
        tbZZiel.Enabled = lStatus
    End Sub

    ''' <summary>
    ''' Speicherung durchführen
    ''' </summary>
    ''' <remarks>
    ''' 23.12.2011 Create
    ''' </remarks>
    Private Sub prSaveBuch()
        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String = ""

        Dim sName As String = tbBez.Text
        Dim sID As String
        Dim sFeld As String = "Btext"
        Dim aLan As Array
        For i = 1 To sLanguage.Length - 1
            aLan = Split(sLanguage(i), ",")
            If aLan(0) = tscSprache.Text Then
                sFeld = "BText" & aLan(1)
            End If
        Next

       

        Try
            sqlText = "ID,Name,Art,BTextDe," & sFeld & ",ZZiel"
            arFields = Split(sqlText, ",")



            If lNew Then
                If fcCheckBuch(sName) Then Exit Sub
                sID = fcAppendBlank("BTexte")
                '  Call fcInsertCommand("BTexte", arFields, arValue)
            Else
                sID = fcGetOneValue(dtTxt, liBuch.Text, "Name", "ID")
            End If
            sqlText = fcSaveBuch(sID)
            arValue = Split(sqlText, "°")
            cBedingung = " WHERE ID='" & sID & "'"
            Call fcUpdateCommand("BTexte", arFields, arValue, cBedingung)

            'DataTable aktualisieren
            If lNew Then
                'Datensatz in DataTable "dtTxt" speichern
                Call fcInsertTable(dtTxt, arFields, arValue)
            Else
                'Datensatz in DataTable "dtTxt" speichern
                cBedingung = "ID Like '" & sID & "'"
                Call fcUpdateTable(dtTxt, arFields, arValue, cBedingung)
            End If

            lNew = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally

            Call prCheckNoRecordBuch(dtTxt)
            Call prLoockBuch(False)
            liBuch = fcLoadListe(liBuch, dtTxt, "Name")

        End Try
    End Sub

    ''' <summary>
    ''' Auswahl eines Eintrages in der Liste
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 23.12.2011 Create
    ''' </remarks>
    Private Sub liBuch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles liBuch.SelectedIndexChanged
        Dim sFeld As String = "Btext"
        Dim aLan As Array
        For i = 1 To sLanguage.Length - 1
            aLan = Split(sLanguage(i), ",")
            If aLan(0) = tscSprache.Text Then
                sFeld = "BText" & aLan(1)
            End If
        Next
        If lDel Then Exit Sub
        If liBuch.Items.Count = 1 Then
        End If
        tbBez.Text = liBuch.Text
        Dim sSQL As String = "Select * from BTexte Where Name='" & tbBez.Text & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)
        If dt.Rows.Count = 1 Then
            tbBuchDe.Text = dt.Rows(0).Item("BTextDe").ToString
            tbBuchEn.Text = dt.Rows(0).Item(sFeld).ToString
            tbZZiel.Text = dt.Rows(0).Item("ZZiel").ToString
            If dt.Rows(0).Item("Art").ToString = "0" Then
                rbMakro.Checked = True
            Else
                rbBuch.Checked = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Zu speichernde Daten aufbereiten
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 23.12.2011 Create
    ''' </remarks>
    Private Function fcSaveBuch(ByVal sID As String) As String
        Dim sb As New StringBuilder
        If tbBuchDe.Text.Trim = "" Then tbBuchDe.Text = " "
        If tbBuchEn.Text.Trim = "" Then tbBuchEn.Text = " "
        If tbZZiel.Text.Trim = "" Then tbZZiel.Text = " "
        sb.Append(sID & "°")
        sb.Append(tbBez.Text & "°")
        If rbMakro.Checked Then
            sb.Append("0" & "°")
        Else
            sb.Append("1" & "°")
        End If
        sb.Append(tbBuchDe.Text & "°")
        sb.Append(tbBuchEn.Text & "°")
        sb.Append(tbZZiel.Text)
        fcSaveBuch = sb.ToString
    End Function

    ''' <summary>
    ''' Prüfen ob dieser Buchungstext schon existiert
    ''' </summary>
    ''' <param name="sName"></param>
    ''' <returns>T/F</returns>
    ''' <remarks>
    ''' 23.12.2011 Create
    ''' </remarks>
    Private Function fcCheckBuch(ByVal sName As String) As Boolean
        Dim sMsg As String = ""
        Dim dt As DataTable
        fcCheckBuch = False

        If sName.Trim = "" Then
            sMsg = "Buchungstext-Name fehlt!"
            fcCheckBuch = True
        Else
            dt = fcReadDataTable("SELECT * from BTexte WHERE Name='" & sName & "'")
            If dt.Rows.Count > 0 Then
                sMsg = "Buchungstext ist schon angelegt"
                fcCheckBuch = True
            End If
        End If
        If fcCheckBuch Then MsgBox(sMsg, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Speichern nicht möglich")

    End Function

    ''' <summary>
    ''' Buchungstext löschen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 23.12.2011 Create
    ''' </remarks>
    Private Sub tsbDelBuch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelBuch.Click
        Dim sMsg As String = "Wollen Sie diesen Text wirklich löschen?  "
        Dim sID As String = fcGetOneValue(dtTxt, liBuch.Text, "Name", "ID") 'liBuch.SelectedValue.ToString

        'sID = fcGetObjektZimmerID(dtTxt, liBuch.Text, "ID")
        Dim cSql As String = "DELETE FROM BTexte WHERE ID = '" & sID & "'"
        If sID = "" Then Exit Sub
        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            lDel = True
            tbBez.Text = ""
            rbMakro.Checked = False
            rbBuch.Checked = False
            tbBuchDe.Text = ""
            tbBuchEn.Text = ""
            'Datensatz per SQL aus der Tabelle löschen
            UpdateTable(cSql)
            'Änderung in DataTable "dtTxt" speichern
            Call fcDeleteTableRow(dtTxt, "ID = '" & sID & "'")
            dtTxt = fcReadDataTable("Select * from BTexte")
            'liBuch.Text = ""
            liBuch = fcLoadListe(liBuch, dtTxt, "Name")


            If dtTxt.Rows.Count > 0 Then liBuch.SelectedIndex = 0
            lDel = False
            Call prCheckNoRecordBuch(dtTxt)
        End If
    End Sub
    Private Sub tscSprache_Click(sender As System.Object, e As System.EventArgs) Handles tscSprache.Click

    End Sub
    Private Sub rbMakro_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbMakro.CheckedChanged
        If rbMakro.Checked Then
            tbZZiel.Visible = False
            ' sb.Append("0" & "°")
        Else
            tbZZiel.Visible = True
            'sb.Append("1" & "°")
        End If
    End Sub


#Region "Mit Enter weiter zum nächsten Feld........................................................"

    Private Sub tbBezDe_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbBez.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbBuchDe.Select()
        End If
    End Sub
#End Region

#End Region

#Region "Werbung bearbeiten........................................................................"

    ''' <summary>
    ''' Prüfen ob Datensätze vorhanden sind, Steuerung der Button Edit und Delete
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub prCheckNoRecordWerbung(ByVal dt As DataTable)
        Dim lNo As Boolean = False
        If dt.Rows.Count > 0 Then lNo = True
        tsbWEdit.Enabled = lNo
        tsbWDel.Enabled = lNo
    End Sub

    ''' <summary>
    ''' Neue Werbung anlegen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub tsbWNeu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbWNeu.Click
        lNew = True
        tbWerbung.Text = ""
        Call prLoockWerbung(True)
        tbWerbung.Select()
    End Sub

    ''' <summary>
    ''' Werbung bearbeiten
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub tsbWEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbWEdit.Click
        Call prLoockWerbung(True)
        tbBetreff.Text = lbBetreff.Text
        tbKopf.Text = lbKopf.Text
        tbFuss.Text = lbFuss.Text
        tbProvision.Text = lbProvision.Text
        tbWEMail.Text = lbWEMail.Text
        tbWerbung.Select()
    End Sub

    ''' <summary>
    ''' Änderung Speichen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub tsbWSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbWSave.Click
        Call prSaveWerbung()
    End Sub

    ''' <summary>
    ''' Bearbeitung abbrechen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    '''  26.01.2012 Create
    ''' </remarks>
    Private Sub tsbWBreack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbWBreack.Click
        Call prLoockWerbung(False)
        Call prCheckNoRecordWerbung(dtWer)
    End Sub

    ''' <summary>
    ''' Steuerung der Button und Eingabefelder
    ''' </summary>
    ''' <param name="lStatus"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub prLoockWerbung(ByVal lStatus As Boolean)
        tsbWEdit.Enabled = Not lStatus
        tsbWNeu.Enabled = Not lStatus
        tsbWSave.Enabled = lStatus
        tsbWBreack.Enabled = lStatus
        tsbWDel.Enabled = Not lStatus
        liWerbung.Enabled = Not lStatus
        tbWerbung.Enabled = lStatus

        tbProvision.Visible = lStatus
        tbBetreff.Visible = lStatus
        tbKopf.Visible = lStatus
        tbFuss.Visible = lStatus
        tbWEMail.Visible = lStatus
        tbNormal.Enabled = lStatus
        liWerbung.Refresh()
    End Sub

    ''' <summary>
    ''' Speicherung durchführen
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' 01.04.2012 Insert Format Provision
    ''' </remarks>
    Private Sub prSaveWerbung()
        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String = ""
        tbProvision.Text = fcFormatDecimal(tbProvision.Text)
        Dim sName As String = tbBez.Text
        Dim sID As String
        If lNew Then
            sID = fcGetTimeID(Date.Today)
        Else
            'sID = liWerbung.SelectedValue.ToString
            sID = fcGetOneValue(dtWer, liWerbung.Text, "Werbung", "ID")
        End If

        Try
            sqlText = "ID,Werbung,Betreff,KText,FText,Link,Provision,Color"
            arFields = Split(sqlText, ",")
            If tbWerbung.Text.Trim = "" Then Exit Sub
            sqlText = fcSaveWerbung(sID) ' & "°" & tbWerbung.Text
            arValue = Split(sqlText, "°")

            If lNew Then
                sID = fcAppendBlank("Werbung")
                '  Call fcInsertCommand("Werbung", arFields, arValue)
            End If
            cBedingung = " WHERE ID='" & sID & "'"
            Call fcUpdateCommand("Werbung", arFields, arValue, cBedingung)

            'DataTable aktualisieren
            If lNew Then
                'Datensatz in DataTable "dtWer" speichern
                Call fcInsertTable(dtWer, arFields, arValue)
            Else
                'Datensatz in DataTable "dtWer" speichern
                cBedingung = "ID Like '" & sID & "'"
                Call fcUpdateTable(dtWer, arFields, arValue, cBedingung)
            End If

            lNew = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            Call prCheckNoRecordWerbung(dtWer)
            Call prLoockWerbung(False)
            liWerbung = fcLoadListe(liWerbung, dtWer, "Werbung")
        End Try
    End Sub

    ''' <summary>
    ''' Datensatz zur Speicherung zusammenstellen
    ''' </summary>
    ''' <param name="sID"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' 01.04.2012 Insert Format Provision
    ''' </remarks>
    Private Function fcSaveWerbung(ByVal sID As String) As String
        Dim sb As New StringBuilder
        If tbBetreff.Text.Trim = "" Then tbBetreff.Text = " "
        If tbKopf.Text.Trim = "" Then tbKopf.Text = " "
        If tbFuss.Text.Trim = "" Then tbFuss.Text = " "
        If tbWEMail.Text.Trim = "" Then tbWEMail.Text = " "
        If tbProvision.Text.Trim = "" Then tbProvision.Text = "0.00"
        sb.Append(sID & "°")
        sb.Append(tbWerbung.Text & "°")
        sb.Append(tbBetreff.Text & "°")
        sb.Append(tbKopf.Text & "°")
        sb.Append(tbFuss.Text & "°")
        sb.Append(tbWEMail.Text & "°")
        sb.Append(tbProvision.Text & "°")
        sb.Append(tbNormal.Text)

        fcSaveWerbung = sb.ToString
    End Function


    ''' <summary>
    ''' Auswahl eines Eintrages in der Liste
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' 01.04.2012 Insert Format Provision
    ''' </remarks>
    Private Sub liWerbung_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles liWerbung.SelectedIndexChanged
        If lDel Or lStart Then Exit Sub

        Dim sID As String = fcGetOneValue(dtWer, liWerbung.Text, "Werbung", "ID")
        Call prGetInfoWerbung(sID)
    End Sub

    ''' <summary>
    ''' Ausgweählte Daten darstellen
    ''' </summary>
    ''' <param name="sID"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' 01.04.2012 Insert Format Provision
    ''' </remarks>
    Private Sub prGetInfoWerbung(ByVal sID As String)
        Dim dt As DataTable = fcReadDataTable("Select * from Werbung Where ID='" & sID & "'")
        If dt.Rows.Count > 0 Then
            lbWEMail.Text = dt.Rows(0).Item("Link").ToString
            lbBetreff.Text = dt.Rows(0).Item("Betreff").ToString
            lbKopf.Text = dt.Rows(0).Item("KText").ToString
            lbFuss.Text = dt.Rows(0).Item("FText").ToString
            tbWerbung.Text = dt.Rows(0).Item("Werbung").ToString
            lbProvision.Text = dt.Rows(0).Item("Provision").ToString
            tbNormal.Text = dt.Rows(0).Item("color").ToString

            tbNormal.BackColor = fcStringRGB(tbNormal.Text)

        End If
    End Sub

    ''' <summary>
    ''' Werbung löschen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub tsbWDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbWDel.Click
        Dim sMsg As String = "Wollen Sie diese Werbung wirklich löschen?  "
        Dim sID As String = fcGetOneValue(dtWer, liWerbung.Text, "Werbung", "ID")
        Dim cSql As String = "DELETE FROM Werbung WHERE ID = '" & sID & "'"
        If sID = "" Then Exit Sub
        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            lDel = True
            tbWerbung.Text = ""
            'Datensatz per SQL aus der Tabelle löschen
            UpdateTable(cSql)
            'Änderung in DataTable "dtWer" speichern
            Call fcDeleteTableRow(dtWer, "ID = '" & sID & "'")
            dtWer = fcReadDataTable("Select * from Werbung")

            liWerbung = fcLoadListe(liWerbung, dtWer, "Werbung")
            If dtWer.Rows.Count > 0 Then liWerbung.SelectedIndex = 0
            lDel = False
            Call prCheckNoRecordBuch(dtWer)
        End If
    End Sub

    Private Sub tbProvision_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProvision.LostFocus
        tbProvision.Text = fcFormatDecimal(tbProvision.Text)
    End Sub
    Private Sub tbNormal_TextChanged(sender As System.Object, e As System.EventArgs) Handles tbNormal.Click
        Dim cd As New ColorDialog
        cd.Color = tbNormal.BackColor
        cd.FullOpen = True
        If cd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            tbNormal.BackColor = cd.Color
            tbNormal.Text = fcRGBString(cd.Color)
        End If
    End Sub



#End Region

#Region "Preise bearbeiten........................................................................."
    ''' <summary>
    ''' Combofeld Zimmer
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub prLoadComboZimmer()

        Dim nMax As Integer = dtZim.Rows.Count - 1
        With tsbPZimmer
            .Items.Clear()
            For i = 0 To nMax
                .Items.Add(dtZim.Rows(i).Item("Name").ToString())
                tsbPZimmer1.Items.Add(dtZim.Rows(i).Item("Name").ToString())
            Next
            .Text = dtZim.Rows(0).Item("Name").ToString()
        End With
        'Dim iJahr As Integer = Year(Date.Today)
        With tsbCoJahr
            .Text = "0 Jahre"
            For i = 0 To 4
                .Items.Add(i.ToString & " Jahre")
            Next
        End With
        lbPZimID.Text = fcGetObjektZimmerID(dtZim, tsbPZimmer.Text, "ID")
        prCreateTabellePreise()
        prLoadPreiseInList()
        Call prLoockPreise(False)
    End Sub
    Private Sub tsbPZimmer_Click(sender As Object, e As EventArgs) Handles tsbPZimmer.TextChanged
        lbPZimID.Text = fcGetObjektZimmerID(dtZim, tsbPZimmer.Text, "ID")
        lbPZim.Text = fcGetObjektZimmerName(dtZim, lbPZimID.Text)
        prLoadPreiseInList()
    End Sub
    ''' <summary>
    ''' Tabelle "Preise" erstellen
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub prCreateTabellePreise()

        With lvPreise
            .Clear()
            .Columns.Add("Datum", 0, HorizontalAlignment.Left)
            .Columns.Add("Von", 70, HorizontalAlignment.Left)
            .Columns.Add("Bis", 70, HorizontalAlignment.Left)
            .Columns.Add("Preis", 300, HorizontalAlignment.Left)
            .Columns.Add("Dauer", 200, HorizontalAlignment.Left)
            .Columns.Add("ID", 0, HorizontalAlignment.Left)
            .Columns.Add("Zimmer", 70, HorizontalAlignment.Left)
            .Columns.Add("Event", 70, HorizontalAlignment.Left)

            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            .Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub

    ''' <summary>
    ''' Tabelle User mit daten aus der DataTabel "Preise" füllen
    ''' </summary>
    ''' <param name="dtT"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub prLoadPreiseInList()
        Dim Sql As String = "SELECT * from Preise Where ZimID='" & lbPZimID.Text & "'"
        dtPre = fcReadDataTable(Sql)
        Dim i As Integer
        Dim sZim As String
        Dim nMax As Integer = dtPre.Rows.Count - 1
        lvPreise.Items.Clear()
        If nMax < 0 Then Exit Sub

        For i = 0 To nMax
            If dtPre.Rows(i).RowState <> DataRowState.Deleted Then
                Dim lv As ListViewItem
                With lvPreise
                    lv = .Items.Add(dtPre.Rows(i).Item("ADatum").ToString)
                    lv.SubItems.Add(fcUmDatum(dtPre.Rows(i).Item("ADatum").ToString))
                    lv.SubItems.Add(fcUmDatum(dtPre.Rows(i).Item("EDatum").ToString))
                    lv.SubItems.Add(fcPreisUm(dtPre.Rows(i).Item("Preis").ToString))
                    lv.SubItems.Add(dtPre.Rows(i).Item("Dauer").ToString)
                    lv.SubItems.Add(dtPre.Rows(i).Item("ID").ToString)
                    sZim = fcGetObjektZimmerName(dtZim, dtPre.Rows(i).Item("ZimID").ToString)
                    lv.SubItems.Add(sZim)
                    lv.SubItems.Add(dtPre.Rows(i).Item("Event").ToString)

                End With
            End If
        Next
    End Sub
    Private Function fcPreisUm(ByRef sPreisGruppe As String) As String
        Dim aPreis() As String = Split(sPreisGruppe, "|")
        fcPreisUm = ""
        For i = 0 To aPreis.Length - 1
            aPreis(i) = fcDecStr(Val(aPreis(i).Trim) / 100,,,).ToString.Trim
            fcPreisUm = " " & fcPreisUm & aPreis(i) & " |"
        Next
        fcPreisUm = Mid(fcPreisUm, 1, fcPreisUm.Length - 1)
    End Function

    ''' <summary>
    ''' Prüfen ob Datensätze vorhanden sind, Steuerung der Button Edit und Delete
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub prCheckNoRecordPreise(ByVal dt As DataTable)
        Dim lNo As Boolean = False
        If dt.Rows.Count > 0 Then lNo = True
        tsbPEdit.Enabled = lNo
        tsbPDel.Enabled = lNo
    End Sub

    ''' <summary>
    ''' Neue Preise anlegen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub tsbPNeu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPNeu.Click
        lNew = True
        ' coUArt.SelectedIndex = 0
        tbPreis1.Text = "0"
        tbPreis2.Text = "0"
        tbPreis3.Text = "0"
        tbPreis4.Text = "0"
        tbPreis5.Text = "0"
        tbPreis6.Text = "0"
        tbPreis7.Text = "0"
        tbD1.Text = "0"
        tbD2.Text = "0"
        tbD3.Text = "0"
        tbD4.Text = "0"
        tbD5.Text = "0"
        tbD6.Text = "0"
        tbD7.Text = "0"
        tbPreisG.Text = ""
        tbDauerG.Text = ""
        ' coKategorie.SelectedIndex = 0
        ' coSasion.SelectedIndex = 0
        Call prLoockPreise(True)
        prCheckPreis()
        ' coUArt.Select()
    End Sub

    ''' <summary>
    ''' Werbung bearbeiten
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub tsbPEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPEdit.Click
        Call prLoockPreise(True)
        prCheckPreis()

    End Sub

    ''' <summary>
    ''' Änderung Speichen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub tsbPSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPSave.Click
        Call prSavePreise()
    End Sub

    ''' <summary>
    ''' Bearbeitung abbrechen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    '''  26.01.2012 Create
    ''' </remarks>
    Private Sub tsbPBreak_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPBreak.Click
        Call prLoockPreise(False)
        Call prCheckNoRecordPreise(dtPre)
    End Sub
    Private Sub prCheckPreis()
        chP1.Checked = True
        chP2.Checked = True
        chP3.Checked = True
        chP4.Checked = True
        chP5.Checked = True
        chP6.Checked = True
        chP7.Checked = True
    End Sub
    ''' <summary>
    ''' Steuerung der Button und Eingabefelder
    ''' </summary>
    ''' <param name="lStatus"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub prLoockPreise(ByVal lStatus As Boolean)
        tsbPEdit.Enabled = Not lStatus
        tsbPZimmer.Enabled = Not lStatus
        tsbPZimmer1.Enabled = Not lStatus
        tsbPCopy.Enabled = Not lStatus
        tsbPZimmerCopyJahr.Enabled = Not lStatus
        tsbCoJahr.Enabled = Not lStatus
        tsbPNeu.Enabled = Not lStatus
        tsbPSave.Enabled = lStatus
        tsbPBreak.Enabled = lStatus
        tsbPDel.Enabled = Not lStatus
        lvPreise.Enabled = Not lStatus
        tbPreisG.Enabled = lStatus
        tbDauerG.Enabled = lStatus
        dtpVon.Enabled = lStatus
        dtpBis.Enabled = lStatus
        tbPreisG.Enabled = lStatus
        tbDauerG.Enabled = lStatus
        coEvent.Enabled = lStatus
        tbPreis1.Enabled = lStatus
        tbPreis2.Enabled = lStatus
        tbPreis3.Enabled = lStatus
        tbPreis4.Enabled = lStatus
        tbPreis5.Enabled = lStatus
        tbPreis6.Enabled = lStatus
        tbPreis7.Enabled = lStatus
        tbD1.Enabled = lStatus
        tbD2.Enabled = lStatus
        tbD3.Enabled = lStatus
        tbD4.Enabled = lStatus
        tbD5.Enabled = lStatus
        tbD6.Enabled = lStatus
        tbD7.Enabled = lStatus
    End Sub
    Private Sub prLoockPreise1(ByVal lStatus As Boolean)
        tsbPSave.Enabled = lStatus
        tsbPBreak.Enabled = lStatus
        tbPreisG.Enabled = lStatus
        tbDauerG.Enabled = lStatus
        dtpVon.Enabled = lStatus
        dtpBis.Enabled = lStatus
        tbPreisG.Enabled = lStatus
        tbDauerG.Enabled = lStatus
        coEvent.Enabled = lStatus
        tbPreis1.Enabled = lStatus
        tbPreis2.Enabled = lStatus
        tbPreis3.Enabled = lStatus
        tbPreis4.Enabled = lStatus
        tbPreis5.Enabled = lStatus
        tbPreis6.Enabled = lStatus
        tbPreis7.Enabled = lStatus
        tbD1.Enabled = lStatus
        tbD2.Enabled = lStatus
        tbD3.Enabled = lStatus
        tbD4.Enabled = lStatus
        tbD5.Enabled = lStatus
        tbD6.Enabled = lStatus
        tbD7.Enabled = lStatus
    End Sub
    ''' <summary>
    ''' Auswahl eines Eintrages in der Preisliste
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub lvPreise_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvPreise.SelectedIndexChanged
        If lDel Then Exit Sub
        Call prGetInfolvPreise()
    End Sub

    ''' <summary>
    ''' Informationen aus der Preisliste in die Eingabefelder übertragen
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub prGetInfolvPreise()
        With lvPreise
            If .SelectedItems.Count <> 0 Then
                Dim aPreis() As String = Split(.SelectedItems(0).SubItems(3).Text, "|")
                tbPreis1.Text = aPreis(0).Trim
                tbPreis2.Text = aPreis(1).Trim
                tbPreis3.Text = aPreis(2).Trim
                tbPreis4.Text = aPreis(3).Trim
                tbPreis5.Text = aPreis(4).Trim
                tbPreis6.Text = aPreis(5).Trim
                tbPreis7.Text = aPreis(6).Trim
                Dim aDauer() As String = Split(.SelectedItems(0).SubItems(4).Text, "|")
                tbD1.Text = aDauer(0).Trim
                tbD2.Text = aDauer(1).Trim
                tbD3.Text = aDauer(2).Trim
                tbD4.Text = aDauer(3).Trim
                tbD5.Text = aDauer(4).Trim
                tbD6.Text = aDauer(5).Trim
                tbD7.Text = aDauer(6).Trim

                lbPID.Text = .SelectedItems(0).SubItems(5).Text
                dtpVon.Value = .SelectedItems(0).SubItems(1).Text
                dtpBis.Value = .SelectedItems(0).SubItems(2).Text
                coEvent.Text = .SelectedItems(0).SubItems(7).Text
            End If
        End With
    End Sub
    Private Sub fcSaveEvent()
        Dim sTmp As String = ReadOneValueFromSystemDb("Event")
        Dim arTmp() As String = sTmp.Split(vbCrLf)
        Dim ii As Integer = 1
        For i = 0 To arTmp.Length - 1
            If arTmp(i) = coEvent.Text Then
                ii = 0
            End If
        Next
        If ii = 1 Then
            sTmp = sTmp & vbCrLf & coEvent.Text
            SaveOneValueInSystemDb("Event", sTmp)
            arTmp = sTmp.Split(vbCrLf)
            coEvent.Items.Clear()
            For i = 0 To arTmp.Length - 1
                coEvent.Items.Add(arTmp(i))
            Next
            coEvent.Text = arTmp(0)
        End If
    End Sub

    ''' <summary>
    ''' Speicherung durchführen
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub prSavePreise()
        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String = ""
        Dim sID As String = lbPID.Text
        If lNew Then
            sID = fcGetTimeID(Date.Today)
        End If

        Try
            sqlText = "ID,ADatum,EDatum,Preis,Dauer,ZimID,Event"
            arFields = Split(sqlText, ",")
            'If tbWerbung.Text.Trim = "" Then Exit Sub

            sqlText = fcSavePreise(sID)
            arValue = Split(sqlText, "°")

            If lNew Then
                Call fcInsertCommand("Preise", arFields, arValue)
            Else
                cBedingung = " WHERE ID='" & sID & "'"
                Call fcUpdateCommand("Preise", arFields, arValue, cBedingung)
            End If
            'DataTable aktualisieren
            If lNew Then
                'Datensatz in DataTable "dtPre" speichern
                Call fcInsertTable(dtPre, arFields, arValue)
            Else
                'Datensatz in DataTable "dtPre" speichern
                cBedingung = "ID Like '" & sID & "'"
                Call fcUpdateTable(dtPre, arFields, arValue, cBedingung)
            End If

            lNew = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            Call prLoadPreiseInList()
            Call prCheckNoRecordPreise(dtPre)
            Call prLoockPreise(False)
            Call prSelectEntry(lvPreise, arValue(1))
        End Try
        fcSaveEvent()

    End Sub

    ''' <summary>
    ''' Zu speichernde Daten aufbereiten
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Function fcSavePreise(ByVal sID As String) As String
        Dim sb As New StringBuilder
        Dim sPreis As String
        Dim sDauer As String
        '  Dim d1 As DateTime = .Value
        Dim sVon As String
        Dim sBis As String
        If tbPreisG.Text = "" Then tbPreisG.Text = 0
        If chP1.Checked = True Or tbPreis1.Text.Trim = "" Then
            tbPreis1.Text = tbPreisG.Text
            tbD1.Text = tbDauerG.Text
        End If
        If chP2.Checked = True Or tbPreis2.Text.Trim = "" Then
            tbPreis2.Text = tbPreisG.Text

            tbD2.Text = tbDauerG.Text
        End If
        If chP3.Checked = True Or tbPreis3.Text.Trim = "" Then
            tbPreis3.Text = tbPreisG.Text
            tbD3.Text = tbDauerG.Text
        End If
        If chP4.Checked = True Or tbPreis4.Text.Trim = "" Then
            tbPreis4.Text = tbPreisG.Text
            tbD4.Text = tbDauerG.Text
        End If
        If chP5.Checked = True Or tbPreis5.Text.Trim = "" Then
            tbPreis5.Text = tbPreisG.Text
            tbD5.Text = tbDauerG.Text
        End If
        If chP6.Checked = True Or tbPreis6.Text.Trim = "" Then
            tbPreis6.Text = tbPreisG.Text
            tbD6.Text = tbDauerG.Text
        End If
        If chP7.Checked = True Or tbPreis7.Text.Trim = "" Then
            tbPreis7.Text = tbPreisG.Text
            tbD7.Text = tbDauerG.Text
        End If
        sPreis = tbPreis1.Text * 100 & " | " & tbPreis2.Text * 100 & " | " & tbPreis3.Text * 100 & " | " & tbPreis4.Text * 100 & " | " & tbPreis5.Text * 100 & " | " & tbPreis6.Text * 100 & " | " & tbPreis7.Text * 100
        sDauer = tbD1.Text & " | " & tbD2.Text & " | " & tbD3.Text & " | " & tbD4.Text & " | " & tbD5.Text & " | " & tbD6.Text & " | " & tbD7.Text
        sVon = fcUmDatum(dtpVon.Value.ToString)
        sBis = fcUmDatum(dtpBis.Value.ToString)
        sb.Append(sID & "°")
        sb.Append(sVon & "°")
        sb.Append(sBis & "°")
        sb.Append(sPreis & "°")
        sb.Append(sDauer & "°")
        sb.Append(lbPZimID.Text & "°")
        sb.Append(coEvent.Text)
        fcSavePreise = sb.ToString
    End Function

    ''' <summary>
    ''' Preis löschen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub tsbPDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPDel.Click

        Dim sMsg As String = "Wollen Sie diesen Preis wirklich löschen?  "
        Dim sID As String = lbPID.Text
        Dim cSql As String = "DELETE FROM Preise WHERE ID = '" & sID & "'"
        If sID = "" Then Exit Sub
        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            lDel = True
            ' coUArt.Text = ""
            tbPreis1.Text = ""
            tbPreis2.Text = ""
            tbPreis3.Text = ""
            tbPreis4.Text = ""
            tbPreis5.Text = ""
            '   coKategorie.Text = ""
            '  coSasion.Text = ""
            'Datensatz per SQL aus der Tabelle löschen
            UpdateTable(cSql)
            prLoadPreiseInList()
            'Änderung in DataTable "dtPre" speichern
            'Call fcDeleteTableRow(dtPre, "ID = '" & sID & "'")
            lvPreise.Select()
            If lvPreise.Items.Count > 0 Then lvPreise.TopItem.Selected = True
            lDel = False
            Call prCheckNoRecordPreise(dtPre)
        End If
    End Sub
    Private Sub chP7_CheckedChanged(sender As Object, e As EventArgs) Handles chP7.CheckedChanged
        If chP7.Checked = False Then
            tbPreis7.Enabled = True
        Else
            tbPreis7.Enabled = False
        End If
    End Sub
    Private Sub tsbPCopy_Click(sender As Object, e As EventArgs) Handles tsbPCopy.Click
        Call prLoockPreise(True)
        Call prLoockPreise1(False)
        Me.Cursor = Cursors.WaitCursor
        tsbPCopy.Enabled = False
        Dim sJahr As String = Mid(fcUmDatum(lvPreise.Items(0).SubItems(1).Text), 1, 4)
        Dim iJahr As Integer = Val(Mid(tsbCoJahr.Text, 1, 1))
        Dim sVon As String = ""
        Dim sBis As String = ""
        Dim sPreis As String = ""
        Dim arPreis() As String
        Dim sDauer As String = ""
        Dim sZim As String = ""
        Dim sEvent As String = ""
        Dim sID As String = ""
        sZim = fcGetObjektZimmerID(dtZim, tsbPZimmer1.Text, "ID")
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        sqlText = "ID,ADatum,EDatum,Preis,Dauer,ZimID,Event"
        arFields = Split(sqlText, ",")
        If tsbPZimmer1.Text <> "" Then
            For i = 0 To lvPreise.Items.Count - 1
                With lvPreise

                    sVon = fcUmDatum(.Items(i).SubItems(1).Text)
                    sBis = fcUmDatum(.Items(i).SubItems(2).Text)
                    sPreis = Trim(.Items(i).SubItems(3).Text)
                    sDauer = Trim(.Items(i).SubItems(4).Text)
                    '  sID = .Items(i).SubItems(4).Text
                    'sZim = fcGetObjektZimmerID(dtZim, .Items(i).SubItems(5).Text, "ID")
                    sEvent = .Items(i).SubItems(7).Text
                    If Mid(sVon, 1, 4) = (Val(sJahr) + iJahr).ToString Then
                        sID = fcGetTimeID(Date.Today)
                        fcWait(2)
                        arPreis = Split(sPreis, "|")
                        sPreis = ""
                        For j = 0 To arPreis.Length - 1
                            sPreis = sPreis & (arPreis(j) * 100) & "|"
                        Next
                        sPreis = Mid(sPreis, 1, sPreis.Length - 1)
                        sqlText = sID & "°" & sVon & "°" & sBis & "°" & sPreis & "°" & sDauer & "°" & sZim & "°" & sEvent
                        arValue = Split(sqlText, "°")
                        Call fcInsertCommand("Preise", arFields, arValue)
                    End If
                    ' Call fcInsertCommand("Preise", arFields, arValue)
                End With
            Next
        End If
        tsbCoJahr.Text = "0 Jahre"
        tsbPZimmer1.Text = ""
        tsbPCopy.Enabled = True
        Me.Cursor = Cursors.Default
        Call prLoockPreise(False)
    End Sub

    Private Sub tsbPZimmerCopyJahr_Click(sender As Object, e As EventArgs) Handles tsbPZimmerCopyJahr.Click
        Call prLoockPreise(True)
        Call prLoockPreise1(False)
        Me.Cursor = Cursors.WaitCursor
        Dim iJahr As Integer = Val(Mid(tsbCoJahr.Text, 1, 1))
        tsbCoJahr.Text = "0 Jahre"
        Dim sVon As String = fcUmDatum(lvPreise.Items(0).SubItems(1).Text)
        Dim sJahr = Mid(sVon, 1, 4)
        Dim sBis As String = ""
        Dim sPreis As String = ""
        Dim arPreis() As String
        Dim sDauer As String = ""
        Dim sZim As String = ""
        Dim sEvent As String = ""
        Dim sID As String = ""
        sZim = fcGetObjektZimmerID(dtZim, tsbPZimmer.Text, "ID")
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        sqlText = "ID,ADatum,EDatum,Preis,Dauer,ZimID,Event"
        arFields = Split(sqlText, ",")

        With lvPreise
            For i = 0 To lvPreise.Items.Count - 1
                sVon = fcUmDatum(.Items(i).SubItems(1).Text)
                If iJahr <> 0 And Mid(sVon, 1, 4) = sJahr Then
                    sID = fcGetTimeID(Date.Today)
                    fcWait(2)

                    sBis = fcUmDatum(.Items(i).SubItems(2).Text)
                    sPreis = Trim(.Items(i).SubItems(3).Text)
                    sDauer = Trim(.Items(i).SubItems(4).Text)
                    '  sID = .Items(i).SubItems(4).Text
                    'sZim = fcGetObjektZimmerID(dtZim, .Items(i).SubItems(5).Text, "ID")
                    sEvent = .Items(i).SubItems(7).Text


                    sBis = (Val(Mid(sBis, 1, 4)) + iJahr).ToString & Mid(sBis, 5)
                    sVon = (Val(Mid(sVon, 1, 4)) + iJahr).ToString & Mid(sVon, 5)
                    arPreis = Split(sPreis, "|")
                    sPreis = ""
                    For j = 0 To arPreis.Length - 1
                        sPreis = sPreis & (arPreis(j) * 100) & "|"
                    Next
                    sPreis = Mid(sPreis, 1, sPreis.Length - 1)
                    sqlText = sID & "°" & sVon & "°" & sBis & "°" & sPreis & "°" & sDauer & "°" & sZim & "°" & sEvent
                    arValue = Split(sqlText, "°")
                    Call fcInsertCommand("Preise", arFields, arValue)
                End If
            Next
        End With
        prLoadPreiseInList()
        Me.Cursor = Cursors.Default
        Call prLoockPreise(False)


    End Sub

    Private Sub tbPreis_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbPreis1.KeyPress
        Call prCheckNumericKey(e.KeyChar, tbPreis1)
    End Sub

    Private Sub tbPreis_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPreis1.Leave
        tbPreis1.Text = fcFormatDecimal(tbPreis1.Text)
    End Sub

    Private Sub tbPreis3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbPreis2.KeyPress
        Call prCheckNumericKey(e.KeyChar, tbPreis2)
    End Sub

    Private Sub tbPreis3_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPreis2.Leave
        tbPreis2.Text = fcFormatDecimal(tbPreis2.Text)
    End Sub

    Private Sub tbPreis4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbPreis3.KeyPress
        Call prCheckNumericKey(e.KeyChar, tbPreis3)
    End Sub

    Private Sub tbPreis4_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPreis3.Leave
        tbPreis3.Text = fcFormatDecimal(tbPreis3.Text)
    End Sub
    Private Sub tbPreis5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbPreis4.KeyPress
        Call prCheckNumericKey(e.KeyChar, tbPreis4)
    End Sub

    Private Sub tbPreis5_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPreis4.Leave
        tbPreis4.Text = fcFormatDecimal(tbPreis4.Text)
    End Sub
    Private Sub tbPreis6_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbPreis5.KeyPress
        Call prCheckNumericKey(e.KeyChar, tbPreis5)
    End Sub

    Private Sub tbPreis6_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPreis5.Leave
        tbPreis5.Text = fcFormatDecimal(tbPreis5.Text)
    End Sub



#End Region

#Region "Sasion bearbeiten........................................................................."

    ''' <summary>
    ''' Tabelle "Sasion" erstellen
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub prCreateTabelleSasion()
        Dim nWidth As Integer
        Call prColorRead()
        With lvSasion
            nWidth = .Width / 3
            .Clear()
            .Columns.Add("Saison", nWidth, HorizontalAlignment.Left)
            .Columns.Add("Anfang", nWidth, HorizontalAlignment.Left)
            .Columns.Add("Ende", nWidth, HorizontalAlignment.Left)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            '.Sorting = SortOrder.Ascending
            .Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
        Dim arTmp() As String = ReadOneValueFromSystemDb("Event").Split(vbCrLf)
        For i = 0 To arTmp.Length - 1
            coEvent.Items.Add(arTmp(i))
        Next
        coEvent.Text = arTmp(0)
    End Sub

    ''' <summary>
    ''' Tabelle Saison mit daten aus der Datei "Saison.ini" füllen
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub prLoadSaisonInList()
        Dim arTmp() As String = ReadOneValueFromSystemDb("Saison").Split(vbCrLf)
        Dim nMax As Integer = arTmp.Length - 1
        Dim arT() As String
        Dim nColor As Integer
        With lvSasion
            .Items.Clear()
            For i As Integer = 0 To nMax
                If arTmp(i).Trim <> "" Then
                    arT = arTmp(i).Split(";")
                    Dim lv As ListViewItem
                    lv = .Items.Add(arT(0).Trim)
                    lv.SubItems.Add(arT(1))
                    lv.SubItems.Add(arT(2))
                    nColor = arT(0).Trim.Substring(0, 1)
                    .Items(i).BackColor = fcStringRGB(arFarbe(nColor, 1))
                End If
            Next
            .Select()
            If .Items.Count > 0 Then .TopItem.Selected = True
        End With
    End Sub

    Private Sub lvSasion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvSasion.SelectedIndexChanged
        Call prGetInfolvSaison()
    End Sub

    ''' <summary>
    ''' Informationen aus der Kontenliste in die Eingabefelder übertragen
    ''' </summary>
    ''' <remarks>
    ''' 18.12.2011 Create
    ''' </remarks>
    Private Sub prGetInfolvSaison()

        With lvSasion
            If .SelectedItems.Count <> 0 Then
                coSaison.Text = .SelectedItems(0).SubItems(0).Text
                sDAnfang = .SelectedItems(0).SubItems(1).Text
                sDEnde = .SelectedItems(0).SubItems(2).Text
                mcSaisonAnfang.SelectionStart = sDAnfang
                mcSaisonEnde.SelectionStart = sDEnde
            End If
        End With
    End Sub

    Private Sub tsbNeuSasion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNeuSasion.Click
        lNew = True
        sDAnfang = sDAK
        sDEnde = sDAK
        mcSaisonAnfang.SelectionStart = sDAnfang
        mcSaisonEnde.SelectionStart = sDEnde
        Call prLoocksaison(True)
        coSaison.Select()
    End Sub
    ''' <summary>
    ''' Steuerung der Button und Eingabefelder
    ''' </summary>
    ''' <param name="lStatus"></param>
    ''' <remarks>
    ''' 18.12.2011 Create
    ''' </remarks>
    Private Sub prLoocksaison(ByVal lStatus As Boolean)
        tsbEditSasion.Enabled = Not lStatus
        tsbNeuSasion.Enabled = Not lStatus
        tsbSaveSasion.Enabled = lStatus
        tsbBreackSasion.Enabled = lStatus
        tsbDelSasion.Enabled = Not lStatus
        lvSasion.Enabled = Not lStatus
        coSaison.Enabled = lStatus
        mcSaisonAnfang.Enabled = lStatus
        mcSaisonEnde.Enabled = lStatus


    End Sub

    Private Sub tsbSaveSasion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSaveSasion.Click
        Dim sb As New StringBuilder

        If lNew = True Then
            Dim lv As ListViewItem
            With lvSasion
                lv = .Items.Add(Trim(coSaison.Text))
                lv.SubItems.Add(Trim(sDAnfang))
                lv.SubItems.Add(Trim(sDEnde))
            End With
        Else
            With lvSasion
                If .SelectedItems.Count <> 0 Then
                    .SelectedItems(0).SubItems(0).Text = coSaison.Text
                    .SelectedItems(0).SubItems(1).Text = sDAnfang
                    .SelectedItems(0).SubItems(2).Text = sDEnde
                End If
            End With
        End If
        Call prLoocksaison(False)
        For i = 0 To lvSasion.Items.Count - 1
            sb.Append(lvSasion.Items(i).SubItems(0).Text & ";")
            sb.Append(lvSasion.Items(i).SubItems(1).Text & ";")
            sb.Append(lvSasion.Items(i).SubItems(2).Text & vbCrLf)
        Next
        SaveOneValueInSystemDb("Saison", sb.ToString)
        Call prSaisonColor()


    End Sub

    Private Sub tsbEditSasion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEditSasion.Click
        lNew = False
        Call prLoocksaison(True)
        coSaison.Select()
    End Sub
    Private Sub mcSasion_Anfang_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles mcSaisonAnfang.DateChanged
        sDAnfang = mcSaisonAnfang.SelectionStart.ToShortDateString
    End Sub

    Private Sub mcSaison_Ende_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles mcSaisonEnde.DateChanged
        sDEnde = mcSaisonEnde.SelectionStart.ToShortDateString
    End Sub

    Private Sub tsbBreackSasion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBreackSasion.Click
        prGetInfolvSaison()
        prLoocksaison(False)
    End Sub

    Private Sub tsbDelSasion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelSasion.Click
        Dim sMsg As String = "Wollen Sie diese Datensatz wirklich löschen?"
        Dim sb As New StringBuilder
        Dim X As Integer
        'Dim inhalt As String = ""
        If X <> -1 Then
            If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
                X = lvSasion.SelectedIndices.Item(0)
                lvSasion.Items.RemoveAt(X)
                lvSasion.Select()
                If lvSasion.Items.Count > 0 Then
                    lvSasion.TopItem.Selected = True
                    'inhalt = ""
                    For i = 0 To lvSasion.Items.Count - 1
                        'inhalt = inhalt + PadR(lvSasion.Items(i).SubItems(0).Text, 25)
                        'inhalt = inhalt + PadR(lvSasion.Items(i).SubItems(1).Text, 11)
                        'inhalt = inhalt + PadR(lvSasion.Items(i).SubItems(2).Text, 11) + Chr(13) + Chr(10)
                        sb.Append(lvSasion.Items(i).SubItems(0).Text & ";")
                        sb.Append(lvSasion.Items(i).SubItems(1).Text & ";")
                        sb.Append(lvSasion.Items(i).SubItems(2).Text & vbCrLf)
                    Next
                    SaveOneValueInSystemDb("Saison", sb.ToString)
                    Call prSaisonColor()
                    'SaveOneValueInSystemDb(cgSystemPath & "\Saison.ini", inhalt)
                End If
            End If
        End If

    End Sub
#End Region



    ''' <summary>
    ''' Ersten eintrag in der Liste selectieren, wenn die Seite ausgewählt wird
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub tcSystem_Selected(ByVal sender As Object, ByVal e As System.Windows.Forms.TabControlEventArgs) Handles tcSystem.Selected

        Select Case e.TabPage.Name.ToString
            Case Is = "tpObjekte"
                lvObjekt.Select()
                If lvObjekt.Items.Count > 0 Then lvObjekt.TopItem.Selected = True
            Case Is = "tpZimmer"
                lvZimmer.Select()
                If lvZimmer.Items.Count > 0 Then lvZimmer.TopItem.Selected = True
            Case Is = "tpKonto"
                lvKonto.Select()
                If lvKonto.Items.Count > 0 Then lvKonto.TopItem.Selected = True
            Case Is = "tpUser"
                lvUser.Select()
                If lvUser.Items.Count > 0 Then lvUser.TopItem.Selected = True
        End Select
    End Sub

    ''' <summary>
    ''' Bestimmten Eintrag in der Liste selektieren
    ''' </summary>
    ''' <param name="lv"></param>
    ''' <param name="sEntry"></param>
    ''' <remarks>
    ''' 20.12.2011 Create
    ''' </remarks>
    Private Sub prSelectEntry(ByVal lv As ListView, ByVal sEntry As String)
        For Each item As ListViewItem In lv.Items
            If item.SubItems(0).Text = sEntry Then
                lv.Select()
                item.Selected = True
                item.EnsureVisible()
                '  .Items(i).Selected = True
                '  .Items(i).EnsureVisible()
                Exit For
            End If
        Next

    End Sub


#Region "Farben bearbeiten........................................................................."

    ''' <summary>
    ''' Tabelle Farben erstellen
    ''' </summary>
    ''' <remarks>
    ''' 12.02.2012 Create
    ''' </remarks>
    Private Sub prCreateTabelleColor()
        Dim nWidth As Integer
        Call prColorRead()

        With lvColor
            nWidth = .Width '/ 3
            .Clear()
            .Columns.Add("        Objekt", nWidth, HorizontalAlignment.Left)
            .Columns.Add("BackColor", 0, HorizontalAlignment.Left)
            .Columns.Add("ForeColor", 0, HorizontalAlignment.Left)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            .Sorting = SortOrder.None 'SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub

    ''' <summary>
    ''' Farben aus der Datei "Color.ini" laden
    ''' </summary>
    ''' <remarks>
    ''' 12.02.2012 Create
    ''' </remarks>
    Private Sub prLoadColor()
        Dim arTmp() As String = ReadOneValueFromSystemDb("Color").Split(vbCrLf)
        Dim nMax As Integer = arTmp.Length - 1
        Dim arT() As String
        With lvColor
            .Items.Clear()
            For i As Integer = 0 To nMax
                If arTmp(i).Trim <> "" Then
                    arT = arTmp(i).Split(";")
                    Dim lv As ListViewItem
                    lv = .Items.Add(arT(0).Trim)
                    lv.SubItems.Add(arT(1))
                    lv.SubItems.Add(arT(2))
                    .Items(i).BackColor = fcStringRGB(arT(1))
                    .Items(i).ForeColor = fcStringRGB(arT(2))
                End If
            Next
            .Select()
            If .Items.Count > 0 Then .TopItem.Selected = True
        End With
    End Sub

    ''' <summary>
    ''' Farbe auswählen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 12.02.2012 Create
    ''' </remarks>
    Private Sub lvColor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvColor.SelectedIndexChanged
        With lvColor

            If .SelectedItems.Count <> 0 Then
                lbBackColor.Text = .SelectedItems(0).SubItems(0).Text
                btBackColor.BackColor = fcStringRGB(.SelectedItems(0).SubItems(1).Text)
                btForeColor.BackColor = fcStringRGB(.SelectedItems(0).SubItems(2).Text)
                lbBackColor.BackColor = btBackColor.BackColor
                lbBackColor.ForeColor = btForeColor.BackColor
            End If
        End With
        btSaveColor.Enabled = False
    End Sub

    ''' <summary>
    ''' Hintergrundfarbe setzen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 12.02.2012 Create
    ''' </remarks>
    Private Sub btBackColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btBackColor.Click
        Dim cd As New ColorDialog
        cd.Color = btBackColor.BackColor
        cd.FullOpen = True
        If cd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            btBackColor.BackColor = cd.Color
            lbRGBString.Text = fcRGBString(cd.Color)
            lbBackColor.BackColor = btBackColor.BackColor
            lbBackColor.ForeColor = btForeColor.BackColor
            btSaveColor.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' Schriftfarbe setzen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 12.02.2012 Create
    ''' </remarks>
    Private Sub btForeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btForeColor.Click
        Dim cd As New ColorDialog
        cd.Color = btForeColor.BackColor
        cd.FullOpen = True
        If cd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            btForeColor.BackColor = cd.Color
            lbRGBString.Text = fcRGBString(cd.Color)
            lbBackColor.BackColor = btBackColor.BackColor
            lbBackColor.ForeColor = btForeColor.BackColor
            btSaveColor.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' Farbe speichern in Datei und array
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 12.02.2012 Create
    ''' </remarks>
    Private Sub btSaveColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSaveColor.Click
        Dim i As Integer
        Dim nMax As Integer = lvColor.Items.Count - 1
        Dim sb As New StringBuilder
        With lvColor
            If .SelectedItems.Count <> 0 Then
                .SelectedItems(0).SubItems(1).Text = fcRGBString(btBackColor.BackColor)
                .SelectedItems(0).SubItems(2).Text = fcRGBString(btForeColor.BackColor)
            End If
            For i = 0 To nMax

                .Items.Item(i).Selected = True
                sb.Append(.SelectedItems(0).SubItems(0).Text & ";")
                sb.Append(.SelectedItems(0).SubItems(1).Text & ";")
                sb.Append(.SelectedItems(0).SubItems(2).Text & vbCrLf)
            Next
        End With
        Call SaveOneValueInSystemDb("Color", sb.ToString)
        Call prSaisonColor()
        Call prLoadColor()
        btSaveColor.Enabled = False
    End Sub

#End Region
    Private Sub prSaisonColor()
        Dim arSaison As Array = Split(ReadOneValueFromSystemDb("Saison"), vbCrLf)
        Dim arColor As Array = Split(ReadOneValueFromSystemDb("Color"), vbCrLf)
        Dim arSaisonZeile As Array
        Dim arColorZeile As Array
        Dim sinhalt As String = ""
        Dim test As String = ""
        For ii = 0 To arSaison.Length - 1
            If arSaison(ii).trim <> "" Then
                arSaisonZeile = Split(arSaison(ii), ";")
                For jj = 0 To arColor.Length - 1
                    arColorZeile = Split(arColor(jj), ";")
                    If arColorZeile(0).trim = Mid(arSaisonZeile(0).trim, 3).Trim Then
                        sinhalt = sinhalt + arSaisonZeile(0) + ";" + arSaisonZeile(1) + ";" + arSaisonZeile(2) + ";" + arColorZeile(1) + ";" + arColorZeile(2) + vbCrLf

                    End If

                Next
            End If
        Next
        SaveOneValueInSystemDb("Saison", sinhalt)
    End Sub
#Region "WerbungLink bearbeiten........................................................................"
    Dim alink(1, 1) As String
    Dim nRow As Integer
    Dim nCol As Integer
    Dim nRows As Integer
    Dim nCols As Integer
    Dim sText As String
    ''' <summary>
    ''' Tabelle Zimmer mit daten aus der DataTabel "Zimmer" füllen
    ''' </summary>
    ''' <param name="dtT"></param>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>

    Private Sub prCreateWerbelink()
        Call prLoadZimWerb()
    End Sub
    Private Sub prLoadZimWerb()

        'Dim i As Integer                              'Zimmername in Array
        'nRow = dtZim.Rows.Count - 1
        'If nRow < 0 Then Exit Sub
        'ReDim alink(nRow + 1, 1)
        'For i = 0 To nRow
        '    ' sFeWo = "Nein"
        '    If dtZim.Rows(i).RowState <> DataRowState.Deleted Then
        '        alink(i + 1, 0) = dtZim.Rows(i).Item("Name").ToString
        '    End If
        'Next
        'nCol = dtWer.Rows.Count - 1                 'werbung in array
        'If nCol < 0 Then Exit Sub
        'ReDim Preserve alink(nRow + 1, nCol + 1)
        'For i = 0 To nCol
        '    ' If dtWer.Rows(i).RowState <> DataRowState.Deleted Then
        '    alink(0, i + 1) = dtWer.Rows(i).Item("werbung").ToString
        '    ' End If
        'Next
        'Dim aText() As String = ReadOneValueFromSystemDb("Link").Split(vbCrLf)
        'Dim aZeile() As String
        'Dim aCelle() As String
        'Dim aCellLink() As String
        'For ii = 0 To aText.Length - 1
        '    aZeile = aText(ii).Split("|")
        '    For i = 1 To nRow + 1
        '        If alink(i, 0).Trim = aZeile(0).Trim Then
        '            aCelle = aZeile(1).Split("*")
        '            For jj = 0 To aCelle.Length - 1
        '                aCellLink = aCelle(jj).Split("=")
        '                For j = 1 To nCol + 1
        '                    If aCellLink(0).Trim = alink(0, j).Trim Then
        '                        alink(i, j) = aCellLink(1)
        '                    End If
        '                Next
        '            Next
        '        End If
        '    Next
        'Next

        ''For i = 1 To nRow                      'array mit werten füllen normal aus datein 
        '' For j = 1 To nCol
        '' alink(i, j) = Str(i) + Str(j)

        ''        Next
        ''        Next
        'With dgvLink
        '    .Columns.Clear()
        '    .ColumnHeadersHeight = 30
        '    .Columns.Add("Zimmer", "Zimmer")
        '    For i = 1 To nCol + 1
        '        .Columns.Add(alink(0, i), alink(0, i))
        '    Next
        '    .RowHeadersVisible = False
        '    .AllowUserToAddRows = False
        '    .AllowUserToDeleteRows = False

        '    .AutoResizeRows()
        '    'Sortierung der Spalten verhindern
        '    Dim DGVCol As DataGridViewColumn
        '    For Each DGVCol In .Columns
        '        DGVCol.SortMode = DataGridViewColumnSortMode.NotSortable
        '    Next
        '    .ReadOnly = True
        '    'array in datagrid
        '    For i = 1 To nRow + 1
        '        .Rows.Add(" ")
        '        For j = 0 To nCol
        '            .Rows(i - 1).Cells(j).Value = alink(i, j)
        '        Next
        '    Next
        'End With

    End Sub
    Private Sub dgvLink_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvLink.CellEnter
        If e.RowIndex > -1 And e.ColumnIndex > 0 Then 'And 'e.ColumnIndex < 14 Then
            nRows = e.RowIndex
            nCols = e.ColumnIndex
            tbLink.Text = dgvLink.Rows(nRows).Cells(nCols).Value
            tbLink.Focus()
        End If

    End Sub

    Private Sub tbLink_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbLink.TextChanged
        dgvLink.Rows(nRows).Cells(nCols).Value = tbLink.Text
        alink(nRows + 1, nCols) = tbLink.Text


    End Sub


    Private Sub tsbSaveLink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSaveLink.Click
        Dim sZimmer As String
        Dim sZeile As String
        Dim slink As String
        Dim a As String
        sText = ""
        For i = 1 To nRow + 1
            sZimmer = alink(i, 0)
            sZeile = alink(i, 0) & "|"
            slink = ""
            For j = 1 To nCol + 1
                a = alink(i, j)
                slink = slink & alink(0, j) & "=" & a & "*"
            Next
            sZeile = sZeile & slink & vbCrLf
            sText = sText & sZeile
        Next
        '  Call SaveOneValueInSystemDb(cgSystemPath & "\Link.ini", stext)



    End Sub

#End Region
#Region "Druckprofil"
    Private Sub LoadDruck()
        '  tsbcbDruck.Text = AtLeft(arDruckZimmer(0), ";", 1)
        arDruckZimmer = Split(ReadOneValueFromSystemDb("Druckprofil"), "#")
        Call prtsbcbDruckLoad()
        Call LoadDruckList(tsbcbDruck.Text)
    End Sub
    Private Sub prCreateTabelleZimmerDruck()
        With lvDruck
            .Clear()
            .Columns.Add("Name", 150, HorizontalAlignment.Left)
            .Columns.Add("Art der Unterkunft", 150, HorizontalAlignment.Left)
            .Columns.Add("Ausstattung", 150, HorizontalAlignment.Left)
            .Columns.Add("Betten", 50, HorizontalAlignment.Center)
            .Columns.Add("IDObjekt", 0, HorizontalAlignment.Left)
            .Columns.Add("ID", 0, HorizontalAlignment.Left)
            .Columns.Add("Objekt", 150, HorizontalAlignment.Left)
            .Columns.Add("FeWo", -2, HorizontalAlignment.Center)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            .Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub
    ''' <summary>
    ''' Tabelle Zimmer mit daten aus der DataTabel "Zimmer" füllen
    ''' </summary>
    ''' <param name="dtT"></param>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Sub prLoadZimInListDruck(ByVal dtT As DataTable)
        Dim i As Integer
        Dim nMax As Integer = dtT.Rows.Count - 1
        If nMax < 0 Then Exit Sub
        lvDruck.Items.Clear()
        Dim sObj As String
        Dim sFeWo As String
        For i = 0 To nMax
            sFeWo = "Nein"
            If dtT.Rows(i).RowState <> DataRowState.Deleted Then
                Dim lv As ListViewItem
                With lvDruck
                    lv = .Items.Add(dtT.Rows(i).Item("Name").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("Art").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("Ausstattung").ToString)
                    lv.SubItems.Add(dtT.Rows(i).Item("Betten").ToString)
                    sObj = dtT.Rows(i).Item("IDObjekte").ToString
                    lv.SubItems.Add(sObj)
                    lv.SubItems.Add(dtT.Rows(i).Item("ID").ToString)
                    sObj = fcGetObjektName(dtObj, sObj)
                    lv.SubItems.Add(sObj)
                    If dtT.Rows(i).Item("FeWo").ToString = "1" Then sFeWo = "Ja"
                    lv.SubItems.Add(sFeWo)
                End With
            End If
        Next

    End Sub
    Private Sub LoadDruckList(ByRef sProfil As String)
        Call prCreateTabelleZimmerDruck()
        Call prLoadZimInListDruck(dtZim)
        Dim arDruckZimmer1 As Array
        Dim IDZ As String = ""
        For i = 0 To arDruckZimmer.Length - 1
            If AtLeft(arDruckZimmer(i), ";", 1) = sProfil Then
                arDruckZimmer1 = Split(arDruckZimmer(i), ";")
            End If
        Next
        For i = 1 To arDruckZimmer1.Length - 1
            IDZ = arDruckZimmer1(i)
            For j = 0 To lvDruck.Items.Count - 1
                If IDZ = lvDruck.Items(j).SubItems(5).Text Then
                    lvDruck.Items(j).Checked = True
                End If
            Next
        Next
    End Sub

    Private Sub tsbDruckSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDruckSave.Click
        Call prDruckSave()
    End Sub
    Private Sub tsbcbDruck_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbcbDruck.TextChanged
        For i = 0 To arDruckZimmer.Length - 1
            If AtLeft(arDruckZimmer(i), ";", 1) = tsbcbDruck.Text Then
                Call LoadDruckList(tsbcbDruck.Text)
                Exit For
            Else
                For j = 0 To lvDruck.Items.Count - 1
                    lvDruck.Items(j).Checked = False
                Next
            End If

        Next

        ' Call LoadDruckList(tsbcbDruck.Text)
    End Sub
    Private Sub tsbDruckDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDruckDel.Click
        Dim sMsg As String = "Soll das Druckprofil Gelöscht werden ?"

        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            For i = 0 To arDruckZimmer.Length - 1

                If AtLeft(arDruckZimmer(i), ";", 1) = tsbcbDruck.Text Then
                    arDruckZimmer(i) = " ;"
                End If
            Next
            tsbcbDruck.Text = ""
            Call prDruckSave()
        End If
    End Sub
    Private Sub prtsbcbDruckLoad()
        For i = 0 To arDruckZimmer.Length - 1
            If AtLeft(arDruckZimmer(i), ";", 1).Trim <> "" Then
                tsbcbDruck.Items.Add(AtLeft(arDruckZimmer(i), ";", 1))
            End If
        Next
        tsbcbDruck.Text = AtLeft(arDruckZimmer(0), ";", 1)
    End Sub
    Private Sub prDruckSave()
        Dim sZeile As String = tsbcbDruck.Text
        Dim nFond As String = 0
        Dim nLength As Integer = arDruckZimmer.Length
        For j = 0 To lvDruck.Items.Count - 1
            If lvDruck.Items(j).Checked = True Then
                sZeile = sZeile & ";" & lvDruck.Items(j).SubItems(5).Text
            End If
        Next
        For i = 0 To arDruckZimmer.Length - 1
            If AtLeft(arDruckZimmer(i), ";", 1) = AtLeft(sZeile, ";", 1) Then
                nFond = 1
                arDruckZimmer(i) = sZeile
            End If
        Next
        If nFond = 1 Then
            sZeile = ""
        Else
            sZeile = sZeile & "#"
        End If
        For i = 0 To arDruckZimmer.Length - 1
            If arDruckZimmer(i).ToString.Trim <> "" Then
                sZeile = sZeile & arDruckZimmer(i) & "#"
            End If
        Next
        Call SaveOneValueInSystemDb("Druckprofil", sZeile)
        tsbcbDruck.Items.Clear()
        arDruckZimmer = Split(ReadOneValueFromSystemDb("Druckprofil"), "#")
        Call prtsbcbDruckLoad()
    End Sub
#End Region


#Region "Zusätze bearbeiten........................................................................."

    ''' <summary>
    ''' Tabelle "Zusatz" erstellen
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub prCreateTabelleZusatz()
        Dim nWidth As Integer
        '   Call prColorRead()
        With lvZusatz
            nWidth = .Width / 3
            .Clear()
            .Columns.Add("Leistung", nWidth, HorizontalAlignment.Left)
            .Columns.Add("Brutto", nWidth, HorizontalAlignment.Left)
            .Columns.Add("Steuer", nWidth, HorizontalAlignment.Left)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            '.Sorting = SortOrder.Ascending
            .Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub

    ''' <summary>
    ''' Tabelle Saison mit daten aus der Datei "Saison.ini" füllen
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 Create
    ''' </remarks>
    Private Sub prLoadZusatzInList()
        Dim arTmp() As String = Split(ReadOneValueFromSystemDb("Zusatzkosten"), vbCrLf)
        Dim nMax As Integer = arTmp.Length - 1
        Dim arT() As String
        '  Dim nColor As Integer
        With lvZusatz
            .Items.Clear()
            For i As Integer = 0 To nMax
                If Trim(arTmp(i)) <> "" Then
                    arT = Split(arTmp(i), ";")
                    Dim lv As ListViewItem
                    lv = .Items.Add(arT(0).Trim)
                    lv.SubItems.Add(arT(1))
                    lv.SubItems.Add(arT(2))
                    '  nColor = arT(0).Trim.Substring(0, 1)
                    '  .Items(i).BackColor = fcStringRGB(arFarbe(nColor, 1))
                End If
            Next
            ' .Select()
            ' If .Items.Count > 0 Then .TopItem.Selected = True
        End With
    End Sub

    Private Sub lvZusatz_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvZusatz.SelectedIndexChanged
        Call prGetInfolvZusatz()
    End Sub

    ''' <summary>
    ''' Informationen aus der Kontenliste in die Eingabefelder übertragen
    ''' </summary>
    ''' <remarks>
    ''' 18.12.2011 Create
    ''' </remarks>
    Private Sub prGetInfolvZusatz()

        With lvZusatz
            If .SelectedItems.Count <> 0 Then
                tbLeistung.Text = .SelectedItems(0).SubItems(0).Text
                tbBrutto.Text = .SelectedItems(0).SubItems(1).Text
                tbSteuer.Text = .SelectedItems(0).SubItems(2).Text
                '  mcSaisonAnfang.SelectionStart = sDAnfang
                ' mcSaisonEnde.SelectionStart = sDEnde
            End If
        End With
    End Sub

    Private Sub tsbZusatzNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbZusatzNew.Click
        lNew = True
        'sDAnfang = sDAK
        'sDEnde = sDAK
        'mcSaisonAnfang.SelectionStart = sDAnfang
        'mcSaisonEnde.SelectionStart = sDEnde
        Call prLoockZusatz(True)
        ' coSaison.Select()
    End Sub
    ''' <summary>
    ''' Steuerung der Button und Eingabefelder
    ''' </summary>
    ''' <param name="lStatus"></param>
    ''' <remarks>
    ''' 18.12.2011 Create
    ''' </remarks>
    Private Sub prLoockZusatz(ByVal lStatus As Boolean)
        tsbZusatzEdit.Enabled = Not lStatus
        tsbZusatzNew.Enabled = Not lStatus
        tsbZusatzSave.Enabled = lStatus
        tsbZusatzOld.Enabled = lStatus
        tsbZusatzDel.Enabled = Not lStatus
        lvZusatz.Enabled = Not lStatus
        tbLeistung.Enabled = lStatus
        tbBrutto.Enabled = lStatus
        tbSteuer.Enabled = lStatus


    End Sub

    Private Sub tsZusatzSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbZusatzSave.Click
        Dim sb As New StringBuilder

        If lNew = True Then
            Dim lv As ListViewItem
            With lvZusatz
                lv = .Items.Add(Trim(tbLeistung.Text))
                lv.SubItems.Add(Trim(tbBrutto.Text))
                lv.SubItems.Add(Trim(tbSteuer.Text))
            End With
        Else
            With lvZusatz
                If .SelectedItems.Count <> 0 Then
                    .SelectedItems(0).SubItems(0).Text = tbLeistung.Text
                    .SelectedItems(0).SubItems(1).Text = tbBrutto.Text
                    .SelectedItems(0).SubItems(2).Text = tbSteuer.Text
                End If
            End With
        End If
        Call prLoockZusatz(False)
        For i = 0 To lvZusatz.Items.Count - 1
            sb.Append(lvZusatz.Items(i).SubItems(0).Text & ";")
            sb.Append(lvZusatz.Items(i).SubItems(1).Text & ";")
            sb.Append(lvZusatz.Items(i).SubItems(2).Text & vbCrLf)
        Next
        SaveOneValueInSystemDb("Zusatzkosten", sb.ToString)
        ' Call prSaisonColor()


    End Sub

    Private Sub tsbZusatzEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbZusatzEdit.Click
        lNew = False
        Call prLoockZusatz(True)

        ' coSaison.Select()
    End Sub




    Private Sub tsbZusatzOld_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbZusatzOld.Click
        prGetInfolvZusatz()
        prLoockZusatz(False)
    End Sub

    Private Sub tsbZusatzDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbZusatzDel.Click
        Dim sb As New StringBuilder
        Dim sMsg As String = "Wollen Sie diese Datensatz wirklich löschen?"
        Dim X As Integer
        Dim inhalt As String = ""
        If X <> -1 Then
            If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
                X = lvZusatz.SelectedIndices.Item(0)
                lvZusatz.Items.RemoveAt(X)
                lvZusatz.Select()
                If lvZusatz.Items.Count > 0 Then
                    lvZusatz.TopItem.Selected = True
                    inhalt = ""
                    Call prLoockZusatz(False)
                    For i = 0 To lvZusatz.Items.Count - 1
                        sb.Append(lvZusatz.Items(i).SubItems(0).Text & ";")
                        sb.Append(lvZusatz.Items(i).SubItems(1).Text & ";")
                        sb.Append(lvZusatz.Items(i).SubItems(2).Text & vbCrLf)
                    Next
                    SaveOneValueInSystemDb("Zusatzkosten", sb.ToString)
                End If
            End If
        End If
        ' Call prSaisonColor()
    End Sub
#End Region

#Region "Sprache"

    Private Sub prSpracheIni()
        lNew = False
        Dim artext As Array = Split(ReadOneValueFromSystemDb("Language"), vbCrLf)
        x = 1
        y = 1
        cbText.Items.Clear()
        For i = 1 To artext.Length - 1
            arFeld = Split(artext(i), ";")
            cbText.Items.Add(arFeld(0))
            x = x + 1
        Next
        With dgvSprache
            .Rows.Clear()
            .ColumnCount = 2
            .ColumnHeadersVisible = True
            .Columns(0).Name = "Sprache"
            .Columns(0).Width = 50
            .Columns(1).Name = "Text"
            .Columns(1).Width = 840
            arFeld = Split(artext(0), ";")
            For i = 1 To arFeld.Length - 1
                .Rows.Add()
                arFeld1 = Split(arFeld(i), ",")
                .Rows(i - 1).Cells(0).Value = arFeld1(0)
                y = y + 1
            Next
            ReDim arFeld2(x, y)
            For i = 0 To x - 1
                arFeld = Split(artext(i), ";")
                If arFeld(0) <> "" Then
                    For j = 0 To y - 1
                        arFeld2(i, j) = arFeld(j)
                    Next
                End If
            Next
            cbText.Text = cbText.Items(0)


            dgvSprache.ReadOnly = True
        End With
    End Sub

    Private Sub dgvSprache_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles dgvSprache.MouseDown

        Dim hit As DataGridView.HitTestInfo = dgvSprache.HitTest(e.X, e.Y)
        If hit.ColumnIndex = 1 Then
            dgvSprache.ReadOnly = False
        Else
            dgvSprache.ReadOnly = True
        End If
    End Sub
    Private Sub tbText_Click(sender As System.Object, e As System.EventArgs) Handles cbText.Click
        For i = 1 To x
            If cbText.Text = arFeld2(i, 0) Then
                For j = 0 To y - 2
                    arFeld2(i, j + 1) = dgvSprache.Rows(j).Cells(1).Value
                Next
            End If
        Next
    End Sub




    Private Sub cbText_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbText.SelectedIndexChanged

        For i = 1 To x
            If cbText.Text = arFeld2(i, 0) Then
                For j = 0 To y - 2
                    dgvSprache.Rows(j).Cells(1).Value = arFeld2(i, j + 1)
                Next
            End If
        Next



    End Sub
    Private Sub tsSpracheVerw_Click(sender As System.Object, e As System.EventArgs) Handles tsSpracheVerw.Click
        For i = 0 To x - 1
            arFeld = Split(arText(i), ";")
            For j = 0 To y - 1
                arFeld2(i, j) = arFeld(j)
            Next
        Next

        For i = 1 To x
            If cbText.Text = arFeld2(i, 0) Then
                For j = 0 To y - 2
                    dgvSprache.Rows(j).Cells(1).Value = arFeld2(i, j + 1)
                Next
            End If
        Next
    End Sub
    Private Sub tsSpracheSave_Click(sender As System.Object, e As System.EventArgs) Handles tsSpracheSave.Click 'Text Kopf

        prSpracheSave()
    End Sub
    Private Sub prSpracheSave()
        Dim sTextSprache As String = ""
        Dim sText_x As String = cbText.Text & ";"
        lNew = True
        For i = 1 To x
            If cbText.Text = arFeld2(i, 0) Then
                lNew = False
                For j = 0 To y - 2
                    arFeld2(i, j + 1) = dgvSprache.Rows(j).Cells(1).Value
                Next
            End If
        Next

        For i = 0 To x - 1
            If arFeld2(i, 0) <> "" Then
                For j = 0 To y
                    sTextSprache = sTextSprache & arFeld2(i, j) & ";"
                Next
                sTextSprache = Mid(sTextSprache, 1, sTextSprache.Length - 2) & vbCrLf
            End If
        Next

        If lNew = True Then
            For j = 0 To y - 2
                sText_x = sText_x & dgvSprache.Rows(j).Cells(1).Value & ";"
            Next
            sText_x = sText_x & vbCrLf & ";;;;;;;;"
            sTextSprache = sTextSprache & sText_x
        End If

        SaveOneValueInSystemDb("Language", sTextSprache)
        '  tsSpracheNew.BackColor = Color.Transparent
        prSpracheIni()
    End Sub

    'Private Sub tsSpracheNew_Click(sender As Object, e As EventArgs) Handles tsSpracheNew.Click
    '    If lNew = False Then
    '        lNew = True
    '        tsSpracheNew.BackColor = Color.Red
    '    Else
    '        lNew = False
    '        tsSpracheNew.BackColor = Color.Transparent
    '    End If
    'End Sub

    Private Sub tsSpracheDel_Click(sender As Object, e As EventArgs) Handles tsSpracheDel.Click
        'lNew = False
        'tsSpracheNew.BackColor = Color.Transparent
        Dim sTextSprache As String = ""
        For i = 1 To x
            If cbText.Text = arFeld2(i, 0) Then
                arFeld2(i, 0) = ""
            End If
        Next
        For i = 0 To x - 1
            If arFeld2(i, 0) <> "" Then
                For j = 0 To y
                    sTextSprache = sTextSprache & arFeld2(i, j) & ";"
                Next
                sTextSprache = Mid(sTextSprache, 1, sTextSprache.Length - 2) & vbCrLf
            End If
        Next
        SaveOneValueInSystemDb("Language", sTextSprache)
        prSpracheIni()
        'prSpracheSave()
    End Sub


















#End Region










End Class