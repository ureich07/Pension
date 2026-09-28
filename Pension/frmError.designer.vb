<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmError
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tbErrtext = New System.Windows.Forms.TextBox
        Me.ToolStrip = New System.Windows.Forms.ToolStrip
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.VPE = New IDEALSoftware.VpeCommunity.VpeControl
        Me.tsbClose = New System.Windows.Forms.ToolStripButton
        Me.tsbDelete = New System.Windows.Forms.ToolStripButton
        Me.ToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbErrtext
        '
        Me.tbErrtext.Location = New System.Drawing.Point(11, 27)
        Me.tbErrtext.Multiline = True
        Me.tbErrtext.Name = "tbErrtext"
        Me.tbErrtext.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbErrtext.Size = New System.Drawing.Size(749, 293)
        Me.tbErrtext.TabIndex = 0
        '
        'ToolStrip
        '
        Me.ToolStrip.AutoSize = False
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbClose, Me.ToolStripSeparator5, Me.tsbDelete})
        Me.ToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.ToolStrip.Size = New System.Drawing.Size(772, 24)
        Me.ToolStrip.Stretch = True
        Me.ToolStrip.TabIndex = 23
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 24)
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
        Me.VPE.Location = New System.Drawing.Point(0, 290)
        Me.VPE.Name = "VPE"
        Me.VPE.PageFormat = IDEALSoftware.VpeCommunity.PageFormat.A4
        Me.VPE.PageHeight = 29.7
        Me.VPE.PageOrientation = IDEALSoftware.VpeCommunity.PageOrientation.Portrait
        Me.VPE.PageScroller = True
        Me.VPE.PageScrollerTracking = True
        Me.VPE.PageWidth = 21
        Me.VPE.PaperView = True
        Me.VPE.PreviewCtrl = IDEALSoftware.VpeCommunity.PreviewCtrl.JumpTop
        Me.VPE.Rulers = True
        Me.VPE.RulersMeasure = IDEALSoftware.VpeCommunity.RulersMeasure.Centimeter
        Me.VPE.Size = New System.Drawing.Size(32, 30)
        Me.VPE.StatusBar = True
        Me.VPE.StatusSegment = True
        Me.VPE.SwapFileName = Nothing
        Me.VPE.TabIndex = 52
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
        Me.VPE.Text = "VPE"
        Me.VPE.ToolBar = True
        Me.VPE.Visible = False
        '
        'tsbClose
        '
        Me.tsbClose.AutoSize = False
        Me.tsbClose.Image = Global.Pension.My.Resources.Resources.door02
        Me.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbClose.Name = "tsbClose"
        Me.tsbClose.Size = New System.Drawing.Size(40, 33)
        Me.tsbClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbClose.ToolTipText = "Close"
        '
        'tsbDelete
        '
        Me.tsbDelete.AutoSize = False
        Me.tsbDelete.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDelete.Name = "tsbDelete"
        Me.tsbDelete.Size = New System.Drawing.Size(40, 33)
        Me.tsbDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbDelete.ToolTipText = "Delete"
        '
        'frmError
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(772, 330)
        Me.Controls.Add(Me.VPE)
        Me.Controls.Add(Me.ToolStrip)
        Me.Controls.Add(Me.tbErrtext)
        Me.Name = "frmError"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Error log"
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbErrtext As System.Windows.Forms.TextBox
    Friend WithEvents ToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents VPE As IDEALSoftware.VpeCommunity.VpeControl
End Class
