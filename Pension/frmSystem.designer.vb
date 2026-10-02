<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSystem
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSystem))
        Me.ToolStrip = New System.Windows.Forms.ToolStrip()
        Me.tsbClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbEdit = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSpeichern = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbESC = New System.Windows.Forms.ToolStripButton()
        Me.tsbLableUpgrade = New System.Windows.Forms.ToolStripLabel()
        Me.fbDialog = New System.Windows.Forms.FolderBrowserDialog()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.tpEMail = New System.Windows.Forms.TabPage()
        Me.gpEMail = New System.Windows.Forms.GroupBox()
        Me.cmdTestEMail = New System.Windows.Forms.Button()
        Me.tbTage = New System.Windows.Forms.TextBox()
        Me.tbPWort = New System.Windows.Forms.TextBox()
        Me.tbUName = New System.Windows.Forms.TextBox()
        Me.tbName = New System.Windows.Forms.TextBox()
        Me.tbEMail = New System.Windows.Forms.TextBox()
        Me.rbUU = New System.Windows.Forms.RadioButton()
        Me.rbMIME = New System.Windows.Forms.RadioButton()
        Me.chHTML = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chServer = New System.Windows.Forms.CheckBox()
        Me.lbTage = New System.Windows.Forms.Label()
        Me.lbPWort = New System.Windows.Forms.Label()
        Me.lbUName = New System.Windows.Forms.Label()
        Me.lbName = New System.Windows.Forms.Label()
        Me.lbEMail = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lbLableUser = New System.Windows.Forms.Label()
        Me.lbLablePWort = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tbSMTP = New System.Windows.Forms.TextBox()
        Me.lbSMTP = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tpAnschrift = New System.Windows.Forms.TabPage()
        Me.gbBasisdaten = New System.Windows.Forms.GroupBox()
        Me.gpDir = New System.Windows.Forms.GroupBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.tbDatevDir = New System.Windows.Forms.TextBox()
        Me.lbDatevDir = New System.Windows.Forms.Label()
        Me.tbMwst2 = New System.Windows.Forms.TextBox()
        Me.lbMwst2 = New System.Windows.Forms.Label()
        Me.cmdDatevDir = New System.Windows.Forms.Button()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.tbSaveDir = New System.Windows.Forms.TextBox()
        Me.lbSaveDir = New System.Windows.Forms.Label()
        Me.tbAblageDir = New System.Windows.Forms.TextBox()
        Me.lbAblageDir = New System.Windows.Forms.Label()
        Me.cmdSaveDir = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdAblageDir = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.tbWeb = New System.Windows.Forms.TextBox()
        Me.lbWeb = New System.Windows.Forms.Label()
        Me.gpDiverses = New System.Windows.Forms.GroupBox()
        Me.lbRFIDPort = New System.Windows.Forms.Label()
        Me.tbRFIDPort = New System.Windows.Forms.TextBox()
        Me.tbRNr = New System.Windows.Forms.TextBox()
        Me.Label67 = New System.Windows.Forms.Label()
        Me.lbIPSchloss = New System.Windows.Forms.Label()
        Me.lbRNr = New System.Windows.Forms.Label()
        Me.tbIPSchloss = New System.Windows.Forms.TextBox()
        Me.lbNetUser = New System.Windows.Forms.Label()
        Me.tbBK = New System.Windows.Forms.TextBox()
        Me.lbBK = New System.Windows.Forms.Label()
        Me.tbNetUser = New System.Windows.Forms.TextBox()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.tbKK = New System.Windows.Forms.TextBox()
        Me.Label66 = New System.Windows.Forms.Label()
        Me.lbKK = New System.Windows.Forms.Label()
        Me.Label65 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tbUStID = New System.Windows.Forms.TextBox()
        Me.lbUStID = New System.Windows.Forms.Label()
        Me.tbUStNr = New System.Windows.Forms.TextBox()
        Me.lbUStNr = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.lbBNr = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.lbKNr = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.tbTel4 = New System.Windows.Forms.TextBox()
        Me.lbTel4 = New System.Windows.Forms.Label()
        Me.tbTel3 = New System.Windows.Forms.TextBox()
        Me.lbTel3 = New System.Windows.Forms.Label()
        Me.tbTel2 = New System.Windows.Forms.TextBox()
        Me.lbTel2 = New System.Windows.Forms.Label()
        Me.tbTel1 = New System.Windows.Forms.TextBox()
        Me.lbTel1 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.tbOrt = New System.Windows.Forms.TextBox()
        Me.lbOrt = New System.Windows.Forms.Label()
        Me.tbPLZ = New System.Windows.Forms.TextBox()
        Me.lbPLZ = New System.Windows.Forms.Label()
        Me.tbStrasse = New System.Windows.Forms.TextBox()
        Me.lbStrasse = New System.Windows.Forms.Label()
        Me.tbFirma = New System.Windows.Forms.TextBox()
        Me.lbFirma = New System.Windows.Forms.Label()
        Me.lbLabelOrt = New System.Windows.Forms.Label()
        Me.lbLabelPLZ = New System.Windows.Forms.Label()
        Me.lbLabelStr = New System.Windows.Forms.Label()
        Me.lbLabelGebName = New System.Windows.Forms.Label()
        Me.tcSystem = New System.Windows.Forms.TabControl()
        Me.tpKonto = New System.Windows.Forms.TabPage()
        Me.paKonto = New System.Windows.Forms.Panel()
        Me.lbKontoID = New System.Windows.Forms.Label()
        Me.tbTyp = New System.Windows.Forms.TextBox()
        Me.tbBIC = New System.Windows.Forms.TextBox()
        Me.tbKZ = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.tbIBAN = New System.Windows.Forms.TextBox()
        Me.tbInstitut = New System.Windows.Forms.TextBox()
        Me.tbBLZ = New System.Windows.Forms.TextBox()
        Me.tbKTO = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.tsKonto = New System.Windows.Forms.ToolStrip()
        Me.tsbNewKonto = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbEditKonto = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSaveKonto = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbBreakKonto = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDelKonto = New System.Windows.Forms.ToolStripButton()
        Me.lvKonto = New System.Windows.Forms.ListView()
        Me.tpObjekte = New System.Windows.Forms.TabPage()
        Me.paObjekt = New System.Windows.Forms.Panel()
        Me.lbRGBString = New System.Windows.Forms.Label()
        Me.btColorObjekt = New System.Windows.Forms.Button()
        Me.lbObjektID = New System.Windows.Forms.Label()
        Me.tbOHNr = New System.Windows.Forms.TextBox()
        Me.tbOOrtsteil = New System.Windows.Forms.TextBox()
        Me.tbOOrt = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.tbOPLZ = New System.Windows.Forms.TextBox()
        Me.tbOTelefon = New System.Windows.Forms.TextBox()
        Me.tbOStr = New System.Windows.Forms.TextBox()
        Me.tbOName = New System.Windows.Forms.TextBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.lvObjekt = New System.Windows.Forms.ListView()
        Me.tsObjekt = New System.Windows.Forms.ToolStrip()
        Me.tsbNewObj = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbEditObj = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSaveObj = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbBraeckObj = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDelObj = New System.Windows.Forms.ToolStripButton()
        Me.tpZimmer = New System.Windows.Forms.TabPage()
        Me.paZimmer = New System.Windows.Forms.Panel()
        Me.Label94 = New System.Windows.Forms.Label()
        Me.Label93 = New System.Windows.Forms.Label()
        Me.tbDatei = New System.Windows.Forms.TextBox()
        Me.tbSaveCode = New System.Windows.Forms.TextBox()
        Me.chCode = New System.Windows.Forms.CheckBox()
        Me.Label92 = New System.Windows.Forms.Label()
        Me.Label91 = New System.Windows.Forms.Label()
        Me.Label57 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.tbTrans5 = New System.Windows.Forms.TextBox()
        Me.tbTrans4 = New System.Windows.Forms.TextBox()
        Me.tbTrans3 = New System.Windows.Forms.TextBox()
        Me.tbTrans2 = New System.Windows.Forms.TextBox()
        Me.tbTrans1 = New System.Windows.Forms.TextBox()
        Me.tbZP10 = New System.Windows.Forms.TextBox()
        Me.tbZP9 = New System.Windows.Forms.TextBox()
        Me.tbZP8 = New System.Windows.Forms.TextBox()
        Me.tbZP7 = New System.Windows.Forms.TextBox()
        Me.tbZP6 = New System.Windows.Forms.TextBox()
        Me.tbZP5 = New System.Windows.Forms.TextBox()
        Me.tbZP4 = New System.Windows.Forms.TextBox()
        Me.tbZP3 = New System.Windows.Forms.TextBox()
        Me.tbZP2 = New System.Windows.Forms.TextBox()
        Me.tbZP1 = New System.Windows.Forms.TextBox()
        Me.Label90 = New System.Windows.Forms.Label()
        Me.Label89 = New System.Windows.Forms.Label()
        Me.Label88 = New System.Windows.Forms.Label()
        Me.Label87 = New System.Windows.Forms.Label()
        Me.Label86 = New System.Windows.Forms.Label()
        Me.Label85 = New System.Windows.Forms.Label()
        Me.Label84 = New System.Windows.Forms.Label()
        Me.Label83 = New System.Windows.Forms.Label()
        Me.Label82 = New System.Windows.Forms.Label()
        Me.Label81 = New System.Windows.Forms.Label()
        Me.Label80 = New System.Windows.Forms.Label()
        Me.tbZBettenKi = New System.Windows.Forms.TextBox()
        Me.tbZBettenEr = New System.Windows.Forms.TextBox()
        Me.tbZBettenMin = New System.Windows.Forms.TextBox()
        Me.Label79 = New System.Windows.Forms.Label()
        Me.Label78 = New System.Windows.Forms.Label()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.tbZNummer = New System.Windows.Forms.TextBox()
        Me.chFeWo = New System.Windows.Forms.CheckBox()
        Me.coZArt = New System.Windows.Forms.ComboBox()
        Me.tbZBetten = New System.Windows.Forms.TextBox()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.coObjekt = New System.Windows.Forms.ComboBox()
        Me.lbZimmerID = New System.Windows.Forms.Label()
        Me.tbZAus = New System.Windows.Forms.TextBox()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.tbZArt = New System.Windows.Forms.TextBox()
        Me.tbZName = New System.Windows.Forms.TextBox()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.tsZimmer = New System.Windows.Forms.ToolStrip()
        Me.tsbNeuZim = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbEditZim = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSaveZim = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbBraekZim = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDelZim = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator28 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmZimmer = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tsmImportZimmer = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmExportZimmer = New System.Windows.Forms.ToolStripMenuItem()
        Me.lvZimmer = New System.Windows.Forms.ListView()
        Me.tpUser = New System.Windows.Forms.TabPage()
        Me.paUser = New System.Windows.Forms.Panel()
        Me.tbKUser = New System.Windows.Forms.TextBox()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.chKlar = New System.Windows.Forms.CheckBox()
        Me.lbUserID = New System.Windows.Forms.Label()
        Me.tbUPassWD = New System.Windows.Forms.TextBox()
        Me.tbUUser = New System.Windows.Forms.TextBox()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.lvUser = New System.Windows.Forms.ListView()
        Me.tsUser = New System.Windows.Forms.ToolStrip()
        Me.tsbNewUser = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbEditUser = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSaveUser = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbBraeckUser = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDelUser = New System.Windows.Forms.ToolStripButton()
        Me.tpBText = New System.Windows.Forms.TabPage()
        Me.paBuch = New System.Windows.Forms.Panel()
        Me.Label62 = New System.Windows.Forms.Label()
        Me.tbZZiel = New System.Windows.Forms.TextBox()
        Me.rbBuch = New System.Windows.Forms.RadioButton()
        Me.rbMakro = New System.Windows.Forms.RadioButton()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.tbBuchEn = New System.Windows.Forms.TextBox()
        Me.tbBuchDe = New System.Windows.Forms.TextBox()
        Me.tbBez = New System.Windows.Forms.TextBox()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.liBuch = New System.Windows.Forms.ListBox()
        Me.tsBuch = New System.Windows.Forms.ToolStrip()
        Me.tsbNewBuch = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator24 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbEditBuch = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator25 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSaveBuch = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator26 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbBreakBuch = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator27 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDelBuch = New System.Windows.Forms.ToolStripButton()
        Me.tscSprache = New System.Windows.Forms.ToolStripComboBox()
        Me.tpPreise = New System.Windows.Forms.TabPage()
        Me.lvPreise = New System.Windows.Forms.ListView()
        Me.paPreise = New System.Windows.Forms.Panel()
        Me.coEvent = New System.Windows.Forms.ComboBox()
        Me.lbPZim = New System.Windows.Forms.Label()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.tbDauerG = New System.Windows.Forms.TextBox()
        Me.tbPreisG = New System.Windows.Forms.TextBox()
        Me.chP7 = New System.Windows.Forms.CheckBox()
        Me.chP6 = New System.Windows.Forms.CheckBox()
        Me.chP5 = New System.Windows.Forms.CheckBox()
        Me.chP4 = New System.Windows.Forms.CheckBox()
        Me.chP3 = New System.Windows.Forms.CheckBox()
        Me.chP2 = New System.Windows.Forms.CheckBox()
        Me.chP1 = New System.Windows.Forms.CheckBox()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.tbD7 = New System.Windows.Forms.TextBox()
        Me.tbD6 = New System.Windows.Forms.TextBox()
        Me.tbD5 = New System.Windows.Forms.TextBox()
        Me.tbD4 = New System.Windows.Forms.TextBox()
        Me.tbD3 = New System.Windows.Forms.TextBox()
        Me.tbD2 = New System.Windows.Forms.TextBox()
        Me.tbD1 = New System.Windows.Forms.TextBox()
        Me.tbPreis7 = New System.Windows.Forms.TextBox()
        Me.tbPreis6 = New System.Windows.Forms.TextBox()
        Me.dtpBis = New System.Windows.Forms.DateTimePicker()
        Me.dtpVon = New System.Windows.Forms.DateTimePicker()
        Me.lbPZimID = New System.Windows.Forms.Label()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.tbPreis5 = New System.Windows.Forms.TextBox()
        Me.tbPreis4 = New System.Windows.Forms.TextBox()
        Me.tbPreis3 = New System.Windows.Forms.TextBox()
        Me.tbPreis2 = New System.Windows.Forms.TextBox()
        Me.tbPreis1 = New System.Windows.Forms.TextBox()
        Me.lbPID = New System.Windows.Forms.Label()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.Übernachtungsart = New System.Windows.Forms.Label()
        Me.tsPreise = New System.Windows.Forms.ToolStrip()
        Me.tsbPNeu = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator20 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbPEdit = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator21 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbPSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator22 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbPBreak = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator23 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbPDel = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator29 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator44 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator45 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbPZimmer = New System.Windows.Forms.ToolStripComboBox()
        Me.tsbPZimmer1 = New System.Windows.Forms.ToolStripComboBox()
        Me.tsbPCopy = New System.Windows.Forms.ToolStripButton()
        Me.tsbCoJahr = New System.Windows.Forms.ToolStripComboBox()
        Me.tsbPZimmerCopyJahr = New System.Windows.Forms.ToolStripButton()
        Me.tpSasion = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.mcSaisonEnde = New System.Windows.Forms.MonthCalendar()
        Me.mcSaisonAnfang = New System.Windows.Forms.MonthCalendar()
        Me.coSaison = New System.Windows.Forms.ComboBox()
        Me.tsSaison = New System.Windows.Forms.ToolStrip()
        Me.tsbNeuSaison = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator30 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbEditSaison = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator31 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSaveSaison = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator32 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbBreackSaison = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator33 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDelSaison = New System.Windows.Forms.ToolStripButton()
        Me.lvSaison = New System.Windows.Forms.ListView()
        Me.tpFarben = New System.Windows.Forms.TabPage()
        Me.btSaveColor = New System.Windows.Forms.Button()
        Me.lbBackColor = New System.Windows.Forms.Label()
        Me.btBackColor = New System.Windows.Forms.Button()
        Me.btForeColor = New System.Windows.Forms.Button()
        Me.lvColor = New System.Windows.Forms.ListView()
        Me.tpDruck = New System.Windows.Forms.TabPage()
        Me.lvDruck = New System.Windows.Forms.ListView()
        Me.tsDruck = New System.Windows.Forms.ToolStrip()
        Me.tsbDruckSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator39 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbDruckDel = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator38 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbcbDruck = New System.Windows.Forms.ToolStripComboBox()
        Me.tpSprache = New System.Windows.Forms.TabPage()
        Me.ToolStrip3 = New System.Windows.Forms.ToolStrip()
        Me.tsSpracheVerw = New System.Windows.Forms.ToolStripButton()
        Me.tsSpracheSave = New System.Windows.Forms.ToolStripButton()
        Me.tsSpracheDel = New System.Windows.Forms.ToolStripButton()
        Me.dgvSprache = New System.Windows.Forms.DataGridView()
        Me.cbText = New System.Windows.Forms.ComboBox()
        Me.ToolStrip.SuspendLayout()
        Me.tpEMail.SuspendLayout()
        Me.gpEMail.SuspendLayout()
        Me.tpAnschrift.SuspendLayout()
        Me.gbBasisdaten.SuspendLayout()
        Me.gpDir.SuspendLayout()
        Me.gpDiverses.SuspendLayout()
        Me.tcSystem.SuspendLayout()
        Me.tpKonto.SuspendLayout()
        Me.paKonto.SuspendLayout()
        Me.tsKonto.SuspendLayout()
        Me.tpObjekte.SuspendLayout()
        Me.paObjekt.SuspendLayout()
        Me.tsObjekt.SuspendLayout()
        Me.tpZimmer.SuspendLayout()
        Me.paZimmer.SuspendLayout()
        Me.tsZimmer.SuspendLayout()
        Me.tpUser.SuspendLayout()
        Me.paUser.SuspendLayout()
        Me.tsUser.SuspendLayout()
        Me.tpBText.SuspendLayout()
        Me.paBuch.SuspendLayout()
        Me.tsBuch.SuspendLayout()
        Me.tpPreise.SuspendLayout()
        Me.paPreise.SuspendLayout()
        Me.tsPreise.SuspendLayout()
        Me.tpSasion.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.tsSaison.SuspendLayout()
        Me.tpFarben.SuspendLayout()
        Me.tpDruck.SuspendLayout()
        Me.tsDruck.SuspendLayout()
        Me.tpSprache.SuspendLayout()
        Me.ToolStrip3.SuspendLayout()
        CType(Me.dgvSprache, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip
        '
        Me.ToolStrip.AutoSize = False
        Me.ToolStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbClose, Me.ToolStripSeparator10, Me.tsbEdit, Me.ToolStripSeparator1, Me.tsbSpeichern, Me.ToolStripSeparator12, Me.tsbESC, Me.tsbLableUpgrade})
        Me.ToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.ToolStrip.Size = New System.Drawing.Size(1478, 37)
        Me.ToolStrip.Stretch = True
        Me.ToolStrip.TabIndex = 36
        '
        'tsbClose
        '
        Me.tsbClose.AutoSize = False
        Me.tsbClose.Image = Global.Pension.My.Resources.Resources.door02
        Me.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbClose.Name = "tsbClose"
        Me.tsbClose.Size = New System.Drawing.Size(40, 33)
        Me.tsbClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbClose.ToolTipText = "Systemeinstellungen beenden"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 37)
        '
        'tsbEdit
        '
        Me.tsbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbEdit.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1474_ico_Ico1
        Me.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEdit.Name = "tsbEdit"
        Me.tsbEdit.Size = New System.Drawing.Size(34, 32)
        Me.tsbEdit.Text = "Bearbeiten"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 37)
        '
        'tsbSpeichern
        '
        Me.tsbSpeichern.AutoSize = False
        Me.tsbSpeichern.Enabled = False
        Me.tsbSpeichern.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSpeichern.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSpeichern.Name = "tsbSpeichern"
        Me.tsbSpeichern.RightToLeftAutoMirrorImage = True
        Me.tsbSpeichern.Size = New System.Drawing.Size(40, 33)
        Me.tsbSpeichern.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbSpeichern.ToolTipText = "Änderung speichern"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(6, 37)
        '
        'tsbESC
        '
        Me.tsbESC.AutoSize = False
        Me.tsbESC.Enabled = False
        Me.tsbESC.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsbESC.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbESC.Name = "tsbESC"
        Me.tsbESC.Size = New System.Drawing.Size(40, 33)
        Me.tsbESC.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbESC.ToolTipText = "Bearbeitung abbrechen"
        '
        'tsbLableUpgrade
        '
        Me.tsbLableUpgrade.Name = "tsbLableUpgrade"
        Me.tsbLableUpgrade.Size = New System.Drawing.Size(0, 32)
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'tpEMail
        '
        Me.tpEMail.Controls.Add(Me.gpEMail)
        Me.tpEMail.Location = New System.Drawing.Point(4, 29)
        Me.tpEMail.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpEMail.Name = "tpEMail"
        Me.tpEMail.Size = New System.Drawing.Size(1441, 879)
        Me.tpEMail.TabIndex = 2
        Me.tpEMail.Text = "E-Mail"
        Me.tpEMail.UseVisualStyleBackColor = True
        '
        'gpEMail
        '
        Me.gpEMail.Controls.Add(Me.cmdTestEMail)
        Me.gpEMail.Controls.Add(Me.tbTage)
        Me.gpEMail.Controls.Add(Me.tbPWort)
        Me.gpEMail.Controls.Add(Me.tbUName)
        Me.gpEMail.Controls.Add(Me.tbName)
        Me.gpEMail.Controls.Add(Me.tbEMail)
        Me.gpEMail.Controls.Add(Me.rbUU)
        Me.gpEMail.Controls.Add(Me.rbMIME)
        Me.gpEMail.Controls.Add(Me.chHTML)
        Me.gpEMail.Controls.Add(Me.Label2)
        Me.gpEMail.Controls.Add(Me.chServer)
        Me.gpEMail.Controls.Add(Me.lbTage)
        Me.gpEMail.Controls.Add(Me.lbPWort)
        Me.gpEMail.Controls.Add(Me.lbUName)
        Me.gpEMail.Controls.Add(Me.lbName)
        Me.gpEMail.Controls.Add(Me.lbEMail)
        Me.gpEMail.Controls.Add(Me.Label15)
        Me.gpEMail.Controls.Add(Me.Label11)
        Me.gpEMail.Controls.Add(Me.Label10)
        Me.gpEMail.Controls.Add(Me.lbLableUser)
        Me.gpEMail.Controls.Add(Me.lbLablePWort)
        Me.gpEMail.Controls.Add(Me.Label7)
        Me.gpEMail.Controls.Add(Me.tbSMTP)
        Me.gpEMail.Controls.Add(Me.lbSMTP)
        Me.gpEMail.Controls.Add(Me.Label6)
        Me.gpEMail.Location = New System.Drawing.Point(22, 15)
        Me.gpEMail.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gpEMail.Name = "gpEMail"
        Me.gpEMail.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gpEMail.Size = New System.Drawing.Size(1401, 595)
        Me.gpEMail.TabIndex = 0
        Me.gpEMail.TabStop = False
        '
        'cmdTestEMail
        '
        Me.cmdTestEMail.Location = New System.Drawing.Point(16, 471)
        Me.cmdTestEMail.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmdTestEMail.Name = "cmdTestEMail"
        Me.cmdTestEMail.Size = New System.Drawing.Size(112, 35)
        Me.cmdTestEMail.TabIndex = 112
        Me.cmdTestEMail.Text = "Test EMail"
        Me.cmdTestEMail.UseVisualStyleBackColor = True
        '
        'tbTage
        '
        Me.tbTage.ForeColor = System.Drawing.Color.Blue
        Me.tbTage.Location = New System.Drawing.Point(114, 288)
        Me.tbTage.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTage.Name = "tbTage"
        Me.tbTage.Size = New System.Drawing.Size(48, 26)
        Me.tbTage.TabIndex = 22
        Me.tbTage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.tbTage.Visible = False
        '
        'tbPWort
        '
        Me.tbPWort.ForeColor = System.Drawing.Color.Blue
        Me.tbPWort.Location = New System.Drawing.Point(112, 215)
        Me.tbPWort.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbPWort.Name = "tbPWort"
        Me.tbPWort.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.tbPWort.Size = New System.Drawing.Size(132, 26)
        Me.tbPWort.TabIndex = 21
        Me.tbPWort.UseSystemPasswordChar = True
        Me.tbPWort.Visible = False
        '
        'tbUName
        '
        Me.tbUName.ForeColor = System.Drawing.Color.Blue
        Me.tbUName.Location = New System.Drawing.Point(112, 183)
        Me.tbUName.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbUName.Name = "tbUName"
        Me.tbUName.Size = New System.Drawing.Size(132, 26)
        Me.tbUName.TabIndex = 20
        Me.tbUName.Visible = False
        '
        'tbName
        '
        Me.tbName.ForeColor = System.Drawing.Color.Blue
        Me.tbName.Location = New System.Drawing.Point(176, 98)
        Me.tbName.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(252, 26)
        Me.tbName.TabIndex = 19
        Me.tbName.Visible = False
        '
        'tbEMail
        '
        Me.tbEMail.ForeColor = System.Drawing.Color.Blue
        Me.tbEMail.Location = New System.Drawing.Point(176, 62)
        Me.tbEMail.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbEMail.Name = "tbEMail"
        Me.tbEMail.Size = New System.Drawing.Size(252, 26)
        Me.tbEMail.TabIndex = 18
        Me.tbEMail.Visible = False
        '
        'rbUU
        '
        Me.rbUU.AutoSize = True
        Me.rbUU.Enabled = False
        Me.rbUU.Location = New System.Drawing.Point(280, 215)
        Me.rbUU.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.rbUU.Name = "rbUU"
        Me.rbUU.Size = New System.Drawing.Size(113, 24)
        Me.rbUU.TabIndex = 101
        Me.rbUU.Text = "UUEncode"
        Me.rbUU.UseVisualStyleBackColor = True
        '
        'rbMIME
        '
        Me.rbMIME.AutoSize = True
        Me.rbMIME.Checked = True
        Me.rbMIME.Enabled = False
        Me.rbMIME.Location = New System.Drawing.Point(280, 185)
        Me.rbMIME.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.rbMIME.Name = "rbMIME"
        Me.rbMIME.Size = New System.Drawing.Size(76, 24)
        Me.rbMIME.TabIndex = 100
        Me.rbMIME.TabStop = True
        Me.rbMIME.Text = "MIME"
        Me.rbMIME.UseVisualStyleBackColor = True
        '
        'chHTML
        '
        Me.chHTML.Enabled = False
        Me.chHTML.Location = New System.Drawing.Point(280, 246)
        Me.chHTML.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chHTML.Name = "chHTML"
        Me.chHTML.Size = New System.Drawing.Size(148, 37)
        Me.chHTML.TabIndex = 99
        Me.chHTML.Text = "HTML"
        Me.chHTML.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(276, 137)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(153, 31)
        Me.Label2.TabIndex = 98
        Me.Label2.Text = "Kodierung"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chServer
        '
        Me.chServer.Checked = True
        Me.chServer.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chServer.Enabled = False
        Me.chServer.Location = New System.Drawing.Point(16, 135)
        Me.chServer.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chServer.Name = "chServer"
        Me.chServer.Size = New System.Drawing.Size(230, 37)
        Me.chServer.TabIndex = 97
        Me.chServer.Text = "Am Server anmelden"
        Me.chServer.UseVisualStyleBackColor = True
        '
        'lbTage
        '
        Me.lbTage.BackColor = System.Drawing.SystemColors.Window
        Me.lbTage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbTage.Location = New System.Drawing.Point(114, 288)
        Me.lbTage.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbTage.Name = "lbTage"
        Me.lbTage.Size = New System.Drawing.Size(50, 31)
        Me.lbTage.TabIndex = 55
        Me.lbTage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbPWort
        '
        Me.lbPWort.BackColor = System.Drawing.SystemColors.Window
        Me.lbPWort.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbPWort.Location = New System.Drawing.Point(114, 215)
        Me.lbPWort.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbPWort.Name = "lbPWort"
        Me.lbPWort.Size = New System.Drawing.Size(132, 31)
        Me.lbPWort.TabIndex = 54
        Me.lbPWort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbUName
        '
        Me.lbUName.BackColor = System.Drawing.SystemColors.Window
        Me.lbUName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbUName.Location = New System.Drawing.Point(114, 185)
        Me.lbUName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbUName.Name = "lbUName"
        Me.lbUName.Size = New System.Drawing.Size(132, 31)
        Me.lbUName.TabIndex = 53
        Me.lbUName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbName
        '
        Me.lbName.BackColor = System.Drawing.SystemColors.Window
        Me.lbName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbName.Location = New System.Drawing.Point(176, 100)
        Me.lbName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbName.Name = "lbName"
        Me.lbName.Size = New System.Drawing.Size(254, 31)
        Me.lbName.TabIndex = 52
        Me.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbEMail
        '
        Me.lbEMail.BackColor = System.Drawing.SystemColors.Window
        Me.lbEMail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbEMail.Location = New System.Drawing.Point(176, 63)
        Me.lbEMail.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbEMail.Name = "lbEMail"
        Me.lbEMail.Size = New System.Drawing.Size(254, 31)
        Me.lbEMail.TabIndex = 51
        Me.lbEMail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.Color.Transparent
        Me.Label15.Location = New System.Drawing.Point(171, 288)
        Me.Label15.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(258, 31)
        Me.Label15.TabIndex = 50
        Me.Label15.Text = "Tagen löschen"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Location = New System.Drawing.Point(12, 62)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(294, 31)
        Me.Label11.TabIndex = 46
        Me.Label11.Text = "Ihre E-Mail Adresse"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Location = New System.Drawing.Point(12, 94)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(294, 31)
        Me.Label10.TabIndex = 45
        Me.Label10.Text = "Ihr Name"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbLableUser
        '
        Me.lbLableUser.BackColor = System.Drawing.Color.Transparent
        Me.lbLableUser.Location = New System.Drawing.Point(12, 185)
        Me.lbLableUser.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbLableUser.Name = "lbLableUser"
        Me.lbLableUser.Size = New System.Drawing.Size(106, 31)
        Me.lbLableUser.TabIndex = 44
        Me.lbLableUser.Text = "Username"
        Me.lbLableUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbLablePWort
        '
        Me.lbLablePWort.BackColor = System.Drawing.Color.Transparent
        Me.lbLablePWort.Location = New System.Drawing.Point(12, 215)
        Me.lbLablePWort.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbLablePWort.Name = "lbLablePWort"
        Me.lbLablePWort.Size = New System.Drawing.Size(294, 31)
        Me.lbLablePWort.TabIndex = 43
        Me.lbLablePWort.Text = "Passwort"
        Me.lbLablePWort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Location = New System.Drawing.Point(12, 288)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(106, 31)
        Me.Label7.TabIndex = 42
        Me.Label7.Text = "Mails nach"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSMTP
        '
        Me.tbSMTP.ForeColor = System.Drawing.Color.Blue
        Me.tbSMTP.Location = New System.Drawing.Point(176, 26)
        Me.tbSMTP.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbSMTP.Name = "tbSMTP"
        Me.tbSMTP.Size = New System.Drawing.Size(252, 26)
        Me.tbSMTP.TabIndex = 17
        Me.tbSMTP.Visible = False
        '
        'lbSMTP
        '
        Me.lbSMTP.BackColor = System.Drawing.SystemColors.Window
        Me.lbSMTP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbSMTP.Location = New System.Drawing.Point(176, 26)
        Me.lbSMTP.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbSMTP.Name = "lbSMTP"
        Me.lbSMTP.Size = New System.Drawing.Size(254, 31)
        Me.lbSMTP.TabIndex = 41
        Me.lbSMTP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(12, 29)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(294, 31)
        Me.Label6.TabIndex = 40
        Me.Label6.Text = "SMTP-Server"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tpAnschrift
        '
        Me.tpAnschrift.BackColor = System.Drawing.Color.Transparent
        Me.tpAnschrift.Controls.Add(Me.gbBasisdaten)
        Me.tpAnschrift.Location = New System.Drawing.Point(4, 29)
        Me.tpAnschrift.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpAnschrift.Name = "tpAnschrift"
        Me.tpAnschrift.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpAnschrift.Size = New System.Drawing.Size(1441, 879)
        Me.tpAnschrift.TabIndex = 0
        Me.tpAnschrift.Text = "Basisdaten"
        Me.tpAnschrift.UseVisualStyleBackColor = True
        '
        'gbBasisdaten
        '
        Me.gbBasisdaten.Controls.Add(Me.gpDir)
        Me.gbBasisdaten.Controls.Add(Me.Label58)
        Me.gbBasisdaten.Controls.Add(Me.tbWeb)
        Me.gbBasisdaten.Controls.Add(Me.lbWeb)
        Me.gbBasisdaten.Controls.Add(Me.gpDiverses)
        Me.gbBasisdaten.Controls.Add(Me.tbTel4)
        Me.gbBasisdaten.Controls.Add(Me.lbTel4)
        Me.gbBasisdaten.Controls.Add(Me.tbTel3)
        Me.gbBasisdaten.Controls.Add(Me.lbTel3)
        Me.gbBasisdaten.Controls.Add(Me.tbTel2)
        Me.gbBasisdaten.Controls.Add(Me.lbTel2)
        Me.gbBasisdaten.Controls.Add(Me.tbTel1)
        Me.gbBasisdaten.Controls.Add(Me.lbTel1)
        Me.gbBasisdaten.Controls.Add(Me.Label9)
        Me.gbBasisdaten.Controls.Add(Me.tbOrt)
        Me.gbBasisdaten.Controls.Add(Me.lbOrt)
        Me.gbBasisdaten.Controls.Add(Me.tbPLZ)
        Me.gbBasisdaten.Controls.Add(Me.lbPLZ)
        Me.gbBasisdaten.Controls.Add(Me.tbStrasse)
        Me.gbBasisdaten.Controls.Add(Me.lbStrasse)
        Me.gbBasisdaten.Controls.Add(Me.tbFirma)
        Me.gbBasisdaten.Controls.Add(Me.lbFirma)
        Me.gbBasisdaten.Controls.Add(Me.lbLabelOrt)
        Me.gbBasisdaten.Controls.Add(Me.lbLabelPLZ)
        Me.gbBasisdaten.Controls.Add(Me.lbLabelStr)
        Me.gbBasisdaten.Controls.Add(Me.lbLabelGebName)
        Me.gbBasisdaten.Location = New System.Drawing.Point(22, 28)
        Me.gbBasisdaten.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbBasisdaten.Name = "gbBasisdaten"
        Me.gbBasisdaten.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbBasisdaten.Size = New System.Drawing.Size(1401, 496)
        Me.gbBasisdaten.TabIndex = 36
        Me.gbBasisdaten.TabStop = False
        Me.gbBasisdaten.Text = "Anschrift / Diverses"
        '
        'gpDir
        '
        Me.gpDir.Controls.Add(Me.Label17)
        Me.gpDir.Controls.Add(Me.tbDatevDir)
        Me.gpDir.Controls.Add(Me.lbDatevDir)
        Me.gpDir.Controls.Add(Me.tbMwst2)
        Me.gpDir.Controls.Add(Me.lbMwst2)
        Me.gpDir.Controls.Add(Me.cmdDatevDir)
        Me.gpDir.Controls.Add(Me.Label23)
        Me.gpDir.Controls.Add(Me.tbSaveDir)
        Me.gpDir.Controls.Add(Me.lbSaveDir)
        Me.gpDir.Controls.Add(Me.tbAblageDir)
        Me.gpDir.Controls.Add(Me.lbAblageDir)
        Me.gpDir.Controls.Add(Me.cmdSaveDir)
        Me.gpDir.Controls.Add(Me.Label1)
        Me.gpDir.Controls.Add(Me.cmdAblageDir)
        Me.gpDir.Controls.Add(Me.Label5)
        Me.gpDir.Location = New System.Drawing.Point(0, 226)
        Me.gpDir.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gpDir.Name = "gpDir"
        Me.gpDir.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gpDir.Size = New System.Drawing.Size(791, 274)
        Me.gpDir.TabIndex = 39
        Me.gpDir.TabStop = False
        Me.gpDir.Text = "Verzeichnisse"
        '
        'Label17
        '
        Me.Label17.BackColor = System.Drawing.Color.Transparent
        Me.Label17.Location = New System.Drawing.Point(19, 219)
        Me.Label17.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(160, 31)
        Me.Label17.TabIndex = 74
        Me.Label17.Text = "Mwst Übernachtung"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbDatevDir
        '
        Me.tbDatevDir.ForeColor = System.Drawing.Color.Blue
        Me.tbDatevDir.Location = New System.Drawing.Point(164, 48)
        Me.tbDatevDir.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbDatevDir.Name = "tbDatevDir"
        Me.tbDatevDir.Size = New System.Drawing.Size(484, 26)
        Me.tbDatevDir.TabIndex = 81
        Me.tbDatevDir.Visible = False
        '
        'lbDatevDir
        '
        Me.lbDatevDir.BackColor = System.Drawing.SystemColors.Window
        Me.lbDatevDir.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbDatevDir.Location = New System.Drawing.Point(164, 48)
        Me.lbDatevDir.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbDatevDir.Name = "lbDatevDir"
        Me.lbDatevDir.Size = New System.Drawing.Size(486, 31)
        Me.lbDatevDir.TabIndex = 84
        Me.lbDatevDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbMwst2
        '
        Me.tbMwst2.ForeColor = System.Drawing.Color.Blue
        Me.tbMwst2.Location = New System.Drawing.Point(188, 224)
        Me.tbMwst2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbMwst2.Name = "tbMwst2"
        Me.tbMwst2.Size = New System.Drawing.Size(62, 26)
        Me.tbMwst2.TabIndex = 76
        Me.tbMwst2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.tbMwst2.Visible = False
        '
        'lbMwst2
        '
        Me.lbMwst2.BackColor = System.Drawing.SystemColors.Window
        Me.lbMwst2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbMwst2.Location = New System.Drawing.Point(188, 222)
        Me.lbMwst2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbMwst2.Name = "lbMwst2"
        Me.lbMwst2.Size = New System.Drawing.Size(64, 32)
        Me.lbMwst2.TabIndex = 78
        Me.lbMwst2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmdDatevDir
        '
        Me.cmdDatevDir.Enabled = False
        Me.cmdDatevDir.Location = New System.Drawing.Point(669, 45)
        Me.cmdDatevDir.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmdDatevDir.Name = "cmdDatevDir"
        Me.cmdDatevDir.Size = New System.Drawing.Size(75, 31)
        Me.cmdDatevDir.TabIndex = 83
        Me.cmdDatevDir.Text = "..."
        Me.cmdDatevDir.UseVisualStyleBackColor = True
        '
        'Label23
        '
        Me.Label23.BackColor = System.Drawing.Color.Transparent
        Me.Label23.Location = New System.Drawing.Point(14, 49)
        Me.Label23.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(132, 31)
        Me.Label23.TabIndex = 78
        Me.Label23.Text = "Rechnung"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSaveDir
        '
        Me.tbSaveDir.ForeColor = System.Drawing.Color.Blue
        Me.tbSaveDir.Location = New System.Drawing.Point(164, 130)
        Me.tbSaveDir.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbSaveDir.Name = "tbSaveDir"
        Me.tbSaveDir.Size = New System.Drawing.Size(484, 26)
        Me.tbSaveDir.TabIndex = 73
        Me.tbSaveDir.Visible = False
        '
        'lbSaveDir
        '
        Me.lbSaveDir.BackColor = System.Drawing.SystemColors.Window
        Me.lbSaveDir.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbSaveDir.Location = New System.Drawing.Point(164, 128)
        Me.lbSaveDir.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbSaveDir.Name = "lbSaveDir"
        Me.lbSaveDir.Size = New System.Drawing.Size(486, 32)
        Me.lbSaveDir.TabIndex = 76
        Me.lbSaveDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbAblageDir
        '
        Me.tbAblageDir.ForeColor = System.Drawing.Color.Blue
        Me.tbAblageDir.Location = New System.Drawing.Point(164, 89)
        Me.tbAblageDir.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbAblageDir.Name = "tbAblageDir"
        Me.tbAblageDir.Size = New System.Drawing.Size(484, 26)
        Me.tbAblageDir.TabIndex = 10
        Me.tbAblageDir.Visible = False
        '
        'lbAblageDir
        '
        Me.lbAblageDir.BackColor = System.Drawing.SystemColors.Window
        Me.lbAblageDir.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbAblageDir.Location = New System.Drawing.Point(164, 89)
        Me.lbAblageDir.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbAblageDir.Name = "lbAblageDir"
        Me.lbAblageDir.Size = New System.Drawing.Size(486, 32)
        Me.lbAblageDir.TabIndex = 66
        Me.lbAblageDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdSaveDir
        '
        Me.cmdSaveDir.Enabled = False
        Me.cmdSaveDir.Location = New System.Drawing.Point(669, 128)
        Me.cmdSaveDir.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmdSaveDir.Name = "cmdSaveDir"
        Me.cmdSaveDir.Size = New System.Drawing.Size(75, 31)
        Me.cmdSaveDir.TabIndex = 75
        Me.cmdSaveDir.Text = "..."
        Me.cmdSaveDir.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(14, 129)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(160, 31)
        Me.Label1.TabIndex = 74
        Me.Label1.Text = "Türschloß"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdAblageDir
        '
        Me.cmdAblageDir.Enabled = False
        Me.cmdAblageDir.Location = New System.Drawing.Point(669, 87)
        Me.cmdAblageDir.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmdAblageDir.Name = "cmdAblageDir"
        Me.cmdAblageDir.Size = New System.Drawing.Size(75, 31)
        Me.cmdAblageDir.TabIndex = 71
        Me.cmdAblageDir.Text = "..."
        Me.cmdAblageDir.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(14, 89)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(160, 31)
        Me.Label5.TabIndex = 70
        Me.Label5.Text = "Ablage"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label58
        '
        Me.Label58.BackColor = System.Drawing.Color.Transparent
        Me.Label58.Location = New System.Drawing.Point(14, 166)
        Me.Label58.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(141, 31)
        Me.Label58.TabIndex = 95
        Me.Label58.Text = "Internet Adresse"
        Me.Label58.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbWeb
        '
        Me.tbWeb.ForeColor = System.Drawing.Color.Blue
        Me.tbWeb.Location = New System.Drawing.Point(163, 166)
        Me.tbWeb.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbWeb.Name = "tbWeb"
        Me.tbWeb.Size = New System.Drawing.Size(244, 26)
        Me.tbWeb.TabIndex = 93
        Me.tbWeb.Visible = False
        '
        'lbWeb
        '
        Me.lbWeb.BackColor = System.Drawing.SystemColors.Window
        Me.lbWeb.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbWeb.Location = New System.Drawing.Point(164, 165)
        Me.lbWeb.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbWeb.Name = "lbWeb"
        Me.lbWeb.Size = New System.Drawing.Size(246, 31)
        Me.lbWeb.TabIndex = 94
        Me.lbWeb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gpDiverses
        '
        Me.gpDiverses.Controls.Add(Me.lbRFIDPort)
        Me.gpDiverses.Controls.Add(Me.tbRFIDPort)
        Me.gpDiverses.Controls.Add(Me.tbRNr)
        Me.gpDiverses.Controls.Add(Me.Label67)
        Me.gpDiverses.Controls.Add(Me.lbIPSchloss)
        Me.gpDiverses.Controls.Add(Me.lbRNr)
        Me.gpDiverses.Controls.Add(Me.tbIPSchloss)
        Me.gpDiverses.Controls.Add(Me.lbNetUser)
        Me.gpDiverses.Controls.Add(Me.tbBK)
        Me.gpDiverses.Controls.Add(Me.lbBK)
        Me.gpDiverses.Controls.Add(Me.tbNetUser)
        Me.gpDiverses.Controls.Add(Me.Label59)
        Me.gpDiverses.Controls.Add(Me.tbKK)
        Me.gpDiverses.Controls.Add(Me.Label66)
        Me.gpDiverses.Controls.Add(Me.lbKK)
        Me.gpDiverses.Controls.Add(Me.Label65)
        Me.gpDiverses.Controls.Add(Me.Label3)
        Me.gpDiverses.Controls.Add(Me.tbUStID)
        Me.gpDiverses.Controls.Add(Me.lbUStID)
        Me.gpDiverses.Controls.Add(Me.tbUStNr)
        Me.gpDiverses.Controls.Add(Me.lbUStNr)
        Me.gpDiverses.Controls.Add(Me.Label31)
        Me.gpDiverses.Controls.Add(Me.Label32)
        Me.gpDiverses.Controls.Add(Me.Label44)
        Me.gpDiverses.Controls.Add(Me.lbBNr)
        Me.gpDiverses.Controls.Add(Me.Label27)
        Me.gpDiverses.Controls.Add(Me.lbKNr)
        Me.gpDiverses.Controls.Add(Me.Label25)
        Me.gpDiverses.Location = New System.Drawing.Point(790, 0)
        Me.gpDiverses.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gpDiverses.Name = "gpDiverses"
        Me.gpDiverses.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gpDiverses.Size = New System.Drawing.Size(611, 500)
        Me.gpDiverses.TabIndex = 38
        Me.gpDiverses.TabStop = False
        '
        'lbRFIDPort
        '
        Me.lbRFIDPort.BackColor = System.Drawing.Color.White
        Me.lbRFIDPort.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbRFIDPort.Location = New System.Drawing.Point(134, 448)
        Me.lbRFIDPort.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbRFIDPort.Name = "lbRFIDPort"
        Me.lbRFIDPort.Size = New System.Drawing.Size(63, 31)
        Me.lbRFIDPort.TabIndex = 93
        Me.lbRFIDPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbRFIDPort
        '
        Me.tbRFIDPort.ForeColor = System.Drawing.Color.Blue
        Me.tbRFIDPort.Location = New System.Drawing.Point(134, 448)
        Me.tbRFIDPort.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbRFIDPort.Name = "tbRFIDPort"
        Me.tbRFIDPort.Size = New System.Drawing.Size(61, 26)
        Me.tbRFIDPort.TabIndex = 90
        Me.tbRFIDPort.Visible = False
        '
        'tbRNr
        '
        Me.tbRNr.ForeColor = System.Drawing.Color.Blue
        Me.tbRNr.Location = New System.Drawing.Point(132, 123)
        Me.tbRNr.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbRNr.Name = "tbRNr"
        Me.tbRNr.Size = New System.Drawing.Size(62, 26)
        Me.tbRNr.TabIndex = 96
        Me.tbRNr.Visible = False
        '
        'Label67
        '
        Me.Label67.AutoSize = True
        Me.Label67.Location = New System.Drawing.Point(9, 449)
        Me.Label67.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label67.Name = "Label67"
        Me.Label67.Size = New System.Drawing.Size(81, 20)
        Me.Label67.TabIndex = 87
        Me.Label67.Text = "RFID Port"
        '
        'lbIPSchloss
        '
        Me.lbIPSchloss.BackColor = System.Drawing.Color.White
        Me.lbIPSchloss.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbIPSchloss.Location = New System.Drawing.Point(132, 403)
        Me.lbIPSchloss.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbIPSchloss.Name = "lbIPSchloss"
        Me.lbIPSchloss.Size = New System.Drawing.Size(214, 31)
        Me.lbIPSchloss.TabIndex = 92
        Me.lbIPSchloss.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbRNr
        '
        Me.lbRNr.Location = New System.Drawing.Point(132, 126)
        Me.lbRNr.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbRNr.Name = "lbRNr"
        Me.lbRNr.Size = New System.Drawing.Size(64, 32)
        Me.lbRNr.TabIndex = 96
        Me.lbRNr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbIPSchloss
        '
        Me.tbIPSchloss.ForeColor = System.Drawing.Color.Blue
        Me.tbIPSchloss.Location = New System.Drawing.Point(132, 403)
        Me.tbIPSchloss.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbIPSchloss.Name = "tbIPSchloss"
        Me.tbIPSchloss.Size = New System.Drawing.Size(212, 26)
        Me.tbIPSchloss.TabIndex = 89
        Me.tbIPSchloss.Visible = False
        '
        'lbNetUser
        '
        Me.lbNetUser.BackColor = System.Drawing.Color.White
        Me.lbNetUser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbNetUser.Location = New System.Drawing.Point(132, 357)
        Me.lbNetUser.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbNetUser.Name = "lbNetUser"
        Me.lbNetUser.Size = New System.Drawing.Size(220, 31)
        Me.lbNetUser.TabIndex = 91
        Me.lbNetUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbBK
        '
        Me.tbBK.ForeColor = System.Drawing.Color.Blue
        Me.tbBK.Location = New System.Drawing.Point(132, 300)
        Me.tbBK.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbBK.Name = "tbBK"
        Me.tbBK.Size = New System.Drawing.Size(62, 26)
        Me.tbBK.TabIndex = 101
        Me.tbBK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.tbBK.Visible = False
        '
        'lbBK
        '
        Me.lbBK.BackColor = System.Drawing.SystemColors.Window
        Me.lbBK.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbBK.Location = New System.Drawing.Point(132, 300)
        Me.lbBK.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbBK.Name = "lbBK"
        Me.lbBK.Size = New System.Drawing.Size(64, 32)
        Me.lbBK.TabIndex = 102
        Me.lbBK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbNetUser
        '
        Me.tbNetUser.ForeColor = System.Drawing.Color.Blue
        Me.tbNetUser.Location = New System.Drawing.Point(132, 358)
        Me.tbNetUser.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbNetUser.Name = "tbNetUser"
        Me.tbNetUser.Size = New System.Drawing.Size(218, 26)
        Me.tbNetUser.TabIndex = 88
        Me.tbNetUser.Visible = False
        '
        'Label59
        '
        Me.Label59.BackColor = System.Drawing.Color.Transparent
        Me.Label59.Location = New System.Drawing.Point(10, 300)
        Me.Label59.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(112, 31)
        Me.Label59.TabIndex = 100
        Me.Label59.Text = "Bank Konto"
        Me.Label59.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbKK
        '
        Me.tbKK.ForeColor = System.Drawing.Color.Blue
        Me.tbKK.Location = New System.Drawing.Point(132, 260)
        Me.tbKK.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbKK.Name = "tbKK"
        Me.tbKK.Size = New System.Drawing.Size(62, 26)
        Me.tbKK.TabIndex = 98
        Me.tbKK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.tbKK.Visible = False
        '
        'Label66
        '
        Me.Label66.AutoSize = True
        Me.Label66.Location = New System.Drawing.Point(6, 408)
        Me.Label66.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label66.Name = "Label66"
        Me.Label66.Size = New System.Drawing.Size(78, 20)
        Me.Label66.TabIndex = 86
        Me.Label66.Text = "IP Schloß"
        '
        'lbKK
        '
        Me.lbKK.BackColor = System.Drawing.SystemColors.Window
        Me.lbKK.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbKK.Location = New System.Drawing.Point(132, 260)
        Me.lbKK.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbKK.Name = "lbKK"
        Me.lbKK.Size = New System.Drawing.Size(64, 32)
        Me.lbKK.TabIndex = 99
        Me.lbKK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label65
        '
        Me.Label65.AutoSize = True
        Me.Label65.Location = New System.Drawing.Point(6, 363)
        Me.Label65.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label65.Name = "Label65"
        Me.Label65.Size = New System.Drawing.Size(68, 20)
        Me.Label65.TabIndex = 85
        Me.Label65.Text = "NetUser"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(10, 260)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(128, 31)
        Me.Label3.TabIndex = 97
        Me.Label3.Text = "Kasse Konto"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbUStID
        '
        Me.tbUStID.ForeColor = System.Drawing.Color.Blue
        Me.tbUStID.Location = New System.Drawing.Point(132, 203)
        Me.tbUStID.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbUStID.Name = "tbUStID"
        Me.tbUStID.Size = New System.Drawing.Size(288, 26)
        Me.tbUStID.TabIndex = 90
        Me.tbUStID.Visible = False
        '
        'lbUStID
        '
        Me.lbUStID.BackColor = System.Drawing.SystemColors.Window
        Me.lbUStID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbUStID.Location = New System.Drawing.Point(132, 203)
        Me.lbUStID.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbUStID.Name = "lbUStID"
        Me.lbUStID.Size = New System.Drawing.Size(290, 32)
        Me.lbUStID.TabIndex = 93
        Me.lbUStID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbUStNr
        '
        Me.tbUStNr.ForeColor = System.Drawing.Color.Blue
        Me.tbUStNr.Location = New System.Drawing.Point(132, 163)
        Me.tbUStNr.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbUStNr.Name = "tbUStNr"
        Me.tbUStNr.Size = New System.Drawing.Size(288, 26)
        Me.tbUStNr.TabIndex = 89
        Me.tbUStNr.Visible = False
        '
        'lbUStNr
        '
        Me.lbUStNr.BackColor = System.Drawing.SystemColors.Window
        Me.lbUStNr.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbUStNr.Location = New System.Drawing.Point(132, 165)
        Me.lbUStNr.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbUStNr.Name = "lbUStNr"
        Me.lbUStNr.Size = New System.Drawing.Size(290, 32)
        Me.lbUStNr.TabIndex = 92
        Me.lbUStNr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label31
        '
        Me.Label31.BackColor = System.Drawing.Color.Transparent
        Me.Label31.Location = New System.Drawing.Point(10, 205)
        Me.Label31.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(70, 31)
        Me.Label31.TabIndex = 91
        Me.Label31.Text = "USt- ID"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label32
        '
        Me.Label32.BackColor = System.Drawing.Color.Transparent
        Me.Label32.Location = New System.Drawing.Point(10, 163)
        Me.Label32.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(70, 31)
        Me.Label32.TabIndex = 88
        Me.Label32.Text = "USt- Nr"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label44
        '
        Me.Label44.BackColor = System.Drawing.Color.Transparent
        Me.Label44.Location = New System.Drawing.Point(6, 122)
        Me.Label44.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(122, 31)
        Me.Label44.TabIndex = 86
        Me.Label44.Text = "Rechnungs- Nr"
        Me.Label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbBNr
        '
        Me.lbBNr.BackColor = System.Drawing.SystemColors.Window
        Me.lbBNr.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbBNr.Location = New System.Drawing.Point(132, 83)
        Me.lbBNr.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbBNr.Name = "lbBNr"
        Me.lbBNr.Size = New System.Drawing.Size(64, 32)
        Me.lbBNr.TabIndex = 84
        Me.lbBNr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label27
        '
        Me.Label27.BackColor = System.Drawing.Color.Transparent
        Me.Label27.Location = New System.Drawing.Point(6, 83)
        Me.Label27.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(117, 31)
        Me.Label27.TabIndex = 83
        Me.Label27.Text = "Buchungs-Nr."
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbKNr
        '
        Me.lbKNr.BackColor = System.Drawing.SystemColors.Window
        Me.lbKNr.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbKNr.Location = New System.Drawing.Point(130, 42)
        Me.lbKNr.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbKNr.Name = "lbKNr"
        Me.lbKNr.Size = New System.Drawing.Size(64, 32)
        Me.lbKNr.TabIndex = 81
        Me.lbKNr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label25
        '
        Me.Label25.BackColor = System.Drawing.Color.Transparent
        Me.Label25.Location = New System.Drawing.Point(6, 42)
        Me.Label25.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(122, 31)
        Me.Label25.TabIndex = 80
        Me.Label25.Text = "Kunden- Nr."
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTel4
        '
        Me.tbTel4.ForeColor = System.Drawing.Color.Blue
        Me.tbTel4.Location = New System.Drawing.Point(506, 163)
        Me.tbTel4.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTel4.Name = "tbTel4"
        Me.tbTel4.Size = New System.Drawing.Size(244, 26)
        Me.tbTel4.TabIndex = 91
        Me.tbTel4.Visible = False
        '
        'lbTel4
        '
        Me.lbTel4.BackColor = System.Drawing.SystemColors.Window
        Me.lbTel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbTel4.Location = New System.Drawing.Point(506, 163)
        Me.lbTel4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbTel4.Name = "lbTel4"
        Me.lbTel4.Size = New System.Drawing.Size(246, 31)
        Me.lbTel4.TabIndex = 92
        Me.lbTel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTel3
        '
        Me.tbTel3.ForeColor = System.Drawing.Color.Blue
        Me.tbTel3.Location = New System.Drawing.Point(506, 123)
        Me.tbTel3.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTel3.Name = "tbTel3"
        Me.tbTel3.Size = New System.Drawing.Size(244, 26)
        Me.tbTel3.TabIndex = 89
        Me.tbTel3.Visible = False
        '
        'lbTel3
        '
        Me.lbTel3.BackColor = System.Drawing.SystemColors.Window
        Me.lbTel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbTel3.Location = New System.Drawing.Point(506, 123)
        Me.lbTel3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbTel3.Name = "lbTel3"
        Me.lbTel3.Size = New System.Drawing.Size(246, 31)
        Me.lbTel3.TabIndex = 90
        Me.lbTel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTel2
        '
        Me.tbTel2.ForeColor = System.Drawing.Color.Blue
        Me.tbTel2.Location = New System.Drawing.Point(506, 82)
        Me.tbTel2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTel2.Name = "tbTel2"
        Me.tbTel2.Size = New System.Drawing.Size(244, 26)
        Me.tbTel2.TabIndex = 87
        Me.tbTel2.Visible = False
        '
        'lbTel2
        '
        Me.lbTel2.BackColor = System.Drawing.SystemColors.Window
        Me.lbTel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbTel2.Location = New System.Drawing.Point(506, 83)
        Me.lbTel2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbTel2.Name = "lbTel2"
        Me.lbTel2.Size = New System.Drawing.Size(246, 31)
        Me.lbTel2.TabIndex = 88
        Me.lbTel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTel1
        '
        Me.tbTel1.ForeColor = System.Drawing.Color.Blue
        Me.tbTel1.Location = New System.Drawing.Point(506, 42)
        Me.tbTel1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTel1.Name = "tbTel1"
        Me.tbTel1.Size = New System.Drawing.Size(244, 26)
        Me.tbTel1.TabIndex = 85
        Me.tbTel1.Visible = False
        '
        'lbTel1
        '
        Me.lbTel1.BackColor = System.Drawing.SystemColors.Window
        Me.lbTel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbTel1.Location = New System.Drawing.Point(506, 43)
        Me.lbTel1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbTel1.Name = "lbTel1"
        Me.lbTel1.Size = New System.Drawing.Size(246, 31)
        Me.lbTel1.TabIndex = 86
        Me.lbTel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Location = New System.Drawing.Point(418, 42)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(96, 31)
        Me.Label9.TabIndex = 84
        Me.Label9.Text = "Telefon"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbOrt
        '
        Me.tbOrt.ForeColor = System.Drawing.Color.Blue
        Me.tbOrt.Location = New System.Drawing.Point(244, 114)
        Me.tbOrt.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbOrt.Name = "tbOrt"
        Me.tbOrt.Size = New System.Drawing.Size(163, 26)
        Me.tbOrt.TabIndex = 4
        Me.tbOrt.Visible = False
        '
        'lbOrt
        '
        Me.lbOrt.BackColor = System.Drawing.SystemColors.Window
        Me.lbOrt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbOrt.Location = New System.Drawing.Point(244, 114)
        Me.lbOrt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbOrt.Name = "lbOrt"
        Me.lbOrt.Size = New System.Drawing.Size(165, 32)
        Me.lbOrt.TabIndex = 74
        Me.lbOrt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbPLZ
        '
        Me.tbPLZ.ForeColor = System.Drawing.Color.Blue
        Me.tbPLZ.Location = New System.Drawing.Point(164, 114)
        Me.tbPLZ.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbPLZ.Name = "tbPLZ"
        Me.tbPLZ.Size = New System.Drawing.Size(62, 26)
        Me.tbPLZ.TabIndex = 3
        Me.tbPLZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.tbPLZ.Visible = False
        '
        'lbPLZ
        '
        Me.lbPLZ.BackColor = System.Drawing.SystemColors.Window
        Me.lbPLZ.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbPLZ.Location = New System.Drawing.Point(164, 114)
        Me.lbPLZ.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbPLZ.Name = "lbPLZ"
        Me.lbPLZ.Size = New System.Drawing.Size(64, 32)
        Me.lbPLZ.TabIndex = 72
        Me.lbPLZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbStrasse
        '
        Me.tbStrasse.ForeColor = System.Drawing.Color.Blue
        Me.tbStrasse.Location = New System.Drawing.Point(164, 77)
        Me.tbStrasse.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbStrasse.Name = "tbStrasse"
        Me.tbStrasse.Size = New System.Drawing.Size(244, 26)
        Me.tbStrasse.TabIndex = 2
        Me.tbStrasse.Visible = False
        '
        'lbStrasse
        '
        Me.lbStrasse.BackColor = System.Drawing.SystemColors.Window
        Me.lbStrasse.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbStrasse.Location = New System.Drawing.Point(164, 77)
        Me.lbStrasse.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbStrasse.Name = "lbStrasse"
        Me.lbStrasse.Size = New System.Drawing.Size(246, 32)
        Me.lbStrasse.TabIndex = 66
        Me.lbStrasse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbFirma
        '
        Me.tbFirma.ForeColor = System.Drawing.Color.Blue
        Me.tbFirma.Location = New System.Drawing.Point(164, 42)
        Me.tbFirma.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbFirma.Name = "tbFirma"
        Me.tbFirma.Size = New System.Drawing.Size(244, 26)
        Me.tbFirma.TabIndex = 1
        Me.tbFirma.Visible = False
        '
        'lbFirma
        '
        Me.lbFirma.BackColor = System.Drawing.SystemColors.Window
        Me.lbFirma.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbFirma.Location = New System.Drawing.Point(164, 42)
        Me.lbFirma.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbFirma.Name = "lbFirma"
        Me.lbFirma.Size = New System.Drawing.Size(246, 31)
        Me.lbFirma.TabIndex = 64
        Me.lbFirma.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbLabelOrt
        '
        Me.lbLabelOrt.BackColor = System.Drawing.Color.Transparent
        Me.lbLabelOrt.Location = New System.Drawing.Point(84, 114)
        Me.lbLabelOrt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbLabelOrt.Name = "lbLabelOrt"
        Me.lbLabelOrt.Size = New System.Drawing.Size(70, 31)
        Me.lbLabelOrt.TabIndex = 60
        Me.lbLabelOrt.Text = "Ort"
        Me.lbLabelOrt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbLabelPLZ
        '
        Me.lbLabelPLZ.BackColor = System.Drawing.Color.Transparent
        Me.lbLabelPLZ.Location = New System.Drawing.Point(9, 115)
        Me.lbLabelPLZ.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbLabelPLZ.Name = "lbLabelPLZ"
        Me.lbLabelPLZ.Size = New System.Drawing.Size(70, 31)
        Me.lbLabelPLZ.TabIndex = 57
        Me.lbLabelPLZ.Text = "PLZ"
        Me.lbLabelPLZ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbLabelStr
        '
        Me.lbLabelStr.BackColor = System.Drawing.Color.Transparent
        Me.lbLabelStr.Location = New System.Drawing.Point(9, 77)
        Me.lbLabelStr.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbLabelStr.Name = "lbLabelStr"
        Me.lbLabelStr.Size = New System.Drawing.Size(158, 31)
        Me.lbLabelStr.TabIndex = 48
        Me.lbLabelStr.Text = "Strasse"
        Me.lbLabelStr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbLabelGebName
        '
        Me.lbLabelGebName.BackColor = System.Drawing.Color.Transparent
        Me.lbLabelGebName.Location = New System.Drawing.Point(9, 42)
        Me.lbLabelGebName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbLabelGebName.Name = "lbLabelGebName"
        Me.lbLabelGebName.Size = New System.Drawing.Size(158, 31)
        Me.lbLabelGebName.TabIndex = 45
        Me.lbLabelGebName.Text = "Firma"
        Me.lbLabelGebName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tcSystem
        '
        Me.tcSystem.Controls.Add(Me.tpAnschrift)
        Me.tcSystem.Controls.Add(Me.tpEMail)
        Me.tcSystem.Controls.Add(Me.tpKonto)
        Me.tcSystem.Controls.Add(Me.tpObjekte)
        Me.tcSystem.Controls.Add(Me.tpZimmer)
        Me.tcSystem.Controls.Add(Me.tpUser)
        Me.tcSystem.Controls.Add(Me.tpBText)
        Me.tcSystem.Controls.Add(Me.tpPreise)
        Me.tcSystem.Controls.Add(Me.tpSasion)
        Me.tcSystem.Controls.Add(Me.tpFarben)
        Me.tcSystem.Controls.Add(Me.tpDruck)
        Me.tcSystem.Controls.Add(Me.tpSprache)
        Me.tcSystem.Location = New System.Drawing.Point(18, 42)
        Me.tcSystem.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tcSystem.Name = "tcSystem"
        Me.tcSystem.SelectedIndex = 0
        Me.tcSystem.Size = New System.Drawing.Size(1449, 912)
        Me.tcSystem.TabIndex = 34
        '
        'tpKonto
        '
        Me.tpKonto.Controls.Add(Me.paKonto)
        Me.tpKonto.Controls.Add(Me.tsKonto)
        Me.tpKonto.Controls.Add(Me.lvKonto)
        Me.tpKonto.Location = New System.Drawing.Point(4, 29)
        Me.tpKonto.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpKonto.Name = "tpKonto"
        Me.tpKonto.Size = New System.Drawing.Size(1441, 879)
        Me.tpKonto.TabIndex = 3
        Me.tpKonto.Text = "Kontodaten"
        Me.tpKonto.UseVisualStyleBackColor = True
        '
        'paKonto
        '
        Me.paKonto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.paKonto.Controls.Add(Me.lbKontoID)
        Me.paKonto.Controls.Add(Me.tbTyp)
        Me.paKonto.Controls.Add(Me.tbBIC)
        Me.paKonto.Controls.Add(Me.tbKZ)
        Me.paKonto.Controls.Add(Me.Label12)
        Me.paKonto.Controls.Add(Me.Label24)
        Me.paKonto.Controls.Add(Me.Label8)
        Me.paKonto.Controls.Add(Me.Label21)
        Me.paKonto.Controls.Add(Me.tbIBAN)
        Me.paKonto.Controls.Add(Me.tbInstitut)
        Me.paKonto.Controls.Add(Me.tbBLZ)
        Me.paKonto.Controls.Add(Me.tbKTO)
        Me.paKonto.Controls.Add(Me.Label14)
        Me.paKonto.Controls.Add(Me.Label26)
        Me.paKonto.Controls.Add(Me.Label28)
        Me.paKonto.Location = New System.Drawing.Point(22, 437)
        Me.paKonto.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.paKonto.Name = "paKonto"
        Me.paKonto.Size = New System.Drawing.Size(1408, 170)
        Me.paKonto.TabIndex = 89
        '
        'lbKontoID
        '
        Me.lbKontoID.AutoSize = True
        Me.lbKontoID.Location = New System.Drawing.Point(1125, 140)
        Me.lbKontoID.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbKontoID.Name = "lbKontoID"
        Me.lbKontoID.Size = New System.Drawing.Size(80, 20)
        Me.lbKontoID.TabIndex = 92
        Me.lbKontoID.Text = "lbKontoID"
        Me.lbKontoID.Visible = False
        '
        'tbTyp
        '
        Me.tbTyp.Enabled = False
        Me.tbTyp.ForeColor = System.Drawing.Color.Blue
        Me.tbTyp.Location = New System.Drawing.Point(668, 55)
        Me.tbTyp.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTyp.MaxLength = 50
        Me.tbTyp.Name = "tbTyp"
        Me.tbTyp.Size = New System.Drawing.Size(316, 26)
        Me.tbTyp.TabIndex = 87
        '
        'tbBIC
        '
        Me.tbBIC.Enabled = False
        Me.tbBIC.ForeColor = System.Drawing.Color.Blue
        Me.tbBIC.Location = New System.Drawing.Point(162, 129)
        Me.tbBIC.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbBIC.MaxLength = 15
        Me.tbBIC.Name = "tbBIC"
        Me.tbBIC.Size = New System.Drawing.Size(146, 26)
        Me.tbBIC.TabIndex = 86
        '
        'tbKZ
        '
        Me.tbKZ.Enabled = False
        Me.tbKZ.ForeColor = System.Drawing.Color.Blue
        Me.tbKZ.Location = New System.Drawing.Point(668, 95)
        Me.tbKZ.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbKZ.MaxLength = 10
        Me.tbKZ.Name = "tbKZ"
        Me.tbKZ.Size = New System.Drawing.Size(146, 26)
        Me.tbKZ.TabIndex = 88
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Location = New System.Drawing.Point(14, 132)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(140, 31)
        Me.Label12.TabIndex = 84
        Me.Label12.Text = "BIC"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label24
        '
        Me.Label24.BackColor = System.Drawing.Color.Transparent
        Me.Label24.Location = New System.Drawing.Point(519, 17)
        Me.Label24.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(140, 31)
        Me.Label24.TabIndex = 81
        Me.Label24.Text = "Institut"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(14, 94)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(140, 31)
        Me.Label8.TabIndex = 83
        Me.Label8.Text = "IBAN"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label21
        '
        Me.Label21.BackColor = System.Drawing.Color.Transparent
        Me.Label21.Location = New System.Drawing.Point(519, 55)
        Me.Label21.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(140, 31)
        Me.Label21.TabIndex = 82
        Me.Label21.Text = "Verwendung"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbIBAN
        '
        Me.tbIBAN.Enabled = False
        Me.tbIBAN.ForeColor = System.Drawing.Color.Blue
        Me.tbIBAN.Location = New System.Drawing.Point(162, 91)
        Me.tbIBAN.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbIBAN.MaxLength = 25
        Me.tbIBAN.Name = "tbIBAN"
        Me.tbIBAN.Size = New System.Drawing.Size(316, 26)
        Me.tbIBAN.TabIndex = 78
        '
        'tbInstitut
        '
        Me.tbInstitut.Enabled = False
        Me.tbInstitut.ForeColor = System.Drawing.Color.Blue
        Me.tbInstitut.Location = New System.Drawing.Point(668, 18)
        Me.tbInstitut.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbInstitut.MaxLength = 50
        Me.tbInstitut.Name = "tbInstitut"
        Me.tbInstitut.Size = New System.Drawing.Size(316, 26)
        Me.tbInstitut.TabIndex = 77
        '
        'tbBLZ
        '
        Me.tbBLZ.Enabled = False
        Me.tbBLZ.ForeColor = System.Drawing.Color.Blue
        Me.tbBLZ.Location = New System.Drawing.Point(162, 52)
        Me.tbBLZ.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbBLZ.MaxLength = 10
        Me.tbBLZ.Name = "tbBLZ"
        Me.tbBLZ.Size = New System.Drawing.Size(146, 26)
        Me.tbBLZ.TabIndex = 76
        '
        'tbKTO
        '
        Me.tbKTO.Enabled = False
        Me.tbKTO.ForeColor = System.Drawing.Color.Blue
        Me.tbKTO.Location = New System.Drawing.Point(162, 14)
        Me.tbKTO.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbKTO.MaxLength = 10
        Me.tbKTO.Name = "tbKTO"
        Me.tbKTO.Size = New System.Drawing.Size(146, 26)
        Me.tbKTO.TabIndex = 75
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Location = New System.Drawing.Point(519, 94)
        Me.Label14.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(140, 31)
        Me.Label14.TabIndex = 85
        Me.Label14.Text = "KZ"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label26
        '
        Me.Label26.BackColor = System.Drawing.Color.Transparent
        Me.Label26.Location = New System.Drawing.Point(14, 55)
        Me.Label26.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(140, 31)
        Me.Label26.TabIndex = 80
        Me.Label26.Text = "Bankleitzahl"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label28
        '
        Me.Label28.BackColor = System.Drawing.Color.Transparent
        Me.Label28.Location = New System.Drawing.Point(14, 17)
        Me.Label28.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(140, 31)
        Me.Label28.TabIndex = 79
        Me.Label28.Text = "Kontonummer"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tsKonto
        '
        Me.tsKonto.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsKonto.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbNewKonto, Me.ToolStripSeparator13, Me.tsbEditKonto, Me.ToolStripSeparator14, Me.tsbSaveKonto, Me.ToolStripSeparator15, Me.tsbBreakKonto, Me.ToolStripSeparator16, Me.tsbDelKonto})
        Me.tsKonto.Location = New System.Drawing.Point(0, 0)
        Me.tsKonto.Name = "tsKonto"
        Me.tsKonto.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.tsKonto.Size = New System.Drawing.Size(1441, 29)
        Me.tsKonto.TabIndex = 7
        Me.tsKonto.Text = "ToolStrip1"
        '
        'tsbNewKonto
        '
        Me.tsbNewKonto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbNewKonto.Image = Global.Pension.My.Resources.Resources.cabview_dll_Ico14_ico_Ico1
        Me.tsbNewKonto.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNewKonto.Name = "tsbNewKonto"
        Me.tsbNewKonto.Size = New System.Drawing.Size(34, 24)
        Me.tsbNewKonto.ToolTipText = "Neues Konto erstellen"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(6, 29)
        '
        'tsbEditKonto
        '
        Me.tsbEditKonto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbEditKonto.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1474_ico_Ico1
        Me.tsbEditKonto.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEditKonto.Name = "tsbEditKonto"
        Me.tsbEditKonto.Size = New System.Drawing.Size(34, 24)
        Me.tsbEditKonto.Text = "Bearbeiten"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(6, 29)
        '
        'tsbSaveKonto
        '
        Me.tsbSaveKonto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSaveKonto.Enabled = False
        Me.tsbSaveKonto.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSaveKonto.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSaveKonto.Name = "tsbSaveKonto"
        Me.tsbSaveKonto.Size = New System.Drawing.Size(34, 24)
        Me.tsbSaveKonto.Text = "Speichen"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(6, 29)
        '
        'tsbBreakKonto
        '
        Me.tsbBreakKonto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbBreakKonto.Enabled = False
        Me.tsbBreakKonto.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsbBreakKonto.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbBreakKonto.Name = "tsbBreakKonto"
        Me.tsbBreakKonto.Size = New System.Drawing.Size(34, 24)
        Me.tsbBreakKonto.Text = "Abbrechen"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(6, 29)
        '
        'tsbDelKonto
        '
        Me.tsbDelKonto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDelKonto.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbDelKonto.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDelKonto.Name = "tsbDelKonto"
        Me.tsbDelKonto.Size = New System.Drawing.Size(34, 24)
        Me.tsbDelKonto.Text = "Löschen"
        '
        'lvKonto
        '
        Me.lvKonto.HideSelection = False
        Me.lvKonto.Location = New System.Drawing.Point(22, 62)
        Me.lvKonto.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lvKonto.Name = "lvKonto"
        Me.lvKonto.Size = New System.Drawing.Size(1408, 364)
        Me.lvKonto.TabIndex = 6
        Me.lvKonto.UseCompatibleStateImageBehavior = False
        '
        'tpObjekte
        '
        Me.tpObjekte.Controls.Add(Me.paObjekt)
        Me.tpObjekte.Controls.Add(Me.lvObjekt)
        Me.tpObjekte.Controls.Add(Me.tsObjekt)
        Me.tpObjekte.Location = New System.Drawing.Point(4, 29)
        Me.tpObjekte.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpObjekte.Name = "tpObjekte"
        Me.tpObjekte.Size = New System.Drawing.Size(1441, 879)
        Me.tpObjekte.TabIndex = 5
        Me.tpObjekte.Text = "Objekte"
        Me.tpObjekte.UseVisualStyleBackColor = True
        '
        'paObjekt
        '
        Me.paObjekt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.paObjekt.Controls.Add(Me.lbRGBString)
        Me.paObjekt.Controls.Add(Me.btColorObjekt)
        Me.paObjekt.Controls.Add(Me.lbObjektID)
        Me.paObjekt.Controls.Add(Me.tbOHNr)
        Me.paObjekt.Controls.Add(Me.tbOOrtsteil)
        Me.paObjekt.Controls.Add(Me.tbOOrt)
        Me.paObjekt.Controls.Add(Me.Label16)
        Me.paObjekt.Controls.Add(Me.Label29)
        Me.paObjekt.Controls.Add(Me.Label30)
        Me.paObjekt.Controls.Add(Me.tbOPLZ)
        Me.paObjekt.Controls.Add(Me.tbOTelefon)
        Me.paObjekt.Controls.Add(Me.tbOStr)
        Me.paObjekt.Controls.Add(Me.tbOName)
        Me.paObjekt.Controls.Add(Me.Label33)
        Me.paObjekt.Controls.Add(Me.Label34)
        Me.paObjekt.Location = New System.Drawing.Point(22, 451)
        Me.paObjekt.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.paObjekt.Name = "paObjekt"
        Me.paObjekt.Size = New System.Drawing.Size(1400, 170)
        Me.paObjekt.TabIndex = 90
        '
        'lbRGBString
        '
        Me.lbRGBString.AutoSize = True
        Me.lbRGBString.Location = New System.Drawing.Point(519, 94)
        Me.lbRGBString.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbRGBString.Name = "lbRGBString"
        Me.lbRGBString.Size = New System.Drawing.Size(99, 20)
        Me.lbRGBString.TabIndex = 91
        Me.lbRGBString.Text = "lbRGBString"
        '
        'btColorObjekt
        '
        Me.btColorObjekt.Enabled = False
        Me.btColorObjekt.Location = New System.Drawing.Point(524, 125)
        Me.btColorObjekt.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btColorObjekt.Name = "btColorObjekt"
        Me.btColorObjekt.Size = New System.Drawing.Size(462, 35)
        Me.btColorObjekt.TabIndex = 90
        Me.btColorObjekt.Text = "Farbe des Objektes setzen"
        Me.btColorObjekt.UseVisualStyleBackColor = True
        '
        'lbObjektID
        '
        Me.lbObjektID.AutoSize = True
        Me.lbObjektID.Location = New System.Drawing.Point(1132, 140)
        Me.lbObjektID.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbObjektID.Name = "lbObjektID"
        Me.lbObjektID.Size = New System.Drawing.Size(72, 20)
        Me.lbObjektID.TabIndex = 89
        Me.lbObjektID.Text = "ObjektID"
        Me.lbObjektID.Visible = False
        '
        'tbOHNr
        '
        Me.tbOHNr.Enabled = False
        Me.tbOHNr.ForeColor = System.Drawing.Color.Blue
        Me.tbOHNr.Location = New System.Drawing.Point(418, 52)
        Me.tbOHNr.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbOHNr.MaxLength = 5
        Me.tbOHNr.Name = "tbOHNr"
        Me.tbOHNr.Size = New System.Drawing.Size(60, 26)
        Me.tbOHNr.TabIndex = 87
        '
        'tbOOrtsteil
        '
        Me.tbOOrtsteil.Enabled = False
        Me.tbOOrtsteil.ForeColor = System.Drawing.Color.Blue
        Me.tbOOrtsteil.Location = New System.Drawing.Point(162, 129)
        Me.tbOOrtsteil.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbOOrtsteil.MaxLength = 50
        Me.tbOOrtsteil.Name = "tbOOrtsteil"
        Me.tbOOrtsteil.Size = New System.Drawing.Size(316, 26)
        Me.tbOOrtsteil.TabIndex = 86
        '
        'tbOOrt
        '
        Me.tbOOrt.Enabled = False
        Me.tbOOrt.ForeColor = System.Drawing.Color.Blue
        Me.tbOOrt.Location = New System.Drawing.Point(243, 91)
        Me.tbOOrt.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbOOrt.MaxLength = 50
        Me.tbOOrt.Name = "tbOOrt"
        Me.tbOOrt.Size = New System.Drawing.Size(235, 26)
        Me.tbOOrt.TabIndex = 88
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.Transparent
        Me.Label16.Location = New System.Drawing.Point(14, 132)
        Me.Label16.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(140, 31)
        Me.Label16.TabIndex = 84
        Me.Label16.Text = "Ortsteil"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label29
        '
        Me.Label29.BackColor = System.Drawing.Color.Transparent
        Me.Label29.Location = New System.Drawing.Point(519, 17)
        Me.Label29.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(140, 31)
        Me.Label29.TabIndex = 81
        Me.Label29.Text = "Telefon"
        '
        'Label30
        '
        Me.Label30.BackColor = System.Drawing.Color.Transparent
        Me.Label30.Location = New System.Drawing.Point(14, 94)
        Me.Label30.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(140, 31)
        Me.Label30.TabIndex = 83
        Me.Label30.Text = "PLZ / Ort"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbOPLZ
        '
        Me.tbOPLZ.Enabled = False
        Me.tbOPLZ.ForeColor = System.Drawing.Color.Blue
        Me.tbOPLZ.Location = New System.Drawing.Point(162, 91)
        Me.tbOPLZ.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbOPLZ.MaxLength = 5
        Me.tbOPLZ.Name = "tbOPLZ"
        Me.tbOPLZ.Size = New System.Drawing.Size(70, 26)
        Me.tbOPLZ.TabIndex = 78
        '
        'tbOTelefon
        '
        Me.tbOTelefon.Enabled = False
        Me.tbOTelefon.ForeColor = System.Drawing.Color.Blue
        Me.tbOTelefon.Location = New System.Drawing.Point(668, 18)
        Me.tbOTelefon.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbOTelefon.MaxLength = 15
        Me.tbOTelefon.Name = "tbOTelefon"
        Me.tbOTelefon.Size = New System.Drawing.Size(316, 26)
        Me.tbOTelefon.TabIndex = 77
        '
        'tbOStr
        '
        Me.tbOStr.Enabled = False
        Me.tbOStr.ForeColor = System.Drawing.Color.Blue
        Me.tbOStr.Location = New System.Drawing.Point(162, 52)
        Me.tbOStr.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbOStr.MaxLength = 50
        Me.tbOStr.Name = "tbOStr"
        Me.tbOStr.Size = New System.Drawing.Size(246, 26)
        Me.tbOStr.TabIndex = 76
        '
        'tbOName
        '
        Me.tbOName.Enabled = False
        Me.tbOName.ForeColor = System.Drawing.Color.Blue
        Me.tbOName.Location = New System.Drawing.Point(162, 14)
        Me.tbOName.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbOName.MaxLength = 50
        Me.tbOName.Name = "tbOName"
        Me.tbOName.Size = New System.Drawing.Size(316, 26)
        Me.tbOName.TabIndex = 75
        '
        'Label33
        '
        Me.Label33.BackColor = System.Drawing.Color.Transparent
        Me.Label33.Location = New System.Drawing.Point(14, 55)
        Me.Label33.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(140, 31)
        Me.Label33.TabIndex = 80
        Me.Label33.Text = "Strasse / HNr."
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label34
        '
        Me.Label34.BackColor = System.Drawing.Color.Transparent
        Me.Label34.Location = New System.Drawing.Point(14, 17)
        Me.Label34.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(140, 31)
        Me.Label34.TabIndex = 79
        Me.Label34.Text = "Name"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvObjekt
        '
        Me.lvObjekt.HideSelection = False
        Me.lvObjekt.Location = New System.Drawing.Point(22, 62)
        Me.lvObjekt.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lvObjekt.Name = "lvObjekt"
        Me.lvObjekt.Size = New System.Drawing.Size(1400, 282)
        Me.lvObjekt.TabIndex = 5
        Me.lvObjekt.UseCompatibleStateImageBehavior = False
        '
        'tsObjekt
        '
        Me.tsObjekt.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsObjekt.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbNewObj, Me.ToolStripSeparator3, Me.tsbEditObj, Me.ToolStripSeparator6, Me.tsbSaveObj, Me.ToolStripSeparator8, Me.tsbBraeckObj, Me.ToolStripSeparator19, Me.tsbDelObj})
        Me.tsObjekt.Location = New System.Drawing.Point(0, 0)
        Me.tsObjekt.Name = "tsObjekt"
        Me.tsObjekt.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.tsObjekt.Size = New System.Drawing.Size(1441, 29)
        Me.tsObjekt.TabIndex = 4
        Me.tsObjekt.Text = "ToolStrip1"
        '
        'tsbNewObj
        '
        Me.tsbNewObj.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbNewObj.Image = Global.Pension.My.Resources.Resources.cabview_dll_Ico14_ico_Ico1
        Me.tsbNewObj.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNewObj.Name = "tsbNewObj"
        Me.tsbNewObj.Size = New System.Drawing.Size(34, 24)
        Me.tsbNewObj.ToolTipText = "Neues Objekt"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 29)
        '
        'tsbEditObj
        '
        Me.tsbEditObj.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbEditObj.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1474_ico_Ico1
        Me.tsbEditObj.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEditObj.Name = "tsbEditObj"
        Me.tsbEditObj.Size = New System.Drawing.Size(34, 24)
        Me.tsbEditObj.Text = "Bearbeiten"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 29)
        '
        'tsbSaveObj
        '
        Me.tsbSaveObj.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSaveObj.Enabled = False
        Me.tsbSaveObj.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSaveObj.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSaveObj.Name = "tsbSaveObj"
        Me.tsbSaveObj.Size = New System.Drawing.Size(34, 24)
        Me.tsbSaveObj.Text = "Speichen"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 29)
        '
        'tsbBraeckObj
        '
        Me.tsbBraeckObj.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbBraeckObj.Enabled = False
        Me.tsbBraeckObj.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsbBraeckObj.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbBraeckObj.Name = "tsbBraeckObj"
        Me.tsbBraeckObj.Size = New System.Drawing.Size(34, 24)
        Me.tsbBraeckObj.Text = "Abbrechen"
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(6, 29)
        '
        'tsbDelObj
        '
        Me.tsbDelObj.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDelObj.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbDelObj.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDelObj.Name = "tsbDelObj"
        Me.tsbDelObj.Size = New System.Drawing.Size(34, 24)
        Me.tsbDelObj.Text = "Löschen"
        '
        'tpZimmer
        '
        Me.tpZimmer.Controls.Add(Me.paZimmer)
        Me.tpZimmer.Controls.Add(Me.tsZimmer)
        Me.tpZimmer.Controls.Add(Me.lvZimmer)
        Me.tpZimmer.Location = New System.Drawing.Point(4, 29)
        Me.tpZimmer.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpZimmer.Name = "tpZimmer"
        Me.tpZimmer.Size = New System.Drawing.Size(1441, 879)
        Me.tpZimmer.TabIndex = 4
        Me.tpZimmer.Text = "Zimmer"
        Me.tpZimmer.UseVisualStyleBackColor = True
        '
        'paZimmer
        '
        Me.paZimmer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.paZimmer.Controls.Add(Me.Label94)
        Me.paZimmer.Controls.Add(Me.Label93)
        Me.paZimmer.Controls.Add(Me.tbDatei)
        Me.paZimmer.Controls.Add(Me.tbSaveCode)
        Me.paZimmer.Controls.Add(Me.chCode)
        Me.paZimmer.Controls.Add(Me.Label92)
        Me.paZimmer.Controls.Add(Me.Label91)
        Me.paZimmer.Controls.Add(Me.Label57)
        Me.paZimmer.Controls.Add(Me.Label56)
        Me.paZimmer.Controls.Add(Me.Label55)
        Me.paZimmer.Controls.Add(Me.tbTrans5)
        Me.paZimmer.Controls.Add(Me.tbTrans4)
        Me.paZimmer.Controls.Add(Me.tbTrans3)
        Me.paZimmer.Controls.Add(Me.tbTrans2)
        Me.paZimmer.Controls.Add(Me.tbTrans1)
        Me.paZimmer.Controls.Add(Me.tbZP10)
        Me.paZimmer.Controls.Add(Me.tbZP9)
        Me.paZimmer.Controls.Add(Me.tbZP8)
        Me.paZimmer.Controls.Add(Me.tbZP7)
        Me.paZimmer.Controls.Add(Me.tbZP6)
        Me.paZimmer.Controls.Add(Me.tbZP5)
        Me.paZimmer.Controls.Add(Me.tbZP4)
        Me.paZimmer.Controls.Add(Me.tbZP3)
        Me.paZimmer.Controls.Add(Me.tbZP2)
        Me.paZimmer.Controls.Add(Me.tbZP1)
        Me.paZimmer.Controls.Add(Me.Label90)
        Me.paZimmer.Controls.Add(Me.Label89)
        Me.paZimmer.Controls.Add(Me.Label88)
        Me.paZimmer.Controls.Add(Me.Label87)
        Me.paZimmer.Controls.Add(Me.Label86)
        Me.paZimmer.Controls.Add(Me.Label85)
        Me.paZimmer.Controls.Add(Me.Label84)
        Me.paZimmer.Controls.Add(Me.Label83)
        Me.paZimmer.Controls.Add(Me.Label82)
        Me.paZimmer.Controls.Add(Me.Label81)
        Me.paZimmer.Controls.Add(Me.Label80)
        Me.paZimmer.Controls.Add(Me.tbZBettenKi)
        Me.paZimmer.Controls.Add(Me.tbZBettenEr)
        Me.paZimmer.Controls.Add(Me.tbZBettenMin)
        Me.paZimmer.Controls.Add(Me.Label79)
        Me.paZimmer.Controls.Add(Me.Label78)
        Me.paZimmer.Controls.Add(Me.Label77)
        Me.paZimmer.Controls.Add(Me.tbZNummer)
        Me.paZimmer.Controls.Add(Me.chFeWo)
        Me.paZimmer.Controls.Add(Me.coZArt)
        Me.paZimmer.Controls.Add(Me.tbZBetten)
        Me.paZimmer.Controls.Add(Me.Label37)
        Me.paZimmer.Controls.Add(Me.coObjekt)
        Me.paZimmer.Controls.Add(Me.lbZimmerID)
        Me.paZimmer.Controls.Add(Me.tbZAus)
        Me.paZimmer.Controls.Add(Me.Label36)
        Me.paZimmer.Controls.Add(Me.Label38)
        Me.paZimmer.Controls.Add(Me.tbZArt)
        Me.paZimmer.Controls.Add(Me.tbZName)
        Me.paZimmer.Controls.Add(Me.Label39)
        Me.paZimmer.Controls.Add(Me.Label40)
        Me.paZimmer.Location = New System.Drawing.Point(22, 512)
        Me.paZimmer.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.paZimmer.Name = "paZimmer"
        Me.paZimmer.Size = New System.Drawing.Size(1400, 348)
        Me.paZimmer.TabIndex = 91
        '
        'Label94
        '
        Me.Label94.AutoSize = True
        Me.Label94.Location = New System.Drawing.Point(1131, 23)
        Me.Label94.Name = "Label94"
        Me.Label94.Size = New System.Drawing.Size(47, 20)
        Me.Label94.TabIndex = 174
        Me.Label94.Text = "Datei"
        '
        'Label93
        '
        Me.Label93.AutoSize = True
        Me.Label93.Location = New System.Drawing.Point(957, 23)
        Me.Label93.Name = "Label93"
        Me.Label93.Size = New System.Drawing.Size(83, 20)
        Me.Label93.TabIndex = 173
        Me.Label93.Text = "SaveCode"
        '
        'tbDatei
        '
        Me.tbDatei.Location = New System.Drawing.Point(1185, 18)
        Me.tbDatei.Name = "tbDatei"
        Me.tbDatei.Size = New System.Drawing.Size(140, 26)
        Me.tbDatei.TabIndex = 172
        '
        'tbSaveCode
        '
        Me.tbSaveCode.Location = New System.Drawing.Point(1059, 18)
        Me.tbSaveCode.Name = "tbSaveCode"
        Me.tbSaveCode.Size = New System.Drawing.Size(64, 26)
        Me.tbSaveCode.TabIndex = 171
        '
        'chCode
        '
        Me.chCode.AutoSize = True
        Me.chCode.Location = New System.Drawing.Point(862, 211)
        Me.chCode.Name = "chCode"
        Me.chCode.Size = New System.Drawing.Size(73, 24)
        Me.chCode.TabIndex = 170
        Me.chCode.Text = "Code"
        Me.chCode.UseVisualStyleBackColor = True
        '
        'Label92
        '
        Me.Label92.AutoSize = True
        Me.Label92.Location = New System.Drawing.Point(957, 205)
        Me.Label92.Name = "Label92"
        Me.Label92.Size = New System.Drawing.Size(54, 20)
        Me.Label92.TabIndex = 169
        Me.Label92.Text = "Chip 5"
        '
        'Label91
        '
        Me.Label91.AutoSize = True
        Me.Label91.Location = New System.Drawing.Point(957, 166)
        Me.Label91.Name = "Label91"
        Me.Label91.Size = New System.Drawing.Size(54, 20)
        Me.Label91.TabIndex = 168
        Me.Label91.Text = "Chip 4"
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.Location = New System.Drawing.Point(957, 131)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(54, 20)
        Me.Label57.TabIndex = 167
        Me.Label57.Text = "Chip 3"
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Location = New System.Drawing.Point(957, 97)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(54, 20)
        Me.Label56.TabIndex = 166
        Me.Label56.Text = "Chip 2"
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Location = New System.Drawing.Point(957, 60)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(54, 20)
        Me.Label55.TabIndex = 165
        Me.Label55.Text = "Chip 1"
        '
        'tbTrans5
        '
        Me.tbTrans5.Location = New System.Drawing.Point(1059, 197)
        Me.tbTrans5.Name = "tbTrans5"
        Me.tbTrans5.Size = New System.Drawing.Size(266, 26)
        Me.tbTrans5.TabIndex = 164
        '
        'tbTrans4
        '
        Me.tbTrans4.Location = New System.Drawing.Point(1059, 162)
        Me.tbTrans4.Name = "tbTrans4"
        Me.tbTrans4.Size = New System.Drawing.Size(266, 26)
        Me.tbTrans4.TabIndex = 163
        '
        'tbTrans3
        '
        Me.tbTrans3.Location = New System.Drawing.Point(1059, 126)
        Me.tbTrans3.Name = "tbTrans3"
        Me.tbTrans3.Size = New System.Drawing.Size(266, 26)
        Me.tbTrans3.TabIndex = 162
        '
        'tbTrans2
        '
        Me.tbTrans2.Location = New System.Drawing.Point(1059, 91)
        Me.tbTrans2.Name = "tbTrans2"
        Me.tbTrans2.Size = New System.Drawing.Size(266, 26)
        Me.tbTrans2.TabIndex = 161
        '
        'tbTrans1
        '
        Me.tbTrans1.Location = New System.Drawing.Point(1059, 55)
        Me.tbTrans1.Name = "tbTrans1"
        Me.tbTrans1.Size = New System.Drawing.Size(266, 26)
        Me.tbTrans1.TabIndex = 160
        '
        'tbZP10
        '
        Me.tbZP10.Location = New System.Drawing.Point(796, 211)
        Me.tbZP10.Name = "tbZP10"
        Me.tbZP10.Size = New System.Drawing.Size(44, 26)
        Me.tbZP10.TabIndex = 159
        '
        'tbZP9
        '
        Me.tbZP9.Location = New System.Drawing.Point(705, 211)
        Me.tbZP9.Name = "tbZP9"
        Me.tbZP9.Size = New System.Drawing.Size(44, 26)
        Me.tbZP9.TabIndex = 158
        '
        'tbZP8
        '
        Me.tbZP8.Location = New System.Drawing.Point(622, 211)
        Me.tbZP8.Name = "tbZP8"
        Me.tbZP8.Size = New System.Drawing.Size(44, 26)
        Me.tbZP8.TabIndex = 157
        '
        'tbZP7
        '
        Me.tbZP7.Location = New System.Drawing.Point(532, 211)
        Me.tbZP7.Name = "tbZP7"
        Me.tbZP7.Size = New System.Drawing.Size(44, 26)
        Me.tbZP7.TabIndex = 156
        '
        'tbZP6
        '
        Me.tbZP6.Location = New System.Drawing.Point(444, 211)
        Me.tbZP6.Name = "tbZP6"
        Me.tbZP6.Size = New System.Drawing.Size(44, 26)
        Me.tbZP6.TabIndex = 155
        '
        'tbZP5
        '
        Me.tbZP5.Location = New System.Drawing.Point(357, 211)
        Me.tbZP5.Name = "tbZP5"
        Me.tbZP5.Size = New System.Drawing.Size(44, 26)
        Me.tbZP5.TabIndex = 154
        '
        'tbZP4
        '
        Me.tbZP4.Location = New System.Drawing.Point(270, 211)
        Me.tbZP4.Name = "tbZP4"
        Me.tbZP4.Size = New System.Drawing.Size(44, 26)
        Me.tbZP4.TabIndex = 153
        '
        'tbZP3
        '
        Me.tbZP3.Location = New System.Drawing.Point(186, 211)
        Me.tbZP3.Name = "tbZP3"
        Me.tbZP3.Size = New System.Drawing.Size(44, 26)
        Me.tbZP3.TabIndex = 152
        '
        'tbZP2
        '
        Me.tbZP2.Location = New System.Drawing.Point(98, 211)
        Me.tbZP2.Name = "tbZP2"
        Me.tbZP2.Size = New System.Drawing.Size(44, 26)
        Me.tbZP2.TabIndex = 151
        '
        'tbZP1
        '
        Me.tbZP1.Location = New System.Drawing.Point(16, 211)
        Me.tbZP1.Name = "tbZP1"
        Me.tbZP1.Size = New System.Drawing.Size(44, 26)
        Me.tbZP1.TabIndex = 150
        '
        'Label90
        '
        Me.Label90.AutoSize = True
        Me.Label90.Location = New System.Drawing.Point(794, 174)
        Me.Label90.Name = "Label90"
        Me.Label90.Size = New System.Drawing.Size(37, 20)
        Me.Label90.TabIndex = 149
        Me.Label90.Text = "P10"
        '
        'Label89
        '
        Me.Label89.AutoSize = True
        Me.Label89.Location = New System.Drawing.Point(702, 174)
        Me.Label89.Name = "Label89"
        Me.Label89.Size = New System.Drawing.Size(28, 20)
        Me.Label89.TabIndex = 148
        Me.Label89.Text = "P9"
        '
        'Label88
        '
        Me.Label88.AutoSize = True
        Me.Label88.Location = New System.Drawing.Point(618, 174)
        Me.Label88.Name = "Label88"
        Me.Label88.Size = New System.Drawing.Size(28, 20)
        Me.Label88.TabIndex = 147
        Me.Label88.Text = "P8"
        '
        'Label87
        '
        Me.Label87.AutoSize = True
        Me.Label87.Location = New System.Drawing.Point(528, 174)
        Me.Label87.Name = "Label87"
        Me.Label87.Size = New System.Drawing.Size(28, 20)
        Me.Label87.TabIndex = 146
        Me.Label87.Text = "P7"
        '
        'Label86
        '
        Me.Label86.AutoSize = True
        Me.Label86.Location = New System.Drawing.Point(440, 174)
        Me.Label86.Name = "Label86"
        Me.Label86.Size = New System.Drawing.Size(28, 20)
        Me.Label86.TabIndex = 145
        Me.Label86.Text = "P6"
        '
        'Label85
        '
        Me.Label85.AutoSize = True
        Me.Label85.Location = New System.Drawing.Point(354, 174)
        Me.Label85.Name = "Label85"
        Me.Label85.Size = New System.Drawing.Size(28, 20)
        Me.Label85.TabIndex = 144
        Me.Label85.Text = "P5"
        '
        'Label84
        '
        Me.Label84.AutoSize = True
        Me.Label84.Location = New System.Drawing.Point(267, 174)
        Me.Label84.Name = "Label84"
        Me.Label84.Size = New System.Drawing.Size(28, 20)
        Me.Label84.TabIndex = 143
        Me.Label84.Text = "P4"
        '
        'Label83
        '
        Me.Label83.AutoSize = True
        Me.Label83.Location = New System.Drawing.Point(183, 174)
        Me.Label83.Name = "Label83"
        Me.Label83.Size = New System.Drawing.Size(28, 20)
        Me.Label83.TabIndex = 142
        Me.Label83.Text = "P3"
        '
        'Label82
        '
        Me.Label82.AutoSize = True
        Me.Label82.Location = New System.Drawing.Point(94, 174)
        Me.Label82.Name = "Label82"
        Me.Label82.Size = New System.Drawing.Size(28, 20)
        Me.Label82.TabIndex = 141
        Me.Label82.Text = "P2"
        '
        'Label81
        '
        Me.Label81.AutoSize = True
        Me.Label81.Location = New System.Drawing.Point(14, 174)
        Me.Label81.Name = "Label81"
        Me.Label81.Size = New System.Drawing.Size(28, 20)
        Me.Label81.TabIndex = 140
        Me.Label81.Text = "P1"
        '
        'Label80
        '
        Me.Label80.AutoSize = True
        Me.Label80.Location = New System.Drawing.Point(14, 140)
        Me.Label80.Name = "Label80"
        Me.Label80.Size = New System.Drawing.Size(119, 20)
        Me.Label80.TabIndex = 139
        Me.Label80.Text = "PreiseAbschlag"
        '
        'tbZBettenKi
        '
        Me.tbZBettenKi.Location = New System.Drawing.Point(904, 54)
        Me.tbZBettenKi.Name = "tbZBettenKi"
        Me.tbZBettenKi.Size = New System.Drawing.Size(32, 26)
        Me.tbZBettenKi.TabIndex = 138
        '
        'tbZBettenEr
        '
        Me.tbZBettenEr.Location = New System.Drawing.Point(786, 52)
        Me.tbZBettenEr.Name = "tbZBettenEr"
        Me.tbZBettenEr.Size = New System.Drawing.Size(34, 26)
        Me.tbZBettenEr.TabIndex = 137
        '
        'tbZBettenMin
        '
        Me.tbZBettenMin.Location = New System.Drawing.Point(672, 52)
        Me.tbZBettenMin.Name = "tbZBettenMin"
        Me.tbZBettenMin.Size = New System.Drawing.Size(36, 26)
        Me.tbZBettenMin.TabIndex = 136
        '
        'Label79
        '
        Me.Label79.AutoSize = True
        Me.Label79.Location = New System.Drawing.Point(828, 60)
        Me.Label79.Name = "Label79"
        Me.Label79.Size = New System.Drawing.Size(59, 20)
        Me.Label79.TabIndex = 135
        Me.Label79.Text = "Max Ki."
        '
        'Label78
        '
        Me.Label78.AutoSize = True
        Me.Label78.Location = New System.Drawing.Point(716, 58)
        Me.Label78.Name = "Label78"
        Me.Label78.Size = New System.Drawing.Size(62, 20)
        Me.Label78.TabIndex = 134
        Me.Label78.Text = "Max Er."
        '
        'Label77
        '
        Me.Label77.AutoSize = True
        Me.Label77.Location = New System.Drawing.Point(630, 60)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(34, 20)
        Me.Label77.TabIndex = 133
        Me.Label77.Text = "Min"
        '
        'tbZNummer
        '
        Me.tbZNummer.ForeColor = System.Drawing.Color.Blue
        Me.tbZNummer.Location = New System.Drawing.Point(98, 272)
        Me.tbZNummer.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbZNummer.Name = "tbZNummer"
        Me.tbZNummer.Size = New System.Drawing.Size(44, 26)
        Me.tbZNummer.TabIndex = 132
        '
        'chFeWo
        '
        Me.chFeWo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chFeWo.Enabled = False
        Me.chFeWo.Location = New System.Drawing.Point(518, 95)
        Me.chFeWo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chFeWo.Name = "chFeWo"
        Me.chFeWo.Size = New System.Drawing.Size(192, 31)
        Me.chFeWo.TabIndex = 131
        Me.chFeWo.Text = "FeWo / Zimmer"
        Me.chFeWo.UseVisualStyleBackColor = True
        '
        'coZArt
        '
        Me.coZArt.Enabled = False
        Me.coZArt.FormattingEnabled = True
        Me.coZArt.Location = New System.Drawing.Point(162, 52)
        Me.coZArt.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.coZArt.Name = "coZArt"
        Me.coZArt.Size = New System.Drawing.Size(316, 28)
        Me.coZArt.TabIndex = 130
        '
        'tbZBetten
        '
        Me.tbZBetten.Enabled = False
        Me.tbZBetten.ForeColor = System.Drawing.Color.Blue
        Me.tbZBetten.Location = New System.Drawing.Point(586, 52)
        Me.tbZBetten.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbZBetten.MaxLength = 50
        Me.tbZBetten.Name = "tbZBetten"
        Me.tbZBetten.Size = New System.Drawing.Size(34, 26)
        Me.tbZBetten.TabIndex = 92
        Me.tbZBetten.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label37
        '
        Me.Label37.BackColor = System.Drawing.Color.Transparent
        Me.Label37.Location = New System.Drawing.Point(513, 51)
        Me.Label37.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(64, 31)
        Me.Label37.TabIndex = 91
        Me.Label37.Text = "Betten"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'coObjekt
        '
        Me.coObjekt.FormattingEnabled = True
        Me.coObjekt.Location = New System.Drawing.Point(586, 14)
        Me.coObjekt.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.coObjekt.Name = "coObjekt"
        Me.coObjekt.Size = New System.Drawing.Size(350, 28)
        Me.coObjekt.TabIndex = 90
        '
        'lbZimmerID
        '
        Me.lbZimmerID.AutoSize = True
        Me.lbZimmerID.Location = New System.Drawing.Point(14, 277)
        Me.lbZimmerID.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbZimmerID.Name = "lbZimmerID"
        Me.lbZimmerID.Size = New System.Drawing.Size(79, 20)
        Me.lbZimmerID.TabIndex = 89
        Me.lbZimmerID.Text = "ZimmerID"
        Me.lbZimmerID.Visible = False
        '
        'tbZAus
        '
        Me.tbZAus.Enabled = False
        Me.tbZAus.ForeColor = System.Drawing.Color.Blue
        Me.tbZAus.Location = New System.Drawing.Point(162, 91)
        Me.tbZAus.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbZAus.MaxLength = 50
        Me.tbZAus.Name = "tbZAus"
        Me.tbZAus.Size = New System.Drawing.Size(316, 26)
        Me.tbZAus.TabIndex = 88
        '
        'Label36
        '
        Me.Label36.BackColor = System.Drawing.Color.Transparent
        Me.Label36.Location = New System.Drawing.Point(516, 12)
        Me.Label36.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(140, 31)
        Me.Label36.TabIndex = 84
        Me.Label36.Text = "Objekt"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label38
        '
        Me.Label38.BackColor = System.Drawing.Color.Transparent
        Me.Label38.Location = New System.Drawing.Point(14, 94)
        Me.Label38.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(140, 31)
        Me.Label38.TabIndex = 83
        Me.Label38.Text = "Ausstattung"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbZArt
        '
        Me.tbZArt.Enabled = False
        Me.tbZArt.ForeColor = System.Drawing.Color.Blue
        Me.tbZArt.Location = New System.Drawing.Point(162, 52)
        Me.tbZArt.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbZArt.MaxLength = 50
        Me.tbZArt.Name = "tbZArt"
        Me.tbZArt.Size = New System.Drawing.Size(316, 26)
        Me.tbZArt.TabIndex = 76
        '
        'tbZName
        '
        Me.tbZName.Enabled = False
        Me.tbZName.ForeColor = System.Drawing.Color.Blue
        Me.tbZName.Location = New System.Drawing.Point(162, 14)
        Me.tbZName.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbZName.MaxLength = 50
        Me.tbZName.Name = "tbZName"
        Me.tbZName.Size = New System.Drawing.Size(316, 26)
        Me.tbZName.TabIndex = 75
        '
        'Label39
        '
        Me.Label39.BackColor = System.Drawing.Color.Transparent
        Me.Label39.Location = New System.Drawing.Point(14, 51)
        Me.Label39.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(140, 31)
        Me.Label39.TabIndex = 80
        Me.Label39.Text = "Art der Unterkunft"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label40
        '
        Me.Label40.BackColor = System.Drawing.Color.Transparent
        Me.Label40.Location = New System.Drawing.Point(14, 17)
        Me.Label40.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(140, 31)
        Me.Label40.TabIndex = 79
        Me.Label40.Text = "Name"
        Me.Label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tsZimmer
        '
        Me.tsZimmer.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsZimmer.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbNeuZim, Me.ToolStripSeparator2, Me.tsbEditZim, Me.ToolStripSeparator4, Me.tsbSaveZim, Me.ToolStripSeparator5, Me.tsbBraekZim, Me.ToolStripSeparator18, Me.tsbDelZim, Me.ToolStripSeparator28, Me.tsmZimmer})
        Me.tsZimmer.Location = New System.Drawing.Point(0, 0)
        Me.tsZimmer.Name = "tsZimmer"
        Me.tsZimmer.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.tsZimmer.Size = New System.Drawing.Size(1441, 41)
        Me.tsZimmer.TabIndex = 3
        Me.tsZimmer.Text = "ToolStrip1"
        '
        'tsbNeuZim
        '
        Me.tsbNeuZim.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbNeuZim.Image = Global.Pension.My.Resources.Resources.cabview_dll_Ico14_ico_Ico1
        Me.tsbNeuZim.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNeuZim.Name = "tsbNeuZim"
        Me.tsbNeuZim.Size = New System.Drawing.Size(34, 36)
        Me.tsbNeuZim.Text = "Neues Zimmer"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 41)
        '
        'tsbEditZim
        '
        Me.tsbEditZim.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbEditZim.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1474_ico_Ico1
        Me.tsbEditZim.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEditZim.Name = "tsbEditZim"
        Me.tsbEditZim.Size = New System.Drawing.Size(34, 36)
        Me.tsbEditZim.Text = "Bearbeiten"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 41)
        '
        'tsbSaveZim
        '
        Me.tsbSaveZim.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSaveZim.Enabled = False
        Me.tsbSaveZim.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSaveZim.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSaveZim.Name = "tsbSaveZim"
        Me.tsbSaveZim.Size = New System.Drawing.Size(34, 36)
        Me.tsbSaveZim.Text = "Speichen"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 41)
        '
        'tsbBraekZim
        '
        Me.tsbBraekZim.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbBraekZim.Enabled = False
        Me.tsbBraekZim.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsbBraekZim.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbBraekZim.Name = "tsbBraekZim"
        Me.tsbBraekZim.Size = New System.Drawing.Size(34, 36)
        Me.tsbBraekZim.Text = "Abbrechen"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(6, 41)
        '
        'tsbDelZim
        '
        Me.tsbDelZim.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDelZim.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbDelZim.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDelZim.Name = "tsbDelZim"
        Me.tsbDelZim.Size = New System.Drawing.Size(34, 36)
        Me.tsbDelZim.Text = "Löschen"
        '
        'ToolStripSeparator28
        '
        Me.ToolStripSeparator28.Name = "ToolStripSeparator28"
        Me.ToolStripSeparator28.Size = New System.Drawing.Size(6, 41)
        '
        'tsmZimmer
        '
        Me.tsmZimmer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsmZimmer.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmImportZimmer, Me.tsmExportZimmer})
        Me.tsmZimmer.Image = CType(resources.GetObject("tsmZimmer.Image"), System.Drawing.Image)
        Me.tsmZimmer.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsmZimmer.Name = "tsmZimmer"
        Me.tsmZimmer.Size = New System.Drawing.Size(193, 36)
        Me.tsmZimmer.Text = "Import / Export"
        '
        'tsmImportZimmer
        '
        Me.tsmImportZimmer.Image = Global.Pension.My.Resources.Resources.ARW05DN
        Me.tsmImportZimmer.Name = "tsmImportZimmer"
        Me.tsmImportZimmer.Size = New System.Drawing.Size(189, 40)
        Me.tsmImportZimmer.Text = "Import"
        Me.tsmImportZimmer.ToolTipText = "Zimmer laden"
        '
        'tsmExportZimmer
        '
        Me.tsmExportZimmer.Image = Global.Pension.My.Resources.Resources.ARW05UP
        Me.tsmExportZimmer.Name = "tsmExportZimmer"
        Me.tsmExportZimmer.Size = New System.Drawing.Size(189, 40)
        Me.tsmExportZimmer.Text = "Export"
        Me.tsmExportZimmer.ToolTipText = "Zimmer sichern"
        '
        'lvZimmer
        '
        Me.lvZimmer.HideSelection = False
        Me.lvZimmer.Location = New System.Drawing.Point(22, 62)
        Me.lvZimmer.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lvZimmer.Name = "lvZimmer"
        Me.lvZimmer.Size = New System.Drawing.Size(1400, 424)
        Me.lvZimmer.TabIndex = 1
        Me.lvZimmer.UseCompatibleStateImageBehavior = False
        '
        'tpUser
        '
        Me.tpUser.Controls.Add(Me.paUser)
        Me.tpUser.Controls.Add(Me.lvUser)
        Me.tpUser.Controls.Add(Me.tsUser)
        Me.tpUser.Location = New System.Drawing.Point(4, 29)
        Me.tpUser.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpUser.Name = "tpUser"
        Me.tpUser.Size = New System.Drawing.Size(1441, 879)
        Me.tpUser.TabIndex = 6
        Me.tpUser.Text = "User-Verwaltung"
        Me.tpUser.UseVisualStyleBackColor = True
        '
        'paUser
        '
        Me.paUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.paUser.Controls.Add(Me.tbKUser)
        Me.paUser.Controls.Add(Me.Label48)
        Me.paUser.Controls.Add(Me.chKlar)
        Me.paUser.Controls.Add(Me.lbUserID)
        Me.paUser.Controls.Add(Me.tbUPassWD)
        Me.paUser.Controls.Add(Me.tbUUser)
        Me.paUser.Controls.Add(Me.Label42)
        Me.paUser.Controls.Add(Me.Label43)
        Me.paUser.Location = New System.Drawing.Point(22, 423)
        Me.paUser.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.paUser.Name = "paUser"
        Me.paUser.Size = New System.Drawing.Size(1400, 170)
        Me.paUser.TabIndex = 93
        '
        'tbKUser
        '
        Me.tbKUser.Enabled = False
        Me.tbKUser.ForeColor = System.Drawing.Color.Blue
        Me.tbKUser.Location = New System.Drawing.Point(162, 57)
        Me.tbKUser.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbKUser.MaxLength = 50
        Me.tbKUser.Name = "tbKUser"
        Me.tbKUser.Size = New System.Drawing.Size(316, 26)
        Me.tbKUser.TabIndex = 91
        '
        'Label48
        '
        Me.Label48.BackColor = System.Drawing.Color.Transparent
        Me.Label48.Location = New System.Drawing.Point(14, 57)
        Me.Label48.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(140, 31)
        Me.Label48.TabIndex = 92
        Me.Label48.Text = "Kurz-Name"
        Me.Label48.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chKlar
        '
        Me.chKlar.AutoSize = True
        Me.chKlar.Enabled = False
        Me.chKlar.Location = New System.Drawing.Point(504, 105)
        Me.chKlar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chKlar.Name = "chKlar"
        Me.chKlar.Size = New System.Drawing.Size(88, 24)
        Me.chKlar.TabIndex = 90
        Me.chKlar.Text = "Klartext"
        Me.chKlar.UseVisualStyleBackColor = True
        '
        'lbUserID
        '
        Me.lbUserID.AutoSize = True
        Me.lbUserID.Location = New System.Drawing.Point(1132, 140)
        Me.lbUserID.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbUserID.Name = "lbUserID"
        Me.lbUserID.Size = New System.Drawing.Size(60, 20)
        Me.lbUserID.TabIndex = 89
        Me.lbUserID.Text = "UserID"
        Me.lbUserID.Visible = False
        '
        'tbUPassWD
        '
        Me.tbUPassWD.Enabled = False
        Me.tbUPassWD.ForeColor = System.Drawing.Color.Blue
        Me.tbUPassWD.Location = New System.Drawing.Point(162, 97)
        Me.tbUPassWD.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbUPassWD.MaxLength = 50
        Me.tbUPassWD.Name = "tbUPassWD"
        Me.tbUPassWD.Size = New System.Drawing.Size(316, 26)
        Me.tbUPassWD.TabIndex = 76
        Me.tbUPassWD.UseSystemPasswordChar = True
        '
        'tbUUser
        '
        Me.tbUUser.Enabled = False
        Me.tbUUser.ForeColor = System.Drawing.Color.Blue
        Me.tbUUser.Location = New System.Drawing.Point(162, 14)
        Me.tbUUser.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbUUser.MaxLength = 50
        Me.tbUUser.Name = "tbUUser"
        Me.tbUUser.Size = New System.Drawing.Size(316, 26)
        Me.tbUUser.TabIndex = 75
        '
        'Label42
        '
        Me.Label42.BackColor = System.Drawing.Color.Transparent
        Me.Label42.Location = New System.Drawing.Point(14, 100)
        Me.Label42.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(140, 31)
        Me.Label42.TabIndex = 80
        Me.Label42.Text = "Passwort"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label43
        '
        Me.Label43.BackColor = System.Drawing.Color.Transparent
        Me.Label43.Location = New System.Drawing.Point(14, 17)
        Me.Label43.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(140, 31)
        Me.Label43.TabIndex = 79
        Me.Label43.Text = "Name"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvUser
        '
        Me.lvUser.HideSelection = False
        Me.lvUser.Location = New System.Drawing.Point(22, 62)
        Me.lvUser.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lvUser.Name = "lvUser"
        Me.lvUser.Size = New System.Drawing.Size(1400, 282)
        Me.lvUser.TabIndex = 92
        Me.lvUser.UseCompatibleStateImageBehavior = False
        '
        'tsUser
        '
        Me.tsUser.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsUser.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbNewUser, Me.ToolStripSeparator7, Me.tsbEditUser, Me.ToolStripSeparator9, Me.tsbSaveUser, Me.ToolStripSeparator11, Me.tsbBraeckUser, Me.ToolStripSeparator17, Me.tsbDelUser})
        Me.tsUser.Location = New System.Drawing.Point(0, 0)
        Me.tsUser.Name = "tsUser"
        Me.tsUser.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.tsUser.Size = New System.Drawing.Size(1441, 29)
        Me.tsUser.TabIndex = 4
        Me.tsUser.Text = "ToolStrip1"
        '
        'tsbNewUser
        '
        Me.tsbNewUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbNewUser.Image = Global.Pension.My.Resources.Resources.cabview_dll_Ico14_ico_Ico1
        Me.tsbNewUser.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNewUser.Name = "tsbNewUser"
        Me.tsbNewUser.Size = New System.Drawing.Size(34, 24)
        Me.tsbNewUser.Text = "Neuer Nutzer"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 29)
        '
        'tsbEditUser
        '
        Me.tsbEditUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbEditUser.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1474_ico_Ico1
        Me.tsbEditUser.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEditUser.Name = "tsbEditUser"
        Me.tsbEditUser.Size = New System.Drawing.Size(34, 24)
        Me.tsbEditUser.Text = "Bearbeiten"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 29)
        '
        'tsbSaveUser
        '
        Me.tsbSaveUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSaveUser.Enabled = False
        Me.tsbSaveUser.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSaveUser.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSaveUser.Name = "tsbSaveUser"
        Me.tsbSaveUser.Size = New System.Drawing.Size(34, 24)
        Me.tsbSaveUser.Text = "Speichen"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(6, 29)
        '
        'tsbBraeckUser
        '
        Me.tsbBraeckUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbBraeckUser.Enabled = False
        Me.tsbBraeckUser.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsbBraeckUser.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbBraeckUser.Name = "tsbBraeckUser"
        Me.tsbBraeckUser.Size = New System.Drawing.Size(34, 24)
        Me.tsbBraeckUser.Text = "Abbrechen"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(6, 29)
        '
        'tsbDelUser
        '
        Me.tsbDelUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDelUser.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbDelUser.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDelUser.Name = "tsbDelUser"
        Me.tsbDelUser.Size = New System.Drawing.Size(34, 24)
        Me.tsbDelUser.Text = "Löschen"
        '
        'tpBText
        '
        Me.tpBText.Controls.Add(Me.paBuch)
        Me.tpBText.Controls.Add(Me.liBuch)
        Me.tpBText.Controls.Add(Me.tsBuch)
        Me.tpBText.Location = New System.Drawing.Point(4, 29)
        Me.tpBText.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpBText.Name = "tpBText"
        Me.tpBText.Size = New System.Drawing.Size(1441, 879)
        Me.tpBText.TabIndex = 8
        Me.tpBText.Text = "Makros- / Buchungstexte"
        Me.tpBText.UseVisualStyleBackColor = True
        '
        'paBuch
        '
        Me.paBuch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.paBuch.Controls.Add(Me.Label62)
        Me.paBuch.Controls.Add(Me.tbZZiel)
        Me.paBuch.Controls.Add(Me.rbBuch)
        Me.paBuch.Controls.Add(Me.rbMakro)
        Me.paBuch.Controls.Add(Me.Label47)
        Me.paBuch.Controls.Add(Me.tbBuchEn)
        Me.paBuch.Controls.Add(Me.tbBuchDe)
        Me.paBuch.Controls.Add(Me.tbBez)
        Me.paBuch.Controls.Add(Me.Label45)
        Me.paBuch.Controls.Add(Me.Label46)
        Me.paBuch.Location = New System.Drawing.Point(210, 62)
        Me.paBuch.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.paBuch.Name = "paBuch"
        Me.paBuch.Size = New System.Drawing.Size(1218, 745)
        Me.paBuch.TabIndex = 10
        '
        'Label62
        '
        Me.Label62.AutoSize = True
        Me.Label62.Location = New System.Drawing.Point(698, 29)
        Me.Label62.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(100, 20)
        Me.Label62.TabIndex = 9
        Me.Label62.Text = "ZahlungsZiel"
        '
        'tbZZiel
        '
        Me.tbZZiel.Enabled = False
        Me.tbZZiel.Location = New System.Drawing.Point(860, 22)
        Me.tbZZiel.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbZZiel.Name = "tbZZiel"
        Me.tbZZiel.Size = New System.Drawing.Size(142, 26)
        Me.tbZZiel.TabIndex = 8
        '
        'rbBuch
        '
        Me.rbBuch.AutoSize = True
        Me.rbBuch.Enabled = False
        Me.rbBuch.Location = New System.Drawing.Point(506, 22)
        Me.rbBuch.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.rbBuch.Name = "rbBuch"
        Me.rbBuch.Size = New System.Drawing.Size(132, 24)
        Me.rbBuch.TabIndex = 7
        Me.rbBuch.TabStop = True
        Me.rbBuch.Text = "Buchungstext"
        Me.rbBuch.UseVisualStyleBackColor = True
        '
        'rbMakro
        '
        Me.rbMakro.AutoSize = True
        Me.rbMakro.Enabled = False
        Me.rbMakro.Location = New System.Drawing.Point(348, 22)
        Me.rbMakro.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.rbMakro.Name = "rbMakro"
        Me.rbMakro.Size = New System.Drawing.Size(78, 24)
        Me.rbMakro.TabIndex = 6
        Me.rbMakro.TabStop = True
        Me.rbMakro.Text = "Makro"
        Me.rbMakro.UseVisualStyleBackColor = True
        '
        'Label47
        '
        Me.Label47.Location = New System.Drawing.Point(24, 426)
        Me.Label47.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(150, 51)
        Me.Label47.TabIndex = 5
        Me.Label47.Text = "Buchungs-Text (Englisch)"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbBuchEn
        '
        Me.tbBuchEn.Enabled = False
        Me.tbBuchEn.Location = New System.Drawing.Point(183, 426)
        Me.tbBuchEn.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbBuchEn.Multiline = True
        Me.tbBuchEn.Name = "tbBuchEn"
        Me.tbBuchEn.Size = New System.Drawing.Size(1016, 295)
        Me.tbBuchEn.TabIndex = 4
        '
        'tbBuchDe
        '
        Me.tbBuchDe.Enabled = False
        Me.tbBuchDe.Location = New System.Drawing.Point(183, 72)
        Me.tbBuchDe.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbBuchDe.Multiline = True
        Me.tbBuchDe.Name = "tbBuchDe"
        Me.tbBuchDe.Size = New System.Drawing.Size(1016, 342)
        Me.tbBuchDe.TabIndex = 3
        '
        'tbBez
        '
        Me.tbBez.Enabled = False
        Me.tbBez.Location = New System.Drawing.Point(183, 18)
        Me.tbBez.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbBez.MaxLength = 10
        Me.tbBez.Name = "tbBez"
        Me.tbBez.Size = New System.Drawing.Size(124, 26)
        Me.tbBez.TabIndex = 2
        '
        'Label45
        '
        Me.Label45.Location = New System.Drawing.Point(24, 71)
        Me.Label45.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(150, 51)
        Me.Label45.TabIndex = 1
        Me.Label45.Text = "Buchungs-Text (Deutsch)"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label46
        '
        Me.Label46.Location = New System.Drawing.Point(24, 17)
        Me.Label46.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(150, 31)
        Me.Label46.TabIndex = 0
        Me.Label46.Text = "Bezeichnung"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'liBuch
        '
        Me.liBuch.FormattingEnabled = True
        Me.liBuch.ItemHeight = 20
        Me.liBuch.Location = New System.Drawing.Point(22, 62)
        Me.liBuch.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.liBuch.Name = "liBuch"
        Me.liBuch.Size = New System.Drawing.Size(178, 744)
        Me.liBuch.Sorted = True
        Me.liBuch.TabIndex = 9
        '
        'tsBuch
        '
        Me.tsBuch.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsBuch.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbNewBuch, Me.ToolStripSeparator24, Me.tsbEditBuch, Me.ToolStripSeparator25, Me.tsbSaveBuch, Me.ToolStripSeparator26, Me.tsbBreakBuch, Me.ToolStripSeparator27, Me.tsbDelBuch, Me.tscSprache})
        Me.tsBuch.Location = New System.Drawing.Point(0, 0)
        Me.tsBuch.Name = "tsBuch"
        Me.tsBuch.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.tsBuch.Size = New System.Drawing.Size(1441, 40)
        Me.tsBuch.TabIndex = 8
        Me.tsBuch.Text = "ToolStrip1"
        '
        'tsbNewBuch
        '
        Me.tsbNewBuch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbNewBuch.Image = Global.Pension.My.Resources.Resources.cabview_dll_Ico14_ico_Ico1
        Me.tsbNewBuch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNewBuch.Name = "tsbNewBuch"
        Me.tsbNewBuch.Size = New System.Drawing.Size(34, 35)
        Me.tsbNewBuch.Text = "Neues Makro- / Buchungstext"
        '
        'ToolStripSeparator24
        '
        Me.ToolStripSeparator24.Name = "ToolStripSeparator24"
        Me.ToolStripSeparator24.Size = New System.Drawing.Size(6, 40)
        '
        'tsbEditBuch
        '
        Me.tsbEditBuch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbEditBuch.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1474_ico_Ico1
        Me.tsbEditBuch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEditBuch.Name = "tsbEditBuch"
        Me.tsbEditBuch.Size = New System.Drawing.Size(34, 35)
        Me.tsbEditBuch.Text = "Bearbeiten"
        '
        'ToolStripSeparator25
        '
        Me.ToolStripSeparator25.Name = "ToolStripSeparator25"
        Me.ToolStripSeparator25.Size = New System.Drawing.Size(6, 40)
        '
        'tsbSaveBuch
        '
        Me.tsbSaveBuch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSaveBuch.Enabled = False
        Me.tsbSaveBuch.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSaveBuch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSaveBuch.Name = "tsbSaveBuch"
        Me.tsbSaveBuch.Size = New System.Drawing.Size(34, 35)
        Me.tsbSaveBuch.Text = "Speichen"
        '
        'ToolStripSeparator26
        '
        Me.ToolStripSeparator26.Name = "ToolStripSeparator26"
        Me.ToolStripSeparator26.Size = New System.Drawing.Size(6, 40)
        '
        'tsbBreakBuch
        '
        Me.tsbBreakBuch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbBreakBuch.Enabled = False
        Me.tsbBreakBuch.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsbBreakBuch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbBreakBuch.Name = "tsbBreakBuch"
        Me.tsbBreakBuch.Size = New System.Drawing.Size(34, 35)
        Me.tsbBreakBuch.Text = "Abbrechen"
        '
        'ToolStripSeparator27
        '
        Me.ToolStripSeparator27.Name = "ToolStripSeparator27"
        Me.ToolStripSeparator27.Size = New System.Drawing.Size(6, 40)
        '
        'tsbDelBuch
        '
        Me.tsbDelBuch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDelBuch.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbDelBuch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDelBuch.Name = "tsbDelBuch"
        Me.tsbDelBuch.Size = New System.Drawing.Size(34, 35)
        Me.tsbDelBuch.Text = "Löschen"
        '
        'tscSprache
        '
        Me.tscSprache.Name = "tscSprache"
        Me.tscSprache.Size = New System.Drawing.Size(180, 40)
        '
        'tpPreise
        '
        Me.tpPreise.Controls.Add(Me.lvPreise)
        Me.tpPreise.Controls.Add(Me.paPreise)
        Me.tpPreise.Controls.Add(Me.tsPreise)
        Me.tpPreise.Location = New System.Drawing.Point(4, 29)
        Me.tpPreise.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpPreise.Name = "tpPreise"
        Me.tpPreise.Size = New System.Drawing.Size(1441, 879)
        Me.tpPreise.TabIndex = 10
        Me.tpPreise.Text = "Preise"
        Me.tpPreise.ToolTipText = "Copy Zimmer"
        Me.tpPreise.UseVisualStyleBackColor = True
        '
        'lvPreise
        '
        Me.lvPreise.HideSelection = False
        Me.lvPreise.Location = New System.Drawing.Point(22, 62)
        Me.lvPreise.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lvPreise.Name = "lvPreise"
        Me.lvPreise.Size = New System.Drawing.Size(1403, 481)
        Me.lvPreise.TabIndex = 109
        Me.lvPreise.UseCompatibleStateImageBehavior = False
        '
        'paPreise
        '
        Me.paPreise.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.paPreise.Controls.Add(Me.coEvent)
        Me.paPreise.Controls.Add(Me.lbPZim)
        Me.paPreise.Controls.Add(Me.Label53)
        Me.paPreise.Controls.Add(Me.Label51)
        Me.paPreise.Controls.Add(Me.tbDauerG)
        Me.paPreise.Controls.Add(Me.tbPreisG)
        Me.paPreise.Controls.Add(Me.chP7)
        Me.paPreise.Controls.Add(Me.chP6)
        Me.paPreise.Controls.Add(Me.chP5)
        Me.paPreise.Controls.Add(Me.chP4)
        Me.paPreise.Controls.Add(Me.chP3)
        Me.paPreise.Controls.Add(Me.chP2)
        Me.paPreise.Controls.Add(Me.chP1)
        Me.paPreise.Controls.Add(Me.Label52)
        Me.paPreise.Controls.Add(Me.tbD7)
        Me.paPreise.Controls.Add(Me.tbD6)
        Me.paPreise.Controls.Add(Me.tbD5)
        Me.paPreise.Controls.Add(Me.tbD4)
        Me.paPreise.Controls.Add(Me.tbD3)
        Me.paPreise.Controls.Add(Me.tbD2)
        Me.paPreise.Controls.Add(Me.tbD1)
        Me.paPreise.Controls.Add(Me.tbPreis7)
        Me.paPreise.Controls.Add(Me.tbPreis6)
        Me.paPreise.Controls.Add(Me.dtpBis)
        Me.paPreise.Controls.Add(Me.dtpVon)
        Me.paPreise.Controls.Add(Me.lbPZimID)
        Me.paPreise.Controls.Add(Me.Label50)
        Me.paPreise.Controls.Add(Me.tbPreis5)
        Me.paPreise.Controls.Add(Me.tbPreis4)
        Me.paPreise.Controls.Add(Me.tbPreis3)
        Me.paPreise.Controls.Add(Me.tbPreis2)
        Me.paPreise.Controls.Add(Me.tbPreis1)
        Me.paPreise.Controls.Add(Me.lbPID)
        Me.paPreise.Controls.Add(Me.Label49)
        Me.paPreise.Controls.Add(Me.Übernachtungsart)
        Me.paPreise.Location = New System.Drawing.Point(22, 552)
        Me.paPreise.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.paPreise.Name = "paPreise"
        Me.paPreise.Size = New System.Drawing.Size(1403, 280)
        Me.paPreise.TabIndex = 96
        '
        'coEvent
        '
        Me.coEvent.FormattingEnabled = True
        Me.coEvent.Location = New System.Drawing.Point(1012, 185)
        Me.coEvent.Name = "coEvent"
        Me.coEvent.Size = New System.Drawing.Size(127, 28)
        Me.coEvent.TabIndex = 155
        '
        'lbPZim
        '
        Me.lbPZim.AutoSize = True
        Me.lbPZim.Location = New System.Drawing.Point(1010, 111)
        Me.lbPZim.Name = "lbPZim"
        Me.lbPZim.Size = New System.Drawing.Size(57, 20)
        Me.lbPZim.TabIndex = 154
        Me.lbPZim.Text = "ibPZim"
        '
        'Label53
        '
        Me.Label53.AutoSize = True
        Me.Label53.Location = New System.Drawing.Point(746, 68)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(53, 20)
        Me.Label53.TabIndex = 153
        Me.Label53.Text = "Dauer"
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Location = New System.Drawing.Point(746, 28)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(44, 20)
        Me.Label51.TabIndex = 152
        Me.Label51.Text = "Preis"
        '
        'tbDauerG
        '
        Me.tbDauerG.Location = New System.Drawing.Point(819, 63)
        Me.tbDauerG.Name = "tbDauerG"
        Me.tbDauerG.Size = New System.Drawing.Size(108, 26)
        Me.tbDauerG.TabIndex = 151
        '
        'tbPreisG
        '
        Me.tbPreisG.Location = New System.Drawing.Point(819, 23)
        Me.tbPreisG.Name = "tbPreisG"
        Me.tbPreisG.Size = New System.Drawing.Size(108, 26)
        Me.tbPreisG.TabIndex = 150
        '
        'chP7
        '
        Me.chP7.AutoSize = True
        Me.chP7.Location = New System.Drawing.Point(759, 137)
        Me.chP7.Name = "chP7"
        Me.chP7.Size = New System.Drawing.Size(55, 24)
        Me.chP7.TabIndex = 149
        Me.chP7.Text = "So"
        Me.chP7.UseVisualStyleBackColor = True
        '
        'chP6
        '
        Me.chP6.AutoSize = True
        Me.chP6.Location = New System.Drawing.Point(656, 137)
        Me.chP6.Name = "chP6"
        Me.chP6.Size = New System.Drawing.Size(55, 24)
        Me.chP6.TabIndex = 148
        Me.chP6.Text = "Sa"
        Me.chP6.UseVisualStyleBackColor = True
        '
        'chP5
        '
        Me.chP5.AutoSize = True
        Me.chP5.Location = New System.Drawing.Point(552, 137)
        Me.chP5.Name = "chP5"
        Me.chP5.Size = New System.Drawing.Size(50, 24)
        Me.chP5.TabIndex = 147
        Me.chP5.Text = "Fr"
        Me.chP5.UseVisualStyleBackColor = True
        '
        'chP4
        '
        Me.chP4.AutoSize = True
        Me.chP4.Location = New System.Drawing.Point(446, 137)
        Me.chP4.Name = "chP4"
        Me.chP4.Size = New System.Drawing.Size(56, 24)
        Me.chP4.TabIndex = 146
        Me.chP4.Text = "Do"
        Me.chP4.UseVisualStyleBackColor = True
        '
        'chP3
        '
        Me.chP3.AutoSize = True
        Me.chP3.Location = New System.Drawing.Point(339, 137)
        Me.chP3.Name = "chP3"
        Me.chP3.Size = New System.Drawing.Size(51, 24)
        Me.chP3.TabIndex = 145
        Me.chP3.Text = "Mi"
        Me.chP3.UseVisualStyleBackColor = True
        '
        'chP2
        '
        Me.chP2.AutoSize = True
        Me.chP2.Location = New System.Drawing.Point(234, 137)
        Me.chP2.Name = "chP2"
        Me.chP2.Size = New System.Drawing.Size(50, 24)
        Me.chP2.TabIndex = 144
        Me.chP2.Text = "Di"
        Me.chP2.UseVisualStyleBackColor = True
        '
        'chP1
        '
        Me.chP1.AutoSize = True
        Me.chP1.Location = New System.Drawing.Point(129, 137)
        Me.chP1.Name = "chP1"
        Me.chP1.Size = New System.Drawing.Size(57, 24)
        Me.chP1.TabIndex = 143
        Me.chP1.Text = "Mo"
        Me.chP1.UseVisualStyleBackColor = True
        '
        'Label52
        '
        Me.Label52.AutoSize = True
        Me.Label52.Location = New System.Drawing.Point(32, 237)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(53, 20)
        Me.Label52.TabIndex = 142
        Me.Label52.Text = "Dauer"
        '
        'tbD7
        '
        Me.tbD7.Enabled = False
        Me.tbD7.Location = New System.Drawing.Point(759, 232)
        Me.tbD7.Name = "tbD7"
        Me.tbD7.Size = New System.Drawing.Size(97, 26)
        Me.tbD7.TabIndex = 141
        '
        'tbD6
        '
        Me.tbD6.Enabled = False
        Me.tbD6.Location = New System.Drawing.Point(656, 232)
        Me.tbD6.Name = "tbD6"
        Me.tbD6.Size = New System.Drawing.Size(97, 26)
        Me.tbD6.TabIndex = 140
        '
        'tbD5
        '
        Me.tbD5.Enabled = False
        Me.tbD5.Location = New System.Drawing.Point(552, 232)
        Me.tbD5.Name = "tbD5"
        Me.tbD5.Size = New System.Drawing.Size(97, 26)
        Me.tbD5.TabIndex = 139
        '
        'tbD4
        '
        Me.tbD4.Enabled = False
        Me.tbD4.Location = New System.Drawing.Point(444, 232)
        Me.tbD4.Name = "tbD4"
        Me.tbD4.Size = New System.Drawing.Size(98, 26)
        Me.tbD4.TabIndex = 138
        '
        'tbD3
        '
        Me.tbD3.Enabled = False
        Me.tbD3.Location = New System.Drawing.Point(339, 232)
        Me.tbD3.Name = "tbD3"
        Me.tbD3.Size = New System.Drawing.Size(97, 26)
        Me.tbD3.TabIndex = 137
        '
        'tbD2
        '
        Me.tbD2.Enabled = False
        Me.tbD2.Location = New System.Drawing.Point(234, 232)
        Me.tbD2.Name = "tbD2"
        Me.tbD2.Size = New System.Drawing.Size(97, 26)
        Me.tbD2.TabIndex = 136
        '
        'tbD1
        '
        Me.tbD1.Enabled = False
        Me.tbD1.Location = New System.Drawing.Point(129, 232)
        Me.tbD1.Name = "tbD1"
        Me.tbD1.Size = New System.Drawing.Size(97, 26)
        Me.tbD1.TabIndex = 135
        '
        'tbPreis7
        '
        Me.tbPreis7.Enabled = False
        Me.tbPreis7.Location = New System.Drawing.Point(759, 185)
        Me.tbPreis7.Name = "tbPreis7"
        Me.tbPreis7.Size = New System.Drawing.Size(97, 26)
        Me.tbPreis7.TabIndex = 134
        '
        'tbPreis6
        '
        Me.tbPreis6.Enabled = False
        Me.tbPreis6.Location = New System.Drawing.Point(656, 185)
        Me.tbPreis6.Name = "tbPreis6"
        Me.tbPreis6.Size = New System.Drawing.Size(97, 26)
        Me.tbPreis6.TabIndex = 133
        '
        'dtpBis
        '
        Me.dtpBis.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpBis.Location = New System.Drawing.Point(374, 22)
        Me.dtpBis.Name = "dtpBis"
        Me.dtpBis.Size = New System.Drawing.Size(224, 26)
        Me.dtpBis.TabIndex = 132
        '
        'dtpVon
        '
        Me.dtpVon.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpVon.Location = New System.Drawing.Point(69, 22)
        Me.dtpVon.Name = "dtpVon"
        Me.dtpVon.Size = New System.Drawing.Size(224, 26)
        Me.dtpVon.TabIndex = 131
        '
        'lbPZimID
        '
        Me.lbPZimID.AutoSize = True
        Me.lbPZimID.Location = New System.Drawing.Point(1010, 63)
        Me.lbPZimID.Name = "lbPZimID"
        Me.lbPZimID.Size = New System.Drawing.Size(74, 20)
        Me.lbPZimID.TabIndex = 130
        Me.lbPZimID.Text = "lbPZimID"
        '
        'Label50
        '
        Me.Label50.BackColor = System.Drawing.Color.Transparent
        Me.Label50.Location = New System.Drawing.Point(336, 17)
        Me.Label50.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(51, 31)
        Me.Label50.TabIndex = 121
        Me.Label50.Text = "Bis"
        Me.Label50.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbPreis5
        '
        Me.tbPreis5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPreis5.Enabled = False
        Me.tbPreis5.Location = New System.Drawing.Point(552, 185)
        Me.tbPreis5.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbPreis5.Name = "tbPreis5"
        Me.tbPreis5.Size = New System.Drawing.Size(96, 26)
        Me.tbPreis5.TabIndex = 120
        Me.tbPreis5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbPreis4
        '
        Me.tbPreis4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPreis4.Enabled = False
        Me.tbPreis4.Location = New System.Drawing.Point(446, 185)
        Me.tbPreis4.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbPreis4.Name = "tbPreis4"
        Me.tbPreis4.Size = New System.Drawing.Size(96, 26)
        Me.tbPreis4.TabIndex = 119
        Me.tbPreis4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbPreis3
        '
        Me.tbPreis3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPreis3.Enabled = False
        Me.tbPreis3.Location = New System.Drawing.Point(339, 185)
        Me.tbPreis3.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbPreis3.Name = "tbPreis3"
        Me.tbPreis3.Size = New System.Drawing.Size(96, 26)
        Me.tbPreis3.TabIndex = 118
        Me.tbPreis3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbPreis2
        '
        Me.tbPreis2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPreis2.Enabled = False
        Me.tbPreis2.Location = New System.Drawing.Point(234, 185)
        Me.tbPreis2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbPreis2.Name = "tbPreis2"
        Me.tbPreis2.Size = New System.Drawing.Size(96, 26)
        Me.tbPreis2.TabIndex = 117
        Me.tbPreis2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbPreis1
        '
        Me.tbPreis1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPreis1.Enabled = False
        Me.tbPreis1.Location = New System.Drawing.Point(129, 185)
        Me.tbPreis1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbPreis1.Name = "tbPreis1"
        Me.tbPreis1.Size = New System.Drawing.Size(96, 26)
        Me.tbPreis1.TabIndex = 116
        Me.tbPreis1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbPID
        '
        Me.lbPID.AutoSize = True
        Me.lbPID.Location = New System.Drawing.Point(1088, 28)
        Me.lbPID.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbPID.Name = "lbPID"
        Me.lbPID.Size = New System.Drawing.Size(61, 20)
        Me.lbPID.TabIndex = 112
        Me.lbPID.Text = "PreisID"
        Me.lbPID.Visible = False
        '
        'Label49
        '
        Me.Label49.BackColor = System.Drawing.Color.Transparent
        Me.Label49.Location = New System.Drawing.Point(32, 183)
        Me.Label49.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(75, 31)
        Me.Label49.TabIndex = 92
        Me.Label49.Text = "Preise"
        Me.Label49.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Übernachtungsart
        '
        Me.Übernachtungsart.BackColor = System.Drawing.Color.Transparent
        Me.Übernachtungsart.Location = New System.Drawing.Point(14, 17)
        Me.Übernachtungsart.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Übernachtungsart.Name = "Übernachtungsart"
        Me.Übernachtungsart.Size = New System.Drawing.Size(48, 31)
        Me.Übernachtungsart.TabIndex = 79
        Me.Übernachtungsart.Text = "Von"
        Me.Übernachtungsart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tsPreise
        '
        Me.tsPreise.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsPreise.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbPNeu, Me.ToolStripSeparator20, Me.tsbPEdit, Me.ToolStripSeparator21, Me.tsbPSave, Me.ToolStripSeparator22, Me.tsbPBreak, Me.ToolStripSeparator23, Me.tsbPDel, Me.ToolStripSeparator29, Me.ToolStripSeparator44, Me.ToolStripSeparator45, Me.tsbPZimmer, Me.tsbPZimmer1, Me.tsbPCopy, Me.tsbCoJahr, Me.tsbPZimmerCopyJahr})
        Me.tsPreise.Location = New System.Drawing.Point(0, 0)
        Me.tsPreise.Name = "tsPreise"
        Me.tsPreise.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.tsPreise.Size = New System.Drawing.Size(1441, 40)
        Me.tsPreise.TabIndex = 94
        Me.tsPreise.Text = "ToolStrip1"
        '
        'tsbPNeu
        '
        Me.tsbPNeu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPNeu.Image = Global.Pension.My.Resources.Resources.cabview_dll_Ico14_ico_Ico1
        Me.tsbPNeu.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPNeu.Name = "tsbPNeu"
        Me.tsbPNeu.Size = New System.Drawing.Size(34, 35)
        Me.tsbPNeu.Text = "Neuer Preis"
        '
        'ToolStripSeparator20
        '
        Me.ToolStripSeparator20.Name = "ToolStripSeparator20"
        Me.ToolStripSeparator20.Size = New System.Drawing.Size(6, 40)
        '
        'tsbPEdit
        '
        Me.tsbPEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPEdit.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1474_ico_Ico1
        Me.tsbPEdit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPEdit.Name = "tsbPEdit"
        Me.tsbPEdit.Size = New System.Drawing.Size(34, 35)
        Me.tsbPEdit.Text = "Bearbeiten"
        '
        'ToolStripSeparator21
        '
        Me.ToolStripSeparator21.Name = "ToolStripSeparator21"
        Me.ToolStripSeparator21.Size = New System.Drawing.Size(6, 40)
        '
        'tsbPSave
        '
        Me.tsbPSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPSave.Enabled = False
        Me.tsbPSave.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbPSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPSave.Name = "tsbPSave"
        Me.tsbPSave.Size = New System.Drawing.Size(34, 35)
        Me.tsbPSave.Text = "Speichen"
        '
        'ToolStripSeparator22
        '
        Me.ToolStripSeparator22.Name = "ToolStripSeparator22"
        Me.ToolStripSeparator22.Size = New System.Drawing.Size(6, 40)
        '
        'tsbPBreak
        '
        Me.tsbPBreak.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPBreak.Enabled = False
        Me.tsbPBreak.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsbPBreak.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPBreak.Name = "tsbPBreak"
        Me.tsbPBreak.Size = New System.Drawing.Size(34, 35)
        Me.tsbPBreak.Text = "Abbrechen"
        '
        'ToolStripSeparator23
        '
        Me.ToolStripSeparator23.Name = "ToolStripSeparator23"
        Me.ToolStripSeparator23.Size = New System.Drawing.Size(6, 40)
        '
        'tsbPDel
        '
        Me.tsbPDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPDel.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbPDel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPDel.Name = "tsbPDel"
        Me.tsbPDel.Size = New System.Drawing.Size(34, 35)
        Me.tsbPDel.Text = "Löschen"
        '
        'ToolStripSeparator29
        '
        Me.ToolStripSeparator29.Name = "ToolStripSeparator29"
        Me.ToolStripSeparator29.Size = New System.Drawing.Size(6, 40)
        '
        'ToolStripSeparator44
        '
        Me.ToolStripSeparator44.Name = "ToolStripSeparator44"
        Me.ToolStripSeparator44.Size = New System.Drawing.Size(6, 40)
        '
        'ToolStripSeparator45
        '
        Me.ToolStripSeparator45.Name = "ToolStripSeparator45"
        Me.ToolStripSeparator45.Size = New System.Drawing.Size(6, 40)
        '
        'tsbPZimmer
        '
        Me.tsbPZimmer.Name = "tsbPZimmer"
        Me.tsbPZimmer.Size = New System.Drawing.Size(136, 40)
        '
        'tsbPZimmer1
        '
        Me.tsbPZimmer1.Name = "tsbPZimmer1"
        Me.tsbPZimmer1.Size = New System.Drawing.Size(136, 40)
        '
        'tsbPCopy
        '
        Me.tsbPCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPCopy.Image = Global.Pension.My.Resources.Resources.MOVER
        Me.tsbPCopy.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPCopy.Name = "tsbPCopy"
        Me.tsbPCopy.Size = New System.Drawing.Size(34, 35)
        Me.tsbPCopy.Text = "tsbPCopy"
        Me.tsbPCopy.ToolTipText = "Copy Zimmer"
        '
        'tsbCoJahr
        '
        Me.tsbCoJahr.Name = "tsbCoJahr"
        Me.tsbCoJahr.Size = New System.Drawing.Size(136, 40)
        '
        'tsbPZimmerCopyJahr
        '
        Me.tsbPZimmerCopyJahr.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPZimmerCopyJahr.Image = Global.Pension.My.Resources.Resources.ARW05UP
        Me.tsbPZimmerCopyJahr.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPZimmerCopyJahr.Name = "tsbPZimmerCopyJahr"
        Me.tsbPZimmerCopyJahr.Size = New System.Drawing.Size(34, 35)
        Me.tsbPZimmerCopyJahr.Text = "ToolStripButton8"
        Me.tsbPZimmerCopyJahr.ToolTipText = "Preis in das näste Jagr"
        '
        'tpSasion
        '
        Me.tpSasion.Controls.Add(Me.Panel1)
        Me.tpSasion.Controls.Add(Me.tsSaison)
        Me.tpSasion.Controls.Add(Me.lvSaison)
        Me.tpSasion.Location = New System.Drawing.Point(4, 29)
        Me.tpSasion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpSasion.Name = "tpSasion"
        Me.tpSasion.Size = New System.Drawing.Size(1441, 879)
        Me.tpSasion.TabIndex = 11
        Me.tpSasion.Text = "Sasion"
        Me.tpSasion.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.mcSaisonEnde)
        Me.Panel1.Controls.Add(Me.mcSaisonAnfang)
        Me.Panel1.Controls.Add(Me.coSaison)
        Me.Panel1.Location = New System.Drawing.Point(546, 77)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(689, 544)
        Me.Panel1.TabIndex = 95
        '
        'mcSaisonEnde
        '
        Me.mcSaisonEnde.Location = New System.Drawing.Point(406, 278)
        Me.mcSaisonEnde.Margin = New System.Windows.Forms.Padding(14)
        Me.mcSaisonEnde.MaxSelectionCount = 1
        Me.mcSaisonEnde.Name = "mcSaisonEnde"
        Me.mcSaisonEnde.TabIndex = 92
        '
        'mcSaisonAnfang
        '
        Me.mcSaisonAnfang.Location = New System.Drawing.Point(14, 278)
        Me.mcSaisonAnfang.Margin = New System.Windows.Forms.Padding(14)
        Me.mcSaisonAnfang.MaxSelectionCount = 1
        Me.mcSaisonAnfang.Name = "mcSaisonAnfang"
        Me.mcSaisonAnfang.TabIndex = 91
        '
        'coSaison
        '
        Me.coSaison.FormattingEnabled = True
        Me.coSaison.Items.AddRange(New Object() {"11 Feiertage 3", "10 Feiertage 2", "09 Feiertage 1", "08 Haupt Saison", "07 Neben Saison 2", "06 Neben Saison 1", "05 Ferien 5", "04 Ferien 4", "03 Ferien 3", "02 Ferien 2", "01 Ferien 1"})
        Me.coSaison.Location = New System.Drawing.Point(14, 20)
        Me.coSaison.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.coSaison.Name = "coSaison"
        Me.coSaison.Size = New System.Drawing.Size(265, 28)
        Me.coSaison.TabIndex = 90
        '
        'tsSaison
        '
        Me.tsSaison.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsSaison.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbNeuSaison, Me.ToolStripSeparator30, Me.tsbEditSaison, Me.ToolStripSeparator31, Me.tsbSaveSaison, Me.ToolStripSeparator32, Me.tsbBreackSaison, Me.ToolStripSeparator33, Me.tsbDelSaison})
        Me.tsSaison.Location = New System.Drawing.Point(0, 0)
        Me.tsSaison.Name = "tsSaison"
        Me.tsSaison.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.tsSaison.Size = New System.Drawing.Size(1441, 29)
        Me.tsSaison.TabIndex = 94
        Me.tsSaison.Text = "ToolStrip1"
        '
        'tsbNeuSaison
        '
        Me.tsbNeuSaison.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbNeuSaison.Image = Global.Pension.My.Resources.Resources.cabview_dll_Ico14_ico_Ico1
        Me.tsbNeuSaison.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNeuSaison.Name = "tsbNeuSaison"
        Me.tsbNeuSaison.Size = New System.Drawing.Size(34, 24)
        Me.tsbNeuSaison.ToolTipText = "Neues Objekt"
        '
        'ToolStripSeparator30
        '
        Me.ToolStripSeparator30.Name = "ToolStripSeparator30"
        Me.ToolStripSeparator30.Size = New System.Drawing.Size(6, 29)
        '
        'tsbEditSaison
        '
        Me.tsbEditSaison.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbEditSaison.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1474_ico_Ico1
        Me.tsbEditSaison.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEditSaison.Name = "tsbEditSaison"
        Me.tsbEditSaison.Size = New System.Drawing.Size(34, 24)
        Me.tsbEditSaison.Text = "Bearbeiten"
        '
        'ToolStripSeparator31
        '
        Me.ToolStripSeparator31.Name = "ToolStripSeparator31"
        Me.ToolStripSeparator31.Size = New System.Drawing.Size(6, 29)
        '
        'tsbSaveSaison
        '
        Me.tsbSaveSaison.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSaveSaison.Enabled = False
        Me.tsbSaveSaison.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbSaveSaison.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSaveSaison.Name = "tsbSaveSaison"
        Me.tsbSaveSaison.Size = New System.Drawing.Size(34, 24)
        Me.tsbSaveSaison.Text = "Speichen"
        '
        'ToolStripSeparator32
        '
        Me.ToolStripSeparator32.Name = "ToolStripSeparator32"
        Me.ToolStripSeparator32.Size = New System.Drawing.Size(6, 29)
        '
        'tsbBreackSaison
        '
        Me.tsbBreackSaison.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbBreackSaison.Enabled = False
        Me.tsbBreackSaison.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsbBreackSaison.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbBreackSaison.Name = "tsbBreackSaison"
        Me.tsbBreackSaison.Size = New System.Drawing.Size(34, 24)
        Me.tsbBreackSaison.Text = "Abbrechen"
        '
        'ToolStripSeparator33
        '
        Me.ToolStripSeparator33.Name = "ToolStripSeparator33"
        Me.ToolStripSeparator33.Size = New System.Drawing.Size(6, 29)
        '
        'tsbDelSaison
        '
        Me.tsbDelSaison.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDelSaison.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbDelSaison.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDelSaison.Name = "tsbDelSaison"
        Me.tsbDelSaison.Size = New System.Drawing.Size(34, 24)
        Me.tsbDelSaison.Text = "Löschen"
        '
        'lvSaison
        '
        Me.lvSaison.HideSelection = False
        Me.lvSaison.Location = New System.Drawing.Point(20, 77)
        Me.lvSaison.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lvSaison.Name = "lvSaison"
        Me.lvSaison.Size = New System.Drawing.Size(516, 542)
        Me.lvSaison.TabIndex = 93
        Me.lvSaison.UseCompatibleStateImageBehavior = False
        '
        'tpFarben
        '
        Me.tpFarben.Controls.Add(Me.btSaveColor)
        Me.tpFarben.Controls.Add(Me.lbBackColor)
        Me.tpFarben.Controls.Add(Me.btBackColor)
        Me.tpFarben.Controls.Add(Me.btForeColor)
        Me.tpFarben.Controls.Add(Me.lvColor)
        Me.tpFarben.Location = New System.Drawing.Point(4, 29)
        Me.tpFarben.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpFarben.Name = "tpFarben"
        Me.tpFarben.Size = New System.Drawing.Size(1441, 879)
        Me.tpFarben.TabIndex = 12
        Me.tpFarben.Text = "Farben"
        Me.tpFarben.UseVisualStyleBackColor = True
        '
        'btSaveColor
        '
        Me.btSaveColor.Enabled = False
        Me.btSaveColor.Location = New System.Drawing.Point(549, 114)
        Me.btSaveColor.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btSaveColor.Name = "btSaveColor"
        Me.btSaveColor.Size = New System.Drawing.Size(284, 35)
        Me.btSaveColor.TabIndex = 94
        Me.btSaveColor.Text = "Speichern"
        Me.btSaveColor.UseVisualStyleBackColor = True
        '
        'lbBackColor
        '
        Me.lbBackColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbBackColor.Location = New System.Drawing.Point(549, 62)
        Me.lbBackColor.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbBackColor.Name = "lbBackColor"
        Me.lbBackColor.Size = New System.Drawing.Size(282, 34)
        Me.lbBackColor.TabIndex = 93
        Me.lbBackColor.Text = "Beispieltext"
        Me.lbBackColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btBackColor
        '
        Me.btBackColor.ForeColor = System.Drawing.Color.Black
        Me.btBackColor.Location = New System.Drawing.Point(256, 62)
        Me.btBackColor.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btBackColor.Name = "btBackColor"
        Me.btBackColor.Size = New System.Drawing.Size(284, 35)
        Me.btBackColor.TabIndex = 92
        Me.btBackColor.Text = "Hintergrundfarbe"
        Me.btBackColor.UseVisualStyleBackColor = True
        '
        'btForeColor
        '
        Me.btForeColor.ForeColor = System.Drawing.Color.White
        Me.btForeColor.Location = New System.Drawing.Point(256, 114)
        Me.btForeColor.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btForeColor.Name = "btForeColor"
        Me.btForeColor.Size = New System.Drawing.Size(284, 35)
        Me.btForeColor.TabIndex = 91
        Me.btForeColor.Text = "Schriftfarbe"
        Me.btForeColor.UseVisualStyleBackColor = True
        '
        'lvColor
        '
        Me.lvColor.HideSelection = False
        Me.lvColor.Location = New System.Drawing.Point(22, 62)
        Me.lvColor.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lvColor.Name = "lvColor"
        Me.lvColor.Size = New System.Drawing.Size(204, 530)
        Me.lvColor.TabIndex = 9
        Me.lvColor.UseCompatibleStateImageBehavior = False
        '
        'tpDruck
        '
        Me.tpDruck.Controls.Add(Me.lvDruck)
        Me.tpDruck.Controls.Add(Me.tsDruck)
        Me.tpDruck.Location = New System.Drawing.Point(4, 29)
        Me.tpDruck.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpDruck.Name = "tpDruck"
        Me.tpDruck.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpDruck.Size = New System.Drawing.Size(1441, 879)
        Me.tpDruck.TabIndex = 14
        Me.tpDruck.Text = "DruckProfil"
        Me.tpDruck.UseVisualStyleBackColor = True
        '
        'lvDruck
        '
        Me.lvDruck.CheckBoxes = True
        Me.lvDruck.HideSelection = False
        Me.lvDruck.Location = New System.Drawing.Point(9, 63)
        Me.lvDruck.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lvDruck.Name = "lvDruck"
        Me.lvDruck.Size = New System.Drawing.Size(1409, 795)
        Me.lvDruck.TabIndex = 1
        Me.lvDruck.UseCompatibleStateImageBehavior = False
        '
        'tsDruck
        '
        Me.tsDruck.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsDruck.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbDruckSave, Me.ToolStripSeparator39, Me.tsbDruckDel, Me.ToolStripSeparator38, Me.tsbcbDruck})
        Me.tsDruck.Location = New System.Drawing.Point(4, 5)
        Me.tsDruck.Name = "tsDruck"
        Me.tsDruck.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.tsDruck.Size = New System.Drawing.Size(1433, 40)
        Me.tsDruck.TabIndex = 0
        Me.tsDruck.Text = "ToolStrip2"
        '
        'tsbDruckSave
        '
        Me.tsbDruckSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDruckSave.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsbDruckSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDruckSave.Name = "tsbDruckSave"
        Me.tsbDruckSave.Size = New System.Drawing.Size(34, 35)
        Me.tsbDruckSave.Text = "ToolStripButton5"
        Me.tsbDruckSave.ToolTipText = "Save"
        '
        'ToolStripSeparator39
        '
        Me.ToolStripSeparator39.Name = "ToolStripSeparator39"
        Me.ToolStripSeparator39.Size = New System.Drawing.Size(6, 40)
        '
        'tsbDruckDel
        '
        Me.tsbDruckDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDruckDel.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsbDruckDel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDruckDel.Name = "tsbDruckDel"
        Me.tsbDruckDel.Size = New System.Drawing.Size(34, 35)
        Me.tsbDruckDel.Text = "ToolStripButton3"
        '
        'ToolStripSeparator38
        '
        Me.ToolStripSeparator38.Name = "ToolStripSeparator38"
        Me.ToolStripSeparator38.Size = New System.Drawing.Size(6, 40)
        '
        'tsbcbDruck
        '
        Me.tsbcbDruck.Name = "tsbcbDruck"
        Me.tsbcbDruck.Size = New System.Drawing.Size(180, 40)
        '
        'tpSprache
        '
        Me.tpSprache.Controls.Add(Me.ToolStrip3)
        Me.tpSprache.Controls.Add(Me.dgvSprache)
        Me.tpSprache.Controls.Add(Me.cbText)
        Me.tpSprache.Location = New System.Drawing.Point(4, 29)
        Me.tpSprache.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpSprache.Name = "tpSprache"
        Me.tpSprache.Size = New System.Drawing.Size(1441, 879)
        Me.tpSprache.TabIndex = 17
        Me.tpSprache.Text = "Sprache"
        Me.tpSprache.UseVisualStyleBackColor = True
        '
        'ToolStrip3
        '
        Me.ToolStrip3.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ToolStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsSpracheVerw, Me.tsSpracheSave, Me.tsSpracheDel})
        Me.ToolStrip3.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip3.Name = "ToolStrip3"
        Me.ToolStrip3.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.ToolStrip3.Size = New System.Drawing.Size(1441, 29)
        Me.ToolStrip3.TabIndex = 2
        Me.ToolStrip3.Text = "ToolStrip3"
        '
        'tsSpracheVerw
        '
        Me.tsSpracheVerw.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsSpracheVerw.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico1491_ico_Ico1
        Me.tsSpracheVerw.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsSpracheVerw.Name = "tsSpracheVerw"
        Me.tsSpracheVerw.Size = New System.Drawing.Size(34, 24)
        Me.tsSpracheVerw.Text = "ToolStripButton9"
        '
        'tsSpracheSave
        '
        Me.tsSpracheSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsSpracheSave.Image = Global.Pension.My.Resources.Resources.ntbackup_exe_Ico8_ico_Ico1
        Me.tsSpracheSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsSpracheSave.Name = "tsSpracheSave"
        Me.tsSpracheSave.Size = New System.Drawing.Size(34, 24)
        Me.tsSpracheSave.Text = "ToolStripButton10"
        '
        'tsSpracheDel
        '
        Me.tsSpracheDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsSpracheDel.Image = Global.Pension.My.Resources.Resources.shell32_dll_Ico65_ico_Ico1
        Me.tsSpracheDel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsSpracheDel.Name = "tsSpracheDel"
        Me.tsSpracheDel.Size = New System.Drawing.Size(34, 24)
        Me.tsSpracheDel.Text = "ToolStripButton7"
        '
        'dgvSprache
        '
        Me.dgvSprache.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSprache.Location = New System.Drawing.Point(17, 112)
        Me.dgvSprache.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvSprache.Name = "dgvSprache"
        Me.dgvSprache.RowHeadersWidth = 62
        Me.dgvSprache.Size = New System.Drawing.Size(1406, 746)
        Me.dgvSprache.TabIndex = 1
        '
        'cbText
        '
        Me.cbText.FormattingEnabled = True
        Me.cbText.Location = New System.Drawing.Point(22, 51)
        Me.cbText.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cbText.Name = "cbText"
        Me.cbText.Size = New System.Drawing.Size(1218, 28)
        Me.cbText.TabIndex = 0
        '
        'frmSystem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ClientSize = New System.Drawing.Size(1478, 972)
        Me.ControlBox = False
        Me.Controls.Add(Me.tcSystem)
        Me.Controls.Add(Me.ToolStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmSystem"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Systemeinstellungen"
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.tpEMail.ResumeLayout(False)
        Me.gpEMail.ResumeLayout(False)
        Me.gpEMail.PerformLayout()
        Me.tpAnschrift.ResumeLayout(False)
        Me.gbBasisdaten.ResumeLayout(False)
        Me.gbBasisdaten.PerformLayout()
        Me.gpDir.ResumeLayout(False)
        Me.gpDir.PerformLayout()
        Me.gpDiverses.ResumeLayout(False)
        Me.gpDiverses.PerformLayout()
        Me.tcSystem.ResumeLayout(False)
        Me.tpKonto.ResumeLayout(False)
        Me.tpKonto.PerformLayout()
        Me.paKonto.ResumeLayout(False)
        Me.paKonto.PerformLayout()
        Me.tsKonto.ResumeLayout(False)
        Me.tsKonto.PerformLayout()
        Me.tpObjekte.ResumeLayout(False)
        Me.tpObjekte.PerformLayout()
        Me.paObjekt.ResumeLayout(False)
        Me.paObjekt.PerformLayout()
        Me.tsObjekt.ResumeLayout(False)
        Me.tsObjekt.PerformLayout()
        Me.tpZimmer.ResumeLayout(False)
        Me.tpZimmer.PerformLayout()
        Me.paZimmer.ResumeLayout(False)
        Me.paZimmer.PerformLayout()
        Me.tsZimmer.ResumeLayout(False)
        Me.tsZimmer.PerformLayout()
        Me.tpUser.ResumeLayout(False)
        Me.tpUser.PerformLayout()
        Me.paUser.ResumeLayout(False)
        Me.paUser.PerformLayout()
        Me.tsUser.ResumeLayout(False)
        Me.tsUser.PerformLayout()
        Me.tpBText.ResumeLayout(False)
        Me.tpBText.PerformLayout()
        Me.paBuch.ResumeLayout(False)
        Me.paBuch.PerformLayout()
        Me.tsBuch.ResumeLayout(False)
        Me.tsBuch.PerformLayout()
        Me.tpPreise.ResumeLayout(False)
        Me.tpPreise.PerformLayout()
        Me.paPreise.ResumeLayout(False)
        Me.paPreise.PerformLayout()
        Me.tsPreise.ResumeLayout(False)
        Me.tsPreise.PerformLayout()
        Me.tpSasion.ResumeLayout(False)
        Me.tpSasion.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.tsSaison.ResumeLayout(False)
        Me.tsSaison.PerformLayout()
        Me.tpFarben.ResumeLayout(False)
        Me.tpDruck.ResumeLayout(False)
        Me.tpDruck.PerformLayout()
        Me.tsDruck.ResumeLayout(False)
        Me.tsDruck.PerformLayout()
        Me.tpSprache.ResumeLayout(False)
        Me.tpSprache.PerformLayout()
        Me.ToolStrip3.ResumeLayout(False)
        Me.ToolStrip3.PerformLayout()
        CType(Me.dgvSprache, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSpeichern As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbESC As System.Windows.Forms.ToolStripButton
    Friend WithEvents fbDialog As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents tsbLableUpgrade As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tpEMail As System.Windows.Forms.TabPage
    Friend WithEvents gpEMail As System.Windows.Forms.GroupBox
    Friend WithEvents cmdTestEMail As System.Windows.Forms.Button
    Friend WithEvents tbTage As System.Windows.Forms.TextBox
    Friend WithEvents tbPWort As System.Windows.Forms.TextBox
    Friend WithEvents tbUName As System.Windows.Forms.TextBox
    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents tbEMail As System.Windows.Forms.TextBox
    Friend WithEvents rbUU As System.Windows.Forms.RadioButton
    Friend WithEvents rbMIME As System.Windows.Forms.RadioButton
    Friend WithEvents chHTML As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chServer As System.Windows.Forms.CheckBox
    Friend WithEvents lbTage As System.Windows.Forms.Label
    Friend WithEvents lbPWort As System.Windows.Forms.Label
    Friend WithEvents lbUName As System.Windows.Forms.Label
    Friend WithEvents lbName As System.Windows.Forms.Label
    Friend WithEvents lbEMail As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lbLableUser As System.Windows.Forms.Label
    Friend WithEvents lbLablePWort As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents tbSMTP As System.Windows.Forms.TextBox
    Friend WithEvents lbSMTP As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents tpAnschrift As System.Windows.Forms.TabPage
    Friend WithEvents gpDir As System.Windows.Forms.GroupBox
    Friend WithEvents cmdAblageDir As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tbAblageDir As System.Windows.Forms.TextBox
    Friend WithEvents lbAblageDir As System.Windows.Forms.Label
    Friend WithEvents gpDiverses As System.Windows.Forms.GroupBox
    Friend WithEvents gbBasisdaten As System.Windows.Forms.GroupBox
    Friend WithEvents tbOrt As System.Windows.Forms.TextBox
    Friend WithEvents lbOrt As System.Windows.Forms.Label
    Friend WithEvents tbPLZ As System.Windows.Forms.TextBox
    Friend WithEvents lbPLZ As System.Windows.Forms.Label
    Friend WithEvents tbStrasse As System.Windows.Forms.TextBox
    Friend WithEvents lbStrasse As System.Windows.Forms.Label
    Friend WithEvents tbFirma As System.Windows.Forms.TextBox
    Friend WithEvents lbFirma As System.Windows.Forms.Label
    Friend WithEvents lbLabelOrt As System.Windows.Forms.Label
    Friend WithEvents lbLabelPLZ As System.Windows.Forms.Label
    Friend WithEvents lbLabelStr As System.Windows.Forms.Label
    Friend WithEvents lbLabelGebName As System.Windows.Forms.Label
    Friend WithEvents tcSystem As System.Windows.Forms.TabControl
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmdSaveDir As System.Windows.Forms.Button
    Friend WithEvents tbSaveDir As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbSaveDir As System.Windows.Forms.Label
    Friend WithEvents tsbEdit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tpKonto As System.Windows.Forms.TabPage
    Friend WithEvents tpZimmer As System.Windows.Forms.TabPage
    Friend WithEvents lvZimmer As System.Windows.Forms.ListView
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents tbTel4 As System.Windows.Forms.TextBox
    Friend WithEvents lbTel4 As System.Windows.Forms.Label
    Friend WithEvents tbTel3 As System.Windows.Forms.TextBox
    Friend WithEvents lbTel3 As System.Windows.Forms.Label
    Friend WithEvents tbTel2 As System.Windows.Forms.TextBox
    Friend WithEvents lbTel2 As System.Windows.Forms.Label
    Friend WithEvents tbTel1 As System.Windows.Forms.TextBox
    Friend WithEvents lbTel1 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents tsZimmer As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbNeuZim As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbEditZim As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbSaveZim As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbBraekZim As System.Windows.Forms.ToolStripButton
    Friend WithEvents tpObjekte As System.Windows.Forms.TabPage
    Friend WithEvents lvObjekt As System.Windows.Forms.ListView
    Friend WithEvents tsObjekt As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbNewObj As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbEditObj As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbSaveObj As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbBraeckObj As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbDatevDir As System.Windows.Forms.TextBox
    Friend WithEvents lbDatevDir As System.Windows.Forms.Label
    Friend WithEvents cmdDatevDir As System.Windows.Forms.Button
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents lbBNr As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents lbKNr As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents tbMwst2 As System.Windows.Forms.TextBox
    Friend WithEvents lbMwst2 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents tpUser As System.Windows.Forms.TabPage
    Friend WithEvents tsUser As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbNewUser As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbEditUser As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbSaveUser As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbBraeckUser As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsKonto As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbNewKonto As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbEditKonto As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbSaveKonto As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbBreakKonto As System.Windows.Forms.ToolStripButton
    Friend WithEvents lvKonto As System.Windows.Forms.ListView
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tbIBAN As System.Windows.Forms.TextBox
    Friend WithEvents tbInstitut As System.Windows.Forms.TextBox
    Friend WithEvents tbBLZ As System.Windows.Forms.TextBox
    Friend WithEvents tbKTO As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents tbBIC As System.Windows.Forms.TextBox
    Friend WithEvents tbKZ As System.Windows.Forms.TextBox
    Friend WithEvents tbTyp As System.Windows.Forms.TextBox
    Friend WithEvents tsbDelKonto As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator19 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDelObj As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator18 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDelZim As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator17 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDelUser As System.Windows.Forms.ToolStripButton
    Friend WithEvents tpBText As System.Windows.Forms.TabPage
    Friend WithEvents paKonto As System.Windows.Forms.Panel
    Friend WithEvents paObjekt As System.Windows.Forms.Panel
    Friend WithEvents tbOHNr As System.Windows.Forms.TextBox
    Friend WithEvents tbOOrtsteil As System.Windows.Forms.TextBox
    Friend WithEvents tbOOrt As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents tbOPLZ As System.Windows.Forms.TextBox
    Friend WithEvents tbOTelefon As System.Windows.Forms.TextBox
    Friend WithEvents tbOStr As System.Windows.Forms.TextBox
    Friend WithEvents tbOName As System.Windows.Forms.TextBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents lbObjektID As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents tbUStID As System.Windows.Forms.TextBox
    Friend WithEvents tbUStNr As System.Windows.Forms.TextBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents lbUStID As System.Windows.Forms.Label
    Friend WithEvents lbUStNr As System.Windows.Forms.Label
    Friend WithEvents paZimmer As System.Windows.Forms.Panel
    Friend WithEvents lbZimmerID As System.Windows.Forms.Label
    Friend WithEvents tbZAus As System.Windows.Forms.TextBox
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents tbZArt As System.Windows.Forms.TextBox
    Friend WithEvents tbZName As System.Windows.Forms.TextBox
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents coObjekt As System.Windows.Forms.ComboBox
    Friend WithEvents paUser As System.Windows.Forms.Panel
    Friend WithEvents lbUserID As System.Windows.Forms.Label
    Friend WithEvents tbUPassWD As System.Windows.Forms.TextBox
    Friend WithEvents tbUUser As System.Windows.Forms.TextBox
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents lvUser As System.Windows.Forms.ListView
    Friend WithEvents chKlar As System.Windows.Forms.CheckBox
    Friend WithEvents paBuch As System.Windows.Forms.Panel
    Friend WithEvents tbBuchDe As System.Windows.Forms.TextBox
    Friend WithEvents tbBez As System.Windows.Forms.TextBox
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents liBuch As System.Windows.Forms.ListBox
    Friend WithEvents tsBuch As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbNewBuch As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator24 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbEditBuch As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator25 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbSaveBuch As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator26 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbBreakBuch As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator27 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDelBuch As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents tbBuchEn As System.Windows.Forms.TextBox
    Friend WithEvents tbKUser As System.Windows.Forms.TextBox
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents lbKontoID As System.Windows.Forms.Label
    Friend WithEvents rbBuch As System.Windows.Forms.RadioButton
    Friend WithEvents rbMakro As System.Windows.Forms.RadioButton
    Friend WithEvents tbZBetten As System.Windows.Forms.TextBox
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents tpPreise As System.Windows.Forms.TabPage
    Friend WithEvents paPreise As System.Windows.Forms.Panel
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents Übernachtungsart As System.Windows.Forms.Label
    Friend WithEvents tsPreise As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbPNeu As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator20 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbPEdit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator21 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbPSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator22 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbPBreak As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator23 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbPDel As System.Windows.Forms.ToolStripButton
    Friend WithEvents lvPreise As System.Windows.Forms.ListView
    Friend WithEvents lbPID As System.Windows.Forms.Label
    Friend WithEvents tbPreis1 As System.Windows.Forms.TextBox
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents tbPreis5 As System.Windows.Forms.TextBox
    Friend WithEvents tbPreis4 As System.Windows.Forms.TextBox
    Friend WithEvents tbPreis3 As System.Windows.Forms.TextBox
    Friend WithEvents tbPreis2 As System.Windows.Forms.TextBox
    Friend WithEvents coZArt As System.Windows.Forms.ComboBox
    Friend WithEvents ToolStripSeparator28 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmZimmer As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmImportZimmer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmExportZimmer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator29 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tpSasion As System.Windows.Forms.TabPage
    Friend WithEvents lvSaison As System.Windows.Forms.ListView
    Friend WithEvents tsSaison As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbNeuSaison As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator30 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbEditSaison As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator31 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbSaveSaison As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator32 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbBreackSaison As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator33 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDelSaison As System.Windows.Forms.ToolStripButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents coSaison As System.Windows.Forms.ComboBox
    Friend WithEvents mcSaisonEnde As System.Windows.Forms.MonthCalendar
    Friend WithEvents mcSaisonAnfang As System.Windows.Forms.MonthCalendar
    Friend WithEvents tbBK As System.Windows.Forms.TextBox
    Friend WithEvents lbBK As System.Windows.Forms.Label
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents tbKK As System.Windows.Forms.TextBox
    Friend WithEvents lbKK As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btColorObjekt As System.Windows.Forms.Button
    Friend WithEvents lbRGBString As System.Windows.Forms.Label
    Friend WithEvents chFeWo As System.Windows.Forms.CheckBox
    Friend WithEvents tpFarben As System.Windows.Forms.TabPage
    Friend WithEvents lvColor As System.Windows.Forms.ListView
    Friend WithEvents btBackColor As System.Windows.Forms.Button
    Friend WithEvents btForeColor As System.Windows.Forms.Button
    Friend WithEvents lbBackColor As System.Windows.Forms.Label
    Friend WithEvents btSaveColor As System.Windows.Forms.Button
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents tbWeb As System.Windows.Forms.TextBox
    Friend WithEvents lbWeb As System.Windows.Forms.Label
    Friend WithEvents tpDruck As System.Windows.Forms.TabPage
    Friend WithEvents tsDruck As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbDruckSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator38 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbcbDruck As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents lvDruck As System.Windows.Forms.ListView
    Friend WithEvents ToolStripSeparator39 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbDruckDel As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label62 As System.Windows.Forms.Label
    Friend WithEvents tbZZiel As System.Windows.Forms.TextBox
    Friend WithEvents tbRFIDPort As System.Windows.Forms.TextBox
    Friend WithEvents tbIPSchloss As System.Windows.Forms.TextBox
    Friend WithEvents tbNetUser As System.Windows.Forms.TextBox
    Friend WithEvents Label67 As System.Windows.Forms.Label
    Friend WithEvents Label66 As System.Windows.Forms.Label
    Friend WithEvents Label65 As System.Windows.Forms.Label
    Friend WithEvents lbRFIDPort As System.Windows.Forms.Label
    Friend WithEvents lbIPSchloss As System.Windows.Forms.Label
    Friend WithEvents lbNetUser As System.Windows.Forms.Label
    Friend WithEvents tscSprache As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tpSprache As System.Windows.Forms.TabPage
    Friend WithEvents ToolStrip3 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsSpracheVerw As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsSpracheSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents dgvSprache As System.Windows.Forms.DataGridView
    Friend WithEvents cbText As System.Windows.Forms.ComboBox
    Friend WithEvents tbZNummer As System.Windows.Forms.TextBox
    Friend WithEvents tbRNr As System.Windows.Forms.TextBox
    Friend WithEvents lbRNr As System.Windows.Forms.Label
    Friend WithEvents tbZP10 As TextBox
    Friend WithEvents tbZP9 As TextBox
    Friend WithEvents tbZP8 As TextBox
    Friend WithEvents tbZP7 As TextBox
    Friend WithEvents tbZP6 As TextBox
    Friend WithEvents tbZP5 As TextBox
    Friend WithEvents tbZP4 As TextBox
    Friend WithEvents tbZP3 As TextBox
    Friend WithEvents tbZP2 As TextBox
    Friend WithEvents tbZP1 As TextBox
    Friend WithEvents Label90 As Label
    Friend WithEvents Label89 As Label
    Friend WithEvents Label88 As Label
    Friend WithEvents Label87 As Label
    Friend WithEvents Label86 As Label
    Friend WithEvents Label85 As Label
    Friend WithEvents Label84 As Label
    Friend WithEvents Label83 As Label
    Friend WithEvents Label82 As Label
    Friend WithEvents Label81 As Label
    Friend WithEvents Label80 As Label
    Friend WithEvents tbZBettenKi As TextBox
    Friend WithEvents tbZBettenEr As TextBox
    Friend WithEvents tbZBettenMin As TextBox
    Friend WithEvents Label79 As Label
    Friend WithEvents Label78 As Label
    Friend WithEvents Label77 As Label
    Friend WithEvents ToolStripSeparator44 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator45 As ToolStripSeparator
    Friend WithEvents tsbPZimmer As ToolStripComboBox
    Friend WithEvents lbPZimID As Label
    Friend WithEvents Label52 As Label
    Friend WithEvents tbD7 As TextBox
    Friend WithEvents tbD6 As TextBox
    Friend WithEvents tbD5 As TextBox
    Friend WithEvents tbD4 As TextBox
    Friend WithEvents tbD3 As TextBox
    Friend WithEvents tbD2 As TextBox
    Friend WithEvents tbD1 As TextBox
    Friend WithEvents tbPreis7 As TextBox
    Friend WithEvents tbPreis6 As TextBox
    Friend WithEvents dtpBis As DateTimePicker
    Friend WithEvents dtpVon As DateTimePicker
    Friend WithEvents chP7 As CheckBox
    Friend WithEvents chP6 As CheckBox
    Friend WithEvents chP5 As CheckBox
    Friend WithEvents chP4 As CheckBox
    Friend WithEvents chP3 As CheckBox
    Friend WithEvents chP2 As CheckBox
    Friend WithEvents chP1 As CheckBox
    Friend WithEvents Label53 As Label
    Friend WithEvents Label51 As Label
    Friend WithEvents tbDauerG As TextBox
    Friend WithEvents tbPreisG As TextBox
    Friend WithEvents lbPZim As Label
    Friend WithEvents Label92 As Label
    Friend WithEvents Label91 As Label
    Friend WithEvents Label57 As Label
    Friend WithEvents Label56 As Label
    Friend WithEvents Label55 As Label
    Friend WithEvents tbTrans5 As TextBox
    Friend WithEvents tbTrans4 As TextBox
    Friend WithEvents tbTrans3 As TextBox
    Friend WithEvents tbTrans2 As TextBox
    Friend WithEvents tbTrans1 As TextBox
    Friend WithEvents coEvent As ComboBox
    Friend WithEvents tsbPZimmer1 As ToolStripComboBox
    Friend WithEvents tsbPCopy As ToolStripButton
    Friend WithEvents tsbCoJahr As ToolStripComboBox
    Friend WithEvents tsbPZimmerCopyJahr As ToolStripButton
    Friend WithEvents chCode As CheckBox
    Friend WithEvents Label94 As Label
    Friend WithEvents Label93 As Label
    Friend WithEvents tbDatei As TextBox
    Friend WithEvents tbSaveCode As TextBox
    Friend WithEvents tsSpracheDel As ToolStripButton
End Class
