<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEinteilung
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
        Me.dgArbeit = New System.Windows.Forms.DataGridView()
        Me.tsArbeit = New System.Windows.Forms.ToolStrip()
        Me.tsbClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel5 = New System.Windows.Forms.ToolStripLabel()
        Me.tsbPrint = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.tscoMonat = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.tscoJahr = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripLabel3 = New System.Windows.Forms.ToolStripLabel()
        Me.tscoPerson = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel6 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripLabel4 = New System.Windows.Forms.ToolStripLabel()
        Me.tscoInfo = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSaveInfo = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDelInfo = New System.Windows.Forms.ToolStripButton()
        CType(Me.dgArbeit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tsArbeit.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgArbeit
        '
        Me.dgArbeit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgArbeit.Location = New System.Drawing.Point(12, 54)
        Me.dgArbeit.Name = "dgArbeit"
        Me.dgArbeit.Size = New System.Drawing.Size(960, 310)
        Me.dgArbeit.TabIndex = 1
        '
        'tsArbeit
        '
        Me.tsArbeit.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbClose, Me.ToolStripSeparator1, Me.tsbSave, Me.ToolStripSeparator2, Me.ToolStripLabel5, Me.tsbPrint, Me.ToolStripSeparator3, Me.ToolStripLabel1, Me.tscoMonat, Me.ToolStripLabel2, Me.tscoJahr, Me.ToolStripLabel3, Me.tscoPerson, Me.ToolStripSeparator4, Me.ToolStripLabel6, Me.ToolStripLabel4, Me.tscoInfo, Me.ToolStripSeparator5, Me.tsbSaveInfo, Me.ToolStripSeparator6, Me.tsbDelInfo})
        Me.tsArbeit.Location = New System.Drawing.Point(0, 0)
        Me.tsArbeit.Name = "tsArbeit"
        Me.tsArbeit.Size = New System.Drawing.Size(986, 25)
        Me.tsArbeit.TabIndex = 2
        Me.tsArbeit.Text = "ToolStrip1"
        '
        'tsbClose
        '
        Me.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbClose.Image = Global.Pension.My.Resources.Resources.door02
        Me.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbClose.Name = "tsbClose"
        Me.tsbClose.Size = New System.Drawing.Size(23, 22)
        Me.tsbClose.Text = "ToolStripButton1"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsbSave
        '
        Me.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSave.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSave.Name = "tsbSave"
        Me.tsbSave.Size = New System.Drawing.Size(23, 22)
        Me.tsbSave.Text = "Einteilung speichern"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel5
        '
        Me.ToolStripLabel5.AutoSize = False
        Me.ToolStripLabel5.Enabled = False
        Me.ToolStripLabel5.Name = "ToolStripLabel5"
        Me.ToolStripLabel5.Size = New System.Drawing.Size(80, 22)
        '
        'tsbPrint
        '
        Me.tsbPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPrint.Image = Global.Pension.My.Resources.Resources.comdlg32_dll_Ico18_ico_Ico1
        Me.tsbPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPrint.Name = "tsbPrint"
        Me.tsbPrint.Size = New System.Drawing.Size(23, 22)
        Me.tsbPrint.Text = "Drucken"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(51, 22)
        Me.ToolStripLabel1.Text = " Monat: "
        '
        'tscoMonat
        '
        Me.tscoMonat.AutoSize = False
        Me.tscoMonat.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.tscoMonat.Name = "tscoMonat"
        Me.tscoMonat.Size = New System.Drawing.Size(50, 23)
        Me.tscoMonat.ToolTipText = "Monat"
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(37, 22)
        Me.ToolStripLabel2.Text = " Jahr: "
        '
        'tscoJahr
        '
        Me.tscoJahr.AutoSize = False
        Me.tscoJahr.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.tscoJahr.Name = "tscoJahr"
        Me.tscoJahr.Size = New System.Drawing.Size(80, 23)
        Me.tscoJahr.ToolTipText = "Jahr"
        '
        'ToolStripLabel3
        '
        Me.ToolStripLabel3.AutoSize = False
        Me.ToolStripLabel3.Name = "ToolStripLabel3"
        Me.ToolStripLabel3.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripLabel3.Text = "  Personen: "
        '
        'tscoPerson
        '
        Me.tscoPerson.AutoSize = False
        Me.tscoPerson.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.tscoPerson.Name = "tscoPerson"
        Me.tscoPerson.Size = New System.Drawing.Size(50, 23)
        Me.tscoPerson.ToolTipText = "Anzahl Personen"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel6
        '
        Me.ToolStripLabel6.AutoSize = False
        Me.ToolStripLabel6.Enabled = False
        Me.ToolStripLabel6.Name = "ToolStripLabel6"
        Me.ToolStripLabel6.Size = New System.Drawing.Size(80, 22)
        '
        'ToolStripLabel4
        '
        Me.ToolStripLabel4.Name = "ToolStripLabel4"
        Me.ToolStripLabel4.Size = New System.Drawing.Size(34, 22)
        Me.ToolStripLabel4.Text = "Info: "
        '
        'tscoInfo
        '
        Me.tscoInfo.AutoSize = False
        Me.tscoInfo.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.tscoInfo.Name = "tscoInfo"
        Me.tscoInfo.Size = New System.Drawing.Size(150, 23)
        Me.tscoInfo.Sorted = True
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 25)
        '
        'tsbSaveInfo
        '
        Me.tsbSaveInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSaveInfo.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSaveInfo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSaveInfo.Name = "tsbSaveInfo"
        Me.tsbSaveInfo.Size = New System.Drawing.Size(23, 22)
        Me.tsbSaveInfo.Text = "Info in speichern"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 25)
        '
        'tsbDelInfo
        '
        Me.tsbDelInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDelInfo.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbDelInfo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDelInfo.Name = "tsbDelInfo"
        Me.tsbDelInfo.Size = New System.Drawing.Size(23, 22)
        Me.tsbDelInfo.Text = "Info löschen"
        '
        'frmEinteilung
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(986, 387)
        Me.Controls.Add(Me.tsArbeit)
        Me.Controls.Add(Me.dgArbeit)
        Me.Name = "frmEinteilung"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Personaleinteilung"
        CType(Me.dgArbeit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tsArbeit.ResumeLayout(False)
        Me.tsArbeit.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgArbeit As System.Windows.Forms.DataGridView
    Friend WithEvents tsArbeit As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents tscoMonat As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tscoJahr As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tscoPerson As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripLabel3 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsbSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel4 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tscoInfo As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbSaveInfo As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbDelInfo As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel5 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripLabel6 As System.Windows.Forms.ToolStripLabel
End Class
