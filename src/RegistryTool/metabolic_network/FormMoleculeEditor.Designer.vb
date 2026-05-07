Imports Galaxy.Workbench.DockDocument

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMoleculeEditor
    Inherits DocumentWindow

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        DataGridView1 = New DataGridView()
        Column1 = New DataGridViewTextBoxColumn()
        Column2 = New DataGridViewTextBoxColumn()
        ContextMenuStrip3 = New ContextMenuStrip(components)
        EditToolStripMenuItem = New ToolStripMenuItem()
        SearchByThisDbXrefToolStripMenuItem = New ToolStripMenuItem()
        Label2 = New Label()
        TextBox1 = New TextBox()
        Button1 = New Button()
        WebView21 = New Microsoft.Web.WebView2.WinForms.WebView2()
        ListBox1 = New ListBox()
        ContextMenuStrip1 = New ContextMenuStrip(components)
        EditToolStripMenuItem1 = New ToolStripMenuItem()
        CopyToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem1 = New ToolStripSeparator()
        SetAsDisplayNameToolStripMenuItem = New ToolStripMenuItem()
        ChineseNameTranslationToolStripMenuItem = New ToolStripMenuItem()
        Label4 = New Label()
        ComboBox1 = New ComboBox()
        Button2 = New Button()
        Label5 = New Label()
        TextBox2 = New TextBox()
        Label6 = New Label()
        TextBox3 = New TextBox()
        Label7 = New Label()
        Label8 = New Label()
        TextBox4 = New TextBox()
        Button3 = New Button()
        Button4 = New Button()
        GroupBox1 = New GroupBox()
        Panel3 = New Panel()
        GroupBox2 = New GroupBox()
        TabControl2 = New TabControl()
        TabPage4 = New TabPage()
        Button18 = New Button()
        txtWikipediaID = New TextBox()
        Button17 = New Button()
        txtMeshID = New TextBox()
        Button16 = New Button()
        txtBioCYCID = New TextBox()
        Button15 = New Button()
        txtDrugBankID = New TextBox()
        Button14 = New Button()
        txtKEGGID = New TextBox()
        Button13 = New Button()
        txtLipidMapsID = New TextBox()
        Button12 = New Button()
        txtHMDBID = New TextBox()
        Button11 = New Button()
        txtCHEBIID = New TextBox()
        Button10 = New Button()
        txtPubChemCID = New TextBox()
        Label17 = New Label()
        Label16 = New Label()
        Label15 = New Label()
        Label14 = New Label()
        Label13 = New Label()
        Label12 = New Label()
        Label11 = New Label()
        Label10 = New Label()
        Button9 = New Button()
        txtCASID = New TextBox()
        Label9 = New Label()
        Label3 = New Label()
        TabPage3 = New TabPage()
        Panel5 = New Panel()
        Button5 = New Button()
        Panel4 = New Panel()
        Button8 = New Button()
        ComboBox3 = New ComboBox()
        TextBox6 = New TextBox()
        GroupBox3 = New GroupBox()
        TextBox5 = New TextBox()
        Label1 = New Label()
        GroupBox4 = New GroupBox()
        LinkLabel3 = New LinkLabel()
        Button19 = New Button()
        TextBox9 = New TextBox()
        Label19 = New Label()
        LinkLabel2 = New LinkLabel()
        TextBox8 = New TextBox()
        Label18 = New Label()
        Button7 = New Button()
        GroupBox5 = New GroupBox()
        ComboBox2 = New ComboBox()
        Button6 = New Button()
        ListBox2 = New ListBox()
        ContextMenuStrip2 = New ContextMenuStrip(components)
        ClearThisTagToolStripMenuItem = New ToolStripMenuItem()
        GroupBox6 = New GroupBox()
        ListBox3 = New ListBox()
        ContextMenuStrip4 = New ContextMenuStrip(components)
        ListSourceToolStripMenuItem = New ToolStripMenuItem()
        DataGridView2 = New DataGridView()
        SplitContainer1 = New SplitContainer()
        DataGridView3 = New DataGridView()
        Column6 = New DataGridViewTextBoxColumn()
        Column11 = New DataGridViewTextBoxColumn()
        Column7 = New DataGridViewTextBoxColumn()
        Column8 = New DataGridViewTextBoxColumn()
        Column9 = New DataGridViewTextBoxColumn()
        Column10 = New DataGridViewTextBoxColumn()
        ContextMenuStrip5 = New ContextMenuStrip(components)
        OpenMoleculeDataToolStripMenuItem = New ToolStripMenuItem()
        TextBox7 = New TextBox()
        ToolStrip1 = New ToolStrip()
        ToolStripLabel1 = New ToolStripLabel()
        LinkLabel1 = New ToolStripLabel()
        Panel1 = New Panel()
        Panel2 = New Panel()
        TabControl1 = New TabControl()
        TabPage1 = New TabPage()
        TabPage2 = New TabPage()
        Column3 = New DataGridViewTextBoxColumn()
        Column5 = New DataGridViewTextBoxColumn()
        Column4 = New DataGridViewTextBoxColumn()
        Column12 = New DataGridViewTextBoxColumn()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        ContextMenuStrip3.SuspendLayout()
        CType(WebView21, ComponentModel.ISupportInitialize).BeginInit()
        ContextMenuStrip1.SuspendLayout()
        GroupBox1.SuspendLayout()
        Panel3.SuspendLayout()
        GroupBox2.SuspendLayout()
        TabControl2.SuspendLayout()
        TabPage4.SuspendLayout()
        TabPage3.SuspendLayout()
        Panel5.SuspendLayout()
        Panel4.SuspendLayout()
        GroupBox3.SuspendLayout()
        GroupBox4.SuspendLayout()
        GroupBox5.SuspendLayout()
        ContextMenuStrip2.SuspendLayout()
        GroupBox6.SuspendLayout()
        ContextMenuStrip4.SuspendLayout()
        CType(DataGridView2, ComponentModel.ISupportInitialize).BeginInit()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        CType(DataGridView3, ComponentModel.ISupportInitialize).BeginInit()
        ContextMenuStrip5.SuspendLayout()
        ToolStrip1.SuspendLayout()
        Panel1.SuspendLayout()
        Panel2.SuspendLayout()
        TabControl1.SuspendLayout()
        TabPage1.SuspendLayout()
        TabPage2.SuspendLayout()
        SuspendLayout()
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.BackgroundColor = Color.LightGray
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Columns.AddRange(New DataGridViewColumn() {Column1, Column2})
        DataGridView1.ContextMenuStrip = ContextMenuStrip3
        DataGridView1.Dock = DockStyle.Left
        DataGridView1.Location = New Point(0, 0)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowTemplate.Height = 23
        DataGridView1.ScrollBars = ScrollBars.Vertical
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(406, 348)
        DataGridView1.TabIndex = 0
        ' 
        ' Column1
        ' 
        Column1.HeaderText = "Database"
        Column1.Name = "Column1"
        Column1.ReadOnly = True
        ' 
        ' Column2
        ' 
        Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Column2.HeaderText = "Xref ID"
        Column2.Name = "Column2"
        Column2.ReadOnly = True
        ' 
        ' ContextMenuStrip3
        ' 
        ContextMenuStrip3.Items.AddRange(New ToolStripItem() {EditToolStripMenuItem, SearchByThisDbXrefToolStripMenuItem})
        ContextMenuStrip3.Name = "ContextMenuStrip3"
        ContextMenuStrip3.Size = New Size(208, 48)
        ' 
        ' EditToolStripMenuItem
        ' 
        EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        EditToolStripMenuItem.Size = New Size(207, 22)
        EditToolStripMenuItem.Text = "Edit"
        ' 
        ' SearchByThisDbXrefToolStripMenuItem
        ' 
        SearchByThisDbXrefToolStripMenuItem.Name = "SearchByThisDbXrefToolStripMenuItem"
        SearchByThisDbXrefToolStripMenuItem.Size = New Size(207, 22)
        SearchByThisDbXrefToolStripMenuItem.Text = "Search By This Db_Xref ID"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(4, 433)
        Label2.Name = "Label2"
        Label2.Size = New Size(48, 15)
        Label2.TabIndex = 2
        Label2.Text = "SMILES:"
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(4, 451)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(459, 23)
        TextBox1.TabIndex = 3
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(469, 451)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 36)
        Button1.TabIndex = 4
        Button1.Text = "Save"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' WebView21
        ' 
        WebView21.AllowExternalDrop = True
        WebView21.CreationProperties = Nothing
        WebView21.DefaultBackgroundColor = Color.White
        WebView21.Location = New Point(6, 19)
        WebView21.Name = "WebView21"
        WebView21.Size = New Size(540, 411)
        WebView21.TabIndex = 5
        WebView21.ZoomFactor = 1R
        ' 
        ' ListBox1
        ' 
        ListBox1.ContextMenuStrip = ContextMenuStrip1
        ListBox1.Dock = DockStyle.Fill
        ListBox1.FormattingEnabled = True
        ListBox1.Location = New Point(3, 55)
        ListBox1.Name = "ListBox1"
        ListBox1.Size = New Size(1329, 389)
        ListBox1.TabIndex = 7
        ' 
        ' ContextMenuStrip1
        ' 
        ContextMenuStrip1.Items.AddRange(New ToolStripItem() {EditToolStripMenuItem1, CopyToolStripMenuItem, ToolStripMenuItem1, SetAsDisplayNameToolStripMenuItem, ChineseNameTranslationToolStripMenuItem})
        ContextMenuStrip1.Name = "ContextMenuStrip1"
        ContextMenuStrip1.Size = New Size(212, 98)
        ' 
        ' EditToolStripMenuItem1
        ' 
        EditToolStripMenuItem1.Name = "EditToolStripMenuItem1"
        EditToolStripMenuItem1.Size = New Size(211, 22)
        EditToolStripMenuItem1.Text = "Edit"
        ' 
        ' CopyToolStripMenuItem
        ' 
        CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        CopyToolStripMenuItem.Size = New Size(211, 22)
        CopyToolStripMenuItem.Text = "Copy"
        ' 
        ' ToolStripMenuItem1
        ' 
        ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        ToolStripMenuItem1.Size = New Size(208, 6)
        ' 
        ' SetAsDisplayNameToolStripMenuItem
        ' 
        SetAsDisplayNameToolStripMenuItem.Name = "SetAsDisplayNameToolStripMenuItem"
        SetAsDisplayNameToolStripMenuItem.Size = New Size(211, 22)
        SetAsDisplayNameToolStripMenuItem.Text = "Set As Display Name"
        ' 
        ' ChineseNameTranslationToolStripMenuItem
        ' 
        ChineseNameTranslationToolStripMenuItem.Name = "ChineseNameTranslationToolStripMenuItem"
        ChineseNameTranslationToolStripMenuItem.Size = New Size(211, 22)
        ChineseNameTranslationToolStripMenuItem.Text = "Chinese Name Translation"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(7, 9)
        Label4.Name = "Label4"
        Label4.Size = New Size(96, 15)
        Label4.TabIndex = 8
        Label4.Text = "Filter Languages:"
        ' 
        ' ComboBox1
        ' 
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox1.FormattingEnabled = True
        ComboBox1.Items.AddRange(New Object() {"*", "en", "zh"})
        ComboBox1.Location = New Point(109, 6)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(152, 23)
        ComboBox1.TabIndex = 9
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(419, 30)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 23)
        Button2.TabIndex = 10
        Button2.Text = "Save"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(13, 35)
        Label5.Name = "Label5"
        Label5.Size = New Size(42, 15)
        Label5.TabIndex = 11
        Label5.Text = "Name:"
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(72, 30)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(339, 23)
        TextBox2.TabIndex = 12
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(13, 68)
        Label6.Name = "Label6"
        Label6.Size = New Size(54, 15)
        Label6.TabIndex = 13
        Label6.Text = "Formula:"
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(72, 63)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(103, 23)
        TextBox3.TabIndex = 14
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(191, 68)
        Label7.Name = "Label7"
        Label7.Size = New Size(65, 15)
        Label7.TabIndex = 15
        Label7.Text = "Exact Mass"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(13, 202)
        Label8.Name = "Label8"
        Label8.Size = New Size(99, 15)
        Label8.TabIndex = 16
        Label8.Text = "Description Note:"
        ' 
        ' TextBox4
        ' 
        TextBox4.Location = New Point(15, 220)
        TextBox4.Multiline = True
        TextBox4.Name = "TextBox4"
        TextBox4.ScrollBars = ScrollBars.Vertical
        TextBox4.Size = New Size(396, 335)
        TextBox4.TabIndex = 17
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(419, 63)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 23)
        Button3.TabIndex = 18
        Button3.Text = "Save"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(422, 231)
        Button4.Name = "Button4"
        Button4.Size = New Size(75, 34)
        Button4.TabIndex = 19
        Button4.Text = "Save"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(ListBox1)
        GroupBox1.Controls.Add(Panel3)
        GroupBox1.Dock = DockStyle.Fill
        GroupBox1.Location = New Point(515, 0)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(1335, 447)
        GroupBox1.TabIndex = 20
        GroupBox1.TabStop = False
        GroupBox1.Text = "Synonym Names"
        ' 
        ' Panel3
        ' 
        Panel3.Controls.Add(ComboBox1)
        Panel3.Controls.Add(Label4)
        Panel3.Dock = DockStyle.Top
        Panel3.Location = New Point(3, 19)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(1329, 36)
        Panel3.TabIndex = 28
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Controls.Add(TabControl2)
        GroupBox2.Dock = DockStyle.Left
        GroupBox2.Location = New Point(0, 0)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(515, 447)
        GroupBox2.TabIndex = 21
        GroupBox2.TabStop = False
        GroupBox2.Text = "Database CrossReference"
        ' 
        ' TabControl2
        ' 
        TabControl2.Controls.Add(TabPage4)
        TabControl2.Controls.Add(TabPage3)
        TabControl2.Dock = DockStyle.Fill
        TabControl2.Location = New Point(3, 19)
        TabControl2.Name = "TabControl2"
        TabControl2.SelectedIndex = 0
        TabControl2.Size = New Size(509, 425)
        TabControl2.TabIndex = 25
        ' 
        ' TabPage4
        ' 
        TabPage4.Controls.Add(Button18)
        TabPage4.Controls.Add(txtWikipediaID)
        TabPage4.Controls.Add(Button17)
        TabPage4.Controls.Add(txtMeshID)
        TabPage4.Controls.Add(Button16)
        TabPage4.Controls.Add(txtBioCYCID)
        TabPage4.Controls.Add(Button15)
        TabPage4.Controls.Add(txtDrugBankID)
        TabPage4.Controls.Add(Button14)
        TabPage4.Controls.Add(txtKEGGID)
        TabPage4.Controls.Add(Button13)
        TabPage4.Controls.Add(txtLipidMapsID)
        TabPage4.Controls.Add(Button12)
        TabPage4.Controls.Add(txtHMDBID)
        TabPage4.Controls.Add(Button11)
        TabPage4.Controls.Add(txtCHEBIID)
        TabPage4.Controls.Add(Button10)
        TabPage4.Controls.Add(txtPubChemCID)
        TabPage4.Controls.Add(Label17)
        TabPage4.Controls.Add(Label16)
        TabPage4.Controls.Add(Label15)
        TabPage4.Controls.Add(Label14)
        TabPage4.Controls.Add(Label13)
        TabPage4.Controls.Add(Label12)
        TabPage4.Controls.Add(Label11)
        TabPage4.Controls.Add(Label10)
        TabPage4.Controls.Add(Button9)
        TabPage4.Controls.Add(txtCASID)
        TabPage4.Controls.Add(Label9)
        TabPage4.Controls.Add(Label3)
        TabPage4.Location = New Point(4, 24)
        TabPage4.Name = "TabPage4"
        TabPage4.Padding = New Padding(3)
        TabPage4.Size = New Size(501, 397)
        TabPage4.TabIndex = 1
        TabPage4.Text = "Main Db Xrefs"
        TabPage4.UseVisualStyleBackColor = True
        ' 
        ' Button18
        ' 
        Button18.Location = New Point(417, 355)
        Button18.Name = "Button18"
        Button18.Size = New Size(75, 23)
        Button18.TabIndex = 29
        Button18.Text = "Save"
        Button18.UseVisualStyleBackColor = True
        ' 
        ' txtWikipediaID
        ' 
        txtWikipediaID.Location = New Point(115, 355)
        txtWikipediaID.Name = "txtWikipediaID"
        txtWikipediaID.Size = New Size(276, 23)
        txtWikipediaID.TabIndex = 28
        ' 
        ' Button17
        ' 
        Button17.Location = New Point(417, 315)
        Button17.Name = "Button17"
        Button17.Size = New Size(75, 23)
        Button17.TabIndex = 27
        Button17.Text = "Save"
        Button17.UseVisualStyleBackColor = True
        ' 
        ' txtMeshID
        ' 
        txtMeshID.Location = New Point(115, 315)
        txtMeshID.Name = "txtMeshID"
        txtMeshID.Size = New Size(276, 23)
        txtMeshID.TabIndex = 26
        ' 
        ' Button16
        ' 
        Button16.Location = New Point(417, 276)
        Button16.Name = "Button16"
        Button16.Size = New Size(75, 23)
        Button16.TabIndex = 25
        Button16.Text = "Save"
        Button16.UseVisualStyleBackColor = True
        ' 
        ' txtBioCYCID
        ' 
        txtBioCYCID.Location = New Point(115, 276)
        txtBioCYCID.Name = "txtBioCYCID"
        txtBioCYCID.Size = New Size(276, 23)
        txtBioCYCID.TabIndex = 24
        ' 
        ' Button15
        ' 
        Button15.Location = New Point(417, 233)
        Button15.Name = "Button15"
        Button15.Size = New Size(75, 23)
        Button15.TabIndex = 23
        Button15.Text = "Save"
        Button15.UseVisualStyleBackColor = True
        ' 
        ' txtDrugBankID
        ' 
        txtDrugBankID.Location = New Point(115, 234)
        txtDrugBankID.Name = "txtDrugBankID"
        txtDrugBankID.Size = New Size(276, 23)
        txtDrugBankID.TabIndex = 22
        ' 
        ' Button14
        ' 
        Button14.Location = New Point(417, 194)
        Button14.Name = "Button14"
        Button14.Size = New Size(75, 23)
        Button14.TabIndex = 21
        Button14.Text = "Save"
        Button14.UseVisualStyleBackColor = True
        ' 
        ' txtKEGGID
        ' 
        txtKEGGID.Location = New Point(115, 195)
        txtKEGGID.Name = "txtKEGGID"
        txtKEGGID.Size = New Size(276, 23)
        txtKEGGID.TabIndex = 20
        ' 
        ' Button13
        ' 
        Button13.Location = New Point(417, 159)
        Button13.Name = "Button13"
        Button13.Size = New Size(75, 23)
        Button13.TabIndex = 19
        Button13.Text = "Save"
        Button13.UseVisualStyleBackColor = True
        ' 
        ' txtLipidMapsID
        ' 
        txtLipidMapsID.Location = New Point(115, 160)
        txtLipidMapsID.Name = "txtLipidMapsID"
        txtLipidMapsID.Size = New Size(276, 23)
        txtLipidMapsID.TabIndex = 18
        ' 
        ' Button12
        ' 
        Button12.Location = New Point(417, 124)
        Button12.Name = "Button12"
        Button12.Size = New Size(75, 23)
        Button12.TabIndex = 17
        Button12.Text = "Save"
        Button12.UseVisualStyleBackColor = True
        ' 
        ' txtHMDBID
        ' 
        txtHMDBID.Location = New Point(115, 125)
        txtHMDBID.Name = "txtHMDBID"
        txtHMDBID.Size = New Size(276, 23)
        txtHMDBID.TabIndex = 16
        ' 
        ' Button11
        ' 
        Button11.Location = New Point(417, 89)
        Button11.Name = "Button11"
        Button11.Size = New Size(75, 23)
        Button11.TabIndex = 15
        Button11.Text = "Save"
        Button11.UseVisualStyleBackColor = True
        ' 
        ' txtCHEBIID
        ' 
        txtCHEBIID.Location = New Point(115, 90)
        txtCHEBIID.Name = "txtCHEBIID"
        txtCHEBIID.Size = New Size(276, 23)
        txtCHEBIID.TabIndex = 14
        ' 
        ' Button10
        ' 
        Button10.Location = New Point(417, 54)
        Button10.Name = "Button10"
        Button10.Size = New Size(75, 23)
        Button10.TabIndex = 13
        Button10.Text = "Save"
        Button10.UseVisualStyleBackColor = True
        ' 
        ' txtPubChemCID
        ' 
        txtPubChemCID.Location = New Point(115, 55)
        txtPubChemCID.Name = "txtPubChemCID"
        txtPubChemCID.Size = New Size(276, 23)
        txtPubChemCID.TabIndex = 12
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Location = New Point(36, 355)
        Label17.Name = "Label17"
        Label17.Size = New Size(62, 15)
        Label17.TabIndex = 11
        Label17.Text = "Wikipedia:"
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Location = New Point(39, 318)
        Label16.Name = "Label16"
        Label16.Size = New Size(56, 15)
        Label16.TabIndex = 10
        Label16.Text = "MeSH ID:"
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Location = New Point(51, 276)
        Label15.Name = "Label15"
        Label15.Size = New Size(47, 15)
        Label15.TabIndex = 9
        Label15.Text = "BioCyc:"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Location = New Point(22, 234)
        Label14.Name = "Label14"
        Label14.Size = New Size(76, 15)
        Label14.TabIndex = 8
        Label14.Text = "DrugBank ID:"
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(45, 195)
        Label13.Name = "Label13"
        Label13.Size = New Size(53, 15)
        Label13.TabIndex = 7
        Label13.Text = "KEGG ID:"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(16, 160)
        Label12.Name = "Label12"
        Label12.Size = New Size(82, 15)
        Label12.TabIndex = 6
        Label12.Text = "LipidMAPS ID:"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(36, 125)
        Label11.Name = "Label11"
        Label11.Size = New Size(59, 15)
        Label11.TabIndex = 5
        Label11.Text = "HMDB ID:"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(40, 90)
        Label10.Name = "Label10"
        Label10.Size = New Size(55, 15)
        Label10.TabIndex = 4
        Label10.Text = "ChEBI ID:"
        ' 
        ' Button9
        ' 
        Button9.Location = New Point(417, 21)
        Button9.Name = "Button9"
        Button9.Size = New Size(75, 23)
        Button9.TabIndex = 3
        Button9.Text = "Save"
        Button9.UseVisualStyleBackColor = True
        ' 
        ' txtCASID
        ' 
        txtCASID.Location = New Point(115, 22)
        txtCASID.Name = "txtCASID"
        txtCASID.Size = New Size(276, 23)
        txtCASID.TabIndex = 2
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(13, 58)
        Label9.Name = "Label9"
        Label9.Size = New Size(85, 15)
        Label9.TabIndex = 1
        Label9.Text = "PubChem CID:"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(52, 25)
        Label3.Name = "Label3"
        Label3.Size = New Size(46, 15)
        Label3.TabIndex = 0
        Label3.Text = "CAS ID:"
        ' 
        ' TabPage3
        ' 
        TabPage3.Controls.Add(Panel5)
        TabPage3.Controls.Add(Panel4)
        TabPage3.Location = New Point(4, 24)
        TabPage3.Name = "TabPage3"
        TabPage3.Padding = New Padding(3)
        TabPage3.Size = New Size(501, 397)
        TabPage3.TabIndex = 0
        TabPage3.Text = "Other Db Xrefs"
        TabPage3.UseVisualStyleBackColor = True
        ' 
        ' Panel5
        ' 
        Panel5.Controls.Add(DataGridView1)
        Panel5.Controls.Add(Button5)
        Panel5.Dock = DockStyle.Fill
        Panel5.Location = New Point(3, 3)
        Panel5.Name = "Panel5"
        Panel5.Size = New Size(495, 348)
        Panel5.TabIndex = 26
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(414, 7)
        Button5.Name = "Button5"
        Button5.Size = New Size(75, 23)
        Button5.TabIndex = 20
        Button5.Text = "Save"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' Panel4
        ' 
        Panel4.Controls.Add(Button8)
        Panel4.Controls.Add(ComboBox3)
        Panel4.Controls.Add(TextBox6)
        Panel4.Dock = DockStyle.Bottom
        Panel4.Location = New Point(3, 351)
        Panel4.Name = "Panel4"
        Panel4.Size = New Size(495, 43)
        Panel4.TabIndex = 25
        ' 
        ' Button8
        ' 
        Button8.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Button8.Location = New Point(333, 8)
        Button8.Name = "Button8"
        Button8.Size = New Size(73, 23)
        Button8.TabIndex = 24
        Button8.Text = "Add"
        Button8.UseVisualStyleBackColor = True
        ' 
        ' ComboBox3
        ' 
        ComboBox3.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        ComboBox3.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox3.FormattingEnabled = True
        ComboBox3.Location = New Point(10, 9)
        ComboBox3.Name = "ComboBox3"
        ComboBox3.Size = New Size(93, 23)
        ComboBox3.TabIndex = 21
        ' 
        ' TextBox6
        ' 
        TextBox6.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        TextBox6.Location = New Point(109, 9)
        TextBox6.Name = "TextBox6"
        TextBox6.Size = New Size(218, 23)
        TextBox6.TabIndex = 23
        ' 
        ' GroupBox3
        ' 
        GroupBox3.Controls.Add(TextBox5)
        GroupBox3.Controls.Add(Label1)
        GroupBox3.Controls.Add(WebView21)
        GroupBox3.Controls.Add(Label2)
        GroupBox3.Controls.Add(TextBox1)
        GroupBox3.Controls.Add(Button1)
        GroupBox3.Location = New Point(521, 9)
        GroupBox3.Name = "GroupBox3"
        GroupBox3.Size = New Size(552, 561)
        GroupBox3.TabIndex = 22
        GroupBox3.TabStop = False
        GroupBox3.Text = "Molecular Structure Data"
        ' 
        ' TextBox5
        ' 
        TextBox5.Location = New Point(4, 495)
        TextBox5.Multiline = True
        TextBox5.Name = "TextBox5"
        TextBox5.ReadOnly = True
        TextBox5.Size = New Size(540, 60)
        TextBox5.TabIndex = 7
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(4, 477)
        Label1.Name = "Label1"
        Label1.Size = New Size(73, 15)
        Label1.TabIndex = 6
        Label1.Text = "Fingerprints:"
        ' 
        ' GroupBox4
        ' 
        GroupBox4.Controls.Add(LinkLabel3)
        GroupBox4.Controls.Add(Button19)
        GroupBox4.Controls.Add(TextBox9)
        GroupBox4.Controls.Add(Label19)
        GroupBox4.Controls.Add(LinkLabel2)
        GroupBox4.Controls.Add(TextBox8)
        GroupBox4.Controls.Add(Label18)
        GroupBox4.Controls.Add(Button7)
        GroupBox4.Controls.Add(TextBox4)
        GroupBox4.Controls.Add(Button2)
        GroupBox4.Controls.Add(Label5)
        GroupBox4.Controls.Add(TextBox2)
        GroupBox4.Controls.Add(Button4)
        GroupBox4.Controls.Add(Label6)
        GroupBox4.Controls.Add(Button3)
        GroupBox4.Controls.Add(TextBox3)
        GroupBox4.Controls.Add(Label7)
        GroupBox4.Controls.Add(Label8)
        GroupBox4.Location = New Point(5, 9)
        GroupBox4.Name = "GroupBox4"
        GroupBox4.Size = New Size(510, 561)
        GroupBox4.TabIndex = 23
        GroupBox4.TabStop = False
        GroupBox4.Text = "Molecular Information"
        ' 
        ' LinkLabel3
        ' 
        LinkLabel3.AutoSize = True
        LinkLabel3.Location = New Point(244, 134)
        LinkLabel3.Name = "LinkLabel3"
        LinkLabel3.Size = New Size(92, 15)
        LinkLabel3.TabIndex = 27
        LinkLabel3.TabStop = True
        LinkLabel3.Text = "Register Symbol"
        ' 
        ' Button19
        ' 
        Button19.Location = New Point(419, 167)
        Button19.Name = "Button19"
        Button19.Size = New Size(75, 23)
        Button19.TabIndex = 26
        Button19.Text = "Save"
        Button19.UseVisualStyleBackColor = True
        ' 
        ' TextBox9
        ' 
        TextBox9.Location = New Point(116, 168)
        TextBox9.Name = "TextBox9"
        TextBox9.Size = New Size(297, 23)
        TextBox9.TabIndex = 25
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Location = New Point(23, 176)
        Label19.Name = "Label19"
        Label19.Size = New Size(87, 15)
        Label19.TabIndex = 24
        Label19.Text = "Chinese Name:"
        ' 
        ' LinkLabel2
        ' 
        LinkLabel2.AutoSize = True
        LinkLabel2.Location = New Point(358, 134)
        LinkLabel2.Name = "LinkLabel2"
        LinkLabel2.Size = New Size(35, 15)
        LinkLabel2.TabIndex = 23
        LinkLabel2.TabStop = True
        LinkLabel2.Text = "Copy"
        ' 
        ' TextBox8
        ' 
        TextBox8.Location = New Point(114, 98)
        TextBox8.Name = "TextBox8"
        TextBox8.ReadOnly = True
        TextBox8.Size = New Size(297, 23)
        TextBox8.TabIndex = 22
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Location = New Point(13, 101)
        Label18.Name = "Label18"
        Label18.Size = New Size(95, 15)
        Label18.TabIndex = 21
        Label18.Text = "Registry Symbol:"
        ' 
        ' Button7
        ' 
        Button7.Location = New Point(419, 271)
        Button7.Name = "Button7"
        Button7.Size = New Size(75, 35)
        Button7.TabIndex = 20
        Button7.Text = "LLMs Talk"
        Button7.UseVisualStyleBackColor = True
        ' 
        ' GroupBox5
        ' 
        GroupBox5.Controls.Add(ComboBox2)
        GroupBox5.Controls.Add(Button6)
        GroupBox5.Controls.Add(ListBox2)
        GroupBox5.Location = New Point(1079, 9)
        GroupBox5.Name = "GroupBox5"
        GroupBox5.Size = New Size(367, 561)
        GroupBox5.TabIndex = 26
        GroupBox5.TabStop = False
        GroupBox5.Text = "Topic Tags"
        ' 
        ' ComboBox2
        ' 
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.FormattingEnabled = True
        ComboBox2.Location = New Point(6, 532)
        ComboBox2.Name = "ComboBox2"
        ComboBox2.Size = New Size(274, 23)
        ComboBox2.TabIndex = 2
        ' 
        ' Button6
        ' 
        Button6.Location = New Point(286, 531)
        Button6.Name = "Button6"
        Button6.Size = New Size(75, 23)
        Button6.TabIndex = 1
        Button6.Text = "Add"
        Button6.UseVisualStyleBackColor = True
        ' 
        ' ListBox2
        ' 
        ListBox2.ContextMenuStrip = ContextMenuStrip2
        ListBox2.FormattingEnabled = True
        ListBox2.Location = New Point(6, 20)
        ListBox2.Name = "ListBox2"
        ListBox2.Size = New Size(355, 499)
        ListBox2.TabIndex = 0
        ' 
        ' ContextMenuStrip2
        ' 
        ContextMenuStrip2.Items.AddRange(New ToolStripItem() {ClearThisTagToolStripMenuItem})
        ContextMenuStrip2.Name = "ContextMenuStrip2"
        ContextMenuStrip2.Size = New Size(147, 26)
        ' 
        ' ClearThisTagToolStripMenuItem
        ' 
        ClearThisTagToolStripMenuItem.Name = "ClearThisTagToolStripMenuItem"
        ClearThisTagToolStripMenuItem.Size = New Size(146, 22)
        ClearThisTagToolStripMenuItem.Text = "Clear This Tag"
        ' 
        ' GroupBox6
        ' 
        GroupBox6.Controls.Add(ListBox3)
        GroupBox6.Location = New Point(1452, 9)
        GroupBox6.Name = "GroupBox6"
        GroupBox6.Size = New Size(367, 561)
        GroupBox6.TabIndex = 27
        GroupBox6.TabStop = False
        GroupBox6.Text = "Organism Source"
        ' 
        ' ListBox3
        ' 
        ListBox3.ContextMenuStrip = ContextMenuStrip4
        ListBox3.Dock = DockStyle.Fill
        ListBox3.FormattingEnabled = True
        ListBox3.Location = New Point(3, 19)
        ListBox3.Name = "ListBox3"
        ListBox3.Size = New Size(361, 539)
        ListBox3.TabIndex = 0
        ' 
        ' ContextMenuStrip4
        ' 
        ContextMenuStrip4.Items.AddRange(New ToolStripItem() {ListSourceToolStripMenuItem})
        ContextMenuStrip4.Name = "ContextMenuStrip4"
        ContextMenuStrip4.Size = New Size(132, 26)
        ' 
        ' ListSourceToolStripMenuItem
        ' 
        ListSourceToolStripMenuItem.Name = "ListSourceToolStripMenuItem"
        ListSourceToolStripMenuItem.Size = New Size(131, 22)
        ListSourceToolStripMenuItem.Text = "List Source"
        ' 
        ' DataGridView2
        ' 
        DataGridView2.AllowUserToAddRows = False
        DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView2.Columns.AddRange(New DataGridViewColumn() {Column3, Column5, Column4, Column12})
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = SystemColors.Window
        DataGridViewCellStyle1.Font = New Font("Cambria", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle1.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.False
        DataGridView2.DefaultCellStyle = DataGridViewCellStyle1
        DataGridView2.Dock = DockStyle.Fill
        DataGridView2.Location = New Point(0, 0)
        DataGridView2.Name = "DataGridView2"
        DataGridView2.RowTemplate.Height = 23
        DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView2.Size = New Size(1850, 556)
        DataGridView2.TabIndex = 28
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.Location = New Point(3, 3)
        SplitContainer1.Name = "SplitContainer1"
        SplitContainer1.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(DataGridView2)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.Controls.Add(DataGridView3)
        SplitContainer1.Panel2.Controls.Add(TextBox7)
        SplitContainer1.Size = New Size(1850, 1023)
        SplitContainer1.SplitterDistance = 556
        SplitContainer1.TabIndex = 30
        ' 
        ' DataGridView3
        ' 
        DataGridView3.AllowUserToAddRows = False
        DataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView3.Columns.AddRange(New DataGridViewColumn() {Column6, Column11, Column7, Column8, Column9, Column10})
        DataGridView3.ContextMenuStrip = ContextMenuStrip5
        DataGridView3.Dock = DockStyle.Fill
        DataGridView3.Location = New Point(0, 90)
        DataGridView3.Name = "DataGridView3"
        DataGridView3.RowTemplate.Height = 23
        DataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView3.Size = New Size(1850, 373)
        DataGridView3.TabIndex = 29
        ' 
        ' Column6
        ' 
        Column6.HeaderText = "registry_id"
        Column6.Name = "Column6"
        Column6.ReadOnly = True
        ' 
        ' Column11
        ' 
        Column11.HeaderText = "db_xref"
        Column11.Name = "Column11"
        Column11.ReadOnly = True
        ' 
        ' Column7
        ' 
        Column7.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Column7.HeaderText = "name"
        Column7.Name = "Column7"
        Column7.ReadOnly = True
        ' 
        ' Column8
        ' 
        Column8.HeaderText = "formula"
        Column8.Name = "Column8"
        Column8.ReadOnly = True
        ' 
        ' Column9
        ' 
        Column9.HeaderText = "exact_mass"
        Column9.Name = "Column9"
        Column9.ReadOnly = True
        ' 
        ' Column10
        ' 
        Column10.HeaderText = "side"
        Column10.Name = "Column10"
        Column10.ReadOnly = True
        ' 
        ' ContextMenuStrip5
        ' 
        ContextMenuStrip5.Items.AddRange(New ToolStripItem() {OpenMoleculeDataToolStripMenuItem})
        ContextMenuStrip5.Name = "ContextMenuStrip5"
        ContextMenuStrip5.Size = New Size(183, 26)
        ' 
        ' OpenMoleculeDataToolStripMenuItem
        ' 
        OpenMoleculeDataToolStripMenuItem.Name = "OpenMoleculeDataToolStripMenuItem"
        OpenMoleculeDataToolStripMenuItem.Size = New Size(182, 22)
        OpenMoleculeDataToolStripMenuItem.Text = "Open Molecule Data"
        ' 
        ' TextBox7
        ' 
        TextBox7.BorderStyle = BorderStyle.FixedSingle
        TextBox7.Dock = DockStyle.Top
        TextBox7.Location = New Point(0, 0)
        TextBox7.Multiline = True
        TextBox7.Name = "TextBox7"
        TextBox7.ReadOnly = True
        TextBox7.Size = New Size(1850, 90)
        TextBox7.TabIndex = 30
        ' 
        ' ToolStrip1
        ' 
        ToolStrip1.Items.AddRange(New ToolStripItem() {ToolStripLabel1, LinkLabel1})
        ToolStrip1.Location = New Point(0, 0)
        ToolStrip1.Name = "ToolStrip1"
        ToolStrip1.Size = New Size(1864, 25)
        ToolStrip1.TabIndex = 30
        ToolStrip1.Text = "ToolStrip1"
        ' 
        ' ToolStripLabel1
        ' 
        ToolStripLabel1.Name = "ToolStripLabel1"
        ToolStripLabel1.Size = New Size(83, 22)
        ToolStripLabel1.Text = "Visit Site Page:"
        ' 
        ' LinkLabel1
        ' 
        LinkLabel1.IsLink = True
        LinkLabel1.Name = "LinkLabel1"
        LinkLabel1.Size = New Size(340, 22)
        LinkLabel1.Text = "http://biocad.innovation.ac.cn/molecule/BioCAD00000000000/"
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(GroupBox4)
        Panel1.Controls.Add(GroupBox3)
        Panel1.Controls.Add(GroupBox5)
        Panel1.Controls.Add(GroupBox6)
        Panel1.Dock = DockStyle.Top
        Panel1.Location = New Point(3, 3)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1850, 576)
        Panel1.TabIndex = 31
        ' 
        ' Panel2
        ' 
        Panel2.Controls.Add(GroupBox1)
        Panel2.Controls.Add(GroupBox2)
        Panel2.Dock = DockStyle.Fill
        Panel2.Location = New Point(3, 579)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(1850, 447)
        Panel2.TabIndex = 32
        ' 
        ' TabControl1
        ' 
        TabControl1.Controls.Add(TabPage1)
        TabControl1.Controls.Add(TabPage2)
        TabControl1.Dock = DockStyle.Fill
        TabControl1.Location = New Point(0, 25)
        TabControl1.Name = "TabControl1"
        TabControl1.SelectedIndex = 0
        TabControl1.Size = New Size(1864, 1057)
        TabControl1.TabIndex = 33
        ' 
        ' TabPage1
        ' 
        TabPage1.Controls.Add(Panel2)
        TabPage1.Controls.Add(Panel1)
        TabPage1.Location = New Point(4, 24)
        TabPage1.Name = "TabPage1"
        TabPage1.Padding = New Padding(3)
        TabPage1.Size = New Size(1856, 1029)
        TabPage1.TabIndex = 0
        TabPage1.Text = "Molecule Information"
        TabPage1.UseVisualStyleBackColor = True
        ' 
        ' TabPage2
        ' 
        TabPage2.Controls.Add(SplitContainer1)
        TabPage2.Location = New Point(4, 24)
        TabPage2.Name = "TabPage2"
        TabPage2.Padding = New Padding(3)
        TabPage2.Size = New Size(1856, 1029)
        TabPage2.TabIndex = 1
        TabPage2.Text = "Related Metabolic Network"
        TabPage2.UseVisualStyleBackColor = True
        ' 
        ' Column3
        ' 
        Column3.HeaderText = "name"
        Column3.Name = "Column3"
        Column3.ReadOnly = True
        Column3.Width = 300
        ' 
        ' Column5
        ' 
        Column5.HeaderText = "ec_number"
        Column5.Name = "Column5"
        Column5.ReadOnly = True
        ' 
        ' Column4
        ' 
        Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Column4.FillWeight = 500F
        Column4.HeaderText = "equation"
        Column4.Name = "Column4"
        Column4.ReadOnly = True
        ' 
        ' Column12
        ' 
        Column12.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Column12.FillWeight = 400F
        Column12.HeaderText = "note"
        Column12.Name = "Column12"
        Column12.ReadOnly = True
        ' 
        ' FormMoleculeEditor
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1864, 1082)
        Controls.Add(TabControl1)
        Controls.Add(ToolStrip1)
        DockAreas = Microsoft.VisualStudio.WinForms.Docking.DockAreas.Float Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockLeft Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockRight Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockTop Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockBottom Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.Document
        DoubleBuffered = True
        FormBorderStyle = FormBorderStyle.FixedSingle
        Name = "FormMoleculeEditor"
        ShowHint = Microsoft.VisualStudio.WinForms.Docking.DockState.Unknown
        TabPageContextMenuStrip = DockContextMenuStrip1
        Text = "Molecule Editor"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ContextMenuStrip3.ResumeLayout(False)
        CType(WebView21, ComponentModel.ISupportInitialize).EndInit()
        ContextMenuStrip1.ResumeLayout(False)
        GroupBox1.ResumeLayout(False)
        Panel3.ResumeLayout(False)
        Panel3.PerformLayout()
        GroupBox2.ResumeLayout(False)
        TabControl2.ResumeLayout(False)
        TabPage4.ResumeLayout(False)
        TabPage4.PerformLayout()
        TabPage3.ResumeLayout(False)
        Panel5.ResumeLayout(False)
        Panel4.ResumeLayout(False)
        Panel4.PerformLayout()
        GroupBox3.ResumeLayout(False)
        GroupBox3.PerformLayout()
        GroupBox4.ResumeLayout(False)
        GroupBox4.PerformLayout()
        GroupBox5.ResumeLayout(False)
        ContextMenuStrip2.ResumeLayout(False)
        GroupBox6.ResumeLayout(False)
        ContextMenuStrip4.ResumeLayout(False)
        CType(DataGridView2, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        SplitContainer1.Panel2.PerformLayout()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        CType(DataGridView3, ComponentModel.ISupportInitialize).EndInit()
        ContextMenuStrip5.ResumeLayout(False)
        ToolStrip1.ResumeLayout(False)
        ToolStrip1.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel2.ResumeLayout(False)
        TabControl1.ResumeLayout(False)
        TabPage1.ResumeLayout(False)
        TabPage2.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents WebView21 As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents Label4 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents Button5 As Button
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents SetAsDisplayNameToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents ListBox2 As ListBox
    Friend WithEvents ContextMenuStrip2 As ContextMenuStrip
    Friend WithEvents ClearThisTagToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStrip3 As ContextMenuStrip
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Button6 As Button
    Friend WithEvents EditToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents Button7 As Button
    Friend WithEvents ChineseNameTranslationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Button8 As Button
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents ComboBox3 As ComboBox
    Friend WithEvents CopyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents ListBox3 As ListBox
    Friend WithEvents ContextMenuStrip4 As ContextMenuStrip
    Friend WithEvents ListSourceToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SearchByThisDbXrefToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents DataGridView3 As DataGridView
    Friend WithEvents ContextMenuStrip5 As ContextMenuStrip
    Friend WithEvents OpenMoleculeDataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Column6 As DataGridViewTextBoxColumn
    Friend WithEvents Column11 As DataGridViewTextBoxColumn
    Friend WithEvents Column7 As DataGridViewTextBoxColumn
    Friend WithEvents Column8 As DataGridViewTextBoxColumn
    Friend WithEvents Column9 As DataGridViewTextBoxColumn
    Friend WithEvents Column10 As DataGridViewTextBoxColumn
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents LinkLabel1 As ToolStripLabel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TextBox7 As TextBox
    Friend WithEvents Panel3 As Panel
    Friend WithEvents TabControl2 As TabControl
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents Label17 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Button9 As Button
    Friend WithEvents txtCASID As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Button18 As Button
    Friend WithEvents txtWikipediaID As TextBox
    Friend WithEvents Button17 As Button
    Friend WithEvents txtMeshID As TextBox
    Friend WithEvents Button16 As Button
    Friend WithEvents txtBioCYCID As TextBox
    Friend WithEvents Button15 As Button
    Friend WithEvents txtDrugBankID As TextBox
    Friend WithEvents Button14 As Button
    Friend WithEvents txtKEGGID As TextBox
    Friend WithEvents Button13 As Button
    Friend WithEvents txtLipidMapsID As TextBox
    Friend WithEvents Button12 As Button
    Friend WithEvents txtHMDBID As TextBox
    Friend WithEvents Button11 As Button
    Friend WithEvents txtCHEBIID As TextBox
    Friend WithEvents Button10 As Button
    Friend WithEvents txtPubChemCID As TextBox
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents TextBox8 As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents LinkLabel2 As LinkLabel
    Friend WithEvents Button19 As Button
    Friend WithEvents TextBox9 As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents LinkLabel3 As LinkLabel
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column12 As DataGridViewTextBoxColumn
End Class
