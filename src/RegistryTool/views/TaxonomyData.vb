Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports RegistryTool.My

Public Class TaxonomyData

    <DatabaseField> Public Property id As UInteger
    <DatabaseField> Public Property name As String
    <DatabaseField> Public Property zh_name As String
    <DatabaseField> Public Property rank As String
    <DatabaseField> Public Property parent_id As UInteger
    <DatabaseField> Public Property parent_tax As String
    <DatabaseField> Public Property parent_zhname As String

    Public Shared Async Function Find(name As String) As Task(Of TaxonomyData())
        Return Await MyApplication.biocad_registry.ncbi_taxonomy _
            .async _
            .left_join("vocabulary").on(field("vocabulary.id") = field("ncbi_taxonomy.`rank`")) _
            .left_join("ncbi_taxonomy parent").on(field("parent.id") = field("ncbi_taxonomy.ancestor")) _
            .where(field("ncbi_taxonomy.name") = name) _
            .select(Of TaxonomyData)("ncbi_taxonomy.id",
    "ncbi_taxonomy.name",
    "ncbi_taxonomy.zh_name",
    "term AS `rank`",
    "parent.id AS parent_id",
    "parent.name AS parent_tax",
    "parent.zh_name AS parent_zhname")
    End Function

End Class
