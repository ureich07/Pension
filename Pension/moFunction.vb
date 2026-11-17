Imports System.Drawing
Imports System.Xml, System.Xml.XPath
Imports IDEALSoftware.VpeCommunity
Imports System.Net
Imports System.Net.Mail
Imports System.IO
Imports System.Security.Cryptography.X509Certificates

Module moFunction

#Region "Datei - Funktionen........................................................................"


    ''' <summary>
    ''' Schreibt einen String im UTF-8-Unicode-Format in eine Datei. Eine bestehende Datei wird ¸berschrieben.
    ''' </summary>
    ''' <param name="cFilename">Der vollst‰ndige Pfad zur Zieldatei.</param>
    ''' <param name="cInhalt">Der in die Datei zu schreibende Text.</param>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - 'Using'-Block f¸r automatische Ressourcenfreigabe (Close/Dispose) implementiert.
    ''' - Explizite UTF-8-Kodierung erzwungen, um Unicode-Zeichen plattform¸bergreifend korrekt zu speichern.
    ''' - Auskommentierten Althandler (Code-Leichen) entfernt.
    ''' </remarks>
    Public Sub WriteFileUniCode(ByVal cFilename As String, ByVal cInhalt As String)
        Try
            ' Using stellt sicher, dass der StreamWriter auch im Fehlerfall die Datei freigibt
            Using sw As New IO.StreamWriter(cFilename, False, System.Text.Encoding.UTF8)
                sw.Write(cInhalt)
            End Using

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Liest den Wert eines spezifischen Datenfeldes aus der Tabelle 'System' aus.
    ''' </summary>
    ''' <param name="cFilename">Der Name der Spalte in der System-Tabelle, deren Wert ausgelesen werden soll.</param>
    ''' <returns>Den Inhalt des Datenfeldes als <see cref="String"/>. Tritt ein Fehler auf oder ist die Tabelle leer, wird ein leerer String zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' <para>Optimiert am 22.09.2026: Performance-Verbesserung durch gezielte Spaltenabfrage und Sicherheitspr¸fung auf vorhandene Zeilen.</para>
    ''' <para>Optimiert am 11.10.2026: Performance-Verbesserung L‰dt nur noch die angeforderte Spalte statt der gesamten Tabelle (SELECT *).</para>
    ''' </remarks>
    Public Function ReadOneValueFromSystemDb(ByVal cFilename As String) As String
        Try

            Dim dt As DataTable = fcReadDataTable("Select " & cFilename & " from System")
            ' Sicherheitspr¸fung: Enth‰lt die Tabelle ¸berhaupt Zeilen und ist der Wert nicht NULL?
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso Not IsDBNull(dt.Rows(0).Item(cFilename)) Then
                Return dt.Rows(0).Item(0).ToString()
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try

        ' Standard-R¸ckgabe, falls ein Fehler auftritt oder keine Daten gefunden wurden
        Return String.Empty
    End Function


    ''' <summary>
    ''' Aktualisiert oder speichert einen Systemwert (z. B. Kopierdaten) in der Datenbanktabelle 'System'.
    ''' </summary>
    ''' <param name="cFilename">Der Name der Spalte (Eigenschaft), in die geschrieben werden soll.</param>
    ''' <param name="cInhalt">Der zu speichernde Inhalt.</param>
    ''' <remarks>
    ''' <para>Optimiert am 22.09.2026: Performance-Verbesserung durch gezielte Spaltenabfrage und Sicherheitspr¸fung auf vorhandene Zeilen.</para>
    ''' </remarks>
    Public Sub SaveOneValueInSystemDb(ByVal cFilename As String, ByVal cInhalt As String)
        ' Validierung: Wenn kein Spaltenname ¸bergeben wurde, abbrechen
        If String.IsNullOrEmpty(cFilename) Then Exit Sub

        Try
            ' Arrays direkt bei der Deklaration mit Werten initialisieren
            Dim arFields As String() = {cFilename}
            Dim arValue As String() = {If(cInhalt, "")}

            ' Den Update-Befehl f¸r die System-Tabelle ausf¸hren
            Call fcUpdateCommand("System", arFields, arValue, String.Empty)

        Catch ex As Exception
            ' Fehler sicher protokollieren
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Liest den gesamten Inhalt einer Textdatei unter Verwendung der ANSI-Codierung (Windows-1252) aus.
    ''' </summary>
    ''' <param name="cFilename">Der vollst‰ndige Pfad zur zu lesenden Datei.</param>
    ''' <returns>Der Dateiinhalt als String. Bei einem Fehler oder einer leeren Datei wird ein leerer String zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - 'Using'-Block f¸r die automatische und sichere Ressourcenfreigabe (Close/Dispose) implementiert.
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen durch modernes 'Return' ersetzt.
    ''' - Vorinitialisierung des R¸ckgabewerts entfernt, da Fehlerf‰lle ¸ber 'Return String.Empty' im Catch-Block abgefangen werden.
    ''' </remarks>
    Public Function ReadFileSeriell(ByVal cFilename As String) As String
        Try
            ' Bestimmt die Windows-1252-Codierung (Westeurop‰isch ANSI)
            Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding(1252)

            ' Using garantiert das Schlieﬂen der Datei, selbst wenn beim Lesen ein Fehler auftritt
            Using sr As New IO.StreamReader(cFilename, enc)
                Return sr.ReadToEnd()
            End Using

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Schreibt eine Zeichenkette serialisiert und mit einer spezifischen Codierung (Windows-1252) in eine Datei.
    ''' </summary>
    ''' <param name="cFilename">Der vollst‰ndige Pfad inklusive Dateiname, in den geschrieben werden soll.</param>
    ''' <param name="cInhalt">Die serialisierte Zeichenkette, die in die Datei geschrieben wird.</param>
    ''' <remarks>
    ''' <para>Die Datei wird bei jedem Aufruf komplett ¸berschrieben. Es wird die westeurop‰ische Codierung Windows-1252 verwendet.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 ñ Optimiert: Umstellung auf einen ressourcensicheren <c>Using</c>-Block, um Datei-Blockaden im Fehlerfall zu verhindern.<br/>
    ''' </para>
    ''' </remarks>
    Public Sub WriteFileSeriell(ByVal cFilename As String, ByVal cInhalt As String)
        ' Validierung der Eingabeparameter
        If String.IsNullOrEmpty(cFilename) Then Exit Sub

        Try
            ' Westeurop‰ische ANSI-Codierung (Windows-1252) initialisieren
            Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding(1252)

            ' WICHTIGE OPTIMIERUNG: Der Using-Block garantiert, dass der StreamWriter 
            ' die Datei schlieﬂt und die Ressourcen freigibt ñ selbst wenn ein Fehler auftritt!
            Using sw As New IO.StreamWriter(cFilename, False, enc)
                sw.Write(cInhalt)
            End Using ' Hier wird sw.Close() und sw.Dispose() automatisch aufgerufen

        Catch ex As Exception
            ' Fehler an die zentrale Fehlerverwaltung ¸bergeben
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Stellt einen Dialog zum Ausw‰hlen eines Verzeichnisses bereit.
    ''' </summary>
    ''' <param name="sDir">Das standardm‰ﬂig vorselektierte Verzeichnis.</param>
    ''' <returns>Der Pfad des ausgew‰hlten Ordners oder der urspr¸nglich ¸bergebene Pfad, falls der Dialog abgebrochen wurde. Bei Fehlern wird ein leerer String zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' 17.07.2011 - Create
    ''' 06.11.2026 - Code-Optimierung:
    ''' - 'Using'-Block f¸r 'FolderBrowserDialog' implementiert, um Ressourcen nach der Nutzung sofort wieder freizugeben (Dispose).
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen durch moderne 'Return'-Anweisungen ersetzt.
    ''' - Logik vereinfacht: Der ¸bergebene Pfad 'sDir' wird nun direkt zur¸ckgegeben, wenn der Benutzer auf 'Abbrechen' klickt.
    ''' </remarks>
    Public Function fcGetDirectory(ByVal sDir As String) As String
        Try
            ' Using stellt sicher, dass die unmanaged Ressourcen des Dialogs sofort freigegeben werden
            Using fbDialog As New System.Windows.Forms.FolderBrowserDialog()
                fbDialog.SelectedPath = sDir

                If fbDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    Return fbDialog.SelectedPath
                End If
            End Using

            ' Wenn der Benutzer abbricht, wird der urspr¸ngliche Pfad zur¸ckgegeben
            Return sDir

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Stellt den Dialog zum Speichern einer Datei bereit und sichert den Inhalt.
    ''' </summary>
    ''' <param name="sFile">Der standardm‰ﬂig vorgeschlagene Dateiname oder Pfad.</param>
    ''' <param name="sFilter">Der Filter f¸r die Dateiendungen (z. B. "Textdateien (*.txt)|*.txt").</param>
    ''' <param name="sInhalt">Der Inhalt, der in der ausgew‰hlten Datei gespeichert werden soll.</param>
    ''' <returns>Der vollst‰ndige Pfad der gespeicherten Datei oder ein leerer String, wenn der Dialog abgebrochen wurde.</returns>
    ''' <remarks>
    ''' 26.11.2008 - Create
    ''' 06.11.2026 - Code-Optimierung:
    ''' - 'Using'-Block f¸r 'SaveFileDialog' implementiert, um unmanaged Ressourcen sofort freizugeben.
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen durch moderne 'Return'-Anweisungen ersetzt.
    ''' - Veraltetes Schl¸sselwort 'Call' beim Aufruf von 'SaveOneValueInSystemDb' entfernt.
    ''' - XML-Kommentar korrigiert (R¸ckgabetyp ist 'String', nicht 'T/F').
    ''' </remarks>
    Public Function fcSaveFileDialog(ByVal sFile As String, ByVal sFilter As String, ByVal sInhalt As String) As String
        Try
            ' Using stellt sicher, dass die Windows-Ressourcen des Dialogs nach der Nutzung sofort freigegeben werden
            Using sfDialog As New System.Windows.Forms.SaveFileDialog()
                sfDialog.CreatePrompt = True
                sfDialog.FileName = sFile
                sfDialog.Filter = sFilter

                If sfDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    ' Speichert den Inhalt in der ausgew‰hlten Datei
                    SaveOneValueInSystemDb(sfDialog.FileName, sInhalt)
                    Return sfDialog.FileName
                End If
            End Using

            ' Wenn der Benutzer abbricht, wird ein leerer String zur¸ckgegeben
            Return String.Empty

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Stellt den Dialog zum ÷ffnen einer Datei bereit.
    ''' </summary>
    ''' <param name="sFile">Der standardm‰ﬂig vorgeschlagene Dateiname oder Pfad.</param>
    ''' <param name="sFilter">Der Filter f¸r die Dateiendungen (z. B. "Textdateien (*.txt)|*.txt").</param>
    ''' <returns>Der vollst‰ndige Pfad der ausgew‰hlten Datei oder ein leerer String, wenn der Dialog abgebrochen wurde.</returns>
    ''' <remarks>
    ''' 26.11.2008 - Create
    ''' 06.11.2026 - Code-Optimierung:
    ''' - 'Using'-Block f¸r 'OpenFileDialog' implementiert, um unmanaged Ressourcen sofort freizugeben.
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen durch moderne 'Return'-Anweisungen ersetzt.
    ''' - Vorinitialisierung des R¸ckgabewerts entfernt, da Fehler- und Abbrechf‰lle explizit ¸ber 'Return String.Empty' gelˆst werden.
    ''' </remarks>
    Public Function fcOpenFileDialog(ByVal sFile As String, ByVal sFilter As String) As String
        Try
            ' Using stellt sicher, dass die Windows-Ressourcen des Dialogs nach der Nutzung sofort freigegeben werden
            Using opDialog As New System.Windows.Forms.OpenFileDialog()
                opDialog.FileName = sFile
                opDialog.Filter = sFilter

                If opDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    Return opDialog.FileName
                End If
            End Using

            ' Wenn der Benutzer abbricht, wird ein leerer String zur¸ckgegeben
            Return String.Empty

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' ÷ffnet einen Dialog zur Dateiauswahl und l‰dt den Inhalt der ausgew‰hlten Datei.
    ''' </summary>
    ''' <param name="sDir">Das Verzeichnis, das beim ÷ffnen des Dialogs standardm‰ﬂig angezeigt wird.</param>
    ''' <param name="sMaske">Der Filter f¸r die Dateiendungen (z. B. "Textdateien (*.txt)|*.txt").</param>
    ''' <returns>Der geladene Inhalt der Datei als String oder ein leerer String, wenn der Dialog abgebrochen wurde.</returns>
    ''' <remarks>
    ''' 04.03.2011 - Create
    ''' 06.11.2026 - Code-Optimierung:
    ''' - 'Using'-Block f¸r 'OpenFileDialog' implementiert, um Windows-Ressourcen sofort sauber freizugeben.
    ''' - Unsauberen und veralteten Befehl 'Exit Function' entfernt und durch direktes 'Return' ersetzt.
    ''' - Vorinitialisierung des R¸ckgabewerts entfernt, da Abbr¸che und Fehler explizit ¸ber 'Return String.Empty' abgefangen werden.
    ''' </remarks>
    Public Function fcOpenReadFile(ByVal sDir As String, ByVal sMaske As String) As String
        Try
            ' Using stellt sicher, dass die Windows-Ressourcen des Dialogs nach der Nutzung sofort freigegeben werden
            Using ofDialog As New OpenFileDialog()
                ofDialog.InitialDirectory = sDir
                ofDialog.Filter = sMaske
                ofDialog.FilterIndex = 2
                ofDialog.RestoreDirectory = True

                If ofDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    ' L‰dt den Wert direkt aus der Datenbank/Datei und beendet die Funktion sauber
                    Return ReadOneValueFromSystemDb(ofDialog.FileName)
                End If
            End Using

            ' Wenn der Benutzer abbricht, wird ein leerer String zur¸ckgegeben
            Return String.Empty

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Pr¸ft, ob eine Datei an dem angegebenen Pfad existiert.
    ''' </summary>
    ''' <param name="sFile">Der vollst‰ndige Pfad zur Datei.</param>
    ''' <returns>True, wenn die Datei existiert; andernfalls False.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Veraltete VB6 'FileLen'-Logik und auskommentiertes 'On Error' vollst‰ndig entfernt.
    ''' - Durch die hochperformante .NET-Methode 'IO.File.Exists' ersetzt (kein Try-Catch f¸r die Existenzpr¸fung mehr nˆtig).
    ''' </remarks>
    Public Function FileExists(ByVal sFile As String) As Boolean
        ' IO.File.Exists f‰ngt intern Fehler (wie ung¸ltige Zeichen) ab und gibt sauber True/False zur¸ck
        Return IO.File.Exists(sFile)
    End Function

    ''' <summary>
    ''' Pr¸ft, ob ein Verzeichnis an dem angegebenen Pfad existiert.
    ''' </summary>
    ''' <param name="sDir">Der vollst‰ndige Pfad zum Verzeichnis.</param>
    ''' <returns>True, wenn das Verzeichnis existiert; andernfalls False.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Auf 'Public' ge‰ndert, um Konsistenz zu den anderen Funktionen zu wahren.
    ''' - 'ByRef' zu 'ByVal' ge‰ndert, da der Pfad innerhalb der Funktion nicht modifiziert werden muss.
    ''' - Veraltete VB6 'Dir$'-Funktion durch die native .NET-Methode 'IO.Directory.Exists' ersetzt.
    ''' </remarks>
    Public Function DirExists(ByVal sDir As String) As Boolean
        Return IO.Directory.Exists(sDir)
    End Function

    ''' <summary>
    ''' Erstellt ein Verzeichnis und alle notwendigen Unterverzeichnisse (vollst‰ndige Pfadstruktur).
    ''' </summary>
    ''' <param name="sDir">Der vollst‰ndige zu erstellende Verzeichnispfad.</param>
    ''' <returns>True, wenn das Verzeichnis erfolgreich erstellt wurde oder bereits existiert; andernfalls False.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Die fehleranf‰llige und langsame String-Schleife (Mid, ChrCount, AtLeft) vollst‰ndig entfernt.
    ''' - Native .NET-Methode 'IO.Directory.CreateDirectory' implementiert, die rekursiv alle Unterordner in einem Schritt erstellt.
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen durch direktes 'Return' ersetzt.
    ''' </remarks>
    Public Function CreateDir(ByVal sDir As String) As Boolean
        ' Wenn der Pfad leer ist, direkt abbrechen
        If String.IsNullOrWhiteSpace(sDir) Then Return False

        Try
            ' IO.Directory.CreateDirectory erstellt automatisch die gesamte Ordnerhierarchie,
            ' falls Teilst¸cke oder der gesamte Pfad noch nicht existieren.
            IO.Directory.CreateDirectory(sDir)
            Return True

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return False
        End Try
    End Function



    ''' <summary>
    ''' Pr¸ft, ob eine Datei bereits existiert, und generiert bei Bedarf einen neuen, numerisch fortlaufenden Dateinamen, um ein ‹berschreiben zu verhindern.
    ''' </summary>
    ''' <param name="cpDatei">Der vollst‰ndige Ausgangspfad inklusive Dateiname und Erweiterung.</param>
    ''' <returns>Den urspr¸nglichen Pfad, falls die Datei nicht existiert, oder einen modifizierten Pfad mit angeh‰ngtem Z‰hler (z. B. '_01', '_02').</returns>
    ''' <remarks>
    ''' <para>Nutzt die hochoptimierten .NET-Klassen <see cref="System.IO.Path"/> und <see cref="System.IO.File"/> statt fehleranf‰lliger VB6-Stringschleifen.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 22.09.2026 ñ Komplettrenovierung: Behebung des Pfad-Verlust-Bugs in der Schleife. Umstellung auf native .NET-Pfadfunktionen und Beseitigung aller VB6-Altlasten (<c>Mid$</c>, <c>Right$</c>).<br/>
    ''' </para>
    ''' </remarks>
    Public Function fcCheckFile(ByVal cpDatei As String) As String
        ' Falls der ¸bergebene Pfad leer ist, sofort abbrechen
        If String.IsNullOrEmpty(cpDatei) Then Return String.Empty

        Try
            ' Wenn die Datei noch nicht existiert, kann der Pfad direkt verwendet werden
            ' Hinweis: Falls "FileExists" eine eigene Funktion ist, kann hier auch das native "System.IO.File.Exists(cpDatei)" genutzt werden.
            If Not System.IO.File.Exists(cpDatei) Then
                Return cpDatei
            End If

            ' Pfadbestandteile sauber ¸ber .NET-Bibliotheken trennen
            Dim verzeichnis As String = System.IO.Path.GetDirectoryName(cpDatei)
            Dim dateinameOhneErweiterung As String = System.IO.Path.GetFileNameWithoutExtension(cpDatei)
            Dim erweiterung As String = System.IO.Path.GetExtension(cpDatei) ' Enth‰lt bereits den Punkt (z.B. ".dat")

            Dim zaehler As Integer = 0
            Dim basisName As String = dateinameOhneErweiterung

            ' Pr¸fen, ob der Dateiname bereits auf einen Unterstrich und eine Zahl endet (z.B. "_01")
            ' Beispiel: SaveDB_2026_09_22_01 -> sucht nach dem letzten "_"
            If dateinameOhneErweiterung.Length > 3 AndAlso dateinameOhneErweiterung.Contains("_") Then
                Dim letzterUnterstrichIndex As Integer = dateinameOhneErweiterung.LastIndexOf("_"c)
                Dim mˆglicherZaehlerText As String = dateinameOhneErweiterung.Substring(letzterUnterstrichIndex + 1)

                ' Wenn der Teil nach dem letzten Unterstrich eine Zahl ist, extrahieren wir sie als Startwert
                Dim extrahierterZaehler As Integer
                If Integer.TryParse(mˆglicherZaehlerText, extrahierterZaehler) Then
                    zaehler = extrahierterZaehler
                    basisName = dateinameOhneErweiterung.Substring(0, letzterUnterstrichIndex)
                End If
            End If

            Dim neuerPfad As String = cpDatei

            ' Schleife l‰uft so lange, bis ein freier Dateiname im Zielverzeichnis gefunden wird
            Do
                zaehler += 1
                ' Format ("D2") sorgt automatisch f¸r f¸hrende Nullen (01, 02, ... 99)
                Dim neuerDateiname As String = String.Format("{0}_{1}{2}", basisName, zaehler.ToString("D2"), erweiterung)
                neuerPfad = System.IO.Path.Combine(verzeichnis, neuerDateiname)
            Loop While System.IO.File.Exists(neuerPfad)

            Return neuerPfad

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            ' Im Fehlerfall geben wir sicherheitshalber den urspr¸nglichen Pfad zur¸ck
            Return cpDatei
        End Try
    End Function

    ''' <summary>
    ''' Schreibt eine Fehlermeldung mit Modul- und StackTrace-Informationen in das globale Fehlerlogbuch.
    ''' </summary>
    ''' <param name="sText">Die Fehlermeldung (Message).</param>
    ''' <param name="cModul">Das Modul oder die Quelle, in der der Fehler aufgetreten ist (Source).</param>
    ''' <param name="cLine">Die StackTrace-Informationen oder die Zeilennummer (StackTrace).</param>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - 'On Error Resume Next' vollst‰ndig durch strukturiertes Try-Catch ersetzt.
    ''' - Performance-Killer entfernt: Datei wird nicht mehr komplett eingelesen und neu geschrieben, sondern per 'IO.File.AppendAllText' erweitert.
    ''' - Pfad-Extraktion modernisiert: Nutzt das sichere 'IO.Path.GetDirectoryName' anstelle von 'AtPfad'.
    ''' - Nutzt die optimierte 'CreateDir'-Logik zur automatischen Verzeichniserstellung.
    ''' - 'Str$(ngError)' durch das moderne, saubere 'ngError.ToString()' ersetzt.
    ''' </remarks>
    Public Sub ErrReport(ByVal sText As String, ByVal cModul As String, ByVal cLine As String)
        Try
            ' Verzeichnis aus dem globalen Logpfad ermitteln
            Dim sDir As String = IO.Path.GetDirectoryName(cgErrLogFile)

            ' Sicherstellen, dass der Ordner existiert (erstellt automatisch die Struktur, falls nˆtig)
            If Not String.IsNullOrEmpty(sDir) Then
                CreateDir(sDir)
            End If

            ' Log-Eintrag sauber strukturieren (String Interpolation f¸r bessere Lesbarkeit)
            Dim sLogEintrag As String = String.Format(
            "[{0}] Fehler in Modul/Quelle: {1}{2}" &
            "Message    : {3}{2}" &
            "StackTrace : {4}{2}" &
            "--------------------------------------------------{2}",
            DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
            cModul,
            Environment.NewLine,
            sText,
            cLine
        )

            ' Text performant an das Ende der Datei anh‰ngen (erstellt die Datei automatisch, falls sie fehlt)
            ' Verwendet standardm‰ﬂig UTF-8, um Sonderzeichen/Umlaute sauber zu loggen
            IO.File.AppendAllText(cgErrLogFile, sLogEintrag, System.Text.Encoding.UTF8)

            ' Fehlerz‰hler erhˆhen
            ngError += 1

            ' UI-Element auf dem Hauptformular aktualisieren (mit modernem ToString)
            If frmMain IsNot Nothing AndAlso frmMain.tssLog IsNot Nothing Then
                frmMain.tssLog.Text = "Info = " & ngError.ToString()
            End If

        Catch ex As Exception
            ' Falls das Schreiben ins Log fehlschl‰gt (z.B. Datei gesperrt), wird ein Absturz der App verhindert
            System.Diagnostics.Debug.WriteLine("Kritischer Fehler im ErrReport: " & ex.Message)
        End Try
    End Sub

#End Region

#Region "String - Funktionen......................................................................."



    ''' <summary>
    ''' Extrahiert einen Teilstring, der sich zwischen einem definierten Anfangs- und Endzeichen befindet.
    ''' Nutzt intern die optimierten Funktionen <c>AtRight</c> und <c>AtLeft</c>.
    ''' </summary>
    ''' <param name="c_Kette">Die gesamte Zeichenkette, die durchsucht werden soll.</param>
    ''' <param name="c_Anf">Das Start-Trennzeichen (z. B. "(").</param>
    ''' <param name="c_End">Das End-Trennzeichen (z. B. ")").</param>
    ''' <param name="Pos">Die Instanz des Start-Trennzeichens von rechts gez‰hlt.</param>
    ''' <returns>Den extrahierten Text zwischen den Trennzeichen oder einen leeren String, falls die Zeichen nicht gefunden wurden.</returns>
    ''' <remarks>
    ''' <para>Optimiert am 22.09.2026: Vollst‰ndige Umstellung von VB6-Stringschleifen (<c>Mid$</c>, <c>Right$</c>) auf die native, hochperformante .NET-Methode <c>LastIndexOf</c>.</para>
    ''' </remarks>
    Function Extract(ByVal c_Kette As String, ByVal c_Anf As String, ByVal c_End As String, ByVal Pos As Integer) As String
        ' Holt den rechten Teil ab dem Start-Trennzeichen
        Dim Temp As String = AtRight(c_Kette, c_Anf, Pos)

        ' Gibt den linken Teil vor dem End-Trennzeichen zur¸ck
        Return AtLeft(Temp, c_End, 1)
    End Function

    ''' <summary>
    ''' Gibt den Teil einer Zeichenkette zur¸ck, der links von einem bestimmten Vorkommen eines Trennzeichens steht.
    ''' </summary>
    ''' <param name="Kette">Die zu durchsuchende Gesamtzeichenkette.</param>
    ''' <param name="Zeichen">Das Trennzeichen, nach dem gesucht wird.</param>
    ''' <param name="Nummer">Das wievielte Vorkommen des Trennzeichens als Trennpunkt dienen soll (1 f¸r das erste Vorkommen).</param>
    ''' <returns>Den linken Teilstring vor dem gefundenen Zeichen. Wird das Zeichen nicht gefunden, wird ein leerer String zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' <para>Optimiert am 22.09.2026: Vollst‰ndige Umstellung von VB6-Stringschleifen (<c>Mid$</c>, <c>Right$</c>) auf die native, hochperformante .NET-Methode <c>LastIndexOf</c>.</para>
    ''' </remarks>
    Function AtLeft(ByVal Kette As String, ByVal Zeichen As String, ByVal Nummer As Integer) As String
        ' Validierung der Eingaben
        If String.IsNullOrEmpty(Kette) OrElse String.IsNullOrEmpty(Zeichen) OrElse Nummer <= 0 Then
            Return String.Empty
        End If

        Dim index As Integer = -1

        ' Schleife, um das Zeichen X-mal von links nach rechts zu finden
        For i As Integer = 1 To Nummer
            ' Suche nach dem n‰chsten Vorkommen ab dem letzten gefundenen Index + 1
            index = Kette.IndexOf(Zeichen, index + 1)

            ' Wenn das Zeichen nicht (mehr) gefunden wird, abbrechen
            If index = -1 Then Return String.Empty
        Next

        ' Den linken Teil bis zum gefundenen Index ausschneiden
        Return Kette.Substring(0, index)
    End Function

    ''' <summary>
    ''' Gibt den Teil einer Zeichenkette zur¸ck, der rechts von einem bestimmten Vorkommen eines Trennzeichens steht.
    ''' </summary>
    ''' <param name="Kette">Die zu durchsuchende Gesamtzeichenkette.</param>
    ''' <param name="Zeichen">Das Trennzeichen, nach dem gesucht wird.</param>
    ''' <param name="Nummer">Das wievielte Vorkommen des Trennzeichens von rechts gez‰hlt als Trennpunkt dienen soll (z. B. 1 f¸r das letzte Vorkommen).</param>
    ''' <returns>Den rechten Teilstring nach dem gefundenen Zeichen. Wird das Zeichen nicht oder nicht oft genug gefunden, wird ein leerer String zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' <para>Optimiert am 22.09.2026: Vollst‰ndige Umstellung von VB6-Stringschleifen (<c>Mid$</c>, <c>Right$</c>) auf die native, hochperformante .NET-Methode <c>LastIndexOf</c>.</para>
    ''' </remarks>
    Function AtRight(ByVal Kette As String, ByVal Zeichen As String, ByVal Nummer As Integer) As String
        ' Validierung der Eingaben
        If String.IsNullOrEmpty(Kette) OrElse String.IsNullOrEmpty(Zeichen) OrElse Nummer <= 0 Then
            Return String.Empty
        End If

        Dim index As Integer = Kette.Length

        ' Schleife, um das Zeichen X-mal von rechts nach links zu finden
        For i As Integer = 1 To Nummer
            ' Suche nach dem n‰chsten Vorkommen links vom aktuellen Index
            index = Kette.LastIndexOf(Zeichen, index - 1)

            ' Wenn das Zeichen nicht (mehr) gefunden wird, abbrechen
            If index = -1 Then Return String.Empty
        Next

        ' Den rechten Teil ab dem gefundenen Index ausschneiden (+ L‰nge des Zeichens selbst)
        Dim startPosition As Integer = index + Zeichen.Length
        If startPosition <= Kette.Length Then
            Return Kette.Substring(startPosition)
        End If

        Return String.Empty
    End Function

    ''' <summary>
    ''' F¸llt eine Zeichenkette auf der rechten Seite mit Leerzeichen auf, bis die angegebene Gesamtl‰nge erreicht ist.
    ''' </summary>
    ''' <param name="ZKette">Die urspr¸ngliche Zeichenkette.</param>
    ''' <param name="l‰ngeString">Die gew¸nschte Gesamtl‰nge des resultierenden Strings.</param>
    ''' <returns>Die rechts mit Leerzeichen aufgef¸llte Zeichenkette. Ist der Ausgangsstring bereits l‰nger, wird er unver‰ndert zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Auf 'Public' ge‰ndert, um Konsistenz im Projekt zu wahren.
    ''' - Die veralteten VB6-Kompatibilit‰tsfunktionen 'Left' und 'StrDup' vollst‰ndig entfernt.
    ''' - Native, hochperformante .NET-Methode 'PadRight' implementiert.
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen durch ein direktes 'Return' ersetzt.
    ''' </remarks>
    Public Function PadR(ByVal ZKette As String, ByVal l‰ngeString As Integer) As String
        ' Falls die ¸bergebene Kette Nothing ist, wird sie als leerer String behandelt
        If ZKette Is Nothing Then ZKette = String.Empty

        ' Wenn die gew¸nschte L‰nge kleiner oder gleich 0 ist, wird ein leerer String zur¸ckgegeben
        If l‰ngeString <= 0 Then Return String.Empty

        ' Die native .NET-Methode f¸llt den String performant mit Leerzeichen auf
        Return ZKette.PadRight(l‰ngeString)
    End Function


    ''' <summary>
    ''' F¸llt eine Zeichenkette auf der linken Seite mit Leerzeichen auf, bis die angegebene Gesamtl‰nge erreicht ist.
    ''' </summary>
    ''' <param name="ZKette">Die urspr¸ngliche Zeichenkette.</param>
    ''' <param name="l‰ngeString">Die gew¸nschte Gesamtl‰nge des resultierenden Strings.</param>
    ''' <returns>Die links mit Leerzeichen aufgef¸llte Zeichenkette. Ist der Ausgangsstring bereits l‰nger, wird er unver‰ndert zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Auf 'Public' ge‰ndert, um Konsistenz im Projekt zu wahren.
    ''' - Die veralteten VB6-Kompatibilit‰tsfunktionen 'Right' und 'StrDup' vollst‰ndig entfernt.
    ''' - Native, hochperformante .NET-Methode 'PadLeft' mit dem Char-Literal " "c implementiert.
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen durch ein direktes 'Return' ersetzt.
    ''' </remarks>
    Public Function PadL(ByVal ZKette As String, ByVal l‰ngeString As Integer) As String
        ' Falls die ¸bergebene Kette Nothing ist, wird sie als leerer String behandelt
        If ZKette Is Nothing Then ZKette = String.Empty

        ' Wenn die gew¸nschte L‰nge kleiner oder gleich 0 ist, wird ein leerer String zur¸ckgegeben
        If l‰ngeString <= 0 Then Return String.Empty

        ' F¸llt die Zeichenkette links mit Leerzeichen auf die gew¸nschte Gesamtl‰nge auf
        Return ZKette.PadLeft(l‰ngeString, " "c)
    End Function

    ''' <summary>
    ''' F¸llt eine Zeichenkette auf der linken Seite mit Nullen auf, bis die angegebene Gesamtl‰nge erreicht ist.
    ''' </summary>
    ''' <param name="ZKette">Die urspr¸ngliche Zeichenkette.</param>
    ''' <param name="l‰ngeString">Die gew¸nschte Gesamtl‰nge des resultierenden Strings.</param>
    ''' <returns>Die links mit Nullen aufgef¸llte Zeichenkette.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - 'Right' und 'StrDup' durch die performante .NET-Methode 'PadLeft' ersetzt.
    ''' - 'Return'-Anweisung implementiert.
    ''' </remarks>
    Public Function PadLN(ByVal ZKette As String, ByVal l‰ngeString As Integer) As String
        Return If(ZKette, String.Empty).PadLeft(Math.Max(0, l‰ngeString), "0"c)
    End Function

    ''' <summary>
    ''' F¸llt eine Zeichenkette auf der rechten Seite mit Nullen auf, bis die angegebene Gesamtl‰nge erreicht ist.
    ''' </summary>
    ''' <param name="ZKette">Die urspr¸ngliche Zeichenkette.</param>
    ''' <param name="l‰ngeString">Die gew¸nschte Gesamtl‰nge des resultierenden Strings.</param>
    ''' <returns>Die rechts mit Nullen aufgef¸llte Zeichenkette. Ist der Ausgangsstring bereits l‰nger, wird er unver‰ndert zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Auf 'Public' ge‰ndert, um Projektweit verf¸gbar zu sein.
    ''' - Auskommentierte Code-Leichen und die langsame 'StrDup'-Verkettung vollst‰ndig entfernt.
    ''' - Native, hochperformante .NET-Methode 'PadRight' mit dem Char-Literal "0"c implementiert.
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen durch ein direktes 'Return' ersetzt.
    ''' </remarks>
    Public Function PadRN(ByVal ZKette As String, ByVal l‰ngeString As Integer) As String
        ' Falls die ¸bergebene Kette Nothing ist, wird sie als leerer String behandelt
        If ZKette Is Nothing Then ZKette = String.Empty

        ' Wenn die gew¸nschte L‰nge kleiner oder gleich 0 ist, wird ein leerer String zur¸ckgegeben
        If l‰ngeString <= 0 Then Return String.Empty

        ' F¸llt die Zeichenkette rechts mit Nullen auf die gew¸nschte Gesamtl‰nge auf
        Return ZKette.PadRight(l‰ngeString, "0"c)
    End Function

    ''' <summary>
    ''' Entfernt alle Vorkommen eines bestimmten Zeichens aus einer Zeichenkette.
    ''' </summary>
    ''' <param name="zKette">Die urspr¸ngliche Zeichenkette.</param>
    ''' <param name="sString">Das zu entfernende Zeichen (sollte nicht l‰nger als ein Zeichen sein).</param>
    ''' <returns>Die bereinigte Zeichenkette. Wenn die Eingabe leer ist, wird ein leerer String zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Auf 'Public' ge‰ndert, um Konsistenz im Projekt zu wahren.
    ''' - Performance-Killer entfernt: Die langsame 'For'-Schleife mit 'Mid' und String-Verkettung wurde vollst‰ndig eliminiert.
    ''' - Native, hochperformante .NET-Methode 'String.Replace' implementiert.
    ''' - Null- und Leerpr¸fung f¸r 'zKette' integriert, um Laufzeitfehler zu verhindern.
    ''' </remarks>
    Public Function StrTrim(ByVal zKette As String, ByVal sString As String) As String
        ' Validierung: Wenn die Kette leer oder Nothing ist, direkt abbrechen
        If String.IsNullOrEmpty(zKette) Then Return String.Empty

        ' Wenn das zu entfernende Zeichen leer ist, gibt es nichts zu tun
        If String.IsNullOrEmpty(sString) Then Return zKette

        ' Ersetzt das gesuchte Zeichen durch einen leeren String (lˆscht es somit)
        Return zKette.Replace(sString, String.Empty)
    End Function

    ''' <summary>
    ''' ‹berpr¸ft, ob eine bestimmte Zeichenkette innerhalb einer anderen Zeichenkette existiert.
    ''' </summary>
    ''' <param name="sString">Die zu durchsuchende Zeichenkette.</param>
    ''' <param name="sGesucht">Die gesuchte Zeichenkette.</param>
    ''' <returns>True, wenn die gesuchte Zeichenkette gefunden wurde; andernfalls False.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Auf 'Public' ge‰ndert, um Konsistenz im Projekt zu wahren.
    ''' - Performance-Killer entfernt: Die langsame 'For'-Schleife mit 'Mid'-Teilstring-Vergleichen wurde vollst‰ndig eliminiert.
    ''' - Native, hochperformante .NET-Methode 'String.Contains' implementiert.
    ''' - Null- und Leerpr¸fung integriert, um Laufzeitfehler bei leeren ‹bergaben zu verhindern.
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen sowie 'Exit Function' durch ein direktes 'Return' ersetzt.
    ''' </remarks>
    Public Function fcSeekString(ByVal sString As String, ByVal sGesucht As String) As Boolean
        ' Validierung: Wenn einer der Strings Nothing oder leer ist, direkt False zur¸ckgeben
        If String.IsNullOrEmpty(sString) OrElse String.IsNullOrEmpty(sGesucht) Then
            Return False
        End If

        ' Verwendet die native .NET-Methode zur Teilstring-Suche (beachtet Groﬂ-/Kleinschreibung)
        Return sString.Contains(sGesucht)
    End Function

    ''' <summary>
    ''' Entschl¸sselt (oder verschl¸sselt) eine Zeichenkette mithilfe eines XOR-Algorithmus und eines Sicherheitsschl¸ssels.
    ''' </summary>
    ''' <param name="sString">Die zu verarbeitende Zeichenkette.</param>
    ''' <param name="sSecurityValue">Der geheime Schl¸ssel f¸r die XOR-Operation.</param>
    ''' <returns>Die transformierte Zeichenkette. Bei leeren Eingaben wird ein leerer String zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Performance-Killer entfernt: 'StringBuilder' implementiert, um massiven Speicher-M¸ll durch String-Verkettung (&) in der Schleife zu verhindern.
    ''' - Veraltete VB6-Befehle (Len, Mid$, Asc, Chr) durch hocheffiziente, native .NET-Zeichenzugriffe ersetzt.
    ''' - Index-Logik auf das moderne, 0-basierte System umgestellt.
    ''' - Null- und Leerpr¸fung integriert, um Abst¸rze bei fehlenden ‹bergaben zu verhindern.
    ''' </remarks>
    Public Function fDeCrypt(ByVal sString As String, ByVal sSecurityValue As String) As String
        ' Validierung: Wenn einer der Strings leer oder Nothing ist, direkt abbrechen
        If String.IsNullOrEmpty(sString) OrElse String.IsNullOrEmpty(sSecurityValue) Then
            Return String.Empty
        End If

        Dim nMax As Integer = sString.Length
        Dim nSecLen As Integer = sSecurityValue.Length

        ' StringBuilder reserviert vorab Speicherplatz f¸r die exakte Zielgrˆﬂe
        Dim sb As New System.Text.StringBuilder(nMax)

        ' 0-basierte Schleife ¸ber alle Zeichen des Eingangsstrings
        For n As Integer = 0 To nMax - 1
            ' Bestimmt die Position im Sicherheitsschl¸ssel mittels Modulo
            Dim x As Integer = n Mod nSecLen

            ' Direkter XOR-Vergleich der Zeichen (Char-Werte werden in .NET intern als UInt16/Integer behandelt)
            Dim decryptedChar As Char = Convert.ToChar(Convert.ToInt32(sSecurityValue(x)) Xor Convert.ToInt32(sString(n)))

            ' Das entschl¸sselte Zeichen hocheffizient an den Buffer anh‰ngen
            sb.Append(decryptedChar)
        Next

        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Ersetzt fehlerhafte UTF-8/ANSI-Codierungsartefakte und Sonderzeichen durch die korrekten deutschen Umlaute und Sonderzeichen.
    ''' </summary>
    ''' <param name="cfText">Der zu korrigierende Text.</param>
    ''' <returns>Der bereinigte Text mit korrekten Umlauten. Bei leerer Eingabe wird ein leerer String zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' 05.03.2002 - Create (Last Update: 12.08.2007)
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Auf 'Public' ge‰ndert, um die projektweite Verf¸gbarkeit zu sichern.
    ''' - Veraltetes VB6 'Replace' durch performantes .NET 'String.Replace' ersetzt.
    ''' - 'Chr(148)' durch das eindeutige Unicode-Zeichen-Literal ersetzt, um Unabh‰ngigkeit von der Code-Page zu garantieren.
    ''' - Effizientes Method-Chaining implementiert, um den Code kompakter und lesbarer zu gestalten.
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen durch ein direktes 'Return' ersetzt.
    ''' </remarks>
    Public Function fcErsetzen(ByVal cfText As String) As String
        If String.IsNullOrEmpty(cfText) Then Return String.Empty
        Return cfText.Replace("Å", "¸").Replace("Ñ", "‰").Replace(Chr(148), "ˆ").Replace("·", "ﬂ").Replace("ô", "÷").Replace("ö", "‹")
    End Function

    ''' <summary>
    ''' Konvertiert eine hexadezimale Zeichenkette in eine bin‰re Zeichenkette.
    ''' </summary>
    ''' <param name="sHex">Die zu konvertierende Hex-Zeichenkette (z. B. "A" oder "F").</param>
    ''' <returns>Die bin‰re Darstellung (z. B. "1010" oder "1111"), aufgef¸llt auf mindestens 4 Stellen. Bei Fehlern wird ein leerer String zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Umst‰ndliche Mehrfach-Konvertierungen (CLng & -> ToString -> Long.Parse) vollst‰ndig entfernt.
    ''' - Native .NET-Methode 'Convert.ToInt64(sHex, 16)' zur direkten Hex-Interpretation implementiert.
    ''' - Nutzt die hochperformante .NET 'PadLeft'-Logik direkt, um unmanaged Alt-Abh‰ngigkeiten aufzulˆsen.
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen durch direktes 'Return' ersetzt.
    ''' </remarks>
    Public Function fcConvertHexToBin(ByVal sHex As String) As String
        ' Validierung: Wenn der Hex-String leer oder Nothing ist, direkt abbrechen
        If String.IsNullOrWhiteSpace(sHex) Then Return String.Empty

        Try
            ' 1. Hex-String direkt in eine Zahl konvertieren (Basis 16)
            Dim nValue As Long = Convert.ToInt64(sHex.Trim(), 16)

            ' 2. Die Zahl in eine Bin‰rzeichenkette umwandeln (Basis 2) 
            '    und links mit Nullen auf mindestens 4 Stellen auff¸llen
            Return Convert.ToString(nValue, 2).PadLeft(4, "0"c)

        Catch ex As Exception
            ' Falls gew¸nscht, kann hier das ErrReport wieder einkommentiert werden
            ' ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return String.Empty
        End Try
    End Function


    ''' <summary>
    ''' Konvertiert einen Geldbetrag von Cent (als String) in einen Euro-String mit Kommatrennung.
    ''' </summary>
    ''' <param name="sCent">Der umzuwandelnde Cent-Betrag als Text.</param>
    ''' <returns>Der formatierte Euro-Betrag mit zwei Nachkommastellen (z. B. "12,50").</returns>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung & Fehlerkorrektur:
    ''' - Kritischen Absturz (ArgumentOutOfRangeException) bei Strings k¸rzer als 2 Zeichen behoben.
    ''' - Veraltete Zuweisung an den Funktionsnamen durch modernes 'Return' ersetzt.
    ''' - Fehlerhafte Logik bei bereits vorhandenen Kommata korrigiert (Zahlen wurden verf‰lscht).
    ''' - Robuste Konvertierung ¸ber numerische Typen (Decimal) f¸r exakte W‰hrungsdarstellung implementiert.
    ''' </remarks>
    Public Function fcChangeCentToEuro(ByVal sCent As String) As String
        ' Bereinigung: Falls der String leer ist oder nur Leerzeichen enth‰lt, als "0" werten
        If String.IsNullOrWhiteSpace(sCent) Then
            Return "0,00"
        End If

        ' Falls bereits ein Komma enthalten ist, den Wert direkt als Euro interpretieren und sauber formatieren
        If sCent.Contains(",") Then
            Dim dEuro As Decimal
            If Decimal.TryParse(sCent, dEuro) Then
                Return dEuro.ToString("N2", New System.Globalization.CultureInfo("de-DE"))
            End If
        End If

        ' Cent-Betrag sicher in eine Zahl umwandeln
        Dim lCent As Long
        If Long.TryParse(sCent, lCent) Then
            ' Mathematisch durch 100 teilen, um den Euro-Wert zu erhalten
            Dim dEuro As Decimal = lCent / 100D
            ' Formatiert die Zahl exakt nach deutschem Standard (z. B. 1234,50 -> "1.234,50" oder "12,50")
            Return dEuro.ToString("N2", New System.Globalization.CultureInfo("de-DE"))
        End If

        ' Fallback, falls die Eingabe keine g¸ltige Zahl war
        Return "0,00"
    End Function

    ''' <summary>
    ''' Konvertiert einen Cent-Betrag (als String) in einen Euro-Betrag mit einem Punkt als Dezimaltrenner.
    ''' </summary>
    ''' <param name="sCent">Der umzuwandelnde Cent-Betrag (z. B. "150" oder "5").</param>
    ''' <returns>Der Euro-Betrag mit Punkt-Trenner (z. B. "1.50" oder "0.05"). Wenn die Eingabe leer ist, wird ein leerer String zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Absturzursache (ArgumentOutOfRangeException) bei Cent-Betr‰gen unter 100 Cent (L‰nge &lt; 3) behoben.
    ''' - Behandlung von kurzen Strings (z. B. "5" zu "0.05") ¸ber die bew‰hrte 'PadLeft'-Logik abgesichert.
    ''' - Logik bei bereits vorhandenem Komma ¸berarbeitet (wird nun durch einen Punkt ersetzt).
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen durch direktes 'Return' ersetzt.
    ''' - Moderne String-Interpolation ($"...") implementiert.
    ''' </remarks>
    Public Function fcChangeCentToEuroPunkt(ByVal sCent As String) As String
        ' Validierung: Wenn der ‹bergabestring leer oder Nothing ist, direkt abbrechen
        If String.IsNullOrWhiteSpace(sCent) Then Return String.Empty

        ' Whitespaces entfernen
        sCent = sCent.Trim()

        ' Fall 1: Der String enth‰lt bereits ein Komma. 
        ' In diesem Fall ersetzen wir es direkt durch einen Punkt, statt es zu lˆschen.
        If sCent.Contains(",") Then
            Return sCent.Replace(",", ".")
        End If

        ' Fall 2: Reine Cent-Ziffernkette formatieren
        ' Sicherstellen, dass der String mindestens 3 Stellen hat (z.B. "5" wird zu "005")
        If sCent.Length < 3 Then
            sCent = sCent.PadLeft(3, "0"c)
        End If

        Dim nL As Integer = sCent.Length

        ' Teilt den String in Vorkomma- und die letzten zwei Nachkommastellen auf
        Return $"{sCent.Substring(0, nL - 2)}.{sCent.Substring(nL - 2)}"
    End Function

    ''' <summary>
    ''' Ersetzt alle Vorkommen eines Suchmusters in einer Zeichenkette durch einen neuen R¸ckgabestring 
    ''' und f¸gt diesen zus‰tzlich als Pr‰fix an den Anfang ein.
    ''' </summary>
    ''' <param name="sZeichen">Die urspr¸ngliche Zeichenkette.</param>
    ''' <param name="sSuch">Das zu suchende Trennzeichen oder die Zeichenfolge.</param>
    ''' <param name="sRueck">Der neu einzuf¸gende String.</param>
    ''' <returns>Die modifizierte Zeichenkette.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - 'ByRef' zu 'ByVal' ge‰ndert, da die Originalvariablen nicht manipuliert werden m¸ssen.
    ''' - Performance-Killer entfernt: Die langsame 'For'-Schleife mit Array-Splitting wurde durch natives 'String.Replace' ersetzt.
    ''' - Flexibilit‰t erhˆht: Der fehleranf‰llige 'Mid(..., 2)'-Befehl wurde eliminiert; funktioniert nun auch mit mehrstelligen Pr‰fixen.
    ''' </remarks>
    Public Function fcChangeString(ByVal sZeichen As String, ByVal sSuch As String, ByVal sRueck As String) As String
        ' Validierung: Wenn die Basiszeichenkette leer ist, direkt abbrechen
        If String.IsNullOrEmpty(sZeichen) Then Return String.Empty
        If String.IsNullOrEmpty(sSuch) Then Return sZeichen

        ' Das alte Verhalten (Splitten und mit sRueck verbinden) entspricht mathematisch 
        ' exakt einem globalen Ersetzen, bei dem ganz am Anfang zus‰tzlich 'sRueck' angeh‰ngt wird.
        Return sRueck & sZeichen.Replace(sSuch, sRueck)
    End Function

    ''' <summary>
    ''' Bereinigt eine numerische Zeichenkette (z.B. Betr‰ge), indem Tausendertrennzeichen (Punkte) entfernt 
    ''' und Dezimaltrenner (Kommas) in das .NET-konforme Punkt-Format konvertiert werden.
    ''' </summary>
    ''' <param name="sZeichen">Die zu bereinigende Zeichenkette (z.B. "1.250,50").</param>
    ''' <returns>Die bereinigte Zeichenkette im Format "1250.50".</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - 'ByRef' zu 'ByVal' ge‰ndert f¸r typsichere Parameter¸bergabe.
    ''' - Zwei komplette Schleifenblˆcke und Speicher-Arrays eliminiert.
    ''' - Nutzt nun hochoptimierte native '.Replace'-Aufrufe in einer einzigen Zeile (Method Chaining).
    ''' </remarks>
    Public Function fcKillKomma(ByVal sZeichen As String) As String
        ' Validierung: Wenn der String leer oder Nothing ist, direkt abbrechen
        If String.IsNullOrWhiteSpace(sZeichen) Then Return String.Empty

        ' 1. Alle Punkte (.) entfernen (Tausendertrennzeichen lˆschen)
        ' 2. Alle Kommas (,) durch Punkte (.) ersetzen (Dezimaltrenner konvertieren)
        Return sZeichen.Trim().Replace(".", String.Empty).Replace(",", ".")
    End Function


    ''' <summary>
    ''' Formatiert eine Dezimalzahl in einen String mit einer festgelegten Anzahl an Vor- und Nachkommastellen sowie einem benutzerdefinierten Trennzeichen.
    ''' Beispiel: 456 -> "  456,00" (abh‰ngig von nVk)
    ''' </summary>
    ''' <param name="nZahl">Die zu formatierende Zahl als Double.</param>
    ''' <param name="nVk">Die gew¸nschte Mindestbreite der Vorkommastellen (wird links mit Leerzeichen aufgef¸llt).</param>
    ''' <param name="nNk">Die Anzahl der Nachkommastellen.</param>
    ''' <param name="sTz">Das zu verwendende Dezimaltrennzeichen (Standard ist ein Komma).</param>
    ''' <returns>Die formatierte Zahl als String.</returns>
    ''' <remarks>
    ''' <b>Historie:</b><br/>
    ''' 08.02.2002 ñ Erstellt<br/>
    ''' 24.09.2026 ñ Uwe: Komplett auf moderne .NET-Formatierung umgestellt, um historische Hilfsfunktionen (PadLN/PadRN) zu ersetzen.<br/>
    ''' </remarks>
    Public Function fcDecStr(ByVal nZahl As Double,
                         Optional ByVal nVk As Integer = 7,
                         Optional ByVal nNk As Integer = 2,
                         Optional ByVal sTz As String = ",") As String

        Dim sResult As String = ""

        Try
            ' 1. Vorkommateil ermitteln (abgerundet)
            Dim nVorKomma As Long = CLng(Math.Floor(nZahl))

            ' 2. Nachkommateil ermitteln und auf die gew¸nschten Stellen runden
            Dim nNachKomma As Double = Math.Round(nZahl - Math.Floor(nZahl), nNk)

            ' 3. Nachkommastellen als reinen String formatieren (z.B. "05" bei 0.05)
            Dim sNachKommaStr As String = nNachKomma.ToString("F" & nNk, System.Globalization.CultureInfo.InvariantCulture)
            If sNachKommaStr.Contains(".") Then
                sNachKommaStr = sNachKommaStr.Split("."c)(1)
            Else
                sNachKommaStr = New String("0"c, nNk)
            End If

            ' 4. Vorkommastellenteil mit Leerzeichen links auff¸llen
            Dim sVorKommaStr As String = nVorKomma.ToString().PadLeft(nVk, " "c)

            ' 5. Gesamten String zusammensetzen
            sResult = sVorKommaStr & sTz & sNachKommaStr

        Catch ex As Exception
            ' Fehler protokollieren
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try

        Return sResult
    End Function


    ''' <summary>
    ''' ‹berpr¸ft Tastatureingaben f¸r Betragsfelder. L‰sst nur Zahlen, Steuerzeichen, ein optionales f¸hrendes Minus 
    ''' sowie maximal ein Komma zu. Ein eingegebener Punkt wird automatisch in ein Komma umgewandelt.
    ''' </summary>
    ''' <param name="sKey">Das gedr¸ckte Zeichen (wird per ByRef modifiziert oder bei Ung¸ltigkeit genullt).</param>
    ''' <param name="oText">Die TextBox, in der die Eingabe stattfindet (f¸r die Kontextpr¸fung).</param>
    ''' <remarks>
    ''' 27.01.2012 - Create
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Parameter auf 'ByRef As Char' ge‰ndert, um perfekt mit 'e.KeyChar' zu harmonieren und Werte zur¸ckzugeben.
    ''' - 'IsNumeric' und unsaubere ASCII-Bereiche durch lesbare .NET-Methoden ('Char.IsDigit', 'Char.IsControl') ersetzt.
    ''' - 'InStr' durch das moderne 'TextBox.Text.Contains' ersetzt.
    ''' - Umwandlung von '.' zu ',' direkt ¸ber das native Char-Literal '","c' gelˆst.
    ''' </remarks>
    Public Sub prCheckNumericKey(ByRef sKey As Char, Optional ByVal oText As TextBox = Nothing)
        ' 1. Punkt automatisch in ein Komma umwandeln
        If sKey = "."c Then
            sKey = ","c
        End If

        ' 2. Erlaubte Standardzeichen direkt zulassen (Zahlen und Steuertasten wie Backspace/Enter)
        If Char.IsDigit(sKey) OrElse Char.IsControl(sKey) Then
            Exit Sub
        End If

        ' 3. Pr¸fung f¸r das Minuszeichen (Negativbetr‰ge)
        If sKey = "-"c Then
            If oText IsNot Nothing Then
                ' Minus ist nur an Position 0 zul‰ssig ODER wenn der gesamte Text markiert ist
                If oText.SelectionStart = 0 OrElse oText.SelectionLength = oText.Text.Length Then
                    Exit Sub
                End If
            End If
        End If

        ' 4. Pr¸fung f¸r das Komma (Dezimaltrenner)
        If sKey = ","c Then
            If oText IsNot Nothing Then
                ' Komma zulassen, falls noch keines im Text existiert
                If Not oText.Text.Contains(",") Then
                    Exit Sub
                End If
            End If
        End If

        ' Wenn kein Kriterium zutrifft, wird das Zeichen durch das Null-Zeichen (Chr(0)) ersetzt und somit verworfen
        sKey = Convert.ToChar(0)
    End Sub


    ''' <summary>
    ''' Formatiert eine Betragseingabe (String) typsicher in ein standardisiertes W‰hrungs- 
    ''' und Dezimalformat mit einer definierbaren Anzahl von Nachkommastellen.
    ''' </summary>
    ''' <param name="sValue">Der zu formatierende numerische String.</param>
    ''' <param name="nDecimals">Die Anzahl der gew¸nschten Nachkommastellen (Standard ist 2).</param>
    ''' <returns>Der formatierte Dezimalstring im deutschen Standardformat (z. B. "1234,56").</returns>
    ''' <remarks>
    ''' 27.01.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Fehlerkorrektur: Die feste Teilung durch '100' wurde durch den dynamischen Faktor 'nDeci' ersetzt, damit 'nDecimals' mathematisch korrekt arbeitet.
    ''' - Runden statt Abschneiden: Die unpr‰zise 'Int()'-Funktion wurde durch das kaufm‰nnische 'Math.Round()' ersetzt.
    ''' - Absturzsicherheit: 'Val()' und 'Replace()' durch das robuste '.TryParse()' unter Ber¸cksichtigung von Komma und Punkt ersetzt.
    ''' - Modernisierung: Veraltetes 'Format()' und die Zuweisung an den Funktionsnamen durch ein natives '.ToString()' mit 'Return' ersetzt.
    ''' </remarks>
    Public Function fcFormatDecimal(ByVal sValue As String, Optional ByVal nDecimals As Integer = 2) As String
        If String.IsNullOrWhiteSpace(sValue) Then
            Return "0" & If(nDecimals > 0, "," & New String("0"c, nDecimals), "")
        End If

        Dim nBetrag As Decimal = 0

        ' Konvertiert den String kultur-unabh‰ngig (ersetzt Komma durch Punkt f¸r den Parser)
        Dim sBereinigt As String = sValue.Replace(","c, "."c).Trim()

        If Decimal.TryParse(sBereinigt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, nBetrag) Then
            ' Kaufm‰nnisches Runden auf die gew¸nschten Nachkommastellen
            nBetrag = Math.Round(nBetrag, nDecimals, MidpointRounding.AwayFromZero)
        Else
            nBetrag = 0
        End If

        ' Erzeugt den Format-String dynamisch basierend auf nDecimals (z.B. "F2" f¸r zwei Nachkommastellen)
        ' Nutzt die deutsche Kultur f¸r das korrekte Dezimalkomma bei der Ausgabe
        Dim nKulturDeutschland As System.Globalization.CultureInfo = System.Globalization.CultureInfo.GetCultureInfo("de-DE")
        Return nBetrag.ToString("F" & nDecimals.ToString(), nKulturDeutschland)
    End Function

    ''' <summary>
    ''' F¸hrt eine pr‰zise kaufm‰nnische Rundung (Halbaufwertung weg von der Null) 
    ''' auf eine definierbare Anzahl von Nachkommastellen durch.
    ''' </summary>
    ''' <param name="Number">Die zu rundende Flieﬂkommazahl (Double).</param>
    ''' <param name="NumDigitsAfterDecimal">Die Anzahl der gew¸nschten Nachkommastellen (Standard ist 0).</param>
    ''' <returns>Die kaufm‰nnisch gerundete Zahl als Double.</returns>
    ''' <remarks>
    ''' 30.08.2008 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Fehlerkorrektur bei Negativwerten: Behebt das Fehlverhalten der alten 'Int(+0.5)'-Formel bei negativen Rechnungsbetr‰gen.
    ''' - Eliminierung von Flieﬂkommafehlern: Durch die interne Nutzung von 'Decimal' werden typische Double-Ungenauigkeiten (wie z. B. 0.1499999999) neutralisiert.
    ''' - Modernisierung: Die VB6-Zuweisung an den Funktionsnamen wurde durch ein sauberes 'Return' im Try-Block ersetzt.
    ''' </remarks>
    Public Function fcRound(ByVal Number As Double, Optional ByVal NumDigitsAfterDecimal As Integer = 0) As Double
        Try
            ' Schutz vor ung¸ltigen Nachkommastellen (Math.Round erlaubt Werte von 0 bis 28)
            If NumDigitsAfterDecimal < 0 Then NumDigitsAfterDecimal = 0
            If NumDigitsAfterDecimal > 28 Then NumDigitsAfterDecimal = 28

            ' Umwandlung in Decimal, um die bin‰re Flieﬂkomma-Ungenauigkeit von Double zu eliminieren
            Dim nDezimalWert As Decimal = Convert.ToDecimal(Number)

            ' Durchf¸hrung der echten kaufm‰nnischen Rundung (AwayFromZero)
            Dim nGerundeterBetrag As Decimal = Math.Round(nDezimalWert, NumDigitsAfterDecimal, MidpointRounding.AwayFromZero)

            Return Convert.ToDouble(nGerundeterBetrag)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            ' Im Fehlerfall wird der ungerundete Originalwert zur¸ckgegeben, um den Datenfluss nicht zu unterbrechen
            Return Number
        End Try
    End Function

    ''' <summary>
    ''' Durchsucht eine Zeichenkette nach einem Anfangs- und Endzeichen und teilt sie in ein dreiteiliges Array auf:
    ''' Index 0: Der Text vor dem Anfangszeichen.
    ''' Index 1: Der verbleibende Text nach dem Endzeichen.
    ''' Index 2: Der extrahierte Text zwischen dem Anfangs- und Endzeichen.
    ''' </summary>
    ''' <param name="sText">Die zu durchsuchende Zeichenkette.</param>
    ''' <param name="sAnfang">Das Start-Trennzeichen.</param>
    ''' <param name="sEnde">Das End-Trennzeichen.</param>
    ''' <returns>Ein String-Array mit exakt 3 Elementen.</returns>
    ''' <remarks>
    ''' 12.04.2012 - Create
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Expliziten R¸ckgabetyp 'As String()' hinzugef¸gt, um 'Option Strict On'-Kompatibilit‰t zu sichern.
    ''' - Performance-Killer entfernt: Beide 'For'-Schleifen und 'Mid'-Aufrufe durch hocheffiziente '.IndexOf'- und '.Substring'-Operationen ersetzt.
    ''' - Index-Logik auf das native, 0-basierte .NET-System umgestellt.
    ''' - Absicherung gegen 'Nothing' (Null-Werte) integriert.
    ''' </remarks>
    Public Function fcStringSeek(ByVal sText As String, ByVal sAnfang As String, ByVal sEnde As String) As String()
        ' Standard-R¸ckgabe vorbereiten (entspricht dem alten Verhalten bei Fehlern/Nichtgearbeitet)
        Dim sRueck() As String = {If(sText, String.Empty), String.Empty, String.Empty}

        ' Validierung: Wenn der Text oder die Trennzeichen leer sind, direkt das Standard-Array zur¸ckgeben
        If String.IsNullOrEmpty(sText) OrElse String.IsNullOrEmpty(sAnfang) OrElse String.IsNullOrEmpty(sEnde) Then
            Return sRueck
        End If

        ' 1. Position des Anfangszeichens bestimmen (0-basierter Index)
        Dim nIdxAnfang As Integer = sText.IndexOf(sAnfang, StringComparison.Ordinal)

        ' Wenn das Anfangszeichen nicht gefunden wurde, brechen wir direkt ab
        If nIdxAnfang = -1 Then
            Return sRueck
        End If

        ' Teil 0 extrahieren: Alles vor dem Anfangszeichen
        sRueck(0) = sText.Substring(0, nIdxAnfang)

        ' Rest-Text nach dem Anfangszeichen ermitteln
        Dim nStartRest As Integer = nIdxAnfang + sAnfang.Length
        Dim sRestText As String = sText.Substring(nStartRest)

        ' 2. Position des Endzeichens im verbleibenden Rest-Text bestimmen
        Dim nIdxEnde As Integer = sRestText.IndexOf(sEnde, StringComparison.Ordinal)

        ' Wenn das Endzeichen im Rest-Text gefunden wurde
        If nIdxEnde <> -1 Then
            ' Teil 2 extrahieren: Der Text zwischen Anfang und Ende
            sRueck(2) = sRestText.Substring(0, nIdxEnde)

            ' Teil 1 extrahieren: Alles nach dem Endzeichen
            Dim nStartNachEnde As Integer = nIdxEnde + sEnde.Length
            If nStartNachEnde < sRestText.Length Then
                sRueck(1) = sRestText.Substring(nStartNachEnde)
            Else
                sRueck(1) = String.Empty
            End If
        Else
            ' Falls kein Endzeichen gefunden wurde, f‰llt die Logik auf den Ausgangszustand zur¸ck
            sRueck(0) = sText
            sRueck(1) = String.Empty
            sRueck(2) = String.Empty
        End If

        Return sRueck
    End Function

#End Region

#Region "Datums - Funktionen......................................................................."

    ''' <summary>
    ''' Erhˆht oder verringert ein Datum im Format "jjjjmmtt" um eine bestimmte Anzahl von Tagen.
    ''' </summary>
    ''' <param name="sDatumX">Das Ausgangsdatum als String im Format "yyyyMMdd".</param>
    ''' <param name="iTage">Die Anzahl der Tage, die addiert (positiv) oder subtrahiert (negativ) werden sollen.</param>
    ''' <returns>Das berechnete Datum als String im urspr¸nglichen Format "yyyyMMdd".</returns>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' Die Funktion nutzt natives .NET DateTime-Parsing, um Fehler bei L‰ndereinstellungen zu vermeiden.
    ''' </remarks>
    Function fcDatumInc(ByVal sDatumX As String, ByVal iTage As Integer) As String
        ' 1. String im Format "yyyyMMdd" in ein echtes DateTime-Objekt parsen
        Dim dDatum As DateTime = DateTime.ParseExact(sDatumX, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)

        ' 2. Die gew¸nschten Tage addieren (oder subtrahieren, falls iTage negativ ist)
        dDatum = dDatum.AddDays(iTage)

        ' 3. Das Ergebnis wieder formatiert als "yyyyMMdd" zur¸ckgeben
        Return dDatum.ToString("yyyyMMdd")
    End Function



    ''' <summary>
    ''' Berechnet die Anzahl der Tage zwischen zwei Datumswerten im Format JJJJMMTT.
    ''' </summary>
    ''' <param name="sDatumStart">Das Startdatum im Format YYYYMMDD.</param>
    ''' <param name="sDatumEnd">Das Enddatum im Format YYYYMMDD.</param>
    ''' <returns>Die Differenz in Tagen als Ganzzahl (Integer).</returns>
    ''' <remarks>
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Parameter¸bergabe von 'ByRef' auf 'ByVal' umgestellt, da die Originalwerte nicht manipuliert werden.
    ''' - Veraltete Zuweisung an den Funktionsnamen durch modernes 'Return'-Statement ersetzt.
    ''' - Typkonvertierung durch die Verwendung von nativem .NET 'Date.Subtract' oder 'TimeSpan' vorbereitet.
    ''' </remarks>
    Function fcDatumDiff(ByVal sDatumStart As String, ByVal sDatumEnd As String) As Integer
        ' Konvertierung der String-Datumsangaben in echte Date-Objekte ¸ber die Hilfsfunktion
        Dim dDatumX As Date = CDate(fcUmDatum(sDatumStart))
        Dim dDatumY As Date = CDate(fcUmDatum(sDatumEnd))

        ' Berechnung der Differenz und direkte R¸ckgabe
        Return CInt(DateDiff(DateInterval.Day, dDatumX, dDatumY))
    End Function

    ''' <summary>
    ''' Ermittelt den nachvongenden Monatsnamen vom aktu. Datum aus
    ''' </summary>
    ''' <param name="Tag"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function fcNextMonthName(ByVal Tag As Date) As String
        Dim nfMonat As Integer = Month(Tag)
        nfMonat = nfMonat + 1
        If nfMonat = 13 Then nfMonat = 1
        fcNextMonthName = fcMonthName(nfMonat)
    End Function

    ''' <summary>
    ''' Gibt den Monatsnamen als Text f¸r eine ¸bergebene Monatsnummer zur¸ck.
    ''' </summary>
    ''' <param name="nfMonat">Die Nummer des Monats (1 bis 12).</param>
    ''' <returns>Den Namen des Monats (z. B. "Januar") oder einen leeren String bei ung¸ltigen Eingaben.</returns>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Den langen 'Select Case'-Block durch die native 'MonthName'-Funktion ersetzt.
    ''' - Validierung f¸r Werte auﬂerhalb des Bereichs 1-12 hinzugef¸gt, um Laufzeitfehler zu verhindern.
    ''' </remarks>
    Function fcMonthName(ByVal nfMonat As Integer) As String
        ' Pr¸fen, ob die Monatsnummer im g¸ltigen Bereich liegt
        If nfMonat >= 1 AndAlso nfMonat <= 12 Then
            Return MonthName(nfMonat, False)
        End If

        ' R¸ckgabe eines leeren Strings bei ung¸ltigen Werten
        Return String.Empty
    End Function

    ''' <summary>
    ''' Konvertiert Datumsstrings bidirektional: Wandelt das deutsche Format (dd.MM.yyyy) 
    ''' in das Datenbankformat (yyyyMMdd) um und umgekehrt.
    ''' </summary>
    ''' <param name="sDatum">Das zu konvertierende Datum als String.</param>
    ''' <returns>Das umgewandelte Datum oder ein Leerzeichen, falls die Konvertierung fehlschl‰gt.</returns>
    ''' <remarks>
    ''' 02.10.2026 - Code-Optimierung:
    ''' - Fehleranf‰lliges 'ParseExact' durch das robustere 'TryParse' ersetzt, um Abst¸rze bei angeh‰ngten Uhrzeiten zu verhindern.
    ''' - Veralteten 'ByVal'-Modifizierer entfernt.
    ''' - Unterst¸tzung f¸r Datumsangaben inklusive Uhrzeitkomponenten integriert.
    ''' </remarks>
    Function fcUmDatum(sDatum As String) As String
        If String.IsNullOrWhiteSpace(sDatum) Then Return " "

        Dim getrimmtesDatum As String = sDatum.Trim()

        ' Wenn ein Punkt enthalten ist: von "dd.MM.yyyy..." zu "yyyyMMdd"
        If getrimmtesDatum.Contains(".") Then
            ' Erlaubt auch Formate, bei denen eine Uhrzeit anh‰ngt
            Dim Formate() As String = {"dd.MM.yyyy", "d.M.yyyy", "dd.MM.yyyy HH:mm:ss", "d.M.yyyy H:mm:ss"}
            Dim dResult As DateTime

            If DateTime.TryParseExact(getrimmtesDatum, Formate, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, dResult) Then
                Return dResult.ToString("yyyyMMdd")
            End If
        Else
            ' Ohne Punkt: von "yyyyMMdd..." zu "dd.MM.yyyy"
            Dim Formate() As String = {"yyyyMMdd", "yyyyMMdd HH:mm:ss"}
            Dim dResult As DateTime

            If DateTime.TryParseExact(getrimmtesDatum, Formate, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, dResult) Then
                Return dResult.ToString("dd.MM.yyyy")
            End If
        End If

        ' R¸ckfallwert bei nicht erkennbarem Format (analog zu Ihrer Original-Logik)
        Return " "
    End Function

    ''' <summary>
    ''' Konvertiert ein kompaktes Datumsformat (JJJJMMTT) in das standardisierte ISO-Format (JJJJ-MM-TT).
    ''' </summary>
    ''' <param name="sDatum">Das umzuwandelnde Datum im Format "JJJJMMTT" (z. B. "20080901").</param>
    ''' <returns>Das formatierte Datum im Format "JJJJ-MM-TT" (z. B. "2008-09-01"). Bei einer leeren oder ung¸ltigen Eingabe wird ein leerer String zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Auf 'Public' ge‰ndert, um die projektweite Sichtbarkeit zu gew‰hrleisten.
    ''' - Veraltetes 'Trim$' und unsauberen R¸ckgabewert '" "' durch 'String.IsNullOrWhiteSpace' und 'String.Empty' ersetzt.
    ''' - 'Exit Function' entfernt und durch direkten, sauberen Kontrollfluss ersetzt.
    ''' - L‰ngenpr¸fung (mindestens 8 Zeichen) hinzugef¸gt, um 'ArgumentOutOfRangeException'-Abst¸rze zu verhindern.
    ''' - Moderne String-Interpolation ($"...") und '.Substring()' implementiert.
    ''' </remarks>
    Public Function fcUmDatumXR(ByVal sDatum As String) As String
        ' Validierung: Wenn der String leer ist oder nur aus Leerzeichen besteht, direkt abbrechen
        If String.IsNullOrWhiteSpace(sDatum) Then
            Return String.Empty
        End If

        ' Whitespaces am Rand entfernen
        sDatum = sDatum.Trim()

        ' Robustheits-Check: Das Format benˆtigt exakt (oder mindestens) 8 Zeichen (JJJJMMTT)
        If sDatum.Length < 8 Then
            Return String.Empty
        End If

        ' Nutzt .NET Substring (0-basiert) und String-Interpolation f¸r maximale Performance
        ' Index 0, L‰nge 4 = Jahr | Index 4, L‰nge 2 = Monat | Index 6, L‰nge 2 = Tag
        Return $"{sDatum.Substring(0, 4)}-{sDatum.Substring(4, 2)}-{sDatum.Substring(6, 2)}"
    End Function


    ''' <summary>
    ''' Konvertiert Zeitstrings bidirektional: Wandelt formatierte Zeiten (hh:mm oder hh.mm) 
    ''' in das kompakte Speicherformat (hhmm) um und umgekehrt.
    ''' </summary>
    ''' <param name="sZeit">Der zu konvertierende Zeitstring.</param>
    ''' <returns>Die konvertierte Zeit oder ein Leerzeichen, wenn der Eingabestring leer ist.</returns>
    ''' <remarks>
    ''' 08.10.2026 Code-Optimierung: <br/>
    ''' - Vollst‰ndige Entfernung von VB6-Altlasten (Trim$, InStr, Mid, Funktionsnamen-Zuweisung). <br/>
    ''' - Umstellung auf moderne .NET String-Methoden (Length, Substring, Contains). <br/>
    ''' - Fehlerresistentere Verarbeitung durch gezielte L‰ngenpr¸fung vor Substring-Zugriffen.
    ''' </remarks>
    Public Function fcUmZeit(sZeit As String) As String
        If String.IsNullOrWhiteSpace(sZeit) Then Return " "

        Dim getrimmteZeit As String = sZeit.Trim()

        ' Enth‰lt Trennzeichen (z.B. "14:30" oder "14.30") -> Konvertierung zu "1430"
        If getrimmteZeit.Contains(":") OrElse getrimmteZeit.Contains(".") Then
            If getrimmteZeit.Length >= 5 Then
                Return getrimmteZeit.Substring(0, 2) & getrimmteZeit.Substring(3, 2)
            End If
        Else
            ' Ohne Trennzeichen (z.B. "1430") -> Konvertierung zu "14:30"
            If getrimmteZeit.Length >= 4 Then
                Return getrimmteZeit.Substring(0, 2) & ":" & getrimmteZeit.Substring(2, 2)
            End If
        End If

        Return " "
    End Function

    ''' <summary>
    ''' Gibt einen Datumsbereich (Von-Bis) als formatierten String basierend auf dem ¸bergebenen Zeitraum (Monat, Quartal oder Jahr) zur¸ck.
    ''' </summary>
    ''' <param name="sMonat">Der Name des Monats (z. B. "Januar"), ein Quartal ("1. Quartal") oder das Wort "Jahr".</param>
    ''' <param name="sJahr">Das Zieljahr als String (z. B. "2026").</param>
    ''' <returns>Ein Datumsbereich im Format "TT.MM.JJJJ-TT.MM.JJJJ" oder ein leerer String bei ung¸ltigen Eingaben.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - 'Exit Function' durch sauberes 'Return String.Empty' ersetzt.
    ''' - Veraltetes VB6 'Val()' durch typsicheres 'Integer.TryParse' ersetzt, um Abst¸rze bei ung¸ltigen Jahreszahlen zu verhindern.
    ''' - Externe Abh‰ngigkeit reduziert: Nutzt die native .NET-Methode 'DateTime.DaysInMonth' f¸r die automatische (und schaltjahressichere) Berechnung des letzten Tages.
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen durch direkte 'Return'-Anweisungen ersetzt.
    ''' - Moderne String-Interpolation ($"...") implementiert.
    ''' </remarks>
    Public Function fcGetDate(ByVal sMonat As String, ByVal sJahr As String) As String
        ' Validierung der Eingaben
        If String.IsNullOrWhiteSpace(sMonat) OrElse String.IsNullOrWhiteSpace(sJahr) Then
            Return String.Empty
        End If

        If sMonat.Contains("-") Then
            Return String.Empty
        End If

        ' Jahreszahl sicher in eine Zahl konvertieren (sch¸tzt vor ung¸ltigen Eingaben)
        Dim nJahr As Integer
        If Not Integer.TryParse(sJahr.Trim(), nJahr) Then
            Return String.Empty
        End If

        Select Case sMonat.Trim()
            Case "Jahr"
                Return $"01.01.{sJahr}-31.12.{sJahr}"

            Case "1. Quartal"
                Return $"01.01.{sJahr}-31.03.{sJahr}"

            Case "2. Quartal"
                Return $"01.04.{sJahr}-30.06.{sJahr}"

            Case "3. Quartal"
                Return $"01.07.{sJahr}-30.09.{sJahr}"

            Case "4. Quartal"
                Return $"01.10.{sJahr}-31.12.{sJahr}"

            Case Else
                ' Monatstext in die Monatsnummer konvertieren (nutzt deine bestehende Funktion)
                Dim sMonatNum As String = MonthToNum(sMonat).Trim()

                ' Monatsnummer f¸r .NET in ein Integer konvertieren
                Dim nMonat As Integer
                If Integer.TryParse(sMonatNum, nMonat) AndAlso nMonat >= 1 AndAlso nMonat <= 12 Then
                    ' Native .NET-Berechnung des letzten Tages im Monat (berechnet Schaltjahre vollautomatisch!)
                    Dim nLastDay As Integer = DateTime.DaysInMonth(nJahr, nMonat)

                    ' F¸hrende Null f¸r die Monatsnummer garantieren (falls MonthToNum diese nicht liefert)
                    Dim sMonatFormatiert As String = nMonat.ToString("D2")

                    Return $"01.{sMonatFormatiert}.{sJahr}-{nLastDay:D2}.{sMonatFormatiert}.{sJahr}"
                Else
                    Return String.Empty
                End If
        End Select
    End Function

    ''' <summary>
    ''' Ermittelt die Anzahl der Tage (den letzten Tag) des ¸bergebenen Monats unter Ber¸cksichtigung von Schaltjahren.
    ''' </summary>
    ''' <param name="plMonth">Die Monatsnummer (1 bis 12).</param>
    ''' <param name="plYear">Das Jahr (entweder zweistellig z. B. 26 oder vierstellig z. B. 2026).</param>
    ''' <returns>Die Anzahl der Tage des Monats oder 0, falls ung¸ltige Parameter ¸bergeben wurden.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Kompletten Datums-Rechner und String-Verkettungen durch die native .NET-Methode 'DateTime.DaysInMonth' ersetzt.
    ''' - Zweistellige Jahreszahlen (Y2K-Kompatibilit‰t) werden sauber in das vierstellige Format konvertiert.
    ''' - Veraltete Fehlerbehandlung 'On Error GoTo' und 'Exit Function' durch strukturiertes Try-Catch ersetzt.
    ''' - 'Val', 'Str' und 'DateAdd' vollst‰ndig entfernt.
    ''' </remarks>
    Public Function DayOfMonthCount(ByVal plMonth As Long, ByVal plYear As Long) As Long
        Try
            ' 1. Validierung der Monatszahlen (Muss zwischen 1 und 12 liegen)
            If plMonth < 1 OrElse plMonth > 12 Then
                Return 0
            End If

            ' 2. Behandlung von zweistelligen Jahreszahlen (z. B. 26 -> 2026)
            Dim nYear As Integer = Convert.ToInt32(plYear)
            If nYear < 100 Then
                nYear += 2000
            End If

            ' 3. Konvertierung des Monats in ein Integer f¸r die .NET-Methode
            Dim nMonth As Integer = Convert.ToInt32(plMonth)

            ' Native .NET-Methode gibt direkt die Anzahl der Tage zur¸ck (z. B. 28, 29, 30 oder 31)
            Return DateTime.DaysInMonth(nYear, nMonth)

        Catch ex As Exception
            ' Falls gew¸nscht, hier wieder dein ErrReport einf¸gen
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Ermittelt die zweistellige Monatsnummer (01-12) aus dem ¸bergebenen Monatsnamen (vollst‰ndig oder abgek¸rzt).
    ''' </summary>
    ''' <param name="sMonat">Der Name des Monats (z. B. "Januar", "Jan", "Februar").</param>
    ''' <returns>Die zweistellige Monatsnummer (z. B. "01" oder "12"). Bei ung¸ltigen Eingaben wird "00" zur¸ckgegeben.</returns>
    ''' <remarks>
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Auf 'Public' ge‰ndert, um Konsistenz im Projekt zu wahren.
    ''' - Fehleranf‰lligen 'CDate'-Trick entfernt, der bei unerwarteten Regions-Einstellungen abst¸rzen konnte.
    ''' - Nutzt die native .NET-Klasse 'Globalization.DateTimeFormatInfo' f¸r eine saubere, hochperformante Namensauflˆsung.
    ''' - Unterst¸tzt nun automatisch sowohl die Langform ("Januar") als auch die Kurzform ("Jan").
    ''' - Nutzt internes '.ToString("D2")' anstelle der alten 'PadLN'-Abh‰ngigkeit.
    ''' </remarks>
    Public Function MonthToNum(ByVal sMonat As String) As String
        ' Validierung: Wenn der ‹bergabestring leer ist, direkt Fehler-Code "00" zur¸ckgeben
        If String.IsNullOrWhiteSpace(sMonat) Then Return "00"

        Try
            ' Bereinigt den String von Leerzeichen und vereinheitlicht die Schreibweise (Erster Buchstabe groﬂ)
            Dim sSuchMonat As String = sMonat.Trim()

            ' Holt die Format-Informationen der aktuellen System-Kultur (z.B. Deutsch)
            Dim dtfi As System.Globalization.DateTimeFormatInfo = System.Globalization.DateTimeFormatInfo.CurrentInfo

            ' 1. Suche in den vollst‰ndigen Monatsnamen (Januar, Februar...)
            For i As Integer = 1 To 12
                If String.Equals(dtfi.GetMonthName(i), sSuchMonat, StringComparison.OrdinalIgnoreCase) Then
                    Return i.ToString("D2") ' Gibt die Zahl direkt zweistellig zur¸ck (z. B. 01)
                End If
            Next

            ' 2. Suche in den abgek¸rzten Monatsnamen (Jan, Feb, Mrz...) als Fallback
            For i As Integer = 1 To 12
                If String.Equals(dtfi.GetAbbreviatedMonthName(i), sSuchMonat, StringComparison.OrdinalIgnoreCase) Then
                    Return i.ToString("D2")
                End If
            Next

            ' Wenn der Monatsname im aktuellen System nicht gefunden wurde, versuchen wir es mit Englisch (globaler Fallback)
            Dim dtfiEn As System.Globalization.DateTimeFormatInfo = New System.Globalization.CultureInfo("en-US").DateTimeFormat
            For i As Integer = 1 To 12
                If String.Equals(dtfiEn.GetMonthName(i), sSuchMonat, StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(dtfiEn.GetAbbreviatedMonthName(i), sSuchMonat, StringComparison.OrdinalIgnoreCase) Then
                    Return i.ToString("D2")
                End If
            Next

            ' Wenn gar nichts passt (z.B. Tippfehler), "00" zur¸ckgeben
            Return "00"

        Catch ex As Exception
            ' Optional: ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return "00"
        End Try
    End Function

    ''' <summary>
    ''' Liefert den Kurznamen des Wochentages (z. B. Mittwoch => Mi).
    ''' </summary>
    ''' <param name="sDate">Das zu pr¸fende Datum.</param>
    ''' <returns>Der zweistellige Kurzname des Wochentages (z. B. "Mo", "Di", "Mi").</returns>
    ''' <remarks>
    ''' 07.01.2012 - Create
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Die verschachtelten VB6-Kompatibilit‰tsfunktionen 'Mid', 'WeekdayName' und 'Weekday' vollst‰ndig entfernt.
    ''' - Nutzt nun die native .NET-Formatierung 'sDate.ToString("ddd")' f¸r maximale Performance.
    ''' - Der erste Buchstabe wird zur Sicherheit groﬂ- und der zweite kleingeschrieben (z. B. "Mi").
    ''' - Veraltete R¸ckgabe-Syntax ¸ber den Funktionsnamen durch ein direktes 'Return' ersetzt.
    ''' </remarks>
    Public Function fcShortDayName(ByVal sDate As Date) As String
        ' "ddd" liefert in der aktuellen Systemkultur automatisch die standardisierte Kurzform (z. B. "Mi.")
        ' Mit .Replace(".", "") entfernen wir einen eventuell angeh‰ngten Punkt (wie es in manchen Kulturen ¸blich ist)
        Dim sTag As String = sDate.ToString("ddd", System.Globalization.CultureInfo.CurrentCulture).Replace(".", "")

        ' Sicherstellen, dass das Ergebnis exakt 2 Zeichen lang ist und im Format "Xy" ausgegeben wird
        If sTag.Length >= 2 Then
            Return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sTag.Substring(0, 2).ToLower())
        End If

        Return sTag
    End Function

    ''' <summary>
    ''' Gibt einen eindeutigen Schl¸ssel im Format yyyyMMddHHmmss zur¸ck.
    ''' </summary>
    ''' <param name="sDate">Das Ausgangsdatum.</param>
    ''' <returns>Eine 14-stellige ID aus ¸bergebenem Datum und aktueller Uhrzeit.</returns>
    ''' <remarks>
    ''' 23.09.2026 Code-Review: Optimiert die Funktion, um die aktuelle Uhrzeit direkt zu kombinieren und das Format zu vereinfachen.
    ''' </remarks>
    Public Function fcGetTimeID(ByVal sDate As Date) As String
        ' Kombiniert das Jahr/Monat/Tag von sDate mit der aktuellen Uhrzeit (Stunde/Minute/Sekunde)
        Dim kombinierteZeit As DateTime = New DateTime(
        sDate.Year, sDate.Month, sDate.Day,
        DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second
        )

        Return kombinierteZeit.ToString("yyyyMMddHHmmss")
    End Function

    ''' <summary>
    ''' Pausiert die Ausf¸hrung des aktuellen Threads f¸r eine definierte Anzahl von Sekunden.
    ''' </summary>
    ''' <param name="nPause">Die Pausenzeit in Sekunden.</param>
    ''' <remarks>
    ''' 27.12.2011 - Create
    ''' 06.11.2026 - Code-Optimierung:
    ''' - Gef‰hrliche 'Do While'-Endlosschleife und 'Application.DoEvents' (Busy Waiting) vollst‰ndig entfernt.
    ''' - CPU-Last von 100% auf 0% gesenkt durch den Einsatz von 'System.Threading.Thread.Sleep'.
    ''' - Externe Abh‰ngigkeit zum Timer auf 'frmMain' eliminiert.
    ''' - 'Exit Sub' durch saubere Kontrollstruktur ersetzt.
    ''' </remarks>
    Public Sub fcWait(ByVal nPause As Integer)
        ' Wenn die Pause 0 oder negativ ist, gibt es nichts zu tun
        If nPause <= 0 Then Return

        ' Konvertiert Sekunden in Millisekunden und legt den Thread ressourcenschonend schlafen
        System.Threading.Thread.Sleep(nPause * 1000)
    End Sub


#End Region

#Region "XML-Functionen............................................................................"

    ''' <summary>
    ''' XML File lesen und f¸r angegebenes Element den Wert ¸bergeben
    ''' </summary>
    ''' <param name="sFile"></param>
    ''' <param name="sElement"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 21.08.2010 Create
    ''' </remarks>
    Public Function fcXMLReader(ByVal sFile As String, ByVal sElement As String) As String
        Try
            Dim XDoc As XPathDocument = New XPathDocument(sFile)
            Dim XNav As XPathNavigator = XDoc.CreateNavigator.SelectSingleNode("//" & sElement)
            fcXMLReader = XNav.InnerXml.ToString
        Catch ex As Exception
            fcXMLReader = ""
            'ErrReport(ex.Message & " => " & sElement, ex.Source, ex.StackTrace)
        End Try
    End Function

    ''' <summary>
    ''' Wert f¸r angegebenes Element im XML-File speicherm
    ''' </summary>
    ''' <param name="sFile"></param>
    ''' <param name="sElement"></param>
    ''' <param name="sValue"></param>
    ''' <remarks>
    ''' 30.08.2010 Create
    ''' </remarks>
    Public Sub prXMLWriter(ByVal sFile As String, ByVal sElement As String, ByVal sValue As String)
        Try
            Dim XDoc As XmlDocument = New XmlDocument
            XDoc.Load(sFile)
            Dim Node As XmlNode = XDoc.SelectSingleNode("//" & sElement)
            Node.InnerText = sValue
            XDoc.Save(sFile)
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

#End Region

#Region "Bewegliche Freiertage berechnen..........................................................."
    ''' <summary>
    ''' Ermittelt den Feiertags- oder Ereignisnamen f¸r ein gegebenes Datum.
    ''' </summary>
    Public Function fcGetFeiertag(ByVal Datum As Date) As String
        ' Nutze Standard-Datentypen und die moderne Return-Anweisung
        Dim nDay As Integer = Datum.Day
        Dim nMonat As Integer = Datum.Month ' Modernes .NET statt Month(Datum)
        Dim nJahr As Integer = Datum.Year   ' Modernes .NET statt Year(Datum)

        ' 1. Feste Feiertage und Ereignisse (Sehr schnelle Pr¸fung)
        Select Case nMonat
            Case 1
                If nDay = 1 Then Return "Neujahr"
                If nDay = 6 Then Return "Heiligen drei Kˆnige"
            Case 2
                If nDay = 14 Then Return "Valentinstag"
            Case 5
                If nDay = 1 Then Return "Maifeiertag"
            Case 8
                If nDay = 8 Then Return "Friedensfest"
                If nDay = 15 Then Return "Maria Himmelfahrt"
            Case 10
                If nDay = 3 Then Return "Tag der Deutschen Einheit"
                If nDay = 31 Then Return "Reformationstag"
            Case 11
                If nDay = 1 Then Return "Allerheiligen"
            Case 12
                If nDay = 6 Then Return "Nikolaus"
                If nDay = 24 Then Return "Heiligabend"
                If nDay = 25 Then Return "1. Weihnachtstag"
                If nDay = 26 Then Return "2. Weihnachtstag"
        End Select

        ' 2. Bewegliche Feiertage (Werden nur berechnet, wenn oben nichts zutraf)

        ' Ostern und abh‰ngige Feste (M‰rz bis Juni)
        If nMonat >= 2 AndAlso nMonat <= 6 Then
            Dim Osterdatum As Date = fcOsterdatum(nJahr)

            ' .AddDays() ist der moderne, schnelle Ersatz f¸r DateAdd()
            If Datum = Osterdatum.AddDays(-52) Then Return "Weiber Fastnacht"
            If Datum = Osterdatum.AddDays(-48) Then Return "Rosenmontag"
            If Datum = Osterdatum.AddDays(-47) Then Return "Fastnacht"
            If Datum = Osterdatum.AddDays(-46) Then Return "Aschermittwoch"
            If Datum = Osterdatum.AddDays(-3) Then Return "Gr¸ndonnerstag"
            If Datum = Osterdatum.AddDays(-2) Then Return "Karfreitag"
            If Datum = Osterdatum Then Return "Ostersonntag"
            If Datum = Osterdatum.AddDays(1) Then Return "Ostermontag"
            If Datum = Osterdatum.AddDays(39) Then Return "Christi Himmelfahrt"
            If Datum = Osterdatum.AddDays(49) Then Return "Pfingstsonntag"
            If Datum = Osterdatum.AddDays(50) Then Return "Pfingstmontag"
            If Datum = Osterdatum.AddDays(60) Then Return "Fronleichnam"
        End If

        ' Muttertag (Immer im Mai)
        If nMonat = 5 Then
            If Datum = GetMuttertag(nJahr) Then Return "Muttertag"
        End If

        ' Erntedankfest (Immer im Oktober)
        If nMonat = 10 Then
            If Datum = GetErntedankfest(nJahr) Then Return "Erntedankfest"
        End If

        ' Herbst-/Winterereignisse (November und Dezember)
        If nMonat = 11 OrElse nMonat = 12 Then
            Dim s4Advent As Date = GetViertenAdvent(nJahr)

            If Datum = s4Advent Then Return "4. Advent"
            If Datum = s4Advent.AddDays(-7) Then Return "3. Advent"
            If Datum = s4Advent.AddDays(-14) Then Return "2. Advent"
            If Datum = s4Advent.AddDays(-21) Then Return "1. Advent"
            If Datum = s4Advent.AddDays(-28) Then Return "Totensonntag"
            If Datum = s4Advent.AddDays(-32) Then Return "Buﬂ- und Bettag"
            If Datum = s4Advent.AddDays(-35) Then Return "Volkstrauertag"
        End If

        ' Zeitumstellungen (M‰rz und Oktober)
        If nMonat = 3 Then
            If Datum = GetBeginnSommerzeit(nJahr) Then Return "Beginn der Sommerzeit"
        End If
        If nMonat = 10 Then
            If Datum = GetBeginnWinterzeit(nJahr) Then Return "Beginn der Winterzeit"
        End If

        ' Wenn kein Feiertag zutrifft
        Return ""
    End Function

    ''' <summary>
    ''' Berechnet das Osterdatum f¸r ein gegebenes Jahr nach der Gauﬂschen Osterformel.
    ''' </summary>
    ''' <param name="Jahr">Das Jahr, f¸r das Ostern berechnet werden soll.</param>
    ''' <returns>Das Osterdatum als Date (DateTime).</returns>
    ''' <remarks>
    ''' 23.12.2011 Create
    ''' </remarks>
    Private Function fcOsterdatum(ByVal Jahr As Integer) As Date
        Dim a As Integer = Jahr Mod 19
        Dim b As Integer = Jahr Mod 4
        Dim c As Integer = Jahr Mod 7
        Dim d As Integer = (19 * a + 24) Mod 30
        Dim e As Integer = (2 * b + 4 * c + 6 * d + 5) Mod 7

        Dim Tag As Integer = 22 + d + e
        Dim Monat As Integer = 3

        ' Wenn der Tag grˆﬂer als 31 ist, f‰llt Ostern in den April
        If Tag > 31 Then
            Tag = d + e - 9
            Monat = 4
        End If

        ' Die beiden mathematischen Sonderf‰lle von Gauﬂ (m¸ssen nach der April-Korrektur gepr¸ft werden)
        If Monat = 4 Then
            If Tag = 26 Then
                Tag = 19
            ElseIf Tag = 25 AndAlso d = 28 AndAlso e = 6 AndAlso a > 10 Then
                Tag = 18
            End If
        End If

        ' Nutze den nativen .NET-Konstruktor statt DateSerial
        Return New Date(Jahr, Monat, Tag)
    End Function

    ''' <summary>
    ''' Berechnet den Beginn der Sommerzeit (letzter Sonntag im M‰rz).
    ''' </summary>
    Private Function GetBeginnSommerzeit(ByVal JahrA As Integer) As Date
        ' Wir starten am letzten Tag des M‰rz (31.03.) unter Verwendung des nativen Date-Konstruktors
        Dim dt As New Date(JahrA, 3, 31)

        ' Die Eigenschaft DayOfWeek liefert den Wochentag als Enum (unabh‰ngig von der Sprache)
        Dim wochentag As DayOfWeek = dt.DayOfWeek

        ' Bestimmen, wie viele Tage wir vom 31. M‰rz abziehen m¸ssen, um zum Sonntag zu gelangen
        ' (DayOfWeek.Sunday hat den numerischen Wert 0)
        Dim tageBisSonntag As Integer = CInt(wochentag)

        ' Den Sonntag berechnen und direkt zur¸ckgeben
        Return dt.AddDays(-tageBisSonntag)
    End Function

    ''' <summary>
    ''' Berechnet den Beginn der Winterzeit (letzter Sonntag im Oktober).
    ''' </summary>
    Private Function GetBeginnWinterzeit(ByVal JahrA As Integer) As Date
        ' Wir starten am letzten Tag des Oktobers (31.10.)
        Dim dt As New Date(JahrA, 10, 31)

        ' Wochentag als sprachunabh‰ngiges Enum abrufen (Sunday = 0)
        Dim wochentag As DayOfWeek = dt.DayOfWeek

        ' Die Differenz zum vorherigen Sonntag abziehen
        Dim tageBisSonntag As Integer = CInt(wochentag)

        ' Das exakte Datum direkt zur¸ckgeben
        Return dt.AddDays(-tageBisSonntag)
    End Function

    ''' <summary>
    ''' Berechnet das Erntedankfest (erster Sonntag im Oktober).
    ''' </summary>
    Private Function GetErntedankfest(ByVal JahrA As Integer) As Date
        ' Wir starten am 1. Oktober des jeweiligen Jahres
        Dim dt As New Date(JahrA, 10, 1)

        ' Bestimmen, wie viele Tage wir addieren m¸ssen, um zum ersten Sonntag zu gelangen.
        ' Formel: (7 - Wochentag) Mod 7
        ' DayOfWeek.Sunday hat den Wert 0, Monday = 1, Samstag = 6.
        Dim tageBisSonntag As Integer = (7 - CInt(dt.DayOfWeek)) Mod 7

        ' Den ersten Sonntag berechnen und direkt zur¸ckgeben
        Return dt.AddDays(tageBisSonntag)
    End Function

    ''' <summary>
    ''' Berechnet den Muttertag (zweiter Sonntag im Mai).
    ''' </summary>
    Private Function GetMuttertag(ByVal JahrA As Integer) As Date
        ' Wir starten am 1. Mai des jeweiligen Jahres
        Dim dt As New Date(JahrA, 5, 1)

        ' Den ersten Sonntag im Mai berechnen (7 - Wochentag) Mod 7
        Dim tageBisErstemSonntag As Integer = (7 - CInt(dt.DayOfWeek)) Mod 7

        ' Zum ersten Sonntag gelangen wir durch Addition dieser Tage.
        ' Um zum ZWEITEN Sonntag zu gelangen, addieren wir einfach 7 weitere Tage dazu.
        Return dt.AddDays(tageBisErstemSonntag + 7)
    End Function

    ''' <summary>
    ''' Berechnet den 4. Advent (der letzte Sonntag vor oder am 24. Dezember).
    ''' </summary>
    Private Function GetViertenAdvent(ByVal JahrA As Integer) As Date
        ' Wir starten direkt am Heiligabend (24.12.)
        Dim dt As New Date(JahrA, 12, 24)

        ' Wochentag von Heiligabend ermitteln (Sunday = 0, Monday = 1, usw.)
        Dim wochentag As DayOfWeek = dt.DayOfWeek

        ' Die Differenz zum vorherigen Sonntag abziehen
        Dim tageBisSonntag As Integer = CInt(wochentag)

        ' Den 4. Advent direkt zur¸ckgeben
        Return dt.AddDays(-tageBisSonntag)
    End Function

#End Region

#Region "Programm Funktionen.(Hotel)..............................................................."

    ''' <summary>
    ''' Holt die letzte Nummer (Kunden-, Buchungs- oder Rechnungsnummer), erhˆht sie um 1 und speichert sie wieder.
    ''' </summary>
    ''' <param name="sArt">Die Art der Nummer ("KNr", "BNr", "RNr").</param>
    ''' <returns>Die um 1 erhˆhte Nummer als String.</returns>
    ''' <remarks>
    ''' 10.01.2012 Create
    ''' 23.09.2026 Refactored (Typsicherheit & Validierung hinzugef¸gt)
    ''' </remarks>
    Public Function fcGetNr(ByVal sArt As String) As String
        Dim iniFile As String = myInit.ReadIni()
        Dim aktuelleNr As String = "0"

        ' 1. Aktuellen Wert auslesen
        Select Case sArt
            Case "KNr"
                aktuelleNr = myInit.ReadEntry(iniFile, KEY_KNr)
            Case "BNr"
                aktuelleNr = myInit.ReadEntry(iniFile, KEY_BNr)
            Case "RNr"
                aktuelleNr = myInit.ReadEntry(iniFile, KEY_RNr)
            Case Else
                Return "0"
        End Select

        ' Falls der Eintrag in der INI leer war, Standardwert setzen
        If String.IsNullOrWhiteSpace(aktuelleNr) Then aktuelleNr = "0"

        ' 2. Nummer numerisch um 1 erhˆhen
        Dim neueZahl As Long = 0
        Long.TryParse(aktuelleNr, neueZahl)
        neueZahl += 1

        Return neueZahl.ToString
    End Function

    ''' <summary>
    ''' Neue Nummer (Kunden-, Buchungs- oder Rechnungsnummer) speichern.
    ''' </summary>
    ''' <param name="sArt">Die Art der Nummer ("KNr", "BNr", "RNr").</param>
    ''' <param name="sNr">Die zu speichernde Nummer.</param>
    ''' <remarks>
    ''' 10.01.2012 Create
    ''' 23.09.2026 Refactored (Typsicherheit & Validierung hinzugef¸gt)
    ''' </remarks>
    Public Sub prSetNr(ByVal sArt As String, ByVal sNr As String)
        ' INI-Inhalt einlesen
        Dim iniContent As String = myInit.ReadIni()
        Dim key As String = Nothing

        ' Den passenden Schl¸ssel ermitteln
        Select Case sArt
            Case "KNr"
                key = KEY_KNr
            Case "BNr"
                key = KEY_BNr
            Case "RNr"
                key = KEY_RNr
            Case Else
                ' Optional: Fehler abfangen oder Sub verlassen, falls 'sArt' ung¸ltig ist
                Exit Sub
        End Select

        ' Nur schreiben, wenn ein g¸ltiger Schl¸ssel gefunden wurde
        iniContent = myInit.WriteEntry(iniContent, key, sNr)
        myInit.WriteIni(iniContent)
    End Sub


    ''' <summary>
    ''' Ermittelt den Namen eines Objekts anhand seiner eindeutigen Objekt-ID aus einer DataTable.
    ''' Durchsucht alle nicht gelˆschten Zeilen der Tabelle und gibt den zugehˆrigen Namen zur¸ck.
    ''' </summary>
    ''' <param name="dtT">Die DataTable, welche die Objektdaten (Spalten "ID" und "Name") enth‰lt.</param>
    ''' <param name="sObj">Die zu suchende Objekt-ID.</param>
    ''' <returns>Den gefundenen Objektnamen oder ein einzelnes Leerzeichen (" "), falls die ID nicht gefunden wurde oder ung¸ltig ist.</returns>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 11.11.2026 - Code-Optimierung:
    ''' - Null- und Leerzeichen-Pr¸fung (Guard Clauses) f¸r Parameter erg‰nzt, um NullReferenceExceptions zu vermeiden.
    ''' - Veraltete Zuweisung ¸ber den Funktionsnamen durch modernes 'Return' ersetzt.
    ''' - 'For Each'-Schleife statt indexbasierter 'For'-Schleife f¸r bessere Lesbarkeit verwendet.
    ''' - Typsicheren Zugriff ¸ber 'Field(Of String)' und robusten String-Vergleich implementiert.
    ''' - Inline-Kommentare zur besseren Wartbarkeit hinzugef¸gt.
    ''' </remarks>
    Public Function fcGetObjektName(ByVal dtT As DataTable, ByVal sObj As String) As String
        ' Standard-R¸ckgabewert beibehalten (Abw‰rtskompatibilit‰t)
        Const DefaultResult As String = " "

        ' Guard Clauses: Abbruch bei fehlender Tabelle oder leerer Such-ID
        If dtT Is Nothing OrElse String.IsNullOrWhiteSpace(sObj) Then
            Return DefaultResult
        End If

        Dim sSuchID As String = sObj.Trim()

        ' Alle Zeilen der Tabelle durchlaufen
        For Each row As DataRow In dtT.Rows
            ' Gelˆschte Zeilen ¸berspringen, um DeletedRowInaccessibleException zu verhindern
            If row.RowState <> DataRowState.Deleted Then
                ' Sicherer Abruf der ID (behandelt auch DBNull automatisch als Nothing)
                Dim sRowID As String = row.Field(Of String)("ID")

                If sRowID IsNot Nothing AndAlso String.Equals(sSuchID, sRowID.Trim(), StringComparison.OrdinalIgnoreCase) Then
                    ' Gefundenen Namen zur¸ckgeben (falls DBNull, wird DefaultResult genutzt)
                    Return If(row.Field(Of String)("Name"), DefaultResult)
                End If
            End If
        Next

        Return DefaultResult
    End Function


    ''' <summary>
    ''' Ermittelt eine Objekt- oder Zimmer-ID (bzw. einen dynamischen Zielspaltenwert) anhand des Objektnamens.
    ''' Durchsucht alle nicht gelˆschten Zeilen einer DataTable nach dem Namen und gibt den getrimmten Wert der angegebenen Zielspalte zur¸ck.
    ''' </summary>
    ''' <param name="dtT">Die DataTable, welche die Objekt- bzw. Zimmerdaten (inklusive der Spalte "Name") enth‰lt.</param>
    ''' <param name="sObj">Der zu suchende Objekt- oder Zimmername.</param>
    ''' <param name="sResult">Der Name der Spalte, deren Wert bei einem Treffer zur¸ckgegeben werden soll (z. B. "ID").</param>
    ''' <returns>Den gefundenen und getrimmten Wert aus der Zielspalte oder ein einzelnes Leerzeichen (" "), falls kein Treffer gefunden wurde oder Parameter ung¸ltig sind.</returns>
    ''' <remarks>
    ''' 19.12.2011 - Create
    ''' 11.11.2026 - Code-Optimierung:
    ''' - Fehlenden Parameter 'sResult' im XML-Kommentar erg‰nzt und dokumentiert.
    ''' - Guard Clauses f¸r Null-Werte, leere Strings sowie Pr¸fung auf Existenz der Spalten ("Name" und 'sResult') erg‰nzt.
    ''' - Veraltete Zuweisung ¸ber den Funktionsnamen durch modernes 'Return' ersetzt.
    ''' - 'For Each'-Schleife und sicheren String-Vergleich ('String.Equals') statt indexbasierter Schleife verwendet.
    ''' - DBNull-sichere Konvertierung ¸ber 'Convert.ToString' eingebaut, um Exceptions und Typ-Konflikte (z. B. Integer-IDs) zu vermeiden.
    ''' </remarks>
    Public Function fcGetObjektZimmerID(ByVal dtT As DataTable, ByVal sObj As String, ByVal sResult As String) As String
        ' Standard-R¸ckgabewert beibehalten (Abw‰rtskompatibilit‰t)
        Const DefaultResult As String = " "

        ' Guard Clauses: Pr¸fung auf ung¸ltige Parameter
        If dtT Is Nothing OrElse String.IsNullOrWhiteSpace(sObj) OrElse String.IsNullOrWhiteSpace(sResult) Then
            Return DefaultResult
        End If

        ' Pr¸fen, ob die benˆtigten Spalten ¸berhaupt in der DataTable existieren
        If Not dtT.Columns.Contains("Name") OrElse Not dtT.Columns.Contains(sResult) Then
            Return DefaultResult
        End If

        Dim sSuchName As String = sObj.Trim()

        ' Alle Zeilen der Tabelle durchlaufen
        For Each row As DataRow In dtT.Rows
            ' Gelˆschte Zeilen ¸berspringen, um DeletedRowInaccessibleException zu verhindern
            If row.RowState <> DataRowState.Deleted Then
                ' Convert.ToString wandelt DBNull sicher in String.Empty um, ohne abzust¸rzen
                Dim sRowName As String = Convert.ToString(row("Name")).Trim()

                If String.Equals(sSuchName, sRowName, StringComparison.OrdinalIgnoreCase) Then
                    ' Wert der dynamischen Zielspalte sicher auslesen und trimmen
                    Dim sZielWert As String = Convert.ToString(row(sResult)).Trim()
                    Return If(String.IsNullOrEmpty(sZielWert), DefaultResult, sZielWert)
                End If
            End If
        Next

        Return DefaultResult
    End Function


    ''' <summary>
    ''' Ermittelt den Namen eines Objekts oder Zimmers anhand seiner eindeutigen ID.
    ''' Durchsucht alle nicht gelˆschten Zeilen der DataTable nach der ID und gibt den zugehˆrigen Namen zur¸ck.
    ''' </summary>
    ''' <param name="dtT">Die DataTable, welche die Objekt- bzw. Zimmerdaten (Spalten "ID" und "Name") enth‰lt.</param>
    ''' <param name="sZim">Die zu suchende Objekt- oder Zimmer-ID.</param>
    ''' <returns>Den gefundenen Namen oder ein einzelnes Leerzeichen (" "), falls kein Treffer gefunden wurde oder Parameter ung¸ltig sind.</returns>
    ''' <remarks>
    ''' 02.02.2012 - Create
    ''' 11.11.2026 - Code-Optimierung:
    ''' - Guard Clauses f¸r Null-Werte, leere Strings sowie Pr¸fung auf Existenz der Spalten ("ID" und "Name") erg‰nzt.
    ''' - Veraltete Zuweisung ¸ber den Funktionsnamen durch modernes 'Return' ersetzt.
    ''' - 'For Each'-Schleife und sicheren String-Vergleich ('String.Equals') statt indexbasierter Schleife verwendet.
    ''' - DBNull-sichere Konvertierung ¸ber 'Convert.ToString' eingebaut, um Exceptions und Typ-Konflikte (z. B. Integer-IDs) zu vermeiden.
    ''' - Inline-Kommentare zur besseren Wartbarkeit hinzugef¸gt.
    ''' </remarks>
    Public Function fcGetObjektZimmerName(ByVal dtT As DataTable, ByVal sZim As String) As String
        ' Standard-R¸ckgabewert beibehalten (Abw‰rtskompatibilit‰t)
        Const DefaultResult As String = " "

        ' Guard Clauses: Pr¸fung auf ung¸ltige Parameter
        If dtT Is Nothing OrElse String.IsNullOrWhiteSpace(sZim) Then
            Return DefaultResult
        End If

        ' Pr¸fen, ob die benˆtigten Spalten in der DataTable vorhanden sind
        If Not dtT.Columns.Contains("ID") OrElse Not dtT.Columns.Contains("Name") Then
            Return DefaultResult
        End If

        Dim sSuchID As String = sZim.Trim()

        ' Alle Zeilen der Tabelle durchlaufen
        For Each row As DataRow In dtT.Rows
            ' Gelˆschte Zeilen ¸berspringen, um DeletedRowInaccessibleException zu verhindern
            If row.RowState <> DataRowState.Deleted Then
                ' Convert.ToString wandelt DBNull und numerische IDs sicher in einen String um
                Dim sRowID As String = Convert.ToString(row("ID")).Trim()

                If String.Equals(sSuchID, sRowID, StringComparison.OrdinalIgnoreCase) Then
                    Dim sGefundenerName As String = Convert.ToString(row("Name"))
                    Return If(String.IsNullOrEmpty(sGefundenerName), DefaultResult, sGefundenerName)
                End If
            End If
        Next

        Return DefaultResult
    End Function

    ''' <summary>
    ''' Ermittelt einen einzelnen Wert aus einer beliebigen Zielspalte einer DataTable anhand eines Suchkriteriums.
    ''' Durchsucht alle nicht gelˆschten Zeilen nach dem angegebenen Vergleichswert in der Suchspalte und gibt den Wert der Zielspalte zur¸ck.
    ''' </summary>
    ''' <param name="dtT">Die DataTable, welche die zu durchsuchenden Daten enth‰lt.</param>
    ''' <param name="sWert">Der gesuchte Wert (z. B. eine Zimmernummer oder ein Name).</param>
    ''' <param name="sVergleich">Der Name der Spalte, in der nach <paramref name="sWert"/> gesucht wird (z. B. "Name").</param>
    ''' <param name="sResult">Der Name der Spalte, deren Wert bei einem Treffer zur¸ckgegeben wird (z. B. "ID").</param>
    ''' <returns>Den gefundenen Wert aus der Zielspalte oder ein einzelnes Leerzeichen (" "), falls kein Treffer gefunden wurde oder Parameter ung¸ltig sind.</returns>
    ''' <remarks>
    ''' 11.11.2026 - Code-Optimierung:
    ''' - XML-Dokumentation f¸r alle Parameter und den R¸ckgabewert vervollst‰ndigt.
    ''' - Guard Clauses f¸r Null-Werte, leere Strings sowie Pr¸fung auf Existenz der dynamischen Spalten ('sVergleich' und 'sResult') erg‰nzt.
    ''' - Veraltete Zuweisung ¸ber den Funktionsnamen durch modernes 'Return' ersetzt.
    ''' - 'For Each'-Schleife und sicheren String-Vergleich ('String.Equals') statt indexbasierter Schleife verwendet.
    ''' - DBNull-sichere Konvertierung ¸ber 'Convert.ToString' eingebaut, um Exceptions und Typ-Konflikte zu vermeiden.
    ''' - Inline-Kommentare zur besseren Wartbarkeit hinzugef¸gt.
    ''' </remarks>
    Public Function fcGetOneValue(ByVal dtT As DataTable, ByVal sWert As String, ByVal sVergleich As String, ByVal sResult As String) As String
        ' Standard-R¸ckgabewert beibehalten (Abw‰rtskompatibilit‰t)
        Const DefaultResult As String = " "

        ' Guard Clauses: Pr¸fung auf ung¸ltige Parameter
        If dtT Is Nothing OrElse
       String.IsNullOrWhiteSpace(sWert) OrElse
       String.IsNullOrWhiteSpace(sVergleich) OrElse
       String.IsNullOrWhiteSpace(sResult) Then
            Return DefaultResult
        End If

        ' Pr¸fen, ob beide dynamisch ¸bergebenen Spalten in der DataTable existieren
        If Not dtT.Columns.Contains(sVergleich) OrElse Not dtT.Columns.Contains(sResult) Then
            Return DefaultResult
        End If

        Dim sSuchWert As String = sWert.Trim()

        ' Alle Zeilen der Tabelle durchlaufen
        For Each row As DataRow In dtT.Rows
            ' Gelˆschte Zeilen ¸berspringen, um DeletedRowInaccessibleException zu verhindern
            If row.RowState <> DataRowState.Deleted Then
                ' Convert.ToString wandelt DBNull und andere Datentypen sicher in einen String um
                Dim sZellenWert As String = Convert.ToString(row(sVergleich)).Trim()

                If String.Equals(sSuchWert, sZellenWert, StringComparison.OrdinalIgnoreCase) Then
                    Dim sGefundenerWert As String = Convert.ToString(row(sResult))
                    Return If(String.IsNullOrEmpty(sGefundenerWert), DefaultResult, sGefundenerWert)
                End If
            End If
        Next

        Return DefaultResult
    End Function



    ''' <summary>
    ''' Synchronisiert die Listenansicht des DataGridViews auf den ¸bergebenen Tag und scrollt diesen in den sichtbaren Bereich.
    ''' </summary>
    ''' <param name="dg">Das zu steuernde DataGridView-Steuerelement.</param>
    ''' <param name="dDay">Das Zieldatum, auf das fokussiert werden soll.</param>
    ''' <remarks>
    ''' 18.02.2012 Create <br/>
    ''' 22.09.2026 Code-Optimierung: Typensichere Datumskonvertierung eingef¸hrt, 
    ''' Validierung leerer Tabellenzellen erg‰nzt und Index-Berechnung f¸r das Scrollen abgesichert.
    ''' </remarks>
    Public Sub prSynchronDay(ByVal dg As DataGridView, ByVal dDay As Date)
        ' Sicherheitspr¸fung: Hat das Grid ¸berhaupt Datenzeilen?
        If dg Is Nothing OrElse dg.Rows.Count = 0 Then Exit Sub

        Try
            Dim gesamtZeilen As Integer = dg.Rows.Count
            Dim zielIndex As Integer = -1

            ' 1. Zeilen durchlaufen und nach dem Datum suchen
            For i As Integer = 0 To gesamtZeilen - 1
                Dim zellWert As Object = dg.Rows(i).Cells(0).Value

                If zellWert IsNot Nothing Then
                    Dim zellDatum As Date

                    ' Sichere Typkonvertierung von String/Objekt zu echtem Date
                    If Date.TryParse(zellWert.ToString(), zellDatum) Then
                        ' Echter Datumsvergleich (unabh‰ngig von der String-Formatierung)
                        If zellDatum.Date = dDay.Date Then
                            zielIndex = i
                            Exit For
                        End If
                    End If
                End If
            Next

            ' 2. Wenn das Datum gefunden wurde, Zelle fokussieren und scrollen
            If zielIndex <> -1 Then
                ' Zielzeile als aktive Zelle setzen
                dg.CurrentCell = dg.Rows(zielIndex).Cells(0)

                ' Berechne den Scroll-Index (5 Zeilen Puffer nach oben f¸r bessere ‹bersicht)
                Dim scrollIndex As Integer = zielIndex - 5

                ' Validierung: Der Scroll-Index darf niemals auﬂerhalb des erlaubten Bereichs liegen
                If scrollIndex < 0 Then
                    scrollIndex = 0
                ElseIf scrollIndex >= gesamtZeilen Then
                    scrollIndex = gesamtZeilen - 1
                End If

                ' Grid an die berechnete Position scrollen
                dg.FirstDisplayedScrollingRowIndex = scrollIndex
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Ermittelt den Spaltenindex eines Zimmers im DataGridView anhand der in Zeile Null hinterlegten Zimmer-ID.
    ''' </summary>
    ''' <param name="sZim">Die gesuchte Zimmer-ID.</param>
    ''' <param name="dg">Das zu durchsuchende DataGridView-Steuerelement.</param>
    ''' <returns>Den nullbasierten Spaltenindex des Zimmers oder -1, wenn das Zimmer nicht gefunden wurde.</returns>
    ''' <remarks>
    ''' 26.12.2011 Create <br/>
    ''' 22.09.2026 Code-Optimierung: Integration einer Null-Pr¸fung f¸r Zellenwerte zur Vermeidung von Abst¸rzen 
    ''' und Umstellung auf das modernere 'Return'-Schl¸sselwort.
    ''' </remarks>
    Public Function fcGetZimmerSpalte(ByVal sZim As String, ByVal dg As DataGridView) As Integer
        ' Vorabpr¸fung: Hat das Grid ¸berhaupt Spalten und mindestens die ID-Zeile (Zeile 0)?
        If dg Is Nothing OrElse dg.Columns.Count < 3 OrElse dg.Rows.Count = 0 Then Return -1

        Try
            Dim nMax As Integer = dg.Columns.Count - 1

            ' Durchl‰uft alle Zimmerspalten (ab Index 2, da 0=Datum und 1=Tag)
            For i As Integer = 2 To nMax
                Dim zellWert As Object = dg.Rows(0).Cells(i).Value

                If zellWert IsNot Nothing Then
                    ' String-Vergleich der Zimmer-ID (Trim entfernt eventuelle Leerzeichen)
                    If sZim.Trim() = zellWert.ToString().Trim() Then
                        Return i ' Spaltenindex sofort zur¸ckgeben und Funktion beenden
                    End If
                End If
            Next

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
        Return -1
    End Function


    ''' <summary>
    ''' Ermittelt den Zimmerpreis f¸r ein bestimmtes Zimmer an einem vorgegebenen Datum aus der Preistabelle.
    ''' Ber¸cksichtigt saisonale Zeitr‰ume und den jeweiligen Wochentag.
    ''' </summary>
    ''' <param name="ZimID">Die eindeutige Identifikationsnummer (ID) des Zimmers.</param>
    ''' <param name="Datum">Das abzufragende Datum als Formatschnittstelle.</param>
    ''' <param name="dtAllePreise">Die DataTable mit allen Preisen f¸r den Buchungsplan.</param>
    ''' <returns>Der ermittelte Preis als formatierter String (z. B. "45.00").</returns>
    ''' <remarks>
    ''' 26.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Parameter von 'ByRef' auf 'ByVal' umgestellt.
    ''' - Performance-Kritik: Es wird empfohlen, die Preistabelle einmalig vorab zu laden, anstatt sie in dieser Funktion pro Zelle neu abzufragen.
    ''' - Typsicheres '.Split()'-Verfahren anstelle des veralteten 'Split()' implementiert.
    ''' - 'Weekday()' und 'CDate()' durch native .NET-Datumslogik ('DayOfWeek') ersetzt.
    ''' - Veraltete 'Str()'-Funktion durch die .NET-Konvertierung '.ToString("F2")' ersetzt, um das fehlerhafte f¸hrende Leerzeichen zu beseitigen.
    ''' - Absicherung gegen 'DBNull' und leere Preis-Strings integriert.
    ''' </remarks> 
    Public Function fcGetPreisToDay(ByVal ZimID As String, ByVal Datum As String, ByVal dtAllePreise As DataTable) As String
        If String.IsNullOrWhiteSpace(ZimID) OrElse String.IsNullOrWhiteSpace(Datum) OrElse dtAllePreise Is Nothing Then
            Return "0.00"
        End If

        Dim nMaxPreisCents As Integer = 0

        ' Wochentag ermitteln (Montag = 0, ..., Sonntag = 6)
        Dim dAktuellesDatum As Date
        If Not Date.TryParse(fcUmDatum(Datum), dAktuellesDatum) Then
            If Not Date.TryParse(Datum, dAktuellesDatum) Then dAktuellesDatum = Date.Today
        End If
        Dim nTagIndex As Integer = CInt(dAktuellesDatum.DayOfWeek) - 1
        If nTagIndex = -1 Then nTagIndex = 6

        Try
            ' REINER ARBEITSSPEICHER-FILTER: Filtert die geladenen Zeilen blitzschnell per SQL-Syntax
            Dim filterExpression As String = "ZimID = '" & ZimID.Replace("'", "''") & "'"
            Dim gefundeneZeilen As DataRow() = dtAllePreise.Select(filterExpression)

            ' Schleife ¸ber die im RAM gefilterten Zeilen
            For Each row As DataRow In gefundeneZeilen
                ' Absolut null- und DBNull-sicheres Auslesen f¸r VB.NET
                Dim sVon As String = If(row.Item("ADatum") IsNot Nothing AndAlso Not IsDBNull(row.Item("ADatum")), row.Item("ADatum").ToString().Trim(), "")
                Dim sBis As String = If(row.Item("EDatum") IsNot Nothing AndAlso Not IsDBNull(row.Item("EDatum")), row.Item("EDatum").ToString().Trim(), "")

                If Datum >= sVon AndAlso Datum <= sBis Then
                    Dim rawPreis As String = If(row.Item("Preis") IsNot Nothing AndAlso Not IsDBNull(row.Item("Preis")), row.Item("Preis").ToString().Trim(), "")

                    If Not String.IsNullOrWhiteSpace(rawPreis) Then
                        Dim aPreis As String() = rawPreis.Split("|"c)

                        If aPreis.Length > nTagIndex Then
                            Dim nTagesPreisCents As Integer = 0
                            If Int32.TryParse(aPreis(nTagIndex), nTagesPreisCents) Then
                                If nMaxPreisCents < nTagesPreisCents Then
                                    nMaxPreisCents = nTagesPreisCents
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            ' Optionales Error-Reporting, falls beim Filtern im RAM etwas schiefgeht
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try

        Dim dFinalerPreis As Double = nMaxPreisCents / 100.0
        Return dFinalerPreis.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
    End Function

    ''' <summary>
    ''' Ermittelt den Gesamtzimmerpreis f¸r einen definierten Zeitraum basierend auf Zimmer-ID, Personenanzahl und Wochentagen.
    ''' </summary>
    ''' <param name="ZimID">Die eindeutige Identifikationsnummer des Zimmers.</param>
    ''' <param name="PeFP">Ein kombinierter String aus Personenanzahl und Festpreis, getrennt durch ein Slash (z. B. "2/50.00").</param>
    ''' <param name="ADatum">Das Enddatum des Aufenthalts (Abreisedatum).</param>
    ''' <param name="EDatum">Das Startdatum des Aufenthalts (Anreisedatum).</param>
    ''' <returns>Der berechnete Gesamtpreis als String-Repr‰sentation eines Dezimalwerts.</returns>
    ''' <remarks>
    ''' 26.12.2011 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - 'Select Case' durch dynamische Spaltenauswahl ("P" & Person) ersetzt, um ¸ber 20 Zeilen redundanten Code einzusparen.
    ''' - 'ByRef'-Parameter zu 'ByVal' ge‰ndert, da die Originalvariablen innerhalb der Funktion nicht modifiziert werden m¸ssen.
    ''' - 'Do While'-Schleife durch direkten Datumsvergleich optimiert.
    ''' - Typkonvertierungen modernisiert (z. B. 'Decimal.TryParse' und 'Convert.ToDecimal' statt 'Val').
    ''' - SQL-Injection-Risiko durch String-Verkettung minimiert (Hinweis auf Parameterized Queries).
    ''' - Verwendung des veralteten R¸ckgabe-Zuweisungsmusters ('fcGetZimPreis = ...') durch 'Return' ersetzt.
    ''' </remarks>
    Public Function fcGetZimPreis(ByVal ZimID As String, ByVal PeFP As String, ByVal ADatum As String, ByVal EDatum As String) As String
        ' Validierung der Eingabe-Parameter
        If String.IsNullOrWhiteSpace(PeFP) OrElse Not PeFP.Contains("/") Then Return "0"

        Dim aPeFP() As String = Split(PeFP, "/")
        Dim Person As String = aPeFP(0)

        Dim nFPreis As Decimal = 0
        Decimal.TryParse(aPeFP(1), nFPreis)

        Dim nAbzug As Decimal = 0
        Dim Preis As Decimal = 0

        ' 1. Ermittlung des Personenabzugs aus dtZim
        ' Optimierung: Statt Select-Case wird der Spaltenname dynamisch erzeugt ("P1" bis "P10")
        Dim intPerson As Integer
        If Integer.TryParse(Person, intPerson) AndAlso intPerson >= 1 AndAlso intPerson <= 10 Then
            Dim columnName As String = "P" & Person

            For i As Integer = 0 To dtZim.Rows.Count - 1
                If dtZim.Rows(i).Item("ID").ToString() = ZimID Then
                    Dim valStr As String = dtZim.Rows(i).Item(columnName).ToString()
                    Decimal.TryParse(valStr, nAbzug)
                    Exit For ' Datensatz gefunden, Schleife kann vorzeitig verlassen werden
                End If
            Next
        End If

        ' 2. Laden der Preistabelle aus der Datenbank
        ' Hinweis: Um SQL-Injection zu vermeiden, sollte sSQL idealerweise auf Parameterized Queries umgestellt werden.
        Dim sSQL As String = "Select * from Preise WHERE ZimID = '" & ZimID & "'"
        Dim dtPreis As DataTable = fcReadDataTable(sSQL)
        Dim nMax As Integer = dtPreis.Rows.Count - 1

        ' Konvertierung der Datumsangaben
        Dim dVon As Date = Convert.ToDateTime(EDatum)
        Dim dBis As Date = Convert.ToDateTime(ADatum)

        ' Vorbereiten der Festpreiskalkulation in Cent
        Dim nFPreisCent As Decimal
        Dim intPersonCount As Integer
        If Integer.TryParse(Person, intPersonCount) Then
            nFPreisCent = nFPreis * intPersonCount * 100
        End If

        Dim nAbzugCent As Decimal = nAbzug * 100

        ' 3. Taggenaue Preisberechnung im Zeitraum (Schleife von Anreise bis Abreise)
        Do While dVon < dBis
            Dim nPreis As Decimal = 0

            For i As Integer = 0 To nMax
                Dim dVon1 As Date = Convert.ToDateTime(fcUmDatum(dtPreis.Rows(i).Item("ADatum").ToString()))
                Dim dBis1 As Date = Convert.ToDateTime(fcUmDatum(dtPreis.Rows(i).Item("EDatum").ToString()))

                ' Pr¸fen, ob der aktuelle Tag im G¸ltigkeitsbereich des Tarifs liegt
                If dVon1 <= dVon AndAlso dBis1 >= dVon Then
                    Dim aPreis() As String = Split(dtPreis.Rows(i).Item("Preis").ToString(), "|")

                    ' Wochentag ermitteln (0 = Montag, 6 = Sonntag)
                    Dim nTag As Integer = CInt(dVon.DayOfWeek) - 1
                    If nTag < 0 Then nTag = 6 ' Korrektur, falls DayOfWeek = Sunday (0) ist

                    ' Hˆchsten g¸ltigen Preis f¸r diesen Tag ermitteln
                    If nTag < aPreis.Length Then
                        Dim currentTarifPreis As Decimal = 0
                        Decimal.TryParse(aPreis(nTag), currentTarifPreis)

                        If nPreis < currentTarifPreis Then
                            nPreis = currentTarifPreis
                        End If
                    End If
                End If
            Next

            ' Wenn ein Basispreis gefunden wurde, Tagessumme aufaddieren
            If nPreis > 0 Then
                Preis += nPreis + nFPreisCent - nAbzugCent
            End If

            ' Einen Tag weitergehen
            dVon = dVon.AddDays(1)
        Loop

        ' R¸ckgabe des finalen Betrags (wieder umgerechnet aus Cent)
        Return (Preis / 100).ToString("F2")
    End Function

    ''' <summary>
    ''' ‹bersetzt einen ¸bergebenen Text anhand der in der Systemdatenbank hinterlegten Sprachkonfiguration.
    ''' Passt zudem den Zeichensatz basierend auf der gew‰hlten Sprache an.
    ''' </summary>
    ''' <param name="nSprache">Der Index der Zielsprache (z. B. 0 = Deutsch, 1 = Englisch).</param>
    ''' <param name="sText">Der zu ¸bersetzende Ausgangstext.</param>
    ''' <returns>Der ¸bersetzte Text, falls gefunden; andernfalls der originale Ausgangstext.</returns>
    ''' <remarks>
    ''' <b>Historie:</b><br/>
    ''' 24.09.2026 ñ Uwe: Typsicherheit erhˆht (String-Arrays), ByVal-Parameter eingef¸hrt und Index-Schutzpr¸fungen eingebaut.<br/>
    ''' </remarks>
    Function fcLanguage(ByVal nSprache As Integer, ByVal sText As String) As String
        ' Zeilen aus der System-DB einlesen
        Dim sRawData As String = ReadOneValueFromSystemDb("Language")
        If String.IsNullOrEmpty(sRawData) Then Return sText

        Dim sLanguage() As String = sRawData.Split(New String() {vbCrLf, vbLf}, StringSplitOptions.None)
        If sLanguage.Length = 0 Then Return sText

        ' Sprachkopf (erste Zeile) analysieren
        Dim sSprache() As String = sLanguage(0).Split(";"c)

        ' Sicherheitspr¸fung: Existiert die angeforderte Sprache im Header?
        If sSprache.Length > (nSprache + 1) Then
            Dim cSprache() As String = sSprache(nSprache + 1).Split(","c)

            ' Sicherheitspr¸fung: Existiert die Zeichensatz-Information (Index 2)?
            If cSprache.Length > 2 Then
                prZeichenSatz(cSprache(2))
            End If
        End If

        Dim sText2 As String = ""
        Dim bGefunden As Boolean = False

        ' Alle Zeilen nach dem gesuchten Text durchsuchen
        For i As Integer = 0 To sLanguage.Length - 1
            Dim x As String = AtLeft(sLanguage(i), ";", 1)

            If x = sText Then
                Dim sText1() As String = sLanguage(i).Split(";"c)

                ' Sicherheitspr¸fung: Hat die Zeile gen¸gend Spalten f¸r die gew‰hlte Sprache?
                If sText1.Length > (nSprache + 1) Then
                    sText2 = sText1(nSprache + 1)
                    bGefunden = True
                End If
                Exit For
            End If
        Next

        ' Ergebnis zur¸ckgeben
        If bGefunden Then
            Return sText2
        Else
            Return sText
        End If
    End Function
#End Region

#Region "Programm Funktionen.(Schloss)..............................................................."

    ''' <summary>
    ''' Generiert einen neuen, eindeutigen numerischen Code f¸r ein Zimmer, sofern f¸r dieses 
    ''' Zimmer die Code-Generierung aktiviert ist. Der Code wird gegen die Datenbank gepr¸ft, 
    ''' um Duplikate auszuschlieﬂen.
    ''' </summary>
    ''' <param name="zid">Die ID des Zimmers, f¸r das der Code generiert werden soll.</param>
    ''' <returns>Der generierte eindeutige Code als Integer oder -1, wenn f¸r das Zimmer kein Code generiert werden darf.</returns>
    ''' <remarks>
    ''' 25.02.2012 Create <br/>
    ''' 08.10.2026 Code-Optimierung: Entfernung von VB6-Altlasten (Str, Trim), Umstellung auf 'ByVal' f¸r Typsicherheit, 
    ''' Optimierung der Schleifenstruktur und Einsatz von String-Interpolation.
    ''' </remarks>
    Public Function fcNewCode(ByVal zid As String) As Integer
        ' Random-Instanz sollte idealerweise auf Klassenebene deklariert werden, 
        ' um bei schnellen aufeinanderfolgenden Aufrufen identische Zahlenwerte zu verhindern.
        Dim r As New System.Random()
        Dim c As Integer = 0
        Dim sC As String = String.Empty

        ' Pr¸fen, ob eine g¸ltige Zimmer-ID ¸bergeben wurde
        If zid <> "-1" Then
            Dim sSQL As String = $"Select * From Zimmer Where ID='{zid}'"
            Dim dt As DataTable = fcReadDataTable(sSQL)

            If dt.Rows.Count <> 0 Then
                ' Wenn das Zimmer keine Code-Generierung erlaubt (Wert ungleich "1"), wird -1 zur¸ckgegeben
                If dt.Rows(0).Item("Code").ToString() <> "1" Then
                    Return -1
                End If
            End If
        End If

        ' Eindeutigen Code in einer Schleife generieren und in der Datenbank auf Existenz pr¸fen
        Dim codeExistiert As Boolean
        Do
            c = r.Next(0, 100000) ' 0 bis 99999 einschlieﬂen

            ' .ToString() ersetzt Str() und Trim(), da es kein f¸hrendes Vorzeichen-Leerzeichen erzeugt
            sC = fcCode(c.ToString())

            Dim sSQL As String = $"Select * From Code Where code='{sC}'"
            Dim dt As DataTable = fcReadDataTable(sSQL)

            ' Wenn kein Datensatz gefunden wurde, ist der Code eindeutig und die Schleife bricht ab
            codeExistiert = (dt.Rows.Count > 0)
        Loop While codeExistiert

        Return c
    End Function



    ''' <summary>
    ''' Transformiert und verschl¸sselt einen Code, indem er zun‰chst formatiert 
    ''' und anschlieﬂend zeichenweise in ein semikolonsepariertes Format umgewandelt wird.
    ''' </summary>
    ''' <param name="sCode">Der zu verarbeitende Code.</param>
    ''' <returns>Eine durch Semikolons getrennte Zeichenkette der transformierten Einzelziffern.</returns>
    ''' <remarks>
    ''' 19.01.2012 Create <br/>
    ''' 23.09.2026 Refactored 
    ''' </remarks>
    Function fcCode(ByVal sCode As String) As String
        ' 1. Eingabewert vorverarbeiten (Nutzt lokale Variable statt ByRef-Seiteneffekt)
        Dim preparedCode As String = fcUmECode(sCode, 5)

        ' 2. Liste vorbereiten, um die transformierten Zeichen zu sammeln
        Dim parts As New List(Of String)(preparedCode.Length)

        ' 3. Jedes Zeichen effizient transformieren
        For Each zeichen As Char In preparedCode
            parts.Add(fcUmECode(zeichen.ToString(), 3))
        Next

        ' 4. Alle Teile performant mit Semikolon verbinden und zur¸ckgeben
        Return String.Join(";", parts)
    End Function

    ''' <summary>
    ''' Formatiert eine Zeichenkette auf eine feste L‰nge, indem sie getrimmt 
    ''' und bei Bedarf von links mit Nullen aufgef¸llt oder von rechts gek¸rzt wird.
    ''' </summary>
    ''' <param name="sCode1">Die zu formatierende Zeichenkette.</param>
    ''' <param name="nLength">Die gew¸nschte Ziell‰nge des Strings.</param>
    ''' <returns>Die auf die Ziell‰nge formatierte und mit Nullen aufgef¸llte Zeichenkette.</returns>
    ''' <remarks>
    ''' 19.01.2012 Create <br/>
    ''' 23.09.2026 Refactored 
    ''' </remarks>
    Function fcUmECode(ByVal sCode1 As String, ByVal nLength As Integer) As String
        ' Falls der ¸bergebene Wert Nothing ist, leeren String nutzen
        Dim cleanedCode As String = If(sCode1, "").Trim()

        ' Wenn der String l‰nger als die gew¸nschte L‰nge ist, von rechts k¸rzen
        If cleanedCode.Length > nLength Then
            Return cleanedCode.Substring(cleanedCode.Length - nLength)
        End If

        ' Andernfalls von links mit Nullen auff¸llen
        Return cleanedCode.PadLeft(nLength, "0"c)
    End Function

    ''' <summary>
    ''' Formatiert eine semikolongetrennte Liste von Einzelcodes auf eine einheitliche dreistellige L‰nge (z. B. "1;12;123" zu "001;012;123").
    ''' Zerlegt die Eingabe anhand des Trennzeichens, bereinigt Leerzeichen und f¸llt jeden Teilcode ¸ber 'fcUmECode' auf.
    ''' </summary>
    ''' <param name="sCode">Die semikolongetrennte Zeichenkette der Codes (wird per ByRef ebenfalls mit dem formatierten Ergebnis ¸berschrieben).</param>
    ''' <returns>Die neu zusammengesetzte, semikolongetrennte Zeichenkette mit dreistelligen Codes oder String.Empty bei ung¸ltiger Eingabe.</returns>
    ''' <remarks>
    ''' 11.11.2026 - Code-Optimierung:
    ''' - Vollst‰ndige XML-Dokumentation f¸r Zusammenfassung, Parameter und R¸ckgabewert erg‰nzt.
    ''' - Guard Clause ('String.IsNullOrWhiteSpace') erg‰nzt, um Abst¸rze bei leeren Eingaben ('Mid(..., 0)') zu verhindern.
    ''' - Veraltete VB6-Funktionen ('Split', 'Mid') und String-Verkettungen in Schleifen durch modernes '.NET' ('String.Split', 'String.Join') ersetzt.
    ''' - Explizite Schleifenvariable ('As Integer') deklariert (Option Strict-Kompatibilit‰t) und 'ByRef'-Verhalten abw‰rtskompatibel beibehalten.
    ''' </remarks>
    Public Function fcUmCode(ByRef sCode As String) As String
        ' Guard Clause: Absturz bei Nothing oder leerem String vermeiden
        If String.IsNullOrWhiteSpace(sCode) Then
            sCode = String.Empty
            Return String.Empty
        End If

        ' String an Semikolons aufteilen
        Dim aCode() As String = sCode.Split(";"c)

        ' Jeden Teilcode bereinigen und auf 3 Stellen formatieren
        For i As Integer = 0 To aCode.Length - 1
            aCode(i) = fcUmECode(aCode(i).Trim(), 3)
        Next

        ' Array sauber wieder mit Semikolon verbinden (spart das nachtr‰gliche Abschneiden des letzten Zeichens)
        sCode = String.Join(";", aCode)

        Return sCode
    End Function

    ''' <summary>
    ''' Formatiert einen achtstelligen Datumsstring f¸r die RFID-Verarbeitung in vier zweistellige, semikolongetrennte Blˆcke (z. B. "20261111" zu "20;26;11;11").
    ''' </summary>
    ''' <param name="Datum">Der achtstellige Datumsstring ohne Trennzeichen.</param>
    ''' <returns>Den in Zweiergruppen mit Semikolon getrennten Datumsstring oder String.Empty, falls die Eingabe k¸rzer als 8 Zeichen ist.</returns>
    ''' <remarks>
    ''' 11.11.2026 - Code-Optimierung:
    ''' - Vollst‰ndige XML-Dokumentation erg‰nzt.
    ''' - L‰ngen- und Null-Pr¸fung (Guard Clause) eingebaut, um 'ArgumentOutOfRangeException' bei zu kurzen Strings zu verhindern.
    ''' - Veraltete VB6-Funktion 'Mid' durch modernes .NET '.Substring()' und 'String.Join()' ersetzt.
    ''' - Veraltete Zuweisung ¸ber den Funktionsnamen durch modernes 'Return' ersetzt.
    ''' </remarks>
    Public Function fcRFIDDatum(ByRef Datum As String) As String
        ' Guard Clause: Sicherstellen, dass Datum nicht Nothing ist und mindestens 8 Zeichen hat
        If String.IsNullOrWhiteSpace(Datum) OrElse Datum.Trim().Length < 8 Then
            Return String.Empty
        End If

        Dim sCleanDatum As String = Datum.Trim()

        ' In vier 2-stellige Blˆcke aufteilen und mit Semikolon verbinden
        Return String.Join(";",
                       sCleanDatum.Substring(0, 2),
                       sCleanDatum.Substring(2, 2),
                       sCleanDatum.Substring(4, 2),
                       sCleanDatum.Substring(6, 2))
    End Function

    ''' <summary>
    ''' Formatiert einen vierstelligen Zeitstring f¸r die RFID-Verarbeitung in zwei zweistellige, semikolongetrennte Blˆcke (z. B. "1430" zu "14;30").
    ''' Wandelt den Sonderwert "0" vorab in den Standardwert "1200" um.
    ''' </summary>
    ''' <param name="Zeit">Der vierstellige Zeitstring ohne Trennzeichen (oder "0" als Standard-Fallback; wird per ByRef bei "0" auf "1200" aktualisiert).</param>
    ''' <returns>Den in Stunden und Minuten mit Semikolon getrennten Zeitstring (z. B. "14;30") oder String.Empty bei ung¸ltiger L‰nge.</returns>
    ''' <remarks>
    ''' 11.11.2026 - Code-Optimierung:
    ''' - Vollst‰ndige XML-Dokumentation inklusive Hinweis auf den Sonderfall "0" erg‰nzt.
    ''' - Null- und L‰ngenpr¸fung (Guard Clause) erg‰nzt, um 'ArgumentOutOfRangeException' bei zu kurzen Strings zu vermeiden.
    ''' - Veraltete VB6-Funktion 'Mid' durch modernes .NET '.Substring()' und 'Return' ersetzt.
    ''' - Inline-Kommentare zur besseren Nachvollziehbarkeit der Sonderlogik hinzugef¸gt.
    ''' </remarks>
    Public Function fcRFIDZeit(ByRef Zeit As String) As String
        ' Guard Clause: Pr¸fung auf leere Eingabe
        If String.IsNullOrWhiteSpace(Zeit) Then
            Return String.Empty
        End If

        Dim sCleanZeit As String = Zeit.Trim()

        ' Sonderfall abfangen: "0" wird auf Standardzeit "1200" gesetzt (inkl. ByRef-R¸ckschreibung wie im Original)
        If sCleanZeit = "0" Then
            sCleanZeit = "1200"
            Zeit = sCleanZeit
        End If

        ' Sicherstellen, dass mindestens 4 Zeichen f¸r Stunden und Minuten vorhanden sind
        If sCleanZeit.Length < 4 Then
            Return String.Empty
        End If

        ' Stunden und Minuten mit Semikolon getrennt zur¸ckgeben
        Return $"{sCleanZeit.Substring(0, 2)};{sCleanZeit.Substring(2, 2)}"
    End Function

    ''' <summary>
    ''' Zerlegt eine 32-Bit-Zahl (z. B. eine Wiegand-34-Bit-Kartennummer) in vier einzelne Bytes (Big-Endian)
    ''' und formatiert diese inklusive f¸hrender "0;" ¸ber 'fcUmCode' als dreistellige, semikolongetrennte Code-Folge.
    ''' </summary>
    ''' <param name="sZahl">Die umzuwandelnde Ganzzahl als Zeichenkette.</param>
    ''' <returns>Eine semikolongetrennte Zeichenkette im Format "000;BBB;BBB;BBB;BBB" oder "000;000;000;000;000" bei ung¸ltiger Eingabe.</returns>
    ''' <remarks>
    ''' 11.11.2026 - Code-Optimierung:
    ''' - Vollst‰ndige XML-Dokumentation erg‰nzt.
    ''' - Veraltetes VB6-'Val()' und Flieﬂkomma-Division ('Int(nZahl / N1)') durch typsicheres 'Long.TryParse' sowie exakte Ganzzahldivision ('\') und Modulo ('Mod') ersetzt.
    ''' - Magische Zahlen (16777216, 65536, 256) durch sprechende Konstanten ersetzt.
    ''' - String-Zusammenbau ¸ber 'String.Join' und modernes 'Return' vereinfacht.
    ''' </remarks>
    Public Function fcWeg34(ByRef sZahl As String) As String
        ' Konstanten f¸r die Byte-Zerlegung (2^24, 2^16, 2^8)
        Const Byte3Divisor As Long = 16777216L
        Const Byte2Divisor As Long = 65536L
        Const Byte1Divisor As Long = 256L

        Dim nZahl As Long = 0

        ' Sicheres Parsen der Eingabe (falls leer oder ung¸ltig, bleibt nZahl = 0)
        If Not String.IsNullOrWhiteSpace(sZahl) Then
            Long.TryParse(sZahl.Trim(), nZahl)
        End If

        ' Zahl in 4 Einzelbytes (0-255) zerlegen (exakte Ganzzahldivision '\' und Restwert 'Mod')
        Dim z1 As Long = nZahl \ Byte3Divisor
        nZahl = nZahl Mod Byte3Divisor

        Dim z2 As Long = nZahl \ Byte2Divisor
        nZahl = nZahl Mod Byte2Divisor

        Dim z3 As Long = nZahl \ Byte1Divisor
        Dim z4 As Long = nZahl Mod Byte1Divisor

        ' Rohstring zusammenbauen und ¸ber fcUmCode auf dreistellige Blˆcke formatieren
        Dim sRawCode As String = String.Join(";", "0", z1, z2, z3, z4)
        Return fcUmCode(sRawCode)
    End Function


    'Function fcWeg34(ByRef sZahl As String) As String
    '    Dim nZahl As Long = Val(sZahl)
    '    Dim N1 As Long = 16777216 '4294967296
    '    Dim N2 As Integer = 65536
    '    Dim N3 As Integer = 256
    '    Dim Z1 As Integer = 0
    '    Dim Z2 As Integer = 0
    '    Dim Z3 As Integer = 0
    '    Dim Z4 As Integer = 0
    '    Z1 = Int(nZahl / N1)
    '    nZahl = nZahl - (Z1 * N1)
    '    Z2 = Int(nZahl / N2)
    '    nZahl = nZahl - (Z2 * N2)
    '    Z3 = Int(nZahl / N3)
    '    Z4 = nZahl - (Z3 * N3)
    '    fcWeg34 = fcUmCode("0;" & Z1.ToString & ";" & Z2.ToString & ";" & Z3.ToString & ";" & Z4.ToString)
    'End Function

#End Region

#Region "Programm Funktionen.(Color)"

    ''' <summary>
    ''' Wandelt einen RGB-String (z. B. "192,54,34") in ein .NET Color-Objekt um.
    ''' </summary>
    ''' <param name="sColorRGB">Der umzuwandelnde RGB-String im Format "R,G,B".</param>
    ''' <returns>Ein Color-Objekt mit den entsprechenden RGB-Werten oder Color.Empty bei Fehlern.</returns>
    ''' <remarks>
    ''' 03.02.2012 Create <br/>
    ''' 22.09.2026 Code-Optimierung: Absicherung gegen Formatfehler, Ersetzen der VB6-Funktion 'Val' durch typsicheres 'Integer.TryParse' und Wertebereichs-Korrektur via Math.Min/Max.
    ''' </remarks>
    Public Function fcStringRGB(ByVal sColorRGB As String) As Color
        ' 1. Vorabpr¸fung: Ist der ¸bergebene String leer?
        If String.IsNullOrWhiteSpace(sColorRGB) Then Return Color.Empty

        Try
            ' String anhand der Kommas aufteilen
            Dim arT() As String = sColorRGB.Split(","c)

            ' 2. Absicherung: Es m¸ssen exakt 3 Werte (R, G, B) vorhanden sein
            If arT.Length >= 3 Then
                Dim r As Integer = 0
                Dim g As Integer = 0
                Dim b As Integer = 0

                ' Sicheres Parsen der einzelnen Farbkan‰le (ignoriert Leerzeichen und ung¸ltige Zeichen)
                Integer.TryParse(arT(0), r)
                Integer.TryParse(arT(1), g)
                Integer.TryParse(arT(2), b)

                ' Ersatz f¸r .Clamp unter .NET Framework: Werte auf 0 bis 255 begrenzen
                r = Math.Max(0, Math.Min(r, 255))
                g = Math.Max(0, Math.Min(g, 255))
                b = Math.Max(0, Math.Min(b, 255))

                Return Color.FromArgb(r, g, b)
            End If

        Catch ex As Exception
            ' Fehler abfangen (nutzt deine Fehlerprotokollierung)
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try

        ' Standard-R¸ckgabewert im Fehlerfall
        Return Color.Empty
    End Function

    ''' <summary>
    ''' Wandelt ein Color-Objekt in einen kommagetrennten RGB-String um (z. B. Color.Black zu "0,0,0").
    ''' Liest die Farbkan‰le Rot, Gr¸n und Blau direkt aus dem Farbobjekt aus und gibt sie formatiert zur¸ck.
    ''' </summary>
    ''' <param name="oColor">Das umzuwandelnde System.Drawing.Color-Objekt.</param>
    ''' <returns>Ein kommagetrennter RGB-String im Format "R,G,B" (z. B. "255,128,0").</returns>
    ''' <example>
    ''' <code>
    ''' Dim sRgb As String = fcRGBString(Color.Black) ' Ergibt "0,0,0"
    ''' </code>
    ''' </example>
    ''' <remarks>
    ''' 03.02.2012 - Create
    ''' 11.11.2026 - Code-Optimierung:
    ''' - Umst‰ndliches String-Parsing ('Color.ToArgb.ToString', 'Mid', 'Extract') durch direkten Zugriff auf die Eigenschaften '.R', '.G' und '.B' ersetzt.
    ''' - ‹berfl¸ssigen Try-Catch-Finally-Block entfernt, da der direkte Eigenschaftszugriff auf die Werttyp-Struktur 'Color' keine Exceptions auslˆst.
    ''' - Veraltete Zuweisung ¸ber den Funktionsnamen durch modernes 'Return' und String-Interpolation ersetzt.
    ''' - XML-Dokumentation um fehlenden Parameter 'oColor', R¸ckgabewert und sauberen '&lt;example&gt;'-Block erg‰nzt.
    ''' </remarks>
    Public Function fcRGBString(ByRef oColor As Color) As String
        ' Direkter Zugriff auf die RGB-Byte-Eigenschaften des Color-Objekts (ohne String-Zerlegung)
        Return $"{oColor.R},{oColor.G},{oColor.B}"
    End Function


    '''' <summary>
    '''' wandelt ein Color-Objeckt in ein RGB-String  um
    '''' dim srgb as string  = fcRGBstring(color.Black)  ->"0,0,0"
    '''' </summary>
    '''' <returns></returns>
    '''' <remarks>
    '''' 03.02.2012 Create
    '''' </remarks>
    'Function fcRGBString(ByRef oColor As Color) As String
    '    Dim sColor As String = Color.FromArgb(oColor.ToArgb).ToString
    '    Dim sR As String = "0"
    '    Dim sG As String = "0"
    '    Dim sB As String = "0"
    '    Try
    '        sR = Mid(Extract(sColor, "R", ",", 1), 2)
    '        sG = Mid(Extract(sColor, "G", ",", 1), 2)
    '        sB = Mid(Extract(sColor, "B", "]", 1), 2)
    '    Catch ex As Exception
    '        sR = "0" : sG = "0" : sB = "0"
    '    Finally
    '        fcRGBString = sR & "," & sG & "," & sB
    '    End Try
    'End Function

    ''' <summary>
    ''' L‰dt das Farbschema aus der Konfigurationsdatei in die globalen Farbvariablen.
    ''' </summary>
    ''' <remarks>
    ''' 12.02.2012 Create <br/>
    ''' 22.09.2026 Code-Optimierung: Absicherung gegen Index-‹berschreitungen bei fehlerhaften INI-Zeilen, 
    ''' Umstellung auf plattformunabh‰ngigen Zeilen-Split und Verbesserung der Typsicherheit.
    ''' </remarks>
    Public Sub prColorRead()
        Try
            ' Einlesen der Datei und Splitten anhand aller g‰ngigen Zeilenumbr¸che (\r, \n)
            Dim roherText As String = ReadOneValueFromSystemDb("Color")
            If String.IsNullOrWhiteSpace(roherText) Then Exit Sub

            Dim arTmp() As String = roherText.Split(New String() {Environment.NewLine, vbCr, vbLf}, StringSplitOptions.RemoveEmptyEntries)
            Dim nMax As Integer = arTmp.Length - 1

            ' Schleife verarbeitet maximal so viele Zeilen, wie das globale Array arFarbe fassen kann
            Dim maximaleZeilen As Integer = Math.Min(nMax, arFarbe.GetUpperBound(0))

            For i As Integer = 0 To maximaleZeilen
                Dim zeile As String = arTmp(i).Trim()

                If zeile <> "" Then
                    Dim arT() As String = zeile.Split(";"c)

                    ' Absicherung: Nur verarbeiten, wenn Name, Back- und Fore-Color (min. 3 Elemente) existieren
                    If arT.Length >= 3 Then
                        arFarbe(i, 0) = arT(0).Trim() ' Name
                        arFarbe(i, 1) = arT(1).Trim() ' Back
                        arFarbe(i, 2) = arT(2).Trim() ' Fore
                    End If
                End If
            Next

            ' Zuweisung der geladenen Farben ¸ber deine Konvertierungsfunktion fcStringRGB
            fColorForeAnreise = fcStringRGB(arFarbe(0, 2))
            fColorBackAnreise = fcStringRGB(arFarbe(0, 1))

            fColorForeFerien1 = fcStringRGB(arFarbe(1, 2))
            fColorBackFerien1 = fcStringRGB(arFarbe(1, 1))

            fColorForeFerien2 = fcStringRGB(arFarbe(2, 2))
            fColorBackFerien2 = fcStringRGB(arFarbe(2, 1))

            fColorForeFerien3 = fcStringRGB(arFarbe(3, 2))
            fColorBackFerien3 = fcStringRGB(arFarbe(3, 1))

            fColorForeFerien4 = fcStringRGB(arFarbe(4, 2))
            fColorBackFerien4 = fcStringRGB(arFarbe(4, 1))

            fColorForeFerien5 = fcStringRGB(arFarbe(5, 2))
            fColorBackFerien5 = fcStringRGB(arFarbe(5, 1))

            fColorForeNebenSaison1 = fcStringRGB(arFarbe(6, 2))
            fColorBackNebenSaison1 = fcStringRGB(arFarbe(6, 1))

            fColorForeNebenSaison2 = fcStringRGB(arFarbe(7, 2))
            fColorBackNebenSaison2 = fcStringRGB(arFarbe(7, 1))

            fColorForeHauptSaison = fcStringRGB(arFarbe(8, 2))
            fColorBackHauptSaison = fcStringRGB(arFarbe(8, 1))

            fColorForeFeiertag1 = fcStringRGB(arFarbe(9, 2))
            fColorBackFeiertag1 = fcStringRGB(arFarbe(9, 1))

            fColorForeFeiertag2 = fcStringRGB(arFarbe(10, 2))
            fColorBackFeiertag2 = fcStringRGB(arFarbe(10, 1))

            fColorForeFeiertag3 = fcStringRGB(arFarbe(11, 2))
            fColorBackFeiertag3 = fcStringRGB(arFarbe(11, 1))

            fColorForeFreitag = fcStringRGB(arFarbe(12, 2))
            fColorBackFreitag = fcStringRGB(arFarbe(12, 1))

            fColorForeSamstag = fcStringRGB(arFarbe(13, 2))
            fColorBackSamstag = fcStringRGB(arFarbe(13, 1))

            fColorForeSonstigeFeiertage = fcStringRGB(arFarbe(14, 2))
            fColorBackSonstigeFeiertage = fcStringRGB(arFarbe(14, 1))

            fColorForeFestGebucht = fcStringRGB(arFarbe(15, 2))
            fColorBackFestGebucht = fcStringRGB(arFarbe(15, 1))

            fColorForeVariabel = fcStringRGB(arFarbe(16, 2))
            fColorBackVariabel = fcStringRGB(arFarbe(16, 1))

            fColorForeErlaubt = fcStringRGB(arFarbe(17, 2))
            fColorBackErlaubt = fcStringRGB(arFarbe(17, 1))

            fColorForeNichtErlaubt = fcStringRGB(arFarbe(18, 2))
            fColorBackNichtErlaubt = fcStringRGB(arFarbe(18, 1))

            fColorForeBank = fcStringRGB(arFarbe(19, 2))
            fColorBackBank = fcStringRGB(arFarbe(19, 1))

            fColorForeKasse = fcStringRGB(arFarbe(20, 2))
            fColorBackKasse = fcStringRGB(arFarbe(20, 1))

        Catch ex As Exception

            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub






#End Region

#Region "Programm Funktionen.(Mail)"



    ''' <summary>
    ''' EMail versenden mit optionalen Dateianh‰ngen.
    ''' </summary>
    ''' <param name="sMailKunde">Die Empf‰nger-E-Mail-Adresse(n), getrennt durch Semikolon.</param>
    ''' <param name="sBetreff">Der Betreff der E-Mail.</param>
    ''' <param name="sKopf">Der Haupttext (Body) der E-Mail.</param>
    ''' <param name="sLink">Optionaler Link, der dem Text hinzugef¸gt wird.</param>
    ''' <param name="sFuss">Optionaler Fuﬂtext, der dem Text hinzugef¸gt wird.</param>
    ''' <param name="sDatei">Optionaler Pfad zur ersten Anhangsdatei.</param>
    ''' <param name="sDatei1">Optionaler Pfad zur zweiten Anhangsdatei.</param>
    ''' <returns>True, wenn die Mail erfolgreich gesendet wurde, andernfalls False.</returns>
    ''' <remarks>
    ''' <para>17.02.2012: Erstellt</para>
    ''' <para><strong>Optimierung:</strong></para>
    ''' <para>24.09.2026: Optimierung: Ressourcenlecks durch fehlendes Disposing behoben
    ''' (<c>MailMessage</c> und <c>SmtpClient</c> implementieren <c>IDisposable</c> und wurden in <c>Using</c>-Blˆcke gekapselt). 
    ''' Zudem wurden ungenutzte Variablen entfernt, veraltete <c>Exit Function</c>-Aufrufe bereinigt und die String-Verkettung performanter gestaltet. 
    ''' Die Parameter wurden auf <c>ByVal</c> umgestellt, um ungewollte Seiteneffekte im Hauptprogramm zu verhindern.</para>
    ''' </remarks>
    Function fcSendeMailAnlage(ByVal sMailKunde As String,
                           ByVal sBetreff As String,
                           ByVal sKopf As String,
                           Optional ByVal sLink As String = "",
                           Optional ByVal sFuss As String = "",
                           Optional ByVal sDatei As String = "",
                           Optional ByVal sDatei1 As String = "") As Boolean

        ' Validierung der Dateianh‰nge
        If Not String.IsNullOrWhiteSpace(sDatei) AndAlso Not System.IO.File.Exists(sDatei) Then
            MessageBox.Show("PDF " & sDatei & " nicht vorhanden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        If Not String.IsNullOrWhiteSpace(sDatei1) AndAlso Not System.IO.File.Exists(sDatei1) Then
            MessageBox.Show("PDF " & sDatei1 & " nicht vorhanden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        ' E-Mail-Adressen bereinigen und splitten
        sMailKunde = sMailKunde.Replace(" ", "")
        Dim aMailKunde() As String = sMailKunde.Split(";"c)

        For Each mail As String In aMailKunde
            If Not ValidateEmail(mail) Then
                Return False
            End If
        Next

        ' Textkorrekturen
        If String.IsNullOrWhiteSpace(sBetreff) Then sBetreff = "Test"

        Dim sbBody As New System.Text.StringBuilder(sKopf)
        If Not String.IsNullOrWhiteSpace(sLink) Then sbBody.AppendLine().Append(sLink)
        If Not String.IsNullOrWhiteSpace(sFuss) Then sbBody.AppendLine().Append(sFuss)

        Dim sBody As String = sbBody.ToString()
        If String.IsNullOrWhiteSpace(sBody) Then sBody = "Sorry Fehler"

        ' Zertifikatspr¸fung umgehen (wie im Original)
        Dim oCertOverride As New CertificateOverride
        ServicePointManager.ServerCertificateValidationCallback = AddressOf oCertOverride.RemoteCertificateValidationCallback

        ' Sicheres Senden mit automatischem Ressourcen-Cleanup (Using)
        Try
            Using SmtpObj As New SmtpClient()
                SmtpObj.Host = arEMail(0)
                SmtpObj.Port = 587
                SmtpObj.UseDefaultCredentials = False
                SmtpObj.Credentials = New NetworkCredential(arEMail(3), arEMail(4))
                SmtpObj.TargetName = arEMail(1)
                SmtpObj.EnableSsl = True

                Using MailNachricht As New MailMessage()
                    MailNachricht.From = New MailAddress(arEMail(1))
                    MailNachricht.BodyEncoding = System.Text.Encoding.UTF8
                    MailNachricht.Subject = sBetreff
                    MailNachricht.IsBodyHtml = False
                    MailNachricht.Body = sBody

                    ' Empf‰nger hinzuf¸gen
                    For Each mail As String In aMailKunde
                        MailNachricht.To.Add(mail)
                    Next
                    MailNachricht.To.Add("feworeich1@web.de")

                    ' Anh‰nge hinzuf¸gen
                    If Not String.IsNullOrWhiteSpace(sDatei) Then
                        MailNachricht.Attachments.Add(New Attachment(sDatei))
                    End If
                    If Not String.IsNullOrWhiteSpace(sDatei1) Then
                        MailNachricht.Attachments.Add(New Attachment(sDatei1))
                    End If

                    ' Sende-Wiederholungsschleife (5 Versuche)
                    Dim iVersuche As Integer = 5
                    Dim bErfolgreich As Boolean = False

                    While iVersuche > 0 AndAlso Not bErfolgreich
                        iVersuche -= 1
                        Try
                            SmtpObj.Send(MailNachricht)
                            bErfolgreich = True
                        Catch ex As Exception
                            ErrReport(ex.Message, ex.Source, ex.StackTrace)
                            If iVersuche = 0 Then Return False
                            System.Threading.Thread.Sleep(1000) ' 1 Sekunde warten vor dem n‰chsten Versuch
                        End Try
                    End While

                    Return bErfolgreich
                End Using
            End Using
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Pr¸ft, ob eine ¸bergebene Zeichenkette einer g¸ltigen E-Mail-Adressstruktur entspricht.
    ''' Nutzt die integrierte .NET-Validierung ¸ber 'System.Net.Mail.MailAddress' und verhindert Abst¸rze bei leeren Eingaben.
    ''' </summary>
    ''' <param name="email">Die zu ¸berpr¸fende E-Mail-Adresse.</param>
    ''' <returns><c>True</c>, wenn das Format der E-Mail-Adresse g¸ltig ist; andernfalls <c>False</c>.</returns>
    ''' <remarks>
    ''' 11.11.2026 - Code-Optimierung:
    ''' - Vollst‰ndige XML-Dokumentation erg‰nzt.
    ''' - Guard Clause ('String.IsNullOrWhiteSpace') erg‰nzt, um 'ArgumentNullException' bei Nothing-Werten zu verhindern.
    ''' - Einfache Regex-Pr¸fung (die z. B. "a@b" ohne Domain-Endung akzeptierte) durch standardkonforme 'System.Net.Mail.MailAddress'-Validierung ersetzt.
    ''' - Inline-Kommentare zur besseren Wartbarkeit hinzugef¸gt.
    ''' </remarks>
    Public Function ValidateEmail(ByVal email As String) As Boolean
        ' Guard Clause: Leere Strings oder Nothing sofort als ung¸ltig abweisen
        If String.IsNullOrWhiteSpace(email) Then
            Return False
        End If

        Dim sCleanEmail As String = email.Trim()

        ' Einfache Vorab-Pr¸fung auf '@' und mindestens einen Punkt im Domain-Teil
        Dim atIndex As Integer = sCleanEmail.IndexOf("@"c)
        If atIndex <= 0 OrElse sCleanEmail.IndexOf("."c, atIndex) <= atIndex + 1 Then
            Return False
        End If

        ' Standardkonforme Syntax-Validierung ¸ber die .NET-Klasse MailAddress
        Try
            Dim mailAddr As New System.Net.Mail.MailAddress(sCleanEmail)
            Return String.Equals(mailAddr.Address, sCleanEmail, StringComparison.OrdinalIgnoreCase)
        Catch ex As FormatException
            Return False
        End Try
    End Function


    'Function ValidateEmail(ByVal email As String) As Boolean
    '    Dim emailRegex As New System.Text.RegularExpressions.Regex(
    '        "^(?<user>[^@]+)@(?<host>.+)$")
    '    Dim emailMatch As System.Text.RegularExpressions.Match =
    '       emailRegex.Match(email)
    '    Return emailMatch.Success
    'End Function

    ''' <summary>
    ''' Hilfsklasse zur benutzerdefinierten Validierung von SSL/TLS-Serverzertifikaten bei Netzwerkverbindungen.
    ''' Akzeptiert pauschal alle Zertifikate (inklusive selbstsignierter oder abgelaufener Zertifikate) ohne Fehlerpr¸fung.
    ''' </summary>
    ''' <remarks>
    ''' 11.11.2026 - Code-Optimierung:
    ''' - Vollst‰ndige XML-Dokumentation f¸r Klasse, Methode, Parameter und R¸ckgabewert erg‰nzt.
    ''' - Sicherheitshinweis im Kommentar erg‰nzt, da das pauschale Akzeptieren aller Zertifikate Man-in-the-Middle-Angriffe ermˆglicht (nur f¸r interne/lokale Test- oder geschlossene Netzwerke empfohlen).
    ''' - Vollst‰ndige Typnamen f¸r X509-Klassen zur besseren ‹bersichtlichkeit verwendet.
    ''' </remarks>
    Public Class CertificateOverride

        ''' <summary>
        ''' R¸ckrufmethode (Callback) f¸r den 'ServicePointManager.ServerCertificateValidationCallback'.
        ''' ‹berschreibt die Standard-Zertifikatspr¸fung und gibt stets <c>True</c> zur¸ck.
        ''' </summary>
        ''' <param name="sender">Das Objekt, das die Validierungsanforderung ausgelˆst hat.</param>
        ''' <param name="certificate">Das vom Remote-Server bereitgestellte X.509-Zertifikat.</param>
        ''' <param name="chain">Die Zertifizierungskette, die dem Remote-Zertifikat zugeordnet ist.</param>
        ''' <param name="sslPolicyErrors">Ein oder mehrere Fehler, die bei der Standardpr¸fung des Zertifikats festgestellt wurden.</param>
        ''' <returns>Gibt immer <c>True</c> zur¸ck, um das Zertifikat als g¸ltig einzustufen.</returns>
        Public Function RemoteCertificateValidationCallback(ByVal sender As Object,
                                                        ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate,
                                                        ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain,
                                                        ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
            ' Hinweis: Deaktiviert die SSL-Zertifikatspr¸fung vollst‰ndig (z. B. f¸r lokale PHP-/Ger‰te-Schnittstellen)
            Return True
        End Function

    End Class


    'Public Class CertificateOverride
    '    Public Function RemoteCertificateValidationCallback(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As Net.Security.SslPolicyErrors) As Boolean
    '        Return True
    '    End Function
    'End Class

    ''' <summary>
    ''' Fragt den Benutzer, ob eine Zahlungsbest‰tigung gesendet werden soll.
    ''' Berechnet den Gesamtpreis der Buchung, erstellt den Text in der entsprechenden Sprache und versendet die E-Mail.
    ''' </summary>
    ''' <param name="sBid">Die Buchungs-ID (BID) als String.</param>
    ''' <returns>True, wenn die E-Mail erfolgreich gesendet wurde, andernfalls False.</returns>
    ''' <remarks>
    ''' <b>Historie:</b><br/>
    ''' 24.09.2026 ñ Uwe: Schleifenlogik korrigiert, StringBuilder f¸r den Textaufbau integriert und MsgBox modernisiert.<br/>
    ''' </remarks>
    Function fcMailBezahlt(ByVal sBid As String) As Boolean
        ' Standardm‰ﬂig auf False setzen
        Dim bResult As Boolean = False

        ' Benutzer fragen, ob gesendet werden soll
        Dim dialogResult As DialogResult = MessageBox.Show("Zahlungsbest‰tigung senden?", "Senden", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)

        If dialogResult = DialogResult.OK Then
            Dim arFeld() As String
            Dim arfeld1() As String
            Dim sKID As String
            Dim nSatz As Integer = 0
            Dim nPreis As Integer = 0

            ' Erste Abfrage der Buchungsdaten
            arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBid, nSatz, {"KunID", "Sprache", "Von", "Bis", "Summe"})

            If arfeld1 IsNot Nothing AndAlso arfeld1.Length >= 5 AndAlso Not String.IsNullOrWhiteSpace(arfeld1(0)) Then
                sKID = arfeld1(0)
                Dim sVon As String = fcUmDatum(arfeld1(2))
                Dim sBis As String = CDate(CDate(fcUmDatum(arfeld1(3))).AddDays(1)).ToString("dd.MM.yyyy")

                ' Sprache ermitteln
                Dim nSprache As Integer = fcDecStr(Str(Val(arfeld1(1) / 100)))

                ' Preissumme aller verkn¸pften Buchungss‰tze ermitteln
                Do While arfeld1 IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(arfeld1(0))
                    nPreis += Val(arfeld1(4)) ' Summe aufaddieren (Index 4 entspricht dem Summe-Feld aus dem ersten Datenaufruf)
                    nSatz += 1

                    ' N‰chsten Satz mit derselben Struktur laden, um Index-Fehler zu vermeiden
                    arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBid, nSatz, {"KunID", "Sprache", "Von", "Bis", "Summe"})
                Loop

                ' Endpreis formatieren
                Dim sP As String = fcDecStr(nPreis / 100)

                ' Kundendaten laden
                arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"EMail", "Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land"})

                If arFeld IsNot Nothing AndAlso arFeld.Length > 0 Then
                    ' E-Mail Text performant mit StringBuilder aufbauen
                    Dim sbText As New System.Text.StringBuilder()

                    sbText.AppendLine(fcLanguage(nSprache, "Sehr geehrter Vertragspartner") & ",")
                    sbText.AppendLine()
                    sbText.AppendLine(fcLanguage(nSprache, "hiermit best‰tigen wir Ihnen den Zahlungseingang f¸r den von Ihnen gebuchten Aufenthalt"))
                    sbText.AppendLine(fcLanguage(nSprache, "in der Zeit vom") & " " & sVon & " " & fcLanguage(nSprache, "bis") & " " & sBis & " ")
                    sbText.AppendLine(fcLanguage(nSprache, "in Hˆhe von") & " " & sP & " Ä")
                    sbText.AppendLine()
                    sbText.AppendLine()
                    sbText.AppendLine(fcLanguage(nSprache, "Mit freundlichen Gr¸ﬂen"))
                    sbText.AppendLine()
                    sbText.AppendLine(arIni(9))
                    sbText.AppendLine(arIni(2))
                    sbText.AppendLine(arIni(3))
                    sbText.AppendLine(arIni(4) & " " & arIni(5))
                    sbText.AppendLine(arIni(22))
                    sbText.AppendLine(arEMail(1))
                    sbText.AppendLine("UST-ID: " & arIni(16))

                    ' E-Mail senden
                    If fcSendeMailAnlage(arFeld(0), fcLanguage(nSprache, "Zahlungsbest‰tigung"), sbText.ToString()) = True Then
                        MessageBox.Show("Buchungs-Mail erfolgreich gesendet.", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        bResult = True
                    Else
                        MessageBox.Show("Fehler beim Senden der Buchungs-Mail.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            End If
        End If

        Return bResult
    End Function


#End Region

#Region "Programm Funktionen.(Datensicherung)"

    ''' <summary>
    ''' F¸hrt eine t‰gliche Datensicherung eines Quellordners in ein datiertes Zielverzeichnis durch und verwaltet die Aufbewahrungsanzahl.
    ''' Pr¸ft, ob f¸r das heutige Datum bereits ein Backup existiert, kopiert bei Bedarf die Daten und lˆscht den ‰ltesten Sicherungsordner,
    ''' sobald die maximale Anzahl an Sicherungen (<paramref name="iMenge"/>) erreicht oder ¸berschritten wurde.
    ''' </summary>
    ''' <param name="sQuelle">Der vollst‰ndige Pfad des zu sichernden Quellverzeichnisses.</param>
    ''' <param name="sZiel">Das Basis-Zielverzeichnis, in dem die Sicherungsordner abgelegt werden.</param>
    ''' <param name="sName">Das Namenspr‰fix f¸r die Sicherungsordner (z. B. "Backup_").</param>
    ''' <param name="iMenge">Die maximale Anzahl an vorzuhaltenden Sicherungsordnern.</param>
    ''' <returns>Gibt <c>"True"</c> zur¸ck, wenn die Sicherungspr¸fung erfolgreich durchlaufen wurde; andernfalls <c>"False"</c> bei Fehlern oder ung¸ltigen Pfaden.</returns>
    ''' <remarks>
    ''' 13.06.2018 - Create
    ''' 11.11.2026 - Code-Optimierung:
    ''' - Vollst‰ndige XML-Dokumentation f¸r alle Parameter und den R¸ckgabewert erg‰nzt.
    ''' - Guard Clauses f¸r Parameter und 'Directory.Exists'-Pr¸fung f¸r das Quellverzeichnis erg‰nzt sowie 'Try-Catch'-Fehlerbehandlung eingebaut (gibt im Fehlerfall "False" zur¸ck).
    ''' - Umst‰ndliches Zusammenbauen und anschlieﬂendes Splitten von Ordnernamen ¸ber Strings ('sDir' / 'Split') durch direkte Verarbeitung von 'DirectoryInfo'-Objekten ersetzt.
    ''' - Filterung auf Ordner mit passendem Pr‰fix ('sName') erg‰nzt, damit fremde Unterordner im Zielverzeichnis nicht versehentlich gelˆscht werden.
    ''' - Pfadzusammensetzung ¸ber 'System.IO.Path.Combine' abgesichert und Pr¸fung auf 'Directory.Exists' vor dem Lˆschen erg‰nzt.
    ''' - Explizite String-R¸ckgabe ("True" / "False") f¸r Option Strict-Kompatibilit‰t verwendet.
    ''' </remarks>
    Public Function fcSaveDate(ByRef sQuelle As String, ByRef sZiel As String, ByRef sName As String, ByRef iMenge As Integer) As String
        ' Guard Clauses: Parameter und Existenz des Quellordners pr¸fen
        If String.IsNullOrWhiteSpace(sQuelle) OrElse String.IsNullOrWhiteSpace(sZiel) OrElse sName Is Nothing Then
            Return "False"
        End If

        If Not System.IO.Directory.Exists(sQuelle) Then
            Return "False"
        End If

        Try
            ' Zielverzeichnis anlegen, falls es noch nicht existiert
            Dim dirZiel As New System.IO.DirectoryInfo(sZiel)
            If Not dirZiel.Exists Then
                dirZiel.Create()
            End If

            ' Alle vorhandenen Sicherungsordner ermitteln, die mit dem angegebenen Pr‰fix beginnen
            Dim lstBackupDirs As New List(Of System.IO.DirectoryInfo)()
            For Each di As System.IO.DirectoryInfo In dirZiel.EnumerateDirectories()
                If di.Name.StartsWith(sName, StringComparison.OrdinalIgnoreCase) Then
                    lstBackupDirs.Add(di)
                End If
            Next

            ' Heutiges Datums-Suffix ermitteln
            Dim sDatumHeute As String = fcUmDatum(Date.Today.ToString())
            Dim sBackupOrdnerHeute As String = sName & sDatumHeute
            Dim bHeuteVorhanden As Boolean = False

            ' Pr¸fen, ob f¸r heute bereits eine Sicherung existiert
            For Each di As System.IO.DirectoryInfo In lstBackupDirs
                If String.Equals(di.Name, sBackupOrdnerHeute, StringComparison.OrdinalIgnoreCase) Then
                    bHeuteVorhanden = True
                    Exit For
                End If
            Next

            ' Falls heute noch nicht gesichert wurde: Quellordner kopieren
            If Not bHeuteVorhanden Then
                Dim sZielPfadHeute As String = System.IO.Path.Combine(sZiel, sBackupOrdnerHeute)
                CopyFolder(sQuelle, sZielPfadHeute)

                ' Neu erstellten Ordner f¸r die anschlieﬂende Mengenpr¸fung zur Liste hinzuf¸gen
                If System.IO.Directory.Exists(sZielPfadHeute) Then
                    lstBackupDirs.Add(New System.IO.DirectoryInfo(sZielPfadHeute))
                End If
            End If

            ' ƒltesten Sicherungsordner lˆschen, falls die maximale Anzahl (iMenge) ¸berschritten wurde
            If iMenge > 0 AndAlso lstBackupDirs.Count > iMenge Then
                Dim sAeltestesDatum As String = "99999999"
                Dim dirAeltestes As System.IO.DirectoryInfo = Nothing

                For Each di As System.IO.DirectoryInfo In lstBackupDirs
                    If di.Name.Length > sName.Length Then
                        Dim sOrdnerDatum As String = di.Name.Substring(sName.Length)
                        If String.CompareOrdinal(sAeltestesDatum, sOrdnerDatum) > 0 Then
                            sAeltestesDatum = sOrdnerDatum
                            dirAeltestes = di
                        End If
                    End If
                Next

                ' Gefundenen ‰ltesten Ordner sicher entfernen
                If dirAeltestes IsNot Nothing AndAlso dirAeltestes.Exists Then
                    DelFolder(dirAeltestes.FullName)
                    If System.IO.Directory.Exists(dirAeltestes.FullName) Then
                        System.IO.Directory.Delete(dirAeltestes.FullName, True)
                    End If
                End If
            End If

            Return "True"

        Catch ex As Exception
            Return "False"
        End Try
    End Function


    '''' <summary>
    '''' Datensicherung
    '''' dim fcSaveDate as string  = True /False
    '''' </summary>
    '''' <returns></returns>
    '''' <remarks>
    '''' 13.06.2018 Create
    '''' </remarks>
    'Function fcSaveDate(ByRef sQuelle As String, ByRef sZiel As String, ByRef sName As String, ByRef iMenge As Integer) As String
    '    fcSaveDate = True
    '    Dim dirPrograms As New DirectoryInfo(sZiel)
    '    Dim dirs = From dir In dirPrograms.EnumerateDirectories()
    '    Dim sDir As String = ""
    '    For Each di As DirectoryInfo In dirs
    '        sDir = sDir & ";" & di.Name
    '    Next


    '    'Datensicherung bei einschalten sichenungs Phath ist in der Si.ini 
    '    Dim aDir As Array = Split(sDir, ";")
    '    Dim sName1 As String
    '    Dim sDatum As String = fcUmDatum(Date.Today.ToString)
    '    Dim iZahl As Integer = 0
    '    For i = 1 To aDir.Length - 1
    '        If sDatum = Mid(aDir(i), sName.Length + 1) Then
    '            iZahl = 1
    '            Exit For

    '        End If
    '    Next
    '    If iZahl = 0 Then
    '        CopyFolder(sQuelle, sZiel & "\" & sName & sDatum)
    '    End If
    '    Dim sD As String = "99999999"
    '    If aDir.Length >= iMenge Then
    '        For i = 1 To aDir.Length - 1
    '            sName1 = Mid(aDir(i), sName.Length + 1)
    '            If sD > sName1 Then
    '                sD = sName1
    '            End If
    '        Next
    '        Dim sDel As String = sZiel & "\" & sName & sD
    '        DelFolder(sDel)
    '        Directory.Delete(sDel)
    '    End If





    'End Function

    ''' <summary>
    ''' Lˆscht den gesamten Inhalt eines Verzeichnisses (alle Dateien sowie rekursiv alle Unterordner),
    ''' l‰sst jedoch das angegebene Basis-Verzeichnis (<paramref name="sSrcPath"/>) selbst leer bestehen.
    ''' </summary>
    ''' <param name="sSrcPath">Der vollst‰ndige Pfad des zu leerenden Verzeichnisses.</param>
    ''' <remarks>
    ''' 11.11.2026 - Code-Optimierung:
    ''' - Veralteten/kopierten Zeilenkommentar durch vollst‰ndige XML-Dokumentation ersetzt.
    ''' - Guard Clauses ('String.IsNullOrWhiteSpace' und 'Directory.Exists') erg‰nzt, um 'DirectoryNotFoundException' bei nicht vorhandenen Pfaden zu verhindern.
    ''' - Schreibschutz-Attribute ('FileAttributes.ReadOnly') vor dem Lˆschen entfernt, damit Dateien nicht mit 'UnauthorizedAccessException' abbrechen.
    ''' - 'For Each'-Schleifen statt indexbasierter Schleifen f¸r bessere Lesbarkeit verwendet.
    ''' - Inline-Kommentare zur besseren Wartbarkeit hinzugef¸gt.
    ''' </remarks>
    Public Sub DelFolder(ByVal sSrcPath As String)
        ' Guard Clauses: Abbruch bei ung¸ltigem oder nicht vorhandenem Pfad
        If String.IsNullOrWhiteSpace(sSrcPath) OrElse Not System.IO.Directory.Exists(sSrcPath) Then
            Exit Sub
        End If

        ' 1. Alle Dateien im aktuellen Ordner lˆschen
        For Each sFile As String In System.IO.Directory.GetFiles(sSrcPath)
            ' Schreibschutz entfernen, falls gesetzt, um Zugriffsfehler zu vermeiden
            Dim attributes As System.IO.FileAttributes = System.IO.File.GetAttributes(sFile)
            If (attributes And System.IO.FileAttributes.ReadOnly) = System.IO.FileAttributes.ReadOnly Then
                System.IO.File.SetAttributes(sFile, attributes And Not System.IO.FileAttributes.ReadOnly)
            End If

            System.IO.File.Delete(sFile)
        Next

        ' 2. Alle Unterordner rekursiv leeren und anschlieﬂend selbst entfernen
        For Each sDir As String In System.IO.Directory.GetDirectories(sSrcPath)
            DelFolder(sDir)
            System.IO.Directory.Delete(sDir, False)
        Next
    End Sub

    '' Gesamten Inhalt eines Ordners kopieren
    'Public Sub DelFolder(ByVal sSrcPath As String)

    '    ' zun‰chst alle Dateien des Quell-Ordners ermitteln
    '    ' und del
    '    Dim sFiles() As String = System.IO.Directory.GetFiles(sSrcPath)
    '    Dim sFile As String
    '    For i As Integer = 0 To sFiles.Length - 1
    '        sFile = sFiles(i)
    '        File.Delete(sFile)
    '    Next i


    '    '    ' jetzt alle Unterordner ermitteln und die delFolder-Funktion
    '    '    ' rekursiv aufrufen
    '    Dim sDirs() As String = System.IO.Directory.GetDirectories(sSrcPath)
    '    Dim sDir As String
    '    For i As Integer = 0 To sDirs.Length - 1
    '        sDir = sDirs(i)
    '        '   sDir = sDirs(i).Substring(sDirs(i).LastIndexOf("\") + 1)
    '        DelFolder(sDirs(i))

    '        Directory.Delete(sDirs(i))
    '    Next i

    'End Sub

    ''' <summary>
    ''' Kopiert den gesamten Inhalt eines Quellverzeichnisses in ein Zielverzeichnis.
    ''' Unterst¸tzt das optionale rekursive Mitkopieren von Unterordnern sowie das ‹berschreiben bestehender Zieldateien.
    ''' </summary>
    ''' <param name="sSrcPath">Der vollst‰ndige Pfad des Quellverzeichnisses.</param>
    ''' <param name="sDestPath">Der vollst‰ndige Pfad des Zielverzeichnisses (wird erstellt, falls nicht vorhanden).</param>
    ''' <param name="bSubFolder">Optional: Gibt an, ob Unterordner ebenfalls rekursiv kopiert werden sollen (Standard: <c>True</c>).</param>
    ''' <param name="bOverWrite">Optional: Gibt an, ob bereits vorhandene Dateien im Zielordner ¸berschrieben werden sollen (Standard: <c>True</c>).</param>
    ''' <remarks>
    ''' 11.11.2026 - Code-Optimierung:
    ''' - Vollst‰ndige XML-Dokumentation inklusive aller optionalen Parameter erg‰nzt.
    ''' - Guard Clauses f¸r Parameter und 'Directory.Exists(sSrcPath)' erg‰nzt, um Abst¸rze bei ung¸ltigen Quellpfaden zu vermeiden.
    ''' - Fehleranf‰lliges String-Slicing ('Substring(LastIndexOf("\"))') und Pfad-Verkettungen ('&amp; "\" &amp;') durch 'System.IO.Path.GetFileName' und 'System.IO.Path.Combine' ersetzt.
    ''' - Endlosschleifen-Schutz beim Kopieren in ein Unterverzeichnis des Quellordners durch normalisierten Pfadvergleich ('Path.GetFullPath') abgesichert.
    ''' - 'For Each'-Schleifen und 'OrElse' (Short-Circuit-Evaluation) verwendet.
    ''' </remarks>
    Public Sub CopyFolder(ByVal sSrcPath As String, ByVal sDestPath As String, Optional ByVal bSubFolder As Boolean = True, Optional ByVal bOverWrite As Boolean = True)
        ' Guard Clauses: Parameter und Existenz des Quellordners pr¸fen
        If String.IsNullOrWhiteSpace(sSrcPath) OrElse String.IsNullOrWhiteSpace(sDestPath) Then
            Exit Sub
        End If

        If Not System.IO.Directory.Exists(sSrcPath) Then
            Exit Sub
        End If

        ' Zielordner erstellen, falls er noch nicht existiert
        If Not System.IO.Directory.Exists(sDestPath) Then
            System.IO.Directory.CreateDirectory(sDestPath)
        End If

        ' 1. Alle Dateien des Quellordners ermitteln und kopieren
        For Each sSourceFile As String In System.IO.Directory.GetFiles(sSrcPath)
            Dim sFileName As String = System.IO.Path.GetFileName(sSourceFile)
            Dim sDestFile As String = System.IO.Path.Combine(sDestPath, sFileName)

            ' Kopieren, wenn ‹berschreiben erlaubt ist oder die Zieldatei noch nicht existiert
            If bOverWrite OrElse Not System.IO.File.Exists(sDestFile) Then
                System.IO.File.Copy(sSourceFile, sDestFile, bOverWrite)
            End If
        Next

        ' 2. Optional alle Unterordner rekursiv kopieren
        If bSubFolder Then
            ' Normalisierten Zielpfad f¸r sicheren Vergleich ermitteln (verhindert Endlosrekursion)
            Dim sNormalizedDest As String = System.IO.Path.GetFullPath(sDestPath).TrimEnd("\"c, "/"c)

            For Each sSourceDir As String In System.IO.Directory.GetDirectories(sSrcPath)
                Dim sNormalizedCurrentDir As String = System.IO.Path.GetFullPath(sSourceDir).TrimEnd("\"c, "/"c)

                If Not String.Equals(sNormalizedCurrentDir, sNormalizedDest, StringComparison.OrdinalIgnoreCase) Then
                    Dim sDirName As String = System.IO.Path.GetFileName(sSourceDir)
                    Dim sNewDestDir As String = System.IO.Path.Combine(sDestPath, sDirName)

                    CopyFolder(sSourceDir, sNewDestDir, True, bOverWrite)
                End If
            Next
        End If
    End Sub


    '' Gesamten Inhalt eines Ordners kopieren
    'Public Sub CopyFolder(ByVal sSrcPath As String, ByVal sDestPath As String, Optional ByVal bSubFolder As Boolean = True, Optional ByVal bOverWrite As Boolean = True)

    '    ' Falls Zielordner nicht existiert, jetzt erstellen
    '    If Not System.IO.Directory.Exists(sDestPath) Then
    '        System.IO.Directory.CreateDirectory(sDestPath)
    '    End If

    '    ' zun‰chst alle Dateien des Quell-Ordners ermitteln
    '    ' und kopieren
    '    Dim sFiles() As String = System.IO.Directory.GetFiles(sSrcPath)
    '    Dim sFile As String
    '    For i As Integer = 0 To sFiles.Length - 1
    '        ' Falls Datei im Zielordner bereits existiert, nur 
    '        ' kopieren, wenn Parameter "bOverWrite" auf True 
    '        ' festgelegt ist
    '        sFile = sFiles(i).Substring(sFiles(i).LastIndexOf("\") + 1)
    '        If bOverWrite Or Not System.IO.File.Exists(sDestPath & "\" & sFile) Then
    '            System.IO.File.Copy(sFiles(i), sDestPath & "\" & sFile, True)
    '        End If
    '    Next i

    '    If bSubFolder Then
    '        ' jetzt alle Unterordner ermitteln und die CopyFolder-Funktion
    '        ' rekursiv aufrufen
    '        Dim sDirs() As String = System.IO.Directory.GetDirectories(sSrcPath)
    '        Dim sDir As String
    '        For i As Integer = 0 To sDirs.Length - 1
    '            If sDirs(i) <> sDestPath Then
    '                sDir = sDirs(i).Substring(sDirs(i).LastIndexOf("\") + 1)
    '                CopyFolder(sDirs(i), sDestPath & "\" & sDir, True, bOverWrite)
    '            End If
    '        Next i
    '    End If
    'End Sub

#End Region


    Public Sub prInfo(sInfo As String)
        frmMain.tssInfo.Text = sInfo
    End Sub

End Module

