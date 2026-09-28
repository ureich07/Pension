<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.dgBuchung = New System.Windows.Forms.DataGridView()
        Me.cmMain = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsS1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsS2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.tscBuchnung = New System.Windows.Forms.ToolStripComboBox()
        Me.tsmInset = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.tstAnreise = New System.Windows.Forms.ToolStripTextBox()
        Me.tstEnd = New System.Windows.Forms.ToolStripTextBox()
        Me.tsS3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmDruckAnAb = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tscCheckin = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmChekin1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmCheckin2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmCheckin3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmCheckin4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmCheckin5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmChekinNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.tssLog = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tssInfo = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tssDatum = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tssUhr = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tssNet = New System.Windows.Forms.ToolStripStatusLabel()
        Me.paDaten = New System.Windows.Forms.Panel()
        Me.lvBuchError = New System.Windows.Forms.ListView()
        Me.lbCheckin = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lbCode = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lbEMail = New System.Windows.Forms.Label()
        Me.lbFunk = New System.Windows.Forms.Label()
        Me.lbFax = New System.Windows.Forms.Label()
        Me.lbTel = New System.Windows.Forms.Label()
        Me.lbLand = New System.Windows.Forms.Label()
        Me.lbOrt = New System.Windows.Forms.Label()
        Me.lbPLZ = New System.Windows.Forms.Label()
        Me.lbStr = New System.Windows.Forms.Label()
        Me.lbVorname = New System.Windows.Forms.Label()
        Me.lbName2 = New System.Windows.Forms.Label()
        Me.lbName = New System.Windows.Forms.Label()
        Me.lbAnrede = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tiUhr = New System.Windows.Forms.Timer(Me.components)
        Me.tiWait = New System.Windows.Forms.Timer(Me.components)
        Me.tsMain = New System.Windows.Forms.ToolStrip()
        Me.tsbClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbKunde = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbStatistik = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbPersonen = New System.Windows.Forms.ToolStripSplitButton()
        Me.tsbEinteilung = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbPersonal = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDatev = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDruck = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSuchen = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSystem = New System.Windows.Forms.ToolStripSplitButton()
        Me.tsmSystem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmRestore = New System.Windows.Forms.ToolStripMenuItem()
        Me.DatenKorekturToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DatenKontrolleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IDKundeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ServerFehlerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IniToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsVersion = New System.Windows.Forms.ToolStripLabel()
        Me.tsbLogin = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbTermine = New System.Windows.Forms.ToolStripButton()
        Me.tsbCorona = New System.Windows.Forms.ToolStripButton()
        Me.tsbRFID = New System.Windows.Forms.ToolStripButton()
        Me.tslDatum = New System.Windows.Forms.ToolStripLabel()
        Me.tslTabelle = New System.Windows.Forms.ToolStripLabel()
        Me.tslGesammt = New System.Windows.Forms.ToolStripLabel()
        Me.tslSatznummer = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.tsbSchloss = New System.Windows.Forms.ToolStripButton()
        Me.paDatum = New System.Windows.Forms.Panel()
        Me.MonthCalendar1 = New System.Windows.Forms.MonthCalendar()
        Me.tsmCheckOut = New System.Windows.Forms.ToolStripMenuItem()
        Me.trTransponder = New System.Windows.Forms.Timer(Me.components)
        CType(Me.dgBuchung, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmMain.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.paDaten.SuspendLayout()
        Me.tsMain.SuspendLayout()
        Me.paDatum.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgBuchung
        '
        Me.dgBuchung.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgBuchung.ContextMenuStrip = Me.cmMain
        Me.dgBuchung.Location = New System.Drawing.Point(10, 34)
        Me.dgBuchung.Name = "dgBuchung"
        Me.dgBuchung.Size = New System.Drawing.Size(766, 419)
        Me.dgBuchung.TabIndex = 1
        '
        'cmMain
        '
        Me.cmMain.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.cmMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmNew, Me.tsS1, Me.tsmEdit, Me.tsS2, Me.tsmCopy, Me.tscBuchnung, Me.tsmInset, Me.ToolStripSeparator15, Me.tsmDelete, Me.tsmInfo, Me.ToolStripSeparator17, Me.tstAnreise, Me.tstEnd, Me.tsS3, Me.ToolStripSeparator16, Me.tsmDruckAnAb, Me.ToolStripSeparator4, Me.tscCheckin, Me.ToolStripSeparator11, Me.tsmChekin1, Me.tsmCheckin2, Me.tsmCheckin3, Me.tsmCheckin4, Me.tsmCheckin5, Me.tsmChekinNew})
        Me.cmMain.Name = "cmMain"
        Me.cmMain.Size = New System.Drawing.Size(261, 437)
        '
        'tsmNew
        '
        Me.tsmNew.Name = "tsmNew"
        Me.tsmNew.Size = New System.Drawing.Size(260, 22)
        Me.tsmNew.Text = "Neu"
        '
        'tsS1
        '
        Me.tsS1.Name = "tsS1"
        Me.tsS1.Size = New System.Drawing.Size(257, 6)
        '
        'tsmEdit
        '
        Me.tsmEdit.Name = "tsmEdit"
        Me.tsmEdit.Size = New System.Drawing.Size(260, 22)
        Me.tsmEdit.Text = "Bearbeiten"
        '
        'tsS2
        '
        Me.tsS2.Name = "tsS2"
        Me.tsS2.Size = New System.Drawing.Size(257, 6)
        '
        'tsmCopy
        '
        Me.tsmCopy.Name = "tsmCopy"
        Me.tsmCopy.Size = New System.Drawing.Size(260, 22)
        Me.tsmCopy.Text = "Kopieren"
        '
        'tscBuchnung
        '
        Me.tscBuchnung.DropDownWidth = 200
        Me.tscBuchnung.Name = "tscBuchnung"
        Me.tscBuchnung.Size = New System.Drawing.Size(200, 23)
        '
        'tsmInset
        '
        Me.tsmInset.Enabled = False
        Me.tsmInset.Name = "tsmInset"
        Me.tsmInset.Size = New System.Drawing.Size(260, 22)
        Me.tsmInset.Text = "Einfügen"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(257, 6)
        '
        'tsmDelete
        '
        Me.tsmDelete.Name = "tsmDelete"
        Me.tsmDelete.Size = New System.Drawing.Size(260, 22)
        Me.tsmDelete.Text = "Löschen"
        '
        'tsmInfo
        '
        Me.tsmInfo.Name = "tsmInfo"
        Me.tsmInfo.Size = New System.Drawing.Size(260, 22)
        Me.tsmInfo.Text = "Info"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(257, 6)
        '
        'tstAnreise
        '
        Me.tstAnreise.Enabled = False
        Me.tstAnreise.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.tstAnreise.Name = "tstAnreise"
        Me.tstAnreise.Size = New System.Drawing.Size(120, 23)
        '
        'tstEnd
        '
        Me.tstEnd.Enabled = False
        Me.tstEnd.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.tstEnd.Name = "tstEnd"
        Me.tstEnd.Size = New System.Drawing.Size(120, 23)
        '
        'tsS3
        '
        Me.tsS3.Name = "tsS3"
        Me.tsS3.Size = New System.Drawing.Size(257, 6)
        Me.tsS3.Visible = False
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(257, 6)
        '
        'tsmDruckAnAb
        '
        Me.tsmDruckAnAb.Name = "tsmDruckAnAb"
        Me.tsmDruckAnAb.Size = New System.Drawing.Size(260, 22)
        Me.tsmDruckAnAb.Text = "Druck An/Ab"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(257, 6)
        '
        'tscCheckin
        '
        Me.tscCheckin.Name = "tscCheckin"
        Me.tscCheckin.Size = New System.Drawing.Size(260, 22)
        Me.tscCheckin.Text = "Chekin Info"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(257, 6)
        '
        'tsmChekin1
        '
        Me.tsmChekin1.Name = "tsmChekin1"
        Me.tsmChekin1.Size = New System.Drawing.Size(260, 22)
        Me.tsmChekin1.Text = "Checkin Start"
        '
        'tsmCheckin2
        '
        Me.tsmCheckin2.Enabled = False
        Me.tsmCheckin2.Name = "tsmCheckin2"
        Me.tsmCheckin2.Size = New System.Drawing.Size(260, 22)
        Me.tsmCheckin2.Text = "Checkin in Arbeit"
        '
        'tsmCheckin3
        '
        Me.tsmCheckin3.Name = "tsmCheckin3"
        Me.tsmCheckin3.Size = New System.Drawing.Size(260, 22)
        Me.tsmCheckin3.Text = "Checkin beendet"
        '
        'tsmCheckin4
        '
        Me.tsmCheckin4.Name = "tsmCheckin4"
        Me.tsmCheckin4.Size = New System.Drawing.Size(260, 22)
        Me.tsmCheckin4.Text = "Zugang Gesendet"
        '
        'tsmCheckin5
        '
        Me.tsmCheckin5.Name = "tsmCheckin5"
        Me.tsmCheckin5.Size = New System.Drawing.Size(260, 22)
        Me.tsmCheckin5.Text = "Code Gesendet"
        '
        'tsmChekinNew
        '
        Me.tsmChekinNew.Name = "tsmChekinNew"
        Me.tsmChekinNew.Size = New System.Drawing.Size(260, 22)
        Me.tsmChekinNew.Text = "Checkin NeuStart"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tssLog, Me.tssInfo, Me.tssDatum, Me.tssUhr, Me.tssNet})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 514)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(984, 22)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'tssLog
        '
        Me.tssLog.Name = "tssLog"
        Me.tssLog.Size = New System.Drawing.Size(47, 17)
        Me.tssLog.Text = "Log = 0"
        '
        'tssInfo
        '
        Me.tssInfo.Name = "tssInfo"
        Me.tssInfo.Size = New System.Drawing.Size(853, 17)
        Me.tssInfo.Spring = True
        Me.tssInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tssDatum
        '
        Me.tssDatum.Name = "tssDatum"
        Me.tssDatum.Size = New System.Drawing.Size(43, 17)
        Me.tssDatum.Text = "Datum"
        '
        'tssUhr
        '
        Me.tssUhr.Name = "tssUhr"
        Me.tssUhr.Size = New System.Drawing.Size(26, 17)
        Me.tssUhr.Text = "Uhr"
        '
        'tssNet
        '
        Me.tssNet.BackgroundImage = CType(resources.GetObject("tssNet.BackgroundImage"), System.Drawing.Image)
        Me.tssNet.Name = "tssNet"
        Me.tssNet.Size = New System.Drawing.Size(0, 17)
        '
        'paDaten
        '
        Me.paDaten.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.paDaten.Controls.Add(Me.lvBuchError)
        Me.paDaten.Controls.Add(Me.lbCheckin)
        Me.paDaten.Controls.Add(Me.Label15)
        Me.paDaten.Controls.Add(Me.lbCode)
        Me.paDaten.Controls.Add(Me.Label13)
        Me.paDaten.Controls.Add(Me.Panel1)
        Me.paDaten.Controls.Add(Me.lbEMail)
        Me.paDaten.Controls.Add(Me.lbFunk)
        Me.paDaten.Controls.Add(Me.lbFax)
        Me.paDaten.Controls.Add(Me.lbTel)
        Me.paDaten.Controls.Add(Me.lbLand)
        Me.paDaten.Controls.Add(Me.lbOrt)
        Me.paDaten.Controls.Add(Me.lbPLZ)
        Me.paDaten.Controls.Add(Me.lbStr)
        Me.paDaten.Controls.Add(Me.lbVorname)
        Me.paDaten.Controls.Add(Me.lbName2)
        Me.paDaten.Controls.Add(Me.lbName)
        Me.paDaten.Controls.Add(Me.lbAnrede)
        Me.paDaten.Controls.Add(Me.Label12)
        Me.paDaten.Controls.Add(Me.Label11)
        Me.paDaten.Controls.Add(Me.Label10)
        Me.paDaten.Controls.Add(Me.Label9)
        Me.paDaten.Controls.Add(Me.Label8)
        Me.paDaten.Controls.Add(Me.Label7)
        Me.paDaten.Controls.Add(Me.Label3)
        Me.paDaten.Controls.Add(Me.Label5)
        Me.paDaten.Controls.Add(Me.Label4)
        Me.paDaten.Controls.Add(Me.Label6)
        Me.paDaten.Controls.Add(Me.Label2)
        Me.paDaten.Controls.Add(Me.Label1)
        Me.paDaten.Location = New System.Drawing.Point(784, 30)
        Me.paDaten.Name = "paDaten"
        Me.paDaten.Size = New System.Drawing.Size(188, 469)
        Me.paDaten.TabIndex = 3
        '
        'lvBuchError
        '
        Me.lvBuchError.HideSelection = False
        Me.lvBuchError.Location = New System.Drawing.Point(-1, 304)
        Me.lvBuchError.Name = "lvBuchError"
        Me.lvBuchError.Size = New System.Drawing.Size(186, 160)
        Me.lvBuchError.TabIndex = 39
        Me.lvBuchError.UseCompatibleStateImageBehavior = False
        '
        'lbCheckin
        '
        Me.lbCheckin.AutoSize = True
        Me.lbCheckin.Location = New System.Drawing.Point(58, 275)
        Me.lbCheckin.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lbCheckin.Name = "lbCheckin"
        Me.lbCheckin.Size = New System.Drawing.Size(13, 13)
        Me.lbCheckin.TabIndex = 38
        Me.lbCheckin.Text = "0"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(3, 275)
        Me.Label15.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(46, 13)
        Me.Label15.TabIndex = 37
        Me.Label15.Text = "Checkin"
        '
        'lbCode
        '
        Me.lbCode.Location = New System.Drawing.Point(60, 253)
        Me.lbCode.Name = "lbCode"
        Me.lbCode.Size = New System.Drawing.Size(125, 20)
        Me.lbCode.TabIndex = 36
        Me.lbCode.Text = " "
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(3, 253)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(32, 13)
        Me.Label13.TabIndex = 35
        Me.Label13.Text = "Code"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Location = New System.Drawing.Point(-1, 298)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(188, 8)
        Me.Panel1.TabIndex = 24
        '
        'lbEMail
        '
        Me.lbEMail.Location = New System.Drawing.Point(60, 233)
        Me.lbEMail.Name = "lbEMail"
        Me.lbEMail.Size = New System.Drawing.Size(123, 20)
        Me.lbEMail.TabIndex = 23
        Me.lbEMail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbFunk
        '
        Me.lbFunk.Location = New System.Drawing.Point(60, 213)
        Me.lbFunk.Name = "lbFunk"
        Me.lbFunk.Size = New System.Drawing.Size(123, 20)
        Me.lbFunk.TabIndex = 22
        Me.lbFunk.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbFax
        '
        Me.lbFax.Location = New System.Drawing.Point(60, 193)
        Me.lbFax.Name = "lbFax"
        Me.lbFax.Size = New System.Drawing.Size(123, 20)
        Me.lbFax.TabIndex = 21
        Me.lbFax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbTel
        '
        Me.lbTel.Location = New System.Drawing.Point(60, 173)
        Me.lbTel.Name = "lbTel"
        Me.lbTel.Size = New System.Drawing.Size(123, 20)
        Me.lbTel.TabIndex = 20
        Me.lbTel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbLand
        '
        Me.lbLand.BackColor = System.Drawing.SystemColors.Control
        Me.lbLand.Location = New System.Drawing.Point(60, 153)
        Me.lbLand.Name = "lbLand"
        Me.lbLand.Size = New System.Drawing.Size(123, 20)
        Me.lbLand.TabIndex = 19
        Me.lbLand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbOrt
        '
        Me.lbOrt.Location = New System.Drawing.Point(60, 133)
        Me.lbOrt.Name = "lbOrt"
        Me.lbOrt.Size = New System.Drawing.Size(123, 20)
        Me.lbOrt.TabIndex = 18
        Me.lbOrt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbPLZ
        '
        Me.lbPLZ.Location = New System.Drawing.Point(60, 113)
        Me.lbPLZ.Name = "lbPLZ"
        Me.lbPLZ.Size = New System.Drawing.Size(123, 20)
        Me.lbPLZ.TabIndex = 17
        Me.lbPLZ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbStr
        '
        Me.lbStr.Location = New System.Drawing.Point(60, 93)
        Me.lbStr.Name = "lbStr"
        Me.lbStr.Size = New System.Drawing.Size(123, 20)
        Me.lbStr.TabIndex = 16
        Me.lbStr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbVorname
        '
        Me.lbVorname.Location = New System.Drawing.Point(60, 73)
        Me.lbVorname.Name = "lbVorname"
        Me.lbVorname.Size = New System.Drawing.Size(123, 20)
        Me.lbVorname.TabIndex = 15
        Me.lbVorname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbName2
        '
        Me.lbName2.Location = New System.Drawing.Point(60, 53)
        Me.lbName2.Name = "lbName2"
        Me.lbName2.Size = New System.Drawing.Size(123, 20)
        Me.lbName2.TabIndex = 14
        Me.lbName2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbName
        '
        Me.lbName.Location = New System.Drawing.Point(60, 33)
        Me.lbName.Name = "lbName"
        Me.lbName.Size = New System.Drawing.Size(123, 20)
        Me.lbName.TabIndex = 13
        Me.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbAnrede
        '
        Me.lbAnrede.Location = New System.Drawing.Point(60, 13)
        Me.lbAnrede.Name = "lbAnrede"
        Me.lbAnrede.Size = New System.Drawing.Size(123, 20)
        Me.lbAnrede.TabIndex = 12
        Me.lbAnrede.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(3, 233)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(51, 20)
        Me.Label12.TabIndex = 11
        Me.Label12.Text = "EMail"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(3, 213)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(51, 20)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Funk"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(3, 173)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(51, 20)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Telefon"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(3, 193)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(51, 20)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Telefax"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(3, 133)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(51, 20)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Ort"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(3, 153)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(51, 20)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Land"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(3, 113)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 20)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "PLZ"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(3, 93)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 20)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Strasse"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(3, 73)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 20)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Vorname"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(3, 33)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(51, 20)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Name"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(3, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 20)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Name 2"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(3, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Anrede"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tiUhr
        '
        Me.tiUhr.Enabled = True
        Me.tiUhr.Interval = 1000
        '
        'tiWait
        '
        '
        'tsMain
        '
        Me.tsMain.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbClose, Me.ToolStripSeparator1, Me.tsbKunde, Me.ToolStripSeparator2, Me.tsbStatistik, Me.ToolStripSeparator3, Me.tsbPersonen, Me.ToolStripSeparator5, Me.tsbDatev, Me.ToolStripSeparator7, Me.tsbDruck, Me.ToolStripSeparator9, Me.tsbSuchen, Me.ToolStripSeparator10, Me.tsbSystem, Me.tsVersion, Me.tsbLogin, Me.ToolStripSeparator18, Me.tsbTermine, Me.tsbCorona, Me.tsbRFID, Me.tslDatum, Me.tslTabelle, Me.tslGesammt, Me.tslSatznummer, Me.ToolStripSeparator8, Me.ToolStripSeparator6, Me.ToolStripButton1, Me.ToolStripButton2, Me.tsbSchloss})
        Me.tsMain.Location = New System.Drawing.Point(0, 0)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.Size = New System.Drawing.Size(984, 27)
        Me.tsMain.TabIndex = 4
        Me.tsMain.Text = "ToolStrip1"
        '
        'tsbClose
        '
        Me.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbClose.ForeColor = System.Drawing.SystemColors.ControlLight
        Me.tsbClose.Image = Global.Pension.My.Resources.Resources.door02
        Me.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbClose.Name = "tsbClose"
        Me.tsbClose.Size = New System.Drawing.Size(24, 24)
        Me.tsbClose.Text = "Programm beenden"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 27)
        '
        'tsbKunde
        '
        Me.tsbKunde.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbKunde.Enabled = False
        Me.tsbKunde.ForeColor = System.Drawing.SystemColors.ControlText
        Me.tsbKunde.Image = CType(resources.GetObject("tsbKunde.Image"), System.Drawing.Image)
        Me.tsbKunde.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbKunde.Name = "tsbKunde"
        Me.tsbKunde.Size = New System.Drawing.Size(77, 24)
        Me.tsbKunde.Text = "Gästestamm"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 27)
        '
        'tsbStatistik
        '
        Me.tsbStatistik.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbStatistik.ForeColor = System.Drawing.SystemColors.ControlLight
        Me.tsbStatistik.Image = Global.Pension.My.Resources.Resources.umsatz
        Me.tsbStatistik.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbStatistik.Name = "tsbStatistik"
        Me.tsbStatistik.Size = New System.Drawing.Size(24, 24)
        Me.tsbStatistik.Text = "Statistik"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 27)
        '
        'tsbPersonen
        '
        Me.tsbPersonen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPersonen.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbEinteilung, Me.ToolStripSeparator13, Me.tsbPersonal})
        Me.tsbPersonen.ForeColor = System.Drawing.SystemColors.ControlLight
        Me.tsbPersonen.Image = Global.Pension.My.Resources.Resources.UrPersonal
        Me.tsbPersonen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPersonen.Name = "tsbPersonen"
        Me.tsbPersonen.Size = New System.Drawing.Size(36, 24)
        Me.tsbPersonen.Text = "Personen"
        '
        'tsbEinteilung
        '
        Me.tsbEinteilung.Name = "tsbEinteilung"
        Me.tsbEinteilung.Size = New System.Drawing.Size(180, 22)
        Me.tsbEinteilung.Text = "Einteilung"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(177, 6)
        '
        'tsbPersonal
        '
        Me.tsbPersonal.Name = "tsbPersonal"
        Me.tsbPersonal.Size = New System.Drawing.Size(180, 22)
        Me.tsbPersonal.Text = "Personal"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 27)
        '
        'tsbDatev
        '
        Me.tsbDatev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDatev.ForeColor = System.Drawing.SystemColors.ControlLight
        Me.tsbDatev.Image = Global.Pension.My.Resources.Resources.MOVER
        Me.tsbDatev.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDatev.Name = "tsbDatev"
        Me.tsbDatev.Size = New System.Drawing.Size(24, 24)
        Me.tsbDatev.Text = "Datev"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 27)
        '
        'tsbDruck
        '
        Me.tsbDruck.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDruck.ForeColor = System.Drawing.SystemColors.ControlLight
        Me.tsbDruck.Image = Global.Pension.My.Resources.Resources.comdlg32_dll_Ico18_ico_Ico1
        Me.tsbDruck.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDruck.Name = "tsbDruck"
        Me.tsbDruck.Size = New System.Drawing.Size(24, 24)
        Me.tsbDruck.Text = "Drucken"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 27)
        '
        'tsbSuchen
        '
        Me.tsbSuchen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSuchen.ForeColor = System.Drawing.SystemColors.ControlLight
        Me.tsbSuchen.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico82_ico_Ico1
        Me.tsbSuchen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSuchen.Name = "tsbSuchen"
        Me.tsbSuchen.Size = New System.Drawing.Size(24, 24)
        Me.tsbSuchen.Text = "Suche Datum"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 27)
        '
        'tsbSystem
        '
        Me.tsbSystem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSystem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmSystem, Me.ToolStripSeparator12, Me.ToolStripMenuItem1})
        Me.tsbSystem.Enabled = False
        Me.tsbSystem.ForeColor = System.Drawing.SystemColors.ControlLight
        Me.tsbSystem.Image = Global.Pension.My.Resources.Resources.WRENCH
        Me.tsbSystem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSystem.Name = "tsbSystem"
        Me.tsbSystem.Size = New System.Drawing.Size(36, 24)
        Me.tsbSystem.Text = "Systemeinstellungen"
        '
        'tsmSystem
        '
        Me.tsmSystem.Name = "tsmSystem"
        Me.tsmSystem.Size = New System.Drawing.Size(125, 22)
        Me.tsmSystem.Text = "Anpassen"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(122, 6)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmSave, Me.tsmRestore, Me.DatenKorekturToolStripMenuItem, Me.DatenKontrolleToolStripMenuItem, Me.CodeToolStripMenuItem, Me.IDKundeToolStripMenuItem, Me.ServerFehlerToolStripMenuItem, Me.IniToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(125, 22)
        Me.ToolStripMenuItem1.Text = "Database"
        '
        'tsmSave
        '
        Me.tsmSave.Name = "tsmSave"
        Me.tsmSave.Size = New System.Drawing.Size(194, 22)
        Me.tsmSave.Text = "Daten sichern"
        '
        'tsmRestore
        '
        Me.tsmRestore.Name = "tsmRestore"
        Me.tsmRestore.Size = New System.Drawing.Size(194, 22)
        Me.tsmRestore.Text = "Daten wiederherstellen"
        '
        'DatenKorekturToolStripMenuItem
        '
        Me.DatenKorekturToolStripMenuItem.Name = "DatenKorekturToolStripMenuItem"
        Me.DatenKorekturToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.DatenKorekturToolStripMenuItem.Text = "Buchung ohne Gast"
        Me.DatenKorekturToolStripMenuItem.ToolTipText = "Bereinigt verwaiste Buchungssätze in der Datenbank und bietet einen Programmneust" &
    "art an."
        '
        'DatenKontrolleToolStripMenuItem
        '
        Me.DatenKontrolleToolStripMenuItem.Name = "DatenKontrolleToolStripMenuItem"
        Me.DatenKontrolleToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.DatenKontrolleToolStripMenuItem.Text = "Kunden doppelt"
        '
        'CodeToolStripMenuItem
        '
        Me.CodeToolStripMenuItem.Name = "CodeToolStripMenuItem"
        Me.CodeToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.CodeToolStripMenuItem.Text = "Code"
        '
        'IDKundeToolStripMenuItem
        '
        Me.IDKundeToolStripMenuItem.Name = "IDKundeToolStripMenuItem"
        Me.IDKundeToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.IDKundeToolStripMenuItem.Text = "Kunde->Buchung"
        '
        'ServerFehlerToolStripMenuItem
        '
        Me.ServerFehlerToolStripMenuItem.Name = "ServerFehlerToolStripMenuItem"
        Me.ServerFehlerToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.ServerFehlerToolStripMenuItem.Text = "Server Fehler"
        '
        'IniToolStripMenuItem
        '
        Me.IniToolStripMenuItem.Name = "IniToolStripMenuItem"
        Me.IniToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.IniToolStripMenuItem.Text = "ini"
        '
        'tsVersion
        '
        Me.tsVersion.ActiveLinkColor = System.Drawing.Color.Black
        Me.tsVersion.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsVersion.BackColor = System.Drawing.SystemColors.ControlLight
        Me.tsVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.tsVersion.Name = "tsVersion"
        Me.tsVersion.Size = New System.Drawing.Size(90, 24)
        Me.tsVersion.Text = "   Version 1.0.0.0"
        '
        'tsbLogin
        '
        Me.tsbLogin.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbLogin.BackColor = System.Drawing.SystemColors.Control
        Me.tsbLogin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbLogin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.tsbLogin.Image = CType(resources.GetObject("tsbLogin.Image"), System.Drawing.Image)
        Me.tsbLogin.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbLogin.Name = "tsbLogin"
        Me.tsbLogin.Size = New System.Drawing.Size(41, 24)
        Me.tsbLogin.Text = "Login"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(6, 27)
        '
        'tsbTermine
        '
        Me.tsbTermine.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbTermine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbTermine.Image = Global.Pension.My.Resources.Resources.CLOCK06
        Me.tsbTermine.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbTermine.Name = "tsbTermine"
        Me.tsbTermine.Size = New System.Drawing.Size(24, 24)
        Me.tsbTermine.ToolTipText = "Termine"
        '
        'tsbCorona
        '
        Me.tsbCorona.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbCorona.Image = Global.Pension.My.Resources.Resources.coronavirus_png_clipart_vector
        Me.tsbCorona.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCorona.Name = "tsbCorona"
        Me.tsbCorona.Size = New System.Drawing.Size(24, 24)
        Me.tsbCorona.Text = "ToolStripButton1"
        '
        'tsbRFID
        '
        Me.tsbRFID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbRFID.Image = Global.Pension.My.Resources.Resources.rfidsign_rfi_13599
        Me.tsbRFID.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRFID.Name = "tsbRFID"
        Me.tsbRFID.Size = New System.Drawing.Size(24, 24)
        Me.tsbRFID.Text = "RFID"
        '
        'tslDatum
        '
        Me.tslDatum.Name = "tslDatum"
        Me.tslDatum.Size = New System.Drawing.Size(39, 24)
        Me.tslDatum.Text = "Heute"
        '
        'tslTabelle
        '
        Me.tslTabelle.BackColor = System.Drawing.SystemColors.Info
        Me.tslTabelle.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tslTabelle.Name = "tslTabelle"
        Me.tslTabelle.Size = New System.Drawing.Size(0, 24)
        Me.tslTabelle.Visible = False
        '
        'tslGesammt
        '
        Me.tslGesammt.BackColor = System.Drawing.SystemColors.Info
        Me.tslGesammt.Name = "tslGesammt"
        Me.tslGesammt.Size = New System.Drawing.Size(0, 24)
        Me.tslGesammt.Visible = False
        '
        'tslSatznummer
        '
        Me.tslSatznummer.BackColor = System.Drawing.SystemColors.Info
        Me.tslSatznummer.Name = "tslSatznummer"
        Me.tslSatznummer.Size = New System.Drawing.Size(0, 24)
        Me.tslSatznummer.Visible = False
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 27)
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 27)
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = Global.Pension.My.Resources.Resources.wasser
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(24, 24)
        Me.ToolStripButton1.Text = "Wasserabrechnung"
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton2.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico965_ico_Ico1
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(24, 24)
        Me.ToolStripButton2.Text = "RFID Senden"
        '
        'tsbSchloss
        '
        Me.tsbSchloss.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSchloss.Image = CType(resources.GetObject("tsbSchloss.Image"), System.Drawing.Image)
        Me.tsbSchloss.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSchloss.Name = "tsbSchloss"
        Me.tsbSchloss.Size = New System.Drawing.Size(24, 24)
        Me.tsbSchloss.Text = "Schloß"
        '
        'paDatum
        '
        Me.paDatum.BackColor = System.Drawing.Color.Wheat
        Me.paDatum.Controls.Add(Me.MonthCalendar1)
        Me.paDatum.Location = New System.Drawing.Point(308, 28)
        Me.paDatum.Name = "paDatum"
        Me.paDatum.Size = New System.Drawing.Size(195, 176)
        Me.paDatum.TabIndex = 6
        Me.paDatum.Visible = False
        '
        'MonthCalendar1
        '
        Me.MonthCalendar1.Location = New System.Drawing.Point(9, 6)
        Me.MonthCalendar1.MaxSelectionCount = 1
        Me.MonthCalendar1.Name = "MonthCalendar1"
        Me.MonthCalendar1.TabIndex = 0
        '
        'tsmCheckOut
        '
        Me.tsmCheckOut.Name = "tsmCheckOut"
        Me.tsmCheckOut.Size = New System.Drawing.Size(158, 22)
        Me.tsmCheckOut.Text = "Verschieben"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 536)
        Me.Controls.Add(Me.paDatum)
        Me.Controls.Add(Me.tsMain)
        Me.Controls.Add(Me.paDaten)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.dgBuchung)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pension am Radweg"
        CType(Me.dgBuchung, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmMain.ResumeLayout(False)
        Me.cmMain.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.paDaten.ResumeLayout(False)
        Me.paDaten.PerformLayout()
        Me.tsMain.ResumeLayout(False)
        Me.tsMain.PerformLayout()
        Me.paDatum.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgBuchung As System.Windows.Forms.DataGridView
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents paDaten As System.Windows.Forms.Panel
    Friend WithEvents tssLog As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tssDatum As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tssUhr As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tssInfo As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tiUhr As System.Windows.Forms.Timer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lbEMail As System.Windows.Forms.Label
    Friend WithEvents lbFunk As System.Windows.Forms.Label
    Friend WithEvents lbFax As System.Windows.Forms.Label
    Friend WithEvents lbTel As System.Windows.Forms.Label
    Friend WithEvents lbLand As System.Windows.Forms.Label
    Friend WithEvents lbOrt As System.Windows.Forms.Label
    Friend WithEvents lbPLZ As System.Windows.Forms.Label
    Friend WithEvents lbStr As System.Windows.Forms.Label
    Friend WithEvents lbVorname As System.Windows.Forms.Label
    Friend WithEvents lbName2 As System.Windows.Forms.Label
    Friend WithEvents lbName As System.Windows.Forms.Label
    Friend WithEvents lbAnrede As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents tiWait As System.Windows.Forms.Timer
    Friend WithEvents tsMain As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbKunde As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbStatistik As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDatev As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDruck As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbSuchen As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsVersion As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsbLogin As System.Windows.Forms.ToolStripButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmMain As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tsmNew As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsS1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsS2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsS3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbSystem As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents tsmSystem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmSave As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmRestore As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbPersonen As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents tsbPersonal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbEinteilung As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents paDatum As System.Windows.Forms.Panel
    Friend WithEvents MonthCalendar1 As System.Windows.Forms.MonthCalendar
    Friend WithEvents tsbTermine As System.Windows.Forms.ToolStripButton
    Friend WithEvents DatenKorekturToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tslDatum As System.Windows.Forms.ToolStripLabel
    Friend WithEvents DatenKontrolleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmCheckOut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmInset As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tscBuchnung As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tstAnreise As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator17 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tstEnd As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tsmDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CodeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lbCode As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ToolStripSeparator18 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tssNet As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents IDKundeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tslTabelle As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tslGesammt As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tslSatznummer As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ServerFehlerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IniToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents trTransponder As System.Windows.Forms.Timer
    Friend WithEvents tsbRFID As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbCorona As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents ToolStripButton2 As ToolStripButton
    Friend WithEvents tsmDruckAnAb As ToolStripMenuItem
    Friend WithEvents tsbSchloss As ToolStripButton
    Friend WithEvents tscCheckin As ToolStripMenuItem
    Friend WithEvents lbCheckin As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents tsmChekin1 As ToolStripMenuItem
    Friend WithEvents tsmCheckin2 As ToolStripMenuItem
    Friend WithEvents tsmCheckin3 As ToolStripMenuItem
    Friend WithEvents tsmCheckin4 As ToolStripMenuItem
    Friend WithEvents tsmCheckin5 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator11 As ToolStripSeparator
    Friend WithEvents lvBuchError As ListView
    Friend WithEvents tsmChekinNew As ToolStripMenuItem
End Class
