Imports BioNovoGene.BioDeep.Chemoinformatics
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports BioNovoGene.BioDeep.Chemoinformatics.SMILES
Imports Galaxy.Data.TextValidates
Imports Galaxy.Workbench
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Emit.Marshal
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.text.markdown
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.VisualStudio.WinForms.Docking
Imports Microsoft.Web.WebView2.Core
Imports Ollama
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports registry_data
Imports registry_data.biocad_registryModel
Imports RegistryTool.Configs
Imports RegistryTool.My
Imports SMRUCC.genomics.Model.MotifGraph

Public Class FormMoleculeEditor

    Public Const viewer As String = "<!DOCTYPE html SYSTEM ""about:legacy-compat"">
<html xmlns=""http://www.w3.org/1999/xhtml"" lang=""en"" xml:lang=""en"">

<head>    
    <script type=""text/javascript"" src=""http://biocad.innovation.ac.cn/resources/vendor/smiles-drawer.min.js""></script>
    <script id=""struct-data"" type=""plain/text"">{$struct_data}</script>
</head>

<body>
<canvas id=""smiles-canvas"" width=""450px"" height=""300px"" role=""img"" focusable=""false""
                            style=""padding: 5px;""></canvas>
</body>

<script type=""text/javascript"">
let options = { width: 450, height: 300 };
    // Initialize the drawer to draw to canvas
    let smilesDrawer = new SmilesDrawer.Drawer(options);
    // Alternatively, initialize the SVG drawer:
    // let svgDrawer = new SmilesDrawer.SvgDrawer(options);

    function rendering(input_value) {
        // Clean the input (remove unrecognized characters, such as spaces and tabs) and parse it
        SmilesDrawer.parse(input_value, function (tree) {
            // Draw to the canvas
            smilesDrawer.draw(tree, ""smiles-canvas"", ""light"", false);
            // Alternatively, draw to SVG:
            // svgDrawer.draw(tree, 'output-svg', 'dark', false);
        });
    }

    rendering(document.getElementById(""struct-data"").innerText);
</script>
</html>"

    Dim struct As biocad_registryModel.struct_data
    Dim mol As biocad_registryModel.metabolites
    Dim morgan As ProteinStructure.MorganFingerprint

    Public Async Function LoadModel(id As String) As Task(Of Boolean)
        mol = Await Task.Run(Function() MyApplication.biocad_registry.metabolites _
            .where(field("id") = UInteger.Parse(id.Match("\d+"))) _
            .find(Of biocad_registryModel.metabolites))

        If mol Is Nothing Then
            MessageBox.Show($"There is no molecule object that associated with the given unique id: {id}", "Missing Object", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        Else
            Return True
        End If
    End Function

    Private Async Sub FormMoleculeEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If mol Is Nothing Then
            Throw New InvalidProgramException("Please load the molecule model data via load model method at first!")
        Else
            txtBioCYCID.Text = mol.biocyc
            txtCASID.Text = mol.cas_id
            txtCHEBIID.Text = mol.chebi_id
            txtDrugBankID.Text = mol.drugbank_id
            txtHMDBID.Text = mol.hmdb_id
            txtKEGGID.Text = mol.kegg_id
            txtLipidMapsID.Text = mol.lipidmaps_id
            txtMeshID.Text = mol.mesh_id
            txtPubChemCID.Text = mol.pubchem_cid
            txtWikipediaID.Text = mol.wikipedia
        End If

        SMILES.MolecularFingerprint.Length = 1024
        morgan = New ProteinStructure.MorganFingerprint(SMILES.MolecularFingerprint.Length)

        MyApplication.settings.molecule_history = New MoleculeEditHistory() {
                New MoleculeEditHistory With {.id = mol.id, .name = mol.name}
        } _
            .JoinIterates(MyApplication.settings.molecule_history) _
            .Take(50) _
            .ToArray
        MyApplication.settings.Save()

        TextBox8.Text = Await MyApplication.biocad_registry.registry_resolver _
            .async _
            .where(field("type") = MyApplication.biocad_registry.biocad_vocabulary.metabolite_type,
                   field("symbol_id") = mol.id) _
            .find_scalar("register_name")

        TextBox2.Text = mol.name
        TextBox3.Text = mol.formula
        Label7.Text = FormulaScanner.EvaluateExactMass(mol.formula).ToString("F4")
        TextBox4.Text = mol.note
        LinkLabel1.Text = $"{MyApplication.settings.website}/metabolite/BioCAD{mol.id.ToString.PadLeft(11, "0"c)}/"
        TextBox9.Text = mol.name_zh

        struct = MyApplication.biocad_registry.struct_data _
            .where(field("metabolite_id") = mol.id) _
            .find(Of biocad_registryModel.struct_data)

        If Not struct Is Nothing Then
            TextBox1.Text = struct.smiles
            TextBox5.Text = struct.fingerprint
        End If

        For Each term As TopicTerm In TopicTerm.GetTopics
            Call ComboBox2.Items.Add(term)
        Next

        For Each term As TopicTerm In TopicTerm.GetDatabaseTerm
            Call ComboBox3.Items.Add(term)
        Next

        Dim taxonomy As OrganismSource() = MyApplication.biocad_registry.organism_source _
            .left_join("ncbi_taxonomy") _
            .on(field("`ncbi_taxonomy`.id") = field("organism_id")) _
            .where(field("metabolite_id") = mol.id) _
            .distinct _
            .select(Of OrganismSource)("organism_id as ncbi_taxid", "`ncbi_taxonomy`.name as taxname")

        Call ListBox3.Items.Clear()

        For Each link As OrganismSource In taxonomy.OrderBy(Function(a) a.taxname)
            If Not link.taxname.StringEmpty(, True) Then
                Call ListBox3.Items.Add(link)
            End If
        Next

        Call refreshNames()
        Await refreshXrefs()
        Call refreshTags()

        Call ApplyVsTheme(ToolStrip1)

        Await LoadReactions()
        Await WebViewLoader.Init(WebView21)
    End Sub

    Private Async Function LoadReactions() As Task
        Dim reaction_ids As UInteger() = Await MyApplication.biocad_registry.metabolic_network _
            .async _
            .where(field("species_id") = mol.id) _
            .distinct _
            .project(Of UInteger)("reaction_id")

        If reaction_ids.IsNullOrEmpty Then
            Return
        End If

        Dim reactions As biocad_registryModel.reaction() = Await MyApplication.biocad_registry.reaction _
            .async _
            .where(field("id").in(reaction_ids)) _
            .select(Of biocad_registryModel.reaction)

        For Each rxn In reactions
            Dim offset = DataGridView2.Rows.Add(rxn.name, rxn.ec_number, rxn.equation, rxn.note)
            DataGridView2.Rows(offset).HeaderCell.Value = rxn.id
            DataGridView2.Rows(offset).Tag = rxn.note
        Next

        Call DataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Function

    Private Sub refreshNames(Optional lang As String = Nothing)
        Dim q As FieldAssert() = {
            field("obj_id") = mol.id,
            field("type") = MyApplication.biocad_registry.biocad_vocabulary.metabolite_type
        }

        If Not lang.StringEmpty(, True) Then
            q = q _
                .JoinIterates(field("lang") = lang) _
                .ToArray
        End If

        Dim names = MyApplication.biocad_registry.synonym _
            .where(q) _
            .order_by("synonym") _
            .select(Of biocad_registryModel.synonym) _
            .OrderBy(Function(a) a.synonym) _
            .ToArray

        Call ListBox1.Items.Clear()

        For Each name As biocad_registryModel.synonym In names
            Call ListBox1.Items.Add(name.synonym)
        Next
    End Sub

    Private Async Function refreshXrefs() As Task
        Dim xrefs = Await MyApplication.biocad_registry.db_xrefs _
            .async _
            .left_join("vocabulary") _
            .on((field("`vocabulary`.id") = field("db_name"))) _
            .where(field("obj_id") = mol.id, field("type") = MyApplication.biocad_registry.biocad_vocabulary.metabolite_type) _
            .order_by("db_name") _
            .select(Of XrefID)("db_xrefs.id as xref_id", "term as dbname", "db_xref as xref", "db_name as db_key")

        DataGridView1.Rows.Clear()

        For Each id As XrefID In xrefs
            Dim offset = DataGridView1.Rows.Add(id.dbname, id.xref)
            DataGridView1.Rows(offset).Tag = id
        Next

        DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Function

    Private Sub refreshTags()
        Call ListBox2.Items.Clear()

        For Each tag As Tag In Tag.GetTags(mol.id)
            Call ListBox2.Items.Add(tag)
        Next
    End Sub

    Private Async Sub SaveCommonName() Handles Button2.Click
        Dim name As String = Strings.Trim(TextBox2.Text)

        Await MyApplication.biocad_registry.metabolites _
            .async _
            .where(field("id") = mol.id) _
            .save(field("name") = name)
    End Sub

    Private Sub WebView21_CoreWebView2InitializationCompleted(sender As Object, e As CoreWebView2InitializationCompletedEventArgs) Handles WebView21.CoreWebView2InitializationCompleted
        If Not struct Is Nothing Then
            Call WebView21.NavigateToString(viewer.Replace("{$struct_data}", struct.smiles))
        End If
    End Sub

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim smilesOrSeq As String = Strings.Trim(TextBox1.Text)
        Dim checksum As Byte()
        Dim fingerprint As String = Nothing

        Await Task.Run(Sub()
                           checksum = MolecularFingerprint.ConvertToMorganFingerprint(smilesOrSeq, radius:=3)
                           fingerprint = checksum.GZipAsBase64
                       End Sub)

        TextBox5.Text = fingerprint

        If struct Is Nothing Then
            Await MyApplication.biocad_registry.struct_data.async.add(
                                   field("smiles") = smilesOrSeq,
                                        field("fingerprint") = fingerprint,
                                        field("checksum") = smilesOrSeq.MD5,
                                        field("metabolite_id") = mol.id
                               )

            struct = Await MyApplication.biocad_registry.struct_data.async _
                                  .where(field("metabolite_id") = mol.id) _
                                  .order_by("id", desc:=True) _
                                  .find(Of biocad_registryModel.struct_data)

            WebView21_CoreWebView2InitializationCompleted(Nothing, Nothing)
        Else
            Await MyApplication.biocad_registry.struct_data _
                .async _
                .where(field("id") = struct.id) _
                .save(field("smiles") = smilesOrSeq,
                        field("fingerprint") = fingerprint,
                        field("checksum") = smilesOrSeq.MD5)

        End If
    End Sub

    Private Sub SetAsDisplayNameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetAsDisplayNameToolStripMenuItem.Click
        If ListBox1.SelectedIndex < 0 Then
            Return
        End If

        Dim lang As String = CStr(ComboBox1.SelectedItem)

        If lang <> "en" Then
            MessageBox.Show("Only allows synonym names in english language to be set as display name.", "Invalid Language", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return
        End If

        Dim name = ListBox1.Items(ListBox1.SelectedIndex).ToString
        TextBox2.Text = name
        Call SaveCommonName()
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim row = DataGridView1.SelectedRows(0)
        Dim db As String = CStr(row.Cells(0).Value)
        Dim xref As String = CStr(row.Cells(1).Value)
        Dim url As String

        Select Case LCase(db)
            Case "pubchem" : url = $"https://pubchem.ncbi.nlm.nih.gov/compound/{xref}"
            Case Else
                Call CommonRuntime.StatusMessage($"Unkonw url builder for db xref {db}:{xref}")
                Return
        End Select

        Call Tools.OpenUrlWithDefaultBrowser(url)
    End Sub

    Private Async Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim desc = Strings.Trim(TextBox4.Text)

        Await MyApplication.biocad_registry.metabolites _
            .async _
            .where(field("id") = mol.id) _
            .save(field("note") = desc)
    End Sub

    Private Sub LinkLabel1_LinkClicked() Handles LinkLabel1.Click
        OpenUrlWithDefaultBrowser(LinkLabel1.Text)
    End Sub

    Private Async Sub ClearThisTagToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearThisTagToolStripMenuItem.Click
        Dim sel As Tag = ListBox2.SelectedItem

        If sel Is Nothing Then
            Return
        End If

        Dim model As registry_resolver = Await MyApplication.biocad_registry.registry_resolver _
            .async _
            .where(field("type") = MyApplication.biocad_registry.biocad_vocabulary.metabolite_type,
                   field("symbol_id") = sel.molecule_id) _
            .find(Of registry_resolver)

        Await MyApplication.biocad_registry.topic _
            .async _
            .where(field("topic_id") = sel.tag_id,
                   field("model_id") = model.id) _
            .delete()

        Call refreshTags()
    End Sub

    Private Async Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        Dim sel = DataGridView1.SelectedRows

        If sel.Count = 0 Then
            Return
        End If

        Dim db As XrefID = sel(0).Tag
        Dim all_xrefs = MyApplication.biocad_registry.db_xrefs _
            .where(field("db_name") = db.db_key,
                   field("type") = Terms.metabolite_type,
                   field("obj_id") = mol.id) _
            .select(Of biocad_registryModel.db_xrefs)
        Dim edit As New FormTextEditor

        edit.SetText(all_xrefs.Select(Function(a) a.db_xref))
        edit.SetPromptText("Edit the database xref id(delete text for removes id from database, add text for add new id into database)")
        edit.ShowDialog()

        Dim current As Dictionary(Of String, db_xrefs()) = all_xrefs _
            .GroupBy(Function(a) a.db_xref) _
            .ToDictionary(Function(a) a.Key,
                          Function(a)
                              Return a.ToArray
                          End Function)

        Call TaskProgress.RunAction(
            Sub(println As ITaskProgress)
                Call println.SetInfo("Commit data changes into the database...")

                For Each id As String In edit.TextLines
                    If current.ContainsKey(id) Then
                        ' no changed
                    Else
                        ' add new
                        Call MyApplication.biocad_registry.db_xrefs.add(
                            field("db_name") = db.db_key,
                            field("obj_id") = mol.id,
                            field("db_xref") = id,
                            field("type") = Terms.metabolite_type
                        )
                    End If
                Next

                Dim modified As Index(Of String) = edit.TextLines.Indexing

                For Each key As String In current.Keys
                    If key Like modified Then
                        ' no changed
                    Else
                        ' deleted
                        Call MyApplication.biocad_registry.db_xrefs.where(field("db_name") = db.db_key,
                            field("obj_id") = mol.id,
                            field("db_xref") = key,
                            field("type") = Terms.metabolite_type).delete()
                    End If
                Next
            End Sub)

        Await refreshXrefs()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If ComboBox2.SelectedIndex < 0 Then
            Return
        End If

        Dim term As TopicTerm = ComboBox2.SelectedItem

        'If MyApplication.biocad_registry.molecule_tags _
        '    .where(field("tag_id") = term.id,
        '           field("molecule_id") = mol.id) _
        '    .find(Of biocad_registryModel.molecule_tags) Is Nothing Then

        '    Call MyApplication.biocad_registry.molecule_tags.add(
        '        field("tag_id") = term.id,
        '        field("molecule_id") = mol.id,
        '        field("description") = "Molecule Editor"
        '    )
        '    Call refreshTags()
        'End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex <= 0 Then
            ' show all
            Call refreshNames()
        Else
            Call refreshNames(lang:=CStr(ComboBox1.SelectedItem))
        End If
    End Sub

    Private Sub EditToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem1.Click
        If ComboBox1.SelectedIndex <= 0 Then
            Call MessageBox.Show("A language for the synonym name must be selected!",
                                 "Invalid Language",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Warning)
            Return
        End If

        Dim lang As String = CStr(ComboBox1.SelectedItem)
        Dim edit As New FormTextEditor
        Dim names = MyApplication.biocad_registry.synonym.where(field("lang") = lang, field("obj_id") = mol.id).project(Of String)("synonym")

        Call edit.SetText(names)
        Call edit.SetPromptText($"edit the synonym names({lang}):")
        Call edit.ShowDialog()

        Dim editData As String() = edit.TextLines

        Call TaskProgress.RunAction(Sub(println As ITaskProgress)
                                        Call println.SetInfo("Commit modified data to database...")
                                        Call UpdateSynonymNames(names.Indexing, editData, lang)
                                    End Sub)

        Call refreshNames(lang)
    End Sub

    Private Sub UpdateSynonymNames(current_old As Index(Of String), editData As String(), lang As String)
        Dim metabolite_type As UInteger = MyApplication.biocad_registry.biocad_vocabulary.metabolite_type
        Dim manual_edit As UInteger = MyApplication.biocad_registry.biocad_vocabulary.db_ManualAudit

        For Each name As String In editData
            If name Like current_old Then
                ' no changed
            Else
                ' add new
                Call MyApplication.biocad_registry.synonym.add(
                    field("type") = metabolite_type,
                    field("obj_id") = mol.id,
                    field("synonym") = name,
                    field("lang") = lang,
                    field("hashcode") = name.ToLower.MD5,
                    field("db_source") = manual_edit
                )
            End If
        Next

        Dim modified As Index(Of String) = editData.Indexing

        For Each key As String In current_old.Objects
            If key Like modified Then
                ' no changed
            Else
                ' deleted
                Call MyApplication.biocad_registry.synonym _
                    .where(field("obj_id") = mol.id,
                           field("lang") = lang,
                           field("type") = metabolite_type,
                          field("synonym") = key).delete()
            End If
        Next
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim prompt As String = $"please talk me about the biological function of the compound: '{TextBox2.Text}' in a short conclusion abstract text"
        Dim msg As OllamaResponse = TaskProgress.LoadData(Function(println As Action(Of String)) MyApplication.ollama.Chat(prompt).GetAwaiter.GetResult)
        Dim markdown As New MarkdownRender

        If Not msg Is Nothing Then
            Try
                TextBox4.Text = markdown.Transform(msg.output)
            Catch ex As Exception
                TextBox4.Text = msg.output
            End Try
        End If
    End Sub

    Private Async Sub ChineseNameTranslationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChineseNameTranslationToolStripMenuItem.Click
        If ListBox1.SelectedIndex < 0 Then
            Return
        End If

        Dim lang As String = CStr(ComboBox1.SelectedItem)

        If lang = "zh" Then
            MessageBox.Show("there is no needs of translate the chinese synonym names into another chinese name.", "Invalid Language", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return
        End If

        Dim name = ListBox1.Items(ListBox1.SelectedIndex).ToString
        Dim prompt As String = $"将下面的这个化合物名称翻译为中文：'{name}'，如果没有正式的翻译，请进行音译。使用下面的json格式返回结果给我以方便我进行数据解析：{{""zh_name"": ""translated_name""}}"
        Dim msg As OllamaResponse = TaskProgress.LoadData(Function(println As Action(Of String)) MyApplication.ollama.Chat(prompt).GetAwaiter.GetResult)
        Dim class_id As UInteger = MyApplication.biocad_registry.biocad_vocabulary.metabolite_type
        Dim llms_source As UInteger = MyApplication.biocad_registry.biocad_vocabulary.db_LLMs
        Dim zh_name As String = TranslatedName.DecodeLLMTranslateOutput(msg?.output)

        If Not zh_name.StringEmpty(, True) Then
            Dim hashcode As String = zh_name.ToString.ToLower.MD5

            If Await MyApplication.biocad_registry.synonym _
                    .async _
                    .where(field("type") = class_id,
                            field("obj_id") = mol.id,
                            field("hashcode") = hashcode,
                            field("lang") = "zh") _
                    .find(Of biocad_registryModel.synonym) Is Nothing Then

                Await MyApplication.biocad_registry.synonym.async.add(
                    field("type") = class_id,
                    field("obj_id") = mol.id,
                    field("db_source") = llms_source,
                    field("synonym") = zh_name.ToString,
                    field("lang") = "zh",
                    field("hashcode") = hashcode
                )
            End If
        End If
    End Sub

    Private Async Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If ComboBox3.SelectedIndex < 0 Then
            Return
        End If
        Dim new_id As String = Strings.Trim(TextBox6.Text)
        Dim dbname As TopicTerm = ComboBox3.SelectedItem

        If new_id.StringEmpty(, True) Then
            Return
        End If

        'If MyApplication.biocad_registry.db_xrefs.where(field("db_key") = dbname.id,
        '    field("obj_id") = mol.id,
        '    field("xref") = new_id,
        '    field("type") = mol.type).find(Of biocad_registryModel.db_xrefs) Is Nothing Then

        '    Call MyApplication.biocad_registry.db_xrefs.add(
        '        field("db_key") = dbname.id,
        '        field("obj_id") = mol.id,
        '        field("xref") = new_id,
        '        field("type") = mol.type
        '    )
        'End If

        Await refreshXrefs()
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.SelectedIndex < 0 Then
            Return
        End If

        Dim dbname As TopicTerm = ComboBox3.SelectedItem
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedIndex > -1 Then
            CommonRuntime.StatusMessage(ListBox1.SelectedItem.ToString)
        End If
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        If ListBox1.SelectedIndex > -1 Then
            Call Clipboard.SetText(ListBox1.SelectedItem.ToString)
        End If
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        Dim formula = Trim(TextBox3.Text)
        Dim exact_mass = FormulaScanner.EvaluateExactMass(formula)

        Label7.Text = exact_mass.ToString("F4")
    End Sub

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim formula = Trim(TextBox3.Text)
        Dim exact_mass = FormulaScanner.EvaluateExactMass(formula)

        Await MyApplication.biocad_registry.metabolites.async.where(field("id") = mol.id).save(
            field("formula") = formula,
            field("exact_mass") = exact_mass
        )
    End Sub

    Private Sub ListSourceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListSourceToolStripMenuItem.Click
        If ListBox3.SelectedIndex < 0 Then
            Return
        End If

        Dim source As OrganismSource = DirectCast(ListBox3.SelectedItem, OrganismSource)
        '    Dim molecules As MoleculeSearch() = TaskProgress _
        '        .LoadData(Function(println As Action(Of String))
        '                      Return MyApplication.biocad_registry.taxonomy_source _
        '                        .left_join("molecule").on(field("`molecule`.id") = field("molecule_id")) _
        '                        .left_join("vocabulary").on(field("`vocabulary`.id") = field("`molecule`.type")) _
        '                        .where(field("ncbi_taxid") = source.ncbi_taxid) _
        '                        .distinct _
        '                        .select(Of MoleculeSearch)("`molecule`.id",
        '"`molecule`.name",
        '"formula",
        '"mass",
        '"term AS type",
        '"`molecule`.note")
        '                  End Function)
        '    Dim view As New FormDbView()
        '    view.LoadTableView(Function() molecules)
        '    view.SetViewer(Sub(row)
        '                       Dim id As String = row.Cells(0).Value.ToString
        '                       Dim name As String = row.Cells(1).Value.ToString

        '                       Call Workbench.OpenMoleculeEditor(id, name)
        '                   End Sub)
        '    view.Text = $"Metabolite From Taxonomy '{source}'"
        '    view.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)
    End Sub

    Private Sub SearchByThisDbXrefToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchByThisDbXrefToolStripMenuItem.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim link As XrefID = DataGridView1.SelectedRows(0).Tag

        If link Is Nothing Then
            Return
        End If

        Dim molecules As MoleculeSearch() = TaskProgress _
            .LoadData(Function(println As Action(Of String))
                          Return MyApplication.biocad_registry.db_xrefs _
                            .left_join("molecule").on(field("`molecule`.id") = field("obj_id")) _
                            .left_join("vocabulary").on(field("`vocabulary`.id") = field("`molecule`.type")) _
                            .where(field("db_key") = link.db_key, field("xref") = link.xref) _
                            .distinct _
                            .select(Of MoleculeSearch)("`molecule`.id",
    "`molecule`.name",
    "formula",
    "mass",
    "term AS type",
    "`molecule`.note")
                      End Function)
        Dim view As New FormDbView()
        view.LoadTableView(Function() molecules)
        view.SetViewer(Sub(row)
                           Dim id As String = row.Cells(0).Value.ToString
                           Dim name As String = row.Cells(1).Value.ToString

                           Call Workbench.OpenMoleculeEditor(id, name)
                       End Sub)
        view.Text = $"Metabolite with Xref '{link}'"
        view.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)
    End Sub

    Private Sub OpenMoleculeDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenMoleculeDataToolStripMenuItem.Click
        If DataGridView3.SelectedRows.Count = 0 Then
            Return
        End If

        Dim meta = DataGridView3.SelectedRows(0)
        Dim registry_id As String = CStr(meta.Cells(0).Value)
        Dim name As String = CStr(meta.Cells(1).Value)

        If registry_id <> "" Then
            Call Workbench.OpenMoleculeEditor(registry_id, name)
        End If
    End Sub

    Private Async Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick
        If DataGridView2.SelectedRows.Count = 0 Then
            Return
        End If

        Dim rxn = DataGridView2.SelectedRows(0)
        Dim rxn_id As String = CStr(rxn.HeaderCell.Value)
        Dim role_left = MyApplication.biocad_registry.MetabolicSubstrateRole
        Dim role_right = MyApplication.biocad_registry.MetabolicProductRole

        Dim graph = Await MyApplication.biocad_registry.metabolic_network _
                    .async _
                    .left_join("metabolites") _
                    .on(field("species_id") = field("metabolites.id")) _
                    .left_join("vocabulary") _
                    .on(field("vocabulary.id") = field("role")) _
                    .where(field("reaction_id") = UInteger.Parse(rxn_id)) _
                    .select(Of reaction_graphdata)("metabolites.*", "symbol_id as db_xref", "term AS role")

        TextBox7.Text = rxn.Tag
        DataGridView3.Rows.Clear()

        For Each compound In graph
            Call DataGridView3.Rows.Add(
                compound.id,
                compound.db_xref,
               compound.name,
                compound.formula,
                compound.exact_mass,
                compound.role
           )
        Next
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Call SaveMainXrefID(txtCASID, "cas_id")
    End Sub

    Private Async Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If txtPubChemCID.StringEmpty(, True) Then
            ' make clear
            Await MyApplication.biocad_registry.metabolites _
                .async _
                .where(field("id") = mol.id) _
                .save(field("pubchem_cid") = Nothing)
        Else
            Await MyApplication.biocad_registry.metabolites _
                .async _
                .where(field("id") = mol.id) _
                .save(field("pubchem_cid") = Strings.Trim(txtPubChemCID.Text).Match("\d+"))
        End If
    End Sub

    Private Async Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If txtCHEBIID.StringEmpty(, True) Then
            ' make clear
            Await MyApplication.biocad_registry.metabolites _
                .async _
                .where(field("id") = mol.id) _
                .save(field("chebi_id") = Nothing)
        Else
            Await MyApplication.biocad_registry.metabolites _
                .async _
                .where(field("id") = mol.id) _
                .save(field("chebi_id") = Strings.Trim(txtCHEBIID.Text).Match("\d+"))
        End If
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Call SaveMainXrefID(txtHMDBID, "hmdb_id")
    End Sub

    Private Async Sub SaveMainXrefID(source As TextBox, name As String)
        If source.StringEmpty(, True) Then
            ' make clear
            Await MyApplication.biocad_registry.metabolites _
                .async _
                .where(field("id") = mol.id) _
                .save(field(name).set_nothing)
        Else
            Await MyApplication.biocad_registry.metabolites _
                .async _
                .where(field("id") = mol.id) _
                .save(field(name) = Strings.Trim(source.Text))
        End If
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Call SaveMainXrefID(txtLipidMapsID, "lipidmaps_id")
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Call SaveMainXrefID(txtKEGGID, "kegg_id")
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Call SaveMainXrefID(txtDrugBankID, "drugbank_id")
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Call SaveMainXrefID(txtBioCYCID, "biocyc")
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        Call SaveMainXrefID(txtMeshID, "mesh_id")
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        Call SaveMainXrefID(txtWikipediaID, "wikipedia")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Clipboard.SetText(TextBox8.Text)
    End Sub

    Private Async Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        Dim zh_name = Trim(TextBox9.Text)

        If zh_name.StringEmpty(, True) Then
            Warning($"Clear the chinese name of metabolite '{mol.name}'")
        End If

        Await MyApplication.biocad_registry.metabolites.async.where(field("id") = mol.id).save(field("name_zh") = zh_name)
    End Sub

    Private Async Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        Dim symbol As String = TextBox2.Text.makeSymbol
        ' check symbol 
        ' find duplicated
        Dim check1 = Await MyApplication.biocad_registry.registry_resolver _
            .async _
            .where(field("type") = MyApplication.biocad_registry.biocad_vocabulary.metabolite_type,
                   field("register_name") = symbol) _
            .find(Of registry_resolver)
        ' find myself
        Dim check2 = Await MyApplication.biocad_registry.registry_resolver _
            .async _
            .where(field("type") = MyApplication.biocad_registry.biocad_vocabulary.metabolite_type,
                   field("symbol_id") = mol.id) _
            .find(Of registry_resolver)

        If check1 IsNot Nothing AndAlso check1.symbol_id <> mol.id Then
            Dim opt = MessageBox.Show($"Current symbol '{symbol}' has already been register by other metabolite, going to replace the registry symbol reference?" & vbCrLf & vbCrLf &
                                      "[Cancel] for view the conflict metabolite.",
                                      "Check Conflicts",
                                      MessageBoxButtons.YesNoCancel,
                                      MessageBoxIcon.Question)

            If opt = DialogResult.Cancel Then
                Call Workbench.OpenMoleculeEditor(check1.symbol_id, check1.register_name)
            ElseIf opt = DialogResult.No Then
                Return
            End If

            Await MyApplication.biocad_registry.registry_resolver _
                .async _
                .where(field("id") = check1.id) _
                .delete()
        End If
        If check2 IsNot Nothing Then
            Await MyApplication.biocad_registry.registry_resolver _
                .async _
                .where(field("id") = check2.id) _
                .save(field("register_name") = symbol)
        Else
            Await MyApplication.biocad_registry.registry_resolver _
                .async _
                .add(field("register_name") = symbol,
                     field("symbol_id") = mol.id,
                     field("type") = MyApplication.biocad_registry.biocad_vocabulary.metabolite_type)
        End If

        TextBox8.Text = symbol

        CommonRuntime.StatusMessage($"Make register of the symbol '{symbol}' to {mol.id} - {mol.name} success!")
    End Sub
End Class

Public Class reaction_graphdata

    <DatabaseField> Public Property id As UInteger
    <DatabaseField> Public Property name As String
    <DatabaseField> Public Property exact_mass As Double
    <DatabaseField> Public Property formula As String
    <DatabaseField> Public Property db_xref As String
    <DatabaseField> Public Property role As String
    <DatabaseField> Public Property symbol_name As String

End Class