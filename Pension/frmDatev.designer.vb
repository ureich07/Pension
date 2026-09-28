<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDatev
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
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbClose = New System.Windows.Forms.ToolStripButton()
        Me.tsbSperren = New System.Windows.Forms.ToolStripButton()
        Me.tsbFreigeben = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.dsbDatevFile = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbKasse = New System.Windows.Forms.ToolStripButton()
        Me.tsbBank = New System.Windows.Forms.ToolStripButton()
        Me.tsbFrei = New System.Windows.Forms.ToolStripButton()
        Me.tsbDel = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbFilter = New System.Windows.Forms.ToolStripButton()
        Me.dgvDatev = New System.Windows.Forms.DataGridView()
        Me.lbKasse = New System.Windows.Forms.Label()
        Me.tbDatum = New System.Windows.Forms.TextBox()
        Me.tbKasse = New System.Windows.Forms.TextBox()
        Me.tbBank = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgvDatev, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbClose, Me.tsbSperren, Me.tsbFreigeben, Me.ToolStripSeparator1, Me.dsbDatevFile, Me.ToolStripSeparator2, Me.tsbKasse, Me.tsbBank, Me.tsbFrei, Me.tsbDel, Me.ToolStripSeparator3, Me.tsbFilter})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1269, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbClose
        '
        Me.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbClose.Image = Global.Pension.My.Resources.Resources.door02
        Me.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbClose.Name = "tsbClose"
        Me.tsbClose.Size = New System.Drawing.Size(23, 22)
        Me.tsbClose.Text = "ToolStripButton1"
        Me.tsbClose.ToolTipText = "Ende"
        '
        'tsbSperren
        '
        Me.tsbSperren.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSperren.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsbSperren.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSperren.Name = "tsbSperren"
        Me.tsbSperren.Size = New System.Drawing.Size(23, 22)
        Me.tsbSperren.Text = "Datensatz sperren"
        Me.tsbSperren.ToolTipText = "Datensatz Sperren"
        '
        'tsbFreigeben
        '
        Me.tsbFreigeben.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFreigeben.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico965_ico_Ico1
        Me.tsbFreigeben.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFreigeben.Name = "tsbFreigeben"
        Me.tsbFreigeben.Size = New System.Drawing.Size(23, 22)
        Me.tsbFreigeben.Text = "Datensatz Freigeben"
        Me.tsbFreigeben.ToolTipText = "Datensatz Freigeben"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'dsbDatevFile
        '
        Me.dsbDatevFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.dsbDatevFile.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.dsbDatevFile.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.dsbDatevFile.Name = "dsbDatevFile"
        Me.dsbDatevFile.Size = New System.Drawing.Size(23, 22)
        Me.dsbDatevFile.Text = "Datev Datei erstellen"
        Me.dsbDatevFile.ToolTipText = "Datev Datei erstellen"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'tsbKasse
        '
        Me.tsbKasse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbKasse.Image = Global.Pension.My.Resources.Resources.COrec81
        Me.tsbKasse.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbKasse.Name = "tsbKasse"
        Me.tsbKasse.Size = New System.Drawing.Size(23, 22)
        Me.tsbKasse.Text = "Kasse"
        Me.tsbKasse.ToolTipText = "Kasse"
        '
        'tsbBank
        '
        Me.tsbBank.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbBank.Image = Global.Pension.My.Resources.Resources.COrec69
        Me.tsbBank.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbBank.Name = "tsbBank"
        Me.tsbBank.Size = New System.Drawing.Size(23, 22)
        Me.tsbBank.Text = "Bank"
        Me.tsbBank.ToolTipText = "Bank"
        '
        'tsbFrei
        '
        Me.tsbFrei.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFrei.Image = Global.Pension.My.Resources.Resources.MOVER
        Me.tsbFrei.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFrei.Name = "tsbFrei"
        Me.tsbFrei.Size = New System.Drawing.Size(23, 22)
        Me.tsbFrei.Text = "Freigeben"
        '
        'tsbDel
        '
        Me.tsbDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDel.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbDel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDel.Name = "tsbDel"
        Me.tsbDel.Size = New System.Drawing.Size(23, 22)
        Me.tsbDel.Text = "ToolStripButton1"
        Me.tsbDel.ToolTipText = "Löschen"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
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
        'dgvDatev
        '
        Me.dgvDatev.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDatev.Location = New System.Drawing.Point(14, 40)
        Me.dgvDatev.Name = "dgvDatev"
        Me.dgvDatev.Size = New System.Drawing.Size(1033, 305)
        Me.dgvDatev.TabIndex = 2
        '
        'lbKasse
        '
        Me.lbKasse.AutoSize = True
        Me.lbKasse.Location = New System.Drawing.Point(283, 6)
        Me.lbKasse.Name = "lbKasse"
        Me.lbKasse.Size = New System.Drawing.Size(36, 13)
        Me.lbKasse.TabIndex = 3
        Me.lbKasse.Text = "Kasse"
        '
        'tbDatum
        '
        Me.tbDatum.Location = New System.Drawing.Point(71, 359)
        Me.tbDatum.Name = "tbDatum"
        Me.tbDatum.Size = New System.Drawing.Size(88, 20)
        Me.tbDatum.TabIndex = 4
        '
        'tbKasse
        '
        Me.tbKasse.Location = New System.Drawing.Point(286, 359)
        Me.tbKasse.Name = "tbKasse"
        Me.tbKasse.Size = New System.Drawing.Size(109, 20)
        Me.tbKasse.TabIndex = 5
        '
        'tbBank
        '
        Me.tbBank.Location = New System.Drawing.Point(514, 359)
        Me.tbBank.Name = "tbBank"
        Me.tbBank.Size = New System.Drawing.Size(109, 20)
        Me.tbBank.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(415, 362)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Buch Nr. Bank"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(27, 362)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Datum"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(183, 362)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(81, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Buch Nr. Kasse"
        '
        'frmDatev
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1269, 391)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbBank)
        Me.Controls.Add(Me.tbKasse)
        Me.Controls.Add(Me.tbDatum)
        Me.Controls.Add(Me.lbKasse)
        Me.Controls.Add(Me.dgvDatev)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "frmDatev"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Datev"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgvDatev, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSperren As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbFreigeben As System.Windows.Forms.ToolStripButton
    Friend WithEvents dsbDatevFile As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbKasse As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbBank As System.Windows.Forms.ToolStripButton
    Friend WithEvents dgvDatev As System.Windows.Forms.DataGridView
    Friend WithEvents tsbFilter As System.Windows.Forms.ToolStripButton
    Friend WithEvents lbKasse As System.Windows.Forms.Label
    Friend WithEvents tbDatum As System.Windows.Forms.TextBox
    Friend WithEvents tbKasse As System.Windows.Forms.TextBox
    Friend WithEvents tbBank As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbFrei As System.Windows.Forms.ToolStripButton
End Class
