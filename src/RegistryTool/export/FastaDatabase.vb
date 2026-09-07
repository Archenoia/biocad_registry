Imports System.IO
Imports Galaxy.Workbench
Imports registry_data
Imports RegistryTool.My
Imports SMRUCC.genomics

Module FastaDatabase

    Public Sub ExportMembraneTransporter()
        Using file As New SaveFileDialog With {.Filter = "Sequence Database(*.fasta)|*.fasta"}
            If file.ShowDialog = DialogResult.OK Then
                Dim s = file.FileName.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)
                Dim str As New SequenceModel.FASTA.StreamWriter(s)

                Call ProgressSpinner.DoLoading(
                    loading:=Sub()
                                 Call str.Add(MyApplication.biocad_registry.ExportCellularLocation)
                             End Sub)
                Call str.Dispose()
                Call s.Dispose()

                Call MessageBox.Show("Export subcellular location annotation database to local annotation repository file success!",
                                     "Task Finish",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Public Sub ExportEnzymeDatabase()
        Using file As New SaveFileDialog With {.Filter = "Enzyme Protein Sequence(*.fasta)|*.fasta"}
            If file.ShowDialog = DialogResult.OK Then
                Dim s = file.FileName.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)
                Dim str As New SequenceModel.FASTA.StreamWriter(s)

                MyApplication.Loading(
                    Function(println)
                        str.Add(MyApplication.biocad_registry.ExportEnzyme)
                        Return True
                    End Function)
                str.Dispose()
                s.Dispose()
                MessageBox.Show("Export enzyme database to local annotation repository file success!",
                                     "Task Finish",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    ''' <summary>
    ''' make export all protein sequence data inside the registry database
    ''' </summary>
    Public Sub ExportProteinDatabase()
        Using file As New SaveFileDialog With {.Filter = "Protein Sequence Database(*.fasta)|*.fasta"}
            If file.ShowDialog = DialogResult.OK Then
                Dim s = file.FileName.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)
                Dim str As New SequenceModel.FASTA.StreamWriter(s)

                MyApplication.Loading(
                    Function(println)
                        str.Add(MyApplication.biocad_registry.ExportProteinDb)
                        Return True
                    End Function)
                str.Dispose()
                s.Dispose()
                MessageBox.Show("Export protein sequence database to local annotation repository file success!",
                                     "Task Finish",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Public Sub ExportKODatabase()
        Using file As New SaveFileDialog With {.Filter = "Protein Sequence Database(*.fasta)|*.fasta"}
            If file.ShowDialog = DialogResult.OK Then
                Dim s = file.FileName.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)
                Dim str As New SequenceModel.FASTA.StreamWriter(s)

                MyApplication.Loading(
                    Function(println)
                        str.Add(MyApplication.biocad_registry.ExportKOProteinDb)
                        Return True
                    End Function)
                str.Dispose()
                s.Dispose()
                MessageBox.Show("Export protein sequence database to local annotation repository file success!",
                                     "Task Finish",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information)
            End If
        End Using
    End Sub
End Module
