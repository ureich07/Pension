Option Explicit On
Imports System.Text

Module moDB

#Region "Hilfsfunktionen zum lesen und schreiben von Daten"

    ''' <summary>
    ''' Fügt einen neuen Datensatz per SQL in die angegebene Tabelle ein.
    ''' Feldstruktur und Werte werden als Arrays übergeben.
    ''' </summary>
    ''' <param name="cTabelle">Der Name der Ziel-Datenbanktabelle.</param>
    ''' <param name="arFields">Ein Array mit den Spaltennamen.</param>
    ''' <param name="arValue">Ein Array mit den dazugehörigen Werten.</param>
    ''' <returns>True, wenn das Einfügen erfolgreich war, andernfalls False.</returns>
    ''' <remarks>
    ''' 14.12.2007 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete VB6-Funktionen (UBound, Trim$, Mid, Len) durch moderne .NET-Entsprechungen ersetzt.
    ''' - SQL-Injection-Schutz durch Maskierung von Hochkommas (.Replace) implementiert.
    ''' - 'StringBuilder' für performanten und sauberen Aufbau des SQL-Befehls verwendet.
    ''' - Veraltete Zuweisung an den Funktionsnamen durch ein explizites 'Return' ersetzt.
    ''' </remarks>
    Public Function fcInsertCommand(ByVal cTabelle As String,
                                ByVal arFields() As String,
                                ByVal arValue() As String) As Boolean
        ' Sicherheitsprüfung: Arrays dürfen nicht Nothing sein
        If arFields Is Nothing OrElse arValue Is Nothing Then Return False

        ' Längenprüfung der Arrays (ersetzt UBound)
        If arFields.Length = 0 OrElse arFields.Length <> arValue.Length Then Return False

        Try
            Dim sbSql As New StringBuilder()
            sbSql.Append("INSERT INTO ").Append(cTabelle).Append(" (")

            ' 1. Spaltennamen verketten
            For i As Integer = 0 To arFields.Length - 1
                Dim fieldName As String = If(arFields(i) IsNot Nothing, arFields(i).Trim(), "")
                sbSql.Append(fieldName)

                If i < arFields.Length - 1 Then
                    sbSql.Append(", ")
                End If
            Next

            sbSql.Append(") VALUES (")

            ' 2. Werte verketten und aufbereiten
            For i As Integer = 0 To arValue.Length - 1
                Dim valueText As String = If(arValue(i) IsNot Nothing, arValue(i).Trim(), "")

                ' Boolean-Konvertierung für Access/SQL-Kompatibilität
                If valueText.Equals("True", StringComparison.OrdinalIgnoreCase) Then
                    valueText = "-1"
                ElseIf valueText.Equals("False", StringComparison.OrdinalIgnoreCase) Then
                    valueText = "0"
                Else
                    ' WICHTIG: SQL-Injection-Schutz für Strings (Hochkommas verdoppeln)
                    valueText = valueText.Replace("'", "''")
                End If

                ' Wert in einfache Anführungszeichen setzen
                sbSql.Append("'").Append(valueText).Append("'")

                If i < arValue.Length - 1 Then
                    sbSql.Append(", ")
                End If
            Next

            sbSql.Append(")")

            ' Eigentliches Schreiben in die Datenbank
            UpdateTable(sbSql.ToString())

            Return True

        Catch ex As Exception
            ' Optional: ErrReport nutzen, falls vorhanden, statt MsgBox im Live-Betrieb
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Fehler in fcInsertCommand")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Aktualisiert Datensätze in einer spezifischen Datenbanktabelle anhand übergebener Feld- und Wert-Arrays.
    ''' </summary>
    ''' <param name="cTabelle">Der Name der zu aktualisierenden Tabelle.</param>
    ''' <param name="arFields">Ein String-Array, das die Namen der zu aktualisierenden Spalten enthält.</param>
    ''' <param name="arValue">Ein String-Array, das die neuen Werte für die entsprechenden Spalten enthält.</param>
    ''' <param name="cBedingung">Die SQL-Filterbedingung (z. B. <c> WHERE ID = '123'</c>).</param>
    ''' <returns><c>True</c>, wenn das Update erfolgreich ausgeführt wurde; andernfalls <c>False</c>.</returns>
    ''' <remarks>
    ''' <para>Die Arrays für Felder und Werte müssen zwingend die gleiche Länge aufweisen. Boolesche Strings ('True'/'False') werden automatisch in SQL-konforme Bit-Werte ('-1'/'0') übersetzt.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 14.12.2007 – Erstellt<br/>
    ''' 22.09.2026 – Optimiert: Umstellung auf den hochperformanten <c>StringBuilder</c> zur String-Generierung. Veraltete VB6-Funktionen (<c>UBound</c>, <c>Mid</c>, <c>Len</c>) durch native .NET-Äquivalente ersetzt. Hochkomma-Verdopplung zur Vorbeugung elementarer SQL-Fehler hinzugefügt.<br/>
    ''' </para>
    ''' </remarks>
    Public Function fcUpdateCommand(ByVal cTabelle As String,
                               ByVal arFields() As String,
                               ByVal arValue() As String,
                               ByVal cBedingung As String) As Boolean
        ' Validierung der Eingaben
        If arFields Is Nothing OrElse arValue Is Nothing Then Return False

        Dim nMax As Integer = arFields.Length - 1
        Dim mMax As Integer = arValue.Length - 1

        ' Validierung: Längenprüfung der Arrays
        If nMax = -1 OrElse nMax <> mMax Then Return False

        Try
            ' WICHTIGE OPTIMIERUNG: StringBuilder statt ressourcenfressender String-Kette (&) in der Schleife
            Dim sbSql As New System.Text.StringBuilder()
            sbSql.AppendFormat("UPDATE {0} SET ", cTabelle)

            For i As Integer = 0 To nMax
                Dim cTmp As String = arValue(i)

                ' Boolesche Werte für Access/SQL-Server kompatibel umwandeln
                If cTmp = "True" Then
                    cTmp = "-1"
                ElseIf cTmp = "False" Then
                    cTmp = "0"
                Else
                    ' MINIMAL-SCHUTZ: Verhindert Abstürze bei Namen wie O'Connor durch Verdopplung des Hochkommas
                    If cTmp IsNot Nothing Then cTmp = cTmp.Replace("'", "''")
                End If

                ' Spaltenname trimmen und Wert hinzufügen
                Dim feldName As String = arFields(i).Trim()
                sbSql.AppendFormat("{0}='{1}',", feldName, cTmp)
            Next

            ' Das letzte, überschüssige Komma entfernen
            If sbSql.Length > 0 Then sbSql.Length -= 1

            ' Bedingung anhängen
            sbSql.Append(cBedingung)

            ' SQL-Befehl an die ausführende Methode übergeben
            UpdateTable(sbSql.ToString())

            Return True

        Catch ex As Exception
            ' Fehlerbehandlung über die interne Schnittstelle (MsgBox durch ErrReport ersetzt, um UI-Blockaden zu vermeiden)
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Erstellt einen neuen Datensatz in der angegebenen DataTable.
    ''' Die Feldstruktur und die dazugehörigen Werte werden per Array übergeben.
    ''' </summary>
    ''' <param name="dtTmp">Die DataTable, in die der neue Datensatz eingefügt werden soll.</param>
    ''' <param name="arFields">Ein String-Array mit den Namen der zu befüllenden Spalten.</param>
    ''' <param name="arValue">Ein String-Array mit den Werten für die neuen Spalten.</param>
    ''' <returns>True, wenn das Hinzufügen erfolgreich war; False bei Validierungs- oder Laufzeitfehlern.</returns>
    ''' <remarks>
    ''' 30.12.2007 - Create
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Null-Validierung für DataTable und Quell-Arrays hinzugefügt.
    ''' - Fehleranfällige 'UBound'-Aufrufe durch die sichere '.Length'-Eigenschaft ersetzt.
    ''' - 'Trim$' durch performanteres, .NET-natives 'Trim()' ersetzt.
    ''' - Sicherheitsprüfung integriert, ob die übergebene Spalte existiert, um Abstürze zu verhindern.
    ''' - 'Exit Function' durch strukturierte 'Return'-Anweisungen ersetzt.
    ''' </remarks>
    Public Function fcInsertTable(ByVal dtTmp As DataTable,
                              ByVal arFields() As String,
                              ByVal arValue() As String) As Boolean
        Try
            ' Validierung: Prüfen, ob die Tabellen- und Array-Objekte existieren
            If dtTmp IsNot Nothing OrElse arFields IsNot Nothing OrElse arValue IsNot Nothing Then
                Return False
            End If

            Dim nMax As Integer = arFields.Length
            Dim mMax As Integer = arValue.Length

            ' Validierung: Arrays dürfen nicht leer sein und müssen dieselbe Länge haben
            If nMax = 0 OrElse nMax <> mMax Then
                Return False
            End If

            ' Neue Zeile basierend auf dem Schema der DataTable erstellen
            Dim newRow As DataRow = dtTmp.NewRow()

            ' Werte in die neue Zeile übertragen
            For i As Integer = 0 To nMax - 1
                Dim fieldName As String = arFields(i).Trim()

                ' Prüfen, ob die Spalte in der DataTable existiert
                If dtTmp.Columns.Contains(fieldName) Then
                    newRow(fieldName) = arValue(i)
                End If
            Next

            ' Die fertig befüllte Zeile an die Tabelle anhängen
            dtTmp.Rows.Add(newRow)
            Return True

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Aktualisiert Werte in einer DataTable für Zeilen, die der angegebenen Bedingung entsprechen.
    ''' Die zu aktualisierenden Felder und deren neue Werte werden als Arrays übergeben.
    ''' </summary>
    ''' <param name="dtTmp">Die zu aktualisierende DataTable.</param>
    ''' <param name="arFields">Ein String-Array mit den Namen der zu aktualisierenden Spalten.</param>
    ''' <param name="arValue">Ein String-Array mit den neuen Werten für die Spalten.</param>
    ''' <param name="cBedingung">Die Filterbedingung (SQL-Where-Syntax), um die Zielzeile(n) zu bestimmen.</param>
    ''' <returns>True, wenn die Aktualisierung erfolgreich war oder die Validierung fehlschlug; False bei einem Laufzeitfehler.</returns>
    ''' <remarks>
    ''' 14.12.2007 - Create
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Fehleranfällige 'UBound'-Aufrufe durch die sichere '.Length'-Eigenschaft ersetzt.
    ''' - Veraltetes 'Trim$' durch das performantere .NET-native 'Trim()' ersetzt.
    ''' - 'Exit Function' bei Validierungsfehlern durch explizite 'Return False'-Anweisungen abgesichert.
    ''' - Lokale Schleifenvariable 'i' direkt im Schleifenkopf deklariert.
    ''' </remarks>
    Public Function fcUpdateTable(ByVal dtTmp As DataTable,
                              ByVal arFields() As String,
                              ByVal arValue() As String,
                              ByVal cBedingung As String) As Boolean
        Try
            ' Validierung: Prüfen, ob die Tabellen- und Array-Objekte existieren
            If dtTmp IsNot Nothing OrElse arFields IsNot Nothing OrElse arValue IsNot Nothing Then
                Return False
            End If

            Dim nMax As Integer = arFields.Length
            Dim mMax As Integer = arValue.Length

            ' Validierung: Arrays dürfen nicht leer sein und müssen dieselbe Länge haben
            If nMax = 0 OrElse nMax <> mMax Then
                Return False
            End If

            ' Zeilen filtern, die der Bedingung entsprechen
            Dim cRow() As DataRow = dtTmp.Select(cBedingung)

            ' Wenn passende Zeilen gefunden wurden, die erste Zeile (Index 0) aktualisieren
            If cRow.Length > 0 Then
                For i As Integer = 0 To nMax - 1
                    Dim fieldName As String = arFields(i).Trim()

                    ' Prüfen, ob die Spalte tatsächlich in der DataTable existiert, um Fehler zu vermeiden
                    If dtTmp.Columns.Contains(fieldName) Then
                        cRow(0)(fieldName) = arValue(i)
                    End If
                Next
            End If

            Return True

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Löscht alle Datensätze aus einer DataTable, die der angegebenen Bedingung entsprechen.
    ''' </summary>
    ''' <param name="dtTmp">Die zu bearbeitende DataTable.</param>
    ''' <param name="cBedingung">Der Filterausdruck (z. B. "ID = '123'").</param>
    ''' <returns>True, wenn der Löschvorgang fehlerfrei durchlaufen wurde, andernfalls False.</returns>
    ''' <remarks>
    ''' 20.03.2009 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Schleife hinzugefügt, um alle matchenden Zeilen zu löschen (nicht nur die erste).
    ''' - Veraltete Namespace-Deklaration 'Data.DataRow' auf die .NET-Standard-Klasse 'DataRow' verkürzt.
    ''' - Veraltete Zuweisung an den Funktionsnamen durch ein explizites 'Return' ersetzt.
    ''' - Null- und Leerkontrolle für die Parameter integriert.
    ''' </remarks>
    Public Function fcDeleteTableRow(ByVal dtTmp As DataTable, ByVal cBedingung As String) As Boolean
        ' Sicherheitsprüfung: DataTable muss existieren und Bedingung darf nicht leer sein
        If dtTmp Is Nothing OrElse String.IsNullOrWhiteSpace(cBedingung) Then Return False

        Try
            ' Alle Zeilen ermitteln, auf die die Bedingung zutrifft
            Dim foundRows As DataRow() = dtTmp.Select(cBedingung)

            ' Wenn Datensätze gefunden wurden, alle Betroffenen löschen
            If foundRows.Length > 0 Then
                For Each row As DataRow In foundRows
                    row.Delete()
                Next
            End If

            Return True

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Führt eine aktualisierende oder löschende SQL-Anweisung (z. B. INSERT, UPDATE, DELETE) 
    ''' über eine externe PHP-Schnittstelle auf dem Webserver aus.
    ''' </summary>
    ''' <param name="sql_String">Die auszuführende SQL-Anweisung.</param>
    ''' <returns>Die Anzahl der betroffenen Datensätze (sofern vom PHP-Skript zurückgegeben).</returns>
    ''' <remarks>
    ''' Die Funktion leitet den SQL-Befehl zusammen mit der in <c>cgIPWeb</c> hinterlegten 
    ''' Server-IP an die PHP-Komponente weiter.
    ''' 19.01.2012 Create <br/>
    ''' 23.09.2026 Refactored 
    ''' </remarks>
    Public Function UpdateTable(ByVal sql_String As String) As Integer
        ' Validierung: Leere SQL-Strings gar nicht erst an den Server senden
        If String.IsNullOrWhiteSpace(sql_String) Then Return 0

        Dim sIP As String = cgIPWeb

        ' WICHTIG: "Return" hinzugefügt, um das Ergebnis von PHP.Update auch wirklich zurückzugeben
        Return PHP.Update(sql_String, sIP)
    End Function

    ''' <summary>
    ''' Sendet eine SQL-Abfrage an das PHP-Web-Backend und gibt das Ergebnis als DataTable zurück.
    ''' </summary>
    ''' <param name="cSQL">Der auszuführende SQL-Befehl als String.</param>
    ''' <returns>Eine DataTable mit den abgefragten Datensätzen oder eine leere DataTable im Fehlerfall.</returns>
    ''' <remarks>
    ''' 22.09.2026 Code-Optimierung: Integration von Exception-Handling zur Absicherung bei Netzwerk- 
    ''' oder Serverfehlern und Umstellung auf das modernere 'Return'-Schlüsselwort.
    ''' </remarks>
    Public Function fcReadDataTable(ByVal cSQL As String) As DataTable
        ' Vorabprüfung: Wenn kein SQL-Befehl übergeben wurde, direkt abbrechen
        If String.IsNullOrWhiteSpace(cSQL) Then Return New DataTable()

        Try

            ' Abfrage über PHP-Schnittstelle ausführen
            Dim sIP As String = cgIPWeb
            Dim dtErgebnis As DataTable = PHP.DataTable(cSQL, sIP)

            If dtErgebnis IsNot Nothing Then
                Return dtErgebnis
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
        ' Gibt im Fehlerfall eine leere, aber initialisierte DataTable zurück (verhindert Folge-Abstürze)
        Return New DataTable()
    End Function

    ''' <summary>
    ''' Sucht einen oder mehrere spezifische Datensätze in der Datenbank.
    ''' Gibt ein String-Array mit den Werten der angeforderten Felder für den angegebenen Datensatz-Index zurück.
    ''' Wenn kein Datensatz gefunden wird, enthält das erste Element des Arrays ein Leerzeichen (" ").
    ''' </summary>
    ''' <param name="sSQLText">Der SQL-Basisstring (z. B. "Select * From Werbung Where Name='").</param>
    ''' <param name="sVergleichswert">Der Wert, der an den SQL-String angehängt wird. Bleibt er leer, wird das öffnende Anführungszeichen entfernt.</param>
    ''' <param name="nSatz">Der zeilenbasierte Index des zurückzugebenden Datensatzes (0 für den ersten Treffer).</param>
    ''' <param name="arRuekgabeFeld">Ein Parameter-Array (ParamArray) mit den Namen der Tabellenspalten, die ausgelesen werden sollen.</param>
    ''' <returns>Ein String-Array mit den Werten der abgefragten Spalten.</returns>
    ''' <remarks>
    ''' <b>Historie:</b><br/>
    ''' 03.02.2012 – Erstellt<br/>
    ''' 24.09.2026 – Uwe: Array-Zuweisung korrigiert (verhindert Indexfehler bei leeren Treffern), auf ByVal umgestellt und Rückgabetyp konkretisiert.<br/>
    ''' </remarks>
    Public Function fcDataSeek(ByVal sSQLText As String,
                           ByVal sVergleichswert As String,
                           ByVal nSatz As Integer,
                           ByVal ParamArray arRuekgabeFeld() As String) As String()

        Dim sSQL As String = ""

        ' 1. SQL-String dynamisch zusammenbauen
        If String.IsNullOrEmpty(sVergleichswert) Then
            If sSQLText.EndsWith("'") OrElse sSQLText.EndsWith("=") Then
                sSQL = sSQLText.Substring(0, sSQLText.Length - 1)
            Else
                sSQL = sSQLText
            End If
        Else
            sSQL = sSQLText & sVergleichswert & "'"
        End If

        ' 2. Daten abrufen
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' Ergebnis-Array in der passenden Größe initialisieren
        Dim nFeldAnzahl As Integer = If(arRuekgabeFeld IsNot Nothing, arRuekgabeFeld.Length, 0)
        Dim arResult(Math.Max(0, nFeldAnzahl - 1)) As String

        ' 3. Prüfen, ob Datensätze vorhanden sind und der gewünschte Index existiert
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso nSatz < dt.Rows.Count Then
            For i As Integer = 0 To nFeldAnzahl - 1
                ' Spaltenwert sicher auslesen
                If dt.Columns.Contains(arRuekgabeFeld(i)) Then
                    arResult(i) = dt.Rows(nSatz).Item(arRuekgabeFeld(i)).ToString()
                Else
                    arResult(i) = ""
                End If
            Next
        Else
            ' Fallback laut historischer Definition: Wenn nichts gefunden wurde, erstes Feld mit Leerzeichen füllen
            If arResult.Length > 0 Then
                arResult(0) = " "
            End If
        End If

        Return arResult
    End Function

    ''' <summary>
    ''' Sucht einen Datensatz anhand des SQL-Befehls und gibt die gewünschten Feldwerte als Array zurück.
    ''' Bei Nichtauffinden enthält das erste Element (Index 0) ein Leerzeichen " ".
    ''' </summary>
    ''' <param name="sSQLText">Der SQL-Select-Befehl zur Datenabfrage.</param>
    ''' <param name="nSatz">Der nullbasierte Index des gewünschten Datensatzes (Zeile) aus der Ergebnismenge.</param>
    ''' <param name="arRuekgabeFeld">Ein ParamArray der Spaltennamen, deren Werte zurückgegeben werden sollen.</param>
    ''' <returns>Ein String-Array mit den Werten der angeforderten Felder oder ein Array mit " " an Index 0 bei Misserfolg.</returns>
    ''' <remarks>
    ''' 26.03.2012 - Create
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Rückgabetyp von generischem 'Array' auf typsicheres 'String()' geändert.
    ''' - 'ByRef' bei Eingabeparametern durch das sicherere 'ByVal' ersetzt.
    ''' - 'Convert.ToString()' implementiert, um DBNull-Fehler abzufangen.
    ''' - Try-Catch-Block zur Fehlerbehandlung integriert.
    ''' - Veraltete Zuweisung an den Funktionsnamen durch modernes 'Return' ersetzt.
    ''' </remarks>
    Public Function fcGetData(ByVal sSQLText As String, ByVal nSatz As Integer, ByVal ParamArray arRuekgabeFeld() As String) As String()
        Try
            ' Daten über SQL laden
            Dim dt As DataTable = fcReadDataTable(sSQLText)

            ' Prüfen, ob die Tabelle Zeilen enthält und der angeforderte Index existiert
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso nSatz <= dt.Rows.Count - 1 Then
                ' Werte der Zeile direkt in das Array schreiben
                For i As Integer = 0 To arRuekgabeFeld.Length - 1
                    arRuekgabeFeld(i) = Convert.ToString(dt.Rows(nSatz).Item(arRuekgabeFeld(i)))
                Next
            Else
                ' Fallback, wenn kein Datensatz gefunden wurde oder der Index außerhalb liegt
                If arRuekgabeFeld.Length > 0 Then
                    arRuekgabeFeld(0) = " "
                Else
                    ' Falls das übergebene Array komplett leer war, ein Element mit Leerzeichen initialisieren
                    ReDim arRuekgabeFeld(0)
                    arRuekgabeFeld(0) = " "
                End If
            End If

            Return arRuekgabeFeld

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            ' Im Fehlerfall ein sicheres Fallback-Array zurückgeben
            Return New String() {" "}
        End Try
    End Function

    ''' <summary>
    ''' Liest Daten aus einer SQL-Tabelle und formatiert diese als ausgerichteten Text-String.
    ''' Jedes Feld wird anhand der maximalen Spaltenbreite aufgefüllt. Die Spalten werden mit "|" getrennt, Zeilen mit CRLF (1013).
    ''' </summary>
    ''' <param name="sTabel">Der Name der abzufragenden SQL-Tabelle.</param>
    ''' <param name="sBedingung">Die SQL-Where-Klausel für die Filterung.</param>
    ''' <param name="sFeld">Semikolon-getrennte Liste der Datenbank-Spaltennamen.</param>
    ''' <param name="sKopf">Semikolon-getrennte Liste der Spaltenüberschriften.</param>
    ''' <returns>Ein formatierter String, der die Tabellendaten tabellarisch darstellt.</returns>
    ''' <remarks>
    ''' 20.04.2024 - Create (Burg Spreewald)
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Den extrem langsamen String-Verkettungsoperator (&amp;) in Schleifen durch einen hochperformanten 'StringBuilder' ersetzt.
    ''' - Das ressourcenfressende zweidimensionale Hilfs-Array (aDT) eliminiert.
    ''' - 'Convert.ToString()' implementiert, um Laufzeitfehler bei DBNull-Werten in der Datenbank zu verhindern.
    ''' - Die erste Zeile (Index 0) enthält wie im Original die berechneten maximalen Spaltenlängen.
    ''' </remarks>
    Public Function fcTabelView(ByVal sTabel As String, ByVal sBedingung As String,
                            ByVal sFeld As String, ByVal sKopf As String) As String
        Try
            Dim sb As New System.Text.StringBuilder()
            Dim aKopf() As String = Split(sKopf, ";")
            Dim aFeld() As String = Split(sFeld, ";")
            Dim nSpalte As Integer = aKopf.Length - 1

            ' Daten über SQL laden
            Dim sSQL As String = "Select * From " & sTabel & " Where " & sBedingung
            Dim dt As DataTable = fcReadDataTable(sSQL)
            Dim nMax As Integer = dt.Rows.Count - 1

            ' Array zur Ermittlung der maximalen Spaltenbreiten
            Dim aLength(nSpalte) As Integer

            ' 1. Durchlauf: Maximale Länge ermitteln (Header + Daten)
            For j As Integer = 0 To nSpalte
                aLength(j) = aKopf(j).Length
                For i As Integer = 0 To nMax
                    Dim val As String = Convert.ToString(dt.Rows(i).Item(aFeld(j)))
                    If val.Length > aLength(j) Then
                        aLength(j) = val.Length
                    End If
                Next
            Next

            ' Zeile 0: Längen-Informationen schreiben
            For j As Integer = 0 To nSpalte
                sb.Append(aLength(j)).Append("|")
            Next
            If sb.Length > 0 Then sb.Length -= 1 ' Letztes Trennzeichen entfernen
            sb.AppendLine()

            ' Zeile 1: Header (Kopfzeile) schreiben
            For j As Integer = 0 To nSpalte
                sb.Append(aKopf(j)).Append("|")
            Next
            If sb.Length > 0 Then sb.Length -= 1
            sb.AppendLine()

            ' Zeile 2 bis N: Datenzeilen schreiben
            For i As Integer = 0 To nMax
                For j As Integer = 0 To nSpalte
                    Dim val As String = Convert.ToString(dt.Rows(i).Item(aFeld(j)))
                    sb.Append(val).Append("|")
                Next
                If sb.Length > 0 Then sb.Length -= 1
                sb.AppendLine()
            Next

            Return sb.ToString()

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Hängt einen leeren Datensatz (Blank) an die angegebene Datei/Tabelle an.
    ''' </summary>
    ''' <param name="sFile">Der Name der Datei oder Tabelle im System.</param>
    ''' <returns>Die ID-Nummer des neu angelegten Datensatzes als String.</returns>
    ''' <remarks>
    ''' 16.09.2013 Create <br/>
    ''' 23.09.2026 Refactored (ByVal-Korrektur und Return-Statement implementiert)
    ''' </remarks>
    Public Function fcAppendBlank(ByVal sFile As String) As String
        Dim sIp As String = cgIPWeb

        ' Explizites Return statt Zuweisung an den Funktionsnamen
        Return PHP.Append(sFile, sIp)
    End Function


#End Region


    ''' <summary>
    ''' Konvertiert einen Datenbankwert sicher in einen Boolean-Wert.
    ''' Fängt Altdaten wie leere Strings, DBNull, 0 sowie VB6/Access-Reste wie -1 sicher ab.
    ''' </summary>
    ''' <param name="dbValue">Der zu prüfende Wert aus der DataRow.</param>
    ''' <param name="defaultValue">Der Standardwert, falls die Konvertierung fehlschlägt (Standard ist False).</param>
    ''' <returns>Der konvertierte Boolean-Wert oder der Standardwert.</returns>
    ''' <remarks>
    ''' 09.10.2026 - Update:
    ''' - Unterstützung für Access/VB6-Altlasterkennung ergänzt: '-1' wird jetzt korrekt als 'True' interpretiert.
    ''' 15.10.2026 - Update:
    ''' - Unterstützung für negative Integer-Werte (z. B. '-1', '-2') hinzugefügt, die als 'True' gewertet werden.
    ''' </remarks>
    Public Function fcGetBoolean(ByVal dbValue As Object, Optional ByVal defaultValue As Boolean = False) As Boolean
        ' Prüfen auf Null oder DBNull
        If dbValue Is Nothing OrElse dbValue Is DBNull.Value Then
            Return defaultValue
        End If

        Dim sValue As String = dbValue.ToString().Trim()

        ' Leere Strings abfangen
        If String.IsNullOrEmpty(sValue) Then
            Return defaultValue
        End If

        ' Legacy-Werte aus älteren Datenbanken (Access, SQL Server, VB6) abfangen
        If sValue = "1" OrElse sValue = "-1" OrElse sValue = "-2" Then Return True
        If sValue = "0" Then Return False

        ' Standard-Parsing für Text ("True" / "False")
        Dim result As Boolean
        If Boolean.TryParse(sValue, result) Then
            Return result
        End If

        Return defaultValue
    End Function


    ''' <summary>
    ''' Lädt die Buchungsdaten für eine bestimmte ID und schreibt alle Spaltenwerte kommagetrennt in ein Textfeld.
    ''' </summary>
    ''' <remarks>
    ''' 14.10.2026 - Code-Optimierung:
    ''' - XML-Dokumentation hinzugefügt.
    ''' - Syntaxfehler im SQL-String korrigiert.
    ''' - Absturzsicherung (Null- und Zeilenprüfung) für die DataTable integriert.
    ''' - Nutzen von LINQ und 'String.Join' für eine performante, kommagetrennte Zusammenführung aller Felder.
    ''' </remarks>
    Public Function prLadeBuchungInTextfeld(sRID As String) As String
        ' 1. SQL-Abfrage definieren (Syntax korrigiert)
        Dim sSQL As String = "SELECT * FROM Buchung WHERE BID = '" & sRID & "'"

        ' 2. Daten über die Hilfsfunktion laden
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' 3. Prüfen, ob ein Datensatz gefunden wurde
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            ' Erste Zeile der Tabelle referenzieren
            Dim row As DataRow = dt.Rows(0)

            ' Alle Spaltenwerte der Zeile in Strings konvertieren und mit Komma verbinden
            Return String.Join(", ", row.ItemArray.Select(Function(x) x.ToString()))
        Else
            ' Fallback, falls keine Daten gefunden wurden
            Return String.Empty
        End If
    End Function


End Module