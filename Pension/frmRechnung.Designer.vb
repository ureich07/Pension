<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRechnung
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
        Me.components = New System.ComponentModel.Container()
        Me.gbGast = New System.Windows.Forms.GroupBox()
        Me.lbLand = New System.Windows.Forms.Label()
        Me.lbOrt = New System.Windows.Forms.Label()
        Me.lbPLZ = New System.Windows.Forms.Label()
        Me.lbStrasse = New System.Windows.Forms.Label()
        Me.lbVorname = New System.Windows.Forms.Label()
        Me.lbName2 = New System.Windows.Forms.Label()
        Me.lbName1 = New System.Windows.Forms.Label()
        Me.lbAnrede = New System.Windows.Forms.Label()
        Me.btGastDaten = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lbGastID = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.chliZimmer = New System.Windows.Forms.CheckedListBox()
        Me.gbOption = New System.Windows.Forms.GroupBox()
        Me.rbPausch = New System.Windows.Forms.RadioButton()
        Me.rbStorno = New System.Windows.Forms.RadioButton()
        Me.cbBar = New System.Windows.Forms.CheckBox()
        Me.lbBID = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.dtpRDatum = New System.Windows.Forms.DateTimePicker()
        Me.tbRNr = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.rbExtraORe = New System.Windows.Forms.RadioButton()
        Me.rbExtraGRe = New System.Windows.Forms.RadioButton()
        Me.rbExtraMRe = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gbRechnung = New System.Windows.Forms.GroupBox()
        Me.buMail = New System.Windows.Forms.Button()
        Me.lbBis = New System.Windows.Forms.Label()
        Me.lbVon = New System.Windows.Forms.Label()
        Me.lbZusatz = New System.Windows.Forms.Label()
        Me.coZimmer = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.coSteuer = New System.Windows.Forms.ComboBox()
        Me.btAbbruch = New System.Windows.Forms.Button()
        Me.btPosSpeichern = New System.Windows.Forms.Button()
        Me.btClose = New System.Windows.Forms.Button()
        Me.btDruck = New System.Windows.Forms.Button()
        Me.lbGesamt = New System.Windows.Forms.Label()
        Me.lbAnzahlung = New System.Windows.Forms.Label()
        Me.lbGSt7 = New System.Windows.Forms.Label()
        Me.lbGNetto = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.lbBrutto = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.lbNetto = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lbSteuer = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.tbBetrag = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.tbText = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tbMenge = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tbPos = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dgRechnung = New System.Windows.Forms.DataGridView()
        Me.tiGast = New System.Windows.Forms.Timer(Me.components)
        Me.gbGast.SuspendLayout()
        Me.gbOption.SuspendLayout()
        Me.gbRechnung.SuspendLayout()
        CType(Me.dgRechnung, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbGast
        '
        Me.gbGast.Controls.Add(Me.lbLand)
        Me.gbGast.Controls.Add(Me.lbOrt)
        Me.gbGast.Controls.Add(Me.lbPLZ)
        Me.gbGast.Controls.Add(Me.lbStrasse)
        Me.gbGast.Controls.Add(Me.lbVorname)
        Me.gbGast.Controls.Add(Me.lbName2)
        Me.gbGast.Controls.Add(Me.lbName1)
        Me.gbGast.Controls.Add(Me.lbAnrede)
        Me.gbGast.Controls.Add(Me.btGastDaten)
        Me.gbGast.Controls.Add(Me.Label6)
        Me.gbGast.Controls.Add(Me.lbGastID)
        Me.gbGast.Controls.Add(Me.Label20)
        Me.gbGast.Controls.Add(Me.Label23)
        Me.gbGast.Controls.Add(Me.Label25)
        Me.gbGast.Controls.Add(Me.Label16)
        Me.gbGast.Controls.Add(Me.Label12)
        Me.gbGast.Controls.Add(Me.Label8)
        Me.gbGast.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbGast.Location = New System.Drawing.Point(12, 12)
        Me.gbGast.Name = "gbGast"
        Me.gbGast.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.gbGast.Size = New System.Drawing.Size(319, 218)
        Me.gbGast.TabIndex = 2
        Me.gbGast.TabStop = False
        Me.gbGast.Text = "Daten des Gastes"
        '
        'lbLand
        '
        Me.lbLand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbLand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbLand.Location = New System.Drawing.Point(110, 183)
        Me.lbLand.Name = "lbLand"
        Me.lbLand.Size = New System.Drawing.Size(195, 20)
        Me.lbLand.TabIndex = 79
        Me.lbLand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbOrt
        '
        Me.lbOrt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbOrt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbOrt.Location = New System.Drawing.Point(166, 160)
        Me.lbOrt.Name = "lbOrt"
        Me.lbOrt.Size = New System.Drawing.Size(139, 20)
        Me.lbOrt.TabIndex = 78
        Me.lbOrt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbPLZ
        '
        Me.lbPLZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbPLZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbPLZ.Location = New System.Drawing.Point(110, 160)
        Me.lbPLZ.Name = "lbPLZ"
        Me.lbPLZ.Size = New System.Drawing.Size(50, 20)
        Me.lbPLZ.TabIndex = 77
        Me.lbPLZ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbStrasse
        '
        Me.lbStrasse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbStrasse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbStrasse.Location = New System.Drawing.Point(110, 138)
        Me.lbStrasse.Name = "lbStrasse"
        Me.lbStrasse.Size = New System.Drawing.Size(195, 20)
        Me.lbStrasse.TabIndex = 76
        Me.lbStrasse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbVorname
        '
        Me.lbVorname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbVorname.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbVorname.Location = New System.Drawing.Point(110, 114)
        Me.lbVorname.Name = "lbVorname"
        Me.lbVorname.Size = New System.Drawing.Size(195, 20)
        Me.lbVorname.TabIndex = 75
        Me.lbVorname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbName2
        '
        Me.lbName2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbName2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbName2.Location = New System.Drawing.Point(110, 90)
        Me.lbName2.Name = "lbName2"
        Me.lbName2.Size = New System.Drawing.Size(195, 20)
        Me.lbName2.TabIndex = 74
        Me.lbName2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbName1
        '
        Me.lbName1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbName1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbName1.Location = New System.Drawing.Point(110, 67)
        Me.lbName1.Name = "lbName1"
        Me.lbName1.Size = New System.Drawing.Size(195, 20)
        Me.lbName1.TabIndex = 73
        Me.lbName1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbAnrede
        '
        Me.lbAnrede.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbAnrede.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbAnrede.Location = New System.Drawing.Point(110, 43)
        Me.lbAnrede.Name = "lbAnrede"
        Me.lbAnrede.Size = New System.Drawing.Size(195, 20)
        Me.lbAnrede.TabIndex = 72
        Me.lbAnrede.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btGastDaten
        '
        Me.btGastDaten.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btGastDaten.Location = New System.Drawing.Point(110, 17)
        Me.btGastDaten.Name = "btGastDaten"
        Me.btGastDaten.Size = New System.Drawing.Size(195, 23)
        Me.btGastDaten.TabIndex = 69
        Me.btGastDaten.Text = "Gastdaten"
        Me.btGastDaten.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(6, 138)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(170, 20)
        Me.Label6.TabIndex = 67
        Me.Label6.Text = "Strasse"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbGastID
        '
        Me.lbGastID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbGastID.Location = New System.Drawing.Point(110, 206)
        Me.lbGastID.Name = "lbGastID"
        Me.lbGastID.Size = New System.Drawing.Size(50, 20)
        Me.lbGastID.TabIndex = 50
        Me.lbGastID.Text = "ID-Gast"
        Me.lbGastID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lbGastID.Visible = False
        '
        'Label20
        '
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(6, 183)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(170, 20)
        Me.Label20.TabIndex = 30
        Me.Label20.Text = "Land"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label23
        '
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(6, 160)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(170, 20)
        Me.Label23.TabIndex = 28
        Me.Label23.Text = "PLZ / Ort"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label25
        '
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(6, 114)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(170, 20)
        Me.Label25.TabIndex = 26
        Me.Label25.Text = "Vorname"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(6, 90)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(170, 20)
        Me.Label16.TabIndex = 24
        Me.Label16.Text = "Name"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(6, 67)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(170, 20)
        Me.Label12.TabIndex = 22
        Me.Label12.Text = "Name - Firma"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(6, 43)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(170, 20)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "Anrede"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chliZimmer
        '
        Me.chliZimmer.FormattingEnabled = True
        Me.chliZimmer.Location = New System.Drawing.Point(6, 39)
        Me.chliZimmer.Name = "chliZimmer"
        Me.chliZimmer.Size = New System.Drawing.Size(148, 109)
        Me.chliZimmer.TabIndex = 3
        '
        'gbOption
        '
        Me.gbOption.Controls.Add(Me.rbPausch)
        Me.gbOption.Controls.Add(Me.rbStorno)
        Me.gbOption.Controls.Add(Me.cbBar)
        Me.gbOption.Controls.Add(Me.lbBID)
        Me.gbOption.Controls.Add(Me.Label32)
        Me.gbOption.Controls.Add(Me.CheckBox1)
        Me.gbOption.Controls.Add(Me.dtpRDatum)
        Me.gbOption.Controls.Add(Me.tbRNr)
        Me.gbOption.Controls.Add(Me.Label3)
        Me.gbOption.Controls.Add(Me.Label2)
        Me.gbOption.Controls.Add(Me.rbExtraORe)
        Me.gbOption.Controls.Add(Me.rbExtraGRe)
        Me.gbOption.Controls.Add(Me.rbExtraMRe)
        Me.gbOption.Controls.Add(Me.Label1)
        Me.gbOption.Controls.Add(Me.chliZimmer)
        Me.gbOption.Location = New System.Drawing.Point(337, 12)
        Me.gbOption.Name = "gbOption"
        Me.gbOption.Size = New System.Drawing.Size(394, 218)
        Me.gbOption.TabIndex = 4
        Me.gbOption.TabStop = False
        '
        'rbPausch
        '
        Me.rbPausch.AutoSize = True
        Me.rbPausch.Location = New System.Drawing.Point(178, 183)
        Me.rbPausch.Name = "rbPausch"
        Me.rbPausch.Size = New System.Drawing.Size(122, 17)
        Me.rbPausch.TabIndex = 74
        Me.rbPausch.TabStop = True
        Me.rbPausch.Text = "Pauschal Rechnung"
        Me.rbPausch.UseVisualStyleBackColor = True
        '
        'rbStorno
        '
        Me.rbStorno.AutoSize = True
        Me.rbStorno.Location = New System.Drawing.Point(178, 160)
        Me.rbStorno.Name = "rbStorno"
        Me.rbStorno.Size = New System.Drawing.Size(56, 17)
        Me.rbStorno.TabIndex = 73
        Me.rbStorno.TabStop = True
        Me.rbStorno.Text = "Storno"
        Me.rbStorno.UseVisualStyleBackColor = True
        '
        'cbBar
        '
        Me.cbBar.AutoSize = True
        Me.cbBar.Location = New System.Drawing.Point(178, 46)
        Me.cbBar.Name = "cbBar"
        Me.cbBar.Size = New System.Drawing.Size(79, 17)
        Me.cbBar.TabIndex = 72
        Me.cbBar.Text = "Barzahlung"
        Me.cbBar.UseVisualStyleBackColor = True
        '
        'lbBID
        '
        Me.lbBID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbBID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbBID.Location = New System.Drawing.Point(85, 160)
        Me.lbBID.Name = "lbBID"
        Me.lbBID.Size = New System.Drawing.Size(77, 20)
        Me.lbBID.TabIndex = 71
        Me.lbBID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label32
        '
        Me.Label32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.Location = New System.Drawing.Point(6, 160)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(82, 20)
        Me.Label32.TabIndex = 59
        Me.Label32.Text = "Buchungs-ID:"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CheckBox1
        '
        Me.CheckBox1.Location = New System.Drawing.Point(178, 67)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(213, 24)
        Me.CheckBox1.TabIndex = 58
        Me.CheckBox1.Text = "Bewertungsmail generieren"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'dtpRDatum
        '
        Me.dtpRDatum.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpRDatum.Location = New System.Drawing.Point(270, 20)
        Me.dtpRDatum.Name = "dtpRDatum"
        Me.dtpRDatum.Size = New System.Drawing.Size(95, 20)
        Me.dtpRDatum.TabIndex = 57
        '
        'tbRNr
        '
        Me.tbRNr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbRNr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbRNr.Location = New System.Drawing.Point(85, 185)
        Me.tbRNr.MaxLength = 10
        Me.tbRNr.Name = "tbRNr"
        Me.tbRNr.Size = New System.Drawing.Size(77, 20)
        Me.tbRNr.TabIndex = 56
        Me.tbRNr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(175, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(89, 20)
        Me.Label3.TabIndex = 30
        Me.Label3.Text = "Datum"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 183)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 20)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "Rechnungs-Nr.:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rbExtraORe
        '
        Me.rbExtraORe.AutoSize = True
        Me.rbExtraORe.Checked = True
        Me.rbExtraORe.Location = New System.Drawing.Point(178, 93)
        Me.rbExtraORe.Name = "rbExtraORe"
        Me.rbExtraORe.Size = New System.Drawing.Size(134, 17)
        Me.rbExtraORe.TabIndex = 28
        Me.rbExtraORe.TabStop = True
        Me.rbExtraORe.Text = "Extras ohne Rechnung"
        Me.rbExtraORe.UseVisualStyleBackColor = True
        '
        'rbExtraGRe
        '
        Me.rbExtraGRe.AutoSize = True
        Me.rbExtraGRe.Location = New System.Drawing.Point(178, 137)
        Me.rbExtraGRe.Name = "rbExtraGRe"
        Me.rbExtraGRe.Size = New System.Drawing.Size(213, 17)
        Me.rbExtraGRe.TabIndex = 27
        Me.rbExtraGRe.Text = "Extras auf einer gesonderten Rechnung"
        Me.rbExtraGRe.UseVisualStyleBackColor = True
        '
        'rbExtraMRe
        '
        Me.rbExtraMRe.AutoSize = True
        Me.rbExtraMRe.Location = New System.Drawing.Point(178, 114)
        Me.rbExtraMRe.Name = "rbExtraMRe"
        Me.rbExtraMRe.Size = New System.Drawing.Size(159, 17)
        Me.rbExtraMRe.TabIndex = 26
        Me.rbExtraMRe.Text = "Extras mit auf der Rechnung"
        Me.rbExtraMRe.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Gainsboro
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(148, 20)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Zimmer"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gbRechnung
        '
        Me.gbRechnung.Controls.Add(Me.buMail)
        Me.gbRechnung.Controls.Add(Me.lbBis)
        Me.gbRechnung.Controls.Add(Me.lbVon)
        Me.gbRechnung.Controls.Add(Me.lbZusatz)
        Me.gbRechnung.Controls.Add(Me.coZimmer)
        Me.gbRechnung.Controls.Add(Me.Label13)
        Me.gbRechnung.Controls.Add(Me.coSteuer)
        Me.gbRechnung.Controls.Add(Me.btAbbruch)
        Me.gbRechnung.Controls.Add(Me.btPosSpeichern)
        Me.gbRechnung.Controls.Add(Me.btClose)
        Me.gbRechnung.Controls.Add(Me.btDruck)
        Me.gbRechnung.Controls.Add(Me.lbGesamt)
        Me.gbRechnung.Controls.Add(Me.lbAnzahlung)
        Me.gbRechnung.Controls.Add(Me.lbGSt7)
        Me.gbRechnung.Controls.Add(Me.lbGNetto)
        Me.gbRechnung.Controls.Add(Me.Label26)
        Me.gbRechnung.Controls.Add(Me.Label24)
        Me.gbRechnung.Controls.Add(Me.Label21)
        Me.gbRechnung.Controls.Add(Me.Label19)
        Me.gbRechnung.Controls.Add(Me.lbBrutto)
        Me.gbRechnung.Controls.Add(Me.Label18)
        Me.gbRechnung.Controls.Add(Me.lbNetto)
        Me.gbRechnung.Controls.Add(Me.Label15)
        Me.gbRechnung.Controls.Add(Me.lbSteuer)
        Me.gbRechnung.Controls.Add(Me.Label11)
        Me.gbRechnung.Controls.Add(Me.Label10)
        Me.gbRechnung.Controls.Add(Me.tbBetrag)
        Me.gbRechnung.Controls.Add(Me.Label9)
        Me.gbRechnung.Controls.Add(Me.tbText)
        Me.gbRechnung.Controls.Add(Me.Label7)
        Me.gbRechnung.Controls.Add(Me.tbMenge)
        Me.gbRechnung.Controls.Add(Me.Label5)
        Me.gbRechnung.Controls.Add(Me.tbPos)
        Me.gbRechnung.Controls.Add(Me.Label4)
        Me.gbRechnung.Controls.Add(Me.dgRechnung)
        Me.gbRechnung.Location = New System.Drawing.Point(12, 241)
        Me.gbRechnung.Name = "gbRechnung"
        Me.gbRechnung.Size = New System.Drawing.Size(719, 445)
        Me.gbRechnung.TabIndex = 5
        Me.gbRechnung.TabStop = False
        Me.gbRechnung.Text = "Rechnungspositionen"
        '
        'buMail
        '
        Me.buMail.Location = New System.Drawing.Point(307, 401)
        Me.buMail.Name = "buMail"
        Me.buMail.Size = New System.Drawing.Size(147, 23)
        Me.buMail.TabIndex = 93
        Me.buMail.Text = "Rechnung Mail"
        Me.buMail.UseVisualStyleBackColor = True
        '
        'lbBis
        '
        Me.lbBis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbBis.Location = New System.Drawing.Point(55, 373)
        Me.lbBis.Name = "lbBis"
        Me.lbBis.Size = New System.Drawing.Size(50, 20)
        Me.lbBis.TabIndex = 92
        Me.lbBis.Text = "lbBis"
        Me.lbBis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lbBis.Visible = False
        '
        'lbVon
        '
        Me.lbVon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbVon.Location = New System.Drawing.Point(9, 373)
        Me.lbVon.Name = "lbVon"
        Me.lbVon.Size = New System.Drawing.Size(50, 20)
        Me.lbVon.TabIndex = 91
        Me.lbVon.Text = "lbVon"
        Me.lbVon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lbVon.Visible = False
        '
        'lbZusatz
        '
        Me.lbZusatz.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbZusatz.Location = New System.Drawing.Point(9, 353)
        Me.lbZusatz.Name = "lbZusatz"
        Me.lbZusatz.Size = New System.Drawing.Size(50, 20)
        Me.lbZusatz.TabIndex = 90
        Me.lbZusatz.Text = "lbZusatz"
        Me.lbZusatz.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lbZusatz.Visible = False
        '
        'coZimmer
        '
        Me.coZimmer.FormattingEnabled = True
        Me.coZimmer.Location = New System.Drawing.Point(67, 293)
        Me.coZimmer.Name = "coZimmer"
        Me.coZimmer.Size = New System.Drawing.Size(63, 21)
        Me.coZimmer.TabIndex = 89
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(65, 271)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(66, 20)
        Me.Label13.TabIndex = 87
        Me.Label13.Text = "Zimmer"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'coSteuer
        '
        Me.coSteuer.FormattingEnabled = True
        Me.coSteuer.Items.AddRange(New Object() {"0", "7", "19"})
        Me.coSteuer.Location = New System.Drawing.Point(440, 293)
        Me.coSteuer.Name = "coSteuer"
        Me.coSteuer.Size = New System.Drawing.Size(47, 21)
        Me.coSteuer.TabIndex = 86
        '
        'btAbbruch
        '
        Me.btAbbruch.Location = New System.Drawing.Point(307, 326)
        Me.btAbbruch.Name = "btAbbruch"
        Me.btAbbruch.Size = New System.Drawing.Size(106, 23)
        Me.btAbbruch.TabIndex = 85
        Me.btAbbruch.Text = "Abbruch"
        Me.btAbbruch.UseVisualStyleBackColor = True
        '
        'btPosSpeichern
        '
        Me.btPosSpeichern.Location = New System.Drawing.Point(9, 327)
        Me.btPosSpeichern.Name = "btPosSpeichern"
        Me.btPosSpeichern.Size = New System.Drawing.Size(106, 23)
        Me.btPosSpeichern.TabIndex = 84
        Me.btPosSpeichern.Text = "Position speichen"
        Me.btPosSpeichern.UseVisualStyleBackColor = True
        '
        'btClose
        '
        Me.btClose.Location = New System.Drawing.Point(12, 402)
        Me.btClose.Name = "btClose"
        Me.btClose.Size = New System.Drawing.Size(106, 23)
        Me.btClose.TabIndex = 83
        Me.btClose.Text = "Schliessen"
        Me.btClose.UseVisualStyleBackColor = True
        '
        'btDruck
        '
        Me.btDruck.Location = New System.Drawing.Point(137, 401)
        Me.btDruck.Name = "btDruck"
        Me.btDruck.Size = New System.Drawing.Size(139, 23)
        Me.btDruck.TabIndex = 82
        Me.btDruck.Text = "Drucken"
        Me.btDruck.UseVisualStyleBackColor = True
        '
        'lbGesamt
        '
        Me.lbGesamt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbGesamt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbGesamt.Location = New System.Drawing.Point(629, 402)
        Me.lbGesamt.Name = "lbGesamt"
        Me.lbGesamt.Size = New System.Drawing.Size(74, 20)
        Me.lbGesamt.TabIndex = 80
        Me.lbGesamt.Text = "0,00"
        Me.lbGesamt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbAnzahlung
        '
        Me.lbAnzahlung.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbAnzahlung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbAnzahlung.ForeColor = System.Drawing.Color.Red
        Me.lbAnzahlung.Location = New System.Drawing.Point(629, 373)
        Me.lbAnzahlung.Name = "lbAnzahlung"
        Me.lbAnzahlung.Size = New System.Drawing.Size(74, 20)
        Me.lbAnzahlung.TabIndex = 79
        Me.lbAnzahlung.Text = "0,00"
        Me.lbAnzahlung.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbGSt7
        '
        Me.lbGSt7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbGSt7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbGSt7.Location = New System.Drawing.Point(629, 349)
        Me.lbGSt7.Name = "lbGSt7"
        Me.lbGSt7.Size = New System.Drawing.Size(74, 20)
        Me.lbGSt7.TabIndex = 77
        Me.lbGSt7.Text = "0,00"
        Me.lbGSt7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbGNetto
        '
        Me.lbGNetto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbGNetto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbGNetto.Location = New System.Drawing.Point(629, 327)
        Me.lbGNetto.Name = "lbGNetto"
        Me.lbGNetto.Size = New System.Drawing.Size(74, 20)
        Me.lbGNetto.TabIndex = 76
        Me.lbGNetto.Text = "0,00"
        Me.lbGNetto.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label26
        '
        Me.Label26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(469, 402)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(117, 20)
        Me.Label26.TabIndex = 75
        Me.Label26.Text = "Rechnungsbetrag:"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label24
        '
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(465, 373)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(117, 20)
        Me.Label24.TabIndex = 74
        Me.Label24.Text = "- Anzahlung"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label21
        '
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(469, 349)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(117, 20)
        Me.Label21.TabIndex = 72
        Me.Label21.Text = "Steuer "
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label19
        '
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(472, 327)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(112, 20)
        Me.Label19.TabIndex = 71
        Me.Label19.Text = "Netto"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbBrutto
        '
        Me.lbBrutto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbBrutto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbBrutto.Location = New System.Drawing.Point(629, 294)
        Me.lbBrutto.Name = "lbBrutto"
        Me.lbBrutto.Size = New System.Drawing.Size(74, 20)
        Me.lbBrutto.TabIndex = 70
        Me.lbBrutto.Text = "0,00"
        Me.lbBrutto.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(629, 271)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(74, 20)
        Me.Label18.TabIndex = 69
        Me.Label18.Text = "Brutto"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbNetto
        '
        Me.lbNetto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbNetto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbNetto.Location = New System.Drawing.Point(549, 294)
        Me.lbNetto.Name = "lbNetto"
        Me.lbNetto.Size = New System.Drawing.Size(74, 20)
        Me.lbNetto.TabIndex = 68
        Me.lbNetto.Text = "0,00"
        Me.lbNetto.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(549, 271)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(74, 20)
        Me.Label15.TabIndex = 67
        Me.Label15.Text = "Netto"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbSteuer
        '
        Me.lbSteuer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbSteuer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbSteuer.Location = New System.Drawing.Point(493, 294)
        Me.lbSteuer.Name = "lbSteuer"
        Me.lbSteuer.Size = New System.Drawing.Size(50, 20)
        Me.lbSteuer.TabIndex = 66
        Me.lbSteuer.Text = "0,00"
        Me.lbSteuer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(493, 271)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(50, 20)
        Me.Label11.TabIndex = 65
        Me.Label11.Text = "Steuer"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(440, 271)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(47, 20)
        Me.Label10.TabIndex = 63
        Me.Label10.Text = "Steuer %"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbBetrag
        '
        Me.tbBetrag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbBetrag.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbBetrag.Location = New System.Drawing.Point(384, 294)
        Me.tbBetrag.MaxLength = 10
        Me.tbBetrag.Name = "tbBetrag"
        Me.tbBetrag.Size = New System.Drawing.Size(50, 20)
        Me.tbBetrag.TabIndex = 62
        Me.tbBetrag.Text = "0,00"
        Me.tbBetrag.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(381, 271)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(53, 20)
        Me.Label9.TabIndex = 61
        Me.Label9.Text = "Betrag"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbText
        '
        Me.tbText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbText.Location = New System.Drawing.Point(190, 293)
        Me.tbText.MaxLength = 10
        Me.tbText.Name = "tbText"
        Me.tbText.Size = New System.Drawing.Size(188, 20)
        Me.tbText.TabIndex = 60
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(187, 271)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(191, 20)
        Me.Label7.TabIndex = 59
        Me.Label7.Text = "Text-Position"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbMenge
        '
        Me.tbMenge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbMenge.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbMenge.Location = New System.Drawing.Point(134, 293)
        Me.tbMenge.MaxLength = 10
        Me.tbMenge.Name = "tbMenge"
        Me.tbMenge.Size = New System.Drawing.Size(50, 20)
        Me.tbMenge.TabIndex = 58
        Me.tbMenge.Text = "0"
        Me.tbMenge.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(134, 270)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(50, 20)
        Me.Label5.TabIndex = 57
        Me.Label5.Text = "Menge"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbPos
        '
        Me.tbPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPos.Enabled = False
        Me.tbPos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbPos.Location = New System.Drawing.Point(9, 294)
        Me.tbPos.MaxLength = 10
        Me.tbPos.Name = "tbPos"
        Me.tbPos.Size = New System.Drawing.Size(50, 20)
        Me.tbPos.TabIndex = 56
        Me.tbPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(9, 271)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 20)
        Me.Label4.TabIndex = 30
        Me.Label4.Text = "Pos."
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dgRechnung
        '
        Me.dgRechnung.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgRechnung.Location = New System.Drawing.Point(9, 19)
        Me.dgRechnung.Name = "dgRechnung"
        Me.dgRechnung.Size = New System.Drawing.Size(694, 249)
        Me.dgRechnung.TabIndex = 0
        '
        'tiGast
        '
        Me.tiGast.Interval = 1000
        '
        'frmRechnung
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(741, 698)
        Me.ControlBox = False
        Me.Controls.Add(Me.gbRechnung)
        Me.Controls.Add(Me.gbOption)
        Me.Controls.Add(Me.gbGast)
        Me.Name = "frmRechnung"
        Me.Text = "Rechnung erstellen"
        Me.gbGast.ResumeLayout(False)
        Me.gbOption.ResumeLayout(False)
        Me.gbOption.PerformLayout()
        Me.gbRechnung.ResumeLayout(False)
        Me.gbRechnung.PerformLayout()
        CType(Me.dgRechnung, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbGast As System.Windows.Forms.GroupBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lbGastID As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents chliZimmer As System.Windows.Forms.CheckedListBox
    Friend WithEvents gbOption As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents dtpRDatum As System.Windows.Forms.DateTimePicker
    Friend WithEvents tbRNr As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents rbExtraORe As System.Windows.Forms.RadioButton
    Friend WithEvents rbExtraGRe As System.Windows.Forms.RadioButton
    Friend WithEvents rbExtraMRe As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents gbRechnung As System.Windows.Forms.GroupBox
    Friend WithEvents lbBrutto As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents lbNetto As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents lbSteuer As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tbBetrag As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents tbText As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents tbMenge As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tbPos As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents dgRechnung As System.Windows.Forms.DataGridView
    Friend WithEvents btClose As System.Windows.Forms.Button
    Friend WithEvents btDruck As System.Windows.Forms.Button
    Friend WithEvents lbGesamt As System.Windows.Forms.Label
    Friend WithEvents lbAnzahlung As System.Windows.Forms.Label
    Friend WithEvents lbGSt7 As System.Windows.Forms.Label
    Friend WithEvents lbGNetto As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents lbBID As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents coSteuer As System.Windows.Forms.ComboBox
    Friend WithEvents btAbbruch As System.Windows.Forms.Button
    Friend WithEvents btPosSpeichern As System.Windows.Forms.Button
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents coZimmer As System.Windows.Forms.ComboBox
    Friend WithEvents btGastDaten As System.Windows.Forms.Button
    Friend WithEvents tiGast As System.Windows.Forms.Timer
    Friend WithEvents lbLand As System.Windows.Forms.Label
    Friend WithEvents lbOrt As System.Windows.Forms.Label
    Friend WithEvents lbPLZ As System.Windows.Forms.Label
    Friend WithEvents lbStrasse As System.Windows.Forms.Label
    Friend WithEvents lbVorname As System.Windows.Forms.Label
    Friend WithEvents lbName2 As System.Windows.Forms.Label
    Friend WithEvents lbName1 As System.Windows.Forms.Label
    Friend WithEvents lbAnrede As System.Windows.Forms.Label
    Friend WithEvents lbBis As System.Windows.Forms.Label
    Friend WithEvents lbVon As System.Windows.Forms.Label
    Friend WithEvents lbZusatz As System.Windows.Forms.Label
    Friend WithEvents buMail As System.Windows.Forms.Button
    Friend WithEvents cbBar As System.Windows.Forms.CheckBox
    Friend WithEvents rbPausch As System.Windows.Forms.RadioButton
    Friend WithEvents rbStorno As System.Windows.Forms.RadioButton
End Class
