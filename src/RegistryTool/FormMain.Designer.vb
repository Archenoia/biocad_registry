<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        MenuStrip1 = New MenuStrip()
        FileToolStripMenuItem = New ToolStripMenuItem()
        OpenMoleculeToolStripMenuItem = New ToolStripMenuItem()
        OpenSymbolToolStripMenuItem = New ToolStripMenuItem()
        ClearListToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem9 = New ToolStripSeparator()
        BatchOperationToolStripMenuItem = New ToolStripMenuItem()
        SearchNamesToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem4 = New ToolStripSeparator()
        SettingsToolStripMenuItem = New ToolStripMenuItem()
        AboutToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem1 = New ToolStripSeparator()
        ExitToolStripMenuItem = New ToolStripMenuItem()
        DataToolStripMenuItem = New ToolStripMenuItem()
        VocabularyToolStripMenuItem = New ToolStripMenuItem()
        ProteinModelsToolStripMenuItem = New ToolStripMenuItem()
        MoleculesToolStripMenuItem = New ToolStripMenuItem()
        SearchToolStripMenuItem = New ToolStripMenuItem()
        SearchNameToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem8 = New ToolStripSeparator()
        CreateNewMetaboliteToolStripMenuItem = New ToolStripMenuItem()
        SubCellularCompartmentsToolStripMenuItem = New ToolStripMenuItem()
        FlavorOdorsToolStripMenuItem = New ToolStripMenuItem()
        ReactionEditorToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem2 = New ToolStripSeparator()
        CreateMetabolicReactionToolStripMenuItem = New ToolStripMenuItem()
        ImportsToolStripMenuItem = New ToolStripMenuItem()
        GenBankToolStripMenuItem = New ToolStripMenuItem()
        PubMedKnowledgeToolStripMenuItem = New ToolStripMenuItem()
        PubChemIDToolStripMenuItem = New ToolStripMenuItem()
        ExportsToolStripMenuItem = New ToolStripMenuItem()
        DaisyLCMSWorkflowToolStripMenuItem = New ToolStripMenuItem()
        ExportLipidMAPSIDMappingToolStripMenuItem = New ToolStripMenuItem()
        ExportHMDBIDMappingToolStripMenuItem = New ToolStripMenuItem()
        ExportMoNAIDMappingToolStripMenuItem = New ToolStripMenuItem()
        ExportKEGGIDMappingToolStripMenuItem1 = New ToolStripMenuItem()
        ExportKEGGMetaboliteTableToolStripMenuItem1 = New ToolStripMenuItem()
        ToolStripMenuItem5 = New ToolStripSeparator()
        ExportMetabolitesDatabaseToolStripMenuItem1 = New ToolStripMenuItem()
        ExportAnnotationTableToolStripMenuItem1 = New ToolStripMenuItem()
        ToolStripMenuItem6 = New ToolStripSeparator()
        ExportTagDataToolStripMenuItem = New ToolStripMenuItem()
        FastaDatabaseToolStripMenuItem = New ToolStripMenuItem()
        ExportMembraneTransporterToolStripMenuItem = New ToolStripMenuItem()
        ExportEnzymeDatabaseToolStripMenuItem1 = New ToolStripMenuItem()
        ExportConservedOperonDatabaseToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem3 = New ToolStripSeparator()
        ExportProteinDatabaseToolStripMenuItem = New ToolStripMenuItem()
        ToolsToolStripMenuItem = New ToolStripMenuItem()
        TaxonomySearchToolStripMenuItem = New ToolStripMenuItem()
        WindowsToolStripMenuItem = New ToolStripMenuItem()
        ShowLogWindowToolStripMenuItem = New ToolStripMenuItem()
        ToolStripMenuItem7 = New ToolStripSeparator()
        CloseAllToolStripMenuItem = New ToolStripMenuItem()
        StatusStrip1 = New StatusStrip()
        ToolStripStatusLabel1 = New ToolStripStatusLabel()
        m_dockPanel = New Microsoft.VisualStudio.WinForms.Docking.DockPanel()
        VisualStudioToolStripExtender1 = New Microsoft.VisualStudio.WinForms.Docking.VisualStudioToolStripExtender(components)
        OpenRegistrySymbolToolStripMenuItem = New ToolStripMenuItem()
        MenuStrip1.SuspendLayout()
        StatusStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {FileToolStripMenuItem, DataToolStripMenuItem, ImportsToolStripMenuItem, ExportsToolStripMenuItem, ToolsToolStripMenuItem, WindowsToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Padding = New Padding(7, 2, 0, 2)
        MenuStrip1.Size = New Size(942, 24)
        MenuStrip1.TabIndex = 1
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' FileToolStripMenuItem
        ' 
        FileToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {OpenMoleculeToolStripMenuItem, BatchOperationToolStripMenuItem, SearchNamesToolStripMenuItem, ToolStripMenuItem4, SettingsToolStripMenuItem, AboutToolStripMenuItem, ToolStripMenuItem1, ExitToolStripMenuItem})
        FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        FileToolStripMenuItem.Size = New Size(37, 20)
        FileToolStripMenuItem.Text = "File"
        ' 
        ' OpenMoleculeToolStripMenuItem
        ' 
        OpenMoleculeToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {OpenSymbolToolStripMenuItem, ClearListToolStripMenuItem, ToolStripMenuItem9})
        OpenMoleculeToolStripMenuItem.Name = "OpenMoleculeToolStripMenuItem"
        OpenMoleculeToolStripMenuItem.Size = New Size(160, 22)
        OpenMoleculeToolStripMenuItem.Text = "Open Molecule"
        ' 
        ' OpenSymbolToolStripMenuItem
        ' 
        OpenSymbolToolStripMenuItem.Name = "OpenSymbolToolStripMenuItem"
        OpenSymbolToolStripMenuItem.Size = New Size(146, 22)
        OpenSymbolToolStripMenuItem.Text = "Open Symbol"
        ' 
        ' ClearListToolStripMenuItem
        ' 
        ClearListToolStripMenuItem.Name = "ClearListToolStripMenuItem"
        ClearListToolStripMenuItem.Size = New Size(146, 22)
        ClearListToolStripMenuItem.Text = "Clear List"
        ' 
        ' ToolStripMenuItem9
        ' 
        ToolStripMenuItem9.Name = "ToolStripMenuItem9"
        ToolStripMenuItem9.Size = New Size(143, 6)
        ' 
        ' BatchOperationToolStripMenuItem
        ' 
        BatchOperationToolStripMenuItem.Name = "BatchOperationToolStripMenuItem"
        BatchOperationToolStripMenuItem.Size = New Size(160, 22)
        BatchOperationToolStripMenuItem.Text = "Batch Operation"
        ' 
        ' SearchNamesToolStripMenuItem
        ' 
        SearchNamesToolStripMenuItem.Name = "SearchNamesToolStripMenuItem"
        SearchNamesToolStripMenuItem.Size = New Size(160, 22)
        SearchNamesToolStripMenuItem.Text = "Search Names"
        ' 
        ' ToolStripMenuItem4
        ' 
        ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        ToolStripMenuItem4.Size = New Size(157, 6)
        ' 
        ' SettingsToolStripMenuItem
        ' 
        SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        SettingsToolStripMenuItem.Size = New Size(160, 22)
        SettingsToolStripMenuItem.Text = "Settings"
        ' 
        ' AboutToolStripMenuItem
        ' 
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        AboutToolStripMenuItem.Size = New Size(160, 22)
        AboutToolStripMenuItem.Text = "About"
        ' 
        ' ToolStripMenuItem1
        ' 
        ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        ToolStripMenuItem1.Size = New Size(157, 6)
        ' 
        ' ExitToolStripMenuItem
        ' 
        ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        ExitToolStripMenuItem.Size = New Size(160, 22)
        ExitToolStripMenuItem.Text = "Exit"
        ' 
        ' DataToolStripMenuItem
        ' 
        DataToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {VocabularyToolStripMenuItem, ProteinModelsToolStripMenuItem, MoleculesToolStripMenuItem, SubCellularCompartmentsToolStripMenuItem, FlavorOdorsToolStripMenuItem, OpenRegistrySymbolToolStripMenuItem, ReactionEditorToolStripMenuItem, ToolStripMenuItem2, CreateMetabolicReactionToolStripMenuItem})
        DataToolStripMenuItem.Name = "DataToolStripMenuItem"
        DataToolStripMenuItem.Size = New Size(43, 20)
        DataToolStripMenuItem.Text = "Data"
        ' 
        ' VocabularyToolStripMenuItem
        ' 
        VocabularyToolStripMenuItem.Name = "VocabularyToolStripMenuItem"
        VocabularyToolStripMenuItem.Size = New Size(271, 22)
        VocabularyToolStripMenuItem.Text = "Vocabulary"
        ' 
        ' ProteinModelsToolStripMenuItem
        ' 
        ProteinModelsToolStripMenuItem.Name = "ProteinModelsToolStripMenuItem"
        ProteinModelsToolStripMenuItem.Size = New Size(271, 22)
        ProteinModelsToolStripMenuItem.Text = "Protein Models"
        ' 
        ' MoleculesToolStripMenuItem
        ' 
        MoleculesToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {SearchToolStripMenuItem, SearchNameToolStripMenuItem, ToolStripMenuItem8, CreateNewMetaboliteToolStripMenuItem})
        MoleculesToolStripMenuItem.Name = "MoleculesToolStripMenuItem"
        MoleculesToolStripMenuItem.Size = New Size(271, 22)
        MoleculesToolStripMenuItem.Text = "Molecules"
        ' 
        ' SearchToolStripMenuItem
        ' 
        SearchToolStripMenuItem.Name = "SearchToolStripMenuItem"
        SearchToolStripMenuItem.Size = New Size(195, 22)
        SearchToolStripMenuItem.Text = "Search Text"
        ' 
        ' SearchNameToolStripMenuItem
        ' 
        SearchNameToolStripMenuItem.Name = "SearchNameToolStripMenuItem"
        SearchNameToolStripMenuItem.Size = New Size(195, 22)
        SearchNameToolStripMenuItem.Text = "Search Name"
        ' 
        ' ToolStripMenuItem8
        ' 
        ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        ToolStripMenuItem8.Size = New Size(192, 6)
        ' 
        ' CreateNewMetaboliteToolStripMenuItem
        ' 
        CreateNewMetaboliteToolStripMenuItem.Name = "CreateNewMetaboliteToolStripMenuItem"
        CreateNewMetaboliteToolStripMenuItem.Size = New Size(195, 22)
        CreateNewMetaboliteToolStripMenuItem.Text = "Create New Metabolite"
        ' 
        ' SubCellularCompartmentsToolStripMenuItem
        ' 
        SubCellularCompartmentsToolStripMenuItem.Name = "SubCellularCompartmentsToolStripMenuItem"
        SubCellularCompartmentsToolStripMenuItem.Size = New Size(271, 22)
        SubCellularCompartmentsToolStripMenuItem.Text = "Sub-Cellular Compartment Locations"
        ' 
        ' FlavorOdorsToolStripMenuItem
        ' 
        FlavorOdorsToolStripMenuItem.Name = "FlavorOdorsToolStripMenuItem"
        FlavorOdorsToolStripMenuItem.Size = New Size(271, 22)
        FlavorOdorsToolStripMenuItem.Text = "Flavor Odors"
        ' 
        ' ReactionEditorToolStripMenuItem
        ' 
        ReactionEditorToolStripMenuItem.Name = "ReactionEditorToolStripMenuItem"
        ReactionEditorToolStripMenuItem.Size = New Size(271, 22)
        ReactionEditorToolStripMenuItem.Text = "Reaction Editor"
        ' 
        ' ToolStripMenuItem2
        ' 
        ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        ToolStripMenuItem2.Size = New Size(268, 6)
        ' 
        ' CreateMetabolicReactionToolStripMenuItem
        ' 
        CreateMetabolicReactionToolStripMenuItem.Name = "CreateMetabolicReactionToolStripMenuItem"
        CreateMetabolicReactionToolStripMenuItem.Size = New Size(271, 22)
        CreateMetabolicReactionToolStripMenuItem.Text = "Create Metabolic Reaction"
        ' 
        ' ImportsToolStripMenuItem
        ' 
        ImportsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {GenBankToolStripMenuItem, PubMedKnowledgeToolStripMenuItem, PubChemIDToolStripMenuItem})
        ImportsToolStripMenuItem.Name = "ImportsToolStripMenuItem"
        ImportsToolStripMenuItem.Size = New Size(60, 20)
        ImportsToolStripMenuItem.Text = "Imports"
        ' 
        ' GenBankToolStripMenuItem
        ' 
        GenBankToolStripMenuItem.Name = "GenBankToolStripMenuItem"
        GenBankToolStripMenuItem.Size = New Size(181, 22)
        GenBankToolStripMenuItem.Text = "GenBank"
        ' 
        ' PubMedKnowledgeToolStripMenuItem
        ' 
        PubMedKnowledgeToolStripMenuItem.Name = "PubMedKnowledgeToolStripMenuItem"
        PubMedKnowledgeToolStripMenuItem.Size = New Size(181, 22)
        PubMedKnowledgeToolStripMenuItem.Text = "PubMed Knowledge"
        ' 
        ' PubChemIDToolStripMenuItem
        ' 
        PubChemIDToolStripMenuItem.Name = "PubChemIDToolStripMenuItem"
        PubChemIDToolStripMenuItem.Size = New Size(181, 22)
        PubChemIDToolStripMenuItem.Text = "PubChem ID"
        ' 
        ' ExportsToolStripMenuItem
        ' 
        ExportsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {DaisyLCMSWorkflowToolStripMenuItem, FastaDatabaseToolStripMenuItem})
        ExportsToolStripMenuItem.Name = "ExportsToolStripMenuItem"
        ExportsToolStripMenuItem.Size = New Size(58, 20)
        ExportsToolStripMenuItem.Text = "Exports"
        ' 
        ' DaisyLCMSWorkflowToolStripMenuItem
        ' 
        DaisyLCMSWorkflowToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {ExportLipidMAPSIDMappingToolStripMenuItem, ExportHMDBIDMappingToolStripMenuItem, ExportMoNAIDMappingToolStripMenuItem, ExportKEGGIDMappingToolStripMenuItem1, ExportKEGGMetaboliteTableToolStripMenuItem1, ToolStripMenuItem5, ExportMetabolitesDatabaseToolStripMenuItem1, ExportAnnotationTableToolStripMenuItem1, ToolStripMenuItem6, ExportTagDataToolStripMenuItem})
        DaisyLCMSWorkflowToolStripMenuItem.Name = "DaisyLCMSWorkflowToolStripMenuItem"
        DaisyLCMSWorkflowToolStripMenuItem.Size = New Size(190, 22)
        DaisyLCMSWorkflowToolStripMenuItem.Text = "Daisy LCMS Workflow"
        ' 
        ' ExportLipidMAPSIDMappingToolStripMenuItem
        ' 
        ExportLipidMAPSIDMappingToolStripMenuItem.Name = "ExportLipidMAPSIDMappingToolStripMenuItem"
        ExportLipidMAPSIDMappingToolStripMenuItem.Size = New Size(231, 22)
        ExportLipidMAPSIDMappingToolStripMenuItem.Text = "Export LipidMAPS IDMapping"
        ' 
        ' ExportHMDBIDMappingToolStripMenuItem
        ' 
        ExportHMDBIDMappingToolStripMenuItem.Name = "ExportHMDBIDMappingToolStripMenuItem"
        ExportHMDBIDMappingToolStripMenuItem.Size = New Size(231, 22)
        ExportHMDBIDMappingToolStripMenuItem.Text = "Export HMDB IDMapping"
        ' 
        ' ExportMoNAIDMappingToolStripMenuItem
        ' 
        ExportMoNAIDMappingToolStripMenuItem.Name = "ExportMoNAIDMappingToolStripMenuItem"
        ExportMoNAIDMappingToolStripMenuItem.Size = New Size(231, 22)
        ExportMoNAIDMappingToolStripMenuItem.Text = "Export MoNA IDMapping"
        ' 
        ' ExportKEGGIDMappingToolStripMenuItem1
        ' 
        ExportKEGGIDMappingToolStripMenuItem1.Name = "ExportKEGGIDMappingToolStripMenuItem1"
        ExportKEGGIDMappingToolStripMenuItem1.Size = New Size(231, 22)
        ExportKEGGIDMappingToolStripMenuItem1.Text = "Export KEGG IDMapping"
        ' 
        ' ExportKEGGMetaboliteTableToolStripMenuItem1
        ' 
        ExportKEGGMetaboliteTableToolStripMenuItem1.Name = "ExportKEGGMetaboliteTableToolStripMenuItem1"
        ExportKEGGMetaboliteTableToolStripMenuItem1.Size = New Size(231, 22)
        ExportKEGGMetaboliteTableToolStripMenuItem1.Text = "Export KEGG Metabolite Table"
        ' 
        ' ToolStripMenuItem5
        ' 
        ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        ToolStripMenuItem5.Size = New Size(228, 6)
        ' 
        ' ExportMetabolitesDatabaseToolStripMenuItem1
        ' 
        ExportMetabolitesDatabaseToolStripMenuItem1.Name = "ExportMetabolitesDatabaseToolStripMenuItem1"
        ExportMetabolitesDatabaseToolStripMenuItem1.Size = New Size(231, 22)
        ExportMetabolitesDatabaseToolStripMenuItem1.Text = "Export Metabolites Database"
        ' 
        ' ExportAnnotationTableToolStripMenuItem1
        ' 
        ExportAnnotationTableToolStripMenuItem1.Name = "ExportAnnotationTableToolStripMenuItem1"
        ExportAnnotationTableToolStripMenuItem1.Size = New Size(231, 22)
        ExportAnnotationTableToolStripMenuItem1.Text = "Export Annotation Table"
        ' 
        ' ToolStripMenuItem6
        ' 
        ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        ToolStripMenuItem6.Size = New Size(228, 6)
        ' 
        ' ExportTagDataToolStripMenuItem
        ' 
        ExportTagDataToolStripMenuItem.Name = "ExportTagDataToolStripMenuItem"
        ExportTagDataToolStripMenuItem.Size = New Size(231, 22)
        ExportTagDataToolStripMenuItem.Text = "Export Tag Data"
        ' 
        ' FastaDatabaseToolStripMenuItem
        ' 
        FastaDatabaseToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {ExportMembraneTransporterToolStripMenuItem, ExportEnzymeDatabaseToolStripMenuItem1, ExportConservedOperonDatabaseToolStripMenuItem, ToolStripMenuItem3, ExportProteinDatabaseToolStripMenuItem})
        FastaDatabaseToolStripMenuItem.Name = "FastaDatabaseToolStripMenuItem"
        FastaDatabaseToolStripMenuItem.Size = New Size(190, 22)
        FastaDatabaseToolStripMenuItem.Text = "Fasta Database"
        ' 
        ' ExportMembraneTransporterToolStripMenuItem
        ' 
        ExportMembraneTransporterToolStripMenuItem.Name = "ExportMembraneTransporterToolStripMenuItem"
        ExportMembraneTransporterToolStripMenuItem.Size = New Size(261, 22)
        ExportMembraneTransporterToolStripMenuItem.Text = "Export Membrane Transporter"
        ' 
        ' ExportEnzymeDatabaseToolStripMenuItem1
        ' 
        ExportEnzymeDatabaseToolStripMenuItem1.Name = "ExportEnzymeDatabaseToolStripMenuItem1"
        ExportEnzymeDatabaseToolStripMenuItem1.Size = New Size(261, 22)
        ExportEnzymeDatabaseToolStripMenuItem1.Text = "Export Enzyme Database"
        ' 
        ' ExportConservedOperonDatabaseToolStripMenuItem
        ' 
        ExportConservedOperonDatabaseToolStripMenuItem.Name = "ExportConservedOperonDatabaseToolStripMenuItem"
        ExportConservedOperonDatabaseToolStripMenuItem.Size = New Size(261, 22)
        ExportConservedOperonDatabaseToolStripMenuItem.Text = "Export Conserved Operon Database"
        ' 
        ' ToolStripMenuItem3
        ' 
        ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        ToolStripMenuItem3.Size = New Size(258, 6)
        ' 
        ' ExportProteinDatabaseToolStripMenuItem
        ' 
        ExportProteinDatabaseToolStripMenuItem.Name = "ExportProteinDatabaseToolStripMenuItem"
        ExportProteinDatabaseToolStripMenuItem.Size = New Size(261, 22)
        ExportProteinDatabaseToolStripMenuItem.Text = "Export Protein Database"
        ' 
        ' ToolsToolStripMenuItem
        ' 
        ToolsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {TaxonomySearchToolStripMenuItem})
        ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        ToolsToolStripMenuItem.Size = New Size(46, 20)
        ToolsToolStripMenuItem.Text = "Tools"
        ' 
        ' TaxonomySearchToolStripMenuItem
        ' 
        TaxonomySearchToolStripMenuItem.Name = "TaxonomySearchToolStripMenuItem"
        TaxonomySearchToolStripMenuItem.Size = New Size(167, 22)
        TaxonomySearchToolStripMenuItem.Text = "Taxonomy Search"
        ' 
        ' WindowsToolStripMenuItem
        ' 
        WindowsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {ShowLogWindowToolStripMenuItem, ToolStripMenuItem7, CloseAllToolStripMenuItem})
        WindowsToolStripMenuItem.Name = "WindowsToolStripMenuItem"
        WindowsToolStripMenuItem.Size = New Size(68, 20)
        WindowsToolStripMenuItem.Text = "Windows"
        ' 
        ' ShowLogWindowToolStripMenuItem
        ' 
        ShowLogWindowToolStripMenuItem.Name = "ShowLogWindowToolStripMenuItem"
        ShowLogWindowToolStripMenuItem.Size = New Size(173, 22)
        ShowLogWindowToolStripMenuItem.Text = "Show Log Window"
        ' 
        ' ToolStripMenuItem7
        ' 
        ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        ToolStripMenuItem7.Size = New Size(170, 6)
        ' 
        ' CloseAllToolStripMenuItem
        ' 
        CloseAllToolStripMenuItem.Name = "CloseAllToolStripMenuItem"
        CloseAllToolStripMenuItem.Size = New Size(173, 22)
        CloseAllToolStripMenuItem.Text = "Close All"
        ' 
        ' StatusStrip1
        ' 
        StatusStrip1.Items.AddRange(New ToolStripItem() {ToolStripStatusLabel1})
        StatusStrip1.Location = New Point(0, 581)
        StatusStrip1.Name = "StatusStrip1"
        StatusStrip1.Padding = New Padding(1, 0, 16, 0)
        StatusStrip1.Size = New Size(942, 22)
        StatusStrip1.TabIndex = 3
        StatusStrip1.Text = "StatusStrip1"
        ' 
        ' ToolStripStatusLabel1
        ' 
        ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        ToolStripStatusLabel1.Size = New Size(42, 17)
        ToolStripStatusLabel1.Text = "Ready!"
        ' 
        ' m_dockPanel
        ' 
        m_dockPanel.Dock = DockStyle.Fill
        m_dockPanel.Location = New Point(0, 24)
        m_dockPanel.Margin = New Padding(4)
        m_dockPanel.Name = "m_dockPanel"
        m_dockPanel.Size = New Size(942, 557)
        m_dockPanel.TabIndex = 4
        ' 
        ' VisualStudioToolStripExtender1
        ' 
        VisualStudioToolStripExtender1.DefaultRenderer = Nothing
        ' 
        ' OpenRegistrySymbolToolStripMenuItem
        ' 
        OpenRegistrySymbolToolStripMenuItem.Name = "OpenRegistrySymbolToolStripMenuItem"
        OpenRegistrySymbolToolStripMenuItem.Size = New Size(271, 22)
        OpenRegistrySymbolToolStripMenuItem.Text = "Open Registry Symbol"
        ' 
        ' FormMain
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(942, 603)
        Controls.Add(m_dockPanel)
        Controls.Add(StatusStrip1)
        Controls.Add(MenuStrip1)
        MainMenuStrip = MenuStrip1
        Margin = New Padding(4)
        Name = "FormMain"
        Text = "Registry Tool"
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VocabularyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SubCellularCompartmentsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents MoleculesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents FlavorOdorsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImportsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GenBankToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PubMedKnowledgeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenMoleculeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As ToolStripSeparator
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents WindowsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseAllToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SearchNamesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BatchOperationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SearchToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SearchNameToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReactionEditorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents m_dockPanel As Microsoft.VisualStudio.WinForms.Docking.DockPanel
    Friend WithEvents VisualStudioToolStripExtender1 As Microsoft.VisualStudio.WinForms.Docking.VisualStudioToolStripExtender
    Friend WithEvents PubChemIDToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DaisyLCMSWorkflowToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportKEGGIDMappingToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ExportKEGGMetaboliteTableToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As ToolStripSeparator
    Friend WithEvents ExportMetabolitesDatabaseToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents FastaDatabaseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportMembraneTransporterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportEnzymeDatabaseToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ExportHMDBIDMappingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportLipidMAPSIDMappingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ProteinModelsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportAnnotationTableToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ExportConservedOperonDatabaseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CreateMetabolicReactionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As ToolStripSeparator
    Friend WithEvents ExportTagDataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportMoNAIDMappingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportProteinDatabaseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripSeparator
    Friend WithEvents ShowLogWindowToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem7 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem8 As ToolStripSeparator
    Friend WithEvents ClearListToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem9 As ToolStripSeparator
    Friend WithEvents OpenSymbolToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CreateNewMetaboliteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TaxonomySearchToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenRegistrySymbolToolStripMenuItem As ToolStripMenuItem
End Class
