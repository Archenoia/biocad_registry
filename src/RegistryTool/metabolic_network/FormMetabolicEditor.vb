Imports Galaxy.Workbench
Imports Galaxy.Workbench.CommonDialogs
Imports Microsoft.VisualBasic.Linq
Imports Ollama
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Scripting
Imports registry_data.biocad_registryModel
Imports RegistryTool.My

Public Class FormMetabolicEditor

    Public ReadOnly Property Page As Integer
        Get
            Return CInt(Val(ToolStripTextBox1.Text))
        End Get
    End Property

    Private Async Sub FormMetabolicEditor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call ApplyVsTheme(ToolStrip1)

        Await GotoPageData()
        Await LoadDBSourceList()
    End Sub

    Private Async Function LoadDBSourceList() As Task
        Dim source_ids As UInteger() = Await MyApplication.biocad_registry.reaction.async.distinct.project(Of UInteger)("db_source")
        Dim dbnames = Await MyApplication.biocad_registry.vocabulary.async.where(field("id").in(source_ids)).select(Of vocabulary)

        Call ToolStripComboBox1.Items.Clear()
        Call ToolStripComboBox1.Items.Add("*")

        For Each dbname As vocabulary In dbnames
            Call ToolStripComboBox1.Items.Add(New TopicTerm() With {.id = dbname.id, .term = dbname.term})
        Next
    End Function

    Private Async Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        If Page = 1 Then
            Return
        End If

        ToolStripTextBox1.Text = Page - 1

        Await GotoPageData()
    End Sub

    Private Async Sub GotoPage() Handles ToolStripButton2.Click
        Await GotoPageData()
    End Sub

    Dim q As FieldAssert() = Nothing

    Private Async Function GotoPageData() As Task
        Call DisplayPageData(Await ReactionModelView.QueryPage(Me.Page, page_size:=500, q:=q))
    End Function

    Private Sub DisplayPageData(reactions As ReactionModelView())
        Dim offset As Integer

        Call DataGridView1.Rows.Clear()

        For Each rxn As ReactionModelView In reactions
            offset = DataGridView1.Rows.Add(rxn.db_name, rxn.name, rxn.ec_number, rxn.equation, rxn.note)
            DataGridView1.Rows(offset).Tag = rxn
            DataGridView1.Rows(offset).HeaderCell.Value = rxn.db_xref
        Next

        Call DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Async Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        ToolStripTextBox1.Text = Page + 1
        Await GotoPageData()
    End Sub

    Private Async Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim row = DataGridView1.SelectedRows(0)

        If row.Tag Is Nothing Then
            Return
        Else
            With DirectCast(row.Tag, ReactionModelView)
                Await ShowReaction(DirectCast(row.Tag, ReactionModelView).id)

                Call ShowReactionEdit(.name, .ec_number, .note)
                Call ShowECEditList(Await Workbench.cad_registry.db_xrefs _
                    .async _
                    .where(field("type") = Workbench.cad_registry.biocad_vocabulary.reaction_type,
                           field("db_name") = Workbench.cad_registry.biocad_vocabulary.db_ECNumber,
                           field("obj_id") = .id) _
                    .select(Of db_xrefs))
            End With
        End If
    End Sub

    Dim reaction_id As UInteger

    ''' <summary>
    ''' show reaction data and then set <see cref="reaction_id"/> for the current reaction data in editing
    ''' </summary>
    ''' <param name="rxn_id"></param>
    ''' <returns></returns>
    Private Async Function ShowReaction(rxn_id As UInteger) As Task
        Dim metab_type As UInteger = MyApplication.biocad_registry.biocad_vocabulary.metabolite_type
        Dim graph = Await MyApplication.biocad_registry.metabolic_network _
            .async _
            .left_join("metabolites") _
            .on(field("species_id") = field("metabolites.id")) _
            .left_join("vocabulary") _
            .on(field("vocabulary.id") = field("role")) _
            .left_join("registry_resolver") _
            .on(field("`registry_resolver`.symbol_id") = field("species_id")) _
            .where(field("reaction_id") = rxn_id) _
            .select(Of reaction_graphdata)("metabolites.*", "`metabolic_network`.symbol_id as db_xref", "term AS role", "register_name as symbol_name")

        Call DataGridView2.Rows.Clear()

        For Each compound In graph
            Call DataGridView2.Rows.Add(
                compound.id,
                compound.db_xref,
                compound.name,
                compound.formula,
                compound.exact_mass,
                compound.role,
                compound.symbol_name
            )
        Next

        reaction_id = rxn_id
    End Function

    Private Sub ShowECEditList(ec_numbers As IEnumerable(Of db_xrefs))
        Call ListBox1.Items.Clear()

        For Each id As db_xrefs In ec_numbers.SafeQuery
            Call ListBox1.Items.Add(id.db_xref)
        Next
    End Sub

    Private Async Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim text As New FormTextEditor
        Dim ec_list As New List(Of String)

        If reaction_id = 0 Then
            Return
        End If

        For i As Integer = 0 To ListBox1.Items.Count - 1
            Call ec_list.Add(CStr(ListBox1.Items(i)))
        Next

        Call text.SetText(ec_list)
        Call text.ShowDialog()

        Dim editIndex = text.TextLines.Indexing
        Dim update As CommitTransaction = Workbench.cad_registry.db_xrefs.open_transaction

        ' ec_id is edited data
        For Each ec_id As String In text.TextLines
            If ec_id.StringEmpty(, True) Then
                Continue For
            End If

            If ec_list.IndexOf(ec_id) < 0 Then
                ' is new added by user, so we need to add it to the database
                Call update.add(Workbench.cad_registry.db_xrefs _
                    .add_sql(
                        field("obj_id") = reaction_id,
                        field("type") = Workbench.cad_registry.biocad_vocabulary.reaction_type,
                        field("db_name") = Workbench.cad_registry.biocad_vocabulary.db_ECNumber,
                        field("db_xref") = ec_id,
                        field("db_source") = Workbench.cad_registry.biocad_vocabulary.db_ManualAudit
                    ))
            End If
        Next

        For Each ec_id As String In ec_list
            ' is missing from the original list, which means user delete from the text editor, so we need to delete it from the database
            If Not (ec_id Like editIndex) Then
                Call update.add(Workbench.cad_registry.db_xrefs _
                    .where(
                        field("obj_id") = reaction_id,
                        field("type") = Workbench.cad_registry.biocad_vocabulary.reaction_type,
                        field("db_name") = Workbench.cad_registry.biocad_vocabulary.db_ECNumber,
                        field("db_xref") = ec_id) _
                    .delete_sql)
            End If
        Next

        Await update.commitTask
    End Sub

    Private Sub ShowReactionEdit(name As String, ec$, note$)
        TextBox1.Text = name
        TextBox2.Text = ec
        TextBox3.Text = note
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        If DataGridView2.SelectedRows.Count = 0 Then
            Return
        End If

        Dim meta = DataGridView2.SelectedRows(0)
        Dim registry_id As String = CStr(meta.Cells(0).Value)
        Dim name As String = CStr(meta.Cells(1).Value)

        If registry_id <> "" Then
            Call Workbench.OpenMoleculeEditor(registry_id, name)
        End If
    End Sub

    Private Async Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        Dim id As String = Strings.Trim(ToolStripTextBox2.Text)

        q = {(field("`reaction`.id") = id) Or (field("db_xref") = id) Or (field("ec_number") = id)}
        ToolStripTextBox1.Text = 1

        Await GotoPageData()
    End Sub

    Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
        Call InputDialog.Input(Of FormBuildReaction)()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim prompt As String = $"请为我使用英文介绍 '{TextBox1.Text}' 这个代谢反应，请直接返回无格式标记的纯文本内容，以方便我自动化的放入到报告文本中。"
        Dim msg As OllamaResponse = TaskProgress.LoadData(
            streamLoad:=Function(println As Action(Of String))
                            Return MyApplication.ollama.Chat(prompt).GetAwaiter.GetResult
                        End Function)

        If msg IsNot Nothing Then
            TextBox3.Text = msg.output
        End If
    End Sub

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If reaction_id > 0 Then
            Await MyApplication.biocad_registry.reaction _
                .async _
                .where(field("id") = reaction_id) _
                .save(field("name") = TextBox1.Text)

            CommonRuntime.StatusMessage("reaction name data update success!")
        End If
    End Sub

    Private Async Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If reaction_id > 0 Then
            Await MyApplication.biocad_registry.reaction _
                .async _
                .where(field("id") = reaction_id) _
                .save(field("ec_number") = TextBox2.Text)

            StatusMessage("reaction enzyme number data update success!")
        End If
    End Sub

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If reaction_id > 0 Then
            Await MyApplication.biocad_registry.reaction _
                .async _
                .where(field("id") = reaction_id) _
                .save(field("note") = TextBox3.Text)

            CommonRuntime.StatusMessage("reaction note data update success!")
        End If
    End Sub

    Private Async Sub ToolStripComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox1.SelectedIndexChanged
        If ToolStripComboBox1.SelectedIndex < 0 Then
            Return
        ElseIf ToolStripComboBox1.SelectedIndex = 0 Then
            q = {}
        Else
            q = {field("db_source") = DirectCast(ToolStripComboBox1.SelectedItem, TopicTerm).id}
        End If

        ToolStripTextBox1.Text = 1

        Await GotoPageData()
    End Sub

    Private Sub CopySymbolNameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopySymbolNameToolStripMenuItem.Click
        If DataGridView2.SelectedRows.Count = 0 Then
            Return
        End If

        Dim meta = DataGridView2.SelectedRows(0)
        Dim name As String = CStr(meta.Cells(6).Value)

        Call Clipboard.SetText(name)
        Call CommonRuntime.StatusMessage($"the metabolite symbol name '{name}' has been copied!")
    End Sub

    Private Async Sub ToolStripButton6_Click(sender As Object, e As EventArgs) Handles ToolStripButton6.Click
        Dim text As String = Strings.Trim(InputBox("Input the substrate name text to search:"))

        If text.StringEmpty(, True) Then
            Return
        Else
            text = text.FullTextEscape.Replace("_", " ")
        End If

        Call CommonRuntime.StatusMessage($"make metabolite searchs for '{text}'.")

        Dim substrate_id = "SELECT DISTINCT species_id FROM cad_registry.metabolic_network"
        Dim metabolites As metabolites() = Await MyApplication.biocad_registry.metabolites _
            .async _
            .where(field("id").in(substrate_id),
                   match("name", "note").against(text, False)) _
            .order_by("name") _
            .select(Of metabolites)("*")

        Call CommonRuntime.GetOutputWindow.AddLog("load metabolic symbols", MyApplication.biocad_registry.metabolites.GetLastMySql)
        Call FormMain.ShowMetabolitesTable(metabolites, text)
    End Sub

    Private Sub ToolStripButton7_Click(sender As Object, e As EventArgs) Handles ToolStripButton7.Click
        Call CommonRuntime.ShowDocument(Of FormMetabolicSymbols)()
    End Sub

    Private Sub CopyEquationStringToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyEquationStringToolStripMenuItem.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim row = DataGridView1.SelectedRows(0)

        If row Is Nothing Then
            Return
        End If

        Dim eqcell = row.Cells(3)

        If eqcell Is Nothing Then
            Return
        End If

        Dim eqstr As String = CStr(eqcell.Value)

        Call Clipboard.SetText(eqstr)
    End Sub
End Class