Imports System.ComponentModel
Imports System.IO
Imports BioNovoGene.BioDeep.Chemistry.NCBI
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem
Imports Galaxy.Workbench
Imports Galaxy.Workbench.CommonDialogs
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.VisualStudio.WinForms.Docking
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Scripting
Imports registry_data
Imports registry_data.biocad_registryModel
Imports registry_data.Exports
Imports registry_exports
Imports RegistryTool.Configs
Imports RegistryTool.My
Imports SMRUCC.genomics
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports ThemeVS2015

Public Class FormMain : Implements AppHost

    Private ReadOnly Property AppHost_ClientRectangle As Rectangle Implements AppHost.ClientRectangle
        Get
            Return New Rectangle(Location, Size)
        End Get
    End Property

    Public ReadOnly Property ActiveDocument As Form Implements AppHost.ActiveDocument
        Get
            Return m_dockPanel.ActiveDocument
        End Get
    End Property

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        App.Exit(0)
    End Sub

    Dim vS2015LightTheme1 As New VS2015LightTheme

    Private Async Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonRuntime.Hook(Me)
        initializeVSPanel()

        Await IntializeMainWindow()
    End Sub

    Private Sub initializeVSPanel()
        Me.m_dockPanel.ShowDocumentIcon = True

        Me.m_dockPanel.Dock = DockStyle.Fill
        Me.m_dockPanel.DockBackColor = Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.m_dockPanel.DockBottomPortion = 150.0R
        Me.m_dockPanel.DockLeftPortion = 200.0R
        Me.m_dockPanel.DockRightPortion = 200.0R
        Me.m_dockPanel.DockTopPortion = 150.0R
        Me.m_dockPanel.Font = New Font("Tahoma", 11.0!, FontStyle.Regular, GraphicsUnit.World, CType(0, Byte))

        Me.m_dockPanel.Name = "dockPanel"
        Me.m_dockPanel.RightToLeftLayout = True
        Me.m_dockPanel.ShowAutoHideContentOnHover = False

        Me.m_dockPanel.TabIndex = 0

        Call SetSchema(Nothing, Nothing)
    End Sub

    Private Sub SetSchema(sender As Object, e As EventArgs)
        m_dockPanel.Theme = vS2015LightTheme1
        EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015LightTheme1)

        If m_dockPanel.Theme.ColorPalette IsNot Nothing Then
            StatusStrip1.BackColor = m_dockPanel.Theme.ColorPalette.MainWindowStatusBarDefault.Background
        End If
    End Sub

    Private Sub EnableVSRenderer(version As VisualStudioToolStripExtender.VsVersion, theme As ThemeBase)
        VisualStudioToolStripExtender1.SetStyle(StatusStrip1, version, theme)
        VisualStudioToolStripExtender1.SetStyle(MenuStrip1, version, theme)
    End Sub

    Private Async Function IntializeMainWindow() As Task
        If Not MyApplication.Load Then
            Call MessageBox.Show("Application initialization error!", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        For Each entry As MoleculeEditHistory In Await Task.Run(Function() MyApplication.settings.GetHistoryItems)
            Dim item As New ToolStripMenuItem(entry.ToString) With {.Tag = entry}

            AddHandler item.Click,
                Async Sub()
                    Await Workbench.OpenMoleculeEditor(entry.id, entry.name)
                End Sub

            OpenMoleculeToolStripMenuItem.DropDownItems.Add(item)
        Next
    End Function

    Private Sub SubCellularCompartmentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SubCellularCompartmentsToolStripMenuItem.Click
        Dim view As New FormDbView()
        view.LoadTableView(Function() MyApplication.biocad_registry.compartment_location.select(Of biocad_registryModel.compartment_location)("*"))
        view.SetLLMsPrompt(Function(row)
                               Return $"please talk me about the sub-cellular compartment location: '{row.Cells(1).Value}' in a short conclusion abstract text"
                           End Function)
        view.SetTable(MyApplication.biocad_registry.compartment_location)
        view.Text = "`biocad_registry`.`compartment_location`"
        view.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)
    End Sub

    Private Sub VocabularyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VocabularyToolStripMenuItem.Click
        Dim view As New FormDbView()
        view.SetFilter("Vocabulary Category", NameOf(biocad_registryModel.vocabulary.category))
        view.LoadTableView(Function() MyApplication.biocad_registry.vocabulary.select(Of biocad_registryModel.vocabulary)("*"))
        view.SetLLMsPrompt(Function(row)
                               Return $"please talk me about the {row.Cells(1).Value} term: '{row.Cells(2).Value}' in a short conclusion abstract text"
                           End Function)
        view.SetTranslatePrompt(Function(row)
                                    Return $"将下面的这个单词名称翻译为中文：'{row.Cells(2).Value}'，如果没有正式的翻译，请进行音译。使用下面的json格式返回结果给我以方便我进行数据解析：{{""zh_name"": ""translated_name""}}"
                                End Function, fieldZh:="term_zh")
        view.SetTable(MyApplication.biocad_registry.vocabulary)
        view.Text = "`biocad_registry`.`vocabulary`"
        view.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)
    End Sub

    Private Sub ProteinModelsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProteinModelsToolStripMenuItem.Click
        Dim view As New FormDbView()
        view.LoadTableView(Function() MyApplication.biocad_registry.protein.select(Of biocad_registryModel.protein)("*"))
        view.SetLLMsPrompt(Function(row)
                               Return $"please talk me about the protein {row.Cells(1).Value}({row.Cells(4).Value}) its biological function in a short conclusion abstract text"
                           End Function)
        view.SetTable(MyApplication.biocad_registry.protein)
        view.Text = "`biocad_registry`.`protein`"
        view.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingsToolStripMenuItem.Click
        Call New FormSettings().Show(CommonRuntime.AppHost.GetDockPanel, DockState.DockRight)
    End Sub

    Private Sub MoleculesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MoleculesToolStripMenuItem.Click
        Dim view As New FormMoleculeTable
        view.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)
    End Sub

    Private Sub ExportMetabolitesDatabaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportMetabolitesDatabaseToolStripMenuItem1.Click
        Using file As New SaveFileDialog With {.Filter = "Metabolite Annotation Database(*.dat)|*.dat|Metabolite Table(*.csv)|*.csv"}
            If file.ShowDialog = DialogResult.OK Then
                MyApplication.Loading(Function(println) ExportLocal(println, file.FileName, Nothing))
                MessageBox.Show("Export metabolite local annotation repository database success!", "Task Finish", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub FlavorOdorsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FlavorOdorsToolStripMenuItem.Click
        Dim viewer As New FormOdors
        viewer.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)
    End Sub

    Private Sub GenBankToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GenBankToolStripMenuItem.Click
        Using file As New OpenFileDialog With {
            .Filter = "NCBI Genbank(*.gbff;*.gb)|*.gbff;*.gb|Genbank Compression Archive File(*.gz)|*.gz"
        }
            If file.ShowDialog = DialogResult.OK Then
                Dim gbff_stream As Stream

                If file.FileName.ExtensionSuffix("gz") Then
                    gbff_stream = file.FileName.ReadBinary.UnGzipStream
                Else
                    gbff_stream = file.FileName.Open(FileMode.Open, doClear:=False, [readOnly]:=True)
                End If

                For Each gb As GBFF.File In GBFF.File.LoadDatabase(gbff_stream)
                    Call ImportsGenBank.SaveGenBank(MyApplication.biocad_registry, gb)
                Next
            End If
        End Using
    End Sub

    Private Sub ExportEnzymeDatabaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportEnzymeDatabaseToolStripMenuItem1.Click
        Call FastaDatabase.ExportEnzymeDatabase()
    End Sub

    Private Sub ExportLipidMAPSIDMappingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportLipidMAPSIDMappingToolStripMenuItem.Click
        Using file As New SaveFileDialog With {.Filter = "id mapping file(*.json)|*.json"}
            If file.ShowDialog = DialogResult.OK Then
                MyApplication.Loading(
                    Function(println)
                        Return MyApplication.biocad_registry _
                            .ExportIDMapping("lipidmaps_id") _
                            .GetJson _
                            .SaveTo(file.FileName)
                    End Function)
                MessageBox.Show("Export LipidMAPS id mapping to local annotation repository file success!",
                                "Task Finish",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub ExportHMDBIDMappingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportHMDBIDMappingToolStripMenuItem.Click
        Using file As New SaveFileDialog With {.Filter = "id mapping file(*.json)|*.json"}
            If file.ShowDialog = DialogResult.OK Then
                MyApplication.Loading(
                    Function(println)
                        Return MyApplication.biocad_registry _
                            .ExportIDMapping("hmdb_id") _
                            .GetJson _
                            .SaveTo(file.FileName)
                    End Function)
                MessageBox.Show("Export HMDB id mapping to local annotation repository file success!",
                                "Task Finish",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub ExportKEGGIDMappingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportKEGGIDMappingToolStripMenuItem1.Click
        Using file As New SaveFileDialog With {.Filter = "id mapping file(*.json)|*.json"}
            If file.ShowDialog = DialogResult.OK Then
                MyApplication.Loading(
                    Function(println)
                        Return MyApplication.biocad_registry _
                            .ExportIDMapping("kegg_id") _
                            .GetJson _
                            .SaveTo(file.FileName)
                    End Function)
                MessageBox.Show("Export KEGG id mapping to local annotation repository file success!",
                                "Task Finish",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub ExportMoNAIDMappingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportMoNAIDMappingToolStripMenuItem.Click
        Using file As New SaveFileDialog With {.Filter = "id mapping file(*.json)|*.json"}
            If file.ShowDialog = DialogResult.OK Then
                MyApplication.Loading(
                    Function(println)
                        Return MyApplication.biocad_registry _
                            .ExportDbXrefIDMapping("MoNA") _
                            .GetJson _
                            .SaveTo(file.FileName)
                    End Function)
                MessageBox.Show("Export MoNA spectrum id mapping to metabolite id local annotation repository file success!",
                                "Task Finish",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub PubMedKnowledgeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PubMedKnowledgeToolStripMenuItem.Click
        Using file As New OpenFileDialog With {.Filter = "pubmed json(*.json)|*.json|pubmed table(*.csv)|*.csv", .Multiselect = True}
            If file.ShowDialog = DialogResult.OK Then
                Dim Topic As String = InputBox("Set Topic Term of current set of the pubmed knowledge data:")

                If Not Topic.StringEmpty(, True) Then
                    Call MyApplication.Loading(
                        Function(println)
                            'Dim kb As New PubChemArticleImports(MyApplication.biocad_registry)

                            'For Each filepath As String In file.FileNames
                            '    Call println(" -> imports: " & filepath.BaseName)

                            '    If filepath.ExtensionSuffix("json") Then
                            '        Try
                            '            Call kb.MakeImports(PubMedTextTable.ParseJSON(filepath), {Topic})
                            '        Catch ex As Exception
                            '            Call MessageBox.Show(ex, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            '        End Try
                            '    Else
                            '        Call kb.MakePageImports(PubMedTextTable.LoadTable(filepath), {Topic})
                            '    End If
                            'Next

                            'Return True
                        End Function)
                End If
            End If
        End Using
    End Sub

    Private Sub ExportAnnotationTableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportAnnotationTableToolStripMenuItem1.Click
        Using idfile As New OpenFileDialog With {.Filter = "ID List(*.txt)|*.txt"}
            If idfile.ShowDialog = DialogResult.OK Then
                Dim ids = idfile.FileName.ReadAllLines

                Using file As New SaveFileDialog With {.Filter = "Annotation Table(*.csv)|*.csv"}
                    If file.ShowDialog = DialogResult.OK Then
                        'Dim exports As New ExportMetabolites(MyApplication.biocad_registry)
                        'Dim list As MetaInfo() = TaskProgress.LoadData(Function(println As Action(Of String)) exports.setOntology("Coconut NPclass").ExportByID(ids, wrap_tqdm:=False).ToArray(Of MetaInfo))
                        'Dim df As New DataFrame With {
                        '    .rownames = list.Select(Function(a) a.ID).ToArray
                        '}

                        'For Each m As MetaInfo In list
                        '    If m.name.StringEmpty(, True) AndAlso Not m.super_class.StringEmpty(, True) Then
                        '        m.name = m.super_class & " NP" & m.formula
                        '    End If
                        'Next

                        'Call df.add("name", From m As MetaInfo In list Select m.name)
                        'Call df.add("formula", From m As MetaInfo In list Select m.formula)
                        'Call df.add("exact_mass", From m As MetaInfo In list Select m.exact_mass)
                        'Call df.add("cas", From m As MetaInfo In list Select m.xref.CAS.DefaultFirst)
                        'Call df.add("kegg", From m As MetaInfo In list Select m.xref.KEGG)
                        'Call df.add("hmdb", From m As MetaInfo In list Select m.xref.HMDB)
                        'Call df.add("lipidmaps", From m As MetaInfo In list Select m.xref.lipidmaps)
                        'Call df.add("mesh", From m As MetaInfo In list Select m.xref.MeSH)
                        'Call df.add("wikipedia", From m As MetaInfo In list Select m.xref.Wikipedia)
                        'Call df.add("smiles", From m As MetaInfo In list Select m.xref.SMILES)
                        'Call df.add("super_class", From m As MetaInfo In list Select m.super_class)
                        'Call df.add("class", From m As MetaInfo In list Select m.class)
                        'Call df.add("sub_class", From m As MetaInfo In list Select m.sub_class)

                        'Call df.WriteCsv(file.FileName, blank:="NULL")
                    End If
                End Using
            End If
        End Using
    End Sub

    Private Async Sub OpenMoleculeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenMoleculeToolStripMenuItem.Click
        Dim id = InputBox("Enter a molecule id to open data editor:")

        If Not id.StringEmpty(, True) Then
            Await Workbench.OpenMoleculeEditor(id, "")
        End If
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        For Each form As Form In Me.MdiChildren
            Call form.Close()
        Next
    End Sub

    Private Sub SearchNamesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchNamesToolStripMenuItem.Click
        Dim edit As New FormTextEditor

        edit.SetPromptText("Input the molecule name at here line by line:")
        edit.ShowDialog()

        Dim names = edit.TextLines
        Dim editor As New FormBatchEditor

        If names.IsNullOrEmpty Then
            Return
        End If

        editor.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)
    End Sub

    Private Sub BatchOperationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BatchOperationToolStripMenuItem.Click
        Dim edit As New FormTextEditor

        edit.SetPromptText("Input the molecule registry id at here line by line:")
        edit.ShowDialog()

        Dim idset = edit.TextLines
        Dim editor As New FormBatchEditor

        If idset.IsNullOrEmpty Then
            Return
        End If

        editor.LoadFromIDSet(idset)
        editor.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)
    End Sub

    Public Event ResizeForm As AppHost.ResizeFormEventHandler Implements AppHost.ResizeForm
    Public Event CloseWorkbench As AppHost.CloseWorkbenchEventHandler Implements AppHost.CloseWorkbench

    Private Sub SearchToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchToolStripMenuItem.Click
        Dim getName As String = InputBox("Input the molecule name to make search:")

        If getName.StringEmpty(, True) Then
            Return
        End If

        Dim text As String = getName.FullTextEscape
        Dim molecules As biocad_registryModel.metabolites() = TaskProgress.LoadData(
            streamLoad:=Function(println As Action(Of String))
                            Return MyApplication.biocad_registry.metabolites.where(match("name", "note").against(text, booleanMode:=False)).select(Of biocad_registryModel.metabolites)
                        End Function)

        If molecules.IsNullOrEmpty Then
            Call MessageBox.Show("Sorry, no search result.", "No result", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Call ShowMetabolitesTable(molecules, text)
        End If
    End Sub

    Friend Shared Sub ShowMetabolitesTable(molecules As biocad_registryModel.metabolites(), text As String)
        Dim view As New FormDbView()
        view.LoadTableView(Function() molecules)
        view.SetViewer(Async Sub(row)
                           Dim id As String = row.Cells(0).Value.ToString
                           Dim name As String = row.Cells(2).Value.ToString
                           Await Workbench.OpenMoleculeEditor(id, name)
                       End Sub)
        view.Text = $"Search Result of '{text}'"
        view.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)

        Call CommonRuntime.StatusMessage($"Show {molecules.TryCount} metabolite search result.")
    End Sub

    Public Sub SetWorkbenchVisible(visible As Boolean) Implements AppHost.SetWorkbenchVisible
        Me.Visible = visible
    End Sub

    Public Sub SetWindowState(stat As FormWindowState) Implements AppHost.SetWindowState
        WindowState = stat
    End Sub

    Public Function GetDesktopLocation() As Point Implements AppHost.GetDesktopLocation
        Return Location
    End Function

    Public Function GetClientSize() As Size Implements AppHost.GetClientSize
        Return Size
    End Function

    Public Function GetDocuments() As IEnumerable(Of Form) Implements AppHost.GetDocuments
        Return m_dockPanel.Documents.OfType(Of Form)
    End Function

    Public Function GetDockPanel() As Control Implements AppHost.GetDockPanel
        Return m_dockPanel
    End Function

    Public Function GetWindowState() As FormWindowState Implements AppHost.GetWindowState
        Return WindowState
    End Function

    Public Sub SetTitle(title As String) Implements AppHost.SetTitle
        Text = title
    End Sub

    Public Sub StatusMessage(msg As String, Optional icon As Image = Nothing) Implements AppHost.StatusMessage
        Call Me.Invoke(
            Sub()
                ToolStripStatusLabel1.Text = msg
                ToolStripStatusLabel1.Image = icon
            End Sub)
    End Sub

    Public Sub Warning(msg As String) Implements AppHost.Warning
        Call Me.Invoke(Sub() ToolStripStatusLabel1.Text = msg)
    End Sub

    Public Sub LogText(text As String) Implements AppHost.LogText
        Call CommonRuntime.GetOutputWindow.AppendLine(text)
    End Sub

    Public Sub ShowProperties(obj As Object) Implements AppHost.ShowProperties
        Call CommonRuntime.GetPropertyWindow.SetObject(obj)
    End Sub

    Private Sub FormMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        RaiseEvent ResizeForm(Location, Size)
    End Sub

    Private Sub FormMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        RaiseEvent CloseWorkbench(e)

        Call CommonRuntime.SaveUISettings()
    End Sub

    Private Sub ExportConservedOperonDatabaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportConservedOperonDatabaseToolStripMenuItem.Click
        Using file As New SaveFileDialog With {.Filter = "Fasta Database File(*.fasta)|*.fasta"}
            If file.ShowDialog = DialogResult.OK Then
                Using s As Stream = file.FileName.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False),
                    fasta As New SequenceModel.FASTA.StreamWriter(s)

                    'Call TaskProgress.RunAction(
                    '    Sub()
                    '        For Each seq As views.OperonGene In MyApplication.biocad_registry.ExportOperonGeneFasta
                    '            Call fasta.Add(New FastaSeq With {
                    '                .Headers = {seq.gene_id, seq.cluster_id, seq.name},
                    '                .SequenceData = seq.sequence
                    '            })
                    '        Next
                    '    End Sub, "Export Data", "Fetch sequence data and write into a fasta sequence data file...")
                End Using
            End If
        End Using
    End Sub

    Private Sub ExportKEGGMetaboliteTableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportKEGGMetaboliteTableToolStripMenuItem1.Click
        Using file As New SaveFileDialog With {.Filter = "Metabolite Table(*.csv)|*.csv"}
            If file.ShowDialog = DialogResult.OK Then
                MyApplication.Loading(Function(println) ExportLocal(println, file.FileName, "kegg_id"))
                MessageBox.Show("Export metabolite local annotation repository database success!",
                                "Task Finish",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub ExportMembraneTransporterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportMembraneTransporterToolStripMenuItem.Click
        Call FastaDatabase.ExportMembraneTransporter()
    End Sub

    Private Sub SearchNameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchNameToolStripMenuItem.Click
        Dim getName As String = InputBox("Input the molecule name to make search:")

        If getName.StringEmpty(, True) Then
            Return
        End If

        Dim hashcode As String = getName.Trim.ToLower.MD5
        Dim fulltext As String = getName.FullTextEscape
        Dim metabolite_class = MyApplication.biocad_registry.biocad_vocabulary.metabolite_type

        Dim idset As UInteger() = TaskProgress.LoadData(
            streamLoad:=Function(p As ITaskProgress)
                            Return MyApplication.biocad_registry.synonym _
                                .where(field("type") = metabolite_class And (field("hashcode") = hashcode Or match("synonym").against(fulltext, False))) _
                                .project(Of UInteger)("obj_id")
                        End Function)

        If idset.IsNullOrEmpty Then
            Call MessageBox.Show("Sorry, no search result.", "No result", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Call ShowMetabolitesTable(MyApplication.biocad_registry.metabolites.where(field("id").in(idset)).select(Of biocad_registryModel.metabolites), fulltext)
        End If
    End Sub

    Private Sub ReactionEditorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReactionEditorToolStripMenuItem.Click
        Call CommonRuntime.ShowDocument(Of FormMetabolicEditor)()
    End Sub

    Private Sub PubChemIDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PubChemIDToolStripMenuItem.Click
        Dim cid As String = InputBox("Input the pubchem cid to make imports into registry database!", "Imports PubChem")

        If cid.StringEmpty(, True) Then
            Return
        Else
            Call TaskProgress.LoadData(
                streamLoad:=Function(p As ITaskProgress) importsCID(cid, p),
                title:=$"fetch pubchem metabolite of cid={cid}",
                info:="Make pubchem metabolite data imports"
            )
        End If
    End Sub

    Private Function importsCID(cid As String, p As ITaskProgress) As Boolean
        Dim data = PubChem.Query.FetchPugViewByCID(Strings.Trim(cid))
        Dim meta = data.GetMetaInfo
        ' 不信任pubchem id的映射结果，在这里直接设置kegg_id来避免直接通过pubchem id查找到结果
        Dim m As metabolites = MyApplication.biocad_registry.FindMolecule(meta, "kegg_id", nameSearch:=True)
        Dim db_pubchem As UInteger = MyApplication.biocad_registry.biocad_vocabulary.db_pubchem

        If m Is Nothing Then
            Return False
        Else
            Call p.SetInfo("save mysql")
        End If

        Call MyApplication.biocad_registry.SaveDbLinks(meta, m, db_pubchem)
        Call MyApplication.biocad_registry.SaveStructureData(m, meta.xref.SMILES)
        Call MyApplication.biocad_registry.SaveSynonyms(m, meta.synonym.JoinIterates({meta.name, meta.IUPACName}).Distinct, db_pubchem)

        Return True
    End Function

    Private Sub CreateMetabolicReactionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateMetabolicReactionToolStripMenuItem.Click
        Call InputDialog.Input(Of FormBuildReaction)()
    End Sub

    Private Sub ExportTagDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportTagDataToolStripMenuItem.Click
        Call CommonRuntime.ShowDocument(Of FormMetabolicTopicView)()
    End Sub

    Private Sub ExportProteinDatabaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportProteinDatabaseToolStripMenuItem.Click
        Call FastaDatabase.ExportProteinDatabase()
    End Sub

    Private Sub ShowLogWindowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowLogWindowToolStripMenuItem.Click
        Call CommonRuntime.Dock(CommonRuntime.GetOutputWindow, DockState.DockBottom)
    End Sub

    Private Async Sub OpenSymbolToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenSymbolToolStripMenuItem.Click
        Dim symbol_name = InputBox("Enter metabolite registry symbol name to open:")

        If symbol_name.StringEmpty(, True) Then
            Return
        End If

        Dim meta_id = Await MyApplication.biocad_registry.registry_resolver _
            .async _
            .where(field("type") = MyApplication.biocad_registry.biocad_vocabulary.metabolite_type,
                   field("register_name") = symbol_name) _
            .find(Of registry_resolver)

        If meta_id Is Nothing Then
            CommonRuntime.Warning($"Could not found metabolite which is associated with registry symbol '{symbol_name}'")
        Else
            Await OpenMoleculeEditor(meta_id.symbol_id, meta_id.register_name)
        End If
    End Sub

    Private Sub ClearListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearListToolStripMenuItem.Click
        MyApplication.settings.molecule_history = {}
        MyApplication.settings.Save()

        Call CommonRuntime.StatusMessage("Molecule view history all has been cleared.")
    End Sub

    Private Async Sub CreateNewMetaboliteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateNewMetaboliteToolStripMenuItem.Click
        Dim name As String = InputBox("Input new metabolite name:")

        If name.StringEmpty(, True) Then
            Return
        ElseIf MessageBox.Show($"Going to create new metabolite which is named: {name}?",
                               "Create New Metabolite",
                               MessageBoxButtons.OKCancel,
                               MessageBoxIcon.Information) <> DialogResult.OK Then
            Return
        End If

        name = name.Trim

        Dim key As String = name.ToLower.MD5

        Await MyApplication.biocad_registry.metabolites.async.add(field("name") = name, field("hashcode") = key, field("formula") = "", field("exact_mass") = 0)

        Dim findNew = Await MyApplication.biocad_registry.metabolites _
            .async _
            .where(field("hashcode") = key) _
            .order_by("id", desc:=True) _
            .find(Of biocad_registryModel.metabolites)

        If findNew Is Nothing Then
            Call CommonRuntime.Warning($"Create metabolite '{name}' error.")
        Else
            Await Workbench.OpenMoleculeEditor(findNew.id, findNew.name)
        End If
    End Sub

    Private Async Sub TaxonomySearchToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TaxonomySearchToolStripMenuItem.Click
        Dim taxnames As String() = Workbench.InputTextData

        If taxnames.IsNullOrEmpty Then
            Return
        End If

        taxnames = taxnames.JoinIterates(taxnames.Select(Function(s) s.Replace("_", " "))) _
            .Select(Function(s) s.StringReplace("\s{2,}", " ").Trim) _
            .Distinct _
            .ToArray

        Dim dataset As New List(Of TaxonomyData)

        For Each name As String In taxnames
            Call dataset.AddRange(Await TaxonomyData.Find(name))
        Next

        Dim view As New FormDbView()
        view.LoadTableView(Function() dataset.ToArray)
        view.SetViewer(Sub(row)

                       End Sub)
        view.Text = $"Search Result of 'Taxonomy Tree'"
        view.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)

        Call CommonRuntime.StatusMessage($"Show {taxnames.Length / 2} taxonomy name search result.")
    End Sub

    Private Async Sub OpenRegistrySymbolToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenRegistrySymbolToolStripMenuItem.Click
        Dim symbol As String = InputBox("Input registry symbol for open to view model object:")

        If Not symbol.StringEmpty(, True) Then
            Dim reg_symbol = Workbench.cad_registry.registry_resolver.where(field("register_name") = symbol).find(Of registry_resolver)

            If reg_symbol IsNot Nothing Then
                Select Case reg_symbol.type
                    Case Workbench.cad_registry.biocad_vocabulary.metabolite_type
                        Await Workbench.OpenMoleculeEditor(reg_symbol.symbol_id, reg_symbol.register_name)
                End Select
            End If
        End If
    End Sub
End Class

