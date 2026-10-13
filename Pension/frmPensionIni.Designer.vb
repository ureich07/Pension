<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPensionIni
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
        Me.tbFeld = New System.Windows.Forms.TextBox()
        Me.buLoad = New System.Windows.Forms.Button()
        Me.buSave = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'tbFeld
        '
        Me.tbFeld.Location = New System.Drawing.Point(45, 93)
        Me.tbFeld.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbFeld.Multiline = True
        Me.tbFeld.Name = "tbFeld"
        Me.tbFeld.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbFeld.Size = New System.Drawing.Size(577, 635)
        Me.tbFeld.TabIndex = 0
        '
        'buLoad
        '
        Me.buLoad.Location = New System.Drawing.Point(45, 29)
        Me.buLoad.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.buLoad.Name = "buLoad"
        Me.buLoad.Size = New System.Drawing.Size(177, 40)
        Me.buLoad.TabIndex = 1
        Me.buLoad.Text = "Load"
        Me.buLoad.UseVisualStyleBackColor = True
        '
        'buSave
        '
        Me.buSave.Location = New System.Drawing.Point(254, 29)
        Me.buSave.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.buSave.Name = "buSave"
        Me.buSave.Size = New System.Drawing.Size(171, 38)
        Me.buSave.TabIndex = 2
        Me.buSave.Text = "Save"
        Me.buSave.UseVisualStyleBackColor = True
        '
        'frmPensionIni
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(651, 754)
        Me.Controls.Add(Me.buSave)
        Me.Controls.Add(Me.buLoad)
        Me.Controls.Add(Me.tbFeld)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPensionIni"
        Me.Text = "Pensions Ini-Datei"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbFeld As System.Windows.Forms.TextBox
    Friend WithEvents buLoad As System.Windows.Forms.Button
    Friend WithEvents buSave As System.Windows.Forms.Button
End Class
