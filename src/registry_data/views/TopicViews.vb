Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel

Public Module TopicViews

    <Extension>
    Public Sub PlantNP(registry As biocad_registry)
        Dim np_topic As UInteger = registry.biocad_vocabulary.GetTopic("Plant Natural Products")
        ' Viridiplantae
        Dim plant_tax As UInteger = 33090

        ' Call registry.NaturalProductLib(np_topic, plant_tax)
        ' andalso add pmhub and herb database content as plant np data?
        Dim herb As UInteger = registry.biocad_vocabulary.GetDatabaseResource("HERB")
        Dim metab_type As UInteger = registry.biocad_vocabulary.metabolite_type
        Dim nplink As CommitTransaction = registry.topic.open_transaction

        For Each meta_key In TqdmWrapper.Wrap(registry.db_xrefs.where(field("type") = metab_type, field("db_name") = herb).select(Of db_xrefs))
            Dim meta As metabolites = registry.metabolites _
                        .where(field("`metabolites`.id") = meta_key.obj_id) _
                        .find(Of metabolites)

            If meta Is Nothing Then
                Continue For
            End If

            Dim meta_id As UInteger = meta.id

            If meta.main_id > 0 Then
                meta_id = meta.main_id
            End If

            Dim check_topic = registry.topic _
                .where(field("topic_id") = np_topic,
                       field("type") = metab_type,
                       field("model_id") = meta_id) _
                .find(Of biocad_registryModel.topic)

            If check_topic Is Nothing Then
                Call nplink.ignore.add(
                    field("topic_id") = np_topic,
                    field("model_id") = meta_id,
                    field("type") = metab_type,
                    field("note") = meta_key.db_xref
                )
            End If
        Next

        Call nplink.commit()
    End Sub

    Public Sub MarineNPs(registry As biocad_registry)
        Dim np_topic As UInteger = registry.biocad_vocabulary.GetTopic("Marine Natural Products")

        Dim Porifera_tax As UInteger = 6040
        ' Dim Proteobacteria As UInteger =
        ' Dim Firmicutes As  UInteger =
        ' Dim Euryarchaeota As UInteger =
        Dim Bacillariophyta As UInteger = 2836
        Dim Dinophyta As UInteger = 2864
        Dim Haptophyta As UInteger = 2830
        Dim Skeletonema As UInteger = 2842
        Dim Thalassiosira As UInteger = 35127
        Dim Dinophysis As UInteger = 47933
        ' Lyngbya majuscula
        Dim Lyngbya_majuscula As UInteger = 158786
        Dim Cnidaria As UInteger = 6073
        ' Dim Urochordata As UInteger =
        Dim Mollusca As UInteger = 6447
        Dim Echinodermata As UInteger = 7586
        Dim Bryozoa As UInteger = 10205

        Call registry.NaturalProductLib(np_topic, Porifera_tax, Bacillariophyta, Dinophyta, Haptophyta, Skeletonema, Thalassiosira, Dinophysis, Lyngbya_majuscula, Cnidaria, Mollusca, Echinodermata, Bryozoa)
    End Sub

    <Extension>
    Public Sub MicrobialNP(registry As biocad_registry)
        Dim np_topic As UInteger = registry.biocad_vocabulary.GetTopic("Microbial Natural Products")
        ' bacterials
        Dim bacterial_tax As UInteger = 2
        ' fungi
        Dim fungi As UInteger = 4751
        ' archaea
        Dim archaea As UInteger = 2157

        Call registry.NaturalProductLib(np_topic, bacterial_tax, fungi, archaea)
    End Sub

    <Extension>
    Private Sub NaturalProductLib(registry As biocad_registry, np_topic As UInteger, ParamArray root_tax As UInteger())
        Dim page_size As Integer = 5000
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type

        For page As Integer = 1 To Integer.MaxValue
            Dim page_data = registry.organism_source _
                .where(field("organism_id") > 0) _
                .limit((page - 1) * page_size, page_size) _
                .select(Of biocad_registryModel.organism_source)

            If page_data.IsNullOrEmpty Then
                Exit For
            End If

            For Each link As biocad_registryModel.organism_source In TqdmWrapper.Wrap(page_data)
                If root_tax.Any(Function(taxid) registry.CheckLineage(link.organism_id, taxid)) Then
                    Dim meta As metabolites = registry.metabolites _
                        .where(field("id") = link.metabolite_id) _
                        .find(Of metabolites)

                    If meta Is Nothing Then
                        Continue For
                    End If

                    Dim meta_id As UInteger = meta.id

                    If meta.main_id > 0 Then
                        meta_id = meta.main_id
                    End If

                    'Dim meta As registry_resolver = registry.SymbolRegister(m)

                    'If meta Is Nothing Then
                    '    ' name is too long, insert error
                    '    Continue For
                    'End If

                    Dim check_topic = registry.topic _
                        .where(field("topic_id") = np_topic,
                               field("type") = metabolite_type,
                               field("model_id") = meta_id) _
                        .find(Of biocad_registryModel.topic)

                    If check_topic Is Nothing Then
                        Call registry.topic.add(
                            field("topic_id") = np_topic,
                            field("model_id") = meta_id,
                            field("type") = metabolite_type,
                            field("note") = link.note
                        )
                    End If
                End If
            Next
        Next
    End Sub
End Module
