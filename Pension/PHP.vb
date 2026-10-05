
Imports System.Net.Http
Imports System.Text
Imports System.IO

Imports System.Threading.Tasks


Imports System.Net.NetworkInformation
Imports System.Net
Public Class PHP
    'Alle SQLfubgtionen nur mit PHPdateien auf Webserver!
    'Passwoerter in der function und PHPdatei müssen übereinstimmen



    ''' <summary>
    ''' Verschluesseln von passwortern
    ''' </summary>
    ''' <param name="sString "></param>
    ''' <param name="sSecurityValue"></param>
    ''' <returns></returns>
    ''' <remarks>03.02.2011 Create koh chang
    ''' </remarks>
    Public Shared Function DeCrypt(ByVal sString As String,
                         ByVal sSecurityValue As String) As String
        Dim n, x, nMax As Integer
        nMax = Len(sString)
        Dim nMaxS = Len(sSecurityValue)
        DeCrypt = ""
        For n = 1 To nMax
            x = n Mod nMaxS
            If x = 0 Then x = nMaxS
            DeCrypt = DeCrypt & Chr(Asc(Mid$(sSecurityValue, x, 1)) Xor Asc(Mid$(sString, n, 1)))
        Next
    End Function
    ''' <summary>
    ''' Verschluesseln von passwortern rueckgabe wert in ASCII 
    ''' </summary>
    ''' <param name="sString "></param>
    ''' <param name="sSecurityValue"></param>
    ''' <returns></returns>
    ''' <remarks>03.02.2011 Create koh chang
    ''' </remarks>


    Public Shared Function DeCryptASCII(ByVal sString As String,
                       ByVal sSecurityValue As String) As String
        Dim n, x, nMax As Integer
        Dim c As Integer
        nMax = Len(sString)
        Dim nMaxS = Len(sSecurityValue)
        DeCryptASCII = ""
        For n = 1 To nMax
            x = n Mod nMaxS
            If x = 0 Then x = nMaxS
            c = Str(Asc(Mid(sSecurityValue, x, 1)) Xor Asc(Mid(sString, n, 1))).Trim
            DeCryptASCII = DeCryptASCII & c & ";"
        Next
        DeCryptASCII = Mid(DeCryptASCII, 1, DeCryptASCII.Length - 1)
    End Function


    ''' <summary>
    ''' Sendet eine synchrone HTTP-Anfrage an eine angegebene URL (z. B. an ein PHP-Skript) und gibt die Antwort als String zurück.
    ''' </summary>
    ''' <param name="url">Die Ziel-URL der Anfrage (z. B. "https://example.com").</param>
    ''' <param name="method">Die HTTP-Methode (z. B. "GET" oder "POST").</param>
    ''' <param name="data">Die zu übermittelnden Daten im "x-www-form-urlencoded"-Format (z. B. "param1=value1&amp;param2=value2").</param>
    ''' <returns>Die Antwort des Servers als String oder "ERROR" im Fehlerfall.</returns>
    ''' <remarks>
    ''' 22.092026 – Code Optimierung
    ''' <para>Umgestellt von der veralteten WebRequest-Klasse auf HttpClient für bessere Performance und sicheres Ressourcen-Management.</para>
    ''' </remarks>
    Public Shared Function PHP(ByVal url As String, ByVal method As String, ByVal data As String) As String
        ' HttpClient blockweise nutzen, um Ressourcen sauber freizugeben
        Using client As New HttpClient()
            Try
                Dim response As HttpResponseMessage
                Dim httpMethod As New HttpMethod(method.ToUpper())

                If httpMethod = HttpMethod.Post Then
                    ' POST-Anfrage mit x-www-form-urlencoded Daten
                    Using content As New StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded")
                        response = client.PostAsync(url, content).GetAwaiter().GetResult()
                    End Using
                ElseIf httpMethod = HttpMethod.Get Then
                    ' GET-Anfrage (Daten als Query-String an URL hängen)
                    Dim finalUrl As String = If(String.IsNullOrEmpty(data), url, $"{url}?{data}")
                    response = client.GetAsync(finalUrl).GetAwaiter().GetResult()
                Else
                    ' Andere Methoden (PUT, DELETE etc.)
                    Dim request As New HttpRequestMessage(httpMethod, url)
                    If Not String.IsNullOrEmpty(data) Then
                        request.Content = New StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded")
                    End If
                    response = client.SendAsync(request).GetAwaiter().GetResult()
                End If

                ' Prüft auf HTTP-Fehler (wirft Exception bei z.B. 404 oder 500)
                response.EnsureSuccessStatusCode()

                ' Antwort synchron auslesen
                Return response.Content.ReadAsStringAsync().GetAwaiter().GetResult()

            Catch ex As UriFormatException
                MsgBox("FEHLER! Ungültiges URL-Format. Vergiss nicht das 'http://' oder 'https://'.", MsgBoxStyle.Critical)
                Return "ERROR"
            Catch ex As HttpRequestException
                MsgBox($"Netzwerkfehler: {ex.Message}", MsgBoxStyle.Critical)
                Return "ERROR"
            Catch ex As Exception
                MsgBox($"Ein unerwarteter Fehler ist aufgetreten: {ex.Message}", MsgBoxStyle.Critical)
                Return "ERROR"
            End Try
        End Using
    End Function


    ''' <summary>
    ''' Sendet eine HTTP-Anfrage (z. B. POST oder GET) an ein PHP-Skript und gibt die Serverantwort zurück.
    ''' </summary>
    ''' <param name="url">Die Ziel-URL des Webservers.</param>
    ''' <param name="method">Die HTTP-Methode (z. B. "POST" oder "GET").</param>
    ''' <param name="data">Die zu übertragenden Rohdaten.</param>
    ''' <returns>Die Antwort des Servers als String, oder "ERROR" im Fehlerfall.</returns>
    ''' <remarks>
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veralteten 'WebRequest' durch den modernen, thread-sicheren 'HttpClient' ersetzt.
    ''' - 'Using'-Blöcke für automatisches Ressourcen- und Stream-Management implementiert.
    ''' - Typsichere Übergabe der Formulardaten als 'StringContent' mit korrektem Encoding.
    ''' - Flexibilität für Groß-/Kleinschreibung bei der HTTP-Methode ("post" vs. "POST") hinzugefügt.
    ''' </remarks>
    Public Shared Function PHPnoF(ByVal url As String, ByVal method As String, ByVal data As String) As String
        ' Validierung der Eingabeparameter
        If String.IsNullOrWhiteSpace(url) Then Return "ERROR"

        Try
            ' HttpClient instanziieren (Nutzt Using zur automatischen Freigabe von Ressourcen)
            Using client As New HttpClient()
                ' HTTP-Methode standardisieren
                Dim httpMethod As New HttpMethod(method.ToUpperInvariant())

                ' Request-Nachricht vorbereiten
                Using request As New HttpRequestMessage(httpMethod, url)

                    ' Daten nur anhängen, wenn es sich um eine schreibende Methode handelt und Daten existieren
                    If httpMethod <> HttpMethod.Get AndAlso Not String.IsNullOrEmpty(data) Then
                        ' Inhalt kodieren und Content-Type auf x-www-form-urlencoded setzen
                        request.Content = New StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded")
                    End If

                    ' Anfrage synchron ausführen und auf Antwort warten (.Result)
                    Using response As HttpResponseMessage = client.SendAsync(request).Result

                        ' Prüfen, ob der Server mit einem Erfolgsstatus (z. B. 200 OK) geantwortet hat
                        If response.IsSuccessStatusCode Then
                            Return response.Content.ReadAsStringAsync().Result
                        Else
                            Return "ERROR"
                        End If

                    End Using
                End Using
            End Using

        Catch ex As Exception
            ' Im Fehlerfall (z. B. Netzwerk-Timeout) wird "ERROR" zurückgegeben
            Return "ERROR"
        End Try
    End Function

    'Public Shared Function PHPnoF(ByVal url As String, ByVal method As String, ByVal data As String)
    '    Try

    '        Dim request As System.Net.WebRequest = System.Net.WebRequest.Create(url)
    '        request.Method = method
    '        Dim postData = data
    '        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
    '        request.ContentType = "application/x-www-form-urlencoded"
    '        request.ContentLength = byteArray.Length
    '        Dim dataStream As Stream = request.GetRequestStream()
    '        dataStream.Write(byteArray, 0, byteArray.Length)
    '        dataStream.Close()
    '        Dim response As WebResponse = request.GetResponse()
    '        dataStream = response.GetResponseStream()
    '        Dim reader As New StreamReader(dataStream)
    '        Dim responseFromServer As String = reader.ReadToEnd()
    '        reader.Close()
    '        dataStream.Close()
    '        response.Close()
    '        Return (responseFromServer)
    '    Catch ex As Exception
    '        Return ("ERROR")
    '    End Try
    'End Function
    Public Shared Function PHPno(ByVal url As String, ByVal method As String, ByVal data As String)
        Try

            Dim request As System.Net.WebRequest = System.Net.WebRequest.Create(url)
            request.Method = method
            Dim postData = data
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            request.ContentType = "application/x-www-form-urlencoded"
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Return "true"
        Catch ex As Exception
        End Try
    End Function

    ''' <summary>
    ''' Zeitschluessel hhMMYYYYmmdd
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>03.02.2011 Create koh chang
    ''' </remarks>
    Public Shared Function GetTimePW() As String
        Dim sID As String = Mid(Date.Today, 7, 4) & Mid(Date.Today, 4, 2) & Mid(Date.Today, 1, 2)
        Dim sTime As String = TimeOfDay
        sTime = Mid(sTime.Replace(":", ""), 1, 4)
        GetTimePW = sTime & sID
    End Function
    Public Shared Function GetQuersumme(ByRef sZeile As String) As String
        Dim nl As Integer = 0
        Dim s As String = ""
        Dim nStart As Integer = sZeile.Length - 10
        If nStart < 1 Then nStart = 1
        For i = nStart To sZeile.Length
            nl = nl + Asc(Mid(sZeile, i, 1))
            s = s & Mid(sZeile, i, 1)
        Next
        Dim sl As String = Trim(Str(nl))
        sl = "000000" & sl
        sl = Mid(sl, sl.Length - 3, 2) & ";" & Mid(sl, sl.Length - 1, 2) & ";"
        GetQuersumme = sl
    End Function
    ''' <summary>
    ''' Erstellen eines Datatables
    ''' </summary>
    ''' <param name="sSQL  "></param>
    ''' <param name="sIP "></param>
    ''' <returns></returns>
    ''' <remarks>03.02.2011 Create koh chang
    ''' </remarks>

    Public Shared Function DataTable(ByRef sSQL As String, ByRef sIP As String) As DataTable
        prInfo("DataTable => " & sSQL)
        Dim sSql_si As String = sSQL
        Dim sSumme As String = GetQuersumme(sSQL)
        Dim sPW() As String = Split("Gabriela;Gewuenscht;Karriere;Elternzeit;Truemmer;Abstand;Bodensee", ";")
        Dim PW As String = DeCrypt(sPW(DateTime.Now.DayOfWeek), GetTimePW())

        Dim sTime As String = TimeOfDay
        ' sSQL = "S=" & Mid(sTime.Replace(":", ";"), 1, 6) & DeCryptASCII(sSQL, PW)
        sSQL = "S=" & Mid(sTime.Replace(":", ";"), 1, 6) & sSumme & DeCryptASCII(sSQL, PW)
        sIP = "http://" & sIP & "/SQLRead.php"
        Dim htmlcode As String = PHP(sIP, "POST", sSQL)

        Dim aZeile() As String = Split(htmlcode, Chr(94)) 'Zeilenende chr(94)
        Dim dt As New DataTable
        Dim aFeld() As String = Split(aZeile(1), Chr(126))  'spaltenende chr(95)
        For i = 0 To aFeld.Length - 1
            dt.Columns.Add(aFeld(i), System.Type.GetType("System.String"))
        Next
        For i = 2 To aZeile.Length - 2
            aFeld = Split(aZeile(i), Chr(126))

            dt.Rows.Add.Item(0) = aFeld(0)
            For j = 1 To aFeld.Length - 1
                If aFeld(j) = "" Then aFeld(j) = " "

                dt.Rows(i - 2).Item(j) = aFeld(j)
            Next
        Next
        prInfo("")
        Return dt
    End Function




    ''' <summary>
    ''' testen der Datenverbindung
    ''' </summary>
    ''' <param name="sSQL  "></param>
    ''' <param name="sIP "></param>
    ''' <returns></returns>
    ''' <remarks>03.02.2011 Create koh chang
    ''' </remarks>

    Public Shared Function DataTest(ByRef sSQL As String, ByRef sIP As String) As Boolean
        Dim sSumme As String = GetQuersumme(sSQL)
        Dim sPW() As String = Split("Gabriela;Gewuenscht;Karriere;Elternzeit;Truemmer;Abstand;Bodensee", ";")
        Dim PW As String = DeCrypt(sPW(DateTime.Now.DayOfWeek), GetTimePW())

        Dim sTime As String = TimeOfDay
        ' sSQL = "S=" & Mid(sTime.Replace(":", ";"), 1, 6) & DeCryptASCII(sSQL, PW)
        sSQL = "S=" & Mid(sTime.Replace(":", ";"), 1, 6) & sSumme & DeCryptASCII(sSQL, PW)
        sIP = "http://" & sIP & "/SQLRead.php"
        Dim htmlcode As String = PHP(sIP, "POST", sSQL)

        Dim aZeile() As String = Split(htmlcode, Chr(94)) 'Zeilenende chr(94)
        Dim dt As New DataTable
        If fcSeekString(htmlcode, "Warning") = True Then
            Return False
        Else
            Return True
        End If

    End Function
    ''' <summary>
    ''' Leersatz an Tabelle anhaengen, Ruckgabe ID 
    ''' </summary>
    ''' <param name="sTabelle  "></param>
    ''' <param name="sIP "></param>
    ''' <returns></returns>
    ''' <remarks>03.02.2011 Create koh chang
    ''' </remarks>
    Public Shared Function Append(ByRef sTabelle As String, ByRef sIP As String) As String
        Dim sSumme As String = GetQuersumme(sTabelle)
        Dim sPW() As String = Split("Rucksack;waehrend;zuHause;unterwegs;Berufung;Chickenbus;Speedboot", ";")
        Dim PW As String = DeCrypt(sPW(DateTime.Now.DayOfWeek), GetTimePW())
        Dim sTime As String = TimeOfDay
        sTabelle = "S=" & Mid(sTime.Replace(":", ";"), 1, 6) & sSumme & DeCryptASCII(sTabelle, PW)
        sIP = "http://" & sIP & "/SQLAppend.php"
        Dim sID As String = PHP(sIP, "POST", sTabelle)
        Dim aZeile() As String = Split(sID, Chr(94)) 'Zeilenende chr(94)
        If aZeile(1) = "False" Then
            Dim sMsg As String = "Fehler beim Anlegen des Datensatzes"
            MsgBox(sMsg, vbOKCancel, "OK")
            Return ""
        Else
            Return aZeile(1)
        End If

    End Function

    ''' <summary>
    ''' Führt das Update eines Datensatzes über eine verschlüsselte HTTP-POST-Anfrage an ein Remote-PHP-Skript aus.
    ''' </summary>
    ''' <param name="sSQL">Das auszuführende SQL-Statement (wird per Referenz manipuliert und verschlüsselt).</param>
    ''' <param name="sIP">Die Ziel-IP-Adresse oder Domain (wird zur vollständigen URL erweitert).</param>
    ''' <returns>True, wenn das Update auf dem Server erfolgreich war (Rückgabe "True"), andernfalls False.</returns>
    ''' <remarks>
    ''' 03.02.2011 - Create (Koh Chang)
    ''' 03.10.2026 - Code-Optimierung:
    ''' - Robustheit erhöht: Index-Prüfung bei 'aZeile.Length > 1' verhindert eine 'IndexOutOfRangeException', falls das PHP-Skript eine fehlerhafte oder leere Antwort liefert.
    ''' - Speicher-Effizienz: Unnötige Variable 'sSql_Si' entfernt.
    ''' - API-Modernisierung: Veraltete VB6-Funktionen ('TimeOfDay', 'Mid', 'Split', 'MsgBox') durch moderne .NET-Äquivalente ersetzt.
    ''' - Typsicherheit: 'DateTime.Now.DayOfWeek' liefert einen typsicheren Index für das Passwort-Array.
    ''' </remarks>
    Public Shared Function Update(ByRef sSQL As String, ByRef sIP As String) As Boolean
        prInfo("Update => " & sSQL)
        Dim isSuccess As Boolean = False

        ' Passwort-Array über native .NET-Syntax initialisieren
        Dim sPW() As String = {"Flipflop", "Begegnung", "neueLand", "sichimmer", "Zukunft", "zuSorgen", "sondern"}

        ' DayOfWeek liefert 0 (Sunday) bis 6 (Saturday). Array-Reihenfolge muss darauf abgestimmt sein.
        Dim currentDayIndex As Integer = CInt(DateTime.Now.DayOfWeek)
        Dim passwordKey As String = DeCrypt(sPW(currentDayIndex), GetTimePW())

        ' Aktuelle Uhrzeit ermitteln und Formatierung anpassen (.NET alternative zu TimeOfDay)
        Dim currentTimeString As String = DateTime.Now.ToString("HH;mm;ss")

        ' Die ersten 6 Zeichen der Zeitzeichenkette herausschneiden (.NET Substring statt Mid)
        Dim timePart As String = currentTimeString.Substring(0, Math.Min(6, currentTimeString.Length))
        Dim sSumme As String = GetQuersumme(sSQL)

        ' String-Verschlüsselung aufbauen
        sSQL = "S=" & timePart & sSumme & DeCryptASCII(sSQL, passwordKey)
        sIP = "http://" & sIP & "/SQLUpdate.php"

        ' POST-Request an den Webserver senden
        Dim htmlcode As String = PHP(sIP, "POST", sSQL)

        If Not String.IsNullOrEmpty(htmlcode) Then
            ' Splitten am Trennzeichen ^ (Chr(94)) mit der nativen .NET-Methode
            Dim aZeile() As String = htmlcode.Split("^"c)

            ' WICHTIG: Prüfen, ob das Array mindestens zwei Elemente enthält, um Abstürze zu verhindern
            If aZeile.Length > 1 AndAlso aZeile(1) = "True" Then
                isSuccess = True
            Else
                Const sMsg As String = " PHP-Update Fehler beim Speichern"
                MessageBox.Show(sMsg, "Fehler", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If
        Else
            MessageBox.Show("Keine Antwort vom Server erhalten.", "Verbindungsfehler", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        prInfo("")
        Return isSuccess
    End Function

    Public Shared Function SaveFtp(ByRef sIP As String) As Boolean
        Dim sSQL = fcGetTimeID(Date.Today)
        Dim sSumme As String = GetQuersumme(sSQL)
        Dim sPW() As String = Split("Fahrwasser;leuchttonne;mobtaste;Dresden;Wasserwerk;flughafen;flugzeug", ";")
        Dim PW As String = DeCrypt(sPW(DateTime.Now.DayOfWeek), GetTimePW())
        Dim sTime As String = TimeOfDay
        sSQL = "S=" & Mid(sTime.Replace(":", ";"), 1, 6) & sSumme & DeCryptASCII(sSQL, PW)
        sIP = "http://" & sIP & "/SQLSave.php"
        Dim htmlcode As String = Replace(PHP(sIP, "POST", sSQL), "~", vbCrLf)
    End Function
    Public Shared Function SaveRestore(ByRef sIP As String) As Array
        Dim sSQL = fcGetTimeID(Date.Today)
        Dim sSumme As String = GetQuersumme(sSQL)
        Dim sPW() As String = Split("Fahrwasser;leuchttonne;mobtaste;Dresden;Wasserwerk;flughafen;flugzeug", ";")
        Dim PW As String = DeCrypt(sPW(DateTime.Now.DayOfWeek), GetTimePW())
        Dim sTime As String = TimeOfDay
        sSQL = "S=" & Mid(sTime.Replace(":", ";"), 1, 6) & sSumme & DeCryptASCII(sSQL, PW)
        sIP = "http://" & sIP & "/SQLDir.php"
        Dim htmlcode As String = PHP(sIP, "POST", sSQL)
        SaveRestore = Split(htmlcode, Chr(94)) 'Zeilenende chr(94)
        SaveRestore = fcArraySort(SaveRestore, 1, SaveRestore.Length - 1, True)

    End Function
    Public Shared Function Restore(ByRef sIP As String, ByRef sDatei As String) As Boolean
        Restore = False
        Dim sSQL As String = sDatei
        Dim sSumme As String = GetQuersumme(sSQL)
        Dim sPW() As String = Split("Bootstour;Hinweise;Handbuch;nachhaltig;Highlights;Auflage;unterwegs", ";")
        Dim PW As String = DeCrypt(sPW(DateTime.Now.DayOfWeek), GetTimePW())
        Dim sTime As String = TimeOfDay
        sSQL = "S=" & Mid(sTime.Replace(":", ";"), 1, 6) & sSumme & DeCryptASCII(sSQL, PW)
        sIP = "http://" & sIP & "/SQLRestore.php"
        Dim htmlcode As String = PHP(sIP, "POST", sSQL)
        Dim sErgebnis() As String = Split(htmlcode, Chr(94)) 'Zeilenende chr(94)
        ' If sErgebnis(1) = "True" Then
        'Restore = True
        'End If
    End Function
    Public Shared Function Kunde(ByRef sIP As String, ByRef P As String) As Array
        Dim sSQL = fcGetTimeID(Date.Today) & P
        Dim sSumme As String = GetQuersumme(sSQL)
        Dim sPW() As String = Split("Fahrwasser;leuchttonne;mobtaste;Dresden;Wasserwerk;flughafen;flugzeug", ";")
        Dim PW As String = DeCrypt(sPW(DateTime.Now.DayOfWeek), GetTimePW())
        Dim sTime As String = TimeOfDay
        sSQL = "S=" & Mid(sTime.Replace(":", ";"), 1, 6) & sSumme & DeCryptASCII(sSQL, PW)
        sIP = "http://" & sIP & "/SQLKunde.php"
        Dim htmlcode As String = PHP(sIP, "POST", sSQL)
        Kunde = Split(htmlcode, Chr(13) & Chr(10)) 'Zeilenende chr(94)
        '  SaveRestore = fcArraySort(SaveRestore, 1, SaveRestore.Length - 1, True)

    End Function
    Public Shared Function KunTOBuc(ByRef sIP As String) As Boolean
        Dim sSQL = fcGetTimeID(Date.Today)
        KunTOBuc = False
        Dim sSumme As String = GetQuersumme(sSQL)
        Dim sPW() As String = Split("Fahrwasser;leuchttonne;mobtaste;Dresden;Wasserwerk;flughafen;flugzeug", ";")
        Dim PW As String = DeCrypt(sPW(DateTime.Now.DayOfWeek), GetTimePW())
        Dim sTime As String = TimeOfDay
        sSQL = "S=" & Mid(sTime.Replace(":", ";"), 1, 6) & sSumme & DeCryptASCII(sSQL, PW)
        sIP = "http://" & sIP & "/SQLKunID.php"
        Dim htmlcode As String = PHP(sIP, "POST", sSQL)
        Dim sErgebnis() As String = Split(htmlcode, Chr(94)) 'Zeilenende chr(94)
        If sErgebnis(1) = "True" Then
            KunTOBuc = True
        End If
    End Function
    Public Shared Function SQLError(ByRef sIP As String, ByRef P As String) As String
        Dim sSQL = fcGetTimeID(Date.Today) & P
        Dim sSumme As String = GetQuersumme(sSQL)
        Dim sPW() As String = Split("Fahrwasser;leuchttonne;mobtaste;Dresden;Wasserwerk;flughafen;flugzeug", ";")
        Dim PW As String = DeCrypt(sPW(DateTime.Now.DayOfWeek), GetTimePW())
        Dim sTime As String = TimeOfDay
        sSQL = "S=" & Mid(sTime.Replace(":", ";"), 1, 6) & sSumme & DeCryptASCII(sSQL, PW)
        sIP = "http://" & sIP & "/SQLError.php"
        Dim htmlcode As String = PHP(sIP, "POST", sSQL)
        SQLError = htmlcode
        '  SaveRestore = fcArraySort(SaveRestore, 1, SaveRestore.Length - 1, True)

    End Function
    Public Shared Function SQLErrorDel(ByRef sIP As String, ByRef P As String) As Boolean
        Dim sSQL = fcGetTimeID(Date.Today) & P
        Dim sSumme As String = GetQuersumme(sSQL)
        Dim sPW() As String = Split("Fahrwasser;leuchttonne;mobtaste;Dresden;Wasserwerk;flughafen;flugzeug", ";")
        Dim PW As String = DeCrypt(sPW(DateTime.Now.DayOfWeek), GetTimePW())
        Dim sTime As String = TimeOfDay
        sSQL = "S=" & Mid(sTime.Replace(":", ";"), 1, 6) & sSumme & DeCryptASCII(sSQL, PW)
        sIP = "http://" & sIP & "/SQLErrorDel.php"
        Dim htmlcode As String = PHP(sIP, "POST", sSQL)
        SQLErrorDel = True
        '  SaveRestore = fcArraySort(SaveRestore, 1, SaveRestore.Length - 1, True)

    End Function
    Public Shared Function Data(ByRef sSQL As String, ByRef sIP1 As String) As Boolean
        Data = False
        ' Dim sPW() As String = Split("Flipflop;Begegnung;neueLand;sichimmer;Zukunft;zuSorgen;sondern", ";")
        ' Dim PW As String = DeCrypt(sPW(DateTime.Now.DayOfWeek), GetTimePW(Date.Today))

        '  Dim sTime As String = TimeOfDay
        '   sSQL = "S=" & Mid(sTime.Replace(":", ""), 1, 4) & DeCryptASCII(sSQL, PW)
        sSQL = "Daten=" & sSQL
        '  Dim sIPx As String = "http://" & sIP1
        Dim sIPx As String = "http://" & sIP1 & "/SQLSend.php"
        If PHPnoF(sIPx, "POST", sSQL) <> "ERROR" Then 'nof
            Data = True
        End If



    End Function
    Public Shared Function DataAll(ByRef sIP1 As String) As Boolean
        DataAll = False

        '  Dim sIPx As String = "http://" & sIP1
        Dim sIPx As String = "http://" & sIP1 & "/SQLTrans.php"
        If PHPnoF(sIPx, "POST", "Daten= ") <> "ERROR" Then 'nof
            DataAll = True
        End If



    End Function

    Public Shared Function DataRead(ByRef sIP1 As String, ByRef Wert As String) As String
        DataRead = False

        '  Dim sIPx As String = "http://" & sIP1
        Dim sIPx As String = "http://" & sIP1 & "/SQLReadCode.php"
        sIPx = sIP1

        DataRead = PHP(sIP1, "POST", "Daten=" & Wert)
        'If PHPnoF(sIPx, "POST", "Daten= ") <> "ERROR" Then 'nof
        '    DataRead = True
        'End If



    End Function

    ' Nicht löschen!!!
    '# Diese Dateien liegen auf dem Raspberry und werden von VB aus aufgerufen. Die PHP-Dateien sind für die Kommunikation mit der Datenbank zuständig.
    'PHP Dateine
    ' <?php
    ' /* SQLRead.php list eine datenbank aus und giebt diese über web an VB aus*/

    '	date_default_timezone_set("Europe/Berlin");
    'include 'function.php';
    '$PW=array("Gabriela","Gewuenscht","Karriere","Elternzeit","Truemmer","Abstand","Bodensee");
    '$timestamp = time();


    '$Keys=array_keys($_POST);
    '$SQL=$_POST['S'];
    '$sZeit=substr($SQL,0,4);
    '$SQL=substr($SQL,4);
    '$PWX =fDeCryptSQL( $PW[date("w")],$sZeit.date("Ymd", $timestamp));
    '$SQL=fDeCryptPara($SQL,$PWX);

    '	$con = fcCon(); //"", "root", "", "Pension"); 
    '	$dt = mysqli_query($con,$SQL); 
    '	$nField=mysqli_num_fields($dt);
    '    $Text="";
    '	$i=0;
    '	echo chr(94);
    '	while ($finfo = mysqli_fetch_field($dt))  //ermiteln der Feldnamen
    '	{
    '		$aField[$i]=$finfo->name;
    '		$Text=$Text.$aField[$i].chr(126);
    '		$i++;
    '	}
    '	$Text=substr($Text,0,strlen($Text)-1);
    '	echo $Text.chr(94);

    '	While ($dSatz =mysqli_fetch_assoc($dt))    //inhalt ausgeben
    '	{
    '		$Text="";
    '		for($j=0;$j<=$nField-1;$j++)
    '		{
    '			$Text=$Text.$dSatz[$aField[$j]].chr(126);
    '		}
    '		$Text=substr($Text,0,strlen($Text)-1);
    '		echo $Text.chr(94);	
    '	}
    '	mysqli_close($con); 
    '?>

    '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    '    <?php
    ' /* SQLAppend.php neuen datensatz anlegen ID über  web an VB aus*/
    ' include 'function.php';
    ' date_default_timezone_set("Europe/Berlin");
    '$PW=array("Rucksack","waehrend","zuHause","unterwegs","Berufung","Chickenbus","Speedboot");
    '$timestamp = time(); 

    '$Keys=array_keys($_POST);
    '$SQL=$_POST['S'];
    '$sZeit=substr($SQL,0,4);
    '$SQL=substr($SQL,4);
    '$PWX =fDeCryptSQL( $PW[date("w")],$sZeit.date("Ymd", $timestamp));
    '$Tabelle=fDeCryptPara($SQL,$PWX);


    '	$con = fcCon(); //"", "root", "", "Pension"); 
    '	echo chr(94);
    '	$SQL="SELECT * FROM ".$Tabelle;
    '	$dt = mysqli_query($con,$SQL); 
    '	$nField=mysqli_num_fields($dt);
    '	$i=0;
    '	while ($finfo = mysqli_fetch_field($dt))  //ermiteln der Feldnamen
    '	{
    '		$aField[$i]=$finfo->name;
    '		$i++;
    '	}
    '	$ID=strftime("%Y%m%d%H%M%S",time()); //JJMMTThhmmss
    '	$Value="";
    '	$sFeld="";
    '	for($i=0; $i <= count($aField)-1; $i++)   
    '	{
    '		$sFeld= $sFeld.$aField[$i].",";
    '		$sWert="0";
    '		if ($aField[$i]=="ID")
    '		{
    '			$sWert=$ID;
    '		}	
    '		$Value=$Value."'".$sWert."',";
    '	}
    '	$sFeld=substr($sFeld,0,strlen($sFeld)-1);
    '	$Value=substr($Value,0,strlen($Value)-1);
    '	$SQL="INSERT INTO ".$Tabelle." (".$sFeld.") value (".$Value.")";
    '	mysqli_query($con,$SQL);
    '	mysqli_close($con);
    '	echo $ID.chr(94);

    '?>

    '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    '    <?php
    ' /* SQLUpdate.php Schreiben in ein datensatzes*/
    '	date_default_timezone_set("Europe/Berlin");
    'include 'function.php';
    '$PW=array("Flipflop","Begegnung","neueLand","sichimmer","Zukunft","zuSorgen","sondern");
    '$timestamp = time();


    '$Keys=array_keys($_POST);
    '$SQL=$_POST['S'];
    '$sZeit=substr($SQL,0,4);
    '$SQL=substr($SQL,4);
    '$PWX =fDeCryptSQL( $PW[date("w")],$sZeit.date("Ymd", $timestamp));
    '$SQL=fDeCryptPara($SQL,$PWX);

    '	$con = fcCon(); //"", "root", "", "Pension"); 
    '	mysqli_query($con,$SQL); 
    '	echo chr(94)."True".chr(94);
    '	mysqli_close($con); 

    '?>
    '
    '************************* php functionen ******************************************

    '  	function fcCon()    //$ID
    '{
    '	$db= mysqli_connect('127.0.0.1','Reich','02041883-Pi','pension');
    '	mysqli_set_charset($db,'utf8');
    '	//mysqli_set_charset('utf8');
    '	If (!$db)
    '	{
    '		echo "verbindungsfehler".mysqli_connect_error();	
    '	}
    '	return $db;
    '}
    '   Function fDeCryptSQL($sString ,$sSecurityValue)
    '   {
    '	$nMaxs=strlen($sSecurityValue);
    '	$nMax=strlen($sString)-1;
    '	$sCrypt="";	
    '	for ($n=0;$n <= $nMax; $n++)
    '	{
    '		$x=($n+1)%$nMaxs;
    '		if ($x==0)
    '		{
    '			$x=$nMaxs;
    '		}
    '		$x=$x-1;
    '		$Z=ord(substr($sString,$n,1));
    '		if ($Z==129)
    '		{
    '			$Z=61;
    '		}
    '		$sCrypt=$sCrypt.chr(ord(substr($sSecurityValue,$x,1)) ^ $Z);
    '	}
    '	return $sCrypt;
    '   }
    'Function fDeCryptPara($sString ,$sSecurityValue)
    '   {
    '	$nMaxs=strlen($sSecurityValue);
    '	$Zeichen = explode(";", $sString);
    '	$nMax=count($Zeichen)-1;
    '	$sCrypt="";	
    '	for ($n=0;$n <= $nMax; $n++)
    '	{
    '		$x=($n+1)%$nMaxs;
    '		if ($x==0)
    '		{
    '			$x=$nMaxs;
    '		}
    '		$x=$x-1;

    '		$sCrypt=$sCrypt.chr(ord(substr($sSecurityValue,$x,1)) ^ intval($Zeichen[$n]));

    '	}
    '	return $sCrypt;
    '   }

    '    function fcUmDatum($sDatum)
    '{
    '	$Datum = trim($sDatum);
    '	if ($sDatum == "")
    '	{
    '		return " ";
    '	}
    '	if (strpos($sDatum,".") === FALSE )  /* '(20080101 > 01.09.2008)*/
    '	{
    '		return  substr($sDatum, 6, 2). "." .substr($sDatum, 4, 2)."." .substr($sDatum, 0, 4);
    '	}
    '	else  /*(01.09.2008 > 20080101)*/
    '	{ 
    '		return substr($sDatum, 6, 4).  substr($sDatum, 3, 2). substr($sDatum, 0, 2);
    '	}
    '}

    '   Function fcGetTimeID()
    '{  
    '	return  strftime("%Y%m%d%H%M%S",time());
    '   }
End Class
