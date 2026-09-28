Imports System.Text

Public Class frmReservierungDest
    Dim lStart As Boolean = True 'ist True solange der Ladevorgang des Formulars läuft
    Dim arZ(30, 4) As String
    Dim arFZ(30, 1) As String
    Dim arZim(30, 2) As String
    Dim dtK As DataTable        'Kunden
    Dim arOld(24) As String
    Dim arNew(24) As String
    Dim sBID As String 'ID einer einzelnen Reservierung
    Dim lRec As Boolean
    Dim lUnbekannt As Boolean = False
    Dim sDayStart As Date
    Dim sDayEnd As Date
    Dim arZZiel() As String
    Dim sRNr As String
    Dim dtBuchIndex As DataTable
    Dim nIndexIst As Integer = 0
    Dim sIndexIst As String = "0"
    Dim sIDRef As String
    Dim sIDB As String
    Dim sIDR As String ' Buchungssatz ID
    Dim bLoad As Boolean = False
    Dim sCode As String
    Dim arKalender(6, 10) As String
    Dim sLanguage1 As Array = Split(ReadOneValueFromSystemDb("Language"), vbCrLf)
    Dim sLanguage As Array = Split(sLanguage1(0), ";")
    Dim sMwstU As String
    Dim sMwstS As String
    Dim sMwstG As String
    Dim sGKU As String
    Dim sGKS As String
    Dim sGKG As String
    Dim sMwstUa As String
    Dim sMwstSa As String
    Dim sMwstGa As String
    Dim sGKUa As String
    Dim sGKSa As String
    Dim sGKGa As String
    Dim arGKAlt() As String = Split(arIni(26), "/")
    Dim arMwstAlt() As String = Split(arIni(27), "/")
    Dim arGKNeu1() As String = Split(arIni(36), "/")
    Dim arMwstNeu1() As String = Split(arIni(28), "/")
    Dim arGKNeu2() As String = Split(arIni(37), "/")
    Dim arMwstNeu2() As String = Split(arIni(29), "/")


#Region "Load Form und Funktionen zur Darstellung des Moduls......................................."

    ''' <summary>
    ''' Modul Reservierung laden
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub frmReservierung_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lbMailList.Visible = False
        MyKalender1.Visible = False
        gbNameZ.Visible = False
        Me.Cursor = Cursors.WaitCursor
        Me.BackColor = Color.LightYellow
        '     coLang.SelectedIndex = 0 'Sprache setzen
        Dim sColor As Color = Color.LightYellow
        '   gbDatum.BackColor = sColor
        gbGast.BackColor = sColor
        '   gbPersonen.BackColor = sColor
        gbZimmer.BackColor = sColor
        ssMain.BackColor = sColor
        tsMain.BackColor = sColor
        tsbVorAnreise.Visible = False
        tsbNachAbreise.Visible = False
        paPreise.Location = New Point(100, 400)
        For i = 0 To 19
            For j = 0 To 2
                arZim(i, j) = ""
            Next
        Next
        Call prLand()
        Call prCreateTabellePreise()
        Call prCreateTabelleGast()
        ' Call prLoadGastInList()
        Call prLadeWerbung()
        Call prLadeZimmerInList(sgRBID)
        Dim arLa As Array
        'sprache in combofeld
        For i = 1 To sLanguage.Length - 1
            arLa = Split(sLanguage(i), ",")
            coLang.Items.Add(arLa(0))
        Next
        coLang.SelectedIndex = 0 'Sprache setzen
        'Auswahl Übernachtungsart
        coArt.Text = "" ' "Ü/F"
        'Laden der Preise
        dtPre = fcReadDataTable("Select * from Preise")

        'Buchungsdaten laden
        Dim sSQL As String = "Select * From Buchung Where BID='" & sgRBID & "' and ZimID = '" & sgRZID & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        If dt.Rows.Count = 0 Then
            Me.Close()
        End If

        sCode = dt.Rows(0).Item("Code").ToString
        If sCode.Trim = "" Or sCode = "00000" Then sCode = fcNewCode(sgRZID)
        If Val(sCode) < 0 Then sCode = ""
        tslCode.Text = sCode
        sIDR = dt.Rows(0).Item("ID").ToString
        Call prLadeBuchung(sgRBID, sIDR) 'Call prLadeBuchung(sgRBID, sgRZID)

        arOld = fcCollectDataInArray(arOld)
        tbAnzPer.Select()
        Call prCreatTabelIndex()





        '    Call prCheckAnAbReise(sgRBID, sgRZID)
        lUnbekannt = fcCheckNewResevierung(sgRBID, sgRZID)
        Call Summe(sgRBID, sgRZID)
        'Lade Buchungstecte
        Call prLoadBText()
        Me.Cursor = Cursors.Default
        bLoad = True
        tssSteuer.Text = sMwstU & "% / " & sMwstS & "% / " & sMwstG & "%"
        tssGKonto.Text = sGKUa & " / " & sGKSa & " / " & sGKGa
        tsmtNeu.Text = arIni(11) & "/" & arIni(23) & "/" & arIni(10) & "/" & "Aktuell"
        tsmtAlt.Text = arMwstAlt(0) & "/" & arMwstAlt(1) & "/" & arMwstAlt(2) & "/" & arMwstAlt(3)
        tsmtAkt.Text = sMwstUa & "/" & sMwstSa & "/" & sMwstGa & "/" & "Gespeichert"
        tsmtNeu1.Text = arMwstNeu1(0) & "/" & arMwstNeu1(1) & "/" & arMwstNeu1(2) & "/" & arMwstNeu1(3)
        tsmtNeu2.Text = arMwstNeu2(0) & "/" & arMwstNeu2(1) & "/" & arMwstNeu2(2) & "/" & arMwstNeu2(3)
    End Sub

    ''' <summary>
    ''' Tabelle "Preise" erstellen
    ''' </summary>
    ''' <remarks>
    ''' 01.02.2012 Create
    ''' </remarks>
    Private Sub prLand()
        Dim arTmp() As String = ReadOneValueFromSystemDb("Land").Split(vbCrLf)
        Dim nMax As Integer = arTmp.Length - 1
        For i = 0 To nMax
            If arTmp(i).Trim <> "" Then
                coLand.Items.Add(arTmp(i).Trim)
            End If
        Next
        coLand.Text = "DE"
    End Sub
    ''' <summary>
    ''' Tabelle "Preise" erstellen
    ''' </summary>
    ''' <remarks>
    ''' 01.02.2012 Create
    ''' </remarks>
    Private Sub prCreateTabellePreise()
        With lvPreise
            .Clear()
            .Columns.Add("Übernachtungsart", 200, HorizontalAlignment.Left)
            .Columns.Add("Saison", 100, HorizontalAlignment.Left)
            .Columns.Add("Preis", 70, HorizontalAlignment.Right)
            .Columns.Add("ID", 0, HorizontalAlignment.Left)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            .Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub

    ''' <summary>
    ''' Tabelle "Gast" erstellen
    ''' </summary>
    ''' <remarks>
    ''' 04.02.2012 Create
    ''' </remarks>
    Private Sub prCreateTabelleGast()
        With lvGast
            .Clear()
            .Columns.Add("Name-Firma", 120, HorizontalAlignment.Left)
            .Columns.Add("Vorname", 70, HorizontalAlignment.Left)
            .Columns.Add("PLZ", 50, HorizontalAlignment.Left)
            .Columns.Add("Ort", 100, HorizontalAlignment.Left)
            .Columns.Add("Strasse", 120, HorizontalAlignment.Left)
            .Columns.Add("ID", 0, HorizontalAlignment.Left)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            .Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub

    ''' <summary>
    ''' Liste mit Gastdaten befüllen
    ''' </summary>
    ''' <remarks>
    ''' 04.02.2012 Create
    ''' </remarks>
    Private Sub prLoadGastInList()
        If Len(Trim(tbName1.Text)) = 0 Then Exit Sub
        If lvGast.Items.Count <> 0 And Len(Trim(tbName1.Text)) > 1 Then Exit Sub
        Dim sSQL As String = "Select * from Kunden where name1 like '" & Mid(tbName1.Text, 1, 1) & "%' order by Name1, Vorname asc" '  where Name1 like '" & Mid(tbName1.Text, 1, 1) & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)
        ' Dim dt As DataTable = fcReadDataTable("Select * from Kunden order by Name1, Vorname asc")
        Dim i As Integer
        Dim nMax As Integer = dt.Rows.Count - 1
        If nMax < 0 Then Exit Sub
        lvGast.Items.Clear()
        For i = 0 To nMax
            ' If dt.Rows(i).RowState <> DataRowState.Deleted Then
            lvwAddItem(lvGast, dt.Rows(i).Item("Name1").ToString, dt.Rows(i).Item("Vorname").ToString, dt.Rows(i).Item("PLZ").ToString, dt.Rows(i).Item("Ort").ToString, dt.Rows(i).Item("Strasse").ToString, dt.Rows(i).Item("ID").ToString)
            ' End If
        Next
    End Sub
    Private Sub prFindGast()
        With lvGast
            .BackColor = Color.White
            Dim ii As Integer = 0
            Dim lvI As Integer = 0
            Dim l As Integer = tbName1.Text.Trim.Length
            Dim l1 As Integer = tbVorname.Text.Trim.Length
            For i = 0 To .Items.Count - 1
                If tbName1.Text.Trim = Mid(.Items(i).SubItems(0).Text, 1, l) Then

                    .Items(i).Selected = True
                    .Items(i).EnsureVisible()
                    lvI = i
                    ii = 0
                    Exit For
                End If
            Next
            For i = ii To .Items.Count - 1
                If tbName1.Text.Trim = Mid(.Items(i).SubItems(0).Text, 1, l) And tbVorname.Text.Trim = Mid(.Items(i).SubItems(1).Text, 1, l1) Then
                    .Items(i).Selected = True
                    .Items(i).EnsureVisible()
                    lvI = i
                    ii = 0
                    Exit For
                End If
            Next
            If tbName1.Text.Trim = .Items(lvI).SubItems(0).Text Then
                .Items(lvI).Selected = False
                .Items(lvI).BackColor = Color.Yellow
                .Refresh()
            End If
            If tbVorname.Text.Trim = .Items(lvI).SubItems(1).Text Then
                .Items(lvI).Selected = False
                .Items(lvI).BackColor = Color.Red
                .Refresh()
            End If
        End With
    End Sub
    ''' Fügt dem ListView eine komplette Datenzeile hinzu
    ''' </summary>
    ''' <param name="lvw">ListView-Control</param>
    ''' <param name="Text">Parameterliste der einzelnen Zellenwerte</param>
    Public Sub lvwAddItem(ByVal lvw As ListView, ByVal ParamArray Text() As String)
        With lvw.Items
            .Add(New ListViewItem(Text))
        End With
    End Sub

    ''' <summary>
    ''' Werbung in Combobox laden
    ''' </summary>
    ''' <remarks>
    ''' 04.02.2012 Create
    ''' </remarks>
    Private Sub prLadeWerbung()
        coWerbung.DataSource = dtWer
        coWerbung.ValueMember = "Werbung"
        coWerbung.DisplayMember = "Werbung"
        coWerbung.Text = "Unbekannt"


    End Sub

    ''' <summary>
    ''' Belegte (tscoZim) und Freie (tscoFreiZim) Zimmer in Combobox laden
    ''' </summary>
    ''' <param name="sRBID">BuchungsID</param>
    ''' <remarks>
    ''' 03.02.2012 Create
    ''' </remarks>
    Private Sub prLadeZimmerInList(ByVal sRBID As String)

        Dim sSQL As String = "Select * From Buchung Where BID='" & sRBID & "' and BIDIndex = '0'"
        Dim dt As DataTable = fcReadDataTable(sSQL)
        arZ(0, 0) = "99"
        arZ(0, 1) = "Weiteres Zimmer"
        tscoZim.Items.Add("Weiteres Zimmer")
        arZ(1, 0) = "98"
        arZ(1, 1) = "Gleiches Zimmer"
        tscoZim.Items.Add("Gleiches Zimmer")
        Dim xx As Integer = dt.Rows.Count
        Dim i As Integer
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                arZ(i + 2, 0) = dt.Rows(i).Item("ZimID").ToString
                arZ(i + 2, 2) = dt.Rows(i).Item("ID").ToString
                arZ(i + 2, 3) = dt.Rows(i).Item("Von").ToString
                arZ(i + 2, 4) = dt.Rows(i).Item("bis").ToString
                arZ(i + 2, 1) = " "
            Next
        End If

        For i = 2 To 20
            If arZ(i, 0) <> Nothing Then
                arZ(i, 1) = fcGetObjektZimmerName(dtZim, arZ(i, 0)).Trim

                If arZ(i, 1) <> "" Then
                    tscoZim.Items.Add(arZ(i, 1) & Space(30) & "[" & arZ(i, 2) & "]")
                End If
            End If
        Next
        'Freie Zimmer in diesem Zeitraum laden
        Dim sB As Long = dt.Rows(0).Item("Von")
        Dim sE As Long = dt.Rows(0).Item("Bis")
        sSQL = "Select * From Buchung Where (Von <='" & sB & "' or Von <='" & sE & "') and (Bis >='" & sB & "' or Bis >='" & sE & "')"
        dt = fcReadDataTable(sSQL)
        Dim arB(0) As String 'Belegte Zimmer
        Dim nMax As Integer = dt.Rows.Count - 1
        For i = 0 To nMax
            ReDim Preserve arB(i)
            arB(i) = dt.Rows(i).Item("ZimID").ToString
        Next
        nMax = dtZim.Rows.Count - 1
        Dim n As Integer = 0
        Dim sTmp As String
        For i = 0 To nMax
            sTmp = dtZim.Rows(i).Item("ID").ToString.Trim
            If fcIfZimmerInArray(arB, sTmp) = False Then
                arFZ(n, 0) = sTmp
                arFZ(n, 1) = dtZim.Rows(i).Item("Name").ToString
                n += 1
            End If
        Next
        For i = 0 To 20
            If arFZ(i, 0) <> Nothing Then
                tscoFreiZim.Items.Add(arFZ(i, 1))
            End If
        Next


        For i = 2 To 19

            If arZ(i, 1) <> "" Then
                For j = 0 To 19
                    If arZim(j, 0) = "" Then
                        arZim(j, 0) = arZ(i, 1)
                        arZim(j, 1) = arZ(i, 3)
                        arZim(j, 2) = arZ(i, 4)
                        Exit For
                    End If
                    If arZ(i, 1) = arZim(j, 0) Then
                        If arZim(j, 1) > arZ(i, 3) Then arZim(j, 1) = arZ(i, 3)
                        If arZim(j, 2) < arZ(i, 4) Then arZim(j, 2) = arZ(i, 4)
                        Exit For
                    End If
                Next
            End If
        Next
        '       arZimmer = arZimmer
    End Sub

    Private Sub prLadeBuchung(ByVal sRID As String, ByVal sIDRe As String)

        Dim sSQL As String = "Select * From Buchung Where BID='" & sRID & "' and ID='" & sIDRe & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)
        Try

            If dt.Rows.Count > 0 Then
                sMwstG = dt.Rows(0).Item("MwstG").ToString
                sMwstS = dt.Rows(0).Item("MwstS").ToString
                sMwstU = dt.Rows(0).Item("MwstU").ToString
                sGKU = dt.Rows(0).Item("GKU").ToString
                sGKS = dt.Rows(0).Item("GKS").ToString
                sGKG = dt.Rows(0).Item("GKG").ToString
                If sMwstG = "0" Then sMwstG = arIni(10)
                If sMwstS = "0" Then sMwstS = arIni(23)
                If sMwstU = "0" Then sMwstU = arIni(11)
                If sGKU = "0" Then sGKU = arIni(20)
                If sGKS = "0" Then sGKS = arIni(25)
                If sGKG = "0" Then sGKG = arIni(21)
                sMwstGa = sMwstG
                sMwstSa = sMwstS
                sMwstUa = sMwstU
                sGKGa = sGKG
                sGKSa = sGKS
                sGKUa = sGKU


                sIDB = dt.Rows(0).Item("ID").ToString
                sBID = dt.Rows(0).Item("ID").ToString
                If dt.Rows(0).Item("BIDIndex").ToString = "0" Then
                    sIDRef = sIDB
                    tscoZim.Enabled = True
                    '   tsmBuchSplitt.Enabled = True
                Else
                    tscoZim.Enabled = False
                    '    tsmBuchSplitt.Enabled = False
                End If
                sIDRef = sIDB
                'An und Abreise
                Call prLadeAnAbReise(dt)
                'Zimmer laden
                Call prLadeZimmer(dt.Rows(0).Item("ZimID").ToString) 'sZID
                'Laden der Preise
                ' Call prLoadPreiseInList(dtPre, sgSasion)
                'lStart = False
                'Lade Personen / Übernachtungen
                Call prLaderPersonenUeberNachtung(dt)
                'Lade Gast-Daten
                lbGastID.Text = dt.Rows(0).Item("KunID").ToString
                Call prLadeGastDaten(lbGastID.Text)
                tscoZim.Text = lbZimNr.Text & Space(30) & "[" & dt.Rows(0).Item("ID").ToString & "]"
                'Lade Frühstückspreis
                '   tbFPreis.Text = (Val(dt.Rows(0).Item("FPreis")) / 100).ToString
                tbFPreis.Text = ((Val(dt.Rows(0).Item("FPreis")) + Val(dt.Rows(0).Item("GPreis"))) / 100).ToString
                tbFPreis.Text = fcFormatDecimal(tbFPreis.Text)
                'Lade Storno
                tbStorno.Text = dt.Rows(0).Item("Storno").ToString
                'lade Gesammtpreis
                tbSumme.Text = fcFormatDecimal((Val(dt.Rows(0).Item("Summe")) / 100).ToString)

                tbName1Z.Text = dt.Rows(0).Item("Name1").ToString
                tbName2Z.Text = dt.Rows(0).Item("Name2").ToString
                If tbName1Z.Text = "0" Then tbName1Z.Text = ""
                If tbName2Z.Text = "0" Then tbName2Z.Text = ""


                '#########################################################################################################################
                If dt.Rows(0).Item("Bez").ToString.Trim = "0" Then
                    cbBezalt.Checked = False
                Else
                    cbBezalt.Checked = True
                End If
                '#########################################################################################################################
                'Lade Werbung
                Dim sW As String = dt.Rows(0).Item("Werbung").ToString
                'Rechnung geschrieben
                lRec = dt.Rows(0).Item("Rechnung")
                'Call prLookForm(lRec)
                Dim test As String = coWerbung.Text
                If coWerbung.Text = "Unbekannt" Then
                    coWerbung.Text = sW
                    ' sgWerbung = sW
                End If


                'Rchnungsstatus
                lgRech = dt.Rows(0).Item("Rechnung")
                sRNr = dt.Rows(0).Item("RID")
                'Prüfen ob Rechnung geschrieben wurde
                lgRech = fcChaneMenue(lgRech)
                'Sprache und Buchungstext
                Dim sL As String = dt.Rows(0).Item("Sprache")
                If sL.Trim = "" Then sL = "0"
                coLang.SelectedIndex = sL
                lbMakro.Text = dt.Rows(0).Item("MText")
                Dim sID As String = dt.Rows(0).Item("BText")
                Dim dtB As DataTable = fcReadDataTable("Select * from BTexte Where ID='" & sID & "'")
                '   Dim dtB As DataTable = fcReadDataTable("Select Name from BTexte Where ID='" & sID & "'")
                If dtB.Rows.Count <> 0 Then coBText.Text = dtB.Rows(0).Item(1)
                lStart = False
                sgRNr = sRNr

                If IsDBNull(dt.Rows(0).Item("RDSenden")) = False Then
                    tbRechSend.Text = dt.Rows(0).Item("RDSenden")
                Else
                    tbRechSend.Text = ""
                End If
                If IsDBNull(dt.Rows(0).Item("Pausch")) = False Then

                    cbPausch.Checked = False
                    If dt.Rows(0).Item("Pausch") = "1" Then cbPausch.Checked = True
                Else
                    cbPausch.Checked = False
                    ' If dt.Rows(0).Item("Pausch") = "1" Then cbPausch.Checked = True

                End If
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Prüfen ob eine neue Reservierung vorliegt ("Unbekannt")
    ''' </summary>
    ''' <param name="sRID"></param>
    ''' <param name="sZID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 26.02.2012 Create
    ''' </remarks>
    Private Function fcCheckNewResevierung(ByVal sRID As String, ByVal sZID As String) As Boolean
        Dim sSQL As String = "Select * From Buchung Where BID='" & sRID & "' and ZimID='" & sZID & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0).Item("KunID").ToString.Trim = "0" Then
                fcCheckNewResevierung = True
            End If
        End If
    End Function

    ''' <summary>
    ''' Prüfung ob die Zimmer-ID im Array arF enthalten ist 
    ''' </summary>
    ''' <param name="arB">Belegte Zimmer</param>
    ''' <param name="sID">Zimmer ID</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 03.02.2012 Create
    ''' </remarks>
    Private Function fcIfZimmerInArray(ByVal arB() As String, ByVal sID As String) As Boolean
        fcIfZimmerInArray = False
        For i As Integer = 0 To arB.Length - 1
            If arB(i).Trim = sID Then
                fcIfZimmerInArray = True
                Exit For
            End If
        Next
    End Function

    ''' <summary>
    ''' Tabelle User mit daten aus der DataTabel "Preise" füllen
    ''' </summary>
    ''' <param name="dtT"></param>
    ''' <remarks>
    ''' 01.02.2012 Create
    ''' </remarks>
    Private Sub prLoadPreiseInList(ByVal dtT As DataTable, ByVal sSasion As String)
        Dim i As Integer
        Dim sZim As String
        Dim nMax As Integer = dtT.Rows.Count - 1
        If nMax < 0 Then Exit Sub
        lvPreise.Items.Clear()
        For i = 0 To nMax
            If dtT.Rows(i).RowState <> DataRowState.Deleted Then
                Dim lv As ListViewItem

                With lvPreise
                    sZim = dtT.Rows(i).Item("Beschreibung").ToString
                    If sZim = lbArt.Text Then
                        lv = .Items.Add(dtT.Rows(i).Item("Kategorie").ToString)
                        Select Case dtT.Rows(i).Item("Sasion").ToString
                            Case Is = "V"
                                lv.SubItems.Add("Vorsaison")
                            Case Is = "H"
                                lv.SubItems.Add("Hauptsaison")
                        End Select
                        lv.SubItems.Add(fcDecStr(Val(dtT.Rows(i).Item("Preis").ToString) / 100, , , ))
                        lv.SubItems.Add(dtT.Rows(i).Item("ID").ToString)
                    End If
                End With
            End If
        Next

    End Sub

    ''' <summary>
    ''' An und Abreise daten laden und anzeigen
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks>
    ''' 03.02.2012 Create
    ''' </remarks>
    Private Sub prLadeAnAbReise(ByVal dt As DataTable)
        Dim dVon As Date = fcUmDatum(dt.Rows(0).Item("Von").ToString)
        Dim dBis As Date = fcUmDatum(dt.Rows(0).Item("Bis").ToString)
        dBis = DateAdd(DateInterval.Day, 1, dBis)
        lbAnreise.Text = dVon
        lbAbreise.Text = dBis
        lbTage.Text = DateDiff(DateInterval.Day, dVon, dBis)
        tbAnZeit.Text = fcUmZeit(dt.Rows(0).Item("VonZeit").ToString)
        tbAbZeit.Text = fcUmZeit(dt.Rows(0).Item("BisZeit").ToString)
    End Sub

    ''' <summary>
    ''' Zimmerdaten laden und anzeigen
    ''' </summary>
    ''' <param name="sRZID"></param>
    ''' <remarks>
    ''' 03.02.2012 Create
    ''' </remarks>
    Private Sub prLadeZimmer(ByVal sRZID As String)
        Dim sSQL As String = "Select * From Zimmer Where ID='" & sRZID & "'"
        Dim dtZ As DataTable = fcReadDataTable(sSQL)
        Dim sObj As String
        If dtZ.Rows.Count > 0 Then
            lbZimNr.Text = dtZ.Rows(0).Item("Name").ToString
            lbArt.Text = dtZ.Rows(0).Item("Art").ToString
            lbBetten.Text = dtZ.Rows(0).Item("Betten").ToString
            lbAusstattung.Text = dtZ.Rows(0).Item("Ausstattung").ToString
            sObj = dtZ.Rows(0).Item("IDObjekte").ToString
            lbObjekt.Text = fcGetObjektName(dtObj, sObj)
        End If
    End Sub

    ''' <summary>
    ''' Anzahl der Personen und Übernachtungen
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks>
    ''' 03.02.2012 Create
    ''' </remarks>
    Private Sub prLaderPersonenUeberNachtung(ByVal dt As DataTable)
        tbInternetNr.Text = dt.Rows(0).Item("InternetNr").ToString
        tbAnzPer.Text = dt.Rows(0).Item("Personen").ToString
        tbUArt.Text = dt.Rows(0).Item("Kategorie").ToString
        coArt.Text = dt.Rows(0).Item("Art").ToString
        tbPreis.Text = fcFormatDecimal(Val(dt.Rows(0).Item("Preis").ToString) / 100)
        tbAnzahlung.Text = fcFormatDecimal(Val(dt.Rows(0).Item("Anzahlung").ToString) / 100)
        Dim sVariable As String = dt.Rows(0).Item("Variable").ToString
        Select Case sVariable
            Case "0"
                rbNormal.Checked = True
            Case "1"
                rbFest.Checked = True
            Case "2"
                rbVariabel.Checked = True
        End Select

    End Sub

    ''' <summary>
    ''' Gastdaten laden und anzeigen
    ''' </summary>
    ''' <param name="sID"></param>
    ''' <remarks>
    ''' 04.02.2012 Create
    ''' </remarks>
    Private Sub prLadeGastDaten(ByVal sID As String)
        Dim sSQL As String = "Select * From Kunden Where ID='" & sID & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)

        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                coAnrede.Text = .Item("Anrede").ToString
                tbName1.Text = .Item("Name1").ToString
                tbName2.Text = .Item("Name2").ToString
                tbName1Z.Text = .Item("Name1Z").ToString
                tbName2Z.Text = .Item("Name2Z").ToString
                If tbName1Z.Text = "0" Then tbName1Z.Text = ""
                If tbName2Z.Text = "0" Then tbName2Z.Text = ""
                tbVorname.Text = .Item("Vorname").ToString
                tbStrasse.Text = .Item("Strasse").ToString

                tbPLZ.Text = .Item("PLZ").ToString
                tbOrt.Text = .Item("Ort").ToString
                coLand.Text = .Item("Land").ToString
                tbTel.Text = .Item("Telefon").ToString
                tbFax.Text = .Item("Telefax").ToString
                tbFunk.Text = .Item("Funk").ToString
                tbEMail.Text = .Item("EMail").ToString
                '  tbPass.Text = .Item("Pass").ToString
                tbInfo.Text = .Item("Info").ToString
                If .Item("Geb").ToString.Trim <> "" Then
                    dpGeb.Value = fcUmDatum(.Item("Geb").ToString)
                End If
                lbGast.Text = "(" & coAnrede.Text & " " & tbName1.Text & ")"
                Dim test As String = coWerbung.Text
                If .Item("Werbung").ToString.Trim <> "" Then
                    ' If coWerbung.Text = "Unbekannt" Then
                    coWerbung.Text = .Item("Werbung").ToString
                End If
                If coLand.Text.Trim = "" Then coLand.Text = "DE"
            End With
        End If
        'Neuer Gast
        If sID = 0 Then
            Call prClearGastDaten()
        End If
    End Sub

    ''' <summary>
    ''' Aktuelle Werte in ein Array sichern (Vergleich vorher / nacher) => Daten speichern
    ''' </summary>
    ''' <param name="arT"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Function fcCollectDataInArray(ByVal arT() As String) As Array
        'Dim arT(22) As String
        arT(0) = tbAnzPer.Text
        arT(1) = coArt.Text
        arT(2) = tbUArt.Text
        arT(3) = tbPreis.Text
        arT(4) = tbAnzahlung.Text
        arT(5) = coAnrede.Text
        arT(6) = tbName1.Text
        arT(7) = tbName2.Text
        arT(8) = tbVorname.Text
        arT(9) = tbStrasse.Text
        arT(10) = tbPLZ.Text
        arT(11) = tbOrt.Text
        arT(12) = coLand.Text
        arT(13) = tbTel.Text
        arT(14) = tbFax.Text
        arT(15) = tbFunk.Text
        arT(16) = tbEMail.Text
        arT(17) = "" 'tbPass.Text
        arT(18) = tbInfo.Text
        arT(19) = fcUmDatum(dpGeb.Value)
        arT(20) = coWerbung.Text
        arT(21) = lbAnreise.Text
        arT(22) = lbAbreise.Text
        arT(23) = tbFPreis.Text
        arT(24) = lbMakro.Text
        fcCollectDataInArray = arT
    End Function




    ''' <summary>
    ''' Steuerung des zweiten Namensfeldes in Abhängigkeit der Anrede
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub coAnrede_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coAnrede.SelectedIndexChanged

        If coAnrede.Text = "Firma" Then
            tbName2.Enabled = True
            tbVorname.Enabled = False
            Label12.Text = "Firma"
            Label16.Text = "Name"
            Label25.Text = ""
        Else
            tbName2.Enabled = False
            tbVorname.Enabled = True
            Label12.Text = "Name"
            Label16.Text = ""
            Label25.Text = "Vorname"
        End If

    End Sub

    ''' <summary>
    ''' Prüfung ob Preis als Zahl eingegeben wurde
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tbAnzahlung_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbAnzahlung.KeyPress
        Call prCheckNumericKey(e.KeyChar, tbAnzahlung)
    End Sub

    ''' <summary>
    ''' Prüfung ob Preis als Zahl eingegeben wurde
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tbAnzahlung_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAnzahlung.Leave
        tbAnzahlung.Text = fcFormatDecimal(tbAnzahlung.Text)
    End Sub

    ''' <summary>
    ''' Prüfung ob Preis als Zahl eingegeben wurde
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tbPreis_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbPreis.KeyPress
        Call prCheckNumericKey(e.KeyChar, tbPreis)
    End Sub

    ''' <summary>
    ''' Prüfung ob Preis als Zahl eingegeben wurde
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tbPreis_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPreis.Leave
        tbPreis.Text = fcFormatDecimal(tbPreis.Text)
        Call Summe(sgRBID, sgRZID)
    End Sub

    ''' <summary>
    ''' Steuerung der Menüs in Abhängigkeit des Rechnungsstatus
    ''' </summary>
    ''' <param name="lr"></param>
    ''' <remarks>
    ''' 28.02.2012 Create
    ''' </remarks>
    Private Sub prLookForm(ByVal lr As Boolean)

        '  gbPersonen.Enabled = Not lr
        gbGast.Enabled = Not lr
        tsbSave.Enabled = Not lr

        tsbAufZimmer.Visible = Not lr
        tsbRechnung.Visible = Not lr
        tsbBestätigung.Visible = Not lr
        tsbErinnerung.Visible = Not lr
        tsbDelReservierung.Visible = Not lr
        tsbDelZimmer.Visible = Not lr
        tsSep1.Visible = Not lr
        '   tsmAnAbReise.Visible = Not lr
        tsSep2.Visible = Not lr
        tsmKopie.Visible = lr

    End Sub

    Private Function fcGetSummeZimmer(ByVal sBID As String, ByVal sZID As String) As String

        Dim nZPreis As Double = tbPreis.Text
        Dim nZusatz As Double = 0
        Dim nTage As Integer = Val(lbTage.Text) - 1
        fcGetSummeZimmer = fcFormatDecimal((nZPreis * nTage + nZusatz).ToString) & " € "
        Dim dt As DataTable = fcReadDataTable("Select * from Zusaetze Where BuchID='" & sBID & "' and ZimID ='" & sZID & "'")
        If dt.Rows.Count = 0 Then Exit Function
        Dim nMax As Integer = dt.Rows.Count - 1
        Dim nSumme As Double
        Dim nMenge As Integer
        Dim nSteuerSatz As Integer = 0
        Try
            For i As Integer = 0 To nMax
                If dt.Rows(i).RowState <> DataRowState.Deleted Then
                    nSumme = dt.Rows(i).Item("Betrag")
                    nMenge = dt.Rows(i).Item("Menge")
                    nSteuerSatz = dt.Rows(i).Item("Steuer")
                    nSumme = nSumme * nMenge
                    nSumme = nSumme + (nSumme * nSteuerSatz / 100)
                    nZusatz = nZusatz + nSumme
                End If
            Next
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
            fcGetSummeZimmer = fcFormatDecimal((nZPreis * nTage + nZusatz).ToString) & " € "
        End Try
    End Function

    Private Sub prLoadBText()
        Dim dt As DataTable = fcReadDataTable("Select * from BTexte Where Art='" & "1" & "'")
        If dt.Rows.Count = 0 Then Exit Sub
        ReDim arZZiel(dt.Rows.Count)
        For i As Integer = 0 To dt.Rows.Count - 1
            coBText.Items.Add(dt.Rows(i).Item(1))
            arZZiel(i) = dt.Rows(i).Item("ZZiel")
        Next
    End Sub

#End Region

#Region "Preis- und Gasteliste....................................................................."



    ''' <summary>
    ''' Preisliste zur Auswahl öffnen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub btUArt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btUArt.Click
        paPreise.Location = New Point(122, 268)
        paPreise.Visible = Not paPreise.Visible
        If paPreise.Visible Then
            lvPreise.Visible = True
        Else
            lvPreise.Visible = False
            ' lvGast.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Mit Doppelklick ein Preis auswählen und liste schliessen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub lvPreise_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvPreise.MouseDoubleClick

        With lvPreise
            If .SelectedItems.Count <> 0 Then
                tbUArt.Text = .SelectedItems(0).SubItems(0).Text
                tbPreis.Text = .SelectedItems(0).SubItems(2).Text
                Call Summe(sgRBID, sgRZID)
            End If
        End With
        paPreise.Visible = False
        lvPreise.Visible = False
        '     lvGast.Visible = False
    End Sub

    ''' <summary>
    ''' Mit Doppelklick einen Gast auswählen und Liste schliessen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub lvGast_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvGast.MouseDoubleClick
        lbGastID.Text = "0"
        With lvGast
            If .SelectedItems.Count <> 0 Then
                lbGastID.Text = .SelectedItems(0).SubItems(5).Text
            End If
        End With
        paPreise.Visible = False
        lvPreise.Visible = False
        'lvGast.Visible = False
        Call prLadeGastDaten(lbGastID.Text)
        coWerbung.Text = "Stammgast"
    End Sub

    ''' <summary>
    ''' Gäste / Preisliste schliessen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub btClosePreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btClosePreise.Click
        paPreise.Visible = False
        lvPreise.Visible = False
        '  lvGast.Visible = False
    End Sub

#End Region

#Region "Button und Toolbar auswerten.............................................................."

    ''' <summary>
    ''' Modul benden
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tsbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbClose.Click


        '  sgNet = fcCodeSend()
        Me.Close()
    End Sub
    Private Sub frmKey_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim arfeld1() As String = fcDataSeek("select * From Buchung Where BID ='", sgRBID, 0, {"SendMailZugang"})
        If arfeld1(0) = "Y" Then
            If MsgBox("Buchung Freigeben?", vbExclamation + vbOKCancel, "Ja") = MsgBoxResult.Ok Then
                Dim arFields() As String = {"SendMailZugang"}
                Dim arValue() As String = {"X"}
                Dim cBedingung As String = " WHERE BID='" & sgRBID & "'"
                Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
            End If
        End If
        '  sgNet = fcCodeSend()
    End Sub

    ''' <summary>
    ''' Mit Doppelklick auf den Namen werden die Gastdaten gelöscht.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub lbGast_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbGast.DoubleClick
        Call prClearGastDaten()
    End Sub

    ''' <summary>
    ''' Gastdaten im Formular löschen
    ''' </summary>
    ''' <remarks>
    ''' 05.02.2012 Create
    ''' </remarks>
    Private Sub prClearGastDaten()
        lbGast.Text = "(neuer Gast)"
        coAnrede.Text = "Firma"
        tbName1.Text = ""
        tbName2.Text = ""
        tbVorname.Text = ""
        tbStrasse.Text = ""
        tbPLZ.Text = ""
        tbOrt.Text = ""
        coLand.Text = "DE"
        tbTel.Text = ""
        tbFax.Text = ""
        tbFunk.Text = ""
        tbEMail.Text = ""
        '   tbPass.Text = ""
        tbInfo.Text = ""
        lbGastID.Text = ""
        coWerbung.Text = "Unbekannt"
        dpGeb.Value = Date.Today
    End Sub

    ''' <summary>
    ''' Änderung in der Reservierung speichen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tsbSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSave.Click
        Dim sC As String = fcCode(sCode)
        Call prSaveZimmerReservierung()
        arNew = fcCollectDataInArray(arNew)
        ' Dim sC As String = fcCode(sCode)
        Dim sb2 As New StringBuilder
        Dim arFields(), arValue() As String
        Dim sqlText As String = ""
        sqlText = "code,von,vonzeit,bis,biszeit,BID"
        arFields = Split(sqlText, ",")
        sb2.Append(sC & "°")  'ID
        sb2.Append(fcUmDatum(arNew(21)) & "°")    'VON 'art(21)
        sb2.Append(fcUmZeit(tbAnZeit.Text) & "°")
        sb2.Append(fcUmDatum(arNew(22)) & "°")   'BIS
        sb2.Append(fcUmZeit(tbAbZeit.Text) & "°")
        sb2.Append(sgRBID)  'BID
        arValue = Split(sb2.ToString, "°")
        If sCode.Trim <> "" And sCode.Trim <> "00000" Then
            Dim sSQL As String = "Select * From Code Where code='" & sC & "'"
            Dim dt As DataTable = fcReadDataTable(sSQL)
            If dt.Rows.Count = 0 Then
                Call fcInsertCommand("Code", arFields, arValue)
            Else
                Dim cBedingung As String = " WHERE code='" & sC & "'"
                Call fcUpdateCommand("Code", arFields, arValue, cBedingung)
            End If
        End If










    End Sub

    ''' <summary>
    ''' Mit Klick in die Auswahlbox alle Daten in das Array "arNew" sichern (Vergleich)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tscoZim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tscoZim.Click
        arNew = fcCollectDataInArray(arNew)

    End Sub

    ''' <summary>
    ''' Wenn ein neuer Eintrag ausgewählt wurde wird geprüft ob Änderungen in der alten Auswahl
    ''' vorgenommen wurden. Wenn Ja, dann wird die Änderung gespeichert und die neue Buchung geladen. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tscoZim_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tscoZim.SelectedIndexChanged
        If lStart Then Exit Sub

        ' Call prSaveZimmerReservierung()
        If tscoZim.Text = "Gleiches Zimmer" Then
            Call prSaveZimmerReservierung()
            tsbVorAnreise.Visible = True
            tsbNachAbreise.Visible = True


        Else

            If tscoZim.Text = "Weiteres Zimmer" Then
                '   If lRec = False Then 'Keine Rechnung geschrieben
                tscoFreiZim.Visible = True
                tsbAddZimmer.Visible = True
                If tscoFreiZim.Items.Count > 0 Then tscoFreiZim.SelectedIndex = 0
                tscoFreiZim.Select()
                'End If
            Else
                tscoFreiZim.Visible = False
                tsbAddZimmer.Visible = False
                sgRZID = fcGetObjektZimmerID(dtZim, tscoZim.Text, "ID")
                sIDR = Extract(tscoZim.Text, "[", "]", 1)
                '       = sIDR
                Call prLadeBuchung(sgRBID, sIDR)
                Call Summe(sgRBID, sgRZID)
                arOld = fcCollectDataInArray(arOld)
                tbAnzPer.Select()

                Call prCreatTabelIndex()
            End If
        End If
    End Sub

    Private Sub tsbVorAnreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbVorAnreise.Click
        ' Call prSaveZimmerReservierung()
        Dim dDa As Date 'abreise datum wie bisheriges anreisedatum
        Dim sAn As String = lbAnreise.Text
        Dim sAb As String = lbAbreise.Text
        dDa = CDate(sAn)

        sAb = dDa
        dDa = DateAdd(DateInterval.Day, -1, dDa)
        sAn = dDa

        If frControlFreeZimmer(lbZimNr.Text, sAn, sAb) = True Then
            Dim ID As String = fcAppendBlank("Buchung")
            Dim arFields() As String = {"SendMailZugang"}
            Dim arValue() As String = {"Y"}
            Dim cBedingung As String = " WHERE ID='" & ID & "'"
            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)

            sBID = ID
            tscoZim.Items.Add(lbZimNr.Text & Space(30) & "[" & ID & "]")
            lbAnreise.Text = sAn
            lbAbreise.Text = sAb
            arNew(21) = sAn
            arNew(22) = sAb
            arNew = fcCollectDataInArray(arNew)
            Call prAddNewZimmer(lbZimNr.Text, arNew, ID)
            tscoZim.Text = lbZimNr.Text & Space(30) & "[" & ID & "]"
            Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")

            'Call prSaveZimmerReservierung()
        Else

            MsgBox("Zimmer schon Vermittet", MsgBoxStyle.Question + MsgBoxStyle.OkOnly)
        End If
        tsbVorAnreise.Visible = False
        tsbNachAbreise.Visible = False
    End Sub

    Private Sub tsbNachAbreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNachAbreise.Click
        '   Call prSaveZimmerReservierung()
        Dim dDa As Date 'anreise datum wie bisheriges abreisedatum

        Dim sAn As String = lbAnreise.Text
        Dim sAb As String = lbAbreise.Text
        dDa = CDate(sAb)
        sAn = lbAbreise.Text
        dDa = DateAdd(DateInterval.Day, 1, dDa)
        sAb = dDa


        If frControlFreeZimmer(lbZimNr.Text, sAn, sAb) = True Then
            Dim ID As String = fcAppendBlank("Buchung")

            Dim arFields() As String = {"SendMailZugang"}
            Dim arValue() As String = {"Y"}
            Dim cBedingung As String = " WHERE ID='" & ID & "'"
            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)





            sBID = ID
            tscoZim.Items.Add(lbZimNr.Text & Space(30) & "[" & ID & "]")
            lbAnreise.Text = sAn
            lbAbreise.Text = sAb
            arNew(21) = sAn
            arNew(22) = sAb
            Call prAddNewZimmer(lbZimNr.Text, arNew, ID)
            tscoZim.Text = lbZimNr.Text & Space(30) & "[" & ID & "]"

        Else
            MsgBox("Zimmer schon Vermittet", MsgBoxStyle.Question + MsgBoxStyle.OkOnly)
        End If
        tsbVorAnreise.Visible = False
        tsbNachAbreise.Visible = False
    End Sub
    Private Function frControlFreeZimmer(ByRef Zim As String, ByRef Von As String, ByRef sBis As String) As Boolean
        frControlFreeZimmer = False
        Dim dDa As Date = CDate(sBis)
        dDa = DateAdd(DateInterval.Day, -1, dDa)
        Dim Bis As String = CDate(dDa)
        Dim sZim As String = fcGetObjektZimmerID(dtZim, Zim, "ID")
        Dim dt As DataTable
        Dim sSql As String
        sSql = "Select * From Buchung  Where ZimID='" & sZim & "' and Von<='" & fcUmDatum(Von) & "' and Bis >= '" & fcUmDatum(Bis) & "'"
        dt = fcReadDataTable(sSql)
        If dt.Rows.Count = 0 Then frControlFreeZimmer = True

    End Function

    Private Sub tbAnreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbAnreise.Click
        Dim aSperr As String = ""
        Dim aBuchung As String = ""
        Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")
        Dim dt As DataTable
        Dim sSql As String
        Dim sID As String = Extract(tscoZim.Text, "[", "]", 1)
        Dim d1 As Date = DateAdd(DateInterval.Month, -8, CDate(lbAnreise.Text))
        Dim d2 As Date = DateAdd(DateInterval.Month, 8, CDate(lbAnreise.Text))
        sSql = "Select * From Buchung  Where ZimID='" & sZim & "' and Von<='" & fcUmDatum(CDate(d2)) & "' and Bis >= '" & fcUmDatum(CDate(d1)) & "'"
        dt = fcReadDataTable(sSql)
        For k = 0 To dt.Rows.Count - 1
            If dt.Rows(k).Item("ID").ToString <> sID And dt.Rows(k).Item("BID").ToString <> sgRBID Then
                aSperr = aSperr & ";" & dt.Rows(k).Item("von").ToString & "-" & dt.Rows(k).Item("bis").ToString

            End If
        Next
        For k = 0 To dt.Rows.Count - 1
            If dt.Rows(k).Item("ID").ToString <> sID And dt.Rows(k).Item("BID").ToString = sgRBID Then
                aBuchung = aBuchung & ";" & dt.Rows(k).Item("von").ToString & "-" & dt.Rows(k).Item("bis").ToString

            End If
        Next

        MyKalender1.Datum = fcUmDatum(lbAnreise.Text)
        MyKalender1.Sperr = Mid(aSperr, 2)
        MyKalender1.Buchung = Mid(aBuchung, 2)
        MyKalender1.AnreiseAbreise = True

        MyKalender1.Anreise = fcUmDatum(lbAnreise.Text) 'dAn
        MyKalender1.Abreise = fcUmDatum(lbAbreise.Text) 'dAb
        MyKalender1.Visible = True

    End Sub

    Private Sub tbAbreise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbAbreise.Click
        Dim aSperr As String = ""
        Dim aBuchung As String = ""
        Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")
        Dim dt As DataTable
        Dim sSql As String
        Dim sID As String = Extract(tscoZim.Text, "[", "]", 1)
        Dim d1 As Date = DateAdd(DateInterval.Month, -8, CDate(lbAnreise.Text))
        Dim d2 As Date = DateAdd(DateInterval.Month, 8, CDate(lbAnreise.Text))
        sSql = "Select * From Buchung  Where ZimID='" & sZim & "' and Von<='" & fcUmDatum(CDate(d2)) & "' and Bis >= '" & fcUmDatum(CDate(d1)) & "'"
        dt = fcReadDataTable(sSql)
        For k = 0 To dt.Rows.Count - 1
            If dt.Rows(k).Item("ID").ToString <> sID And dt.Rows(k).Item("BID").ToString <> sgRBID Then
                aSperr = aSperr & ";" & dt.Rows(k).Item("von").ToString & "-" & dt.Rows(k).Item("bis").ToString

            End If
        Next
        For k = 0 To dt.Rows.Count - 1
            If dt.Rows(k).Item("ID").ToString <> sID And dt.Rows(k).Item("BID").ToString = sgRBID Then
                aBuchung = aBuchung & ";" & dt.Rows(k).Item("von").ToString & "-" & dt.Rows(k).Item("bis").ToString

            End If
        Next
        ' dDa = DateAdd(DateInterval.Day, 1, dDa)
        MyKalender1.Datum = fcUmDatum(lbAnreise.Text)
        MyKalender1.Sperr = Mid(aSperr, 2)
        MyKalender1.Buchung = Mid(aBuchung, 2)
        MyKalender1.AnreiseAbreise = False
        MyKalender1.Anreise = fcUmDatum(lbAnreise.Text) 'dAn
        MyKalender1.Abreise = fcUmDatum(lbAbreise.Text) 'dAb
        MyKalender1.Visible = True
    End Sub





    ''' <summary>
    ''' Neues Zimmer zur Reservierung hinzufügen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub tsbAddZimmer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAddZimmer.Click
        Dim sZim As String = tscoFreiZim.Text

        Dim ID As String = fcAppendBlank("Buchung") 'fcGetTimeID(Date.Today)


        Dim arFields() As String = {"SendMailZugang"}
        Dim arValue() As String = {"Y"}
        Dim cBedingung As String = " WHERE ID='" & ID & "'"
        Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)


        tscoZim.Items.Add(sZim & Space(30) & "[" & ID & "]")
        tscoFreiZim.Items.Remove(tscoFreiZim.SelectedItem)
        tscoFreiZim.Visible = False
        tsbAddZimmer.Visible = False
        'Buchungsdatensatz anlegen
        Call prAddNewZimmer(sZim, arNew, ID)
    End Sub

    ''' <summary>
    ''' Buchungsdatensatz für das neue Zimmer erzeugen
    ''' </summary>
    ''' <param name="sZim"></param>
    ''' <param name="arT"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prAddNewZimmer(ByVal sZim As String, ByVal arT() As String, ByVal ID As String)
        Dim sBID As String = sgRBID
        Dim sKNr As String = lbGastID.Text
        Dim sZID As String = fcGetObjektZimmerID(dtZim, sZim, "ID")
        Dim sOID As String = fcGetObjektZimmerID(dtZim, sZim, "IDObjekte")

        Call prSaveNewReservierung(sBID, sKNr, sZID, sOID, arT, ID)
        Call prSetReservierungInDataGrid(arT(21), arT(22), sZID, "0", arT(6), sBID, "Ü")
    End Sub

    ''' <summary>
    ''' Neue Reservierung speichern
    ''' </summary>
    ''' <param name="sBID"></param>
    ''' <param name="sKNr"></param>
    ''' <param name="sZID"></param>
    ''' <param name="sOID"></param>
    ''' <param name="arT"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prSaveNewReservierung(ByVal sBID As String, ByVal sKNr As String, _
                                      ByVal sZID As String, ByVal sOID As String, _
                                      ByVal arT() As String, ByVal ID As String)
        Dim sPauch As String = "0"
        If cbPausch.Checked = True Then sPauch = "1"
        Dim sDatum As Date = arT(22)
        Dim sADatum As Date = DateAdd(DateInterval.Day, -1, sDatum)
        Dim sb2 As New StringBuilder
        Dim arFields(), arValue() As String
        Dim sqlText As String = ""
        sqlText = "ID,BID,Von,Bis,ObjID,ZimID,Variable,KunID,Kunde,Name1,Name2,Personen,Tiere,Art,Frueh,Kategorie,Preis,Anzahlung,Werbung,Info,Rechnung,BuchDatum,Sprache,BText,MText,RID,RDatum,FPreis,GPreis,MwstU,MwstS,MwstG,GKU,GKS,GKG,RDSenden,BIDIndex,IDRef,RAID,RADatum,Storno,Summe,Pausch,Code,InternetNr,VonZeit,BisZeit"
        arFields = Split(sqlText, ",")
        If sKNr = "" Then sKNr = "0"
        sb2.Append(ID & "°")  'ID
        sb2.Append(sgRBID & "°")  'BID
        sb2.Append(fcUmDatum(arT(21)) & "°")    'VON 'art(21)
        sb2.Append(fcUmDatum(sADatum) & "°")   'BIS
        sb2.Append(sOID & "°") 'Objekt
        sb2.Append(sZID & "°") 'zimmerID
        sb2.Append("0" & "°")  'Variabel
        sb2.Append(sKNr & "°") 'kunde_ID
        sb2.Append(tbName1.Text & "°") 'kunde
        sb2.Append(tbName1Z.Text & "°") 'kunde
        sb2.Append(tbName2Z.Text & "°") 'kunde
        sb2.Append("0" & "°")   'Person
        sb2.Append("0" & "°")  'Tiere
        sb2.Append(" " & "°")  'Art
        sb2.Append(" " & "°")  'Frü
        sb2.Append(" " & "°")  'kato
        sb2.Append("0" & "°")  'Preis
        sb2.Append("0" & "°")  'anzahlung
        sb2.Append(arT(20) & "°") 'werbung
        sb2.Append(" " & "°") 'Info
        sb2.Append("0" & "°") 'rechnung
        sb2.Append(fcUmDatum(Date.Today) & "°") 'buchdatum
        sb2.Append("0" & "°") 'sprache
        sb2.Append(" " & "°") 'Buchungstext ID
        sb2.Append(arT(24) & "°") 'Makrotext
        sb2.Append(" " & "°") 'R-ID
        sb2.Append(" " & "°") 'Rechnungsnummer
        sb2.Append("0" & "°") 'FPreis
        sb2.Append("0" & "°") 'GPreis
        sb2.Append("0" & "°") 'MwstU
        sb2.Append("0" & "°") 'MwstS
        sb2.Append("0" & "°") 'MwstG
        sb2.Append("0" & "°") 'GKU
        sb2.Append("0" & "°") 'GKS
        sb2.Append("0" & "°") 'GKG
        sb2.Append(tbRechSend.Text & "°")    'RD Send
        sb2.Append("0" & "°")    'bidIndex
        sb2.Append(sgRBID & "°")  'Referrens ID
        sb2.Append(" " & "°")     'RA ID
        sb2.Append(" " & "°")   'RA Datum
        sb2.Append("100" & "°")  'Storno
        sb2.Append("0" & "°")    'Summe
        sb2.Append(sPauch & "°") 'Pausch
        sb2.Append(sCode & "°")        'Code key
        sb2.Append(tbInternetNr.Text & "°")
        sb2.Append(fcUmZeit(tbAnZeit.Text) & "°")    'Summe
        sb2.Append(fcUmZeit(tbAbZeit.Text))        'Code key
        arValue = Split(sb2.ToString, "°")
        '   Call fcInsertCommand("Buchung", arFields, arValue)
        Dim cBedingung As String = " WHERE ID='" & ID & "'"
        Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
        '   ngIDB += 1


        Call prLadeGastDaten(sKNr)
    End Sub

    Private Sub tsbAufZimmer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAufZimmer.Click
        sgZNr = lbZimNr.Text
        frmZusatz.Show()
    End Sub

    Private Sub btBText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btBText.Click
        lLang = False
        If coLang.Text = "Englisch" Then lLang = True
        frmBText.Show()
        frmBText.tbBText.Text = lbMakro.Text
    End Sub

    Private Sub coLang_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles coLang.TextChanged
        lLang = False
        If coLang.Text = "Englisch" Then lLang = True
    End Sub

    ''' <summary>
    ''' Steuerung des Menues in Abhängigkeit des Rechnungsstatus
    ''' </summary>
    ''' <param name="lR"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 14.03.2012 Create
    ''' </remarks>
    Private Function fcChaneMenue(ByVal lR As Boolean) As Boolean
        fcChaneMenue = lR
        tsmStorno.Enabled = lR
        ' tsbBestätigung.Enabled = Not lR
        tsbDelReservierung.Enabled = Not lR
        tsbDelZimmer.Enabled = Not lR
        '    tsmAnAbReise.Enabled = Not lR
        '     tsbAufZimmer.Enabled = Not lR
        If lR Then
            Me.Text = "Reservierung bearbeiten => (Rechnung geschrieben!)"
        Else
            Me.Text = "Reservierung bearbeiten"
        End If
    End Function
    ''' <summary>
    ''' Code Speicher auf key.text
    ''' </summary>
    ''' <remarks>
    ''' 25.12.2014 Create
    ''' </remarks>
    Private Sub tsbKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbKey.Click
        'arNew = fcCollectDataInArray(arNew)
        Call prSaveZimmerReservierung()
        Dim sToDay As String = fcUmDatum(arNew(21)) 'VON
        Dim sToDay1 As String = fcUmDatum(CDate(DateAdd(DateInterval.Day, 1, CDate(arNew(22))))) 'BIS
        Dim C As String = fcCode(sCode) & "°" & sToDay & "°1200°" & sToDay1 & "°1200|"
        Dim a As String = "Code=" & C
        Dim sIP As String = cgIPWeb
        PHP.Data(C, sIP)
    End Sub



#End Region

#Region "Speichern der Reservierungsdaten.........................................................."

    ''' <summary>
    ''' Änderung an einer Reservierung speichern
    ''' </summary>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prSaveZimmerReservierung()
        Dim sKnr As String = lbGastID.Text
        If tbName1.Text.Trim = "" And tbName2.Text.Trim = "" Then
            MsgBox("Speichern nicht möglich, da kein Gast ausgewählt wurde!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        If tbAnzPer.Text.Trim = "" Or tbAnzPer.Text.Trim = "0" Then
            MsgBox("Speichern nicht möglich, da keine Personenzahl!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        arNew = fcCollectDataInArray(arNew)
        '  If fcDifference(arOld, arNew) Then
        sKnr = fcSaveGast(arNew, sKnr)
        Call prSavePreis(arNew, sKnr)
        Call prSaveRestZimmerOfReservierung(lUnbekannt, sKnr, sgRBID)
        Call prSaveCodeToErinnerung()
        'Refresh MainGrid
        Call prRefreshData()

        'If arOld(6).Trim = "" And arOld(7).Trim = "" Then
        '    sKnr = fcGetNr("KNr")
        '    prSetNr("KNr", sKnr)
        'End If
        lbGastID.Text = sKnr
        ' End If
        Call prSaveBuchungsText(sgRBID)
    End Sub

    ''' <summary>
    ''' Änderung am Buchungsdatensatz speichern
    ''' </summary>
    ''' <param name="arT">Änderungsdaten</param>
    ''' <param name="sKNr">Kundennummer</param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prSavePreis(ByVal arT() As String, ByVal sKNr As String)
        Dim sPauch As String = "0"
        If cbPausch.Checked = True Then sPauch = "1"
        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim sID As String = sKNr
        Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID") ' tscoZim.Text
        Dim sOID As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "IDObjekte") 'tscoZim.Text
        Dim cBedingung As String = " WHERE ID='" & sBID & "'"  'sBiD
        Dim sDatum As Date = lbAbreise.Text
        Dim sADatum As Date = DateAdd(DateInterval.Day, -1, sDatum)
        Dim sPreisS As String = "0"
        Dim sPreisG As String = "0"
        Dim sVariable As String = "0"
        If rbNormal.Checked = True Then sVariable = "0"
        If rbFest.Checked = True Then sVariable = "1"
        If rbVariabel.Checked = True Then sVariable = "2"
        Try

            sqlText = "Von,Bis,ObjID,ZimID,Variable,KunID,Name1,Name2,Personen,Tiere,Art,Frueh,Kategorie,Preis,Anzahlung,Werbung,Info,FPreis,Storno,Summe,Pausch,RDSenden,ZDatum,InternetNr,GPreis,MwstU,MwstS,MwstG,GKU,GKS,GKG,VonZeit,BisZeit,Code"
            arFields = Split(sqlText, ",")

            sb.Append(fcUmDatum(lbAnreise.Text) & "°")
            sb.Append(fcUmDatum(sADatum) & "°")
            sb.Append(sOID & "°")
            sb.Append(sZim & "°")
            sb.Append(sVariable & "°")           'Variabel/fest
            sb.Append(sKNr & "°")
            sb.Append(tbName1Z.Text & "°")
            sb.Append(tbName2Z.Text & "°")
            sb.Append(tbAnzPer.Text & "°")
            sb.Append("0" & "°")
            sb.Append(coArt.Text & "°")
            If coArt.Text = "Ü/F" Then
                sb.Append(tbAnzPer.Text & "°")

                sPreisS = ((Val(fcChangeString(tbFPreis.Text, ",", ".")) * 100) - arIni(24)).ToString ' "300"
                ' Dim n1 As Integer = Val(fcChangeString(tbFPreis.Text, ",", ".")) * 100
                ' sPreisS = arIni(17)
                sPreisG = arIni(24)
            Else
                sb.Append("0" & "°")
            End If
            sb.Append(tbUArt.Text & "°")

            Dim preisx As String = fcChangeString(tbPreis.Text, ",", ".")
            sb.Append((Val(fcChangeString(tbPreis.Text, ",", ".")) * 100).ToString & "°")
            sb.Append((Val(fcChangeString(tbAnzahlung.Text, ",", ".")) * 100).ToString & "°")
            sb.Append(sgWerbung & "°")
            sb.Append(tbInfo.Text & "°")
            sb.Append(sPreisS & "°")  'Frühstück
            sb.Append(tbStorno.Text & "°")
            sb.Append((Val(fcChangeString(tbSumme.Text, ",", ".")) * 100).ToString & "°")
            sb.Append(sPauch & "°")
            sb.Append(tbRechSend.Text & "°")
            Dim zDatum As String = ""
            If IsDate(tbRechSend.Text) Then
                zDatum = fcUmDatum(tbRechSend.Text)
            Else

                Select Case Val(tbRechSend.Text)
                Case Is <= 0
                    'Anreise datum
                    zDatum = fcUmDatum(CDate(arT(21)).AddDays(Val(tbRechSend.Text)))
                    '   Datum = CDate(fcUmDatum(von)).AddDays(nTage)
                Case Is > 0
                    'buchungsDatum

                    zDatum = fcUmDatum(CDate(Date.Today).AddDays(Val(tbRechSend.Text)))
                Case Else
                    'festdatum
                    If tbRechSend.Text.Length > 6 Then
                        zDatum = fcUmDatum(tbRechSend.Text)
                        ' datum mit Jahr
                    Else
                        'datum ohne jahr ->>
                        zDatum = fcUmDatum(Trim(tbRechSend.Text) & "." & (CDate(Date.Today).AddYears(1).Year.ToString))

                    End If

            End Select
            End If
            sb.Append(zDatum & "°")
            sb.Append(tbInternetNr.Text & "°")
            sb.Append(sPreisG & "°")
            sb.Append(sMwstU & "°")
            sb.Append(sMwstS & "°")
            sb.Append(sMwstG & "°")
            sb.Append(sGKU & "°")
            sb.Append(sGKS & "°")
            sb.Append(sGKG & "°")
            sb.Append(fcUmZeit(tbAnZeit.Text) & "°")    'Summe
            sb.Append(fcUmZeit(tbAbZeit.Text) & "°")
            sb.Append(sCode)

            arValue = Split(sb.ToString, "°")

            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)

            'Datensatz in DataTable "dtBuc" speichern
            cBedingung = "ID Like '" & sBID & "'" 'sbid
            Call fcUpdateTable(dtBuc, arFields, arValue, cBedingung)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)

        End Try

    End Sub

    ''' <summary>
    ''' Bei Erstanlage einer Buchung die Buchungsdaten auf alle Zimmer der Reservierung übertragen
    ''' </summary>
    ''' <param name="lUn"></param>
    ''' <param name="sKnr"></param>
    ''' <param name="sBID"></param>
    ''' <remarks>
    ''' 26.02.2012 Create
    ''' </remarks>
    Private Sub prSaveRestZimmerOfReservierung(ByVal lUn As Boolean, ByVal sKnr As String, ByVal sBID As String)
        If lUn Then
            Dim sb As New StringBuilder
            Dim sqlText As String = ""
            Dim arFields(0), arValue(0) As String
            Dim cBedingung As String = " WHERE BID='" & sBID & "'"
            Dim sDatum As Date = lbAbreise.Text
            Dim sADatum As Date = DateAdd(DateInterval.Day, -1, sDatum)

            sqlText = "Von,Bis,KunID,Kunde,Name1,Name2,Personen,Tiere,Art,Frueh,Kategorie,Preis,Werbung,InternetNr,VonZeit,BisZeit"
            arFields = Split(sqlText, ",")

            sb.Append(fcUmDatum(lbAnreise.Text) & "°")
            sb.Append(fcUmDatum(sADatum) & "°")
            sb.Append(sKnr & "°")
            sb.Append(tbName1.Text & "°")
            sb.Append(tbName1Z.Text & "°")
            sb.Append(tbName2Z.Text & "°")
            sb.Append(tbAnzPer.Text & "°")
            sb.Append("0" & "°")
            sb.Append(coArt.Text & "°")
            If coArt.Text = "Ü/F" Then
                sb.Append(tbAnzPer.Text & "°")
            Else
                sb.Append("0" & "°")
            End If
            sb.Append(tbUArt.Text & "°")
            sb.Append((Val(tbPreis.Text) * 100).ToString & "°")
            sb.Append(sgWerbung & "°")
            sb.Append(tbInternetNr.Text & "°")
            sb.Append(fcUmZeit(tbAnZeit.Text) & "°")    'Summe
            sb.Append(fcUmZeit(tbAbZeit.Text))
            arValue = Split(sb.ToString, "°")

            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
            cBedingung = "BID Like '" & sBID & "'"
            Call fcUpdateTable(dtBuc, arFields, arValue, cBedingung)
            Call prRefreshDataAll(sBID)

            lUnbekannt = False
        End If
    End Sub

    ''' <summary>
    ''' Daten des Gastes speichern
    ''' </summary>
    ''' <param name="arT"></param>
    ''' <param name="sKNr"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Function fcSaveGast(ByVal arT() As String, ByVal sKNr As String) As String
        Dim lNew As Boolean = False
        fcSaveGast = sKNr
        If Trim(sKNr) = "" Then
            lNew = True
            'Neuanlage des Gastes
        Else
            'Aktuallisierung der Daten
        End If

        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String = ""
        Dim sID As String = sKNr
        If lNew Then sID = fcAppendBlank("Kunden")





        fcSaveGast = sID
        Try
            sqlText = "ID,Anrede,Name1,Name2,Name1Z,Name2Z,Vorname,Strasse,PLZ,Ort,Land,Telefon,Telefax,Funk,EMail,Pass,Geb,Info,Werbung"
            arFields = Split(sqlText, ",")
            arValue = Split(sqlText, ",")
            arValue(0) = sID
            arValue(1) = coAnrede.Text
            arValue(2) = tbName1.Text
            arValue(3) = tbName2.Text
            arValue(4) = tbName1Z.Text
            arValue(5) = tbName2Z.Text
            arValue(6) = tbVorname.Text
            arValue(7) = tbStrasse.Text
            arValue(8) = tbPLZ.Text
            arValue(9) = tbOrt.Text
            arValue(10) = coLand.Text
            arValue(11) = tbTel.Text
            arValue(12) = tbFax.Text
            arValue(13) = tbFunk.Text
            arValue(14) = tbEMail.Text
            arValue(15) = ""
            arValue(16) = fcUmDatum(dpGeb.Value)
            arValue(17) = tbInfo.Text
            arValue(18) = coWerbung.Text


            '  sqlText = fcGetSQLTextGast(sID, arT)
            '  arValue = Split(sqlText, "°")
            'If lNew Then
            '    Call fcInsertCommand("Kunden", arFields, arValue)
            '    fcSaveGast = sID
            '    ' Call prSetNr("KNr", sID)
            'Else
            cBedingung = " WHERE ID='" & sID & "'"
            Call fcUpdateCommand("Kunden", arFields, arValue, cBedingung)
            'End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally
        End Try
    End Function

    ''' <summary>
    ''' Zu speichernde Daten aufbereiten
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 10.01..2012 Create
    ''' </remarks>
    Private Function fcGetSQLTextGast(ByVal sID As String, ByVal arT() As String) As String
        Dim sb As New StringBuilder
        Dim sGeb As String = " "
        For i As Integer = 5 To 19
            If arT(i).Trim = "" Then arT(i) = " "
        Next
        ' "ID,Anrede,Name1,Name2,Vorname,Strasse,PLZ,Ort,Land,Telefon,Telefax,Funk,EMail,Pass,Geb,Info"
        sb.Append(sID & "°")
        sb.Append(arT(5) & "°")
        sb.Append(arT(6) & "°")
        sb.Append(arT(7) & "°")
        sb.Append(arT(8) & "°")
        sb.Append(arT(9) & "°")
        sb.Append(arT(10) & "°")
        sb.Append(arT(11) & "°")
        sb.Append(arT(12) & "°")
        sb.Append(arT(13) & "°")
        sb.Append(arT(14) & "°")
        sb.Append(arT(15) & "°")
        sb.Append(arT(16) & "°")
        sb.Append(arT(17) & "°")
        sb.Append(arT(19) & "°")
        sb.Append(arT(18) & "°")
        sb.Append(sgWerbung)
        fcGetSQLTextGast = sb.ToString
    End Function

    ''' <summary>
    ''' Aktualisierung der Datem im Buchungs-Datagrid im Hauptfenster starten
    ''' </summary>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prRefreshData()
        If sIndexIst.Trim = "0" Then
            Dim cBedingung As String = " WHERE ID='" & sBID & "'"
            Dim sBO As Date = arOld(21)
            Dim sEO As Date = arOld(22)
            Dim sBN As Date = arNew(21)
            Dim sEN As Date = arNew(22)
            Dim sP As String = arNew(0)
            Dim sID As String = sgRBID
            Dim sName As String = arNew(6)
            Dim sFr As String = arNew(1)
            Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID")
            Call prDeleteReservierungInDataGrid(sBO, sEO, sZim)
            Call prSetReservierungInDataGrid(sBN, sEN, sZim, sP, sName, sID, sFr)
        End If

    End Sub

    ''' <summary>
    ''' Buchungstext und Sprache in all Reservierungen speichen
    ''' </summary>
    ''' <param name="sBID"></param>
    ''' <remarks>
    ''' 10.03.2012 Create
    ''' </remarks>
    Private Sub prSaveBuchungsText(ByVal sBID As String)
        Dim sBez As String = "0"
        If cbBezalt.Checked = True Then sBez = "1"
        Dim sID As String = " "
        Dim dt As DataTable = fcReadDataTable("Select ID from BTexte Where Name='" & coBText.Text & "'")
        If dt.Rows.Count <> 0 Then sID = dt.Rows(0).Item(0).ToString


        Dim sL As String = "0"
        sL = coLang.SelectedIndex.ToString
        If lLang Then sL = 1
        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String = " WHERE BID='" & sBID & "'"
        arFields = Split("Bez,Sprache,BText,MText", ",")

        sb.Append(sBez & "°")
        sb.Append(sL & "°")
        sb.Append(sID & "°")
        sb.Append(lbMakro.Text)
        arValue = Split(sb.ToString, "°")

        Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
        ' prCheckinMail("4")

        cBedingung = "BID Like '" & sBID & "'"
        Call fcUpdateTable(dtBuc, arFields, arValue, cBedingung)
    End Sub

    ''' <summary>
    ''' Bei Erstanlage einer Buchung alle Zimmer aktualisieren
    ''' </summary>
    ''' <param name="sBID"></param>
    ''' <remarks>
    ''' 26.02.2012 Create
    ''' </remarks>
    Private Sub prRefreshDataAll(ByVal sBID As String)
        Dim sBO As Date = lbAnreise.Text
        Dim sDatum As Date = lbAbreise.Text
        Dim sEO As Date = lbAbreise.Text 'DateAdd(DateInterval.Day, -1, sDatum)

        Dim sP As String = tbAnzPer.Text
        Dim sID As String
        Dim sName As String = tbName1.Text
        Dim sFr As String = coArt.Text
        Dim sSQL As String = "Select * From Buchung Where BID='" & sBID & "'"
        Dim dt As DataTable = fcReadDataTable(sSQL)
        If dt.Rows.Count > 0 Then

            Dim nMax As Integer = dt.Rows.Count - 1
            For i As Integer = 0 To nMax
                sID = dt.Rows(i).Item("ZimID").ToString.Trim()
                Call prDeleteReservierungInDataGrid(sBO, sEO, sID)
                Call prSetReservierungInDataGrid(sBO, sEO, sID, sP, sName, sBID, sFr)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Alte Reservierung aus dem Buchungs-Datagried löschen
    ''' </summary>
    ''' <param name="sBegin"></param>
    ''' <param name="sEnd"></param>
    ''' <param name="sZim"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prDeleteReservierungInDataGrid(ByVal sBegin As Date, ByVal sEnd As Date, ByVal sZim As String)
        'Liste  synchronisieren
        Dim nCol As Integer = fcGetZimmerSpalte(sZim, frmMain.dgBuchung)
        Dim nDay As Integer
        Dim sDay As Date
        With frmMain.dgBuchung
            nDay = .Rows.Count - 1
            For i = 0 To nDay
                sDay = .Rows(i).Cells(0).Value
                If sDay = sEnd Then
                    Exit For
                End If
                If sDay >= sBegin Then
                    .Rows(i).Cells(nCol).Value = ""
                    .Rows(i).Cells(nCol).Style.BackColor = Color.White
                End If
            Next
        End With
    End Sub

    ''' <summary>
    ''' Neue Reservierung eintragen
    ''' </summary>
    ''' <param name="sBegin"></param>
    ''' <param name="sEnd"></param>
    ''' <param name="sZim"></param>
    ''' <param name="sP"></param>
    ''' <param name="sName"></param>
    ''' <param name="sID"></param>
    ''' <param name="sFr"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Sub prSetReservierungInDataGrid(ByVal sBegin As Date, ByVal sEnd As Date, _
                                            ByVal sZim As String, ByVal sP As String, _
                                            ByVal sName As String, ByVal sID As String, _
                                            ByVal sFr As String)
        'Liste  synchronisieren
        Dim nCol As Integer = fcGetZimmerSpalte(sZim, frmMain.dgBuchung)
        Dim nDay As Integer
        Dim sDay As Date
        With frmMain.dgBuchung
            nDay = .Rows.Count - 1
            For i = 0 To nDay
                sDay = .Rows(i).Cells(0).Value
                If sDay = sEnd Then Exit For
                If sDay >= sBegin Then

                    If sFr = "Ü/F" Then
                        .Rows(i).Cells(nCol).Style.BackColor = Color.LimeGreen
                    Else
                        .Rows(i).Cells(nCol).Style.BackColor = Color.Turquoise
                    End If
                    .Rows(i).Cells(nCol).Value = PadR(sName, 15) & "(" & sID & ") [" & sP & "]"
                End If
            Next
        End With
    End Sub

    ''' <summary>
    '''  türcode in den Terminplan
    ''' </summary>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>

    Private Sub prSaveCodeToErinnerung()
        'Dim sID As String = ""
        'Dim sqlText As String = "Datum,TerminText,Termin,Zeit,Aktive,BID"

        'Dim sb As New StringBuilder
        'Dim arFields As Array
        'Dim arValue As Array
        'Dim sSQL As String = "Select * From Termine Where BID='" & sgRBID & "'"
        'Dim dt As DataTable = fcReadDataTable(sSQL)
        'For i = 0 To dt.Rows.Count - 1
        '    If Extract(dt.Rows(i).Item("TerminText").ToString.Trim(), "(", ")", 1) = sCode Then
        '        sID = dt.Rows(i).Item("ID").ToString.Trim()
        '    End If
        'Next
        'sb.Append(fcUmDatum(DateAdd(DateInterval.Day, -14, CDate(lbAnreise.Text))) & "°")
        'sb.Append(tbName1.Text & ", Tür Code senden [" & sCode & "]     (" & lbGastID.Text & ") " & "°")
        'sb.Append(fcUmDatum(lbAnreise.Text) & "°")
        'sb.Append("12:00:00" & "°")
        'sb.Append("1" & "°")
        'sb.Append(sgRBID)
        'arFields = Split(sqlText, ",")
        '' sqlText = fcGetSQLTextGast(sID, arT)
        'arValue = Split(sb.ToString, "°")
        ''  UPDATE Termine SET [Datum]='20150102',[TerminText]='Werel, Tür Code senden (21693)',[Termin]='20141118',[Zeit]='07:00:00',[Aktiv]='1',[BID]='05605' WHERE ID='20150102194601'
        'If sID = "" Then sID = fcAppendBlank("Termine")
        'Dim cBedingung As String = " WHERE ID='" & sID & "'"
        'Call fcUpdateCommand("Termine", arFields, arValue, cBedingung)
    End Sub

#End Region

#Region "Hilfetexte in der Statusleiste ausgeben..................................................."

    Private Sub lbAnreise_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbAnreise.MouseMove
        tssInfo.Text = "Anreisedatum (ab 12:00 Uhr)"
    End Sub

    Private Sub lbAbreise_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbAbreise.MouseMove
        tssInfo.Text = "Abreisedatum (bis 11:00 Uhr)"
    End Sub

    Private Sub lbTage_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbTage.MouseMove
        tssInfo.Text = "Anzahl der Übernachtungen"
    End Sub

    Private Sub lbZimNr_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbZimNr.MouseMove
        tssInfo.Text = "Zimmernummer"
    End Sub

    Private Sub lbAusstattung_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbAusstattung.MouseMove
        tssInfo.Text = "Ausstattung des Zimmers"
    End Sub

    Private Sub lbArt_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbArt.MouseMove
        tssInfo.Text = "Kategorie des Zimmers"
    End Sub

    Private Sub lbBetten_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbBetten.MouseMove
        tssInfo.Text = "Anzahl der möglichen Betten"
    End Sub

    Private Sub lbObjekt_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbObjekt.MouseMove
        tssInfo.Text = "In welchem Objekt ist das Zimmer"""
    End Sub

    Private Sub tbAnzPer_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbAnzPer.MouseMove
        tssInfo.Text = "Anzahl der Personen pro Zimmer"
    End Sub

    Private Sub tbUArt_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbUArt.MouseMove
        tssInfo.Text = "Übernachtungsart"
    End Sub

    Private Sub btUArt_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btUArt.MouseMove
        tssInfo.Text = "Auswahl der Übernachtungsart"
    End Sub

    Private Sub coArt_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles coArt.MouseMove
        tssInfo.Text = "Auswahl der Übernachtung mit und ohne Frühstück"
    End Sub

    Private Sub tbPreis_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbPreis.MouseMove
        tssInfo.Text = "Vereinbarter Preis pro Zimmer"
    End Sub

    Private Sub tbAnzahlung_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbAnzahlung.MouseMove
        tssInfo.Text = "Getätigte Anzahlung"
    End Sub

    Private Sub coAnrede_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles coAnrede.MouseMove
        tssInfo.Text = "Anrede: Herr, Frau, Firma"
    End Sub

    Private Sub tbName1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbName1.MouseMove
        tssInfo.Text = "Nachname / Firmenname"
    End Sub

    Private Sub tbName2_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbName2.MouseMove
        tssInfo.Text = "Nachname wenn Firmenbezug"
    End Sub

    Private Sub tbVorname_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbVorname.MouseMove
        tssInfo.Text = "Vorname"
    End Sub

    Private Sub tbStrasse_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbStrasse.MouseMove
        tssInfo.Text = "Strasse"
    End Sub

    Private Sub tbPLZ_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbPLZ.MouseMove
        tssInfo.Text = "PLZ"
    End Sub

    Private Sub tbOrt_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbOrt.MouseMove
        tssInfo.Text = "Ort"
    End Sub

    Private Sub coLand_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles coLand.MouseMove
        tssInfo.Text = "Land (Kurz-Zeichen)"
    End Sub

    Private Sub tbPass_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        tssInfo.Text = "Ausweisnummer"
    End Sub

    Private Sub dtpGeb_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dpGeb.MouseMove
        tssInfo.Text = "Geburtsdatum"
    End Sub

    Private Sub tbTelefon_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbTel.MouseMove
        tssInfo.Text = "Telefon"
    End Sub

    Private Sub tbTelefax_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbFax.MouseMove
        tssInfo.Text = "Telefax"
    End Sub

    Private Sub tbFunk_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbFunk.MouseMove
        tssInfo.Text = "Mobiltelefon"
    End Sub

    Private Sub tbEMail_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbEMail.MouseMove
        tssInfo.Text = "E-Mail Adresse"
    End Sub

    Private Sub tbInfo_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbInfo.MouseMove
        tssInfo.Text = "Informationen zum Gast"
    End Sub

    Private Sub btGast_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btGast.MouseMove
        tssInfo.Text = "Auswahl des Gastes über den Gastestamm"
    End Sub

    Private Sub coWerbung_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles coWerbung.MouseMove
        tssInfo.Text = "Über welchen Kanal ist der Gast auf uns aufmerksam geworden?"
    End Sub

    Private Sub lbGast_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbGast.MouseMove
        tssInfo.Text = "Mit Doppelklick werden die Daten des Gastes (im Formular) gelöscht. Neuanlage ist nun möglich."
    End Sub

    Private Sub tsbDelZimmer_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbDelZimmer.MouseHover
        tssInfo.Text = "Löschen der ausgewählten Reservierung / Zimmer"
    End Sub

    Private Sub tsbDelReservierung_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbDelReservierung.MouseHover
        tssInfo.Text = "Die gesamte Reservierung wird gelöscht (alle Zimmer)."
    End Sub

    ' Anzeige löschen
    Private Sub gbGast_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gbGast.MouseMove
        tssInfo.Text = ""
    End Sub
    Private Sub gbPersonen_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        tssInfo.Text = ""
    End Sub
    Private Sub gbZimmer_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gbZimmer.MouseMove
        tssInfo.Text = ""
    End Sub
    Private Sub gbDatum_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        tssInfo.Text = ""
    End Sub
    Private Sub tsMain_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tsMain.MouseMove
        tssInfo.Text = ""
    End Sub

#End Region

#Region "Reservierung / Zimmer löschen............................................................."

    ''' <summary>
    ''' Reservierung komplett löschen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.02.2012 Create
    ''' </remarks>
    Private Sub tsbDelReservierung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelReservierung.Click
        Dim arS() As String
        Dim sDatum As Date
        Dim sADatum As Date

        'Löschen aus der Tabelle "Buchen" (Buchungsnummer)

        Dim arT() As String = fcGetZimmerIDAusBuchung(sgRBID)
        If fcDelBuchung(sgRBID, "") Then
            'DataGrid aktualisieren (Buchungsnummer, Zimmernummern)
            Dim i As Integer
            Dim nMax As Integer = arT.Length - 1
            For i = 0 To nMax
                arS = arT(i).Split(";")
                sDatum = fcUmDatum(arS(2))
                sADatum = DateAdd(DateInterval.Day, 1, sDatum)
                Call prDeleteReservierungInDataGrid(fcUmDatum(arS(1)), sADatum, arS(0))
            Next
            'Modul schliessen
            Me.Close()
        End If


    End Sub

    ''' <summary>
    ''' Einzelnes Zimmer aus der Reservierung löschen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 26.02.2012 Create
    ''' </remarks>
    Private Sub tsbDelZimmer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDelZimmer.Click

        Dim sID As String = Extract(tscoZim.Text, "[", "]", 1)
        Dim sZim As String = fcGetObjektZimmerID(dtZim, lbZimNr.Text, "ID") ' tscoZim.Text
        'Löschen aus der Tabelle "Buchen" (Buchungsnummer, Zimmernummern)
        If fcDelBuchung(sgRBID, sID) Then
            'DataGrid aktualisieren (Buchungsnummer, Zimmernummern) 
            Call prDeleteReservierungInDataGrid(lbAnreise.Text, lbAbreise.Text, sZim)
            'Komboboxen aktualisieren
            tscoFreiZim.Items.Add(lbZimNr.Text)
            tscoZim.Items.Remove(tscoZim.SelectedItem)
            lStart = True
            If tscoZim.Items.Count > 1 Then tscoZim.SelectedIndex = 1
            'Wenn letztes Zimmer, dann Modul schliessen
            Dim sSql As String = "Select * FROM Buchung WHERE BID = '" & sgRBID & "'"
            Dim dt As DataTable = fcReadDataTable(sSql)
            If dt.Rows.Count = 0 Then Me.Close()
        End If

    End Sub


    ''' <summary>
    ''' Buchung löschen
    ''' </summary>
    ''' <param name="sBID"></param>
    ''' <param name="sZID"></param>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Function fcDelBuchung(ByVal sBID As String, Optional ByVal sID As String = "") As Boolean
        fcDelBuchung = False
        Dim sMsg As String = "Wollen Sie dieses Zimmer wirklich löschen?  "
        Dim cSql As String = "DELETE FROM Buchung WHERE BID = '" & sBID & "' and ID = '" & sID & "'"
        If sID = "" Then
            sMsg = "Wollen Sie diese Reservierung wirklich löschen?  "
            cSql = "DELETE FROM Buchung WHERE BID = '" & sBID & "'"
        End If
        If sBID = "" Then Exit Function
        If MsgBox(sMsg, vbExclamation + vbOKCancel, "Löschen") = MsgBoxResult.Ok Then
            'Datensatz per SQL aus der Tabelle löschen
            UpdateTable(cSql)
            'Änderung in DataTable "dtBuc" speichern
            Call fcDeleteTableRow(dtBuc, "BID = '" & sBID & "'")
            fcDelBuchung = True
        End If
    End Function

    ''' <summary>
    ''' ZimmerID und Zeitraum der Buchung laden
    ''' </summary>
    ''' <param name="sBID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 25.02.2012 Create
    ''' </remarks>
    Private Function fcGetZimmerIDAusBuchung(ByVal sBID As String) As Array
        Dim sSql As String = "Select * FROM Buchung WHERE BID = '" & sBID & "'"
        Dim dt As DataTable = fcReadDataTable(sSql)
        Dim i As Integer
        Dim arT(0) As String
        fcGetZimmerIDAusBuchung = arT
        If dt.Rows.Count = 0 Then Exit Function
        Dim nMax As Integer = dt.Rows.Count - 1
        ReDim arT(nMax)
        Try

            For i = 0 To nMax
                arT(i) = dt.Rows(i).Item("ZimID").ToString & ";" & _
                         dt.Rows(i).Item("Von").ToString & ";" & _
                         dt.Rows(i).Item("Bis").ToString
            Next
            fcGetZimmerIDAusBuchung = arT
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Function

#End Region

#Region "Rechnung stornieren......................................................................."

    Private Sub tsmStorno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmStorno.Click
        Dim sfeld As String
        Dim arfeld2() As String
        If MsgBox("Soll die Rechnung storniert werden? " & vbCrLf & "RNr.: " & sRNr, MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Storno") = MsgBoxResult.Yes Then
            Dim arFields(0), arValue(0) As String
            Dim cBedingung As String = " WHERE RID='" & sRNr & "'"
            arFields = Split("Rechnung,RID", ",")
            arValue = Split("0" & "°" & " ", "°")

            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
            cBedingung = "RID Like '" & sRNr & "'"
            Call fcUpdateTable(dtBuc, arFields, arValue, cBedingung)

            lgRech = fcChaneMenue(False)

            arfeld2 = fcDataSeek("select * From Datev Where RechNr ='", sRNr, 0, {"Id", "Name", "Rechnr", _
                                     "Umsatz1", "Mwst1", "MwstS1", "GKonto1", "Konto1", "Umsatz2", "Mwst2", _
                                     "MwstS2", "Gkonto2", "Konto2", "Datum", "BuchNr", "KunNr"})

            sfeld = arfeld2(6)
            arfeld2(6) = arfeld2(7)
            arfeld2(7) = sfeld
            sfeld = arfeld2(11)
            arfeld2(11) = arfeld2(12)
            arfeld2(12) = sfeld
            'datensatz nicht gefunfen
            arfeld2(1) = "Storno " + arfeld2(1)
            arfeld2(15) = "Storno"
            If arfeld2(0) <> " " Then
                arfeld2(0) = fcGetTimeID(Date.Today)
                For i = 1 To 15
                    If arfeld2(i) = "" Then arfeld2(i) = "-"
                Next
                Call fcInsertCommand("Datev", {"Id", "Name", "Rechnr", _
                                     "Umsatz1", "Mwst1", "MwstS1", "GKonto1", "Konto1", "Umsatz2", "Mwst2", _
                                     "MwstS2", "Gkonto2", "Konto2", "Datum", "BuchNr", "KunNr"}, arfeld2)

            End If

        End If

    End Sub

#End Region

#Region "Reservierungsbestätigung drucken.........................................................."

    Private Sub tsbBestätigung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBestätigung.Click
        Call prSaveZimmerReservierung()
        Call fcDruckBuchnung(sgRBID)
    End Sub

#End Region



#Region "Rechnung erstellen........................................................................"

    ''' <summary>
    ''' Rechnungsmodul aufrufen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 02.04.2012 Create
    ''' </remarks>
    Private Sub tsbRechnung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRechnung.Click
        prSaveZimmerReservierung()
        sgGID = lbGastID.Text
        frmRechnung.Show()
    End Sub
    Private Sub tsbAnzahlungRechnung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAnzahlungRechnung.Click
        prSaveZimmerReservierung()
        sgGID = lbGastID.Text
        frmAnzahlung.Show()
    End Sub

#End Region


#Region "Mit Enter weiter zum nächsten Feld........................................................"

    Private Sub tbName1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbName1.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbVorname.Select()
        End If
    End Sub
    Private Sub tbName2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbName2.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbVorname.Select()
        End If
    End Sub
    Private Sub tbVorname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbVorname.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbStrasse.Select()
        End If
    End Sub
    Private Sub tbStrasse_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbStrasse.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbPLZ.Select()
        End If
    End Sub
    Private Sub tbPLZ_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbPLZ.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbOrt.Select()
        End If
    End Sub
    Private Sub tbOrt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbOrt.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            coLand.Select()
        End If
    End Sub
    Private Sub coLand_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles coLand.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbTel.Select()
        End If
    End Sub
    Private Sub tbTel_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbTel.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbFax.Select()
        End If
    End Sub
    Private Sub tbFax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbFax.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbFunk.Select()
        End If
    End Sub
    Private Sub tbFunk_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbFunk.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbEMail.Select()
        End If
    End Sub

    Private Sub tbEMail_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbEMail.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            '  tbPass.Select()
        End If
    End Sub
    Private Sub tbPass_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            coWerbung.Select()
        End If
    End Sub

    Private Sub coWerbung_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles coWerbung.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            tbName1.Select()
        End If
    End Sub

#End Region


    Private Sub tsbErinnerung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbErinnerung.Click
        frmErinnerung.Show()
    End Sub

    Private Sub coArt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coArt.SelectedIndexChanged

        If coArt.Text = "Ü/F" Then
            tbFPreis.Visible = True
            tbFPreis.Text = (arIni(17) / 100) + (arIni(24) / 100) '"3,00"  früschtuckspreis pro person
        Else
            tbFPreis.Visible = False
            tbFPreis.Text = "0,00"
        End If
    End Sub

    Private Sub tbFPreis_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFPreis.LostFocus
        tbFPreis.Text = fcFormatDecimal(tbFPreis.Text)
    End Sub


    Private Sub tsbMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbMail.Click
        If FileExists(arIni(32) & "\Buchung\Buch_" & sgRBID & ".PDF") = False Then
            MsgBox("Buchungsbestätigung noch nicht Erstelt", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        Dim sText As Array = Split(ReadOneValueFromSystemDb("Mail"), "#")
        Dim sPdf As String = arIni(32) & "\Buchung\Buch_" & sgRBID & ".PDF"
        Dim arFeld(1) As String
        Dim arfeld1(1) As String
        Dim ii As Integer = 0
        Dim sKID As String
        Dim sName As String = ""
        arfeld1 = fcDataSeek("select * From Buchung Where BID ='", sgRBID, 0, {"KunID", "Sprache"})
        sKID = arfeld1(0)
        arFeld = fcDataSeek("select * From Kunden Where ID ='", sKID, 0, {"EMail", "Anrede", "Name1", "Vorname", "Strasse", "Ort", "plz", "Land"})
        ii = Val(arfeld1(1))
        If arFeld(0).Trim <> "" Then
            If fcSendeMailAnlage(arFeld(0), "Buchung", sText(ii), sDatei:=sPdf) = True Then
                MsgBox("Buchungs Mail Erfolgreich gesendet", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Else
                MsgBox(" F e h l e r  Buchungs Mail ", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                End If
            Else
                MsgBox(" F e h l e r  Keine Mailadresse", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
        End If
    End Sub
    Private Sub tsbBezahlt_Click(sender As Object, e As EventArgs) Handles tsbBezahlt.Click
        '  If MsgBox("Zahlungsbestätigung Senden?", MsgBoxStyle.Information + MsgBoxStyle.RetryCancel) = True Then
        If fcMailBezahlt(sgRBID) = True Then
            cbBezalt.Checked = True
        End If


    End Sub








    Private Sub coWerbung_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coWerbung.SelectedIndexChanged
        sgWerbung = coWerbung.Text
    End Sub





    Private Sub tsbBewertung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBewertung.Click
        Dim sTem As Array
        sTem = fcDataSeek("select * From Bewertung Where BuchID ='", sgRBID, 0, {"ID"}) 'kontrolle ob schon geschriben
        If sTem(0) = " " Then
            Call fcInsertCommand("Bewertung", {"Id", "BuchID", "Status"}, {fcGetTimeID(Date.Today), sgRBID, "-1"})
        End If
    End Sub

    Private Sub tbVorname_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbVorname.TextChanged
        Call prLoadGastInList()
        Call prFindGast()

    End Sub



    Private Sub tbName1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbName1.TextChanged
        Call prLoadGastInList()
        Call prFindGast()

    End Sub

    Private Sub coBText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles coBText.SelectedIndexChanged
        Dim n As String = coBText.SelectedIndex
        If tbRechSend.Text.Trim = "" Then tbRechSend.Text = arZZiel(n)
    End Sub


    Private Sub prCreatTabelIndex()
        Dim sSQL As String = "Select * From Buchung  Where IDRef='" & sIDB & "' order by BIDindex asc" ' & " And ZimID='" & sgRZID & "'"
        dtBuchIndex = fcReadDataTable(sSQL) ' order by Name1, Vorname asc

        If dtBuchIndex.Rows.Count - 1 = -1 Then   'wenn BIDRef ="XXXXX"
            Dim sqlText As String = "IDRef"
            Dim arfields() As String
            Dim arValue(0) As String
            Dim cBedingung As String
            arfields = Split(sqlText, ",")
            arValue(0) = sIDB
            cBedingung = " WHERE ID='" & sIDB & "'"
            Call fcUpdateCommand("Buchung", arfields, arValue, cBedingung)
            dtBuchIndex = fcReadDataTable(sSQL)
        End If

    End Sub

    Private Sub Summe(ByVal sRBID As String, ByVal sRZID As String)

        Dim dVon As Date = CDate(lbAnreise.Text)
        Dim dBis As Date = CDate(lbAbreise.Text)
        dBis = DateAdd(DateInterval.Day, 1, dBis)
        lbTage.Text = DateDiff(DateInterval.Day, dVon, dBis)

        Dim sPreisNacht As String = fcKillKomma(tbPreis.Text)
        Dim sSummeGesamt As String = fcKillKomma(tbSumme.Text)

        'lbSumme.Text = fcGetSummeZimmer(sRBID, sRZID)
        'If Val(Trim(tbSumme.Text)) = 0 Then
        '    tbSumme.Text = fcFormatDecimal(StrTrim(lbSumme.Text, "€"))
        'Else
        '    If Val(tbPreis.Text) <> 0 Then

        '        tbSumme.Text = fcFormatDecimal(Str(Val(sPreisNacht) * (Val(lbTage.Text) - 1)))
        '    End If
        'End If
        Dim nFPreis1 As String = "0"
        If coArt.Text = "Ü/F" Then
            nFPreis1 = tbFPreis.Text
        End If
        Dim p As String = fcGetZimPreis(sgRZID, tbAnzPer.Text & "/" & nFPreis1, lbAbreise.Text, lbAnreise.Text) 'Zimmerpreis nach tabelle
        lbPauschPreis.Text = fcFormatDecimal(StrTrim(p, "")) & " €"
        If cbPausch.Checked = True Then
            If Val(Trim(tbSumme.Text)) = 0 Then
                tbSumme.Text = fcFormatDecimal(StrTrim(p, "€"))
            Else
                tbSumme.Text = fcFormatDecimal(StrTrim(tbSumme.Text, "€"))
            End If
        Else
            tbSumme.Text = fcGetSummeZimmer(sRBID, sRZID).Trim

            'tbSumme.Text = fcFormatDecimal(StrTrim(tbSumme.Text, "€"))
        End If



    End Sub




    Private Function fcGetObjID(ByVal sZim As String) As String
        fcGetObjID = " "
        Dim nMax As Integer = dtZim.Rows.Count - 1
        For i As Integer = 0 To nMax
            If dtZim.Rows(i).Item("ID") = sZim Then
                fcGetObjID = dtZim.Rows(i).Item("IDObjekte")
                Exit For
            End If
        Next
    End Function
    Private Sub buBerechnung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buBerechnung.Click
        Call Summe(sgRBID, sgRZID)
    End Sub

    'Private Sub MwstHohlenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MwstHohlenToolStripMenuItem.Click
    '    sMwstG = arIni(10)
    '    sMwstS = arIni(23)
    '    sMwstU = arIni(11)
    '    sGKU = arIni(21)
    '    sGKS = arIni(25)
    '    sGKG = arIni(20)
    'End Sub


    Private Sub tsmtNeu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmtNeu.Click
        sMwstG = arIni(10)
        sMwstS = arIni(23)
        sMwstU = arIni(11)
        sGKG = arIni(21)
        sGKS = arIni(25)
        sGKU = arIni(20)
        tssSteuer.Text = sMwstU & "% / " & sMwstS & "% / " & sMwstG & "%"
        tssGKonto.Text = sGKU & " / " & sGKS & " / " & sGKG
        Call prSaveMwst()
    End Sub

    Private Sub tsmtAlt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmtAlt.Click
        sMwstG = arMwstAlt(2)
        sMwstS = arMwstAlt(1)
        sMwstU = arMwstAlt(0)
        sGKG = arGKAlt(2)
        sGKS = arGKAlt(1)
        sGKU = arGKAlt(0)
        tssSteuer.Text = sMwstU & "% / " & sMwstS & "% / " & sMwstG & "%"
        tssGKonto.Text = sGKU & " / " & sGKS & " / " & sGKG
        Call prSaveMwst()
    End Sub

    Private Sub tsmtAkt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmtAkt.Click
        sMwstG = sMwstGa
        sMwstS = sMwstSa
        sMwstU = sMwstUa
        sGKG = sGKGa
        sGKS = sGKSa
        sGKU = sGKUa
        tssSteuer.Text = sMwstU & "% / " & sMwstS & "% / " & sMwstG & "%"
        tssGKonto.Text = sGKU & " / " & sGKS & " / " & sGKG
        Call prSaveMwst()
    End Sub

    Private Sub tsmtNeu2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmtNeu2.Click
        sMwstG = arMwstNeu2(2)
        sMwstS = arMwstNeu2(1)
        sMwstU = arMwstNeu2(0)
        sGKG = arGKNeu2(2)
        sGKS = arGKNeu2(1)
        sGKU = arGKNeu2(0)
        tssSteuer.Text = sMwstU & "% / " & sMwstS & "% / " & sMwstG & "%"
        tssGKonto.Text = sGKU & " / " & sGKS & " / " & sGKG
        Call prSaveMwst()
    End Sub

    Private Sub tsmtNeu1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmtNeu1.Click
        sMwstG = arMwstNeu1(2)
        sMwstS = arMwstNeu1(1)
        sMwstU = arMwstNeu1(0)
        sGKG = arGKNeu1(2)
        sGKS = arGKNeu1(1)
        sGKU = arGKNeu1(0)
        tssSteuer.Text = sMwstU & "% / " & sMwstS & "% / " & sMwstG & "%"
        tssGKonto.Text = sGKU & " / " & sGKS & " / " & sGKG
        Call prSaveMwst()
    End Sub





    Private Sub prSaveMwst()

        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String
        Try
            cBedingung = " WHERE BID ='" & sgRBID & "'"
            sqlText = "MwstU,MwstS,MwstG,GKU,GKS,GKG"
            arFields = Split(sqlText, ",")
            sqlText = sMwstU & "°" & sMwstS & "°" & sMwstG & "°" & sGKU & "°" & sGKS & "°" & sGKG
            arValue = Split(sqlText, "°")
            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally


        End Try
    End Sub

    Private Sub PreisToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PreisToolStripMenuItem.Click
        Dim P As String = fcGetZimPreis(sgRZID, tbAnzPer.Text, lbAbreise.Text, lbAnreise.Text)
    End Sub

    Private Sub prCheckinMail(ByRef sNummer As String)

        Dim sb As New StringBuilder
        Dim sqlText As String = ""
        Dim arFields(0), arValue(0) As String
        Dim cBedingung As String
        Try
            cBedingung = " WHERE BID ='" & sgRBID & "'"
            sqlText = "SendMailZugang"
            arFields = Split(sqlText, ",")
            sqlText = sNummer
            arValue = Split(sqlText, "°")
            Call fcUpdateCommand("Buchung", arFields, arValue, cBedingung)
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        Finally


        End Try
    End Sub

    Private Sub tsbCheckin_Click(sender As Object, e As EventArgs) Handles tsbCheckin.Click
        frmCheckin.Show()

    End Sub
    Private Sub tbEMail_TextChanged(sender As Object, e As EventArgs) Handles tbEMail.MouseHover

        lbMailList.Visible = True
        lbMailList.Text = Replace(tbEMail.Text, ";", vbCrLf)
        lbMailList.Text = Replace(lbMailList.Text, " ", "")

    End Sub

    Private Sub tbEMail_TextChanged_1(sender As Object, e As EventArgs) Handles tbEMail.MouseLeave

        lbMailList.Visible = False
    End Sub
    Private Sub cbZusatz_CheckedChanged(sender As Object, e As EventArgs) Handles cbZusatz.CheckedChanged
        If cbZusatz.Checked = True Then
            gbNameZ.Visible = True
        Else
            gbNameZ.Visible = False
        End If
    End Sub


End Class
