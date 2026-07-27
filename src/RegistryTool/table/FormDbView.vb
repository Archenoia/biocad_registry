Imports System.Reflection
Imports Galaxy.Data
Imports Galaxy.Workbench
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.MIME.text.markdown
Imports Ollama
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports RegistryTool.My

Public Class FormDbView

    Public Delegate Function SourceProvider(Of T)() As T()

    Dim _sourceProvider As Func(Of DataTable)
    Dim _sourceFilter As Func(Of DataTable)
    Dim _binding As New BindingSource
    Dim _filter As String = Nothing
    Dim _view As Action(Of DataGridViewRow)
    Dim _abstractNotePrompt As Func(Of DataGridViewRow, String)
    Dim _translatePrompt As Func(Of DataGridViewRow, String)
    Dim _translateField As String
    Dim _table As Model

    Public Sub SetViewer(view As Action(Of DataGridViewRow))
        _view = view
    End Sub

    Public Sub SetLLMsPrompt(prompt As Func(Of DataGridViewRow, String))
        _abstractNotePrompt = prompt
    End Sub

    Public Sub SetTranslatePrompt(prompt As Func(Of DataGridViewRow, String), fieldZh As String)
        _translatePrompt = prompt
        _translateField = fieldZh
    End Sub

    Public Sub SetTable(table As Model)
        _table = table
    End Sub

    Public Sub DisableFilter()
        ToolStripComboBox1.Visible = False
        ToolStripLabel1.Visible = False
    End Sub

    Public Sub SetFilter(name As String, fieldname As String)
        ToolStripLabel1.Text = $"Filter [{name}]"
        _filter = fieldname
    End Sub

    Public Sub LoadTableView(Of T)(source As SourceProvider(Of T))
        Dim schema As PropertyInfo() = DataFramework _
            .Schema(Of T)(PropertyAccess.Readable,
                          nonIndex:=True,
                          primitive:=True).Values _
            .ToArray

        _sourceProvider = loadTableFunc(source, schema)
        _sourceFilter = loadTableFilter(source, schema)

        Call LoadTable()
    End Sub

    Private Sub LoadTable()
        If ToolStripComboBox1.SelectedIndex <= -1 Then
            If _sourceProvider Is Nothing Then
                Return
            End If

            ToolStripComboBox1.Items.Clear()
            _binding.DataSource = _sourceProvider()
        Else
            If _sourceFilter Is Nothing Then
                Return
            End If

            _binding.DataSource = _sourceFilter()
        End If

        DataGridView1.DataSource = _binding
    End Sub

    Private Function loadTableFilter(Of T)(source As SourceProvider(Of T), schema As PropertyInfo()) As Func(Of DataTable)
        Return Function()
                   Dim term As String = ToolStripComboBox1.SelectedItem.ToString
                   Dim dt As New DataTable
                   Dim filter As PropertyInfo = Nothing

                   For Each field As PropertyInfo In schema
                       Call dt.Columns.Add(field.Name, field.PropertyType)

                       If _filter IsNot Nothing AndAlso _filter = field.Name Then
                           filter = field
                       End If
                   Next

                   For Each row As T In source()
                       Dim rowData As Object() = schema _
                           .Select(Function(col) col.GetValue(row)) _
                           .ToArray

                       If term = CStr(filter.GetValue(row)) Then
                           Call dt.Rows.Add(rowData)
                       End If
                   Next

                   Return dt
               End Function
    End Function

    Private Function loadTableFunc(Of T)(source As SourceProvider(Of T), schema As PropertyInfo()) As Func(Of DataTable)
        Return Function()
                   Dim dt As New DataTable
                   Dim catList As New List(Of String)
                   Dim filter As PropertyInfo = Nothing

                   For Each field As PropertyInfo In schema
                       Call dt.Columns.Add(field.Name, field.PropertyType)

                       If _filter IsNot Nothing AndAlso _filter = field.Name Then
                           filter = field
                       End If
                   Next

                   For Each row As T In source()
                       Dim rowData As Object() = schema _
                           .Select(Function(col) col.GetValue(row)) _
                           .ToArray

                       Call dt.Rows.Add(rowData)

                       If Not filter Is Nothing Then
                           Call catList.Add(CStr(filter.GetValue(row)))
                       End If
                   Next

                   For Each item As String In catList.Distinct
                       Call ToolStripComboBox1.Items.Add(item)
                   Next

                   Return dt
               End Function
    End Function

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Call LoadTable()
    End Sub

    Private Sub ToolStripComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox1.SelectedIndexChanged
        If ToolStripComboBox1.SelectedIndex > -1 Then
            Call LoadTable()
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub ViewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewToolStripMenuItem.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        If Not _view Is Nothing Then
            Call _view(DataGridView1.SelectedRows(0))
        End If
    End Sub

    Private Sub FormDbView_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call ApplyVsTheme(ToolStrip1, ContextMenuStrip1)
    End Sub

    Private Sub UpdateNoteTextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateNoteTextToolStripMenuItem.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If
        If _abstractNotePrompt IsNot Nothing AndAlso _table IsNot Nothing Then
            Dim target As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim prompt_text As String = _abstractNotePrompt(target)
            Dim id As UInteger = CUInt(target.Cells(0).Value)
            Dim note As String = TaskProgress.LoadData(Of String)(
                Function(p As ITaskProgress)
                    Dim msg As LLMsResponse = MyApplication.ollama.Chat(prompt_text).GetAwaiter.GetResult
                    Dim markdown As New MarkdownRender

                    If Not msg Is Nothing Then
                        Try
                            Return markdown.Transform(msg.output)
                        Catch ex As Exception
                            Return msg.output
                        End Try
                    Else
                        Return ""
                    End If
                End Function, title:="LLMs talk: Generates the LLM note...", info:=prompt_text)

            Call _table.where(field("id") = id).save(field("note") = note)
        End If
    End Sub

    Private Sub TranslateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TranslateToolStripMenuItem.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If
        If _translatePrompt IsNot Nothing AndAlso _table IsNot Nothing Then
            Dim target As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim prompt_text As String = _translatePrompt(target)
            Dim id As UInteger = CUInt(target.Cells(0).Value)
            Dim name_translate As String = TaskProgress.LoadData(Of String)(
                Function(p As ITaskProgress)
                    Call p.SetInfo(prompt_text)

                    Dim msg As LLMsResponse = MyApplication.ollama.Chat(prompt_text).GetAwaiter.GetResult
                    Dim zh_name As String = TranslatedName.DecodeLLMTranslateOutput(msg?.output)

                    Return zh_name
                End Function, title:="LLMs talk: Translate with LLMs...", info:=prompt_text)

            If Not name_translate.StringEmpty(, True) Then
                Call _table.where(field("id") = id).save(field(_translateField) = name_translate)
            End If
        End If
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        Call DataGridView1.SaveDataGrid("Export Sql Table")
    End Sub
End Class