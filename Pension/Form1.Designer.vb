<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.cbZimmer = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtpVon = New System.Windows.Forms.DateTimePicker()
        Me.dtpBis = New System.Windows.Forms.DateTimePicker()
        Me.tbPersonen = New System.Windows.Forms.TextBox()
        Me.tbKinder = New System.Windows.Forms.TextBox()
        Me.lbZimID = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lbABschlag = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lbTag = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lbMin = New System.Windows.Forms.Label()
        Me.label9 = New System.Windows.Forms.Label()
        Me.lbSumme = New System.Windows.Forms.Label()
        Me.lvPreise = New System.Windows.Forms.ListView()
        Me.lbRest = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cbZimmer
        '
        Me.cbZimmer.FormattingEnabled = True
        Me.cbZimmer.Location = New System.Drawing.Point(120, 19)
        Me.cbZimmer.Name = "cbZimmer"
        Me.cbZimmer.Size = New System.Drawing.Size(273, 24)
        Me.cbZimmer.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "zimmer"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(28, 78)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 17)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "anreise"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(304, 78)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 17)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "abreise"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(541, 80)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(92, 17)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "erwachsende"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(788, 83)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(47, 17)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "kinder"
        '
        'dtpVon
        '
        Me.dtpVon.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpVon.Location = New System.Drawing.Point(98, 78)
        Me.dtpVon.Name = "dtpVon"
        Me.dtpVon.Size = New System.Drawing.Size(133, 22)
        Me.dtpVon.TabIndex = 6
        '
        'dtpBis
        '
        Me.dtpBis.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpBis.Location = New System.Drawing.Point(365, 78)
        Me.dtpBis.Name = "dtpBis"
        Me.dtpBis.Size = New System.Drawing.Size(125, 22)
        Me.dtpBis.TabIndex = 7
        '
        'tbPersonen
        '
        Me.tbPersonen.Location = New System.Drawing.Point(657, 80)
        Me.tbPersonen.Name = "tbPersonen"
        Me.tbPersonen.Size = New System.Drawing.Size(100, 22)
        Me.tbPersonen.TabIndex = 8
        '
        'tbKinder
        '
        Me.tbKinder.Location = New System.Drawing.Point(853, 80)
        Me.tbKinder.Name = "tbKinder"
        Me.tbKinder.Size = New System.Drawing.Size(100, 22)
        Me.tbKinder.TabIndex = 9
        '
        'lbZimID
        '
        Me.lbZimID.AutoSize = True
        Me.lbZimID.Location = New System.Drawing.Point(634, 31)
        Me.lbZimID.Name = "lbZimID"
        Me.lbZimID.Size = New System.Drawing.Size(55, 17)
        Me.lbZimID.TabIndex = 10
        Me.lbZimID.Text = "lbZimID"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(1012, 43)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(150, 52)
        Me.Button1.TabIndex = 11
        Me.Button1.Text = "suchen"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lbABschlag
        '
        Me.lbABschlag.AutoSize = True
        Me.lbABschlag.Location = New System.Drawing.Point(155, 190)
        Me.lbABschlag.Name = "lbABschlag"
        Me.lbABschlag.Size = New System.Drawing.Size(76, 17)
        Me.lbABschlag.TabIndex = 12
        Me.lbABschlag.Text = "lbabschlag"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(77, 190)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 17)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Abschlag"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(304, 190)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(36, 17)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "tage"
        '
        'lbTag
        '
        Me.lbTag.AutoSize = True
        Me.lbTag.Location = New System.Drawing.Point(362, 190)
        Me.lbTag.Name = "lbTag"
        Me.lbTag.Size = New System.Drawing.Size(44, 17)
        Me.lbTag.TabIndex = 15
        Me.lbTag.Text = "lbTag"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(481, 190)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(69, 17)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "MinDauer"
        '
        'lbMin
        '
        Me.lbMin.AutoSize = True
        Me.lbMin.Location = New System.Drawing.Point(576, 190)
        Me.lbMin.Name = "lbMin"
        Me.lbMin.Size = New System.Drawing.Size(41, 17)
        Me.lbMin.TabIndex = 17
        Me.lbMin.Text = "lbMin"
        '
        'label9
        '
        Me.label9.AutoSize = True
        Me.label9.Location = New System.Drawing.Point(654, 190)
        Me.label9.Name = "label9"
        Me.label9.Size = New System.Drawing.Size(55, 17)
        Me.label9.TabIndex = 18
        Me.label9.Text = "Summe"
        '
        'lbSumme
        '
        Me.lbSumme.AutoSize = True
        Me.lbSumme.Location = New System.Drawing.Point(761, 190)
        Me.lbSumme.Name = "lbSumme"
        Me.lbSumme.Size = New System.Drawing.Size(66, 17)
        Me.lbSumme.TabIndex = 19
        Me.lbSumme.Text = "lbSumme"
        '
        'lvPreise
        '
        Me.lvPreise.Location = New System.Drawing.Point(80, 237)
        Me.lvPreise.Name = "lvPreise"
        Me.lvPreise.Size = New System.Drawing.Size(1101, 288)
        Me.lvPreise.TabIndex = 20
        Me.lvPreise.UseCompatibleStateImageBehavior = False
        '
        'lbRest
        '
        Me.lbRest.AutoSize = True
        Me.lbRest.Location = New System.Drawing.Point(1023, 131)
        Me.lbRest.Name = "lbRest"
        Me.lbRest.Size = New System.Drawing.Size(48, 17)
        Me.lbRest.TabIndex = 21
        Me.lbRest.Text = "lbRest"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1289, 537)
        Me.Controls.Add(Me.lbRest)
        Me.Controls.Add(Me.lvPreise)
        Me.Controls.Add(Me.lbSumme)
        Me.Controls.Add(Me.label9)
        Me.Controls.Add(Me.lbMin)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.lbTag)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lbABschlag)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lbZimID)
        Me.Controls.Add(Me.tbKinder)
        Me.Controls.Add(Me.tbPersonen)
        Me.Controls.Add(Me.dtpBis)
        Me.Controls.Add(Me.dtpVon)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cbZimmer)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cbZimmer As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents dtpVon As DateTimePicker
    Friend WithEvents dtpBis As DateTimePicker
    Friend WithEvents tbPersonen As TextBox
    Friend WithEvents tbKinder As TextBox
    Friend WithEvents lbZimID As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents lbABschlag As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents lbTag As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents lbMin As Label
    Friend WithEvents label9 As Label
    Friend WithEvents lbSumme As Label
    Friend WithEvents lvPreise As ListView
    Friend WithEvents lbRest As Label
End Class
