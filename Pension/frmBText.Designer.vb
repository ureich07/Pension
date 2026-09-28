<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBText
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
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSpeichern = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbLableUpgrade = New System.Windows.Forms.ToolStripLabel()
        Me.tscoBText = New System.Windows.Forms.ToolStripComboBox()
        Me.tsbAdd = New System.Windows.Forms.ToolStripButton()
        Me.tslZeichen = New System.Windows.Forms.ToolStripLabel()
        Me.tslZeichen1 = New System.Windows.Forms.ToolStripLabel()
        Me.tbBText = New System.Windows.Forms.TextBox()
        Me.tsMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tsMain
        '
        Me.tsMain.AutoSize = False
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbClose, Me.ToolStripSeparator10, Me.tsbSpeichern, Me.ToolStripSeparator12, Me.tsbLableUpgrade, Me.tscoBText, Me.tsbAdd, Me.tslZeichen, Me.tslZeichen1})
        Me.tsMain.Location = New System.Drawing.Point(0, 0)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.tsMain.Size = New System.Drawing.Size(521, 24)
        Me.tsMain.Stretch = True
        Me.tsMain.TabIndex = 37
        '
        'tsbClose
        '
        Me.tsbClose.AutoSize = False
        Me.tsbClose.Image = Global.Pension.My.Resources.Resources.door02
        Me.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbClose.Name = "tsbClose"
        Me.tsbClose.Size = New System.Drawing.Size(40, 33)
        Me.tsbClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbClose.ToolTipText = "Systemeinstellungen beenden"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 24)
        '
        'tsbSpeichern
        '
        Me.tsbSpeichern.AutoSize = False
        Me.tsbSpeichern.Enabled = False
        Me.tsbSpeichern.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSpeichern.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSpeichern.Name = "tsbSpeichern"
        Me.tsbSpeichern.RightToLeftAutoMirrorImage = True
        Me.tsbSpeichern.Size = New System.Drawing.Size(40, 33)
        Me.tsbSpeichern.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbSpeichern.ToolTipText = "Änderung speichern"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(6, 24)
        '
        'tsbLableUpgrade
        '
        Me.tsbLableUpgrade.Name = "tsbLableUpgrade"
        Me.tsbLableUpgrade.Size = New System.Drawing.Size(0, 21)
        '
        'tscoBText
        '
        Me.tscoBText.Name = "tscoBText"
        Me.tscoBText.Size = New System.Drawing.Size(121, 24)
        '
        'tsbAdd
        '
        Me.tsbAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbAdd.Enabled = False
        Me.tsbAdd.Image = Global.Pension.My.Resources.Resources.MISC12
        Me.tsbAdd.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAdd.Name = "tsbAdd"
        Me.tsbAdd.Size = New System.Drawing.Size(23, 21)
        Me.tsbAdd.Text = "Makro hinzufügen"
        '
        'tslZeichen
        '
        Me.tslZeichen.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tslZeichen.AutoSize = False
        Me.tslZeichen.Name = "tslZeichen"
        Me.tslZeichen.Size = New System.Drawing.Size(56, 21)
        Me.tslZeichen.Text = "nZeichen"
        '
        'tslZeichen1
        '
        Me.tslZeichen1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tslZeichen1.Name = "tslZeichen1"
        Me.tslZeichen1.Size = New System.Drawing.Size(78, 21)
        Me.tslZeichen1.Text = "Max. Zeichen: "
        '
        'tbBText
        '
        Me.tbBText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbBText.Location = New System.Drawing.Point(12, 38)
        Me.tbBText.MaxLength = 255
        Me.tbBText.Multiline = True
        Me.tbBText.Name = "tbBText"
        Me.tbBText.Size = New System.Drawing.Size(498, 199)
        Me.tbBText.TabIndex = 38
        '
        'frmBText
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(521, 249)
        Me.ControlBox = False
        Me.Controls.Add(Me.tbBText)
        Me.Controls.Add(Me.tsMain)
        Me.Name = "frmBText"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Buchungstext bearbeiten"
        Me.tsMain.ResumeLayout(False)
        Me.tsMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tsMain As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbSpeichern As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbLableUpgrade As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tscoBText As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tsbAdd As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbBText As System.Windows.Forms.TextBox
    Friend WithEvents tslZeichen As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tslZeichen1 As System.Windows.Forms.ToolStripLabel
End Class
