Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel

Public Module Query

    ''' <summary>
    ''' find the metabolite by its external reference symbols
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="name">the metabolite name</param>
    ''' <param name="xref_ids"></param>
    ''' <returns></returns>
    <Extension>
    Public Function FindSymbol(registry As biocad_registry, name As String, xref_ids As IReadOnlyCollection(Of String)) As metabolites
        Dim metab_type As UInteger = registry.biocad_vocabulary.metabolite_type
        Dim idset As UInteger() = Nothing
        Dim hashcode As String = Strings.Trim(name).ToLower.MD5

        If xref_ids.Count > 0 Then
            idset = registry.db_xrefs _
                .where(field("type") = metab_type,
                       field("db_xref").in(xref_ids)
                ) _
                .project(Of UInteger)("obj_id")
        End If

        If xref_ids.Count = 0 OrElse idset.IsNullOrEmpty Then
            idset = registry.synonym _
                .where(field("type") = metab_type,
                       field("hashcode") = hashcode) _
                .project(Of UInteger)("obj_id")
        End If

        If idset.IsNullOrEmpty Then
            idset = registry.metabolites.where(field("hashcode") = hashcode).project(Of UInteger)("id")
        End If
        If idset.IsNullOrEmpty Then
            Return Nothing
        End If

        Dim top_id As UInteger = idset _
            .GroupBy(Function(int) int) _
            .OrderByDescending(Function(g) g.Count) _
            .First.Key
        Dim meta As metabolites = registry.metabolites.where(field("id") = top_id).find(Of metabolites)

        Do While meta.main_id > 0
            meta = registry.metabolites.where(field("id") = meta.main_id).find(Of metabolites)
        Loop

        Return meta
    End Function

End Module
