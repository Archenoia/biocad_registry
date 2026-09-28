Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports SMRUCC.genomics.Metagenomics
Imports SMRUCC.genomics.SequenceModel.FASTA

Public Module FastaData

    <Extension>
    Public Iterator Function ExportStrainSequence(registry As biocad_registry, Optional page_size As Integer = 5000) As IEnumerable(Of FastaSeq)
        For i As Integer = 1 To Integer.MaxValue
            Dim offset As ULong = (i - 1) * page_size
            Dim page = registry.protein_data _
                .left_join("ncbi_taxonomy") _
                .on(field("`ncbi_taxonomy`.id") = field("ncbi_taxid")) _
                .where(field("ncbi_taxid") > 0) _
                .limit(offset, page_size) _
                .select(Of StrainSequence)("source_id", "ncbi_taxonomy.name", "ncbi_taxid", "sequence")

            For Each line As StrainSequence In page
                If line.name.StringEmpty Then
                    Continue For
                End If

                Yield New FastaSeq({line.ncbi_taxid & "." & line.source_id, line.name.ExtractSpeciesName}, line.sequence)
            Next

            Call $"export page {i}".debug
        Next
    End Function

    Public Class StrainSequence

        <DatabaseField> Public Property source_id As String
        <DatabaseField> Public Property name As String
        <DatabaseField> Public Property ncbi_taxid As UInteger
        <DatabaseField> Public Property sequence As String

    End Class
End Module
