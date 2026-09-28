<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAnzahlung
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
        Me.buDruck = New System.Windows.Forms.Button()
        Me.buMail = New System.Windows.Forms.Button()
        Me.buClose = New System.Windows.Forms.Button()
        Me.dtpARDatum = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbRech = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lbBID = New System.Windows.Forms.Label()
        Me.cbBar = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'buDruck
        '
        Me.buDruck.Location = New System.Drawing.Point(51, 37)
        Me.buDruck.Name = "buDruck"
        Me.buDruck.Size = New System.Drawing.Size(81, 26)
        Me.buDruck.TabIndex = 0
        Me.buDruck.Text = "Druck"
        Me.buDruck.UseVisualStyleBackColor = True
        '
        'buMail
        '
        Me.buMail.Location = New System.Drawing.Point(51, 82)
        Me.buMail.Name = "buMail"
        Me.buMail.Size = New System.Drawing.Size(81, 26)
        Me.buMail.TabIndex = 1
        Me.buMail.Text = "Mail"
        Me.buMail.UseVisualStyleBackColor = True
        '
        'buClose
        '
        Me.buClose.Location = New System.Drawing.Point(51, 124)
        Me.buClose.Name = "buClose"
        Me.buClose.Size = New System.Drawing.Size(81, 26)
        Me.buClose.TabIndex = 2
        Me.buClose.Text = "Schließen"
        Me.buClose.UseVisualStyleBackColor = True
        '
        'dtpARDatum
        '
        Me.dtpARDatum.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpARDatum.Location = New System.Drawing.Point(278, 65)
        Me.dtpARDatum.Name = "dtpARDatum"
        Me.dtpARDatum.Size = New System.Drawing.Size(101, 20)
        Me.dtpARDatum.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(171, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Rechnungs Nr."
        '
        'lbRech
        '
        Me.lbRech.AutoSize = True
        Me.lbRech.Location = New System.Drawing.Point(286, 37)
        Me.lbRech.Name = "lbRech"
        Me.lbRech.Size = New System.Drawing.Size(43, 13)
        Me.lbRech.TabIndex = 5
        Me.lbRech.Text = "000000"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(171, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Rechnungs Datum"
        '
        'lbBID
        '
        Me.lbBID.AutoSize = True
        Me.lbBID.Location = New System.Drawing.Point(339, 9)
        Me.lbBID.Name = "lbBID"
        Me.lbBID.Size = New System.Drawing.Size(37, 13)
        Me.lbBID.TabIndex = 7
        Me.lbBID.Text = "00000"
        '
        'cbBar
        '
        Me.cbBar.AutoSize = True
        Me.cbBar.Location = New System.Drawing.Point(174, 91)
        Me.cbBar.Name = "cbBar"
        Me.cbBar.Size = New System.Drawing.Size(42, 17)
        Me.cbBar.TabIndex = 8
        Me.cbBar.Text = "Bar"
        Me.cbBar.UseVisualStyleBackColor = True
        '
        'frmAnzahlung
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(391, 171)
        Me.Controls.Add(Me.cbBar)
        Me.Controls.Add(Me.lbBID)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lbRech)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dtpARDatum)
        Me.Controls.Add(Me.buClose)
        Me.Controls.Add(Me.buMail)
        Me.Controls.Add(Me.buDruck)
        Me.Name = "frmAnzahlung"
        Me.Text = "Anzahlung"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents buDruck As System.Windows.Forms.Button
    Friend WithEvents buMail As System.Windows.Forms.Button
    Friend WithEvents buClose As System.Windows.Forms.Button
    Friend WithEvents dtpARDatum As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbRech As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lbBID As System.Windows.Forms.Label
    Friend WithEvents cbBar As System.Windows.Forms.CheckBox
End Class
