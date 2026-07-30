Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module Translations

    <Extension>
    Public Async Function TranslateOntology(registry As biocad_registry, ontology As String) As Task
        Dim ontology_id As UInteger = registry.biocad_vocabulary.GetDatabaseResource(ontology).id
        Dim terms = registry.ontology _
            .where(field("ontology_id") = ontology_id,
                  (field("term_zh").char_length = 0) Or field("term_zh").is_nothing) _
            .select(Of biocad_registryModel.ontology)

        For Each term As biocad_registryModel.ontology In TqdmWrapper.Wrap(terms)
            Dim prompt As String = $"将下面的这个化合物分类词条名称翻译为中文：'{term.term}'，如果没有正式的翻译，请进行音译。使用下面的json格式返回结果给我以方便我进行数据解析：{{""zh_name"": ""translated_name""}}"
            Dim zh_name As String = TranslatedName.DecodeLLMTranslateOutput(Await LLMs.LLMsTalk(prompt))

            If Not zh_name.StringEmpty(, True) Then
                Call registry.ontology _
                    .where(field("id") = term.id) _
                    .save(field("term_zh") = zh_name)
            End If
        Next
    End Function

    <Extension>
    Public Async Function TranslateMetaboliteName(registry As biocad_registry, main_db As String) As Task
        Dim page_size As Integer = 1000
        Dim metabo_class As UInteger = registry.biocad_vocabulary.metabolite_type
        Dim llms_source As UInteger = registry.biocad_vocabulary.db_LLMs

        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim pagedata = registry.metabolites _
                .where(field(main_db).char_length > 0) _
                .limit(offset, page_size) _
                .select(Of biocad_registryModel.metabolites)

            If pagedata.IsNullOrEmpty Then
                Exit For
            End If

            For Each metab As biocad_registryModel.metabolites In TqdmWrapper.Wrap(pagedata)
                Dim zh_name = registry.synonym _
                    .where(field("type") = metabo_class,
                           field("obj_id") = metab.id,
                           field("lang") = "zh") _
                    .find(Of biocad_registryModel.synonym)

                If zh_name Is Nothing Then
                    Dim prompt As String = $"将下面的这个化合物名称翻译为中文：'{metab.name}'，如果没有正式的翻译，请进行音译。使用下面的json格式返回结果给我以方便我进行数据解析：{{""zh_name"": ""translated_name""}}"
                    Dim zh As String = TranslatedName.DecodeLLMTranslateOutput(Await LLMs.LLMsTalk(prompt))

                    If Not zh.StringEmpty(, True) Then
                        Call registry.synonym.add(
                            field("obj_id") = metab.id,
                            field("type") = metabo_class,
                            field("lang") = "zh",
                            field("db_source") = llms_source,
                            field("synonym") = zh,
                            field("hashcode") = zh.ToLower.MD5)
                    End If
                End If
            Next
        Next
    End Function

End Module
