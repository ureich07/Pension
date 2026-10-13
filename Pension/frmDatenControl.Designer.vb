<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDatenControl
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
        Me.lvControll = New System.Windows.Forms.ListView()
        Me.tbDel = New System.Windows.Forms.Button()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.SuspendLayout()
        '
        'lvControll
        '
        Me.lvControll.HideSelection = False
        Me.lvControll.Location = New System.Drawing.Point(18, 114)
        Me.lvControll.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lvControll.Name = "lvControll"
        Me.lvControll.Size = New System.Drawing.Size(1795, 673)
        Me.lvControll.TabIndex = 0
        Me.lvControll.UseCompatibleStateImageBehavior = False
        '
        'tbDel
        '
        Me.tbDel.Location = New System.Drawing.Point(1645, 64)
        Me.tbDel.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbDel.Name = "tbDel"
        Me.tbDel.Size = New System.Drawing.Size(168, 35)
        Me.tbDel.TabIndex = 3
        Me.tbDel.Text = "Del."
        Me.tbDel.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(46, 69)
        Me.RadioButton1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(88, 24)
        Me.RadioButton1.TabIndex = 4
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Einfach"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(194, 69)
        Me.RadioButton2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(97, 24)
        Me.RadioButton2.TabIndex = 5
        Me.RadioButton2.Text = "Erweitert"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'frmDatenControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1848, 808)
        Me.Controls.Add(Me.RadioButton2)
        Me.Controls.Add(Me.RadioButton1)
        Me.Controls.Add(Me.tbDel)
        Me.Controls.Add(Me.lvControll)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmDatenControl"
        Me.Text = "DatenControlle"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lvControll As System.Windows.Forms.ListView
    Friend WithEvents tbDel As System.Windows.Forms.Button
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
End Class
