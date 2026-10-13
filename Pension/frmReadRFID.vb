Public Class frmReadRFID
    Dim sTmp As String
    Private Sub ReadRFID_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Me.Cursor = Cursors.WaitCursor
        'sTmp = PHP.PHPnoF("http://" & arIni(34), "POST", "Daten=Speicher_?")
        'Dim arTemp2() As String = Split(sTmp, "#")
        'lbDatum.Text = fcUmDatum(arTemp2(0))
        'lbZeit.Text = Mid(arTemp2(1), 1, 2) & ":" & Mid(arTemp2(1), 3, 2)

        '' PHP.Data("Speicher_Del", arIni(34))

        prCreateTabelleRFID()
        rbTrue.Checked = True
        'prLoadRFIDInList(arTemp2(2))
        'prSeekKunde()
        'Me.Cursor = Cursors.Arrow

    End Sub
    Private Sub prCreateTabelleRFID()
        With lvRFID
            .Clear()
            .Columns.Add("Code", 150, HorizontalAlignment.Left)
            .Columns.Add("Von", 150, HorizontalAlignment.Left)
            .Columns.Add("Zeit", 50, HorizontalAlignment.Left)
            .Columns.Add("Bis", 150, HorizontalAlignment.Left)
            .Columns.Add("Zeit", 50, HorizontalAlignment.Left)
            .Columns.Add("Name", 150, HorizontalAlignment.Left)
            .Columns.Add("Zimmer", 100, HorizontalAlignment.Left)
            .Columns.Add("T/F", 50, HorizontalAlignment.Left)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            ' .Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub

    ''' <summary>
    ''' Tabelle Zimmer mit daten aus der DataTabel "Zimmer" füllen
    ''' </summary>
    ''' <param name="dtT"></param>
    ''' <remarks>
    ''' 19.12.2011 Create
    ''' </remarks>
    Private Sub prLoadRFIDInList(ByVal sT As String)
        Dim arTmp() As String = Split(sT, "|")
        Dim arName() As String
        Dim i As Integer
        Dim nMax As Integer = arTmp.Length - 1
        If nMax = 0 Then Exit Sub
        lvRFID.Items.Clear()
        Dim arZeile() As String
        Dim sCode As String
        Dim sVon As String
        Dim sVonZeit As String
        Dim sBis As String
        Dim sBisZeit As String
        Dim sName As String
        For i = 0 To nMax - 1
            arZeile = Split(arTmp(i), ";")
            sCode = ""
            For ii = 0 To 4
                sCode = sCode + arZeile(ii) + ";"
            Next

            sCode = fcUmCode(Mid(sCode, 1, sCode.Length - 1))
            sVon = ""
            For ii = 5 To 8
                sVon = sVon + fcUmRFID(arZeile(ii))
            Next
            sBis = ""
            For ii = 11 To 14
                sBis = sBis + fcUmRFID(arZeile(ii))
            Next

            arName = Split(fcSeekKunde(sCode), "°")




            Dim lv As ListViewItem
            With lvRFID
                lv = .Items.Add(sCode)
                lv.SubItems.Add(fcUmDatum(sVon))
                lv.SubItems.Add(fcUmRFID(arZeile(9)) & "." & fcUmRFID(arZeile(10)))
                lv.SubItems.Add(fcUmDatum(sBis))
                lv.SubItems.Add(fcUmRFID(arZeile(15)) & "." & fcUmRFID(arZeile(16)))
                lv.SubItems.Add(arName(0))    'Name
                lv.SubItems.Add(arName(1))    'Zimmer
                lv.SubItems.Add(arZeile(17))
            End With
        Next


    End Sub
    Private Function fcSeekKunde(ByRef sCode As String) As String
        Dim dtP As DataTable
        Dim dtB As DataTable
        Dim dtC As DataTable
        Dim sName As String
        Dim sZID As String

        dtP = fcReadDataTable("SELECT * from Personal1 Where Code='" & sCode & "'")
        If dtP.Rows.Count <> 0 Then
            sName = dtP.Rows(0).Item("Name").ToString
            sName = sName & ";" & dtP.Rows(0).Item("Vorname").ToString
            sName = sName & "° "
        Else
            dtC = fcReadDataTable("Select * from code Where code='" & sCode & "'")
            If dtC.Rows.Count <> 0 Then
                sName = dtC.Rows(0).Item("BID").ToString
                If IsNumeric(sName) = True Then
                    'bid'
                    dtB = fcReadDataTable("Select * from Buchung Where BID='" & sName & "'")
                    If dtB.Rows.Count <> 0 Then
                        sZID = dtB.Rows(0).Item("ZimID").ToString
                        sName = dtB.Rows(0).Item("Kunde").ToString
                        sName = sName & "°" & fcGetObjektZimmerName(dtZim, sZID)
                    Else
                        sName = "x°x"
                    End If
                Else
                    sName = " °" & sName
                End If
            Else
                sName = "x°x"
            End If
        End If
        fcSeekKunde = sName
    End Function
    Private Function fcUmRFID(ByRef z As String) As String
        If z.Length = 1 Then z = "0" & z
        fcUmRFID = z
    End Function

    Private Sub rbTrue_CheckedChanged(sender As Object, e As EventArgs) Handles rbTrue.CheckedChanged
        If rbAll.Checked = False Then
            Me.Cursor = Cursors.WaitCursor

            sTmp = PHP.DataRead("http://" & cgIPWeb & "/SQLReadCode.php", "Speicher_?")
            '  sTmp = PHP.PHPnoF("http://" & arIni(34), "POST", "/Speicher_T")
            '  sTmp = PHP.PHPnoF("http://" & arIni(34), "POST", "Daten=Speicher_?")
            If sTmp = "ERROR" Or sTmp = "" Then Exit Sub
            Dim arTemp2() As String = Split(sTmp, "#")
            ' LoadURL(arIni(34) & "/?Speicher_T")
            'sTmp = WebBrowser1.DocumentText.ToString
            'Dim arTemp2() As String = Split(sTmp, "#")

            lbDatum.Text = fcUmDatum(arTemp2(0))
            lbZeit.Text = Mid(arTemp2(1), 1, 2) & ":" & Mid(arTemp2(1), 3, 2)
            prLoadRFIDInList(arTemp2(2))
            ' prSeekKunde()
            Me.Cursor = Cursors.Arrow
        End If

    End Sub

    Private Sub rbAll_CheckedChanged(sender As Object, e As EventArgs) Handles rbAll.CheckedChanged
        If rbTrue.Checked = False Then
            Me.Cursor = Cursors.WaitCursor
            sTmp = PHP.DataRead("http://" & cgIPWeb & "/SQLReadCode.php", "Speicher_All")
            ' sTmp = PHP.PHPnoF("http://" & arIni(34), "POST", "Speicher_All")
            If sTmp = "ERROR" Or sTmp = "" Then Exit Sub
            Dim arTemp2() As String = Split(sTmp, "#")
            lbDatum.Text = fcUmDatum(arTemp2(0))
            lbZeit.Text = Mid(arTemp2(1), 1, 2) & ":" & Mid(arTemp2(1), 3, 2)
            prLoadRFIDInList(arTemp2(2))
            '  prSeekKunde()
            Me.Cursor = Cursors.Arrow
        End If
    End Sub
    Private Function LoadURL(ByVal sURL As String) As Boolean
        Dim nTimeout As Integer
        Dim vStart As Date
        Dim bResult As Boolean = True

        Me.Cursor = Cursors.WaitCursor
        With WebBrowser1
            ' URL übergeben
            .Navigate(sURL)

            ' Warten, bis vollständig geladen
            ' Timeout auf 30 Sek. festlegen
            nTimeout = 30
            vStart = Now

            Do While .ReadyState <> WebBrowserReadyState.Complete
                Application.DoEvents()
                ' Timeout ?
                If DateDiff(DateInterval.Second, vStart, Now) > nTimeout Then
                    bResult = False
                    Exit Do
                End If
            Loop
        End With
        Me.Cursor = Cursors.Default

        Return bResult
    End Function
    Private Sub buSuch_Click(sender As Object, e As EventArgs) Handles buSuch.Click
        Dim sRFID As String = fcWeg34(tbRFID.Text)
        tbRFID.Text = sRFID
        Dim nMax As Integer = lvRFID.Items.Count - 1
        Dim lv As ListViewItem
        With lvRFID
            For i = 0 To nMax
                .Items(i).Selected = True
                If .SelectedItems(0).SubItems(0).Text = sRFID Then
                    .Items(i).BackColor = Color.Cyan
                Else
                    .Items(i).BackColor = Color.White
                End If
            Next
        End With
        tbRFID.Text = ""
    End Sub


End Class