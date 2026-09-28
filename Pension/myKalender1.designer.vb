<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class myKalender
    Inherits System.Windows.Forms.UserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(myKalender))
        Me.pKalender = New System.Windows.Forms.Panel()
        Me.lbAnAb = New System.Windows.Forms.Label()
        Me.buExet = New System.Windows.Forms.Button()
        Me.tbSperr = New System.Windows.Forms.TextBox()
        Me.tbBuchung = New System.Windows.Forms.TextBox()
        Me.tbAnAb = New System.Windows.Forms.TextBox()
        Me.tbAnreise = New System.Windows.Forms.TextBox()
        Me.tbAbreise = New System.Windows.Forms.TextBox()
        Me.tbD = New System.Windows.Forms.TextBox()
        Me.lMonat3 = New System.Windows.Forms.Label()
        Me.zurück = New System.Windows.Forms.Button()
        Me.lMonat2 = New System.Windows.Forms.Label()
        Me.vor = New System.Windows.Forms.Button()
        Me.lMonat1 = New System.Windows.Forms.Label()
        Me.dgvKalender = New System.Windows.Forms.DataGridView()
        Me.pKalender.SuspendLayout()
        CType(Me.dgvKalender, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pKalender
        '
        Me.pKalender.Controls.Add(Me.lbAnAb)
        Me.pKalender.Controls.Add(Me.buExet)
        Me.pKalender.Controls.Add(Me.tbSperr)
        Me.pKalender.Controls.Add(Me.tbBuchung)
        Me.pKalender.Controls.Add(Me.tbAnAb)
        Me.pKalender.Controls.Add(Me.tbAnreise)
        Me.pKalender.Controls.Add(Me.tbAbreise)
        Me.pKalender.Controls.Add(Me.tbD)
        Me.pKalender.Controls.Add(Me.lMonat3)
        Me.pKalender.Controls.Add(Me.zurück)
        Me.pKalender.Controls.Add(Me.lMonat2)
        Me.pKalender.Controls.Add(Me.vor)
        Me.pKalender.Controls.Add(Me.lMonat1)
        Me.pKalender.Controls.Add(Me.dgvKalender)
        Me.pKalender.Location = New System.Drawing.Point(3, 3)
        Me.pKalender.Name = "pKalender"
        Me.pKalender.Size = New System.Drawing.Size(683, 285)
        Me.pKalender.TabIndex = 0
        '
        'lbAnAb
        '
        Me.lbAnAb.AutoSize = True
        Me.lbAnAb.Location = New System.Drawing.Point(262, 0)
        Me.lbAnAb.Name = "lbAnAb"
        Me.lbAnAb.Size = New System.Drawing.Size(39, 13)
        Me.lbAnAb.TabIndex = 16
        Me.lbAnAb.Text = "Label1"
        '
        'buExet
        '
        Me.buExet.Image = CType(resources.GetObject("buExet.Image"), System.Drawing.Image)
        Me.buExet.Location = New System.Drawing.Point(632, 248)
        Me.buExet.Name = "buExet"
        Me.buExet.Size = New System.Drawing.Size(29, 23)
        Me.buExet.TabIndex = 15
        Me.buExet.UseVisualStyleBackColor = True
        '
        'tbSperr
        '
        Me.tbSperr.Location = New System.Drawing.Point(361, 262)
        Me.tbSperr.Name = "tbSperr"
        Me.tbSperr.Size = New System.Drawing.Size(103, 20)
        Me.tbSperr.TabIndex = 14
        Me.tbSperr.Visible = False
        '
        'tbBuchung
        '
        Me.tbBuchung.Location = New System.Drawing.Point(265, 262)
        Me.tbBuchung.Name = "tbBuchung"
        Me.tbBuchung.Size = New System.Drawing.Size(90, 20)
        Me.tbBuchung.TabIndex = 13
        Me.tbBuchung.Visible = False
        '
        'tbAnAb
        '
        Me.tbAnAb.Location = New System.Drawing.Point(206, 262)
        Me.tbAnAb.Name = "tbAnAb"
        Me.tbAnAb.Size = New System.Drawing.Size(53, 20)
        Me.tbAnAb.TabIndex = 12
        Me.tbAnAb.Visible = False
        '
        'tbAnreise
        '
        Me.tbAnreise.Location = New System.Drawing.Point(147, 262)
        Me.tbAnreise.Name = "tbAnreise"
        Me.tbAnreise.Size = New System.Drawing.Size(53, 20)
        Me.tbAnreise.TabIndex = 11
        Me.tbAnreise.Visible = False
        '
        'tbAbreise
        '
        Me.tbAbreise.Location = New System.Drawing.Point(80, 262)
        Me.tbAbreise.Name = "tbAbreise"
        Me.tbAbreise.Size = New System.Drawing.Size(61, 20)
        Me.tbAbreise.TabIndex = 10
        Me.tbAbreise.Visible = False
        '
        'tbD
        '
        Me.tbD.Location = New System.Drawing.Point(3, 262)
        Me.tbD.Name = "tbD"
        Me.tbD.Size = New System.Drawing.Size(60, 20)
        Me.tbD.TabIndex = 9
        Me.tbD.Visible = False
        '
        'lMonat3
        '
        Me.lMonat3.AutoSize = True
        Me.lMonat3.Location = New System.Drawing.Point(536, 21)
        Me.lMonat3.Name = "lMonat3"
        Me.lMonat3.Size = New System.Drawing.Size(39, 13)
        Me.lMonat3.TabIndex = 7
        Me.lMonat3.Text = "Label7"
        '
        'zurück
        '
        Me.zurück.BackgroundImage = CType(resources.GetObject("zurück.BackgroundImage"), System.Drawing.Image)
        Me.zurück.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.zurück.Location = New System.Drawing.Point(632, 12)
        Me.zurück.Name = "zurück"
        Me.zurück.Size = New System.Drawing.Size(29, 28)
        Me.zurück.TabIndex = 2
        Me.zurück.UseVisualStyleBackColor = True
        '
        'lMonat2
        '
        Me.lMonat2.AutoSize = True
        Me.lMonat2.Location = New System.Drawing.Point(332, 21)
        Me.lMonat2.Name = "lMonat2"
        Me.lMonat2.Size = New System.Drawing.Size(39, 13)
        Me.lMonat2.TabIndex = 6
        Me.lMonat2.Text = "Label6"
        '
        'vor
        '
        Me.vor.BackgroundImage = CType(resources.GetObject("vor.BackgroundImage"), System.Drawing.Image)
        Me.vor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.vor.Location = New System.Drawing.Point(14, 12)
        Me.vor.Name = "vor"
        Me.vor.Size = New System.Drawing.Size(28, 28)
        Me.vor.TabIndex = 1
        Me.vor.UseVisualStyleBackColor = True
        '
        'lMonat1
        '
        Me.lMonat1.AutoSize = True
        Me.lMonat1.Location = New System.Drawing.Point(77, 20)
        Me.lMonat1.Name = "lMonat1"
        Me.lMonat1.Size = New System.Drawing.Size(39, 13)
        Me.lMonat1.TabIndex = 5
        Me.lMonat1.Text = "Label5"
        '
        'dgvKalender
        '
        Me.dgvKalender.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvKalender.Location = New System.Drawing.Point(14, 46)
        Me.dgvKalender.Name = "dgvKalender"
        Me.dgvKalender.Size = New System.Drawing.Size(657, 152)
        Me.dgvKalender.TabIndex = 0
        '
        'myKalender
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pKalender)
        Me.Name = "myKalender"
        Me.Size = New System.Drawing.Size(695, 294)
        Me.pKalender.ResumeLayout(False)
        Me.pKalender.PerformLayout()
        CType(Me.dgvKalender, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pKalender As System.Windows.Forms.Panel
    Friend WithEvents lMonat1 As System.Windows.Forms.Label
    Friend WithEvents lMonat2 As System.Windows.Forms.Label
    Friend WithEvents lMonat3 As System.Windows.Forms.Label
    Friend WithEvents dgvKalender As System.Windows.Forms.DataGridView
    Friend WithEvents zurück As System.Windows.Forms.Button
    Friend WithEvents vor As System.Windows.Forms.Button
    Friend WithEvents tbD As System.Windows.Forms.TextBox
    Friend WithEvents tbAnAb As System.Windows.Forms.TextBox
    Friend WithEvents tbAnreise As System.Windows.Forms.TextBox
    Friend WithEvents tbAbreise As System.Windows.Forms.TextBox
    Friend WithEvents tbSperr As System.Windows.Forms.TextBox
    Friend WithEvents tbBuchung As System.Windows.Forms.TextBox
    Friend WithEvents buExet As System.Windows.Forms.Button
    Friend WithEvents lbAnAb As System.Windows.Forms.Label

End Class
