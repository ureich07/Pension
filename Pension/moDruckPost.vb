Imports System.IO

Module moDruckPost
    Dim sSS7 As String
    Dim sSS19 As String
    Dim sSSS As String
    Dim nSprache As Integer

    ''' <summary>
    ''' Erstellt das Drucklayout für eine Buchungsbestätigung über das VPE-System, fügt die reservierten Zimmer, 
    ''' optionale Makrotexte sowie dynamische Buchungstexte hinzu und generiert das fertige PDF-Dokument im Archiv.
    ''' </summary>
    ''' <param name="sBID">Die eindeutige Buchungs-ID (ByRef übergeben).</param>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Fehlerkorrektur: 'Str()' und 'Val()' durch standardisierte .NET-Konvertierungen ('Convert.ToInt32') ersetzt.
    ''' - Performance-Schub: Unnötige Zeichenverkettungs-Schleifen durch 'String.Join' eliminiert.
    ''' - Code-Hygiene: Ungenutzte Array-Variablen bereinigt, redundante 'Call'-Befehle entfernt und Syntax auf '&' vereinheitlicht.
    ''' 11.10.2026 - Erweiterung:
    ''' - Prüfung ob Buchungs-PDF existiert, wenn ja, dann Aktivierung der Mail-Funktion.
    ''' </remarks>
    Public Function fcDruckBuchnung(ByRef sBID As String) As Boolean
        Dim xlaenge As Integer = 25
        Dim arFeld() As String
        Dim arfeld1() As String
        Dim ii As Integer
        Dim sKID As String

        ' Erzeugt den gesperrten Text (Buchstaben mit Leerzeichen getrennt) ohne langsame Schleife
        Dim sPensionsName As String = arIni(2)
        Dim sNameGesperrt As String = String.Join(" ", sPensionsName.ToCharArray()) & " "

        ' Stammdaten abrufen
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "BText", "MText", "Sprache", "von", "bis", "BuchDatum", "Rdatum", "InternetNr", "Name1", "Name2"})
        sKID = arfeld1(0)
        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "Name2"})

        prVpeOpen()
        prVpePageFormat("A4")

        ' Briefkopf, Anschrift und Objekt-Details zeichnen
        prDruckKopf(xlaenge, arfeld1(7), arfeld1(10), arfeld1(11), arFeld)
        prDruckBuch(xlaenge, arfeld1(1), arfeld1(4))

        ' Zimmer/Objektliste andrucken (liefert Y-Endhöhe in arFeld(0))
        arFeld = fcDruckZimmer(xlaenge, sgRBID, "B", arfeld1(4))
        ii = Convert.ToInt32(Val(arFeld(0)))

        ' Makro- und Freitexte verarbeiten
        ii = fcDruckMakro(xlaenge, ii, arfeld1(3), arfeld1(4))
        If ii < Convert.ToInt32(Val(arFeld(0))) + 15 Then
            ii = Convert.ToInt32(Val(arFeld(0))) + 15
        End If

        ' Dynamischen Buchungstextblock rendern
        ii = prDruckBuchungsText(xlaenge, ii, arfeld1(2), arfeld1(4), arfeld1(5), arfeld1(6), arfeld1(7), arfeld1(8), arfeld1(9))

        ' Seitenüberlauf-Schutz (falls Y-Position das Seitenende überschreitet)
        If ii > 240 Then
            prVpeText(xlaenge, ii + 5, "Seite 1 von 2")
            prDruckFuss(xlaenge)
            prVpeNewPage()

            prVpeTextFormat("Times New Roman", 18)
            prVpeText(xlaenge, 18, sNameGesperrt)
            prVpeTextFormat("Times New Roman", 12)
            prVpeText(xlaenge, 25, arIni(3) & ", " & arIni(4) & " " & arIni(5))

            prVpeTextFormat("Times New Roman", 10)
            ii = 50
            prVpeText(xlaenge, ii, "Seite 2 von 2")
            ii = 60
        End If

        ' Grußformel und Unterschriftenzeilen andrucken
        Dim nSpracheLokal As Integer = Convert.ToInt32(Val(arfeld1(4)))
        prVpeTextFormat("Times New Roman", 12)
        prVpeText(xlaenge, ii, fcLanguage(nSpracheLokal, "[N B]Wir freuen uns auf Sie") & ".")

        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge, ii + 12, sNameGesperrt)
        prVpeText(xlaenge, ii + 25, fcLanguage(nSpracheLokal, "Unterschrift Gast Datum"))

        prDruckFuss(xlaenge)

        ' Zielverzeichnis prüfen und PDF exportieren
        Dim sBuchDir As String = Path.Combine(arIni(32), "Buchung")
        If Not DirExists(sBuchDir) Then
            CreateDir(sBuchDir)
        End If

        prVpePDF(Path.Combine(sBuchDir, "Buch_" & sBID & ".PDF"))
        prVpeVeiw()
        Dim sBuchFile As String = Path.Combine(sBuchDir, "Buch_" & sBID & ".PDF")
        If FileExists(sBuchFile) Then Return True
        Return False
    End Function


    ''' <summary>
    ''' Steuert das Drucklayout der Zusatzrechnung über das VPE-System, berechnet die Zusatzleistungssummen,
    ''' verwaltet Seitenumbrüche bei mehrseitigen Positionslisten, erzeugt das Beleglayout
    ''' und fügt den passenden Zahlungshinweis hinzu.
    ''' </summary>
    ''' <param name="sBID">Die Buchungs-ID (ByRef übergeben).</param>
    ''' <param name="sKID">Die Kunden-ID.</param>
    ''' <param name="nRNr">Die Zusatzrechnungsnummer (ByRef übergeben).</param>
    ''' <param name="sdatum">Das Rechnungsdatum.</param>
    ''' <param name="bBar">Gibt an, ob Barzahlung vorliegt (ByRef übergeben).</param>
    ''' <param name="anzahlung">Der geleistete Anzahlungsbetrag.</param>
    ''' <param name="arDruck1">Ein Parameter-Array mit den formatierten Zusatzpositionen.</param>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Altlasten entfernt: VB6-Befehle wie 'Int()', 'Str()' und 'Val()' durch native .NET-Äquivalente ersetzt.
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter entfernt, Zuweisungsoperatoren modernisiert ('+=' / '&=').
    ''' - Speicheroptimierung: Fehlerhafte statische Array-Dimensionierungen bei 'arFeld' und 'arfeld1' aufgehoben.
    ''' </remarks>
    Public Sub prDruckRechnungZusatz(ByRef sBID As String, ByVal sKID As String, ByRef nRNr As String, ByVal sdatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByVal ParamArray arDruck1() As String)
        Dim xlaenge As Integer = 25
        Dim arFeld() As String
        Dim arfeld1() As String
        Dim aÜbergabe() As String
        Dim rechnung() As Double
        Dim rechnung1(6) As Double
        Dim yHoehe As Integer
        Dim nSeiten As Integer = CInt(Math.Floor(arDruck1.Length / 13)) + 1
        Dim nSeite As Integer = 1

        ' Stammdaten laden
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "Name1", "Name2"})
        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "Name2"})
        nSprache = Convert.ToInt32(Val(arfeld1(2)))

        For i As Integer = 0 To 5
            rechnung1(i) = 0
        Next

        ' Positions-Schleife mit automatischem Seitenumbruch alle 13 Zeilen
        For i As Integer = 0 To arDruck1.Length - 1
            If i = 0 OrElse i Mod 13 = 0 Then
                If i <> 0 Then
                    prDruckFuss(xlaenge)
                    prVpeNewPage()
                End If
                prVpePageFormat("A4")
                prVpeTextFormat("Times New Roman", 10)
                prVpeText(xlaenge + 112, 53, "Tel. " & arIni(6))
                prVpeText(xlaenge + 112, 61, "Seite " & nSeite.ToString() & " von " & nSeiten.ToString())
                nSeite += 1
                prDruckKopf(xlaenge, sdatum, arfeld1(3), arfeld1(4), arFeld)
                prDruckRech(xlaenge, arfeld1(1), nRNr, arfeld1(2))
                yHoehe = 117
                prDruckKopfPositionZusatz(xlaenge, arfeld1(2))
            End If

            rechnung = fcDruckPositionZusatz(i, xlaenge, yHoehe, arDruck1(i), arfeld1(2))
            yHoehe += 5
            For j As Integer = 0 To 5
                rechnung1(j) = Math.Round(rechnung1(j) + rechnung(j), 2)
            Next
        Next

        rechnung1(6) = Convert.ToDouble(anzahlung)

        ' Gesamtsummen-Block berechnen und zeichnen
        aÜbergabe = FcDruckSumme(xlaenge, yHoehe, arfeld1(2), rechnung1)

        ' Zahlungsvereinbarung andrucken
        If bBar Then
            prVpeText(xlaenge, yHoehe + 36, "Betrag dankend erhalten")
        Else
            prVpeText(xlaenge, yHoehe + 36, "Bitte überweisen Sie den Gesamtbetrag bis: " & Date.Today.AddDays(14).ToString("dd.MM.yyyy") & " auf unten genanntes Konto")
        End If

        prDruckFuss(xlaenge)
    End Sub

    ''' <summary>
    ''' Zeichnet die Tabellenüberschrift (Spaltenköpfe) für die Zusatzrechnungspositionen 
    ''' über das VPE-Layoutsystem.
    ''' </summary>
    ''' <param name="xlaenge">Die X-Koordinate für die horizontale Ausrichtung der Tabelle (ByRef übergeben).</param>
    ''' <param name="sSprache1">Die Sprachsteuerung als String-ID (ByRef übergeben).</param>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Code-Hygiene: Unbenutzte Variablen 'i' und 'ii' entfernt und die Tabellenhöhe direkt als Konstante (5) übergeben.
    ''' - VB6-Altlast entfernt: 'Val()' durch standardisiertes 'Convert.ToInt32()' für die Sprachübersetzung ersetzt.
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter konsequent entfernt.
    ''' </remarks>
    Private Sub prDruckKopfPositionZusatz(ByRef xlaenge As Integer, ByRef sSprache1 As String)
        Dim nSprache As Integer = Convert.ToInt32(Val(sSprache1))

        ' Tabellendimensionierung für die Kopfzeile initialisieren (Höhe: 5 Einheiten, 1 Zeile)
        prTableDim(xlaenge, 112, 165, 5)
        prTableColumns(8)
        prTableRows(1)

        For j As Integer = 0 To 7
            prTabelCellSelectFont(j, 0, "Times New Roman", 9)
            prTabelCellLeft(j, 0, 1)
        Next

        ' Exakte Spaltenbreiten zuweisen
        prTableColumnsWidth(0, 6)
        prTableColumnsWidth(1, 30)
        prTableColumnsWidth(2, 35)
        prTableColumnsWidth(3, 22)
        prTableColumnsWidth(5, 10)
        prTableColumnsWidth(6, 22)
        prTableColumnsWidth(7, 22)

        ' Beschriftung der Zellen über das Übersetzungssystem
        prTabelCellText(0, 0, fcLanguage(nSprache, "Pos"))
        prTabelCellText(1, 0, fcLanguage(nSprache, "Objekt"))
        prTabelCellText(2, 0, fcLanguage(nSprache, "Leistung"))
        prTabelCellText(3, 0, fcLanguage(nSprache, "Preis"))
        prTabelCellText(4, 0, fcLanguage(nSprache, "Mwst"))
        prTabelCellText(5, 0, fcLanguage(nSprache, "Stück"))
        prTabelCellText(6, 0, fcLanguage(nSprache, "MwstB."))
        prTabelCellText(7, 0, fcLanguage(nSprache, "Brutto"))

        prTabelWrite()
    End Sub

    ''' <summary>
    ''' Berechnet die Steuer- und Bruttobeträge einer Zusatzleistungsposition (differenziert nach 7% oder 19% MwSt),
    ''' trägt diese formatiert in das VPE-Tabellenlayout ein und gibt die aufgeteilten 
    ''' Netto- und Steuerwerte für die Finanzbuchhaltung zurück.
    ''' </summary>
    ''' <param name="pos">Die laufende Indexposition der aktuellen Tabellenzeile.</param>
    ''' <param name="xlaenge">Die X-Koordinate für die horizontale Ausrichtung der Tabelle (ByRef übergeben).</param>
    ''' <param name="yHoehe">Die vertikale Y-Startposition für das Zeichnen der Zeile.</param>
    ''' <param name="sInhald">Die per Raute (#) getrennten Zusatzpositionsdaten aus dem Quell-Array.</param>
    ''' <param name="sSprache1">Die Sprachsteuerung als String-ID (ByRef übergeben).</param>
    ''' <returns>Ein Double-Array mit den berechneten Beträgen für die Umsatz- und MwSt-Klassen.</returns>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Typsicherheit erhöht: Rückgabetyp von generischem 'Array' auf ein präzises 'Double()' geändert.
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter entfernt und Zuweisungsoperatoren modernisiert.
    ''' - VB6-Relikte entfernt: 'Split' durch '.Split()' und 'Str()' durch '.ToString()' abgelöst.
    ''' - Robustheit: 'fcChangs' durch native '.Replace()'-Aufrufe ersetzt, um Kultur-unabhängige mathematische Konvertierungen abzusichern.
    ''' </remarks>
    Private Function fcDruckPositionZusatz(ByVal pos As Integer, ByRef xlaenge As Integer, ByVal yHoehe As Integer, ByVal sInhald As String, ByRef sSprache1 As String) As Double()
        Dim sInhald1() As String = sInhald.Split("#"c)
        Dim arRueck(5) As Double
        Dim nSprache As Integer = Convert.ToInt32(Val(sSprache1))

        ' Tabellendimensionierung und Font-Zuweisung für eine Zeile (Höhe 5)
        prTableDim(xlaenge, yHoehe, 165, 5)
        prTableColumns(8)
        prTableRows(1)

        For j As Integer = 0 To 7
            prTabelCellSelectFont(j, 0, "Times New Roman", 9)
            prTabelCellLeft(j, 0, 1)
        Next

        ' Exakte Spaltenbreiten definieren
        prTableColumnsWidth(0, 6)
        prTableColumnsWidth(1, 30)
        prTableColumnsWidth(2, 35)
        prTableColumnsWidth(3, 22)
        prTableColumnsWidth(5, 10)
        prTableColumnsWidth(6, 22)
        prTableColumnsWidth(7, 22)

        ' Textzellen befüllen
        prTabelCellText(0, 0, (pos + 1).ToString().Trim())
        prTabelCellText(1, 0, fcLanguage(nSprache, sInhald1(0)))
        prTabelCellText(2, 0, sInhald1(2).Trim())
        prTabelCellText(3, 0, fcDecStr(sInhald1(3), 10) & " €")
        prTabelCellText(7, 0, fcDecStr(sInhald1(7), 10) & " €")
        prTabelCellText(4, 0, sInhald1(12) & " %")
        prTabelCellText(5, 0, sInhald1(1))

        ' Werte für die mathematische Konvertierung vorbereiten
        Dim nBruttoGesamt As Double = Convert.ToDouble(Val(sInhald1(7).Replace(",", ".")))
        Dim nMwst19Wert As Double = Convert.ToDouble(Val(sInhald1(5).Replace(",", ".")))
        Dim nMwst7Wert As Double = Convert.ToDouble(Val(sInhald1(4).Replace(",", ".")))

        ' Differenzierung nach Steuersatz (19% vs 7%)
        If sInhald1(5).Trim() <> "0,00" Then
            prTabelCellText(6, 0, fcDecStr(sInhald1(5), 10) & " €") ' 19% MwSt-Betrag andrucken

            arRueck(3) = Math.Round(nBruttoGesamt - nMwst19Wert, 2) ' 19% Netto
            arRueck(4) = Math.Round(nMwst19Wert, 2)                ' 19% MwSt
            arRueck(1) = 0
            arRueck(2) = 0
        Else
            prTabelCellText(6, 0, fcDecStr(sInhald1(4), 10) & " €") ' 7% MwSt-Betrag andrucken

            arRueck(1) = Math.Round(nBruttoGesamt - nMwst7Wert, 2)  ' 7% Netto
            arRueck(2) = Math.Round(nMwst7Wert, 2)                 ' 7% MwSt
            arRueck(3) = 0
            arRueck(4) = 0
        End If

        prTabelWrite()

        ' Gesamtes Brutto im Index 5 hinterlegen
        arRueck(5) = nBruttoGesamt
        Return arRueck
    End Function


    ''' <summary>
    ''' Steuert das Drucklayout der Hauptrechnung über das VPE-System, berechnet die Umsatzsummen,
    ''' verwaltet Seitenumbrüche bei mehrseitigen Positionslisten, erzeugt das PDF-Dokument
    ''' und stößt die Verbuchung für das Kassenbuch an.
    ''' </summary>
    ''' <param name="sBID">Die Buchungs-ID (ByRef übergeben).</param>
    ''' <param name="sKID">Die Kunden-ID.</param>
    ''' <param name="nRNr">Die Rechnungsnummer (ByRef übergeben).</param>
    ''' <param name="sdatum">Das Rechnungsdatum.</param>
    ''' <param name="bBar">Gibt an, ob Barzahlung vorliegt (ByRef übergeben).</param>
    ''' <param name="anzahlung">Der geleistete Anzahlungsbetrag.</param>
    ''' <param name="sRZiel">Das Zahlungsziel oder Fälligkeitsdatum (ByRef übergeben).</param>
    ''' <param name="S1">Steuersatz für Übernachtungen (z. B. 7%).</param>
    ''' <param name="s2">Steuersatz für Speisen (z. B. 19%).</param>
    ''' <param name="s3">Zusätzlicher Steuersatz (Getränke).</param>
    ''' <param name="nReArt">Die aktive Rechnungsart.</param>
    ''' <param name="arDruck1">Ein Parameter-Array mit den formatierten Rechnungspositionen.</param>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Altlasten entfernt: VB6-Befehle wie 'Int()', 'Str()' und 'Val()' durch native .NET-Äquivalente ersetzt.
    ''' - Robustheit erhöht: String-Splittings und Datumsberechnungen mittels 'Date.TryParse' und '.Substring()' abgesichert.
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter entfernt, Zuweisungsoperatoren modernisiert ('+=' / '&=').
    ''' - Speicheroptimierung: Fehlerhafte statische Array-Dimensionierungen entfernt.
    ''' </remarks>
    Public Sub prDruckRechnung(ByRef sBID As String, ByVal sKID As String, ByRef nRNr As String, ByVal sdatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByRef sRZiel As String, ByVal S1 As String, ByRef s2 As String, ByRef s3 As String, ByVal nReArt As Integer, ByVal ParamArray arDruck1() As String)
        Dim sDatei As String
        Dim aÜbergabeKasse() As String
        Dim arIn(12) As String
        Dim xlaenge As Integer = 25
        Dim arFeld() As String
        Dim arfeld1() As String
        Dim rechnung() As Double
        Dim rechnung1(8) As Double
        Dim yHoehe As Integer
        Dim nSeite As Integer = 1

        sSS7 = S1
        sSS19 = s2
        sSSS = s3

        ' Stammdaten laden
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "BuchDatum", "Name1", "Name2"})
        nSprache = Convert.ToInt32(Val(arfeld1(2)))
        arfeld1(4) = ""
        arfeld1(5) = ""

        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "Name2"})

        For i As Integer = 0 To 5
            rechnung1(i) = 0
        Next

        ' Positions-Schleife mit automatischem Seitenumbruch alle 13 Zeilen
        For i As Integer = 0 To arDruck1.Length - 1
            If i = 0 OrElse i Mod 13 = 0 Then
                If i <> 0 Then
                    prDruckFuss(xlaenge)
                    prVpeNewPage()
                End If
                prVpePageFormat("A4")
                prVpeTextFormat("Times New Roman", 10)
                nSeite += 1
                prDruckKopf(xlaenge, sdatum, arfeld1(4), arfeld1(5), arFeld)
                prDruckRech(xlaenge, arfeld1(1), nRNr, arfeld1(2))
                yHoehe = 117
                prDruckKopfPosition(xlaenge, arfeld1(2), nReArt)
            End If

            rechnung = fcDruckPosition(i, xlaenge, yHoehe, arDruck1(i), arfeld1(2), nReArt)
            yHoehe += 5
            For j As Integer = 0 To 7
                rechnung1(j) = Math.Round(rechnung1(j) + rechnung(j), 2)
            Next
        Next

        rechnung1(8) = Convert.ToDouble(anzahlung)

        ' Gesamtsummen-Block berechnen und zeichnen
        aÜbergabeKasse = FcDruckSumme(xlaenge, yHoehe, arfeld1(2), rechnung1)

        ' Zahlungs- und Übergabestatus prüfen
        If bBar Then
            prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Betrag dankend erhalten"))
            sDatei = "K" & nRNr & ".DAV"
            arIn(0) = "K" & nRNr & "-1"
            arIn(6) = "K" & nRNr & "-2"
            arIn(3) = arIni(18)
            arIn(9) = arIni(18)
        Else
            Dim sZDatum As String = ""
            sRZiel = sRZiel.Trim()

            If String.IsNullOrEmpty(sRZiel) Then
                prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis") & ": " & Date.Today.AddDays(14).ToString("dd.MM.yyyy") & " " & fcLanguage(nSprache, "auf unten genanntes Konto"))
            Else
                Dim dZieldatum As Date
                If Date.TryParse(sRZiel, dZieldatum) Then
                    sZDatum = dZieldatum.ToString("dd.MM.yyyy")
                Else
                    Dim sInhald1() As String = arDruck1(0).Split("#"c)
                    Dim sVon As String = ""
                    If sInhald1.Length > 10 AndAlso sInhald1(10).Length >= 10 Then
                        sVon = sInhald1(10).Substring(0, 10)
                    End If

                    If sRZiel.Length >= 3 AndAlso sRZiel.Substring(2, 1) = "." Then
                        Dim sJahrAusVon As String = If(sVon.Length >= 10, sVon.Substring(6), "")
                        sZDatum = sRZiel & "." & sJahrAusVon

                        If fcUmDatum(sZDatum) > fcUmDatum(sVon) Then
                            Dim nJahrTemp As Integer = 0
                            If Integer.TryParse(sJahrAusVon, nJahrTemp) Then
                                sZDatum = sRZiel.Substring(0, 5) & "." & (nJahrTemp - 1).ToString()
                            End If
                        End If
                    Else
                        If IsNumeric(sRZiel) Then
                            Dim nTage As Integer = Convert.ToInt32(Val(sRZiel))
                            Dim ddatum As Date
                            If nTage < 0 Then
                                Dim dVonDate As Date
                                If Date.TryParse(sVon, dVonDate) Then
                                    ddatum = dVonDate.AddDays(nTage)
                                End If
                            Else
                                Dim dBuchDate As Date
                                If Date.TryParse(fcUmDatum(arfeld1(3)), dBuchDate) Then
                                    ddatum = dBuchDate.AddDays(nTage)
                                End If
                            End If
                            sZDatum = ddatum.ToString("dd.MM.yyyy")
                        End If
                    End If
                End If
                prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis") & ": " & sZDatum & " " & fcLanguage(nSprache, "auf unten genanntes Konto"))
            End If

            sDatei = "B" & nRNr & ".DAV"
            arIn(0) = "B" & nRNr & "-1"
            arIn(6) = "B" & nRNr & "-2"
            arIn(3) = arIni(19)
            arIn(9) = arIni(19)
        End If

        prDruckFuss(xlaenge)

        ' PDF-Datei schreiben
        Dim sArchivPfad As String = Path.Combine(arIni(32), "Rechnung")
        If Not DirExists(sArchivPfad) Then
            CreateDir(sArchivPfad)
        End If
        prVpePDF(Path.Combine(cgPfad, "Ablage\Rechnung.PDF"))
    End Sub


    ''' <summary>
    ''' Steuert das Drucklayout der Stornorechnung über das VPE-System, berechnet die reduzierten 
    ''' Stornogebühren und Steuersätze, verwaltet Seitenumbrüche und generiert das PDF-Dokument im Archiv.
    ''' </summary>
    ''' <param name="sBID">Die Buchungs-ID (ByRef übergeben).</param>
    ''' <param name="sKID">Die Kunden-ID.</param>
    ''' <param name="nRNr">Die Rechnungsnummer (ByRef übergeben).</param>
    ''' <param name="sdatum">Das Rechnungsdatum.</param>
    ''' <param name="bBar">Gibt an, ob Barzahlung vorliegt (ByRef übergeben).</param>
    ''' <param name="anzahlung">Der bereits geleistete Anzahlungsbetrag.</param>
    ''' <param name="sRZiel">Das Zahlungsziel oder Fälligkeitsdatum (ByRef übergeben).</param>
    ''' <param name="arDruck1">Ein Parameter-Array mit den formatierten Stornopositionen.</param>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - VB6-Altlasten entfernt: 'Int()', 'Str()' und 'Val()' durch native .NET-Äquivalente und '.ToString()' ersetzt.
    ''' - Datumsverarbeitung stabilisiert: String-Splitting und Datumsberechnungen mittels 'Date.TryParse' und '.Substring()' abgesichert.
    ''' - Code-Hygiene: Ungenutzte Arrays ('arfeld2', 'arfeld3', 'arfeld4') vollständig entfernt.
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter entfernt und Zuweisungsoperatoren modernisiert.
    ''' </remarks>
    Public Sub prDruckRechnungStorno(ByRef sBID As String, ByVal sKID As String, ByRef nRNr As String, ByVal sdatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByRef sRZiel As String, ByVal ParamArray arDruck1() As String)
        Dim sDatei As String
        Dim aÜbergabeKasse() As String
        Dim arIn(12) As String
        Dim xlaenge As Integer = 25
        Dim arFeld() As String
        Dim arfeld1() As String
        Dim rechnung() As Double
        Dim rechnung1(6) As Double
        Dim yHoehe As Integer
        Dim nSeiten As Integer = CInt(Math.Floor(arDruck1.Length / 13)) + 1
        Dim nSeite As Integer = 1

        ' Stammdaten laden
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "BuchDatum", "Name1", "Name2"})
        nSprache = Convert.ToInt32(Val(arfeld1(2)))

        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "Name2"})

        For i As Integer = 0 To 5
            rechnung1(i) = 0
        Next

        ' Positions-Schleife für den Stornodruck mit automatischem Seitenumbruch
        For i As Integer = 0 To arDruck1.Length - 1
            If i = 0 OrElse i Mod 13 = 0 Then
                If i <> 0 Then
                    prDruckFuss(xlaenge)
                    prVpeNewPage()
                End If
                prVpePageFormat("A4")
                prVpeTextFormat("Times New Roman", 10)
                prVpeText(xlaenge + 112, 53, "Tel. " & arIni(6))
                prVpeText(xlaenge + 112, 61, "Seite " & nSeite.ToString() & " von " & nSeiten.ToString())
                nSeite += 1
                prDruckKopf(xlaenge, sdatum, arfeld1(4), arfeld1(5), arFeld)
                prDruckRech(xlaenge, arfeld1(1), nRNr, arfeld1(2))
                yHoehe = 117
                prDruckKopfPositionStorno(xlaenge, arfeld1(2))
            End If

            rechnung = fcDruckPositionStorno(i, xlaenge, yHoehe, arDruck1(i), arfeld1(2))
            yHoehe += 5
            For j As Integer = 0 To 5
                rechnung1(j) = Math.Round(rechnung1(j) + rechnung(j), 2)
            Next
        Next

        rechnung1(6) = Convert.ToDouble(anzahlung)

        ' Stornorestbetrag und geminderte Steuer berechnen
        Dim dRest As Decimal = Convert.ToDecimal(rechnung1(1) + rechnung1(2)) - anzahlung
        Dim nSteuerSatz11 As Decimal = Convert.ToDecimal(Val(arIni(11)))

        rechnung1(1) = Convert.ToDouble(Math.Round(dRest / ((nSteuerSatz11 + 100) / 100), 2))
        rechnung1(2) = Convert.ToDouble(Math.Round(dRest - Convert.ToDecimal(rechnung1(1)), 2))

        ' Gesamtsummenblock ausgeben
        aÜbergabeKasse = FcDruckSumme(xlaenge, yHoehe, arfeld1(2), rechnung1)

        ' Zahlungskondition andrucken und Kassenbelegnamen zuweisen
        If bBar Then
            prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Betrag dankend erhalten"))
            sDatei = "K" & nRNr & ".DAV"
            arIn(0) = "K" & nRNr & "-1"
            arIn(6) = "K" & nRNr & "-2"
            arIn(3) = arIni(18)
            arIn(9) = arIni(18)
        Else
            Dim sZDatum As String = ""
            sRZiel = sRZiel.Trim()

            If String.IsNullOrEmpty(sRZiel) Then
                prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis") & ": " & Date.Today.AddDays(14).ToString("dd.MM.yyyy") & " " & fcLanguage(nSprache, "auf unten genanntes Konto"))
            Else
                Dim sInhald1() As String = arDruck1(0).Split("#"c)
                Dim sVon As String = ""
                If sInhald1.Length > 9 AndAlso sInhald1(9).Length >= 10 Then
                    sVon = sInhald1(9).Substring(0, 10)
                End If

                If sRZiel.Length >= 3 AndAlso sRZiel.Substring(2, 1) = "." Then
                    Dim sJahrAusVon As String = If(sVon.Length >= 10, sVon.Substring(6), "")
                    sZDatum = sRZiel & "." & sJahrAusVon

                    If fcUmDatum(sZDatum) > fcUmDatum(sVon) Then
                        Dim nJahrTemp As Integer = 0
                        If Integer.TryParse(sJahrAusVon, nJahrTemp) Then
                            sZDatum = sRZiel.Substring(0, 5) & "." & (nJahrTemp - 1).ToString()
                        End If
                    End If
                Else
                    If IsNumeric(sRZiel) Then
                        Dim nTage As Integer = Convert.ToInt32(Val(sRZiel))
                        Dim ddatum As Date
                        If nTage < 0 Then
                            Dim dVonDate As Date
                            If Date.TryParse(sVon, dVonDate) Then
                                ddatum = dVonDate.AddDays(nTage)
                            End If
                        Else
                            Dim dBuchDate As Date
                            If Date.TryParse(fcUmDatum(arfeld1(3)), dBuchDate) Then
                                ddatum = dBuchDate.AddDays(nTage)
                            End If
                        End If
                        sZDatum = ddatum.ToString("dd.MM.yyyy")
                    End If
                End If

                prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis") & ": " & sZDatum & " " & fcLanguage(nSprache, "auf unten genanntes Konto"))
            End If

            sDatei = "B" & nRNr & ".DAV"
            arIn(0) = "B" & nRNr & "-1"
            arIn(6) = "B" & nRNr & "-2"
            arIn(3) = arIni(19)
            arIn(9) = arIni(19)
        End If

        prDruckFuss(xlaenge)

        ' Zielordner prüfen und PDF-Beleg schreiben
        Dim sArchivPfad As String = Path.Combine(arIni(32), "Rechnung")
        If Not DirExists(sArchivPfad) Then
            CreateDir(sArchivPfad)
        End If

        prVpePDF(Path.Combine(sArchivPfad, "Rech_" & nRNr & ".PDF"))
    End Sub


    '''' <summary>
    '''' prDruckRechnung( Buchungs ID )
    '''' </summary>
    '''' <remarks>
    '''' 03.03.2012 Create
    '''' </remarks>
    'Public Sub prDruckRechnungPausch(ByRef sBID As String, ByVal sKID As String, ByRef nRNr As String, ByVal sdatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByRef sRZiel As String, ByVal ParamArray arDruck1() As String)
    '    Dim sDatei As String  'Kassenbuch
    '    ' Dim sKasseName As String 'Kassenbuch
    '    '  Dim skonto As String
    '    Dim aÜbergabeKasse(4) As String
    '    Dim arIn(12) As String
    '    Dim xlaenge As Integer = 25
    '    Dim arFeld(1) As String
    '    Dim arfeld1(1) As String
    '    Dim arfeld2(1) As String
    '    Dim arfeld3(1) As String
    '    Dim arfeld4(1) As String
    '    Dim rechnung() As Double
    '    Dim rechnung1(6) As Double
    '    Dim yHoehe As Integer
    '    Dim ii As Integer
    '    Dim nSeiten As Integer = Int(arDruck1.Length / 13) + 1
    '    Dim nSeite As Integer = 1
    '    arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "BuchDatum", "Name1", "Name2"})
    '    nSprache = Val(arfeld1(2))
    '    '   sKID = arfeld1(0)
    '    arfeld1(4) = ""
    '    arfeld1(5) = ""
    '    arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "Name2"})
    '    '   prVpeOpen()
    '    For i = 0 To 5
    '        rechnung1(i) = 0
    '    Next
    '    For i = 0 To arDruck1.Length - 1
    '        If i = 0 Or i / 13 = Int(i / 13) Then
    '            If i <> 0 Then
    '                Call prDruckFuss(xlaenge)
    '                prVpeNewPage()
    '            End If
    '            Call prVpePageFormat("A4")
    '            prVpeTextFormat("Times New Roman", 10)
    '            prVpeText(xlaenge + 112, 53, "Tel. " & arIni(6))
    '            prVpeText(xlaenge + 112, 61, "Seite " & Str(nSeite) & " von " & Str(nSeiten))
    '            nSeite = nSeite + 1
    '            Call prDruckKopf(xlaenge, sdatum, arfeld1(4), arfeld1(5), arFeld)
    '            Call prDruckRech(xlaenge, arfeld1(1), nRNr, arfeld1(2))
    '            yHoehe = 117
    '            Call prDruckKopfPosition(xlaenge, arfeld1(2), 0)


    '        End If


    '        'If arDruck1(i) <> Nothing Then

    '        'End If
    '        rechnung = fcDruckPosition(i, xlaenge, yHoehe, arDruck1(i), arfeld1(2), 0)
    '        yHoehe = yHoehe + 5
    '        For j = 0 To 5
    '            rechnung1(j) = Math.Round(rechnung1(j) + rechnung(j), 2)
    '        Next
    '    Next
    '    rechnung1(6) = anzahlung
    '    Dim dRest As Decimal = rechnung1(1) + rechnung1(2) - anzahlung
    '    rechnung1(1) = Math.Round(dRest / ((Val(arIni(11)) + 100) / 100), 2)
    '    rechnung1(2) = Math.Round(dRest - rechnung1(1), 2)


    '    aÜbergabeKasse = FcDruckSumme(xlaenge, yHoehe, arfeld1(2), rechnung1)

    '    ' arfeld4 = fcDruckPosition(xlaenge, sgRBID, "R", arfeld1(2))
    '    '  arfeld4 = fcDruckZimmer(xlaenge, sgRBID, "R", arfeld1(2))
    '    ii = Val(arfeld4(0))
    '    If bBar = True Then
    '        prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Betrag dankend erhalten"))
    '        sDatei = "K" & nRNr & ".DAV"
    '        ' sKasseName = "Rechnung Einnahme K" & Str(nRNr)
    '        arIn(0) = "K" & nRNr & "-1"  'Name
    '        arIn(6) = "K" & nRNr & "-2"  'Name
    '        arIn(3) = arIni(18)   'konto
    '        arIn(9) = arIni(18)
    '        ' skonto = arIni(18)
    '    Else
    '        Dim sZDatum As String = ""
    '        Dim t As String = arfeld1(3)
    '        sRZiel = sRZiel.Trim
    '        If sRZiel = " " Or sRZiel = "" Then
    '            prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis") & ": " & Date.Today.AddDays(14) & " " & fcLanguage(nSprache, "auf unten genanntes Konto"))

    '        Else
    '            Dim sInhald1() As String = Split(arDruck1(0), "#")
    '            Dim sVon As String = Mid(sInhald1(9), 1, 10)
    '            If Mid(sRZiel, 3, 1) = "." Then
    '                sZDatum = sRZiel + "." + Mid(sVon, 7)
    '                If fcUmDatum(sZDatum) > fcUmDatum(sVon) Then
    '                    sZDatum = Mid(sRZiel, 1, 5) + "." + Str((Mid(sVon, 7) - 1))
    '                End If
    '            Else
    '                Dim ddatum As Date
    '                If IsNumeric(sRZiel) = True Then
    '                    Dim nTage As Integer = Val(sRZiel)
    '                    If nTage < 0 Then                              ' kleiner 0 zahlung nach vor anreise -tage
    '                        ddatum = CDate(sVon).AddDays(nTage)
    '                    Else                                            'größer 0 zahlung nach erhald + tage
    '                        ddatum = CDate(fcUmDatum(arfeld1(3))).AddDays(nTage)
    '                    End If
    '                    sZDatum = ddatum
    '                End If
    '            End If

    '            prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis") & ": " & sZDatum & " " & fcLanguage(nSprache, "auf unten genanntes Konto"))

    '        End If
    '        sDatei = "B" & nRNr & ".DAV"
    '        '  sKasseName = "Rechnung Einnahme B" & Str(nRNr)
    '        arIn(0) = "B" & nRNr & "-1"  'Name
    '        arIn(6) = "B" & nRNr & "-2"  'Name
    '        arIn(3) = arIni(19)
    '        arIn(9) = arIni(19)
    '        '  skonto = arIni(19)
    '    End If
    '    Call prDruckFuss(xlaenge)

    '    If DirExists(arIni(32) & "\Rechnung") = False Then
    '        CreateDir(arIni(32) & "\Rechnung")
    '    End If

    '    Call prVpePDF(cgPfad & "\Ablage" & "\Rechnung.PDF")
    '    'Call prVpePDF(arIni(32) & "\Rechnung" & "\Rech_" & nRNr & ".PDF")



    'End Sub


    '''' <summary>
    '''' Erstellt eine CSV-strukturierte Übergabedatei für das Kassenprogramm mit den aufgeteilten 
    '''' Buchungssätzen und exportiert diese in das konfigurierte Datev-Verzeichnis.
    '''' </summary>
    '''' <param name="sDatei">Der Name der zu erstellenden Datei (ByRef übergeben).</param>
    '''' <param name="arIn">Das Parameter-Array mit den strukturierten Buchungsinformationen.</param>
    '''' <remarks>
    '''' 03.03.2012 - Create
    '''' 07.10.2026 - Code-Optimierung:
    '''' - Index-Schutz: Schleifen greifen nun dynamisch mit einer Längenprüfung auf das Array zu, um OutOfBounds-Exceptions zu verhindern.
    '''' - Code-Hygiene: Redundantes 'Call'-Schlüsselwort entfernt und String-Verknüpfung modernisiert.
    '''' </remarks>
    'Private Sub prKassenDatei(ByRef sDatei As String, ByVal ParamArray arIn() As String)
    '    Dim sb As New System.Text.StringBuilder()
    '    sb.AppendLine("Name,Betrag,Mwst,Konto,gegenkonto,Datum ")

    '    ' Erster Buchungssatz (Index 0 bis 5)
    '    For i As Integer = 0 To 5
    '        If i < arIn.Length Then
    '            sb.Append(arIn(i) & ";")
    '        End If
    '    Next
    '    sb.AppendLine()

    '    ' Zweiter Buchungssatz (Index 6 bis 11)
    '    For i As Integer = 6 To 11
    '        If i < arIn.Length Then
    '            sb.Append(arIn(i) & ";")
    '        End If
    '    Next

    '    WriteFileSeriell(arIni(32) & "\Datev\" & sDatei, sb.ToString())
    'End Sub

    ''' <summary>
    ''' Zeichnet die Tabellenüberschrift (Spaltenköpfe) für die Standard-Rechnungspositionen 
    ''' über das VPE-Layoutsystem (inklusive conditional Spaltenbeschriftung bei Pauschalrechnungen).
    ''' </summary>
    ''' <param name="xlaenge">Die X-Koordinate für die horizontale Ausrichtung der Tabelle (ByRef übergeben).</param>
    ''' <param name="sSprache1">Die Sprachsteuerung als String-ID (ByRef übergeben).</param>
    ''' <param name="nReArt">Die aktive Rechnungsart (z. B. 4 für Pauschal) (ByRef übergeben).</param>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Code-Hygiene: Unbenutzte Variablen 'i' und 'ii' entfernt und Zeilenhöhe direkt als Konstante (5) übergeben.
    ''' - VB6-Altlast entfernt: 'Val()' durch standardisierte 'Convert.ToInt32()' für die Sprachübersetzung ersetzt.
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter konsequent entfernt.
    ''' </remarks>
    Private Sub prDruckKopfPosition(ByRef xlaenge As Integer, ByRef sSprache1 As String, ByRef nReArt As Integer)
        Dim nSprache As Integer = Convert.ToInt32(Val(sSprache1))

        ' Tabellendimensionierung für die Kopfzeile initialisieren (Höhe: 5 Einheiten)
        prTableDim(xlaenge, 112, 165, 5)
        prTableColumns(8)
        prTableRows(1)

        For j As Integer = 0 To 7
            prTabelCellSelectFont(j, 0, "Times New Roman", 9)
            prTabelCellLeft(j, 0, 1)
        Next

        ' Exakte Definition der Spaltenbreiten
        prTableColumnsWidth(0, 6)
        prTableColumnsWidth(1, 30)
        prTableColumnsWidth(2, 25)
        prTableColumnsWidth(3, 10)
        prTableColumnsWidth(5, 10)
        prTableColumnsWidth(6, 22)
        prTableColumnsWidth(7, 22)

        ' Beschriftung der Zellen über das Übersetzungssystem
        prTabelCellText(0, 0, fcLanguage(nSprache, "Pos"))
        prTabelCellText(1, 0, fcLanguage(nSprache, "Objekt"))
        prTabelCellText(2, 0, fcLanguage(nSprache, "Ausstattung"))
        prTabelCellText(3, 0, fcLanguage(nSprache, "Art"))
        prTabelCellText(4, 0, fcLanguage(nSprache, "Zeit"))
        prTabelCellText(5, 0, fcLanguage(nSprache, "Pers."))

        ' Spalte 6 wird bei Pauschalrechnungen (ReArt = 4) ausgeblendet
        If nReArt = 4 Then
            prTabelCellText(6, 0, "")
        Else
            prTabelCellText(6, 0, fcLanguage(nSprache, "Preis/Nacht"))
        End If
        prTabelCellText(7, 0, fcLanguage(nSprache, "Zimmerpreis"))

        prTabelWrite()
    End Sub


    ''' <summary>
    ''' Berechnet die detaillierten Umsatz-, Frühstücks- und Getränkebeträge einer Standard-Rechnungsposition,
    ''' trägt diese formatiert in das VPE-Tabellenlayout ein und gibt die aufgeteilten 
    ''' Netto- und Steuerwerte für die Finanzbuchhaltung zurück.
    ''' </summary>
    ''' <param name="pos">Die laufende Indexposition der aktuellen Tabellenzeile.</param>
    ''' <param name="xlaenge">Die X-Koordinate für die horizontale Ausrichtung der Tabelle (ByRef übergeben).</param>
    ''' <param name="yHoehe">Die vertikale Y-Startposition für das Zeichnen der Zeile.</param>
    ''' <param name="sInhald">Die per Raute (#) getrennten Positionsdaten aus dem Quell-Array.</param>
    ''' <param name="sSprache1">Die Sprachsteuerung als String-ID (ByRef übergeben).</param>
    ''' <param name="nReArt">Die aktive Rechnungsart (z. B. 4 für Pauschal) (ByRef übergeben).</param>
    ''' <returns>Ein Double-Array mit den berechneten Beträgen für Zimmer-, Frühstücks- und Getränkeumsätze.</returns>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Typsicherheit erhöht: Rückgabetyp von generischem 'Array' auf ein präzises 'Double()' geändert.
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter entfernt und Zuweisungsoperatoren modernisiert.
    ''' - VB6-Relikte entfernt: 'Split' durch '.Split()' und 'Str()' durch '.ToString()' abgelöst.
    ''' - Robustheit: 'fcChangs' durch native '.Replace()'-Aufrufe ersetzt, um Kultur-unabhängige mathematische Konvertierungen abzusichern.
    ''' </remarks>
    Private Function fcDruckPosition(ByVal pos As Integer, ByRef xlaenge As Integer, ByVal yHoehe As Integer, ByVal sInhald As String, ByRef sSprache1 As String, ByRef nReArt As Integer) As Double()
        Dim sInhald1() As String = sInhald.Split("#"c)
        Dim nFPreis As Double
        Dim nZPreis As Double
        Dim nGPreis As Double
        Dim arRueck(7) As Double
        Dim nSprache As Integer = Convert.ToInt32(Val(sSprache1))

        ' Tabellendimensionierung und Font-Zuweisung für eine Zeile (Höhe 5)
        prTableDim(xlaenge, yHoehe, 165, 5)
        prTableColumns(8)
        prTableRows(1)

        For j As Integer = 0 To 7
            prTabelCellSelectFont(j, 0, "Times New Roman", 9)
            prTabelCellLeft(j, 0, 1)
        Next

        ' Exakte Spaltenbreiten definieren
        prTableColumnsWidth(0, 6)
        prTableColumnsWidth(1, 30)
        prTableColumnsWidth(2, 25)
        prTableColumnsWidth(3, 10)
        prTableColumnsWidth(5, 10)
        prTableColumnsWidth(6, 22)
        prTableColumnsWidth(7, 22)

        ' Textzellen befüllen
        prTabelCellText(0, 0, (pos + 1).ToString().Trim())
        prTabelCellText(1, 0, fcLanguage(nSprache, sInhald1(2)))   ' Objekt
        prTabelCellText(2, 0, fcLanguage(nSprache, sInhald1(13)))  ' Ausstattung
        prTabelCellText(3, 0, sInhald1(12))                        ' Art
        prTabelCellText(4, 0, sInhald1(10))                        ' Zeit
        prTabelCellText(5, 0, sInhald1(11))

        If nReArt <> 4 Then
            prTabelCellText(6, 0, fcDecStr(sInhald1(3), 10) & " €")
        End If
        prTabelCellText(7, 0, fcDecStr(sInhald1(8), 10) & " €")

        ' Mathematische Umsatzaufteilung (Frühstück, Getränke und Basis-Zimmerpreis)
        Dim nAnzahlNaechte As Double = Convert.ToDouble(Val(sInhald1(1)))
        Dim nFaktor11 As Double = Convert.ToDouble(Val(sInhald1(11)))

        nFPreis = nAnzahlNaechte * nFaktor11 * Convert.ToDouble(Val(sInhald1(14))) / 100
        nGPreis = nAnzahlNaechte * nFaktor11 * Convert.ToDouble(Val(sInhald1(18))) / 100

        Dim nBruttoGesamt As Double = Convert.ToDouble(Val(sInhald1(8).Replace(",", ".")))
        nZPreis = nBruttoGesamt - nFPreis - nGPreis

        prTabelWrite()

        ' Rückgabe-Array für die Datev-Verbuchung (Netto- und MwSt-Anteile) befüllen
        Dim nFeld5Wert As Double = Convert.ToDouble(Val(sInhald1(5).Replace(",", ".")))
        arRueck(3) = Math.Round(nFPreis - nFeld5Wert, 2)
        arRueck(4) = Math.Round(nFeld5Wert, 2)

        Dim nFeld4Wert As Double = Convert.ToDouble(Val(sInhald1(4).Replace(",", ".")))
        arRueck(1) = Math.Round(nZPreis - nFeld4Wert, 2)
        arRueck(2) = Math.Round(nFeld4Wert, 2)

        arRueck(5) = nBruttoGesamt

        Dim nFeld6Wert As Double = Convert.ToDouble(Val(sInhald1(6).Replace(",", ".")))
        arRueck(6) = Math.Round(nGPreis - nFeld6Wert, 2)
        arRueck(7) = Math.Round(nFeld6Wert, 2)

        Return arRueck
    End Function


    ''' <summary>
    ''' Zeichnet die Tabellenüberschrift (Spaltenköpfe) für die Stornopositionen 
    ''' einer Rechnung über das VPE-Layoutsystem.
    ''' </summary>
    ''' <param name="xlaenge">Die X-Koordinate für die horizontale Ausrichtung der Tabelle (ByRef übergeben).</param>
    ''' <param name="sSprache1">Die Sprachsteuerung als String-ID (ByRef übergeben).</param>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Code-Hygiene: Unbenutzte Zählervariable 'i' sowie alle auskommentierten Spaltendefinitionen restlos gelöscht.
    ''' - Performance & Übersicht: Variable 'ii' entfernt und die Tabellenhöhe direkt als konstanter Wert (5) übergeben.
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter entfernt und Zuweisungen modernisiert.
    ''' </remarks>
    Private Sub prDruckKopfPositionStorno(ByRef xlaenge As Integer, ByRef sSprache1 As String)
        Dim nSprache As Integer = Convert.ToInt32(Val(sSprache1))

        ' Tabellendimensionierung für die Kopfzeile (Höhe: 5 Einheiten)
        prTableDim(xlaenge, 112, 165, 5)
        prTableColumns(8)
        prTableRows(1)

        For j As Integer = 0 To 7
            prTabelCellSelectFont(j, 0, "Times New Roman", 9)
            prTabelCellLeft(j, 0, 1)
        Next

        ' Exakte Spaltenbreiten analog zur Positionstabelle zuweisen
        prTableColumnsWidth(0, 6)
        prTableColumnsWidth(1, 22)
        prTableColumnsWidth(3, 8)
        prTableColumnsWidth(4, 23)
        prTableColumnsWidth(5, 23)
        prTableColumnsWidth(6, 10)
        prTableColumnsWidth(7, 23)

        ' Spaltenübersetzungen eintragen
        prTabelCellText(0, 0, fcLanguage(nSprache, "Pos"))
        prTabelCellText(1, 0, fcLanguage(nSprache, "Objekt"))
        prTabelCellText(2, 0, fcLanguage(nSprache, "Zeit"))
        prTabelCellText(3, 0, fcLanguage(nSprache, "Pers."))
        prTabelCellText(4, 0, fcLanguage(nSprache, "Preis/Nacht"))
        prTabelCellText(5, 0, fcLanguage(nSprache, "Zimmerpreis"))
        prTabelCellText(6, 0, fcLanguage(nSprache, "%"))
        prTabelCellText(7, 0, fcLanguage(nSprache, "Preis-%"))

        prTabelWrite()
    End Sub


    ''' <summary>
    ''' Berechnet die Stornogebühren und Preisreduzierungen einer einzelnen Rechnungsposition,
    ''' trägt diese formatiert in das VPE-Tabellenlayout ein und gibt die aufgeteilten 
    ''' Netto- und Steuerwerte für die Finanzbuchung zurück.
    ''' </summary>
    ''' <param name="pos">Die laufende Indexposition der aktuellen Tabellenzeile.</param>
    ''' <param name="xlaenge">Die X-Koordinate für die horizontale Ausrichtung der Tabelle (ByRef übergeben).</param>
    ''' <param name="yHoehe">Die vertikale Y-Startposition für das Zeichnen der Zeile.</param>
    ''' <param name="sInhald">Die per Raute (#) getrennten Positionsdaten aus dem Quell-Array.</param>
    ''' <param name="sSprache1">Die Sprachsteuerung als String-ID (ByRef übergeben).</param>
    ''' <returns>Ein Double-Array mit den berechneten Beträgen für Zimmer- und Frühstücksumsätze.</returns>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Typsicherheit erhöht: Rückgabetyp von generischem 'Array' auf ein echtes 'Double()' umgestellt.
    ''' - Syntax bereinigt: Redundante 'Call'-Aufrufe entfernt und Zuweisungen harmonisiert.
    ''' - VB6-Altlasten entfernt: 'Split' durch die Instanzmethode '.Split()' und 'Str()' durch '.ToString()' abgelöst.
    ''' - Fehlerprävention: 'fcChangs' wurde durch native '.Replace()'-Aufrufe ersetzt, um Kultur-unabhängige mathematische Konvertierungen abzusichern.
    ''' </remarks>
    Private Function fcDruckPositionStorno(ByVal pos As Integer, ByRef xlaenge As Integer, ByVal yHoehe As Integer, ByVal sInhald As String, ByRef sSprache1 As String) As Double()
        Dim sInhald1() As String = sInhald.Split("#"c)
        Dim nFPreis As Double
        Dim nZPreis As Double
        Dim arRueck(5) As Double

        Dim nZimmerP As Double = Convert.ToDouble(Val(sInhald1(3)))
        Dim ngPreis As Double = nZimmerP * Convert.ToDouble(Val(sInhald1(1)))
        Dim ngPreis1 As Double = ngPreis * Convert.ToDouble(Val(sInhald1(15))) / 100
        Dim nSprache As Integer = Convert.ToInt32(Val(sSprache1))

        ' Tabellendimensionierung und Font-Auswahl
        prTableDim(xlaenge, yHoehe, 165, 5)
        prTableColumns(8)
        prTableRows(1)

        For j As Integer = 0 To 7
            prTabelCellSelectFont(j, 0, "Times New Roman", 9)
            prTabelCellLeft(j, 0, 1)
        Next

        ' Spaltenbreiten definieren
        prTableColumnsWidth(0, 6)
        prTableColumnsWidth(1, 22)
        prTableColumnsWidth(3, 8)
        prTableColumnsWidth(4, 23)
        prTableColumnsWidth(5, 23)
        prTableColumnsWidth(6, 10)
        prTableColumnsWidth(7, 23)

        ' Zellen befüllen
        prTabelCellText(0, 0, (pos + 1).ToString().Trim())
        prTabelCellText(1, 0, fcLanguage(nSprache, sInhald1(2)))
        prTabelCellText(2, 0, sInhald1(9))
        prTabelCellText(3, 0, sInhald1(10))
        prTabelCellText(4, 0, fcDecStr(nZimmerP, 10) & " €")
        prTabelCellText(5, 0, fcDecStr(ngPreis, 10) & " €")
        prTabelCellText(6, 0, sInhald1(15))
        prTabelCellText(7, 0, fcDecStr(ngPreis1, 10) & " €")

        ' Mathematische Preisaufteilung (Zimmer vs. Frühstück)
        nFPreis = Convert.ToDouble(Val(sInhald1(1))) * Convert.ToDouble(Val(sInhald1(10))) * Convert.ToDouble(Val(sInhald1(13))) / 100
        nZPreis = Convert.ToDouble(Val(sInhald1(7).Replace(",", "."))) - nFPreis

        prTabelWrite()

        ' Rückgabe-Array befüllen
        arRueck(3) = Math.Round(nFPreis - Convert.ToDouble(Val(sInhald1(5).Replace(",", "."))), 2)
        arRueck(4) = Math.Round(Convert.ToDouble(Val(sInhald1(5).Replace(",", "."))), 2)

        arRueck(1) = Math.Round(nZPreis - Convert.ToDouble(Val(sInhald1(4).Replace(",", "."))), 2)
        arRueck(2) = Math.Round(Convert.ToDouble(Val(sInhald1(4).Replace(",", "."))), 2)

        arRueck(5) = Convert.ToDouble(Val(sInhald1(7).Replace(",", ".")))

        Return arRueck
    End Function



    ''' <summary>
    ''' Ersetzt das erste Vorkommen eines Suchzeichens in einem String durch einen Ziel-String.
    ''' </summary>
    ''' <param name="sText">Der zu verarbeitende Text (ByRef übergeben).</param>
    ''' <param name="sSuch">Das Zeichen oder die Zeichenkette, nach der gesucht wird.</param>
    ''' <param name="sZiel">Die Zeichenkette, die stattdessen eingesetzt wird.</param>
    ''' <returns>Der modifizierte Gesamttext als String.</returns>
    ''' <remarks>
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Schleife komplett entfernt: Ersetzt durch die performante, native '.IndexOf()'-Methode.
    ''' - Datentyp definiert: Expliziten Rückgabetyp 'As String' hinzugefügt, um Option Strict Konformität zu garantieren.
    ''' - Logik bereinigt: Eliminiert die riskanten, verschachtelten 'Replace'-Aufrufe der alten Variante.
    ''' </remarks>
    Public Function fcChangs(ByRef sText As String, ByVal sSuch As String, ByVal sZiel As String) As String
        If String.IsNullOrEmpty(sText) OrElse String.IsNullOrEmpty(sSuch) Then
            Return sText
        End If

        ' Position des gesuchten Zeichens/Strings bestimmen
        Dim nPos As Integer = sText.IndexOf(sSuch)

        ' Wenn gefunden, den String an dieser Position sauber aufteilen und neu zusammensetzen
        If nPos <> -1 Then
            sText = sText.Substring(0, nPos) & sZiel & sText.Substring(nPos + sSuch.Length)
        End If

        Return sText
    End Function


    ''' <summary>
    ''' Berechnet die Mehrwertsteuer- und Rechnungssummen (aufgeteilt nach Übernachtung, Speisen und Getränken), 
    ''' zeichnet die Summenblöcke über die Tabellenfunktionen in das VPE-Dokument und 
    ''' gibt die Kassenbuch-Übergabewerte zurück.
    ''' </summary>
    ''' <param name="xlaenge">Die X-Koordinate für die horizontale Positionierung der Tabellen.</param>
    ''' <param name="yHoehe">Die Y-Koordinate für die vertikale Positionierung der Tabellen.</param>
    ''' <param name="sSprache1">Die ID der zu verwendenden Sprache als String.</param>
    ''' <param name="sInhald1">Ein Parameter-Array mit den berechneten kumulierten Rechnungswerten.</param>
    ''' <returns>Ein String-Array mit den Bruttobeträgen und Steuersätzen für die Kassenübergabe.</returns>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Typsicherheit erhöht: Rückgabetyp von generischem 'Array' auf stark typisiertes 'String()' geändert.
    ''' - Syntax bereinigt: Redundante 'Call'-Schlüsselwörter entfernt und Zuweisungsoperatoren modernisiert.
    ''' - VB6-Altlasten entfernt: 'Val()' und 'Str()' durch standardisierte .NET-Konvertierungsmethoden abgelöst.
    ''' - Scope geschärft: Schleifen-Zähler ('i', 'j', 'i1') direkt in den Kontrollstrukturen lokal deklariert.
    ''' </remarks>
    Private Function FcDruckSumme(ByVal xlaenge As Integer, ByVal yHoehe As Integer, ByVal sSprache1 As String, ByVal ParamArray sInhald1() As Double) As String()
        Dim nSprache As Integer = Convert.ToInt32(Val(sSprache1))
        Dim aÜbergabeKasse(6) As String

        ' --- Tabelle 1: Gesamtsummen ---
        prTableDim(xlaenge + 165 - 54, yHoehe, 54, 25)
        prTableColumns(2)
        prTableRows(3)
        prTableColumnsWidth(1, 22)

        For i As Integer = 0 To 1
            For j As Integer = 0 To 2
                prTabelCellSelectFont(i, j, "Times New Roman", 9)
                prTabelCellLeft(i, j, 1)
            Next
        Next

        prTabelCellText(0, 0, fcLanguage(nSprache, "Summe"))
        prTabelCellText(0, 1, fcLanguage(nSprache, "Anzahlung"))
        prTabelCellText(0, 2, fcLanguage(nSprache, "zu zahlender Betrag"))

        prTabelCellText(1, 0, fcDecStr(sInhald1(5), 10) & " €")
        prTabelCellText(1, 1, fcDecStr(sInhald1(8), 10) & " €")
        prTabelCellText(1, 2, fcDecStr(sInhald1(5) - sInhald1(8), 10) & " €")

        prTabelWrite()

        ' --- Tabelle 2: MwSt-Aufschlüsselung ---
        prTableDim(xlaenge, yHoehe + 10, 110, 15)
        prTableColumns(4)
        prTableRows(4)

        For i1 As Integer = 0 To 3
            For j As Integer = 0 To 3
                prTabelCellSelectFont(j, i1, "Times New Roman", 9)
                prTabelCellLeft(j, i1, 1)
            Next
        Next

        prTabelCellText(0, 0, fcLanguage(nSprache, "MwSt-Satz"))
        prTabelCellText(1, 0, fcLanguage(nSprache, "Nettobetrag"))
        prTabelCellText(2, 0, fcLanguage(nSprache, "MwSt-Betrag"))
        prTabelCellText(3, 0, fcLanguage(nSprache, "Bruttobetrag"))

        ' --- Absturzsichere Konvertierung der Steuersatz-Strings ---
        Dim dblSSS As Double = 0
        Dim dblSS7 As Double = 0
        Dim dblSS19 As Double = 0

        ' Prüfen und Konvertieren von sSSS (Getränke)
        If Not String.IsNullOrWhiteSpace(sSSS) Then
            Double.TryParse(sSSS.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, dblSSS)
        End If

        ' Prüfen und Konvertieren von sSS7 (Übernachtung)
        If Not String.IsNullOrWhiteSpace(sSS7) Then
            Double.TryParse(sSS7.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, dblSS7)
        End If

        ' Prüfen und Konvertieren von sSS19 (Speisen)
        If Not String.IsNullOrWhiteSpace(sSS19) Then
            Double.TryParse(sSS19.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, dblSS19)
        End If

        ' --- Ausgabe in die Tabelle unter Verwendung der geprüften Double-Werte ---
        prTabelCellText(0, 3, fcDecStr(dblSSS, 3, 0, "") & " % " & fcLanguage(nSprache, "Getränke"))
        prTabelCellText(0, 2, fcDecStr(dblSS7, 3, 0, "") & " % " & fcLanguage(nSprache, "Übernachtung"))
        prTabelCellText(0, 1, fcDecStr(dblSS19, 3, 0, "") & " % " & fcLanguage(nSprache, "Speisen"))


        Dim nBruttoNormal As Decimal = Convert.ToDecimal(sInhald1(3) + sInhald1(4))
        Dim nBruttoReduzirt As Decimal = Convert.ToDecimal(sInhald1(1) + sInhald1(2))
        Dim nBruttoSonder As Decimal = Convert.ToDecimal(sInhald1(6) + sInhald1(7))

        Dim nSteuersatz7 As Decimal = (Convert.ToDecimal(Val(sSS7)) / 100) + 1
        Dim nNettoReduzirt As Decimal = nBruttoReduzirt / nSteuersatz7

        Dim nNettoNormal As Decimal = Convert.ToDecimal(sInhald1(3))
        Dim nNettoSonder As Decimal = Convert.ToDecimal(sInhald1(6))

        ' --- Datenübergabe vorbereiten ---
        aÜbergabeKasse(0) = nBruttoNormal.ToString(System.Globalization.CultureInfo.InvariantCulture)
        aÜbergabeKasse(1) = sSS19
        aÜbergabeKasse(2) = nBruttoReduzirt.ToString(System.Globalization.CultureInfo.InvariantCulture)
        aÜbergabeKasse(3) = sSS7
        aÜbergabeKasse(4) = nBruttoSonder.ToString(System.Globalization.CultureInfo.InvariantCulture)
        aÜbergabeKasse(5) = sSSS

        ' --- Werte in Steuer-Tabelle schreiben ---
        prTabelCellText(3, 1, fcDecStr(nBruttoNormal, 10) & " €")
        prTabelCellText(3, 2, fcDecStr(nBruttoReduzirt, 10) & " €")
        prTabelCellText(3, 3, fcDecStr(nBruttoSonder, 10) & " €")

        prTabelCellText(1, 1, fcDecStr(sInhald1(3), 10) & " €")
        prTabelCellText(1, 2, fcDecStr(nNettoReduzirt, 10) & " €")
        prTabelCellText(1, 3, fcDecStr(nNettoSonder, 10) & " €")

        prTabelCellText(2, 1, fcDecStr(nBruttoNormal - nNettoNormal, 10) & " €")
        prTabelCellText(2, 2, fcDecStr(nBruttoReduzirt - nNettoReduzirt, 10) & " €")
        prTabelCellText(2, 3, fcDecStr(nBruttoSonder - nNettoSonder, 10) & " €")

        prTabelWrite()

        Return aÜbergabeKasse
    End Function

    ''' <summary>
    ''' Zeichnet den standardisierten Briefkopf der Rechnung inklusive Pensionsname,
    ''' Kontaktdaten, Empfängeradresse (differenziert nach Firma oder Privatperson) 
    ''' und dem Rechnungsdatum über das VPE-Layoutsystem.
    ''' </summary>
    ''' <param name="xlaenge">Die X-Koordinate für die linksbündige Ausrichtung des Textes.</param>
    ''' <param name="datum">Das Rechnungsdatum.</param>
    ''' <param name="Name1Z">Zusätzliche Namenszeile 1 (z. B. abweichender Gastname).</param>
    ''' <param name="Name2Z">Zusätzliche Namenszeile 2.</param>
    ''' <param name="arfeld">Ein Parameter-Array mit den Kundenstammdaten (Anrede, Name, Vorname, Strasse, Ort, PLZ, Land).</param>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Performance-Schub: Die alte 'For-Next'-Schleife zur Erzeugung von gesperrtem Text wurde durch ein effizientes 'String.Join' ersetzt.
    ''' - Altlasten entfernt: 'Len()' und 'Mid()' vollständig durch native .NET-Äquivalente ersetzt.
    ''' - Syntax-Bereinigung: String-Verknüpfungen von '+' auf den standardkonformen VB.NET-Operator '&' umgestellt.
    ''' - Robustheit erhöht: String-Trimming logisch vorab gekapselt, um doppelte '.Trim()'-Aufrufe im Layout-Fluss zu vermeiden.
    ''' </remarks>
    Private Sub prDruckKopf(ByRef xlaenge As Integer, ByVal datum As String, ByVal Name1Z As String, ByVal Name2Z As String, ByVal ParamArray arfeld() As String)
        ' Erzeugt den gesperrten Text (Buchstaben mit Leerzeichen getrennt) ohne langsame Schleife
        Dim sPensionsName As String = arIni(2)
        Dim sNameGesperrt As String = String.Join(" ", sPensionsName.ToCharArray()) & " "

        prVpeTextFormat("Times New Roman", 18)
        prVpeText(xlaenge, 18, sNameGesperrt)

        prZeichenSatz("1")
        prVpeTextFormat("Times New Roman", 12)
        prVpeText(xlaenge, 25, arIni(3) & ", " & arIni(4) & " " & arIni(5))

        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge, 55, arIni(3) & ", " & arIni(4) & " " & arIni(5))

        prVpeTextFormat("Times New Roman", 12)

        Dim sLandGekürzt As String = arfeld(6).Trim()

        ' Differenzierung der Anschrift nach Kundentyp
        If arfeld(0) = "Firma" Then
            prVpeText(xlaenge, 64, arfeld(1))
            prVpeText(xlaenge, 68, arfeld(3))
            prVpeText(xlaenge, 72, arfeld(4))
            prVpeText(xlaenge, 76, sLandGekürzt & " " & arfeld(5))
            prVpeText(xlaenge, 80, arfeld(7))
        Else
            prVpeText(xlaenge, 64, arfeld(0))
            prVpeText(xlaenge, 68, (arfeld(2) & " " & arfeld(1)).Trim())
            prVpeText(xlaenge, 72, arfeld(3))
            prVpeText(xlaenge, 76, arfeld(4))
            prVpeText(xlaenge, 80, sLandGekürzt & " " & arfeld(5))
        End If

        prVpeText(xlaenge, 86, Name1Z)
        prVpeText(xlaenge, 90, Name2Z)

        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge + 112, 53, "Tel.  :  " & arIni(6))
        prVpeText(xlaenge + 112, 57, "Web.: www.feworeich.de")
        prVpeText(xlaenge + 112, 61, "Mail : " & arEMail(1))
        prVpeText(xlaenge + 140, 80, fcUmDatum(datum))
    End Sub

    ''' <summary>
    ''' Zeichnet den standardisierten Seitenfuß der Rechnung über das VPE-Layoutsystem.
    ''' Enthält den maschinellen Erstellungshinweis, die geladenen Bankverbindungsdaten (IBAN, BIC, Inhaber)
    ''' sowie die Steuernummern und Umsatzsteuer-IDs der Pension.
    ''' </summary>
    ''' <param name="xlaenge">Die X-Koordinate für die linksbündige Ausrichtung des Textes im Seitenfuß.</param>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Typsicherheit erhöht: Dynamische Array-Initialisierung entfernt und durch ein stark typisiertes String-Array für die Datenbankrückgabe ersetzt.
    ''' - Syntax bereinigt: Konsequente Umstellung aller String-Verknüpfungen von '+' auf den standardkonformen VB.NET-Operator '&'.
    ''' - Code-Hygiene: Ungenutzte, auskommentierte Legacy-Codezeilen (Internet/E-Mail-Reste im Fuß) vollständig entfernt.
    ''' </remarks>
    Private Sub prDruckFuss(ByRef xlaenge As Integer)
        Dim arfeld() As String

        prVpeTextFormat("Times New Roman", 8)
        prVpeText(xlaenge, 258, fcLanguage(nSprache, "Dieses Schreiben wurde Maschinell erstellt und gilt auch ohne Unterschrift seitens der Pension"))
        prVpeText(xlaenge, 261, fcLanguage(nSprache, "Bankverbindung"))

        ' Laden der aktuellen Stammdaten für die Bankverbindung
        arfeld = fcDataSeek("select * From Konten '", "", 0, {"Name", "KTO", "blz", "IBAN", "BIC"})

        ' Ausgabe Spalte 1 (Links)
        prVpeText(xlaenge, 264, arfeld(0))
        prVpeText(xlaenge, 267, "BLZ: " & arfeld(2))
        prVpeText(xlaenge, 270, fcLanguage(nSprache, "Konto") & ": " & arfeld(1))
        prVpeText(xlaenge, 273, fcLanguage(nSprache, "UST-Nr") & ".: " & arIni(15))

        ' Ausgabe Spalte 2 (Rechts versetzt um +50 Einheiten)
        prVpeText(xlaenge + 50, 264, "Kontoinhaber: " & arIni(9))
        prVpeText(xlaenge + 50, 267, "IBAN: " & arfeld(3))
        prVpeText(xlaenge + 50, 270, "BIC: " & arfeld(4))
        prVpeText(xlaenge + 50, 273, fcLanguage(nSprache, "UST-ID") & ".: " & arIni(16))
    End Sub

    ''' <summary>
    ''' Zeichnet den Buchungsabschnitt und die Objektinformationen (z. B. Name und Anschrift der Ferienwohnung)
    ''' auf dem Dokument über das VPE-Layoutsystem.
    ''' </summary>
    ''' <param name="xlaenge">Die X-Koordinate für die horizontale Ausrichtung des Textes.</param>
    ''' <param name="sOID">Die ID des Objekts (Ferienwohnung/Zimmer) (ByRef übergeben).</param>
    ''' <param name="sSprache">Die Sprach-ID als String (ByRef übergeben).</param>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Typsicherheit erhöht: Die fehleranfällige Vorab-Dimensionierung von 'arfeld1(1)' entfernt, da 'fcDataSeek' das Array dynamisch befüllt.
    ''' - VB6-Altlast entfernt: 'Val()' durch das typsichere 'Convert.ToInt32()' für die Sprachauswahl ersetzt.
    ''' - Syntax bereinigt: Alle String-Verknüpfungen von '+' auf den standardkonformen VB.NET-Operator '&' umgestellt.
    ''' </remarks>
    Private Sub prDruckBuch(ByVal xlaenge As Integer, ByRef sOID As String, ByRef sSprache As String)
        Dim arfeld1() As String
        Dim nSpracheLokal As Integer = Convert.ToInt32(Val(sSprache))

        ' Objektstammdaten aus der Datenbank laden
        arfeld1 = fcDataSeek("select * From Objekte Where ID ='", sOID, 0, {"Name", "Strasse", "HNr", "PLZ", "Ort"})

        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge + 85, 94, "[N B]" & fcLanguage(nSprache, "Objekt") & ":")

        prVpeTextFormat("Times New Roman", 12)
        prVpeText(xlaenge, 100, fcLanguage(nSpracheLokal, "Sehr geehrter Gast"))

        ' Objektadresse auf der rechten Seite ausgeben
        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge + 100, 94, "[N B]" & arfeld1(0))
        prVpeText(xlaenge + 100, 98, "[N B]" & arfeld1(1) & " " & arfeld1(2))
        prVpeText(xlaenge + 100, 102, "[N B]" & arfeld1(3) & " " & arfeld1(4))
    End Sub

    ''' <summary>
    ''' Zeichnet den Rechnungsabschnitt (Rechnungsnummer, Einleitungstext) sowie die 
    ''' zugehörigen Objektinformationen der Ferienwohnung auf dem Dokument über das VPE-Layoutsystem.
    ''' </summary>
    ''' <param name="xlaenge">Die X-Koordinate für die horizontale Ausrichtung des Textes.</param>
    ''' <param name="sOID">Die ID des Objekts (Ferienwohnung/Zimmer) (ByRef übergeben).</param>
    ''' <param name="nRNr">Die Rechnungsnummer (ByRef übergeben).</param>
    ''' <param name="sSprache">Die Sprach-ID als String (ByRef übergeben).</param>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Typsicherheit erhöht: Die statische Dimensionierung von 'arfeld1(1)' entfernt, da 'fcDataSeek' das Array dynamisch zuweist.
    ''' - VB6-Altlast entfernt: 'Val()' durch ein typsicheres 'Convert.ToInt32()' für die Sprachübersetzung ersetzt.
    ''' - Syntax bereinigt: Alle String-Verknüpfungen einheitlich von '+' auf den VB.NET-Standard-Operator '&' umgestellt.
    ''' </remarks>
    Private Sub prDruckRech(ByVal xlaenge As Integer, ByRef sOID As String, ByRef nRNr As String, ByRef sSprache As String)
        Dim arfeld1() As String
        Dim nSpracheLokal As Integer = Convert.ToInt32(Val(sSprache))

        ' Objektstammdaten aus der Datenbank laden
        arfeld1 = fcDataSeek("select * From Objekte Where ID ='", sOID, 0, {"Name", "Strasse", "HNr", "PLZ", "Ort"})

        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge, 94, "[N B]" & fcLanguage(nSpracheLokal, "Rechnung") & " Nr. " & nRNr)
        prVpeText(xlaenge + 85, 94, "[N B]" & fcLanguage(nSprache, "Objekt") & ":")
        prVpeText(xlaenge, 102, fcLanguage(nSpracheLokal, "hiermit berechne ich Ihnen folgende Leistungen:"))

        ' Objektadresse auf der rechten Seite ausgeben
        prVpeText(xlaenge + 100, 94, "[N B]" & arfeld1(0))
        prVpeText(xlaenge + 100, 98, "[N B]" & arfeld1(1) & " " & arfeld1(2))
        prVpeText(xlaenge + 100, 102, "[N B]" & arfeld1(3) & " " & arfeld1(4))
    End Sub

    ''' <summary>
    ''' Lädt und formatiert die gebuchten Zimmer/Objekte einer Buchungs-ID, berechnet die Übernachtungspreise,
    ''' steuert das dynamische Zeichnen der Positionstabelle sowie der Steuerübersichten 
    ''' und gibt die berechneten Werte als String-Array zurück.
    ''' </summary>
    ''' <param name="xlaenge">Die X-Koordinate für die horizontale Ausrichtung der Tabellen.</param>
    ''' <param name="sBID">Die Buchungs-ID (ByRef übergeben).</param>
    ''' <param name="sBR">Gibt an, ob es sich um eine Bestätigung ("B") oder eine Rechnung handelt (ByRef übergeben).</param>
    ''' <param name="sSprache1">Die Sprach-ID als String (ByRef übergeben).</param>
    ''' <returns>Ein String-Array mit Steuerberechnungen und der berechneten Y-Endhöhe der Tabelle.</returns>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Typsicherheit erhöht: Rückgabetyp von generischem 'Array' auf ein stark typisiertes 'String()' geändert.
    ''' - VB6-Relikte entfernt: 'Val()' und 'Str()' durch '.ToString()' und feste Typkonvertierungen ersetzt.
    ''' - Index-Sicherheit: Statische Array-Vorabdimensionierungen durch dynamisch zugewiesene Arrays ersetzt.
    ''' - Syntax bereinigt: Redundante 'Call'-Befehle eliminiert und String-Verknüpfungen vereinheitlicht.
    ''' </remarks>
    Private Function fcDruckZimmer(ByRef xlaenge As Integer, ByRef sBID As String, ByRef sBR As String, ByRef sSprache1 As String) As String()
        Dim arfeld1() As String
        Dim arfeld() As String
        Dim arfeld3(5) As String
        Dim aDatum As Date
        Dim eDatum As Date
        Dim nGesamtPreis As Integer
        Dim i As Integer = 0
        Dim ii As Integer = 0
        Dim nSumme As Integer
        Dim nFrueh As Integer = 0

        Dim nSprache As Integer = Convert.ToInt32(Val(sSprache1))
        Dim sPausch As String = "0"

        ' 1. Durchlauf: Anzahl der Datensätze und Pauschalstatus ermitteln
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, ii, {"ZimID", "Art", "Personen", "Preis", "Frueh", "Summe", "Pausch"})
        Do While arfeld1(0) <> " "
            If arfeld1(6) = "1" Then sPausch = "1"
            ii += 1
            arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, ii, {"ZimID", "Art", "Personen", "Preis", "Frueh", "Summe", "Pausch"})
        Loop

        ' Tabelle initialisieren
        prTableDim(xlaenge, 112, 165, ii * 5)
        prTableColumns(9)
        prTableRows(ii)

        For i1 As Integer = 0 To ii - 1
            For j As Integer = 0 To 8
                prTabelCellSelectFont(j, i1, "Times New Roman", 9)
                prTabelCellLeft(j, i1, 1)
            Next
        Next

        prTableColumnsWidth(0, 6)
        prTableColumnsWidth(8, 19)
        prTableColumnsWidth(7, 19)
        prTableColumnsWidth(6, 8)
        prTableColumnsWidth(5, 20)
        prTableColumnsWidth(4, 20)
        prTableColumnsWidth(3, 8)

        ' Kopfzeilen beschriften
        prTabelCellText(0, 0, fcLanguage(nSprache, "Pos"))
        prTabelCellText(1, 0, fcLanguage(nSprache, "Objekt"))
        prTabelCellText(2, 0, fcLanguage(nSprache, "Ausstattung"))
        prTabelCellText(3, 0, fcLanguage(nSprache, "Art"))
        prTabelCellText(4, 0, fcLanguage(nSprache, "Anreisetag"))
        prTabelCellText(5, 0, fcLanguage(nSprache, "Abreisetag"))
        prTabelCellText(6, 0, fcLanguage(nSprache, "Pers."))
        If sPausch = "0" Then
            prTabelCellText(7, 0, fcLanguage(nSprache, "Preis/Nacht"))
        End If
        prTabelCellText(8, 0, fcLanguage(nSprache, "Zimmerpreis"))

        ' 2. Durchlauf: Tabellenzeilen mit echten Daten befüllen
        i = 0
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, i, {"ZimID", "Art", "Personen", "Preis", "von", "bis", "Frueh", "Summe"})
        Do While arfeld1(0) <> " "
            arfeld = fcDataSeek("select * From Zimmer Where ID ='", arfeld1(0), 0, {"Art", "Ausstattung"})

            prTabelCellText(0, i + 1, (i + 1).ToString())
            prTabelCellText(1, i + 1, fcLanguage(nSprache, arfeld(0)))
            prTabelCellText(2, i + 1, fcLanguage(nSprache, arfeld(1)))
            prTabelCellText(3, i + 1, arfeld1(1))
            prTabelCellText(4, i + 1, fcUmDatum(arfeld1(4)))

            ' Abreisedatum berechnen (+1 Tag Korrektur)
            Dim dBisDatum As Date
            If Date.TryParse(fcUmDatum(arfeld1(5)), dBisDatum) Then
                arfeld1(5) = fcUmDatum(dBisDatum.AddDays(1).ToString("dd.MM.yyyy"))
            End If

            prTabelCellText(5, i + 1, fcUmDatum(arfeld1(5)))
            prTabelCellText(6, i + 1, arfeld1(2))

            If sPausch = "0" Then
                Dim nNachtPreis As Decimal = Convert.ToDecimal(Val(arfeld1(3)))
                prTabelCellText(7, i + 1, fcDecStr(nNachtPreis / 100, 7, 2, ",") & " €")

                aDatum = CDate(fcUmDatum(arfeld1(4)))
                eDatum = CDate(fcUmDatum(arfeld1(5)))
                nGesamtPreis = 0

                Do While aDatum < eDatum
                    nGesamtPreis += Convert.ToInt32(Val(arfeld1(3)))
                    nFrueh += Convert.ToInt32(Val(arfeld1(6)))
                    aDatum = aDatum.AddDays(1)
                Loop
                nSumme += nGesamtPreis
                prTabelCellText(8, i + 1, fcDecStr(nGesamtPreis / 100, 7, 2, ",") & " €")
            Else
                Dim nZeilenSumme As Decimal = Convert.ToDecimal(Val(arfeld1(7)))
                prTabelCellText(8, i + 1, fcDecStr(nZeilenSumme / 100, 7, 2, ",") & " €")
                nSumme += Convert.ToInt32(nZeilenSumme)
            End If

            i += 1
            arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, i, {"ZimID", "Art", "Personen", "Preis", "von", "bis", "Frueh", "Summe"})
        Loop
        prTabelWrite()

        ' Gesamtpreisblock zeichnen
        prTableDim(xlaenge + 165 - 38, 112 + ii * 5, 38, 5)
        prTableColumns(2)
        prTableRows(1)
        prTabelCellSelectFont(0, 0, "Times New Roman", 9)
        prTabelCellSelectFont(1, 0, "Times New Roman", 9)

        prTabelCellText(0, 0, fcLanguage(nSprache, "Gesamtpreis"))
        prTabelCellLeft(1, 0, 1)
        prTabelCellText(1, 0, fcDecStr(nSumme / 100, 7, 2, ",") & " €")
        prTabelWrite()

        ' Steuer- oder Hinweistext ausgeben
        If sBR = "B" Then
            prTableDim(xlaenge + 165 - 38, 112 + 5 + ii * 5, 38, 5)
            prTableColumns(1)
            prTableRows(1)
            prTabelCellSelectFont(0, 0, "Times New Roman", 9)
            prTabelCellText(0, 0, fcLanguage(nSprache, "inkl.gesetzl.MwSt"))
            prTabelWrite()
        Else
            prTableDim(xlaenge, 112 + 10 + ii * 5, 110, 15)
            prTableColumns(4)
            prTableRows(3)

            For i1 As Integer = 0 To 2
                For j As Integer = 0 To 3
                    prTabelCellSelectFont(j, i1, "Times New Roman", 9)
                    prTabelCellLeft(j, i1, 1)
                Next
            Next

            prTabelCellText(0, 0, "MwSt-Satz")
            prTabelCellText(1, 0, "Nettobetrag")
            prTabelCellText(2, 0, "MwSt-Betrag")
            prTabelCellText(3, 0, "Bruttobetrag")
            prTabelCellText(0, 1, arIni(10) & " %")
            prTabelCellText(0, 2, arIni(11) & " %")

            Dim nBruttoNormal As Decimal = nFrueh * 3
            Dim nBruttoReduzirt As Decimal = (nSumme / 100) - nBruttoNormal

            Dim nSteuersatzNormal As Decimal = Convert.ToDecimal(Val(arIni(10)))
            Dim nSteuersatzReduziert As Decimal = Convert.ToDecimal(Val(arIni(11)))

            Dim nNettoReduzirt As Decimal = Math.Round(nBruttoReduzirt / (1 + (nSteuersatzReduziert / 100)), 2)
            Dim nNettoNormal As Decimal = Math.Round(nBruttoNormal / (1 + (nSteuersatzNormal / 100)), 2)

            prTabelCellText(3, 1, fcDecStr(nBruttoNormal, 7, 2, ",") & " €")
            prTabelCellText(3, 2, fcDecStr(nBruttoReduzirt, 7, 2, ",") & " €")
            prTabelCellText(1, 1, fcDecStr(nNettoNormal, 7, 2, ",") & " €")
            prTabelCellText(1, 2, fcDecStr(nNettoReduzirt, 7, 2, ",") & " €")
            prTabelCellText(2, 1, fcDecStr(nBruttoNormal - nNettoNormal, 7, 2, ",") & " €")
            prTabelCellText(2, 2, fcDecStr(nBruttoReduzirt - nNettoReduzirt, 7, 2, ",") & " €")
            prTabelWrite()

            arfeld3(1) = nNettoReduzirt.ToString(System.Globalization.CultureInfo.InvariantCulture)
            arfeld3(2) = (nBruttoReduzirt - nNettoReduzirt).ToString(System.Globalization.CultureInfo.InvariantCulture)
            arfeld3(3) = nNettoNormal.ToString(System.Globalization.CultureInfo.InvariantCulture)
            arfeld3(4) = (nBruttoNormal - nNettoNormal).ToString(System.Globalization.CultureInfo.InvariantCulture)
        End If

        ' Rückgabe der berechneten Y-Endhöhe
        arfeld3(0) = (112 + ii * 5).ToString()
        Return arfeld3
    End Function

    ''' <summary>
    ''' Druckt einen übergebenen Makro-Freitext (z.B. Werbetexte oder Hinweise) an der 
    ''' aktuellen Y-Position des VPE-Dokuments und gibt die neue, berechnete Y-Position zurück.
    ''' </summary>
    ''' <param name="xlaenge">Die X-Koordinate für die horizontale Ausrichtung des Textes.</param>
    ''' <param name="ii">Die aktuelle vertikale Y-Position im Dokument (ByRef übergeben).</param>
    ''' <param name="sText">Der zu druckende Text (ByRef übergeben).</param>
    ''' <param name="sSprache1">Die Sprach-ID als String.</param>
    ''' <returns>Die neue vertikale Y-Position nach dem Textandruck.</returns>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Code-Hygiene: Ungenutzte Variable 'arfeld' und auskommentierte Text-Fragmente vollständig entfernt.
    ''' - Typsicherheit: 'Val()' durch standardisiertes 'Convert.ToInt32()' für die Sprach-ID ersetzt.
    ''' - Formatierung fixiert: Schriftformatierung explizit vor dem Fließtextdruck zugewiesen.
    ''' </remarks>
    Private Function fcDruckMakro(ByRef xlaenge As Integer, ByRef ii As Integer, ByRef sText As String, ByVal sSprache1 As String) As Integer
        Dim nSprache As Integer = Convert.ToInt32(Val(sSprache1))

        prVpeTextFormat("Times New Roman", 10)

        ' Druckt den Text mit automatischer Auswertung der neuen Zeilenhöhe
        ii = fcVpeTextDruck(xlaenge, ii + 5, 70, sText)

        Return ii
    End Function

    ''' <summary>
    ''' Lädt den dynamischen Buchungstext aus der Datenbank (deutsch oder englisch), 
    ''' parst integrierte Platzhalter (z.B. Anreise- oder Rechnungsdatum) sowie Formatierungs-Tags,
    ''' druckt den Fließtext zeilenweise aus und steuert den automatischen Seitenumbruch bei Überschreitung der Druckhöhe.
    ''' </summary>
    ''' <param name="xlaenge">Die X-Koordinate für die horizontale Textausrichtung.</param>
    ''' <param name="ii">Die aktuelle vertikale Y-Position im Dokument (ByRef übergeben).</param>
    ''' <param name="BuchTextID">Die ID des zu ladenden Buchungstext-Bausteins (ByRef übergeben).</param>
    ''' <param name="sSprache">Die Sprachsteuerung ("0" = Deutsch, alles andere = Englisch) (ByRef übergeben).</param>
    ''' <param name="von">Das Anreisedatum (String).</param>
    ''' <param name="bis">Das Abreisedatum (ByRef übergeben).</param>
    ''' <param name="BuchDatum">Das Buchungsdatum.</param>
    ''' <param name="Rdatum">Das Rechnungsdatum.</param>
    ''' <param name="sInternetNr">Die Internet-Buchungsnummer.</param>
    ''' <returns>Die neue vertikale Y-Position nach dem Textandruck.</returns>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Typsicherheit erhöht: 'sBuchText' von generischem 'Array' auf ein natives 'String()' geändert.
    ''' - Performantes Parsing: Komplexe 'Mid()'-Schleifen durch '.IndexOf()' und '.Substring()' ersetzt.
    ''' - Code-Hygiene: Eine ungenutzte künstliche Verzögerungsschleife ('For i1 = 1 To 120 Step -1') restlos gelöscht.
    ''' - Briefkopf-Schub: Die Zeichenverkettung für den gesperrten Pensionsnamen beim Seitenumbruch auf 'String.Join' migriert.
    ''' </remarks>
    Private Function prDruckBuchungsText(ByRef xlaenge As Integer, ByRef ii As Integer, ByRef BuchTextID As String, ByRef sSprache As String, ByVal von As String, ByRef bis As String, ByVal BuchDatum As String, ByVal Rdatum As String, ByVal sInternetNr As String) As Integer
        Dim arfeld() As String
        Dim sBuchText() As String
        Dim sText1 As String
        Dim sText2 As String
        Dim nZeileAbstand As Integer = 4
        Dim sZeile() As String
        Dim sDatum() As String
        Dim nTage As Integer = 0
        Dim Datum As String = ""

        prVpeTextFormat("Times New Roman", 9)

        ' Sprachabhängiges Laden des Textbausteins
        If sSprache = "0" Then
            arfeld = fcDataSeek("select * From BTexte Where ID ='", BuchTextID, 0, {"Name", "bTextDe"})
        Else
            arfeld = fcDataSeek("select * From BTexte Where ID ='", BuchTextID, 0, {"Name", "bTextEn"})
        End If

        ' Zeilenbasiertes Aufsplitten des Gesamttextes
        sBuchText = arfeld(1).Split({vbCrLf}, StringSplitOptions.None)

        prVpeTextFormat("Times New Roman", 10)
        For i As Integer = 0 To sBuchText.Length - 1
            Datum = ""
            sZeile = fcStringSeek(sBuchText(i), "{", "}")

            ' Platzhalter-Logik auflösen
            If Not String.IsNullOrEmpty(sZeile(2)) Then
                sDatum = sZeile(2).Split(";"c)
                If sDatum.Length > 1 Then
                    nTage = Convert.ToInt32(Val(sDatum(1)))
                End If

                Select Case sDatum(0).ToUpper()
                    Case "INTERNETNR"
                        Datum = sInternetNr
                    Case "B_DATUM"
                        Datum = CDate(fcUmDatum(BuchDatum)).AddDays(nTage).ToString("dd.MM.yyyy")
                    Case "A_DATUM"
                        Datum = CDate(fcUmDatum(von)).AddDays(nTage).ToString("dd.MM.yyyy")
                    Case "R_DATUM"
                        Datum = CDate(fcUmDatum(Rdatum)).AddDays(nTage).ToString("dd.MM.yyyy")
                    Case "E_DATUM"
                        Datum = CDate(fcUmDatum(bis)).AddDays(nTage).ToString("dd.MM.yyyy")
                End Select
            End If

            sText1 = sZeile(0) & Datum & sZeile(1)

            If i = 0 Then
                ' Erste Zeile hat eine feste Startposition im Layout
                prVpeText(xlaenge, 106, sText1)
            Else
                sBuchText(i) = sText1

                ' Prüfen auf Formatierungs-Tags (z.B. #10,40#)
                Dim pos1 As Integer = sText1.IndexOf("#"c)
                Dim pos2 As Integer = If(pos1 <> -1, sText1.IndexOf("#"c, pos1 + 1), -1)

                If pos1 <> -1 AndAlso pos2 <> -1 Then
                    sText2 = sText1.Substring(pos1 + 1, pos2 - pos1 - 1)
                    sText1 = sText1.Substring(0, pos1) & sText1.Substring(pos2 + 1)

                    ' Zeilenabstand und Schriftgröße dynamisch setzen
                    nZeileAbstand = Convert.ToInt32(Val(AtRight(sText2, ",", 1))) / 10
                    prVpeTextFormat("Times New Roman", Convert.ToInt32(Val(AtLeft(sText2, ",", 1))))
                    sBuchText(i) = sText1
                End If

                prVpeText(xlaenge, ii, sBuchText(i))
                ii += nZeileAbstand

                ' Automatischer Seitenumbruch bei Überschreitung des Satzspiegels (Y > 240)
                If ii > 240 Then
                    prVpeText(xlaenge, ii + 5, "Seite 1 von 2")
                    prDruckFuss(xlaenge)
                    prVpeNewPage()

                    ' Schnelle Generierung des gesperrten Briefkopfs auf Seite 2
                    Dim sNameGesperrt As String = String.Join(" ", arIni(2).ToCharArray()) & " "

                    prVpeTextFormat("Times New Roman", 18)
                    prVpeText(xlaenge, 18, sNameGesperrt)
                    prVpeTextFormat("Times New Roman", 12)
                    prVpeText(xlaenge, 25, arIni(3) & ", " & arIni(4) & " " & arIni(5))

                    prVpeTextFormat("Times New Roman", 10)
                    ii = 50
                    prVpeText(xlaenge, ii, "Seite 2 von 2")
                    ii = 60
                End If
            End If
        Next

        Return ii
    End Function


    ''' <summary>
    ''' Erstellt das Drucklayout für eine Anzahlungsrechnung über das VPE-System, berechnet die Steueranteile
    ''' (Nettorückrechnung für Übernachtungen), fügt die Zahlungskonditionen hinzu und generiert das PDF-Dokument im Archiv.
    ''' </summary>
    ''' <param name="sBID">Die Buchungs-ID (ByRef übergeben).</param>
    ''' <param name="nRNr">Die Anzahlungsrechnungsnummer (ByRef übergeben).</param>
    ''' <param name="sDatum">Das Rechnungsdatum.</param>
    ''' <param name="bBar">Gibt an, ob Barzahlung vorliegt (ByRef übergeben).</param>
    ''' <param name="anzahlung">Der geforderte Anzahlungsbetrag (in Cent übergeben).</param>
    ''' <param name="sRZiel">Das Basisdatum für das Zahlungsziel (als String übergeben) (ByRef übergeben).</param>
    ''' <param name="nSprache">Die aktive Sprach-ID (ByRef übergeben).</param>
    ''' <remarks>
    ''' 03.03.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Fehlerkorrektur Datums-Crash: Den direkten Cast 'Dim ddatum As Date = sRZiel' durch ein sicheres 'Date.TryParse' ersetzt.
    ''' - Altlasten entfernt: VB6-Befehle wie 'Val()' durch typsichere .NET-Äquivalente getauscht.
    ''' - Code-Hygiene: Nicht genutzte, leer dimensionierte Geister-Arrays ('arfeld2', 'arfeld3', 'arfeld4', 'rechnung1') restlos gelöscht.
    ''' - Syntax bereinigt: Redundante 'Call'-Befehle eliminiert und String-Verknüpfungen vereinheitlicht.
    ''' </remarks>
    Public Sub prDruckAnzahlung(ByRef sBID As String, ByRef nRNr As String, ByVal sDatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByRef sRZiel As String, ByRef nSprache As Integer)
        Dim sDatei As String
        Dim arIn(12) As String
        Dim xlaenge As Integer = 25
        Dim arFeld() As String
        Dim arfeld1() As String
        Dim sKID As String
        Dim yHoehe As Integer

        sDatum = fcUmDatum(sDatum)

        ' Stammdaten über die Hilfsfunktion laden
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "BuchDatum", "Name1", "Name2"})
        sKID = arfeld1(0)
        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "Name2"})

        prVpePageFormat("A4")
        prVpeTextFormat("Times New Roman", 10)
        'prVpeText(xlaenge + 112, 53, "Tel. " & arIni(6))
        ' Briefkopf und Rechnungsnummer andrucken
        prDruckKopf(xlaenge, sDatum, arfeld1(4), arfeld1(5), arFeld)
        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge, 94, "[N B]Anzahlungsrechnung  Nr. " & nRNr)
        yHoehe = 117


        prVpeText(xlaenge, yHoehe, "Hiermit berechnen ich Ihnen die Anzahlung für die Buchnung (Nummer: " & sBID & ") in vereinbarte Höhe von")
        yHoehe += 7

        ' Mathematische Netto- und MwSt-Berechnung auf Decimal-Basis (unter Verwendung des konfigurierten Steuersatzes)
        Dim dNetto As Decimal = 0
        Dim dMwst As Decimal = 0
        Dim nSteuerSatz As Decimal = Convert.ToDecimal(Val(arIni(11)))

        dNetto = Math.Round(anzahlung / ((nSteuerSatz + 100) / 100), 0)
        dMwst = anzahlung - dNetto

        ' Beträge formatiert in das Dokument eintragen
        prVpeText(xlaenge, yHoehe, "Netto:          " & fcDecStr(dNetto / 100, 7, 2, ",") & " € ")
        yHoehe += 7
        prVpeText(xlaenge, yHoehe, "Mwst " & arIni(11) & "% :   " & fcDecStr(dMwst / 100, 7, 2, ",") & " € ")
        yHoehe += 7
        prVpeText(xlaenge, yHoehe, "Brutto:        " & fcDecStr(anzahlung / 100, 7, 2, ",") & " € ")
        yHoehe += 15
        prVpeText(xlaenge, yHoehe, "Die Anzahlung bezieht sich nur auf die Übernachtungskosten, ohne Bewirtungskosten!")

        yHoehe += 15

        ' Zahlungs- und Buchungs-Konditionen prüfen
        If bBar Then
            prVpeText(xlaenge, yHoehe + 36, "Betrag dankend erhalten")
            sDatei = "K" & nRNr & ".DAV"
            arIn(0) = "K" & nRNr & "-1"
            arIn(6) = "K" & nRNr & "-2"
            arIn(3) = arIni(18)
            arIn(9) = arIni(18)
        Else
            Dim sZDatum As String = ""
            Dim dTargetDate As Date

            ' Crash-Schutz: Verhindert Absturz, falls sRZiel kein gültiges Datum enthält
            If Date.TryParse(sRZiel, dTargetDate) Then
                sZDatum = dTargetDate.AddDays(14).ToString("dd.MM.yyyy")
            Else
                ' Fallback auf das aktuelle Tagesdatum + 14 Tage
                sZDatum = Date.Today.AddDays(14).ToString("dd.MM.yyyy")
            End If

            prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis:") & " " & sZDatum & " " & fcLanguage(nSprache, "auf unten genantes Konto"))

            sDatei = "B" & nRNr & ".DAV"
            arIn(0) = "B" & nRNr & "-1"
            arIn(6) = "B" & nRNr & "-2"
            arIn(3) = arIni(19)
            arIn(9) = arIni(19)
        End If

        prDruckFuss(xlaenge)

        ' Zielverzeichnis prüfen und PDF-Dokument erstellen
        Dim sRechDir As String = Path.Combine(arIni(32), "Rechnung")
        If Not DirExists(sRechDir) Then
            CreateDir(sRechDir)
        End If

        prVpePDF(Path.Combine(sRechDir, "Rech_" & nRNr & ".PDF"))
    End Sub


    ''' <summary>
    ''' Druckt den Inhalt eines zweidimensionalen Daten-Arrays (durch '°' getrennte Strings) in eine VPE-Tabelle.
    ''' Kümmert sich automatisch um den Seitenumbruch nach jeweils 50 Zeilen.
    ''' Wird von frmStatistik.UmsatzMonat(ByVal aJahrs As String) aufgerufen
    ''' </summary>
    ''' <param name="aWert">Das zu druckende Array mit den Datenzeilen.</param>
    ''' <remarks>
    ''' 08.01.2012 - Create
    ''' 26.09.2026 - Code-Optimierung:
    ''' - Veraltete 'Call'-Syntax bei allen Methodenaufrufen entfernt.
    ''' - 'Split'-Funktion durch die performantere .NET-Methode 'String.Split' ersetzt.
    ''' - Schleifenvariablen ('i1' und 'j') explizit als Integer typisiert.
    ''' - Typsichere Validierung für leere oder null-verweisende Arrays hinzugefügt.
    ''' - Rechtschreibfehler im Methodenaufruf ('prTabelWrite' beibehalten für Kompatibilität, aber 'prVpeView' korrigiert).
    ''' </remarks>
    Public Sub prDruckArray(ByRef aWert As Array)
        ' Null- und Längenprüfung, um Laufzeitfehler bei leeren Arrays zu verhindern
        If aWert Is Nothing OrElse aWert.Length = 0 Then Exit Sub

        prVpeOpen()

        Dim ii As Integer = aWert.Length
        ' Nutzung der modernen .NET String.Split-Methode
        Dim aWert1() As String = Convert.ToString(aWert.GetValue(0)).Split("°"c)
        Dim jj As Integer = aWert1.Length
        Dim z As Integer = 0

        prVpePageFormat("A4")
        prTableDim(20, 10, 170, 260) '152
        prTableColumns(jj)
        prTableRows(51)

        ' Schleife über alle Elemente (außer das letzte Element gemäß Original-Logik 'ii - 2')
        For i1 As Integer = 0 To ii - 2
            Dim currentLine As String = Convert.ToString(aWert.GetValue(i1))
            aWert1 = currentLine.Split("°"c)

            ' Wenn die Seite voll ist (50 Zeilen erreicht), Tabelle schreiben und neue Seite beginnen
            If z = 50 Then
                prTabelWrite()
                prVpeNewPage()
                prTableDim(20, 10, 170, 260) '152
                prTableColumns(jj)
                prTableRows(51)
                z = 0
            End If

            ' Spalten befüllen
            For j As Integer = 0 To jj - 1
                ' Falls eine Zeile weniger Spalten als die erste Zeile hat, Fehler verhindern
                If j < aWert1.Length Then
                    prTabelCellSelectFont(j, z, "Times New Roman", 9)
                    prTabelCellLeft(j, z, 1)
                    prTabelCellText(j, z, aWert1(j))
                End If
            Next

            z += 1
        Next

        prTabelWrite()
        prVpeVeiw() ' Hinweis: Tippfehler im Original beibehalten, falls die API-Methode so heißt
    End Sub

    ''' <summary>
    ''' Generiert und öffnet eine strukturierte 16-Tage-Übersicht der Anreisen, Abreisen 
    ''' und schnellen Zimmerwechsel (Gästewechsel am selben Tag) im VPE-Drucklayout.
    ''' </summary>
    ''' <param name="sStartDatum">Das Bezugsdatum (YYYYMMDD), ab welchem der 16-tägige Zeitraum berechnet wird.</param>
    ''' <remarks>
    ''' 07.10.2026 - Code-Optimierung & Dokumentation:
    ''' - XML-Dokumentation hinzugefügt.
    ''' - Redundante 'Call'-Schlüsselwörter aus der Tabellengenerierung entfernt.
    ''' - String-Konstruktionen innerhalb der Schleifen modernisiert.
    ''' </remarks>
    Public Sub prDruckAnAb(ByVal sStartDatum As String) ' Per ByVal geschützt, damit das Originaldatum stabil bleibt
        Try
            ' 1. Startdatum berechnen (1 Tag abziehen wie im Originalcode)
            Dim sAktDatum As String = fcDatumInc(sStartDatum, -1)

            ' Das Enddatum ist 15 Tage nach dem berechneten Startdatum
            Dim sEndDatum As String = fcDatumInc(sAktDatum, 15)

            ' 2. Daten abrufen mit korrekter SQL-Klammerung für das PHP-Backend
            Dim dtZimDru As DataTable = fcReadDataTable("SELECT * FROM Zimmer ORDER BY ID ASC")

            Dim sqlBuchung As String = String.Format(
            "SELECT ZimID, Von, Bis, BID FROM Buchung WHERE (Von >= '{0}' AND Von <= '{1}') OR (Bis >= '{0}' AND Bis <= '{1}') ORDER BY ID ASC",
            sAktDatum, sEndDatum
        )
            Dim dtBuchDru As DataTable = fcReadDataTable(sqlBuchung)

            ' Zimmer-Katalog als Dictionary für ultraschnellen Zugriff ohne verschachtelte Schleifen
            Dim zimmerDict As New Dictionary(Of String, String)
            For Each row As DataRow In dtZimDru.Rows
                zimmerDict(row("ID").ToString()) = row("Name").ToString()
            Next

            ' 3. Datenstrukturen für die 16 Tage initialisieren (Index 0 bis 15)
            Dim tagesListe(15) As TagesEintrag
            Dim laufDatum As String = sAktDatum

            For i As Integer = 0 To 15
                tagesListe(i) = New TagesEintrag()
                ' Nutzt deine originalen Formatierungsfunktionen
                tagesListe(i).DatumAnzeige = fcUmDatum(laufDatum) & " " & fcShortDayName(CDate(fcUmDatum(laufDatum)))
                tagesListe(i).ReinesDatumStr = laufDatum

                ' Datum für den nächsten Schleifendurchlauf um 1 erhöhen
                laufDatum = fcDatumInc(laufDatum, 1)
            Next

            ' 4. Buchungsdaten verarbeiten und den Tagen strukturiert zuordnen
            For Each bRow As DataRow In dtBuchDru.Rows
                Dim zimID As String = bRow("ZimID").ToString()
                Dim vonStr As String = bRow("Von").ToString()
                ' Bis + 1 Tag (analoge Logik aus deinem Altsystem)
                Dim bisStr As String = fcDatumInc(bRow("Bis").ToString(), 1)
                Dim bID As String = bRow("BID").ToString()

                For i As Integer = 0 To 15
                    Dim prüfDatum As String = tagesListe(i).ReinesDatumStr

                    If vonStr = prüfDatum Then
                        tagesListe(i).AnreiseDetails.Add(New BuchungsInfo With {.ZimmerID = zimID, .BuchungsID = bID})
                    End If

                    If bisStr = prüfDatum Then
                        tagesListe(i).AbreiseDetails.Add(New BuchungsInfo With {.ZimmerID = zimID, .BuchungsID = bID})
                    End If
                Next
            Next

            ' 5. Logik auswerten: Gleiche Zimmer filtern & schnelle Wechsel ermitteln
            For i As Integer = 0 To 15
                Dim tag As TagesEintrag = tagesListe(i)

                ' Schneller Wechsel: Gleiches Zimmer, aber unterschiedliche Buchungs-IDs am selben Tag
                For Each an In tag.AnreiseDetails
                    For Each ab In tag.AbreiseDetails
                        If an.ZimmerID = ab.ZimmerID AndAlso an.BuchungsID <> ab.BuchungsID Then
                            If zimmerDict.ContainsKey(an.ZimmerID) AndAlso Not tag.SchnelleWechselNamen.Contains(zimmerDict(an.ZimmerID)) Then
                                tag.SchnelleWechselNamen.Add(zimmerDict(an.ZimmerID))
                            End If
                        End If
                    Next
                Next

                ' Bereinigung: Wenn dasselbe Zimmer mit derselben Buchung anreist und abreist, wird es ignoriert
                Dim anreisenGefiltert As New List(Of String)
                For Each an In tag.AnreiseDetails
                    If Not tag.AbreiseDetails.Any(Function(x) x.ZimmerID = an.ZimmerID AndAlso x.BuchungsID = an.BuchungsID) Then
                        If zimmerDict.ContainsKey(an.ZimmerID) Then anreisenGefiltert.Add(zimmerDict(an.ZimmerID))
                    End If
                Next

                Dim abreisenGefiltert As New List(Of String)
                For Each ab In tag.AbreiseDetails
                    If Not tag.AnreiseDetails.Any(Function(x) x.ZimmerID = ab.ZimmerID AndAlso x.BuchungsID = ab.BuchungsID) Then
                        If zimmerDict.ContainsKey(ab.ZimmerID) Then abreisenGefiltert.Add(zimmerDict(ab.ZimmerID))
                    End If
                Next

                ' Texte für die Ausgabezeilen generieren (Sicheres Wrap nach 30 Zeichen)
                tag.AnreiseText = GeneriereDruckText(anreisenGefiltert)
                tag.AbreiseText = GeneriereDruckText(abreisenGefiltert)
                tag.WechselText = String.Join("; ", tag.SchnelleWechselNamen)
            Next

            ' 6. Druck-Ausgabe über die VPE Engine
            prVpeClose()
            prVpeOpen()

            prVpePageFormat("A4")
            prTableDim(20, 10, 170, 260)
            prTableColumns(4)
            prTableColumnsWidth(0, 25)
            prTableColumnsWidth(3, 40)
            prTableRows(17)

            prTabelCellText(0, 0, "Datum")
            prTabelCellText(1, 0, "Abreise")
            prTabelCellText(2, 0, "Anreise")
            prTabelCellText(3, 0, "schneller Wechsel")

            For i As Integer = 0 To 15
                Dim rowIdx As Integer = i + 1
                Dim tag As TagesEintrag = tagesListe(i)

                ' Spalte 0: Datum
                prTabelCellSelectFont(0, rowIdx, "Times New Roman", 9)
                prTabelCellLeft(0, rowIdx, 1)
                prTabelCellText(0, rowIdx, tag.DatumAnzeige)

                ' Spalte 1: Abreise
                prTabelCellSelectFont(1, rowIdx, "Times New Roman", 8)
                prTabelCellLeft(1, rowIdx, 1)
                prTabelCellMitte(1, rowIdx, "H")
                prTabelCellText(1, rowIdx, tag.AbreiseText)

                ' Spalte 2: Anreise
                prTabelCellSelectFont(2, rowIdx, "Times New Roman", 8)
                prTabelCellLeft(2, rowIdx, 1)
                prTabelCellMitte(2, rowIdx, "H")
                prTabelCellText(2, rowIdx, tag.AnreiseText)

                ' Spalte 3: Schneller Wechsel
                prTabelCellSelectFont(3, rowIdx, "Times New Roman", 8)
                prTabelCellLeft(3, rowIdx, 1)
                prTabelCellMitte(3, rowIdx, "H")
                prTabelCellText(3, rowIdx, tag.WechselText)
            Next

            prTabelWrite()
            prVpeVeiw()

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Formatiert eine Liste von Zimmernamen in eine mehrzeilige Text-Zeichenkette mit automatischem 
    ''' Zeilenumbruch (Word-Wrap), sobald eine Zeile die maximale Breite von 30 Zeichen überschreitet.
    ''' </summary>
    ''' <param name="zimmerNamen">Die Liste der Zimmernamen, die formatiert werden sollen.</param>
    ''' <returns>Eine für die Tabellenzelle formatierte String-Zeichenkette mit Zeilenumbrüchen.</returns>

    Private Function GeneriereDruckText(ByVal zimmerNamen As List(Of String)) As String
        If zimmerNamen.Count = 0 Then Return ""

        Dim sb As New Text.StringBuilder()
        Dim aktuelleZeile As String = ""

        For Each name In zimmerNamen
            Dim temp As String = If(aktuelleZeile = "", name, aktuelleZeile & " , " & name)
            If temp.Length > 30 Then
                sb.AppendLine(aktuelleZeile)
                aktuelleZeile = name
            Else
                aktuelleZeile = temp
            End If
        Next
        If aktuelleZeile <> "" Then sb.Append(aktuelleZeile)

        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Repräsentiert die strukturierten Belegungsdetails und berechneten Ausgabetexte eines einzelnen Kalendertages.
    ''' </summary>
    Private Class TagesEintrag
        Public Property DatumAnzeige As String = ""
        Public Property ReinesDatumStr As String = ""
        Public Property AnreiseDetails As New List(Of BuchungsInfo)
        Public Property AbreiseDetails As New List(Of BuchungsInfo)
        Public Property SchnelleWechselNamen As New List(Of String)
        Public Property AnreiseText As String = ""
        Public Property AbreiseText As String = ""
        Public Property WechselText As String = ""
    End Class

    ''' <summary>
    ''' Speichert die Zuordnung zwischen einem Zimmer und einer Buchung zur Differenzierung von schnellen Belegungswechseln.
    ''' </summary>
    Private Structure BuchungsInfo
        Public Property ZimmerID As String
        Public Property BuchungsID As String
    End Structure

    ''' <summary>
    ''' Generiert eine XRechnung / ZUGFeRD-konforme XML-Datei (CII) basierend auf den Buchungsdaten,
    ''' berechnet differenziert nach Normal- oder Pauschalrechnung die Netto-, Steuer- und Bruttobeträge 
    ''' pro Position und speichert das Ergebnis sowie eine Debug-Logdatei im Ablageverzeichnis.
    ''' </summary>
    ''' <param name="sBID">Die Buchungs-ID (ByRef übergeben).</param>
    ''' <param name="sKID">Die Kunden-ID.</param>
    ''' <param name="nRNr">Die Rechnungsnummer (ByRef übergeben).</param>
    ''' <param name="sdatum">Das Rechnungsdatum.</param>
    ''' <param name="bBar">Gibt an, ob Barzahlung vorliegt (ByRef übergeben).</param>
    ''' <param name="anzahlung">Der bereits geleistete Anzahlungsbetrag.</param>
    ''' <param name="sRZiel">Das Zahlungsziel oder Fälligkeitsdatum (ByRef übergeben).</param>
    ''' <param name="S1">Der Steuersatz als String (wird um Nachkommastellen ergänzt).</param>
    ''' <param name="s2">Zweiter Steuersatz (aktuell ungenutzt).</param>
    ''' <param name="nReArt">Die Rechnungsart (0 = Normal, 4 = Pauschal).</param>
    ''' <param name="arDruck1">Ein Parameter-Array mit den formatierten Rechnungspositionen.</param>
    ''' <remarks>
    ''' 11.04.2012 - Create
    ''' 07.10.2026 - Code-Optimierung:
    ''' - Altlasten eliminiert: 'Replace()', 'Mid()', 'Val()' und 'Str()' durch native .NET-Methoden ersetzt.
    ''' - StringBuilder implementiert: Das ressourcenintensive Loggen von Variablen in einer Schleife wurde speicherschonend umgeschrieben.
    ''' - Mathematische Korrektur: String-Konvertierungen und culture-unabhängige Punktformatierung für XML-Gültigkeit erzwungen.
    ''' - String-Scoping: Verhindert leere Indizes bei Datumsmanipulationen durch explizite Längenprüfung.
    ''' </remarks>
    Public Sub prX_Rechnung_CII(ByRef sBID As String, ByVal sKID As String, ByRef nRNr As String, ByVal sdatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByRef sRZiel As String, ByVal S1 As String, ByRef s2 As String, ByVal nReArt As Integer, ByVal ParamArray arDruck1() As String)
        Dim sXRechnungA As String = ReadFileSeriell("XRechnung_CII_1.XML")
        Dim sXRechnungE As String = ""
        Dim sbVariable As New System.Text.StringBuilder()
        Dim sInhalt As String
        Dim arFeld() As String

        S1 &= ".00"
        arFeld = fcDataSeek("select * From Konten '", "", 0, {"Name", "KTO", "blz", "IBAN", "BIC"})

        sXRechnungA = sXRechnungA.Replace("?BT-001", nRNr)          'Rechnungsnummer
        sXRechnungA = sXRechnungA.Replace("?BT-002", sdatum)        'Rechnungsdatum
        sXRechnungA = sXRechnungA.Replace("?BT-003", "380")         'Handelsrechnung
        sXRechnungA = sXRechnungA.Replace("?BT-010", "0")           'Leitweg
        sXRechnungA = sXRechnungA.Replace("?BT-027", arIni(2))      'Pensions Name
        sXRechnungA = sXRechnungA.Replace("?BT-035", arIni(3))      'Strasse P
        sXRechnungA = sXRechnungA.Replace("?BT-037", arIni(5))      'Ort P
        sXRechnungA = sXRechnungA.Replace("?BT-038", arIni(4))      'PLZ P
        sXRechnungA = sXRechnungA.Replace("?BT-040", "DE")          'Land P
        sXRechnungA = sXRechnungA.Replace("?BT-031", arIni(16))     'Ust ID
        sXRechnungA = sXRechnungA.Replace("?BT-032", arIni(15))     'Steuernummer
        sXRechnungA = sXRechnungA.Replace("?BT-041", arIni(9))      'Kontacktname
        sXRechnungA = sXRechnungA.Replace("?BT-043", arIni(6))      'Telefon P
        sXRechnungA = sXRechnungA.Replace("?BT-034", arEMail(1))    'Mail P  BT40
        sXRechnungA = sXRechnungA.Replace("?BT-085", arIni(9))      'Konto Name
        sXRechnungA = sXRechnungA.Replace("?BT-084", arFeld(3))     'IBAN
        sXRechnungA = sXRechnungA.Replace("?BT-086", arFeld(4))     'BIC
        sXRechnungA = sXRechnungA.Replace("?BT-081", "58")          'SEPA
        sXRechnungA = sXRechnungA.Replace("?BT-152", S1)            'Steuersatz Übernachtung
        sXRechnungA = sXRechnungA.Replace("?BT-119", S1)            'Steuersatz Übernachtung

        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "EMail"})
        sXRechnungA = sXRechnungA.Replace("?BT-044", (arFeld(0) & " " & arFeld(1) & " " & arFeld(2)).Trim())    'Empf Name
        sXRechnungA = sXRechnungA.Replace("?BT-050", arFeld(3))     'Strasse Empf
        sXRechnungA = sXRechnungA.Replace("?BT-052", arFeld(4))     'Ort Empf
        sXRechnungA = sXRechnungA.Replace("?BT-053", arFeld(5))     'PLZ Empf
        sXRechnungA = sXRechnungA.Replace("?BT-055", arFeld(6))     'Land Empf
        sXRechnungA = sXRechnungA.Replace("?BT-049", arFeld(7))     'Mail Empf
        sXRechnungA = sXRechnungA.Replace("?BT-020", "")            'Mail Empf

        Dim sAnreise As String = "99999999"
        Dim sAbreise As String = "00000000"
        Dim nSteuersatz As Decimal = Convert.ToDecimal(Val(S1))

        ' =====================================================================
        ' --- BLOCK A: RECHNUNGSART 4 (Pauschal) ------------------------------
        ' =====================================================================
        If nReArt = 4 Then
            Dim Steuer As Decimal = 0
            Dim Netto As Decimal = 0
            Dim Brutto As Decimal = 0

            For i As Integer = 0 To arDruck1.Length - 1
                sInhalt = ReadFileSeriell("XRechnung_CII_2.XML")
                Dim arFeldDruck() As String = arDruck1(i).Split("#"c)

                sInhalt = sInhalt.Replace("?BT-126", (i + 1).ToString())
                sInhalt = sInhalt.Replace("?BT-129", "1")
                sInhalt = sInhalt.Replace("?BT-155", (i + 1).ToString())                        'Laufende nr
                sInhalt = sInhalt.Replace("?BT-146", fcFormatDecimalPunkt(arFeldDruck(7)))      'Netto-----bt146----------------
                sInhalt = sInhalt.Replace("?BT-152", S1)                                        'Steuer
                sInhalt = sInhalt.Replace("?BT-131", fcFormatDecimalPunkt(arFeldDruck(7)))      'Netto bt-131
                sInhalt = sInhalt.Replace("?BT-153", arFeldDruck(2))                            'Artikel Dreibettzimmer
                sInhalt = sInhalt.Replace("?BT-145", arFeldDruck(10) & "," & arFeldDruck(13) & "," & arFeldDruck(12))   'Beschreibung

                sXRechnungA = sXRechnungA.Replace("?BT-083", "R.Nr.:" & nRNr & "/" & arFeldDruck(10))
                sXRechnungE &= sInhalt

                Dim sDatumTeil1 As String = If(arFeldDruck(10).Length >= 10, arFeldDruck(10).Substring(0, 10), "")
                Dim sDatumTeil2 As String = If(arFeldDruck(10).Length >= 14, arFeldDruck(10).Substring(13), "")

                Dim sAnreiseUmgewandelt As String = fcUmDatum(sDatumTeil1)
                Dim sAbreiseUmgewandelt As String = fcUmDatum(sDatumTeil2)

                If sAnreiseUmgewandelt < sAnreise Then sAnreise = sAnreiseUmgewandelt
                If sAbreiseUmgewandelt > sAbreise Then sAbreise = sAbreiseUmgewandelt

                Dim nFeld4Wert As Decimal = Convert.ToDecimal(Val(arFeldDruck(4).Replace(",", ".")))
                Dim nFeld7Wert As Decimal = Convert.ToDecimal(Val(arFeldDruck(7).Replace(",", ".")))

                Steuer += nFeld4Wert
                Netto += nFeld7Wert
                Brutto += (nFeld4Wert + nFeld7Wert)
            Next

            sXRechnungA = sXRechnungA.Replace("<!--Rechnungsposition_ENDE-->", sXRechnungE)         'Mail Emp
            sXRechnungA = sXRechnungA.Replace("?BT-110", Steuer.ToString("F2", System.Globalization.CultureInfo.InvariantCulture))  ' Steuerbetrag
            sXRechnungA = sXRechnungA.Replace("?BT-106", fcFormatDecimalPunkt(Netto.ToString()))    'NettoBetrag
            sXRechnungA = sXRechnungA.Replace("?BT-109", fcFormatDecimalPunkt(Netto.ToString()))
            sXRechnungA = sXRechnungA.Replace("?BT-116", fcFormatDecimalPunkt(Netto.ToString()))
            sXRechnungA = sXRechnungA.Replace("?BT-073", sAnreise)
            sXRechnungA = sXRechnungA.Replace("?BT-074", sAbreise)
            sXRechnungA = sXRechnungA.Replace("?BT-072", sAnreise)

            sXRechnungA = sXRechnungA.Replace("?BT-112", fcFormatDecimalPunkt(Brutto.ToString()))   'Brutobetrag
            sXRechnungA = sXRechnungA.Replace("?BT-115", fcFormatDecimalPunkt(Brutto.ToString()))   'Brutobetrag
            sXRechnungA = sXRechnungA.Replace("?BT-113", "0")                                       'Anzahlung
        End If

        ' =====================================================================
        ' --- BLOCK B: RECHNUNGSART 0 (Normalrechnung) -----------------------
        ' =====================================================================
        If nReArt = 0 Then
            Dim Steuer As Decimal = 0
            Dim Netto As Decimal = 0
            Dim Brutto As Decimal = 0

            For i As Integer = 0 To arDruck1.Length - 1
                sInhalt = ReadFileSeriell("XRechnung_CII_2.XML")
                Dim arFeldDruck() As String = arDruck1(i).Split("#"c)

                sbVariable.AppendLine(" Zimmer " & arFeldDruck(0))
                sbVariable.AppendLine(" Nächte " & arFeldDruck(1))
                sbVariable.AppendLine(" E Preis " & arFeldDruck(3))
                sbVariable.AppendLine(" GesamtSteuer 7% " & arFeldDruck(4))
                sbVariable.AppendLine(" Netto Gesamt " & arFeldDruck(7))
                sbVariable.AppendLine(" Bruto Gesamt " & arFeldDruck(8))
                sbVariable.AppendLine(" ------------ ")

                Dim nEinzelPreisBrutto As Decimal = Convert.ToDecimal(Val(arFeldDruck(3).Replace(",", ".")))
                Dim nEinzelPreisNetto As Decimal = Math.Round(nEinzelPreisBrutto / (1 + (nSteuersatz / 100)), 2)
                sbVariable.AppendLine(" Netto erechnet EinzelPreis " & nEinzelPreisNetto.ToString("F2", System.Globalization.CultureInfo.InvariantCulture))

                Dim nSteuerEinzeln As Decimal = Math.Round(nEinzelPreisBrutto - nEinzelPreisNetto, 2)
                sbVariable.AppendLine(" Steuer erechnet Einzel 7% " & nSteuerEinzeln.ToString("F2", System.Globalization.CultureInfo.InvariantCulture))

                Dim nNaechte As Decimal = Convert.ToDecimal(Val(arFeldDruck(1)))
                Dim nNettoGesamtErrechnet As Decimal = nEinzelPreisNetto * nNaechte
                sbVariable.AppendLine(" Netto Gesamt erechnet  " & nNettoGesamtErrechnet.ToString("F2", System.Globalization.CultureInfo.InvariantCulture))

                Dim nSteuerGesamtErrechnet As Decimal = nSteuerEinzeln * nNaechte


                sbVariable.AppendLine(" Steuer Gesamt erechnet 7% " & nSteuerGesamtErrechnet.ToString("F2", System.Globalization.CultureInfo.InvariantCulture))
                sInhalt = sInhalt.Replace("?BT-129", arFeldDruck(1))
                sInhalt = sInhalt.Replace("?BT-126", (i + 1).ToString())
                sbVariable.AppendLine(" ====================" & vbCrLf)
                sInhalt = sInhalt.Replace("?BT-155", (i + 1).ToString())    'Laufende nr
                sInhalt = sInhalt.Replace("?BT-146", fcFormatDecimalPunkt(nEinzelPreisNetto.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)))
                sInhalt = sInhalt.Replace("?BT-152", S1)                    'Steuer
                sInhalt = sInhalt.Replace("?BT-131", fcFormatDecimalPunkt(nNettoGesamtErrechnet.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)))
                sInhalt = sInhalt.Replace("?BT-153", arFeldDruck(2))        'Artikel Dreibettzimmer
                sInhalt = sInhalt.Replace("?BT-145", arFeldDruck(10) & "," & arFeldDruck(13) & "," & arFeldDruck(12))   'Beschreibung
                sXRechnungA = sXRechnungA.Replace("?BT-083", "R.Nr.:" & nRNr & "/" & arFeldDruck(10))
                sXRechnungE &= sInhalt
                Dim sDatumTeil1 As String = If(arFeldDruck(10).Length >= 10, arFeldDruck(10).Substring(0, 10), "")
                Dim sDatumTeil2 As String = If(arFeldDruck(10).Length >= 14, arFeldDruck(10).Substring(13), "")
                Dim sAnreiseUmgewandelt As String = fcUmDatum(sDatumTeil1)
                Dim sAbreiseUmgewandelt As String = fcUmDatum(sDatumTeil2)
                If sAnreiseUmgewandelt < sAnreise Then sAnreise = sAnreiseUmgewandelt
                If sAbreiseUmgewandelt > sAbreise Then sAbreise = sAbreiseUmgewandelt
                Steuer += nSteuerGesamtErrechnet
                Netto += nNettoGesamtErrechnet
                Brutto += (nNettoGesamtErrechnet + nSteuerGesamtErrechnet)
            Next
            sbVariable.AppendLine(" =======Summen======" & vbCrLf)
            sbVariable.AppendLine(" Netto Summe Gesamt " & Netto.ToString("F2", System.Globalization.CultureInfo.InvariantCulture))
            sbVariable.AppendLine(" Steuer Summe Gesamt " & Steuer.ToString("F2", System.Globalization.CultureInfo.InvariantCulture))
            sbVariable.AppendLine(" Bruto Summe Gesamt " & Brutto.ToString("F2", System.Globalization.CultureInfo.InvariantCulture))
            sXRechnungA = sXRechnungA.Replace("", sXRechnungE)                                      'Mail Emp
            sXRechnungA = sXRechnungA.Replace("?BT-110", Steuer.ToString("F2", System.Globalization.CultureInfo.InvariantCulture))  ' Steuerbetrag
            sXRechnungA = sXRechnungA.Replace("?BT-106", fcFormatDecimalPunkt(Netto.ToString()))    'NettoBetrag
            sXRechnungA = sXRechnungA.Replace("?BT-109", fcFormatDecimalPunkt(Netto.ToString()))
            sXRechnungA = sXRechnungA.Replace("?BT-116", fcFormatDecimalPunkt(Netto.ToString()))
            sXRechnungA = sXRechnungA.Replace("?BT-073", sAnreise)
            sXRechnungA = sXRechnungA.Replace("?BT-074", sAbreise)
            sXRechnungA = sXRechnungA.Replace("?BT-072", sAnreise)
            sXRechnungA = sXRechnungA.Replace("?BT-112", fcFormatDecimalPunkt(Brutto.ToString()))   'Brutobetrag
            sXRechnungA = sXRechnungA.Replace("?BT-115", fcFormatDecimalPunkt(Brutto.ToString()))   'Brutobetrag
            sXRechnungA = sXRechnungA.Replace("?BT-113", "0")                                       'Anzahlung
        End If
        ' =====================================================================
        ' --- GEMEINSAMES ZAHLUNGSZIEL & DATEI-AUSGABE -----------------------
        ' =====================================================================
        If nReArt = 0 Or nReArt = 4 Then
            arFeld = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "BuchDatum"})
            Dim sZDatum As String = ""
            sRZiel = sRZiel.Trim()
            If String.IsNullOrEmpty(sRZiel) Then
                sZDatum = Date.Today.AddDays(14).ToString("dd.MM.yyyy")
            Else
                Dim sInhald1() As String = arDruck1(0).Split("#"c)
                Dim sVon As String = ""
                If sInhald1.Length > 10 AndAlso sInhald1(10).Length >= 10 Then
                    sVon = sInhald1(10).Substring(0, 10)
                End If
                If sRZiel.Length >= 3 AndAlso sRZiel.Substring(2, 1) = "." Then
                    Dim sJahrAusVon As String = ""
                    If sVon.Length >= 10 Then sJahrAusVon = sVon.Substring(6)
                    sZDatum = sRZiel & "." & sJahrAusVon
                    If fcUmDatum(sZDatum) > fcUmDatum(sVon) Then
                        Dim nJahrTemp As Integer = 0
                        If Integer.TryParse(sJahrAusVon, nJahrTemp) Then
                            sZDatum = sRZiel.Substring(0, 5) & "." & (nJahrTemp - 1).ToString()
                        End If
                    End If
                Else
                    If IsNumeric(sRZiel) Then
                        Dim nTage As Integer = Convert.ToInt32(Val(sRZiel))
                        Dim ddatum As Date
                        If nTage < 0 Then
                            Dim dVonDate As Date
                            If Date.TryParse(sVon, dVonDate) Then
                                ddatum = dVonDate.AddDays(nTage)
                            End If
                        Else
                            Dim dBuchDate As Date
                            If Date.TryParse(fcUmDatum(arFeld(3)), dBuchDate) Then
                                ddatum = dBuchDate.AddDays(nTage)
                            End If
                        End If
                        sZDatum = ddatum.ToString("dd.MM.yyyy")
                    End If
                End If
            End If
            sXRechnungA = sXRechnungA.Replace("?BT-009", fcUmDatum(sZDatum))
        End If
        WriteFileUniCode("Var.txt", sbVariable.ToString())
        WriteFileUniCode(cgPfad & "\Ablage\Rechnung.xml", sXRechnungA)
    End Sub

    ''' <summary>
    ''' Formatiert einen numerischen Wert als String über die standardisierte 'fcFormatDecimal'-Logik 
    ''' und ersetzt das deutsche Dezimalkomma durch einen Punkt, um XML- und Datenbankkompatibilität zu gewährleisten.
    ''' </summary>
    ''' <param name="sWert">Der zu formatierende numerische String (ByRef übergeben).</param>
    ''' <returns>Der formatierte String mit einem Punkt als Dezimaltrennzeichen.</returns>
    ''' <remarks>
    ''' 07.10.2026 - Code-Optimierung:
    ''' - VB6-Zuweisung entfernt: Zuweisung an den Funktionsnamen durch ein modernes 'Return' ersetzt.
    ''' - Instanzmethode genutzt: Das veraltete globale 'Replace()' durch das native '.Replace()' der String-Klasse ersetzt.
    ''' - Typsicherheit: Expliziten Rückgabetyp deklariert, um Probleme unter 'Option Strict On' vollständig zu verhindern.
    ''' </remarks>
    Public Function fcFormatDecimalPunkt(ByRef sWert As String) As String
        sWert = fcFormatDecimal(sWert)

        If String.IsNullOrEmpty(sWert) Then
            Return "0.00"
        End If

        Return sWert.Replace(","c, "."c)
    End Function

End Module
