Imports System
Public Class frmCorona
    Private Sub frmCorona_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sVon As String = fcUmDatum(Date.Today)
        Dim sSQL As String
        Dim dtB1 As DataTable
        Dim sName As String
        sSQL = "Select * from Buchung WHERE  Von >= '" & sVon & "' order by von asc"
        dtB1 = fcReadDataTable(sSQL)

        '  prCorona()
        prCreateTabelleCorona()
        prLoadCoronaInList(dtB1)

    End Sub
    ''' <summary>
    ''' Tabelle Corona erstellen
    ''' </summary>
    ''' <remarks>
    ''' 18.02.2012 Create
    ''' </remarks>
    Private Sub prCreateTabelleCorona()
        With lvCorona
            .Clear()

            .Columns.Add("Anreise", 75, HorizontalAlignment.Left)
            .Columns.Add("Name", 120, HorizontalAlignment.Left)
            .Columns.Add("Plz", 50, HorizontalAlignment.Left)
            .Columns.Add("Ort", 150, HorizontalAlignment.Left)
            .Columns.Add("Tele", 100, HorizontalAlignment.Left)
            .Columns.Add("Mail", 180, HorizontalAlignment.Left)
            .Columns.Add("Funk", 100, HorizontalAlignment.Left)
            .Columns.Add("Zimmer", 100, HorizontalAlignment.Left)
            .Columns.Add("Co", 20, HorizontalAlignment.Left)
            .FullRowSelect = True
            .GridLines = True
            .HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            .HideSelection = False
            .MultiSelect = False
            '      .Sorting = SortOrder.Ascending
            .TabIndex = 0
            .View = View.Details
        End With
    End Sub

    ''' <summary>
    ''' Tabelle Personal mit Daten aus der DataTabel "Personal" füllen
    ''' </summary>
    ''' <param name="dtT"></param>
    ''' <remarks>
    ''' 18.02.2012 Create
    ''' </remarks>
    Private Sub prLoadCoronaInList(ByVal dtT As DataTable)
        '   Dim DateiCSV As String = fcOrdnerLesen("C:\Users\S-Reich\Downloads\")
        Dim DateiCSV As String = fcOrdnerLesen("C:\Users\Stein\Downloads\")
        Dim arPLZ() As String = Split(ReadFileSeriell(DateiCSV), vbCrLf)
        Dim nMaxPLZ As Integer = arPLZ.Length - 1
        Dim arZe() As String
        Dim arAdresse() As String
        Dim sSQL As String = "Select * from Kunden  order by ID asc"
        Dim dtK1 As DataTable = fcReadDataTable(sSQL)
        Dim i As Integer
        lvCorona.Items.Clear()
        Dim nMax As Integer = dtT.Rows.Count - 1
        If nMax < 0 Then Exit Sub
        For i = 0 To nMax
            If dtT.Rows(i).RowState <> DataRowState.Deleted Then
                Dim lv As ListViewItem
                With lvCorona
                    lv = .Items.Add(fcUmDatum(dtT.Rows(i).Item("Von").ToString))
                    arAdresse = Split(fcGetKundeData(dtT.Rows(i).Item("KunID").ToString), "°")
                    lv.SubItems.Add(arAdresse(0))
                    lv.SubItems.Add(arAdresse(1))
                    lv.SubItems.Add(arAdresse(2))
                    lv.SubItems.Add(arAdresse(3))
                    lv.SubItems.Add(arAdresse(4))
                    lv.SubItems.Add(arAdresse(5))
                    lv.SubItems.Add(fcGetObjektZimmerName(dtZim, dtT.Rows(i).Item("ZimID").ToString))
                    For j = 0 To nMaxPLZ
                        If arPLZ(j).Trim <> "" Then
                            arZe = Split(arPLZ(j), ";")

                            If arZe(2).Trim = arAdresse(1).Trim Then
                                If arZe(2).Trim.Length = 4 Then arZe(2) = "0" & arZe(2)
                                lv.SubItems.Add("Yes")
                                lv.ForeColor = Color.Red
                            End If
                        End If
                    Next
                End With
            End If
        Next
        lvCorona.Select()
        If lvCorona.Items.Count > 0 Then lvCorona.TopItem.Selected = True
    End Sub
    Private Function fcGetKundeData(ByRef KID As String) As String
        Dim sSQL As String = "Select * from Kunden WHERE  ID = '" & KID & "' order by ID asc"
        Dim dtK1 As DataTable = fcReadDataTable(sSQL)
        fcGetKundeData = ""
        If dtK1.Rows.Count <> 0 Then
            fcGetKundeData = fcGetKundeData & dtK1.Rows(0).Item("Name1").ToString.Trim & "°"
            fcGetKundeData = fcGetKundeData & dtK1.Rows(0).Item("PLZ").ToString.Trim & "°"
            fcGetKundeData = fcGetKundeData & dtK1.Rows(0).Item("Ort").ToString.Trim & "°"
            fcGetKundeData = fcGetKundeData & dtK1.Rows(0).Item("Telefon").ToString.Trim & "°"
            fcGetKundeData = fcGetKundeData & dtK1.Rows(0).Item("EMail").ToString.Trim & "°"
            fcGetKundeData = fcGetKundeData & dtK1.Rows(0).Item("Funk").ToString.Trim
        Else
            fcGetKundeData = "°°°°°"
        End If
    End Function
    'Private Sub prCorona()
    '    Dim arPlZ() As String = Split(ReadFileSeriell("PLZ1.txt"), vbCrLf)
    '    Dim arZeile() As String
    '    Dim X As Integer = 5
    '    Dim Y As Integer = arPlZ.Length
    '    Dim arPLZNew(X, Y) As String
    '    For i = 0 To Y - 2
    '        arZeile = Split(arPlZ(i), ";")
    '        arPLZNew(0, i) = arZeile(0)
    '        arPLZNew(1, i) = arZeile(1)
    '        arPLZNew(2, i) = arZeile(2)
    '        arPLZNew(3, i) = arZeile(3)
    '        arPLZNew(4, i) = "NO"
    '    Next
    '    prCoronaSYS(arPLZNew, Y)
    'End Sub
    'Private Sub prCoronaSYS(ByVal z As Array, ByRef y As Integer)
    '    Dim c As String = ""
    '    Dim arCorona() As String = Split(ReadFileUniCode("corona.txt"), vbCrLf)
    '    Dim arC() As String
    '    Dim i As Integer
    '    For i = 0 To arCorona.Length - 1
    '        arC = Split(arCorona(i), ";")
    '        If arC(1).Trim = "Landkreis" Then
    '            arC(0) = "Landkreis " & arC(0)
    '        End If
    '        If arC(1).Trim = "Kreis" Then
    '            arC(0) = "Kreis " & arC(0)
    '        End If
    '        arCorona(i) = arC(0) & ";" & arC(1)
    '    Next


    '    Dim nMax As Integer = y - 2
    '    If nMax < 0 Then Exit Sub

    '    For i = 0 To nMax
    '        '   sZ = Split(arZ(i), ";")
    '        For ii = 0 To arCorona.Length - 1
    '            arC = Split(arCorona(ii), ";")
    '            If arC(1).Trim = "Landkreis" Or arC(1).Trim = "Kreis" Then
    '                If arC(0).Trim = z(2, i).Trim Then
    '                    z(4, i) = "CORONA"
    '                End If
    '            Else
    '                If arC(0).Trim = z(0, i).Trim And z(2, i).Trim = "" Then
    '                    z(4, i) = "CORONA"
    '                End If
    '            End If
    '        Next
    '        If z(4, i) = "CORONA" Then

    '            c = c & z(0, i) & ";" & z(1, i) & ";" & z(2, i) & ";" & z(3, i) & vbCrLf

    '        End If
    '    Next

    '    c = Mid(c, 1, c.Length - 2)
    '    WriteFileSeriell("CoronaPLZ.txt", c)
    'End Sub
    Private Function fcOrdnerLesen(ByRef Ph As String) As String
        Dim fs As Object
        Dim Folder As Object
        Dim Datei(500) As String
        Dim i As Integer = 0
        Dim r As String
        fs = CreateObject("Scripting.FileSystemObject")
        Folder = fs.GetFolder(Ph)
        For Each File In Folder.Files
            If File.Name Like "*.csv" Then

                Datei(i) = File.Name
                i = i + 1
                '  r = File.Name
                'mach was mit dem File
            End If
        Next
        r = ""
        For j = 0 To i - 1
            If Datei(j) > r Then

                r = Datei(j)
            End If
        Next
        fcOrdnerLesen = Ph & r
    End Function
  

End Class