Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports SMRUCC.genomics.SequenceModel.FASTA
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module EnzymeData

    <Extension>
    Public Iterator Function ExportEnzyme(registry As biocad_registry,
                                          Optional model As ProteinModel = Nothing,
                                          Optional page_size As Integer = 5000) As IEnumerable(Of FastaSeq)

        Dim terms = registry.biocad_vocabulary
        Dim ec_number As UInteger = terms.db_ECNumber
        Dim prot_fasta As UInteger = terms.protein_data

        model = If(model, New ProteinModel(registry))

        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim page_data As EnzymeSequenceView() = registry.protein_data _
                .left_join("db_xrefs") _
                .on((field("db_xrefs.obj_id") = field("protein_data.id")) And (field("type") = prot_fasta)) _
                .where(field("db_name") = ec_number,
                       field("sequence").char_length > 1) _
                .limit(offset, page_size) _
                .select(Of EnzymeSequenceView)("db_xref AS term", "sequence", "protein_data.id")

            If page_data.IsNullOrEmpty Then
                Exit For
            Else
                Call $"Export enzyme sequence data page {page}...".info

                For Each seq As EnzymeSequenceView In page_data
                    Yield New FastaSeq(seq.sequence, title:=seq.term & " " & seq.id & " " & model.GetProteinModelLabel(seq.id))
                Next
            End If
        Next
    End Function

    Private Class EnzymeSequenceView

        <DatabaseField> Public Property id As UInteger
        <DatabaseField> Public Property term As String
        <DatabaseField> Public Property sequence As String

    End Class

    <Extension>
    Public Iterator Function ExportCellularLocation(registry As biocad_registry, Optional page_size As Integer = 5000) As IEnumerable(Of FastaSeq)
        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim page_data As EnzymeSequenceView() = registry.subcellular_location _
                .left_join("protein_data") _
                .on(field("protein_data.id") = field("subcellular_location.protein_id")) _
                .left_join("compartment_location") _
                .on(field("compartment_location.id") = field("location_id")) _
                .where(field("sequence").char_length > 1) _
                .limit(offset, page_size) _
                .select(Of EnzymeSequenceView)("compartment_location.name AS term", "protein_data.id", "sequence")

            If page_data.IsNullOrEmpty Then
                Exit For
            Else
                Call $"Export subcellular location reference sequence data page {page}...".info

                For Each seq As EnzymeSequenceView In page_data
                    Yield New FastaSeq(seq.sequence, title:=seq.term.Replace(" ", "_") & " " & seq.id)
                Next
            End If
        Next
    End Function

    ''' <summary>
    ''' Export protein sequence data with clean id and no annotation,
    ''' which is suitable for sequence analysis and alignment, make cluster, ko alignment.
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="page_size"></param>
    ''' <returns></returns>
    <Extension>
    Public Iterator Function ExportProteinDb(registry As biocad_registry, Optional page_size As Integer = 5000) As IEnumerable(Of FastaSeq)
        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim pagedata = registry.protein_data.limit(offset, page_size).select(Of biocad_registryModel.protein_data)

            If pagedata.IsNullOrEmpty Then
                Exit For
            End If

            For Each seq As biocad_registryModel.protein_data In pagedata
                Yield New FastaSeq(New String() {seq.source_id, seq.id}, seq.sequence)
            Next
        Next
    End Function

    <Extension>
    Public Iterator Function ExportKOProteinDb(registry As biocad_registry, Optional page_size As Integer = 5000) As IEnumerable(Of FastaSeq)
        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim pagedata = registry.protein_data _
                .left_join("protein").on(field("`protein`.id") = field("protein_id")) _
                .where(field("protein_id") > 0) _
                .limit(offset, page_size) _
                .select(Of protein_seq)("`protein_data`.id", "`protein`.name", "`protein`.function", "db_xref", "sequence")

            If pagedata.IsNullOrEmpty Then
                Exit For
            End If

            For Each seq As protein_seq In pagedata
                Yield New FastaSeq(New String() {seq.id, seq.db_xref, seq.name, seq.function}, seq.sequence)
            Next
        Next
    End Function

    Public Class protein_seq

        <DatabaseField> Public Property id As UInteger
        <DatabaseField> Public Property name As String
        <DatabaseField> Public Property [function] As String
        <DatabaseField> Public Property db_xref As String
        <DatabaseField> Public Property sequence As String

    End Class
End Module
