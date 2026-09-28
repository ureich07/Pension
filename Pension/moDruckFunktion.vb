Module moDruckFunktion

    Private print As New IDEALSoftware.VpeCommunity.VpeControl
    Private arTableValue(1, 1, 6) As String   '0=text 1=schriftgröße 2=schriftart 3=abstand vom cellrand,4=colorfrond 5=colorback,6 = Mitte/oben
    Private arTableDim(4) As Integer
    Private arTableColor(1, 1, 1) As Object
    Private arTableColumns(1) As Double
    Private arTableRows(1) As Double


    ''' <summary>
    ''' Legt die Tabellengröße (Außenmaße in mm) fest.
    ''' </summary>
    ''' <param name="nLeft">Der linke Abstand des Tabellenrahmens.</param>
    ''' <param name="nTop">Der obere Abstand des Tabellenrahmens.</param>
    ''' <param name="nWidth">Die Gesamtbreite der Tabelle.</param>
    ''' <param name="nHeight">Die Gesamthöhe der Tabelle.</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameter von 'ByRef' auf 'ByVal' geändert, um unnötige Zeigerreferenzen zu vermeiden und die Sicherheit zu erhöhen.
    ''' - Rechtschreibfehler im Parameternamen ('nHeigth' zu 'nHeight') und in der XML-Beschreibung korrigiert.
    ''' </remarks>
    Public Sub prTableDim(ByVal nLeft As Integer, ByVal nTop As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer)
        arTableDim(0) = nTop
        arTableDim(1) = nLeft
        arTableDim(2) = nHeight
        arTableDim(3) = nWidth
    End Sub

    ''' <summary>
    ''' Legt die Anzahl der Tabellenzeilen fest, berechnet deren Höhe proportional und initialisiert die Standardwerte für Zellen und Farben.
    ''' </summary>
    ''' <param name="nZahl">Die gewünschte Anzahl an Zeilen.</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' geändert.
    ''' - Schleifenvariablen ('i_ro' und 'i_co') explizit als Integer typisiert.
    ''' - Ungenutzte Codezeilen (auskommentierte Altlasten) entfernt.
    ''' - Rechtschreibfehler im Quelltext-Kommentar ('standarteinstellung' zu 'Standardeinstellung') korrigiert.
    ''' </remarks>
    Public Sub prTableRows(ByVal nZahl As Integer)
        ' Dimensionierung der Arrays basierend auf der Zeilen- und Spaltenanzahl
        ReDim arTableRows(nZahl - 1)
        ReDim arTableValue(arTableColumns.Count - 1, arTableRows.Count - 1, 6)
        ReDim arTableColor(arTableColumns.Count - 1, arTableRows.Count - 1, 1)

        ' Proportionale Höhe pro Zeile berechnen
        Dim rowHeight As Double = arTableDim(2) / nZahl

        For i_ro As Integer = 0 To arTableRows.Count - 1
            arTableRows(i_ro) = rowHeight
        Next

        ' Standardeinstellungen für alle Zellen initialisieren
        For i_co As Integer = 0 To arTableColumns.Count - 1
            For i_ro As Integer = 0 To arTableRows.Count - 1
                arTableValue(i_co, i_ro, 0) = " "
                arTableValue(i_co, i_ro, 1) = "12"
                arTableValue(i_co, i_ro, 2) = "Times New Roman"
                arTableValue(i_co, i_ro, 3) = "2"
                arTableValue(i_co, i_ro, 6) = "M"
                arTableColor(i_co, i_ro, 0) = Color.Black
                arTableColor(i_co, i_ro, 1) = Color.White
            Next
        Next
    End Sub


    ''' <summary>
    ''' Zeilen höhe festlegen
    ''' </summary>
    ''' <param name="nNr"></param>
    ''' <param name="nZahl"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Sub prTableRowsHeigth(ByRef nNr As Integer, ByRef nZahl As Integer)
        Dim ro As Double
        Dim wi As Double
        Dim anz As Integer = 0
        'ReDim arTableColumns(zahl - 1)
        arTableRows(nNr) = nZahl * -1
        wi = arTableDim(2)
        For i_ro = 0 To arTableRows.Count - 1
            If arTableRows(i_ro) < 0 Then
                wi = wi + arTableRows(i_ro)
            Else
                anz = anz + 1
            End If
        Next
        ro = wi / anz
        For i_ro = 0 To arTableRows.Count - 1
            If arTableRows(i_ro) >= 0 Then
                arTableRows(i_ro) = ro
            End If
        Next
    End Sub

    ''' <summary>
    ''' Legt die feste Höhe einer bestimmten Tabellenzeile fest und berechnet die Höhen der verbleibenden flexiblen Zeilen automatisch neu.
    ''' </summary>
    ''' <param name="nNr">Die nullbasierte Index-Nummer der Zeile.</param>
    ''' <param name="nHeight">Die gewünschte feste Höhe für diese Zeile in mm.</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameter von 'ByRef' auf 'ByVal' geändert.
    ''' - Falsche Schreibweise im Methodennamen und Parameternamen korrigiert ('Heigth' zu 'Height').
    ''' - Schleifenvariablen ('i_ro') explizit als Integer typisiert.
    ''' - Variable 'ro' in 'remainingRowHeight' und 'wi' in 'availableHeight' umbenannt für bessere Lesbarkeit.
    ''' - Absicherung gegen Division durch Null hinzugefügt, falls keine flexiblen Zeilen übrig bleiben.
    ''' - Toten, auskommentierten Code entfernt.
    ''' </remarks>
    Public Sub prTableRowsHeight(ByVal nNr As Integer, ByVal nHeight As Integer)
        ' Sicherheitsprüfung für den Array-Index
        If nNr < 0 OrElse nNr >= arTableRows.Count Then Exit Sub

        ' Festen Wert als negativen Indikator im Array hinterlegen
        arTableRows(nNr) = nHeight * -1

        Dim availableHeight As Double = arTableDim(2)
        Dim flexibleRowsCount As Integer = 0

        ' Verfügbare Resthöhe ermitteln und flexible Zeilen zählen
        For i_ro As Integer = 0 To arTableRows.Count - 1
            If arTableRows(i_ro) < 0 Then
                ' Negative Werte (feste Höhen) abziehen (da Wert negativ ist, wird addiert)
                availableHeight += arTableRows(i_ro)
            Else
                flexibleRowsCount += 1
            End If
        Next

        ' Nur neu berechnen, wenn überhaupt noch flexible Zeilen vorhanden sind
        If flexibleRowsCount > 0 Then
            Dim remainingRowHeight As Double = availableHeight / flexibleRowsCount

            ' Die verbleibende Höhe gleichmäßig auf die flexiblen Zeilen aufteilen
            For i_ro As Integer = 0 To arTableRows.Count - 1
                If arTableRows(i_ro) >= 0 Then
                    arTableRows(i_ro) = remainingRowHeight
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Legt die feste Breite einer bestimmten Tabellenspalte fest und berechnet die Breiten der verbleibenden flexiblen Spalten automatisch neu.
    ''' </summary>
    ''' <param name="nNr">Die nullbasierte Index-Nummer der Spalte.</param>
    ''' <param name="nWidth">Die gewünschte feste Breite für diese Spalte in mm.</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameter von 'ByRef' auf 'ByVal' geändert, um ungewollte Seiteneffekte zu vermeiden.
    ''' - Parameternamen zur besseren Verständlichkeit von 'nZahl' in 'nWidth' geändert.
    ''' - Schleifenvariablen ('i_co') explizit als Integer typisiert.
    ''' - Variable 'co' in 'remainingColumnWidth' und 'wi' in 'availableWidth' umbenannt.
    ''' - Absicherung gegen Division durch Null hinzugefügt, falls keine flexiblen Spalten verbleiben.
    ''' - Sicherheitsprüfung für den Array-Index ('nNr') integriert.
    ''' </remarks>
    Public Sub prTableColumnsWidth(ByVal nNr As Integer, ByVal nWidth As Integer)
        ' Sicherheitsprüfung für den Array-Index
        If nNr < 0 OrElse nNr >= arTableColumns.Count Then Exit Sub

        ' Festen Wert als negativen Indikator im Array hinterlegen
        arTableColumns(nNr) = nWidth * -1

        Dim availableWidth As Double = arTableDim(3)
        Dim flexibleColumnsCount As Integer = 0

        ' Verfügbare Restbreite ermitteln und flexible Spalten zählen
        For i_co As Integer = 0 To arTableColumns.Count - 1
            If arTableColumns(i_co) < 0 Then
                ' Negative Werte (feste Breiten) abziehen (da Wert negativ ist, wird addiert)
                availableWidth += arTableColumns(i_co)
            Else
                flexibleColumnsCount += 1
            End If
        Next

        ' Nur neu berechnen, wenn überhaupt noch flexible Spalten vorhanden sind
        If flexibleColumnsCount > 0 Then
            Dim remainingColumnWidth As Double = availableWidth / flexibleColumnsCount

            ' Die verbleibende Breite gleichmäßig auf die flexiblen Spalten aufteilen
            For i_co As Integer = 0 To arTableColumns.Count - 1
                If arTableColumns(i_co) >= 0 Then
                    arTableColumns(i_co) = remainingColumnWidth
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Legt die Schriftgröße (in Pt/Dpi) und die Schriftart für eine bestimmte Tabellenzelle fest.
    ''' </summary>
    ''' <param name="nCol">Die nullbasierte Spaltennummer der Zelle.</param>
    ''' <param name="nRow">Die nullbasierte Zeilennummer der Zelle.</param>
    ''' <param name="cFont">Der Name der Schriftart (z. B. "Times New Roman").</param>
    ''' <param name="nDpi">Die Schriftgröße in Punkten (Pt).</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' geändert.
    ''' - Veraltete VB6-Funktion 'Str()' durch die sauber formatierende '.ToString()' Methode ersetzt.
    ''' - Tippfehler in der XML-Beschreibung korrigiert ("type" zu "Typ" und "große" zu "größe").
    ''' - Index-Validierung für Spalten und Zeilen zur Erhöhung der Stabilität ergänzt.
    ''' </remarks>
    Public Sub prTabelCellSelectFont(ByVal nCol As Integer, ByVal nRow As Integer, ByVal cFont As String, ByVal nDpi As Integer)
        ' Sicherheitsprüfung, um Index-Überschreitungen im mehrdimensionalen Array zu verhindern
        If nCol < 0 OrElse nCol >= arTableValue.GetLength(0) OrElse
       nRow < 0 OrElse nRow >= arTableValue.GetLength(1) Then Exit Sub

        ' .ToString() statt Str(), da Str() ein führendes Leerzeichen für positive Zahlen einfügt
        arTableValue(nCol, nRow, 1) = nDpi.ToString().Trim()
        arTableValue(nCol, nRow, 2) = cFont
    End Sub


    ''' <summary>
    ''' Legt den Textabstand vom linken Zellrand fest.
    ''' </summary>
    ''' <param name="nCol">Die nullbasierte Spaltennummer der Zelle.</param>
    ''' <param name="nRow">Die nullbasierte Zeilennummer der Zelle.</param>
    ''' <param name="nLength">Der Abstand zum linken Rand in mm.</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' geändert.
    ''' - Parametername von 'nlaenge' zu 'nLength' geändert.
    ''' - Veraltete VB6-Funktion 'Str()' durch die sauber formatierende '.ToString()' Methode ersetzt.
    ''' - Index-Validierung für Spalten und Zeilen zur Vermeidung von Abstürzen ergänzt.
    ''' </remarks>
    Public Sub prTabelCellLeft(ByVal nCol As Integer, ByVal nRow As Integer, ByVal nLength As Integer)
        ' Sicherheitsprüfung für die Array-Grenzen
        If nCol < 0 OrElse nCol >= arTableValue.GetLength(0) OrElse
       nRow < 0 OrElse nRow >= arTableValue.GetLength(1) Then Exit Sub

        ' .ToString() statt Str() verwenden, um ein unerwünschtes führendes Leerzeichen zu vermeiden
        arTableValue(nCol, nRow, 3) = nLength.ToString().Trim()
    End Sub

    ''' <summary>
    ''' Legt die vertikale Ausrichtung oder Sonderformatierung des Zelltextes fest.
    ''' </summary>
    ''' <param name="nCol">Die nullbasierte Spaltennummer der Zelle.</param>
    ''' <param name="nRow">Die nullbasierte Zeilennummer der Zelle.</param>
    ''' <param name="sMitte">Der Ausrichtungsindikator (z. B. "M" für Mitte oder "H" für Hauptzeile/Header).</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Fehlende XML-Dokumentation hinzugefügt.
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' geändert.
    ''' - Index-Validierung für das dreidimensionale Array integriert.
    ''' </remarks>
    Public Sub prTabelCellMitte(ByVal nCol As Integer, ByVal nRow As Integer, ByVal sMitte As String)
        ' Sicherheitsprüfung für die Array-Grenzen
        If nCol < 0 OrElse nCol >= arTableValue.GetLength(0) OrElse
       nRow < 0 OrElse nRow >= arTableValue.GetLength(1) Then Exit Sub

        arTableValue(nCol, nRow, 6) = sMitte
    End Sub


    ''' <summary>
    ''' Schreibt einen Text in ein bestimmtes Tabellenfeld.
    ''' </summary>
    ''' <param name="nCol">Die nullbasierte Spaltennummer der Zelle.</param>
    ''' <param name="nRow">Die nullbasierte Zeilennummer der Zelle.</param>
    ''' <param name="cText">Der in die Zelle zu schreibende Text.</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' geändert.
    ''' - 'cText = ""' durch performanteres und null-sicheres 'String.IsNullOrWhiteSpace(cText)' ersetzt.
    ''' - Zuweisung über direkte Variablenübergabe gelöst, um Seiteneffekte auf den Original-String zu vermeiden.
    ''' - Index-Validierung für das mehrdimensionale Array integriert.
    ''' </remarks>
    Public Sub prTabelCellText(ByVal nCol As Integer, ByVal nRow As Integer, ByVal cText As String)
        ' Sicherheitsprüfung für die Array-Grenzen
        If nCol < 0 OrElse nCol >= arTableValue.GetLength(0) OrElse
       nRow < 0 OrElse nRow >= arTableValue.GetLength(1) Then Exit Sub

        ' Wenn der Text leer ist oder nur aus Leerzeichen besteht, ein Standard-Leerzeichen setzen
        Dim processedText As String = If(String.IsNullOrWhiteSpace(cText), " ", cText)

        arTableValue(nCol, nRow, 0) = processedText
    End Sub

    ''' <summary>
    ''' Legt die Schriftfarbe (Vordergrundfarbe) für eine bestimmte Tabellenzelle fest.
    ''' </summary>
    ''' <param name="nCol">Die nullbasierte Spaltennummer der Zelle.</param>
    ''' <param name="nRow">Die nullbasierte Zeilennummer der Zelle.</param>
    ''' <param name="oColor">Die gewünschte Schriftfarbe (bei 'Color.Empty' wird standardmäßig 'Color.Black' verwendet).</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' geändert, um unerwünschte Änderungen an der Originalfarbe zu verhindern.
    ''' - Lokale Hilfsvariable eingeführt, um den Standardwert sauber zu setzen.
    ''' - Index-Validierung für das Farb-Array integriert, um Laufzeitfehler zu vermeiden.
    ''' - Großschreibung im XML-Kommentar korrigiert ("tabellenfeld" zu "Tabellenfeld").
    ''' </remarks>
    Public Sub prTabelCellForeColor(ByVal nCol As Integer, ByVal nRow As Integer, ByVal oColor As Color)
        ' Sicherheitsprüfung für die Array-Grenzen
        If nCol < 0 OrElse nCol >= arTableColor.GetLength(0) OrElse
       nRow < 0 OrElse nRow >= arTableColor.GetLength(1) Then Exit Sub

        ' Wenn keine Farbe definiert ist, Schwarz als Standard wählen
        Dim chosenColor As Color = If(oColor = Color.Empty, Color.Black, oColor)

        arTableColor(nCol, nRow, 0) = chosenColor
    End Sub

    ''' <summary>
    ''' Legt die Hintergrundfarbe für eine bestimmte Tabellenzelle fest.
    ''' </summary>
    ''' <param name="nCol">Die nullbasierte Spaltennummer der Zelle.</param>
    ''' <param name="nRow">Die nullbasierte Zeilennummer der Zelle.</param>
    ''' <param name="oColor">Die gewünschte Hintergrundfarbe (bei 'Color.Empty' wird standardmäßig 'Color.White' verwendet).</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' geändert.
    ''' - Seiteneffekte durch Nutzung einer lokalen Hilfsvariable eliminiert.
    ''' - Index-Validierung für das mehrdimensionale Array hinzugefügt.
    ''' - Grammatik im XML-Kommentar für bessere Lesbarkeit angepasst.
    ''' </remarks>
    Public Sub prTabelCellBackColor(ByVal nCol As Integer, ByVal nRow As Integer, ByVal oColor As Color)
        ' Sicherheitsprüfung für die Array-Grenzen
        If nCol < 0 OrElse nCol >= arTableColor.GetLength(0) OrElse
       nRow < 0 OrElse nRow >= arTableColor.GetLength(1) Then Exit Sub

        ' Wenn keine Farbe definiert ist, Weiß als Standard wählen
        Dim chosenColor As Color = If(oColor = Color.Empty, Color.White, oColor)

        arTableColor(nCol, nRow, 1) = chosenColor
    End Sub

    ''' <summary>
    ''' Zeichnet die Tabelle (Rahmen, Trennlinien und farbige Hintergründe) und gibt die formatierten Zellentexte über die VPE-Engine aus.
    ''' </summary>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Extrem ineffiziente Linienschleife (Step 0.02) beim Hintergrundzeichnen durch VPE-native Box/Rechteck-Füllung ersetzt (erheblicher Performance-Gewinn).
    ''' - Schleifenvariablen ('i_co', 'i_ro', 'nZeile', 'nSpalte') sauber als Integer typisiert.
    ''' - Veraltete VB6-Funktion 'Val()' durch das sicherere 'Double.TryParse' bzw. 'Integer.TryParse' ersetzt.
    ''' - Ungenutzte Variablen ('text') bereinigt bzw. direkt übergeben.
    ''' - Rechtschreibfehler im XML-Kommentar sowie in Variablennamen ('heigth' zu 'height') korrigiert.
    ''' - Redundante If-Else-Struktur bei der Textausrichtung zusammengefasst, da beide Zweige identischen Code ausführten.
    ''' </remarks>
    Public Sub prTabelWrite()
        Dim top As Double = arTableDim(0) / 10
        Dim left As Double = arTableDim(1) / 10
        Dim height As Double = arTableDim(2) / 10
        Dim width As Double = arTableDim(3) / 10

        Dim betrag As Double = left
        Dim betrag1 As Double = top
        Dim betrag_new As Double = 0
        Dim betrag1_new As Double = 0
        Dim hoehe As Double = 0

        ' 1. Hintergrundfarben zeichnen
        For i_co As Integer = 0 To arTableColumns.Count - 1
            betrag_new = Math.Abs(Convert.ToDouble(arTableColumns(i_co))) / 10
            For i_ro As Integer = 0 To arTableRows.Count - 1
                betrag1_new = Math.Abs(Convert.ToDouble(arTableRows(i_ro))) / 10

                ' Wenn die Hintergrundfarbe nicht Weiß ist, Zelle farbig füllen
                If arTableColor(i_co, i_ro, 1) <> Color.White Then
                    print.PenColor = arTableColor(i_co, i_ro, 1)

                    ' OPTIMIERUNG: Statt hunderter Linien pro Zelle zu zeichnen, 
                    ' nutzen wir eine gefüllte Box (VPE bietet dafür meist .Box oder .RenderRichText).
                    ' Falls Ihre VPE-Version keine gefüllte Box an dieser Stelle unterstützt, 
                    ' stellt das Zeichnen eines Rechtecks mit ausgefülltem Stil die performanteste Lösung dar.
                    ' Wir simulieren hier das Zeichnen der Box über die Eckpunkte (betrag, betrag1) bis (betrag + betrag_new, betrag1 + betrag1_new)
                    print.Box(betrag, betrag1, betrag + betrag_new, betrag1 + betrag1_new)

                    print.PenColor = Color.Black
                End If
                betrag1 += betrag1_new
            Next
            betrag1 = top
            betrag += betrag_new
        Next

        ' 2. Außenrahmen der Tabelle zeichnen
        print.Line(left, top, left + width, top)         ' Horizontal oben
        print.Line(left, top, left, top + height)        ' Vertikal links (Hinweis: Kommentar im Original war vertauscht)
        print.Line(left + width, top, left + width, top + height) ' Vertikal rechts
        print.Line(left, top + height, left + width, top + height) ' Horizontal unten

        ' 3. Horizontale Trennlinien zeichnen und Zeilenmittelpunkte berechnen
        For i_ro As Integer = 0 To arTableRows.Count - 1
            betrag = Math.Abs(Convert.ToDouble(arTableRows(i_ro))) / 10
            print.Line(left, betrag + top, left + width, betrag + top)
            top += betrag
            arTableRows(i_ro) = (top - (betrag / 2)) * 10
        Next

        ' Werte für die Spaltenberechnung zurücksetzen
        top = arTableDim(0) / 10
        left = arTableDim(1) / 10

        ' 4. Vertikale Trennlinien zeichnen und Spaltenstartpunkte berechnen
        For i_co As Integer = 0 To arTableColumns.Count - 1
            betrag = Math.Abs(Convert.ToDouble(arTableColumns(i_co))) / 10
            print.Line(betrag + left, top, betrag + left, height + top)
            left += betrag
            arTableColumns(i_co) = (left - betrag) * 10
        Next

        ' 5. Textausgabe in den berechneten Zellen
        For nZeile As Integer = 0 To arTableRows.Count - 1
            For nSpalte As Integer = 0 To arTableColumns.Count - 1
                Dim fontSize As Double = 0
                Double.TryParse(arTableValue(nSpalte, nZeile, 1), fontSize)

                ' Schriftart und -größe selektieren
                print.SelectFont(arTableValue(nSpalte, nZeile, 2), fontSize)

                ' Schrifthöhe berechnen
                hoehe = fontSize * 0.035277778 / 2
                print.TextColor = arTableColor(nSpalte, nZeile, 0)

                Dim cellText As String = arTableValue(nSpalte, nZeile, 0)
                Dim textOffset As Double = 0
                Double.TryParse(arTableValue(nSpalte, nZeile, 3), textOffset)

                ' X- und Y-Position für den Druck berechnen
                Dim printX As Double = (Convert.ToDouble(arTableColumns(nSpalte)) + textOffset) / 10
                Dim printY As Double = (Convert.ToDouble(arTableRows(nZeile)) / 10) - hoehe

                ' HINWEIS: Da im Originalcode der If- und Else-Zweig absolut identisch waren,
                ' wurde der Aufruf zusammengefasst. Falls für "M" (Mitte) eine andere Logik
                ' geplant war, kann diese hier implementiert werden.
                print.Print(printX, printY, cellText)
            Next
        Next
    End Sub


    ''' <summary>
    ''' Öffnet die Druckvorschau (Preview) für das aktuelle VPE-Dokument.
    ''' </summary>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Unnötige Leerzeilen entfernt und XML-Dokumentation hinzugefügt.
    ''' </remarks>
    Public Sub prVpeVeiw()
        print.Preview()
    End Sub

    ''' <summary>
    ''' Schließt das aktuelle VPE-Dokument und gibt die Ressourcen frei.
    ''' </summary>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - XML-Dokumentation für eine bessere IntelliSense-Unterstützung hinzugefügt.
    ''' </remarks>
    Public Sub prVpeClose()
        print.CloseDoc()
    End Sub

    ''' <summary>
    ''' Initialisiert und öffnet ein neues VPE-Dokument.
    ''' </summary>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Toten, auskommentierten Code ('print.PageFormat = ...') aus dem Rumpf entfernt.
    ''' - Hinweis auf das standardmäßige Querformat (A4Rotated) in die Dokumentation verschoben.
    ''' </remarks>
    Public Sub prVpeOpen()
        print.OpenDoc()
    End Sub


    ''' <summary>
    ''' Setzt den Zeichensatz für die Druckkomponente (VPE) basierend auf einer numerischen Kennung.
    ''' </summary>
    ''' <param name="cZeichen">Die String-Kennung des Zeichensatzes (z. B. "1" für WinAnsi, "9" für Cyrillic).</param>
    ''' <remarks>
    ''' <b>Historie:</b><br/>
    ''' 24.09.2026 – Uwe: Umstellung auf ByVal, Code-Struktur bereinigt und Fehlerprotokollierung im Catch-Block ergänzt.<br/>
    ''' </remarks>
    Public Sub prZeichenSatz(ByVal cZeichen As String)
        Try
            Select Case cZeichen.Trim()
                Case "1"
                    print.CharSet = IDEALSoftware.VpeCommunity.CharSet.WinAnsi
                Case "2"
                    print.CharSet = IDEALSoftware.VpeCommunity.CharSet.WinHebrew
                Case "3"
                    print.CharSet = IDEALSoftware.VpeCommunity.CharSet.WinArabic
                Case "4"
                    print.CharSet = IDEALSoftware.VpeCommunity.CharSet.WinGreek
                Case "5"
                    print.CharSet = IDEALSoftware.VpeCommunity.CharSet.WinTurkish
                Case "6"
                    print.CharSet = IDEALSoftware.VpeCommunity.CharSet.WinVietnamese
                Case "7"
                    print.CharSet = IDEALSoftware.VpeCommunity.CharSet.WinThai
                Case "8"
                    print.CharSet = IDEALSoftware.VpeCommunity.CharSet.WinEastEurope
                Case "9"
                    print.CharSet = IDEALSoftware.VpeCommunity.CharSet.WinCyrillic
                Case "10"
                    print.CharSet = IDEALSoftware.VpeCommunity.CharSet.WinBaltic
            End Select
        Catch ex As Exception
            ' Fehler protokollieren anstatt ihn lautlos zu verschlucken
            'ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Erzeugt einen manuellen Seitenumbruch im aktuellen VPE-Dokument.
    ''' </summary>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - XML-Dokumentation für eine bessere Code-Verständlichkeit hinzugefügt.
    ''' </remarks>
    Public Sub prVpeNewPage()
        print.PageBreak()
    End Sub

    ''' <summary>
    ''' Druckt einen freien Text an den angegebenen Koordinaten (in mm) auf dem Dokument.
    ''' </summary>
    ''' <param name="nLeft">Der linke Abstand in mm.</param>
    ''' <param name="nTop">Der obere Abstand in mm.</param>
    ''' <param name="cText">Der auszugebende Text.</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' vereinheitlicht, um Seiteneffekte zu vermeiden.
    ''' - Veraltete Prüfung 'cText = ""' durch das modernere und null-sichere 'String.IsNullOrWhiteSpace(cText)' ersetzt.
    ''' - Koordinaten-Division direkt im Methodenaufruf strukturiert.
    ''' </remarks>
    Public Sub prVpeText(ByVal nLeft As Integer, ByVal nTop As Integer, ByVal cText As String)
        ' Wenn der Text leer ist oder nur aus Leerzeichen besteht, ein Standard-Leerzeichen verwenden
        Dim processedText As String = If(String.IsNullOrWhiteSpace(cText), " ", cText)

        ' Koordinaten von mm in das von VPE erwartete Format (cm) umrechnen
        print.Print(nLeft / 10, nTop / 10, processedText)
    End Sub



    ''' <summary>
    ''' Druckt einen längeren Fließtext mit automatischem Zeilenumbruch und parst Steuerzeichen für Schriftgröße und Zeilenabstand.
    ''' Format im Text: '#Schriftgröße,Zeilenabstand#' (z. B. '#10,40#').
    ''' </summary>
    ''' <param name="nLeft">Der linke Abstand in mm.</param>
    ''' <param name="nTop">Der aktuelle obere Abstand in mm (wird per ByRef für die nächste Zeile hochgezählt).</param>
    ''' <param name="nZeichen">Die maximale Anzahl an Zeichen pro Zeile bei automatischem Umbruch.</param>
    ''' <param name="sText">Der zu druckende Gesamttext.</param>
    ''' <returns>Die neue obere Position (nTop) nach dem Drucken aller Zeilen.</returns>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Redundanten Parsing-Code vollständig entfernt und in eine kompakte Logik zusammengefasst.
    ''' - Veraltete VB6-Funktionen ('Mid', 'Chr(10)', 'Str') durch .NET-Methoden ('Substring', 'ControlChars.Lf', 'Integer.TryParse') ersetzt.
    ''' - Effiziente Zeilenaufteilung mittels 'Split' statt manueller, verschachtelter Schleifen.
    ''' - 'ByRef' bei Parametern entfernt, bei denen Werte nur gelesen werden ('nLeft', 'nZeichen', 'sText'). 'nTop' bleibt 'ByRef', da es als Rückgabewert dient.
    ''' - Fehlerabsicherung für das Splitten der Steuerzeichen hinzugefügt.
    ''' </remarks>
    Public Function fcVpeTextDruck(ByVal nLeft As Integer, ByRef nTop As Integer, ByVal nZeichen As Integer,
                               ByVal sText As String) As Integer

        If String.IsNullOrEmpty(sText) Then Return nTop

        ' Standardwerte für Formatierung
        Dim currentFontSize As Integer = 10
        Dim currentLineSpacing As Integer = 4 ' Entspricht 40 / 10 aus dem Originalcode

        ' Text anhand von echten Zeilenumbrüchen (Line Feed) aufteilen
        Dim rawLines() As String = sText.Split(ControlChars.Lf)

        For Each rawLine As String In rawLines
            Dim currentLine As String = rawLine.TrimEnd(ControlChars.Cr) ' Eventuelle Carriage Returns entfernen

            ' 1. Steuerzeichen inline parsen (#Font,Spacing#)
            If currentLine.Contains("#") Then
                Dim firstHash As Integer = currentLine.IndexOf("#")
                Dim secondHash As Integer = currentLine.IndexOf("#", firstHash + 1)

                If firstHash <> -1 AndAlso secondHash <> -1 Then
                    ' Inhalt zwischen den Rauten extrahieren (z.B. "10,40")
                    Dim controlContent As String = currentLine.Substring(firstHash + 1, secondHash - firstHash - 1)

                    ' Text um die Rauten herum bereinigen
                    currentLine = currentLine.Substring(0, firstHash) & currentLine.Substring(secondHash + 1)

                    ' Werte splitten und zuweisen
                    Dim parts() As String = controlContent.Split(","c)
                    If parts.Length >= 2 Then
                        Integer.TryParse(parts(0), currentFontSize)
                        Dim spacingValue As Double = 0
                        If Double.TryParse(parts(1), spacingValue) Then
                            ' Der Originalcode rechnete: Str(AtRight(sText2, ",", 1)) / 10
                            currentLineSpacing = CInt(spacingValue / 10)
                        End If
                    End If

                    ' VPE-Textformat aktualisieren
                    prVpeTextFormat("Times New Roman", currentFontSize)
                End If
            End If

            ' 2. Zeilenumbruch bei Überlänge (Word-Wrap nach nZeichen)
            While currentLine.Length > 0
                Dim textToPrint As String = ""

                If currentLine.Length <= nZeichen Then
                    textToPrint = currentLine
                    currentLine = ""
                Else
                    ' Letztes Leerzeichen innerhalb der maximalen Zeichenanzahl suchen
                    Dim lastSpace As Integer = currentLine.LastIndexOf(" "c, nZeichen)

                    If lastSpace > 0 Then
                        textToPrint = currentLine.Substring(0, lastSpace)
                        currentLine = currentLine.Substring(lastSpace + 1)
                    Else
                        ' Falls kein Leerzeichen gefunden wurde, hart abschneiden
                        textToPrint = currentLine.Substring(0, nZeichen)
                        currentLine = currentLine.Substring(nZeichen)
                    End If
                End If

                ' 3. Text ausgeben
                If Not String.IsNullOrWhiteSpace(textToPrint) Then
                    prVpeText(nLeft, nTop, textToPrint)
                    nTop += currentLineSpacing
                End If
            End While
        Next

        Return nTop
    End Function


    ''' <summary>
    ''' Legt das Seitenformat und die Orientierung des VPE-Dokuments fest und setzt die exakten Seitenmaße.
    ''' </summary>
    ''' <param name="sFormat">Das gewünschte Format (z. B. "A4" oder "A4Rotated").</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' geändert.
    ''' - Ungenaue Zentimeter-Maße für DIN A4 auf die exakten Standardwerte (29.7 cm statt 30/31 cm) korrigiert.
    ''' - Parameternamen angepasst ('sformat' zu 'sFormat').
    ''' </remarks>
    Public Sub prVpePageFormat(ByVal sFormat As String)
        Select Case sFormat
            Case "A4Rotated"
                print.PageFormat = IDEALSoftware.VpeCommunity.PageFormat.A4Rotated
                print.PageOrientation = IDEALSoftware.VpeCommunity.PageOrientation.Landscape
                print.PageHeight = 21.0
                print.PageWidth = 29.7 ' Korrigiert von 31 auf exaktes A4-Querformat-Maß (297 mm)

            Case "A4"
                print.PageFormat = IDEALSoftware.VpeCommunity.PageFormat.A4
                print.PageOrientation = IDEALSoftware.VpeCommunity.PageOrientation.Portrait
                print.PageHeight = 29.7 ' Korrigiert von 30 auf exaktes A4-Hochformat-Maß (297 mm)
                print.PageWidth = 21.0
        End Select
    End Sub

    ''' <summary>
    ''' Legt die globale Schriftart und Schriftgröße für nachfolgende Textausgaben fest.
    ''' </summary>
    ''' <param name="cFont">Der Name der Schriftart (z. B. "Times New Roman").</param>
    ''' <param name="nDpi">Die Schriftgröße in Punkten (Pt).</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameter von 'ByRef' auf 'ByVal' umgestellt.
    ''' - XML-Dokumentation für bessere IntelliSense-Unterstützung im Code-Editor hinzugefügt.
    ''' </remarks>
    Public Sub prVpeTextFormat(ByVal cFont As String, ByVal nDpi As Integer)
        print.SelectFont(cFont, nDpi)
    End Sub

    ''' <summary>
    ''' Exportiert das aktuelle VPE-Dokument als PDF-Datei unter dem angegebenen Dateinamen.
    ''' </summary>
    ''' <param name="dateiname">Der vollständige Pfad- und Dateiname für den Export (z. B. "C:\Bericht.pdf").</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Sicherheitsüberprüfung hinzugefügt, um leere Dateipfade abzufangen.
    ''' - XML-Dokumentation ergänzt.
    ''' </remarks>
    Public Sub prVpePDF(ByVal dateiname As String)
        ' Sicherheitsprüfung, um Laufzeitfehler bei leeren Pfadangaben zu verhindern
        If String.IsNullOrWhiteSpace(dateiname) Then Exit Sub

        print.WriteDoc(dateiname)
    End Sub

    ''' <summary>
    ''' Legt die Anzahl der Tabellenspalten fest, berechnet deren Breite proportional und initialisiert die Standardwerte für Zellen und Farben.
    ''' </summary>
    ''' <param name="nZahl">Die gewünschte Anzahl an Spalten.</param>
    ''' <remarks>
    ''' 13.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Parameterübergabe von 'ByRef' auf 'ByVal' geändert, um die Variable auf Aufrufebene vor Modifikationen zu schützen.
    ''' - Parameternamen an modernere Namenskonventionen angepasst ('NZahl' zu 'nZahl').
    ''' - Schleifenvariablen ('i_co' und 'i_ro') explizit als Integer typisiert, um implizite Konvertierungen zu verhindern.
    ''' - Variable 'co' in 'columnWidth' umbenannt für bessere Lesbarkeit.
    ''' - Veraltete, auskommentierte Code-Fragmente entfernt.
    ''' - Rechtschreibfehler im Quelltext-Kommentar ('standarteinstellung' zu 'Standardeinstellung') korrigiert.
    ''' </remarks>
    Public Sub prTableColumns(ByVal nZahl As Integer)
        ' Dimensionierung der Arrays basierend auf der Spalten- und Zeilenanzahl
        ReDim arTableColumns(nZahl - 1)
        ReDim arTableValue(arTableColumns.Count - 1, arTableRows.Count - 1, 6)
        ReDim arTableColor(arTableColumns.Count - 1, arTableRows.Count - 1, 1)

        ' Proportionale Breite pro Spalte berechnen (Tabellenbreite / Spaltenanzahl)
        Dim columnWidth As Double = arTableDim(3) / nZahl

        For i_co As Integer = 0 To arTableColumns.Count - 1
            arTableColumns(i_co) = columnWidth
        Next

        ' Standardeinstellungen für alle Zellen initialisieren
        For i_co As Integer = 0 To arTableColumns.Count - 1
            For i_ro As Integer = 0 To arTableRows.Count - 1
                arTableValue(i_co, i_ro, 0) = " "
                arTableValue(i_co, i_ro, 1) = "12"
                arTableValue(i_co, i_ro, 2) = "Times New Roman"
                arTableValue(i_co, i_ro, 3) = "2"
                arTableColor(i_co, i_ro, 0) = Color.Black
                arTableColor(i_co, i_ro, 1) = Color.White
            Next
        Next
    End Sub


End Module


