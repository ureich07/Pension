<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRestore
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
        Me.coDatei = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.buWiederherstellen = New System.Windows.Forms.Button()
        Me.buAbbrechen = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'coDatei
        '
        Me.coDatei.BackColor = System.Drawing.SystemColors.Info
        Me.coDatei.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.coDatei.FormattingEnabled = True
        Me.coDatei.Location = New System.Drawing.Point(188, 40)
        Me.coDatei.Name = "coDatei"
        Me.coDatei.Size = New System.Drawing.Size(177, 28)
        Me.coDatei.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(31, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(118, 20)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Sicherung vom "
        '
        'buWiederherstellen
        '
        Me.buWiederherstellen.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.buWiederherstellen.Location = New System.Drawing.Point(35, 107)
        Me.buWiederherstellen.Name = "buWiederherstellen"
        Me.buWiederherstellen.Size = New System.Drawing.Size(131, 31)
        Me.buWiederherstellen.TabIndex = 7
        Me.buWiederherstellen.Text = "Wiederherstellen"
        Me.buWiederherstellen.UseVisualStyleBackColor = True
        '
        'buAbbrechen
        '
        Me.buAbbrechen.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.buAbbrechen.Location = New System.Drawing.Point(234, 107)
        Me.buAbbrechen.Name = "buAbbrechen"
        Me.buAbbrechen.Size = New System.Drawing.Size(131, 31)
        Me.buAbbrechen.TabIndex = 8
        Me.buAbbrechen.Text = "Abbrechen"
        Me.buAbbrechen.UseVisualStyleBackColor = True
        '
        'frmRestore
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(406, 218)
        Me.Controls.Add(Me.buAbbrechen)
        Me.Controls.Add(Me.buWiederherstellen)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.coDatei)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRestore"
        Me.ShowIcon = False
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents coDatei As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents buWiederherstellen As System.Windows.Forms.Button
    Friend WithEvents buAbbrechen As System.Windows.Forms.Button
End Class
