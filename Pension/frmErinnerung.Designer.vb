<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmErinnerung
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
        Me.paEingabe = New System.Windows.Forms.Panel()
        Me.lbClose = New System.Windows.Forms.Label()
        Me.dgTermin = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.tbEintrag = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.dtpDatum = New System.Windows.Forms.DateTimePicker()
        Me.chAktive = New System.Windows.Forms.CheckBox()
        Me.dtpTime = New System.Windows.Forms.DateTimePicker()
        Me.lbDatum = New System.Windows.Forms.Label()
        Me.cbTime = New System.Windows.Forms.ComboBox()
        Me.gbZusatz = New System.Windows.Forms.GroupBox()
        Me.tsMain.SuspendLayout()
        Me.paEingabe.SuspendLayout()
        CType(Me.dgTermin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbZusatz.SuspendLayout()
        Me.SuspendLayout()
        '
        'tsMain
        '
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbClose, Me.ToolStripSeparator1, Me.tsbNew, Me.ToolStripSeparator4, Me.tsbDelete, Me.ToolStripSeparator6})
        Me.tsMain.Location = New System.Drawing.Point(0, 0)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.Size = New System.Drawing.Size(612, 25)
        Me.tsMain.TabIndex = 129
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
        'paEingabe
        '
        Me.paEingabe.BackColor = System.Drawing.Color.LightGray
        Me.paEingabe.Controls.Add(Me.gbZusatz)
        Me.paEingabe.Controls.Add(Me.lbClose)
        Me.paEingabe.Location = New System.Drawing.Point(119, 77)
        Me.paEingabe.Name = "paEingabe"
        Me.paEingabe.Size = New System.Drawing.Size(377, 249)
        Me.paEingabe.TabIndex = 128
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
        Me.dgTermin.Size = New System.Drawing.Size(587, 348)
        Me.dgTermin.TabIndex = 127
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
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(9, 29)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(50, 20)
        Me.Label10.TabIndex = 146
        Me.Label10.Text = "Datum"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(9, 52)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(57, 20)
        Me.Label13.TabIndex = 157
        Me.Label13.Text = "Menge"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(274, 205)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 23)
        Me.cmdClose.TabIndex = 163
        Me.cmdClose.Text = "Schliessen"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'dtpDatum
        '
        Me.dtpDatum.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDatum.Location = New System.Drawing.Point(72, 146)
        Me.dtpDatum.Name = "dtpDatum"
        Me.dtpDatum.Size = New System.Drawing.Size(95, 20)
        Me.dtpDatum.TabIndex = 174
        Me.dtpDatum.Value = New Date(2012, 11, 13, 22, 5, 1, 0)
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
        'dtpTime
        '
        Me.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpTime.Location = New System.Drawing.Point(206, 204)
        Me.dtpTime.Name = "dtpTime"
        Me.dtpTime.Size = New System.Drawing.Size(62, 20)
        Me.dtpTime.TabIndex = 178
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
        'cbTime
        '
        Me.cbTime.FormattingEnabled = True
        Me.cbTime.Location = New System.Drawing.Point(173, 145)
        Me.cbTime.Name = "cbTime"
        Me.cbTime.Size = New System.Drawing.Size(72, 21)
        Me.cbTime.TabIndex = 180
        '
        'gbZusatz
        '
        Me.gbZusatz.BackColor = System.Drawing.Color.LightGray
        Me.gbZusatz.Controls.Add(Me.cbTime)
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
        Me.gbZusatz.Location = New System.Drawing.Point(10, 3)
        Me.gbZusatz.Name = "gbZusatz"
        Me.gbZusatz.Size = New System.Drawing.Size(364, 239)
        Me.gbZusatz.TabIndex = 122
        Me.gbZusatz.TabStop = False
        '
        'frmErinnerung
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(612, 387)
        Me.ControlBox = False
        Me.Controls.Add(Me.tsMain)
        Me.Controls.Add(Me.paEingabe)
        Me.Controls.Add(Me.dgTermin)
        Me.Name = "frmErinnerung"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Termine / Erinnerungen"
        Me.tsMain.ResumeLayout(False)
        Me.tsMain.PerformLayout()
        Me.paEingabe.ResumeLayout(False)
        Me.paEingabe.PerformLayout()
        CType(Me.dgTermin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbZusatz.ResumeLayout(False)
        Me.gbZusatz.PerformLayout()
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
    Friend WithEvents lbClose As System.Windows.Forms.Label
    Friend WithEvents dgTermin As System.Windows.Forms.DataGridView
    Friend WithEvents gbZusatz As System.Windows.Forms.GroupBox
    Friend WithEvents cbTime As System.Windows.Forms.ComboBox
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
End Class
