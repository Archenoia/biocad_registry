Imports Galaxy.Data.TableSheet
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel
Imports RegistryTool.My
Imports std = System.Math

Public Class FormMetabolicSymbols

    Dim grid As GridLoaderHandler

    Private Async Sub FormMetabolicSymbols_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.ApplyVsTheme(AdvancedDataGridViewSearchToolBar1)
        Me.grid = New GridLoaderHandler(AdvancedDataGridView1, AdvancedDataGridViewSearchToolBar1)

        Await LoadTable()
    End Sub

    Private Async Function LoadTable() As Task
        Dim species_id As String = "SELECT DISTINCT species_id FROM cad_registry.metabolic_network WHERE species_id > 0"
        Dim table As metabolites() = Await MyApplication.biocad_registry.metabolites _
            .async _
            .left_join("registry_resolver") _
            .on(field("`registry_resolver`.symbol_id") = field("`metabolites`.id") And field("type") = MyApplication.biocad_registry.biocad_vocabulary.metabolite_type) _
            .where(field("metabolites.id").in(species_id)) _
            .order_by("name") _
            .select(Of metabolites)("metabolites.id",
    "name",
    "name_zh",
    "register_name AS note",
    "formula",
    "exact_mass",
    "cas_id",
    "hmdb_id",
    "kegg_id")

        Me.grid.ClearData()
        Me.grid.LoadTable(
            Sub(tbl)
                Call tbl.Columns.Add("metabolite_id", GetType(UInteger))
                Call tbl.Columns.Add("name", GetType(String))
                Call tbl.Columns.Add("name_zh", GetType(String))
                Call tbl.Columns.Add("registry_symbol", GetType(String))
                Call tbl.Columns.Add("formula", GetType(String))
                Call tbl.Columns.Add("exact_mass", GetType(Double))
                Call tbl.Columns.Add("cas_id", GetType(String))
                Call tbl.Columns.Add("hmdb_id", GetType(String))
                Call tbl.Columns.Add("kegg_id", GetType(String))

                For Each meta In table
                    tbl.Rows.Add(meta.id,
                        meta.name,
                        meta.name_zh,
                        meta.note,
                        meta.formula,
                        std.Round(meta.exact_mass, 4),
                        meta.cas_id,
                        meta.hmdb_id,
                        meta.kegg_id)
                Next
            End Sub)
    End Function

    Private Async Sub ViewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewToolStripMenuItem.Click
        If AdvancedDataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim row = AdvancedDataGridView1.SelectedRows(0)
        Dim id = CStr(row.Cells(0).Value)
        Dim name = CStr(row.Cells(1).Value)

        Await Workbench.OpenMoleculeEditor(id, name)
    End Sub
End Class