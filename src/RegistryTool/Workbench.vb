Imports Galaxy.Workbench
Imports Microsoft.VisualStudio.WinForms.Docking
Imports registry_data
Imports RegistryTool.My

Module Workbench

    Public ReadOnly Property cad_registry As biocad_registry
        Get
            Return MyApplication.biocad_registry
        End Get
    End Property

    Public Async Function OpenMoleculeEditor(id As String, name As String) As Task
        Dim edit As New FormMoleculeEditor

        If id.IsPattern("\d+") OrElse id.IsPattern("BioCAD\d+") Then
            If Await edit.LoadModel(id) Then
                edit.Text = $"BioCAD{id.PadLeft(11, "0"c)} - {name}"
                edit.Show(CommonRuntime.AppHost.GetDockPanel, DockState.Document)
            End If
        Else
            Call MessageBox.Show($"the requested id '{id}' is not a valid registry number!",
                                 "Invalid input",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Warning)
        End If
    End Function

    Public Function InputTextData() As String()
        Dim input As New FormTextEditor
        Call input.ShowDialog()
        Return input.TextLines
    End Function
End Module
