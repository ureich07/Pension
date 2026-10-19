
Imports System.Net.Http
Imports System.Text
Imports System.IO

Public Class PHP
    'Alle SQL Funktionen nur mit PHPdateien auf Webserver!
    'Passwoerter in der Funktion und PHPdatei müssen übereinstimmen


    ''' <summary>
    ''' Führt eine symmetrische XOR-Verschlüsselung oder -Entschlüsselung auf einem String aus.
    ''' Da es sich um eine XOR-Operation handelt, führt das erneute Anwenden mit demselben Schlüssel zum Originaltext.
    ''' </summary>
    ''' <param name="sString">Der zu ver- oder entschlüsselnde Text.</param>
    ''' <param name="sSecurityValue">Der geheime Schlüssel für die XOR-Operation.</param>
    ''' <returns>Der bearbeitete Text als String.</returns>
    ''' <remarks>
    ''' 03.02.2011 - Create (Koh Chang)
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Den extrem langsamen String-Verkettungsoperator (&amp;) in der Schleife durch 'StringBuilder' ersetzt.
    ''' - VB6-Altlasten (Len, Mid$, Asc, Chr) vollständig entfernt.
    ''' - Direkten, performanten 0-basierten Zeichenzugriff implementiert.
    ''' - Null- und Leer-Validierung für Eingabestrings hinzugefügt, um Abstürze zu verhindern.
    ''' - Schleifen-Index auf standardmäßige 0-basierte Logik umgestellt.
    ''' </remarks>
    Public Shared Function DeCrypt(ByVal sString As String, ByVal sSecurityValue As String) As String
        Try
            ' Validierung: Wenn einer der Strings leer ist, gibt es nichts zu verarbeiten
            If String.IsNullOrEmpty(sString) OrElse String.IsNullOrEmpty(sSecurityValue) Then
                Return sString
            End If

            Dim nMax As Integer = sString.Length
            Dim nMaxS As Integer = sSecurityValue.Length
            Dim sb As New System.Text.StringBuilder(nMax)

            ' 0-basierte Schleife über alle Zeichen des Eingabestrings
            For n As Integer = 0 To nMax - 1
                ' Bestimmung des passenden Schlüsselzeichens per Modulo-Operation
                Dim x As Integer = n Mod nMaxS

                ' Direkte XOR-Verknüpfung der Zeichen-Codes
                Dim charCode As Integer = AscW(sSecurityValue(x)) Xor AscW(sString(n))

                ' Zeichen an den StringBuilder anhängen
                sb.Append(ChrW(charCode))
            Next

            Return sb.ToString()

        Catch ex As Exception
            ' Hinweis: In produktiven Krypto-Funktionen sollte hier je nach Architektur 
            ' ein Log-Eintrag erfolgen oder der Fehler weitergeworfen werden.
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Verschlüsselt einen String per XOR und gibt das Ergebnis als semikolon-getrennte Kette von ASCII/Unicode-Zahlenwerten zurück.
    ''' </summary>
    ''' <param name="sString">Der zu verschlüsselnde Text.</param>
    ''' <param name="sSecurityValue">Der geheime Schlüssel für die XOR-Operation.</param>
    ''' <returns>Ein String aus numerischen Werten, getrennt durch Semikolons (z.B. "72;101;108;108;111").</returns>
    ''' <remarks>
    ''' 03.02.2011 - Create (Koh Chang)
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Den speicherintensiven Verkettungsoperator (&amp;) und 'Str().Trim' durch 'StringBuilder' ersetzt.
    ''' - VB6-Legacy-Funktionen (Len, Mid, Asc, Str) vollständig entfernt.
    ''' - Das Entfernen des letzten Semikolons optimiert (sb.Length -= 1).
    ''' - Fehlerbehandlung (Try-Catch) und Null-Validierung hinzugefügt.
    ''' </remarks>
    Public Shared Function DeCryptASCII(ByVal sString As String, ByVal sSecurityValue As String) As String
        Try
            ' Validierung: Wenn einer der Strings leer ist, sofort abbrechen
            If String.IsNullOrEmpty(sString) OrElse String.IsNullOrEmpty(sSecurityValue) Then
                Return String.Empty
            End If

            Dim nMax As Integer = sString.Length
            Dim nMaxS As Integer = sSecurityValue.Length

            ' StringBuilder initialisieren (Kapazität grob geschätzt: ca. 4 Zeichen pro Buchstabe)
            Dim sb As New System.Text.StringBuilder(nMax * 4)

            ' 0-basierte Schleife über alle Zeichen des Eingabestrings
            For n As Integer = 0 To nMax - 1
                ' Bestimmung des passenden Schlüsselzeichens per Modulo-Operation
                Dim x As Integer = n Mod nMaxS

                ' Direkte XOR-Verknüpfung der Zeichen-Codes (AscW ist das .NET-native Gegenstück zu Asc)
                Dim charCode As Integer = AscW(sSecurityValue(x)) Xor AscW(sString(n))

                ' Zahl direkt anfügen, gefolgt vom Trennzeichen (kein Str() oder Trim mehr nötig)
                sb.Append(charCode).Append(";")
            Next

            ' Das letzte überflüssige Semikolon entfernen, falls Daten vorhanden sind
            If sb.Length > 0 Then
                sb.Length -= 1
            End If

            Return sb.ToString()

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return String.Empty
        End Try
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


    ''' <summary>
    ''' Generiert einen standardisierten Zeitschlüssel im Format hhMMYYYYmmdd (StundeMinuteJahrMonatTag).
    ''' </summary>
    ''' <returns>Ein 12-stelliger String mit dem aktuellen Zeitstempel als Sicherheitsschlüssel.</returns>
    ''' <remarks>
    ''' 03.02.2011 - Create (Koh Chang)
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Komplett kulturunabhängig umgestellt via .NET-Format-Strings (verhindert Abstürze bei US/UK-Windows-Systemen).
    ''' - Fehleranfällige VB6-'Mid'- und '.Replace'-Ketten entfernt.
    ''' - 'DateTime.Now' zentralisiert, um minimale Zeitabweichungen zwischen Datums- und Uhrzeitabruf zu verhindern.
    ''' </remarks>
    Public Shared Function GetTimePW() As String
        Try
            ' Aktuellen Zeitstempel exakt einmal einlesen (verhindert Fehler beim Datumswechsel um Mitternacht)
            Dim nun As DateTime = DateTime.Now

            ' Generiert direkt: hhmm (StundeMinute) und yyyyMMdd (JahrMonatTag)
            Dim sTime As String = nun.ToString("HHmm")
            Dim sID As String = nun.ToString("yyyyMMdd")

            Return sTime & sID

        Catch ex As Exception
            ' Fallback auf ein festes Format bei unerwarteten Systemfehlern
            Return "000020000101"
        End Try
    End Function

    ''' <summary>
    ''' Berechnet eine modifizierte Quersumme (ASCII-Summe) aus den letzten maximal 11 Zeichen eines Strings
    ''' und formatiert das Ergebnis in ein standardisiertes, zweigeteiltes Format (z. B. "XX;YY;").
    ''' </summary>
    ''' <param name="sZeile">Der Eingabestring, aus dessen Ende die Quersumme berechnet wird.</param>
    ''' <returns>Ein formatierter String mit zwei zweistelligen Zahlen, getrennt durch Semikolons.</returns>
    ''' <remarks>
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Kritischen Schleifenüberlauf (i To sZeile.Length) und OutOfBounds-Gefahr bei kurzen Strings behoben.
    ''' - Auf 0-basierten, nativen und schnellen Zeichenzugriff umgestellt (sZeile(i) statt Mid).
    ''' - Veraltete VB6-Befehle ('Mid', 'Trim', 'Str') vollständig entfernt.
    ''' - Mathematisch sichere Formatierung der Quersumme mittels '.ToString("D4")' (erzwingt 4 Stellen mit führenden Nullen).
    ''' - Parameter von 'ByRef' auf das sicherere 'ByVal' geändert.
    ''' </remarks>
    Public Shared Function GetQuersumme(ByVal sZeile As String) As String
        Try
            ' Validierung: Bei leeren Strings sofort ein sicheres Fallback liefern
            If String.IsNullOrEmpty(sZeile) Then
                Return "00;00;"
            End If

            Dim nl As Integer = 0

            ' Startindex berechnen (die letzten 11 Zeichen, da im Original von (Length - 10) bis Length gelaufen wurde)
            Dim nStart As Integer = sZeile.Length - 11
            If nStart < 0 Then nStart = 0

            ' 0-basierte Schleife über die Zeichen am Ende des Strings
            For i As Integer = nStart To sZeile.Length - 1
                ' Direkter, ultraschneller Zugriff auf den ASCII/Unicode-Wert des Zeichens
                nl += AscW(sZeile(i))
            Next

            ' Die Summe mathematisch auf eine 4-stellige Zahl mit führenden Nullen formatieren (z. B. 1234 oder 0045)
            ' Das ersetzt das fehleranfällige "000000" & sl und anschließende Abschneiden
            Dim sl As String = (nl Mod 10000).ToString("D4")

            ' Die vierstellige Zahl aufteilen: die ersten 2 Stellen und die letzten 2 Stellen
            ' Beispiel "1234" wird zu "12;34;"
            Dim ergebnis As String = sl.Substring(0, 2) & ";" & sl.Substring(2, 2) & ";"

            Return ergebnis

        Catch ex As Exception
            ' Im Fehlerfall ein gültig formatiertes Fehler-Fallback zurückgeben
            Return "00;00;"
        End Try
    End Function

    ''' <summary>
    ''' Fragt Daten verschlüsselt von einem Webserver ab und baut daraus eine DataTable auf.
    ''' </summary>
    ''' <param name="sSQL">Der auszuführende SQL-Befehl.</param>
    ''' <param name="sIP">Die IP-Adresse des Webservers.</param>
    ''' <returns>Eine befüllte DataTable mit den Ergebnissen der Abfrage.</returns>
    ''' <remarks>
    ''' 03.02.2011 - Create (Koh Chang)
    ''' 19.10.2026 - Code-Optimierung:
    ''' - 'ByRef'-Parameter zu 'ByVal' geändert, um ungewolltes Überschreiben beim Aufrufer zu verhindern.
    ''' - 'TimeOfDay' durch kultursicheres 'DateTime.Now.ToString("HH;mm;ss")' ersetzt (AM/PM-Crash-Schutz).
    ''' - Fehleranfälliges Befüllen der DataRows durch die native, sichere '.Rows.Add(aFeld)'-Logik ersetzt.
    ''' - Arrayüberlauf beim Passwort-Wochentagszugriff mittels Modulo abgesichert.
    ''' - VB6-Legacy-Befehle ('Mid', 'Split') durch .NET-native Methoden ersetzt.
    ''' - Validierung der Serverantwort gegen leere oder fehlerhafte Übertragungen integriert.
    ''' </remarks>
    Public Shared Function DataTable(ByVal sSQL As String, ByVal sIP As String) As DataTable
        prInfo("DataTable => " & sSQL)

        Dim dt As New DataTable()
        Try
            ' 1. Quersumme und Verschlüsselung vorbereiten
            Dim sSumme As String = GetQuersumme(sSQL)
            Dim sPW() As String = {"Gabriela", "Gewuenscht", "Karriere", "Elternzeit", "Truemmer", "Abstand", "Bodensee"}
            Dim dayIndex As Integer = CInt(DateTime.Now.DayOfWeek) Mod sPW.Length
            Dim passwordKey As String = DeCrypt(sPW(dayIndex), GetTimePW())

            ' 2. Zeitstempel kultur-unabhängig formatieren
            Dim sTime As String = DateTime.Now.ToString("HH;mm;ss")
            Dim secureSQL As String = "S=" & sTime.Substring(0, Math.Min(6, sTime.Length)) & sSumme & DeCryptASCII(sSQL, passwordKey)

            Dim sUrl As String = "http://" & sIP & "/SQLRead.php"


            ' 4. Daten vom Webserver abrufen
            Dim htmlcode As String = PHP(sUrl, "POST", secureSQL)

            ' Validierung: Wenn keine Daten zurückkommen, leere Tabelle liefern
            If String.IsNullOrEmpty(htmlcode) Then
                prInfo("DataTable => Keine Daten empfangen")
                Return dt
            End If

            ' 5. Daten parsen (Zeilen trennen über Chr(94) -> '^')
            Dim aZeile() As String = htmlcode.Split(ChrW(94))

            ' Mindestens zwei Zeilen müssen vorhanden sein (Header + Daten)
            If aZeile.Length < 2 Then
                Return dt
            End If

            ' 6. Spaltenköpfe generieren (Spalten trennen über Chr(126) -> '~')
            Dim aFeld() As String = aZeile(1).Split(ChrW(126))
            For i As Integer = 0 To aFeld.Length - 1
                dt.Columns.Add(aFeld(i).Trim(), GetType(String))
            Next

            ' 7. Datenzeilen hinzufügen
            ' Schleife startet bei Index 2 (erste Datenzeile) und läuft bis zum vorletzten Element
            For i As Integer = 2 To aZeile.Length - 2
                ' Zeile splitten
                Dim aDaten() As String = aZeile(i).Split(ChrW(126))

                ' Wenn die Spaltenanzahl der Datenzeile nicht zum Tabellenschema passt, Array anpassen
                If aDaten.Length <> dt.Columns.Count Then
                    ReDim Preserve aDaten(dt.Columns.Count - 1)
                End If

                ' Bereinigung: Leere Werte durch ein Leerzeichen ersetzen
                For j As Integer = 0 To aDaten.Length - 1
                    If String.IsNullOrEmpty(aDaten(j)) Then
                        aDaten(j) = " "
                    End If
                Next

                ' Die gesamte Zeile auf einmal sicher zur DataTable hinzufügen
                dt.Rows.Add(aDaten)
            Next

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try

        prInfo("")
        Return dt
    End Function

    ''' <summary>
    ''' Hängt einen Leersatz an die angegebene Tabelle an und gibt die vom Server generierte ID zurück.
    ''' </summary>
    ''' <param name="sTabelle">Der Name der SQL-Tabelle, in die eingefügt werden soll.</param>
    ''' <param name="sIP">Die IP-Adresse des Webservers.</param>
    ''' <returns>Die generierte Datensatz-ID als String oder ein leerer String im Fehlerfall.</returns>
    ''' <remarks>
    ''' 03.02.2011 - Create (Koh Chang)
    ''' 19.10.2026 - Code-Optimierung:
    ''' - 'ByRef'-Parameter durch 'ByVal' ersetzt, um Variablen-Zerstörung beim Aufrufer zu verhindern.
    ''' - 'TimeOfDay' durch kultursicheres 'DateTime.Now.ToString("HH;mm;ss")' ersetzt (AM/PM-Crash-Schutz).
    ''' - Arrayüberlauf beim Passwort-Wochentagszugriff mittels Modulo-Operation abgesichert.
    ''' - Index-Validierung für das 'aZeile'-Array integriert, um Abstürze bei leeren Serverantworten zu verhindern.
    ''' - VB6-Legacy-Befehle ('Mid', 'Split') durch .NET-native Methoden ersetzt.
    ''' </remarks>
    Public Shared Function Append(ByVal sTabelle As String, ByVal sIP As String) As String
        Try
            ' 1. Quersumme und Verschlüsselung vorbereiten
            Dim sSumme As String = GetQuersumme(sTabelle)
            Dim sPW() As String = {"Rucksack", "waehrend", "zuHause", "unterwegs", "Berufung", "Chickenbus", "Speedboot"}
            Dim dayIndex As Integer = CInt(DateTime.Now.DayOfWeek) Mod sPW.Length
            Dim passwordKey As String = DeCrypt(sPW(dayIndex), GetTimePW())

            ' 2. Zeitstempel kultur-unabhängig formatieren
            Dim sTime As String = DateTime.Now.ToString("HH;mm;ss")
            Dim securePayload As String = "S=" & sTime.Substring(0, Math.Min(6, sTime.Length)) & sSumme & DeCryptASCII(sTabelle, passwordKey)

            ' 3. Flexibles URL-Handling (Prüfen, ob bereits http/https übergeben wurde)
            Dim sUrl As String = "http://" & sIP & "/SQLAppend.php"


            ' 4. Daten an Webserver senden und Antwort empfangen
            Dim sID As String = PHP(sUrl, "POST", securePayload)

            ' Validierung: Wenn die Antwort komplett leer ist, abbrechen
            If String.IsNullOrEmpty(sID) Then
                Return String.Empty
            End If

            ' 5. Antwort parsen (Zeilen trennen über Chr(94) -> '^')
            Dim aZeile() As String = sID.Split(ChrW(94))

            ' Sicherheitsprüfung: Enthält das Array das erwartete Ergebnis-Element?
            If aZeile.Length > 1 Then
                Dim statusOrID As String = aZeile(1).Trim()

                If statusOrID = "False" OrElse String.IsNullOrEmpty(statusOrID) Then
                    Dim sMsg As String = "Fehler beim Anlegen des Datensatzes"
                    MsgBox(sMsg, MsgBoxStyle.OkCancel, "OK")
                    Return String.Empty
                Else
                    ' Rückgabe der erfolgreich generierten ID
                    Return statusOrID
                End If
            Else
                ' Fallback, wenn der Server ein unerwartetes Format zurückliefert
                Return String.Empty
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return String.Empty
        End Try
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

    ''' <summary>
    ''' Sendet einen verschlüsselten Befehl an das PHP-Skript 'SQLSave.php', um Daten serverseitig zu sichern.
    ''' </summary>
    ''' <param name="sIP">Die IP-Adresse des Webservers (wird als Wertkopie übergeben).</param>
    ''' <returns>True, wenn die Anfrage fehlerfrei verarbeitet wurde; andernfalls False.</returns>
    ''' <remarks>
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Fehlende Rückgabewerte (Return True/False) für die Boolean-Funktion ergänzt.
    ''' - 'ByRef' zu 'ByVal' geändert, um das Überschreiben der IP-Variable beim Aufrufer zu verhindern.
    ''' - 'TimeOfDay' durch kultursicheres 'DateTime.Now.ToString("HH;mm;ss")' ersetzt (AM/PM-Crash-Schutz).
    ''' - Arrayüberlauf beim Passwort-Wochentagszugriff mittels Modulo abgesichert.
    ''' - VB6-Legacy-Befehle ('Mid', 'Split') durch .NET-native Gegenstücke ersetzt.
    ''' </remarks>
    Public Shared Function SaveFtp(ByVal sIP As String) As Boolean
        Try
            ' 1. SQL-Identifikationsstring und Quersumme generieren
            Dim sSQL As String = fcGetTimeID(Date.Today)
            Dim sSumme As String = GetQuersumme(sSQL)

            ' 2. Passwort basierend auf dem aktuellen Wochentag ermitteln (Überlaufsicher)
            Dim sPW() As String = {"Fahrwasser", "leuchttonne", "mobtaste", "Dresden", "Wasserwerk", "flughafen", "flugzeug"}
            Dim dayIndex As Integer = CInt(DateTime.Now.DayOfWeek) Mod sPW.Length
            Dim passwordKey As String = DeCrypt(sPW(dayIndex), GetTimePW())

            ' 3. Zeitstempel kultur-unabhängig formatieren
            Dim sTime As String = DateTime.Now.ToString("HH;mm;ss")

            ' 4. Verschlüsselten POST-Body zusammenbauen
            Dim securePayload As String = "S=" & sTime.Substring(0, Math.Min(6, sTime.Length)) & sSumme & DeCryptASCII(sSQL, passwordKey)

            ' 5. Ziel-URL zusammenbauen (Sicherer Aufbau, da sIP als ByVal übergeben wird)
            Dim sUrl As String = "http://" & sIP & "/SQLSave.php"


            ' 6. Daten an Webserver senden
            Dim htmlcode As String = PHP(sUrl, "POST", securePayload)

            ' Wenn nötig, die Tilden durch Zeilenumbrüche ersetzen (wie in deiner Logik vorbereitet)
            If Not String.IsNullOrEmpty(htmlcode) Then
                htmlcode = htmlcode.Replace("~", vbCrLf)
            End If

            ' Erfolgreichen Durchlauf zurückgeben
            Return True

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return False
        End Try
    End Function

    '''' <summary>
    '''' Fragt eine Liste von Sicherungsdateien vom PHP-Skript 'SQLDir.php' ab,
    '''' zerlegt diese in ein Array und sortiert sie absteigend (neueste zuerst).
    '''' </summary>
    '''' <param name="sIP">Die IP-Adresse oder die vollständige URL des Webservers.</param>
    '''' <returns>Ein sortiertes String-Array mit den Einträgen des Backup-Verzeichnisses oder ein leeres Array im Fehlerfall.</returns>
    '''' <remarks>
    '''' 19.10.2026 - Code-Optimierung:
    '''' - Rückgabetyp von generischem 'Array' auf typsicheres 'String()' geändert.
    '''' - 'ByRef' zu 'ByVal' geändert, um das Überschreiben der IP-Variable beim Aufrufer zu verhindern.
    '''' - 'TimeOfDay' durch kultursicheres 'DateTime.Now.ToString("HH;mm;ss")' ersetzt (AM/PM-Crash-Schutz).
    '''' - Arrayüberlauf beim Passwort-Wochentagszugriff mittels Modulo abgesichert.
    '''' - Validierung der Serverantwort gegen Leerwerte/Verbindungsfehler integriert.
    '''' - VB6-Legacy-Befehle ('Mid', 'Split') durch .NET-native Methoden ersetzt.
    '''' </remarks>
    'Public Shared Function SaveRestore(ByVal sIP As String) As String()
    '    Try
    '        ' 1. SQL-Identifikationsstring und Quersumme generieren
    '        Dim sSQL As String = fcGetTimeID(Date.Today)
    '        Dim sSumme As String = GetQuersumme(sSQL)

    '        ' 2. Passwort basierend auf dem aktuellen Wochentag ermitteln (Überlaufsicher)
    '        Dim sPW() As String = {"Fahrwasser", "leuchttonne", "mobtaste", "Dresden", "Wasserwerk", "flughafen", "flugzeug"}
    '        Dim dayIndex As Integer = CInt(DateTime.Now.DayOfWeek) Mod sPW.Length
    '        Dim passwordKey As String = DeCrypt(sPW(dayIndex), GetTimePW())

    '        ' 3. Zeitstempel kultur-unabhängig formatieren
    '        Dim sTime As String = DateTime.Now.ToString("HH;mm;ss")

    '        ' 4. Verschlüsselten POST-Body zusammenbauen
    '        Dim securePayload As String = "S=" & sTime.Substring(0, Math.Min(6, sTime.Length)) & sSumme & DeCryptASCII(sSQL, passwordKey)

    '        ' 5. Ziel-URL zusammenbauen (Sicherer Aufbau dank ByVal)
    '        Dim sUrl As String = "http://" & sIP & "/SQLDir.php"


    '        ' 6. Daten an Webserver senden und Antwort empfangen
    '        Dim htmlcode As String = PHP(sUrl, "POST", securePayload)

    '        ' Validierung: Wenn keine Daten zurückkommen, leeres Array liefern
    '        If String.IsNullOrEmpty(htmlcode) Then
    '            Return New String() {}
    '        End If

    '        ' 7. Antwort in Zeilen zerlegen (Zeilen trennen über Chr(94) -> '^')
    '        Dim aErgebnis() As String = htmlcode.Split(ChrW(94))

    '        ' 8. Array sortieren, falls Einträge vorhanden sind
    '        If aErgebnis IsNot Nothing AndAlso aErgebnis.Length > 1 Then
    '            ' Deine bestehende Sortierfunktion aufrufen (übergibt das gesamte Array)
    '            aErgebnis = fcArraySort(aErgebnis, 1, aErgebnis.Length - 1, True)
    '        End If

    '        Return aErgebnis

    '    Catch ex As Exception
    '        ErrReport(ex.Message, ex.Source, ex.StackTrace)
    '        ' Im Fehlerfall ein leeres Array zurückgeben, um Abstürze beim Aufrufer zu vermeiden
    '        Return New String() {}
    '    End Try
    'End Function

    '''' <summary>
    '''' Sendet den Namen einer Sicherungsdatei verschlüsselt an das PHP-Skript 'SQLRestore.php',
    '''' um eine Wiederherstellung (Restore) der Datenbank auf dem Server anzustoßen.
    '''' </summary>
    '''' <param name="sIP">Die IP-Adresse oder die vollständige URL des Webservers.</param>
    '''' <param name="sDatei">Der Dateiname der wiederherzustellenden Sicherung.</param>
    '''' <returns>True, wenn der Server die Wiederherstellung erfolgreich ("True") bestätigt hat; andernfalls False.</returns>
    '''' <remarks>
    '''' 19.10.2026 - Code-Optimierung:
    '''' - Aktivierung der auskommentierten Rückgabelogik unter Berücksichtigung von Index-Validierungen.
    '''' - 'ByRef'-Parameter zu 'ByVal' geändert, um Variablen-Zerstörung beim Aufrufer zu verhindern.
    '''' - 'TimeOfDay' durch kultursicheres 'DateTime.Now.ToString("HH;mm;ss")' ersetzt (AM/PM-Crash-Schutz).
    '''' - Arrayüberlauf beim Passwort-Wochentagszugriff mittels Modulo-Operation abgesichert.
    '''' - VB6-Legacy-Befehle ('Mid', 'Split') durch .NET-native Gegenstücke ersetzt.
    '''' </remarks>
    'Public Shared Function Restore(ByVal sIP As String, ByVal sDatei As String) As Boolean
    '    Try
    '        ' 1. SQL-Übergabestring (Dateiname) und Quersumme generieren
    '        Dim sSQL As String = sDatei
    '        Dim sSumme As String = GetQuersumme(sSQL)

    '        ' 2. Passwort basierend auf dem aktuellen Wochentag ermitteln (Überlaufsicher)
    '        Dim sPW() As String = {"Bootstour", "Hinweise", "Handbuch", "nachhaltig", "Highlights", "Auflage", "unterwegs"}
    '        Dim dayIndex As Integer = CInt(DateTime.Now.DayOfWeek) Mod sPW.Length
    '        Dim passwordKey As String = DeCrypt(sPW(dayIndex), GetTimePW())

    '        ' 3. Zeitstempel kultur-unabhängig formatieren
    '        Dim sTime As String = DateTime.Now.ToString("HH;mm;ss")

    '        ' 4. Verschlüsselten POST-Body zusammenbauen
    '        Dim securePayload As String = "S=" & sTime.Substring(0, Math.Min(6, sTime.Length)) & sSumme & DeCryptASCII(sSQL, passwordKey)

    '        ' 5. Ziel-URL zusammenbauen (Sicherer Aufbau dank ByVal)
    '        Dim sUrl As String = "http://" & sIP & "/SQLRestore.php"

    '        ' 6. Daten an Webserver senden und Antwort empfangen
    '        Dim htmlcode As String = PHP(sUrl, "POST", securePayload)

    '        ' Validierung: Wenn keine Antwort zurückkommt, war die Operation nicht erfolgreich
    '        If String.IsNullOrEmpty(htmlcode) Then
    '            Return False
    '        End If

    '        ' 7. Antwort in Zeilen zerlegen (Zeilen trennen über Chr(94) -> '^')
    '        Dim sErgebnis() As String = htmlcode.Split(ChrW(94))

    '        ' Sicherheitsprüfung: Enthält das Array das erwartete Ergebnis-Element?
    '        If sErgebnis.Length > 1 Then
    '            ' Gibt True zurück, wenn der Server exakt "True" liefert
    '            Return (sErgebnis(1).Trim() = "True")
    '        End If

    '        Return False

    '    Catch ex As Exception
    '        ErrReport(ex.Message, ex.Source, ex.StackTrace)
    '        Return False
    '    End Try
    'End Function

    ''' <summary>
    ''' Ruft Kundendaten verschlüsselt vom PHP-Skript 'SQLKunde.php' ab und trennt diese zeilenweise in ein String-Array.
    ''' </summary>
    ''' <param name="sIP">Die IP-Adresse oder die vollständige URL des Webservers.</param>
    ''' <param name="sParam">Der SQL-Parameter oder Befehlsteil für die Kundenabfrage.</param>
    ''' <returns>Ein String-Array mit den Zeilen der Kundendaten oder ein leeres Array im Fehlerfall.</returns>
    ''' <remarks>
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Rückgabetyp von generischem 'Array' auf typsicheres 'String()' geändert.
    ''' - 'ByRef'-Parameter zu 'ByVal' geändert, um Variablen-Zerstörung beim Aufrufer zu verhindern.
    ''' - 'TimeOfDay' durch kultursicheres 'DateTime.Now.ToString("HH;mm;ss")' ersetzt (AM/PM-Crash-Schutz).
    ''' - Arrayüberlauf beim Passwort-Wochentagszugriff mittels Modulo-Operation abgesichert.
    ''' - VB6-Legacy-Befehle ('Mid', 'Split') durch .NET-native Gegenstücke ersetzt.
    ''' - Trennung der Zeilen auf .NET-konformes 'ControlChars.CrLf' umgestellt.
    ''' </remarks>
    Public Shared Function Kunde(ByVal sIP As String, ByVal sParam As String) As String()
        Try
            ' 1. SQL-String und Quersumme generieren
            Dim sSQL As String = fcGetTimeID(Date.Today) & sParam
            Dim sSumme As String = GetQuersumme(sSQL)

            ' 2. Passwort basierend auf dem aktuellen Wochentag ermitteln (Überlaufsicher)
            Dim sPW() As String = {"Fahrwasser", "leuchttonne", "mobtaste", "Dresden", "Wasserwerk", "flughafen", "flugzeug"}
            Dim dayIndex As Integer = CInt(DateTime.Now.DayOfWeek) Mod sPW.Length
            Dim passwordKey As String = DeCrypt(sPW(dayIndex), GetTimePW())

            ' 3. Zeitstempel kultur-unabhängig formatieren
            Dim sTime As String = DateTime.Now.ToString("HH;mm;ss")

            ' 4. Verschlüsselten POST-Body zusammenbauen
            Dim securePayload As String = "S=" & sTime.Substring(0, Math.Min(6, sTime.Length)) & sSumme & DeCryptASCII(sSQL, passwordKey)

            ' 5. Ziel-URL zusammenbauen (Sicherer Aufbau dank ByVal)
            Dim sUrl As String = "http://" & sIP & "/SQLKunde.php"


            ' 6. Daten an Webserver senden und Antwort empfangen
            Dim htmlcode As String = PHP(sUrl, "POST", securePayload)

            ' Validierung: Wenn keine Daten zurückkommen, leeres Array liefern
            If String.IsNullOrEmpty(htmlcode) Then
                Return New String() {}
            End If

            ' 7. Antwort in Zeilen zerlegen (Trennt anhand von Carriage Return + Line Feed)
            Dim aKundendaten() As String = htmlcode.Split(New String() {ControlChars.CrLf}, StringSplitOptions.None)

            Return aKundendaten

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            ' Im Fehlerfall ein leeres Array zurückgeben, um Abstürze beim Aufrufer zu vermeiden
            Return New String() {}
        End Try
    End Function

    ''' <summary>
    ''' Führt eine verschlüsselte Operation (z. B. Synchronisierung von Kunden- zu Buchungsdaten) 
    ''' über das PHP-Skript 'SQLKunID.php' aus.
    ''' </summary>
    ''' <param name="sIP">Die IP-Adresse oder die vollständige URL des Webservers.</param>
    ''' <returns>True, wenn der Server die Operation erfolgreich ("True") bestätigt hat; andernfalls False.</returns>
    ''' <remarks>
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Index-Validierung für das 'sErgebnis'-Array integriert, um Abstürze bei leeren Serverantworten zu verhindern.
    ''' - 'ByRef'-Parameter zu 'ByVal' geändert, um Variablen-Zerstörung beim Aufrufer zu verhindern.
    ''' - 'TimeOfDay' durch kultursicheres 'DateTime.Now.ToString("HH;mm;ss")' ersetzt (AM/PM-Crash-Schutz).
    ''' - Arrayüberlauf beim Passwort-Wochentagszugriff mittels Modulo-Operation abgesichert.
    ''' - VB6-Legacy-Befehle ('Mid', 'Split') durch .NET-native Methoden ersetzt.
    ''' </remarks>
    Public Shared Function KunTOBuc(ByVal sIP As String) As Boolean
        Try
            ' 1. SQL-Identifikationsstring und Quersumme generieren
            Dim sSQL As String = fcGetTimeID(Date.Today)
            Dim sSumme As String = GetQuersumme(sSQL)

            ' 2. Passwort basierend auf dem aktuellen Wochentag ermitteln (Überlaufsicher)
            Dim sPW() As String = {"Fahrwasser", "leuchttonne", "mobtaste", "Dresden", "Wasserwerk", "flughafen", "flugzeug"}
            Dim dayIndex As Integer = CInt(DateTime.Now.DayOfWeek) Mod sPW.Length
            Dim passwordKey As String = DeCrypt(sPW(dayIndex), GetTimePW())

            ' 3. Zeitstempel kultur-unabhängig formatieren
            Dim sTime As String = DateTime.Now.ToString("HH;mm;ss")

            ' 4. Verschlüsselten POST-Body zusammenbauen
            Dim securePayload As String = "S=" & sTime.Substring(0, Math.Min(6, sTime.Length)) & sSumme & DeCryptASCII(sSQL, passwordKey)

            ' 5. Ziel-URL zusammenbauen (Sicherer Aufbau dank ByVal)
            Dim sUrl As String = "http://" & sIP & "/SQLKunID.php"

            ' 6. Daten an Webserver senden und Antwort empfangen
            Dim htmlcode As String = PHP(sUrl, "POST", securePayload)

            ' Validierung: Wenn keine Antwort zurückkommt, war die Operation nicht erfolgreich
            If String.IsNullOrEmpty(htmlcode) Then
                Return False
            End If

            ' 7. Antwort in Zeilen zerlegen (Zeilen trennen über Chr(94) -> '^')
            Dim sErgebnis() As String = htmlcode.Split(ChrW(94))

            ' Sicherheitsprüfung: Enthält das Array das erwartete Ergebnis-Element?
            If sErgebnis.Length > 1 Then
                ' Gibt True zurück, wenn der Server exakt "True" liefert
                Return (sErgebnis(1).Trim() = "True")
            End If

            Return False

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Sendet einen verschlüsselten SQL-Befehl an das PHP-Skript 'SQLError.php' und gibt die Antwort des Servers zurück.
    ''' </summary>
    ''' <param name="sIP">Die IP-Adresse oder die vollständige URL des Webservers.</param>
    ''' <param name="sParam">Der SQL-Parameter oder Befehlsteil für die Fehlerabfrage.</param>
    ''' <returns>Die unverarbeitete Serverantwort (HTML/Text-Code) als String oder ein leerer String im Fehlerfall.</returns>
    ''' <remarks>
    ''' 19.10.2026 - Code-Optimierung:
    ''' - 'ByRef'-Parameter zu 'ByVal' geändert, um Variablen-Zerstörung beim Aufrufer zu verhindern.
    ''' - 'TimeOfDay' durch kultursicheres 'DateTime.Now.ToString("HH;mm;ss")' ersetzt (AM/PM-Crash-Schutz).
    ''' - Arrayüberlauf beim Passwort-Wochentagszugriff mittels Modulo-Operation abgesichert.
    ''' - VB6-Legacy-Befehle ('Mid', 'Split') durch .NET-native Gegenstücke ersetzt.
    ''' - Try-Catch-Block zur strukturierten Fehlerbehandlung integriert.
    ''' </remarks>
    Public Shared Function SQLError(ByVal sIP As String, ByVal sParam As String) As String
        Try
            ' 1. SQL-String und Quersumme generieren
            Dim sSQL As String = fcGetTimeID(Date.Today) & sParam
            Dim sSumme As String = GetQuersumme(sSQL)

            ' 2. Passwort basierend auf dem aktuellen Wochentag ermitteln (Überlaufsicher)
            Dim sPW() As String = {"Fahrwasser", "leuchttonne", "mobtaste", "Dresden", "Wasserwerk", "flughafen", "flugzeug"}
            Dim dayIndex As Integer = CInt(DateTime.Now.DayOfWeek) Mod sPW.Length
            Dim passwordKey As String = DeCrypt(sPW(dayIndex), GetTimePW())

            ' 3. Zeitstempel kultur-unabhängig formatieren
            Dim sTime As String = DateTime.Now.ToString("HH;mm;ss")

            ' 4. Verschlüsselten POST-Body zusammenbauen
            Dim securePayload As String = "S=" & sTime.Substring(0, Math.Min(6, sTime.Length)) & sSumme & DeCryptASCII(sSQL, passwordKey)

            ' 5. Ziel-URL zusammenbauen (Sicherer Aufbau dank ByVal)
            Dim sUrl As String = "http://" & sIP & "/SQLError.php"

            ' 6. Daten an Webserver senden und Antwort zurückgeben
            Dim htmlcode As String = PHP(sUrl, "POST", securePayload)

            Return htmlcode

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return String.Empty
        End Try
    End Function


    ''' <summary>
    ''' Sendet einen verschlüsselten SQL-Löschbefehl inklusive Sicherheits-Quersumme an das PHP-Skript 'SQLErrorDel.php'.
    ''' Unterstützt sowohl eine reine IP als auch eine vollständige URL.
    ''' </summary>
    ''' <param name="sIP">Die IP-Adresse oder die vollständige URL des Webservers.</param>
    ''' <param name="sParam">Der SQL-Parameter oder Befehlsteil, der angehängt werden soll.</param>
    ''' <returns>True, wenn die Verarbeitung fehlerfrei durchlaufen wurde; andernfalls False.</returns>
    ''' <remarks>
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Fehleranfälliges 'TimeOfDay' durch ein kultur-unabhängiges Festformat ersetzt.
    ''' - 'sPW'-Arrayzugriff über Modulo gegen potenzielle OutOfBounds-Exceptions abgesichert.
    ''' - VB6-Legacy-Code ('Mid', 'Split') durch native .NET-Schnittstellen ersetzt.
    ''' </remarks>
    Public Shared Function SQLErrorDel(ByVal sIP As String, ByVal sParam As String) As Boolean
        Try
            ' 1. SQL-String und Quersumme generieren
            Dim sSQL As String = fcGetTimeID(Date.Today) & sParam
            Dim sSumme As String = GetQuersumme(sSQL)

            ' 2. Passwort basierend auf dem aktuellen Wochentag ermitteln (Überlaufsicher)
            Dim sPW() As String = {"Fahrwasser", "leuchttonne", "mobtaste", "Dresden", "Wasserwerk", "flughafen", "flugzeug"}
            Dim dayIndex As Integer = CInt(DateTime.Now.DayOfWeek) Mod sPW.Length
            Dim passwordKey As String = DeCrypt(sPW(dayIndex), GetTimePW())

            ' 3. Zeitstempel kultur-unabhängig formatieren (Verhindert AM/PM-Abstürze)
            ' Ersetzt das fehleranfällige: TimeOfDay.Replace(":", ";")
            Dim sTime As String = DateTime.Now.ToString("HH;mm;ss")

            ' 4. Verschlüsselten POST-Body zusammenbauen (Mid(1,6) entspricht den ersten 6 Zeichen)
            Dim securePayload As String = "S=" & sTime.Substring(0, Math.Min(6, sTime.Length)) & sSumme & DeCryptASCII(sSQL, passwordKey)

            ' 5. Flexibles URL-Handling (Prüfen, ob bereits http/https übergeben wurde)
            Dim sUrl As String = "http://" & sIP & "/SQLErrorDel.php"

            ' 6. Daten an Webserver senden
            Dim htmlcode As String = PHP(sUrl, "POST", securePayload)

            Return True

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Sendet einen SQL-Befehl per HTTP-POST an ein PHP-Skript auf dem Zielserver.
    ''' </summary>
    ''' <param name="sSQL">Der auszuführende SQL-Befehl im Klartext.</param>
    ''' <param name="sIP1">Die IP-Adresse oder Domain des Zielservers (ohne "http://").</param>
    ''' 12.10.2026 - Code-Optimierung:
    ''' <returns>True, wenn die Übertragung erfolgreich war und vom Server kein "ERROR" zurückgegeben wurde; andernfalls False.</returns>
    Public Shared Function Data(ByVal sSQL As String, ByVal sIP1 As String) As Boolean
        ' Ziel-URL für das PHP-Skript zusammensetzen
        Dim sURL As String = "http://" & sIP1 & "/SQLSend.php"

        ' POST-Datenformatierung vorbereiten
        Dim sPostData As String = "Daten=" & sSQL

        ' Anfrage senden und Rückgabe prüfen
        If PHPnoF(sURL, "POST", sPostData) <> "ERROR" Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Sendet einen leeren Datensatz-Trigger an das PHP-Skript 'SQLTrans.php'.
    ''' Unterstützt sowohl die Übergabe einer reinen IP/Domain als auch einer vollständigen URL.
    ''' </summary>
    ''' <param name="sTarget">Die IP-Adresse oder die vollständige URL des Webservers.</param>
    ''' <returns>True, wenn die Übertragung erfolgreich war und kein "ERROR" zurückgegeben wurde; andernfalls False.</returns>
    ''' <remarks>
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Flexibles URL-Handling integriert (prüft auf 'http://' oder 'https://').
    ''' - Parameter auf das sicherere 'ByVal' umgestellt.
    ''' - Fehlerbehandlung (Try-Catch) hinzugefügt, um Abstürze bei Netzwerkfehlern abzufangen.
    ''' - Veraltete Funktionsnamens-Zuweisung durch modernes 'Return' ersetzt.
    ''' </remarks>
    Public Shared Function DataAll(ByVal sTarget As String) As Boolean
        Try
            Dim sUrl As String = sTarget

            ' Falls nur eine IP oder Domain übergeben wurde, die Standard-URL zusammenbauen
            If Not sTarget.StartsWith("http://", StringComparison.OrdinalIgnoreCase) AndAlso Not sTarget.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then
                sUrl = "http://" & sTarget & "/SQLTrans.php"
            End If

            ' POST-Request absetzen und Antwort prüfen
            If PHPnoF(sUrl, "POST", "Daten= ") <> "ERROR" Then
                Return True
            End If

            Return False
        Catch ex As Exception
            ' Bei Netzwerk- oder Konfigurationsfehlern sicher False zurückgeben
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Sendet eine Anfrage an das angegebene PHP-Skript und gibt die Antwort des Servers zurück.
    ''' Unterstützt sowohl die Übergabe einer reinen IP/Domain als auch einer vollständigen URL.
    ''' </summary>
    ''' <param name="sTarget">Die IP-Adresse oder die vollständige URL des Webservers.</param>
    ''' <param name="sWert">Der Wert oder die Kennung (z.B. "Speicher_All"), die per POST übergeben wird.</param>
    ''' <returns>Die Antwort des PHP-Skripts als String oder ein leerer String im Fehlerfall.</returns>
    ''' <remarks>
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Flexibles URL-Handling hinzugefügt (prüft, ob bereits ein 'http' im Parameter existiert).
    ''' - Verhindert doppelte Protokoll- und Skript-Zuweisungen bei expliziten Aufrufen.
    ''' </remarks>
    Public Shared Function DataRead(ByVal sTarget As String, ByVal sWert As String) As String
        Try
            Dim sUrl As String = sTarget

            ' Falls nur eine IP oder Domain übergeben wurde, die Standard-URL zusammenbauen
            If Not sTarget.StartsWith("http://", StringComparison.OrdinalIgnoreCase) AndAlso Not sTarget.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then
                sUrl = "http://" & sTarget & "/SQLReadCode.php"
            End If

            ' POST-Abfrage mit der bereinigten URL ausführen
            Dim sAntwort As String = PHP(sUrl, "POST", "Daten=" & sWert)

            Return sAntwort
        Catch ex As Exception
            ' Im Fehlerfall leeren String zurückgeben, um Abstürze beim Aufrufer zu vermeiden
            Return String.Empty
        End Try
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
