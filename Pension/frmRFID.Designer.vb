<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRFID
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
        Me.tbRFID = New System.Windows.Forms.TextBox()
        Me.mcVon = New System.Windows.Forms.MonthCalendar()
        Me.mcBis = New System.Windows.Forms.MonthCalendar()
        Me.lvRFID = New System.Windows.Forms.ListView()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSammelChip = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbNew = New System.Windows.Forms.ToolStripButton()
        Me.tsbEdit = New System.Windows.Forms.ToolStripButton()
        Me.tsbReturn = New System.Windows.Forms.ToolStripButton()
        Me.tsbSave = New System.Windows.Forms.ToolStripButton()
        Me.tsbDel = New System.Windows.Forms.ToolStripButton()
        Me.tsbReadRFID = New System.Windows.Forms.ToolStripButton()
        Me.tsbSchlossSet = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbReturn1 = New System.Windows.Forms.ToolStripButton()
        Me.tsbSave1 = New System.Windows.Forms.ToolStripButton()
        Me.cbVonZeit = New System.Windows.Forms.ComboBox()
        Me.cbBisZeit = New System.Windows.Forms.ComboBox()
        Me.cbVonZeitM = New System.Windows.Forms.ComboBox()
        Me.cbBisZeitM = New System.Windows.Forms.ComboBox()
        Me.tbBemerkung = New System.Windows.Forms.TextBox()
        Me.dgZimmerChip = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgZimmerChip, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbRFID
        '
        Me.tbRFID.Location = New System.Drawing.Point(63, 358)
        Me.tbRFID.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbRFID.Name = "tbRFID"
        Me.tbRFID.Size = New System.Drawing.Size(139, 26)
        Me.tbRFID.TabIndex = 0
        '
        'mcVon
        '
        Me.mcVon.Location = New System.Drawing.Point(219, 318)
        Me.mcVon.Margin = New System.Windows.Forms.Padding(14)
        Me.mcVon.Name = "mcVon"
        Me.mcVon.TabIndex = 1
        '
        'mcBis
        '
        Me.mcBis.Location = New System.Drawing.Point(586, 315)
        Me.mcBis.Margin = New System.Windows.Forms.Padding(14)
        Me.mcBis.Name = "mcBis"
        Me.mcBis.TabIndex = 2
        '
        'lvRFID
        '
        Me.lvRFID.HideSelection = False
        Me.lvRFID.Location = New System.Drawing.Point(63, 42)
        Me.lvRFID.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lvRFID.Name = "lvRFID"
        Me.lvRFID.Size = New System.Drawing.Size(878, 255)
        Me.lvRFID.TabIndex = 3
        Me.lvRFID.UseCompatibleStateImageBehavior = False
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbClose, Me.ToolStripSeparator1, Me.tsbSammelChip, Me.ToolStripSeparator2, Me.tsbNew, Me.tsbEdit, Me.tsbReturn, Me.tsbSave, Me.tsbDel, Me.tsbReadRFID, Me.tsbSchlossSet, Me.ToolStripSeparator3, Me.tsbReturn1, Me.tsbSave1})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(2164, 29)
        Me.ToolStrip1.TabIndex = 4
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbClose
        '
        Me.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbClose.Image = Global.Pension.My.Resources.Resources.door02
        Me.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbClose.Name = "tsbClose"
        Me.tsbClose.Size = New System.Drawing.Size(34, 24)
        Me.tsbClose.Text = "tsbClose"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 29)
        '
        'tsbSammelChip
        '
        Me.tsbSammelChip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSammelChip.Image = Global.Pension.My.Resources.Resources.ARW01DN
        Me.tsbSammelChip.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSammelChip.Name = "tsbSammelChip"
        Me.tsbSammelChip.Size = New System.Drawing.Size(34, 24)
        Me.tsbSammelChip.Text = "SammelChip"
        Me.tsbSammelChip.ToolTipText = "Sammel Chip"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 29)
        '
        'tsbNew
        '
        Me.tsbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbNew.Image = Global.Pension.My.Resources.Resources.cabview_dll_Ico14_ico_Ico1
        Me.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNew.Name = "tsbNew"
        Me.tsbNew.Size = New System.Drawing.Size(34, 24)
        Me.tsbNew.Text = "Neuer Chip"
        '
        'tsbEdit
        '
        Me.tsbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbEdit.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1474_ico_Ico1
        Me.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEdit.Name = "tsbEdit"
        Me.tsbEdit.Size = New System.Drawing.Size(34, 24)
        Me.tsbEdit.Text = "Chip bearbeiten"
        '
        'tsbReturn
        '
        Me.tsbReturn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbReturn.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsbReturn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbReturn.Name = "tsbReturn"
        Me.tsbReturn.Size = New System.Drawing.Size(34, 24)
        Me.tsbReturn.Text = "Bearbeitung abbrechen"
        '
        'tsbSave
        '
        Me.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSave.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSave.Name = "tsbSave"
        Me.tsbSave.Size = New System.Drawing.Size(34, 24)
        Me.tsbSave.Text = "Speichern"
        '
        'tsbDel
        '
        Me.tsbDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDel.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbDel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDel.Name = "tsbDel"
        Me.tsbDel.Size = New System.Drawing.Size(34, 24)
        Me.tsbDel.Text = "Chip löschen"
        '
        'tsbReadRFID
        '
        Me.tsbReadRFID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbReadRFID.Image = Global.Pension.My.Resources.Resources.rfidsign_rfi_13599
        Me.tsbReadRFID.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbReadRFID.Name = "tsbReadRFID"
        Me.tsbReadRFID.Size = New System.Drawing.Size(34, 24)
        Me.tsbReadRFID.Text = "Chips lesen und auflisten"
        '
        'tsbSchlossSet
        '
        Me.tsbSchlossSet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSchlossSet.Image = Global.Pension.My.Resources.Resources.WRENCH
        Me.tsbSchlossSet.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSchlossSet.Name = "tsbSchlossSet"
        Me.tsbSchlossSet.Size = New System.Drawing.Size(34, 24)
        Me.tsbSchlossSet.Text = "SchlossSet"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 29)
        '
        'tsbReturn1
        '
        Me.tsbReturn1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbReturn1.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsbReturn1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbReturn1.Name = "tsbReturn1"
        Me.tsbReturn1.Size = New System.Drawing.Size(34, 24)
        Me.tsbReturn1.Text = "Reset Transponder"
        '
        'tsbSave1
        '
        Me.tsbSave1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSave1.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSave1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSave1.Name = "tsbSave1"
        Me.tsbSave1.Size = New System.Drawing.Size(34, 24)
        Me.tsbSave1.Text = "Speichen der mit x markierten Transponder"
        '
        'cbVonZeit
        '
        Me.cbVonZeit.FormattingEnabled = True
        Me.cbVonZeit.Location = New System.Drawing.Point(498, 358)
        Me.cbVonZeit.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cbVonZeit.Name = "cbVonZeit"
        Me.cbVonZeit.Size = New System.Drawing.Size(59, 28)
        Me.cbVonZeit.TabIndex = 5
        '
        'cbBisZeit
        '
        Me.cbBisZeit.FormattingEnabled = True
        Me.cbBisZeit.Location = New System.Drawing.Point(881, 358)
        Me.cbBisZeit.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cbBisZeit.Name = "cbBisZeit"
        Me.cbBisZeit.Size = New System.Drawing.Size(52, 28)
        Me.cbBisZeit.TabIndex = 6
        '
        'cbVonZeitM
        '
        Me.cbVonZeitM.FormattingEnabled = True
        Me.cbVonZeitM.Location = New System.Drawing.Point(498, 394)
        Me.cbVonZeitM.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cbVonZeitM.Name = "cbVonZeitM"
        Me.cbVonZeitM.Size = New System.Drawing.Size(59, 28)
        Me.cbVonZeitM.TabIndex = 7
        '
        'cbBisZeitM
        '
        Me.cbBisZeitM.FormattingEnabled = True
        Me.cbBisZeitM.Location = New System.Drawing.Point(881, 394)
        Me.cbBisZeitM.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cbBisZeitM.Name = "cbBisZeitM"
        Me.cbBisZeitM.Size = New System.Drawing.Size(52, 28)
        Me.cbBisZeitM.TabIndex = 8
        '
        'tbBemerkung
        '
        Me.tbBemerkung.Location = New System.Drawing.Point(63, 499)
        Me.tbBemerkung.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.tbBemerkung.Name = "tbBemerkung"
        Me.tbBemerkung.Size = New System.Drawing.Size(139, 26)
        Me.tbBemerkung.TabIndex = 9
        '
        'dgZimmerChip
        '
        Me.dgZimmerChip.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgZimmerChip.Location = New System.Drawing.Point(950, 42)
        Me.dgZimmerChip.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dgZimmerChip.Name = "dgZimmerChip"
        Me.dgZimmerChip.RowHeadersWidth = 62
        Me.dgZimmerChip.RowTemplate.Height = 24
        Me.dgZimmerChip.Size = New System.Drawing.Size(1202, 534)
        Me.dgZimmerChip.TabIndex = 10
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(68, 326)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 20)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "RFID"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(68, 475)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(91, 20)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Bemerkung"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(498, 326)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(50, 20)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Von..."
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(884, 326)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 20)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Bis..."
        '
        'frmRFID
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(2164, 591)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dgZimmerChip)
        Me.Controls.Add(Me.tbBemerkung)
        Me.Controls.Add(Me.cbBisZeitM)
        Me.Controls.Add(Me.cbVonZeitM)
        Me.Controls.Add(Me.cbBisZeit)
        Me.Controls.Add(Me.cbVonZeit)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.lvRFID)
        Me.Controls.Add(Me.mcBis)
        Me.Controls.Add(Me.mcVon)
        Me.Controls.Add(Me.tbRFID)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmRFID"
        Me.Text = "RFID"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgZimmerChip, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbRFID As System.Windows.Forms.TextBox
    Friend WithEvents mcVon As System.Windows.Forms.MonthCalendar
    Friend WithEvents mcBis As System.Windows.Forms.MonthCalendar
    Friend WithEvents lvRFID As System.Windows.Forms.ListView
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbEdit As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbReturn As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbDel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents cbVonZeit As System.Windows.Forms.ComboBox
    Friend WithEvents cbBisZeit As System.Windows.Forms.ComboBox
    Protected Friend WithEvents tsbReadRFID As ToolStripButton
    Friend WithEvents tsbSchlossSet As ToolStripButton
    Friend WithEvents cbVonZeitM As ComboBox
    Friend WithEvents cbBisZeitM As ComboBox
    Friend WithEvents tbBemerkung As TextBox
    Friend WithEvents dgZimmerChip As DataGridView
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents tsbSammelChip As ToolStripButton
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents tsbReturn1 As ToolStripButton
    Friend WithEvents tsbSave1 As ToolStripButton
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
End Class
