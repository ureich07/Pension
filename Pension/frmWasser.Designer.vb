<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWasser
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lbGesammt = New System.Windows.Forms.Label()
        Me.lbBetragMwstVoll = New System.Windows.Forms.Label()
        Me.lbBetragMwstErmae = New System.Windows.Forms.Label()
        Me.tbBetragVoll = New System.Windows.Forms.TextBox()
        Me.tbBetragErmaessigt = New System.Windows.Forms.TextBox()
        Me.tbRest = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lbPro16 = New System.Windows.Forms.Label()
        Me.lbPro5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lbSumme = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(35, 66)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(146, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "NettoBetrag Mwst Voll"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(35, 105)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(184, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "NettoBetrag Mwst ermässigt"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(35, 149)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(115, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "GesammtSumme"
        '
        'lbGesammt
        '
        Me.lbGesammt.AutoSize = True
        Me.lbGesammt.Location = New System.Drawing.Point(225, 149)
        Me.lbGesammt.Name = "lbGesammt"
        Me.lbGesammt.Size = New System.Drawing.Size(16, 17)
        Me.lbGesammt.TabIndex = 3
        Me.lbGesammt.Text = "0"
        '
        'lbBetragMwstVoll
        '
        Me.lbBetragMwstVoll.AutoSize = True
        Me.lbBetragMwstVoll.Location = New System.Drawing.Point(676, 66)
        Me.lbBetragMwstVoll.Name = "lbBetragMwstVoll"
        Me.lbBetragMwstVoll.Size = New System.Drawing.Size(16, 17)
        Me.lbBetragMwstVoll.TabIndex = 4
        Me.lbBetragMwstVoll.Text = "0"
        '
        'lbBetragMwstErmae
        '
        Me.lbBetragMwstErmae.AutoSize = True
        Me.lbBetragMwstErmae.Location = New System.Drawing.Point(676, 105)
        Me.lbBetragMwstErmae.Name = "lbBetragMwstErmae"
        Me.lbBetragMwstErmae.Size = New System.Drawing.Size(16, 17)
        Me.lbBetragMwstErmae.TabIndex = 5
        Me.lbBetragMwstErmae.Text = "0"
        '
        'tbBetragVoll
        '
        Me.tbBetragVoll.Location = New System.Drawing.Point(228, 63)
        Me.tbBetragVoll.Name = "tbBetragVoll"
        Me.tbBetragVoll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tbBetragVoll.Size = New System.Drawing.Size(146, 22)
        Me.tbBetragVoll.TabIndex = 6
        Me.tbBetragVoll.Text = "0"
        '
        'tbBetragErmaessigt
        '
        Me.tbBetragErmaessigt.Location = New System.Drawing.Point(228, 102)
        Me.tbBetragErmaessigt.Name = "tbBetragErmaessigt"
        Me.tbBetragErmaessigt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tbBetragErmaessigt.Size = New System.Drawing.Size(146, 22)
        Me.tbBetragErmaessigt.TabIndex = 7
        Me.tbBetragErmaessigt.Text = "0"
        '
        'tbRest
        '
        Me.tbRest.Location = New System.Drawing.Point(228, 20)
        Me.tbRest.Name = "tbRest"
        Me.tbRest.Size = New System.Drawing.Size(146, 22)
        Me.tbRest.TabIndex = 8
        Me.tbRest.Text = "0"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(35, 25)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(158, 17)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "Nachzahlung/Guthaben"
        '
        'lbPro16
        '
        Me.lbPro16.AutoSize = True
        Me.lbPro16.Location = New System.Drawing.Point(409, 66)
        Me.lbPro16.Name = "lbPro16"
        Me.lbPro16.Size = New System.Drawing.Size(16, 17)
        Me.lbPro16.TabIndex = 11
        Me.lbPro16.Text = "0"
        '
        'lbPro5
        '
        Me.lbPro5.AutoSize = True
        Me.lbPro5.Location = New System.Drawing.Point(409, 105)
        Me.lbPro5.Name = "lbPro5"
        Me.lbPro5.Size = New System.Drawing.Size(16, 17)
        Me.lbPro5.TabIndex = 12
        Me.lbPro5.Text = "0"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(457, 66)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(20, 17)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "%"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(457, 105)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(20, 17)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "%"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(493, 66)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(110, 17)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "Betrag Mwst voll"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(493, 105)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(117, 17)
        Me.Label10.TabIndex = 16
        Me.Label10.Text = "Betrag Mwst erm."
        '
        'lbSumme
        '
        Me.lbSumme.AutoSize = True
        Me.lbSumme.Location = New System.Drawing.Point(676, 149)
        Me.lbSumme.Name = "lbSumme"
        Me.lbSumme.Size = New System.Drawing.Size(16, 17)
        Me.lbSumme.TabIndex = 17
        Me.lbSumme.Text = "0"
        '
        'frmWasser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(813, 203)
        Me.Controls.Add(Me.lbSumme)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lbPro5)
        Me.Controls.Add(Me.lbPro16)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.tbRest)
        Me.Controls.Add(Me.tbBetragErmaessigt)
        Me.Controls.Add(Me.tbBetragVoll)
        Me.Controls.Add(Me.lbBetragMwstErmae)
        Me.Controls.Add(Me.lbBetragMwstVoll)
        Me.Controls.Add(Me.lbGesammt)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmWasser"
        Me.Text = "Abwasserberechnung"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lbGesammt As Label
    Friend WithEvents lbBetragMwstVoll As Label
    Friend WithEvents lbBetragMwstErmae As Label
    Friend WithEvents tbBetragVoll As TextBox
    Friend WithEvents tbBetragErmaessigt As TextBox
    Friend WithEvents tbRest As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents lbPro16 As Label
    Friend WithEvents lbPro5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents lbSumme As Label
End Class
