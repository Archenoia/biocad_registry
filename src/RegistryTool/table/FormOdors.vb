Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports RegistryTool.My
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class FormOdors

    Dim page As Integer = 1
    Dim page_size As Integer = 3000

    Private Async Sub FormOdors_Load(sender As Object, e As EventArgs) Handles Me.Load
        Await loadPage()

        Call ApplyVsTheme(StatusStrip1, ToolStrip1, ContextMenuStrip1, ContextMenuStrip2)
    End Sub

    Private Async Function loadPage() As Task
        Dim q = Await OdorData.LoadPage(page, page_size)

        DataGridView1.Visible = True
        DataGridView2.Visible = False
        DataGridView1.Rows.Clear()
        DataGridView2.Dock = DockStyle.None
        DataGridView1.Dock = DockStyle.Fill

        For Each item In q
            Dim i = DataGridView1.Rows.Add(item.name, item.formula, item.category, item.odor, item.text)
            Dim row = DataGridView1.Rows(i)

            row.Tag = item
        Next

        DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Function

    Private Function getSelected() As OdorData
        Dim selRows = DataGridView1.SelectedRows

        If selRows.Count = 0 Then
            Return Nothing
        Else
            Return selRows(0).Tag
        End If
    End Function

    Private Async Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        Dim odor As OdorData = getSelected()

        If odor Is Nothing Then
            Return
        End If

        If MessageBox.Show($"Going to delete current odor data: {odor.odor} ({odor.name})?", "Delete Confirmed", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = DialogResult.OK Then

            '  MyApplication.biocad_registry.odor.where(field("id") = odor.id).limit(1).delete()
            Await loadPage()
        End If
    End Sub

    Private Async Sub DeleteSimilarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteSimilarToolStripMenuItem.Click
        Dim odor As OdorData = getSelected()

        If odor Is Nothing Then
            Return
        End If

        If MessageBox.Show($"Going to delete all odor data: {odor.odor}?", "Delete Confirmed", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = DialogResult.OK Then

            '  MyApplication.biocad_registry.odor.where(field("odor") = odor.odor).delete()
            Await loadPage()
        End If
    End Sub

    Private Async Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Await loadPage()
    End Sub

    Private Async Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        page -= 1

        If page < 1 Then
            page = 1
        Else
            Await loadPage()
        End If
    End Sub

    Private Async Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        page += 1
        Await loadPage()
    End Sub

    Private Async Sub ToolStripButton4_Click() Handles ToolStripButton4.Click
        If ToolStripButton4.Checked Then
            DataGridView2.Visible = True
            DataGridView1.Visible = False
            DataGridView1.Dock = DockStyle.None
            DataGridView2.Dock = DockStyle.Fill
            DataGridView2.Visible = True

            Dim terms = Await Task.Run(Function() OdorTerm.LoadTerms)

            Call DataGridView2.Rows.Clear()

            For Each term In terms
                Dim i = DataGridView2.Rows.Add(term.category, term.term, term.count)
                Dim row = DataGridView2.Rows(i)

                row.Tag = term
            Next
        Else
            DataGridView1.Visible = True
            DataGridView2.Visible = False
            DataGridView2.Dock = DockStyle.None
            DataGridView1.Dock = DockStyle.Fill
        End If
    End Sub

    Private Async Sub DeleteTermRecordsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteTermRecordsToolStripMenuItem.Click
        Dim selRows = DataGridView2.SelectedRows

        If selRows.Count = 0 Then
            Return
        Else
            Dim refresh As Boolean = False

            For i As Integer = 0 To selRows.Count - 1
                Dim term As OdorTerm = selRows(i).Tag

                If MessageBox.Show($"Delete all {term.count} odor information of term: {term.term}({term.category})",
                               "Check Operation",
                               MessageBoxButtons.OKCancel,
                               MessageBoxIcon.Information) = DialogResult.OK Then

                    Await Task.Run(
                        Sub()
                            ' Call MyApplication.biocad_registry.odor.where(field("odor") = term.term).delete()
                        End Sub)

                    refresh = True
                Else
                    Exit For
                End If
            Next

            If refresh Then
                ToolStripButton4_Click()
            End If
        End If
    End Sub

    Private Async Sub DeleteRecordsSkipConfirmToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteRecordsSkipConfirmToolStripMenuItem.Click
        Dim selRows = DataGridView2.SelectedRows
        Dim list As New List(Of String)

        For i As Integer = 0 To selRows.Count - 1
            Dim term As OdorTerm = selRows(i).Tag
            list.Add($"{term.term} ({term.category})")
        Next

        If selRows.Count = 0 Then
            Return
        ElseIf MessageBox.Show($"Delete all {list.count} odor information of term:" & vbCrLf & vbCrLf & list.JoinBy(vbCrLf),
                               "Check Operation",
                               MessageBoxButtons.OKCancel,
                               MessageBoxIcon.Information) = DialogResult.OK Then

            For i As Integer = 0 To selRows.Count - 1
                Dim term As OdorTerm = selRows(i).Tag

                Await Task.Run(
                        Sub()
                            ' Call MyApplication.biocad_registry.odor.where(field("odor") = term.term).delete()
                        End Sub)
            Next

            ToolStripButton4_Click()
        End If
    End Sub

    Private Async Sub DataGridView1_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        Dim meta As OdorData = getSelected()

        If meta Is Nothing Then
            Return
        End If

        Await Workbench.OpenMoleculeEditor(meta.molecule_id, meta.name)
    End Sub

    Private Sub ViewMoleculeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewMoleculeToolStripMenuItem.Click
        Call DataGridView1_CellContentDoubleClick(Nothing, Nothing)
    End Sub
End Class

Public Class OdorTerm

    <DatabaseField> Public Property category As String
    <DatabaseField> Public Property term As String
    <DatabaseField> Public Property count As Long

    Public Shared Function LoadTerms() As OdorTerm()
        'Return MyApplication.biocad_registry.odor _
        '    .left_join("vocabulary") _
        '    .on(field("`vocabulary`.id") = field("`odor`.category")) _
        '    .group_by("term", "odor") _
        '    .order_by("count") _
        '    .select(Of OdorTerm)("term AS category", "odor AS term", "COUNT(*) AS count")
        Throw New NotImplementedException
    End Function

End Class

Public Class OdorData

    <DatabaseField> Public Property id As UInteger
    <DatabaseField> Public Property molecule_id As UInteger
    <DatabaseField> Public Property name As String
    <DatabaseField> Public Property formula As String
    <DatabaseField> Public Property category As String
    <DatabaseField> Public Property odor As String
    <DatabaseField> Public Property text As String

    Public Shared Async Function LoadPage(page As Integer, page_size As Integer) As Task(Of OdorData())
        '    Dim offset As Long = (page - 1) * page_size
        '    Dim q = Await Task.Run(Function() MyApplication.biocad_registry.odor _
        '        .left_join("molecule") _
        '        .on(field("`molecule`.id") = field("molecule_id")) _
        '        .left_join("vocabulary") _
        '        .on(field("`vocabulary`.id") = field("`odor`.category")) _
        '        .limit(offset, page_size) _
        '        .select(Of OdorData)("`odor`.id",
        '"molecule_id",
        '"name",
        '"formula",
        '"`vocabulary`.term AS category",
        '"odor",
        '"text"))

        '    Return q
        Throw New NotImplementedException
    End Function

End Class