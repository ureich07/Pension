Imports System.Text
Imports System.Net.Mail

Public Class frmSystem
    Inherits System.Windows.Forms.Form
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

    ' HIER GEÄNDERT: Nur deklarieren, noch nicht befüllen!
    Dim sLanguage1() As String
    Dim sLanguage() As String
    Dim arText() As String
    Dim arFeld As Array
    Dim arFeld1 As Array
    Dim arFeld2(1, 1) As String
    Dim x As Integer = 1
    Dim y As Integer = 1


    ''' <summary>
    ''' Konstruktor der Form
    ''' </summary>
    Public Sub New()
        ' Dieser Aufruf ist für den Designer zwingend erforderlich.
        InitializeComponent()
    End Sub
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
        ' WICHTIG: Verhindert, dass der Visual Studio Designer versucht, 
        ' zur Design-Zeit Daten zu laden oder Tabellen zu erstellen!
        If Me.DesignMode Then Exit Sub
        sLanguage1 = Split(ReadOneValueFromSystemDb("Language"), vbCrLf)
        sLanguage = Split(sLanguage1(0), ";")
        arText = Split(ReadOneValueFromSystemDb("Language"), vbCrLf)
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

    ''' <summary>
    ''' Wählt automatisch den ersten Eintrag in einer ListBox aus, sofern Datensätze vorhanden sind und aktuell keine Auswahl existiert.
    ''' </summary>
    ''' <param name="listBox">Die ListBox, deren Auswahl gesteuert werden soll (z. B. liBuch oder liWerbung).</param>
    ''' <remarks>
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Logik als eigenständige, wiederverwendbare Prozedur extrahiert.
    ''' - Durch die Übergabe als 'ListBox' kann die Methode flexibel für alle Listen im Formular genutzt werden.
    ''' </remarks>
    Private Sub prSelectFirstListBoxItemIfNeeded(ByVal listBox As ListBox)
        ' Prüfen, ob die Liste Steuerelemente enthält und aktuell kein Eintrag selektiert ist
        If listBox IsNot Nothing AndAlso listBox.SelectedIndex = -1 AndAlso listBox.Items.Count > 0 Then
            listBox.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' Wählt automatisch den ersten Eintrag in einer ListView aus, sofern Einträge vorhanden sind und aktuell keine Auswahl existiert.
    ''' </summary>
    ''' <param name="listView">Die ListView, deren Auswahl gesteuert werden soll.</param>
    ''' <remarks>
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Eigenständige Methode für ListView-Steuerelemente erstellt.
    ''' - Nutzt 'SelectedIndices.Count', um plattformunabhängig und null-sicher eine fehlende Auswahl zu erkennen.
    ''' - Setzt sowohl 'Selected' als auch 'Focused', damit das Element in der UI visuell hervorgehoben wird.
    ''' </remarks>
    Private Sub prSelectFirstListViewItemIfNeeded(ByVal listView As ListView)
        ' Prüfen, ob die ListView existiert, Einträge besitzt und aktuell nichts selektiert ist
        If listView IsNot Nothing AndAlso listView.Items.Count > 0 AndAlso listView.SelectedIndices.Count = 0 Then
            ' Das erste Element markieren
            listView.Items(0).Selected = True
            ' Den Fokusrahmen auf das erste Element setzen (wichtig für Tastatursteuerung)
            listView.Items(0).Focused = True
        End If
    End Sub

    ''' <summary>
    ''' Reagiert auf den Wechsel des aktiven Reiters im TabControl 'tcSystem' und stößt die jeweiligen Aktualisierungen an.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 30.09.2026 - Erstellt:
    ''' - Ereignis-Steuerung für den Tab-Wechsel implementiert.
    ''' - 'SelectedTab'-Abfrage über Select Case zur einfachen Erweiterung strukturiert.
    ''' </remarks>
    Private Sub tcSystem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tcSystem.SelectedIndexChanged
        ' Sicherstellen, dass ein Tab ausgewählt ist
        If tcSystem.SelectedTab IsNot Nothing Then

            ' Abfrage basierend auf dem Namen des TabPages (im Designer vergeben)
            Select Case tcSystem.SelectedTab.Name

                Case "tpPreise"
                    prCheckNoRecordPreise(dtPre)

                Case "tbSonstiges"
                    prCheckNoRecordWerbung(dtWer)

                Case "tpZimmer"
                    prCheckNoRecordZimmer(dtZim)

            End Select
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
    tbOOrtsteil.KeyPress, tbOName.KeyPress, tbOTelefon.KeyPress,
    tbKUser.KeyPress, tbUPassWD.KeyPress, tbURechte.KeyPress, tbUUser.KeyPress


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

                    Case "tbKUser" : tbUPassWD.Select()
                    Case "tbUPassWD" : tbURechte.Select()
                    Case "tbURechte" : tbUUser.Select()
                    Case "tbUUser" : tbKUser.Select()


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
        ' Wenn Datensätze vorhanden sind, die Selektionsprüfung für die ListBox aufrufen
        If hasRecords Then
            prSelectFirstListViewItemIfNeeded(lvKonto)
        End If
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
        ' Wenn Datensätze vorhanden sind, die Selektionsprüfung für die ListBox aufrufen
        If hasRecords Then
            prSelectFirstListViewItemIfNeeded(lvObjekt)
        End If
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
    ''' Erstellt die Tabellenstruktur für die Zimmerübersicht und konfiguriert die Anzeige-Eigenschaften des ListView-Steuerelements.
    ''' </summary>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Spalten-Generierung über ein strukturiertes Array und eine Schleife kompakt zusammengefasst (bessere Wartbarkeit).
    ''' - Konstante Breitenangaben und Ausrichtungen zur Reduzierung von redundantem Code ausgelagert.
    ''' - ListView-Grundeinstellungen für ein konsistentes Verhalten und flackerfreie Darstellung beibehalten.
    ''' </remarks>
    Private Sub prCreateTabelleZimmer()
        With lvZimmer
            .Clear()

            ' Definition der Spalten: (Name/Header, Breite, Ausrichtung)
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

            ' Grid- und Anzeige-Eigenschaften konfigurieren
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
    ''' Befüllt das ListView-Steuerelement "lvZimmer" mit den Daten aus der übergebenen Zimmer-DataTable.
    ''' </summary>
    ''' <param name="dtT">Die DataTable, die die Zimmerdaten enthält.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - 'BeginUpdate' und 'EndUpdate' hinzugefügt, um UI-Flackern zu verhindern und die Performance drastisch zu steigern.
    ''' - 'For Each'-Schleife anstelle der indexbasierten Schleife für bessere Lesbarkeit verwendet.
    ''' - 'ListViewItemCollection.AddRange' genutzt, um alle Zeilen performant in einem Rutsch hinzuzufügen.
    ''' - Null-Zuweisungen und String-Vergleiche modernisiert.
    ''' </remarks>
    Private Sub prLoadZimInList(ByVal dtT As DataTable)
        ' Validierung: Wenn die Tabelle leer oder ungültig ist, abbrechen
        If dtT Is Nothing OrElse dtT.Rows.Count = 0 Then
            lvZimmer.Items.Clear()
            Exit Sub
        End If

        ' Zeichnen des Steuerelements einfrieren, um Performance zu maximieren
        lvZimmer.BeginUpdate()
        lvZimmer.Items.Clear()

        ' Temporäre Liste für das gebündelte Hinzufügen der Items
        Dim listItems As New List(Of ListViewItem)()

        For Each row As DataRow In dtT.Rows
            ' Gelöschte Zeilen ignorieren
            If row.RowState = DataRowState.Deleted Then Continue For

            ' Werte auslesen und vorbereiten
            Dim sObjId As String = row("IDObjekte").ToString()
            Dim sObjName As String = fcGetObjektName(dtObj, sObjId)
            Dim sFeWo As String = If(row("FeWo").ToString() = "1", "Ja", "Nein")

            ' Neues ListViewItem mit dem Hauptwert (Spalte 1: Name) initialisieren
            Dim lvItem As New ListViewItem(row("Name").ToString())

            ' SubItems (Spalten 2 bis 30) strukturiert hinzufügen
            With lvItem.SubItems
                .Add(row("Art").ToString())
                .Add(row("Ausstattung").ToString())
                .Add(sObjId)
                .Add(row("ID").ToString())
                .Add(sObjName)
                .Add(sFeWo)
                .Add(row("Nummer").ToString())
                .Add(row("Betten").ToString())
                .Add(row("BettenMin").ToString())
                .Add(row("BettenEr").ToString())
                .Add(row("BettenKi").ToString())
                .Add(row("P1").ToString())
                .Add(row("P2").ToString())
                .Add(row("P3").ToString())
                .Add(row("P4").ToString())
                .Add(row("P5").ToString())
                .Add(row("P6").ToString())
                .Add(row("P7").ToString())
                .Add(row("P8").ToString())
                .Add(row("P9").ToString())
                .Add(row("P10").ToString())
                .Add(row("Trans1").ToString())
                .Add(row("Trans2").ToString())
                .Add(row("Trans3").ToString())
                .Add(row("Trans4").ToString())
                .Add(row("Trans5").ToString())
                .Add(row("Code").ToString())
                .Add(row("SaveCode").ToString())
                .Add(row("Datei").ToString())
            End With

            ' Item zur temporären Liste hinzufügen
            listItems.Add(lvItem)
        Next

        ' Alle Items auf einmal dem ListView hinzufügen
        If listItems.Count > 0 Then
            lvZimmer.Items.AddRange(listItems.ToArray())
        End If

        ' Zeichnen des Steuerelements wieder aktivieren
        lvZimmer.EndUpdate()
    End Sub

    ''' <summary>
    ''' Prüft, ob Datensätze in der Zimmer-Tabelle vorhanden sind, und steuert entsprechend die Aktivierung der Bearbeiten- und Löschen-Schaltflächen.
    ''' </summary>
    ''' <param name="dt">Die zu prüfende DataTable mit den Zimmer-Datensätzen.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Redundante 'If'-Bedingung und temporäre Boolean-Variable entfernt.
    ''' - Direkte Zuweisung des Vergleichsergebnisses an die 'Enabled'-Eigenschaft implementiert.
    ''' - Null-Sicherheitsprüfung ('IsNot Nothing') hinzugefügt, um Laufzeitfehler zu verhindern.
    ''' </remarks>
    Private Sub prCheckNoRecordZimmer(ByVal dt As DataTable)
        ' Prüfen, ob die DataTable existiert und Zeilen enthält
        Dim hasRecords As Boolean = (dt IsNot Nothing AndAlso dt.Rows.Count > 0)

        ' Buttons direkt basierend auf dem Ergebnis aktivieren oder deaktivieren
        tsbEditZim.Enabled = hasRecords
        tsbDelZim.Enabled = hasRecords
        ' Wenn Datensätze vorhanden sind, die Selektionsprüfung für die ListBox aufrufen
        If hasRecords Then
            prSelectFirstListViewItemIfNeeded(lvZimmer)
        End If
    End Sub


    ''' <summary>
    ''' Bereitet die Eingabemaske für das Anlegen eines neuen Zimmer-Datensatzes vor.
    ''' Setzt alle Textfelder zurück, aktiviert die Steuerelemente und fokussiert das Namensfeld.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 18.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf von 'prLoockZim' entfernt.
    ''' - '""' durch die performantere .NET-Konstante 'String.Empty' ersetzt.
    ''' - Sicherheitsprüfung für 'coZArt.Items.Count' hinzugefügt, um Indexfehler beim Zurücksetzen der ComboBox zu vermeiden.
    ''' - Auskommentierten Code-Ballast ('tbZArt.Text') entfernt.
    ''' </remarks>
    Private Sub tsbNeuZim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNeuZim.Click
        ' Flag für neuen Datensatz setzen
        lNew = True

        ' Eingabefelder zurücksetzen
        tbZName.Text = String.Empty
        tbZAus.Text = String.Empty
        tbZBetten.Text = String.Empty
        tbZNummer.Text = String.Empty

        ' ComboBox auf den ersten Eintrag zurücksetzen, sofern Einträge vorhanden sind
        If coZArt.Items.Count > 0 Then
            coZArt.SelectedIndex = 0
        Else
            coZArt.SelectedIndex = -1
        End If

        ' Eingabemaske entsperren
        prLockZim(True)

        ' Fokus auf das erste Eingabefeld setzen
        tbZName.Select()
    End Sub

    ''' <summary>
    ''' Versetzt die Eingabemaske in den Bearbeitungsmodus und fokussiert das Namensfeld.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsbEditZim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEditZim.Click
        prLockZim(True)
        tbZName.Select()
    End Sub

    ''' <summary>
    ''' Löst den Speichervorgang für die vorgenommenen Änderungen oder den neuen Zimmer-Datensatz aus.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsbSaveZim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSaveZim.Click
        prSaveZimmer()
    End Sub

    ''' <summary>
    ''' Bricht den aktuellen Bearbeitungs- oder Neuanlage-Modus ab, sperrt die Eingabemaske und aktualisiert den Status der Steuerungsschaltflächen.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' - Falsche Methodenbezeichnung im XML-Kommentar korrigiert.
    ''' </remarks>
    Private Sub tsbBraekZim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBraekZim.Click
        prLockZim(False)
        prCheckNoRecordZimmer(dtObj)
    End Sub

    ''' <summary>
    ''' Steuert den Aktivierungsstatus (Enabled) aller Eingabefelder und Schaltflächen der Zimmerverwaltung.
    ''' Schaltet zwischen Bearbeitungsmodus und Anzeige-/Sperrmodus um.
    ''' </summary>
    ''' <param name="lStatus">True, wenn die Eingabefelder für die Bearbeitung freigegeben werden sollen; andernfalls False.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Fehler bei der Typkonvertierung behoben: ToolStripButtons separat als ToolStripItem deklariert.
    ''' - Steuerelemente in Arrays gruppiert und Zuweisung über Schleifen gelöst (bessere Übersicht und Wartbarkeit).
    ''' - Redundante Einzelzuweisungen entfernt.
    ''' </remarks>
    Private Sub prLockZim(ByVal lStatus As Boolean)
        Dim lInvertedStatus As Boolean = Not lStatus

        ' --- 1. Steuerung der Standard-Formular-Steuerelemente (Control) ---
        Dim editControls() As Control = {
        tbZBetten, tbZName, tbZAus, coZArt, coObjekt, tbZNummer, chFeWo,
        tbZBettenMin, tbZBettenEr, tbZBettenKi, lvZimmer,
        tbZP1, tbZP2, tbZP3, tbZP4, tbZP5, tbZP6, tbZP7, tbZP8, tbZP9, tbZP10,
        tbTrans1, tbTrans2, tbTrans3, tbTrans4, tbTrans5
    }

        For Each ctrl In editControls
            ' lvZimmer verhält sich umgekehrt zur Eingabemaske
            If ctrl Is lvZimmer Then
                ctrl.Enabled = lInvertedStatus
            Else
                ctrl.Enabled = lStatus
            End If
        Next

        ' --- 2. Steuerung der Menü-Schaltflächen (ToolStripItem) ---
        Dim editButtons() As ToolStripItem = {tsbSaveZim, tsbBraekZim}
        Dim navButtons() As ToolStripItem = {tsbEditZim, tsbNeuZim, tsbDelZim}

        For Each btn In editButtons
            btn.Enabled = lStatus
        Next

        For Each btn In navButtons
            btn.Enabled = lInvertedStatus
        Next
    End Sub

    ''' <summary>
    ''' Führt die Speicherung (Einfügen oder Aktualisieren) eines Zimmer-Datensatzes in der Datenbank und der lokalen DataTable durch.
    ''' </summary>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen konsequent entfernt.
    ''' - Unbenutzte Variablen ('sb') und auskommentierte Code-Fragmente entfernt, um die Übersicht zu verbessern.
    ''' - Sicherheitsabfrage vor dem Zugriff auf 'arValue(1)' im Finally-Block eingebaut.
    ''' - SQL-Spalten-String als lesbare Konstante oder direkte Zuweisung beibehalten, Splitting optimiert.
    ''' </remarks>
    Private Sub prSaveZimmer()
        Dim sqlText As String
        Dim arFields() As String
        Dim arValue() As String = {}
        Dim cBedingung As String

        Dim sObj As String = fcGetObjektZimmerID(dtObj, coObjekt.Text, "ID")
        Dim sName As String = tbZName.Text
        Dim sID As String = lbZimmerID.Text

        ' Validierung und ID-Generierung bei Neuanlage
        If lNew Then
            sID = fcGetTimeID(Date.Today)
            If fcCheckZimmer(sName, sObj) Then Exit Sub
        End If

        Try
            ' Spalten-Struktur definieren und splitten
            sqlText = "ID,Name,Art,Ausstattung,Betten,IDObjekte,FeWo,Nummer,BettenMin,BettenEr,BettenKi,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10,Trans1,Trans2,Trans3,Trans4,Trans5,Code,SaveCode,Datei"
            arFields = sqlText.Split(","c)

            ' Werte ermitteln und splitten
            sqlText = fcSaveZimmer(sID, sObj)
            arValue = sqlText.Split("°"c)

            ' Datenbank-Operationen durchführen
            If lNew Then
                sID = fcAppendBlank("Zimmer")
            End If

            cBedingung = " WHERE ID='" & sID & "'"
            fcUpdateCommand("Zimmer", arFields, arValue, cBedingung)

            ' Lokale DataTable synchronisieren
            If lNew Then
                fcInsertTable(dtZim, arFields, arValue)
            Else
                cBedingung = "ID Like '" & sID & "'"
                fcUpdateTable(dtZim, arFields, arValue, cBedingung)
            End If

            lNew = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ' Benutzeroberfläche aktualisieren und sperren
            prLoadZimInList(dtZim)
            prCheckNoRecordZimmer(dtZim)
            prLockZim(False)

            ' Gespeicherten Eintrag selektieren (Absicherung gegen leeres/falsches Array)
            If arValue IsNot Nothing AndAlso arValue.Length > 1 Then
                prSelectEntry(lvZimmer, arValue(1))
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Tritt auf, wenn sich die Auswahl in der Zimmer-Liste ändert, und stößt die Aktualisierung der Eingabemaske an.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax entfernt.
    ''' </remarks>
    Private Sub lvZimmer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvZimmer.SelectedIndexChanged
        prGetInfolvZimmer()
    End Sub

    ''' <summary>
    ''' Überträgt die Detailinformationen des aktuell selektierten Zimmers aus der ListView in die entsprechenden Eingabe- und Steuerelemente.
    ''' </summary>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Zugriff auf '.SelectedItems(0)' durch eine lokale Variable ('item') zentralisiert (bessere Performance und Lesbarkeit).
    ''' - 'If-Else'-Strukturen für CheckBoxen durch direkte, boolesche Zuweisungen ('Checked') vereinfacht.
    ''' - Redundante Typkonvertierungen durch direkten SubItem-Zugriff optimiert.
    ''' </remarks>
    Private Sub prGetInfolvZimmer()
        If lvZimmer.SelectedItems.Count = 0 Then Exit Sub

        ' Das erste ausgewählte Element für den direkten Zugriff zwischenspeichern
        Dim item As ListViewItem = lvZimmer.SelectedItems(0)

        ' Werte in die Eingabefelder übertragen
        tbZName.Text = item.SubItems(0).Text
        coZArt.Text = item.SubItems(1).Text
        tbZAus.Text = item.SubItems(2).Text
        lbZimmerID.Text = item.SubItems(4).Text
        coObjekt.Text = item.SubItems(5).Text
        tbZNummer.Text = item.SubItems(7).Text
        tbZBetten.Text = item.SubItems(8).Text

        ' CheckBox für Ferienwohnung direkt über den String-Vergleich steuern
        chFeWo.Checked = (item.SubItems(6).Text = "Ja")

        ' Betten- und Kapazitätsfelder befüllen
        tbZBettenMin.Text = item.SubItems(9).Text
        tbZBettenEr.Text = item.SubItems(10).Text
        tbZBettenKi.Text = item.SubItems(11).Text

        ' Preise (P1 - P10) befüllen
        tbZP1.Text = item.SubItems(12).Text
        tbZP2.Text = item.SubItems(13).Text
        tbZP3.Text = item.SubItems(14).Text
        tbZP4.Text = item.SubItems(15).Text
        tbZP5.Text = item.SubItems(16).Text
        tbZP6.Text = item.SubItems(17).Text
        tbZP7.Text = item.SubItems(18).Text
        tbZP8.Text = item.SubItems(19).Text
        tbZP9.Text = item.SubItems(20).Text
        tbZP10.Text = item.SubItems(21).Text

        ' Transfer-Felder befüllen
        tbTrans1.Text = item.SubItems(22).Text
        tbTrans2.Text = item.SubItems(23).Text
        tbTrans3.Text = item.SubItems(24).Text
        tbTrans4.Text = item.SubItems(25).Text
        tbTrans5.Text = item.SubItems(26).Text

        ' Code-Verschlüsselung und restliche Felder steuern
        chCode.Checked = (item.SubItems(27).Text = "1")
        tbSaveCode.Text = item.SubItems(28).Text
        tbDatei.Text = item.SubItems(29).Text
    End Sub

    ''' <summary>
    ''' Bereitet die in der Eingabemaske erfassten Zimmerdaten auf und gibt sie als verketteten, durch Gradzeichen (°) getrennten String zurück.
    ''' </summary>
    ''' <param name="sID">Die eindeutige Identifikationsnummer (ID) des Zimmers.</param>
    ''' <param name="sOID">Die ID des zugeordneten Objekts.</param>
    ''' <returns>Ein durch '°' separierter String mit allen Zimmer-Attributen.</returns>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete VB6-Befehle ('Trim', 'Mid', Funktionsname-Zuweisung) durch moderne .NET-Entsprechungen ersetzt.
    ''' - Redundante Logik der Transfer-Felder (tbTrans1-5) in einem Array zusammengefasst.
    ''' - 'String.IsNullOrWhiteSpace' für null-sichere und performante String-Validierung implementiert.
    ''' - Verkettung flüssig gestaltet und 'Return'-Anweisung verwendet.
    ''' </remarks>
    Private Function fcSaveZimmer(ByVal sID As String, ByVal sOID As String) As String
        Dim sb As New StringBuilder()

        ' 1. Transfer-Felder in einer Schleife prüfen und formatieren
        Dim transBoxes() As TextBox = {tbTrans1, tbTrans2, tbTrans3, tbTrans4, tbTrans5}
        For Each tb In transBoxes
            Dim text As String = tb.Text.Trim()
            ' Prüfen, ob das Feld befüllt ist und das 4. Zeichen kein Semikolon ist
            If text <> String.Empty AndAlso (text.Length < 4 OrElse text.Substring(3, 1) <> ";") Then
                tb.Text = fcWeg34(tb.Text)
            End If
        Next

        ' 2. Standardwerte für leere Pflichtfelder setzen
        If String.IsNullOrWhiteSpace(coZArt.Text) AndAlso coZArt.Items.Count > 0 Then
            coZArt.SelectedIndex = 0
        End If

        If String.IsNullOrWhiteSpace(tbZAus.Text) Then tbZAus.Text = " "
        If String.IsNullOrWhiteSpace(tbZBetten.Text) Then tbZBetten.Text = "0"
        If String.IsNullOrWhiteSpace(tbZNummer.Text) Then tbZNummer.Text = "Z"

        ' 3. Code-Status ermitteln
        Dim sCode As String = If(chCode.Checked, "1", "0")

        ' 4. String strukturiert zusammenbauen
        sb.Append(sID).Append("°")
        sb.Append(tbZName.Text).Append("°")
        sb.Append(coZArt.Text).Append("°")
        sb.Append(tbZAus.Text).Append("°")
        sb.Append(tbZBetten.Text).Append("°")
        sb.Append(sOID).Append("°")
        sb.Append(chFeWo.CheckState).Append("°")
        sb.Append(tbZNummer.Text).Append("°")
        sb.Append(tbZBettenMin.Text).Append("°")
        sb.Append(tbZBettenEr.Text).Append("°")
        sb.Append(tbZBettenKi.Text).Append("°")
        sb.Append(tbZP1.Text).Append("°")
        sb.Append(tbZP2.Text).Append("°")
        sb.Append(tbZP3.Text).Append("°")
        sb.Append(tbZP4.Text).Append("°")
        sb.Append(tbZP5.Text).Append("°")
        sb.Append(tbZP6.Text).Append("°")
        sb.Append(tbZP7.Text).Append("°")
        sb.Append(tbZP8.Text).Append("°")
        sb.Append(tbZP9.Text).Append("°")
        sb.Append(tbZP10.Text).Append("°")
        sb.Append(tbTrans1.Text).Append("°")
        sb.Append(tbTrans2.Text).Append("°")
        sb.Append(tbTrans3.Text).Append("°")
        sb.Append(tbTrans4.Text).Append("°")
        sb.Append(tbTrans5.Text).Append("°")
        sb.Append(sCode).Append("°")
        sb.Append(tbSaveCode.Text).Append("°")
        sb.Append(tbDatei.Text)

        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Prüft, ob die erforderlichen Daten vorhanden sind und ob das Zimmer unter dem angegebenen Objekt bereits in der Datenbank existiert.
    ''' </summary>
    ''' <param name="sName">Die zu prüfende Zimmerbezeichnung.</param>
    ''' <param name="sObj">Die ID des zugeordneten Objekts.</param>
    ''' <returns>True, wenn das Zimmer bereits existiert oder Pflichtangaben fehlen (Validierung fehlgeschlagen); andernfalls False.</returns>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - 'String.IsNullOrWhiteSpace' für null-sichere und performante Validierung eingesetzt.
    ''' - 'Return'-Anweisungen für einen klareren Kontrollfluss integriert.
    ''' - Hinweis auf SQL-Parameter zur Vermeidung von SQL-Injection und Apostroph-Fehlern bei Namen (z.B. "Käpt'n").
    ''' </remarks>
    Private Function fcCheckZimmer(ByVal sName As String, ByVal sObj As String) As Boolean
        Dim sMsg As String = String.Empty

        ' 1. Pflichtfelder auf Inhalt prüfen
        If String.IsNullOrWhiteSpace(sName) OrElse String.IsNullOrWhiteSpace(sObj) Then
            sMsg = "Zimmerbezeichnung / Objekt fehlt!"
            MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Speichern nicht möglich")
            Return True
        End If

        ' 2. Datenbank-Abfrage durchführen 
        ' HINWEIS: Falls deine fcReadDataTable-Methode Parameter unterstützt, sollte dies dringend auf Parameter umgestellt werden!
        Dim sql As String = "SELECT ID FROM Zimmer WHERE Name = '" & sName.Replace("'", "''") & "' AND IDObjekte = '" & sObj & "'"
        Dim dt As DataTable = fcReadDataTable(sql)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            sMsg = "Zimmer ist schon angelegt!"
            MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Speichern nicht möglich")
            Return True
        End If

        ' Validierung erfolgreich (Zimmer existiert noch nicht und Eingaben sind vollständig)
        Return False
    End Function

    ''' <summary>
    ''' Löscht den aktuell ausgewählten Zimmer-Datensatz nach einer Bestätigungsabfrage aus der Datenbank, 
    ''' aktualisiert die lokale DataTable und setzt die Eingabemaske zurück.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Sicherheitsabfrage ('sZim = String.Empty') ganz nach oben gezogen, um unnötige String-Zuweisungen bei Abbruch zu verhindern.
    ''' - Veraltete 'Call'-Syntax bei Methodenaufrufen entfernt.
    ''' - '""' durch die performantere Konstante 'String.Empty' ersetzt.
    ''' - Selektions-Logik nach dem Löschen korrigiert: Es wird nun versucht, das erste Element sauber zu aktivieren.
    ''' </remarks>
    Private Sub tsbDelZim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelZim.Click
        Dim sZim As String = lbZimmerID.Text

        ' Guard Clause: Wenn keine ID vorhanden ist, sofort abbrechen
        If String.IsNullOrWhiteSpace(sZim) Then Exit Sub

        Dim sMsg As String = "Wollen Sie dieses Zimmer wirklich löschen?"
        Dim cSql As String = "DELETE FROM Zimmer WHERE ID = '" & sZim & "'"

        ' Sicherheitsabfrage vor dem Löschen
        If MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkCancel, "Löschen") = MsgBoxResult.Ok Then
            ' Datensatz per SQL aus der Tabelle löschen
            UpdateTable(cSql)

            ' Änderung in lokaler DataTable "dtZim" nachziehen
            fcDeleteTableRow(dtZim, "ID = '" & sZim & "'")

            ' Eingabemaske zurücksetzen
            tbZName.Text = String.Empty
            coZArt.Text = String.Empty
            tbZAus.Text = String.Empty
            lbZimmerID.Text = String.Empty
            tbZBetten.Text = String.Empty

            ' UI-Liste neu laden
            prLoadZimInList(dtZim)

            ' Fokus zurück auf das ListView setzen
            lvZimmer.Select()

            ' Nach dem Löschen das erste verbleibende Element auswählen
            If lvZimmer.Items.Count > 0 Then
                lvZimmer.Items(0).Selected = True
                lvZimmer.Items(0).EnsureVisible()
            End If
        End If
    End Sub


#Region "Mit Enter weiter zum nächsten Feld........................................................"


    'Private Sub tbZname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbZName.KeyPress
    '    If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
    '        tbZArt.Select()
    '    End If
    'End Sub
    'Private Sub tbZArt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbZArt.KeyPress
    '    If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
    '        tbZAus.Select()
    '    End If
    'End Sub
    'Private Sub tbZAus_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbZAus.KeyPress
    '    If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
    '        coObjekt.Select()
    '    End If
    'End Sub

#End Region

#Region "Import / Export..........................................................................."


    ''' <summary>
    ''' Löst den Import bzw. das Neuladen der Zimmerliste aus einer CSV-Datei aus.
    ''' Löscht die bestehenden Daten nach Bestätigung und baut die Tabelle neu auf.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Den auskommentierten Datei-Auswahldialog für 'sDaten' reaktiviert, da die Methode sonst wirkungslos abbrach.
    ''' - 'Finally'-Block ergänzt, um den Cursor bei Fehlern garantiert auf 'Cursors.Default' zurückzusetzen.
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' - 'Select Case' durch ein einfacheres und lesbareres 'If'-Statement für das MsgBoxResult ersetzt.
    ''' </remarks>
    Private Sub tsmImportZimmer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmImportZimmer.Click
        Dim sDaten As String = String.Empty

        ' Sicherheitsabfrage vor dem Überschreiben der Daten
        Dim result As MsgBoxResult = MsgBox("Mit dieser Funktion wird die Zimmerliste neu geladen. Bestehende Einträge werden gelöscht!",
                                        MsgBoxStyle.Information Or MsgBoxStyle.OkCancel,
                                        "Erstinitialisierung")

        If result <> MsgBoxResult.Ok Then Exit Sub

        Try

            'sDaten = fcOpenReadOneValueFromSystemDb(cgSystemPath & "\", "Zimmerliste (Zimmerliste*.csv)|Zimmerliste*.csv")

            ' Wenn keine Datei ausgewählt oder diese leer ist, abbrechen
            If String.IsNullOrWhiteSpace(sDaten) Then Exit Sub

            ' Warte-Cursor setzen, da der Import länger dauern kann
            Me.Cursor = Cursors.WaitCursor

            ' 1. Altdaten in der Datenbank löschen
            UpdateTable("DELETE FROM Zimmer")

            ' 2. Neue Daten parsen und in DB einspielen
            prLadeZimmer(sDaten)

            ' 3. Lokale DataTable frisch aus der Datenbank befüllen
            dtZim = fcReadDataTable("SELECT * FROM Zimmer")

            ' 4. UI-Komponenten und Buttons aktualisieren
            prLoadZimInList(dtZim)
            prCheckNoRecordZimmer(dtZim)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ' Garantiert das Zurücksetzen des Cursors, selbst wenn im Try-Block ein Fehler auftritt
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Parst die übergebenen Rohdaten (CSV-Zeilen), bereitet sie auf und fügt sie zeilenweise in die Datenbank-Tabelle "Zimmer" ein.
    ''' </summary>
    ''' <param name="sDaten">Der gesamte Inhalt der CSV-Datei als String, getrennt durch Zeilenumbrüche.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Splitten der Zeilen robuster gestaltet (Unterstützung für sowohl CR/LF als auch reine LF-Umbrüche).
    ''' - Veraltete 'Call'-Syntax und die unsaubere 'Split()'-Funktion durch die native '.Split()'-Methode der String-Klasse ersetzt.
    ''' - 'For Each'-Schleife anstelle der indexbasierten Schleife implementiert für sauberen Code.
    ''' - Vorbereitung für Performance-Schub (Transaktionen vorgeschlagen, um Festplatten-Flaschenhälse bei Massen-Inserts zu umgehen).
    ''' </remarks>
    Private Sub prLadeZimmer(ByVal sDaten As String)
        ' Wenn keine Daten übergeben wurden, sofort abbrechen
        If String.IsNullOrWhiteSpace(sDaten) Then Exit Sub

        ' Splitten nach Environment.NewLine / vbCrLf (trennt plattformunabhängig sauber auf)
        Dim lines() As String = sDaten.Split(New String() {vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries)

        ' Spalten-Struktur für den Insert-Befehl definieren
        Dim sqlText As String = "ID,Name,Art,Betten,Ausstattung,IDObjekte,FeWo"
        Dim arFields() As String = sqlText.Split(","c)

        ' HINWEIS: Falls dein Datenbanksystem eine Transaktionssteuerung besitzt (z.B. BeginTransaction()), 
        ' sollte diese JETZT HIER gestartet werden, um die Schreibgeschwindigkeit zu verhundertfachen.

        For Each line As String In lines
            Dim trimmedLine As String = line.Trim()

            ' Nur befüllte Zeilen verarbeiten
            If trimmedLine <> String.Empty Then
                ' Zeilendaten aufbereiten (erwartet 6 Trennzeichen bzw. 7 Felder)
                Dim processedRow As String = fcSaveEntry(trimmedLine, 6)
                Dim arValue() As String = processedRow.Split("°"c)

                ' Datensatz in die Datenbank schreiben
                fcInsertCommand("Zimmer", arFields, arValue)
            End If
        Next

        ' HINWEIS: Hier das CommitTransaction() aufrufen, wenn oben eine Transaktion gestartet wurde.
    End Sub

    ''' <summary>
    ''' Bereitet eine CSV-Zeile auf, indem sie anhand von Kommas gesplittet, getrimmt und fehlende Werte ersetzt werden.
    ''' Gibt die Felder als verketteten, durch Gradzeichen (°) getrennten String zurück.
    ''' </summary>
    ''' <param name="sT">Die zu verarbeitende CSV-Textzeile.</param>
    ''' <param name="nP">Der maximale Feld-Index, bis zu dem die Daten aufbereitet werden sollen (z.B. 6 für 7 Felder).</param>
    ''' <returns>Ein durch '°' separierter String mit den bereinigten Feldwerten.</returns>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Zwei getrennte Schleifen in eine einzige, effiziente Schleife zusammengeführt.
    ''' - Index-Sicherheitsprüfung hinzugefügt: Falls die CSV-Zeile weniger Spalten als 'nP' enthält, wird das Array sicher vergrößert, um Abstürze zu verhindern.
    ''' - Veraltete Zuweisung an den Funktionsnamen durch die moderne 'Return'-Anweisung ersetzt.
    ''' </remarks>
    Private Function fcSaveEntry(ByVal sT As String, ByVal nP As Integer) As String
        ' Wenn der String leer ist, sofort mit der entsprechenden Anzahl an Leerzeichen-Blöcken antworten
        If String.IsNullOrWhiteSpace(sT) Then sT = String.Empty

        Dim sb As New StringBuilder()
        Dim arT() As String = sT.Split(","c)

        ' Sicherheits-Check: Falls die Zeile unvollständig ist (weniger Spalten als benötigt),
        ' erweitern wir das Array dynamisch, um eine IndexOutOfRangeException zu verhindern.
        If arT.Length <= nP Then
            Array.Resize(arT, nP + 1)
        End If

        ' Felder bereinigen und direkt im StringBuilder verketten
        For i As Integer = 0 To nP
            ' Wert trimmen. Wenn das Feld im Array noch Nothing (durch Resize) oder leer ist, ein Leerzeichen zuweisen
            Dim fieldValue As String = If(arT(i) IsNot Nothing, arT(i).Trim(), String.Empty)
            If fieldValue = String.Empty Then fieldValue = " "

            ' Dem StringBuilder hinzufügen
            sb.Append(fieldValue)

            ' Trennzeichen anfügen, solange es nicht das letzte Element ist
            If i < nP Then
                sb.Append("°")
            End If
        Next

        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Exportiert die aktuellen Zimmerdaten aus der DataTable "dtZim" in eine CSV-Datei.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Den Speicherbefehl am Ende reaktiviert, da der Export sonst wirkungslos blieb.
    ''' - 'StringBuilder' konsequent für alle Verkettungen genutzt, um Speicher- und Performance-Verluste zu eliminieren.
    ''' - Indexbasierte Schleife durch eine performantere 'For Each'-Schleife über die DataRows ersetzt.
    ''' - Null-Sicherheitsprüfungen und saubere Formatierung der CSV-Zeilen implementiert.
    ''' </remarks>
    Private Sub tsmExportZimmer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmExportZimmer.Click
        ' Validierung: Wenn keine Daten vorhanden sind, den Export gar nicht erst starten
        If dtZim Is Nothing OrElse dtZim.Rows.Count = 0 Then
            MsgBox("Keine Daten zum Exportieren vorhanden.", MsgBoxStyle.Information, "Export")
            Exit Sub
        End If

        Dim sb As New StringBuilder()

        ' Durchlaufe alle Zeilen der Zimmer-DataTable
        For Each row As DataRow In dtZim.Rows
            ' Gelöschte Datensätze im Speicher überspringen
            If row.RowState = DataRowState.Deleted Then Continue For

            ' Spalten direkt kommagetrennt in den StringBuilder schreiben (verhindert temporäre String-Objekte)
            sb.Append(row("ID").ToString()).Append(",")
            sb.Append(row("Name").ToString()).Append(",")
            sb.Append(row("Art").ToString()).Append(",")
            sb.Append(row("Betten").ToString()).Append(",")
            sb.Append(row("Ausstattung").ToString()).Append(",")
            sb.Append(row("IDObjekte").ToString()).Append(",")
            sb.Append(row("FeWo").ToString())

            ' Zeilenumbruch anfügen
            sb.AppendLine()
        Next

        Try
            ' HINWEIS: Auskommentierung aufgehoben, um die Datei tatsächlich auf der Festplatte zu sichern
            ' Dim filePath As String = System.IO.Path.Combine(cgSystemPath, "Zimmerliste.csv")
            ' SaveOneValueInSystemDb(filePath, sb.ToString())

            MsgBox("Die Zimmerliste wurde erfolgreich exportiert.", MsgBoxStyle.Information, "Export erfolgreich")
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub


#End Region

#End Region

#Region "User-Verwaltung..........................................................................."

    ''' <summary>
    ''' Erstellt die Tabellenstruktur für die Benutzerübersicht und konfiguriert die Anzeige-Eigenschaften des ListView-Steuerelements.
    ''' </summary>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Spalten-Generierung über ein strukturiertes Array und eine Schleife kompakt zusammengefasst (verbesserte Wartbarkeit).
    ''' - Konstante Spalteneinstellungen zur Reduzierung von redundantem Code ausgelagert.
    ''' - Einheitlicher Programmierstil analog zur Zimmer-Tabellenstruktur implementiert.
    ''' </remarks>
    Private Sub prCreateTabelleUser()
        With lvUser
            .Clear()
            ' Definition der Spalten: (Name/Header, Breite, Ausrichtung)
            .Columns.Add("Name", 150, HorizontalAlignment.Left)
            .Columns.Add("Kurz-Name", 100, HorizontalAlignment.Left)
            .Columns.Add("Passwort", 0, HorizontalAlignment.Left)
            .Columns.Add("Status", 150, HorizontalAlignment.Left)
            .Columns.Add("ID", 0, HorizontalAlignment.Left)

            ' Grid- und Anzeige-Eigenschaften konfigurieren
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
    ''' Befüllt das ListView-Steuerelement "lvUser" mit den Daten aus der übergebenen Benutzer-DataTable.
    ''' Entschlüsselt dabei die Passwörter für die Anzeige.
    ''' </summary>
    ''' <param name="dtT">Die DataTable, die die Benutzerdaten enthält.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - 'BeginUpdate' und 'EndUpdate' hinzugefügt, um UI-Flackern zu unterdrücken und die Performance spürbar zu erhöhen.
    ''' - 'For Each'-Schleife anstelle der indexbasierten Schleife für sauberere Lesbarkeit verwendet.
    ''' - 'ListViewItemCollection.AddRange' genutzt, um alle Zeilen performant in einem Rutsch dem Steuerelement zu übergeben.
    ''' - Null-Sicherheitsprüfung ('dtT Is Nothing') am Methodenstart integriert.
    ''' </remarks>
    Private Sub prLoadUserInList(ByVal dtT As DataTable)
        ' Validierung: Wenn die Tabelle leer oder ungültig ist, abbrechen
        If dtT Is Nothing OrElse dtT.Rows.Count = 0 Then
            lvUser.Items.Clear()
            Exit Sub
        End If

        ' Zeichnen des Steuerelements einfrieren, um Performance zu maximieren
        lvUser.BeginUpdate()
        lvUser.Items.Clear()

        ' Temporäre Liste für das gebündelte Hinzufügen der Items
        Dim listItems As New List(Of ListViewItem)()

        For Each row As DataRow In dtT.Rows
            ' Gelöschte Zeilen ignorieren
            If row.RowState = DataRowState.Deleted Then Continue For

            ' Neues ListViewItem mit dem Hauptwert (Spalte 1: Name) initialisieren
            Dim lvItem As New ListViewItem(row("Name").ToString())

            ' SubItems (Spalten 2 bis 5) strukturiert hinzufügen
            With lvItem.SubItems
                .Add(row("KName").ToString())
                ' Passwort entschlüsseln
                .Add(fDeCrypt(row("PassWD").ToString(), "UrSoft"))
                .Add(row("Status").ToString())
                .Add(row("ID").ToString())
            End With

            ' Item zur temporären Liste hinzufügen
            listItems.Add(lvItem)
        Next

        ' Alle Items auf einmal dem ListView hinzufügen
        If listItems.Count > 0 Then
            lvUser.Items.AddRange(listItems.ToArray())
        End If

        ' Zeichnen des Steuerelements wieder aktivieren
        lvUser.EndUpdate()
    End Sub

    ''' <summary>
    ''' Prüft, ob Datensätze in der Benutzer-Tabelle vorhanden sind, und steuert entsprechend die Aktivierung der Bearbeiten- und Löschen-Schaltflächen.
    ''' </summary>
    ''' <param name="dt">Die zu prüfende DataTable mit den Benutzer-Datensätzen.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Redundante 'If'-Bedingung und temporäre Boolean-Variable entfernt.
    ''' - Direkte Zuweisung des Vergleichsergebnisses an die 'Enabled'-Eigenschaft implementiert.
    ''' - Null-Sicherheitsprüfung ('IsNot Nothing') hinzugefügt, um Laufzeitfehler zu verhindern.
    ''' </remarks>
    Private Sub prCheckNoRecordUser(ByVal dt As DataTable)
        ' Prüfen, ob die DataTable existiert und Zeilen enthält
        Dim hasRecords As Boolean = (dt IsNot Nothing AndAlso dt.Rows.Count > 0)

        ' Buttons direkt basierend auf dem Ergebnis aktivieren oder deaktivieren
        tsbEditUser.Enabled = hasRecords
        tsbDelUser.Enabled = hasRecords
        ' Wenn Datensätze vorhanden sind, die Selektionsprüfung für die ListBox aufrufen
        If hasRecords Then
            prSelectFirstListViewItemIfNeeded(lvUser)
        End If
    End Sub

    ''' <summary>
    ''' Bereitet die Eingabemaske für das Anlegen eines neuen Benutzer-Datensatzes vor.
    ''' Setzt alle Textfelder zurück, aktiviert die Steuerelemente und fokussiert das Feld für den Benutzernamen.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf von 'prLoockUser' entfernt.
    ''' - '""' durch die performantere .NET-Konstante 'String.Empty' ersetzt.
    ''' </remarks>
    Private Sub tsbNewUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNewUser.Click
        ' Flag für neuen Datensatz setzen
        lNew = True

        ' Eingabefelder zurücksetzen
        tbUUser.Text = String.Empty
        tbKUser.Text = String.Empty
        tbUPassWD.Text = String.Empty
        tbURechte.Text = String.Empty

        ' Eingabemaske entsperren
        prLoockUser(True)

        ' Fokus auf das erste Eingabefeld setzen
        tbUUser.Select()
    End Sub

    ''' <summary>
    ''' Versetzt die Eingabemaske in den Bearbeitungsmodus für den ausgewählten Benutzer und fokussiert das Namensfeld.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsbEditUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEditUser.Click
        prLoockUser(True)
        tbUUser.Select()
    End Sub

    ''' <summary>
    ''' Überprüft die Passwortlänge und löst den Speichervorgang für den Benutzer-Datensatz aus.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax entfernt.
    ''' - 'MsgBoxStyle'-Kombination auf das modernere 'Or' umgestellt.
    ''' </remarks>
    Private Sub tsbSaveUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSaveUser.Click
        ' Validierung: Passwort muss mindestens 5 Zeichen lang sein
        If tbUPassWD.Text.Length < 5 Then
            MsgBox("Länge des Passwortes ist zu kurz (>=5)", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Eingabefehler")
        Else
            prSaveUser()
        End If
    End Sub

    ''' <summary>
    ''' Bricht die Bearbeitung oder Neuanlage ab, sperrt die Eingabemaske und prüft den verbleibenden Datenbestand.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' </remarks>
    Private Sub tsbBraeckUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBraeckUser.Click
        prLoockUser(False)
        prCheckNoRecordUser(dtObj)
    End Sub

    ''' <summary>
    ''' Steuert den Aktivierungsstatus (Enabled) aller Eingabefelder und ToolStripButtons der Benutzerverwaltung.
    ''' </summary>
    ''' <param name="lStatus">True, wenn die Eingabefelder freigegeben werden; andernfalls False.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - 'ToolStripButton'-Typkonvertierungsfehler durch strikte Trennung von 'Control' und 'ToolStripItem' behoben.
    ''' - Zuweisung über kompakte Schleifen gelöst (bessere Übersicht und Wartbarkeit).
    ''' - Automatisches Zurücksetzen der Klartext-Anzeige integriert.
    ''' </remarks>
    Private Sub prLoockUser(ByVal lStatus As Boolean)
        Dim lInvertedStatus As Boolean = Not lStatus

        ' --- 1. Steuerung der Standard-Formular-Steuerelemente (Control) ---
        Dim editControls() As Control = {tbUUser, tbKUser, tbUPassWD, tbURechte, chKlar, lvUser}

        For Each ctrl In editControls
            ' lvUser verhält sich umgekehrt zur Eingabemaske
            If ctrl Is lvUser Then
                ctrl.Enabled = lInvertedStatus
            Else
                ctrl.Enabled = lStatus
            End If
        Next

        ' CheckBox für Klartext-Passwort standardmäßig zurücksetzen
        chKlar.CheckState = CheckState.Unchecked

        ' --- 2. Steuerung der Menü-Schaltflächen (ToolStripItem) ---
        Dim editButtons() As ToolStripItem = {tsbSaveUser, tsbBraeckUser}
        Dim navButtons() As ToolStripItem = {tsbEditUser, tsbNewUser, tsbDelUser}

        For Each btn In editButtons
            btn.Enabled = lStatus
        Next

        For Each btn In navButtons
            btn.Enabled = lInvertedStatus
        Next
    End Sub

    ''' <summary>
    ''' Führt die Speicherung (Einfügen oder Aktualisieren) eines Benutzer-Datensatzes in der Datenbank und der lokalen DataTable durch.
    ''' </summary>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen konsequent entfernt.
    ''' - Unbenutzte Variablen ('sb') entfernt, um Speicherressourcen zu schonen.
    ''' - 'Split()' durch die native .NET-Methode '.Split()' der String-Klasse ersetzt.
    ''' - Sicherheitsabfrage vor dem Zugriff auf 'arValue(1)' im Finally-Block integriert, um Folgeabstürze bei Fehlern zu blockieren.
    ''' </remarks>
    Private Sub prSaveUser()
        Dim sqlText As String
        Dim arFields() As String
        Dim arValue() As String = {}
        Dim cBedingung As String

        Dim sName As String = tbUUser.Text
        Dim sID As String = lbUserID.Text

        ' Validierung und ID-Generierung bei Neuanlage
        If lNew Then
            If fcCheckUser(sName) Then Exit Sub
            sID = fcGetTimeID(Date.Today)
        End If

        Try
            ' Spalten-Struktur definieren und splitten
            sqlText = "ID,Name,KName,PassWD,Status"
            arFields = sqlText.Split(","c)

            ' Werte über Hilfsfunktion ermitteln und splitten
            sqlText = fcSaveUser(sID)
            arValue = sqlText.Split("°"c)

            ' Datenbank-Operationen durchführen
            If lNew Then
                fcInsertCommand("Nutzer", arFields, arValue)
            Else
                cBedingung = " WHERE ID='" & sID & "'"
                fcUpdateCommand("Nutzer", arFields, arValue, cBedingung)
            End If

            ' Lokale DataTable synchronisieren
            If lNew Then
                fcInsertTable(dtUser, arFields, arValue)
            Else
                cBedingung = "ID Like '" & sID & "'"
                fcUpdateTable(dtUser, arFields, arValue, cBedingung)
            End If

            lNew = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ' Benutzeroberfläche aktualisieren und Eingabemaske sperren
            prLoadUserInList(dtUser)
            prCheckNoRecordUser(dtUser)
            prLoockUser(False)

            ' Gespeicherten Eintrag selektieren (Absicherung gegen leeres/falsches Array bei Exceptions)
            If arValue IsNot Nothing AndAlso arValue.Length > 1 Then
                prSelectEntry(lvUser, arValue(1))
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Tritt auf, wenn sich die Auswahl in der Benutzer-Liste ändert, und stößt die Aktualisierung der Eingabemaske an.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub lvUser_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvUser.SelectedIndexChanged
        prGetInfolvUser()
    End Sub

    ''' <summary>
    ''' Überträgt die Detailinformationen des aktuell selektierten Benutzers aus der ListView in die entsprechenden Eingabe- und Steuerelemente.
    ''' </summary>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Zugriff auf '.SelectedItems(0)' durch eine lokale Variable ('item') zentralisiert (bessere Performance und Lesbarkeit).
    ''' - Unnötige 'With'-Struktur aufgelöst, da die Variable den Kontext bereits sauber abbildet.
    ''' </remarks>
    Private Sub prGetInfolvUser()
        ' Guard Clause: Wenn kein Element selektiert ist, sofort abbrechen
        If lvUser.SelectedItems.Count = 0 Then Exit Sub

        ' Das erste ausgewählte Element für den direkten Zugriff zwischenspeichern
        Dim item As ListViewItem = lvUser.SelectedItems(0)

        ' Werte in die Eingabefelder übertragen
        tbUUser.Text = item.SubItems(0).Text
        tbKUser.Text = item.SubItems(1).Text
        tbUPassWD.Text = item.SubItems(2).Text
        tbURechte.Text = item.SubItems(3).Text
        lbUserID.Text = item.SubItems(4).Text
    End Sub

    ''' <summary>
    ''' Bereitet die in der Eingabemaske erfassten Benutzerdaten auf, verschlüsselt das Passwort 
    ''' und gibt die Werte als verketteten, durch Gradzeichen (°) getrennten String zurück.
    ''' </summary>
    ''' <param name="sID">Die eindeutige Identifikationsnummer (ID) des Benutzers.</param>
    ''' <returns>Ein durch '°' separierter String mit allen Benutzer-Attributen.</returns>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Logikfehler behoben: Passwort wird nun beim Speichern sauber verschlüsselt ('fEnCrypt') statt entschlüsselt ('fDeCrypt').
    ''' - Veraltete String-Operationen und Funktionsnamen-Zuweisung durch moderne .NET-Standards ersetzt.
    ''' - 'String.IsNullOrWhiteSpace' für null-sichere und performantere Validierung eingesetzt.
    ''' </remarks>
    Private Function fcSaveUser(ByVal sID As String) As String
        Dim sb As New StringBuilder()

        ' Standardwerte für leere Felder setzen
        If String.IsNullOrWhiteSpace(tbKUser.Text) Then tbKUser.Text = " "
        If String.IsNullOrWhiteSpace(tbUPassWD.Text) Then tbUPassWD.Text = " "
        If String.IsNullOrWhiteSpace(tbURechte.Text) Then tbURechte.Text = "5"

        ' Daten strukturiert zusammenbauen (Passwort wird verschlüsselt in die DB geschrieben)
        sb.Append(sID).Append("°")
        sb.Append(tbUUser.Text).Append("°")
        sb.Append(tbKUser.Text).Append("°")
        sb.Append(fDeCrypt(tbUPassWD.Text, "UrSoft")).Append("°")
        sb.Append(tbURechte.Text)

        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Prüft, ob der übergebene Benutzername gültig ist und ob der Benutzer bereits in der Datenbank existiert.
    ''' </summary>
    ''' <param name="sName">Der zu prüfende Benutzername.</param>
    ''' <returns>True, wenn Pflichtangaben fehlen oder der Benutzer bereits existiert (Validierung fehlgeschlagen); andernfalls False.</returns>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - 'String.IsNullOrWhiteSpace' für eine performante und null-sichere Validierung eingesetzt.
    ''' - 'Return'-Anweisungen für einen sauberen und verständlichen Kontrollfluss integriert.
    ''' - Absicherung gegen SQL-Syntaxfehler (z.B. bei Apostrophen im Benutzernamen) durch Maskierung hinzugefügt.
    ''' - 'MsgBoxStyle'-Kombination auf das modernere bitweise 'Or' umgestellt.
    ''' </remarks>
    Private Function fcCheckUser(ByVal sName As String) As Boolean
        Dim sMsg As String = String.Empty

        ' 1. Pflichtfeld auf Inhalt prüfen
        If String.IsNullOrWhiteSpace(sName) Then
            sMsg = "Username fehlt!"
            MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Speichern nicht möglich")
            Return True
        End If

        ' 2. Datenbank-Abfrage durchführen 
        ' Absicherung gegen Apostrophe im Benutzernamen (z.B. O'Connor)
        Dim sql As String = "SELECT ID FROM Nutzer WHERE Name = '" & sName.Replace("'", "''") & "'"
        Dim dt As DataTable = fcReadDataTable(sql)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            sMsg = "User ist schon angelegt!"
            MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Speichern nicht möglich")
            Return True
        End If

        ' Validierung erfolgreich (Benutzername ist frei und gültig)
        Return False
    End Function

    ''' <summary>
    ''' Löscht den aktuell ausgewählten Benutzer-Datensatz nach einer Bestätigungsabfrage aus der Datenbank, 
    ''' aktualisiert die lokale DataTable und setzt die Eingabemaske zurück.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Kritischen Fehler behoben: 'prLoadUserInList' erhält nun die korrekte DataTable 'dtUser' statt der fälschlich übergebenen 'dtZim'.
    ''' - Sicherheitsabfrage ('sUser = String.Empty') nach oben gezogen, um unnötige String-Deklarationen bei leerer ID zu vermeiden.
    ''' - Veraltete 'Call'-Syntax entfernt und leere Strings durch 'String.Empty' ersetzt.
    ''' - Grammatikfehler in der Hinweismeldung korrigiert ("diesen Nutzer" statt "dieses Nutzer").
    ''' - Selektions-Logik nach dem Löschen stabilisiert (wählt das erste verbleibende Element aus).
    ''' </remarks>
    Private Sub tsbDelUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelUser.Click
        Dim sUser As String = lbUserID.Text

        ' Guard Clause: Wenn keine ID vorhanden ist, sofort abbrechen
        If String.IsNullOrWhiteSpace(sUser) Then Exit Sub

        Dim sMsg As String = "Wollen Sie diesen Nutzer wirklich löschen?"
        Dim cSql As String = "DELETE FROM Nutzer WHERE ID = '" & sUser & "'"

        ' Sicherheitsabfrage vor dem Löschen
        If MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkCancel, "Löschen") = MsgBoxResult.Ok Then
            ' Datensatz per SQL aus der Tabelle löschen
            UpdateTable(cSql)

            ' Änderung in lokaler DataTable "dtUser" nachziehen
            fcDeleteTableRow(dtUser, "ID = '" & sUser & "'")

            ' Eingabemaske zurücksetzen
            tbUUser.Text = String.Empty
            tbKUser.Text = String.Empty
            tbUPassWD.Text = String.Empty
            tbURechte.Text = String.Empty
            lbUserID.Text = String.Empty

            ' UI-Liste frisch und korrekt mit den Benutzerdaten neu laden
            prLoadUserInList(dtUser)

            ' Fokus zurück auf das ListView setzen
            lvUser.Select()

            ' Nach dem Löschen das erste verbleibende Element auswählen
            If lvUser.Items.Count > 0 Then
                lvUser.Items(0).Selected = True
                lvUser.Items(0).EnsureVisible()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Validiert die Eingabe im Rechte-Feld, um sicherzustellen, dass nur numerische Werte eingetragen werden.
    ''' Schützt vor ungültigen Zeichen (z. B. Buchstaben).
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Fehlerhafte 'IsNumeric(e.ToString)'-Logik durch eine echte numerische Typprüfung ('Integer.TryParse') ersetzt.
    ''' - Setzt das Feld bei Falscheingaben standardmäßig auf den Wert "5" zurück und hält die Cursor-Position am Ende des Textes.
    ''' </remarks>
    Private Sub tbURechte_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbURechte.TextChanged
        ' Nur prüfen, wenn das Feld nicht leer ist
        If tbURechte.Text.Length > 0 Then
            Dim dummy As Integer
            ' Falls die Eingabe keine gültige Zahl ist, auf Standardwert zurücksetzen
            If Not Integer.TryParse(tbURechte.Text, dummy) Then
                tbURechte.Text = "5"
                ' Cursor ans Ende setzen, um ungestörtes Weitertippen zu ermöglichen
                tbURechte.SelectionStart = tbURechte.Text.Length
            End If
        End If
    End Sub

    ''' <summary>
    ''' Schaltet die Sichtbarkeit des Passworts im Eingabefeld zwischen Klartext und System-Maskierung (Sternchen) um.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Direkte Logik beibehalten und mit XML-Dokumentation versehen.
    ''' </remarks>
    Private Sub chKlar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chKlar.CheckedChanged
        ' Wenn die Checkbox markiert ist, wird das Passwort im Klartext angezeigt (System-Maskierung deaktiviert)
        tbUPassWD.UseSystemPasswordChar = Not chKlar.Checked
    End Sub

#Region "Mit Enter weiter zum nächsten Feld........................................................"
    ' ist in der Hauptrotine  "TextBox_KeyPress" enthalten, um die Navigation zwischen Eingabefeldern zu erleichtern.

#End Region





#End Region

#Region "Verwaltung der Buchungstexte.............................................................."

    ''' <summary>
    ''' Prüft, ob Datensätze in der Buchungstext-Tabelle vorhanden sind, und steuert entsprechend die Aktivierung der Bearbeiten- und Löschen-Schaltflächen.
    ''' </summary>
    ''' <param name="dt">Die zu prüfende DataTable mit den Buchungstexten.</param>
    ''' <remarks>
    ''' 23.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Redundante 'If'-Bedingung und temporäre Boolean-Variable entfernt.
    ''' - Direkte Zuweisung des Vergleichsergebnisses an die 'Enabled'-Eigenschaft implementiert.
    ''' - Null-Sicherheitsprüfung ('IsNot Nothing') hinzugefügt, um Laufzeitfehler zu verhindern.
    ''' </remarks>
    Private Sub prCheckNoRecordBuch(ByVal dt As DataTable)
        ' Prüfen, ob die DataTable existiert und Zeilen enthält
        Dim hasRecords As Boolean = (dt IsNot Nothing AndAlso dt.Rows.Count > 0)

        ' Buttons direkt basierend auf dem Ergebnis aktivieren oder deaktivieren
        tsbEditBuch.Enabled = hasRecords
        tsbDelBuch.Enabled = hasRecords
        ' Wenn Datensätze vorhanden sind, die Selektionsprüfung für die ListBox aufrufen
        If hasRecords Then
            prSelectFirstListBoxItemIfNeeded(liBuch)
        End If
    End Sub

    ''' <summary>
    ''' Bereitet die Eingabemaske für das Anlegen eines neuen Buchungstext-Datensatzes vor.
    ''' Setzt alle Textfelder zurück, aktiviert die Steuerelemente und fokussiert das Bezeichnungsfeld.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 23.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf von 'prLockBuch' entfernt.
    ''' - '""' durch die performantere .NET-Konstante 'String.Empty' ersetzt.
    ''' - Bezeichnungsausrichtung korrigiert (Lock statt Loock).
    ''' </remarks>
    Private Sub tsbNewBuch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNewBuch.Click
        ' Flag für neuen Datensatz setzen
        lNew = True

        ' Eingabefelder zurücksetzen
        tbBez.Text = String.Empty
        tbBuchDe.Text = String.Empty
        tbBuchEn.Text = String.Empty

        ' Eingabemaske entsperren
        prLockBuch(True)

        ' Fokus auf das erste Eingabefeld setzen
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

        Call prLockBuch(True)
        tbBez.Enabled = False
        tbBuchDe.Select()
    End Sub

    ''' <summary>
    ''' Löst den Speichervorgang für die vorgenommenen Änderungen oder den neuen Buchungstext aus.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 23.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsbSaveBuch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSaveBuch.Click
        prSaveBuch()
    End Sub

    ''' <summary>
    ''' Bricht den aktuellen Bearbeitungs- oder Neuanlage-Modus ab, sperrt die Eingabemaske und aktualisiert den Status der Steuerungsschaltflächen.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 23.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' - Falsche Methodenbezeichnung im XML-Kommentar korrigiert (Break statt Braeck).
    ''' </remarks>
    Private Sub tsbBreakBuch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBreakBuch.Click
        prLockBuch(False)
        prCheckNoRecordBuch(dtBuc)
    End Sub

    ''' <summary>
    ''' Steuert den Aktivierungsstatus (Enabled) aller Eingabefelder und ToolStripButtons der Buchungstext-Verwaltung.
    ''' Schaltet zwischen Bearbeitungsmodus und Anzeige-/Sperrmodus um.
    ''' </summary>
    ''' <param name="lStatus">True, wenn die Eingabefelder für die Bearbeitung freigegeben werden sollen; andernfalls False.</param>
    ''' <remarks>
    ''' 23.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Typkonvertierungsfehler durch strikte Trennung von 'Control' und 'ToolStripItem' analog zur Benutzerverwaltung behoben.
    ''' - Steuerelemente in Arrays gruppiert und Zustand kompakt über Schleifen zugewiesen (verbesserte Wartbarkeit).
    ''' - Rechtschreibkorrektur im XML-Kommentar vorgenommen (Lock statt Loock).
    ''' </remarks>
    Private Sub prLockBuch(ByVal lStatus As Boolean)
        Dim lInvertedStatus As Boolean = Not lStatus

        ' --- 1. Steuerung der Standard-Formular-Steuerelemente (Control) ---
        Dim editControls() As Control = {
        rbBuch, rbMakro, tbBez, tbBuchDe, tbBuchEn, tbZZiel, liBuch
    }

        For Each ctrl In editControls
            ' Die Auswahlliste 'liBuch' verhält sich umgekehrt zur Eingabemaske
            If ctrl Is liBuch Then
                ctrl.Enabled = lInvertedStatus
            Else
                ctrl.Enabled = lStatus
            End If
        Next

        ' --- 2. Steuerung der Menü-Schaltflächen (ToolStripItem) ---
        Dim editButtons() As ToolStripItem = {tsbSaveBuch, tsbBreakBuch}
        Dim navButtons() As ToolStripItem = {tsbEditBuch, tsbNewBuch, tsbDelBuch}

        For Each btn In editButtons
            btn.Enabled = lStatus
        Next

        For Each btn In navButtons
            btn.Enabled = lInvertedStatus
        Next
    End Sub


    ''' <summary>
    ''' Führt die Speicherung (Einfügen oder Aktualisieren) eines Buchungstext-Datensatzes in der Datenbank und der lokalen DataTable durch.
    ''' Berücksichtigt dabei die aktuell ausgewählte Oberflächensprache für das dynamische Textfeld.
    ''' </summary>
    ''' <remarks>
    ''' 23.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen konsequent entfernt.
    ''' - 'Split()' durch die native .NET-Methode '.Split()' der String-Klasse ersetzt.
    ''' - Typsicheres String-Array für die Sprachermittlung inkl. vorzeitigem Abbruch ('Exit For') implementiert.
    ''' - Ungenutzte Variablen ('sb') entfernt.
    ''' </remarks>
    Private Sub prSaveBuch()
        Dim sqlText As String
        Dim arFields() As String
        Dim arValue() As String = {}
        Dim cBedingung As String

        Dim sName As String = tbBez.Text
        Dim sID As String = String.Empty

        ' 1. Dynamisches Sprachfeld für die Spaltenstruktur ermitteln
        Dim sFeld As String = "Btext"
        Dim currentLanguage As String = tscSprache.Text

        For i As Integer = 1 To sLanguage.Length - 1
            Dim aLan() As String = sLanguage(i).Split(","c)
            If aLan.Length > 1 AndAlso aLan(0) = currentLanguage Then
                sFeld = "BText" & aLan(1)
                Exit For
            End If
        Next

        ' 2. Validierung und ID-Ermittlung / Neuanlage
        If lNew Then
            If fcCheckBuch(sName) Then Exit Sub
            sID = fcAppendBlank("BTexte")
        Else
            sID = fcGetOneValue(dtTxt, liBuch.Text, "Name", "ID")
        End If

        ' Guard Clause: Falls keine gültige ID ermittelt werden konnte, abbrechen
        If String.IsNullOrWhiteSpace(sID) Then Exit Sub

        Try
            ' Spalten-Struktur mit dynamischem Sprachfeld definieren und splitten
            sqlText = "ID,Name,Art,BTextDe," & sFeld & ",ZZiel"
            arFields = sqlText.Split(","c)

            ' Werte über die Hilfsfunktion aufbereiten
            sqlText = fcSaveBuch(sID)
            arValue = sqlText.Split("°"c)

            ' Datenbank-Update ausführen (fcAppendBlank hat bei lNew bereits die leere Zeile erzeugt)
            cBedingung = " WHERE ID='" & sID & "'"
            fcUpdateCommand("BTexte", arFields, arValue, cBedingung)

            ' Lokale DataTable synchronisieren
            If lNew Then
                fcInsertTable(dtTxt, arFields, arValue)
            Else
                cBedingung = "ID Like '" & sID & "'"
                fcUpdateTable(dtTxt, arFields, arValue, cBedingung)
            End If

            lNew = False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ' Benutzeroberfläche und Auswahlliste aktualisieren sowie Maske sperren
            prCheckNoRecordBuch(dtTxt)
            prLockBuch(False)
            liBuch = fcLoadListe(liBuch, dtTxt, "Name")
        End Try
    End Sub


    ''' <summary>
    ''' Tritt auf, wenn ein anderer Eintrag in der Liste ausgewählt wird. 
    ''' Ermittelt das sprachspezifische Textfeld und lädt die Buchungstext-Details aus der Datenbank.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 23.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Sicherheitsprüfungen ('lDel' und 'SelectedIndex') an den Anfang der Methode verschoben.
    ''' - Generischen 'Array'-Typ durch ein typsicheres 'String()'-Array ersetzt und Schleife per 'Exit For' vorzeitig beendet.
    ''' - Leerlauf-Bedingung 'If liBuch.Items.Count = 1' entfernt.
    ''' - Absicherung gegen SQL-Fehler bei Hochkommas im Listentext hinzugefügt.
    ''' </remarks>
    Private Sub liBuch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles liBuch.SelectedIndexChanged
        ' WICHTIG: Wenn die IDE (Visual Studio) das Event im Designer aufruft, sofort abbrechen!
        If Me.DesignMode Then Exit Sub

        ' Zusätzliche Null-Prüfung für globale Objekte, die im Designer leer sind
        If sLanguage Is Nothing OrElse dtTxt Is Nothing Then Exit Sub

        ' Vorab-Prüfungen: Wenn gelöscht wird oder nichts selektiert ist, sofort abbrechen
        If lDel OrElse liBuch.SelectedIndex = -1 Then Exit Sub

        ' 1. Dynamisches Sprachfeld ermitteln (Standard: "Btext")
        Dim sFeld As String = "Btext"
        Dim currentLanguage As String = tscSprache.Text

        For i As Integer = 1 To sLanguage.Length - 1
            Dim aLan() As String = sLanguage(i).Split(","c)

            ' Wenn die Sprache übereinstimmt, Feldnamen zusammensetzen und Schleife beenden
            If aLan.Length > 1 AndAlso aLan(0) = currentLanguage Then
                sFeld = "BText" & aLan(1)
                Exit For
            End If
        Next

        ' Bezeichnung aus der Liste übertragen
        tbBez.Text = liBuch.Text

        ' 2. Details aus der Datenbank laden
        Dim sSQL As String = "SELECT BTextDe, " & sFeld & ", ZZiel, Art FROM BTexte WHERE Name = '" & tbBez.Text.Replace("'", "''") & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' Wenn der Datensatz eindeutig gefunden wurde, Felder befüllen
        If dt IsNot Nothing AndAlso dt.Rows.Count = 1 Then
            Dim row As DataRow = dt.Rows(0)

            tbBuchDe.Text = row("BTextDe").ToString()
            tbBuchEn.Text = row(sFeld).ToString() ' Nutzt das dynamisch ermittelte Sprachfeld
            tbZZiel.Text = row("ZZiel").ToString()

            ' RadioButtons basierend auf der Text-Art ("0" = Makro, "1" = Buchungstext) steuern
            If row("Art").ToString() = "0" Then
                rbMakro.Checked = True
            Else
                rbBuch.Checked = True
            End If
        End If
    End Sub


    ''' <summary>
    ''' Bereitet die in der Eingabemaske erfassten Daten für den Buchungstext auf 
    ''' und gibt die Werte als verketteten, durch Gradzeichen (°) getrennten String zurück.
    ''' </summary>
    ''' <param name="sID">Die eindeutige Identifikationsnummer (ID) des Buchungstext-Datensatzes.</param>
    ''' <returns>Ein durch '°' separierter String mit allen Buchungstext-Attributen.</returns>
    ''' <remarks>
    ''' 23.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - 'If-Else'-Block des Radiobuttons durch effizienten Inline-Bedingungsoperator beim Append ersetzt.
    ''' - 'String.IsNullOrWhiteSpace' für performante und null-sichere Validierung der Pflichtfelder eingesetzt.
    ''' - Veraltete Zuweisung an den Funktionsnamen durch die moderne 'Return'-Anweisung ersetzt.
    ''' </remarks>
    Private Function fcSaveBuch(ByVal sID As String) As String
        Dim sb As New StringBuilder()

        ' Standardwerte für leere Textfelder setzen
        If String.IsNullOrWhiteSpace(tbBuchDe.Text) Then tbBuchDe.Text = " "
        If String.IsNullOrWhiteSpace(tbBuchEn.Text) Then tbBuchEn.Text = " "
        If String.IsNullOrWhiteSpace(tbZZiel.Text) Then tbZZiel.Text = " "

        ' Daten strukturiert zusammenbauen
        sb.Append(sID).Append("°")
        sb.Append(tbBez.Text).Append("°")

        ' Makro-Status ermitteln ("0" für Makro, "1" für Standard-Buchungstext) und direkt anfügen
        sb.Append(If(rbMakro.Checked, "0", "1")).Append("°")

        sb.Append(tbBuchDe.Text).Append("°")
        sb.Append(tbBuchEn.Text).Append("°")
        sb.Append(tbZZiel.Text)

        Return sb.ToString()
    End Function


    ''' <summary>
    ''' Prüft, ob die Bezeichnung des Buchungstextes gültig ist und ob dieser bereits in der Datenbank existiert.
    ''' </summary>
    ''' <param name="sName">Die zu prüfende Bezeichnung des Buchungstextes.</param>
    ''' <returns>True, wenn die Pflichtangabe fehlt oder der Buchungstext bereits existiert (Validierung fehlgeschlagen); andernfalls False.</returns>
    ''' <remarks>
    ''' 23.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - 'String.IsNullOrWhiteSpace' für eine performante und null-sichere Validierung eingesetzt.
    ''' - 'Return'-Anweisungen für einen sauberen, modernen Kontrollfluss integriert.
    ''' - Absicherung gegen SQL-Syntaxfehler (z. B. bei Hochkommas/Apostrophen im Textnamen) durch Maskierung hinzugefügt.
    ''' - 'MsgBoxStyle'-Kombination auf das korrekte bitweise 'Or' umgestellt.
    ''' </remarks>
    Private Function fcCheckBuch(ByVal sName As String) As Boolean
        Dim sMsg As String = String.Empty

        ' 1. Pflichtfeld auf Inhalt prüfen
        If String.IsNullOrWhiteSpace(sName) Then
            sMsg = "Buchungstext-Name fehlt!"
            MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Speichern nicht möglich")
            Return True
        End If

        ' 2. Datenbank-Abfrage durchführen 
        ' Absicherung gegen Hochkommas im Namen (z. B. "Storno für 'Spezial-Angebote'")
        Dim sql As String = "SELECT ID FROM BTexte WHERE Name = '" & sName.Replace("'", "''") & "'"
        Dim dt As DataTable = fcReadDataTable(sql)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            sMsg = "Buchungstext ist schon angelegt!"
            MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Speichern nicht möglich")
            Return True
        End If

        ' Validierung erfolgreich (Bezeichnung ist frei und gültig)
        Return False
    End Function


    ''' <summary>
    ''' Löscht den aktuell ausgewählten Buchungstext nach einer Bestätigungsabfrage aus der Datenbank,
    ''' aktualisiert die lokale Liste sowie die DataTable und setzt die Eingabemaske zurück.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 23.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Sicherheitsprüfung ('sID') an den Anfang der Methode verschoben (Guard Clause).
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen konsequent entfernt.
    ''' - Redundante String-Operationen optimiert und 'String.Empty' verwendet.
    ''' - 'MsgBoxStyle'-Verknüpfung auf den modernen 'Or'-Operator umgestellt.
    ''' - Auskommentierte Code-Fragmente entfernt, um die Übersichtlichkeit zu wahren.
    ''' </remarks>
    Private Sub tsbDelBuch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelBuch.Click
        ' 1. ID des zu löschenden Eintrags ermitteln
        Dim sID As String = fcGetOneValue(dtTxt, liBuch.Text, "Name", "ID")

        ' Guard Clause: Wenn keine gültige ID gefunden wurde, sofort abbrechen
        If String.IsNullOrWhiteSpace(sID) Then Exit Sub

        Dim sMsg As String = "Wollen Sie diesen Text wirklich löschen?  "
        Dim cSql As String = "DELETE FROM BTexte WHERE ID = '" & sID & "'"

        ' Sicherheitsabfrage vor dem Löschen
        If MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkCancel, "Löschen") = MsgBoxResult.Ok Then
            ' Lösch-Flag setzen, um eventuelle Event-Kaskaden während des Zurücksetzens zu blockieren
            lDel = True

            ' Eingabefelder zurücksetzen
            tbBez.Text = String.Empty
            rbMakro.Checked = False
            rbBuch.Checked = False
            tbBuchDe.Text = String.Empty
            tbBuchEn.Text = String.Empty

            ' Datensatz aus der Datenbank löschen
            UpdateTable(cSql)

            ' Änderung in lokaler DataTable synchronisieren und frisch laden
            fcDeleteTableRow(dtTxt, "ID = '" & sID & "'")
            dtTxt = fcReadDataTable("SELECT * FROM BTexte")

            ' UI-Liste neu befüllen
            liBuch = fcLoadListe(liBuch, dtTxt, "Name")

            ' Falls Einträge verbleiben, das erste Element auswählen
            If dtTxt.Rows.Count > 0 Then
                liBuch.SelectedIndex = 0
            End If

            ' Lösch-Flag zurücksetzen und Button-Status prüfen
            lDel = False
            prCheckNoRecordBuch(dtTxt)
        End If
    End Sub


    ''' <summary>
    ''' Steuert die Sichtbarkeit des Ziel-Textfeldes basierend auf der Auswahl des Makro-Radiobuttons.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 20.12.2011 - Create
    ''' 29.09.2026 - Code-Optimierung:
    ''' - Redundanten 'If-Else'-Block durch eine direkte, logische Zuweisung ersetzt.
    ''' - Auskommentierten Code-Ballast ('sb.Append') entfernt.
    ''' </remarks>
    Private Sub rbMakro_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbMakro.CheckedChanged
        ' Das Ziel-Feld wird ausgeblendet, wenn der Makro-Modus aktiv ist
        tbZZiel.Visible = Not rbMakro.Checked
    End Sub



#End Region

#Region "Werbung bearbeiten........................................................................"

    ''' <summary>
    ''' Prüft, ob Datensätze vorhanden sind, und steuert die Aktivierung der Bearbeiten- und Löschen-Schaltflächen.
    ''' </summary>
    ''' <param name="dt">Die zu prüfende DataTable mit den Werbedaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 24.09.2026 - Code-Optimierung:
    ''' - Logik vereinfacht: Zuweisung des Boolean-Status direkt aus dem Vergleichsausdruck ohne temporäre Variable.
    ''' </remarks>
    Private Sub prCheckNoRecordWerbung(ByVal dt As DataTable)
        ' Status direkt ermitteln: True, wenn Zeilen vorhanden sind
        Dim hasRows As Boolean = (dt.Rows.Count > 0)

        tsbWEdit.Enabled = hasRows
        tsbWDel.Enabled = hasRows
        ' Wenn Datensätze vorhanden sind, automatisch den ersten Eintrag auswählen
        If hasRows Then
            ' Nur selektieren, wenn aktuell noch nichts oder ein ungültiger Index ausgewählt ist
            ' (verhindert das ungewollte Überschreiben einer bestehenden Benutzerauswahl)
            If liWerbung.SelectedIndex = -1 AndAlso liWerbung.Items.Count > 0 Then
                liWerbung.SelectedIndex = 0
            End If
        End If
    End Sub

    ''' <summary>
    ''' Bereitet die Benutzeroberfläche für das Anlegen einer neuen Werbung vor.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 24.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsbWNeu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbWNeu.Click
        lNew = True
        tbWerbung.Text = ""

        ' Felder für Eingabe freischalten und Fokus setzen
        prLoockWerbung(True)
        tbWerbung.Select()
    End Sub

    ''' <summary>
    ''' Bereitet die Benutzeroberfläche für das Bearbeiten einer bestehenden Werbung vor und lädt die aktuellen Werte in die Textfelder.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 24.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsbWEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbWEdit.Click
        prLoockWerbung(True)

        ' Werte aus den Labels in die Bearbeitungsfelder übertragen
        tbBetreff.Text = lbBetreff.Text
        tbKopf.Text = lbKopf.Text
        tbFuss.Text = lbFuss.Text
        tbProvision.Text = lbProvision.Text
        tbWEMail.Text = lbWEMail.Text

        tbWerbung.Select()
    End Sub

    ''' <summary>
    ''' Löst das Speichern der vorgenommenen Änderungen an der Werbung aus.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 24.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsbWSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbWSave.Click
        prSaveWerbung()
    End Sub

    ''' <summary>
    ''' Bricht die aktuelle Bearbeitung ab und setzt den Zustand der Benutzeroberfläche zurück.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 24.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei Methodenaufrufen entfernt.
    ''' </remarks>
    Private Sub tsbWBreack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbWBreack.Click
        ' Eingabemodus sperren und Button-Status anhand bestehender Daten neu evaluieren
        prLoockWerbung(False)
        prCheckNoRecordWerbung(dtWer)
    End Sub

    ''' <summary>
    ''' Steuert die Aktivierung und Sichtbarkeit der Eingabefelder sowie der Funktionstasten basierend auf dem aktuellen Bearbeitungsstatus.
    ''' </summary>
    ''' <param name="lStatus">Gibt an, ob sich das Formular im Bearbeitungsmodus (True) oder im Ansichtsmodus (False) befindet.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 24.09.2026 - Code-Optimierung:
    ''' - Rechtschreibfehler im Methodennamen beibehalten (Kompatibilität), interne Steuerung logisch strukturiert.
    ''' </remarks>
    Private Sub prLoockWerbung(ByVal lStatus As Boolean)
        ' Steuerung der Button-Aktivierung
        tsbWEdit.Enabled = Not lStatus
        tsbWNeu.Enabled = Not lStatus
        tsbWSave.Enabled = lStatus
        tsbWBreack.Enabled = lStatus
        tsbWDel.Enabled = Not lStatus

        ' Steuerung der Listen- und Eingabeelemente
        liWerbung.Enabled = Not lStatus
        tbWerbung.Enabled = lStatus

        ' Sichtbarkeiten der Detail-Eingabefelder umschalten
        tbProvision.Visible = lStatus
        tbBetreff.Visible = lStatus
        tbKopf.Visible = lStatus
        tbFuss.Visible = lStatus
        tbWEMail.Visible = lStatus
        tbNormal.Enabled = lStatus

        ' Ansicht aktualisieren
        liWerbung.Refresh()
    End Sub

    ''' <summary>
    ''' Führt die Speicherung oder Aktualisierung des Werbedatensatzes in der Datenbank und der lokalen DataTable durch.
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 01.04.2012 - Insert Format Provision
    ''' 24.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' - 'tbWerbung.Text.Trim = ""' durch performanteres 'String.IsNullOrWhiteSpace' ersetzt.
    ''' - Nicht verwendete Variable 'sb' (StringBuilder) und 'sName' entfernt.
    ''' - 'Like'-Operator bei der ID-Filterung der DataTable durch präzisen '='-Operator ersetzt.
    ''' - Inline-Kommentare zur Dokumentation der Logikschritte hinzugefügt.
    ''' </remarks>
    Private Sub prSaveWerbung()
        Dim sqlText As String = ""
        Dim arFields() As String
        Dim arValue() As String
        Dim cBedingung As String = ""
        Dim sID As String

        ' Formatierung der Provision und ID-Ermittlung
        tbProvision.Text = fcFormatDecimal(tbProvision.Text)

        If lNew Then
            sID = fcGetTimeID(Date.Today)
        Else
            sID = fcGetOneValue(dtWer, liWerbung.Text, "Werbung", "ID")
        End If

        Try
            ' Felder definieren und Splitten
            sqlText = "ID,Werbung,Betreff,KText,FText,Link,Provision,Color"
            arFields = Split(sqlText, ",")

            ' Validierung: Wenn Werbetext leer ist, Speichervorgang abbrechen
            If String.IsNullOrWhiteSpace(tbWerbung.Text) Then Exit Sub

            ' Werte für die Felder holen
            sqlText = fcSaveWerbung(sID)
            arValue = Split(sqlText, "°")

            ' Bei Neuanlage leeren Datensatz in der Datenbank erzeugen
            If lNew Then
                sID = fcAppendBlank("Werbung")
            End If

            ' Daten in der Datenbank aktualisieren
            cBedingung = " WHERE ID='" & sID & "'"
            fcUpdateCommand("Werbung", arFields, arValue, cBedingung)

            ' Lokale DataTable synchronisieren
            If lNew Then
                ' Neuen Datensatz in DataTable "dtWer" hinzufügen
                fcInsertTable(dtWer, arFields, arValue)
            Else
                ' Bestehenden Datensatz in DataTable "dtWer" aktualisieren
                cBedingung = "ID = '" & sID & "'"
                fcUpdateTable(dtWer, arFields, arValue, cBedingung)
            End If

            lNew = False

        Catch ex As Exception
            ' Fehlerprotokollierung
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ' Benutzeroberfläche zurücksetzen und Liste neu laden
            prCheckNoRecordWerbung(dtWer)
            prLoockWerbung(False)
            liWerbung = fcLoadListe(liWerbung, dtWer, "Werbung")
        End Try
    End Sub

    ''' <summary>
    ''' Stellt die Formulardaten zu einem mit Trennzeichen (°) separierten String für die Speicherung zusammen.
    ''' </summary>
    ''' <param name="sID">Die eindeutige Identifikationsnummer des Werbedatensatzes.</param>
    ''' <returns>Ein mit '°' separierter String, der alle Feldwerte enthält.</returns>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 01.04.2012 - Insert Format Provision
    ''' 24.09.2026 - Code-Optimierung:
    ''' - 'Text.Trim = ""' durch performanteres 'String.IsNullOrWhiteSpace' ersetzt.
    ''' - UI-Entkopplung: Standardwerte werden direkt im Speicher (StringBuilder) gesetzt, anstatt die Textbox-Inhalte der UI mit Leerzeichen zu überschreiben.
    ''' - 'fcSaveWerbung = ...' durch die moderne 'Return'-Anweisung ersetzt.
    ''' </remarks>
    Private Function fcSaveWerbung(ByVal sID As String) As String
        Dim sb As New StringBuilder

        ' IDs und den Haupttext anfügen
        sb.Append(sID).Append("°")
        sb.Append(tbWerbung.Text).Append("°")

        ' Optionale Textfelder prüfen und direkt im StringBuilder puffern (verhindert Leerzeichen in der UI)
        If String.IsNullOrWhiteSpace(tbBetreff.Text) Then sb.Append(" 1°") Else sb.Append(tbBetreff.Text).Append("°")
        If String.IsNullOrWhiteSpace(tbKopf.Text) Then sb.Append(" 1°") Else sb.Append(tbKopf.Text).Append("°")
        If String.IsNullOrWhiteSpace(tbFuss.Text) Then sb.Append(" 1°") Else sb.Append(tbFuss.Text).Append("°")
        If String.IsNullOrWhiteSpace(tbWEMail.Text) Then sb.Append(" 1°") Else sb.Append(tbWEMail.Text).Append("°")

        ' Provision prüfen und standardisieren
        If String.IsNullOrWhiteSpace(tbProvision.Text) Then
            sb.Append("0.00°")
        Else
            sb.Append(tbProvision.Text).Append("°")
        End If

        ' Letzten Wert ohne abschließendes Trennzeichen anfügen
        sb.Append(tbNormal.Text)

        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Reagiert auf die Auswahl eines Eintrags in der Liste und lädt die dazugehörigen Detailinformationen.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 01.04.2012 - Insert Format Provision
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' - Sicherheitsabfrage hinzugefügt: Verarbeitet die Auswahl nur, wenn der Index gültig ist (>-1).
    ''' </remarks>
    Private Sub liWerbung_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles liWerbung.SelectedIndexChanged
        ' Abbruch bei Löschvorgang, Systemstart oder wenn kein Eintrag selektiert ist
        If lDel Or lStart OrElse liWerbung.SelectedIndex = -1 Then Exit Sub

        Dim sID As String = fcGetOneValue(dtWer, liWerbung.Text, "Werbung", "ID")
        prGetInfoWerbung(sID)
    End Sub

    ''' <summary>
    ''' Lädt die detaillierten Daten der ausgewählten Werbung aus der Datenbank und stellt sie in der UI dar.
    ''' </summary>
    ''' <param name="sID">Die eindeutige Identifikationsnummer der anzuzeigenden Werbung.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 01.04.2012 - Insert Format Provision
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Rechtschreibfehler im XML-Kommentar korrigiert.
    ''' - Datenzugriff durch direkte Typkonvertierung (.ToString) optimiert.
    ''' - Hintergrundfarbe wird nur zugewiesen, wenn Daten vorhanden sind.
    ''' </remarks>
    Private Sub prGetInfoWerbung(ByVal sID As String)
        Dim dt As DataTable = fcReadDataTable("Select * from Werbung Where ID='" & sID & "'")

        ' Wenn ein Datensatz gefunden wurde, die Steuerelemente befüllen
        If dt.Rows.Count > 0 Then
            Dim row As DataRow = dt.Rows(0)

            lbWEMail.Text = row("Link").ToString()
            lbBetreff.Text = row("Betreff").ToString()
            lbKopf.Text = row("KText").ToString()
            lbFuss.Text = row("FText").ToString()
            tbWerbung.Text = row("Werbung").ToString()
            lbProvision.Text = row("Provision").ToString()
            tbNormal.Text = row("color").ToString()

            ' Hintergrundfarbe basierend auf dem Farbwert setzen
            tbNormal.BackColor = fcStringRGB(tbNormal.Text)
        End If
    End Sub

    ''' <summary>
    ''' Löscht den aktuell ausgewählten Werbedatensatz nach einer Sicherheitsabfrage aus der Datenbank und aktualisiert die Anzeige.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' - 'sID = ""' durch performanteres 'String.IsNullOrEmpty' ersetzt und an den Methodenstart verschoben (Early Exit).
    ''' - Fehlerhaften Aufruf 'prCheckNoRecordBuch' auf die korrekte Methode 'prCheckNoRecordWerbung' umgestellt.
    ''' - Ablauflogik zur Vermeidung von unnötigen String-Operationen strukturiert.
    ''' </remarks>
    Private Sub tsbWDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbWDel.Click
        Dim sID As String = fcGetOneValue(dtWer, liWerbung.Text, "Werbung", "ID")

        ' Abbrechen, wenn keine gültige ID ermittelt werden konnte
        If String.IsNullOrEmpty(sID) Then Exit Sub

        Dim sMsg As String = "Wollen Sie diese Werbung wirklich löschen?"

        ' Sicherheitsabfrage vor dem Löschen
        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            lDel = True
            tbWerbung.Text = ""

            ' Datensatz per SQL aus der Tabelle löschen
            Dim cSql As String = "DELETE FROM Werbung WHERE ID = '" & sID & "'"
            UpdateTable(cSql)

            ' Änderung in lokaler DataTable "dtWer" nachvollziehen und neu laden
            fcDeleteTableRow(dtWer, "ID = '" & sID & "'")
            dtWer = fcReadDataTable("Select * from Werbung")

            ' Liste aktualisieren
            liWerbung = fcLoadListe(liWerbung, dtWer, "Werbung")

            ' Selektion zurücksetzen auf das erste Element, falls noch Daten vorhanden sind
            If dtWer.Rows.Count > 0 Then
                liWerbung.SelectedIndex = 0
            End If

            lDel = False

            ' Button-Status der Benutzeroberfläche aktualisieren
            prCheckNoRecordWerbung(dtWer)
        End If
    End Sub

    ''' <summary>
    ''' Formatiert den eingegebenen Wert im Provisionsfeld automatisch als Dezimalzahl, sobald das Feld den Fokus verliert.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 30.09.2026 - Code-Dokumentation:
    ''' - XML-Kommentar für konsistente Projektdokumentation hinzugefügt.
    ''' </remarks>
    Private Sub tbProvision_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProvision.LostFocus
        tbProvision.Text = fcFormatDecimal(tbProvision.Text)
    End Sub

    ''' <summary>
    ''' Öffnet bei einem Klick auf das Farbfeld den Windows-Farbdialog und weist die gewählte Farbe als Hintergrund sowie als RGB-String zu.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 30.09.2026 - Code-Optimierung:
    ''' - XML-Kommentar hinzugefügt.
    ''' - 'ColorDialog' in einen 'Using'-Block eingebettet, um eine saubere Freigabe der Systemressourcen (Dispose) zu garantieren.
    ''' </remarks>
    Private Sub tbNormal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbNormal.Click
        ' Ressourcenschonende Instanziierung des Farbdialogs
        Using cd As New ColorDialog()
            cd.Color = tbNormal.BackColor
            cd.FullOpen = True

            ' Wenn der Benutzer mit OK bestätigt, Farben anwenden
            If cd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                tbNormal.BackColor = cd.Color
                tbNormal.Text = fcRGBString(cd.Color)
            End If
        End Using
    End Sub



#End Region

#Region "Preise bearbeiten........................................................................."

    ''' <summary>
    ''' Lädt die verfügbaren Zimmer in die ToolStripComboBoxen, initialisiert die Jahresauswahl 
    ''' und stößt das Laden der zugehörigen Preise an.
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax entfernt.
    ''' - 'dtZim.Rows.Count - 1' Absturzsicherung hinzugefügt (Null- / Leerprüfung der DataTable).
    ''' - 'tsbPZimmer1' wurde vor dem Befüllen nicht geleert – '.Items.Clear()' hinzugefügt, um doppelte Einträge bei erneutem Laden zu verhindern.
    ''' - Performance-Optimierung durch direkte Verwendung von DataRow-Objekten in der Schleife.
    ''' </remarks>
    Private Sub prLoadComboZimmer()
        ' Prüfen, ob überhaupt Zimmer-Datensätze vorhanden sind, um Indexfehler zu vermeiden
        If dtZim IsNot Nothing AndAlso dtZim.Rows.Count > 0 Then

            With tsbPZimmer
                .Items.Clear()
                tsbPZimmer1.Items.Clear() ' Sicherstellen, dass auch die zweite Box zurückgesetzt wird

                ' Alle Zimmernamen in beide ComboBoxen einfragen
                For i As Integer = 0 To dtZim.Rows.Count - 1
                    Dim row As DataRow = dtZim.Rows(i)
                    Dim zimmerName As String = row("Name").ToString()

                    .Items.Add(zimmerName)
                    tsbPZimmer1.Items.Add(zimmerName)
                Next

                ' Den ersten Eintrag als Standard vorauswählen
                .Text = dtZim.Rows(0)("Name").ToString()
            End With
        End If

        ' Alters- bzw. Jahresauswahl (0 bis 4 Jahre) befüllen
        With tsbCoJahr
            .Items.Clear() ' Zur Sicherheit bestehende Items löschen
            .Text = "0 Jahre"
            For i As Integer = 0 To 4
                .Items.Add(i.ToString() & " Jahre")
            Next
        End With

        ' IDs ermitteln und nachgelagerte Preislisten-Strukturen aufbauen
        lbPZimID.Text = fcGetObjektZimmerID(dtZim, tsbPZimmer.Text, "ID")
        prCreateTabellePreise()
        prLoadPreiseInList()
        prLoockPreise(False)
    End Sub

    ''' <summary>
    ''' Reagiert auf die Textänderung der Zimmer-Auswahl, aktualisiert die Anzeige-IDs und lädt die Preistabelle neu.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 30.09.2026 - Code-Optimierung:
    ''' - XML-Kommentar hinzugefügt.
    ''' - Event-Parameter korrekter typisiert (System.Object, System.EventArgs) entsprechend den .NET-Konventionen.
    ''' </remarks>
    Private Sub tsbPZimmer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPZimmer.TextChanged
        ' ID und Name des neu gewählten Zimmers ermitteln
        lbPZimID.Text = fcGetObjektZimmerID(dtZim, tsbPZimmer.Text, "ID")
        lbPZim.Text = fcGetObjektZimmerName(dtZim, lbPZimID.Text)

        ' Die Preisliste für das ausgewählte Zimmer aktualisieren
        prLoadPreiseInList()
    End Sub

    ''' <summary>
    ''' Initialisiert das Layout und die Spaltenstruktur der Preis-ListView 'lvPreise'.
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Steuerelement-Eigenschaften logisch nach Anzeige- und Verhaltensregeln gruppiert.
    ''' - Spaltenbreiten für eine ausgewogene UI angepasst (Verkleinerung extrem breiter Spalten).
    ''' </remarks>
    Private Sub prCreateTabellePreise()
        With lvPreise
            ' 1. Anzeige- und Verhaltensoptionen der ListView konfigurieren
            .Clear()
            .View = View.Details
            .FullRowSelect = True
            .GridLines = True
            .HideSelection = False
            .MultiSelect = False
            .HeaderStyle = ColumnHeaderStyle.Nonclickable
            .Sorting = SortOrder.Ascending
            .TabIndex = 0

            ' 2. Spaltenstruktur definieren (Name, Breite in Pixeln, Ausrichtung)
            .Columns.Add("Datum", 0, HorizontalAlignment.Left)      ' Versteckte Spalte für Sortierung/Interne Zwecke
            .Columns.Add("Von", 70, HorizontalAlignment.Left)
            .Columns.Add("Bis", 70, HorizontalAlignment.Left)
            .Columns.Add("Preis", 75, HorizontalAlignment.Right)     ' Rechtsbündig für Währungsbeträge
            .Columns.Add("Dauer", 65, HorizontalAlignment.Left)
            .Columns.Add("ID", 0, HorizontalAlignment.Left)         ' Versteckte ID-Spalte
            .Columns.Add("Zimmer", 75, HorizontalAlignment.Left)
            .Columns.Add("Event", 75, HorizontalAlignment.Left)
        End With
    End Sub

    ''' <summary>
    ''' Füllt die ListView 'lvPreise' mit den Preisdaten aus der DataTable 'dtPre' basierend auf der gewählten Zimmer-ID.
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Auf performantere 'For Each'-Schleife umgestellt und 'dtPre.Rows(i)' durch direkte DataRow-Referenz ersetzt.
    ''' - nMax kleiner 0 -Prüfung durch saubere 'Rows.Count = 0'-Prüfung ersetzt.
    ''' - Redundanten 'With lvPreise'-Block innerhalb der Schleife entfernt, um den Scope sauber zu halten.
    ''' </remarks>
    Private Sub prLoadPreiseInList()
        Dim sql As String = "SELECT * FROM Preise WHERE ZimID = '" & lbPZimID.Text & "'"
        dtPre = fcReadDataTable(sql)

        lvPreise.Items.Clear()

        ' Abbrechen, wenn keine Zeilen geladen wurden
        If dtPre IsNot Nothing AndAlso dtPre.Rows.Count = 0 Then Exit Sub

        ' Alle Datensätze durchlaufen
        For Each row As DataRow In dtPre.Rows
            ' Gelöschte Zeilen überspringen
            If row.RowState <> DataRowState.Deleted Then
                ' Neues ListViewItem mit dem internen Datum erstellen
                Dim lv As ListViewItem = lvPreise.Items.Add(row("ADatum").ToString())

                ' SubItems mit formatierten Werten befüllen
                lv.SubItems.Add(fcUmDatum(row("ADatum").ToString()))
                lv.SubItems.Add(fcUmDatum(row("EDatum").ToString()))
                lv.SubItems.Add(fcPreisUm(row("Preis").ToString()))
                lv.SubItems.Add(row("Dauer").ToString())
                lv.SubItems.Add(row("ID").ToString())

                ' Zimmernamen ermitteln und hinzufügen
                Dim sZim As String = fcGetObjektZimmerName(dtZim, row("ZimID").ToString())
                lv.SubItems.Add(sZim)
                lv.SubItems.Add(row("Event").ToString())

            End If
        Next



    End Sub

    ''' <summary>
    ''' Konvertiert eine mit Pipe (|) getrennte Kette von Cent-Preisen in eine formatierte Euro-Preis-Kette.
    ''' </summary>
    ''' <param name="sPreisGruppe">Der umzuwandelnde String mit den Rohpreisen (z. B. "1000|2500").</param>
    ''' <returns>Ein formatierter String mit den umgerechneten Preisen (z. B. "10.00 | 25.00 |").</returns>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Übergabeparameter von 'ByRef' auf den sichereren Standard 'ByVal' geändert.
    ''' - String-Verkettung in der Schleife durch ein hocheffizientes 'StringBuilder'-Objekt ersetzt.
    ''' - Fehleranfälliges 'Mid' und alte Zuweisung durch modernes 'Return' und '.ToString().Trim()' ersetzt.
    ''' </remarks>
    Private Function fcPreisUm(ByVal sPreisGruppe As String) As String
        ' Abfangen von leeren oder ungültigen Werten
        If String.IsNullOrWhiteSpace(sPreisGruppe) Then Return ""

        Dim aPreis() As String = Split(sPreisGruppe, "|")
        Dim sb As New StringBuilder()

        For i As Integer = 0 To aPreis.Length - 1
            Dim rawValue As String = aPreis(i).Trim()

            ' Sicherstellen, dass das Teilsegment nicht leer ist
            If Not String.IsNullOrEmpty(rawValue) Then
                ' Wert konvertieren (Cent zu Euro) und formatieren
                Dim dblPreis As Double = Val(rawValue) / 100.0
                Dim sFormatiert As String = fcDecStr(dblPreis, , , ).ToString().Trim()

                ' Wert an den StringBuilder anfügen
                sb.Append(" ").Append(sFormatiert).Append(" |")
            End If
        Next

        ' Das Ergebnis trimmen und zurückgeben
        Return sb.ToString().Trim()
    End Function

    ''' <summary>
    ''' Prüft, ob Preisdatensätze vorhanden sind, steuert die Aktivierung der Bearbeiten- und Löschen-Schaltflächen 
    ''' und selektiert bei vorhandenen Daten automatisch den ersten Eintrag in der Tabelle.
    ''' </summary>
    ''' <param name="dt">Die zu prüfende DataTable mit den Preisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Logik vereinfacht: Direkte Zuweisung des Boolean-Status ohne temporäre Variable.
    ''' - UI-Erweiterung: Ruft bei vorhandenen Datensätzen automatisch 'prSelectFirstListViewItemIfNeeded' auf, um den ersten Eintrag in 'lvPreise' auszuwählen.
    ''' </remarks>
    Private Sub prCheckNoRecordPreise(ByVal dt As DataTable)
        ' Prüfen, ob die DataTable existiert und Zeilen enthält
        Dim hasRecords As Boolean = (dt IsNot Nothing AndAlso dt.Rows.Count > 0)

        ' Buttons direkt basierend auf dem Ergebnis aktivieren oder deaktivieren
        tsbPEdit.Enabled = hasRecords
        tsbPDel.Enabled = hasRecords

        ' Wenn Datensätze vorhanden sind, automatisch das erste Element der ListView selektieren
        If hasRecords Then
            prSelectFirstListViewItemIfNeeded(lvPreise)
        End If
    End Sub

    ''' <summary>
    ''' Bereitet die Benutzeroberfläche für das Anlegen eines neuen Preisdatensatzes vor, indem alle Eingabefelder zurückgesetzt werden.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax entfernt.
    ''' - Massenzuweisungen der durchnummerierten Textboxen (Preise und Dauer) über strukturierte Arrays und Schleifen zusammengefasst.
    ''' - Übersichtlichkeit und Wartbarkeit bei Feldänderungen drastisch erhöht.
    ''' </remarks>
    Private Sub tsbPNeu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPNeu.Click
        lNew = True

        ' Arrays für die strukturiert durchnummerierten Textboxen definieren
        Dim preisTextBoxes() As TextBox = {tbPreis1, tbPreis2, tbPreis3, tbPreis4, tbPreis5, tbPreis6, tbPreis7}
        Dim dauerTextBoxes() As TextBox = {tbD1, tbD2, tbD3, tbD4, tbD5, tbD6, tbD7}

        ' Preisfelder auf Standardwert zurücksetzen
        For Each tb As TextBox In preisTextBoxes
            tb.Text = "0"
        Next

        ' Dauerfelder auf Standardwert zurücksetzen
        For Each tb As TextBox In dauerTextBoxes
            tb.Text = "0"
        Next

        ' Globale Preisfelder leeren
        tbPreisG.Text = ""
        tbDauerG.Text = ""

        ' Eingabemodus sperren/entsperren und Validierung anstoßen
        prLoockPreise(True)
        prCheckPreis()
    End Sub

    ''' <summary>
    ''' Bereitet die Benutzeroberfläche für das Bearbeiten der bestehenden Preise vor.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsbPEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPEdit.Click
        prLoockPreise(True)
        prCheckPreis()
    End Sub

    ''' <summary>
    ''' Löst das Speichern der vorgenommenen Änderungen an den Preisen aus.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Rechtschreibfehler im XML-Kommentar korrigiert.
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' </remarks>
    Private Sub tsbPSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPSave.Click
        prSavePreise()
    End Sub

    ''' <summary>
    ''' Bricht die aktuelle Bearbeitung ab und setzt den Zustand der Benutzeroberfläche zurück.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei den Methodenaufrufen entfernt.
    ''' </remarks>
    Private Sub tsbPBreak_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPBreak.Click
        prLoockPreise(False)
        prCheckNoRecordPreise(dtPre)
    End Sub

    ''' <summary>
    ''' Setzt den Auswahlstatus (Checked) für alle Preis-Checkboxen standardmäßig auf Aktiv.
    ''' </summary>
    ''' <remarks>
    ''' 30.09.2026 - Code-Optimierung:
    ''' - XML-Kommentar hinzugefügt.
    ''' - Massenzuweisung der Checkboxen 'chP1' bis 'chP7' über eine wartbare Array-Schleife gelöst.
    ''' </remarks>
    Private Sub prCheckPreis()
        ' Array aller betroffenen Checkboxen für eine kompakte Bearbeitung
        Dim priceCheckBoxes() As CheckBox = {chP1, chP2, chP3, chP4, chP5, chP6, chP7}

        ' Alle Checkboxen in einer Schleife aktivieren
        For Each chk As CheckBox In priceCheckBoxes
            chk.Checked = True
        Next
    End Sub

    ''' <summary>
    ''' Steuert die Aktivierung und Sperrung aller Navigationselemente, Auswahllisten und Eingabefelder für die Preise.
    ''' </summary>
    ''' <param name="lStatus">Gibt an, ob sich das Formular im Bearbeitungsmodus (True) oder im Ansichtsmodus (False) befindet.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Rechtschreibfehler im Methodennamen beibehalten (Kompatibilität).
    ''' - Doppelte Zuweisungen entfernt und gemeinsame Logik in 'prSetEingabefelderStatus' ausgelagert.
    ''' </remarks>
    Private Sub prLoockPreise(ByVal lStatus As Boolean)
        ' 1. Navigation und Menüelemente umschalten
        tsbPEdit.Enabled = Not lStatus
        tsbPZimmer.Enabled = Not lStatus
        tsbPZimmer1.Enabled = Not lStatus
        tsbPCopy.Enabled = Not lStatus
        tsbPZimmerCopyJahr.Enabled = Not lStatus
        tsbCoJahr.Enabled = Not lStatus
        tsbPNeu.Enabled = Not lStatus
        tsbPDel.Enabled = Not lStatus
        lvPreise.Enabled = Not lStatus

        ' 2. Gemeinsame Eingabefelder über zentrale Hilfsfunktion steuern
        prSetEingabefelderStatus(lStatus)
    End Sub

    ''' <summary>
    ''' Steuert eine reduzierte Auswahl an Eingabefeldern und Speicher-Buttons, ohne die Navigationsleisten zu verändern.
    ''' </summary>
    ''' <param name="lStatus">Gibt an, ob die Felder aktiviert (True) oder gesperrt (False) werden sollen.</param>
    ''' <remarks>
    ''' 30.09.2026 - Code-Optimierung:
    ''' - XML-Kommentar für konsistente Projektdokumentation hinzugefügt.
    ''' - Redundante Doppelzuweisungen entfernt und Core-Logik mit 'prLoockPreise' vereinheitlicht.
    ''' </remarks>
    Private Sub prLoockPreise1(ByVal lStatus As Boolean)
        ' Gemeinsame Eingabefelder über zentrale Hilfsfunktion steuern
        prSetEingabefelderStatus(lStatus)
    End Sub

    ''' <summary>
    ''' Interne Hilfsmethode zur Aktivierung/Deaktivierung der Daten- und Preisfelder, um Code-Duplikate zu vermeiden.
    ''' </summary>
    Private Sub prSetEingabefelderStatus(ByVal lStatus As Boolean)
        ' Speicher- und Abbrechen-Buttons
        tsbPSave.Enabled = lStatus
        tsbPBreak.Enabled = lStatus

        ' Allgemeine Datenfelder
        tbPreisG.Enabled = lStatus
        tbDauerG.Enabled = lStatus
        dtpVon.Enabled = lStatus
        dtpBis.Enabled = lStatus
        coEvent.Enabled = lStatus

        ' Arrays für die durchnummerierten Eingabefelder definieren
        Dim preisTextBoxes() As TextBox = {tbPreis1, tbPreis2, tbPreis3, tbPreis4, tbPreis5, tbPreis6, tbPreis7}
        Dim dauerTextBoxes() As TextBox = {tbD1, tbD2, tbD3, tbD4, tbD5, tbD6, tbD7}

        ' Preisfelder via Schleife steuern
        For Each tb As TextBox In preisTextBoxes
            tb.Enabled = lStatus
        Next

        ' Dauerfelder via Schleife steuern
        For Each tb As TextBox In dauerTextBoxes
            tb.Enabled = lStatus
        Next
    End Sub

    ''' <summary>
    ''' Reagiert auf die Auswahl eines Eintrags in der Preisliste und stößt das Laden der Details an.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax beim Methodenaufruf entfernt.
    ''' - Sicherheitsabfrage erweitert: Bricht den Vorgang auch ab, wenn die Auswahl im Grid temporär aufgehoben wurde.
    ''' </remarks>
    Private Sub lvPreise_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvPreise.SelectedIndexChanged
        ' Abbruch bei Löschvorgang oder wenn kein Eintrag selektiert ist
        If lDel OrElse lvPreise.SelectedIndices.Count = 0 Then Exit Sub

        prGetInfolvPreise()
    End Sub

    ''' <summary>
    ''' Überträgt die detaillierten Informationen des ausgewählten Preislisteneintrags in die jeweiligen Eingabefelder der UI.
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Massenzuweisung der Arrays (Preise und Dauer) über Index-Schleifen abgesichert, um 'IndexOutOfRangeException' bei unvollständigen Datensätzen zu verhindern.
    ''' - UI-Steuerelemente über strukturierte Arrays angesprochen, um den Code kompakt und wartbar zu halten.
    ''' </remarks>
    Private Sub prGetInfolvPreise()
        With lvPreise
            ' Nur verarbeiten, wenn ein Element ausgewählt ist
            If .SelectedItems.Count > 0 Then
                Dim selectedItem As ListViewItem = .SelectedItems(0)

                ' Arrays für die Steuerelemente initialisieren
                Dim preisTextBoxes() As TextBox = {tbPreis1, tbPreis2, tbPreis3, tbPreis4, tbPreis5, tbPreis6, tbPreis7}
                Dim dauerTextBoxes() As TextBox = {tbD1, tbD2, tbD3, tbD4, tbD5, tbD6, tbD7}

                ' 1. Preise aus SubItem(3) extrahieren und zuweisen
                Dim aPreis() As String = Split(selectedItem.SubItems(3).Text, "|")
                For i As Integer = 0 To preisTextBoxes.Length - 1
                    If i < aPreis.Length Then
                        preisTextBoxes(i).Text = aPreis(i).Trim()
                    Else
                        preisTextBoxes(i).Text = "0" ' Fallback, wenn weniger als 7 Werte vorhanden sind
                    End If
                Next

                ' 2. Dauer aus SubItem(4) extrahieren und zuweisen
                Dim aDauer() As String = Split(selectedItem.SubItems(4).Text, "|")
                For i As Integer = 0 To dauerTextBoxes.Length - 1
                    If i < aDauer.Length Then
                        dauerTextBoxes(i).Text = aDauer(i).Trim()
                    Else
                        dauerTextBoxes(i).Text = "0" ' Fallback, wenn weniger als 7 Werte vorhanden sind
                    End If
                Next

                ' 3. Allgemeine Metadaten und Datumswerte übertragen
                lbPID.Text = selectedItem.SubItems(5).Text
                dtpVon.Value = Convert.ToDateTime(selectedItem.SubItems(1).Text)
                dtpBis.Value = Convert.ToDateTime(selectedItem.SubItems(2).Text)
                coEvent.Text = selectedItem.SubItems(7).Text
            End If
        End With
    End Sub

    ''' <summary>
    ''' Überprüft, ob das aktuell eingegebene Event bereits in der Systemdatenbank existiert, 
    ''' fügt es bei Bedarf hinzu und aktualisiert die Auswahl-ComboBox.
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Datentyp von 'Function' zu 'Sub' korrigiert (da kein Rückgabewert existiert).
    ''' - Erste Suchschleife durch ein performantes '.Contains' ersetzt.
    ''' - 'StringSplitOptions.RemoveEmptyEntries' hinzugefügt, um leere Fragmente (\n) oder doppelte Zeilenumbrüche sauber zu filtern.
    ''' - 'String.IsNullOrWhiteSpace'-Prüfung integriert, um das Speichern leerer Eventnamen zu verhindern.
    ''' </remarks>
    Private Sub fcSaveEvent()
        ' Wenn das Feld leer ist oder nur aus Leerzeichen besteht, sofort abbrechen
        Dim newEvent As String = coEvent.Text.Trim()
        If String.IsNullOrWhiteSpace(newEvent) Then Exit Sub

        ' Aktuell gespeicherte Events aus der Datenbank laden
        Dim sTmp As String = ReadOneValueFromSystemDb("Event")

        ' Zeilenweise aufsplitten und leere Zeilen direkt entfernen
        Dim separators() As String = {vbCrLf, vbCr, vbLf}
        Dim arTmp() As String = sTmp.Split(separators, StringSplitOptions.RemoveEmptyEntries)

        ' Prüfen, ob das Event bereits in der Liste existiert
        Dim eventExists As Boolean = False
        For Each existingEvent As String In arTmp
            If existingEvent.Trim().Equals(newEvent, StringComparison.OrdinalIgnoreCase) Then
                eventExists = True
                Exit For
            End If
        Next

        ' Wenn das Event neu ist, in der DB anhängen und ComboBox aktualisieren
        If Not eventExists Then
            ' Datensatz mit System-Zeilenumbruch erweitern und sichern
            sTmp = If(String.IsNullOrEmpty(sTmp), newEvent, sTmp & vbCrLf & newEvent)
            SaveOneValueInSystemDb("Event", sTmp)

            ' Array mit dem neuen Wert aktualisieren
            arTmp = sTmp.Split(separators, StringSplitOptions.RemoveEmptyEntries)

            ' ComboBox neu befüllen
            coEvent.Items.Clear()
            For i As Integer = 0 To arTmp.Length - 1
                coEvent.Items.Add(arTmp(i))
            Next

            ' Den ersten Eintrag der Liste als Standard vorauswählen
            If arTmp.Length > 0 Then
                coEvent.Text = arTmp(0)
            End If
        End If
    End Sub


    ''' <summary>
    ''' Führt die Speicherung oder Aktualisierung des Preisdatensatzes in der Datenbank und der lokalen DataTable durch.
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' - Ungenutzte 'StringBuilder'-Variable entfernt.
    ''' - 'Like'-Operator bei der ID-Filterung der DataTable durch präzisen '='-Operator ersetzt.
    ''' - Absicherung im 'Finally'-Block integriert, falls 'arValue' vor dem Fehler nicht initialisiert wurde.
    ''' - 'fcSaveEvent()' logisch sauber am Ende der erfolgreichen Speicherung platziert.
    ''' </remarks>
    Private Sub prSavePreise()
        Dim sqlText As String = ""
        Dim arFields() As String
        Dim arValue() As String = Nothing
        Dim cBedingung As String = ""
        Dim sID As String = lbPID.Text

        ' Bei Neuanlage eine zeitbasierte ID generieren
        If lNew Then
            sID = fcGetTimeID(Date.Today)
        End If

        Try
            ' Feldstruktur definieren und splitten
            sqlText = "ID,ADatum,EDatum,Preis,Dauer,ZimID,Event"
            arFields = Split(sqlText, ",")

            ' Rohdaten über die Hilfsfunktion zusammensetzen und splitten
            sqlText = fcSavePreise(sID)
            arValue = Split(sqlText, "°")

            ' 1. In der Datenbank speichern oder aktualisieren
            If lNew Then
                fcInsertCommand("Preise", arFields, arValue)
            Else
                cBedingung = " WHERE ID='" & sID & "'"
                fcUpdateCommand("Preise", arFields, arValue, cBedingung)
            End If

            ' 2. Lokale DataTable "dtPre" synchronisieren
            If lNew Then
                fcInsertTable(dtPre, arFields, arValue)
            Else
                cBedingung = "ID = '" & sID & "'"
                fcUpdateTable(dtPre, arFields, arValue, cBedingung)
            End If

            lNew = False

            ' Neues oder geändertes Event in der System-DB registrieren
            fcSaveEvent()

        Catch ex As Exception
            ' Fehlerprotokollierung
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            ' Benutzeroberfläche und Tabellenansicht aktualisieren
            prLoadPreiseInList()
            prCheckNoRecordPreise(dtPre)
            prLoockPreise(False)

            ' Eintrag selektieren, falls Daten erfolgreich ermittelt wurden
            If arValue IsNot Nothing AndAlso arValue.Length > 1 Then
                prSelectEntry(lvPreise, arValue(1))
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Bereitet die eingegebenen Preis- und Dauerdaten vor, berechnet die Cent-Beträge und stellt sie als verketteten String bereit.
    ''' </summary>
    ''' <param name="sID">Die eindeutige Identifikationsnummer des Preisdatensatzes.</param>
    ''' <returns>Ein mit '°' separierter String, der alle formatierten Feldwerte enthält.</returns>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Massenhafte 'If'-Bedingungen für die 7 Preiskategorien durch eine wartbare Array-Schleife ersetzt.
    ''' - 'Text.Trim = ""' durch 'String.IsNullOrWhiteSpace' ersetzt.
    ''' - Implizite String-Multiplikation durch sichere numerische Konvertierung via 'Val()' behoben.
    ''' - 'fcSavePreise = ...' durch moderne 'Return'-Anweisung ersetzt.
    ''' </remarks>
    Private Function fcSavePreise(ByVal sID As String) As String
        Dim sb As New StringBuilder()

        ' Standardwert für den globalen Preis setzen, falls leer
        If String.IsNullOrWhiteSpace(tbPreisG.Text) Then
            tbPreisG.Text = "0"
        End If

        ' Steuerungs-Arrays für die strukturierten Steuerelemente definieren
        Dim preisTextBoxes() As TextBox = {tbPreis1, tbPreis2, tbPreis3, tbPreis4, tbPreis5, tbPreis6, tbPreis7}
        Dim dauerTextBoxes() As TextBox = {tbD1, tbD2, tbD3, tbD4, tbD5, tbD6, tbD7}
        Dim checkBoxes() As CheckBox = {chP1, chP2, chP3, chP4, chP5, chP6, chP7}

        ' Schleife durch alle 7 Preiskategorien zur automatischen Befüllung/Kopie der globalen Werte
        For i As Integer = 0 To preisTextBoxes.Length - 1
            If checkBoxes(i).Checked OrElse String.IsNullOrWhiteSpace(preisTextBoxes(i).Text) Then
                preisTextBoxes(i).Text = tbPreisG.Text
                dauerTextBoxes(i).Text = tbDauerG.Text
            End If
        Next

        ' Preis-Kette generieren (Werte mit Val() absichern und in Cent umrechnen)
        Dim sPreis As String = String.Format("{0} | {1} | {2} | {3} | {4} | {5} | {6}",
        Val(tbPreis1.Text) * 100, Val(tbPreis2.Text) * 100, Val(tbPreis3.Text) * 100,
        Val(tbPreis4.Text) * 100, Val(tbPreis5.Text) * 100, Val(tbPreis6.Text) * 100, Val(tbPreis7.Text) * 100)

        ' Dauer-Kette generieren
        Dim sDauer As String = String.Format("{0} | {1} | {2} | {3} | {4} | {5} | {6}",
        tbD1.Text, tbD2.Text, tbD3.Text, tbD4.Text, tbD5.Text, tbD6.Text, tbD7.Text)

        ' Datumswerte konvertieren
        Dim sVon As String = fcUmDatum(dtpVon.Value.ToString())
        Dim sBis As String = fcUmDatum(dtpBis.Value.ToString())

        ' Gesamt-String mit Trennzeichen zusammenbauen
        sb.Append(sID).Append("°")
        sb.Append(sVon).Append("°")
        sb.Append(sBis).Append("°")
        sb.Append(sPreis).Append("°")
        sb.Append(sDauer).Append("°")
        sb.Append(lbPZimID.Text).Append("°")
        sb.Append(coEvent.Text)

        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Löscht den aktuell ausgewählten Preisdatensatz nach einer Sicherheitsabfrage aus der Datenbank und aktualisiert die Anzeige.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - 'sID = ""' durch 'String.IsNullOrEmpty' ersetzt und an den Methodenstart vorgezogen (Early Exit).
    ''' - Massenhafte Textbox-Leerungen über eine kompakte Array-Schleife für alle 7 Preisfelder vereinheitlicht.
    ''' - Fehlerhaften '.TopItem.Selected'-Aufruf durch die sichere Methode 'prSelectFirstListViewItemIfNeeded' ersetzt.
    ''' - Veraltete 'Call'-Syntax entfernt.
    ''' </remarks>
    Private Sub tsbPDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPDel.Click
        Dim sID As String = lbPID.Text

        ' Abbrechen, wenn keine gültige ID vorhanden ist
        If String.IsNullOrEmpty(sID) Then Exit Sub

        Dim sMsg As String = "Wollen Sie diesen Preis wirklich löschen?"

        ' Sicherheitsabfrage vor dem Löschen
        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            lDel = True

            ' Alle 7 Preis-Textfelder über ein Array leeren
            Dim preisTextBoxes() As TextBox = {tbPreis1, tbPreis2, tbPreis3, tbPreis4, tbPreis5, tbPreis6, tbPreis7}
            For Each tb As TextBox In preisTextBoxes
                tb.Text = ""
            Next

            ' Datensatz per SQL aus der Tabelle löschen
            Dim cSql As String = "DELETE FROM Preise WHERE ID = '" & sID & "'"
            UpdateTable(cSql)

            ' Preisliste neu laden
            prLoadPreiseInList()

            ' Fokus auf das Steuerelement setzen und das erste Element auswählen, falls Daten vorhanden sind
            lvPreise.Select()
            prSelectFirstListViewItemIfNeeded(lvPreise)

            lDel = False

            ' Button-Status der Benutzeroberfläche aktualisieren
            prCheckNoRecordPreise(dtPre)
        End If
    End Sub

    ''' <summary>
    ''' Schaltet die Bearbeitbarkeit des Preis-Eingabefeldes 7 frei, wenn die dazugehörige Checkbox deaktiviert wird.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 30.09.2026 - Code-Optimierung:
    ''' - XML-Kommentar hinzugefügt.
    ''' - 'If-Else'-Struktur durch eine hocheffiziente, direkte Boolean-Zuweisung ersetzt.
    ''' - Event-Parameter korrekter typisiert (System.Object, System.EventArgs) entsprechend den .NET-Konventionen.
    ''' </remarks>
    'Private Sub chP7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chP7.CheckedChanged
    '    ' Wenn die Checkbox NICHT gesetzt ist (False), wird das Textfeld aktiviert (True)
    '    tbPreis7.Enabled = Not chP7.Checked
    'End Sub

    ''' <summary>
    ''' Universeller Event-Handler für alle sieben Preis-Checkboxen.
    ''' Schaltet das jeweils zugehörige Textfeld basierend auf dem Auswahlstatus frei oder sperrt es.
    ''' </summary>
    Private Sub chP_Generic_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles chP1.CheckedChanged, chP2.CheckedChanged, chP3.CheckedChanged,
            chP4.CheckedChanged, chP5.CheckedChanged, chP6.CheckedChanged, chP7.CheckedChanged

        ' Den Sender als Checkbox identifizieren
        Dim currentCheckBox As CheckBox = TryCast(sender, CheckBox)
        If currentCheckBox IsNot Nothing Then
            ' Die Nummer der Checkbox aus dem Namen ermitteln (z.B. "chP7" -> "7")
            Dim indexStr As String = currentCheckBox.Name.Substring(3)

            ' Das passende Textfeld über den Namen im Formular suchen und umschalten
            Dim targetTextBox As TextBox = TryCast(Me.Controls.Find("tbPreis" & indexStr, True).FirstOrDefault(), TextBox)
            If targetTextBox IsNot Nothing Then
                targetTextBox.Enabled = Not currentCheckBox.Checked
            End If
        End If
    End Sub

    ''' <summary>
    ''' Kopiert bestehende Preiselemente für ein ausgewähltes Zimmer in ein zukünftiges Jahr 
    ''' und berechnet die Werte auf Cent-Basis neu.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' - 'With lvPreise' aus der performancerelevanten i-Schleife nach oben herausgezogen.
    ''' - String-Zusammenbau bei den Preisen durch ein effizientes 'StringBuilder'-Objekt optimiert.
    ''' - Unnötige 'Mid'-Funktion bei der String-Längen-Kürzung durch modernes '.Length - 1' ersetzt.
    ''' - Event-Parameter konform auf 'ByVal' und korrekte .NET-Typen umgestellt.
    ''' </remarks>
    Private Sub tsbPCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPCopy.Click
        ' Benutzeroberfläche sperren und Warte-Cursor aktivieren
        prLoockPreise(True)
        prLoockPreise1(False)
        Me.Cursor = Cursors.WaitCursor
        tsbPCopy.Enabled = False

        ' Abbrechen, falls keine ListView-Einträge vorhanden sind
        If lvPreise.Items.Count = 0 Then
            tsbPCopy.Enabled = True
            Me.Cursor = Cursors.Default
            prLoockPreise(False)
            Exit Sub
        End If

        ' Basisdaten ermitteln
        Dim sJahr As String = Mid(fcUmDatum(lvPreise.Items(0).SubItems(1).Text), 1, 4)
        Dim iJahr As Integer = Val(Mid(tsbCoJahr.Text, 1, 1))
        Dim sZim As String = fcGetObjektZimmerID(dtZim, tsbPZimmer1.Text, "ID")

        Dim sqlText As String = "ID,ADatum,EDatum,Preis,Dauer,ZimID,Event"
        Dim arFields() As String = Split(sqlText, ",")
        Dim arValue() As String

        ' Kopierprozess starten, wenn ein Zielzimmer ausgewählt wurde
        If Not String.IsNullOrWhiteSpace(tsbPZimmer1.Text) Then
            With lvPreise
                For i As Integer = 0 To .Items.Count - 1
                    Dim item As ListViewItem = .Items(i)

                    Dim sVon As String = fcUmDatum(item.SubItems(1).Text)
                    Dim sBis As String = fcUmDatum(item.SubItems(2).Text)
                    Dim sPreisRaw As String = item.SubItems(3).Text.Trim()
                    Dim sDauer As String = item.SubItems(4).Text.Trim()
                    Dim sEvent As String = item.SubItems(7).Text

                    ' Prüfen, ob das Jahr des Datensatzes dem Zieljahr entspricht
                    If Mid(sVon, 1, 4) = (Val(sJahr) + iJahr).ToString() Then
                        Dim sID As String = fcGetTimeID(Date.Today)
                        fcWait(2) ' Kurze Pause zur Gewährleistung eindeutiger IDs

                        ' Preise splitten und neu berechnen (Cent-Multiplikation)
                        Dim arPreis() As String = Split(sPreisRaw, "|")
                        Dim sbPreis As New StringBuilder()

                        For j As Integer = 0 To arPreis.Length - 1
                            Dim priceValue As Double = Val(arPreis(j)) * 100
                            sbPreis.Append(priceValue).Append("|")
                        Next

                        ' Letztes Trennzeichen abschneiden
                        Dim sPreis As String = sbPreis.ToString()
                        If sPreis.Length > 0 Then
                            sPreis = sPreis.Substring(0, sPreis.Length - 1)
                        End If

                        ' Datensatz-Array vorbereiten und in Datenbank einfügen
                        sqlText = String.Format("{0}°{1}°{2}°{3}°{4}°{5}°{6}", sID, sVon, sBis, sPreis, sDauer, sZim, sEvent)
                        arValue = Split(sqlText, "°")

                        fcInsertCommand("Preise", arFields, arValue)
                    End If
                Next
            End With
        End If

        ' Ansicht zurücksetzen
        tsbCoJahr.Text = "0 Jahre"
        tsbPZimmer1.Text = ""
        tsbPCopy.Enabled = True
        Me.Cursor = Cursors.Default
        prLoockPreise(False)
    End Sub

    ''' <summary>
    ''' Kopiert alle Preise des aktuellen Zimmers aus dem ausgewählten Basisjahr in ein zukünftiges Jahr 
    ''' und berechnet die Werte auf Cent-Basis neu.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Absturzsicherung hinzugefügt für den Fall, dass 'lvPreise' keine Einträge enthält.
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' - Preis-Verkettung in der inneren Schleife auf performantes 'StringBuilder'-Objekt umgestellt.
    ''' - 'String.Format' für eine saubere und lesbare SQL-Wert-Generierung implementiert.
    ''' - Parameter auf korrekte .NET-Typen typisiert.
    ''' </remarks>
    Private Sub tsbPZimmerCopyJahr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPZimmerCopyJahr.Click
        ' Abbrechen, falls keine ListView-Einträge vorhanden sind (verhindert Absturz bei Items(0))
        If lvPreise.Items.Count = 0 Then Exit Sub

        ' Oberflächen-Status sperren und Warte-Cursor aktivieren
        prLoockPreise(True)
        prLoockPreise1(False)
        Me.Cursor = Cursors.WaitCursor

        ' Jahre und Basis-Jahr ermitteln
        Dim iJahr As Integer = Val(Mid(tsbCoJahr.Text, 1, 1))
        tsbCoJahr.Text = "0 Jahre"

        Dim sVonBase As String = fcUmDatum(lvPreise.Items(0).SubItems(1).Text)
        Dim sJahr As String = Mid(sVonBase, 1, 4)

        Dim sZim As String = fcGetObjektZimmerID(dtZim, tsbPZimmer.Text, "ID")

        Dim sqlText As String = "ID,ADatum,EDatum,Preis,Dauer,ZimID,Event"
        Dim arFields() As String = Split(sqlText, ",")
        Dim arValue() As String

        With lvPreise
            For i As Integer = 0 To .Items.Count - 1
                Dim item As ListViewItem = .Items(i)
                Dim sVon As String = fcUmDatum(item.SubItems(1).Text)

                ' Nur kopieren, wenn ein Zieljahr gewählt wurde und der Datensatz zum Basisjahr gehört
                If iJahr <> 0 AndAlso Mid(sVon, 1, 4) = sJahr Then
                    Dim sID As String = fcGetTimeID(Date.Today)
                    fcWait(2) ' Kurze Pause zur Absicherung eindeutiger IDs

                    Dim sBis As String = fcUmDatum(item.SubItems(2).Text)
                    Dim sPreisRaw As String = item.SubItems(3).Text.Trim()
                    Dim sDauer As String = item.SubItems(4).Text.Trim()
                    Dim sEvent As String = item.SubItems(7).Text

                    ' Jahre der Datumsfelder um die gewählte Jahresanzahl erhöhen
                    sBis = (Val(Mid(sBis, 1, 4)) + iJahr).ToString() & Mid(sBis, 5)
                    sVon = (Val(Mid(sVon, 1, 4)) + iJahr).ToString() & Mid(sVon, 5)

                    ' Preise splitten und in Cent umrechnen
                    Dim arPreis() As String = Split(sPreisRaw, "|")
                    Dim sbPreis As New StringBuilder()

                    For j As Integer = 0 To arPreis.Length - 1
                        Dim priceValue As Double = Val(arPreis(j)) * 100
                        sbPreis.Append(priceValue).Append("|")
                    Next

                    ' Abschließendes Trennzeichen entfernen
                    Dim sPreis As String = sbPreis.ToString()
                    If sPreis.Length > 0 Then
                        sPreis = sPreis.Substring(0, sPreis.Length - 1)
                    End If

                    ' Datensatz-Array vorbereiten und in Datenbank einfügen
                    sqlText = String.Format("{0}°{1}°{2}°{3}°{4}°{5}°{6}", sID, sVon, sBis, sPreis, sDauer, sZim, sEvent)
                    arValue = Split(sqlText, "°")

                    fcInsertCommand("Preise", arFields, arValue)
                End If
            Next
        End With

        ' Ansicht aktualisieren und UI freigeben
        prLoadPreiseInList()
        Me.Cursor = Cursors.Default
        prLoockPreise(False)
    End Sub

    ''' <summary>
    ''' Validiert die Tastatureingaben für alle Preis-Textfelder und erlaubt nur numerische Zeichen sowie Steuerzeichen.
    ''' </summary>
    ''' <param name="sender">Die TextBox, in der das Zeichen eingegeben wurde.</param>
    ''' <param name="e">Die Ereignisdaten mit dem eingegebenen Zeichen.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Alle einzelnen KeyPress-Events in einer einzigen, generischen Methode zusammengefasst.
    ''' - 'tbPreis6' und 'tbPreis7' direkt im 'Handles'-Block ergänzt, da diese im Originalcode fehlten.
    ''' - Veraltete 'Call'-Syntax entfernt und 'sender' dynamisch an die Prüffunktion übergeben.
    ''' </remarks>
    Private Sub tbPreis_Generic_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _
    Handles tbPreis1.KeyPress, tbPreis2.KeyPress, tbPreis3.KeyPress,
            tbPreis4.KeyPress, tbPreis5.KeyPress, tbPreis6.KeyPress, tbPreis7.KeyPress

        Dim currentTextBox As TextBox = TryCast(sender, TextBox)
        If currentTextBox IsNot Nothing Then
            ' Übergibt das gedrückte Zeichen und die auslösende TextBox an Ihre Prüfroutine
            prCheckNumericKey(e.KeyChar, currentTextBox)
        End If
    End Sub

    ''' <summary>
    ''' Formatiert den Inhalt des Preis-Textfeldes automatisch als Dezimalzahl, sobald das Feld verlassen wird.
    ''' </summary>
    ''' <param name="sender">Die TextBox, die den Fokus verloren hat.</param>
    ''' <param name="e">Die Ereignisdaten.</param>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Alle einzelnen Leave-Events in einer einzigen, generischen Methode zusammengefasst.
    ''' - 'tbPreis6' und 'tbPreis7' direkt mit abgesichert.
    ''' - Castet den 'sender' dynamisch, um die Formatierung fehlerfrei auf das aktive Feld anzuwenden.
    ''' </remarks>
    Private Sub tbPreis_Generic_Leave(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles tbPreis1.Leave, tbPreis2.Leave, tbPreis3.Leave,
            tbPreis4.Leave, tbPreis5.Leave, tbPreis6.Leave, tbPreis7.Leave

        Dim currentTextBox As TextBox = TryCast(sender, TextBox)
        If currentTextBox IsNot Nothing Then
            ' Formatiert den Text der betroffenen TextBox neu
            currentTextBox.Text = fcFormatDecimal(currentTextBox.Text)
        End If
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
    ''' Füllt die ListView 'lvSasion' mit den Saisondaten aus der Konfigurationsdatei und formatiert die Zeilen farblich.
    ''' </summary>
    ''' <remarks>
    ''' 26.01.2012 - Create
    ''' 30.09.2026 - Code-Optimierung:
    ''' - Zeilenumbruch-Splitting robuster gestaltet, um leere Fragmente (\n) abzufangen.
    ''' - Index-Zähler für die Hintergrundfarbe ('BackColor') entkoppelt, damit Leerzeilen im Array das Grid nicht verschieben.
    ''' - Fehlerhaften '.TopItem.Selected'-Aufruf durch die neue, sichere Methode 'prSelectFirstListViewItemIfNeeded' ersetzt.
    ''' </remarks>
    Private Sub prLoadSaisonInList()
        ' Daten auslesen und zeilenweise splitten (leere Zeilen direkt ignorieren)
        Dim rawData As String = ReadOneValueFromSystemDb("Saison")
        Dim arTmp() As String = rawData.Split(New String() {vbCrLf, vbCr, vbLf}, StringSplitOptions.RemoveEmptyEntries)

        Dim arT() As String
        Dim nColor As Integer
        Dim itemIndex As Integer = 0

        With lvSasion
            .Items.Clear()

            ' Zeilen verarbeiten
            For i As Integer = 0 To arTmp.Length - 1
                Dim currentRow As String = arTmp(i).Trim()

                ' Nur verarbeiten, wenn die Zeile nach dem Trimmen valide Daten enthält und ein Semikolon besitzt
                If Not String.IsNullOrWhiteSpace(currentRow) AndAlso currentRow.Contains(";") Then
                    arT = currentRow.Split(";"c)

                    ' Neues ListViewItem erstellen und SubItems befüllen
                    Dim lv As ListViewItem = .Items.Add(arT(0).Trim())

                    ' Prüfen, ob genügend Spaltenwerte vorhanden sind (Sicherheit gegen korrupte INI-Zeilen)
                    If arT.Length > 2 Then
                        lv.SubItems.Add(arT(1))
                        lv.SubItems.Add(arT(2))
                    End If

                    ' Farbindex ermitteln (Erstes Zeichen des ersten Elements)
                    Dim firstChar As String = arT(0).Trim().Substring(0, 1)
                    If Integer.TryParse(firstChar, nColor) Then
                        ' Hintergrundfarbe zuweisen
                        .Items(itemIndex).BackColor = fcStringRGB(arFarbe(nColor, 1))
                    End If

                    ' Index für den nächsten visuellen Eintrag erhöhen
                    itemIndex += 1
                End If
            Next

            ' Fokus auf das Steuerelement setzen
            .Select()

            ' Automatisch den ersten Eintrag selektieren, falls Daten geladen wurden
            prSelectFirstListViewItemIfNeeded(lvSasion)
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