Public Class VPE

    Private Shared print As New IDEALSoftware.VpeCommunity.VpeControl
    Private Shared arTableValue(1, 1, 6) As String   '0=text 1=schriftgröße 2=schriftart 3=abstand vom cellrand,4=colorfrond, 5=colorback, 6=abstand von oben
    Private Shared arTableDim(4) As Integer
    Private Shared arTableColor(1, 1, 1) As Object
    Private Shared arTableColumns(1) As Double
    Private Shared arTableRows(1) As Double
    Private Shared cDefautColor As Color = Color.Black
    ''' <summary>
    ''' Tabellen größe festlegen(Außenmasse in mm)
    ''' </summary>
    ''' <param name="nLeft"></param>
    ''' <param name="nTop"></param>
    ''' <param name="nWidth"></param>
    ''' <param name="nHeigth"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TableDim(ByRef nLeft As Integer, ByRef nTop As Integer, ByRef nWidth As Integer, ByRef nHeigth As Integer)
        arTableDim(0) = nTop
        arTableDim(1) = nLeft
        arTableDim(2) = nHeigth
        arTableDim(3) = nWidth
    End Sub

    ''' <summary>
    ''' Giebt Y2 kordinate einer zelle Zurück( in mm)
    ''' </summary>
    ''' <param name="nRow"></param>
    ''' <remarks>
    ''' 13.01.2014 Create
    ''' </remarks>
    Public Shared Function GetTabeleCellBottom(ByRef nRow As Integer) As Double
        GetTabeleCellBottom = arTableDim(0)
        For i_ro = 0 To nRow
            GetTabeleCellBottom = GetTabeleCellBottom + Math.Abs(arTableRows(i_ro))
        Next
    End Function

    ''' <summary>
    ''' Giebt Y1 kordinate einer zelle Zurück( in mm)
    ''' </summary>
    ''' <param name="nRow"></param>
    ''' <remarks>
    ''' 13.01.2014 Create
    ''' </remarks>
    Public Shared Function GetTabeleCellTop(ByRef nRow As Integer) As Double
        GetTabeleCellTop = arTableDim(0)
        For i_ro = 0 To nRow - 1
            GetTabeleCellTop = GetTabeleCellTop + Math.Abs(arTableRows(i_ro))
        Next
    End Function

    ''' <summary>
    '''Giebt X1 kordinate einer zelle Zurück( in mm)
    ''' </summary>
    ''' <param name="nCol"></param>
    ''' <remarks>
    ''' 13.01.2014 Create
    ''' </remarks>
    Public Shared Function GetTabeleCellLeft(ByRef nCol As Integer) As Double
        GetTabeleCellLeft = arTableDim(1)
        For i_ro = 0 To nCol - 1
            GetTabeleCellLeft = GetTabeleCellLeft + Math.Abs(arTableColumns(i_ro))
        Next
    End Function

    ''' <summary>
    ''' Giebt X2 kordinate einer zelle Zurück( in mm)
    ''' </summary>
    ''' <param name="nCol"></param>
    ''' <remarks>
    ''' 13.01.2014 Create
    ''' </remarks>
    Public Shared Function GetTabeleCellRight(ByRef nCol As Integer) As Double
        GetTabeleCellRight = arTableDim(1)
        For i_ro = 0 To nCol
            GetTabeleCellRight = GetTabeleCellRight + Math.Abs(arTableColumns(i_ro))
        Next
    End Function

    ''' <summary>
    ''' Anzahl der Zeilen
    ''' </summary>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TableRows(ByRef nZahl As Integer)
        Dim ro As Double
        'zahl = zahl - arTableRows.Length
        ReDim arTableRows(nZahl - 1)
        ReDim arTableValue(arTableColumns.Count - 1, arTableRows.Count - 1, 6)
        ReDim arTableColor(arTableColumns.Count - 1, arTableRows.Count - 1, 1)
        ro = arTableDim(2) / nZahl
        For i_ro = 0 To arTableRows.Count - 1
            arTableRows(i_ro) = ro
        Next
        For i_co = 0 To arTableColumns.Count - 1    'standarteinstellung
            For i_ro = 0 To arTableRows.Count - 1
                arTableValue(i_co, i_ro, 0) = " "
                arTableValue(i_co, i_ro, 1) = "12"
                arTableValue(i_co, i_ro, 2) = "Times New Roman"
                arTableValue(i_co, i_ro, 3) = "2"
                arTableValue(i_co, i_ro, 6) = "2"
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
    Public Shared Sub TableRowsHeigth(ByRef nNr As Integer, ByRef nZahl As Integer)
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
    ''' Anzahl der Spalten
    ''' </summary>
    ''' <param name="nZahl"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TableColumns(ByRef nZahl As Integer)
        Dim co As Double
        ' zahl = zahl - arTableColumns.Length

        ReDim arTableColumns(nZahl - 1)
        ReDim arTableValue(arTableColumns.Count - 1, arTableRows.Count - 1, 6)
        ReDim arTableColor(arTableColumns.Count - 1, arTableRows.Count - 1, 1)
        co = arTableDim(3) / nZahl
        For i_co = 0 To arTableColumns.Count - 1
            arTableColumns(i_co) = co
        Next
        For i_co = 0 To arTableColumns.Count - 1    'standarteinstellung
            For i_ro = 0 To arTableRows.Count - 1
                arTableValue(i_co, i_ro, 0) = " "
                arTableValue(i_co, i_ro, 1) = "12"
                arTableValue(i_co, i_ro, 2) = "Times New Roman"
                arTableValue(i_co, i_ro, 3) = "2"
                arTableValue(i_co, i_ro, 6) = "2"
                arTableColor(i_co, i_ro, 0) = Color.Black
                arTableColor(i_co, i_ro, 1) = Color.White
            Next
        Next
    End Sub

    ''' <summary>
    ''' Spaltenbreite festlegen
    ''' </summary>
    ''' <param name="nNr"></param>
    ''' <param name="nZahl"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TableColumnsWidth(ByRef nNr As Integer, ByRef nZahl As Integer)
        Dim co As Double
        Dim wi As Double
        Dim anz As Integer = 0
        'ReDim arTableColumns(zahl - 1)
        arTableColumns(nNr) = nZahl * -1
        wi = arTableDim(3)
        For i_co = 0 To arTableColumns.Count - 1
            If arTableColumns(i_co) < 0 Then
                wi = wi + arTableColumns(i_co)
            Else
                anz = anz + 1
            End If
        Next
        co = wi / anz
        For i_co = 0 To arTableColumns.Count - 1
            If arTableColumns(i_co) >= 0 Then
                arTableColumns(i_co) = co
            End If
        Next
    End Sub

    ''' <summary>
    ''' Schriftgröße und type festlegen für ein feld festlegen, Standart 12 Dpi und Times New Roman
    ''' </summary>
    ''' <param name="nCol"></param>
    ''' <param name="nRow"></param>
    ''' <param name="cFont"></param>
    ''' <param name="nDpi"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TabelCellSelectFont(ByRef nCol As Integer, ByRef nRow As Integer, ByRef cFont As String, ByRef nDpi As Integer)
        arTableValue(nCol, nRow, 1) = Str(nDpi)
        arTableValue(nCol, nRow, 2) = cFont
    End Sub

    ''' <summary>
    ''' Textabstand vom linken Zellrand Standart 2
    ''' </summary>
    ''' <param name="nCol"></param>
    ''' <param name="nRow"></param>
    ''' <param name="nlaenge"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TabelCellLeft(ByRef nCol As Integer, ByRef nRow As Integer, ByRef nlaenge As Integer)
        arTableValue(nCol, nRow, 3) = Str(nlaenge)
    End Sub

    ''' <summary>
    ''' Textabstand vom Oben Zellrand Standart 2
    ''' </summary>
    ''' <param name="nCol"></param>
    ''' <param name="nRow"></param>
    ''' <param name="nlaenge"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TabelCellTop(ByRef nCol As Integer, ByRef nRow As Integer, ByRef nlaenge As Integer)
        arTableValue(nCol, nRow, 6) = Str(nlaenge)
    End Sub


    ''' <summary>
    '''  text in ein Tabellenfeld schreiben
    ''' </summary>
    ''' <param name="nCol"></param>
    ''' <param name="nRow"></param>
    ''' <param name="cText"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TabelCellText(ByRef nCol As Integer, ByRef nRow As Integer, ByRef cText As String)
        If cText = "" Then cText = " "
        arTableValue(nCol, nRow, 0) = cText
    End Sub

    ''' <summary>
    ''' Schriftfarbe in ein Tabellenfeld schreiben Standart Schwarz
    ''' </summary>
    ''' <param name="nCol"></param>
    ''' <param name="nRow"></param>
    ''' <param name="oColor"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TabelCellForeColor(ByRef nCol As Integer, ByRef nRow As Integer, ByRef oColor As Color)
        If oColor = Color.Empty Then oColor = Color.Black
        arTableColor(nCol, nRow, 0) = oColor
    End Sub

    ''' <summary>
    ''' Hintergrundfarbe tabellenfeld schreiben Standart Weiß
    ''' </summary>
    ''' <param name="nCol"></param>
    ''' <param name="nRow"></param>
    ''' <param name="oColor"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TabelCellBackColor(ByRef nCol As Integer, ByRef nRow As Integer, ByRef oColor As Color)
        If oColor = Color.Empty Then oColor = Color.White
        arTableColor(nCol, nRow, 1) = oColor
    End Sub

    ''' <summary>
    ''' Tabelle  zeichnen und textausgabe
    ''' </summary>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TabelWrite()
        Dim nZeile As Integer
        Dim nSpalte As Integer
        Dim top As Double = arTableDim(0) / 10
        Dim left As Double = arTableDim(1) / 10
        Dim heigth As Double = arTableDim(2) / 10
        Dim width As Double = arTableDim(3) / 10
        Dim betrag As Double = 0
        Dim betrag1 As Double = 0
        Dim betrag_new As Double = 0
        Dim betrag1_new As Double = 0
        Dim hoehe As Double = 0
        Dim text As String
        betrag = left
        betrag1 = top
        For i_co = 0 To arTableColumns.Count - 1
            betrag_new = Math.Abs(arTableColumns(i_co)) / 10
            For i_ro = 0 To arTableRows.Count - 1
                betrag1_new = Math.Abs(arTableRows(i_ro)) / 10
                If arTableColor(i_co, i_ro, 1) <> Color.White Then
                    print.PenColor = arTableColor(i_co, i_ro, 1)
                    For x = betrag1 To betrag1 + betrag1_new Step 0.02
                        print.Line(betrag, x, betrag_new + betrag, x)

                    Next
                    print.PenColor = Color.Black

                End If
                betrag1 = betrag1 + betrag1_new
            Next
            betrag1 = top
            betrag = betrag + betrag_new
        Next
        ' print.PenColor = Color.Azure    linienfarbe
        print.Line(left, top, left + width, top) 'horizontal oben
        print.Line(left, top, left, top + heigth) 'vertikal rechts
        print.Line(left + width, top, left + width, top + heigth) 'vertikal links
        print.Line(left, top + heigth, left + width, top + heigth) 'horizontal unten
        For i_ro = 0 To arTableRows.Count - 1
            betrag = Math.Abs(arTableRows(i_ro)) / 10
            'print.Line(top, arTableRows(i) / 10 + left, heigth + top,arTableRows(i) / 10 + left)
            print.Line(left, betrag + top, left + width, betrag + top)

            top = top + betrag
            arTableRows(i_ro) = (top - (betrag / 2)) * 10  'Position cell mitte
        Next
        top = arTableDim(0) / 10
        left = arTableDim(1) / 10
        ' print.Line(3, 2, 7, 8)
        For i_co = 0 To arTableColumns.Count - 1
            betrag = Math.Abs(arTableColumns(i_co)) / 10
            ' print.Line(top, arTableColumns(i) / 10 + left, width + left, arTableColumns(i) / 10 + top)
            print.Line(betrag + left, top, betrag + left, heigth + top)
            left = left + betrag
            arTableColumns(i_co) = (left - betrag) * 10 'Position linie lings
        Next
        For nZeile = 0 To arTableRows.Count - 1
            For nSpalte = 0 To arTableColumns.Count - 1
                print.SelectFont(arTableValue(nSpalte, nZeile, 2), Val(arTableValue(nSpalte, nZeile, 1)))
                hoehe = Val(arTableValue(nSpalte, nZeile, 1)) * 0.035277778 / 2             'Schrifthöhe in cm
                print.TextColor = arTableColor(nSpalte, nZeile, 0)
                text = arTableValue(nSpalte, nZeile, 0)
                print.Print((arTableColumns(nSpalte) + Val(arTableValue(nSpalte, nZeile, 3))) / 10, arTableRows(nZeile) / 10 - hoehe, text)
            Next
        Next
    End Sub

    ''' <summary>
    ''' Seitenvorschau
    ''' </summary>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub Veiw()
        print.Preview()
    End Sub

    ''' <summary>
    ''' Dokoment Öffen
    ''' </summary>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub Open()
        print.OpenDoc()
        'print.PageFormat = IDEALSoftware.VpeCommunity.PageFormat.A4Rotated
    End Sub
    Public Shared Sub Close()
        print.CloseDoc()
    End Sub

    ''' <summary>
    ''' Neue Seite
    ''' </summary>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub NewPage()
        print.PageBreak()
    End Sub

    ''' <summary>
    ''' Zeichenkette auf Seite Schreiben
    ''' </summary>
    ''' <param name="nLeft "></param>
    ''' <param name="nTop"></param>
    ''' <param name="cText"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TextDruck(ByRef nLeft As Integer, ByRef nTop As Integer, ByVal cText As String)
        If cText = "" Then cText = " "
        print.Print(nLeft / 10, nTop / 10, cText)
    End Sub

    ' ''' <summary>
    ' ''' Text formatiren und auf Seite Schreiben
    ' ''' </summary>
    ' ''' <param name="nLeft"></param>
    ' ''' <param name="ntop"></param>
    ' ''' <param name="nZeichen"></param>
    ' ''' <param name="sText"></param>
    ' ''' <remarks>
    ' ''' 13.01.2012 Create
    ' ''' </remarks>
    'Public Shared Function TextDruck(ByRef nLeft As Integer, ByRef nTop As Integer, ByRef nZeichen As Integer, _
    '                                ByRef sText As String) As Integer
    '    Dim ii As Integer
    '    Dim sText1 As String
    '    Dim sTextOut(2) As String
    '    Dim i1 As Integer = 0
    '    Dim pos1 As Integer = 0
    '    Dim pos2 As Integer = 0
    '    Dim sText2 As String = ""
    '    Dim nTop1 As Integer = 4
    '    Dim nFont As Integer = 10
    '    Dim sLeft As String = ""
    '    Dim sRight As String = ""
    '    Dim l_nLoop, laenge, l_nZähler As Long
    '    ii = 1

    '    For i = 1 To sText.Length
    '        ii = -1
    '        For j = i To nZeichen + i    'suchen nach zeilen ende
    '            If j = sText.Length Then Exit For
    '            If Mid(sText, j, 1) = Chr(10) Then
    '                ii = j
    '                Exit For
    '            End If
    '        Next
    '        If ii = 0 Then Exit For
    '        If ii <> -1 Then
    '            sText1 = Mid(sText, i, ii - i)
    '            pos1 = 0
    '            pos2 = 0
    '            For a = 1 To sText1.Length
    '                If Mid(sText1, a, 1) = "#" And pos1 = 0 Then
    '                    pos1 = a
    '                End If
    '                If Mid(sText1, a, 1) = "#" And pos1 <> 0 Then
    '                    pos2 = a
    '                End If
    '            Next
    '            If pos1 <> 0 Then
    '                sText2 = Mid(sText1, pos1 + 1, pos2 - pos1 - 1)
    '                sText1 = Mid(sText1, 1, pos1 - 1) + Mid(sText1, pos2 + 1)

    '                ' nFont = Str(AtLeft(sText2, ",", 1))
    '                '  Function AtLeft(ByRef Kette As String, ByRef Zeichen As String, ByRef Nummer As Short) As String

    '                sLeft = ""
    '                l_nZähler = 0
    '                laenge = Len(sText2)
    '                Dim LaengeZeichen As Integer = Len(",")
    '                For l_nLoop = 1 To laenge
    '                    If Mid(sText2, l_nLoop, LaengeZeichen) = "," Then
    '                        l_nZähler = l_nZähler + 1

    '                        If l_nZähler = 1 Then
    '                            nFont = Str(Left(sText2, l_nLoop - 1))
    '                            Exit For
    '                        End If
    '                    End If

    '                Next








    '                '  nTop1 = Str(AtRight(sText2, ",", 1)) / 10
    '                ' Function AtRight(ByVal Kette As String, ByVal Zeichen As String, ByVal Nummer As Integer) As String
    '                '  Dim laenge, l_nLoop, l_nZähler As Long
    '                sRight = ""
    '                l_nZähler = 0
    '                laenge = Len(sText2)
    '                LaengeZeichen = Len(",")
    '                For l_nLoop = laenge To 1 Step -1

    '                    If Mid$(sText2, l_nLoop, LaengeZeichen) = "," Then
    '                        l_nZähler = l_nZähler + 1

    '                        If l_nZähler = 1 Then
    '                            nTop1 = Str(Right$(sText2, laenge - l_nLoop)) / 10
    '                            Exit For
    '                        End If
    '                    End If

    '                Next







    '                Call TextFormat("Times New Roman", nFont)
    '            End If
    '            If sText1 <> "" Then
    '                TextDruck(nLeft, nTop, sText1)
    '                nTop = nTop + nTop1
    '            End If
    '            i = ii
    '        Else
    '            sText1 = Mid(sText, i)
    '            pos1 = 0
    '            pos2 = 0
    '            For a = 1 To sText1.Length
    '                If Mid(sText1, a, 1) = "#" And pos1 = 0 Then
    '                    pos1 = a
    '                End If
    '                If Mid(sText1, a, 1) = "#" And pos1 <> 0 Then
    '                    pos2 = a
    '                End If
    '            Next
    '            If pos1 <> 0 Then
    '                sText2 = Mid(sText1, pos1 + 1, pos2 - pos1 - 1)
    '                sText1 = Mid(sText1, 1, pos1 - 1) + Mid(sText1, pos2 + 1)
    '                '   nFont = Str(AtLeft(sText2, ",", 1))
    '                sLeft = ""
    '                l_nZähler = 0
    '                laenge = Len(sText2)
    '                Dim LaengeZeichen As Integer = Len(",")
    '                For l_nLoop = 1 To laenge
    '                    If Mid(sText2, l_nLoop, LaengeZeichen) = "," Then
    '                        l_nZähler = l_nZähler + 1

    '                        If l_nZähler = 1 Then
    '                            nFont = Str(Left(sText2, l_nLoop - 1))
    '                            Exit For
    '                        End If
    '                    End If

    '                Next



    '                '   nTop1 = Str(AtRight(sText2, ",", 1)) / 10
    '                sRight = ""
    '                l_nZähler = 0
    '                laenge = Len(sText2)
    '                LaengeZeichen = Len(",")
    '                For l_nLoop = laenge To 1 Step -1

    '                    If Mid$(sText2, l_nLoop, LaengeZeichen) = "," Then
    '                        l_nZähler = l_nZähler + 1

    '                        If l_nZähler = 1 Then
    '                            nTop1 = Str(Right$(sText2, laenge - l_nLoop)) / 10
    '                            Exit For
    '                        End If
    '                    End If

    '                Next
    '                Call TextFormat("Times New Roman", nFont)
    '            End If
    '            If sText1.Length <= nZeichen Then
    '                If sText1 <> "" Then
    '                    TextDruck(nLeft, nTop, sText1)
    '                    nTop = nTop + nTop1
    '                End If
    '                Exit For
    '            Else
    '                For j = nZeichen + i To i Step -1    'suchen                         nach(lestzten(lerrzeichen))
    '                    If Mid(sText, j, 1) = " " Then
    '                        ii = j
    '                        Exit For
    '                    End If
    '                Next
    '                sText1 = Mid(sText, i, ii - i)
    '                If sText1 <> "" Then
    '                    TextDruck(nLeft, nTop, sText1)
    '                    nTop = nTop + nTop1
    '                End If
    '                i = ii
    '            End If
    '        End If
    '    Next
    '    Return nTop
    'End Function

    Public Shared Sub PageFormat(ByRef sformat As String)
        Select Case sformat
            Case "A4Rotated"
                print.PageFormat = IDEALSoftware.VpeCommunity.PageFormat.A4Rotated
                print.PageOrientation = IDEALSoftware.VpeCommunity.PageOrientation.Landscape
                print.PageHeight = 21
                print.PageWidth = 31
            Case "A4"
                print.PageFormat = IDEALSoftware.VpeCommunity.PageFormat.A4
                print.PageHeight = 30
                print.PageWidth = 21
        End Select
    End Sub

    Public Shared Sub TextFormat(ByRef cFont As String, ByRef nDpi As Integer)
        print.SelectFont(cFont, nDpi)
    End Sub
    Public Shared Sub ZeichenSatz(ByRef cZeichen As String)
        'print.CharSet = VCHARSET_win_CYRILLIC
        print.CharSet = IDEALSoftware.VpeCommunity.CharSet.WinCyrillic
    End Sub

    Public Shared Sub PDF(ByVal Dateiname As String)
        print.WriteDoc(Dateiname)
    End Sub

    ''' <summary>
    ''' Bild in Zelle Schreiben 
    ''' </summary>
    ''' <param name="nCol"></param>
    ''' <param name="nRow"></param>
    ''' <param name="sFiele"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TabeleCellPict(ByRef nCol As Integer, ByRef nRow As Integer, ByRef sFiele As String, Optional ByRef Darstellung As Boolean = False)
        Dim x1 As Double = GetTabeleCellLeft(nCol)
        Dim x2 As Double = GetTabeleCellRight(nCol)
        Dim y1 As Double = GetTabeleCellTop(nRow)
        Dim y2 As Double = GetTabeleCellBottom(nRow)
        Dim yoff As Integer = Val(arTableValue(nCol, nRow, 6))
        Dim xoff As Integer = Val(arTableValue(nCol, nRow, 3))

        print.PictureBestFit = Darstellung

        Pict(x1 + xoff, y1 + yoff, x2 - xoff, y2 - yoff, sFiele, 0)
        print.PictureBestFit = False
    End Sub

    ''' <summary>
    ''' Text in Zelle Schreiben und Formatieren (0 lings,1 Rechts,2 Mitte,3 Block,5 Block AB)
    ''' </summary>
    ''' <param name="nCol"></param>
    ''' <param name="nRow"></param>
    ''' <param name="sText"></param>
    ''' <param name="nFormat"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TabeleCellTextBox(ByRef nCol As Integer, ByRef nRow As Integer, ByRef sText As String, Optional ByRef nFormat As Integer = 0)
        Dim yoff As Integer = Val(arTableValue(nCol, nRow, 6))
        Dim xoff As Integer = Val(arTableValue(nCol, nRow, 3))
        Dim x1 As Double = GetTabeleCellLeft(nCol) + xoff
        Dim x2 As Double = GetTabeleCellRight(nCol) - xoff
        Dim y1 As Double = GetTabeleCellTop(nRow) + yoff
        Dim y2 As Double = GetTabeleCellBottom(nRow) - yoff
        print.TextAlignment = nFormat
        print.Write(x1 / 10, y1 / 10, x2 / 10, y2 / 10, sText)
        print.TextAlignment = 0
    End Sub

    ''' <summary>
    ''' Zahl in StringFormatiren Rechtsbundig in Zelle schreiben
    ''' </summary>
    ''' <param name="nCol"></param>
    ''' <param name="nRow"></param>
    ''' <param name="nZahl"></param>
    ''' <param name="nNk"></param>
    ''' <param name="sTrenn"></param>
    ''' <param name="sWaerung"></param>
    ''' <remarks>
    ''' 13.01.2012 Create
    ''' </remarks>
    Public Shared Sub TabeleCellPay(ByRef nCol As Integer, ByRef nRow As Integer, ByRef nZahl As Double, Optional ByRef nNk As Integer = 2, Optional ByRef sTrenn As String = ".", Optional ByRef sWaerung As String = "")
        Dim yoff As Integer = Val(arTableValue(nCol, nRow, 6))
        Dim xoff As Integer = Val(arTableValue(nCol, nRow, 3))
        Dim x1 As Double = GetTabeleCellLeft(nCol) + xoff
        Dim x2 As Double = GetTabeleCellRight(nCol) - xoff
        Dim y1 As Double = GetTabeleCellTop(nRow) + yoff
        Dim y2 As Double = GetTabeleCellBottom(nRow) - yoff
        Dim sNk As String = StrDup(nNk, "0")
        Dim aText As Array = Split(Str(nZahl), ".")
        Dim sText As String = ""
        Dim nL As Integer = 0
        If aText.Length = 1 Then
            sText = aText(0) & sTrenn & sNk & " " & sWaerung
        Else
            nL = aText(1).ToString.Length
            sText = aText(0) & sTrenn & aText(1) & Mid(sNk, 1, nNk - nL) & " " & sWaerung
        End If
        print.TextAlignment = 1
        print.Write(x1 / 10, y1 / 10, x2 / 10, y2 / 10, sText)
        print.TextAlignment = 0
    End Sub


    Public Shared Sub Pict(ByVal left As Double, ByRef top As Double, ByRef Right As Double, ByRef bottom As Double, ByRef sFiele As String, Optional ByRef Rahmen As Double = 0.01)
        print.PenSize = Rahmen
        print.Picture(left / 10, top / 10, Right / 10, bottom / 10, sFiele)
        print.PenSize = 0.01
    End Sub


    Public Shared Sub Line(ByRef X1 As Double, ByRef Y1 As Double, ByRef X2 As Double, ByVal Y2 As Double, Optional ByRef cPenColor As Color = Nothing)
        If cPenColor = Nothing Then
            print.PenColor = Color.Black
        Else
            print.PenColor = cPenColor
        End If
        print.Line(X1 / 10, Y1 / 10, X2 / 10, Y2 / 10)
    End Sub
End Class

