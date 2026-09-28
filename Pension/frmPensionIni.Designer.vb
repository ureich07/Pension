<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPensionIni
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
        Me.tbFeld = New System.Windows.Forms.TextBox()
        Me.buLoad = New System.Windows.Forms.Button()
        Me.buSave = New System.Windows.Forms.Button()
        Me.buMwstAll = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'tbFeld
        '
        Me.tbFeld.Location = New System.Drawing.Point(30, 68)
        Me.tbFeld.Multiline = True
        Me.tbFeld.Name = "tbFeld"
        Me.tbFeld.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbFeld.Size = New System.Drawing.Size(807, 358)
        Me.tbFeld.TabIndex = 0
        '
        'buLoad
        '
        Me.buLoad.Location = New System.Drawing.Point(560, 18)
        Me.buLoad.Name = "buLoad"
        Me.buLoad.Size = New System.Drawing.Size(118, 26)
        Me.buLoad.TabIndex = 1
        Me.buLoad.Text = "Load"
        Me.buLoad.UseVisualStyleBackColor = True
        '
        'buSave
        '
        Me.buSave.Location = New System.Drawing.Point(723, 19)
        Me.buSave.Name = "buSave"
        Me.buSave.Size = New System.Drawing.Size(114, 25)
        Me.buSave.TabIndex = 2
        Me.buSave.Text = "Save"
        Me.buSave.UseVisualStyleBackColor = True
        '
        'buMwstAll
        '
        Me.buMwstAll.Enabled = False
        Me.buMwstAll.Location = New System.Drawing.Point(670, 432)
        Me.buMwstAll.Name = "buMwstAll"
        Me.buMwstAll.Size = New System.Drawing.Size(101, 36)
        Me.buMwstAll.TabIndex = 3
        Me.buMwstAll.Text = "Mwst All"
        Me.buMwstAll.UseVisualStyleBackColor = True
        Me.buMwstAll.Visible = False
        '
        'frmPensionIni
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(870, 490)
        Me.Controls.Add(Me.buMwstAll)
        Me.Controls.Add(Me.buSave)
        Me.Controls.Add(Me.buLoad)
        Me.Controls.Add(Me.tbFeld)
        Me.Name = "frmPensionIni"
        Me.Text = "IniDaten"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbFeld As System.Windows.Forms.TextBox
    Friend WithEvents buLoad As System.Windows.Forms.Button
    Friend WithEvents buSave As System.Windows.Forms.Button
    Friend WithEvents buMwstAll As System.Windows.Forms.Button
End Class
