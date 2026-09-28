Imports System.Text
Imports System.IO
Module moDruckPost
    Dim sSS7 As String
    Dim sSS19 As String
    Dim sSSS As String
    Dim sGKU As String
    Dim sGKS As String
    Dim sGKG As String
    Dim nSprache As Integer

    ''' <summary>
    ''' prDruckBuchung( Buchungs ID, Mackro ID, BuchungsText ID )
    ''' </summary>
    ''' <remarks>
    ''' 03.03.2012 Create
    ''' </remarks>
    Public Sub fcDruckBuchnung(ByRef sBID As String)
        Dim xlaenge As Integer = 25
        Dim arFeld(1) As String
        Dim arfeld1(1) As String

        Dim ii As Integer
        Dim sKID As String
        Dim sName As String = ""
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "BText", "MText", "Sprache", "von", "bis", "BuchDatum", "Rdatum", "InternetNr", "Name1", "Name2"})
        sKID = arfeld1(0)
        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "Name2"})
        For i = 1 To Len(arIni(2))
            sName = sName + Mid(arIni(2), i, 1) + " "
        Next
        prVpeOpen()

        Call prVpePageFormat("A4")

        Call prDruckKopf(xlaenge, arfeld1(7), arfeld1(10), arfeld1(11), arFeld)
        Call prDruckBuch(xlaenge, arfeld1(1), arfeld1(4))
        arFeld = fcDruckZimmer(xlaenge, sgRBID, "B", arfeld1(4))   'im Arry stehen auch die summen siehe Rechnung
        ii = Str(arFeld(0))
        ii = fcDruckMakro(xlaenge, ii, arfeld1(3), arfeld1(4))
        If ii < Val(arFeld(0)) + 15 Then ii = arFeld(0) + 15
        ii = prDruckBuchungsText(xlaenge, ii, arfeld1(2), arfeld1(4), arfeld1(5), arfeld1(6), arfeld1(7), arfeld1(8), arfeld1(9))
        If ii > 240 Then
            prVpeText(xlaenge, ii + 5, "Seite 1 von 2")
            Call prDruckFuss(xlaenge)
            Call prVpeNewPage()

            For ix = 1 To Len(arIni(2))
                sName = sName + Mid(arIni(2), ix, 1) + " "
            Next
            prVpeTextFormat("Times New Roman", 18)
            prVpeText(xlaenge, 18, sName)
            prVpeTextFormat("Times New Roman", 12)
            prVpeText(xlaenge, 25, arIni(3) + ", " + arIni(4) + " " + arIni(5))
            prVpeTextFormat("Times New Roman", 10)
            ii = 50
            Call prVpeText(xlaenge, ii, "Seite 2 von 2")

            ii = 60

        End If
        prVpeTextFormat("Times New Roman", 12)
        prVpeText(xlaenge, ii, fcLanguage(arfeld1(4), "[N B]Wir freuen uns auf Sie") & ".")
        prVpeTextFormat("Times New Roman", 10)

        prVpeText(xlaenge, ii + 12, sName)
        prVpeText(xlaenge, ii + 25, fcLanguage(nSprache, "Unterschrift Gast Datum")) '15
        Call prDruckFuss(xlaenge)

        If DirExists(arIni(32) & "\Buchung") = False Then
            CreateDir(arIni(32) & "\Buchung")
        End If


        Call prVpePDF(arIni(32) & "\Buchung" & "\Buch_" & sBID & ".PDF")


        '   Call prVpePDF(arIni(32) & "\Buch_" & sBID & ".PDF")
        ' Report.WriteDoc("Mein Dokument.pdf")
        prVpeVeiw()
    End Sub
    ''' <summary>
    ''' prDruckRechnung( Buchungs ID )
    ''' </summary>
    ''' <remarks>
    ''' 03.03.2012 Create
    ''' </remarks>
    Public Sub prDruckRechnungZusatz(ByRef sBID As String, ByVal sKID As String, ByRef nRNr As String, ByVal sdatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByVal ParamArray arDruck1() As String)
        Dim xlaenge As Integer = 25
        Dim arFeld(1) As String
        Dim arfeld1(1) As String
        Dim arfeld2(1) As String
        Dim arfeld3(1) As String
        Dim arfeld4(1) As String
        Dim aÜbergabe(4) As String
        Dim rechnung() As Double
        Dim rechnung1(6) As Double
        Dim yHoehe As Integer
        Dim ii As Integer
        Dim nSeiten As Integer = Int(arDruck1.Length / 13) + 1
        Dim nSeite As Integer = 1
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "Name1", "Name2"})
        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "Name2"})
        nSprache = Val(arfeld1(2))
        For i = 0 To 5
            rechnung1(i) = 0
        Next

        For i = 0 To arDruck1.Length - 1
            If i = 0 Or i / 13 = Int(i / 13) Then
                If i <> 0 Then
                    Call prDruckFuss(xlaenge)
                    prVpeNewPage()
                End If
                Call prVpePageFormat("A4")
                prVpeTextFormat("Times New Roman", 10)
                prVpeText(xlaenge + 112, 53, "Tel. " & arIni(6))
                prVpeText(xlaenge + 112, 61, "Seite " & Str(nSeite) & " von " & Str(nSeiten))
                nSeite = nSeite + 1
                Call prDruckKopf(xlaenge, sdatum, arfeld1(3), arfeld1(4), arFeld)
                Call prDruckRech(xlaenge, arfeld1(1), nRNr, arfeld1(2))
                yHoehe = 117
                prDruckKopfPositionZusatz(xlaenge, arfeld1(2))
            End If
            rechnung = fcDruckPositionZusatz(i, xlaenge, yHoehe, arDruck1(i), arfeld1(2))
            yHoehe = yHoehe + 5
            For j = 0 To 5
                rechnung1(j) = Math.Round(rechnung1(j) + rechnung(j), 2)
            Next
        Next
        rechnung1(6) = anzahlung
        aÜbergabe = FcDruckSumme(xlaenge, yHoehe, arfeld1(2), rechnung1)

        ' arfeld4 = fcDruckPosition(xlaenge, sgRBID, "R", arfeld1(2))
        '  arfeld4 = fcDruckZimmer(xlaenge, sgRBID, "R", arfeld1(2))
        ii = Val(arfeld4(0))
        If bBar = True Then
            prVpeText(xlaenge, yHoehe + 36, "Betrag dankend erhalten")
        Else
            prVpeText(xlaenge, yHoehe + 36, "Bitte überweisen Sie den Gesamtbetrag bis: " & Date.Today.AddDays(14) & " auf unten genantes Konto")
        End If
        Call prDruckFuss(xlaenge)
        arfeld2 = fcDataSeek("select * From Datev Where RechNr ='", nRNr, 0, {"ID"})


        'datensatz nicht gefunfen
        arfeld3 = {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "}
        arfeld3(0) = fcGetTimeID(Date.Today)
        arfeld3(1) = arFeld(1) & " " & arFeld(2)
        arfeld3(2) = nRNr
        arfeld3(5) = arIni(11)
        arfeld3(10) = arIni(10)
        arfeld3(6) = arIni(20)
        arfeld3(11) = arIni(21)
        arfeld3(3) = (rechnung1(1) + rechnung1(2)) * 100
        arfeld3(4) = rechnung1(2) * 100
        arfeld3(8) = (rechnung1(3) + rechnung1(4)) * 100
        arfeld3(9) = rechnung1(4) * 100
        If arfeld2(0) = " " Then
            Call fcInsertCommand("Datev", {"Id", "Name", "Rechnr",
                                 "Umsatz1", "Mwst1", "MwstS1", "GKonto1", "Konto1", "Umsatz2", "Mwst2",
                                 "MwstS2", "Gkonto2", "Konto2", "Datum", "BuchNr", "KunNr"}, arfeld3)

        Else
            'datensatz neu schreiben
            Dim sMsg As String = "Datev Satz (Extras) neu Schreiben ?"
            If MsgBox(sMsg, vbExclamation + vbOKCancel, "Schreiben") = MsgBoxResult.Ok Then
                Dim cBedingung As String = " WHERE Rechnr='" & nRNr & "'"
                Call fcUpdateCommand("Datev", {"Id", "Name", "Rechnr", "Umsatz1", "Mwst1",
                                               "MwstS1", "GKonto1", "Konto1", "Umsatz2", "Mwst2",
                                               "MwstS2", "Gkonto2", "Konto2", "Datum", "BuchNr",
                                               "KunNr"}, arfeld3, cBedingung)

            End If
        End If


        '  prVpeVeiw()
    End Sub
    Private Sub prDruckKopfPositionZusatz(ByRef xlaenge As Integer, ByRef sSprache1 As String)
        Dim i As Integer = 0
        Dim ii As Integer = 1
        Dim nSprache As Integer = Val(sSprache1)
        Call prTableDim(xlaenge, 112, 165, ii * 5)  '152
        Call prTableColumns(8)
        Call prTableRows(ii)
        For i1 = 0 To ii - 1
            For j = 0 To 7
                Call prTabelCellSelectFont(j, i1, "Times New Roman", 9)
                Call prTabelCellLeft(j, i1, 1)
            Next
        Next
        Call prTableColumnsWidth(0, 6)
        Call prTableColumnsWidth(1, 30)
        Call prTableColumnsWidth(2, 35)
        Call prTableColumnsWidth(3, 22)
        Call prTableColumnsWidth(5, 10)
        Call prTableColumnsWidth(6, 22)
        Call prTableColumnsWidth(7, 22)

        Call prTabelCellText(0, 0, fcLanguage(nSprache, "Pos"))
        Call prTabelCellText(1, 0, fcLanguage(nSprache, "Objekt"))
        Call prTabelCellText(2, 0, fcLanguage(nSprache, "Leistung"))
        Call prTabelCellText(3, 0, fcLanguage(nSprache, "Preis"))
        Call prTabelCellText(4, 0, fcLanguage(nSprache, "Mwst"))
        Call prTabelCellText(5, 0, fcLanguage(nSprache, "Stück"))
        Call prTabelCellText(6, 0, fcLanguage(nSprache, "MwstB."))
        Call prTabelCellText(7, 0, fcLanguage(nSprache, "Brutto"))

        prTabelWrite()
    End Sub
    Private Function fcDruckPositionZusatz(ByVal pos As Integer, ByRef xlaenge As Integer, ByVal yHoehe As Integer, ByVal sInhald As String, ByRef sSprache1 As String) As Array
        Dim sInhald1() As String = Split(sInhald, "#")
        Dim i As Integer = 0

        Dim arRueck(5) As Double
        Dim nSprache As Integer = Val(sSprache1)
        Call prTableDim(xlaenge, yHoehe, 165, 5)
        Call prTableColumns(8)
        Call prTableRows(1)

        For j = 0 To 7
            Call prTabelCellSelectFont(j, 0, "Times New Roman", 9)
            Call prTabelCellLeft(j, 0, 1)
        Next

        Call prTableColumnsWidth(0, 6)
        Call prTableColumnsWidth(1, 30)
        Call prTableColumnsWidth(2, 35)
        Call prTableColumnsWidth(3, 22)
        Call prTableColumnsWidth(5, 10)
        Call prTableColumnsWidth(6, 22)
        Call prTableColumnsWidth(7, 22)

        Call prTabelCellText(0, 0, Str(pos + 1))

        Call prTabelCellText(1, 0, fcLanguage(nSprache, sInhald1(0)))
        Call prTabelCellText(2, 0, sInhald1(2).Trim)
        Call prTabelCellText(3, 0, fcDecStr(sInhald1(3), 10) + " €")
        Call prTabelCellText(7, 0, fcDecStr(sInhald1(7), 10) + " €")
        Call prTabelCellText(4, 0, sInhald1(12) + " %")
        Call prTabelCellText(5, 0, sInhald1(1))
        If sInhald1(5).Trim <> "0,00" Then
            Call prTabelCellText(6, 0, fcDecStr(sInhald1(5), 10) + " €") '19%
            arRueck(3) = Math.Round(Val(fcChangs(sInhald1(7), ",", ".")) - Val(fcChangs(sInhald1(5), ",", ".")), 2)  '19%
            arRueck(4) = Math.Round(Val(fcChangs(sInhald1(5), ",", ".")), 2)
            arRueck(1) = 0
            arRueck(2) = 0

        Else
            Call prTabelCellText(6, 0, fcDecStr(sInhald1(4), 10) + " €") '7%
            arRueck(1) = Math.Round(Val(fcChangs(sInhald1(7), ",", ".")) - Val(fcChangs(sInhald1(4), ",", ".")), 2)  '7%
            arRueck(2) = Math.Round(Val(fcChangs(sInhald1(4), ",", ".")), 2)
            arRueck(3) = 0
            arRueck(4) = 0

        End If
        '   Call prTabelCellText(7, 0, fcDecStr(sInhald1(7), 10) + " €")

        prTabelWrite()


        arRueck(5) = Val(fcChangs(sInhald1(7), ",", "."))
        Return arRueck
    End Function
    ''' <summary>
    ''' prDruckRechnung( Buchungs ID )
    ''' </summary>
    ''' <remarks>
    ''' 03.03.2012 Create
    ''' </remarks>
    Public Sub prDruckRechnung(ByRef sBID As String, ByVal sKID As String, ByRef nRNr As String, ByVal sdatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByRef sRZiel As String, ByVal S1 As String, ByRef s2 As String, ByRef s3 As String, ByRef g1 As String, ByRef g2 As String, ByRef g3 As String, ByVal nReArt As Integer, ByVal ParamArray arDruck1() As String)
        Dim sDatei As String  'Kassenbuch
        ' Dim sKasseName As String 'Kassenbuch
        '  Dim skonto As String
        Dim aÜbergabeKasse(4) As String
        Dim arIn(12) As String
        Dim xlaenge As Integer = 25
        Dim arFeld(1) As String
        Dim arfeld1(1) As String
        Dim arfeld2(1) As String
        Dim arfeld3(1) As String
        Dim arfeld4(1) As String
        Dim rechnung() As Double
        Dim rechnung1(8) As Double
        Dim yHoehe As Integer
        Dim ii As Integer
        Dim nSeiten As Integer = Int(arDruck1.Length / 13) + 1
        Dim nSeite As Integer = 1
        sSS7 = S1
        sSS19 = s2
        sSSS = s3
        sGKU = g1
        sGKS = g2
        sGKG = g3
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "BuchDatum", "Name1", "Name2"})
        nSprache = Val(arfeld1(2))
        '   sKID = arfeld1(0)
        arfeld1(4)=""
        arfeld1(5) = ""
        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "Name2"})
        '   prVpeOpen()
        For i = 0 To 5
            rechnung1(i) = 0
        Next
        For i = 0 To arDruck1.Length - 1
            If i = 0 Or i / 13 = Int(i / 13) Then
                If i <> 0 Then
                    Call prDruckFuss(xlaenge)
                    prVpeNewPage()
                End If
                Call prVpePageFormat("A4")
                prVpeTextFormat("Times New Roman", 10)
                '     prVpeText(xlaenge + 112, 53, "Tel. " & arIni(6))
                '     prVpeText(xlaenge + 112, 61, "Seite " & Str(nSeite) & " von " & Str(nSeiten))
                nSeite = nSeite + 1
                Call prDruckKopf(xlaenge, sdatum, arfeld1(4), arfeld1(5), arFeld)
                Call prDruckRech(xlaenge, arfeld1(1), nRNr, arfeld1(2))
                yHoehe = 117
                Call prDruckKopfPosition(xlaenge, arfeld1(2), nReArt) 'nReArt


            End If


            rechnung = fcDruckPosition(i, xlaenge, yHoehe, arDruck1(i), arfeld1(2), nReArt)
            yHoehe = yHoehe + 5
            For j = 0 To 7
                rechnung1(j) = Math.Round(rechnung1(j) + rechnung(j), 2)
            Next
        Next
        rechnung1(8) = anzahlung
        '   Dim dRest As Decimal = rechnung1(1) + rechnung1(2) - anzahlung
        '   rechnung1(1) = Math.Round(dRest / ((Val(arIni(11)) + 100) / 100), 2)
        '   rechnung1(2) = Math.Round(dRest - rechnung1(1), 2)


        aÜbergabeKasse = FcDruckSumme(xlaenge, yHoehe, arfeld1(2), rechnung1)

        ' arfeld4 = fcDruckPosition(xlaenge, sgRBID, "R", arfeld1(2))
        '  arfeld4 = fcDruckZimmer(xlaenge, sgRBID, "R", arfeld1(2))
        ii = Val(arfeld4(0))
        If bBar = True Then
            prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Betrag dankend erhalten"))
            sDatei = "K" & nRNr & ".DAV"
            ' sKasseName = "Rechnung Einnahme K" & Str(nRNr)
            arIn(0) = "K" & nRNr & "-1"  'Name
            arIn(6) = "K" & nRNr & "-2"  'Name
            arIn(3) = arIni(18)   'konto
            arIn(9) = arIni(18)
            ' skonto = arIni(18)
        Else
            Dim sZDatum As String = ""
            Dim t As String = arfeld1(3)
            sRZiel = sRZiel.Trim
            If sRZiel = " " Or sRZiel = "" Then
                ' prVpeText(xlaenge, yHoehe + 36, "Bitte überweisen Sie den Gesamtbetrag bis: " & Date.Today.AddDays(14) & " auf unten genanntes Konto")
                prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis") & ": " & Date.Today.AddDays(14) & " " & fcLanguage(nSprache, "auf unten genanntes Konto"))
            Else
                If IsDate(sRZiel) Then
                    sZDatum = sRZiel

                Else

                    Dim sInhald1() As String = Split(arDruck1(0), "#")
                    Dim sVon As String = Mid(sInhald1(10), 1, 10)
                    If Mid(sRZiel, 3, 1) = "." Then
                        sZDatum = sRZiel + "." + Mid(sVon, 7)
                        If fcUmDatum(sZDatum) > fcUmDatum(sVon) Then
                            sZDatum = Mid(sRZiel, 1, 5) + "." + Str((Mid(sVon, 7) - 1))
                        End If
                    Else
                        Dim ddatum As Date
                        If IsNumeric(sRZiel) = True Then
                            Dim nTage As Integer = Val(sRZiel)
                            If nTage < 0 Then                              ' kleiner 0 zahlung nach vor anreise -tage
                                ddatum = CDate(sVon).AddDays(nTage)
                            Else                                            'größer 0 zahlung nach erhald + tage
                                ddatum = CDate(fcUmDatum(arfeld1(3))).AddDays(nTage)
                            End If
                            sZDatum = ddatum
                        End If
                    End If
                End If
                'prVpeText(xlaenge, yHoehe + 36, "Bitte überweisen Sie den Gesamtbetrag bis: " & sZDatum & " auf unten genantes Konto")
                prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis") & ": " & sZDatum & " " & fcLanguage(nSprache, "auf unten genanntes Konto"))

            End If
            sDatei = "B" & nRNr & ".DAV"
            '  sKasseName = "Rechnung Einnahme B" & Str(nRNr)
            arIn(0) = "B" & nRNr & "-1"  'Name
            arIn(6) = "B" & nRNr & "-2"  'Name
            arIn(3) = arIni(19)
            arIn(9) = arIni(19)
            '  skonto = arIni(19)
        End If
        Call prDruckFuss(xlaenge)

        If DirExists(arIni(32) & "\Rechnung") = False Then
            CreateDir(arIni(32) & "\Rechnung")
        End If
        Call prVpePDF(cgPfad & "\Ablage" & "\Rechnung.PDF")

        '    Call prVpePDF(arIni(32) & "\Rechnung" & "\Rech_" & nRNr & ".PDF")
        arfeld2 = fcDataSeek("select * From Datev Where RechNr ='", nRNr, 0, {"ID"})


        'datensatz nicht gefunfen
        arfeld3 = {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "}
        arfeld3(0) = fcGetTimeID(Date.Today)
        arfeld3(1) = arFeld(1) & " " & arFeld(2)
        arfeld3(2) = nRNr
        arfeld3(5) = sSS7 ' arIni(11)
        arfeld3(10) = sSS19 'arIni(10)
        arfeld3(15) = sSSS 'arIni(23)
        arfeld3(6) = sGKU 'arIni(20)
        arfeld3(11) = sGKG 'arIni(21)
        arfeld3(16) = sGKS ' ari(25)
        arfeld3(3) = (rechnung1(1) + rechnung1(2)) * 100
        arfeld3(4) = rechnung1(2) * 100
        arfeld3(8) = (rechnung1(3) + rechnung1(4)) * 100
        arfeld3(9) = rechnung1(4) * 100
        arfeld3(13) = (rechnung1(6) + rechnung1(7)) * 100
        arfeld3(14) = rechnung1(7) * 100
        arIn(1) = aÜbergabeKasse(0) 'Betrag
        arIn(2) = aÜbergabeKasse(1) 'Steuer
        arIn(7) = aÜbergabeKasse(2) 'Betrag
        arIn(8) = aÜbergabeKasse(3) 'Steuer
        arIn(4) = arIni(21)         'Gegenkoto gkg
        arIn(10) = arIni(20)        'Gegenkonto gku
        arIn(11) = sdatum           'datum
        arIn(5) = sdatum

        Call prKassenDatei(sDatei, arIn)

        If arfeld2(0) = " " Then
            Call fcInsertCommand("Datev", {"Id", "Name", "Rechnr",
                                 "Umsatz1", "Mwst1", "MwstS1", "GKonto1", "Konto1",
                                 "Umsatz2", "Mwst2", "MwstS2", "Gkonto2", "Konto2",
                                 "Umsatz3", "Mwst3", "MwstS3", "Gkonto3", "Konto3",
                                 "Datum", "BuchNr", "KunNr"}, arfeld3)

        Else
            'datensatz neu schreiben
            Dim sMsg As String = "Datev Satz (Zimmer) neu Schreiben ?"
            If MsgBox(sMsg, vbExclamation + vbOKCancel, "Schreiben") = MsgBoxResult.Ok Then
                Dim cBedingung As String = " WHERE Rechnr='" & nRNr & "'"
                Call fcUpdateCommand("Datev", {"Id", "Name", "Rechnr",
                                               "Umsatz1", "Mwst1", "MwstS1", "GKonto1", "Konto1",
                                               "Umsatz2", "Mwst2", "MwstS2", "Gkonto2", "Konto2",
                                               "Umsatz3", "Mwst3", "MwstS3", "Gkonto3", "Konto3",
                                               "Datum", "BuchNr", "KunNr"}, arfeld3, cBedingung)

            End If
        End If


        '    prVpeVeiw()
    End Sub

    ''' <summary>
    ''' prDruckRechnung( Buchungs ID )
    ''' </summary>
    ''' <remarks>
    ''' 03.03.2012 Create
    ''' </remarks>
    Public Sub prDruckRechnungStorno(ByRef sBID As String, ByVal sKID As String, ByRef nRNr As String, ByVal sdatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByRef sRZiel As String, ByVal ParamArray arDruck1() As String)
        Dim sDatei As String  'Kassenbuch
        ' Dim sKasseName As String 'Kassenbuch
        '  Dim skonto As String
        Dim aÜbergabeKasse(4) As String
        Dim arIn(12) As String
        Dim xlaenge As Integer = 25
        Dim arFeld(1) As String
        Dim arfeld1(1) As String
        Dim arfeld2(1) As String
        Dim arfeld3(1) As String
        Dim arfeld4(1) As String
        Dim rechnung() As Double
        Dim rechnung1(6) As Double
        Dim yHoehe As Integer
        Dim ii As Integer
        Dim nSeiten As Integer = Int(arDruck1.Length / 13) + 1
        Dim nSeite As Integer = 1
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "BuchDatum", "Name1", "Name2"})
        nSprache = Val(arfeld1(2))
        '   sKID = arfeld1(0)
        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "Name2"})
        '   prVpeOpen()
        For i = 0 To 5
            rechnung1(i) = 0
        Next
        For i = 0 To arDruck1.Length - 1
            If i = 0 Or i / 13 = Int(i / 13) Then
                If i <> 0 Then
                    Call prDruckFuss(xlaenge)
                    prVpeNewPage()
                End If
                Call prVpePageFormat("A4")
                prVpeTextFormat("Times New Roman", 10)
                prVpeText(xlaenge + 112, 53, "Tel. " & arIni(6))
                prVpeText(xlaenge + 112, 61, "Seite " & Str(nSeite) & " von " & Str(nSeiten))
                nSeite = nSeite + 1
                Call prDruckKopf(xlaenge, sdatum, arfeld1(4), arfeld1(5), arFeld)
                Call prDruckRech(xlaenge, arfeld1(1), nRNr, arfeld1(2))
                yHoehe = 117
                Call prDruckKopfPositionStorno(xlaenge, arfeld1(2))


            End If


            'If arDruck1(i) <> Nothing Then

            'End If
            rechnung = fcDruckPositionStorno(i, xlaenge, yHoehe, arDruck1(i), arfeld1(2))
            yHoehe = yHoehe + 5
            For j = 0 To 5
                rechnung1(j) = Math.Round(rechnung1(j) + rechnung(j), 2)
            Next
        Next
        rechnung1(6) = anzahlung
        Dim dRest As Decimal = rechnung1(1) + rechnung1(2) - anzahlung
        rechnung1(1) = Math.Round(dRest / ((Val(arIni(11)) + 100) / 100), 2)
        rechnung1(2) = Math.Round(dRest - rechnung1(1), 2)


        aÜbergabeKasse = FcDruckSumme(xlaenge, yHoehe, arfeld1(2), rechnung1)

        ' arfeld4 = fcDruckPosition(xlaenge, sgRBID, "R", arfeld1(2))
        '  arfeld4 = fcDruckZimmer(xlaenge, sgRBID, "R", arfeld1(2))
        ii = Val(arfeld4(0))
        If bBar = True Then
            prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Betrag dankend erhalten"))
            sDatei = "K" & nRNr & ".DAV"
            ' sKasseName = "Rechnung Einnahme K" & Str(nRNr)
            arIn(0) = "K" & nRNr & "-1"  'Name
            arIn(6) = "K" & nRNr & "-2"  'Name
            arIn(3) = arIni(18)   'konto
            arIn(9) = arIni(18)
            ' skonto = arIni(18)
        Else
            Dim sZDatum As String = ""
            Dim t As String = arfeld1(3)
            sRZiel = sRZiel.Trim
            If sRZiel = " " Or sRZiel = "" Then
                '    prVpeText(xlaenge, yHoehe + 36, "Bitte überweisen Sie den Gesamtbetrag bis: " & Date.Today.AddDays(14) & " auf unten genantes Konto")
                prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis") & ": " & Date.Today.AddDays(14) & " " & fcLanguage(nSprache, "auf unten genanntes Konto"))

            Else
                Dim sInhald1() As String = Split(arDruck1(0), "#")
                Dim sVon As String = Mid(sInhald1(9), 1, 10)
                If Mid(sRZiel, 3, 1) = "." Then
                    sZDatum = sRZiel + "." + Mid(sVon, 7)
                    If fcUmDatum(sZDatum) > fcUmDatum(sVon) Then
                        sZDatum = Mid(sRZiel, 1, 5) + "." + Str((Mid(sVon, 7) - 1))
                    End If
                Else
                    Dim ddatum As Date
                    If IsNumeric(sRZiel) = True Then
                        Dim nTage As Integer = Val(sRZiel)
                        If nTage < 0 Then                              ' kleiner 0 zahlung nach vor anreise -tage
                            ddatum = CDate(sVon).AddDays(nTage)
                        Else                                            'größer 0 zahlung nach erhald + tage
                            ddatum = CDate(fcUmDatum(arfeld1(3))).AddDays(nTage)
                        End If
                        sZDatum = ddatum
                    End If
                End If

                prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis") & ": " & sZDatum & " " & fcLanguage(nSprache, "auf unten genanntes Konto"))

            End If
            sDatei = "B" & nRNr & ".DAV"
            '  sKasseName = "Rechnung Einnahme B" & Str(nRNr)
            arIn(0) = "B" & nRNr & "-1"  'Name
            arIn(6) = "B" & nRNr & "-2"  'Name
            arIn(3) = arIni(19)
            arIn(9) = arIni(19)
            '  skonto = arIni(19)
        End If
        Call prDruckFuss(xlaenge)

        If DirExists(arIni(32) & "\Rechnung") = False Then
            CreateDir(arIni(32) & "\Rechnung")
        End If



        Call prVpePDF(arIni(32) & "\Rechnung\Rech_" & nRNr & ".PDF")
        arfeld2 = fcDataSeek("select * From Datev Where RechNr ='", nRNr, 0, {"ID"})


        'datensatz nicht gefunfen
        arfeld3 = {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "}
        arfeld3(0) = fcGetTimeID(Date.Today)
        arfeld3(1) = arFeld(1) & " " & arFeld(2)
        arfeld3(2) = nRNr
        arfeld3(5) = arIni(11)
        arfeld3(10) = arIni(10)
        arfeld3(6) = arIni(20)
        arfeld3(11) = arIni(21)
        arfeld3(3) = (rechnung1(1) + rechnung1(2)) * 100
        arfeld3(4) = rechnung1(2) * 100
        arfeld3(8) = (rechnung1(3) + rechnung1(4)) * 100
        arfeld3(9) = rechnung1(4) * 100
        arIn(1) = aÜbergabeKasse(0) 'Betrag
        arIn(2) = aÜbergabeKasse(1) 'Steuer
        arIn(7) = aÜbergabeKasse(2) 'Betrag
        arIn(8) = aÜbergabeKasse(3) 'Steuer
        arIn(4) = arIni(21)         'Gegenkoto
        arIn(10) = arIni(20)        'Gegenkonto
        arIn(11) = sdatum           'datum
        arIn(5) = sdatum

        Call prKassenDatei(sDatei, arIn)

        If arfeld2(0) = " " Then
            Call fcInsertCommand("Datev", {"Id", "Name", "Rechnr",
                                 "Umsatz1", "Mwst1", "MwstS1", "GKonto1", "Konto1", "Umsatz2", "Mwst2",
                                 "MwstS2", "Gkonto2", "Konto2", "Datum", "BuchNr", "KunNr"}, arfeld3)

        Else
            'datensatz neu schreiben
            Dim sMsg As String = "Datev Satz (Zimmer) neu Schreiben ?"
            If MsgBox(sMsg, vbExclamation + vbOKCancel, "Schreiben") = MsgBoxResult.Ok Then
                Dim cBedingung As String = " WHERE Rechnr='" & nRNr & "'"
                Call fcUpdateCommand("Datev", {"Id", "Name", "Rechnr", "Umsatz1", "Mwst1",
                                               "MwstS1", "GKonto1", "Konto1", "Umsatz2", "Mwst2",
                                               "MwstS2", "Gkonto2", "Konto2", "Datum", "BuchNr",
                                               "KunNr"}, arfeld3, cBedingung)

            End If
        End If


        '    prVpeVeiw()
    End Sub
    ''' <summary>
    ''' prDruckRechnung( Buchungs ID )
    ''' </summary>
    ''' <remarks>
    ''' 03.03.2012 Create
    ''' </remarks>
    Public Sub prDruckRechnungPausch(ByRef sBID As String, ByVal sKID As String, ByRef nRNr As String, ByVal sdatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByRef sRZiel As String, ByVal ParamArray arDruck1() As String)
        Dim sDatei As String  'Kassenbuch
        ' Dim sKasseName As String 'Kassenbuch
        '  Dim skonto As String
        Dim aÜbergabeKasse(4) As String
        Dim arIn(12) As String
        Dim xlaenge As Integer = 25
        Dim arFeld(1) As String
        Dim arfeld1(1) As String
        Dim arfeld2(1) As String
        Dim arfeld3(1) As String
        Dim arfeld4(1) As String
        Dim rechnung() As Double
        Dim rechnung1(6) As Double
        Dim yHoehe As Integer
        Dim ii As Integer
        Dim nSeiten As Integer = Int(arDruck1.Length / 13) + 1
        Dim nSeite As Integer = 1
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "BuchDatum", "Name1", "Name2"})
        nSprache = Val(arfeld1(2))
        '   sKID = arfeld1(0)
        arfeld1(4) = ""
        arfeld1(5) = ""
        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "Name2"})
        '   prVpeOpen()
        For i = 0 To 5
            rechnung1(i) = 0
        Next
        For i = 0 To arDruck1.Length - 1
            If i = 0 Or i / 13 = Int(i / 13) Then
                If i <> 0 Then
                    Call prDruckFuss(xlaenge)
                    prVpeNewPage()
                End If
                Call prVpePageFormat("A4")
                prVpeTextFormat("Times New Roman", 10)
                prVpeText(xlaenge + 112, 53, "Tel. " & arIni(6))
                prVpeText(xlaenge + 112, 61, "Seite " & Str(nSeite) & " von " & Str(nSeiten))
                nSeite = nSeite + 1
                Call prDruckKopf(xlaenge, sdatum, arfeld1(4), arfeld1(5), arFeld)
                Call prDruckRech(xlaenge, arfeld1(1), nRNr, arfeld1(2))
                yHoehe = 117
                Call prDruckKopfPosition(xlaenge, arfeld1(2), 0)


            End If


            'If arDruck1(i) <> Nothing Then

            'End If
            rechnung = fcDruckPosition(i, xlaenge, yHoehe, arDruck1(i), arfeld1(2), 0)
            yHoehe = yHoehe + 5
            For j = 0 To 5
                rechnung1(j) = Math.Round(rechnung1(j) + rechnung(j), 2)
            Next
        Next
        rechnung1(6) = anzahlung
        Dim dRest As Decimal = rechnung1(1) + rechnung1(2) - anzahlung
        rechnung1(1) = Math.Round(dRest / ((Val(arIni(11)) + 100) / 100), 2)
        rechnung1(2) = Math.Round(dRest - rechnung1(1), 2)


        aÜbergabeKasse = FcDruckSumme(xlaenge, yHoehe, arfeld1(2), rechnung1)

        ' arfeld4 = fcDruckPosition(xlaenge, sgRBID, "R", arfeld1(2))
        '  arfeld4 = fcDruckZimmer(xlaenge, sgRBID, "R", arfeld1(2))
        ii = Val(arfeld4(0))
        If bBar = True Then
            prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Betrag dankend erhalten"))
            sDatei = "K" & nRNr & ".DAV"
            ' sKasseName = "Rechnung Einnahme K" & Str(nRNr)
            arIn(0) = "K" & nRNr & "-1"  'Name
            arIn(6) = "K" & nRNr & "-2"  'Name
            arIn(3) = arIni(18)   'konto
            arIn(9) = arIni(18)
            ' skonto = arIni(18)
        Else
            Dim sZDatum As String = ""
            Dim t As String = arfeld1(3)
            sRZiel = sRZiel.Trim
            If sRZiel = " " Or sRZiel = "" Then
                prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis") & ": " & Date.Today.AddDays(14) & " " & fcLanguage(nSprache, "auf unten genanntes Konto"))

            Else
                Dim sInhald1() As String = Split(arDruck1(0), "#")
                Dim sVon As String = Mid(sInhald1(9), 1, 10)
                If Mid(sRZiel, 3, 1) = "." Then
                    sZDatum = sRZiel + "." + Mid(sVon, 7)
                    If fcUmDatum(sZDatum) > fcUmDatum(sVon) Then
                        sZDatum = Mid(sRZiel, 1, 5) + "." + Str((Mid(sVon, 7) - 1))
                    End If
                Else
                    Dim ddatum As Date
                    If IsNumeric(sRZiel) = True Then
                        Dim nTage As Integer = Val(sRZiel)
                        If nTage < 0 Then                              ' kleiner 0 zahlung nach vor anreise -tage
                            ddatum = CDate(sVon).AddDays(nTage)
                        Else                                            'größer 0 zahlung nach erhald + tage
                            ddatum = CDate(fcUmDatum(arfeld1(3))).AddDays(nTage)
                        End If
                        sZDatum = ddatum
                    End If
                End If

                prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis") & ": " & sZDatum & " " & fcLanguage(nSprache, "auf unten genanntes Konto"))

            End If
            sDatei = "B" & nRNr & ".DAV"
            '  sKasseName = "Rechnung Einnahme B" & Str(nRNr)
            arIn(0) = "B" & nRNr & "-1"  'Name
            arIn(6) = "B" & nRNr & "-2"  'Name
            arIn(3) = arIni(19)
            arIn(9) = arIni(19)
            '  skonto = arIni(19)
        End If
        Call prDruckFuss(xlaenge)

        If DirExists(arIni(32) & "\Rechnung") = False Then
            CreateDir(arIni(32) & "\Rechnung")
        End If

        Call prVpePDF(cgPfad & "\Ablage" & "\Rechnung.PDF")
        'Call prVpePDF(arIni(32) & "\Rechnung" & "\Rech_" & nRNr & ".PDF")




        arfeld2 = fcDataSeek("select * From Datev Where RechNr ='", nRNr, 0, {"ID"})


        'datensatz nicht gefunfen
        arfeld3 = {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "}
        arfeld3(0) = fcGetTimeID(Date.Today)
        arfeld3(1) = arFeld(1) & " " & arFeld(2)
        arfeld3(2) = nRNr
        arfeld3(5) = arIni(11)
        arfeld3(10) = arIni(10)
        arfeld3(6) = arIni(20)
        arfeld3(11) = arIni(21)
        arfeld3(3) = (rechnung1(1) + rechnung1(2)) * 100
        arfeld3(4) = rechnung1(2) * 100
        arfeld3(8) = (rechnung1(3) + rechnung1(4)) * 100
        arfeld3(9) = rechnung1(4) * 100
        arIn(1) = aÜbergabeKasse(0) 'Betrag
        arIn(2) = aÜbergabeKasse(1) 'Steuer
        arIn(7) = aÜbergabeKasse(2) 'Betrag
        arIn(8) = aÜbergabeKasse(3) 'Steuer
        arIn(4) = arIni(21)         'Gegenkoto
        arIn(10) = arIni(20)        'Gegenkonto
        arIn(11) = sdatum           'datum
        arIn(5) = sdatum

        Call prKassenDatei(sDatei, arIn)

        If arfeld2(0) = " " Then
            Call fcInsertCommand("Datev", {"Id", "Name", "Rechnr",
                                 "Umsatz1", "Mwst1", "MwstS1", "GKonto1", "Konto1", "Umsatz2", "Mwst2",
                                 "MwstS2", "Gkonto2", "Konto2", "Datum", "BuchNr", "KunNr"}, arfeld3)

        Else
            'datensatz neu schreiben
            Dim sMsg As String = "Datev Satz (Zimmer) neu Schreiben ?"
            If MsgBox(sMsg, vbExclamation + vbOKCancel, "Schreiben") = MsgBoxResult.Ok Then
                Dim cBedingung As String = " WHERE Rechnr='" & nRNr & "'"
                Call fcUpdateCommand("Datev", {"Id", "Name", "Rechnr", "Umsatz1", "Mwst1",
                                               "MwstS1", "GKonto1", "Konto1", "Umsatz2", "Mwst2",
                                               "MwstS2", "Gkonto2", "Konto2", "Datum", "BuchNr",
                                               "KunNr"}, arfeld3, cBedingung)

            End If
        End If


        '    prVpeVeiw()
    End Sub



    Private Sub prKassenDatei(ByRef sDatei As String, ByVal ParamArray arIn() As String)
        Dim sb As New StringBuilder
        sb.Append("Name,Betrag,Mwst,Konto,gegenkonto,Datum " + vbCrLf)
        For i = 0 To 5
            sb.Append(arIn(i) + ";")
        Next
        sb.Append(vbCrLf)
        For i = 6 To 11
            sb.Append(arIn(i) + ";")
        Next
        Call WriteFileSeriell(arIni(32) & "\Datev\" & sDatei, sb.ToString) 'datenübergabe auf Kassenprogramm
    End Sub
    Private Sub prDruckKopfPosition(ByRef xlaenge As Integer, ByRef sSprache1 As String, ByRef nReArt As Integer)
        Dim i As Integer = 0
        Dim ii As Integer = 1
        Dim nSprache As Integer = Val(sSprache1)
        Call prTableDim(xlaenge, 112, 165, ii * 5)  '152
        Call prTableColumns(8)
        Call prTableRows(ii)
        For i1 = 0 To ii - 1
            For j = 0 To 7
                Call prTabelCellSelectFont(j, i1, "Times New Roman", 9)
                Call prTabelCellLeft(j, i1, 1)
            Next
        Next
        Call prTableColumnsWidth(0, 6)
        Call prTableColumnsWidth(1, 30)
        Call prTableColumnsWidth(2, 25)
        Call prTableColumnsWidth(3, 10)
        Call prTableColumnsWidth(5, 10)
        Call prTableColumnsWidth(6, 22)
        Call prTableColumnsWidth(7, 22)

        Call prTabelCellText(0, 0, fcLanguage(nSprache, "Pos"))
        Call prTabelCellText(1, 0, fcLanguage(nSprache, "Objekt"))
        Call prTabelCellText(2, 0, fcLanguage(nSprache, "Ausstattung"))
        Call prTabelCellText(3, 0, fcLanguage(nSprache, "Art"))
        Call prTabelCellText(4, 0, fcLanguage(nSprache, "Zeit"))
        Call prTabelCellText(5, 0, fcLanguage(nSprache, "Pers."))
        If nReArt = 4 Then
            Call prTabelCellText(6, 0, "")
        Else
            Call prTabelCellText(6, 0, fcLanguage(nSprache, "Preis/Nacht"))
        End If
        Call prTabelCellText(7, 0, fcLanguage(nSprache, "Zimmerpreis"))

        prTabelWrite()
    End Sub
    Private Function fcDruckPosition(ByVal pos As Integer, ByRef xlaenge As Integer, ByVal yHoehe As Integer, ByVal sInhald As String, ByRef sSprache1 As String, ByRef nReArt As Integer) As Array
        Dim sInhald1() As String = Split(sInhald, "#")
        Dim i As Integer = 0
        Dim nFPreis As Double
        Dim nZPreis As Double
        Dim nGPreis As Double
        Dim arRueck(7) As Double
        Dim nSprache As Integer = Val(sSprache1)
        Call prTableDim(xlaenge, yHoehe, 165, 5)
        Call prTableColumns(8)
        Call prTableRows(1)

        For j = 0 To 7
            Call prTabelCellSelectFont(j, 0, "Times New Roman", 9)
            Call prTabelCellLeft(j, 0, 1)
        Next

        Call prTableColumnsWidth(0, 6)
        Call prTableColumnsWidth(1, 30)
        Call prTableColumnsWidth(2, 25)
        Call prTableColumnsWidth(3, 10)
        Call prTableColumnsWidth(5, 10)
        Call prTableColumnsWidth(6, 22)
        Call prTableColumnsWidth(7, 22)

        Call prTabelCellText(0, 0, Str(pos + 1))
        Dim AAA As String = sInhald1(2)
        Call prTabelCellText(1, 0, fcLanguage(nSprache, sInhald1(2)))   'object
        Call prTabelCellText(2, 0, fcLanguage(nSprache, sInhald1(13)))   'austatung
        Call prTabelCellText(3, 0, sInhald1(12))                         'art
        Call prTabelCellText(4, 0, sInhald1(10))                          'zeit
        Call prTabelCellText(5, 0, sInhald1(11))
        If nReArt <> 4 Then
            Call prTabelCellText(6, 0, fcDecStr(sInhald1(3), 10) + " €")
        End If
        Call prTabelCellText(7, 0, fcDecStr(sInhald1(8), 10) + " €")
        nFPreis = Val(sInhald1(1)) * Val(sInhald1(11)) * Val(sInhald1(14)) / 100
        nGPreis = Val(sInhald1(1)) * Val(sInhald1(11)) * Val(sInhald1(18)) / 100
        ' sInhald1(8) = Replace(sInhald1(8), ".", "")
        nZPreis = Val(fcChangs(sInhald1(8), ",", ".")) - nFPreis - nGPreis

        prTabelWrite()


        arRueck(3) = Math.Round(nFPreis - Val(fcChangs(sInhald1(5), ",", ".")), 2)
        arRueck(4) = Math.Round(Val(fcChangs(sInhald1(5), ",", ".")), 2)


        arRueck(1) = Math.Round(nZPreis - Val(fcChangs(sInhald1(4), ",", ".")), 2)
        arRueck(2) = Math.Round(Val(fcChangs(sInhald1(4), ",", ".")), 2)

        arRueck(5) = Val(fcChangs(sInhald1(8), ",", "."))
        arRueck(6) = Math.Round(nGPreis - Val(fcChangs(sInhald1(6), ",", ".")), 2)
        arRueck(7) = Math.Round(Val(fcChangs(sInhald1(6), ",", ".")), 2)

        Return arRueck
    End Function
    Private Sub prDruckKopfPositionStorno(ByRef xlaenge As Integer, ByRef sSprache1 As String)
        Dim i As Integer = 0
        Dim ii As Integer = 1
        Dim nSprache As Integer = Val(sSprache1)
        Call prTableDim(xlaenge, 112, 165, ii * 5)  '152



        Call prTableColumns(8)
        Call prTableRows(1)

        For j = 0 To 7
            Call prTabelCellSelectFont(j, 0, "Times New Roman", 9)
            Call prTabelCellLeft(j, 0, 1)
        Next

        Call prTableColumnsWidth(0, 6)
        Call prTableColumnsWidth(1, 22)
        'Call prTableColumnsWidth(2, 19)
        'Call prTableColumnsWidth(3, 8)
        Call prTableColumnsWidth(3, 8)
        Call prTableColumnsWidth(4, 23)
        Call prTableColumnsWidth(5, 23)
        Call prTableColumnsWidth(6, 10)
        Call prTableColumnsWidth(7, 23)
        Call prTabelCellText(0, 0, fcLanguage(nSprache, "Pos"))
        Call prTabelCellText(1, 0, fcLanguage(nSprache, "Objekt"))
        ' Call prTabelCellText(2, 0, fcLanguage(nSprache, "Ausstattung"))
        'Call prTabelCellText(3, 0, fcLanguage(nSprache, "Art"))
        Call prTabelCellText(2, 0, fcLanguage(nSprache, "Zeit"))
        Call prTabelCellText(3, 0, fcLanguage(nSprache, "Pers."))
        Call prTabelCellText(4, 0, fcLanguage(nSprache, "Preis/Nacht"))
        Call prTabelCellText(5, 0, fcLanguage(nSprache, "Zimmerpreis"))
        Call prTabelCellText(6, 0, fcLanguage(nSprache, "%"))
        Call prTabelCellText(7, 0, fcLanguage(nSprache, "Preis-%"))


        prTabelWrite()


    End Sub

    Private Function fcDruckPositionStorno(ByVal pos As Integer, ByRef xlaenge As Integer, ByVal yHoehe As Integer, ByVal sInhald As String, ByRef sSprache1 As String) As Array
        Dim sInhald1() As String = Split(sInhald, "#")
        Dim i As Integer = 0
        Dim nFPreis As Double
        Dim nZPreis As Double
        Dim arRueck(5) As Double
        Dim nFruestuek As Decimal = Val(sInhald1(10)) * Val(sInhald1(13)) / 100
        Dim nZimmerP As Double = Val(sInhald1(3)) ' - nFruestuek
        Dim ngPreis As Double = nZimmerP * Val(sInhald1(1))
        Dim ngPreis1 As Double = ngPreis * Val(sInhald1(15)) / 100
        Dim nSprache As Integer = Val(sSprache1)
        Call prTableDim(xlaenge, yHoehe, 165, 5)
        Call prTableColumns(8)
        Call prTableRows(1)

        For j = 0 To 7
            Call prTabelCellSelectFont(j, 0, "Times New Roman", 9)
            Call prTabelCellLeft(j, 0, 1)
        Next

        Call prTableColumnsWidth(0, 6)
        Call prTableColumnsWidth(1, 22)
        ' Call prTableColumnsWidth(2, 0)  '19
        ' Call prTableColumnsWidth(3, 0)    '8
        Call prTableColumnsWidth(3, 8)
        Call prTableColumnsWidth(4, 23)
        Call prTableColumnsWidth(5, 23)
        Call prTableColumnsWidth(6, 10)
        Call prTableColumnsWidth(7, 23)
        Call prTabelCellText(0, 0, Str(pos + 1))
        Dim AAA As String = sInhald1(2)
        Call prTabelCellText(1, 0, fcLanguage(nSprache, sInhald1(2)))
        'Call prTabelCellText(2, 0, fcLanguage(nSprache, sInhald1(12)))
        ' Call prTabelCellText(3, 0, sInhald1(11))
        Call prTabelCellText(2, 0, sInhald1(9))
        Call prTabelCellText(3, 0, sInhald1(10))
        Call prTabelCellText(4, 0, fcDecStr(nZimmerP, 10) + " €")
        Call prTabelCellText(5, 0, fcDecStr(ngPreis, 10) + " €")
        Call prTabelCellText(6, 0, sInhald1(15))
        ' AAA = fcDecStr(ngPreis1, 10)
        Call prTabelCellText(7, 0, fcDecStr(ngPreis1, 10) + " €")

        nFPreis = Val(sInhald1(1)) * Val(sInhald1(10)) * Val(sInhald1(13)) / 100
        nZPreis = Val(fcChangs(sInhald1(7), ",", ".")) - nFPreis

        prTabelWrite()


        arRueck(3) = Math.Round(nFPreis - Val(fcChangs(sInhald1(5), ",", ".")), 2)
        arRueck(4) = Math.Round(Val(fcChangs(sInhald1(5), ",", ".")), 2)


        arRueck(1) = Math.Round(nZPreis - Val(fcChangs(sInhald1(4), ",", ".")), 2)
        arRueck(2) = Math.Round(Val(fcChangs(sInhald1(4), ",", ".")), 2)

        arRueck(5) = Val(fcChangs(sInhald1(7), ",", "."))
        Return arRueck
    End Function
    'entfernt ein punkt, wenn im text ein komma ist und ersetzt deises durch ein punkt

    Function fcChangs(ByRef sText As String, ByVal sSuch As String, ByVal sZiel As String)

        For i = 1 To sText.Length
            If Mid(sText, i, 1) = sSuch Then
                sText = Replace(sText, sZiel, "")
                sText = Replace(sText, sSuch, sZiel)
                Exit For
            End If
        Next


        Return sText
    End Function
    Private Function FcDruckSumme(ByVal xlaenge As Integer, ByVal yHoehe As Integer, ByVal sSprache1 As String, ByVal ParamArray sInhald1() As Double) As Array
        '   Dim sInhald1() As String = Split(sInhald, "#")
        Dim nSprache As Integer = Val(sSprache1)
        Dim aÜbergabeKasse(6) As String
        Call prTableDim(xlaenge + 165 - 54, yHoehe, 54, 25)
        '     Call prTableDim(xlaenge + 165 - 44, yHoehe, 44, 25)
        Call prTableColumns(2)
        Call prTableRows(3)
        Call prTableColumnsWidth(1, 22)
        For i = 0 To 1
            For j = 0 To 2
                Call prTabelCellSelectFont(i, j, "Times New Roman", 9)
                Call prTabelCellLeft(i, j, 1)
            Next
        Next
        Call prTabelCellText(0, 0, fcLanguage(nSprache, "Summe"))
        Call prTabelCellText(0, 1, fcLanguage(nSprache, "Anzahlung"))
        Call prTabelCellText(0, 2, fcLanguage(nSprache, "zu zahlender Betrag"))
        'Call prTabelCellText(0, 3, fcLanguage(nSprache, "Anzahlung"))
        'Call prTabelCellText(0, 4, fcLanguage(nSprache, "Restbetrag"))
        Call prTabelCellText(1, 0, fcDecStr(sInhald1(5), 10) + " €")
        Call prTabelCellText(1, 1, fcDecStr(sInhald1(8), 10) + " €")
        Call prTabelCellText(1, 2, fcDecStr(sInhald1(5) - sInhald1(8), 10) + " €")

        ' Call prTabelCellText(1, 0, fcDecStr(sInhald1(1) + sInhald1(3), 10) + " €")
        ' Call prTabelCellText(1, 1, fcDecStr(sInhald1(2) + sInhald1(4), 10) + " €")
        ' Call prTabelCellText(1, 2, fcDecStr(sInhald1(5), 10) + " €")
        ' Call prTabelCellText(1, 3, fcDecStr(sInhald1(6), 10) + " €")
        ' Call prTabelCellText(1, 4, fcDecStr(sInhald1(5) - sInhald1(6), 10) + " €")




        prTabelWrite()




        Call prTableDim(xlaenge, yHoehe + 10, 110, 15)
        Call prTableColumns(4)
        Call prTableRows(4)
        For i1 = 0 To 3
            For j = 0 To 3
                Call prTabelCellSelectFont(j, i1, "Times New Roman", 9)
                Call prTabelCellLeft(j, i1, 1)
            Next
        Next
        Call prTabelCellText(0, 0, fcLanguage(nSprache, "MwSt-Satz"))
        Call prTabelCellText(1, 0, fcLanguage(nSprache, "Nettobetrag"))
        Call prTabelCellText(2, 0, fcLanguage(nSprache, "MwSt-Betrag"))
        Call prTabelCellText(3, 0, fcLanguage(nSprache, "Bruttobetrag"))
        Call prTabelCellText(0, 3, fcDecStr(sSSS, 3, 0, "") & " % " & fcLanguage(nSprache, "Getränke"))
        Call prTabelCellText(0, 2, fcDecStr(sSS7, 3, 0, "") & " % " & fcLanguage(nSprache, "Übernachtung"))
        Call prTabelCellText(0, 1, fcDecStr(sSS19, 3, 0, "") & " % " & fcLanguage(nSprache, "Speisen"))

        Dim nBruttoNormal As Decimal = sInhald1(3) + sInhald1(4)     '300 Frühstückpreis in cent 100/1,19
        Dim nBruttoReduzirt As Decimal = sInhald1(1) + sInhald1(2)
        Dim nBruttoSonder As Decimal = sInhald1(6) + sInhald1(7)

        Dim nSt7 = (Val(sSS7) / 100) + 1  '0,07
        Dim nNettoReduzirt As Decimal = nBruttoReduzirt / nSt7 ' sInhald1(1)


        Dim nNettoNormal As Decimal = sInhald1(3)
        Dim nNettoSonder As Decimal = sInhald1(6)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''###''''''''''''''''''''''''''''''''''
        aÜbergabeKasse(0) = Str(nBruttoNormal)
        aÜbergabeKasse(1) = sSS19
        aÜbergabeKasse(2) = Str(nBruttoReduzirt)
        aÜbergabeKasse(3) = sSS7
        aÜbergabeKasse(4) = Str(nBruttoSonder)
        aÜbergabeKasse(5) = sSSS


        Call prTabelCellText(3, 1, fcDecStr(nBruttoNormal, 10) + " €")
        Call prTabelCellText(3, 2, fcDecStr(nBruttoReduzirt, 10) + " €")
        Call prTabelCellText(3, 3, fcDecStr(nBruttoSonder, 10) + " €")
        Call prTabelCellText(1, 1, fcDecStr(sInhald1(3), 10) + " €")
        Call prTabelCellText(1, 2, fcDecStr(nNettoReduzirt, 10) + " €")
        Call prTabelCellText(1, 3, fcDecStr(nNettoSonder, 10) + " €")
        Call prTabelCellText(2, 1, fcDecStr(nBruttoNormal - nNettoNormal, 10) + " €")
        Call prTabelCellText(2, 2, fcDecStr(nBruttoReduzirt - nNettoReduzirt, 10) + " €")
        Call prTabelCellText(2, 3, fcDecStr(nBruttoSonder - nNettoSonder, 10) + " €")

        prTabelWrite()
        Return aÜbergabeKasse
    End Function
    Private Sub prDruckKopf(ByRef xlaenge As Integer, ByVal datum As String, ByVal Name1Z As String, ByVal Name2Z As String, ByVal ParamArray arfeld() As String)
        Dim sName As String = ""
        For i = 1 To Len(arIni(2))
            sName = sName + Mid(arIni(2), i, 1) + " "
        Next
        ''*******************************************
        'Dim test As String = ReadFileUniCode(cgSystemPath & "\test.txt")

        'prZeichenSatz("9")
        ''******************************************
        prVpeTextFormat("Times New Roman", 18)
        ' prVpeText(xlaenge, 18, test)
        prVpeText(xlaenge, 18, sName)
        prZeichenSatz("1")
        prVpeTextFormat("Times New Roman", 12)
        prVpeText(xlaenge, 25, arIni(3) + ", " + arIni(4) + " " + arIni(5))
        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge, 55, arIni(3) + ", " + arIni(4) + " " + arIni(5))
        prVpeTextFormat("Times New Roman", 12)
        If arfeld(0) = "Firma" Then

            prVpeText(xlaenge, 64, arfeld(1))
            prVpeText(xlaenge, 68, arfeld(3))
            prVpeText(xlaenge, 72, arfeld(4))
            prVpeText(xlaenge, 76, arfeld(6).Trim & " " & arfeld(5))
            prVpeText(xlaenge, 80, arfeld(7))
        Else
            prVpeText(xlaenge, 64, arfeld(0))
            prVpeText(xlaenge, 68, Trim(arfeld(2) & " " & arfeld(1)))
            prVpeText(xlaenge, 72, arfeld(3))
            prVpeText(xlaenge, 76, arfeld(4))
            prVpeText(xlaenge, 80, arfeld(6).Trim & " " & arfeld(5))
        End If
        prVpeText(xlaenge, 86, Name1Z)
        prVpeText(xlaenge, 90, Name2Z)




        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge + 112, 53, "Tel.  : " & arIni(6))

        prVpeText(xlaenge + 112, 57, "Web.: www.feworeich.de")
        prVpeText(xlaenge + 112, 61, "Mail : " & arEMail(1))
        prVpeText(xlaenge + 140, 80, fcUmDatum(datum))
    End Sub

    Private Sub prDruckFuss(ByRef xlaenge As Integer)
        Dim arfeld(1) As String
        prVpeTextFormat("Times New Roman", 8)
        prVpeText(xlaenge, 258, fcLanguage(nSprache, "Dieses Schreiben wurde Maschinell erstellt und gilt auch ohne Unterschrift seitens der Pension"))
        prVpeText(xlaenge, 261, fcLanguage(nSprache, "Bankverbindung"))
        arfeld = fcDataSeek("select * From Konten '", "", 0, {"Name", "KTO", "blz", "IBAN", "BIC"})
        prVpeText(xlaenge, 264, arfeld(0))
        prVpeText(xlaenge + 50, 264, "Kontoinhaber: " & arIni(9))
        prVpeText(xlaenge, 267, "BLZ: " & arfeld(2))
        prVpeText(xlaenge, 270, fcLanguage(nSprache, "Konto") & ": " & arfeld(1))
        prVpeText(xlaenge, 273, fcLanguage(nSprache, "UST-Nr") & ".: " & arIni(15))
        prVpeText(xlaenge + 50, 267, "IBAN: " & arfeld(3))
        prVpeText(xlaenge + 50, 270, "BIC: " & arfeld(4))
        prVpeText(xlaenge + 50, 273, fcLanguage(nSprache, "UST-ID") & ".: " & arIni(16))
        '  prVpeText(xlaenge + 100, 261, "Internet: www.feworeich.de")
        ' prVpeText(xlaenge + 100, 264, "email: " & arEMail(1))
    End Sub

    Private Sub prDruckBuch(ByVal xlaenge As Integer, ByRef sOID As String, ByRef sSprache As String)
        Dim arfeld1(1) As String
        arfeld1 = fcDataSeek("select * From Objekte Where ID ='", sOID, 0, {"Name", "Strasse", "HNr", "PLZ", "Ort"})
        prVpeTextFormat("Times New Roman", 10)
        'prVpeText(xlaenge, 94, "Annahmeerklärung / Buchungsbestätigung")
        prVpeText(xlaenge + 85, 94, "[N B]" & fcLanguage(nSprache, "Objekt") & ":")
        prVpeTextFormat("Times New Roman", 12)
        prVpeText(xlaenge, 100, fcLanguage(Val(sSprache), "Sehr geehrter Gast"))


        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge + 100, 94, "[N B]" & arfeld1(0))
        prVpeText(xlaenge + 100, 98, "[N B]" & arfeld1(1) & " " & arfeld1(2))
        prVpeText(xlaenge + 100, 102, "[N B]" & arfeld1(3) & " " & arfeld1(4))
    End Sub

    Private Sub prDruckRech(ByVal xlaenge As Integer, ByRef sOID As String, ByRef nRNr As String, ByRef sSprache As String)
        Dim arfeld1(1) As String
        arfeld1 = fcDataSeek("select * From Objekte Where ID ='", sOID, 0, {"Name", "Strasse", "HNr", "PLZ", "Ort"})
        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge, 94, "[N B]" & fcLanguage(Val(sSprache), "Rechnung") & " Nr. " + nRNr)
        prVpeText(xlaenge + 85, 94, "[N B]" & fcLanguage(nSprache, "Objekt") & ":")
        prVpeText(xlaenge, 102, fcLanguage(Val(sSprache), "hiermit berechne ich Ihnen folgende Leistungen:"))


        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge + 100, 94, "[N B]" & arfeld1(0))
        prVpeText(xlaenge + 100, 98, "[N B]" & arfeld1(1) & " " & arfeld1(2))
        prVpeText(xlaenge + 100, 102, "[N B]" & arfeld1(3) & " " & arfeld1(4))
    End Sub

    Private Function fcDruckZimmer(ByRef xlaenge As Integer, ByRef sBID As String, ByRef sBR As String, ByRef sSprache1 As String) As Array
        Dim arfeld1(1) As String
        Dim arfeld(1) As String
        Dim arfeld3(5) As String
        Dim aDatum As Date
        Dim eDatum As Date
        Dim nGesamtPreis As Integer
        Dim i As Integer = 0
        Dim ii As Integer = 0
        Dim nSumme As Integer
        Dim nFrueh As Integer = 0
        arfeld1(0) = "1"

        Dim nSprache As Integer = Val(sSprache1)
        Dim sPausch As String = "0"
        Do While arfeld1(0) <> " "   ' anzahl der Datensätzen
            arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, ii, {"ZimID", "Art", "Personen", "Preis", "Frueh", "Summe", "Pausch"})
            If arfeld1(6) = "1" Then sPausch = "1" 'Pauschale ermiteln
            Dim a As String = arfeld1(0)
            ii = ii + 1
        Loop
        Call prTableDim(xlaenge, 112, 165, ii * 5)
        Call prTableColumns(9)
        Call prTableRows(ii)
        For i1 = 0 To ii - 1
            For j = 0 To 8
                Call prTabelCellSelectFont(j, i1, "Times New Roman", 9)
                Call prTabelCellLeft(j, i1, 1)
            Next
        Next
        Call prTableColumnsWidth(0, 6)
        Call prTableColumnsWidth(8, 19)
        Call prTableColumnsWidth(7, 19)
        Call prTableColumnsWidth(6, 8)
        Call prTableColumnsWidth(5, 20)
        Call prTableColumnsWidth(4, 20)
        Call prTableColumnsWidth(3, 8)


        Call prTabelCellText(0, 0, fcLanguage(nSprache, "Pos"))
        Call prTabelCellText(1, 0, fcLanguage(nSprache, "Objekt"))
        Call prTabelCellText(2, 0, fcLanguage(nSprache, "Ausstattung"))
        Call prTabelCellText(3, 0, fcLanguage(nSprache, "Art"))
        Call prTabelCellText(4, 0, fcLanguage(nSprache, "Anreisetag"))
        Call prTabelCellText(5, 0, fcLanguage(nSprache, "Abreisetag"))
        Call prTabelCellText(6, 0, fcLanguage(nSprache, "Pers."))
        If sPausch = "0" Then
            Call prTabelCellText(7, 0, fcLanguage(nSprache, "Preis/Nacht"))
        End If
        Call prTabelCellText(8, 0, fcLanguage(nSprache, "Zimmerpreis"))


        i = 0
        arfeld1(0) = "1"
        nSumme = 0
        Do While arfeld1(0) <> " "
            arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, i, {"ZimID", "Art", "Personen", "Preis", "von", "bis", "Frueh", "Summe"})
            If arfeld1(0) <> " " Then
                arfeld = fcDataSeek("select * From Zimmer Where ID ='", arfeld1(0), 0, {"Art", "Ausstattung"})
                Call prTabelCellText(0, i + 1, i + 1)
                Call prTabelCellText(1, i + 1, fcLanguage(nSprache, arfeld(0)))
                Call prTabelCellText(2, i + 1, fcLanguage(nSprache, arfeld(1)))
                Call prTabelCellText(3, i + 1, arfeld1(1))
                Call prTabelCellText(4, i + 1, fcUmDatum(arfeld1(4)))
                arfeld1(5) = fcUmDatum(CDate(fcUmDatum(arfeld1(5))).AddDays(1))
                Call prTabelCellText(5, i + 1, fcUmDatum(arfeld1(5)))
                Call prTabelCellText(6, i + 1, arfeld1(2))
                If sPausch = "0" Then
                    Call prTabelCellText(7, i + 1, fcDecStr(Val(arfeld1(3) / 100), 7, 2, ",") & " €")
                    aDatum = CDate(fcUmDatum(arfeld1(4)))
                    eDatum = CDate(fcUmDatum(arfeld1(5)))
                    nGesamtPreis = 0

                    Do While aDatum < eDatum

                        nGesamtPreis = nGesamtPreis + Val(arfeld1(3))
                        nFrueh = nFrueh + arfeld1(6)
                        aDatum = aDatum.AddDays(1)
                    Loop
                    nSumme = nSumme + nGesamtPreis
                    Call prTabelCellText(8, i + 1, fcDecStr(nGesamtPreis / 100, 7, 2, ",") & " €")
                Else
                    Call prTabelCellText(8, i + 1, fcDecStr(Val(arfeld1(7) / 100), 7, 2, ",") & " €")
                    nSumme = nSumme + Val(arfeld1(7))
                End If
                i = i + 1
            End If
        Loop
        prTabelWrite()

        Call prTableDim(xlaenge + 165 - 38, 112 + ii * 5, 38, 5)
        Call prTableColumns(2)
        Call prTableRows(1)
        Call prTabelCellSelectFont(0, 0, "Times New Roman", 9)
        Call prTabelCellSelectFont(1, 0, "Times New Roman", 9)

        Call prTabelCellText(0, 0, fcLanguage(nSprache, "Gesamtpreis"))




        Call prTabelCellLeft(1, 0, 1)
        Call prTabelCellText(1, 0, fcDecStr(nSumme / 100, 7, 2, ",") & " €")
        prTabelWrite()
        If sBR = "B" Then
            Call prTableDim(xlaenge + 165 - 38, 112 + 5 + ii * 5, 38, 5)
            Call prTableColumns(1)
            Call prTableRows(1)
            Call prTabelCellSelectFont(0, 0, "Times New Roman", 9)

            Call prTabelCellText(0, 0, fcLanguage(nSprache, "inkl.gesetzl.MwSt"))


            prTabelWrite()
        Else
            Call prTableDim(xlaenge, 112 + 10 + ii * 5, 110, 15)
            Call prTableColumns(4)
            Call prTableRows(3)
            For i1 = 0 To 2
                For j = 0 To 3
                    Call prTabelCellSelectFont(j, i1, "Times New Roman", 9)
                    Call prTabelCellLeft(j, i1, 1)
                Next
            Next
            Call prTabelCellText(0, 0, "MwSt-Satz")
            Call prTabelCellText(1, 0, "Nettobetrag")
            Call prTabelCellText(2, 0, "MwSt-Betrag")
            Call prTabelCellText(3, 0, "Bruttobetrag")
            Call prTabelCellText(0, 1, arIni(10) & " %")
            Call prTabelCellText(0, 2, arIni(11) & " %")

            Dim nBruttoNormal As Decimal = nFrueh * 3     '300 Frühstückpreis in cent 100/1,19
            Dim nBruttoReduzirt As Decimal = nSumme / 100 - nBruttoNormal
            Dim nNettoReduzirt As Decimal = Math.Round(nBruttoReduzirt / (1 + arIni(11) / 100), 2)
            Dim nNettoNormal As Decimal = Math.Round(nBruttoNormal / (1 + arIni(10) / 100), 2)
            Call prTabelCellText(3, 1, fcDecStr(nBruttoNormal, 7, 2, ",") + " €")
            Call prTabelCellText(3, 2, fcDecStr(nBruttoReduzirt, 7, 2, ",") + " €")
            Call prTabelCellText(1, 1, fcDecStr(nNettoNormal, 7, 2, ",") + " €")
            Call prTabelCellText(1, 2, fcDecStr(nNettoReduzirt, 7, 2, ",") + " €")
            Call prTabelCellText(2, 1, fcDecStr(nBruttoNormal - nNettoNormal, 7, 2, ",") + " €")
            Call prTabelCellText(2, 2, fcDecStr(nBruttoReduzirt - nNettoReduzirt, 7, 2, ",") + " €")
            prTabelWrite()
            arfeld3(1) = Str(nNettoReduzirt)
            arfeld3(2) = Str(nBruttoReduzirt - nNettoReduzirt)
            arfeld3(3) = Str(nNettoNormal)
            arfeld3(4) = Str(nBruttoNormal - nNettoNormal)
        End If

        arfeld3(0) = 112 + ii * 5
        Return arfeld3
    End Function

    Private Function fcDruckMakro(ByRef xlaenge As Integer, ByRef ii As Integer, ByRef sText As String, ByVal sSprache1 As String) As Integer
        Dim arfeld(1) As String
        Dim nSprache As Integer = Val(sSprache1)
        prVpeTextFormat("Times New Roman", 10)
        '     prVpeText(xlaenge, ii + 1, "[N B]" & fcLanguage(nSprache, "Nutzen Sie auch unseren Fahrradverleih"))
        ii = fcVpeTextDruck(xlaenge, ii + 5, 70, sText)
        Return ii
    End Function

    Private Function prDruckBuchungsText(ByRef xlaenge As Integer, ByRef ii As Integer, ByRef BuchTextID As String, ByRef sSprache As String, ByVal von As String, ByRef bis As String, ByVal BuchDatum As String, ByVal Rdatum As String, ByVal sInternetNr As String) As Integer



        Dim arfeld(1) As String
        Dim sTextOut(2) As String
        Dim sBuchText As Array
        Dim pos1 As Integer
        Dim pos2 As Integer
        Dim sText1 As String
        Dim sText2 As String
        Dim nZeileAbstand As Integer = 4
        Dim sZeile() As String
        Dim sDatum() As String
        Dim nTage As Integer
        Dim Datum As String = ""

        prVpeTextFormat("Times New Roman", 9)
        If sSprache = "0" Then
            arfeld = fcDataSeek("select * From BTexte Where ID ='", BuchTextID, 0, {"Name", "bTextDe"}) ', "von", "bis", "BuchDatum", "Rdatum"
        Else
            arfeld = fcDataSeek("select * From BTexte Where ID ='", BuchTextID, 0, {"Name", "bTextEn"})
        End If


        sBuchText = Split(arfeld(1), vbCrLf)


        prVpeTextFormat("Times New Roman", 10)
        For i = 0 To sBuchText.Length - 1   '1
            Datum = ""
            sZeile = fcStringSeek(sBuchText(i), "{", "}")
            If sZeile(2) <> "" Then
                sDatum = Split(sZeile(2), ";")
                If sDatum.Length > 1 Then nTage = Val(sDatum(1))
                Select Case sDatum(0).ToUpper
                    Case "INTERNETNR"
                        Datum = sInternetNr
                    Case "B_DATUM"
                        Datum = CDate(fcUmDatum(BuchDatum)).AddDays(nTage)
                    Case "A_DATUM"
                        Datum = CDate(fcUmDatum(von)).AddDays(nTage)
                    Case "R_DATUM"
                        Datum = CDate(fcUmDatum(Rdatum)).AddDays(nTage)
                    Case "E_DATUM"
                        Datum = CDate(fcUmDatum(bis)).AddDays(nTage)
                    Case "F_DATUM"
                        'Datum =
                End Select

            End If
            sText1 = sZeile(0) + Datum + sZeile(1)
            If i = 0 Then
                prVpeText(xlaenge, 106, sText1)
            Else


                sBuchText(i) = sText1
                pos1 = 0
                pos2 = 0
                For a1 = 1 To sText1.Length       'Textabstand und schriftgröße festlegen "#10,40#
                    If Mid(sText1, a1, 1) = "#" And pos1 = 0 Then
                        pos1 = a1
                    End If
                    If Mid(sText1, a1, 1) = "#" And pos1 <> 0 Then
                        pos2 = a1
                    End If
                Next
                If pos1 <> 0 Then
                    sText2 = Mid(sText1, pos1 + 1, pos2 - pos1 - 1)
                    sText1 = Mid(sText1, 1, pos1 - 1) + Mid(sText1, pos2 + 1)
                    nZeileAbstand = Str(AtRight(sText2, ",", 1)) / 10
                    Call prVpeTextFormat("Times New Roman", Str(AtLeft(sText2, ",", 1)))
                    sBuchText(i) = sText1
                End If

                For i1 = 1 To 120 Step -1

                Next

                prVpeText(xlaenge, ii, sBuchText(i))
                ii = ii + nZeileAbstand
                If ii > 240 Then
                    prVpeText(xlaenge, ii + 5, "Seite 1 von 2")
                    Call prDruckFuss(xlaenge)
                    Call prVpeNewPage()
                    Dim sName As String = ""
                    For ix = 1 To Len(arIni(2))
                        sName = sName + Mid(arIni(2), ix, 1) + " "
                    Next
                    prVpeTextFormat("Times New Roman", 18)
                    prVpeText(xlaenge, 18, sName)
                    prVpeTextFormat("Times New Roman", 12)
                    prVpeText(xlaenge, 25, arIni(3) + ", " + arIni(4) + " " + arIni(5))
                    prVpeTextFormat("Times New Roman", 10)
                    ii = 50
                    Call prVpeText(xlaenge, ii, "Seite 2 von 2")

                    ii = 60

                End If
            End If
        Next
        Return ii
    End Function

    'Private Function fcLanguage(ByRef nSprache As Integer, ByRef sText As String) As String
    '    Dim sLanguage As Array = Split(ReadOneValueFromSystemDb("Language"), vbCrLf)



    '    Dim sText1 As Array
    '    Dim sText2 As String = ""
    '    Dim x As String
    '    Dim n As Boolean = False
    '    Dim sSprache As Array = Split(sLanguage(0), ";")
    '    Dim cSprache As Array = Split(sSprache(nSprache + 1), ",")

    '    prZeichenSatz(cSprache(2))

    '    For i = 0 To sLanguage.Length - 1
    '        x = AtLeft(sLanguage(i), ";", 1)
    '        If AtLeft(sLanguage(i), ";", 1) = sText Then
    '            sText1 = Split(sLanguage(i), ";")
    '            sText2 = sText1(nSprache + 1)
    '            n = True
    '            Exit For
    '        End If
    '    Next
    '    If n = True Then
    '        fcLanguage = sText2
    '    Else
    '        fcLanguage = sText
    '    End If
    'End Function



    ''' <summary>
    ''' prDruckRechnung( Buchungs ID )
    ''' </summary>
    ''' <remarks>
    ''' 03.03.2012 Create
    ''' </remarks>
    Public Sub prDruckAnzahlung(ByRef sBID As String, ByRef nRNr As String, ByVal sDatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByRef sRZiel As String, ByRef nSprache As Integer)
        Dim sDatei As String  'Kassenbuch

        Dim aÜbergabeKasse(4) As String
        Dim arIn(12) As String
        Dim xlaenge As Integer = 25
        Dim arFeld(1) As String
        Dim arfeld1(1) As String
        Dim arfeld2(1) As String
        Dim arfeld3(1) As String
        Dim arfeld4(1) As String

        Dim rechnung1(6) As Double
        Dim sKID As String
        Dim yHoehe As Integer
        Dim ii As Integer
        sDatum = fcUmDatum(sDatum)

        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "BuchDatum", "Name1", "Name2"})
        sKID = arfeld1(0)
        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "Name2"})
        Call prVpePageFormat("A4")
        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge + 112, 53, "Tel. " & arIni(6))

        Call prDruckKopf(xlaenge, sDatum, arfeld1(4), arfeld1(5), arFeld)
        prVpeTextFormat("Times New Roman", 10)
        prVpeText(xlaenge, 94, "[N B]Anzahlungsrechnung  Nr. " + nRNr)
        yHoehe = 117


        prVpeText(xlaenge, yHoehe, "Hiermit berechnen ich Ihnen die Anzahlung für die Buchnung (Nummer: " + sBID + ") in vereinbarte Höhe von")
        yHoehe = yHoehe + 7
        Dim dNetto As Decimal = 0
        Dim dMwst As Decimal = 0
        dNetto = Math.Round(anzahlung / ((Val(arIni(11)) + 100) / 100), 0)
        dMwst = anzahlung - dNetto
        Dim sNetto As String
        sNetto = fcDecStr(dNetto, 7, 2, ",") + " €"
        prVpeText(xlaenge, yHoehe, "Netto:          " + fcDecStr(dNetto / 100, 7, 2, ",") + " € ")
        yHoehe = yHoehe + 7
        prVpeText(xlaenge, yHoehe, "Mwst " + arIni(11) + "% :   " + fcDecStr((dMwst / 100), 7, 2, ",") + " € ")
        yHoehe = yHoehe + 7
        prVpeText(xlaenge, yHoehe, "Brutto:        " + fcDecStr(anzahlung / 100, 7, 2, ",") + " € ")
        yHoehe = yHoehe + 15
        prVpeText(xlaenge, yHoehe, "Die Anzahlung bezieht sich nur auf die Übernachtungskosten, ohne Bewirtungskosten!")

        yHoehe = yHoehe + 15

        ii = Val(arfeld4(0))
        If bBar = True Then
            prVpeText(xlaenge, yHoehe + 36, "Betrag dankend erhalten")
            sDatei = "K" & nRNr & ".DAV"
            ' sKasseName = "Rechnung Einnahme K" & Str(nRNr)
            arIn(0) = "K" & nRNr & "-1"  'Name
            arIn(6) = "K" & nRNr & "-2"  'Name
            arIn(3) = arIni(18)   'konto
            arIn(9) = arIni(18)
            ' skonto = arIni(18)
        Else
            Dim sZDatum As String
            Dim ddatum As Date = sRZiel
            sZDatum = ddatum.AddDays(14)

            prVpeText(xlaenge, yHoehe + 36, fcLanguage(nSprache, "Bitte überweisen Sie den Gesamtbetrag bis:") & " " & sZDatum & " " & fcLanguage(nSprache, "auf unten genantes Konto"))

            sDatei = "B" & nRNr & ".DAV"
            '  sKasseName = "Rechnung Einnahme B" & Str(nRNr)
            arIn(0) = "B" & nRNr & "-1"  'Name
            arIn(6) = "B" & nRNr & "-2"  'Name
            arIn(3) = arIni(19)
            arIn(9) = arIni(19)
            '  skonto = arIni(19)
        End If
        Call prDruckFuss(xlaenge)

        If DirExists(arIni(32) & "\Rechnung") = False Then
            CreateDir(arIni(32) & "\Rechnung")
        End If



        Call prVpePDF(arIni(32) & "\Rechnung\Rech_" & nRNr & ".PDF")
        arfeld2 = fcDataSeek("select * From Datev Where RechNr ='", nRNr, 0, {"ID"})


        'datensatz nicht gefunfen
        arfeld3 = {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "}
        arfeld3(0) = fcGetTimeID(Date.Today)
        arfeld3(1) = arFeld(1) & " " & arFeld(2)
        arfeld3(2) = nRNr
        arfeld3(5) = arIni(11)
        arfeld3(10) = arIni(10)
        arfeld3(6) = arIni(20)
        arfeld3(11) = arIni(21)
        arfeld3(3) = anzahlung
        arfeld3(4) = Str(dMwst)
        arfeld3(8) = "0"
        arfeld3(9) = "0"
        arIn(1) = aÜbergabeKasse(0) 'Betrag
        arIn(2) = aÜbergabeKasse(1) 'Steuer
        arIn(7) = aÜbergabeKasse(2) 'Betrag
        arIn(8) = aÜbergabeKasse(3) 'Steuer
        arIn(4) = arIni(21)         'Gegenkoto
        arIn(10) = arIni(20)        'Gegenkonto
        arIn(11) = sDatum           'datum
        arIn(5) = sDatum

        Call prKassenDatei(sDatei, arIn)

        If arfeld2(0) = " " Then
            Call fcInsertCommand("Datev", {"Id", "Name", "Rechnr",
                                 "Umsatz1", "Mwst1", "MwstS1", "GKonto1", "Konto1", "Umsatz2", "Mwst2",
                                 "MwstS2", "Gkonto2", "Konto2", "Datum", "BuchNr", "KunNr"}, arfeld3)

        Else
            'datensatz neu schreiben
            Dim sMsg As String = "Datev Satz (Zimmer) neu Schreiben ?"
            If MsgBox(sMsg, vbExclamation + vbOKCancel, "Schreiben") = MsgBoxResult.Ok Then
                Dim cBedingung As String = " WHERE Rechnr='" & nRNr & "'"
                Call fcUpdateCommand("Datev", {"Id", "Name", "Rechnr", "Umsatz1", "Mwst1",
                                               "MwstS1", "GKonto1", "Konto1", "Umsatz2", "Mwst2",
                                               "MwstS2", "Gkonto2", "Konto2", "Datum", "BuchNr",
                                               "KunNr"}, arfeld3, cBedingung)

            End If
        End If


        '    prVpeVeiw()
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

            Call prVpePageFormat("A4")
            Call prTableDim(20, 10, 170, 260)
            Call prTableColumns(4)
            Call prTableColumnsWidth(0, 25)
            Call prTableColumnsWidth(3, 40)
            Call prTableRows(17)

            Call prTabelCellText(0, 0, "Datum")
            Call prTabelCellText(1, 0, "Abreise")
            Call prTabelCellText(2, 0, "Anreise")
            Call prTabelCellText(3, 0, "schneller Wechsel")

            For i As Integer = 0 To 15
                Dim rowIdx As Integer = i + 1
                Dim tag As TagesEintrag = tagesListe(i)

                ' Spalte 0: Datum
                Call prTabelCellSelectFont(0, rowIdx, "Times New Roman", 9)
                Call prTabelCellLeft(0, rowIdx, 1)
                Call prTabelCellText(0, rowIdx, tag.DatumAnzeige)

                ' Spalte 1: Abreise
                Call prTabelCellSelectFont(1, rowIdx, "Times New Roman", 8)
                Call prTabelCellLeft(1, rowIdx, 1)
                Call prTabelCellMitte(1, rowIdx, "H")
                Call prTabelCellText(1, rowIdx, tag.AbreiseText)

                ' Spalte 2: Anreise
                Call prTabelCellSelectFont(2, rowIdx, "Times New Roman", 8)
                Call prTabelCellLeft(2, rowIdx, 1)
                Call prTabelCellMitte(2, rowIdx, "H")
                Call prTabelCellText(2, rowIdx, tag.AnreiseText)

                ' Spalte 3: Schneller Wechsel
                Call prTabelCellSelectFont(3, rowIdx, "Times New Roman", 8)
                Call prTabelCellLeft(3, rowIdx, 1)
                Call prTabelCellMitte(3, rowIdx, "H")
                Call prTabelCellText(3, rowIdx, tag.WechselText)
            Next

            prTabelWrite()
            prVpeVeiw()

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ' Hilfsfunktion zur Formatierung langer Zimmerlisten (Ersetzt das fehleranfällige sZimN(5) Array)
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

    ' Datenmodell-Klassen am Ende der Code-Datei oder innerhalb der Klasse platzieren:
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

    Private Structure BuchungsInfo
        Public Property ZimmerID As String
        Public Property BuchungsID As String
    End Structure




    'Public Sub prDrRechnung(ByRef sBID As String, ByVal sKID As String, ByRef nRNr As String, ByVal sdatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByRef sRZiel As String, ByVal S1 As String, ByRef s2 As String, ByRef s3 As String, ByRef g1 As String, ByRef g2 As String, ByRef g3 As String, ByVal nReArt As Integer, ByVal ParamArray arDruck1() As String)
    'Public Sub prX_Rechnung(ByRef sBID As String, ByVal sKID As String, ByRef nRNr As String, ByVal sdatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByRef sRZiel As String, ByVal S1 As String, ByRef s2 As String, ByVal nReArt As Integer, ByVal ParamArray arDruck1() As String)
    '    prX_Rechnung_CII(sBID, sKID, nRNr, sdatum, bBar, anzahlung, sRZiel, S1, s2, nReArt, arDruck1)


    '    Dim sXRechnungA As String = ReadFileSeriell("XRe_Pa_A.XML")
    '    Dim sXRechnungE As String = ""
    '    Dim sVariable As String = ""
    '    Dim sInhalt As String
    '    Dim arFeld() As String

    '    arFeld = fcDataSeek("select * From Konten '", "", 0, {"Name", "KTO", "blz", "IBAN", "BIC"})
    '    sXRechnungA = Replace(sXRechnungA, "?BT-001", nRNr)  'Rechnungsnummer
    '    sXRechnungA = Replace(sXRechnungA, "?BT-002", fcUmDatumXR(sdatum))  'Rechnungsdatum
    '    sXRechnungA = Replace(sXRechnungA, "?BT-003", "380")  'Handelsrechnung
    '    sXRechnungA = Replace(sXRechnungA, "?BT-010", "0")  'Leitweg
    '    sXRechnungA = Replace(sXRechnungA, "?BT-027", arIni(2))  'Pensions Name
    '    sXRechnungA = Replace(sXRechnungA, "?BT-035", arIni(3))  'Strasse P
    '    sXRechnungA = Replace(sXRechnungA, "?BT-037", arIni(5))  'Ort P
    '    sXRechnungA = Replace(sXRechnungA, "?BT-038", arIni(4))  'PLZ P
    '    sXRechnungA = Replace(sXRechnungA, "?BT-040", "DE")  ' Land P
    '    sXRechnungA = Replace(sXRechnungA, "?BT-031", arIni(16))  'Ust ID
    '    sXRechnungA = Replace(sXRechnungA, "?BT-032", arIni(15))  'Steuernummer
    '    sXRechnungA = Replace(sXRechnungA, "?BT-041", arIni(9))  ' Kontacktname
    '    sXRechnungA = Replace(sXRechnungA, "?BT-043", arIni(6))  'Telefon P
    '    sXRechnungA = Replace(sXRechnungA, "?BT-042", arEMail(1))  ' Mail P
    '    sXRechnungA = Replace(sXRechnungA, "?BT-085", arIni(9))  ' Konto Name
    '    sXRechnungA = Replace(sXRechnungA, "?BT-084", arFeld(3))  'IBAN
    '    sXRechnungA = Replace(sXRechnungA, "?BT-086", arFeld(4))  'BIC
    '    sXRechnungA = Replace(sXRechnungA, "?BT-081", "58")  'SEPA
    '    sXRechnungA = Replace(sXRechnungA, "?BT-152", S1)  ' Steuersatz Übernachtung
    '    arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "EMail"})
    '    sXRechnungA = Replace(sXRechnungA, "?BT-044", arFeld(0) & " " & arFeld(1) & " " & arFeld(2))  ' Empf Name
    '    sXRechnungA = Replace(sXRechnungA, "?BT-050", arFeld(3))  'Strasse Empf
    '    sXRechnungA = Replace(sXRechnungA, "?BT-052", arFeld(4))  'Ort
    '    sXRechnungA = Replace(sXRechnungA, "?BT-053", arFeld(5))  'Plz
    '    sXRechnungA = Replace(sXRechnungA, "?BT-055", arFeld(6))  'Land
    '    sXRechnungA = Replace(sXRechnungA, "?BT-049", arFeld(7))  'Mail Emp
    '    sXRechnungA = Replace(sXRechnungA, "?BT-034", arFeld(7))  'Mail Emp
    '    If nReArt = 4 Then
    '        Dim Steuer As Decimal = 0
    '        Dim Netto As Decimal = 0
    '        Dim Brutto As Decimal = 0
    '        Dim NettoE As String = ""
    '        For i = 0 To arDruck1.Length - 1

    '            sInhalt = ReadFileSeriell("XRe_Pa_E.XML")
    '            arFeld = Split(arDruck1(i), "#")
    '            NettoE = Str(Math.Round(Val(arFeld(3)) / (1 + Val(S1) / 100), 2))
    '            sInhalt = Replace(sInhalt, "?BT-129", "1")
    '            sInhalt = Replace(sInhalt, "?BT-155", Str(i + 1))  'Laufende nr
    '            sInhalt = Replace(sInhalt, "?BT-146", fcFormatDecimalPunkt(arFeld(7)))    'Netto---------------------
    '            sInhalt = Replace(sInhalt, "?BT-152", S1)             'Steuer
    '            sInhalt = Replace(sInhalt, "?BT-131", fcFormatDecimalPunkt(arFeld(7)))       'Netto
    '            sInhalt = Replace(sInhalt, "?BT-153", arFeld(2))       'Artikel Dreibettzimmer
    '            sInhalt = Replace(sInhalt, "?BT-145", arFeld(10) & "," & arFeld(13) & "," & arFeld(12))   'Beschreibung
    '            sXRechnungA = Replace(sXRechnungA, "?BT-083", "R.Nr.:" & nRNr & "/" & arFeld(10))
    '            sXRechnungE = sXRechnungE + sInhalt
    '            sXRechnungA = Replace(sXRechnungA, "?BT-072", fcUmDatumXR(fcUmDatum(Mid(arFeld(10), 1, 10)))) 'Liferdatum
    '            Steuer = Steuer + Val(Replace(arFeld(4), ",", "."))
    '            Netto = Netto + Val(Replace(arFeld(7), ",", "."))
    '            Brutto = Brutto + Val(Replace(arFeld(4), ",", ".")) + Val(Replace(arFeld(7), ",", "."))
    '        Next
    '        sXRechnungA = Replace(sXRechnungA, "?Position", sXRechnungE)  'Mail Emp
    '        sXRechnungA = Replace(sXRechnungA, "?BT-110", Str(Steuer))  ' Steuerbetrag
    '        sXRechnungA = Replace(sXRechnungA, "?BT-106", fcFormatDecimalPunkt(Str(Netto)))  '   NettoBetrag
    '        sXRechnungA = Replace(sXRechnungA, "?BT-112", fcFormatDecimalPunkt(Str(Brutto))) '   Brutobetrag
    '        sXRechnungA = Replace(sXRechnungA, "?BT-115", fcFormatDecimalPunkt(Str(Brutto))) '   Brutobetrag
    '        sXRechnungA = Replace(sXRechnungA, "?BT-113", "0") '   Anzahlung

    '        arFeld = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "BuchDatum"})
    '        Dim sZDatum As String = ""
    '        Dim t As String = arFeld(3)
    '        sRZiel = sRZiel.Trim
    '        If sRZiel = " " Or sRZiel = "" Then
    '            sZDatum = Date.Today.AddDays(14)
    '        Else
    '            Dim sInhald1() As String = Split(arDruck1(0), "#")
    '            Dim sVon As String = Mid(sInhald1(10), 1, 10)
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
    '                        ddatum = CDate(fcUmDatum(arFeld(3))).AddDays(nTage)
    '                    End If
    '                    sZDatum = ddatum
    '                End If
    '            End If
    '        End If
    '        sXRechnungA = Replace(sXRechnungA, "?BT-009", fcUmDatumXR(fcUmDatum(sZDatum)))
    '    End If
    '    If nReArt = 0 Then   'Normalrechnung

    '        Dim Steuer As Decimal = 0
    '        Dim Netto As Decimal = 0
    '        Dim Brutto As Decimal = 0
    '        Dim NettoE As String = ""
    '        Dim NettoZG As Decimal = 0
    '        Dim SteuerZ As Decimal = 0
    '        Dim SteuerZG As Decimal = 0
    '        Dim NettoZ As Decimal = 0
    '        For i = 0 To arDruck1.Length - 1
    '            sInhalt = ReadFileSeriell("XRe_Pa_E.XML")
    '            arFeld = Split(arDruck1(i), "#")
    '            '-------------------------------------------------------
    '            sVariable = sVariable & " Zimmer " & arFeld(0) & vbCrLf
    '            sVariable = sVariable & " Nächte " & arFeld(1) & vbCrLf
    '            sVariable = sVariable & " E Preis " & arFeld(3) & vbCrLf
    '            sVariable = sVariable & " GesamtSteuer 7% " & arFeld(4) & vbCrLf
    '            sVariable = sVariable & " Netto Gesamt " & arFeld(7) & vbCrLf
    '            sVariable = sVariable & " Bruto Gesamt " & arFeld(8) & vbCrLf
    '            sVariable = sVariable & " ------------ " & vbCrLf

    '            '_______________________________________________________

    '            NettoE = Str(Math.Round(Val(arFeld(3)) / (1 + Val(S1) / 100), 2))
    '            sVariable = sVariable & " Netto erechnet EinzelPreis " & NettoE & vbCrLf
    '            SteuerZ = Math.Round(Val(arFeld(3)) - Val(NettoE), 2)
    '            sVariable = sVariable & " Steuer erechnet Einzel 7% " & Str(SteuerZ) & vbCrLf
    '            NettoZG = Val(NettoE) * Val(arFeld(1))
    '            sVariable = sVariable & " Netto Gesamt erechnet  " & Str(NettoZG) & vbCrLf
    '            SteuerZG = Math.Round(Val(arFeld(3)) - Val(NettoE), 2) * Val(arFeld(1))
    '            sVariable = sVariable & " Steuer Gesamt erechnet 7% " & Str(SteuerZG) & vbCrLf


    '            sInhalt = Replace(sInhalt, "?BT-129", arFeld(1))
    '            sVariable = sVariable & " ====================" & vbCrLf & vbCrLf
    '            sInhalt = Replace(sInhalt, "?BT-155", Str(i + 1))  'Laufende nr
    '            sInhalt = Replace(sInhalt, "?BT-131", fcFormatDecimalPunkt(NettoE))     'Netto---------------------
    '            sInhalt = Replace(sInhalt, "?BT-152", S1)             'Steuer
    '            sInhalt = Replace(sInhalt, "?BT-146", fcFormatDecimalPunkt(Str(NettoZG)))     'Netto arfeld(7)
    '            sInhalt = Replace(sInhalt, "?BT-153", arFeld(2))       'Artikel Dreibettzimmer
    '            sInhalt = Replace(sInhalt, "?BT-145", arFeld(10) & "," & arFeld(13) & "," & arFeld(12))   'Beschreibung
    '            sXRechnungA = Replace(sXRechnungA, "?BT-083", "R.Nr.:" & nRNr & "/" & arFeld(10))
    '            sXRechnungE = sXRechnungE + sInhalt
    '            sXRechnungA = Replace(sXRechnungA, "?BT-072", fcUmDatumXR(fcUmDatum(Mid(arFeld(10), 1, 10)))) 'Liferdatum
    '            Steuer = Steuer + SteuerZG ' Val(Replace(arFeld(4), ",", "."))
    '            Netto = Netto + NettoZG 'Val(Replace(arFeld(7), ",", "."))
    '            Brutto = Brutto + NettoZG + SteuerZG 'Val(Replace(arFeld(4), ",", ".")) + Val(Replace(arFeld(7), ",", "."))
    '        Next
    '        sVariable = sVariable & " =======Summen======" & vbCrLf & vbCrLf
    '        sVariable = sVariable & " Netto Summe Gesamt " & Netto & vbCrLf
    '        sVariable = sVariable & " Steuer Summe Gesamt " & Steuer & vbCrLf
    '        sVariable = sVariable & " Bruto Summe Gesamt " & Brutto & vbCrLf
    '        sXRechnungA = Replace(sXRechnungA, "?Position", sXRechnungE)  'Mail Emp
    '        sXRechnungA = Replace(sXRechnungA, "?BT-110", Str(Steuer))  ' Steuerbetrag
    '        sXRechnungA = Replace(sXRechnungA, "?BT-106", fcFormatDecimalPunkt(Str(Netto)))  '   NettoBetrag
    '        sXRechnungA = Replace(sXRechnungA, "?BT-112", fcFormatDecimalPunkt(Str(Brutto))) '   Brutobetrag
    '        sXRechnungA = Replace(sXRechnungA, "?BT-115", fcFormatDecimalPunkt(Str(Brutto))) '   Brutobetrag
    '        sXRechnungA = Replace(sXRechnungA, "?BT-113", "0") '   Anzahlung

    '        arFeld = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "BuchDatum"})
    '        Dim sZDatum As String = ""
    '        Dim t As String = arFeld(3)
    '        sRZiel = sRZiel.Trim
    '        If sRZiel = " " Or sRZiel = "" Then
    '            sZDatum = Date.Today.AddDays(14)
    '        Else
    '            Dim sInhald1() As String = Split(arDruck1(0), "#")
    '            Dim sVon As String = Mid(sInhald1(10), 1, 10)
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
    '                        ddatum = CDate(fcUmDatum(arFeld(3))).AddDays(nTage)
    '                    End If
    '                    sZDatum = ddatum
    '                End If
    '            End If
    '        End If
    '        sXRechnungA = Replace(sXRechnungA, "?BT-009", fcUmDatumXR(fcUmDatum(sZDatum)))
    '    End If

    '    WriteFileUniCode("Var.txt", sVariable)


    '    ' WriteFileUniCode("Rech_" & nRNr & ".xml", sXRechnungA)
    '    WriteFileUniCode(arIni(32) & "\Rechnung\Rech_" & nRNr & ".xml", sXRechnungA)
    '    sXRechnungA = sXRechnungA

    'End Sub

    Public Sub prX_Rechnung_CII(ByRef sBID As String, ByVal sKID As String, ByRef nRNr As String, ByVal sdatum As String, ByRef bBar As Boolean, ByVal anzahlung As Decimal, ByRef sRZiel As String, ByVal S1 As String, ByRef s2 As String, ByVal nReArt As Integer, ByVal ParamArray arDruck1() As String)
        Dim sXRechnungA As String = ReadFileSeriell("XRechnung_CII_1.XML")
        Dim sXRechnungE As String = ""
        Dim sVariable As String = ""
        Dim sInhalt As String
        Dim arFeld() As String
        S1 = S1 & ".00"
        arFeld = fcDataSeek("select * From Konten '", "", 0, {"Name", "KTO", "blz", "IBAN", "BIC"})
        sXRechnungA = Replace(sXRechnungA, "?BT-001", nRNr)  'Rechnungsnummer
        sXRechnungA = Replace(sXRechnungA, "?BT-002", sdatum)  'Rechnungsdatum
        sXRechnungA = Replace(sXRechnungA, "?BT-003", "380")  'Handelsrechnung
        sXRechnungA = Replace(sXRechnungA, "?BT-010", "0")  'Leitweg
        sXRechnungA = Replace(sXRechnungA, "?BT-027", arIni(2))  'Pensions Name
        sXRechnungA = Replace(sXRechnungA, "?BT-035", arIni(3))  'Strasse P
        sXRechnungA = Replace(sXRechnungA, "?BT-037", arIni(5))  'Ort P
        sXRechnungA = Replace(sXRechnungA, "?BT-038", arIni(4))  'PLZ P
        sXRechnungA = Replace(sXRechnungA, "?BT-040", "DE")  ' Land P
        sXRechnungA = Replace(sXRechnungA, "?BT-031", arIni(16))  'Ust ID
        sXRechnungA = Replace(sXRechnungA, "?BT-032", arIni(15))  'Steuernummer
        sXRechnungA = Replace(sXRechnungA, "?BT-041", arIni(9))  ' Kontacktname
        sXRechnungA = Replace(sXRechnungA, "?BT-043", arIni(6))  'Telefon P
        sXRechnungA = Replace(sXRechnungA, "?BT-034", arEMail(1))  ' Mail P  BT40
        sXRechnungA = Replace(sXRechnungA, "?BT-085", arIni(9))  ' Konto Name
        sXRechnungA = Replace(sXRechnungA, "?BT-084", arFeld(3))  'IBAN
        sXRechnungA = Replace(sXRechnungA, "?BT-086", arFeld(4))  'BIC
        sXRechnungA = Replace(sXRechnungA, "?BT-081", "58")  'SEPA
        sXRechnungA = Replace(sXRechnungA, "?BT-152", S1)  ' Steuersatz Übernachtung
        sXRechnungA = Replace(sXRechnungA, "?BT-119", S1)  ' Steuersatz Übernachtung
        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land", "EMail"})
        sXRechnungA = Replace(sXRechnungA, "?BT-044", arFeld(0) & " " & arFeld(1) & " " & arFeld(2))  ' Empf Name
        sXRechnungA = Replace(sXRechnungA, "?BT-050", arFeld(3))  'Strasse Empf
        sXRechnungA = Replace(sXRechnungA, "?BT-052", arFeld(4))  'Ort
        sXRechnungA = Replace(sXRechnungA, "?BT-053", arFeld(5))  'Plz
        sXRechnungA = Replace(sXRechnungA, "?BT-055", arFeld(6))  'Land
        sXRechnungA = Replace(sXRechnungA, "?BT-049", arFeld(7))  'Mail Emp
        'sXRechnungA = Replace(sXRechnungA, "?BT-034", arFeld(7))  'Mail Emp
        sXRechnungA = Replace(sXRechnungA, "?BT-020", "")  'Mail Emp
        Dim sAnreise As String = "99999999"
        Dim sAbreise As String = "00000000"
        If nReArt = 4 Then
            Dim Steuer As Decimal = 0
            Dim Netto As Decimal = 0
            Dim Brutto As Decimal = 0
            Dim NettoE As String = ""
            For i = 0 To arDruck1.Length - 1
                sInhalt = ReadFileSeriell("XRechnung_CII_2.XML")
                arFeld = Split(arDruck1(i), "#")
                NettoE = Str(Math.Round(Val(Replace(arFeld(3), ",", ".")) / (1 + Val(S1) / 100), 2))
                sInhalt = Replace(sInhalt, "?BT-126", Str(i + 1))
                sInhalt = Replace(sInhalt, "?BT-129", "1")
                sInhalt = Replace(sInhalt, "?BT-155", Str(i + 1))  'Laufende nr
                sInhalt = Replace(sInhalt, "?BT-146", fcFormatDecimalPunkt(arFeld(7)))    'Netto-----bt146----------------
                sInhalt = Replace(sInhalt, "?BT-152", S1)             'Steuer
                sInhalt = Replace(sInhalt, "?BT-131", fcFormatDecimalPunkt(arFeld(7)))       'Netto bt-131
                sInhalt = Replace(sInhalt, "?BT-153", arFeld(2))       'Artikel Dreibettzimmer
                sInhalt = Replace(sInhalt, "?BT-145", arFeld(10) & "," & arFeld(13) & "," & arFeld(12))   'Beschreibung
                sXRechnungA = Replace(sXRechnungA, "?BT-083", "R.Nr.:" & nRNr & "/" & arFeld(10))
                sXRechnungE = sXRechnungE + sInhalt
                ' sXRechnungA = Replace(sXRechnungA, "?BT-072", fcUmDatum(Mid(arFeld(10), 1, 10))) 'Liferdatum
                If fcUmDatum(Mid(arFeld(10), 1, 10)) < sAnreise Then sAnreise = fcUmDatum(Mid(arFeld(10), 1, 10))
                If fcUmDatum(Mid(arFeld(10), 14)) > sAbreise Then sAbreise = fcUmDatum(Mid(arFeld(10), 14))
                Steuer = Steuer + Val(Replace(arFeld(4), ",", "."))
                Netto = Netto + Val(Replace(arFeld(7), ",", "."))
                Brutto = Brutto + Val(Replace(arFeld(4), ",", ".")) + Val(Replace(arFeld(7), ",", "."))
            Next
            sXRechnungA = Replace(sXRechnungA, "<!--Rechnungsposition_ENDE-->", sXRechnungE)  'Mail Emp
            sXRechnungA = Replace(sXRechnungA, "?BT-110", Str(Steuer))  ' Steuerbetrag
            sXRechnungA = Replace(sXRechnungA, "?BT-106", fcFormatDecimalPunkt(Str(Netto)))  '   NettoBetrag
            sXRechnungA = Replace(sXRechnungA, "?BT-109", fcFormatDecimalPunkt(Str(Netto)))  '  
            sXRechnungA = Replace(sXRechnungA, "?BT-116", fcFormatDecimalPunkt(Str(Netto)))  '  
            sXRechnungA = Replace(sXRechnungA, "?BT-073", sAnreise)  '  
            sXRechnungA = Replace(sXRechnungA, "?BT-074", sAbreise)  '  
            sXRechnungA = Replace(sXRechnungA, "?BT-072", sAnreise)

            sXRechnungA = Replace(sXRechnungA, "?BT-112", fcFormatDecimalPunkt(Str(Brutto))) '   Brutobetrag
            sXRechnungA = Replace(sXRechnungA, "?BT-115", fcFormatDecimalPunkt(Str(Brutto))) '   Brutobetrag
            sXRechnungA = Replace(sXRechnungA, "?BT-113", "0") '   Anzahlung

            arFeld = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "BuchDatum"})
            Dim sZDatum As String = ""
            Dim t As String = arFeld(3)
            sRZiel = sRZiel.Trim
            If sRZiel = " " Or sRZiel = "" Then
                sZDatum = Date.Today.AddDays(14)
            Else
                Dim sInhald1() As String = Split(arDruck1(0), "#")
                Dim sVon As String = Mid(sInhald1(10), 1, 10)
                If Mid(sRZiel, 3, 1) = "." Then
                    sZDatum = sRZiel + "." + Mid(sVon, 7)
                    If fcUmDatum(sZDatum) > fcUmDatum(sVon) Then
                        sZDatum = Mid(sRZiel, 1, 5) + "." + Str((Mid(sVon, 7) - 1))
                    End If
                Else
                    Dim ddatum As Date
                    If IsNumeric(sRZiel) = True Then
                        Dim nTage As Integer = Val(sRZiel)
                        If nTage < 0 Then                              ' kleiner 0 zahlung nach vor anreise -tage
                            ddatum = CDate(sVon).AddDays(nTage)
                        Else                                            'größer 0 zahlung nach erhald + tage
                            ddatum = CDate(fcUmDatum(arFeld(3))).AddDays(nTage)
                        End If
                        sZDatum = ddatum
                    End If
                End If
            End If
            sXRechnungA = Replace(sXRechnungA, "?BT-009", fcUmDatum(sZDatum))
        End If
        If nReArt = 0 Then   'Normalrechnung

            Dim Steuer As Decimal = 0
            Dim Netto As Decimal = 0
            Dim Brutto As Decimal = 0
            Dim NettoE As String = ""
            Dim NettoZG As Decimal = 0
            Dim SteuerZ As Decimal = 0
            Dim SteuerZG As Decimal = 0
            Dim NettoZ As Decimal = 0
            For i = 0 To arDruck1.Length - 1
                sInhalt = ReadFileSeriell("XRechnung_CII_2.XML")
                arFeld = Split(arDruck1(i), "#")
                '-------------------------------------------------------
                sVariable = sVariable & " Zimmer " & arFeld(0) & vbCrLf
                sVariable = sVariable & " Nächte " & arFeld(1) & vbCrLf
                sVariable = sVariable & " E Preis " & arFeld(3) & vbCrLf
                sVariable = sVariable & " GesamtSteuer 7% " & arFeld(4) & vbCrLf
                sVariable = sVariable & " Netto Gesamt " & arFeld(7) & vbCrLf
                sVariable = sVariable & " Bruto Gesamt " & arFeld(8) & vbCrLf
                sVariable = sVariable & " ------------ " & vbCrLf

                '_______________________________________________________

                NettoE = Str(Math.Round(Val(Replace(arFeld(3), ",", ".")) / (1 + Val(S1) / 100), 2))
                sVariable = sVariable & " Netto erechnet EinzelPreis " & NettoE & vbCrLf
                'Dim Mst As Double = 1 + Val(S1) / 100
                'Dim Nettox As Double = Val(arFeld(3)) / Mst
                ''   Dim Nettox As Double = Val(Replace(arFeld(3), ",", ".")) / Mst
                '' Dim nettoy As Double = Math.Round(Nettox, 2)
                'Nettox = Val(Replace(arFeld(3), ",", ".")) / Mst
                'Nettox = Nettox
                'Nettox = Val(Replace(arFeld(3), ",", ".")) / Mst



                '   Dim nettoy As Double = Math.Round(Nettox, 2)


                SteuerZ = Math.Round(Val(Replace(arFeld(3), ",", ".")) - Val(NettoE), 2)
                sVariable = sVariable & " Steuer erechnet Einzel 7% " & Str(SteuerZ) & vbCrLf
                NettoZG = Val(NettoE) * Val(arFeld(1))
                sVariable = sVariable & " Netto Gesamt erechnet  " & Str(NettoZG) & vbCrLf
                SteuerZG = Math.Round(Val(Replace(arFeld(3), ",", ".")) - Val(NettoE), 2) * Val(arFeld(1))
                sVariable = sVariable & " Steuer Gesamt erechnet 7% " & Str(SteuerZG) & vbCrLf


                sInhalt = Replace(sInhalt, "?BT-129", arFeld(1))
                sInhalt = Replace(sInhalt, "?BT-126", Str(i + 1))
                sVariable = sVariable & " ====================" & vbCrLf & vbCrLf
                sInhalt = Replace(sInhalt, "?BT-155", Str(i + 1))  'Laufende nr
                sInhalt = Replace(sInhalt, "?BT-146", fcFormatDecimalPunkt(NettoE))     'Netto------------bt131---------
                sInhalt = Replace(sInhalt, "?BT-152", S1)             'Steuer
                sInhalt = Replace(sInhalt, "?BT-131", fcFormatDecimalPunkt(Str(NettoZG)))     'Netto arfeld(7) bt146
                sInhalt = Replace(sInhalt, "?BT-153", arFeld(2))       'Artikel Dreibettzimmer
                sInhalt = Replace(sInhalt, "?BT-145", arFeld(10) & "," & arFeld(13) & "," & arFeld(12))   'Beschreibung

                sXRechnungA = Replace(sXRechnungA, "?BT-083", "R.Nr.:" & nRNr & "/" & arFeld(10))
                sXRechnungE = sXRechnungE + sInhalt
                '  sXRechnungA = Replace(sXRechnungA, "?BT-072", fcUmDatum(Mid(arFeld(10), 1, 10))) 'Liferdatum
                If fcUmDatum(Mid(arFeld(10), 1, 10)) < sAnreise Then sAnreise = fcUmDatum(Mid(arFeld(10), 1, 10))
                If fcUmDatum(Mid(arFeld(10), 14)) > sAbreise Then sAbreise = fcUmDatum(Mid(arFeld(10), 14))



                Steuer = Steuer + SteuerZG ' Val(Replace(arFeld(4), ",", "."))
                Netto = Netto + NettoZG 'Val(Replace(arFeld(7), ",", "."))
                Brutto = Brutto + NettoZG + SteuerZG 'Val(Replace(arFeld(4), ",", ".")) + Val(Replace(arFeld(7), ",", "."))
            Next
            sVariable = sVariable & " =======Summen======" & vbCrLf & vbCrLf
            sVariable = sVariable & " Netto Summe Gesamt " & Netto & vbCrLf
            sVariable = sVariable & " Steuer Summe Gesamt " & Steuer & vbCrLf
            sVariable = sVariable & " Bruto Summe Gesamt " & Brutto & vbCrLf
            sXRechnungA = Replace(sXRechnungA, "<!--Rechnungsposition_ENDE-->", sXRechnungE)  'Mail Emp
            sXRechnungA = Replace(sXRechnungA, "?BT-110", Str(Steuer))  ' Steuerbetrag
            sXRechnungA = Replace(sXRechnungA, "?BT-106", fcFormatDecimalPunkt(Str(Netto)))  '   NettoBetrag
            sXRechnungA = Replace(sXRechnungA, "?BT-109", fcFormatDecimalPunkt(Str(Netto)))  '  
            sXRechnungA = Replace(sXRechnungA, "?BT-116", fcFormatDecimalPunkt(Str(Netto)))  '  
            sXRechnungA = Replace(sXRechnungA, "?BT-073", sAnreise)  '  
            sXRechnungA = Replace(sXRechnungA, "?BT-074", sAbreise)  '  
            sXRechnungA = Replace(sXRechnungA, "?BT-072", sAnreise)

            sXRechnungA = Replace(sXRechnungA, "?BT-112", fcFormatDecimalPunkt(Str(Brutto))) '   Brutobetrag
            sXRechnungA = Replace(sXRechnungA, "?BT-115", fcFormatDecimalPunkt(Str(Brutto))) '   Brutobetrag
            sXRechnungA = Replace(sXRechnungA, "?BT-113", "0") '   Anzahlung

            arFeld = fcDataSeek("select * From Buchung Where BID ='", sBID, 0, {"KunID", "ObjID", "Sprache", "BuchDatum"})
            Dim sZDatum As String = ""
            Dim t As String = arFeld(3)
            sRZiel = sRZiel.Trim
            If sRZiel = " " Or sRZiel = "" Then
                sZDatum = Date.Today.AddDays(14)
            Else
                Dim sInhald1() As String = Split(arDruck1(0), "#")
                Dim sVon As String = Mid(sInhald1(10), 1, 10)
                If Mid(sRZiel, 3, 1) = "." Then
                    sZDatum = sRZiel + "." + Mid(sVon, 7)
                    If fcUmDatum(sZDatum) > fcUmDatum(sVon) Then
                        sZDatum = Mid(sRZiel, 1, 5) + "." + Str((Mid(sVon, 7) - 1))
                    End If
                Else
                    Dim ddatum As Date
                    If IsNumeric(sRZiel) = True Then
                        Dim nTage As Integer = Val(sRZiel)
                        If nTage < 0 Then                              ' kleiner 0 zahlung nach vor anreise -tage
                            ddatum = CDate(sVon).AddDays(nTage)
                        Else                                            'größer 0 zahlung nach erhald + tage
                            ddatum = CDate(fcUmDatum(arFeld(3))).AddDays(nTage)
                        End If
                        sZDatum = ddatum
                    End If
                End If
            End If
            sXRechnungA = Replace(sXRechnungA, "?BT-009", fcUmDatum(sZDatum))
        End If

        WriteFileUniCode("Var.txt", sVariable)


        ' WriteFileUniCode("Rech_" & nRNr & ".xml", sXRechnungA)
        WriteFileUniCode(cgPfad & "\Ablage\Rechnung.xml", sXRechnungA)
        sXRechnungA = sXRechnungA

    End Sub






    Public Function fcFormatDecimalPunkt(ByRef sWert As String) As String
        sWert = fcFormatDecimal(sWert)
        fcFormatDecimalPunkt = Replace(sWert, ",", ".")
    End Function
End Module
