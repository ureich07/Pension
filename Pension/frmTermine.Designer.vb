<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTermine
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
        Me.tsMain = New System.Windows.Forms.ToolStrip()
        Me.tsbClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbNew = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDelete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbFilter = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbMail = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.paEingabe = New System.Windows.Forms.Panel()
        Me.gbZusatz = New System.Windows.Forms.GroupBox()
        Me.lbDatum = New System.Windows.Forms.Label()
        Me.dtpTime = New System.Windows.Forms.DateTimePicker()
        Me.chAktive = New System.Windows.Forms.CheckBox()
        Me.dtpDatum = New System.Windows.Forms.DateTimePicker()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.tbEintrag = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbClose = New System.Windows.Forms.Label()
        Me.dgTermin = New System.Windows.Forms.DataGridView()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lbEMail = New System.Windows.Forms.Label()
        Me.lbTel = New System.Windows.Forms.Label()
        Me.lbVorname = New System.Windows.Forms.Label()
        Me.lbName2 = New System.Windows.Forms.Label()
        Me.lbName = New System.Windows.Forms.Label()
        Me.lbBis = New System.Windows.Forms.Label()
        Me.liZimmer = New System.Windows.Forms.ListBox()
        Me.lbVon = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tsMain.SuspendLayout()
        Me.paEingabe.SuspendLayout()
        Me.gbZusatz.SuspendLayout()
        CType(Me.dgTermin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tsMain
        '
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbClose, Me.ToolStripSeparator1, Me.tsbNew, Me.ToolStripSeparator4, Me.tsbDelete, Me.ToolStripSeparator6, Me.tsbFilter, Me.ToolStripSeparator2, Me.tsbMail, Me.ToolStripSeparator3})
        Me.tsMain.Location = New System.Drawing.Point(0, 0)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.Size = New System.Drawing.Size(751, 25)
        Me.tsMain.TabIndex = 132
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
        'tsbFilter
        '
        Me.tsbFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFilter.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico82_ico_Ico1
        Me.tsbFilter.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFilter.Name = "tsbFilter"
        Me.tsbFilter.Size = New System.Drawing.Size(23, 22)
        Me.tsbFilter.Text = "Filter"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'tsbMail
        '
        Me.tsbMail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbMail.Image = Global.Pension.My.Resources.Resources.MAIL21B
        Me.tsbMail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbMail.Name = "tsbMail"
        Me.tsbMail.Size = New System.Drawing.Size(23, 22)
        Me.tsbMail.Text = "Mail"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'paEingabe
        '
        Me.paEingabe.BackColor = System.Drawing.Color.LightGray
        Me.paEingabe.Controls.Add(Me.gbZusatz)
        Me.paEingabe.Controls.Add(Me.lbClose)
        Me.paEingabe.Location = New System.Drawing.Point(196, 71)
        Me.paEingabe.Name = "paEingabe"
        Me.paEingabe.Size = New System.Drawing.Size(377, 249)
        Me.paEingabe.TabIndex = 131
        Me.paEingabe.Visible = False
        '
        'gbZusatz
        '
        Me.gbZusatz.BackColor = System.Drawing.Color.LightGray
        Me.gbZusatz.Controls.Add(Me.lbDatum)
        Me.gbZusatz.Controls.Add(Me.dtpTime)
        Me.gbZusatz.Controls.Add(Me.chAktive)
        Me.gbZusatz.Controls.Add(Me.dtpDatum)
        Me.gbZusatz.Controls.Add(Me.cmdClose)
        Me.gbZusatz.Controls.Add(Me.cmdSave)
        Me.gbZusatz.Controls.Add(Me.Label13)
        Me.gbZusatz.Controls.Add(Me.tbEintrag)
        Me.gbZusatz.Controls.Add(Me.Label10)
        Me.gbZusatz.Controls.Add(Me.Label1)
        Me.gbZusatz.Location = New System.Drawing.Point(7, 2)
        Me.gbZusatz.Name = "gbZusatz"
        Me.gbZusatz.Size = New System.Drawing.Size(364, 239)
        Me.gbZusatz.TabIndex = 122
        Me.gbZusatz.TabStop = False
        '
        'lbDatum
        '
        Me.lbDatum.Location = New System.Drawing.Point(69, 29)
        Me.lbDatum.Name = "lbDatum"
        Me.lbDatum.Size = New System.Drawing.Size(280, 20)
        Me.lbDatum.TabIndex = 179
        Me.lbDatum.Text = "Datum"
        Me.lbDatum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTime
        '
        Me.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpTime.Location = New System.Drawing.Point(173, 146)
        Me.dtpTime.Name = "dtpTime"
        Me.dtpTime.Size = New System.Drawing.Size(62, 20)
        Me.dtpTime.TabIndex = 178
        '
        'chAktive
        '
        Me.chAktive.AutoSize = True
        Me.chAktive.Location = New System.Drawing.Point(72, 175)
        Me.chAktive.Name = "chAktive"
        Me.chAktive.Size = New System.Drawing.Size(126, 17)
        Me.chAktive.TabIndex = 175
        Me.chAktive.Text = "Erinnerung aktivieren"
        Me.chAktive.UseVisualStyleBackColor = True
        '
        'dtpDatum
        '
        Me.dtpDatum.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDatum.Location = New System.Drawing.Point(72, 146)
        Me.dtpDatum.Name = "dtpDatum"
        Me.dtpDatum.Size = New System.Drawing.Size(95, 20)
        Me.dtpDatum.TabIndex = 174
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(274, 205)
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
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(9, 52)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(57, 20)
        Me.Label13.TabIndex = 157
        Me.Label13.Text = "Eintrag"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbEintrag
        '
        Me.tbEintrag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbEintrag.ForeColor = System.Drawing.Color.Blue
        Me.tbEintrag.Location = New System.Drawing.Point(72, 52)
        Me.tbEintrag.MaxLength = 255
        Me.tbEintrag.Multiline = True
        Me.tbEintrag.Name = "tbEintrag"
        Me.tbEintrag.Size = New System.Drawing.Size(277, 82)
        Me.tbEintrag.TabIndex = 122
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
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(9, 146)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 20)
        Me.Label1.TabIndex = 134
        Me.Label1.Text = "Datum"
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
        'dgTermin
        '
        Me.dgTermin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgTermin.Location = New System.Drawing.Point(12, 28)
        Me.dgTermin.Name = "dgTermin"
        Me.dgTermin.Size = New System.Drawing.Size(727, 348)
        Me.dgTermin.TabIndex = 130
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lbEMail)
        Me.GroupBox1.Controls.Add(Me.lbTel)
        Me.GroupBox1.Controls.Add(Me.lbVorname)
        Me.GroupBox1.Controls.Add(Me.lbName2)
        Me.GroupBox1.Controls.Add(Me.lbName)
        Me.GroupBox1.Controls.Add(Me.lbBis)
        Me.GroupBox1.Controls.Add(Me.liZimmer)
        Me.GroupBox1.Controls.Add(Me.lbVon)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 382)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(727, 121)
        Me.GroupBox1.TabIndex = 133
        Me.GroupBox1.TabStop = False
        '
        'lbEMail
        '
        Me.lbEMail.Location = New System.Drawing.Point(96, 98)
        Me.lbEMail.Name = "lbEMail"
        Me.lbEMail.Size = New System.Drawing.Size(281, 20)
        Me.lbEMail.TabIndex = 14
        Me.lbEMail.Text = "Name"
        Me.lbEMail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbTel
        '
        Me.lbTel.Location = New System.Drawing.Point(96, 77)
        Me.lbTel.Name = "lbTel"
        Me.lbTel.Size = New System.Drawing.Size(281, 20)
        Me.lbTel.TabIndex = 13
        Me.lbTel.Text = "Name"
        Me.lbTel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbVorname
        '
        Me.lbVorname.Location = New System.Drawing.Point(96, 57)
        Me.lbVorname.Name = "lbVorname"
        Me.lbVorname.Size = New System.Drawing.Size(281, 20)
        Me.lbVorname.TabIndex = 12
        Me.lbVorname.Text = "Name"
        Me.lbVorname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbName2
        '
        Me.lbName2.Location = New System.Drawing.Point(96, 37)
        Me.lbName2.Name = "lbName2"
        Me.lbName2.Size = New System.Drawing.Size(281, 20)
        Me.lbName2.TabIndex = 11
        Me.lbName2.Text = "Name"
        Me.lbName2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbName
        '
        Me.lbName.Location = New System.Drawing.Point(96, 17)
        Me.lbName.Name = "lbName"
        Me.lbName.Size = New System.Drawing.Size(281, 20)
        Me.lbName.TabIndex = 10
        Me.lbName.Text = "Name"
        Me.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbBis
        '
        Me.lbBis.Location = New System.Drawing.Point(503, 37)
        Me.lbBis.Name = "lbBis"
        Me.lbBis.Size = New System.Drawing.Size(92, 20)
        Me.lbBis.TabIndex = 9
        Me.lbBis.Text = "Bis"
        Me.lbBis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'liZimmer
        '
        Me.liZimmer.FormattingEnabled = True
        Me.liZimmer.Location = New System.Drawing.Point(601, 19)
        Me.liZimmer.Name = "liZimmer"
        Me.liZimmer.Size = New System.Drawing.Size(120, 95)
        Me.liZimmer.TabIndex = 8
        '
        'lbVon
        '
        Me.lbVon.Location = New System.Drawing.Point(503, 17)
        Me.lbVon.Name = "lbVon"
        Me.lbVon.Size = New System.Drawing.Size(92, 20)
        Me.lbVon.TabIndex = 7
        Me.lbVon.Text = "Von"
        Me.lbVon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(413, 19)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 20)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Reservierung:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(6, 97)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(59, 20)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Email"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 77)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 20)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Telefon"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(6, 17)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(84, 20)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Name / Firma"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 57)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 20)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Vorname"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(6, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 20)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmTermine
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(751, 515)
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.tsMain)
        Me.Controls.Add(Me.paEingabe)
        Me.Controls.Add(Me.dgTermin)
        Me.Name = "frmTermine"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Termine / Erinnerungen"
        Me.TopMost = True
        Me.tsMain.ResumeLayout(False)
        Me.tsMain.PerformLayout()
        Me.paEingabe.ResumeLayout(False)
        Me.paEingabe.PerformLayout()
        Me.gbZusatz.ResumeLayout(False)
        Me.gbZusatz.PerformLayout()
        CType(Me.dgTermin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tsMain As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents paEingabe As System.Windows.Forms.Panel
    Friend WithEvents gbZusatz As System.Windows.Forms.GroupBox
    Friend WithEvents lbDatum As System.Windows.Forms.Label
    Friend WithEvents dtpTime As System.Windows.Forms.DateTimePicker
    Friend WithEvents chAktive As System.Windows.Forms.CheckBox
    Friend WithEvents dtpDatum As System.Windows.Forms.DateTimePicker
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents tbEintrag As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbClose As System.Windows.Forms.Label
    Friend WithEvents dgTermin As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lbEMail As System.Windows.Forms.Label
    Friend WithEvents lbTel As System.Windows.Forms.Label
    Friend WithEvents lbVorname As System.Windows.Forms.Label
    Friend WithEvents lbName2 As System.Windows.Forms.Label
    Friend WithEvents lbName As System.Windows.Forms.Label
    Friend WithEvents lbBis As System.Windows.Forms.Label
    Friend WithEvents liZimmer As System.Windows.Forms.ListBox
    Friend WithEvents lbVon As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents tsbFilter As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbMail As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
End Class
