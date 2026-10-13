<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCheckin
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
        Me.buClose = New System.Windows.Forms.Button()
        Me.lvTabel = New System.Windows.Forms.ListView()
        Me.SuspendLayout()
        '
        'buClose
        '
        Me.buClose.Location = New System.Drawing.Point(675, 411)
        Me.buClose.Name = "buClose"
        Me.buClose.Size = New System.Drawing.Size(73, 27)
        Me.buClose.TabIndex = 0
        Me.buClose.Text = "Close"
        Me.buClose.UseVisualStyleBackColor = True
        '
        'lvTabel
        '
        Me.lvTabel.Location = New System.Drawing.Point(12, 12)
        Me.lvTabel.Name = "lvTabel"
        Me.lvTabel.Size = New System.Drawing.Size(1285, 393)
        Me.lvTabel.TabIndex = 1
        Me.lvTabel.UseCompatibleStateImageBehavior = False
        '
        'frmCheckin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1309, 450)
        Me.Controls.Add(Me.lvTabel)
        Me.Controls.Add(Me.buClose)
        Me.Name = "frmCheckin"
        Me.Text = "Checkin"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents buClose As Button
    Friend WithEvents lvTabel As ListView
End Class
