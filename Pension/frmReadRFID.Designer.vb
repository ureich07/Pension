<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReadRFID
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
        Me.lvRFID = New System.Windows.Forms.ListView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbDatum = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lbZeit = New System.Windows.Forms.Label()
        Me.rbTrue = New System.Windows.Forms.RadioButton()
        Me.rbAll = New System.Windows.Forms.RadioButton()
        Me.tbRFID = New System.Windows.Forms.TextBox()
        Me.buSuch = New System.Windows.Forms.Button()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.SuspendLayout()
        '
        'lvRFID
        '
        Me.lvRFID.Location = New System.Drawing.Point(13, 100)
        Me.lvRFID.Margin = New System.Windows.Forms.Padding(4)
        Me.lvRFID.Name = "lvRFID"
        Me.lvRFID.Size = New System.Drawing.Size(1152, 431)
        Me.lvRFID.TabIndex = 0
        Me.lvRFID.UseCompatibleStateImageBehavior = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(40, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Datum"
        '
        'lbDatum
        '
        Me.lbDatum.AutoSize = True
        Me.lbDatum.Location = New System.Drawing.Point(125, 41)
        Me.lbDatum.Name = "lbDatum"
        Me.lbDatum.Size = New System.Drawing.Size(51, 17)
        Me.lbDatum.TabIndex = 2
        Me.lbDatum.Text = "Label2"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(256, 41)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 17)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Zeit"
        '
        'lbZeit
        '
        Me.lbZeit.AutoSize = True
        Me.lbZeit.Location = New System.Drawing.Point(316, 41)
        Me.lbZeit.Name = "lbZeit"
        Me.lbZeit.Size = New System.Drawing.Size(51, 17)
        Me.lbZeit.TabIndex = 4
        Me.lbZeit.Text = "Label4"
        '
        'rbTrue
        '
        Me.rbTrue.AutoSize = True
        Me.rbTrue.Location = New System.Drawing.Point(606, 43)
        Me.rbTrue.Name = "rbTrue"
        Me.rbTrue.Size = New System.Drawing.Size(59, 21)
        Me.rbTrue.TabIndex = 5
        Me.rbTrue.TabStop = True
        Me.rbTrue.Text = "True"
        Me.rbTrue.UseVisualStyleBackColor = True
        '
        'rbAll
        '
        Me.rbAll.AutoSize = True
        Me.rbAll.Location = New System.Drawing.Point(753, 43)
        Me.rbAll.Name = "rbAll"
        Me.rbAll.Size = New System.Drawing.Size(44, 21)
        Me.rbAll.TabIndex = 6
        Me.rbAll.TabStop = True
        Me.rbAll.Text = "All"
        Me.rbAll.UseVisualStyleBackColor = True
        '
        'tbRFID
        '
        Me.tbRFID.Location = New System.Drawing.Point(898, 43)
        Me.tbRFID.Name = "tbRFID"
        Me.tbRFID.Size = New System.Drawing.Size(115, 22)
        Me.tbRFID.TabIndex = 7
        '
        'buSuch
        '
        Me.buSuch.BackgroundImage = Global.Pension.My.Resources.Resources.shell32_dll_Ico82_ico_Ico1
        Me.buSuch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.buSuch.Location = New System.Drawing.Point(1037, 33)
        Me.buSuch.Name = "buSuch"
        Me.buSuch.Size = New System.Drawing.Size(35, 33)
        Me.buSuch.TabIndex = 8
        Me.buSuch.UseVisualStyleBackColor = True
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Location = New System.Drawing.Point(398, 12)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(163, 70)
        Me.WebBrowser1.TabIndex = 9
        '
        'frmReadRFID
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1196, 544)
        Me.Controls.Add(Me.WebBrowser1)
        Me.Controls.Add(Me.buSuch)
        Me.Controls.Add(Me.tbRFID)
        Me.Controls.Add(Me.rbAll)
        Me.Controls.Add(Me.rbTrue)
        Me.Controls.Add(Me.lbZeit)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lbDatum)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lvRFID)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmReadRFID"
        Me.Text = "Read RFID"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lvRFID As System.Windows.Forms.ListView
    Friend WithEvents Label1 As Label
    Friend WithEvents lbDatum As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lbZeit As Label
    Friend WithEvents rbTrue As RadioButton
    Friend WithEvents rbAll As RadioButton
    Friend WithEvents tbRFID As TextBox
    Friend WithEvents buSuch As Button
    Friend WithEvents WebBrowser1 As WebBrowser
End Class
