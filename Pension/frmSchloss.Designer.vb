<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSchloss
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
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbEdit = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbRet = New System.Windows.Forms.ToolStripButton()
        Me.tbOpenZeit = New System.Windows.Forms.TextBox()
        Me.tbCloseZeit = New System.Windows.Forms.TextBox()
        Me.tbDoorZeit = New System.Windows.Forms.TextBox()
        Me.tbWarteZeit = New System.Windows.Forms.TextBox()
        Me.tbSMail = New System.Windows.Forms.TextBox()
        Me.tbSKey = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cbAlarm = New System.Windows.Forms.CheckBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tbBetr = New System.Windows.Forms.TextBox()
        Me.tbIPMail = New System.Windows.Forms.TextBox()
        Me.tbMail2 = New System.Windows.Forms.TextBox()
        Me.tbMail1 = New System.Windows.Forms.TextBox()
        Me.tbPort = New System.Windows.Forms.TextBox()
        Me.tbSMTP = New System.Windows.Forms.TextBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.buDel = New System.Windows.Forms.Button()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator1, Me.tsbEdit, Me.ToolStripSeparator2, Me.tsbSave, Me.ToolStripSeparator3, Me.tsbRet})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(800, 27)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "tsSchloss"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 27)
        '
        'tsbEdit
        '
        Me.tsbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbEdit.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1474_ico_Ico1
        Me.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEdit.Name = "tsbEdit"
        Me.tsbEdit.Size = New System.Drawing.Size(24, 24)
        Me.tsbEdit.Text = "Edit"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 27)
        '
        'tsbSave
        '
        Me.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSave.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSave.Name = "tsbSave"
        Me.tsbSave.Size = New System.Drawing.Size(24, 24)
        Me.tsbSave.Text = "tsbSave"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 27)
        '
        'tsbRet
        '
        Me.tsbRet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbRet.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsbRet.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRet.Name = "tsbRet"
        Me.tsbRet.Size = New System.Drawing.Size(24, 24)
        Me.tsbRet.Text = "tsbRet"
        '
        'tbOpenZeit
        '
        Me.tbOpenZeit.Enabled = False
        Me.tbOpenZeit.Location = New System.Drawing.Point(142, 16)
        Me.tbOpenZeit.Name = "tbOpenZeit"
        Me.tbOpenZeit.Size = New System.Drawing.Size(68, 22)
        Me.tbOpenZeit.TabIndex = 1
        '
        'tbCloseZeit
        '
        Me.tbCloseZeit.Enabled = False
        Me.tbCloseZeit.Location = New System.Drawing.Point(142, 47)
        Me.tbCloseZeit.Name = "tbCloseZeit"
        Me.tbCloseZeit.Size = New System.Drawing.Size(68, 22)
        Me.tbCloseZeit.TabIndex = 2
        '
        'tbDoorZeit
        '
        Me.tbDoorZeit.Enabled = False
        Me.tbDoorZeit.Location = New System.Drawing.Point(142, 75)
        Me.tbDoorZeit.Name = "tbDoorZeit"
        Me.tbDoorZeit.Size = New System.Drawing.Size(68, 22)
        Me.tbDoorZeit.TabIndex = 3
        '
        'tbWarteZeit
        '
        Me.tbWarteZeit.Enabled = False
        Me.tbWarteZeit.Location = New System.Drawing.Point(142, 103)
        Me.tbWarteZeit.Name = "tbWarteZeit"
        Me.tbWarteZeit.Size = New System.Drawing.Size(68, 22)
        Me.tbWarteZeit.TabIndex = 4
        '
        'tbSMail
        '
        Me.tbSMail.Enabled = False
        Me.tbSMail.Location = New System.Drawing.Point(113, 16)
        Me.tbSMail.Name = "tbSMail"
        Me.tbSMail.Size = New System.Drawing.Size(204, 22)
        Me.tbSMail.TabIndex = 5
        '
        'tbSKey
        '
        Me.tbSKey.Enabled = False
        Me.tbSKey.Location = New System.Drawing.Point(113, 42)
        Me.tbSKey.Name = "tbSKey"
        Me.tbSKey.Size = New System.Drawing.Size(204, 22)
        Me.tbSKey.TabIndex = 6
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Info
        Me.Panel1.Controls.Add(Me.cbAlarm)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.tbOpenZeit)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.tbCloseZeit)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.tbDoorZeit)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.tbWarteZeit)
        Me.Panel1.Location = New System.Drawing.Point(27, 41)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(335, 208)
        Me.Panel1.TabIndex = 7
        '
        'cbAlarm
        '
        Me.cbAlarm.AutoSize = True
        Me.cbAlarm.Location = New System.Drawing.Point(142, 139)
        Me.cbAlarm.Name = "cbAlarm"
        Me.cbAlarm.Size = New System.Drawing.Size(18, 17)
        Me.cbAlarm.TabIndex = 24
        Me.cbAlarm.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(36, 140)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(96, 17)
        Me.Label13.TabIndex = 25
        Me.Label13.Text = "Alarm Ja/Nein"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(36, 103)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(68, 17)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Wartezeit"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(36, 75)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(90, 17)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "ÖffnungsZeit"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(36, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 17)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "CloseZeit"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(36, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 17)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "OpenZeit"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.Info
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.Label11)
        Me.Panel2.Controls.Add(Me.Label10)
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.tbBetr)
        Me.Panel2.Controls.Add(Me.tbIPMail)
        Me.Panel2.Controls.Add(Me.tbMail2)
        Me.Panel2.Controls.Add(Me.tbMail1)
        Me.Panel2.Controls.Add(Me.tbPort)
        Me.Panel2.Controls.Add(Me.tbSMTP)
        Me.Panel2.Controls.Add(Me.tbSKey)
        Me.Panel2.Controls.Add(Me.tbSMail)
        Me.Panel2.Location = New System.Drawing.Point(382, 41)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(406, 253)
        Me.Panel2.TabIndex = 8
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(23, 184)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(50, 17)
        Me.Label12.TabIndex = 23
        Me.Label12.Text = "Betreff"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(23, 212)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(75, 17)
        Me.Label11.TabIndex = 22
        Me.Label11.Text = "MailServer"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(23, 156)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(89, 17)
        Me.Label10.TabIndex = 21
        Me.Label10.Text = "Empfänger 2"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(23, 128)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(89, 17)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Empfänger 1"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(23, 100)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(34, 17)
        Me.Label8.TabIndex = 19
        Me.Label8.Text = "Port"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(23, 73)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(84, 17)
        Me.Label7.TabIndex = 18
        Me.Label7.Text = "smtp Server"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(23, 45)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(32, 17)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "Key"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(23, 19)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(70, 17)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Send Mail"
        '
        'tbBetr
        '
        Me.tbBetr.Enabled = False
        Me.tbBetr.Location = New System.Drawing.Point(113, 181)
        Me.tbBetr.Name = "tbBetr"
        Me.tbBetr.Size = New System.Drawing.Size(204, 22)
        Me.tbBetr.TabIndex = 12
        '
        'tbIPMail
        '
        Me.tbIPMail.Enabled = False
        Me.tbIPMail.Location = New System.Drawing.Point(113, 209)
        Me.tbIPMail.Name = "tbIPMail"
        Me.tbIPMail.Size = New System.Drawing.Size(204, 22)
        Me.tbIPMail.TabIndex = 11
        '
        'tbMail2
        '
        Me.tbMail2.Enabled = False
        Me.tbMail2.Location = New System.Drawing.Point(113, 153)
        Me.tbMail2.Name = "tbMail2"
        Me.tbMail2.Size = New System.Drawing.Size(204, 22)
        Me.tbMail2.TabIndex = 10
        '
        'tbMail1
        '
        Me.tbMail1.Enabled = False
        Me.tbMail1.Location = New System.Drawing.Point(113, 125)
        Me.tbMail1.Name = "tbMail1"
        Me.tbMail1.Size = New System.Drawing.Size(204, 22)
        Me.tbMail1.TabIndex = 9
        '
        'tbPort
        '
        Me.tbPort.Enabled = False
        Me.tbPort.Location = New System.Drawing.Point(113, 97)
        Me.tbPort.Name = "tbPort"
        Me.tbPort.Size = New System.Drawing.Size(204, 22)
        Me.tbPort.TabIndex = 8
        '
        'tbSMTP
        '
        Me.tbSMTP.Enabled = False
        Me.tbSMTP.Location = New System.Drawing.Point(113, 70)
        Me.tbSMTP.Name = "tbSMTP"
        Me.tbSMTP.Size = New System.Drawing.Size(204, 22)
        Me.tbSMTP.TabIndex = 7
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.SystemColors.Info
        Me.Panel3.Controls.Add(Me.buDel)
        Me.Panel3.Location = New System.Drawing.Point(27, 255)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(335, 68)
        Me.Panel3.TabIndex = 9
        '
        'buDel
        '
        Me.buDel.Location = New System.Drawing.Point(15, 15)
        Me.buDel.Name = "buDel"
        Me.buDel.Size = New System.Drawing.Size(301, 39)
        Me.buDel.TabIndex = 10
        Me.buDel.Text = "alle Chips Löschen "
        Me.buDel.UseVisualStyleBackColor = True
        '
        'frmSchloss
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "frmSchloss"
        Me.Text = "Schloss Set"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents tbOpenZeit As TextBox
    Friend WithEvents tbCloseZeit As TextBox
    Friend WithEvents tbDoorZeit As TextBox
    Friend WithEvents tbWarteZeit As TextBox
    Friend WithEvents tbSMail As TextBox
    Friend WithEvents tbSKey As TextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label12 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents tbBetr As TextBox
    Friend WithEvents tbIPMail As TextBox
    Friend WithEvents tbMail2 As TextBox
    Friend WithEvents tbMail1 As TextBox
    Friend WithEvents tbPort As TextBox
    Friend WithEvents tbSMTP As TextBox
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents tsbEdit As ToolStripButton
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents tsbSave As ToolStripButton
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents tsbRet As ToolStripButton
    Friend WithEvents cbAlarm As CheckBox
    Friend WithEvents Label13 As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents buDel As Button
End Class
