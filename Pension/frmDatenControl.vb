Public Class frmDatenControl
    Dim sBIDg As String
    Dim nNr As Integer = 0
    Dim sNewID As String = ""
    Dim sOldID As String = ""
    Dim P As String = "1"
    Private Sub frmDatenControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sIP As String = cgIPWeb
        Dim arKunde As Array = PHP.Kunde(sIP, P)
        UpdateTable("DELETE FROM Buchung WHERE KunID ='0'")   'löschen aller buchungsätze ohne kunde
        Call prCreateTable()
        Call prFillTabelle(arKunde)
        '  Dim dStart As Date = DateAdd(DateInterval.Year, -1, Date.Today)
        '  tbStart.Text = CDate(dStart)
    End Sub
    ''' <summary>
    ''' Tabelle der zu sendenden EMails erstellen
    ''' </summary>
    ''' <remarks>
    ''' 17.02.2012 Create
    ''' </remarks>
    Private Sub prCreateTable()
        Dim nWidth As Integer
        With lvControll
            nWidth = (.Width - 80) / 7
            .Clear()
            .Columns.Add("Nummer", 30, HorizontalAlignment.Left)
            .Columns.Add("Name", nWidth, HorizontalAlignment.Left)
            .Columns.Add("Name2", nWidth, HorizontalAlignment.Left)
            .Columns.Add("Vorname", nWidth, HorizontalAlignment.Left)
            .Columns.Add("PLZ", 50, HorizontalAlignment.Left)
            .Columns.Add("Ort", nWidth, HorizontalAlignment.Left)
            .Columns.Add("Straße", nWidth, HorizontalAlignment.Left)
            .Columns.Add("EMail", nWidth, HorizontalAlignment.Left)
            .Columns.Add("ID", nWidth, HorizontalAlignment.Left)
            .FullRowSelect = True
            .GridLines = True
            '.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            '.Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub
    Private Sub prFillTabelle(ByRef arKunde As Array)
        Dim arKun() As String
        Dim nNummer As Integer = 1
        Dim lv As ListViewItem
        For i = 2 To arKunde.Length - 1
            If arKunde(i) = "#" Then
                With lvControll
                    nNummer = nNummer + 1
                    lv = .Items.Add("") 'Name1
                End With

            Else
                If arKunde(i) <> "" Then
                    arKun = Split(arKunde(i), ";")
                    With lvControll

                        lv = .Items.Add(nNummer) 'nummer
                        lv.SubItems.Add(arKun(0)) 'name1 
                        lv.SubItems.Add(arKun(1)) 'name2 
                        lv.SubItems.Add(arKun(2)) 'vorname 
                        lv.SubItems.Add(arKun(3))  'PLZ
                        lv.SubItems.Add(arKun(4))  'ort
                        lv.SubItems.Add(arKun(5))  'Strasse
                        lv.SubItems.Add(arKun(6))  'mail
                        lv.SubItems.Add(arKun(7))  'ID

                    End With
                End If
            End If
        Next

    End Sub
    Private Sub lvControll_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvControll.SelectedIndexChanged
       
        With lvControll
            If .SelectedItems.Count <> 0 Then
                nNr = .SelectedItems(0).SubItems(0).Text
                sOldID = .SelectedItems(0).SubItems(8).Text
            End If



        End With
    End Sub
   


    Private Sub tbDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbDel.Click
        With lvControll


            For i = 0 To lvControll.Items.Count - 1
                If .Items(i).SubItems(0).Text <> "" Then
                    If nNr = Val(.Items(i).SubItems(0).Text) And sOldID <> .Items(i).SubItems(8).Text Then
                        sNewID = .Items(i).SubItems(8).Text
                    End If

                End If
            Next

        End With
        Dim arFields(0), arValue(0) As String
        arFields = Split("KunID", ",")
        arValue = Split(sNewID, "°")
        Dim cBedingung As String = " WHERE KunID='" & sOldID & "'"
        Call fcUpdateCommand("buchung", arFields, arValue, cBedingung)
        UpdateTable("DELETE FROM Kunden WHERE ID ='" & sOldID & "'")
        lvControll.Clear()
        Dim sIP As String = cgIPWeb
        Dim arKunde As Array = PHP.Kunde(sIP, P)

        Call prCreateTable()
        Call prFillTabelle(arKunde)

        '  UpdateTable("DELETE FROM Buchung WHERE BID ='" & sBIDg & "'")   'löschen der Buchung

    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged

        P = "1"
       

        Dim sIP As String = cgIPWeb
        Dim arKunde As Array = PHP.Kunde(sIP, P)

        Call prCreateTable()
        Call prFillTabelle(arKunde)
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        P = "0"


        Dim sIP As String = cgIPWeb
        Dim arKunde As Array = PHP.Kunde(sIP, P)

        Call prCreateTable()
        Call prFillTabelle(arKunde)
    End Sub
End Class