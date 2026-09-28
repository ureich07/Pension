<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPersonal
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
        Me.tsKonto = New System.Windows.Forms.ToolStrip()
        Me.tsbClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbNew = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbEdit = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbBreak = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDel = New System.Windows.Forms.ToolStripButton()
        Me.lvPersonal = New System.Windows.Forms.ListView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.buTransDel = New System.Windows.Forms.Button()
        Me.tbWeg34 = New System.Windows.Forms.TextBox()
        Me.lbChip = New System.Windows.Forms.Label()
        Me.buSend = New System.Windows.Forms.Button()
        Me.tbOrt = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tbPLZ = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tbStrasse = New System.Windows.Forms.TextBox()
        Me.lbID = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbInfo = New System.Windows.Forms.TextBox()
        Me.tbTel2 = New System.Windows.Forms.TextBox()
        Me.tbTel1 = New System.Windows.Forms.TextBox()
        Me.tbVorname = New System.Windows.Forms.TextBox()
        Me.tbName = New System.Windows.Forms.TextBox()
        Me.tsKonto.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tsKonto
        '
        Me.tsKonto.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsKonto.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbClose, Me.ToolStripSeparator1, Me.tsbNew, Me.ToolStripSeparator13, Me.tsbEdit, Me.ToolStripSeparator14, Me.tsbSave, Me.ToolStripSeparator15, Me.tsbBreak, Me.ToolStripSeparator16, Me.tsbDel})
        Me.tsKonto.Location = New System.Drawing.Point(0, 0)
        Me.tsKonto.Name = "tsKonto"
        Me.tsKonto.Size = New System.Drawing.Size(984, 27)
        Me.tsKonto.TabIndex = 8
        Me.tsKonto.Text = "ToolStrip1"
        '
        'tsbClose
        '
        Me.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbClose.Image = Global.Pension.My.Resources.Resources.door02
        Me.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbClose.Name = "tsbClose"
        Me.tsbClose.Size = New System.Drawing.Size(24, 24)
        Me.tsbClose.Text = "Personalbearbeitung beenden"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 27)
        '
        'tsbNew
        '
        Me.tsbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbNew.Image = Global.Pension.My.Resources.Resources.cabview_dll_Ico14_ico_Ico1
        Me.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNew.Name = "tsbNew"
        Me.tsbNew.Size = New System.Drawing.Size(24, 24)
        Me.tsbNew.ToolTipText = "Neu"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(6, 27)
        '
        'tsbEdit
        '
        Me.tsbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbEdit.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1474_ico_Ico1
        Me.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEdit.Name = "tsbEdit"
        Me.tsbEdit.Size = New System.Drawing.Size(24, 24)
        Me.tsbEdit.Text = "Bearbeiten"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(6, 27)
        '
        'tsbSave
        '
        Me.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSave.Enabled = False
        Me.tsbSave.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSave.Name = "tsbSave"
        Me.tsbSave.Size = New System.Drawing.Size(24, 24)
        Me.tsbSave.Text = "Speichen"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(6, 27)
        '
        'tsbBreak
        '
        Me.tsbBreak.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbBreak.Enabled = False
        Me.tsbBreak.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsbBreak.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbBreak.Name = "tsbBreak"
        Me.tsbBreak.Size = New System.Drawing.Size(24, 24)
        Me.tsbBreak.Text = "Abbrechen"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(6, 27)
        '
        'tsbDel
        '
        Me.tsbDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDel.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbDel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDel.Name = "tsbDel"
        Me.tsbDel.Size = New System.Drawing.Size(24, 24)
        Me.tsbDel.Text = "Löschen"
        '
        'lvPersonal
        '
        Me.lvPersonal.HideSelection = False
        Me.lvPersonal.Location = New System.Drawing.Point(12, 28)
        Me.lvPersonal.Name = "lvPersonal"
        Me.lvPersonal.Size = New System.Drawing.Size(960, 213)
        Me.lvPersonal.TabIndex = 9
        Me.lvPersonal.UseCompatibleStateImageBehavior = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.buTransDel)
        Me.Panel1.Controls.Add(Me.tbWeg34)
        Me.Panel1.Controls.Add(Me.lbChip)
        Me.Panel1.Controls.Add(Me.buSend)
        Me.Panel1.Controls.Add(Me.tbOrt)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.tbPLZ)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.tbStrasse)
        Me.Panel1.Controls.Add(Me.lbID)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.tbInfo)
        Me.Panel1.Controls.Add(Me.tbTel2)
        Me.Panel1.Controls.Add(Me.tbTel1)
        Me.Panel1.Controls.Add(Me.tbVorname)
        Me.Panel1.Controls.Add(Me.tbName)
        Me.Panel1.Location = New System.Drawing.Point(12, 247)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(960, 128)
        Me.Panel1.TabIndex = 10
        '
        'buTransDel
        '
        Me.buTransDel.Location = New System.Drawing.Point(711, 88)
        Me.buTransDel.Name = "buTransDel"
        Me.buTransDel.Size = New System.Drawing.Size(145, 20)
        Me.buTransDel.TabIndex = 30
        Me.buTransDel.Text = "Lösche Transponder Code"
        Me.buTransDel.UseVisualStyleBackColor = True
        '
        'tbWeg34
        '
        Me.tbWeg34.Enabled = False
        Me.tbWeg34.Location = New System.Drawing.Point(862, 11)
        Me.tbWeg34.Name = "tbWeg34"
        Me.tbWeg34.Size = New System.Drawing.Size(95, 20)
        Me.tbWeg34.TabIndex = 29
        '
        'lbChip
        '
        Me.lbChip.AutoSize = True
        Me.lbChip.Location = New System.Drawing.Point(718, 42)
        Me.lbChip.Name = "lbChip"
        Me.lbChip.Size = New System.Drawing.Size(49, 13)
        Me.lbChip.TabIndex = 28
        Me.lbChip.Text = "_______"
        '
        'buSend
        '
        Me.buSend.Location = New System.Drawing.Point(711, 12)
        Me.buSend.Name = "buSend"
        Me.buSend.Size = New System.Drawing.Size(145, 20)
        Me.buSend.TabIndex = 27
        Me.buSend.Text = "Sende Transponder Code"
        Me.buSend.UseVisualStyleBackColor = True
        '
        'tbOrt
        '
        Me.tbOrt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbOrt.Enabled = False
        Me.tbOrt.Location = New System.Drawing.Point(474, 64)
        Me.tbOrt.Name = "tbOrt"
        Me.tbOrt.Size = New System.Drawing.Size(217, 20)
        Me.tbOrt.TabIndex = 15
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(324, 64)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(76, 20)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "PLZ / Ort"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbPLZ
        '
        Me.tbPLZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPLZ.Enabled = False
        Me.tbPLZ.Location = New System.Drawing.Point(406, 64)
        Me.tbPLZ.Name = "tbPLZ"
        Me.tbPLZ.Size = New System.Drawing.Size(62, 20)
        Me.tbPLZ.TabIndex = 14
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(15, 64)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(76, 20)
        Me.Label6.TabIndex = 23
        Me.Label6.Text = "Strasse / HNr."
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbStrasse
        '
        Me.tbStrasse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbStrasse.Enabled = False
        Me.tbStrasse.Location = New System.Drawing.Point(97, 64)
        Me.tbStrasse.Name = "tbStrasse"
        Me.tbStrasse.Size = New System.Drawing.Size(200, 20)
        Me.tbStrasse.TabIndex = 13
        '
        'lbID
        '
        Me.lbID.AutoSize = True
        Me.lbID.Location = New System.Drawing.Point(905, 97)
        Me.lbID.Name = "lbID"
        Me.lbID.Size = New System.Drawing.Size(26, 13)
        Me.lbID.TabIndex = 21
        Me.lbID.Text = "lbID"
        Me.lbID.Visible = False
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(15, 90)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(76, 20)
        Me.Label5.TabIndex = 20
        Me.Label5.Text = "Bemerkung"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(324, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 20)
        Me.Label4.TabIndex = 19
        Me.Label4.Text = "Funk"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(324, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 20)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "Telefon"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(15, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 20)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Vorname"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(15, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 20)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbInfo
        '
        Me.tbInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbInfo.Enabled = False
        Me.tbInfo.Location = New System.Drawing.Point(97, 90)
        Me.tbInfo.Name = "tbInfo"
        Me.tbInfo.Size = New System.Drawing.Size(594, 20)
        Me.tbInfo.TabIndex = 18
        '
        'tbTel2
        '
        Me.tbTel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbTel2.Enabled = False
        Me.tbTel2.Location = New System.Drawing.Point(537, 38)
        Me.tbTel2.Name = "tbTel2"
        Me.tbTel2.Size = New System.Drawing.Size(154, 20)
        Me.tbTel2.TabIndex = 17
        '
        'tbTel1
        '
        Me.tbTel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbTel1.Enabled = False
        Me.tbTel1.Location = New System.Drawing.Point(537, 12)
        Me.tbTel1.Name = "tbTel1"
        Me.tbTel1.Size = New System.Drawing.Size(154, 20)
        Me.tbTel1.TabIndex = 16
        '
        'tbVorname
        '
        Me.tbVorname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbVorname.Enabled = False
        Me.tbVorname.Location = New System.Drawing.Point(97, 38)
        Me.tbVorname.Name = "tbVorname"
        Me.tbVorname.Size = New System.Drawing.Size(200, 20)
        Me.tbVorname.TabIndex = 12
        '
        'tbName
        '
        Me.tbName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbName.Enabled = False
        Me.tbName.Location = New System.Drawing.Point(97, 12)
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(200, 20)
        Me.tbName.TabIndex = 11
        '
        'frmPersonal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 387)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lvPersonal)
        Me.Controls.Add(Me.tsKonto)
        Me.Name = "frmPersonal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Personal"
        Me.tsKonto.ResumeLayout(False)
        Me.tsKonto.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tsKonto As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbEdit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbBreak As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDel As System.Windows.Forms.ToolStripButton
    Friend WithEvents lvPersonal As System.Windows.Forms.ListView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbID As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbInfo As System.Windows.Forms.TextBox
    Friend WithEvents tbTel2 As System.Windows.Forms.TextBox
    Friend WithEvents tbTel1 As System.Windows.Forms.TextBox
    Friend WithEvents tbVorname As System.Windows.Forms.TextBox
    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents tbOrt As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents tbPLZ As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents tbStrasse As System.Windows.Forms.TextBox
    Friend WithEvents lbChip As System.Windows.Forms.Label
    Friend WithEvents buSend As System.Windows.Forms.Button
    Friend WithEvents tbWeg34 As System.Windows.Forms.TextBox
    Friend WithEvents buTransDel As System.Windows.Forms.Button
End Class
