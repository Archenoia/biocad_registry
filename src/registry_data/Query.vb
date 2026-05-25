Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel

Public Module Query

    ''' <summary>
    ''' find the metabolite by its external reference symbols
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="name">the metabolite name</param>
    ''' <param name="xref_ids">the external referene id of this metabolite</param>
    ''' <returns></returns>
    <Extension>
    Public Function FindSymbol(registry As biocad_registry, name As String, xref_ids As IReadOnlyCollection(Of String)) As metabolites
        Dim metab_type As UInteger = registry.biocad_vocabulary.metabolite_type
        Dim idset1 As UInteger() = Nothing
        Dim idset2 As UInteger() = Nothing
        Dim idset3 As UInteger() = Nothing
        ' hashcode for make fast extract name matches
        Dim hashcode As String = Strings.Trim(name).ToLower.MD5

        If xref_ids.Count > 0 Then
            idset1 = registry.db_xrefs _
                .where(field("type") = metab_type,
                       field("db_xref").in(xref_ids)
                ) _
                .project(Of UInteger)("obj_id")
        End If

        idset2 = registry.synonym _
            .where(field("type") = metab_type,
                   field("hashcode") = hashcode) _
            .project(Of UInteger)("obj_id")
        idset3 = registry.metabolites _
            .where(field("hashcode") = hashcode) _
            .project(Of UInteger)("id")

        If idset1.IsNullOrEmpty AndAlso
            idset2.IsNullOrEmpty AndAlso
            idset3.IsNullOrEmpty Then

            Return Nothing
        End If

        Dim top_id As IGrouping(Of UInteger, UInteger) = {idset1, idset2, idset3} _
            .IteratesALL _
            .GroupBy(Function(int) int) _
            .OrderByDescending(Function(g) g.Count) _
            .First
        Dim meta As metabolites = registry.metabolites _
            .where(field("id") = top_id.Key) _
            .find(Of metabolites)

        If Not meta Is Nothing Then
            Do While meta IsNot Nothing AndAlso meta.main_id > 0
                meta = registry.metabolites.where(field("id") = meta.main_id).find(Of metabolites)
            Loop
        End If

        Return meta
    End Function

End Module
