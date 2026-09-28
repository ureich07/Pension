<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTSend
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
        Me.lvTrans = New System.Windows.Forms.ListView()
        Me.buEnde = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lvTrans
        '
        Me.lvTrans.HideSelection = False
        Me.lvTrans.Location = New System.Drawing.Point(9, 10)
        Me.lvTrans.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.lvTrans.Name = "lvTrans"
        Me.lvTrans.Size = New System.Drawing.Size(583, 332)
        Me.lvTrans.TabIndex = 0
        Me.lvTrans.UseCompatibleStateImageBehavior = False
        '
        'buEnde
        '
        Me.buEnde.Location = New System.Drawing.Point(491, 355)
        Me.buEnde.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.buEnde.Name = "buEnde"
        Me.buEnde.Size = New System.Drawing.Size(101, 34)
        Me.buEnde.TabIndex = 1
        Me.buEnde.Text = "Ende"
        Me.buEnde.UseVisualStyleBackColor = True
        '
        'frmTSend
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(600, 393)
        Me.Controls.Add(Me.buEnde)
        Me.Controls.Add(Me.lvTrans)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTSend"
        Me.Text = "Senden"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lvTrans As ListView
    Friend WithEvents buEnde As Button
End Class
