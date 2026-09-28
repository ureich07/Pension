<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPauschal
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
        Me.cbBar = New System.Windows.Forms.CheckBox()
        Me.dtpARDatum = New System.Windows.Forms.DateTimePicker()
        Me.lbRech = New System.Windows.Forms.Label()
        Me.lbBID = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'buDruck
        '
        Me.buDruck.Location = New System.Drawing.Point(36, 25)
        Me.buDruck.Name = "buDruck"
        Me.buDruck.Size = New System.Drawing.Size(104, 23)
        Me.buDruck.TabIndex = 0
        Me.buDruck.Text = "Drucken"
        Me.buDruck.UseVisualStyleBackColor = True
        '
        'buMail
        '
        Me.buMail.Location = New System.Drawing.Point(36, 66)
        Me.buMail.Name = "buMail"
        Me.buMail.Size = New System.Drawing.Size(104, 23)
        Me.buMail.TabIndex = 1
        Me.buMail.Text = "Mail"
        Me.buMail.UseVisualStyleBackColor = True
        '
        'buClose
        '
        Me.buClose.Location = New System.Drawing.Point(36, 108)
        Me.buClose.Name = "buClose"
        Me.buClose.Size = New System.Drawing.Size(104, 24)
        Me.buClose.TabIndex = 2
        Me.buClose.Text = "Schließen"
        Me.buClose.UseVisualStyleBackColor = True
        '
        'cbBar
        '
        Me.cbBar.AutoSize = True
        Me.cbBar.Location = New System.Drawing.Point(172, 113)
        Me.cbBar.Name = "cbBar"
        Me.cbBar.Size = New System.Drawing.Size(42, 17)
        Me.cbBar.TabIndex = 3
        Me.cbBar.Text = "Bar"
        Me.cbBar.UseVisualStyleBackColor = True
        '
        'dtpARDatum
        '
        Me.dtpARDatum.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpARDatum.Location = New System.Drawing.Point(299, 72)
        Me.dtpARDatum.Name = "dtpARDatum"
        Me.dtpARDatum.Size = New System.Drawing.Size(79, 20)
        Me.dtpARDatum.TabIndex = 4
        '
        'lbRech
        '
        Me.lbRech.AutoSize = True
        Me.lbRech.Location = New System.Drawing.Point(296, 45)
        Me.lbRech.Name = "lbRech"
        Me.lbRech.Size = New System.Drawing.Size(43, 13)
        Me.lbRech.TabIndex = 5
        Me.lbRech.Text = "000000"
        '
        'lbBID
        '
        Me.lbBID.AutoSize = True
        Me.lbBID.Location = New System.Drawing.Point(369, 15)
        Me.lbBID.Name = "lbBID"
        Me.lbBID.Size = New System.Drawing.Size(37, 13)
        Me.lbBID.TabIndex = 6
        Me.lbBID.Text = "00000"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(169, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Rechnungs Nr."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(169, 76)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Rechnungs Datum"
        '
        'frmPauschal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(499, 229)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lbBID)
        Me.Controls.Add(Me.lbRech)
        Me.Controls.Add(Me.dtpARDatum)
        Me.Controls.Add(Me.cbBar)
        Me.Controls.Add(Me.buClose)
        Me.Controls.Add(Me.buMail)
        Me.Controls.Add(Me.buDruck)
        Me.Name = "frmPauschal"
        Me.Text = "Pauschal Rechnung"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents buDruck As System.Windows.Forms.Button
    Friend WithEvents buMail As System.Windows.Forms.Button
    Friend WithEvents buClose As System.Windows.Forms.Button
    Friend WithEvents cbBar As System.Windows.Forms.CheckBox
    Friend WithEvents dtpARDatum As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbRech As System.Windows.Forms.Label
    Friend WithEvents lbBID As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
