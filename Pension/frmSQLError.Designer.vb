<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSQLError
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
        Me.rbSQLError = New System.Windows.Forms.RadioButton()
        Me.rbWebStoerung = New System.Windows.Forms.RadioButton()
        Me.tbSQL = New System.Windows.Forms.RichTextBox()
        Me.buDelete = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'rbSQLError
        '
        Me.rbSQLError.AutoSize = True
        Me.rbSQLError.Checked = True
        Me.rbSQLError.Location = New System.Drawing.Point(34, 35)
        Me.rbSQLError.Name = "rbSQLError"
        Me.rbSQLError.Size = New System.Drawing.Size(88, 17)
        Me.rbSQLError.TabIndex = 1
        Me.rbSQLError.TabStop = True
        Me.rbSQLError.Text = "Web Störung"
        Me.rbSQLError.UseVisualStyleBackColor = True
        '
        'rbWebStoerung
        '
        Me.rbWebStoerung.AutoSize = True
        Me.rbWebStoerung.Location = New System.Drawing.Point(130, 35)
        Me.rbWebStoerung.Name = "rbWebStoerung"
        Me.rbWebStoerung.Size = New System.Drawing.Size(78, 17)
        Me.rbWebStoerung.TabIndex = 2
        Me.rbWebStoerung.TabStop = True
        Me.rbWebStoerung.Text = "SQL Fehler"
        Me.rbWebStoerung.UseVisualStyleBackColor = True
        '
        'tbSQL
        '
        Me.tbSQL.Location = New System.Drawing.Point(12, 73)
        Me.tbSQL.Name = "tbSQL"
        Me.tbSQL.Size = New System.Drawing.Size(726, 271)
        Me.tbSQL.TabIndex = 3
        Me.tbSQL.Text = ""
        '
        'buDelete
        '
        Me.buDelete.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.buDelete.Location = New System.Drawing.Point(682, 27)
        Me.buDelete.Name = "buDelete"
        Me.buDelete.Size = New System.Drawing.Size(47, 33)
        Me.buDelete.TabIndex = 4
        Me.buDelete.UseVisualStyleBackColor = True
        '
        'frmSQLError
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(750, 356)
        Me.Controls.Add(Me.buDelete)
        Me.Controls.Add(Me.tbSQL)
        Me.Controls.Add(Me.rbWebStoerung)
        Me.Controls.Add(Me.rbSQLError)
        Me.Name = "frmSQLError"
        Me.Text = "SQLError"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents rbSQLError As System.Windows.Forms.RadioButton
    Friend WithEvents rbWebStoerung As System.Windows.Forms.RadioButton
    Friend WithEvents tbSQL As System.Windows.Forms.RichTextBox
    Friend WithEvents buDelete As System.Windows.Forms.Button
End Class
