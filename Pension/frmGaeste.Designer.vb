<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGaeste
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
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbNew = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDelete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSuchen = New System.Windows.Forms.ToolStripButton()
        Me.tsbSuchfeld = New System.Windows.Forms.ToolStripTextBox()
        Me.tslKunID = New System.Windows.Forms.ToolStripLabel()
        Me.dgKunden = New System.Windows.Forms.DataGridView()
        Me.cmKunde = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmIDSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmIDplusKunde = New System.Windows.Forms.ToolStripMenuItem()
        Me.paGast = New System.Windows.Forms.Panel()
        Me.lbClose = New System.Windows.Forms.Label()
        Me.tpDaten = New System.Windows.Forms.TabControl()
        Me.tpGast = New System.Windows.Forms.TabPage()
        Me.gbGaeste = New System.Windows.Forms.GroupBox()
        Me.tbInfo = New System.Windows.Forms.TextBox()
        Me.dtpGeb = New System.Windows.Forms.DateTimePicker()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.tbTelefax = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.coAnrede = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.tbName2 = New System.Windows.Forms.TextBox()
        Me.tbVorname = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.tbPass = New System.Windows.Forms.TextBox()
        Me.tbHandy = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.tbName1 = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.tbEMail = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tbTelefon = New System.Windows.Forms.TextBox()
        Me.tbLand = New System.Windows.Forms.TextBox()
        Me.tbPLZ = New System.Windows.Forms.TextBox()
        Me.tbStrasse = New System.Windows.Forms.TextBox()
        Me.tbOrt = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tpHistory = New System.Windows.Forms.TabPage()
        Me.dgBuchung = New System.Windows.Forms.DataGridView()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tssInfo = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgKunden, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmKunde.SuspendLayout()
        Me.paGast.SuspendLayout()
        Me.tpDaten.SuspendLayout()
        Me.tpGast.SuspendLayout()
        Me.gbGaeste.SuspendLayout()
        Me.tpHistory.SuspendLayout()
        CType(Me.dgBuchung, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbClose, Me.ToolStripSeparator1, Me.tsbNew, Me.ToolStripSeparator4, Me.tsbDelete, Me.ToolStripSeparator6, Me.tsbSuchen, Me.tsbSuchfeld, Me.tslKunID})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(762, 25)
        Me.ToolStrip1.TabIndex = 50
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbClose
        '
        Me.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbClose.Image = Global.Pension.My.Resources.Resources.door02
        Me.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbClose.Name = "tsbClose"
        Me.tsbClose.Size = New System.Drawing.Size(23, 22)
        Me.tsbClose.Text = "Close"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsbNew
        '
        Me.tsbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbNew.Image = Global.Pension.My.Resources.Resources.cabview_dll_Ico14_ico_Ico1
        Me.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNew.Name = "tsbNew"
        Me.tsbNew.Size = New System.Drawing.Size(23, 22)
        Me.tsbNew.Text = "Neuer Gast"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'tsbDelete
        '
        Me.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDelete.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDelete.Name = "tsbDelete"
        Me.tsbDelete.Size = New System.Drawing.Size(23, 22)
        Me.tsbDelete.Text = "Auswahl löschen"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 25)
        '
        'tsbSuchen
        '
        Me.tsbSuchen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSuchen.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico82_ico_Ico1
        Me.tsbSuchen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSuchen.Name = "tsbSuchen"
        Me.tsbSuchen.Size = New System.Drawing.Size(23, 22)
        Me.tsbSuchen.Text = "Suchen"
        '
        'tsbSuchfeld
        '
        Me.tsbSuchfeld.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tsbSuchfeld.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.tsbSuchfeld.Name = "tsbSuchfeld"
        Me.tsbSuchfeld.Size = New System.Drawing.Size(100, 25)
        Me.tsbSuchfeld.Visible = False
        '
        'tslKunID
        '
        Me.tslKunID.Name = "tslKunID"
        Me.tslKunID.Size = New System.Drawing.Size(0, 22)
        '
        'dgKunden
        '
        Me.dgKunden.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgKunden.ContextMenuStrip = Me.cmKunde
        Me.dgKunden.Location = New System.Drawing.Point(10, 28)
        Me.dgKunden.Name = "dgKunden"
        Me.dgKunden.Size = New System.Drawing.Size(744, 462)
        Me.dgKunden.TabIndex = 70
        '
        'cmKunde
        '
        Me.cmKunde.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmIDSave, Me.cmIDplusKunde})
        Me.cmKunde.Name = "cmKunde"
        Me.cmKunde.Size = New System.Drawing.Size(152, 48)
        '
        'cmIDSave
        '
        Me.cmIDSave.Name = "cmIDSave"
        Me.cmIDSave.Size = New System.Drawing.Size(151, 22)
        Me.cmIDSave.Text = "ID Speichern"
        '
        'cmIDplusKunde
        '
        Me.cmIDplusKunde.Name = "cmIDplusKunde"
        Me.cmIDplusKunde.Size = New System.Drawing.Size(151, 22)
        Me.cmIDplusKunde.Text = "ID= Kunden ID"
        '
        'paGast
        '
        Me.paGast.BackColor = System.Drawing.Color.LightGray
        Me.paGast.Controls.Add(Me.lbClose)
        Me.paGast.Controls.Add(Me.tpDaten)
        Me.paGast.Location = New System.Drawing.Point(47, 67)
        Me.paGast.Name = "paGast"
        Me.paGast.Size = New System.Drawing.Size(685, 376)
        Me.paGast.TabIndex = 123
        Me.paGast.Visible = False
        '
        'lbClose
        '
        Me.lbClose.AutoSize = True
        Me.lbClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbClose.Location = New System.Drawing.Point(667, 0)
        Me.lbClose.Name = "lbClose"
        Me.lbClose.Size = New System.Drawing.Size(15, 13)
        Me.lbClose.TabIndex = 124
        Me.lbClose.Text = "X"
        '
        'tpDaten
        '
        Me.tpDaten.Controls.Add(Me.tpGast)
        Me.tpDaten.Controls.Add(Me.tpHistory)
        Me.tpDaten.Location = New System.Drawing.Point(16, 14)
        Me.tpDaten.Name = "tpDaten"
        Me.tpDaten.SelectedIndex = 0
        Me.tpDaten.Size = New System.Drawing.Size(652, 346)
        Me.tpDaten.TabIndex = 123
        '
        'tpGast
        '
        Me.tpGast.BackColor = System.Drawing.Color.Transparent
        Me.tpGast.Controls.Add(Me.gbGaeste)
        Me.tpGast.Location = New System.Drawing.Point(4, 22)
        Me.tpGast.Name = "tpGast"
        Me.tpGast.Padding = New System.Windows.Forms.Padding(3)
        Me.tpGast.Size = New System.Drawing.Size(644, 320)
        Me.tpGast.TabIndex = 0
        Me.tpGast.Text = "Gastdaten"
        Me.tpGast.UseVisualStyleBackColor = True
        '
        'gbGaeste
        '
        Me.gbGaeste.Controls.Add(Me.tbInfo)
        Me.gbGaeste.Controls.Add(Me.dtpGeb)
        Me.gbGaeste.Controls.Add(Me.Label17)
        Me.gbGaeste.Controls.Add(Me.tbTelefax)
        Me.gbGaeste.Controls.Add(Me.Label16)
        Me.gbGaeste.Controls.Add(Me.coAnrede)
        Me.gbGaeste.Controls.Add(Me.Label7)
        Me.gbGaeste.Controls.Add(Me.cmdClose)
        Me.gbGaeste.Controls.Add(Me.cmdSave)
        Me.gbGaeste.Controls.Add(Me.tbName2)
        Me.gbGaeste.Controls.Add(Me.tbVorname)
        Me.gbGaeste.Controls.Add(Me.Label9)
        Me.gbGaeste.Controls.Add(Me.Label13)
        Me.gbGaeste.Controls.Add(Me.Label11)
        Me.gbGaeste.Controls.Add(Me.tbPass)
        Me.gbGaeste.Controls.Add(Me.tbHandy)
        Me.gbGaeste.Controls.Add(Me.Label8)
        Me.gbGaeste.Controls.Add(Me.tbName1)
        Me.gbGaeste.Controls.Add(Me.Label10)
        Me.gbGaeste.Controls.Add(Me.tbEMail)
        Me.gbGaeste.Controls.Add(Me.Label6)
        Me.gbGaeste.Controls.Add(Me.tbTelefon)
        Me.gbGaeste.Controls.Add(Me.tbLand)
        Me.gbGaeste.Controls.Add(Me.tbPLZ)
        Me.gbGaeste.Controls.Add(Me.tbStrasse)
        Me.gbGaeste.Controls.Add(Me.tbOrt)
        Me.gbGaeste.Controls.Add(Me.Label5)
        Me.gbGaeste.Controls.Add(Me.Label4)
        Me.gbGaeste.Controls.Add(Me.Label3)
        Me.gbGaeste.Controls.Add(Me.Label1)
        Me.gbGaeste.Location = New System.Drawing.Point(6, 6)
        Me.gbGaeste.Name = "gbGaeste"
        Me.gbGaeste.Size = New System.Drawing.Size(630, 304)
        Me.gbGaeste.TabIndex = 122
        Me.gbGaeste.TabStop = False
        '
        'tbInfo
        '
        Me.tbInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbInfo.ForeColor = System.Drawing.Color.Blue
        Me.tbInfo.Location = New System.Drawing.Point(75, 190)
        Me.tbInfo.Multiline = True
        Me.tbInfo.Name = "tbInfo"
        Me.tbInfo.Size = New System.Drawing.Size(537, 74)
        Me.tbInfo.TabIndex = 166
        '
        'dtpGeb
        '
        Me.dtpGeb.Location = New System.Drawing.Point(412, 66)
        Me.dtpGeb.Name = "dtpGeb"
        Me.dtpGeb.Size = New System.Drawing.Size(200, 20)
        Me.dtpGeb.TabIndex = 174
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(340, 140)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(67, 20)
        Me.Label17.TabIndex = 173
        Me.Label17.Text = "Mobile"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTelefax
        '
        Me.tbTelefax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbTelefax.ForeColor = System.Drawing.Color.Blue
        Me.tbTelefax.Location = New System.Drawing.Point(412, 116)
        Me.tbTelefax.Name = "tbTelefax"
        Me.tbTelefax.Size = New System.Drawing.Size(200, 20)
        Me.tbTelefax.TabIndex = 170
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(340, 117)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(67, 20)
        Me.Label16.TabIndex = 171
        Me.Label16.Text = "Telefax"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'coAnrede
        '
        Me.coAnrede.ForeColor = System.Drawing.Color.Blue
        Me.coAnrede.FormattingEnabled = True
        Me.coAnrede.Items.AddRange(New Object() {"Frau", "Herr", "Firma"})
        Me.coAnrede.Location = New System.Drawing.Point(75, 17)
        Me.coAnrede.Name = "coAnrede"
        Me.coAnrede.Size = New System.Drawing.Size(64, 21)
        Me.coAnrede.TabIndex = 161
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 188)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(84, 20)
        Me.Label7.TabIndex = 168
        Me.Label7.Text = "Bemerkung"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(537, 275)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 23)
        Me.cmdClose.TabIndex = 163
        Me.cmdClose.Text = "Schliessen"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdSave
        '
        Me.cmdSave.Location = New System.Drawing.Point(17, 275)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(75, 23)
        Me.cmdSave.TabIndex = 162
        Me.cmdSave.Text = "Speichern"
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'tbName2
        '
        Me.tbName2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbName2.ForeColor = System.Drawing.Color.Blue
        Me.tbName2.Location = New System.Drawing.Point(412, 42)
        Me.tbName2.Name = "tbName2"
        Me.tbName2.Size = New System.Drawing.Size(200, 20)
        Me.tbName2.TabIndex = 156
        '
        'tbVorname
        '
        Me.tbVorname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbVorname.ForeColor = System.Drawing.Color.Blue
        Me.tbVorname.Location = New System.Drawing.Point(74, 67)
        Me.tbVorname.Name = "tbVorname"
        Me.tbVorname.Size = New System.Drawing.Size(248, 20)
        Me.tbVorname.TabIndex = 123
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(9, 67)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(57, 20)
        Me.Label9.TabIndex = 147
        Me.Label9.Text = "Vorname"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(9, 42)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(35, 20)
        Me.Label13.TabIndex = 157
        Me.Label13.Text = "Name"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(135, 147)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(47, 13)
        Me.Label11.TabIndex = 154
        Me.Label11.Text = "Pass-Nr."
        '
        'tbPass
        '
        Me.tbPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPass.ForeColor = System.Drawing.Color.Blue
        Me.tbPass.Location = New System.Drawing.Point(192, 143)
        Me.tbPass.Name = "tbPass"
        Me.tbPass.Size = New System.Drawing.Size(129, 20)
        Me.tbPass.TabIndex = 130
        '
        'tbHandy
        '
        Me.tbHandy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbHandy.ForeColor = System.Drawing.Color.Blue
        Me.tbHandy.Location = New System.Drawing.Point(412, 140)
        Me.tbHandy.Name = "tbHandy"
        Me.tbHandy.Size = New System.Drawing.Size(200, 20)
        Me.tbHandy.TabIndex = 132
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(340, 162)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(36, 20)
        Me.Label8.TabIndex = 152
        Me.Label8.Text = "E-Mail"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbName1
        '
        Me.tbName1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbName1.ForeColor = System.Drawing.Color.Blue
        Me.tbName1.Location = New System.Drawing.Point(75, 42)
        Me.tbName1.Name = "tbName1"
        Me.tbName1.Size = New System.Drawing.Size(247, 20)
        Me.tbName1.TabIndex = 122
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(9, 18)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(50, 20)
        Me.Label10.TabIndex = 146
        Me.Label10.Text = "Anrede"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbEMail
        '
        Me.tbEMail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbEMail.ForeColor = System.Drawing.Color.Blue
        Me.tbEMail.Location = New System.Drawing.Point(412, 164)
        Me.tbEMail.Name = "tbEMail"
        Me.tbEMail.Size = New System.Drawing.Size(200, 20)
        Me.tbEMail.TabIndex = 133
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(339, 67)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(30, 20)
        Me.Label6.TabIndex = 144
        Me.Label6.Text = "Geb."
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTelefon
        '
        Me.tbTelefon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbTelefon.ForeColor = System.Drawing.Color.Blue
        Me.tbTelefon.Location = New System.Drawing.Point(412, 92)
        Me.tbTelefon.Name = "tbTelefon"
        Me.tbTelefon.Size = New System.Drawing.Size(200, 20)
        Me.tbTelefon.TabIndex = 131
        '
        'tbLand
        '
        Me.tbLand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbLand.ForeColor = System.Drawing.Color.Blue
        Me.tbLand.Location = New System.Drawing.Point(74, 143)
        Me.tbLand.Name = "tbLand"
        Me.tbLand.Size = New System.Drawing.Size(42, 20)
        Me.tbLand.TabIndex = 128
        '
        'tbPLZ
        '
        Me.tbPLZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPLZ.ForeColor = System.Drawing.Color.Blue
        Me.tbPLZ.Location = New System.Drawing.Point(74, 117)
        Me.tbPLZ.Name = "tbPLZ"
        Me.tbPLZ.Size = New System.Drawing.Size(42, 20)
        Me.tbPLZ.TabIndex = 126
        '
        'tbStrasse
        '
        Me.tbStrasse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbStrasse.ForeColor = System.Drawing.Color.Blue
        Me.tbStrasse.Location = New System.Drawing.Point(74, 92)
        Me.tbStrasse.Name = "tbStrasse"
        Me.tbStrasse.Size = New System.Drawing.Size(248, 20)
        Me.tbStrasse.TabIndex = 124
        '
        'tbOrt
        '
        Me.tbOrt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbOrt.ForeColor = System.Drawing.Color.Blue
        Me.tbOrt.Location = New System.Drawing.Point(138, 117)
        Me.tbOrt.Name = "tbOrt"
        Me.tbOrt.Size = New System.Drawing.Size(184, 20)
        Me.tbOrt.TabIndex = 127
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(339, 92)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 20)
        Me.Label5.TabIndex = 137
        Me.Label5.Text = "Telefon"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 143)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 20)
        Me.Label4.TabIndex = 136
        Me.Label4.Text = "Land"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 117)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 20)
        Me.Label3.TabIndex = 135
        Me.Label3.Text = "PLZ / Ort"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 95)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 20)
        Me.Label1.TabIndex = 134
        Me.Label1.Text = "Strasse"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tpHistory
        '
        Me.tpHistory.BackColor = System.Drawing.Color.Transparent
        Me.tpHistory.Controls.Add(Me.dgBuchung)
        Me.tpHistory.Location = New System.Drawing.Point(4, 22)
        Me.tpHistory.Name = "tpHistory"
        Me.tpHistory.Padding = New System.Windows.Forms.Padding(3)
        Me.tpHistory.Size = New System.Drawing.Size(644, 320)
        Me.tpHistory.TabIndex = 1
        Me.tpHistory.Text = "Bisherige Aufenthalte"
        Me.tpHistory.UseVisualStyleBackColor = True
        '
        'dgBuchung
        '
        Me.dgBuchung.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgBuchung.Location = New System.Drawing.Point(6, 6)
        Me.dgBuchung.Name = "dgBuchung"
        Me.dgBuchung.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgBuchung.Size = New System.Drawing.Size(632, 308)
        Me.dgBuchung.TabIndex = 69
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.tssInfo})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 521)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(762, 22)
        Me.StatusStrip1.TabIndex = 124
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(39, 17)
        Me.ToolStripStatusLabel1.Text = "Infos: "
        '
        'tssInfo
        '
        Me.tssInfo.Name = "tssInfo"
        Me.tssInfo.Size = New System.Drawing.Size(10, 17)
        Me.tssInfo.Text = " "
        '
        'frmGaeste
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(762, 543)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.paGast)
        Me.Controls.Add(Me.dgKunden)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "frmGaeste"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gästestamm"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgKunden, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmKunde.ResumeLayout(False)
        Me.paGast.ResumeLayout(False)
        Me.paGast.PerformLayout()
        Me.tpDaten.ResumeLayout(False)
        Me.tpGast.ResumeLayout(False)
        Me.gbGaeste.ResumeLayout(False)
        Me.gbGaeste.PerformLayout()
        Me.tpHistory.ResumeLayout(False)
        CType(Me.dgBuchung, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents dgKunden As System.Windows.Forms.DataGridView
    Friend WithEvents paGast As System.Windows.Forms.Panel
    Friend WithEvents lbClose As System.Windows.Forms.Label
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents tssInfo As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsbSuchen As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSuchfeld As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tslKunID As System.Windows.Forms.ToolStripLabel
    Friend WithEvents cmKunde As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmIDSave As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmIDplusKunde As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tpDaten As System.Windows.Forms.TabControl
    Friend WithEvents tpGast As System.Windows.Forms.TabPage
    Friend WithEvents gbGaeste As System.Windows.Forms.GroupBox
    Friend WithEvents tbInfo As System.Windows.Forms.TextBox
    Friend WithEvents dtpGeb As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents tbTelefax As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents coAnrede As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents tbName2 As System.Windows.Forms.TextBox
    Friend WithEvents tbVorname As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents tbPass As System.Windows.Forms.TextBox
    Friend WithEvents tbHandy As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tbName1 As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tbEMail As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents tbTelefon As System.Windows.Forms.TextBox
    Friend WithEvents tbLand As System.Windows.Forms.TextBox
    Friend WithEvents tbPLZ As System.Windows.Forms.TextBox
    Friend WithEvents tbStrasse As System.Windows.Forms.TextBox
    Friend WithEvents tbOrt As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tpHistory As System.Windows.Forms.TabPage
    Friend WithEvents dgBuchung As System.Windows.Forms.DataGridView
End Class
