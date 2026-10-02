Imports System.IO

Module moStart

    ''' <summary>
    ''' Zentraler Initialisierungsknoten beim Programmstart. Bereitet die Log-Infrastruktur vor, 
    ''' ermittelt die Web-IP aus der IP.ini (ggf. per FTP-Auflösung), prüft den System- und Fehlerstatus 
    ''' und stößt die globale Datenbankverbindung an.
    ''' </summary>
    ''' <returns>Die ermittelte und ggf. dynamisch aufgelöste Web-IP-Adresse (cgIPWeb).</returns>
    ''' <remarks>
    ''' 17.12.2011 Create <br/>
    ''' 21.09.2026 Code-Optimierung: Umstellung auf natives .NET (Path.Combine, File.ReadAllLines, String-Interpolation) und Entfernung von VB6-Altlasten.
    ''' </remarks>
    Public Function Main() As String
        ' Aktuellen Pfad sicher ermitteln (AppDomain ist zuverlässiger als CurDir)
        cgPfad = AppDomain.CurrentDomain.BaseDirectory.TrimEnd("\"c)
        ' Verzeichnisse und Logdateien vorbereiten
        Dim logDir As String = Path.Combine(cgPfad, "Log")
        Directory.CreateDirectory(logDir) ' Erstellt den Ordner, falls er nicht existiert
        cgLogFile = Path.Combine(logDir, "Log.txt")
        cgErrLogFile = Path.Combine(logDir, "ErrLog.log")

        ' IP.ini einlesen und auswerten
        Dim ipIniPath As String = Path.Combine(cgPfad, "IP.ini")
        If File.Exists(ipIniPath) Then
            ' Liest alle Zeilen ein und filtert leere Zeilen direkt heraus
            Dim lines() As String = File.ReadAllLines(ipIniPath)
            For Each line As String In lines
                Dim trimmedLine As String = line.Trim()
                If trimmedLine.Length > 0 AndAlso Not trimmedLine.StartsWith("/") Then
                    cgIPWeb = trimmedLine
                    Exit For
                End If
            Next
        End If
        ' Dynamische IP über FTP-Klasse auflösen, falls sie mit '#' beginnt
        If Not String.IsNullOrEmpty(cgIPWeb) AndAlso cgIPWeb.StartsWith("#") Then
            cgIPWeb = FTP.getWebIP(cgIPWeb.Substring(1))
        End If


        ' Dim WebIp As String = ReadFileSeriell("IP.INI") ' = FTP.getWebIP("elbepension.spdns.de")
        ' WebIp = "192.168.0.2" ' Netgir
        ' WebIp = "192.168.178.55" 'fritzbox
        ' https://dpddxjamtr339hza.myfritz.net:40360
        ' cgIPWeb = FTP.getWebIP("dpddxjamtr339hza.myfritz.net") 'Sabine
        ' cgIPWeb = "192.168.178.55"
        ' cgIPWeb = "127.0.0.1"
        ' conString = "server=" & cgIPWeb & ";port=3306;uid=Reich;pwd=02041883-Pi;database=pension;CharSet=utf8"
        ' cgPfad = "C:\pension_daten" 'CurDir() 


        ' System.ini einlesen (Erstes Zeichen prüfen)
        Dim systemIniPath As String = Path.Combine(cgPfad, "System.ini")
        If File.Exists(systemIniPath) Then
            Dim sysContent As String = ReadFileSeriell(systemIniPath)
            cgTablet = If(sysContent.Length > 0, sysContent.Substring(0, 1), "")
        End If
        ' Fehler-Logdatei auswerten (Fehler zählen)
        If File.Exists(cgErrLogFile) Then
            Dim errLines() As String = File.ReadAllLines(cgErrLogFile)
            ngError = 0 ' Zurücksetzen, um Dopplungen bei mehrmaligem Aufruf zu vermeiden

            For Each errLine As String In errLines
                If errLine.Contains("Message :") Then
                    ngError += 1
                End If
            Next

            ' Update der UI (Falls frmMain existiert und geladen ist)
            If frmMain IsNot Nothing AndAlso frmMain.tssLog IsNot Nothing Then
                frmMain.tssLog.Text = $"Info = {ngError}"
            End If
        End If

        Call prInitDatenBankGlobal()

        Call prLoadArINI()
        prDirControll()
        Main = cgIPWeb
    End Function

    ''' <summary>
    ''' Daten der INI-Datei laden oder Datei erstellen
    ''' </summary>
    ''' <remarks>
    ''' 17.12.2011 Create <br/>
    ''' 20.09.2026 Code-Optimierung: Zusammenfassung sequentieller Array-Indizes in Schleifenstrukturen zur Reduzierung von Redundanz und Erhöhung der Lesbarkeit.
    ''' 02.10.2026
    ''' - Mwst für Speise und Getränke und Gegenkonten entfernt
    ''' </remarks>
    Public Sub prLoadArINI()
        Dim IniFile As String = myInit.ReadIni()
        ' 1. Sequentielle Standard-Schlüssel 
        arIni(2) = myInit.ReadEntry(IniFile, KEY_COMPANY)
        arIni(3) = myInit.ReadEntry(IniFile, KEY_STR)
        arIni(4) = myInit.ReadEntry(IniFile, KEY_PLZ)
        arIni(5) = myInit.ReadEntry(IniFile, KEY_ORT)
        arIni(6) = myInit.ReadEntry(IniFile, KEY_TE1)
        arIni(7) = myInit.ReadEntry(IniFile, KEY_TE2)
        arIni(8) = myInit.ReadEntry(IniFile, KEY_TE3)
        arIni(9) = myInit.ReadEntry(IniFile, KEY_TE4)
        arIni(12) = myInit.ReadEntry(IniFile, KEY_KNr)
        arIni(13) = myInit.ReadEntry(IniFile, KEY_BNr)
        arIni(14) = myInit.ReadEntry(IniFile, KEY_RNr)
        arIni(15) = myInit.ReadEntry(IniFile, KEY_UStNr)
        arIni(16) = myInit.ReadEntry(IniFile, KEY_UStID)
        arIni(17) = myInit.ReadEntry(IniFile, KEY_Fr)
        arIni(22) = myInit.ReadEntry(IniFile, KEY_Web)
        arIni(24) = myInit.ReadEntry(IniFile, KEY_Ge)
        ' 2. Nicht-sequentielle Steuern und Konten 

        arIni(11) = myInit.ReadEntry(IniFile, KEY_MWST2)
        arIni(18) = myInit.ReadEntry(IniFile, KEY_KKonto)
        arIni(19) = myInit.ReadEntry(IniFile, KEY_BKonto)
        ' 3. Verzeichnisse 
        arIni(30) = myInit.ReadEntry(IniFile, KEY_SDIR)
        arIni(31) = myInit.ReadEntry(IniFile, KEY_DDIR)
        arIni(32) = myInit.ReadEntry(IniFile, KEY_ADIR)
        ' 4. System- und Netzwerk-Hardware 
        arIni(33) = myInit.ReadEntry(IniFile, KEY_NetUser)
        arIni(34) = myInit.ReadEntry(IniFile, KEY_IPSchloss)
        arIni(35) = myInit.ReadEntry(IniFile, KEY_RFIDPort)
        ' 5. Kategorien
        arIni(40) = myInit.ReadEntry(IniFile, KEY_UArt)
        arIni(41) = myInit.ReadEntry(IniFile, KEY_Kate)
        ' 6. E-Mail-Einstellungen (Sequentiell von Index 0 bis 9)
        Dim mailKeys() As String = {
         KEY_SMTP, KEY_EMAIL, KEY_SERVER, KEY_USER, KEY_PASS,
         KEY_MIME, KEY_UUE, KEY_HTML, KEY_AUTOR, KEY_BETREFF
        }
        For i As Integer = 0 To mailKeys.Length - 1
            arEMail(i) = myInit.ReadEntry(IniFile, mailKeys(i))
        Next

    End Sub

    ''' <summary>
    ''' Initialisiert globale Datenbanktabellen (Werbung, Zimmer, Buchungen, Objekte, Nutzer) 
    ''' für die Anwendung und lädt Daten ab einem definierten historischen Stichtag (2 Jahre rückwirkend).
    ''' </summary>
    ''' <remarks>
    ''' 20.09.2026 Code-Optimierung: Bereinigung von auskommentiertem totem Code, Optimierung der SQL-String-Zuweisungen und String-Interpolation.
    ''' </remarks>
    Public Sub prInitDatenBankGlobal()
        ' Berechnet das Datum von vor genau 2 Jahren (730 Tagen)
        Dim von2Jahre As String = fcUmDatum(Date.Today.AddDays(-730))

        ' Laden der globalen DataTables
        dtWer = fcReadDataTable("SELECT * FROM Werbung")
        dtZim = fcReadDataTable("SELECT * FROM Zimmer ORDER BY Nummer ASC")
        dtBuc = fcReadDataTable($"SELECT * FROM Buchung WHERE Von > '{von2Jahre}' ORDER BY Von ASC")

        ' Lokale DataTable (wird im Originalcode geladen, aber nicht weiterverwendet - falls nötig, global deklarieren)
        'Dim dt As DataTable = fcReadDataTable($"SELECT * FROM Buchung WHERE Von > '{von2Jahre}' ORDER BY RID ASC")

        dtObj = fcReadDataTable("SELECT * FROM Objekte")
        dtGas = fcReadDataTable("SELECT * FROM Nutzer") ' Nur zur Erstdeklaration
        dtKun = fcReadDataTable("SELECT * from Kunden") ' Nur zur Erstdeklaration
    End Sub

    ''' <summary>
    ''' Erstellt standardmäßig benötigte Anwendungs- und Datensicherungsverzeichnisse, 
    ''' falls diese noch nicht existieren.
    ''' </summary>
    Private Sub prDirControll()
        Try
            ' Erstellt den Ordner nur, wenn er nicht existiert – vollautomatisch
            Directory.CreateDirectory(Path.Combine(arIni(32), "Datev"))
            Directory.CreateDirectory(Path.Combine(cgPfad, "Ablage"))
        Catch ex As Exception
            ' Falls z.B. Schreibrechte fehlen, wird hier der Fehler abgefangen
            'Globals.LoggeFehler($"Fehler beim Erstellen der Ordner: {ex.Message}")
        End Try
    End Sub

    ''''' <summary>
    ''''' Erstellt standardmäßig benötigte Anwendungs- und Datensicherungsverzeichnisse, 
    ''''' falls diese noch nicht existieren.
    ''''' </summary>
    ''''' <param name="cPath">Optionaler Basispfad (wird in dieser Version durch ein internes Array strukturiert).</param>
    ''''' <remarks>
    ''''' 20.09.2026 Code-Optimierung: Entfernung von 'On Error Resume Next' und Umstellung auf ein robustes, schleifenbasiertes Erstellen von Verzeichnissen mittels System.IO.
    ''''' </remarks>
    'Public Sub prCreateDir(ByVal cPath As String)
    '    ' Alle benötigten Unterordner zentral als Array definieren
    '    Dim subDirectories() As String = {
    '     "BackUp", "Log", "EMail", "SaveDB", "Datev", "Ablage", "System"
    '    }

    '    ' Verwende cPath als Basis, falls übergeben und nicht leer, andernfalls cgPfad
    '    Dim basePath As String = If(Not String.IsNullOrEmpty(cPath), cPath, cgPfad)

    '    Try
    '        For Each folder As String In subDirectories
    '            Dim fullPath As String = Path.Combine(basePath, folder)
    '            ' Erstellt das Verzeichnis nativ mit .NET (löst keinen Fehler aus, wenn der Ordner bereits existiert)
    '            Directory.CreateDirectory(fullPath)
    '        Next
    '    Catch ex As Exception
    '        ' Optional: Fehler protokollieren, falls keine Schreibrechte vorliegen
    '        ' LogError("Fehler beim Erstellen der Verzeichnisse: " & ex.Message)
    '    End Try
    'End Sub

End Module
