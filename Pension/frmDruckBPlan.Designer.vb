<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDruckBPlan
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.cmdDruckPlan = New System.Windows.Forms.Button()
        Me.cmdClosePlan = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.coDJahr = New System.Windows.Forms.ComboBox()
        Me.coDMonat = New System.Windows.Forms.ComboBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.coDBisMonat = New System.Windows.Forms.ComboBox()
        Me.VPE = New IDEALSoftware.VpeCommunity.VpeControl()
        Me.chAnschriften = New System.Windows.Forms.CheckBox()
        Me.cbProfil = New System.Windows.Forms.ComboBox()
        Me.chPreis = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'cmdDruckPlan
        '
        Me.cmdDruckPlan.Location = New System.Drawing.Point(92, 210)
        Me.cmdDruckPlan.Name = "cmdDruckPlan"
        Me.cmdDruckPlan.Size = New System.Drawing.Size(75, 23)
        Me.cmdDruckPlan.TabIndex = 7
        Me.cmdDruckPlan.Text = "Drucken"
        Me.cmdDruckPlan.UseVisualStyleBackColor = True
        '
        'cmdClosePlan
        '
        Me.cmdClosePlan.Location = New System.Drawing.Point(10, 210)
        Me.cmdClosePlan.Name = "cmdClosePlan"
        Me.cmdClosePlan.Size = New System.Drawing.Size(75, 23)
        Me.cmdClosePlan.TabIndex = 6
        Me.cmdClosePlan.Text = "Schliessen"
        Me.cmdClosePlan.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(11, 14)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(37, 13)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Monat"
        '
        'coDJahr
        '
        Me.coDJahr.FormattingEnabled = True
        Me.coDJahr.Location = New System.Drawing.Point(119, 68)
        Me.coDJahr.Name = "coDJahr"
        Me.coDJahr.Size = New System.Drawing.Size(51, 21)
        Me.coDJahr.TabIndex = 5
        '
        'coDMonat
        '
        Me.coDMonat.FormattingEnabled = True
        Me.coDMonat.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"})
        Me.coDMonat.Location = New System.Drawing.Point(119, 13)
        Me.coDMonat.Name = "coDMonat"
        Me.coDMonat.Size = New System.Drawing.Size(51, 21)
        Me.coDMonat.TabIndex = 1
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(11, 69)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(27, 13)
        Me.Label17.TabIndex = 4
        Me.Label17.Text = "Jahr"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(11, 41)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(53, 13)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "bis Monat"
        '
        'coDBisMonat
        '
        Me.coDBisMonat.FormattingEnabled = True
        Me.coDBisMonat.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"})
        Me.coDBisMonat.Location = New System.Drawing.Point(119, 41)
        Me.coDBisMonat.Name = "coDBisMonat"
        Me.coDBisMonat.Size = New System.Drawing.Size(51, 21)
        Me.coDBisMonat.TabIndex = 3
        '
        'VPE
        '
        Me.VPE.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.VPE.DocFileReadOnly = False
        Me.VPE.EnableHelpRouting = False
        Me.VPE.EnablePrintSetupDialog = True
        Me.VPE.ExternalWindow = True
        Me.VPE.GridMode = IDEALSoftware.VpeCommunity.GridMode.InForeground
        Me.VPE.GridVisible = False
        Me.VPE.Location = New System.Drawing.Point(68, -3)
        Me.VPE.Name = "VPE"
        Me.VPE.PageFormat = IDEALSoftware.VpeCommunity.PageFormat.A4
        Me.VPE.PageHeight = 29.7R
        Me.VPE.PageOrientation = IDEALSoftware.VpeCommunity.PageOrientation.Portrait
        Me.VPE.PageScroller = True
        Me.VPE.PageScrollerTracking = True
        Me.VPE.PageWidth = 21.0R
        Me.VPE.PaperView = True
        Me.VPE.PreviewCtrl = IDEALSoftware.VpeCommunity.PreviewCtrl.JumpTop
        Me.VPE.Rulers = True
        Me.VPE.RulersMeasure = IDEALSoftware.VpeCommunity.RulersMeasure.Centimeter
        Me.VPE.Size = New System.Drawing.Size(45, 37)
        Me.VPE.StatusBar = True
        Me.VPE.StatusSegment = True
        Me.VPE.SwapFileName = Nothing
        Me.VPE.TabIndex = 54
        Me.VPE.TabStop = False
        Me.VPE.tbAbout = True
        Me.VPE.tbClose = True
        Me.VPE.tbGrid = False
        Me.VPE.tbHelp = True
        Me.VPE.tbMail = True
        Me.VPE.tbNavigate = True
        Me.VPE.tbOpen = True
        Me.VPE.tbPrint = True
        Me.VPE.tbSave = True
        Me.VPE.tbScale = True
        Me.VPE.ToolBar = True
        Me.VPE.Visible = False
        '
        'chAnschriften
        '
        Me.chAnschriften.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chAnschriften.Location = New System.Drawing.Point(10, 139)
        Me.chAnschriften.Name = "chAnschriften"
        Me.chAnschriften.Size = New System.Drawing.Size(158, 24)
        Me.chAnschriften.TabIndex = 55
        Me.chAnschriften.Text = "Anschriften drucken"
        Me.chAnschriften.UseVisualStyleBackColor = True
        '
        'cbProfil
        '
        Me.cbProfil.FormattingEnabled = True
        Me.cbProfil.Location = New System.Drawing.Point(12, 170)
        Me.cbProfil.Name = "cbProfil"
        Me.cbProfil.Size = New System.Drawing.Size(156, 21)
        Me.cbProfil.TabIndex = 56
        '
        'chPreis
        '
        Me.chPreis.AutoSize = True
        Me.chPreis.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chPreis.Location = New System.Drawing.Point(14, 116)
        Me.chPreis.Margin = New System.Windows.Forms.Padding(2)
        Me.chPreis.Name = "chPreis"
        Me.chPreis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chPreis.Size = New System.Drawing.Size(155, 17)
        Me.chPreis.TabIndex = 57
        Me.chPreis.Text = "mit Preise                            "
        Me.chPreis.UseVisualStyleBackColor = True
        '
        'frmDruckBPlan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(180, 249)
        Me.ControlBox = False
        Me.Controls.Add(Me.chPreis)
        Me.Controls.Add(Me.cbProfil)
        Me.Controls.Add(Me.chAnschriften)
        Me.Controls.Add(Me.VPE)
        Me.Controls.Add(Me.cmdDruckPlan)
        Me.Controls.Add(Me.cmdClosePlan)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.coDBisMonat)
        Me.Controls.Add(Me.coDJahr)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.coDMonat)
        Me.Controls.Add(Me.Label17)
        Me.Name = "frmDruckBPlan"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Belegungsplan"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdDruckPlan As System.Windows.Forms.Button
    Friend WithEvents cmdClosePlan As System.Windows.Forms.Button
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents coDJahr As System.Windows.Forms.ComboBox
    Friend WithEvents coDMonat As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents coDBisMonat As System.Windows.Forms.ComboBox
    Friend WithEvents VPE As IDEALSoftware.VpeCommunity.VpeControl
    Friend WithEvents chAnschriften As System.Windows.Forms.CheckBox
    Friend WithEvents cbProfil As System.Windows.Forms.ComboBox
    Friend WithEvents chPreis As CheckBox
End Class
