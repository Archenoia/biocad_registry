Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel
Imports std = System.Math

Public Module ImportsMetabolite

    <Extension>
    Public Sub SaveStructureData(registry As biocad_registry,
                                 m As metabolites,
                                 smiles As String,
                                 Optional commit As CommitTransaction = Nothing)

        If Not smiles.StringEmpty Then
            Dim struct = registry.struct_data.where(field("metabolite_id") = m.id).find(Of struct_data)

            If struct Is Nothing Then
                Dim struct_data = {
                    field("metabolite_id") = m.id,
                    field("checksum") = smiles.MD5,
                    field("smiles") = smiles
                }

                If commit Is Nothing Then
                    registry.struct_data.add(struct_data)
                Else
                    Call commit.add(registry.struct_data.ignore.add_sql(struct_data))
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' save class information into ontology table
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="m"></param>
    ''' <param name="ontology_id"></param>
    ''' <param name="ont"></param>
    ''' <param name="source_id"></param>
    <Extension>
    Public Sub SaveMetaboliteClass(registry As biocad_registry,
                                   m As metabolites,
                                   ontology_id As UInteger,
                                   ont As (kingdom$, super_class$, class$, sub_class$),
                                   source_id As String)
        Dim kingdom As ontology
        Dim super_class As ontology
        Dim [class] As ontology
        Dim sub_class As ontology
        Dim node As ontology = Nothing

        If Not ont.kingdom.StringEmpty Then
            kingdom = registry.ontology _
                .where(field("ontology_id") = ontology_id,
                       field("term_id") = ont.kingdom) _
                .find(Of ontology)

            If kingdom Is Nothing Then
                registry.ontology.add(field("ontology_id") = ontology_id,
                                      field("term_id") = ont.kingdom,
                                      field("term") = ont.kingdom)
                kingdom = registry.ontology _
                    .where(field("ontology_id") = ontology_id,
                           field("term_id") = ont.kingdom) _
                    .find(Of ontology)
            End If

            If kingdom IsNot Nothing Then node = kingdom

            If kingdom IsNot Nothing AndAlso Not ont.super_class.StringEmpty Then
                super_class = registry.ontology _
                    .where(field("ontology_id") = ontology_id,
                           field("term_id") = ont.super_class) _
                    .find(Of ontology)

                If super_class Is Nothing Then
                    registry.ontology.add(field("ontology_id") = ontology_id,
                                          field("term_id") = ont.super_class,
                                          field("term") = ont.super_class)
                    super_class = registry.ontology _
                        .where(field("ontology_id") = ontology_id,
                               field("term_id") = ont.super_class) _
                        .find(Of ontology)
                    registry.ontology_relation.add(field("term_id") = super_class.id,
                                                   field("is_a") = kingdom.id)
                End If

                If super_class IsNot Nothing Then node = super_class

                If super_class IsNot Nothing AndAlso Not ont.class.StringEmpty Then
                    [class] = registry.ontology _
                        .where(field("ontology_id") = ontology_id,
                               field("term_id") = ont.class) _
                        .find(Of ontology)

                    If [class] Is Nothing Then
                        registry.ontology.add(field("ontology_id") = ontology_id,
                                              field("term_id") = ont.class,
                                              field("term") = ont.class)
                        [class] = registry.ontology _
                            .where(field("ontology_id") = ontology_id,
                                   field("term_id") = ont.class) _
                            .find(Of ontology)
                        registry.ontology_relation.add(field("term_id") = [class].id,
                                                       field("is_a") = super_class.id)
                    End If

                    If [class] IsNot Nothing Then node = [class]

                    If [class] IsNot Nothing AndAlso Not ont.sub_class.StringEmpty Then
                        sub_class = registry.ontology _
                            .where(field("ontology_id") = ontology_id,
                                   field("term_id") = ont.sub_class) _
                            .find(Of ontology)

                        If sub_class Is Nothing Then
                            registry.ontology.add(field("ontology_id") = ontology_id,
                                                  field("term_id") = ont.sub_class,
                                                  field("term") = ont.sub_class)
                            sub_class = registry.ontology _
                                .where(field("ontology_id") = ontology_id,
                                       field("term_id") = ont.sub_class) _
                                .find(Of ontology)
                            registry.ontology_relation.add(field("term_id") = sub_class.id,
                                                           field("is_a") = [class].id)
                        End If

                        node = sub_class
                    End If
                End If
            End If
        End If

        If node IsNot Nothing Then
            Call registry.metabolite_class _
                .ignore _
                .add(field("metabolite_id") = m.id,
                     field("class_id") = node.id,
                     field("note") = source_id)
        End If
    End Sub

    <Extension>
    Public Sub SaveSynonyms(registry As biocad_registry,
                            m As metabolites,
                            synonyms As IEnumerable(Of String),
                            db_source As UInteger, Optional trans As CommitTransaction = Nothing)

        Call registry.SaveSynonyms(
            obj_id:=m.id,
            synonyms:=synonyms,
            db_source:=db_source,
            class_id:=registry.biocad_vocabulary.metabolite_type,
            commit:=trans
        )
    End Sub

    <Extension>
    Private Function PickName(m As metabolites, registry As biocad_registry) As String
        If Not m.biocyc.StringEmpty Then
            Return m.biocyc
        ElseIf Not m.mesh_id.StringEmpty Then
            Return m.mesh_id
        ElseIf Not m.wikipedia.StringEmpty Then
            Return m.wikipedia
        ElseIf Not m.cas_id.StringEmpty Then
            Return m.cas_id
        Else
            Dim np1 = registry.biocad_vocabulary.GetDatabaseResource("Coconut NaturalProduct")
            Dim xref = registry.db_xrefs _
                .where(field("obj_id") = m.id,
                       field("type") = registry.biocad_vocabulary.metabolite_type,
                       field("db_name") = np1.id) _
                .find(Of db_xrefs)

            If xref IsNot Nothing Then
                Return xref.db_xref
            Else
                Return $"Metabolite_{m.id}"
            End If
        End If
    End Function

    Private Sub UpdateHashCode(registry As biocad_registry, Optional page_size As Integer = 500000)
        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim page_data = registry.metabolites.limit(offset, page_size).select(Of metabolites)()
            Dim updates As CommitTransaction = registry.metabolites.open_transaction

            If page_data.IsNullOrEmpty Then
                Exit For
            Else
                Call $"update hashcode for metabolite data page {page}...".debug
            End If

            For Each name As metabolites In TqdmWrapper.Wrap(page_data)
                Dim update_name As Boolean = False

                If name.name.StringEmpty(, True) OrElse name.name.IsPattern("Metabolite_\d+") Then
                    ' Call $"processing empty name of metabolite {name.id} '{name.name}'".debug
                    update_name = True
                    name.name = name.PickName(registry)
                    ' invalid update logic for
                    ' XXXX-HETE
                    ' ATP
                    ' XXXXX-UMP
                    'ElseIf name.name.IsUpperName AndAlso name.name.Length > 9 Then
                    '    update_name = True
                    '    name.name = name.name.ToLower
                End If

                name.name = RegisterSymbol.CleanName(name.name)

                Dim hashcode As String = Strings.LCase(name.name).MD5

                ' name value has been changed
                If update_name OrElse hashcode <> name.hashcode Then
                    Call updates.add(registry.metabolites _
                        .where(field("id") = name.id) _
                        .save_sql(field("hashcode") = hashcode, field("name") = name.name))
                End If
            Next

            Call $"commit the name hashcode with {updates.size} record updates!".debug
            Call updates.commit()
        Next
    End Sub

    <Extension>
    Private Function IsUpperName(name As String) As Boolean
        If name.StringEmpty Then
            Return False
        Else
            Return Not name.Any(Function(c) Char.IsLetter(c) AndAlso Char.IsLower(c))
        End If
    End Function

    <Extension>
    Private Sub ResolveMetaboliteAlias(metas As metabolites(), registry As biocad_registry, trans As CommitTransaction)
        Dim top_main As metabolites = metas.OrderBy(Function(a) a.id).First
        Dim struct = registry.struct_data.where(field("metabolite_id") = top_main.id).find(Of struct_data)

        For Each meta As metabolites In metas
            If std.Abs(meta.exact_mass - top_main.exact_mass) > 0.1 Then
                Continue For
            End If

            If meta.id = top_main.id Then
                Call trans.add(registry.metabolites _
                               .where(field("id") = top_main.id) _
                               .save_sql(field("main_id") = 0))
            Else
                Dim updates As New List(Of FieldAssert)(FieldUpdates(top_main:=top_main, meta:=meta))

                Call trans.add(registry.metabolites _
                    .where(field("id") = meta.id) _
                    .save_sql(field("main_id") = top_main.id))

                If updates.Any Then
                    Call trans.add(registry.metabolites _
                        .where(field("id") = top_main.id) _
                        .save_sql(updates.ToArray))
                End If

                If struct Is Nothing Then
                    Dim smiles_str = registry.struct_data _
                        .where(field("metabolite_id") = meta.id) _
                        .find(Of struct_data)

                    If Not smiles_str Is Nothing Then
                        Call registry.struct_data.add(
                            field("metabolite_id") = top_main.id,
                            field("checksum") = smiles_str.checksum,
                            field("smiles") = smiles_str.smiles
                        )

                        struct = smiles_str
                    End If
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Update empty main data from the alias meta data
    ''' </summary>
    ''' <param name="top_main"></param>
    ''' <param name="meta"></param>
    ''' <returns></returns>
    Private Iterator Function FieldUpdates(top_main As metabolites, meta As metabolites) As IEnumerable(Of FieldAssert)
        If top_main.cas_id.StringEmpty AndAlso Not meta.cas_id.StringEmpty Then
            top_main.cas_id = meta.cas_id

            Yield (field(NameOf(metabolites.cas_id)) = meta.cas_id)
        End If
        If top_main.kegg_id.StringEmpty AndAlso Not meta.kegg_id.StringEmpty Then
            top_main.kegg_id = meta.kegg_id

            Yield (field(NameOf(metabolites.kegg_id)) = meta.kegg_id)
        End If
        If top_main.hmdb_id.StringEmpty AndAlso Not meta.hmdb_id.StringEmpty Then
            top_main.hmdb_id = meta.hmdb_id

            Yield (field(NameOf(metabolites.hmdb_id)) = meta.hmdb_id)
        End If
        If top_main.lipidmaps_id.StringEmpty AndAlso Not meta.lipidmaps_id.StringEmpty Then
            top_main.lipidmaps_id = meta.lipidmaps_id

            Yield (field(NameOf(metabolites.lipidmaps_id)) = meta.lipidmaps_id)
        End If
        If top_main.drugbank_id.StringEmpty AndAlso Not meta.drugbank_id.StringEmpty Then
            top_main.drugbank_id = meta.drugbank_id

            Yield (field(NameOf(metabolites.drugbank_id)) = meta.drugbank_id)
        End If
        If top_main.mesh_id.StringEmpty AndAlso Not meta.mesh_id.StringEmpty Then
            top_main.mesh_id = meta.mesh_id

            Yield (field(NameOf(metabolites.mesh_id)) = meta.mesh_id)
        End If
        If top_main.biocyc.StringEmpty AndAlso Not meta.biocyc.StringEmpty Then
            top_main.biocyc = meta.biocyc

            Yield (field(NameOf(metabolites.biocyc)) = meta.biocyc)
        End If
        If top_main.wikipedia.StringEmpty AndAlso Not meta.wikipedia.StringEmpty Then
            top_main.wikipedia = meta.wikipedia

            Yield (field(NameOf(metabolites.wikipedia)) = meta.wikipedia)
        End If
        If top_main.chebi_id = 0 AndAlso meta.chebi_id > 0 Then
            top_main.chebi_id = meta.chebi_id

            Yield (field(NameOf(metabolites.chebi_id)) = meta.chebi_id)
        End If
        If top_main.pubchem_cid = 0 AndAlso meta.pubchem_cid > 0 Then
            top_main.pubchem_cid = meta.pubchem_cid

            Yield (field(NameOf(metabolites.pubchem_cid)) = meta.pubchem_cid)
        End If
        If top_main.note.StringEmpty AndAlso Not meta.note.StringEmpty Then
            top_main.note = meta.note

            Yield (field(NameOf(metabolites.note)) = meta.note)
        End If
    End Function

    <Extension>
    Public Sub MetaboliteLinks(registry As biocad_registry, Optional check_hash As Boolean = False)
        Dim trans As CommitTransaction = registry.metabolites.open_transaction

        If check_hash Then
            Call UpdateHashCode(registry)
        End If

        For Each hashcode As String In TqdmWrapper.Wrap(registry.metabolites _
                                                        .group_by("hashcode") _
                                                        .having(field("*").count > 1) _
                                                        .project(Of String)("hashcode"))
            Call registry.metabolites _
                .where(field("hashcode") = hashcode) _
                .select(Of metabolites) _
                .ResolveMetaboliteAlias(registry, trans)
        Next

        Call trans.commit()
    End Sub
End Module
