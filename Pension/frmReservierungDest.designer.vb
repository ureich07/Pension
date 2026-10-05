<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReservierungDest
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReservierungDest))
        Me.tbAbreise = New System.Windows.Forms.Button()
        Me.tbAnreise = New System.Windows.Forms.Button()
        Me.lbTage = New System.Windows.Forms.Label()
        Me.lbAbreise = New System.Windows.Forms.Label()
        Me.lbAnreise = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gbZimmer = New System.Windows.Forms.GroupBox()
        Me.lbMailList = New System.Windows.Forms.Label()
        Me.cbPausch = New System.Windows.Forms.CheckBox()
        Me.lbPauschPreis = New System.Windows.Forms.Label()
        Me.tbAbZeit = New System.Windows.Forms.TextBox()
        Me.tbAnZeit = New System.Windows.Forms.TextBox()
        Me.tbSumme = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.buBerechnung = New System.Windows.Forms.Button()
        Me.cbBezalt = New System.Windows.Forms.CheckBox()
        Me.tbStorno = New System.Windows.Forms.TextBox()
        Me.tbFPreis = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.tbAnzahlung = New System.Windows.Forms.TextBox()
        Me.coArt = New System.Windows.Forms.ComboBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.tbUArt = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.rbVariabel = New System.Windows.Forms.RadioButton()
        Me.tbPreis = New System.Windows.Forms.TextBox()
        Me.rbFest = New System.Windows.Forms.RadioButton()
        Me.tbAnzPer = New System.Windows.Forms.TextBox()
        Me.rbNormal = New System.Windows.Forms.RadioButton()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.lbBetten = New System.Windows.Forms.Label()
        Me.lbObjekt = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lbAusstattung = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lbArt = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lbZimNr = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.gbGast = New System.Windows.Forms.GroupBox()
        Me.cbZusatz = New System.Windows.Forms.CheckBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.tbInternetNr = New System.Windows.Forms.TextBox()
        Me.coLand = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lbGast = New System.Windows.Forms.Label()
        Me.btGast = New System.Windows.Forms.Button()
        Me.tbStrasse = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.coAnrede = New System.Windows.Forms.ComboBox()
        Me.dpGeb = New System.Windows.Forms.DateTimePicker()
        Me.lbGastID = New System.Windows.Forms.Label()
        Me.tbTel = New System.Windows.Forms.TextBox()
        Me.tbFax = New System.Windows.Forms.TextBox()
        Me.tbFunk = New System.Windows.Forms.TextBox()
        Me.tbEMail = New System.Windows.Forms.TextBox()
        Me.tbOrt = New System.Windows.Forms.TextBox()
        Me.tbInfo = New System.Windows.Forms.TextBox()
        Me.tbPLZ = New System.Windows.Forms.TextBox()
        Me.tbVorname = New System.Windows.Forms.TextBox()
        Me.tbName2 = New System.Windows.Forms.TextBox()
        Me.tbName1 = New System.Windows.Forms.TextBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.coWerbung = New System.Windows.Forms.ComboBox()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lvGast = New System.Windows.Forms.ListView()
        Me.tsMain = New System.Windows.Forms.ToolStrip()
        Me.tsbClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tsbAufZimmer = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbBestätigung = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbMail = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbRechnung = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbAnzahlungRechnung = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbBezahlt = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbBewertung = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmStorno = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbErinnerung = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDelReservierung = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbDelZimmer = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmKopie = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbCheckin = New System.Windows.Forms.ToolStripMenuItem()
        Me.PreisToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.tscoZim = New System.Windows.Forms.ToolStripComboBox()
        Me.tscoFreiZim = New System.Windows.Forms.ToolStripComboBox()
        Me.tsbAddZimmer = New System.Windows.Forms.ToolStripButton()
        Me.lbSumme = New System.Windows.Forms.ToolStripLabel()
        Me.tsbVorAnreise = New System.Windows.Forms.ToolStripButton()
        Me.tsbNachAbreise = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.tslCode = New System.Windows.Forms.ToolStripLabel()
        Me.tsbKey = New System.Windows.Forms.ToolStripButton()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel4 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tssInfo = New System.Windows.Forms.ToolStripStatusLabel()
        Me.grBText = New System.Windows.Forms.GroupBox()
        Me.tbRechSend = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.coBText = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lbMakro = New System.Windows.Forms.Label()
        Me.btBText = New System.Windows.Forms.Button()
        Me.coLang = New System.Windows.Forms.ComboBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.gbNameZ = New System.Windows.Forms.GroupBox()
        Me.tbName2Z = New System.Windows.Forms.TextBox()
        Me.tbName1Z = New System.Windows.Forms.TextBox()
        Me.MyKalender1 = New Pension.myKalender()
        Me.gbZimmer.SuspendLayout()
        Me.gbGast.SuspendLayout()
        Me.tsMain.SuspendLayout()
        Me.ssMain.SuspendLayout()
        Me.grBText.SuspendLayout()
        Me.gbNameZ.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbAbreise
        '
        Me.tbAbreise.Image = CType(resources.GetObject("tbAbreise.Image"), System.Drawing.Image)
        Me.tbAbreise.Location = New System.Drawing.Point(507, 22)
        Me.tbAbreise.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbAbreise.Name = "tbAbreise"
        Me.tbAbreise.Size = New System.Drawing.Size(32, 37)
        Me.tbAbreise.TabIndex = 7
        Me.tbAbreise.UseVisualStyleBackColor = True
        '
        'tbAnreise
        '
        Me.tbAnreise.Image = CType(resources.GetObject("tbAnreise.Image"), System.Drawing.Image)
        Me.tbAnreise.Location = New System.Drawing.Point(207, 25)
        Me.tbAnreise.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbAnreise.Name = "tbAnreise"
        Me.tbAnreise.Size = New System.Drawing.Size(28, 31)
        Me.tbAnreise.TabIndex = 6
        Me.tbAnreise.UseVisualStyleBackColor = True
        '
        'lbTage
        '
        Me.lbTage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbTage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbTage.Location = New System.Drawing.Point(744, 28)
        Me.lbTage.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbTage.Name = "lbTage"
        Me.lbTage.Size = New System.Drawing.Size(100, 30)
        Me.lbTage.TabIndex = 5
        Me.lbTage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbAbreise
        '
        Me.lbAbreise.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbAbreise.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbAbreise.Location = New System.Drawing.Point(400, 25)
        Me.lbAbreise.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbAbreise.Name = "lbAbreise"
        Me.lbAbreise.Size = New System.Drawing.Size(96, 30)
        Me.lbAbreise.TabIndex = 4
        Me.lbAbreise.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbAnreise
        '
        Me.lbAnreise.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbAnreise.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbAnreise.Location = New System.Drawing.Point(90, 25)
        Me.lbAnreise.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbAnreise.Name = "lbAnreise"
        Me.lbAnreise.Size = New System.Drawing.Size(107, 30)
        Me.lbAnreise.TabIndex = 3
        Me.lbAnreise.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(626, 28)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 31)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Anzahl Tage:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(332, 25)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 31)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Abreise:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(15, 25)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 31)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Anreise:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbZimmer
        '
        Me.gbZimmer.BackColor = System.Drawing.SystemColors.Info
        Me.gbZimmer.Controls.Add(Me.lbMailList)
        Me.gbZimmer.Controls.Add(Me.cbPausch)
        Me.gbZimmer.Controls.Add(Me.lbPauschPreis)
        Me.gbZimmer.Controls.Add(Me.tbAbZeit)
        Me.gbZimmer.Controls.Add(Me.tbAnZeit)
        Me.gbZimmer.Controls.Add(Me.tbSumme)
        Me.gbZimmer.Controls.Add(Me.tbAbreise)
        Me.gbZimmer.Controls.Add(Me.Label26)
        Me.gbZimmer.Controls.Add(Me.buBerechnung)
        Me.gbZimmer.Controls.Add(Me.tbAnreise)
        Me.gbZimmer.Controls.Add(Me.lbTage)
        Me.gbZimmer.Controls.Add(Me.lbAbreise)
        Me.gbZimmer.Controls.Add(Me.cbBezalt)
        Me.gbZimmer.Controls.Add(Me.Label3)
        Me.gbZimmer.Controls.Add(Me.lbAnreise)
        Me.gbZimmer.Controls.Add(Me.tbStorno)
        Me.gbZimmer.Controls.Add(Me.tbFPreis)
        Me.gbZimmer.Controls.Add(Me.Label22)
        Me.gbZimmer.Controls.Add(Me.tbAnzahlung)
        Me.gbZimmer.Controls.Add(Me.coArt)
        Me.gbZimmer.Controls.Add(Me.Label15)
        Me.gbZimmer.Controls.Add(Me.tbUArt)
        Me.gbZimmer.Controls.Add(Me.Label17)
        Me.gbZimmer.Controls.Add(Me.Label2)
        Me.gbZimmer.Controls.Add(Me.Label1)
        Me.gbZimmer.Controls.Add(Me.rbVariabel)
        Me.gbZimmer.Controls.Add(Me.tbPreis)
        Me.gbZimmer.Controls.Add(Me.rbFest)
        Me.gbZimmer.Controls.Add(Me.tbAnzPer)
        Me.gbZimmer.Controls.Add(Me.rbNormal)
        Me.gbZimmer.Controls.Add(Me.Label19)
        Me.gbZimmer.Controls.Add(Me.lbBetten)
        Me.gbZimmer.Controls.Add(Me.lbObjekt)
        Me.gbZimmer.Controls.Add(Me.Label11)
        Me.gbZimmer.Controls.Add(Me.lbAusstattung)
        Me.gbZimmer.Controls.Add(Me.Label13)
        Me.gbZimmer.Controls.Add(Me.Label21)
        Me.gbZimmer.Controls.Add(Me.Label9)
        Me.gbZimmer.Controls.Add(Me.lbArt)
        Me.gbZimmer.Controls.Add(Me.Label7)
        Me.gbZimmer.Controls.Add(Me.lbZimNr)
        Me.gbZimmer.Controls.Add(Me.Label5)
        Me.gbZimmer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbZimmer.Location = New System.Drawing.Point(21, 57)
        Me.gbZimmer.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbZimmer.Name = "gbZimmer"
        Me.gbZimmer.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbZimmer.Size = New System.Drawing.Size(868, 325)
        Me.gbZimmer.TabIndex = 1
        Me.gbZimmer.TabStop = False
        Me.gbZimmer.Text = "Zimmer Informationen"
        '
        'lbMailList
        '
        Me.lbMailList.AutoSize = True
        Me.lbMailList.BackColor = System.Drawing.Color.GreenYellow
        Me.lbMailList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbMailList.Location = New System.Drawing.Point(310, 194)
        Me.lbMailList.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbMailList.Name = "lbMailList"
        Me.lbMailList.Size = New System.Drawing.Size(68, 20)
        Me.lbMailList.TabIndex = 35
        Me.lbMailList.Text = "Label29"
        '
        'cbPausch
        '
        Me.cbPausch.AutoSize = True
        Me.cbPausch.Location = New System.Drawing.Point(327, 275)
        Me.cbPausch.Name = "cbPausch"
        Me.cbPausch.Size = New System.Drawing.Size(112, 24)
        Me.cbPausch.TabIndex = 34
        Me.cbPausch.Text = "Pauschal"
        Me.cbPausch.UseVisualStyleBackColor = True
        '
        'lbPauschPreis
        '
        Me.lbPauschPreis.AutoSize = True
        Me.lbPauschPreis.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbPauschPreis.Location = New System.Drawing.Point(494, 277)
        Me.lbPauschPreis.Name = "lbPauschPreis"
        Me.lbPauschPreis.Size = New System.Drawing.Size(40, 20)
        Me.lbPauschPreis.TabIndex = 33
        Me.lbPauschPreis.Text = "0.00"
        '
        'tbAbZeit
        '
        Me.tbAbZeit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbAbZeit.Location = New System.Drawing.Point(548, 25)
        Me.tbAbZeit.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbAbZeit.Name = "tbAbZeit"
        Me.tbAbZeit.Size = New System.Drawing.Size(60, 26)
        Me.tbAbZeit.TabIndex = 32
        Me.tbAbZeit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbAnZeit
        '
        Me.tbAnZeit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbAnZeit.Location = New System.Drawing.Point(249, 26)
        Me.tbAnZeit.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbAnZeit.Name = "tbAnZeit"
        Me.tbAnZeit.Size = New System.Drawing.Size(60, 26)
        Me.tbAnZeit.TabIndex = 31
        Me.tbAnZeit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbSumme
        '
        Me.tbSumme.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbSumme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbSumme.Location = New System.Drawing.Point(744, 272)
        Me.tbSumme.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbSumme.Name = "tbSumme"
        Me.tbSumme.Size = New System.Drawing.Size(100, 26)
        Me.tbSumme.TabIndex = 26
        Me.tbSumme.Text = "0,00"
        Me.tbSumme.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(582, 277)
        Me.Label26.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(112, 20)
        Me.Label26.TabIndex = 25
        Me.Label26.Text = "Gesamt Preis"
        '
        'buBerechnung
        '
        Me.buBerechnung.Location = New System.Drawing.Point(458, 268)
        Me.buBerechnung.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.buBerechnung.Name = "buBerechnung"
        Me.buBerechnung.Size = New System.Drawing.Size(27, 37)
        Me.buBerechnung.TabIndex = 30
        Me.buBerechnung.Text = "?"
        Me.buBerechnung.UseVisualStyleBackColor = True
        '
        'cbBezalt
        '
        Me.cbBezalt.AutoSize = True
        Me.cbBezalt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbBezalt.Location = New System.Drawing.Point(249, 275)
        Me.cbBezalt.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cbBezalt.Name = "cbBezalt"
        Me.cbBezalt.Size = New System.Drawing.Size(83, 24)
        Me.cbBezalt.TabIndex = 29
        Me.cbBezalt.Text = "Bezalt"
        Me.cbBezalt.UseVisualStyleBackColor = True
        '
        'tbStorno
        '
        Me.tbStorno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbStorno.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbStorno.Location = New System.Drawing.Point(165, 274)
        Me.tbStorno.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbStorno.Name = "tbStorno"
        Me.tbStorno.Size = New System.Drawing.Size(74, 26)
        Me.tbStorno.TabIndex = 27
        Me.tbStorno.Text = "0"
        Me.tbStorno.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbFPreis
        '
        Me.tbFPreis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbFPreis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbFPreis.Location = New System.Drawing.Point(492, 192)
        Me.tbFPreis.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbFPreis.Name = "tbFPreis"
        Me.tbFPreis.Size = New System.Drawing.Size(80, 26)
        Me.tbFPreis.TabIndex = 23
        Me.tbFPreis.Text = "0,00"
        Me.tbFPreis.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.tbFPreis.Visible = False
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(16, 277)
        Me.Label22.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(96, 20)
        Me.Label22.TabIndex = 24
        Me.Label22.Text = "Storno in %"
        '
        'tbAnzahlung
        '
        Me.tbAnzahlung.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbAnzahlung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbAnzahlung.Location = New System.Drawing.Point(744, 234)
        Me.tbAnzahlung.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbAnzahlung.Name = "tbAnzahlung"
        Me.tbAnzahlung.Size = New System.Drawing.Size(100, 26)
        Me.tbAnzahlung.TabIndex = 5
        Me.tbAnzahlung.Text = "0,00"
        Me.tbAnzahlung.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'coArt
        '
        Me.coArt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.coArt.FormattingEnabled = True
        Me.coArt.Items.AddRange(New Object() {"Ü", "Ü/F"})
        Me.coArt.Location = New System.Drawing.Point(490, 232)
        Me.coArt.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.coArt.Name = "coArt"
        Me.coArt.Size = New System.Drawing.Size(79, 28)
        Me.coArt.TabIndex = 3
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(582, 228)
        Me.Label15.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(154, 31)
        Me.Label15.TabIndex = 20
        Me.Label15.Text = "Anzahlung:"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbUArt
        '
        Me.tbUArt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbUArt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbUArt.Location = New System.Drawing.Point(165, 231)
        Me.tbUArt.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbUArt.Name = "tbUArt"
        Me.tbUArt.Size = New System.Drawing.Size(274, 26)
        Me.tbUArt.TabIndex = 22
        '
        'Label17
        '
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(16, 228)
        Me.Label17.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(154, 31)
        Me.Label17.TabIndex = 18
        Me.Label17.Text = "Übernachtungsart:"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rbVariabel
        '
        Me.rbVariabel.AutoSize = True
        Me.rbVariabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbVariabel.Location = New System.Drawing.Point(744, 155)
        Me.rbVariabel.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.rbVariabel.Name = "rbVariabel"
        Me.rbVariabel.Size = New System.Drawing.Size(95, 24)
        Me.rbVariabel.TabIndex = 19
        Me.rbVariabel.Text = "Variabel"
        Me.rbVariabel.UseVisualStyleBackColor = True
        '
        'tbPreis
        '
        Me.tbPreis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPreis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbPreis.Location = New System.Drawing.Point(744, 192)
        Me.tbPreis.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbPreis.Name = "tbPreis"
        Me.tbPreis.Size = New System.Drawing.Size(100, 26)
        Me.tbPreis.TabIndex = 4
        Me.tbPreis.Text = "0,00"
        Me.tbPreis.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'rbFest
        '
        Me.rbFest.AutoSize = True
        Me.rbFest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbFest.Location = New System.Drawing.Point(630, 155)
        Me.rbFest.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.rbFest.Name = "rbFest"
        Me.rbFest.Size = New System.Drawing.Size(67, 24)
        Me.rbFest.TabIndex = 18
        Me.rbFest.Text = "Fest"
        Me.rbFest.UseVisualStyleBackColor = True
        '
        'tbAnzPer
        '
        Me.tbAnzPer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbAnzPer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbAnzPer.Location = New System.Drawing.Point(165, 192)
        Me.tbAnzPer.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbAnzPer.Name = "tbAnzPer"
        Me.tbAnzPer.Size = New System.Drawing.Size(74, 26)
        Me.tbAnzPer.TabIndex = 1
        Me.tbAnzPer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'rbNormal
        '
        Me.rbNormal.AutoSize = True
        Me.rbNormal.Checked = True
        Me.rbNormal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbNormal.Location = New System.Drawing.Point(518, 155)
        Me.rbNormal.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.rbNormal.Name = "rbNormal"
        Me.rbNormal.Size = New System.Drawing.Size(88, 24)
        Me.rbNormal.TabIndex = 17
        Me.rbNormal.TabStop = True
        Me.rbNormal.Text = "Normal"
        Me.rbNormal.UseVisualStyleBackColor = True
        '
        'Label19
        '
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(582, 189)
        Me.Label19.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(184, 31)
        Me.Label19.TabIndex = 16
        Me.Label19.Text = "Vereinbarter Preis:"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbBetten
        '
        Me.lbBetten.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbBetten.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbBetten.Location = New System.Drawing.Point(770, 69)
        Me.lbBetten.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbBetten.Name = "lbBetten"
        Me.lbBetten.Size = New System.Drawing.Size(74, 30)
        Me.lbBetten.TabIndex = 8
        Me.lbBetten.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbObjekt
        '
        Me.lbObjekt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbObjekt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbObjekt.Location = New System.Drawing.Point(465, 120)
        Me.lbObjekt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbObjekt.Name = "lbObjekt"
        Me.lbObjekt.Size = New System.Drawing.Size(378, 30)
        Me.lbObjekt.TabIndex = 13
        Me.lbObjekt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(304, 120)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(216, 31)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "Objekt:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbAusstattung
        '
        Me.lbAusstattung.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbAusstattung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbAusstattung.Location = New System.Drawing.Point(128, 120)
        Me.lbAusstattung.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbAusstattung.Name = "lbAusstattung"
        Me.lbAusstattung.Size = New System.Drawing.Size(149, 30)
        Me.lbAusstattung.TabIndex = 11
        Me.lbAusstattung.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(16, 120)
        Me.Label13.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(112, 31)
        Me.Label13.TabIndex = 10
        Me.Label13.Text = "Ausstattung:"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label21
        '
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label21.Location = New System.Drawing.Point(16, 189)
        Me.Label21.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(154, 31)
        Me.Label21.TabIndex = 14
        Me.Label21.Text = "Anzahl Personen:"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(735, 69)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(26, 31)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "/"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbArt
        '
        Me.lbArt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbArt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbArt.Location = New System.Drawing.Point(464, 69)
        Me.lbArt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbArt.Name = "lbArt"
        Me.lbArt.Size = New System.Drawing.Size(269, 30)
        Me.lbArt.TabIndex = 7
        Me.lbArt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(304, 69)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(216, 31)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Art / Betten:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbZimNr
        '
        Me.lbZimNr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbZimNr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbZimNr.Location = New System.Drawing.Point(128, 69)
        Me.lbZimNr.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbZimNr.Name = "lbZimNr"
        Me.lbZimNr.Size = New System.Drawing.Size(149, 30)
        Me.lbZimNr.TabIndex = 5
        Me.lbZimNr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(16, 69)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(112, 31)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Zimmer Nr.:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbGast
        '
        Me.gbGast.Controls.Add(Me.cbZusatz)
        Me.gbGast.Controls.Add(Me.Label28)
        Me.gbGast.Controls.Add(Me.tbInternetNr)
        Me.gbGast.Controls.Add(Me.coLand)
        Me.gbGast.Controls.Add(Me.Label14)
        Me.gbGast.Controls.Add(Me.Label10)
        Me.gbGast.Controls.Add(Me.lbGast)
        Me.gbGast.Controls.Add(Me.btGast)
        Me.gbGast.Controls.Add(Me.tbStrasse)
        Me.gbGast.Controls.Add(Me.Label6)
        Me.gbGast.Controls.Add(Me.coAnrede)
        Me.gbGast.Controls.Add(Me.dpGeb)
        Me.gbGast.Controls.Add(Me.lbGastID)
        Me.gbGast.Controls.Add(Me.tbTel)
        Me.gbGast.Controls.Add(Me.tbFax)
        Me.gbGast.Controls.Add(Me.tbFunk)
        Me.gbGast.Controls.Add(Me.tbEMail)
        Me.gbGast.Controls.Add(Me.tbOrt)
        Me.gbGast.Controls.Add(Me.tbInfo)
        Me.gbGast.Controls.Add(Me.tbPLZ)
        Me.gbGast.Controls.Add(Me.tbVorname)
        Me.gbGast.Controls.Add(Me.tbName2)
        Me.gbGast.Controls.Add(Me.tbName1)
        Me.gbGast.Controls.Add(Me.Label27)
        Me.gbGast.Controls.Add(Me.coWerbung)
        Me.gbGast.Controls.Add(Me.Label40)
        Me.gbGast.Controls.Add(Me.Label31)
        Me.gbGast.Controls.Add(Me.Label33)
        Me.gbGast.Controls.Add(Me.Label35)
        Me.gbGast.Controls.Add(Me.Label37)
        Me.gbGast.Controls.Add(Me.Label39)
        Me.gbGast.Controls.Add(Me.Label20)
        Me.gbGast.Controls.Add(Me.Label23)
        Me.gbGast.Controls.Add(Me.Label25)
        Me.gbGast.Controls.Add(Me.Label16)
        Me.gbGast.Controls.Add(Me.Label12)
        Me.gbGast.Controls.Add(Me.Label8)
        Me.gbGast.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbGast.Location = New System.Drawing.Point(21, 391)
        Me.gbGast.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbGast.Name = "gbGast"
        Me.gbGast.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbGast.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.gbGast.Size = New System.Drawing.Size(868, 406)
        Me.gbGast.TabIndex = 1
        Me.gbGast.TabStop = False
        Me.gbGast.Text = "Daten des Gastes"
        '
        'cbZusatz
        '
        Me.cbZusatz.AutoSize = True
        Me.cbZusatz.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbZusatz.Location = New System.Drawing.Point(24, 331)
        Me.cbZusatz.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cbZusatz.Name = "cbZusatz"
        Me.cbZusatz.Size = New System.Drawing.Size(134, 24)
        Me.cbZusatz.TabIndex = 76
        Me.cbZusatz.Text = "Name Zusatz"
        Me.cbZusatz.UseVisualStyleBackColor = True
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(482, 252)
        Me.Label28.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(92, 20)
        Me.Label28.TabIndex = 75
        Me.Label28.Text = "Internet Nr."
        '
        'tbInternetNr
        '
        Me.tbInternetNr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbInternetNr.Location = New System.Drawing.Point(630, 255)
        Me.tbInternetNr.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbInternetNr.Name = "tbInternetNr"
        Me.tbInternetNr.Size = New System.Drawing.Size(212, 26)
        Me.tbInternetNr.TabIndex = 74
        '
        'coLand
        '
        Me.coLand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.coLand.FormattingEnabled = True
        Me.coLand.Location = New System.Drawing.Point(165, 248)
        Me.coLand.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.coLand.Name = "coLand"
        Me.coLand.Size = New System.Drawing.Size(290, 28)
        Me.coLand.TabIndex = 73
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(838, 0)
        Me.Label14.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(15, 20)
        Me.Label14.TabIndex = 72
        Me.Label14.Text = ")"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(580, 0)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(109, 20)
        Me.Label10.TabIndex = 71
        Me.Label10.Text = "Kunden-Nr. ( "
        '
        'lbGast
        '
        Me.lbGast.AutoSize = True
        Me.lbGast.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbGast.Location = New System.Drawing.Point(204, 0)
        Me.lbGast.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbGast.Name = "lbGast"
        Me.lbGast.Size = New System.Drawing.Size(104, 20)
        Me.lbGast.TabIndex = 70
        Me.lbGast.Text = "(neuer Gast)"
        Me.lbGast.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btGast
        '
        Me.btGast.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btGast.Image = CType(resources.GetObject("btGast.Image"), System.Drawing.Image)
        Me.btGast.Location = New System.Drawing.Point(434, 69)
        Me.btGast.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btGast.Name = "btGast"
        Me.btGast.Size = New System.Drawing.Size(24, 31)
        Me.btGast.TabIndex = 69
        Me.btGast.UseVisualStyleBackColor = True
        '
        'tbStrasse
        '
        Me.tbStrasse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbStrasse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbStrasse.Location = New System.Drawing.Point(165, 177)
        Me.tbStrasse.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbStrasse.MaxLength = 50
        Me.tbStrasse.Name = "tbStrasse"
        Me.tbStrasse.Size = New System.Drawing.Size(292, 26)
        Me.tbStrasse.TabIndex = 68
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(9, 174)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(120, 31)
        Me.Label6.TabIndex = 67
        Me.Label6.Text = "Strasse"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'coAnrede
        '
        Me.coAnrede.BackColor = System.Drawing.SystemColors.Window
        Me.coAnrede.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.coAnrede.FormattingEnabled = True
        Me.coAnrede.Items.AddRange(New Object() {"Firma", "Frau", "Herr"})
        Me.coAnrede.Location = New System.Drawing.Point(165, 34)
        Me.coAnrede.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.coAnrede.MaxLength = 8
        Me.coAnrede.Name = "coAnrede"
        Me.coAnrede.Size = New System.Drawing.Size(290, 28)
        Me.coAnrede.TabIndex = 66
        '
        'dpGeb
        '
        Me.dpGeb.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dpGeb.Location = New System.Drawing.Point(630, 29)
        Me.dpGeb.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dpGeb.Name = "dpGeb"
        Me.dpGeb.Size = New System.Drawing.Size(212, 26)
        Me.dpGeb.TabIndex = 65
        '
        'lbGastID
        '
        Me.lbGastID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbGastID.Location = New System.Drawing.Point(681, 0)
        Me.lbGastID.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbGastID.Name = "lbGastID"
        Me.lbGastID.Size = New System.Drawing.Size(164, 26)
        Me.lbGastID.TabIndex = 50
        Me.lbGastID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbTel
        '
        Me.tbTel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbTel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbTel.Location = New System.Drawing.Point(630, 69)
        Me.tbTel.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTel.MaxLength = 50
        Me.tbTel.Name = "tbTel"
        Me.tbTel.Size = New System.Drawing.Size(214, 26)
        Me.tbTel.TabIndex = 63
        '
        'tbFax
        '
        Me.tbFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbFax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbFax.Location = New System.Drawing.Point(630, 105)
        Me.tbFax.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbFax.MaxLength = 50
        Me.tbFax.Name = "tbFax"
        Me.tbFax.Size = New System.Drawing.Size(214, 26)
        Me.tbFax.TabIndex = 62
        '
        'tbFunk
        '
        Me.tbFunk.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbFunk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbFunk.Location = New System.Drawing.Point(630, 140)
        Me.tbFunk.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbFunk.MaxLength = 50
        Me.tbFunk.Name = "tbFunk"
        Me.tbFunk.Size = New System.Drawing.Size(214, 26)
        Me.tbFunk.TabIndex = 61
        '
        'tbEMail
        '
        Me.tbEMail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbEMail.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbEMail.Location = New System.Drawing.Point(630, 178)
        Me.tbEMail.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbEMail.MaxLength = 100
        Me.tbEMail.Name = "tbEMail"
        Me.tbEMail.Size = New System.Drawing.Size(214, 26)
        Me.tbEMail.TabIndex = 60
        '
        'tbOrt
        '
        Me.tbOrt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbOrt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbOrt.Location = New System.Drawing.Point(249, 214)
        Me.tbOrt.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbOrt.MaxLength = 50
        Me.tbOrt.Name = "tbOrt"
        Me.tbOrt.Size = New System.Drawing.Size(208, 26)
        Me.tbOrt.TabIndex = 58
        '
        'tbInfo
        '
        Me.tbInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbInfo.Location = New System.Drawing.Point(168, 295)
        Me.tbInfo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbInfo.Multiline = True
        Me.tbInfo.Name = "tbInfo"
        Me.tbInfo.Size = New System.Drawing.Size(680, 97)
        Me.tbInfo.TabIndex = 57
        '
        'tbPLZ
        '
        Me.tbPLZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPLZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbPLZ.Location = New System.Drawing.Point(165, 214)
        Me.tbPLZ.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbPLZ.MaxLength = 10
        Me.tbPLZ.Name = "tbPLZ"
        Me.tbPLZ.Size = New System.Drawing.Size(74, 26)
        Me.tbPLZ.TabIndex = 55
        '
        'tbVorname
        '
        Me.tbVorname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbVorname.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbVorname.Location = New System.Drawing.Point(165, 140)
        Me.tbVorname.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbVorname.MaxLength = 50
        Me.tbVorname.Name = "tbVorname"
        Me.tbVorname.Size = New System.Drawing.Size(292, 26)
        Me.tbVorname.TabIndex = 54
        '
        'tbName2
        '
        Me.tbName2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbName2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbName2.Location = New System.Drawing.Point(165, 105)
        Me.tbName2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbName2.MaxLength = 50
        Me.tbName2.Name = "tbName2"
        Me.tbName2.Size = New System.Drawing.Size(292, 26)
        Me.tbName2.TabIndex = 53
        '
        'tbName1
        '
        Me.tbName1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbName1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbName1.Location = New System.Drawing.Point(165, 69)
        Me.tbName1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbName1.MaxLength = 50
        Me.tbName1.Name = "tbName1"
        Me.tbName1.Size = New System.Drawing.Size(269, 26)
        Me.tbName1.TabIndex = 52
        '
        'Label27
        '
        Me.Label27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.Location = New System.Drawing.Point(20, 295)
        Me.Label27.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(255, 31)
        Me.Label27.TabIndex = 49
        Me.Label27.Text = "Info"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'coWerbung
        '
        Me.coWerbung.BackColor = System.Drawing.SystemColors.Window
        Me.coWerbung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.coWerbung.FormattingEnabled = True
        Me.coWerbung.Location = New System.Drawing.Point(628, 214)
        Me.coWerbung.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.coWerbung.Name = "coWerbung"
        Me.coWerbung.Size = New System.Drawing.Size(212, 28)
        Me.coWerbung.TabIndex = 48
        '
        'Label40
        '
        Me.Label40.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label40.Location = New System.Drawing.Point(482, 212)
        Me.Label40.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(255, 31)
        Me.Label40.TabIndex = 47
        Me.Label40.Text = "Werbung"
        Me.Label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label31
        '
        Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(482, 175)
        Me.Label31.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(255, 31)
        Me.Label31.TabIndex = 43
        Me.Label31.Text = "E-Mail"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label33
        '
        Me.Label33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.Location = New System.Drawing.Point(482, 138)
        Me.Label33.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(255, 31)
        Me.Label33.TabIndex = 41
        Me.Label33.Text = "Funk"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label35
        '
        Me.Label35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.Location = New System.Drawing.Point(478, 103)
        Me.Label35.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(255, 31)
        Me.Label35.TabIndex = 39
        Me.Label35.Text = "Telefax"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label37
        '
        Me.Label37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.Location = New System.Drawing.Point(478, 66)
        Me.Label37.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(140, 31)
        Me.Label37.TabIndex = 37
        Me.Label37.Text = "Telefon"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label39
        '
        Me.Label39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.Location = New System.Drawing.Point(478, 32)
        Me.Label39.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(140, 31)
        Me.Label39.TabIndex = 35
        Me.Label39.Text = "Geburtsdatum"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label20
        '
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(15, 246)
        Me.Label20.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(255, 31)
        Me.Label20.TabIndex = 30
        Me.Label20.Text = "Land"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label23
        '
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(15, 212)
        Me.Label23.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(255, 31)
        Me.Label23.TabIndex = 28
        Me.Label23.Text = "PLZ / Ort"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label25
        '
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(9, 137)
        Me.Label25.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(255, 31)
        Me.Label25.TabIndex = 26
        Me.Label25.Text = "Vorname"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(9, 103)
        Me.Label16.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(255, 31)
        Me.Label16.TabIndex = 24
        Me.Label16.Text = "Name"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(9, 69)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(255, 31)
        Me.Label12.TabIndex = 22
        Me.Label12.Text = "Name - Firma"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(9, 32)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(255, 31)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "Anrede"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvGast
        '
        Me.lvGast.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvGast.HideSelection = False
        Me.lvGast.Location = New System.Drawing.Point(898, 91)
        Me.lvGast.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lvGast.Name = "lvGast"
        Me.lvGast.ShowItemToolTips = True
        Me.lvGast.Size = New System.Drawing.Size(1010, 862)
        Me.lvGast.TabIndex = 2
        Me.lvGast.UseCompatibleStateImageBehavior = False
        '
        'tsMain
        '
        Me.tsMain.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbClose, Me.ToolStripSeparator1, Me.tsbSave, Me.ToolStripSeparator2, Me.ToolStripDropDownButton1, Me.ToolStripSeparator4, Me.ToolStripLabel1, Me.tscoZim, Me.tscoFreiZim, Me.tsbAddZimmer, Me.lbSumme, Me.tsbVorAnreise, Me.tsbNachAbreise, Me.ToolStripSeparator6, Me.tslCode, Me.tsbKey})
        Me.tsMain.Location = New System.Drawing.Point(0, 0)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.tsMain.Size = New System.Drawing.Size(1924, 40)
        Me.tsMain.TabIndex = 3
        Me.tsMain.Text = "ToolStrip1"
        '
        'tsbClose
        '
        Me.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbClose.Image = CType(resources.GetObject("tsbClose.Image"), System.Drawing.Image)
        Me.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbClose.Name = "tsbClose"
        Me.tsbClose.Size = New System.Drawing.Size(34, 35)
        Me.tsbClose.Text = "Reservierungsmodule schliessen"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 40)
        '
        'tsbSave
        '
        Me.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSave.Image = CType(resources.GetObject("tsbSave.Image"), System.Drawing.Image)
        Me.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSave.Name = "tsbSave"
        Me.tsbSave.Size = New System.Drawing.Size(34, 35)
        Me.tsbSave.Text = "Speichern"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 40)
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbAufZimmer, Me.tsbBestätigung, Me.tsbMail, Me.ToolStripSeparator7, Me.tsbRechnung, Me.tsbAnzahlungRechnung, Me.tsbBezahlt, Me.ToolStripSeparator8, Me.tsbBewertung, Me.ToolStripSeparator3, Me.tsmStorno, Me.ToolStripSeparator5, Me.tsbErinnerung, Me.tsSep1, Me.tsbDelReservierung, Me.tsbDelZimmer, Me.tsmKopie, Me.tsSep2, Me.tsbCheckin, Me.PreisToolStripMenuItem})
        Me.ToolStripDropDownButton1.Image = CType(resources.GetObject("ToolStripDropDownButton1.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(38, 35)
        '
        'tsbAufZimmer
        '
        Me.tsbAufZimmer.Image = CType(resources.GetObject("tsbAufZimmer.Image"), System.Drawing.Image)
        Me.tsbAufZimmer.Name = "tsbAufZimmer"
        Me.tsbAufZimmer.Size = New System.Drawing.Size(357, 40)
        Me.tsbAufZimmer.Text = "Schreibe auf Zimmer"
        '
        'tsbBestätigung
        '
        Me.tsbBestätigung.Name = "tsbBestätigung"
        Me.tsbBestätigung.Size = New System.Drawing.Size(357, 40)
        Me.tsbBestätigung.Text = "Buchungs Bestätigung"
        '
        'tsbMail
        '
        Me.tsbMail.Name = "tsbMail"
        Me.tsbMail.Size = New System.Drawing.Size(357, 40)
        Me.tsbMail.Text = "eMail Senden"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(354, 6)
        '
        'tsbRechnung
        '
        Me.tsbRechnung.Image = CType(resources.GetObject("tsbRechnung.Image"), System.Drawing.Image)
        Me.tsbRechnung.Name = "tsbRechnung"
        Me.tsbRechnung.Size = New System.Drawing.Size(357, 40)
        Me.tsbRechnung.Text = "Rechnung generieren"
        '
        'tsbAnzahlungRechnung
        '
        Me.tsbAnzahlungRechnung.Name = "tsbAnzahlungRechnung"
        Me.tsbAnzahlungRechnung.Size = New System.Drawing.Size(357, 40)
        Me.tsbAnzahlungRechnung.Text = "Anzahlung Rechnung"
        '
        'tsbBezahlt
        '
        Me.tsbBezahlt.Name = "tsbBezahlt"
        Me.tsbBezahlt.Size = New System.Drawing.Size(357, 40)
        Me.tsbBezahlt.Text = "Zahlungsbestätigung"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(354, 6)
        '
        'tsbBewertung
        '
        Me.tsbBewertung.Name = "tsbBewertung"
        Me.tsbBewertung.Size = New System.Drawing.Size(357, 40)
        Me.tsbBewertung.Text = "Bewertung Mail"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(354, 6)
        '
        'tsmStorno
        '
        Me.tsmStorno.Name = "tsmStorno"
        Me.tsmStorno.Size = New System.Drawing.Size(357, 40)
        Me.tsmStorno.Text = "Rechnung stornieren"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(354, 6)
        '
        'tsbErinnerung
        '
        Me.tsbErinnerung.Name = "tsbErinnerung"
        Me.tsbErinnerung.Size = New System.Drawing.Size(357, 40)
        Me.tsbErinnerung.Text = "Erinnerung "
        '
        'tsSep1
        '
        Me.tsSep1.Name = "tsSep1"
        Me.tsSep1.Size = New System.Drawing.Size(354, 6)
        '
        'tsbDelReservierung
        '
        Me.tsbDelReservierung.Image = CType(resources.GetObject("tsbDelReservierung.Image"), System.Drawing.Image)
        Me.tsbDelReservierung.Name = "tsbDelReservierung"
        Me.tsbDelReservierung.Size = New System.Drawing.Size(357, 40)
        Me.tsbDelReservierung.Text = "Reservierung löschen"
        Me.tsbDelReservierung.ToolTipText = "Gesammte Reservierung (alle Zimmer) löschen."
        '
        'tsbDelZimmer
        '
        Me.tsbDelZimmer.Name = "tsbDelZimmer"
        Me.tsbDelZimmer.Size = New System.Drawing.Size(357, 40)
        Me.tsbDelZimmer.Text = "Zimmer löschen"
        Me.tsbDelZimmer.ToolTipText = "Ausgewählte Zimmer löschen."
        '
        'tsmKopie
        '
        Me.tsmKopie.Name = "tsmKopie"
        Me.tsmKopie.Size = New System.Drawing.Size(357, 40)
        Me.tsmKopie.Text = "Rechnungskopie"
        Me.tsmKopie.Visible = False
        '
        'tsSep2
        '
        Me.tsSep2.Name = "tsSep2"
        Me.tsSep2.Size = New System.Drawing.Size(354, 6)
        '
        'tsbCheckin
        '
        Me.tsbCheckin.Name = "tsbCheckin"
        Me.tsbCheckin.Size = New System.Drawing.Size(357, 40)
        Me.tsbCheckin.Text = "Checkin"
        '
        'PreisToolStripMenuItem
        '
        Me.PreisToolStripMenuItem.Name = "PreisToolStripMenuItem"
        Me.PreisToolStripMenuItem.Size = New System.Drawing.Size(357, 40)
        Me.PreisToolStripMenuItem.Text = "Preis"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 40)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(228, 35)
        Me.ToolStripLabel1.Text = "Zimmerauswahl => "
        '
        'tscoZim
        '
        Me.tscoZim.Name = "tscoZim"
        Me.tscoZim.Size = New System.Drawing.Size(180, 40)
        '
        'tscoFreiZim
        '
        Me.tscoFreiZim.Name = "tscoFreiZim"
        Me.tscoFreiZim.Size = New System.Drawing.Size(180, 40)
        Me.tscoFreiZim.Visible = False
        '
        'tsbAddZimmer
        '
        Me.tsbAddZimmer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbAddZimmer.Image = CType(resources.GetObject("tsbAddZimmer.Image"), System.Drawing.Image)
        Me.tsbAddZimmer.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAddZimmer.Name = "tsbAddZimmer"
        Me.tsbAddZimmer.Size = New System.Drawing.Size(34, 35)
        Me.tsbAddZimmer.Text = "Zimmer hinzufügen"
        Me.tsbAddZimmer.Visible = False
        '
        'lbSumme
        '
        Me.lbSumme.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.lbSumme.BackColor = System.Drawing.Color.Green
        Me.lbSumme.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.lbSumme.ForeColor = System.Drawing.Color.Red
        Me.lbSumme.ImageTransparentColor = System.Drawing.Color.Green
        Me.lbSumme.Name = "lbSumme"
        Me.lbSumme.Size = New System.Drawing.Size(96, 35)
        Me.lbSumme.Text = "Summe"
        Me.lbSumme.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tsbVorAnreise
        '
        Me.tsbVorAnreise.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbVorAnreise.Image = CType(resources.GetObject("tsbVorAnreise.Image"), System.Drawing.Image)
        Me.tsbVorAnreise.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbVorAnreise.Name = "tsbVorAnreise"
        Me.tsbVorAnreise.Size = New System.Drawing.Size(34, 35)
        Me.tsbVorAnreise.Text = "ToolStripButton1"
        Me.tsbVorAnreise.ToolTipText = "vor der Buchung"
        '
        'tsbNachAbreise
        '
        Me.tsbNachAbreise.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbNachAbreise.Image = CType(resources.GetObject("tsbNachAbreise.Image"), System.Drawing.Image)
        Me.tsbNachAbreise.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNachAbreise.Name = "tsbNachAbreise"
        Me.tsbNachAbreise.Size = New System.Drawing.Size(34, 35)
        Me.tsbNachAbreise.Text = "ToolStripButton2"
        Me.tsbNachAbreise.ToolTipText = "Nach der Buchung"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 40)
        '
        'tslCode
        '
        Me.tslCode.Name = "tslCode"
        Me.tslCode.Size = New System.Drawing.Size(79, 35)
        Me.tslCode.Text = "00000"
        '
        'tsbKey
        '
        Me.tsbKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbKey.Image = CType(resources.GetObject("tsbKey.Image"), System.Drawing.Image)
        Me.tsbKey.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbKey.Name = "tsbKey"
        Me.tsbKey.Size = New System.Drawing.Size(34, 35)
        '
        'ssMain
        '
        Me.ssMain.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel4, Me.ToolStripStatusLabel1, Me.tssInfo})
        Me.ssMain.Location = New System.Drawing.Point(0, 973)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Padding = New System.Windows.Forms.Padding(2, 0, 21, 0)
        Me.ssMain.Size = New System.Drawing.Size(1924, 39)
        Me.ssMain.TabIndex = 4
        Me.ssMain.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel4
        '
        Me.ToolStripStatusLabel4.Name = "ToolStripStatusLabel4"
        Me.ToolStripStatusLabel4.Size = New System.Drawing.Size(42, 32)
        Me.ToolStripStatusLabel4.Text = "    "
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(68, 32)
        Me.ToolStripStatusLabel1.Text = "Info: "
        '
        'tssInfo
        '
        Me.tssInfo.Name = "tssInfo"
        Me.tssInfo.Size = New System.Drawing.Size(28, 32)
        Me.tssInfo.Text = "  "
        '
        'grBText
        '
        Me.grBText.Controls.Add(Me.tbRechSend)
        Me.grBText.Controls.Add(Me.Label18)
        Me.grBText.Controls.Add(Me.coBText)
        Me.grBText.Controls.Add(Me.Label4)
        Me.grBText.Controls.Add(Me.lbMakro)
        Me.grBText.Controls.Add(Me.btBText)
        Me.grBText.Controls.Add(Me.coLang)
        Me.grBText.Controls.Add(Me.Label24)
        Me.grBText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grBText.Location = New System.Drawing.Point(21, 806)
        Me.grBText.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grBText.Name = "grBText"
        Me.grBText.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grBText.Size = New System.Drawing.Size(868, 143)
        Me.grBText.TabIndex = 5
        Me.grBText.TabStop = False
        Me.grBText.Text = "Buchungstext"
        '
        'tbRechSend
        '
        Me.tbRechSend.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.tbRechSend.Location = New System.Drawing.Point(165, 22)
        Me.tbRechSend.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbRechSend.Name = "tbRechSend"
        Me.tbRechSend.Size = New System.Drawing.Size(144, 26)
        Me.tbRechSend.TabIndex = 13
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(15, 26)
        Me.Label18.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(113, 20)
        Me.Label18.TabIndex = 12
        Me.Label18.Text = "Rech_Senden"
        '
        'coBText
        '
        Me.coBText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.coBText.FormattingEnabled = True
        Me.coBText.Location = New System.Drawing.Point(165, 91)
        Me.coBText.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.coBText.Name = "coBText"
        Me.coBText.Size = New System.Drawing.Size(144, 28)
        Me.coBText.TabIndex = 11
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(16, 95)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(112, 31)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Buchungstext"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbMakro
        '
        Me.lbMakro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbMakro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbMakro.Location = New System.Drawing.Point(483, 25)
        Me.lbMakro.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbMakro.Name = "lbMakro"
        Me.lbMakro.Size = New System.Drawing.Size(365, 100)
        Me.lbMakro.TabIndex = 9
        '
        'btBText
        '
        Me.btBText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btBText.Location = New System.Drawing.Point(369, 25)
        Me.btBText.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btBText.Name = "btBText"
        Me.btBText.Size = New System.Drawing.Size(105, 102)
        Me.btBText.TabIndex = 8
        Me.btBText.Text = "Makrotext bearbeiten"
        Me.btBText.UseVisualStyleBackColor = True
        '
        'coLang
        '
        Me.coLang.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.coLang.FormattingEnabled = True
        Me.coLang.Location = New System.Drawing.Point(165, 55)
        Me.coLang.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.coLang.Name = "coLang"
        Me.coLang.Size = New System.Drawing.Size(144, 28)
        Me.coLang.TabIndex = 7
        '
        'Label24
        '
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(16, 54)
        Me.Label24.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(80, 31)
        Me.Label24.TabIndex = 0
        Me.Label24.Text = "Sprache"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbNameZ
        '
        Me.gbNameZ.Controls.Add(Me.tbName2Z)
        Me.gbNameZ.Controls.Add(Me.tbName1Z)
        Me.gbNameZ.Location = New System.Drawing.Point(922, 566)
        Me.gbNameZ.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbNameZ.Name = "gbNameZ"
        Me.gbNameZ.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbNameZ.Size = New System.Drawing.Size(970, 149)
        Me.gbNameZ.TabIndex = 8
        Me.gbNameZ.TabStop = False
        Me.gbNameZ.Text = "Name Zusatz"
        '
        'tbName2Z
        '
        Me.tbName2Z.Location = New System.Drawing.Point(10, 94)
        Me.tbName2Z.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbName2Z.Name = "tbName2Z"
        Me.tbName2Z.Size = New System.Drawing.Size(936, 26)
        Me.tbName2Z.TabIndex = 1
        '
        'tbName1Z
        '
        Me.tbName1Z.Location = New System.Drawing.Point(10, 37)
        Me.tbName1Z.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbName1Z.Name = "tbName1Z"
        Me.tbName1Z.Size = New System.Drawing.Size(936, 26)
        Me.tbName1Z.TabIndex = 0
        '
        'MyKalender1
        '
        Me.MyKalender1.Abreise = "20150329"
        Me.MyKalender1.Anreise = "20150605"
        Me.MyKalender1.AnreiseAbreise = False
        Me.MyKalender1.Buchung = "20150525-20150530"
        Me.MyKalender1.Datum = Nothing
        Me.MyKalender1.Location = New System.Drawing.Point(898, 497)
        Me.MyKalender1.Margin = New System.Windows.Forms.Padding(6)
        Me.MyKalender1.Name = "MyKalender1"
        Me.MyKalender1.Size = New System.Drawing.Size(1012, 466)
        Me.MyKalender1.Sperr = "20150625-20150630"
        Me.MyKalender1.TabIndex = 7
        '
        'frmReservierungDest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Info
        Me.ClientSize = New System.Drawing.Size(1924, 1012)
        Me.Controls.Add(Me.gbNameZ)
        Me.Controls.Add(Me.MyKalender1)
        Me.Controls.Add(Me.lvGast)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.tsMain)
        Me.Controls.Add(Me.gbGast)
        Me.Controls.Add(Me.gbZimmer)
        Me.Controls.Add(Me.grBText)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReservierungDest"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Reservierung bearbeiten"
        Me.gbZimmer.ResumeLayout(False)
        Me.gbZimmer.PerformLayout()
        Me.gbGast.ResumeLayout(False)
        Me.gbGast.PerformLayout()
        Me.tsMain.ResumeLayout(False)
        Me.tsMain.PerformLayout()
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.grBText.ResumeLayout(False)
        Me.grBText.PerformLayout()
        Me.gbNameZ.ResumeLayout(False)
        Me.gbNameZ.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gbZimmer As System.Windows.Forms.GroupBox
    Friend WithEvents gbGast As System.Windows.Forms.GroupBox
    Friend WithEvents tsMain As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripDropDownButton1 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsbAufZimmer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbBestätigung As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbErinnerung As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsSep1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDelReservierung As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ssMain As System.Windows.Forms.StatusStrip
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbAnreise As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lbTage As System.Windows.Forms.Label
    Friend WithEvents lbAbreise As System.Windows.Forms.Label
    Friend WithEvents lbZimNr As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lbObjekt As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lbAusstattung As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lbBetten As System.Windows.Forms.Label
    Friend WithEvents lbArt As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents tbAnzPer As System.Windows.Forms.TextBox
    Friend WithEvents tbAnzahlung As System.Windows.Forms.TextBox
    Friend WithEvents tbPreis As System.Windows.Forms.TextBox
    Friend WithEvents coArt As System.Windows.Forms.ComboBox
    Friend WithEvents tbUArt As System.Windows.Forms.TextBox
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tscoZim As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tscoFreiZim As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tsbAddZimmer As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents lbGastID As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents coWerbung As System.Windows.Forms.ComboBox
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents tbTel As System.Windows.Forms.TextBox
    Friend WithEvents tbFax As System.Windows.Forms.TextBox
    Friend WithEvents tbFunk As System.Windows.Forms.TextBox
    Friend WithEvents tbEMail As System.Windows.Forms.TextBox
    Friend WithEvents tbOrt As System.Windows.Forms.TextBox
    Friend WithEvents tbInfo As System.Windows.Forms.TextBox
    Friend WithEvents tbPLZ As System.Windows.Forms.TextBox
    Friend WithEvents tbVorname As System.Windows.Forms.TextBox
    Friend WithEvents tbName2 As System.Windows.Forms.TextBox
    Friend WithEvents tbName1 As System.Windows.Forms.TextBox
    Friend WithEvents dpGeb As System.Windows.Forms.DateTimePicker
    Friend WithEvents coAnrede As System.Windows.Forms.ComboBox
    Friend WithEvents tbStrasse As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btGast As System.Windows.Forms.Button
    Friend WithEvents lvGast As System.Windows.Forms.ListView
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tssInfo As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lbGast As System.Windows.Forms.Label
    Friend WithEvents tsbDelZimmer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmKopie As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lbSumme As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsSep2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents grBText As System.Windows.Forms.GroupBox
    Friend WithEvents btBText As System.Windows.Forms.Button
    Friend WithEvents coLang As System.Windows.Forms.ComboBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents lbMakro As System.Windows.Forms.Label
    Friend WithEvents coBText As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmStorno As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tbFPreis As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tsbMail As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents coLand As System.Windows.Forms.ComboBox
    Friend WithEvents rbVariabel As System.Windows.Forms.RadioButton
    Friend WithEvents rbFest As System.Windows.Forms.RadioButton
    Friend WithEvents rbNormal As System.Windows.Forms.RadioButton
    Friend WithEvents tsbRechnung As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbBewertung As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents tbRechSend As System.Windows.Forms.TextBox
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbAnzahlungRechnung As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tbStorno As System.Windows.Forms.TextBox
    Friend WithEvents tbSumme As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents cbBezalt As System.Windows.Forms.CheckBox
    Friend WithEvents buBerechnung As System.Windows.Forms.Button
    Friend WithEvents tsbVorAnreise As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbNachAbreise As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbAbreise As System.Windows.Forms.Button
    Friend WithEvents tbAnreise As System.Windows.Forms.Button
    Friend WithEvents tsbKey As System.Windows.Forms.ToolStripButton
    Friend WithEvents tslCode As System.Windows.Forms.ToolStripLabel
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents tbInternetNr As System.Windows.Forms.TextBox
    Friend WithEvents MyKalender1 As Pension.myKalender
    Friend WithEvents ToolStripStatusLabel4 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tbAbZeit As System.Windows.Forms.TextBox
    Friend WithEvents tbAnZeit As System.Windows.Forms.TextBox
    Friend WithEvents PreisToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents lbPauschPreis As Label
    Friend WithEvents cbPausch As CheckBox
    Friend WithEvents tsbBezahlt As ToolStripMenuItem
    Friend WithEvents tsbCheckin As ToolStripMenuItem
    Friend WithEvents lbMailList As Label
    Friend WithEvents gbNameZ As GroupBox
    Friend WithEvents cbZusatz As CheckBox
    Friend WithEvents tbName2Z As TextBox
    Friend WithEvents tbName1Z As TextBox
End Class
