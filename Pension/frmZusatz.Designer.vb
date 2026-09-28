<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmZusatz
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.paZusatz = New System.Windows.Forms.Panel()
        Me.gbZusatz = New System.Windows.Forms.GroupBox()
        Me.tbLeistung = New System.Windows.Forms.TextBox()
        Me.btZusatz = New System.Windows.Forms.Button()
        Me.coSteuer = New System.Windows.Forms.ComboBox()
        Me.lbGesamt = New System.Windows.Forms.Label()
        Me.lbNetto = New System.Windows.Forms.Label()
        Me.lbSteuer = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpDatum = New System.Windows.Forms.DateTimePicker()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.tbMenge = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.tbPreis = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbClose = New System.Windows.Forms.Label()
        Me.dgZusatz = New System.Windows.Forms.DataGridView()
        Me.tsMain = New System.Windows.Forms.ToolStrip()
        Me.tsbClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbNew = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDelete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tssInfo = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lbSumme = New System.Windows.Forms.Label()
        Me.lbName = New System.Windows.Forms.Label()
        Me.lbZimNr = New System.Windows.Forms.Label()
        Me.paLeistung = New System.Windows.Forms.Panel()
        Me.lvLeistung = New System.Windows.Forms.ListView()
        Me.btCloseLeistung = New System.Windows.Forms.Button()
        Me.paZusatz.SuspendLayout()
        Me.gbZusatz.SuspendLayout()
        CType(Me.dgZusatz, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tsMain.SuspendLayout()
        Me.ssMain.SuspendLayout()
        Me.paLeistung.SuspendLayout()
        Me.SuspendLayout()
        '
        'paZusatz
        '
        Me.paZusatz.BackColor = System.Drawing.Color.LightYellow
        Me.paZusatz.Controls.Add(Me.gbZusatz)
        Me.paZusatz.Controls.Add(Me.lbClose)
        Me.paZusatz.Location = New System.Drawing.Point(224, 76)
        Me.paZusatz.Name = "paZusatz"
        Me.paZusatz.Size = New System.Drawing.Size(324, 249)
        Me.paZusatz.TabIndex = 125
        Me.paZusatz.Visible = False
        '
        'gbZusatz
        '
        Me.gbZusatz.BackColor = System.Drawing.Color.LightYellow
        Me.gbZusatz.Controls.Add(Me.tbLeistung)
        Me.gbZusatz.Controls.Add(Me.btZusatz)
        Me.gbZusatz.Controls.Add(Me.coSteuer)
        Me.gbZusatz.Controls.Add(Me.lbGesamt)
        Me.gbZusatz.Controls.Add(Me.lbNetto)
        Me.gbZusatz.Controls.Add(Me.lbSteuer)
        Me.gbZusatz.Controls.Add(Me.Label2)
        Me.gbZusatz.Controls.Add(Me.dtpDatum)
        Me.gbZusatz.Controls.Add(Me.cmdClose)
        Me.gbZusatz.Controls.Add(Me.cmdSave)
        Me.gbZusatz.Controls.Add(Me.Label9)
        Me.gbZusatz.Controls.Add(Me.Label13)
        Me.gbZusatz.Controls.Add(Me.tbMenge)
        Me.gbZusatz.Controls.Add(Me.Label10)
        Me.gbZusatz.Controls.Add(Me.tbPreis)
        Me.gbZusatz.Controls.Add(Me.Label4)
        Me.gbZusatz.Controls.Add(Me.Label3)
        Me.gbZusatz.Controls.Add(Me.Label1)
        Me.gbZusatz.Location = New System.Drawing.Point(7, 3)
        Me.gbZusatz.Name = "gbZusatz"
        Me.gbZusatz.Size = New System.Drawing.Size(309, 239)
        Me.gbZusatz.TabIndex = 122
        Me.gbZusatz.TabStop = False
        Me.gbZusatz.Text = "Eingabe von Zusatzleistungen"
        '
        'tbLeistung
        '
        Me.tbLeistung.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbLeistung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLeistung.Location = New System.Drawing.Point(84, 76)
        Me.tbLeistung.Name = "tbLeistung"
        Me.tbLeistung.Size = New System.Drawing.Size(195, 20)
        Me.tbLeistung.TabIndex = 182
        '
        'btZusatz
        '
        Me.btZusatz.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btZusatz.Image = Global.Pension.My.Resources.Resources.ARW01DN
        Me.btZusatz.Location = New System.Drawing.Point(278, 76)
        Me.btZusatz.Name = "btZusatz"
        Me.btZusatz.Size = New System.Drawing.Size(16, 20)
        Me.btZusatz.TabIndex = 181
        Me.btZusatz.UseVisualStyleBackColor = True
        '
        'coSteuer
        '
        Me.coSteuer.ForeColor = System.Drawing.Color.Blue
        Me.coSteuer.FormattingEnabled = True
        Me.coSteuer.Items.AddRange(New Object() {"0", "7", "19"})
        Me.coSteuer.Location = New System.Drawing.Point(84, 124)
        Me.coSteuer.Name = "coSteuer"
        Me.coSteuer.Size = New System.Drawing.Size(98, 21)
        Me.coSteuer.TabIndex = 180
        Me.coSteuer.Text = "0"
        '
        'lbGesamt
        '
        Me.lbGesamt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbGesamt.Location = New System.Drawing.Point(196, 172)
        Me.lbGesamt.Name = "lbGesamt"
        Me.lbGesamt.Size = New System.Drawing.Size(98, 20)
        Me.lbGesamt.TabIndex = 179
        Me.lbGesamt.Text = "0,00"
        Me.lbGesamt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbNetto
        '
        Me.lbNetto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbNetto.Location = New System.Drawing.Point(84, 172)
        Me.lbNetto.Name = "lbNetto"
        Me.lbNetto.Size = New System.Drawing.Size(98, 20)
        Me.lbNetto.TabIndex = 178
        Me.lbNetto.Text = "0,00"
        Me.lbNetto.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbSteuer
        '
        Me.lbSteuer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbSteuer.Location = New System.Drawing.Point(84, 148)
        Me.lbSteuer.Name = "lbSteuer"
        Me.lbSteuer.Size = New System.Drawing.Size(98, 20)
        Me.lbSteuer.TabIndex = 177
        Me.lbSteuer.Text = "0,00"
        Me.lbSteuer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(9, 172)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(83, 20)
        Me.Label2.TabIndex = 175
        Me.Label2.Text = "Netto / Brutto"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpDatum
        '
        Me.dtpDatum.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDatum.Location = New System.Drawing.Point(85, 27)
        Me.dtpDatum.Name = "dtpDatum"
        Me.dtpDatum.Size = New System.Drawing.Size(97, 20)
        Me.dtpDatum.TabIndex = 174
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(219, 205)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 23)
        Me.cmdClose.TabIndex = 163
        Me.cmdClose.Text = "Schliessen"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdSave
        '
        Me.cmdSave.Location = New System.Drawing.Point(11, 205)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(75, 23)
        Me.cmdSave.TabIndex = 162
        Me.cmdSave.Text = "Speichern"
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(9, 76)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(83, 20)
        Me.Label9.TabIndex = 147
        Me.Label9.Text = "Beschreibung"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(9, 52)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(57, 20)
        Me.Label13.TabIndex = 157
        Me.Label13.Text = "Menge"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbMenge
        '
        Me.tbMenge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbMenge.ForeColor = System.Drawing.Color.Blue
        Me.tbMenge.Location = New System.Drawing.Point(85, 52)
        Me.tbMenge.Name = "tbMenge"
        Me.tbMenge.Size = New System.Drawing.Size(97, 20)
        Me.tbMenge.TabIndex = 122
        Me.tbMenge.Text = "0"
        Me.tbMenge.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(9, 29)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(50, 20)
        Me.Label10.TabIndex = 146
        Me.Label10.Text = "Datum"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbPreis
        '
        Me.tbPreis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPreis.ForeColor = System.Drawing.Color.Blue
        Me.tbPreis.Location = New System.Drawing.Point(84, 101)
        Me.tbPreis.Name = "tbPreis"
        Me.tbPreis.Size = New System.Drawing.Size(98, 20)
        Me.tbPreis.TabIndex = 124
        Me.tbPreis.Text = "0,00"
        Me.tbPreis.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 150)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 20)
        Me.Label4.TabIndex = 136
        Me.Label4.Text = "Steuer"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 124)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 20)
        Me.Label3.TabIndex = 135
        Me.Label3.Text = "Steuer %"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(9, 102)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 20)
        Me.Label1.TabIndex = 134
        Me.Label1.Text = "Preis"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        'dgZusatz
        '
        Me.dgZusatz.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgZusatz.Location = New System.Drawing.Point(12, 34)
        Me.dgZusatz.Name = "dgZusatz"
        Me.dgZusatz.Size = New System.Drawing.Size(773, 348)
        Me.dgZusatz.TabIndex = 124
        '
        'tsMain
        '
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbClose, Me.ToolStripSeparator1, Me.tsbNew, Me.ToolStripSeparator4, Me.tsbDelete, Me.ToolStripSeparator6})
        Me.tsMain.Location = New System.Drawing.Point(0, 0)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.Size = New System.Drawing.Size(797, 25)
        Me.tsMain.TabIndex = 126
        Me.tsMain.Text = "ToolStrip1"
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
        Me.tsbNew.Text = "Neue Leistung"
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
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.tssInfo})
        Me.ssMain.Location = New System.Drawing.Point(0, 431)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(797, 22)
        Me.ssMain.TabIndex = 127
        Me.ssMain.Text = "StatusStrip1"
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
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(12, 395)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(70, 20)
        Me.Label11.TabIndex = 177
        Me.Label11.Text = "Zimmer-Nr.:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(186, 395)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(49, 20)
        Me.Label12.TabIndex = 178
        Me.Label12.Text = "Name: "
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(612, 395)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(70, 20)
        Me.Label14.TabIndex = 179
        Me.Label14.Text = "Summe: "
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbSumme
        '
        Me.lbSumme.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbSumme.Location = New System.Drawing.Point(687, 395)
        Me.lbSumme.Name = "lbSumme"
        Me.lbSumme.Size = New System.Drawing.Size(98, 20)
        Me.lbSumme.TabIndex = 180
        Me.lbSumme.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbName
        '
        Me.lbName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbName.Location = New System.Drawing.Point(224, 395)
        Me.lbName.Name = "lbName"
        Me.lbName.Size = New System.Drawing.Size(362, 20)
        Me.lbName.TabIndex = 181
        Me.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbZimNr
        '
        Me.lbZimNr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbZimNr.Location = New System.Drawing.Point(73, 395)
        Me.lbZimNr.Name = "lbZimNr"
        Me.lbZimNr.Size = New System.Drawing.Size(98, 20)
        Me.lbZimNr.TabIndex = 182
        Me.lbZimNr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'paLeistung
        '
        Me.paLeistung.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.paLeistung.Controls.Add(Me.lvLeistung)
        Me.paLeistung.Controls.Add(Me.btCloseLeistung)
        Me.paLeistung.Location = New System.Drawing.Point(243, 388)
        Me.paLeistung.Name = "paLeistung"
        Me.paLeistung.Size = New System.Drawing.Size(307, 174)
        Me.paLeistung.TabIndex = 183
        Me.paLeistung.Visible = False
        '
        'lvLeistung
        '
        Me.lvLeistung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvLeistung.Location = New System.Drawing.Point(9, 7)
        Me.lvLeistung.Name = "lvLeistung"
        Me.lvLeistung.Size = New System.Drawing.Size(288, 126)
        Me.lvLeistung.TabIndex = 2
        Me.lvLeistung.UseCompatibleStateImageBehavior = False
        Me.lvLeistung.Visible = False
        '
        'btCloseLeistung
        '
        Me.btCloseLeistung.Location = New System.Drawing.Point(219, 144)
        Me.btCloseLeistung.Name = "btCloseLeistung"
        Me.btCloseLeistung.Size = New System.Drawing.Size(78, 23)
        Me.btCloseLeistung.TabIndex = 1
        Me.btCloseLeistung.Text = "Schliessen"
        Me.btCloseLeistung.UseVisualStyleBackColor = True
        '
        'frmZusatz
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(797, 453)
        Me.ControlBox = False
        Me.Controls.Add(Me.paLeistung)
        Me.Controls.Add(Me.lbZimNr)
        Me.Controls.Add(Me.lbName)
        Me.Controls.Add(Me.lbSumme)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.tsMain)
        Me.Controls.Add(Me.paZusatz)
        Me.Controls.Add(Me.dgZusatz)
        Me.Name = "frmZusatz"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Schreibe aufs Zimmer"
        Me.paZusatz.ResumeLayout(False)
        Me.paZusatz.PerformLayout()
        Me.gbZusatz.ResumeLayout(False)
        Me.gbZusatz.PerformLayout()
        CType(Me.dgZusatz, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tsMain.ResumeLayout(False)
        Me.tsMain.PerformLayout()
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.paLeistung.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents paZusatz As System.Windows.Forms.Panel
    Friend WithEvents lbClose As System.Windows.Forms.Label
    Friend WithEvents gbZusatz As System.Windows.Forms.GroupBox
    Friend WithEvents dtpDatum As System.Windows.Forms.DateTimePicker
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents tbMenge As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tbPreis As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgZusatz As System.Windows.Forms.DataGridView
    Friend WithEvents tsMain As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ssMain As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tssInfo As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lbGesamt As System.Windows.Forms.Label
    Friend WithEvents lbNetto As System.Windows.Forms.Label
    Friend WithEvents lbSteuer As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lbSumme As System.Windows.Forms.Label
    Friend WithEvents lbName As System.Windows.Forms.Label
    Friend WithEvents lbZimNr As System.Windows.Forms.Label
    Friend WithEvents coSteuer As System.Windows.Forms.ComboBox
    Friend WithEvents tbLeistung As System.Windows.Forms.TextBox
    Friend WithEvents btZusatz As System.Windows.Forms.Button
    Friend WithEvents paLeistung As System.Windows.Forms.Panel
    Friend WithEvents lvLeistung As System.Windows.Forms.ListView
    Friend WithEvents btCloseLeistung As System.Windows.Forms.Button
End Class
