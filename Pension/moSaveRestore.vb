Imports MySql
Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports System.IO


Imports System.Text

Module moSaveRestore
    Public con As New MySqlConnection

#Region "Save......................................................................................"

    '''' <summary>
    '''' Start der manuellen Sicherung aller Daten des Programmes                              
    '''' </summary>
    '''' <remarks>
    '''' 05.02.2012 Create
    '''' </remarks>
    'Public Sub prSaveDaten()
    '    Dim cFile As String  'Name der Sicherungsdatei
    '    Dim sDir As String   'Sicherungsverzeichnis
    '    Dim nTag As Integer  'Aktueller Tag als Nummer
    '    Dim sTag As String   'Aktueller Tag als Name
    '    Try
    '        nTag = Weekday(Date.Today, FirstDayOfWeek.Monday)
    '        sTag = "Montag"
    '        Select Case nTag
    '            Case 1
    '                sTag = "Montag"
    '            Case 2
    '                sTag = "Dienstag"
    '            Case 3
    '                sTag = "Mittwoch"
    '            Case 4
    '                sTag = "Donnerstag"
    '            Case 5
    '                sTag = "Freitag"
    '            Case 6
    '                sTag = "Samstag"
    '            Case 7
    '                sTag = "Sonntag"
    '        End Select

    '        sDir = arIni(30) & "\" & sTag

    '        If CreateDir(sDir) = False Then
    '            MsgBox("Verzeichnis konnte nicht angelegt werden, Sicherung wurde nicht durchgeführt!")
    '            Exit Sub
    '        End If

    '        cFile = sDir & "\" & "SaveDB" & Date.Today & ".dat"
    '        cFile = fcCheckFile(cFile)

    '        'Daten in Datei speichern
    '        Call WriteFileSeriell(cFile, fcSaveDatabase())

    '    Catch ex As Exception
    '        ErrReport(ex.Message, ex.Source, ex.StackTrace)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Führt die serielle Datensicherung der gesamten Datenbank in ein wochentagsabhängiges Verzeichnis aus.
    ''' </summary>
    ''' <remarks>
    ''' <para>Ermittelt den aktuellen Wochentag auf Deutsch, erstellt falls nötig das Zielverzeichnis aus den INI-Einstellungen und schreibt die serialisierten Daten.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 05.02.2012 – Erstellt<br/>
    ''' 22.09.2026 – Optimiert: Wochentagsermittlung modernisiert, sicheres ISO-Datumsformat für Dateinamen eingeführt und VB6-Überbleibsel (<c>Call</c>, <c>MsgBox</c>) entfernt.<br/>
    ''' </para>
    ''' </remarks>
    Public Sub prSaveDaten()
        Try
            ' Modern & kompakt: Holt den deutschen Wochentagsnamen direkt aus den Kultur-Infos
            Dim info As New System.Globalization.CultureInfo("de-DE")
            Dim sTag As String = info.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek)

            ' Zielverzeichnis aus Array (Index 30) auslesen und Wochentag anhängen
            Dim sDir As String = Path.Combine(arIni(30), sTag)

            ' Sicherheitsprüfung: Verzeichnis erstellen (.NET-konforme Prüfung)
            If Not CreateDir(sDir) Then
                MessageBox.Show("Verzeichnis konnte nicht angelegt werden. Die Sicherung wurde abgebrochen!", "Fehler bei der Datensicherung", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' WICHTIGE OPTIMIERUNG: Ein sicheres Dateiformat wählen (z.B. SaveDB_2026_09_22.dat)
            ' Das originale "Date.Today" erzeugt Punkte (.), was zu Problemen mit Dateierweiterungen führen kann.
            Dim sDatumsSuffix As String = DateTime.Today.ToString("yyyy_MM_dd")
            Dim cFile As String = Path.Combine(sDir, "SaveDB_" & sDatumsSuffix & ".dat") ' sDir & "\SaveDB_" & sDatumsSuffix & ".dat"

            ' Datei-Prüfung ausführen
            cFile = fcCheckFile(cFile)

            ' Daten übergeben und speichern (Ohne das alte 'Call')
            WriteFileSeriell(cFile, fcSaveDatabase())

        Catch ex As Exception
            ' Fehler an die zentrale Fehlerverwaltung übergeben
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            ' Fehler weiterreichen, damit die aufrufende Methode (prSaveData) bescheid weiß und kein falsches Erfolgs-Log schreibt
            Throw
        End Try
    End Sub


    '''' <summary>
    '''' Sicherung der einzelnen Tabellen wird gestartet
    '''' </summary>
    '''' <returns></returns>
    '''' <remarks>
    '''' 05.02.2012 Create
    '''' </remarks>
    'Public Function fcSaveDatabase() As String
    '    Dim arTable As Array = Split(fcGetTable(conString), ",")
    '    Dim sSaveData As String = ""
    '    Try
    '        For i = 0 To arTable.Length - 1
    '            sSaveData = sSaveData & fcSaveData(arTable(i))
    '        Next
    '        fcSaveDatabase = sSaveData
    '    Catch ex As Exception
    '        ErrReport(ex.Message, ex.Source, ex.StackTrace)
    '        fcSaveDatabase = ""
    '    End Try
    'End Function

    ''' <summary>
    ''' Startet die sequentielle Sicherung aller in der Datenbank vorhandenen Tabellen und führt die Daten in einer einzigen Zeichenkette zusammen.
    ''' </summary>
    ''' <returns>Eine serialisierte Zeichenkette, die die exportierten Daten aller Tabellen enthält. Im Fehlerfall wird ein leerer String zurückgegeben.</returns>
    ''' <remarks>
    ''' <para>Ermittelt die Tabellennamen über <see cref="fcGetTable"/> anhand des Verbindungsstrings und durchläuft diese in einer Schleife.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 05.02.2012 – Erstellt<br/>
    ''' 22.09.2026 – Optimiert: Umstellung von langsamer String-Verknüpfung auf den hochperformanten <c>StringBuilder</c>, Behebung des Late-Binding-Risikos beim Array und Einführung des <c>Return</c>-Schlüsselworts.<br/>
    ''' </para>
    ''' </remarks>
    Public Function fcSaveDatabase() As String
        Try
            ' Tabellennamen abrufen
            Dim tableString As String = fcGetTable(conString)
            If String.IsNullOrEmpty(tableString) Then Return String.Empty

            ' Typensicheres String-Array statt des generischen Typs 'Array' verwenden (verhindert Late Binding)
            Dim arTable() As String = tableString.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries)

            ' WICHTIGE OPTIMIERUNG: StringBuilder reserviert einen dynamischen Speicherbereich.
            ' Das verhindert das ressourcenfressende Neuerstellen von Strings in der Schleife.
            Dim sbSaveData As New System.Text.StringBuilder()

            ' Schleife mit expliziter Typisierung der Laufvariable
            For i As Integer = 0 To arTable.Length - 1
                ' Den getrimmten Tabellennamen sichern und anhängen
                Dim tableName As String = arTable(i).Trim()
                If tableName <> "" Then
                    sbSaveData.Append(fcSaveData(tableName))
                End If
            Next

            ' Den fertigen Text .NET-konform zurückgeben
            Return sbSaveData.ToString()

        Catch ex As Exception
            ' Fehler an die zentrale Fehlerverwaltung übergeben
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return String.Empty
        End Try
    End Function


    ''' <summary>
    ''' Sicherung der ausgewählten Tabelle wird durchgeführt. Daten werden in das Array arSaveDB gespeichert.
    ''' </summary>
    ''' <param name="sTB"></param>
    ''' <param name="sStart"></param>
    ''' <param name="n"></param>
    ''' <remarks>
    ''' 05.02.2012 Create
    ''' </remarks>
    Public Function fcSaveData(ByVal sStart As String) As String

        Dim i As Integer           'Laufvariable
        Dim k As Integer           'Laufvariable
        Dim j As Long              'Laufvariable
        Dim arZeile() As String    'Temporäres Array zum Speichern eine Zeile
        Dim dtTmp As DataTable
        Dim nMax As Integer
        ' Dim nMaxs As Integer

        Dim lStart As Boolean = False
        Dim arFieles() As String
        Dim ii As Integer = 0
        ReDim arFieles(0)

        fcSaveData = ""
        ii = ii - 1

        Try
            dtTmp = fcReadDataTable("Select * from " & sStart)
            nMax = dtTmp.Rows.Count
            Dim arSaveDB(nMax + 2) As String
            Dim sFeldName As String = ""
            Dim nMaxc As Integer = dtTmp.Columns.Count
            ReDim arZeile(nMaxc)
            arSaveDB(0) = "<" & sStart & ">" & vbCrLf    'Startposition (Beginn der Tabelle in der Sicherung) <Zimmer>
            arSaveDB(1) = fcTableStructure(sStart, conString) & vbCrLf
            If nMax <> 0 Then
                j = 1
                For i = 0 To nMax - 1
                    j = j + 1
                    For k = 0 To nMaxc - 1
                        arZeile(k) = dtTmp.Rows(i).Item(k).ToString
                    Next
                    'vbCrLf aus Hinweisfeldern in chr(182) umwandeln, sonst keine richtiege sicherung möglich
                    arSaveDB(j) = Join(arZeile, Chr(176)).Replace(vbCrLf, Chr(182)) & vbCrLf '"°"
                Next
            End If
            arSaveDB(nMax + 2) = "<End>" & vbCrLf   'Ende der Tabelle in der Sicherung

            fcSaveData = Join(arSaveDB)
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Function
    '...
    ''' <summary>
    ''' Erstellen einer Datatable mit der Struktur einer SQL-Tabelle als Inhalt***** Nur unter Linox MySQL!
    ''' </summary>
    ''' <param name="tabNameSrc">Name der Quelltabelle</param>
    ''' <returns>String mit der Struktur</returns>
    ''' <remarks>"Feldname,Typ,Länge ° Feldname..."   °=chr(176) .</remarks>
    ''' 
    Public Function fcTableStructure(ByVal tabNameSrc As String, ByRef con As String) As String
        'Dim dtTemp As DataTable

        conPension = New MySqlConnection
        conPension.ConnectionString = con
        Dim sFeldStru As String = ""
        Dim sSQL As String = "SELECT column_name, data_type, character_maximum_length, " & _
              "numeric_precision, numeric_scale " & _
               "FROM information_schema.COLUMNS " & _
               "WHERE table_name LIKE @TableName " & _
               "ORDER BY ordinal_position"

        Dim cmd As New MySqlCommand(sSQL, conPension)
        cmd.Parameters.AddWithValue("@TableName", tabNameSrc)

        Dim adp As New MySqlDataAdapter(cmd)
        Dim tempT As New DataTable("dtTemp")

        Try
            With adp
                .Fill(tempT)
                .Dispose()
            End With
            For ii = 0 To tempT.Rows.Count - 1
                For i = 0 To 2
                    sFeldStru = sFeldStru & tempT.Rows(ii).Item(i).ToString & ","
                Next
                sFeldStru = Mid(sFeldStru, 1, Len(sFeldStru) - 1) & Chr(176)
            Next

            fcTableStructure = Mid(sFeldStru, 1, sFeldStru.Length - 1)
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, "FEHLER", _
              MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End Try
    End Function
   

    Private Function fcGetTable(ByRef con As String) As String

        Dim t As String = ""
        Dim dt As New DataTable
        Dim adptr As MySqlDataAdapter
        Using conPension As New MySqlConnection(con)
            Using cmd As New MySqlCommand("Select table_name from information_schema.tables WHERE table_schema = 'pension'", conPension)
                Try
                    conPension.Open()
                    adptr = New MySqlDataAdapter(cmd)
                    adptr.Fill(dt)
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try
            End Using
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            For i = 0 To dt.Rows.Count - 1
                t = t & dt.Rows(i).Item(0).ToString & ","
            Next
            t = Mid(t, 1, Len(t) - 1)
        End If

        fcGetTable = t

        conPension.Close()
    End Function
#End Region

    '#Region "Restore..................................................................................."

    '    ''' <summary>
    '    ''' Datenrücksicherung wird vorbereiten und das Modul für die Rücksicherung gestartet.
    '    ''' </summary>
    '    ''' <remarks>
    '    ''' 05.02.2012 Create
    '    ''' </remarks>
    '    Public Sub prRestore(ByVal sFile As String)
    '        '   Dim sDatei As String = ReadFileSeriell(cgPfad & "\SaveDB\DBSicherung.dat")
    '        Dim arDatei() As String = Split(ReadFileSeriell(cgPfad & "\SaveDB\DBSicherung.dat"), vbCrLf)
    '        Dim sTabelleName As String = ""
    '        Dim iStru As Integer = 0
    '        Dim arStrucktur() As String
    '        Dim sDaten As String = 0
    '        Dim arDaten() As String
    '        For i = 0 To UBound(arDatei) - 1
    '            If Mid(arDatei(i), 1, 1) = "<" And arDatei(i) <> "<End>" Then
    '                sTabelleName = Trim(Mid(arDatei(i), 2, arDatei(i).Length - 2))
    '                iStru = 1
    '            Else
    '                If iStru = 1 Then
    '                    arStrucktur = Split(Trim(arDatei(i)), "°")
    '                    iStru = 2
    '                Else
    '                    If iStru = 2 Then
    '                        If Trim(arDatei(i)) = "<End>" Then
    '                            iStru = 3
    '                        Else
    '                            sDaten = sDaten & Trim(arDatei(i)) & vbCrLf
    '                        End If


    '                    End If
    '                    If iStru = 3 Then

    '                        sDaten = Trim(Mid(sDaten, 1, sDaten.Length - 1))
    '                        arDaten = Split(sDaten, vbCrLf)
    '                        sDaten = ""
    '                        ' del tabelle  sTabelleName
    '                        frmMain.tslSatznummer.Text = "0"
    '                        frmMain.tslGesammt.Text = "0"
    '                        frmMain.tslTabelle.Text = sTabelleName & " Löschen"
    '                        frmMain.Refresh()
    '                        fcDeleteSQL(sTabelleName, conString) 'bei true alles gelöscht
    '                        ' create tabele sTabelle , arStrucktur
    '                        frmMain.tslTabelle.Text = sTabelleName & " Strucktur"
    '                        frmMain.Refresh()
    '                        CreateDBSQL(arStrucktur, sTabelleName, conString)
    '                        ' copy tabelle arStrucktur, arDaten
    '                        fcCopySQL(sTabelleName, arDaten, arStrucktur)
    '                        '   Exit Sub
    '                        iStru = 0
    '                    End If
    '                End If
    '            End If

    '        Next

    '    End Sub
    '    ''' <summary>
    '    ''' Procedure  : CreateDB
    '    ''' Created by : Uwe Reich
    '    ''' Date-Time  : 16.05.2007               Last Upate: 26.03.2011
    '    ''' </summary>
    '    ''' <param name="dbfile"></param>
    '    ''' <returns>
    '    ''' True / False
    '    ''' </returns>
    '    ''' <remarks>
    '    ''' Description: Erstellen der einzelnen Tabellen aus einer Resourcendatei
    '    ''' </remarks>
    '    Function CreateDBSQL(ByVal arStru As Array, ByRef cTabelle As String, ByRef conn As String) As Boolean

    '        Dim nMax As Integer

    '        Dim sSql As String
    '        '   Dim Sql As String
    '        Dim arStrucktur() As String
    '        CreateDBSQL = True
    '        Dim sTabelle As String = ""
    '        Try
    '            nMax = UBound(arStru)
    '            sSql = " ("
    '            For i = 0 To nMax
    '                arStrucktur = Split(arStru(i), ",")
    '                sSql = sSql & "`" & Trim(arStrucktur(0)) & "` "

    '                If Trim(arStrucktur(1)) = "varchar" Then

    '                    sSql = sSql & "varchar(" & arStrucktur(2) & ") NULL, "
    '                Else

    '                    sSql = sSql & "text " & " NULL, "
    '                End If

    '            Next
    '            sSql = Mid(sSql, 1, sSql.Length - 2)
    '            sSql = "CREATE TABLE " & cTabelle & sSql & ") CHARACTER SET utf8 COLLATE utf8_general_ci;"

    '            '     Sql = "CREATE TABLE Zimmer (`ID` varchar(16) NULL,`Name` varchar(50) NULL,`Art` varchar(100) NULL,`Betten` varchar(2) NULL,`Ausstattung` varchar(50) NULL,`FeWo` varchar(2) NULL,`IDObjekte` varchar(16) NULL,`Nummer` varchar(2) NULL) CHARACTER SET utf8 COLLATE utf8_general_ci;"



    '            '#################################################
    '            con = New MySqlConnection
    '            con.ConnectionString = conn
    '            con.Open()
    '            Dim cmd As New MySqlCommand(sSql, con)
    '            cmd.ExecuteNonQuery()
    '            cmd.Dispose()
    '            con.Close()
    '            '######################################################################


    '            '        UpdateTableSQL(sSql)




    '            CreateDBSQL = True
    '        Catch ex As Exception
    '            MsgBox(ex.Message, ex.Source, ex.StackTrace)
    '            CreateDBSQL = False
    '        End Try
    '        '  sTabelle = Mid(sTabelle, 1, sTabelle.Length - 1)
    '    End Function
    '    Function fcDeleteSQL(ByRef cTabele As String, ByRef conn As String) As Boolean
    '        fcDeleteSQL = True
    '        Try
    '            con = New MySqlConnection
    '            con.ConnectionString = conn
    '            con.Open()
    '            Dim cmd1 As New MySqlCommand("SELECT * FROM " & cTabele, con)
    '            Dim Zahl As Integer = cmd1.ExecuteNonQuery()
    '            cmd1.Dispose()
    '            con.Close()
    '            If Zahl = -1 Then
    '                Dim sSql As String = "DROP TABLE " & cTabele

    '                con = New MySqlConnection
    '                con.ConnectionString = conString


    '                con.Open()
    '                Dim cmd As New MySqlCommand(sSql, con)
    '                cmd.ExecuteNonQuery()
    '                cmd.Dispose()
    '                con.Close()
    '            End If
    '        Catch ex As Exception
    '            fcDeleteSQL = False
    '        End Try
    '    End Function


    '    Function fcCopySQL(ByRef cTabele As String, ByRef arDaten As Array, ByRef arStru As Array)

    '        Dim arStrucktur() As String

    '        Dim sID As String = ""

    '        Dim sSQLText As String = ""
    '        ' Dim sSQLValue As String = ""

    '        Dim arFields(0), arValue(0) As String


    '        For i = 0 To UBound(arStru)
    '            arStrucktur = Split(arStru(i), ",")
    '            sSQLText = sSQLText & arStrucktur(0) & ","
    '        Next


    '        sSQLText = Mid(sSQLText, 1, Len(sSQLText) - 1)
    '        arFields = Split(sSQLText, ",")

    '        Dim sValue As String
    '        Dim nFields As Integer = arFields.Length - 1
    '        Dim nMax As Integer = UBound(arDaten)
    '        '   Dim sWert As String = ""
    '        frmMain.tslTabelle.Text = cTabele & " Copy"
    '        frmMain.tslGesammt.Text = nMax + 1


    '        For i = 0 To nMax
    '            frmMain.tslSatznummer.Text = i + 1
    '            If i / 100 = Int(i / 100) Or nMax - i < 100 Then
    '                frmMain.Refresh()
    '            End If

    '            sValue = Trim(arDaten(i))
    '            sValue = sValue.Trim
    '            If sValue = "" Then Exit Function
    '            sValue = Mid(sValue, 1, Len(sValue) - 1)

    '            arValue = Split(sValue, "°")

    '            For ii = 0 To nFields
    '                arValue(ii) = arValue(ii).Replace(Chr(182), vbCrLf)
    '            Next
    '            sID = fcAppendBlank(cTabele)
    '            fcUpdateCommand(cTabele, arFields, arValue, " WHERE ID='" & sID & "'")

    '        Next


    '    End Function





    '    ' Aufrufbeispiel:

    '    'Dim tempT As DataTable = _
    '    '  TableStructureTable(myConnection, "mySourceTableName", "tempStructure")

    '    '' Inhalt des DataTable-Objekts in einem DataGridView anzeigen
    '    'DataGridView1.DataSource = tempT





















    'Function fcDBTabelle() As String
    '    Dim arDaten As Array = Split(StruckturBD, vbCrLf)
    '    Dim sEnd As String = "<End>"
    '    Dim lStart As Boolean
    '    Dim cTabelle As String = ""
    '    Dim nMax As Integer = UBound(arDaten) - 1
    '    For i = 0 To nMax
    '        If InStr(1, Trim$(arDaten(i)), "<") <> 0 And Trim$(arDaten(i)) <> sEnd Then
    '            cTabelle = cTabelle & Trim$(arDaten(i)) & ";"
    '            cTabelle = Replace(cTabelle, "<", "")
    '            cTabelle = Replace(cTabelle, ">", "")
    '            lStart = True
    '        End If
    '    Next
    '    cTabelle = Mid(cTabelle, 1, cTabelle.Length - 1)
    '    fcDBTabelle = cTabelle
    'End Function


    '#End Region

End Module
